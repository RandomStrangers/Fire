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
using System.Drawing;
using System.Windows.Forms;
using Flames.Gui.Popups;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public void LoadChatProps()
        {
            Chat_ParseColor(Server.Config.DefaultColor, Chat_btnDefault);
            Chat_ParseColor(Server.Config.IRCColor, Chat_btnIRC);
            Chat_ParseColor(Server.Config.HelpSyntaxColor, Chat_btnSyntax);
            Chat_ParseColor(Server.Config.HelpDescriptionColor, Chat_btnDesc);
            Chat_ParseColor(Server.Config.WarningErrorColor, Chat_btnWarn);
            Chat_txtConsole.Text = Server.Config.FlameState;
            Chat_chkFilter.Checked = Server.Config.ProfanityFiltering;
            Chat_cbTabRank.Checked = Server.Config.TablistRankSorted;
            Chat_cbTabLevel.Checked = !Server.Config.TablistGlobal;
            Chat_cbTabBots.Checked = Server.Config.TablistBots;
            Chat_txtShutdown.Text = Server.Config.DefaultShutdownMessage;
            Chat_chkCheap.Checked = Server.Config.ShowInvincibleMessage;
            Chat_txtCheap.Enabled = Chat_chkCheap.Checked;
            Chat_txtCheap.Text = Server.Config.InvincibleMessage;
            Chat_txtBan.Text = Server.Config.DefaultBanMessage;
            Chat_txtPromote.Text = Server.Config.DefaultPromoteMessage;
            Chat_txtDemote.Text = Server.Config.DefaultDemoteMessage;
            Chat_txtLogin.Text = Server.Config.DefaultLoginMessage;
            Chat_txtLogout.Text = Server.Config.DefaultLogoutMessage;
        }
        public void ApplyChatProps()
        {
            Server.Config.DefaultColor = Colors.Parse(Chat_btnDefault.Text);
            Server.Config.IRCColor = Colors.Parse(Chat_btnIRC.Text);
            Server.Config.HelpSyntaxColor = Colors.Parse(Chat_btnSyntax.Text);
            Server.Config.HelpDescriptionColor = Colors.Parse(Chat_btnDesc.Text);
            Server.Config.WarningErrorColor = Colors.Parse(Chat_btnWarn.Text);
            Server.Config.FlameState = Chat_txtConsole.Text;
            Server.Config.ProfanityFiltering = Chat_chkFilter.Checked;
            Server.Config.TablistRankSorted = Chat_cbTabRank.Checked;
            Server.Config.TablistGlobal = !Chat_cbTabLevel.Checked;
            Server.Config.TablistBots = Chat_cbTabBots.Checked;
            Server.Config.DefaultShutdownMessage = Chat_txtShutdown.Text;
            Server.Config.ShowInvincibleMessage = Chat_chkCheap.Checked;
            Server.Config.InvincibleMessage = Chat_txtCheap.Text;
            Server.Config.DefaultBanMessage = Chat_txtBan.Text;
            Server.Config.DefaultPromoteMessage = Chat_txtPromote.Text;
            Server.Config.DefaultDemoteMessage = Chat_txtDemote.Text;
            Server.Config.DefaultLoginMessage = Chat_txtLogin.Text;
            Server.Config.DefaultLogoutMessage = Chat_txtLogout.Text;
        }
        public void Chat_chkCheap_CheckedChanged(object sender, EventArgs e)
        {
            Chat_txtCheap.Enabled = Chat_chkCheap.Checked;
        }
        public void Chat_cmbDefault_Click(object sender, EventArgs e)
        {
            Chat_ShowColorDialog(Chat_btnDefault, "Default color");
        }
        public void Chat_btnIRC_Click(object sender, EventArgs e)
        {
            Chat_ShowColorDialog(Chat_btnIRC, "IRC text color");
        }
        public void Chat_btnSyntax_Click(object sender, EventArgs e)
        {
            Chat_ShowColorDialog(Chat_btnSyntax, "Help syntax color");
        }
        public void Chat_btnDesc_Click(object sender, EventArgs e)
        {
            Chat_ShowColorDialog(Chat_btnDesc, "Help description color");
        }
        public void Chat_btnWarn_Click(object sender, EventArgs e)
        {
            Chat_ShowColorDialog(Chat_btnWarn, "Warning / error color");
        }
        public void Chat_ParseColor(string value, Button target)
        {
            char code = value[1];
            target.Text = Colors.Name(value);
            target.BackColor = ColorSelector.LookupColor(code, out Color textCol);
            target.ForeColor = textCol;
        }
        public void Chat_ShowColorDialog(Button target, string title)
        {
            string parsed = Colors.Parse(target.Text);
            char col = parsed.Length == 0 ? 'f' : parsed[1];
            using ColorSelector sel = new ColorSelector(title, col);
            DialogResult result = sel.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }
            target.Text = Colors.Name(sel.ColorCode);
            target.BackColor = ColorSelector.LookupColor(sel.ColorCode, out Color textCol);
            target.ForeColor = textCol;
        }
    }
}