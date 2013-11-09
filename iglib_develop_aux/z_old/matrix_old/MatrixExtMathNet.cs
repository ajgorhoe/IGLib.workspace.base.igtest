// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

// EXTENDED DEFINITION OF Matrix and Vector - with conversion support for MatDotNet library.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;

// From MathNet:

using Vector_MathNet = MathNet.Numerics.LinearAlgebra.Vector;
using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;
using MathNet.Numerics.RandomSources;
using MathNet.Numerics.Distributions;


// From MathNetNumerics:

using Vector_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseVector;
using VectorBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Generic.Vector<double>;

using Matrix_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;
using MatrixBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Generic.Matrix<double>;
using System.Globalization;



namespace IG.Old
{

    [Obsolete("Old class, use IG.Num.Vector instead.")]
    public class VectorWithMathNet : Vector
    {
        
        #region Construction

       protected internal VectorWithMathNet(): base()
        {  }

       /// <summary>Constructs a vector from aanother array.</summary>
       /// <param name="vec">Vector whose components are copied to the current vector.</param>
       /// <seealso cref="Create"/>
       public VectorWithMathNet(IVector vec)
           : base(vec)
       { }

        /// <summary>Constructs an n-dimensional vector of zeros.</summary>
        /// <param name="n">Dimensionality of vector.</param>
       public VectorWithMathNet(int n)
           : base(n)
       { }

        /// <summary>Constructs an n-dimensional unit vector for i'th coordinate.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="which">Specifies which unit vector is constructed (which components equals one).</param>
       public VectorWithMathNet(int n, int which)
           : base(n, which)
       { }

        /// <summary>Constructs an n-dimensional constant vector with all components initialized to the specified value.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="value">Value to which all components are set.</param>
       public VectorWithMathNet(int n, double value)
           : base(n, value)
       { }


        // TODO: check how constructor with array arguments copies vec! If it just copies the reference then 
        // consider whethe this should be re-implemented!

        /// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        /// <param name="vec">One-dimensional array of doubles.</param>
        /// <seealso cref="Create"/>
       public VectorWithMathNet(params double[] components)
           : base(components)
       { }

        /// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        /// <param name="vec">One-dimensional array of doubles.</param>
        /// <seealso cref="Create"/>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        public VectorWithMathNet(Vector_MathNet vec) 
        {
            if (vec==null)
                throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
            int length=vec.Length;
            if (length <= 0)
                throw new ArgumentException("Vector creation: array of components contains no values, can not create a vector with dimensionality 0.");
            _length = vec.Length;
            _elements = new double[_length];
            for (int i = 0; i < _length; ++i)
                _elements[i] = vec[i];
        }

        /// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        /// <param name="vec">One-dimensional array of doubles.</param>
        /// <seealso cref="Create"/>
        public VectorWithMathNet(VectorBase_MathNetNumerics vec)
            : base(vec)
        { }

        #region StaticConstruction


        /// <summary>Constructs a vector from a 1-D array.</summary>
        public static new VectorWithMathNet Create(double[] components)
        {
            VectorWithMathNet ret = null;
            if (components == null)
                throw new ArgumentNullException("Array of components is not specified (null reference).");
            if (components.Length <= 0)
                throw new ArgumentNullException("Array length is 0.");
            ret = new VectorWithMathNet(components.Length);
            for (int i = 0; i < components.Length; ++i)
                ret[i] = components[i];
            return ret;
        }

        /// <summary>Constructs a vector as a copy of a MathNetVector object.</summary>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        public static new VectorWithMathNet Create(Vector_MathNet vec)
        {
            VectorWithMathNet ret = null;
            if (vec == null)
                throw new ArgumentNullException("Vector is not specified (null reference).");
            if (vec.Length <= 0)
                throw new ArgumentNullException("Vector length is 0.");
            ret = new VectorWithMathNet(vec.Length);
            for (int i = 0; i < vec.Length; ++i)
                ret[i] = vec[i];
            return ret;
        }
        /// <summary>Constructs a vector as a copy of a MathNetVector object.</summary>
        public static new VectorWithMathNet Create(VectorBase_MathNetNumerics vec)
        {
            VectorWithMathNet ret = null;
            if (vec == null)
                throw new ArgumentNullException("Vector is not specified (null reference).");
            if (vec.Count <= 0)
                throw new ArgumentNullException("Vector length is 0.");
            ret = new VectorWithMathNet(vec.Count);
            for (int i = 0; i < vec.Count; ++i)
                ret[i] = vec[i];
            return ret;
        }


