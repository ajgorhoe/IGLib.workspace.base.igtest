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
using System.Threading;
using System.ServiceModel;
using System.Web.Services.Protocols;
using System.ServiceModel.Description;

using IG.Lib;
using IG.Num;
using IG.Script;


namespace IG.Web
{

    /// <summary>Generic class for launching web services.</summary>
    /// <typeparam name="ServiceClass"></typeparam>
    public class WebServiceLauncher<ServiceType>: ILockable
        where ServiceType : WebService, IWSBase, new()
    {


        #region ILockable

        protected object _lock = new object();

        /// <summary>Gets an instantiated object that is used for locking of the current object.</summary>
        public object Lock
        {
            get { return _lock; }
        }

        #endregion ILockable


        #region Data

        /// <summary>Default output level for objects of this class.</summary>
        public static int DefaultOutputLevel = 1;

        protected int _outputLevel = DefaultOutputLevel;

        public int OutputLevel
        {
            get { lock (Lock) { return _outputLevel; } }
            set { lock (Lock) { _outputLevel = value; } }
        }


        protected bool _stopService = false;

        /// <summary>Flag indicating that the service syould be stopped if it is running.</summary>
        public bool StopService
        {
            get { lock (Lock) { return _stopService; } }
            set { lock (Lock) { _stopService = value; } }
        }



        protected bool _busy = false;

        /// <summary>The busy flag. When this is set the service can not be launched.</summary>
        public bool Busy
        {
            get { lock (Lock) { return _busy; } }
            set { lock (Lock) { _busy = value; } }
        }



        protected bool _busySetOutside = false;

        /// <summary>Whether a web service can be set when the busy flag is switched on.</summary>
        public bool BusySetOutside
        {
            get { lock (Lock) { return _busySetOutside; } }
            protected set { _busySetOutside = false; }
        }


        #endregion Data


        #region Operation.Waitcondition


        /// <summary>Default value for minimal sleeping time, in milliseconds, when waiting for a condition to be fulfilled.</summary>
        public static int DefaultMinSleepMs = 5;

        /// <summary>Default value for maximal sleeping time, in milliseconds, when waiting for a condition to be fulfilled.</summary>
        public static int DefaultMaxSleepMs = 500;

        /// <summary>Default value for maximal relative latency, in milliseconds, when waiting for a condition to be fulfilled.</summary>
        public static double DefaultMaxRelativeLatency = 0.05;

        /// <summary>Default value for a flag indicating whether sleep is performed first, when waiting for a condition to be fulfilled.</summary>
        public static bool DefaultSleepFirst = false;


        protected WaitCondition _waiter;

        /// <summary>Object that performs waiting until a particular condition is fulfilled.</summary>
        protected WaitCondition Waiter
        {
            get
            {
                lock (Lock)
                {
                    if (_waiter == null)
                        _waiter = new WaitCondition();
                    _waiter.MinSleepMs = DefaultMinSleepMs;
                    _waiter.MaxSleepMs = DefaultMaxSleepMs;
                    _waiter.SleepFirst = DefaultSleepFirst;
                    _waiter.MaxRelativeLatency = DefaultMaxRelativeLatency;
                }
                return _waiter;
            }
        }


        /// <summary>Minimal sleeping time, in milliseconds, when waiting a condition to be fulfilled.</summary>
        public int MinSleepMs
        {
            get { lock (Lock) { return Waiter.MinSleepMs; } }
            set { lock (Lock) { Waiter.MinSleepMs = value; } }
        }

        /// <summary>Maximal sleeping time, in milliseconds, when waiting a condition to be fulfilled.</summary>
        public int MaxSleepMs
        {
            get { lock (Lock) { return Waiter.MaxSleepMs; } }
            set { lock (Lock) { Waiter.MaxSleepMs = value; } }
        }

        /// <summary>Maximal relative latency when waiting a condition to be fulfilled.</summary>
        public double MaxRelativeLatencyMs
        {
            get { lock (Lock) { return Waiter.MaxRelativeLatency; } }
            set { lock (Lock) { Waiter.MaxRelativeLatency = value; } }
        }

        /// <summary>Whether sleeping is performed first when waiting for a condition to be fulfilled.</summary>
        public bool SleepFirst
        {
            get { lock (Lock) { return Waiter.SleepFirst; } }
            set { lock(Lock) { Waiter.SleepFirst = value; } }
        }



        /// <summary>Waits until the specified condition is fulfilled, i.e. untill the <param>condition</param>
        /// returns true.</summary>
        /// <param name="condition">Delegate that returns true when stipping condition is fulfilled and false
        /// otherwise.</param>
        protected virtual void Wait(ConditionDelegateBase condition)
        {
            WaitCondition waiter;
            lock (Lock)
            {
                waiter = Waiter;
                waiter.ConditionDelegate = condition; ;
            }
            Waiter.Wait();
        }


        #endregion Operation.WaitCondition


        #region Operation

        protected ServiceType _service;

        public ServiceType Service
        {
            get
            {
                lock (Lock)
                {
                    if (_service == null)
                    {
                        _service = new ServiceType();
                    }
                    return _service;
                }
            }
            set { lock (Lock) { _service = value; } }
        }

        
        public static string DefaultBaseUrl = "http://localhost:8080/";

        protected string _url;

        public string Url
        {
            get {
                lock (Lock)
                {
                    if (_url == null)
                    {
                        if (_service != null)
                        {
                            _url = _service.Url;
                        } else
                        {
                            _url = DefaultBaseUrl;
                        }
                    }
                    return _url;
                }
            }
            set
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException("Service URL not specified (null or empty string).");
                    _url = value;
                }
            }
        }
        

        /// <summary>Creates the web service and launches it.</summary>
        public void Launch()
        {
            bool busyAcquired = false;
            ConditionDelegateBase condition = delegate()
            {
                lock (Lock)
                {
                    return (!Busy || BusySetOutside);
                }
            };
            do
            {
                lock (Lock)
                {
                    if (condition())
                    {
                        // Set the busy flag
                        if (!BusySetOutside)
                            Busy = true;
                        busyAcquired = true;
                    }
                }
                if (!busyAcquired)
                {
                    Wait(condition);
                }
            } while (!busyAcquired);
            try
            {

                // Launch the web service:
                StopService = false;

                if (string.IsNullOrEmpty(Url))
                    throw new InvalidOperationException("Base URL where service should be launched is not specified.");
                Uri baseAddress = new Uri(Url);
                if (Service == null)
                    throw new InvalidOperationException("The service object is not instantiated.");
                // Service = new ServiceType();
                Service.Url = Url;

                // using (ServiceHost host = new ServiceHost(typeof(ServiceType), baseAddress)) {}
                // Create the ServiceHost.
                using (ServiceHost host = new ServiceHost(Service, baseAddress))
                {
                    // Enable metadata publishing:
                    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    smb.HttpGetEnabled = true;
                    smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                    host.Description.Behaviors.Add(smb);

                    // Open the ServiceHost to start listening for messages. Since no endpoints are explicitly configured, 
                    // the runtime will create one endpoint per base address for each service contract implemented
                    // by the service.
                    host.Open();

                    if (OutputLevel > 0)
                    {
                        Console.WriteLine(Environment.NewLine + 
                            "The service named " + Service.GetServiceName() + "started." + Environment.NewLine
                            + "  Base address: " + baseAddress);
                    }
                    
                    //Console.WriteLine("Press <Enter> to stop the service.");
                    //Console.ReadLine();
                    // Service started, now wait until the stop flag is set by somebody:
                    ConditionDelegateBase stopCondition = delegate()
                    {
                        return (StopService);
                    };
                    // Now, wait until the stop condition is fulfilled, i.e. somebody sets the StopService flag:
                    Wait(stopCondition);
                    StopService = false;
                    // Close the ServiceHost.
                    host.Close();
                }
            }
            catch { throw; }
            finally { 
                //if (!BusySetOutside)
                Busy = false; 
            }
        }


        protected Thread _thread = null;

        /// <summary>Creates the web service and launches it in a new theread.</summary>
        public void LaunchInNewThread()
        {
            bool busyAcquired = false;
            ConditionDelegateBase condition = delegate()
            {
                lock (Lock)
                {
                    return (!Busy);
                }
            };
            do
            {
                lock (Lock)
                {
                    if (condition())
                    {
                        // Set the busy flag
                        Busy = true;
                        BusySetOutside = true;  // notify the Launch() method that Busy flag is already set.
                        busyAcquired = true;
                    }
                }
                if (!busyAcquired)
                {
                    Wait(condition);
                }
            } while (!busyAcquired);
            try
            {

                // Call the Launch() method in a new thread:
                lock (Lock)
                {
                    _thread = new System.Threading.Thread (
                        delegate()
                        {
                            Launch();
                        });
                }
                _thread.Start();

            }
            catch { throw; }
            finally
            {
                BusySetOutside = false;
            }
        }  // LaunchInNewThread()



        #endregion Operation




    }  // WebServiceLauncher<>



    public class WSBaseLauncher : WebServiceLauncher<WSBaseClass>
    {




    }




}
