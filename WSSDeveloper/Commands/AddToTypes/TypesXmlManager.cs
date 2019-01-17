using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using EnvDTE;
using WSSConsulting.WSSDeveloper.Commands.AddToTypes;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Объект для работы с Types.xml.
	/// </summary>
	internal class TypesXmlManager
	{
		/// <summary>
		/// Команда.
		/// </summary>
		internal WSSCommandBase Cmd { get; }

		/// <summary>
		/// Объект для работы с Types.xml.
		/// </summary>
		/// <param name="cmd">Команда.</param>
		public TypesXmlManager(WSSCommandBase cmd)
		{
			this.Cmd = cmd ?? throw new ArgumentNullException(nameof(cmd));
		}

		private bool __init_TypesXmlItem;
		private ProjectItem _TypesXmlItem;
		/// <summary>
		/// Элемент проекта - Types.xml.
		/// </summary>
		private ProjectItem TypesXmlItem
		{
			get
			{
				if (!__init_TypesXmlItem)
				{
					string typesFileName = $"Types_{this.Cmd.DTEInfo.SelectedProject.Name}.xml";
					_TypesXmlItem = this.Cmd.DTEInfo.SelectedProject.GetChildItem(typesFileName);
					if (_TypesXmlItem == null)
					{
						if (WSSDeveloperPackage.ShowUserConfirmYesNo("Создать файл Types.xml?", "Создать Types.xml?"))
						{
							this.Cmd.DTE.CheckOutItem(this.Cmd.DTEInfo.SelectedProject);

							string typesTemplate = (this.Cmd.DTE.Solution as EnvDTE80.Solution2).GetProjectItemTemplate("Types.xml", "CSharp");
							if (String.IsNullOrEmpty(typesTemplate))
								throw new Exception("Не установлен шаблон Types.xml");

							this.Cmd.DTEInfo.SelectedProject.ProjectItems.AddFromTemplate(typesTemplate, typesFileName);
							_TypesXmlItem = this.Cmd.DTEInfo.SelectedProject.ProjectItems.Item(typesFileName);
						}
					}
					__init_TypesXmlItem = true;
				}
				return _TypesXmlItem;
			}
		}

		/// <summary>
		/// Проверяет наличие файла Types.xml.
		/// </summary>
		public bool EnsureTypesXmlItem()
		{
			return this.TypesXmlItem != null;
		}

		/// <summary>
		/// Добавляет определение в XML.
		/// </summary>
		/// <param name="definition"></param>
		public void Add(ItemDefinition definition)
		{
			if (definition == null)
				throw new ArgumentNullException(nameof(definition));

			string typesXmlPath = this.TypesXmlItem.GetFullPath();

			//грузим существующий XML
			XmlDocument xdoc = new XmlDocument();
			xdoc.PreserveWhitespace = false;
			xdoc.LoadXml(File.ReadAllText(typesXmlPath));

			XmlNode typeDefinitionsNode = xdoc.SelectSingleNode("TypeDefinitions");
			if (typeDefinitionsNode == null)
				throw new Exception("Узел <TypeDefinitions> не найден");

			//находим соответствующую секцию для типа
			XmlNode section = typeDefinitionsNode.SelectSingleNode(definition.SectionName);
			if (section == null)
			{
				//создаём секцию
				section = xdoc.CreateElement(definition.SectionName);
				typeDefinitionsNode.AppendChild(section);
			}
			else
			{
				//ищем дубликаты в секции
				XmlNode duplicateNode = section.SelectSingleNode(definition.UniqueNodeXPath);
				if (duplicateNode != null)
				{
					if (WSSDeveloperPackage.ShowUserConfirmOkCancel($"В файле Types.xml уже есть определение по xpath: {definition.UniqueNodeXPath}.\n\nЗаменить?", "Дублирование"))
					{
						XmlNode parentNode;
						if ((parentNode = duplicateNode.ParentNode) == null)
							throw new NotificationException("Некорректный формат Types.xml");

						section.RemoveChild(parentNode);
					}
					else
						return;
				}
			}

			//вставляем XML-определение типа
			XmlDocument temp = new XmlDocument();
			temp.LoadXml(definition.CreateXml());
			section.AppendChild(xdoc.ImportNode(temp.FirstChild, true));

			//делаем чекаут Types.xml
			this.Cmd.DTE.CheckOutItem(this.TypesXmlItem);

			//сохраняем XML
			xdoc.Save(typesXmlPath);

			//открываем
			Window win = this.Cmd.DTE.ItemOperations.OpenFile(typesXmlPath, EnvDTE.Constants.vsViewKindTextView);
		}
	}
}
