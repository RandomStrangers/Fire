using System;
using Flames.Tasks;
using Flames.Scripting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Flames;

namespace Flames
{
    public class SimplePluginLoader : Plugin
    {
        public override string name { get { return "SimplePluginLoader"; } }
        public override string Flames_Version { get { return Server.Version; } }
        public override string creator { get { return "HarmonyNetwork"; } }
        public override void Load(bool startup)
        {
            AutoloadSimplePlugins2();
        }
            public static string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);        
        public static void AutoloadSimplePlugins2()
        {
            string[] files = AtomicIO.TryGetFiles(path,"*.dll");
            
            if (files != null)
            {
                foreach (string path in files) { IScripting_Simple.LoadSimplePlugin(path, true); }
            }
            else
            {
                Directory.CreateDirectory("");
            }
        }
        public override void Unload(bool shutdown)
        {
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
}