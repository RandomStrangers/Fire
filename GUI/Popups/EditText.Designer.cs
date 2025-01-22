using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Flames.Gui.Popups
{
    public partial class EditText
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public IContainer Components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (Components != null))
            {
                Components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.CmbList = new ComboBox();
            this.TxtEdit = new TextBox();
            this.BtnColor = new Button();
            this.BtnToken = new Button();
            this.BtnSave = new Button();
            this.SuspendLayout();
            // 
            // CmbList
            // 
            this.CmbList.BackColor = SystemColors.Window;
            this.CmbList.FormattingEnabled = true;
            this.CmbList.Location = new Point(392, 10);
            this.CmbList.Name = "CmbList";
            this.CmbList.Size = new Size(108, 21);
            this.CmbList.TabIndex = 0;
            this.CmbList.SelectedIndexChanged += new EventHandler(this.cmbList_SelectedIndexChanged);
            // 
            // TxtEdit
            // 
            this.TxtEdit.BackColor = SystemColors.Window;
            this.TxtEdit.Location = new Point(7, 39);
            this.TxtEdit.Multiline = true;
            this.TxtEdit.Name = "TxtEdit";
            this.TxtEdit.Size = new Size(493, 282);
            this.TxtEdit.TabIndex = 2;
            this.TxtEdit.WordWrap = false;
            // 
            // BtnColor
            // 
            this.BtnColor.Location = new Point(7, 9);
            this.BtnColor.Name = "BtnColor";
            this.BtnColor.Size = new Size(75, 23);
            this.BtnColor.TabIndex = 3;
            this.BtnColor.Text = "Insert color";
            this.BtnColor.UseVisualStyleBackColor = true;
            this.BtnColor.Click += new EventHandler(this.btnColor_Click);
            // 
            // BtnToken
            // 
            this.BtnToken.Location = new Point(90, 9);
            this.BtnToken.Name = "BtnToken";
            this.BtnToken.Size = new Size(75, 23);
            this.BtnToken.TabIndex = 4;
            this.BtnToken.Text = "Insert token";
            this.BtnToken.UseVisualStyleBackColor = true;
            this.BtnToken.Click += new EventHandler(this.btnToken_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new Point(321, 9);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new Size(65, 23);
            this.BtnSave.TabIndex = 5;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new EventHandler(this.btnSave_Click);
            // 
            // EditText
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(507, 328);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnToken);
            this.Controls.Add(this.BtnColor);
            this.Controls.Add(this.TxtEdit);
            this.Controls.Add(this.CmbList);
            this.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Load += new EventHandler(this.EditText_Load);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditText";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Editing (none)";
            this.FormClosing += new FormClosingEventHandler(this.EditTxt_Unload);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public Button BtnSave;
        public Button BtnToken;
        public Button BtnColor;
        #endregion
        public ComboBox CmbList;
        public TextBox TxtEdit;
    }
}