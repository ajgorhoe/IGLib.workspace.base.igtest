//#define VIRTUALPROPERTIES


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IG.Lib;
using IG.Num;

namespace IG.Num
{




    /// <summary>Calculates weighted least squares approximations.</summary>
    /// <remarks>
    /// <para></para>
    /// <para><see cref="SpaceDimension"/> returns the dimension of the space.</para>
    /// <para><see cref="NumBasisFunctions"/> returns the number of basis funcitons.</para>
    /// <para><see cref="NumPoints"/> returns the number of sampling points.</para>
    /// <para></para>
    /// <para>History</para>
    /// <para>WLS approximator was one of the first numerical utility classes in IGLib and was firstt modeled 
    /// upon its equivalents from the IOptLib C library. The class was designed for various extensions, e.g.
    /// incorporation of analytical derivative calculation and moving least squares (MLS). At that time the containing
    /// library was names IOptLib.NET (the "Investtigative Optimization Library") and has later renamed to IGLib (the 
    /// "Investtigative Generic Library").</para>
    /// <para>The moving least squares (MLS) support was first added in January 2008 and a part of testing libraries, which
    /// are used only internally.</para>
    /// <para>The set of derived classes has undergone several modifications. Analytical derivatives were added
    /// in November 2008. In June 2008, WLS and MLS classes for handling several approximators sharing the same set 
    /// of sampling points & weights / weighting functions (and thus the system matrix and its decomposition) were
    /// added. In July 2010, this was synchronized with hierarchy of vector functions.</para>
    /// <para>It has for long been perceived that the implementation of the class hierarchy is overly complex.
    /// Redesign from ground up therefore started in December 2014, following the principles that simple classes
    /// (such as just a plain WLS for a single response) should be implemennted simply. For now, the classes 
    /// that include MLS, vector approximators and analytical differentiation are not be included in basic IGLib.</para>
    /// <para>Implementation of WLS/MLS classes was used to reproduce results from the followig paper (which were 
    /// originally produced with C implementation):</para>
    /// <para>Igor Grešovnik: The Use of Moving Least Squares for a Smooth Approximation of Sampled Data,
    /// Journal of Mechanical Engineering 53 (2007), 582-598,
    /// http://en.sv-jme.eu/scripts/download.php?file=/data/upload/2007/9/SV-JME_53(2007)09_582-598_Gresovnik.pdf </para>
    /// <para>Other references: </para>
    /// <para>Igor Grešovnik: IOptLib User's Manual (2009), http://www2.arnes.si/~ljc3m2/igor/ioptlib/doc/optlib.pdf </para>
    /// <para>Igor Grešovnik: Specifications for software to determine sensitivities for optimization of the design 
    /// of underground construction as part of IOPT,
    /// http://www2.arnes.si/~ljc3m2/igor/doc/pr/TUNCONSTRUCT_C3M_D1.3.2.1.pdf </para>
    /// <para></para>
    /// </remarks>
    /// $A Igor xx Jan08 Nov08 Jun09 Jul10 Dec14;
    public class ApproximatorWlsLinearBase
    {

        /// <summary>Default constructor: do not use!</summary>
        private ApproximatorWlsLinearBase() {
            throw new InvalidOperationException("Constructor without parameters is not legal for WLS approximators.");
        }


        /// <summary>Constructs a WLS calculation object with the specified space dimention.</summary>
        /// <param name="spaceDimension">Dimension of the space on which WLS approximation is calculated.</param>
        /// <param name="copyData">Specifies whether the object will create dynamic copies of input data when provided.
        /// <para>When true, for each input data provided (such as points and weights) internal copies are creaded.</para>	
        /// <para>When false, reference of provided data is taken.</para></param>
        /// <remarks><para>For each input data provided (such as points), internal copies are creaded, and are
        /// deallocated when destructor is called.</para></remarks>
        public ApproximatorWlsLinearBase(int spaceDimension, bool copyData = true,
            VectorFunctionBaseComponentWise basisFunctions =null)
        {
            DecompositionTolerance = 1.0e-6f;

            IsCopyData = true;

            SpaceDimension = spaceDimension;
            NumPoints = 0;
            NumBasisFunctions = 0;

            TrimExcessive = true;
            PrecalculateBasisFunctionValues = true;
            IsBasisFunctionsCalculated = false;

            IsSystemMatrixCalculated = false;
            IsDecompositionCalculated = false;
            IsRighthandSidesCalculated = false;
            IsRighthandSidesCalculated = false;
            IsCoefficientsCalculated = false;

            IsCopyData = copyData;

            Basis = basisFunctions;
            if (basisFunctions == null)
                NumBasisFunctions = 0;
            else
                NumBasisFunctions = basisFunctions.NumValues;

        }

        /// <summary>Constructs a WLS calculation object with the specified basis and with copy mode set to true.</summary>
        /// <param name="spaceDimension">Dimension of the space on which WLS approximation is calculated.</param>
        /// <param name="basisFunctions">Linear basis of type <see cref="BasisFunctionsGeneric"/> that provides basis functions.</param>
        public ApproximatorWlsLinearBase(int spaceDimension, VectorFunctionBaseComponentWise basisFunctions):
            this(spaceDimension, false /* copyData */, basisFunctions)
        {  }



        /// <summary>Destructor.</summary>
        ~ApproximatorWlsLinearBase()  { 
            // TODO: Consider if we actually need a destructtor!

            ClearData();
        }


        /// <summary>Auxiliary string used to assemble error messages.</summary>
        protected string ErrorString
        { get; set; }





#if !VIRTUALPROPERTIES

        /// <summary>Number of basis functions when precalculation of basis functions was performed.</summary>
        protected int PrecalcNumBasisFuctions { get; set; }

        /// <summary>Number of points when precalculation of basis functions was performed.</summary>
        protected int PrecalcNumPoints { get; set; }  

        private int _spaceDimension;

        /// <summary>Dimension of the space where basis function is defined.
        /// <para>When setting, if the dimension differs from the current one, all internal constructs
        /// sependent oo the dimension are reset.</para></summary>
        public int SpaceDimension
        {
            get
            {
                return _spaceDimension;
            }
            protected set
            {
                if (value != _spaceDimension)
                {
                    _spaceDimension = value;
                    ClearPoints(true);
                }
            }
        }
        protected int _numPoints;

        /// <summary>Number of points.</summary>
        public int NumPoints
        {
            get
            {
                return _numPoints;
            }
            protected set { _numPoints = value; }
        }

        protected int _numBasisFunctions;

        /// <summary>Number of basis functions.</summary>
        public int NumBasisFunctions
        {
            get
            {
                return _numBasisFunctions;
            }
            protected set { _numBasisFunctions = value; }
        }


        /// <summary>Auxiliary vector used in calculations. Dimension: number of functions.</summary>
        protected IVector AuxVector { get; set; }

        /// <summary>Auxiliary vector used in calculation. Dimention: space dimension.</summary>
        protected IVector AuxPoint { get; set; }


        private List<IVector> _points = new List<IVector>();

        /// <summary>Sampling points.</summary>
        protected List<IVector> Points { get { return _points; } set { _points = value; } }

        private List<double> _pointValues = new List<double>();

        /// <summary>Values in sampling points to be approximated.</summary>
        protected List<double> PointValues { get { return _pointValues; } set { _pointValues = value; } }  

        private List<double> _pointWeights = new List<double>();

        /// <summary>Weights corresponding to sampling points.
        /// <para>These are always constants, even when MLS is calculated.</para></summary>
        protected List<double> PointWeights { get { return _pointWeights; } set { _pointWeights = value; } }  

        /// <summary>System matrix.</summary>
        protected IMatrix SystemMatrix { get; set; }

        /// <summary>Decomposed System matrix.</summary>
        protected IMatrix Decomposition { get ; set; }

        /// <summary>Vector of right-hand sides.</summary>
        protected IVector RighthandSides { get; set; }

        /// <summary>Vector of calculated approximation coefficients.</summary>
        protected IVector Coefficients { get; set; }

        /// <summary>Pre-calculated values of vector funcitions in sampling points.</summary>
        protected List<IVector> BasisFunctionValues { get; set; } // precalculated values of basis functions

