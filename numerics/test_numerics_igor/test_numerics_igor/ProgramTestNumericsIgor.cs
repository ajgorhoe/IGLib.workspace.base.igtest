using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IG.Lib;
using IG.Num;



namespace IG.Test
{
    class ProgramTestNumericsIgor
    {

        public static int TestList(IList<int> l)
        {
            return l[0];
        }


        /// <summary>Main program for testing numerical utilities.</summary>
        static void Main(string[] args)
        {

            
            Console.WriteLine(Environment.NewLine + "Test program for numerical utilities." + Environment.NewLine);


            if (true)  // Gramm-Schmidt orthogonalization:
            {
                IRandomGenerator rand = null;
                int dim = 5;
                int numRepetitions = 3;
                bool normalize = false;
                bool modifiedGrammSchmidt = true;
                double tol = 1.0e-8;
                int outputLevel = 3;
                bool nonRobust = false;

                bool success = VectorBase.TestGramSchmidtOrthogonalization(dim, numRepetitions, tol, outputLevel, rand, normalize, 
                    modifiedGrammSchmidt, nonRobust);
                Console.WriteLine();
            }


            if (false)
            {

                //MatrixDecompositionProgram.MainMatrixDecompositionProgram(null);

                MatrixBase.TestLuDecompositionDemo();

                //MatrixBase.TestCholeskyDecompositionDemo();

                // Tests:

                //MatrixBase.TestIndices();

                if (false) MatrixBase.TestLuDecomposition(100 /* dim */, 1 /* numRepetitions */, 1e-6 /* tol */, 1 /* outputLevel */ ,
                    null /* randomGenerator */, null /* A */, null /* b */ );

                IMatrix A = new Matrix(3, 3);
                A[0, 0] = 4; A[0, 1] = 12; A[0, 2] = -16;
                A[1, 0] = 12; A[1, 1] = 37; A[1, 2] = -43;
                A[2, 0] = -16; A[2, 1] = -43; A[2, 2] = 98;

                if (false) Matrix1.TestLdltDecomposition(100 /* dim */, 1 /* numRepetitions */, 1e-6 /* tol */, 1 /* outputLevel */ ,
                    null /* randomGenerator */, null /* A */, null /* b */ );

                if (false) Matrix1.TestCholeskyDecomposition(100 /* dim */, 1 /* numRepetitions */, 1e-6 /* tol */, 1 /* outputLevel */ ,
                    null /* randomGenerator */, null /* A */, null /* b */ );

            }

            if (false)
            {
                // Speed tests:

                int numEquations = 500;

                //SpeedTestCpu.TestComputationalTimesLU_Base(numEquations /* numEquations */, 2 /* outputLevel */, false /* testProduct */);
                //SpeedTestCpu.TestComputationalTimesLU_IGLib(numEquations /* numEquations */, 2 /* outputLevel */, false /* testProduct */);

                //SpeedTestCpu.TestComputationalTimesCholesky_Base(numEquations /* numEquations */, 2 /* outputLevel */, false /* testProduct */);
                SpeedTestCpu.TestComputationalTimesCholesky_IGLib(numEquations /* numEquations */, 2 /* outputLevel */, false /* testProduct */);

                //SpeedTestCpu.TestComputationalTimesLdlt_Base(numEquations /* numEquations */, 2 /* outputLevel */, false /* testProduct */);

            }


            if (false)  // Testt WLS approximations
            {
                double error = 0;
                int dim = 3;
                int outputLevel = 1;
                int numValueChanges = 3;
                bool changePoints = true; 
                bool ClearSystemEquations = true;
                double excessFactor = 1.5;
                int randomSeed = 555;
                double perturbationFactor =  -1.0e-3;  //  0; //

                //error = ApproximatorWlsLinearBase.TestWeightedLeastSquaresQuadraticBasis(dim, outputLevel,
                //    excessFactor, randomSeed, perturbationFactor);

                error = ApproximatorWlsLinearBase.TestWeightedLeastSquaresMultipleQuadraticBasis(dim, outputLevel, numValueChanges,
                    changePoints, ClearSystemEquations, excessFactor, randomSeed, perturbationFactor);

#if false

                
                error = ApproximatorWlsLinearBase.TestWeightedLeastSquaresQuadraticBasis(dim, outputLevel,
                    randomSeed, excessFactor);

                //error = ApproximatorWlsLinearBase.TestWeightedLeastSquaresMultipleQuadraticBasis(dim, outputLevel, numValueChanges,
                //    changePoints, ClearSystemEquations, excessFactor, randomSeed);


#endif // previoius content:

            }



            Console.WriteLine(Environment.NewLine + "Program ENDED." + Environment.NewLine);

            
            //Console.WriteLine(Environment.NewLine + UtilSystem.GetApplicationInfo(infoLevel:5, includeIglibInfo: true));

        }  // Main()


        public static void Test()
        {
            Double D = new Double();
            double a = 2.0, b = 3.5;

            D = a * b;


        }



    }  // class ProgramTestNumericsIgor
}
