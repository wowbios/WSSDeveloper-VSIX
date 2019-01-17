using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions
{
    internal class ItemHandlerDefinition : ItemDefinition
    {
        public ItemHandlerDefinition(ClassMetadata metadata)
            : base(metadata)
        {
        }

        /// <summary>
        /// Название XML секции для данных типов.
        /// </summary>
        internal override string SectionName => "ItemHandlerDefinitions";

	    /// <summary>
        /// Создаёт XML описание элемента.
        /// </summary>
        /// <returns></returns>
        internal override string CreateXml()
        {
            StringBuilder sb = new StringBuilder(4096);
            using (DefinitionXmlBuilder xb = new DefinitionXmlBuilder(sb, "ItemHandlerDefinition"))
            {
                xb.Append(_Type);
                xb.AppendClassName(this.Metadata);
                xb.Append(_HandlerDisplayName);
            }
            return sb.ToString();
        }

        private readonly NamedBox _Type = new NamedBox("Type (уникальное название)", "Type");
        private readonly NamedBox _HandlerDisplayName = new NamedBox("Название", "HandlerDisplayName");

        /// <summary>
        /// Контролы страницы.
        /// </summary>
        internal override Control[] PageControls => new Control[]
        {
	        _Type.GroupBox,
	        _HandlerDisplayName.GroupBox
        };

	    /// <summary>
        /// Валидирует поля.
        /// </summary>
        /// <returns></returns>
        internal override void Validate()
        {
            _Type.Required();
            _HandlerDisplayName.Required();
        }

        /// <summary>
        /// XPath уникального узла для определения.
        /// </summary>
        internal override string UniqueNodeXPath => $"ItemHandlerDefinition/Type[.={_Type.XPathValue}]";

	    internal class Factory : ItemDefinitionFactory
        {
            protected override ItemDefinition CreateDefinition(ClassMetadata metadata)
            {
                return new ItemHandlerDefinition(metadata);
            }

            public Factory()
                : base("Обработчик элемента")
            {
            }
        }
    }
}
