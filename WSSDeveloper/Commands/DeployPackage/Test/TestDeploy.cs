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
	/// Выкладывание в тестирование.
	/// </summary>
	internal class TestDeploy : DeployType
	{
		/// <summary>
		/// Выбор папки с папками тестирования.
		/// </summary>
		public ComboBox TestComboBox { get; private set; }

		/// <summary>
		/// Выбор папки тестирования.
		/// </summary>
		public ComboBox TestFoldersBox { get; }

		/// <summary>
		/// Выкладывание в тестирование.
		/// </summary>
		/// <param name="switcher"></param>
		/// <param name="testComboBox">Выбор папки с папками тестирования.</param>
		/// <param name="deployPackageDestinationForm"></param>
		public TestDeploy(RadioButton switcher, ComboBox testComboBox, ComboBox testFoldersBox, DeployPackageDestinationForm deployPackageDestinationForm)
			: base(switcher, new Control[] { testComboBox, testFoldersBox }, deployPackageDestinationForm)
		{
			this.TestComboBox = testComboBox ?? throw new ArgumentNullException(nameof(testComboBox));
			TestFoldersBox = testFoldersBox ?? throw new ArgumentNullException(nameof(testFoldersBox));

			// Определяем поведение при выборе папки тестирования
			this.TestComboBox.SelectedIndexChanged += (s, e) =>
			{
				OptionItem item = this.TestComboBox.SelectedItem as OptionItem;
				int year = DateTime.Now.Year;

				this.TestFoldersBox.Items.Clear();
				this.TestFoldersBox.Items.AddRange(

					// Берём подпапки выбранной папки тестирования
					Directory.EnumerateDirectories(item.DirectoryPath, $"{year}*", SearchOption.TopDirectoryOnly)

					// Получаем информацию по папкам
					.Select(path => new DirectoryInfo(path))

					// На всякий случай отсеиваем несуществующие
					.Where(dir => dir.Exists)

					// Сортируем по названию
					.OrderByDescending(dir => dir.Name)

					// Берём первые 10
					.Take(10)

					// Формируем варианты выбора для списка
					.Select(x => new OptionItem(x)).ToArray()
				);
				this.TestFoldersBox.Text = this.FolderTemplateName;
			};
		}

		private bool __init_FolderTemplateName;
		private string _FolderTemplateName;
		/// <summary>
		/// Шаблон новой папки тестирования.
		/// </summary>
		private string FolderTemplateName
		{
			get
			{
				if (!__init_FolderTemplateName)
				{
					WSSCommandBase cmd = this.Form.Command;
					WSSOptions options = cmd.Options;
					string projName = cmd.DTEInfo.SelectedProject.Name;

					const string dmsProjNameStart = "WSSC.V4.DMS.";
					const string sysProjNameStart = "WSSC.V4.SYS.";

					Dictionary<string, string> shortCuts = options.OptionsPage.TestTemplateShortcuts;
					if (shortCuts != null && shortCuts.TryGetValue(projName?.ToLower(), out string shortCut))
					{
						projName = shortCut;
					}
					else if (projName.StartsWith(dmsProjNameStart, StringComparison.InvariantCultureIgnoreCase))
					{
						projName = projName.Substring(dmsProjNameStart.Length);
					}
					else if (projName.StartsWith(sysProjNameStart, StringComparison.InvariantCultureIgnoreCase))
					{
						projName = projName.Substring(sysProjNameStart.Length);
					}

					_FolderTemplateName = $"{DateTime.Now:yyyy.MM.dd} - {projName} - Описание ({options.UserName})";
					__init_FolderTemplateName = true;
				}
				return _FolderTemplateName;
			}
		}

		/// <summary>
		/// Выполняет деплой модуля.
		/// </summary>
		/// <param name="deployInfo"></param>
		internal override bool Deploy(DeployInfo deployInfo)
		{
			if (deployInfo == null)
				throw new ArgumentNullException(nameof(deployInfo));

			if (!(this.TestComboBox.SelectedItem is OptionItem item))
				throw new NotificationException("Не выбрана папка тестирования");

			//получаем папку тестирования
			DirectoryInfo testDir = new DirectoryInfo(item.DirectoryPath);

			//формируем имя папки с комплектом
			string description =
				this.TestFoldersBox.SelectedIndex >= 0
				? ((OptionItem)this.TestFoldersBox.SelectedItem).DirectoryPath
				: this.TestFoldersBox.Text;

			/*string targetDirName = Path.Combine(testDir.FullName, String.Format("{0:yyyy.MM.dd}{1}", DateTime.Now,
                                                                                String.IsNullOrEmpty(description)
                                                                                    ? null
                                                                                    : " - " + description));*/
			if (String.IsNullOrEmpty(description?.Trim()))
				throw new NotificationException("Не задано название папки");

			string targetDirName = Path.Combine(testDir.FullName, description);

			//создаём папку
			DirectoryInfo targetDir = this.CreateDirectory(targetDirName);

			//деплоим в папку
			if (deployInfo.CreateReleaseDirectory(targetDir, true) == null)
				return false;

			deployInfo.CreateSourcesDirectory(targetDir, false);

			this.Form.Command.Package.WriteToOutput(
				$"Сформирован комплект для проекта {deployInfo.Project.Name} по пути:\n<file://{targetDir.FullName.TrimStart('\\').Replace('\\', '/')}>");
			return true;
		}

		/// <summary>
		/// Пытается создать папку. Если такая папка существует, то
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private DirectoryInfo CreateDirectory(string path)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			var dir = new DirectoryInfo(path);
			if (!dir.Exists)
				dir.Create();

			return dir;
		}

		/// <summary>
		/// Выполняет действия при инициализации контролов для данного вида деплоя.
		/// </summary>
		internal override void OnInit()
		{
			//добавляем выбор папок
			string[] directories = Directory.GetDirectories(this.Form.Command.Options.TestFolderPath, "Тестирование*", SearchOption.TopDirectoryOnly);
			if (directories.Length > 0)
			{
				foreach (string directory in directories)
				{
					this.TestComboBox.Items.Add(new OptionItem(directory));
				}
				this.TestComboBox.SelectedIndex = 0;
			}

			//добавляем текст для папки тестирования по шаблону
			string projName = this.Form.Command.DTEInfo.SelectedProject.Name;
			int dotIndex = projName.LastIndexOf('.', projName.Length - 1);
			if (dotIndex != -1)
				projName = projName.Substring(dotIndex + 1);

			this.TestFoldersBox.Text = this.FolderTemplateName;
		}
	}
}
