// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;

using IG.Lib;
using IG.Forms;
using IG.Num;
using IG.Script;

 
namespace TestOpt
{



    /// <summary>Class containing the main method of application.</summary>
    /// <remarks><para>Standard scheme for IGLib-based test applications has been adopted in December 2011.</para></remarks>
    /// $A Igor xx Nov09 Dec11;
    public class IGLibShellExtMainProgram: ApplicationCommandlineBase
    {

        public IGLibShellExtMainProgram() : base()
        {
            DefaultActiveDir = @"../../testdata/scripts";
            OptDir = DefaultActiveDir;
        }

        #region Operation

        /// <summary>Entry point of the application.</summary>
        /// <param name="args">Application arguments.</param>
        static void Main(string[] args)
        {
            new IGLibShellExtMainProgram().AplicationMain(args);
        } // Main(string[])

        /// <summary>Optimization directory that contains data and message files of optimization server.</summary>
        public string OptDir;

 
        /// <summary>Creates and returns application's command-line interpreter.</summary>
        protected override CommandLineApplicationInterpreter CreateInterpreter()
        {
            CommandLineApplicationInterpreter ret = new AppTestOpt(false);
            ret.RegisterSystemPriorityUpdating();  // thread priority of the interpreter will be updated when global thread priority changes.
            return ret;
        }

        /// <summary>Adds assemblies to be automatically referenced by loaded scripts.</summary>
        public override void AddDefaultAssemblies()
        {
            base.AddDefaultAssemblies();
            //ScriptLoaderBase.AddDefaultAssemblies(
            //    "System.Windows.Forms.dll",
            //    "System.Drawing.dll",
            //    "igplot2d.dll",
            //    "igplot3d.dll",
            //    "iglib_test.exe",  /* remove this if necessary */
            //    "Kitware.VTK.dll",
            //    "ZedGraph.dll"
            //    );
            ScriptLoaderBase.AddDefaultAssemblies(
                "iglibopt.dll",
                "iglibshellext.dll",
                "iglibneuralext.dll", 
                "System.Windows.Forms.dll",
                "System.Drawing.dll",
                "igplot2d.dll",
                "igplot3d.dll",
                "Kitware.VTK.dll",
                "ZedGraph.dll",
                "iglib_develop_test.dll",
                "iglib_develop_test_aux.dll"
                );
        }

        
        /// <summary>Default main method for the current application.</summary>
        /// <param name="args">Command-line argumets passed when the application is run.</param>
        /// <remarks><para>Standard scheme for IGLib-based test applications has been adopted in December 2011.</para></remarks>
        /// $A Igor xx Nov09 Dec11;
        public override void AplicationMain(string[] args)
        {

            // IGLibShellExtApp app = new IGLibShellExtApp();
            IGLibShellExtApp.Init();

            Util.OutputLevel = 0;
            base.AplicationMain(args);
            Util.OutputLevel = 0;
        }


