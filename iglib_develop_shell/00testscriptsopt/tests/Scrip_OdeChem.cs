// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: Chemical reactions, systems of ordinary differential equations.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;
using IG.Physics;

using IG.Plot2d;
using IG.Gr3d;

namespace IG.Script
{

    /// <summary>Script for testing systems of ordinary differential equations and 
    /// chemical reactions.</summary>
    public class Scrip_OdeChem : LoadableScriptBase, ILoadableScript
    {

        #region Standard_TestScripts

        public Scrip_OdeChem()
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


        /// <summary>Command name for test of chemical reactions.</summary>
        public const string ConstChemicalReactions = "ChemicalReactions";
        public const string ConstHelpChemicalReactions = "Tests of utilities related to chemical reactions.";

        /// <summary>Command name for test of systems of ordinary differential equations.</summary>
        public const string ConstOrdinaryDifferentialEquations = "DifferentialEquations";
        public const string ConstHelpOrdinaryDifferentialEquations = "Tests of ordinary differential equations.";

        /// <summary>Command name for test of oscillation.</summary>
        public const string ConstOscillation = "Oscillation";
        public const string ConstHelpOscillation = "Tests of solution of harmonic and dumped oscillation.";


        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstChemicalReactions, ChemicalReactions, ConstHelpChemicalReactions);
            Script_AddCommand(interpreter, helpStrings, ConstOrdinaryDifferentialEquations, OrdinaryDifferentialEquations, ConstHelpOrdinaryDifferentialEquations);
            Script_AddCommand(interpreter, helpStrings, ConstOscillation, Oscillation, ConstHelpOscillation);
        }

        #endregion Standard_TestScripts



        #region Actions


        //public string PrintHelp()
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("INSTRUCTIONS: ");
        //    Console.WriteLine("Select an action by the first commans-line argument:");
        //    Console.WriteLine("  {0,12}: Simple test to see that script is alive.", ConstTest);
        //    Console.WriteLine("  {0,12}: Test of output formats.", ConstTestScript);
        //    Console.WriteLine();
        //    return null;
        //}

        ///// <summary>Just an example action.</summary>
        ///// <returns>The null string.</returns>
        //public string Test()
        //{
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine("ScriptExamples: THIS IS AN EXMPLE ACTION.");
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    return null;
        //}


        ///// <summary>Test action - to see that script is alive.</summary>
        ///// <param name="arguments">Array of command-line arguments.</param>
        ///// <returns>The null string.</returns>
        //public string TestScript(string[] arguments)
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("ScriptExamples: This script is alive.");
        //    Script_PrintArguments("Script arguments: ", arguments);
        //    Console.WriteLine();
        //    return null;
        //}


        /// <summary>Test action - chemical reactions.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string ChemicalReactions(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("ScriptExamples: test of CHEMICAL REACTIONS.");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            ChemicalElement He = new ChemicalElement(ChemicalElements.Ar, 0, null); 


            return null;
        }


        /// <summary>Test action - to see that script is alive.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string OrdinaryDifferentialEquations(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("ScriptExamples: test of ORDINARY DIFFERENTIAL EQUATIONS.");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            DifferentialHarmonicOscillation oscillation =
            new DifferentialHarmonicOscillation(1 /*mass*/ , 1 /* frequency */, 0.4 /* damping ratio */);
            //DifferentialFirstOrderSolverEuler solver = new DifferentialFirstOrderSolverEuler(oscillation);


            return null;
        }


        /// <summary>Test action - solution of oscillation prblems by ODE solvers.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string Oscillation(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("ScriptExamples: test of ORDINARY DIFFERENTIAL EQUATIONS.");
            Script_PrintArguments("Script arguments: ", arguments);
            Console.WriteLine();

            DifferentialHarmonicOscillation oscillation =
            new DifferentialHarmonicOscillation(1 /*mass*/ , 1 /* frequency */, 0.4 /* damping ratio */);
            //DifferentialFirstOrderSolverEuler solver = new DifferentialFirstOrderSolverEuler(oscillation);


            return null;
        }



        #endregion Actions

    }  // script
}

