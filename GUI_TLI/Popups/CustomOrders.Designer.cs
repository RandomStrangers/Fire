using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Flames.Gui.Popups
{
    public partial class CustomOrders
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
            if (disposing && (Components != null))
            {
                Components.Dispose();
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
            this.LblOrders = new Label();
            this.LstOrders = new ListBox();
            this.GrpCreate = new GroupBox();
            this.BtnCreate1 = new Button();
            this.BtnCreate2 = new Button();
            this.BtnCreate3 = new Button();
            this.BtnCreate4 = new Button();
            this.BtnCreate5 = new Button();
            this.TxtOrdName = new TextBox();
            this.LblTxtName = new Label();
            this.BtnLoad = new Button();
            this.BtnUnload = new Button();
            this.GrpCreate.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblOrders
            // 
            this.LblOrders.AutoSize = true;
            this.LblOrders.Location = new Point(13, 111);
            this.LblOrders.Name = "LblOrders";
            this.LblOrders.Size = new Size(133, 13);
            this.LblOrders.TabIndex = 45;
            this.LblOrders.Text = "Loaded custom orders";
            // 
            // LstOrders
            // 
            this.LstOrders.BackColor = SystemColors.Window;
            this.LstOrders.ForeColor = SystemColors.WindowText;
            this.LstOrders.FormattingEnabled = true;
            this.LstOrders.Location = new Point(12, 127);
            this.LstOrders.MultiColumn = true;
            this.LstOrders.Name = "LstOrders";
            this.LstOrders.Size = new Size(459, 134);
            this.LstOrders.TabIndex = 44;
            this.LstOrders.SelectedIndexChanged += new EventHandler(this.lstOrders_SelectedIndexChanged);
            // 
            // GrpCreate
            // 
            this.GrpCreate.Controls.Add(this.BtnCreate1);
            this.GrpCreate.Controls.Add(this.BtnCreate2);
            this.GrpCreate.Controls.Add(this.BtnCreate3);
            this.GrpCreate.Controls.Add(this.BtnCreate4);
            this.GrpCreate.Controls.Add(this.BtnCreate5);
            this.GrpCreate.Controls.Add(this.TxtOrdName);
            this.GrpCreate.Controls.Add(this.LblTxtName);
            this.GrpCreate.Location = new Point(12, 12);
            this.GrpCreate.Name = "GrpCreate";
            this.GrpCreate.Size = new Size(459, 84);
            this.GrpCreate.TabIndex = 43;
            this.GrpCreate.TabStop = false;
            this.GrpCreate.Text = "Create custom order";
            // 
            // BtnCreate1
            // 
            this.BtnCreate1.Location = new Point(11, 48);
            this.BtnCreate1.Margin = new Padding(2, 3, 2, 3);
            this.BtnCreate1.Name = "BtnCreate1";
            this.BtnCreate1.Size = new Size(80, 23);
            this.BtnCreate1.TabIndex = 29;
            this.BtnCreate1.UseVisualStyleBackColor = true;
            // 
            // BtnCreate2
            // 
            this.BtnCreate2.Location = new Point(96, 48);
            this.BtnCreate2.Margin = new Padding(2, 3, 2, 3);
            this.BtnCreate2.Name = "BtnCreate2";
            this.BtnCreate2.Size = new Size(80, 23);
            this.BtnCreate2.TabIndex = 30;
            this.BtnCreate2.UseVisualStyleBackColor = true;
            // 
            // BtnCreate3
            // 
            this.BtnCreate3.Location = new Point(181, 48);
            this.BtnCreate3.Margin = new Padding(2, 3, 2, 3);
            this.BtnCreate3.Name = "BtnCreate3";
            this.BtnCreate3.Size = new Size(80, 23);
            this.BtnCreate3.TabIndex = 31;
            this.BtnCreate3.UseVisualStyleBackColor = true;
            // 
            // BtnCreate4
            // 
            this.BtnCreate4.Location = new Point(266, 48);
            this.BtnCreate4.Margin = new Padding(2, 3, 2, 3);
            this.BtnCreate4.Name = "BtnCreate4";
            this.BtnCreate4.Size = new Size(80, 23);
            this.BtnCreate4.TabIndex = 32;
            this.BtnCreate4.UseVisualStyleBackColor = true;
            // 
            // BtnCreate5
            // 
            this.BtnCreate5.Location = new Point(351, 48);
            this.BtnCreate5.Margin = new Padding(2, 3, 2, 3);
            this.BtnCreate5.Name = "BtnCreate5";
            this.BtnCreate5.Size = new Size(80, 23);
            this.BtnCreate5.TabIndex = 33;
            this.BtnCreate5.UseVisualStyleBackColor = true;
            // 
            // TxtOrdName
            // 
            this.TxtOrdName.BackColor = SystemColors.Window;
            this.TxtOrdName.Location = new Point(100, 20);
            this.TxtOrdName.Margin = new Padding(2, 3, 2, 3);
            this.TxtOrdName.Name = "TxtOrdName";
            this.TxtOrdName.Size = new Size(348, 18);
            this.TxtOrdName.TabIndex = 27;
            // 
            // LblTxtName
            // 
            this.LblTxtName.AutoSize = true;
            this.LblTxtName.Location = new Point(11, 23);
            this.LblTxtName.Margin = new Padding(2, 0, 2, 0);
            this.LblTxtName.Name = "LblTxtName";
            this.LblTxtName.Size = new Size(78, 12);
            this.LblTxtName.TabIndex = 28;
            this.LblTxtName.Text = "Order Name:";
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new Point(13, 267);
            this.BtnLoad.Margin = new Padding(2, 3, 2, 3);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new Size(80, 23);
            this.BtnLoad.TabIndex = 41;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new EventHandler(this.btnLoad_Click);
            // 
            // BtnUnload
            // 
            this.BtnUnload.Enabled = false;
            this.BtnUnload.Location = new Point(391, 267);
            this.BtnUnload.Margin = new Padding(2, 3, 2, 3);
            this.BtnUnload.Name = "BtnUnload";
            this.BtnUnload.Size = new Size(80, 23);
            this.BtnUnload.TabIndex = 42;
            this.BtnUnload.Text = "Unload";
            this.BtnUnload.UseVisualStyleBackColor = true;
            this.BtnUnload.Click += new EventHandler(this.btnUnload_Click);
            // 
            // CustomOrders
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(484, 301);
            this.Controls.Add(this.LblOrders);
            this.Controls.Add(this.LstOrders);
            this.Controls.Add(this.GrpCreate);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.BtnUnload);
            this.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Load += new EventHandler(this.CustomOrders_Load);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Name = "CustomOrders";
            this.Text = "Custom orders";
            this.GrpCreate.ResumeLayout(false);
            this.GrpCreate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public Button BtnUnload;
        public Button BtnLoad;
        public Label LblTxtName;
        public TextBox TxtOrdName;
        public Button BtnCreate1;
        public Button BtnCreate2;
        public Button BtnCreate3;
        public Button BtnCreate4;
        public Button BtnCreate5;
        public GroupBox GrpCreate;
        public ListBox LstOrders;
        public Label LblOrders;
    }
}