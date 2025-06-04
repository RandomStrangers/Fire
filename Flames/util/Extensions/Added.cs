#if FLAMES_TLI
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using Flames.Added;
using Flames.Added.Compiling;
using Flames.Added.Experiments;
using Flames.Added.Scripting;
using Flames.Commands;
using Flames.Events.ServerEvents;
using Flames.SQL;
using Flames.Tasks;
namespace Flames.Added.Experiments
{
    public class ReqRequestLoad : Request
    {
        public static string ReqName = "RequestLoad";
        public override string RequestName
        {
            get
            {
                return ReqName;
            }
            set
            {
                ReqName = value;
            }
        }
        public override string RequestType { get { return RequestTypes.Request; } }
        public override string RequestShortcut { get { return "ReqLoad"; } }
        public override LevelPermission RequestDefaultRank { get { return LevelPermission.Owner; } }
        public override bool RequestMessageBlockRestricted { get { return true; } }
        public override void ExecuteRequest(Player p, string ordName)
        {
            if (ordName.Length == 0)
            {
                Help(p);
                return;
            }
            if (!Formatter.ValidFilename(p, ordName))
            {
                return;
            }
            string path = ExpIScripting.RequestPath(ordName);
            ExpScriptingOperations.LoadRequests(p, path);
        }
        public override void Help(Player p)
        {
            p.Message("&T/RequestLoad [request name]");
            p.Message("&HLoads a compiled request into the server for use.");
        }
    }
    public class ReqRequestUnload : Request
    {
        public static string ReqName = "RequestUnload";
        public override string RequestName
        {
            get
            {
                return ReqName;
            }
            set
            {
                ReqName = value;
            }
        }

