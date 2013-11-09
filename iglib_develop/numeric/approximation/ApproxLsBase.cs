using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using IG.Lib;
using IG.Num;


namespace IG.Num
{


    /// <summary>Base class for least squares approximation and various derived methods.
    /// <para>Base for ordinary least squares, weighted least squares (WLS), and moving least squares (MLS).</para></summary>
    /// <remarks>
    /// <para>History:</para>
    /// <para>This class is a part object-oriented re-implementation of the moving least squares (MLS) module of IOptLib.</para>
    /// <para>Information for developers:</para>
    /// <para>The current base class is multifunctional and therefore contains some overhead as regard to internal
    /// variables and properties. It implements all data necessary for approximators from linear ordinary least squares
    /// through linear weighted least squares and finally moving least squares. Subclasses then make appropriate use
    /// of the data and some data is not used in these classes at all. Assumption that justifies this is that such 
    /// approximators will not be used in scenarios where light-weight approximation objects are required. It should
    /// also be noted that handling of internal data structures is quite complex in some kinds of derived 
    /// classes such as those for moving least squares. Therefore, this base class provides tools for rather complex 
    /// scenarios, and some implementation of sub-classes can use only a small portion of these tools.
    /// Derived  classes are intended for use in optimization or in approximation-based modeling of processes, especially 
    /// with several process parameters.</para>
    /// <para>CPU efficiency is given priority over small memory usage and also over good object oriented design to some
    /// extent. Internal variables are defined as protected rather than private, in order to increase performance
    /// where these variables are accessed in loops. Some data is copied to higher performance structures before
    /// execution of code that uses such data in loops with many iterations. The nature of thinhs is such that algorithms
    /// make use of intensive iterative computations. Therefoer preparation activities are not performance-critical
    /// and are be implemented with overhead that aids reusability and other benefits of object oriented programming.
    /// This includes use of properties instead of direct variables, complex data validation procedures, use of
    /// virtual functions, complex branching in order to cover a wide range of usage scenarios (e.g. WLS vs. MLS,
    /// calculation of sensitivities, inclusio of derivative data), etc.</para>
    /// <para></para>
    /// <para>Sources:</para>
    /// <para>Igor Grešovnik: Linear Approximation with Regularization and Moving Least Squares, technical study.</para>
    /// <para>Igor Grešovnik: The Use of Moving Least Squares for a Smooth Approximation of Sampled Data,
    /// Journal of Mechanical Engineering 53 (2007), 582-598,
    /// http://en.sv-jme.eu/scripts/download.php?file=/data/upload/2007/9/SV-JME_53(2007)09_582-598_Gresovnik.pdf </para>
    /// <para>Igor Grešovnik: IOptLib User's Manual (2009), http://www2.arnes.si/~ljc3m2/igor/ioptlib/doc/optlib.pdf </para>
    /// <para>Igor Grešovnik: Specifications for software to determine sensitivities for optimization of the design 
    /// of underground construction as part of IOPT,
    /// http://www2.arnes.si/~ljc3m2/igor/doc/pr/TUNCONSTRUCT_C3M_D1.3.2.1.pdf </para>
    /// <para></para>
    /// <para>History:</para>
    /// <para>This is one of the first higher level classes in the IGLib, created after some basic numerical classes such 
    /// as <see cref="Matrix"/>, <see cref="Vector"/>, etc. Also the first class that has been transfered from the IOptLib
    /// optimization library (ported from ANSI C).</para>
    /// <para>Adopted fully object oriented approach in January 2008 (giving up numerical efficiency as first priority).</para>
    /// <para>Adapted for approximation of vector functions, allowing for approximation of several scalar functions that
    /// can share sampling points, weighting functions, static weights, etc.</para>
    /// <para>In June 2009, the class has been updated to fully comply with scalar and vector function definitions in
    /// the IGLib.</para>
    /// <para>In July & August 2010, extensive testing and some detail improvements were made. Since this point, the class is
    /// considered mature, stable and suitable for any professional use, excpet prehaps in cases where mroe specialized
    /// implementations should be used for efficiency reasons. This is possible in lower dimensions, especially where the 
    /// approximations are used in simulation codes (such as mesh free simulations, geometry or graphics), but is not
    /// expected elsewhere.</para>
    /// </remarks>
    /// $A Igor xx Jan08 Nov08 Jun09 Jul10;
    public class ApproximationLeastSquaresBase : ILockable
    {


        #region Construction

        /// <summary>Constructs a new object of the <see cref="ApproximationLeastSquaresBase"/> that is NOT a master object.</summary>
        public ApproximationLeastSquaresBase()
        {  }


        /// <summary>Creates a new object of the <see cref="ApproximationLeastSquaresBase"/> that is or is not a master object,
        /// and doesn't have any master object.</summary>
        /// <param name="isMaster">Whether this is a master approximation object or not.</param>
        public ApproximationLeastSquaresBase(bool isMaster)
        { this.IsMaster = isMaster; }

