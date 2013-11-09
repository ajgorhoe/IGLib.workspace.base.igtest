using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using IG.Lib;
using IG.Forms;
using IG.Gr;
using IG.Num;

namespace IG.Web.Forms
{

    /// <summary>Form for parametric tests and other viewing operations performed on ANN models.</summary>
    /// $A Igor Apr13;
    public partial class WSClientsForm : Form
    {

        public WSClientsForm()
        {
            InitializeComponent();
        }


        protected string _workingDirectory = Directory.GetCurrentDirectory();

        /// <summary>Working directory of the outer form.</summary>
        public string WorkingDirectory
        {
            get { return _workingDirectory; }
            set { _workingDirectory = value; }
        }


        #region About

        protected WSClientFormsAboutWindow _aboutWindow;

        /// <summary>Window that shows information about the software.
        /// <para>Help is also accessible through that window.</para></summary>
        public WSClientFormsAboutWindow AboutWindow
        {
            get
            {
                if (_aboutWindow == null)
                {
                    _aboutWindow = new WSClientFormsAboutWindow();
                }
                return _aboutWindow;
            }
        }

        private void NeuralParametricDemo_HelpEventHandlers(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (e.Control && e.Shift)
                {
                    AboutWindow.ShowDialog();
                }
                else
                    AboutWindow.ShowHelp();
            }
        }

        #endregion About

        private void NeuralParametricDemo_KeyUp(object sender, KeyEventArgs e)
        {
            this.NeuralParametricDemo_HelpEventHandlers(sender, e);
        }

        private void tabControl_KeyUp(object sender, KeyEventArgs e)
        {
            this.NeuralParametricDemo_HelpEventHandlers(sender, e);
        }



        // TODO:
        // Gumbi za shranjevanje in printanje:
        // To je provizorična rešitev, pozneje je to treba prestaviti v meni ali v kontekstni meni,
        // tuidi same operacije je treba implementirati na drug na;in (zdaj se sprinta ali shrane 
        // kar vizualna kontrola).

        /// <summary>Saves image of the prediction control to a file that is chosen via a file dialog box.</summary>
        private void btnSavePrediction_Click(object sender, EventArgs e)
        {
            string filePath = UtilForms.SaveControlFileDialogJpeg(neuralPredictionControl1, WorkingDirectory);
            if (!string.IsNullOrEmpty(filePath))
            {
                string dirPath = Path.GetDirectoryName(filePath);
                if (dirPath!=WorkingDirectory)
                    WorkingDirectory = dirPath;
            }
        }

        /// <summary>Prints the image of the prediction control to a file that is chosen via a file dialog box.</summary>
        private void btnPrintPrediction_Click(object sender, EventArgs e)
        {
            UtilForms.PrintControl(neuralPredictionControl1);
        }

        /// <summary>Saves image of the parametric test control to a file that is chosen via a file dialog box.</summary>
        private void btnSaveParametricTest_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>Prints the image of the parametric test control to a file that is chosen via a file dialog box.</summary>
        private void btnPrintParametricTest_Click(object sender, EventArgs e)
        {
        }



    }
}
