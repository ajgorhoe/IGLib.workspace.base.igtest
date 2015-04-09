// Copyright (c) Igor Grešovnik (2008-present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.IO;

using IG.Lib;


namespace IG.Num
{

    /// <summary>Base class for definition of a system of first order ordinary differential equations (initial problem).</summary>
    /// <remarks><para>Notes on storing the current state:</para>
    /// <para>Solver is only aware of the parameter (independent variable) and function values. These are stored separately
    /// (function values are stored in an array) and define the current state of the problem for the solver (beside the step
    /// number and step length).</para>
    /// <para>Problem definition may include additional state variables. Current state of the problem includes the current
    /// parameter (independent variable) and function values and eventually some additional state variables (such a history
    /// variables) but exclude step number and step length.</para>
    /// <para> Problem definition object stores its state into (or restores from) a packed state array. In this array,
    /// the first element is always the vlue of independent variable (problem parameter), followed by values of searched
    /// functions (solutions of the problem), followed by eventual additional state variables.</para>
    /// <para>When saving the state from the solver, the solver packs the state into an array that includes the 
    /// complete problem state (as vould be stored by the problem definition object) plus the step number and step length.
    /// Sometimes this array does not include all the data, e.g. it may exclude additional values of state variables of
    /// the problem definition object (this is when the state that is saved was not created by the problem definition
    /// object but by solver, which is not aware of the additional state variables). However, the order of the saved
    /// parameters is always the same as that of the problem definition object, only some data may be unavailable.
    /// The solver gets the number of problem state parameters from the problem definition object when it is initialized.</para>
    /// <para>In view of the above, the order of values in the saved state array is as follows</para>
    /// <para>  - value of independent variable (problem parameter, e.g. time).</para>
    /// <para>  - valus of functions that are searched for</para>
    /// <para>  - eventual additional state variables of the problem; these have unspecified values when the saved state is
    /// packed merely by the solver, without assistance of the problem definition object.</para>
    /// <para>  - step number; this has unspecified value when the saved state is packed by the problem definition object,
    /// since it is not aware of the solver.</para>
    /// <para>  - step size used to calculate the saved state; this has unspecified value when the saved state is packed by 
    /// the problem definition object, since it is not aware of the solver.</para>
    /// <para></para></remarks>
    /// $A Igor Mar2009;
    public abstract class DifferentialFirstOrderSolverBase
    {

        #region Construction 

        private DifferentialFirstOrderSolverBase()  {  } // prevent argumentless constructor

        /// <summary>Constructs a solver for the specified system of first order ordinary differential
        /// equations.</summary>
        /// <param name="equations"></param>
        public DifferentialFirstOrderSolverBase(DifferentialFirstOrderSystemBase equations)  
        {
            this.Problem = _problem;
        }

        #endregion construction

        #region OutputAndSave

        // Printing human readable output about calculations:

        /// <summary>Default output level for solvers of first order systems of differential equations.</summary>
        public static int DefaultOutputLevel = 0;

        protected int _outputLevel = DefaultOutputLevel;

        /// <summary>Level of output to console produced during operation (0 means no output).</summary>
        public int OutputLevel
        {
            get { return _outputLevel; }
            set { 
                _outputLevel = value; 
            }
        }

        /// <summary>Default output frequency, sefines how often output is made.</summary>
        public static int DefaultOutputFrequency = 5;

        public int _outputFrequency;

        public int OutputFrequency
        {
            get { return _outputFrequency; }
            set {
                _outputFrequency = value;
            }
        }

        public bool _doOutput = (DefaultOutputLevel > 0 && DefaultOutputFrequency!=0);

        public bool DoOutput
        {
            get { return _doOutput; }
            set { _doOutput = value; }
        }

        // Saving to a file:

        public static int DefaultFileSaveFrequency = 5;

        protected int _fileSaveFrequency = DefaultFileSaveFrequency;

        public int FileSaveFrequency
        {
            get { return _fileSaveFrequency; }
            set
            {
                _fileSaveFrequency = 0;
                DoFileSave = (_fileSaver != null && _fileSaveFrequency != 0);
            }
        }

        protected bool _doFileSave;

        bool DoFileSave
        {
            get { return _doFileSave; }
            set { _doFileSave = value; }
        }

