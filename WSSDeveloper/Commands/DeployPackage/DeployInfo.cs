using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using WSSConsulting.WSSDeveloper.Utils;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Информация для формируемого комплекта.
	/// </summary>
	internal class DeployInfo
	{
		/// <summary>
		/// Проект, по которому формируется комплект.
		/// </summary>
		public readonly Project Project;

		/// <summary>
		/// Информация для формируемого комплекта.
		/// </summary>
		/// <param name="project">Проект, по которому формируется комплект.</param>
		public DeployInfo(Project project)
		{
			this.Project = project ?? throw new ArgumentNullException(nameof(project));
		}

		private bool __init_ReleaseFolder;
		private ProjectItem _ReleaseFolder;
		/// <summary>
		/// Папка Release.
		/// </summary>
		internal ProjectItem ReleaseFolder
		{
			get
			{
				if (!__init_ReleaseFolder)
				{
					_ReleaseFolder = this.Project.ProjectItems.FirstOrDefaultOfType<ProjectItem>(x => x.Name == Constants.ReleaseFolderName);
					if (_ReleaseFolder == null)
						throw new NotificationException("Папка Release не найдена");

					__init_ReleaseFolder = true;
				}
				return _ReleaseFolder;
			}
		}

		private bool __init_DeployFolder;
		private ProjectItem _DeployFolder;
		/// <summary>
		/// Папка Deploy.
		/// </summary>
		internal ProjectItem DeployFolder
		{
			get
			{
				if (!__init_DeployFolder)
				{
					_DeployFolder = this.Project.ProjectItems.FirstOrDefaultOfType<ProjectItem>(x => x.Name == Constants.DeployFilesFolderName);

					__init_DeployFolder = true;
				}
				return _DeployFolder;
			}
		}

		private bool __init_LogFile;
		private ProjectItem _LogFile;
		/// <summary>
		/// Log файл.
		/// </summary>
		internal ProjectItem LogFile
		{
			get
			{
				if (!__init_LogFile)
				{
					_LogFile = this.ReleaseFolder.ProjectItems.FirstOrDefaultOfType<ProjectItem>(x => x.Name.ToLower() == this.Project.Name.ToLower() + ".log");
					if (_LogFile == null)
						throw new Exception("В папке Release не найден *.log файл");

					__init_LogFile = true;
				}
				return _LogFile;
			}
		}

		private bool __init_Logs;
		private ReadOnlyCollection<string> _Logs;
		/// <summary>
		/// Логи проекта за сегодня.
		/// </summary>
		internal ReadOnlyCollection<string> Logs
		{
			get
			{
				if (!__init_Logs)
				{
					_Logs = this.GetTodayLogs();
					__init_Logs = true;
				}
				return _Logs;
			}
		}

		private bool __init_ProjectName;
		private string _ProjectName;
		/// <summary>
		/// Имя проекта.
		/// </summary>
		private string ProjectName
		{
			get
			{
				if (!__init_ProjectName)
				{
					_ProjectName = this.Project.Name;
					__init_ProjectName = true;
				}
				return _ProjectName;
			}
		}

		private bool __init_FolderModuleName;
		private string _FolderModuleName;
		/// <summary>
		/// Имя модуля, адаптированное под использование в названии папки.
		/// </summary>
		private string FolderModuleName
		{
			get
			{
				if (!__init_FolderModuleName)
				{
					HashSet<char> invalidFolderNameChars = new HashSet<char>(Path.GetInvalidPathChars());
					_FolderModuleName = new string(this.ProjectName.Where(c => !invalidFolderNameChars.Contains(c)).ToArray());
					__init_FolderModuleName = true;
				}
				return _FolderModuleName;
			}
		}

		private bool __init_FileModuleName;
		private string _FileModuleName;
		/// <summary>
		/// Имя модуля, адаптированное под использование в названии файла.
		/// </summary>
		private string FileModuleName
		{
			get
			{
				if (!__init_FileModuleName)
				{
					HashSet<char> invalidFilenameChars = new HashSet<char>(Path.GetInvalidFileNameChars());
					_FileModuleName = new string(this.ProjectName.Where(c => !invalidFilenameChars.Contains(c)).ToArray());
					__init_FileModuleName = true;
				}
				return _FileModuleName;
			}
		}

		/// <summary>
		/// Возвращает список логов за сегодня.
		/// </summary>
		/// <returns></returns>
		private ReadOnlyCollection<string> GetTodayLogs()
		{
			List<string> logs = new List<string>();
			string today = DateTime.Now.ToString("yyyy.MM.dd");
			bool todayRead = false;
			List<string> firstDate = File.ReadLines(this.LogFile.GetFullPath()).SkipWhile(x =>
											   {
												   if (!Regex.IsMatch(x, Constants.LogDatePattern))
													   return true;

												   todayRead = x.Contains(today);
												   return false;
											   }).ToList();

			if (todayRead)
			{
				logs = firstDate
					.Skip(2) //date и projectname
					.TakeWhile(x => !String.IsNullOrEmpty(x.Trim('\n', '\r', '\t')))
					.ToList();
			}

			return new ReadOnlyCollection<string>(logs);
		}

		/// <summary>
		/// Создаёт папку Release.
		/// </summary>
		/// <param name="dir"></param>
		public DirectoryInfo CreateReleaseDirectory(DirectoryInfo dir, bool confirmIfExists)
		{
			if (dir == null)
				throw new ArgumentNullException(nameof(dir));

			DirectoryInfo releaseDirectory = new DirectoryInfo(Path.Combine(dir.FullName, "R - " + this.FolderModuleName));
			if (!releaseDirectory.Exists)
			{
				releaseDirectory.Create();
			}
			else if (confirmIfExists)
			{
				if (!WSSDeveloperPackage.ShowUserConfirmYesNo($"Папка\n{releaseDirectory.FullName}\nуже существует. Заменить?", $"Папка {releaseDirectory.Name} уже существует"))
					return null;
			}

			foreach (ProjectItem item in this.ReleaseFolder.ProjectItems)
			{
				string sourcePath = item.GetFullPath();
				string targetPath = Path.Combine(releaseDirectory.FullName, item.Name);
				this.CopyAndOverwrite(sourcePath, targetPath);
			}

			if (this.DeployFolder != null)
			{
				this.EnsureFile(this.DeployFolder, releaseDirectory, "uninstall.bat", Constants.UninstallBatContent, Encoding.ASCII);
				this.EnsureFile(this.DeployFolder, releaseDirectory, "setup.bat", Constants.SetupBatContent, Encoding.ASCII);
			}
			return releaseDirectory;
		}

		/// <summary>
		/// Создаёт папку Sources.
		/// </summary>
		/// <param name="dir"></param>
		public void CreateSourcesDirectory(DirectoryInfo dir, bool confirmIfExists)
		{
			if (dir == null)
				throw new ArgumentNullException(nameof(dir));

			DirectoryInfo sourcesDirectory = new DirectoryInfo(Path.Combine(dir.FullName, "S - " + this.FolderModuleName));
			if (!sourcesDirectory.Exists)
			{
				sourcesDirectory.Create();
			}
			else
			{
				if (confirmIfExists)
				{
					if (!WSSDeveloperPackage.ShowUserConfirmYesNo($"Папка\n{sourcesDirectory.FullName}\nуже существует. Заменить?", $"Папка {sourcesDirectory.Name} уже существует"))
						return;
				}
			}

			string zipFilePath = Path.Combine(sourcesDirectory.FullName, this.FileModuleName + ".zip");
			this.DeleteFile(zipFilePath);

			DirectoryInfo projectDir = new DirectoryInfo(this.Project.GetProjectFolder());
			try
			{
				ZipFile.CreateFromDirectory(projectDir.FullName, zipFilePath,
					CompressionLevel.Optimal,
					true,
					Encoding.UTF8);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Не удалось сформировать S архив: " + ex);

				// Удаляем возможно частично сформированный архив
				this.DeleteFile(zipFilePath);

				//копируем в %temp% и формируем архив оттуда
				this.CreateZipFromTempCopy(projectDir, zipFilePath);
			}
		}

		/// <summary>
		/// Создаёт архив из папки <paramref name="sourceDir"/>, путём копирования исхондых данных во временную папку.
		/// </summary>
		/// <param name="sourceDir">Папка с файлами для копирования.</param>
		/// <param name="destinationArchiveFileName">Путь к формируемому архиву.</param>
		private void CreateZipFromTempCopy(DirectoryInfo sourceDir, string destinationArchiveFileName)
		{
			if (sourceDir == null)
				throw new ArgumentNullException(nameof(sourceDir));
			if (String.IsNullOrEmpty(destinationArchiveFileName))
				throw new ArgumentNullException(nameof(destinationArchiveFileName));

			using (TempDirWorker tempDirWorker = new TempDirWorker())
			{
				tempDirWorker.CopyDirectoryWithFiles(sourceDir);
				ZipFile.CreateFromDirectory(tempDirWorker.CreatedDir.FullName, destinationArchiveFileName,
					CompressionLevel.Optimal,
					false,
					Encoding.UTF8);
			}
		}

		/// <summary>
		/// Если файл папке проекта <paramref name="sourceProjectDirectory"/> отсутствует, то создаёт новый файл с именем <paramref name="name"/>
		/// и содержанием <paramref name="content"/> в кодировке <paramref name="encoding"/>. Файл копируется в <paramref name="targetDirectory"/>.
		/// </summary>
		/// <param name="sourceProjectDirectory"></param>
		/// <param name="targetDirectory"></param>
		/// <param name="name"></param>
		/// <param name="content"></param>
		/// <param name="encoding"></param>
		private void EnsureFile(ProjectItem sourceProjectDirectory, DirectoryInfo targetDirectory, string name, string content, Encoding encoding)
		{
			if (sourceProjectDirectory == null)
				throw new ArgumentNullException(nameof(sourceProjectDirectory));
			if (targetDirectory == null)
				throw new ArgumentNullException(nameof(targetDirectory));
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));
			if (String.IsNullOrEmpty(content))
				throw new ArgumentNullException(nameof(content));
			if (encoding == null)
				throw new ArgumentNullException(nameof(encoding));

			ProjectItem projectFile = sourceProjectDirectory.GetChildItem(name);
			if (projectFile == null)
			{
				File.WriteAllText(Path.Combine(targetDirectory.FullName, name), content, encoding);
			}
			else
				this.CopyAndOverwrite(projectFile.GetFullPath(), Path.Combine(targetDirectory.FullName, projectFile.Name));
		}

		/// <summary>
		/// Копирует файл с перезаписью.
		/// </summary>
		/// <param name="sourceFilePath"></param>
		/// <param name="targetFilePath"></param>
		protected void CopyAndOverwrite(string sourceFilePath, string targetFilePath)
		{
			if (String.IsNullOrEmpty(sourceFilePath))
				throw new ArgumentNullException(nameof(sourceFilePath));
			if (String.IsNullOrEmpty(targetFilePath))
				throw new ArgumentNullException(nameof(targetFilePath));

			this.DeleteFile(targetFilePath);

			File.Copy(sourceFilePath, targetFilePath);
		}


		/// <summary>
		/// Удаляет файл.
		/// </summary>
		/// <param name="path"></param>
		protected void DeleteFile(string path)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (File.Exists(path))
			{
				FileInfo fi = new FileInfo(path);
				if (fi.IsReadOnly)
					fi.IsReadOnly = false;

				File.Delete(fi.FullName);
			}
		}
	}
}
