using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using WSSConsulting.WSSDeveloper.Commands;

namespace WSSConsulting.WSSDeveloper
{
	/// <summary>
	/// Расширения DTE.
	/// </summary>
	public static class DTEExtensions
	{
		/// <summary>
		/// Возвращает первый элемент коллекци, удовлетворяющий условию, либо default(T).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static T FirstOrDefaultOfType<T>(this IEnumerable collection, Func<T, bool> predicate)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			if (predicate == null)
				throw new ArgumentNullException(nameof(predicate));

			return collection.OfType<T>().FirstOrDefault(predicate);
		}

		/// <summary>
		/// Возвращает первый элемент коллекци, либо default(T).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static T FirstOrDefaultOfType<T>(this IEnumerable collection)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));

			return collection.OfType<T>().FirstOrDefault();
		}

		/// <summary>
		/// Возвращает дочерний элемент в для элемента проекта.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="childName"></param>
		/// <param name="throwException"></param>
		/// <returns></returns>
		public static ProjectItem GetChildItem(this ProjectItem parent, string childName, bool throwException = false)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (parent == null)
				throw new ArgumentNullException(nameof(parent));
			if (String.IsNullOrEmpty(childName))
				throw new ArgumentNullException(nameof(childName));

			string childNameLow = childName.ToLower();
			ProjectItem item = parent.ProjectItems.FirstOrDefaultOfType<ProjectItem>(x => x.Name.ToLower() == childNameLow);
			if (item == null && throwException)
				throw new Exception($"Не найден дочерний элемент '{childName}' для элемента '{parent.Name}'");

			return item;
		}

		/// <summary>
		/// Возвращает дочерний элемент 1-го уровня в проекте.
		/// </summary>
		/// <param name="project"></param>
		/// <param name="childName"></param>
		/// <param name="throwException"></param>
		/// <returns></returns>
		public static ProjectItem GetChildItem(this Project project, string childName, bool throwException = false)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (project == null)
				throw new ArgumentNullException(nameof(project));
			if (String.IsNullOrEmpty(childName))
				throw new ArgumentNullException(nameof(childName));

			string childNameLow = childName.ToLower();
			ProjectItem item = project.ProjectItems.FirstOrDefaultOfType<ProjectItem>(x => x.Name.ToLower() == childNameLow);
			if (item == null && throwException)
				throw new Exception($"Не найден дочерний элемент '{childName}' для проекта '{project.Name}'");

			return item;
		}

		/// <summary>
		/// Возвращает путь к элементу на диске.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string GetFullPath(this ProjectItem item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			return item.Properties.Item("FullPath").Value.ToString();
		}

		/// <summary>
		/// Возвращает путь к файлу проекта на диске.
		/// </summary>
		/// <param name="proj"></param>
		/// <returns></returns>
		public static string GetProjectFolder(this Project proj)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (proj == null)
				throw new ArgumentNullException(nameof(proj));

			string csProjFilePath = proj.Properties.Item("FullPath").Value.ToString();
			int slashInd = csProjFilePath.LastIndexOf('\\');
			if (slashInd != -1)
				csProjFilePath = csProjFilePath.Substring(0, slashInd);
			return csProjFilePath;
		}

		/// <summary>
		/// Возвращает отнсительный путь к элементу в проекте.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static string GetProjectRelativePath(this ProjectItem item)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (item == null)
				throw new ArgumentNullException(nameof(item));

			List<string> path = new List<string>();
			path.Add(item.Name);

			ProjectItem projItem = item.Collection.Parent as ProjectItem;
			while (projItem != null)
			{
				if (projItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder
					|| projItem.Kind == EnvDTE.Constants.vsProjectItemKindVirtualFolder)
					path.Add(projItem.Name);
				projItem = projItem.Collection.Parent as ProjectItem;
			}

			path.Reverse();
			return String.Join("\\", path.ToArray());
		}


		/// <summary>
		/// Выполняет чек-аут проекта.
		/// </summary>
		/// <param name="dte"></param>
		/// <param name="proj"></param>
		internal static void CheckOutItem(this DTE dte, Project proj)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (dte == null)
				throw new ArgumentNullException(nameof(dte));
			if (proj == null)
				throw new ArgumentNullException(nameof(proj));

			string projName = (proj.Properties.Item("LocalPath").Value ?? String.Empty).ToString();
			dte.CheckOutItem(projName);
		}

		/// <summary>
		/// Выполняет чек-аут элемента проекта.
		/// </summary>
		/// <param name="dte"></param>
		/// <param name="item"></param>
		internal static void CheckOutItem(this DTE dte, ProjectItem item)
		{
			if (dte == null)
				throw new ArgumentNullException(nameof(dte));
			if (item == null)
				throw new ArgumentNullException(nameof(item));


			string itemName = item.GetFullPath();
			dte.CheckOutItem(itemName);
		}

		/// <summary>
		/// Выполняет чек-аут элемента.
		/// </summary>
		/// <param name="dte"></param>
		/// <param name="itemFullPath"></param>
		internal static void CheckOutItem(this DTE dte, string itemFullPath)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			if (dte == null)
				throw new ArgumentNullException(nameof(dte));

			if (dte.SourceControl.IsItemUnderSCC(itemFullPath))
			{
				if (!dte.SourceControl.IsItemCheckedOut(itemFullPath))
				{
					if (!dte.SourceControl.CheckOutItem(itemFullPath))
					{
						throw new NotificationException("Не удалось сделать Check-Out для файла '{0}'", itemFullPath);
					}
				}
			}
		}
	}
}
