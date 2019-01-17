using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;

namespace $rootnamespace$
{
    /// <summary>
    /// Сервис.
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [WebService]
    public class $safeitemrootname$ : WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello, world!";
        }
    }
}
