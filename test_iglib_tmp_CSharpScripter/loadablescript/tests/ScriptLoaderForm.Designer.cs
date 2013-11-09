namespace IG.Forms
{
    partial class ScriptLoaderForm
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
            this.rtfCode = new System.Windows.Forms.RichTextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.lblInitArgs = new System.Windows.Forms.Label();
            this.txtInitArgs = new System.Windows.Forms.TextBox();
            this.lblRunArgs = new System.Windows.Forms.Label();
            this.txtRunArgs = new System.Windows.Forms.TextBox();
            this.btnOther = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtfCode
            // 
            this.rtfCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfCode.Location = new System.Drawing.Point(5, 7);
            this.rtfCode.Name = "rtfCode";
            this.rtfCode.Size = new System.Drawing.Size(653, 389);
            this.rtfCode.TabIndex = 0;
            this.rtfCode.Text = "";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(5, 456);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(96, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load Example";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompile.Location = new System.Drawing.Point(209, 456);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(75, 23);
            this.btnCompile.TabIndex = 2;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.Location = new System.Drawing.Point(290, 457);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(583, 456);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadFromFile.Location = new System.Drawing.Point(107, 456);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(96, 21);
            this.btnLoadFromFile.TabIndex = 5;
            this.btnLoadFromFile.Text = "Load from File";
            this.btnLoadFromFile.UseVisualStyleBackColor = true;
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromFile_Click);
            // 
            // lblInitArgs
            // 
            this.lblInitArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInitArgs.AutoSize = true;
            this.lblInitArgs.Location = new System.Drawing.Point(12, 408);
            this.lblInitArgs.Name = "lblInitArgs";
            this.lblInitArgs.Size = new System.Drawing.Size(116, 13);
            this.lblInitArgs.TabIndex = 6;
            this.lblInitArgs.Text = "Initialization arguments:";
            // 
            // txtInitArgs
            // 
            this.txtInitArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInitArgs.Location = new System.Drawing.Point(131, 405);
            this.txtInitArgs.Name = "txtInitArgs";
            this.txtInitArgs.Size = new System.Drawing.Size(527, 20);
            this.txtInitArgs.TabIndex = 7;
            this.txtInitArgs.Text = "initarg1 initarg2 initarg3";
            // 
            // lblRunArgs
            // 
            this.lblRunArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRunArgs.AutoSize = true;
            this.lblRunArgs.Location = new System.Drawing.Point(9, 434);
            this.lblRunArgs.Name = "lblRunArgs";
            this.lblRunArgs.Size = new System.Drawing.Size(82, 13);
            this.lblRunArgs.TabIndex = 6;
            this.lblRunArgs.Text = "Run arguments:";
            // 
            // txtRunArgs
            // 
            this.txtRunArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRunArgs.Location = new System.Drawing.Point(131, 431);
            this.txtRunArgs.Name = "txtRunArgs";
            this.txtRunArgs.Size = new System.Drawing.Size(527, 20);
            this.txtRunArgs.TabIndex = 7;
            this.txtRunArgs.Text = "arg1 arg2";
            // 
            // btnOther
            // 
            this.btnOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOther.Location = new System.Drawing.Point(413, 457);
            this.btnOther.Name = "btnOther";
            this.btnOther.Size = new System.Drawing.Size(96, 23);
            this.btnOther.TabIndex = 1;
            this.btnOther.Text = "Other tests";
            this.btnOther.UseVisualStyleBackColor = true;
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // ScriptLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 481);
            this.Controls.Add(this.txtRunArgs);
            this.Controls.Add(this.txtInitArgs);
            this.Controls.Add(this.lblRunArgs);
            this.Controls.Add(this.lblInitArgs);
            this.Controls.Add(this.btnLoadFromFile);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.btnOther);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.rtfCode);
            this.Name = "ScriptLoaderForm";
            this.Text = "CompilerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtfCode;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.Label lblInitArgs;
        private System.Windows.Forms.TextBox txtInitArgs;
        private System.Windows.Forms.Label lblRunArgs;
        private System.Windows.Forms.TextBox txtRunArgs;
        private System.Windows.Forms.Button btnOther;
    }
}