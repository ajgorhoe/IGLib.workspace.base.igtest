using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathEval
{
    class Program
    {
        public static bool readdouble(ref double val)
            ///Reads a number from a console; The user can leave the value unchanged by pressing <Enter>
            ///or check the current value any number of times by inserting an invalid string
        {
            string valstr;
            bool err=false;
            double aux=0;
            bool ret = false;
            while (true)
            {
                valstr = Console.ReadLine();
                if (valstr.Length < 1)
                {
                    Console.WriteLine("      = {0}",val);
                    break;
                }
                try
                {
                    aux = double.Parse(valstr);
                }
                catch { err = true; }
                if (!err)
                {
                    // Inserted string is a valid representation of the number:
                    val = aux;
                    ret = true;
                    break;
                }
                // Inserted string is invalid, output the current value and re - read the number
                Console.WriteLine("      = {0}", val);
                Console.Write("\nInput the value of type double, <Enter> to keep the currnt \n  value:  ");
            }
            return ret;
        }
        


        static void Main(string[] args)
        {
            string expression = "Math.Sin(x)*y",input;
            int length;
            double x = 0, y = 0;
            double val;
            MathExpressionParser mp = new MathExpressionParser();
            // mp.init("Math.Sin(x)*y");

            for (int i = 0; i < 1000000; i++)
            {
                if (mp.init(expression))
                {
                    val=mp.eval(x, y);
                    Console.WriteLine("x = {0}, y = {0}, \n  {0} = {0} .\n",x,y,expression,val);
                }  else
                    Console.WriteLine();

                Console.Write("\n\nInsert a new mathematical expression involving variables x and y: \n  f(x, y) = ");
                input=Console.ReadLine();
                if (input.Length > 0)
                    expression = input;
                if (expression != null) length = expression.Length; else length = 0;
                if (length < 1)
                {
                    Console.WriteLine("\n\nThe inserted expression is empty.\n\nFinishing evaluation.\n");
                    break;
                }
                else
                {
                    Console.WriteLine("input the variables!");
                    Console.Write("  x: ");
                    readdouble(ref x);
                    Console.Write("  y: "); readdouble(ref y);
                }

            }



        }
    }
}




