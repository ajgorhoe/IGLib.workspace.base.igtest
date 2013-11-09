using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>System of first order differential equations fro Lorentz Oscillator.</summary>
    /// <remarks>
    /// <para>See http://en.wikipedia.org/wiki/Lorenz_oscillator </para></remarks>
    /// $A Igor Jan10 Dec11;
    public class DiffLorenzOscillator: DifferentialFirstOrderSystemBase
    {

        /// <summary>Construct a system of differential equations for Lorenz oscillator.</summary>
        /// <param name="sigma">Parameter σ of Lorenz oscillator, "Prandtl number".</param>
        /// <param name="ro">Parameter ρ of Lorenz oscillator, "Rayleigh number".
        /// <para>System exhibits chaotic behavior for ρ = 28, but knotted periodic orbits behaviors 
        /// for other values of ρ. For example, with ρ = 99.96 it becomes a T(3,2) torus knot.</para></param>
        /// <param name="beta">Parameter β of Lorenz oscillator.</param>
        public DiffLorenzOscillator(double sigma, double ro, double beta)
        {
            this.Sigma = sigma;
            this.Ro = ro;
            this.Beta = beta;
        }

        #region ProblemParameters

        /// <summary>Number of first orter differential equations that govern the system.</summary>
        public override int NumEquations
        {
            get
            {
                return 3;
            }
            protected set
            {
                if (value != 3)
                    throw new ArgumentException("Number of equations must be set to 3.");
                base.NumEquations = value;
            }
        }

        protected double _sigma = 10;

        /// <summary>Parameter σ of Lorenz oscillator, the Prandtl number.</summary>
        public double Sigma
        {
            get { return _sigma; }
            set {
                if (value <= 0)
                    throw new ArgumentException("Prandtl number (parameter σ) of Lorenz oscillator may not be less or equal to 0.");
                _sigma = value;  }
        }

        protected double _ro = 28;

        /// <summary>Parameter ρ of Lorenz oscillator, the Rayleigh number.</summary>
        public double Ro
        {
            get { return _ro; }
            protected set {
                if (value <= 0)
                    throw new ArgumentException("Rayleigh number (parameter ρ) of Lorenz oscillator may not be less or equal to 0.");
                _ro = value; }
        }

        protected double _beta = 8.0/3.0;

        /// <summary>Parameter β of Lorenz oscillator.</summary>
        public double Beta
        {
            get { return _beta; }
            set {
                if (value <= 0)
                    throw new ArgumentException("Parameter β of Lorenz oscillator may not be less or equal to 0.");
                _beta = value; }
        }


        // INITIAL CONDITIONS:

        protected double _initialX = 0;

        /// <summary>Initial Y coordinate of the system.</summary>
        public double InitialX
        {
            get { return _initialX; }
            set { _initialX = value; }
        }

        protected double _initialY = 0;

        /// <summary>Initial Y coordinate of the system.</summary>
        public double InitialY
        {
            get { return _initialY; }
            set { _initialY = value; }
        }

        protected double _initialZ = 0;

        /// <summary>Initial Z coordinate of the system.</summary>
        public double InitialZ
        {
            get { return _initialZ; }
            set { _initialZ = value; }
        }

        protected double _initialTime = 0;

        /// <summary>Initial time.</summary>
        public double InitialTime
        {
            get { return _initialTime; }
            set { _initialTime = value; }
        }

        // FINAL TIME:

        protected double _finalTime;

        /// <summary>The final time.</summary>
        public double FinalTime
        {
            get { return _finalTime; }
            set { _finalTime = value; }
        }

        #endregion ProblemParameters



        #region State

        protected double _time;

        /// <summary>Current time.</summary>
        public double Time
        {
            get { return _time; }
            set { _time = value; }
        }

        protected double _x;

        /// <summary>X coordinate of the current position.</summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        protected double _y;

        /// <summary>Y coordinate of the current position.</summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        protected double _z;

        /// <summary>Z coordinate of the current position.</summary>
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }


        #endregion State



        #region Operation

        /// <summary>Initializes and resets the state of the current problem definition.
        /// <para>All variables are set to initial values.</para>
        /// <para>Any history dependent state variables are reset to the initial state.</para></summary>
        public override void InitializeAndReset()
        {
            Time = InitialTime;
            X = InitialX;
            Y = InitialY;
            Y = InitialY;
        }


        /// <summary>Calculates derivatives of searched functions from the current time and form current </summary>
        /// <param name="t">Time (independent variable) ad which derivatives are calculated.</param>
        /// <param name="y">Function values (positio and velocity) at the specified time.</param>
        /// <param name="derivative">Array where calculated derivatives of unknown functions (of position 
        /// and velocity) are stored.</param>
        public override void CalculateFunctionDerivatives(double t, double[] y, double[] derivative)
        {
            derivative[0] = Sigma*(_y-_x);
            derivative[1] = _x*(Ro - _z) - _y;
            derivative[2] = _x*_y-Beta*_z;
        }


        /// <summary>Stores initial values of unknown functions (position and velocity) to the specified array.</summary>
        /// <param name="initialValues">Array where initial funtion values are stored.</param>
        public override void GetInitialFunctionValues(double[] initialValues)
        {
            initialValues[0] = _initialX;
            initialValues[1] = _initialY;
            initialValues[1] = _initialZ;
        }

        /// <summary>Returns the initial value of time (independent variable).</summary>
        public override double GetInitialParameter()
        {
            return _initialTime; ;
        }


        /// <summary>Returns the final value of time (independent variable).</summary>
        public override double GetFinalParameter()
        {
            return _finalTime;
        }

        public override void SetNextStep(double parameter, double[] functionValues)
        {
            _time = parameter;
            _x = functionValues[0];
            _y = functionValues[1];
            _y = functionValues[2];
        }

        /// <summary>Saves state to the specified array.</summary>
        /// <param name="state">Array where state variables are stored.</param>
        public override void SaveState(double[] state)
        {
            state[0] = _time;
            state[1] = _x;
            state[2] = _y;
            state[3] = _y;
        }

        /// <summary>Restores state from the specified array.</summary>
        /// <param name="state">Array from which state variables are reatored.</param>
        public override void RestoreState(double[] state)
        {
            _time = state[0];
            _x = state[1];
            _y = state[2];        
            _z = state[3];
        }

        #endregion Operation


    }
}
