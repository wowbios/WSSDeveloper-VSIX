using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;
using WSSC.V4.SYS.DBFramework;

namespace $rootnamespace$
{
    /// <summary>
    /// Контрол $safeitemrootname$.
    /// </summary>
	public class $safeitemrootname$ : DBListFormWebControl
	{
        /// <summary>
        /// Контрол $safeitemrootname$.
        /// </summary>
        protected $safeitemrootname$(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            : base(metadata, listForm) { }
        
        /// <summary>
        /// Фабрика для создания контрола.
        /// </summary>
        protected class Factory : DBListFormWebControlFactory
        {
            /// <summary>
            /// Создает экземпляр контрола на форме элемента списка.
            /// </summary>
            /// <param name="metadata">Метаданные контрола.</param>
            /// <param name="listForm">Форма элемента списка.</param>
            /// <returns/>
            protected override DBListFormWebControl CreateListFormWebControl(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            {
                return new $safeitemrootname$(metadata, listForm);
            }
        }

        /// <summary>
        /// Вызывается при инициализации формы, до инициализации полей.
        /// </summary>
        protected override void OnListFormInit()
        {
            this.AppContext.ScriptManager.RegisterResource(@"/$projectitem_rel_path_web$/$ListFormControlPrefix$_$safeitemrootname$.js", VersionProvider.ModulePath);
        }
	}
}