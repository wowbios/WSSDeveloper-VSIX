using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Элемент выпадающего меню.
	/// </summary>
	internal class OptionItem
	{
		/// <summary>
		/// Путь к папке.
		/// </summary>
		internal string DirectoryPath { get; }

		/// <summary>
		/// Отображаемое название.
		/// </summary>
		internal string DisplayName { get; }


		/// <summary>
		/// Элемент выпадающего меню.
		/// </summary>
		/// <param name="dir">Директория.</param>
		public OptionItem(DirectoryInfo dir)
		{
			if (dir == null)
				throw new ArgumentNullException(nameof(dir));

			this.DirectoryPath = dir.FullName;
			this.DisplayName = dir.Name;
		}

		/// <summary>
		/// Элемент выпадающего меню.
		/// </summary>
		/// <param name="directoryPath">Путь к папке.</param>
		public OptionItem(string directoryPath)
		{
			if (String.IsNullOrEmpty(directoryPath))
				throw new ArgumentException("Argument is null or empty", nameof(directoryPath));

			this.DirectoryPath = directoryPath;
			this.DisplayName = new DirectoryInfo(directoryPath).Name;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return this.DisplayName;
		}
	}
}
