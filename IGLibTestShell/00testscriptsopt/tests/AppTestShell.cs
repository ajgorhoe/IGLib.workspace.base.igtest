// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// TESTING SCRIPT FILE: various examples.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.ServiceModel;
using System.Net;

using IG.Num;
using IG.Lib;

using IG.Forms;
using IG.Plot2d;
using IG.Gr3d;
using IG.Neural;
using IG.Neural.Forms;
using IG.Neural.Forms.Old;
using IG.Neural.Applications;
using IG.Web;
using IG.Web.Forms;

namespace IG.Script
{


    /// <summary>Internal script for running embedded applications.</summary>
    /// <remarks>
    /// <para>In the applications that have the command-line interpreter, embedded applications from this class can typically be
    /// run in the following way:</para>
    /// <para>  AppName Internal IG.Script.AppShellExt CommandName arg1 arg2 ...</para>
    /// <para>where AppName is the application name, IG.Script.AppBase is the full name of the script class that contains
    /// embedded applications, CommandName is name of the command thar launches embedded application, and arg1, arg2, etc.
    /// are command arguments for the embedded application.</para></remarks>
    /// <seealso cref="ScriptAppBase"/>
    /// $A Igor Sep12;
    public class AppTestShell : AppExtBase, ILoadableScript
    {

        public AppTestShell()
            : base()
        { }

        

        #region Commands




        /// <summary>Name of the command for the demonstrational and testing applications for things in development.</summary>
        public const string ConstDevelop = "Develop";
        public const string ConstHelpDevelop =
@"Various applications for testing and demonstrating things in development. 
  Run with the '?' argument to see which applications are available.";



        /// <summary>Name of the command for Web services and clients applications.</summary>
        public const string ConstWebService = "WebService";
        public const string ConstHelpWebService =
@"Various applications for web services and their clients. 
  Run with the '?' argument to see which applications are available.";

        /// <summary>Name of the command for the neural networks-related embedded applications.</summary>
        public const string ConstNeuralDemo = "NeuralDemo";
        public const string ConstHelpNeuralDemo =
@"Various neural network approximation-related embedded demo applications. 
  Run with the '?' argument to see which applications are available.";


        #endregion Commands



        /// <summary>Adds commands to the internal interpreter.</summary>
        /// <param name="interpreter">Interpreter where commands are executed.</param>
        /// <param name="helpStrings">List containg help strings.</param>
        public override void Script_AddCommands(ICommandLineApplicationInterpreter interpreter, SortedList<string, string> helpStrings)
        {
            base.Script_AddCommands(interpreter, helpStrings);
            Script_AddCommand(interpreter, helpStrings, ConstDevelop, AppDevelop, ConstHelpDevelop);
            Script_AddCommand(interpreter, helpStrings, ConstWebService, AppWebService, ConstHelpWebService);
            Script_AddCommand(interpreter, helpStrings, ConstNeuralDemo, AppNeuralDemo, ConstHelpNeuralDemo);
        }


        #region Actions



        #region Actions.DataStructures_Inherited


        #region Actions.DataStructures.TestMultiDimArray

        public const string DataStructuresTestMultiDimArrayApp = "TestMultiDimArray";

        protected const string DataStructuresHelpTestMultiDimArrayApp = DataStructuresTestMultiDimArrayApp + " <dim1> <dim2> <dim3> ... : Runs the multidimensional array demo application.";

        /// <summary>Executes embedded application - demo application for demonstration of work with multidimensional arrays.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string DataStructuresFunctionTestMultiDimArrayApp(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the Multidimensional arrays demo..." + Environment.NewLine);

            if (numArgs == 0)
            {
                Console.WriteLine("Dimensions not specified, running test for default dimensions." + Environment.NewLine);
                MultiDimensionalArray<int>.Example();
            } else
            {
                int[] dimensions = new int[numArgs];
                for (int i = 0; i < numArgs; ++i)
                {
                    dimensions[i] = int.Parse(args[i]);
                }
                MultiDimensionalArray<int>.Example(dimensions);
            }

