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
using Flames.SQL;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public void LoadMiscProps()
        {
            Bak_numTime.Value = Server.Config.BackupInterval;
            Bak_txtLocation.Text = Server.Config.BackupDirectory;
            Hack_lbl.Checked = Server.Config.HackrankKicks;
            Hack_num.Value = Server.Config.HackrankKickDelay;
            Afk_numTimer.Value = Server.Config.AutoAfkTime;
            ChkPhysRestart.Checked = Server.Config.PhysicsRestart;
            TxtRP.Text = Server.Config.PhysicsRestartLimit.ToString();
            TxtNormRp.Text = Server.Config.PhysicsRestartNormLimit.ToString();
            ChkDeath.Checked = Server.Config.AnnounceDeathCount;
            ChkSmile.Checked = Server.Config.ParseEmotes;
            Chk17Dollar.Checked = Server.Config.DollarNames;
            ChkRepeatMessages.Checked = Server.Config.RepeatMBs;
            ChkGuestLimitNotify.Checked = Server.Config.GuestLimitNotify;
            Misc_numReview.Value = Server.Config.ReviewCooldown;
            ChkRestart.Checked = Server.Config.restartOnError;
        }
        public void ApplyMiscProps()
        {
            Server.Config.BackupInterval = Bak_numTime.Value;
            Server.Config.BackupDirectory = Bak_txtLocation.Text;
            Server.Config.HackrankKicks = Hack_lbl.Checked;
            Server.Config.HackrankKickDelay = Hack_num.Value;
            Server.Config.AutoAfkTime = Afk_numTimer.Value;
            Server.Config.PhysicsRestart = ChkPhysRestart.Checked;
            Server.Config.PhysicsRestartLimit = int.Parse(TxtRP.Text);
            Server.Config.PhysicsRestartNormLimit = int.Parse(TxtNormRp.Text);
            Server.Config.AnnounceDeathCount = ChkDeath.Checked;
            Server.Config.ParseEmotes = ChkSmile.Checked;
            Server.Config.DollarNames = Chk17Dollar.Checked;
            Server.Config.RepeatMBs = ChkRepeatMessages.Checked;
            Server.Config.GuestLimitNotify = ChkGuestLimitNotify.Checked;
            Server.Config.ReviewCooldown = Misc_numReview.Value;
            Server.Config.restartOnError = ChkRestart.Checked;
        }
        public void Adv_btnEditTexts_Click(object sender, EventArgs e)
        {
            using Form form = new EditText();
            form.ShowDialog();
        }
        public void LoadSqlProps()
        {
            Sql_chkUseSQL.Checked = Server.Config.UseMySQL;
            Sql_txtUser.Text = Server.Config.MySQLUsername;
            Sql_txtPass.Text = Server.Config.MySQLPassword;
            Sql_txtDBName.Text = Server.Config.MySQLDatabaseName;
            Sql_txtHost.Text = Server.Config.MySQLHost;
            Sql_txtPort.Text = Server.Config.MySQLPort;
            ToggleMySQLSettings(Server.Config.UseMySQL);
        }
        public void ApplySqlProps()
        {
            Server.Config.UseMySQL = Sql_chkUseSQL.Checked;
            Server.Config.MySQLUsername = Sql_txtUser.Text;
            Server.Config.MySQLPassword = Sql_txtPass.Text;
            Server.Config.MySQLDatabaseName = Sql_txtDBName.Text;
            Server.Config.MySQLHost = Sql_txtHost.Text;
            Server.Config.MySQLPort = Sql_txtPort.Text;
            Database.UpdateActiveBackend();
            //Server.Config.MySQLPooling = ; // No setting for this?            
        }
        public void ToggleMySQLSettings(bool enabled)
        {
            Sql_txtUser.Enabled = enabled; 
            Sql_lblUser.Enabled = enabled;
            Sql_txtPass.Enabled = enabled; 
            Sql_lblPass.Enabled = enabled;
            Sql_txtPort.Enabled = enabled; 
            Sql_lblPort.Enabled = enabled;
            Sql_txtHost.Enabled = enabled; 
            Sql_lblHost.Enabled = enabled;
            Sql_txtDBName.Enabled = enabled; 
            Sql_lblDBName.Enabled = enabled;
        }
        public void Sql_linkDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GuiUtils.OpenBrowser("https://dev.mysql.com/downloads/");
        }
        public void Sql_chkUseSQL_CheckedChanged(object sender, EventArgs e)
        {
            ToggleMySQLSettings(Sql_chkUseSQL.Checked);
        }
    }
}