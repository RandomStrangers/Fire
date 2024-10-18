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
    public sealed class ReplaceBrush : Brush
    {
        public ushort[] include;
        public ushort target;

        public ReplaceBrush(ushort[] include, ushort target)
        {
            this.include = include; 
            this.target = target;
        }

        public override string Name { get { return "Replace"; } }

        public override void Configure(DrawOp op, Player p)
        {
            op.Flags = BlockDBFlags.Replaced;
        }

        public override ushort NextBlock(DrawOp op)
        {
            ushort x = op.Coords.X, y = op.Coords.Y, z = op.Coords.Z;
            ushort block = op.Level.GetBlock(x, y, z);

            for (int i = 0; i < include.Length; i++)
            {
                if (block == include[i]) return target;
            }
            return Block.Invalid;
        }
    }

    public sealed class ReplaceNotBrush : Brush
    {
        public ushort[] exclude;
        public ushort target;

        public ReplaceNotBrush(ushort[] exclude, ushort target)
        {
            this.exclude = exclude; 
            this.target = target;
        }

        public override string Name { get { return "ReplaceNot"; } }

        public override void Configure(DrawOp op, Player p)
        {
            op.Flags = BlockDBFlags.Replaced;
        }

        public override ushort NextBlock(DrawOp op)
        {
            ushort x = op.Coords.X, y = op.Coords.Y, z = op.Coords.Z;
            ushort block = op.Level.GetBlock(x, y, z);

            for (int i = 0; i < exclude.Length; i++)
            {
                if (block == exclude[i]) return Block.Invalid;
            }
            return target;
        }
    }
}