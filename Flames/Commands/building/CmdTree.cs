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
using Flames.Drawing.Ops;
using Flames.Generator.Foliage;

namespace Flames.Commands.Building
{
    public sealed class CmdTree : DrawCmd
    {
        public override string name { get { return "Tree"; } }
        public override string type { get { return CommandTypes.Building; } }

        public override int MarksCount { get { return 1; } }
        public override string SelectionType { get { return "location"; } }
        public override string PlaceMessage { get { return "Select where you wish your tree to grow"; } }

        public override DrawOp GetDrawOp(DrawArgs dArgs)
        {
            string[] args = dArgs.Message.SplitSpaces(3);
            Tree tree = Tree.Find(args[0]);
            if (tree == null) tree = new NormalTree();

            int size;
            if (args.Length > 1 && int.TryParse(args[1], out size))
            {
                Player p = dArgs.Player;
                string opt = args[0] + " tree size";
                if (!CommandParser.GetInt(p, args[1], opt, ref size, tree.MinSize, tree.MaxSize)) return null;
            }
            else
            {
                size = -1;
            }

            TreeDrawOp op = new TreeDrawOp
            {
                Tree = tree,
                Size = size
            };
            return op;
        }

        public override void GetBrush(DrawArgs dArgs)
        {
            TreeDrawOp op = (TreeDrawOp)dArgs.Op;
            if (op.Size != -1)
            {
                dArgs.BrushArgs = dArgs.Message.Splice(2, 0); // type, value/height, brush args
            }
            else
            {
                dArgs.BrushArgs = dArgs.Message.Splice(1, 0); // type, brush args
            }

            // use leaf blocks by default
            if (dArgs.BrushName.CaselessEq("Normal") && dArgs.BrushArgs.Length == 0)
            {
                dArgs.BrushArgs = Block.Leaves.ToString();
            }
        }

        public override void Help(Player p)
        {
            p.Message("&T/Tree [type] <brush args> &H- Draws a tree.");
            p.Message("&T/Tree [type] [size/height] <brush args>");
            p.Message("&H  Types: &f{0}", Tree.TreeTypes.Join(t => t.Key));
            p.Message(BrushHelpLine);
        }
    }
}