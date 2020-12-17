// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: Different examples.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using IG.Num;
using IG.Lib;



namespace IG.Script
{

    public class Script_Numeric : LoadableScriptBase, ILoadableScript
    {

        #region Standard_TestScripts

        public Script_Numeric()
            : base()
        { }


        /// <summary>Initializes the current object.</summary>
        protected override void InitializeThis(string[] arguments)
        {
            // Script_DefaultInitialize(arguments);
        }

        /// <summary>Runs action of the current object.</summary>
        /// <param name="arguments">Command-line arguments of the action.</param>
        protected override string RunThis(string[] arguments)
        {
            return Script_DefaultRun(arguments);
        }

        #endregion Standard_TestScripts

        #region Commands

        /// <summary>Test of running the script, writes arguments.</summary>
        public const string ConstTestScriptArguments = "Test1";
        public const string ConstHelpTestScriptArguments = "Performs test , outputs script arguments.";

        /// <summary>Custom test.</summary>
        public const string ConstCustom = "CustomTest";
        public const string ConstHelpCustom = "Custom test.";

        /// <summary>Tests of matrix operations.</summary>
        public const string ConstMatrixOperations = "MatrixOperations";
        public const string ConstHelpMatrixOperations = "Tests of matrix operations.";

        /// <summary>Test of real functions of one variable.</summary>
        public const string ConstPerformanceTests = "PerformanceTests";
        public const string ConstHelpPerformanceTests = "Tests of various performance tests.";

        /// <summary>Test of real functions of one variable.</summary>
        public const string ConstRealFunction = "RealFunction";
        public const string ConstHelpRealFunction = "Tests of real function of one variable.";

        /// <summary>Test of numerical differentiation.</summary>
        public const string ConstDifferentiation = "Differentiation";
        public const string ConstHelpDifferentiation = "Tests of numerical differentiation.";

        /// <summary>Test of linear approximation.</summary>
        public const string ConstLinearApproximation = "LinearApproximation";
        public const string ConstHelpLinearApproximation = "Tests of linear approximations.";

        /// <summary>Test of moving least squares.</summary>
        public const string ConstMovingLeastSquares = "MovingLeastSquares";
        public const string ConstHelpMovingLeastSquares = "Tests of the moving least squares method.";

        /// <summary>Test of result tables.</summary>
        public const string ConstTabResults = "TabResults";
        public const string ConstHelpTabResults = "Demonstrations of linear reault tables.";

        /// <summary>Tests parallel job dispatcher.</summary>
        public const string ConstParallelJobs = "ParallelJobs";
        public const string ConstHelpParallelJobs = "Testing of parallel job dispatcher.";

        /// <summary>Tests sampling algorithms.</summary>
        public const string ConstSampling = "Sampling";
        public const string ConstHelpSampling = "Testing of various sampling algorithms.";

        /// <summary>Tests Alglib optimization algorithms.</summary>
        public const string ConstOptAlgLib = "OptAlgLib";
        public const string ConstHelpOptAlgLib = "Testing of AlgLib's optimization algorithms.";