        public override string RequestType { get { return RequestTypes.Request; } }
        public override string RequestShortcut { get { return "ReqUnload"; } }
        public override LevelPermission RequestDefaultRank { get { return LevelPermission.Owner; } }
        public override bool RequestMessageBlockRestricted { get { return true; } }
        public override void ExecuteRequest(Player p, string reqName)
        {
            if (reqName.Length == 0)
            {
                Help(p);
                return;
            }
            string ordArgs = "";
            Search(ref reqName, ref ordArgs);
            Request req = FindREQ(reqName);
            if (req == null)
            {
                p.Message("\"{0}\" is not a valid or loaded request.", reqName);
                return;
            }
            ExpScriptingOperations.UnloadRequest(p, req);
        }
        public override void Help(Player p)
        {
            p.Message("&T/RequestUnload [request]");
            p.Message("&HUnloads a request from the server.");
        }
    }
    public class ReqExperiments : Request
    {
        public static string ReqName = "Experiment";
        public override string RequestName
        {
            get
            {
                return ReqName;
            }
            set
            {
                ReqName = value;
            }
        }
        public override string RequestType
        {
            get
            {
                return RequestTypes.Request;
            }
        }
        public override LevelPermission RequestDefaultRank
        {
            get
            {
                return LevelPermission.Owner;
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ELoad", "load"),
                    new CommandAlias("EUnload", "unload"),
                    new CommandAlias("Experiments", "list")
                };
            }
        }
        public override bool RequestMessageBlockRestricted
        {
            get
            {
                return true;
            }
        }
        public override void ExecuteRequest(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            if (IsListAction(args[0]))
            {
                string modifier = args.Length > 1 ? args[1] : "";
                p.Message("Loaded experiments:");
                Paginator.Output(p, Experiment.experiments, exp => exp.Name,
                                 "Experiments", "experiments", modifier);
                return;
            }
            if (args.Length == 1)
            {
                Help(p);
                return;
            }
            string req = args[0], name = args[1];
            if (!Formatter.ValidFilename(p, name))
            {
                return;
            }
            if (req.CaselessEq("load"))
            {
                string path = ExpIScripting.ExperimentPath(name);
                ExpScriptingOperations.LoadExperiments(p, path);
            }
            else if (req.CaselessEq("unload"))
            {
                UnloadExperiment(p, name);
            }
            else if (req.CaselessEq("create"))
            {
                FindREQ("RequestCreate").ExecuteRequest(p, "experiment " + name);
            }
            else if (req.CaselessEq("compile"))
            {
                FindREQ("RequestCompile").ExecuteRequest(p, "experiment " + name);
            }
            else
            {
                Help(p);
            }
        }
        public static void UnloadExperiment(Player p, string name)
        {
            int matches;
            Experiment experiment = Matcher.Find(p, name, out matches, Experiment.experiments,
                                         null, exp => exp.ExperimentName, "experiments");
            if (experiment == null)
            {
                return;
            }
            ExpScriptingOperations.UnloadExperiment(p, experiment);
        }
        public override void Help(Player p)
        {
            p.Message("&T/Experiment load [filename]");
            p.Message("&HLoad a compiled experiment from the &fexperiments &Hfolder");
            p.Message("&T/Experiment unload [name]");
            p.Message("&HUnloads a currently loaded experiment");
            p.Message("&T/Experiment list");
            p.Message("&HLists all loaded experiments");
        }
    }
    public class ReqRequests : Request
    {
        public static string ReqName = "Requests";
        public override string RequestName
        {
            get
            {
                return ReqName;
            }
            set
            {
                ReqName = value;
            }
        }
        public override string RequestShortcut { get { return "Reqs"; } }
        public override string RequestType { get { return RequestTypes.Request; } }
        public override bool RequestUseableWhenFrozen { get { return true; } }
        public override CommandAlias[] Aliases
        {
            get { return new[] { new CommandAlias("ReqList") }; }
        }
        public override void ExecuteRequest(Player p, string message)
        {
            if (!ListRequests(p, message))
            {
                Help(p);
            }
        }
        public static bool ListRequests(Player p, string message)
        {
            string[] args = message.SplitSpaces();
            string sort = args.Length > 1 ? args[1].ToLower() : "";
            string modifier = args.Length > 2 ? args[2] : sort;
            if (args.Length == 2)
            {
                if (modifier == "name" || modifier == "names" || modifier == "rank" || modifier == "ranks")
                {
                    modifier = "";
                }
                else
                {
                    sort = "";
                }
            }
            string type = args[0].ToLower();
            if (type == "short" || type == "shortcut" || type == "shortcuts")
            {
                PrintShortcuts(p, modifier);
            }
            else if (type == "old" || type == "oldmenu" || type == "" || type == "order")
            {
                PrintRankRequests(p, sort, modifier, p.group, true);
            }
            else if (type == "all" || type == "requestall" || type == "requestsall")
            {
                PrintAllRequests(p, sort, modifier);
            }
            else
            {
                bool any = PrintCategoryRequests(p, sort, modifier, type);
                if (any) return true;
                Group grp = Group.Find(type);
                if (grp == null) return false;
                PrintRankRequests(p, sort, modifier, grp, false);
            }
            return true;
        }

        public static void PrintShortcuts(Player p, string modifier)
        {
            List<Request> shortcuts = new List<Request>();
            foreach (Request req in allReqs)
            {
                if (req.RequestShortcut.Length == 0) continue;
                if (!p.CanUse(req)) continue;
                shortcuts.Add(req);
            }

            Paginator.Output(p, shortcuts,
                             (req) => "&b" + req.RequestShortcut + " &S[" + req.RequestName + "]",
                             "Requests shortcuts", "shortcuts", modifier);
        }

        public static void PrintRankRequests(Player p, string sort, string modifier, Group group, bool own)
        {
            List<Request> req = new List<Request>();
            foreach (Request r in allReqs)
            {
                if (r.Permissions.UsableBy(group.Permission)) req.Add(r);
            }
            if (req.Count == 0)
            {
                p.Message("{0} &Scannot use any requests.", group.ColoredName);
                return;
            }
            SortRequests(req, sort);
            if (own)
                p.Message("Available requests:");
            else
                p.Message("Requests available to " + group.ColoredName + " &Srank:");

            string type = "Reqs " + group.Name;
            if (sort.Length > 0) type += " " + sort;
            Paginator.Output(p, req, GetColoredName,
                             type, "req", modifier);
            p.Message("Type &T/Help <request> &Sfor more help on a request.");
        }

        public static void PrintAllRequests(Player p, string sort, string modifier)
        {
            List<Request> req = CopyAll();
            SortRequests(req, sort);
            p.Message("All req:");

            string type = "Requests all";
            if (sort.Length > 0) type += " " + sort;
            Paginator.Output(p, req, GetColoredName,
                             type, "requests", modifier);
            p.Message("Type &T/Help <request> &Sfor more help on a request.");
        }

        public static bool PrintCategoryRequests(Player p, string sort, string modifier, string type)
        {
            List<Request> req = new List<Request>();
            bool foundAny = false;

            // common shortcuts people tend to use
            type = MapCategory(type);

            foreach (Request r in allReqs)
            {
                string category = MapCategory(r.type);
                if (!type.CaselessEq(category)) continue;

                if (p.CanUse(r)) req.Add(r);
                foundAny = true;
            }
            if (!foundAny) return false;

            if (req.Count == 0)
            {
                p.Message("You cannot use any of the {0} requests.", type.Capitalize());
                return true;
            }
            SortRequests(req, sort);
            p.Message(type.Capitalize() + " requests you may use:");

            type = "Requests " + type;
            if (sort.Length > 0) type += " " + sort;
            Paginator.Output(p, req, GetColoredName,
                             type, "requests", modifier);

            p.Message("Type &T/Help <request> &Sfor more help on a request.");
            return true;
        }

        public static void SortRequests(List<Request> reqs, string sort)
        {
            if (sort == "name" || sort == "names")
            {
                reqs.Sort((a, b) => a.RequestName
                          .CompareTo(b.RequestName));
            }
            if (sort == "rank" || sort == "ranks")
            {
                reqs.Sort((a, b) => a.Permissions.MinRank
                          .CompareTo(b.Permissions.MinRank));
            }
        }

        public static string MapCategory(string type)
        {
            type = type.ToLower();
            // convert old category/type names
            if (type == "add") return CommandTypes.Added;
            if (type == "build") return CommandTypes.Building;
            if (type == "chat") return CommandTypes.Chat;
            if (type == "economy") return CommandTypes.Economy;
            if (type == "game") return CommandTypes.Games;
            if (type == "mod") return CommandTypes.Moderation;
            if (type == "other") return CommandTypes.Other;
            if (type == "world") return CommandTypes.World;
            if (type == "information") return CommandTypes.Information;
            if (type == "add") return OrderTypes.Order;
            if (type == "request") return RequestTypes.Request;
            return type;
        }

        public static string GetCategories()
        {
            Dictionary<string, bool> categories = new Dictionary<string, bool>();
            foreach (Request req in allReqs)
            {
                categories[MapCategory(req.type)] = true;
            }

            List<string> list = new List<string>(categories.Keys);
            list.Sort();
            return list.Join(" ");
        }

        public override void Help(Player p)
        {
            p.Message("&T/Requests [category] <sort by>");
            p.Message("  &HIf no category is given, outputs all requests you can use.");
            p.Message("  &HIf category is \"shortcuts\", outputs all request shortcuts.");
            p.Message("  &HIf category is \"all\", outputs all requests.");
            p.Message("  &HIf category is a rank name, outputs what that rank can use.");
            p.Message("&HOther request categories:");
            p.Message("  &H{0}", GetCategories());
            p.Message("&HSort By is optional, and can be either \"name\" or \"rank\"");
        }
    }
    public class ReqSearch : Request
    {
        public static string ReqName = "ReqSearch";
        public override string RequestName
        {
            get
            {
                return ReqName;
            }
            set
            {
                ReqName = value;
            }
        }
        public override string RequestType { get { return RequestTypes.Request; } }
        public override LevelPermission RequestDefaultRank { get { return LevelPermission.Builder; } }
        public override bool RequestUseableWhenFrozen { get { return true; } }
        public override void ExecuteRequest(Player p, string message)
        {
            string[] args = message.SplitSpaces(3);
            if (args.Length < 2)
            {
                Help(p);
                return;
            }
            string list = args[0].ToLower();
            string keyword = args[1];
            string modifier = args.Length > 2 ? args[2] : "";
            if (list == "block" || list == "blocks")
            {
                SearchBlocks(p, keyword, modifier);
            }
            else if (list == "rank" || list == "ranks")
            {
                SearchRanks(p, keyword, modifier);
            }
            else if (list == "command" || list == "commands")
            {
                SearchCommands(p, keyword, modifier);
            }
            else if (list == "order" || list == "orders")
            {
                SearchOrders(p, keyword, modifier);
            }
            else if (list == "request" || list == "requests")
            {
                SearchRequests(p, keyword, modifier);
            }
            else if (list == "player" || list == "players")
            {
                SearchPlayers(p, keyword, modifier);
            }
            else if (list == "online")
            {
                SearchOnline(p, keyword, modifier);
            }
            else if (list == "loaded")
            {
                SearchLoaded(p, keyword, modifier);
            }
            else if (list == "level" || list == "levels" || list == "maps")
            {
                SearchMaps(p, keyword, modifier);
            }
            else
            {
                Help(p);
            }
        }


        public static void SearchBlocks(Player p, string keyword, string modifier)
        {
            List<ushort> blocks = new List<ushort>();
            for (int b = 0; b < Block.SUPPORTED_COUNT; b++)
            {
                ushort block = (ushort)b;
                if (Block.ExistsFor(p, block)) blocks.Add(block);
            }

            List<string> blockNames = Wildcard.Filter(blocks, keyword,
                                                      b => Block.GetName(p, b), null,
                                                      b => Block.GetColoredName(p, b));
            OutputList(p, keyword, "search blocks", "blocks", modifier, blockNames);
        }
        public static void SearchCommands(Player p, string keyword, string modifier)
        {
            List<string> commands = Wildcard.Filter(allCmds, keyword, cmd => cmd.name,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(allCmds, keyword, cmd => cmd.shortcut,
                                                     cmd => !string.IsNullOrEmpty(cmd.shortcut),
                                                     GetColoredName);

            // Match both names and shortcuts
            foreach (string shortcutCmd in shortcuts)
            {
                if (commands.CaselessContains(shortcutCmd)) continue;
                commands.Add(shortcutCmd);
            }

            OutputList(p, keyword, "search commands", "commands", modifier, commands);
        }
        public static void SearchOrders(Player p, string keyword, string modifier)
        {
            List<string> orders = Wildcard.Filter(Order.allOrds, keyword, ord => ord.Name,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(Order.allOrds, keyword, ord => ord.Shortcut,
                                                     ord => !string.IsNullOrEmpty(ord.Shortcut),
                                                     GetColoredName);
            foreach (string shortcutOrd in shortcuts)
            {
                if (orders.CaselessContains(shortcutOrd))
                {
                    continue;
                }
                orders.Add(shortcutOrd);
            }

            OutputList(p, keyword, "search orders", "orders", modifier, orders);
        }
        public static void SearchRequests(Player p, string keyword, string modifier)
        {
            List<string> requests = Wildcard.Filter(allReqs, keyword, req => req.RequestName,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(allReqs, keyword, req => req.RequestShortcut,
                                                     req => !string.IsNullOrEmpty(req.RequestShortcut),
                                                     GetColoredName);
            foreach (string shortcutReq in shortcuts)
            {
                if (requests.CaselessContains(shortcutReq))
                {
                    continue;
                }
                requests.Add(shortcutReq);
            }

            OutputList(p, keyword, "search requests", "requests", modifier, requests);
        }

        public static void SearchRanks(Player p, string keyword, string modifier)
        {
            List<string> ranks = Wildcard.Filter(Group.GroupList, keyword, grp => grp.Name,
                                                null, grp => grp.ColoredName);
            OutputList(p, keyword, "search ranks", "ranks", modifier, ranks);
        }

        public static void SearchOnline(Player p, string keyword, string modifier)
        {
            Player[] online = PlayerInfo.Online.Items;
            List<string> players = Wildcard.Filter(online, keyword, pl => pl.name,
                                                  pl => p.CanSee(pl), pl => pl.ColoredName);
            OutputList(p, keyword, "search online", "players", modifier, players);
        }

        public static void SearchLoaded(Player p, string keyword, string modifier)
        {
            Level[] loaded = LevelInfo.Loaded.Items;
            List<string> levels = Wildcard.Filter(loaded, keyword, level => level.name);
            OutputList(p, keyword, "search loaded", "loaded levels", modifier, levels);
        }

        public static void SearchMaps(Player p, string keyword, string modifier)
        {
            string[] allMaps = LevelInfo.AllMapNames();
            List<string> maps = Wildcard.Filter(allMaps, keyword, map => map);
            OutputList(p, keyword, "search levels", "maps", modifier, maps);
        }

        public static void OutputList(Player p, string keyword, string cmd, string type, string modifier, List<string> items)
        {
            if (items.Count == 0)
            {
                p.Message("No {0} found containing \"{1}\"", type, keyword);
            }
            else
            {
                Paginator.Output(p, items, item => item, cmd + " " + keyword, type, modifier);
            }
        }


        public static void SearchPlayers(Player p, string keyword, string modifier)
        {
            List<string> names = new List<string>();
            string suffix = Database.Backend.CaselessLikeSuffix;

            // TODO supporting more than 100 matches somehow
            Database.ReadRows("Players", "Name", r => names.Add(r.GetText(0)),
                              "WHERE Name LIKE @0 ESCAPE '#' LIMIT 100" + suffix,
                              Wildcard.ToSQLFilter(keyword));

            OutputList(p, keyword, "search players", "players", modifier, names);
        }


        public override void Help(Player p)
        {
            p.Message("&T/Search [list] [keyword]");
            p.Message("&HFinds entries in a list that match the given keyword");
            p.Message("&H  keyword can also include wildcard characters:");
            p.Message("&H    * - placeholder for zero or more characters");
            p.Message("&H    ? - placeholder for exactly one character");
            p.Message("&HLists: &fblocks/commands/orders/requests/ranks/players/online/loaded/maps");
        }
    }
    public static class AddedFormatter
    {
        public static void PrintRequestInfo(Player p, Request req)
        {
            p.Message("Usable by: " + req.Permissions.Describe());
            PrintAliases(p, req);
            List<CommandExtraPerms> extraPerms = CommandExtraPerms.FindAll(req.RequestName);
            if (req.ExtraPerms == null)
            {
                extraPerms.Clear();
            }
            if (extraPerms.Count == 0)
            {
                return;
            }
            p.Message("&TExtra permissions:");
            foreach (CommandExtraPerms extra in extraPerms)
            {
                p.Message("{0}) {1} {2}", extra.Num, extra.Describe(), extra.Desc);
            }
        }
        public static void PrintAliases(Player p, Request req)
        {
            StringBuilder dst = new StringBuilder("Shortcuts: &T");
            if (!string.IsNullOrEmpty(req.RequestShortcut))
            {
                dst.Append('/').Append(req.RequestShortcut).Append(", ");
            }
            FindAliases(Alias.aliases, req, dst);
            if (dst.Length == "Shortcuts: &T".Length)
            {
                return;
            }
            p.Message(dst.ToString(0, dst.Length - 2));
        }
        public static void FindAliases(List<Alias> aliases, Command req, StringBuilder dst)
        {
            foreach (Alias a in aliases)
            {
                if (!a.Target.CaselessEq(req.name))
                {
                    continue;
                }
                dst.Append('/').Append(a.Trigger);
                if (a.Format == null)
                {
                    dst.Append(", ");
                    continue;
                }
                string name = string.IsNullOrEmpty(req.shortcut) ? req.name : req.shortcut;
                if (name.Length > req.name.Length)
                {
                    name = req.name;
                }
                string args = a.Format.Replace("{args}", "[args]");
                dst.Append(" for /").Append(name + " " + args);
                dst.Append(", ");
            }
        }
    }
    public partial class Request : Command
    {
        public override bool SuperUseable { get { return RequestSuperUseable; } }
        public override bool MessageBlockRestricted { get { return RequestMessageBlockRestricted; } }
        public override bool UseableWhenFrozen { get { return RequestUseableWhenFrozen; } }
        public override bool LogUsage { get { return RequestLogUsage; } }
        public override bool UpdatesLastCmd { get { return UpdatesLastReq; } }
        public override string name { get { return RequestName; } }
        public virtual string RequestName { get; set; }
        public override string shortcut { get { return RequestShortcut; } }
        public virtual string RequestShortcut { get { return ""; } }
        public override string type { get { return RequestType; } }
        public virtual string RequestType { get { return ""; } }
        public override bool museumUsable { get { return RequestMuseumUsable; } }
        public virtual bool RequestMuseumUsable { get { return true; } }
        public override bool ShowCommandInfo { get { return ShowRequestInfo; } }
        public virtual bool ShowRequestInfo { get { return true; } }
        public override LevelPermission defaultRank { get { return RequestDefaultRank; } }
        public virtual LevelPermission RequestDefaultRank { get { return LevelPermission.Guest; } }
        public override void Use(Player p, string message, CommandData data)
        {
            ExecuteRequest(p, message, data);
        }
        public override void Use(Player p, string message)
        {
            ExecuteRequest(p, message);
        }
        public virtual void ExecuteRequest(Player p, string message)
        {
        }
        public virtual void ExecuteRequest(Player p, string message, CommandData data)
        {
            ExecuteRequest(p, message);
        }
        public override void Help(Player p)
        {
            p.Message("No help is available for this request.");
        }
        public virtual void Help(Player p, string message)
        {
            Help(p);
            AddedFormatter.PrintRequestInfo(p, this);
        }
        public static List<Request> CopyAll()
        {
            return new List<Request>(allReqs);
        }
        public virtual bool RequestSuperUseable { get { return true; } }
        public virtual bool RequestMessageBlockRestricted { get { return RequestType.CaselessContains("mod"); } }
        public virtual bool RequestUseableWhenFrozen { get { return false; } }
        public virtual bool RequestLogUsage { get { return true; } }
        public virtual bool UpdatesLastReq { get { return true; } }
        public override CommandParallelism Parallelism
        {
            get
            {
                return base.Parallelism;
            }
        }
        public static List<Request> allReqs = new List<Request>();
        public static void InitAll()
        {
            Order.InitAll();
            allReqs.Clear();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typeof(Request)) || type.IsAbstract || !type.IsPublic)
                {
                    continue;
                }
                Request req = (Request)Activator.CreateInstance(type);
                RegisterREQ(req);
            }
            ExpIScripting.AutoloadRequests();
        }
        public static void RegisterREQ(Request req)
        {
            Register(req);
            allReqs.Add(req);
            req.Permissions = CommandPerms.GetOrAdd(req.RequestName, req.RequestDefaultRank);
            CommandPerm[] extra = req.ExtraPerms;
            if (extra != null)
            {
                for (int i = 0; i < extra.Length; i++)
                {
                    CommandExtraPerms exPerms = CommandExtraPerms.GetOrAdd(req.RequestName, i + 1, extra[i].Perm);
                    exPerms.Desc = extra[i].Description;
                }
            }
            Alias.RegisterDefaults(req);
            allCmds.Add(req);
        }
        public static void RegisterCMD(params Command[] commands)
        {
            foreach (Command command in commands)
            {
                Register(command);
            }
        }
        public static bool Unregister(Request req)
        {
            if (req != null)
            {
                Alias.UnregisterDefaults(req);
            }
            return Command.Unregister(req);
        }
        public static void UnregisterRequests()
        {
            foreach (Request req in allReqs)
            {
                Unregister(req);
            }
        }
        public static Request FindREQ(string name)
        {
            foreach (Request req in allReqs)
            {
                if (req.RequestName.CaselessEq(name))
                {
                    return req;
                }
            }
            return FindORD(name);
        }
        public static Order FindORD(string name)
        {
            foreach (Order ord in Order.allOrds)
            {
                if (ord.Name.CaselessEq(name))
                {
                    return ord;
                }
            }
            return null;
        }
        public static Command FindCMD(string name)
        {
            foreach (Command cmd in allCmds)
            {
                if (cmd.name.CaselessEq(name))
                {
                    return cmd;
                }
            }
            return FindREQ(name);
        }
        public static void Search(ref string reqName, ref string reqArgs)
        {
            if (reqName.Length == 0)
            {
                return;
            }
            Alias alias = Alias.Find(reqName);
            if (alias == null)
            {
                foreach (Request req in allReqs)
                {
                    if (!req.RequestShortcut.CaselessEq(reqName))
                    {
                        continue;
                    }
                    reqName = req.RequestName;
                    return;
                }
                return;
            }
            reqName = alias.Target;
            string format = alias.Format;
            if (format == null)
            {
                return;
            }
            if (format.Contains("{args}"))
            {
                reqArgs = format.Replace("{args}", reqArgs);
            }
            else
            {
                reqArgs = format + " " + reqArgs;
            }
            reqArgs = reqArgs.Trim();
        }
    }
    public class ReqRequestCreate : ReqCompile
    {
        public override string RequestName
        {
            get
            {
                return "RequestCreate";
            }
        }
        public override string RequestShortcut
        {
            get
            {
                return "reqcreate";
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ECreate", "experiment")
                };
            }
        }
        public override void CompileRequest(Player p, string[] paths, ICompiler compiler)
        {
            if (compiler == null)
            {
                compiler = new CSCompiler();
            }
            foreach (string req in paths)
            {
                CompilerOperations.CreateRequest(p, req, compiler);
            }
        }
        public override void CompileExperiment(Player p, string[] paths, ICompiler compiler)
        {
            foreach (string req in paths)
            {
                CompilerOperations.CreateExperiment(p, req, compiler);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/RequestCreate [name]");
            p.Message("&HCreates an example C# request named [name]");
            p.Message("&H  This can be used as the basis for creating a new request");
            p.Message("&T/RequestCreate experiment [name]");
            p.Message("&HCreate an example C# experiment named [name]");
        }
    }
    public class ReqCompile : Request
    {
        public static string ReqName = "RequestCompile";
        public override string RequestName
        {
            get
            {
                return ReqName;
            }
            set
            {
                ReqName = value;
            }
        }
        public override string RequestShortcut
        {
            get
            {
                return "reqcompile";
            }
        }
        public override string RequestType
        {
            get
            {
                return RequestTypes.Request;
            }
        }
        public override LevelPermission RequestDefaultRank
        {
            get
            {
                return LevelPermission.Owner;
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ECompile", "experiment")
                };
            }
        }
        public override bool MessageBlockRestricted
        {
            get
            {
                return true;
            }
        }
        public override void Use(Player p, string message)
        {
            string[] args = message.SplitSpaces();
            bool experiment = args[0].CaselessEq("experiment");
            string name, lang;
            if (experiment)
            {
                name = args.Length > 1 ? args[1] : "";
                lang = args.Length > 2 ? args[2] : "";
            }
            else
            {
                name = args[0];
                lang = args.Length > 1 ? args[1] : "";
            }
            if (name.Length == 0)
            {
                Help(p);
                return;
            }
            if (!Formatter.ValidFilename(p, name))
            {
                return;
            }
            ICompiler compiler = CompilerOperations.GetCompiler(p, lang);
            if (compiler == null)
            {
                return;
            }
            string[] paths = name.SplitComma();
            if (experiment)
            {
                CompileExperiment(p, paths, compiler);
            }
            else
            {
                CompileRequest(p, paths, compiler);
            }
        }
        public virtual void CompileExperiment(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = ExpIScripting.ExperimentPath(paths[0]);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.ExperimentPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "Experiment", paths, dstPath);
        }
        public virtual void CompileRequest(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = ExpIScripting.RequestPath(paths[0]);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.RequestPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "Request", paths, dstPath);
        }
        public override void Help(Player p)
        {
            ICompiler compiler = ICompiler.Compilers[0];
            p.Message("&T/RequestCompile [request name]");
            p.Message("&HCompiles a .cs file containing a C# request into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.RequestPath("&H<name>&f"));
            p.Message("&T/RequestCompile experiment [experiment name]");
            p.Message("&HCompiles a .cs file containing a C# experiment into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.ExperimentPath("&H<name>&f"));
        }
    }
    public class ReqCompLoad : ReqCompile
    {
        public override string RequestName
        {
            get
            {
                return "RequestCompLoad";
            }
        }
        public override string RequestShortcut
        {
            get
            {
                return "reqcml";
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ReqCompLoad"),
                    new CommandAlias("Requestcml")
                };
            }
        }
        public override void CompileExperiment(Player p, string[] paths, ICompiler compiler)
        {
            string dst = ExpIScripting.ExperimentPath(paths[0]);
            UnloadExperiment(p, paths[0]);
            base.CompileExperiment(p, paths, compiler);
            ExpScriptingOperations.LoadExperiments(p, dst);
        }
        public static void UnloadExperiment(Player p, string name)
        {
            Experiment experiment = Experiment.FindExperiment(name);
            if (experiment == null)
            {
                return;
            }
            ExpScriptingOperations.UnloadExperiment(p, experiment);
        }
        public override void CompileRequest(Player p, string[] paths, ICompiler compiler)
        {
            string req = paths[0];
            string dst = ExpIScripting.RequestPath(req);
            UnloadRequest(p, req);
            base.CompileRequest(p, paths, compiler);
            ExpScriptingOperations.LoadRequests(p, dst);
        }
        public static void UnloadRequest(Player p, string reqName)
        {
            string ordArgs = "";
            Search(ref reqName, ref ordArgs);
            Request req = FindREQ(reqName);
            if (req == null)
            {
                return;
            }
            ExpScriptingOperations.UnloadRequest(p, req);
        }
        public override void Help(Player p)
        {
            p.Message("&T/RequestCompLoad [request]");
            p.Message("&HCompiles and loads (or reloads) a C# request into the server");
            p.Message("&T/RequestCompLoad experiment [experiment]");
            p.Message("&HCompiles and loads (or reloads) a C# experiment into the server");
        }
    }
    public abstract class ICompiler
    {
        public const string REQUESTS_SOURCE_DIR = "requests/";
        public const string EXPERIMENTS_SOURCE_DIR = "experiments/";
        public const string ERROR_LOG_PATH = "logs/errors/addedcompiler.log";
        public virtual string FileExtension
        {
            get
            {
                return ".cs";
            }
        }
        public virtual string ShortName { get { return "C#"; } }
        public virtual string FullName { get { return "CSharp"; } }
        public virtual string RequestSkeleton
        {
            get
            {
                return @"//\tAuto-generated request skeleton class
//\tUse this as a basis for custom Flames requests
//\tNaming should be kept consistent (e.g. /update request should have a class name of 'ReqUpdate' and a filename of 'ReqUpdate.cs')
// As a note, Flames is designed for .NET 4.8

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;
using Flames.Added.Experiments;
using Flames;

public class Req{0} : Request
{{
\t// The request's name (what you put after a slash to use this request)
\tpublic override string requestName {{ get {{ return ""{0}""; }} }}
\t// Request's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""o"")
\tpublic override string requestShortcut {{ get {{ return """"; }} }}
\t// Which submenu this request displays in under /Help
\tpublic override string requestType {{ get {{ return ""other""; }} }}
\t// Whether or not this request can be used in a museum. Block/map altering requests should return false to avoid errors.
\tpublic override bool requestMuseumUsable {{ get {{ return true; }} }}
\t// The default rank required to use this request. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tpublic override LevelPermission requestDefaultRank {{ get {{ return LevelPermission.Guest; }} }}
\t// This is for when a player executes this order by doing /{0}
\t//   p is the player object for the player executing the request. 
\t//   message is the arguments given to the request. (e.g. for '/{0} this', message is ""this"")
\tpublic override void ExecuteRequest(Player p, string message)
\t{{
\t\tp.Message(""Hello World!"");
\t}}
\t// This is for when a player does /Help {0}
\tpublic override void Help(Player p)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example request."");
\t}}
}}";
            }
        }
        public virtual string ExperimentSkeleton
        {
            get
            {
                return @"//\tAuto-generated experiment skeleton class
//\tUse this as a basis for custom Flames experiments
// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""
// Add any other using statements you need after this
using System;
using Flames;
namespace Flames.Added.Experiments
{{
\tpublic class {0} : Experiment
\t{{
\t\t// The experiment's name (i.e what shows in /Experiments)
\t\tpublic override string ExperimentName {{ get {{ return ""{0}""; }} }}
\t\t// The oldest version of Flames this experiment is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}
\t\t// Message displayed in server logs when this experiment is loaded
\t\tpublic override string Welcome {{ get {{ return ""Loaded Message!""; }} }}
\t\t// Who created/authored this experiment
\t\tpublic override string Creator {{ get {{ return ""{1}""; }} }}
\t\t// Called when this experiment is being loaded (e.g. on server startup)
\t\tpublic override void LoadExperiment()
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}
\t\t// Called when this experiment is being unloaded (e.g. on server shutdown)
\t\tpublic override void UnloadExperiment()
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}
\t\t// Displays help for or information about this experiment
\t\tpublic override void HelpExperiment(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this experiment."");
\t\t}}
\t}}
}}";
            }
        }
        public string RequestPath(string name)
        {
            return REQUESTS_SOURCE_DIR + "Req" + name + FileExtension;
        }
        public string ExperimentPath(string name)
        {
            return EXPERIMENTS_SOURCE_DIR + name + FileExtension;
        }

        public static List<ICompiler> Compilers = new List<ICompiler>()
        {
            new CSCompiler()
        };
        public static string FormatSource(string source, params string[] args)
        {
            source = source.Replace(@"\t", "\t");
            source = source.Replace("\n", "\r\n");
            return string.Format(source, args);
        }
        public string GenExampleRequest(string reqName)
        {
            reqName = reqName.ToLower().Capitalize();
            return FormatSource(RequestSkeleton, reqName);
        }
        public string GenExampleExperiment(string experiment, string creator)
        {
            return FormatSource(ExperimentSkeleton, experiment, creator, Server.Version);
        }
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, bool logErrors)
        {
            ICompilerErrors errors = DoCompile(srcPaths, dstPath);
            if (!errors.HasErrors || !logErrors)
            {
                return errors;
            }
            SourceMap sources = new SourceMap(srcPaths);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("############################################################");
            sb.AppendLine("Errors when compiling " + srcPaths.Join());
            sb.AppendLine("############################################################");
            sb.AppendLine();
            foreach (ICompilerError err in errors)
            {
                string type = err.IsWarning ? "Warning" : "Error";
                sb.AppendLine(DescribeError(err, srcPaths, "") + ":");
                if (err.Line > 0)
                {
                    sb.AppendLine(sources.Get(err.FileName, err.Line - 1));
                }
                if (err.Column > 0)
                {
                    sb.Append(' ', err.Column - 1);
                }
                sb.AppendLine("^-- " + type + " #" + err.ErrorNumber + " - " + err.ErrorText);
                sb.AppendLine();
                sb.AppendLine("-------------------------");
                sb.AppendLine();
            }
            using (StreamWriter w = new StreamWriter(ERROR_LOG_PATH, true))
            {
                w.Write(sb.ToString());
            }
            return errors;
        }
        public static string DescribeError(ICompilerError err, string[] srcs, string text)
        {
            string type = err.IsWarning ? "Warning" : "Error";
            string file = Path.GetFileName(err.FileName);
            return string.Format("{0}{1}{2}{3}", type, text,
                                 err.Line > 0 ? " on line " + err.Line : "",
                                 srcs.Length > 1 ? " in " + file : "");
        }
        public abstract ICompilerErrors DoCompile(string[] srcPaths, string dstPath);
        public static List<string> ProcessInput(string[] srcPaths, string commentPrefix)
        {
            List<string> referenced = new List<string>();
            for (int i = 0; i < srcPaths.Length; i++)
            {
                string path = Path.GetFullPath(srcPaths[i]);
                AddReferences(path, commentPrefix, referenced);
                srcPaths[i] = path;
            }
            referenced.Add(Server.GetServerDLLPath());
            referenced.Add(Path.GetFullPath("plugins/Added.dll"));
            return referenced;
        }
        public static void AddReferences(string path, string commentPrefix, List<string> referenced)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string refPrefix = commentPrefix + "reference ";
                string expPrefix = commentPrefix + "expref ";
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    if (line.CaselessStarts(refPrefix))
                    {
                        referenced.Add(GetDLL(line));
                    }
                    else if (line.CaselessStarts(expPrefix))
                    {
                        path = Path.Combine(ExpIScripting.EXPERIMENTS_DLL_DIR, GetDLL(line));
                        referenced.Add(Path.GetFullPath(path));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        public static string GetDLL(string line)
        {
            int index = line.IndexOf(' ') + 1;
            return line.Substring(index).Replace(";", "");
        }
    }
    public class ICompilerErrors : List<ICompilerError>
    {
        public bool HasErrors
        {
            get
            {
                return FindIndex(ce => !ce.IsWarning) >= 0;
            }
        }
    }
    public class ICompilerError
    {
        public int Line, Column;
        public string ErrorNumber, ErrorText, FileName;
        public bool IsWarning;
    }
    public class SourceMap
    {
        public string[] files;
        public List<string>[] sources;
        public SourceMap(string[] paths)
        {
            files = paths;
            sources = new List<string>[paths.Length];
        }
        public int FindFile(string file)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (file.CaselessEq(files[i]))
                {
                    return i;
                }
            }
            return -1;
        }
        public string Get(string file, int line)
        {
            int i = FindFile(file);
            if (i == -1)
            {
                return "";
            }
            List<string> source = sources[i];
            if (source == null)
            {
                try
                {
                    source = Utils.ReadAllLinesList(file);
                }
                catch
                {
                    source = new List<string>();
                }
                sources[i] = source;
            }
            return line < source.Count ? source[line] : "";
        }
    }
    public class CompilerExperiment : Experiment
    {
        public static string ExpName = "ExperimentCompiler";
        public override string ExperimentName
        {
            get
            {
                return ExpName;
            }
            set
            {
                ExpName = value;
            }
        }
        public override void LoadExperiment()
        {
            ExpIScripting.Init();
            Server.EnsureDirectoryExists(ICompiler.REQUESTS_SOURCE_DIR);
            Server.EnsureDirectoryExists(ICompiler.EXPERIMENTS_SOURCE_DIR);
        }
        public override void UnloadExperiment()
        {
            Request.UnregisterRequests();
        }
    }
    public static class ICodeDomCompiler
    {
        public static CompilerParameters PrepareInput(string[] srcPaths, string dstPath, string commentPrefix)
        {
            CompilerParameters args = new CompilerParameters
            {
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                OutputAssembly = dstPath
            };
            List<string> referenced = ICompiler.ProcessInput(srcPaths, commentPrefix);
            foreach (string assembly in referenced)
            {
                args.ReferencedAssemblies.Add(assembly);
            }
            return args;
        }
        public static void InitCompiler(ICompiler c, string language, ref CodeDomProvider compiler)
        {
            if (compiler != null)
            {
                return;
            }
            compiler = CodeDomProvider.CreateProvider(language);
            if (compiler != null)
            {
                return;
            }
            Logger.Log(LogType.Warning,
                       "WARNING: {0} compiler is missing, you will be unable to compile {1} files.",
                       c.FullName, c.FileExtension);
        }
        public static ICompilerErrors Compile(CompilerParameters args, string[] srcPaths, CodeDomProvider compiler)
        {
            CompilerResults results = compiler.CompileAssemblyFromFile(args, srcPaths);
            ICompilerErrors errors = new ICompilerErrors();
            foreach (CompilerError error in results.Errors)
            {
                ICompilerError ce = new ICompilerError
                {
                    Line = error.Line,
                    Column = error.Column,
                    ErrorNumber = error.ErrorNumber,
                    ErrorText = error.ErrorText,
                    IsWarning = error.IsWarning,
                    FileName = error.FileName
                };
                errors.Add(ce);
            }
            return errors;
        }
    }
    public class CSCompiler : ICompiler
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "C#"; } }
        public override string FullName { get { return "CSharp"; } }
        public CodeDomProvider compiler;
        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
        {
            CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");
            args.CompilerOptions += " /unsafe";
            ICodeDomCompiler.InitCompiler(this, "CSharp", ref compiler);
            return ICodeDomCompiler.Compile(args, srcPaths, compiler);
        }
        public override string RequestSkeleton
        {
            get
            {
                return @"//\tAuto-generated request skeleton class
//\tUse this as a basis for custom Flames requests
//\tNaming should be kept consistent (e.g. /update request should have a class name of 'ReqUpdate' and a filename of 'ReqUpdate.cs')
// As a note, Flames is designed for .NET 4.8
// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""
// Add any other using statements you need after this
using System;
using Flames;
using Flames.Added.Experiments;
public class Req{0} : Request
{{
\t// The request's name (what you put after a slash to use this request)
\tpublic override string RequestName {{ get {{ return ""{0}""; }} }}
\t// Request's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""o"")
\tpublic override string RequestShortcut {{ get {{ return """"; }} }}
\t// Which submenu this request displays in under /Help
\tpublic override string RequestType {{ get {{ return ""other""; }} }}
\t// Whether or not this request can be used in a museum. Block/map altering requests should return false to avoid errors.
\tpublic override bool RequestMuseumUsable {{ get {{ return true; }} }}
\t// The default rank required to use this request. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tpublic override LevelPermission RequestDefaultRank {{ get {{ return LevelPermission.Guest; }} }}
\t// This is for when a player executes this request by doing /{0}
\t//   p is the player object for the player executing the request. 
\t//   message is the arguments given to the request. (e.g. for '/{0} this', message is ""this"")
\tpublic override void ExecuteRequest(Player p, string message)
\t{{
\t\tp.Message(""Hello World!"");
\t}}
\t// This is for when a player does /Help {0}
\tpublic override void Help(Player p)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example request."");
\t}}
}}";
            }
        }
        public override string ExperimentSkeleton
        {
            get
            {
                return @"//\tAuto-generated experiment skeleton class
//\tUse this as a basis for custom Flames experiments
// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""
// Add any other using statements you need after this
using System;
using Flames;
namespace Flames.Added.Experiments
{{
\tpublic class {0} : Experiment
\t{{
\t\t// The experiment's name (i.e what shows in /Experiments)
\t\tpublic override string ExperimentName {{ get {{ return ""{0}""; }} }}
\t\t// The oldest version of Flames this experiment is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}
\t\t// Message displayed in server logs when this experiment is loaded
\t\tpublic override string Welcome {{ get {{ return ""Loaded Message!""; }} }}
\t\t// Who created/authored this experiment
\t\tpublic override string Creator {{ get {{ return ""{1}""; }} }}
\t\t// Called when this experiment is being loaded (e.g. on server startup)
\t\tpublic override void LoadExperiment()
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}
\t\t// Called when this experiment is being unloaded (e.g. on server shutdown)
\t\tpublic override void UnloadExperiment()
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}
\t\t// Displays help for or information about this experiment
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this experiment."");
\t\t}}
\t}}
}}";
            }
        }
    }
    public static class CompilerOperations
    {
        public static ICompiler GetCompiler(Player p, string name)
        {
            if (name.Length == 0)
            {
                return ICompiler.Compilers[0];
            }
            foreach (ICompiler comp in ICompiler.Compilers)
            {
                if (comp.ShortName.CaselessEq(name))
                {
                    return comp;
                }
            }
            p.Message("&WUnknown language \"{0}\"", name);
            p.Message("&HAvailable languages: &f{0}",
                      ICompiler.Compilers.Join(c => c.ShortName + " (" + c.FullName + ")"));
            return null;
        }
        public static bool CreateRequest(Player p, string name, ICompiler compiler)
        {
            string path = compiler.RequestPath(name);
            string source = compiler.GenExampleRequest(name);
            return CreateFile(p, name, path, "request &fOrd", source);
        }
        public static bool CreateExperiment(Player p, string name, ICompiler compiler)
        {
            string path = compiler.ExperimentPath(name);
            string creator = p.IsSuper ? Server.Config.Name : p.truename;
            string source = compiler.GenExampleExperiment(name, creator);
            return CreateFile(p, name, path, "experiment &f", source);
        }
        public static bool CreateFile(Player p, string name, string path, string type, string source)
        {
            if (File.Exists(path))
            {
                p.Message("File {0} already exists. Choose another name.", path);
                return false;
            }
            File.WriteAllText(path, source);
            p.Message("Successfully saved example {2}{0} &Sto {1}", name, path, type);
            return true;
        }
        public static bool Compile(Player p, ICompiler compiler, string type, string[] srcs, string dst)
        {
            foreach (string path in srcs)
            {
                if (File.Exists(path))
                {
                    continue;
                }
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }
            ICompilerErrors errors = compiler.Compile(srcs, dst, true);
            if (!errors.HasErrors)
            {
                p.Message("{0} compiled successfully from {1}",
                        type, srcs.Join(file => Path.GetFileName(file)));
                return true;
            }
            SummariseErrors(errors, srcs, p);
            return false;
        }
        public const int MAX_LOG = 5;
        public static void SummariseErrors(ICompilerErrors errors, string[] srcs, Player p)
        {
            int logged = 0;
            foreach (ICompilerError err in errors)
            {
                p.Message("&W{1} - {0}", err.ErrorText,
                          ICompiler.DescribeError(err, srcs, " #" + err.ErrorNumber));
                logged++;
                if (logged >= MAX_LOG)
                {
                    break;
                }
            }
            if (logged < errors.Count)
            {
                p.Message(" &W.. and {0} more", errors.Count - logged);
            }
            p.Message("&WCompiling failed. See " + ICompiler.ERROR_LOG_PATH + " for more detail");
        }
    }
    public class ExperimentLoader : Addon
    {
        public override string Name { get { return "ExperimentLoader"; } }
        public override string Creator { get { return Colors.Strip(Server.SoftwareName + " team"); } }
        public static void LoadAllExperiments(SchedulerTask task)
        {
            Experiment.LoadAll();
        }
        public override void Load()
        {
            OnShuttingDownEvent.Register(OnShutdown, Priority.Critical);
            Server.Critical.QueueOnce(LoadAllExperiments);
        }
        public void OnShutdown(bool restarting, string message)
        {
            Experiment.UnloadAll();
        }
        public override void Unload()
        {
            OnShuttingDownEvent.Unregister(OnShutdown);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
    public class AlreadyLoadedException : Exception
        {
            public AlreadyLoadedException(string msg) : base(msg)
            {
            }
        }
        public static class ExpIScripting
        {
            public const string REQUESTS_DLL_DIR = "requests/";
            public const string EXPERIMENTS_DLL_DIR = "experiments/";
            public static string RequestPath(string name)
            {
                return REQUESTS_DLL_DIR + "Req" + name + ".dll";
            }
            public static string ExperimentPath(string name)
            {
                return EXPERIMENTS_DLL_DIR + name + ".dll";
            }
            public static void Init()
            {
                Request.InitAll();
                Directory.CreateDirectory(REQUESTS_DLL_DIR);
                Directory.CreateDirectory(EXPERIMENTS_DLL_DIR);
                AppDomain.CurrentDomain.AssemblyResolve += ResolveExperimentAssembly;
            }
            public static Assembly ResolveExperimentAssembly(object sender, ResolveEventArgs args)
            {
                if (args.RequestingAssembly == null)
                {
                    return null;
                }
                if (!IsExperimentDLL(args.RequestingAssembly))
                {
                    return null;
                }
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assem in assemblies)
                {
                    if (!IsExperimentDLL(assem))
                    {
                        continue;
                    }
                    if (args.Name == assem.FullName)
                    {
                        return assem;
                    }
                }
                Logger.Log(LogType.Warning, "Custom request/experiment [{0}] tried to load [{1}], but it could not be found",
                           args.RequestingAssembly.FullName, args.Name);
                return null;
            }
            public static bool IsExperimentDLL(Assembly a)
            {
                return string.IsNullOrEmpty(a.Location);
            }
            public static List<T> LoadTypes<T>(Assembly lib)
            {
                List<T> instances = new List<T>();
                foreach (Type t in lib.GetTypes())
                {
                    if (t.IsAbstract || t.IsInterface || !t.IsSubclassOf(typeof(T)))
                    {
                        continue;
                    }
                    object instance = Activator.CreateInstance(t);
                    if (instance == null)
                    {
                        Logger.Log(LogType.Warning, "{0} \"{1}\" could not be loaded", typeof(T).Name, t.Name);
                        throw new BadImageFormatException();
                    }
                    instances.Add((T)instance);
                }
                return instances;
            }
            public static Assembly LoadAssembly(string path)
            {
                byte[] data = File.ReadAllBytes(path);
                return Assembly.Load(data);
            }
            public static void AutoloadRequests()
            {
                string[] files = AtomicIO.TryGetFiles(REQUESTS_DLL_DIR, "*.dll");
                if (files == null)
                {
                    return;
                }
                foreach (string path in files)
                {
                    AutoloadRequests(path);
                }
            }
            public static void AutoloadRequests(string path)
            {
                List<Request> reqs;
                try
                {
                    reqs = LoadRequests(path);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error loading requests from " + path, ex);
                    return;
                }

                Logger.Log(LogType.SystemActivity, "AUTOLOAD: Loaded {0} from {1}",
                           reqs.Join(r => "/" + r.RequestName), Path.GetFileName(path));
            }
            public static List<Request> LoadRequests(string path)
            {
                Assembly lib = LoadAssembly(path);
                List<Request> requests = LoadTypes<Request>(lib);
                if (requests.Count == 0)
                {
                    throw new InvalidOperationException("No requests in " + path);
                }
                foreach (Request req in requests)
                {
                    if (Request.Find(req.RequestName) != null)
                    {
                        throw new AlreadyLoadedException("/" + req.RequestName + " is already loaded");
                    }
                    Request.RegisterREQ(req);
                }
                return requests;
            }
            public static string DescribeLoadError(string path, Exception ex)
            {
                string file = Path.GetFileName(path);
                if (ex is BadImageFormatException)
                {
                    return "&W" + file + " is not a valid assembly, or has an invalid dependency. Details in the error log.";
                }
                else if (ex is FileLoadException)
                {
                    return "&W" + file + " or one of its dependencies could not be loaded. Details in the error log.";
                }
                return "&WAn unknown error occured. Details in the error log.";
            }
            public static void AutoloadExperiments()
            {
                string[] files = AtomicIO.TryGetFiles(EXPERIMENTS_DLL_DIR, "*.dll");
                if (files == null)
                {
                    return;
                }
                Array.Sort(files);
                foreach (string path in files)
                {
                    try
                    {
                        LoadExperiment(path);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Error loading experiments from " + path, ex);
                    }
                }
            }
            public static List<Experiment> LoadExperiment(string path)
            {
                Assembly lib = LoadAssembly(path);
                List<Experiment> experiments = LoadTypes<Experiment>(lib);
                foreach (Experiment exp in experiments)
                {
                    if (Experiment.FindExperiment(exp.Name) != null)
                    {
                        throw new AlreadyLoadedException("Experiment " + exp.Name + " is already loaded");
                    }
                    Experiment.Load(exp);
                }
                return experiments;
            }
        }
        public static class ExpScriptingOperations
        {
            public static bool LoadRequests(Player p, string path)
            {
                if (!File.Exists(path))
                {
                    p.Message("File &9{0} &Snot found.", path);
                    return false;
                }
                try
                {
                    List<Request> reqs = ExpIScripting.LoadRequests(path);
                    p.Message("Successfully loaded &T{0}",
                              reqs.Join(r => "/" + r.RequestName));
                    return true;
                }
                catch (AlreadyLoadedException ex)
                {
                    p.Message(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    p.Message(ExpIScripting.DescribeLoadError(path, ex));
                    Logger.LogError("Error loading request from " + path, ex);
                    return false;
                }
            }
            public static bool LoadExperiments(Player p, string path)
            {
                if (!File.Exists(path))
                {
                    p.Message("File &9{0} &Snot found.", path);
                    return false;
                }
                try
                {
                    List<Experiment> experiments = ExpIScripting.LoadExperiment(path);
                    p.Message("Experiment {0} loaded successfully",
                              experiments.Join(exp => exp.ExperimentName));
                    return true;
                }
                catch (AlreadyLoadedException ex)
                {
                    p.Message(ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    p.Message(ExpIScripting.DescribeLoadError(path, ex));
                    Logger.LogError("Error loading experiments from " + path, ex);
                    return false;
                }
            }
            public static bool UnloadRequest(Player p, Request req)
            {
                Request.Unregister(req);
                p.Message("Request &T/{0} &Sunloaded successfully", req.RequestName);
                return true;
            }
            public static bool UnloadExperiment(Player p, Experiment experiment)
            {
                if (!Experiment.UnloadExperiment(experiment))
                {
                    p.Message("&WError unloading experiment. See error logs for more information.");
                    return false;
                }
                p.Message("Experiment {0} &Sunloaded successfully", experiment.ExperimentName);
                return true;
            }
        }
    public abstract partial class Experiment : Addon
    {
        public abstract void LoadExperiment();
        public abstract void UnloadExperiment();
        public override void Load() 
        {
            LoadExperiment();
        }
        public override void Unload()
        {
            UnloadExperiment();
        }
        public virtual void ExperimentHelp(Player p)
        {
            p.Message("No help is available for this experiment.");
        }
        public override void Help(Player p)
        {
            ExperimentHelp(p);
        }
        public abstract string ExperimentName
        {
            get; set;
        }
        public override string Name
        {
            get
            {
                return ExperimentName;
            }
        }
        public override string Flames_Version
        {
            get
            {
                return Server.Version;
            }
        }
        public override int Build
        {
            get
            {
                return 0;
            }
        }
        public override string Welcome
        {
            get
            {
                return "";
            }
        }
        public virtual string Creator
        {
            get
            {
                return "";
            }
        }
        public virtual bool LoadAtStartup
        {
            get
            {
                return true;
            }
        }
        public static List<Experiment> experiments = new List<Experiment>();
        public static Experiment FindExperiment(string name)
        {
            foreach (Experiment exp in experiments)
            {
                if (exp.ExperimentName.CaselessEq(name))
                {
                    return exp;
                }
            }
            return null;
        }
        public static void Load(Experiment exp)
        {
            try
            {
                experiments.Add(exp);
                if (exp.LoadAtStartup)
                {
                    exp.Load();
                    Logger.Log(LogType.SystemActivity, "Experiment {0} loaded...build: {1}", exp.Name, exp.Build);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "Experiment {0} was not loaded, you can load it with /expload", exp.Name);
                }

                if (!string.IsNullOrEmpty(exp.Welcome))
                {
                    Logger.Log(LogType.SystemActivity, exp.Welcome);
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(exp.Creator))
                {
                    Logger.Log(LogType.Warning, "You can go bug {0} about {1} failing to load.", exp.Creator, exp.Name);
                }
                throw;
            }
        }
        public static bool Unload(Experiment exp)
        {
            bool success = UnloadExperiment(exp);
            if (success)
            {
                experiments.Remove(exp);
            }
            return success;
        }
        public static bool UnloadExperiment(Experiment exp)
        {
            try
            {
                exp.UnloadExperiment();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading experiment " + exp.ExperimentName, ex);
                return false;
            }
        }
        public static void UnloadAll()
        {
            for (int i = 0; i < experiments.Count; i++)
            {
                UnloadExperiment(experiments[i]);
            }
            experiments.Clear();
        }
        public static void LoadAll()
        {
            LoadExperiment(new CompilerExperiment());
            ExpIScripting.AutoloadExperiments();
        }
        public static void LoadExperiment(Experiment experiment)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(experiment.ExperimentName))
            {
                return;
            }
            experiment.LoadExperiment();
            experiments.Add(experiment);
        }
    }
}
namespace Flames
{
    public partial class Server
    {
        public static bool cancelrequest, TLIMode, cancelorder;
        public delegate void OnFlameRequest(string req, string message);
        public static event OnFlameRequest FlameRequest;
        public static bool CheckRequests(string req, string message)
        {
            FlameRequest?.Invoke(req, message);
            return cancelrequest;
        }
        public delegate void OnFlameOrder(string ord, string message);
        public static event OnFlameOrder FlameOrder;
        public static bool CheckOrders(string ord, string message)
        {
            FlameOrder?.Invoke(ord, message);
            return cancelorder;
        }
    }
    public class AddonLoader : Plugin
    {
        public override string name { get { return "AddonLoader"; } }
        public override string creator { get { return Colors.Strip(Server.SoftwareName + " team"); } }
        public static void LoadAllAddons(SchedulerTask task)
        {
            Addon.LoadAll();
        }
        public override void Load(bool startup)
        {
            OnShuttingDownEvent.Register(OnShutdown, Priority.Critical);
            Server.Critical.QueueOnce(LoadAllAddons);
        }
        public void OnShutdown(bool restarting, string message)
        {
            Addon.UnloadAll();
        }
        public override void Unload(bool shutdown)
        {
            OnShuttingDownEvent.Unregister(OnShutdown);
        }
        public override void Help(Player p)
        {
            p.Message("");
        }
    }
}
namespace Flames.Added
{
    public class OrdOrders : Order
    {
        public static string OrderName = "Orders";
        public override string Name
        {
            get
            {
                return OrderName;
            }
            set
            {
                OrderName = value;
            }
        }
        public override string Shortcut { get { return "ords"; } }
        public override string Type { get { return OrderTypes.Order; } }
        public override bool RequestUseableWhenFrozen { get { return true; } }
        public override CommandAlias[] Aliases
        {
            get { return new[] { new CommandAlias("ordsList") }; }
        }
        public override void Execute(Player p, string message)
        {
            if (!ListRequests(p, message))
            {
                Help(p);
            }
        }
        public static bool ListRequests(Player p, string message)
        {
            string[] args = message.SplitSpaces();
            string sort = args.Length > 1 ? args[1].ToLower() : "";
            string modifier = args.Length > 2 ? args[2] : sort;
            if (args.Length == 2)
            {
                if (modifier == "name" || modifier == "names" || modifier == "rank" || modifier == "ranks")
                {
                    modifier = "";
                }
                else
                {
                    sort = "";
                }
            }
            string type = args[0].ToLower();
            if (type == "short" || type == "shortcut" || type == "shortcuts")
            {
                PrintShortcuts(p, modifier);
            }
            else if (type == "old" || type == "oldmenu" || type == "" || type == "order")
            {
                PrintRankOrders(p, sort, modifier, p.group, true);
            }
            else if (type == "all" || type == "orderall" || type == "ordersall")
            {
                PrintAllOrders(p, sort, modifier);
            }
            else
            {
                bool any = PrintCategoryOrders(p, sort, modifier, type);
                if (any) return true;
                Group grp = Group.Find(type);
                if (grp == null) return false;
                PrintRankOrders(p, sort, modifier, grp, false);
            }
            return true;
        }

