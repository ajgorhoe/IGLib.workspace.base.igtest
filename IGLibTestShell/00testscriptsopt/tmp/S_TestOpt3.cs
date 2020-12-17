// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// SCRIPT FILE. Test of graphics.

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using IG.Lib;
using IG.Num;
using IG.Neural;



namespace IG.Script
{

    public class S_TestOpt3 : LoadableScriptBase, ILoadableScript
    {

        public S_TestOpt3()
            : base()
        { }

        /// <summary>Initializes the current object.</summary>
        protected override void InitializeThis(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("*********** SCRIPT INITIALIZATION... ***********");
            Console.WriteLine("Class name: " + this.GetType().Name);
            Script_PrintArguments("Initialization arguments: ", arguments);
            // Perform base class' initialization:

            // base.InitializeThis(arguments);

            if (arguments != null)
            {
                if (arguments.Length > 0)
                {
                    //// this.OptimizationDirectory = arguments[0]; // This should be performed in the base class' InitializeThis()!
                    //Console.WriteLine("Optimization directory set to: " + this.OptimizationDirectory);
                    //Console.WriteLine("Working directory set to: " + this.WorkingDirectory);
                    //if (!Directory.Exists(this.OptimizationDirectory))
                    //    Console.WriteLine("WARNING: Working directory does not exist!");
                }
            }
            Console.WriteLine("*********** INITIALIZATION FINISHED ***********");
            Console.WriteLine();
        }


        /// <summary>Runs action of the current object.</summary>
        /// <param name="arguments">Command-line arguments of the action.</param>
        protected override string RunThis(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("*********** SCRIPT EXECUTION... ***********");
            Console.WriteLine("Class name: " + this.GetType().Name);
            Script_PrintArguments("Run arguments: ", arguments);

            Console.WriteLine("");
            Console.WriteLine("Development of script for neural networks paper...");
            Console.WriteLine("");

            //// Test if graphics works:
            //PlotterZedGraph.ExampleLissajous();
            //VtkPlotBase.ExampleCurvePlotLissajous();


            Script_Run(ConstDefaultHelp);  // Runs help through interpreter; this is not intended with custom application scripts!


            // MessageBox.Show("This is a testmessage from the file 'script1.cms'.");
            Console.WriteLine("*********** EXECUTION FINISHED. ***********");
            Console.WriteLine("Class name: " + this.GetType().Name);
            Console.WriteLine();
            return null;
        }



        /// <summary>Prints help.</summary>
        /// <param name="arguments">Array of script run arguments.</param>
        /// <returns>null.</returns>
        protected override string Script_CommandHelp(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("INFO on this script:");
            Console.WriteLine("This is a custom application script.");
            Console.WriteLine("The script does not suppoer selectable runnable methods.");
            Console.WriteLine("Script (class) name: " + this.GetType().Name);
            Console.WriteLine();
            return null;
        }

        /// <summary>Override the TestScript runnable method in order to print some additional information.</summary>
        /// <param name="arguments">Script arguments.</param>
        /// <returns>null.</returns>
        protected override string Script_CommandTestScript(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("WARNING: ");
            Console.WriteLine("Method " + arguments[0] + ": This script is not intended for running commands.");
            Console.WriteLine();
            base.Script_CommandTestScript(arguments);
            Console.WriteLine();
            return null;
        }


    } // script class
}