        /// <summary>Basis functions.</summary>
        public virtual VectorFunctionBaseComponentWise  Basis { get; protected set; }

        /// <summary>Gets or sets a flag that indicates whether values of basis functions in sampling points should be
        /// precalculated and stored prior to calculation of system matrix and right-hand side vector, or not.</summary>
        public virtual bool  PrecalculateBasisFunctionValues { get; set; }

        /// <summary>Gets or sets a flag that indicates whether excessive calculated values on the lists should
        /// be removed if the number of elements is greater than the actual number used. Initialized to true by default.</summary>
        public virtual bool  TrimExcessive { get; set; }
            
        /// <summary>Gets or sets a flag indicating whether values of basis functions in sampling points are calculated.</summary>
        public virtual bool IsBasisFunctionsCalculated { get; protected set; }

        /// <summary>Indicates whether the system matrix has been calculated already (and is valid for the 
        ///current state of the approximator).</summary>
        public virtual bool IsSystemMatrixCalculated { get; protected set; }

        /// <summary>Indicates whether decomposition of the system matrix has been calculated already (and is valid for the 
        ///current state of the approximator).</summary>
        public virtual bool IsDecompositionCalculated { get; protected set; }

        /// <summary>Indicates whether the right-hand sides have been calculated already (and are valid for the 
        ///current state of the approximator).</summary>
        public virtual bool IsRighthandSidesCalculated { get; protected set; }

        /// <summary>Indicates whether the coefficients of the approximation (i.e., of linear combination of basis
        /// functions) have been calculated already (and are valid for the  current state of the approximator).</summary>
        public virtual bool IsCoefficientsCalculated { get; protected set; }

        /// <summary>Flag indicating whether the object copies its input data internally.
        /// In this case, the provided data can be modified after it is fed to the object.</summary>
        public bool IsCopyData { get; protected set; }

        /// <summary>Tolerance used for decomposition. If a pivot occurs whose absolute value is below tolerance
        /// then exception shoulld be thrown.</summary>
        public double DecompositionTolerance { get {return _decompositionTolerance;}
            set{
                if (value < 0)
                    throw new ArgumentException("Decomposition tolerance can not be less than 0. Provided: " + value + ".");
                _decompositionTolerance = value;
            } }

#else  // if VIRTUALPROPERTIES


        /// <summary>Number of basis functions when precalculation of basis functions was performed.</summary>
        protected virtual int PrecalcNumBasisFuctions { get; set; }

        /// <summary>Number of points when precalculation of basis functions was performed.</summary>
        protected virtual int PrecalcNumPoints { get; set; }  

        /// <summary>Dimension of the space where basis function is defined.
        /// <para>When setting, if the dimension differs from the current one, all internal constructs
        /// sependent oo the dimension are reset.</para></summary>
        public virtual int Dimension
        {
            get
            {
                return _dim;
            }
            protected set
            {
                if (value != _dim)
                {
                    _dim = value;
                    ClearPoints(true);
                }
            }
        }

        /// <summary>Number of points.</summary>
        public virtual int NumPoints
        {
            get
            {
                return _numPoints;
            }
            protected set { _numPoints = value; }
        }

        /// <summary>Number of basis functions.</summary>
        public virtual int NumBasisFunctions
        {
            get
            {
                return _numBasisFunctions;
            }
            protected set { _numBasisFunctions = value; }
        }

        

        /// <summary>Auxiliary vector used in calculations. Dimension: number of functions.</summary>
        protected virtual IVector AuxVector { get; set; }

        /// <summary>Auxiliary vector used in calculation. Dimention: space dimension.</summary>
        protected virtual IVector AuxPoint { get; set; }

        /// <summary>Sampling points.</summary>
        protected virtual List<IVector> Points { get { return _points; } set { _points = value; } }  

        /// <summary>Values in sampling points to be approximated.</summary>
        protected virtual List<double> PointValues { get { return _pointValues; } set { _pointValues = value; } }  

        /// <summary>Weights corresponding to sampling points.</summary>
        protected virtual List<double> Weights { get { return _pointWeights; } set { _pointWeights = value; } }  

        /// <summary>System matrix.</summary>
        protected virtual IMatrix SystemMatrix { get; set; }

        /// <summary>Decomposed System matrix.</summary>
        protected virtual IMatrix Decomposition { get; set; }

        /// <summary>Vector of right-hand sides.</summary>
        protected virtual IVector RighthandSides { get; set; }

        /// <summary>Vector of calculated approximation coefficients.</summary>
        protected virtual IVector Coefficients { get; set; }

        /// <summary>Pre-calculated values of vector funcitions in sampling points.</summary>
        protected virtual List<IVector> BasisFunctionValues { get; set; } // precalculated values of basis functions

        /// <summary>Basis functions.</summary>
        public virtual VectorFunctionBaseComponentWise Basis { get; protected set; }

        /// <summary>Gets or sets a flag that indicates whether values of basis functions in sampling points should be
        /// precalculated and stored prior to calculation of system matrix and right-hand side vector, or not.</summary>
        public virtual bool PrecalculateBasisFunctionValues { get; set; }
            
        /// <summary>Gets or sets a flag indicating whether values of basis functions in sampling points are calculated.</summary>
        public virtual bool IsBasisFunctionsCalculated { get; protected set; }

        /// <summary>Indicates whether the system matrix has been calculated already (and is valid for the 
        ///current state of the approximator).</summary>
        public virtual bool IsSystemMatrixCalculated { get; protected set; }

        /// <summary>Indicates whether decomposition of the system matrix has been calculated already (and is valid for the 
        ///current state of the approximator).</summary>
        public virtual bool IsDecompositionCalculated { get; protected set; }

        /// <summary>Indicates whether the right-hand sides have been calculated already (and are valid for the 
        ///current state of the approximator).</summary>
        public virtual bool IsRighthandSidesCalculated { get; protected set; }

        /// <summary>Indicates whether the coefficients of the approximation (i.e., of linear combination of basis
        /// functions) have been calculated already (and are valid for the  current state of the approximator).</summary>
        public virtual bool IsCoefficientsCalculated { get; protected set; }

        /// <summary>Flag indicating whether the object copies its input data internally.
        /// In this case, the provided data can be modified after it is fed to the object.</summary>
        public virtual bool IsCopyData { get; protected set; }

        /// <summary>Tolerance used for decomposition. If a pivot occurs whose absolute value is below tolerance
        /// then exception shoulld be thrown.</summary>
        public virtual double DecompositionTolerance
        {
            get { return _decompositionTolerance; }
            set{
                if (value < 0)
                    throw new ArgumentException("Decomposition tolerance can not be less than 0. Provided: " + value + ".");
                _decompositionTolerance = value;
            } }




#endif  // if VIRTUALPROPERTIES




        // Parameters:

        protected double _decompositionTolerance = 1.0e-15;


        /// <summary>Creates and returns a human readable string representation of the current object.
        /// <para>The returned string can be used for immediate output to a file or output device, usually by
        /// calling the c_str() function on it to get the "char *" form.</para></summary>
        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Weighted Least Squares object: ");
            sb.AppendLine("Points: ");

