using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
using WSSC.V4.SYS.DBFramework;

namespace $rootnamespace$
{
    /// <summary>
    /// Периодическая процедура "$safeitemrootname$".
    /// </summary>
	public class $safeitemrootname$ : DBJob
	{
        /// <summary>
        /// Периодическая процедура "$safeitemrootname$".
        /// </summary>
        public $safeitemrootname$(DBJobDefinition jobDefinition, DBSite site)
            : base(jobDefinition, site) { }

        /// <summary>
        /// Выполняет работу процедуры.
        /// </summary>
        public override void Execute()
        {
            base.Execute();
        }
	}
}