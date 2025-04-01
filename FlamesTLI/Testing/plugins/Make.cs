//reference System.Core.dll
using Flames.Commands;
using Flames.Blocks;
using System.Collections.Generic;
namespace Flames
{
    public class Make : Plugin
    {
        public override string name { get { return "Make"; } }
        public override string creator { get { return "Goodly"; } }
        public override string Flames_Version { get { return Server.Version; } }

        public static Command makeCommand = new CmdMake();
        public static Command makeGBCommand = new CmdMakeGB();
        public override void Load(bool startup)
        {
            CmdMakeBase.Load();

            Command.Register(makeCommand);
            Command.Register(makeGBCommand);
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(makeCommand);
            Command.Unregister(makeGBCommand);
        }
    }

    public abstract class CmdMakeBase : Command2
    {

        public delegate void MakeType(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot);
        public static string TYPES = "";
        public static Dictionary<string, MakeType> makeActions = new Dictionary<string, MakeType>();

        public static object locker = new object();

        public static void Load()
        {
            makeActions["slabs"] = MakeSlabs;
            makeActions["walls"] = MakeWalls;
            makeActions["stairs"] = MakeStairs;
            makeActions["flatstairs"] = MakeFlatStairs;
            makeActions["corners"] = MakeCorners;
            makeActions["eighths"] = MakeEighths;
            makeActions["panes"] = MakePanes;
            makeActions["poles"] = MakePoles;

            TYPES = string.Join(", ", makeActions.Keys);
        }

        public static string GetBlockName(Player p, ushort blockID)
        {
            blockID = Block.FromRaw(blockID); //convert from clientside to server side ID

            if (Block.IsPhysicsType(blockID)) return "Physics block";

            BlockDefinition def = null;
            if (!p.IsSuper)
            {
                def = p.level.GetBlockDef(blockID);
            }
            else
            {
                def = BlockDefinition.GlobalDefs[blockID];
            }
            if (def == null && blockID > 0 && blockID < Block.CPE_COUNT)
            {
                p.Message("Making default");
                def = DefaultSet.MakeCustomBlock(blockID);
                p.Message("def.Name is {0}", def.Name);
            }

            if (def != null) 
            { 
                return def.Name; 
            }

            return "Unknown";
        }

        public abstract bool CanUse(Player p);
        public abstract Command BlockDefCommand { get; }
        public abstract int Dir { get; }
        public abstract int DefaultRequestedSlot { get; }
        public abstract ushort IterateTo { get; }
        public abstract BlockDefinition[] GetBlockDefs(Player p);
        public abstract string BlockPropsScope { get; }

        public Command _defCmd;
        public Command defCmd { get { return _defCmd; } set { _defCmd = value; } }
        public override void Use(Player p, string message, CommandData data)
        {
            if (!CanUse(p))
            {
                return;
            }

            if (message == "") 
            { 
                Help(p); 
                return; 
            }
            string[] args = message.SplitSpaces(3);
            if (args.Length < 2) 
            { 
                Help(p); 
                return; 
            }

            string type = args[0].ToLower();
            ushort sourceID;
            if (!CommandParser.GetBlock(p, args[1], out sourceID)) 
            { 
                return; 
            }
            sourceID = Block.Convert(sourceID); // convert physics blocks to their visual form
            sourceID = Block.ToRaw(sourceID);   // convert server block IDs to client block IDs

            string name = GetBlockName(p, sourceID);
            int requestedSlot = DefaultRequestedSlot;
            if (requestedSlot == -1)
            {
                p.Message("&WThere are no unused custom block ids left.");
                return;
            }
            if (args.Length > 2)
            {
                if (!CommandParser.GetInt(p, args[2], "starting ID", ref requestedSlot, Block.CPE_COUNT, Block.MaxRaw)) 
                { 
                    return; 
                }
            }

            //p.Message("Requested slot is {0}", requestedSlot);
            //p.Message("IterateTo: {0}", IterateTo);

            if (makeActions.ContainsKey(type))
            {
                lock (locker)
                {
                    defCmd = BlockDefCommand; //assign to temporary variable to avoid Command.Find spam
                    makeActions[type](this, p, sourceID, name, (ushort)requestedSlot);
                }
            }
            else
            {
                p.Message("&WInvalid type \"{0}\".", type);
                p.Message("Types can be: &b{0}", TYPES);
            }
        }

        public bool Iterator(ref ushort b)
        {
            if (b > IterateTo) 
            { 
                b--;
                return true; 
            }
            else if (b < IterateTo) 
            { 
                b++; 
                return true; 
            }
            //reached goal stop iterating
            return false;
        }
        public bool AreEnoughBlockIDsFree(Player p, int amountRequested, out int startID, ushort requestedSlot)
        {
            startID = 0;
            int amountFound = 0;

            BlockDefinition[] defs = GetBlockDefs(p);

            ushort b = requestedSlot;
            do
            {
                ushort cur = Block.FromRaw(b);
                if (defs[cur] == null)
                {
                    if (startID == 0)
                    {
                        startID = b;
                    }
                    amountFound++;
                    if (amountFound == amountRequested)
                    {
                        return true;
                    }
                    continue;
                }

                if (startID != 0)
                {
                    //if a starting point was already found and we run into a used slot before finding the amount requested, stop
                    break;
                }
            } 
            while (Iterator(ref b));

            p.Message("&WThere are not enough free sequential block IDs to perform this command.");
            string start = (startID != 0) ? " starting at " + startID : "";
            p.Message("(found {0} free IDs{1}, but needed {2})", amountFound, start, amountRequested);
            return false;
        }

        public static void MakePanes(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 2, out dest, requestedSlot)) 
            { 
                return; 
            }

