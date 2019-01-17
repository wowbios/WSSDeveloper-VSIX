using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using WSSC.V4.SYS.DBFramework;

namespace $rootnamespace$
{
    /// <summary>
    /// Обработчик Ajax запросов $safeitemrootname$.
    /// </summary>
    public class $safeitemrootname$ : Page
    {
        private bool __init_AppContext;
        private DBAppContext _AppContext;
        /// <summary>
        /// Веб контекст приложения.
        /// </summary>
        private DBAppContext AppContext
        {
            get
            {
                if (!__init_AppContext)
                {
                    _AppContext = DBAppContext.Current;
                    __init_AppContext = true;
                }
                return _AppContext;
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                // Проверить, что текущий пользователь авторизован
                this.AppContext.CheckCurrentUser(false);

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.ToString());
            }
        }
    }
}