using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands
{
    /// <summary>
    /// Метаданные класса.
    /// </summary>
    internal class ClassMetadata
    {
        /// <summary>
        /// Метаданные класса.
        /// </summary>
        /// <param name="class">Класс.</param>
        internal ClassMetadata(CodeClass @class)
        {
			this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
        }

        /// <summary>
        /// Класс.
        /// </summary>
        internal CodeClass Class { get; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this.Class.FullName;
        }
    }
}