        /// <summary>Runs tests from scripts according to hard-coded settings.
        /// <para>Standard form of test applications for functionality based on IGLib.</para></summary>
        /// <param name="args">Command-line argumets passed when the application is run.</param>
        /// <remarks><para>Standard scheme for IGLib-based test applications has been adopted in December 2011.</para></remarks>
        /// $A Igor xx Nov09 Dec11;
        public override void TestMain(string[] args)
        {
            try
            {
                // Hard-coded scripts:
                //DefaultActiveDir = @"d:\users\workspaceprojects\11_10_clanek_neural\projects\testing\opt\";
                DefaultActiveDir = @"../../testdata/scripts";
                OptDir = DefaultActiveDir;
                Console.WriteLine("This is IGLib's Sandbox testing application (used only by Igor G.).");
                Console.WriteLine();

                // TESTING SECTION: (set interactive to false in order to run this)
                bool enterTestFunctionLoader = false;
                //enterTestFunctionLoader = true;
                if (enterTestFunctionLoader)
                {
                    //RealFunctionLoader.Example(@"c:\temp\realfunctionscript.cs");
                    RealFunctionLoader.Example(null);
                    //ScalarFunctionLoader.Example(@"c:\temp\scalarfunctionscript.cs");
                    //ScalarFunctionLoader.Example(null);
                    return;
                }


                // TEST RUNNING SCRIPTS:

                RunThroughInterpreter = false;
                LoadableScriptBase.DefaultOutputLevel = 2;

                string usualActiveDir = @"../../testdata/scripts";
                string testScriptDir = @"../../00testscriptsopt/tests/";
                string templateScriptDir = @"../../00testscriptsopt/templates/";
                string customScriptDir = @"../../00testscriptsopt/tmp/";

                // MAIN SELECTION options:
                ScriptGroupChoice = ConstInteractive;      // run commands interactively
                ScriptGroupChoice = ConstScriptTests;      // run test scripts (containing tests in different areas)
                ScriptGroupChoice = ConstScriptCustomApp;  // run custom applications defined by scripts
                // Actual selection (copy one of the above!)
                ScriptGroupChoice = ConstScriptTests;

                if (ScriptGroupChoice == ConstScriptCustomApp)
                {
                    // CUSTOM APPLICATION selection options:
                    ScriptType = typeof(S_TestOpt1);
                    ScriptType = typeof(S_TestOpt2);
                    ScriptType = typeof(S_TestOpt3);
                    // Actual selection of the script to be run:
                    ScriptType = typeof(S_TestOpt1);

                    // Default location of custom application scripts:
                    ScriptLocation = Path.GetFullPath(Path.Combine(customScriptDir, ScriptChoice + ".cs"));

                } else if (ScriptGroupChoice == ConstScriptTests)
                {
                    // TEST SCRIPT selection options:
                    ScriptType = typeof(ScriptOptTemplate);
                    ScriptType = typeof(Script_Examples);
                    ScriptType = typeof(Script_Misc);
                    ScriptType = typeof(Scrip_OdeChem);
                    ScriptType = typeof(Script_Numeric);
                    ScriptType = typeof(Script_Optimization);
                    // Actual selection of custom script (copy one of the above options!):
                    ScriptType = typeof(Script_Numeric);

                    // Default location for test scripts (for some scripts you will need to change this):
                    ScriptLocation = Path.GetFullPath(Path.Combine(testScriptDir, ScriptChoice + ".cs"));

                    // TEST SCRIPT ACTION SELECTION:
                    if (ScriptType == typeof(ScriptOptTemplate))
                    {
                        // Test script - TEMPLATES
                        if (!RunThroughInterpreter) ScriptToRun = new ScriptOptTemplate();
                        // Location is different for this script:
                        ScriptLocation = Path.GetFullPath(Path.Combine(templateScriptDir, typeof(ScriptOptTemplate).Name + ".cs"));
                        DefaultActiveDir = usualActiveDir;
                        ScriptAction = ScriptOptTemplate.ConstDefaultHelp;
                        ScriptAction = ScriptOptTemplate.ConstDefaultTestScrip;
                        ScriptAction = ScriptOptTemplate.ConstMyTest;
                        ScriptAction = ScriptOptTemplate.ConstAnotherTest;
                        // Actual action chosen (copy one of the above):
                        ScriptAction = ScriptOptTemplate.ConstDefaultHelp;
                    }
                    else if (ScriptType == typeof(Script_Examples))
                    {
                        // Test script - EXAMPLES
                        if (!RunThroughInterpreter) ScriptToRun = new Script_Examples();
                        DefaultActiveDir = usualActiveDir;
                        ScriptAction = Script_Examples.ConstDefaultHelp;
                        ScriptAction = Script_Examples.ConstDefaultTestScrip;
                        ScriptAction = Script_Examples.ConstFunctionLoader;
                        ScriptAction = Script_Examples.ConstPointClouds;
                        ScriptAction = Script_Examples.ConstGraphics3d;
                        ScriptAction = Script_Examples.ConstCopyDir;
                        ScriptAction = Script_Examples.ConstWaitFileEvents;
                        // Actual action chosen (copy one of the above):
                        ScriptAction = Script_Examples.ConstDefaultHelp;
                    }
                    else if (ScriptType == typeof(Script_Misc))
                    {
                        // Test script - MISCELLANEOUS TESTS
                        if (!RunThroughInterpreter) ScriptToRun = new Script_Misc();
                        DefaultActiveDir = usualActiveDir;
                        ScriptAction = Script_Misc.ConstDefaultHelp;
                        ScriptAction = Script_Misc.ConstDefaultTestScrip;

                        ScriptAction = Script_Misc.ConstListOperations;
                        ScriptAction = Script_Misc.ConstFileOperations;
                        ScriptAction = Script_Misc.ConstStringParser;
                        ScriptAction = Script_Misc.ConstConsoleUtilities;
                        ScriptAction = Script_Misc.ConstSerializerJson;
                        ScriptAction = Script_Misc.ConstExpressionEvaluatorJavaScript;
                        ScriptAction = Script_Misc.ConstRegisterable_IdGenerator;
                        // Actual action chosen (copy one of the above):
                        ScriptAction = Script_Misc.ConstExpressionEvaluatorJavaScript;
                    }
                    else if (ScriptType == typeof(Scrip_OdeChem))
                    {
                        // Test script - ODE, CHEMICAL REACTIONS
                        if (!RunThroughInterpreter) ScriptToRun = new Scrip_OdeChem();
                        DefaultActiveDir = usualActiveDir;
                        ScriptAction = Scrip_OdeChem.ConstDefaultHelp;
                        ScriptAction = Scrip_OdeChem.ConstDefaultTestScrip;
                        ScriptAction = Scrip_OdeChem.ConstOrdinaryDifferentialEquations;
                        ScriptAction = Scrip_OdeChem.ConstChemicalReactions;
                        // Actual action chosen (copy one of the above):
                        ScriptAction = Scrip_OdeChem.ConstDefaultHelp;
                    }
                    else if (ScriptType == typeof(Script_Numeric))
                    {
                        // Test script - NUMERICS
                        if (!RunThroughInterpreter) ScriptToRun = new Script_Numeric();
                        DefaultActiveDir = usualActiveDir;
                        ScriptAction = Script_Numeric.ConstTestScriptArguments;
                        ScriptAction = Script_Numeric.ConstCustom;
                        ScriptAction = Script_Numeric.ConstMatrixOperations;
                        ScriptAction = Script_Numeric.ConstRealFunction;
                        ScriptAction = Script_Numeric.ConstDifferentiation;
                        ScriptAction = Script_Numeric.ConstLinearApproximation;
                        ScriptAction = Script_Numeric.ConstMovingLeastSquares;
                        ScriptAction = Script_Numeric.ConstPerformanceTests;
                        ScriptAction = Script_Numeric.ConstTabResults;
                        ScriptAction = Script_Numeric.ConstParallelJobs;
                        ScriptAction = Script_Numeric.ConstSampling;
                        ScriptAction = Script_Numeric.ConstOptAlgLib;
                        // Actual action chosen (copy one of the above):
                        ScriptAction = Script_Numeric.ConstParallelJobs;
                    }
                    else if (ScriptType == typeof(Script_Optimization))
                    {
                        // Custom script - OPTIMIZATION
                        if (!RunThroughInterpreter) ScriptToRun = new Script_Optimization();
                        DefaultActiveDir = usualActiveDir;
                        ScriptAction = Script_Optimization.ConstTestScriptArguments;
                        
                        // Actual action chosen (copy one of the above):
                        ScriptAction = Script_Optimization.ConstTestScriptArguments;
                    }

                }  // ScriptGroupChoice == ConstScriptTests

                bool takeCompleteChoice = true;
                if (takeCompleteChoice)
                {
                    
                    // Script_Numeric:
                    ScriptGroupChoice = ConstScriptTests;
                    ScriptType = typeof(Script_Numeric);
                    ScriptToRun = new Script_Numeric();
                    // Custom test from Script_Numeric:
                    ScriptAction = Script_Numeric.ConstCustom;
                    // Test of running parallel jobs from Script_Numeric:
                    ScriptAction = Script_Numeric.ConstParallelJobs;
                    // Test of AlgLib optimization from Script_Numeric:
                    ScriptAction = Script_Numeric.ConstOptAlgLib;
                    // Test of sampling from Script_Numeric:
                    ScriptAction = Script_Numeric.ConstSampling;
 
                }

                if (string.IsNullOrEmpty(ScriptGroupChoice))
                    throw new ArgumentException("Script group is not selected.");

                if (ScriptGroupChoice == ConstInteractive)
                {
                    TestInterpreter.Run(TestInterpreter.MainThread, new string[] { ConstRunInteractive });
                }


                #region Tests

                if (ScriptGroupChoice == ConstScriptTests)
                {
                    // RUN THE SELECTED TEST:

                    if (string.IsNullOrEmpty(ScriptChoice))
                        throw new ArgumentException("Test script choice is not specified (null or empty string).");
                    Console.WriteLine("\nRunning a TEST script ...\n");
                    OptDir = DefaultActiveDir; //  @"d:\users\workspaceprojects\11_10_clanek_neural\projects\testing\opt\";
                    string arg1 = "arg1", arg2 = "arg2", arg3 = "arg3", arg4 = "arg4", arg5 = "arg5",
                        arg6 = "arg6", arg7 = "arg7", arg8 = "arg8";

                    DirectArguments = new string[] { ScriptAction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 };
                    InterpreterArguments = new string[] {  ConstRunScript, ScriptLocation, ScriptAction,
                                arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8  };

                    SetScriptNumArguments(1);

                    if (ScriptType == typeof(Script_Examples))
                    {
                        // Examples:
                        if (ScriptAction == Script_Examples.ConstHelpDefaultUniversal)
                        {
                            // Just for test, switch one argument for specific action: 
                            SetScriptArgument(1, "?");
                        }
                    }

                    if (ScriptType == typeof(Script_Misc))
                    {
                    }


                    if (ScriptType == typeof(Script_Numeric))
                    {

                        if (ScriptAction == Script_Numeric.ConstSampling)
                        {
                            SetScriptArgument(1, "5");
                            SetScriptArgument(2, "200");
                        }

                    }


                    Directory.SetCurrentDirectory(DefaultActiveDir);
                    if (RunThroughInterpreter)
                    {
                        if (string.IsNullOrEmpty(ScriptLocation))
                            throw new InvalidDataException("Script location is not defined.");
                        if (InterpreterArguments == null)
                            throw new InvalidDataException("Interpreter arguments are not defined.");
                        if (InterpreterArguments.Length < 2)
                            throw new InvalidDataException("There should be at least 2 interpreter arguments (run command + script location), now " + InterpreterArguments.Length);
                        InterpreterArguments[1] = ScriptLocation;  // script location is the second argument!
                        if (!File.Exists(InterpreterArguments[1]))
                            throw new InvalidDataException("Script file does not exist: \"" + InterpreterArguments[1] + "\".");
                        Console.WriteLine("Running TEST script through interpreter...");
                        // Remark: current rule is that initializatin and run arguments are the same.
                        TestInterpreter.Run(TestInterpreter.MainThread, InterpreterArguments);
                    }
                    else
                    {
                        Console.WriteLine("Running TEST script directly...");
                        if (ScriptToRun == null)
                            throw new InvalidOperationException("Script object to be run directly is not allocated (null reference). "
                                + Environment.NewLine + "  Chosen script: " + ScriptChoice + ", action: " + ScriptAction + ".");
                         // Remark: current rule is that initializatin and run arguments are the same.
                        ScriptToRun.InitializationArguments = DirectArguments;
                        // Remark: Normally, for test scripts the first argument must define the action method that 
                        // is run (by script's internal interpreter), but this rule is not enforced strictly.
                        ScriptToRun.Run(DirectArguments);
                    }

                    return;
                }  // ScriptChoiceGroup == ConstScriptTests

                #endregion Tests


                #region CustomApp


                if (ScriptGroupChoice == ConstScriptCustomApp)
                {

                    // RUN CUSTOM APPLICATION SCRIPT:

                    OptDir = DefaultActiveDir; //  @"d:\users\workspaceprojects\11_10_clanek_neural\projects\testing\opt\";
                    string arg1 = "arg1", arg2 = "arg2", arg3 = "arg3", arg4 = "arg4", arg5 = "arg5",
                        arg6 = "arg6", arg7 = "arg7", arg8 = "arg8";

                    DirectArguments = new string[] { ScriptAction, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 };
                    InterpreterArguments = new string[] {  ConstRunScript, ScriptLocation, ScriptAction,
                                arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8  };

                    if (ScriptType == typeof(S_TestOpt1))
                    {
                        if (!RunThroughInterpreter) ScriptToRun = new S_TestOpt1();
                    }
                    if (ScriptType == typeof(S_TestOpt2))
                    {
                        if (!RunThroughInterpreter) ScriptToRun = new S_TestOpt2();
                    }
                    if (ScriptType == typeof(S_TestOpt3))
                    {
                        if (!RunThroughInterpreter) ScriptToRun = new S_TestOpt3();
                    }

                    // Run the CUSTOM application script:
                    if (RunThroughInterpreter)
                    {
                        if (string.IsNullOrEmpty(ScriptLocation))
                            throw new InvalidDataException("Script location is not defined.");
                        if (InterpreterArguments == null)
                            throw new InvalidDataException("Interpreter arguments are not defined.");
                        if (InterpreterArguments.Length < 2)
                            throw new InvalidDataException("There should be at least 2 interpreter arguments (run command + script location), now " + InterpreterArguments.Length);
                        InterpreterArguments[1] = ScriptLocation;  // script location is the second argument!
                        if (!File.Exists(InterpreterArguments[1]))
                            throw new InvalidDataException("Script file does not exist: \"" + InterpreterArguments[1] + "\".");
                        Console.WriteLine("Running " + ScriptChoice + " through interpreter...");
                        Directory.SetCurrentDirectory(DefaultActiveDir);
                        // Remark: current rule is that initializatin and run arguments are the same.
                        TestInterpreter.Run(TestInterpreter.MainThread, InterpreterArguments);
                    }
                    else
                    {
                        Console.WriteLine("Running script " + ScriptChoice + " directly...");
                        if (ScriptToRun == null)
                            throw new InvalidOperationException("Script to be run is not allocated (null reference). "
                                + Environment.NewLine + "  Chosen script: " + ScriptChoice + ", action: " + ScriptAction + ".");
                        //if (string.IsNullOrEmpty(DirectArguments[0]))
                        //    throw new ArgumentException("Script action is not defined.");
                        ScriptToRun.InitializationArguments = DirectArguments;
                        ScriptToRun.Run(DirectArguments); 
                    }

                    return;

                }  // ScriptChoiceGroup == ConstScriptCustomApp
                
                #endregion CustomApp

            } 
            catch (Exception ex) 
            {
                Console.WriteLine();
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine();
                Console.WriteLine("In: Testmain.");
                Console.WriteLine("Script group executed: " + ScriptGroupChoice);
                Console.WriteLine("Script: " + ScriptChoice);
                Console.WriteLine("Action: " + ScriptAction);
                throw; 
            }
        }