        /// <summary>Constructs a new object of the <see cref="ApproximationLeastSquaresBase"/> that is NOT a master object,
        /// and has the specified master object.
        /// <para>The <see cref="ApproximationLeastSquaresBase.IsMaster"/> property is set to false.</para></summary>
        /// <param name="master">The master object of the created object.</param>
        public ApproximationLeastSquaresBase(ApproximationLeastSquaresBase master)
        {
            this.Master = master; // this will also set IsMaster to false.
        }

        #endregion Construction


        #region ThreadLocking

        private readonly object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ThreadLocking


        #region Settings

        protected static int _defaultOutputLevel = 0;

        /// <summary>Gets or sets the default level of output for this class.
        /// <para>When accessed for the first time, the current value of <see cref="Util.OutputLevel"/> is returned.</para>
        /// <para>If set to less than 0 then the first subsequent set access will return the current the current value of <see cref="Util.OutputLevel"/>.</para></summary>
        public static int DefaultOutputLevel
        {
            get
            {
                if (_defaultOutputLevel < 0)
                    _defaultOutputLevel = Util.OutputLevel;
                return _defaultOutputLevel;
            }
            set { _defaultOutputLevel = value; }
        }

        protected int _outputLevel = DefaultOutputLevel;

        /// <summary>Level of output to the console for the current object.</summary>
        public int OutputLevel
        { get { return _outputLevel; } set { _outputLevel = value; } }


        /// <summary>Default value of the testmode flag.
        /// <para>Also used for classes derived from <see cref="ParallelJobServerBase"/> and <see cref="ParallelJobDispatcherBase"/></para></summary>
        public static volatile bool DefaultIsTestMode = false;

        protected bool _isTestMode = DefaultIsTestMode;

        /// <summary>Whether the current job data conntainer is in test mode.
        /// In this mode, delays specified by internal variables are automatically added in job execution.</summary>
        public bool IsTestMode
        {
            get { return _isTestMode; }
            set
            {
                lock (Lock)
                {
                    if (value == true && value != _isTestMode)
                    {
                        //OutputLevel = OutputLevelTestMode;
                    }
                    _isTestMode = value;
                }
            }
        }

        protected StopWatch _timer = null;

        /// <summary>Gets a stopwatch that measures time of operations when in test mode.</summary>
        public StopWatch Timer
        {
            get
            {
                lock (Lock)
                {
                    if (_timer == null)
                        _timer = new StopWatch();
                    return _timer;
                }
            }
        }

        #endregion Settings
        

        #region DelegateToMaster

        protected bool _isMaster = false;

        /// <summary>Whether the current approximation object is a master object that contains
        /// a list of approximation objects that calculate individual components of approximated
        /// vector function.</summary>
        public virtual bool IsMaster
        {
            get { lock (Lock) { return _isMaster; } }
            set
            {
                lock (Lock)
                {
                    if (value == true)
                    {
                        Master = null;
                        _delegateBasis = false;
                        _delegatedStaticWeights = false;
                        _delegatedWeightingFunction = false;
                        _delegateNodalPositions = false;
                        _delegateNodalValues = false;
                        if (_elementApproximators == null)
                            _elementApproximators = new List<ApproximationLeastSquaresBase>();
                        UpdateDelegationFlags();
                    }
                    _isMaster = value;
                }
            }
        }

        /// <summary>Gets the flag indicating whether the current approximator is a slave object,
        /// i.e. it has a master object that may contain (or not) some of its data necessary
        /// to calculate the approximation.</summary>
        public virtual bool IsSlave
        {
            get { lock (Lock) { return (!_isMaster && _master != null); } }
        }

        protected ApproximationLeastSquaresBase _master;

        /// <summary>Eventual master object that contains the current approximation object.</summary>
        public virtual ApproximationLeastSquaresBase Master
        {
            get { lock (Lock) { return _master; } }
            protected internal set
            {
                lock (Lock)
                {
                    if (value != null)
                    {
                        IsMaster = false;
                    } else
                    {
                        _delegateBasis = false;
                        _delegatedStaticWeights = false;
                        _delegatedWeightingFunction = false;
                        _delegateNodalPositions = false;
                        _delegateNodalValues = false;
                        IsMovingLeastSquares = value.IsMovingLeastSquares;
                    }
                    _master = value;
                }
            }
        }

        protected List<ApproximationLeastSquaresBase> _elementApproximators;

        /// <summary>Approximators that approximate elements of a vector function.</summary>
        public virtual List<ApproximationLeastSquaresBase> ElementApproximators
        {
            get { lock (Lock) { return _elementApproximators; } }
            protected set
            {
                lock (Lock)
                {
                    _elementApproximators = value;
                }
            }
        }

        public virtual ApproximationLeastSquaresBase CreateSlave()
        {
            return new ApproximationLeastSquaresBase(this);
        }



