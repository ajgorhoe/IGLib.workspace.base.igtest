using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

using System.Windows.Forms;

using IG.Lib; 
using IG.Forms;

namespace IG.Forms
{


    public class TestLoadableScriptInterpreter
    {

        public static void RunTest()
        {

            string scriptPath1 = @"../../loadablescript\tests\data\script1.cms";
            string scriptPath2 = @"../../loadablescript\tests\data\script1copy.cs";

            LoadableScriptInterpreterBase scriptInterpreter = new LoadableScriptInterpreterBase();

            scriptInterpreter.ScriptLoader.AddReferencedAssemblies(
                "System.Windows.Forms.dll"
            );

            scriptInterpreter.ScriptLoader.RunFile(scriptPath1, new string[] { "arg1", "arg2", "arg3" });


        }

    }

}