        public static void PrintShortcuts(Player p, string modifier)
        {
            List<Order> shortcuts = new List<Order>();
            foreach (Order ord in allOrds)
            {
                if (ord.Shortcut.Length == 0) continue;
                if (!p.CanUse(ord)) continue;
                shortcuts.Add(ord);
            }

            Paginator.Output(p, shortcuts,
                             (ord) => "&b" + ord.Shortcut + " &S[" + ord.Name + "]",
                             "Orders shortcuts", "shortcuts", modifier);
        }

        public static void PrintRankOrders(Player p, string sort, string modifier, Group group, bool own)
        {
            List<Order> ord = new List<Order>();
            foreach (Order r in allOrds)
            {
                if (r.Permissions.UsableBy(group.Permission)) ord.Add(r);
            }
            if (ord.Count == 0)
            {
                p.Message("{0} &Scannot use any orders.", group.ColoredName);
                return;
            }
            SortOrders(ord, sort);
            if (own)
                p.Message("Available orders:");
            else
                p.Message("Orders available to " + group.ColoredName + " &Srank:");

            string type = "Ords " + group.Name;
            if (sort.Length > 0) type += " " + sort;
            Paginator.Output(p, ord, GetColoredName,
                             type, "orders", modifier);
            p.Message("Type &T/Help <order> &Sfor more help on an order.");
        }

