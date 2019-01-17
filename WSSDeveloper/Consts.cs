using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSConsulting.WSSDeveloper
{
    internal class Constants
    {
        /// <summary>
        /// Название папки Deploy
        /// </summary>
        public const string DeployFilesFolderName = "Deploy";

        /// <summary>
        /// Название файла манифеста.
        /// </summary>
        public const string ManifestFileName = "Manifest.xml";

        /// <summary>
        /// Название файла DeployParams.
        /// </summary>
        public const string DeployParamsFileName = "DeployParams.txt";

        /// <summary>
        /// Название папки Release.
        /// </summary>
        public const string ReleaseFolderName = "Release";

        /// <summary>
        /// Шаблон даты в логе.
        /// </summary>
        public const string LogDatePattern = @"^\d{4}.\d\d.\d\d[\n\r\t\s]*$";

        /// <summary>
        /// Шаблон имени проекта в логе.
        /// </summary>
        public const string ProjectNamePattern = @"^[A-z\d\.]+[\n\r\t\s]*$";


        public class Commands
        {
            public class AnalyzeReferences
            {
                public const string ClientChangesPathPart = "2.WSSDocs.ClientChanges";
                public const string CurrentVersionSystem = "1.System.v4";
                public const string CurrentVersionEDMS = "2.EDMS.v4";
                public const string FixedRelease = "2.WSSDocs";
                public const string References = "1.References";
            }
        }

        /// <summary>
        /// Скрипт установки модуля.
        /// </summary>
        public const string SetupBatContent = @"
@ECHO OFF
@SET WspInstaller=""c:\program files\common files\microsoft shared\web server extensions\12\TEMPLATE\LAYOUTS\WSS\WspInstaller.exe""
IF NOT EXIST %WspInstaller% @SET WspInstaller=""c:\program files\common files\microsoft shared\web server extensions\14\TEMPLATE\LAYOUTS\WSS\WspInstaller.exe""
IF NOT EXIST %WspInstaller% @SET WspInstaller=""..\WspInstaller.exe""
IF NOT EXIST %WspInstaller% @SET WspInstaller=""WspInstaller.exe""
IF EXIST %WspInstaller% (%WspInstaller% -setup -single -recycle) ELSE (ECHO. & ECHO Could not find WspInstaller.exe. Copy WspInstaller.exe in the current executable folder, or in the folder level above, or in the folder LAYOUTS\WSS)
PAUSE";

        /// <summary>
        /// Скрипт удаления модуля.
        /// </summary>
        public const string UninstallBatContent = @"
@ECHO OFF
@SET WspInstaller=""c:\program files\common files\microsoft shared\web server extensions\12\TEMPLATE\LAYOUTS\WSS\WspInstaller.exe""
IF NOT EXIST %WspInstaller% @SET WspInstaller=""c:\program files\common files\microsoft shared\web server extensions\14\TEMPLATE\LAYOUTS\WSS\WspInstaller.exe""
IF NOT EXIST %WspInstaller% @SET WspInstaller=""..\WspInstaller.exe""
IF NOT EXIST %WspInstaller% @SET WspInstaller=""WspInstaller.exe""
IF EXIST %WspInstaller% (%WspInstaller% -uninstall -single -recycle) ELSE (ECHO. & ECHO Could not find WspInstaller.exe. Copy WspInstaller.exe in the current executable folder, or in the folder level above, or in the folder LAYOUTS\WSS)
PAUSE";
    }
}
