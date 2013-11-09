// Copyright (c) Igor Grešovnik (2008-present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Text;

using IG.Lib;


namespace IG.Num
{


    /// <summary>Defines a system of first order ordinary diferential equations with initial conditions
    /// for dumped harmonic oscillation of a mass on a spring.</summary>
    /// <remarks><para>Dhis system of 2 1st order differential equatiosn describes oscillation of a specified point
    /// mass attached to the end of a fixed linear spring with specified spring coefficient, and possible with linear 
    /// and/or quadratic damping.</para>
    /// <para>Independent parameters are mass, spring coefficients, amplitude, and eventually linear and quadratic
    /// damping coefficients. However, dependent parameters (such as undamped frequency) can also be set, in which 
    /// case independent parameters are adapted appropriately.</para></remarks>
    /// <remarks>See also remarks on saving the state for the <see cref="DifferentialFirstOrderSolverBase"/> and
    /// <see cref="DifferentialFirstOrderSystemBase"/> classes.</remarks>
    /// $A Igor Mar2009;
    public class DifferentialHarmonicOscillation : DifferentialFirstOrderSystemBase
    {


        /// <summary>Construct a system of equations for undamped hasmonic oscillation with mass equal to
        /// 1 and frequency equal to 1.</summary>
        public DifferentialHarmonicOscillation()
        {
            NumEquations = 2;
            NumStateVariables = 3;
            this.Mass = 1;
            this.UndampedFrequency = 1;
        }

        /// <summary>Construct a system of equations for damped harmonic oscillation with specified
        /// mass, frequency, and damping ratio.</summary>
        /// <param name="mass">Mass of the oscillator.</param>
        /// <param name="frequency">Oscillator frequency (spring coefficien is adapted to this).</param>
        /// <param name="dampingRatio">Damping ratio (1 - critical damping, beyond this there are
        /// no oscillations) - defines linear damping coefficient.</param>
        public DifferentialHarmonicOscillation(double mass, double frequency, double dampingRatio):
            this()
        {
            this.Mass = mass;
            this.UndampedFrequency = frequency;
            this.DampingRatio = dampingRatio;
        }


        #region ProblemParameters

        /// <summary>Number of differential equations in the system fo equations that govern the system.</summary>
        public override int NumEquations
        {
            get
            {
                return 2;
            }
            protected set
            {
                if (value != 2)
                    throw new ArgumentException("Number of equations must be set to 2.");
                base.NumEquations = value;
            }
        }

        protected double _m = 1;

        /// <summary>Mass of the weight attached to a spring.</summary>
        public double Mass
        {
            get { return _m; }
            set 
            {
                if (value < 0)
                    throw new ArgumentNullException("Mass can not be less than 0.");
                _m = value;
                UpdateDependencies();
            }
        }

        protected double _k = 1;

        /// <summary>Spring coefficient.</summary>
        public double SpringCoefficient
        {
            get { return _k; }
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Oscillation coefficient can not be less than 0.");
                _k = value;
                UpdateDependencies();
            }
        }

        protected double _kappa = 0;

