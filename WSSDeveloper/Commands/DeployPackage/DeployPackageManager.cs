using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using EnvDTE;

namespace WSSConsulting.WSSDeveloper.Commands
{
	/// <summary>
	/// Объект, реализующий логику команды деплоя проекта.
	/// </summary>
	internal class DeployPackageManager : WSSCommandBase
	{
		/// <summary>
		/// Срабатывает при выполнении команды.
		/// </summary>
		protected override void OnExecute()
		{
			DeployPackageDestinationForm form = new DeployPackageDestinationForm(this);
			form.Show(new Owner(this.DTE.MainWindow.HWnd));
			form.FormClosed += (s, e) =>
			{
				form.Dispose();
			};
		}

		private class Owner : System.Windows.Forms.IWin32Window
		{
			public Owner(int ownerHwnd)
			{
				this.Handle = (IntPtr)ownerHwnd;
			}
			public IntPtr Handle { get; }
		}
	}
}