            //WE  (0, 0, 6) to (16, 16, 10)
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-WE");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 6");
            maker.defCmd.Use(p, "edit " + dest + " max 16 16 10");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //NS (6, 0, 0) to (10, 16, 16)
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NS");
            maker.defCmd.Use(p, "edit " + dest + " min 6 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 10 16 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeSlabs(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 2, out dest, requestedSlot)) 
            { 
                return; 
            }

            //down
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 8 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 1");
            Find("blockproperties").Use(p, maker.BlockPropsScope + " " + (dest) + " stackblock " + origin);
            dest += maker.Dir;
            //up
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 16 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 1");

        }
        public static void MakeWalls(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 4, out dest, requestedSlot)) 
            { 
                return; 
            }

            int height = 16;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-E");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeStairs(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 8, out dest, requestedSlot)) 
            { 
                return; 
            }


            int height = 8;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-E");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;

            //--------------------------------------------------------------------------------upper
            height = 16;

            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-E");
            maker.defCmd.Use(p, "edit " + dest + " min 8 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeFlatStairs(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 8, out dest, requestedSlot)) 
            {
                return;
            }


            int height = 8;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 1");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 15");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 1 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-E");
            maker.defCmd.Use(p, "edit " + dest + " min 15 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;

            //--------------------------------------------------------------------------------upper
            height = 16;

            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-N");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 1");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-S");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 15");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-W");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 1 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-E");
            maker.defCmd.Use(p, "edit " + dest + " min 15 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeCorners(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 4, out dest, requestedSlot)) 
            { 
                return; 
            }

            int height = 16;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-SE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-SW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakeEighths(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 8, out dest, requestedSlot)) 
            { 
                return; 
            }

            int height = 8;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-NW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-SE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-SW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 0 8");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-D-NE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 0 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;


            height = 16;
            //north
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-NW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //south
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-SE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 8 8");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //west
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-SW");
            maker.defCmd.Use(p, "edit " + dest + " min 0 8 8");
            maker.defCmd.Use(p, "edit " + dest + " max 8 " + height + " 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //east
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-U-NE");
            maker.defCmd.Use(p, "edit " + dest + " min 8 8 0");
            maker.defCmd.Use(p, "edit " + dest + " max 16 " + height + " 8");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }
        public static void MakePoles(CmdMakeBase maker, Player p, int origin, string name, ushort requestedSlot)
        {
            int dest;
            if (!maker.AreEnoughBlockIDsFree(p, 3, out dest, requestedSlot)) 
            { 
                return; 
            }

            //UD
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-UD");
            maker.defCmd.Use(p, "edit " + dest + " min 4 0 4");
            maker.defCmd.Use(p, "edit " + dest + " max 12 16 12");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //NS
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-NS");
            maker.defCmd.Use(p, "edit " + dest + " min 4 4 0");
            maker.defCmd.Use(p, "edit " + dest + " max 12 12 16");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
            dest += maker.Dir;
            //WE
            maker.defCmd.Use(p, "copy " + origin + " " + dest);
            maker.defCmd.Use(p, "edit " + dest + " name " + name + "-WE");
            maker.defCmd.Use(p, "edit " + dest + " min 0 4 4");
            maker.defCmd.Use(p, "edit " + dest + " max 16 12 12");
            maker.defCmd.Use(p, "edit " + dest + " blockslight 0");
        }

        public override void Help(Player p)
        {
            p.Message("&T/{0} [type] [block] <optional starting ID>", name);
            p.Message("&HCreates block variants out of [block] based on [type].");
            p.Message("&H[type] can be: &b{0}", TYPES);
            p.Message("&HFor example, &T/make eighths stone &Hwould give you 8 new stone eighth piece blocks.");
        }
    }

    public class CmdMake : CmdMakeBase
    {
        public override string name { get { return "Make"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Guest; } }


        public override bool CanUse(Player p)
        {
            bool canUse = false;
            if (LevelInfo.IsRealmOwner(p.name, p.level.name)) canUse = true;
            if (!LevelInfo.Check(p, p.group.Permission, p.level, "make new blocks in this level")) 
            { 
                return false;
            } //restrict making new blocks if you can't edit blockprops
            if (p.group.Permission >= LevelPermission.Operator && p.group.Permission >= p.level.BuildAccess.Min) 
            { 
                canUse = true; 
            }
            if (!canUse) 
            { 
                p.Message("&cYou can only use this command on your own maps."); 
            }
            return canUse;
        }
        public override Command BlockDefCommand { get { return Find("levelblock"); } }
        public override int Dir { get { return -1; } }
        public override int DefaultRequestedSlot { get { return Block.MaxRaw; } }
        public override ushort IterateTo { get { return Block.CPE_COUNT; } }
        public override BlockDefinition[] GetBlockDefs(Player p)
        {
            return p.level.CustomBlockDefs;
        }
        public override string BlockPropsScope { get { return "level"; } }
    }

    public class CmdMakeGB : CmdMakeBase
    {
        public override string name { get { return "MakeGB"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }


        public override bool CanUse(Player p) { return true; }
        public override Command BlockDefCommand { get { return Find("globalblock"); } }
        public override int Dir { get { return 1; } }
        public override int DefaultRequestedSlot
        {
            get
            {
                BlockDefinition[] defs = BlockDefinition.GlobalDefs;
                for (ushort b = Block.CPE_COUNT; b <= Block.MaxRaw; b++)
                {
                    ushort block = Block.FromRaw(b);
                    if (defs[block] == null) return b;
                }
                return -1;
            }
        }
        public override ushort IterateTo { get { return Block.MaxRaw; } }
        public override BlockDefinition[] GetBlockDefs(Player p) 
        { 
            return BlockDefinition.GlobalDefs; 
        }
        public override string BlockPropsScope { get { return "global"; } }
    }
}