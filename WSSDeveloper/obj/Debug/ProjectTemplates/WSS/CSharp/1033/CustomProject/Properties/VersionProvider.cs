using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $projectname$
{
    /// <summary>
    /// Сведения о версии модуля.
    /// </summary>
    public class VersionProvider
    {
        /// <summary>
        /// Номер версии.
        /// </summary>
        public const string VersionNumber = "4.0.0.0";

        /// <summary>
        /// Полное название сборки.
        /// </summary>
        public const string AssemblyName = "$projectname$,Version=4.0.0.0,Culture=neutral,PublicKeyToken=$publickeytoken$";

        /// <summary>
        /// Название модуля.
        /// </summary>
        public const string ModuleName = "$projectname$";

        /// <summary>
        /// Относительный путь к модулю.
        /// </summary>
        public const string ModulePath = "/_layouts/WSS/$projectname$";
    }
}
