using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSSConsulting.WSSDeveloper.Commands;

namespace WSSConsulting.WSSDeveloper.Utils
{
	/// <summary>
	/// Класс для формирования папок в %temp%.
	/// </summary>
	internal class TempDirWorker : IDisposable
	{
		/// <summary>
		/// Класс для формирования папок в %temp%.
		/// </summary>
		public TempDirWorker()
		{
			string guid = Guid.NewGuid().ToString("B");
			string tempDirName = Path.Combine(this.TempDir.FullName, $"WSSDeveloper_temp_projectDeploy_{guid}");

			this.CreatedDir = Directory.CreateDirectory(tempDirName);
		}

		private bool __init_TempDir;
		private DirectoryInfo _TempDir;
		/// <summary>
		/// Папка %temp%.
		/// </summary>
		private DirectoryInfo TempDir
		{
			get
			{
				if (!__init_TempDir)
				{
					_TempDir = new DirectoryInfo(Path.GetTempPath());
					if (!_TempDir.Exists)
						throw new NotificationException($"Не найден путь TEMP: {_TempDir.FullName}");

					__init_TempDir = true;
				}
				return _TempDir;
			}
		}

		/// <summary>
		/// Созданная временная директория.
		/// </summary>
		public readonly DirectoryInfo CreatedDir;

		/// <summary>
		/// Копирует содержимое папки <paramref name="sourceDir"/> во временную папку.
		/// </summary>
		/// <param name="sourceDir">Папка с исходными файлами.</param>
		public void CopyDirectoryWithFiles(DirectoryInfo sourceDir)
		{
			if (sourceDir == null)
				throw new ArgumentNullException(nameof(sourceDir));

			string sourceDirName = sourceDir.FullName;
			string tempDirName = this.CreatedDir.FullName;

			// Создаём все вложенные папки
			foreach (string dirPath in Directory.GetDirectories(sourceDirName, "*",
				SearchOption.AllDirectories))
				Directory.CreateDirectory(dirPath.Replace(sourceDirName, tempDirName));

			// Копируем все файлы с заменой
			foreach (string newPath in Directory.GetFiles(sourceDirName, "*.*",
				SearchOption.AllDirectories))
			{
				try
				{
					this.CopyFile(sourceDirName, newPath);
				}
				catch (IOException accessException)
				{
					if (!newPath.EndsWith("db.lock"))
						throw;
				}
			}
		}

		/// <summary>
		/// Копирует файл из <paramref name="fileFolder"/> во временную папку.
		/// </summary>
		/// <param name="fileFolder">Путь к папке файла.</param>
		/// <param name="fileFullName">Полный путь к файлу.</param>
		public void CopyFile(string fileFolder, string fileFullName)
		{
			if (String.IsNullOrEmpty(fileFolder))
				throw new ArgumentNullException(nameof(fileFolder));
			if (String.IsNullOrEmpty(fileFullName))
				throw new ArgumentNullException(nameof(fileFullName));

			string targetFileFullName = fileFullName.Replace(fileFolder, this.CreatedDir.FullName);
			File.Copy(fileFullName, targetFileFullName, true);

			//снимаем Readonly для возможности последующего удаления
			new FileInfo(targetFileFullName).IsReadOnly = false;
		}

		/// <summary>
		/// Освобождает ресурсы.
		/// </summary>
		public void Dispose()
		{
			// Удаляем временную папку
			this.CreatedDir.Delete(true);
		}
	}
}
