using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IG.Lib;
using IG.Num;

//namespace IG.Num
//{

//    public class TestAlgLibBase
//    {

//        protected static int _defaultOutputLevel = 1;

//        public static int DefaultOutputLevel
//        {
//            get { return _defaultOutputLevel; }
//            set { _defaultOutputLevel = value; }
//        }

//        protected int _outputLevel = DefaultOutputLevel;

//        public int OutputLevel
//        {
//            get { return _outputLevel; }
//            set { _outputLevel = value; }
//        }

//    }

//    /// <summary>Test class for the old version of AlgLib algorithms that were not under GPL.</summary>
//    /// $A Igor Apr08;
//    public class TestAlglibOld : TestAlgLibBase
//    {



//        /// <summary>Test method for the old version of AlgLib algorithms that were not under GPL.</summary>
//        public void TestLbfgsAlglibOld()
//        {

//            Console.WriteLine("");
//            Console.WriteLine("    Test of LBFGS from AlgLib...");
//            Console.WriteLine("");
//            Console.WriteLine("Test performed on the Rosenbrock function...");

//            int dim = 2; // dimension

//            // Initial guess:
//            IVector param = new Vector(dim);
//            param[0] = 2;
//            param[1] = 2;
//            IScalarFunction func = new ScalarFunctionExamples.Rosenbrock();

//            ScalarFunctionExamples.Rosenbrock fa = null;
//            fa.NumericalGradientForwardPlain(null, null, null);

//            double functionValue = 0;
//            IVector functionGradient = new Vector(dim);

//            double x = 0;
//            double y = 0;

//            //int dim = 2;
//            //// int m = 2;  // 

//            int m = 2;  // number of corrections of Hessina in the BFGS scheme, 3<=m<=7, m<dim 
//            double[] paramTable = new double[dim];
//            double epsg = 0;
//            double epsf = 0;
//            double epsx = 0;
//            int maxits = 0;
//            int flags = 0;
//            lbfgs.lbfgsstate state = new lbfgs.lbfgsstate();


//            paramTable[0] = param[0];
//            paramTable[1] = param[0];

//            epsg = 0;
//            epsf = 0;
//            epsf = 0;

//            double eps = 1e-6;
//            epsg = eps;
//            epsf = eps;
//            epsx = eps;


//            lbfgs.minlbfgs(dim, m, ref paramTable, epsg, epsf, epsx, maxits, flags, ref state);


//            if (OutputLevel >= 1)
//            {
//                Console.WriteLine("{0,3:S}: {1,15} , {2,15:S} ;  {3,12:S} ; {4,10:S}", "No", "x    ", "y    ", "f(x, y)", "error");
//            }

//            // Auxiiary variables:
//            int whicheval = 0;   // consecutive evaluation number:
//            double xsol = 1, ysol = 1;
//            double error = 0;

//            while (lbfgs.minlbfgsiteration(ref state))
//            {

//                x = state.x[0];
//                y = state.x[1];
//                param[0] = x;
//                param[1] = y;
//                // paramTable[0] = x; paramTable[1] = y;

//                functionValue = func.Value(param);
//                func.Gradient(param, ref functionGradient);

//                //state.f = (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
//                //state.g[0] = -2 * (1 - x) - 400 * (y - x * x) * x;
//                //state.g[1] = 200 * (y - x * x);

//                state.f = functionValue;
//                for (int i = 0; i < dim; ++i)
//                {
//                    state.g[i] = functionGradient[i];
//                }


//                // state.g[0] *= 1.01;

//                error = M.sqr(x - xsol) + M.sqr(y - ysol);

//                if (OutputLevel >= 1)
//                {
//                    Console.WriteLine("{0,3}: {1,15:G10} , {2,15:G10} ;  {3,12:G4} ; {4,10:G3}", whicheval, x, y, state.f, error);
//                }
//                ++whicheval;

//                //if (whicheval > 1)
//                //    flags = 2;

//            }
//            Console.WriteLine();

//        }  // TestLbfgsAlglibOld()


//        /// <summary>Test method for the old version of AlgLib algorithms that were not under GPL.</summary>
//        /// <param name="dim">Dimension of parameter space.</param>
//        /// <remarks>The generaliyed Rosenbrock function is minimized.
//        /// <para>Function has global minimum at {1, 1, 1, ...}, but can have several local minima for dimensions greater than 3.</para>
//        /// <para>For dimensions up to 7, eventual non-global local minima can be at {-1, 1, 1, ...}.</para></remarks>
//        public void TestLbfgsAlglibOld(int dim)
//        {

//            Console.WriteLine("");
//            Console.WriteLine("    Test of the LBFGS algorithm from AlgLib...");
//            Console.WriteLine("");
//            Console.WriteLine("Test performed on the generalized Rosenbrock function, dim = " + dim);


//            // Initial guess:
//            IVector param = new Vector(dim);
//            param.SetConstant(3.0);
//            IScalarFunction func = new ScalarFunctionExamples.RosenbrockGeneralizedAdjacent();
//            double functionValue = 0;
//            IVector functionGradient = new Vector(dim);

//            IVector globalMinimum = new Vector(dim);
//            for (int i = 0; i < dim; ++i)
//                globalMinimum[i] = 1.0;

//            //double x = 0;
//            //double y = 0;

//            //int dim = 2;
//            //// int m = 2;  // 

//            int m = dim;  // number of corrections of Hessina in the BFGS scheme, 3<=m<=7, m<dim 
//            double[] paramTable = new double[dim];
//            double epsg = 0;
//            double epsf = 0;
//            double epsx = 0;
//            int maxits = 0;
//            int flags = 0;
//            lbfgs.lbfgsstate state = new lbfgs.lbfgsstate();

//            for (int i = 0; i < dim; ++i)
//                paramTable[i] = param[i];

//            epsg = 0;
//            epsf = 0;
//            epsf = 0;

//            double eps = 1e-10;
//            epsg = eps;
//            epsf = eps;
//            epsx = eps;


//            lbfgs.minlbfgs(dim, m, ref paramTable, epsg, epsf, epsx, maxits, flags, ref state);

//            if (OutputLevel >= 1)
//            {
//                Console.WriteLine("{0,3:S}: {1,12:S} ; {2,10:S}", "No", "f(x, y)", "||grad||");
//            }

//            // Auxiiary variables:
//            int whicheval = 0;   // consecutive evaluation number:
//            double xsol = 1, ysol = 1;
//            double error = 0;

//            while (lbfgs.minlbfgsiteration(ref state))
//            {

//                for (int i = 0; i < dim; ++i)
//                    param[i] = state.x[i];

//                functionValue = func.Value(param);
//                func.Gradient(param, ref functionGradient);

//                state.f = functionValue;
//                for (int i = 0; i < dim; ++i)
//                {
//                    state.g[i] = functionGradient[i];
//                }

//                error = functionGradient.NormEuclidean;

//                if (OutputLevel >= 1)
//                {
//                    Console.WriteLine("{0,3}:  {1,12:G4} ; {2,10:G3}", whicheval, state.f, error);
//                    if (OutputLevel >= 2)
//                    {
//                        Console.WriteLine("  x = {0:S}", param.ToString("G4"));
//                    }
//                }
                
//                ++whicheval;

//                //if (whicheval > 1)
//                //    flags = 2;

//            }
//            Console.WriteLine();

//        }  // TestLbfgsAlglibOld()




//    }
//}
