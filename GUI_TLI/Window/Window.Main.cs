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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Flames.Added.UI;
namespace Flames.Gui
{
    public partial class Window : Form
    {
        public Player GetSelectedPlayer()
        {
            string name = GetSelected(Main_Players);
            if (name == null)
            {
                return null;
            }
            return PlayerInfo.FindExact(name);
        }
        public void PlayerOrd(string order)
        {
            Player player = GetSelectedPlayer();
            if (player == null)
            {
                return;
            }
            UIHelpers.HandleOrder(order + " " + player.name);
        }
        public void PlayerOrd(string order, string prefix, string suffix)
        {
            Player player = GetSelectedPlayer();
            if (player == null)
            {
                return;
            }
            UIHelpers.HandleOrder(order + " " + prefix + player.name + suffix);
        }
        public void TsPlayer_Clones_Click(object sender, EventArgs e) 
        { 
            PlayerOrd("Clones"); 
        }
        public void TsPlayer_Voice_Click(object sender, EventArgs e) 
        { 
            PlayerOrd("Voice");
        }
        public void TsPlayer_Whois_Click(object sender, EventArgs e) 
        { 
            PlayerOrd("WhoIs"); 
        }
        public void TsPlayer_Ban_Click(object sender, EventArgs e) 
        { 
            PlayerOrd("Ban"); 
        }
        public void TsPlayer_Kick_Click(object sender, EventArgs e) 
        { 
            PlayerOrd("Kick"); 
        }
        public void TsPlayer_Promote_Click(object sender, EventArgs e) 
        {
            PlayerOrd("SetRank", "+up ", "");
        }
        public void TsPlayer_Demote_Click(object sender, EventArgs e) 
        { 
            PlayerOrd("SetRank", "-down ", ""); 
        }
        public Level GetSelectedLevel()
        {
            string name = GetSelected(Main_Maps);
            if (name == null)
            {
                return null;
            }
            return LevelInfo.FindExact(name);
        }
        public void LevelOrd(string order)
        {
            Level level = GetSelectedLevel();
            if (level == null)
            {
                return;
            }
            UIHelpers.HandleOrder(order + " " + level.name);
        }
        public void LevelOrd(string order, string prefix, string suffix)
        {
            Level level = GetSelectedLevel();
            if (level == null)
            {
                return;
            }
            UIHelpers.HandleOrder(order + " " + prefix + level.name + suffix);
        }
        public void TsMap_Info_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Map"); 
            LevelOrd("MapInfo"); 
        }
        public void TsMap_MoveAll_Click(object sender, EventArgs e) 
        { 
            LevelOrd("MoveAll"); 
        }
        public void TsMap_Physics0_Click(object sender, EventArgs e)
        { 
            LevelOrd("Physics", "", " 0"); 
        }
        public void TsMap_Physics1_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Physics", "", " 1"); 
        }
        public void TsMap_Physics2_Click(object sender, EventArgs e)
        { 
            LevelOrd("Physics", "", " 2"); 
        }
        public void TsMap_Physics3_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Physics", "", " 3"); 
        }
        public void TsMap_Physics4_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Physics", "", " 4"); 
        }
        public void TsMap_Physics5_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Physics", "", " 5"); 
        }
        public void TsMap_Save_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Save"); 
        }
        public void TsMap_Unload_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Unload"); 
        }
        public void TsMap_Reload_Click(object sender, EventArgs e) 
        { 
            LevelOrd("Reload"); 
        }
        public List<string> InputLog = new List<string>(21);
        public int InputIndex = -1;
        public void Main_TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                InputIndex = Math.Min(InputIndex + 1, InputLog.Count - 1);
                if (InputIndex > -1)
                {
                    SetInputText();
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                InputIndex = Math.Max(InputIndex - 1, -1);
                if (InputIndex > -1)
                {
                    SetInputText();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                InputText();
            }
            else
            {
                InputIndex = -1; 
                return;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        public void SetInputText()
        {
            if (InputIndex == -1)
            {
                return;
            }
            Main_txtInput.Text = InputLog[InputIndex];
            Main_txtInput.SelectionLength = 0;
            Main_txtInput.SelectionStart = Main_txtInput.Text.Length;
        }
        public void AddInputLog(string text)
        {
            // Simplify navigating through input history by not logging duplicate entries
            if (InputLog.Count > 0 && text == InputLog[0])
            {
                return;
            }
            InputLog.Insert(0, text);
            if (InputLog.Count > 20)
            {
                InputLog.RemoveAt(20);
            }
        }
        public void InputText()
        {
            string text = Main_txtInput.Text;
            if (text.Length == 0)
            {
                return;
            }
            AddInputLog(text);
            if (text == "/")
            {
                UIHelpers.RepeatOrder();
            }
            else if (text[0] == '/' && text.Length > 1 && text[1] == '/')
            {
                UIHelpers.HandleChat(text.Substring(1));
            }
            else if (text[0] == '/')
            {
                UIHelpers.HandleOrder(text.Substring(1));
            }
            else
            {
                UIHelpers.HandleChat(text);
            }
            Main_txtInput.Clear();
        }
        public void Main_BtnRestart_Click(object sender, EventArgs e)
        {
            if (Popup.OKCancel("Are you sure you want to restart?", "Restart"))
            {
                Server.Stop(true, Server.Config.DefaultRestartMessage);
            }
        }
        public void Main_TxtUrl_DoubleClick(object sender, EventArgs e)
        {
            if (!Main_IsUsingUrl())
            {
                return;
            }
            GuiUtils.OpenBrowser(Main_txtUrl.Text);
        }
        public void Main_BtnSaveAll_Click(object sender, EventArgs e)
        {
            UIHelpers.HandleOrder("Save all");
        }
        public void Main_BtnKillPhysics_Click(object sender, EventArgs e)
        {
            UIHelpers.HandleOrder("Physics kill");
        }
        public void Main_BtnUnloadEmpty_Click(object sender, EventArgs e)
        {
            UIHelpers.HandleOrder("Unload empty");
        }
        public void TsLog_Night_Click(object sender, EventArgs e)
        {
            Main_txtLog.NightMode = TsLog_night.Checked;
            TsLog_night.Checked = !TsLog_night.Checked;
        }
        public void TsLog_Colored_Click(object sender, EventArgs e)
        {
            Main_txtLog.Colorize = !TsLog_Colored.Checked;
            TsLog_Colored.Checked = !TsLog_Colored.Checked;
        }
        public void TsLog_DateStamp_Click(object sender, EventArgs e)
        {
            Main_txtLog.DateStamp = !TsLog_dateStamp.Checked;
            TsLog_dateStamp.Checked = !TsLog_dateStamp.Checked;
        }
        public void TsLog_AutoScroll_Click(object sender, EventArgs e)
        {
            Main_txtLog.AutoScroll = !TsLog_autoScroll.Checked;
            TsLog_autoScroll.Checked = !TsLog_autoScroll.Checked;
        }
        public void TsLog_CopySelected_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Main_txtLog.SelectedText))
            {
                return;
            }
            Clipboard.SetText(Main_txtLog.SelectedText, TextDataFormat.Text);
        }
        public void TsLog_CopyAll_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Main_txtLog.Text, TextDataFormat.Text);
        }
        public void TsLog_Clear_Click(object sender, EventArgs e)
        {
            if (Popup.OKCancel("Are you sure you want to clear logs?", "Clear logs"))
            {
                Main_txtLog.ClearLog();
            }
        }
        public bool Main_IsUsingUrl()
        {
            return Uri.TryCreate(Main_txtUrl.Text, UriKind.Absolute, out Uri uri);
        }
        public void Main_UpdateUrl(string s)
        {
            Main_txtUrl.Text = s;
            bool isUrl = Main_IsUsingUrl();
            Color linkCol = Color.FromArgb(255, 0, 102, 204);
            // https://stackoverflow.com/questions/20688408/how-do-you-change-the-text-color-of-a-readonly-textbox
            Main_txtUrl.BackColor = Main_txtUrl.BackColor;
            Main_txtUrl.ForeColor = isUrl ? linkCol : SystemColors.WindowText;
            Main_txtUrl.Font = new Font(Main_txtUrl.Font,
                                             isUrl ? FontStyle.Underline : FontStyle.Regular);
        }
        public void Main_UpdateMapList()
        {
            Level[] loaded = LevelInfo.Loaded.Items;
            string selected = GetSelected(Main_Maps);
            Main_Maps.Rows.Clear();
            foreach (Level lvl in loaded)
            {
                Main_Maps.Rows.Add(lvl.name, lvl.players.Count, lvl.physics);
            }
            Reselect(Main_Maps, selected);
            Main_Maps.Refresh();
        }
        public void Main_UpdatePlayersList()
        {
            UpdateNotifyIconText();
            Player[] players = PlayerInfo.Online.Items;
            string selected = GetSelected(Main_Players);
            Main_Players.Rows.Clear();
            foreach (Player pl in players)
            {
                Main_Players.Rows.Add(pl.truename, pl.level.name, pl.group.Name);
            }
            Reselect(Main_Players, selected);
            Main_Players.Refresh();
        }
        public static string GetSelected(DataGridView view)
        {
            DataGridViewSelectedRowCollection selected = view.SelectedRows;
            if (selected.Count <= 0)
            {
                return null;
            }
            return (string)selected[0].Cells[0].Value;
        }
        public static void Reselect(DataGridView view, string selected)
        {
            if (selected == null)
            {
                return;
            }
            foreach (DataGridViewRow row in view.Rows)
            {
                string name = (string)row.Cells[0].Value;
                if (name.CaselessEq(selected))
                {
                    row.Selected = true;
                }
            }
        }
    }
}