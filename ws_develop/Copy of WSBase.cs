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

namespace IG.Web
{



    /// <summary>Base class for webservices.</summary>
    [WebService(Namespace = WSBaseBase.DefaultNamespace,   // Namespace = "http://codeproject.com/webservices/",
                Description = "This is a demonstration WebService.")]
    public class WSBase : WSBaseBase,
        IIdentifiable, ILockable
    {


        ///// <summary>Web Service example.
        ///// <para>To test, right-click the Web Service's .asmx file and select View in Browser</para></summary>
        ///// <returns></returns>
        //[WebMethod(
        // Description = "Increments and retrns a new ID.")]
        //public string TestMethod1()
        //{
        //    ++lastId;
        //    return "This is a test method call No. " + lastId + ".";
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



    }  // class WS_DevelopBase


}