        public static void PrintAllOrders(Player p, string sort, string modifier)
        {
            List<Order> ord = CopyAll();
            SortOrders(ord, sort);
            p.Message("All orders:");

            string type = "Orders all";
            if (sort.Length > 0) type += " " + sort;
            Paginator.Output(p, ord, GetColoredName,
                             type, "orders", modifier);
            p.Message("Type &T/Help <order> &Sfor more help on an order.");
        }

        public static bool PrintCategoryOrders(Player p, string sort, string modifier, string type)
        {
            List<Order> ord = new List<Order>();
            bool foundAny = false;

            // common shortcuts people tend to use
            type = MapCategory(type);

            foreach (Order o in allOrds)
            {
                string category = MapCategory(o.type);
                if (!type.CaselessEq(category)) continue;

                if (p.CanUse(o)) ord.Add(o);
                foundAny = true;
            }
            if (!foundAny) return false;

            if (ord.Count == 0)
            {
                p.Message("You cannot use any of the {0} orders.", type.Capitalize());
                return true;
            }
            SortOrders(ord, sort);
            p.Message(type.Capitalize() + " orders you may use:");

            type = "orders " + type;
            if (sort.Length > 0) type += " " + sort;
            Paginator.Output(p, ord, GetColoredName,
                             type, "orders", modifier);

            p.Message("Type &T/Help <order> &Sfor more help on an order.");
            return true;
        }

