/*
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
using Flames.DB;
using Flames.Drawing.Ops;

namespace Flames.Drawing.Brushes
{
    public class ReplaceBrushBrush : Brush
    {
        public ushort target;
        public Brush replacement;

        public ReplaceBrushBrush(ushort include, Brush replacement)
        {
            target = include; 
            this.replacement = replacement;
        }

        public override string Name { get { return "ReplaceBrush"; } }

        public override void Configure(DrawOp op, Player p)
        {
            op.Flags = BlockDBFlags.Replaced;
            replacement.Configure(op, p);
        }

        public override ushort NextBlock(DrawOp op)
        {
            ushort x = op.Coords.X, y = op.Coords.Y, z = op.Coords.Z;
            ushort block = op.Level.GetBlock(x, y, z); // TODO FastGetBlock

            if (block != target) return Block.Invalid;
            return replacement.NextBlock(op);
        }
    }

    public class ReplaceNotBrushBrush : ReplaceBrushBrush
    {
        public ReplaceNotBrushBrush(ushort exclude, Brush replacement) : base(exclude, replacement) 
        { 
        }

        public override string Name { get { return "ReplaceNotBrush"; } }

        public override ushort NextBlock(DrawOp op)
        {
            ushort x = op.Coords.X, y = op.Coords.Y, z = op.Coords.Z;
            ushort block = op.Level.GetBlock(x, y, z); // TODO FastGetBlock

            if (block == target) return Block.Invalid;
            return replacement.NextBlock(op);
        }
    }
}