            if (Points==null)
            {
                sb.AppendLine("  Points not specified (null reference).");
            } else
            {
                for (int i=0; i<NumPoints; i++)
                {
                    IVector p = Points[i];
                    sb.AppendLine("  No. " + i + ": " + p.ToString());
                }
            }
            sb.AppendLine("Weights: ");
            if (PointWeights == null)
            {
                sb.AppendLine("  Weights not specified (null reference).");
            } else
            {
                sb.AppendLine(new Vector(PointWeights.ToArray()).ToString());
            }
            sb.AppendLine("Point values:");
            if (PointValues == null)
            {
                sb.AppendLine("  Point values not specified (null reference).");
            } else
            {
                sb.AppendLine(new Vector(PointValues.ToArray()).ToString());
            }
            // sb.NewLine();
            sb.AppendLine("System matrix: ");
            if (SystemMatrix == null || !IsSystemMatrixCalculated)
            {
                if (SystemMatrix == null)
                    sb.AppendLine("  System matrix not defined (null reference).");
                if (!IsSystemMatrixCalculated)
                    sb.AppendLine("  System matrix NOT calculated.");
            } else
            {
                sb.AppendLine("System matrx is calculated.");
                // sb.AppendLine(_systemMatrix.ToString());
            }
            sb.AppendLine("Decomposed system matrix: ");
            if (Decomposition == null || !IsDecompositionCalculated)
            {
                if (Decomposition == null)
                    sb.AppendLine("  Decomposed system matrix not defined (null reference).");
                if (!IsDecompositionCalculated)
                    sb.AppendLine("  Decomposed system matrix NOT calculated.");
            } else
            {
                sb.AppendLine("  Decomposed system matrix calculated.");
                // sb.AppendLine(_decomposition.ToString());
            }
            sb.AppendLine("Right-hand side vector:");
            if (RighthandSides == null || !IsRighthandSidesCalculated)
            {
                if (RighthandSides == null)
                    sb.AppendLine("  Right-hand side vector not defined (null reference).");
                if (!IsRighthandSidesCalculated)
                    sb.AppendLine("  Right-hand side vector NOT calculated.");
            } else
            {
                sb.AppendLine("Right-hand sides are calculated.");
                //sb.AppendLine(_righthandSides.ToString());
            }
            sb.AppendLine("WLS Coefficients: ");
            if (Coefficients == null || !IsCoefficientsCalculated)
            {
                if (Coefficients == null)
                    sb.AppendLine("  WLS coefficients not defined (null reference).");
                if (!IsCoefficientsCalculated)
                    sb.AppendLine("  WLS coefficients NOT calculated.");
            }
            else
            {
                sb.AppendLine(Coefficients.ToString());
            }
            sb.AppendLine();
            return sb.ToString();
        }


        /// <summary>Called by destructor! Clears all the data used by the current object.</summary>

        public void ClearData()
        {
            bool exceptionThrown = false;
            try
            {
                Basis = null;
                if (AuxPoint != null)
                {
                    AuxPoint = null;
                }
                if (SystemMatrix !=null)
                {
                    SystemMatrix = null;
                }
                if (Decomposition != null)
                {
                    Decomposition = null;
                }
            }
            catch(Exception ex)
            {
                exceptionThrown = true;
                ErrorString += "WLS object: could not deallocate point data.";
                if (ex.Message != null)
                {
                    if (ex.Message != null)
                    {
                        ErrorString += Environment.NewLine + "  Original message: " + ex.Message;
                    }
                }
            }
            try
            {
                ClearPoints(true);
            }
            catch(Exception ex)
            {
                exceptionThrown = true;
                ErrorString += "WLS object: could not deallocate point data.";
                if (ex.Message != null)
                {
                    if (ex.Message != null)
                    {
                        ErrorString += Environment.NewLine + "  Original message: " + ex.Message;
                    }
                }
            }

            if (exceptionThrown)
            {
                string messagge = ErrorString;
                ErrorString = null;
                throw new InvalidOperationException(ErrorString);
            }
        }


