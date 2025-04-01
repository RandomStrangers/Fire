//reference System.dll
// ============================================================
// ============================================================
// NOTE: This plugin is for Microsoft JScript, NOT JAVASCRIPT
// ============================================================
// ============================================================
using System;
using System.CodeDom.Compiler;
using Flames.Modules.Compiling;
using Flames;

public class JScriptPlugin : Plugin
{
	public override string creator { get { return "UnknownShadow200"; } }
	public override string Flames_Version { get { return Server.Version; } }
	public override string name { get { return "JScriptPlugin"; } }

	public ICompiler compiler = new JSriptCompiler();
	public override void Load(bool startup) {
		ICompiler.Compilers.Add(compiler);
	}
	
	public override void Unload(bool shutdown) {
		ICompiler.Compilers.Remove(compiler);
	}
}

public class JSriptCompiler : ICompiler
{
	public override string ShortName { get { return "JS"; } }
	public override string FullName { get { return "JScript"; } }
	public override string FileExtension { get { return ".jscript"; } }

	public CodeDomProvider compiler;
	public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath) {
		CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");

		ICodeDomCompiler.InitCompiler(this, "JScript", ref compiler);
		return ICodeDomCompiler.Compile(args, srcPaths, compiler);
	}
	
	
	public override string CommandSkeleton {
		get {
			return @"//\tAuto-generated command skeleton class.
//\tUse this as a basis for custom Flames commands.
//\tNaming should be kept consistent. (e.g. /update command should have a class name of 'CmdUpdate' and a filename of 'CmdUpdate.cs')
// As a note, Flames is designed for .NET 4.8

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
import System;
import Flames;

class Cmd{0} extends Command
{{
\t// The command's name (what you put after a slash to use this command)
\toverride function get_name() : String {{ return ""{0}""; }}

\t// Command's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""c"")
\toverride function get_shortcut() : String {{ return """"; }}

\t// Which submenu this command displays in under /Help
\toverride function get_type() : String {{ return ""other""; }}

\t// Whether or not this command can be used in a museum. Block/map altering commands should return false to avoid errors.
\toverride function get_museumUsable() : boolean {{ return true; }}

\t// The default rank required to use this command. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\toverride function get_defaultRank() : LevelPermission {{ return LevelPermission.Guest; }}

\t// This is for when a player executes this command by doing /{0}
\t//   p is the player object for the player executing the command.
\t//   message is the arguments given to the command. (e.g. for '/{0} this', message is ""this"")
\toverride function Use(p : Player, message : String)
\t{{
\t\tp.Message(""Hello World!"");
\t}}

\t// This is for when a player does /Help {0}
\toverride function Help(p : Player)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example command."");
\t}}
}}";
		}
	}
	
	public override string PluginSkeleton {
		get {
			return @"//This is an example plugin source!
import System;
import Flames;

class {0} extends Plugin
{{
\toverride function get_name() : String {{ return ""{0}""; }}
\toverride function get_Flames_Version() : String {{ return ""{2}""; }}
\toverride function get_creator() : String {{ return ""{1}""; }}

\toverride function Load(startup : boolean)
\t{{
\t\t//LOAD YOUR PLUGIN WITH EVENTS OR OTHER THINGS!
\t}}
\t
\toverride function Unload(shutdown : boolean)
\t{{
\t\t//UNLOAD YOUR PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
\t}}
\t
\toverride function Help(p : Player)
\t{{
\t\t//HELP INFO!
\t\tp.Message(""No help available for '{0}' plugin"");
\t}}
}}";
		}
	}
}