using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace WSS.Wizards
{
	/// <summary>
	/// Класс для заполнения словаря ключевых слов.
	/// </summary>
	public class ReplacementDictionaryHelper
	{
		/// <summary>
		/// Класс для заполнения словаря ключевых слов.
		/// </summary>
		public ReplacementDictionaryHelper(EnvDTE80.DTE2 dte, Dictionary<string, string> replacementDictionary)
		{
			Dte = dte ?? throw new ArgumentNullException(nameof(dte));
			ReplacementDictionary = replacementDictionary ?? throw new ArgumentNullException(nameof(replacementDictionary));
		}

		/// <summary>
		/// DTE.
		/// </summary>
		public EnvDTE80.DTE2 Dte { get; }

		/// <summary>
		/// Словарь замен.
		/// </summary>
		public Dictionary<string, string> ReplacementDictionary { get; }


		private bool __init_Project;
		private Project _Project;
		/// <summary>
		/// Преокт.
		/// </summary>
		private Project Project
		{
			get
			{
				if (!__init_Project)
				{
					SelectedItems selectedItems = this.Dte.SelectedItems;
					if (selectedItems == null || selectedItems.Count > 0)
					{
						SelectedItem selectedItem = selectedItems.Item(1); //индексация с 1

						if (selectedItem.Project != null || selectedItem.ProjectItem != null)
						{
							if (selectedItem.Project != null)
								_Project = selectedItem.Project.ProjectItems.ContainingProject;
							else
								_Project = selectedItem.ProjectItem.ContainingProject;
						}
					}
					__init_Project = true;
				}
				return _Project;
			}
		}

		private bool __init_ProjectName;
		private string _ProjectName;
		/// <summary>
		/// Название проекта.
		/// </summary>
		private string ProjectName
		{
			get
			{
				if (!__init_ProjectName)
				{
					if (!this.ReplacementDictionary.TryGetValue("$projectname$", out _ProjectName))
					{
						_ProjectName = this.Project?.Name;
					}

					if (String.IsNullOrEmpty(_ProjectName))
						throw new Exception("Не удалось определить название создаваемого проекта");

					__init_ProjectName = true;
				}
				return _ProjectName;
			}
		}

		/// <summary>
		/// Заполняет словарь.
		/// </summary>
		public void FillDictionary()
		{
			//название проекта
			this.AddValue("$projectname$", this.ProjectName, false);

			//открытый ключ
			this.AddValue("$publickeytoken$", new PublicKeyTokenHelper().GetPublicKeyToken(this.Project), true);

			//относительный путь к папке проекта
			this.AddProjectRelativePath();

			//подменяем компанию. По умолчанию берётся из реестра "HKLM\Software\Microsoft\Windows NT\CurrentVersion\RegisteredOrganization"
			this.AddValue("$registeredorganization$", "WSS-Consulting", true);

			//добавляем дату Сегодня
			this.AddValue("$today$", DateTime.Today.ToString("yyyy.MM.dd"), true);

			//код заказчика и префикс для контрола
			this.AddCustomerCodeAndListFormControlPrefix();
		}

		/// <summary>
		/// Добавляет относительный путь к проекту.
		/// </summary>
		/// <returns></returns>
		private void AddProjectRelativePath()
		{
			SelectedItem selectedItem = this.Dte.SelectedItems?.Item(1);
			if (selectedItem != null)
			{
				string relativePath = this.GetProjectRelativePath(selectedItem);
				this.AddValue("$projectitem_rel_path$", relativePath, false);
				this.AddValue("$projectitem_rel_path_web$", relativePath.Replace('\\', '/'), false);
			}
		}

		/// <summary>
		/// Добавляет код заказчика и префикс для контролов.
		/// </summary>
		/// <returns></returns>
		private string AddCustomerCodeAndListFormControlPrefix()
		{
			//получаем код заказчика 
			string customerCode = "WSS";

			int lastDotIndex = this.ProjectName.LastIndexOf('.');
			if (lastDotIndex == -1 || lastDotIndex == this.ProjectName.Length - 1)
				customerCode = this.ProjectName;
			else
				customerCode = this.ProjectName.Substring(lastDotIndex + 1);

			this.AddValue("$customercode$", customerCode, true);
			this.AddValue("$ListFormControlPrefix$", customerCode, true);

			return customerCode;
		}

		/// <summary>
		/// Добавляет значение в словарь замен.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="replace"></param>
		/// <returns></returns>
		private string AddValue(string key, string value, bool replace)
		{
			if (this.ReplacementDictionary.TryGetValue(key, out string existingValue))
			{
				if (replace)
					this.ReplacementDictionary[key] = value;
				else
					value = existingValue;
			}
			else
				this.ReplacementDictionary.Add(key, value);

			return value;
		}

		/// <summary>
		/// Возвращает отнсительный путь к элементу в проекте.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private string GetProjectRelativePath(SelectedItem item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			List<string> path = new List<string>();
			if (item.ProjectItem != null)
			{
				path.Add(item.Name);

				ProjectItem projItem = item.ProjectItem.Collection.Parent as ProjectItem;
				while (projItem != null)
				{
					path.Add(projItem.Name);
					projItem = projItem.Collection.Parent as ProjectItem;
				}
			}

			path.Reverse();
			return String.Join("\\", path.ToArray());
		}
	}
}
