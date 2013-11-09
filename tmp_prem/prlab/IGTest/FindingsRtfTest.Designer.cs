namespace IGTest
{
    partial class FindingsRtfTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMacro = new System.Windows.Forms.Label();
            this.rtMacro = new System.Windows.Forms.RichTextBox();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.btnImportInput = new System.Windows.Forms.Button();
            this.btnImportOutput = new System.Windows.Forms.Button();
            this.btnSaveOutput = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnImportInputRtf = new System.Windows.Forms.Button();
            this.btnImportOutputRtf = new System.Windows.Forms.Button();
            this.btnSaveOutputRtf = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMacro
            // 
            this.lblMacro.AutoSize = true;
            this.lblMacro.Location = new System.Drawing.Point(9, 205);
            this.lblMacro.Name = "lblMacro";
            this.lblMacro.Size = new System.Drawing.Size(125, 13);
            this.lblMacro.TabIndex = 0;
            this.lblMacro.Text = "Macroscopic description:";
            // 
            // rtMacro
            // 
            this.rtMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtMacro.EnableAutoDragDrop = true;
            this.rtMacro.ImeMode = System.Windows.Forms.ImeMode.On;
            this.rtMacro.Location = new System.Drawing.Point(12, 221);
            this.rtMacro.Name = "rtMacro";
            this.rtMacro.Size = new System.Drawing.Size(761, 315);
            this.rtMacro.TabIndex = 1;
            this.rtMacro.Text = "<< There is currently no text imported. >>";
            // 
            // lblInputFile
            // 
            this.lblInputFile.AutoSize = true;
            this.lblInputFile.Location = new System.Drawing.Point(9, 117);
            this.lblInputFile.Name = "lblInputFile";
            this.lblInputFile.Size = new System.Drawing.Size(53, 13);
            this.lblInputFile.TabIndex = 2;
            this.lblInputFile.Text = "Input File:";
            // 
            // txtInputFile
            // 
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(12, 135);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(757, 20);
            this.txtInputFile.TabIndex = 3;
            this.txtInputFile.Text = "c:\\DevProjects\\LABEX\\LabexSolution\\00_Utilities\\LabexBis\\schemas\\FindingsHistoPat" +
                "ological_Example.xml";
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(9, 158);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(61, 13);
            this.lblOutputFile.TabIndex = 2;
            this.lblOutputFile.Text = "Output File:";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFile.Location = new System.Drawing.Point(12, 174);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(757, 20);
            this.txtOutputFile.TabIndex = 3;
            this.txtOutputFile.Text = "c:\\DevProjects\\LABEX\\LabexSolution\\00_Utilities\\LabexBis\\schemas\\FindingsHistoPat" +
                "ological_Out.xml";
            // 
            // btnImportInput
            // 
            this.btnImportInput.Location = new System.Drawing.Point(12, 12);
            this.btnImportInput.Name = "btnImportInput";
            this.btnImportInput.Size = new System.Drawing.Size(97, 23);
            this.btnImportInput.TabIndex = 4;
            this.btnImportInput.Text = "Import input file";
            this.btnImportInput.UseVisualStyleBackColor = true;
            this.btnImportInput.Click += new System.EventHandler(this.btnImportInput_Click);
            // 
            // btnImportOutput
            // 
            this.btnImportOutput.Location = new System.Drawing.Point(140, 12);
            this.btnImportOutput.Name = "btnImportOutput";
            this.btnImportOutput.Size = new System.Drawing.Size(100, 23);
            this.btnImportOutput.TabIndex = 4;
            this.btnImportOutput.Text = "Import output file";
            this.btnImportOutput.UseVisualStyleBackColor = true;
            this.btnImportOutput.Click += new System.EventHandler(this.btnImportOutput_Click);
            // 
            // btnSaveOutput
            // 
            this.btnSaveOutput.Location = new System.Drawing.Point(274, 12);
            this.btnSaveOutput.Name = "btnSaveOutput";
            this.btnSaveOutput.Size = new System.Drawing.Size(111, 23);
            this.btnSaveOutput.TabIndex = 4;
            this.btnSaveOutput.Text = "Save to output file";
            this.btnSaveOutput.UseVisualStyleBackColor = true;
            this.btnSaveOutput.Click += new System.EventHandler(this.btnSaveOutput_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClose.Location = new System.Drawing.Point(680, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close the form";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnClear
            // 
            this.btnClear.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClear.Location = new System.Drawing.Point(12, 70);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(390, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear Macro desc.";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnImportInputRtf
            // 
            this.btnImportInputRtf.Location = new System.Drawing.Point(12, 41);
            this.btnImportInputRtf.Name = "btnImportInputRtf";
            this.btnImportInputRtf.Size = new System.Drawing.Size(122, 23);
            this.btnImportInputRtf.TabIndex = 4;
            this.btnImportInputRtf.Text = "Import input file, RTF";
            this.btnImportInputRtf.UseVisualStyleBackColor = true;
            this.btnImportInputRtf.Click += new System.EventHandler(this.btnImportInputRtf_Click);
            // 
            // btnImportOutputRtf
            // 
            this.btnImportOutputRtf.Location = new System.Drawing.Point(140, 41);
            this.btnImportOutputRtf.Name = "btnImportOutputRtf";
            this.btnImportOutputRtf.Size = new System.Drawing.Size(128, 23);
            this.btnImportOutputRtf.TabIndex = 4;
            this.btnImportOutputRtf.Text = "Import output file, RTF";
            this.btnImportOutputRtf.UseVisualStyleBackColor = true;
            this.btnImportOutputRtf.Click += new System.EventHandler(this.btnImportOutputRtf_Click);
            // 
            // btnSaveOutputRtf
            // 
            this.btnSaveOutputRtf.Location = new System.Drawing.Point(274, 41);
            this.btnSaveOutputRtf.Name = "btnSaveOutputRtf";
            this.btnSaveOutputRtf.Size = new System.Drawing.Size(128, 23);
            this.btnSaveOutputRtf.TabIndex = 4;
            this.btnSaveOutputRtf.Text = "Save to output file, RTF";
            this.btnSaveOutputRtf.UseVisualStyleBackColor = true;
            this.btnSaveOutputRtf.Click += new System.EventHandler(this.btnSaveOutputRtf_Click);
            // 
            // FindingsRtfTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 548);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveOutputRtf);
            this.Controls.Add(this.btnSaveOutput);
            this.Controls.Add(this.btnImportOutputRtf);
            this.Controls.Add(this.btnImportOutput);
            this.Controls.Add(this.btnImportInputRtf);
            this.Controls.Add(this.btnImportInput);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.lblOutputFile);
            this.Controls.Add(this.lblInputFile);
            this.Controls.Add(this.rtMacro);
            this.Controls.Add(this.lblMacro);
            this.Name = "FindingsRtfTest";
            this.Text = "FindingsRtfTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMacro;
        private System.Windows.Forms.RichTextBox rtMacro;
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.Button btnImportInput;
        private System.Windows.Forms.Button btnImportOutput;
        private System.Windows.Forms.Button btnSaveOutput;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnImportInputRtf;
        private System.Windows.Forms.Button btnImportOutputRtf;
        private System.Windows.Forms.Button btnSaveOutputRtf;
    }
}