        public static void SortOrders(List<Order> ords, string sort)
        {
            if (sort == "name" || sort == "names")
            {
                ords.Sort((a, b) => a.Name
                          .CompareTo(b.Name));
            }
            if (sort == "rank" || sort == "ranks")
            {
                ords.Sort((a, b) => a.Permissions.MinRank
                          .CompareTo(b.Permissions.MinRank));
            }
        }

        public static string MapCategory(string type)
        {
            type = type.ToLower();
            // convert old category/type names
            if (type == "add") return CommandTypes.Added;
            if (type == "build") return CommandTypes.Building;
            if (type == "chat") return CommandTypes.Chat;
            if (type == "economy") return CommandTypes.Economy;
            if (type == "game") return CommandTypes.Games;
            if (type == "mod") return CommandTypes.Moderation;
            if (type == "other") return CommandTypes.Other;
            if (type == "world") return CommandTypes.World;
            if (type == "information") return CommandTypes.Information;
            if (type == "order") return OrderTypes.Order;
            return type;
        }
        public static string GetCategories()
        {
            Dictionary<string, bool> categories = new Dictionary<string, bool>();
            foreach (Order ord in allOrds)
            {
                categories[MapCategory(ord.type)] = true;
            }

            List<string> list = new List<string>(categories.Keys);
            list.Sort();
            return list.Join(" ");
        }

        public override void Help(Player p)
        {
            p.Message("&T/Orders [category] <sort by>");
            p.Message("  &HIf no category is given, outputs all orders you can use.");
            p.Message("  &HIf category is \"shortcuts\", outputs all order shortcuts.");
            p.Message("  &HIf category is \"all\", outputs all orders.");
            p.Message("  &HIf category is a rank name, outputs what that rank can use.");
            p.Message("&HOther order categories:");
            p.Message("  &H{0}", GetCategories());
            p.Message("&HSort By is optional, and can be either \"name\" or \"rank\"");
        }
    }
    public class OrdSearch : Order
    {
        public static string OrdName = "OrdSearch";
        public override string Name
        {
            get
            {
                return OrdName;
            }
            set
            {
                OrdName = value;
            }
        }
        public override string Type { get { return OrderTypes.Order; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Builder; } }
        public override bool UseableWhenFrozen { get { return true; } }
        public override void Execute(Player p, string message)
        {
            string[] args = message.SplitSpaces(3);
            if (args.Length < 2)
            {
                Help(p);
                return;
            }
            string list = args[0].ToLower();
            string keyword = args[1];
            string modifier = args.Length > 2 ? args[2] : "";
            if (list == "block" || list == "blocks")
            {
                SearchBlocks(p, keyword, modifier);
            }
            else if (list == "rank" || list == "ranks")
            {
                SearchRanks(p, keyword, modifier);
            }
            else if (list == "command" || list == "commands")
            {
                SearchCommands(p, keyword, modifier);
            }
            else if (list == "order" || list == "orders")
            {
                SearchOrders(p, keyword, modifier);
            }
            else if (list == "request" || list == "requests")
            {
                SearchRequests(p, keyword, modifier);
            }
            else if (list == "player" || list == "players")
            {
                SearchPlayers(p, keyword, modifier);
            }
            else if (list == "online")
            {
                SearchOnline(p, keyword, modifier);
            }
            else if (list == "loaded")
            {
                SearchLoaded(p, keyword, modifier);
            }
            else if (list == "level" || list == "levels" || list == "maps")
            {
                SearchMaps(p, keyword, modifier);
            }
            else
            {
                Help(p);
            }
        }


