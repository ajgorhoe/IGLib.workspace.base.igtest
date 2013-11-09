using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP1;

using AlgLib;

namespace NumLib
{

    public class ExampleAlgLib2008
    {


        #region OptQuadratic

        /// <summary>Minimization of a quadratic function performed by the <see cref="MinimizerLbfgs"/> object.</summary>
        public static void OptimizeQuadraticWithObject()
        {
            
            // Prepare definition of the quadratic function:
            double[] shifts = new double[] { 1, 1, 1, 1 };
            double[] factors = new double[] { 1e-3, 2e-2, 3e-1, 4  };
            FunctionWithGradient f = new QuadraticFunctionShifted(shifts, factors);

            // Prepare the initial guess:
            double[] initialGuess = new double[] {
                9.4, 
                132.1, 
                55.2, 
                3.14
            };

            double tolg = 1.0e-6;
            double tolf = 0;
            double tolx = 0;
            int maxIt = 300;
            int flags = 0;

            // Create an initialized minimizer:
            MinimizerLbfgs minimizer = new MinimizerLbfgs(f, initialGuess, maxIt, tolg, tolf, tolx);
            minimizer.OutputLevel = 2;
            // Find the solution:
            minimizer.Minimize();
        }

        /// <summary>Minimization of a quadratic function.</summary>
        public static void OptimizeQuadratic()
        {
            double[] shifts = new double[] { 1, 1, 1, 1 };
            double[] factors = new double[] { 1e-3, 2e-2, 3e-1, 4  };

            FunctionWithGradient f = new QuadraticFunctionShifted(shifts, factors);

            double[] initialGuess = new double[] {
                9.4, 
                132.1, 
                55.2, 
                3.14
            };

            int dim = initialGuess.Length;

            int numHessianCorrections = 7;
            if (numHessianCorrections > dim)
                numHessianCorrections = dim;

            double tolg = 1.0e-6;
            double tolf = 0;
            double tolx = 0;
            int maxIt = 300;
            int flags = 0;

            lbfgs.lbfgsstate state = new lbfgs.lbfgsstate();
            lbfgs.lbfgsreport rep = new lbfgs.lbfgsreport();


            lbfgs.minlbfgs(
                dim,
                numHessianCorrections,
                ref initialGuess,
                tolg,
                tolf,
                tolx,
                maxIt,
                flags,
                ref state);

            // lbfgs.minlbfgs(dim, numHessianCorrections, ref initialGuess, 0.0, 0.0, 0.0001, 0, 0, ref state);

            int numIt = 0;

            while (lbfgs.minlbfgsiteration(ref state))
            {

                ++numIt;

                // Calculate function and its gradient by a function object and store them in the state struct:
                state.f = f.Value(state.x);
                state.g = f.Gradient(state.x);

                Console.WriteLine("It. {0}: f(x) = {1,8:F4}, ||grad f(x)|| = {2,8:F5} ", numIt, state.f, FunctionWithGradient.Norm(state.g));
                Console.WriteLine("  x = " + FunctionWithGradient.ArrayToString(state.x));

            }
            lbfgs.minlbfgsresults(ref state, ref initialGuess, ref rep);

            //
            // Uutput results:
            //

            double[] errors = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                errors[i] = state.x[i] - shifts[i];
            }

            Console.WriteLine();
            Console.Write("Function: ");
            Console.WriteLine("  " + f.Description);
            Console.WriteLine("Minimum:");
            Console.WriteLine("  x = " + FunctionWithGradient.ArrayToString(state.x));
            Console.WriteLine("    expected: " + FunctionWithGradient.ArrayToString(shifts));
            Console.WriteLine("    errors: " + FunctionWithGradient.ArrayToString(errors));
            Console.WriteLine();
            Console.WriteLine("Error norm:  {0,12:F8} ", FunctionWithGradient.Norm(errors));
            Console.WriteLine();
            Console.WriteLine("Gradient norm: {0} ", FunctionWithGradient.Norm(state.g));
            Console.WriteLine();
        }  // OptimizeRosenbrock()


        #endregion OptQuadratic


        #region OptRosenbrock

