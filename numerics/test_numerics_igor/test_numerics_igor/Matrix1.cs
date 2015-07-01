using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IG.Lib;

namespace IG.Num
{
    
    public class Matrix1: IG.Num.Matrix
    {

        public Matrix1(int d1, int d2): base(d1, d2)
        {  }

        
        /// <summary>Returns a readable an easily string form of a matrix, accuracy and padding can be set.</summary>
        /// <param name="mat">Matrix to be changed to a string.</param>
        /// <param name="accuracy">Accuracy of matrix elments representations.</param>
        /// <param name="padding">Paddind of matrix elements.</param>
        /// <returns>A readable string representation in tabular form.</returns>
        public override string ToStringReadable(int accuracy = 4, int padding = 8)
        {
            return Environment.NewLine + "ToStringReadable from Matrix1 class: " + Environment.NewLine + base.ToStringReadable(accuracy, padding);

        }




        #region IMatrix







        #endregion IMatrix




        #region Decomposition.Old


        // ******************************


        //static IMatrix MatrixCreate1111(int rows, int cols)
        //{
        //    return new Matrix(rows, cols);
        //}


        //static IMatrix MatrixRandom(int rows, int cols, double minVal, double maxVal, int seed)
        //{
        //    // return a matrix with random values
        //    Random ran = new Random(seed);
        //    IMatrix result = new Matrix(rows, cols);
        //    for (int i = 0; i < rows; ++i)
        //        for (int j = 0; j < cols; ++j)
        //            result[i, j] = (maxVal - minVal) * ran.NextDouble() + minVal;
        //    return result;
        //}


        //static IMatrix MatrixIdentity(int n)
        //{
        //    // return an n x n Identity matrix
        //    IMatrix result = new Matrix(n, n);
        //    for (int i = 0; i < n; ++i)
        //        result[i, i] = 1.0;

        //    return result;
        //}


        static string MatrixAsString(IMatrix matrix)
        {
            string s = "";
            for (int i = 0; i < matrix.RowCount; ++i)
            {
                for (int j = 0; j < matrix.ColumnCount; ++j)
                    s += matrix[i, j].ToString("F3").PadLeft(8) + " ";
                s += Environment.NewLine;
            }
            return s;
        }



        static string VectorAsString(double[] vector)
        {
            string s = "";
            for (int i = 0; i < vector.Length; ++i)
                s += vector[i].ToString("F3").PadLeft(8) + Environment.NewLine;
            s += Environment.NewLine;
            return s;
        }

        static string VectorAsString(int[] vector)
        {
            string s = "";
            for (int i = 0; i < vector.Length; ++i)
                s += vector[i].ToString().PadLeft(2) + " ";
            s += Environment.NewLine;
            return s;
        }


        static bool MatrixAreEqual(IMatrix matrixA, IMatrix matrixB, double epsilon)
        {
            // true if all values in matrixA == corresponding values in matrixB
            int aRows = matrixA.RowCount; int aCols = matrixA.ColumnCount;
            int bRows = matrixB.RowCount; int bCols = matrixB.ColumnCount;
            if (aRows != bRows || aCols != bCols)
                throw new Exception("Non-conformable matrices in MatrixAreEqual");

            for (int i = 0; i < aRows; ++i) // each row of A and B
                for (int j = 0; j < aCols; ++j) // each col of A and B
                    //if (matrixA[i, j] != matrixB[i, j])
                    if (Math.Abs(matrixA[i, j] - matrixB[i, j]) > epsilon)
                        return false;
            return true;
        }


        static IMatrix MatrixProduct(IMatrix matrixA, IMatrix matrixB)
        {
            int aRows = matrixA.RowCount; int aCols = matrixA.ColumnCount;
            int bRows = matrixB.RowCount; int bCols = matrixB.ColumnCount;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");

            IMatrix result = new Matrix(aRows, bCols);

            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k < bRows
                        result[i, j] += matrixA[i, k] * matrixB[k, j];

            //Parallel.For(0, aRows, i =>
            //  {
            //    for (int j = 0; j < bCols; ++j) // each col of B
            //      for (int k = 0; k < aCols; ++k) // could use k < bRows
            //        result[i, j] += matrixA[i, k] * matrixB[k, j];
            //  }
            //);

            return result;
        }


        static double[] MatrixVectorProduct(IMatrix matrix, double[] vector)
        {
            // result of multiplying an n x m matrix by a m x 1 column vector (yielding an n x 1 column vector)
            int mRows = matrix.RowCount; int mCols = matrix.ColumnCount;
            int vRows = vector.Length;
            if (mCols != vRows)
                throw new Exception("Non-conformable matrix and vector in MatrixVectorProduct");
            double[] result = new double[mRows]; // an n x m matrix times a m x 1 column vector is a n x 1 column vector
            for (int i = 0; i < mRows; ++i)
                for (int j = 0; j < mCols; ++j)
                    result[i] += matrix[i, j] * vector[j];
            return result;
        }


