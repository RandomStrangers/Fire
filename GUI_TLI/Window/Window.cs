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
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Flames.Events;
using Flames.Events.LevelEvents;
using Flames.Events.PlayerEvents;
using Flames.Generator;
using Flames.Gui.Popups;
using Flames.Tasks;
using Flames.Added;
using Context = System.Environment;
namespace Flames.Gui
{
    public partial class Window : Form
    {
        // for cross thread use
        public delegate void StringCallback(string s);
        public delegate void PlayerListCallback(List<Player> players);
        public delegate void VoidDelegate();
        public bool Mapgen, Loaded;
        public NotifyIcon GUINotifyIcon = new NotifyIcon();
        public Player CurPlayer;
        public Window()
        {
            GUILogCallback = LogMessageCore;
            InitializeComponent();
        }
        // warn user if they're using the GUI with a DLL for different server version
        public static void CheckVersions()
        {
            string gui_version = Server.InternalVersion;
            string dll_version = Server.Version;
            if (gui_version.CaselessEq(dll_version))
            {
                return;
            }
            const string Fmt =
@"Currently you are using:
  {2} for {0} {1}
  {4} for {0} {3}

Trying to mix two versions is unsupported - you may experience issues";
            string msg = string.Format(Fmt, Server.SoftwareName,
                                       gui_version, AssemblyFile(typeof(Window), "FlamesGUI.exe"),
                                       dll_version, AssemblyFile(typeof(Server), "FlamesTLI_.dll"));
            RunAsync(() => Popup.Warning(msg));
        }
        public static string AssemblyFile(Type type, string defPath)
        {
            try
            {
                string path = type.Assembly.CodeBase;
                return Path.GetFileName(path);
            }
            catch
            {
                return defPath;
            }
        }
        public void Window_Load(object sender, EventArgs e)
        {
            LoadIcon();
            // Necessary as some versions of WINE may call Window_Load multiple times
            //  (however icon must still be reloaded each time)
            if (Loaded) 
            {
                return;
            }
            Loaded = true;
            Text = "Starting " + Colors.Strip(Server.SoftwareNameVersioned) + "...";
            Show();
            BringToFront();
            WindowState = FormWindowState.Normal;
            CheckVersions();
            InitServer();
            foreach (MapGen gen in MapGen.Generators)
            {
                if (gen.Type == GenType.Advanced) 
                {
                    continue;
                }
                Map_cmbType.Items.Add(gen.Theme);
            }
            Text = Colors.Strip(Server.Config.Name) + " - " + Colors.Strip(Server.SoftwareNameVersioned);
            MakeNotifyIcon();
            Main_Players.Font = new Font("Calibri", 8.25f);
            Main_Maps.Font = new Font("Calibri", 8.25f);
        }
        public void LoadIcon()
        {
            // Normally this code would be in InitializeComponent method in Window.Designer.cs,
            //  however that doesn't work properly with some WINE versions (you get WINE icon instead)
            try
            {
                Icon = GetIcon();
                GuiUtils.WinIcon = Icon;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
        public void UpdateNotifyIconText()
        {
            int playerCount = PlayerInfo.Online.Count;
            string players = " (" + playerCount + " players)";
            // ArgumentException thrown if text length is > 63
            string text = Colors.Strip(Server.Config.Name) + players;
            if (text.Length > 63)
            {
                text = text.Substring(0, 63);
            }
            GUINotifyIcon.Text = text;
        }
        public void MakeNotifyIcon()
        {
            UpdateNotifyIconText();
            GUINotifyIcon.ContextMenuStrip = Icon_context;
            GUINotifyIcon.Icon = Icon;
            GUINotifyIcon.Visible = true;
            GUINotifyIcon.MouseClick += NotifyIcon_MouseClick;
        }
        public void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Icon_OpenTerminal_Click(sender, e);
            }
        }
        public void InitServer()
        {
            Logger.LogHandler += LogMessage;
            Updater.NewerVersionDetected += OnNewerVersionDetected;
            Server.OnURLChange += UpdateUrl;
            Server.OnSettingsUpdate += SettingsUpdate;
            Server.Background.QueueOnce(InitServerTask);
        }
        // cache LogMessage, avoids new object being allocated every time
        public delegate void LogCallback(LogType type, string message);
        public LogCallback GUILogCallback;
        public void LogMessage(LogType type, string message)
        {
            if (!Server.Config.FlameLogging[(int)type])
            {
                return;
            }
            try
            {
                BeginInvoke(GUILogCallback, type, message);
            }
            catch (InvalidOperationException)
            {
                // This exception is thrown when trying to log
                //  messages after window has already been closed
            }
        }
        public void LogMessageCore(LogType type, string message)
        {
            if (Server.shuttingDown)
            {
                return;
            }
            string newline = Context.NewLine;
            switch (type)
            {
                case LogType.Error:
                    Main_txtLog.AppendLog("&c!!!Error" + ExtractErrorMessage(message)
                                          + " - See Logs tab for more details" + newline);
                    message = FormatError(message);
                    Logs_txtError.AppendText(message + newline);
                    break;
                case LogType.BackgroundActivity:
                    message = DateTime.Now.ToString("(HH:mm:ss) ") + message;
                    Logs_txtSystem.AppendText(message + newline);
                    break;
                case LogType.CommandUsage:
                    message = DateTime.Now.ToString("(HH:mm:ss) ") + message;
                    Main_txtLog.AppendLog(message + newline, Main_txtLog.ForeColor, false);
                    break;
                default:
                    Main_txtLog.AppendLog(message + newline);
                    break;
            }
        }
        public static string FormatError(string message)
        {
            string date = "----" + DateTime.Now + "----";
            return date + Context.NewLine + message + Context.NewLine + "-------------------------";
        }
        public static string MsgPrefix = Context.NewLine + "Message: ";
        public static string ExtractErrorMessage(string raw)
        {
            // Error messages are usually structured like so:
            //   Type: whatever
            //   Message: whatever
            //   Something: whatever
            // this code extracts the Message line from the raw message
            int beg = raw.IndexOf(MsgPrefix);
            if (beg == -1)
            {
                return "";
            }
            beg += MsgPrefix.Length;
            int end = raw.IndexOf(Context.NewLine, beg);
            if (end == -1)
            {
                return "";
            }
            return " (" + raw.Substring(beg, end - beg) + ")";
        }
        public void OnNewerVersionDetected(object sender, EventArgs e)
        {
            RunOnUI_Async(ShowUpdateMessageBox);
        }
        public void ShowUpdateMessageBox()
        {
            if (UpdateAvailable.Active)
            {
                return;
            }
            UpdateAvailable form = new UpdateAvailable();
            // https://stackoverflow.com/questions/8566582/how-to-centerparent-a-non-modal-form
            form.Location = new Point(Location.X + (Width - form.Width) / 2,
                                     Location.Y + (Height - form.Height) / 2);
            form.Show(this);
        }
        public static void RunAsync(ThreadStart func)
        {
            Thread thread = new Thread(func)
            {
                Name = "MsgBox"
            };
            thread.Start();
        }
        public void InitServerTask(SchedulerTask task)
        {
            Server.Start();
            // The first check for updates is run after 10 seconds, subsequent ones every two hours
            Server.Background.QueueRepeat(Updater.UpdaterTask, null, TimeSpan.FromSeconds(10));
            OnPlayerConnectEvent.Register(Player_PlayerConnect, Priority.Low);
            OnPlayerDisconnectEvent.Register(Player_PlayerDisconnect, Priority.Low);
            OnSentMapEvent.Register(Player_OnJoinedLevel, Priority.Low);
            OnModActionEvent.Register(Player_OnModAction, Priority.Low);
            OnLevelAddedEvent.Register(Level_LevelAdded, Priority.Low);
            OnLevelRemovedEvent.Register(Level_LevelRemoved, Priority.Low);
            OnPhysicsLevelChangedEvent.Register(Level_PhysicsLevelChanged, Priority.Low);
            RunOnUI_Async(() => Main_btnProps.Enabled = true);
        }
        public void RunOnUI_Async(UIAction act) 
        { 
            BeginInvoke(act); 
        }
        public void Player_PlayerConnect(Player p)
        {
            RunOnUI_Async(() => 
            {
                Main_UpdatePlayersList();
                Players_UpdateList();
            });
        }
        public void Player_PlayerDisconnect(Player p, string reason)
        {
            RunOnUI_Async(() => 
            {
                Main_UpdateMapList();
                Main_UpdatePlayersList();
                Players_UpdateList();
            });
        }
        public void Player_OnJoinedLevel(Player p, Level prevLevel, Level lvl)
        {
            RunOnUI_Async(() => 
            {
                Main_UpdateMapList();
                Main_UpdatePlayersList();
                Players_UpdateSelected();
            });
        }
        public void Player_OnModAction(ModAction action)
        {
            if (action.Type != ModActionType.Rank)
            {
                return;
            }
            RunOnUI_Async(() => 
            {
                Main_UpdatePlayersList();
            });
        }
        public void Level_LevelAdded(Level lvl)
        {
            RunOnUI_Async(() => 
            {
                Main_UpdateMapList();
                Map_UpdateLoadedList();
                Map_UpdateUnloadedList();
            });
        }
        public void Level_LevelRemoved(Level lvl)
        {
            RunOnUI_Async(() => 
            {
                Main_UpdateMapList();
                Map_UpdateLoadedList();
                Map_UpdateUnloadedList();
            });
        }
        public void Level_PhysicsLevelChanged(Level lvl, int level)
        {
            RunOnUI_Async(() => 
            {
                Main_UpdateMapList();
                Map_UpdateLoadedList();
            });
        }
        public void SettingsUpdate()
        {
            RunOnUI_Async(() => 
            {
                if (Server.shuttingDown)
                {
                    return;
                }
                Text = Colors.Strip(Server.Config.Name) + " - " + Colors.Strip(Server.SoftwareNameVersioned);
                UpdateNotifyIconText();
            });
        }
        public void PopupNotify(string message, ToolTipIcon icon = ToolTipIcon.Info)
        {
            GUINotifyIcon.ShowBalloonTip(3000, Colors.Strip(Server.Config.Name), message, icon);
        }
        public void UpdateUrl(string s)
        {
            RunOnUI_Async(() => 
            { 
                Main_UpdateUrl(s); 
            });
        }
        public void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                Server.Stop(false, "Server shutdown - PC turning off");
                GUINotifyIcon.Dispose();
            }
            if (Server.shuttingDown || Popup.OKCancel("Really shutdown the server? All players will be disconnected!", "Exit"))
            {
                Server.Stop(false, Server.Config.DefaultShutdownMessage);
                GUINotifyIcon.Dispose();
            }
            else
            {
                // Prevents form from closing when user clicks the X and then hits 'cancel'
                e.Cancel = true;
            }
        }
        public void BtnClose_Click(object sender, EventArgs e) 
        { 
            Close(); 
        }
        public void BtnProperties_Click(object sender, EventArgs e)
        {
            if (!HasPropsForm)
            {
                PropsForm = new PropertyWindow();
                HasPropsForm = true;
            }
            PropsForm.Show();
            if (!PropsForm.Focused)
            {
                PropsForm.Focus();
            }
        }
        public static bool HasPropsForm;
        public PropertyWindow PropsForm;
        public bool AlwaysInTaskbar = true;
        public void Window_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = AlwaysInTaskbar;
        }
        public void Icon_HideWindow_Click(object sender, EventArgs e)
        {
            AlwaysInTaskbar = !AlwaysInTaskbar;
            ShowInTaskbar = AlwaysInTaskbar;
            Icon_hideWindow.Text = AlwaysInTaskbar ? "Hide from taskbar" : "Show in taskbar";
        }
        public void Icon_OpenTerminal_Click(object sender, EventArgs e)
        {
            Show();
            BringToFront();
            WindowState = FormWindowState.Normal;
        }
        public void Icon_Shutdown_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void Icon_restart_Click(object sender, EventArgs e)
        {
            Main_BtnRestart_Click(sender, e);
        }
        public void Tabs_Click(object sender, EventArgs e)
        {
            try 
            { 
                Map_UpdateUnloadedList(); 
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            try 
            { 
                Players_UpdateList(); 
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            try
            {
                if (Logs_txtGeneral.Text.Length == 0)
                {
                    Logs_dateGeneral.Value = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            foreach (TabPage page in Tabs.TabPages)
            {
                foreach (Control control in page.Controls)
                {
                    if (!control.GetType().IsSubclassOf(typeof(TextBox))) 
                    {
                        continue;
                    }
                    control.Update();
                }
            }
            Tabs.Update();
        }
        public void Main_players_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }
    }
}