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
using Flames.Drawing.Ops;

namespace Flames.Commands.Building
{
    public class CmdReplace : DrawCmd
    {
        public override string name { get { return "Replace"; } }
        public override string shortcut { get { return "r"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }

        public override DrawOp GetDrawOp(DrawArgs dArgs)
        {
            DrawOp op = new CuboidDrawOp
            {
                AffectedByTransform = false
            };
            return op;
        }

        public override void GetBrush(DrawArgs dArgs)
        {
            dArgs.BrushName = "Replace";
            dArgs.BrushArgs = dArgs.Message;
        }

        public override void Help(Player p)
        {
            p.Message("&T/Replace [block] [block2].. [new]");
            p.Message("&HReplaces [block] with [new] between two points.");
            p.Message("&H  If more than one [block] is given, they are all replaced.");
            p.Message("&H  If only [block] is given, replaces with your held block.");
        }
    }

    public sealed class CmdReplaceNot : CmdReplace
    {
        public override string name { get { return "ReplaceNot"; } }
        public override string shortcut { get { return "rn"; } }

        public override void GetBrush(DrawArgs dArgs)
        {
            dArgs.BrushName = "ReplaceNot";
            dArgs.BrushArgs = dArgs.Message;
        }

        public override void Help(Player p)
        {
            p.Message("&T/ReplaceNot [block] [block2].. [new]");
            p.Message("&HReplaces everything but [block] with [new] between two points.");
            p.Message("&H  If more than one [block] is given, they are all skipped.");
            p.Message("&H  If only [block] is given, replaces with your held block.");
        }
    }
}