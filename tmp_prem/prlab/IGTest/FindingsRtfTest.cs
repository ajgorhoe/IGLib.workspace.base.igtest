using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using LabexBis;  // communication with the Birpis system


namespace IGTest
{
    public partial class FindingsRtfTest : Form
    {

        #region Construction

        public FindingsRtfTest()
        {
            InitializeComponent();
        }

        #endregion  // Construction

        private DocFindings _findings = null;

        /// <summary>Findings object used in testing.</summary>
        public DocFindings Findings
        { get { return _findings; } set { _findings = value; } }


        /// <summary>Exit the form.</summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>Clears rich textbox containing Macro description.</summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtMacro.Clear();
        }

        /// <summary>Imports findings from the input file whose path is defined in its textbox.</summary>
        private void btnImportInput_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txtInputFile.Text;
                Findings = null;
                if (!File.Exists(filename))
                    throw new Exception("Input file does not exist: " + filename);
                Findings = new DocFindings();
                Findings.UsePlainText = true;  // indicate that plain text is used for fields that can be formatted
                Findings.Load(filename);
                Findings.Read();
                rtMacro.Text = Findings.MacroDescription;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>Imports findings from the output file whose path is defined in its textbox.</summary>
        private void btnImportOutput_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txtOutputFile.Text;
                Findings = null;
                if (!File.Exists(filename))
                    throw new Exception("Input file does not exist: " + filename);
                Findings = new DocFindings();
                Findings.UsePlainText = true;  // indicate that plain text is used for fields that can be formatted
                Findings.Load(filename);
                Findings.Read();
                rtMacro.Text = Findings.MacroDescription;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>Exports findings to the output file whose path is defined in its textbox.</summary>
        private void btnSaveOutput_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txtOutputFile.Text;
                if (Findings == null)
                    throw new Exception("The findings document is not iitialized.");
                if (string.IsNullOrEmpty(filename))
                    throw new Exception("Output file name is not specified.");
                else
                {
                    string dir = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(dir))
                        throw new Exception("Directory to save the output file to does not exist. Directory name: "
                            + Environment.NewLine + "  \"" + dir + "\"." );
                    if (File.Exists(filename))
                    {
                        DialogResult answer = MessageBox.Show("The following output file already exists: "
                            + Environment.NewLine+ "  \"" + filename + "\"","Owerwrite a file",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                        if (answer == DialogResult.No)
                            return;
                    }
                }
                // TODO: find out how to extract the actual markup from a rich text box!
                Findings.UsePlainText = true;  // indicate that plain text is used for fields that can be formatted
                Findings.MacroDescription = rtMacro.Text;
                Findings.ToXml();
                Findings.Save(filename);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        // DEALING WITH RICH TEXT:


        /// <summary>Imports findings from the input file whose path is defined in its textbox, and loads macro description in RTF.</summary>
        private void btnImportInputRtf_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txtInputFile.Text;
                Findings = null;
                if (!File.Exists(filename))
                    throw new Exception("Input file does not exist: " + filename);
                Findings = new DocFindings();
                Findings.UsePlainText = false;  // indicate that rich text is used for fields that can be formatted
                Findings.Load(filename);
                Findings.Read();
                rtMacro.Rtf = Findings.MacroDescription;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>Imports findings from the output file whose path is defined in its textbox, and loads macro description in RTF.</summary>
        private void btnImportOutputRtf_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txtOutputFile.Text;
                Findings = null;
                if (!File.Exists(filename))
                    throw new Exception("Input file does not exist: " + filename);
                Findings = new DocFindings();
                Findings.UsePlainText = false;  // indicate that rich text is used for fields that can be formatted
                Findings.Load(filename);
                Findings.Read();
                rtMacro.Rtf = Findings.MacroDescription;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        /// <summary>Exports findings to the output file whose path is defined in its textbox; Macro description is stored to XML in RTF.</summary>
        private void btnSaveOutputRtf_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = txtOutputFile.Text;
                if (Findings == null)
                    throw new Exception("The findings document is not iitialized.");
                if (string.IsNullOrEmpty(filename))
                    throw new Exception("Output file name is not specified.");
                else
                {
                    string dir = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(dir))
                        throw new Exception("Directory to save the output file to does not exist. Directory name: "
                            + Environment.NewLine + "  \"" + dir + "\".");
                    if (File.Exists(filename))
                    {
                        DialogResult answer = MessageBox.Show("The following output file already exists: "
                            + Environment.NewLine + "  \"" + filename + "\"", "Owerwrite a file", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (answer == DialogResult.No)
                            return;
                    }
                }
                // TODO: find out how to extract the actual markup from a rich text box!
                Findings.UsePlainText = false;  // indicate that rich text is used for fields that can be formatted
                Findings.MacroDescription = rtMacro.Rtf;
                Findings.ToXml();
                Findings.Save(filename);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
