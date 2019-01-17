using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes
{
    /// <summary>
    /// Фабрика создания типов.
    /// </summary>
    internal abstract class ItemDefinitionFactory
    {
        /// <summary>
        /// Отображаемое название типа.
        /// </summary>
        internal string DisplayName { get; private set; }

        /// <summary>
        /// Фабрика создания типов.
        /// </summary>
        /// <param name="displayName">Отображаемое название типа.</param>
        protected ItemDefinitionFactory(string displayName)
        {
            if (String.IsNullOrEmpty(displayName))
                throw new ArgumentNullException(nameof(displayName));

            this.DisplayName = displayName;
        }

        /// <summary>
        /// Создаёт тип.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public ItemDefinition Create(ClassMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            return this.CreateDefinition(metadata);
        }

        /// <summary>
        /// Создаёт тип.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        protected abstract ItemDefinition CreateDefinition(ClassMetadata metadata);

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
