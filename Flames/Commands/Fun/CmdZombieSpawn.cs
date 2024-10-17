/*
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
using System.Threading;
using BlockID = System.UInt16;

namespace Flames
{
    public class CmdZombieSpawn : Command
    {
        public override string name { get { return "Zombiespawn"; } }
        public override string shortcut { get { return "zspawn"; } }
        public override string type { get { return CommandTypes.Games; } }
        public override bool museumUsable { get { return false; } }
        public override bool SuperUseable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public int wavesNum, wavesLength, zombiesNum, thex, they, thez;
        public bool isRandom;

        public void ZombieMob(object person)
        {
            int xBegin = 0;
            int zBegin = 0;
            Player p = (Player)person;

            if (zombiesNum % 2 == 0 && !isRandom)
            {
                xBegin = thex - (zombiesNum / 2);
                zBegin = thez - (zombiesNum / 2);
            }

            if (zombiesNum % 2 == 1 && !isRandom)
            {
                xBegin = thex - ((zombiesNum - 1) / 2);
                zBegin = thez - ((zombiesNum - 1) / 2);
            }


            p.level.Message("&aInitiating zombie attack!");
            p.level.Message("&a" + wavesNum + " wave(s)");
            p.level.Message("&a" + wavesLength + " second(s) each wave");
            for (int num = 1; num <= wavesNum; num++)
            {
                if (isRandom)
                    RandomZombies(p);
                else
                    PlacedZombies(p, xBegin, zBegin);

                p.level.Message("&aZombie wave # " + num);
                Thread.Sleep(wavesLength * 1000);
            }
            p.level.Message("&aZombie attack is over.");
        }

        public void RandomZombies(Player p)
        {
            Random randomCoord = new Random();
            int x, y, z;

            for (int i = 0; i < zombiesNum; i++)
            {
                x = randomCoord.Next(0, p.level.Width);
                y = randomCoord.Next(p.level.Height / 2, p.level.Height);
                z = randomCoord.Next(0, p.level.Length);

                p.level.Blockchange((ushort)x, (ushort)y, (ushort)z, Block.ZombieBody);
            }
        }

        public void PlacedZombies(Player p, int xBegin, int zBegin)
        {
            for (int x = xBegin; x < xBegin + zombiesNum; x++)
            {
                for (int z = zBegin; z < zBegin + zombiesNum; z++)
                {
                    p.level.Blockchange((ushort)x, (ushort)they, (ushort)z, Block.ZombieBody);
                }
            }
        }

        public override void Use(Player p, string message)
        {
            int number = message.SplitSpaces().Length;
            string[] param = message.SplitSpaces();

            if (number == 1)
            {
                if (string.Compare(param[0], "x", true) == 0)
                {
                    Find("replaceall").Use(p, "zombie air");
                    p.Message("&aAll zombies have been destroyed.");
                    return;
                }
            }

            if (number != 4)
            {
                Help(p);
                return;
            }

            try
            {
                if (string.Compare(param[0], "r", true) == 0)
                {
                    isRandom = true;
                }
                else if (string.Compare(param[0], "d", true) == 0)
                {
                    isRandom = false;
                }
                else
                {
                    p.Message("Flag set must be 'r' or 'd'.");
                    return;
                }

                wavesNum = int.Parse(param[1]);
                wavesLength = int.Parse(param[2]);
                zombiesNum = int.Parse(param[3]);

                if (!isRandom)
                {
                    p.Message("Place a block for center of zombie spawn.");
                    p.ClearBlockchange();
                    p.Blockchange += Blockchange1;
                }
                else
                {
                    Thread t = new Thread(ZombieMob);
                    t.Start(p);
                }

            }
            catch (FormatException)
            {
                p.Message("&4All parameters must be numbers!");
            }

        }

        public void Blockchange1(Player p, ushort x, ushort y, ushort z, BlockID block)
        {
            p.ClearBlockchange();
            p.RevertBlock(x, y, z);

            thex = x;
            they = y + 2;
            thez = z;
            Thread t = new Thread(ZombieMob);
            t.Start(p);
        }

        public override void Help(Player p)
        {
            p.Message("&T/zombiespawn <flag> <x> <y> <z> &H- Spawns waves of zombies.");
            p.Message("&H<flag> - 'r' for random or 'd' for diameter");
            p.Message("&H<x> - the number of waves");
            p.Message("&H<y> - the length of the waves in seconds");
            p.Message("&H<z> - the number of zombies spawned/diameter of spawn");
            p.Message("&T/zombiespawn x &H- Destroys all zombies.");
        }
    }
}