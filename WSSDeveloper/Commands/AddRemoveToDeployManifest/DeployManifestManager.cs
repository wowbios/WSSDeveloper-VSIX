using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Менеджер для добавления файлов в Deploy\Manifest.
	/// </summary>
	internal class DeployManifestManager : WSSCommandBase
	{
		private bool __init_SelectedItems;
		private List<ProjectItem> _SelectedItems;
		/// <summary>
		/// Выбранные элементы в проекте.
		/// </summary>
		private List<ProjectItem> SelectedItems
		{
			get
			{
				if (!__init_SelectedItems)
				{
					if (this.DTEInfo.ItemsFromMultipleProjects)
						throw new NotificationException("Выбраные файлы из разных проектов");

					_SelectedItems = this.DTEInfo.SelectedItems.OrderBy(x => x.Name).ToList();
					__init_SelectedItems = true;
				}
				return _SelectedItems;
			}
		}

		/// <summary>
		/// Срабатывает при выполнении команды.
		/// </summary>
		protected override void OnExecute()
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			if (this.SelectedItems.Count == 0)
				return;

			//получаем папку Deploy
			ProjectItem deployFolder = this.DTEInfo.SelectedProject.GetChildItem(Constants.DeployFilesFolderName, true);
			if (deployFolder.ProjectItems == null || deployFolder.ProjectItems.Count == 0)
				throw new NotificationException("В папке \"{0}\" отсутствуют файлы", Constants.DeployFilesFolderName);

			//DeployParams.txt
			ProjectItem deployParamsItem = this.GetDeployFile(deployFolder, Constants.DeployParamsFileName);

			//Manifest.xml
			ProjectItem manifestItem = this.GetDeployFile(deployFolder, Constants.ManifestFileName);

			//Записываем в DeployParams.txt
			DeployParamsParser deployParams = new DeployParamsParser(deployParamsItem);

			//Записываем в Manifest.xml
			ManifestParser manifest = new ManifestParser(manifestItem);

			manifest.AddFiles(deployParams.AddFiles(this.SelectedItems));

			this.DTE.ItemOperations.OpenFile(deployParams.ItemFullPath, EnvDTE.Constants.vsViewKindTextView);
			this.DTE.ItemOperations.OpenFile(manifest.ItemFullPath, EnvDTE.Constants.vsViewKindTextView);
			manifest.Item.Save();
			deployParams.Item.Save();
		}

		/// <summary>
		/// Сохраняет изменения в файле и возвращает его в качестве файла для добавления в Deploy\Manifest.
		/// </summary>
		/// <param name="deployFolder"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private ProjectItem GetDeployFile(ProjectItem deployFolder, string fileName)
		{
			if (deployFolder == null)
				throw new ArgumentNullException(nameof(deployFolder));
			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			ProjectItem item = deployFolder.GetChildItem(fileName, true);

			this.DTE.CheckOutItem(item);
			if (item.IsOpen)
			{
				item.Save();
				item.Document?.Close(vsSaveChanges.vsSaveChangesYes);
			}

			return item;
		}
	}
}
