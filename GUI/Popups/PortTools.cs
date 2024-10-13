/*
 
    Copyright 2012 MCForge
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
 
    https://opensource.org/license/ecl-2-0/
    https://www.gnu.org/licenses/gpl-3.0.html
 
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Flames.Gui.Popups
{
    public partial class PortTools : Form
    {
        public readonly BackgroundWorker worker;
        public readonly UPnP upnp;
        public int port;
        
        public PortTools(int port) {
            InitializeComponent();
            worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            worker.DoWork += AsyncWorker_DoWork;
            worker.RunWorkerCompleted += AsyncWorker_OnCompleted;
            
            this.port = port;
            btnForward.Text = "Forward " + port;
            
            upnp = new UPnP();
            upnp.Log = LogUPnP;
        }

        public void PortTools_Load(object sender, EventArgs e) {
            GuiUtils.SetIcon(this);
        }

        public void PortChecker_FormClosing(object sender, FormClosingEventArgs e) {
            worker.CancelAsync();
        }

        public void linkManually_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            GuiUtils.OpenBrowser("https://www.canyouseeme.org/");
        }

        public void linkHelpForward_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            GuiUtils.OpenBrowser("https://portforward.com");
        }

        public void btnForward_Click(object sender, EventArgs e) {
            StartForwardOrDelete(true);
        }

        public void btnDelete_Click(object sender, EventArgs e) {
            StartForwardOrDelete(false);
        }


        public void StartForwardOrDelete(bool forwardingMode) {
            SetUPnPEnabled(false);
            txtLogs.Text = "";
            MakeLogsVisible();
            worker.RunWorkerAsync(forwardingMode);
        }

        public void MakeLogsVisible() {
            if (gbLogs.Visible) return;
            // https://stackoverflow.com/questions/5962595/how-do-you-resize-a-form-to-fit-its-content-automatically
            this.AutoSize  = true;
            gbLogs.Visible = true;
        }

        public void SetUPnPEnabled(bool enabled) {
            btnDelete.Enabled  = enabled;
            btnForward.Enabled = enabled;
        }


        public void AsyncWorker_DoWork(object sender, DoWorkEventArgs e) {
            bool forwarding = (bool)e.Argument;
            
            try {
                if (!upnp.Discover()) {
                    e.Result = 0;
                } else if (forwarding) {
                    upnp.ForwardPort(port, UPnP.TCP_PROTOCOL, Server.SoftwareName + "Server");
                    e.Result = 1;
                } else {
                    upnp.DeleteForwardingRule(port, UPnP.TCP_PROTOCOL);
                    e.Result = 3;
                }
            } catch (Exception ex) {
                Logger.LogError("Unexpected UPnP error", ex);
                e.Result = 2;
            }
        }

        public void AsyncWorker_OnCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Cancelled) return;
            SetUPnPEnabled(true);

            int result = (int)e.Result;
            switch (result) {
                case 0:
                    lblResult.Text = "Error contacting router.";
                    lblResult.ForeColor = Color.Red;
                    return;
                case 1:
                    lblResult.Text = "Port forwarded automatically using UPnP";
                    lblResult.ForeColor = Color.Green;
                    return;
                case 2:
                    lblResult.Text = "Unexpected error, see Error Logs";
                    lblResult.ForeColor = Color.Red;
                    return;
                case 3:
                    lblResult.Text = "Deleted port forward rule";
                    lblResult.ForeColor = Color.Green;
                    return;
            }
        }

        public void LogUPnP(string message) {
            RunOnUI_Async(() => txtLogs.AppendText(message + "\r\n"));
        }

        public void RunOnUI_Async(UIAction act) { BeginInvoke(act); }
    }
}