        #endregion Commands

        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstTestScriptArguments, TestScriptArguments, ConstHelpTestScriptArguments);
            Script_AddCommand(interpreter, helpStrings, ConstCustom, TestCustom, ConstHelpCustom);
            Script_AddCommand(interpreter, helpStrings, ConstMatrixOperations, TestMatrixOperations, ConstHelpMatrixOperations);
            Script_AddCommand(interpreter, helpStrings, ConstRealFunction, TestRealFunction, ConstHelpRealFunction);
            Script_AddCommand(interpreter, helpStrings, ConstDifferentiation, TestDifferentiation, ConstHelpDifferentiation);
            Script_AddCommand(interpreter, helpStrings, ConstLinearApproximation, TestLinearApproximation, ConstHelpLinearApproximation);
            Script_AddCommand(interpreter, helpStrings, ConstMovingLeastSquares, TestMovingLeastSquares, ConstHelpMovingLeastSquares);
            Script_AddCommand(interpreter, helpStrings, ConstPerformanceTests, TestPerformanceTests, ConstHelpPerformanceTests);
            Script_AddCommand(interpreter, helpStrings, ConstTabResults, TestTabResults, ConstHelpTabResults);
            Script_AddCommand(interpreter, helpStrings, ConstParallelJobs, TestParallelJobs, ConstHelpParallelJobs);
            Script_AddCommand(interpreter, helpStrings, ConstSampling, TestSampling, ConstHelpSampling);
            Script_AddCommand(interpreter, helpStrings, ConstOptAlgLib, TestOptAlglib, ConstHelpOptAlgLib);
        }


        #region Actions

        /// <summary>Simple test, just writes script arguments to console.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestScriptArguments(string[] arguments)
        {
            Console.WriteLine("This output is written from class " + this.GetType().Name);
            return this.Script_CommandTestScript(arguments);

        }
        
        /// <summary>Test of matrix operations (decompositions, etc.).</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestCustom(string[] arguments)
        {


            Console.WriteLine();
            if (M.CheckFactorialsArray())
                Console.WriteLine("Array of factorials is correct.");
            M.TestFactorials();
            M.TestBinomialCoefficients();

            return null;
        }


        /// <summary>Test of matrix operations (decompositions, etc.).</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestMatrixOperations(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of MATRIX OPERATIONS:");
            Console.WriteLine();
            
            int dim = 500;
            bool testProduct = false;  // wheter product of decomposed matrix is tested or not.
            int outLevel = 3;

            bool testGeneration = false;
            if (testGeneration)
            {
                Console.WriteLine();
                Console.WriteLine("Generation of a random " + dim + "x" + dim + " symmetric positive definite matrix:");
                IMatrix M = new Matrix(dim, dim);
                Matrix.SetRandomSymmetricPositiveDefinite(M);
                CholeskyDecomposition dec = new CholeskyDecomposition(M as Matrix);
                Console.WriteLine("Generation successful.");
                Console.WriteLine();
                return null;
            }

            bool testBasic = false;
            if (testBasic)
            {

                MatrixBaseDev.TestMatrixDecompositions(1e-6, true);

                // MatrixBase.TestMatrixProducts(true);

                //mat3.Example();  // 3D matrix struct examples!

                return null;
            }

            bool testDecomp = true;
            if (testDecomp)
            {
                // TESTS OF MATRIX DECOMPOSITIONS - SPEED & ACCURACY

                bool testDecompositions = true;
                if (testDecompositions)
                {

                    Console.WriteLine();
                    Console.WriteLine("LU Decomposition with new IGLib wrappers (Math.Net Numerics): ");
                    SpeedTestCpu.TestComputationalTimesLU(dim, outLevel, testProduct);

                    Console.WriteLine();
                    Console.WriteLine("QR Decomposition with new IGLib wrappers (Math.Net Numerics): ");
                    SpeedTestCpu.TestComputationalTimesQR(dim, outLevel, testProduct);

                    Console.WriteLine();
                    Console.WriteLine("Cholesky Decomposition with new IGLib wrappers (Math.Net Numerics): ");
                    SpeedTestCpu.TestComputationalTimesCholesky(dim, outLevel, testProduct);
                   
                }

                bool testDecompositionsMathNet = false;  // perform tests with the old MathNet library
                if (testDecompositionsMathNet)
                {

                    Console.WriteLine("Test of LU decomposition with the MathNet library:");
                    SpeedTestCpu.TestComputationalTimesLU(dim, outLevel, testProduct);

                    Console.WriteLine("Test of QR decomposition with the MathNet library:");
                    SpeedTestCpu.TestComputationalTimesQR(dim, outLevel, testProduct);
                }
            }

            bool testExternalLib = false;
            if (testExternalLib)
            {

                //SpeedTestCpu.ExampleMathNetNumericsLU();

                //SpeedTestCpu.ExampleMathNetNumericsQR();

                SpeedTestCpu.ExampleMathNetNumericsEVD();

                //SpeedTestCpu.ExampleMathNetNumericsSVD();

            }

            return null;
        }  // MatrixOperations


        /// <summary>Test of real functions of one variable.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestRealFunction(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of real function of a single cariable...");
            Console.WriteLine();

            RealFunction.TestSpeed();

            RealFunction.ExampleTests();

            Console.WriteLine();
            return null;
        } 

        /// <summary>Test of numerical differentiation.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestDifferentiation(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of numerical differentiation...");
            Console.WriteLine();

            Numeric.TestDifferentiation();

            Console.WriteLine();
            return null;
        }  

        /// <summary>Test of linear approximations.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestLinearApproximation(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of linear approximation...");
            Console.WriteLine();


            LinearBasis.Example();

            Console.WriteLine();
            return null;
        } 

        /// <summary>Test of linear approximations.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestMovingLeastSquares(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Test of Moving Least Squares...");
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine("No tests implemeted yet for moving least squares!");
            Console.WriteLine();

            Console.WriteLine();
            return null;
        } 

        /// <summary>Test of various performance tests.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestPerformanceTests(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Testing performance tests...");
            Console.WriteLine();

            ThreadPerformanceTest.Example();

            Console.WriteLine();
            return null;
        } 

        /// <summary>Test of tables of results (e.g. tables of approximated values or of optimization 
        /// analysis results).</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestTabResults(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Testing tables of results (approximation results, optimization analysis results, etc.)...");
            Console.WriteLine();

            IG.Num.AnalysisTable.ExampleTableFactors();

            //AnalysisTable.ExampleTableFactors();

            Console.WriteLine();
            return null;
        }

        /// <summary>Test of parallel jobs.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestParallelJobs(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Testing parallel job dispatcher...");
            Console.WriteLine();

            ParallelJobContainerBase.DefaultOutputLevel = 0;
            int clientOutputLevel = 0;

            int numPoints = 1000;
            int numServers = 20;
            int maxEnqueued = 100; 
            double delayTimeSeconds = 0.001; 
            double delayTimeRelativeError = 0.0;
            int sleepTimeMs = 2;
            ParallelJobContainerBase.TestPerformance(numPoints,
            numServers, maxEnqueued, delayTimeSeconds, delayTimeRelativeError, sleepTimeMs, clientOutputLevel);
            Console.WriteLine();
            return null;
        }

        
        /// <summary>Test of various sampling algorithms.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestSampling(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Testing sampling algorithms...");
            Console.WriteLine();

            int dimension = 5;
            int numSamples = 200;

            bool parsed;
            if (arguments.Length >=2)
            {
                parsed = int.TryParse(arguments[1], out dimension);
                if (parsed)
                    Console.WriteLine("Dimension set to " + dimension + " by the arguments.");
                else
                    Console.WriteLine("Wrong format for integer dimension: \"" + arguments[1] + "\", set to " + dimension + ".");
                if (arguments.Length >= 3)
                {
                    parsed = int.TryParse(arguments[2], out numSamples);
                    if (parsed)
                        Console.WriteLine("Dimension set to " + numSamples + " by the arguments.");
                    else
                        Console.WriteLine("Wrong format for integer number of samples: \"" + arguments[2] 
                            + "\", set to " + numSamples + ".");
                }
            }

            //SamplerUnitBallRandomFromCube.TestSampleNorms(dimension, numSamples);
            SamplerUnitBallRandomFromCube.TestSamplingSpeed(dimension, 100*numSamples);

            return null;

        }

        /// <summary>Test of AlgLib optimization algorithms.</summary>
        /// <param name="arguments">Array of command-line arguments.</param>
        /// <returns>The null string.</returns>
        public string TestOptAlglib(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Testing Alglib optimization algorithms...");
            Console.WriteLine();


            bool test2d = false;
            if (test2d)
            {
                new TestAlglibOld2008().TestLbfgsAlglibOld();
            }

            int dimension = 30;
            bool testGeneralizedRosenbrock = true;
            if (testGeneralizedRosenbrock)
            {
                //TestAlgLibBase.DefaultOutputLevel = 2;
                TestAlgLibBase2008.DefaultOutputLevel = 2;
                new TestAlglibOld2008().TestLbfgsAlglibOld(dimension);
            }

            Console.WriteLine();
            return null;
        } 

        #endregion Actions

    }  // script
}

