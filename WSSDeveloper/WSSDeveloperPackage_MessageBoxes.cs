using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSSConsulting.WSSDeveloper
{
	/// <summary>
	/// Часть расширения, отвечающая за всплывающие сообщения.
	/// </summary>
	public partial class WSSDeveloperPackage
	{
		/// <summary>
		/// Показывает пользователю всплывающее диалоговое сообщение для вопроса.
		/// Кнопки [Да, Нет].
		/// </summary>
		/// <param name="message">Сообщение.</param>
		/// <param name="title">Заголовок.</param>
		/// <returns></returns>
		internal static bool ShowUserConfirmYesNo(string message, string title)
		{
			MessageBoxManager.RegisterRU();
			return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes;
		}

		/// <summary>
		/// Показывает пользователю всплывающее диалоговое сообщение для вопроса.
		/// Кнопки [ОК, Отмена].
		/// </summary>
		/// <param name="message">Сообщение.</param>
		/// <param name="title">Заголовок.</param>
		/// <returns></returns>
		internal static bool ShowUserConfirmOkCancel(string message, string title)
		{
			MessageBoxManager.RegisterRU();
			return MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
				== DialogResult.OK;
		}

		/// <summary>
		/// Показывает пользователю всплывающее диалоговое сообщение с предупреждением.
		/// </summary>
		/// <param name="message">Сообщение.</param>
		/// <param name="title">Заголовок.</param>
		internal static void ShowUserWarn(string message, string title)
		{
			MessageBoxManager.RegisterRU();
			MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// Показывает пользователю всплывающее диалоговое сообщение с ошибкой.
		/// </summary>
		/// <param name="message">Сообщение.</param>
		/// <param name="title">Заголовок.</param>
		internal static void ShowUserError(string message, string title)
		{
			MessageBoxManager.RegisterRU();
			MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
