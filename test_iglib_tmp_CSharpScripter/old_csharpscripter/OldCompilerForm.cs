using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

using System.Windows.Forms;

using IG.Lib; 
using IG.Forms;

namespace IG.Forms
{
    public partial class CompilerFormOld : Form
    {
        /// <summary>Initializes form components and internal variables.</summary>
        public CompilerFormOld()
        {
            InitializeComponent();
            Reporter.ReportingLevel = ReportLevel.Warning;
        }


        protected RuntimeCompilerOld Compiler = new RuntimeCompilerOld();

        public ReporterConsoleMsgbox Reporter = new ReporterConsoleMsgbox();

        Logger _logger = null;

        /// <summary>Gets compiler's logger.</summary>
        protected Logger Logger { 
            get 
            {
                if (Compiler.Logger != null)
                    return Compiler.Logger;
                else 
                {
                    if (_logger == null)
                        _logger = new Logger();
                    return _logger;
                }
            } 
        }


        private void btnQuit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>Loads code template into the rich textbox.</summary>
        private void btnLoad_Click(object sender, System.EventArgs e)
        {

            this.rtfCode.Text = Compiler.CodeBase;

        }  // btnLoad_Click()


        /// <summary>Launches error report.</summary>
        /// <param name="message">Custom part of the message shown in report.</param>
        /// <param name="ex">Exception that was thrown.</param>
        protected void ReportError(string message, Exception ex)
        {
            string errorStr = message + " ";
            if (ex != null)
            {
                if (ex.Message != null)
                    errorStr += Environment.NewLine + "Details: " + ex.Message + Environment.NewLine;
                if (ex.InnerException != null) if (ex.InnerException.Message != null)
                    {
                        errorStr += Environment.NewLine + "Cause: " + ex.InnerException.Message + Environment.NewLine;
                    }
                Console.WriteLine();
                Console.WriteLine(errorStr);
                Console.WriteLine();
                MessageBox.Show(this, errorStr,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>Checks whether the compiled code is ready to run, and enables the Run button accordingly.</summary>
        protected void CheckReadyToRun()
        {
            btnExecute.Enabled = Compiler.ReadyToRun;
        }


        /// <summary>Compilation of code.</summary>
        private void btnCompile_Click(object sender, System.EventArgs e)
        {
            bool useMessageBox = Reporter.UseMessageBox;
            try
            {
                Compiler.ReadyToRun = false;
                CheckReadyToRun();

                Compiler.Code = rtfCode.Text;
                Compiler.Compile();

                CheckReadyToRun();  // enable the Run button if compilation was successful (compiler has ready flag)
                // Report eventual errors to console:
                Reporter.UseMessageBox = false;
                Logger.Report(Reporter);
                Reporter.UseMessageBox = useMessageBox;
                if (Logger.HasErrors())
                {
                    string report;
                    if (Logger.HasWarnings())
                        report = Logger.GetReport(ReportLevel.Warning);
                    else
                        report = Logger.GetErrorsReport();
                    MessageBox.Show(this, 
                            "Errors occurred during compilation: " + Environment.NewLine
                            + report,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (Logger.HasWarnings())
                {
                    string report = Logger.GetWarningsReport();
                    MessageBox.Show(this, 
                            "The following warnings occurred during compilation: " + Environment.NewLine
                            + report,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                Reporter.ReportError("Compilation", "Compile-time error.", ex);
            }
            finally
            {
                Reporter.UseMessageBox = useMessageBox;
                Logger.Clear();
                CheckReadyToRun();
            }

            // Delete the dll file if it exists:

            try
            {
            }
            catch (Exception ex)
            {
                ReportError("Error occurred during compiling: ", ex);
            }
            Console.WriteLine();
            Console.WriteLine("Compiling has completed.\n");
        }



        private void btnExecute_Click(object sender, System.EventArgs e)
        {
            AppDomainSetup ads = new AppDomainSetup();
            ads.ShadowCopyFiles = "false";
            // ads.ShadowCopyFiles();
            // AppDomain.CurrentDomain.SetShadowCopyFiles();

            AppDomain newDomain = AppDomain.CreateDomain("newDomain");

            byte[] rawAssembly = Compiler.LoadFile(Compiler.LibraryPath);
            // byte[] rawAssembly = loadFile(Application.StartupPath + "TestClass.dll");
            Assembly assembly = newDomain.Load(rawAssembly, null);

            IRunnableOld testClass = (IRunnableOld)assembly.CreateInstance(Compiler.RunnableFullClass);
            testClass.Run();

            testClass = null;
            assembly = null;

            AppDomain.Unload(newDomain);
            newDomain = null;
        }





    }
}
