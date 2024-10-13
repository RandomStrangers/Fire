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
using Flames.UI;

namespace Flames.Gui 
{
    public partial class Window : Form 
    {
        public PlayerProperties playerProps;

        public void NoPlayerSelected() { Popup.Warning("No player selected"); }

        public void pl_BtnUndo_Click(object sender, EventArgs e) {
            if (curPlayer == null) { NoPlayerSelected(); return; }
            TimeSpan interval = pl_numUndo.Value;
            if (interval.TotalSeconds == 0) { Players_AppendStatus("Amount of time to undo required"); return; }

            // TODO ModerationActions instead of HandleCommand
            string duration = pl_numUndo.Value.Shorten(true, false);
            UIHelpers.HandleCommand("UndoPlayer " + curPlayer.name + " " + duration);
            Players_AppendStatus("Undid " + curPlayer.truename + " for " + duration);
        }

        public void pl_BtnMessage_Click(object sender, EventArgs e) {
            if (curPlayer == null) { NoPlayerSelected(); return; }

            string text = pl_txtMessage.Text.Trim();
            if (text.Length == 0) { Players_AppendStatus("No message to send"); return; }
            
            ChatModes.MessageDirect(Player.Flame, curPlayer, pl_txtMessage.Text);
            Players_AppendStatus("Sent player message: " + pl_txtMessage.Text);
            pl_txtMessage.Text = "";
        }

        public void pl_BtnSendCommand_Click(object sender, EventArgs e) {
            if (curPlayer == null) { NoPlayerSelected(); return; }

            string text = pl_txtSendCommand.Text.Trim()
                                                .TrimStart('/');
            if (text.Length == 0) { Players_AppendStatus("No command to execute"); return; }

            string[] args  = text.SplitSpaces(2);
            string cmdName = args[0], cmdArgs = args.Length > 1 ? args[1] : "";
            
            CommandData data = default(CommandData);
            data.Rank    = LevelPermission.Flames;
            data.Context = CommandContext.SendCmd;
            curPlayer.HandleCommand(cmdName, cmdArgs, data);
                
            if (args.Length > 1) {
                Players_AppendStatus("Made player do /" + cmdName + " " + cmdArgs);
            } else {
                Players_AppendStatus("Made player do /" + cmdName);
            }
            pl_txtSendCommand.Text = "";
        }

        public void pl_BtnMute_Click(object sender, EventArgs e)  {
            if (curPlayer != null && !curPlayer.muted) {
                DoCmd("mute", "Muted @p"); 
            } else {
                DoCmd("unmute", "Unmuted @p");
            }
        }

        public void pl_BtnFreeze_Click(object sender, EventArgs e) {
            if (curPlayer != null && !curPlayer.frozen) {
                DoCmd("freeze", "Froze @p", "10m"); 
            } else {
                DoCmd("freeze", "Unfroze @p");
            }
        }

        public void pl_BtnWarn_Click(object sender, EventArgs e)  { DoCmd("warn", "Warned @p"); }
        public void pl_BtnKick_Click(object sender, EventArgs e)  { DoCmd("kick", "Kicked @p"); }
        public void pl_BtnBan_Click(object sender, EventArgs e)   { DoCmd("ban", "Banned @p"); }
        public void pl_BtnIPBan_Click(object sender, EventArgs e) { DoCmd("banip", "IP-Banned @p"); }
        public void pl_BtnKill_Click(object sender, EventArgs e)  { DoCmd("kill", "Killed @p"); }
        public void pl_BtnRules_Click(object sender, EventArgs e) { DoCmd("Rules", "Sent rules to @p"); }

        public void DoCmd(string cmdName, string action, string suffix = null) {
            if (curPlayer == null) { NoPlayerSelected(); return; }
            
            string cmd = (cmdName + " " + curPlayer.name + " " + suffix).Trim();
            UIHelpers.HandleCommand(cmd);
            
            Players_AppendStatus(action.Replace("@p", curPlayer.truename));
            Players_UpdateButtons();
        }

        public void pl_listBox_Click(object sender, EventArgs e) {
            Player p = PlayerInfo.FindExact(pl_listBox.Text);
            if (p == null || p == curPlayer) return;
            
            pl_statusBox.Text = "";
            Players_AppendStatus("==" + p.truename + "==");
            curPlayer = p;
            
            Players_SetSelected(p.truename, new PlayerProperties(p));
            Players_UpdateSelected();
        }

        public void pl_txtSendCommand_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) pl_BtnSendCommand_Click(sender, e);
        }
        public void pl_numUndo_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) pl_BtnUndo_Click(sender, e);
        }
        public void pl_txtMessage_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) pl_BtnMessage_Click(sender, e);
        }

        public void Players_AppendStatus(string text) {
            pl_statusBox.AppendText(text + Environment.NewLine);
        }

        public void Players_UpdateSelected() {
            if (tabs.SelectedTab != tp_Players) return;
            try { pl_pgProps.Refresh(); } catch { }
        }

        public void Players_UpdateList() {
            pl_listBox.Items.Clear();
            Player[] players = PlayerInfo.Online.Items;
            foreach (Player p in players) {
                pl_listBox.Items.Add(p.truename);
            }
            
            if (curPlayer == null) return;
            if (PlayerInfo.FindExact(curPlayer.name) != null) return;
            
            curPlayer = null;
            Players_SetSelected("(none selected)", null);
        }

        public void Players_SetSelected(string name, PlayerProperties props) {
            playerProps     = props;
            pl_gbProps.Text = "Properties for " + name;
            
            pl_pgProps.SelectedObject = props;
            Players_UpdateButtons();
        }

        public void Players_UpdateButtons() {
            Player p = curPlayer;  
            pl_btnMute.Text   = p != null && p.muted  ? "Unmute"   : "Mute";
            pl_btnFreeze.Text = p != null && p.frozen ? "Unfreeze" : "Freeze";
            // TODO: Automatically update when player is muted/frozen in-game
        }
    }
}