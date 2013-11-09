using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Interface representing sampling objects that produce a desired number of sampling points with
    /// a particular arrangement in space.</summary>
    /// $A Igor xx;
    public interface ISampler
    {

        /// <summary>Creates the next sampling point and stores it to the specified vector.</summary>
        /// <param name="samplingPoint">Vector where the generated sampling point is stored.</param>
        void GetSamplingPoint(ref IVector samplingPoint);

        /// <summary>Creates the specified number of next sampling points and stores it to the specified array of vectors.</summary>
        /// <param name="samplingPoint">Array of vectors where the generated sampling points are stored.
        /// <para>In general, the storage array and its elements will be resized if necessary, in order to 
        /// fit the number and dimension of sampling points.</para></param>
        void GetSamplingPoints(int numPoints, ref IVector[] samplingPoints);

        int SpaceDimension
        {
            get;
        }

    }  // interface ISampler


    /// <summary>Base class for sampling classes that produce a desired number of sampling points with
    /// a particular arrangement in space.</summary>
    /// $A Igor xx;
    public abstract class SamplerBase: ISampler, ILockable
    {

        /// <summary>Prevent calling default constructor.</summary>
        private SamplerBase() {  }

        /// <summary>Constructs a new sampling point generator for the specified dimension of sampling space.</summary>
        /// <param name="spaceDimension">Dimension of the sampling space.</param>
        public SamplerBase(int spaceDimension)
        {
            this.SpaceDimension = spaceDimension;
        }


        #region ThreadLocking

        private readonly object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ThreadLocking


        #region Operation

        /// <summary>Creates the next sampling point and stores it to the specified vector.</summary>
        /// <param name="samplingPoint">Vector where the generated sampling point is stored.</param>
        public abstract void GetSamplingPoint(ref IVector samplingPoint);

        /// <summary>Creates the specified number of next sampling points and stores it to the specified array of vectors.</summary>
        /// <param name="samplingPoint">Array of vectors where the generated sampling points are stored.
        /// <para>In general, the storage array and its elements will be resized if necessary, in order to 
        /// fit the number and dimension of sampling points.</para></param>
        public virtual void GetSamplingPoints(int numPoints, ref IVector[] samplingPoints)
        {
            lock (Lock)
            {
                ResizeSamplingPoints(numPoints, ref samplingPoints);
                IVector samplingPoint;
                for (int i = 0; i < numPoints; ++i)
                {
                    samplingPoint = samplingPoints[i];
                    GetSamplingPoint(ref samplingPoint);
                    samplingPoints[i] = samplingPoint;
                }
            }
        }

        #endregion Operation


        #region Data

        protected int _spaceDimension;

        public int SpaceDimension
        {
            get { lock (Lock) { return _spaceDimension; } }
            protected set
            {
                lock (Lock) { _spaceDimension = value; }
            }
        }

        #endregion Data


        #region AuxiliaryMethods

        ///// <summary>Auxiliary method that resizes the array of sampling points as necessary.</summary>
        ///// <param name="numPoints">Number of points that should fit in the array.</param>
        ///// <param name="samplingPoints">The array for storing sampling points that is resized.</param>
        //public void ResizeSamplingPoints(int numPoints, ref IVector[] samplingPoints)
        //{
        //    if (samplingPoints == null)
        //        samplingPoints = new IVector[numPoints];
        //    else
        //    {
        //        int num = samplingPoints.Length;
        //        if (num!=numPoints)
        //        {
        //            IVector[] newPoints = new IVector[numPoints];
        //            // Array was allocated anew, copy contnts from the old array:
        //            for (int i = 0; i < num && i < numPoints; ++i)
        //            {
        //                newPoints[i] = samplingPoints[i];
        //            }
        //            samplingPoints = newPoints;
        //        }
        //    }
        //}

        /// <summary>Auxiliary method that resizes the array of sampling points, and also 
        /// the vectors contained in it, as necessary.
        /// <para>Dimension of vectors contained in the array is specified by the <see cref="SpaceDimension"/> property.</para></summary>
        /// <param name="numPoints">Number of points that should fit in the array.</param>
        /// (sampling points) contained in the array.</param>
        /// <param name="samplingPoints">The array for storing sampling points that is resized.</param>
        public void ResizeSamplingPoints(int numPoints, ref IVector[] samplingPoints)
        {
            int dim = SpaceDimension;
            if (samplingPoints == null)
                samplingPoints = new IVector[numPoints];
            else
            {
                int num = samplingPoints.Length;
                if (num != numPoints)
                {
                    IVector[] newPoints = new IVector[numPoints];
                    // Array was allocated anew, copy contnts from the old array:
                    for (int i = 0; i < num && i < numPoints; ++i)
                    {
                        newPoints[i] = samplingPoints[i];
                    }
                    samplingPoints = newPoints;
                }
                IVector vec;
                for (int i = 0; i < numPoints; ++i)
                {
                    vec = samplingPoints[i];
                    Vector.Resize(ref vec, dim);
                    samplingPoints[i] = vec;
                }
            }
        }


        #endregion AuxiliaryMethods


    }  // abstract class SamplerBase


    /// <summary>Base class for sampling classes that produce a desired number of sampling points with
    /// a particular arrangement in space, and which use a random generator for creation of sampling points.</summary>
    /// $A Igor xx;
    public abstract class SamplerBaseRandom: SamplerBase, ISampler
    {

        #region Construction


        /// <summary>Construct a new sampling object with the specified random generator that is used for
        /// creating new sampling points.
        /// <para>Random generator can be null, in which case a global random generator will be used.</para></summary>
        /// <param name="spaceDimension">Dimension of the sampled space.</param>
        /// <param name="rand">The random generator that will be used for generation of sampling points.
        /// It can be null, in which case the global random generator will be used.</param>
        public SamplerBaseRandom(int spaceDimension, IRandomGenerator rand):
            base(spaceDimension)
        {
            _random = rand;
        }

        #endregion Construction

        #region Data

        IRandomGenerator _random;

        /// <summary>The random generator that will be used for generation of sampling points.
        /// If it is null, the next call to getter will automatically assign it to the global
        /// random generator.</summary>
        public IRandomGenerator Random
        {
            get
            {
                lock (Lock)
                {
                    if (_random == null)
                        _random = RandomGenerator.Global;
                    return _random;
                }
            }
            protected set { lock (Lock) { _random = value; } }
        }

        /// <summary>Sets the random generator that will be used for generation of sampling points.</summary>
        /// <param name="rand">Tandom generator to be used for generation of sampling points.</param>
        public void SetRandomGenerator(IRandomGenerator rand)
        {
            Random = rand;
        }

        #endregion Data

    }  // abstract class SamplerBaseRandom


    /// <summary>Uniformly distributed random sampling within the coordinate origin-centered cube.
    /// <para>Cube side lengths can be specified, and are 1 by default.</para></summary>
    /// $A Igor xx;
    public class SamplerCubeRandomUniform: SamplerBaseRandom, ISampler
    {
        
        /// <summary>Construct a new sampling object with the specified random generator that is used for
        /// creating new sampling points.
        /// <para>Constructed sampler object generates uniformly distributed random samples within a cube centered
        /// around coordinate origin with side lengths equal to 1.</para>
        /// <para>Random generator can be null, in which case a global random generator will be used.</para></summary>
        /// <param name="spaceDimension">Dimension of the sampled space.</param>
        /// <param name="rand">The random generator that will be used for generation of sampling points.
        /// It can be null, in which case the global random generator will be used.</param>
        public SamplerCubeRandomUniform(int spaceDimension, IRandomGenerator rand) :
            base(spaceDimension, rand)
        {  }

        /// <summary>Construct a new sampling object with the specified random generator that is used for
        /// creating new sampling points.
        /// <para>Constructed sampler object generates uniformly distributed random samples within a cube centered
        /// around coordinate origin with specified side length.</para>
        /// <para>Random generator can be null, in which case a global random generator will be used.</para></summary>
        /// <param name="spaceDimension">Dimension of the sampled space.</param>
        /// <param name="rand">The random generator that will be used for generation of sampling points.
        /// It can be null, in which case the global random generator will be used.</param>
        /// <param name="sideLength">Side length of the coordinate-origin centered unit cube that defines the
        /// sampling region.</param>
        public SamplerCubeRandomUniform(int spaceDimension, IRandomGenerator rand, double sideLength) :
            this(spaceDimension, rand)
        {
            this.SideLength = sideLength;
        }

        #region Data

        protected double _sideLength = 1.0;

        public double SideLength
        {
            get { lock (Lock) { return _sideLength; }  }
            protected set {
                lock (Lock)
                {
                    if (value <= 0)
                        throw new ArgumentException("Sampled cube's side length can not be less or equal to 1.");
                    _sideLength = value;
                }
            }
        }

        #endregion Data

        #region Operation 


        /// <summary>Creates the next sampling point and stores it to the specified vector.</summary>
        /// <param name="samplingPoint">Vector where the generated sampling point is stored.</param>
        public override void GetSamplingPoint(ref IVector samplingPoint)
        {
            lock (Lock)
            {
                IRandomGenerator rnd = Random;
                int dim = SpaceDimension;
                Vector.Resize(ref samplingPoint, dim);
                double sideLenght = SideLength;
                for (int i = 0; i < dim; ++i)
                {
                    samplingPoint[i] = SideLength * (rnd.NextDouble() - 0.5);
                }
            }
        }

        #endregion Operation

    } // class SamplerCubeRandomUniform




    /// <summary>Uniformly distributed random sampling within the coordinate origin-centered cube.
    /// <para>Cube side lengths can be specified, and are 1 by default.</para></summary>
    /// $A Igor May08;
    public class SamplerBoxRandomUniform : SamplerBaseRandom, ISampler
    {

        /// <summary>Construct a new sampling object with the specified random generator that is used for
        /// creating new sampling points, and the specified bounding box that defines the sampling region.
        /// <para>Constructed sampler object generates uniformly distributed random samples within the specified
        /// bounding box.</para>
        /// <param name="spaceDimension">Dimension of the sampled space.</param>
        /// <para>Random generator can be null, in which case a global random generator will be used.</para></summary>
        /// <param name="rand">The random generator that will be used for generation of sampling points.
        /// It can be null, in which case the global random generator will be used.</param>
        public SamplerBoxRandomUniform(int spaceDimension, IRandomGenerator rand, IBoundingBox box) :
            base(spaceDimension, rand)
        {
            // Remarks: 
            // If bounding box is null or its dimension is different than the space dimension (which 
            // has been set by the called base constructor), the property's setter will throw an exception.
            this.Box = box;
        }

        /// <summary>Construct a new sampling object with the specified random generator that is used for
        /// creating new sampling points.
        /// <para>Constructed sampler object generates uniformly distributed random samples within a cube centered
        /// around coordinate origin with specified side length.</para>
        /// <para>Dimension of the space is obtained from the bounding box.</para>
        /// <para>Random generator can be null, in which case a global random generator will be used.</para></summary>
        /// <param name="rand">The random generator that will be used for generation of sampling points.
        /// It can be null, in which case the global random generator will be used.</param>
        public SamplerBoxRandomUniform(IRandomGenerator rand, IBoundingBox box) :
            this(box==null?1:box.Dimension, rand, box)
        {
            // Remarks:
            // If box is null, spaceDimension is just set to 1. The other construcor call will
            // in this case throw exception while setting the Box property, notifying that the
            // bounding box attempted to set was null.
        }

        #region Data

        protected IBoundingBox _box;

        /// <summary>Bounding box that defines the sampling region.</summary>
        public IBoundingBox Box
        {
            get { lock (Lock) { return _box; } }
            protected set
            {
                lock (Lock)
                {
                    if (value == null)
                        throw new ArgumentException("Bounding box defining the sampling region is not specified (null reference).");
                    int dim = value.Dimension;
                    if (dim != SpaceDimension)
                        throw new ArgumentException("Dimension of the provided bounding box (" + dim
                            + ") does not match the specified space dimension (" + SpaceDimension + ").");
                    {
                        for (int i = 0; i < dim; ++i)
                        {
                            if (!value.IsMinDefined(i) || !value.IsMaxDefined(i))
                                throw new ArgumentException("Bounds are not defined for the coordinate No. " + i + 
                                    " of the bounding box defining the sampling region.");
                        }
                    }
                    _box = value;
                }
            }
        }

        #endregion Data

        #region Operation


        /// <summary>Creates the next sampling point and stores it to the specified vector.</summary>
        /// <param name="samplingPoint">Vector where the generated sampling point is stored.</param>
        public override void GetSamplingPoint(ref IVector samplingPoint)
        {
            lock (Lock)
            {
                IRandomGenerator rnd = Random;
                int dim = SpaceDimension;
                IBoundingBox box = Box;
                Vector.Resize(ref samplingPoint, dim);
                for (int i = 0; i < dim; ++i)
                {
                    double min = box.GetMin(i);
                    double max = box.GetMax(i);
                    samplingPoint[i] = min + (max - min) * rnd.NextDouble();
                }
            }
        }

        #endregion Operation

    } // class SamplerBoxRandomUniform



    /// <summary>Random sampling within the coordinate origin-centered hyper ball with radius one; 
    /// derived from uniform random sampling of unit cube by radially stretching (or shrinking) the cube 
    /// surface in order to fit the ball surface.
    /// <remarks>
    /// <para>Sampling procedure is as follows:</para>
    /// <para>A reference sample is first generated by uniform random sampling proxedure for a zero
    /// centered cube with side lengths equal to 1. Then, the reference sample is mapped by a non-smooth
    /// mapping that maps the surface of the hyper cube into the surface of the origin-centered hyper 
    /// ball with radius 1. Mapping maps in radial directions of the reference sample. It fisrt extends
    /// the reference sample's radius vector to the the cube surface (this is simply done by multiplying
    /// the vector by a factor that sets maximal absolute vector eleemnt to 0.5) in order to calculate the distance
    /// on the cube surface (in the direction of the sample) from the origin. Then, the generated reference
    /// sample is divided by this distance, ensuring that actual samples generated in the same direction
    /// will fit exactly within the unit ball.</para>
    /// <para>This procedure produces random samples whose density is larger around the cube corners.</para>
    /// </remarks>
    /// $A Igor xx;
    public class SamplerUnitBallRandomFromCube : SamplerBaseRandom, ISampler
    {

        /// <summary>Construct a new sampling object with the specified random generator that is used for
        /// creating new sampling points.
        /// <para>Constructed sampler object generates random samples within a hzper ball with radius 1 centered
        /// around the coordinate origin.</para>
        /// <para>Random generator can be null, in which case a global random generator will be used.</para></summary>
        /// <param name="spaceDimension">Dimension of the sampled space.</param>
        /// <param name="rand">The random generator that will be used for generation of sampling points.
        /// It can be null, in which case the global random generator will be used.</param>
        public SamplerUnitBallRandomFromCube(int spaceDimension, IRandomGenerator rand) :
            base(spaceDimension, rand)
        { }


        #region Operation


        /// <summary>Creates the next sampling point and stores it to the specified vector.</summary>
        /// <param name="samplingPoint">Vector where the generated sampling point is stored.</param>
        public override void GetSamplingPoint(ref IVector samplingPoint)
        {
            double smallPositiveNumber = 1.0e-40;  // for numerical stability
            lock (Lock)
            {
                IRandomGenerator rnd = Random;
                int dim = SpaceDimension;
                Vector.Resize(ref samplingPoint, dim);
                // Generate a reference sampled point first, which will fall within the
                // zero centered cube with side length 0.5:
                double maxElementAbs = 0.0;  // maximal absolute value of elements of the generated vector
                double vectorSize = 0.0;  // size of the generated vector (Euclidean norm)
                for (int i = 0; i < dim; ++i)
                {
                    double element = (rnd.NextDouble() - 0.5);
                    samplingPoint[i] = element;
                    vectorSize += element * element;
                    double elementAbs = Math.Abs(element);
                    if (elementAbs > maxElementAbs)
                        maxElementAbs = elementAbs;
                }
                vectorSize = Math.Sqrt(vectorSize);
                if (maxElementAbs > smallPositiveNumber)
                {
                    // The factor (0.5/maxElementAbs) is the factor by which vector should be multiplied in order 
                    // to fit to the hypercube surface. 1/(vector vectorSize*(0.5/maxElementAbs)) is the factor by which
                    // a point on the cube surface obtained in this way should be multiplied in order to fit
                    // on the unit radius hyper ball's surface. We must multiply the reference sampling point 
                    // by this factor:
                    double factor = maxElementAbs / (0.5 * vectorSize);
                    for (int i = 0; i < dim; ++i)
                    {
                        samplingPoint[i] *= factor;
                    }
                }
            }
        }

        #endregion Operation

        #region Testing

        /// <summary>Generates the specified number of samples of the specified dimension, and prints 
        /// distances from the coordinate origin to the console. A warning is printed whenever a distance
        /// is greater thatn 1.</summary>
        /// <param name="dimension">Dimension of the sampling space.</param>
        /// <param name="numGenerated">Number of samples generated.</param>
        public static void TestSampleNorms(int dimension, int numGenerated)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Test of generation of random samples within a origin-centered unit ball.");
            Console.WriteLine("    Space dimension: " + dimension);
            Console.WriteLine("    Numm. samples:   " + numGenerated );
            ISampler sampler = new SamplerUnitBallRandomFromCube(dimension, null);
            Console.WriteLine("    Sampler Type: " + sampler.GetType().Name);
            IVector samplingPoint = new Vector(dimension);
            double minSize = double.MaxValue;
            double maxSize = 0.0;
            for (int i=0; i< numGenerated; ++i)
            {
                sampler.GetSamplingPoint(ref samplingPoint);
                double size = samplingPoint.NormEuclidean;
                if (size < minSize)
                    minSize = size;
                if (size > maxSize)
                    maxSize = size;
                Console.Write("{0,-8:G4} ",size);
                if (size > 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: size of the last generated point above was greater than 1 by {0,-5:G4}.",
                        (size-1.0));
                }
                if (i % 8 == 5)
                    Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Minimal vector length of any generated sampling points: {0, 1:G4}.", minSize);
            Console.WriteLine("Maximal vector length of any generated sampling points: {0}.", maxSize);
            Console.WriteLine("1-Max. vector length of any generated sampling points:  {0, 1:G4}.", 1 - maxSize);
            Console.WriteLine();
        }


        /// <summary>Generates the specified number of samples of the specified dimension, and prints 
        /// distances from the coordinate origin to the console. A warning is printed whenever a distance
        /// is greater thatn 1.</summary>
        /// <param name="dimension">Dimension of the sampling space.</param>
        /// <param name="numGenerated">Number of samples generated.</param>
        public static void TestSamplingSpeed(int dimension, int numGenerated)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Test of speed of generation of random samples.");
            Console.WriteLine("    Space dimension: " + dimension);
            Console.WriteLine("    Numm. samples:   " + numGenerated);
            ISampler sampler = new SamplerUnitBallRandomFromCube(dimension, null);
            Console.WriteLine("    Sampler Type: " + sampler.GetType().Name);
            IVector samplingPoint = new Vector(dimension);
            double minSize = double.MaxValue;
            double maxSize = 0.0;
            StopWatch t = new StopWatch();
            t.Start();
            for (int i = 0; i < numGenerated; ++i)
            {
                sampler.GetSamplingPoint(ref samplingPoint);
                double size = samplingPoint.NormEuclidean;
                if (size < minSize)
                    minSize = size;
                if (size > maxSize)
                    maxSize = size;
            }
            t.Stop();
            Console.WriteLine();
            Console.WriteLine("Minimal vector length of any generated sampling points: {0, 1:G4}.", minSize);
            Console.WriteLine("Maximal vector length of any generated sampling points: {0}.", maxSize);
            Console.WriteLine("1-Max. vector length of any generated sampling points:  {0, 1:G4}.", 1 - maxSize);
            Console.WriteLine();
            Console.WriteLine("Time of generation of " + numGenerated + " " + dimension + "-D points: {0,0:G4} s.", t.TotalTime);
            Console.WriteLine("Time of generation of a single sample: {0,0:G4} s.", t.TotalTime / (double)numGenerated);
            Console.WriteLine();
        }


        #endregion Testing

    } // class SamplerCubeRandomUniform








}
