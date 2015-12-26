
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IG.Lib;
using IG.Num;

namespace IG.Num
{

#if true

    

    /// <summary>Calculates moving least squares approximations.</summary>
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
    public class ApproximatorMlsBase: ApproximatorWlsLinearBase
    {

        /// <summary>Default constructor: do not use!</summary>
        private ApproximatorMlsBase(): this(0, true)
        {
            throw new InvalidOperationException("Constructor without parameters is not legal for MLS approximators.");
        }



        /// <summary>Constructs a MLS approximation object with the specified space dimension.</summary>
        /// <param name="spaceDimension">Dimension of the space on which WLS approximation is calculated.</param>
        /// <param name="copyData">Specifies whether the object will create dynamic copies of input data when provided.
        /// <para>When true, for each input data provided (such as points and weights) internal copies are creaded.</para>	
        /// <para>When false, reference of provided data is taken.</para></param>
        /// <remarks><para>For each input data provided (such as points), internal copies are creaded, and are
        /// deallocated when destructor is called.</para></remarks>
        public ApproximatorMlsBase(int spaceDimension, bool copyData = true,
            VectorFunctionBaseComponentWise basisFunctions = null, IScalarFunction weightingFunction = null):
                base(spaceDimension, copyData, basisFunctions)
        {
            if (weightingFunction == null)
                this.WeightingFunction = null;
            else this.WeightingFunction = weightingFunction;
        }
        
        /// <summary>Constructs a MLS approximation object with the specified basis and with copy mode set to true.</summary>
        /// <param name="spaceDimension">Dimension of the space on which WLS approximation is calculated.</param>
        /// <param name="basisFunctions">Linear basis of type <see cref="BasisFunctionsGeneric"/> that provides basis functions.</param>
        public ApproximatorMlsBase(int spaceDimension, VectorFunctionBaseComponentWise basisFunctions, IScalarFunction weightingFunction) :
            this(spaceDimension, true /* copyData */, basisFunctions, weightingFunction)
        {  }


        /// <summary>Constructs a MLS approximation object with the specified space dimension, and an 1D weighting
        /// function that defines a radial function, and possibly an affine transformation that transforms such radial
        /// weighting function into a more general one.</summary>
        /// <param name="spaceDimension">Dimension of the space on which WLS approximation is calculated.</param>
        /// <param name="copyData">Specifies whether the object will create dynamic copies of input data when provided.
        /// <para>When true, for each input data provided (such as points and weights) internal copies are creaded.</para>	
        /// <para>When false, reference of provided data is taken.</para></param>
        /// <param name="basisFunctions">Basis functions used in approximation.</param>
        /// <param name="function1d">1D function that defines shape of the weighting function of vector parameter.</param>
        /// <paramref name=""function1d/>1d weighting function, which specifies the (eventually affine-transformed) radial function in n-d.</param>
        /// <param name="transformation">Affine transformation that is used to transform the radial function defined by
        /// <remarks><para>For each input data provided (such as points), internal copies are creaded, and are
        /// deallocated when destructor is called.</para></remarks>
        public ApproximatorMlsBase(int spaceDimension, bool copyData,
            VectorFunctionBaseComponentWise basisFunctions, IRealFunction function1d, IAffineTransformation transformation /*= null */) :
            this(spaceDimension, true /* copyData */, basisFunctions, (IScalarFunction) null /* weighting function */)
        {
            this.WeightingFunction1d = function1d;
            if (transformation != null)
                this.WeightingFunctionTransformation = transformation;
        }



        /// <summary>Destructor.</summary>
        ~ApproximatorMlsBase()
        {
            // TODO: Consider if we actually need a destructtor!

            ClearData();
        }


        #region Data


        protected IScalarFunction _weightingFunction;

