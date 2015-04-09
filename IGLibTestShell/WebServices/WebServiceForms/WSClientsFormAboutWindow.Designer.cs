namespace IG.Web.Forms
{
    partial class WSClientFormsAboutWindow
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
            this.wsClientFormsAbout1 = new IG.Web.Forms.WSClientFormsAboutForm();
            this.SuspendLayout();
            // 
            // clientformsAbout
            // 
            this.wsClientFormsAbout1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wsClientFormsAbout1.Location = new System.Drawing.Point(0, 0);
            this.wsClientFormsAbout1.Name = "WSClients";
            this.wsClientFormsAbout1.Size = new System.Drawing.Size(946, 547);
            this.wsClientFormsAbout1.TabIndex = 0;
            // 
            // ClientFormsAboutWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 548);
            this.Controls.Add(this.wsClientFormsAbout1);
            this.Name = "WSClientsAboitWindow";
            this.Text = "WsClientsAboutWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private WSClientFormsAboutForm wsClientFormsAbout1;

    }
}