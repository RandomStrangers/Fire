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
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Flames.Scripting;
using Flames.Added.Compiling;
using Flames.Added;
namespace Flames.Gui.Popups
{
    public partial class CustomOrders : Form
    {

        public CustomOrders()
        {
            InitializeComponent();
            LoadCompilers();

            //Sigh. I wish there were SOME event to help me.
            foreach (Order ord in Order.allOrds)
            {
                if (!Order.IsCore(ord)) 
                {
                    LstOrders.Items.Add(ord.Name);
                }
            }
        }

        public void CustomOrders_Load(object sender, EventArgs e)
        {
            GuiUtils.SetIcon(this);
        }

        public void LoadCompilers()
        {
            Button[] buttons = 
            { 
                BtnCreate1, BtnCreate2, BtnCreate3, BtnCreate4, BtnCreate5 
            };
            List<ICompiler> compilers = ICompiler.Compilers;
            int i;
            for (i = 0; i < Math.Min(compilers.Count, buttons.Length); i++)
            {
                // must be copied to local variable because of the way C# for loop closures work,
                //  as otherwise the delegate { ... compilers[i] ... } uses compiler 
                //   from LAST iteration instead of the current iteration
                ICompiler compiler = compilers[i];
                buttons[i].Visible = true;
                buttons[i].Text = "Create " + compiler.ShortName;
                buttons[i].Click += delegate 
                { 
                    CreateOrder(compiler); 
                };
            }

            for (; i < buttons.Length; i++) buttons[i].Visible = false;
        }

        public void CreateOrder(ICompiler compiler)
        {
            string ordName = TxtOrdName.Text.Trim();
            if (ordName.Length == 0)
            {
                Popup.Warning("Order must have a name"); 
                return;
            }

            string path = compiler.OrderPath(ordName);
            if (File.Exists(path))
            {
                Popup.Warning("Order already exists");
                return;
            }

            try
            {
                string source = compiler.GenExampleOrder(ordName);
                File.WriteAllText(path, source);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Popup.Error("Failed to generate order. Check error logs for more details.");
                return;
            }
            Popup.Message("Order Ord" + ordName + compiler.FileExtension + " created.");
        }

        public void btnLoad_Click(object sender, EventArgs e)
        {
            string path;

            using (FileDialog dialog = new OpenFileDialog())
            {
                dialog.RestoreDirectory = true;
                dialog.Filter = GetFilterText();

                if (dialog.ShowDialog() != DialogResult.OK) return;
                path = dialog.FileName;
            }
            if (!File.Exists(path)) return;

            if (path.CaselessEnds(".dll"))
            {
                LoadOrders(path); 
                return;
            }

            // compile to temp .dll and load that
            string tmp = CompileOrders(path);
            if (tmp == null) return;
            LoadOrders(tmp);
            DeleteAssembly(tmp);
        }

        public void btnUnload_Click(object sender, EventArgs e)
        {
            string ordName = LstOrders.SelectedItem.ToString();
            Order ord = Order.FindORD(ordName);
            if (ord == null)
            {
                Popup.Warning("Order " + ordName + " is not Loaded."); 
                return;
            }

            LstOrders.Items.Remove(ord.Name);
            Order.Unregister(ord);
            Popup.Message("Order successfully unloaded.");
        }

        public void lstOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnUnload.Enabled = LstOrders.SelectedIndex != -1;
        }


        public void LoadOrders(string path)
        {
            Assembly lib = IScripting.LoadAssembly(path);
            if (lib == null) return;
            List<Order> orders = IScripting.LoadTypes<Order>(lib);

            for (int i = 0; i < orders.Count; i++)
            {
                Order ord = orders[i];

                if (LstOrders.Items.Contains(ord.Name))
                {
                    Popup.Warning("Order " + ord.Name + " already exists, so was not Loaded");
                    continue;
                }

                LstOrders.Items.Add(ord.Name);
                Order.Register(ord);
                Logger.Log(LogType.SystemActivity, "Added /" + ord.Name + " to orders");
            }
        }

        public string CompileOrders(string path)
        {
            ICompiler compiler = GetCompiler(path);
            if (compiler == null)
            {
                Popup.Warning("Unsupported file '" + path + "'");
                return null;
            }

            string tmp = "orders/TMP_" + Path.GetRandomFileName() + ".dll";
            FlamesHelpPlayer p = new FlamesHelpPlayer();
            if (CompilerOperations.Compile(p, compiler, "Order", new[] { path }, tmp))
                return tmp;

            Popup.Error(Colors.StripUsed(p.Messages));
            DeleteAssembly(tmp);
            return null;
        }

        public static ICompiler GetCompiler(string path)
        {
            foreach (ICompiler c in ICompiler.Compilers)
            {
                if (path.CaselessEnds(c.FileExtension)) return c;
            }
            return null;
        }

        public static void DeleteAssembly(string path)
        {
            try 
            { 
                File.Delete(path); 
            } 
            catch 
            { 
            }
            try 
            { 
                File.Delete(path.Replace(".dll", ".pdb")); 
            } 
            catch 
            {
            }
            try 
            { 
                File.Delete(path + ".mdb"); 
            } 
            catch 
            { 
            }
        }


        public static string ListCompilers(StringFormatter<ICompiler> formatter)
        {
            return ICompiler.Compilers.Join(formatter, "");
        }

        public static string GetFilterText()
        {
            StringBuilder sb = new StringBuilder();
            // Returns e.g. "Accepted File Types (*.cs, *.dll)|*.cs;*.dll|C# Source (*.cs)|*.cs|.NET Assemblies (*.dll)|*.dll";

            sb.AppendFormat("Accepted File Types ({0}*.dll)|",
                            ListCompilers(c => string.Format("*{0}, ", c.FileExtension)));

            sb.AppendFormat("{0}*.dll|",
                            ListCompilers(c => string.Format("*{0};", c.FileExtension)));

            sb.AppendFormat("{0}.NET Assemblies (*.dll)|*.dll",
                            ListCompilers(c => string.Format("{0} Source (*{1})|*{1}|", c.FullName, c.FileExtension)));
            return sb.ToString();
        }
    }
}