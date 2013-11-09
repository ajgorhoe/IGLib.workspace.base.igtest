// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// DEVELOPMENT OF WEB SERVICES.


using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Net;

using IG.Lib;
using IG.Num;
using IG.Script;

namespace IG.Web
{



    /// <summary>Base class for IGLib webservices.</summary>
    //[WebService(Namespace = WSBaseBaseWeb.DefaultNamespace,   
    //            Description = "This is a demonstration WebService.")]
    public class WSBase : System.Web.Services.WebService,
        IIdentifiable, ILockable
    {

        #region Data

        public const string DefaultNamespace = "http://www2.arnes.si/~ljc3m2/igor/iglib/";

        protected static string _namespace = DefaultNamespace;

        public static string NameSpace
        {
            get
            {
                if (string.IsNullOrEmpty(_namespace))
                {
                    _namespace = DefaultNamespace;
                }
                return _namespace;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Web service namespace not specified (null or empty string).");
                _namespace = value;
            }
        }

        protected const int CacheHelloWorldTime = 10;	// seconds

        #endregion Data

        #region Interfaces

        #region Interfaces.IIDentifiable

        protected static int _nextId = 0;

        /// <summary>Returns the next service object ID to be assigned.</summary>
        public static int NextIdToBeAssigned
        {
            get { lock (Util.LockGlobal) { return _nextId; } }
        }

        /// <summary>Returns a new ID for a web service object.</summary>
        protected static int GetNextId()
        {
            lock (Util.LockGlobal)
            {
                ++_nextId;
                return _nextId;
            }
        }

        protected int _id = GetNextId();

        public virtual int Id
        {
            get { return _id; }
            protected set
            {
                throw new ArgumentException("IGLib's web service's ID can not be set.");
            }
        }

        #endregion Interfaces.IIDentifiable

        #region Interfaces.ILockable

        private readonly object _lock = new object();

        /// <summary>Object used for locking of the current object.</summary>
        public object Lock
        {
            get { return _lock; }
        }

        #endregion Interfaces.ILockable

        #endregion Interfaces


        #region WebMethods


        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
         Description = "Tests whether the service functions is alive. Returns a stiring containing some data about the service (e.g. its calas and ID) and a ist of arguments passed to the service.")]
        public string TestService()
        {
            return TestServiceArgs(null);
            // return "This is a web service of type " + this.GetType().FullName + ", ID = " + this.Id + ".";
        }


        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
         Description = "Tests whether the service functions is alive. Returns a stiring containing some data about the service (e.g. its calas and ID) and a ist of arguments passed to the service.")]
        public string TestServiceArg(string commandlineArguments)
        {
            string[] args = UtilStr.GetArgumentsArray(commandlineArguments);
            return TestServiceArgs(args);
        }

        protected const string VarNumTestCalls = "NumTestCalls__IGLib_Service_Base";

        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
         Description = "Tests whether the service functions is alive. Returns a stiring containing some data about the service (e.g. its calas and ID) and a ist of arguments passed to the service.")]
        public string TestServiceArgs(string[] commandlineArguments)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This is a web service of type " + this.GetType().FullName + ", ID = " + this.Id + ".");
            // get the number of calls to the test method from teh Session State:
            int? count = (int?)Session[VarNumTestCalls];
            if (count == null)
                count = 0;
            // increment and store the count
            count++;
            Session[VarNumTestCalls] = count;
            sb.AppendLine("Number of calls to this method within the session: " + count + ".");
            sb.AppendLine("Commandline arguments passed:");
            if (commandlineArguments == null)
                sb.AppendLine("  null");
            else if (commandlineArguments.Length == 0)
                sb.AppendLine("  None.");
            else for (int i = 0; i < commandlineArguments.Length; ++i)
                    sb.AppendLine("  " + i + ": \"" + commandlineArguments[i] + "\"");
            return sb.ToString();
        }


        ///// <summary></summary>
        ///// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public string HelloWorld(string[] arguments)
        //{
        //    // get the Count out of Session State
        //    int? Count = (int?)Session["Count"];
        //    if (Count == null)
        //        Count = 0;
        //    // increment and store the count
        //    Count++;
        //    Session["Count"] = Count;
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("Arguments:");
        //    if (arguments == null)
        //        sb.AppendLine("null");
        //    else if (arguments.Length == 0)
        //        sb.AppendLine("  No arguments passed.");
        //    else for (int i = 0; i < arguments.Length; ++i)
        //        sb.AppendLine("  " + i + ": " + arguments[i]);
        //    return "Hello World - Call Number: " + Count.ToString() + Environment.NewLine + sb.ToString() ;
        //}




        protected static int _testCount;

        /// <summary>Web service method example. Increments a static counter.
        /// <para>To test, right-click the Web Service's .asmx file and select View in Browser</para></summary>
        /// <returns></returns>
        [WebMethod(  // CacheDuration = CacheHelloWorldTime,
         Description = "Increments and retrns a new ID.")]
        public string TestCount()
        {
            ++_testCount;
            return "This is a test method call No. " + _testCount + ".";
        }


        #endregion WebMethods


        #region Examples





        public static void ExampleUseServiceWithSessions(object sender, EventArgs e)
        {

            WcfService_WebDev_FromTemplate.WSBaseRef.WSBase ws;

            ws = new WcfService_WebDev_FromTemplate.WSBaseRef.WSBase();

            ws.TestServiceArg("arg1 arg2 arg3");

            //// create a container for the SessionID cookie

            ws.CookieContainer = new CookieContainer();

            // WSBase ref = null;            

            //// instantiate the proxy 

            //localhost.MyDemo MyService = new localhost.MyDemo();


            //// create a container for the SessionID cookie

            //MyService.CookieContainer = new CookieContainer();

            //// call the Web Service function

            //Label1.Text += MyService.HelloWorld() + "<br />";

        }

        #endregion Examples



    }  // class WSBase


}