        #endregion Operation



        /// <summary>Application class for the current application (IGLib Extended Shell Application, igsx).</summary>
        public class IGLibShellExtApp : App
        {

            public IGLibShellExtApp()
                : base("IGLib Extended Shell (IGSX)", 2 /* version */, 7 /* subversion */, "Beta" /* release */)
            {
            }


            protected override void BeforeInitialization()
            {
                base.BeforeInitialization();
                Expires = false;
                AuthorFirstName = "Igor Grešovnik";
                AuthorAddress1 = "Črneče 147";
                AuthorAddress2 = "SI-2370 Dravograd";
                AuthorAddress3 = "Slovenia";
            }

            protected override void AfterInitialization()
            {
                base.AfterInitialization();
                LaunchInitNotice();
                // AddModule(ModuleTest.Get());
            }


            #region global


            /// <summary>Launches initialization notice.</summary>
            public override void LaunchInitNotice()
            {
                int indent = 8;
                int padLeft = 2;
                int padRight = 2;
                int padTop = 0;
                int padBottom = 0;

                Console.WriteLine();
                Console.WriteLine(
                    DecorationFrameDoubleDashed (Notice(), indent, padLeft, padRight, padTop, padBottom));
                Console.WriteLine();

                //Console.WriteLine(
                //    DecorationFrameDashed(
                //    Notice(), indent, padLeft, padRight, padTop, padBottom)
                //    );
                Console.WriteLine();

                // Reporter.ReportWarning(InitNotice());
            }

            /// <summary>Initializes global application data for the current class of application.</summary>
            public static void Init()
            {
                lock (lockGlobal)
                {
                    if (!InitializedGlobal)
                    {
                        App.InitApp();
                        Global = new IGLibShellExtApp();
                    }
                }
            }

            #endregion

        }  // class IGLibShellApp


    }  // class IGLibShellExtMainProgram

    



} // namespace igform_console_test
