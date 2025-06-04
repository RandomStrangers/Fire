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
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Flames.Levels.IO
{

    //WARNING! DO NOT CHANGE THE WAY THE LEVEL IS SAVED/LOADED!
    //You MUST make it able to save and load as a new version other wise you will make old levels incompatible!
    public unsafe class McfExporter : IMapExporter
    {
        public override string Extension { get { return ".mcf"; } }

        public override void Write(Stream dst, Level lvl)
        {
            using (Stream gs = new GZipStream(dst, CompressionMode.Compress))
            {
                // We need to copy blocks to a temp byte array due to the multithreaded nature of the server
                // Otherwise, some blocks can change between writing data and calculating its crc32, which
                // then causes the level to fail to load next time do to the crc32 not matching the data.
                byte[] buffer = new byte[16];
                WriteHeader(lvl, gs, buffer);
                // lock physics so it can't change blocks or checks during saving
                lock (lvl.physTickLock)
                {
                    WriteBlocksSection(lvl, gs);
                }
            }
        }
        public static void WriteHeader(Level lvl, Stream gs, byte[] header)
        {
            BitConverter.GetBytes(1874).CopyTo(header, 0);
            gs.Write(header, 0, 2);
            BitConverter.GetBytes(lvl.Width).CopyTo(header, 0);
            BitConverter.GetBytes(lvl.Height).CopyTo(header, 2);
            BitConverter.GetBytes(lvl.Length).CopyTo(header, 4);
            lvl.Changed = false;
            BitConverter.GetBytes(lvl.spawnx).CopyTo(header, 6);
            BitConverter.GetBytes(lvl.spawnz).CopyTo(header, 8);
            BitConverter.GetBytes(lvl.spawny).CopyTo(header, 10);
            header[12] = lvl.rotx;
            header[13] = lvl.roty;
            header[14] = (byte)lvl.VisitAccess.Min;
            header[15] = (byte)lvl.BuildAccess.Min;
            gs.Write(header, 0, header.Length);
        }

        public static void WriteBlocksSection(Level lvl, Stream gs)
        {
            var bl = new byte[lvl.blocks.Length * 2];
            for (int i = 0; i < lvl.blocks.Length; ++i)
            {
                ushort blockVal = 0;
                if (lvl.blocks[i] < 57)
                //CHANGED THIS TO INCOPARATE SOME MORE SPACE THAT I NEEDED FOR THE door_orange_air ETC.
                {
                    if(lvl.blocks[i] != Block.Air)
                        blockVal = (ushort)lvl.blocks[i];
                }
                else
                {
                    if (Block.Convert(lvl.blocks[i]) != Block.Air)
                        blockVal = (ushort)Block.Convert(lvl.blocks[i]);
                }
                bl[i*2] = (byte)blockVal;
                bl[i*2 + 1] = (byte)(blockVal >> 8);
            }
            gs.Write(bl, 0, bl.Length);
        }
    }
}