            Console.WriteLine(Environment.NewLine + "Multidimensional arrays demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.DataStructures.TestMultiDimArray


        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected override void InitAppDataStructures()
        {
            lock (Lock)
            {
                base.InitAppDataStructures();

                AddDataStructuresCommand(DataStructuresTestMultiDimArrayApp, DataStructuresFunctionTestMultiDimArrayApp, DataStructuresHelpTestMultiDimArrayApp);
                
            }
        }

        #endregion Actions.DataStructures_Inherited


        #region Actions.FormDemos_Inherited


        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected override void InitAppFormDemo()
        {

            lock (Lock)
            {
                if (_appFormDemoCommandsInitialized)
                    return;
                //AddFormDemoCommand(FormDemoWindowPositioning, FormDemoFunctionWindowPositioning, FormDemoHelpWindowPositioning);
                base.InitAppFormDemo();
            }
        }

        #endregion Actions.FormDemos_Inherited


        #region Actions.Develop


        /// <summary>List of installed data structure - related demo command names.</summary>
        protected List<string> AppDevelopNames = new List<string>();

        /// <summary>List of help strings corresponding to the installed data structure - related demo commands.</summary>
        protected List<string> AppDevelopHelpStrings = new List<string>();

        /// <summary>List of methods used to perform data structure - related demo commmands.</summary>
        protected List<CommandMethod> AppDevelopMethods = new List<CommandMethod>();

        /// <summary>Adds a new data structure- related embedded demo application's command (added as 
        /// a sub-command of the base command named <see cref="ConstDevelop"/>).</summary>
        /// <param name="appName">Application name.</param>
        /// <param name="appMethod">Method used to perform the application.</param>
        /// <param name="appHelp">Eventual help string for the application.</param>
        protected void AddDevelopCommand(string appName, CommandMethod appMethod, string appHelp)
        {
            lock (Lock)
            {
                AppDevelopNames.Add(appName.ToLower());
                AppDevelopHelpStrings.Add(appHelp);
                AppDevelopMethods.Add(appMethod);
            }
        }




        #region Actions.Develops.TestZeroFinderNewtonApp

        public const string DevelopTestZeroFinderNewtonApp = "TestZeroFinderNewton";

        protected const string DevelopHelpTestZeroFinderNewtonApp = DevelopTestZeroFinderNewtonApp + " : Runs the example application for finding zeros of a function by the Newton's method.";

        /// <summary>Executes embedded application - demo application for demonstration of finding zeros of a function by the Newton's method.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string DevelopFunctionTestZeroFinderNewtonApp(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning demo of the Newton's method for function zeros..." + Environment.NewLine);

            ZeroFinderNewton.ExampleNewton();

            Console.WriteLine(Environment.NewLine + "Newton's method demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.Develops.TestZeroFinderNewtonApp


        // MultiDimArray MultiDimArray


        protected bool _appDevelopCommandsInitialized = false;

        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected virtual void InitAppDevelop()
        {

            lock (Lock)
            {
                if (_appDevelopCommandsInitialized)
                    return;
                AddDevelopCommand(DevelopTestZeroFinderNewtonApp, DevelopFunctionTestZeroFinderNewtonApp, DevelopHelpTestZeroFinderNewtonApp);

                _appDevelopCommandsInitialized = true;
            }
        }


        /// <summary>Runs a data structures demo - related utility (embedded application) according to arguments.</summary>
        /// <param name="args">Arguments. 0-th argument is the base command name, 1st argument is the embedded application name, and the rest
        /// are arguments that are used by the embedded application.</param>
        protected virtual string RunAppDevelop(string[] args)
        {
            InitAppDevelop();
            if (args == null)
                throw new ArgumentException("No arguments. Embedded application name (or '?' for help) should be specified (null argument array).");
            if (args.Length < 2)
                throw new ArgumentException("Test name (or '?' for help) should be specified (less than 2 arguments).");
            if (string.IsNullOrEmpty(args[1]))
                throw new ArgumentException("Test name (or '?' for help) not specified (null or empty string argument).");
            if (args != null)
                if (args.Length >= 2)
                    if (args[1] == "?")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usage: " + args[0] + " ApplicationName arg1 arg2...");
                        Console.WriteLine(args[0] + " ApplicationName ? : prints help.");
                        Console.WriteLine();
                        Console.WriteLine("List of embedded applications: ");
                        for (int i = 0; i < AppDevelopNames.Count; ++i)
                            Console.WriteLine("  " + AppDevelopNames[i]);
                        Console.WriteLine();
                        return null;
                    }
            string testName = args[1];
            string[] appParams = new string[args.Length - 2];
            for (int i = 0; i < appParams.Length; ++i)
                appParams[i] = args[i + 2];
            int index = AppDevelopNames.IndexOf(testName.ToLower());
            if (index < 0)
                throw new ArgumentException("Embedded application named " + testName + " is not found. Call with '?' for list of application names.");
            testName = AppDevelopNames[index];
            string helpString = AppDevelopHelpStrings[index];
            CommandMethod method = AppDevelopMethods[index];
            if (appParams.Length >= 1)
                if (appParams[0] == "?")
                {
                    // Print help for the specified surface:
                    Console.WriteLine();
                    Console.WriteLine("Test " + testName + ": ");
                    if (string.IsNullOrEmpty(AppDevelopHelpStrings[index]))
                        Console.WriteLine("  No help available.");
                    Console.WriteLine(AppDevelopHelpStrings[index]);
                    Console.WriteLine();
                    return null;
                }
            try
            {
                return method(testName, appParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("Usage: " + Environment.NewLine + helpString + Environment.NewLine);
                throw;
            }
        }

        /// <summary>Runs one of the data structures demo - related embedded applications.</summary>
        /// <param name="arguments">Array containing the base command name, application command name and arguments.</param>
        public virtual string AppDevelop(string[] arguments)
        {
            string ret = null;
            if (arguments == null)
                throw new ArgumentException("Commandline arguments not specified (null argument).");
            if (arguments.Length < 2)
                Console.WriteLine("Number of arguments should be at least 2 (at least base command name & application name).");
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Running form demo - related embedded application..."
                + Environment.NewLine +
                "=============================="
                + Environment.NewLine);
            //Console.WriteLine();
            //Script_PrintArguments("Application arguments: ", arguments);
            //Console.WriteLine();

            if (ret == null)
                ret = RunAppDevelop(arguments);


            Console.WriteLine("==============================");
            Console.WriteLine("Form demo - related application  finished.");
            Console.WriteLine();
            return ret;
        }  // AppDevelop



        #endregion Actions.Develop


        #region Actions.WebService


        /// <summary>List of installed data structure - related demo command names.</summary>
        protected List<string> AppWebServiceNames = new List<string>();

        /// <summary>List of help strings corresponding to the installed data structure - related demo commands.</summary>
        protected List<string> AppWebServiceHelpStrings = new List<string>();

        /// <summary>List of methods used to perform data structure - related demo commmands.</summary>
        protected List<CommandMethod> AppWebServiceMethods = new List<CommandMethod>();

        /// <summary>Adds a new data structure- related embedded demo application's command (added as 
        /// a sub-command of the base command named <see cref="ConstWebService"/>).</summary>
        /// <param name="appName">Application name.</param>
        /// <param name="appMethod">Method used to perform the application.</param>
        /// <param name="appHelp">Eventual help string for the application.</param>
        protected void AddWebServiceCommand(string appName, CommandMethod appMethod, string appHelp)
        {
            lock (Lock)
            {
                AppWebServiceNames.Add(appName.ToLower());
                AppWebServiceHelpStrings.Add(appHelp);
                AppWebServiceMethods.Add(appMethod);
            }
        }


        #region Actions.WebServices.TestWebServiceClient

        public const string WebServiceTestWebServiceClient = "TestClient";

        protected const string WebServiceHelpTestWebServiceClient = WebServiceTestWebServiceClient + " : Runs a GUI client for testing web services.";

        /// <summary>Runs a GUI client for testing web serivices.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string WebServiceFunctionTestWebServiceClient(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning a GUI client for web service testing..." + Environment.NewLine);

            Application.Run(new WSClientsForm());

            Console.WriteLine(Environment.NewLine + "Service testing finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.WebServices.TestWebServiceClient


        #region Actions.WebServices.CustomTest

        public const string WebServiceCustomTest = "CustomTest";

        protected const string WebServiceHelpCustomTest = WebServiceCustomTest + " : Runs the custom web service test - a client communicating with the IIS hosted service.";

        /// <summary>Executes embedded application - Custom web service test.
        /// <para>Test communication with some web services developed within IGLib.</para></summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string WebServiceFunctionCustomTest(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning custom web service tests..." + Environment.NewLine);
            Console.WriteLine("WARNING: " + Environment.NewLine + "Before running these tests, open in browser the ASMX files of all the web services involved!" );


            string baseUrl = null; // "http://localhost:8080/";
            string argStr = null;
            string[] stringArgs = null;
            string resultStr = null;

            if (numArgs > 0)
            {
                if (!string.IsNullOrEmpty(args[0]))
                {
                    baseUrl = args[0];
                }
            }

            if (!string.IsNullOrEmpty(baseUrl))
                Console.WriteLine(Environment.NewLine + "Base URL of services: " + baseUrl + Environment.NewLine);
            else
                Console.WriteLine(Environment.NewLine + "Base URL of services is not specified, using proxy class' URL." + Environment.NewLine);


            // Test the WSBase web service through the proxy class object as web service client:

            Console.WriteLine("Testing the WSBase web service through the proxy class:");
            WSBaseRef clientWSBase = new WSBaseRef();
            if (!string.IsNullOrEmpty(baseUrl))
            {
                // Set the base Url:
                clientWSBase.Url = baseUrl + clientWSBase.GetServiceName();
            }
            // Create a cookie container for the session ID cookie:
            clientWSBase.CookieContainer = new CookieContainer();

            Console.WriteLine("Url is set to " + clientWSBase.Url + Environment.NewLine);
            Console.WriteLine("Service name: " + clientWSBase.GetServiceName() + Environment.NewLine
                + "Service name obtained from WS:" + clientWSBase.GetServiceName() + Environment.NewLine);

            argStr = "\"arg1 arg2 arg3 arg4\"";
            Console.WriteLine("TestServiceCmd(" + argStr + ") :");
            resultStr = clientWSBase.TestServiceCmd(argStr);
            Console.WriteLine(Environment.NewLine + resultStr + Environment.NewLine);

            argStr = "\"arg1 arg2 arg3 arg4\"";
            Console.WriteLine("TestServiceCmd(" + argStr + ") :");
            resultStr = clientWSBase.TestServiceCmd(argStr);
            Console.WriteLine(Environment.NewLine + resultStr + Environment.NewLine);

            stringArgs = UtilStr.GetArgumentsArray(argStr);
            Console.WriteLine("TestServiceArgs(" + argStr + "), array of string arguments:");
            resultStr = clientWSBase.TestServiceArgs(stringArgs);
            Console.WriteLine(Environment.NewLine + resultStr + Environment.NewLine);

            int numCalls = 100;
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "SPEED TEST:" + Environment.NewLine
                + "Testing speed by performing " + numCalls + " calls to the web service method TestServiceArgs...");
            StopWatch t = new StopWatch();
            t.Reset();
            t.Start();
            for (int i = 0; i < numCalls; ++i)
            {
                resultStr = clientWSBase.TestServiceArgs(stringArgs);
            }
            t.Stop();
            Console.WriteLine("  ... Done. " + Environment.NewLine
                + "Test duration: " + t.TotalTime + " s, speed: " + ((double)numCalls / t.TotalTime) + " calls / s.");

            Console.WriteLine(Environment.NewLine + Environment.NewLine + "SPEED TEST:" + Environment.NewLine
                + "Testing speed by performing " + numCalls + " calls to the web service method GetServiceName...");
            t.Reset();
            t.Start();
            for (int i = 0; i < numCalls; ++i)
            {
                resultStr = clientWSBase.GetServiceName();
            }
            t.Stop();
            Console.WriteLine("  ... Done. " + Environment.NewLine
                + "Test duration: " + t.TotalTime + " s, speed: " + ((double)numCalls / t.TotalTime) + " calls / s.");

            //proxy.close();

            Console.WriteLine(Environment.NewLine + "Newton's method demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.WebServices.CustomTest


        #region Actions.WebServices.LaunchService

        public const string WebServiceLaunchService = "LaunchService";

        protected const string WebServiceHelpLaunchService = WebServiceLaunchService + " : Launches a web services hosted by the application.";

        /// <summary>Launches a web service hosted by the application.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments of the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string WebServiceFunctionLaunchService(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Launching a web service hosted by the current application..." + Environment.NewLine);

            string url = "http://localhost:8080/WSBase";
            WebServiceLauncher<WSBaseClass> launcher = new WebServiceLauncher<WSBaseClass>();
            launcher.Service = new WSBaseClass();
            launcher.LaunchInNewThread();

            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Press <Enter> to stop the service!");
            Console.ReadLine();
            launcher.StopService = true;

            Console.WriteLine(Environment.NewLine + "Web service finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.WebServices.LaunchService






        protected bool _appWebServiceCommandsInitialized = false;

        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected virtual void InitAppWebService()
        {

            lock (Lock)
            {
                if (_appWebServiceCommandsInitialized)
                    return;
                AddWebServiceCommand(WebServiceTestWebServiceClient, WebServiceFunctionTestWebServiceClient, WebServiceHelpTestWebServiceClient);
                AddWebServiceCommand(WebServiceCustomTest, WebServiceFunctionCustomTest, WebServiceHelpCustomTest);

                _appWebServiceCommandsInitialized = true;
            }
        }


        /// <summary>Runs a data structures demo - related utility (embedded application) according to arguments.</summary>
        /// <param name="args">Arguments. 0-th argument is the base command name, 1st argument is the embedded application name, and the rest
        /// are arguments that are used by the embedded application.</param>
        protected virtual string RunAppWebService(string[] args)
        {
            InitAppWebService();
            if (args == null)
                throw new ArgumentException("No arguments. Embedded application name (or '?' for help) should be specified (null argument array).");
            if (args.Length < 2)
                throw new ArgumentException("Test name (or '?' for help) should be specified (less than 2 arguments).");
            if (string.IsNullOrEmpty(args[1]))
                throw new ArgumentException("Test name (or '?' for help) not specified (null or empty string argument).");
            if (args != null)
                if (args.Length >= 2)
                    if (args[1] == "?")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usage: " + args[0] + " ApplicationName arg1 arg2...");
                        Console.WriteLine(args[0] + " ApplicationName ? : prints help.");
                        Console.WriteLine();
                        Console.WriteLine("List of embedded applications: ");
                        for (int i = 0; i < AppWebServiceNames.Count; ++i)
                            Console.WriteLine("  " + AppWebServiceNames[i]);
                        Console.WriteLine();
                        return null;
                    }
            string testName = args[1];
            string[] appParams = new string[args.Length - 2];
            for (int i = 0; i < appParams.Length; ++i)
                appParams[i] = args[i + 2];
            int index = AppWebServiceNames.IndexOf(testName.ToLower());
            if (index < 0)
                throw new ArgumentException("Embedded application named " + testName + " is not found. Call with '?' for list of application names.");
            testName = AppWebServiceNames[index];
            string helpString = AppWebServiceHelpStrings[index];
            CommandMethod method = AppWebServiceMethods[index];
            if (appParams.Length >= 1)
                if (appParams[0] == "?")
                {
                    // Print help for the specified surface:
                    Console.WriteLine();
                    Console.WriteLine("Test " + testName + ": ");
                    if (string.IsNullOrEmpty(AppWebServiceHelpStrings[index]))
                        Console.WriteLine("  No help available.");
                    Console.WriteLine(AppWebServiceHelpStrings[index]);
                    Console.WriteLine();
                    return null;
                }
            try
            {
                return method(testName, appParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("Usage: " + Environment.NewLine + helpString + Environment.NewLine);
                throw;
            }
        }

        /// <summary>Runs one of the data structures demo - related embedded applications.</summary>
        /// <param name="arguments">Array containing the base command name, application command name and arguments.</param>
        public virtual string AppWebService(string[] arguments)
        {
            string ret = null;
            if (arguments == null)
                throw new ArgumentException("Commandline arguments not specified (null argument).");
            if (arguments.Length < 2)
                Console.WriteLine("Number of arguments should be at least 2 (at least base command name & application name).");
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Running form demo - related embedded application..."
                + Environment.NewLine +
                "=============================="
                + Environment.NewLine);
            //Console.WriteLine();
            //Script_PrintArguments("Application arguments: ", arguments);
            //Console.WriteLine();

            if (ret == null)
                ret = RunAppWebService(arguments);


            Console.WriteLine("==============================");
            Console.WriteLine("Form demo - related application  finished.");
            Console.WriteLine();
            return ret;
        }  // AppWebService



        #endregion Actions.WebService


// Develop Develop Develop

        #region Actions.NeuralDemos

        /// <summary>List of installed neural network approximation - related demo command names.</summary>
        protected List<string> AppNeuralDemoNames = new List<string>();

        /// <summary>List of help strings corresponding to the installed neural network approximation - related demo commands.</summary>
        protected List<string> AppNeuralDemoHelpStrings = new List<string>();

        /// <summary>List of methods used to perform neural network approximation - related demo commmands.</summary>
        protected List<CommandMethod> AppNeuralDemoMethods = new List<CommandMethod>();

        /// <summary>Adds a new neural network approximation - related embedded demo application's command (added as 
        /// a sub-command of the base command named <see cref="ConstNeuralDemo"/>).</summary>
        /// <param name="appName">Application name.</param>
        /// <param name="appMethod">Method used to perform the application.</param>
        /// <param name="appHelp">Eventual help string for the application.</param>
        protected void AddNeuralDemoCommand(string appName, CommandMethod appMethod, string appHelp)
        {
            lock (Lock)
            {
                AppNeuralDemoNames.Add(appName.ToLower());
                AppNeuralDemoHelpStrings.Add(appHelp);
                AppNeuralDemoMethods.Add(appMethod);
            }
        }


        #region Actions.NeuralDemos.TestApp

        public const string NeuralDemoTestApp = "TestApp";

        protected const string NeuralDemoHelpTestApp = NeuralDemoTestApp + " : Runs the neural network approximation simple demo application.";

        /// <summary>Executes embedded application - demo application for approximation with artificial neural networks.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string NeuralDemoFunctionTestApp(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the artificial neural network approximation demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NeuralDemo());

            Console.WriteLine(Environment.NewLine + "ANN approximation demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.NeuralDemos.TestApp


        #region Actions.NeuralDemos.TestAppOld

        public const string NeuralDemoTestAppOld = "TestAppOld";

        protected const string NeuralDemoHelpTestAppOld = NeuralDemoTestAppOld + " : Runs the OLD neural network approximation simple demo application.";

        /// <summary>Executes embedded application - demo application for approximation with artificial neural networks.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string NeuralDemoFunctionTestAppOld(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the artificial neural network approximation demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DemoNeuralOld());

            Console.WriteLine(Environment.NewLine + "ANN approximation demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.NeuralDemos.TestAppOld
        

        #region Actions.NeuralDemos.TestApp1DOld

        public const string NeuralDemoTestApp1DOld = "TestApp1DOld";

        protected const string NeuralDemoHelpTestApp1DOld = NeuralDemoTestApp1DOld + " : Runs the neural network approximation old 1D simple demo application.";

        /// <summary>Executes embedded application - demo application for approximation with artificial neural networks.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string NeuralDemoFunctionTestApp1DOld(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the artificial neural network approximation demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormNeural1DOld());

            Console.WriteLine(Environment.NewLine + "ANN approximation demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.NeuralDemos.TestApp1DOld



        #region Actions.NeuralDemos.TestApp2DOld

        public const string NeuralDemoTestApp2DOld = "TestApp2DOld";

        protected const string NeuralDemoHelpTestApp2DOld = NeuralDemoTestApp2DOld + " : Runs the neural network approximation old 2D simple demo application.";

        /// <summary>Executes embedded application - demo application for approximation with artificial neural networks.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string NeuralDemoFunctionTestApp2DOld(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the artificial neural network approximation demo..." + Environment.NewLine);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormNeural2DOld());

            Console.WriteLine(Environment.NewLine + "ANN approximation demo finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.NeuralDemos.TestApp2DOld
         



        #region Actions.NeuralDemos.ParametricTests

        public const string NeuralDemoParametricTests = "ParametricTests";

        protected const string NeuralDemoHelpParametricTests = NeuralDemoParametricTests + " : Runs the neural network model parametric tests.";

        /// <summary>Executes embedded application - demo application for parametric tests of ANN models.</summary>
        /// <param name="appName">Name of the embedded application.</param>
        /// <param name="args">Arguments fo the embedded application's command.</param>
        /// <returns>Number of arguments passed.</returns>
        protected virtual string NeuralDemoFunctionParametricTests(string appName, string[] args)
        {
            int numArgs = 0;
            if (args != null)
                numArgs = args.Length;
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Runnning the artificial neural network model - based parametric tests..." + Environment.NewLine);

            // GUI FOR PARAMETRIC STUDIES:
            string workingDir = @"c:/COBIK/workspaceprojects/12_01_paper_neuralcasting/train_tadej_01/opt";
            if (UtilSystem.IsUserTadej)
            {
                workingDir = @"c:/COBIK/workspaceprojects/12_01_paper_neuralcasting/train_tadej_01/opt";

                workingDir = "../../../../../../../workspaceprojects/12_01_paper_neuralcasting/train_tadej_01/opt";
            }
            if (UtilSystem.IsUserIgor)
            {
                workingDir = @"d:/users/workspaceprojects/12_01_paper_neuralcasting/train_tadej/opt";
                workingDir = @"d:/users/workspaceprojects/12_01_paper_neuralcasting/train/opt";
                workingDir = "../../../../../../../workspaceprojects/12_01_paper_neuralcasting/train/opt";
            }

            if (!Directory.Exists(workingDir))
            {
                Console.WriteLine(Environment.NewLine + "WARNING: Working directory does not exist: " + Environment.NewLine
                    + "  " + workingDir + Environment.NewLine);
                //Creating a fading message in this thread, which must be canceled explicitly (e.g. by pressing mouse button 3:)
                string msgtitle = "Warning!";
                string msgtext =
@"Working directory does not exist. 

Directory: 
  " + workingDir;
                int showTime = 4000;
                FadingMessage fm = new FadingMessage(msgtitle, msgtext, showTime);
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AnnAppDemo(workingDir));


            Console.WriteLine(Environment.NewLine + "ANN parametric tests finished." + Environment.NewLine);
            return numArgs.ToString();
        }

        #endregion Actions.NeuralDemos.ParametricTests


        protected bool _appNeuralDemoCommandsInitialized = false;

        /// <summary>Initializes commands for form demo related utilities (embedded applications).</summary>
        protected virtual void InitAppNeuralDemo()
        {

            lock (Lock)
            {
                if (_appNeuralDemoCommandsInitialized)
                    return;
                AddNeuralDemoCommand(NeuralDemoTestApp, NeuralDemoFunctionTestApp, NeuralDemoHelpTestApp);
                AddNeuralDemoCommand(NeuralDemoParametricTests, NeuralDemoFunctionParametricTests, NeuralDemoHelpParametricTests);

                // Old tests (left here to be able to look backwards, will be deleted in the future):
                AddNeuralDemoCommand(NeuralDemoTestAppOld, NeuralDemoFunctionTestAppOld, NeuralDemoHelpTestAppOld);
                AddNeuralDemoCommand(NeuralDemoTestApp1DOld, NeuralDemoFunctionTestApp1DOld, NeuralDemoHelpTestApp1DOld);
                AddNeuralDemoCommand(NeuralDemoTestApp2DOld, NeuralDemoFunctionTestApp2DOld, NeuralDemoHelpTestApp2DOld);


                _appNeuralDemoCommandsInitialized = true;
            }
        }


        /// <summary>Runs a form demo - related utility (embedded application) according to arguments.</summary>
        /// <param name="args">Arguments. 0-th argument is the base command name, 1st argument is the embedded application name, and the rest
        /// are arguments that are used by the embedded application.</param>
        protected virtual string RunAppNeuralDemo(string[] args)
        {
            InitAppNeuralDemo();
            if (args == null)
                throw new ArgumentException("No arguments. Embedded application name (or '?' for help) should be specified (null argument array).");
            if (args.Length < 2)
                throw new ArgumentException("Test name (or '?' for help) should be specified (less than 2 arguments).");
            if (string.IsNullOrEmpty(args[1]))
                throw new ArgumentException("Test name (or '?' for help) not specified (null or empty string argument).");
            if (args != null)
                if (args.Length >= 2)
                    if (args[1] == "?")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Usage: " + args[0] + " ApplicationName arg1 arg2...");
                        Console.WriteLine(args[0] + " ApplicationName ? : prints help.");
                        Console.WriteLine();
                        Console.WriteLine("List of embedded applications: ");
                        for (int i = 0; i < AppNeuralDemoNames.Count; ++i)
                            Console.WriteLine("  " + AppNeuralDemoNames[i]);
                        Console.WriteLine();
                        return null;
                    }
            string testName = args[1];
            string[] appParams = new string[args.Length - 2];
            for (int i = 0; i < appParams.Length; ++i)
                appParams[i] = args[i + 2];
            int index = AppNeuralDemoNames.IndexOf(testName.ToLower());
            if (index < 0)
                throw new ArgumentException("Embedded application named " + testName + " is not found. Call with '?' for list of application names.");
            testName = AppNeuralDemoNames[index];
            string helpString = AppNeuralDemoHelpStrings[index];
            CommandMethod method = AppNeuralDemoMethods[index];
            if (appParams.Length >= 1)
                if (appParams[0] == "?")
                {
                    // Print help for the specified surface:
                    Console.WriteLine();
                    Console.WriteLine("Test " + testName + ": ");
                    if (string.IsNullOrEmpty(AppNeuralDemoHelpStrings[index]))
                        Console.WriteLine("  No help available.");
                    Console.WriteLine(AppNeuralDemoHelpStrings[index]);
                    Console.WriteLine();
                    return null;
                }
            try
            {
                return method(testName, appParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("Usage: " + Environment.NewLine + helpString + Environment.NewLine);
                throw;
            }
        }

        /// <summary>Runs one of the form demo - related embedded applications.</summary>
        /// <param name="arguments">Array containing the base command name, application command name and arguments.</param>
        public virtual string AppNeuralDemo(string[] arguments)
        {
            string ret = null;
            if (arguments == null)
                throw new ArgumentException("Commandline arguments not specified (null argument).");
            if (arguments.Length < 2)
                Console.WriteLine("Number of arguments should be at least 2 (at least base command name & application name).");
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Running form demo - related embedded application..."
                + Environment.NewLine +
                "=============================="
                + Environment.NewLine);
            //Console.WriteLine();
            //Script_PrintArguments("Application arguments: ", arguments);
            //Console.WriteLine();

            if (ret == null)
                ret = RunAppNeuralDemo(arguments);


            Console.WriteLine("==============================");
            Console.WriteLine("Form demo - related application  finished.");
            Console.WriteLine();
            return ret;
        }  // AppNeuralDemo



        #endregion Actions.NeuralDemos


        #endregion Actions


    }

}