        TextWriter _fileSaver = null;

        public TextWriter FileSaver
        {
            get { return _fileSaver; }
            set 
            {
                _fileSaver = value;
                DoFileSave = (_fileSaver != null && _fileSaveFrequency != 0);
            }
        }

        // Storing to a list:

        public static int DefaultStoringFrequency = 1;

        public int _storingFrequency = DefaultStoringFrequency;

        public int StoringFrequency
        {
            get { return _storingFrequency; }
            protected set
            {
                _storingFrequency = value;
                DoStoreResults = _storingFrequency != 0;
            }
        }

        public bool _doStoreResults = DefaultStoringFrequency != 0;

        public bool DoStoreResults
        {
            get { return _doStoreResults; }
            protected set { _doStoreResults = value; }
        }

        protected List<double[]> _results = new List<double[]>();

        /// <summary>Gets a reference to the list where results are stored.
        /// <para>Results after each step are stored as plain double array where elements are ordered the same as
        /// when the solver state is stored. The array is usually copied from the array addressed by the <see cref="SavedState"/>
        /// property by using the <see cref="StoreSavedState"/> function.</para></summary>
        public List<double[]> Results
        {
            get { return Results; }
        }

        /// <summary>Creates and returns an array containing all stored results.
        /// <para>The returned array does not change when new results are stored by the solver. To include new 
        /// results, either call this method again or use the <see cref="Results"/> property, which gives a reference
        /// to teh actual result list that is updated.</para></summary>
        /// <returns></returns>
        public double[][] GetResultsArray()
        {
            return _results.ToArray();
        }


        #endregion OutputAndSave


        #region ProblemData

        protected int _numEquations;

        /// <summary>Number of equations in the system of equations.</summary>
        public virtual int NumEquations
        {
            get { return _numEquations; }
            protected set
            {
                _numEquations = value;
            }
        }

        protected int _numStateVariables;

        /// <summary>Number of state variables, including independent variable and function values.</summary>
        public int NumStateVariables
        {
            get { return _numStateVariables; }
            set { 
                _numStateVariables = value;
                _numSavedStateVariables = value + 3; // 1 for timestep number + 1 for length of the step that produced the saved state
                                                     // + 1 for estimated error in the time step that produced the saved state
            }
        }

        protected int _numSavedStateVariables = 0;

        /// <summary>Gets the required length of the saved state array.</summary>
        public virtual int NumSavedStateVariables
        {
            get
            {
                return _numSavedStateVariables; 
            }
        }

        protected bool _isHistoryDependent = false;

        /// <summary>Flag indicating whether the problem is history dependent (meaning that it has
        /// additional state variables that are not calculated by the solver).</summary>
        public bool IsHistoryDependent
        {
            get { return _isHistoryDependent; }
            set { _isHistoryDependent = value; }
        }

        protected DifferentialFirstOrderSystemBase _problem;

        /// <summary>Definition of a system of first order ordinary differential equations with initial condition.</summary>
        public DifferentialFirstOrderSystemBase Problem
        {
            get { return _problem; }
            protected set
            {
                _problem = value;
                if (value != null)
                {
                    InitializeSolver();
                }
                else
                {
                    NumEquations = 0;
                    NumStateVariables = 0;
                    IsHistoryDependent = false;
                }
            }
        }

        #endregion ProblemData
        

        #region SolutionProcedureSettings

        protected double _initialParameter;

        /// <summary>Initial value of parameter (independent variable).</summary>
        public double InitialParameter
        {
            get { return _initialParameter; }
            protected set { _initialParameter = value; }
        }

        protected double _finalParameter;

        /// <summary>Final value of parameter (independent variable).</summary>
        public double FinalParameter
        {
            get { return _finalParameter; }
            protected set { _finalParameter = value; }
        }

        protected double _numSteps;

        /// <summary>Number of steps.</summary>
        public double NumSteps
        {
            get { return _numSteps; }
            protected set { _numSteps = value; }
        }

        protected bool _isFixedStep = true;

        /// <summary>Flag indicating whether a fixed step is used (as contrary to adaptive step).</summary>
        public bool IsFixedStep
        {
            get { return _isFixedStep; }
            protected set { _isFixedStep = value; }
        }

