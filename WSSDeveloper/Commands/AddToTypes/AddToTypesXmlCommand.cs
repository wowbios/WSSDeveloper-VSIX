using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Команда добавления в Types.xml.
	/// </summary>
	internal class AddToTypesXmlCommand : WSSCommandBase
	{
		/// <summary>
		/// Срабатывает при выполнении команды.
		/// </summary>
		protected override void OnExecute()
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

			List<ProjectItem> selectedItems = this.DTEInfo.SelectedItems;
			if (selectedItems == null || selectedItems.Count == 0)
				throw new NotificationException("Не выбран ни один документ");

			ProjectItem selectedItem = selectedItems[0];
			if (selectedItem?.FileCodeModel == null)
				throw new NotificationException("Файл не является файлом исходного кода");

			List<ClassMetadata> allClasses = this.GetClassesFromFile(selectedItem.FileCodeModel);
			if (allClasses == null || allClasses.Count == 0)
				throw new NotificationException("В файле не найдены определения классов");

			using (AddToTypes.AddToTypesForm form = new AddToTypes.AddToTypesForm(allClasses, this))
			{
				if (!form.TypesManager.EnsureTypesXmlItem())
					throw new NotificationException("Отсутствует файл Types.xml");

				form.ShowDialog();
			}
		}

		/// <summary>
		/// Извлекает информацию по всем классам двух уровней внутри файла.
		/// </summary>
		/// <param name="codeModel"></param>
		/// <returns></returns>
		private List<ClassMetadata> GetClassesFromFile(FileCodeModel codeModel)
		{
			if (codeModel == null)
				throw new ArgumentNullException(nameof(codeModel));

			List<ClassMetadata> classes = new List<ClassMetadata>();
			foreach (CodeElement element in codeModel.CodeElements.OfType<CodeElement>())
			{
				if (element.Kind == vsCMElement.vsCMElementNamespace)
					classes.AddRange(from childCodeElement in ((CodeNamespace)element).Children.OfType<CodeElement>()
									 where childCodeElement.Kind == vsCMElement.vsCMElementClass
									 select new ClassMetadata((CodeClass)childCodeElement));
			}

			return classes;
		}
	}
}