        /// <summary>Adds the specified approximator to the current master approximator objects.
        /// <para>Added approximators will approximate individual elements of the vector function.</para>
        /// <para>Exception is thrown if the current object is not a master approximator object 
        /// (i.e., if <see cref="IsMaster"/> == false)</para></summary>
        /// <param name="approximators">Approximators to be added for approximation of elements of a vector function.</param>
        public void AddApproximator(params ApproximationLeastSquaresBase[] approximators)
        {
            if (approximators == null)
                return;
            if (!IsMaster)
                throw new InvalidOperationException("Approximator elements can only be added to master approximator.");
            lock (Lock)
            {
                if (_elementApproximators == null)
                    _elementApproximators = new List<ApproximationLeastSquaresBase>();
                for (int i = 0; i < approximators.Length; ++i)
                {
                    ApproximationLeastSquaresBase approximator = approximators[i];
                    _elementApproximators.Add(approximator);
                }
            }
        }


        protected internal void UpdateDelegationFlags()
        {
        }

        
        protected internal bool _delegateBasis = false;

        /// <summary>Whether basis functions are delegated to the master object containing the current object.</summary>
        public virtual bool DelegateBasis
        {
            get { lock (Lock) { return _delegateBasis; } }
            protected internal set
            {
                lock (Lock)
                {
                    if (value != _delegateBasis)
                    {
                        _delegateBasis = value;
                        if (value == true && _isMaster == null)
                            throw new ArgumentException("Can not delegate basis functions: master object not specified.");
                        UpdateDelegationFlags();
                    }
                }
            }
        }


        protected bool _delegatedStaticWeights = false;

        /// <summary>Whether static weights are delegated to master object containing the current object.</summary>
        public virtual bool DelegateStaticWeights
        {
            get { lock (Lock) { return _delegatedStaticWeights;  } }
            protected internal set
            {
                lock (Lock)
                {
                    if (value != _delegatedStaticWeights)
                    {
                        _delegatedStaticWeights = value;
                        if (value == true && _isMaster == null)
                            throw new ArgumentException("Can not delegate static weights: master object not specified.");
                        UpdateDelegationFlags();
                    }
                }
            }
        }


        protected bool _delegatedWeightingFunction = false;

        /// <summary>Whether static weights are delegated to master object containing the current object.</summary>
        public virtual bool delegateWeightingFunction
        {
            get { lock (Lock) { return _delegatedWeightingFunction; } }
            protected internal set
            {
                lock (Lock)
                {
                    if (value != _delegatedWeightingFunction)
                    {
                        _delegatedWeightingFunction = value;
                        if (value == true && _isMaster == null)
                            throw new ArgumentException("Can not delegate weighting functions: master object not specified.");
                        UpdateDelegationFlags();
                    }
                }
            }
        }

        protected bool _delegateNodalPositions = false;

        /// <summary>Whether positions of nodes (parameters of sampled data) are delegated to master object containing the current object.</summary>
        public virtual bool DelegateNodalPositions
        {
            get { lock (Lock) { return _delegateNodalPositions; } }
            protected internal set 
            {
                lock (Lock)
                {
                    _delegateNodalPositions = value;
                    if (value == true && _isMaster == null)
                        throw new ArgumentException("Can not delegate nodal positions: master object not specified.");
                    UpdateDelegationFlags();
                }
            }
        }


        protected internal bool _delegateNodalValues = false;

        /// <summary>Whether approximated function values (and derivatives) at nodes are delegated to master object containing the current object.</summary>
        public virtual bool DelegateNodalValues
        {
            get { lock (Lock) { return _delegateNodalValues; } }
            protected internal set
            {
                lock (Lock)
                {
                    _delegateNodalValues = value;
                    if (value == true && _isMaster == null)
                        throw new ArgumentException("Can not delegate nodal values: master object not specified.");
                    UpdateDelegationFlags();
                }
            }
        }

        // Delegation of higher level (derived) constructs (dependent on lower level):

        /// <summary>Whether the system matrix is delegated.</summary>
        /// <remarks>This is a dependent flag.</remarks>
        public virtual bool DelegateSystemMatrix
        {
            get
            {
                lock (Lock)
                {
                    return (_delegatedStaticWeights && _delegatedWeightingFunction && 
                        _delegateBasis && _delegateNodalPositions);
                }
            }
        }

        /// <summary>Whether the system matrix is delegated.</summary>
        /// <remarks>This is a dependent flag.</remarks>
        public virtual bool DelegateRightHandSides
        {
            get
            {
                lock (Lock)
                {
                    return (_delegatedStaticWeights && _delegatedWeightingFunction &&
                        _delegateBasis && _delegateNodalPositions && _delegateNodalValues);
                }
            }
        }

        /// <summary>Whether the vector of calculated coefficients is delegated.</summary>
        /// <remarks>This is a dependent flag.</remarks>
        public virtual bool DelegateCoefficients
        {
            get
            {
                //lock (Lock)
                //{
                    return DelegateRightHandSides;
                //}
            }
        }


        #endregion DelegateToMaster


        #region Data

        /// <summary>Number of parameters (independent variables of approximation).</summary>
        int _dimension;

