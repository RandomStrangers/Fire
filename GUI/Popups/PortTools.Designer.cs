using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Flames.Gui.Popups
{
    public partial class PortTools
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
            this.Components = new Container();
            this.LinkManually = new LinkLabel();
            this.GbUpnp = new GroupBox();
            this.BtnForward = new Button();
            this.BtnDelete = new Button();
            this.LblResult = new Label();
            this.LblInfo = new Label();
            this.LinkHelpForward = new LinkLabel();
            this.ToolTip1 = new ToolTip(this.Components);
            this.GbLogs = new GroupBox();
            this.TxtLogs = new TextBox();
            this.GbUpnp.SuspendLayout();
            this.GbLogs.SuspendLayout();
            this.SuspendLayout();
            // 
            // LinkManually
            // 
            this.LinkManually.AutoSize = true;
            this.LinkManually.Location = new Point(12, 10);
            this.LinkManually.Name = "LinkManually";
            this.LinkManually.Size = new Size(86, 13);
            this.LinkManually.TabIndex = 1;
            this.LinkManually.TabStop = true;
            this.LinkManually.Text = "Check port open";
            this.LinkManually.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkManually_LinkClicked);
            // 
            // GbUpnp
            // 
            this.GbUpnp.Controls.Add(this.BtnForward);
            this.GbUpnp.Controls.Add(this.BtnDelete);
            this.GbUpnp.Controls.Add(this.LblResult);
            this.GbUpnp.Controls.Add(this.LblInfo);
            this.GbUpnp.Location = new Point(11, 42);
            this.GbUpnp.Name = "GbUpnp";
            this.GbUpnp.Size = new Size(274, 107);
            this.GbUpnp.TabIndex = 7;
            this.GbUpnp.TabStop = false;
            this.GbUpnp.Text = "Auto port forward";
            // 
            // BtnForward
            // 
            this.BtnForward.Location = new Point(22, 66);
            this.BtnForward.Name = "BtnForward";
            this.BtnForward.Size = new Size(86, 23);
            this.BtnForward.TabIndex = 3;
            this.BtnForward.Text = "Forward 25565";
            this.ToolTip1.SetToolTip(this.BtnForward, "This does not work for everyone, keep trying or manually port forward.\r\n");
            this.BtnForward.UseVisualStyleBackColor = true;
            this.BtnForward.Click += new EventHandler(this.btnForward_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Location = new Point(156, 66);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new Size(93, 23);
            this.BtnDelete.TabIndex = 4;
            this.BtnDelete.Text = "Delete forward";
            this.BtnDelete.UseVisualStyleBackColor = true;
            this.BtnDelete.Click += new EventHandler(this.btnDelete_Click);
            // 
            // LblResult
            // 
            this.LblResult.AutoSize = true;
            this.LblResult.ForeColor = SystemColors.ButtonShadow;
            this.LblResult.Location = new Point(22, 91);
            this.LblResult.Name = "LblResult";
            this.LblResult.Size = new Size(0, 13);
            this.LblResult.TabIndex = 11;
            // 
            // LblInfo
            // 
            this.LblInfo.AutoSize = true;
            this.LblInfo.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.LblInfo.Location = new Point(5, 17);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new Size(266, 39);
            this.LblInfo.TabIndex = 12;
            this.LblInfo.Text = "This uses UPnP, which not all routers support.\r\nIf this doesn\'t work, you will ha" +
            "ve to\r\n manually port forward in your router.";
            this.LblInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LinkHelpForward
            // 
            this.LinkHelpForward.AutoSize = true;
            this.LinkHelpForward.Location = new Point(172, 10);
            this.LinkHelpForward.Name = "LinkHelpForward";
            this.LinkHelpForward.Size = new Size(114, 13);
            this.LinkHelpForward.TabIndex = 2;
            this.LinkHelpForward.TabStop = true;
            this.LinkHelpForward.Text = "Need help forwarding?";
            this.LinkHelpForward.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkHelpForward_LinkClicked);
            // 
            // GbLogs
            // 
            this.GbLogs.Controls.Add(this.TxtLogs);
            this.GbLogs.Location = new Point(11, 153);
            this.GbLogs.Name = "GbLogs";
            this.GbLogs.Size = new Size(274, 165);
            this.GbLogs.TabIndex = 13;
            this.GbLogs.TabStop = false;
            this.GbLogs.Text = "UPnP logs";
            this.GbLogs.Visible = false;
            // 
            // TxtLogs
            // 
            this.TxtLogs.BackColor = SystemColors.Window;
            this.TxtLogs.Location = new Point(7, 20);
            this.TxtLogs.Multiline = true;
            this.TxtLogs.Name = "TxtLogs";
            this.TxtLogs.ReadOnly = true;
            this.TxtLogs.ScrollBars = ScrollBars.Both;
            this.TxtLogs.Size = new Size(261, 139);
            this.TxtLogs.TabIndex = 14;
            // 
            // PortTools
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(296, 160);
            this.Controls.Add(this.GbLogs);
            this.Controls.Add(this.GbUpnp);
            this.Controls.Add(this.LinkManually);
            this.Controls.Add(this.LinkHelpForward);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortTools";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Port forward tools";
            this.FormClosing += new FormClosingEventHandler(this.PortChecker_FormClosing);
            this.Load += new EventHandler(this.PortTools_Load);
            this.GbUpnp.ResumeLayout(false);
            this.GbUpnp.PerformLayout();
            this.GbLogs.ResumeLayout(false);
            this.GbLogs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public TextBox TxtLogs;
        public GroupBox GbLogs;
        #endregion
        public LinkLabel LinkManually;
        public GroupBox GbUpnp;
        public LinkLabel LinkHelpForward;
        public Label LblResult;
        public Button BtnForward;
        public Label LblInfo;
        public Button BtnDelete;
        public ToolTip ToolTip1;
    }
}