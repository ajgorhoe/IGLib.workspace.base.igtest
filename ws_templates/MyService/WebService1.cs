namespace MyService
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Services;

	public struct ClientData
	{
		public String Name;
		public int	ID;
	}


    
    /// <summary>
    ///    Summary description for WebService1.
    /// </summary>
    [WebService(Namespace = "http://codeproject.com/webservices/",
                Description = "This is a demonstration WebService.")]
    public class WebService2 : System.Web.Services.WebService
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



    }

    /// <summary>
    ///    Summary description for WebService1.
    /// </summary>
    [WebService(Namespace = "http://codeproject.com/webservices/",
                Description = "This is a demonstration WebService.")]
    public class WebServiceBase : System.Web.Services.WebService
    {


        protected const int CacheHelloWorldTime = 10;	// seconds


        public static int id;

        //WEB SERVICE EXAMPLE
        //The HelloWorld() example service returns the string Hello World
        //To build, uncomment the following lines then save and build the project
        //To test, right-click the Web Service's .asmx file and select View in Browser
        //
        // [WebMethod(CacheDuration = CacheHelloWorldTime,
        [WebMethod(
         Description = "As simple as it gets - the TestMethod.")]
        public string TestMethod()
        {
            ++id;
            return "This is a test method call No. " + id + ".";
        }
    }



    /// <summary>
    ///    Summary description for WebService1.
    /// </summary>
	[WebService(Namespace="http://codeproject.com/webservices/",
	            Description="This is a demonstration WebService.")]
    public class WebService1 : WebServiceBase
    {
        private System.Windows.Forms.Button button1;

        public WebService1()
        {
            //CODEGEN: This call is required by the ASP+ Web Services Designer
            InitializeComponent();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        /// </summary>
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
		 Description="As simple as it gets - the ubiquitous Hello World.")]
        public string HelloWorld()
        {
            return "Hello World";
        }

		[WebMethod(CacheDuration = 30,
		 Description="Returns an array of Clients.")]
		public ClientData[] GetClientData(int Number)
		{
			ClientData [] Clients = null;

			if (Number > 0 && Number <= 10)
			{
				Clients = new ClientData[Number];
				for (int i = 0; i < Number; i++)
				{
				    Clients[i].Name = "Client " + i.ToString();
					Clients[i].ID = i;
				}
			}
			return Clients;
		}
    }
}
