using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using WSSConsulting.WSSDeveloper.Commands;
using Microsoft.VisualStudio.Threading;
using Microsoft;

namespace WSSConsulting.WSSDeveloper
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	///
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the 
	/// IVsPackage interface and uses the registration attributes defined in the framework to 
	/// register itself and its components with the shell.
	/// </summary>
	// This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
	// a package.
	[PackageRegistration(UseManagedResourcesOnly = true)]
	// This attribute is used to register the information needed to show this package
	// in the Help/About dialog of Visual Studio.
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[Guid(GuidList.guidWSSDeveloperPkgString)]
	[ProvideBindingPath]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[ProvideOptionPage(typeof(WSSOptionsPage), "WSSDeveloper", "Настройка", 0, 0, true)]
	[ProvideAutoLoad(UIContextGuids80.DesignMode)]
	[ProvideAutoLoad(UIContextGuids80.EmptySolution)]
	[ProvideAutoLoad(UIContextGuids80.NoSolution)]
	[ProvideAutoLoad(UIContextGuids80.SolutionExists)]
	public sealed partial class WSSDeveloperPackage : Package
	{
		/// <summary>
		/// Default constructor of the package.
		/// Inside this method you can place any initialization code that does not require 
		/// any Visual Studio service because at this point the package object is created but 
		/// not sited yet inside Visual Studio environment. The place to do all the other 
		/// initialization is the Initialize method.
		/// </summary>
		public WSSDeveloperPackage()
		{
			Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
		}

		private bool __init_DTE;
		private DTE _DTE;
		/// <summary>
		/// DTE - корневой объект для автоматизации студии.
		/// </summary>
		internal DTE DTE
		{
			get
			{
				if (!__init_DTE)
				{
					_DTE = (DTE)this.GetService(typeof(DTE));
					__init_DTE = true;
				}
				return _DTE;
			}
		}

		private bool __init_OutputPane;
		private IVsOutputWindowPane _OutputPane;
		/// <summary>
		/// Output панель.
		/// </summary>
		private IVsOutputWindowPane OutputPane
		{
			get
			{
				if (!__init_OutputPane)
				{
					_OutputPane = this.GetOutputPane(VSConstants.OutputWindowPaneGuid.GeneralPane_guid, "Output");
					__init_OutputPane = true;
				}
				return _OutputPane;
			}
		}

		private bool __init_Options;
		private WSSOptions _Options;
		/// <summary>
		/// Настройки.
		/// </summary>
		internal WSSOptions Options
		{
			get
			{
				if (!__init_Options)
				{
					_Options = new WSSOptions((WSSOptionsPage)this.GetDialogPage(typeof(WSSOptionsPage)));
					__init_Options = true;
				}
				return _Options;
			}
		}

		/// <summary>
		/// Записывает сообщение в Output.
		/// </summary>
		/// <param name="message"></param>
		internal void WriteToOutput(string message)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			this.OutputPane.OutputString($"\nWSSDev [{DateTime.Now:HH:mm:ss}]: {message}");

			this.OutputPane.Activate();

			//показываем окно
			this.DTE.ExecuteCommand("View.Output", String.Empty);
		}

		/*
        internal void WriteError()
        {
            //записывает ошибку в список ошибок (например, как ошибки при билде)
            ErrorListProvider errors = new ErrorListProvider(this);
            ErrorTask error = new ErrorTask();
            errors.Tasks.Add(error);
        }*/

		/// <summary>
		/// Initialization of the package; this method is called right after the package is sited, so this is the place
		/// where you can put all the initialization code that rely on services provided by VisualStudio.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			ThreadHelper.ThrowIfNotOnUIThread();

			// Add our command handlers for menu (commands must exist in the .vsct file)
			if (this.GetService(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
			{
				this.AddCommand<DeployManifestManager>(mcs, (int)CommandsList.AddToDeploy);
				this.AddCommand<DeployPackageManager>(mcs, (int)CommandsList.Deploy);
				this.AddCommand<MakeReleaseCommand>(mcs, (int)CommandsList.MakeRelease);
				this.AddCommand<AnalyzeReferencesCommand>(mcs, (int)CommandsList.RefCheck);
				this.AddCommand<AddToTypesXmlCommand>(mcs, (int)CommandsList.AddToTypesXml);
			}
		}

		/// <summary>
		/// Добавляет команду.
		/// </summary>
		/// <typeparam name="T">Логика команды.</typeparam>
		/// <param name="mcs">Сервис команд.</param>
		/// <param name="commandCode">Код команды.</param>
		private void AddCommand<T>(OleMenuCommandService mcs, int commandCode)
			where T : WSSCommandBase, new()
		{
			if (mcs == null)
				throw new ArgumentNullException(nameof(mcs));

			CommandID commandID = new CommandID(GuidList.WSSCommandsGuid, commandCode);
			MenuCommand command = new MenuCommand(eventHandler, commandID);
			mcs.AddCommand(command);

			// Обработчик команды
			void eventHandler(object s, EventArgs e)
			{
				try
				{
					ThreadHelper.ThrowIfNotOnUIThread();
					this.OutputPane.Clear();

					T commandHandler = new T();
					commandHandler.Init(this);
					commandHandler.Execute();
				}
				catch (NotificationException nex)
				{
					this.ShowError(nex.Message, "Ошибка выполнения команды", OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGICON.OLEMSGICON_WARNING);
				}
				catch (Exception ex)
				{
					this.ShowError(ex.ToString(), "Критическая ошибка выполнения команды", OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGICON.OLEMSGICON_CRITICAL);
				}
			}
		}

		/// <summary>
		/// Показывает ошибку пользователю.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <param name="icon"></param>
		private void ShowError(string text, string caption, OLEMSGBUTTON buttons, OLEMSGICON icon)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			this.OutputPane.Clear();
			this.WriteToOutput(text);

			IVsUIShell uiShell = (IVsUIShell)this.GetService(typeof(SVsUIShell));
			Guid clsid = Guid.Empty;
			int result;
			ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(
					   0,
					   ref clsid,
					   caption,
					   text,
					   string.Empty,
					   0,
					   buttons,
					   OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
					   icon,
					   0,        // false
					   out result));
		}
	}
}
