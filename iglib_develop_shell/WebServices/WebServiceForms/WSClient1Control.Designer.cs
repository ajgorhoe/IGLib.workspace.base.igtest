namespace IG.Web.Forms
{
    partial class WSClient1Control
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
            this.btnRunServiceMethod = new System.Windows.Forms.Button();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.comboMethods = new System.Windows.Forms.ComboBox();
            this.lblMethods = new System.Windows.Forms.Label();
            this.lblArguments = new System.Windows.Forms.Label();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.comboWebServices = new System.Windows.Forms.ComboBox();
            this.lblWebService = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblErrors = new System.Windows.Forms.Label();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnRunServiceMethod
            // 
            this.btnRunServiceMethod.Location = new System.Drawing.Point(74, 247);
            this.btnRunServiceMethod.Name = "btnRunServiceMethod";
            this.btnRunServiceMethod.Size = new System.Drawing.Size(125, 23);
            this.btnRunServiceMethod.TabIndex = 1;
            this.btnRunServiceMethod.Text = "Run Service Method";
            this.btnRunServiceMethod.UseVisualStyleBackColor = true;
            this.btnRunServiceMethod.Click += new System.EventHandler(this.btnRunServiceMethod_Click);
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(0, 146);
            this.txtArguments.Multiline = true;
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(477, 95);
            this.txtArguments.TabIndex = 2;
            // 
            // comboMethods
            // 
            this.comboMethods.FormattingEnabled = true;
            this.comboMethods.Items.AddRange(new object[] {
            "TestAlive",
            "TestCount",
            "HelloWorld"});
            this.comboMethods.Location = new System.Drawing.Point(3, 106);
            this.comboMethods.Name = "comboMethods";
            this.comboMethods.Size = new System.Drawing.Size(474, 21);
            this.comboMethods.TabIndex = 3;
            // 
            // lblMethods
            // 
            this.lblMethods.AutoSize = true;
            this.lblMethods.Location = new System.Drawing.Point(3, 90);
            this.lblMethods.Name = "lblMethods";
            this.lblMethods.Size = new System.Drawing.Size(102, 13);
            this.lblMethods.TabIndex = 4;
            this.lblMethods.Text = "Web service metod:";
            // 
            // lblArguments
            // 
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(0, 130);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(98, 13);
            this.lblArguments.TabIndex = 4;
            this.lblArguments.Text = "Method arguments:";
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResults.Location = new System.Drawing.Point(0, 276);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(866, 113);
            this.txtResults.TabIndex = 5;
            this.txtResults.Text = "<< Results will be shown here. >>";
            // 
            // comboWebServices
            // 
            this.comboWebServices.FormattingEnabled = true;
            this.comboWebServices.Items.AddRange(new object[] {
            "WSBaseBase",
            "WSBase",
            "TestDevelop"});
            this.comboWebServices.Location = new System.Drawing.Point(3, 66);
            this.comboWebServices.Name = "comboWebServices";
            this.comboWebServices.Size = new System.Drawing.Size(474, 21);
            this.comboWebServices.TabIndex = 3;
            // 
            // lblWebService
            // 
            this.lblWebService.AutoSize = true;
            this.lblWebService.Location = new System.Drawing.Point(3, 50);
            this.lblWebService.Name = "lblWebService";
            this.lblWebService.Size = new System.Drawing.Size(99, 13);
            this.lblWebService.TabIndex = 4;
            this.lblWebService.Text = "Web service name:";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(3, 3);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(95, 13);
            this.lblUrl.TabIndex = 7;
            this.lblUrl.Text = "Web service URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(6, 19);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(471, 20);
            this.txtUrl.TabIndex = 2;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(3, 260);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(40, 13);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Result:";
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.Location = new System.Drawing.Point(3, 392);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(37, 13);
            this.lblErrors.TabIndex = 4;
            this.lblErrors.Text = "Errors:";
            // 
            // txtErrors
            // 
            this.txtErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrors.Location = new System.Drawing.Point(0, 408);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.Size = new System.Drawing.Size(866, 139);
            this.txtErrors.TabIndex = 5;
            this.txtErrors.Text = "<< Errors will be shown here. >>";
            // 
            // WSClient1Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.lblErrors);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblArguments);
            this.Controls.Add(this.lblWebService);
            this.Controls.Add(this.comboWebServices);
            this.Controls.Add(this.lblMethods);
            this.Controls.Add(this.comboMethods);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.btnRunServiceMethod);
            this.Name = "WSClient1Control";
            this.Size = new System.Drawing.Size(872, 550);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunServiceMethod;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.ComboBox comboMethods;
        private System.Windows.Forms.Label lblMethods;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.ComboBox comboWebServices;
        private System.Windows.Forms.Label lblWebService;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.TextBox txtErrors;


    }
}