        /// <summary>Number of parameters (independent variables of approximation).
        /// <para>Not thread safe.</para></summary>
        public virtual int Dimension
        {
            get { lock (Lock) { return _dimension; } }
            protected set
            {
                lock (Lock)
                {
                    if (value != _dimension)
                    {
                        _dimension = value;
                        // dimension changed, invalidate dependencies:
                        Basis = null;
                        Parameters = null;
                        Center = null; 
                        IsSystemMatrixAssembled = false;
                        SystemMatrix = null;
                        IsRightHandSideAssembled = false;
                        RighthandSides = null;
                        IsCoefficientsCalculated = false;
                        Coefficients = null;
                    }
                }
            }
        }

        /// <summary>Number of sampling points.
        protected int _numPoints;

        /// <summary>Number of sampling points.
        /// <para>Approximation is calculated on basis of function values in these points.</para></summary>
        public virtual int NumPoints
        {
            get { lock (Lock) { return _numPoints; } }
            protected set {
                lock (Lock)
                {
                    if (value != _numPoints)
                    {
                        _numPoints = value;
                        // Number of sampling points changed, invalidate dependencies:
                        IsSystemMatrixAssembled = false;
                        IsRightHandSideAssembled = false;
                        IsCoefficientsCalculated = false;
                    }
                }
            }
        }

        int _numBasisFunctions;

        public virtual int NumBasisFunctions
        {
            get { lock (Lock) { return _numBasisFunctions; } }
            protected set {
                lock (Lock)
                {
                    if (value!=_numBasisFunctions)
                    {
                        _numBasisFunctions = value;
                        // Number of basis functions changed, invalidate dependencies:
                        IsSystemMatrixAssembled = false;
                        IsRightHandSideAssembled = false;
                        IsCoefficientsCalculated = false;
                        Coefficients = null;
                    }
                }
            }
        }


        /// <summary>Basis functions.</summary>
        LinearBasis _basis;

        /// <summary>Basis functions.</summary>
        public LinearBasis Basis
        {
            get {
                lock (Lock)
                {
                    if (_delegateBasis)
                        return _master.Basis;
                    else
                        return _basis;
                }
            }
            set 
            {
                lock (Lock)
                {
                    if (_delegateBasis)
                        _master.Basis = value;
                    else
                    {
                        if (value != _basis)
                        {
                            IsSystemMatrixAssembled = false;
                            IsRightHandSideAssembled = false;
                            IsCoefficientsCalculated = false;
                            if (value != null)
                            {
                                this.Dimension = value.NumParameters;
                                this.NumBasisFunctions = value.NumValues;
                            }
                            _basis = value;
                        }
                    }
                 }
            }
        }


        protected bool _isMovingLeastSquares = true;

        /// <summary>Whether the current approximator is a moving least squares approximator, i.e. the center
        /// of approximatiion coinceeds with parameters where approximation is evaluated.</summary>
        /// <remarks><para>If a master approximator object exist then this property is always delegated to it.</para>
        /// <para>If the current object is a master object and the property is assigned a value then the same value
        /// is assigned to all slave approximator objects.</para></remarks>
        public bool IsMovingLeastSquares
        {
            get {
                lock (Lock)
                {
                    if (_master != null)
                        return _master.IsMovingLeastSquares;
                    else
                        return _isMovingLeastSquares;
                }
            }
            protected internal set
            {
                lock (Lock)
                {
                    _isMovingLeastSquares = value;
                    if (_isMaster)
                        if (_elementApproximators != null)
                        {
                            foreach (ApproximationLeastSquaresBase approximator in _elementApproximators)
                            {
                                if (approximator != null)
                                    approximator.IsMovingLeastSquares = value;
                            }
                        }
                }
            }
        }
        
        /// <summary>Updates dependencies on parameters where approximaiton is evaluated.</summary>
        /// <remarks>
        /// <para>If <see cref="Master"/> property is defined then parameters are always delegated to the Master,
        /// i.e. change of parameters always affects all slave approximation objects.</para>
        /// <para>If the current object is a master object then the call to this method will also call
        /// the same method on all contained slave objects.</para></remarks>
        public virtual void UpdateParametersChange()
        {

        }

        /// <summary>Updated dependencies on the center of approximation.</summary>
        /// <remarks>
        /// <para>If <see cref="Master"/> property is defined then center of approximation is always delegated to 
        /// the Master, i.e. change of the center always affects all slave approximation objects.</para>
        /// <para>Center of approximation makes sense when weighting functions are defined.</para>
        /// <para>If the current object is a master object then the call to this method will also call
        /// the same method on all contained slave objects.</para></remarks>
        public virtual void UpDateCenterChange()
        {

        }

       /// <summary>Current evaluation point.</summary>
        protected IVector _parameters;

