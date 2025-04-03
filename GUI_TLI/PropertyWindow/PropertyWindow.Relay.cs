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
using Flames.Modules.Relay.Discord;
using Flames.Modules.Relay.IRC;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public void LoadRelayProps()
        {
            LoadIRCProps();
            LoadDiscordProps();
        }
        public void ApplyRelayProps()
        {
            ApplyIRCProps();
            ApplyDiscordProps();
        }
        public void LoadIRCProps()
        {
            Irc_chkEnabled.Checked = Server.Config.UseIRC;
            Irc_txtServer.Text = Server.Config.IRCServer;
            Irc_numPort.Value = Server.Config.IRCPort;
            Irc_txtNick.Text = Server.Config.IRCNick;
            Irc_txtChannel.Text = Server.Config.IRCChannels;
            Irc_txtOpChannel.Text = Server.Config.IRCOpChannels;
            Irc_chkPass.Checked = Server.Config.IRCIdentify;
            Irc_txtPass.Text = Server.Config.IRCPassword;
            Irc_cbTitles.Checked = Server.Config.IRCShowPlayerTitles;
            Irc_cbWorldChanges.Checked = Server.Config.IRCShowWorldChanges;
            Irc_cbAFK.Checked = Server.Config.IRCShowAFK;
            ToggleIrcSettings(Server.Config.UseIRC);
            GuiPerms.SetRanks(Irc_cmbRank);
            GuiPerms.SetSelectedRank(Irc_cmbRank, Server.Config.IRCControllerRank);
            Irc_cmbVerify.Items.AddRange(Enum.GetNames(typeof(IRCControllerVerify)));
            Irc_cmbVerify.SelectedIndex = (int)Server.Config.IRCVerify;
            Irc_txtPrefix.Text = Server.Config.IRCCommandPrefix;
        }
        public void ApplyIRCProps()
        {
            Server.Config.UseIRC = Irc_chkEnabled.Checked;
            Server.Config.IRCServer = Irc_txtServer.Text;
            Server.Config.IRCPort = (int)Irc_numPort.Value;
            Server.Config.IRCNick = Irc_txtNick.Text;
            Server.Config.IRCChannels = Irc_txtChannel.Text;
            Server.Config.IRCOpChannels = Irc_txtOpChannel.Text;
            Server.Config.IRCIdentify = Irc_chkPass.Checked;
            Server.Config.IRCPassword = Irc_txtPass.Text;
            Server.Config.IRCShowPlayerTitles = Irc_cbTitles.Checked;
            Server.Config.IRCShowWorldChanges = Irc_cbWorldChanges.Checked;
            Server.Config.IRCShowAFK = Irc_cbAFK.Checked;
            Server.Config.IRCControllerRank = GuiPerms.GetSelectedRank(Irc_cmbRank, LevelPermission.Admin);
            Server.Config.IRCVerify = (IRCControllerVerify)Irc_cmbVerify.SelectedIndex;
            Server.Config.IRCCommandPrefix = Irc_txtPrefix.Text;
        }
        public void ToggleIrcSettings(bool enabled)
        {
            Irc_txtServer.Enabled = enabled; 
            Irc_lblServer.Enabled = enabled;
            Irc_numPort.Enabled = enabled; 
            Irc_lblPort.Enabled = enabled;
            Irc_txtNick.Enabled = enabled; 
            Irc_lblNick.Enabled = enabled;
            Irc_txtChannel.Enabled = enabled; 
            Irc_lblChannel.Enabled = enabled;
            Irc_txtOpChannel.Enabled = enabled; 
            Irc_lblOpChannel.Enabled = enabled;
            Irc_chkPass.Enabled = enabled; 
            Irc_txtPass.Enabled = enabled && Irc_chkPass.Checked;
            Irc_cbTitles.Enabled = enabled;
            Irc_cbWorldChanges.Enabled = enabled;
            Irc_cbAFK.Enabled = enabled;
            Irc_lblRank.Enabled = enabled; 
            Irc_cmbRank.Enabled = enabled;
            Irc_lblVerify.Enabled = enabled; 
            Irc_cmbVerify.Enabled = enabled;
            Irc_lblPrefix.Enabled = enabled; 
            Irc_txtPrefix.Enabled = enabled;
        }
        public void Irc_chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ToggleIrcSettings(Irc_chkEnabled.Checked);
        }
        public void Irc_chkPass_CheckedChanged(object sender, EventArgs e)
        {
            Irc_txtPass.Enabled = Irc_chkEnabled.Checked && Irc_chkPass.Checked;
        }
        public void LoadDiscordProps()
        {
            DiscordConfig cfg = DiscordPlugin.Config;
            Dis_chkEnabled.Checked = cfg.Enabled;
            Dis_txtToken.Text = cfg.BotToken;
            Dis_txtChannel.Text = cfg.Channels;
            Dis_txtOpChannel.Text = cfg.OpChannels;
            Dis_chkNicks.Checked = cfg.UseNicks;
            ToggleDiscordSettings(cfg.Enabled);
        }
        public void ApplyDiscordProps()
        {
            DiscordConfig cfg = DiscordPlugin.Config;
            cfg.Enabled = Dis_chkEnabled.Checked;
            cfg.BotToken = Dis_txtToken.Text;
            cfg.Channels = Dis_txtChannel.Text;
            cfg.OpChannels = Dis_txtOpChannel.Text;
            cfg.UseNicks = Dis_chkNicks.Checked;
        }
        public void SaveDiscordProps()
        {
            DiscordPlugin.Config.Save();
        }
        public void ToggleDiscordSettings(bool enabled)
        {
            Dis_txtToken.Enabled = enabled; 
            Dis_lblToken.Enabled = enabled;
            Dis_txtChannel.Enabled = enabled; 
            Dis_lblChannel.Enabled = enabled;
            Dis_txtOpChannel.Enabled = enabled; 
            Dis_lblOpChannel.Enabled = enabled;
            Dis_chkNicks.Enabled = enabled;
        }
        public void Dis_chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ToggleDiscordSettings(Dis_chkEnabled.Checked);
        }
        public void Dis_lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GuiUtils.OpenBrowser(Updater.WikiURL + "Discord-relay-bot/");
        }
    }
}