        protected double _initialStepLength;

        /// <summary>Initial step length.</summary>
        public double InitialStepLength
        {
            get { return _initialStepLength; }
            set { _initialStepLength = value; }
        }

        protected double _stepLength;

        /// <summary>Step length.</summary>
        public double StepLength
        {
            get { return _stepLength; }
            set { _stepLength = value; }
        }

        #endregion SolutionProcedureSettings

        #region OperationData

        protected double _stepNumber = 0;

        /// <summary>Current step number.</summary>
        public double StepNumber
        {
            get { return _stepNumber; }
            protected set { _stepNumber = value; }
        }

        protected double _parameter;

        /// <summary>Current value of independent parameter.</summary>
        public double Parameter
        { 
            get { return _parameter; } 
            protected set { _parameter = value; }
        }

        protected double _nextParameter;

        /// <summary>Value of independent parameter in the next step.</summary>
        public double NextParameter
        {
            get { return _nextParameter; }
            protected set { _nextParameter = value; }
        }

        protected double[] _functionValues;

        /// <summary>Array of current function values.</summary>
        public double[] FunctionValues
        {
            get { return _functionValues; }
            protected set { _functionValues = value; }
        }

        protected double[] _nextFunctionValues;

        /// <summary>Array of function values in the next step.</summary>
        public double[] NextFunctionValues
        {
            get { return _nextFunctionValues; }
            protected set { _nextFunctionValues = value; }
        }

        protected double[] _state;

        /// <summary>Current state of the problem.
        /// <para>This array contains complete state of the problem, which makes possible to restore
        /// the state witout losing anything. State is generated by the problem object and has
        /// nothing to do with solver. It includes value of independent variable (function parameter), 
        /// all function values and eventual additional (history) variables necessary to define the state.</para></summary>
        public double[] State
        {
            get { return _state; }
            protected set { _state = value; }
        }

        protected double[] _nextState;

        /// <summary>State of the problem for the next step.</summary>
        /// <para>This array contains complete state of the problem, which makes possible to restore
        /// the state witout losing anything. State is generated by the problem object and has
        /// nothing to do with solver. It includes value of independent variable (function parameter), 
        /// all function values and eventual additional (history) variables necessary to define the state.</para></summary>
        public double[] NextState
        {
            get { return _nextFunctionValues; }
            protected set { _nextFunctionValues = value; }
        }

        protected double[] _savedState;

        /// <summary>Array of state and other variables to be saved or restored.
        /// <para>Number of elements is two more than number of elements in state array, the one before the last 
        /// element being the step number (cast to double), and the last being the step length used to produce
        /// the current results.</para>
        /// <para>This array can be used to just save the normal state because the order of the first elements is 
        /// the same.</para></summary>
        public double[] SavedState
        {
            get { return _savedState; }
            protected set { _savedState = value; }
        }

        /// <summary>Stores the saved solver-extended state of the problem (property <see cref="SavedState"/>) to
        /// the list of results (property <see cref="Results"/>).</summary>
        public void StoreSavedState()
        {
            _results.Add(_savedState);
            _savedState = new double[_numSavedStateVariables];
        }

        /// <summary>Saves the current values of the problem parameter (independent variable) and function values 
        /// to the auxiliary state array (property <see cref="SavedState"/>), together with the specified step 
        /// number and step length.
        /// <para>This does not necessarily include the complete state of the problem as defined by the <see cref="Problem"/>
        /// object (since the problem can include additional state variables that the solver is not aware of, such as 
        /// history variables), but the order of elements will be the same (eventually missed parameters are just set to 0).</para></summary>
        /// <param name="state">State to be saved. Stat will normally be obtained from the <see cref="Problem"/> object.</param>
        /// <param name="stepNumber">The step number to be stored with the state (this is not part of the state as 
        /// defined by the <see cref="Problem"/> object).</param>
        /// <param name="stepLength">The step length to be stored with the state (this is not part of the state as 
        /// defined by the <see cref="Problem"/> object).</param>
        public void SaveState(double parameterValue, double[] functionValues,
            int stepNumber, double stepLength)
        {
            _savedState[0] = parameterValue;
            for (int i = 0; i < _numEquations; ++i)
                _savedState[i + 1] = functionValues[i];
            for (int i = _numEquations; i < _numStateVariables; ++i)
                _savedState[i] = 0.0;
            _savedState[_numStateVariables] = stepNumber;
            _savedState[_numStateVariables] = stepLength;
        }