        static IMatrix MatrixDuplicate(IMatrix matrix)
        {
            // allocates/creates a duplicate of a matrix. assumes matrix is not null.
            IMatrix result = new Matrix(matrix.RowCount, matrix.ColumnCount);
            for (int i = 0; i < matrix.RowCount; ++i) // copy the values
                for (int j = 0; j < matrix.ColumnCount; ++j)
                    result[i, j] = matrix[i, j];
            return result;
        }


        public static void MainMatrixDecompositionOld(string[] args)
        {
            Console.WriteLine("\nBegin matrix decomposition demo\n");

            IMatrix m = new Matrix(4, 4);
            m[0, 0] = 3.0; m[0, 1] = 7.0; m[0, 2] = 2.0; m[0, 3] = 5.0;
            m[1, 0] = 1.0; m[1, 1] = 8.0; m[1, 2] = 4.0; m[1, 3] = 2.0;
            m[2, 0] = 2.0; m[2, 1] = 1.0; m[2, 2] = 9.0; m[2, 3] = 3.0;
            m[3, 0] = 5.0; m[3, 1] = 4.0; m[3, 2] = 7.0; m[3, 3] = 1.0;

            Console.WriteLine("Matrix m = \n" + MatrixAsString(m));

            int[] perm = new int[4];
            int toggle;
            IMatrix luMatrix = MatrixDuplicate(m);
            LuDecompose(m, out toggle, ref perm, ref luMatrix);
            IMatrix lower = LuExtractLowerOld(luMatrix);
            IMatrix upper = LuExtractUpperOld(luMatrix);

            Console.WriteLine("The (combined) LUP decomposition of m is\n" + MatrixAsString(luMatrix));
            Console.WriteLine("The decomposition permutation array is: " + VectorAsString(perm));
            Console.WriteLine("The decomposition toggle value is: " + toggle);
            Console.WriteLine("\nThe lower part of LU is \n" + MatrixAsString(lower));
            Console.WriteLine("The upper part of LU is \n" + MatrixAsString(upper));

            IMatrix inverse = LuInveseOld(m);
            Console.WriteLine("Inverse of m computed from its decomposition is\n" + MatrixAsString(inverse));
            IMatrix prod = m.GetNew();
            MatrixBase.Multiply(inverse, m, ref prod);
            // Console.WriteLine("Product of oridinal matrix and its inverse is\n" + MatrixAsString(prod));
            IMatrix diff = m.GetNew();
            MatrixBase.SetIdentity(diff);
            MatrixBase.Subtract(prod, diff, ref diff);
            Console.WriteLine("Error norm for matrix inverse: " + (diff.NormForbenius).ToString() + Environment.NewLine);

            double det = LuDeterminantOld(m);
            Console.WriteLine("Determinant of m computed via decomposition = " + det.ToString("F1"));

            Console.WriteLine("\nLinear system mx = b problem: \n");
            Console.WriteLine("3x0 + 7x1 + 2x2 + 5x3 = 49");
            Console.WriteLine(" x0 + 8x1 + 4x2 + 2x3 = 30");
            Console.WriteLine("2x0 +  x1 + 9x2 + 3x3 = 43");
            Console.WriteLine("5x0 + 4x1 + 7x2 +  x3 = 52");
            Console.WriteLine("");

            double[] b = new double[] { 49.0, 30.0, 43.0, 52.0 };
            Console.WriteLine("Solving system via decomposition");
            double[] x = LuSolveOld(m, b);
            Console.WriteLine("\nSolution is x = \n" + VectorAsString(x));

            // extra: matrix decomposition concepts
            IMatrix lu = MatrixProduct(lower, upper);
            IMatrix orig = LuUnPermuteOld(lu, perm) as IMatrix;  // use a custom method with the perm array to unscramble LU
            if (MatrixAreEqual(orig, m, 0.000000001) == true)
                Console.WriteLine("Product of L and U successfully unpermuted using perm array");
            else
                Console.WriteLine("Pruduct of L and U unpermuted UNSUCCESSFULLY using perm array.");

            IMatrix permMatrix = PermArrayToMatrixOld(perm); // convert the perm array to a matrix
            orig = MatrixProduct(permMatrix, lu); // another way to unscramble
            if (MatrixAreEqual(orig, m, 0.000000001) == true)
                Console.WriteLine("\nProduct of L and U successfully unpermuted using perm matrix\n");
            else
                Console.WriteLine("Pruduct of L and U unpermuted UNSUCCESSFULLY using perm matrix.");

            Console.WriteLine("End matrix decomposition demo\n");
            // Console.ReadLine();
        }  // Main LU


