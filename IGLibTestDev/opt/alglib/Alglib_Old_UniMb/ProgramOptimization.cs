using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumLib
{

    class ProgramOptimization
    {


        public static void Main(string[] args)
        {


            Console.WriteLine(Environment.NewLine + "Optimizing Rosenbrock function, hard-coded definition: " + Environment.NewLine);
            ExampleAlgLib2008.OptimizeQuadraticWithObject();

            //Console.WriteLine(Environment.NewLine + "Optimizing Rosenbrock function, hard-coded definition: " + Environment.NewLine);
            //ExampleAlgLib2008.OptimizeQuadratic();

            //Console.WriteLine(Environment.NewLine + "Optimizing Rosenbrock function, hard-coded definition: " + Environment.NewLine);
            //ExampleAlgLib2008.OptimizeRosenbrock(false);

            //Console.WriteLine(Environment.NewLine + "Optimizing Rosenbrock function, definition through function object: " + Environment.NewLine);
            //ExampleAlgLib2008.OptimizeRosenbrock(true);

            //Console.WriteLine(Environment.NewLine + "Optimizing Rosenbrock function, original example: " + Environment.NewLine);
            //ExampleAlgLib2008.OptimizeRosenbrockOriginal();



        } // Main


    } // class ProgramOptimization

}
