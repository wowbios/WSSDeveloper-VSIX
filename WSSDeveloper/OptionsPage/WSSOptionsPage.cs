using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace WSSConsulting.WSSDeveloper
{
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[CLSCompliant(false), ComVisible(true)]
	[Guid("882BC36F-3894-44A9-8612-4321E101EEC1")]
	public class WSSOptionsPage : DialogPage
	{
		/// <summary>
		/// Путь к папке с комплектами.
		/// </summary>
		public string PackageFolderPath { get; set; }

		/// <summary>
		/// Путь к папкам тестирования.
		/// </summary>
		public string TestFolderPath { get; set; }

		/// <summary>
		/// Имя разработчика.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Замены для проектов для шаблона тестирования.
		/// </summary>
		public Dictionary<string, string> TestTemplateShortcuts { get; set; }

		public string _ShortCuts_Storage { get; set; }

		/// <summary>
		/// Gets the window that is used as the user interface of the dialog page.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Windows.Forms.IWin32Window"/> that provides the handle to the window that acts as the user interface for the dialog page.
		/// </returns>
		protected override IWin32Window Window
		{
			get
			{
				return new WSSOptionsPageControl(this);
			}
		}

		public override void LoadSettingsFromStorage()
		{
			base.LoadSettingsFromStorage();
			this.TestTemplateShortcuts = new Dictionary<string, string>();

			this.TestTemplateShortcuts = this._ShortCuts_Storage?.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Split('\t'))
				.ToDictionary(x => x[0], x => x[1]) ?? new Dictionary<string, string>();
		}

		public override void SaveSettingsToStorage()
		{
			this._ShortCuts_Storage = String.Join("\n", this.TestTemplateShortcuts?.Select(x => x.Key + "\t" + x.Value) ?? new string[0]);
			base.SaveSettingsToStorage();
		}
	}
}
