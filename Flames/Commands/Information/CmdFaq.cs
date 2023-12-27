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
using Flames.Util;

namespace Flames.Commands.Info {
    public sealed class CmdFaq : Command2 {        
        public override string name { get { return "FAQ"; } }
        public override string type { get { return CommandTypes.Information; } }
        public override bool UseableWhenFrozen { get { return true; } }
        
        public override void Use(Player p, string message, CommandData data) {
            TextFile faqFile = TextFile.Files["FAQ"];
            faqFile.EnsureExists();
            
            string[] faq = faqFile.GetText();
            p.Message("&cFAQ&f:");
            foreach (string line in faq)
                p.Message("&f" + line);
        }

        public override void Help(Player p) {
            p.Message("&T/FAQ");
            p.Message("&HDisplays frequently asked questions");
        }
    }
}