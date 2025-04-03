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
using System.ComponentModel;
using System.Drawing;
namespace Flames.Gui
{
    public partial class Window
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
            this.TsMap = new ContextMenuStrip(this.Components);
            this.TsMap_physicsMenu = new ToolStripMenuItem();
            this.TsMap_physics0 = new ToolStripMenuItem();
            this.TsMap_physics1 = new ToolStripMenuItem();
            this.TsMap_physics2 = new ToolStripMenuItem();
            this.TsMap_physics3 = new ToolStripMenuItem();
            this.TsMap_physics4 = new ToolStripMenuItem();
            this.TsMap_physics5 = new ToolStripMenuItem();
            this.TsMap_actionsMenu = new ToolStripMenuItem();
            this.TsMap_Save = new ToolStripMenuItem();
            this.TsMap_Reload = new ToolStripMenuItem();
            this.TsMap_Unload = new ToolStripMenuItem();
            this.TsMap_moveAll = new ToolStripMenuItem();
            this.TsMap_separator = new ToolStripSeparator();
            this.TsMap_info = new ToolStripMenuItem();
            this.TsPlayer = new ContextMenuStrip(this.Components);
            this.TsPlayer_whois = new ToolStripMenuItem();
            this.TsPlayer_kick = new ToolStripMenuItem();
            this.TsPlayer_ban = new ToolStripMenuItem();
            this.TsPlayer_voice = new ToolStripMenuItem();
            this.TsPlayer_clones = new ToolStripMenuItem();
            this.TsPlayer_promote = new ToolStripMenuItem();
            this.TsPlayer_demote = new ToolStripMenuItem();
            this.Icon_context = new ContextMenuStrip(this.Components);
            this.Icon_separator = new ToolStripSeparator();
            this.Icon_hideWindow = new ToolStripMenuItem();
            this.Icon_openTerminal = new ToolStripMenuItem();
            this.Icon_shutdown = new ToolStripMenuItem();
            this.Icon_restart = new ToolStripMenuItem();
            this.Main_btnProps = new Button();
            this.Main_btnClose = new Button();
            this.Main_btnRestart = new Button();
            this.Logs_tp = new TabPage();
            this.Logs_tab = new TabControl();
            this.Logs_tabErr = new TabPage();
            this.Logs_txtError = new TextBox();
            this.Logs_tabGen = new TabPage();
            this.Logs_lblGeneral = new Label();
            this.Logs_dateGeneral = new DateTimePicker();
            this.Logs_txtGeneral = new RichTextBox();
            this.TabLog_Sys = new TabPage();
            this.Logs_txtSystem = new TextBox();
            this.Tp_Main = new TabPage();
            this.Main_btnUnloadEmpty = new Button();
            this.Main_btnKillPhysics = new Button();
            this.Main_btnSaveAll = new Button();
            this.Main_colLvlName = new DataGridViewTextBoxColumn();
            this.Main_colLvlPlayers = new DataGridViewTextBoxColumn();
            this.Main_colLvlPhysics = new DataGridViewTextBoxColumn();
            this.Main_Maps = new DataGridView();
            this.Main_txtLog = new Flames.Gui.Components.ColoredTextBox();
            this.TsLog_Menu = new ContextMenuStrip(this.Components);
            this.TsLog_night = new ToolStripMenuItem();
            this.TsLog_Colored = new ToolStripMenuItem();
            this.TsLog_dateStamp = new ToolStripMenuItem();
            this.TsLog_autoScroll = new ToolStripMenuItem();
            this.TsLog_separator1 = new ToolStripSeparator();
            this.TsLog_copySelected = new ToolStripMenuItem();
            this.TsLog_copyAll = new ToolStripMenuItem();
            this.TsLog_separator2 = new ToolStripSeparator();
            this.TsLog_clear = new ToolStripMenuItem();
            this.Main_txtInput = new TextBox();
            this.Main_txtUrl = new TextBox();
            this.Main_colPlName = new DataGridViewTextBoxColumn();
            this.Main_colPlMap = new DataGridViewTextBoxColumn();
            this.Main_colPlRank = new DataGridViewTextBoxColumn();
            this.Main_Players = new DataGridView();
            this.Tabs = new TabControl();
            this.Tp_Maps = new TabPage();
            this.Map_gbProps = new GroupBox();
            this.Map_pgProps = new Flames.Gui.HackyPropertyGrid();
            this.Map_gbLoaded = new GroupBox();
            this.Map_lbLoaded = new ListBox();
            this.Map_gbUnloaded = new GroupBox();
            this.Map_btnLoad = new Button();
            this.Map_lbUnloaded = new ListBox();
            this.Map_gbNew = new GroupBox();
            this.Map_btnGen = new Button();
            this.Map_lblType = new Label();
            this.Map_lblSeed = new Label();
            this.Map_lblZ = new Label();
            this.Map_lblX = new Label();
            this.Map_lblY = new Label();
            this.Map_txtSeed = new TextBox();
            this.Map_cmbType = new ComboBox();
            this.Map_cmbZ = new ComboBox();
            this.Map_cmbY = new ComboBox();
            this.Map_cmbX = new ComboBox();
            this.Map_lblName = new Label();
            this.Map_txtName = new TextBox();
            this.Tp_Players = new TabPage();
            this.Pl_gbProps = new GroupBox();
            this.Pl_pgProps = new Flames.Gui.HackyPropertyGrid();
            this.Pl_gbOther = new GroupBox();
            this.Pl_txtSendOrder = new TextBox();
            this.Pl_btnSendOrder = new Button();
            this.Pl_txtMessage = new TextBox();
            this.Pl_btnMessage = new Button();
            this.Pl_gbActions = new GroupBox();
            this.Pl_btnKill = new Button();
            this.Pl_numUndo = new Flames.Gui.TimespanUpDown();
            this.Pl_btnWarn = new Button();
            this.Pl_btnRules = new Button();
            this.Pl_btnKick = new Button();
            this.Pl_btnBanIP = new Button();
            this.Pl_btnUndo = new Button();
            this.Pl_btnMute = new Button();
            this.Pl_btnBan = new Button();
            this.Pl_btnFreeze = new Button();
            this.Pl_statusBox = new TextBox();
            this.Pl_listBox = new ListBox();
            this.Pl_lblOnline = new Label();
            this.GUIToolTip = new ToolTip(this.Components);
            this.TsMap.SuspendLayout();
            this.TsPlayer.SuspendLayout();
            this.Icon_context.SuspendLayout();
            this.Logs_tp.SuspendLayout();
            this.Logs_tab.SuspendLayout();
            this.Logs_tabErr.SuspendLayout();
            this.Logs_tabGen.SuspendLayout();
            this.TabLog_Sys.SuspendLayout();
            this.Tp_Main.SuspendLayout();
            ((ISupportInitialize)(this.Main_Maps)).BeginInit();
            this.TsLog_Menu.SuspendLayout();
            ((ISupportInitialize)(this.Main_Players)).BeginInit();
            this.Tabs.SuspendLayout();
            this.Tp_Maps.SuspendLayout();
            this.Map_gbProps.SuspendLayout();
            this.Map_gbLoaded.SuspendLayout();
            this.Map_gbUnloaded.SuspendLayout();
            this.Map_gbNew.SuspendLayout();
            this.Tp_Players.SuspendLayout();
            this.Pl_gbProps.SuspendLayout();
            this.Pl_gbOther.SuspendLayout();
            this.Pl_gbActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // TsMap
            // 
            this.TsMap.Items.AddRange(new ToolStripItem[] {
                                    this.TsMap_physicsMenu,
                                    this.TsMap_actionsMenu,
                                    this.TsMap_separator,
                                    this.TsMap_info});
            this.TsMap.Name = "mapsStrip";
            this.TsMap.Size = new Size(138, 76);
            // 
            // TsMap_physicsMenu
            // 
            this.TsMap_physicsMenu.DropDownItems.AddRange(new ToolStripItem[] {
                                    this.TsMap_physics0,
                                    this.TsMap_physics1,
                                    this.TsMap_physics2,
                                    this.TsMap_physics3,
                                    this.TsMap_physics4,
                                    this.TsMap_physics5});
            this.TsMap_physicsMenu.Name = "TsMap_physicsMenu";
            this.TsMap_physicsMenu.Size = new Size(137, 22);
            this.TsMap_physicsMenu.Text = "Physics Level";
            // 
            // TsMap_physics0
            // 
            this.TsMap_physics0.Name = "TsMap_physics0";
            this.TsMap_physics0.Size = new Size(152, 22);
            this.TsMap_physics0.Text = "Off";
            this.TsMap_physics0.Click += new EventHandler(this.TsMap_Physics0_Click);
            // 
            // TsMap_physics1
            // 
            this.TsMap_physics1.Name = "TsMap_physics1";
            this.TsMap_physics1.Size = new Size(152, 22);
            this.TsMap_physics1.Text = "Normal";
            this.TsMap_physics1.Click += new EventHandler(this.TsMap_Physics1_Click);
            // 
            // TsMap_physics2
            // 
            this.TsMap_physics2.Name = "TsMap_physics2";
            this.TsMap_physics2.Size = new Size(152, 22);
            this.TsMap_physics2.Text = "Advanced";
            this.TsMap_physics2.Click += new EventHandler(this.TsMap_Physics2_Click);
            // 
            // TsMap_physics3
            // 
            this.TsMap_physics3.Name = "TsMap_physics3";
            this.TsMap_physics3.Size = new Size(152, 22);
            this.TsMap_physics3.Text = "Hardcore";
            this.TsMap_physics3.Click += new EventHandler(this.TsMap_Physics3_Click);
            // 
            // TsMap_physics4
            // 
            this.TsMap_physics4.Name = "TsMap_physics4";
            this.TsMap_physics4.Size = new Size(152, 22);
            this.TsMap_physics4.Text = "Instant";
            this.TsMap_physics4.Click += new EventHandler(this.TsMap_Physics4_Click);
            // 
            // TsMap_physics5
            // 
            this.TsMap_physics5.Name = "TsMap_physics5";
            this.TsMap_physics5.Size = new Size(152, 22);
            this.TsMap_physics5.Text = "Doors-Only";
            this.TsMap_physics5.Click += new EventHandler(this.TsMap_Physics5_Click);
            // 
            // TsMap_actionsMenu
            // 
            this.TsMap_actionsMenu.DropDownItems.AddRange(new ToolStripItem[] {
                                    this.TsMap_Save,
                                    this.TsMap_Reload,
                                    this.TsMap_Unload,
                                    this.TsMap_moveAll});
            this.TsMap_actionsMenu.Name = "TsMap_actionsMenu";
            this.TsMap_actionsMenu.Size = new Size(137, 22);
            this.TsMap_actionsMenu.Text = "Actions";
            // 
            // TsMap_Save
            // 
            this.TsMap_Save.Name = "TsMap_Save";
            this.TsMap_Save.Size = new Size(152, 22);
            this.TsMap_Save.Text = "Save";
            this.TsMap_Save.Click += new EventHandler(this.TsMap_Save_Click);
            // 
            // TsMap_Reload
            // 
            this.TsMap_Reload.Name = "TsMap_Reload";
            this.TsMap_Reload.Size = new Size(152, 22);
            this.TsMap_Reload.Text = "Reload";
            this.TsMap_Reload.Click += new EventHandler(this.TsMap_Reload_Click);
            // 
            // TsMap_Unload
            // 
            this.TsMap_Unload.Name = "TsMap_Unload";
            this.TsMap_Unload.Size = new Size(152, 22);
            this.TsMap_Unload.Text = "Unload";
            this.TsMap_Unload.Click += new EventHandler(this.TsMap_Unload_Click);
            // 
            // TsMap_moveAll
            // 
            this.TsMap_moveAll.Name = "TsMap_moveAll";
            this.TsMap_moveAll.Size = new Size(152, 22);
            this.TsMap_moveAll.Text = "Move All";
            this.TsMap_moveAll.Click += new EventHandler(this.TsMap_MoveAll_Click);
            // 
            // TsMap_separator
            // 
            this.TsMap_separator.Name = "TsMap_separator";
            this.TsMap_separator.Size = new Size(134, 6);
            // 
            // TsMap_info
            // 
            this.TsMap_info.Name = "TsMap_info";
            this.TsMap_info.Size = new Size(137, 22);
            this.TsMap_info.Text = "Info";
            this.TsMap_info.Click += new EventHandler(this.TsMap_Info_Click);
            // 
            // TsPlayer
            // 
            this.TsPlayer.Items.AddRange(new ToolStripItem[] {
                                    this.TsPlayer_whois,
                                    this.TsPlayer_kick,
                                    this.TsPlayer_ban,
                                    this.TsPlayer_voice,
                                    this.TsPlayer_clones,
                                    this.TsPlayer_promote,
                                    this.TsPlayer_demote});
            this.TsPlayer.Name = "playerStrip";
            this.TsPlayer.Size = new Size(115, 158);
            // 
            // TsPlayer_whois
            // 
            this.TsPlayer_whois.Name = "TsPlayer_whois";
            this.TsPlayer_whois.Size = new Size(114, 22);
            this.TsPlayer_whois.Text = "Whois";
            this.TsPlayer_whois.Click += new EventHandler(this.TsPlayer_Whois_Click);
            // 
            // TsPlayer_kick
            // 
            this.TsPlayer_kick.Name = "TsPlayer_kick";
            this.TsPlayer_kick.Size = new Size(114, 22);
            this.TsPlayer_kick.Text = "Kick";
            this.TsPlayer_kick.Click += new EventHandler(this.TsPlayer_Kick_Click);
            // 
            // TsPlayer_ban
            // 
            this.TsPlayer_ban.Name = "TsPlayer_ban";
            this.TsPlayer_ban.Size = new Size(114, 22);
            this.TsPlayer_ban.Text = "Ban";
            this.TsPlayer_ban.Click += new EventHandler(this.TsPlayer_Ban_Click);
            // 
            // TsPlayer_voice
            // 
            this.TsPlayer_voice.Name = "TsPlayer_voice";
            this.TsPlayer_voice.Size = new Size(114, 22);
            this.TsPlayer_voice.Text = "Voice";
            this.TsPlayer_voice.Click += new EventHandler(this.TsPlayer_Voice_Click);
            // 
            // TsPlayer_clones
            // 
            this.TsPlayer_clones.Name = "TsPlayer_clones";
            this.TsPlayer_clones.Size = new Size(114, 22);
            this.TsPlayer_clones.Text = "Clones";
            this.TsPlayer_clones.Click += new EventHandler(this.TsPlayer_Clones_Click);
            // 
            // TsPlayer_promote
            // 
            this.TsPlayer_promote.Name = "TsPlayer_promote";
            this.TsPlayer_promote.Size = new Size(114, 22);
            this.TsPlayer_promote.Text = "Promote";
            this.TsPlayer_promote.Click += new EventHandler(this.TsPlayer_Promote_Click);
            // 
            // TsPlayer_demote
            // 
            this.TsPlayer_demote.Name = "TsPlayer_demote";
            this.TsPlayer_demote.Size = new Size(114, 22);
            this.TsPlayer_demote.Text = "Demote";
            this.TsPlayer_demote.Click += new EventHandler(this.TsPlayer_Demote_Click);
            // 
            // Icon_context
            // 
            this.Icon_context.Items.AddRange(new ToolStripItem[] {
                                    this.Icon_hideWindow,
                                    this.Icon_separator,
                                    this.Icon_openTerminal,
                                    this.Icon_shutdown,
                                    this.Icon_restart});
            this.Icon_context.Name = "iconContext";
            this.Icon_context.Size = new Size(158, 70);
            // 
            // Icon_separator
            // 
            this.Icon_separator.Name = "Icon_separator";
            this.Icon_separator.Size = new Size(157, 6);
            // 
            // Icon_hideWindow
            // 
            this.Icon_hideWindow.Name = "Icon_hideWindow";
            this.Icon_hideWindow.Size = new Size(157, 22);
            this.Icon_hideWindow.Text = "Hide from taskbar";
            this.Icon_hideWindow.Click += new EventHandler(this.Icon_HideWindow_Click);
            // 
            // Icon_openTerminal
            // 
            this.Icon_openTerminal.Name = "Icon_openTerminal";
            this.Icon_openTerminal.Size = new Size(157, 22);
            this.Icon_openTerminal.Text = "Open terminal";
            this.Icon_openTerminal.Click += new EventHandler(this.Icon_OpenTerminal_Click);
            // 
            // Icon_shutdown
            // 
            this.Icon_shutdown.Name = "Icon_shutdown";
            this.Icon_shutdown.Size = new Size(157, 22);
            this.Icon_shutdown.Text = "Shutdown server";
            this.Icon_shutdown.Click += new EventHandler(this.Icon_Shutdown_Click);
            // 
            // Icon_restart
            // 
            this.Icon_restart.Name = "Icon_restart";
            this.Icon_restart.Size = new Size(157, 22);
            this.Icon_restart.Text = "Restart server";
            this.Icon_restart.Click += new EventHandler(this.Icon_restart_Click);
            // 
            // Main_btnProps
            // 
            this.Main_btnProps.Cursor = Cursors.Hand;
            this.Main_btnProps.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnProps.Location = new Point(501, 5);
            this.Main_btnProps.Name = "Main_btnProps";
            this.Main_btnProps.Size = new Size(80, 23);
            this.Main_btnProps.TabIndex = 34;
            this.Main_btnProps.Text = "Settings";
            this.Main_btnProps.UseVisualStyleBackColor = true;
            this.Main_btnProps.Enabled = false;
            this.Main_btnProps.Click += new EventHandler(this.BtnProperties_Click);
            // 
            // Main_btnClose
            // 
            this.Main_btnClose.Cursor = Cursors.Hand;
            this.Main_btnClose.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnClose.Location = new Point(675, 5);
            this.Main_btnClose.Name = "Main_btnClose";
            this.Main_btnClose.Size = new Size(88, 23);
            this.Main_btnClose.TabIndex = 35;
            this.Main_btnClose.Text = "Close";
            this.Main_btnClose.UseVisualStyleBackColor = true;
            this.Main_btnClose.Click += new EventHandler(this.BtnClose_Click);
            // 
            // Main_btnRestart
            // 
            this.Main_btnRestart.Cursor = Cursors.Hand;
            this.Main_btnRestart.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnRestart.Location = new Point(584, 5);
            this.Main_btnRestart.Name = "Main_btnRestart";
            this.Main_btnRestart.Size = new Size(88, 23);
            this.Main_btnRestart.TabIndex = 36;
            this.Main_btnRestart.Text = "Restart";
            this.Main_btnRestart.UseVisualStyleBackColor = true;
            this.Main_btnRestart.Click += new EventHandler(this.Main_BtnRestart_Click);
            // 
            // Logs_tp
            // 
            this.Logs_tp.BackColor = SystemColors.Control;
            this.Logs_tp.Controls.Add(this.Logs_tab);
            this.Logs_tp.Location = new Point(4, 22);
            this.Logs_tp.Name = "Logs_tp";
            this.Logs_tp.Padding = new Padding(3);
            this.Logs_tp.Size = new Size(767, 488);
            this.Logs_tp.TabIndex = 4;
            this.Logs_tp.Text = "Logs";
            // 
            // Logs_tab
            // 
            this.Logs_tab.Controls.Add(this.Logs_tabErr);
            this.Logs_tab.Controls.Add(this.Logs_tabGen);
            this.Logs_tab.Controls.Add(this.TabLog_Sys);
            this.Logs_tab.Location = new Point(-1, 1);
            this.Logs_tab.Name = "Logs_tab";
            this.Logs_tab.SelectedIndex = 0;
            this.Logs_tab.Size = new Size(775, 491);
            this.Logs_tab.TabIndex = 0;
            // 
            // Logs_tabErr
            // 
            this.Logs_tabErr.Controls.Add(this.Logs_txtError);
            this.Logs_tabErr.Location = new Point(4, 22);
            this.Logs_tabErr.Name = "Logs_tabErr";
            this.Logs_tabErr.Size = new Size(767, 465);
            this.Logs_tabErr.TabIndex = 2;
            this.Logs_tabErr.Text = "Errors";
            this.Logs_tabErr.UseVisualStyleBackColor = true;
            // 
            // Logs_txtError
            // 
            this.Logs_txtError.BackColor = SystemColors.Window;
            this.Logs_txtError.Cursor = Cursors.Arrow;
            this.Logs_txtError.Location = new Point(-2, 0);
            this.Logs_txtError.Multiline = true;
            this.Logs_txtError.Name = "Logs_txtError";
            this.Logs_txtError.ReadOnly = true;
            this.Logs_txtError.ScrollBars = ScrollBars.Vertical;
            this.Logs_txtError.Size = new Size(765, 465);
            this.Logs_txtError.TabIndex = 2;
            // 
            // Logs_tabGen
            // 
            this.Logs_tabGen.Controls.Add(this.Logs_lblGeneral);
            this.Logs_tabGen.Controls.Add(this.Logs_dateGeneral);
            this.Logs_tabGen.Controls.Add(this.Logs_txtGeneral);
            this.Logs_tabGen.Location = new Point(4, 22);
            this.Logs_tabGen.Name = "Logs_tabGen";
            this.Logs_tabGen.Padding = new Padding(3);
            this.Logs_tabGen.Size = new Size(767, 465);
            this.Logs_tabGen.TabIndex = 0;
            this.Logs_tabGen.Text = "General";
            this.Logs_tabGen.UseVisualStyleBackColor = true;
            // 
            // Logs_lblGeneral
            // 
            this.Logs_lblGeneral.AutoSize = true;
            this.Logs_lblGeneral.Location = new Point(3, 9);
            this.Logs_lblGeneral.Name = "Logs_lblGeneral";
            this.Logs_lblGeneral.Size = new Size(78, 13);
            this.Logs_lblGeneral.TabIndex = 6;
            this.Logs_lblGeneral.Text = "View logs from:";
            // 
            // Logs_dateGeneral
            // 
            this.Logs_dateGeneral.Location = new Point(87, 4);
            this.Logs_dateGeneral.CalendarForeColor = SystemColors.WindowText;
            this.Logs_dateGeneral.Name = "Logs_dateGeneral";
            this.Logs_dateGeneral.Size = new Size(200, 21);
            this.Logs_dateGeneral.TabIndex = 5;
            this.Logs_dateGeneral.Value = new DateTime(2011, 7, 20, 18, 31, 50, 0);
            this.Logs_dateGeneral.ValueChanged += new EventHandler(this.logs_dateGeneral_Changed);
            // 
            // Logs_txtGeneral
            // 
            this.Logs_txtGeneral.BackColor = SystemColors.Window;
            this.Logs_txtGeneral.Location = new Point(-2, 30);
            this.Logs_txtGeneral.Name = "Logs_txtGeneral";
            this.Logs_txtGeneral.ReadOnly = true;
            this.Logs_txtGeneral.Size = new Size(765, 436);
            this.Logs_txtGeneral.TabIndex = 4;
            this.Logs_txtGeneral.Text = "";
            // 
            // TabLog_Sys
            // 
            this.TabLog_Sys.Controls.Add(this.Logs_txtSystem);
            this.TabLog_Sys.Location = new Point(4, 22);
            this.TabLog_Sys.Name = "TabLog_Sys";
            this.TabLog_Sys.Padding = new Padding(3);
            this.TabLog_Sys.Size = new Size(767, 465);
            this.TabLog_Sys.TabIndex = 1;
            this.TabLog_Sys.Text = "System";
            this.TabLog_Sys.UseVisualStyleBackColor = true;
            // 
            // Logs_txtSystem
            // 
            this.Logs_txtSystem.BackColor = SystemColors.Window;
            this.Logs_txtSystem.Cursor = Cursors.Arrow;
            this.Logs_txtSystem.Location = new Point(-2, 0);
            this.Logs_txtSystem.Multiline = true;
            this.Logs_txtSystem.Name = "Logs_txtSystem";
            this.Logs_txtSystem.ReadOnly = true;
            this.Logs_txtSystem.ScrollBars = ScrollBars.Vertical;
            this.Logs_txtSystem.Size = new Size(765, 465);
            this.Logs_txtSystem.TabIndex = 2;
            // 
            // Tp_Main
            // 
            this.Tp_Main.BackColor = SystemColors.Control;
            this.Tp_Main.Controls.Add(this.Main_btnUnloadEmpty);
            this.Tp_Main.Controls.Add(this.Main_btnKillPhysics);
            this.Tp_Main.Controls.Add(this.Main_btnSaveAll);
            this.Tp_Main.Controls.Add(this.Main_Maps);
            this.Tp_Main.Controls.Add(this.Main_txtLog);
            this.Tp_Main.Controls.Add(this.Main_txtInput);
            this.Tp_Main.Controls.Add(this.Main_txtUrl);
            this.Tp_Main.Controls.Add(this.Main_Players);
            this.Tp_Main.Location = new Point(4, 22);
            this.Tp_Main.Name = "Tp_Main";
            this.Tp_Main.Padding = new Padding(3);
            this.Tp_Main.Size = new Size(767, 488);
            this.Tp_Main.TabIndex = 0;
            this.Tp_Main.Text = "Main";
            // 
            // Main_btnUnloadEmpty
            // 
            this.Main_btnUnloadEmpty.Cursor = Cursors.Hand;
            this.Main_btnUnloadEmpty.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnUnloadEmpty.Location = new Point(676, 263);
            this.Main_btnUnloadEmpty.Name = "Main_btnUnloadEmpty";
            this.Main_btnUnloadEmpty.Size = new Size(81, 23);
            this.Main_btnUnloadEmpty.TabIndex = 41;
            this.Main_btnUnloadEmpty.Text = "Unload Empty";
            this.Main_btnUnloadEmpty.UseVisualStyleBackColor = true;
            this.Main_btnUnloadEmpty.Click += new EventHandler(this.Main_BtnUnloadEmpty_Click);
            // 
            // Main_btnKillPhysics
            // 
            this.Main_btnKillPhysics.Cursor = Cursors.Hand;
            this.Main_btnKillPhysics.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnKillPhysics.Location = new Point(582, 263);
            this.Main_btnKillPhysics.Name = "Main_btnKillPhysics";
            this.Main_btnKillPhysics.Size = new Size(88, 23);
            this.Main_btnKillPhysics.TabIndex = 40;
            this.Main_btnKillPhysics.Text = "Kill All Physics";
            this.Main_btnKillPhysics.UseVisualStyleBackColor = true;
            this.Main_btnKillPhysics.Click += new EventHandler(this.Main_BtnKillPhysics_Click);
            // 
            // Main_btnSaveAll
            // 
            this.Main_btnSaveAll.Cursor = Cursors.Hand;
            this.Main_btnSaveAll.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnSaveAll.Location = new Point(513, 263);
            this.Main_btnSaveAll.Name = "Main_btnSaveAll";
            this.Main_btnSaveAll.Size = new Size(63, 23);
            this.Main_btnSaveAll.TabIndex = 39;
            this.Main_btnSaveAll.Text = "Save All";
            this.Main_btnSaveAll.UseVisualStyleBackColor = true;
            this.Main_btnSaveAll.Click += new EventHandler(this.Main_BtnSaveAll_Click);
            // 
            // Main_colLvlName
            // 
            this.Main_colLvlName.HeaderText = "Name";
            this.Main_colLvlName.Name = "Main_colLvlName";
            this.Main_colLvlName.ReadOnly = true;
            // 
            // Main_colLvlPlayers
            // 
            this.Main_colLvlPlayers.FillWeight = 70F;
            this.Main_colLvlPlayers.HeaderText = "Players";
            this.Main_colLvlPlayers.Name = "Main_colLvlPlayers";
            this.Main_colLvlPlayers.ReadOnly = true;
            // 
            // Main_colLvlPhysics
            // 
            this.Main_colLvlPhysics.FillWeight = 70F;
            this.Main_colLvlPhysics.HeaderText = "Physics";
            this.Main_colLvlPhysics.Name = "Main_colLvlPhysics";
            this.Main_colLvlPhysics.ReadOnly = true;
            // 
            // Main_Maps
            // 
            this.Main_Maps.AllowUserToAddRows = false;
            this.Main_Maps.AllowUserToDeleteRows = false;
            this.Main_Maps.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Main_Maps.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.Main_Maps.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.Main_Maps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Main_Maps.Columns.AddRange(new DataGridViewColumn[] {
        	        	        	this.Main_colLvlName,
        	        	        	this.Main_colLvlPlayers,
        	        	        	this.Main_colLvlPhysics});
            this.Main_Maps.ContextMenuStrip = this.TsMap;
            this.Main_Maps.Location = new Point(512, 292);
            this.Main_Maps.MultiSelect = false;
            this.Main_Maps.Name = "Main_Maps";
            this.Main_Maps.ReadOnly = true;
            this.Main_Maps.RowHeadersVisible = false;
            this.Main_Maps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Main_Maps.Size = new Size(246, 150);
            this.Main_Maps.TabIndex = 38;
            // 
            // Main_txtLog
            // 
            this.Main_txtLog.BackColor = SystemColors.Window;
            this.Main_txtLog.ContextMenuStrip = this.TsLog_Menu;
            this.Main_txtLog.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_txtLog.Location = new Point(8, 38);
            this.Main_txtLog.Name = "Main_txtLog";
            this.Main_txtLog.ReadOnly = true;
            this.Main_txtLog.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            this.Main_txtLog.Size = new Size(498, 404);
            this.Main_txtLog.TabIndex = 0;
            this.Main_txtLog.Text = "";
            // 
            // TsLog_Menu
            // 
            this.TsLog_Menu.Items.AddRange(new ToolStripItem[] {
                                    this.TsLog_night,
                                    this.TsLog_Colored,
                                    this.TsLog_dateStamp,
                                    this.TsLog_autoScroll,
                                    this.TsLog_separator1,
                                    this.TsLog_copySelected,
                                    this.TsLog_copyAll,
                                    this.TsLog_separator2,
                                    this.TsLog_clear});
            this.TsLog_Menu.Name = "txtLogMenuStrip";
            this.TsLog_Menu.Size = new Size(144, 170);
            // 
            // TsLog_night
            // 
            this.TsLog_night.Name = "TsLog_night";
            this.TsLog_night.Size = new Size(143, 22);
            this.TsLog_night.Text = "Night Theme";
            this.TsLog_night.Click += new EventHandler(this.TsLog_Night_Click);
            // 
            // TsLog_Colored
            // 
            this.TsLog_Colored.Checked = true;
            this.TsLog_Colored.CheckState = CheckState.Checked;
            this.TsLog_Colored.Name = "TsLog_Colored";
            this.TsLog_Colored.Size = new Size(143, 22);
            this.TsLog_Colored.Text = "Colors";
            this.TsLog_Colored.Click += new EventHandler(this.TsLog_Colored_Click);
            // 
            // TsLog_dateStamp
            // 
            this.TsLog_dateStamp.Checked = true;
            this.TsLog_dateStamp.CheckState = CheckState.Checked;
            this.TsLog_dateStamp.Name = "TsLog_dateStamp";
            this.TsLog_dateStamp.Size = new Size(143, 22);
            this.TsLog_dateStamp.Text = "Date Stamp";
            this.TsLog_dateStamp.Click += new EventHandler(this.TsLog_DateStamp_Click);
            // 
            // TsLog_autoScroll
            // 
            this.TsLog_autoScroll.Checked = true;
            this.TsLog_autoScroll.CheckState = CheckState.Checked;
            this.TsLog_autoScroll.Name = "TsLog_autoScroll";
            this.TsLog_autoScroll.Size = new Size(143, 22);
            this.TsLog_autoScroll.Text = "Auto Scroll";
            this.TsLog_autoScroll.Click += new EventHandler(this.TsLog_AutoScroll_Click);
            // 
            // TsLog_separator1
            // 
            this.TsLog_separator1.Name = "TsLog_separator1";
            this.TsLog_separator1.Size = new Size(140, 6);
            // 
            // tsPlayer_copySelected
            // 
            this.TsLog_copySelected.Name = "TsLog_copySelected";
            this.TsLog_copySelected.Size = new Size(143, 22);
            this.TsLog_copySelected.Text = "Copy Selected";
            this.TsLog_copySelected.Click += new EventHandler(this.TsLog_CopySelected_Click);
            // 
            // TsLog_copyAll
            // 
            this.TsLog_copyAll.Name = "TsLog_copyAll";
            this.TsLog_copyAll.Size = new Size(143, 22);
            this.TsLog_copyAll.Text = "Copy All";
            this.TsLog_copyAll.Click += new EventHandler(this.TsLog_CopyAll_Click);
            // 
            // TsLog_separator2
            // 
            this.TsLog_separator2.Name = "TsLog_separator2";
            this.TsLog_separator2.Size = new Size(140, 6);
            // 
            // TsLog_clear
            // 
            this.TsLog_clear.Name = "TsLog_clear";
            this.TsLog_clear.Size = new Size(143, 22);
            this.TsLog_clear.Text = "Clear";
            this.TsLog_clear.Click += new EventHandler(this.TsLog_Clear_Click);
            // 
            // Main_txtInput
            // 
            this.Main_txtInput.BackColor = SystemColors.Window;
            this.Main_txtInput.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_txtInput.Location = new Point(8, 454);
            this.Main_txtInput.Name = "Main_txtInput";
            this.Main_txtInput.Size = new Size(750, 21);
            this.Main_txtInput.TabIndex = 27;
            this.GUIToolTip.SetToolTip(this.Main_txtInput, "To send chat to players, just type the message in.\nTo enter an order, put a / be" +
                        "fore it. (e.g. /help orders)");
            this.Main_txtInput.KeyDown += new KeyEventHandler(this.Main_TxtInput_KeyDown);
            // 
            // Main_txtUrl
            // 
            this.Main_txtUrl.Cursor = Cursors.Default;
            this.Main_txtUrl.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_txtUrl.Location = new Point(8, 7);
            this.Main_txtUrl.Name = "Main_txtUrl";
            this.Main_txtUrl.ReadOnly = true;
            this.Main_txtUrl.Size = new Size(498, 21);
            this.Main_txtUrl.TabIndex = 25;
            this.Main_txtUrl.Text = "Starting server..";
            this.Main_txtUrl.DoubleClick += new EventHandler(this.Main_TxtUrl_DoubleClick);
            // 
            // Main_colPlName
            // 
            this.Main_colPlName.HeaderText = "Name";
            this.Main_colPlName.Name = "Main_colPlName";
            this.Main_colPlName.ReadOnly = true;
            // 
            // Main_colPlMap
            // 
            this.Main_colPlMap.HeaderText = "Map";
            this.Main_colPlMap.Name = "Main_colPlMap";
            this.Main_colPlMap.ReadOnly = true;
            // 
            // Main_colPlRank
            // 
            this.Main_colPlRank.HeaderText = "Rank";
            this.Main_colPlRank.Name = "Main_colPlRank";
            this.Main_colPlRank.ReadOnly = true;
            // 
            // Main_Players
            // 
            this.Main_Players.AllowUserToAddRows = false;
            this.Main_Players.AllowUserToDeleteRows = false;
            this.Main_Players.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.Main_Players.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.Main_Players.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Main_Players.Columns.AddRange(new DataGridViewColumn[] {
        	        	        	this.Main_colPlName,
        	        	        	this.Main_colPlMap,
        	        	        	this.Main_colPlRank});
            this.Main_Players.ContextMenuStrip = this.TsPlayer;
            this.Main_Players.Location = new Point(512, 7);
            this.Main_Players.MultiSelect = false;
            this.Main_Players.Name = "Main_Players";
            this.Main_Players.ReadOnly = true;
            this.Main_Players.RowHeadersVisible = false;
            this.Main_Players.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Main_Players.Size = new Size(246, 250);
            this.Main_Players.TabIndex = 37;
            this.Main_Players.RowPrePaint += new DataGridViewRowPrePaintEventHandler(this.Main_players_RowPrePaint);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.Tp_Main);
            this.Tabs.Controls.Add(this.Logs_tp);
            this.Tabs.Controls.Add(this.Tp_Maps);
            this.Tabs.Controls.Add(this.Tp_Players);
            this.Tabs.Cursor = Cursors.Default;
            this.Tabs.Font = new Font("Calibri", 8.25F);
            this.Tabs.Location = new Point(1, 11);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new Size(775, 514);
            this.Tabs.TabIndex = 2;
            this.Tabs.Click += new EventHandler(this.Tabs_Click);
            // 
            // Tp_Maps
            // 
            this.Tp_Maps.BackColor = SystemColors.Control;
            this.Tp_Maps.Controls.Add(this.Map_gbProps);
            this.Tp_Maps.Controls.Add(this.Map_gbLoaded);
            this.Tp_Maps.Controls.Add(this.Map_gbUnloaded);
            this.Tp_Maps.Controls.Add(this.Map_gbNew);
            this.Tp_Maps.Location = new Point(4, 22);
            this.Tp_Maps.Name = "Tp_Maps";
            this.Tp_Maps.Size = new Size(767, 488);
            this.Tp_Maps.TabIndex = 9;
            this.Tp_Maps.Text = "Maps";
            // 
            // Map_gbProps
            // 
            this.Map_gbProps.Controls.Add(this.Map_pgProps);
            this.Map_gbProps.Location = new Point(415, 3);
            this.Map_gbProps.Name = "Map_gbProps";
            this.Map_gbProps.Size = new Size(343, 349);
            this.Map_gbProps.TabIndex = 5;
            this.Map_gbProps.TabStop = false;
            this.Map_gbProps.Text = "Properties for (none selected)";
            // 
            // Map_pgProps
            // 
            this.Map_pgProps.Location = new Point(7, 20);
            this.Map_pgProps.Name = "Map_pgProps";
            this.Map_pgProps.Size = new Size(330, 323);
            this.Map_pgProps.TabIndex = 0;
            this.Map_pgProps.ToolbarVisible = false;
            // 
            // Map_gbLoaded
            // 
            this.Map_gbLoaded.Controls.Add(this.Map_lbLoaded);
            this.Map_gbLoaded.Location = new Point(7, 3);
            this.Map_gbLoaded.Name = "Map_gbLoaded";
            this.Map_gbLoaded.Size = new Size(390, 221);
            this.Map_gbLoaded.TabIndex = 4;
            this.Map_gbLoaded.TabStop = false;
            this.Map_gbLoaded.Text = "Loaded levels";
            // 
            // Map_lbLoaded
            // 
            this.Map_lbLoaded.BackColor = SystemColors.Window;
            this.Map_lbLoaded.ForeColor = SystemColors.WindowText;
            this.Map_lbLoaded.FormattingEnabled = true;
            this.Map_lbLoaded.Location = new Point(5, 15);
            this.Map_lbLoaded.MultiColumn = true;
            this.Map_lbLoaded.Name = "Map_lbLoaded";
            this.Map_lbLoaded.Size = new Size(379, 199);
            this.Map_lbLoaded.TabIndex = 0;
            this.Map_lbLoaded.SelectedIndexChanged += new EventHandler(this.Map_UpdateSelected);
            // 
            // Map_gbUnloaded
            // 
            this.Map_gbUnloaded.Controls.Add(this.Map_btnLoad);
            this.Map_gbUnloaded.Controls.Add(this.Map_lbUnloaded);
            this.Map_gbUnloaded.Location = new Point(7, 227);
            this.Map_gbUnloaded.Name = "Map_gbUnloaded";
            this.Map_gbUnloaded.Size = new Size(390, 258);
            this.Map_gbUnloaded.TabIndex = 3;
            this.Map_gbUnloaded.TabStop = false;
            this.Map_gbUnloaded.Text = "Unloaded levels";
            // 
            // Map_btnLoad
            // 
            this.Map_btnLoad.Location = new Point(150, 230);
            this.Map_btnLoad.Name = "Map_btnLoad";
            this.Map_btnLoad.Size = new Size(75, 23);
            this.Map_btnLoad.TabIndex = 1;
            this.Map_btnLoad.Text = "Load map";
            this.Map_btnLoad.UseVisualStyleBackColor = true;
            this.Map_btnLoad.Click += new EventHandler(this.Map_BtnLoad_Click);
            // 
            // Map_lbUnloaded
            // 
            this.Map_lbUnloaded.BackColor = SystemColors.Window;
            this.Map_lbUnloaded.ForeColor = SystemColors.WindowText;
            this.Map_lbUnloaded.FormattingEnabled = true;
            this.Map_lbUnloaded.Location = new Point(5, 15);
            this.Map_lbUnloaded.MultiColumn = true;
            this.Map_lbUnloaded.Name = "Map_lbUnloaded";
            this.Map_lbUnloaded.Size = new Size(379, 212);
            this.Map_lbUnloaded.TabIndex = 0;
            // 
            // Map_gbNew
            // 
            this.Map_gbNew.Controls.Add(this.Map_btnGen);
            this.Map_gbNew.Controls.Add(this.Map_lblType);
            this.Map_gbNew.Controls.Add(this.Map_lblSeed);
            this.Map_gbNew.Controls.Add(this.Map_lblZ);
            this.Map_gbNew.Controls.Add(this.Map_lblX);
            this.Map_gbNew.Controls.Add(this.Map_lblY);
            this.Map_gbNew.Controls.Add(this.Map_txtSeed);
            this.Map_gbNew.Controls.Add(this.Map_cmbType);
            this.Map_gbNew.Controls.Add(this.Map_cmbZ);
            this.Map_gbNew.Controls.Add(this.Map_cmbY);
            this.Map_gbNew.Controls.Add(this.Map_cmbX);
            this.Map_gbNew.Controls.Add(this.Map_lblName);
            this.Map_gbNew.Controls.Add(this.Map_txtName);
            this.Map_gbNew.Location = new Point(415, 358);
            this.Map_gbNew.Name = "Map_gbNew";
            this.Map_gbNew.Size = new Size(343, 127);
            this.Map_gbNew.TabIndex = 0;
            this.Map_gbNew.TabStop = false;
            this.Map_gbNew.Text = "Create new map";
            // 
            // Map_btnGen
            // 
            this.Map_btnGen.Location = new Point(150, 99);
            this.Map_btnGen.Name = "Map_btnGen";
            this.Map_btnGen.Size = new Size(75, 23);
            this.Map_btnGen.TabIndex = 17;
            this.Map_btnGen.Text = "Generate";
            this.Map_btnGen.UseVisualStyleBackColor = true;
            this.Map_btnGen.Click += new EventHandler(this.Map_BtnGen_Click);
            // 
            // Map_lblType
            // 
            this.Map_lblType.AutoSize = true;
            this.Map_lblType.Location = new Point(13, 78);
            this.Map_lblType.Name = "Map_lblType";
            this.Map_lblType.Size = new Size(32, 13);
            this.Map_lblType.TabIndex = 16;
            this.Map_lblType.Text = "Type:";
            // 
            // Map_lblSeed
            // 
            this.Map_lblSeed.AutoSize = true;
            this.Map_lblSeed.Location = new Point(192, 78);
            this.Map_lblSeed.Name = "Map_lblSeed";
            this.Map_lblSeed.Size = new Size(33, 13);
            this.Map_lblSeed.TabIndex = 15;
            this.Map_lblSeed.Text = "Seed:";
            // 
            // Map_lblZ
            // 
            this.Map_lblZ.AutoSize = true;
            this.Map_lblZ.Location = new Point(231, 51);
            this.Map_lblZ.Name = "Map_lblZ";
            this.Map_lblZ.Size = new Size(42, 13);
            this.Map_lblZ.TabIndex = 14;
            this.Map_lblZ.Text = "Length:";
            // 
            // Map_lblX
            // 
            this.Map_lblX.AutoSize = true;
            this.Map_lblX.Location = new Point(7, 51);
            this.Map_lblX.Name = "Map_lblX";
            this.Map_lblX.Size = new Size(39, 13);
            this.Map_lblX.TabIndex = 13;
            this.Map_lblX.Text = "Width:";
            // 
            // Map_lblY
            // 
            this.Map_lblY.AutoSize = true;
            this.Map_lblY.Location = new Point(118, 51);
            this.Map_lblY.Name = "Map_lblY";
            this.Map_lblY.Size = new Size(41, 13);
            this.Map_lblY.TabIndex = 12;
            this.Map_lblY.Text = "Height:";
            // 
            // Map_txtSeed
            // 
            this.Map_txtSeed.BackColor = SystemColors.Window;
            this.Map_txtSeed.Location = new Point(231, 75);
            this.Map_txtSeed.Name = "Map_txtSeed";
            this.Map_txtSeed.Size = new Size(107, 21);
            this.Map_txtSeed.TabIndex = 11;
            // 
            // Map_cmbType
            // 
            this.Map_cmbType.BackColor = SystemColors.Window;
            this.Map_cmbType.FormattingEnabled = true;
            this.Map_cmbType.Location = new Point(51, 75);
            this.Map_cmbType.Name = "Map_cmbType";
            this.Map_cmbType.Size = new Size(121, 21);
            this.Map_cmbType.TabIndex = 10;
            // 
            // Map_cmbZ
            // 
            this.Map_cmbZ.BackColor = SystemColors.Window;
            this.Map_cmbZ.FormattingEnabled = true;
            this.Map_cmbZ.Items.AddRange(new object[] {
                                    "16",
                                    "32",
                                    "64",
                                    "128",
                                    "256",
                                    "512",
                                    "1024"});
            this.Map_cmbZ.Location = new Point(279, 48);
            this.Map_cmbZ.Name = "Map_cmbZ";
            this.Map_cmbZ.Size = new Size(60, 21);
            this.Map_cmbZ.TabIndex = 9;
            // 
            // Map_cmbY
            // 
            this.Map_cmbY.BackColor = SystemColors.Window;
            this.Map_cmbY.FormattingEnabled = true;
            this.Map_cmbY.Items.AddRange(new object[] {
                                    "16",
                                    "32",
                                    "64",
                                    "128",
                                    "256",
                                    "512",
                                    "1024"});
            this.Map_cmbY.Location = new Point(165, 48);
            this.Map_cmbY.Name = "Map_cmbY";
            this.Map_cmbY.Size = new Size(60, 21);
            this.Map_cmbY.TabIndex = 8;
            // 
            // Map_cmbX
            // 
            this.Map_cmbX.BackColor = SystemColors.Window;
            this.Map_cmbX.FormattingEnabled = true;
            this.Map_cmbX.Items.AddRange(new object[] {
                                    "16",
                                    "32",
                                    "64",
                                    "128",
                                    "256",
                                    "512",
                                    "1024"});
            this.Map_cmbX.Location = new Point(52, 48);
            this.Map_cmbX.Name = "Map_cmbX";
            this.Map_cmbX.Size = new Size(60, 21);
            this.Map_cmbX.TabIndex = 7;
            // 
            // Map_lblName
            // 
            this.Map_lblName.AutoSize = true;
            this.Map_lblName.Location = new Point(7, 24);
            this.Map_lblName.Name = "Map_lblName";
            this.Map_lblName.Size = new Size(38, 13);
            this.Map_lblName.TabIndex = 6;
            this.Map_lblName.Text = "Name:";
            // 
            // Map_txtName
            // 
            this.Map_txtName.BackColor = SystemColors.Window;
            this.Map_txtName.Location = new Point(51, 21);
            this.Map_txtName.Name = "Map_txtName";
            this.Map_txtName.Size = new Size(287, 21);
            this.Map_txtName.TabIndex = 0;
            // 
            // Tp_Players
            // 
            this.Tp_Players.Controls.Add(this.Pl_lblOnline);
            this.Tp_Players.Controls.Add(this.Pl_gbProps);
            this.Tp_Players.Controls.Add(this.Pl_gbOther);
            this.Tp_Players.Controls.Add(this.Pl_gbActions);
            this.Tp_Players.Controls.Add(this.Pl_statusBox);
            this.Tp_Players.Controls.Add(this.Pl_listBox);
            this.Tp_Players.Location = new Point(4, 22);
            this.Tp_Players.Name = "Tp_Players";
            this.Tp_Players.Padding = new Padding(3);
            this.Tp_Players.Size = new Size(767, 488);
            this.Tp_Players.TabIndex = 7;
            this.Tp_Players.Text = "Players";
            // 
            // Pl_lblOnline
            // 
            this.Pl_lblOnline.AutoSize = true;
            this.Pl_lblOnline.Location = new Point(8, 9);
            this.Pl_lblOnline.Name = "Pl_lblOnline";
            this.Pl_lblOnline.Size = new Size(78, 13);
            this.Pl_lblOnline.TabIndex = 68;
            this.Pl_lblOnline.Text = "Online players:";
            // 
            // Pl_gbProps
            // 
            this.Pl_gbProps.Controls.Add(this.Pl_pgProps);
            this.Pl_gbProps.Location = new Point(147, 9);
            this.Pl_gbProps.Name = "Pl_gbProps";
            this.Pl_gbProps.Size = new Size(363, 380);
            this.Pl_gbProps.TabIndex = 67;
            this.Pl_gbProps.TabStop = false;
            this.Pl_gbProps.Text = "Properties for (none selected)";
            // 
            // Pl_pgProps
            // 
            this.Pl_pgProps.HelpVisible = false;
            this.Pl_pgProps.Location = new Point(6, 18);
            this.Pl_pgProps.Name = "Pl_pgProps";
            this.Pl_pgProps.Size = new Size(351, 356);
            this.Pl_pgProps.TabIndex = 64;
            this.Pl_pgProps.ToolbarVisible = false;
            // 
            // Pl_gbOther
            // 
            this.Pl_gbOther.Controls.Add(this.Pl_txtSendOrder);
            this.Pl_gbOther.Controls.Add(this.Pl_btnSendOrder);
            this.Pl_gbOther.Controls.Add(this.Pl_txtMessage);
            this.Pl_gbOther.Controls.Add(this.Pl_btnMessage);
            this.Pl_gbOther.Location = new Point(147, 395);
            this.Pl_gbOther.Name = "Pl_gbOther";
            this.Pl_gbOther.Size = new Size(614, 78);
            this.Pl_gbOther.TabIndex = 66;
            this.Pl_gbOther.TabStop = false;
            this.Pl_gbOther.Text = "Other";
            // 
            // Pl_txtSendOrder
            // 
            this.Pl_txtSendOrder.BackColor = SystemColors.Window;
            this.Pl_txtSendOrder.Location = new Point(115, 50);
            this.Pl_txtSendOrder.Name = "Pl_txtSendOrder";
            this.Pl_txtSendOrder.Size = new Size(485, 21);
            this.Pl_txtSendOrder.TabIndex = 38;
            this.Pl_txtSendOrder.KeyDown += new KeyEventHandler(this.Pl_txtSendOrder_KeyDown);
            // 
            // pl_btnImpersonate
            // 
            this.Pl_btnSendOrder.Location = new Point(6, 48);
            this.Pl_btnSendOrder.Name = "pl_btnImpersonate";
            this.Pl_btnSendOrder.Size = new Size(98, 23);
            this.Pl_btnSendOrder.TabIndex = 37;
            this.Pl_btnSendOrder.Text = "Do order:";
            this.Pl_btnSendOrder.UseVisualStyleBackColor = true;
            this.Pl_btnSendOrder.Click += new EventHandler(this.Pl_BtnSendOrder_Click);
            // 
            // Pl_txtMessage
            // 
            this.Pl_txtMessage.BackColor = SystemColors.Window;
            this.Pl_txtMessage.Location = new Point(115, 18);
            this.Pl_txtMessage.Name = "Pl_txtMessage";
            this.Pl_txtMessage.Size = new Size(485, 21);
            this.Pl_txtMessage.TabIndex = 8;
            this.Pl_txtMessage.KeyDown += new KeyEventHandler(this.Pl_txtMessage_KeyDown);
            // 
            // Pl_btnMessage
            // 
            this.Pl_btnMessage.Location = new Point(6, 16);
            this.Pl_btnMessage.Name = "Pl_btnMessage";
            this.Pl_btnMessage.Size = new Size(98, 23);
            this.Pl_btnMessage.TabIndex = 9;
            this.Pl_btnMessage.Text = "Send message:";
            this.Pl_btnMessage.UseVisualStyleBackColor = true;
            this.Pl_btnMessage.Click += new EventHandler(this.Pl_BtnMessage_Click);
            // 
            // Pl_gbActions
            // 
            this.Pl_gbActions.Controls.Add(this.Pl_btnKill);
            this.Pl_gbActions.Controls.Add(this.Pl_numUndo);
            this.Pl_gbActions.Controls.Add(this.Pl_btnWarn);
            this.Pl_gbActions.Controls.Add(this.Pl_btnRules);
            this.Pl_gbActions.Controls.Add(this.Pl_btnKick);
            this.Pl_gbActions.Controls.Add(this.Pl_btnBanIP);
            this.Pl_gbActions.Controls.Add(this.Pl_btnUndo);
            this.Pl_gbActions.Controls.Add(this.Pl_btnMute);
            this.Pl_gbActions.Controls.Add(this.Pl_btnBan);
            this.Pl_gbActions.Controls.Add(this.Pl_btnFreeze);
            this.Pl_gbActions.Location = new Point(529, 9);
            this.Pl_gbActions.Name = "Pl_gbActions";
            this.Pl_gbActions.Size = new Size(228, 186);
            this.Pl_gbActions.TabIndex = 65;
            this.Pl_gbActions.TabStop = false;
            this.Pl_gbActions.Text = "Actions";
            // 
            // Pl_btnKill
            // 
            this.Pl_btnKill.Location = new Point(8, 105);
            this.Pl_btnKill.Name = "Pl_btnKill";
            this.Pl_btnKill.Size = new Size(98, 23);
            this.Pl_btnKill.TabIndex = 43;
            this.Pl_btnKill.Text = "Kill";
            this.Pl_btnKill.UseVisualStyleBackColor = true;
            this.Pl_btnKill.Click += new EventHandler(this.Pl_BtnKill_Click);
            // 
            // Pl_numUndo
            // 
            this.Pl_numUndo.BackColor = SystemColors.Window;
            this.Pl_numUndo.Location = new Point(122, 149);
            this.Pl_numUndo.Name = "Pl_numUndo";
            this.Pl_numUndo.Size = new Size(98, 21);
            this.Pl_numUndo.TabIndex = 42;
            this.Pl_numUndo.Seconds = ((long)(1800));
            this.Pl_numUndo.Text = "30m";
            this.Pl_numUndo.Value = TimeSpan.Parse("00:30:00");
            this.Pl_numUndo.KeyDown += new KeyEventHandler(this.Pl_numUndo_KeyDown);
            // 
            // Pl_btnWarn
            // 
            this.Pl_btnWarn.Location = new Point(8, 18);
            this.Pl_btnWarn.Name = "Pl_btnWarn";
            this.Pl_btnWarn.Size = new Size(98, 23);
            this.Pl_btnWarn.TabIndex = 10;
            this.Pl_btnWarn.Text = "Warn";
            this.Pl_btnWarn.UseVisualStyleBackColor = true;
            this.Pl_btnWarn.Click += new EventHandler(this.Pl_BtnWarn_Click);
            // 
            // Pl_btnRules
            // 
            this.Pl_btnRules.Location = new Point(122, 105);
            this.Pl_btnRules.Name = "Pl_btnRules";
            this.Pl_btnRules.Size = new Size(98, 23);
            this.Pl_btnRules.TabIndex = 39;
            this.Pl_btnRules.Text = "Send Rules";
            this.Pl_btnRules.UseVisualStyleBackColor = true;
            this.Pl_btnRules.Click += new EventHandler(this.Pl_BtnRules_Click);
            // 
            // Pl_btnKick
            // 
            this.Pl_btnKick.Location = new Point(122, 18);
            this.Pl_btnKick.Name = "Pl_btnKick";
            this.Pl_btnKick.Size = new Size(98, 23);
            this.Pl_btnKick.TabIndex = 4;
            this.Pl_btnKick.Text = "Kick";
            this.Pl_btnKick.UseVisualStyleBackColor = true;
            this.Pl_btnKick.Click += new EventHandler(this.Pl_BtnKick_Click);
            // 
            // Pl_btnBanIP
            // 
            this.Pl_btnBanIP.Location = new Point(122, 47);
            this.Pl_btnBanIP.Name = "Pl_btnBanIP";
            this.Pl_btnBanIP.Size = new Size(98, 23);
            this.Pl_btnBanIP.TabIndex = 6;
            this.Pl_btnBanIP.Text = "IP Ban";
            this.Pl_btnBanIP.UseVisualStyleBackColor = true;
            this.Pl_btnBanIP.Click += new EventHandler(this.Pl_BtnIPBan_Click);
            // 
            // Pl_btnUndo
            // 
            this.Pl_btnUndo.Location = new Point(8, 148);
            this.Pl_btnUndo.Name = "Pl_btnUndo";
            this.Pl_btnUndo.Size = new Size(98, 23);
            this.Pl_btnUndo.TabIndex = 41;
            this.Pl_btnUndo.Text = "Undo:";
            this.Pl_btnUndo.UseVisualStyleBackColor = true;
            this.Pl_btnUndo.Click += new EventHandler(this.Pl_BtnUndo_Click);
            // 
            // Pl_btnMute
            // 
            this.Pl_btnMute.Location = new Point(8, 76);
            this.Pl_btnMute.Name = "Pl_btnMute";
            this.Pl_btnMute.Size = new Size(98, 23);
            this.Pl_btnMute.TabIndex = 40;
            this.Pl_btnMute.Text = "Mute";
            this.Pl_btnMute.UseVisualStyleBackColor = true;
            this.Pl_btnMute.Click += new EventHandler(this.Pl_BtnMute_Click);
            // 
            // Pl_btnBan
            // 
            this.Pl_btnBan.Location = new Point(8, 47);
            this.Pl_btnBan.Name = "Pl_btnBan";
            this.Pl_btnBan.Size = new Size(98, 23);
            this.Pl_btnBan.TabIndex = 5;
            this.Pl_btnBan.Text = "Ban";
            this.Pl_btnBan.UseVisualStyleBackColor = true;
            this.Pl_btnBan.Click += new EventHandler(this.Pl_BtnBan_Click);
            // 
            // Pl_btnFreeze
            // 
            this.Pl_btnFreeze.Location = new Point(122, 76);
            this.Pl_btnFreeze.Name = "Pl_btnFreeze";
            this.Pl_btnFreeze.Size = new Size(98, 23);
            this.Pl_btnFreeze.TabIndex = 36;
            this.Pl_btnFreeze.Text = "Freeze";
            this.Pl_btnFreeze.UseVisualStyleBackColor = true;
            this.Pl_btnFreeze.Click += new EventHandler(this.Pl_BtnFreeze_Click);
            // 
            // Pl_statusBox
            // 
            this.Pl_statusBox.BackColor = SystemColors.Window;
            this.Pl_statusBox.Cursor = Cursors.Default;
            this.Pl_statusBox.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Pl_statusBox.Location = new Point(529, 201);
            this.Pl_statusBox.Multiline = true;
            this.Pl_statusBox.Name = "Pl_statusBox";
            this.Pl_statusBox.ReadOnly = true;
            this.Pl_statusBox.ScrollBars = ScrollBars.Both;
            this.Pl_statusBox.Size = new Size(232, 188);
            this.Pl_statusBox.TabIndex = 63;
            // 
            // Pl_listBox
            // 
            this.Pl_listBox.BackColor = SystemColors.Window;
            this.Pl_listBox.ForeColor = SystemColors.WindowText;
            this.Pl_listBox.FormattingEnabled = true;
            this.Pl_listBox.Location = new Point(8, 27);
            this.Pl_listBox.Name = "Pl_listBox";
            this.Pl_listBox.Size = new Size(123, 446);
            this.Pl_listBox.TabIndex = 62;
            this.Pl_listBox.Click += new EventHandler(this.Pl_listBox_Click);
            // 
            // TokenToolTip
            // 
            this.GUIToolTip.AutoPopDelay = 8000;
            this.GUIToolTip.InitialDelay = 500;
            this.GUIToolTip.IsBalloon = true;
            this.GUIToolTip.ReshowDelay = 100;
            this.GUIToolTip.ToolTipIcon = ToolTipIcon.Info;
            this.GUIToolTip.ToolTipTitle = "Information";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(775, 523);
            this.Controls.Add(this.Main_btnClose);
            this.Controls.Add(this.Main_btnProps);
            this.Controls.Add(this.Main_btnRestart);
            this.Controls.Add(this.Tabs);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Window";
            this.FormClosing += new FormClosingEventHandler(this.Window_FormClosing);
            this.Load += new EventHandler(this.Window_Load);
            this.Resize += new EventHandler(this.Window_Resize);
            this.TsMap.ResumeLayout(false);
            this.TsPlayer.ResumeLayout(false);
            this.Icon_context.ResumeLayout(false);
            this.Logs_tp.ResumeLayout(false);
            this.Logs_tab.ResumeLayout(false);
            this.Logs_tabErr.ResumeLayout(false);
            this.Logs_tabErr.PerformLayout();
            this.Logs_tabGen.ResumeLayout(false);
            this.Logs_tabGen.PerformLayout();
            this.TabLog_Sys.ResumeLayout(false);
            this.TabLog_Sys.PerformLayout();
            this.Tp_Main.ResumeLayout(false);
            this.Tp_Main.PerformLayout();
            ((ISupportInitialize)(this.Main_Maps)).EndInit();
            this.TsLog_Menu.ResumeLayout(false);
            ((ISupportInitialize)(this.Main_Players)).EndInit();
            this.Tabs.ResumeLayout(false);
            this.Tp_Maps.ResumeLayout(false);
            this.Map_gbProps.ResumeLayout(false);
            this.Map_gbLoaded.ResumeLayout(false);
            this.Map_gbUnloaded.ResumeLayout(false);
            this.Map_gbNew.ResumeLayout(false);
            this.Map_gbNew.PerformLayout();
            this.Tp_Players.ResumeLayout(false);
            this.Tp_Players.PerformLayout();
            this.Pl_gbProps.ResumeLayout(false);
            this.Pl_gbOther.ResumeLayout(false);
            this.Pl_gbOther.PerformLayout();
            this.Pl_gbActions.ResumeLayout(false);
            this.Pl_gbActions.PerformLayout();
            this.ResumeLayout(false);
        }
        public DataGridViewTextBoxColumn Main_colPlName;
        public DataGridViewTextBoxColumn Main_colPlMap;
        public DataGridViewTextBoxColumn Main_colPlRank;
        public DataGridViewTextBoxColumn Main_colLvlPhysics;
        public DataGridViewTextBoxColumn Main_colLvlPlayers;
        public DataGridViewTextBoxColumn Main_colLvlName;
        public Label Pl_lblOnline;
        public GroupBox Pl_gbProps;
        public GroupBox Pl_gbActions;
        public GroupBox Pl_gbOther;
        public PropertyGrid Pl_pgProps;
        public TextBox Map_txtName;
        public Label Map_lblName;
        public ComboBox Map_cmbX;
        public ComboBox Map_cmbY;
        public ComboBox Map_cmbZ;
        public ComboBox Map_cmbType;
        public TextBox Map_txtSeed;
        public Label Map_lblY;
        public Label Map_lblX;
        public Label Map_lblZ;
        public Label Map_lblSeed;
        public Label Map_lblType;
        public Button Map_btnGen;
        public GroupBox Map_gbNew;
        public ListBox Map_lbUnloaded;
        public Button Map_btnLoad;
        public GroupBox Map_gbUnloaded;
        public ListBox Map_lbLoaded;
        public GroupBox Map_gbLoaded;
        public PropertyGrid Map_pgProps;
        public GroupBox Map_gbProps;
        public TabPage Tp_Main;
        public TabPage TabLog_Sys;
        public TabPage Logs_tabErr;
        public TabPage Logs_tabGen;
        public TabControl Logs_tab;
        #endregion
        public Button Main_btnClose;
        public ContextMenuStrip Icon_context;
        public ToolStripMenuItem Icon_hideWindow;
        public ToolStripSeparator Icon_separator;
        public ToolStripMenuItem Icon_openTerminal;
        public ToolStripMenuItem Icon_shutdown;
        public ContextMenuStrip TsPlayer;
        public ToolStripMenuItem TsPlayer_whois;
        public ToolStripMenuItem TsPlayer_kick;
        public ToolStripMenuItem TsPlayer_ban;
        public ToolStripMenuItem TsPlayer_voice;
        public ContextMenuStrip TsMap;
        public ToolStripMenuItem TsPlayer_clones;
        public Button Main_btnRestart;
        public ToolStripMenuItem Icon_restart;
        public TabPage Logs_tp;
        public Label Logs_lblGeneral;
        public TextBox Logs_txtError;
        public TextBox Logs_txtSystem;
        public TabPage Tp_Maps;
        public DataGridView Main_Maps;
        public TextBox Main_txtInput;
        public TextBox Main_txtUrl;
        public DataGridView Main_Players;
        public TabControl Tabs;
        public ToolStripMenuItem TsPlayer_promote;
        public ToolStripMenuItem TsPlayer_demote;
        public TabPage Tp_Players;
        public RichTextBox Logs_txtGeneral;
        public DateTimePicker Logs_dateGeneral;
        public Button Pl_btnBanIP;
        public Button Pl_btnBan;
        public Button Pl_btnKick;
        public Button Pl_btnMessage;
        public TextBox Pl_txtMessage;
        public Button Pl_btnWarn;
        public Button Pl_btnFreeze;
        public TextBox Pl_txtSendOrder;
        public Button Pl_btnSendOrder;
        public Button Pl_btnKill;
        public Flames.Gui.TimespanUpDown Pl_numUndo;
        public Button Pl_btnUndo;
        public Button Pl_btnMute;
        public Button Pl_btnRules;
        public TextBox Pl_statusBox;
        public ListBox Pl_listBox;
        public Button Main_btnSaveAll;
        public Button Main_btnUnloadEmpty;
        public Button Main_btnKillPhysics;
        public ToolStripMenuItem TsMap_info;
        public ToolStripMenuItem TsMap_actionsMenu;
        public ToolStripMenuItem TsMap_Save;
        public ToolStripMenuItem TsMap_Unload;
        public ToolStripMenuItem TsMap_moveAll;
        public ToolStripMenuItem TsMap_Reload;
        public ToolStripMenuItem TsMap_physicsMenu;
        public ToolStripMenuItem TsMap_physics0;
        public ToolStripMenuItem TsMap_physics1;
        public ToolStripMenuItem TsMap_physics2;
        public ToolStripMenuItem TsMap_physics3;
        public ToolStripMenuItem TsMap_physics4;
        public ToolStripMenuItem TsMap_physics5;
        public ToolStripSeparator TsMap_separator;
        public Components.ColoredTextBox Main_txtLog;
        public ContextMenuStrip TsLog_Menu;
        public ToolStripMenuItem TsLog_night;
        public ToolStripMenuItem TsLog_Colored;
        public ToolStripSeparator TsLog_separator1;
        public ToolStripMenuItem TsLog_copySelected;
        public ToolStripMenuItem TsLog_copyAll;
        public ToolStripSeparator TsLog_separator2;
        public ToolStripMenuItem TsLog_clear;
        public ToolStripMenuItem TsLog_dateStamp;
        public ToolStripMenuItem TsLog_autoScroll;
        public Button Main_btnProps;
        public ToolTip GUIToolTip;
    }
}