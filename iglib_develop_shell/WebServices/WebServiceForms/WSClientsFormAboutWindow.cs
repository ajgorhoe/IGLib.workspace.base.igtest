using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IG.Web.Forms
{

    /// <summary>Window that displays information about Demo application for performing parametric studies
    /// on ANN models.</summary>
    /// $A Igor Apr13;
    public partial class WSClientFormsAboutWindow : Form
    {
        public WSClientFormsAboutWindow()
        {
            InitializeComponent();
        }

        public void ShowHelp()
        {
            this.wsClientFormsAbout1.ShowHelp();
        }


        public void ShowWebPage()
        {
            this.wsClientFormsAbout1.ShowWebPage();
        }

    }
}
