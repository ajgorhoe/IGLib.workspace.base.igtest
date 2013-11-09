
// CONTENTS TO BE PUT INTO CSM FILE - CoMmandSharp  file.

using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using IG.Lib;

namespace IG.Script
{

    public class TestClass1 : LoadableScriptBase, ILoadableScript
    {

        public TestClass1()
            : base()
        {  }


        //public TestClass1(string [] args) : base(args)
        //{  }

        /// <summary>Initializes the current object.</summary>
        protected override void InitializeThis(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("TestLoadable object: initialization performed.");
            Console.WriteLine("Initialization arguments: ");
            if (arguments == null)
                Console.WriteLine("  null.");
            else if (arguments.Length == 0)
                Console.WriteLine("  empty array.");
            else
            {
                for (int i = 0; i < arguments.Length; ++i)
                {
                    Console.Write("'" + arguments[i] + "' ");
                }
            }
            Console.WriteLine();
        }

        /// <summary>Runs action of the current object.</summary>
        /// <param name="arguments">Command-line arguments of the action.</param>
        protected override string RunThis(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Loadable object's script is executed.");
            Console.WriteLine("Arguments: ");
            if (arguments == null)
                Console.WriteLine("  null.");
            else if (arguments.Length == 0)
                Console.WriteLine("  empty array.");
            else
            {
                for (int i = 0; i < arguments.Length; ++i)
                {
                    Console.Write("'" + arguments[i] + "' ");
                }
            }
            Console.WriteLine();
            MessageBox.Show("This is a testmessage from the file 'script1.cms'.");
            return null;
        }
        

    }


}



