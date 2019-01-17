namespace WSSConsulting.WSSDeveloper.Commands.AddToTypes
{
    partial class AddToTypesForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddToTypesForm));
			this.ClassesBox = new System.Windows.Forms.ComboBox();
			this.ItemTypeBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.paramsGroupBox = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ClassesBox
			// 
			this.ClassesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ClassesBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ClassesBox.FormattingEnabled = true;
			this.ClassesBox.Location = new System.Drawing.Point(13, 25);
			this.ClassesBox.Name = "ClassesBox";
			this.ClassesBox.Size = new System.Drawing.Size(292, 21);
			this.ClassesBox.TabIndex = 0;
			// 
			// ItemTypeBox
			// 
			this.ItemTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItemTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ItemTypeBox.FormattingEnabled = true;
			this.ItemTypeBox.Location = new System.Drawing.Point(13, 69);
			this.ItemTypeBox.Name = "ItemTypeBox";
			this.ItemTypeBox.Size = new System.Drawing.Size(292, 21);
			this.ItemTypeBox.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(26, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Тип";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Класс";
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(149, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "ОК";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(230, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Отмена";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// paramsGroupBox
			// 
			this.paramsGroupBox.AutoSize = true;
			this.paramsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.paramsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.paramsGroupBox.Location = new System.Drawing.Point(13, 96);
			this.paramsGroupBox.MaximumSize = new System.Drawing.Size(292, 0);
			this.paramsGroupBox.MinimumSize = new System.Drawing.Size(292, 0);
			this.paramsGroupBox.Name = "paramsGroupBox";
			this.paramsGroupBox.Size = new System.Drawing.Size(292, 5);
			this.paramsGroupBox.TabIndex = 3;
			this.paramsGroupBox.TabStop = false;
			this.paramsGroupBox.Text = "Параметры";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.okButton);
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 151);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(317, 29);
			this.panel1.TabIndex = 0;
			// 
			// AddToTypesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(317, 180);
			this.Controls.Add(this.ItemTypeBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ClassesBox);
			this.Controls.Add(this.paramsGroupBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(333, 38);
			this.Name = "AddToTypesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Добавить в Types.xml";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ClassesBox;
        private System.Windows.Forms.ComboBox ItemTypeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox paramsGroupBox;
        private System.Windows.Forms.Panel panel1;
    }
}