        /// <summary>Current evaluation point.
        /// <para>Must be overridden in the case of MLS where changing x affects coefficients 
        /// and thus system matrix and right-hand side.</para></summary>
        public virtual IVector Parameters
        {
            get { lock (Lock) { 
                return _parameters;
            } }
            set 
            {
                lock (Lock)
                {
                    //if (value != _parameters)  // condition for invalidation is less strict here!
                    //{
                    //}
                    if (value != null)
                    {
                        // For parameter vector, we invalidate dependencies even if vector reference is 
                        // the same as the current one. This is because the caller can change indivitual
                        // elements of the same vector and pass it again. We assume that the same parmeter 
                        // vector wouldn't be passed twice in a row.
                        if (value.Length != _dimension)
                        {
                            Dimension = value.Length;  // this will invalidate dep. related to dimension
                        }
                        else
                        {
                            // Dimension has not changed, manually invalidate dependencies:
                            IsSystemMatrixAssembled = false;
                            IsRightHandSideAssembled = false;
                            IsCoefficientsCalculated = false;
                        }
                    }
                    _parameters = value;
                }
            }
        }

        protected IVector _center;

        /// <summary>Center of weighting functions.</summary>
        /// <remarks><para>In WLS, this affects calculation of weights corresponding to sampling points.
        /// In MLS, this does not have effect because center is set to the current point of evaluation 
        /// every time.</para></remarks>
        public virtual IVector Center
        {
            get { lock (Lock) { return _center; } }
            set
            {
                lock (Lock)
                {
                    if (value != null)
                    {
                        // For parameter vector, we invalidate dependencies even if vector reference is 
                        // the same as the current one. This is because the caller can change indivitual
                        // elements of the same vector and pass it again. We assume that the same parmeter 
                        // vector wouldn't be passed twice in a row.
                        if (value.Length != _dimension)
                        {
                            Dimension = value.Length;  // this will invalidate dep. related to dimension
                        }
                        else
                        {
                            // Dimension has not changed, manually invalidate dependencies:
                            IsSystemMatrixAssembled = false;
                            IsRightHandSideAssembled = false;
                            IsCoefficientsCalculated = false;
                        }
                        _center = value;
                    }
                }
            }
        }

        /// <summary>Vector of static weights.</summary>
        protected double[] _weightsStatic;

        /// <summary>Vector of static weights.</summary>
        /// <remarks><para>If this vector is null then all weights are assumed to be 1.</para>
        /// <para>These are multiplicative weights by which values of weighting functions are multiplied in the case of MLS.</para></remarks>
        public virtual double[] WeightsStatic
        {
            get { lock (Lock) 
            {
                if (_delegatedStaticWeights)
                    return _master.WeightsStatic;
                else
                    return _weightsStatic; 
            } }
            set
            {
                lock (Lock)
                {
                    if (_delegatedStaticWeights)
                        _master.WeightsStatic = value;
                    else
                    {
                        if (value != null)
                        {
                            // For parameter vector, we invalidate dependencies even if vector reference is 
                            // the same as the current one. This is because the caller can change indivitual
                            // elements of the same vector and pass it again. We assume that the same parmeter 
                            // vector wouldn't be passed twice in a row.
                            if (value.Length != _numPoints)
                            {
                                NumPoints = value.Length;  // this will invalidate dep. related to dimension
                            }
                            else
                            {
                                // Dimension has not changed, manually invalidate dependencies:
                                IsSystemMatrixAssembled = false;
                                IsRightHandSideAssembled = false;
                                IsCoefficientsCalculated = false;
                            }
                        }
                        _weightsStatic = value;
                    }
                }
            }
        }

        /// <summary>Weighting function used for calculation of weights.</summary>
        protected IScalarFunctionUntransformed _weightingFunction;

        /// <summary>Weighting function used for calculation of weights.</summary>
        /// <remarks>
        /// <para>If the weighting function is not specified then it is assumed to be a constant function equal to 1 everywhere.</para>
        /// <para>Weights assigned to sampling points are calculated by evaluating this function on the difference between
        /// the sampling point and point of evaluation, and multiplying the result with the corresponding static weight.</para></remarks>
        public virtual IScalarFunctionUntransformed WeightingFunction
        {
            get
            {
                lock (Lock)
                {
                    if (_delegatedWeightingFunction)
                        return _master.WeightingFunction;
                    else
                        return _weightingFunction;
                }
            }
            set {
                lock (Lock)
                {
                    if (_delegatedWeightingFunction)
                        _master.WeightingFunction = value;
                    else
                    {
                        if (value != null || value != _weightingFunction)
                        {
                            // Invalidate dependencies even if a non-null reference is the same as
                            // previous reference. This is because weighting function parameters can 
                            // change even if the reference function itself does not change, and the caller
                            // has the ability to indicate this by setting the weighting function again:
                            IsSystemMatrixAssembled = false;
                            IsRightHandSideAssembled = false;
                            IsCoefficientsCalculated = false;
                        }
                        _weightingFunction = value;
                        if (value != _weightingFunctionRadial) // exclude situation when this property is set through WeightingFunctionRadial
                        {
                            ScalarFunctionRadial rwf = value as ScalarFunctionRadial;
                            WeightingFunctionRadial = rwf;
                            IsWeightingFunctionRadial = (rwf != null);
                        }
                    }
               }
            }
        }

