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
using System;
using System.Collections.Generic;
using Flames.Commands;

namespace Flames.Blocks
{

    public sealed class BlockOption
    {
        public string Name, Help;
        public BlockOptions.OptionSetter SetFunc;

        public BlockOption(string name, BlockOptions.OptionSetter func, string help)
        {
            Name = name;
            SetFunc = func;
            Help = help;
        }
    }

    public static class BlockOptions
    {
        public delegate void OptionSetter(Player p, BlockProps[] scope, ushort b, string value);

        public static List<BlockOption> Options = new List<BlockOption>() 
        {
            new BlockOption("Portal",       SetPortal, "&HToggles whether the block is a &T/Portal"),
            new BlockOption("MessageBlock", SetMB,     "&HToggles whether the block is a &T/MessageBlock"),
            new BlockOption("Rails",        SetRails,  "&HToggles whether &Strain &Hblocks can run over this block"),
            new BlockOption("WaterKills",   SetWater,  "&HToggles whether flooding water kills this block"),
            new BlockOption("LavaKills",    SetLava,   "&HToggles whether flooding lava kills this block"),
            new BlockOption("Door",         SetDoor,   "&HToggles whether this block is a Door block"),
            new BlockOption("tDoor",        SetTDoor,  "&HToggles whether this block is a tDoor block"),
            new BlockOption("Killer",       SetKiller, "&HToggles whether colliding with this block kills player"),
            new BlockOption("DeathMessage", SetDeathMsg, "&HSets or removes the death message for this block " +
                            "(Note: &S@p &His a placeholder for name of player who dies"),
            new BlockOption("AnimalAI",   SetAI,     "&HSets the flying or swimming animal AI for this block. " +
                            "Types: &f" + Enum.GetNames(typeof(AnimalAI)).Join()),
            new BlockOption("StackBlock", SetStackId,"&HSets the block this block is changed into, when placed on top " +
                            "of itself (e.g. placing a slab on top of another slab turns into a double slab)"),
            new BlockOption("OPBlock",    SetOPBlock,"&HMarks the block as being on OP block. OP blocks can't be " +
                            "blown up by explosions, and can't be replaced in games when build type is ModifyOnly."),
            new BlockOption("Drownable",  SetDrown,  "&HSets whether this block can drown players " +
                            "(Note &T/Map death &Hmust be enabled for players to drown)"),
            new BlockOption("Grass",      SetGrass,  "&HSets the block that this block is changed into, " +
                            "when exposed to sunlight (leave block blank to remove)"),
            new BlockOption("Dirt",       SetDirt,   "&HSets the block that this block is changed into, " +
                            "when no longer exposed to sunlight (leave block blank to remove)"),
            new BlockOption("oDoor",      SetODoor,  "&HSets the block that this block is changed into, " +
                            "when activated by a neighbouring door"),
        };

        public static BlockOption Find(string opt)
        {
            if (opt.CaselessEq("MB")) opt = "MessageBlock";
            if (opt.CaselessEq("Death")) opt = "Killer";
            if (opt.CaselessEq("DeathMsg")) opt = "DeathMessage";
            if (opt.CaselessEq("Animal")) opt = "AnimalAI";
            if (opt.CaselessEq("StackID")) opt = "StackBlock";
            if (opt.CaselessEq("Drown")) opt = "Drownable";

            foreach (BlockOption option in Options)
            {
                if (option.Name.CaselessEq(opt)) return option;
            }
            return null;
        }


        public static void SetPortal(Player p, BlockProps[] s, ushort b, string v)
        {
            ToggleBehaviour(p, s, b, "a portal", ref s[b].IsPortal);
        }
        public static void SetMB(Player p, BlockProps[] s, ushort b, string v)
        {
            ToggleBehaviour(p, s, b, "a message block", ref s[b].IsMessageBlock);
        }
        public static void SetRails(Player p, BlockProps[] s, ushort b, string v)
        {
            Toggle(p, s, b, "train rails", ref s[b].IsRails);
        }
        public static void SetWater(Player p, BlockProps[] s, ushort b, string v)
        {
            Toggle(p, s, b, "killed by water", ref s[b].WaterKills);
        }
        public static void SetLava(Player p, BlockProps[] s, ushort b, string v)
        {
            Toggle(p, s, b, "killed by lava", ref s[b].LavaKills);
        }
        public static void SetDoor(Player p, BlockProps[] s, ushort b, string v)
        {
            ToggleBehaviour(p, s, b, "a door", ref s[b].IsDoor);
        }
        public static void SetTDoor(Player p, BlockProps[] s, ushort b, string v)
        {
            ToggleBehaviour(p, s, b, "a tDoor", ref s[b].IsTDoor);
        }
        public static void SetKiller(Player p, BlockProps[] s, ushort b, string v)
        {
            Toggle(p, s, b, "a killer block", ref s[b].KillerBlock);
        }
        public static void SetOPBlock(Player p, BlockProps[] s, ushort b, string v)
        {
            Toggle(p, s, b, "an OP block", ref s[b].OPBlock);
        }
        public static void SetDrown(Player p, BlockProps[] s, ushort b, string v)
        {
            Toggle(p, s, b, "drowns players", ref s[b].Drownable);
        }
        public static void SetGrass(Player p, BlockProps[] s, ushort b, string v)
        {
            SetBlock(p, s, b, v, "Grass form", ref s[b].GrassBlock);
        }
        public static void SetDirt(Player p, BlockProps[] s, ushort b, string v)
        {
            SetBlock(p, s, b, v, "Dirt form", ref s[b].DirtBlock);
        }
        public static void SetODoor(Player p, BlockProps[] s, ushort b, string v)
        {
            SetBlock(p, s, b, v, "oDoor form", ref s[b].oDoorBlock);
        }

