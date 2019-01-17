using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSConsulting.WSSDeveloper
{
    /// <summary>
    /// Список кодов команд.
    /// </summary>
    public class CommandsList
    {
        /// <summary>
        /// Добавить в Deploy/Manifest.
        /// </summary>
        public const uint AddToDeploy = 0x100;

        /// <summary>
        /// Выложить комплект.
        /// </summary>
        public const uint Deploy = 0x101;

        /// <summary>
        /// Сформировать Release.
        /// </summary>
        public const uint MakeRelease = 0x102;

        /// <summary>
        /// Анализ References.
        /// </summary>
        public const uint RefCheck = 0x103;

        /// <summary>
        /// Добавить в Types.xml.
        /// </summary>
        public const uint AddToTypesXml = 0x104;
    }
}
