using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WSSConsulting.WSSDeveloper
{
	/// <summary>
	/// Контрол страницы настроек расширения.
	/// </summary>
	public partial class WSSOptionsPageControl : UserControl
	{
		/// <summary>
		/// Опции расширения.
		/// </summary>
		private readonly WSSOptionsPage OptionsPage;

		/// <summary>
		/// Контрол страницы настроек расширения.
		/// </summary>
		/// <param name="optionsPage">Опции расширения.</param>
		public WSSOptionsPageControl(WSSOptionsPage optionsPage)
		{
			OptionsPage = optionsPage ?? throw new ArgumentNullException(nameof(optionsPage));

			//инициализируем компоненты контрола
			this.InitializeComponent();

			//инициализируем опции
			this.Initialize();
		}

		/// <summary>
		/// Инициализация опций расширения.
		/// </summary>
		private void Initialize()
		{
			TestTextBox.Text = OptionsPage.TestFolderPath;
			PackagesTextBox.Text = OptionsPage.PackageFolderPath;
			UserNameTextBox.Text = OptionsPage.UserName;

			//Добавляем столбец с кнопкой "Удалить" для ListView
			ListViewExtender extender = new ListViewExtender(this.ShortcutsListView);
			var buttonColumn = new ListViewButtonColumn(2);
			buttonColumn.Click += DeleteAliasButtonColumn_Click;
			buttonColumn.FixedWidth = true;
			extender.AddColumn(buttonColumn);

			int listViewWidth = this.ShortcutsListView.Width;
			columnHeader_projectName.Width = (int)(listViewWidth * 0.4);
			columnHeader_projectAlias.Width = (int)(listViewWidth * 0.4);
			columnHeader_buttons.Width = (int)(listViewWidth * 0.2);

			Dictionary<string, string> shortCuts = this.OptionsPage.TestTemplateShortcuts;
			if (shortCuts != null)
			{
				foreach (KeyValuePair<string, string> kvp in shortCuts)
					this.AddShortcutToListView(kvp.Key, kvp.Value);
			}
		}

		private void DeleteAliasButtonColumn_Click(object sender, ListViewColumnMouseEventArgs e)
		{
			string name = e.Item.Name;
			var shortCuts = this.OptionsPage.TestTemplateShortcuts;
			if (shortCuts != null)
				if (shortCuts.ContainsKey(name))
				{
					shortCuts.Remove(name);
				}

			e.Item.Remove();
		}

		/// <summary>
		/// Изменение пути к папке комплектов.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PackagesTextBox_TextChanged(object sender, EventArgs e)
		{
			OptionsPage.PackageFolderPath = PackagesTextBox.Text;
		}

		/// <summary>
		/// Изменение пути к папке тестирования.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TestTextBox_TextChanged(object sender, EventArgs e)
		{
			OptionsPage.TestFolderPath = TestTextBox.Text;
		}

		/// <summary>
		/// Клик по "Обзор ..." для папок комплектов.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PackagesButton_Click(object sender, EventArgs e)
		{
			string folderPath;
			if (this.ChooseFolderDialog(PackagesTextBox.Text, out folderPath))
				PackagesTextBox.Text = folderPath;
		}

		/// <summary>
		/// Клик по "Обзор ..." для папок тестирования.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TestButton_Click(object sender, EventArgs e)
		{
			string folderPath;
			if (this.ChooseFolderDialog(TestTextBox.Text, out folderPath))
				TestTextBox.Text = folderPath;
		}

		/// <summary>
		/// Возвращает результат выбора папки.
		/// </summary>
		/// <param name="initialDir"></param>
		/// <param name="folderPath"></param>
		/// <returns></returns>
		private bool ChooseFolderDialog(string initialDir, out string folderPath)
		{
			using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog())
			{
				fileDialog.IsFolderPicker = true;
				if (!String.IsNullOrEmpty(initialDir))
					fileDialog.InitialDirectory = initialDir;

				if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
				{
					folderPath = fileDialog.FileName;
					return true;
				}
			}

			folderPath = null;
			return false;
		}

		/// <summary>
		/// При изменении имени пользователя.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UserNameTextBox_TextChanged(object sender, EventArgs e)
		{
			OptionsPage.UserName = this.UserNameTextBox.Text;
		}

		private void ProjectNameVar_TextChanged(object sender, EventArgs e)
		{
			this.ChangeAliasSaveButtonState();
		}

		private void ProjectAliasVar_TextChanged(object sender, EventArgs e)
		{
			this.ChangeAliasSaveButtonState();
		}

		private void ChangeAliasSaveButtonState()
		{
			this.SaveAliasButton.Enabled = !String.IsNullOrEmpty(this.ProjectAliasVar.Text)
				&&
				!String.IsNullOrEmpty(this.ProjectNameVar.Text);
		}

		private void SaveAliasButton_Click(object sender, EventArgs e)
		{
			string projName = this.ProjectNameVar.Text;
			string projNameLow = projName?.ToLower() ?? String.Empty;
			string projAlias = this.ProjectAliasVar.Text ?? String.Empty;
			Dictionary<string, string> shortcuts = this.OptionsPage.TestTemplateShortcuts
				?? (this.OptionsPage.TestTemplateShortcuts = new Dictionary<string, string>());

			if (!String.IsNullOrEmpty(projNameLow) && !String.IsNullOrEmpty(projAlias))
			{
				if (shortcuts.ContainsKey(projNameLow))
					WSSDeveloperPackage.ShowUserError($"Указана повторяющаяся настройка для проекта {projNameLow}",
						"Нельзя указывать повторяющиеся строки");
				else
				{
					shortcuts.Add(projNameLow, projAlias);
					this.AddShortcutToListView(projNameLow, projAlias);
				}

				this.ProjectAliasVar.Text = null;
				this.ProjectNameVar.Text = null;
			}
		}

		private void AddShortcutToListView(string projName, string alias)
		{
			var listViewItem = new ListViewItem(new string[] { projName, alias })
			{
				Name = projName
			};
			listViewItem.SubItems.Add("Удалить");
			this.ShortcutsListView.Items.Add(listViewItem);
		}
	}
}
