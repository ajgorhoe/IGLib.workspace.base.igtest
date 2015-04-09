using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlgLib;

namespace NumLib
{
    class Program
    {
        public static void function1_grad(double[] x, ref double func, double[] grad, object obj)
        {
            // this callback calculates f(x0,x1) = (1-x0)^2 + 100(x1-x0^2)^2
            // and its derivatives df/d0 and df/dx1
            func = System.Math.Pow(1 - x[0], 2) + 100 * System.Math.Pow(x[1] - x[0] * x[0], 2);
            grad[0] = -2.0 * (1 - x[0]) - 400.0 * x[0] * (-Math.Pow(x[0], 2)+x[1]);
            grad[1] = 200.0*(-Math.Pow(x[0], 2) + x[1]);
        
        }


        //static void Main(string[] args)
        //{
        //    //
        //    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
        //    // using LBFGS method.
        //    //
        //    for (int maxits = 1; maxits < 20; maxits++ )
        //    {
        //        double[] x = new double[] { 0, 0 }; //zacetni vrednosti
        //        double epsg = 0.0000000001;
        //        double epsf = 0;
        //        double epsx = 0;
        //        //int maxits = 0;
                
        //        alglib.minlbfgsstate state;
        //        alglib.minlbfgsreport rep;

        //        alglib.minlbfgscreate(1, x, out state);
        //        alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
        //        alglib.minlbfgsoptimize(state, function1_grad, null, null);
        //        alglib.minlbfgsresults(state, out x, out rep);


        //        //st iteracij
        //        Console.WriteLine("Stevilo iteracij:");
        //        Console.WriteLine(maxits.ToString());
        //        Console.WriteLine();
        //        // izpis vrednosti funkcije
        //        Console.WriteLine("Vrednost funkcije");
        //        double funkcija2 = System.Math.Pow(1 - x[0], 2) + 100 * System.Math.Pow(x[1] - x[0] * x[0], 2);
        //        Console.WriteLine(funkcija2.ToString());
        //        Console.WriteLine();

        //        //izpis odaljenosti od minimuma
        //        Console.WriteLine("Izpis odaljenosti od analitičnega minimuma (1,1):");
        //        Console.WriteLine("Za x:");
        //        Console.WriteLine(Math.Abs(x[0] - 1.0).ToString());
        //        Console.WriteLine("Za y:");
        //        Console.WriteLine(Math.Abs(x[1] - 1.0).ToString());
        //        Console.WriteLine();


        //        //izpis x in y
        //        Console.WriteLine("Izpis x in y:");
        //        Console.WriteLine(x[0].ToString()+" "+x[1].ToString());
        //        Console.WriteLine("--------------------------");

              

        //      //  Console.WriteLine("{0}", alglib.ap.format(x, 2)); // EXPECTED: [1,1]
        //       // Console.ReadLine();
               
        //    }
            
        //}

    }
}
