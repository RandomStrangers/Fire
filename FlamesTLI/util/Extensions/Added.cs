using Flames.Added;
using Flames.Added.Compiling;
using Flames.Added.Scripting;
using Flames.Commands;
using Flames.Events.ServerEvents;
using Flames.Maths;
using Flames.SQL;
using Flames.Tasks;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Context = System.Environment;
namespace Flames
{
    public static partial class Paths
    {
        public const string DesignationsFile = "text/designations.txt";
        public const string OrdPermsFile = "properties/order.properties";
        public const string OrdExtraPermsFile = "properties/ExtraOrderPermissions.properties";
    }
    public partial class Server
    {
        public static bool TLIMode, cancelorder;
        public delegate void OnFlameOrder(string ord, string message);
        public static event OnFlameOrder FlameOrder;
        public static bool CheckOrders(string ord, string message)
        {
            FlameOrder?.Invoke(ord, message);
            return cancelorder;
        }
    }
    public class AddonLoader : NewPlugin
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
    /// <summary> Represents additional permissions required to perform a special action in an ordr. </summary>
    /// <remarks> For example, /color order has an extra permission for changing the color of other players. </remarks>
    public class OrderExtraPerms : ItemPerms
    {
        public string OrdName, Desc = "";
        public int Num;
        public override string ItemName { get { return OrdName + ":" + Num; } }

        public static List<OrderExtraPerms> list = new List<OrderExtraPerms>();


        public OrderExtraPerms(string ord, int num, string desc, LevelPermission min) : base(min)
        {
            OrdName = ord;
            Num = num;
            Desc = desc;
        }

        public OrderExtraPerms Copy()
        {
            OrderExtraPerms copy = new OrderExtraPerms(OrdName, Num, Desc, 0);
            CopyPermissionsTo(copy);
            return copy;
        }


        public static OrderExtraPerms Find(string ord, int num)
        {
            foreach (OrderExtraPerms perms in list)
            {
                if (perms.OrdName.CaselessEq(ord) && perms.Num == num) return perms;
            }
            return null;
        }

        public static List<OrderExtraPerms> FindAll(string cmd)
        {
            List<OrderExtraPerms> all = new List<OrderExtraPerms>();
            foreach (OrderExtraPerms perms in list)
            {
                if (perms.OrdName.CaselessEq(cmd) && perms.Desc.Length > 0) all.Add(perms);
            }
            return all;
        }


        /// <summary> Gets or adds the nth extra permission for the given order. </summary>
        public static OrderExtraPerms GetOrAdd(string ord, int num, LevelPermission min)
        {
            OrderExtraPerms perms = Find(ord, num);
            if (perms != null) return perms;

            perms = new OrderExtraPerms(ord, num, "", min);
            list.Add(perms);
            return perms;
        }

        public void MessageCannotUse(Player p)
        {
            p.Message("Only {0} {1}", Describe(), Desc);
        }


        public static object ioLock = new object();
        /// <summary> Saves list of extra permissions to disc. </summary>
        public static void Save()
        {
            try
            {
                lock (ioLock) SaveCore();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving " + Paths.OrdExtraPermsFile, ex);
            }
        }

        public static void SaveCore()
        {
            using (StreamWriter w = new StreamWriter(Paths.OrdExtraPermsFile))
            {
                WriteHeader(w, "extra order permissions", "extra permissions in some order",
                            "OrderName:ExtraPermissionNumber", "countdown:1");

                foreach (OrderExtraPerms perms in list)
                {
                    w.WriteLine(perms.Serialise());
                }
            }
        }


        /// <summary> Loads list of extra permissions to disc. </summary>
        public static void Load()
        {
            lock (ioLock)
            {
                if (!File.Exists(Paths.OrdExtraPermsFile)) Save();

                using (StreamReader r = new StreamReader(Paths.OrdExtraPermsFile))
                {
                    ProcessLines(r);
                }
            }
        }

        public static void ProcessLines(StreamReader r)
        {
            string[] args = new string[5];
            OrderExtraPerms perms;
            string line;

            while ((line = r.ReadLine()) != null)
            {
                if (line.IsCommentLine() || line.IndexOf(':') == -1) continue;
                // Format - Name:Num : Lowest : Disallow : Allow
                line.Replace(" ", "").FixedSplit(args, ':');

                try
                {
                    LevelPermission min;
                    List<LevelPermission> allowed, disallowed;

                    // Old format - Name:Num : Lowest : Description
                    if (IsDescription(args[3]))
                    {
                        min = (LevelPermission)int.Parse(args[2]);
                        allowed = null;
                        disallowed = null;
                    }
                    else
                    {
                        Deserialise(args, 2, out min, out allowed, out disallowed);
                    }

                    perms = GetOrAdd(args[0], int.Parse(args[1]), min);
                    perms.Init(min, allowed, disallowed);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogType.Warning, "Hit an error on the extra order perms " + line);
                    Logger.LogError(ex);
                }
            }
        }