        /// <summary>Saves the current state of the problem (including the current parameter and function values,
        /// step number and step length) to the auxiliary state array (property <see cref="SavedState"/>).</summary>
        /// <param name="state">State to be saved. Stat will normally be obtained from the <see cref="Problem"/> object.</param>
        /// <param name="stepNumber">The step number to be stored with the state (this is not part of the state as 
        /// defined by the <see cref="Problem"/> object).</param>
        /// <param name="stepLength">The step length to be stored with the state (this is not part of the state as 
        /// defined by the <see cref="Problem"/> object).</param>
        public void SaveState(double[] state, int stepNumber, int stepLength)
        {
            for (int i = 0; i < _numStateVariables; ++i)
            {
                _savedState[i] = state[i];
            }
            _savedState[_numStateVariables] = stepNumber;
            _savedState[_numStateVariables] = stepLength;
        }



        /// <summary>Retrieves the value of parameter (independent variable) from the saved state and returns it.</summary>
        /// <param name="savedState">Array containing saved state.</param>
        /// <exception cref="ArgumentException">When the specified array containing the saved state is null or it
        /// has too few elements.</exception>
        public double GetSavedParameter(double[] savedState)
        {
            if (savedState == null)
                throw new ArgumentException("Saved state array is nost specified (null argument)");
            if (savedState.Length < 1)
                throw new ArgumentException("Invalid size of the saved state array (" + savedState.Length 
                    + ", should be at least " + "1" + ").");
            return savedState[0];
        }

        /// <summary>Retrieves the step number from the saved state and returns it.</summary>
        /// <param name="savedState">Array containing saved state.</param>
        /// <exception cref="ArgumentException">When the specified array containing the saved state is null or it
        /// has too few elements.</exception>
        public int GetSavedStepNumber(double[] savedState)
        {
            if (savedState == null)
                throw new ArgumentException("Saved state array is nost specified (null argument)");
            if (savedState.Length < _numStateVariables + 1)
                throw new ArgumentException("Invalid size of the saved state array (" + savedState.Length 
                    + ", should be at least " + (NumStateVariables + 1) + ").");
            return (int) savedState[_numStateVariables];
        }

        /// <summary>Retrieves the step size from the saved state and returns it.</summary>
        /// <param name="savedState">Array containing saved state.</param>
        /// <exception cref="ArgumentException">When the specified array containing the saved state is null or it
        /// has too few elements.</exception>
        public double GetSavedStepSize(double[] savedState)
        {
            if (savedState == null)
                throw new ArgumentException("Saved state array is nost specified (null argument)");
            if (savedState.Length < _numStateVariables + 2)
                throw new ArgumentException("Invalid size of the saved state array (" + savedState.Length 
                    + ", should be at least " + (NumStateVariables + 2) + ").");
            return savedState[_numStateVariables + 1];
        }

        /// <summary>Retrieves the calculated function values from the saved state and copies them to the provided array.</summary>
        /// <param name="savedState">Array containing saved state.</param>
        /// <param name="functionValues">Array where state will be restored. It must be allocated with size
        /// greater or equal to the number of state parameters.</param>
        /// <exception cref="ArgumentException">When the specified array containing the saved state is null or it
        /// has too few elements, or when the provided array to store copied values is null or has too few elements.</exception>
        public void GetSavedFunctionValues(double[] savedState, double[] functionValues)
        {
            if (savedState == null)
                throw new ArgumentException("Saved state array is nost specified (null reference)");
            if (savedState.Length < 1 + _numEquations)  // 1 is for parameter, which is stored at the first place
                throw new ArgumentException("Invalid size of the saved state array (" + savedState.Length
                    + ", should be at least " + (1 + NumEquations) + ").");
            if (functionValues == null)
                throw new Exception("Array to copy saved function values to is not specified (null reference).");
            if (functionValues.Length < _numEquations)
                throw new ArgumentException("Invalid size of the array to copy function values to (" + functionValues.Length
                    + ", should be at least " + NumEquations + ").");
            for (int i = 0; i < _numEquations; ++i)
                functionValues[i] = savedState[i + 1];
        }