        public static void SearchBlocks(Player p, string keyword, string modifier)
        {
            List<ushort> blocks = new List<ushort>();
            for (int b = 0; b < Block.SUPPORTED_COUNT; b++)
            {
                ushort block = (ushort)b;
                if (Block.ExistsFor(p, block)) blocks.Add(block);
            }

            List<string> blockNames = Wildcard.Filter(blocks, keyword,
                                                      b => Block.GetName(p, b), null,
                                                      b => Block.GetColoredName(p, b));
            OutputList(p, keyword, "search blocks", "blocks", modifier, blockNames);
        }
        public static void SearchCommands(Player p, string keyword, string modifier)
        {
            List<string> commands = Wildcard.Filter(allCmds, keyword, cmd => cmd.name,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(allCmds, keyword, cmd => cmd.shortcut,
                                                     cmd => !string.IsNullOrEmpty(cmd.shortcut),
                                                     GetColoredName);

            // Match both names and shortcuts
            foreach (string shortcutCmd in shortcuts)
            {
                if (commands.CaselessContains(shortcutCmd)) continue;
                commands.Add(shortcutCmd);
            }

            OutputList(p, keyword, "search commands", "commands", modifier, commands);
        }
        public static void SearchOrders(Player p, string keyword, string modifier)
        {
            List<string> orders = Wildcard.Filter(allOrds, keyword, ord => ord.Name,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(allOrds, keyword, ord => ord.Shortcut,
                                                     ord => !string.IsNullOrEmpty(ord.Shortcut),
                                                     GetColoredName);
            foreach (string shortcutOrd in shortcuts)
            {
                if (orders.CaselessContains(shortcutOrd))
                {
                    continue;
                }
                orders.Add(shortcutOrd);
            }

            OutputList(p, keyword, "search orders", "orders", modifier, orders);
        }
        public static void SearchRequests(Player p, string keyword, string modifier)
        {
            List<string> requests = Wildcard.Filter(allReqs, keyword, req => req.RequestName,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(allReqs, keyword, req => req.RequestShortcut,
                                                     req => !string.IsNullOrEmpty(req.RequestShortcut),
                                                     GetColoredName);
            foreach (string shortcutReq in shortcuts)
            {
                if (requests.CaselessContains(shortcutReq))
                {
                    continue;
                }
                requests.Add(shortcutReq);
            }

            OutputList(p, keyword, "search requests", "requests", modifier, requests);
        }

        public static void SearchRanks(Player p, string keyword, string modifier)
        {
            List<string> ranks = Wildcard.Filter(Group.GroupList, keyword, grp => grp.Name,
                                                null, grp => grp.ColoredName);
            OutputList(p, keyword, "search ranks", "ranks", modifier, ranks);
        }

        public static void SearchOnline(Player p, string keyword, string modifier)
        {
            Player[] online = PlayerInfo.Online.Items;
            List<string> players = Wildcard.Filter(online, keyword, pl => pl.name,
                                                  pl => p.CanSee(pl), pl => pl.ColoredName);
            OutputList(p, keyword, "search online", "players", modifier, players);
        }

        public static void SearchLoaded(Player p, string keyword, string modifier)
        {
            Level[] loaded = LevelInfo.Loaded.Items;
            List<string> levels = Wildcard.Filter(loaded, keyword, level => level.name);
            OutputList(p, keyword, "search loaded", "loaded levels", modifier, levels);
        }

        public static void SearchMaps(Player p, string keyword, string modifier)
        {
            string[] allMaps = LevelInfo.AllMapNames();
            List<string> maps = Wildcard.Filter(allMaps, keyword, map => map);
            OutputList(p, keyword, "search levels", "maps", modifier, maps);
        }

        public static void OutputList(Player p, string keyword, string cmd, string type, string modifier, List<string> items)
        {
            if (items.Count == 0)
            {
                p.Message("No {0} found containing \"{1}\"", type, keyword);
            }
            else
            {
                Paginator.Output(p, items, item => item, cmd + " " + keyword, type, modifier);
            }
        }


        public static void SearchPlayers(Player p, string keyword, string modifier)
        {
            List<string> names = new List<string>();
            string suffix = Database.Backend.CaselessLikeSuffix;

            // TODO supporting more than 100 matches somehow
            Database.ReadRows("Players", "Name", r => names.Add(r.GetText(0)),
                              "WHERE Name LIKE @0 ESCAPE '#' LIMIT 100" + suffix,
                              Wildcard.ToSQLFilter(keyword));

            OutputList(p, keyword, "search players", "players", modifier, names);
        }


        public override void Help(Player p)
        {
            p.Message("&T/OrdSearch [list] [keyword]");
            p.Message("&HFinds entries in a list that match the given keyword");
            p.Message("&H  keyword can also include wildcard characters:");
            p.Message("&H    * - placeholder for zero or more characters");
            p.Message("&H    ? - placeholder for exactly one character");
            p.Message("&HLists: &fblocks/commands/orders/requests/ranks/players/online/loaded/maps");
        }
    }
    public partial class Order : Request
    {
        public override bool RequestSuperUseable { get { return OrderSuperUseable; } }
        public override bool RequestMessageBlockRestricted { get { return OrderMessageBlockRestricted; } }
        public override bool RequestUseableWhenFrozen { get { return OrderUseableWhenFrozen; } }
        public override bool RequestLogUsage { get { return OrderLogUsage; } }
        public override bool UpdatesLastReq { get { return UpdatesLastOrd; } }
        public override string RequestName { get { return Name; } }
        public virtual string Name { get; set; }
        public override string RequestShortcut { get { return Shortcut; } }
        public virtual string Shortcut { get { return ""; } }
        public override string RequestType { get { return Type; } }
        public virtual string Type { get { return ""; } }
        public override bool RequestMuseumUsable { get { return MuseumUsable; } }
        public virtual bool MuseumUsable { get { return true; } }
        public override bool ShowRequestInfo { get { return ShowOrderInfo; } }
        public virtual bool ShowOrderInfo { get { return true; } }
        public override LevelPermission RequestDefaultRank { get { return DefaultRank; } }
        public virtual LevelPermission DefaultRank { get { return LevelPermission.Guest; } }
        public override void ExecuteRequest(Player p, string message, CommandData data)
        {
            Execute(p, message, data);
        }
        public override void ExecuteRequest(Player p, string message)
        {
            Execute(p, message);
        }
        public virtual void Execute(Player p, string message)
        {
        }
        public virtual void Execute(Player p, string message, CommandData data)
        {
            Execute(p, message);
        }
        public override void Help(Player p)
        {
            p.Message("No help is available for this order.");
        }
        public virtual void Help(Player p, string message)
        {
            Help(p);
            AddedFormatter.PrintRequestInfo(p, this);
        }
        public static List<Order> CopyAll()
        {
            return new List<Order>(allOrds);
        }
        public virtual bool OrderSuperUseable { get { return true; } }
        public virtual bool OrderMessageBlockRestricted { get { return Type.CaselessContains("mod"); } }
        public virtual bool OrderUseableWhenFrozen { get { return false; } }
        public virtual bool OrderLogUsage { get { return true; } }
        public virtual bool UpdatesLastOrd { get { return true; } }
        public override CommandParallelism Parallelism
        {
            get
            {
                return base.Parallelism;
            }
        }
        public static List<Order> allOrds = new List<Order>();
        public static void InitAll()
        {
            allOrds.Clear();
            Alias.aliases.Clear();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (!type.IsSubclassOf(typeof(Order)) || type.IsAbstract || !type.IsPublic)
                {
                    continue;
                }
                Order ord = (Order)Activator.CreateInstance(type);
                RegisterORD(ord);
            }
            IScripting.AutoloadOrders();
        }
        public static void RegisterORD(Order ord)
        {
            RegisterREQ(ord);
            allOrds.Add(ord);
            allReqs.Add(ord);
            ord.Permissions = CommandPerms.GetOrAdd(ord.Name, ord.DefaultRank);
            CommandPerm[] extra = ord.ExtraPerms;
            if (extra != null)
            {
                for (int i = 0; i < extra.Length; i++)
                {
                    CommandExtraPerms exPerms = CommandExtraPerms.GetOrAdd(ord.Name, i + 1, extra[i].Perm);
                    exPerms.Desc = extra[i].Description;
                }
            }
            Alias.RegisterDefaults(ord);
        }
        public static void Register(params Order[] orders)
        {
            foreach (Order ord in orders)
            {
                RegisterORD(ord);
            }
        }
        public static bool Unregister(Order ord)
        {
            if (ord != null)
            {
                Alias.UnregisterDefaults(ord);
            }
            return Request.Unregister(ord);
        }
        public static void UnregisterOrders()
        {
            foreach (Order ord in allOrds)
            {
                Unregister(ord);
            }
        }
        public static Order FindORD(string name)
        {
            foreach (Order ord in allOrds)
            {
                if (ord.Name.CaselessEq(name))
                {
                    return ord;
                }
            }
            return null;
        }
        /*public static Request FindREQ(string name)
        {
            foreach (Request req in allOrds)
            {
                if (req.RequestName.CaselessEq(name))
                {
                    return req;
                }
            }
            return FindORD(name);
        }
        public static Command FindCMD(string name)
        {
            foreach (Command cmd in allCmds)
            {
                if (cmd.name.CaselessEq(name))
                {
                    return cmd;
                }
            }
            return FindREQ(name);
        }*/
        public static void Search(ref string ordName, ref string ordArgs)
        {
            if (ordName.Length == 0)
            {
                return;
            }
            Alias alias = Alias.Find(ordName);
            if (alias == null)
            {
                foreach (Order ord in allOrds)
                {
                    if (!ord.Shortcut.CaselessEq(ordName))
                    {
                        continue;
                    }
                    ordName = ord.Name;
                    return;
                }
                return;
            }
            ordName = alias.Target;
            string format = alias.Format;
            if (format == null)
            {
                return;
            }
            if (format.Contains("{args}"))
            {
                ordArgs = format.Replace("{args}", ordArgs);
            }
            else
            {
                ordArgs = format + " " + ordArgs;
            }
            ordArgs = ordArgs.Trim();
        }
    }
    public class OrderTypes
    {
        public const string Order = RequestTypes.Order;
    }
    public class RequestTypes
    {
        public const string Order = Request;
        public const string Request = CommandTypes.Added;
    }
    public abstract partial class Addon
    {
        public abstract void Load();
        public abstract void Unload();
        public virtual void Help(Player p)
        {
            p.Message("No help is available for this addon.");
        }
        public abstract string Name
        {
            get;
        }
        public virtual string Flames_Version
        {
            get
            {
                return Server.Version;
            }
        }
        public virtual int Build
        {
            get
            {
                return 0;
            }
        }
        public virtual string Welcome
        {
            get
            {
                return "";
            }
        }
        public virtual string Creator
        {
            get
            {
                return "";
            }
        }
        public virtual bool LoadAtStartup
        {
            get
            {
                return true;
            }
        }
        public static List<Addon> addons = new List<Addon>();
        public static Addon FindAddon(string name)
        {
            foreach (Addon ad in addons)
            {
                if (ad.Name.CaselessEq(name))
                {
                    return ad;
                }
            }
            return null;
        }
        public static void Load(Addon ad)
        {
            try
            {
                addons.Add(ad);
                if (ad.LoadAtStartup)
                {
                    ad.Load();
                    Logger.Log(LogType.SystemActivity, "Addon {0} loaded...build: {1}", ad.Name, ad.Build);
                }
                else
                {
                    Logger.Log(LogType.SystemActivity, "Addon {0} was not loaded, you can load it with /adload", ad.Name);
                }

                if (!string.IsNullOrEmpty(ad.Welcome))
                {
                    Logger.Log(LogType.SystemActivity, ad.Welcome);
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(ad.Creator))
                {
                    Logger.Log(LogType.Warning, "You can go bug {0} about {1} failing to load.", ad.Creator, ad.Name);
                }
                throw;
            }
        }
        public static bool Unload(Addon ad)
        {
            bool success = UnloadAddon(ad);
            if (success)
            {
                addons.Remove(ad);
            }
            return success;
        }
        public static bool UnloadAddon(Addon ad)
        {
            try
            {
                ad.Unload();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error unloading experiment " + ad.Name, ex);
                return false;
            }
        }
        public static void UnloadAll()
        {
            for (int i = 0; i < addons.Count; i++)
            {
                UnloadAddon(addons[i]);
            }
            addons.Clear();
        }
        public static void LoadAll()
        {
            LoadAddon(new ExperimentLoader());
            LoadAddon(new CompilerAddon());
            IScripting.AutoloadAddons();
        }
        public static void LoadAddon(Addon addon)
        {
            List<string> disabled = Server.Config.DisabledModules;
            if (disabled.CaselessContains(addon.Name))
            {
                return;
            }
            addon.Load();
            addons.Add(addon);
        }
    }
}
namespace Flames.Added.Compiling
{
    public class OrdOrderCreate : OrdCompile
    {
        public override string Name
        {
            get
            {
                return "OrderCreate";
            }
        }
        public override string Shortcut
        {
            get
            {
                return "ordcreate";
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ACreate", "addon")
                };
            }
        }
        public override void CompileOrder(Player p, string[] paths, ICompiler compiler)
        {
            if (compiler == null)
            {
                compiler = new CSCompiler();
            }
            foreach (string ord in paths)
            {
                CompilerOperations.CreateOrder(p, ord, compiler);
            }
        }
        public override void CompileAddon(Player p, string[] paths, ICompiler compiler)
        {
            foreach (string ord in paths)
            {
                CompilerOperations.CreateAddon(p, ord, compiler);
            }
        }
        public override void Help(Player p)
        {
            p.Message("&T/OrderCreate [name]");
            p.Message("&HCreates an example C# order named [name]");
            p.Message("&H  This can be used as the basis for creating a new order");
            p.Message("&T/OrderCreate addon [name]");
            p.Message("&HCreate an example C# addon named [name]");
        }
    }
    public class OrdCompile : Order
    {
        public static string OrderName = "OrderCompile";
        public override string Name
        {
            get
            {
                return OrderName;
            }
            set
            {
                OrderName = value;
            }
        }
        public override string Shortcut
        {
            get
            {
                return "ordcompile";
            }
        }
        public override string Type
        {
            get
            {
                return OrderTypes.Order;
            }
        }
        public override LevelPermission DefaultRank
        {
            get
            {
                return LevelPermission.Owner;
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ACompile", "addon")
                };
            }
        }
        public override bool MessageBlockRestricted
        {
            get
            {
                return true;
            }
        }
        public override void Execute(Player p, string message)
        {
            string[] args = message.SplitSpaces();
            bool addon = args[0].CaselessEq("addon");
            string name, lang;
            if (addon)
            {
                name = args.Length > 1 ? args[1] : "";
                lang = args.Length > 2 ? args[2] : "";
            }
            else
            {
                name = args[0];
                lang = args.Length > 1 ? args[1] : "";
            }
            if (name.Length == 0)
            {
                Help(p);
                return;
            }
            if (!Formatter.ValidFilename(p, name))
            {
                return;
            }
            ICompiler compiler = CompilerOperations.GetCompiler(p, lang);
            if (compiler == null)
            {
                return;
            }
            string[] paths = name.SplitComma();
            if (addon)
            {
                CompileAddon(p, paths, compiler);
            }
            else
            {
                CompileOrder(p, paths, compiler);
            }
        }
        public virtual void CompileAddon(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = IScripting.AddonPath(paths[0]);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.AddonPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "Addon", paths, dstPath);
        }
        public virtual void CompileOrder(Player p, string[] paths, ICompiler compiler)
        {
            string dstPath = IScripting.OrderPath(paths[0]);

            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = compiler.OrderPath(paths[i]);
            }
            CompilerOperations.Compile(p, compiler, "Order", paths, dstPath);
        }
        public override void Help(Player p)
        {
            ICompiler compiler = ICompiler.Compilers[0];
            p.Message("&T/Compile [order name]");
            p.Message("&HCompiles a .cs file containing a C# order into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.OrderPath("&H<name>&f"));
            p.Message("&T/Compile addon [addon name]");
            p.Message("&HCompiles a .cs file containing a C# addon into a DLL");
            p.Message("&H  Compiles from &f{0}", compiler.AddonPath("&H<name>&f"));
        }
    }
    public class OrdCompLoad : OrdCompile
    {
        public override string Name
        {
            get
            {
                return "OrderCompLoad";
            }
        }
        public override string Shortcut
        {
            get
            {
                return "ordcml";
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("OrdCompLoad"),
                    new CommandAlias("Ordercml")
                };
            }
        }
        public override void CompileAddon(Player p, string[] paths, ICompiler compiler)
        {
            string dst = IScripting.AddonPath(paths[0]);
            UnloadAddon(p, paths[0]);
            base.CompileAddon(p, paths, compiler);
            ScriptingOperations.LoadAddons(p, dst);
        }
        public static void UnloadAddon(Player p, string name)
        {
            Addon addon = Addon.FindAddon(name);
            if (addon == null)
            {
                return;
            }
            ScriptingOperations.UnloadAddon(p, addon);
        }
        public override void CompileOrder(Player p, string[] paths, ICompiler compiler)
        {
            string ord = paths[0];
            string dst = IScripting.OrderPath(ord);
            UnloadOrder(p, ord);
            base.CompileOrder(p, paths, compiler);
            ScriptingOperations.LoadOrders(p, dst);
        }
        public static void UnloadOrder(Player p, string ordName)
        {
            string ordArgs = "";
            Search(ref ordName, ref ordArgs);
            Order ord = FindORD(ordName);
            if (ord == null)
            {
                return;
            }
            ScriptingOperations.UnloadOrder(p, ord);
        }
        public override void Help(Player p)
        {
            p.Message("&T/CompLoad [order]");
            p.Message("&HCompiles and loads (or reloads) a C# order into the server");
            p.Message("&T/CompLoad addon [addon]");
            p.Message("&HCompiles and loads (or reloads) a C# addon into the server");
        }
    }
    public abstract class ICompiler
    {
        public const string ORDERS_SOURCE_DIR = "orders/";
        public const string ADDONS_SOURCE_DIR = "addons/";
        public const string ERROR_LOG_PATH = "logs/errors/addedcompiler.log";
        public virtual string FileExtension
        {
            get
            {
                return ".cs";
            }
        }
        public virtual string ShortName { get { return "C#"; } }
        public virtual string FullName { get { return "CSharp"; } }
        public virtual string OrderSkeleton
        {
            get
            {
                return @"//\tAuto-generated order skeleton class
//\tUse this as a basis for custom Flames orders
//\tNaming should be kept consistent (e.g. /update order should have a class name of 'OrdUpdate' and a filename of 'OrdUpdate.cs')
// As a note, Flames is designed for .NET 4.8

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;
using Flames.Added;
using Flames;

public class Ord{0} : Order
{{
\t// The order's name (what you put after a slash to use this order)
\tpublic override string Name {{ get {{ return ""{0}""; }} }}
\t// Order's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""o"")
\tpublic override string Shortcut {{ get {{ return """"; }} }}
\t// Which submenu this order displays in under /Help
\tpublic override string Type {{ get {{ return ""other""; }} }}
\t// Whether or not this order can be used in a museum. Block/map altering req should return false to avoid errors.
\tpublic override bool MuseumUsable {{ get {{ return true; }} }}
\t// The default rank required to use this order. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tpublic override LevelPermission DefaultRank {{ get {{ return LevelPermission.Guest; }} }}
\t// This is for when a player executes this order by doing /{0}
\t//   p is the player object for the player executing the order. 
\t//   message is the arguments given to the order. (e.g. for '/{0} this', message is ""this"")
\tpublic override void Use(Player p, string message)
\t{{
\t\tp.Message(""Hello World!"");
\t}}
\t// This is for when a player does /Help {0}
\tpublic override void Help(Player p)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example order."");
\t}}
}}";
            }
        }
        public virtual string AddonSkeleton
        {
            get
            {
                return @"//\tAuto-generated addon skeleton class
//\tUse this as a basis for custom Flames addons
// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""
// Add any other using statements you need after this
using System;
using Flames;
namespace Flames.Added
{{
\tpublic class {0} : Addon
\t{{
\t\t// The addon's name (i.e what shows in /Addons)
\t\tpublic override string Name {{ get {{ return ""{0}""; }} }}
\t\t// The oldest version of Flames this addon is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}
\t\t// Message displayed in server logs when this addon is loaded
\t\tpublic override string Welcome {{ get {{ return ""Loaded Message!""; }} }}
\t\t// Who created/authored this addon
\t\tpublic override string Creator {{ get {{ return ""{1}""; }} }}
\t\t// Called when this addon is being loaded (e.g. on server startup)
\t\tpublic override void Load()
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}
\t\t// Called when this addon is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload()
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}
\t\t// Displays help for or information about this addon
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this addon."");
\t\t}}
\t}}
}}";
            }
        }
        public string OrderPath(string name)
        {
            return ORDERS_SOURCE_DIR + "Ord" + name + FileExtension;
        }
        public string AddonPath(string name)
        {
            return ADDONS_SOURCE_DIR + name + FileExtension;
        }

        public static List<ICompiler> Compilers = new List<ICompiler>()
        {
            new CSCompiler()
        };
        public static string FormatSource(string source, params string[] args)
        {
            source = source.Replace(@"\t", "\t");
            source = source.Replace("\n", "\r\n");
            return string.Format(source, args);
        }
        public string GenExampleOrder(string ordName)
        {
            ordName = ordName.ToLower().Capitalize();
            return FormatSource(OrderSkeleton, ordName);
        }
        public string GenExampleAddon(string addon, string creator)
        {
            return FormatSource(AddonSkeleton, addon, creator, Server.Version);
        }
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, bool logErrors)
        {
            ICompilerErrors errors = DoCompile(srcPaths, dstPath);
            if (!errors.HasErrors || !logErrors)
            {
                return errors;
            }
            SourceMap sources = new SourceMap(srcPaths);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("############################################################");
            sb.AppendLine("Errors when compiling " + srcPaths.Join());
            sb.AppendLine("############################################################");
            sb.AppendLine();
            foreach (ICompilerError err in errors)
            {
                string type = err.IsWarning ? "Warning" : "Error";
                sb.AppendLine(DescribeError(err, srcPaths, "") + ":");
                if (err.Line > 0)
                {
                    sb.AppendLine(sources.Get(err.FileName, err.Line - 1));
                }
                if (err.Column > 0)
                {
                    sb.Append(' ', err.Column - 1);
                }
                sb.AppendLine("^-- " + type + " #" + err.ErrorNumber + " - " + err.ErrorText);
                sb.AppendLine();
                sb.AppendLine("-------------------------");
                sb.AppendLine();
            }
            using (StreamWriter w = new StreamWriter(ERROR_LOG_PATH, true))
            {
                w.Write(sb.ToString());
            }
            return errors;
        }
        public static string DescribeError(ICompilerError err, string[] srcs, string text)
        {
            string type = err.IsWarning ? "Warning" : "Error";
            string file = Path.GetFileName(err.FileName);
            return string.Format("{0}{1}{2}{3}", type, text,
                                 err.Line > 0 ? " on line " + err.Line : "",
                                 srcs.Length > 1 ? " in " + file : "");
        }
        public abstract ICompilerErrors DoCompile(string[] srcPaths, string dstPath);
        public static List<string> ProcessInput(string[] srcPaths, string commentPrefix)
        {
            List<string> referenced = new List<string>();
            for (int i = 0; i < srcPaths.Length; i++)
            {
                string path = Path.GetFullPath(srcPaths[i]);
                AddReferences(path, commentPrefix, referenced);
                srcPaths[i] = path;
            }
            referenced.Add(Server.GetServerDLLPath());
            return referenced;
        }
        public static void AddReferences(string path, string commentPrefix, List<string> referenced)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string refPrefix = commentPrefix + "reference ";
                string addPrefix = commentPrefix + "addonref ";
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    if (line.CaselessStarts(refPrefix))
                    {
                        referenced.Add(GetDLL(line));
                    }
                    else if (line.CaselessStarts(addPrefix))
                    {
                        path = Path.Combine(IScripting.ADDONS_DLL_DIR, GetDLL(line));
                        referenced.Add(Path.GetFullPath(path));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        public static string GetDLL(string line)
        {
            int index = line.IndexOf(' ') + 1;
            return line.Substring(index).Replace(";", "");
        }
    }
    public class ICompilerErrors : List<ICompilerError>
    {
        public bool HasErrors
        {
            get
            {
                return FindIndex(ce => !ce.IsWarning) >= 0;
            }
        }
    }
    public class ICompilerError
    {
        public int Line, Column;
        public string ErrorNumber, ErrorText, FileName;
        public bool IsWarning;
    }
    public class SourceMap
    {
        public string[] files;
        public List<string>[] sources;
        public SourceMap(string[] paths)
        {
            files = paths;
            sources = new List<string>[paths.Length];
        }
        public int FindFile(string file)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (file.CaselessEq(files[i]))
                {
                    return i;
                }
            }
            return -1;
        }
        public string Get(string file, int line)
        {
            int i = FindFile(file);
            if (i == -1)
            {
                return "";
            }
            List<string> source = sources[i];
            if (source == null)
            {
                try
                {
                    source = Utils.ReadAllLinesList(file);
                }
                catch
                {
                    source = new List<string>();
                }
                sources[i] = source;
            }
            return line < source.Count ? source[line] : "";
        }
    }
    public class CompilerAddon : Addon
    {
        public override string Name
        {
            get
            {
                return "CompilerAddon";
            }
        }
        public override void Load()
        {
            IScripting.Init();
            Server.EnsureDirectoryExists(ICompiler.ORDERS_SOURCE_DIR);
            Server.EnsureDirectoryExists(ICompiler.ADDONS_SOURCE_DIR);
        }
        public override void Unload()
        {
            Order.UnregisterOrders();
        }
    }
    public static class ICodeDomCompiler
    {
        public static CompilerParameters PrepareInput(string[] srcPaths, string dstPath, string commentPrefix)
        {
            CompilerParameters args = new CompilerParameters
            {
                GenerateExecutable = false,
                IncludeDebugInformation = false,
                OutputAssembly = dstPath
            };
            List<string> referenced = ICompiler.ProcessInput(srcPaths, commentPrefix);
            foreach (string assembly in referenced)
            {
                args.ReferencedAssemblies.Add(assembly);
            }
            return args;
        }
        public static void InitCompiler(ICompiler c, string language, ref CodeDomProvider compiler)
        {
            if (compiler != null)
            {
                return;
            }
            compiler = CodeDomProvider.CreateProvider(language);
            if (compiler != null)
            {
                return;
            }
            Logger.Log(LogType.Warning,
                       "WARNING: {0} compiler is missing, you will be unable to compile {1} files.",
                       c.FullName, c.FileExtension);
        }
        public static ICompilerErrors Compile(CompilerParameters args, string[] srcPaths, CodeDomProvider compiler)
        {
            CompilerResults results = compiler.CompileAssemblyFromFile(args, srcPaths);
            ICompilerErrors errors = new ICompilerErrors();
            foreach (CompilerError error in results.Errors)
            {
                ICompilerError ce = new ICompilerError
                {
                    Line = error.Line,
                    Column = error.Column,
                    ErrorNumber = error.ErrorNumber,
                    ErrorText = error.ErrorText,
                    IsWarning = error.IsWarning,
                    FileName = error.FileName
                };
                errors.Add(ce);
            }
            return errors;
        }
    }
    public class CSCompiler : ICompiler
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "C#"; } }
        public override string FullName { get { return "CSharp"; } }
        public CodeDomProvider compiler;
        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
        {
            CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");
            args.CompilerOptions += " /unsafe";
            ICodeDomCompiler.InitCompiler(this, "CSharp", ref compiler);
            return ICodeDomCompiler.Compile(args, srcPaths, compiler);
        }
        public override string OrderSkeleton
        {
            get
            {
                return @"//\tAuto-generated order skeleton class
//\tUse this as a basis for custom Flames orders
//\tNaming should be kept consistent (e.g. /update order should have a class name of 'OrdUpdate' and a filename of 'OrdUpdate.cs')
// As a note, Flames is designed for .NET 4.8
// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""
// Add any other using statements you need after this
using System;
using Flames;
using Flames.Added;
public class Ord{0} : Order
{{
\t// The order's name (what you put after a slash to use this order)
\tpublic override string Name {{ get {{ return ""{0}""; }} }}
\t// Order's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""o"")
\tpublic override string Shortcut {{ get {{ return """"; }} }}
\t// Which submenu this order displays in under /Help
\tpublic override string Type {{ get {{ return ""other""; }} }}
\t// Whether or not this order can be used in a museum. Block/map altering order should return false to avoid errors.
\tpublic override bool MuseumUsable {{ get {{ return true; }} }}
\t// The default rank required to use this order. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tpublic override LevelPermission DefaultRank {{ get {{ return LevelPermission.Guest; }} }}
\t// This is for when a player executes this order by doing /{0}
\t//   p is the player object for the player executing the order. 
\t//   message is the arguments given to the order. (e.g. for '/{0} this', message is ""this"")
\tpublic override void Use(Player p, string message)
\t{{
\t\tp.Message(""Hello World!"");
\t}}
\t// This is for when a player does /Help {0}
\tpublic override void Help(Player p)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example order."");
\t}}
}}";
            }
        }
        public override string AddonSkeleton
        {
            get
            {
                return @"//\tAuto-generated experiment skeleton class
//\tUse this as a basis for custom Flames experiments
// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""
// Add any other using statements you need after this
using System;
using Flames;
namespace Flames.Added
{{
\tpublic class {0} : Addon
\t{{
\t\t// The addon's name (i.e what shows in /Addons)
\t\tpublic override string Name {{ get {{ return ""{0}""; }} }}
\t\t// The oldest version of Flames this addon is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}
\t\t// Message displayed in server logs when this addon is loaded
\t\tpublic override string Welcome {{ get {{ return ""Loaded Message!""; }} }}
\t\t// Who created/authored this addon
\t\tpublic override string Creator {{ get {{ return ""{1}""; }} }}
\t\t// Called when this addon is being loaded (e.g. on server startup)
\t\tpublic override void Load()
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}
\t\t// Called when this addon is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload()
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}
\t\t// Displays help for or information about this addon
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this addon."");
\t\t}}
\t}}
}}";
            }
        }
    }
    public static class CompilerOperations
    {
        public static ICompiler GetCompiler(Player p, string name)
        {
            if (name.Length == 0)
            {
                return ICompiler.Compilers[0];
            }
            foreach (ICompiler comp in ICompiler.Compilers)
            {
                if (comp.ShortName.CaselessEq(name))
                {
                    return comp;
                }
            }
            p.Message("&WUnknown language \"{0}\"", name);
            p.Message("&HAvailable languages: &f{0}",
                      ICompiler.Compilers.Join(c => c.ShortName + " (" + c.FullName + ")"));
            return null;
        }
        public static bool CreateOrder(Player p, string name, ICompiler compiler)
        {
            string path = compiler.OrderPath(name);
            string source = compiler.GenExampleOrder(name);
            return CreateFile(p, name, path, "order &fOrd", source);
        }
        public static bool CreateAddon(Player p, string name, ICompiler compiler)
        {
            string path = compiler.AddonPath(name);
            string creator = p.IsSuper ? Server.Config.Name : p.truename;
            string source = compiler.GenExampleAddon(name, creator);
            return CreateFile(p, name, path, "addon &f", source);
        }
        public static bool CreateFile(Player p, string name, string path, string type, string source)
        {
            if (File.Exists(path))
            {
                p.Message("File {0} already exists. Choose another name.", path);
                return false;
            }
            File.WriteAllText(path, source);
            p.Message("Successfully saved example {2}{0} &Sto {1}", name, path, type);
            return true;
        }
        public static bool Compile(Player p, ICompiler compiler, string type, string[] srcs, string dst)
        {
            foreach (string path in srcs)
            {
                if (File.Exists(path))
                {
                    continue;
                }
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }
            ICompilerErrors errors = compiler.Compile(srcs, dst, true);
            if (!errors.HasErrors)
            {
                p.Message("{0} compiled successfully from {1}",
                        type, srcs.Join(file => Path.GetFileName(file)));
                return true;
            }
            SummariseErrors(errors, srcs, p);
            return false;
        }
        public const int MAX_LOG = 5;
        public static void SummariseErrors(ICompilerErrors errors, string[] srcs, Player p)
        {
            int logged = 0;
            foreach (ICompilerError err in errors)
            {
                p.Message("&W{1} - {0}", err.ErrorText,
                          ICompiler.DescribeError(err, srcs, " #" + err.ErrorNumber));
                logged++;
                if (logged >= MAX_LOG)
                {
                    break;
                }
            }
            if (logged < errors.Count)
            {
                p.Message(" &W.. and {0} more", errors.Count - logged);
            }
            p.Message("&WCompiling failed. See " + ICompiler.ERROR_LOG_PATH + " for more detail");
        }
    }
}
namespace Flames.Added.Orders.Scripting
{
    public class OrdOrderLoad : Order
    {
        public static string OrderName = "OrderLoad";
        public override string Name
        {
            get
            {
                return OrderName;
            }
            set
            {
                OrderName = value;
            }
        }
        public override string Type { get { return OrderTypes.Order; } }
        public override string Shortcut { get { return "OrdLoad"; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Execute(Player p, string ordName)
        {
            if (ordName.Length == 0)
            {
                Help(p);
                return;
            }
            if (!Formatter.ValidFilename(p, ordName))
            {
                return;
            }
            string path = IScripting.OrderPath(ordName);
            ScriptingOperations.LoadOrders(p, path);
        }
        public override void Help(Player p)
        {
            p.Message("&T/OrderLoad [order name]");
            p.Message("&HLoads a compiled order into the server for use.");
        }
    }
    public class OrdOrderUnload : Order
    {
        public static string OrderName = "OrderUnload";
        public override string Name
        {
            get
            {
                return OrderName;
            }
            set
            {
                OrderName = value;
            }
        }

        public override string Type { get { return OrderTypes.Order; } }
        public override string Shortcut { get { return "OrdUnload"; } }
        public override LevelPermission DefaultRank { get { return LevelPermission.Owner; } }
        public override bool MessageBlockRestricted { get { return true; } }
        public override void Execute(Player p, string ordName)
        {
            if (ordName.Length == 0)
            {
                Help(p);
                return;
            }
            string ordArgs = "";
            Search(ref ordName, ref ordArgs);
            Order ord = FindORD(ordName);
            if (ord == null)
            {
                p.Message("\"{0}\" is not a valid or loaded order.", ordName);
                return;
            }
            ScriptingOperations.UnloadOrder(p, ord);
        }
        public override void Help(Player p)
        {
            p.Message("&T/OrderUnload [order]");
            p.Message("&HUnloads an order from the server.");
        }
    }
    public class OrdAddon : Order
    {
        public static string OrderName = "Addon";
        public override string Name
        {
            get
            {
                return OrderName;
            }
            set
            {
                OrderName = value;
            }
        }
        public override string Type
        {
            get
            {
                return OrderTypes.Order;
            }
        }
        public override LevelPermission DefaultRank
        {
            get
            {
                return LevelPermission.Owner;
            }
        }
        public override CommandAlias[] Aliases
        {
            get
            {
                return new[]
                {
                    new CommandAlias("ALoad", "load"),
                    new CommandAlias("AUnload", "unload"),
                    new CommandAlias("Addons", "list")
                };
            }
        }
        public override bool MessageBlockRestricted
        {
            get
            {
                return true;
            }
        }
        public override void Execute(Player p, string message)
        {
            string[] args = message.SplitSpaces(2);
            if (IsListAction(args[0]))
            {
                string modifier = args.Length > 1 ? args[1] : "";
                p.Message("Loaded addons:");
                Paginator.Output(p, Addon.addons, ad => ad.Name,
                                 "Addons", "addons", modifier);
                return;
            }
            if (args.Length == 1)
            {
                Help(p);
                return;
            }
            string ord = args[0], name = args[1];
            if (!Formatter.ValidFilename(p, name))
            {
                return;
            }
            if (ord.CaselessEq("load"))
            {
                string path = IScripting.AddonPath(name);
                ScriptingOperations.LoadAddons(p, path);
            }
            else if (ord.CaselessEq("unload"))
            {
                UnloadAddon(p, name);
            }
            else if (ord.CaselessEq("create"))
            {
                FindORD("OrderCreate").Execute(p, "addon " + name);
            }
            else if (ord.CaselessEq("compile"))
            {
                FindORD("OrderCompile").Execute(p, "addon " + name);
            }
            else
            {
                Help(p);
            }
        }
        public static void UnloadAddon(Player p, string name)
        {
            int matches;
            Addon addon = Matcher.Find(p, name, out matches, Addon.addons,
                                         null, ad => ad.Name, "addons");
            if (addon == null)
            {
                return;
            }
            ScriptingOperations.UnloadAddon(p, addon);
        }
        public override void Help(Player p)
        {
            p.Message("&T/Addon load [filename]");
            p.Message("&HLoad a compiled addon from the &faddons &Hfolder");
            p.Message("&T/Addon unload [name]");
            p.Message("&HUnloads a currently loaded addon");
            p.Message("&T/Addon list");
            p.Message("&HLists all loaded addons");
        }
    }
}
namespace Flames.Added.Scripting
{
    public class AlreadyLoadedException : Exception
    {
        public AlreadyLoadedException(string msg) : base(msg)
        {
        }
    }
    public static class IScripting
    {
        public const string ORDERS_DLL_DIR = "orders/";
        public const string ADDONS_DLL_DIR = "addons/";
        public static string OrderPath(string name)
        {
            return ORDERS_DLL_DIR + "Ord" + name + ".dll";
        }
        public static string AddonPath(string name)
        {
            return ADDONS_DLL_DIR + name + ".dll";
        }
        public static void Init()
        {
            Order.InitAll();
            Directory.CreateDirectory(ORDERS_DLL_DIR);
            Directory.CreateDirectory(ADDONS_DLL_DIR);
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAddonAssembly;
        }
        public static Assembly ResolveAddonAssembly(object sender, ResolveEventArgs args)
        {
            if (args.RequestingAssembly == null)
            {
                return null;
            }
            if (!IsAddonDLL(args.RequestingAssembly))
            {
                return null;
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assem in assemblies)
            {
                if (!IsAddonDLL(assem))
                {
                    continue;
                }
                if (args.Name == assem.FullName)
                {
                    return assem;
                }
            }
            Logger.Log(LogType.Warning, "Custom order/addon [{0}] tried to load [{1}], but it could not be found",
                       args.RequestingAssembly.FullName, args.Name);
            return null;
        }
        public static bool IsAddonDLL(Assembly a)
        {
            return string.IsNullOrEmpty(a.Location);
        }
        public static List<T> LoadTypes<T>(Assembly lib)
        {
            List<T> instances = new List<T>();
            foreach (Type t in lib.GetTypes())
            {
                if (t.IsAbstract || t.IsInterface || !t.IsSubclassOf(typeof(T)))
                {
                    continue;
                }
                object instance = Activator.CreateInstance(t);
                if (instance == null)
                {
                    Logger.Log(LogType.Warning, "{0} \"{1}\" could not be loaded", typeof(T).Name, t.Name);
                    throw new BadImageFormatException();
                }
                instances.Add((T)instance);
            }
            return instances;
        }
        public static Assembly LoadAssembly(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            return Assembly.Load(data);
        }
        public static void AutoloadOrders()
        {
            string[] files = AtomicIO.TryGetFiles(ORDERS_DLL_DIR, "*.dll");
            if (files == null)
            {
                return;
            }
            foreach (string path in files)
            {
                AutoloadOrders(path);
            }
        }
        public static void AutoloadOrders(string path)
        {
            List<Order> ords;
            try
            {
                ords = LoadOrders(path);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error loading orders from " + path, ex);
                return;
            }

            Logger.Log(LogType.SystemActivity, "AUTOLOAD: Loaded {0} from {1}",
                       ords.Join(o => "/" + o.Name), Path.GetFileName(path));
        }
        public static List<Order> LoadOrders(string path)
        {
            Assembly lib = LoadAssembly(path);
            List<Order> orders = LoadTypes<Order>(lib);
            if (orders.Count == 0)
            {
                throw new InvalidOperationException("No orders in " + path);
            }
            foreach (Order ord in orders)
            {
                if (Order.FindORD(ord.Name) != null)
                {
                    throw new AlreadyLoadedException("/" + ord.Name + " is already loaded");
                }
                Order.Register(ord);
            }
            return orders;
        }
        public static string DescribeLoadError(string path, Exception ex)
        {
            string file = Path.GetFileName(path);
            if (ex is BadImageFormatException)
            {
                return "&W" + file + " is not a valid assembly, or has an invalid dependency. Details in the error log.";
            }
            else if (ex is FileLoadException)
            {
                return "&W" + file + " or one of its dependencies could not be loaded. Details in the error log.";
            }
            return "&WAn unknown error occured. Details in the error log.";
        }
        public static void AutoloadAddons()
        {
            string[] files = AtomicIO.TryGetFiles(ADDONS_DLL_DIR, "*.dll");
            if (files == null)
            {
                return;
            }
            Array.Sort(files);
            foreach (string path in files)
            {
                try
                {
                    LoadAddon(path);
                }
                catch (Exception ex)
                {
                    Logger.LogError("Error loading addons from " + path, ex);
                }
            }
        }
        public static List<Addon> LoadAddon(string path)
        {
            Assembly lib = LoadAssembly(path);
            List<Addon> addons = LoadTypes<Addon>(lib);
            foreach (Addon ad in addons)
            {
                if (Addon.FindAddon(ad.Name) != null)
                {
                    throw new AlreadyLoadedException("Addon " + ad.Name + " is already loaded");
                }
                Addon.Load(ad);
            }
            return addons;
        }
    }
    public static class ScriptingOperations
    {
        public static bool LoadOrders(Player p, string path)
        {
            if (!File.Exists(path))
            {
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }
            try
            {
                List<Order> ords = IScripting.LoadOrders(path);
                p.Message("Successfully loaded &T{0}",
                          ords.Join(o => "/" + o.Name));
                return true;
            }
            catch (AlreadyLoadedException ex)
            {
                p.Message(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                p.Message(IScripting.DescribeLoadError(path, ex));
                Logger.LogError("Error loading order from " + path, ex);
                return false;
            }
        }
        public static bool LoadAddons(Player p, string path)
        {
            if (!File.Exists(path))
            {
                p.Message("File &9{0} &Snot found.", path);
                return false;
            }
            try
            {
                List<Addon> addons = IScripting.LoadAddon(path);
                p.Message("Addon {0} loaded successfully",
                          addons.Join(ad => ad.Name));
                return true;
            }
            catch (AlreadyLoadedException ex)
            {
                p.Message(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                p.Message(IScripting.DescribeLoadError(path, ex));
                Logger.LogError("Error loading addons from " + path, ex);
                return false;
            }
        }
        public static bool UnloadOrder(Player p, Order ord)
        {
            Order.Unregister(ord);
            p.Message("Order &T/{0} &Sunloaded successfully", ord.Name);
            return true;
        }
        public static bool UnloadAddon(Player p, Addon addon)
        {
            if (!Addon.Unload(addon))
            {
                p.Message("&WError unloading addon. See error logs for more information.");
                return false;
            }
            p.Message("Addon {0} &Sunloaded successfully", addon.Name);
            return true;
        }
    }
}
namespace Flames.Added.UI
{
    public enum LogType
    {
        BackgroundActivity,
        SystemActivity,
        GameActivity,
        UserActivity,
        SuspiciousActivity,
        RelayActivity,
        Warning,
        Error,
        RequestUsage,
        PlayerChat,
        RelayChat,
        ChatroomChat,
        StaffChat,
        PrivateChat,
        RankChat,
        Debug,
        FlameMessage,
        TerminalMessage,
    }
    public delegate void LogHandler(LogType type, string message);
    public static class Logger
    {
        public static LogHandler LogHandler;
        public static object logLock = new object();
        public static void Log(LogType type, string message)
        {
            lock (logLock)
            {
                try
                {
                    if (LogHandler != null)
                    {
                        LogHandler(type, message);
                    }
                }
                catch (Exception ex)
                {
                    LogLoggerError(ex);
                }
            }
        }
        public static void LogLoggerError(Exception ex)
        {
            try
            {
                LogHandler(LogType.Error, FormatException(ex));
            }
            catch
            {
            }
        }
        public static void Log(LogType type, string format, object arg0)
        {
            Log(type, string.Format(format, arg0));
        }
        public static void Log(LogType type, string format, object arg0, object arg1)
        {
            Log(type, string.Format(format, arg0, arg1));
        }
        public static void Log(LogType type, string format, object arg0, object arg1, object arg2)
        {
            Log(type, string.Format(format, arg0, arg1, arg2));
        }
        public static void Log(LogType type, string format, params object[] args)
        {
            Log(type, string.Format(format, args));
        }
        public static void LogError(string action, Exception ex)
        {
            Log(LogType.Warning, action);
            Log(LogType.Error, FormatException(ex));
        }
        public static void LogError(Exception ex)
        {
            Log(LogType.Error, FormatException(ex));
        }
        public static string FormatException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                DescribeError(ex, sb);
                ex = ex.InnerException;
            }
            return sb.ToString();
        }
        public static void DescribeError(Exception ex, StringBuilder sb)
        {
            try
            {
                sb.AppendLine("Type: " + ex.GetType().Name);
            }
            catch
            {
            }
            try
            {
                sb.AppendLine("Source: " + ex.Source);
            }
            catch
            {
            }
            try
            {
                sb.AppendLine("Message: " + ex.Message);
            }
            catch
            {
            }
            try
            {
                sb.AppendLine("Trace: " + ex.StackTrace);
            }
            catch
            {
            }
            try
            {
                ReflectionTypeLoadException refEx = ex as ReflectionTypeLoadException;
                if (refEx != null)
                {
                    LogLoaderErrors(refEx, sb);
                }
            }
            catch
            {
            }

            try
            {
                SocketException sockEx = ex as SocketException;
                if (sockEx != null)
                {
                    LogSocketErrors(sockEx, sb);
                }
            }
            catch
            {
            }
        }
        public static void LogLoaderErrors(ReflectionTypeLoadException ex, StringBuilder sb)
        {
            sb.AppendLine("## Loader exceptions ##");
            foreach (Exception loadEx in ex.LoaderExceptions)
            {
                DescribeError(loadEx, sb);
            }
        }
        public static void LogSocketErrors(SocketException ex, StringBuilder sb)
        {
            sb.AppendLine("Error: " + ex.SocketErrorCode);
        }
    }
    public static class UIHelpers
    {
        public static string lastREQ = "";
        public static void HandleChat(string text)
        {
            if (text != null)
            {
                text = text.Trim();
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            Player p = Player.Flame;
            if (ChatModes.Handle(p, text))
            {
                return;
            }
            Chat.MessageChat(ChatScope.Global, p, "λFULL: &f" + text, null, null, true);
        }
        public static void RepeatRequest()
        {
            if (lastREQ.Length == 0)
            {
                Logger.Log(LogType.RequestUsage, "(Flames): Cannot repeat request - no requests used yet.");
                return;
            }
            Logger.Log(LogType.RequestUsage, "Repeating &T/" + lastREQ);
            HandleRequest(lastREQ);
        }
        public static void HandleRequest(string text)
        {
            if (text != null)
            {
                text = text.Trim();
            }
            if (string.IsNullOrEmpty(text))
            {
                Logger.Log(LogType.RequestUsage, "(Flames): Whitespace requests are not allowed.");
                return;
            }
            if (text[0] == '/' && text.Length > 1)
            {
                text = text.Substring(1);
            }
            Flames.UI.UIHelpers.lastCMD = lastREQ;
            lastREQ = text;
            string name, args;
            text.Separate(' ', out name, out args);
            Command.Search(ref name, ref args);
            if (Server.CheckRequests(name, args))
            {
                Server.cancelrequest = false;
                if (Server.CheckOrders(name, args))
                {
                    Server.cancelorder = false;
                    if (Server.Check(name, args))
                    {
                        Server.cancelcommand = false;
                        return;
                    }
                }
            }
            Command req = Request.FindCMD(name);
            if (req == null)
            {
                Logger.Log(LogType.RequestUsage, "(Flames): Unknown request \"{0}\"", name);
                return;
            }
            if (req != null)
            {
                if (!req.SuperUseable)
                {
                    Logger.Log(LogType.RequestUsage, "(Flames): /{0} can only be used in-game.", req.name);
                    return;
                }
                Thread thread;
                Server.StartThread(out thread, "FlamesREQ_" + name,
                    () =>
                    {
                        try
                        {
                            req.Use(Player.Flame, args);
                            if (args.Length == 0)
                            {
                                Logger.Log(LogType.RequestUsage, "(Flames) used /" + req.name);
                            }
                            else
                            {
                                Logger.Log(LogType.RequestUsage, "(Flames) used /" + req.name + " " + args);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                            Logger.Log(LogType.RequestUsage, "(Flames): FAILED TO FOLLOW REQUEST!");
                        }
                    });
                Utils.SetBackgroundMode(thread);
            }
        }
        public static string Format(string message)
        {
            message = message.Replace("%S", "&f");
            message = Colors.Escape(message);
            return message;
        }
        public static string OutputPart(ref char nextCol, ref int start, string message)
        {
            int next = NextPart(start, message);
            string part;
            if (next == -1)
            {
                part = message.Substring(start);
                start = message.Length;
            }
            else
            {
                part = message.Substring(start, next - start);
                start = next + 2;
                nextCol = message[next + 1];
            }
            return part;
        }
        public static int NextPart(int start, string message)
        {
            for (int i = start; i < message.Length; i++)
            {
                if (message[i] != '&')
                {
                    continue;
                }
                if (i == message.Length - 1)
                {
                    return -1;
                }
                char col = Colors.Lookup(message[i + 1]);
                if (col != '\0')
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
#endif