        /// <summary>Example: minimization of Rosenbrock function.</summary>
        /// <param name="useFunctions">If true then function object is used for definition of the minimized function.
        /// Otherwise, function definition is hard/coded at the place where function value and gradient are calculated.d</param>
        public static void OptimizeRosenbrock(bool useFunctions)
        {

            RosenbrockFunction f = new RosenbrockFunction();

            int dim = 2;
            int numHessianCorrections = 2;
            double tolg = 1.0e-6;
            double tolf = 0;
            double tolx = 0;
            int maxIt = 300;
            int flags = 0;



            double[] initialGuess = new double[0];
            double x = 0;
            double y = 0;


            initialGuess = new double[dim];

            //initialGuess[0] = Mathe.RandomReal() - 0.5;
            //initialGuess[1] = Mathe.RandomReal() - 0.5;

            initialGuess[0] = 30.38;
            initialGuess[1] = 44.825;

            //initialGuess[0] = 5.38;
            //initialGuess[1] = 4.825;

            //initialGuess[0] = 0.22;
            //initialGuess[1] = 0.58;

            lbfgs.lbfgsstate state = new lbfgs.lbfgsstate();
            lbfgs.lbfgsreport rep = new lbfgs.lbfgsreport();


            lbfgs.minlbfgs(
                dim,
                numHessianCorrections,
                ref initialGuess,
                tolg,
                tolf,
                tolx,
                maxIt,
                flags,
                ref state);

            // lbfgs.minlbfgs(dim, numHessianCorrections, ref initialGuess, 0.0, 0.0, 0.0001, 0, 0, ref state);

            while (lbfgs.minlbfgsiteration(ref state))
            {
                x = state.x[0];
                y = state.x[1];

                if (useFunctions)
                {
                    // Calculate function and its gradient by a function object and store them in the state struct:
                    state.f = f.Value(state.x);
                    state.g = f.Gradient(state.x);
                }
                else
                {

                    // Alternative: calculate function and its gradient manually:
                    state.f = Mathe.Sqr(1 - x) + 100.0 * Mathe.Sqr(y - Mathe.Sqr(x));
                    state.g[0] = 2.0 * (x - 1) + 400.0 * x * (Mathe.Sqr(x) - y);
                    state.g[1] = 200.0 * (y - Mathe.Sqr(x));
                }
                Console.WriteLine("x = {0,8:F4}, y = {1,8:F4}, f(x,y) = {2,8:F4}, ||grad f|| = {3,8:F5} ", 
                    x, y, state.f, FunctionWithGradient.Norm(state.g));
            }
            lbfgs.minlbfgsresults(ref state, ref initialGuess, ref rep);

            //
            // output results
            //
            Console.WriteLine();
            Console.Write("Function: ");
            Console.WriteLine("  " + f.Description);
            Console.WriteLine("Minimum:");
            Console.WriteLine("  x = {0,8:F4}, y = {1,8:F4} ", x, y);
            Console.Write("    expected: x = 1, y = 1  ");
            Console.WriteLine();

        }  // OptimizeRosenbrock()


        #endregion OptRosenbrock


        #region OptRosenbrockOriginal


        /// <summary>Original example of minimizing the Rosenbrock function.</summary>
        public static void OptimizeRosenbrockOriginal()
        {
            int n = 0;
            int m = 0;
            lbfgs.lbfgsstate state = new lbfgs.lbfgsstate();
            lbfgs.lbfgsreport rep = new lbfgs.lbfgsreport();
            double[] s = new double[0];
            double x = 0;
            double y = 0;


            //
            // Function minimized:
            //     F = exp(x-1) + exp(1-x) + (y-x)^2
            // N = 2 - task dimension
            // M = 1 - build tank-1 model
            //
            n = 2;
            m = 1;
            s = new double[2];
            s[0] = Mathe.RandomReal() - 0.5;
            s[1] = Mathe.RandomReal() - 0.5;

            s[0] = 54.4562;
            s[1] = 23.924;

            lbfgs.minlbfgs(n, m, ref s, 0.0, 0.0, 0.0001, 0, 0, ref state);
            while (lbfgs.minlbfgsiteration(ref state))
            {
                x = state.x[0];
                y = state.x[1];

                Console.WriteLine("x = " + x + ", y = " + y);

                state.f = Mathe.Sqr(1 - x) + 100.0 * Mathe.Sqr(y - Mathe.Sqr(x));
                state.g[0] = 2.0 * (x - 1) + 400.0 * x * (Mathe.Sqr(x) - y);
                state.g[1] = 200.0 * (y - Mathe.Sqr(x));
            }
            lbfgs.minlbfgsresults(ref state, ref s, ref rep);

            //
            // output results
            //
            Console.Write("F = Sqr(1 - x) + 100.0 * Sqr(y - Sqr(x))");
            Console.WriteLine();
            Console.Write("X = ");
            Console.Write("{0,4:F2}", s[0]);
            Console.Write(" (pricakovano bi bilo 1.00)");
            Console.WriteLine();
            Console.Write("Y = ");
            Console.Write("{0,4:F2}", s[1]);
            Console.Write(" (pricakovano bi bilo 1.00)");
            Console.WriteLine();
        }


        #endregion OptRosenbrockOriginal



    } // class ExampleAlgLib2008


}


