﻿/*
    Copyright 2015 MCGalaxy
        
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
    public class CmdReplaceBrush : DrawCmd
    {
        public override string name { get { return "ReplaceBrush"; } }
        public override string shortcut { get { return "rb"; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public virtual bool ReplaceNot { get { return false; } }

        public override DrawOp GetDrawOp(DrawArgs dArgs)
        {
            Player p = dArgs.Player;

            string replaceCmd = ReplaceNot ? "ReplaceNot" : "Replace";
            if (!p.CanUse(replaceCmd) || !p.CanUse("Brush"))
            {
                p.Message("You cannot use &T/Brush &Sand/or &T/" + replaceCmd +
                          "&S, so therefore cannot use this command."); 
                return null;
            }

            DrawOp op = new CuboidDrawOp
            {
                AffectedByTransform = false
            };
            return op;
        }

        public override void GetBrush(DrawArgs dArgs)
        {
            dArgs.BrushName = ReplaceNot ? "ReplaceNotBrush" : "ReplaceBrush";
            dArgs.BrushArgs = dArgs.Message;
        }

        public override void Help(Player p)
        {
            p.Message("&T/ReplaceBrush [block] [brush name] <brush args>");
            p.Message("&HReplaces all blocks of the given type, " +
                      "in the specified area with the output of the given brush.");
            p.Message(BrushHelpLine);
        }
    }

    public class CmdReplaceNotBrush : CmdReplaceBrush
    {
        public override string name { get { return "ReplaceNotBrush"; } }
        public override string shortcut { get { return "rnb"; } }
        public override bool ReplaceNot { get { return true; } }

        public override void Help(Player p)
        {
            p.Message("&T/ReplaceNotBrush [block] [brush name] <brush args>");
            p.Message("&HReplaces all blocks (except for the given block), " +
                      "in the specified area with the output of the given brush.");
            p.Message(BrushHelpLine);
        }
    }
}