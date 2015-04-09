// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

// MATRIX DECOMPOSITIONS; This module is based on the MathNet external library.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using IG.Lib;
using IG.Num;

using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;
using LUDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.LUDecomposition;
using QRDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.QRDecomposition;

//using Matrix_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;
//using MatrixBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Generic.Matrix<double>;
//using Vector_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseVector;
//using VectorBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Generic.Vector<double>;
//using VectorComplexBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Generic.Vector<System.Numerics.Complex>;
//using LUDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseLU;
//using QRDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseQR;
//using EigenValueDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseEvd;
//using SingularValueDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseSvd;

//using MathNet.Numerics.LinearAlgebra.Double;
//using QRDecomposition_MathNetNumericx = MathNet.Numerics.LinearAlgebra.Double.Factorization;


namespace IG.Old
{


    #region Old_MathNet

    /// <summary>LU decomposition.</summary>
    /// <remarks>
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    /// unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    /// and a permutation vector pivot of length m so that A(piv,:) = L*U.
    /// <c> If m &lt; n, then L is m-by-m and U is m-by-n. </c>
    /// The LU decomposition with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    /// LU decomposition is in the solution of square systems of simultaneous
    /// linear equations.  This will fail if IsNonSingular() returns false.
    /// </remarks>
    [Serializable]
    public class LUDecompositionOld
    {
        protected internal int
            _rowCount = 0,  // number of rows of a decomposed matrix
            _columnCount = 0;  // number of columns of a decomposed matrix

        protected internal LUDecomposition_MathNet Base = null;

        #region Construction

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public LUDecompositionOld(MatrixWithMathNet A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = new LUDecomposition_MathNet(A.CopyMathNet);
        }

        #endregion Construction


        /// <summary>Indicates whether the matrix is nonsingular.</summary>
        /// <returns><c>true</c> if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.IsNonSingular;
            }
        }

        /// <summary>Returns the lower triangular factor.</summary>
        public Matrix L
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.L);
            }
        }

        /// <summary>Returns the upper triangular factor.</summary>
        public Matrix U
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.U);
            }
        }

        /// <summary>
        /// Returns the integer pivot permutation vector.
        /// </summary>
        public int[] Pivot
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.Pivot;
            }
        }

        /// <summary>Returns pivot permutation vector.</summary>
        public Vector PivotVector
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Vector(Base.PivotVector);
            }
        }

        /// <summary>Returns the permutation matrix P, such that L*U = P*X.</summary>
        public Matrix PermutationMatrix
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.PermutationMatrix);
            }
        }

        /// <summary>Returns the determinant.</summary>
        /// <returns>det(A)</returns>
        /// <exception cref="System.ArgumentException">Matrix must be square</exception>
        public double
        Determinant
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.Determinant();
            }
        }

        /// <summary>Solve A*X = B.</summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        /// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException">Matrix is singular.</exception>
        public MatrixWithMathNet Solve(MatrixWithMathNet B)
        {
            if (Base == null)
                throw new Exception("Invalid data.");
            if (B == null)
                throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
            if (B.CopyMathNet == null)
                throw new ArgumentException("Matrix of right-hand sides: invalid data.");
            return new MatrixWithMathNet(Base.Solve(B.CopyMathNet));
        }

        /// <summary>Solves A*x=s.</summary>
        /// <param name="s">Right-hand side vector with as many elements as A has rows.</param>
        /// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        public Vector Solve(Vector b)
        {
            if (b == null)
                throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
            if (b.Length != _rowCount)
                throw new Exception("Inconsistent dimension of the right-hand side vector.");
            MatrixWithMathNet B = new MatrixWithMathNet(b.Length, 1);
            MatrixWithMathNet X = Solve(B);
            Vector ret = new Vector(X.RowCount);
            for (int i = 0; i < X.RowCount; ++i)
                ret[i] = X[i, 0];
            return ret;
        }

        // TODO: Add solution of equations where right-hand side is a vector or a list of vectors!

        // TODO: Solution of system of equations where result storage is provided!

    }  // class LUDecomposition



    [Serializable]
    public class QRDecompositionOld
    {

        protected internal QRDecomposition_MathNet Base = null;

        protected internal int
            _rowCount = 0,  // number of rows of a decomposed matrix
            _columnCount = 0;  // number of columns of a decomposed matrix

        /// <summary>Constructs QR Decomposition, computed by Householder reflections.</summary>
        /// <remarks>Provides access to R, the Householder vectors and computes Q.</remarks>
        /// <param name="A">Rectangular matrixwhose decomposition is computed and stored.</param>
        public QRDecompositionOld(MatrixWithMathNet A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = new QRDecomposition_MathNet(A.CopyMathNet);
        }


        /// <summary>Indicates whether the matrix is full rank.</summary>
        /// <returns><c>true</c> if R, and hence A, has full rank.</returns>
        public bool IsFullRank
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.IsFullRank;
            }
        }

        /// <summary>Gets the Householder vectors.</summary>
        /// <returns>Lower trapezoidal matrix whose columns define the reflections.</returns>
        public Matrix H
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.H);
            }
        }

        /// <summary>Gets the upper triangular factor.</summary>
        public Matrix R
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.R);
            }
        }

        /// <summary>Gets the (economy-sized) orthogonal factor.</summary>
        public Matrix Q
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.Q);
            }
        }

        /// <summary>Least squares solution of A*X = B.</summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns.</param>
        /// <returns>X that minimizes the two norm of Q*R*X-B.</returns>
        /// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException">Matrix is rank deficient.</exception>
        public MatrixWithMathNet Solve(MatrixWithMathNet B)
        {

            if (Base == null)
                throw new Exception("Invalid data.");
            if (B == null)
                throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
            if (B.CopyMathNet == null)
                throw new ArgumentException("Matrix of right-hand sides: invalid data.");
            return new MatrixWithMathNet(Base.Solve(B.CopyMathNet));
        }

        /// <summary>Least squares solution of A*x=s.</summary>
        /// <param name="s">Right-hand side vector with as many elements as decomposed matrix A has rows.</param>
        /// <returns>x that minimizes the two norm of Q*R*x-s.</returns>
        public Vector Solve(VectorWithMathNet b)
        {
            if (b == null)
                throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
            if (b.Length != _rowCount)
                throw new Exception("Inconsistent dimension of the right-hand side vector.");
            MatrixWithMathNet B = new MatrixWithMathNet(b.Length, 1);
            MatrixWithMathNet X = Solve(B);
            Vector ret = new Vector(X.RowCount);
            for (int i = 0; i < X.RowCount; ++i)
                ret[i] = X[i, 0];
            return ret;
        }

    }  // class QRDecomposition

    #endregion Old_MathNet



}
