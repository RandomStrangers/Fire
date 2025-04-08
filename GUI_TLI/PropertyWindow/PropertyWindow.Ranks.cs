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
using System.Windows.Forms;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public bool RankSupressEvents = false;
        public void LoadRankProps()
        {
            LoadDefaultRank();
            GuiPerms.SetRanks(Rank_cmbOsMap);
            GuiPerms.SetSelectedRank(Rank_cmbOsMap, Server.Config.OSPerbuildDefault);
            Rank_cbTPHigher.Checked = Server.Config.HigherRankTP;
            Rank_cbSilentAdmins.Checked = Server.Config.AdminsJoinSilently;
            Rank_cbEmpty.Checked = Server.Config.ListEmptyRanks;
        }
        public void LoadDefaultRank()
        {
            Rank_cmbDefault.Items.Clear();
            foreach (Group group in Group.GroupList)
            {
                Rank_cmbDefault.Items.Add(group.Name);
            }
            Rank_cmbDefault.SelectedItem = Server.Config.DefaultRankName;
            if (Rank_cmbDefault.SelectedItem == null)
            {
                Rank_cmbDefault.SelectedIndex = 1; // guest rank (usually) TODO rethink
            }
        }
        public void ApplyRankProps()
        {
            Server.Config.DefaultRankName = Rank_cmbDefault.SelectedItem.ToString();
            Server.Config.OSPerbuildDefault = GuiPerms.GetSelectedRank(Rank_cmbOsMap, LevelPermission.Owner);
            Server.Config.HigherRankTP = Rank_cbTPHigher.Checked;
            Server.Config.AdminsJoinSilently = Rank_cbSilentAdmins.Checked;
            Server.Config.ListEmptyRanks = Rank_cbEmpty.Checked;
        }
        public List<Group> CopiedGroups = new List<Group>();
        public Group CurGroup;
        public void LoadRanks()
        {
            Rank_list.Items.Clear();
            CopiedGroups.Clear();
            CurGroup = null;
            foreach (Group grp in Group.GroupList)
            {
                CopiedGroups.Add(grp.CopyConfig());
                Rank_list.Items.Add(grp.Name + " = " + (int)grp.Permission);
            }
            Rank_list.SelectedIndex = 0;
        }
        public void SaveRanks()
        {
            Group.SaveAll(CopiedGroups);
            Group.LoadAll();
            LoadRanks();
        }
        public void Rank_btnColor_Click(object sender, EventArgs e)
        {
            Chat_ShowColorDialog(Rank_btnColor, CurGroup.Name + " rank color");
            CurGroup.Color = Colors.Parse(Rank_btnColor.Text);
        }
        public void Rank_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RankSupressEvents)
            {
                return;
            }
            CurGroup = null;
            if (Rank_list.SelectedIndex == -1)
            {
                return;
            }
            Group grp = CopiedGroups[Rank_list.SelectedIndex];
            CurGroup = grp;
            Rank_txtName.Text = grp.Name;
            Rank_numPerm.Value = (int)grp.Permission;
            Chat_ParseColor(grp.Color, Rank_btnColor);
            Rank_txtMOTD.Text = grp.MOTD;
            Rank_txtPrefix.Text = grp.Prefix;
            Rank_cbAfk.Checked = grp.AfkKicked;
            Rank_numAfk.Value = grp.AfkKickTime;
            Rank_numDraw.Value = grp.DrawLimit;
            Rank_numUndo.Value = grp.MaxUndo;
            Rank_numMaps.Value = grp.OverseerMaps;
            Rank_numGen.Value = grp.GenVolume;
            Rank_numCopy.Value = grp.CopySlots;
        }
        public void Rank_txtName_TextChanged(object sender, EventArgs e)
        {
            if (Rank_txtName.Text.IndexOf(' ') > 0)
            {
                Rank_txtName.Text = Rank_txtName.Text.Replace(" ", "");
                return;
            }
            if (Rank_txtName.Text.Length == 0)
            {
                return;
            }
            CurGroup.Name = Rank_txtName.Text;
            RankSupressEvents = true;
            Rank_list.Items[Rank_list.SelectedIndex] = Rank_txtName.Text + " = " + (int)CurGroup.Permission;
            RankSupressEvents = false;
        }
        public void Rank_numPerm_ValueChanged(object sender, EventArgs e)
        {
            int perm = (int)Rank_numPerm.Value;
            CurGroup.Permission = (LevelPermission)perm;
            RankSupressEvents = true;
            Rank_list.Items[Rank_list.SelectedIndex] = CurGroup.Name + " = " + perm;
            RankSupressEvents = false;
        }
        public void Rank_txtMOTD_TextChanged(object sender, EventArgs e)
        {
            CurGroup.MOTD = Rank_txtMOTD.Text;
        }
        public void Rank_txtPrefix_TextChanged(object sender, EventArgs e)
        {
            CurGroup.Prefix = Rank_txtPrefix.Text;
        }
        public void Rank_cbAfk_CheckedChanged(object sender, EventArgs e)
        {
            CurGroup.AfkKicked = Rank_cbAfk.Checked;
            Rank_numAfk.Enabled = Rank_cbAfk.Checked;
        }
        public void Rank_numAfk_ValueChanged(object sender, EventArgs e)
        {
            CurGroup.AfkKickTime = Rank_numAfk.Value;
        }
        public void Rank_numDraw_ValueChanged(object sender, EventArgs e)
        {
            CurGroup.DrawLimit = (int)Rank_numDraw.Value;
        }
        public void Rank_numUndo_ValueChanged(object sender, EventArgs e)
        {
            CurGroup.MaxUndo = Rank_numUndo.Value;
        }
        public void Rank_numMaps_ValueChanged(object sender, EventArgs e)
        {
            CurGroup.OverseerMaps = (int)Rank_numMaps.Value;
        }
        public void Rank_numGen_ValueChanged(object sender, EventArgs e)
        {
            CurGroup.GenVolume = (int)Rank_numGen.Value;
        }
        public void Rank_numCopy_ValueChanged(object sender, EventArgs e)
        {
            CurGroup.CopySlots = (int)Rank_numCopy.Value;
        }
        public void Rank_btnAdd_Click(object sender, EventArgs e)
        {
            // Find first free rank permission
            int perm = 5;
            for (int i = (int)LevelPermission.Guest; i <= (int)LevelPermission.Flames; i++)
            {
                if (PermissionFree(i)) 
                { 
                    perm = i; 
                    break; 
                }
            }
            Group newGroup = Group.DefaultRank.CopyConfig();
            newGroup.Permission = (LevelPermission)perm;
            newGroup.Name = "CHANGEME_" + perm;
            newGroup.Color = "&1";
            CopiedGroups.Add(newGroup);
            Rank_list.Items.Add(newGroup.Name + " = " + (int)newGroup.Permission);
        }
        public void Rank_btnDel_Click(object sender, EventArgs e)
        {
            if (Rank_list.Items.Count == 0)
            {
                return;
            }
            CopiedGroups.RemoveAt(Rank_list.SelectedIndex);
            RankSupressEvents = true;
            Rank_list.Items.RemoveAt(Rank_list.SelectedIndex);
            RankSupressEvents = false;
            int i = Rank_list.Items.Count > 0 ? 0 : -1;
            Rank_list.SelectedIndex = i;
        }
        public bool PermissionFree(int i)
        {
            foreach (Group grp in CopiedGroups)
            {
                if (grp.Permission == (LevelPermission)i)
                {
                    return false;
                }
            }
            return true;
        }
    }
}