using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSSConsulting.WSSDeveloper.Commands
{
    /// <summary>
    /// Некритичная ошибка с текстом оповещения.
    /// </summary>
    internal class NotificationException : Exception
    {
        /// <summary>
        /// Некритичная ошибка с текстом оповещения.
        /// </summary>
        /// <param name="message">Текст.</param>
        /// <param name="parameters">Параметры для замены в тексте </param>
        public NotificationException(string message, params object[] parameters)
            : base(String.Format(message, parameters))
        {

        }
    }
}
