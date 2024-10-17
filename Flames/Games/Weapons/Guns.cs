﻿/*
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
using System;
using Flames.Maths;
using Flames.Network;
using Flames.Tasks;
using BlockID = System.UInt16;

namespace Flames.Games
{
    /// <summary> Represents a gun weapon that fires in a straight line from where player is looking. </remarks>
    public class Gun : Weapon
    {
        public override string Name { get { return "Gun"; } }

        public override void OnActivated(Vec3F32 dir, BlockID block)
        {
            AmmunitionData args = MakeArgs(dir, block);
            SchedulerTask task = new SchedulerTask(GunCallback, args, TimeSpan.Zero, true);
            p.CriticalTasks.Add(task);
        }

        public AmmunitionData MakeArgs(Vec3F32 dir, BlockID block)
        {
            AmmunitionData args = new AmmunitionData
            {
                block = block,

                start = (Vec3U16)p.Pos.BlockCoords,
                dir = dir,
                iterations = 4
            };
            return args;
        }

        public void BufferedRevert(Vec3U16 pos, BufferedBlockSender buffer)
        {
            int index;
            BlockID block = p.level.GetBlock(pos.X, pos.Y, pos.Z, out index);

            if (index == -1) return;
            buffer.Add(index, block);
        }


        public void GunCallback(SchedulerTask task)
        {
            BufferedBlockSender buffer = p.weaponBuffer;
            buffer.level = p.level;
            AmmunitionData args = (AmmunitionData)task.State;

            if (args.moving)
            {
                args.moving = TickGun(args, buffer);
            }
            else
            {
                task.Repeating = TickRevert(task, buffer);
            }

            buffer.Flush(); // TODO bufferedblocksender across guns
        }

        public bool TickGun(AmmunitionData args, BufferedBlockSender buffer)
        {
            while (true)
            {
                Vec3U16 pos = args.PosAt(args.iterations);
                args.iterations++;

                BlockID cur = p.level.GetBlock(pos.X, pos.Y, pos.Z);
                if (cur == Block.Invalid) return false;
                if (cur != Block.Air && !args.all.Contains(pos) && OnHitBlock(args, pos, cur))
                    return false;

                buffer.Add(p.level.PosToInt(pos.X, pos.Y, pos.Z), args.block);
                args.visible.Add(pos);
                args.all.Add(pos);

                Player pl = PlayerAt(p, pos, true);
                if (pl != null) 
                { 
                    OnHitPlayer(args, pl); 
                    return false; 
                }
                if (TickMove(args, buffer)) return true;
            }
        }

        public virtual bool TickMove(AmmunitionData args, BufferedBlockSender buffer)
        {
            if (args.iterations > 12)
            {
                Vec3U16 pos = args.visible[0];
                args.visible.RemoveAt(0);
                BufferedRevert(pos, buffer);
            }
            return true;
        }

        public virtual bool TickRevert(SchedulerTask task, BufferedBlockSender buffer)
        {
            AmmunitionData args = (AmmunitionData)task.State;

            if (args.visible.Count > 0)
            {
                Vec3U16 pos = args.visible[0];
                args.visible.RemoveAt(0);
                BufferedRevert(pos, buffer);
            }
            return args.visible.Count > 0;
        }


        /// <summary> Called when a bullet has collided with a block. </summary>
        /// <returns> true if this block stops the bullet, false if it should continue moving. </returns>
        public virtual bool OnHitBlock(AmmunitionData args, Vec3U16 pos, BlockID block)
        {
            return true;
        }

        /// <summary> Called when a bullet has collided with a player. </summary>
        public virtual void OnHitPlayer(AmmunitionData args, Player pl)
        {
            pl.HandleDeath(Block.Cobblestone, "@p &Swas shot by " + p.ColoredName);
        }
    }

    public class PenetrativeGun : Gun
    {
        public override string Name { get { return "Penetrative gun"; } }

        public override bool OnHitBlock(AmmunitionData args, Vec3U16 pos, BlockID block)
        {
            if (p.level.physics < 2) return true;

            if (!p.level.Props[block].LavaKills) return true;
            // Penetrative gun goes through blocks lava can go through
            p.level.Blockchange(pos.X, pos.Y, pos.Z, Block.Air);
            return false;
        }
    }

    public class ExplosiveGun : Gun
    {
        public override string Name { get { return "Explosive gun"; } }

        public override bool OnHitBlock(AmmunitionData args, Vec3U16 pos, BlockID block)
        {
            if (p.level.physics >= 3) p.level.MakeExplosion(pos.X, pos.Y, pos.Z, 1);
            return true;
        }

        public override void OnHitPlayer(AmmunitionData args, Player pl)
        {
            if (pl.level.physics >= 3)
            {
                pl.HandleDeath(Block.Cobblestone, "@p &Swas blown up by " + p.ColoredName, true);
            }
            else
            {
                base.OnHitPlayer(args, pl);
            }
        }
    }

    public class LaserGun : ExplosiveGun
    {
        public override string Name { get { return "Laser"; } }

        public override bool TickMove(AmmunitionData args, BufferedBlockSender buffer)
        {
            // laser immediately strikes target
            return false;
        }

        public override bool TickRevert(SchedulerTask task, BufferedBlockSender buffer)
        {
            AmmunitionData args = (AmmunitionData)task.State;

            if (args.all.Count > 0)
            {
                // laser persists for a short while
                task.Delay = TimeSpan.FromMilliseconds(400);
                args.all.Clear();
            }
            else
            {
                foreach (Vec3U16 pos in args.visible)
                {
                    BufferedRevert(pos, buffer);
                }
                args.visible.Clear();
            }
            return args.visible.Count > 0;
        }
    }

    public class TeleportGun : Gun
    {
        public override string Name { get { return "Teleporter gun"; } }

        public override void OnHitPlayer(AmmunitionData args, Player pl)
        {
            args.DoTeleport(p);
        }

        public override bool OnHitBlock(AmmunitionData args, Vec3U16 pos, BlockID block)
        {
            args.DoTeleport(p);
            return true;
        }
    }
}