        public static bool IsDescription(string arg)
        {
            foreach (char c in arg)
            {
                if (c >= 'a' && c <= 'z') return true;
            }
            return false;
        }
    }
    /// <summary> Represents which ranks are allowed (and which are disallowed) to use an order. </summary>
    public class OrderPerms : ItemPerms
    {
        public static CommandPerm[] ORDToCMD(params OrderPerm[] Perms)
        {
            CommandPerm[] cmdPerms = new CommandPerm[]
            {
            };
            foreach (OrderPerm perm in Perms)
            {
                CommandPerm cmdPerm = new CommandPerm();
                cmdPerm.Perm = perm.Perm;
                cmdPerm.Description = perm.Description;
                cmdPerms = new CommandPerm[]
                {
                    cmdPerm
                };
                return cmdPerms;
            }
            return cmdPerms;
        }
        public static OrderPerm[] CMDToORD(params CommandPerm[] Perms)
        {
            OrderPerm[] ordPerms = new OrderPerm[]
            {
            };
            if (Perms == null)
            {
                return ordPerms;
            }
            foreach (CommandPerm perm in Perms)
            {
                OrderPerm ordPerm = new OrderPerm();
                ordPerm.Perm = perm.Perm;
                ordPerm.Description = perm.Description;
                ordPerms = new OrderPerm[]
                {
                    ordPerm
                };
                return ordPerms;
            }
            return ordPerms;
        }
        public string OrdName;
        public override string ItemName { get { return OrdName; } }

        public static List<OrderPerms> List = new List<OrderPerms>();


        public OrderPerms(string ord, LevelPermission min) : base(min)
        {
            OrdName = ord;
        }

        public OrderPerms Copy()
        {
            OrderPerms copy = new OrderPerms(OrdName, 0);
            CopyPermissionsTo(copy);
            return copy;
        }


        /// <summary> Find the permissions for the given order. (case insensitive) </summary>
        public static OrderPerms Find(string cmd)
        {
            foreach (OrderPerms perms in List)
            {
                if (perms.OrdName.CaselessEq(cmd)) return perms;
            }
            return null;
        }


        /// <summary> Gets or adds permissions for the given command. </summary>
        public static OrderPerms GetOrAdd(string ord, LevelPermission min)
        {
            OrderPerms perms = Find(ord);
            if (perms != null) return perms;

            perms = new OrderPerms(ord, min);
            List.Add(perms);
            return perms;
        }

        public void MessageCannotUse(Player p)
        {
            p.Message("Only {0} can use &T/{1}", Describe(), OrdName);
        }


        public static object ioLock = new object();
        /// <summary> Saves list of order permissions to disc. </summary>
        public static void Save()
        {
            try
            {
                lock (ioLock) SaveCore();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving " + Paths.OrdPermsFile, ex);
            }
        }

        public static void SaveCore()
        {
            using (StreamWriter w = new StreamWriter(Paths.OrdPermsFile))
            {
                WriteHeader(w, "order", "each order", "OrderName", "gun");

                foreach (OrderPerms perms in List)
                {
                    w.WriteLine(perms.Serialise());
                }
            }
        }


        /// <summary> Applies new order permissions to server state. </summary>
        public static void ApplyChanges()
        {
            // does nothing... for now anyways 
            //  (may be required if p.CanUse is changed to instead
            //   use a list of usable orders as a field instead)
        }


        /// <summary> Loads list of order permissions from disc. </summary>
        public static void Load()
        {
            lock (ioLock) LoadCore();
            ApplyChanges();
        }

        public static void LoadCore()
        {
            if (!File.Exists(Paths.OrdPermsFile))
            {
                Save();
                return;
            }

            using (StreamReader r = new StreamReader(Paths.OrdPermsFile))
            {
                ProcessLines(r);
            }
        }

        public static void ProcessLines(StreamReader r)
        {
            string[] args = new string[4];
            OrderPerms perms;
            string line;

            while ((line = r.ReadLine()) != null)
            {
                if (line.IsCommentLine()) continue;
                // Format - Name : Lowest : Disallow : Allow
                line.Replace(" ", "").FixedSplit(args, ':');

                try
                {

                    Deserialise(args, 1, out LevelPermission min, out List<LevelPermission> allowed, out List<LevelPermission> disallowed);
                    perms = GetOrAdd(args[0], min);
                    perms.Init(min, allowed, disallowed);
                }
                catch
                {
                    Logger.Log(LogType.Warning, "Hit an error on the order " + line);
                    continue;
                }
            }
        }
    }
    public static class AddedFormatter
    {
        public static void PrintOrderInfo(Player p, Order ord)
        {
            p.Message("Usable by: " + ord.Permissions.Describe());
            PrintAliases(p, ord);
            List<OrderExtraPerms> extraPerms = OrderExtraPerms.FindAll(ord.Name);
            if (ord.OrdExtraPerms == null)
            {
                extraPerms.Clear();
            }
            if (extraPerms.Count == 0)
            {
                return;
            }
            p.Message("&TExtra permissions:");
            foreach (OrderExtraPerms extra in extraPerms)
            {
                p.Message("{0}) {1} {2}", extra.Num, extra.Describe(), extra.Desc);
            }
        }
        public static void PrintAliases(Player p, Order ord)
        {
            StringBuilder dst = new StringBuilder("Shortcuts: &T");
            if (!string.IsNullOrEmpty(ord.Shortcut))
            {
                dst.Append('/').Append(ord.Shortcut).Append(", ");
            }
            FindAliases(Alias.aliases, ord, dst);
            if (dst.Length == "Shortcuts: &T".Length)
            {
                return;
            }
            p.Message(dst.ToString(0, dst.Length - 2));
        }
        public static void FindAliases(List<Alias> aliases, Order ord, StringBuilder dst)
        {
            foreach (Alias a in aliases)
            {
                if (!a.Target.CaselessEq(ord.Name))
                {
                    continue;
                }
                dst.Append('/').Append(a.Trigger);
                if (a.Format == null)
                {
                    dst.Append(", ");
                    continue;
                }
                string name = string.IsNullOrEmpty(ord.Shortcut) ? ord.Name : ord.Shortcut;
                if (name.Length > ord.Name.Length)
                {
                    name = ord.Name;
                }
                string args = a.Format.Replace("{args}", "[args]");
                dst.Append(" for /").Append(name + " " + args);
                dst.Append(", ");
            }
        }
    }
    public struct OrderPerm
    {
        public LevelPermission Perm;
        public string Description;

        public OrderPerm(LevelPermission perm, string desc)
        {
            Perm = perm;
            Description = desc;
        }
        public static bool operator ==(OrderPerm a, OrderPerm[] permB)
        {
            foreach (OrderPerm b in permB)
            {
                return a.Perm == b.Perm && a.Description == b.Description;
            }
            return false;
        }
        public static bool operator !=(OrderPerm a, OrderPerm[] permB)
        {
            foreach (CommandPerm b in permB)
            {
                return a.Perm != b.Perm && a.Description != b.Description;
            }
            return false;
        }
        public static bool operator ==(OrderPerm[] permA, OrderPerm b)
        {
            foreach (OrderPerm a in permA)
            {
                return a.Perm == b.Perm && a.Description == b.Description;
            }
            return false;
        }
        public static bool operator !=(OrderPerm[] permA, OrderPerm b)
        {
            foreach (OrderPerm a in permA)
            {
                return a.Perm != b.Perm && a.Description != b.Description;
            }
            return false;
        }
        public static bool operator ==(OrderPerm a, CommandPerm[] permB)
        {
            foreach (CommandPerm b in permB)
            {
                return a.Perm == b.Perm && a.Description == b.Description;
            }
            return false;
        }
        public static bool operator !=(OrderPerm a, CommandPerm[] permB)
        {
            foreach (CommandPerm b in permB)
            {
                return a.Perm != b.Perm && a.Description != b.Description;
            }
            return false;
        }
        public static explicit operator OrderPerm(CommandPerm v)
        {
            OrderPerm perm = new OrderPerm();
            perm.Perm = v.Perm;
            perm.Description = v.Description;
            return perm;
        }
        public static explicit operator OrderPerm(CommandPerm[] cmdPerms)
        {
            OrderPerm perm = new OrderPerm();
            foreach (CommandPerm v in cmdPerms)
            {
                perm.Perm = v.Perm;
                perm.Description = v.Description;
                return perm;
            }
            return perm;
        }
    }

    public struct OrderDesignation : IEnumerable<OrderDesignation>
    {
        public string Trigger, Format;
        public OrderDesignation(string ord, string format = null)
        {
            Trigger = ord;
            Format = format;
        }
        public IEnumerator<OrderDesignation> GetEnumerator()
        {
            Order ord = new Order();
            return ord.designations.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            Order ord = new Order();
            return ord.designations.Values.GetEnumerator();
        }
        public static bool operator ==(OrderDesignation a, CommandAlias[] permB)
        {
            foreach (CommandAlias b in permB)
            {
                return a.Trigger == b.Trigger && a.Format == b.Format;
            }
            return false;
        }
        public static bool operator !=(OrderDesignation a, CommandAlias[] permB)
        {
            foreach (CommandAlias b in permB)
            {
                return a.Trigger != b.Trigger && a.Format != b.Format;
            }
            return false;
        }
    }
    public enum OrderParallelism
    {
        NoAndSilent, NoAndWarn, Yes
    }
    public class Designation
    {
        public static CommandAlias[] ORDToCMD(params OrderDesignation[] designations)
        {
            CommandAlias[] cmdAliases = new CommandAlias[]
            {
            };
            foreach (OrderDesignation designation in designations)
            {
                CommandAlias cmdAlias = new CommandAlias();
                cmdAlias.Trigger = designation.Trigger;
                cmdAlias.Format = designation.Format;
                cmdAliases = new CommandAlias[]
                {
                    cmdAlias
                };
                return cmdAliases;
            }
            return cmdAliases;
        }
        public static OrderDesignation[] CMDToORD(params CommandAlias[] aliases)
        {
            OrderDesignation[] ordDesignations = new OrderDesignation[]
            {
            };
            if (aliases == null)
            {
                return ordDesignations;
            }
            foreach (CommandAlias alias in aliases)
            {
                OrderDesignation ordDesignation = new OrderDesignation();
                ordDesignation.Trigger = alias.Trigger;
                ordDesignation.Format = alias.Format;
                ordDesignations = new OrderDesignation[]
                {
                    ordDesignation
                };
                return ordDesignations;
            }
            return ordDesignations;
        }
        public static List<Designation> coreDesignations = new List<Designation>();
        public static List<Designation> designations = new List<Designation>();
        public string Trigger, Target, Format;

        public Designation(string trigger, string target)
        {
            Trigger = trigger;
            target = target.Trim();
            int space = target.IndexOf(' ');

            if (space < 0)
            {
                Target = target;
            }
            else
            {
                Target = target.Substring(0, space);
                Format = target.Substring(space + 1);
            }
        }

        public Designation(string trigger, string target, string format)
        {
            Trigger = trigger;
            Target = target;
            Format = format;
        }

        public static void LoadCustom()
        {
            designations.Clear();

            if (!File.Exists(Paths.DesignationsFile))
            {
                SaveCustom();
                return;
            }
            PropertiesFile.Read(Paths.DesignationsFile, LineProcessor, ':');
        }

        public static void LineProcessor(string key, string value)
        {
            designations.Add(new Designation(key, value));
        }

        public static void SaveCustom()
        {
            using (StreamWriter sw = new StreamWriter(Paths.DesignationsFile))
            {
                sw.WriteLine("# Aliases can be in one of three formats:");
                sw.WriteLine("# trigger : order");
                sw.WriteLine("#    e.g. \"xyz : help\" means /xyz is treated as /help <args given by user>");
                sw.WriteLine("# trigger : order [prefix]");
                sw.WriteLine("#    e.g. \"xyz : help me\" means /xyz is treated as /help me <args given by user>");
                sw.WriteLine("# trigger : order <prefix> {args} <suffix>");
                sw.WriteLine("#    e.g. \"mod : setrank {args} mod\" means /mod is treated as /setrank <args given by user> mod");

                foreach (Designation d in designations)
                {
                    if (d.Format == null)
                    {
                        sw.WriteLine(d.Trigger + " : " + d.Target);
                    }
                    else
                    {
                        sw.WriteLine(d.Trigger + " : " + d.Target + " " + d.Format);
                    }
                }
            }
        }

        public static Designation Find(string ord)
        {
            foreach (Designation designation in designations)
            {
                if (designation.Trigger.CaselessEq(ord)) return designation;
            }
            foreach (Designation designation in coreDesignations)
            {
                if (designation.Trigger.CaselessEq(ord)) return designation;
            }
            return null;
        }

        /// <summary> Registers default designations specified by an order. </summary>
        public static void RegisterDefaults(Order ord)
        {
            OrderDesignation[] designations = ord.Designations;
            if (designations == null) return;

            foreach (OrderDesignation d in designations)
            {
                Designation designation = new Designation(d.Trigger, ord.Name, d.Format);
                coreDesignations.Add(designation);
            }
        }

        public static void UnregisterDefaults(Order ord)
        {
            if (ord.Designations == null) return;
            coreDesignations.RemoveAll(o => o.Target == ord.Name);
        }
    }
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
        public virtual OrderDesignation[] Designations
        {
            get { return new[] { new OrderDesignation("ordsList") }; }
        }
        public override void Execute(Player p, string message)
        {
            if (!ListOrders(p, message))
            {
                OrderHelp(p);
            }
        }
        public static bool ListOrders(Player p, string message)
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
                string category = MapCategory(o.Type);
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
                categories[MapCategory(ord.Type)] = true;
            }

            List<string> list = new List<string>(categories.Keys);
            list.Sort();
            return list.Join(" ");
        }

        public override void OrderHelp(Player p)
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
        public override bool OrderUseableWhenFrozen { get { return true; } }
        public override void Execute(Player p, string message)
        {
            string[] args = message.SplitSpaces(3);
            if (args.Length < 2)
            {
                OrderHelp(p);
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
                OrderHelp(p);
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
            List<string> commands = Wildcard.Filter(Command.allCmds, keyword, cmd => cmd.name,
                                                     null, GetColoredName);
            List<string> shortcuts = Wildcard.Filter(Command.allCmds, keyword, cmd => cmd.shortcut,
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


        public override void OrderHelp(Player p)
        {
            p.Message("&T/OrdSearch [list] [keyword]");
            p.Message("&HFinds entries in a list that match the given keyword");
            p.Message("&H  keyword can also include wildcard characters:");
            p.Message("&H    * - placeholder for zero or more characters");
            p.Message("&H    ? - placeholder for exactly one character");
            p.Message("&HLists: &fblocks/commands/orders/ranks/players/online/loaded/maps");
        }
    }
    public enum OrderContext : byte
    {
        Normal, Static, SendOrd, Purchase, MessageBlock
    }

    public struct OrderData
    {
        public LevelPermission OrderRank;
        public OrderContext orderContext;
        public Vec3S32 OrderMBCoords;
        public static explicit operator OrderData(CommandData v)
        {
            OrderData orderData = new OrderData();
            orderData.OrderRank = v.Rank;
            orderData.orderContext = (OrderContext)v.Context;
            orderData.OrderMBCoords = v.MBCoords;
            return orderData;
        }
    }
    public partial class Order 
    {
        public Dictionary<string, OrderDesignation> designations = new Dictionary<string, OrderDesignation>();

        public static bool IsCreateAction(string str)
        {
            return str.CaselessEq("create") || str.CaselessEq("add") || str.CaselessEq("new");
        }

        public static bool IsDeleteAction(string str)
        {
            return str.CaselessEq("del") || str.CaselessEq("delete") || str.CaselessEq("remove");
        }

        public static bool IsEditAction(string str)
        {
            return str.CaselessEq("edit") || str.CaselessEq("change") || str.CaselessEq("modify")
                || str.CaselessEq("move") || str.CaselessEq("update");
        }

        public static bool IsInfoAction(string str)
        {
            return str.CaselessEq("about") || str.CaselessEq("info") || str.CaselessEq("status")
                || str.CaselessEq("check");
        }

        public static bool IsListAction(string str)
        {
            return str.CaselessEq("list") || str.CaselessEq("view");
        }
        public static string GetColoredName(Order ord)
        {
            LevelPermission perm = ord.Permissions.MinRank;
            return Group.GetColor(perm) + ord.Name;
        }
        public OrderPerms Permissions;
        public virtual OrderPerm[] OrdExtraPerms { get { return null; } }
        public virtual string Name { get; set; }
        public virtual string Shortcut { get { return ""; } }
        public virtual string Type { get { return ""; } }
        public virtual bool MuseumUsable { get { return true; } }
        public virtual bool ShowOrderInfo { get { return true; } }
        public virtual LevelPermission DefaultRank { get { return LevelPermission.Guest; } }
        public virtual void Execute(Player p, string message)
        {
        }
        public virtual OrderDesignation[] Designations { get { return null; } }

        public virtual void Execute(Player p, string message, OrderData data)
        {
            Execute(p, message);
        }
        public virtual void OrderHelp(Player p)
        {
            p.Message("No help is available for this order.");
        }
        public virtual void OrderHelp(Player p, string message)
        {
            OrderHelp(p);
            AddedFormatter.PrintOrderInfo(p, this);
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
        public virtual OrderParallelism OrdParallelism
        {
            get
            {
                return Type.CaselessEq(OrderTypes.Information) ? OrderParallelism.NoAndWarn : OrderParallelism.Yes; 
            }
        }
        public static List<Order> allOrds = new List<Order>();
        public static void InitAll()
        {
            Command.InitAll();
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
            allOrds.Add(ord);
            ord.Permissions = OrderPerms.GetOrAdd(ord.Name, ord.DefaultRank);
            OrderPerm[] extra = ord.OrdExtraPerms;
            if (extra != null)
            {
                for (int i = 0; i < extra.Length; i++)
                {
                    OrderExtraPerms exPerms = OrderExtraPerms.GetOrAdd(ord.Name, i + 1, extra[i].Perm);
                    exPerms.Desc = extra[i].Description;
                }
            }
            Designation.RegisterDefaults(ord);
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
            bool removed = allOrds.Remove(ord);

            if (ord != null)
            {
                Designation.UnregisterDefaults(ord);
            }
            return removed;
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
            return FindCMD(name);
        }
        public static Command FindCMD(string name)
        {
            foreach (Command cmd in Command.allCmds)
            {
                if (cmd.name.CaselessEq(name))
                {
                    return cmd;
                }
            }
            return null;
        }
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
        public const string Added = "Added";
        public const string Building = "Building";
        public const string Chat = "Chat";
        public const string Economy = "Economy";
        public const string Games = "Games";
        public const string Information = "Info";
        public const string Moderation = "Moderation";
        public const string Other = "Other";
        public const string World = "World";
        public const string Order = "Order";
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
        public override OrderDesignation[] Designations
        {
            get
            {
                return new[]
                {
                    new OrderDesignation("ACreate", "addon")
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
        public override void OrderHelp(Player p)
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
        public override OrderDesignation[] Designations
        {
            get
            {
                return new[]
                {
                    new OrderDesignation("ACompile", "addon")
                };
            }
        }
        public override bool OrderMessageBlockRestricted
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
                OrderHelp(p);
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
        public override void OrderHelp(Player p)
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
        public override OrderDesignation[] Designations
        {
            get
            {
                return new[]
                {
                    new OrderDesignation("OrdCompLoad"),
                    new OrderDesignation("Ordercml")
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
        public override void OrderHelp(Player p)
        {
            p.Message("&T/CompLoad [order]");
            p.Message("&HCompiles and loads (or reloads) a C# order into the server");
            p.Message("&T/CompLoad addon [addon]");
            p.Message("&HCompiles and loads (or reloads) a C# addon into the server");
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
    /// <summary> Compiles C# source files into a .dll by invoking a compiler executable directly </summary>
    public abstract class CommandLineCompiler
    {
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, List<string> referenced)
        {
            string args = GetCommandLineArguments(srcPaths, dstPath, referenced);
            string exe = GetExecutable();

            ICompilerErrors errors = new ICompilerErrors();
            List<string> output = new List<string>();
            int retValue = Compile(exe, GetCompilerArgs(exe, args), output);

            // Only look for errors/warnings if the compile failed
            // TODO still log warnings anyways error when success?
            if (retValue != 0)
            {
                foreach (string line in output)
                {
                    ProcessCompilerOutputLine(errors, line);
                }
            }
            return errors;
        }


        public virtual string GetCommandLineArguments(string[] srcPaths, string dstPath,
                                                         List<string> referencedAssemblies)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/t:library ");

            sb.Append("/utf8output /noconfig /fullpaths ");

            AddCoreAssembly(sb);
            AddReferencedAssemblies(sb, referencedAssemblies);
            sb.AppendFormat("/out:{0} ", Quote(dstPath));
            sb.Append("/optimize- ");
            sb.Append("/warnaserror- /unsafe ");

            foreach (string path in srcPaths)
            {
                sb.AppendFormat("{0} ", Quote(path));
            }
            return sb.ToString();
        }

        public virtual void AddCoreAssembly(StringBuilder sb)
        {
            string coreAssemblyFileName = typeof(object).Assembly.Location;

            if (!string.IsNullOrEmpty(coreAssemblyFileName))
            {
                sb.Append("/nostdlib+ ");
                sb.AppendFormat("/R:{0} ", Quote(coreAssemblyFileName));
            }
        }

        public abstract void AddReferencedAssemblies(StringBuilder sb, List<string> referenced);

        public static string Quote(string value)
        {
            return "\"" + value.Trim() + "\"";
        }

        public abstract string GetExecutable();
        public abstract string GetCompilerArgs(string exe, string args);


        public static int Compile(string path, string args, List<string> output)
        {
            // https://stackoverflow.com/questions/285760/how-to-spawn-a-process-and-capture-its-stdout-in-net
            ProcessStartInfo psi = CreateStartInfo(path, args);

            using (Process p = new Process())
            {
                p.OutputDataReceived += (s, e) => { if (e.Data != null) output.Add(e.Data); };
                p.ErrorDataReceived += (s, e) => { }; // swallow stderr output

                p.StartInfo = psi;
                p.Start();

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (!p.WaitForExit(120 * 1000))
                    throw new InvalidOperationException("C# compiler ran for over two minutes! Giving up..");

                return p.ExitCode;
            }
        }

        public static ProcessStartInfo CreateStartInfo(string path, string args)
        {
            ProcessStartInfo psi = new ProcessStartInfo(path, args)
            {
                WorkingDirectory = Context.CurrentDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return psi;
        }


        public static Regex outputRegWithFileAndLine;
        public static Regex outputRegSimple;

        public static void ProcessCompilerOutputLine(ICompilerErrors errors, string line)
        {
            if (outputRegSimple == null)
            {
                outputRegWithFileAndLine =
                    new Regex(@"(^(.*)(\(([0-9]+),([0-9]+)\)): )(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
                outputRegSimple =
                    new Regex(@"(error|warning) ([A-Z]+[0-9]+) ?: (.*)");
            }

            //First look for full file info
            Match m = outputRegWithFileAndLine.Match(line);
            bool full;
            if (m.Success)
            {
                full = true;
            }
            else
            {
                m = outputRegSimple.Match(line);
                full = false;
            }

            if (!m.Success) return;
            ICompilerError ce = new ICompilerError();

            if (full)
            {
                ce.FileName = m.Groups[2].Value;
                ce.Line = NumberUtils.ParseInt32(m.Groups[4].Value);
                ce.Column = NumberUtils.ParseInt32(m.Groups[5].Value);
            }

            ce.IsWarning = m.Groups[full ? 6 : 1].Value.CaselessEq("warning");
            ce.ErrorNumber = m.Groups[full ? 7 : 2].Value;
            ce.ErrorText = m.Groups[full ? 8 : 3].Value;
            errors.Add(ce);
        }
    }

    public class ClassicCSharpCompiler : CommandLineCompiler
    {
        public override void AddCoreAssembly(StringBuilder sb)
        {
            string coreAssemblyFileName = typeof(object).Assembly.Location;

            if (!string.IsNullOrEmpty(coreAssemblyFileName))
            {
                sb.Append("/nostdlib+ ");
                sb.AppendFormat("/R:{0} ", Quote(coreAssemblyFileName));
            }
        }

        public override void AddReferencedAssemblies(StringBuilder sb, List<string> referenced)
        {
            foreach (string path in referenced)
            {
                sb.AppendFormat("/R:{0} ", Quote(path));
            }
        }


        public override string GetExecutable()
        {
            string root = RuntimeEnvironment.GetRuntimeDirectory();

            string[] paths = new string[] {
                // First try new C# compiler
                Path.Combine(root, "csc.exe"),
                // Then fallback to old Mono C# compiler
                Path.Combine(root, @"../../../bin/mcs"),
                Path.Combine(root, "mcs.exe"),
                "/usr/bin/mcs",
            };

            foreach (string path in paths)
            {
                if (File.Exists(path)) return path;
            }
            return paths[0];
        }

        public override string GetCompilerArgs(string exe, string args)
        {
            return args;
        }
    }
    public class CSCompiler : ICompiler
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "C#"; } }
        public override string FullName { get { return "CSharp"; } }

        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
        {
            List<string> referenced = ProcessInput(srcPaths, "//");

            CommandLineCompiler compiler = new ClassicCSharpCompiler();
            return compiler.Compile(srcPaths, dstPath, referenced);
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
using Flames.Added;

public class Ord{0} : Order
{{
\t// The order's name (what you put after a slash to use this order)
\tpublic override string Name {{ get {{ return ""{0}""; }} }}

\t// Order's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""c"")
\tpublic override string Shortcut {{ get {{ return """"; }} }}

\t// Which submenu this order displays in under /Help
\tpublic override string Type {{ get {{ return ""order""; }} }}

\t// Whether or not this order can be used in a museum. Block/map altering orders should return false to avoid errors.
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
                return @"//\tAuto-generated addon skeleton class
//\tUse this as a basis for custom Flames addons

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;

namespace Flames.Added
{{
\tpublic class {0} : Addon
\t{{
\t\t// The addon's name (i.e what shows in /Addons)
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}

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
            if (name.Length == 0) return ICompiler.Compilers[0];

            foreach (ICompiler comp in ICompiler.Compilers)
            {
                if (comp.ShortName.CaselessEq(name)) return comp;
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
            string creator = p.IsSuper ? Colors.Strip(Server.Config.Name) : p.truename;
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


        /// <summary> Attempts to compile the given source code files into a .dll </summary>
        /// <param name="p"> Player to send messages to </param>
        /// <param name="type"> Type of files being compiled (e.g. Addon) </param>
        /// <param name="srcs"> Path of the source code files </param>
        /// <param name="dst"> Path to the destination .dll </param>
        /// <returns> Whether compilation succeeded </returns>
        public static bool Compile(Player p, ICompiler compiler, string type, string[] srcs, string dst)
        {
            foreach (string path in srcs)
            {
                if (File.Exists(path)) continue;

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
                if (logged >= MAX_LOG) break;
            }

            if (logged < errors.Count)
            {
                p.Message(" &W.. and {0} more", errors.Count - logged);
            }
            p.Message("&WCompiling failed. See " + ICompiler.ERROR_LOG_PATH + " for more detail");
        }
    }
    /// <summary> Compiles source code files for a particular programming language into a .dll </summary>
    public abstract class ICompiler
    {
        public const string ORDERS_SOURCE_DIR = "orders/";
        public const string ADDONS_SOURCE_DIR = "addons/";
        public const string ERROR_LOG_PATH = "logs/errors/compiler.log";

        /// <summary> Default file extension used for source code files </summary>
        /// <example> .cs, .vb </example>
        public abstract string FileExtension { get; }
        /// <summary> The short name of this programming language </summary>
        /// <example> C#, VB </example>
        public abstract string ShortName { get; }
        /// <summary> The full name of this programming language </summary>
        /// <example> CSharp, Visual Basic </example>
        public abstract string FullName { get; }
        /// <summary> Returns source code for an example Order </summary>
        public abstract string OrderSkeleton { get; }
        /// <summary> Returns source code for an example Addon </summary>
        public abstract string AddonSkeleton { get; }

        public string OrderPath(string name) { return ORDERS_SOURCE_DIR + "Ord" + name + FileExtension; }
        public string AddonPath(string name) { return ADDONS_SOURCE_DIR + name + FileExtension; }

        public static List<ICompiler> Compilers = new List<ICompiler>() {
            new CSCompiler()
        };


        public static string FormatSource(string source, params string[] args)
        {
            // Always use \r\n line endings so it looks correct in Notepad
            source = source.Replace(@"\t", "\t");
            source = source.Replace("\n", "\r\n");
            return string.Format(source, args);
        }

        /// <summary> Generates source code for an example order, 
        /// preformatted with the given order name </summary>
        public string GenExampleOrder(string ordName)
        {
            ordName = ordName.ToLower().Capitalize();
            return FormatSource(OrderSkeleton, ordName);
        }

        /// <summary> Generates source code for an example addon, 
        /// preformatted with the given name and creator </summary>
        public string GenExampleAddon(string addon, string creator)
        {
            return FormatSource(AddonSkeleton, addon, creator, Server.Version);
        }


        /// <summary> Attempts to compile the given source code files to a .dll file. </summary>
        /// <param name="logErrors"> Whether to log compile errors to ERROR_LOG_PATH </param>
        public ICompilerErrors Compile(string[] srcPaths, string dstPath, bool logErrors)
        {
            ICompilerErrors errors = DoCompile(srcPaths, dstPath);
            if (!errors.HasErrors || !logErrors) return errors;

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

                if (err.Line > 0) sb.AppendLine(sources.Get(err.FileName, err.Line - 1));
                if (err.Column > 0) sb.Append(' ', err.Column - 1);
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

            // Include filename if compiling multiple source code files
            return string.Format("{0}{1}{2}{3}", type, text,
                                 err.Line > 0 ? " on line " + err.Line : "",
                                 srcs.Length > 1 ? " in " + file : "");
        }


        /// <summary> Compiles the given source code. </summary>
        public abstract ICompilerErrors DoCompile(string[] srcPaths, string dstPath);


        /// <summary> Converts source file paths to full paths, 
        /// then returns list of parsed referenced assemblies </summary>
        public List<string> ProcessInput(string[] srcPaths, string commentPrefix)
        {
            List<string> referenced = new List<string>();

            for (int i = 0; i < srcPaths.Length; i++)
            {
                // CodeDomProvider doesn't work properly with relative paths
                string path = Path.GetFullPath(srcPaths[i]);

                AddReferences(path, commentPrefix, referenced);
                srcPaths[i] = path;
            }

            referenced.Add(Server.GetServerDLLPath());
            return referenced;
        }

        public void AddReferences(string path, string commentPrefix, List<string> referenced)
        {
            // Allow referencing other assemblies using '//reference [assembly name]' at top of the file
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
                        ProcessInputLine(line, referenced);
                    }
                }
            }
        }

        public virtual void ProcessInputLine(string line, List<string> referenced) { }

        public static string GetDLL(string line)
        {
            int index = line.IndexOf(' ') + 1;
            // For consistency with C#, treat '//reference X.dll;' as '//reference X.dll'
            return line.Substring(index).Replace(";", "");
        }
    }

    public class ICompilerErrors : List<ICompilerError>
    {
        public bool HasErrors
        {
            get { return FindIndex(ce => !ce.IsWarning) >= 0; }
        }
    }

    public class ICompilerError
    {
        public int Line, Column;
        public string ErrorNumber, ErrorText;
        public bool IsWarning;
        public string FileName;
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
                if (file.CaselessEq(files[i])) return i;
            }
            return -1;
        }

        /// <summary> Returns the given line in the given source code file </summary>
        public string Get(string file, int line)
        {
            int i = FindFile(file);
            if (i == -1) return "";

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
        public override bool OrderMessageBlockRestricted { get { return true; } }
        public override void Execute(Player p, string ordName)
        {
            if (ordName.Length == 0)
            {
                OrderHelp(p);
                return;
            }
            if (!Formatter.ValidFilename(p, ordName))
            {
                return;
            }
            string path = IScripting.OrderPath(ordName);
            ScriptingOperations.LoadOrders(p, path);
        }
        public override void OrderHelp(Player p)
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
        public override bool OrderMessageBlockRestricted { get { return true; } }
        public override void Execute(Player p, string ordName)
        {
            if (ordName.Length == 0)
            {
                OrderHelp(p);
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
        public override void OrderHelp(Player p)
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
        public override OrderDesignation[] Designations
        {
            get
            {
                return new[]
                {
                    new OrderDesignation("ALoad", "load"),
                    new OrderDesignation("AUnload", "unload"),
                    new OrderDesignation("Addons", "list")
                };
            }
        }
        public override bool OrderMessageBlockRestricted
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
                OrderHelp(p);
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
                OrderHelp(p);
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
        public override void OrderHelp(Player p)
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
    public static class UIHelpers
    {
        public static string lastORD = "";
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
        public static void RepeatOrder()
        {
            if (lastORD.Length == 0)
            {
                Logger.Log(LogType.OrderUsage, "(Flames): Cannot repeat order - no orders issued yet.");
                return;
            }
            Logger.Log(LogType.OrderUsage, "Repeating &T/" + lastORD);
            HandleOrder(lastORD);
        }
        public static void HandleOrder(string text)
        {
            if (text != null)
            {
                text = text.Trim();
            }
            if (string.IsNullOrEmpty(text))
            {
                Logger.Log(LogType.OrderUsage, "(Flames): Whitespace orders are not allowed.");
                return;
            }
            if (text[0] == '/' && text.Length > 1)
            {
                text = text.Substring(1);
            }
            lastORD = text;
            string name, args;
            text.Separate(' ', out name, out args);
            Order.Search(ref name, ref args);
            if (Server.CheckOrders(name, args))
            {
                Server.cancelorder = false;
                if (Server.Check(name, args))
                {
                    Server.cancelcommand = false;
                    return;
                }
            }
            Order ord = Order.FindORD(name);
            if (ord == null)
            {
                Logger.Log(LogType.OrderUsage, "(Flames): Unknown order \"{0}\"", name);
                return;
            }
            if (ord != null)
            {
                if (!ord.OrderSuperUseable)
                {
                    Logger.Log(LogType.OrderUsage, "(Flames): /{0} can only be used in-game.", ord.Name);
                    return;
                }
                Thread thread;
                Server.StartThread(out thread, "FlamesORD_" + name,
                    () =>
                    {
                        try
                        {
                            ord.Execute(Player.Flame, args);
                            if (args.Length == 0)
                            {
                                Logger.Log(LogType.OrderUsage, "(Flames) used /" + ord.Name);
                            }
                            else
                            {
                                Logger.Log(LogType.OrderUsage, "(Flames) used /" + ord.Name + " " + args);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                            Logger.Log(LogType.OrderUsage, "(Flames): FAILED TO FOLLOW ORDER!");
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