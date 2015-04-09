// Copyright (c) Igor Grešovnik (2008-present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;

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
    public abstract class DifferentialFirstOrderSystemBase
    {

        /// <summary>Construct a system of first order ordinary differential equations, without specifying the
        /// number of equations.</summary>
        public DifferentialFirstOrderSystemBase()
        {  }

        /// <summary>Constructs a new definition of a system of <paramref name="numEquations"/> first order
        /// ordinary differential equations.</summary>
        /// <param name="numEquations">Number of first order differential equations in the system.</param>
        public DifferentialFirstOrderSystemBase(int numEquations)
        {
            this.NumEquations = numEquations;
        }

        #region Data

        protected int _numEquations;

        /// <summary>Number of equations in the system of first order equations.</summary>
        public virtual int NumEquations
        {
            get { return _numEquations; }
            protected set { _numEquations = value; }
        }

        protected int _numStateVariables;

        /// <summary>Number of state variables, including independent variable and function values.</summary>
        public int NumStateVariables
        {
            get { return _numStateVariables; }
            set { _numStateVariables = value; }
        }

        protected bool _isHistoryDependent = false;

        /// <summary>Flag indicating whether the problem is history dependent (meaning that it has
        /// additional state variables that are not calculated by the solver).</summary>
        public bool IsHistoryDependent
        {
            get { return _isHistoryDependent; }
            set { _isHistoryDependent = value; } 
        }

        #endregion Data


        #region Operation

        /// <summary>Initializes and resets the state of the current problem definition.
        /// <para>All variables are set to initial values.</para>
        /// <para>Any history dependent state variables are reset to the initial state.</para></summary>
        public abstract void InitializeAndReset();

        /// <summary>Calculates derivatives fo searched functions for specific values of independent parameter 
        /// and function values.</summary>
        /// <param name="parameter">Value of independent parameters.</param>
        /// <param name="functionValues">Function values.</param>
        /// <param name="resultDerivatives">Array where corresponding derivatives are written.
        /// Must be allocated and of correct size.</param>
        public abstract void CalculateFunctionDerivatives(double parameter, double[] functionValues, double[] resultDerivatives);

        /// <summary>Stores the initial function values (initial conditions) for the problem.</summary>
        /// <param name="initialFunctionValues">Stores the initial function values to the specified array.</param>
        public abstract void GetInitialFunctionValues(double [] initialFunctionValues);

        /// <summary>Returns the initial value of the parameter (independent variable).</summary>
        public abstract double GetInitialParameter();

        /// <summary>Returns the final value of the parameter (independent variable).</summary>
        /// <returns></returns>
        public abstract double GetFinalParameter();

        /// <summary>Sets parameters and function values for the next step.
        /// <para>Additional state variables (e.g. history variables) are updated if applicable.</para></summary>
        /// <param name="parameter">Parameter values in the next step.</param>
        /// <param name="functionValues">Function values in the next step.</param>
        public abstract void SetNextStep(double parameter, double[] functionValues);

        /// <summary>Saves state to the specified array.</summary>
        /// <param name="state">Array where state variables are stored.</param>
        public abstract void SaveState(double[] state);

        /// <summary>Restores state from the specified array.</summary>
        /// <param name="state">Array from which state variables are reatored.</param>
        public abstract void RestoreState(double[] state);

        #endregion Operation

    }  // class DifferentialFirstOrderSystemBase




}




