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
using Flames.Commands;
using Flames.Gui.Popups;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public ItemPermsHelper CommandItems = new ItemPermsHelper();
        public ComboBox[] CommandExtraBoxes;
        public Label[] CommandExtraLabels;
        public Command Cmd;
        // need to keep a list of changed command perms, because we don't want
        // to modify the server's live permissions if user clicks 'discard'
        public CommandPerms CommandPermsOrig, CommandPermsCopy;
        public List<CommandExtraPerms> ExtraPermsList;
        public List<CommandPerms> CommandPermsChanged = new List<CommandPerms>();
        public List<CommandExtraPerms> CommandExtraPermsChanged = new List<CommandExtraPerms>();
        public void LoadCommands()
        {
            Cmd_list.Items.Clear();
            List<Command> all = Command.CopyAll();
            all.Sort((a, b) => a.name.CompareTo(b.name));
            foreach (Command cmd in all)
            {
                Cmd_list.Items.Add(cmd.name);
            }
            CommandItems.GetCurPerms = CommandGetOrAddPermsChanged;
            if (Cmd_list.SelectedIndex == -1)
            {
                Cmd_list.SelectedIndex = 0;
            }
        }
        public void SaveCommands()
        {
            if (CommandPermsChanged.Count > 0)
            {
                SaveCommandPermissions();
            }
            if (CommandExtraPermsChanged.Count > 0) 
            { 
                SaveExtraCommandPermissions(); 
            }
            LoadCommands();
        }
        public void SaveCommandPermissions()
        {
            foreach (CommandPerms changed in CommandPermsChanged)
            {
                CommandPerms orig = CommandPerms.Find(changed.CmdName);
                changed.CopyPermissionsTo(orig);
            }
            CommandPerms.Save();
            CommandPerms.ApplyChanges();
        }
        public void SaveExtraCommandPermissions()
        {
            foreach (CommandExtraPerms changed in CommandExtraPermsChanged)
            {
                CommandExtraPerms orig = CommandExtraPerms.Find(changed.CmdName, changed.Num);
                changed.CopyPermissionsTo(orig);
            }
            CommandExtraPerms.Save();
        }
        public void Cmd_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmdName = Cmd_list.SelectedItem.ToString();
            CommandInitSpecificArrays();
            Cmd = Command.Find(cmdName);
            if (Cmd == null)
            {
                return;
            }
            CommandPermsOrig = CommandPerms.Find(cmdName);
            CommandPermsCopy = CommandPermsChanged.Find(p => p.CmdName.CaselessEq(cmdName));
            CommandItems.SupressEvents = true;
            CommandInitExtraPerms();
            CommandPerms perms = CommandPermsCopy ?? CommandPermsOrig;
            CommandItems.Update(perms);
        }
        public void CommandInitSpecificArrays()
        {
            if (CommandItems.MinBox != null)
            {
                return;
            }
            CommandItems.MinBox = Cmd_cmbMin;
            CommandItems.AllowBoxes = new ComboBox[] 
            { 
                Cmd_cmbAlw1, Cmd_cmbAlw2, Cmd_cmbAlw3 
            };
            CommandItems.DisallowBoxes = new ComboBox[] 
            {
                Cmd_cmbDis1, Cmd_cmbDis2, Cmd_cmbDis3 
            };
            CommandItems.FillInitial();
            CommandExtraBoxes = new ComboBox[] 
            { 
                Cmd_cmbExtra1, Cmd_cmbExtra2, Cmd_cmbExtra3,
                Cmd_cmbExtra4, Cmd_cmbExtra5, Cmd_cmbExtra6, Cmd_cmbExtra7 
            };
            CommandExtraLabels = new Label[] 
            { 
                Cmd_lblExtra1, Cmd_lblExtra2, Cmd_lblExtra3,
                Cmd_lblExtra4, Cmd_lblExtra5, Cmd_lblExtra6, Cmd_lblExtra7 
            };
            GuiPerms.SetRanks(CommandExtraBoxes);
        }
        public ItemPerms CommandGetOrAddPermsChanged()
        {
            if (CommandPermsCopy != null)
            {
                return CommandPermsCopy;
            }
            CommandPermsCopy = CommandPermsOrig.Copy();
            CommandPermsChanged.Add(CommandPermsCopy);
            return CommandPermsCopy;
        }
        public void Cmd_cmbMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommandItems.OnMinRankChanged((ComboBox)sender);
        }
        public void Cmd_cmbSpecific_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommandItems.OnSpecificChanged((ComboBox)sender);
        }
        public void Cmd_btnHelp_Click(object sender, EventArgs e)
        {
            GetHelp(Cmd_list.SelectedItem.ToString());
        }
        public void Cmd_btnCustom_Click(object sender, EventArgs e)
        {
            using CustomCommands form = new CustomCommands();
            form.ShowDialog();
        }
        public void CommandInitExtraPerms()
        {
            ExtraPermsList = CommandExtraPerms.FindAll(Cmd.name);
            for (int i = 0; i < CommandExtraBoxes.Length; i++)
            {
                CommandExtraBoxes[i].Visible = false;
                CommandExtraLabels[i].Visible = false;
            }
            if (Cmd.ExtraPerms == null)
            {
                ExtraPermsList.Clear();
            }
            int height = 12;
            for (int i = 0; i < ExtraPermsList.Count; i++)
            {
                CommandExtraPerms perms = LookupExtraPerms(ExtraPermsList[i].CmdName, ExtraPermsList[i].Num) ?? ExtraPermsList[i];
                GuiPerms.SetSelectedRank(CommandExtraBoxes[i], perms.MinRank);
                CommandExtraBoxes[i].Visible = true;
                CommandExtraLabels[i].Text = "+ " + perms.Desc;
                CommandExtraLabels[i].Visible = true;
                height = CommandExtraBoxes[i].Bottom + 12;
            }
            Cmd_grpExtra.Visible = ExtraPermsList.Count > 0;
            Cmd_grpExtra.Height = height;
        }
        public CommandExtraPerms LookupExtraPerms(string cmdName, int number)
        {
            return CommandExtraPermsChanged.Find(
                p => p.CmdName == cmdName && p.Num == number);
        }
        public void Cmd_cmbExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (CommandItems.SupressEvents)
            {
                return;
            }
            GuiRank rank = (GuiRank)box.SelectedItem;
            if (rank == null)
            {
                return;
            }
            int boxIdx = Array.IndexOf(CommandExtraBoxes, box);
            CommandExtraPerms orig = ExtraPermsList[boxIdx];
            CommandExtraPerms copy = LookupExtraPerms(orig.CmdName, orig.Num);
            if (copy == null)
            {
                copy = orig.Copy();
                CommandExtraPermsChanged.Add(copy);
            }
            copy.MinRank = rank.Permission;
        }
    }
}