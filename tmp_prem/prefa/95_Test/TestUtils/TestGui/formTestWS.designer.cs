namespace TestGui
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPaketTip = new System.Windows.Forms.TextBox();
            this.txtPaketDatum = new System.Windows.Forms.TextBox();
            this.txtStZapisov = new System.Windows.Forms.TextBox();
            this.txtVsotaDokumentDatum = new System.Windows.Forms.TextBox();
            this.txtVsotaCenaNeto = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnPosljiPaket = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.textBoxXMLPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServiceUrl = new System.Windows.Forms.TextBox();
            this.btnGetPaketShema = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnGetPaketStatusShema = new System.Windows.Forms.Button();
            this.btnPaketTipZV = new System.Windows.Forms.Button();
            this.btnPaketStatusZV = new System.Windows.Forms.Button();
            this.btnPodrocjaZV = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "paket tip Id (byte)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "paket datum (datetime)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "vsota dokument datum (float)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "število zapisov (int64)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "vsota cena neto (decimal)";
            // 
            // txtPaketTip
            // 
            this.txtPaketTip.Location = new System.Drawing.Point(157, 6);
            this.txtPaketTip.Name = "txtPaketTip";
            this.txtPaketTip.Size = new System.Drawing.Size(100, 20);
            this.txtPaketTip.TabIndex = 5;
            // 
            // txtPaketDatum
            // 
            this.txtPaketDatum.Location = new System.Drawing.Point(157, 32);
            this.txtPaketDatum.Name = "txtPaketDatum";
            this.txtPaketDatum.Size = new System.Drawing.Size(100, 20);
            this.txtPaketDatum.TabIndex = 6;
            this.txtPaketDatum.TextChanged += new System.EventHandler(this.txtPaketDatum_TextChanged);
            // 
            // txtStZapisov
            // 
            this.txtStZapisov.Location = new System.Drawing.Point(157, 58);
            this.txtStZapisov.Name = "txtStZapisov";
            this.txtStZapisov.Size = new System.Drawing.Size(100, 20);
            this.txtStZapisov.TabIndex = 7;
            // 
            // txtVsotaDokumentDatum
            // 
            this.txtVsotaDokumentDatum.Location = new System.Drawing.Point(157, 84);
            this.txtVsotaDokumentDatum.Name = "txtVsotaDokumentDatum";
            this.txtVsotaDokumentDatum.Size = new System.Drawing.Size(100, 20);
            this.txtVsotaDokumentDatum.TabIndex = 8;
            // 
            // txtVsotaCenaNeto
            // 
            this.txtVsotaCenaNeto.Location = new System.Drawing.Point(157, 110);
            this.txtVsotaCenaNeto.Name = "txtVsotaCenaNeto";
            this.txtVsotaCenaNeto.Size = new System.Drawing.Size(100, 20);
            this.txtVsotaCenaNeto.TabIndex = 9;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(12, 137);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(766, 300);
            this.txtResult.TabIndex = 10;
            // 
            // btnPosljiPaket
            // 
            this.btnPosljiPaket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPosljiPaket.Location = new System.Drawing.Point(8, 447);
            this.btnPosljiPaket.Name = "btnPosljiPaket";
            this.btnPosljiPaket.Size = new System.Drawing.Size(75, 23);
            this.btnPosljiPaket.TabIndex = 12;
            this.btnPosljiPaket.Text = "Pošlji paket";
            this.btnPosljiPaket.UseVisualStyleBackColor = true;
            this.btnPosljiPaket.Click += new System.EventHandler(this.btnPosljiPaket_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(703, 447);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Clear results";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(700, 81);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 15;
            this.btnTest.Text = "open Form2";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // textBoxXMLPath
            // 
            this.textBoxXMLPath.Location = new System.Drawing.Point(280, 110);
            this.textBoxXMLPath.Name = "textBoxXMLPath";
            this.textBoxXMLPath.Size = new System.Drawing.Size(414, 20);
            this.textBoxXMLPath.TabIndex = 16;
            this.textBoxXMLPath.TextChanged += new System.EventHandler(this.textBoxXMLPath_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(700, 108);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.button5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(277, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Pot do xml datoteke:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(277, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "url webservica";
            // 
            // txtServiceUrl
            // 
            this.txtServiceUrl.Location = new System.Drawing.Point(280, 33);
            this.txtServiceUrl.Name = "txtServiceUrl";
            this.txtServiceUrl.Size = new System.Drawing.Size(414, 20);
            this.txtServiceUrl.TabIndex = 20;
            this.txtServiceUrl.Text = "http://localhost:1792/Service.asmx";
            // 
            // btnGetPaketShema
            // 
            this.btnGetPaketShema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetPaketShema.Location = new System.Drawing.Point(89, 447);
            this.btnGetPaketShema.Name = "btnGetPaketShema";
            this.btnGetPaketShema.Size = new System.Drawing.Size(98, 23);
            this.btnGetPaketShema.TabIndex = 21;
            this.btnGetPaketShema.Text = "Get paket shema";
            this.btnGetPaketShema.UseVisualStyleBackColor = true;
            this.btnGetPaketShema.Click += new System.EventHandler(this.btnGetPaketShema_Click);
            // 
            // btnGetPaketStatusShema
            // 
            this.btnGetPaketStatusShema.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetPaketStatusShema.Location = new System.Drawing.Point(192, 447);
            this.btnGetPaketStatusShema.Name = "btnGetPaketStatusShema";
            this.btnGetPaketStatusShema.Size = new System.Drawing.Size(128, 23);
            this.btnGetPaketStatusShema.TabIndex = 22;
            this.btnGetPaketStatusShema.Text = "Get paket status shema";
            this.btnGetPaketStatusShema.UseVisualStyleBackColor = true;
            this.btnGetPaketStatusShema.Click += new System.EventHandler(this.btnGetPaketStatusShema_Click);
            // 
            // btnPaketTipZV
            // 
            this.btnPaketTipZV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPaketTipZV.Location = new System.Drawing.Point(326, 447);
            this.btnPaketTipZV.Name = "btnPaketTipZV";
            this.btnPaketTipZV.Size = new System.Drawing.Size(161, 23);
            this.btnPaketTipZV.TabIndex = 23;
            this.btnPaketTipZV.Text = "Get paket tip zaloga vrednosti";
            this.btnPaketTipZV.UseVisualStyleBackColor = true;
            this.btnPaketTipZV.Click += new System.EventHandler(this.btnPaketTipZV_Click);
            // 
            // btnPaketStatusZV
            // 
            this.btnPaketStatusZV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPaketStatusZV.Location = new System.Drawing.Point(492, 447);
            this.btnPaketStatusZV.Name = "btnPaketStatusZV";
            this.btnPaketStatusZV.Size = new System.Drawing.Size(176, 23);
            this.btnPaketStatusZV.TabIndex = 24;
            this.btnPaketStatusZV.Text = "Get paket status zaloga vrednosti";
            this.btnPaketStatusZV.UseVisualStyleBackColor = true;
            this.btnPaketStatusZV.Click += new System.EventHandler(this.btnPaketStatusZV_Click);
            // 
            // btnPodrocjaZV
            // 
            this.btnPodrocjaZV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPodrocjaZV.Location = new System.Drawing.Point(492, 476);
            this.btnPodrocjaZV.Name = "btnPodrocjaZV";
            this.btnPodrocjaZV.Size = new System.Drawing.Size(176, 23);
            this.btnPodrocjaZV.TabIndex = 24;
            this.btnPodrocjaZV.Text = "Get področja zaloga vrednosti";
            this.btnPodrocjaZV.UseVisualStyleBackColor = true;
            this.btnPodrocjaZV.Click += new System.EventHandler(this.btnPodrocjaZV_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(703, 476);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 13;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(674, 476);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(19, 23);
            this.buttonBrowse.TabIndex = 25;
            this.buttonBrowse.Text = "B";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 510);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.btnPodrocjaZV);
            this.Controls.Add(this.btnPaketStatusZV);
            this.Controls.Add(this.btnPaketTipZV);
            this.Controls.Add(this.btnGetPaketStatusShema);
            this.Controls.Add(this.btnGetPaketShema);
            this.Controls.Add(this.txtServiceUrl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.textBoxXMLPath);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnPosljiPaket);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtVsotaCenaNeto);
            this.Controls.Add(this.txtVsotaDokumentDatum);
            this.Controls.Add(this.txtStZapisov);
            this.Controls.Add(this.txtPaketDatum);
            this.Controls.Add(this.txtPaketTip);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPaketTip;
        private System.Windows.Forms.TextBox txtPaketDatum;
        private System.Windows.Forms.TextBox txtStZapisov;
        private System.Windows.Forms.TextBox txtVsotaDokumentDatum;
        private System.Windows.Forms.TextBox txtVsotaCenaNeto;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnPosljiPaket;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox textBoxXMLPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtServiceUrl;
        private System.Windows.Forms.Button btnGetPaketShema;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnGetPaketStatusShema;
        private System.Windows.Forms.Button btnPaketTipZV;
        private System.Windows.Forms.Button btnPaketStatusZV;
        private System.Windows.Forms.Button btnPodrocjaZV;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonBrowse;
    }
}