        /// <summary>Whether weighting function is a radial weighting function with affine transformation.</summary>
        protected bool _isWeightingFunctionRadial = false;

        /// <summary>Whether weifhting function is a radial weighting function with affine transformation.</summary>
        public virtual bool IsWeightingFunctionRadial
        {
            get
            {
                lock (Lock)
                {
                    if (_delegatedWeightingFunction)
                        return _master.IsWeightingFunctionRadial;
                    else
                        return _isWeightingFunctionRadial;
                }
            }
            protected set
            {
                lock (Lock)
                {
                    if (_delegatedWeightingFunction)
                        _master.IsWeightingFunctionRadial = value;
                    else
                    {
                        _isWeightingFunctionRadial = value;
                        if (value == false)
                            WeightingFunctionRadial = null;
                    }
                }
            }
        }

        protected ScalarFunctionRadial _weightingFunctionRadial;

        /// <summary>Auxliliary property whose getter evaluates to <see cref="WeightingFunction"/> if it is of
        /// type <see cref="ScalarFunctionRadial"/> (i.e. radial function with affine transformation fo parameters),
        /// or null if weighting function is not of that type. It is also possible to set weighting function through 
        /// this property.</summary>
        public ScalarFunctionRadial WeightingFunctionRadial
        {
            get
            {
                lock (Lock)
                {
                    if (_delegatedWeightingFunction)
                        return _master.WeightingFunctionRadial;
                    else
                        return _weightingFunctionRadial;

                }
            }
            set
            {
                lock (Lock)
                {
                    if (_delegatedWeightingFunction)
                        _master.WeightingFunctionRadial = value;
                    else
                    {
                        _weightingFunctionRadial = value;
                        // It is possible to set weighting function through this value!
                        if (value != null && value != WeightingFunction)
                        {
                            WeightingFunction = value;
                            IsWeightingFunctionRadial = true;
                        }
                    }
                }
            }
        }


        /// <summary>Whether the system matrix has been assembled.</summary>
        protected bool _isSystemMatrixAssembled = false;

        /// <summary>Whether the system matrix has been assembled.</summary>
        public virtual bool IsSystemMatrixAssembled
        {
            get
            {
                lock (Lock)
                {
                    if (DelegateSystemMatrix)
                        return _master.IsSystemMatrixAssembled;
                    else
                        return _isSystemMatrixAssembled;
                }
            }
            protected internal set
            {
                lock (Lock)
                {
                    if (DelegateSystemMatrix)
                        _master.IsSystemMatrixAssembled = value;
                    else
                    {
                        _isSystemMatrixAssembled = value;
                        if (value == false)
                        {
                            IsSolverCreated = false;
                            // IsCoefficientsCalculated = false; // this is already done in IsSolverCreated
                        }
                    }
                }
            }
        }

        /// <summary>System matrix.</summary>
        protected IMatrix _systemMatrix;

        /// <summary>System matrix.</summary>
        /// <remarks>Matrix of the system of equations to be solved for coefficients of the linear combination
        /// of functions that define the approximation.</remarks>
        public IMatrix SystemMatrix
        {
            get { lock (Lock) { return _systemMatrix; } }
            protected internal set { lock (Lock) { _systemMatrix = value; } }
        }

        /// <summary>Whether solver hsa been created.</summary>
        protected bool _isSolverCreated = false;

        /// <summary>Whether a linear solver for calculating coefficients has been created.</summary>
        public bool IsSolverCreated
        {
            get
            {
                lock (Lock)
                {
                    if (DelegateSystemMatrix)
                        return _master.IsSolverCreated;
                    else
                        return _isSolverCreated;
                }
            }
            protected internal set
            {
                lock (Lock)
                {
                    if (DelegateSystemMatrix)
                        _master.IsSolverCreated = value;
                    else
                    {
                        _isSolverCreated = value;
                        if (value == false)
                        {
                            IsCoefficientsCalculated = false;
                        }
                    }
                }
            }
        }

        /// <summary>Solver used for calculating approximation coefficients.</summary>
        protected ILinearSolver _solver;

        /// <summary>Solver used for calculating approximation coefficients.</summary>
        public ILinearSolver Solver
        {
            get { lock (Lock) {
                if (DelegateSystemMatrix)
                    return _master.Solver;
                else
                return _solver; } }
            protected internal set { lock (Lock) {
                if (DelegateSystemMatrix)
                    _master.Solver = value;
                else
                _solver = value; } }
        }

        /// <summary>Whether the right-hand side has been assembled.</summary>
        protected bool _isRightHandSideAssembled = false;

