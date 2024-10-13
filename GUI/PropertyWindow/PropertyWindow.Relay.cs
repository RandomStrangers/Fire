﻿/*
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

namespace Flames.Gui {

    public partial class PropertyWindow : Form {

        public void LoadRelayProps() {
            LoadIRCProps();
            LoadDiscordProps();
        }

        public void ApplyRelayProps() {
            ApplyIRCProps();
            ApplyDiscordProps();
        }


        public void LoadIRCProps() {
            irc_chkEnabled.Checked = Server.Config.UseIRC;
            irc_txtServer.Text = Server.Config.IRCServer;
            irc_numPort.Value = Server.Config.IRCPort;
            irc_txtNick.Text = Server.Config.IRCNick;
            irc_txtChannel.Text = Server.Config.IRCChannels;
            irc_txtOpChannel.Text = Server.Config.IRCOpChannels;
            irc_chkPass.Checked = Server.Config.IRCIdentify;
            irc_txtPass.Text = Server.Config.IRCPassword;
            
            irc_cbTitles.Checked = Server.Config.IRCShowPlayerTitles;
            irc_cbWorldChanges.Checked = Server.Config.IRCShowWorldChanges;
            irc_cbAFK.Checked = Server.Config.IRCShowAFK;
            ToggleIrcSettings(Server.Config.UseIRC);

            GuiPerms.SetRanks(irc_cmbRank);
            GuiPerms.SetSelectedRank(irc_cmbRank, Server.Config.IRCControllerRank);
            irc_cmbVerify.Items.AddRange(Enum.GetNames(typeof(IRCControllerVerify)));
            irc_cmbVerify.SelectedIndex = (int)Server.Config.IRCVerify;
            irc_txtPrefix.Text = Server.Config.IRCCommandPrefix;
        }

        public void ApplyIRCProps() {
            Server.Config.UseIRC = irc_chkEnabled.Checked;
            Server.Config.IRCServer = irc_txtServer.Text;
            Server.Config.IRCPort = (int)irc_numPort.Value;
            Server.Config.IRCNick = irc_txtNick.Text;
            Server.Config.IRCChannels = irc_txtChannel.Text;
            Server.Config.IRCOpChannels = irc_txtOpChannel.Text;
            Server.Config.IRCIdentify = irc_chkPass.Checked;
            Server.Config.IRCPassword = irc_txtPass.Text;
            
            Server.Config.IRCShowPlayerTitles = irc_cbTitles.Checked;
            Server.Config.IRCShowWorldChanges = irc_cbWorldChanges.Checked;
            Server.Config.IRCShowAFK = irc_cbAFK.Checked;
            
            Server.Config.IRCControllerRank = GuiPerms.GetSelectedRank(irc_cmbRank, LevelPermission.Admin);
            Server.Config.IRCVerify = (IRCControllerVerify)irc_cmbVerify.SelectedIndex;
            Server.Config.IRCCommandPrefix = irc_txtPrefix.Text;  
        }

        public void ToggleIrcSettings(bool enabled) {
            irc_txtServer.Enabled = enabled; irc_lblServer.Enabled = enabled;
            irc_numPort.Enabled = enabled; irc_lblPort.Enabled = enabled;
            irc_txtNick.Enabled = enabled; irc_lblNick.Enabled = enabled;
            irc_txtChannel.Enabled = enabled; irc_lblChannel.Enabled = enabled;
            irc_txtOpChannel.Enabled = enabled; irc_lblOpChannel.Enabled = enabled;    
            irc_chkPass.Enabled = enabled; irc_txtPass.Enabled = enabled && irc_chkPass.Checked;
            
            irc_cbTitles.Enabled = enabled;
            irc_cbWorldChanges.Enabled = enabled;
            irc_cbAFK.Enabled = enabled;           
            irc_lblRank.Enabled = enabled; irc_cmbRank.Enabled = enabled;
            irc_lblVerify.Enabled = enabled; irc_cmbVerify.Enabled = enabled;
            irc_lblPrefix.Enabled = enabled; irc_txtPrefix.Enabled = enabled;
        }

        public void irc_chkEnabled_CheckedChanged(object sender, EventArgs e) {
            ToggleIrcSettings(irc_chkEnabled.Checked);
        }

        public void irc_chkPass_CheckedChanged(object sender, EventArgs e) {
            irc_txtPass.Enabled = irc_chkEnabled.Checked && irc_chkPass.Checked;
        }


        public void LoadDiscordProps() {
            DiscordConfig cfg = DiscordPlugin.Config;
            dis_chkEnabled.Checked = cfg.Enabled;
            dis_txtToken.Text      = cfg.BotToken;
            dis_txtChannel.Text    = cfg.Channels;
            dis_txtOpChannel.Text  = cfg.OpChannels;
            dis_chkNicks.Checked   = cfg.UseNicks;
            ToggleDiscordSettings(cfg.Enabled);
        }

        public void ApplyDiscordProps() {
            DiscordConfig cfg = DiscordPlugin.Config;
            cfg.Enabled    = dis_chkEnabled.Checked;
            cfg.BotToken   = dis_txtToken.Text;
            cfg.Channels   = dis_txtChannel.Text;
            cfg.OpChannels = dis_txtOpChannel.Text;
            cfg.UseNicks   = dis_chkNicks.Checked;
        }

        public void SaveDiscordProps() {
            DiscordPlugin.Config.Save();
        }

        public void ToggleDiscordSettings(bool enabled) {
            dis_txtToken.Enabled = enabled; dis_lblToken.Enabled = enabled;
            dis_txtChannel.Enabled = enabled; dis_lblChannel.Enabled = enabled;
            dis_txtOpChannel.Enabled = enabled; dis_lblOpChannel.Enabled = enabled;
            dis_chkNicks.Enabled = enabled;
        }

        public void dis_chkEnabled_CheckedChanged(object sender, EventArgs e) {
            ToggleDiscordSettings(dis_chkEnabled.Checked);
        }

        public void dis_lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            GuiUtils.OpenBrowser(Updater.WikiURL + "Discord-relay-bot/");
        }
    }
}