        /// <summary>Retrieves the calculated function values from the saved state and copies them to the provided array.</summary>
        /// <param name="savedState">Array containing saved state.</param>
        /// <param name="state">Array where state will be stored. It must be allocated with size
        /// greater or equal to the number of elements of the state parameters (property <see cref="NumStateVariables"/>).</param>
        /// <exception cref="ArgumentException">When the specified array containing the state is null or its
        /// has too few elements, or when the provided array to store copied values is null or has too few elements.</exception>
        public void GetSavedState(double[] savedState, double[] state)
        {
            if (savedState == null)
                throw new ArgumentException("Saved state array is nost specified (null reference)");
            if (savedState.Length < _numStateVariables)
                throw new ArgumentException("Invalid size of the saved state array (" + savedState.Length
                    + ", should be at least " + NumStateVariables + ").");
            if (state == null)
                throw new Exception("Array to copy problem state to is not specified (null reference).");
            if (state.Length < _numStateVariables)
                throw new ArgumentException("Invalid size of the array to copy state to (" + state.Length
                    + ", should be at least " + NumStateVariables + ").");
            for (int i = 0; i < _numStateVariables; ++i)
                state[i] = savedState[i];
        }


        #endregion OperationData

        #region Operation

        /// <summary>Initializes basic problem data according to equations.
        /// <para>Problem must be defined before running this.</para>
        /// <para>Call base class method wen overriding!</para></summary>
        protected virtual void InitializeProblemData()
        {
            if (Problem == null)
                throw new InvalidOperationException("Problem is not set on the solver object."); 
            NumEquations = Problem.NumEquations;
            NumStateVariables = Problem.NumEquations;
            IsHistoryDependent = Problem.IsHistoryDependent;
        }

        /// <summary>Initializes the solver state.
        /// <para>Allocates auxiliary variables and sets the initial conditions from the problem object.</para>
        /// <para>Call base class method when overriding!</para></summary>
        protected virtual void InitializeSolverInternals()
        {
            if (Problem == null)
                throw new InvalidOperationException("Problem is not set on the solver object.");
            Problem.InitializeAndReset();
            Results.Clear();
            StepLength = InitialStepLength;
            // Allocation of auxiliary variables, if necessary:
            bool reallocate = false;
            if (FunctionValues==null)
                reallocate = true;
            else if (FunctionValues.Length!=NumEquations)
                reallocate = true;
            if (reallocate)
                FunctionValues = new double[NumEquations];
            reallocate = false;
            if (NextFunctionValues == null)
                reallocate = true;
            else if (NextFunctionValues.Length != NumEquations)
                reallocate = true;
            if (reallocate)
                NextFunctionValues = new double[NumEquations];
            reallocate = false;
            if (SavedState == null)
                reallocate = true;
            else if (SavedState.Length != NumSavedStateVariables)
                reallocate = true;
            if (reallocate)
                SavedState = new double[NumSavedStateVariables];
            // Set initial conditions:
            Parameter = Problem.GetInitialParameter();
            Problem.GetInitialFunctionValues(FunctionValues);
        }

        /// <summary>Performs all the initializations that are necessary before solving the equations.
        /// This includes transcribing basic problem data from the problem object, transcribing initial 
        /// state from equations object as well as allocating all  auxiliary variables.</summary>
        public void InitializeSolver()
        {
            InitializeProblemData();
            InitializeSolverInternals();  // also initializes and resets the problem.
        }


        /// <summary>Calculates a trial solution after the specified increase in step size.</summary>
        /// <param name="stepLength">Length by which parameter value is incremented.</param>
        public abstract void CalculateTrial(double stepLength);


        public virtual void CalculateWithFixedStep(double finalTime)
        {
            for (int i = 0; i < NumSteps; ++i)
            {
                CalculateTrial(_stepLength);
                // ConfirmNextStep();
            }
        }

        
        //public abstract void CalculateNextStepError(double[] errorStorage);

        //public abstract void CalculateNextStepError();

        //public abstract bool IsNextStepErrorOk();

        //public abstract void ReduceStepSize();

        //public abstract void CalculateNextstepAdaptive();

        #endregion Operation


    }  // class 

}

