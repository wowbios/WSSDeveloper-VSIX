using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes.ItemDefinitions
{
    /// <summary>
    /// Именованное поле.
    /// </summary>
    internal class NamedBox
    {
        /// <summary>
        /// Соответствующий XML элемент.
        /// </summary>
        public string XmlElement { get; private set; }

        /// <summary>
        /// Контрол GrooupBox.
        /// </summary>
        public GroupBox GroupBox { get; private set; }

        /// <summary>
        /// Соответствующий текстбокс.
        /// </summary>
        public TextBox TextBox { get; private set; }

        /// <summary>
        /// Именованное поле.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xmlElement"></param>
        internal NamedBox(string name, string xmlElement)
        {
			this.XmlElement = xmlElement ?? throw new ArgumentNullException(nameof(xmlElement));

            this.GroupBox = new GroupBox()
            {
                Text = name,
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowOnly,
                Size = new System.Drawing.Size(335, 47),
                Location = new System.Drawing.Point(0, 0),

            };
            this.TextBox = new TextBox()
            {
                Location = new System.Drawing.Point(9, 20),
                Size = new System.Drawing.Size(278, 20)
            };
            this.GroupBox.Controls.Add(this.TextBox);
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Value => this.TextBox.Text;

	    /// <summary>
        /// Значение для XML.
        /// </summary>
        public string XmlValue => String.Format("<{0}>{1}</{0}>", this.XmlElement, this.TextBox.Text);

	    /// <summary>
        /// Значение для XPath.
        /// </summary>
        public string XPathValue => $"'{this.Value}'";

	    /// <summary>
        /// Требует наличие значения.
        /// </summary>
        public void Required()
        {
            if (String.IsNullOrEmpty(this.Value))
                throw new EmptyFormFieldException(this);
        }
    }
}
