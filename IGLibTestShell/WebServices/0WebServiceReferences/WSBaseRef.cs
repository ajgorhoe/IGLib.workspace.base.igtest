// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IG.Web
{



    /// <summary>Class for referencing a web service based on the <see cref="WSBaseClass"/>. </summary>
    /// <remarks><para>In the conditions where web service proxy can not be obtained, this clas' base will inherit 
    /// from the service' base clase instead of it's generated proxy class.</para></remarks>
    public class WSBaseRef : WSBaseRefBase, IWSBase
    {
    }


}
