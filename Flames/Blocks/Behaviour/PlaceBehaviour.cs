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
using Flames.Blocks.Physics;

namespace Flames.Blocks
{

    public static class PlaceBehaviour
    {

        public static bool SkipGrassDirt(Player p, ushort block)
        {
            Level lvl = p.level;
            return !lvl.Config.GrassGrow || p.ModeBlock == block || !(lvl.physics == 0 || lvl.physics == 5);
        }

        public static ChangeResult GrassDie(Player p, ushort block, ushort x, ushort y, ushort z)
        {
            if (SkipGrassDirt(p, block)) return p.ChangeBlock(x, y, z, block);
            Level lvl = p.level;
            ushort above = lvl.GetBlock(x, (ushort)(y + 1), z);

            if (above != Block.Invalid && !lvl.LightPasses(above))
            {
                block = p.level.Props[block].DirtBlock;
            }
            return p.ChangeBlock(x, y, z, block);
        }

        public static ChangeResult DirtGrow(Player p, ushort block, ushort x, ushort y, ushort z)
        {
            if (SkipGrassDirt(p, block)) return p.ChangeBlock(x, y, z, block);
            Level lvl = p.level;
            ushort above = lvl.GetBlock(x, (ushort)(y + 1), z);

            if (above == Block.Invalid || lvl.LightPasses(above))
            {
                block = p.level.Props[block].GrassBlock;
            }
            return p.ChangeBlock(x, y, z, block);
        }

        public static ChangeResult Stack(Player p, ushort block, ushort x, ushort y, ushort z)
        {
            if (p.level.GetBlock(x, (ushort)(y - 1), z) != block)
            {
                return p.ChangeBlock(x, y, z, block);
            }

            p.SendBlockchange(x, y, z, Block.Air); // send the air block back only to the user
            ushort stack = p.level.Props[block].StackBlock;
            p.ChangeBlock(x, (ushort)(y - 1), z, stack);
            return ChangeResult.Modified;
        }

        public static ChangeResult C4(Player p, ushort block, ushort x, ushort y, ushort z)
        {
            if (p.level.physics == 0 || p.level.physics == 5) return ChangeResult.Unchanged;

            // Use red wool to detonate c4
            ushort held = p.BlockBindings[p.ClientHeldBlock];
            if (held == Block.Red)
            {
                p.Message("Placed detonator block, delete it to detonate.");
                return C4Det(p, Block.C4Detonator, x, y, z);
            }

            if (p.c4circuitNumber == -1)
            {
                sbyte num = C4Physics.NextCircuit(p.level);
                p.level.C4list.Add(new C4Data(num));
                p.c4circuitNumber = num;

                string detonatorName = Block.GetName(p, Block.Red);
                p.Message("Place more blocks for more c4, then place a &c{0} &Sblock for the detonator.",
                               detonatorName);
            }

            C4Data c4 = C4Physics.Find(p.level, p.c4circuitNumber);
            c4?.list.Add(p.level.PosToInt(x, y, z));
            return p.ChangeBlock(x, y, z, Block.C4);
        }

        public static ChangeResult C4Det(Player p, ushort block, ushort x, ushort y, ushort z)
        {
            if (p.level.physics == 0 || p.level.physics == 5)
            {
                p.c4circuitNumber = -1;
                return ChangeResult.Unchanged;
            }

            C4Data c4 = C4Physics.Find(p.level, p.c4circuitNumber);
            if (c4 != null) c4.detIndex = p.level.PosToInt(x, y, z);
            p.c4circuitNumber = -1;
            return p.ChangeBlock(x, y, z, Block.C4Detonator);
        }
    }
}