using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes
{
    /// <summary>
    /// Определение типа.
    /// </summary>
    internal abstract class ItemDefinition
    {
        /// <summary>
        /// Метаданные класса.
        /// </summary>
        public ClassMetadata Metadata { get; private set; }

        /// <summary>
        /// Определение типа.
        /// </summary>
        /// <param name="metadata">Метаданные класса.</param>
        protected ItemDefinition(ClassMetadata metadata)
        {
			this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        /// <summary>
        /// Контролы страницы.
        /// </summary>
        internal virtual Control[] PageControls => null;

	    /// <summary>
        /// Название XML секции для данных типов.
        /// </summary>
        internal abstract string SectionName { get; }

        /// <summary>
        /// XPath уникального узла для определения.
        /// </summary>
        internal abstract string UniqueNodeXPath { get; }

        /// <summary>
        /// Создаёт XML описание элемента.
        /// </summary>
        /// <returns></returns>
        internal abstract string CreateXml();

        /// <summary>
        /// Валидирует поля.
        /// </summary>
        /// <returns></returns>
        internal abstract void Validate();
    }
}