        /// <summary>Weighting function used for Moving least squares approximations.
        /// <para>Default is a constant function, which reduces the approximation method to the ordinary
        /// weighted least squares. In this case, the <see cref="ApproximatorWlsLinearBase"/>
        /// should be used as is more efficient.</para></summary>
        /// <remarks>When setting the weighting function, all values that depend on this function are invalidated.
        /// This is true even if the new weighting function reference is the same as the old one. This enables invalidation 
        /// of dependent values even if only some parameter contained on the weighting function is changed.
        /// <para>Weighting functions can be alternatively be specified as affine-transformed radial basis functions defined 
        /// by a function of a single variable and affine transformation that transforms the radially symmetric function 
        /// defined by the 1D function. This is handled by properties <see cref="RadialWeightingFunction"/>,
        /// <see cref="WeightingFunction1d"/> and <see cref="WeightingFunctionTransformation"/>. These properties 
        /// are synchronized with this property. They can be get or set, which will properly affect the value of this 
        /// property. Rule are as follows:</para>
        /// <para>If the current weighting function is not a radial weighting function, the radial weighting function
        /// property getter will evaluate to null. Otherwise, the radial weighting function property getter will evaluate
        /// to the value of this properrty. If the radial weighting function property is set, this will also set this
        /// property to the same value.</para>
        /// <para>Getters of 1D function and affine transform will return non-null value if the weighting function is a radial
        /// function (possibly transformed), and values will be as they are on the radial function. </para>
        /// <para>Setter of 1D function will change the 1D function of the radial weighting function if the current weighting
        /// function is a radial function of type <see cref="ScalarFunctionRadial"/>. Otherwise, it will create a radial scalar
        /// function with the specified 1D function, and set the 1D funciton on it to the 1D function value that was set.
        /// Smilar is true for affine transformation, except that the setter can not be called if the current weighting
        /// function is not a radial weighting function (such a call would produce an error).</para></remarks>
        public virtual IScalarFunction WeightingFunction {
            get {
                if (_weightingFunction == null)
                {
                    _weightingFunction = new ScalarFunctionTransformed(new ScalarFunctionConstant(0.0), null);
                }
                return _weightingFunction; 
            }
            set
            {
                ClearSystemEquations(false);
                ClearRightHandSides(false);
                IsWeightingFunctionsCalculated = false;
                RadialWeightingFunction = null;
                _weightingFunction = value;
            }
        }

        protected ScalarFunctionRadial _radialWeightingFunction;

        /// <summary>Radial scalar function (eventually affine transformed) that is currently used as the weighting function. 
        /// <para>If the current weighting function is not a radial function then getter returns null. Setter also sets the 
        /// current weighting function defined by the <see cref="WeightingFunction"/> property.</para></summary>
        public ScalarFunctionRadial RadialWeightingFunction
        {
            get {
                if (_radialWeightingFunction == null)
                {
                    if (_weightingFunction != null)
                        _radialWeightingFunction = _weightingFunction as ScalarFunctionRadial;
                }
                return _radialWeightingFunction;
            }
            set
            {
                WeightingFunction = value;
                if (value == null)
                {
                    _weightingFunction1d = null;
                    _weightingFunctionTransformation = null;
                } else
                {
                    _weightingFunction1d = value.Function;
                    _weightingFunctionTransformation = value.Transformation;
                }
                _radialWeightingFunction = value;
            }
        }


        protected IRealFunction _weightingFunction1d;

        /// <summary>Gets or sets the 1D weighting function that defines the radial function (eventually affine transformed)
        /// that is the current weighting function of this object.
        /// <para>If the current weighting function is not a radial function then getter returns null.</para>
        /// <para>Setter also sets the 1D function that defines the radial function used as weighting function. If the
        /// current weighting function is not a radial function then setter creates a new radial function ant the current
        /// weighting function is set to the newly created function.</para></summary>
        public IRealFunction WeightingFunction1d
        {
            get 
            {
                if (_weightingFunction1d == null)
                {
                    ScalarFunctionRadial wr = RadialWeightingFunction;
                    if (wr != null)
                        _weightingFunction1d = wr.Function;
                }
                return _weightingFunction1d; 
            }
            set 
            {
                if (value == null)
                {
                    _weightingFunctionTransformation = null;
                    WeightingFunction = null;
                } else
                {
                    ScalarFunctionRadial wr = RadialWeightingFunction;
                    if (wr == null)
                        wr = new ScalarFunctionRadial(value, WeightingFunctionTransformation);
                    else
                        wr.SetFunction(value);
                    RadialWeightingFunction = wr;  // This will set the weighting function and invalidate dependencies
                }
            } 
        }