        /// <summary>Linear damping coefficient.</summary>
        public double LinearDampingCoefficient
        {
            get { return _kappa; }
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Linear damping coefficient can not be less than 0.");
                _kappa = value;
                UpdateDependencies();
            }
        }

        protected double _kSqr = 0;

        /// <summary>Quadratic damping coefficient.</summary>
        public double QuadraticDampingCoefficient
        {
            get { return _kSqr; }
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Quadratic damping coefficient can not be less than 0.");
                _kSqr = value;
                UpdateDependencies();
            }
        }

        // INITIAL CONDITIONS:

        protected double _initialPosition = 1;

        /// <summary>Initial position.</summary>
        public double InitialPosition
        {
            get { return _initialPosition; }
            set { _initialPosition = value; }
        }

        protected double _initialVelocity = 0;

        /// <summary>Initial velocity.</summary>
        public double InitialVelocity
        {
            get { return _initialVelocity; }
            set { _initialVelocity = value; }
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

        protected double _position;

        /// <summary>Current position.</summary>
        public double Position
        {
            get { return _position; }
            set { _position = value; }
        }

        protected double _velocity;

        /// <summary>Current velocity.</summary>
        public double Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        #endregion State

        #region ConvenienceData

        double _nu0;

        /// <summary>Undamped frequency of oscillation.</summary>
        public virtual double UndampedFrequency
        {
            get { return _nu0; }
            set 
            {
                if (value <= 0)
                    throw new ArgumentException("Undamped frequency can not be set to value smaller than or equal to 0.");
                _nu0 = value;
                UndampedAngularFrequency = 2 * Math.PI * value;
            }
        }

        protected double _T0;

        /// <summary>Undamped period of oscillation.</summary>
        public virtual double UndampedPeriod
        {
            get { return _T0; }
            set { 
                if (value <= 0)
                    throw new ArgumentException("Undamped period can not be set to value smaller than or equal to 0.");
                _T0 = value;
                UndampedAngularFrequency =  2 * Math.PI / value;
            }
        }

        protected double _omega0;

        /// <summary>Undamped angular frequency.</summary>
        public virtual double UndampedAngularFrequency
        {
            get { return _omega0; }
            set  
            {
                if (value <= 0)
                    throw new ArgumentException("Undamped angular frequency can not be set to value smaller than or equal to 0.");
                _omega0 = value;
                if (SpringCoefficient <= 0)
                {
                    if (Mass <= 0)
                        Mass = 1;
                    SpringCoefficient = Mass * value * value;
                }
                else if (Mass <= 0)
                {
                    Mass = SpringCoefficient / (value * value);
                } else
                {
                    SpringCoefficient = Mass * value * value;
                }
            }
        }

        protected double _zeta;

        /// <summary>Damping ratio, a normalized linear damping parameter.
        /// Value of 1 means critical damping. Above this value the system approaches the equilibrium 
        /// position without oscillation.</summary>
        public virtual double DampingRatio
        {
            get { return _zeta; }
            set 
            {
                if (value < 0)
                    throw new ArgumentException("Damping ratio can not be smaller than 0.");
                LinearDampingCoefficient = DampingRatio * UndampedAngularFrequency; 
            }
        }


        /// <summary>Updates dependent parameters in in accordance with values of independent parameters.</summary>
        protected virtual void UpdateDependencies()
        {
            if (_k > 0 && _m > 0)
            {
                // Undamped angular frequency:
                _omega0 = Math.Sqrt(SpringCoefficient / Mass);
                // Undamped frequency:
                _nu0 = _omega0 / (2.0 * Math.PI);
                _T0 = 1.0 / _nu0;
                if (_kappa > 0)
                {
                    // Damping ratio:
                    _zeta = _kappa / _omega0;
                }
            }
        }

        #endregion ConvenienceData


        #region Operation

        public double GetAcceleration(double time, double position, double velocity)
        {
            return -2 * _kappa * velocity - _omega0 * _omega0 * position;
        }

        
        /// <summary>Initializes and resets the state of the current problem definition.
        /// <para>All variables are set to initial values.</para>
        /// <para>Any history dependent state variables are reset to the initial state.</para></summary>
        public override void InitializeAndReset()
        {
            Time = InitialTime;
            Position = InitialPosition;
            Velocity = InitialVelocity;
        }


        /// <summary>Calculates derivatives of searched functions from the current time and form current </summary>
        /// <param name="t">Time (independent variable) ad which derivatives are calculated.</param>
        /// <param name="y">Function values (positio and velocity) at the specified time.</param>
        /// <param name="derivative">Array where calculated derivatives of unknown functions (of position 
        /// and velocity) are stored.</param>
        public override void CalculateFunctionDerivatives(double t, double[] y, double[] derivative)
        {
            double x = y[0];
            double v = y[1];
            derivative[0] = v;
            derivative[1] = GetAcceleration(t, x, v);
        }


        /// <summary>Stores initial values of unknown functions (position and velocity) to the specified array.</summary>
        /// <param name="initialValues">Array where initial funtion values are stored.</param>
        public override void GetInitialFunctionValues(double[] initialValues)
        {
            initialValues[0] = _initialPosition;
            initialValues[1] = _initialVelocity;
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
            _position = functionValues[0];
            _velocity = functionValues[1];
        }

        /// <summary>Saves state to the specified array.</summary>
        /// <param name="state">Array where state variables are stored.</param>
        public override void SaveState(double[] state)
        {
            state[0] = _time;
            state[1] = _position;
            state[2] = _velocity;
        }

        /// <summary>Restores state from the specified array.</summary>
        /// <param name="state">Array from which state variables are reatored.</param>
        public override void RestoreState(double[] state)
        {
            _time = state[0];
            _position = state[1];
            _velocity = state[2];
        }

        #endregion Operation


        #region Auxiliary

        /// <summary>Returns string representation of the current system.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Damped harmonic oscillator: ");
            sb.AppendLine(" Mass: " + Mass);
            sb.AppendLine(" Spring coefficient: " + SpringCoefficient);
            sb.AppendLine(" Linear damping coef.: " + LinearDampingCoefficient);
            sb.AppendLine(" Quadratic damping coef.: " + QuadraticDampingCoefficient);
            sb.AppendLine("  Undamped frequency: " + UndampedAngularFrequency);
            sb.AppendLine("  Undamped angular frequency: " + UndampedAngularFrequency);
            sb.AppendLine("  Damping ratio: " + DampingRatio);
            sb.AppendLine(" State: ");
            sb.AppendLine("  Time: " + Time);
            sb.AppendLine("  Position: " + Position);
            sb.AppendLine("  Velocity: " + Velocity);
            sb.AppendLine("  Acceleration: " + GetAcceleration(Time, Position, Velocity));
            return sb.ToString();
        }

        #endregion Auxiliary


    }  // class DifferentialHarmonicOscillation



}