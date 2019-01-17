using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Выкладка комплекта.
	/// </summary>
	internal class PackageDeploy : DeployType
	{
		/// <summary>
		/// Список папок комплектов заказчиков.
		/// </summary>
		public ComboBox CustomerComboBox { get; private set; }

		/// <summary>
		/// Выкладка комплекта.
		/// </summary>
		/// <param name="btn"></param>
		/// <param name="customerComboBox">Список папок комплектов заказчиков.</param>
		/// <param name="form"></param>
		public PackageDeploy(RadioButton btn, ComboBox customerComboBox, DeployPackageDestinationForm form)
			: base(btn, new Control[] { customerComboBox }, form)
		{
			if (btn == null)
				throw new ArgumentNullException(nameof(btn));
			this.CustomerComboBox = customerComboBox ?? throw new ArgumentNullException(nameof(customerComboBox));
		}

		/// <summary>
		/// Выполняет деплой модуля.
		/// </summary>
		/// <param name="deployInfo"></param>
		internal override bool Deploy(DeployInfo deployInfo)
		{
			if (deployInfo == null)
				throw new ArgumentNullException(nameof(deployInfo));

			OptionItem item = this.CustomerComboBox.SelectedItem as OptionItem;
			if (item == null)
				throw new NotificationException("Не выбрана папка комплекта");

			//получение папки
			string todayDirName = DateTime.Now.ToString("yyyy.MM.dd");
			string readmeFilePath = null;
			string[] dateFolders = Directory.GetDirectories(item.DirectoryPath).Where(x => Regex.IsMatch(x, @"[\d\.]+")).ToArray();
			string todayDir = dateFolders.FirstOrDefault(x => x.EndsWith("\\" + todayDirName));
			DirectoryInfo dir;
			if (todayDir == null)
			{
				dir = Directory.CreateDirectory(Path.Combine(item.DirectoryPath, todayDirName));
			}
			else
			{
				dir = new DirectoryInfo(todayDir);
				readmeFilePath = Directory.GetFiles(dir.FullName, "readme.txt", SearchOption.TopDirectoryOnly).FirstOrDefault();
			}

			if (readmeFilePath == null)
			{
				//получаем уже существующий файл readme
				IOrderedEnumerable<string> orderedDirectories = dateFolders.OrderByDescending(x => x);
				readmeFilePath = orderedDirectories
					.Select(d => Directory.GetFiles(d, "readme.txt", SearchOption.TopDirectoryOnly).FirstOrDefault())
					.FirstOrDefault(x => x != null);

			}

			//формируем комплект
			return this.CreatePackage(deployInfo, dir, readmeFilePath);
		}

		/// <summary>
		/// Выполняет действия при инициализации контролов для данного вида деплоя.
		/// </summary>
		internal override void OnInit()
		{
			//пытаемся автоматически выбрать подходящую папку
			bool? projectSelected = null;
			string projectNameStartPatter = "wssc.v4.dms.";
			string projectName = this.Form.Command.DTEInfo.SelectedProject.Name.ToLower();
			if (!String.IsNullOrEmpty(projectName) && projectName.StartsWith(projectNameStartPatter))
			{
				projectName = projectName.Substring(projectNameStartPatter.Length);
				int dotIndex = projectName.IndexOf('.');
				if (dotIndex != -1)
				{
					projectName = projectName.Substring(0, dotIndex - 1);
				}
				projectSelected = false;
			}

			int selectIndex = -1;

			//заполняем варианты папок для деплоя
			string[] directories = Directory.GetDirectories(this.Form.Command.Options.PackageFolderPath, "*", SearchOption.TopDirectoryOnly);
			if (directories.Length > 0)
			{
				foreach (string directory in directories)
				{
					OptionItem optionItem = new OptionItem(directory);
					this.CustomerComboBox.Items.Add(optionItem);
					if (selectIndex == -1
						&& projectSelected.HasValue
						&& projectSelected.Value == false
						&& optionItem.DisplayName.ToLower().StartsWith(projectName))
					{
						selectIndex = this.CustomerComboBox.Items.Count - 1;
						projectSelected = true;
					}
				}
				this.CustomerComboBox.SelectedIndex = projectSelected == true ? selectIndex : 0;
			}
		}

		/// <summary>
		/// Выкладывает модуль в папку.
		/// </summary>
		/// <param name="deployInfo"></param>
		/// <param name="directory"></param>
		/// <param name="prevReadmePath"></param>
		private bool CreatePackage(DeployInfo deployInfo, DirectoryInfo directory, string prevReadmePath)
		{
			if (deployInfo == null)
				throw new ArgumentNullException(nameof(deployInfo));
			if (directory == null)
				throw new ArgumentNullException(nameof(directory));

			//README
			string readmePath = Path.Combine(directory.FullName, "Readme.txt");

			if (!File.Exists(readmePath))
			{
				if (String.IsNullOrEmpty(prevReadmePath))
				{
					File.Copy(deployInfo.LogFile.GetFullPath(), readmePath);
				}
				else
				{
					if (this.IsASCIIFile(prevReadmePath))
					{
						File.WriteAllLines(readmePath, this.GetUTF8LinesFromASCIIFile(prevReadmePath), Encoding.UTF8);
					}
					else
						File.Copy(prevReadmePath, readmePath);
				}
			}

			if (deployInfo.Logs != null && deployInfo.Logs.Count > 0)
			{
				List<string> lines = this.GetNewReadmeLines(deployInfo, prevReadmePath);
				File.WriteAllLines(readmePath, lines, Encoding.UTF8);
			}

			DirectoryInfo releaseDir = deployInfo.CreateReleaseDirectory(directory, false);
			deployInfo.CreateSourcesDirectory(directory, false);
			this.Form.Command.Package.WriteToOutput(
				$"Сформирован комплект для проекта {deployInfo.Project.Name} по пути:\n<file://{releaseDir.FullName.TrimStart('\\').Replace('\\', '/')}>");
			return true;
		}

		/// <summary>
		/// Возвращает преобразованные в UTF8 строки из файла в кодировке Windows-1251.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private string[] GetUTF8LinesFromASCIIFile(string path)
		{
			Encoding ansi = Encoding.GetEncoding(1251);
			Encoding utf8 = Encoding.UTF8;
			string[] prevContent = File.ReadAllLines(path, ansi);
			List<byte[]> prevContentUTF8 = prevContent.Select(row => Encoding.Convert(ansi, utf8, ansi.GetBytes(row))).ToList();
			return prevContentUTF8.Select(x => utf8.GetString(x)).ToArray();
		}

		/// <summary>
		/// Определяет в какой кодировке сохранён файл. Вероятность 80%.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private bool IsASCIIFile(string path)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			using (FileStream fs = File.OpenRead(path))
			{
				Ude.CharsetDetector detector = new Ude.CharsetDetector();
				detector.Feed(fs);
				detector.DataEnd();
				if (detector.Charset != null)
				{
					return detector.Charset == "windows-1251" && detector.Confidence > 0.8;
				}
			}
			return false;
		}

		/// <summary>
		/// Формирует новое содержание файла Readme.
		/// </summary>
		/// <param name="deployInfo"></param>
		/// <param name="prevReadmePath"></param>
		/// <returns></returns>
		private List<string> GetNewReadmeLines(DeployInfo deployInfo, string prevReadmePath)
		{
			string projectName = deployInfo.Project.Name;
			List<string> lines = null;
			string today = DateTime.Now.ToString("yyyy.MM.dd");
			List<string> prevLogs = new List<string>();
			if (!String.IsNullOrEmpty(prevReadmePath))
			{
				lines = this.IsASCIIFile(prevReadmePath)
					? this.GetUTF8LinesFromASCIIFile(prevReadmePath).ToList()
					: File.ReadAllLines(prevReadmePath).ToList();

				int todayIndex = -1;
				for (int i = 0; i < lines.Count; i++)
				{
					string line = lines[0];
					if (Regex.IsMatch(line, Constants.LogDatePattern))
					{
						if (line.Contains(today))
						{
							todayIndex = i;
						}

						break;
					}
				}

				if (todayIndex == -1) //вставляем логи с новой датой
				{
					List<string> rows = new List<string>();
					rows.Add(today);
					rows.Add(projectName);
					rows.AddRange(deployInfo.Logs);
					rows.Add(String.Empty);
					lines.InsertRange(0, rows);
				}
				else if (todayIndex + 1 < lines.Count) //добавляем логи к старой дате
				{
					int insertIndex = todayIndex + 1;
					int lastOldLogIndex = -1;
					for (int i = todayIndex + 1; i < lines.Count; i++)
					{
						string todayLog = lines[i].Trim('\r', '\n');
						if (String.IsNullOrEmpty(todayLog) || Regex.IsMatch(todayLog, Constants.LogDatePattern))
						{
							lastOldLogIndex = i - 1;
							break;
						}

						prevLogs.Add(todayLog);
					}
					if (lastOldLogIndex != -1)
						lines.RemoveRange(insertIndex, lastOldLogIndex - todayIndex);

					Dictionary<string, List<string>> logsByProject = this.GroupLogsByProjectName(prevLogs);
					IEnumerable<string> currentLogs = deployInfo.Logs.Where(x => !logsByProject.ContainsKey(x));
					if (logsByProject.TryGetValue(projectName, out List<string> currentProjectLogs))
					{
						logsByProject[projectName] = currentProjectLogs.Union(currentLogs).Distinct().ToList();
					}
					else
						logsByProject.Add(projectName, deployInfo.Logs.ToList());

					lines.InsertRange(todayIndex + 1, logsByProject.SelectMany(x =>
					{
						List<string> logsForProj = x.Value;
						logsForProj.Insert(0, x.Key);
						return logsForProj;
					}));
				}
			}
			else
			{
				lines = new List<string> { today, projectName };
				lines.AddRange(deployInfo.Logs);
			}
			return lines;
		}

		/// <summary>
		/// Группирует логи по проектам.
		/// </summary>
		/// <param name="logs"></param>
		/// <returns></returns>
		private Dictionary<string, List<string>> GroupLogsByProjectName(IEnumerable<string> logs)
		{
			if (logs == null)
				throw new ArgumentNullException(nameof(logs));

			Dictionary<string, List<string>> logsByProject = new Dictionary<string, List<string>>();

			List<string> currentProjLogs = null;
			foreach (string log in logs)
			{
				if (Regex.IsMatch(log, Constants.ProjectNamePattern))
				{
					string projName = log.Trim('\n', '\t', '\r');
					if (!logsByProject.TryGetValue(projName, out currentProjLogs))
					{
						currentProjLogs = new List<string>();
						logsByProject.Add(projName, currentProjLogs);
					}
				}
				else
				{
					currentProjLogs?.Add(log);
				}
			}
			return logsByProject;
		}
	}
}
