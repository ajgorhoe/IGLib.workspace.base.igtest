
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
    public class ApproximatorMls : ApproximatorMlsBase
    {

        /// <summary>Default constructor: do not use!</summary>
        private ApproximatorMls(): this(0, true)
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
        public ApproximatorMls(int spaceDimension, bool copyData = true,
            VectorFunctionBaseComponentWise basisFunctions = null, IScalarFunction weightingFunction = null):
                base(spaceDimension, copyData, basisFunctions, weightingFunction)
        {  }
        
        /// <summary>Constructs a MLS approximation object with the specified basis and with copy mode set to true.</summary>
        /// <param name="spaceDimension">Dimension of the space on which WLS approximation is calculated.</param>
        /// <param name="basisFunctions">Linear basis of type <see cref="BasisFunctionsGeneric"/> that provides basis functions.</param>
        public ApproximatorMls(int spaceDimension, VectorFunctionBaseComponentWise basisFunctions, IScalarFunction weightingFunction) :
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
        /// <param name="transformation">Affine transformation that is used to transform the radial function defined by
        /// <paramref name=""function1d/>.</param>
        /// <remarks><para>For each input data provided (such as points), internal copies are creaded, and are
        /// deallocated when destructor is called.</para></remarks>
        public ApproximatorMls(int spaceDimension, bool copyData,
            VectorFunctionBaseComponentWise basisFunctions, IRealFunction function1d, IAffineTransformation transformation = null) :
            this(spaceDimension, true /* copyData */, basisFunctions, null)
        {
            this.WeightingFunction1d = function1d;
            if (transformation != null)
                this.WeightingFunctionTransformation = transformation;
        }



        /// <summary>Destructor.</summary>
        ~ApproximatorMls()
        {
            // TODO: Consider if we actually need a destructtor!

            ClearData();
        }


        #region Data


        #endregion Data


        #region Operation 


        #endregion Operation



        #region Tests



        #endregion Tests




    }  // class ApproximatorMls




#endif // if false


}  