        // NOTE: Make sure to keep this in sync with BlockBehaviour.GetDeleteHandler
        public static string CheckBehaviour(BlockProps[] props, ushort block)
        {
            if (props[block].IsMessageBlock) return "message block";
            if (props[block].IsPortal) return "portal";
            if (props[block].IsTDoor) return "tDoor";
            if (props[block].oDoorBlock != Block.Invalid) return "oDoor";
            if (props[block].IsDoor) return "door";
            return null;
        }

        public static void ToggleBehaviour(Player p, BlockProps[] scope, ushort block, string type, ref bool on)
        {
            string behaviour;
            // Check if this would make a block both a door and a portal for instance
            // If blocks have multiple behaviour, this would confuse users because only 1 behaviour works
            if (!on && (behaviour = CheckBehaviour(scope, block)) != null)
            {
                string name = BlockProps.ScopedName(scope, p, block);
                p.Message("&WBlock {0} cannot be made {1}, it is already a {2}", name, type, behaviour);
                return;
            }
            Toggle(p, scope, block, type, ref on);
        }

        public static void Toggle(Player p, BlockProps[] scope, ushort block, string type, ref bool on)
        {
            on = !on;
            string name = BlockProps.ScopedName(scope, p, block);
            p.Message("Block {0} is {1}: {2}", name, type, on ? "&aYes" : "&cNo");
        }

        public static void SetAI(Player p, BlockProps[] scope, ushort block, string msg)
        {
            AnimalAI ai = AnimalAI.None;
            if (!CommandParser.GetEnum(p, msg, "Animal AI", ref ai)) return;
            scope[block].AnimalAI = ai;

            string name = BlockProps.ScopedName(scope, p, block);
            p.Message("Animal AI for {0} set to: {1}", name, ai);
        }

        public static void SetDeathMsg(Player p, BlockProps[] scope, ushort block, string msg)
        {
            string name = BlockProps.ScopedName(scope, p, block);
            if (msg.Length == 0)
            {
                scope[block].DeathMessage = null;
                p.Message("Death message for {0} removed.", name);
            }
            else
            {
                scope[block].DeathMessage = msg;
                p.Message("Death message for {0} set to: {1}", name, msg);
            }
        }

        public static void SetStackId(Player p, BlockProps[] scope, ushort block, string msg)
        {
            ushort stackBlock;
            if (msg.Length == 0)
            {
                stackBlock = Block.Air;
            }
            else
            {
                if (!CommandParser.GetBlock(p, msg, out stackBlock)) return;
            }
            scope[block].StackBlock = stackBlock;

            string name = BlockProps.ScopedName(scope, p, block);
            if (stackBlock == Block.Air)
            {
                p.Message("Removed stack block for {0}", name);
            }
            else
            {
                p.Message("Stack block for {0} set to: {1}",
                          name, BlockProps.ScopedName(scope, p, stackBlock));
            }
        }

        public static void SetBlock(Player p, BlockProps[] scope, ushort block,
                             string msg, string type, ref ushort target)
        {
            string name = BlockProps.ScopedName(scope, p, block);
            if (msg.Length == 0)
            {
                target = Block.Invalid;
                p.Message("{1} for {0} removed.", name, type);
            }
            else
            {
                if (!CommandParser.GetBlockIfAllowed(p, msg, "use", out ushort other)) return;
                if (other == block)
                {
                    p.Message("ID of {0} must be different.", type);
                    return;
                }

                target = other;
                p.Message("{2} for {0} set to: {1}",
                          name, BlockProps.ScopedName(scope, p, other), type);
            }
        }
    }
}