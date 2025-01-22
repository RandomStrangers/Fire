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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Flames.Gui
{
    public partial class PropertyWindow
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
            this.PageChat = new TabPage();
            this.Chat_grpTab = new GroupBox();
            this.Chat_cbTabRank = new CheckBox();
            this.Chat_cbTabLevel = new CheckBox();
            this.Chat_cbTabBots = new CheckBox();
            this.Chat_grpMessages = new GroupBox();
            this.Chat_grpModeration = new GroupBox();
            this.Chat_lblShutdown = new Label();
            this.Chat_txtShutdown = new TextBox();
            this.Chat_chkCheap = new CheckBox();
            this.Chat_txtCheap = new TextBox();
            this.Chat_lblBan = new Label();
            this.Chat_txtBan = new TextBox();
            this.Chat_lblPromote = new Label();
            this.Chat_txtPromote = new TextBox();
            this.Chat_lblDemote = new Label();
            this.Chat_txtDemote = new TextBox();
            this.Chat_lblLogin = new Label();
            this.Chat_txtLogin = new TextBox();
            this.Chat_lblLogout = new Label();
            this.Chat_txtLogout = new TextBox();
            this.Chat_grpOther = new GroupBox();
            this.Chat_chkFilter = new CheckBox();
            this.Chat_lblConsole = new Label();
            this.Chat_txtConsole = new TextBox();
            this.Chat_grpColors = new GroupBox();
            this.Chat_lblWarn = new Label();
            this.Chat_btnWarn = new Button();
            this.Chat_lblDefault = new Label();
            this.Chat_btnDefault = new Button();
            this.Chat_lblIRC = new Label();
            this.Chat_btnIRC = new Button();
            this.Chat_lblSyntax = new Label();
            this.Chat_btnSyntax = new Button();
            this.Chat_lblDesc = new Label();
            this.Chat_btnDesc = new Button();
            this.Main_btnSave = new Button();
            this.Main_btnDiscard = new Button();
            this.Main_btnApply = new Button();
            this.GUIToolTip = new ToolTip(this.Components);
            this.ChkRpLimit = new Label();
            this.ChkDeath = new CheckBox();
            this.Hack_lbl = new CheckBox();
            this.Sec_cmbVerifyRank = new ComboBox();
            this.Sec_cbVerifyAdmins = new CheckBox();
            this.ChkGuestLimitNotify = new CheckBox();
            this.Rank_cbTPHigher = new CheckBox();
            this.Rank_cmbDefault = new ComboBox();
            this.Sec_cbWhitelist = new CheckBox();
            this.Sql_chkUseSQL = new CheckBox();
            this.Irc_chkEnabled = new CheckBox();
            this.Irc_txtServer = new TextBox();
            this.Irc_txtNick = new TextBox();
            this.Irc_txtChannel = new TextBox();
            this.Irc_txtOpChannel = new TextBox();
            this.Lvl_chkAutoload = new CheckBox();
            this.Lvl_chkWorld = new CheckBox();
            this.Adv_chkVerify = new CheckBox();
            this.Srv_txtName = new TextBox();
            this.Srv_txtMOTD = new TextBox();
            this.Srv_numPort = new NumericUpDown();
            this.Srv_chkPublic = new CheckBox();
            this.Rank_cbSilentAdmins = new CheckBox();
            this.Rank_txtPrefix = new TextBox();
            this.Rank_txtMOTD = new TextBox();
            this.Rank_numPerm = new NumericUpDown();
            this.Rank_txtName = new TextBox();
            this.Rank_btnColor = new Button();
            this.Rank_cmbOsMap = new ComboBox();
            this.Irc_chkPass = new CheckBox();
            this.Irc_txtPass = new TextBox();
            this.Rank_numMaps = new NumericUpDown();
            this.Rank_numDraw = new NumericUpDown();
            this.Rank_numGen = new NumericUpDown();
            this.Rank_numCopy = new NumericUpDown();
            this.Dis_grp = new GroupBox();
            this.Dis_lblToken = new Label();
            this.Dis_lblChannel = new Label();
            this.Dis_txtChannel = new TextBox();
            this.Dis_lblOpChannel = new Label();
            this.Dis_chkEnabled = new CheckBox();
            this.Dis_chkNicks = new CheckBox();
            this.Dis_txtToken = new TextBox();
            this.Dis_txtOpChannel = new TextBox();
            this.Dis_linkHelp = new LinkLabel();
            this.Adv_chkCPE = new CheckBox();
            this.Eco_cmbItemRank = new ComboBox();
            this.Rank_numUndo = new Flames.Gui.TimespanUpDown();
            this.ChkPhysRestart = new CheckBox();
            this.Ls_numMax = new NumericUpDown();
            this.Ls_numWater = new NumericUpDown();
            this.Ls_numFast = new NumericUpDown();
            this.Ls_numFloodUp = new NumericUpDown();
            this.Ls_numLayer = new NumericUpDown();
            this.Ls_numCount = new NumericUpDown();
            this.Ls_numHeight = new NumericUpDown();
            this.Ls_cbMain = new CheckBox();
            this.Ls_cbStart = new CheckBox();
            this.Rank_numAfk = new Flames.Gui.TimespanUpDown();
            this.Sec_cbLogNotes = new CheckBox();
            this.Sec_cbChatAuto = new CheckBox();
            this.PageBlocks = new TabPage();
            this.Blk_grpPhysics = new GroupBox();
            this.Blk_cbWater = new CheckBox();
            this.Blk_cbLava = new CheckBox();
            this.Blk_cbRails = new CheckBox();
            this.Blk_cbTdoor = new CheckBox();
            this.Blk_cbDoor = new CheckBox();
            this.Blk_grpBehaviour = new GroupBox();
            this.Blk_txtDeath = new TextBox();
            this.Blk_cbDeath = new CheckBox();
            this.Blk_cbPortal = new CheckBox();
            this.Blk_cbMsgBlock = new CheckBox();
            this.Blk_grpPermissions = new GroupBox();
            this.Blk_cmbAlw3 = new ComboBox();
            this.Blk_cmbAlw2 = new ComboBox();
            this.Blk_cmbDis3 = new ComboBox();
            this.Blk_cmbDis2 = new ComboBox();
            this.Blk_cmbAlw1 = new ComboBox();
            this.Blk_cmbDis1 = new ComboBox();
            this.Blk_cmbMin = new ComboBox();
            this.Blk_lblMin = new Label();
            this.Blk_lblAllow = new Label();
            this.Blk_lblDisallow = new Label();
            this.Blk_btnHelp = new Button();
            this.Blk_list = new ListBox();
            this.PageRanks = new TabPage();
            this.Rank_grpLimits = new GroupBox();
            this.Rank_lblCopy = new Label();
            this.Rank_lblGen = new Label();
            this.Rank_lblMaps = new Label();
            this.Rank_lblDraw = new Label();
            this.Rank_lblUndo = new Label();
            this.Rank_grpGeneral = new GroupBox();
            this.Rank_lblOsMap = new Label();
            this.Rank_cbEmpty = new CheckBox();
            this.Rank_lblDefault = new Label();
            this.Rank_grpMisc = new GroupBox();
            this.Rank_cbAfk = new CheckBox();
            this.Rank_lblPrefix = new Label();
            this.Rank_lblPerm = new Label();
            this.Rank_lblMOTD = new Label();
            this.Rank_lblName = new Label();
            this.Rank_lblColor = new Label();
            this.Rank_btnDel = new Button();
            this.Rank_btnAdd = new Button();
            this.Rank_list = new ListBox();
            this.PageMisc = new TabPage();
            this.GrpExtra = new GroupBox();
            this.Misc_numReview = new Flames.Gui.TimespanUpDown();
            this.ChkRestart = new CheckBox();
            this.Misc_lblReview = new Label();
            this.ChkRepeatMessages = new CheckBox();
            this.Chk17Dollar = new CheckBox();
            this.ChkSmile = new CheckBox();
            this.GrpMessages = new GroupBox();
            this.Hack_num = new Flames.Gui.TimespanUpDown();
            this.GrpPhysics = new GroupBox();
            this.TxtRP = new TextBox();
            this.ChkRpNorm = new Label();
            this.TxtNormRp = new TextBox();
            this.Afk_grp = new GroupBox();
            this.Afk_numTimer = new Flames.Gui.TimespanUpDown();
            this.Afk_lblTimer = new Label();
            this.Bak_grp = new GroupBox();
            this.Bak_numTime = new Flames.Gui.TimespanUpDown();
            this.Bak_lblLocation = new Label();
            this.Bak_txtLocation = new TextBox();
            this.Bak_lblTime = new Label();
            this.PageRelay = new TabPage();
            this.Gb_ircSettings = new GroupBox();
            this.Irc_txtPrefix = new TextBox();
            this.Irc_lblPrefix = new Label();
            this.Irc_cmbVerify = new ComboBox();
            this.Irc_lblVerify = new Label();
            this.Irc_cmbRank = new ComboBox();
            this.Irc_lblRank = new Label();
            this.Irc_cbAFK = new CheckBox();
            this.Irc_cbWorldChanges = new CheckBox();
            this.Irc_cbTitles = new CheckBox();
            this.Sql_grp = new GroupBox();
            this.Sql_linkDownload = new LinkLabel();
            this.Sql_lblUser = new Label();
            this.Sql_txtUser = new TextBox();
            this.Sql_lblPass = new Label();
            this.Sql_txtPass = new TextBox();
            this.Sql_lblDBName = new Label();
            this.Sql_txtDBName = new TextBox();
            this.Sql_lblHost = new Label();
            this.Sql_txtHost = new TextBox();
            this.Sql_lblPort = new Label();
            this.Sql_txtPort = new TextBox();
            this.Irc_grp = new GroupBox();
            this.Irc_lblServer = new Label();
            this.Irc_lblPort = new Label();
            this.Irc_numPort = new NumericUpDown();
            this.Irc_lblNick = new Label();
            this.Irc_lblChannel = new Label();
            this.Irc_lblOpChannel = new Label();
            this.PageServer = new TabPage();
            this.Lvl_grp = new GroupBox();
            this.Lvl_lblMain = new Label();
            this.Lvl_txtMain = new TextBox();
            this.Adv_grp = new GroupBox();
            this.Adv_btnEditTexts = new Button();
            this.Srv_grp = new GroupBox();
            this.Srv_lblName = new Label();
            this.Srv_lblMotd = new Label();
            this.Srv_lblPort = new Label();
            this.Srv_btnPort = new Button();
            this.Srv_lblOwner = new Label();
            this.Srv_txtOwner = new TextBox();
            this.Srv_grpUpdate = new GroupBox();
            this.Srv_btnForceUpdate = new Button();
            this.ChkUpdates = new CheckBox();
            this.grpPlayers = new GroupBox();
            this.Srv_lblPlayers = new Label();
            this.Srv_numPlayers = new NumericUpDown();
            this.Srv_cbMustAgree = new CheckBox();
            this.Srv_lblGuests = new Label();
            this.Srv_numGuests = new NumericUpDown();
            this.TabControl = new TabControl();
            this.PageEco = new TabPage();
            this.Eco_gbRank = new GroupBox();
            this.Eco_dgvRanks = new DataGridView();
            this.Eco_colRankName = new DataGridViewTextBoxColumn();
            this.Eco_colRankPrice = new DataGridViewTextBoxColumn();
            this.Eco_cbRank = new CheckBox();
            this.Eco_gbLvl = new GroupBox();
            this.Eco_dgvMaps = new DataGridView();
            this.Eco_colLvlName = new DataGridViewTextBoxColumn();
            this.Eco_colLvlPrice = new DataGridViewTextBoxColumn();
            this.Eco_colLvlX = new DataGridViewTextBoxColumn();
            this.Eco_colLvlY = new DataGridViewTextBoxColumn();
            this.Eco_colLvlZ = new DataGridViewTextBoxColumn();
            this.Eco_colLvlTheme = new DataGridViewComboBoxColumn();
            this.Eco_btnLvlDel = new Button();
            this.Eco_btnLvlAdd = new Button();
            this.Eco_cbLvl = new CheckBox();
            this.Eco_gbItem = new GroupBox();
            this.Eco_lblItemRank = new Label();
            this.Eco_numItemPrice = new NumericUpDown();
            this.Eco_lblItemPrice = new Label();
            this.Eco_cbItem = new CheckBox();
            this.Eco_gb = new GroupBox();
            this.Eco_cmbCfg = new ComboBox();
            this.Eco_lblCfg = new Label();
            this.Eco_cbEnabled = new CheckBox();
            this.Eco_txtCurrency = new TextBox();
            this.Eco_lblCurrency = new Label();
            this.PageGames = new TabPage();
            this.TabGames = new TabControl();
            this.TabLS = new TabPage();
            this.Ls_grpControls = new GroupBox();
            this.Ls_btnEnd = new Button();
            this.Ls_btnStop = new Button();
            this.Ls_btnStart = new Button();
            this.Ls_grpMapSettings = new GroupBox();
            this.Ls_grpTime = new GroupBox();
            this.Ls_numFlood = new Flames.Gui.TimespanUpDown();
            this.Ls_numLayerTime = new Flames.Gui.TimespanUpDown();
            this.Ls_numRound = new Flames.Gui.TimespanUpDown();
            this.Ls_lblLayerTime = new Label();
            this.Ls_lblFlood = new Label();
            this.Ls_lblRound = new Label();
            this.Ls_grpLayer = new GroupBox();
            this.Ls_lblBlocksTall = new Label();
            this.Ls_lblLayersEach = new Label();
            this.Ls_lblLayer = new Label();
            this.Ls_grpFlood = new GroupBox();
            this.Ls_lblFloodUp = new Label();
            this.Ls_lblFast = new Label();
            this.Ls_lblWater = new Label();
            this.Ls_grpSettings = new GroupBox();
            this.Ls_lblMax = new Label();
            this.Ls_cbMap = new CheckBox();
            this.Ls_grpMaps = new GroupBox();
            this.Ls_lblNotUsed = new Label();
            this.Ls_lblUsed = new Label();
            this.Ls_btnAdd = new Button();
            this.Ls_btnRemove = new Button();
            this.Ls_lstNotUsed = new ListBox();
            this.Ls_lstUsed = new ListBox();
            this.TabZS = new TabPage();
            this.Zs_grpControls = new GroupBox();
            this.Zs_btnEnd = new Button();
            this.Zs_btnStop = new Button();
            this.Zs_btnStart = new Button();
            this.Zs_grpMap = new GroupBox();
            this.Zs_grpTime = new GroupBox();
            this.TimespanUpDown1 = new Flames.Gui.TimespanUpDown();
            this.TimespanUpDown2 = new Flames.Gui.TimespanUpDown();
            this.TimespanUpDown3 = new Flames.Gui.TimespanUpDown();
            this.Label1 = new Label();
            this.Label2 = new Label();
            this.Label3 = new Label();
            this.Zs_grpSettings = new GroupBox();
            this.Zs_grpZombie = new GroupBox();
            this.Zs_txtModel = new TextBox();
            this.Zs_txtName = new TextBox();
            this.Zs_lblModel = new Label();
            this.Zs_lblName = new Label();
            this.Zs_grpRevive = new GroupBox();
            this.Zs_lblReviveEff = new Label();
            this.Zs_numReviveEff = new NumericUpDown();
            this.Label4 = new Label();
            this.Zs_lblReviveLimitFtr = new Label();
            this.Zs_lblReviveLimitHdr = new Label();
            this.Zs_numReviveLimit = new NumericUpDown();
            this.Zs_numReviveMax = new NumericUpDown();
            this.Label9 = new Label();
            this.Zs_cbMain = new CheckBox();
            this.Zs_cbMap = new CheckBox();
            this.Zs_grpInv = new GroupBox();
            this.Zs_numInvZombieDur = new NumericUpDown();
            this.Zs_numInvHumanDur = new NumericUpDown();
            this.Zs_numInvZombieMax = new NumericUpDown();
            this.Zs_numInvHumanMax = new NumericUpDown();
            this.Zs_lblInvZombieDur = new Label();
            this.Zs_lblInvZombieMax = new Label();
            this.Zs_lblInvHumanDur = new Label();
            this.Zs_lblInvHumanMax = new Label();
            this.Zs_cbStart = new CheckBox();
            this.Zs_grpMaps = new GroupBox();
            this.Zs_lblNotUsed = new Label();
            this.Zs_lblUsed = new Label();
            this.Zs_btnAdd = new Button();
            this.Zs_btnRemove = new Button();
            this.Zs_lstNotUsed = new ListBox();
            this.Zs_lstUsed = new ListBox();
            this.TabZS_old = new TabPage();
            this.PropsZG = new Flames.Gui.HackyPropertyGrid();
            this.TabCTF = new TabPage();
            this.Ctf_grpControls = new GroupBox();
            this.Ctf_btnEnd = new Button();
            this.Ctf_btnStop = new Button();
            this.Ctf_btnStart = new Button();
            this.Ctf_grpSettings = new GroupBox();
            this.Ctf_cbMain = new CheckBox();
            this.Ctf_cbMap = new CheckBox();
            this.Ctf_cbStart = new CheckBox();
            this.Ctf_grpMaps = new GroupBox();
            this.Ctf_lblNotUsed = new Label();
            this.Ctf_lblUsed = new Label();
            this.Ctf_btnAdd = new Button();
            this.Ctf_btnRemove = new Button();
            this.Ctf_lstNotUsed = new ListBox();
            this.Ctf_lstUsed = new ListBox();
            this.TabTW = new TabPage();
            this.Tw_grpControls = new GroupBox();
            this.Tw_btnEnd = new Button();
            this.Tw_btnStop = new Button();
            this.Tw_btnStart = new Button();
            this.Tw_grpMapSettings = new GroupBox();
            this.Tw_grpTeams = new GroupBox();
            this.Tw_cbKills = new CheckBox();
            this.Tw_cbBalance = new CheckBox();
            this.Tw_grpGrace = new GroupBox();
            this.Tw_numGrace = new Flames.Gui.TimespanUpDown();
            this.Tw_lblGrace = new Label();
            this.Tw_cbGrace = new CheckBox();
            this.Tw_grpScores = new GroupBox();
            this.Tw_lblMulti = new Label();
            this.Tw_lblAssist = new Label();
            this.Tw_cbStreaks = new CheckBox();
            this.Tw_numMultiKills = new NumericUpDown();
            this.Tw_numScoreAssists = new NumericUpDown();
            this.Tw_lblScorePerKill = new Label();
            this.Tw_numScorePerKill = new NumericUpDown();
            this.Tw_lblScoreLimit = new Label();
            this.Tw_numScoreLimit = new NumericUpDown();
            this.Tw_grpSettings = new GroupBox();
            this.Tw_cmbMode = new ComboBox();
            this.Tw_cmbDiff = new ComboBox();
            this.Tw_lblMode = new Label();
            this.Tw_lblDiff = new Label();
            this.Tw_cbMain = new CheckBox();
            this.Tw_cbMap = new CheckBox();
            this.Tw_cbStart = new CheckBox();
            this.Tw_gbMaps = new GroupBox();
            this.Tw_lblInUse = new Label();
            this.Tw_btnAdd = new Button();
            this.Tw_btnRemove = new Button();
            this.Tw_lstNotUsed = new ListBox();
            this.Tw_lstUsed = new ListBox();
            this.TabCD = new TabPage();
            this.Cd_grpControls = new GroupBox();
            this.Cd_btnEnd = new Button();
            this.Cd_btnStop = new Button();
            this.Cd_btnStart = new Button();
            this.Cd_grpSettings = new GroupBox();
            this.Cd_cbMain = new CheckBox();
            this.Cd_cbMap = new CheckBox();
            this.Cd_cbStart = new CheckBox();
            this.Cd_grpMaps = new GroupBox();
            this.Cd_lblNotUsed = new Label();
            this.Cd_lblUsed = new Label();
            this.Cd_btnAdd = new Button();
            this.Cd_btnRemove = new Button();
            this.Cd_lstNotUsed = new ListBox();
            this.Cd_lstUsed = new ListBox();
            this.PageCommands = new TabPage();
            this.Cmd_grpExtra = new GroupBox();
            this.Cmd_cmbExtra7 = new ComboBox();
            this.Cmd_lblExtra7 = new Label();
            this.Cmd_cmbExtra6 = new ComboBox();
            this.Cmd_lblExtra6 = new Label();
            this.Cmd_cmbExtra5 = new ComboBox();
            this.Cmd_lblExtra5 = new Label();
            this.Cmd_cmbExtra4 = new ComboBox();
            this.Cmd_lblExtra4 = new Label();
            this.Cmd_cmbExtra3 = new ComboBox();
            this.Cmd_lblExtra3 = new Label();
            this.Cmd_cmbExtra2 = new ComboBox();
            this.Cmd_lblExtra2 = new Label();
            this.Cmd_cmbExtra1 = new ComboBox();
            this.Cmd_lblExtra1 = new Label();
            this.Cmd_grpPermissions = new GroupBox();
            this.Cmd_cmbAlw3 = new ComboBox();
            this.Cmd_cmbAlw2 = new ComboBox();
            this.Cmd_cmbDis3 = new ComboBox();
            this.Cmd_cmbDis2 = new ComboBox();
            this.Cmd_cmbAlw1 = new ComboBox();
            this.Cmd_cmbDis1 = new ComboBox();
            this.Cmd_cmbMin = new ComboBox();
            this.Cmd_lblMin = new Label();
            this.Cmd_lblDisallow = new Label();
            this.Cmd_lblAllow = new Label();
            this.Cmd_btnCustom = new Button();
            this.Cmd_btnHelp = new Button();
            this.Cmd_list = new ListBox();
            this.PageSecurity = new TabPage();
            this.Sec_grpChat = new GroupBox();
            this.Sec_lblChatOnMute = new Label();
            this.Sec_numChatMsgs = new NumericUpDown();
            this.Sec_lblChatOnMsgs = new Label();
            this.Sec_numChatSecs = new Flames.Gui.TimespanUpDown();
            this.Sec_lblChatForMute = new Label();
            this.Sec_numChatMute = new Flames.Gui.TimespanUpDown();
            this.Sec_grpCmd = new GroupBox();
            this.Sec_cbCmdAuto = new CheckBox();
            this.Sec_lblCmdOnMute = new Label();
            this.Sec_numCmdMsgs = new NumericUpDown();
            this.Sec_lblCmdOnMsgs = new Label();
            this.Sec_numCmdSecs = new Flames.Gui.TimespanUpDown();
            this.Sec_lblCmdForMute = new Label();
            this.Sec_numCmdMute = new Flames.Gui.TimespanUpDown();
            this.Sec_grpIP = new GroupBox();
            this.Sec_cbIPAuto = new CheckBox();
            this.Sec_lblIPOnMute = new Label();
            this.Sec_numIPMsgs = new NumericUpDown();
            this.Sec_lblIPOnMsgs = new Label();
            this.Sec_numIPSecs = new Flames.Gui.TimespanUpDown();
            this.Sec_lblIPForMute = new Label();
            this.Sec_numIPMute = new Flames.Gui.TimespanUpDown();
            this.Sec_grpOther = new GroupBox();
            this.Sec_lblRank = new Label();
            this.Sec_grpBlocks = new GroupBox();
            this.Sec_cbBlocksAuto = new CheckBox();
            this.Sec_lblBlocksOnMute = new Label();
            this.Sec_numBlocksMsgs = new NumericUpDown();
            this.Sec_lblBlocksOnMsgs = new Label();
            this.Sec_numBlocksSecs = new Flames.Gui.TimespanUpDown();
            this.PageChat.SuspendLayout();
            this.Chat_grpTab.SuspendLayout();
            this.Chat_grpMessages.SuspendLayout();
            this.Chat_grpModeration.SuspendLayout();
            this.Chat_grpOther.SuspendLayout();
            this.Chat_grpColors.SuspendLayout();
            ((ISupportInitialize)(this.Srv_numPort)).BeginInit();
            ((ISupportInitialize)(this.Rank_numPerm)).BeginInit();
            ((ISupportInitialize)(this.Rank_numMaps)).BeginInit();
            ((ISupportInitialize)(this.Rank_numDraw)).BeginInit();
            ((ISupportInitialize)(this.Rank_numGen)).BeginInit();
            ((ISupportInitialize)(this.Rank_numCopy)).BeginInit();
            ((ISupportInitialize)(this.Rank_numUndo)).BeginInit();
            ((ISupportInitialize)(this.Ls_numMax)).BeginInit();
            ((ISupportInitialize)(this.Ls_numWater)).BeginInit();
            ((ISupportInitialize)(this.Ls_numFast)).BeginInit();
            ((ISupportInitialize)(this.Ls_numFloodUp)).BeginInit();
            ((ISupportInitialize)(this.Ls_numLayer)).BeginInit();
            ((ISupportInitialize)(this.Ls_numCount)).BeginInit();
            ((ISupportInitialize)(this.Ls_numHeight)).BeginInit();
            ((ISupportInitialize)(this.Rank_numAfk)).BeginInit();
            this.PageBlocks.SuspendLayout();
            this.Blk_grpPhysics.SuspendLayout();
            this.Blk_grpBehaviour.SuspendLayout();
            this.Blk_grpPermissions.SuspendLayout();
            this.PageRanks.SuspendLayout();
            this.Rank_grpLimits.SuspendLayout();
            this.Rank_grpGeneral.SuspendLayout();
            this.Rank_grpMisc.SuspendLayout();
            this.PageMisc.SuspendLayout();
            this.GrpExtra.SuspendLayout();
            ((ISupportInitialize)(this.Misc_numReview)).BeginInit();
            this.GrpMessages.SuspendLayout();
            ((ISupportInitialize)(this.Hack_num)).BeginInit();
            this.GrpPhysics.SuspendLayout();
            this.Afk_grp.SuspendLayout();
            ((ISupportInitialize)(this.Afk_numTimer)).BeginInit();
            this.Bak_grp.SuspendLayout();
            ((ISupportInitialize)(this.Bak_numTime)).BeginInit();
            this.PageRelay.SuspendLayout();
            this.Gb_ircSettings.SuspendLayout();
            this.Sql_grp.SuspendLayout();
            this.Irc_grp.SuspendLayout();
            this.PageServer.SuspendLayout();
            this.Lvl_grp.SuspendLayout();
            this.Adv_grp.SuspendLayout();
            this.Srv_grp.SuspendLayout();
            this.Srv_grpUpdate.SuspendLayout();
            this.grpPlayers.SuspendLayout();
            ((ISupportInitialize)(this.Srv_numPlayers)).BeginInit();
            ((ISupportInitialize)(this.Srv_numGuests)).BeginInit();
            this.TabControl.SuspendLayout();
            this.PageEco.SuspendLayout();
            this.Eco_gbRank.SuspendLayout();
            ((ISupportInitialize)(this.Eco_dgvRanks)).BeginInit();
            this.Eco_gbLvl.SuspendLayout();
            ((ISupportInitialize)(this.Eco_dgvMaps)).BeginInit();
            this.Eco_gbItem.SuspendLayout();
            ((ISupportInitialize)(this.Eco_numItemPrice)).BeginInit();
            this.Eco_gb.SuspendLayout();
            this.PageGames.SuspendLayout();
            this.TabGames.SuspendLayout();
            this.TabLS.SuspendLayout();
            this.Ls_grpControls.SuspendLayout();
            this.Ls_grpMapSettings.SuspendLayout();
            this.Ls_grpTime.SuspendLayout();
            ((ISupportInitialize)(this.Ls_numFlood)).BeginInit();
            ((ISupportInitialize)(this.Ls_numLayerTime)).BeginInit();
            ((ISupportInitialize)(this.Ls_numRound)).BeginInit();
            this.Ls_grpLayer.SuspendLayout();
            this.Ls_grpFlood.SuspendLayout();
            this.Ls_grpSettings.SuspendLayout();
            this.Ls_grpMaps.SuspendLayout();
            this.TabZS.SuspendLayout();
            this.Zs_grpControls.SuspendLayout();
            this.Zs_grpMap.SuspendLayout();
            this.Zs_grpTime.SuspendLayout();
            ((ISupportInitialize)(this.TimespanUpDown1)).BeginInit();
            ((ISupportInitialize)(this.TimespanUpDown2)).BeginInit();
            ((ISupportInitialize)(this.TimespanUpDown3)).BeginInit();
            this.Zs_grpSettings.SuspendLayout();
            this.Zs_grpZombie.SuspendLayout();
            this.Zs_grpRevive.SuspendLayout();
            ((ISupportInitialize)(this.Zs_numReviveEff)).BeginInit();
            ((ISupportInitialize)(this.Zs_numReviveLimit)).BeginInit();
            ((ISupportInitialize)(this.Zs_numReviveMax)).BeginInit();
            this.Zs_grpInv.SuspendLayout();
            ((ISupportInitialize)(this.Zs_numInvZombieDur)).BeginInit();
            ((ISupportInitialize)(this.Zs_numInvHumanDur)).BeginInit();
            ((ISupportInitialize)(this.Zs_numInvZombieMax)).BeginInit();
            ((ISupportInitialize)(this.Zs_numInvHumanMax)).BeginInit();
            this.Zs_grpMaps.SuspendLayout();
            this.TabZS_old.SuspendLayout();
            this.TabCTF.SuspendLayout();
            this.Ctf_grpControls.SuspendLayout();
            this.Ctf_grpSettings.SuspendLayout();
            this.Ctf_grpMaps.SuspendLayout();
            this.TabTW.SuspendLayout();
            this.Tw_grpControls.SuspendLayout();
            this.Tw_grpMapSettings.SuspendLayout();
            this.Tw_grpTeams.SuspendLayout();
            this.Tw_grpGrace.SuspendLayout();
            ((ISupportInitialize)(this.Tw_numGrace)).BeginInit();
            this.Tw_grpScores.SuspendLayout();
            ((ISupportInitialize)(this.Tw_numMultiKills)).BeginInit();
            ((ISupportInitialize)(this.Tw_numScoreAssists)).BeginInit();
            ((ISupportInitialize)(this.Tw_numScorePerKill)).BeginInit();
            ((ISupportInitialize)(this.Tw_numScoreLimit)).BeginInit();
            this.Tw_grpSettings.SuspendLayout();
            this.Tw_gbMaps.SuspendLayout();
            this.TabCD.SuspendLayout();
            this.Cd_grpControls.SuspendLayout();
            this.Cd_grpSettings.SuspendLayout();
            this.Cd_grpMaps.SuspendLayout();
            this.PageCommands.SuspendLayout();
            this.Cmd_grpExtra.SuspendLayout();
            this.Cmd_grpPermissions.SuspendLayout();
            this.PageSecurity.SuspendLayout();
            this.Sec_grpChat.SuspendLayout();
            ((ISupportInitialize)(this.Sec_numChatMsgs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numChatSecs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numChatMute)).BeginInit();
            this.Sec_grpCmd.SuspendLayout();
            ((ISupportInitialize)(this.Sec_numCmdMsgs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numCmdSecs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numCmdMute)).BeginInit();
            this.Sec_grpIP.SuspendLayout();
            ((ISupportInitialize)(this.Sec_numIPMsgs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numIPSecs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numIPMute)).BeginInit();
            this.Sec_grpOther.SuspendLayout();
            this.Sec_grpBlocks.SuspendLayout();
            ((ISupportInitialize)(this.Sec_numBlocksMsgs)).BeginInit();
            ((ISupportInitialize)(this.Sec_numBlocksSecs)).BeginInit();
            this.SuspendLayout();
            // 
            // PageChat
            // 
            this.PageChat.BackColor = SystemColors.Control;
            this.PageChat.Controls.Add(this.Chat_grpTab);
            this.PageChat.Controls.Add(this.Chat_grpMessages);
            this.PageChat.Controls.Add(this.Chat_grpModeration);
            this.PageChat.Controls.Add(this.Chat_grpOther);
            this.PageChat.Controls.Add(this.Chat_grpColors);
            this.PageChat.Location = new Point(4, 22);
            this.PageChat.Name = "PageChat";
            this.PageChat.Padding = new Padding(3);
            this.PageChat.Size = new Size(498, 521);
            this.PageChat.TabIndex = 10;
            this.PageChat.Text = "Chat";
            // 
            // Chat_grpTab
            // 
            this.Chat_grpTab.Controls.Add(this.Chat_cbTabRank);
            this.Chat_grpTab.Controls.Add(this.Chat_cbTabLevel);
            this.Chat_grpTab.Controls.Add(this.Chat_cbTabBots);
            this.Chat_grpTab.Location = new Point(235, 88);
            this.Chat_grpTab.Name = "Chat_grpTab";
            this.Chat_grpTab.Size = new Size(256, 92);
            this.Chat_grpTab.TabIndex = 3;
            this.Chat_grpTab.TabStop = false;
            this.Chat_grpTab.Text = "Tab list";
            // 
            // Chat_cbTabRank
            // 
            this.Chat_cbTabRank.AutoSize = true;
            this.Chat_cbTabRank.Location = new Point(6, 19);
            this.Chat_cbTabRank.Name = "Chat_cbTabRank";
            this.Chat_cbTabRank.Size = new Size(116, 17);
            this.Chat_cbTabRank.TabIndex = 31;
            this.Chat_cbTabRank.Text = "Sort tab list by rank";
            this.Chat_cbTabRank.UseVisualStyleBackColor = true;
            // 
            // Chat_cbTabLevel
            // 
            this.Chat_cbTabLevel.AutoSize = true;
            this.Chat_cbTabLevel.Location = new Point(6, 44);
            this.Chat_cbTabLevel.Name = "Chat_cbTabLevel";
            this.Chat_cbTabLevel.Size = new Size(108, 17);
            this.Chat_cbTabLevel.TabIndex = 30;
            this.Chat_cbTabLevel.Text = "Level only tab list";
            this.Chat_cbTabLevel.UseVisualStyleBackColor = true;
            // 
            // Chat_cbTabBots
            // 
            this.Chat_cbTabBots.AutoSize = true;
            this.Chat_cbTabBots.Location = new Point(6, 69);
            this.Chat_cbTabBots.Name = "Chat_cbTabBots";
            this.Chat_cbTabBots.Size = new Size(120, 17);
            this.Chat_cbTabBots.TabIndex = 32;
            this.Chat_cbTabBots.Text = "Show bots in tab list";
            this.Chat_cbTabBots.UseVisualStyleBackColor = true;
            // 
            // Chat_grpMessages
            // 
            this.Chat_grpMessages.Controls.Add(this.Chat_lblShutdown);
            this.Chat_grpMessages.Controls.Add(this.Chat_txtShutdown);
            this.Chat_grpMessages.Controls.Add(this.Chat_chkCheap);
            this.Chat_grpMessages.Controls.Add(this.Chat_txtCheap);
            this.Chat_grpMessages.Controls.Add(this.Chat_lblLogin);
            this.Chat_grpMessages.Controls.Add(this.Chat_txtLogin);
            this.Chat_grpMessages.Controls.Add(this.Chat_lblLogout);
            this.Chat_grpMessages.Controls.Add(this.Chat_txtLogout);
            this.Chat_grpMessages.Location = new Point(8, 186);
            this.Chat_grpMessages.Name = "Chat_grpMessages";
            this.Chat_grpMessages.Size = new Size(483, 145);
            this.Chat_grpMessages.TabIndex = 2;
            this.Chat_grpMessages.TabStop = false;
            this.Chat_grpMessages.Text = "Messages";
            // 
            // Chat_lblShutdown
            // 
            this.Chat_lblShutdown.AutoSize = true;
            this.Chat_lblShutdown.Location = new Point(6, 23);
            this.Chat_lblShutdown.Name = "Chat_lblShutdown";
            this.Chat_lblShutdown.Size = new Size(101, 13);
            this.Chat_lblShutdown.TabIndex = 34;
            this.Chat_lblShutdown.Text = "Shutdown message:";
            // 
            // Chat_txtShutdown
            // 
            this.Chat_txtShutdown.BackColor = SystemColors.Window;
            this.Chat_txtShutdown.Location = new Point(134, 20);
            this.Chat_txtShutdown.MaxLength = 128;
            this.Chat_txtShutdown.Name = "Chat_txtShutdown";
            this.Chat_txtShutdown.Size = new Size(343, 21);
            this.Chat_txtShutdown.TabIndex = 29;
            // 
            // Chat_chkCheap
            // 
            this.Chat_chkCheap.AutoSize = true;
            this.Chat_chkCheap.Location = new Point(9, 52);
            this.Chat_chkCheap.Name = "Chat_chkCheap";
            this.Chat_chkCheap.Size = new Size(123, 17);
            this.Chat_chkCheap.TabIndex = 30;
            this.Chat_chkCheap.Text = "/invincible message:";
            this.GUIToolTip.SetToolTip(this.Chat_chkCheap, "Is immortality cheap and unfair?");
            this.Chat_chkCheap.UseVisualStyleBackColor = true;
            // 
            // Chat_txtCheap
            // 
            this.Chat_txtCheap.BackColor = SystemColors.Window;
            this.Chat_txtCheap.Location = new Point(134, 50);
            this.Chat_txtCheap.Name = "Chat_txtCheap";
            this.Chat_txtCheap.Size = new Size(343, 21);
            this.Chat_txtCheap.TabIndex = 31;
            // 
            // Chat_lblLogin
            // 
            this.Chat_lblLogin.AutoSize = true;
            this.Chat_lblLogin.Location = new Point(6, 83);
            this.Chat_lblLogin.Name = "Chat_lblLogin";
            this.Chat_lblLogin.Size = new Size(119, 13);
            this.Chat_lblLogin.TabIndex = 43;
            this.Chat_lblLogin.Text = "Default login message:";
            // 
            // Chat_txtLogin
            // 
            this.Chat_txtLogin.BackColor = SystemColors.Window;
            this.Chat_txtLogin.Location = new Point(134, 80);
            this.Chat_txtLogin.MaxLength = 64;
            this.Chat_txtLogin.Name = "Chat_txtLogin";
            this.Chat_txtLogin.Size = new Size(343, 21);
            this.Chat_txtLogin.TabIndex = 42;
            // 
            // Chat_lblLogout
            // 
            this.Chat_lblLogout.AutoSize = true;
            this.Chat_lblLogout.Location = new Point(6, 113);
            this.Chat_lblLogout.Name = "Chat_lblLogout";
            this.Chat_lblLogout.Size = new Size(119, 13);
            this.Chat_lblLogout.TabIndex = 45;
            this.Chat_lblLogout.Text = "Default logout message:";
            // 
            // Chat_txtLogout
            // 
            this.Chat_txtLogout.BackColor = SystemColors.Window;
            this.Chat_txtLogout.Location = new Point(134, 110);
            this.Chat_txtLogout.MaxLength = 64;
            this.Chat_txtLogout.Name = "Chat_txtLogout";
            this.Chat_txtLogout.Size = new Size(343, 21);
            this.Chat_txtLogout.TabIndex = 44;
            // 
            // Chat_grpModeration
            // 
            this.Chat_grpModeration.Controls.Add(this.Chat_lblBan);
            this.Chat_grpModeration.Controls.Add(this.Chat_txtBan);
            this.Chat_grpModeration.Controls.Add(this.Chat_lblPromote);
            this.Chat_grpModeration.Controls.Add(this.Chat_txtPromote);
            this.Chat_grpModeration.Controls.Add(this.Chat_lblDemote);
            this.Chat_grpModeration.Controls.Add(this.Chat_txtDemote);
            this.Chat_grpModeration.Location = new Point(8, 337);
            this.Chat_grpModeration.Name = "Chat_grpModeration";
            this.Chat_grpModeration.Size = new Size(483, 115);
            this.Chat_grpModeration.TabIndex = 3;
            this.Chat_grpModeration.TabStop = false;
            this.Chat_grpModeration.Text = "Moderation messages";
            // 
            // Chat_lblBan
            // 
            this.Chat_lblBan.AutoSize = true;
            this.Chat_lblBan.Location = new Point(6, 23);
            this.Chat_lblBan.Name = "Chat_lblBan";
            this.Chat_lblBan.Size = new Size(100, 13);
            this.Chat_lblBan.TabIndex = 39;
            this.Chat_lblBan.Text = "Default ban reason:";
            // 
            // Chat_txtBan
            // 
            this.Chat_txtBan.BackColor = SystemColors.Window;
            this.Chat_txtBan.Location = new Point(134, 20);
            this.Chat_txtBan.MaxLength = 64;
            this.Chat_txtBan.Name = "Chat_txtBan";
            this.Chat_txtBan.Size = new Size(343, 21);
            this.Chat_txtBan.TabIndex = 33;
            // 
            // Chat_lblPromote
            // 
            this.Chat_lblPromote.AutoSize = true;
            this.Chat_lblPromote.Location = new Point(6, 53);
            this.Chat_lblPromote.Name = "Chat_lblPromote";
            this.Chat_lblPromote.Size = new Size(123, 13);
            this.Chat_lblPromote.TabIndex = 40;
            this.Chat_lblPromote.Text = "Default promote reason:";
            // 
            // Chat_txtPromote
            // 
            this.Chat_txtPromote.BackColor = SystemColors.Window;
            this.Chat_txtPromote.Location = new Point(134, 50);
            this.Chat_txtPromote.MaxLength = 64;
            this.Chat_txtPromote.Name = "Chat_txtPromote";
            this.Chat_txtPromote.Size = new Size(343, 21);
            this.Chat_txtPromote.TabIndex = 36;
            // 
            // Chat_lblDemote
            // 
            this.Chat_lblDemote.AutoSize = true;
            this.Chat_lblDemote.Location = new Point(6, 83);
            this.Chat_lblDemote.Name = "Chat_lblDemote";
            this.Chat_lblDemote.Size = new Size(119, 13);
            this.Chat_lblDemote.TabIndex = 41;
            this.Chat_lblDemote.Text = "Default demote reason:";
            // 
            // Chat_txtDemote
            // 
            this.Chat_txtDemote.BackColor = SystemColors.Window;
            this.Chat_txtDemote.Location = new Point(134, 80);
            this.Chat_txtDemote.MaxLength = 64;
            this.Chat_txtDemote.Name = "Chat_txtDemote";
            this.Chat_txtDemote.Size = new Size(343, 21);
            this.Chat_txtDemote.TabIndex = 38;
            // 
            // Chat_grpOther
            // 
            this.Chat_grpOther.Controls.Add(this.Chat_chkFilter);
            this.Chat_grpOther.Controls.Add(this.Chat_lblConsole);
            this.Chat_grpOther.Controls.Add(this.Chat_txtConsole);
            this.Chat_grpOther.Location = new Point(235, 6);
            this.Chat_grpOther.Name = "Chat_grpOther";
            this.Chat_grpOther.Size = new Size(256, 76);
            this.Chat_grpOther.TabIndex = 1;
            this.Chat_grpOther.TabStop = false;
            this.Chat_grpOther.Text = "Other";
            // 
            // Chat_chkFilter
            // 
            this.Chat_chkFilter.AutoSize = true;
            this.Chat_chkFilter.Location = new Point(6, 49);
            this.Chat_chkFilter.Name = "Chat_chkFilter";
            this.Chat_chkFilter.Size = new Size(96, 17);
            this.Chat_chkFilter.TabIndex = 31;
            this.Chat_chkFilter.Text = "Profanity Filter";
            this.Chat_chkFilter.UseVisualStyleBackColor = true;
            // 
            // Chat_lblConsole
            // 
            this.Chat_lblConsole.AutoSize = true;
            this.Chat_lblConsole.Location = new Point(6, 20);
            this.Chat_lblConsole.Name = "Chat_lblConsole";
            this.Chat_lblConsole.Size = new Size(77, 13);
            this.Chat_lblConsole.TabIndex = 4;
            this.Chat_lblConsole.Text = "Flame name:";
            // 
            // Chat_txtConsole
            // 
            this.Chat_txtConsole.BackColor = SystemColors.Window;
            this.Chat_txtConsole.Location = new Point(89, 17);
            this.Chat_txtConsole.Name = "Chat_txtConsole";
            this.Chat_txtConsole.Size = new Size(161, 21);
            this.Chat_txtConsole.TabIndex = 3;
            // 
            // Chat_grpColors
            // 
            this.Chat_grpColors.Controls.Add(this.Chat_lblWarn);
            this.Chat_grpColors.Controls.Add(this.Chat_btnWarn);
            this.Chat_grpColors.Controls.Add(this.Chat_lblDefault);
            this.Chat_grpColors.Controls.Add(this.Chat_btnDefault);
            this.Chat_grpColors.Controls.Add(this.Chat_lblIRC);
            this.Chat_grpColors.Controls.Add(this.Chat_btnIRC);
            this.Chat_grpColors.Controls.Add(this.Chat_lblSyntax);
            this.Chat_grpColors.Controls.Add(this.Chat_btnSyntax);
            this.Chat_grpColors.Controls.Add(this.Chat_lblDesc);
            this.Chat_grpColors.Controls.Add(this.Chat_btnDesc);
            this.Chat_grpColors.Location = new Point(8, 6);
            this.Chat_grpColors.Name = "Chat_grpColors";
            this.Chat_grpColors.Size = new Size(221, 174);
            this.Chat_grpColors.TabIndex = 0;
            this.Chat_grpColors.TabStop = false;
            this.Chat_grpColors.Text = "Colors";
            // 
            // Chat_lblWarn
            // 
            this.Chat_lblWarn.AutoSize = true;
            this.Chat_lblWarn.Location = new Point(20, 141);
            this.Chat_lblWarn.Name = "Chat_lblWarn";
            this.Chat_lblWarn.Size = new Size(88, 13);
            this.Chat_lblWarn.TabIndex = 35;
            this.Chat_lblWarn.Text = "Warnings/errors:";
            // 
            // Chat_btnWarn
            // 
            this.Chat_btnWarn.Location = new Point(113, 136);
            this.Chat_btnWarn.Name = "Chat_btnWarn";
            this.Chat_btnWarn.Size = new Size(95, 23);
            this.Chat_btnWarn.TabIndex = 34;
            this.GUIToolTip.SetToolTip(this.Chat_btnWarn, "The color of warning/error messages produced by commands");
            this.Chat_btnWarn.Click += new EventHandler(this.Chat_btnWarn_Click);
            // 
            // Chat_lblDefault
            // 
            this.Chat_lblDefault.AutoSize = true;
            this.Chat_lblDefault.Location = new Point(38, 25);
            this.Chat_lblDefault.Name = "Chat_lblDefault";
            this.Chat_lblDefault.Size = new Size(71, 13);
            this.Chat_lblDefault.TabIndex = 11;
            this.Chat_lblDefault.Text = "Default color:";
            // 
            // Chat_btnDefault
            // 
            this.Chat_btnDefault.Location = new Point(113, 20);
            this.Chat_btnDefault.Name = "Chat_btnDefault";
            this.Chat_btnDefault.Size = new Size(95, 23);
            this.Chat_btnDefault.TabIndex = 10;
            this.GUIToolTip.SetToolTip(this.Chat_btnDefault, "The default color of server messages (excluding player chat).\nFor example, when y" +
                        "ou are asked to select two corners in a cuboid.");
            this.Chat_btnDefault.Click += new EventHandler(this.Chat_cmbDefault_Click);
            // 
            // Chat_lblIRC
            // 
            this.Chat_lblIRC.AutoSize = true;
            this.Chat_lblIRC.Location = new Point(36, 54);
            this.Chat_lblIRC.Name = "Chat_lblIRC";
            this.Chat_lblIRC.Size = new Size(74, 13);
            this.Chat_lblIRC.TabIndex = 22;
            this.Chat_lblIRC.Text = "IRC messages:";
            // 
            // Chat_btnIRC
            // 
            this.Chat_btnIRC.Location = new Point(113, 49);
            this.Chat_btnIRC.Name = "Chat_btnIRC";
            this.Chat_btnIRC.Size = new Size(95, 23);
            this.Chat_btnIRC.TabIndex = 24;
            this.GUIToolTip.SetToolTip(this.Chat_btnIRC, "The color of messages from IRC, and nicknames of IRC users.");
            this.Chat_btnIRC.Click += new EventHandler(this.Chat_btnIRC_Click);
            // 
            // Chat_lblSyntax
            // 
            this.Chat_lblSyntax.AutoSize = true;
            this.Chat_lblSyntax.Location = new Point(41, 83);
            this.Chat_lblSyntax.Name = "Chat_lblSyntax";
            this.Chat_lblSyntax.Size = new Size(68, 13);
            this.Chat_lblSyntax.TabIndex = 31;
            this.Chat_lblSyntax.Text = "/help syntax:";
            // 
            // Chat_btnSyntax
            // 
            this.Chat_btnSyntax.Location = new Point(113, 78);
            this.Chat_btnSyntax.Name = "Chat_btnSyntax";
            this.Chat_btnSyntax.Size = new Size(95, 23);
            this.Chat_btnSyntax.TabIndex = 30;
            this.GUIToolTip.SetToolTip(this.Chat_btnSyntax, "The color of the /cmdname [args] in /help.");
            this.Chat_btnSyntax.Click += new EventHandler(this.Chat_btnSyntax_Click);
            // 
            // Chat_lblDesc
            // 
            this.Chat_lblDesc.AutoSize = true;
            this.Chat_lblDesc.Location = new Point(19, 112);
            this.Chat_lblDesc.Name = "Chat_lblDesc";
            this.Chat_lblDesc.Size = new Size(90, 13);
            this.Chat_lblDesc.TabIndex = 32;
            this.Chat_lblDesc.Text = "/help description:";
            // 
            // Chat_btnDesc
            // 
            this.Chat_btnDesc.Location = new Point(113, 107);
            this.Chat_btnDesc.Name = "Chat_btnDesc";
            this.Chat_btnDesc.Size = new Size(95, 23);
            this.Chat_btnDesc.TabIndex = 33;
            this.GUIToolTip.SetToolTip(this.Chat_btnDesc, "The color for the description of a command in /help.");
            this.Chat_btnDesc.Click += new EventHandler(this.Chat_btnDesc_Click);
            // 
            // Main_btnSave
            // 
            this.Main_btnSave.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnSave.Location = new Point(346, 553);
            this.Main_btnSave.Name = "Main_btnSave";
            this.Main_btnSave.Size = new Size(75, 23);
            this.Main_btnSave.TabIndex = 1;
            this.Main_btnSave.Text = "Save";
            this.Main_btnSave.UseVisualStyleBackColor = true;
            this.Main_btnSave.Click += new EventHandler(this.BtnSave_Click);
            // 
            // Main_btnDiscard
            // 
            this.Main_btnDiscard.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnDiscard.Location = new Point(4, 553);
            this.Main_btnDiscard.Name = "Main_btnDiscard";
            this.Main_btnDiscard.Size = new Size(75, 23);
            this.Main_btnDiscard.TabIndex = 1;
            this.Main_btnDiscard.Text = "Discard";
            this.Main_btnDiscard.UseVisualStyleBackColor = true;
            this.Main_btnDiscard.Click += new EventHandler(this.BtnDiscard_Click);
            // 
            // Main_btnApply
            // 
            this.Main_btnApply.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Main_btnApply.Location = new Point(427, 553);
            this.Main_btnApply.Name = "Main_btnApply";
            this.Main_btnApply.Size = new Size(75, 23);
            this.Main_btnApply.TabIndex = 1;
            this.Main_btnApply.Text = "Apply";
            this.Main_btnApply.UseVisualStyleBackColor = true;
            this.Main_btnApply.Click += new EventHandler(this.BtnApply_Click);
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
            // ChkRpLimit
            // 
            this.ChkRpLimit.AutoSize = true;
            this.ChkRpLimit.Location = new Point(5, 52);
            this.ChkRpLimit.Name = "ChkRpLimit";
            this.ChkRpLimit.Size = new Size(48, 13);
            this.ChkRpLimit.TabIndex = 15;
            this.ChkRpLimit.Text = "/rp limit:";
            this.GUIToolTip.SetToolTip(this.ChkRpLimit, "Limit for custom physics set by /rp");
            // 
            // ChkDeath
            // 
            this.ChkDeath.AutoSize = true;
            this.ChkDeath.Location = new Point(6, 20);
            this.ChkDeath.Name = "ChkDeath";
            this.ChkDeath.Size = new Size(84, 17);
            this.ChkDeath.TabIndex = 21;
            this.ChkDeath.Text = "Death count";
            this.GUIToolTip.SetToolTip(this.ChkDeath, "\"Bob has died 10 times.\"");
            this.ChkDeath.UseVisualStyleBackColor = true;
            // 
            // Hack_lbl
            // 
            this.Hack_lbl.AutoSize = true;
            this.Hack_lbl.Location = new Point(7, 20);
            this.Hack_lbl.Name = "Hack_lbl";
            this.Hack_lbl.Size = new Size(193, 17);
            this.Hack_lbl.TabIndex = 32;
            this.Hack_lbl.Text = "Kick people who use hackrank after ";
            this.GUIToolTip.SetToolTip(this.Hack_lbl, "Hackrank kicks people? Or not?");
            this.Hack_lbl.UseVisualStyleBackColor = true;
            // 
            // Sec_cmbVerifyRank
            // 
            this.Sec_cmbVerifyRank.BackColor = SystemColors.Window;
            this.Sec_cmbVerifyRank.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Sec_cmbVerifyRank.FormattingEnabled = true;
            this.Sec_cmbVerifyRank.Location = new Point(72, 95);
            this.Sec_cmbVerifyRank.Name = "Sec_cmbVerifyRank";
            this.Sec_cmbVerifyRank.Size = new Size(103, 21);
            this.Sec_cmbVerifyRank.TabIndex = 22;
            this.GUIToolTip.SetToolTip(this.Sec_cmbVerifyRank, "Minimum rank that is required to use /pass before they can use commands, modify blocks, or chat");
            // 
            // Sec_cbVerifyAdmins
            // 
            this.Sec_cbVerifyAdmins.AutoSize = true;
            this.Sec_cbVerifyAdmins.Location = new Point(15, 74);
            this.Sec_cbVerifyAdmins.Name = "Sec_cbVerifyAdmins";
            this.Sec_cbVerifyAdmins.Size = new Size(111, 17);
            this.Sec_cbVerifyAdmins.TabIndex = 23;
            this.Sec_cbVerifyAdmins.Text = "Admin verification";
            this.GUIToolTip.SetToolTip(this.Sec_cbVerifyAdmins, "If enabled, admins must verify with /pass before they can use commands, modify blocks, or chat");
            this.Sec_cbVerifyAdmins.UseVisualStyleBackColor = true;
            this.Sec_cbVerifyAdmins.CheckedChanged += new EventHandler(this.VerifyAdminsChecked);
            // 
            // ChkGuestLimitNotify
            // 
            this.ChkGuestLimitNotify.AutoSize = true;
            this.ChkGuestLimitNotify.Location = new Point(6, 134);
            this.ChkGuestLimitNotify.Name = "ChkGuestLimitNotify";
            this.ChkGuestLimitNotify.Size = new Size(109, 17);
            this.ChkGuestLimitNotify.TabIndex = 46;
            this.ChkGuestLimitNotify.Text = "Guest Limit Notify";
            this.GUIToolTip.SetToolTip(this.ChkGuestLimitNotify, "Notify in-game if a guest can\'t join due to the guest limit being reached.");
            this.ChkGuestLimitNotify.UseVisualStyleBackColor = true;
            // 
            // Rank_cbTPHigher
            // 
            this.Rank_cbTPHigher.AutoSize = true;
            this.Rank_cbTPHigher.Location = new Point(11, 75);
            this.Rank_cbTPHigher.Name = "Rank_cbTPHigher";
            this.Rank_cbTPHigher.Size = new Size(136, 17);
            this.Rank_cbTPHigher.TabIndex = 42;
            this.Rank_cbTPHigher.Text = "Allow tp to higher ranks";
            this.GUIToolTip.SetToolTip(this.Rank_cbTPHigher, "Allows players to /tp to higher ranked players");
            this.Rank_cbTPHigher.UseVisualStyleBackColor = true;
            // 
            // Rank_cmbDefault
            // 
            this.Rank_cmbDefault.BackColor = SystemColors.Window;
            this.Rank_cmbDefault.FormattingEnabled = true;
            this.Rank_cmbDefault.Location = new Point(85, 20);
            this.Rank_cmbDefault.Name = "Rank_cmbDefault";
            this.Rank_cmbDefault.Size = new Size(81, 21);
            this.Rank_cmbDefault.TabIndex = 44;
            this.GUIToolTip.SetToolTip(this.Rank_cmbDefault, "Default rank assigned to new players.");
            // 
            // Sec_cbWhitelist
            // 
            this.Sec_cbWhitelist.Location = new Point(15, 44);
            this.Sec_cbWhitelist.Name = "Sec_cbWhitelist";
            this.Sec_cbWhitelist.Size = new Size(104, 24);
            this.Sec_cbWhitelist.TabIndex = 23;
            this.Sec_cbWhitelist.Text = "Use whitelist";
            this.GUIToolTip.SetToolTip(this.Sec_cbWhitelist, "If enabled, only players who have been whitelisted with /whitelist are allowed to" +
                        " join");
            this.Sec_cbWhitelist.UseVisualStyleBackColor = true;
            // 
            // Sql_chkUseSQL
            // 
            this.Sql_chkUseSQL.AutoSize = true;
            this.Sql_chkUseSQL.Location = new Point(12, 20);
            this.Sql_chkUseSQL.Name = "Sql_chkUseSQL";
            this.Sql_chkUseSQL.Size = new Size(77, 17);
            this.Sql_chkUseSQL.TabIndex = 28;
            this.Sql_chkUseSQL.Text = "Use MySQL";
            this.GUIToolTip.SetToolTip(this.Sql_chkUseSQL, "Whether to use MySQL instead of SQLite for database storage. You will need to hav" +
                        "e installed it for this to work.");
            this.Sql_chkUseSQL.UseVisualStyleBackColor = true;
            this.Sql_chkUseSQL.CheckedChanged += new EventHandler(this.Sql_chkUseSQL_CheckedChanged);
            // 
            // Irc_chkEnabled
            // 
            this.Irc_chkEnabled.AutoSize = true;
            this.Irc_chkEnabled.Location = new Point(9, 20);
            this.Irc_chkEnabled.Name = "Irc_chkEnabled";
            this.Irc_chkEnabled.Size = new Size(61, 17);
            this.Irc_chkEnabled.TabIndex = 22;
            this.Irc_chkEnabled.Text = "Use IRC";
            this.GUIToolTip.SetToolTip(this.Irc_chkEnabled, "Whether to use the IRC bot or not.\nIRC stands for Internet Relay Chat and allows " +
                        "for communication with the server while outside Minecraft.");
            this.Irc_chkEnabled.UseVisualStyleBackColor = true;
            this.Irc_chkEnabled.CheckedChanged += new EventHandler(this.Irc_chkEnabled_CheckedChanged);
            // 
            // Irc_txtServer
            // 
            this.Irc_txtServer.BackColor = SystemColors.Window;
            this.Irc_txtServer.Location = new Point(82, 47);
            this.Irc_txtServer.Name = "Irc_txtServer";
            this.Irc_txtServer.Size = new Size(106, 21);
            this.Irc_txtServer.TabIndex = 15;
            this.GUIToolTip.SetToolTip(this.Irc_txtServer, "IRC server hostname.\nDefault = irc.esper.net\nAnother choice = irc.geekshed.net");
            // 
            // Irc_txtNick
            // 
            this.Irc_txtNick.BackColor = SystemColors.Window;
            this.Irc_txtNick.Location = new Point(82, 101);
            this.Irc_txtNick.Name = "Irc_txtNick";
            this.Irc_txtNick.Size = new Size(106, 21);
            this.Irc_txtNick.TabIndex = 16;
            this.GUIToolTip.SetToolTip(this.Irc_txtNick, "The Nick that the IRC bot will try and use.");
            // 
            // Irc_txtChannel
            // 
            this.Irc_txtChannel.BackColor = SystemColors.Window;
            this.Irc_txtChannel.Location = new Point(82, 128);
            this.Irc_txtChannel.Name = "Irc_txtChannel";
            this.Irc_txtChannel.Size = new Size(106, 21);
            this.Irc_txtChannel.TabIndex = 17;
            this.GUIToolTip.SetToolTip(this.Irc_txtChannel, "The IRC channel to be used for general chat");
            // 
            // Irc_txtOpChannel
            // 
            this.Irc_txtOpChannel.BackColor = SystemColors.Window;
            this.Irc_txtOpChannel.Location = new Point(82, 155);
            this.Irc_txtOpChannel.Name = "Irc_txtOpChannel";
            this.Irc_txtOpChannel.Size = new Size(106, 21);
            this.Irc_txtOpChannel.TabIndex = 26;
            this.GUIToolTip.SetToolTip(this.Irc_txtOpChannel, "The IRC channel to be used for opchat");
            // 
            // Lvl_chkAutoload
            // 
            this.Lvl_chkAutoload.AutoSize = true;
            this.Lvl_chkAutoload.Location = new Point(9, 49);
            this.Lvl_chkAutoload.Name = "Lvl_chkAutoload";
            this.Lvl_chkAutoload.Size = new Size(90, 17);
            this.Lvl_chkAutoload.TabIndex = 4;
            this.Lvl_chkAutoload.Text = "Load on /goto";
            this.GUIToolTip.SetToolTip(this.Lvl_chkAutoload, "Load a map when a user wishes to go to it, and unload empty maps");
            this.Lvl_chkAutoload.UseVisualStyleBackColor = true;
            // 
            // Lvl_chkWorld
            // 
            this.Lvl_chkWorld.AutoSize = true;
            this.Lvl_chkWorld.Location = new Point(9, 72);
            this.Lvl_chkWorld.Name = "Lvl_chkWorld";
            this.Lvl_chkWorld.Size = new Size(105, 17);
            this.Lvl_chkWorld.TabIndex = 4;
            this.Lvl_chkWorld.Text = "Server-wide chat";
            this.GUIToolTip.SetToolTip(this.Lvl_chkWorld, "If disabled, every map has isolated chat.\nIf enabled, every map is able to commun" +
                        "icate without special letters.");
            this.Lvl_chkWorld.UseVisualStyleBackColor = true;
            // 
            // Adv_chkVerify
            // 
            this.Adv_chkVerify.AutoSize = true;
            this.Adv_chkVerify.Location = new Point(9, 20);
            this.Adv_chkVerify.Name = "Adv_chkVerify";
            this.Adv_chkVerify.Size = new Size(87, 17);
            this.Adv_chkVerify.TabIndex = 4;
            this.Adv_chkVerify.Text = "Verify Names";
            this.GUIToolTip.SetToolTip(this.Adv_chkVerify, "Make sure the user is who they claim to be.");
            this.Adv_chkVerify.UseVisualStyleBackColor = true;
            this.Adv_chkVerify.CheckedChanged += new EventHandler(this.ChkVerify_CheckedChanged);
            // 
            // Srv_txtName
            // 
            this.Srv_txtName.BackColor = SystemColors.Window;
            this.Srv_txtName.Location = new Point(83, 19);
            this.Srv_txtName.MaxLength = 64;
            this.Srv_txtName.Name = "Srv_txtName";
            this.Srv_txtName.Size = new Size(387, 21);
            this.Srv_txtName.TabIndex = 0;
            this.GUIToolTip.SetToolTip(this.Srv_txtName, "The name of the server.\nPick something good!");
            // 
            // Srv_txtMOTD
            // 
            this.Srv_txtMOTD.BackColor = SystemColors.Window;
            this.Srv_txtMOTD.Location = new Point(83, 46);
            this.Srv_txtMOTD.MaxLength = 64;
            this.Srv_txtMOTD.Name = "Srv_txtMOTD";
            this.Srv_txtMOTD.Size = new Size(387, 21);
            this.Srv_txtMOTD.TabIndex = 1;
            this.GUIToolTip.SetToolTip(this.Srv_txtMOTD, "The MOTD of the server.\nUse \"+hax\" to allow any WoM hack, \"-hax\" to disallow any " +
                        "hacks at all and use \"-fly\" and whatnot to disallow other things.");
            // 
            // Srv_numPort
            // 
            this.Srv_numPort.BackColor = SystemColors.Window;
            this.Srv_numPort.Location = new Point(83, 73);
            this.Srv_numPort.Maximum = new decimal(new int[] {
                                    65535,
                                    0,
                                    0,
                                    0});
            this.Srv_numPort.Name = "Srv_numPort";
            this.Srv_numPort.Size = new Size(60, 21);
            this.Srv_numPort.TabIndex = 2;
            this.GUIToolTip.SetToolTip(this.Srv_numPort, "The port that the server will output on.\nDefault = 25565\n\nChanging will reset you" +
                        "r ExternalURL.");
            this.Srv_numPort.Value = new decimal(new int[] {
                                    25565,
                                    0,
                                    0,
                                    0});
            // 
            // Srv_chkPublic
            // 
            this.Srv_chkPublic.AutoSize = true;
            this.Srv_chkPublic.Location = new Point(9, 124);
            this.Srv_chkPublic.Name = "Srv_chkPublic";
            this.Srv_chkPublic.Size = new Size(55, 17);
            this.Srv_chkPublic.TabIndex = 5;
            this.Srv_chkPublic.Text = "Public";
            this.GUIToolTip.SetToolTip(this.Srv_chkPublic, "Whether or not the server will appear on the server list.");
            this.Srv_chkPublic.UseVisualStyleBackColor = true;
            // 
            // Rank_cbSilentAdmins
            // 
            this.Rank_cbSilentAdmins.AutoSize = true;
            this.Rank_cbSilentAdmins.Location = new Point(11, 52);
            this.Rank_cbSilentAdmins.Name = "Rank_cbSilentAdmins";
            this.Rank_cbSilentAdmins.Size = new Size(118, 17);
            this.Rank_cbSilentAdmins.TabIndex = 41;
            this.Rank_cbSilentAdmins.Text = "Admins join silently";
            this.GUIToolTip.SetToolTip(this.Rank_cbSilentAdmins, "Players who can read adminchat also join the server silently");
            this.Rank_cbSilentAdmins.UseVisualStyleBackColor = true;
            // 
            // Rank_txtPrefix
            // 
            this.Rank_txtPrefix.BackColor = SystemColors.Window;
            this.Rank_txtPrefix.Location = new Point(259, 47);
            this.Rank_txtPrefix.Name = "Rank_txtPrefix";
            this.Rank_txtPrefix.Size = new Size(81, 21);
            this.Rank_txtPrefix.TabIndex = 21;
            this.GUIToolTip.SetToolTip(this.Rank_txtPrefix, "Short prefix showed before player names in chat.");
            this.Rank_txtPrefix.TextChanged += new EventHandler(this.Rank_txtPrefix_TextChanged);
            // 
            // Rank_txtMOTD
            // 
            this.Rank_txtMOTD.BackColor = SystemColors.Window;
            this.Rank_txtMOTD.Location = new Point(85, 74);
            this.Rank_txtMOTD.Name = "Rank_txtMOTD";
            this.Rank_txtMOTD.Size = new Size(255, 21);
            this.Rank_txtMOTD.TabIndex = 17;
            this.GUIToolTip.SetToolTip(this.Rank_txtMOTD, "MOTD shown to players of this rank.\r\nIf left blank, the server MOTD is shown to t" +
                        "hem.");
            this.Rank_txtMOTD.TextChanged += new EventHandler(this.Rank_txtMOTD_TextChanged);
            // 
            // Rank_numPerm
            // 
            this.Rank_numPerm.BackColor = SystemColors.Window;
            this.Rank_numPerm.Location = new Point(259, 20);
            this.Rank_numPerm.Maximum = new decimal(new int[] {
                                    127,
                                    0,
                                    0,
                                    0});
            this.Rank_numPerm.Minimum = new decimal(new int[] {
                                    20,
                                    0,
                                    0,
                                    -2147483648});
            this.Rank_numPerm.Name = "Rank_numPerm";
            this.Rank_numPerm.Size = new Size(81, 21);
            this.Rank_numPerm.TabIndex = 6;
            this.GUIToolTip.SetToolTip(this.Rank_numPerm, "Permission level of this rank.");
            this.Rank_numPerm.ValueChanged += new EventHandler(this.Rank_numPerm_ValueChanged);
            // 
            // Rank_txtName
            // 
            this.Rank_txtName.BackColor = SystemColors.Window;
            this.Rank_txtName.Location = new Point(85, 20);
            this.Rank_txtName.Name = "Rank_txtName";
            this.Rank_txtName.Size = new Size(81, 21);
            this.Rank_txtName.TabIndex = 5;
            this.GUIToolTip.SetToolTip(this.Rank_txtName, "Name of this rank");
            this.Rank_txtName.TextChanged += new EventHandler(this.Rank_txtName_TextChanged);
            // 
            // Rank_btnColor
            // 
            this.Rank_btnColor.Location = new Point(85, 47);
            this.Rank_btnColor.Name = "Rank_btnColor";
            this.Rank_btnColor.Size = new Size(81, 23);
            this.Rank_btnColor.TabIndex = 12;
            this.GUIToolTip.SetToolTip(this.Rank_btnColor, "Color of this rank name in chat and the tab list");
            this.Rank_btnColor.MouseClick += new MouseEventHandler(this.Rank_btnColor_Click);
            // 
            // Rank_cmbOsMap
            // 
            this.Rank_cmbOsMap.BackColor = SystemColors.Window;
            this.Rank_cmbOsMap.FormattingEnabled = true;
            this.Rank_cmbOsMap.Location = new Point(259, 20);
            this.Rank_cmbOsMap.Name = "Rank_cmbOsMap";
            this.Rank_cmbOsMap.Size = new Size(80, 21);
            this.Rank_cmbOsMap.TabIndex = 49;
            this.GUIToolTip.SetToolTip(this.Rank_cmbOsMap, "Default minimum rank required to build on maps made with /os map add.");
            // 
            // Irc_chkPass
            // 
            this.Irc_chkPass.AutoSize = true;
            this.Irc_chkPass.Location = new Point(9, 185);
            this.Irc_chkPass.Name = "Irc_chkPass";
            this.Irc_chkPass.Size = new Size(72, 17);
            this.Irc_chkPass.TabIndex = 27;
            this.Irc_chkPass.Text = "Password";
            this.GUIToolTip.SetToolTip(this.Irc_chkPass, "NickServ password set for the username");
            this.Irc_chkPass.UseVisualStyleBackColor = true;
            this.Irc_chkPass.CheckedChanged += new EventHandler(this.Irc_chkPass_CheckedChanged);
            // 
            // Irc_txtPass
            // 
            this.Irc_txtPass.BackColor = SystemColors.Window;
            this.Irc_txtPass.Location = new Point(82, 182);
            this.Irc_txtPass.Name = "Irc_txtPass";
            this.Irc_txtPass.PasswordChar = '*';
            this.Irc_txtPass.Size = new Size(106, 21);
            this.Irc_txtPass.TabIndex = 28;
            this.GUIToolTip.SetToolTip(this.Irc_txtPass, "NickServ password set for the username");
            // 
            // Rank_numMaps
            // 
            this.Rank_numMaps.BackColor = SystemColors.Window;
            this.Rank_numMaps.Location = new Point(259, 20);
            this.Rank_numMaps.Maximum = new decimal(new int[] {
                                    2147483647,
                                    0,
                                    0,
                                    0});
            this.Rank_numMaps.Name = "Rank_numMaps";
            this.Rank_numMaps.Size = new Size(81, 21);
            this.Rank_numMaps.TabIndex = 19;
            this.GUIToolTip.SetToolTip(this.Rank_numMaps, "Maximum number of /os maps players are allowed");
            this.Rank_numMaps.ValueChanged += new EventHandler(this.Rank_numMaps_ValueChanged);
            // 
            // Rank_numDraw
            // 
            this.Rank_numDraw.BackColor = SystemColors.Window;
            this.Rank_numDraw.Location = new Point(85, 20);
            this.Rank_numDraw.Maximum = new decimal(new int[] {
                                    2147483647,
                                    0,
                                    0,
                                    0});
            this.Rank_numDraw.Name = "Rank_numDraw";
            this.Rank_numDraw.Size = new Size(81, 21);
            this.Rank_numDraw.TabIndex = 4;
            this.GUIToolTip.SetToolTip(this.Rank_numDraw, "Maximum number of blocks players can affect in draw commands.");
            this.Rank_numDraw.ValueChanged += new EventHandler(this.Rank_numDraw_ValueChanged);
            // 
            // Rank_numGen
            // 
            this.Rank_numGen.BackColor = SystemColors.Window;
            this.Rank_numGen.Location = new Point(259, 47);
            this.Rank_numGen.Maximum = new decimal(new int[] {
                                    2147483647,
                                    0,
                                    0,
                                    0});
            this.Rank_numGen.Name = "Rank_numGen";
            this.Rank_numGen.Size = new Size(81, 21);
            this.Rank_numGen.TabIndex = 21;
            this.GUIToolTip.SetToolTip(this.Rank_numGen, "Maximum volume of (number of blocks in) a map that players can generate");
            this.Rank_numGen.ValueChanged += new EventHandler(this.Rank_numGen_ValueChanged);
            // 
            // Rank_numCopy
            // 
            this.Rank_numCopy.BackColor = SystemColors.Window;
            this.Rank_numCopy.Location = new Point(85, 74);
            this.Rank_numCopy.Maximum = new decimal(new int[] {
                                    255,
                                    0,
                                    0,
                                    0});
            this.Rank_numCopy.Minimum = new decimal(new int[] {
                                    1,
                                    0,
                                    0,
                                    0});
            this.Rank_numCopy.Name = "Rank_numCopy";
            this.Rank_numCopy.Size = new Size(81, 21);
            this.Rank_numCopy.TabIndex = 23;
            this.GUIToolTip.SetToolTip(this.Rank_numCopy, "Maximum number of copies player can select in /copyslot");
            this.Rank_numCopy.Value = new decimal(new int[] {
                                    1,
                                    0,
                                    0,
                                    0});
            this.Rank_numCopy.ValueChanged += new EventHandler(this.Rank_numCopy_ValueChanged);
            // 
            // Adv_chkCPE
            // 
            this.Adv_chkCPE.AutoSize = true;
            this.Adv_chkCPE.Location = new Point(9, 43);
            this.Adv_chkCPE.Name = "Adv_chkCPE";
            this.Adv_chkCPE.Size = new Size(122, 17);
            this.Adv_chkCPE.TabIndex = 4;
            this.Adv_chkCPE.Text = "Non-classic features";
            this.GUIToolTip.SetToolTip(this.Adv_chkCPE, "Enables custom blocks, multiline chat, changing env settings, etc");
            this.Adv_chkCPE.UseVisualStyleBackColor = true;
            // 
            // Eco_cmbItemRank
            // 
            this.Eco_cmbItemRank.BackColor = SystemColors.Window;
            this.Eco_cmbItemRank.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Eco_cmbItemRank.FormattingEnabled = true;
            this.Eco_cmbItemRank.Location = new Point(368, 43);
            this.Eco_cmbItemRank.Name = "Eco_cmbItemRank";
            this.Eco_cmbItemRank.Size = new Size(110, 21);
            this.Eco_cmbItemRank.TabIndex = 23;
            this.GUIToolTip.SetToolTip(this.Eco_cmbItemRank, "Minimum rank a player must have to purchase this item.");
            this.Eco_cmbItemRank.SelectedIndexChanged += new EventHandler(this.Eco_cmbItemRank_SelectedIndexChanged);
            // 
            // Rank_numUndo
            // 
            this.Rank_numUndo.BackColor = SystemColors.Window;
            this.Rank_numUndo.Location = new Point(85, 47);
            this.Rank_numUndo.Name = "Rank_numUndo";
            this.Rank_numUndo.Seconds = ((long)(0));
            this.Rank_numUndo.Size = new Size(81, 21);
            this.Rank_numUndo.TabIndex = 24;
            this.Rank_numUndo.Text = "0s";
            this.GUIToolTip.SetToolTip(this.Rank_numUndo, "Maximum time players can undo up to in the past with /undo");
            this.Rank_numUndo.Value = TimeSpan.Parse("00:00:00");
            this.Rank_numUndo.ValueChanged += new EventHandler(this.Rank_numUndo_ValueChanged);
            // 
            // ChkPhysRestart
            // 
            this.ChkPhysRestart.AutoSize = true;
            this.ChkPhysRestart.Location = new Point(6, 20);
            this.ChkPhysRestart.Name = "ChkPhysRestart";
            this.ChkPhysRestart.Size = new Size(124, 17);
            this.ChkPhysRestart.TabIndex = 52;
            this.ChkPhysRestart.Text = "Restart on shutdown";
            this.ChkPhysRestart.UseVisualStyleBackColor = true;
            // 
            // Ls_numMax
            // 
            this.Ls_numMax.BackColor = SystemColors.Window;
            this.Ls_numMax.Location = new Point(71, 89);
            this.Ls_numMax.Maximum = new decimal(new int[] {
                                    1000000,
                                    0,
                                    0,
                                    0});
            this.Ls_numMax.Name = "Ls_numMax";
            this.Ls_numMax.Size = new Size(52, 21);
            this.Ls_numMax.TabIndex = 25;
            this.Ls_numMax.Value = new decimal(new int[] {
                                    3,
                                    0,
                                    0,
                                    0});
            // 
            // Ls_numWater
            // 
            this.Ls_numWater.BackColor = SystemColors.Window;
            this.Ls_numWater.Location = new Point(79, 20);
            this.Ls_numWater.Name = "Ls_numWater";
            this.Ls_numWater.Size = new Size(52, 21);
            this.Ls_numWater.TabIndex = 27;
            // 
            // Ls_numFast
            // 
            this.Ls_numFast.BackColor = SystemColors.Window;
            this.Ls_numFast.Location = new Point(79, 45);
            this.Ls_numFast.Name = "Ls_numFast";
            this.Ls_numFast.Size = new Size(52, 21);
            this.Ls_numFast.TabIndex = 31;
            // 
            // Ls_numFloodUp
            // 
            this.Ls_numFloodUp.BackColor = SystemColors.Window;
            this.Ls_numFloodUp.Location = new Point(226, 45);
            this.Ls_numFloodUp.Name = "Ls_numFloodUp";
            this.Ls_numFloodUp.Size = new Size(52, 21);
            this.Ls_numFloodUp.TabIndex = 33;
            // 
            // Ls_numLayer
            // 
            this.Ls_numLayer.BackColor = SystemColors.Window;
            this.Ls_numLayer.Location = new Point(128, 16);
            this.Ls_numLayer.Name = "Ls_numLayer";
            this.Ls_numLayer.Size = new Size(52, 21);
            this.Ls_numLayer.TabIndex = 34;
            // 
            // Ls_numCount
            // 
            this.Ls_numCount.BackColor = SystemColors.Window;
            this.Ls_numCount.Location = new Point(7, 44);
            this.Ls_numCount.Maximum = new decimal(new int[] {
                                    1000000,
                                    0,
                                    0,
                                    0});
            this.Ls_numCount.Name = "Ls_numCount";
            this.Ls_numCount.Size = new Size(52, 21);
            this.Ls_numCount.TabIndex = 35;
            this.Ls_numCount.Value = new decimal(new int[] {
                                    10,
                                    0,
                                    0,
                                    0});
            // 
            // Ls_numHeight
            // 
            this.Ls_numHeight.BackColor = SystemColors.Window;
            this.Ls_numHeight.Location = new Point(128, 44);
            this.Ls_numHeight.Maximum = new decimal(new int[] {
                                    1000000,
                                    0,
                                    0,
                                    0});
            this.Ls_numHeight.Name = "Ls_numHeight";
            this.Ls_numHeight.Size = new Size(52, 21);
            this.Ls_numHeight.TabIndex = 37;
            this.Ls_numHeight.Value = new decimal(new int[] {
                                    3,
                                    0,
                                    0,
                                    0});
            // 
            // Ls_cbMain
            // 
            this.Ls_cbMain.AutoSize = true;
            this.Ls_cbMain.Location = new Point(11, 66);
            this.Ls_cbMain.Name = "Ls_cbMain";
            this.Ls_cbMain.Size = new Size(112, 17);
            this.Ls_cbMain.TabIndex = 24;
            this.Ls_cbMain.Text = "Change main level";
            this.Ls_cbMain.UseVisualStyleBackColor = true;
            // 
            // Ls_cbStart
            // 
            this.Ls_cbStart.AutoSize = true;
            this.Ls_cbStart.Location = new Point(11, 20);
            this.Ls_cbStart.Name = "Ls_cbStart";
            this.Ls_cbStart.Size = new Size(139, 17);
            this.Ls_cbStart.TabIndex = 22;
            this.Ls_cbStart.Text = "Start when server starts";
            this.Ls_cbStart.UseVisualStyleBackColor = true;
            // 
            // Rank_numAfk
            // 
            this.Rank_numAfk.BackColor = SystemColors.Window;
            this.Rank_numAfk.Location = new Point(113, 102);
            this.Rank_numAfk.Name = "Rank_numAfk";
            this.Rank_numAfk.Seconds = ((long)(0));
            this.Rank_numAfk.Size = new Size(62, 21);
            this.Rank_numAfk.TabIndex = 23;
            this.Rank_numAfk.Text = "0s";
            this.Rank_numAfk.Value = TimeSpan.Parse("00:00:00");
            this.Rank_numAfk.ValueChanged += new EventHandler(this.Rank_numAfk_ValueChanged);
            // 
            // Sec_cbLogNotes
            // 
            this.Sec_cbLogNotes.AutoSize = true;
            this.Sec_cbLogNotes.Location = new Point(15, 20);
            this.Sec_cbLogNotes.Name = "Sec_cbLogNotes";
            this.Sec_cbLogNotes.Size = new Size(178, 17);
            this.Sec_cbLogNotes.TabIndex = 22;
            this.Sec_cbLogNotes.Text = "Log notes (/ban, /warn, /kick etc)";
            this.Sec_cbLogNotes.UseVisualStyleBackColor = true;
            // 
            // Sec_cbChatAuto
            // 
            this.Sec_cbChatAuto.AutoSize = true;
            this.Sec_cbChatAuto.Location = new Point(10, 20);
            this.Sec_cbChatAuto.Name = "Sec_cbChatAuto";
            this.Sec_cbChatAuto.Size = new Size(142, 17);
            this.Sec_cbChatAuto.TabIndex = 24;
            this.Sec_cbChatAuto.Text = "Enable automatic muting";
            this.Sec_cbChatAuto.UseVisualStyleBackColor = true;
            this.Sec_cbChatAuto.CheckedChanged += new EventHandler(this.Sec_cbChatAuto_Checked);
            // 
            // PageBlocks
            // 
            this.PageBlocks.BackColor = SystemColors.Control;
            this.PageBlocks.Controls.Add(this.Blk_grpPhysics);
            this.PageBlocks.Controls.Add(this.Blk_grpBehaviour);
            this.PageBlocks.Controls.Add(this.Blk_grpPermissions);
            this.PageBlocks.Controls.Add(this.Blk_btnHelp);
            this.PageBlocks.Controls.Add(this.Blk_list);
            this.PageBlocks.Location = new Point(4, 22);
            this.PageBlocks.Name = "PageBlocks";
            this.PageBlocks.Padding = new Padding(3);
            this.PageBlocks.Size = new Size(498, 521);
            this.PageBlocks.TabIndex = 5;
            this.PageBlocks.Text = "Blocks";
            // 
            // Blk_grpPhysics
            // 
            this.Blk_grpPhysics.Controls.Add(this.Blk_cbWater);
            this.Blk_grpPhysics.Controls.Add(this.Blk_cbLava);
            this.Blk_grpPhysics.Controls.Add(this.Blk_cbRails);
            this.Blk_grpPhysics.Controls.Add(this.Blk_cbTdoor);
            this.Blk_grpPhysics.Controls.Add(this.Blk_cbDoor);
            this.Blk_grpPhysics.Location = new Point(134, 180);
            this.Blk_grpPhysics.Name = "Blk_grpPhysics";
            this.Blk_grpPhysics.Size = new Size(360, 92);
            this.Blk_grpPhysics.TabIndex = 26;
            this.Blk_grpPhysics.TabStop = false;
            this.Blk_grpPhysics.Text = "Physics behaviour";
            // 
            // Blk_cbWater
            // 
            this.Blk_cbWater.Location = new Point(186, 65);
            this.Blk_cbWater.Name = "Blk_cbWater";
            this.Blk_cbWater.Size = new Size(104, 24);
            this.Blk_cbWater.TabIndex = 7;
            this.Blk_cbWater.Text = "Killed by water";
            this.Blk_cbWater.UseVisualStyleBackColor = true;
            this.Blk_cbWater.CheckedChanged += new EventHandler(this.Blk_cbWater_CheckedChanged);
            // 
            // Blk_cbLava
            // 
            this.Blk_cbLava.Location = new Point(10, 65);
            this.Blk_cbLava.Name = "Blk_cbLava";
            this.Blk_cbLava.Size = new Size(116, 24);
            this.Blk_cbLava.TabIndex = 6;
            this.Blk_cbLava.Text = "Killed by lava";
            this.Blk_cbLava.UseVisualStyleBackColor = true;
            this.Blk_cbLava.CheckedChanged += new EventHandler(this.Blk_cbLava_CheckedChanged);
            // 
            // Blk_cbRails
            // 
            this.Blk_cbRails.Location = new Point(10, 40);
            this.Blk_cbRails.Name = "Blk_cbRails";
            this.Blk_cbRails.Size = new Size(89, 24);
            this.Blk_cbRails.TabIndex = 5;
            this.Blk_cbRails.Text = "Is train rails";
            this.Blk_cbRails.UseVisualStyleBackColor = true;
            this.Blk_cbRails.CheckedChanged += new EventHandler(this.Blk_cbRails_CheckedChanged);
            // 
            // Blk_cbTdoor
            // 
            this.Blk_cbTdoor.Location = new Point(187, 15);
            this.Blk_cbTdoor.Name = "Blk_cbTdoor";
            this.Blk_cbTdoor.Size = new Size(104, 24);
            this.Blk_cbTdoor.TabIndex = 4;
            this.Blk_cbTdoor.Text = "Is a tDoor";
            this.Blk_cbTdoor.UseVisualStyleBackColor = true;
            this.Blk_cbTdoor.CheckedChanged += new EventHandler(this.Blk_cbTdoor_CheckedChanged);
            // 
            // Blk_cbDoor
            // 
            this.Blk_cbDoor.Location = new Point(10, 15);
            this.Blk_cbDoor.Name = "Blk_cbDoor";
            this.Blk_cbDoor.Size = new Size(116, 24);
            this.Blk_cbDoor.TabIndex = 3;
            this.Blk_cbDoor.Text = "Is a door";
            this.Blk_cbDoor.UseVisualStyleBackColor = true;
            this.Blk_cbDoor.CheckedChanged += new EventHandler(this.Blk_cbDoor_CheckedChanged);
            // 
            // Blk_grpBehaviour
            // 
            this.Blk_grpBehaviour.Controls.Add(this.Blk_txtDeath);
            this.Blk_grpBehaviour.Controls.Add(this.Blk_cbDeath);
            this.Blk_grpBehaviour.Controls.Add(this.Blk_cbPortal);
            this.Blk_grpBehaviour.Controls.Add(this.Blk_cbMsgBlock);
            this.Blk_grpBehaviour.Location = new Point(133, 105);
            this.Blk_grpBehaviour.Name = "Blk_grpBehaviour";
            this.Blk_grpBehaviour.Size = new Size(360, 70);
            this.Blk_grpBehaviour.TabIndex = 25;
            this.Blk_grpBehaviour.TabStop = false;
            this.Blk_grpBehaviour.Text = "Behaviour";
            // 
            // Blk_txtDeath
            // 
            this.Blk_txtDeath.BackColor = SystemColors.Window;
            this.Blk_txtDeath.Location = new Point(100, 42);
            this.Blk_txtDeath.Name = "Blk_txtDeath";
            this.Blk_txtDeath.Size = new Size(254, 21);
            this.Blk_txtDeath.TabIndex = 3;
            this.Blk_txtDeath.TextChanged += new EventHandler(this.Blk_txtDeath_TextChanged);
            // 
            // Blk_cbDeath
            // 
            this.Blk_cbDeath.Location = new Point(10, 40);
            this.Blk_cbDeath.Name = "Blk_cbDeath";
            this.Blk_cbDeath.Size = new Size(89, 24);
            this.Blk_cbDeath.TabIndex = 2;
            this.Blk_cbDeath.Text = "Kills players";
            this.Blk_cbDeath.UseVisualStyleBackColor = true;
            this.Blk_cbDeath.CheckedChanged += new EventHandler(this.Blk_cbDeath_CheckedChanged);
            // 
            // Blk_cbPortal
            // 
            this.Blk_cbPortal.Location = new Point(187, 15);
            this.Blk_cbPortal.Name = "Blk_cbPortal";
            this.Blk_cbPortal.Size = new Size(104, 24);
            this.Blk_cbPortal.TabIndex = 1;
            this.Blk_cbPortal.Text = "Is a portal";
            this.Blk_cbPortal.UseVisualStyleBackColor = true;
            this.Blk_cbPortal.CheckedChanged += new EventHandler(this.Blk_cbPortal_CheckedChanged);
            // 
            // Blk_cbMsgBlock
            // 
            this.Blk_cbMsgBlock.Location = new Point(10, 15);
            this.Blk_cbMsgBlock.Name = "Blk_cbMsgBlock";
            this.Blk_cbMsgBlock.Size = new Size(116, 24);
            this.Blk_cbMsgBlock.TabIndex = 0;
            this.Blk_cbMsgBlock.Text = "Is a message block";
            this.Blk_cbMsgBlock.UseVisualStyleBackColor = true;
            this.Blk_cbMsgBlock.CheckedChanged += new EventHandler(this.Blk_cbMsgBlock_CheckedChanged);
            // 
            // Blk_grpPermissions
            // 
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbAlw3);
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbAlw2);
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbDis3);
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbDis2);
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbAlw1);
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbDis1);
            this.Blk_grpPermissions.Controls.Add(this.Blk_cmbMin);
            this.Blk_grpPermissions.Controls.Add(this.Blk_lblMin);
            this.Blk_grpPermissions.Controls.Add(this.Blk_lblAllow);
            this.Blk_grpPermissions.Controls.Add(this.Blk_lblDisallow);
            this.Blk_grpPermissions.Location = new Point(133, 6);
            this.Blk_grpPermissions.Name = "Blk_grpPermissions";
            this.Blk_grpPermissions.Size = new Size(360, 94);
            this.Blk_grpPermissions.TabIndex = 24;
            this.Blk_grpPermissions.TabStop = false;
            this.Blk_grpPermissions.Text = "Permissions";
            // 
            // Blk_cmbAlw3
            // 
            this.Blk_cmbAlw3.BackColor = SystemColors.Window;
            this.Blk_cmbAlw3.FormattingEnabled = true;
            this.Blk_cmbAlw3.Location = new Point(274, 67);
            this.Blk_cmbAlw3.Name = "Blk_cmbAlw3";
            this.Blk_cmbAlw3.Size = new Size(81, 21);
            this.Blk_cmbAlw3.TabIndex = 28;
            this.Blk_cmbAlw3.SelectedIndexChanged += new EventHandler(this.Blk_cmbSpecific_SelectedIndexChanged);
            // 
            // Blk_cmbAlw2
            // 
            this.Blk_cmbAlw2.BackColor = SystemColors.Window;
            this.Blk_cmbAlw2.FormattingEnabled = true;
            this.Blk_cmbAlw2.Location = new Point(187, 67);
            this.Blk_cmbAlw2.Name = "Blk_cmbAlw2";
            this.Blk_cmbAlw2.Size = new Size(81, 21);
            this.Blk_cmbAlw2.TabIndex = 27;
            this.Blk_cmbAlw2.SelectedIndexChanged += new EventHandler(this.Blk_cmbSpecific_SelectedIndexChanged);
            // 
            // Blk_cmbDis3
            // 
            this.Blk_cmbDis3.BackColor = SystemColors.Window;
            this.Blk_cmbDis3.FormattingEnabled = true;
            this.Blk_cmbDis3.Location = new Point(274, 41);
            this.Blk_cmbDis3.Name = "Blk_cmbDis3";
            this.Blk_cmbDis3.Size = new Size(81, 21);
            this.Blk_cmbDis3.TabIndex = 26;
            this.Blk_cmbDis3.SelectedIndexChanged += new EventHandler(this.Blk_cmbSpecific_SelectedIndexChanged);
            // 
            // Blk_cmbDis2
            // 
            this.Blk_cmbDis2.BackColor = SystemColors.Window;
            this.Blk_cmbDis2.FormattingEnabled = true;
            this.Blk_cmbDis2.Location = new Point(187, 41);
            this.Blk_cmbDis2.Name = "Blk_cmbDis2";
            this.Blk_cmbDis2.Size = new Size(81, 21);
            this.Blk_cmbDis2.TabIndex = 25;
            this.Blk_cmbDis2.SelectedIndexChanged += new EventHandler(this.Blk_cmbSpecific_SelectedIndexChanged);
            // 
            // Blk_cmbAlw1
            // 
            this.Blk_cmbAlw1.BackColor = SystemColors.Window;
            this.Blk_cmbAlw1.FormattingEnabled = true;
            this.Blk_cmbAlw1.Location = new Point(100, 67);
            this.Blk_cmbAlw1.Name = "Blk_cmbAlw1";
            this.Blk_cmbAlw1.Size = new Size(81, 21);
            this.Blk_cmbAlw1.TabIndex = 24;
            this.Blk_cmbAlw1.SelectedIndexChanged += new EventHandler(this.Blk_cmbSpecific_SelectedIndexChanged);
            // 
            // Blk_cmbDis1
            // 
            this.Blk_cmbDis1.BackColor = SystemColors.Window;
            this.Blk_cmbDis1.FormattingEnabled = true;
            this.Blk_cmbDis1.Location = new Point(100, 41);
            this.Blk_cmbDis1.Name = "Blk_cmbDis1";
            this.Blk_cmbDis1.Size = new Size(81, 21);
            this.Blk_cmbDis1.TabIndex = 23;
            this.Blk_cmbDis1.SelectedIndexChanged += new EventHandler(this.Blk_cmbSpecific_SelectedIndexChanged);
            // 
            // Blk_cmbMin
            // 
            this.Blk_cmbMin.BackColor = SystemColors.Window;
            this.Blk_cmbMin.FormattingEnabled = true;
            this.Blk_cmbMin.Location = new Point(100, 14);
            this.Blk_cmbMin.Name = "Blk_cmbMin";
            this.Blk_cmbMin.Size = new Size(81, 21);
            this.Blk_cmbMin.TabIndex = 22;
            this.Blk_cmbMin.SelectedIndexChanged += new EventHandler(this.Blk_cmbMin_SelectedIndexChanged);
            // 
            // Blk_lblMin
            // 
            this.Blk_lblMin.AutoSize = true;
            this.Blk_lblMin.Location = new Point(10, 17);
            this.Blk_lblMin.Name = "Blk_lblMin";
            this.Blk_lblMin.Size = new Size(89, 13);
            this.Blk_lblMin.TabIndex = 16;
            this.Blk_lblMin.Text = "Min rank needed:";
            // 
            // Blk_lblAllow
            // 
            this.Blk_lblAllow.AutoSize = true;
            this.Blk_lblAllow.Location = new Point(10, 71);
            this.Blk_lblAllow.Name = "Blk_lblAllow";
            this.Blk_lblAllow.Size = new Size(91, 13);
            this.Blk_lblAllow.TabIndex = 18;
            this.Blk_lblAllow.Text = "Specifically allow:";
            // 
            // Blk_lblDisallow
            // 
            this.Blk_lblDisallow.AutoSize = true;
            this.Blk_lblDisallow.Location = new Point(10, 44);
            this.Blk_lblDisallow.Name = "Blk_lblDisallow";
            this.Blk_lblDisallow.Size = new Size(80, 13);
            this.Blk_lblDisallow.TabIndex = 17;
            this.Blk_lblDisallow.Text = "But don\'t allow:";
            // 
            // Blk_btnHelp
            // 
            this.Blk_btnHelp.Location = new Point(6, 485);
            this.Blk_btnHelp.Name = "Blk_btnHelp";
            this.Blk_btnHelp.Size = new Size(121, 29);
            this.Blk_btnHelp.TabIndex = 23;
            this.Blk_btnHelp.Text = "Help information";
            this.Blk_btnHelp.UseVisualStyleBackColor = true;
            this.Blk_btnHelp.Click += new EventHandler(this.Blk_btnHelp_Click);
            // 
            // Blk_list
            // 
            this.Blk_list.BackColor = SystemColors.Window;
            this.Blk_list.ForeColor = SystemColors.WindowText;
            this.Blk_list.FormattingEnabled = true;
            this.Blk_list.Location = new Point(6, 6);
            this.Blk_list.Name = "Blk_list";
            this.Blk_list.Size = new Size(121, 472);
            this.Blk_list.TabIndex = 15;
            this.Blk_list.SelectedIndexChanged += new EventHandler(this.Blk_list_SelectedIndexChanged);
            // 
            // PageRanks
            // 
            this.PageRanks.BackColor = SystemColors.Control;
            this.PageRanks.Controls.Add(this.Rank_grpLimits);
            this.PageRanks.Controls.Add(this.Rank_grpGeneral);
            this.PageRanks.Controls.Add(this.Rank_grpMisc);
            this.PageRanks.Controls.Add(this.Rank_btnDel);
            this.PageRanks.Controls.Add(this.Rank_btnAdd);
            this.PageRanks.Controls.Add(this.Rank_list);
            this.PageRanks.Location = new Point(4, 22);
            this.PageRanks.Name = "PageRanks";
            this.PageRanks.Padding = new Padding(3);
            this.PageRanks.Size = new Size(498, 521);
            this.PageRanks.TabIndex = 4;
            this.PageRanks.Text = "Ranks";
            // 
            // Rank_grpLimits
            // 
            this.Rank_grpLimits.Controls.Add(this.Rank_numUndo);
            this.Rank_grpLimits.Controls.Add(this.Rank_numCopy);
            this.Rank_grpLimits.Controls.Add(this.Rank_lblCopy);
            this.Rank_grpLimits.Controls.Add(this.Rank_lblGen);
            this.Rank_grpLimits.Controls.Add(this.Rank_numGen);
            this.Rank_grpLimits.Controls.Add(this.Rank_lblMaps);
            this.Rank_grpLimits.Controls.Add(this.Rank_numMaps);
            this.Rank_grpLimits.Controls.Add(this.Rank_numDraw);
            this.Rank_grpLimits.Controls.Add(this.Rank_lblDraw);
            this.Rank_grpLimits.Controls.Add(this.Rank_lblUndo);
            this.Rank_grpLimits.Location = new Point(142, 143);
            this.Rank_grpLimits.Name = "Rank_grpLimits";
            this.Rank_grpLimits.Size = new Size(349, 106);
            this.Rank_grpLimits.TabIndex = 22;
            this.Rank_grpLimits.TabStop = false;
            this.Rank_grpLimits.Text = "Rank limits";
            // 
            // Rank_lblCopy
            // 
            this.Rank_lblCopy.AutoSize = true;
            this.Rank_lblCopy.Location = new Point(18, 77);
            this.Rank_lblCopy.Name = "Rank_lblCopy";
            this.Rank_lblCopy.Size = new Size(61, 13);
            this.Rank_lblCopy.TabIndex = 22;
            this.Rank_lblCopy.Text = "/copy slots:";
            this.Rank_lblCopy.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblGen
            // 
            this.Rank_lblGen.AutoSize = true;
            this.Rank_lblGen.Location = new Point(185, 50);
            this.Rank_lblGen.Name = "Rank_lblGen";
            this.Rank_lblGen.Size = new Size(68, 13);
            this.Rank_lblGen.TabIndex = 20;
            this.Rank_lblGen.Text = "/gen volume:";
            this.Rank_lblGen.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblMaps
            // 
            this.Rank_lblMaps.AutoSize = true;
            this.Rank_lblMaps.Location = new Point(200, 23);
            this.Rank_lblMaps.Name = "Rank_lblMaps";
            this.Rank_lblMaps.Size = new Size(53, 13);
            this.Rank_lblMaps.TabIndex = 18;
            this.Rank_lblMaps.Text = "/os maps:";
            this.Rank_lblMaps.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblDraw
            // 
            this.Rank_lblDraw.AutoSize = true;
            this.Rank_lblDraw.Location = new Point(20, 23);
            this.Rank_lblDraw.Name = "Rank_lblDraw";
            this.Rank_lblDraw.Size = new Size(59, 13);
            this.Rank_lblDraw.TabIndex = 3;
            this.Rank_lblDraw.Text = "Draw limit:";
            this.Rank_lblDraw.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblUndo
            // 
            this.Rank_lblUndo.AutoSize = true;
            this.Rank_lblUndo.Location = new Point(19, 50);
            this.Rank_lblUndo.Name = "Rank_lblUndo";
            this.Rank_lblUndo.Size = new Size(60, 13);
            this.Rank_lblUndo.TabIndex = 14;
            this.Rank_lblUndo.Text = "Max /undo:";
            this.Rank_lblUndo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_grpGeneral
            // 
            this.Rank_grpGeneral.Controls.Add(this.Rank_lblOsMap);
            this.Rank_grpGeneral.Controls.Add(this.Rank_cmbOsMap);
            this.Rank_grpGeneral.Controls.Add(this.Rank_cbEmpty);
            this.Rank_grpGeneral.Controls.Add(this.Rank_lblDefault);
            this.Rank_grpGeneral.Controls.Add(this.Rank_cmbDefault);
            this.Rank_grpGeneral.Controls.Add(this.Rank_cbSilentAdmins);
            this.Rank_grpGeneral.Controls.Add(this.Rank_cbTPHigher);
            this.Rank_grpGeneral.Location = new Point(142, 255);
            this.Rank_grpGeneral.Name = "Rank_grpGeneral";
            this.Rank_grpGeneral.Size = new Size(349, 121);
            this.Rank_grpGeneral.TabIndex = 19;
            this.Rank_grpGeneral.TabStop = false;
            this.Rank_grpGeneral.Text = "General settings";
            // 
            // Rank_lblOsMap
            // 
            this.Rank_lblOsMap.AutoSize = true;
            this.Rank_lblOsMap.Location = new Point(186, 23);
            this.Rank_lblOsMap.Name = "Rank_lblOsMap";
            this.Rank_lblOsMap.Size = new Size(67, 13);
            this.Rank_lblOsMap.TabIndex = 50;
            this.Rank_lblOsMap.Text = "/os perbuild:";
            // 
            // Rank_cbEmpty
            // 
            this.Rank_cbEmpty.AutoSize = true;
            this.Rank_cbEmpty.Location = new Point(11, 98);
            this.Rank_cbEmpty.Name = "Rank_cbEmpty";
            this.Rank_cbEmpty.Size = new Size(163, 17);
            this.Rank_cbEmpty.TabIndex = 45;
            this.Rank_cbEmpty.Text = "Show empty ranks in /players";
            this.Rank_cbEmpty.UseVisualStyleBackColor = true;
            // 
            // Rank_lblDefault
            // 
            this.Rank_lblDefault.AutoSize = true;
            this.Rank_lblDefault.Location = new Point(11, 23);
            this.Rank_lblDefault.Name = "Rank_lblDefault";
            this.Rank_lblDefault.Size = new Size(68, 13);
            this.Rank_lblDefault.TabIndex = 43;
            this.Rank_lblDefault.Text = "Default rank:";
            // 
            // Rank_grpMisc
            // 
            this.Rank_grpMisc.Controls.Add(this.Rank_numAfk);
            this.Rank_grpMisc.Controls.Add(this.Rank_cbAfk);
            this.Rank_grpMisc.Controls.Add(this.Rank_lblPrefix);
            this.Rank_grpMisc.Controls.Add(this.Rank_txtPrefix);
            this.Rank_grpMisc.Controls.Add(this.Rank_lblPerm);
            this.Rank_grpMisc.Controls.Add(this.Rank_txtMOTD);
            this.Rank_grpMisc.Controls.Add(this.Rank_numPerm);
            this.Rank_grpMisc.Controls.Add(this.Rank_txtName);
            this.Rank_grpMisc.Controls.Add(this.Rank_btnColor);
            this.Rank_grpMisc.Controls.Add(this.Rank_lblMOTD);
            this.Rank_grpMisc.Controls.Add(this.Rank_lblName);
            this.Rank_grpMisc.Controls.Add(this.Rank_lblColor);
            this.Rank_grpMisc.Location = new Point(142, 6);
            this.Rank_grpMisc.Name = "Rank_grpMisc";
            this.Rank_grpMisc.Size = new Size(349, 131);
            this.Rank_grpMisc.TabIndex = 18;
            this.Rank_grpMisc.TabStop = false;
            this.Rank_grpMisc.Text = "Rank settings";
            // 
            // Rank_cbAfk
            // 
            this.Rank_cbAfk.Location = new Point(11, 102);
            this.Rank_cbAfk.Name = "Rank_cbAfk";
            this.Rank_cbAfk.Size = new Size(108, 24);
            this.Rank_cbAfk.TabIndex = 22;
            this.Rank_cbAfk.Text = "Kick after AFK for";
            this.Rank_cbAfk.UseVisualStyleBackColor = true;
            this.Rank_cbAfk.CheckedChanged += new EventHandler(this.Rank_cbAfk_CheckedChanged);
            // 
            // Rank_lblPrefix
            // 
            this.Rank_lblPrefix.AutoSize = true;
            this.Rank_lblPrefix.Location = new Point(216, 50);
            this.Rank_lblPrefix.Name = "Rank_lblPrefix";
            this.Rank_lblPrefix.Size = new Size(37, 13);
            this.Rank_lblPrefix.TabIndex = 20;
            this.Rank_lblPrefix.Text = "Prefix:";
            this.Rank_lblPrefix.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblPerm
            // 
            this.Rank_lblPerm.AutoSize = true;
            this.Rank_lblPerm.Location = new Point(190, 23);
            this.Rank_lblPerm.Name = "Rank_lblPerm";
            this.Rank_lblPerm.Size = new Size(63, 13);
            this.Rank_lblPerm.TabIndex = 7;
            this.Rank_lblPerm.Text = "Permission:";
            this.Rank_lblPerm.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblMOTD
            // 
            this.Rank_lblMOTD.AutoSize = true;
            this.Rank_lblMOTD.Location = new Point(41, 78);
            this.Rank_lblMOTD.Name = "Rank_lblMOTD";
            this.Rank_lblMOTD.Size = new Size(38, 13);
            this.Rank_lblMOTD.TabIndex = 16;
            this.Rank_lblMOTD.Text = "MOTD:";
            // 
            // Rank_lblName
            // 
            this.Rank_lblName.AutoSize = true;
            this.Rank_lblName.Location = new Point(41, 23);
            this.Rank_lblName.Name = "Rank_lblName";
            this.Rank_lblName.Size = new Size(38, 13);
            this.Rank_lblName.TabIndex = 4;
            this.Rank_lblName.Text = "Name:";
            this.Rank_lblName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Rank_lblColor
            // 
            this.Rank_lblColor.AutoSize = true;
            this.Rank_lblColor.Location = new Point(44, 50);
            this.Rank_lblColor.Name = "Rank_lblColor";
            this.Rank_lblColor.Size = new Size(35, 13);
            this.Rank_lblColor.TabIndex = 11;
            this.Rank_lblColor.Text = "Color:";
            // 
            // Rank_btnDel
            // 
            this.Rank_btnDel.Location = new Point(79, 353);
            this.Rank_btnDel.Name = "Rank_btnDel";
            this.Rank_btnDel.Size = new Size(57, 23);
            this.Rank_btnDel.TabIndex = 2;
            this.Rank_btnDel.Text = "Delete";
            this.Rank_btnDel.UseVisualStyleBackColor = true;
            this.Rank_btnDel.Click += new EventHandler(this.Rank_btnDel_Click);
            // 
            // Rank_btnAdd
            // 
            this.Rank_btnAdd.Location = new Point(6, 353);
            this.Rank_btnAdd.Name = "Rank_btnAdd";
            this.Rank_btnAdd.Size = new Size(57, 23);
            this.Rank_btnAdd.TabIndex = 1;
            this.Rank_btnAdd.Text = "Add";
            this.Rank_btnAdd.UseVisualStyleBackColor = true;
            this.Rank_btnAdd.Click += new EventHandler(this.Rank_btnAdd_Click);
            // 
            // Rank_list
            // 
            this.Rank_list.BackColor = SystemColors.Window;
            this.Rank_list.ForeColor = SystemColors.WindowText;
            this.Rank_list.FormattingEnabled = true;
            this.Rank_list.Location = new Point(6, 6);
            this.Rank_list.Name = "Rank_list";
            this.Rank_list.Size = new Size(130, 342);
            this.Rank_list.TabIndex = 0;
            this.Rank_list.SelectedIndexChanged += new EventHandler(this.Rank_list_SelectedIndexChanged);
            // 
            // PageMisc
            // 
            this.PageMisc.BackColor = SystemColors.Control;
            this.PageMisc.Controls.Add(this.GrpExtra);
            this.PageMisc.Controls.Add(this.GrpMessages);
            this.PageMisc.Controls.Add(this.GrpPhysics);
            this.PageMisc.Controls.Add(this.Afk_grp);
            this.PageMisc.Controls.Add(this.Bak_grp);
            this.PageMisc.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.PageMisc.Location = new Point(4, 22);
            this.PageMisc.Name = "PageMisc";
            this.PageMisc.Size = new Size(498, 521);
            this.PageMisc.TabIndex = 3;
            this.PageMisc.Text = "Misc";
            // 
            // GrpExtra
            // 
            this.GrpExtra.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.GrpExtra.Controls.Add(this.Misc_numReview);
            this.GrpExtra.Controls.Add(this.ChkRestart);
            this.GrpExtra.Controls.Add(this.Misc_lblReview);
            this.GrpExtra.Controls.Add(this.ChkGuestLimitNotify);
            this.GrpExtra.Controls.Add(this.ChkRepeatMessages);
            this.GrpExtra.Controls.Add(this.ChkDeath);
            this.GrpExtra.Controls.Add(this.Chk17Dollar);
            this.GrpExtra.Controls.Add(this.ChkSmile);
            this.GrpExtra.Location = new Point(10, 158);
            this.GrpExtra.Name = "GrpExtra";
            this.GrpExtra.Size = new Size(332, 124);
            this.GrpExtra.TabIndex = 40;
            this.GrpExtra.TabStop = false;
            this.GrpExtra.Text = "Extra";
            // 
            // Misc_numReview
            // 
            this.Misc_numReview.BackColor = SystemColors.Window;
            this.Misc_numReview.Location = new Point(126, 89);
            this.Misc_numReview.Name = "Misc_numReview";
            this.Misc_numReview.Seconds = ((long)(300));
            this.Misc_numReview.Size = new Size(66, 21);
            this.Misc_numReview.TabIndex = 35;
            this.Misc_numReview.Text = "5m";
            this.Misc_numReview.Value = TimeSpan.Parse("00:05:00");
            // 
            // ChkRestart
            // 
            this.ChkRestart.AutoSize = true;
            this.ChkRestart.Location = new Point(190, 20);
            this.ChkRestart.Name = "ChkRestart";
            this.ChkRestart.Size = new Size(101, 17);
            this.ChkRestart.TabIndex = 51;
            this.ChkRestart.Text = "Restart on error";
            this.ChkRestart.UseVisualStyleBackColor = true;
            // 
            // Misc_lblReview
            // 
            this.Misc_lblReview.AutoSize = true;
            this.Misc_lblReview.Location = new Point(6, 91);
            this.Misc_lblReview.Name = "Misc_lblReview";
            this.Misc_lblReview.Size = new Size(115, 13);
            this.Misc_lblReview.TabIndex = 49;
            this.Misc_lblReview.Text = "Review cooldown time:";
            // 
            // ChkRepeatMessages
            // 
            this.ChkRepeatMessages.AutoSize = true;
            this.ChkRepeatMessages.Location = new Point(190, 43);
            this.ChkRepeatMessages.Name = "ChkRepeatMessages";
            this.ChkRepeatMessages.Size = new Size(136, 17);
            this.ChkRepeatMessages.TabIndex = 29;
            this.ChkRepeatMessages.Text = "Repeat message blocks";
            this.ChkRepeatMessages.UseVisualStyleBackColor = true;
            // 
            // Chk17Dollar
            // 
            this.Chk17Dollar.AutoSize = true;
            this.Chk17Dollar.Location = new Point(6, 66);
            this.Chk17Dollar.Name = "Chk17Dollar";
            this.Chk17Dollar.Size = new Size(100, 17);
            this.Chk17Dollar.TabIndex = 22;
            this.Chk17Dollar.Text = "$ before $name";
            this.Chk17Dollar.UseVisualStyleBackColor = true;
            // 
            // ChkSmile
            // 
            this.ChkSmile.AutoSize = true;
            this.ChkSmile.Location = new Point(6, 43);
            this.ChkSmile.Name = "ChkSmile";
            this.ChkSmile.Size = new Size(91, 17);
            this.ChkSmile.TabIndex = 19;
            this.ChkSmile.Text = "Parse emotes";
            this.ChkSmile.UseVisualStyleBackColor = true;
            // 
            // GrpMessages
            // 
            this.GrpMessages.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.GrpMessages.Controls.Add(this.Hack_num);
            this.GrpMessages.Controls.Add(this.Hack_lbl);
            this.GrpMessages.Location = new Point(10, 103);
            this.GrpMessages.Name = "GrpMessages";
            this.GrpMessages.Size = new Size(332, 49);
            this.GrpMessages.TabIndex = 39;
            this.GrpMessages.TabStop = false;
            this.GrpMessages.Text = "Messages";
            // 
            // Hack_num
            // 
            this.Hack_num.BackColor = SystemColors.Window;
            this.Hack_num.Location = new Point(201, 18);
            this.Hack_num.Name = "Hack_num";
            this.Hack_num.Seconds = ((long)(5));
            this.Hack_num.Size = new Size(66, 21);
            this.Hack_num.TabIndex = 34;
            this.Hack_num.Text = "5s";
            this.Hack_num.Value = TimeSpan.Parse("00:00:05");
            // 
            // GrpPhysics
            // 
            this.GrpPhysics.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.GrpPhysics.Controls.Add(this.ChkPhysRestart);
            this.GrpPhysics.Controls.Add(this.ChkRpLimit);
            this.GrpPhysics.Controls.Add(this.TxtRP);
            this.GrpPhysics.Controls.Add(this.ChkRpNorm);
            this.GrpPhysics.Controls.Add(this.TxtNormRp);
            this.GrpPhysics.Location = new Point(352, 124);
            this.GrpPhysics.Name = "GrpPhysics";
            this.GrpPhysics.Size = new Size(133, 117);
            this.GrpPhysics.TabIndex = 38;
            this.GrpPhysics.TabStop = false;
            this.GrpPhysics.Text = "Physics Restart";
            // 
            // TxtRP
            // 
            this.TxtRP.BackColor = SystemColors.Window;
            this.TxtRP.Location = new Point(72, 49);
            this.TxtRP.Name = "TxtRP";
            this.TxtRP.Size = new Size(55, 21);
            this.TxtRP.TabIndex = 14;
            // 
            // ChkRpNorm
            // 
            this.ChkRpNorm.AutoSize = true;
            this.ChkRpNorm.Location = new Point(5, 79);
            this.ChkRpNorm.Name = "ChkRpNorm";
            this.ChkRpNorm.Size = new Size(61, 13);
            this.ChkRpNorm.TabIndex = 16;
            this.ChkRpNorm.Text = "Normal /rp:";
            // 
            // TxtNormRp
            // 
            this.TxtNormRp.BackColor = SystemColors.Window;
            this.TxtNormRp.Location = new Point(72, 76);
            this.TxtNormRp.Name = "TxtNormRp";
            this.TxtNormRp.Size = new Size(55, 21);
            this.TxtNormRp.TabIndex = 13;
            // 
            // Afk_grp
            // 
            this.Afk_grp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Afk_grp.Controls.Add(this.Afk_numTimer);
            this.Afk_grp.Controls.Add(this.Afk_lblTimer);
            this.Afk_grp.Location = new Point(352, 13);
            this.Afk_grp.Name = "Afk_grp";
            this.Afk_grp.Size = new Size(133, 100);
            this.Afk_grp.TabIndex = 37;
            this.Afk_grp.TabStop = false;
            this.Afk_grp.Text = "AFK";
            // 
            // Afk_numTimer
            // 
            this.Afk_numTimer.BackColor = SystemColors.Window;
            this.Afk_numTimer.Location = new Point(61, 16);
            this.Afk_numTimer.Name = "Afk_numTimer";
            this.Afk_numTimer.Seconds = ((long)(600));
            this.Afk_numTimer.Size = new Size(66, 21);
            this.Afk_numTimer.TabIndex = 33;
            this.Afk_numTimer.Text = "10m";
            this.Afk_numTimer.Value = TimeSpan.Parse("00:10:00");
            // 
            // Afk_lblTimer
            // 
            this.Afk_lblTimer.AutoSize = true;
            this.Afk_lblTimer.Location = new Point(5, 20);
            this.Afk_lblTimer.Name = "Afk_lblTimer";
            this.Afk_lblTimer.Size = new Size(54, 13);
            this.Afk_lblTimer.TabIndex = 12;
            this.Afk_lblTimer.Text = "AFK timer:";
            // 
            // Bak_grp
            // 
            this.Bak_grp.AutoSize = true;
            this.Bak_grp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Bak_grp.Controls.Add(this.Bak_numTime);
            this.Bak_grp.Controls.Add(this.Bak_lblLocation);
            this.Bak_grp.Controls.Add(this.Bak_txtLocation);
            this.Bak_grp.Controls.Add(this.Bak_lblTime);
            this.Bak_grp.Location = new Point(10, 13);
            this.Bak_grp.Name = "Bak_grp";
            this.Bak_grp.Size = new Size(332, 84);
            this.Bak_grp.TabIndex = 36;
            this.Bak_grp.TabStop = false;
            this.Bak_grp.Text = "Backups";
            // 
            // Bak_numTime
            // 
            this.Bak_numTime.BackColor = SystemColors.Window;
            this.Bak_numTime.Location = new Point(81, 43);
            this.Bak_numTime.Name = "Bak_numTime";
            this.Bak_numTime.Seconds = ((long)(300));
            this.Bak_numTime.Size = new Size(66, 21);
            this.Bak_numTime.TabIndex = 34;
            this.Bak_numTime.Text = "5m";
            this.Bak_numTime.Value = TimeSpan.Parse("00:05:00");
            // 
            // Bak_lblLocation
            // 
            this.Bak_lblLocation.AutoSize = true;
            this.Bak_lblLocation.Location = new Point(5, 21);
            this.Bak_lblLocation.Name = "Bak_lblLocation";
            this.Bak_lblLocation.Size = new Size(44, 13);
            this.Bak_lblLocation.TabIndex = 3;
            this.Bak_lblLocation.Text = "Location:";
            // 
            // Bak_txtLocation
            // 
            this.Bak_txtLocation.BackColor = SystemColors.Window;
            this.Bak_txtLocation.Location = new Point(81, 17);
            this.Bak_txtLocation.Name = "Bak_txtLocation";
            this.Bak_txtLocation.Size = new Size(245, 21);
            this.Bak_txtLocation.TabIndex = 2;
            // 
            // Bak_lblTime
            // 
            this.Bak_lblTime.AutoSize = true;
            this.Bak_lblTime.Location = new Point(5, 47);
            this.Bak_lblTime.Name = "Bak_lblTime";
            this.Bak_lblTime.Size = new Size(67, 13);
            this.Bak_lblTime.TabIndex = 7;
            this.Bak_lblTime.Text = "Backup time:";
            // 
            // PageRelay
            // 
            this.PageRelay.BackColor = SystemColors.Control;
            this.PageRelay.Controls.Add(this.Dis_grp);
            this.PageRelay.Controls.Add(this.Gb_ircSettings);
            this.PageMisc.Controls.Add(this.Sql_grp);
            this.PageRelay.Controls.Add(this.Irc_grp);
            this.PageRelay.Location = new Point(4, 22);
            this.PageRelay.Name = "PageRelay";
            this.PageRelay.Size = new Size(498, 521);
            this.PageRelay.TabIndex = 6;
            this.PageRelay.Text = "IRC";
            // 
            // Gb_ircSettings
            // 
            this.Gb_ircSettings.Controls.Add(this.Irc_txtPrefix);
            this.Gb_ircSettings.Controls.Add(this.Irc_lblPrefix);
            this.Gb_ircSettings.Controls.Add(this.Irc_cmbVerify);
            this.Gb_ircSettings.Controls.Add(this.Irc_lblVerify);
            this.Gb_ircSettings.Controls.Add(this.Irc_cmbRank);
            this.Gb_ircSettings.Controls.Add(this.Irc_lblRank);
            this.Gb_ircSettings.Controls.Add(this.Irc_cbAFK);
            this.Gb_ircSettings.Controls.Add(this.Irc_cbWorldChanges);
            this.Gb_ircSettings.Controls.Add(this.Irc_cbTitles);
            this.Gb_ircSettings.Location = new Point(8, 223);
            this.Gb_ircSettings.Name = "Gb_ircSettings";
            this.Gb_ircSettings.Size = new Size(483, 95);
            this.Gb_ircSettings.TabIndex = 33;
            this.Gb_ircSettings.TabStop = false;
            this.Gb_ircSettings.Text = "IRC settings";
            // 
            // Irc_txtPrefix
            // 
            this.Irc_txtPrefix.BackColor = SystemColors.Window;
            this.Irc_txtPrefix.Location = new Point(367, 68);
            this.Irc_txtPrefix.Name = "Irc_txtPrefix";
            this.Irc_txtPrefix.Size = new Size(100, 21);
            this.Irc_txtPrefix.TabIndex = 32;
            // 
            // Irc_lblPrefix
            // 
            this.Irc_lblPrefix.AutoSize = true;
            this.Irc_lblPrefix.Location = new Point(265, 70);
            this.Irc_lblPrefix.Name = "Irc_lblPrefix";
            this.Irc_lblPrefix.Size = new Size(87, 13);
            this.Irc_lblPrefix.TabIndex = 39;
            this.Irc_lblPrefix.Text = "Command prefix:";
            // 
            // Irc_cmbVerify
            // 
            this.Irc_cmbVerify.BackColor = SystemColors.Window;
            this.Irc_cmbVerify.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Irc_cmbVerify.FormattingEnabled = true;
            this.Irc_cmbVerify.Location = new Point(387, 42);
            this.Irc_cmbVerify.Name = "Irc_cmbVerify";
            this.Irc_cmbVerify.Size = new Size(80, 21);
            this.Irc_cmbVerify.TabIndex = 38;
            // 
            // Irc_lblVerify
            // 
            this.Irc_lblVerify.AutoSize = true;
            this.Irc_lblVerify.Location = new Point(284, 45);
            this.Irc_lblVerify.Name = "Irc_lblVerify";
            this.Irc_lblVerify.Size = new Size(99, 13);
            this.Irc_lblVerify.TabIndex = 37;
            this.Irc_lblVerify.Text = "Verification method:";
            // 
            // Irc_cmbRank
            // 
            this.Irc_cmbRank.BackColor = SystemColors.Window;
            this.Irc_cmbRank.FormattingEnabled = true;
            this.Irc_cmbRank.Location = new Point(367, 17);
            this.Irc_cmbRank.Name = "Irc_cmbRank";
            this.Irc_cmbRank.Size = new Size(100, 21);
            this.Irc_cmbRank.TabIndex = 36;
            // 
            // Irc_lblRank
            // 
            this.Irc_lblRank.AutoSize = true;
            this.Irc_lblRank.Location = new Point(265, 21);
            this.Irc_lblRank.Name = "Irc_lblRank";
            this.Irc_lblRank.Size = new Size(97, 13);
            this.Irc_lblRank.TabIndex = 35;
            this.Irc_lblRank.Text = "IRC controller rank:";
            // 
            // Irc_cbAFK
            // 
            this.Irc_cbAFK.AutoSize = true;
            this.Irc_cbAFK.Location = new Point(6, 70);
            this.Irc_cbAFK.Name = "Irc_cbAFK";
            this.Irc_cbAFK.Size = new Size(176, 17);
            this.Irc_cbAFK.TabIndex = 34;
            this.Irc_cbAFK.Text = "Announce when player goes AFK";
            this.Irc_cbAFK.UseVisualStyleBackColor = true;
            // 
            // Irc_cbWorldChanges
            // 
            this.Irc_cbWorldChanges.AutoSize = true;
            this.Irc_cbWorldChanges.Location = new Point(6, 45);
            this.Irc_cbWorldChanges.Name = "Irc_cbWorldChanges";
            this.Irc_cbWorldChanges.Size = new Size(199, 17);
            this.Irc_cbWorldChanges.TabIndex = 33;
            this.Irc_cbWorldChanges.Text = "Announce when player changes level";
            this.Irc_cbWorldChanges.UseVisualStyleBackColor = true;
            // 
            // Irc_cbTitles
            // 
            this.Irc_cbTitles.AutoSize = true;
            this.Irc_cbTitles.Location = new Point(6, 20);
            this.Irc_cbTitles.Name = "Irc_cbTitles";
            this.Irc_cbTitles.Size = new Size(171, 17);
            this.Irc_cbTitles.TabIndex = 32;
            this.Irc_cbTitles.Text = "Show player\'s title in messages";
            this.Irc_cbTitles.UseVisualStyleBackColor = true;
            // 
            // Dis_grp
            // 
            this.Dis_grp.Controls.Add(this.Dis_linkHelp);
            this.Dis_grp.Controls.Add(this.Dis_chkEnabled);
            this.Dis_grp.Controls.Add(this.Dis_lblToken);
            this.Dis_grp.Controls.Add(this.Dis_txtToken);
            this.Dis_grp.Controls.Add(this.Dis_lblChannel);
            this.Dis_grp.Controls.Add(this.Dis_txtChannel);
            this.Dis_grp.Controls.Add(this.Dis_lblOpChannel);
            this.Dis_grp.Controls.Add(this.Dis_txtOpChannel);
            this.Dis_grp.Controls.Add(this.Dis_chkNicks);
            this.Dis_grp.Location = new Point(241, 3);
            this.Dis_grp.Name = "Dis_grp";
            this.Dis_grp.Size = new Size(250, 158);
            this.Dis_grp.TabIndex = 34;
            this.Dis_grp.TabStop = false;
            this.Dis_grp.Text = "Discord";
            // 
            // Dis_lblToken
            // 
            this.Dis_lblToken.AutoSize = true;
            this.Dis_lblToken.Location = new Point(6, 50);
            this.Dis_lblToken.Name = "Dis_lblToken";
            this.Dis_lblToken.Size = new Size(55, 13);
            this.Dis_lblToken.TabIndex = 19;
            this.Dis_lblToken.Text = "Bot token:";
            // 
            // Dis_lblChannel
            // 
            this.Dis_lblChannel.AutoSize = true;
            this.Dis_lblChannel.Location = new Point(6, 77);
            this.Dis_lblChannel.Name = "Dis_lblChannel";
            this.Dis_lblChannel.Size = new Size(61, 13);
            this.Dis_lblChannel.TabIndex = 30;
            this.Dis_lblChannel.Text = "Channel ID:";
            // 
            // Dis_txtChannel
            // 
            this.Dis_txtChannel.BackColor = SystemColors.Window;
            this.Dis_txtChannel.Location = new Point(82, 74);
            this.Dis_txtChannel.Name = "Dis_txtChannel";
            this.Dis_txtChannel.Size = new Size(152, 21);
            this.Dis_txtChannel.TabIndex = 31;
            this.GUIToolTip.SetToolTip(this.Dis_txtOpChannel, "The ID of the channel that chat is sent to and read from.\n\n" +
                                    "To get the ID of a channel on Discord, right click it and then click Copy ID on the dropdown menu");
            // 
            // Dis_lblOpChannel
            // 
            this.Dis_lblOpChannel.AutoSize = true;
            this.Dis_lblOpChannel.Location = new Point(6, 104);
            this.Dis_lblOpChannel.Name = "Dis_lblOpChannel";
            this.Dis_lblOpChannel.Size = new Size(67, 13);
            this.Dis_lblOpChannel.TabIndex = 20;
            this.Dis_lblOpChannel.Text = "OpChannel ID:";
            // 
            // Dis_chkEnabled
            // 
            this.Dis_chkEnabled.AutoSize = true;
            this.Dis_chkEnabled.Location = new Point(9, 20);
            this.Dis_chkEnabled.Name = "Dis_chkEnabled";
            this.Dis_chkEnabled.Size = new Size(96, 17);
            this.Dis_chkEnabled.TabIndex = 22;
            this.Dis_chkEnabled.Text = "Enable Discord integration";
            this.GUIToolTip.SetToolTip(this.Dis_chkEnabled, "Enables sending chat to and reading chat from Discord channel(s) using a bot account");
            this.Dis_chkEnabled.UseVisualStyleBackColor = true;
            this.Dis_chkEnabled.CheckedChanged += new EventHandler(this.Dis_chkEnabled_CheckedChanged);
            // 
            // Dis_txtToken
            // 
            this.Dis_txtToken.BackColor = SystemColors.Window;
            this.Dis_txtToken.Location = new Point(82, 47);
            this.Dis_txtToken.Name = "Dis_txtToken";
            this.Dis_txtToken.PasswordChar = '*';
            this.Dis_txtToken.Size = new Size(152, 21);
            this.Dis_txtToken.TabIndex = 15;
            this.GUIToolTip.SetToolTip(this.Dis_txtToken, "The token for the bot account. To find this token:\n" +
                                    "Go to Developer portal -> go to the bot application -> Settings -> Bot -> click Copy under TOKEN\n\n" +
                                    "Note: This token allows full access to the bot - NEVER SHARE THIS TOKEN WITH ANYONE ELSE");
            // 
            // Dis_txtOpChannel
            // 
            this.Dis_txtOpChannel.BackColor = SystemColors.Window;
            this.Dis_txtOpChannel.Location = new Point(82, 101);
            this.Dis_txtOpChannel.Name = "Dis_txtOpChannel";
            this.Dis_txtOpChannel.Size = new Size(152, 21);
            this.Dis_txtOpChannel.TabIndex = 16;
            this.GUIToolTip.SetToolTip(this.Dis_txtOpChannel, "The ID of the channel that staff only chat is sent to and read from. Can be left blank.\n\n" +
                                    "To get the ID of a channel on Discord, right click it and then click Copy ID on the dropdown menu");
            // 
            // Dis_chkNicks
            // 
            this.Dis_chkNicks.AutoSize = true;
            this.Dis_chkNicks.Location = new Point(9, 131);
            this.Dis_chkNicks.Name = "Dis_chkNicks";
            this.Dis_chkNicks.Size = new Size(171, 17);
            this.Dis_chkNicks.TabIndex = 32;
            this.Dis_chkNicks.Text = "Prefer nicknames to usernames";
            this.Dis_chkNicks.UseVisualStyleBackColor = true;
            // 
            // Dis_linkHelp
            // 
            this.Dis_linkHelp.AutoSize = true;
            this.Dis_linkHelp.Location = new Point(207, 21);
            this.Dis_linkHelp.Name = "Dis_linkHelp";
            this.Dis_linkHelp.Size = new Size(28, 13);
            this.Dis_linkHelp.TabIndex = 33;
            this.Dis_linkHelp.TabStop = true;
            this.Dis_linkHelp.Text = "Help";
            this.Dis_linkHelp.LinkClicked += new LinkLabelLinkClickedEventHandler(this.Dis_lnkHelp_LinkClicked);
            // 
            // Sql_grp
            // 
            this.Sql_grp.Controls.Add(this.Sql_chkUseSQL);
            this.Sql_grp.Controls.Add(this.Sql_linkDownload);
            this.Sql_grp.Controls.Add(this.Sql_lblUser);
            this.Sql_grp.Controls.Add(this.Sql_txtUser);
            this.Sql_grp.Controls.Add(this.Sql_lblPass);
            this.Sql_grp.Controls.Add(this.Sql_txtPass);
            this.Sql_grp.Controls.Add(this.Sql_lblDBName);
            this.Sql_grp.Controls.Add(this.Sql_txtDBName);
            this.Sql_grp.Controls.Add(this.Sql_lblHost);
            this.Sql_grp.Controls.Add(this.Sql_txtHost);
            this.Sql_grp.Controls.Add(this.Sql_lblPort);
            this.Sql_grp.Controls.Add(this.Sql_txtPort);
            this.Sql_grp.Location = new Point(10, 288);
            this.Sql_grp.Name = "Sql_grp";
            this.Sql_grp.Size = new Size(227, 214);
            this.Sql_grp.TabIndex = 29;
            this.Sql_grp.TabStop = false;
            this.Sql_grp.Text = "MySQL";
            // 
            // Sql_linkDownload
            // 
            this.Sql_linkDownload.AutoSize = true;
            this.Sql_linkDownload.Location = new Point(108, 21);
            this.Sql_linkDownload.Name = "Sql_linkDownload";
            this.Sql_linkDownload.Size = new Size(113, 13);
            this.Sql_linkDownload.TabIndex = 30;
            this.Sql_linkDownload.TabStop = true;
            this.Sql_linkDownload.Tag = "Click here to go to the download page for MySQL.";
            this.Sql_linkDownload.Text = "MySQL Download Page";
            this.Sql_linkDownload.LinkClicked += new LinkLabelLinkClickedEventHandler(this.Sql_linkDownload_LinkClicked);
            // 
            // Sql_lblUser
            // 
            this.Sql_lblUser.AutoSize = true;
            this.Sql_lblUser.Location = new Point(9, 50);
            this.Sql_lblUser.Name = "Sql_lblUser";
            this.Sql_lblUser.Size = new Size(59, 13);
            this.Sql_lblUser.TabIndex = 3;
            this.Sql_lblUser.Text = "Username:";
            // 
            // Sql_txtUser
            // 
            this.Sql_txtUser.BackColor = SystemColors.Window;
            this.Sql_txtUser.Location = new Point(111, 47);
            this.Sql_txtUser.Name = "Sql_txtUser";
            this.Sql_txtUser.Size = new Size(100, 21);
            this.Sql_txtUser.TabIndex = 1;
            this.Sql_txtUser.Tag = "The username set while installing MySQL";
            // 
            // Sql_lblPass
            // 
            this.Sql_lblPass.AutoSize = true;
            this.Sql_lblPass.Location = new Point(9, 77);
            this.Sql_lblPass.Name = "Sql_lblPass";
            this.Sql_lblPass.Size = new Size(56, 13);
            this.Sql_lblPass.TabIndex = 4;
            this.Sql_lblPass.Text = "Password:";
            // 
            // Sql_txtPass
            // 
            this.Sql_txtPass.BackColor = SystemColors.Window;
            this.Sql_txtPass.Location = new Point(111, 74);
            this.Sql_txtPass.Name = "Sql_txtPass";
            this.Sql_txtPass.PasswordChar = '*';
            this.Sql_txtPass.Size = new Size(100, 21);
            this.Sql_txtPass.TabIndex = 2;
            this.Sql_txtPass.Tag = "The password set while installing MySQL";
            // 
            // Sql_lblDBName
            // 
            this.Sql_lblDBName.AutoSize = true;
            this.Sql_lblDBName.Location = new Point(9, 104);
            this.Sql_lblDBName.Name = "Sql_lblDBName";
            this.Sql_lblDBName.Size = new Size(86, 13);
            this.Sql_lblDBName.TabIndex = 5;
            this.Sql_lblDBName.Text = "Database Name:";
            // 
            // Sql_txtDBName
            // 
            this.Sql_txtDBName.BackColor = SystemColors.Window;
            this.Sql_txtDBName.Location = new Point(111, 101);
            this.Sql_txtDBName.Name = "Sql_txtDBName";
            this.Sql_txtDBName.Size = new Size(100, 21);
            this.Sql_txtDBName.TabIndex = 6;
            this.Sql_txtDBName.Tag = "The name of the database stored (Default = MCZall)";
            // 
            // Sql_lblHost
            // 
            this.Sql_lblHost.AutoSize = true;
            this.Sql_lblHost.Location = new Point(9, 131);
            this.Sql_lblHost.Name = "Sql_lblHost";
            this.Sql_lblHost.Size = new Size(32, 13);
            this.Sql_lblHost.TabIndex = 7;
            this.Sql_lblHost.Text = "Host:";
            // 
            // Sql_txtHost
            // 
            this.Sql_txtHost.BackColor = SystemColors.Window;
            this.Sql_txtHost.Location = new Point(111, 128);
            this.Sql_txtHost.Name = "Sql_txtHost";
            this.Sql_txtHost.Size = new Size(100, 21);
            this.Sql_txtHost.TabIndex = 8;
            this.Sql_txtHost.Tag = "The host name for the database. Leave this unless problems occur.";
            // 
            // Sql_lblPort
            // 
            this.Sql_lblPort.AutoSize = true;
            this.Sql_lblPort.Location = new Point(9, 158);
            this.Sql_lblPort.Name = "Sql_lblPort";
            this.Sql_lblPort.Size = new Size(30, 13);
            this.Sql_lblPort.TabIndex = 31;
            this.Sql_lblPort.Text = "Port:";
            // 
            // Sql_txtPort
            // 
            this.Sql_txtPort.BackColor = SystemColors.Window;
            this.Sql_txtPort.Location = new Point(111, 155);
            this.Sql_txtPort.Name = "Sql_txtPort";
            this.Sql_txtPort.Size = new Size(100, 21);
            this.Sql_txtPort.TabIndex = 32;
            // 
            // Irc_grp
            // 
            this.Irc_grp.Controls.Add(this.Irc_chkEnabled);
            this.Irc_grp.Controls.Add(this.Irc_lblServer);
            this.Irc_grp.Controls.Add(this.Irc_txtServer);
            this.Irc_grp.Controls.Add(this.Irc_lblPort);
            this.Irc_grp.Controls.Add(this.Irc_numPort);
            this.Irc_grp.Controls.Add(this.Irc_lblNick);
            this.Irc_grp.Controls.Add(this.Irc_txtNick);
            this.Irc_grp.Controls.Add(this.Irc_lblChannel);
            this.Irc_grp.Controls.Add(this.Irc_txtChannel);
            this.Irc_grp.Controls.Add(this.Irc_lblOpChannel);
            this.Irc_grp.Controls.Add(this.Irc_chkPass);
            this.Irc_grp.Controls.Add(this.Irc_txtPass);
            this.Irc_grp.Controls.Add(this.Irc_txtOpChannel);
            this.Irc_grp.Location = new Point(8, 3);
            this.Irc_grp.Name = "Irc_grp";
            this.Irc_grp.Size = new Size(225, 214);
            this.Irc_grp.TabIndex = 27;
            this.Irc_grp.TabStop = false;
            this.Irc_grp.Text = "IRC";
            // 
            // Irc_lblServer
            // 
            this.Irc_lblServer.AutoSize = true;
            this.Irc_lblServer.Location = new Point(6, 50);
            this.Irc_lblServer.Name = "Irc_lblServer";
            this.Irc_lblServer.Size = new Size(40, 13);
            this.Irc_lblServer.TabIndex = 19;
            this.Irc_lblServer.Text = "Server:";
            // 
            // Irc_lblPort
            // 
            this.Irc_lblPort.AutoSize = true;
            this.Irc_lblPort.Location = new Point(6, 77);
            this.Irc_lblPort.Name = "Irc_lblPort";
            this.Irc_lblPort.Size = new Size(30, 13);
            this.Irc_lblPort.TabIndex = 30;
            this.Irc_lblPort.Text = "Port:";
            // 
            // Irc_numPort
            // 
            this.Irc_numPort.BackColor = SystemColors.Window;
            this.Irc_numPort.Location = new Point(82, 74);
            this.Irc_numPort.Name = "Irc_numPort";
            this.Irc_numPort.Size = new Size(63, 21);
            this.Irc_numPort.TabIndex = 31;
            this.Irc_numPort.Maximum = new decimal(new int[] {
                                    65535,
                                    0,
                                    0,
                                    0});
            this.Irc_numPort.Value = new decimal(new int[] {
                                    6667,
                                    0,
                                    0,
                                    0});
            // 
            // Irc_lblNick
            // 
            this.Irc_lblNick.AutoSize = true;
            this.Irc_lblNick.Location = new Point(6, 104);
            this.Irc_lblNick.Name = "Irc_lblNick";
            this.Irc_lblNick.Size = new Size(30, 13);
            this.Irc_lblNick.TabIndex = 20;
            this.Irc_lblNick.Text = "Nick:";
            // 
            // Irc_lblChannel
            // 
            this.Irc_lblChannel.AutoSize = true;
            this.Irc_lblChannel.Location = new Point(6, 131);
            this.Irc_lblChannel.Name = "Irc_lblChannel";
            this.Irc_lblChannel.Size = new Size(49, 13);
            this.Irc_lblChannel.TabIndex = 18;
            this.Irc_lblChannel.Text = "Channel:";
            // 
            // Irc_lblOpChannel
            // 
            this.Irc_lblOpChannel.AutoSize = true;
            this.Irc_lblOpChannel.Location = new Point(6, 158);
            this.Irc_lblOpChannel.Name = "Irc_lblOpChannel";
            this.Irc_lblOpChannel.Size = new Size(64, 13);
            this.Irc_lblOpChannel.TabIndex = 25;
            this.Irc_lblOpChannel.Text = "Op Channel:";
            // 
            // PageServer
            // 
            this.PageServer.BackColor = SystemColors.Control;
            this.PageServer.Controls.Add(this.Lvl_grp);
            this.PageServer.Controls.Add(this.Adv_grp);
            this.PageServer.Controls.Add(this.Srv_grp);
            this.PageServer.Controls.Add(this.Srv_grpUpdate);
            this.PageServer.Controls.Add(this.grpPlayers);
            this.PageServer.Location = new Point(4, 22);
            this.PageServer.Name = "PageServer";
            this.PageServer.Padding = new Padding(3);
            this.PageServer.Size = new Size(498, 521);
            this.PageServer.TabIndex = 0;
            this.PageServer.Text = "Server";
            // 
            // Lvl_grp
            // 
            this.Lvl_grp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Lvl_grp.Controls.Add(this.Lvl_lblMain);
            this.Lvl_grp.Controls.Add(this.Lvl_txtMain);
            this.Lvl_grp.Controls.Add(this.Lvl_chkAutoload);
            this.Lvl_grp.Controls.Add(this.Lvl_chkWorld);
            this.Lvl_grp.Location = new Point(314, 160);
            this.Lvl_grp.Name = "Lvl_grp";
            this.Lvl_grp.Size = new Size(177, 105);
            this.Lvl_grp.TabIndex = 44;
            this.Lvl_grp.TabStop = false;
            this.Lvl_grp.Text = "Level Settings";
            // 
            // Lvl_lblMain
            // 
            this.Lvl_lblMain.AutoSize = true;
            this.Lvl_lblMain.Location = new Point(6, 22);
            this.Lvl_lblMain.Name = "Lvl_lblMain";
            this.Lvl_lblMain.Size = new Size(63, 13);
            this.Lvl_lblMain.TabIndex = 3;
            this.Lvl_lblMain.Text = "Main name:";
            // 
            // Lvl_txtMain
            // 
            this.Lvl_txtMain.BackColor = SystemColors.Window;
            this.Lvl_txtMain.Location = new Point(75, 19);
            this.Lvl_txtMain.Name = "Lvl_txtMain";
            this.Lvl_txtMain.Size = new Size(87, 21);
            this.Lvl_txtMain.TabIndex = 2;
            // 
            // Adv_grp
            // 
            this.Adv_grp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Adv_grp.Controls.Add(this.Adv_chkVerify);
            this.Adv_grp.Controls.Add(this.Adv_chkCPE);
            this.Adv_grp.Controls.Add(this.Adv_btnEditTexts);
            this.Adv_grp.Location = new Point(8, 271);
            this.Adv_grp.Name = "Adv_grp";
            this.Adv_grp.Size = new Size(206, 98);
            this.Adv_grp.TabIndex = 42;
            this.Adv_grp.TabStop = false;
            this.Adv_grp.Text = "Advanced Configuration";
            // 
            // Adv_btnEditTexts
            // 
            this.Adv_btnEditTexts.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Adv_btnEditTexts.Cursor = Cursors.Hand;
            this.Adv_btnEditTexts.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Adv_btnEditTexts.Location = new Point(6, 68);
            this.Adv_btnEditTexts.Name = "Adv_btnEditTexts";
            this.Adv_btnEditTexts.Size = new Size(80, 23);
            this.Adv_btnEditTexts.TabIndex = 35;
            this.Adv_btnEditTexts.Text = "Edit Text Files";
            this.Adv_btnEditTexts.UseVisualStyleBackColor = true;
            this.Adv_btnEditTexts.Click += new EventHandler(this.Adv_btnEditTexts_Click);
            // 
            // Srv_grp
            // 
            this.Srv_grp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Srv_grp.Controls.Add(this.Srv_lblName);
            this.Srv_grp.Controls.Add(this.Srv_txtName);
            this.Srv_grp.Controls.Add(this.Srv_lblMotd);
            this.Srv_grp.Controls.Add(this.Srv_txtMOTD);
            this.Srv_grp.Controls.Add(this.Srv_lblPort);
            this.Srv_grp.Controls.Add(this.Srv_numPort);
            this.Srv_grp.Controls.Add(this.Srv_btnPort);
            this.Srv_grp.Controls.Add(this.Srv_lblOwner);
            this.Srv_grp.Controls.Add(this.Srv_txtOwner);
            this.Srv_grp.Controls.Add(this.Srv_chkPublic);
            this.Srv_grp.Location = new Point(8, 6);
            this.Srv_grp.Name = "Srv_grp";
            this.Srv_grp.Size = new Size(483, 148);
            this.Srv_grp.TabIndex = 41;
            this.Srv_grp.TabStop = false;
            this.Srv_grp.Text = "General Configuration";
            // 
            // Srv_lblName
            // 
            this.Srv_lblName.AutoSize = true;
            this.Srv_lblName.Location = new Point(6, 22);
            this.Srv_lblName.Name = "Srv_lblName";
            this.Srv_lblName.Size = new Size(38, 13);
            this.Srv_lblName.TabIndex = 100;
            this.Srv_lblName.Text = "Name:";
            // 
            // Srv_lblMotd
            // 
            this.Srv_lblMotd.AutoSize = true;
            this.Srv_lblMotd.Location = new Point(6, 49);
            this.Srv_lblMotd.Name = "Srv_lblMotd";
            this.Srv_lblMotd.Size = new Size(38, 13);
            this.Srv_lblMotd.TabIndex = 101;
            this.Srv_lblMotd.Text = "MOTD:";
            // 
            // Srv_lblPort
            // 
            this.Srv_lblPort.AutoSize = true;
            this.Srv_lblPort.Location = new Point(6, 76);
            this.Srv_lblPort.Name = "Srv_lblPort";
            this.Srv_lblPort.Size = new Size(30, 13);
            this.Srv_lblPort.TabIndex = 102;
            this.Srv_lblPort.Text = "Port:";
            // 
            // Srv_btnPort
            // 
            this.Srv_btnPort.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Srv_btnPort.Location = new Point(152, 72);
            this.Srv_btnPort.Name = "Srv_btnPort";
            this.Srv_btnPort.Size = new Size(95, 23);
            this.Srv_btnPort.TabIndex = 3;
            this.Srv_btnPort.Text = "Port forwarding";
            this.Srv_btnPort.UseVisualStyleBackColor = true;
            this.Srv_btnPort.Click += new EventHandler(this.ChkPort_Click);
            // 
            // Srv_lblOwner
            // 
            this.Srv_lblOwner.AutoSize = true;
            this.Srv_lblOwner.Location = new Point(6, 103);
            this.Srv_lblOwner.Name = "Srv_lblOwner";
            this.Srv_lblOwner.Size = new Size(72, 13);
            this.Srv_lblOwner.TabIndex = 104;
            this.Srv_lblOwner.Text = "Server owner:";
            // 
            // Srv_txtOwner
            // 
            this.Srv_txtOwner.BackColor = SystemColors.Window;
            this.Srv_txtOwner.Location = new Point(83, 100);
            this.Srv_txtOwner.MaxLength = 64;
            this.Srv_txtOwner.Name = "Srv_txtOwner";
            this.Srv_txtOwner.Size = new Size(119, 21);
            this.Srv_txtOwner.TabIndex = 4;
            // 
            // Srv_grpUpdate
            // 
            this.Srv_grpUpdate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Srv_grpUpdate.Controls.Add(this.Srv_btnForceUpdate);
            this.Srv_grpUpdate.Controls.Add(this.ChkUpdates);
            this.Srv_grpUpdate.Location = new Point(220, 271);
            this.Srv_grpUpdate.Name = "Srv_grpUpdate";
            this.Srv_grpUpdate.Size = new Size(271, 98);
            this.Srv_grpUpdate.TabIndex = 44;
            this.Srv_grpUpdate.TabStop = false;
            this.Srv_grpUpdate.Text = "Update Settings";
            // 
            // Srv_btnForceUpdate
            // 
            this.Srv_btnForceUpdate.AutoSize = true;
            this.Srv_btnForceUpdate.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Srv_btnForceUpdate.Location = new Point(186, 16);
            this.Srv_btnForceUpdate.Name = "Srv_btnForceUpdate";
            this.Srv_btnForceUpdate.Size = new Size(79, 23);
            this.Srv_btnForceUpdate.TabIndex = 6;
            this.Srv_btnForceUpdate.Text = "Force update";
            this.Srv_btnForceUpdate.UseVisualStyleBackColor = true;
            this.Srv_btnForceUpdate.Click += new EventHandler(this.ForceUpdateBtn_Click);
            // 
            // ChkUpdates
            // 
            this.ChkUpdates.AutoSize = true;
            this.ChkUpdates.Location = new Point(6, 20);
            this.ChkUpdates.Name = "ChkUpdates";
            this.ChkUpdates.Size = new Size(110, 17);
            this.ChkUpdates.TabIndex = 4;
            this.ChkUpdates.Text = "Check for updates";
            this.ChkUpdates.UseVisualStyleBackColor = true;
            // 
            // grpPlayers
            // 
            this.grpPlayers.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.grpPlayers.Controls.Add(this.Srv_lblPlayers);
            this.grpPlayers.Controls.Add(this.Srv_numPlayers);
            this.grpPlayers.Controls.Add(this.Srv_cbMustAgree);
            this.grpPlayers.Controls.Add(this.Srv_lblGuests);
            this.grpPlayers.Controls.Add(this.Srv_numGuests);
            this.grpPlayers.Location = new Point(8, 160);
            this.grpPlayers.Name = "grpPlayers";
            this.grpPlayers.Size = new Size(300, 105);
            this.grpPlayers.TabIndex = 46;
            this.grpPlayers.TabStop = false;
            this.grpPlayers.Text = "Players";
            // 
            // Srv_lblPlayers
            // 
            this.Srv_lblPlayers.AutoSize = true;
            this.Srv_lblPlayers.Location = new Point(6, 22);
            this.Srv_lblPlayers.Name = "Srv_lblPlayers";
            this.Srv_lblPlayers.Size = new Size(67, 13);
            this.Srv_lblPlayers.TabIndex = 3;
            this.Srv_lblPlayers.Text = "Max Players:";
            // 
            // Srv_numPlayers
            // 
                this.Srv_numPlayers.BackColor = SystemColors.Window;
                this.Srv_numPlayers.Location = new Point(83, 20);
                this.Srv_numPlayers.Maximum = new decimal(new int[] {
                                    Flames.Server.MAX_PLAYERS,
                                    0,
                                    0,
                                    0});
            this.Srv_numPlayers.Name = "Srv_numPlayers";
            this.Srv_numPlayers.Size = new Size(60, 21);
            this.Srv_numPlayers.TabIndex = 29;
            this.Srv_numPlayers.Value = new decimal(new int[] {
                                    12,
                                    0,
                                    0,
                                    0});
            this.Srv_numPlayers.ValueChanged += new EventHandler(this.NumPlayers_ValueChanged);
            // 
            // Srv_cbMustAgree
            // 
            this.Srv_cbMustAgree.AutoSize = true;
            this.Srv_cbMustAgree.Location = new Point(9, 74);
            this.Srv_cbMustAgree.Name = "Srv_cbMustAgree";
            this.Srv_cbMustAgree.Size = new Size(169, 17);
            this.Srv_cbMustAgree.TabIndex = 32;
            this.Srv_cbMustAgree.Tag = "Forces guests to use /agree on entry to the server";
            this.Srv_cbMustAgree.Text = "Force new guests to read rules";
            this.Srv_cbMustAgree.UseVisualStyleBackColor = true;
            // 
            // Srv_lblGuests
            // 
            this.Srv_lblGuests.AutoSize = true;
            this.Srv_lblGuests.Location = new Point(6, 49);
            this.Srv_lblGuests.Name = "Srv_lblGuests";
            this.Srv_lblGuests.Size = new Size(65, 13);
            this.Srv_lblGuests.TabIndex = 27;
            this.Srv_lblGuests.Text = "Max Guests:";
            // 
            // Srv_numGuests
            // 
            this.Srv_numGuests.BackColor = SystemColors.Window;
            this.Srv_numGuests.Location = new Point(83, 47);
                this.Srv_numGuests.Maximum = new decimal(new int[] {
                                    Flames.Server.MAX_PLAYERS,
                                    0,
                                    0,
                                    0});
            this.Srv_numGuests.Name = "Srv_numGuests";
            this.Srv_numGuests.Size = new Size(60, 21);
            this.Srv_numGuests.TabIndex = 28;
            this.Srv_numGuests.Value = new decimal(new int[] {
                                    10,
                                    0,
                                    0,
                                    0});
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
                                    | AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.PageServer);
            this.TabControl.Controls.Add(this.PageChat);
            this.TabControl.Controls.Add(this.PageRelay);
            this.TabControl.Controls.Add(this.PageEco);
            this.TabControl.Controls.Add(this.PageMisc);
            this.TabControl.Controls.Add(this.PageGames);
            this.TabControl.Controls.Add(this.PageRanks);
            this.TabControl.Controls.Add(this.PageCommands);
            this.TabControl.Controls.Add(this.PageBlocks);
            this.TabControl.Controls.Add(this.PageSecurity);
            this.TabControl.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.TabControl.Location = new Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new Size(506, 547);
            this.TabControl.TabIndex = 1;
            // 
            // PageEco
            // 
            this.PageEco.Controls.Add(this.Eco_gbRank);
            this.PageEco.Controls.Add(this.Eco_gbLvl);
            this.PageEco.Controls.Add(this.Eco_gbItem);
            this.PageEco.Controls.Add(this.Eco_gb);
            this.PageEco.Location = new Point(4, 22);
            this.PageEco.Name = "PageEco";
            this.PageEco.Size = new Size(498, 521);
            this.PageEco.TabIndex = 11;
            this.PageEco.Text = "Eco";
            // 
            // Eco_gbRank
            // 
            this.Eco_gbRank.Controls.Add(this.Eco_dgvRanks);
            this.Eco_gbRank.Controls.Add(this.Eco_cbRank);
            this.Eco_gbRank.Enabled = false;
            this.Eco_gbRank.Location = new Point(8, 85);
            this.Eco_gbRank.Margin = new Padding(2);
            this.Eco_gbRank.Name = "Eco_gbRank";
            this.Eco_gbRank.Padding = new Padding(2);
            this.Eco_gbRank.Size = new Size(484, 197);
            this.Eco_gbRank.TabIndex = 43;
            this.Eco_gbRank.TabStop = false;
            this.Eco_gbRank.Text = "Rank";
            this.Eco_gbRank.Visible = false;
            // 
            // Eco_dgvRanks
            // 
            this.Eco_dgvRanks.AllowUserToAddRows = false;
            this.Eco_dgvRanks.AllowUserToDeleteRows = false;
            this.Eco_dgvRanks.AllowUserToResizeRows = false;
            this.Eco_dgvRanks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Eco_dgvRanks.Columns.AddRange(new DataGridViewColumn[] {
                                    this.Eco_colRankName,
                                    this.Eco_colRankPrice});
            this.Eco_dgvRanks.Location = new Point(6, 38);
            this.Eco_dgvRanks.Margin = new Padding(2);
            this.Eco_dgvRanks.MultiSelect = false;
            this.Eco_dgvRanks.Name = "Eco_dgvRanks";
            this.Eco_dgvRanks.RowHeadersVisible = false;
            this.Eco_dgvRanks.RowTemplate.Height = 18;
            this.Eco_dgvRanks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Eco_dgvRanks.Size = new Size(472, 152);
            this.Eco_dgvRanks.TabIndex = 10;
            this.Eco_dgvRanks.CellValueChanged += new DataGridViewCellEventHandler(this.Eco_dgvRanks_CellValueChanged);
            // 
            // Eco_colRankName
            // 
            this.Eco_colRankName.HeaderText = "Rank";
            this.Eco_colRankName.Name = "Eco_colRankName";
            this.Eco_colRankName.ReadOnly = true;
            this.Eco_colRankName.Width = 140;
            // 
            // Eco_colRankPrice
            // 
            this.Eco_colRankPrice.HeaderText = "Price";
            this.Eco_colRankPrice.Name = "Eco_colRankPrice";
            this.Eco_colRankPrice.Width = 60;
            // 
            // Eco_cbRank
            // 
            this.Eco_cbRank.AutoSize = true;
            this.Eco_cbRank.Location = new Point(6, 17);
            this.Eco_cbRank.Margin = new Padding(2);
            this.Eco_cbRank.Name = "Eco_cbRank";
            this.Eco_cbRank.Size = new Size(64, 17);
            this.Eco_cbRank.TabIndex = 5;
            this.Eco_cbRank.Text = "Enabled";
            this.Eco_cbRank.UseVisualStyleBackColor = true;
            this.Eco_cbRank.CheckedChanged += new EventHandler(this.Eco_cbRank_CheckedChanged);
            // 
            // Eco_gbLvl
            // 
            this.Eco_gbLvl.Controls.Add(this.Eco_dgvMaps);
            this.Eco_gbLvl.Controls.Add(this.Eco_btnLvlDel);
            this.Eco_gbLvl.Controls.Add(this.Eco_btnLvlAdd);
            this.Eco_gbLvl.Controls.Add(this.Eco_cbLvl);
            this.Eco_gbLvl.Enabled = false;
            this.Eco_gbLvl.Location = new Point(8, 85);
            this.Eco_gbLvl.Margin = new Padding(2);
            this.Eco_gbLvl.Name = "Eco_gbLvl";
            this.Eco_gbLvl.Padding = new Padding(2);
            this.Eco_gbLvl.Size = new Size(484, 222);
            this.Eco_gbLvl.TabIndex = 44;
            this.Eco_gbLvl.TabStop = false;
            this.Eco_gbLvl.Text = "Level";
            this.Eco_gbLvl.Visible = false;
            // 
            // Eco_dgvMaps
            // 
            this.Eco_dgvMaps.AllowUserToAddRows = false;
            this.Eco_dgvMaps.AllowUserToDeleteRows = false;
            this.Eco_dgvMaps.AllowUserToResizeRows = false;
            this.Eco_dgvMaps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Eco_dgvMaps.Columns.AddRange(new DataGridViewColumn[] {
                                    this.Eco_colLvlName,
                                    this.Eco_colLvlPrice,
                                    this.Eco_colLvlX,
                                    this.Eco_colLvlY,
                                    this.Eco_colLvlZ,
                                    this.Eco_colLvlTheme});
            this.Eco_dgvMaps.Location = new Point(6, 39);
            this.Eco_dgvMaps.Margin = new Padding(2);
            this.Eco_dgvMaps.MultiSelect = false;
            this.Eco_dgvMaps.Name = "Eco_dgvMaps";
            this.Eco_dgvMaps.RowHeadersVisible = false;
            this.Eco_dgvMaps.RowTemplate.Height = 24;
            this.Eco_dgvMaps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Eco_dgvMaps.Size = new Size(472, 151);
            this.Eco_dgvMaps.TabIndex = 9;
            this.Eco_dgvMaps.CellValueChanged += new DataGridViewCellEventHandler(this.Eco_dgvMaps_CellValueChanged);
            // 
            // Eco_colLvlName
            // 
            this.Eco_colLvlName.HeaderText = "Name";
            this.Eco_colLvlName.Name = "Eco_colLvlName";
            this.Eco_colLvlName.Width = 140;
            // 
            // Eco_colLvlPrice
            // 
            this.Eco_colLvlPrice.HeaderText = "Price";
            this.Eco_colLvlPrice.Name = "Eco_colLvlPrice";
            this.Eco_colLvlPrice.Width = 60;
            // 
            // Eco_colLvlX
            // 
            this.Eco_colLvlX.HeaderText = "Width";
            this.Eco_colLvlX.Name = "Eco_colLvlX";
            this.Eco_colLvlX.Width = 50;
            // 
            // Eco_colLvlY
            // 
            this.Eco_colLvlY.HeaderText = "Height";
            this.Eco_colLvlY.Name = "Eco_colLvlY";
            this.Eco_colLvlY.Width = 50;
            // 
            // Eco_colLvlZ
            // 
            this.Eco_colLvlZ.HeaderText = "Length";
            this.Eco_colLvlZ.Name = "Eco_colLvlZ";
            this.Eco_colLvlZ.Width = 50;
            // 
            // Eco_colLvlTheme
            // 
            this.Eco_colLvlTheme.HeaderText = "Theme";
            this.Eco_colLvlTheme.Name = "Eco_colLvlTheme";
            this.Eco_colLvlTheme.Resizable = DataGridViewTriState.True;
            this.Eco_colLvlTheme.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Eco_btnLvlDel
            // 
            this.Eco_btnLvlDel.Enabled = false;
            this.Eco_btnLvlDel.Location = new Point(406, 194);
            this.Eco_btnLvlDel.Margin = new Padding(2);
            this.Eco_btnLvlDel.Name = "Eco_btnLvlDel";
            this.Eco_btnLvlDel.Size = new Size(72, 23);
            this.Eco_btnLvlDel.TabIndex = 8;
            this.Eco_btnLvlDel.Text = "Remove";
            this.Eco_btnLvlDel.UseVisualStyleBackColor = true;
            this.Eco_btnLvlDel.Click += new EventHandler(this.Eco_lvlDelete_Click);
            // 
            // Eco_btnLvlAdd
            // 
            this.Eco_btnLvlAdd.Location = new Point(4, 194);
            this.Eco_btnLvlAdd.Margin = new Padding(2);
            this.Eco_btnLvlAdd.Name = "Eco_btnLvlAdd";
            this.Eco_btnLvlAdd.Size = new Size(72, 23);
            this.Eco_btnLvlAdd.TabIndex = 7;
            this.Eco_btnLvlAdd.Text = "Add";
            this.Eco_btnLvlAdd.UseVisualStyleBackColor = true;
            this.Eco_btnLvlAdd.Click += new EventHandler(this.Eco_lvlAdd_Click);
            // 
            // Eco_cbLvl
            // 
            this.Eco_cbLvl.AutoSize = true;
            this.Eco_cbLvl.Location = new Point(6, 17);
            this.Eco_cbLvl.Margin = new Padding(2);
            this.Eco_cbLvl.Name = "Eco_cbLvl";
            this.Eco_cbLvl.Size = new Size(64, 17);
            this.Eco_cbLvl.TabIndex = 6;
            this.Eco_cbLvl.Text = "Enabled";
            this.Eco_cbLvl.UseVisualStyleBackColor = true;
            this.Eco_cbLvl.CheckedChanged += new EventHandler(this.Eco_lvlEnabled_CheckedChanged);
            // 
            // Eco_gbItem
            // 
            this.Eco_gbItem.Controls.Add(this.Eco_lblItemRank);
            this.Eco_gbItem.Controls.Add(this.Eco_cmbItemRank);
            this.Eco_gbItem.Controls.Add(this.Eco_numItemPrice);
            this.Eco_gbItem.Controls.Add(this.Eco_lblItemPrice);
            this.Eco_gbItem.Controls.Add(this.Eco_cbItem);
            this.Eco_gbItem.Enabled = false;
            this.Eco_gbItem.Location = new Point(8, 85);
            this.Eco_gbItem.Margin = new Padding(2);
            this.Eco_gbItem.Name = "Eco_gbItem";
            this.Eco_gbItem.Padding = new Padding(2);
            this.Eco_gbItem.Size = new Size(484, 70);
            this.Eco_gbItem.TabIndex = 42;
            this.Eco_gbItem.TabStop = false;
            this.Eco_gbItem.Text = "Titlecolor";
            this.Eco_gbItem.Visible = false;
            // 
            // Eco_lblItemRank
            // 
            this.Eco_lblItemRank.AutoSize = true;
            this.Eco_lblItemRank.Location = new Point(311, 46);
            this.Eco_lblItemRank.Name = "Eco_lblItemRank";
            this.Eco_lblItemRank.Size = new Size(51, 13);
            this.Eco_lblItemRank.TabIndex = 24;
            this.Eco_lblItemRank.Text = "Min rank:";
            // 
            // Eco_numItemPrice
            // 
            this.Eco_numItemPrice.BackColor = SystemColors.Window;
            this.Eco_numItemPrice.Location = new Point(76, 43);
            this.Eco_numItemPrice.Margin = new Padding(2);
            this.Eco_numItemPrice.Maximum = new decimal(new int[] {
                                    16777215,
                                    0,
                                    0,
                                    0});
            this.Eco_numItemPrice.Name = "Eco_numItemPrice";
            this.Eco_numItemPrice.Size = new Size(78, 21);
            this.Eco_numItemPrice.TabIndex = 6;
            this.Eco_numItemPrice.Value = new decimal(new int[] {
                                    100,
                                    0,
                                    0,
                                    0});
            this.Eco_numItemPrice.ValueChanged += new EventHandler(this.Eco_numItemPrice_ValueChanged);
            // 
            // Eco_lblItemPrice
            // 
            this.Eco_lblItemPrice.AutoSize = true;
            this.Eco_lblItemPrice.Location = new Point(38, 45);
            this.Eco_lblItemPrice.Margin = new Padding(2, 0, 2, 0);
            this.Eco_lblItemPrice.Name = "Eco_lblItemPrice";
            this.Eco_lblItemPrice.Size = new Size(34, 13);
            this.Eco_lblItemPrice.TabIndex = 6;
            this.Eco_lblItemPrice.Text = "Price:";
            // 
            // Eco_cbItem
            // 
            this.Eco_cbItem.AutoSize = true;
            this.Eco_cbItem.Location = new Point(6, 17);
            this.Eco_cbItem.Margin = new Padding(2);
            this.Eco_cbItem.Name = "Eco_cbItem";
            this.Eco_cbItem.Size = new Size(64, 17);
            this.Eco_cbItem.TabIndex = 6;
            this.Eco_cbItem.Text = "Enabled";
            this.Eco_cbItem.UseVisualStyleBackColor = true;
            this.Eco_cbItem.CheckedChanged += new EventHandler(this.Eco_cbItem_CheckedChanged);
            // 
            // Eco_gb
            // 
            this.Eco_gb.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Eco_gb.Controls.Add(this.Eco_cmbCfg);
            this.Eco_gb.Controls.Add(this.Eco_lblCfg);
            this.Eco_gb.Controls.Add(this.Eco_cbEnabled);
            this.Eco_gb.Controls.Add(this.Eco_txtCurrency);
            this.Eco_gb.Controls.Add(this.Eco_lblCurrency);
            this.Eco_gb.Location = new Point(8, 3);
            this.Eco_gb.Name = "Eco_gb";
            this.Eco_gb.Size = new Size(484, 70);
            this.Eco_gb.TabIndex = 41;
            this.Eco_gb.TabStop = false;
            this.Eco_gb.Text = "Economy";
            // 
            // Eco_cmbCfg
            // 
            this.Eco_cmbCfg.BackColor = SystemColors.Window;
            this.Eco_cmbCfg.FormattingEnabled = true;
            this.Eco_cmbCfg.Location = new Point(368, 42);
            this.Eco_cmbCfg.Name = "Eco_cmbCfg";
            this.Eco_cmbCfg.Size = new Size(110, 21);
            this.Eco_cmbCfg.TabIndex = 23;
            this.Eco_cmbCfg.SelectedIndexChanged += new EventHandler(this.Eco_cmbCfg_SelectedIndexChanged);
            // 
            // Eco_lblCfg
            // 
            this.Eco_lblCfg.AutoSize = true;
            this.Eco_lblCfg.Location = new Point(286, 45);
            this.Eco_lblCfg.Name = "Eco_lblCfg";
            this.Eco_lblCfg.Size = new Size(79, 13);
            this.Eco_lblCfg.TabIndex = 22;
            this.Eco_lblCfg.Text = "Configure item:";
            // 
            // Eco_cbEnabled
            // 
            this.Eco_cbEnabled.AutoSize = true;
            this.Eco_cbEnabled.Location = new Point(6, 20);
            this.Eco_cbEnabled.Name = "Eco_cbEnabled";
            this.Eco_cbEnabled.Size = new Size(64, 17);
            this.Eco_cbEnabled.TabIndex = 21;
            this.Eco_cbEnabled.Text = "Enabled";
            this.Eco_cbEnabled.UseVisualStyleBackColor = true;
            this.Eco_cbEnabled.CheckedChanged += new EventHandler(this.Eco_cbEnabled_CheckedChanged);
            // 
            // Eco_txtCurrency
            // 
            this.Eco_txtCurrency.BackColor = SystemColors.Window;
            this.Eco_txtCurrency.Location = new Point(76, 42);
            this.Eco_txtCurrency.Name = "Eco_txtCurrency";
            this.Eco_txtCurrency.Size = new Size(118, 21);
            this.Eco_txtCurrency.TabIndex = 1;
            // 
            // Eco_lblCurrency
            // 
            this.Eco_lblCurrency.AutoSize = true;
            this.Eco_lblCurrency.Location = new Point(18, 45);
            this.Eco_lblCurrency.Name = "Eco_lblCurrency";
            this.Eco_lblCurrency.Size = new Size(52, 13);
            this.Eco_lblCurrency.TabIndex = 11;
            this.Eco_lblCurrency.Text = "Currency:";
            // 
            // PageGames
            // 
            this.PageGames.BackColor = SystemColors.Control;
            this.PageGames.Controls.Add(this.TabGames);
            this.PageGames.Location = new Point(4, 22);
            this.PageGames.Name = "PageGames";
            this.PageGames.Padding = new Padding(3);
            this.PageGames.Size = new Size(498, 521);
            this.PageGames.TabIndex = 8;
            this.PageGames.Text = "Games";
            // 
            // TabGames
            // 
            this.TabGames.Controls.Add(this.TabLS);
            this.TabGames.Controls.Add(this.TabZS);
            this.TabGames.Controls.Add(this.TabZS_old);
            this.TabGames.Controls.Add(this.TabCTF);
            this.TabGames.Controls.Add(this.TabTW);
            this.TabGames.Controls.Add(this.TabCD);
            this.TabGames.Location = new Point(3, 3);
            this.TabGames.Name = "TabGames";
            this.TabGames.SelectedIndex = 0;
            this.TabGames.Size = new Size(492, 515);
            this.TabGames.TabIndex = 0;
            // 
            // TabLS
            // 
            this.TabLS.BackColor = SystemColors.Control;
            this.TabLS.Controls.Add(this.Ls_grpControls);
            this.TabLS.Controls.Add(this.Ls_grpMapSettings);
            this.TabLS.Controls.Add(this.Ls_grpSettings);
            this.TabLS.Controls.Add(this.Ls_grpMaps);
            this.TabLS.Location = new Point(4, 22);
            this.TabLS.Name = "TabLS";
            this.TabLS.Padding = new Padding(3);
            this.TabLS.Size = new Size(484, 489);
            this.TabLS.TabIndex = 0;
            this.TabLS.Text = "Lava Survival";
            // 
            // Ls_grpControls
            // 
            this.Ls_grpControls.Controls.Add(this.Ls_btnEnd);
            this.Ls_grpControls.Controls.Add(this.Ls_btnStop);
            this.Ls_grpControls.Controls.Add(this.Ls_btnStart);
            this.Ls_grpControls.Location = new Point(110, 5);
            this.Ls_grpControls.Name = "Ls_grpControls";
            this.Ls_grpControls.Size = new Size(279, 51);
            this.Ls_grpControls.TabIndex = 4;
            this.Ls_grpControls.TabStop = false;
            this.Ls_grpControls.Text = "Controls";
            // 
            // Ls_btnEnd
            // 
            this.Ls_btnEnd.Location = new Point(190, 19);
            this.Ls_btnEnd.Name = "Ls_btnEnd";
            this.Ls_btnEnd.Size = new Size(80, 23);
            this.Ls_btnEnd.TabIndex = 2;
            this.Ls_btnEnd.Text = "End Round";
            this.Ls_btnEnd.UseVisualStyleBackColor = true;
            // 
            // Ls_btnStop
            // 
            this.Ls_btnStop.Location = new Point(100, 19);
            this.Ls_btnStop.Name = "Ls_btnStop";
            this.Ls_btnStop.Size = new Size(80, 23);
            this.Ls_btnStop.TabIndex = 1;
            this.Ls_btnStop.Text = "Stop Game";
            this.Ls_btnStop.UseVisualStyleBackColor = true;
            // 
            // Ls_btnStart
            // 
            this.Ls_btnStart.Location = new Point(10, 19);
            this.Ls_btnStart.Name = "Ls_btnStart";
            this.Ls_btnStart.Size = new Size(80, 23);
            this.Ls_btnStart.TabIndex = 0;
            this.Ls_btnStart.Text = "Start Game";
            this.Ls_btnStart.UseVisualStyleBackColor = true;
            // 
            // Ls_grpMapSettings
            // 
            this.Ls_grpMapSettings.Controls.Add(this.Ls_grpTime);
            this.Ls_grpMapSettings.Controls.Add(this.Ls_grpLayer);
            this.Ls_grpMapSettings.Controls.Add(this.Ls_grpFlood);
            this.Ls_grpMapSettings.Enabled = false;
            this.Ls_grpMapSettings.Location = new Point(182, 184);
            this.Ls_grpMapSettings.Name = "Ls_grpMapSettings";
            this.Ls_grpMapSettings.Size = new Size(296, 287);
            this.Ls_grpMapSettings.TabIndex = 3;
            this.Ls_grpMapSettings.TabStop = false;
            this.Ls_grpMapSettings.Text = "Map Settings";
            // 
            // Ls_grpTime
            // 
            this.Ls_grpTime.Controls.Add(this.Ls_numFlood);
            this.Ls_grpTime.Controls.Add(this.Ls_numLayerTime);
            this.Ls_grpTime.Controls.Add(this.Ls_numRound);
            this.Ls_grpTime.Controls.Add(this.Ls_lblLayerTime);
            this.Ls_grpTime.Controls.Add(this.Ls_lblFlood);
            this.Ls_grpTime.Controls.Add(this.Ls_lblRound);
            this.Ls_grpTime.Location = new Point(6, 180);
            this.Ls_grpTime.Name = "Ls_grpTime";
            this.Ls_grpTime.Size = new Size(284, 71);
            this.Ls_grpTime.TabIndex = 39;
            this.Ls_grpTime.TabStop = false;
            this.Ls_grpTime.Text = "Time settings";
            // 
            // Ls_numFlood
            // 
            this.Ls_numFlood.BackColor = SystemColors.Window;
            this.Ls_numFlood.Location = new Point(69, 43);
            this.Ls_numFlood.Name = "Ls_numFlood";
            this.Ls_numFlood.Seconds = ((long)(300));
            this.Ls_numFlood.Size = new Size(62, 21);
            this.Ls_numFlood.TabIndex = 38;
            this.Ls_numFlood.Text = "5m";
            this.Ls_numFlood.Value = TimeSpan.Parse("00:05:00");
            // 
            // Ls_numLayerTime
            // 
            this.Ls_numLayerTime.BackColor = SystemColors.Window;
            this.Ls_numLayerTime.Location = new Point(216, 16);
            this.Ls_numLayerTime.Name = "Ls_numLayerTime";
            this.Ls_numLayerTime.Seconds = ((long)(120));
            this.Ls_numLayerTime.Size = new Size(62, 21);
            this.Ls_numLayerTime.TabIndex = 37;
            this.Ls_numLayerTime.Text = "2m";
            this.Ls_numLayerTime.Value = TimeSpan.Parse("00:02:00");
            // 
            // Ls_numRound
            // 
            this.Ls_numRound.BackColor = SystemColors.Window;
            this.Ls_numRound.Location = new Point(69, 16);
            this.Ls_numRound.Name = "Ls_numRound";
            this.Ls_numRound.Seconds = ((long)(900));
            this.Ls_numRound.Size = new Size(62, 21);
            this.Ls_numRound.TabIndex = 36;
            this.Ls_numRound.Text = "15m";
            this.Ls_numRound.Value = TimeSpan.Parse("00:15:00");
            // 
            // Ls_lblLayerTime
            // 
            this.Ls_lblLayerTime.AutoSize = true;
            this.Ls_lblLayerTime.Location = new Point(155, 20);
            this.Ls_lblLayerTime.Name = "Ls_lblLayerTime";
            this.Ls_lblLayerTime.Size = new Size(59, 13);
            this.Ls_lblLayerTime.TabIndex = 35;
            this.Ls_lblLayerTime.Text = "Layer time:";
            // 
            // Ls_lblFlood
            // 
            this.Ls_lblFlood.AutoSize = true;
            this.Ls_lblFlood.Location = new Point(8, 46);
            this.Ls_lblFlood.Name = "Ls_lblFlood";
            this.Ls_lblFlood.Size = new Size(59, 13);
            this.Ls_lblFlood.TabIndex = 34;
            this.Ls_lblFlood.Text = "Flood time:";
            // 
            // Ls_lblRound
            // 
            this.Ls_lblRound.AutoSize = true;
            this.Ls_lblRound.Location = new Point(4, 20);
            this.Ls_lblRound.Name = "Ls_lblRound";
            this.Ls_lblRound.Size = new Size(63, 13);
            this.Ls_lblRound.TabIndex = 34;
            this.Ls_lblRound.Text = "Round time:";
            // 
            // Ls_grpLayer
            // 
            this.Ls_grpLayer.Controls.Add(this.Ls_lblBlocksTall);
            this.Ls_grpLayer.Controls.Add(this.Ls_numHeight);
            this.Ls_grpLayer.Controls.Add(this.Ls_lblLayersEach);
            this.Ls_grpLayer.Controls.Add(this.Ls_numCount);
            this.Ls_grpLayer.Controls.Add(this.Ls_numLayer);
            this.Ls_grpLayer.Controls.Add(this.Ls_lblLayer);
            this.Ls_grpLayer.Location = new Point(6, 100);
            this.Ls_grpLayer.Name = "Ls_grpLayer";
            this.Ls_grpLayer.Size = new Size(284, 74);
            this.Ls_grpLayer.TabIndex = 1;
            this.Ls_grpLayer.TabStop = false;
            this.Ls_grpLayer.Text = "Layer flood settings";
            // 
            // Ls_lblBlocksTall
            // 
            this.Ls_lblBlocksTall.AutoSize = true;
            this.Ls_lblBlocksTall.Location = new Point(183, 48);
            this.Ls_lblBlocksTall.Name = "Ls_lblBlocksTall";
            this.Ls_lblBlocksTall.Size = new Size(55, 13);
            this.Ls_lblBlocksTall.TabIndex = 38;
            this.Ls_lblBlocksTall.Text = "blocks tall";
            // 
            // Ls_lblLayersEach
            // 
            this.Ls_lblLayersEach.AutoSize = true;
            this.Ls_lblLayersEach.Location = new Point(62, 48);
            this.Ls_lblLayersEach.Name = "Ls_lblLayersEach";
            this.Ls_lblLayersEach.Size = new Size(64, 13);
            this.Ls_lblLayersEach.TabIndex = 36;
            this.Ls_lblLayersEach.Text = "layers, each";
            // 
            // Ls_lblLayer
            // 
            this.Ls_lblLayer.AutoSize = true;
            this.Ls_lblLayer.Location = new Point(28, 20);
            this.Ls_lblLayer.Name = "Ls_lblLayer";
            this.Ls_lblLayer.Size = new Size(98, 13);
            this.Ls_lblLayer.TabIndex = 34;
            this.Ls_lblLayer.Text = "Layer flood chance:";
            // 
            // Ls_grpFlood
            // 
            this.Ls_grpFlood.Controls.Add(this.Ls_numFloodUp);
            this.Ls_grpFlood.Controls.Add(this.Ls_numFast);
            this.Ls_grpFlood.Controls.Add(this.Ls_numWater);
            this.Ls_grpFlood.Controls.Add(this.Ls_lblFloodUp);
            this.Ls_grpFlood.Controls.Add(this.Ls_lblFast);
            this.Ls_grpFlood.Controls.Add(this.Ls_lblWater);
            this.Ls_grpFlood.Location = new Point(6, 20);
            this.Ls_grpFlood.Name = "Ls_grpFlood";
            this.Ls_grpFlood.Size = new Size(284, 74);
            this.Ls_grpFlood.TabIndex = 0;
            this.Ls_grpFlood.TabStop = false;
            this.Ls_grpFlood.Text = "Flood settings";
            // 
            // Ls_lblFloodUp
            // 
            this.Ls_lblFloodUp.AutoSize = true;
            this.Ls_lblFloodUp.Location = new Point(133, 48);
            this.Ls_lblFloodUp.Name = "Ls_lblFloodUp";
            this.Ls_lblFloodUp.Size = new Size(88, 13);
            this.Ls_lblFloodUp.TabIndex = 30;
            this.Ls_lblFloodUp.Text = "Floods up chance:";
            // 
            // Ls_lblFast
            // 
            this.Ls_lblFast.AutoSize = true;
            this.Ls_lblFast.Location = new Point(11, 48);
            this.Ls_lblFast.Name = "Ls_lblFast";
            this.Ls_lblFast.Size = new Size(66, 13);
            this.Ls_lblFast.TabIndex = 29;
            this.Ls_lblFast.Text = "Fast chance:";
            // 
            // Ls_lblWater
            // 
            this.Ls_lblWater.AutoSize = true;
            this.Ls_lblWater.Location = new Point(1, 23);
            this.Ls_lblWater.Name = "Ls_lblWater";
            this.Ls_lblWater.Size = new Size(71, 13);
            this.Ls_lblWater.TabIndex = 27;
            this.Ls_lblWater.Text = "Water chance:";
            // 
            // Ls_grpSettings
            // 
            this.Ls_grpSettings.Controls.Add(this.Ls_lblMax);
            this.Ls_grpSettings.Controls.Add(this.Ls_numMax);
            this.Ls_grpSettings.Controls.Add(this.Ls_cbMain);
            this.Ls_grpSettings.Controls.Add(this.Ls_cbMap);
            this.Ls_grpSettings.Controls.Add(this.Ls_cbStart);
            this.Ls_grpSettings.Location = new Point(182, 59);
            this.Ls_grpSettings.Name = "Ls_grpSettings";
            this.Ls_grpSettings.Size = new Size(296, 119);
            this.Ls_grpSettings.TabIndex = 2;
            this.Ls_grpSettings.TabStop = false;
            this.Ls_grpSettings.Text = "Settings";
            // 
            // Ls_lblMax
            // 
            this.Ls_lblMax.AutoSize = true;
            this.Ls_lblMax.Location = new Point(14, 92);
            this.Ls_lblMax.Name = "Ls_lblMax";
            this.Ls_lblMax.Size = new Size(54, 13);
            this.Ls_lblMax.TabIndex = 26;
            this.Ls_lblMax.Text = "Max lives:";
            // 
            // Ls_cbMap
            // 
            this.Ls_cbMap.AutoSize = true;
            this.Ls_cbMap.Location = new Point(11, 43);
            this.Ls_cbMap.Name = "Ls_cbMap";
            this.Ls_cbMap.Size = new Size(136, 17);
            this.Ls_cbMap.TabIndex = 23;
            this.Ls_cbMap.Text = "Map name in server list";
            this.Ls_cbMap.UseVisualStyleBackColor = true;
            // 
            // Ls_grpMaps
            // 
            this.Ls_grpMaps.Controls.Add(this.Ls_lblNotUsed);
            this.Ls_grpMaps.Controls.Add(this.Ls_lblUsed);
            this.Ls_grpMaps.Controls.Add(this.Ls_btnAdd);
            this.Ls_grpMaps.Controls.Add(this.Ls_btnRemove);
            this.Ls_grpMaps.Controls.Add(this.Ls_lstNotUsed);
            this.Ls_grpMaps.Controls.Add(this.Ls_lstUsed);
            this.Ls_grpMaps.Location = new Point(6, 59);
            this.Ls_grpMaps.Name = "Ls_grpMaps";
            this.Ls_grpMaps.Size = new Size(170, 412);
            this.Ls_grpMaps.TabIndex = 1;
            this.Ls_grpMaps.TabStop = false;
            this.Ls_grpMaps.Text = "Maps";
            // 
            // Ls_lblNotUsed
            // 
            this.Ls_lblNotUsed.AutoSize = true;
            this.Ls_lblNotUsed.Location = new Point(187, 17);
            this.Ls_lblNotUsed.Name = "Ls_lblNotUsed";
            this.Ls_lblNotUsed.Size = new Size(83, 13);
            this.Ls_lblNotUsed.TabIndex = 6;
            this.Ls_lblNotUsed.Text = "Maps Not In Use";
            // 
            // Ls_lblUsed
            // 
            this.Ls_lblUsed.AutoSize = true;
            this.Ls_lblUsed.Location = new Point(6, 17);
            this.Ls_lblUsed.Name = "Ls_lblUsed";
            this.Ls_lblUsed.Size = new Size(38, 13);
            this.Ls_lblUsed.TabIndex = 5;
            this.Ls_lblUsed.Text = "In use:";
            // 
            // Ls_btnAdd
            // 
            this.Ls_btnAdd.Location = new Point(6, 188);
            this.Ls_btnAdd.Name = "Ls_btnAdd";
            this.Ls_btnAdd.Size = new Size(77, 23);
            this.Ls_btnAdd.TabIndex = 4;
            this.Ls_btnAdd.Text = "<< Add";
            this.Ls_btnAdd.UseVisualStyleBackColor = true;
            // 
            // Ls_btnRemove
            // 
            this.Ls_btnRemove.Location = new Point(89, 188);
            this.Ls_btnRemove.Name = "Ls_btnRemove";
            this.Ls_btnRemove.Size = new Size(75, 23);
            this.Ls_btnRemove.TabIndex = 3;
            this.Ls_btnRemove.Text = "Remove >>";
            this.Ls_btnRemove.UseVisualStyleBackColor = true;
            // 
            // Ls_lstNotUsed
            // 
            this.Ls_lstNotUsed.BackColor = SystemColors.Window;
            this.Ls_lstNotUsed.ForeColor = SystemColors.WindowText;
            this.Ls_lstNotUsed.FormattingEnabled = true;
            this.Ls_lstNotUsed.Location = new Point(6, 219);
            this.Ls_lstNotUsed.Name = "Ls_lstNotUsed";
            this.Ls_lstNotUsed.Size = new Size(158, 186);
            this.Ls_lstNotUsed.TabIndex = 2;
            // 
            // Ls_lstUsed
            // 
            this.Ls_lstUsed.BackColor = SystemColors.Window;
            this.Ls_lstUsed.ForeColor = SystemColors.WindowText;
            this.Ls_lstUsed.FormattingEnabled = true;
            this.Ls_lstUsed.Location = new Point(6, 33);
            this.Ls_lstUsed.Name = "Ls_lstUsed";
            this.Ls_lstUsed.Size = new Size(158, 147);
            this.Ls_lstUsed.TabIndex = 0;
            this.Ls_lstUsed.SelectedIndexChanged += new EventHandler(this.LsMapUse_SelectedIndexChanged);
            // 
            // TabZS
            // 
            this.TabZS.BackColor = SystemColors.Control;
            this.TabZS.Controls.Add(this.Zs_grpControls);
            this.TabZS.Controls.Add(this.Zs_grpMap);
            this.TabZS.Controls.Add(this.Zs_grpSettings);
            this.TabZS.Controls.Add(this.Zs_grpMaps);
            this.TabZS.Location = new Point(4, 22);
            this.TabZS.Name = "TabZS";
            this.TabZS.Padding = new Padding(3);
            this.TabZS.Size = new Size(484, 489);
            this.TabZS.TabIndex = 6;
            this.TabZS.Text = "Zombie Survival";
            // 
            // Zs_grpControls
            // 
            this.Zs_grpControls.Controls.Add(this.Zs_btnEnd);
            this.Zs_grpControls.Controls.Add(this.Zs_btnStop);
            this.Zs_grpControls.Controls.Add(this.Zs_btnStart);
            this.Zs_grpControls.Location = new Point(110, 5);
            this.Zs_grpControls.Name = "Zs_grpControls";
            this.Zs_grpControls.Size = new Size(279, 51);
            this.Zs_grpControls.TabIndex = 4;
            this.Zs_grpControls.TabStop = false;
            this.Zs_grpControls.Text = "Controls";
            // 
            // Zs_btnEnd
            // 
            this.Zs_btnEnd.Location = new Point(190, 19);
            this.Zs_btnEnd.Name = "Zs_btnEnd";
            this.Zs_btnEnd.Size = new Size(80, 23);
            this.Zs_btnEnd.TabIndex = 2;
            this.Zs_btnEnd.Text = "End Round";
            this.Zs_btnEnd.UseVisualStyleBackColor = true;
            // 
            // Zs_btnStop
            // 
            this.Zs_btnStop.Location = new Point(100, 19);
            this.Zs_btnStop.Name = "Zs_btnStop";
            this.Zs_btnStop.Size = new Size(80, 23);
            this.Zs_btnStop.TabIndex = 1;
            this.Zs_btnStop.Text = "Stop Game";
            this.Zs_btnStop.UseVisualStyleBackColor = true;
            // 
            // Zs_btnStart
            // 
            this.Zs_btnStart.Location = new Point(10, 19);
            this.Zs_btnStart.Name = "Zs_btnStart";
            this.Zs_btnStart.Size = new Size(80, 23);
            this.Zs_btnStart.TabIndex = 0;
            this.Zs_btnStart.Text = "Start Game";
            this.Zs_btnStart.UseVisualStyleBackColor = true;
            // 
            // Zs_grpMap
            // 
            this.Zs_grpMap.Controls.Add(this.Zs_grpTime);
            this.Zs_grpMap.Enabled = false;
            this.Zs_grpMap.Location = new Point(182, 374);
            this.Zs_grpMap.Name = "Zs_grpMap";
            this.Zs_grpMap.Size = new Size(296, 97);
            this.Zs_grpMap.TabIndex = 3;
            this.Zs_grpMap.TabStop = false;
            this.Zs_grpMap.Text = "Map Settings";
            // 
            // Zs_grpTime
            // 
            this.Zs_grpTime.Controls.Add(this.TimespanUpDown1);
            this.Zs_grpTime.Controls.Add(this.TimespanUpDown2);
            this.Zs_grpTime.Controls.Add(this.TimespanUpDown3);
            this.Zs_grpTime.Controls.Add(this.Label1);
            this.Zs_grpTime.Controls.Add(this.Label2);
            this.Zs_grpTime.Controls.Add(this.Label3);
            this.Zs_grpTime.Location = new Point(6, 16);
            this.Zs_grpTime.Name = "Zs_grpTime";
            this.Zs_grpTime.Size = new Size(284, 71);
            this.Zs_grpTime.TabIndex = 39;
            this.Zs_grpTime.TabStop = false;
            this.Zs_grpTime.Text = "Time settings";
            // 
            // TimespanUpDown1
            // 
            this.TimespanUpDown1.BackColor = SystemColors.Window;
            this.TimespanUpDown1.Location = new Point(69, 43);
            this.TimespanUpDown1.Name = "TimespanUpDown1";
            this.TimespanUpDown1.Seconds = ((long)(300));
            this.TimespanUpDown1.Size = new Size(62, 21);
            this.TimespanUpDown1.TabIndex = 38;
            this.TimespanUpDown1.Text = "5m";
            this.TimespanUpDown1.Value = TimeSpan.Parse("00:05:00");
            // 
            // TimespanUpDown2
            // 
            this.TimespanUpDown2.BackColor = SystemColors.Window;
            this.TimespanUpDown2.Location = new Point(216, 16);
            this.TimespanUpDown2.Name = "TimespanUpDown2";
            this.TimespanUpDown2.Seconds = ((long)(120));
            this.TimespanUpDown2.Size = new Size(62, 21);
            this.TimespanUpDown2.TabIndex = 37;
            this.TimespanUpDown2.Text = "2m";
            this.TimespanUpDown2.Value = TimeSpan.Parse("00:02:00");
            // 
            // TimespanUpDown3
            // 
            this.TimespanUpDown3.BackColor = SystemColors.Window;
            this.TimespanUpDown3.Location = new Point(69, 16);
            this.TimespanUpDown3.Name = "TimespanUpDown3";
            this.TimespanUpDown3.Seconds = ((long)(900));
            this.TimespanUpDown3.Size = new Size(62, 21);
            this.TimespanUpDown3.TabIndex = 36;
            this.TimespanUpDown3.Text = "15m";
            this.TimespanUpDown3.Value = TimeSpan.Parse("00:15:00");
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new Point(155, 20);
            this.Label1.Name = "Label1";
            this.Label1.Size = new Size(59, 13);
            this.Label1.TabIndex = 35;
            this.Label1.Text = "Layer time:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new Point(8, 46);
            this.Label2.Name = "Label2";
            this.Label2.Size = new Size(59, 13);
            this.Label2.TabIndex = 34;
            this.Label2.Text = "Flood time:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new Point(5, 20);
            this.Label3.Name = "Label3";
            this.Label3.Size = new Size(63, 13);
            this.Label3.TabIndex = 34;
            this.Label3.Text = "Round time:";
            // 
            // Zs_grpSettings
            // 
            this.Zs_grpSettings.Controls.Add(this.Zs_grpZombie);
            this.Zs_grpSettings.Controls.Add(this.Zs_grpRevive);
            this.Zs_grpSettings.Controls.Add(this.Zs_cbMain);
            this.Zs_grpSettings.Controls.Add(this.Zs_cbMap);
            this.Zs_grpSettings.Controls.Add(this.Zs_grpInv);
            this.Zs_grpSettings.Controls.Add(this.Zs_cbStart);
            this.Zs_grpSettings.Location = new Point(182, 59);
            this.Zs_grpSettings.Name = "Zs_grpSettings";
            this.Zs_grpSettings.Size = new Size(296, 309);
            this.Zs_grpSettings.TabIndex = 2;
            this.Zs_grpSettings.TabStop = false;
            this.Zs_grpSettings.Text = "Settings";
            // 
            // Zs_grpZombie
            // 
            this.Zs_grpZombie.Controls.Add(this.Zs_txtModel);
            this.Zs_grpZombie.Controls.Add(this.Zs_txtName);
            this.Zs_grpZombie.Controls.Add(this.Zs_lblModel);
            this.Zs_grpZombie.Controls.Add(this.Zs_lblName);
            this.Zs_grpZombie.Location = new Point(6, 250);
            this.Zs_grpZombie.Name = "Zs_grpZombie";
            this.Zs_grpZombie.Size = new Size(284, 46);
            this.Zs_grpZombie.TabIndex = 40;
            this.Zs_grpZombie.TabStop = false;
            this.Zs_grpZombie.Text = "Zombie settings";
            // 
            // Zs_txtModel
            // 
            this.Zs_txtModel.BackColor = SystemColors.Window;
            this.Zs_txtModel.Location = new Point(200, 17);
            this.Zs_txtModel.Name = "Zs_txtModel";
            this.Zs_txtModel.Size = new Size(76, 21);
            this.Zs_txtModel.TabIndex = 39;
            this.GUIToolTip.SetToolTip(this.Zs_txtModel, "Model to use for infected players.\nIf left blank, then 'zombie' model is used.");
            // 
            // Zs_txtName
            // 
            this.Zs_txtName.BackColor = SystemColors.Window;
            this.Zs_txtName.Location = new Point(49, 17);
            this.Zs_txtName.Name = "Zs_txtName";
            this.Zs_txtName.Size = new Size(80, 21);
            this.Zs_txtName.TabIndex = 38;
            this.GUIToolTip.SetToolTip(this.Zs_txtName, "Name to show above head of infected players.\nIf left blank, then the player's name is shown instead.");
            // 
            // Zs_lblModel
            // 
            this.Zs_lblModel.AutoSize = true;
            this.Zs_lblModel.Location = new Point(154, 20);
            this.Zs_lblModel.Name = "Zs_lblModel";
            this.Zs_lblModel.Size = new Size(40, 13);
            this.Zs_lblModel.TabIndex = 35;
            this.Zs_lblModel.Text = "Model:";
            // 
            // Zs_lblName
            // 
            this.Zs_lblName.AutoSize = true;
            this.Zs_lblName.Location = new Point(5, 20);
            this.Zs_lblName.Name = "Zs_lblName";
            this.Zs_lblName.Size = new Size(38, 13);
            this.Zs_lblName.TabIndex = 34;
            this.Zs_lblName.Text = "Name:";
            // 
            // Zs_grpRevive
            // 
            this.Zs_grpRevive.Controls.Add(this.Zs_lblReviveEff);
            this.Zs_grpRevive.Controls.Add(this.Zs_numReviveEff);
            this.Zs_grpRevive.Controls.Add(this.Label4);
            this.Zs_grpRevive.Controls.Add(this.Zs_lblReviveLimitFtr);
            this.Zs_grpRevive.Controls.Add(this.Zs_lblReviveLimitHdr);
            this.Zs_grpRevive.Controls.Add(this.Zs_numReviveLimit);
            this.Zs_grpRevive.Controls.Add(this.Zs_numReviveMax);
            this.Zs_grpRevive.Controls.Add(this.Label9);
            this.Zs_grpRevive.Location = new Point(6, 169);
            this.Zs_grpRevive.Name = "Zs_grpRevive";
            this.Zs_grpRevive.Size = new Size(284, 73);
            this.Zs_grpRevive.TabIndex = 25;
            this.Zs_grpRevive.TabStop = false;
            this.Zs_grpRevive.Text = "Revive settings";
            // 
            // Zs_lblReviveEff
            // 
            this.Zs_lblReviveEff.AutoSize = true;
            this.Zs_lblReviveEff.Location = new Point(202, 20);
            this.Zs_lblReviveEff.Name = "Zs_lblReviveEff";
            this.Zs_lblReviveEff.Size = new Size(79, 13);
            this.Zs_lblReviveEff.TabIndex = 40;
            this.Zs_lblReviveEff.Text = "% effectiveness";
            // 
            // Zs_numReviveEff
            // 
            this.Zs_numReviveEff.BackColor = SystemColors.Window;
            this.Zs_numReviveEff.Location = new Point(150, 16);
            this.Zs_numReviveEff.Maximum = new decimal(new int[] {
                                    1000000,
                                    0,
                                    0,
                                    0});
            this.Zs_numReviveEff.Name = "Zs_numReviveEff";
            this.Zs_numReviveEff.Size = new Size(52, 21);
            this.Zs_numReviveEff.TabIndex = 39;
            this.Zs_numReviveEff.Value = new decimal(new int[] {
                                    3,
                                    0,
                                    0,
                                    0});
            this.GUIToolTip.SetToolTip(this.Zs_numReviveEff, "Likelihood that /buy revive will disinfect a zombie");
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new Point(91, 19);
            this.Label4.Name = "Label4";
            this.Label4.Size = new Size(59, 13);
            this.Label4.TabIndex = 34;
            this.Label4.Text = "times, with";
            // 
            // Zs_lblReviveLimitFtr
            // 
            this.Zs_lblReviveLimitFtr.AutoSize = true;
            this.Zs_lblReviveLimitFtr.Location = new Point(167, 48);
            this.Zs_lblReviveLimitFtr.Name = "Zs_lblReviveLimitFtr";
            this.Zs_lblReviveLimitFtr.Size = new Size(100, 13);
            this.Zs_lblReviveLimitFtr.TabIndex = 38;
            this.Zs_lblReviveLimitFtr.Text = "seconds of infection";
            // 
            // Zs_lblReviveLimitHdr
            // 
            this.Zs_lblReviveLimitHdr.AutoSize = true;
            this.Zs_lblReviveLimitHdr.Location = new Point(7, 48);
            this.Zs_lblReviveLimitHdr.Name = "Zs_lblReviveLimitHdr";
            this.Zs_lblReviveLimitHdr.Size = new Size(102, 13);
            this.Zs_lblReviveLimitHdr.TabIndex = 36;
            this.Zs_lblReviveLimitHdr.Text = "Must be used within";
            // 
            // Zs_numReviveLimit
            // 
            this.Zs_numReviveLimit.BackColor = SystemColors.Window;
            this.Zs_numReviveLimit.Location = new Point(112, 45);
            this.Zs_numReviveLimit.Maximum = new decimal(new int[] {
                                    1000000,
                                    0,
                                    0,
                                    0});
            this.Zs_numReviveLimit.Name = "Zs_numReviveLimit";
            this.Zs_numReviveLimit.Size = new Size(52, 21);
            this.Zs_numReviveLimit.TabIndex = 35;
            this.Zs_numReviveLimit.Value = new decimal(new int[] {
                                    10,
                                    0,
                                    0,
                                    0});
            this.GUIToolTip.SetToolTip(this.Zs_numReviveLimit, "The time limit after a human is infected that /buy revive must be used within");
            // 
            // Zs_numReviveMax
            // 
            this.Zs_numReviveMax.BackColor = SystemColors.Window;
            this.Zs_numReviveMax.Location = new Point(36, 17);
            this.Zs_numReviveMax.Name = "Zs_numReviveMax";
            this.Zs_numReviveMax.Size = new Size(52, 21);
            this.Zs_numReviveMax.TabIndex = 34;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new Point(6, 20);
            this.Label9.Name = "Label9";
            this.Label9.Size = new Size(27, 13);
            this.Label9.TabIndex = 34;
            this.Label9.Text = "Max";
            // 
            // Zs_cbMain
            // 
            this.Zs_cbMain.AutoSize = true;
            this.Zs_cbMain.Location = new Point(11, 66);
            this.Zs_cbMain.Name = "Zs_cbMain";
            this.Zs_cbMain.Size = new Size(112, 17);
            this.Zs_cbMain.TabIndex = 24;
            this.Zs_cbMain.Text = "Change main level";
            this.Zs_cbMain.UseVisualStyleBackColor = true;
            // 
            // Zs_cbMap
            // 
            this.Zs_cbMap.AutoSize = true;
            this.Zs_cbMap.Location = new Point(11, 43);
            this.Zs_cbMap.Name = "Zs_cbMap";
            this.Zs_cbMap.Size = new Size(136, 17);
            this.Zs_cbMap.TabIndex = 23;
            this.Zs_cbMap.Text = "Map name in server list";
            this.Zs_cbMap.UseVisualStyleBackColor = true;
            // 
            // Zs_grpInv
            // 
            this.Zs_grpInv.Controls.Add(this.Zs_numInvZombieDur);
            this.Zs_grpInv.Controls.Add(this.Zs_numInvHumanDur);
            this.Zs_grpInv.Controls.Add(this.Zs_numInvZombieMax);
            this.Zs_grpInv.Controls.Add(this.Zs_numInvHumanMax);
            this.Zs_grpInv.Controls.Add(this.Zs_lblInvZombieDur);
            this.Zs_grpInv.Controls.Add(this.Zs_lblInvZombieMax);
            this.Zs_grpInv.Controls.Add(this.Zs_lblInvHumanDur);
            this.Zs_grpInv.Controls.Add(this.Zs_lblInvHumanMax);
            this.Zs_grpInv.Location = new Point(6, 89);
            this.Zs_grpInv.Name = "Zs_grpInv";
            this.Zs_grpInv.Size = new Size(284, 74);
            this.Zs_grpInv.TabIndex = 0;
            this.Zs_grpInv.TabStop = false;
            this.Zs_grpInv.Text = "Invisibility settings";
            // 
            // Zs_numInvZombieDur
            // 
            this.Zs_numInvZombieDur.BackColor = SystemColors.Window;
            this.Zs_numInvZombieDur.Location = new Point(227, 45);
            this.Zs_numInvZombieDur.Name = "Zs_numInvZombieDur";
            this.Zs_numInvZombieDur.Size = new Size(52, 21);
            this.Zs_numInvZombieDur.TabIndex = 33;
            this.GUIToolTip.SetToolTip(this.Zs_numInvZombieDur, "How many seconds a zombie is invisible for after using /buy invisibility");
            // 
            // Zs_numInvHumanDur
            // 
            this.Zs_numInvHumanDur.BackColor = SystemColors.Window;
            this.Zs_numInvHumanDur.Location = new Point(227, 20);
            this.Zs_numInvHumanDur.Name = "Zs_numInvHumanDur";
            this.Zs_numInvHumanDur.Size = new Size(52, 21);
            this.Zs_numInvHumanDur.TabIndex = 32;
            this.GUIToolTip.SetToolTip(this.Zs_numInvHumanDur, "How many seconds a human is invisible for after using /buy invisibility");
            // 
            // Zs_numInvZombieMax
            // 
            this.Zs_numInvZombieMax.BackColor = SystemColors.Window;
            this.Zs_numInvZombieMax.Location = new Point(77, 45);
            this.Zs_numInvZombieMax.Name = "Zs_numInvZombieMax";
            this.Zs_numInvZombieMax.Size = new Size(52, 21);
            this.Zs_numInvZombieMax.TabIndex = 31;
            this.GUIToolTip.SetToolTip(this.Zs_numInvZombieMax, "Maximum number of times a zombie can use /buy invisibility in a round");
            // 
            // Zs_numInvHumanMax
            // 
            this.Zs_numInvHumanMax.BackColor = SystemColors.Window;
            this.Zs_numInvHumanMax.Location = new Point(78, 20);
            this.Zs_numInvHumanMax.Name = "Zs_numInvHumanMax";
            this.Zs_numInvHumanMax.Size = new Size(52, 21);
            this.Zs_numInvHumanMax.TabIndex = 27;
            this.GUIToolTip.SetToolTip(this.Zs_numInvHumanMax, "Maximum number of times a human can use /buy invisibility in a round");
            // 
            // Zs_lblInvZombieDur
            // 
            this.Zs_lblInvZombieDur.AutoSize = true;
            this.Zs_lblInvZombieDur.Location = new Point(129, 48);
            this.Zs_lblInvZombieDur.Name = "Zs_lblInvZombieDur";
            this.Zs_lblInvZombieDur.Size = new Size(100, 13);
            this.Zs_lblInvZombieDur.TabIndex = 30;
            this.Zs_lblInvZombieDur.Text = "times, which Last for";
            // 
            // Zs_lblInvZombieMax
            // 
            this.Zs_lblInvZombieMax.AutoSize = true;
            this.Zs_lblInvZombieMax.Location = new Point(6, 48);
            this.Zs_lblInvZombieMax.Name = "Zs_lblInvZombieMax";
            this.Zs_lblInvZombieMax.Size = new Size(72, 13);
            this.Zs_lblInvZombieMax.TabIndex = 29;
            this.Zs_lblInvZombieMax.Text = "Zombies: max";
            // 
            // Zs_lblInvHumanDur
            // 
            this.Zs_lblInvHumanDur.AutoSize = true;
            this.Zs_lblInvHumanDur.Location = new Point(129, 22);
            this.Zs_lblInvHumanDur.Name = "Zs_lblInvHumanDur";
            this.Zs_lblInvHumanDur.Size = new Size(101, 13);
            this.Zs_lblInvHumanDur.TabIndex = 28;
            this.Zs_lblInvHumanDur.Text = "times, which Last for";
            // 
            // Zs_lblInvHumanMax
            // 
            this.Zs_lblInvHumanMax.AutoSize = true;
            this.Zs_lblInvHumanMax.Location = new Point(7, 23);
            this.Zs_lblInvHumanMax.Name = "Zs_lblInvHumanMax";
            this.Zs_lblInvHumanMax.Size = new Size(71, 13);
            this.Zs_lblInvHumanMax.TabIndex = 27;
            this.Zs_lblInvHumanMax.Text = "Humans: max";
            // 
            // Zs_cbStart
            // 
            this.Zs_cbStart.AutoSize = true;
            this.Zs_cbStart.Location = new Point(11, 20);
            this.Zs_cbStart.Name = "Zs_cbStart";
            this.Zs_cbStart.Size = new Size(139, 17);
            this.Zs_cbStart.TabIndex = 22;
            this.Zs_cbStart.Text = "Start when server starts";
            this.Zs_cbStart.UseVisualStyleBackColor = true;
            // 
            // Zs_grpMaps
            // 
            this.Zs_grpMaps.Controls.Add(this.Zs_lblNotUsed);
            this.Zs_grpMaps.Controls.Add(this.Zs_lblUsed);
            this.Zs_grpMaps.Controls.Add(this.Zs_btnAdd);
            this.Zs_grpMaps.Controls.Add(this.Zs_btnRemove);
            this.Zs_grpMaps.Controls.Add(this.Zs_lstNotUsed);
            this.Zs_grpMaps.Controls.Add(this.Zs_lstUsed);
            this.Zs_grpMaps.Location = new Point(6, 59);
            this.Zs_grpMaps.Name = "Zs_grpMaps";
            this.Zs_grpMaps.Size = new Size(170, 412);
            this.Zs_grpMaps.TabIndex = 1;
            this.Zs_grpMaps.TabStop = false;
            this.Zs_grpMaps.Text = "Maps";
            // 
            // Zs_lblNotUsed
            // 
            this.Zs_lblNotUsed.AutoSize = true;
            this.Zs_lblNotUsed.Location = new Point(187, 17);
            this.Zs_lblNotUsed.Name = "Zs_lblNotUsed";
            this.Zs_lblNotUsed.Size = new Size(83, 13);
            this.Zs_lblNotUsed.TabIndex = 6;
            this.Zs_lblNotUsed.Text = "Maps Not In Use";
            // 
            // Zs_lblUsed
            // 
            this.Zs_lblUsed.AutoSize = true;
            this.Zs_lblUsed.Location = new Point(6, 17);
            this.Zs_lblUsed.Name = "Zs_lblUsed";
            this.Zs_lblUsed.Size = new Size(38, 13);
            this.Zs_lblUsed.TabIndex = 5;
            this.Zs_lblUsed.Text = "In use:";
            // 
            // Zs_btnAdd
            // 
            this.Zs_btnAdd.Location = new Point(6, 188);
            this.Zs_btnAdd.Name = "Zs_btnAdd";
            this.Zs_btnAdd.Size = new Size(77, 23);
            this.Zs_btnAdd.TabIndex = 4;
            this.Zs_btnAdd.Text = "<< Add";
            this.Zs_btnAdd.UseVisualStyleBackColor = true;
            // 
            // Zs_btnRemove
            // 
            this.Zs_btnRemove.Location = new Point(89, 188);
            this.Zs_btnRemove.Name = "Zs_btnRemove";
            this.Zs_btnRemove.Size = new Size(75, 23);
            this.Zs_btnRemove.TabIndex = 3;
            this.Zs_btnRemove.Text = "Remove >>";
            this.Zs_btnRemove.UseVisualStyleBackColor = true;
            // 
            // Zs_lstNotUsed
            // 
            this.Zs_lstNotUsed.BackColor = SystemColors.Window;
            this.Zs_lstNotUsed.ForeColor = SystemColors.WindowText;
            this.Zs_lstNotUsed.FormattingEnabled = true;
            this.Zs_lstNotUsed.Location = new Point(6, 219);
            this.Zs_lstNotUsed.Name = "Zs_lstNotUsed";
            this.Zs_lstNotUsed.Size = new Size(158, 186);
            this.Zs_lstNotUsed.TabIndex = 2;
            // 
            // Zs_lstUsed
            // 
            this.Zs_lstUsed.BackColor = SystemColors.Window;
            this.Zs_lstUsed.ForeColor = SystemColors.WindowText;
            this.Zs_lstUsed.FormattingEnabled = true;
            this.Zs_lstUsed.Location = new Point(6, 33);
            this.Zs_lstUsed.Name = "Zs_lstUsed";
            this.Zs_lstUsed.Size = new Size(158, 147);
            this.Zs_lstUsed.TabIndex = 0;
            // 
            // TabZS_old
            // 
            this.TabZS_old.BackColor = SystemColors.Control;
            this.TabZS_old.Controls.Add(this.PropsZG);
            this.TabZS_old.Location = new Point(4, 22);
            this.TabZS_old.Name = "TabZS_old";
            this.TabZS_old.Padding = new Padding(3);
            this.TabZS_old.Size = new Size(484, 489);
            this.TabZS_old.TabIndex = 1;
            this.TabZS_old.Text = "Zombie old";
            // 
            // PropsZG
            // 
            this.PropsZG.Location = new Point(6, 3);
            this.PropsZG.Name = "PropsZG";
            this.PropsZG.Size = new Size(456, 464);
            this.PropsZG.TabIndex = 42;
            this.PropsZG.ToolbarVisible = false;
            // 
            // TabCTF
            // 
            this.TabCTF.BackColor = SystemColors.Control;
            this.TabCTF.Controls.Add(this.Ctf_grpControls);
            this.TabCTF.Controls.Add(this.Ctf_grpSettings);
            this.TabCTF.Controls.Add(this.Ctf_grpMaps);
            this.TabCTF.Location = new Point(4, 22);
            this.TabCTF.Name = "TabCTF";
            this.TabCTF.Size = new Size(484, 489);
            this.TabCTF.TabIndex = 3;
            this.TabCTF.Text = "CTF";
            // 
            // Ctf_grpControls
            // 
            this.Ctf_grpControls.Controls.Add(this.Ctf_btnEnd);
            this.Ctf_grpControls.Controls.Add(this.Ctf_btnStop);
            this.Ctf_grpControls.Controls.Add(this.Ctf_btnStart);
            this.Ctf_grpControls.Location = new Point(110, 5);
            this.Ctf_grpControls.Name = "Ctf_grpControls";
            this.Ctf_grpControls.Size = new Size(279, 51);
            this.Ctf_grpControls.TabIndex = 7;
            this.Ctf_grpControls.TabStop = false;
            this.Ctf_grpControls.Text = "Controls";
            // 
            // Ctf_btnEnd
            // 
            this.Ctf_btnEnd.Location = new Point(190, 19);
            this.Ctf_btnEnd.Name = "Ctf_btnEnd";
            this.Ctf_btnEnd.Size = new Size(80, 23);
            this.Ctf_btnEnd.TabIndex = 2;
            this.Ctf_btnEnd.Text = "End Round";
            this.Ctf_btnEnd.UseVisualStyleBackColor = true;
            // 
            // Ctf_btnStop
            // 
            this.Ctf_btnStop.Location = new Point(100, 19);
            this.Ctf_btnStop.Name = "Ctf_btnStop";
            this.Ctf_btnStop.Size = new Size(80, 23);
            this.Ctf_btnStop.TabIndex = 1;
            this.Ctf_btnStop.Text = "Stop Game";
            this.Ctf_btnStop.UseVisualStyleBackColor = true;
            // 
            // Ctf_btnStart
            // 
            this.Ctf_btnStart.Location = new Point(10, 19);
            this.Ctf_btnStart.Name = "Ctf_btnStart";
            this.Ctf_btnStart.Size = new Size(80, 23);
            this.Ctf_btnStart.TabIndex = 0;
            this.Ctf_btnStart.Text = "Start Game";
            this.Ctf_btnStart.UseVisualStyleBackColor = true;
            // 
            // Ctf_grpSettings
            // 
            this.Ctf_grpSettings.Controls.Add(this.Ctf_cbMain);
            this.Ctf_grpSettings.Controls.Add(this.Ctf_cbMap);
            this.Ctf_grpSettings.Controls.Add(this.Ctf_cbStart);
            this.Ctf_grpSettings.Location = new Point(182, 59);
            this.Ctf_grpSettings.Name = "Ctf_grpSettings";
            this.Ctf_grpSettings.Size = new Size(296, 89);
            this.Ctf_grpSettings.TabIndex = 6;
            this.Ctf_grpSettings.TabStop = false;
            this.Ctf_grpSettings.Text = "Settings";
            // 
            // Ctf_cbMain
            // 
            this.Ctf_cbMain.AutoSize = true;
            this.Ctf_cbMain.Location = new Point(11, 66);
            this.Ctf_cbMain.Name = "Ctf_cbMain";
            this.Ctf_cbMain.Size = new Size(112, 17);
            this.Ctf_cbMain.TabIndex = 24;
            this.Ctf_cbMain.Text = "Change main level";
            this.Ctf_cbMain.UseVisualStyleBackColor = true;
            // 
            // Ctf_cbMap
            // 
            this.Ctf_cbMap.AutoSize = true;
            this.Ctf_cbMap.Location = new Point(11, 43);
            this.Ctf_cbMap.Name = "Ctf_cbMap";
            this.Ctf_cbMap.Size = new Size(136, 17);
            this.Ctf_cbMap.TabIndex = 23;
            this.Ctf_cbMap.Text = "Map name in server list";
            this.Ctf_cbMap.UseVisualStyleBackColor = true;
            // 
            // Ctf_cbStart
            // 
            this.Ctf_cbStart.AutoSize = true;
            this.Ctf_cbStart.Location = new Point(11, 20);
            this.Ctf_cbStart.Name = "Ctf_cbStart";
            this.Ctf_cbStart.Size = new Size(139, 17);
            this.Ctf_cbStart.TabIndex = 22;
            this.Ctf_cbStart.Text = "Start when server starts";
            this.Ctf_cbStart.UseVisualStyleBackColor = true;
            // 
            // Ctf_grpMaps
            // 
            this.Ctf_grpMaps.Controls.Add(this.Ctf_lblNotUsed);
            this.Ctf_grpMaps.Controls.Add(this.Ctf_lblUsed);
            this.Ctf_grpMaps.Controls.Add(this.Ctf_btnAdd);
            this.Ctf_grpMaps.Controls.Add(this.Ctf_btnRemove);
            this.Ctf_grpMaps.Controls.Add(this.Ctf_lstNotUsed);
            this.Ctf_grpMaps.Controls.Add(this.Ctf_lstUsed);
            this.Ctf_grpMaps.Location = new Point(6, 59);
            this.Ctf_grpMaps.Name = "Ctf_grpMaps";
            this.Ctf_grpMaps.Size = new Size(170, 412);
            this.Ctf_grpMaps.TabIndex = 5;
            this.Ctf_grpMaps.TabStop = false;
            this.Ctf_grpMaps.Text = "Maps";
            // 
            // Ctf_lblNotUsed
            // 
            this.Ctf_lblNotUsed.AutoSize = true;
            this.Ctf_lblNotUsed.Location = new Point(187, 17);
            this.Ctf_lblNotUsed.Name = "Ctf_lblNotUsed";
            this.Ctf_lblNotUsed.Size = new Size(83, 13);
            this.Ctf_lblNotUsed.TabIndex = 6;
            this.Ctf_lblNotUsed.Text = "Maps Not In Use";
            // 
            // Ctf_lblUsed
            // 
            this.Ctf_lblUsed.AutoSize = true;
            this.Ctf_lblUsed.Location = new Point(6, 17);
            this.Ctf_lblUsed.Name = "Ctf_lblUsed";
            this.Ctf_lblUsed.Size = new Size(38, 13);
            this.Ctf_lblUsed.TabIndex = 5;
            this.Ctf_lblUsed.Text = "In use:";
            // 
            // Ctf_btnAdd
            // 
            this.Ctf_btnAdd.Location = new Point(6, 188);
            this.Ctf_btnAdd.Name = "Ctf_btnAdd";
            this.Ctf_btnAdd.Size = new Size(77, 23);
            this.Ctf_btnAdd.TabIndex = 4;
            this.Ctf_btnAdd.Text = "<< Add";
            this.Ctf_btnAdd.UseVisualStyleBackColor = true;
            // 
            // Ctf_btnRemove
            // 
            this.Ctf_btnRemove.Location = new Point(89, 188);
            this.Ctf_btnRemove.Name = "Ctf_btnRemove";
            this.Ctf_btnRemove.Size = new Size(75, 23);
            this.Ctf_btnRemove.TabIndex = 3;
            this.Ctf_btnRemove.Text = "Remove >>";
            this.Ctf_btnRemove.UseVisualStyleBackColor = true;
            // 
            // Ctf_lstNotUsed
            // 
            this.Ctf_lstNotUsed.BackColor = SystemColors.Window;
            this.Ctf_lstNotUsed.ForeColor = SystemColors.WindowText;
            this.Ctf_lstNotUsed.FormattingEnabled = true;
            this.Ctf_lstNotUsed.Location = new Point(6, 219);
            this.Ctf_lstNotUsed.Name = "Ctf_lstNotUsed";
            this.Ctf_lstNotUsed.Size = new Size(158, 186);
            this.Ctf_lstNotUsed.TabIndex = 2;
            // 
            // Ctf_lstUsed
            // 
            this.Ctf_lstUsed.BackColor = SystemColors.Window;
            this.Ctf_lstUsed.ForeColor = SystemColors.WindowText;
            this.Ctf_lstUsed.FormattingEnabled = true;
            this.Ctf_lstUsed.Location = new Point(6, 33);
            this.Ctf_lstUsed.Name = "Ctf_lstUsed";
            this.Ctf_lstUsed.Size = new Size(158, 147);
            this.Ctf_lstUsed.TabIndex = 0;
            // 
            // TabTW
            // 
            this.TabTW.BackColor = SystemColors.Control;
            this.TabTW.Controls.Add(this.Tw_grpControls);
            this.TabTW.Controls.Add(this.Tw_grpMapSettings);
            this.TabTW.Controls.Add(this.Tw_grpSettings);
            this.TabTW.Controls.Add(this.Tw_gbMaps);
            this.TabTW.Location = new Point(4, 22);
            this.TabTW.Name = "TabTW";
            this.TabTW.Size = new Size(484, 489);
            this.TabTW.TabIndex = 4;
            this.TabTW.Text = "TNT Wars";
            // 
            // Tw_grpControls
            // 
            this.Tw_grpControls.Controls.Add(this.Tw_btnEnd);
            this.Tw_grpControls.Controls.Add(this.Tw_btnStop);
            this.Tw_grpControls.Controls.Add(this.Tw_btnStart);
            this.Tw_grpControls.Location = new Point(110, 5);
            this.Tw_grpControls.Name = "Tw_grpControls";
            this.Tw_grpControls.Size = new Size(279, 51);
            this.Tw_grpControls.TabIndex = 8;
            this.Tw_grpControls.TabStop = false;
            this.Tw_grpControls.Text = "Controls";
            // 
            // Tw_btnEnd
            // 
            this.Tw_btnEnd.Location = new Point(190, 19);
            this.Tw_btnEnd.Name = "Tw_btnEnd";
            this.Tw_btnEnd.Size = new Size(80, 23);
            this.Tw_btnEnd.TabIndex = 2;
            this.Tw_btnEnd.Text = "End Round";
            this.Tw_btnEnd.UseVisualStyleBackColor = true;
            // 
            // Tw_btnStop
            // 
            this.Tw_btnStop.Location = new Point(100, 19);
            this.Tw_btnStop.Name = "Tw_btnStop";
            this.Tw_btnStop.Size = new Size(80, 23);
            this.Tw_btnStop.TabIndex = 1;
            this.Tw_btnStop.Text = "Stop Game";
            this.Tw_btnStop.UseVisualStyleBackColor = true;
            // 
            // Tw_btnStart
            // 
            this.Tw_btnStart.Location = new Point(10, 19);
            this.Tw_btnStart.Name = "Tw_btnStart";
            this.Tw_btnStart.Size = new Size(80, 23);
            this.Tw_btnStart.TabIndex = 0;
            this.Tw_btnStart.Text = "Start Game";
            this.Tw_btnStart.UseVisualStyleBackColor = true;
            // 
            // Tw_grpMapSettings
            // 
            this.Tw_grpMapSettings.Controls.Add(this.Tw_grpTeams);
            this.Tw_grpMapSettings.Controls.Add(this.Tw_grpGrace);
            this.Tw_grpMapSettings.Controls.Add(this.Tw_grpScores);
            this.Tw_grpMapSettings.Enabled = false;
            this.Tw_grpMapSettings.Location = new Point(182, 213);
            this.Tw_grpMapSettings.Name = "Tw_grpMapSettings";
            this.Tw_grpMapSettings.Size = new Size(296, 207);
            this.Tw_grpMapSettings.TabIndex = 7;
            this.Tw_grpMapSettings.TabStop = false;
            this.Tw_grpMapSettings.Text = "Map Settings";
            // 
            // Tw_grpTeams
            // 
            this.Tw_grpTeams.Controls.Add(this.Tw_cbKills);
            this.Tw_grpTeams.Controls.Add(this.Tw_cbBalance);
            this.Tw_grpTeams.Location = new Point(172, 125);
            this.Tw_grpTeams.Name = "Tw_grpTeams";
            this.Tw_grpTeams.Size = new Size(118, 73);
            this.Tw_grpTeams.TabIndex = 8;
            this.Tw_grpTeams.TabStop = false;
            this.Tw_grpTeams.Text = "Teams:";
            // 
            // Tw_cbKills
            // 
            this.Tw_cbKills.AutoSize = true;
            this.Tw_cbKills.Location = new Point(6, 43);
            this.Tw_cbKills.Name = "Tw_cbKills";
            this.Tw_cbKills.Size = new Size(81, 17);
            this.Tw_cbKills.TabIndex = 2;
            this.Tw_cbKills.Text = "Team killing";
            this.Tw_cbKills.UseVisualStyleBackColor = true;
            // 
            // Tw_cbBalance
            // 
            this.Tw_cbBalance.AutoSize = true;
            this.Tw_cbBalance.Checked = true;
            this.Tw_cbBalance.CheckState = CheckState.Checked;
            this.Tw_cbBalance.Location = new Point(6, 20);
            this.Tw_cbBalance.Name = "Tw_cbBalance";
            this.Tw_cbBalance.Size = new Size(96, 17);
            this.Tw_cbBalance.TabIndex = 1;
            this.Tw_cbBalance.Text = "Balance teams";
            this.Tw_cbBalance.UseVisualStyleBackColor = true;
            // 
            // Tw_grpGrace
            // 
            this.Tw_grpGrace.Controls.Add(this.Tw_numGrace);
            this.Tw_grpGrace.Controls.Add(this.Tw_lblGrace);
            this.Tw_grpGrace.Controls.Add(this.Tw_cbGrace);
            this.Tw_grpGrace.Location = new Point(6, 125);
            this.Tw_grpGrace.Name = "Tw_grpGrace";
            this.Tw_grpGrace.Size = new Size(160, 73);
            this.Tw_grpGrace.TabIndex = 7;
            this.Tw_grpGrace.TabStop = false;
            this.Tw_grpGrace.Text = "Grace Period";
            // 
            // Tw_numGrace
            // 
            this.Tw_numGrace.BackColor = SystemColors.Window;
            this.Tw_numGrace.Location = new Point(59, 41);
            this.Tw_numGrace.Name = "Tw_numGrace";
            this.Tw_numGrace.Seconds = ((long)(30));
            this.Tw_numGrace.Size = new Size(66, 21);
            this.Tw_numGrace.TabIndex = 35;
            this.Tw_numGrace.Text = "30s";
            this.Tw_numGrace.Value = TimeSpan.Parse("00:00:30");
            // 
            // Tw_lblGrace
            // 
            this.Tw_lblGrace.AutoSize = true;
            this.Tw_lblGrace.Location = new Point(23, 44);
            this.Tw_lblGrace.Name = "Tw_lblGrace";
            this.Tw_lblGrace.Size = new Size(33, 13);
            this.Tw_lblGrace.TabIndex = 2;
            this.Tw_lblGrace.Text = "Time:";
            // 
            // Tw_cbGrace
            // 
            this.Tw_cbGrace.AutoSize = true;
            this.Tw_cbGrace.Checked = true;
            this.Tw_cbGrace.CheckState = CheckState.Checked;
            this.Tw_cbGrace.Location = new Point(6, 20);
            this.Tw_cbGrace.Name = "Tw_cbGrace";
            this.Tw_cbGrace.Size = new Size(64, 17);
            this.Tw_cbGrace.TabIndex = 0;
            this.Tw_cbGrace.Text = "Enabled";
            this.Tw_cbGrace.UseVisualStyleBackColor = true;
            // 
            // Tw_grpScores
            // 
            this.Tw_grpScores.Controls.Add(this.Tw_lblMulti);
            this.Tw_grpScores.Controls.Add(this.Tw_lblAssist);
            this.Tw_grpScores.Controls.Add(this.Tw_cbStreaks);
            this.Tw_grpScores.Controls.Add(this.Tw_numMultiKills);
            this.Tw_grpScores.Controls.Add(this.Tw_numScoreAssists);
            this.Tw_grpScores.Controls.Add(this.Tw_lblScorePerKill);
            this.Tw_grpScores.Controls.Add(this.Tw_numScorePerKill);
            this.Tw_grpScores.Controls.Add(this.Tw_lblScoreLimit);
            this.Tw_grpScores.Controls.Add(this.Tw_numScoreLimit);
            this.Tw_grpScores.Location = new Point(6, 20);
            this.Tw_grpScores.Name = "Tw_grpScores";
            this.Tw_grpScores.Size = new Size(284, 99);
            this.Tw_grpScores.TabIndex = 6;
            this.Tw_grpScores.TabStop = false;
            this.Tw_grpScores.Text = "Scores";
            // 
            // Tw_lblMulti
            // 
            this.Tw_lblMulti.AutoSize = true;
            this.Tw_lblMulti.Location = new Point(157, 47);
            this.Tw_lblMulti.Name = "Tw_lblMulti";
            this.Tw_lblMulti.Size = new Size(79, 13);
            this.Tw_lblMulti.TabIndex = 10;
            this.Tw_lblMulti.Text = "Multikill bonus:";
            // 
            // Tw_lblAssist
            // 
            this.Tw_lblAssist.AutoSize = true;
            this.Tw_lblAssist.Location = new Point(168, 20);
            this.Tw_lblAssist.Name = "Tw_lblAssist";
            this.Tw_lblAssist.Size = new Size(69, 13);
            this.Tw_lblAssist.TabIndex = 9;
            this.Tw_lblAssist.Text = "Assist bonus:";
            // 
            // Tw_cbStreaks
            // 
            this.Tw_cbStreaks.AutoSize = true;
            this.Tw_cbStreaks.Checked = true;
            this.Tw_cbStreaks.CheckState = CheckState.Checked;
            this.Tw_cbStreaks.Location = new Point(11, 72);
            this.Tw_cbStreaks.Name = "Tw_cbStreaks";
            this.Tw_cbStreaks.Size = new Size(61, 17);
            this.Tw_cbStreaks.TabIndex = 0;
            this.Tw_cbStreaks.Text = "Streaks";
            this.Tw_cbStreaks.UseVisualStyleBackColor = true;
            // 
            // Tw_numMultiKills
            // 
            this.Tw_numMultiKills.BackColor = SystemColors.Window;
            this.Tw_numMultiKills.Location = new Point(240, 44);
            this.Tw_numMultiKills.Maximum = new decimal(new int[] {
                                    100000,
                                    0,
                                    0,
                                    0});
            this.Tw_numMultiKills.Name = "Tw_numMultiKills";
            this.Tw_numMultiKills.Size = new Size(38, 21);
            this.Tw_numMultiKills.TabIndex = 7;
            this.Tw_numMultiKills.Value = new decimal(new int[] {
                                    5,
                                    0,
                                    0,
                                    0});
            // 
            // Tw_numScoreAssists
            // 
            this.Tw_numScoreAssists.BackColor = SystemColors.Window;
            this.Tw_numScoreAssists.Location = new Point(240, 17);
            this.Tw_numScoreAssists.Maximum = new decimal(new int[] {
                                    100000,
                                    0,
                                    0,
                                    0});
            this.Tw_numScoreAssists.Name = "Tw_numScoreAssists";
            this.Tw_numScoreAssists.Size = new Size(38, 21);
            this.Tw_numScoreAssists.TabIndex = 4;
            this.Tw_numScoreAssists.Value = new decimal(new int[] {
                                    5,
                                    0,
                                    0,
                                    0});
            // 
            // Tw_lblScorePerKill
            // 
            this.Tw_lblScorePerKill.AutoSize = true;
            this.Tw_lblScorePerKill.Location = new Point(9, 46);
            this.Tw_lblScorePerKill.Name = "Tw_lblScorePerKill";
            this.Tw_lblScorePerKill.Size = new Size(70, 13);
            this.Tw_lblScorePerKill.TabIndex = 3;
            this.Tw_lblScorePerKill.Text = "Score per kill:";
            // 
            // Tw_numScorePerKill
            // 
            this.Tw_numScorePerKill.BackColor = SystemColors.Window;
            this.Tw_numScorePerKill.Location = new Point(82, 44);
            this.Tw_numScorePerKill.Maximum = new decimal(new int[] {
                                    100000,
                                    0,
                                    0,
                                    0});
            this.Tw_numScorePerKill.Name = "Tw_numScorePerKill";
            this.Tw_numScorePerKill.Size = new Size(50, 21);
            this.Tw_numScorePerKill.TabIndex = 2;
            this.Tw_numScorePerKill.Value = new decimal(new int[] {
                                    10,
                                    0,
                                    0,
                                    0});
            // 
            // Tw_lblScoreLimit
            // 
            this.Tw_lblScoreLimit.AutoSize = true;
            this.Tw_lblScoreLimit.Location = new Point(5, 19);
            this.Tw_lblScoreLimit.Name = "Tw_lblScoreLimit";
            this.Tw_lblScoreLimit.Size = new Size(74, 13);
            this.Tw_lblScoreLimit.TabIndex = 1;
            this.Tw_lblScoreLimit.Text = "Score needed:";
            // 
            // Tw_numScoreLimit
            // 
            this.Tw_numScoreLimit.BackColor = SystemColors.Window;
            this.Tw_numScoreLimit.Location = new Point(82, 17);
            this.Tw_numScoreLimit.Maximum = new decimal(new int[] {
                                    100000,
                                    0,
                                    0,
                                    0});
            this.Tw_numScoreLimit.Name = "Tw_numScoreLimit";
            this.Tw_numScoreLimit.Size = new Size(50, 21);
            this.Tw_numScoreLimit.TabIndex = 0;
            this.Tw_numScoreLimit.Value = new decimal(new int[] {
                                    150,
                                    0,
                                    0,
                                    0});
            // 
            // Tw_grpSettings
            // 
            this.Tw_grpSettings.Controls.Add(this.Tw_cmbMode);
            this.Tw_grpSettings.Controls.Add(this.Tw_cmbDiff);
            this.Tw_grpSettings.Controls.Add(this.Tw_lblMode);
            this.Tw_grpSettings.Controls.Add(this.Tw_lblDiff);
            this.Tw_grpSettings.Controls.Add(this.Tw_cbMain);
            this.Tw_grpSettings.Controls.Add(this.Tw_cbMap);
            this.Tw_grpSettings.Controls.Add(this.Tw_cbStart);
            this.Tw_grpSettings.Location = new Point(182, 59);
            this.Tw_grpSettings.Name = "Tw_grpSettings";
            this.Tw_grpSettings.Size = new Size(296, 148);
            this.Tw_grpSettings.TabIndex = 6;
            this.Tw_grpSettings.TabStop = false;
            this.Tw_grpSettings.Text = "Settings";
            // 
            // Tw_cmbMode
            // 
            this.Tw_cmbMode.BackColor = SystemColors.Window;
            this.Tw_cmbMode.FormattingEnabled = true;
            this.Tw_cmbMode.Items.AddRange(new object[] {
                                    "FFA",
                                    "TDM"});
            this.Tw_cmbMode.Location = new Point(74, 116);
            this.Tw_cmbMode.Name = "Tw_cmbMode";
            this.Tw_cmbMode.Size = new Size(76, 21);
            this.Tw_cmbMode.TabIndex = 29;
            // 
            // Tw_cmbDiff
            // 
            this.Tw_cmbDiff.BackColor = SystemColors.Window;
            this.Tw_cmbDiff.FormattingEnabled = true;
            this.Tw_cmbDiff.Items.AddRange(new object[] {
                                    "Easy",
                                    "Normal",
                                    "Hard",
                                    "Extreme"});
            this.Tw_cmbDiff.Location = new Point(74, 89);
            this.Tw_cmbDiff.Name = "Tw_cmbDiff";
            this.Tw_cmbDiff.Size = new Size(76, 21);
            this.Tw_cmbDiff.TabIndex = 28;
            this.GUIToolTip.SetToolTip(this.Tw_cmbDiff, "Easy (2 Hits to die, TNT has long delay)\nNormal (2 Hits to die, TNT has normal delay)\n" +
                                    "Hard (1 Hit to die, TNT has short delay and team kills on)\nExtreme (1 Hit to die, TNT has short delay, big explosion and team kills on)");
            // 
            // Tw_lblMode
            // 
            this.Tw_lblMode.AutoSize = true;
            this.Tw_lblMode.Location = new Point(8, 119);
            this.Tw_lblMode.Name = "Tw_lblMode";
            this.Tw_lblMode.Size = new Size(65, 13);
            this.Tw_lblMode.TabIndex = 27;
            this.Tw_lblMode.Text = "Gamemode:";
            // 
            // Tw_lblDiff
            // 
            this.Tw_lblDiff.AutoSize = true;
            this.Tw_lblDiff.Location = new Point(21, 94);
            this.Tw_lblDiff.Name = "Tw_lblDiff";
            this.Tw_lblDiff.Size = new Size(52, 13);
            this.Tw_lblDiff.TabIndex = 26;
            this.Tw_lblDiff.Text = "Difficulty:";
            // 
            // Tw_cbMain
            // 
            this.Tw_cbMain.AutoSize = true;
            this.Tw_cbMain.Location = new Point(11, 66);
            this.Tw_cbMain.Name = "Tw_cbMain";
            this.Tw_cbMain.Size = new Size(112, 17);
            this.Tw_cbMain.TabIndex = 24;
            this.Tw_cbMain.Text = "Change main level";
            this.Tw_cbMain.UseVisualStyleBackColor = true;
            // 
            // Tw_cbMap
            // 
            this.Tw_cbMap.AutoSize = true;
            this.Tw_cbMap.Location = new Point(11, 43);
            this.Tw_cbMap.Name = "Tw_cbMap";
            this.Tw_cbMap.Size = new Size(136, 17);
            this.Tw_cbMap.TabIndex = 23;
            this.Tw_cbMap.Text = "Map name in server list";
            this.Tw_cbMap.UseVisualStyleBackColor = true;
            // 
            // Tw_cbStart
            // 
            this.Tw_cbStart.AutoSize = true;
            this.Tw_cbStart.Location = new Point(11, 20);
            this.Tw_cbStart.Name = "Tw_cbStart";
            this.Tw_cbStart.Size = new Size(139, 17);
            this.Tw_cbStart.TabIndex = 22;
            this.Tw_cbStart.Text = "Start when server starts";
            this.Tw_cbStart.UseVisualStyleBackColor = true;
            // 
            // Tw_gbMaps
            // 
            this.Tw_gbMaps.Controls.Add(this.Tw_lblInUse);
            this.Tw_gbMaps.Controls.Add(this.Tw_btnAdd);
            this.Tw_gbMaps.Controls.Add(this.Tw_btnRemove);
            this.Tw_gbMaps.Controls.Add(this.Tw_lstNotUsed);
            this.Tw_gbMaps.Controls.Add(this.Tw_lstUsed);
            this.Tw_gbMaps.Location = new Point(6, 59);
            this.Tw_gbMaps.Name = "Tw_gbMaps";
            this.Tw_gbMaps.Size = new Size(170, 412);
            this.Tw_gbMaps.TabIndex = 5;
            this.Tw_gbMaps.TabStop = false;
            this.Tw_gbMaps.Text = "Maps";
            // 
            // Tw_lblInUse
            // 
            this.Tw_lblInUse.AutoSize = true;
            this.Tw_lblInUse.Location = new Point(6, 17);
            this.Tw_lblInUse.Name = "Tw_lblInUse";
            this.Tw_lblInUse.Size = new Size(38, 13);
            this.Tw_lblInUse.TabIndex = 5;
            this.Tw_lblInUse.Text = "In use:";
            // 
            // Tw_btnAdd
            // 
            this.Tw_btnAdd.Location = new Point(6, 188);
            this.Tw_btnAdd.Name = "Tw_btnAdd";
            this.Tw_btnAdd.Size = new Size(77, 23);
            this.Tw_btnAdd.TabIndex = 4;
            this.Tw_btnAdd.Text = "<< Add";
            this.Tw_btnAdd.UseVisualStyleBackColor = true;
            // 
            // Tw_btnRemove
            // 
            this.Tw_btnRemove.Location = new Point(89, 188);
            this.Tw_btnRemove.Name = "Tw_btnRemove";
            this.Tw_btnRemove.Size = new Size(75, 23);
            this.Tw_btnRemove.TabIndex = 3;
            this.Tw_btnRemove.Text = "Remove >>";
            this.Tw_btnRemove.UseVisualStyleBackColor = true;
            // 
            // Tw_lstNotUsed
            // 
            this.Tw_lstNotUsed.BackColor = SystemColors.Window;
            this.Tw_lstNotUsed.ForeColor = SystemColors.WindowText;
            this.Tw_lstNotUsed.FormattingEnabled = true;
            this.Tw_lstNotUsed.Location = new Point(6, 219);
            this.Tw_lstNotUsed.Name = "Tw_lstNotUsed";
            this.Tw_lstNotUsed.Size = new Size(158, 186);
            this.Tw_lstNotUsed.TabIndex = 2;
            // 
            // Tw_lstUsed
            // 
            this.Tw_lstUsed.BackColor = SystemColors.Window;
            this.Tw_lstUsed.ForeColor = SystemColors.WindowText;
            this.Tw_lstUsed.FormattingEnabled = true;
            this.Tw_lstUsed.Location = new Point(6, 33);
            this.Tw_lstUsed.Name = "Tw_lstUsed";
            this.Tw_lstUsed.Size = new Size(158, 147);
            this.Tw_lstUsed.TabIndex = 0;
            this.Tw_lstUsed.SelectedIndexChanged += new EventHandler(this.TwMapUse_SelectedIndexChanged);
            // 
            // TabCD
            // 
            this.TabCD.BackColor = SystemColors.Control;
            this.TabCD.Controls.Add(this.Cd_grpControls);
            this.TabCD.Controls.Add(this.Cd_grpSettings);
            this.TabCD.Controls.Add(this.Cd_grpMaps);
            this.TabCD.Location = new Point(4, 22);
            this.TabCD.Name = "TabCD";
            this.TabCD.Size = new Size(484, 489);
            this.TabCD.TabIndex = 3;
            this.TabCD.Text = "Countdown";
            // 
            // Cd_grpControls
            // 
            this.Cd_grpControls.Controls.Add(this.Cd_btnEnd);
            this.Cd_grpControls.Controls.Add(this.Cd_btnStop);
            this.Cd_grpControls.Controls.Add(this.Cd_btnStart);
            this.Cd_grpControls.Location = new Point(110, 5);
            this.Cd_grpControls.Name = "Cd_grpControls";
            this.Cd_grpControls.Size = new Size(279, 51);
            this.Cd_grpControls.TabIndex = 7;
            this.Cd_grpControls.TabStop = false;
            this.Cd_grpControls.Text = "Controls";
            // 
            // Cd_btnEnd
            // 
            this.Cd_btnEnd.Location = new Point(190, 19);
            this.Cd_btnEnd.Name = "Cd_btnEnd";
            this.Cd_btnEnd.Size = new Size(80, 23);
            this.Cd_btnEnd.TabIndex = 2;
            this.Cd_btnEnd.Text = "End Round";
            this.Cd_btnEnd.UseVisualStyleBackColor = true;
            // 
            // Cd_btnStop
            // 
            this.Cd_btnStop.Location = new Point(100, 19);
            this.Cd_btnStop.Name = "Cd_btnStop";
            this.Cd_btnStop.Size = new Size(80, 23);
            this.Cd_btnStop.TabIndex = 1;
            this.Cd_btnStop.Text = "Stop Game";
            this.Cd_btnStop.UseVisualStyleBackColor = true;
            // 
            // Cd_btnStart
            // 
            this.Cd_btnStart.Location = new Point(10, 19);
            this.Cd_btnStart.Name = "Cd_btnStart";
            this.Cd_btnStart.Size = new Size(80, 23);
            this.Cd_btnStart.TabIndex = 0;
            this.Cd_btnStart.Text = "Start Game";
            this.Cd_btnStart.UseVisualStyleBackColor = true;
            // 
            // Cd_grpSettings
            // 
            this.Cd_grpSettings.Controls.Add(this.Cd_cbMain);
            this.Cd_grpSettings.Controls.Add(this.Cd_cbMap);
            this.Cd_grpSettings.Controls.Add(this.Cd_cbStart);
            this.Cd_grpSettings.Location = new Point(182, 59);
            this.Cd_grpSettings.Name = "Cd_grpSettings";
            this.Cd_grpSettings.Size = new Size(296, 89);
            this.Cd_grpSettings.TabIndex = 6;
            this.Cd_grpSettings.TabStop = false;
            this.Cd_grpSettings.Text = "Settings";
            // 
            // Cd_cbMain
            // 
            this.Cd_cbMain.AutoSize = true;
            this.Cd_cbMain.Location = new Point(11, 66);
            this.Cd_cbMain.Name = "Cd_cbMain";
            this.Cd_cbMain.Size = new Size(112, 17);
            this.Cd_cbMain.TabIndex = 24;
            this.Cd_cbMain.Text = "Change main level";
            this.Cd_cbMain.UseVisualStyleBackColor = true;
            // 
            // Cd_cbMap
            // 
            this.Cd_cbMap.AutoSize = true;
            this.Cd_cbMap.Location = new Point(11, 43);
            this.Cd_cbMap.Name = "Cd_cbMap";
            this.Cd_cbMap.Size = new Size(136, 17);
            this.Cd_cbMap.TabIndex = 23;
            this.Cd_cbMap.Text = "Map name in server list";
            this.Cd_cbMap.UseVisualStyleBackColor = true;
            // 
            // Cd_cbStart
            // 
            this.Cd_cbStart.AutoSize = true;
            this.Cd_cbStart.Location = new Point(11, 20);
            this.Cd_cbStart.Name = "Cd_cbStart";
            this.Cd_cbStart.Size = new Size(139, 17);
            this.Cd_cbStart.TabIndex = 22;
            this.Cd_cbStart.Text = "Start when server starts";
            this.Cd_cbStart.UseVisualStyleBackColor = true;
            // 
            // Cd_grpMaps
            // 
            this.Cd_grpMaps.Controls.Add(this.Cd_lblNotUsed);
            this.Cd_grpMaps.Controls.Add(this.Cd_lblUsed);
            this.Cd_grpMaps.Controls.Add(this.Cd_btnAdd);
            this.Cd_grpMaps.Controls.Add(this.Cd_btnRemove);
            this.Cd_grpMaps.Controls.Add(this.Cd_lstNotUsed);
            this.Cd_grpMaps.Controls.Add(this.Cd_lstUsed);
            this.Cd_grpMaps.Location = new Point(6, 59);
            this.Cd_grpMaps.Name = "Cd_grpMaps";
            this.Cd_grpMaps.Size = new Size(170, 412);
            this.Cd_grpMaps.TabIndex = 5;
            this.Cd_grpMaps.TabStop = false;
            this.Cd_grpMaps.Text = "Maps";
            // 
            // Cd_lblNotUsed
            // 
            this.Cd_lblNotUsed.AutoSize = true;
            this.Cd_lblNotUsed.Location = new Point(187, 17);
            this.Cd_lblNotUsed.Name = "Cd_lblNotUsed";
            this.Cd_lblNotUsed.Size = new Size(83, 13);
            this.Cd_lblNotUsed.TabIndex = 6;
            this.Cd_lblNotUsed.Text = "Maps Not In Use";
            // 
            // Cd_lblUsed
            // 
            this.Cd_lblUsed.AutoSize = true;
            this.Cd_lblUsed.Location = new Point(6, 17);
            this.Cd_lblUsed.Name = "Cd_lblUsed";
            this.Cd_lblUsed.Size = new Size(38, 13);
            this.Cd_lblUsed.TabIndex = 5;
            this.Cd_lblUsed.Text = "In use:";
            // 
            // Cd_btnAdd
            // 
            this.Cd_btnAdd.Location = new Point(6, 188);
            this.Cd_btnAdd.Name = "Cd_btnAdd";
            this.Cd_btnAdd.Size = new Size(77, 23);
            this.Cd_btnAdd.TabIndex = 4;
            this.Cd_btnAdd.Text = "<< Add";
            this.Cd_btnAdd.UseVisualStyleBackColor = true;
            // 
            // Cd_btnRemove
            // 
            this.Cd_btnRemove.Location = new Point(89, 188);
            this.Cd_btnRemove.Name = "Cd_btnRemove";
            this.Cd_btnRemove.Size = new Size(75, 23);
            this.Cd_btnRemove.TabIndex = 3;
            this.Cd_btnRemove.Text = "Remove >>";
            this.Cd_btnRemove.UseVisualStyleBackColor = true;
            // 
            // Cd_lstNotUsed
            // 
            this.Cd_lstNotUsed.BackColor = SystemColors.Window;
            this.Cd_lstNotUsed.ForeColor = SystemColors.WindowText;
            this.Cd_lstNotUsed.FormattingEnabled = true;
            this.Cd_lstNotUsed.Location = new Point(6, 219);
            this.Cd_lstNotUsed.Name = "Cd_lstNotUsed";
            this.Cd_lstNotUsed.Size = new Size(158, 186);
            this.Cd_lstNotUsed.TabIndex = 2;
            // 
            // Cd_lstUsed
            // 
            this.Cd_lstUsed.BackColor = SystemColors.Window;
            this.Cd_lstUsed.ForeColor = SystemColors.WindowText;
            this.Cd_lstUsed.FormattingEnabled = true;
            this.Cd_lstUsed.Location = new Point(6, 33);
            this.Cd_lstUsed.Name = "Cd_lstUsed";
            this.Cd_lstUsed.Size = new Size(158, 147);
            this.Cd_lstUsed.TabIndex = 0;
            // 
            // PageCommands
            // 
            this.PageCommands.AutoScroll = true;
            this.PageCommands.Controls.Add(this.Cmd_grpExtra);
            this.PageCommands.Controls.Add(this.Cmd_grpPermissions);
            this.PageCommands.Controls.Add(this.Cmd_btnCustom);
            this.PageCommands.Controls.Add(this.Cmd_btnHelp);
            this.PageCommands.Controls.Add(this.Cmd_list);
            this.PageCommands.Location = new Point(4, 22);
            this.PageCommands.Name = "PageCommands";
            this.PageCommands.Size = new Size(498, 521);
            this.PageCommands.TabIndex = 2;
            this.PageCommands.Text = "Commands";
            // 
            // Cmd_grpExtra
            // 
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra7);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra7);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra6);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra6);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra5);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra5);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra4);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra4);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra3);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra3);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra2);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra2);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_cmbExtra1);
            this.Cmd_grpExtra.Controls.Add(this.Cmd_lblExtra1);
            this.Cmd_grpExtra.Location = new Point(133, 105);
            this.Cmd_grpExtra.Name = "Cmd_grpExtra";
            this.Cmd_grpExtra.Size = new Size(360, 205);
            this.Cmd_grpExtra.TabIndex = 28;
            this.Cmd_grpExtra.TabStop = false;
            this.Cmd_grpExtra.Text = "Extra permissions";
            // 
            // Cmd_cmbExtra7
            // 
            this.Cmd_cmbExtra7.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra7.FormattingEnabled = true;
            this.Cmd_cmbExtra7.Location = new Point(10, 173);
            this.Cmd_cmbExtra7.Name = "Cmd_cmbExtra7";
            this.Cmd_cmbExtra7.Size = new Size(81, 21);
            this.Cmd_cmbExtra7.TabIndex = 42;
            this.Cmd_cmbExtra7.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra7
            // 
            this.Cmd_lblExtra7.AutoSize = true;
            this.Cmd_lblExtra7.Location = new Point(91, 176);
            this.Cmd_lblExtra7.Name = "Cmd_lblExtra7";
            this.Cmd_lblExtra7.Size = new Size(12, 13);
            this.Cmd_lblExtra7.TabIndex = 41;
            this.Cmd_lblExtra7.Text = "+";
            // 
            // Cmd_cmbExtra6
            // 
            this.Cmd_cmbExtra6.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra6.FormattingEnabled = true;
            this.Cmd_cmbExtra6.Location = new Point(10, 147);
            this.Cmd_cmbExtra6.Name = "Cmd_cmbExtra6";
            this.Cmd_cmbExtra6.Size = new Size(81, 21);
            this.Cmd_cmbExtra6.TabIndex = 40;
            this.Cmd_cmbExtra6.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra6
            // 
            this.Cmd_lblExtra6.AutoSize = true;
            this.Cmd_lblExtra6.Location = new Point(91, 150);
            this.Cmd_lblExtra6.Name = "Cmd_lblExtra6";
            this.Cmd_lblExtra6.Size = new Size(12, 13);
            this.Cmd_lblExtra6.TabIndex = 39;
            this.Cmd_lblExtra6.Text = "+";
            // 
            // Cmd_cmbExtra5
            // 
            this.Cmd_cmbExtra5.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra5.FormattingEnabled = true;
            this.Cmd_cmbExtra5.Location = new Point(10, 121);
            this.Cmd_cmbExtra5.Name = "Cmd_cmbExtra5";
            this.Cmd_cmbExtra5.Size = new Size(81, 21);
            this.Cmd_cmbExtra5.TabIndex = 38;
            this.Cmd_cmbExtra5.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra5
            // 
            this.Cmd_lblExtra5.AutoSize = true;
            this.Cmd_lblExtra5.Location = new Point(91, 124);
            this.Cmd_lblExtra5.Name = "Cmd_lblExtra5";
            this.Cmd_lblExtra5.Size = new Size(12, 13);
            this.Cmd_lblExtra5.TabIndex = 37;
            this.Cmd_lblExtra5.Text = "+";
            // 
            // Cmd_cmbExtra4
            // 
            this.Cmd_cmbExtra4.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra4.FormattingEnabled = true;
            this.Cmd_cmbExtra4.Location = new Point(10, 95);
            this.Cmd_cmbExtra4.Name = "Cmd_cmbExtra4";
            this.Cmd_cmbExtra4.Size = new Size(81, 21);
            this.Cmd_cmbExtra4.TabIndex = 36;
            this.Cmd_cmbExtra4.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra4
            // 
            this.Cmd_lblExtra4.AutoSize = true;
            this.Cmd_lblExtra4.Location = new Point(91, 98);
            this.Cmd_lblExtra4.Name = "Cmd_lblExtra4";
            this.Cmd_lblExtra4.Size = new Size(12, 13);
            this.Cmd_lblExtra4.TabIndex = 35;
            this.Cmd_lblExtra4.Text = "+";
            // 
            // Cmd_cmbExtra3
            // 
            this.Cmd_cmbExtra3.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra3.FormattingEnabled = true;
            this.Cmd_cmbExtra3.Location = new Point(10, 69);
            this.Cmd_cmbExtra3.Name = "Cmd_cmbExtra3";
            this.Cmd_cmbExtra3.Size = new Size(81, 21);
            this.Cmd_cmbExtra3.TabIndex = 34;
            this.Cmd_cmbExtra3.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra3
            // 
            this.Cmd_lblExtra3.AutoSize = true;
            this.Cmd_lblExtra3.Location = new Point(91, 72);
            this.Cmd_lblExtra3.Name = "Cmd_lblExtra3";
            this.Cmd_lblExtra3.Size = new Size(12, 13);
            this.Cmd_lblExtra3.TabIndex = 33;
            this.Cmd_lblExtra3.Text = "+";
            // 
            // Cmd_cmbExtra2
            // 
            this.Cmd_cmbExtra2.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra2.FormattingEnabled = true;
            this.Cmd_cmbExtra2.Location = new Point(10, 43);
            this.Cmd_cmbExtra2.Name = "Cmd_cmbExtra2";
            this.Cmd_cmbExtra2.Size = new Size(81, 21);
            this.Cmd_cmbExtra2.TabIndex = 32;
            this.Cmd_cmbExtra2.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra2
            // 
            this.Cmd_lblExtra2.AutoSize = true;
            this.Cmd_lblExtra2.Location = new Point(91, 46);
            this.Cmd_lblExtra2.Name = "Cmd_lblExtra2";
            this.Cmd_lblExtra2.Size = new Size(12, 13);
            this.Cmd_lblExtra2.TabIndex = 31;
            this.Cmd_lblExtra2.Text = "+";
            // 
            // Cmd_cmbExtra1
            // 
            this.Cmd_cmbExtra1.BackColor = SystemColors.Window;
            this.Cmd_cmbExtra1.FormattingEnabled = true;
            this.Cmd_cmbExtra1.Location = new Point(10, 17);
            this.Cmd_cmbExtra1.Name = "Cmd_cmbExtra1";
            this.Cmd_cmbExtra1.Size = new Size(81, 21);
            this.Cmd_cmbExtra1.TabIndex = 30;
            this.Cmd_cmbExtra1.SelectedIndexChanged += new EventHandler(this.Cmd_cmbExtra_SelectedIndexChanged);
            // 
            // Cmd_lblExtra1
            // 
            this.Cmd_lblExtra1.AutoSize = true;
            this.Cmd_lblExtra1.Location = new Point(91, 20);
            this.Cmd_lblExtra1.Name = "Cmd_lblExtra1";
            this.Cmd_lblExtra1.Size = new Size(12, 13);
            this.Cmd_lblExtra1.TabIndex = 29;
            this.Cmd_lblExtra1.Text = "+";
            // 
            // Cmd_grpPermissions
            // 
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbAlw3);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbAlw2);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbDis3);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbDis2);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbAlw1);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbDis1);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_cmbMin);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_lblMin);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_lblDisallow);
            this.Cmd_grpPermissions.Controls.Add(this.Cmd_lblAllow);
            this.Cmd_grpPermissions.Location = new Point(133, 6);
            this.Cmd_grpPermissions.Name = "Cmd_grpPermissions";
            this.Cmd_grpPermissions.Size = new Size(360, 94);
            this.Cmd_grpPermissions.TabIndex = 27;
            this.Cmd_grpPermissions.TabStop = false;
            this.Cmd_grpPermissions.Text = "Permissions";
            // 
            // Cmd_cmbAlw3
            // 
            this.Cmd_cmbAlw3.BackColor = SystemColors.Window;
            this.Cmd_cmbAlw3.FormattingEnabled = true;
            this.Cmd_cmbAlw3.Location = new Point(274, 67);
            this.Cmd_cmbAlw3.Name = "Cmd_cmbAlw3";
            this.Cmd_cmbAlw3.Size = new Size(81, 21);
            this.Cmd_cmbAlw3.TabIndex = 28;
            this.Cmd_cmbAlw3.SelectedIndexChanged += new EventHandler(this.Cmd_cmbSpecific_SelectedIndexChanged);
            // 
            // Cmd_cmbAlw2
            // 
            this.Cmd_cmbAlw2.BackColor = SystemColors.Window;
            this.Cmd_cmbAlw2.FormattingEnabled = true;
            this.Cmd_cmbAlw2.Location = new Point(187, 67);
            this.Cmd_cmbAlw2.Name = "Cmd_cmbAlw2";
            this.Cmd_cmbAlw2.Size = new Size(81, 21);
            this.Cmd_cmbAlw2.TabIndex = 27;
            this.Cmd_cmbAlw2.SelectedIndexChanged += new EventHandler(this.Cmd_cmbSpecific_SelectedIndexChanged);
            // 
            // Cmd_cmbDis3
            // 
            this.Cmd_cmbDis3.BackColor = SystemColors.Window;
            this.Cmd_cmbDis3.FormattingEnabled = true;
            this.Cmd_cmbDis3.Location = new Point(274, 41);
            this.Cmd_cmbDis3.Name = "Cmd_cmbDis3";
            this.Cmd_cmbDis3.Size = new Size(81, 21);
            this.Cmd_cmbDis3.TabIndex = 26;
            this.Cmd_cmbDis3.SelectedIndexChanged += new EventHandler(this.Cmd_cmbSpecific_SelectedIndexChanged);
            // 
            // Cmd_cmbDis2
            // 
            this.Cmd_cmbDis2.BackColor = SystemColors.Window;
            this.Cmd_cmbDis2.FormattingEnabled = true;
            this.Cmd_cmbDis2.Location = new Point(187, 41);
            this.Cmd_cmbDis2.Name = "Cmd_cmbDis2";
            this.Cmd_cmbDis2.Size = new Size(81, 21);
            this.Cmd_cmbDis2.TabIndex = 25;
            this.Cmd_cmbDis2.SelectedIndexChanged += new EventHandler(this.Cmd_cmbSpecific_SelectedIndexChanged);
            // 
            // Cmd_cmbAlw1
            // 
            this.Cmd_cmbAlw1.BackColor = SystemColors.Window;
            this.Cmd_cmbAlw1.FormattingEnabled = true;
            this.Cmd_cmbAlw1.Location = new Point(100, 67);
            this.Cmd_cmbAlw1.Name = "Cmd_cmbAlw1";
            this.Cmd_cmbAlw1.Size = new Size(81, 21);
            this.Cmd_cmbAlw1.TabIndex = 24;
            this.Cmd_cmbAlw1.SelectedIndexChanged += new EventHandler(this.Cmd_cmbSpecific_SelectedIndexChanged);
            // 
            // Cmd_cmbDis1
            // 
            this.Cmd_cmbDis1.BackColor = SystemColors.Window;
            this.Cmd_cmbDis1.FormattingEnabled = true;
            this.Cmd_cmbDis1.Location = new Point(100, 41);
            this.Cmd_cmbDis1.Name = "Cmd_cmbDis1";
            this.Cmd_cmbDis1.Size = new Size(81, 21);
            this.Cmd_cmbDis1.TabIndex = 23;
            this.Cmd_cmbDis1.SelectedIndexChanged += new EventHandler(this.Cmd_cmbSpecific_SelectedIndexChanged);
            // 
            // Cmd_cmbMin
            // 
            this.Cmd_cmbMin.BackColor = SystemColors.Window;
            this.Cmd_cmbMin.FormattingEnabled = true;
            this.Cmd_cmbMin.Location = new Point(100, 14);
            this.Cmd_cmbMin.Name = "Cmd_cmbMin";
            this.Cmd_cmbMin.Size = new Size(81, 21);
            this.Cmd_cmbMin.TabIndex = 22;
            this.Cmd_cmbMin.SelectedIndexChanged += new EventHandler(this.Cmd_cmbMin_SelectedIndexChanged);
            // 
            // Cmd_lblMin
            // 
            this.Cmd_lblMin.AutoSize = true;
            this.Cmd_lblMin.Location = new Point(10, 17);
            this.Cmd_lblMin.Name = "Cmd_lblMin";
            this.Cmd_lblMin.Size = new Size(89, 13);
            this.Cmd_lblMin.TabIndex = 16;
            this.Cmd_lblMin.Text = "Min rank needed:";
            // 
            // Cmd_lblDisallow
            // 
            this.Cmd_lblDisallow.AutoSize = true;
            this.Cmd_lblDisallow.Location = new Point(10, 71);
            this.Cmd_lblDisallow.Name = "Cmd_lblDisallow";
            this.Cmd_lblDisallow.Size = new Size(91, 13);
            this.Cmd_lblDisallow.TabIndex = 18;
            this.Cmd_lblDisallow.Text = "Specifically allow:";
            // 
            // Cmd_lblAllow
            // 
            this.Cmd_lblAllow.AutoSize = true;
            this.Cmd_lblAllow.Location = new Point(10, 44);
            this.Cmd_lblAllow.Name = "Cmd_lblAllow";
            this.Cmd_lblAllow.Size = new Size(80, 13);
            this.Cmd_lblAllow.TabIndex = 17;
            this.Cmd_lblAllow.Text = "But don\'t allow:";
            // 
            // Cmd_btnCustom
            // 
            this.Cmd_btnCustom.Location = new Point(370, 485);
            this.Cmd_btnCustom.Name = "Cmd_btnCustom";
            this.Cmd_btnCustom.Size = new Size(121, 29);
            this.Cmd_btnCustom.TabIndex = 26;
            this.Cmd_btnCustom.Text = "Custom commands";
            this.Cmd_btnCustom.UseVisualStyleBackColor = true;
            this.Cmd_btnCustom.Click += new EventHandler(this.Cmd_btnCustom_Click);
            // 
            // Cmd_btnHelp
            // 
            this.Cmd_btnHelp.Location = new Point(6, 485);
            this.Cmd_btnHelp.Name = "Cmd_btnHelp";
            this.Cmd_btnHelp.Size = new Size(121, 29);
            this.Cmd_btnHelp.TabIndex = 25;
            this.Cmd_btnHelp.Text = "Help information";
            this.Cmd_btnHelp.UseVisualStyleBackColor = true;
            this.Cmd_btnHelp.Click += new EventHandler(this.Cmd_btnHelp_Click);
            // 
            // Cmd_list
            // 
            this.Cmd_list.BackColor = SystemColors.Window;
            this.Cmd_list.ForeColor = SystemColors.WindowText;
            this.Cmd_list.FormattingEnabled = true;
            this.Cmd_list.Location = new Point(6, 6);
            this.Cmd_list.Name = "Cmd_list";
            this.Cmd_list.Size = new Size(121, 472);
            this.Cmd_list.TabIndex = 24;
            this.Cmd_list.SelectedIndexChanged += new EventHandler(this.Cmd_list_SelectedIndexChanged);
            // 
            // PageSecurity
            // 
            this.PageSecurity.BackColor = SystemColors.Control;
            this.PageSecurity.Controls.Add(this.Sec_grpChat);
            this.PageSecurity.Controls.Add(this.Sec_grpCmd);
            this.PageSecurity.Controls.Add(this.Sec_grpIP);
            this.PageSecurity.Controls.Add(this.Sec_grpOther);
            this.PageSecurity.Controls.Add(this.Sec_grpBlocks);
            this.PageSecurity.Location = new Point(4, 22);
            this.PageSecurity.Name = "PageSecurity";
            this.PageSecurity.Padding = new Padding(3);
            this.PageSecurity.Size = new Size(498, 521);
            this.PageSecurity.TabIndex = 7;
            this.PageSecurity.Text = "Security";
            // 
            // Sec_grpChat
            // 
            this.Sec_grpChat.Controls.Add(this.Sec_cbChatAuto);
            this.Sec_grpChat.Controls.Add(this.Sec_lblChatOnMute);
            this.Sec_grpChat.Controls.Add(this.Sec_numChatMsgs);
            this.Sec_grpChat.Controls.Add(this.Sec_lblChatOnMsgs);
            this.Sec_grpChat.Controls.Add(this.Sec_numChatSecs);
            this.Sec_grpChat.Controls.Add(this.Sec_lblChatForMute);
            this.Sec_grpChat.Controls.Add(this.Sec_numChatMute);
            this.Sec_grpChat.Location = new Point(14, 6);
            this.Sec_grpChat.Name = "Sec_grpChat";
            this.Sec_grpChat.Size = new Size(238, 111);
            this.Sec_grpChat.TabIndex = 1;
            this.Sec_grpChat.TabStop = false;
            this.Sec_grpChat.Text = "Chat spam control";
            // 
            // Sec_lblChatOnMute
            // 
            this.Sec_lblChatOnMute.AutoSize = true;
            this.Sec_lblChatOnMute.Location = new Point(6, 48);
            this.Sec_lblChatOnMute.Name = "Sec_lblChatOnMute";
            this.Sec_lblChatOnMute.Size = new Size(46, 13);
            this.Sec_lblChatOnMute.TabIndex = 25;
            this.Sec_lblChatOnMute.Text = "Mute on";
            // 
            // Sec_numChatMsgs
            // 
            this.Sec_numChatMsgs.BackColor = SystemColors.Window;
            this.Sec_numChatMsgs.Location = new Point(53, 45);
            this.Sec_numChatMsgs.Maximum = new decimal(new int[] {
                                    10000,
                                    0,
                                    0,
                                    0});
            this.Sec_numChatMsgs.Name = "Sec_numChatMsgs";
            this.Sec_numChatMsgs.Size = new Size(37, 21);
            this.Sec_numChatMsgs.TabIndex = 30;
            this.Sec_numChatMsgs.Value = new decimal(new int[] {
                                    8,
                                    0,
                                    0,
                                    0});
            // 
            // Sec_lblChatOnMsgs
            // 
            this.Sec_lblChatOnMsgs.AutoSize = true;
            this.Sec_lblChatOnMsgs.Location = new Point(91, 48);
            this.Sec_lblChatOnMsgs.Name = "Sec_lblChatOnMsgs";
            this.Sec_lblChatOnMsgs.Size = new Size(65, 13);
            this.Sec_lblChatOnMsgs.TabIndex = 31;
            this.Sec_lblChatOnMsgs.Text = "messages in";
            // 
            // Sec_numChatSecs
            // 
            this.Sec_numChatSecs.BackColor = SystemColors.Window;
            this.Sec_numChatSecs.Location = new Point(156, 45);
            this.Sec_numChatSecs.Name = "Sec_numChatSecs";
            this.Sec_numChatSecs.Seconds = ((long)(5));
            this.Sec_numChatSecs.Size = new Size(62, 21);
            this.Sec_numChatSecs.TabIndex = 34;
            this.Sec_numChatSecs.Text = "5s";
            this.Sec_numChatSecs.Value = TimeSpan.Parse("00:00:05");
            // 
            // Sec_lblChatForMute
            // 
            this.Sec_lblChatForMute.AutoSize = true;
            this.Sec_lblChatForMute.Location = new Point(6, 83);
            this.Sec_lblChatForMute.Name = "Sec_lblChatForMute";
            this.Sec_lblChatForMute.Size = new Size(47, 13);
            this.Sec_lblChatForMute.TabIndex = 25;
            this.Sec_lblChatForMute.Text = "Mute for";
            // 
            // Sec_numChatMute
            // 
            this.Sec_numChatMute.BackColor = SystemColors.Window;
            this.Sec_numChatMute.Location = new Point(53, 79);
            this.Sec_numChatMute.Name = "Sec_numChatMute";
            this.Sec_numChatMute.Seconds = ((long)(60));
            this.Sec_numChatMute.Size = new Size(62, 21);
            this.Sec_numChatMute.TabIndex = 32;
            this.Sec_numChatMute.Text = "1m";
            this.Sec_numChatMute.Value = TimeSpan.Parse("00:01:00");
            // 
            // Sec_grpCmd
            // 
            this.Sec_grpCmd.Controls.Add(this.Sec_cbCmdAuto);
            this.Sec_grpCmd.Controls.Add(this.Sec_lblCmdOnMute);
            this.Sec_grpCmd.Controls.Add(this.Sec_numCmdMsgs);
            this.Sec_grpCmd.Controls.Add(this.Sec_lblCmdOnMsgs);
            this.Sec_grpCmd.Controls.Add(this.Sec_numCmdSecs);
            this.Sec_grpCmd.Controls.Add(this.Sec_lblCmdForMute);
            this.Sec_grpCmd.Controls.Add(this.Sec_numCmdMute);
            this.Sec_grpCmd.Location = new Point(14, 123);
            this.Sec_grpCmd.Name = "Sec_grpCmd";
            this.Sec_grpCmd.Size = new Size(238, 110);
            this.Sec_grpCmd.TabIndex = 35;
            this.Sec_grpCmd.TabStop = false;
            this.Sec_grpCmd.Text = "Command spam control";
            // 
            // Sec_cbCmdAuto
            // 
            this.Sec_cbCmdAuto.AutoSize = true;
            this.Sec_cbCmdAuto.Location = new Point(10, 20);
            this.Sec_cbCmdAuto.Name = "Sec_cbCmdAuto";
            this.Sec_cbCmdAuto.Size = new Size(149, 17);
            this.Sec_cbCmdAuto.TabIndex = 24;
            this.Sec_cbCmdAuto.Text = "Enable automatic blocking";
            this.Sec_cbCmdAuto.UseVisualStyleBackColor = true;
            this.Sec_cbCmdAuto.CheckedChanged += new EventHandler(this.Sec_cbCmdAuto_Checked);
            // 
            // Sec_lblCmdOnMute
            // 
            this.Sec_lblCmdOnMute.AutoSize = true;
            this.Sec_lblCmdOnMute.Location = new Point(6, 48);
            this.Sec_lblCmdOnMute.Name = "Sec_lblCmdOnMute";
            this.Sec_lblCmdOnMute.Size = new Size(46, 13);
            this.Sec_lblCmdOnMute.TabIndex = 25;
            this.Sec_lblCmdOnMute.Text = "Block on";
            // 
            // Sec_numCmdMsgs
            // 
            this.Sec_numCmdMsgs.BackColor = SystemColors.Window;
            this.Sec_numCmdMsgs.Location = new Point(53, 45);
            this.Sec_numCmdMsgs.Maximum = new decimal(new int[] {
                                    10000,
                                    0,
                                    0,
                                    0});
            this.Sec_numCmdMsgs.Name = "Sec_numCmdMsgs";
            this.Sec_numCmdMsgs.Size = new Size(37, 21);
            this.Sec_numCmdMsgs.TabIndex = 30;
            this.Sec_numCmdMsgs.Value = new decimal(new int[] {
                                    25,
                                    0,
                                    0,
                                    0});
            // 
            // Sec_lblCmdOnMsgs
            // 
            this.Sec_lblCmdOnMsgs.AutoSize = true;
            this.Sec_lblCmdOnMsgs.Location = new Point(91, 48);
            this.Sec_lblCmdOnMsgs.Name = "Sec_lblCmdOnMsgs";
            this.Sec_lblCmdOnMsgs.Size = new Size(70, 13);
            this.Sec_lblCmdOnMsgs.TabIndex = 31;
            this.Sec_lblCmdOnMsgs.Text = "commands in";
            // 
            // Sec_numCmdSecs
            // 
            this.Sec_numCmdSecs.BackColor = SystemColors.Window;
            this.Sec_numCmdSecs.Location = new Point(161, 45);
            this.Sec_numCmdSecs.Name = "Sec_numCmdSecs";
            this.Sec_numCmdSecs.Seconds = ((long)(1));
            this.Sec_numCmdSecs.Size = new Size(62, 21);
            this.Sec_numCmdSecs.TabIndex = 34;
            this.Sec_numCmdSecs.Text = "1s";
            this.Sec_numCmdSecs.Value = TimeSpan.Parse("00:00:01");
            // 
            // Sec_lblCmdForMute
            // 
            this.Sec_lblCmdForMute.AutoSize = true;
            this.Sec_lblCmdForMute.Location = new Point(6, 83);
            this.Sec_lblCmdForMute.Name = "Sec_lblCmdForMute";
            this.Sec_lblCmdForMute.Size = new Size(47, 13);
            this.Sec_lblCmdForMute.TabIndex = 25;
            this.Sec_lblCmdForMute.Text = "Block for";
            // 
            // Sec_numCmdMute
            // 
            this.Sec_numCmdMute.BackColor = SystemColors.Window;
            this.Sec_numCmdMute.Location = new Point(53, 79);
            this.Sec_numCmdMute.Name = "Sec_numCmdMute";
            this.Sec_numCmdMute.Seconds = ((long)(60));
            this.Sec_numCmdMute.Size = new Size(62, 21);
            this.Sec_numCmdMute.TabIndex = 32;
            this.Sec_numCmdMute.Text = "1m";
            this.Sec_numCmdMute.Value = TimeSpan.Parse("00:01:00");
            // 
            // Sec_grpIP
            // 
            this.Sec_grpIP.Controls.Add(this.Sec_cbIPAuto);
            this.Sec_grpIP.Controls.Add(this.Sec_lblIPOnMute);
            this.Sec_grpIP.Controls.Add(this.Sec_numIPMsgs);
            this.Sec_grpIP.Controls.Add(this.Sec_lblIPOnMsgs);
            this.Sec_grpIP.Controls.Add(this.Sec_numIPSecs);
            this.Sec_grpIP.Controls.Add(this.Sec_lblIPForMute);
            this.Sec_grpIP.Controls.Add(this.Sec_numIPMute);
            this.Sec_grpIP.Location = new Point(14, 240);
            this.Sec_grpIP.Name = "Sec_grpIP";
            this.Sec_grpIP.Size = new Size(238, 110);
            this.Sec_grpIP.TabIndex = 37;
            this.Sec_grpIP.TabStop = false;
            this.Sec_grpIP.Text = "Connection spam control";
            // 
            // Sec_cbIPAuto
            // 
            this.Sec_cbIPAuto.AutoSize = true;
            this.Sec_cbIPAuto.Location = new Point(10, 20);
            this.Sec_cbIPAuto.Name = "Sec_cbIPAuto";
            this.Sec_cbIPAuto.Size = new Size(149, 17);
            this.Sec_cbIPAuto.TabIndex = 24;
            this.Sec_cbIPAuto.Text = "Enable automatic blocking";
            this.Sec_cbIPAuto.UseVisualStyleBackColor = true;
            this.Sec_cbIPAuto.CheckedChanged += new EventHandler(this.Sec_cbIPAuto_Checked);
            // 
            // Sec_lblIPOnMute
            // 
            this.Sec_lblIPOnMute.AutoSize = true;
            this.Sec_lblIPOnMute.Location = new Point(6, 48);
            this.Sec_lblIPOnMute.Name = "Sec_lblIPOnMute";
            this.Sec_lblIPOnMute.Size = new Size(46, 13);
            this.Sec_lblIPOnMute.TabIndex = 25;
            this.Sec_lblIPOnMute.Text = "Block on";
            // 
            // Sec_numIPMsgs
            // 
            this.Sec_numIPMsgs.BackColor = SystemColors.Window;
            this.Sec_numIPMsgs.Location = new Point(53, 45);
            this.Sec_numIPMsgs.Maximum = new decimal(new int[] {
                                    10000,
                                    0,
                                    0,
                                    0});
            this.Sec_numIPMsgs.Name = "Sec_numIPMsgs";
            this.Sec_numIPMsgs.Size = new Size(37, 21);
            this.Sec_numIPMsgs.TabIndex = 30;
            this.Sec_numIPMsgs.Value = new decimal(new int[] {
                                    25,
                                    0,
                                    0,
                                    0});
            // 
            // Sec_lblIPOnMsgs
            // 
            this.Sec_lblIPOnMsgs.AutoSize = true;
            this.Sec_lblIPOnMsgs.Location = new Point(91, 48);
            this.Sec_lblIPOnMsgs.Name = "Sec_lblIPOnMsgs";
            this.Sec_lblIPOnMsgs.Size = new Size(75, 13);
            this.Sec_lblIPOnMsgs.TabIndex = 31;
            this.Sec_lblIPOnMsgs.Text = "connections in";
            // 
            // Sec_numIPSecs
            // 
            this.Sec_numIPSecs.BackColor = SystemColors.Window;
            this.Sec_numIPSecs.Location = new Point(166, 45);
            this.Sec_numIPSecs.Name = "Sec_numIPSecs";
            this.Sec_numIPSecs.Seconds = ((long)(1));
            this.Sec_numIPSecs.Size = new Size(62, 21);
            this.Sec_numIPSecs.TabIndex = 34;
            this.Sec_numIPSecs.Text = "1s";
            this.Sec_numIPSecs.Value = TimeSpan.Parse("00:00:01");
            // 
            // Sec_lblIPForMute
            // 
            this.Sec_lblIPForMute.AutoSize = true;
            this.Sec_lblIPForMute.Location = new Point(6, 83);
            this.Sec_lblIPForMute.Name = "Sec_lblIPForMute";
            this.Sec_lblIPForMute.Size = new Size(47, 13);
            this.Sec_lblIPForMute.TabIndex = 25;
            this.Sec_lblIPForMute.Text = "Block for";
            // 
            // Sec_numIPMute
            //
            this.Sec_numIPMute.BackColor = SystemColors.Window;
            this.Sec_numIPMute.Location = new Point(53, 79);
            this.Sec_numIPMute.Name = "Sec_numIPMute";
            this.Sec_numIPMute.Seconds = ((long)(300));
            this.Sec_numIPMute.Size = new Size(62, 21);
            this.Sec_numIPMute.TabIndex = 32;
            this.Sec_numIPMute.Text = "5m";
            this.Sec_numIPMute.Value = TimeSpan.Parse("00:05:00");
            // 
            // Sec_grpOther
            // 
            this.Sec_grpOther.Controls.Add(this.Sec_lblRank);
            this.Sec_grpOther.Controls.Add(this.Sec_cbWhitelist);
            this.Sec_grpOther.Controls.Add(this.Sec_cbLogNotes);
            this.Sec_grpOther.Controls.Add(this.Sec_cbVerifyAdmins);
            this.Sec_grpOther.Controls.Add(this.Sec_cmbVerifyRank);
            this.Sec_grpOther.Location = new Point(264, 6);
            this.Sec_grpOther.Name = "Sec_grpOther";
            this.Sec_grpOther.Size = new Size(217, 138);
            this.Sec_grpOther.TabIndex = 2;
            this.Sec_grpOther.TabStop = false;
            this.Sec_grpOther.Text = "Other settings";
            // 
            // Sec_lblRank
            // 
            this.Sec_lblRank.AutoSize = true;
            this.Sec_lblRank.Location = new Point(33, 98);
            this.Sec_lblRank.Name = "Sec_lblRank";
            this.Sec_lblRank.Size = new Size(33, 13);
            this.Sec_lblRank.TabIndex = 24;
            this.Sec_lblRank.Text = "Rank:";
            // 
            // Sec_grpBlocks
            // 
            this.Sec_grpBlocks.Controls.Add(this.Sec_cbBlocksAuto);
            this.Sec_grpBlocks.Controls.Add(this.Sec_lblBlocksOnMute);
            this.Sec_grpBlocks.Controls.Add(this.Sec_numBlocksMsgs);
            this.Sec_grpBlocks.Controls.Add(this.Sec_lblBlocksOnMsgs);
            this.Sec_grpBlocks.Controls.Add(this.Sec_numBlocksSecs);
            this.Sec_grpBlocks.Location = new Point(264, 150);
            this.Sec_grpBlocks.Name = "Sec_grpBlocks";
            this.Sec_grpBlocks.Size = new Size(217, 83);
            this.Sec_grpBlocks.TabIndex = 36;
            this.Sec_grpBlocks.TabStop = false;
            this.Sec_grpBlocks.Text = "Block spam control";
            // 
            // Sec_cbBlocksAuto
            // 
            this.Sec_cbBlocksAuto.AutoSize = true;
            this.Sec_cbBlocksAuto.Location = new Point(10, 20);
            this.Sec_cbBlocksAuto.Name = "Sec_cbBlocksAuto";
            this.Sec_cbBlocksAuto.Size = new Size(142, 17);
            this.Sec_cbBlocksAuto.TabIndex = 24;
            this.Sec_cbBlocksAuto.Text = "Enable automatic kicking";
            this.Sec_cbBlocksAuto.UseVisualStyleBackColor = true;
            this.Sec_cbBlocksAuto.CheckedChanged += new EventHandler(this.Sec_cbBlocksAuto_Checked);
            // 
            // Sec_lblBlocksOnMute
            // 
            this.Sec_lblBlocksOnMute.AutoSize = true;
            this.Sec_lblBlocksOnMute.Location = new Point(6, 48);
            this.Sec_lblBlocksOnMute.Name = "Sec_lblBlocksOnMute";
            this.Sec_lblBlocksOnMute.Size = new Size(40, 13);
            this.Sec_lblBlocksOnMute.TabIndex = 25;
            this.Sec_lblBlocksOnMute.Text = "Kick on";
            // 
            // Sec_numBlocksMsgs
            // 
            this.Sec_numBlocksMsgs.BackColor = SystemColors.Window;
            this.Sec_numBlocksMsgs.Location = new Point(46, 45);
            this.Sec_numBlocksMsgs.Maximum = new decimal(new int[] {
                                    10000,
                                    0,
                                    0,
                                    0});
            this.Sec_numBlocksMsgs.Name = "Sec_numBlocksMsgs";
            this.Sec_numBlocksMsgs.Size = new Size(46, 21);
            this.Sec_numBlocksMsgs.TabIndex = 30;
            this.Sec_numBlocksMsgs.Value = new decimal(new int[] {
                                    200,
                                    0,
                                    0,
                                    0});
            // 
            // Sec_lblBlocksOnMsgs
            // 
            this.Sec_lblBlocksOnMsgs.AutoSize = true;
            this.Sec_lblBlocksOnMsgs.Location = new Point(93, 48);
            this.Sec_lblBlocksOnMsgs.Name = "Sec_lblBlocksOnMsgs";
            this.Sec_lblBlocksOnMsgs.Size = new Size(48, 13);
            this.Sec_lblBlocksOnMsgs.TabIndex = 31;
            this.Sec_lblBlocksOnMsgs.Text = "blocks in";
            // 
            // Sec_numBlocksSecs
            // 
            this.Sec_numBlocksSecs.BackColor = SystemColors.Window;
            this.Sec_numBlocksSecs.Location = new Point(142, 45);
            this.Sec_numBlocksSecs.Name = "Sec_numBlocksSecs";
            this.Sec_numBlocksSecs.Seconds = ((long)(5));
            this.Sec_numBlocksSecs.Size = new Size(62, 21);
            this.Sec_numBlocksSecs.TabIndex = 34;
            this.Sec_numBlocksSecs.Text = "5s";
            this.Sec_numBlocksSecs.Value = TimeSpan.Parse("00:00:05");
            // 
            // PropertyWindow
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ClientSize = new Size(507, 585);
            this.Controls.Add(this.Main_btnApply);
            this.Controls.Add(this.Main_btnDiscard);
            this.Controls.Add(this.Main_btnSave);
            this.Controls.Add(this.TabControl);
            this.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PropertyWindow";
            this.Text = "Settings";
            this.Load += new EventHandler(this.PropertyWindow_Load);
            this.Disposed += new EventHandler(this.PropertyWindow_Unload);
            this.PageChat.ResumeLayout(false);
            this.Dis_grp.ResumeLayout(false);
            this.Dis_grp.PerformLayout();
            this.Chat_grpTab.ResumeLayout(false);
            this.Chat_grpTab.PerformLayout();
            this.Chat_grpMessages.ResumeLayout(false);
            this.Chat_grpMessages.PerformLayout();
            this.Chat_grpOther.ResumeLayout(false);
            this.Chat_grpOther.PerformLayout();
            this.Chat_grpColors.ResumeLayout(false);
            this.Chat_grpColors.PerformLayout();
            ((ISupportInitialize)(this.Srv_numPort)).EndInit();
            ((ISupportInitialize)(this.Irc_numPort)).EndInit();
            ((ISupportInitialize)(this.Rank_numPerm)).EndInit();
            ((ISupportInitialize)(this.Rank_numMaps)).EndInit();
            ((ISupportInitialize)(this.Rank_numDraw)).EndInit();
            ((ISupportInitialize)(this.Rank_numGen)).EndInit();
            ((ISupportInitialize)(this.Rank_numCopy)).EndInit();
            ((ISupportInitialize)(this.Rank_numUndo)).EndInit();
            ((ISupportInitialize)(this.Ls_numMax)).EndInit();
            ((ISupportInitialize)(this.Ls_numWater)).EndInit();
            ((ISupportInitialize)(this.Ls_numFast)).EndInit();
            ((ISupportInitialize)(this.Ls_numFloodUp)).EndInit();
            ((ISupportInitialize)(this.Ls_numLayer)).EndInit();
            ((ISupportInitialize)(this.Ls_numCount)).EndInit();
            ((ISupportInitialize)(this.Ls_numHeight)).EndInit();
            ((ISupportInitialize)(this.Rank_numAfk)).EndInit();
            this.PageBlocks.ResumeLayout(false);
            this.Blk_grpPhysics.ResumeLayout(false);
            this.Blk_grpBehaviour.ResumeLayout(false);
            this.Blk_grpBehaviour.PerformLayout();
            this.Blk_grpPermissions.ResumeLayout(false);
            this.Blk_grpPermissions.PerformLayout();
            this.PageRanks.ResumeLayout(false);
            this.Rank_grpLimits.ResumeLayout(false);
            this.Rank_grpLimits.PerformLayout();
            this.Rank_grpGeneral.ResumeLayout(false);
            this.Rank_grpGeneral.PerformLayout();
            this.Rank_grpMisc.ResumeLayout(false);
            this.Rank_grpMisc.PerformLayout();
            this.PageMisc.ResumeLayout(false);
            this.PageMisc.PerformLayout();
            this.GrpExtra.ResumeLayout(false);
            this.GrpExtra.PerformLayout();
            ((ISupportInitialize)(this.Misc_numReview)).EndInit();
            this.GrpMessages.ResumeLayout(false);
            this.GrpMessages.PerformLayout();
            ((ISupportInitialize)(this.Hack_num)).EndInit();
            this.GrpPhysics.ResumeLayout(false);
            this.GrpPhysics.PerformLayout();
            this.Afk_grp.ResumeLayout(false);
            this.Afk_grp.PerformLayout();
            ((ISupportInitialize)(this.Afk_numTimer)).EndInit();
            this.Bak_grp.ResumeLayout(false);
            this.Bak_grp.PerformLayout();
            ((ISupportInitialize)(this.Bak_numTime)).EndInit();
            this.PageRelay.ResumeLayout(false);
            this.Gb_ircSettings.ResumeLayout(false);
            this.Gb_ircSettings.PerformLayout();
            this.Sql_grp.ResumeLayout(false);
            this.Sql_grp.PerformLayout();
            this.Irc_grp.ResumeLayout(false);
            this.Irc_grp.PerformLayout();
            this.PageServer.ResumeLayout(false);
            this.Lvl_grp.ResumeLayout(false);
            this.Lvl_grp.PerformLayout();
            this.Adv_grp.ResumeLayout(false);
            this.Adv_grp.PerformLayout();
            this.Srv_grp.ResumeLayout(false);
            this.Srv_grp.PerformLayout();
            this.Srv_grpUpdate.ResumeLayout(false);
            this.Srv_grpUpdate.PerformLayout();
            this.grpPlayers.ResumeLayout(false);
            this.grpPlayers.PerformLayout();
            ((ISupportInitialize)(this.Srv_numPlayers)).EndInit();
            ((ISupportInitialize)(this.Srv_numGuests)).EndInit();
            this.TabControl.ResumeLayout(false);
            this.PageEco.ResumeLayout(false);
            this.Eco_gbRank.ResumeLayout(false);
            this.Eco_gbRank.PerformLayout();
            ((ISupportInitialize)(this.Eco_dgvRanks)).EndInit();
            this.Eco_gbLvl.ResumeLayout(false);
            this.Eco_gbLvl.PerformLayout();
            ((ISupportInitialize)(this.Eco_dgvMaps)).EndInit();
            this.Eco_gbItem.ResumeLayout(false);
            this.Eco_gbItem.PerformLayout();
            ((ISupportInitialize)(this.Eco_numItemPrice)).EndInit();
            this.Eco_gb.ResumeLayout(false);
            this.Eco_gb.PerformLayout();
            this.PageGames.ResumeLayout(false);
            this.TabGames.ResumeLayout(false);
            this.TabLS.ResumeLayout(false);
            this.Ls_grpControls.ResumeLayout(false);
            this.Ls_grpMapSettings.ResumeLayout(false);
            this.Ls_grpTime.ResumeLayout(false);
            this.Ls_grpTime.PerformLayout();
            ((ISupportInitialize)(this.Ls_numFlood)).EndInit();
            ((ISupportInitialize)(this.Ls_numLayerTime)).EndInit();
            ((ISupportInitialize)(this.Ls_numRound)).EndInit();
            this.Ls_grpLayer.ResumeLayout(false);
            this.Ls_grpLayer.PerformLayout();
            this.Ls_grpFlood.ResumeLayout(false);
            this.Ls_grpFlood.PerformLayout();
            this.Ls_grpSettings.ResumeLayout(false);
            this.Ls_grpSettings.PerformLayout();
            this.Ls_grpMaps.ResumeLayout(false);
            this.Ls_grpMaps.PerformLayout();
            this.TabZS.ResumeLayout(false);
            this.Zs_grpControls.ResumeLayout(false);
            this.Zs_grpMap.ResumeLayout(false);
            this.Zs_grpTime.ResumeLayout(false);
            this.Zs_grpTime.PerformLayout();
            ((ISupportInitialize)(this.TimespanUpDown1)).EndInit();
            ((ISupportInitialize)(this.TimespanUpDown2)).EndInit();
            ((ISupportInitialize)(this.TimespanUpDown3)).EndInit();
            this.Zs_grpSettings.ResumeLayout(false);
            this.Zs_grpSettings.PerformLayout();
            this.Zs_grpZombie.ResumeLayout(false);
            this.Zs_grpZombie.PerformLayout();
            this.Zs_grpRevive.ResumeLayout(false);
            this.Zs_grpRevive.PerformLayout();
            ((ISupportInitialize)(this.Zs_numReviveEff)).EndInit();
            ((ISupportInitialize)(this.Zs_numReviveLimit)).EndInit();
            ((ISupportInitialize)(this.Zs_numReviveMax)).EndInit();
            this.Zs_grpInv.ResumeLayout(false);
            this.Zs_grpInv.PerformLayout();
            ((ISupportInitialize)(this.Zs_numInvZombieDur)).EndInit();
            ((ISupportInitialize)(this.Zs_numInvHumanDur)).EndInit();
            ((ISupportInitialize)(this.Zs_numInvZombieMax)).EndInit();
            ((ISupportInitialize)(this.Zs_numInvHumanMax)).EndInit();
            this.Zs_grpMaps.ResumeLayout(false);
            this.Zs_grpMaps.PerformLayout();
            this.TabZS_old.ResumeLayout(false);
            this.TabCTF.ResumeLayout(false);
            this.Ctf_grpControls.ResumeLayout(false);
            this.Ctf_grpSettings.ResumeLayout(false);
            this.Ctf_grpSettings.PerformLayout();
            this.Ctf_grpMaps.ResumeLayout(false);
            this.Ctf_grpMaps.PerformLayout();
            this.TabTW.ResumeLayout(false);
            this.Tw_grpControls.ResumeLayout(false);
            this.Tw_grpMapSettings.ResumeLayout(false);
            this.Tw_grpTeams.ResumeLayout(false);
            this.Tw_grpTeams.PerformLayout();
            this.Tw_grpGrace.ResumeLayout(false);
            this.Tw_grpGrace.PerformLayout();
            ((ISupportInitialize)(this.Tw_numGrace)).EndInit();
            this.Tw_grpScores.ResumeLayout(false);
            this.Tw_grpScores.PerformLayout();
            ((ISupportInitialize)(this.Tw_numMultiKills)).EndInit();
            ((ISupportInitialize)(this.Tw_numScoreAssists)).EndInit();
            ((ISupportInitialize)(this.Tw_numScorePerKill)).EndInit();
            ((ISupportInitialize)(this.Tw_numScoreLimit)).EndInit();
            this.Tw_grpSettings.ResumeLayout(false);
            this.Tw_grpSettings.PerformLayout();
            this.Tw_gbMaps.ResumeLayout(false);
            this.Tw_gbMaps.PerformLayout();
            this.TabCD.ResumeLayout(false);
            this.Cd_grpControls.ResumeLayout(false);
            this.Cd_grpSettings.ResumeLayout(false);
            this.Cd_grpSettings.PerformLayout();
            this.Cd_grpMaps.ResumeLayout(false);
            this.Cd_grpMaps.PerformLayout();
            this.PageCommands.ResumeLayout(false);
            this.Cmd_grpExtra.ResumeLayout(false);
            this.Cmd_grpExtra.PerformLayout();
            this.Cmd_grpPermissions.ResumeLayout(false);
            this.Cmd_grpPermissions.PerformLayout();
            this.PageSecurity.ResumeLayout(false);
            this.Sec_grpChat.ResumeLayout(false);
            this.Sec_grpChat.PerformLayout();
            ((ISupportInitialize)(this.Sec_numChatMsgs)).EndInit();
            ((ISupportInitialize)(this.Sec_numChatSecs)).EndInit();
            ((ISupportInitialize)(this.Sec_numChatMute)).EndInit();
            this.Sec_grpCmd.ResumeLayout(false);
            this.Sec_grpCmd.PerformLayout();
            ((ISupportInitialize)(this.Sec_numCmdMsgs)).EndInit();
            ((ISupportInitialize)(this.Sec_numCmdSecs)).EndInit();
            ((ISupportInitialize)(this.Sec_numCmdMute)).EndInit();
            this.Sec_grpIP.ResumeLayout(false);
            this.Sec_grpIP.PerformLayout();
            ((ISupportInitialize)(this.Sec_numIPMsgs)).EndInit();
            ((ISupportInitialize)(this.Sec_numIPSecs)).EndInit();
            ((ISupportInitialize)(this.Sec_numIPMute)).EndInit();
            this.Sec_grpOther.ResumeLayout(false);
            this.Sec_grpOther.PerformLayout();
            this.Sec_grpBlocks.ResumeLayout(false);
            this.Sec_grpBlocks.PerformLayout();
            ((ISupportInitialize)(this.Sec_numBlocksMsgs)).EndInit();
            ((ISupportInitialize)(this.Sec_numBlocksSecs)).EndInit();
            this.ResumeLayout(false);
        }
        public TextBox Zs_txtName;
        public TextBox Zs_txtModel;
        public Label Zs_lblName;
        public Label Zs_lblModel;
        public GroupBox Zs_grpZombie;
        public Label Label9;
        public NumericUpDown Zs_numReviveMax;
        public NumericUpDown Zs_numReviveLimit;
        public Label Zs_lblReviveLimitHdr;
        public Label Zs_lblReviveLimitFtr;
        public GroupBox Zs_grpRevive;
        public ListBox Zs_lstUsed;
        public ListBox Zs_lstNotUsed;
        public Button Zs_btnRemove;
        public Button Zs_btnAdd;
        public Label Zs_lblUsed;
        public Label Zs_lblNotUsed;
        public GroupBox Zs_grpMaps;
        public CheckBox Zs_cbStart;
        public CheckBox Zs_cbMap;
        public CheckBox Zs_cbMain;
        public GroupBox Zs_grpSettings;
        public Label Zs_lblInvHumanMax;
        public Label Zs_lblInvHumanDur;
        public Label Zs_lblInvZombieMax;
        public Label Zs_lblInvZombieDur;
        public NumericUpDown Zs_numInvHumanMax;
        public NumericUpDown Zs_numInvZombieMax;
        public NumericUpDown Zs_numInvHumanDur;
        public NumericUpDown Zs_numInvZombieDur;
        public GroupBox Zs_grpInv;
        public Label Zs_lblReviveEff;
        public NumericUpDown Zs_numReviveEff;
        public Label Label4;
        public Label Label3;
        public Label Label2;
        public Label Label1;
        public Flames.Gui.TimespanUpDown TimespanUpDown3;
        public Flames.Gui.TimespanUpDown TimespanUpDown2;
        public Flames.Gui.TimespanUpDown TimespanUpDown1;
        public GroupBox Zs_grpTime;
        public GroupBox Zs_grpMap;
        public Button Zs_btnStart;
        public Button Zs_btnStop;
        public Button Zs_btnEnd;
        public GroupBox Zs_grpControls;
        public TabPage TabZS;
        public CheckBox ChkPhysRestart;
        public Label Ls_lblRound;
        public Label Ls_lblFlood;
        public Label Ls_lblLayerTime;
        public Flames.Gui.TimespanUpDown Ls_numRound;
        public Flames.Gui.TimespanUpDown Ls_numLayerTime;
        public Flames.Gui.TimespanUpDown Ls_numFlood;
        public Flames.Gui.TimespanUpDown Misc_numReview;
        public Flames.Gui.TimespanUpDown Hack_num;
        public Flames.Gui.TimespanUpDown Afk_numTimer;
        public Label Tw_lblAssist;
        public Label Tw_lblMulti;
        public Button Chat_btnWarn;
        public Label Chat_lblWarn;
        public Label Tw_lblMode;
        public ComboBox Tw_cmbDiff;
        public ComboBox Tw_cmbMode;
        public ListBox Tw_lstUsed;
        public ListBox Tw_lstNotUsed;
        public Button Tw_btnRemove;
        public Button Tw_btnAdd;
        public Label Tw_lblInUse;
        public GroupBox Tw_gbMaps;
        public CheckBox Tw_cbStart;
        public CheckBox Tw_cbMap;
        public CheckBox Tw_cbMain;
        public Label Tw_lblDiff;
        public GroupBox Tw_grpSettings;
        public GroupBox Tw_grpMapSettings;
        public Button Tw_btnStart;
        public Button Tw_btnStop;
        public Button Tw_btnEnd;
        public GroupBox Tw_grpControls;
        public TabPage TabTW;
        public ListBox Ctf_lstUsed;
        public ListBox Ctf_lstNotUsed;
        public Button Ctf_btnRemove;
        public Button Ctf_btnAdd;
        public Label Ctf_lblUsed;
        public Label Ctf_lblNotUsed;
        public GroupBox Ctf_grpMaps;
        public CheckBox Ctf_cbStart;
        public CheckBox Ctf_cbMap;
        public CheckBox Ctf_cbMain;
        public GroupBox Ctf_grpSettings;
        public Button Ctf_btnStart;
        public Button Ctf_btnStop;
        public Button Ctf_btnEnd;
        public GroupBox Ctf_grpControls;
        public TabPage TabCTF;
        public ListBox Cd_lstUsed;
        public ListBox Cd_lstNotUsed;
        public Button Cd_btnRemove;
        public Button Cd_btnAdd;
        public Label Cd_lblUsed;
        public Label Cd_lblNotUsed;
        public GroupBox Cd_grpMaps;
        public CheckBox Cd_cbStart;
        public CheckBox Cd_cbMap;
        public CheckBox Cd_cbMain;
        public GroupBox Cd_grpSettings;
        public Button Cd_btnStart;
        public Button Cd_btnStop;
        public Button Cd_btnEnd;
        public GroupBox Cd_grpControls;
        public TabPage TabCD;
        public GroupBox Ls_grpTime;
        public Label Ls_lblLayer;
        public NumericUpDown Ls_numLayer;
        public NumericUpDown Ls_numCount;
        public Label Ls_lblLayersEach;
        public NumericUpDown Ls_numHeight;
        public Label Ls_lblBlocksTall;
        public Label Ls_lblWater;
        public Label Ls_lblFast;
        public Label Ls_lblFloodUp;
        public NumericUpDown Ls_numWater;
        public NumericUpDown Ls_numFast;
        public NumericUpDown Ls_numFloodUp;
        public GroupBox Ls_grpFlood;
        public GroupBox Ls_grpLayer;
        public CheckBox Ls_cbMain;
        public NumericUpDown Ls_numMax;
        public Label Ls_lblMax;
        public CheckBox Ls_cbStart;
        public CheckBox Ls_cbMap;
        public DataGridViewTextBoxColumn Eco_colRankPrice;
        public DataGridViewTextBoxColumn Eco_colRankName;
        public DataGridView Eco_dgvRanks;
        public Label Eco_lblItemRank;
        public ComboBox Eco_cmbItemRank;
        public Label Eco_lblCfg;
        public ComboBox Eco_cmbCfg;
        public CheckBox Eco_cbItem;
        public Label Eco_lblItemPrice;
        public NumericUpDown Eco_numItemPrice;
        public GroupBox Eco_gbItem;
        public CheckBox Eco_cbRank;
        public GroupBox Eco_gbRank;
        public CheckBox Eco_cbLvl;
        public Button Eco_btnLvlAdd;
        public Button Eco_btnLvlDel;
        public DataGridViewComboBoxColumn Eco_colLvlTheme;
        public DataGridViewTextBoxColumn Eco_colLvlZ;
        public DataGridViewTextBoxColumn Eco_colLvlY;
        public DataGridViewTextBoxColumn Eco_colLvlX;
        public DataGridViewTextBoxColumn Eco_colLvlPrice;
        public DataGridViewTextBoxColumn Eco_colLvlName;
        public DataGridView Eco_dgvMaps;
        public GroupBox Eco_gbLvl;
        public Label Eco_lblCurrency;
        public TextBox Eco_txtCurrency;
        public CheckBox Eco_cbEnabled;
        public GroupBox Eco_gb;
        public TabPage PageEco;
        public CheckBox ChkRestart;
        public Label Rank_lblCopy;
        public NumericUpDown Rank_numCopy;
        public CheckBox Rank_cbAfk;
        public Flames.Gui.TimespanUpDown Rank_numAfk;
        public Label Rank_lblUndo;
        public Flames.Gui.TimespanUpDown Rank_numUndo;
        public Label Rank_lblDraw;
        public NumericUpDown Rank_numDraw;
        public NumericUpDown Rank_numMaps;
        public Label Rank_lblMaps;
        public NumericUpDown Rank_numGen;
        public Label Rank_lblGen;
        public GroupBox Rank_grpLimits;
        public Label Irc_lblRank;
        public ComboBox Irc_cmbRank;
        public Label Irc_lblVerify;
        public ComboBox Irc_cmbVerify;
        public Label Irc_lblPrefix;
        public TextBox Irc_txtPrefix;
        public CheckBox Irc_cbAFK;
        public CheckBox Irc_cbWorldChanges;
        public GroupBox Gb_ircSettings;
        public Label Cmd_lblExtra1;
        public ComboBox Cmd_cmbExtra1;
        public Label Cmd_lblExtra2;
        public ComboBox Cmd_cmbExtra2;
        public Label Cmd_lblExtra3;
        public ComboBox Cmd_cmbExtra3;
        public Label Cmd_lblExtra4;
        public ComboBox Cmd_cmbExtra4;
        public Label Cmd_lblExtra5;
        public ComboBox Cmd_cmbExtra5;
        public Label Cmd_lblExtra6;
        public ComboBox Cmd_cmbExtra6;
        public Label Cmd_lblExtra7;
        public ComboBox Cmd_cmbExtra7;
        public GroupBox Cmd_grpExtra;
        public ListBox Cmd_list;
        public Button Cmd_btnHelp;
        public Label Cmd_lblAllow;
        public Label Cmd_lblDisallow;
        public Label Cmd_lblMin;
        public ComboBox Cmd_cmbMin;
        public ComboBox Cmd_cmbDis1;
        public ComboBox Cmd_cmbAlw1;
        public ComboBox Cmd_cmbDis2;
        public ComboBox Cmd_cmbDis3;
        public ComboBox Cmd_cmbAlw2;
        public ComboBox Cmd_cmbAlw3;
        public GroupBox Cmd_grpPermissions;
        public Button Cmd_btnCustom;
        public CheckBox Blk_cbLava;
        public CheckBox Blk_cbWater;
        public CheckBox Blk_cbDoor;
        public CheckBox Blk_cbTdoor;
        public CheckBox Blk_cbRails;
        public CheckBox Blk_cbMsgBlock;
        public CheckBox Blk_cbPortal;
        public CheckBox Blk_cbDeath;
        public TextBox Blk_txtDeath;
        public GroupBox Blk_grpBehaviour;
        public GroupBox Blk_grpPhysics;
        public ComboBox Blk_cmbMin;
        public ComboBox Blk_cmbDis1;
        public ComboBox Blk_cmbAlw1;
        public ComboBox Blk_cmbDis2;
        public ComboBox Blk_cmbDis3;
        public ComboBox Blk_cmbAlw2;
        public ComboBox Blk_cmbAlw3;
        public GroupBox Blk_grpPermissions;
        #endregion
        public TextBox Dis_txtOpChannel;
        public Label Dis_lblOpChannel;
        public TextBox Dis_txtChannel;
        public Label Dis_lblChannel;
        public TextBox Dis_txtToken;
        public Label Dis_lblToken;
        public CheckBox Dis_chkEnabled;
        public CheckBox Dis_chkNicks;
        public GroupBox Dis_grp;
        public LinkLabel Dis_linkHelp;
        public ComboBox Rank_cmbOsMap;
        public Label Rank_lblOsMap;
        public TextBox Rank_txtPrefix;
        public Label Rank_lblPrefix;
        public Label Srv_lblOwner;
        public GroupBox Rank_grpMisc;
        public GroupBox Rank_grpGeneral;
        public TabPage PageChat;
        public GroupBox Chat_grpTab;
        public CheckBox Chat_cbTabRank;
        public CheckBox Chat_cbTabLevel;
        public CheckBox Chat_cbTabBots;
        public GroupBox Chat_grpMessages;
        public GroupBox Chat_grpModeration;
        public Label Chat_lblShutdown;
        public TextBox Chat_txtShutdown;
        public CheckBox Chat_chkCheap;
        public TextBox Chat_txtCheap;
        public Label Chat_lblBan;
        public TextBox Chat_txtBan;
        public Label Chat_lblPromote;
        public TextBox Chat_txtPromote;
        public Label Chat_lblDemote;
        public TextBox Chat_txtDemote;
        public Label Chat_lblLogin;
        public TextBox Chat_txtLogin;
        public Label Chat_lblLogout;
        public TextBox Chat_txtLogout;
        public GroupBox Chat_grpColors;
        public Label Chat_lblDefault;
        public Button Chat_btnDefault;
        public Label Chat_lblIRC;
        public Button Chat_btnIRC;
        public Label Chat_lblSyntax;
        public Button Chat_btnSyntax;
        public Label Chat_lblDesc;
        public Button Chat_btnDesc;
        public GroupBox Chat_grpOther;
        public Label Chat_lblConsole;
        public TextBox Chat_txtConsole;
        public TabPage PageSecurity;
        public GroupBox Sec_grpChat;
        public CheckBox Sec_cbChatAuto;
        public Label Sec_lblChatForMute;
        public Flames.Gui.TimespanUpDown Sec_numChatMute;
        public Label Sec_lblChatOnMsgs;
        public NumericUpDown Sec_numChatMsgs;
        public Label Sec_lblChatOnMute;
        public Flames.Gui.TimespanUpDown Sec_numChatSecs;
        public GroupBox Sec_grpCmd;
        public CheckBox Sec_cbCmdAuto;
        public Flames.Gui.TimespanUpDown Sec_numCmdMute;
        public Label Sec_lblCmdForMute;
        public Flames.Gui.TimespanUpDown Sec_numCmdSecs;
        public Label Sec_lblCmdOnMsgs;
        public NumericUpDown Sec_numCmdMsgs;
        public Label Sec_lblCmdOnMute;
        public GroupBox Sec_grpIP;
        public CheckBox Sec_cbIPAuto;
        public Flames.Gui.TimespanUpDown Sec_numIPMute;
        public Label Sec_lblIPForMute;
        public Flames.Gui.TimespanUpDown Sec_numIPSecs;
        public Label Sec_lblIPOnMsgs;
        public NumericUpDown Sec_numIPMsgs;
        public Label Sec_lblIPOnMute;
        public GroupBox Sec_grpOther;
        public CheckBox Sec_cbLogNotes;
        public CheckBox Sec_cbWhitelist;
        public CheckBox Sec_cbVerifyAdmins;
        public Label Sec_lblRank;
        public ComboBox Sec_cmbVerifyRank;
        public GroupBox Sec_grpBlocks;
        public CheckBox Sec_cbBlocksAuto;
        public Flames.Gui.TimespanUpDown Sec_numBlocksSecs;
        public Label Sec_lblBlocksOnMsgs;
        public NumericUpDown Sec_numBlocksMsgs;
        public Label Sec_lblBlocksOnMute;
        public Button Main_btnSave;
        public Button Main_btnDiscard;
        public Button Main_btnApply;
        public ToolTip GUIToolTip;
        public TabPage PageBlocks;
        public Button Blk_btnHelp;
        public Label Blk_lblAllow;
        public Label Blk_lblDisallow;
        public Label Blk_lblMin;
        public ListBox Blk_list;
        public TabPage PageCommands;
        public TabPage PageRanks;
        public Button Rank_btnColor;
        public Label Rank_lblColor;
        public NumericUpDown Rank_numPerm;
        public TextBox Rank_txtName;
        public Label Rank_lblPerm;
        public Label Rank_lblName;
        public Button Rank_btnDel;
        public Button Rank_btnAdd;
        public ListBox Rank_list;
        public TabPage PageMisc;
        public TextBox TxtNormRp;
        public TextBox TxtRP;
        public Flames.Gui.TimespanUpDown Bak_numTime;
        public TextBox Bak_txtLocation;
        public CheckBox Hack_lbl;
        public CheckBox Chat_chkFilter;
        public CheckBox ChkRepeatMessages;
        public CheckBox ChkDeath;
        public CheckBox Chk17Dollar;
        public CheckBox ChkSmile;
        public Label ChkRpNorm;
        public Label ChkRpLimit;
        public Label Afk_lblTimer;
        public Label Bak_lblTime;
        public Label Bak_lblLocation;
        public TabPage PageRelay;
        public TextBox Irc_txtOpChannel;
        public TextBox Irc_txtChannel;
        public TextBox Irc_txtServer;
        public TextBox Irc_txtNick;
        public Label Irc_lblOpChannel;
        public CheckBox Irc_chkEnabled;
        public Label Irc_lblChannel;
        public Label Irc_lblServer;
        public Label Irc_lblNick;
        public TabPage PageServer;
        public NumericUpDown Srv_numPlayers;
        public NumericUpDown Srv_numGuests;
        public Label Srv_lblGuests;
        public TextBox Lvl_txtMain;
        public NumericUpDown Srv_numPort;
        public TextBox Srv_txtMOTD;
        public TextBox Srv_txtName;
        public Button Srv_btnPort;
        public ComboBox Rank_cmbDefault;
        public Label Rank_lblDefault;
        public CheckBox Adv_chkCPE;
        public CheckBox Srv_chkPublic;
        public CheckBox Lvl_chkAutoload;
        public CheckBox Lvl_chkWorld;
        public CheckBox ChkUpdates;
        public CheckBox Adv_chkVerify;
        public Label Lvl_lblMain;
        public Label Srv_lblPlayers;
        public Label Srv_lblPort;
        public Label Srv_lblMotd;
        public Label Srv_lblName;
        public TabControl TabControl;
        public CheckBox Srv_cbMustAgree;
        public CheckBox Rank_cbSilentAdmins;
        public Button Adv_btnEditTexts;
        public CheckBox Rank_cbTPHigher;
        public GroupBox Irc_grp;
        public GroupBox Srv_grp;
        public GroupBox Adv_grp;
        public GroupBox grpPlayers;
        public GroupBox Lvl_grp;
        public TextBox Srv_txtOwner;
        public GroupBox GrpExtra;
        public GroupBox GrpMessages;
        public GroupBox GrpPhysics;
        public GroupBox Afk_grp;
        public GroupBox Bak_grp;
        public GroupBox Sql_grp;
        public LinkLabel Sql_linkDownload;
        public TextBox Sql_txtHost;
        public Label Sql_lblHost;
        public TextBox Sql_txtDBName;
        public Label Sql_lblDBName;
        public Label Sql_lblPass;
        public Label Sql_lblUser;
        public TextBox Sql_txtPass;
        public TextBox Sql_txtUser;
        public CheckBox Sql_chkUseSQL;
        public TabPage PageGames;
        public TabControl TabGames;
        public TabPage TabZS_old;
        public TabPage TabLS;
        public TextBox Irc_txtPass;
        public CheckBox Irc_chkPass;
        public CheckBox Irc_cbTitles;
        public Label Irc_lblPort;
        public NumericUpDown Irc_numPort;
        public CheckBox Rank_cbEmpty;
        public GroupBox Ls_grpMaps;
        public Label Ls_lblNotUsed;
        public Label Ls_lblUsed;
        public Button Ls_btnAdd;
        public Button Ls_btnRemove;
        public ListBox Ls_lstNotUsed;
        public ListBox Ls_lstUsed;
        public GroupBox Ls_grpSettings;
        public GroupBox Ls_grpMapSettings;
        public GroupBox Ls_grpControls;
        public Button Ls_btnEnd;
        public Button Ls_btnStop;
        public Button Ls_btnStart;
        public TextBox Sql_txtPort;
        public Label Sql_lblPort;
        public GroupBox Srv_grpUpdate;
        public Button Srv_btnForceUpdate;
        public CheckBox ChkGuestLimitNotify;
        public Label Misc_lblReview;
        public Label Rank_lblMOTD;
        public TextBox Rank_txtMOTD;
        public GroupBox Tw_grpScores;
        public NumericUpDown Tw_numScoreAssists;
        public Label Tw_lblScorePerKill;
        public NumericUpDown Tw_numScorePerKill;
        public Label Tw_lblScoreLimit;
        public NumericUpDown Tw_numScoreLimit;
        public NumericUpDown Tw_numMultiKills;
        public GroupBox Tw_grpGrace;
        public Label Tw_lblGrace;
        public Flames.Gui.TimespanUpDown Tw_numGrace;
        public GroupBox Tw_grpTeams;
        public CheckBox Tw_cbKills;
        public CheckBox Tw_cbBalance;
        public CheckBox Tw_cbGrace;
        public CheckBox Tw_cbStreaks;
        public PropertyGrid PropsZG;
    }
}