        /// <summary>Constructs a vector as a copy of another Vector object.</summary>
        public static new VectorWithMathNet Create(Vector vec)
        {
            VectorWithMathNet ret = null;
            if (vec == null)
                throw new ArgumentNullException("Vector is not specified (null reference).");
            if (vec.Length <= 0)
                throw new ArgumentNullException("Vector length is 0.");
            ret = new VectorWithMathNet(vec.Length);
            for (int i = 0; i < vec.Length; ++i)
                ret[i] = vec[i];
            return ret;
        }

        ///// <summary>Generates vector with random elements.</summary>
        ///// <param name="d2">Dimensionality of vector.</param>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source</param>
        ///// <returns>An d2-dimensional vector with random elements distributed according
        ///// to the specified random distribution.</returns>
        //public static Vector Random(int n,IContinuousGenerator randomDistribution)
        //{
        //    if (n <= 0)
        //        throw new ArgumentException("Can not create a vector with dimensionality less thatn 1. Provided dimensionality: "
        //            + n.ToString() + ".");
        //    Vector ret = new Vector(n);
        //    for (int i = 0; i < n; i++)
        //    {
        //        ret[i] = randomDistribution.NextDouble();
        //    }
        //    return ret;
        //}

        /// <summary>Generates vector with random elements uniformly distributed on [0, 1).</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <returns>An d2-dimensional vector with uniformly distributedrandom elements in <c>[0, 1)</c> interval.</returns>
        public static new VectorWithMathNet Random(int n)
        {
            VectorWithMathNet ret = new VectorWithMathNet(n);
            ret.SetRandom();
            return ret;
            // return Random(n, new SystemRandomSource());
        }

        /// <summary>Generates an d2-dimensional vector filled with 1.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        public static new VectorWithMathNet Ones(int n)
        {
            return new VectorWithMathNet(n, 1.0);
        }

        /// <summary>Generates an d2-dimensional vector filled with 0.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        public static VectorWithMathNet Zeros(int n)
        {
            return new VectorWithMathNet(n, 0.0);
        }

        /// <summary>Generates an d2-dimensional unit vector for i-th coordinate.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <param name="i">Coordinate index.</param>
        public static new Vector BasisVector(int n, int i)
        {
            return new VectorWithMathNet(n, i);
        }

        #endregion StaticConstruction

        #endregion  Construction


        #region MathNetSupport

        //  WARNING: This region will be removed in the future! MathNet.Numerics library will be used instead of MathNet!

        Vector_MathNet _copyMathNet;

        protected bool _mathNetConsistent = false;


        /// <summary>Tells whether the internal MathNet representation of the current vector is 
        /// consistent with the current vector. The MathNet representation is used for operations that
        /// are used from that library such as different kinds of decompositions.</summary>
        /// <remarks>Currrently, an internal flag indicating consistency of the MathNet matrix is not used.
        /// Every time this property is required, the consistence is actually verified by comparing values.
        /// There may be derived matrix classes where the falg is actually used. These must keep track
        /// when anything in the matrix changes and invalidate the flag on each such event.</remarks>
        protected internal virtual bool IsCopyMathNetConsistent
        {
            get
            {
                if (_copyMathNet == null)
                    return false;
                else if (_copyMathNet.Length != Length)
                    return false;
                else
                {
                    for (int i = 0; i < _length; ++i)
                    {
                        if (_copyMathNet[i] != _elements[i])
                            return false;
                    }
                }
                return true;
            }
            protected set
            {
                _mathNetConsistent = value;
            }
        }

        //$$
        ///// <summary>Copies values from the specified Math.Net vector.</summary>
        ///// <param name="v">Vector from which elements are copied.</param>
        //protected void CopyFromMatNetMatrix(Vector_MathNet v)
        //{
        //    if (v == null)
        //        throw new ArgumentNullException("Vector to copy components from is not specified (null reference).");
        //    if (v.Length == 0)
        //        throw new ArgumentException("Vector to copy components from has 0 elements.");
        //    // Copy the array to a jagged array:
        //    int numRows = _length = v.Length;
        //    _elements = new double[_length];
        //    for (int i = 0; i < _length; ++i)
        //    {
        //        _elements[i] = v[i];
        //    }
        //}

