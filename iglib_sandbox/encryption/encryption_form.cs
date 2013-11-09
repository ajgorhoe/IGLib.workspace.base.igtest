using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IG.Forms;

using System.Security.Cryptography.Xml;



namespace IG.Crypt
{

    public partial class EncryptionForm : Form
    {
        public EncryptionForm()
        {
            InitializeComponent();
        }

        private Label MainLbl;
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
            this.MainLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MainLbl
            // 
            this.MainLbl.AutoSize = true;
            this.MainLbl.BackColor = System.Drawing.Color.White;
            this.MainLbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MainLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.MainLbl.Location = new System.Drawing.Point(12, 9);
            this.MainLbl.Name = "MainLbl";
            this.MainLbl.Padding = new System.Windows.Forms.Padding(6);
            this.MainLbl.Size = new System.Drawing.Size(153, 34);
            this.MainLbl.TabIndex = 0;
            this.MainLbl.Text = "Encryption tests";
            // 
            // EncryptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 623);
            this.Controls.Add(this.MainLbl);
            this.Name = "EncryptionForm";
            this.Text = "Encryption";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion



        private void ServerForm_Load(object sender, EventArgs e)
        {

        }
    }
}


