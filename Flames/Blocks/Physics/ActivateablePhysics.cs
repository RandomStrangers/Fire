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

namespace Flames.Blocks.Physics
{
    public static class ActivateablePhysics
    {

        /// <summary> Activates fireworks, rockets, and TNT in 1 block radius around (x, y, z) </summary>
        public static void DoNeighbours(Level lvl, ushort x, ushort y, ushort z)
        {
            int bHead = 0;
            for (int dy = -1; dy <= 1; dy++)
                for (int dz = -1; dz <= 1; dz++)
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        ushort block = lvl.GetBlock((ushort)(x + dx), (ushort)(y + dy), (ushort)(z + dz));
                        if (block == Block.RocketStart)
                        {
                            bool isFree =
                                lvl.GetBlock((ushort)(x + dx * 2), (ushort)(y + dy * 2), (ushort)(z + dz * 2), out int bTail) == Block.Air &&
                                lvl.GetBlock((ushort)(x + dx * 3), (ushort)(y + dy * 3), (ushort)(z + dz * 3), out bHead) == Block.Air &&
                                !lvl.listUpdateExists.Get(x + dx * 3, y + dy * 3, z + dz * 3) &&
                                !lvl.listUpdateExists.Get(x + dx * 2, y + dy * 2, z + dz * 2);

                            if (isFree)
                            {
                                lvl.AddUpdate(bHead, Block.RocketHead, default(PhysicsArgs));
                                lvl.AddUpdate(0, Block.LavaFire, default(PhysicsArgs));
                            }
                        }
                        else if (block == Block.Fireworks)
                        {
                            bool isFree =
                                lvl.GetBlock((ushort)(x + dx), (ushort)(y + dy + 1), (ushort)(z + dz), out int bTail) == Block.Air &&
                                lvl.GetBlock((ushort)(x + dx), (ushort)(y + dy + 2), (ushort)(z + dz), out bHead) == Block.Air &&
                                !lvl.listUpdateExists.Get(x + dx, y + dy + 1, z + dz) &&
                                !lvl.listUpdateExists.Get(x + dx, y + dy + 2, z + dz);

                            if (isFree)
                            {
                                lvl.AddUpdate(bHead, Block.Fireworks, default(PhysicsArgs));
                                PhysicsArgs args = default;
                                args.Type1 = PhysicsArgs.Dissipate; 
                                args.Value1 = 100;
                                lvl.AddUpdate(0, Block.StillLava, args);
                            }
                        }
                        else if (block == Block.TNT)
                        {
                            lvl.MakeExplosion((ushort)(x + dx), (ushort)(y + dy), (ushort)(z + dz), 0);
                        }
                    }
        }

        /// <summary> Activates doors, tdoors and toggles odoors at (x, y, z) </summary>
        public static void DoDoors(Level lvl, ushort x, ushort y, ushort z, bool instant)
        {
            ushort block = lvl.GetBlock(x, y, z, out int index);
            if (index == -1) return;

            if (lvl.Props[block].IsDoor)
            {
                PhysicsArgs args = GetDoorArgs(block, out ushort physForm);
                if (!instant) lvl.AddUpdate(index, physForm, args);
                else lvl.Blockchange(index, physForm, false, args);
            }
            else if (lvl.Props[block].IsTDoor)
            {
                PhysicsArgs args = GetTDoorArgs(block);
                lvl.AddUpdate(index, Block.Air, args);
            }
            else
            {
                ushort oDoor = lvl.Props[block].oDoorBlock;
                if (oDoor == Block.Invalid) return;
                lvl.AddUpdate(index, oDoor, true);
            }
        }


        public static PhysicsArgs GetDoorArgs(ushort block, out ushort physForm)
        {
            PhysicsArgs args = default;
            args.Type1 = PhysicsArgs.Custom; 
            args.Value1 = 16 - 1;
            args.Type2 = PhysicsArgs.Revert; 
            args.Value2 = (byte)block;
            args.ExtBlock = (byte)(block >> Block.ExtendedShift);

            physForm = Block.Door_Log_air; // air
            if (block == Block.Door_Air || block == Block.Door_AirActivatable)
            {
                args.Value1 = 4 - 1;
            }
            else if (block == Block.Door_Green)
            {
                physForm = Block.Door_Green_air; // red wool
            }
            else if (block == Block.Door_TNT)
            {
                args.Value1 = 4 - 1; 
                physForm = Block.Door_TNT_air; // lava
            }
            return args;
        }

        public static PhysicsArgs GetTDoorArgs(ushort block)
        {
            PhysicsArgs args = default;
            args.Type1 = PhysicsArgs.Custom; 
            args.Value1 = 16;
            args.Type2 = PhysicsArgs.Revert; 
            args.Value2 = (byte)block;
            args.ExtBlock = (byte)(block >> Block.ExtendedShift);
            return args;
        }


        public static void CheckNeighbours(Level lvl, ushort x, ushort y, ushort z)
        {
            CheckAt(lvl, (ushort)(x + 1), y, z);
            CheckAt(lvl, (ushort)(x - 1), y, z);
            CheckAt(lvl, x, y, (ushort)(z + 1));
            CheckAt(lvl, x, y, (ushort)(z - 1));
            CheckAt(lvl, x, (ushort)(y + 1), z);
            // NOTE: omission of y-1 to match original behaviour
        }

        // TODO: Stop checking block type and just always call lvl.AddCheck
        public static void CheckAt(Level lvl, ushort x, ushort y, ushort z)
        {
            ushort block = lvl.GetBlock(x, y, z, out int index);

            switch (block)
            {
                //case Block.water:
                //case Block.lava:
                case Block.Sapling:
                case Block.Sand:
                case Block.Gravel:
                case Block.Log:
                case Block.Leaves:
                case Block.FloatWood:
                    /*case Block.lava_fast:
                    case Block.WaterDown:
                    case Block.LavaDown:
                    case Block.deathlava:
                    case Block.deathwater:
                    case Block.geyser:
                    case Block.magma:*/
                    lvl.AddCheck(index);
                    break;
                default:
                    block = Block.Convert(block);
                    if (block == Block.Water || block == Block.Lava || (block >= Block.Red && block <= Block.White))
                    {
                        lvl.AddCheck(index);
                    }
                    break;
            }
        }
    }
}