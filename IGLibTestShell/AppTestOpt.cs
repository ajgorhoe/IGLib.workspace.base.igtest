
// NEURAL NETWORKS TRAINING DATA

using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Text;
using System;
using System.Collections.Generic;

using IG.Lib;
using IG.Num;
using IG.Neural;

namespace IG.Lib
{

    public class AppTestOpt : NeuralApplicationInterpreter // AnalysisFileServerNeural
    {

        public AppTestOpt()
            : this(false)
        { }

        public AppTestOpt(bool caseSensitive)
            : base(caseSensitive)
        {

            // this.AddModule("", AnalysisFileServerNeural_ijs_11_05.InstallCommands_ijs_11_05 );

            // AnalysisFileServerNeuralExtBase.InstallCommands_ijs_11_05(this);

            //this.AddCommand("PerformAnalysis_ijs_11_05", CmdAnalyseServer_ijs_11_05);
            //this.AddCommand("PerformAnalysisClient_ijs_11_05", CmdAnalyseClient_ijs_11_05);


        }



        /// <summary>Execution method that prints some information about the application.</summary>
        /// <param name="thread">Interpreter thread on which commad is run.</param>
        /// <param name="cmdName">Command name.</param>
        /// <param name="args">Command arguments.</param>
        protected override string CmdAbout(CommandThread thread,
            string cmdName, string[] args)
        {
            if (args != null)
                if (args.Length > 0)
                    if (args[0] == "?")
                    {
                        return base.CmdAbout(thread, cmdName, args);
                    }
            string retBase = base.CmdAbout(thread, cmdName, args);
            return "This is a prototypic application, written by Igor Grešovnik and others."
                    + Environment.NewLine + retBase;
        }




    }


}