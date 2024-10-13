﻿/*
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
using Flames.Commands;
using Flames.Events.ServerEvents;

namespace Flames.Modules.Relay.IRC 
{   
    public sealed class IRCPlugin : Plugin 
    {
        public override string name { get { return "IRCRelay"; } }

        public static IRCBot Bot = new IRCBot();

        public static Command cmdIrcBot   = new CmdIRCBot();
        public static Command cmdIrcCtrls = new CmdIrcControllers();
        
        public override void Load(bool startup) {
            Command.Register(cmdIrcBot);
            Command.Register(cmdIrcCtrls);

            Bot.ReloadConfig();
            Bot.Connect();
            OnConfigUpdatedEvent.Register(OnConfigUpdated, Priority.Low);
        }
        
        public override void Unload(bool shutdown) {
            Command.Unregister(cmdIrcBot, cmdIrcCtrls);
            
            OnConfigUpdatedEvent.Unregister(OnConfigUpdated);
            Bot.Disconnect("Disconnecting IRC bot");
        }

        public void OnConfigUpdated() { Bot.ReloadConfig(); }
    }

    public sealed class CmdIRCBot : RelayBotCmd 
    {
        public override string name { get { return "IRCBot"; } }
        public override CommandAlias[] Aliases {
            get { return new[] { new CommandAlias("ResetBot", "reset"), new CommandAlias("ResetIRC", "reset") }; }
        }
        public override RelayBot Bot { get { return IRCPlugin.Bot; } }
    }

    public sealed class CmdIrcControllers : BotControllersCmd 
    {
        public override string name { get { return "IRCControllers"; } }
        public override string shortcut { get { return "IRCCtrl"; } }
        public override RelayBot Bot { get { return IRCPlugin.Bot; } }
    }
}
