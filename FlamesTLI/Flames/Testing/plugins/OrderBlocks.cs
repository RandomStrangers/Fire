namespace Flames
{
    public class PluginOrderBlocks : Plugin
    {
        public override string name { get { return "OrderBlocks"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string creator { get { return "goodly"; } }

        public Command orderBlocksCmd = new CmdOrderBlocks();
        public override void Load(bool startup)
        {
            Command.Register(orderBlocksCmd);
        }
        public override void Unload(bool shutdown)
        {
            Command.Unregister(orderBlocksCmd);
        }
    }

    public class CmdOrderBlocks : Command2
    {
        public override string name { get { return "OrderBlocks"; } }
        public override string shortcut { get { return "BlockOrder"; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override string type { get { return CommandTypes.Added; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Admin; } }
        public override void Use(Player p, string message, CommandData data)
        {
            if (!CanUse(p)) { p.Message("You can't use {0} in this map. See /help OrderBlocks", name); return; }

            if (message.CaselessEq("place current order"))
            {
                PlaceCurrentOrder(p);
                return;
            }

            int loops = 767;
            for (int i = 1; i < loops + 1; ++i)
            {
                Find("globalblock").Use(p, "edit " + i + " order 0");

                //p.Message("i is " + i + ".");
            }

            int maxBlock = 767;
            int orderIndex = 1;
            for (ushort z = 0; z < 153; z = (ushort)(z + 2))
            {
                //p.Message("Z is {0}", z);

                for (ushort x = 0; x < 19; x = (ushort)(x + 2))
                {

                    //p.Message("X is {0}", x);
                    //p.level.UpdateBlock(p, x, 0, z, Block.Stone, BlockDBFlags.Drawn, true);
                    ushort curBlock = p.level.GetBlock(x, 0, z);

                    if (Block.IsPhysicsType(curBlock))
                    {
                        p.Message("Skipping block with ToRaw ID of {0}", Block.ToRaw(curBlock));
                        orderIndex++;
                        continue;
                    }
                    ushort clientID = Block.ToRaw(curBlock);

                    if (curBlock != Block.Air)
                    {
                        //p.Message("The block's ID is {0}", curBlock);
                        //p.Message("The block's clientside id is {0}", clientID);

                        Find("globalblock").Use(p, "edit " + clientID + " order " + orderIndex);
                    }

                    orderIndex++;
                    if (orderIndex >= maxBlock) 
                    { 
                        break; 
                    }
                }
                if (orderIndex >= maxBlock) 
                { 
                    break; 
                }
            }
            p.Message("We placed {0} blocks!", orderIndex);

        }

        public void PlaceCurrentOrder(Player p)
        {
            p.Message("Nothing");
        }

        public bool CanUse(Player p)
        {
            Level lvl = p.level;
            if (lvl.name != "blockorder") 
            { 
                return false; 
            }
            if (lvl.Width != 19) 
            { 
                return false; 
            }
            //if (lvl.Height != 1)
            //{
                //return false;
            //}
            if (lvl.Length != 153) 
            { 
                return false;
            }
            return true;
        }

        public override void Help(Player p)
        {
            p.Message("&T/OrderBlocks");
            p.Message("&HRe-orders the global inventory based on the layout of the blocks in the map you're in.");
            p.Message("&HThe map must be named \"blockorder\" and be sized 19 anyY 153.");
        }
    }
}