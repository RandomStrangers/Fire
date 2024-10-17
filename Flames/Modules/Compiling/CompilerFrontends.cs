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
using Flames.Modules.Compiling;
using System.CodeDom.Compiler;

namespace Flames.Modules.Compiling
{
    public sealed class CSCompiler : ICompiler
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "C#"; } }
        public override string FullName { get { return "CSharp"; } }

        CodeDomProvider compiler;

        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
        {
            CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");
            args.CompilerOptions += " /unsafe";
            // NOTE: Make sure to keep CompilerOptions in sync with RoslynCSharpCompiler

            ICodeDomCompiler.InitCompiler(this, "CSharp", ref compiler);
            return ICodeDomCompiler.Compile(args, srcPaths, compiler);
        }

        public override string CommandSkeleton
        {
            get
            {
                return @"//\tAuto-generated command skeleton class
//\tUse this as a basis for custom Flames commands
//\tNaming should be kept consistent (e.g. /update command should have a class name of 'CmdUpdate' and a filename of 'CmdUpdate.cs')
// As a note, Flames is designed for .NET 4.8

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;
using Flames;

public class Cmd{0} : Command
{{
\t// The command's name (what you put after a slash to use this command)
\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t// Command's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""c"")
\tpublic override string shortcut {{ get {{ return """"; }} }}

\t// Which submenu this command displays in under /Help
\tpublic override string type {{ get {{ return ""other""; }} }}

\t// Whether or not this command can be used in a museum. Block/map altering commands should return false to avoid errors.
\tpublic override bool museumUsable {{ get {{ return true; }} }}

\t// The default rank required to use this command. Valid values are:
\t//   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
\t//   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
\tpublic override LevelPermission defaultRank {{ get {{ return LevelPermission.Guest; }} }}

\t// This is for when a player executes this command by doing /{0}
\t//   p is the player object for the player executing the command. 
\t//   message is the arguments given to the command. (e.g. for '/{0} this', message is ""this"")
\tpublic override void Use(Player p, string message)
\t{{
\t\tp.Message(""Hello World!"");
\t}}

\t// This is for when a player does /Help {0}
\tpublic override void Help(Player p)
\t{{
\t\tp.Message(""/{0} - Does stuff. Example command."");
\t}}
}}";
            }
        }

        public override string PluginSkeleton
        {
            get
            {
                return @"//\tAuto-generated plugin skeleton class
//\tUse this as a basis for custom Flames plugins

// To reference other assemblies, put a ""//reference [assembly filename]"" at the top of the file
//   e.g. to reference the System.Data assembly, put ""//reference System.Data.dll""

// Add any other using statements you need after this
using System;

namespace Flames
{{
\tpublic class {0} : Plugin
\t{{
\t\t// The plugin's name (i.e what shows in /Plugins)
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}

\t\t// The oldest version of Flames this plugin is compatible with
\t\tpublic override string Flames_Version {{ get {{ return ""{2}""; }} }}

\t\t// Message displayed in server logs when this plugin is loaded
\t\tpublic override string welcome {{ get {{ return ""Loaded Message!""; }} }}

\t\t// Who created/authored this plugin
\t\tpublic override string creator {{ get {{ return ""{1}""; }} }}

\t\t// Called when this plugin is being loaded (e.g. on server startup)
\t\tpublic override void Load(bool startup)
\t\t{{
\t\t\t//code to hook into events, load state/resources etc goes here
\t\t}}

\t\t// Called when this plugin is being unloaded (e.g. on server shutdown)
\t\tpublic override void Unload(bool shutdown)
\t\t{{
\t\t\t//code to unhook from events, dispose of state/resources etc goes here
\t\t}}

\t\t// Displays help for or information about this plugin
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\tp.Message(""No help is available for this plugin."");
\t\t}}
\t}}
}}";
            }
        }
    }
    public sealed class VBCompiler : ICompiler
    {
        public override string FileExtension { get { return ".vb"; } }
        public override string ShortName { get { return "VB"; } }
        public override string FullName { get { return "Visual Basic"; } }

        CodeDomProvider compiler;

        public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
        {
            CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");
            args.CompilerOptions += " ";

            ICodeDomCompiler.InitCompiler(this, "VisualBasic", ref compiler);
            return ICodeDomCompiler.Compile(args, srcPaths, compiler);
        }
        public override string CommandSkeleton
        {
            get
            {
                return @"
Imports System
Imports Flames

Public Class Cmd{0}
    Inherits Command

    ' The command's name (what you put after a slash to use this command)
    Public Overrides ReadOnly Property name As String
        Get
            Return ""{0}""
        End Get
    End Property

    ' Command's shortcut, can be left blank (e.g. ""/Copy"" has a shortcut of ""c"")
    Public Overrides ReadOnly Property shortcut As String
        Get
            Return """"
        End Get
    End Property

    ' Which submenu this command displays in under /Help
    Public Overrides ReadOnly Property type As String
        Get
            Return ""other""
        End Get
    End Property

    ' Whether or not this command can be used in a museum. Block/map altering commands should return false to avoid errors.
    Public Overrides ReadOnly Property museumUsable As Boolean
        Get
            Return True
        End Get
    End Property

    ' The default rank required to use this command. Valid values are:
    '   LevelPermission.Guest, LevelPermission.Builder, LevelPermission.AdvBuilder,
    '   LevelPermission.Operator, LevelPermission.Admin, LevelPermission.Owner
    Public Overrides ReadOnly Property defaultRank As LevelPermission
        Get
            Return LevelPermission.Guest
        End Get
    End Property

    ' This is for when a player executes this command by doing /{0}
    '   p is the player object for the player executing the command. 
    '   message is the arguments given to the command. (e.g. for '/{0} this', message is ""this"")
    Public Overrides Sub Use(p As Player, message As String)
        p.Message(""Hello World!"")
    End Sub

    ' This is for when a player does /Help {0}
    Public Overrides Sub Help(p As Player)
        p.Message(""/{0} - Does stuff. Example command."")
    End Sub
End Class


";

            }
        }

        public override string PluginSkeleton
        {
            get
            {
                return @"' This is an example simple plugin source!
Imports System

Namespace Flames
\tPublic Class {0}
\t\tInherits Plugin

\t\tPublic Overrides ReadOnly Property name() As String
\t\t\tGet
\t\t\t\tReturn ""{0}""
\t\t\tEnd Get
\t\t End Property
\t\tPublic Overrides ReadOnly Property creator() As String
\t\t\tGet
\t\t\t\tReturn ""{1}""
\t\t\tEnd Get
\t\t End Property

\t\tPublic Overrides Sub Load(startup As Boolean)
\t\t\t' LOAD YOUR SIMPLE PLUGIN WITH EVENTS OR OTHER THINGS!
\t\tEnd Sub
                        
\t\tPublic Overrides Sub Unload(shutdown As Boolean)
\t\t\t' UNLOAD YOUR SIMPLE PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
\t\tEnd Sub
                        
\t\tPublic Overrides Sub Help(p As Player)
\t\t\t' HELP INFO!
\t\tEnd Sub
\tEnd Class
End Namespace";
            }
        }
    }
}
public sealed class JScriptCompiler : ICompiler
{
    public override string ShortName { get { return "JS"; } }
    public override string FullName { get { return "JScript"; } }
    public override string FileExtension { get { return ".jscript"; } }

    public CodeDomProvider compiler;
    public override ICompilerErrors DoCompile(string[] srcPaths, string dstPath)
    {
        CompilerParameters args = ICodeDomCompiler.PrepareInput(srcPaths, dstPath, "//");
        ICodeDomCompiler.InitCompiler(this, "JScript", ref compiler);
        return ICodeDomCompiler.Compile(args, srcPaths, compiler);
    }


    public override string CommandSkeleton
    {
        get
        {
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

    public override string PluginSkeleton
    {
        get
        {
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