        protected static IMatrix LuInveseOld(IMatrix matrix)
        {
            double[] x = new double[matrix.RowCount];
            return LuInverseOld(matrix, x);
        }


        public static IMatrix LuInverseOld(IMatrix matrix, double[] x)
        {
            int dim = matrix.RowCount;
            IMatrix lu = matrix.GetNew(dim, dim);
            IMatrix res = matrix.GetNew(dim, dim);
            int[] perm = new int[dim];
            int toggle = 0;
            IVector auxRight = matrix.GetNewVector(dim);
            IVector auxX = matrix.GetNewVector(dim);

            LuDecompose(matrix, out toggle, ref perm, ref lu);
            LuInverse(lu, perm, ref auxRight, ref auxX, ref res);
            return res;
        }

        public static double LuDeterminantOld(IMatrix matrix)
        {
            int dim = matrix.RowCount;
            int[] perm = new int[dim];
            int toggle;
            IMatrix lum = MatrixDuplicate(matrix);
            LuDecompose(matrix, out toggle, ref perm, ref lum);
            if (lum == null)
                throw new Exception("Unable to compute MatrixDeterminant");
            //double result = toggle;
            //for (int i = 0; i < lum.RowCount; ++i)
            //    result *= lum[i, i];
            double result = LuDeterminant(lum, toggle);
            return result;
        }


        protected static double[] HelperSolveOld(IMatrix luMatrix, double[] b, double[] x)
        {
            int dim = luMatrix.RowCount;
            IVector bVec = new Vector(b);
            IVector xVec = new Vector(dim);
            LuSolveNoPermutationsPlain(luMatrix, bVec, xVec);
            // double[] ret = xVec.ToArray();
            for (int i = 0; i < dim; ++i)
                x[i] = xVec[i];
            return x;
        }

        public static void LuSolveOld(IMatrix A, IVector b, ref int[] auxPerm, ref IVector auxVec, ref IMatrix auxMatLu, ref IVector x)
        {
            Console.WriteLine(Environment.NewLine + "CALLED LuSolve." + Environment.NewLine);
            int toggle = 0;
            LuDecompose(A, out toggle, ref auxPerm, ref auxMatLu);
            LuSolve(auxMatLu, auxPerm, b, ref auxVec, ref x);


            //// Solve Ax = b
            //int n = A.RowCount;

            //// 1. decompose A
            //int dim = A.RowCount;
            //int[] perm = new int[dim];
            //IMatrix luMatrix = MatrixDuplicate(A);
            //LuDecompose(A, out toggle, ref perm, ref luMatrix);
            ////if (luMatrix == null)
            ////    return null;

            //// 2. permute b according to perm[] into bp
            //IVector bp = new Vector(b.Length);
            //for (int i = 0; i < n; ++i)
            //    bp[i] = b[perm[i]];

            //// 3. call helper
            //LuSolveNoPermutationsPlain(luMatrix, bp, x);
            //// return x;


        } // SystemSolve


        protected static double[] LuSolveOld(IMatrix A, double[] b)
        {
            int dim = A.RowCount;
            IVector x = new Vector(dim);
            IMatrix luMat = new Matrix(dim, dim);
            IVector auxVec = null;
            IVector bVec = new Vector(b);
            int[] auxPerm = new int[dim];
            // LuSolveOld(A, bVec, ref auxPerm, ref auxVec, ref luMat, ref x);
            int toggle = 0;
            LuDecompose(A, out toggle, ref auxPerm, ref luMat);
            LuSolve(luMat, auxPerm, bVec, ref auxVec, ref x);

            return x.ToArray();

        }


        public static IMatrix LuExtractLowerOld(IMatrix matrix)
        {
            IMatrix res = new Matrix(matrix.RowCount, matrix.RowCount);
            LuExtractLower(matrix, ref res);
            return res;
        }

        public static IMatrix LuExtractUpperOld(IMatrix matrix)
        {
            IMatrix res = new Matrix(matrix.RowCount, matrix.RowCount);
            LuExtractUpper(matrix, ref res);
            return res;
        }

        protected static IMatrix PermArrayToMatrixOld(int[] perm)
        {
            // convert Doolittle perm array to corresponding perm matrix
            int dim = perm.Length;
            IMatrix result = new Matrix(dim, dim);
            PermutationArrayToMatrix(perm, ref result);
            return result as IMatrix;
        }


        static IMatrix LuUnPermuteOld(IMatrix luProduct, int[] perm)
        {
            int dim = luProduct.RowCount; 
            IMatrix ret = new Matrix(dim, dim);
            int[] aux = new int[dim];
            UnPermute(luProduct, perm, ref aux, ref ret);
            return ret;
        }


        // ******


        #endregion Decomposition.Old






    }




}
