using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AP1;
using AlgLib;

namespace NumLib
{

    /// <summary>Performs minimization of multivariate functions by the LBFGS method.</summary>
    public class MinimizerLbfgs: MinimizerBase
    {

        /// <summary>Constructs a new function mnimizer.</summary>
        /// <param name="function">Function to be minimized.</param>
        /// <param name="initialGuess">Initial guess.</param>
        /// <param name="maxIterations">Maximal number of iterations.</param>
        /// <param name="tolGrad">Tolerance on norm of function gradient.</param>
        /// <param name="tolValue">Tolerance on error in function value.</param>
        /// <param name="tolParameters">Tolerance on norm of error in parameters.</param>
        /// <remarks>This class provides an object-oriented minimization tool. It wraps the 
        /// AlgLib BFGS algorithm in order to perform the minimization.</remarks>
        public MinimizerLbfgs(FunctionWithGradient function, double[] initialGuess, int maxIterations,
            double tolGrad, double tolValue, double tolParameters)
        {
            this.Function = function;
            this.InitialGuess = initialGuess;
            this.MaxIterations = maxIterations;
            this.ToleranceFunctionGradient = tolGrad;
            this.ToleranceFunctionValue = tolValue;
            this.ToleranceParameters = tolParameters;
        }

        #region Constants


        protected static int _defaultMaxIterationsLbfgs = 500;

        /// <summary>Default value for the maximal number of iterations for the Lbfgs method.
        /// <para>Must be greater or equal to 1.</para></summary>
        public static int DefaultMaxIterationsLbfgs
        {
            get { return _defaultMaxIterationsLbfgs; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Default maximal number of iterations can not be less than 1.");
                _defaultMaxiIterations = value;
            }
        }

        #endregion Constants


        #region Data.Settings

        protected int _maxIterationsLbfgs = DefaultMaxIterationsLbfgs;

        /// <summary>Maximal number of iterations.
        /// <para>Must be greater or equal to 1.</para>
        /// <para>Default value is defined by the <see cref="MinimizerLbfgs.DefaultMaxIterationsLbfgs"/> property.</para></summary>
        public override int MaxIterations
        {
            get { return _maxIterationsLbfgs; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Maximal number of iterations can not be less than 1.");
                }
                _maxIterationsLbfgs = value;
            }
        }

        #endregion Data.Settings


        #region ProblemData

        #endregion ProblemData


        #region Operation

        /// <summary>LBFGS algorithm's state struct.</summary>
        protected lbfgs.lbfgsstate LbfgsState;
        
        /// <summary>LBFGS algorithm's results.</summary>
        protected lbfgs.lbfgsreport LbfgsReport;

        /// <summary>Number of Hessian corrections.</summary>
        int numHessianCorrections = 2;

        /// <summary>Flags used for the LBFGS algorithm.</summary>
        int flags = 0;

        /// <summary>Initializes the solution iteration.</summary>
        public override void InitializeIteration()
        {
            if (OutputLevel >= 1)
            {
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Initializing minimization procedure...");
            }

            LbfgsState = new lbfgs.lbfgsstate();
            LbfgsReport = new lbfgs.lbfgsreport();

            numHessianCorrections = 7;
            if (numHessianCorrections > this.NumParameters)
                numHessianCorrections = this.NumParameters;

            double[] initialGuess = new double[NumParameters];
            for (int i = 0; i < initialGuess.Length; ++i)
            {
                initialGuess[i] = InitialGuess[i];
            }

            lbfgs.minlbfgs(
                NumParameters,
                numHessianCorrections,
                ref initialGuess,
                ToleranceFunctionGradient,
                ToleranceFunctionValue,
                ToleranceParameters,
                MaxIterations,
                flags,
                ref LbfgsState);

        }

        /// <summary>Optimization of quadratic functions.</summary>
        public override void NextIteration()
        {
            ++IterationCount;

            if (lbfgs.minlbfgsiteration(ref LbfgsState))
            {
                _isConvergenceCriteriaMet = false;
                // Calculate function and its gradient by a function object and store them in the state struct:
                LbfgsState.f = Function.Value(LbfgsState.x);
                LbfgsState.g = Function.Gradient(LbfgsState.x);
            } else
            {
                _isConvergenceCriteriaMet = true;
            }
            if (OutputLevel > -1)
            {
                Console.WriteLine("It. {0}: f(x) = {1,8:F4}, ||grad f(x)|| = {2,8:F5} ", 
                    IterationCount, LbfgsState.f, FunctionWithGradient.Norm(LbfgsState.g));
                Console.WriteLine("  x = " + FunctionWithGradient.ArrayToString(LbfgsState.x));
            }
        }
        
        /// <summary>Optimization of quadratic functions.</summary>
        public override void Minimize()
        {
            InitializeIteration();
            if (OutputLevel >= 1)
            {
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Starting minimization...");
            }
            do
            {
                NextIteration();
            } while (!IsConvergenceCriteriaMet && IterationCount < MaxIterations);

            Console.WriteLine(Environment.NewLine + "Minimization finished." + Environment.NewLine);

            double[] result = SolutionParameters;
            bool resize = false;
            if (result == null)
                resize = true;
            else if (result.Length != NumParameters)
                resize = true;
            if (resize)
                result = new double[NumParameters];

            lbfgs.minlbfgsresults(ref LbfgsState, ref result, ref LbfgsReport);
            // Store results:
            SolutionParameters = result;
            SolutionValue = Function.Value(result);
            SolutionGradient = Function.Gradient(result);

            var x = LbfgsReport.terminationtype;

            // Output results
            if (OutputLevel >= 0)
            {
                Console.WriteLine(Environment.NewLine + "Minimization finished.");
                Console.Write("Function: ");
                Console.WriteLine("  " + Function.Description);
                Console.WriteLine("Minimum:");
                Console.WriteLine("  x = " + FunctionWithGradient.ArrayToString(SolutionParameters));  // LbfgsState.x));
                Console.WriteLine("  f(x) = " + SolutionValue);
                Console.WriteLine("  grad f = " + FunctionWithGradient.ArrayToString(SolutionGradient));
                Console.WriteLine("  ||grad f(x)|| = " + FunctionWithGradient.Norm(SolutionGradient));
                Console.WriteLine();
            }

        } // Minimize()


        #endregion Operation

    }  // MinimizerLbfgs

}
