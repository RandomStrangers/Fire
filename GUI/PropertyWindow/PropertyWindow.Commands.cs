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

        public ItemPermsHelper commandItems = new ItemPermsHelper();
        public ComboBox[] commandExtraBoxes;
        public Label[] commandExtraLabels;
        public Command cmd;

        // need to keep a list of changed command perms, because we don't want
        // to modify the server's live permissions if user clicks 'discard'
        public CommandPerms commandPermsOrig, commandPermsCopy;
        public List<CommandExtraPerms> extraPermsList;
        public List<CommandPerms> commandPermsChanged = new List<CommandPerms>();
        public List<CommandExtraPerms> commandExtraPermsChanged = new List<CommandExtraPerms>();

        public void LoadCommands()
        {
            cmd_list.Items.Clear();
            List<Command> all = Command.CopyAll();
            all.Sort((a, b) => a.name.CompareTo(b.name));

            foreach (Command cmd in all)
            {
                cmd_list.Items.Add(cmd.name);
            }

            commandItems.GetCurPerms = CommandGetOrAddPermsChanged;
            if (cmd_list.SelectedIndex == -1)
                cmd_list.SelectedIndex = 0;
        }

        public void SaveCommands()
        {
            if (commandPermsChanged.Count > 0)
                SaveCommandPermissions();
            if (commandExtraPermsChanged.Count > 0)
                SaveExtraCommandPermissions();

            LoadCommands();
        }

        public void SaveCommandPermissions()
        {
            foreach (CommandPerms changed in commandPermsChanged)
            {
                CommandPerms orig = CommandPerms.Find(changed.CmdName);
                changed.CopyPermissionsTo(orig);
            }

            CommandPerms.Save();
            CommandPerms.ApplyChanges();
        }

        public void SaveExtraCommandPermissions()
        {
            foreach (CommandExtraPerms changed in commandExtraPermsChanged)
            {
                CommandExtraPerms orig = CommandExtraPerms.Find(changed.CmdName, changed.Num);
                changed.CopyPermissionsTo(orig);
            }

            CommandExtraPerms.Save();
        }


        public void cmd_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmdName = cmd_list.SelectedItem.ToString();
            CommandInitSpecificArrays();
            cmd = Command.Find(cmdName);
            if (cmd == null) return;

            commandPermsOrig = CommandPerms.Find(cmdName);
            commandPermsCopy = commandPermsChanged.Find(p => p.CmdName.CaselessEq(cmdName));

            commandItems.SupressEvents = true;
            CommandInitExtraPerms();
            CommandPerms perms = commandPermsCopy != null ? commandPermsCopy : commandPermsOrig;
            commandItems.Update(perms);
        }

        public void CommandInitSpecificArrays()
        {
            if (commandItems.MinBox != null) return;
            commandItems.MinBox = cmd_cmbMin;
            commandItems.AllowBoxes = new ComboBox[] 
            { 
                cmd_cmbAlw1, cmd_cmbAlw2, cmd_cmbAlw3 
            };
            commandItems.DisallowBoxes = new ComboBox[] 
            {
                cmd_cmbDis1, cmd_cmbDis2, cmd_cmbDis3 
            };
            commandItems.FillInitial();

            commandExtraBoxes = new ComboBox[] 
            { 
                cmd_cmbExtra1, cmd_cmbExtra2, cmd_cmbExtra3,
                cmd_cmbExtra4, cmd_cmbExtra5, cmd_cmbExtra6, cmd_cmbExtra7 
            };
            commandExtraLabels = new Label[] 
            { 
                cmd_lblExtra1, cmd_lblExtra2, cmd_lblExtra3,
                cmd_lblExtra4, cmd_lblExtra5, cmd_lblExtra6, cmd_lblExtra7 
            };
            GuiPerms.SetRanks(commandExtraBoxes);
        }

        public ItemPerms CommandGetOrAddPermsChanged()
        {
            if (commandPermsCopy != null) return commandPermsCopy;
            commandPermsCopy = commandPermsOrig.Copy();
            commandPermsChanged.Add(commandPermsCopy);
            return commandPermsCopy;
        }


        public void cmd_cmbMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            commandItems.OnMinRankChanged((ComboBox)sender);
        }

        public void cmd_cmbSpecific_SelectedIndexChanged(object sender, EventArgs e)
        {
            commandItems.OnSpecificChanged((ComboBox)sender);
        }

        public void cmd_btnHelp_Click(object sender, EventArgs e)
        {
            GetHelp(cmd_list.SelectedItem.ToString());
        }

        public void cmd_btnCustom_Click(object sender, EventArgs e)
        {
            using (CustomCommands form = new CustomCommands())
            {
                form.ShowDialog();
            }
        }


        public void CommandInitExtraPerms()
        {
            extraPermsList = CommandExtraPerms.FindAll(cmd.name);
            for (int i = 0; i < commandExtraBoxes.Length; i++)
            {
                commandExtraBoxes[i].Visible = false;
                commandExtraLabels[i].Visible = false;
            }
            if (cmd.ExtraPerms == null) extraPermsList.Clear();

            int height = 12;
            for (int i = 0; i < extraPermsList.Count; i++)
            {
                CommandExtraPerms perms = LookupExtraPerms(extraPermsList[i].CmdName, extraPermsList[i].Num);
                if (perms == null) perms = extraPermsList[i];

                GuiPerms.SetSelectedRank(commandExtraBoxes[i], perms.MinRank);
                commandExtraBoxes[i].Visible = true;
                commandExtraLabels[i].Text = "+ " + perms.Desc;
                commandExtraLabels[i].Visible = true;
                height = commandExtraBoxes[i].Bottom + 12;
            }
            cmd_grpExtra.Visible = extraPermsList.Count > 0;
            cmd_grpExtra.Height = height;
        }

        public CommandExtraPerms LookupExtraPerms(string cmdName, int number)
        {
            return commandExtraPermsChanged.Find(
                p => p.CmdName == cmdName && p.Num == number);
        }

        public void cmd_cmbExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (commandItems.SupressEvents) return;
            GuiRank rank = (GuiRank)box.SelectedItem;
            if (rank == null) return;

            int boxIdx = Array.IndexOf(commandExtraBoxes, box);
            CommandExtraPerms orig = extraPermsList[boxIdx];
            CommandExtraPerms copy = LookupExtraPerms(orig.CmdName, orig.Num);

            if (copy == null)
            {
                copy = orig.Copy();
                commandExtraPermsChanged.Add(copy);
            }
            copy.MinRank = rank.Permission;
        }
    }
}