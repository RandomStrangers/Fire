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
using Flames.Added;
using Flames.Gui.Popups;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public ItemPermsHelper OrderItems = new ItemPermsHelper();
        public ComboBox[] OrderExtraBoxes;
        public Label[] OrderExtraLabels;
        public Order Ord;
        // need to keep a list of changed order perms, because we don't want
        // to modify the server's live permissions if user clicks 'discard'
        public OrderPerms OrderPermsOrig, OrderPermsCopy;
        public List<OrderExtraPerms> ExtraPermsList;
        public List<OrderPerms> OrderPermsChanged = new List<OrderPerms>();
        public List<OrderExtraPerms> OrderExtraPermsChanged = new List<OrderExtraPerms>();
        public void LoadOrders()
        {
            Ord_list.Items.Clear();
            List<Order> all = Order.CopyAll();
            all.Sort((a, b) => a.Name.CompareTo(b.Name));
            foreach (Order ord in all)
            {
                Ord_list.Items.Add(ord.Name);
            }
            OrderItems.GetCurPerms = OrderGetOrAddPermsChanged;
            if (Ord_list.SelectedIndex == -1)
            {
                Ord_list.SelectedIndex = 0;
            }
        }
        public void SaveOrders()
        {
            if (OrderPermsChanged.Count > 0)
            {
                SaveOrderPermissions();
            }
            if (OrderExtraPermsChanged.Count > 0) 
            { 
                SaveExtraOrderPermissions(); 
            }
            LoadOrders();
        }
        public void SaveOrderPermissions()
        {
            foreach (OrderPerms changed in OrderPermsChanged)
            {
                OrderPerms orig = OrderPerms.Find(changed.OrdName);
                changed.CopyPermissionsTo(orig);
            }
            OrderPerms.Save();
            OrderPerms.ApplyChanges();
        }
        public void SaveExtraOrderPermissions()
        {
            foreach (OrderExtraPerms changed in OrderExtraPermsChanged)
            {
                OrderExtraPerms orig = OrderExtraPerms.Find(changed.OrdName, changed.Num);
                changed.CopyPermissionsTo(orig);
            }
            OrderExtraPerms.Save();
        }
        public void Ord_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ordName = Ord_list.SelectedItem.ToString();
            OrderInitSpecificArrays();
            Ord = Order.Find(ordName);
            if (Ord == null)
            {
                return;
            }
            OrderPermsOrig = OrderPerms.Find(ordName);
            OrderPermsCopy = OrderPermsChanged.Find(p => p.OrdName.CaselessEq(ordName));
            OrderItems.SupressEvents = true;
            OrderInitExtraPerms();
            OrderPerms perms = OrderPermsCopy ?? OrderPermsOrig;
            OrderItems.Update(perms);
        }
        public void OrderInitSpecificArrays()
        {
            if (OrderItems.MinBox != null)
            {
                return;
            }
            OrderItems.MinBox = Ord_cmbMin;
            OrderItems.AllowBoxes = new ComboBox[] 
            { 
                Ord_cmbAlw1, Ord_cmbAlw2, Ord_cmbAlw3 
            };
            OrderItems.DisallowBoxes = new ComboBox[] 
            {
                Ord_cmbDis1, Ord_cmbDis2, Ord_cmbDis3 
            };
            OrderItems.FillInitial();
            OrderExtraBoxes = new ComboBox[] 
            { 
                Ord_cmbExtra1, Ord_cmbExtra2, Ord_cmbExtra3,
                Ord_cmbExtra4, Ord_cmbExtra5, Ord_cmbExtra6, Ord_cmbExtra7 
            };
            OrderExtraLabels = new Label[] 
            { 
                Ord_lblExtra1, Ord_lblExtra2, Ord_lblExtra3,
                Ord_lblExtra4, Ord_lblExtra5, Ord_lblExtra6, Ord_lblExtra7 
            };
            GuiPerms.SetRanks(OrderExtraBoxes);
        }
        public ItemPerms OrderGetOrAddPermsChanged()
        {
            if (OrderPermsCopy != null)
            {
                return OrderPermsCopy;
            }
            OrderPermsCopy = OrderPermsOrig.Copy();
            OrderPermsChanged.Add(OrderPermsCopy);
            return OrderPermsCopy;
        }
        public void Ord_cmbMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            OrderItems.OnMinRankChanged((ComboBox)sender);
        }
        public void Ord_cmbSpecific_SelectedIndexChanged(object sender, EventArgs e)
        {
            OrderItems.OnSpecificChanged((ComboBox)sender);
        }
        public void Ord_btnHelp_Click(object sender, EventArgs e)
        {
            GetHelp(Ord_list.SelectedItem.ToString());
        }
        public void Ord_btnCustom_Click(object sender, EventArgs e)
        {
            using CustomOrders form = new CustomOrders();
            form.ShowDialog();
        }
        public void OrderInitExtraPerms()
        {
            ExtraPermsList = OrderExtraPerms.FindAll(Ord.Name);
            for (int i = 0; i < OrderExtraBoxes.Length; i++)
            {
                OrderExtraBoxes[i].Visible = false;
                OrderExtraLabels[i].Visible = false;
            }
            if (Ord.OrdExtraPerms == null)
            {
                ExtraPermsList.Clear();
            }
            int height = 12;
            for (int i = 0; i < ExtraPermsList.Count; i++)
            {
                OrderExtraPerms perms = LookupExtraPerms(ExtraPermsList[i].OrdName, ExtraPermsList[i].Num) ?? ExtraPermsList[i];
                GuiPerms.SetSelectedRank(OrderExtraBoxes[i], perms.MinRank);
                OrderExtraBoxes[i].Visible = true;
                OrderExtraLabels[i].Text = "+ " + perms.Desc;
                OrderExtraLabels[i].Visible = true;
                height = OrderExtraBoxes[i].Bottom + 12;
            }
            Ord_grpExtra.Visible = ExtraPermsList.Count > 0;
            Ord_grpExtra.Height = height;
        }
        public OrderExtraPerms LookupExtraPerms(string ordName, int number)
        {
            return OrderExtraPermsChanged.Find(
                p => p.OrdName == ordName && p.Num == number);
        }
        public void Ord_cmbExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (OrderItems.SupressEvents)
            {
                return;
            }
            GuiRank rank = (GuiRank)box.SelectedItem;
            if (rank == null)
            {
                return;
            }
            int boxIdx = Array.IndexOf(OrderExtraBoxes, box);
            OrderExtraPerms orig = ExtraPermsList[boxIdx];
            OrderExtraPerms copy = LookupExtraPerms(orig.OrdName, orig.Num);
            if (copy == null)
            {
                copy = orig.Copy();
                OrderExtraPermsChanged.Add(copy);
            }
            copy.MinRank = rank.Permission;
        }
    }
}