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
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public void LoadSecurityProps()
        {
            Sec_cbLogNotes.Checked = Server.Config.LogNotes;
            Sec_cbVerifyAdmins.Checked = Server.Config.verifyadmins;
            Sec_cbWhitelist.Checked = Server.Config.WhitelistedOnly;
            GuiPerms.SetRanks(Sec_cmbVerifyRank);
            GuiPerms.SetSelectedRank(Sec_cmbVerifyRank, Server.Config.VerifyAdminsRank);
            Sec_cmbVerifyRank.Enabled = Server.Config.verifyadmins;
            Sec_cbChatAuto.Checked = Server.Config.ChatSpamCheck;
            Sec_numChatMsgs.Value = Server.Config.ChatSpamCount;
            Sec_numChatSecs.Value = Server.Config.ChatSpamInterval;
            Sec_numChatMute.Value = Server.Config.ChatSpamMuteTime;
            ToggleChatSpamSettings(Server.Config.ChatSpamCheck);
            Sec_cbCmdAuto.Checked = Server.Config.CmdSpamCheck;
            Sec_numCmdMsgs.Value = Server.Config.CmdSpamCount;
            Sec_numCmdSecs.Value = Server.Config.CmdSpamInterval;
            Sec_numCmdMute.Value = Server.Config.CmdSpamBlockTime;
            ToggleCmdSpamSettings(Server.Config.CmdSpamCheck);
            Sec_cbBlocksAuto.Checked = Server.Config.BlockSpamCheck;
            Sec_numBlocksMsgs.Value = Server.Config.BlockSpamCount;
            Sec_numBlocksSecs.Value = Server.Config.BlockSpamInterval;
            ToggleBlocksSpamSettings(Server.Config.BlockSpamCheck);
            Sec_cbIPAuto.Checked = Server.Config.IPSpamCheck;
            Sec_numIPMsgs.Value = Server.Config.IPSpamCount;
            Sec_numIPSecs.Value = Server.Config.IPSpamInterval;
            Sec_numIPMute.Value = Server.Config.IPSpamBlockTime;
            ToggleIPSpamSettings(Sec_cbIPAuto.Checked);
        }
        public void ApplySecurityProps()
        {
            Server.Config.LogNotes = Sec_cbLogNotes.Checked;
            Server.Config.verifyadmins = Sec_cbVerifyAdmins.Checked;
            Server.Config.VerifyAdminsRank = GuiPerms.GetSelectedRank(Sec_cmbVerifyRank, LevelPermission.Operator);
            Server.Config.WhitelistedOnly = Sec_cbWhitelist.Checked;
            Server.Config.ChatSpamCheck = Sec_cbChatAuto.Checked;
            Server.Config.ChatSpamCount = (int)Sec_numChatMsgs.Value;
            Server.Config.ChatSpamInterval = Sec_numChatSecs.Value;
            Server.Config.ChatSpamMuteTime = Sec_numChatMute.Value;
            Server.Config.CmdSpamCheck = Sec_cbCmdAuto.Checked;
            Server.Config.CmdSpamCount = (int)Sec_numCmdMsgs.Value;
            Server.Config.CmdSpamInterval = Sec_numCmdSecs.Value;
            Server.Config.CmdSpamBlockTime = Sec_numCmdMute.Value;
            Server.Config.BlockSpamCheck = Sec_cbBlocksAuto.Checked;
            Server.Config.BlockSpamCount = (int)Sec_numBlocksMsgs.Value;
            Server.Config.BlockSpamInterval = Sec_numBlocksSecs.Value;
            Server.Config.IPSpamCheck = Sec_cbIPAuto.Checked;
            Server.Config.IPSpamCount = (int)Sec_numIPMsgs.Value;
            Server.Config.IPSpamInterval = Sec_numIPSecs.Value;
            Server.Config.IPSpamBlockTime = Sec_numIPMute.Value;
        }
        public void Sec_cbChatAuto_Checked(object sender, EventArgs e)
        {
            ToggleChatSpamSettings(Sec_cbChatAuto.Checked);
        }
        public void Sec_cbCmdAuto_Checked(object sender, EventArgs e)
        {
            ToggleCmdSpamSettings(Sec_cbCmdAuto.Checked);
        }
        public void Sec_cbBlocksAuto_Checked(object sender, EventArgs e)
        {
            ToggleBlocksSpamSettings(Sec_cbBlocksAuto.Checked);
        }
        public void Sec_cbIPAuto_Checked(object sender, EventArgs e)
        {
            ToggleIPSpamSettings(Sec_cbIPAuto.Checked);
        }
        public void ToggleChatSpamSettings(bool enabled)
        {
            Sec_numChatMsgs.Enabled = enabled;
            Sec_numChatMute.Enabled = enabled;
            Sec_numChatSecs.Enabled = enabled;
        }
        public void ToggleCmdSpamSettings(bool enabled)
        {
            Sec_numCmdMsgs.Enabled = enabled;
            Sec_numCmdMute.Enabled = enabled;
            Sec_numCmdSecs.Enabled = enabled;
        }
        public void ToggleBlocksSpamSettings(bool enabled)
        {
            Sec_numBlocksMsgs.Enabled = enabled;
            Sec_numBlocksSecs.Enabled = enabled;
        }
        public void ToggleIPSpamSettings(bool enabled)
        {
            Sec_numIPMsgs.Enabled = enabled;
            Sec_numIPMute.Enabled = enabled;
            Sec_numIPSecs.Enabled = enabled;
        }
        public void VerifyAdminsChecked(object sender, EventArgs e)
        {
            Sec_cmbVerifyRank.Enabled = Sec_cbVerifyAdmins.Checked;
        }
    }
}