using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace WSSConsulting.WSSDeveloper.Commands
{
    /// <summary>
    /// Базовый класс команды.
    /// </summary>
    internal abstract class WSSCommandBase
    {
        /// <summary>
        /// Базовый класс команды.
        /// </summary>
        protected WSSCommandBase()
        {

        }

        /// <summary>
        /// Расширение студии.
        /// </summary>
        internal WSSDeveloperPackage Package { get; private set; }

        /// <summary>
        /// Выполняет инициализацию.
        /// </summary>
        /// <param name="wssPackage"></param>
        internal void Init(WSSDeveloperPackage wssPackage)
        {
			this.Package = wssPackage ?? throw new ArgumentNullException(nameof(wssPackage));

            this.Inited = true;
        }

        /// <summary>
        /// Проверяет
        /// </summary>
        private void CheckInit()
        {
            if (!this.Inited)
                throw new Exception("Команда не была инициализирована");
        }

        private bool Inited;

        private bool __init_DTEInfo;
        private DTEInfo _DTEInfo;
        /// <summary>
        /// Вспомогательная обёртка над DTE.
        /// </summary>
        internal DTEInfo DTEInfo
        {
            get
            {
                if (!__init_DTEInfo)
                {
                    this.CheckInit();

                    _DTEInfo = new DTEInfo(this.Package.DTE);
                    __init_DTEInfo = true;
                }
                return _DTEInfo;
            }
        }

        /// <summary>
        /// DTE.
        /// </summary>
        internal DTE DTE => this.Package.DTE;

	    /// <summary>
        /// Настройки.
        /// </summary>
        internal WSSOptions Options => this.Package.Options;

	    /// <summary>
        /// Исполняет команду.
        /// </summary>
        public void Execute()
        {
            this.CheckInit();
            this.OnExecute();
        }

        /// <summary>
        /// Срабатывает при выполнении команды.
        /// </summary>
        protected abstract void OnExecute();

        /// <summary>
        /// Записывает текст в Output.
        /// </summary>
        /// <param name="message"></param>
        protected void WriteToOutput(string message)
        {
            this.Package.WriteToOutput(message);
        }
    }
}
