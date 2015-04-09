// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;


namespace IG.Num
{

    /// <summary>Constants used in evaluator classes.
    /// $A Igor Mar09 Aug11;</summary>
    public static class EvaluatorConst
    {

        #region Basic

        public const string FileNameBaseDefault = "data";
        public const string LogfileExtension = ".log";

        public const string ResultfileJsonExtension = ".json";
        public const string ResultfileJsonPrefix = "result_";

        #endregion Basic


        #region Specific

        public const string ResultfileJsonPrefixAnalysis = "analysis_";
        public const string ResultfileJsonPrefixTab1d = "tab1d_";
        public const string ResultfileJsonPrefixTab2d = "tab2d_";

        #endregion Secific


    }


}