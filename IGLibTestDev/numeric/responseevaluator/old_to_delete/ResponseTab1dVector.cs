// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;

namespace IG.Num
{

    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public class ResponseTab1dVectorFunction : ResponseTab1dGeneric<IVectorFunctionResults, IVectorFunction>
    {

        protected ResponseTab1dVectorFunction()
            : base()
        {  }



        protected override void CreateEvaluatorWithStorage()
        {
            _responses = new ResponseEvaluatorWithStorageVector();
        }


    }


}



