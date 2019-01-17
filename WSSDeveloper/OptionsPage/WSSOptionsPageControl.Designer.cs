namespace WSSConsulting.WSSDeveloper
{
	partial class WSSOptionsPageControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PackagesTextBox = new System.Windows.Forms.TextBox();
			this.PackagesButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.TestButton = new System.Windows.Forms.Button();
			this.TestTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.UserNameTextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.ShortcutsListView = new System.Windows.Forms.ListView();
			this.columnHeader_projectName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_projectAlias = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader_buttons = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ProjectNameVar = new System.Windows.Forms.TextBox();
			this.ProjectAliasVar = new System.Windows.Forms.TextBox();
			this.SaveAliasButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// PackagesTextBox
			// 
			this.PackagesTextBox.Location = new System.Drawing.Point(6, 27);
			this.PackagesTextBox.Name = "PackagesTextBox";
			this.PackagesTextBox.Size = new System.Drawing.Size(274, 20);
			this.PackagesTextBox.TabIndex = 1;
			this.PackagesTextBox.TextChanged += new System.EventHandler(this.PackagesTextBox_TextChanged);
			// 
			// PackagesButton
			// 
			this.PackagesButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.PackagesButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.PackagesButton.Location = new System.Drawing.Point(286, 23);
			this.PackagesButton.Name = "PackagesButton";
			this.PackagesButton.Size = new System.Drawing.Size(63, 26);
			this.PackagesButton.TabIndex = 2;
			this.PackagesButton.Text = "Обзор ...";
			this.PackagesButton.UseVisualStyleBackColor = true;
			this.PackagesButton.Click += new System.EventHandler(this.PackagesButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(145, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Путь к папкам комплектов";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(154, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Путь к папкам тестирования";
			// 
			// TestButton
			// 
			this.TestButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.TestButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.TestButton.Location = new System.Drawing.Point(286, 114);
			this.TestButton.Name = "TestButton";
			this.TestButton.Size = new System.Drawing.Size(63, 26);
			this.TestButton.TabIndex = 5;
			this.TestButton.Text = "Обзор ...";
			this.TestButton.UseVisualStyleBackColor = true;
			this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
			// 
			// TestTextBox
			// 
			this.TestTextBox.Location = new System.Drawing.Point(6, 118);
			this.TestTextBox.Name = "TestTextBox";
			this.TestTextBox.Size = new System.Drawing.Size(274, 20);
			this.TestTextBox.TabIndex = 4;
			this.TestTextBox.TextChanged += new System.EventHandler(this.TestTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label3.Location = new System.Drawing.Point(6, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(350, 26);
			this.label3.TabIndex = 7;
			this.label3.Text = "Укажите путь к папке, в которой находятся папки с комплектами, \r\nсгруппированные " +
	"по заказчикам";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label4.Location = new System.Drawing.Point(6, 143);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(347, 26);
			this.label4.TabIndex = 8;
			this.label4.Text = "Укажите путь к папке, в которой находятся папки с комплектами \r\nдля тестирования";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label5.Location = new System.Drawing.Point(6, 225);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(294, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Фамилия и имя в краткой форме. Например: Иванов И.";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 184);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "Фамилия Имя";
			// 
			// UserNameTextBox
			// 
			this.UserNameTextBox.Location = new System.Drawing.Point(6, 200);
			this.UserNameTextBox.Name = "UserNameTextBox";
			this.UserNameTextBox.Size = new System.Drawing.Size(343, 20);
			this.UserNameTextBox.TabIndex = 9;
			this.UserNameTextBox.TextChanged += new System.EventHandler(this.UserNameTextBox_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 254);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(222, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "Замены для шаблона папки тестирования";
			// 
			// ShortcutsListView
			// 
			this.ShortcutsListView.AutoArrange = false;
			this.ShortcutsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader_projectName,
			this.columnHeader_projectAlias,
			this.columnHeader_buttons});
			this.ShortcutsListView.FullRowSelect = true;
			this.ShortcutsListView.GridLines = true;
			this.ShortcutsListView.Location = new System.Drawing.Point(6, 270);
			this.ShortcutsListView.Name = "ShortcutsListView";
			this.ShortcutsListView.Size = new System.Drawing.Size(343, 97);
			this.ShortcutsListView.TabIndex = 15;
			this.ShortcutsListView.UseCompatibleStateImageBehavior = false;
			this.ShortcutsListView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader_projectName
			// 
			this.columnHeader_projectName.Text = "Проект";
			this.columnHeader_projectName.Width = 140;
			// 
			// columnHeader_projectAlias
			// 
			this.columnHeader_projectAlias.Text = "Сокращение";
			this.columnHeader_projectAlias.Width = 130;
			// 
			// columnHeader_buttons
			// 
			this.columnHeader_buttons.Text = "Действия";
			this.columnHeader_buttons.Width = 68;
			// 
			// ProjectNameVar
			// 
			this.ProjectNameVar.Location = new System.Drawing.Point(9, 373);
			this.ProjectNameVar.Name = "ProjectNameVar";
			this.ProjectNameVar.Size = new System.Drawing.Size(134, 20);
			this.ProjectNameVar.TabIndex = 16;
			this.ProjectNameVar.TextChanged += new System.EventHandler(this.ProjectNameVar_TextChanged);
			// 
			// ProjectAliasVar
			// 
			this.ProjectAliasVar.Location = new System.Drawing.Point(149, 373);
			this.ProjectAliasVar.Name = "ProjectAliasVar";
			this.ProjectAliasVar.Size = new System.Drawing.Size(131, 20);
			this.ProjectAliasVar.TabIndex = 17;
			this.ProjectAliasVar.TextChanged += new System.EventHandler(this.ProjectAliasVar_TextChanged);
			// 
			// SaveAliasButton
			// 
			this.SaveAliasButton.Location = new System.Drawing.Point(286, 371);
			this.SaveAliasButton.Name = "SaveAliasButton";
			this.SaveAliasButton.Size = new System.Drawing.Size(63, 23);
			this.SaveAliasButton.TabIndex = 18;
			this.SaveAliasButton.Text = "OK";
			this.SaveAliasButton.Enabled = false;
			this.SaveAliasButton.UseVisualStyleBackColor = true;
			this.SaveAliasButton.Click += new System.EventHandler(this.SaveAliasButton_Click);
			// 
			// WSSOptionsPageControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.SaveAliasButton);
			this.Controls.Add(this.ProjectAliasVar);
			this.Controls.Add(this.ProjectNameVar);
			this.Controls.Add(this.ShortcutsListView);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.UserNameTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PackagesTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.PackagesButton);
			this.Controls.Add(this.TestButton);
			this.Controls.Add(this.TestTextBox);
			this.Name = "WSSOptionsPageControl";
			this.Size = new System.Drawing.Size(356, 505);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox PackagesTextBox;
		private System.Windows.Forms.Button PackagesButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button TestButton;
		private System.Windows.Forms.TextBox TestTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox UserNameTextBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListView ShortcutsListView;
		private System.Windows.Forms.ColumnHeader columnHeader_projectName;
		private System.Windows.Forms.ColumnHeader columnHeader_projectAlias;
		private System.Windows.Forms.ColumnHeader columnHeader_buttons;
		private System.Windows.Forms.TextBox ProjectNameVar;
		private System.Windows.Forms.TextBox ProjectAliasVar;
		private System.Windows.Forms.Button SaveAliasButton;
	}
}
