using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MathNet.Numerics.LinearAlgebra;
// using Iridium.Test;

// using MathNet.Numerics;
  


namespace IGTestMathNet
{

    public static class ComplexExt
    {

        public static double Im(this MathNet.Numerics.Complex c)
        {
            return c.Imag;
        }

        public static string ToString(this MathNet.Numerics.Complex c)
        {
            return "{" + c.Real.ToString() + c.Imag.ToString();
        }


    }


    class Program
    {

        static void Main(string[] args)
        {

            MathNet.Numerics.Complex a = new MathNet.Numerics.Complex(10,12);
            Console.WriteLine("a = " + a.ToString());
            Console.WriteLine("a.Im() = " + a.Im() );

            //MatrixTest test=mew MatrixTest();
            //test.TestMatrix_Create();

            // TM_Create();
            
        }

        public static void TM_Create()
        {
            double[][] a = { new double[] { 1, 2 }, new double[] { 2, 3 } };
            Matrix ma = Matrix.Create(a);
            double[][] b = { new double[] { 1.0, 2.0 }, new double[] { 2.0, 3.0 } };
            Matrix mb = Matrix.Create(a);

            Console.WriteLine("Matrix ma:");
            Console.WriteLine(ma.ToString());
            Console.WriteLine("Matrix mb:");
            Console.WriteLine(mb.ToString());
            Console.WriteLine("Matrix ma*mb:");
            Console.WriteLine((ma*mb).ToString());

            //double[] da = new double[]{1,1};

            Vector va = new Vector(new double[]{1,1});
            Console.WriteLine();
            Console.WriteLine("Matrix ma:");
            Console.WriteLine(ma.ToString());
            Console.WriteLine("Vector va:");
            Console.WriteLine(va.ToString());
            Console.WriteLine("ma*va:");
            //Vector vb = ma * va;  // To ne dela - ni množenja vektorjev!

            

            // Assert.IsTrue(ma.Equals(ma), "Matrices should be equal");
        }



    }
}
