// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

        /*
         DERIVED PROXY CLASSES FOR ACCESSING WEB SERVICES
         
         This file contains derived proxy classes for accessing web services.
         
         the intention is that definition of base classes of these classes can be easily changed by use
         of the using directives and the so called pointer references.
         
         To define different base proxy classes for web services, just change the pointer values in the 
         using directives proxy classes will automatically switch to different proxy classes as their base.
         All classes that access web services will then work (or at least compile) even if web references 
         change or are not accessible. 
         
         For not to break compiling, there are pointers to base classes that are defined at the 1st level.
         
         In this way, if service references are changed or break for some reason (e.g. you have your code on
         a system where web services can not be deployed), you can easily change which proxy classes are 
         actually used, and some of these don't actually interface the web services but they only ensure 
         here that comiling is not broken.
         */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//// Pointers to classes that always work, to aovid breakng of the compiling process:
//using WSBasePointer = IG.Web.WSBaseClass;
//using WSDevelopPointer = IG.Web.WSDevelopClass;
//using WSDevelop1Pointer = IG.Web.WSDevelop1Class;


// Pointers to classess that actully proxy web services:
using WSBasePointer = IG.Lib.WsBaseRef.WSBase;
using WSDevelopPointer = IG.Lib.WSDevelopRef.WSDevelop;
using WSDevelop1Pointer = IG.Lib.WSDevelop1Ref.WSDevelop1;


namespace IG.Web
{



    /// <summary>Intermediate proxy class to a web service based on the <see cref="WSBaseClass"/>.</para></summary>
    /// <remarks><para>This class can inherit either from the automatically generated web service proxy class,
    /// or from the web service' base class when generation of proxy fails.</para></remarks>
    public abstract class WSBaseRefBase : WSBasePointer, IWSBase
    {
    }


    /// <summary>Intermediate proxy class to a web service based on the <see cref="WSDevelopClass"/>.</summary>
    /// <remarks><para>This class can inherit either from the automatically generated web service proxy class,
    /// or from the web service' base class when generation of proxy fails.</para></remarks>
    public abstract class WSDevelopRefBase : WSDevelopPointer  // , IWsDevelop
    {
    } 

    /// <summary>Intermediate proxy class to a web service based on the <see cref="WSDevelop1Class"/>.</summary>
    /// <remarks><para>This class can inherit either from the automatically generated web service proxy class,
    /// or from the web service' base class when generation of proxy fails.</para></remarks>
    public abstract class WSDevelop1RefBase : WSDevelop1Pointer // , IWsDevelop1
    {


    }  // class WSDevelop1Ref


}
