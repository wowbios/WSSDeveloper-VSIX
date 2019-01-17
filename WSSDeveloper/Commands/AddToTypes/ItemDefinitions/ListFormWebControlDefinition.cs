using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions
{
	/// <summary>
	/// Определение контрола формы списка.
	/// </summary>
	internal class ListFormWebControlDefinition : ItemDefinition
	{
		private ListFormWebControlDefinition(ClassMetadata metadata)
			: base(metadata)
		{
		}

		/// <summary>
		/// Название XML секции для данных типов.
		/// </summary>
		internal override string SectionName => "ListFormWebControlDefinitions";

		/// <summary>
		/// Создаёт XML описание элемента.
		/// </summary>
		/// <returns></returns>
		internal override string CreateXml()
		{
			StringBuilder sb = new StringBuilder(4096);
			using (DefinitionXmlBuilder xb = new DefinitionXmlBuilder(sb, "ListFormWebControlDefinition"))
			{
				xb.Append(_Type);
				xb.AppendClassName(this.Metadata);
				CodeClass factoryClass = this.FindControlFactory();
				if (factoryClass != null)
					xb.Append("FactoryClassName", this.Metadata.Class.FullName + "+" + factoryClass.Name);
				else if (!WSSDeveloperPackage.ShowUserConfirmOkCancel("Фабрика контрола не задана, продолжить?", "Не задана фабрика"))
				{
					throw new NotificationException("Операция прервана");
				}

				xb.Append(_DisplayName);
			}
			return sb.ToString();
		}

		private readonly NamedBox _Type = new NamedBox("Type (уникальное название)", "Type");
		private readonly NamedBox _DisplayName = new NamedBox("Название", "DisplayName");

		/// <summary>
		/// Контролы страницы.
		/// </summary>
		internal override Control[] PageControls => new Control[]
		{
			_Type.GroupBox,
			_DisplayName.GroupBox
		};

		/// <summary>
		/// Находит класс фабрики контрола.
		/// </summary>
		/// <returns></returns>
		private CodeClass FindControlFactory()
		{
			CodeClass factoryClass = this.Metadata.Class.Children.FirstOrDefaultOfType<CodeClass>(x => x.IsDerivedFrom["WSSC.V4.SYS.DBFramework.DBListFormWebControlFactory"]);
			return factoryClass;
		}

		/// <summary>
		/// Валидирует поля.
		/// </summary>
		/// <returns></returns>
		internal override void Validate()
		{
			_Type.Required();
			_DisplayName.Required();
		}

		/// <summary>
		/// XPath уникального узла для определения.
		/// </summary>
		internal override string UniqueNodeXPath => $"ListFormWebControlDefinition/Type[.={_Type.XPathValue}]";

		/// <summary>
		/// Фабрика.
		/// </summary>
		internal class Factory : ItemDefinitionFactory
		{
			/// <summary>
			/// Создаёт тип.
			/// </summary>
			/// <param name="metadata"></param>
			/// <returns></returns>
			protected override ItemDefinition CreateDefinition(ClassMetadata metadata)
			{
				return new ListFormWebControlDefinition(metadata);
			}

			/// <summary>
			/// Фабрика.
			/// </summary>
			public Factory()
				: base("Контрол списка")
			{
			}
		}
	}
}
