using System;
using System.Collections.Generic;
using System.Text;

using IG.Lib;

namespace ADOGuy
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("");
            Console.WriteLine("CSharp Expression Evaluator from CofeProject...");
            Console.WriteLine("");

            string expr = "";

            expr = "2.0/3.0";
            Console.WriteLine("Test0: {0} = {1}", expr, Evaluator.EvaluateToDouble(expr));
            expr = "(7 + 1) * 2";
            Console.WriteLine("Test0: {0} = {1}", expr, Evaluator.EvaluateToInteger(expr));
            expr = "\"Hello \" + \"There\"";
            Console.WriteLine("Test1: {0} = {1}", expr, Evaluator.EvaluateToString(expr));
            expr = "30 == 40";
            Console.WriteLine("Test2: {0} = {1}", expr, Evaluator.EvaluateToBool(expr));
            expr = "new DataSet()";
            Console.WriteLine("Test3: {0} = {1}", expr, Evaluator.EvaluateToObject(expr));

            Console.WriteLine();

            expr = "(30 + 4) * 2";
            Console.WriteLine("EvaluateToObject: {0} = {1}", expr, Evaluator.EvaluateToObject(expr));

            Console.WriteLine();

            EvaluatorItem[] items = {
                          new EvaluatorItem(typeof(object), "2*1.1111", "objectExpr"),
                          new EvaluatorItem(typeof(int), "(30 + 4) * 2", "intNum1"),
                          new EvaluatorItem(typeof(string), "\"Hello \" + \"There\"", 
                                                            "StringExpr1"),
                          new EvaluatorItem(typeof(bool), "30 == 40", "bVar"),
                          new EvaluatorItem(typeof(object), "new DataSet()", "ds")
                        };

            Evaluator eval = new Evaluator(items);

            double d = (double) eval.Evaluate("objectExpr");
            Console.WriteLine();
            Console.WriteLine("Explicit cast of value of objectExpr to double: {0}", d);
            Console.WriteLine();

            Console.WriteLine("objectExpr: {0}", eval.Evaluate("objectExpr"));
            Console.WriteLine("intNum1: {0}", eval.EvaluateInt("intNum1"));
            Console.WriteLine("StringExpr1: {0}", eval.EvaluateString("StringExpr1"));
            Console.WriteLine("bVar: {0}", eval.EvaluateBool("bVar"));
            Console.WriteLine("ds: {0}", eval.Evaluate("ds"));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Test whether recompiling functinos with the same name can give different results:");
            Console.WriteLine();


            items = new EvaluatorItem[] {
                          new EvaluatorItem(typeof(object), "0.0001", "objectExpr"),
                          new EvaluatorItem(typeof(int), "22222", "intNum1"),
                          new EvaluatorItem(typeof(string), "\"Hello \"", 
                                                            "StringExpr1"),
                          new EvaluatorItem(typeof(bool), "true", "bVar"),
                          new EvaluatorItem(typeof(object), "new DataSet()", "ds")
                        };

            eval = new Evaluator(items);

            Console.WriteLine("objectExpr: {0}", eval.Evaluate("objectExpr"));
            Console.WriteLine("intNum1: {0}", eval.EvaluateInt("intNum1"));
            Console.WriteLine("StringExpr1: {0}", eval.EvaluateString("StringExpr1"));
            Console.WriteLine("bVar: {0}", eval.EvaluateBool("bVar"));
            Console.WriteLine("ds: {0}", eval.Evaluate("ds"));

        }
    }
}