        private IAffineTransformation _weightingFunctionTransformation;

        /// <summary>Gets or sets the affine transformation of the radial function that is the current weighting function 
        /// of this object.
        /// <para>If the current weighting function is not a radial function then getter returns null.</para>
        /// <para>Setter may not be called if the current weighting function is not a radial function. In such
        /// situation, exception woulld be thrown.</para></summary>
        public IAffineTransformation WeightingFunctionTransformation
        {
            get 
            {
                if (_weightingFunctionTransformation == null)
                {
                    ScalarFunctionRadial wr = RadialWeightingFunction;
                    if (wr != null)
                        _weightingFunctionTransformation = wr.Transformation;
                }
                return _weightingFunctionTransformation; 
            }
            set 
            { 
                if (value == null)
                {
                    _weightingFunction1d = null;
                    WeightingFunction = null;
                } else
                {
                    ScalarFunctionRadial wr = RadialWeightingFunction;
                    if (wr == null)
                    {
                        throw new ArgumentException("Can not set affine transformation for WLS, radial weighting function is not defined. "
                            + Environment.NewLine + "  You should set the 1D weighting function before setting the transformation.");
                        // wr = new ScalarFunctionRadial(WeightingFunction1d, value);
                    } else
                        wr.Transformation = value;
                    RadialWeightingFunction = wr;  // This will set the weighting function and invalidate dependencies
                }
            }
        }
        


        private IVector _evaluationPoint;

        /// <summary>Gets or sets the current evaluation point.
        /// <para>When weighting functions are calculated, the final weights are also calculated (property <see cref="FinalWeights"/>).,
        /// as well as their squares (property <see cref="FinalWeightsSquare"/>).</para></summary>
        public IVector EvaluationPoint {
            get { return _evaluationPoint; }
            set
            {
                if (! VectorBase.Equals(value, _evaluationPoint))
                {
                    if (value == null)
                        throw new ArgumentException("Evaluation point not specified (null reference).");
                    ClearSystemEquations(false);
                    ClearRightHandSides(false);
                    if (IsCopyData)
                    {
                        int dim = value.Length;
                        if (_evaluationPoint == null || _evaluationPoint.Length != dim)
                            _evaluationPoint = value.GetNew();
                    }
                    else
                    {
                        _evaluationPoint = value;
                    }
                }
            }
        }

        /// <summary>Values of weighting funcitions in sampling points.
        /// <para></para></summary>
        protected List<double> WeightingFunctionValues { get; set; }

        protected List<double> FinalWeights { get; set; }

        //protected List<double> FinalSquareWeights { get; set; }

        /// <summary>Gets or sets a flag indicating whether values of basis functions in sampling points are calculated.</summary>
        public virtual bool IsWeightingFunctionsCalculated { get; protected set; }


        #endregion Data


        #region Operation 

        
        /// <summary>Returns final weight in the specified point.</summary>
        /// <param name="whichPoint">Index of point for which final wight is returned.</param>
        /// <remarks>This method enables use of the same methods for calculation of the system matrix and right-hand sides
        /// regardless of how weights are actually calculated. Inherited classes must only override this function.</remarks>
        public override double GetFinalWeight(int whichPoint)
        {
            return FinalWeights[whichPoint];
        }


