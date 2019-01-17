using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions
{
    /// <summary>
    /// Определение типа DataObject.
    /// </summary>
    internal class DataObjectDefinition : ItemDefinition
    {
        /// <summary>
        /// Определение типа DataObject.
        /// </summary>
        /// <param name="metadata">Метаданные класса.</param>
        private DataObjectDefinition(ClassMetadata metadata)
            : base(metadata)
        {
        }

        /// <summary>
        /// Название XML секции для данных типов.
        /// </summary>
        internal override string SectionName => "DataObjectTypes";

	    /// <summary>
        /// Создаёт XML описание элемента.
        /// </summary>
        /// <returns></returns>
        internal override string CreateXml()
        {
            return $"<DataObjectType>{this.Metadata.Class.FullName}</DataObjectType>";
        }

        /// <summary>
        /// Валидирует поля.
        /// </summary>
        /// <returns></returns>
        internal override void Validate()
        {
        }

        /// <summary>
        /// XPath уникального узла для определения.
        /// </summary>
        internal override string UniqueNodeXPath => $"DataObjectType[.='{this.Metadata.Class.FullName}']";

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
                return new DataObjectDefinition(metadata);
            }

            /// <summary>
            /// Фабрика.
            /// </summary>
            public Factory()
                : base("DataObject")
            {
            }
        }
    }
}
