namespace IG.Forms
{
    partial class MathematicaCalculatorControl
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


        #region Component Designer generated code


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MathematicaCalculatorControl));
            this.lblInput = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.ComboBox();
            this.btnEvaluate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.textResult = new System.Windows.Forms.TextBox();
            this.lblPrint = new System.Windows.Forms.Label();
            this.txtPrint = new System.Windows.Forms.TextBox();
            this.lblGraphics = new System.Windows.Forms.Label();
            this.imgGraphics = new System.Windows.Forms.PictureBox();
            this.lblMessages = new System.Windows.Forms.Label();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.chkNumeric = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgGraphics)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(3, 14);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(34, 13);
            this.lblInput.TabIndex = 0;
            this.lblInput.Text = "Input:";
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Items.AddRange(new object[] {
            "Cos[6*Pi]",
            "Table[Sin[x/20],{x, 1, 100}]",
            "varX = 22; varY = 23; Sin[varX*varY]",
            "varX = 3;  Print[\"Sin[\", varX, \"] = \", Sin[varX] ]   ",
            "Plot[ Sin[Exp[x]] * x^2 , {x,0,5}]",
            "Plot3D[ x*x*y, {x, -1, 1}, {y, -1, 1} ]",
            "Plot[ Sin[Exp[x]] * x^2 , {x,0,5}]"});
            this.txtInput.Location = new System.Drawing.Point(7, 30);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(516, 21);
            this.txtInput.TabIndex = 1;
            this.txtInput.Text = "Cos[Pi/3]";
            this.txtInput.SelectedIndexChanged += new System.EventHandler(this.txtInput_SelectedIndexChanged);
            // 
            // btnEvaluate
            // 
            this.btnEvaluate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEvaluate.Location = new System.Drawing.Point(529, 30);
            this.btnEvaluate.Name = "btnEvaluate";
            this.btnEvaluate.Size = new System.Drawing.Size(68, 20);
            this.btnEvaluate.TabIndex = 2;
            this.btnEvaluate.Text = "Evaluate";
            this.btnEvaluate.UseVisualStyleBackColor = true;
            this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(3, 60);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(40, 13);
            this.lblResult.TabIndex = 0;
            this.lblResult.Text = "Result:";
            // 
            // textResult
            // 
            this.textResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textResult.Location = new System.Drawing.Point(7, 76);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResult.Size = new System.Drawing.Size(592, 68);
            this.textResult.TabIndex = 3;
            // 
            // lblPrint
            // 
            this.lblPrint.AutoSize = true;
            this.lblPrint.Location = new System.Drawing.Point(3, 238);
            this.lblPrint.Name = "lblPrint";
            this.lblPrint.Size = new System.Drawing.Size(31, 13);
            this.lblPrint.TabIndex = 0;
            this.lblPrint.Text = "Print:";
            // 
            // txtPrint
            // 
            this.txtPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrint.Location = new System.Drawing.Point(7, 254);
            this.txtPrint.Multiline = true;
            this.txtPrint.Name = "txtPrint";
            this.txtPrint.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPrint.Size = new System.Drawing.Size(591, 95);
            this.txtPrint.TabIndex = 3;
            // 
            // lblGraphics
            // 
            this.lblGraphics.AutoSize = true;
            this.lblGraphics.Location = new System.Drawing.Point(3, 352);
            this.lblGraphics.Name = "lblGraphics";
            this.lblGraphics.Size = new System.Drawing.Size(52, 13);
            this.lblGraphics.TabIndex = 0;
            this.lblGraphics.Text = "Graphics:";
            // 
            // imgGraphics
            // 
            this.imgGraphics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgGraphics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgGraphics.Image = ((System.Drawing.Image)(resources.GetObject("imgGraphics.Image")));
            this.imgGraphics.InitialImage = null;
            this.imgGraphics.Location = new System.Drawing.Point(7, 368);
            this.imgGraphics.Name = "imgGraphics";
            this.imgGraphics.Size = new System.Drawing.Size(587, 256);
            this.imgGraphics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgGraphics.TabIndex = 4;
            this.imgGraphics.TabStop = false;
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(4, 147);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(58, 13);
            this.lblMessages.TabIndex = 0;
            this.lblMessages.Text = "Messages:";
            // 
            // txtMessages
            // 
            this.txtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessages.Location = new System.Drawing.Point(6, 163);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessages.Size = new System.Drawing.Size(592, 72);
            this.txtMessages.TabIndex = 3;
            // 
            // chkNumeric
            // 
            this.chkNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkNumeric.AutoSize = true;
            this.chkNumeric.Checked = true;
            this.chkNumeric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNumeric.Location = new System.Drawing.Point(529, 56);
            this.chkNumeric.Name = "chkNumeric";
            this.chkNumeric.Size = new System.Drawing.Size(65, 17);
            this.chkNumeric.TabIndex = 5;
            this.chkNumeric.Text = "Numeric";
            this.chkNumeric.UseVisualStyleBackColor = true;
            // 
            // MathematicaCalculatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkNumeric);
            this.Controls.Add(this.imgGraphics);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.txtPrint);
            this.Controls.Add(this.textResult);
            this.Controls.Add(this.lblGraphics);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnEvaluate);
            this.Controls.Add(this.lblPrint);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblInput);
            this.Name = "MathematicaCalculatorControl";
            this.Size = new System.Drawing.Size(603, 627);
            ((System.ComponentModel.ISupportInitialize)(this.imgGraphics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.ComboBox txtInput;
        private System.Windows.Forms.Button btnEvaluate;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox textResult;
        private System.Windows.Forms.Label lblPrint;
        private System.Windows.Forms.TextBox txtPrint;
        private System.Windows.Forms.Label lblGraphics;
        private System.Windows.Forms.PictureBox imgGraphics;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.CheckBox chkNumeric;
    }
}