        /// <summary>Gets the internal MathNet representation of the current vector.
        /// Representation is created on demand. However, the same copy is returned
        /// as long as it is consistent with the current matrix.
        /// Use GetCopyMathNet() to create a new copy each time.</summary>
        protected internal virtual Vector_MathNet CopyMathNet
        {
            get
            {
                if (!IsCopyMathNetConsistent)
                {
                    _copyMathNet = new Vector_MathNet(_length);
                    for (int i = 0; i < _length; ++i)
                        _copyMathNet[i] = this[i];
                }
                return _copyMathNet;
            }
        }

        /// <summary>Creates and returns a newly allocated MathNet representation of the current vector.</summary>
        protected internal Vector_MathNet GetCopyMathNet()
        {
            Vector_MathNet ret = new Vector_MathNet(_length);
            for (int i = 0; i < _length; ++i)
                ret[i] = this[i];
            return ret;
        }

        #endregion MathNetSupport


    }  // VectorOld


    [Obsolete("Old class, use IG.Num.Matrix instead.")]
    public class MatrixWithMathNet : Matrix
    {

        
        #region Construction

        protected MatrixWithMathNet()
            : base()
        { }


        /// <summary>Constructs a matrix from another matrix by copying the provided matrix components 
        /// to the internal data structure.</summary>
        /// <param name="A">Matrix whose components are copied to the current matrix.</param>
        public MatrixWithMathNet(IMatrix A)
            : base(A)
        { }