        /// <summary>Calculates weighting functions and final values of weights corresponding to sampling points.</summary>
        /// <param name="trimExcessive">If true then excessive elements of the lists where weighting functions and final weights
        /// are stored, are removed if the lists have more elements than necessary. If false, the excessive elements
        /// are left in the list as they will be ignores in calculations anyway. The latter possibility may consume more
        /// memory but may be faster because of avoiding deletions of list elements.</param>
        /// <remarks>The <see cref="TrimExcessive"/> property influences the operation of this methos (if true then excessive
        /// elements on the precalculated lists may be removed).</remarks>
        public virtual void CalculateWeightingFunctions()
        {
            if (! IsWeightingFunctionsCalculated)
            {

                // Assure sufficient number of elements - weighting function values:
                if (WeightingFunctionValues == null)
                    WeightingFunctionValues = new List<double>();
                int currentCount = WeightingFunctionValues.Count;
                if (currentCount < NumPoints)
                {
                    if (WeightingFunctionValues.Capacity < NumPoints)
                        WeightingFunctionValues.Capacity = NumPoints;
                    int numItemsToAdd = NumPoints - currentCount;
                    for (int i = 0; i < numItemsToAdd; ++i)
                        WeightingFunctionValues.Add(0);
                }
                else if (TrimExcessive && currentCount > NumPoints)
                {
                    int numItemsToRemove = currentCount - NumPoints;
                    WeightingFunctionValues.RemoveRange(NumPoints, numItemsToRemove);
                }
                // Assure sufficient number of elements - final weights:
                if (FinalWeights == null)
                    FinalWeights = new List<double>();
                currentCount = FinalWeights.Count;
                if (currentCount < NumPoints)
                {
                    if (FinalWeights.Capacity < NumPoints)
                        FinalWeights.Capacity = NumPoints;
                    int numItemsToAdd = NumPoints - currentCount;
                    for (int i = 0; i < numItemsToAdd; ++i)
                        FinalWeights.Add(0);
                }
                else if (TrimExcessive && currentCount > NumPoints)
                {
                    int numItemsToRemove = currentCount - NumPoints;
                    FinalWeights.RemoveRange(NumPoints, numItemsToRemove);
                }

                //// Assure sufficient number of elements - final square weights:
                //if (FinalSquareWeights == null)
                //    FinalSquareWeights = new List<double>();
                //currentCount = FinalSquareWeights.Count;
                //if (currentCount < NumPoints)
                //{
                //    if (FinalSquareWeights.Capacity < NumPoints)
                //        FinalSquareWeights.Capacity = NumPoints;
                //    int numItemsToAdd = NumPoints - currentCount;
                //    for (int i = 0; i < numItemsToAdd; ++i)
                //        FinalSquareWeights.Add(0);
                //}
                //else if (TrimExcessive && currentCount > NumPoints)
                //{
                //    int numItemsToRemove = currentCount - NumPoints;
                //    FinalSquareWeights.RemoveRange(NumPoints, numItemsToRemove);
                //}
                for (int whichPoint = 0; whichPoint < NumPoints; whichPoint++)
                {
                    double w = WeightingFunction.Value(Points[whichPoint]);
                    WeightingFunctionValues[whichPoint] = w;
                    w *= PointWeights[whichPoint];
                    FinalWeights[whichPoint] = w;
                    //FinalSquareWeights[whichPoint] = w * w;
                }
                IsBasisFunctionsCalculated = true;
            }
        }


        /// <summary>Calculates the system matrix of the linear system of equations for calculation of 
        /// coefficients of the WLS approximation.</summary>
        public virtual void CalculateSystemMatrix()
        {
            CalculateWeightingFunctions();
            base.CalculateSystemMatrix();
        }


        /// <summary>Calculated the right-hand sides of the linear system of equations for calculation of 
        /// coefficients of the WLS approximation.</summary>
        public override void CalculateRighthandSides()
        {
            CalculateWeightingFunctions();
            base.CalculateRighthandSides();
        }


        #endregion Operation



        #region Tests



        #endregion Tests




    }  // class ApproximatorMlsBase




#endif // if false


}  
