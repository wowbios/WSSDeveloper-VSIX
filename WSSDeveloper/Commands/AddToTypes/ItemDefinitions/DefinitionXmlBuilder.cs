using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes
{
    /// <summary>
    /// Построитель XML описания типа.
    /// </summary>
    internal class DefinitionXmlBuilder : IDisposable
    {
        /// <summary>
        /// Построитель строки.
        /// </summary>
        internal StringBuilder Builder { get; private set; }

        /// <summary>
        /// Название корневого элемента.
        /// </summary>
        internal string RootElementName { get; private set; }

        /// <summary>
        /// Построитель XML описания типа.
        /// </summary>
        /// <param name="builder">Построитель строки.</param>
        /// <param name="rootElementName">Название корневого элемента.</param>
        public DefinitionXmlBuilder(StringBuilder builder, string rootElementName)
        {
			if (String.IsNullOrEmpty(rootElementName))
                throw new ArgumentException("Argument is null or empty", nameof(rootElementName));
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.RootElementName = rootElementName;
            builder.AppendFormat("<{0}>", rootElementName);
        }

        /// <summary>
        /// Дописывает строку.
        /// </summary>
        /// <param name="value"></param>
        public void Append(string value)
        {
            this.Builder.Append(value);
        }

        /// <summary>
        /// Дописывает XML элемент.
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="value"></param>
        public void Append(string tagName, string value)
        {
            this.Builder.AppendFormat("<{0}>{1}</{0}>", tagName, value);
        }

        /// <summary>
        /// Дописывает значение именованного поля.
        /// </summary>
        /// <param name="box"></param>
        public void Append(NamedBox box)
        {
            if (box == null) throw new ArgumentNullException(nameof(box));

            this.Append(box.XmlValue);
        }

        /// <summary>
        /// Дописывает название класса в элемент <![CDATA[<ClassName>]]>;
        /// </summary>
        /// <param name="class"></param>
        public void AppendClassName(ClassMetadata @class)
        {
            if (@class == null)
                throw new ArgumentNullException(nameof(@class));

            this.Append("ClassName", @class.Class.FullName);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Builder.AppendFormat("</{0}>", this.RootElementName);
        }
    }
}
