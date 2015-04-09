// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: Different examples.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;

using IG.Plot2d;
using IG.Gr3d;

namespace IG.Script
{

    public class Script_Optimization : LoadableScriptBase, ILoadableScript
    {

        #region Standard_TestScripts

        public Script_Optimization()
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

        #endregion Standard_TestScripts

        #region Commands

        /// <summary>Tests of file operations.</summary>
        public const string ConstTestScriptArguments = "Test1.";
        public const string ConstHelpTestScriptArguments = "Performs test of arguments..";



        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstTestScriptArguments, TestScriptArguments, ConstHelpTestScriptArguments);
        }

        #endregion Commands


        #region Actions

        /// <summary>Test of file operations.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestScriptArguments(string[] arguments)
        {
            return this.Script_CommandTestScript(arguments);
        }  // TestArguments



        #endregion Actions

    }  // script
}