        /// <summary>Whether the right-hand side vector has been assembled.</summary>
        public virtual bool IsRightHandSideAssembled
        {
            get
            {

                lock (Lock)
                {
                    if (DelegateRightHandSides)
                        return _master.IsRightHandSideAssembled;
                    else return _isRightHandSideAssembled;
                }
            }
            protected set
            {
                lock (Lock)
                {
                    if (DelegateRightHandSides)
                        _master.IsRightHandSideAssembled = value;
                    else
                    {
                        _isRightHandSideAssembled = value;
                        if (value == false)
                        {
                            IsCoefficientsCalculated = false;
                        }
                    }
                }
            }
        }

        /// <summary>Vector of right-hand sides.</summary>
        protected IVector _righthandSides;

        /// <summary>Vector of right-hand sides.</summary>
        /// <remarks>Right-hand side of the system of equations to be solved for coefficients of the linear combination
        /// of functions that define the approximation.</remarks>
        public IVector RighthandSides
        {
            get
            {
                lock (Lock)
                {
                    if (DelegateRightHandSides)
                        return _master.RighthandSides;
                    else
                        return _righthandSides;
                }
            }
            protected internal set
            {
                lock (Lock)
                {
                    if (DelegateRightHandSides)
                        _master.RighthandSides = value;
                    else
                        _righthandSides = value; ;
                }
            }
        }

        /// <summary>Whether coefficients of the approximation have been calculated.</summary>
        protected bool _isCoefficientsCalculated = false;

        /// <summary>Whether coefficients of the approximation have been calculated.</summary>
        /// <remarks>Indicates whether coefficients of the approximation have been calculated
        /// (in the case of moving least squares with coefficient dependent on intependent variable,
        /// this refers to the current point of evaluation).</remarks>
        public virtual bool IsCoefficientsCalculated
        {
            get { lock (Lock) {
                if (DelegateCoefficients)
                    return _master.IsCoefficientsCalculated;
                else 
                    return _isCoefficientsCalculated;
            }
            }
            protected set {
                if (DelegateCoefficients)
                    _master.IsCoefficientsCalculated = value;
                else
                    _isCoefficientsCalculated = value;
            }
        }

        /// <summary>Vector of coefficients of linear basis functions at the evaluation point.</summary>
        /// <remarks>Coefficients of the linear combination of base functions that define the approximation.</remarks>
        protected IVector _coefficients;

        /// <summary>Vector of coefficients of linear basis functions at the evaluation point.</summary>
        /// <remarks>Coefficients of the linear combination of base functions that define the approximation.</remarks>
        public IVector Coefficients
        {
            get
            {
                lock (Lock)
                {
                    if (DelegateCoefficients)
                        return _master.Coefficients;
                    else
                        return _coefficients;
                }
            }
            protected set
            {
                lock (Lock)
                {
                    if (DelegateCoefficients)
                        _master.Coefficients = value;
                    else
                        _coefficients = value;
                }
            }
        }


        /// <summary>Returns value of the base function in the specified point.
        /// <para>Usually not thread safe.</para></summary>
        /// <param name="whichFunction">Index of base function whose value is to be returned.</param>
        /// <param name="whichPoint">Index of sampling point in which base function value is calculated.</param>
        public double GetBasisFunctionValue(int whichFunction, int whichPoint)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns value of the overall weight assigned to the specified sampling point.</summary>
        /// <param name="whichPoint">Index of sampling point for which weight is returned.</param>
        public double GetWeight(int whichPoint)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns value of the approximated function in the specified sampling point.</summary>
        /// <param name="whichPoint">Index of sampling point for which function is returned.</param>
        public double GetValue(int whichPoint)
        {
            throw new NotImplementedException();
        }


        // Auxiliary data:

        /// <summary>Auxiliary array for temporarily storing values of base functions in the specific point.</summary>
        double[] AuxBaseFunctionValues;


        #endregion Data


        #region Operation

        //public virtual void AssembleSystem()
        //{
        //    int nPoints = NumPoints;
        //    int nFunctions = NumFunctions;
        //    lock (Lock)
        //    {
        //    }
        //}

        /// <summary>Assembles the system matrix.</summary>
        public virtual void AssembleSystemMatrix()
        {
            lock (Lock)
            {
                if (!IsSystemMatrixAssembled)
                {
                    int numBaseFunctions = NumBasisFunctions;
                    int numPoints = NumPoints;
                    // Prepare matrix of correct dimensions and set elements to 0:
                    Matrix.Resize(ref _systemMatrix, numBaseFunctions, numBaseFunctions);
                    Matrix.SetZero(_systemMatrix);
                    bool resizeFAux = false;
                    if (AuxBaseFunctionValues == null)
                        resizeFAux = true;
                    else if (AuxBaseFunctionValues.Length != numBaseFunctions)
                        resizeFAux = true;
                    if (resizeFAux)
                        AuxBaseFunctionValues = new double[numBaseFunctions];
                    // Assembly the matrix, iterate over points and add corresponding terms:
                    for (int whichPoint = 0; whichPoint < numPoints; ++whichPoint)
                    {
                        // First, calculate base functions in the specific point:
                        for (int wichFunction = 0; wichFunction < numBaseFunctions; ++wichFunction)
                            AuxBaseFunctionValues[wichFunction] = GetBasisFunctionValue(wichFunction, whichPoint);
                        // Then, evaluate contributions and arrange them to appropriate places in the system matrix:
                        double w = GetWeight(whichPoint);
                        for (int i=0; i<numBaseFunctions; ++i)
                            for (int j = 0; j < numBaseFunctions; ++j)
                            {
                                double element = w * w * AuxBaseFunctionValues[i] * AuxBaseFunctionValues[j];
                                _systemMatrix[i, j] += element;
                            }
                    }
                    IsSystemMatrixAssembled = true;
                }
            }
        }

