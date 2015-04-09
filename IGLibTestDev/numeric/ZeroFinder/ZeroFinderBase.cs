// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{
    /*
      REMARKS:
     
       This file contains a base class for solvers for nonlinear equations of a single variable
     (or, identically, zero finders of nonlinear functions of a single variable).
     
       The class is designed in a similar way as wolud be professional classes for numerical solvers
     for finding function zeros, therefore it can be used as example how such classes can be designed.
     * */

    /// <summary>Base class for single variable nonlinear exuation solvers.</summary>
    public abstract class ZeroFinderBase
    {


        #region Construction

        // Construction with function definition of type IRealFunction:
        
        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for. May also contain method for derivative calculation.</param>
        /// <param name="maxIterations">Maximal number of iteratioins.
        /// <para>If less or equal to 0 then the default value is taken.</para></param>
        /// <param name="maxStepSize">Maximal step size.
        /// <para>If less or equal to 0 then default value is taken.</para></param>
        /// <param name="numericalDifferentiationStep">Step size for numerical differentiation (when used internally).
        /// <para>If less or equal to 0 then the default value is used.</para></param>
        public ZeroFinderBase(IRealFunction function, 
            double toleranceFunctionValue, int maxIterations,
            double maxStepSize, double numericalDifferentiationStep)
        {
            this.Function = function;
            if (toleranceFunctionValue > 0)
                this.ToleranceFunctionValue = toleranceFunctionValue;
            if (maxIterations > 0)
                this.MaxIterations = maxIterations;
            if (maxStepSize > 0)
                this.MaxStepSize = maxStepSize;
            if (numericalDifferentiationStep > 0)
                this.NumericalDifferentiationStep = numericalDifferentiationStep;
        }

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
        public ZeroFinderBase(IRealFunction function,
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
        public ZeroFinderBase(IRealFunction function,
            double initialGuess, double toleranceFunctionValue, int maxIterations) :
            this(function, 
                 initialGuess, toleranceFunctionValue, maxIterations,
                 0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        { }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for.</param>
         public ZeroFinderBase(IRealFunction function) :
            this(function,
             0 /* toleranceFunctionValue */, 0 /* maxIterations */,
             0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        { }


        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        private ZeroFinderBase(): this((IRealFunction) null)
        {  }


        #region Construction.WithDelegates

        // Construction with delegates for evaluation of function values and eventually derivatives:

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
        public ZeroFinderBase(DlgFunctionValue function, DlgFunctionValue functionDerivative,
            double toleranceFunctionValue, int maxIterations, 
            double maxStepSize, double numericalDifferentiationStep)
        {
            SetFunction(function, functionDerivative);
            if (toleranceFunctionValue>0)
                this.ToleranceFunctionValue = toleranceFunctionValue;
            if (maxIterations>0)
                this.MaxIterations = maxIterations;
            if (maxStepSize > 0)
                this.MaxStepSize = maxStepSize;
            if (numericalDifferentiationStep > 0)
                this.NumericalDifferentiationStep = numericalDifferentiationStep;
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
        /// <param name="maxStepSize">Maximal step size.
        /// <para>If less or equal to 0 then default value is taken.</para></param>
        /// <param name="numericalDifferentiationStep">Step size for numerical differentiation (when used internally).
        /// <para>If less or equal to 0 then the default value is used.</para></param>
        public ZeroFinderBase(DlgFunctionValue function, DlgFunctionValue functionDerivative,
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
        public ZeroFinderBase(DlgFunctionValue function, DlgFunctionValue functionDerivative,
            double initialGuess, double toleranceFunctionValue, int maxIterations) :
                this(function, functionDerivative,
                 initialGuess, toleranceFunctionValue, maxIterations,
                 0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        {  } 

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.</summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        /// <param name="functionDerivative">Derivative of the function whose zeros are searched for.
        /// <para>If null then derivatives will be calculated by numerical differentiation when necessary.</para></param>
        public ZeroFinderBase(DlgFunctionValue function, DlgFunctionValue functionDerivative) :
            this(function, functionDerivative,
             0 /* toleranceFunctionValue */, 0 /* maxIterations */,
             0 /* maxStepSize */, 0 /* numericalDifferentiationStep */)
        {  }

        /// <summary>Constructs a new solver for finding zeros of nonlinear functions.
        /// <para>Since function for calculation of function derivatives is not set, derivatives will be
        /// calculated numerically if needed.</para></summary>
        /// <param name="function">Function whose zeros are searched for.</param>
        public ZeroFinderBase(DlgFunctionValue function) :
            this(function, null /* functionDerivative */ )
        { }
        
        
        #endregion Construction.WithDelegates


        #endregion Construction


        #region Constants

        protected static int _defaultOutputLevel = 0;

        /// <summary>Default output level for nonlinear equation solvers.
        /// <para>Output level defines to which extent operation of the solver is logged to the console.</para></summary>
        public static int DefaultOutputLevel
        {
            get { return _defaultOutputLevel; }
            set
            {
                if (value < 0)
                    value = 0;
                _defaultOutputLevel = value;
            }
        }

        protected static int _defaultMaxiIterations = 1000;

        /// <summary>Default value for the maximal number of iterations.
        /// <para>Must be greater or equal to 1.</para></summary>
        public static int DefaultMaxIterations
        {
            get { return _defaultMaxiIterations; }
            set {
                if (value < 1)
                    throw new ArgumentException("Default maximal number of iterations can not be less than 1.");
                _defaultMaxiIterations = value;
            }
        }


        protected static double _defaultInitialGuess = 0;

        /// <summary>Default value for the initial guess in solvers for finding zeros of nonlinear functions.</summary>
        public static double DefaultInitialGuess
        {
            get { return _defaultInitialGuess; }
            set { _defaultInitialGuess = value; }
        }

        protected static double _defaultToleranceFunctionValue = 1.0e-6;
        
        /// <summary>Default tolerance on absolute value of the function in solution.
        /// <para>Must be a positive value.</para></summary>
        public static double DefaultToleranceFunctionValue
        {
            get { return _defaultToleranceFunctionValue; }
            set {
                if (value <= 0)
                    throw new ArgumentException("Default tolerance on absolute function value can not be less or equal to 0.");
                _defaultToleranceFunctionValue = value; }
        }


        protected static double _defaultMaxStepSize = double.MaxValue;

        /// <summary>Default value for maximal step size.
        /// <para>Must be greater than 0.</para></summary>
        public static double DefaultMaxStepSize
        {
            get { return _defaultMaxStepSize; }
            set {
                if (value <= 0)
                    throw new ArgumentException("Default Maximal step size must be greater than 0.");
                _defaultMaxStepSize = value;
            }
        }

        protected static double _defaultStepReductionFactor = 4.0;

        /// <summary>Default value for step reduction factor.
        /// <para>Must be greater than one.</para>
        /// <para>By this factor, step size is reduced (sometimes iteratively) if improvement in solution quality
        /// is not achieved.</para></summary>
        public static double DefaultStepReductionFactor
        {
            get { return _defaultStepReductionFactor; }
            set
            {
                if (value <= 1)
                    throw new ArgumentException("Default step reduction factor must be greater than 1.");
                _defaultStepReductionFactor = value;
            }
        }

        protected static int _defaultMaxStepReductions = 20;

        /// <summary>Default value for maximal number of step reductions.
        /// <para>Must be greater or equal to 0. 0 means that step reductions are not allowed.</para>
        /// <para>In some algorithms, step size is reduced (sometimes iteratively) if improvement in solution quality
        /// is not achieved.</para></summary>
        public static int DefaultMaxStepReductions
        {
            get { return _defaultMaxStepReductions; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Default maximal number of step reductions must be greater or equal to 0.");
                _defaultMaxStepReductions = value;
            }
        }

        protected static double _defaultNumericalDifferentiationStep = 1.0e-4;

        /// <summary>Default value for the step size for numerical differentiation.
        /// <para>Must be greater than 0.</para>
        /// <para>Numerical differentiation is sometimes used if the zero searching algorithm uses derivatives and 
        /// function derivative is not explicitly specified.</para></summary>
        public static double DefaultNumericalDifferentiationStep
        {
            get { return _defaultNumericalDifferentiationStep; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Default step size for numerical differentiation must be greater than 0.");
                _defaultNumericalDifferentiationStep = value;
            }
        }



        #endregion Constants


        #region Data.Settings


        protected int _outputLevel = DefaultOutputLevel;


        /// <summary>Level of console output for nonlinear equation solvers.
        /// <para>Output level defines to which extent operation of the solver is logged to the console.</para>
        /// <para>Should be greater or equal to 0, 0 means no output is generated on the application's console.</para>
        /// <para>Default value is specified by the <see cref="ZeroFinderBase.DefaultOutputLevel"/> property.</para></summary>
        public virtual int OutputLevel
        {
            get { return _outputLevel; }
            set
            {
                if (value < 0)
                    value = 0;
                _outputLevel = value;
            }
        }

        protected int _maxIterations = DefaultMaxIterations;

        /// <summary>Maximal number of iterations.
        /// <para>Must be greater or equal to 1.</para>
        /// <para>Default value is defined by the <see cref="ZeroFinderBase.DefaultMaxIterations"/> property.</para></summary>
        public virtual int MaxIterations
        {
            get { return _maxIterations; }
            set {
                if (value < 1)
                {
                    throw new ArgumentException("Maximal number of iterations can not be less than 1.");
                }
                _maxIterations = value;
            }
        }


        protected double _toleranceFunctionValue = DefaultToleranceFunctionValue;

        /// <summary>Tolerance on absolute value of the function in solution.
        /// <para>Must be a positive value.</para>
        /// <para>Default value is defined by the <see cref="ZeroFinderBase.DefaultToleranceFunctionValue"/> property.</para></summary>
        public virtual double ToleranceFunctionValue
        {
            get { return _toleranceFunctionValue; }
            set
            {
                if (value<=0)
                    throw new ArgumentException("Tolerance on absolute function value can not be less or equal to 0.");
                _toleranceFunctionValue = value;
            }
        }

        protected double _initialGuess = DefaultInitialGuess;

        /// <summary>Initial guess for finding function zero.
        /// <para>Default value is defined by the <see cref="ZeroFinderBase.DefaultInitialGuess"/> property.</para></summary>
        public double InitialGuess
        {
            get { return _initialGuess; }
            set { _initialGuess = value; }
        }

        protected double _maxStepSize = DefaultMaxStepSize;

        /// <summary>Default value maximal step size.
        /// <para>Must be greater than 0. Default value is specified by <see cref="ZeroFinderBase.DefaultMaxStepSize"/>.</para>
        /// <para>With this value, step size during iterations is limited. This can be used as means of stabilization of the algorithm.</para></summary>
        public virtual double MaxStepSize
        {
            get { return _maxStepSize; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Maximal step size must be greater than 0.");
                _maxStepSize = value;
            }
        }

        
        protected double _stepReductionFactor = DefaultStepReductionFactor;

        /// <summary>Step reduction factor.
        /// <para>Must be greater than one. Default value is <see cref="ZeroFinderBase.DefaultStepReductionFactor"/>.</para>
        /// <para>By this factor, step size is reduced (sometimes iteratively) if improvement in solution quality
        /// is not achieved.</para></summary>
        public virtual double StepReductionFactor
        {
            get { return _stepReductionFactor; }
            set
            {
                if (value <= 1)
                    throw new ArgumentException("Step reduction factor must be greater than 1.");
                _stepReductionFactor = value;
            }
        }


        protected int _maxStepReductions = DefaultMaxStepReductions;

        /// <summary>Maximal number of step reductions.
        /// <para>Must be greater or equal to 0. 0 means that step reductions are not allowed.
        /// Default value is <see cref="ZeroFinderBase.DefaultMaxStepReductions"/>.</para>
        /// <para>In some algorithms, step size is reduced (sometimes iteratively) if improvement in solution quality
        /// is not achieved.</para></summary>
        public virtual int MaxStepReductions
        {
            get { return _maxStepReductions; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Maximal number of step reductions must be greater or equal to 0.");
                _maxStepReductions = value;
            }
        }


        protected double _numericalDifferentiationStep = DefaultNumericalDifferentiationStep;

        /// <summary>Step size for numerical differentiation.
        /// <para>Must be greater than 0.</para>
        /// <para>Numerical differentiation is sometimes used if the zero searching algorithm uses derivatives and 
        /// function derivative is not explicitly specified.</para></summary>
        public virtual double NumericalDifferentiationStep
        {
            get { return _numericalDifferentiationStep; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Step size for numerical differentiation must be greater than 0.");
                _numericalDifferentiationStep = value;
            }
        }

        #endregion Data.Settings


        #region Data.Operation

        protected IRealFunction _function;

        public IRealFunction Function
        {
            get { return _function; }
            set { 
                _function = value;
                // Update dependencies:
                ResetSolutionAndProblemDependentData();
            }
        }

        //protected DlgFunctionValue _functionValueDelegate;

        ///// <summary>Delegate used for evaluation of function whose zero is searched for.</summary>
        //public virtual DlgFunctionValue FunctionValueDelegate
        //{
        //    get { return _functionValueDelegate; }
        //    set {
        //        _functionDerivativeDelegate = null;
        //        _functionValueDelegate = value;
        //        // Update dependencies:
        //        ResetSolutionAndProblemDependentData();
        //    }
        //}

        //protected DlgFunctionValue _functionDerivativeDelegate;

        ///// <summary>Delegate used for evaluation of derivative of the function whose zero is searched for.</summary>
        //public virtual DlgFunctionValue FunctionDerivativeDelegate
        //{
        //    get { return _functionDerivativeDelegate; }
        //    set { _functionDerivativeDelegate = value;
        //        // Update dependencies:
        //        ResetSolutionAndProblemDependentData();
        //    }
        //}




        /// <summary>Sets definition of the function whose zeros are searched for.</summary>
        /// <param name="valueDelegate">Delegate used for calculation of function values.</param>
        /// <param name="derivativeDelegate">Delegate used for calculation of function derivatives.</param>
        public void SetFunction(DlgFunctionValue valueDelegate, DlgFunctionValue derivativeDelegate)
        {
            IRealFunction function = new RealFunction(valueDelegate, derivativeDelegate);
            this.Function = function;
        }


        #endregion Data.Operation


        #region Data.Solution


        /// <summary>Resets output data that contains information about the solution and solution procedure.</summary>
        protected virtual void ResetSolutionData()
        {
            IsSolutionObtained = false;
            NumIterations = 0;
            NumFunctionEvaluations = 0;
            NumDerivativeEvaluations = 0;
            Solution = double.NaN;
            FunctionValueInSolution = double.NaN;
        }

        /// <summary>Resets data that is not valid any more when problem changes.</summary>
        protected virtual void ResetProblemDependentData()
        {
            _lastFunctionArgument = double.NaN;
            _lastFunctionValue = double.NaN;
        }

        
        /// <summary>Resets the following data: 
        /// <para>  - output data that contains information about the solution and solution procedure</para>
        /// <para>  - data that is not valid any more when problem changes.</para></summary>
        protected virtual void ResetSolutionAndProblemDependentData()
        {
            ResetSolutionData();
            ResetProblemDependentData();
        }
        
        protected int _numIterations = 0;

        /// <summary>Number of iterations performed up to the current point.
        /// <para>After the algorithm completes, this contains the total number of iterations.</para></summary>
        public virtual int NumIterations
        {
            get { return _numIterations; }
            protected set { _numIterations = value; }
        }


        protected int _numFunctionEvaluations = 0;

        /// <summary>Number of function evaluations performed by the solution algorithm up to the current point.
        /// <para>After the algorithm completes, this contains the total number of function evaluations.</para>
        /// <para>In the case that numerical differentiation is used, additional function evaluations that are used in
        /// numerical differentiation are counted in.</para></summary>
        public virtual int NumFunctionEvaluations
        {
            get { return _numFunctionEvaluations; }
            protected set { _numFunctionEvaluations = value; }
        }

        protected int _numDerivativeEvaluations = 0;

        /// <summary>Number of function derivative evaluations performed by the solution algorithm up to the current point.
        /// <para>After the algorithm completes, this contains the total number of derivative evaluations.</para></summary>
        public virtual int NumDerivativeEvaluations
        {
            get { return _numDerivativeEvaluations; }
            protected set { _numDerivativeEvaluations = value; }
        }


        protected double _solution = double.NaN;

        /// <summary>Calculated solution of the problem (i.e. zero of the function whose value is calculated
        /// by the <see cref="FunctionValue"/> method).
        /// <para>Valid if <see cref="IsSolutionObtained"/> == true.</para></summary>
        public virtual double Solution
        {
            get { return _solution; }
            protected set { _solution = value; }
        }


        protected double _functionValueInSolution = double.NaN;

        /// <summary>Value of the function whose zero is searched for in the calculated approximate solution.
        /// <para>Valid if <see cref="IsSolutionObtained"/> == true.</para></summary>
        public double FunctionValueInSolution
        {
            get { return _functionValueInSolution; }
            protected set { _functionValueInSolution = value; }
        }


        protected bool _isSolutionObtained = false;

        /// <summary>Flag specifying whether the problem has been successfully solved.
        /// <para>Value becomes true after the solution algorithm is successfully performed and yields a solution that
        /// is within the prescribed tolerances. Otherwise, the value is false.</para></summary>
        public bool IsSolutionObtained
        {
            get { return _isSolutionObtained; }
            protected set { _isSolutionObtained = value; }
        }


        #endregion Data.Solution


        #region Operation

        protected bool _recordLastFunctionValue = true;
        protected double
            _lastFunctionArgument = double.NaN,
            _lastFunctionValue = double.NaN;

        /// <summary>calculates and returns value of the function whose zeros are searched for at the specified value of independent variable.</summary>
        /// <param name="x">Argument of the function (value of independent variable) for which function value is returned.</param>
        /// <returns>Function value.</returns>
        /// <remarks>
        /// <para>Delegate <see cref="FunctionValueDelegate"/> is used for calculation of function value.</para>
        /// <para>Function remembers last evaluated values and stores them, which speeds up some operation such as
        /// evaluation of numerical derivatives. Last function argument is stored to the internal variable <see cref="_lastFunctionArgument"/>
        /// and function value at that argument is stored to the internal variable <see cref="_lastFunctionValue"/>. Before any function
        /// value is calculated, these variables have the value <see cref="double.NaN"/>. Fields are also reset to these values
        /// when definition of the function changes. Implementers of derived classes are responsible to restore this behavior
        /// if properties that interact with function definitions are overriden. If the <see cref="_recordLastFunctionValue"/> field
        /// is set to false then the last calculated value is not remembered. This is used e.g. in the <see cref="NumericalFunctionDerivative"/>
        /// method for calculation of numerical derivatives to switch off recording of the last function value when function is evaluated
        /// at perturbed parameters.</para>
        /// </remarks>
        public virtual double FunctionValue(double x)
        {
            if (x == _lastFunctionArgument)
                return _lastFunctionValue;
            bool isValueDefined = false;
            if (Function != null)
                if (Function.ValueDefined)
                    isValueDefined = true;
            if (!isValueDefined)
            {
                _lastFunctionArgument = double.NaN;
                _lastFunctionValue = double.NaN;
                if (Function == null)
                    throw new InvalidOperationException("Function value can not be calculated, function not defined.");
                else
                    throw new InvalidCastException("Function does not have a method for calculating its value defined.");
            }
            double value = Function.Value(x);
            if (_recordLastFunctionValue)
            {
                _lastFunctionArgument = x;
                _lastFunctionValue = value;
            }
            return value;
        }


        /// <summary>calculates and returns derivative of the function whose zeros are searched for at the specified value of independent variable.</summary>
        /// <param name="x">Argument of the function (value of independent variable) for which function derivative is returned.</param>
        /// <returns>Function value.</returns>
        /// <remarks>
        /// <para>Delegate <see cref="FunctionDerivativeDelegate"/> is used for calculation of function value when it is defined (non-null).</para>
        /// <para>If the derivative delegate (<see cref="FunctionDerivativeDelegate"/>) is not defined then function derivative is calculated 
        /// numerically by the <see cref="NumericalFunctionDerivative"/> method.</para>
        /// </remarks>
        public virtual double FunctionDerivative(double x)
        {
            bool isDerivativeDefined = false;
            if (Function != null)
                if (Function.DerivativeDefined)
                    isDerivativeDefined = true;
            if (isDerivativeDefined)
                return Function.Derivative(x);
            else
                return NumericalFunctionDerivative(x);
        }

        /// <summary>Calculates and returns numerical derivative of the function whose zero is searched for.</summary>
        /// <param name="x">Function argumebnt at which numerical derivative is calculated.</param>
        /// <remarks><para>Property <see cref="NumericalDifferentiationStep"/> is used as step size for numerical differentiation.</para>
        /// <para>This method uses the forward difference scheme for numerical differentiation.</para></remarks>
        public virtual double NumericalFunctionDerivative(double x)
        {
            double step = NumericalDifferentiationStep;
            double f = FunctionValue(x);
            bool recordLastFunctionValueSaved = _recordLastFunctionValue;
            double fPlus;
            try
            {
                _recordLastFunctionValue = false;
                fPlus = FunctionValue(x + step);
            }
            catch { throw; }
            finally { _recordLastFunctionValue = recordLastFunctionValueSaved; }
            return (fPlus - f) / step;
        }


        /// <summary>Tries to find problem solution according to settings and solution data.
        /// <para>This method must be overridden in derived classes.</para></summary>
        public abstract void Solve();


        #endregion Operation


        #region Examples

        

        /// <summary>Example where a nonlinear equation equation is solved by a solver class for finding zeros of a function by the Newton's method.</summary>
        /// <remarks><para>Equation solved is defined by f(x) == 0, where the function f(x) is represented by the 
        /// <see cref="ExampleFunction"/> method and its derivative by the <see cref="ExampleFunctionDerivative"/> method.</para>
        /// <para>Class <see cref="ZeroFinderNewton"/> is used to solve the equation.</para></remarks>
        public static void ExampleNewton()
        {
            ExampleNewton(ExampleFunction, ExampleFunctionDerivative);
        }

        /// <summary>Example where a nonlinear equation equation is solved by a solver class for finding zeros of a function by the Newton's method.</summary>
        /// <param name="function">Delegate used for calculation of values of the function whose zeros are searched for.</param>
        /// <param name="functionDerivative">Delegate used for calculation of derivatives of the function whose zeros are searched for.</param>
        /// <remarks><para>Equation solved is defined by f(x) == 0, where the function f(x) is represented by the 
        /// <paramref name="function"/> delegate and its derivative by the <paramref name="functionDerivative"/> delegate.</para>
        /// <para>Class <see cref="ZeroFinderNewton"/> is used to solve the equation.</para></remarks>
        public static void ExampleNewton(DlgFunctionValue function, DlgFunctionValue functionDerivative)
        {

            ZeroFinderNewton solver = new ZeroFinderNewton(function, functionDerivative);

            solver.InitialGuess = -5;
            solver.ToleranceFunctionValue = 1.0e-8;
            solver.MaxStepSize = 1000.0;
            solver.MaxIterations = 50;
            solver.OutputLevel = 1;
            Console.WriteLine(Environment.NewLine
                + "Solving equation by the newton method, implementation by a solver class...");

            // Call the solver method to find function zero:
            solver.Solve();

            Console.WriteLine(Environment.NewLine + "Solution procedure finished."
                + Environment.NewLine + "Results: "
                + Environment.NewLine + "                 Successfull: " + (solver.IsSolutionObtained ? "Yes." : "No.")
                + Environment.NewLine + "               Function zero: " + solver.Solution
                + Environment.NewLine + "  Function value in solution: " + solver.FunctionValueInSolution
                + Environment.NewLine + "           Test of the above: " + function(solver.Solution)
                + Environment.NewLine + "  Number of iterations spent: " + solver.NumIterations);
        }


        /// <summary>Example function whose zero is searched for in the example.
        /// <para>f(x) = exp(x) - 10</para>
        /// <para>Equation to be solved: exp(x) == 10</para></summary>
        public static double ExampleFunction(double x)
        {
            return (Math.Exp(x) - 10);
        }

        /// <summary>Derivative of the example function whose zero is searched for in the example.
        /// <para>f(x) = exp(x) - 10</para>
        /// <para>f'(x) = exp(x)</para>
        /// <para>Equation to be solved: exp(x) == 10</para></summary>
        public static double ExampleFunctionDerivative(double x)
        {
            return Math.Exp(x);
        }


        #endregion Examples

    }
}
