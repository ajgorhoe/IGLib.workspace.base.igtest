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

using IG.Lib;
using IG.Num;
using IG.Script;
using System.Net;

namespace IG.Web
{

    /// <summary>Base class for webservices.</summary>
    [WebService(Namespace = WSBaseClass.DefaultNamespace,   
                Description = "This is a development web service.")]
    public class WSBase : WSBaseClass,   IWSBase,
        IIdentifiable, ILockable
    {  }  // class WSBaseWeb



    /// <summary>Web service class.</summary>
    [WebService(Namespace = WSBaseClass.DefaultNamespace,
                Description = "This is a higher level development web service.")]
    public class WSDevelop : WSDevelopClass,   IWsDevelop, IWSBase
    {



    }  // class WSDevelopWeb



    /// <summary>Web service class.</summary>
    [WebService(Namespace = WSBaseClass.DefaultNamespace,
                Description = "This is a higher level development web service.")]
    public class WSDevelop1 : WSDevelop1Class,   IWsDevelop1, IWSBase
    {  }  // class WS_Develop1Web


    /// <summary>Class containing some examples.</summary>
    public class WSBaseExamples //: WSBase
    {

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



    }  // class WBaseExamples

}
