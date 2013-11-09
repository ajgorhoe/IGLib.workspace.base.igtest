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

    public class Script_Examples : LoadableScriptBase, ILoadableScript
    {

        #region Standard_TestScripts

        public Script_Examples()
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

        /// <summary>Name of the command for test of function loader.</summary>
        public const string ConstFunctionLoader = "FunctionLoader";
        public const string ConstHelpFunctionLoader = "Test of function loader.";

        /// <summary>Name of the command for test of operations on point clouds.</summary>
        public const string ConstPointClouds = "PointClouds";
        public const string ConstHelpPointClouds = "Test of operations on point clouds.";

        /// <summary>Name of the command for tests of 3D graphics.</summary>
        public const string ConstGraphics3d = "Graphics3D";
        public const string ConstHelpGraphics3d = "Tests of 3D graphics.";

        /// <summary>Name of the command for test of copying directory.</summary>
        public const string ConstCopyDir ="CopyDir";
        public const string ConstHelpCopyDir = "Test of copying directory.";

        /// <summary>Name of the command for test of waiting for file events.</summary>
        public const string ConstWaitFileEvents = "WaitFileEvents";
        public const string ConstHelpWaitFileEvents = "Test of waiting for file events.";

 
        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstFunctionLoader, FunctionLoader, ConstHelpFunctionLoader);
            Script_AddCommand(interpreter, helpStrings, ConstPointClouds, PointClouds, ConstHelpPointClouds);
            Script_AddCommand(interpreter, helpStrings, ConstGraphics3d, Graphics3d , ConstHelpGraphics3d);
            Script_AddCommand(interpreter, helpStrings, ConstCopyDir, CopyDir, ConstHelpCopyDir);
            Script_AddCommand(interpreter, helpStrings, ConstWaitFileEvents, WaitFileEvents, ConstHelpWaitFileEvents);
        }

       #endregion Commands


        #region Actions


        /// <summary>Test action - to see that script is alive.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string FunctionLoader(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of FUNCSION LOADER (for real and scalar functions).");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            //RealFunctionLoader.Example(arguments[1]); // @"c:\temp\realfunctionscript.cs");
            RealFunctionLoader.Example(null);
            //ScalarFunctionLoader.Example(arguments[1]); // @"c:\temp\scalarfunctionscript.cs");
            //ScalarFunctionLoader.Example(null);

            return null;
        }


        /// <summary>Test action - to see that script is alive.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string PointClouds(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of POINT CLOUDS.");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            int numPoints = 1000;
            int spaceDimension = 4;
            int numClosestPoints = 6;

            Console.WriteLine("Ide3ntify nearest neighbrs - dummy method:");
            PointCloudVector pointCloud = PointCloudVector.ExampleClosestPointsDummy
                (numPoints, spaceDimension, numClosestPoints, true /* accelerateSortingByStoringDistances */);
            pointCloud.PrintNeighborDistanceStatistics(numClosestPoints, false, new DistanceComparerVector(null));

            return null;
        }


        /// <summary>Test action - to see that script is alive.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string Graphics3d(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("ScriptExamples: test of 3D graphics.");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            VtkPlotBase.ExampleCurvePlotTorusKnot(5,7);

            return null;
        }



        /// <summary>Test action - to see that script is alive.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string CopyDir(string[] arguments)
        {
            UtilSystem.ExampleCopyDir();

            return null;
        }


        /// <summary>Test of waiting for file events.</summary>
        /// <param name="arguments">Array of command-line arguments.</param> 
        /// <returns>The null string.</returns>
        public string WaitFileEvents(string[] arguments)
        {

            string testFileOrDirectory = arguments[1];
            // testFileOrDirectory = @"D:\users\igor\cvis\igcs\igtest\iglib_optimization\data\test";


            bool WaitFileEvents = false;

            if (WaitFileEvents)
            {
                // WAITING FOR A SPECIFIC NUMBER OF FILE EVENTS TO OCCUR:
                WaitFileEvent.ExampleWaitEvents(testFileOrDirectory, 5 /* numevents */);
            }




            bool reportFileEvents = false;

            if (reportFileEvents)
            {

                // REPORTING CERTAIN KINDS OF FILE EVENTS:

            }


            bool testWaitFile = false;

            if (testWaitFile)
            {
                // WAITING FILE EVENTS - WITHOUT LATENCE:

                //WaitFileEventBase.ExampleBlockCreateRemove(
                //    testFileOrDirectory,
                //    3,  /* numSwitches */
                //    true /* waitDirectory - if true then directory is monitored rather than a file */);

                WaitFileEventBase.TestSpeedBlockCreateRemove(
                    testFileOrDirectory,
                    100, /* numSwitches */
                    0, /* sleepMs */
                    false /* waitDirectory - if true then directory is monitored rather thana file */);
            }


            bool testWaitFileWithLatence = false;

            if (testWaitFileWithLatence)
            {

                // WAITING FILE EVENTS WITH LATENCE:

                //WaitFileEventLatenceBase.ExampleBlockCreateRemoveLatence(
                //    testFileOrDirectory,
                //    3,  /* numSwitches */
                //    true /* waitDirectory - if true then directory is monitored rather thana file */);

                WaitFileEventLatenceBase.TestSpeedBlockCreateRemoveLatence(
                    testFileOrDirectory,
                    100, /* numSwitches */
                    1, /* sleepMs */
                    false /* waitDirectory - if true then directory is monitored rather thana file */);

            }

            return null;
        }  // TestWaitFileEvents



        #endregion Actions

    }  // script
}

