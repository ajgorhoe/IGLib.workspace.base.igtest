﻿using System;


namespace Meta.Numerics.Matrices {

    /// <summary>
    /// Stores the singular value decomposition of a matrix.
    /// </summary>
    /// <remarks>
    /// <para>The singular value decomposition of a matrix represents it as a product of a left orthogonal matrix, a diagonal
    /// matrix, and a right orthogonal matrix:</para>
    /// <img src="../images/SVDForm.png" />
    /// <para>The elements of the diagonal matrix are called the singular values of the original matrix.</para>
    /// <para>If the orginal matrix is viewed as a linear transform operator, the rows of the right orthogonal matrix matrix form an
    /// orthonormal basis for the domain of the operator, while the columns of the left orthogonal matrix form an orthonormal
    /// basis for the range of the operator. These rows and columns are called, respectively, the right and left singular vectors
    /// of the matrix.</para>
    /// <para>The right singular vectors corresponding to zero singular values span the nullspace of the matrix, that is the
    /// set of all x for which Ax = 0.</para>
    /// <para>The SVD can be used to approximate the action of a high-dimensional matrix by a lower-rank one.</para>
    /// <para>Use the <see cref="RectangularMatrix.SingularValueDecomposition"/> of the <see cref="RectangularMatrix"/> class
    /// to obtain the SVD of an rectangular matrix, or the corresponding <see cref="SquareMatrix.SingularValueDecomposition"/>
    /// method of the <see cref="SquareMatrix"/> class to obtain the SVD of a square matrix.</para>
    /// </remarks>
    public sealed class SingularValueDecomposition {

        internal SingularValueDecomposition (double[] utStore, double[] wStore, double[] vStore, int rows, int cols) {
            this.utStore = utStore;
            this.vStore = vStore;
            this.wStore = wStore;
            this.rows = rows;
            this.cols = cols;
        }

        private readonly int rows, cols;
        private readonly double[] utStore;
        private readonly double[] wStore;
        private readonly double[] vStore;

        /// <summary>
        /// Gets the number of rows in the original matrix.
        /// </summary>
        public int RowCount {
            get {
                return (rows);
            }
        }

        /// <summary>
        /// Gets the number of columns in the original matrix.
        /// </summary>
        public int ColumnCount {
            get {
                return (cols);
            }
        }

        /// <summary>
        /// Returns the left transform matrix.
        /// </summary>
        /// <returns>The matrix U, such that A = U S V<sup>T</sup>.</returns>
        public SquareMatrix LeftTransformMatrix () {
            double[] left = MatrixAlgorithms.Transpose(utStore, rows, rows);
            return (new SquareMatrix(left, rows));
        }

        /// <summary>
        /// Returns the right transform matrix.
        /// </summary>
        /// <returns>The matrix V, such that A = U S V<sup>T</sup>.</returns>
        /// <remarks>
        /// <para>The returned matrix is read-only. If you need to make changes to it, you can call <see cref="SquareMatrix.Copy"/> to obtain a writable copy.</para>
        /// </remarks>
        public SquareMatrix RightTransformMatrix () {
            return (new SquareMatrix(vStore, cols, true));
        }

        /// <summary>
        /// Gets the number of singular values.
        /// </summary>
        /// <remarks>
        /// <para>For a square matrix, the number of singular values is equal to the dimension of the matrix.
        /// For a rectangular matrix with more rows than columns, the number of singular values is equal to
        /// the number of columns.</para>
        /// </remarks>
        public int Dimension {
            get {
                return (wStore.Length);
            }
        }

        /// <summary>
        /// Gets the specificed singular value.
        /// </summary>
        /// <param name="n">The (zero-based) index.</param>
        /// <returns>The <paramref name="n"/>th singular value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="n"/> lies outside the range [0, <see cref="Dimension"/> - 1].</exception>
        public double SingularValue (int n) {
            if ((n < 0) || (n >= wStore.Length)) throw new ArgumentOutOfRangeException("n");
            return (wStore[n]);
        }

        /// <summary>
        /// Returns the specified left singular vector.
        /// </summary>
        /// <param name="n">The (zero-based) index.</param>
        /// <returns>The <paramref name="n"/>th left singular vector.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="n"/> lies outside the range [0, <see cref="Dimension"/> - 1].</exception>
        /// <remarks>
        /// <para>The returned vector is read-only. If you need to make changes to it, you can call <see cref="ColumnVector.Copy"/> to obtain a writable copy.</para>
        /// </remarks>
        public ColumnVector LeftSingularVector (int n) {
            if ((n < 0) || (n >= wStore.Length)) throw new ArgumentOutOfRangeException("n");
            return (new ColumnVector(utStore, n, rows, rows, true));
        }

        /// <summary>
        /// Returns the specified right singular vector.
        /// </summary>
        /// <param name="n">The (zero-based) index.</param>
        /// <returns>The <paramref name="n"/>th right singular vector.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="n"/> lies outside the range [0, <see cref="Dimension"/> - 1].</exception>
        /// <remarks>
        /// <para>The returned vector is read-only. If you need to make changes to it, you can call <see cref="ColumnVector.Copy"/> to obtain a writable copy.</para>
        /// </remarks> 
        public ColumnVector RightSingularVector (int n) {
            if ((n < 0) || (n >= wStore.Length)) throw new ArgumentOutOfRangeException("n");
            return (new ColumnVector(vStore, n * cols, 1, cols, true));
        }

        /// <summary>
        /// Returns the condition number of the matrix.
        /// </summary>
        /// <remarks>
        /// <para>The conidition number is the ratio of the largest singular value to smallest singular value. It is therefore always larger than one.</para>
        /// </remarks>
        public double ConditionNumber {
            get {
                return (wStore[0] / wStore[wStore.Length - 1]);
            }
        }

        private double tolerance = Global.Accuracy;

        /// <summary>
        /// Gets or sets the tolerance with which singular values are distinguished from zero.
        /// </summary>
        /// <remarks>
        /// <para>Some operations offered by singular value decompositions, including rank determination and computation of the pseudo-inverse matrix,
        /// depend on determining whether or not a singular value is zero. Since floating point numbers are only approximate representations of real
        /// numbers, singular values will usually not be exactly zero even for matrices for which they should be, but will instead be very tiny
        /// numbers, on the order of floating point precision (about 10<sup>-16</sup>) as a fraction of the largest singular value. The value
        /// of this property is used to determine how small a singular value must be, as a fraction of the largest singular value, to be considered
        /// zero for these purposes. Usually you will want to maintain its default value.</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value is outside the range [0,1).</exception>
        public double Tolerance {
            get {
                return (tolerance);
            }
            set {
                if ((value < 0.0) || (value >= 1.0)) throw new ArgumentOutOfRangeException("value");
                tolerance = value;
            }
        }

        /// <summary>
        /// Computes the rank of the original matrix.
        /// </summary>
        /// <remarks>
        /// <para>The rank of a matrix is the dimension of the space of input vectors which produce non-zero output vectors upon multiplication by the original matrix.</para>
        /// <para>Since this operation depends on identifying zero singular values, the result will depend on the value of the <see cref="Tolerance"/> property.</para>
        /// </remarks>
        public int Rank {
            get {
                double minValue = tolerance * wStore[0];
                int rank = 0;
                for (int i = 0; i < wStore.Length; i++) {
                    if (wStore[i] > minValue) {
                        rank++;
                    } else {
                        break;
                    }
                }
                return (rank);
            }
        }
    }

}
