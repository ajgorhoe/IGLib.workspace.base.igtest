// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Solver for single variable nonlinear exuations by the Newton method.</summary>
    public class ZeroFinderNewton : ZeroFinderBase
    {


        #region Construction

        
        // Construction with function definition of type IRealFunction:
        
        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for. May also contain method for derivative calculation.</param>
        /// <param name="toleranceFunctionValue">Tolerance on absolute function value in solution.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxStepSize">Maximal step size.
        /// <para>If less or equal to 0 then default value is taken.</para></param>
        /// <param name="numericalDifferentiationStep">Step size for numerical differentiation (when used internally).
        /// <para>If less or equal to 0 then the default value is used.</para></param>
        public ZeroFinderNewton(IRealFunction function, 
            double toleranceFunctionValue, int maxIterations,
            double maxStepSize, double numericalDifferentiationStep): 
                base(function, toleranceFunctionValue, maxIterations, maxStepSize, numericalDifferentiationStep)
        {  }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for. May also contain method for derivative calculation.</param>
        /// <param name="initialGuess">Initial guess.</param>
        /// <param name="toleranceFunctionValue">Tolerance on absolute function value in solution.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxStepSize">Maximal step size.
        /// <para>If less or equal to 0 then default value is taken.</para></param>
        /// <param name="numericalDifferentiationStep">Step size for numerical differentiation (when used internally).
        /// <para>If less or equal to 0 then the default value is used.</para></param>
        public ZeroFinderNewton(IRealFunction function,
            double initialGuess, double toleranceFunctionValue, int maxIterations,
            double maxStepSize, double numericalDifferentiationStep) :
            this(function, 
                 toleranceFunctionValue, maxIterations,
                 maxStepSize, numericalDifferentiationStep)
        {
            this.InitialGuess = initialGuess;
        }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for. May also contain method for derivative calculation.</param>
        /// <param name="initialGuess">Initial guess.</param>
        /// <param name="toleranceFunctionValue">Tolerance on absolute function value in solution.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        public ZeroFinderNewton(IRealFunction function,
            double initialGuess, double toleranceFunctionValue, int maxIterations) :
            this(function, 
                 initialGuess, toleranceFunctionValue, maxIterations,
                 0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        { }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for. May also contain method for derivative calculation.</param>
        public ZeroFinderNewton(IRealFunction function) :
            this(function,
             0 /* toleranceFunctionValue */, 0 /* maxIterations */,
             0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        { }



        #region Construction.WithDelegates


        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        /// <param name="functionDerivative">Derivative of the function whose zeros are searched for.
        /// <para>If null then derivatives will be calculated by numerical differentiation when necessary.</para></param>
        /// <param name="toleranceFunctionValue">Tolerance on absolute function value in solution.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxStepSize">Maximal step size.
        /// <para>If less or equal to 0 then default value is taken.</para></param>
        /// <param name="numericalDifferentiationStep">Step size for numerical differentiation (when used internally).
        /// <para>If less or equal to 0 then the default value is used.</para></param>
        public ZeroFinderNewton(DlgFunctionValue function, DlgFunctionValue functionDerivative,
            double toleranceFunctionValue, int maxIterations, 
            double maxStepSize, double numericalDifferentiationStep): 
                base(function, functionDerivative,
                toleranceFunctionValue, maxIterations, 
                maxStepSize, numericalDifferentiationStep)
        {  }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        /// <param name="functionDerivative">Derivative of the function whose zeros are searched for.
        /// <para>If null then derivatives will be calculated by numerical differentiation when necessary.</para></param>
        /// <param name="initialGuess">Initial guess.</param>
        /// <param name="toleranceFunctionValue">Tolerance on absolute function value in solution.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxStepSize">Maximal step size.
        /// <para>If less or equal to 0 then default value is taken.</para></param>
        /// <param name="numericalDifferentiationStep">Step size for numerical differentiation (when used internally).
        /// <para>If less or equal to 0 then the default value is used.</para></param>
        public ZeroFinderNewton(DlgFunctionValue function, DlgFunctionValue functionDerivative,
            double initialGuess, double toleranceFunctionValue, int maxIterations, 
            double maxStepSize, double numericalDifferentiationStep): 
                this(function, functionDerivative,
                toleranceFunctionValue, maxIterations, 
                maxStepSize, numericalDifferentiationStep)
        {
            this.InitialGuess = initialGuess;
        }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        /// <param name="functionDerivative">Derivative of the function whose zeros are searched for.
        /// <para>If null then derivatives will be calculated by numerical differentiation when necessary.</para></param>
        /// <param name="initialGuess">Initial guess.</param>
        /// <param name="toleranceFunctionValue">Tolerance on absolute function value in solution.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        public ZeroFinderNewton(DlgFunctionValue function, DlgFunctionValue functionDerivative,
            double initialGuess, double toleranceFunctionValue, int maxIterations) :
                this(function, functionDerivative,
                 initialGuess, toleranceFunctionValue, maxIterations,
                 0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        {  } 

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        /// <param name="functionDerivative">Derivative of the function whose zeros are searched for.
        /// <para>If null then derivatives will be calculated by numerical differentiation when necessary.</para></param>
        public ZeroFinderNewton(DlgFunctionValue function, DlgFunctionValue functionDerivative) :
            this(function, functionDerivative,
             0 /* toleranceFunctionValue */, 0 /* maxIterations */,
             0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        {  }
 
        
        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.
        /// <para>Since function for calculation of function derivatives is not set, derivatives will be
        /// calculated numerically if needed.</para></summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        public ZeroFinderNewton(DlgFunctionValue function) :
            this(function, null /* functionDerivative */ )
        { }

    
        #endregion Construction.WithDelegates


        #endregion Construction



        #region Constants

        protected static int _defaultMaxIterationsNewton = 100;

        /// <summary>Default value for the maximal number of iterations for the Newton's method.
        /// <para>Must be greater or equal to 1.</para></summary>
        public static int DefaultMaxIterationsNewton
        {
            get { return _defaultMaxIterationsNewton; }
            set {
                if (value < 1)
                    throw new ArgumentException("Default maximal number of iterations can not be less than 1.");
                _defaultMaxiIterations = value; }
        }


        protected static double _defaultMaxStepSizeNewton = 1.0e50;

        /// <summary>Default value for the maximal step size in the Newton's method.</summary>
        public static double DefauleMaxStepSizeNewton
        {
            get { return _defaultMaxStepSizeNewton; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Default maximal step size for the Newton's method can not be less or equal to 0.");
                _defaultMaxStepSizeNewton = value;
            }
        }


        #endregion Constants


        #region Data


        protected int _maxIterationsNewton = DefaultMaxIterations;

        /// <summary>Maximal number of iterations.
        /// <para>Must be greater or equal to 1.</para>
        /// <para>Default value is defined by the <see cref="ZeroFinderNewton.DefaultMaxIterationsNewton"/> property.</para></summary>
        public override int MaxIterations
        {
            get { return _maxIterationsNewton; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Maximal number of iterations can not be less than 1.");
                }
                _maxIterationsNewton = value;
            }
        }


        protected double _maxStepSizeNewton = DefauleMaxStepSizeNewton;

        /// <summary>Maximal step size for zefo finding by teh Newton method.
        /// <para>Must be greater than 0.</para>
        /// <para>Default value is defined by the <see cref="ZeroFinderNewton.DefauleMaxStepSizeNewton"/> property.</para></summary>
        public override double MaxStepSize
        {
            get { return _maxStepSizeNewton; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Maximal step size for the Newton's method can not be less or equal to 0.");
                _maxStepSizeNewton = value;
            }
        }


        #endregion Data


        #region Operation


        /// <summary>Tries to find problem solution (function zero) by the Newton's method according to settings and solution data.</summary>
        public override void Solve()
        {
            ResetSolutionData(); // reset solution data
            if (OutputLevel > 0)
                Console.WriteLine(Environment.NewLine
                    + "Searching for function zero by the Newton's method. " + Environment.NewLine
                    + "  Initial guess: " + InitialGuess + Environment.NewLine
                    + "  Tolerance on function absolute value: " + ToleranceFunctionValue + Environment.NewLine
                    + "  Maximal number of iterations: " + MaxIterations + Environment.NewLine
                    + "  Step limit: " + MaxStepSize);
            // Initialize the loop:
            bool doStop = false;
            double x = InitialGuess;
            double f = FunctionValue(InitialGuess);
            double der = FunctionDerivative(InitialGuess);
            Solution = x;
            FunctionValueInSolution = f;
            NumIterations = 0;
            IsSolutionObtained = false;
            if (Math.Abs(f) < ToleranceFunctionValue)
            {
                doStop = true;
                IsSolutionObtained = true;
            }
            while (!doStop)
            {
                ++NumIterations;
                // Calculate the step by the Newton's formula:
                double step = -f / der;
                // Impose the step limit:
                if (Math.Abs(step) > MaxStepSize)
                {
                    if (step < 0)
                        step = -MaxStepSize;
                    else
                        step = MaxStepSize;
                }
                // Calculate the new (trial) guess:
                double xNew = x + step;
                double fNew = FunctionValue(xNew);
                int numStepReductions = 0;
                while (Math.Abs(fNew) >= Math.Abs(f) && numStepReductions < MaxStepReductions)
                {
                    step /= StepReductionFactor;
                    xNew = x + step;
                    fNew = FunctionValue(xNew);
                    ++numStepReductions;
                    if (OutputLevel > 0)
                        Console.WriteLine("  Iteration " + NumIterations + ": step reduction No. " + numStepReductions
                            + Environment.NewLine + "    step = " + step
                            + Environment.NewLine + "       x = " + xNew
                            + Environment.NewLine + "    f(x) = " + fNew);
                }
                double derNew = FunctionDerivative(xNew);
                if (Math.Abs(fNew) < Math.Abs(f))
                {
                    // We obtained a better guess, write it to the output:
                    Solution = x;
                    FunctionValueInSolution = f;
                }
                x = xNew;
                f = fNew;
                der = derNew;
                // Check for stopping conditions:
                if (Math.Abs(FunctionValueInSolution) < ToleranceFunctionValue)
                {
                    // Conditions fulfilled, function zero was found successfully:
                    doStop = true;
                    IsSolutionObtained = true;
                }
                else
                {
                    if (NumIterations >= MaxIterations)
                        doStop = true;
                }
                if (OutputLevel > 0)
                {
                    Console.WriteLine("Iteration No. " + NumIterations + ": "
                        + Environment.NewLine + "      x = " + x
                        + Environment.NewLine + "   f(x) = " + f
                        + Environment.NewLine + "  f'(x) = " + der
                        + Environment.NewLine + "   step = " + step
                        );
                }
                if (doStop && OutputLevel > 0)
                {
                    if (IsSolutionObtained)
                    {
                        Console.WriteLine(
                            Environment.NewLine + "Function zero found successfully."
                            + Environment.NewLine + "  Number of iterations: " + NumIterations
                            + Environment.NewLine + "              Solution: " + Solution
                            + Environment.NewLine + "        Function value: " + FunctionValueInSolution
                            );
                    }
                }
            }
        }  // Solve()

        
        #endregion Operation

    }


}
