/*
   Copyright 2015-2024 MCGalaxy

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
using Flames.Blocks;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public ushort CurBlock;
        public List<ushort> BlockIDMap;
        public ItemPermsHelper BlockItems = new ItemPermsHelper();
        // need to keep a list of changed block perms, because we don't want
        // to modify the server's live permissions if user clicks 'discard'
        public BlockPerms BlockPermsOrig, BlockPermsCopy;
        public List<BlockPerms> BlockPermsChanged = new List<BlockPerms>();
        public BlockProps[] BlockPropsChanged = new BlockProps[Block.Props.Length];
        public void LoadBlocks()
        {
            Blk_list.Items.Clear();
            BlockPermsChanged.Clear();
            BlockIDMap = new List<ushort>();
            for (int b = 0; b < BlockPropsChanged.Length; b++)
            {
                BlockPropsChanged[b] = Block.Props[b];
                BlockPropsChanged[b].ChangedScope = 0;
                ushort block = (ushort)b;
                if (!Block.ExistsGlobal(block)) continue;
                string name = Block.GetName(Player.Flame, block);
                Blk_list.Items.Add(name);
                BlockIDMap.Add(block);
            }
            BlockItems.GetCurPerms = BlockGetOrAddPermsChanged;
            if (Blk_list.SelectedIndex == -1)
            {
                Blk_list.SelectedIndex = 0;
            }
        }
        public void SaveBlocks()
        {
            if (BlockPermsChanged.Count > 0)
            {
                SaveBlockPermissions();
            }
            if (AnyBlockPropsChanged())
            {
                SaveBlockProps();
            }
            LoadBlocks();
        }
        public void SaveBlockPermissions()
        {
            foreach (BlockPerms changed in BlockPermsChanged)
            {
                BlockPerms orig = BlockPerms.Find(changed.ID);
                changed.CopyPermissionsTo(orig);
            }
            BlockPerms.Save();
            BlockPerms.ApplyChanges();
            BlockPerms.ResendAllBlockPermissions();
        }
        public bool AnyBlockPropsChanged()
        {
            for (int b = 0; b < BlockPropsChanged.Length; b++)
            {
                if (BlockPropsChanged[b].ChangedScope != 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void SaveBlockProps()
        {
            for (int b = 0; b < BlockPropsChanged.Length; b++)
            {
                if (BlockPropsChanged[b].ChangedScope == 0) continue;
                Block.Props[b] = BlockPropsChanged[b];
            }
            BlockProps.Save("default", Block.Props, 1);
            Block.SetBlocks();
        }
        public void Blk_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurBlock = BlockIDMap[Blk_list.SelectedIndex];
            BlockPermsOrig = BlockPerms.Find(CurBlock);
            BlockPermsCopy = BlockPermsChanged.Find(p => p.ID == CurBlock);
            BlockInitSpecificArrays();
            BlockItems.SupressEvents = true;
            BlockProps props = BlockPropsChanged[CurBlock];
            Blk_cbMsgBlock.Checked = props.IsMessageBlock;
            Blk_cbPortal.Checked = props.IsPortal;
            Blk_cbDeath.Checked = props.KillerBlock;
            Blk_txtDeath.Text = props.DeathMessage;
            Blk_txtDeath.Enabled = Blk_cbDeath.Checked;
            Blk_cbDoor.Checked = props.IsDoor;
            Blk_cbTdoor.Checked = props.IsTDoor;
            Blk_cbRails.Checked = props.IsRails;
            Blk_cbLava.Checked = props.LavaKills;
            Blk_cbWater.Checked = props.WaterKills;
            BlockPerms perms = BlockPermsCopy ?? BlockPermsOrig;
            BlockItems.Update(perms);
        }
        public void BlockInitSpecificArrays()
        {
            if (BlockItems.MinBox != null)
            {
                return;
            }
            BlockItems.MinBox = Blk_cmbMin;
            BlockItems.AllowBoxes = new ComboBox[] 
            { 
                Blk_cmbAlw1, Blk_cmbAlw2, Blk_cmbAlw3 
            };
            BlockItems.DisallowBoxes = new ComboBox[] 
            { 
                Blk_cmbDis1, Blk_cmbDis2, Blk_cmbDis3 
            };
            BlockItems.FillInitial();
        }
        public ItemPerms BlockGetOrAddPermsChanged()
        {
            if (BlockPermsCopy != null)
            {
                return BlockPermsCopy;
            }
            BlockPermsCopy = BlockPermsOrig.Copy();
            BlockPermsChanged.Add(BlockPermsCopy);
            return BlockPermsCopy;
        }
        public void Blk_cmbMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlockItems.OnMinRankChanged((ComboBox)sender);
        }
        public void Blk_cmbSpecific_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlockItems.OnSpecificChanged((ComboBox)sender);
        }
        public void Blk_btnHelp_Click(object sender, EventArgs e)
        {
            GetHelp(Blk_list.SelectedItem.ToString());
        }
        public void Blk_cbMsgBlock_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].IsMessageBlock = Blk_cbMsgBlock.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_cbPortal_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].IsPortal = Blk_cbPortal.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_cbDeath_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].KillerBlock = Blk_cbDeath.Checked;
            Blk_txtDeath.Enabled = Blk_cbDeath.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_txtDeath_TextChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].DeathMessage = Blk_txtDeath.Text;
            MarkBlockPropsChanged();
        }
        public void Blk_cbDoor_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].IsDoor = Blk_cbDoor.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_cbTdoor_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].IsTDoor = Blk_cbTdoor.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_cbRails_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].IsRails = Blk_cbRails.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_cbLava_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].LavaKills = Blk_cbLava.Checked;
            MarkBlockPropsChanged();
        }
        public void Blk_cbWater_CheckedChanged(object sender, EventArgs e)
        {
            BlockPropsChanged[CurBlock].WaterKills = Blk_cbWater.Checked;
            MarkBlockPropsChanged();
        }
        public void MarkBlockPropsChanged()
        {
            // don't mark props as changed when supressing events
            int changed = BlockItems.SupressEvents ? 0 : BlockProps.SCOPE_GLOBAL;
            BlockPropsChanged[CurBlock].ChangedScope = (byte)changed;
        }
    }
}