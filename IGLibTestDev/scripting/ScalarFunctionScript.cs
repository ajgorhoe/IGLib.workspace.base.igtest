// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: output formats for numbers.
// Original filename: ScriptExtFormats

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

using IG.Num;
using IG.Lib;

namespace IG.Script
{


    public class LoadableScriptScalarFunctionCreator : LoadableScriptSpecialFunctionBase, ILoadableScript
    {

        public LoadableScriptScalarFunctionCreator()
            : base()
        { }



        #region Commands


        /// <summary>Name of the command that performs my custom test.</summary>
        public const string ConstMyTest = "MyTest";
        public const string ConstHelpMyTest = "Custom test function.";

        /// <summary>Name of the command for custom test.</summary>
        public const string ConstCustomApp = "CustomApp";
        public const string ConstHelpCustomApp = "Custom aplication.";

        #endregion Commands



        /// <summary>Adds a new internal script command under specified name to the internal interpreter of the current 
        /// script object.</summary>
        /// <param name="interpreter">Interpreter on which the command is added.</param>
        /// <param name="commandName">Name of the command. <para>Must not be null or empty string.</para></param>
        /// <param name="command">Method that executes the command. <para>Must not be null.</para></param>
        /// <param name="helpString">Help string associated with command, optionsl (can be null).</param>
        public override void Script_AddCommand(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings,
            string commandName, Script_CommandDelegate command, string helpString)
        {
            base.Script_AddCommand(interpreter, helpStrings, commandName, command, helpString);
        }


        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);

            Script_AddCommand(interpreter, helpStrings, ConstMyTest, AppMyTest, ConstHelpMyTest);
            Script_AddCommand(interpreter, helpStrings, ConstCustomApp, AppCustomApp, ConstHelpCustomApp);
        }

        #region Actions


        /// <summary>Test action.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string AppMyTest(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("MY CUSTOM TEST.");
            Console.WriteLine("This script is alive.");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine("==== END of my custom test.");
            Console.WriteLine();
            return null;
        }

        /// <summary>Custom application.</summary>
        public virtual string AppCustomApp(string[] arguments)
        {
            string ret = null;
            Console.WriteLine();
            Console.WriteLine("CUSTOM APPLICATION run from the APPLICATION SCRIPT.");
            Console.WriteLine("==============================");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            Console.WriteLine("==============================");
            Console.WriteLine("Custom application finished.");
            Console.WriteLine();
            return ret;
        }


        #endregion Actions


    }  // LoadableScriptScalarFunctionCreator



}
