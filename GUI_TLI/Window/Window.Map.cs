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
using System.Threading;
using System.Windows.Forms;
using Flames.Added.UI;
namespace Flames.Gui
{
    public partial class Window : Form
    {
        public void Map_BtnGen_Click(object sender, EventArgs e)
        {
            if (Mapgen)
            {
                Popup.Warning("Another map is already being generated.");
                return;
            }
            string name = Map_txtName.Text;
            string seed = Map_txtSeed.Text;
            if (string.IsNullOrEmpty(name))
            {
                Popup.Warning("Map name cannot be blank.");
                return;
            }
            string x = Map_GetComboboxSize(Map_cmbX, "width");
            if (x == null)
            {
                return;
            }
            string y = Map_GetComboboxSize(Map_cmbY, "height");
            if (y == null)
            {
                return;
            }
            string z = Map_GetComboboxSize(Map_cmbZ, "length");
            if (z == null)
            {
                return; 
            }
            string type = Map_GetComboboxItem(Map_cmbType, "type");
            if (type == null) 
            { 
                return;
            }
            string args = name + " " + x + " " + y + " " + z + " " + type;
            if (!string.IsNullOrEmpty(seed))
            {
                args += " " + seed;
            }
            Thread genThread = new Thread(() => DoGen(name, args))
            {
                Name = "GuiGenMap"
            };
            genThread.Start();
        }
        public void DoGen(string name, string args)
        {
            Mapgen = true;
            try
            {
                Order.Find("NewLvl").Use(Player.Flame, args);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Popup.Error("Failed to generate level. Check error logs for details.");
                Mapgen = false;
                return;
            }
            if (LevelInfo.MapExists(name))
            {
                Popup.Message("Level successfully generated.");
                RunOnUI_Async(() => 
                {
                    Map_UpdateUnloadedList();
                    Map_UpdateLoadedList();
                    Main_UpdateMapList();
                });
            }
            else
            {
                Popup.Error("Level was not generated. Check main log for details.");
            }
            Mapgen = false;
        }
        public string Map_GetComboboxItem(ComboBox box, string propName)
        {
            object selected = box.SelectedItem;
            string value = selected == null ? "" : selected.ToString();
            if (value.Length == 0)
            {
                Popup.Warning("Map " + propName + " cannot be blank.");
                return null;
            }
            return value;
        }
        public string Map_GetComboboxSize(ComboBox box, string propName)
        {
            string value = box.Text;
            if (value.Length == 0)
            {
                Popup.Warning("Map " + propName + " cannot be blank.");
                return null;
            }
            if (!ushort.TryParse(value, out ushort size) || size == 0 || size > 16384)
            {
                Popup.Warning("Map " + propName + " must be an integer between 1 and 16384");
                return null;
            }
            return value;
        }
        public void Map_BtnLoad_Click(object sender, EventArgs e)
        {
            object selected = Map_lbUnloaded.SelectedItem;
            if (selected == null) 
            { 
                Popup.Warning("No unloaded level selected."); 
                return; 
            }
            UIHelpers.HandleOrder("Load " + selected.ToString());
        }

        public string Last = null;
        public void Map_UpdateSelected(object sender, EventArgs e)
        {
            if (Map_lbLoaded.SelectedItem == null)
            {
                if (Map_pgProps.SelectedObject == null)
                {
                    return;
                }
                Map_pgProps.SelectedObject = null; 
                Last = null;
                Map_gbProps.Text = "Properties for (none selected)";
                return;
            }
            string name = Map_lbLoaded.SelectedItem.ToString();
            Level lvl = LevelInfo.FindExact(name);
            if (lvl == null)
            {
                if (Map_pgProps.SelectedObject == null)
                {
                    return;
                }
                Map_pgProps.SelectedObject = null; 
                Last = null;
                Map_gbProps.Text = "Properties for (none selected)"; 
                return;
            }
            if (name == Last)
            {
                return;
            }
            Last = name;
            LevelProperties settings = new LevelProperties(lvl);
            Map_pgProps.SelectedObject = settings;
            Map_gbProps.Text = "Properties for " + name;
        }
        public void Map_UpdateUnloadedList()
        {
            object selected = Map_lbUnloaded.SelectedItem;
            Map_lbUnloaded.Items.Clear();
            string[] allMaps = LevelInfo.AllMapNames();
            foreach (string map in allMaps)
            {
                if (LevelInfo.FindExact(map) == null)
                {
                    Map_lbUnloaded.Items.Add(map);
                }
            }
            Map_Reselect(Map_lbUnloaded, selected);
        }
        public void Map_UpdateLoadedList()
        {
            object selected = Map_lbLoaded.SelectedItem;
            Map_lbLoaded.Items.Clear();
            Level[] loaded = LevelInfo.Loaded.Items;
            foreach (Level lvl in loaded)
            {
                Map_lbLoaded.Items.Add(lvl.name);
            }
            Map_Reselect(Map_lbLoaded, selected);
            Map_UpdateSelected(null, null);
        }
        public void Map_Reselect(ListBox box, object selected)
        {
            int i = -1;
            if (selected != null)
            {
                i = box.Items.IndexOf(selected);
            }
            box.SelectedIndex = i;
        }
    }
}