using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Выкладывание комплекта в папку.
	/// </summary>
	internal class FolderDeploy : DeployType
	{
		/// <summary>
		/// Контрол с указанием пути к папке.
		/// </summary>
		private readonly TextBox FolderPath;

		/// <summary>
		/// Кнопка для выбора папки.
		/// </summary>
		private readonly Button BtnBrowse;

		/// <summary>
		/// Выкладывание комплекта в папку.
		/// </summary>
		/// <param name="switcher"></param>
		/// <param name="folderPath">Контрол с указанием пути к папке.</param>
		/// <param name="btnBrowse">Кнопка для выбора папки.</param>
		/// <param name="deployPackageDestinationForm"></param>
		public FolderDeploy(RadioButton switcher, TextBox folderPath, Button btnBrowse, DeployPackageDestinationForm deployPackageDestinationForm)
			: base(switcher, new Control[] { folderPath, btnBrowse }, deployPackageDestinationForm)
		{
			this.FolderPath = folderPath ?? throw new ArgumentNullException(nameof(folderPath));
			this.BtnBrowse = btnBrowse ?? throw new ArgumentNullException(nameof(btnBrowse));
		}

		/// <summary>
		/// Выполняет деплой модуля.
		/// </summary>
		/// <param name="deployInfo"></param>
		internal override bool Deploy(DeployInfo deployInfo)
		{
			if (deployInfo == null)
				throw new ArgumentNullException(nameof(deployInfo));

			//получаем указанную папку
			string selectedPath = this.FolderPath.Text;
			if (String.IsNullOrEmpty(selectedPath))
				throw new NotificationException("Не выбран путь для комплекта");

			DirectoryInfo targetDir;
			if (!Directory.Exists(selectedPath))
			{
				if (WSSDeveloperPackage.ShowUserConfirmOkCancel("Папка " + selectedPath + " отсутствует.\nСоздать?", "Папка отсутствует"))
					targetDir = Directory.CreateDirectory(selectedPath);
				else
					return false;
			}
			else
				targetDir = new DirectoryInfo(selectedPath);

			//деплоим в указанную папку
			deployInfo.CreateReleaseDirectory(targetDir, false);
			deployInfo.CreateSourcesDirectory(targetDir, false);

			this.Form.Command.Package.WriteToOutput(
				$"Сформирован комплект для проекта {deployInfo.Project.Name} по пути:\n<file://{targetDir.FullName.TrimStart('\\').Replace('\\', '/')}>");
			return true;
		}
	}
}