        /// <summary>Resets sampling points with corresponding (measured) function values and weights.
        /// Dependent data is also reset.</summary>
        /// <param name="deallocate">Whether or not the the corresponding memory is also dellocated. If false then
        /// we try to keep te memory and just adjust the flags telling that this and the dependent constructs are not calculated.</param>
        public virtual void ClearPoints(bool deallocate)
        {
            NumPoints = 0;
            bool exceptionThrown = false;
            try
            {
                try
                {
                    ClearBasisFunctionValues(deallocate);
                    ClearSystemEquations(deallocate);  // will call clear ClearCoefficients()
                    ClearRightHandSides(deallocate);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR in clearing poits: " + ex.Message + Environment.NewLine);
                    throw; }
                if (IsCopyData)
                {
                    int size = Points.Count;
                    for (int i=0; i<size; i++)
                    {
                        if (Points[i]!=null)
                        {
                            // delete[] _points[i];
                            Points[i] = null;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                // Append notifications to throw exception later:
                exceptionThrown = true;
                ErrorString += "ERROR: WLS object: could not deallocate point data.";
                if (ex.Message != null)
                {
                    ErrorString += Environment.NewLine + "  Original message: " + ex.Message;
                }
            }
            try
            {
                Points.Clear();
                PointWeights.Clear();
                PointValues.Clear();
            }
            catch(Exception ex)
            {
                // Append notifications to throw exception later:
                exceptionThrown = true;
                ErrorString += "ERROR: WLS object: could not clear data containers.";
                if (ex.Message != null)
                {
                    ErrorString += Environment.NewLine + "  Original message: " + ex.Message;
                }
            }
            if (exceptionThrown)
            {
                string message = ErrorString;
                ErrorString = null;
                throw new InvalidOperationException(message);
            }

        }

        /// <summary>Resets the array of values of basis functions precalculated in sampling points.</summary>
        /// <param name="deallocate">Whether or not the memory is also reset. If false then
        /// we try to keep te memory allocated and just adjust the flags telling that the dependent constructs are not calculated.</param>
        public virtual void ClearBasisFunctionValues(bool deallocate)
        {
            IsBasisFunctionsCalculated = false;
            if (deallocate && BasisFunctionValues != null)
            {
                BasisFunctionValues.Clear();
            }
        }

        /// <summary>Resets the system matrix of the set of linear equations for WLS approximation, and all dependent data.</summary>
        /// <param name="deallocate">Whether or not the memory is also reset. If false then
        /// we try to keep te memory allocated and just adjust the flags telling that the system matrix and dependent constructs are not calculated.</param>
        public virtual void ClearSystemEquations(bool deallocate)
        {
            IsSystemMatrixCalculated = false;
            ClearDecomposition(deallocate);
            if (deallocate && SystemMatrix != null)
            {
                SystemMatrix = null;
            }
        }

        /// <summary>Resets the right-hand side vector of the set of linear equations for WLS approximation, and all dependent data.</summary>
        /// <param name="deallocate">Whether or not the corresponding memory is also deallocated. If false then
        /// we try to keep te memory allocated and just adjust the flags telling that this and the dependent constructs are not calculated.</param>
        public virtual void ClearRightHandSides(bool deallocate)
        {
            IsRighthandSidesCalculated = false;
            ClearCoefficients(deallocate);
            if (deallocate && RighthandSides!=null)
            {
                RighthandSides = null;
            }
        }

        /// <summary>Resets the decomposition of the system matrix of the set of linear equations for WLS approximation, and all dependent data.</summary>
        /// <param name="deallocate">Whether or not the corresponding memory is also deallocated. If false then
        /// we try to keep te memory allocated and just adjust the flags telling that this and the dependent constructs are not calculated.</param>
        public virtual void ClearDecomposition(bool deallocate)
        {
            IsDecompositionCalculated = false;
            ClearCoefficients(deallocate);
            if (deallocate && Decomposition != null)
            {
                Decomposition = null;
            }
        }

        /// <summary>Resets the coefficients of the WLS approximation, and all dependent data.</summary>
        /// <param name="deallocate">Whether or not the corresponding memory is also deallocated. If false then
        /// we try to keep te memory allocated and just adjust the flags telling that this and the dependent constructs are not calculated.</param>
        public virtual void ClearCoefficients(bool deallocate)
        {
            IsCoefficientsCalculated = false;
            if (deallocate && Coefficients!=null)
            {
                Coefficients = null;
            }
        }




        /// <summary>Changes basis functions of the approximation.
        /// <para>If a basis different from the current basis is supplied then all the dependent data is cleared.</para></summary>
        /// <param name="basis">New set of basis functions.</param>
        public virtual void SetBasis(VectorFunctionBaseComponentWise basis)
        {
            if (basis!=Basis)
            {
                ClearBasisFunctionValues(true);
                ClearRightHandSides(true);
                ClearSystemEquations(true);
                Basis = basis;
                if (basis == null)
                    NumBasisFunctions = 0;
                else
                    NumBasisFunctions = basis.NumValues;
            }
        }

        /// <summary>Returns the set of basis functions used by the current approximation.</summary>
        VectorFunctionBaseComponentWise GetBasis()
        {
            return Basis;
        }

        /// <summary>Adds a new sampling point with the corresponding function value and weight.
        /// <para>Dependent constructs are invalidated (flags are adjusted telling that these constructs are not calculated).</para></summary>
        /// <param name="parameters">Parameters for which function value is specified.</param>
        /// <param name="value">Measured value of the approximated function at the specified parameters.</param>
        /// <param name="weight">The corresponding weight, default is 1.</param>
        public virtual void AddValue( IVector parameters, double value, double weight = 1.0)
        {
            ClearBasisFunctionValues(true);  // eventually precalculated function values must also be deallocated
            ClearSystemEquations(false);
            ClearRightHandSides(false);
            if (IsCopyData)
            {
                IVector point = parameters.GetCopy();
                Points.Add(point);
            } else
            {
                Points.Add(parameters);
            }
            PointValues.Add(value);
            PointWeights.Add(weight);
            ++NumPoints;
        }

        /// <summary>Adds a set of new sampling points with the corresponding function values and weights.
        /// <para>Dependent constructs are invalidated (flags are adjusted telling that these constructs are not calculated).</para></summary>
        /// <param name="numValues">Number of values that are added. Must also define the guaranteed size of array parameters.</param>
        /// <param name="parameterSets">Sets of parameters that define sampling points where function values are specified.</param>
        /// <param name="values">Measured values of the approximated function at the specified parameters (in sampling points).</param>
        /// <param name="weights">The corresponding weight taken into account in the approximation. If null or without elements
        /// then default values are taken for weights (which should be 1).</param>
        public virtual void AddValues2d(IList<IVector> parameterSets, IList<double> values, IList<double> weights = null)
        {
            int numValues = parameterSets.Count;
            if (values == null || values.Count != numValues)
                throw new ArgumentException("Number of falues specified is not the same as the number of points.");
            if (weights != null) if (weights.Count != numValues && weights.Count != 0)
                    throw new ArgumentException("Weighs are specified but their nuber is different than the number of points.");
            if (weights == null)
            {
                for (int i=0; i<numValues; i++)
                {
                    AddValue(parameterSets[i], values[i]);
                }
            } else
            {
                for (int i=0; i<numValues; i++)
                {
                    AddValue(parameterSets[i], values[i], weights[i]);
                }
            }
        }


        /// <summary>Sets (changes) the value of the approximated function in the specified sampling point.
        /// <para>Dependent constructs are invalidated (flags are adjusted telling that these constructs are not calculated).</para></summary>
        /// <param name="whichPoint">Index of the sampling point for which function value is set.</param>
        /// <param name="value">Measured value of the approximated function in the specified sampling point.</param>
        /// <remarks><para>When only changing the values in sampling points (with weights and parameter vectors kept the same), 
        /// only the right-hand side vector of the system of equations for calculation of approximation coefficients is 
        /// invalidated. The system matrix (and thus its decomposition) does not need to be recalculated, and recalculation
        /// of approximation coefficients only requires back substitution.</para></remarks>
        public virtual void SetValue(int whichPoint, double value)
        {
            ClearRightHandSides(false);
            PointValues[whichPoint] = value;
        }

        /// <summary>Sets (changes) ALL values of the approximated function in the sampling points currently specified.
        /// <para>Dependent constructs are invalidated (flags are adjusted telling that these constructs are not calculated).</para></summary>
        /// <param name="numValues">Number of function values to be set. This must correspond to the actual values 
        /// of sampling points currently defined on the approximated object (as a safequard for inconsistent attemps).</param>
        /// <param name="values">Measured values of the approximated function in ALL sampling points.</param>
        /// <remarks><para>When only changing the values in sampling points (with weights and parameter vectors kept the same), 
        /// only the right-hand side vector of the system of equations for calculation of approximation coefficients is 
        /// invalidated. The system matrix (and thus its decomposition) does not need to be recalculated, and recalculation
        /// of approximation coefficients only requires back substitution.</para></remarks>
        public virtual void SetValues(int numValues, IList<double> values)
        {
            ClearRightHandSides(false);
            if (numValues != NumPoints)
                throw new ArgumentException("Number of function values to be set (" 
                + numValues + ") differs from the number of sampling points (" + NumPoints + ").");
            for (int i=0; i < numValues; i++)
                PointValues[i] = values[i];
        }


        /// <summary>Checks consistency of storage that is related to the number of points used in the WLS approximation.
        /// (like if actual numbers of points, values, and weights are consistent with the related internal parameter).
        /// <para>If the state is inconsistent, the method either returns false or throws an exception, dependent on its argument.</para></summary>
        /// <param name="throwException">If true then exception is thrown in the case that the state is inconsistent.</param>
        /// <remarks>Operations should maitain the state consistent all the time. This function tetects if someyhing failed in this mechanism.</remarks>
        /// <returns>true if the state is consistent, false otherwise.</returns<>
        public virtual bool CheckPointsConsistency(bool throwException = false)
        {
            if ((NumPoints == Points.Count) && (NumPoints == PointValues.Count) && (NumPoints == PointWeights.Count))
                return true;
            else if (throwException)
            {
                if (Points.Count != NumPoints)
                    throw new ArgumentException("Actual number of points (" + Points.Count
                    + ") differs from the registered number " + NumPoints + "." );
                if (PointValues.Count!=NumPoints)
                    throw new ArgumentException("Actual number of functoin values (" + PointValues.Count
                    + ") differs from the registered number " + NumPoints + "." );
                if (PointWeights.Count!=NumPoints)
                    throw new ArgumentException("Actual number of weights (" + PointWeights.Count
                    + ") differs from the registered number " + NumPoints + "." );
            }
            return false;
        }



        /// <summary>Returns true if the number of data points used in the WLS approximation is 
        /// greater than or equal to the number of basis functions, otherwise it returns false.
        /// <para>If the number of points is insufficient, the method either returns false or throws an exception, dependent on its argument.</para></summary>
        /// <param name="throwException">If true then exception is thrown in the case that there are not enough points.</param>
        /// <returns>true if the number of points is sufficient with respect to the number of basis functions (greater or equal to), false otherwise.</returns<>
        public virtual bool IsNumPointsSufficient(bool throwException = false)
        {
            if ( Basis!=null && ( NumPoints >= Basis.NumValues ) )
                return true;
            else if (throwException)
            {
                if (Basis == null)
                    throw new ArgumentException("Basis functions are not defined (null reerence).");
                if (NumPoints < Basis.NumValues)
                    throw new ArgumentException("Number of points " + NumPoints + " is smaller than the number of basis functions "
                    + Basis.NumValues + ".");
            }
            return false;
        }

        public virtual double BasisFunction(int whichFunction, int whichPoint)
        {
            return Basis.Value(Points[whichPoint], whichFunction);
        }

        public virtual double BasisFunction(int whichFunction, IVector parameters)
        {
            return Basis.Value(parameters, whichFunction);
        }


        /// <summary>Precalculates values of basis functions in sampling points.
        /// <para>If values are already calculated then nothing happens.</para></summary>
        /// <param name="setFlagToTrue">If true then the flag is also set that specifies that
        /// basis function values are normally precalculated prior to calculation of the system matrix and right-hand sides.</param>
        public virtual void CalculateBasisFunctionValues(bool setFlagToTrue = false)
        {
            try
            {
                if (setFlagToTrue)
                    PrecalculateBasisFunctionValues = true;
                if (!IsBasisFunctionsCalculated)
                {
                    //if (_basisFunctionValues!=null)
                    //{
                    //    // TODO: Could we avoid this?
                    //    _basisFunctionValues = null;
                    //}
                    // Resize if necessary:
                    if (BasisFunctionValues == null)
                        BasisFunctionValues = new List<IVector>();
                    int currentCount = BasisFunctionValues.Count;
                    if (currentCount < NumPoints)
                    {
                        if (BasisFunctionValues.Capacity <NumPoints)
                            BasisFunctionValues.Capacity = NumPoints;
                        int numItemsToAdd = NumPoints - currentCount;
                        for (int i = 0; i < numItemsToAdd ; ++i)
                            BasisFunctionValues.Add(new Vector(NumBasisFunctions));
                    } else if (TrimExcessive && currentCount > NumPoints)
                    {
                        int numItemsToRemove = currentCount - NumPoints;
                        BasisFunctionValues.RemoveRange(NumPoints, numItemsToRemove);
                    }
                    for (int whichPoint=0; whichPoint < NumPoints; whichPoint++)
                    {
                        IVector point = BasisFunctionValues[whichPoint];
                        if (point == null || point.Length != NumBasisFunctions)
                        {
                            point = new Vector(NumBasisFunctions);
                            BasisFunctionValues[whichPoint] = point;
                        }
                        for (int whichFunction = 0; whichFunction < NumBasisFunctions; whichFunction++)
                        {
                            point[whichFunction]  = BasisFunction(whichFunction, whichPoint);
                        }
                    }
                    PrecalcNumBasisFuctions = NumBasisFunctions;
                    PrecalcNumPoints = NumPoints;

                    IsBasisFunctionsCalculated = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(Environment.NewLine + "Basis functions ERROR (calculation): " + ex.Message + Environment.NewLine);
                throw;
            }
        }


        /// <summary>Returns final weight in the specified point.</summary>
        /// <param name="whichPoint">Index of point for which final wight is returned.</param>
        /// <remarks>This method enables use of the same methods for calculation of the system matrix and right-hand sides
        /// regardless of how weights are actually calculated. Inherited classes must only override this function.</remarks>
        public virtual double GetFinalWeight(int whichPoint)
        {
            return PointWeights[whichPoint];
        }

        //protected virtual double GetSquareWeight(int whichPoint)
        //{
        //    double w = PointWeights[whichPoint];
        //    return w * w;
        //}

        /// <summary>Calculates the system matrix of the linear system of equations for calculation of 
        /// coefficients of the WLS approximation.</summary>
        public virtual void CalculateSystemMatrix()
        {
            try
            {
                if (!IsSystemMatrixCalculated)
                {
                    if (PrecalculateBasisFunctionValues)
                    {
                        if (!IsBasisFunctionsCalculated)
                            CalculateBasisFunctionValues();
                    }
                    CheckPointsConsistency(true /* throeException */);
                    IsNumPointsSufficient(true /* throwException */);

                    int dim = NumBasisFunctions;
                    if (SystemMatrix == null)
                        SystemMatrix = new Matrix(dim, dim);

                    bool basFuncPrecalculated = IsBasisFunctionsCalculated;
                    if (basFuncPrecalculated)
                    {

                        if (PrecalcNumBasisFuctions != NumBasisFunctions || PrecalcNumPoints != NumPoints)
                            Console.WriteLine(Environment.NewLine + "ERROR: dimensions of precalculated basis functions storage do not match!"
                            + Environment.NewLine + "  Storage: functions: " + PrecalcNumBasisFuctions + ", points: "
                            + PrecalcNumPoints + ", Now: functions: " + NumBasisFunctions + ", points: " + NumPoints + Environment.NewLine);
                    }
                    // Basis functions are not precalculated, use calculateion 
                    for (int i = 0; i < dim; i++)
                    {
                        for (int j=0; j < dim; j++)
                        {
                            double element = 0;
                            for (int whichPoint=0; whichPoint<NumPoints; whichPoint++)
                            {
                                double w = GetFinalWeight(whichPoint); // PointWeights[whichPoint];
                                if (basFuncPrecalculated)
                                {
                                    element += w * w * 
                                        BasisFunctionValues[whichPoint][i] * // BasisFunction(i, whichPoint) * 
                                        BasisFunctionValues[whichPoint][j];  //  BasisFunction(j, whichPoint);
                                } else
                                {
                                    element += w * w * BasisFunction(i, whichPoint) * BasisFunction(j, whichPoint);
                                }
                            }
                            SystemMatrix[i * dim + j] 
                            =  SystemMatrix[j * dim + i] = element;
                        }
                    }

                    IsSystemMatrixCalculated = true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(Environment.NewLine + "Basis functions (?) ERROR (system matrix): " + ex.Message + Environment.NewLine);
            }
        }



        /// <summary>Calculated the right-hand sides of the linear system of equations for calculation of 
        /// coefficients of the WLS approximation.</summary>
        // __declspec(noinline)
        public virtual void CalculateRighthandSides()
        {
            try
            {
            if (!IsRighthandSidesCalculated)
            {
                if (PrecalculateBasisFunctionValues)
                {
                    if (!IsBasisFunctionsCalculated)
                        CalculateBasisFunctionValues();
                }
                CheckPointsConsistency(true /* throeException */);
                IsNumPointsSufficient(true /* throwException */);
                int dim = NumBasisFunctions;
                if (RighthandSides == null)
                    RighthandSides = new Vector(dim);
                if (RighthandSides.Length != dim)
                    RighthandSides = new Vector(dim);

                bool basFuncPrecalculated = IsBasisFunctionsCalculated;
                if (basFuncPrecalculated)
                {
                    if (PrecalcNumBasisFuctions != NumBasisFunctions || PrecalcNumPoints != NumPoints)
                        Console.WriteLine(Environment.NewLine + "ERROR: dimensions of precalculated basis functions storage do not match!"
                        + Environment.NewLine + "  Storage: functions: " + PrecalcNumBasisFuctions + ", points: "
                        + PrecalcNumPoints + ", Now: functions: " + NumBasisFunctions + ", points: " + NumPoints + Environment.NewLine);
                }
                for (int i = 0; i < dim; i++)
                {
                    double element = 0;
                    for (int whichPoint=0; whichPoint < NumPoints; ++ whichPoint)
                    {
                        double w = GetFinalWeight(whichPoint);   // PointWeights[whichPoint];
                        double basisFunction_i_point;
                        if (basFuncPrecalculated)
                        {
                            basisFunction_i_point = BasisFunctionValues[whichPoint][i];
                        } else
                        {
                            basisFunction_i_point = BasisFunction(i, whichPoint);
                        }
                        double measuredValue = PointValues[whichPoint];
                        double contribution = w * w * basisFunction_i_point * measuredValue;
                        element += contribution;
                    }
                    RighthandSides[i] = element;
                }
                IsRighthandSidesCalculated = true;
            }
            }
            catch(Exception ex)
            {
                Console.WriteLine(Environment.NewLine + "Basis functions (?) ERROR (right-hand sides): " + ex.Message + Environment.NewLine);
            }
        }


        /// <summary>Calculates Decomposition of the system matrix for calculation of WLS approximation coeddicients (if not yet calculated).</summary>
        // __declspec(noinline)
        public virtual void CalculateDecomposition()
        {
            if (!IsDecompositionCalculated)
            {
                CalculateSystemMatrix();
                // _isSystemMatrixCalculated = false;  // because the matrix will be overridden by the in-place operation
                int dim = NumBasisFunctions;
                int numElements = dim * dim;
                if (Decomposition ==null)
                    Decomposition = new Matrix(dim, dim);
                // Transcribe _systemMatrix to _decomposition, then perform the decomposition on _decomposition:
                for (int i=0; i< numElements; i++)
                    Decomposition[i] = SystemMatrix[i];
                IMatrix _decomposition = Decomposition;
                bool successful = MatrixBase.CholeskyDecompose(SystemMatrix, ref _decomposition, _decompositionTolerance);
                Decomposition = _decomposition;
                if (!successful)
                    throw new ArgumentException("Singular or close to singular system matrix. Tolerance: " + _decompositionTolerance + ".");
                IsDecompositionCalculated = true;
            }
        }


        /// <summary>Calculates coefficients of the WLS approximation.</summary>
        // __declspec(noinline)
        public virtual void CalculateCoefficients()
        {
            if (!IsCoefficientsCalculated)
            {
                CalculateDecomposition();
                CalculateRighthandSides();
                IVector _coefficients = Coefficients;
                MatrixBase.CholeskySolve(Decomposition, RighthandSides, ref _coefficients);
                Coefficients = _coefficients;

                IsCoefficientsCalculated = true;
            }
        }



        // TODO: Consider how GetCoefficients() and other functions should be best implemented!


        /// <summary>Calculates (if not yet calculated) and returns coefficients of the weighted least squares approximation.</para>
        /// <para>Warning: coefficients can be deallocated if the state of the approximation changes!</para></summary>
        public virtual IVector GetCoefficients()
        {
            if (!IsCoefficientsCalculated)
                CalculateCoefficients();
            return Coefficients;
        }


        /// <summary>Calculates (if not yet calculated) and returns system matrix for calculation of the weighted least squares approximation coefficients.</para>
        /// <para>Warning: coefficients can be deallocated if the state of the approximation changes!</para>
        /// <para>Warning: Calculation of coefficients or matrix decomposition overwrites this matrix, therefore it must be re-calculated after after those are calculated!</para></summary>
        public virtual IMatrix GetSystemMatrix()
        {
            if (!IsSystemMatrixCalculated)
                CalculateSystemMatrix();
            return SystemMatrix;
        }

        /// <summary>Calculates (if not yet calculated) and returns right-hand side vector for calculation of the weighted least squares approximation coefficients.</para>
        /// <para>Warning: coefficients can be deallocated if the state of the approximation changes!</para></summary>
        public virtual IVector GetRightHandSides()
        {
            if (!IsRighthandSidesCalculated)
                CalculateRighthandSides();
            return RighthandSides;
        }



        /// <summary>Returns value of the current weighting least squares approximation at the specified parameters.
        /// <para>Any intermediate results necessary are calculated automatically.</para>
        /// <para>Exception is thrown if something can not be calculated (e.g. there is insufficient data or basis functions are not defined).</para></summary>
        /// <param name="parameters">Parameters where value is calculated.</param>
        public virtual double Value(IVector parameters)
        {
            if (Basis == null)
                throw new ArgumentException("Can not calculate the value of WLS, basis functions are not defined.");
            if (!IsCoefficientsCalculated)
                CalculateCoefficients();
            return Basis.LinearCombinationValue(parameters, Coefficients);
        }


        /// <summary>Returns derivative with respect to the specified variable of the current weighting least squares approximation at the specified parameters.
        /// <para>Any intermediate results necessary are calculated automatically.</para>
        /// <para>Exception is thrown if something can not be calculated (e.g. there is insufficient data or basis functions are not defined).</para></summary>
        /// <param name="whichVariable">Variable with respect to which derivative is calculated.</param>
        /// <param name="parameters">Parameters where derivative is calculated.</param>
        public virtual double Derivative(int whichVariable, IVector parameters)
        {
            if (Basis == null)
                throw new InvalidOperationException("Can not calculate the derivative of WLS, basis functions are not defined.");
            if (!Basis.DerivativeDefined)
                throw new InvalidOperationException("Can not calculate the derivative of WLS, basis function derivatives are not defined.");
            if (!IsCoefficientsCalculated)
                CalculateCoefficients();
            return Basis.LinearCombinationDerivative(parameters, Coefficients, whichVariable);
        }


        #region Tests

        // STATIC TEST FUNCTIONS:

        // WEIGHTING LEAST SQUARES - SINGLE APPROXIMATION of sampled linear combination of basis functions:

        /// <summary>Tests weighting Least squares approximation with liner basis functions. See <see cref="TestWeightedLeastSquaresPolynomialBasis"/>.</summary>
        public static double TestWeightedLeastSquaresLinearBasis(int dim, int outputLevel = 2, 
            double excessFactor = 1.5, int randomSeed = 222, double perturbationFactor = 0.0)
        {
            return TestWeightedLeastSquaresPolynomialBasis(dim, false /* quadratic */ , outputLevel, 
                excessFactor, randomSeed, perturbationFactor);
        }

        /// <summary>Tests weighting Least squares approximation with quadratic basis functions. See <see cref="TestWeightedLeastSquaresPolynomialBasis"/>.</summary>
        public static double TestWeightedLeastSquaresQuadraticBasis(int dim, int outputLevel = 2, 
            double excessFactor = 1.5, int randomSeed = 222, double perturbationFactor = 0.0)
        {
            return TestWeightedLeastSquaresPolynomialBasis(dim, true /* quadratic */, outputLevel, 
                excessFactor, randomSeed, perturbationFactor);
        }


        /// <summary>Tests weighting least squares approximation by sampling a fixed linear combination of lienar or 
        /// quadratic polynomial basis functions on a random set of sampling points, calculating approximations in samplng 
        /// points and comparing them to the original function.
        /// <para>All components of sampling points are on the interval [0, 1], thus values of all basis functions (which are
        /// 1st or 2nd degree monomials) also lie in this interval.</para></summary>
        /// <param name="dim">Dimension of space where baiss functions are defined.</param>
        /// <param name="quadratic">Whether quadratic polynomial approximation is tested. If false then linear polynomial basis is taken.</param>
        /// <param name="outputLevel">Level of output printed to standard output. If 0 or less then no outputs are produced, and 
        /// this function can be used in unit testing.</param>
        /// <param name="excessFactor">Factor by which there are more points than basis functions (and thus unknown coefficients to be calculated).
        /// <para>Must be greater than 1.</para></param>
        /// <param name="randomSeed">Seed for initialization of the random generator used to generate sampling points. By supplying 
        /// the seed, results are repeatable, and the function is better suited for use in unit testing. If less or equal to 0 then 
        /// random generator used for generation of sampling points is not seeded.</param>
        /// <param name="perturbationFactor">If different than 0 then pure cubic terms are added to the function used for sampling,
        /// which means that sampled values can not be any more exactly approximated by the used linear or quadratic basis.</param>
        public static double TestWeightedLeastSquaresPolynomialBasis(int dim, bool quadratic = true, int outputLevel = 2, 
            double excessFactor = 1.5, int randomSeed = 222, double perturbationFactor = 0.0)
        {
            IRandomGenerator rand = null;
            if (randomSeed > 0)
                rand = new RandomGenerator(randomSeed);
            else 
                rand = new RandomGenerator();
            if (excessFactor < 1.0)
                throw new ArgumentException("Excess factor shoulld be greater or equal to 1.");
            double totalError = 0.0;  // total absolute error
            if (outputLevel>=1)
            {
                Console.WriteLine(Environment.NewLine + "============" + 
                    Environment.NewLine + "Testing weighted least squares... " + Environment.NewLine) ;
                if (quadratic)
                    Console.WriteLine(Environment.NewLine + "Basis: Quadratic polynomial " + dim + "-D. " + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "Basis: Linear polynomial " + dim + "-D. " + Environment.NewLine);
            }
            try
            {
                StopWatch1 t = new StopWatch1();
                int numfunc = 0;
                VectorFunctionBaseComponentWise basis = null;
                if (quadratic)
                    basis = new QuadraticBasis(dim);
                else 
                    basis = new LinearBasis(dim);
                numfunc = basis.NumValues;
                int numPoints = (int)Math.Ceiling((double)numfunc * excessFactor);

                IVector 
                    coefOriginal = new Vector(numfunc),
                    coefCalculated = new Vector(numfunc),
                    coefPerturbed = new Vector(dim),   // coefficients of a sum of 3rd powers used to calculate perturbation
                    pointValues = new Vector(numPoints),
                    approximatedValues = new Vector(numPoints), 
                    errors = new Vector(numPoints),
                    vec = new Vector(dim);
                List<IVector> points = new List<IVector>();
                ApproximatorWlsLinearBase wls = new ApproximatorWlsLinearBase(dim, true, basis);

                if (outputLevel >= 1)
                {
                    Console.WriteLine("Basis functions: " + Environment.NewLine + basis.ToString() + Environment.NewLine);
                }

                // Coefficients of the linear combination:
                for (int iAux=0; iAux<numfunc; ++iAux)
                {
                    coefOriginal[iAux] = rand.NextDouble();
                }
                if (perturbationFactor != 0.0)
                {
                    for (int iAux = 0; iAux < dim; ++iAux)
                        coefPerturbed[iAux] = rand.NextDouble() * perturbationFactor;
                }
                // Sample values on the assumed linear combination:
                t.Start();
                for (int i=0; i<numPoints; ++i)
                {
                    if (i >= points.Count)
                    {
                        points.Add(new Vector(dim));
                    }
                    for (int j=0; j<dim; ++j)
                    {
                        points[i][j] = rand.NextDouble();
                    }
                    // Evaluete linear combination of basis functions at the generated point:
                    IVector coefficients = null;
                    VectorBase.Copy(coefOriginal, ref coefficients);
                    IVector point = points[i];
                    double val = basis.LinearCombinationValue(point, coefficients);
                    if (perturbationFactor != 0.0)
                    {
                        for (int iAux = 0; iAux < dim; ++iAux)
                            val += coefPerturbed[iAux] * Math.Pow(point[iAux], 3);
                    }
                    pointValues[i] = val;

                    if (outputLevel >= 3)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append( "Point No. " + i + ": ");
                        sb.AppendLine(points[i].ToString());
                        sb.AppendLine("  Sampled value: " + val);
                        Console.WriteLine( sb );
                    }

                }
                t.Stop();
                if (outputLevel >= 1)
                {
                    Console.WriteLine(Environment.NewLine + "Samplng performed in " + t.Time + "s (CPU: " + t.CpuTime + " s)."
                        + Environment.NewLine);
                }
                if (outputLevel >= 2)
                {
                    StringBuilder sb1 = new StringBuilder();
                    sb1.AppendLine("After addition - points:") ;
                    for (int whichPoint = 0; whichPoint < points.Count; ++whichPoint)
                        sb1.AppendLine("  " + whichPoint + ": " + points[whichPoint].ToString());

                    sb1.AppendLine(Environment.NewLine + "Point values: ");
                    for (int whichPoint = 0; whichPoint < points.Count; ++whichPoint)
                        sb1.AppendLine("  " + whichPoint + ": " + pointValues[whichPoint]);
                    sb1.AppendLine(Environment.NewLine + "Coefficients: " + Environment.NewLine
                        + "  " + coefOriginal.ToString());
                    Console.WriteLine(Environment.NewLine + sb1.ToString() + Environment.NewLine);
                }

                if (true)
                {

                    wls.AddValues2d(points, pointValues.ToArray() /* weights = null, defaultWeight = 1 */);
                    if (outputLevel >=2)
                    {
                        Console.WriteLine(Environment.NewLine + "WLS object after adding points: " + wls.ToString() + Environment.NewLine);
                    }
                    t.Start();
                    wls.CalculateCoefficients();  // not necessary, coef. automatically calculated
                    t.Stop();
                    if (outputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "Approx. coefficients calculated in " + t.Time + "s (CPU: " + t.CpuTime + " s)."
                            + Environment.NewLine);
                    }
                    if (outputLevel >= 2)
                    {
                        Console.WriteLine(Environment.NewLine + "WLS object after calculating coefficients: " + wls.ToString() + Environment.NewLine);
                    }

                    // Calculate approximation errors in sampling points and update the total absolute error:
                    totalError = 0;
                    t.Start();
                    for (int i=0; i<numPoints; ++i)
                    {
                        double val = approximatedValues[i] = wls.Value(points[i]);
                        double error = errors[i] = val - pointValues[i];
                        totalError += Math.Abs(error);
                    }
                    t.Stop();
                    if (outputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "Approximation error calculated in " + t.Time + "s (CPU: " + t.CpuTime + " s)."
                            + Environment.NewLine);
                        Console.WriteLine(Environment.NewLine + "Total error: " + totalError + Environment.NewLine +
                            "  Divided by number of points: " + (totalError / (double) numPoints ) + Environment.NewLine);
                    }
                }
                if (outputLevel >= 1)
                {
                    Console.WriteLine(Environment.NewLine + "========" + Environment.NewLine
                         + "End of the test." + Environment.NewLine);
                }
                return totalError / (double) numPoints;
            }
            catch(Exception ex)
            {
                if (outputLevel >=1)
                {
                    Console.WriteLine(Environment.NewLine + "EXCEPTION thrown: " + Environment.NewLine + "Message: " + Environment.NewLine
                        + ex.Message + Environment.NewLine);
                }
            }

            return 1.0e10;
        }  // TestWeightedLeastSquaresPolynomialBasis(..)




        // WEIGHTING LEAST SQUARES - MULTIPLE APPROXIMATIONS of sampled linear combination of basis functions 
        // (possibly with the same poinst and different values):

        /// <summary>Tests weighting Least squares approximation with linear basis functions. See <see cref="TestWeightedLeastSquaresMultiplePolynomialBasis"/>.</summary>
        public static double TestWeightedLeastSquaresMultipleLinearBasis(int dim, int outputLevel = 2, int numValueChanges = 100, 
            bool changePoints = true, bool ClearSystemEquations = true, 
            double excessFactor = 1.5, int randomSeed = 555, double perturbationFactor = 0.0)
        {
            return TestWeightedLeastSquaresMultiplePolynomialBasis(dim, false /*  quadratic */ , outputLevel,
                numValueChanges, changePoints, ClearSystemEquations, excessFactor, randomSeed, perturbationFactor);
        }

        /// <summary>Tests weighting Least squares approximation with quadratic basis functions. See <see cref="TestWeightedLeastSquaresMultiplePolynomialBasis"/>.</summary>
        public static double TestWeightedLeastSquaresMultipleQuadraticBasis(int dim, int outputLevel = 2, int numValueChanges = 100,
            bool changePoints = true, bool ClearSystemEquations = true, 
            double excessFactor = 1.5, int randomSeed = 555, double perturbationFactor = 0.0)
        {
            return TestWeightedLeastSquaresMultiplePolynomialBasis(dim, true /*  quadratic */ , outputLevel,
                numValueChanges, changePoints, ClearSystemEquations, excessFactor, randomSeed, perturbationFactor);
        }


        /// <summary>Tests weighting least squares approximation by sampling fixed linear combinations of basis functions
        /// on multiple sets of points, calculating approximations in samplng points and comparing them to the original function.
        /// <para></para></summary>
        /// <param name="dim">Dimension of space where baiss functions are defined.</param>
        /// <param name="quadratic">Whether quadratic polynomial approximatio is tested. If false then linear approximation is taken.</param>
        /// <param name="outputLevel">Level of output printed to standard output. If 0 or less then no outputs are produced, and function can be used in unit testing.</param>
        /// <param name="numValueChanges">Number of times when the original function (formed as a fixed linear combination of basis functions) change.
        /// <para>Function values are re/calculated every time this happens, which implies that right-hand sides of the system of equations for coefficients also change.</para></param>
        /// <param name="changePoints">If true then for each resampling the positions of sampling points also change.
        /// <para>If false then only the right-hand sides must be recalculated after each change of the approximated function.
        /// If true then the system matrix must be re-calculated, too.</para></param>
        /// <param name="ClearSystemEquations">If true then the system matrix is invalidated (and must be re-calculated when 
        /// approximation coefficitnes or values are required) after each change of the approximated function, even if sampling points do not change.</param>
        /// <param name="excessFactor">Factor by which there are more points than basis functions (and thus unknown coefficients to be calculated).
        /// <para></para>Must be greater than 1.</param>
        /// <param name="randomSeed">Seed for initialization of the random generator used to generate sampling points. By supplying 
        /// the seed, results are repeatable, and the function is better suited for use in unit testing.</param>
        /// <param name="perturbationFactor">If different than 0 then pure cubic terms are added to the function used for sampling,
        /// which means that sampled values can not be any more exactly approximated by the used linear or quadratic basis.</param>
        public static double TestWeightedLeastSquaresMultiplePolynomialBasis(int dim, bool quadratic = true, int outputLevel = 2, 
            int numValueChanges = 100, bool changePoints =  true, bool ClearSystemEquations = true, 
            double excessFactor = 1.5,int randomSeed = 222, double perturbationFactor = 0.0)
        {
           IRandomGenerator rand = null;
            if (randomSeed > 0)
                rand = new RandomGenerator(randomSeed);
            else 
                rand = new RandomGenerator();
            if (excessFactor < 1.0)
                throw new ArgumentException("Excess factor shoulld be greater or equal to 1.");
            double totalError = 0.0;  // total absolute error
            if (outputLevel>=1)
            {
                Console.WriteLine(Environment.NewLine + "============" + 
                    Environment.NewLine + "Testing weighted least squares (" + numValueChanges + " approximations)... " + Environment.NewLine) ;
                if (quadratic)
                    Console.WriteLine(Environment.NewLine + "Basis: Quadratic polynomial " + dim + "-D. " + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "Basis: Linear polynomial " + dim + "-D. " + Environment.NewLine);
            }
            try
            {
                StopWatch1 t = new StopWatch1();
                int numfunc = 0;
                VectorFunctionBaseComponentWise basis = null;
                if (quadratic)
                    basis = new QuadraticBasis(dim);
                else 
                    basis = new LinearBasis(dim);
                numfunc = basis.NumValues;
                int numPoints = (int)Math.Ceiling((double)numfunc * excessFactor);

                IVector 
                    coefOriginal = new Vector(numfunc),
                    coefCalculated = new Vector(numfunc),
                    coefPerturbed = new Vector(dim),   // coefficients of a sum of 3rd powers used to calculate perturbation
                    pointValues = new Vector(numPoints),
                    approximatedValues = new Vector(numPoints), 
                    errors = new Vector(numPoints),
                    vec = new Vector(dim);
                List<IVector> points = new List<IVector>();
                ApproximatorWlsLinearBase wls = new ApproximatorWlsLinearBase(dim, true, basis);
                if (outputLevel >= 1)
                {
                    Console.WriteLine("Basis functions: " + Environment.NewLine + basis.ToString() + Environment.NewLine);
                }

                for (int dataNum = 1; dataNum <= numValueChanges; dataNum++)
                {

                    if (outputLevel >=1)
                    {
                        Console.WriteLine(Environment.NewLine + Environment.NewLine + 
                            "---------------------------------------------------------"  + Environment.NewLine + 
                            "Data change No. " + dataNum + ": " + Environment.NewLine);
                    }

                    // Coefficients of the linear combination:
                    for (int iAux = 0; iAux < numfunc; ++iAux)
                    {
                        coefOriginal[iAux] = rand.NextDouble();
                    }
                    int auxNum = numValueChanges - 1;
                    if (auxNum < 1)
                        auxNum = 1;
                    double iterationPerturbationFactor = perturbationFactor * (double)(dataNum - 1) / (double)(auxNum);
                    if (perturbationFactor != 0.0)
                    {
                        for (int iAux = 0; iAux < dim; ++iAux)
                            coefPerturbed[iAux] = rand.NextDouble() * iterationPerturbationFactor;
                    }
                    bool addPoints = changePoints || dataNum <= 1;
                    if (addPoints)
                    {
                        wls.ClearPoints(false);
                    }

                    // Sample values on the assumed linear combination:
                    t.Start();
                    for (int i=0; i<numPoints; ++i)
                    {
                        if (i >= points.Count)
                        {
                            points.Add(new Vector(dim));
                        }
                        if (addPoints)
                        {
                            // Also change the sampling points, not only values:
                            for (int j=0; j<dim; ++j)
                            {
                                points[i][j] = rand.NextDouble();
                            }
                        } else if (ClearSystemEquations)
                        {
                            // Clear system matrix even if sampling points do not change:
                            wls.ClearSystemEquations(false);
                            wls.ClearRightHandSides(false);
                        }

                        // Evaluete linear combination of basis functions at the generated point:
                        IVector coefficients = null;
                        VectorBase.Copy(coefOriginal, ref coefficients);
                        IVector point = points[i];
                        double val = basis.LinearCombinationValue(point, coefficients);
                        if (perturbationFactor != 0.0)
                        {
                            for (int iAux = 0; iAux < dim; ++iAux)
                                val += coefPerturbed[iAux] * Math.Pow(point[iAux], 3);
                        }
                        pointValues[i] = val;

                        // Update value on wls:
                        if (addPoints)
                        {
                            wls.AddValue(point, val);
                        } else
                        {
                            wls.SetValue(i, val);
                        }

                        if (outputLevel >= 3)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("Point No. " + i + ": ");
                            sb.AppendLine(points[i].ToString());
                            sb.AppendLine("  Sampled value: " + val);
                            Console.WriteLine(sb);
                        }
                    }
                    t.Stop();
                    if (outputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "Samplng performed in " + t.Time + "s (CPU: " + t.CpuTime + " s)."
                            + Environment.NewLine);
                    }

                    if (outputLevel >= 2)
                    {
                        StringBuilder sb1 = new StringBuilder();
                        sb1.AppendLine("After addition - points:");
                        for (int whichPoint = 0; whichPoint < points.Count; ++whichPoint)
                            sb1.AppendLine("  " + whichPoint + ": " + points[whichPoint].ToString());

                        sb1.AppendLine(Environment.NewLine + "Point values: ");
                        for (int whichPoint = 0; whichPoint < points.Count; ++whichPoint)
                            sb1.AppendLine("  " + whichPoint + ": " + pointValues[whichPoint]);
                        sb1.AppendLine(Environment.NewLine + "Coefficients: " + Environment.NewLine
                            + "  " + coefOriginal.ToString());
                        Console.WriteLine(Environment.NewLine + sb1.ToString() + Environment.NewLine);
                    }
                    if (true)
                    {
                        //wls.AddValues2d(points, pointValues.ToArray() /* weights = null, defaultWeight = 1 */);
                        if (outputLevel >= 2)
                        {
                            Console.WriteLine(Environment.NewLine + "WLS object after adding points: " + wls.ToString() + Environment.NewLine);
                        }
                        t.Start();
                        wls.CalculateCoefficients();  // not necessary, coef. automatically calculated; done separately to measure time.
                        t.Stop();
                        if (outputLevel >= 1)
                        {
                            Console.WriteLine(Environment.NewLine + "Approx. coefficients calculated in " + t.Time + "s (CPU: " + t.CpuTime + " s)."
                                + Environment.NewLine);
                        }
                        // Calculate approximation errors in sampling points and update the total absolute error:
                        t.Start();
                        for (int i=0; i<numPoints; ++i)
                        {
                            double val = approximatedValues[i] = wls.Value(points[i]);
                            double error = errors[i] = val - pointValues[i];
                            totalError += Math.Abs(error);
                            if (outputLevel >=2)
                            {
                                //Console.WriteLine(Environment.NewLine + "Total error: " + totalError + Environment.NewLine +
                                //    "  Divided by number of points: " + (totalError / (double)numPoints) + Environment.NewLine);

                                Console.WriteLine("Point No. " + i + ": approximated = " + val.ToString() 
                                    + ", exact = " + pointValues[i].ToString() + ", err = " + Math.Abs(error) + Environment.NewLine);
                            }
                        }
                        t.Stop();
                        if (outputLevel >= 1)
                        {
                        Console.WriteLine(Environment.NewLine + "Approximation error calculated in " + t.Time + "s (CPU: " + t.CpuTime + " s)."
                            + Environment.NewLine);
                            if (perturbationFactor != 0.0)
                            {
                                Console.WriteLine("Current perturbation factor: " + iterationPerturbationFactor.ToString());
                            }
                            Console.WriteLine(Environment.NewLine + "Total error: " + totalError + Environment.NewLine +
                                "  Divided by number of points: " + (totalError / (double)numPoints) + Environment.NewLine);
                        }
                    }

                }  // iteration over data changes

                if (outputLevel >= 1)
                {
                    Console.WriteLine(Environment.NewLine + "========" + Environment.NewLine 
                        + "End of the test." + Environment.NewLine);
                }
                return totalError / (double) numPoints;
            }
            catch(Exception ex)
            {
                if (outputLevel >=1)
                {
                    Console.WriteLine(Environment.NewLine + "EXCEPTION thrown: " + Environment.NewLine + "Message: " + Environment.NewLine
                        + ex.Message + Environment.NewLine);
                }
            }

            return 1.0e10;
        }  // TestWeightedLeastSquaresMultiplePolynomialBasis(...)


        #endregion Tests


    };  // class ApproximatorWlsLinearBase




}  // namespace Ig.Test
