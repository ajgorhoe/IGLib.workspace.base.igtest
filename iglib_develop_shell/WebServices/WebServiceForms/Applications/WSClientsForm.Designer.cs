
using IG.Lib;
using IG.Num;


namespace IG.Web.Forms
{
    partial class WSClientsForm
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
            this.tbInfoLine = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpPrediction = new System.Windows.Forms.TabPage();
            this.btnSavePrediction = new System.Windows.Forms.Button();
            this.btnPrintPrediction = new System.Windows.Forms.Button();
            this.neuralPredictionControl1 = new IG.Web.Forms.WSClient1Control();
            this.tpParmTest = new System.Windows.Forms.TabPage();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.wsClientFormsAboutForm1 = new IG.Web.Forms.WSClientFormsAboutForm();
            this.tabControl.SuspendLayout();
            this.tpPrediction.SuspendLayout();
            this.tpAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbInfoLine
            // 
            this.tbInfoLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInfoLine.BackColor = System.Drawing.SystemColors.Control;
            this.tbInfoLine.Location = new System.Drawing.Point(0, 563);
            this.tbInfoLine.Margin = new System.Windows.Forms.Padding(2);
            this.tbInfoLine.Name = "tbInfoLine";
            this.tbInfoLine.Size = new System.Drawing.Size(1132, 20);
            this.tbInfoLine.TabIndex = 7;
            this.tbInfoLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl.Controls.Add(this.tpPrediction);
            this.tabControl.Controls.Add(this.tpParmTest);
            this.tabControl.Controls.Add(this.tpAbout);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1136, 559);
            this.tabControl.TabIndex = 10;
            this.tabControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyUp);
            // 
            // tpPrediction
            // 
            this.tpPrediction.BackColor = System.Drawing.SystemColors.Control;
            this.tpPrediction.Controls.Add(this.btnSavePrediction);
            this.tpPrediction.Controls.Add(this.btnPrintPrediction);
            this.tpPrediction.Controls.Add(this.neuralPredictionControl1);
            this.tpPrediction.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tpPrediction.Location = new System.Drawing.Point(4, 25);
            this.tpPrediction.Margin = new System.Windows.Forms.Padding(2);
            this.tpPrediction.Name = "tpPrediction";
            this.tpPrediction.Padding = new System.Windows.Forms.Padding(2);
            this.tpPrediction.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tpPrediction.Size = new System.Drawing.Size(1128, 530);
            this.tpPrediction.TabIndex = 0;
            this.tpPrediction.Text = "WS Client 1";
            // 
            // btnSavePrediction
            // 
            this.btnSavePrediction.Location = new System.Drawing.Point(969, 590);
            this.btnSavePrediction.Name = "btnSavePrediction";
            this.btnSavePrediction.Size = new System.Drawing.Size(75, 23);
            this.btnSavePrediction.TabIndex = 1;
            this.btnSavePrediction.Text = "Save View";
            this.btnSavePrediction.UseVisualStyleBackColor = true;
            this.btnSavePrediction.Click += new System.EventHandler(this.btnSavePrediction_Click);
            // 
            // btnPrintPrediction
            // 
            this.btnPrintPrediction.Location = new System.Drawing.Point(1050, 590);
            this.btnPrintPrediction.Name = "btnPrintPrediction";
            this.btnPrintPrediction.Size = new System.Drawing.Size(75, 23);
            this.btnPrintPrediction.TabIndex = 1;
            this.btnPrintPrediction.Text = "Print View";
            this.btnPrintPrediction.UseVisualStyleBackColor = true;
            this.btnPrintPrediction.Click += new System.EventHandler(this.btnPrintPrediction_Click);
            // 
            // neuralPredictionControl1
            // 
            this.neuralPredictionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.neuralPredictionControl1.Location = new System.Drawing.Point(0, 0);
            this.neuralPredictionControl1.Name = "neuralPredictionControl1";
            this.neuralPredictionControl1.Size = new System.Drawing.Size(1128, 616);
            this.neuralPredictionControl1.TabIndex = 0;
            // 
            // tpParmTest
            // 
            this.tpParmTest.Location = new System.Drawing.Point(4, 25);
            this.tpParmTest.Margin = new System.Windows.Forms.Padding(2);
            this.tpParmTest.Name = "tpParmTest";
            this.tpParmTest.Padding = new System.Windows.Forms.Padding(2);
            this.tpParmTest.Size = new System.Drawing.Size(1128, 530);
            this.tpParmTest.TabIndex = 1;
            this.tpParmTest.Text = "WS Client Np. 2";
            this.tpParmTest.UseVisualStyleBackColor = true;
            // 
            // tpAbout
            // 
            this.tpAbout.Controls.Add(this.wsClientFormsAboutForm1);
            this.tpAbout.Location = new System.Drawing.Point(4, 25);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tpAbout.Size = new System.Drawing.Size(1128, 530);
            this.tpAbout.TabIndex = 2;
            this.tpAbout.Text = "About";
            this.tpAbout.UseVisualStyleBackColor = true;
            // 
            // wsClientFormsAboutForm1
            // 
            this.wsClientFormsAboutForm1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsClientFormsAboutForm1.Location = new System.Drawing.Point(0, 0);
            this.wsClientFormsAboutForm1.Name = "wsClientFormsAboutForm1";
            this.wsClientFormsAboutForm1.Size = new System.Drawing.Size(1128, 530);
            this.wsClientFormsAboutForm1.TabIndex = 0;
            // 
            // WSClientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1136, 583);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.tbInfoLine);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WSClientsForm";
            this.Text = "NeuralParametricDemo1";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NeuralParametricDemo_KeyUp);
            this.tabControl.ResumeLayout(false);
            this.tpPrediction.ResumeLayout(false);
            this.tpAbout.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInfoLine;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpPrediction;
        private System.Windows.Forms.TabPage tpParmTest;
        private Forms.WSClient1Control neuralPredictionControl1;
        private System.Windows.Forms.Button btnSavePrediction;
        private System.Windows.Forms.Button btnPrintPrediction;
        private System.Windows.Forms.TabPage tpAbout;
        private WSClientFormsAboutForm wsClientFormsAboutForm1;
    }
}