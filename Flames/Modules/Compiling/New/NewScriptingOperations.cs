/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
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
using System.IO;

namespace Flames.NewScripting
{
    public static class ScriptingOperations
    {
        public static bool LoadNewPlugins(Player p, string path) {
            if (!File.Exists(path)) {
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }
            
            try {
                List<NewPlugin> newplugins = IScripting.LoadNewPlugin(path, false);
                
                p.Message("New plugin {0} loaded successfully",
                          newplugins.Join(pl => pl.name));
                return true;
            } catch (AlreadyLoadedException ex) {
                p.Message(ex.Message);
                return false;
            } catch (Exception ex) {
                p.Message(IScripting.DescribeLoadError(path, ex));
                Logger.LogError("Error loading new plugins from " + path, ex);
                return false;
            }
        }
        
        public static bool UnloadNewPlugin(Player p, NewPlugin newplugin) {
            if (!NewPlugin.Unload(newplugin)) {
                p.Message("&WError unloading new plugin. See error logs for more information.");
                return false;
            }
            
            p.Message("New plugin {0} &Sunloaded successfully", newplugin.name);
            return true;
        }
    }
}
