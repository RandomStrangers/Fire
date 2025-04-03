/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
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
using System.Windows.Forms;
using Flames.Gui.Popups;
namespace Flames.Gui
{

    public partial class PropertyWindow : Form
    {
        public bool WarnDisabledVerification = true;
        public void LoadGeneralProps()
        {
            Srv_txtName.Text = Server.Config.Name;
            Srv_txtMOTD.Text = Server.Config.MOTD;
            Srv_numPort.Value = Server.Config.Port;
            Srv_txtOwner.Text = Server.Config.OwnerName;
            Srv_chkPublic.Checked = Server.Config.Public;
            Srv_numPlayers.Value = Server.Config.MaxPlayers;
            Srv_numGuests.Value = Server.Config.MaxGuests;
            Srv_numGuests.Maximum = Srv_numPlayers.Value;
            Srv_cbMustAgree.Checked = Server.Config.AgreeToRulesOnEntry;
            Lvl_txtMain.Text = Server.Config.MainLevel;
            Lvl_chkAutoload.Checked = Server.Config.AutoLoadMaps;
            Lvl_chkWorld.Checked = Server.Config.ServerWideChat;
            WarnDisabledVerification = false;
            Adv_chkVerify.Checked = Server.Config.VerifyNames;
            WarnDisabledVerification = true;
            Adv_chkCPE.Checked = Server.Config.EnableCPE;
            ChkUpdates.Checked = Server.Config.CheckForUpdates;
        }
        public void ApplyGeneralProps()
        {
            Server.Config.Name = Srv_txtName.Text;
            Server.Config.MOTD = Srv_txtMOTD.Text;
            Server.Config.Port = (int)Srv_numPort.Value;
            Server.Config.OwnerName = Srv_txtOwner.Text;
            Server.Config.Public = Srv_chkPublic.Checked;
            Server.Config.MaxPlayers = (int)Srv_numPlayers.Value;
            Server.Config.MaxGuests = (int)Srv_numGuests.Value;
            Server.Config.AgreeToRulesOnEntry = Srv_cbMustAgree.Checked;
            Server.Config.MainLevel = Lvl_txtMain.Text;
            Server.Config.AutoLoadMaps = Lvl_chkAutoload.Checked;
            Server.Config.ServerWideChat = Lvl_chkWorld.Checked;
            Server.Config.VerifyNames = Adv_chkVerify.Checked;
            Server.Config.EnableCPE = Adv_chkCPE.Checked;
            Server.Config.CheckForUpdates = ChkUpdates.Checked;
            //Server.Config.reportBack = ;  //No setting for this?                
        }
        public const string WarnMsg = "Disabling name verification means players\ncan login as anyone, including YOU\n\n" +
            "Are you sure you want to disable name verification?";
        public void ChkVerify_CheckedChanged(object sender, EventArgs e)
        {
            if (!WarnDisabledVerification || Adv_chkVerify.Checked)
            {
                return;
            }
            if (Popup.OKCancel(WarnMsg, "Security warning"))
            {
                return;
            }
            Adv_chkVerify.Checked = true;
        }
        public void NumPlayers_ValueChanged(object sender, EventArgs e)
        {
            // Ensure that number of guests is never more than number of players
            if (Srv_numGuests.Value > Srv_numPlayers.Value)
            {
                Srv_numGuests.Value = Srv_numPlayers.Value;
            }
            Srv_numGuests.Maximum = Srv_numPlayers.Value;
        }
        public void ChkPort_Click(object sender, EventArgs e)
        {
            int port = (int)Srv_numPort.Value;
            using PortTools form = new PortTools(port);
            form.ShowDialog();
        }
        public void ForceUpdateBtn_Click(object sender, EventArgs e)
        {
            Srv_btnForceUpdate.Enabled = false;
            string msg = "Would you like to force update " + Colors.Strip(Server.SoftwareName) + " now?";
            if (Popup.YesNo(msg, "Force update"))
            {
                SaveChanges();
                Updater.PerformUpdate(true);
                Dispose();
            }
            else
            {
                Srv_btnForceUpdate.Enabled = true;
            }
        }
    }
}