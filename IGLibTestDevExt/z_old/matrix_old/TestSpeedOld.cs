// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using IG.Lib;

// From Math.Net:

using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;
using QRDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.QRDecomposition;
using LUDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.LUDecomposition;



namespace IG.Old
{

    /// <summary>Various utilities for testing computational speed of the current system - Old version!</summary>
    /// $A Igor xx Feb08;
    public class SpeedTestCpuOld
    {

        #region MathNet

        // Utilities based on Math.NET numerical library.

        /// <summary>Test of LU decomposition.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLU_MathNet(int numEq, int outLevel)
        {
            return TestComputationalTimesLU_MathNet(numEq, outLevel, false /* testProduct */);
        }

        /// <summary>Test of LU decomposition.</summary>
        /// <param name="outLevel">Level of output.</param>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total wallclock time (in seconds) spent for the test.</returns>
        public static double TestComputationalTimesLU_MathNet(int numEq, int outLevel, bool testProduct)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for LU decomposition:");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            // Naredimo štoparico, s katero merimo čas. V konstruktorju z argumentom podamo ime, po katerem
            // lahko identificiramo narejeno štoparico. To pride prav, če jih imamo več.
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with LU decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            Matrix_MathNet A = Matrix_MathNet.Random(numEq, numEq);  // naredimo kvadratno matriko z naključnimi elementi
            A[0, 0] = 0.0;  // Element [1,1] is set to 0, so pivoting is needed in LU (swapping of lines)
            Matrix_MathNet b = Matrix_MathNet.Random(numEq, 1);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            LUDecomposition_MathNet LU = A.LUDecomposition;  // Calculation of LU decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time nexessary for LU DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                Matrix_MathNet APerm = A.GetMatrix(LU.Pivot, 0, numEq - 1);  // permutation matrix for A
                Matrix_MathNet P = LU.L * LU.U;  // product of lower ant upper triangular matrix
                double NormProductDifference = (APerm - P).NormF();
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||Δ A|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            Matrix_MathNet x = LU.Solve(b);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            double NormResiduum = (A * x - b).NormF();
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + NormResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (LU, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());
                Console.WriteLine("____________________________________________________________");

                Console.WriteLine(Environment.NewLine);
            }

            //t.Reset();
            //numEq *= 10;

            return t.TotalTime;
        }  // TestComputationalTimesLU()

        /// <summary>Test of QR decomposition. Writes times necessary for all steps.
        /// <para>This method does not perform test of decomposition (comparison of product 
        /// of factors with the original matrix).</para></summary>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>        
        /// <param name="outLevel">Level of output.</param>
        /// <returns>Total time spent for all operations.</returns>
        public static double TestComputationalTimesQR_MathNet(int numEq, int outLevel)
        {
            return TestComputationalTimesQR_MathNet(numEq, outLevel, false /* testProduct */);
        }

        /// <summary>Test of QR decomposition. Writes times necessary for all steps.</summary>
        /// <param name="numEq">Number of equations to be solved with decomposition.</param>        
        /// <param name="outLevel">Level of output.</param>
        /// <param name="testProduct">If true then it is tested if the product of factors gives the original 
        /// matrix. Otherwise, this test is skipped.</param>
        /// <returns>Total time spent for all operations.</returns>
        public static double TestComputationalTimesQR_MathNet(int numEq, int outLevel, bool testProduct)
        {
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Measuring time needed for QR decomposition:");
                Console.WriteLine();
            }
            if (numEq < 1)
                throw new ArgumentException("Number of equations must be greater than 0.");
            // Naredimo štoparico, s katero merimo čas. V konstruktorju z argumentom podamo ime, po katerem
            // lahko identificiramo narejeno štoparico. To pride prav, če jih imamo več.
            StopWatch1 t = new StopWatch1("Decomposition");
            // Naredimo še eno štoparico, ki meri čisti čas računanja brez preizkusov:
            StopWatch1 tbare = new StopWatch1("Decomposition, pure time");

            if (outLevel > 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("========================================================================");
                Console.WriteLine("Solution of system of " + numEq.ToString() + " equations with QR decomposition: " + Environment.NewLine);
            }
            t.Start();
            // Form system of equations:
            Matrix_MathNet A = Matrix_MathNet.Random(numEq, numEq);  // naredimo kvadratno matriko z naključnimi elementi
            A[0, 0] = 0.0;  // Element [1,1] is set to 0, so pivoting is needed in LU (swapping of lines)
            Matrix_MathNet b = Matrix_MathNet.Random(numEq, 1);
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for CREATION of system matrix and right-hand side vector:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            t.Start();
            tbare.Start();
            QRDecomposition_MathNet QR = A.QRDecomposition;  // Calculation of QR decomposition
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time nexessary for QR DECOMPOSITION:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }
            if (testProduct)
            {
                t.Start();
                Matrix_MathNet P = QR.Q * QR.R;  // product of factors
                double NormProductDifference = (A - P).NormF();
                t.Stop();
                if (outLevel > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error of decomposition - norm of the difference ||Δ A|| = " + NormProductDifference.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Time necessary for verification of decomposition:");
                    Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                    Console.WriteLine();
                }
            }
            // Solution of system of equations with decomposition (back substitution):
            t.Start();
            tbare.Start();
            Matrix_MathNet x = QR.Solve(b);
            t.Stop();
            tbare.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Time necessary for SOLUTION with decomposed matrix:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();
            }

            // Verification: calculation of error:
            t.Start();
            double NormResiduum = (A * x - b).NormF();
            t.Stop();
            if (outLevel > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error of solution: Norm of the difference ||A x - b|| = " + NormResiduum.ToString());
                Console.WriteLine();
                Console.WriteLine("Time necessary for calculating solution error:");
                Console.WriteLine("    t = " + t.Time + " s (CPU: " + t.CpuTime + " s)");
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("Total time for all operations (QR, " + numEq + " equations): ");
                Console.WriteLine("    t = " + t.TotalTime + " s (CPU: " + t.TotalCpuTime + " s)");
                Console.WriteLine();

                //Console.WriteLine();
                //Console.WriteLine("Data from stopwatch:");
                //Console.WriteLine(t.ToString());
                //Console.WriteLine();
                //Console.WriteLine("Stopwatch for pure execution time (without testing):");
                //Console.WriteLine(tbare.ToString());

                Console.WriteLine("____________________________________________________________");
                Console.WriteLine(Environment.NewLine);
            }

            //// Na koncu resetiramo štoparico in povečamo dimenzijo problema:
            //t.Reset();
            //numEq *= 10;

            return t.TotalTime;
        }  // TestComputationalTimesQR()

        #endregion MathNet



    }

}

