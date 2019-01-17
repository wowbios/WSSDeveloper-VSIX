using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Команда копирования в Release.
	/// </summary>
	internal class MakeReleaseCommand : WSSCommandBase
	{
		private bool __init_ModuleFileName;
		private string _ModuleFileName;
		/// <summary>
		/// Название файла модуля.
		/// </summary>
		private string ModuleFileName
		{
			get
			{
				if (!__init_ModuleFileName)
				{
					HashSet<char> invalidFilenameChars = new HashSet<char>(Path.GetInvalidFileNameChars());
					_ModuleFileName = new string(this.DTEInfo.SelectedProject.Name.Where(c => !invalidFilenameChars.Contains(c)).ToArray());
					__init_ModuleFileName = true;
				}
				return _ModuleFileName;
			}
		}

		private bool __init_WSPFileName;
		private string _WSPFileName;
		/// <summary>
		/// Название WSP файла модуля.
		/// </summary>
		private string WSPFileName
		{
			get
			{
				if (!__init_WSPFileName)
				{
					_WSPFileName = this.ModuleFileName + ".wsp";
					__init_WSPFileName = true;
				}
				return _WSPFileName;
			}
		}

		private bool __init_Release;
		private ProjectItem _Release;
		/// <summary>
		/// Папка /Release.
		/// </summary>
		private ProjectItem Release
		{
			get
			{
				if (!__init_Release)
				{
					_Release = this.DTEInfo.SelectedProject.GetChildItem(Constants.ReleaseFolderName, true);
					__init_Release = true;
				}
				return _Release;
			}
		}

		/// <summary>
		/// Срабатывает при выполнении команды.
		/// </summary>
		protected override void OnExecute()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var copiedFiles = new List<string>();

			//копируем из папки с результатами билда все файлы, которые есть в папке Release
			string outputPath = this.DTEInfo.SelectedProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath")?.Value?.ToString() ?? "bin";

			foreach (ProjectItem releaseFile in this.Release.ProjectItems.OfType<ProjectItem>())
				_CopyFile(outputPath, releaseFile.Name, releaseFile);

			// Копирум WSP, если есть
			_CopyFile("Deploy", this.WSPFileName, this.Release.GetChildItem(this.WSPFileName));

			// Пишем, какие файлы были скопированы
			if (copiedFiles.Count > 0)
				this.WriteToOutput("\nСледующие файлы были скопированы в Release:\n\t"
					+ String.Join("\n\t", copiedFiles.ToArray()));
			else
				this.WriteToOutput("Ни один файл не был скопирован в Release.");

			// Выполняет копирование файла.
			void _CopyFile(string dir, string name, ProjectItem projItem)
			{
				FileInfo sourceFile = this.GetFile(dir, name, SearchOption.TopDirectoryOnly);
				if (sourceFile != null)
				{
					if (String.Equals(sourceFile.Extension, ".config", StringComparison.InvariantCultureIgnoreCase))
					{
						if (!WSSDeveloperPackage.ShowUserConfirmYesNo($"Заменить файл {name} в папке Release?", "Замена файла"))
							return;
					}
					this.AddOrReplaceFile(sourceFile, projItem);
					copiedFiles.Add($"{name} из [{dir?.Trim('\\')}]");
				}
			}
		}

		/// <summary>
		/// Добавляет или обновляет файл с чекаутом.
		/// </summary>
		/// <param name="sourceFile"></param>
		/// <param name="targetFile"></param>
		private void AddOrReplaceFile(FileInfo sourceFile, ProjectItem targetFile)
		{
			if (targetFile != null)
			{
				this.DTE.CheckOutItem(targetFile);
				FileInfo targetFileInfo = new FileInfo(targetFile.GetFullPath());
				if (targetFileInfo.IsReadOnly)
					throw new NotificationException("Не удалось сделать чекаут для файла '{0}'", targetFileInfo.FullName);

				File.Copy(sourceFile.FullName, targetFile.GetFullPath(), true);
			}
			else
				this.Release.ProjectItems.AddFromFileCopy(sourceFile.FullName);
		}

		/// <summary>
		/// Возвращает локальный файл в проекте.
		/// </summary>
		/// <param name="folderName"></param>
		/// <param name="fileName"></param>
		/// <param name="searchOption"></param>
		/// <returns></returns>
		private FileInfo GetFile(string folderName, string fileName, SearchOption searchOption)
		{
			if (String.IsNullOrEmpty(folderName))
				throw new ArgumentNullException(nameof(folderName));
			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			string folderPath = Path.Combine(this.DTEInfo.SelectedProject.Properties.Item("LocalPath").Value.ToString(), folderName);

			FileInfo file = null;

			if (!Directory.Exists(folderPath))
				Debug.WriteLine("В проекте не найдена папка '{0}'", folderName);
			else
			{
				DirectoryInfo folder = new DirectoryInfo(folderPath);
				file = folder.GetFiles(fileName, searchOption).FirstOrDefault();
				if (file == null)
					Debug.WriteLine($"Файл {fileName} не найден в папке {folderName} {searchOption}");
			}
			return file;
		}
	}
}
