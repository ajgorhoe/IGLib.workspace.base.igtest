// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// DEVELOPMENT OF WEB SERVICES.


using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;


namespace IG.Web
{





    /// <summary>Web service class.</summary>
    [WebService(Namespace = WSBaseClass.DefaultNamespace,   // Namespace = "http://codeproject.com/webservices/",
                Description = "This is a demonstration WebService.")]
    public class WSDevelopClass : WSBaseClass,   IWsDevelop, IWSBase
    {
        private System.Windows.Forms.Button button1;

        public WSDevelopClass()
        {
            //REMARK: This call is required by the ASP+ Web Services Designer
            InitializeComponent();
        }

        /// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
        }


        #region WebMethods


        /// <summary>Returns an array of client data objects.</summary>
        [WebMethod(CacheDuration = CacheDurationTimeBase,
         Description = "Returns an array of client data.")]
        public string ReturnClientTable()
        {
            return "Hello World";
        }

        /// <summary>"Returns an array of Clients (maximum 10!)."</summary>
        /// <param name="Number">Number of client data objects retuerned, maximum 10.</param>
        [WebMethod(CacheDuration = 30,
         Description = "Returns an array of Clients (maximum 10!).")]
        public ClientDataBase[] GetClientData(int Number)
        {
            ClientDataBase[] Clients = null;

            if (Number > 0 && Number <= 10)
            {
                Clients = new ClientDataBase[Number];
                for (int i = 0; i < Number; i++)
                {
                    Clients[i] = new ClientDataBase(i);
                    Clients[i].Name = "Client " + i.ToString();
                }
            }
            return Clients;
        }


        #endregion WebMethods


    }  // class WSDevelop




    /// <summary>Web service class.</summary>
    [WebService(Namespace = WSBaseClass.DefaultNamespace,
                Description = "This is a demonstration WebService.")]
    public class WSDevelop1Class : WSDevelopClass,   IWsDevelop1, IWSBase
    {

        /// <summary>Another test method from higher level development service.</summary>
        [WebMethod(CacheDuration = CacheDurationTimeBase,
         Description = "Another test method fromhigher level development service.")]
        public string TestMethodWS2()
        {
            return "This is a test method No. 2!!";
        }


    } // class WSDevelop1



}
