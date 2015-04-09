using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumLib
{


    public abstract class MinimizerBase
    {


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
            set
            {
                if (value < 1)
                    throw new ArgumentException("Default maximal number of iterations can not be less than 1.");
                _defaultMaxiIterations = value;
            }
        }


        protected static double _defaultToleranceFunctionValue = -1;

        /// <summary>Default tolerance on the error of the value of the minimized function in solution.
        /// <para>Must be a positive value or less than 0 if this tolerance is not specified.</para></summary>
        public static double DefaultToleranceFunctionValue
        {
            get { return _defaultToleranceFunctionValue; }
            set
            {
                //if (value <= 0)
                //    throw new ArgumentException("Default tolerance on function value can not be less or equal to 0.");
                _defaultToleranceFunctionValue = value;
            }
        }


        protected static double _defaultToleranceParemeterNorm = -1;

        /// <summary>Default tolerance on norm of the errors in parameters in the solution.
        /// <para>Must be a positive value or less than 0 if this tolerance is not specified.</para></summary>
        public static double DefaultToleranceParameterNorm
        {
            get { return _defaultToleranceParemeterNorm; }
            set
            {
                //if (value <= 0)
                //    throw new ArgumentException("Default tolerance on parameters can not be less or equal to 0.");
                _defaultToleranceParemeterNorm = value;
            }
        }

        protected static double _defaultToleranceGradientNorm = 1.0e-6;

        /// <summary>Default tolerance on norm of the function gradient in the solution.
        /// <para>Must be a positive value or less than 0 if this tolerance is not specified.</para></summary>
        public static double DefaultToleranceGradientNorm
        {
            get { return _defaultToleranceGradientNorm; }
            set
            {
                //if (value <= 0)
                //    throw new ArgumentException("Default tolerance on gradient norm can not be less or equal to 0.");
                _defaultToleranceGradientNorm = value;
            }
        }

        #endregion Constants


        #region Data.Settings


        protected int _outputLevel = DefaultOutputLevel;

        /// <summary>Level of console output for nonlinear equation solvers.
        /// <para>Output level defines to which extent operation of the solver is logged to the console.</para>
        /// <para>Should be greater or equal to 0, 0 means no output is generated on the application's console.</para>
        /// <para>Default value is specified by the <see cref="EquationSolverBase.DefaultOutputLevel"/> property.</para></summary>
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
        /// <para>Default value is defined by the <see cref="EquationSolverBase.DefaultMaxIterations"/> property.</para></summary>
        public virtual int MaxIterations
        {
            get { return _maxIterations; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Maximal number of iterations can not be less than 1.");
                }
                _maxIterations = value;
            }
        }


        protected double _toleranceFunctionValue = DefaultToleranceFunctionValue;

        /// <summary>Tolerance on absolute error of function value of the function in solution.
        /// <para>Must be a positive value or less than 0 if this tolerance is not specified.</para>
        /// <para>Default value is defined by the <see cref="EquationSolverBase.DefaultToleranceFunctionValue"/> property.</para></summary>
        public virtual double ToleranceFunctionValue
        {
            get { return _toleranceFunctionValue; }
            set
            {
                //if (value <= 0)
                //    throw new ArgumentException("Tolerance on absolute function value can not be less or equal to 0.");
                _toleranceFunctionValue = value;
            }
        }


        protected double _toleranceParameterNorm = DefaultToleranceParameterNorm;

        /// <summary>Tolerance on norm of error in parameters in solution.
        /// <para>Must be a positive value or less than 0 if this tolerance is not specified.</para>
        /// <para>Default value is defined by the <see cref="EquationSolverBase.DefaultToleranceParameterNorm"/> property.</para></summary>
        public virtual double ToleranceParameters
        {
            get { return _toleranceParameterNorm; }
            set
            {
                //if (value <= 0)
                //    throw new ArgumentException("Tolerance on absolute function value can not be less or equal to 0.");
                _toleranceParameterNorm = value;
            }
        }


        protected double _toleranceGradientNorm = DefaultToleranceGradientNorm;

        /// <summary>Tolerance on absolute error of function value of the function in solution.
        /// <para>Must be a positive value or less than 0 if this tolerance is not specified.</para>
        /// <para>Default value is defined by the <see cref="EquationSolverBase.DefaultToleranceGradientNorm"/> property.</para></summary>
        public virtual double ToleranceFunctionGradient
        {
            get { return _toleranceGradientNorm; }
            set
            {
                //if (value <= 0)
                //    throw new ArgumentException("Tolerance on absolute function value can not be less or equal to 0.");
                _toleranceGradientNorm = value;
            }
        }

        #endregion Data.Settings


        #region ProblemData

        protected FunctionWithGradient _function;

        /// <summary>Function to be minimized.</summary>
        public virtual FunctionWithGradient Function
        {
            get { return _function; }
            set { _function = value; }
        }

        protected double[] _initialGuess;

        /// <summary>Initial guess.</summary>
        public virtual double[] InitialGuess
        {
            get { return _initialGuess; }
            set { _initialGuess = value; }
        }

        protected int _numParameters;

        /// <summary>Number of parameters.</summary>
        public virtual int NumParameters
        {
            get
            {
                if (CurrentGuess != null)
                    _numParameters = CurrentGuess.Length;
                else if (InitialGuess != null)
                    _numParameters = InitialGuess.Length;
                else if (SolutionParameters != null)
                    _numParameters = SolutionParameters.Length;
                return _numParameters;
            }
            protected set { _numParameters = value; }
        }

        #endregion ProblemData


        #region OperationData

        protected double[] _currentGuess;

        /// <summary>Current guess.</summary>
        public virtual double[] CurrentGuess
        {
            get { return _currentGuess; }
            set { _currentGuess = value; }
        }

        protected double _currentValue;

        /// <summary>Current function value.</summary>
        public virtual double CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        protected double[] _currentGradient;

        /// <summary>Current function gradient.</summary>
        public virtual double[] CurrentGradient
        {
            get { return _currentGradient; }
            set { _currentGradient = value; }
        }


        protected double[] _solutionParameters;

        /// <summary>Solution of the prioblem - parameter values in minimum.</summary>
        public virtual double[] SolutionParameters
        {
            get { return _solutionParameters; }
            set { _solutionParameters = value; }
        }

        protected double _solutionValue;

        /// <summary>Function value in solution.</summary>
        public virtual double SolutionValue
        {
            get { return _solutionValue; }
            set { _solutionValue = value; }
        }

        protected double[] _solutionGradient;

        /// <summary>Function gradient in solution.</summary>
        public virtual double[] SolutionGradient
        {
            get { return _solutionGradient; }
            set { _solutionGradient = value; }
        }

        #endregion OperationData



        #region Operation

        protected int _iterationCount = 0;

        /// <summary>Current iteration number.</summary>
        public virtual int IterationCount
        {
            get { return _iterationCount; }
            protected set { _iterationCount = value; }
        }


        protected int _evalutaionCount = 0;

        /// <summary>Number of function evaluations performed up to now (usually within the single minimization procedure).</summary>
        public virtual int Evaluationcount
        {
            get { return _evalutaionCount; }
            protected set { _evalutaionCount = value; }
        }


        protected bool _isConvergenceCriteriaMet = false;

        /// <summary>Returna a flag telling whether convergence criteria are met.</summary>
        public virtual bool IsConvergenceCriteriaMet
        {
            get { return _isConvergenceCriteriaMet; }
        }


        /// <summary>Initializes the solution iteration.</summary>
        public abstract void InitializeIteration();

        /// <summary>Optimization of quadratic functions.</summary>
        public abstract void NextIteration();

        /// <summary>Performs complete minimization of the function.</summary>
        public abstract void Minimize();
        


        #endregion Operation


    }



}