        /// <summary>Assembles the right-hand side vector.</summary>
        public virtual void AssembleRighthandSide()
        {
            lock (Lock)
            {
                if (!IsRightHandSideAssembled)
                {
                    int numBaseFunctions = NumBasisFunctions;
                    int numPoints = NumPoints;
                    // Prepare matrix of correct dimensions and set elements to 0:
                    Vector.Resize(ref _righthandSides, numBaseFunctions);
                    Vector.SetZero(_righthandSides);
                    bool resizeFAux = false;
                    if (AuxBaseFunctionValues == null)
                        resizeFAux = true;
                    else if (AuxBaseFunctionValues.Length != numBaseFunctions)
                        resizeFAux = true;
                    if (resizeFAux)
                        AuxBaseFunctionValues = new double[numBaseFunctions];
                    // Assembly the vector, iterate over points and add corresponding terms:
                    for (int whichPoint = 0; whichPoint < numPoints; ++whichPoint)
                    {
                        // First, calculate base functions in the specific point:
                        for (int wichFunction = 0; wichFunction < numBaseFunctions; ++wichFunction)
                            AuxBaseFunctionValues[wichFunction] = GetBasisFunctionValue(wichFunction, whichPoint);
                        // Then, evaluate contributions and arrange them to appropriate places in the system matrix:
                        double w = GetWeight(whichPoint);
                        for (int i = 0; i < numBaseFunctions; ++i)
                        {
                            _righthandSides[i] += w * w * GetBasisFunctionValue(i, whichPoint) * GetValue(whichPoint); ;
                        }
                    }
                    IsRightHandSideAssembled = true;
                }
            }
        }

        public virtual void CalculateCoefficients()
        {
            lock (Lock)
            {
                AssembleSystemMatrix();
                AssembleRighthandSide();
                if (!IsCoefficientsCalculated)
                {
                    if (!_isRightHandSideAssembled)
                        AssembleRighthandSide();
                    if (!IsSystemMatrixAssembled)
                        AssembleSystemMatrix();
                    if (!IsSolverCreated)
                    {
                        Matrix aux = new Matrix(_systemMatrix);
                        _solver = new CholeskyDecomposition(aux);
                    }
                    _solver.Solve(new Vector(_righthandSides), ref _coefficients);
                    IsCoefficientsCalculated = true;
                }
            }
        }


        public virtual double ApproximationValue(IVector x)
        {
            throw new NotImplementedException();
        }

        public virtual double ApproximationGradient(IVector x, ref IVector grad)
        {
            throw new NotImplementedException();
        }

        #endregion Operation


        #region Tests


        /// <summary>Tests the MLS approximation by sampling and approximating a quadratic function.</summary>
        /// <param name="dimension"></param>
        /// <param name="dataExcessFactor"></param>
        public static void TestMlsQuadratic(IRandomGenerator rand, int dimension, double dataExcessFactor)
        {
            if (rand == null)
                throw new ArgumentException("Random generator is not specified (null argument).");
            if (dimension < 1)
                throw new ArgumentException("Dimension of approximted domain is less than 1.");
            if (dataExcessFactor < 1)
                throw new ArgumentException("Data overhead should be greater or equal to 1.");
            // Create a quadratic function with random coefficients:
            IMatrix Q = new Matrix(dimension, dimension);
            Q.SetRandom(rand);
            Matrix.SymmetricPartPlain(Q, Q);
            IVector b = new Vector(dimension);
            b.SetRandom(rand);
            double c = rand.NextDouble();
            ScalarFunctionQuadratic function = new ScalarFunctionQuadratic(Q, b, c);
            IScalarFunctionUntransformed funcint = function;
            // Sample the function:
            int numCoefficients = ScalarFunctionQuadratic.GetNumConstants(dimension);
            int numPoints = (int)Math.Round(numCoefficients*dataExcessFactor);
            IBoundingBox sampledRegion = new BoundingBox(dimension);
            sampledRegion.UpdateAll(-1, 1);
            SampledDataSet data = SampledDataSet.CreateExample(dimension, 1, numPoints, sampledRegion, 
                new IScalarFunctionUntransformed[] {function}, rand);
            // Calculate approximation:
            ApproximationMls approx = new ApproximationMls();

            throw new NotImplementedException();

        }


        #endregion Tests


    } // LeastSquaresApproximationBase
     


}
