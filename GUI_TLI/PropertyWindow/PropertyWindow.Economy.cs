/*
    Copyright 2011 MCForge
        
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
using System.ComponentModel;
using System.Windows.Forms;
using Flames.Eco;
using Flames.Generator;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public void LoadEcoProps()
        {
            Eco_cbEnabled.Checked = Economy.Enabled;
            Eco_txtCurrency.Text = Server.Config.Currency;
            Eco_UpdateEnables();
            foreach (Item item in Economy.Items)
            {
                Eco_cmbCfg.Items.Add(item.Name);
            }
            Eco_cmbCfg.Items.Add("(none)");
            Eco_cmbCfg.SelectedIndex = Eco_cmbCfg.Items.Count - 1;
            GuiPerms.SetRanks(Eco_cmbItemRank);
            Eco_colRankPrice.CellTemplate = new NumericalCell();
            Eco_dgvRanks.DataError += Eco_dgv_DataError;
            Eco_colLvlPrice.CellTemplate = new NumericalCell();
            Eco_colLvlX.CellTemplate = new NumericalCell();
            Eco_colLvlY.CellTemplate = new NumericalCell();
            Eco_colLvlZ.CellTemplate = new NumericalCell();
            Eco_colLvlTheme.CellTemplate = new ThemeCell();
            foreach (MapGen gen in MapGen.Generators)
            {
                if (gen.Type == GenType.Advanced) 
                {
                    continue;
                }
                Eco_colLvlTheme.Items.Add(gen.Theme);
            }
            Eco_dgvMaps.DataError += Eco_dgv_DataError;
        }
        public void ApplyEcoProps()
        {
            Economy.Enabled = Eco_cbEnabled.Checked;
            Server.Config.Currency = Eco_txtCurrency.Text;
        }
        public class NumericalCell : DataGridViewTextBoxCell
        {
            protected override bool SetValue(int rowIndex, object raw)
            {
                if (raw == null)
                {
                    return true;
                }
                string str = raw.ToString(); 
                if (!int.TryParse(str, out int num) || num < 0)
                {
                    return false;
                }
                return base.SetValue(rowIndex, raw);
            }
        }
        public class ThemeCell : DataGridViewComboBoxCell
        {
            protected override object GetFormattedValue(object value, int rowIndex,
                                                        ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter,
                                                        TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
            {
                MapGen gen = MapGen.Find((string)value);
                return gen?.Theme;
            }
        }
        public void Eco_UpdateEnables()
        {
            Eco_lblCurrency.Enabled = Eco_cbEnabled.Checked;
            Eco_txtCurrency.Enabled = Eco_cbEnabled.Checked;
            Eco_lblCfg.Enabled = Eco_cbEnabled.Checked;
            Eco_cmbCfg.Enabled = Eco_cbEnabled.Checked;
            Eco_gbItem.Enabled = Eco_cbEnabled.Checked;
            Eco_gbLvl.Enabled = Eco_cbEnabled.Checked;
            Eco_gbRank.Enabled = Eco_cbEnabled.Checked;
        }
        public void Eco_cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Eco_UpdateEnables();
        }
        public void Eco_cmbCfg_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = "(none)";
            if (Eco_cmbCfg.SelectedIndex != -1)
            {
                text = Eco_cmbCfg.SelectedItem.ToString();
            }
            Eco_gbItem.Visible = false;
            Eco_gbLvl.Visible = false;
            Eco_gbRank.Visible = false;
            Eco_curItem = null;
            Item item = Economy.GetItem(text);
            if (text == "(none)" || item == null)
            {
                return;
            }
            if (item == Economy.Levels)
            {
                Eco_gbLvl.Visible = true;
                Eco_cbLvl.Checked = Economy.Levels.Enabled;
                Eco_UpdateLevels();
            }
            else if (item == Economy.Ranks)
            {
                Eco_gbRank.Visible = true;
                Eco_cbRank.Checked = Economy.Ranks.Enabled;
                Eco_UpdateRanks();
            }
            else if (item is SimpleItem item1)
            {
                Eco_gbItem.Visible = true;
                Eco_curItem = item1;
                Eco_cbItem.Checked = item.Enabled;
                Eco_UpdateItem();
            }
        }
        public SimpleItem Eco_curItem;
        public void Eco_UpdateItemEnables()
        {
            Eco_lblItemPrice.Enabled = Eco_cbItem.Checked;
            Eco_numItemPrice.Enabled = Eco_cbItem.Checked;
            Eco_lblItemRank.Enabled = Eco_cbItem.Checked;
            Eco_cmbItemRank.Enabled = Eco_cbItem.Checked;
        }
        public void Eco_UpdateItem()
        {
            Eco_gbItem.Text = Eco_curItem.Name;
            Eco_numItemPrice.Value = Eco_curItem.Price;
            Eco_UpdateItemEnables();
            GuiPerms.SetSelectedRank(Eco_cmbItemRank, Eco_curItem.PurchaseRank);
        }
        public void Eco_cbItem_CheckedChanged(object sender, EventArgs e)
        {
            Eco_UpdateItemEnables();
            Eco_curItem.Enabled = Eco_cbItem.Checked;
        }
        public void Eco_numItemPrice_ValueChanged(object sender, EventArgs e)
        {
            Eco_curItem.Price = (int)Eco_numItemPrice.Value;
        }
        public void Eco_cmbItemRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            const LevelPermission perm = LevelPermission.Guest;
            if (Eco_curItem == null)
            {
                return;
            }
            Eco_curItem.PurchaseRank = GuiPerms.GetSelectedRank(Eco_cmbItemRank, perm);
        }
        public void Eco_UpdateRankEnables()
        {
            Eco_dgvRanks.Enabled = Eco_cbRank.Enabled;
        }
        public void Eco_UpdateRanks()
        {
            Eco_dgvRanks.Rows.Clear();
            foreach (Group grp in Group.GroupList)
            {
                RankItem.RankEntry rank = Economy.Ranks.Find(grp.Permission);
                int price = rank == null ? 0 : rank.Price;
                int idx = Eco_dgvRanks.Rows.Add(grp.Name, price);
                Eco_dgvRanks.Rows[idx].Tag = grp.Permission;
            }
            Eco_UpdateRankEnables();
        }
        public void Eco_cbRank_CheckedChanged(object sender, EventArgs e)
        {
            Eco_UpdateRankEnables();
            Economy.Ranks.Enabled = Eco_cbRank.Checked;
        }
        public void Eco_dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string col = Eco_dgvMaps.Columns[e.ColumnIndex].HeaderText;
            if (e.ColumnIndex > 0)
            {
                Popup.Warning(col + " must be an integer greater than zero");
            }
            else
            {
                Popup.Warning("Error setting contents of column " + col);
            }
        }

        public void Eco_dgvRanks_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            DataGridViewRow row = Eco_dgvRanks.Rows[e.RowIndex];
            object price = row.Cells[1].Value;
            // On Mono this event is raised during initialising cells too
            // However, first time event is raised, price is not initialised yet
            if (price == null)
            {
                return;
            }
            LevelPermission perm = (LevelPermission)row.Tag;
            RankItem.RankEntry rank = Economy.Ranks.GetOrAdd(perm);
            rank.Price = int.Parse(price.ToString());
            if (rank.Price == 0)
            {
                Economy.Ranks.Remove(perm);
            }
        }
        public void Eco_UpdateLevelEnables()
        {
            Eco_dgvMaps.Enabled = Eco_cbLvl.Checked;
            Eco_btnLvlAdd.Enabled = Eco_cbLvl.Checked;
            Eco_btnLvlDel.Enabled = Eco_cbLvl.Checked;
        }
        public void Eco_UpdateLevels()
        {
            Eco_dgvMaps.Rows.Clear();
            foreach (LevelItem.LevelPreset p in Economy.Levels.Presets)
            {
                Eco_dgvMaps.Rows.Add(p.name, p.price, p.x, p.y, p.z, p.type);
            }
            Eco_UpdateLevelEnables();
        }
        public void Eco_lvlEnabled_CheckedChanged(object sender, EventArgs e)
        {
            Eco_UpdateLevelEnables();
            Economy.Levels.Enabled = Eco_cbLvl.Checked;
        }
        public void Eco_dgvMaps_Apply()
        {
            List<LevelItem.LevelPreset> presets = new List<LevelItem.LevelPreset>();
            foreach (DataGridViewRow row in Eco_dgvMaps.Rows)
            {
                LevelItem.LevelPreset p = new LevelItem.LevelPreset
                {
                    name = row.Cells[0].Value.ToString(),
                    price = int.Parse(row.Cells[1].Value.ToString()),
                    x = row.Cells[2].Value.ToString(),
                    y = row.Cells[3].Value.ToString(),
                    z = row.Cells[4].Value.ToString(),
                    type = row.Cells[5].Value.ToString()
                };
                presets.Add(p);
            }
            Economy.Levels.Presets = presets;
        }
        public void Eco_dgvMaps_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Eco_dgvMaps_Apply();
            }
        }
        public void Eco_lvlAdd_Click(object sender, EventArgs e)
        {
            string name = "preset_" + (Eco_dgvMaps.RowCount + 1);
            Eco_dgvMaps.Rows.Add(name, 1000, "64", "64", "64", "flat");
            Eco_dgvMaps_Apply();
        }
        public void Eco_lvlDelete_Click(object sender, EventArgs e)
        {
            if (Eco_dgvMaps.SelectedRows.Count == 0)
            {
                Popup.Warning("No available presets to remove");
            }
            else
            {
                DataGridViewRow row = Eco_dgvMaps.SelectedRows[0];
                Eco_dgvMaps.Rows.Remove(row);
                Eco_dgvMaps_Apply();
            }
        }
    }
}