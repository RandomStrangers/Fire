using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Flames.Gui.Popups 
{
    public partial class UpdateAvailable
	{
        /// <summary>
        /// Designer variable used to keep track of non-visual Components.
        /// </summary>
        public IContainer Components = null;
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) 
			{
				if (Components != null) 
				{
					Components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        public void InitializeComponent()
		{
			this.BtnUpdate = new Button();
			this.BtnNo = new Button();
			this.LblText = new Label();
			this.SuspendLayout();
			// 
			// BtnUpdate
			// 
			this.BtnUpdate.Location = new Point(130, 96);
			this.BtnUpdate.Name = "BtnUpdate";
			this.BtnUpdate.Size = new Size(88, 26);
			this.BtnUpdate.TabIndex = 0;
			this.BtnUpdate.Text = "Update";
			this.BtnUpdate.UseVisualStyleBackColor = true;
			this.BtnUpdate.Click += new EventHandler(this.BtnUpdate_Click);
			// 
			// BtnNo
			// 
			this.BtnNo.Location = new Point(226, 96);
			this.BtnNo.Name = "BtnNo";
			this.BtnNo.Size = new Size(88, 26);
			this.BtnNo.TabIndex = 1;
			this.BtnNo.Text = "No";
			this.BtnNo.UseVisualStyleBackColor = true;
			this.BtnNo.Click += new EventHandler(this.BtnNo_Click);
			// 
			// LblText
			// 
			this.LblText.AutoSize = true;
			this.LblText.Location = new Point(63, 35);
			this.LblText.Name = "LblText";
			this.LblText.Size = new Size(226, 13);
			this.LblText.TabIndex = 2;
			this.LblText.Text = "New version found. Would you like to update?";
			// 
			// UpdateCheck
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(323, 133);
			this.Controls.Add(this.LblText);
			this.Controls.Add(this.BtnNo);
			this.Controls.Add(this.BtnUpdate);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateAvailable";
			this.StartPosition = FormStartPosition.Manual;
			this.Text = "Update?";
			this.Closed += new EventHandler(this.UpdateCheck_Closed);
			this.Load += new EventHandler(this.UpdateCheck_Load);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
        public Label LblText;
        public Button BtnNo;
        public Button BtnUpdate;
	}
}