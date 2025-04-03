/*
   Copyright 2015-2024 MCGalaxy

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
using System.ComponentModel;
namespace Flames.Gui
{
    public class LevelProperties
    {
        public Level Lvl;
        public LevelConfig Cfg;
        public LevelProperties(Level lvl)
        {
            Lvl = lvl;
            Cfg = lvl.Config;
        }
        [Description("Message shown to users when they join this level.")]
        [Category("General")]
        [DisplayName("MOTD")]
        public string MOTD 
        { 
            get 
            { 
                return Cfg.MOTD; 
            } 
            set 
            {
                DoMap(LevelOptions.MOTD, value); 
            } 
        }
        [Description("Whether this map automatically loads when the server is started.")]
        [Category("General")]
        [DisplayName("Load on startup")]
        public bool AutoLoad 
        { 
            get 
            { 
                return GetAutoload(); 
            }
            set 
            { 
                SetAutoload(value); 
            } 
        }
        [Description("Whether if a player uses /goto on this map and this map is not " +
                     "Loaded, the server then automatically loads this map.")]
        [Category("General")]
        [DisplayName("Load on /goto")]
        public bool LoadOnGoto 
        { 
            get 
            { 
                return Cfg.LoadOnGoto; 
            }
            set 
            { 
                DoMap(LevelOptions.Goto, value); 
            } 
        }
        [Description("Whether this level should be automatically unloaded " +
                     "if there are no players on it anymore.")]
        [Category("General")]
        [DisplayName("Unload when empty")]
        public bool UnloadWhenEmpty 
        { 
            get 
            { 
                return Cfg.AutoUnload; 
            } 
            set 
            { 
                DoMap(LevelOptions.Unload, value); 
            } 
        }
        [Description("Whether chat on this level is only shown to players in the level," +
                     "and not to any other levels or IRC (if enabled).")]
        [Category("General")]
        [DisplayName("Level only chat")]
        public bool LevelOnlyChat 
        { 
            get 
            { 
                return !Cfg.ServerWideChat; 
            } 
            set 
            { 
                DoMap(LevelOptions.Chat, value); 
            } 
        }
        [Description("If this is 0 then physics is disabled. If this is 5 then only door physics are used." +
                     "Values between 1-4 are varying levels of physics.")]
        [Category("General")]
        [DisplayName("Physics level")]
        public int PhysicsLevel 
        { 
            get 
            { 
                return Lvl.physics; 
            } 
            set 
            { 
                DoCmd("Physics", value); 
            } 
        }
        [Description("If physics is active, whether liquids spread infinitely or not.")]
        [Category("Physics")]
        [DisplayName("Finite liquids")]
        public bool FiniteLiquids 
        { 
            get 
            { 
                return Cfg.FiniteLiquids; 
            } 
            set 
            { 
                DoMap(LevelOptions.Finite, value); 
            } 
        }
        [Description("If physics is active, whether saplings should randomly grow into trees.")]
        [Category("Physics")]
        [DisplayName("Tree growing")]
        public bool TreeGrowing 
        { 
            get 
            { 
                return Cfg.GrowTrees; 
            } 
            set 
            {
                DoMap(LevelOptions.Trees, value); 
            } 
        }
        [Description("If physics is active, whether leaf blocks not connected to trees should decay.")]
        [Category("Physics")]
        [DisplayName("Leaf decay")]
        public bool LeafDecay 
        { 
            get 
            { 
                return Cfg.LeafDecay; 
            } 
            set 
            { 
                DoMap(LevelOptions.Decay, value); 
            } 
        }
        [Description("If physics is active, whether spreading liquids randomly pick a direction to flow in." +
                     "Otherwise, spreads in all directions.")]
        [Category("Physics")]
        [DisplayName("Random flow")]
        public bool RandomFlow 
        { 
            get 
            { 
                return Cfg.RandomFlow; 
            } 
            set 
            { 
                DoMap(LevelOptions.Flow, value); 
            } 
        }
        [Description("If physics is active, water flows from the map edges into the world.")]
        [Category("Physics")]
        [DisplayName("Edge water")]
        public bool EdgeWater 
        { 
            get 
            {
                return Cfg.EdgeWater; 
            } 
            set 
            { 
                DoMap(LevelOptions.Edge, value); 
            } 
        }
        [Description("If physics is active, whether 'animal' (bird, fish, etc) blocks " +
                     "should move towards the nearest player.")]
        [Category("Physics")]
        [DisplayName("Animal hunt AI")]
        public bool AnimalHuntAI 
        { 
            get 
            { 
                return Cfg.AnimalHuntAI; 
            } 
            set 
            { 
                DoMap(LevelOptions.AI, value); 
            } 
        }
        [Description("Whether dirt grows into grass.")]
        [Category("Physics")]
        [DisplayName("Grass growth")]
        public bool GrassGrowth 
        { 
            get 
            { 
                return Cfg.GrassGrow; 
            } 
            set 
            { 
                DoMap(LevelOptions.Grass, value); 
            } 
        }
        [Description("Whether gun usage is allowed.")]
        [Category("Survival")]
        [DisplayName("Guns")]
        public bool Guns 
        { 
            get 
            { 
                return Cfg.Guns; 
            } 
            set 
            { 
                DoMap(LevelOptions.Guns, value); 
            } 
        }
        [Description("Whether certain blocks can kill players.")]
        [Category("Survival")]
        [DisplayName("Killer blocks")]
        public bool KillerBlocks 
        { 
            get 
            { 
                return Cfg.KillerBlocks; 
            } 
            set 
            { 
                DoMap(LevelOptions.Killer, value); 
            } 
        }
        [Description("Whether players can die from drowning and falling from too high.")]
        [Category("Survival")]
        [DisplayName("Survival death")]
        public bool SurvivalDeath 
        { 
            get 
            { 
                return Cfg.SurvivalDeath; 
            } 
            set 
            { 
                DoMap(LevelOptions.Death, value); 
            } 
        }
        [Description("Time taken before players drown, in tenths of a second.")]
        [Category("Survival")]
        [DisplayName("Drown time")]
        public int DrownTime 
        { 
            get 
            { 
                return Cfg.DrownTime; 
            } 
            set 
            { 
                DoMap(LevelOptions.Drown, value); 
            } 
        }
        [Description("Falling more than this number of blocks results in death.")]
        [Category("Survival")]
        [DisplayName("Fall height")]
        public int FallHeight 
        { 
            get 
            { 
                return Cfg.FallHeight; 
            } 
            set 
            { 
                DoMap(LevelOptions.Fall, value); 
            } 
        }
        public bool GetAutoload()
        {
            return Server.AutoloadMaps.Contains(Lvl.name);
        }
        public void SetAutoload(bool value)
        {
            if (value)
            {
                Server.AutoloadMaps.Update(Lvl.name, Lvl.physics.ToString());
            }
            else
            {
                Server.AutoloadMaps.Remove(Lvl.name);
            }
            Server.AutoloadMaps.Save();
        }
        public void DoMap(string key, object raw)
        {
            DoCmd("Map", key + " " + raw);
        }
        public void DoCmd(string cmd, object raw)
        {
            try
            {
                string args = raw == null ? "" : raw.ToString();
                string cmdArgs = args.Length == 0 ? Lvl.name : Lvl.name + " " + args;
                Command.Find(cmd).Use(Player.Flame, cmdArgs);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}