using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSSConsulting.WSSDeveloper.Commands;

namespace WSSConsulting.WSSDeveloper
{
	/// <summary>
	/// Настройка расширения.
	/// </summary>
	internal class WSSOptions
	{
		/// <summary>
		/// Страница настройки.
		/// </summary>
		public WSSOptionsPage OptionsPage { get; private set; }

		/// <summary>
		/// Настройка расширения.
		/// </summary>
		/// <param name="optionsPage">Страница настройки.</param>
		public WSSOptions(WSSOptionsPage optionsPage)
		{
			this.OptionsPage = optionsPage ?? throw new ArgumentNullException(nameof(optionsPage));
		}

		/// <summary>
		/// Путь к папкам комплектов.
		/// </summary>
		public string PackageFolderPath
		{
			get
			{
				string folderPath = this.OptionsPage.PackageFolderPath;
				if (String.IsNullOrEmpty(folderPath))
					throw new NotificationException("Не задан путь к папкам комплектов.\nПроверьте настройку: Tools -> Options -> WSSDeveloper");

				return folderPath;
			}
		}

		/// <summary>
		/// Путь к папкам тестирования.
		/// </summary>
		public string TestFolderPath
		{
			get
			{
				string folderPath = this.OptionsPage.TestFolderPath;
				if (String.IsNullOrEmpty(folderPath))
					throw new Exception("Не задан путь к папкам тестирования.\nПроверьте настройку: Tools -> Options -> WSSDeveloper");

				return folderPath;
			}
		}

		/// <summary>
		/// Имя пользователя.
		/// </summary>
		public string UserName
		{
			get
			{
				string userName = this.OptionsPage.UserName;
				if (String.IsNullOrEmpty(userName))
				{
					try
					{
						userName = System.DirectoryServices.AccountManagement.UserPrincipal.Current?.DisplayName;

						string[] userNameParts = userName?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						if (userNameParts?.Length > 1)
						{
							userName = $"{userNameParts[0]} {userNameParts[1].First()}.";
						}
					}
					catch (Exception ex)
					{
						userName = null;
					}
				}

				if (String.IsNullOrEmpty(userName))
					userName = Environment.UserName;

				return userName;
			}
		}
	}
}
