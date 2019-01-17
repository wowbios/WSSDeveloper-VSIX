namespace WSSConsulting.WSSDeveloper.Commands
{
	internal partial class DeployPackageDestinationForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeployPackageDestinationForm));
			this.combo_Package = new System.Windows.Forms.ComboBox();
			this.gb_Destination = new System.Windows.Forms.GroupBox();
			this.comboBox_TestFolder = new System.Windows.Forms.ComboBox();
			this.btn_BrowseFolder = new System.Windows.Forms.Button();
			this.tb_Folder = new System.Windows.Forms.TextBox();
			this.combo_Test = new System.Windows.Forms.ComboBox();
			this.rb_Other = new System.Windows.Forms.RadioButton();
			this.rb_Test = new System.Windows.Forms.RadioButton();
			this.rb_Package = new System.Windows.Forms.RadioButton();
			this.btn_OK = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.NewYear2019Label = new System.Windows.Forms.Label();
			this.gb_Destination.SuspendLayout();
			this.SuspendLayout();
			// 
			// combo_Package
			// 
			this.combo_Package.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.combo_Package.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.combo_Package.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.combo_Package.FormattingEnabled = true;
			this.combo_Package.Location = new System.Drawing.Point(137, 18);
			this.combo_Package.Name = "combo_Package";
			this.combo_Package.Size = new System.Drawing.Size(240, 21);
			this.combo_Package.Sorted = true;
			this.combo_Package.TabIndex = 0;
			// 
			// gb_Destination
			// 
			this.gb_Destination.Controls.Add(this.comboBox_TestFolder);
			this.gb_Destination.Controls.Add(this.btn_BrowseFolder);
			this.gb_Destination.Controls.Add(this.tb_Folder);
			this.gb_Destination.Controls.Add(this.combo_Test);
			this.gb_Destination.Controls.Add(this.combo_Package);
			this.gb_Destination.Controls.Add(this.rb_Other);
			this.gb_Destination.Controls.Add(this.rb_Test);
			this.gb_Destination.Controls.Add(this.rb_Package);
			this.gb_Destination.Location = new System.Drawing.Point(12, 12);
			this.gb_Destination.Name = "gb_Destination";
			this.gb_Destination.Size = new System.Drawing.Size(383, 151);
			this.gb_Destination.TabIndex = 3;
			this.gb_Destination.TabStop = false;
			this.gb_Destination.Text = "Назначение";
			// 
			// comboBox_TestFolder
			// 
			this.comboBox_TestFolder.Enabled = false;
			this.comboBox_TestFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.comboBox_TestFolder.FormattingEnabled = true;
			this.comboBox_TestFolder.Location = new System.Drawing.Point(6, 72);
			this.comboBox_TestFolder.Name = "comboBox_TestFolder";
			this.comboBox_TestFolder.Size = new System.Drawing.Size(371, 21);
			this.comboBox_TestFolder.TabIndex = 6;
			// 
			// btn_BrowseFolder
			// 
			this.btn_BrowseFolder.Enabled = false;
			this.btn_BrowseFolder.Location = new System.Drawing.Point(6, 122);
			this.btn_BrowseFolder.Name = "btn_BrowseFolder";
			this.btn_BrowseFolder.Size = new System.Drawing.Size(62, 20);
			this.btn_BrowseFolder.TabIndex = 4;
			this.btn_BrowseFolder.Text = "Обзор ...";
			this.btn_BrowseFolder.UseVisualStyleBackColor = true;
			this.btn_BrowseFolder.Click += new System.EventHandler(this.btn_BrowseFolder_Click);
			// 
			// tb_Folder
			// 
			this.tb_Folder.Enabled = false;
			this.tb_Folder.Location = new System.Drawing.Point(74, 122);
			this.tb_Folder.Name = "tb_Folder";
			this.tb_Folder.Size = new System.Drawing.Size(303, 20);
			this.tb_Folder.TabIndex = 5;
			// 
			// combo_Test
			// 
			this.combo_Test.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combo_Test.Enabled = false;
			this.combo_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.combo_Test.FormattingEnabled = true;
			this.combo_Test.Location = new System.Drawing.Point(137, 45);
			this.combo_Test.Name = "combo_Test";
			this.combo_Test.Size = new System.Drawing.Size(240, 21);
			this.combo_Test.TabIndex = 2;
			// 
			// rb_Other
			// 
			this.rb_Other.AutoSize = true;
			this.rb_Other.Location = new System.Drawing.Point(6, 99);
			this.rb_Other.Name = "rb_Other";
			this.rb_Other.Size = new System.Drawing.Size(62, 17);
			this.rb_Other.TabIndex = 1;
			this.rb_Other.Text = "Другое";
			this.rb_Other.UseVisualStyleBackColor = true;
			// 
			// rb_Test
			// 
			this.rb_Test.AutoSize = true;
			this.rb_Test.Location = new System.Drawing.Point(6, 46);
			this.rb_Test.Name = "rb_Test";
			this.rb_Test.Size = new System.Drawing.Size(97, 17);
			this.rb_Test.TabIndex = 1;
			this.rb_Test.Text = "Тестирование";
			this.rb_Test.UseVisualStyleBackColor = true;
			// 
			// rb_Package
			// 
			this.rb_Package.AutoSize = true;
			this.rb_Package.Checked = true;
			this.rb_Package.Location = new System.Drawing.Point(6, 19);
			this.rb_Package.Name = "rb_Package";
			this.rb_Package.Size = new System.Drawing.Size(125, 17);
			this.rb_Package.TabIndex = 1;
			this.rb_Package.TabStop = true;
			this.rb_Package.Text = "Комплект поставки";
			this.rb_Package.UseVisualStyleBackColor = true;
			// 
			// btn_OK
			// 
			this.btn_OK.Location = new System.Drawing.Point(239, 169);
			this.btn_OK.Name = "btn_OK";
			this.btn_OK.Size = new System.Drawing.Size(75, 23);
			this.btn_OK.TabIndex = 6;
			this.btn_OK.Text = "ОК";
			this.btn_OK.UseVisualStyleBackColor = true;
			this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(320, 169);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_Cancel.TabIndex = 7;
			this.btn_Cancel.Text = "Отмена";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// NewYear2019Label
			// 
			this.NewYear2019Label.AutoSize = true;
			this.NewYear2019Label.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.NewYear2019Label.Location = new System.Drawing.Point(15, 175);
			this.NewYear2019Label.Name = "NewYear2019Label";
			this.NewYear2019Label.Size = new System.Drawing.Size(181, 13);
			this.NewYear2019Label.TabIndex = 8;
			this.NewYear2019Label.Text = "До нового года осталось XXX ч";
			this.NewYear2019Label.Visible = false;
			this.NewYear2019Label.Click += new System.EventHandler(this.DccGfccdjhl2);
			// 
			// DeployPackageDestinationForm
			// 
			this.AcceptButton = this.btn_OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(407, 203);
			this.Controls.Add(this.NewYear2019Label);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.btn_OK);
			this.Controls.Add(this.gb_Destination);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DeployPackageDestinationForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Выложить комплект";
			this.TopMost = true;
			this.gb_Destination.ResumeLayout(false);
			this.gb_Destination.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox combo_Package;
		private System.Windows.Forms.GroupBox gb_Destination;
		private System.Windows.Forms.TextBox tb_Folder;
		private System.Windows.Forms.ComboBox combo_Test;
		private System.Windows.Forms.RadioButton rb_Other;
		private System.Windows.Forms.RadioButton rb_Test;
		private System.Windows.Forms.RadioButton rb_Package;
		private System.Windows.Forms.Button btn_BrowseFolder;
		private System.Windows.Forms.Button btn_OK;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.ComboBox comboBox_TestFolder;
		private System.Windows.Forms.Label NewYear2019Label;
	}
}