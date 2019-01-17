using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions
{
    /// <summary>
    /// Определение типа процедуры.
    /// </summary>
    internal class JobDefinition : ItemDefinition
    {
        /// <summary>
        /// Определение типа процедуры.
        /// </summary>
        /// <param name="metadata">Метаданные класса.</param>
        private JobDefinition(ClassMetadata metadata)
            : base(metadata)
        {
        }

        /// <summary>
        /// Название XML секции для данных типов.
        /// </summary>
        internal override string SectionName => "JobDefinitions";

	    /// <summary>
        /// Создаёт XML описание элемента.
        /// </summary>
        /// <returns></returns>
        internal override string CreateXml()
        {
            StringBuilder sb = new StringBuilder(4096);
            using (DefinitionXmlBuilder xb = new DefinitionXmlBuilder(sb, "JobDefinition"))
            {
                xb.Append(_Type);
                xb.AppendClassName(this.Metadata);
                xb.Append(_DisplayName);
            }
            return sb.ToString();
        }

        private readonly NamedBox _Type = new NamedBox("Type (уникальное название)", "Type");
        private readonly NamedBox _DisplayName = new NamedBox("Название", "JobDisplayName");

        /// <summary>
        /// Контролы страницы.
        /// </summary>
        internal override Control[] PageControls => new Control[]
        {
	        _Type.GroupBox,
	        _DisplayName.GroupBox
        };

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
        internal override string UniqueNodeXPath => $"JobDefinition/Type[.={_Type.XPathValue}]";

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
                return new JobDefinition(metadata);
            }

            /// <summary>
            /// Фабрика.
            /// </summary>
            public Factory()
                : base("Процедура")
            {
            }
        }
    }
}
