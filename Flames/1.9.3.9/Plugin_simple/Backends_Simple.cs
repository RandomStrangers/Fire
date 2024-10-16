/*
    Copyright 2010 MCLawl Team - Written by Valek (Modified by MCGalaxy)

    Edited for use with MCGalaxy
 
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System.CodeDom.Compiler;


namespace Flames.Scripting
{
    public sealed class CSCompiler_Simple : ICodeDomCompiler_Simple
    {
        public override string FileExtension { get { return ".cs"; } }
        public override string ShortName { get { return "CS"; } }
        public override string FullName { get { return "CSharp"; } }

        public override CodeDomProvider CreateProvider()
        {

            return CodeDomProvider.CreateProvider("CSharp");
        }

        public override void PrepareArgs(CompilerParameters args)
        {
            args.CompilerOptions += " /unsafe";
        }


        public override string SimplePluginSkeleton
        {
            get
            {
                return @"//This is an example simple plugin source!
using System;
namespace Flames
{{
\tpublic class {0} : Plugin_Simple
\t{{
\t\tpublic override string name {{ get {{ return ""{0}""; }} }}
\t\tpublic override string creator {{ get {{ return ""{1}""; }} }}

\t\tpublic override void Load(bool startup)
\t\t{{
\t\t\t//LOAD YOUR SIMPLE PLUGIN WITH EVENTS OR OTHER THINGS!
\t\t}}
                        
\t\tpublic override void Unload(bool shutdown)
\t\t{{
\t\t\t//UNLOAD YOUR SIMPLE PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!
\t\t}}
                        
\t\tpublic override void Help(Player p)
\t\t{{
\t\t\t//HELP INFO!
\t\t}}
\t}}
}}";
            }
        }
    }

    public sealed class VBCompiler_Simple : ICodeDomCompiler_Simple
    {
        public override string FileExtension { get { return ".vb"; } }
        public override string ShortName { get { return "VB"; } }
        public override string FullName { get { return "Visual Basic"; } }

        public override CodeDomProvider CreateProvider()
        {

            return CodeDomProvider.CreateProvider("VisualBasic");
        }

        public override void PrepareArgs(CompilerParameters args) { }
        public override string CommentPrefix { get { return "'"; } }



        public override string SimplePluginSkeleton
        {
            get
            {
                return @"' This is an example simple plugin source!
Imports System

Namespace Flames
\tPublic Class {0}
\t\tInherits Plugin_Simple

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