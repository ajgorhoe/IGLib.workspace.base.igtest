// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: various examples.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;

using IG.Forms;
using IG.Plot2d;
using IG.Gr3d;
using IG.Neural;
using IG.Neural.Forms;
using IG.Neural.Forms.Old;
using IG.Neural.Applications;

namespace IG.Script
{


    /// <summary>Internal script for running embedded applications.</summary>
    /// <remarks>
    /// <para>In the applications that have the command-line interpreter, embedded applications from this class can typically be
    /// run in the following way:</para>
    /// <para>  AppName Internal IG.Script.AppShellExt CommandName arg1 arg2 ...</para>
    /// <para>where AppName is the application name, IG.Script.AppBase is the full name of the script class that contains
    /// embedded applications, CommandName is name of the command thar launches embedded application, and arg1, arg2, etc.
    /// are command arguments for the embedded application.</para></remarks>
    /// <seealso cref="ScriptAppBase"/>
    /// $A Igor Sep12;
    public class AppTestShellExt : AppTestShell, ILoadableScript
    {

        public AppTestShellExt()
            : base()
        { }

        

        #region Commands


        /// <summary>Name of the command for the file system-related embedded applications.</summary>
        public const string ConstMathematica = "Mathematica";
        public const string ConstHelpMathematica =
@"Various Mathematica-related embedded applications. 
  Run with the '?' argument to see which applications are available.";


