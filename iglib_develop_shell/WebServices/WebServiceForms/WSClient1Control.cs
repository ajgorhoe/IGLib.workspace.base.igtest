using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IG.Lib;
using IG.Num;
using IG.Web;

namespace IG.Web.Forms
{

    /// <summary>Control for making predictions of output values based on ANN model.</summary>
    /// $A Igor Apr13;
    public partial class WSClient1Control : UserControl
    {



        public WSClient1Control()
        {
            InitializeComponent();
            // Add event handler that occurs when cell values change in the input parameters DataGridView:

            comboWebServices.Items.Clear();
            comboMethods.Items.Clear();

            comboWebServices.Items.Add(WSBaseBase);
            comboWebServices.Items.Add(WSBase);
            comboWebServices.Items.Add(WS_Develop1);
            comboWebServices.Items.Add(WS_Develop2);

            comboWebServices.SelectedIndex = 0;
            WebService = comboWebServices.SelectedItem as string;

            txtUrl.Text = ServiceUrl;

        }

        #region Data

        // Known web Services:

        public const string WSBaseBase = "WSBaseBase";

        public const string WSBase = "WSBase";

        public const string WS_Develop1 = "WS_Develop1";

        public const string WS_Develop2 = "WS_Develop2";

        // Known web service methods:

        public const string TestService = "TestService";

        public const string TestServiceArg = "TestServiceArg";

        public const string TestServiceArgs = "TestServiceArgs";


        private string _serviceUrl = "http://localhost:40197/WSBase.asmx";

        protected string ServiceUrl
        {
            get { return txtUrl.Text; }
            set { txtUrl.Text = value; }
        }

        /// <summary>Clears the result fields.</summary>
        protected void ClearResults()
        {
            txtResults.Clear();
            txtErrors.Clear();
        }

        private string _webService;

        /// <summary>Name of the web service that will execute the web method.</summary>
        public string WebService
        {
            get { return _webService; }
            set {
                _webService = value;
                // Update dependencies:
                ClearServiceReferences();
                ClearResults();
                comboMethods.Items.Clear();
                if (value == WSBaseBase || 
                    value == WSBase ||
                    value == WS_Develop1 ||
                    value == WS_Develop2)
                {
                    comboMethods.Items.Add(TestService);
                    comboMethods.Items.Add(TestServiceArg);
                    comboMethods.Items.Add(TestServiceArgs);
                }
                comboMethods.SelectedIndex = 0;
                WebMethod = comboMethods.SelectedItem as string;
            }
        }

        protected string _webMethod;

        public string WebMethod
        {
            get { return _webMethod; }
            set { 
                _webMethod = value;
                // Update dependencies:
                ClearResults();
            }
        }


        /// <summary>Service proy objects become null.</summary>
        public void ClearServiceReferences()
        {
            SrvWSBase = null;
        }


        IWSBase _srvWSBase;

        /// <summary>Service proxy object fof the service WSBase.</summary>
        protected IWSBase SrvWSBase
        {
            get {
                if (_srvWSBase == null)
                {
                    _srvWSBase = new WSBaseRef();
                }
                return SrvWSBase;
            }
            set { _srvWSBase = value; }
        }


        #endregion Data;


        private void btnRunServiceMethod_Click(object sender, EventArgs e)
        {
            ClearResults();
            string result=null;
            string[] args = txtArguments.Lines;
            var commandLine = UtilStr.GetCommandLine(args);
            if (WebService == WSBaseBase || WebService == WSBase)
            {
                if (WebMethod == TestService)
                {
                    result = SrvWSBase.TestService();
                } else if (WebMethod == TestService)
                {
                    result = SrvWSBase.TestService();
                }
                else if (WebMethod == TestServiceArg)
                {
                    result = SrvWSBase.TestServiceCmd(null);
                }
                else if (WebMethod == TestServiceArgs)
                {
                    result = SrvWSBase.TestServiceArgs(null);
                } 
            } else if (WebService == WS_Develop2)
            {
                result = "<< Not yet implemented for WS_Develop1. >>";
            }
            else if (WebService == WS_Develop1)
            {
                result = "<< Not yet implemented for WS_Develop2. >>";
            }

            txtResults.Text = result;
        }





    }
}