        /// <summary>Construct a matrix from MathNet.Numerics.LinearAlgebra.Matrix.
        /// Only a reference of A is copied.</summary>
        /// <param name="A">MathNet.Numerics.LinearAlgebra.Matrix from which a new matrix is created.</param>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        protected MatrixWithMathNet(Matrix_MathNet A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to copy new matrix components from is not specified (null reference).");
            if (A.RowCount <= 0)
                throw new ArgumentException("Matrix to create a new matrix from has 0 rows.");
            if (A.ColumnCount <= 0)
                throw new ArgumentException("Matrix to create a new matrix from has 0 columns.");
            // Copy the array to a jagged array:
            int numRows = _rowCount = A.RowCount;
            int numColumns = _columnCount = A.ColumnCount;
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i)
            {
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = A[i, j];
            }
        }

        /// <summary>Construct a matrix from MathNet.Numerics.LinearAlgebra.Matrix.
        /// Only a reference of A is copied.</summary>
        /// <param name="A">MathNet.Numerics.LinearAlgebra.Matrix from which a new matrix is created.</param>
        public MatrixWithMathNet(MatrixBase_MathNetNumerics A)
            : base(A)
        { }

        /// <summary>Constructs an d1*d2 - dimensional matrix of zeros.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d1">Number of columns.</param>
        public MatrixWithMathNet(int d1, int d2)
            : base(d1, d2)
        { }

        /// <summary> Construct an numrows-by-d2 constant matrix with specified value for all elements.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <param name="value">Value of all components.</param>
        public MatrixWithMathNet(int d1, int d2, double val)
            : base(d1, d2, val)
        { }

        /// <summary>Constructs a square matrix with specified diagonal values.</summary>
        /// <param name="d">Size of the square matrix.</param>
        /// <param name="val">Vector of diagonal values.</param>
        public MatrixWithMathNet(IVector diagonal)
            : base(diagonal)
        { }

        /// <summary>Constructs a d*d square matrix with specified diagonal value.</summary>
        /// <param name="dim">Size of the square matrix.</param>
        /// <param name="elementValue">Diagonal value.</param>
        public MatrixWithMathNet(int dim, double elementValue)
            : base(dim, elementValue)
        { }

        /// <summary>Constructs a matrix from a jagged 2-D array, directly using the provided array as 
        /// internal data structure.</summary>
        /// <param name="A">Two-dimensional jagged array of doubles.</param>
        /// <exception cref="System.ArgumentException">All rows must have the same length.</exception>
        /// <seealso cref="Matrix.Create(double[][])"/>
        /// <seealso cref="Matrix.Create(double[,])"/>
        public MatrixWithMathNet(double[][] A)
            : base(A)
        { }

        /// <summary>Constructs a matrix from a 2-D array by deep-copying the provided array 
        /// to the internal data structure.</summary>
        /// <param name="elementTable">Two-dimensional array of doubles.</param>
        public MatrixWithMathNet(double[,] elementTable)
            : base(elementTable)
        { }

        /// <summary>Construct a matrix from a one-dimensional packed array.</summary>
        /// <param name="_matrixElements">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
        /// <param name="numRows">Number of rows.</param>
        /// <exception cref="System.ArgumentException">Array length must be a multiple of numrows.</exception>
        [Obsolete("This method may be unsupported in future versions.")]
        public MatrixWithMathNet(double[] _matrixElements, int numRows)
            : base(_matrixElements, numRows)
        { }


        #region StaticConstruction



        /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static new MatrixWithMathNet Create(double[][] A)
        {
            return new MatrixWithMathNet(A);
        }

        /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static new MatrixWithMathNet Create(double[,] A)
        {
            return new MatrixWithMathNet(A);
        }

        /// <summary>Construct a complex matrix from a set of real column vectors.</summary>
        public static new MatrixWithMathNet CreateFromColumns(IList<Vector> columnVectors)
        {
            if (columnVectors == null)
                throw new ArgumentNullException("List of column vectors to create a matrix from is not specified (null reference).");
            if (columnVectors.Count == 0)
                throw new ArgumentException("List of column vectors to create a matrix from does not contain any vectors.");
            int rows = columnVectors[0].Length;
            int columns = columnVectors.Count;
            double[][] newData = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                double[] newRow = new double[columns];
                for (int j = 0; j < columns; j++)
                {
                    newRow[j] = columnVectors[j][i];
                }

                newData[i] = newRow;
            }
            return new MatrixWithMathNet(newData);
        }

        /// <summary>Construct a complex matrix from a set of real row vectors.</summary>
        public static new MatrixWithMathNet CreateFromRows(IList<Vector> rowVectors)
        {
            if (rowVectors == null)
                throw new ArgumentNullException("List of row vectors to create a matrix from is not specified (null reference).");
            if (rowVectors.Count == 0)
                throw new ArgumentException("List of row vectors to create a matrix from does not contain any vectors.");
            int rows = rowVectors.Count;
            int columns = rowVectors[0].Length;
            double[][] newData = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                newData[i] = rowVectors[i].ToArray();
            }
            return new MatrixWithMathNet(newData);
        }


        // TODO: add a couple of other creations from lists of doubles, from submatirices, etc.!



        /// <summary>Creates a d1*d2 identity matrix.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>An d1*d2 matrix with ones on the diagonal and zeros elsewhere.</returns>
        public static new MatrixWithMathNet Identity(int d1, int d2)
        {
            double[][] data = new double[d1][];
            for (int i = 0; i < d1; i++)
            {
                double[] col = new double[d2];
                if (i < d2)
                {
                    col[i] = 1.0;
                }

                data[i] = col;
            }
            return new MatrixWithMathNet(data);
        }

        /// <summary>Creates a square identity matrix of dimension d*d.</summary>
        /// <param name="d">Matrix dimension.</param>
        /// <returns>A d*d identity matrix.</returns>
        public static new MatrixWithMathNet Identity(int d)
        {
            return MatrixWithMathNet.Identity(d, d);
        }

        /// <summary>Creates a d1*d2 matrix filled with 0.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        public static new Matrix Zeros(int d1, int d2)
        {
            return new MatrixWithMathNet(d1, d2, 0.0);
        }

        /// <summary>creates a square d*d matrix filled with 0.</summary>
        /// <param name="d">Number of rows and columns.</param>
        public static new Matrix Zeros(int d)
        {
            return new MatrixWithMathNet(d, d, 0.0);
        }

        /// <summary>Creates a d1*d2 matrix filled with 1.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        public static new MatrixWithMathNet Ones(int d1, int d2)
        {
            return new MatrixWithMathNet(d1, d2, 1.0);
        }

        /// <summary>Generates a square d*d matrix filled with 1.</summary>
        /// <param name="d1">Number of rows and columns.</param>
        public static new MatrixWithMathNet Ones(int d)
        {
            return new MatrixWithMathNet(d, d, 1.0);
        }

        /// <summary>Creates a new diagonal d1*d2 matrix based on the diagonal vector.</summary>
        /// <param name="diagonalVector">The values of the matrix diagonal.</param>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>A d1*d2 matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
        public static new MatrixWithMathNet Diagonal(IVector<double> diagonalVector, int d1, int d2)
        {
            if (diagonalVector == null)
                throw new ArgumentNullException("Vector to create a diagonal matrix from is not specified (nul reference).");
            if (diagonalVector.Length==0)
                throw new ArgumentException("Vector to create a diagonal matrix from has 0 elements.");
            if (d1 < diagonalVector.Length || d2 < diagonalVector.Length)
                throw new ArgumentException("Vector to create a diagonal matrix from is too large.");
            if (d1>diagonalVector.Length && d2>diagonalVector.Length)
                throw new ArgumentException("Vector to create a diagonal matrix from is too small.");
            double[][] data = new double[d1][];
            for (int i = 0; i < d1; i++)
            {
                double[] col = new double[d2];
                if ((i < d2) && (i < diagonalVector.Length))
                {
                    col[i] = diagonalVector[i];
                }
                data[i] = col;
            }
            return new MatrixWithMathNet(data);
        }

        /// <summary>Creates a new square diagonal matrix based on the diagonal vector.</summary>
        /// <param name="diagonalVector">The values of the matrix diagonal.</param>
        /// <returns>A square matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
        public static new MatrixWithMathNet Diagonal(IVector<double> diagonalVector)
        {
            return MatrixWithMathNet.Diagonal(diagonalVector, diagonalVector.Length, diagonalVector.Length);
        }


        ///// <summary>Creates a d1*d2 matrix with random elements.</summary>
        ///// <param name="d1">Number of rows.</param>
        ///// <param name="d2">Number of columns.</param>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
        ///// <returns>An d1*d2 matrix with elements distributed according to the provided distribution.</returns>
        //public static Matrix Random(int d1, int d2, IContinuousGenerator randomDistribution)
        //{
        //    double[][] data = new double[d1][];
        //    for (int i = 0; i < d1; i++)
        //    {
        //        double[] col = new double[d2];
        //        for (int j = 0; j < d2; j++)
        //        {
        //            col[j] = randomDistribution.NextDouble();
        //        }

        //        data[i] = col;
        //    }
        //    return new Matrix(data);
        //}

        ///// <summary>Creates a d*d square matrix with random elements.</summary>
        ///// <param name="d">Number of rows and columns.</param>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
        ///// <returns>An d*d matrix with elements distributed according to the provided distribution.</returns>
        //public static Matrix Random(int d, IContinuousGenerator randomDistribution)
        //{
        //    return Random(d, d, randomDistribution);
        //}

        /// <summary>Generates a d1*d2 matrix with uniformly distributed random elements.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>A d1*d2 matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static new MatrixWithMathNet Random(int d1, int d2)
        {
            MatrixWithMathNet ret = new MatrixWithMathNet(d1, d2);
            Matrix.SetRandom(ret);
            return ret;
        }

        /// <summary>Creates and returns a d1*d2 matrix with uniformly distributed random elements.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>A d1*d2 matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static new MatrixWithMathNet Random(int d1, int d2, IRandomGenerator rnd)
        {
            MatrixWithMathNet ret = new MatrixWithMathNet(d1, d2);
            Matrix.SetRandom(ret, rnd);
            return ret;
        }

        /// <summary>Generates a d*d square matrix with standard-distributed random elements.</summary>
        /// <param name="d">Number of rows and columns.</param>
        /// <returns>A d*d square matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static new MatrixWithMathNet Random(int d)
        {
            return Random(d, d);
        }

        #endregion StaticConstruction


        #endregion  // Construction


        #region MathNetSupport

        protected Matrix_MathNet _copyMathNet = null;

        protected bool _mathNetConsistent = false;

        /// <summary>Tells whether the internal MathNet matrix representation of the current matrix is 
        /// consistent with the current matrix. The MathNet representation is used for operations that
        /// are used from that library such as different kinds of decompositions.</summary>
        /// <remarks>Currrently, an internal flag indicating consistency of the MathNet matrix is not used.
        /// Every time this property is required, the consistence is actually verified by comparing values.
        /// There may be derived matrix classes where the falg is actually used. These must keep track
        /// when anything in the matrix changes and invalidate the flag on each such event.</remarks>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        protected internal virtual bool IsCopyMathNetConsistent
        {
            get
            {
                if (_copyMathNet == null)
                    return false;
                else if (_copyMathNet.RowCount != _rowCount || _copyMathNet.ColumnCount != ColumnCount)
                    return false;
                else
                {
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _columnCount; ++j)
                        {
                            if (_copyMathNet[i, j] != _elements[i][j])
                                return false;
                        }
                }
                return true;
            }
            protected set
            {
                _mathNetConsistent = value;
            }
        }

        /// <summary>Copies components from a specified Math.Net matrix.</summary>
        /// <param name="A">Matrix from which elements are copied.</param>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        protected void CopyFromMatNetMatrix(Matrix_MathNet A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to copy components from is not specified (null reference).");
            if (A.RowCount == 0)
                throw new ArgumentException("Matrix to copy components from has 0 rows.");
            if (A.ColumnCount == 0)
                throw new ArgumentException("Matrix to copy components from has 0 columns.");
            // Copy the array to a jagged array:
            int numRows = _rowCount = A.RowCount;
            int numColumns = _columnCount = A.ColumnCount;
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i)
            {
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = A[i, j];
            }
        }

        /// <summary>Gets the internal MathNet representation of the current matrix.
        /// Representation is created on demand. However, the same copy is returned
        /// as long as it is consistent with the current matrix.
        /// Use GetCopyMathNet() to create a new copy each time.</summary>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        public virtual Matrix_MathNet CopyMathNet
        {
            get
            {
                if (!IsCopyMathNetConsistent)
                {
                    _copyMathNet = new Matrix_MathNet(_rowCount, _columnCount);
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _columnCount; ++j)
                            _copyMathNet[i, j] = this[i, j];
                }
                return _copyMathNet;
            }
        }

        /// <summary>Creates and returns a newly allocated MathNet representation of the current matrix.</summary>
        [Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        protected internal Matrix_MathNet GetCopyMathNet()
        {
            int numRows = this.RowCount;
            int numColumns = this.ColumnCount;
            Matrix_MathNet ret = new Matrix_MathNet(numRows, numColumns);
            for (int i = 0; i < numRows; ++i)
                for (int j = 0; j < numColumns; ++j)
                {
                    ret[i, j] = this[i, j];
                }
            return ret;
        }

        #endregion MathNetSupport


    }  // MAtrixOld


    /// <summary>Class that is directly derived from MathNet.Numerics.LinearAlgebra.Vector</summary>
    [Obsolete("Old class.")]
    public class XVector : MathNet.Numerics.LinearAlgebra.Vector, IVector_OldNumerics<double>
    {

        // Constructors copied from base class:

        #region Construction



        /// <summary>Constructs an d2-dimensional vector of zeros.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        public XVector(int n)
            : base(n)
        {
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimensionality less thatn 1. Provided dimensionality: "
                    + n.ToString() + ".");
        }

        /// <summary>Constructs an d2-dimensional unit vector for i'th coordinate.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <param name="i">Coordinate index.</param>
        public XVector(int n, int i)
            : base(n, i)
        {
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimensionality less thatn 1. Provided dimensionality: "
                    + n.ToString() + ".");
            if (i < 0 || i >= n - 1)
                throw new ArgumentException("Creation of unit vector: index out of range: "
                    + i.ToString() + ", should be between 0 and " + (n - 1).ToString() + ".");
        }

        /// <summary>Constructs an d2-dimensional constant vector.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <param name="value">Fill the vector with this scalar value.</param>
        public XVector(int n, double value)
            : base(n, value)
        {
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimensionality less thatn 1. Provided dimensionality: "
                    + n.ToString() + ".");
        }


        // TODO: check how constructor with array arguments copies vec! If it just copies the reference then 
        // consider whethe this should be re-implemented!

        /// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        /// <param name="vec">One-dimensional array of doubles.</param>
        /// <seealso cref="Create"/>
        public XVector(double[] components)
            : base(components)
        {
            if (components == null)
                throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
            if (components.Length <= 0)
                throw new ArgumentException("Vector creation: array of components contains no values, can not create a vector with dimensionality 0.");
        }


        // Static construction methods:


        /// <summary>Constructs a vector from a copy of a 1-D array.</summary>
        public new static XVector Create(double[] components)
        {
            if (components == null)
                throw new ArgumentNullException("Array of componnets not specified (null reference).");
            int length = components.Length;
            if (length <= 0)
                throw new ArgumentException("Vector creation: array of components contains no values, can not create a vector with dimensionality 0.");
            XVector ret = new XVector(length);
            for (int i = 0; i < length; ++i)
                ret[i] = components[i];
            return ret;
        }

        /// <summary>Generates vector with random elements.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <param name="randomDistribution">Continuous Random Distribution or Source</param>
        /// <returns>An d2-dimensional vector with random elements distributed according
        /// to the specified random distribution.</returns>
        public new static XVector Random(int n, IContinuousGenerator randomDistribution)
        {
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimensionality less thatn 1. Provided dimensionality: "
                    + n.ToString() + ".");
            XVector ret = new XVector(n);
            for (int i = 0; i < n; i++)
            {
                ret[i] = randomDistribution.NextDouble();
            }
            return ret;
        }

        /// <summary>Generates vector with random elements uniformly distributed on [0, 1).</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <returns>An d2-dimensional vector with uniformly distributedrandom elements in <c>[0, 1)</c> interval.</returns>
        public new static XVector Random(int n)
        {
            return Random(n, new SystemRandomSource());
        }

        /// <summary>Generates an d2-dimensional vector filled with 1.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        public static new XVector Ones(int n)
        {
            return new XVector(n, 1.0);
        }

        /// <summary>Generates an d2-dimensional vector filled with 0.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        public new static XVector Zeros(int n)
        {
            return new XVector(n, 0.0);
        }

        /// <summary>Generates an d2-dimensional unit vector for i-th coordinate.</summary>
        /// <param name="d2">Dimensionality of vector.</param>
        /// <param name="i">Coordinate index.</param>
        public new static XVector BasisVector(int n, int i)
        {
            return new XVector(n, i);
        }

        #endregion  // Construction


        /// <summary>Returns a string representation of this vector in a standard IGLib form.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int d = this.Length - 1;
            sb.Append('{');
            for (int i = 0; i <= d; ++i)
            {
                sb.Append(this[i].ToString(null, CultureInfo.InvariantCulture));
                if (i < d)
                    sb.Append(", ");
            }
            sb.Append("}");
            return sb.ToString();
        }


        #region Helpers_Infrastructure


        #endregion




        #region operators


        // || v1.Length!=result.Length

        public void Resize(int newsize)
        {
        }

        /// <summary>Raw vector addition.</summary>
        /// <param name="v1">Left summand.</param>
        /// <param name="v2">Right Summand.</param>
        /// <param name="result">Result.</param>
        protected static void AddRaw(MathNet.Numerics.LinearAlgebra.Vector v1, MathNet.Numerics.LinearAlgebra.Vector v2,
                MathNet.Numerics.LinearAlgebra.Vector result)
        {
            if (v1 == null || v2 == null || result == null)
                throw new ArgumentNullException("One of the operands is not specified.");
            if (v1.Length != v2.Length)
                throw new ArgumentNullException("Insonsistent dimensions.");
            //if (result.Length != v1.Length)
            //    result.Resize(v1.Length);
            for (int i = 0; i < v1.Length; ++i)
                result[i] = v1[i] + v2[i];
        }

        /// <summary>In-place vector addition.</summary>
        public void AddInplace(XVector v2)
        {
            AddRaw(this, v2, this);
        }

        /// <summary>In-place vector addition.</summary>
        public XVector Add(XVector v2)
        {
            XVector result = new XVector(this.Length);
            AddRaw(this, v2, result);
            return result;
        }

        ///// <summary>Addition Operator.</summary>
        //public static XVector operator +(XVector u, XVector v)
        //{
        //    XVector result = new XVector(u.Length);
        //    AddRaw(u, v, result);
        //    return result;
        //}


        ///// <summary>Addition Operator.</summary>
        //public static XVector operator +(XVector u, XVector v)
        //{
        //    XVector result = new XVector();
        //    AddRaw(u, v, result);
        //    return result;
        //}




        #endregion operators







        public IVector_OldNumerics<double> GetCopy()
        {
            XVector ret = null;
            ret = new XVector(this.Length);
            for (int i = 0; i < this.Length; ++i)
                ret[i] = this[i];
            return ret;
        }


        public double FirstComponent()
        {
            return this[0];
        }

        public static void Examples()
        {
            XVector a, b;
            a = XVector.Ones(5);

            Console.WriteLine("First component of a: " + a.FirstComponent().ToString());

            a = new XVector(new double[] { 1.1, 1.2, 1.3 });
            b = new XVector(new double[] { 2.1, 2.2, 2.3 });

            Console.WriteLine("a = " + a.ToString());
            Console.WriteLine("b = " + b.ToString());
            Console.WriteLine("a+b = " + (a + b).ToString());

        }



    }  // class XVector


}