        #endregion Commands



        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstMathematica, AppMathematica, ConstHelpMathematica);
        }


        #region Actions



        #region Actions.Develop_Inherited

        
        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected override void InitAppDevelop()
        {
            lock (Lock)
            {
                // Below, add new embedded applications for develop command.


                base.InitAppDevelop();
            }
        }
      
        #endregion Actions.Develop_Inherited



        #region Actions.Mathematica

        /// <summary>List of installed Mathematica - related demo command names.</summary>
        protected List<string> AppMathematicaNames = new List<string>();

        /// <summary>List of help strings corresponding to the installed Mathematica - related demo commands.</summary>
        protected List<string> AppMathematicaHelpStrings = new List<string>();

        /// <summary>List of methods used to perform Mathematica - related demo commmands.</summary>
        protected List<CommandMethod> AppMathematicaMethods = new List<CommandMethod>();

        /// <summary>Adds a new Mathematica - related embedded demo application's command (added as 
        /// a sub-command of the base command named <see cref="ConstMathematica"/>).</summary>
        /// <param name="appName">Application name.</param>
        /// <param name="appMethod">Method used to perform the application.</param>
        /// <param name="appHelp">Eventual help string for the application.</param>
        protected void AddMathematicaCommand(string appName, CommandMethod appMethod, string appHelp)
        {
            lock (Lock)
            {
                AppMathematicaNames.Add(appName.ToLower());
                AppMathematicaHelpStrings.Add(appHelp);
                AppMathematicaMethods.Add(appMethod);
            }
        }


        #region Actions.Mathematica.TestApp

        public const string MathematicaTestApp = "TestApp";

        protected const string MathematicaHelpTestApp = MathematicaTestApp + " : Runs the Mathematica simple console calculator.";

        /// <summary>Executes embedded application - demo application for Mathematica interface.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string MathematicaFunctionTestApp(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the Mathematica demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            
            //Application.Run(new ());

            MathematicaInterface.ExampleCalculator();


            Console.WriteLine(Environment.NewLine + "Mathematica demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.Mathematica.TestApp



        #region Actions.Mathematica.CalculatorForm

        public const string MathematicaCalculatorForm = "CalculatorForm";

        protected const string MathematicaHelpCalculatorForm = MathematicaCalculatorForm + " : Runs the Mathematica simple console calculator.";

        /// <summary>Executes embedded application - demo application for Mathematica interface.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string MathematicaFunctionCalculatorForm(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the Mathematica demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MathematicaCalculatorApp());


            Console.WriteLine(Environment.NewLine + "Mathematica demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.Mathematica.CalculatorForm



        #region Actions.Mathematica.CalculatorFormOld

        public const string MathematicaCalculatorFormOld = "CalculatorFormOld";

        protected const string MathematicaHelpCalculatorFormOld = MathematicaCalculatorFormOld + " : Runs the Mathematica simple console calculator, old version.";

        /// <summary>Executes embedded application - demo application for Mathematica interface.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string MathematicaFunctionCalculatorFormOld(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the Mathematica demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FormMathematicaCalculatorOld());


            Console.WriteLine(Environment.NewLine + "Mathematica demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.Mathematica.CalculatorFormOld



        protected bool _appMathematicaCommandsInitialized = false;

        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected virtual void InitAppMathematica()
        {

            lock (Lock)
            {
                if (_appMathematicaCommandsInitialized)
                    return;
                AddMathematicaCommand(MathematicaTestApp, MathematicaFunctionTestApp, MathematicaHelpTestApp);
                AddMathematicaCommand(MathematicaCalculatorForm, MathematicaFunctionCalculatorForm, MathematicaHelpCalculatorForm);
                AddMathematicaCommand(MathematicaCalculatorFormOld, MathematicaFunctionCalculatorFormOld, MathematicaHelpCalculatorFormOld);


                _appMathematicaCommandsInitialized = true;
            }
        }


        /// <summary>Runs a form demo - related utility (embedded application) according to arguments.</summary>
        /// <param name="args">Arguments. 0-th argument is the base command name, 1st argument is the embedded application name, and the rest
        /// are arguments that are used by the embedded application.</param>
        protected virtual string RunAppMathematica(string[] args)
        {
            InitAppMathematica();
            if (args == null)
                throw new ArgumentException("No arguments. Embedded application name (or '?' for help) should be specified (null argument array).");
            if (args.Length < 2)
                throw new ArgumentException("Test name (or '?' for help) should be specified (less than 2 arguments).");
            if (string.IsNullOrEmpty(args[1]))
                throw new ArgumentException("Test name (or '?' for help) not specified (null or empty string argument).");
            if (args != null)
                if (args.Length >= 2)
                    if (args[1] == "?")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usage: " + args[0] + " ApplicationName arg1 arg2...");
                        Console.WriteLine(args[0] + " ApplicationName ? : prints help.");
                        Console.WriteLine();
                        Console.WriteLine("List of embedded applications: ");
                        for (int i = 0; i < AppMathematicaNames.Count; ++i)
                            Console.WriteLine("  " + AppMathematicaNames[i]);
                        Console.WriteLine();
                        return null;
                    }
            string testName = args[1];
            string[] appParams = new string[args.Length - 2];
            for (int i = 0; i < appParams.Length; ++i)
                appParams[i] = args[i + 2];
            int index = AppMathematicaNames.IndexOf(testName.ToLower());
            if (index < 0)
                throw new ArgumentException("Embedded application named " + testName + " is not found. Call with '?' for list of application names.");
            testName = AppMathematicaNames[index];
            string helpString = AppMathematicaHelpStrings[index];
            CommandMethod method = AppMathematicaMethods[index];
            if (appParams.Length >= 1)
                if (appParams[0] == "?")
                {
                    // Print help for the specified surface:
                    Console.WriteLine();
                    Console.WriteLine("Test " + testName + ": ");
                    if (string.IsNullOrEmpty(AppMathematicaHelpStrings[index]))
                        Console.WriteLine("  No help available.");
                    Console.WriteLine(AppMathematicaHelpStrings[index]);
                    Console.WriteLine();
                    return null;
                }
            try
            {
                return method(testName, appParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("Usage: " + Environment.NewLine + helpString + Environment.NewLine);
                throw;
            }
        }

        /// <summary>Runs one of the form demo - related embedded applications.</summary>
        /// <param name="arguments">Array containing the base command name, application command name and arguments.</param>
        public virtual string AppMathematica(string[] arguments)
        {
            string ret = null;
            if (arguments == null)
                throw new ArgumentException("Commandline arguments not specified (null argument).");
            if (arguments.Length < 2)
                Console.WriteLine("Number of arguments should be at least 2 (at least base command name & application name).");
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Running form demo - related embedded application..."
                + Environment.NewLine +
                "=============================="
                + Environment.NewLine);
            //Console.WriteLine();
            //Script_PrintArguments("Application arguments: ", arguments);
            //Console.WriteLine();

            if (ret == null)
                ret = RunAppMathematica(arguments);


            Console.WriteLine("==============================");
            Console.WriteLine("Form demo - related application  finished.");
            Console.WriteLine();
            return ret;
        }  // AppMathematica



        #endregion Actions.Mathematica



        #endregion Actions


    }

}