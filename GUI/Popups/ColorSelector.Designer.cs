using System;
using System.Drawing;
using System.Windows.Forms;
namespace Flames.Gui.Popups
{
    public partial class ColorSelector
    {
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        public void InitializeComponent()
        {
            this.BtnCancel = new Button();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = DialogResult.Cancel;
            this.BtnCancel.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Location = new Point(100, 100);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new Size(100, 25);
            this.BtnCancel.TabIndex = 260;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new SizeF(8F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(200, 200);
            this.Controls.Add(this.BtnCancel);
            this.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Load += new EventHandler(this.ColorSelector_Load);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "ColorPicker";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "ColorPicker";
            this.ResumeLayout(false);
        }
        public Button BtnCancel;
    }
}