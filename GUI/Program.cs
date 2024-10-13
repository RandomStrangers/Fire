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
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Flames.Gui 
{
    public static class Program 
    {   
        [STAThread]
        public static void Main(string[] args) {
            SetCurrentDirectory();

            // separate method, in case Flames_.dll is missing
            try {
                StartGUI();
            } catch (FileNotFoundException) {
                // If Flames_.dll is missing, a FileNotFoundException will get thrown for Flames dll
                Popup.Error("Cannot start server as Flames_.dll is missing from " + Environment.CurrentDirectory
                            + "\n\nDownload it from " + Updater.UploadsURL);
                return;
            }    
        }

        public static void SetCurrentDirectory() {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try {
                Environment.CurrentDirectory = path;
            } catch {
                // assembly.Location usually gives full path of the .exe, but has issues with mkbundle
                //   https://mono-devel-list.ximian.narkive.com/KfCAxY1F/mkbundle-assembly-getentryassembly
                //   https://stackoverflow.com/questions/57648241/reliably-get-location-of-bundled-executable-on-linux
                // Rather than trying to guess when this issue happens, just don't bother at all
                //  (since most users will not be trying to run .exe from a different folder anyways)
            }
        }

        public static void StartGUI() {
            FileLogger.Init();
            Server.RestartPath = Application.ExecutablePath;
            AppDomain.CurrentDomain.UnhandledException += GlobalExHandler;
            Application.ThreadException += ThreadExHandler;
            DetectBuggyCursors();

            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                Application.Run(new Window());
            } catch (Exception e) {
                Logger.LogError(e);
            }
        }

        public static void LogAndRestart(Exception ex) {
            Logger.LogError(ex);
            FileLogger.Flush(null);
            
            Thread.Sleep(500);
            if (Server.Config.restartOnError) {
                Thread stopThread = Server.Stop(true, "Server restart - unhandled error");
                stopThread.Join();
            }
        }

        public static void GlobalExHandler(object sender, UnhandledExceptionEventArgs e) {
            LogAndRestart((Exception)e.ExceptionObject);
        }

        public static void ThreadExHandler(object sender, ThreadExceptionEventArgs e) {
            LogAndRestart(e.Exception);
        }

        public static void DetectBuggyCursors() {
            // In very rare cases, trying to create SizeNWSE cursor on Mono on Linux will throw an ArgumentException
            // Message: A null reference or invalid value was found[GDI + status: InvalidParameter]
            //   ..
            //   at System.Drawing.Bitmap..ctor(System.Drawing.Image, System.Drawing.Size)
            //   at System.Windows.Forms.XplatUIX11.DefineCursor(System.Drawing.Bitmap bitmap, System.Drawing.Bitmap mask, System.Drawing.Color cursor_pixel, System.Drawing.Color mask_pixel, System.Int32 xHotSpot, System.Int32 yHotSpot)[0x00045] in < 28e46de2d20c496895000ef0abfc2106 >:0
            //   at System.Windows.Forms.XplatUI.DefineCursor(System.Drawing.Bitmap bitmap, System.Drawing.Bitmap mask, System.Drawing.Color cursor_pixel, System.Drawing.Color mask_pixel, System.Int32 xHotSpot, System.Int32 yHotSpot)[0x00000] in < 28e46de2d20c496895000ef0abfc2106 >:0
            //   at System.Windows.Forms.Cursor.CreateCursor(System.IO.Stream stream)[0x00058] in < 28e46de2d20c496895000ef0abfc2106 >:0
            //   at System.Windows.Forms.Cursor..ctor(System.Type type, System.String resource)[0x0001e] in < 28e46de2d20c496895000ef0abfc2106 >:0
            //   at System.Windows.Forms.Cursors.get_SizeNWSE()[0x00014] in < 28e46de2d20c496895000ef0abfc2106 >:0
            //   ..
            // However, some X11 video drivers will cause XQueryBestCursor to return width/height 0,
            //  which will then cause the subsequent 'new Bitmap(width, height)' in XplatUIX11.DefineCursor to fail
            // See https://github.com/ClassiCube/MCGalaxy/issues/658 for more details
            try {
                Cursor c = Cursors.SizeNWSE;
            } catch (ArgumentException ex) {
                Logger.LogError("checking Cursors", ex);
                Popup.Warning("Video driver appears to be returning buggy cursor sizes\n\nAttempting to workaround this issue (might not work)");
                try { BypassCursorsHACK(); } catch { }
            } catch (Exception ex) {
                Logger.LogError("checking Cursors", ex);
            } 
        }

        public static void BypassCursorsHACK() {
            if (!Server.RunningOnMono()) return;
            Type stdCursorType = typeof(Cursor).Assembly.GetType("System.Windows.Forms.StdCursor");

            // cursor = new Cursor(StdCursor type) { .. }
            ConstructorInfo cursor_cons = typeof(Cursor).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { stdCursorType }, null);
            object cursor = cursor_cons.Invoke(new object[] { 23 }); // StdCursor.SizeNWSE

            // Cursors.NWSE = cursor;
            FieldInfo nwse_field = typeof(Cursors).GetField("size_nwse", BindingFlags.NonPublic | BindingFlags.Static);
            nwse_field.SetValue(null, cursor);
        }
    }
}