// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TEMPLATE SCRIPT FILE for performing tests. 
// DO NOT MODIFY UNLESS YOU WANT TO IMPROVE THE TEMPLATE!

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;

namespace IG.Script
{

    /// <summary>A template script for running tests.
    /// <para>This is a test template.</para>
    /// <para>The first argument of the <see cref="Run"/> method defines which test is performed.</para></summary>
    public class ScriptOptTemplateExt : ScriptOptTemplate, ILoadableScript
    {
 
        #region Standard_TestScripts

        public ScriptOptTemplateExt()
            : base()
        { }

        /// <summary>Initializes the current object.</summary>
        protected override void InitializeThis(string[] arguments)
        {
            Script_DefaultInitialize(arguments);
        }

        /// <summary>Runs action of the current object.</summary>
        /// <param name="arguments">Command-line arguments of the action.</param>
        protected override string RunThis(string[] arguments)
        {
            return Script_DefaultRun(arguments);
        }

        public const string ConstMyTest = "MyTest";
        public const string ConstHelpMyTest = "Custom test.";

        public const string ConstAnotherTest = "AnotherTest";
        public const string ConstHelpAnotherTest = "Another custom test.";


        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstMyTest, TestMyTest, ConstHelpMyTest);
            Script_AddCommand(interpreter, helpStrings, ConstAnotherTest, TestAnotherTest, ConstHelpAnotherTest);
        }

        #endregion Standard_TestScripts


        #region Actions

        /// <summary>Test action.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string TestMyTest(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("MY CUSTOM TEST (in optimization template script).");
            Console.WriteLine("This script is alive.");
            //Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine("==== END of my custom test.");
            Console.WriteLine();
            return null;
        }

        /// <summary>Another test action.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        public string TestAnotherTest(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("This is ANOTHER TEST (in optimization template script)...");
            Console.WriteLine("Another test finished.");
            Console.WriteLine();

            return null;
        }  // TestFormats



        #endregion Actions



    }


}

