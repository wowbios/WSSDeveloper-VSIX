using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Форма выбора деплоя комплекта.
	/// </summary>
	internal partial class DeployPackageDestinationForm : Form
	{
		/// <summary>
		/// Команда.
		/// </summary>
		internal WSSCommandBase Command { get; }

		/// <summary>
		/// Вида деплоя.
		/// </summary>
		private readonly DeployType[] DeployTypes;

		/// <summary>
		/// Форма выбора деплоя комплекта.
		/// </summary>
		/// <param name="command">Команда.</param>
		public DeployPackageDestinationForm(WSSCommandBase command)
		{
			this.Command = command ?? throw new ArgumentNullException(nameof(command));
			this.InitializeComponent();

			//создаём типы деплоя
			this.DeployTypes = new DeployType[]
			{
				new PackageDeploy(rb_Package, combo_Package, this),
				new TestDeploy(rb_Test, combo_Test, comboBox_TestFolder, this),
				new FolderDeploy(rb_Other, tb_Folder, btn_BrowseFolder, this)
			};

			//активируем первый - по умолчанию
			this.DeployTypes[0].IsActive = true;

			//запускаем инициализацию
			foreach (DeployType deployType in this.DeployTypes) deployType.OnInit();

			//НГ 2019
			if (DateTime.Now.Year < 2019 && DateTime.Now.Month == 12)
			{
				this.NewYear2019();
			}
		}

		private void NewYear2019()
		{
			var timer = new Timer
			{
				Interval = 10000
			};

			timer.Tick += (s, e) => SetNewYearText();
			SetNewYearText();

			timer.Start();

			this.FormClosed += (s, e) => { timer.Stop(); timer.Dispose(); };

			this.NewYear2019Label.Visible = true;

			void SetNewYearText()
			{
				TimeSpan diff = new DateTime(2019, 1, 1, 0, 0, 0) - DateTime.Now;
				this.NewYear2019Label.Text = $"До Нового Года осталось {(int)diff.TotalHours} ч";
			}
		}

		/// <summary>
		/// Возвращает выбранный деплой.
		/// </summary>
		internal DeployType ActiveDeploy => this.DeployTypes.Single(x => x.IsActive);

		/// <summary>
		/// Нажатие "ОК".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btn_OK_Click(object sender, EventArgs e)
		{
			try
			{
				DeployInfo deployInfo = new DeployInfo(this.Command.DTEInfo.SelectedProject);
				if (this.ActiveDeploy.Deploy(deployInfo))
				{
					this.CloseForm(DialogResult.OK);
				}
			}
			catch (NotificationException nex)
			{
				WSSDeveloperPackage.ShowUserWarn(nex.Message, "Ошибка");
				this.CloseForm(DialogResult.Cancel);
			}
			catch (Exception ex)
			{
				WSSDeveloperPackage.ShowUserError(ex.ToString(), "Критическая ошибка");
				this.CloseForm(DialogResult.Cancel);
			}
		}

		/// <summary>
		/// Нажатие "Отмена".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btn_Cancel_Click(object sender, EventArgs e)
		{
			this.CloseForm(DialogResult.Cancel);
		}

		/// <summary>
		/// Закрывает форму с результатом <paramref name="result"/>.
		/// </summary>
		/// <param name="result"></param>
		private void CloseForm(DialogResult result)
		{
			this.DialogResult = result;
			this.Close();
		}

		/// <summary>
		/// Нажатие "Обзор...".
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btn_BrowseFolder_Click(object sender, EventArgs e)
		{
			using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog())
			{
				fileDialog.IsFolderPicker = true;

				if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
				{
					tb_Folder.Text = fileDialog.FileName;
				}
			}
		}

		private void DccGfccdjhl2(object sender, EventArgs e)
		{
			try
			{
				throw new UnauthorizedAccessException(@"
11010001
10000001
11010000
10110000
11010001
10001001
11010001
10000011
11010001
10000000
11010001
10000001
11010001
10000011
11010000
10111101
11010000
10111101
11010000
10111110
00100000
11010001
10000101
00100000
11010000
10111111
11010000
10110000
11010001
10000011
11010000
10111010
11010000
10111100
00100000
11010001
10001100
11010000
10110000
11010001
10010001
11010001
10001101
11010001
10001101
00100001
00100000
11010001
10000000
11010000
10110110
11010001
10001111
11010000
10111101
11010000
10110111
11010000
10110101
00100000
11010001
10000010
11010001
10000001
11010000
10110101
11010001
10001101
11010001
10000010
00101100
00100000
11010000
10110101
11010001
10000000
11010001
10001101
11010000
10111101
11010000
10110111
11010000
10110101
00100000
11010001
10010001
11010001
10001010
11010001
10001001
00100001
");
			}
			catch (Exception ex)
			{
				WSSDeveloperPackage.ShowUserWarn(ex.ToString(), "С Нов–•”љ™љ№№##///");
			}
		}
	}
}