
// CONTENTS TO BE PUT INTO CSM FILE - CoMmandSharp  file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Lib
{

    public class TestLoadable: LoadableBase, ILoadable, ILockable
    {

        public TestLoadable(string workingDirectory) : base(workingDirectory)
        {  }

        /// <summary>Initializes the current object.</summary>
        protected override void InitializeSpecific()
        {
            Console.WriteLine();
            Console.WriteLine("TestLoadable object: initialization performed.");
            Console.WriteLine();
        }

        /// <summary>Runs action of the current object.</summary>
        /// <param name="arguments">Command-line arguments of the action.</param>
        public override string Execute(string[] arguments)
        {
            Console.WriteLine();
            Console.WriteLine("Object is executed.");
            Console.WriteLine("Arguments: ");
            Console.WriteLine();
            return null;
        }
        

    }


}



