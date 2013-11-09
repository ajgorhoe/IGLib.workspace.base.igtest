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

    public class Script_Misc : LoadableScriptBase, ILoadableScript
    {

        #region Standard_TestScripts

        public Script_Misc()
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

        /// <summary>Tests of list operations.</summary>
        public const string ConstListOperations = "ListOperations";
        public const string ConstHelpListOperations = "Tests of lists and operations on them.";

        
        /// <summary>Tests of file operations.</summary>
        public const string ConstFileOperations = "FileOperations";
        public const string ConstHelpFileOperations = "Tests of file operations.";
        
        /// <summary>Tests of string parsers.</summary>
        public const string ConstSystem = "System";
        public const string ConstHelpSystem = "Tests of system utilities.";

        /// <summary>Tests of string parsers.</summary>
        public const string ConstStringParser = "StringParser";
        public const string ConstHelpStringParser = "Tests of strng parser.";

        /// <summary>Tests of console utilities.</summary>
        public const string ConstConsoleUtilities = "ConsoleUtilities";
        public const string ConstHelpConsoleUtilities = "Tests of console utilities.";

        /// <summary>Test of JSON serializer.</summary>
        public const string ConstSerializerJson = "SerializerJson";
        public const string ConstHelpSerializerJson = "Tests of JSON serialization.";

        /// <summary>JavaScript - based expression evaluator.</summary>
        public const string ConstExpressionEvaluatorJavaScript = "ExpressionEvaluatorJs";
        public const string ConstHelpExpressionEvaluatorJavaScript = "Tests of JavaScript based Expression evaluator.";

        /// <summary>Test of Registerable classes and ID generators.</summary>
        public const string ConstRegisterable_IdGenerator = "Registerable_IdGenerator";
        public const string ConstHelpRegisterable_IdGenerator = "Test of IRegisterable interface and ID generator.";



        /// <summary>Test of exotic and obsolete classes.</summary>
        public const string ConstExoticAndObsolete = "Registerable_IdGenerator";
        public const string ConstHelpExoticAndObsolete = "Test of IRegisterable interface and ID generator.";


        #endregion Commands


        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstListOperations, TestListsOperations, ConstHelpListOperations);
            Script_AddCommand(interpreter, helpStrings, ConstFileOperations, TestFileOperations, ConstHelpFileOperations);
            Script_AddCommand(interpreter, helpStrings, ConstSystem, TestSystem, ConstHelpSystem);
            Script_AddCommand(interpreter, helpStrings, ConstStringParser, TestStringParser, ConstHelpStringParser);
            Script_AddCommand(interpreter, helpStrings, ConstConsoleUtilities, TestConsoleUtilities, ConstHelpConsoleUtilities);
            Script_AddCommand(interpreter, helpStrings, ConstSerializerJson, TestSerializationJson, ConstHelpSerializerJson);
            Script_AddCommand(interpreter, helpStrings, ConstExpressionEvaluatorJavaScript, TestExpressionEvaluatorJavaScript, ConstHelpExpressionEvaluatorJavaScript);
            Script_AddCommand(interpreter, helpStrings, ConstRegisterable_IdGenerator, TestRegisterable_IdGenerator, ConstHelpRegisterable_IdGenerator);

            Script_AddCommand(interpreter, helpStrings, ConstExoticAndObsolete, TestExoticAndObsolete, ConstHelpExoticAndObsolete);
        }


        #region Actions

        /// <summary>Test of lists and operations on them.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestListsOperations(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of LISTS:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            Util.ExampleList();

            bool doTestBelow = true;
            if (doTestBelow)
            {
                List<double> l = new List<double>(10);
                l.Capacity = 10;
                for (int i = 0; i < 7; ++i)
                    l.Add(0);
                l[6] = 6.66;
                Console.WriteLine("List Capacity: " + l.Capacity);
                Console.WriteLine("List Count   : " + l.Count);
                Console.WriteLine("List         : \n" + l);
                Console.WriteLine("By componentds: ");
                for (int i = 0; i < l.Count; ++i)
                    Console.WriteLine("  l[" + i + "] = " + l[i]);
            }
            
            return null;
        }  // MatrixOperations

        /// <summary>Test of file operations.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestFileOperations(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of FILE OPERATIONS:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            UtilSystem.ExampleRelativePath();

            return null;
        }  // MatrixOperations


        /// <summary>Test of system utilities.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestSystem(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of SYSTEM UTILITIES:");
            Console.WriteLine();

            // UtilSystem.OutputLevel = 6;
            Console.WriteLine("MAC address of the fastest network interface: " + UtilSystem.GetMacAddressFastest());
            Console.WriteLine("Computer name: " + UtilSystem.GetComputerName());
            Console.WriteLine("Domain name: " + UtilSystem.GetDomainName());
            Console.WriteLine("Local IP address: " + UtilSystem.GetIpAddressLocal());
            Console.WriteLine();

            return null;
        }  // MatrixOperations


        /// <summary>Test of string parsers.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestStringParser(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of STRING PARSERS:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            CharacterBuffer.TestSpeed();

            Parser.Examples();

            return null;
        }  

        /// <summary>Test of console utilities.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestConsoleUtilities(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of CONSOLE UTILITIES:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            UtilConsole.Examples();

            return null;
        }  

        /// <summary>Test of JSON serializer.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestSerializationJson(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of MATRIX DECOMPOSITIONS:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            SerializerBase.ExampleTestType();

            //SerializerBase.TestSerialization<SerializerBase.TestClass>(
            //    new SerializerJson(true),
            //    // new SerializerBase.TestClass("TestClass object", 2.33),
            //    new SerializerBase.TestClass(),
            //    "../../testdata/serialization/Serialized.json");

            //SerializerBase.TestSerialization<SerializerBase.SerializationTestClass>(
            //    new SerializerJson(true),
            //    // new SerializerBase.TestClass("TestClass object", 2.33),
            //    SerializerBase.SerializationTestClass.CreateTestObject(),
            //    "../../testdata/serialization/Serialized.json",
            //    true /* firstStep */, true /* secondStep */);


            SerializerBase.TestSerializationAll("../../testdata/serialization/");


            // SerializerBase.TestSerializationJSON("../../testdata/serialization/Serialized.json");


            return null;
        }  // SerializerJson

        /// <summary>JavaScript - based expression evaluator.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestExpressionEvaluatorJavaScript(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of MATRIX DECOMPOSITIONS:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            ExpressionEvaluatorJs.ExampleCommandLine();

            return null;
        }  // ExpressionEvaluatorJavaScript

        /// <summary>Test of Registerable classes and ID generators.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestRegisterable_IdGenerator(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of MATRIX DECOMPOSITIONS:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();


            RegisterableExamples.ExampleIdGenerator();

            RegisterableExamples.ExampleRegistrable();


            return null;
        }  // ExpressionEvaluatorJavaScript



        /// <summary>Test of exotic and obsolete classes.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestExoticAndObsolete(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of EXOTIC AND OBSOLETE utilities:");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            //XComplex.Examples();

            Scalar.Examples();

            return null;
        }  


        #endregion Actions

    }  // script
}

