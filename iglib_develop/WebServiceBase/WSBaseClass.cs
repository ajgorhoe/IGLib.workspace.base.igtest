// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


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
using System.Web.Services.Protocols;


namespace IG.Web
{



    /// <summary>Base class for IGLib webservices.</summary>
    //[WebService(Namespace = WSBaseBaseWeb.DefaultNamespace,   
    //            Description = "This is a demonstration WebService.")]
    public class WSBaseClass : System.Web.Services.WebService, IWSBase,
        IIdentifiable, ILockable
    {

        #region Data

        public const string DefaultNamespace = "http://www2.arnes.si/~ljc3m2/igor/iglib/";

        protected string _url;

        /// <summary>URL of the web service.</summary>
        /// <remarks>This is put into Web service such that base service classes can be used instead of proxy classes,
        /// which may be useful when the appropriate service references are not available to generate the proxy classes.</remarks>
        public virtual string Url
        {
            get {
                if (_url == null)
                {
                    lock (Lock)
                    {
                        if (_url == null)
                            Url = "http://localhost:8080/" + this.GetType().Name +".asmx";
                    }
                }
                return _url;
            }
            set { lock (Lock) { this._url = value; } }
        }

        protected CookieContainer _cookieContainer;

        /// <summary>Cookie container, for compatibility with proxy classes that are derived 
        /// from the <see cref="System.Web.Services.Protocols.HttpWebClientProtocol"/> class.</summary>
        public virtual CookieContainer CookieContainer
        {
            get {
                if (_cookieContainer == null)
                {
                    lock (Lock)
                    {
                        // Create a cookie container for the SessionID cookie
                        if (_cookieContainer == null)
                            CookieContainer = new CookieContainer();
                    }
                }
                return _cookieContainer;
            }
            set { lock (Lock) { this._cookieContainer = value; } }
        }

        ///// <summary>Implementation of interface function <see cref="IWSBase.Close"/>, for compatibility 
        ///// with proxy classes that are derived  from the <see cref="System.Web.Services.Protocols.HttpWebClientProtocol"/> class.</summary>
        //public virtual void Close()
        //{  }

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

        protected const int CacheDurationTimeBase = 10;	// seconds

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


        #region Operation.Settings


        protected static string _name;

        /// <summary>Returns name of the web service.
        /// <para>Usually, address (URL) of the web service will consists of some base address and service name.</para></summary>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
        Description = "Returns name of the web service.")]
        public virtual string GetServiceName()
        {
            if (_name == null)
            {
                lock (Lock)
                {
                    if (_name == null)
                        _name = this.GetType().Name;
                }
            }
            return _name;
        }


        /// <summary>Sets name of the web service.
        /// <para>Usually, address (URL) of the web service will consists of some base address and service name.</para></summary>
        [WebMethod(EnableSession = true,
        Description = "Sets name of the web service.")]
        public virtual void SetServiceName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name of the web service can not be a null or empty string.");
            _name = name;
        }


        public static string VarNameOutputLevel = "OutputLevel";

        // private static int _outputLevel;

        /// <summary>Internal flag indicating the level of output the current object generates (e.g. output to the console).</summary>
        protected int OutputLevel
        {
            get {
                return GetOutputLevel();
            }
            set { 
                SetOutputLevel(value); 
            }
        }


        /// <summary>Sets the level of output generated by the service.</summary>
        /// <param name="level">Level of output generated by the service. 0 or less means that all output should be
        /// suppressed, higher numbers mean more output.</param>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
         Description = "Sets the level of generated output of the service methods (0 means that all output is suppressed).")]
        public void SetOutputLevel(int level)
        {
            lock (Lock)
            {
                Session[VarNameOutputLevel] = OutputLevel;
                // this.OutputLevel = OutputLevel;
            }
        }


        /// <summary>Returns the current level of output generated by the service.</summary>
        /// <param name="level">Level of output generated by the service. 0 or less means that all output should be
        /// suppressed, higher numbers mean more output.</param>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
         Description = "Returns the level of generated output of the service methods (0 means that all output is suppressed).")]
        public int GetOutputLevel()
        {
            lock (Lock)
            {
                int? level = (int?) Session[VarNameOutputLevel];
                if (level == null)
                    level = 0;
                return level.Value;
                // return OutputLevel;
            }
        }


        #endregion Operation.Settings


        #region WebMethods



        /// <summary>Tests whether the web service is alive.
        /// <para>Returns a string identifying web service' class and object ID.</para>
        /// <para>To test, right-click the Web Service's .asmx file and select View in a browser.</para></summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true,  // CacheDuration = CacheHelloWorldTime,
        Description = "Tests whether the service functions is alive. Returns a stiring containing some data about the service (e.g. its class and ID) and a ist of arguments passed to the service.")]
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
         Description = "Tests whether the service functions is alive. Returns a stiring containing some data about the service (e.g. its class and ID) and a ist of arguments passed to the service.")]
        public string TestServiceCmd(string commandlineArguments)
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
         Description = "Tests whether the service functions is alive. Returns a stiring containing some data about the service (e.g. its class and ID) and a ist of arguments passed to the service.")]
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
            if (OutputLevel >= 1)
            {
                sb.AppendLine(Environment.NewLine + "Session variables: ");
                System.Collections.Specialized.NameObjectCollectionBase.KeysCollection keys = Session.Keys;
                if (keys.Count < 1)
                    sb.AppendLine("  No variables are defined.");
                for (int i = 0; i < keys.Count; ++i)
                {
                    sb.AppendLine("  " + keys[i] + " = \'" + Session[keys[i]] + "\'.");
                }
            }
            return sb.ToString();
        }



        protected static int _testCount;

        /// <summary>Web service method example. Increments a static counter.
        /// <para>To test, right-click the Web Service's .asmx file and select View in Browser</para></summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true,   // CacheDuration = CacheHelloWorldTime,
         Description = "Increments and retrns a new ID.")]
        public string TestCount()
        {
            ++_testCount;
            return "This is a test method call No. " + _testCount + ".";
        }


        #endregion WebMethods


    }  // class WSBase


}
