using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions
{
    /// <summary>
    /// Определение типа поля.
    /// </summary>
    internal class FieldDefinition : ItemDefinition
    {
        /// <summary>
        /// Определение типа поля.
        /// </summary>
        /// <param name="metadata">Метаданные класса.</param>
        private FieldDefinition(ClassMetadata metadata)
            : base(metadata)
        {
        }

        /// <summary>
        /// Название XML секции для данных типов.
        /// </summary>
        internal override string SectionName => "FieldDefinitions";

	    /// <summary>
        /// Создаёт XML описание элемента.
        /// </summary>
        /// <returns></returns>
        internal override string CreateXml()
        {
            StringBuilder sb = new StringBuilder(4096);
            using (DefinitionXmlBuilder xb = new DefinitionXmlBuilder(sb, "FieldDefinition"))
            {
                xb.Append(_Type);
                xb.AppendClassName(this.Metadata);
                // TODO: [AV] CR: ADD FACTORY

                xb.Append(_TypeDisplayName);
                xb.Append("UserCreatable", _UserCreatable.Checked.ToString().ToLower());
                xb.Append(_ListFormControlSource);
            }
            return sb.ToString();
        }

        private readonly NamedBox _Type = new NamedBox("Тип (уникальное название)", "Type");
        private readonly NamedBox _TypeDisplayName = new NamedBox("Отображаемое название типа", "TypeDisplayName");
        private readonly CheckBox _UserCreatable = new CheckBox();
        private readonly NamedBox _ListFormControlSource = new NamedBox("Адрес контрола", "ListFormControlSource");

        /// <summary>
        /// Контролы страницы.
        /// </summary>
        internal override Control[] PageControls => new Control[]
        {
	        _Type.GroupBox,
	        _TypeDisplayName.GroupBox,
	        _UserCreatable,
	        _ListFormControlSource.GroupBox
        };

	    /// <summary>
        /// Валидирует поля.
        /// </summary>
        /// <returns></returns>
        internal override void Validate()
        {
            _Type.Required();
            _TypeDisplayName.Required();
            _ListFormControlSource.Required();
        }

        /// <summary>
        /// XPath уникального узла для определения.
        /// </summary>
        internal override string UniqueNodeXPath => $"FieldDefinition/Type[.={_Type.XPathValue}]";

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
                return new FieldDefinition(metadata);
            }

            /// <summary>
            /// Фабрика.
            /// </summary>
            public Factory()
                : base("Поле")
            {
            }
        }
    }
}
