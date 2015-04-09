using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


using IG.Lib;
using IG.Forms;

namespace IG.Web.Forms
{

    /// <summary>Form containing information about Web service test clients software.
    /// <para>Help can also be triggered from here.</para></summary>
    public partial class WSClientFormsAboutForm : UserControl
    {
        public WSClientFormsAboutForm()
        {
            InitializeComponent();
            this.lblAboutWeb.Text = DefaultHomePageShort;
        }

        


        #region Operation

        const string DefaultHomePageShort = "www2.arnes.si/~ljc3m2/igor/iglib/";

        protected string _homePageShort;

        /// <summary>Short version of the application's home page.</summary>
        public virtual string HomePageShort
        {
            get
            {
                if (_homePageShort == null)
                    _homePageShort = DefaultHomePageShort;
                return _homePageShort;
            }
            protected set { 
                _homePageShort = value;
                this.lblAboutWeb.Text = DefaultHomePageShort;
            }
        }

        public virtual string HomePage
        {
            get { return "http://" + HomePageShort; }
        }

        const string DefaultHelpLocation = "http://www2.arnes.si/~ljc3m2/igor/iglib/index.html";

        protected string _helpLocation;

        /// <summary>Location of the help file that can be displayed in a web browser.</summary>
        public virtual string HelpLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_helpLocation))
                {
                    _helpLocation = DefaultHelpLocation;
                    string trialLocation = GetHelpLocationInExecutableDirectory();
                    if (!string.IsNullOrEmpty(trialLocation))
                        if (File.Exists(trialLocation))
                        {
                            _helpLocation = trialLocation;
                        }
                }
                return _helpLocation;
            }
        }

        public virtual string GetHelpLocationInExecutableDirectory()
        {
            string location = null;
            string executablePath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrEmpty(executablePath))
            {
                string directoryPath = Path.Combine(executablePath, ApplicationName);
                location = Path.Combine(directoryPath, "help.html");
            }
            return location;
        }

        protected BrowserSimpleWindow _browser;

        protected BrowserSimpleWindow Browser
        {
            get
            {
                bool openNew = false;
                if (_browser == null)
                    openNew = true;
                else if (_browser.IsDisposed)
                    openNew = true;
                if (openNew)
                    _browser = new BrowserSimpleWindow();
                return _browser;
            }
        }

        public string DefaultApplicationName = "WS Test Clients";

        protected string _applicationName;


        public string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(_applicationName))
                {
                    _applicationName = DefaultApplicationName;
                }
                return _applicationName;
            }
        }


        public void ShowHelp()
        {

            try
            {
                string location = HelpLocation;
                Browser.OpenLocation(location);
                Browser.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + Environment.NewLine 
                    + "ERROR: " + Environment.NewLine
                    + "  " + ex.Message +Environment.NewLine + Environment.NewLine
                    );
            }
            //            // Open software web page:
            //            System.Diagnostics.Process.Start(lblAboutWeb.Text);

            //            //Creating a fading message in this thread, which must be canceled explicitly (e.g. by pressing mouse button 3:)
            //            string msgtitle = "Info:";
            //            string msgtext = 
            //@"Sorry, help is not yet available.";
            //            int showTime = 4000;
            //            FadingMessage fm = new FadingMessage(msgtitle, msgtext, showTime);
        }

        public void ShowWebPage()
        {
            try
            {
                string location = HomePage;
                Browser.OpenLocation(location);
                Browser.ShowDialog();
            }
            catch (Exception)
            {
            }
        }

        #endregion Operation

        private void lblAboutWeb_Click(object sender, EventArgs e)
        {
            // Open software web page:
            System.Diagnostics.Process.Start(lblAboutWeb.Text);
            // BrowserForm.BrowserMain();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

            this.ShowHelp();
        }

        private void btnWeb_Click(object sender, EventArgs e)
        {
            this.ShowWebPage();
        }
        
        
    }
}
