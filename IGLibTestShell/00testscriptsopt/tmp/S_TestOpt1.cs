// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// CUSTOM APPLICATION SCRIPT TEMPLATE.

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;

namespace IG.Script
{

    public class S_TestOpt1 : LoadableScriptBase, ILoadableScript
    {

        public S_TestOpt1()
            : base()
        { }

        // public TestClass(string [] args) : base(args)
        //  {  }

        /// <summary>Initializes the current object.</summary>
        protected override void InitializeThis(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("*********** SCRIPT INITIALIZATION... ***********");
            
            Console.WriteLine("Class name: " + this.GetType().Name);
            Console.WriteLine("TestLoadable object: initialization performed.");
            Script_PrintArguments("Initialization arguments: ", arguments);

            Script_Interpreter.RemoveCommand("xx");
            
            Console.WriteLine("*********** INITIALIZATION FINISHED ***********");
            Console.WriteLine();
        }

        /// <summary>Runs action of the current object.</summary>
        /// <param name="arguments">Command-line arguments of the action.</param>
        protected override string RunThis(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Class name: " + this.GetType().Name);
            Console.WriteLine("*********** SCRIPT EXECUTION... ***********");
            Script_PrintArguments("Run arguments: ", arguments);

            Console.WriteLine("\n\n\nBefore allocating a vector...");

            Vector a = new Vector(2, 0.2);
            Vector b = new Vector(2, 1.1);
            IVector c = null;
            Vector.Add(a, b, ref c);
            Console.WriteLine("a: " + a);
            Console.WriteLine("b: " + b);
            Console.WriteLine("a+b: " + c);
            Console.WriteLine("Sum of vectors: " + c);

            Console.WriteLine("After summation of two vectors.\n\n\n");


            Script_CommandHelp(arguments);


            Console.WriteLine("*********** EXECUTION FINISHED. ***********");
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
        protected override string  Script_CommandTestScript(string[] arguments)
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

