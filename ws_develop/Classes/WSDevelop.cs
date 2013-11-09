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
    [WebService(Namespace = WSBase.DefaultNamespace,   
                Description = "This is a demonstration WebService.")]
    public class WSDevelop1 : WSDevelop, IWsDevelop1, IWSBase
    {


        //WEB SERVICE EXAMPLE
        //The HelloWorld() example service returns the string Hello World
        //To build, uncomment the following lines then save and build the project
        //To test, right-click the Web Service's .asmx file and select View in Browser
        //
        // [WebMethod(CacheDuration = CacheHelloWorldTime,
        [WebMethod(
         Description = "As simple as it gets - the TestMethodWS2.")]
        public string TestMethodWS2()
        {
            return "This is a test method No. 2!!";
        }



    } // class WS_Develop2



    /// <summary>Web service class.</summary>
    [WebService(Namespace = WSBase.DefaultNamespace,   // Namespace = "http://codeproject.com/webservices/",
                Description = "This is a demonstration WebService.")]
    public class WSDevelop : WSBase, IWsDevelop, IWSBase
    {
        private System.Windows.Forms.Button button1;

        public WSDevelop()
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


        //WEB SERVICE EXAMPLE
        //The HelloWorld() example service returns the string Hello World
        //To build, uncomment the following lines then save and build the project
        //To test, right-click the Web Service's .asmx file and select View in Browser
        //
        [WebMethod(CacheDuration = CacheHelloWorldTime,
         Description = "Just returns the 'Hello World!' string.")]
        public string ReturnClientTable()
        {
            return "Hello World";
        }

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
                    Clients[i].Name = "Client " + i.ToString();
                    Clients[i].Id = i;
                }
            }
            return Clients;
        }
    }  // class WS_Develop1



}
