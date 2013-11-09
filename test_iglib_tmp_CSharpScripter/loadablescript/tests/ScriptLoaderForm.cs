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

    public partial class ScriptLoaderForm : Form
    {
        /// <summary>Initializes form components and internal variables.</summary>
        public ScriptLoaderForm()
        {
            InitializeComponent();
            Reporter.ReportingLevel = ReportLevel.Warning;
        }


        protected ScriptLoaderIGLib.ScriptLoaderTest Compiler = new ScriptLoaderIGLib.ScriptLoaderTest();

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


        /// <summary>Loads code template into the rich textbox.</summary>
        private void btnLoad_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.rtfCode.Text = Compiler.Code;  // .GetCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }  // btnLoad_Click()


        private string _filePath = "../../loadablescript/tests/data/";

        private bool _fileChosen = false;

        private void ChooseFile()
        {

            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());

            _fileChosen = false;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = false;
            string filePath = Path.GetFullPath(_filePath);
            string dirPath = Path.GetDirectoryName(filePath);
            if ((!string.IsNullOrEmpty(filePath)) && Directory.Exists(dirPath))
            {
                dialog.InitialDirectory = dirPath;
                // dialog.FileName = Path.GetFileName(filePath);
            }
            else
            {
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                // dialog.FileName = "ExampleFile.txt";
            }
            Console.WriteLine(Environment.NewLine + "Initial directory: " + dialog.InitialDirectory + Environment.NewLine);
            DialogResult res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                filePath = Path.GetFullPath(dialog.FileName);
                if (File.Exists(filePath))
                {
                    _fileChosen = true;
                    _filePath = filePath;
                }
            }
        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            try
            {
                ChooseFile();
                if (_fileChosen)
                {

                    StreamReader reader = null;
                    // open selected file
                    reader = File.OpenText(_filePath); // File.OpenText( openFileDialog.FileName );
                    rtfCode.Text = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }



        /// <summary>Compilation of code.</summary>
        private void btnCompile_Click(object sender, System.EventArgs e)
        {
            bool useMessageBox = Reporter.UseMessageBox;
            try
            {
                //try
                //{
                Compiler.ClearLogger();

                Compiler.Code = rtfCode.Text;
                Compiler.Compile();

                Console.WriteLine();
                Console.WriteLine("Compiling has completed.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            finally
            {
                rtfCode.Text = Compiler.Code;  // to see eventual changes in code if they were performed by the compiler
                Reporter.UseMessageBox = useMessageBox;
 
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
                }
                else if (Logger.HasWarnings())
                {
                    string report = Logger.GetWarningsReport();
                    MessageBox.Show(this,
                            "The following warnings occurred during compilation: " + Environment.NewLine
                            + report,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Logger.Clear();


            }
        }



        private void btnRun_Click(object sender, System.EventArgs e)
        {
            try
            {
                string[] initArguments = UtilStr.GetArgumentsArray(txtInitArgs.Text);
                string[] runArguments = UtilStr.GetArgumentsArray(txtRunArgs.Text);
                Compiler.Run(initArguments, runArguments);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
        



        private void btnQuit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void btnOther_Click(object sender, EventArgs e)
        {
            TestLoadableScriptInterpreter.RunTest();
        }



    }
}
