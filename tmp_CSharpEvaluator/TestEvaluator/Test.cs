using System;
using ADOGuy;

namespace TestEvaluator
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Test
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
      Console.WriteLine("Test0: {0}", Evaluator.EvaluateToInteger("(30 + 4) * 2"));
      Console.WriteLine("Test1: {0}", Evaluator.EvaluateToString("\"Hello \" + \"There\""));
      Console.WriteLine("Test2: {0}", Evaluator.EvaluateToBool("30 == 40"));
      Console.WriteLine("Test3: {0}", Evaluator.EvaluateToObject("new DataSet()"));

      EvaluatorItem[] items = {
                                new EvaluatorItem(typeof(int), "(30 + 4) * 2", "GetNumber"),
                                new EvaluatorItem(typeof(string), "\"Hello \" + \"There\"", "GetString"),
                                new EvaluatorItem(typeof(bool), "30 == 40", "GetBool"),
                                new EvaluatorItem(typeof(object), "new DataSet()", "GetDataSet")
                              };
      
      Evaluator eval = new Evaluator(items);
      Console.WriteLine("TestStatic0: {0}", eval.EvaluateInt("GetNumber"));
      Console.WriteLine("TestStatic1: {0}", eval.EvaluateString("GetString"));
      Console.WriteLine("TestStatic2: {0}", eval.EvaluateBool("GetBool"));
      Console.WriteLine("TestStatic3: {0}", eval.Evaluate("GetDataSet"));
    }
	}
}
