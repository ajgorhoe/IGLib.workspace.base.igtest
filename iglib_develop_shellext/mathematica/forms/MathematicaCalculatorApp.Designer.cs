﻿namespace IG.Forms
{
    partial class MathematicaCalculatorApp
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
            this.btnClose = new System.Windows.Forms.Button();
            this.mathematicaCalculatorControl1 = new IG.Forms.MathematicaCalculatorControl();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(565, 629);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // mathematicaCalculatorControl1
            // 
            this.mathematicaCalculatorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mathematicaCalculatorControl1.Location = new System.Drawing.Point(0, 0);
            this.mathematicaCalculatorControl1.Name = "mathematicaCalculatorControl1";
            this.mathematicaCalculatorControl1.Size = new System.Drawing.Size(640, 623);
            this.mathematicaCalculatorControl1.TabIndex = 0;
            this.mathematicaCalculatorControl1.Load += new System.EventHandler(this.mathematicaCalculatorControl1_Load);
            // 
            // MathematicaCalculatorApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 664);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.mathematicaCalculatorControl1);
            this.Name = "MathematicaCalculatorApp";
            this.Text = "Mathematica Calculator";
            this.ResumeLayout(false);

        }

        #endregion

        private MathematicaCalculatorControl mathematicaCalculatorControl1;
        private System.Windows.Forms.Button btnClose;
    }
}