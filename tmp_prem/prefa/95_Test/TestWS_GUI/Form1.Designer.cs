namespace TestWS_GUI {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonFolder1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSize = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numPackages = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxPackages = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonLoadScenario = new System.Windows.Forms.Button();
            this.buttonSaveScenario = new System.Windows.Forms.Button();
            this.buttonRemoveThread = new System.Windows.Forms.Button();
            this.buttonAddThread = new System.Windows.Forms.Button();
            this.tabControlThreads = new System.Windows.Forms.TabControl();
            this.tabPageThread1 = new System.Windows.Forms.TabPage();
            this.listBoxSelected = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxNumSendPackages = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTimeLag = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonFolder2 = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxCheckAll = new System.Windows.Forms.CheckBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxSendPackages = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxResults = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControlThreads.SuspendLayout();
            this.tabPageThread1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(760, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.fileToolStripMenuItem.Text = "&Datoteka";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.aboutToolStripMenuItem.Text = "&O programu...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 448);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Click += new System.EventHandler(this.tabControl1_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonFolder1);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.textBoxSize);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.buttonGenerate);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.numPackages);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.listBoxPackages);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(752, 422);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "kreiraj pakete";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonFolder1
            // 
            this.buttonFolder1.Location = new System.Drawing.Point(10, 196);
            this.buttonFolder1.Name = "buttonFolder1";
            this.buttonFolder1.Size = new System.Drawing.Size(75, 23);
            this.buttonFolder1.TabIndex = 10;
            this.buttonFolder1.Text = "Izberi mapo";
            this.buttonFolder1.UseVisualStyleBackColor = true;
            this.buttonFolder1.Click += new System.EventHandler(this.buttonFolder1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(278, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Vpiši pribl. velikost v [MB] (1)";
            // 
            // textBoxSize
            // 
            this.textBoxSize.Location = new System.Drawing.Point(281, 90);
            this.textBoxSize.Name = "textBoxSize";
            this.textBoxSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxSize.TabIndex = 8;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(278, 150);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(74, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Prikaži pakete";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(143, 196);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 6;
            this.buttonGenerate.Text = "Generiraj";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Vpiši št. paketov (1)";
            // 
            // numPackages
            // 
            this.numPackages.Location = new System.Drawing.Point(281, 33);
            this.numPackages.Name = "numPackages";
            this.numPackages.Size = new System.Drawing.Size(100, 20);
            this.numPackages.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Izberi paket, ki bo služil za osnovo:";
            // 
            // listBoxPackages
            // 
            this.listBoxPackages.FormattingEnabled = true;
            this.listBoxPackages.Location = new System.Drawing.Point(10, 30);
            this.listBoxPackages.Name = "listBoxPackages";
            this.listBoxPackages.Size = new System.Drawing.Size(208, 147);
            this.listBoxPackages.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonLoadScenario);
            this.tabPage2.Controls.Add(this.buttonSaveScenario);
            this.tabPage2.Controls.Add(this.buttonRemoveThread);
            this.tabPage2.Controls.Add(this.buttonAddThread);
            this.tabPage2.Controls.Add(this.tabControlThreads);
            this.tabPage2.Controls.Add(this.buttonCancel);
            this.tabPage2.Controls.Add(this.buttonFolder2);
            this.tabPage2.Controls.Add(this.buttonDown);
            this.tabPage2.Controls.Add(this.buttonRemoveAll);
            this.tabPage2.Controls.Add(this.buttonRemove);
            this.tabPage2.Controls.Add(this.buttonUp);
            this.tabPage2.Controls.Add(this.buttonAdd);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.checkBoxCheckAll);
            this.tabPage2.Controls.Add(this.buttonSend);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.listBoxSendPackages);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(752, 422);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "pošlji na WS";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonLoadScenario
            // 
            this.buttonLoadScenario.Location = new System.Drawing.Point(566, 254);
            this.buttonLoadScenario.Name = "buttonLoadScenario";
            this.buttonLoadScenario.Size = new System.Drawing.Size(84, 28);
            this.buttonLoadScenario.TabIndex = 31;
            this.buttonLoadScenario.Text = "Naloži scenarij";
            this.buttonLoadScenario.UseVisualStyleBackColor = true;
            this.buttonLoadScenario.Click += new System.EventHandler(this.buttonLoadScenario_Click);
            // 
            // buttonSaveScenario
            // 
            this.buttonSaveScenario.Location = new System.Drawing.Point(566, 220);
            this.buttonSaveScenario.Name = "buttonSaveScenario";
            this.buttonSaveScenario.Size = new System.Drawing.Size(84, 28);
            this.buttonSaveScenario.TabIndex = 31;
            this.buttonSaveScenario.Text = "Shrani scenarij";
            this.buttonSaveScenario.UseVisualStyleBackColor = true;
            this.buttonSaveScenario.Click += new System.EventHandler(this.buttonSaveScenario_Click);
            // 
            // buttonRemoveThread
            // 
            this.buttonRemoveThread.Location = new System.Drawing.Point(457, 304);
            this.buttonRemoveThread.Name = "buttonRemoveThread";
            this.buttonRemoveThread.Size = new System.Drawing.Size(90, 23);
            this.buttonRemoveThread.TabIndex = 30;
            this.buttonRemoveThread.Text = "Odstrani thread";
            this.buttonRemoveThread.UseVisualStyleBackColor = true;
            this.buttonRemoveThread.Click += new System.EventHandler(this.buttonRemoveThread_Click);
            // 
            // buttonAddThread
            // 
            this.buttonAddThread.Location = new System.Drawing.Point(328, 304);
            this.buttonAddThread.Name = "buttonAddThread";
            this.buttonAddThread.Size = new System.Drawing.Size(90, 23);
            this.buttonAddThread.TabIndex = 29;
            this.buttonAddThread.Text = "Dodaj thread";
            this.buttonAddThread.UseVisualStyleBackColor = true;
            this.buttonAddThread.Click += new System.EventHandler(this.buttonAddThread_Click);
            // 
            // tabControlThreads
            // 
            this.tabControlThreads.Controls.Add(this.tabPageThread1);
            this.tabControlThreads.Location = new System.Drawing.Point(328, 19);
            this.tabControlThreads.Name = "tabControlThreads";
            this.tabControlThreads.SelectedIndex = 0;
            this.tabControlThreads.Size = new System.Drawing.Size(219, 283);
            this.tabControlThreads.TabIndex = 28;
            // 
            // tabPageThread1
            // 
            this.tabPageThread1.Controls.Add(this.listBoxSelected);
            this.tabPageThread1.Controls.Add(this.label6);
            this.tabPageThread1.Controls.Add(this.textBoxNumSendPackages);
            this.tabPageThread1.Controls.Add(this.label5);
            this.tabPageThread1.Controls.Add(this.textBoxTimeLag);
            this.tabPageThread1.Location = new System.Drawing.Point(4, 22);
            this.tabPageThread1.Name = "tabPageThread1";
            this.tabPageThread1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageThread1.Size = new System.Drawing.Size(211, 257);
            this.tabPageThread1.TabIndex = 0;
            this.tabPageThread1.Text = "thread 1";
            this.tabPageThread1.UseVisualStyleBackColor = true;
            // 
            // listBoxSelected
            // 
            this.listBoxSelected.FormattingEnabled = true;
            this.listBoxSelected.Location = new System.Drawing.Point(0, 0);
            this.listBoxSelected.Name = "listBoxSelected";
            this.listBoxSelected.Size = new System.Drawing.Size(208, 147);
            this.listBoxSelected.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Vpiši kolikokrat se pošlje cel seznam (1)";
            // 
            // textBoxNumSendPackages
            // 
            this.textBoxNumSendPackages.Location = new System.Drawing.Point(9, 179);
            this.textBoxNumSendPackages.Name = "textBoxNumSendPackages";
            this.textBoxNumSendPackages.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumSendPackages.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Vpiši časovni zamik v [ms] (100)";
            // 
            // textBoxTimeLag
            // 
            this.textBoxTimeLag.Location = new System.Drawing.Point(9, 227);
            this.textBoxTimeLag.Name = "textBoxTimeLag";
            this.textBoxTimeLag.Size = new System.Drawing.Size(100, 20);
            this.textBoxTimeLag.TabIndex = 16;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(156, 279);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 27;
            this.buttonCancel.Text = "Prekliči";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonFolder2
            // 
            this.buttonFolder2.Location = new System.Drawing.Point(156, 194);
            this.buttonFolder2.Name = "buttonFolder2";
            this.buttonFolder2.Size = new System.Drawing.Size(75, 23);
            this.buttonFolder2.TabIndex = 26;
            this.buttonFolder2.Text = "Izberi mapo";
            this.buttonFolder2.UseVisualStyleBackColor = true;
            this.buttonFolder2.Click += new System.EventHandler(this.buttonFolder2_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(566, 84);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(84, 28);
            this.buttonDown.TabIndex = 25;
            this.buttonDown.Text = "Premakni dol";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Location = new System.Drawing.Point(566, 152);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(84, 28);
            this.buttonRemoveAll.TabIndex = 24;
            this.buttonRemoveAll.Text = "Odstrani vse";
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            this.buttonRemoveAll.Click += new System.EventHandler(this.buttonRemoveAll_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(566, 118);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(84, 28);
            this.buttonRemove.TabIndex = 24;
            this.buttonRemove.Text = "Odstrani";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(566, 50);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(84, 28);
            this.buttonUp.TabIndex = 23;
            this.buttonUp.Text = "Premakni gor";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(252, 93);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(58, 28);
            this.buttonAdd.TabIndex = 22;
            this.buttonAdd.Text = "Dodaj ->";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(325, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Izbrani paketi:";
            // 
            // checkBoxCheckAll
            // 
            this.checkBoxCheckAll.AutoSize = true;
            this.checkBoxCheckAll.Location = new System.Drawing.Point(23, 198);
            this.checkBoxCheckAll.Name = "checkBoxCheckAll";
            this.checkBoxCheckAll.Size = new System.Drawing.Size(77, 17);
            this.checkBoxCheckAll.TabIndex = 15;
            this.checkBoxCheckAll.Text = "označi vse";
            this.checkBoxCheckAll.UseVisualStyleBackColor = true;
            this.checkBoxCheckAll.CheckedChanged += new System.EventHandler(this.checkBoxCheckAll_CheckedChanged);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(41, 279);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 13;
            this.buttonSend.Text = "Pošlji";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(194, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Izberi pakete, ki se bodo poslali na WS:";
            // 
            // listBoxSendPackages
            // 
            this.listBoxSendPackages.FormattingEnabled = true;
            this.listBoxSendPackages.Location = new System.Drawing.Point(23, 41);
            this.listBoxSendPackages.Name = "listBoxSendPackages";
            this.listBoxSendPackages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSendPackages.Size = new System.Drawing.Size(208, 147);
            this.listBoxSendPackages.TabIndex = 8;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxResults);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(752, 422);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "rezultati";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxResults
            // 
            this.textBoxResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResults.Location = new System.Drawing.Point(3, 3);
            this.textBoxResults.Multiline = true;
            this.textBoxResults.Name = "textBoxResults";
            this.textBoxResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResults.Size = new System.Drawing.Size(746, 416);
            this.textBoxResults.TabIndex = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(14, 476);
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(208, 16);
            this.progressBar1.Step = 50;
            this.progressBar1.TabIndex = 4;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "xml files|*.xml";
            this.saveFileDialog1.Title = "Shrani scenarij";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "xml files|*.xml";
            this.openFileDialog1.Title = "Naloži scenarij";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 495);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "5";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControlThreads.ResumeLayout(false);
            this.tabPageThread1.ResumeLayout(false);
            this.tabPageThread1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxPackages;
        private System.Windows.Forms.TextBox numPackages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTimeLag;
        private System.Windows.Forms.CheckBox checkBoxCheckAll;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxSendPackages;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxNumSendPackages;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBoxResults;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox listBoxSelected;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonAdd;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button buttonFolder1;
        private System.Windows.Forms.Button buttonFolder2;
        private System.Windows.Forms.Button buttonRemoveAll;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControlThreads;
        private System.Windows.Forms.TabPage tabPageThread1;
        private System.Windows.Forms.Button buttonRemoveThread;
        private System.Windows.Forms.Button buttonAddThread;
        private System.Windows.Forms.Button buttonSaveScenario;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonLoadScenario;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}

