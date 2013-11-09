using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using Microsoft.CSharp;


using IG.Lib;
using IG.Forms;

namespace IG.Test
{

    public class RuntimeCompilerMain
    {
        /// <summary>Main program.</summary>
        [STAThread]
        static void Main()
        {

            bool useOldForm = false;  // whether to use the old (original) scripter & form.

            // Logging to console of all LogRecords created:
            LogRecord.LogToConsole = false;

            if (useOldForm)
            {
                CompilerFormOld form = new CompilerFormOld();
                // Reporting level of Compiler Form's reporter:
                form.Reporter.ReportingLevel = ReportLevel.Warning;
                Application.Run(form);
            }
            else
            {
                ScriptLoaderForm form = new ScriptLoaderForm();
                // Reporting level of Compiler Form's reporter:
                form.Reporter.ReportingLevel = ReportLevel.Warning;
                Application.Run(form);
            }



            // Application.Run(new FormScriptCompile());

        }

    }

}