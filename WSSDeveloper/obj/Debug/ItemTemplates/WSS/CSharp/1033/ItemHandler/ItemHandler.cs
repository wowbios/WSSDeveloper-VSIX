using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
using WSSC.V4.SYS.DBFramework;

namespace $rootnamespace$
{
    /// <summary>
    /// Обработчик $safeitemrootname$.
    /// </summary>
	public class $safeitemrootname$ : DBItemHandler
	{
        /// <summary>
        /// Обработчик $safeitemrootname$.
        /// </summary>
        public $safeitemrootname$(DBItemHandlerDefinition handlerDefinition, DBList list)
            : base(handlerDefinition, list) { }

        /// <summary>
        /// Обработчик, вызываемый перед обновлением элемента списка.
        /// </summary>
        /// <param name="operationProperties">Параметры операции обновления.</param>
        protected override void OnBeforeItemUpdate(DBItemOperation operationProperties)
        {
            base.OnBeforeItemUpdate(operationProperties);
    
            throw new NotImplementedException();
        }
	}
}