﻿/*
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
using Flames.Generator.fCraft;
using BlockID = System.UInt16;

namespace Flames.Generator.Foliage 
{
    public delegate void TreeOutput(ushort x, ushort y, ushort z, BlockID block);
    
    public delegate Tree TreeConstructor();
    
    public abstract class Tree 
    {
        protected internal int height, size;
        protected Random rnd;
        

        /// <summary> Minimum allowed size (usually means height) for this tree. </summary>
        public virtual int MinSize { get { return 3; } }

        /// <summary> Maximum allowed size (usually means height) for this tree. </summary>
        public virtual int MaxSize { get { return 4096; } }
        
        /// <summary> Estimated the maximum number of blocks affected by this tree. </summary>
        public abstract long EstimateBlocksAffected();
        
        /// <summary> Calculates a random default size (usually means height) for this tree. </summary>
        public abstract int DefaultSize(Random rnd);
                
        /// <summary> Initalises data (e.g. height and size) for this tree using the input value. </summary>
        public abstract void SetData(Random rnd, int value);

        /// <summary> Outputs the blocks generated by this tree at the given coordinates. </summary>
        public abstract void Generate(ushort x, ushort y, ushort z, TreeOutput output);
        

        /// <summary> Returns true if any green or trunk blocks are in the cube centred at (x, y, z) of extent 'size'. </summary>
        public static bool TreeCheck(Level lvl, ushort x, ushort y, ushort z, short size) { //return true if tree is near
            for (int dy = -size; dy <= size; ++dy)
                for (int dz = -size; dz <= size; ++dz)
                    for (int dx = -size; dx <= size; ++dx)
            {
                BlockID block = lvl.GetBlock((ushort)(x + dx), (ushort)(y + dy), (ushort)(z + dz));
                if (block == Block.Log || block == Block.Green) return true;
            }
            return false;
        } // TODO move to generic helper function
        
        
        public static Dictionary<string, TreeConstructor> TreeTypes = 
            new Dictionary<string, TreeConstructor>() {
            { "Fern", () => new NormalTree() },   { "Cactus", () => new CactusTree() },
            { "Notch", () => new ClassicTree() }, { "Swamp", () => new SwampTree() },
            { "Bamboo", () => new BambooTree() }, { "Palm", () => new PalmTree() },
            { "Oak", () => new OakTree() },       { "Ash", () => new AshTree() },            
            { "Round", () => new RoundTree() },   { "Cone", () => new ConeTree() }, 
            { "Rainforest", () => new RainforestTree() }, { "Mangrove", () => new MangroveTree() },
            { "fCraft", () => new fCraftTree() }
        };
        
        public static Tree Find(string name) {
            foreach (var entry in TreeTypes) 
            {
                if (entry.Key.CaselessEq(name)) return entry.Value();
            }
            return null;
        }
    }
}