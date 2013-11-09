// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    /***********************/
    /*                     */
    /*  MATRIX OPERATIONS  */
    /*                     */
    /***********************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

using F = IG.Num.M;

namespace IG.Num
{


    /// <summary>Development of matrix operations.</summary>
    /// $A Igor May09;
    public abstract class MatrixBaseDev: MatrixBase, IMatrix
    {


        /// <summary>Attemps to create and return a symmetric positive definite square matrix.</summary>
        /// <param name="dim">Dimention of the matrix.</param>
        /// <param name="rnd">Random number generator.</param>
        public static IMatrix CreateRandomSymmetricPositiveDefinite(int dim, IRandomGenerator rnd)
        {
            double factorOutDiag = 1.0e-4/(double) (dim*dim);
            IMatrix aux = new Matrix(dim, dim);
            for (int i = 0; i < dim; ++i)
            {
                aux[i, i] = (double) i;
                for (int j = 0; j < i; ++j)
                    aux[i, j] = aux[j, i] = rnd.NextDouble() * factorOutDiag;
            }
            IMatrix ret = new Matrix(dim, dim);
            Matrix.MultiplyTranspMat(aux, aux, ref ret);
            Matrix.SymmetricPartPlain(ret, ret);
            return ret;
        }


        /// <summary>Attemps to create and return a symmetric positive definite square matrix.</summary>
        /// <param name="dim">Dimention of the matrix.</param>
        /// <param name="rnd">Random number generator.</param>
        public static IMatrix CreateRandomSymmetricPositiveDefinite(int dim)
        {
            IRandomGenerator rnd = new RandGeneratorThreadSafe(1111);
            return CreateRandomSymmetricPositiveDefinite(dim, rnd);
        }




        /********************************/
        /*                              */
        /*  DECOMPOSITIONS OF MATRICES  */
        /*                              */
        /********************************/



        #region DecompositionLLT


        /// LLT (Choleski) DECOMPOSITION (symmetric positive definite matrices) 

        // TODO: check the validity of this!


        /// <summary>Performs Choleski (LLT) decomposition of a matrix and stores result to the specified matrix.
        /// Can be done in place.
        /// WARNING:
        /// Dimensions are not checked, therefore dimensions of arguments must be consistent.
        /// Method also does not control whethe matrix is symmetric, but it simply operates on the lower triangle of the original.</summary>
        /// <param name="m1">Matrix whose Choleski decomposition is performed. It must be symmetric and positive definite
        /// in order for operation to succeed.</param>
        /// <param name="res">Resulting matrix where the decomposed matrix is stored. Can be the same matrix as m1.
        /// Only lower triangle is stored, and elements above diagonal are 0. Diagonal elements are positive.</param>
        /// <returns>0 if the decomposition has been successfully performed (i.e. the matrix is positive definite).
        /// A negative number otherwise:
        ///   minus row number (1-based) if the calculated diagonal element in this row is zero.</returns>
        ///   <remarks>Transcribed from IOptLib C library.</remarks>
        ///   $A Igor May09;
        public static int DecomposeLLTPlain(IMatrix m1, IMatrix res)
        {
            return DecomposeLLTPlain(m1, res, 0.0 /* smalltol */ );
        }


        /// <summary>Performs Choleski (LLT) decomposition of a matrix and stores result to the specified matrix.
        /// Can be done in place.
        /// WARNING:
        /// Dimensions are not checked, therefore dimensions of arguments must be consistent.
        /// Method also does not control whethe matrix is symmetric, but it simply operates on the lower triangle of the original.</summary>
        /// <param name="m1">Matrix whose Choleski decomposition is performed. It must be symmetric and positive definite
        /// in order for operation to succeed.</param>
        /// <param name="res">Resulting matrix where the decomposed matrix is stored. Can be the same matrix as m1.
        /// Only lower triangle is stored, and elements above diagonal are 0. Diagonal elements are positive.</param>
        /// <param name="smalltol">Tolerance for size of diagonal element and ratio between minimal and maximal diagonal element.
        /// Can be 0. If the mentioned values are less or equal to the tolerance then it is considered that decomposition can not
        /// be performed.</param>
        /// <returns>0 if the decomposition has been successfully performed (i.e. the matrix is positive definite).
        /// A negative number otherwise:
        ///   minus row number (1-based) if the calculated diagonal element in this row is zero (within tolerance).
        ///   minus number of rows minus 1 if the ratio between minimal and maximal diagonal element is below the tolerance.</returns>
        ///   <remarks>Transcribed from IOptLib C library.</remarks>
        ///   $A Igor May09;
        public static int DecomposeLLTPlain(IMatrix m1, IMatrix res, double smalltol)
        {
            int k, ret = 0;
            double sum_ii, mindiag = 0.0, maxdiag = 0.0;
            if (smalltol < 0)
                smalltol = -smalltol;
            for (int i = 0; i < m1.RowCount; ++i)
            {
                // Diagonal elements:
                sum_ii = m1[i,i];
                for (k = 0; k < i; ++k)
                    sum_ii -= res[i,k] * res[i,k];
                if (sum_ii <= smalltol)
                {
                    Console.WriteLine("LLT: l[{0},{1}]<=0.", i, i);
                    return -(i+1);
                }
                // double sAbs = Math.Abs(s);
                double res_ii = Math.Sqrt(sum_ii);
                res[i, i] = res_ii;
                if (i == 1)
                    maxdiag = mindiag = res_ii;
                else
                {
                    if (res_ii > maxdiag)
                        maxdiag = res_ii;
                    else if (res_ii < mindiag)
                        mindiag = res_ii;
                }
                // Out of diagonal elements:
                for (int j = i + 1; j < m1.RowCount; ++j)
                {
                    double sum_ji = m1[j, i]; // m1[i,j];
                    for (k = 0; k < i; ++k)
                        sum_ji -= res[i, k] * res[j, k];
                    res[j, i] = sum_ji / res_ii;
                    res[i,j] = 0;
                }
            }
            if (mindiag / maxdiag <= smalltol)
            {
                // Console.WriteLine("LLT: (min. diag.)/(max.diag.) < {0}", smalltol);
                ret = -m1.RowCount - 1;
            }
            return ret;
        }


        public static int DecomposeLLT(IMatrix m1, IMatrix res, double smalltol)
        {
            if (m1 == null)
                throw new ArgumentNullException("Matrix not specified for LLT decomposition (null reference).");
            else if (m1.RowCount != m1.ColumnCount)
                throw new ArgumentException("Matrix for LLT decomposition is not square.");
            else if (res == null)
                throw new ArgumentNullException("Matrix to store results of LLT decomposition is not specified (null reference).");
            else if (res.RowCount != m1.RowCount || res.ColumnCount != m1.ColumnCount)
                throw new ArgumentException("Matrix to store results of LLT decomposition has wrong dimensions: "
                    + res.RowCount + "x" + res.ColumnCount + " instead of " + m1.RowCount + "x" + m1.ColumnCount + ".");
            return DecomposeLLTPlain(m1, res, smalltol);
        }

        public static int DecomposeLLT(IMatrix m1, ref IMatrix res, double smalltol)
        {
            if (m1 == null)
                throw new ArgumentNullException("Matrix not specified for LLT decomposition (null reference).");
            else if (m1.RowCount != m1.ColumnCount)
                throw new ArgumentException("Matrix for LLT decomposition is not square.");
            if (res == null)
                res = m1.GetNew();
            else if (res.RowCount != m1.RowCount || res.ColumnCount != m1.ColumnCount)
                res = m1.GetNew();
            return DecomposeLLTPlain(m1, res, smalltol);
        }

        /// <summary>Solves a linear system of equations by using a Choleski (LLT) decomposition of the system matrix.
        /// Linear system to be solved is L LT x = b, where L is a lower triangular matrix.
        /// Can be done in place, x and b can be the same vectors.
        /// WARNING:
        /// Dimensions are not checked, therefore dimensions of arguments must be consistent.</summary>
        /// <param name="L">Choleski decomposition of the matrix of the system.</param>
        /// <param name="b">Vector of right-hand sides.</param>
        /// <param name="x">Vector where solution is stored.</param>
        /// <remarks>The function is usually used for back-substitution after the Choleski
        /// (LLT) decomposition for positive definite matrices. A system of equations
        /// with a symmetric invertible matrix A can be converted to a system with a positive
        /// definite matrix by multiplying the matrix and the right-hand side vector
        /// by AT: A x = b => (AT A) x = (AT b). This is however not feasible because preparation of the system 
        /// takes much more time th is much better to perform the LDLT deocmposition.
        /// Transcribed from IOptLib C library.</remarks>
        ///   $A Igor May09;
        public static void SolveLLTPlain(IMatrix L, IVector b, IVector x)
        {
            int dim = L.ColumnCount;
            // Remark: The order of solution is L y = b, LT x = y 
            // Solve L y = b: 
            x[0] = b[0] / L[0,0];
            for (int i = 1; i < dim; ++i)
            {
                x[i] = b[i];
                for (int k = 0; k < i; ++k)
                    x[i] -= L[i,k] * x[k];
                x[i] /= L[i,i];
            }
            /* Solve LT x = y: */
            x[dim-1] /= L[dim-1,dim-1];
            for (int i = dim - 2; i >= 0; --i)
            {
                for (int k = i + 1; k < dim; ++k)
                    x[i] -= L[k,i] * x[k];   /* coef. of LT obtained from L (sym.) */
                x[i] /= L[i,i];
            }
        }

        public static void SolveLLT(IMatrix L, IVector b, IVector x)
        {
            if (L == null)
                throw new ArgumentNullException("Matrix LLT decomposition not specified (null reference).");
            else if (b == null)
                throw new ArgumentNullException("Right-hand side vector is not specified (null reference).");
            else if (b.Length != L.ColumnCount)
                throw new ArgumentException("Right-hand side vector has wrong dimension: "
                    + b.Length + " instead of " + L.ColumnCount);
            else if (x==null)
                throw new ArgumentNullException("Vector of variables is not specified (null reference).");
            else if (x.Length!=L.RowCount)
                throw new ArgumentException("Right-hand side vector has wrong dimension: "
                    + x.Length + " instead of " + L.RowCount);
            SolveLLTPlain(L, b, x);
        }

        public static void SolveLLT(IMatrix L, IVector b, ref IVector x)
        {
            if (L == null)
                throw new ArgumentNullException("Matrix LLT decomposition not specified (null reference).");
            else if (b == null)
                throw new ArgumentNullException("Right-hand side vector is not specified (null reference).");
            else if (b.Length != L.ColumnCount)
                throw new ArgumentException("Right-hand side vector has wrong dimension: "
                    + b.Length + " instead of " + L.ColumnCount);
            if (x == null)
                x = L.GetNewVector(L.RowCount);
            else if (x.Length != L.RowCount)
                x = L.GetNewVector(L.RowCount);
            SolveLLTPlain(L, b, x);
        }


        /// <summary>Splits the LLT product contained in a lower triangular part of a single matrix 
        /// into separate lower triangular and upper diagonal factors, and stores them in the specified matrices. 
        /// matrices.
        /// Both matrices where the split part is stored can be the same objects as the original matrix.
        /// WARNING: This method does not check consistency of matrix dimensons.</summary>
        /// <param name="LLT">A matrix containing the lower triangular Choleski product.</param>
        /// <param name="L">Matrix where the lower triangular factor is stored.</param>
        /// <param name="LT">Matrix where the upper triangular factor is stored.</param>
        public static void GetFactorsLLTPlain(IMatrix LLT, IMatrix L, IMatrix LT)
        {
            int dim = LLT.RowCount;
            for (int i = 0; i < dim; ++i)
            {
                // Diagonal elements are copied to both products:
                L[i,i] = LT[i,i] = LLT[i,i];
                for (int j = i + i; j < dim; ++j)
                {
                    double element = L[i,j];
                    L[j, i] = LT[i, j] = 0.0;
                    L[i, j] = LT[j, i] = element;
                }
            }
        }

        public static void GetFactorsLLT(IMatrix LLT, IMatrix L, IMatrix LT)
        {
            if (LLT == null)
                throw new ArgumentNullException("Decomposed LLT matrix not specified (null reference).");
            else if (LLT.RowCount != LLT.ColumnCount)
                throw new ArgumentException("Dexomposed LLT matrix is not a square matrix. Dimensions: "
                    + LLT.RowCount + "x" + LLT.ColumnCount + ".");
            else if (L == null)
                throw new ArgumentNullException("Storage for lower triangular factor of LLT decomposition not specified (null reference).");
            else if (L.RowCount != LLT.RowCount || L.ColumnCount != LLT.ColumnCount)
                throw new ArgumentException("Matrix to store lower triangular factor of LLT decomposition has wrong dimensions: "
                    + L.RowCount + "x" + L.ColumnCount + " instead of " + LLT.RowCount + "x" + LLT.ColumnCount);
            else if (LT == null)
                throw new ArgumentNullException("Storage for upper triangular factor of LLT decomposition not specified (null reference).");
            else if (LT.RowCount != LLT.RowCount || LT.ColumnCount != LLT.ColumnCount)
                throw new ArgumentException("Matrix to store upper triangular factor of LLT decomposition has wrong dimensions: "
                    + L.RowCount + "x" + L.ColumnCount + " instead of " + LLT.RowCount + "x" + LLT.ColumnCount);
            GetFactorsLLTPlain(LLT, L, LT);
        }

        public static void GetFactorsLLT(IMatrix LLT, ref IMatrix L, ref IMatrix LT)
        {
            if (LLT == null)
                throw new ArgumentNullException("Decomposed LLT matrix not specified (null reference).");
            else if (LLT.RowCount != LLT.ColumnCount)
                throw new ArgumentException("Dexomposed LLT matrix is not a square matrix. Dimensions: "
                    + LLT.RowCount + "x" + LLT.ColumnCount + ".");
            if (L == null)
                L=LLT.GetNew();
            else if (L.RowCount != LLT.RowCount || L.ColumnCount != LLT.ColumnCount)
                L=LLT.GetNew();
            if (LT == null)
                LT=LLT.GetNew();
            else if (LT.RowCount != LLT.RowCount || LT.ColumnCount != LLT.ColumnCount)
                LT=LLT.GetNew();
            GetFactorsLLTPlain(LLT, L, LT);
        }


        #endregion DecompositionLLT



        #region DecompositionLDLT




        #endregion DecompositionLDLT




        #region Testing


        /// <summary>Tests Cholecki decomposition of a matrix.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass.</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestDecompositionLLT(double tolerance, bool printReports)
        {
            int dimension = 4;
            IMatrix original = CreateRandomSymmetricPositiveDefinite(dimension);
            IMatrix decomp = null;  
            DecomposeLLT(original, ref decomp,0.0);
            IMatrix L = null;
            IMatrix LT = null;
            GetFactorsLLT(decomp, ref L, ref LT);
            IMatrix referenceResult = null;
            Multiply(L, LT, ref referenceResult);
            bool ret = CheckTestResult(original, referenceResult, tolerance, printReports);
            return ret;
        }



        /// <summary>Tests solution of a system of equations with a Choleski-decomposed matrix.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass.</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestSolveLLT(double tolerance, bool printReports)
        {
            int d=4;
            IMatrix original = CreateRandomSymmetricPositiveDefinite(d);
            IMatrix decomp = null;  
            DecomposeLLT(original, ref decomp, 0.0);
            IVector rightSides = new Vector(d);
            rightSides.SetRandom();
            IVector x = null;
            SolveLLT(decomp, rightSides, ref x);
            IVector referenceResult = null;
            Multiply(original, x, ref referenceResult);
            bool ret = CheckTestResult(rightSides, referenceResult, tolerance, printReports);
            return ret;
        }




        /// <summary>Tests various matrix and vector decompositions with fixed tolerance of 1.0E-6.</summary>
        /// <param name="printReports">Specifies whether to print short reports to console or not.</param>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixDecompositions(bool printReports)
        {
            return TestMatrixDecompositions(1.0e-6, printReports);
        }

        /// <summary>Tests various matrix and vector decompositions, without printing reports.</summary>
        /// <param name="tolerance">Tolerance for difference between product and test expression below which any individual test passes.
        /// Must be greater than 0.</param>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixDecompositions(double tolerance)
        {
            return TestMatrixDecompositions(tolerance, false);
        }

        /// <summary>Tests various matrix and vector decompositions with fixed tolerance of 1.0E-6 and without printing reports.</summary>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixDecompositions()
        {
            return TestMatrixDecompositions(1.0e-6, false);
        }

        /// <summary>Tests various matrix and vector decompositions.</summary>
        /// <param name="tolerance">Tolerance for difference between product and test expression below which any individual test passes.
        /// Must be greater than 0.</param>
        /// <param name="printReports">Specifies whether to print short reports to console or not.</param>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixDecompositions(double tolerance, bool printReports)
        {
            if (tolerance <= 0)
                throw new ArgumentException("Invalid tolerance " + tolerance + ", should not be less or equal to 0!");
            bool passed = true, passedThis;
            int numPassed = 0, numFailed = 0;
            if (printReports)
                Console.WriteLine(Environment.NewLine, "Tests of matrix decompositions:");

            if (printReports)
                Console.WriteLine(Environment.NewLine + "TestDecompositionLLT: ");
            passedThis = TestDecompositionLLT(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
                Console.WriteLine(Environment.NewLine + "TestSolveLLT: ");
            passedThis = TestSolveLLT(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;


            if (printReports)
            {
                if (passed)
                    Console.WriteLine(Environment.NewLine + "All tests have passed." + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "WARNING: Some tests have NOT PASSED ("
                        + numFailed + " of " + (numPassed + numFailed) + " failed)." + Environment.NewLine);
            }
            return passed;
        }


        #endregion Testing







    }  // abstract class MatrixBaseDev


}


