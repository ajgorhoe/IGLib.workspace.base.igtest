// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;

namespace IG.Num
{

    public abstract class ResponseTab1dGeneric<TypeData, TypeEvaluator> : ILockable
        where TypeData : class
        where TypeEvaluator : class
    {

        protected ResponseTab1dGeneric()
        {
        }

        ResponseTab1dGeneric(int numElements, bool centered, double growthFactor,
             double scalingFactor, ref List<double> factors) : this()
        {
        }

        protected abstract void CreateEvaluatorWithStorage();

        protected ResponseEvaluatorWithStorageBase<VectorFunctionResults, IVectorFunction> _responses;

        public ResponseEvaluatorWithStorageBase<VectorFunctionResults, IVectorFunction> Responses
        {
            get { lock (Lock) { return _responses; }  }
            protected set { lock (Lock) { _responses = value; } }
        }


        #region ThreadLocking

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }


        #endregion ThreadLocking


        #region Data

        protected int _numElements = 0;


        protected bool _centered = false;


        protected double growthFactor = 0.0;


        protected double _scalingFactor = 0.0;


        protected List<double> factors = new List<double>();


        #endregion Data





    }

}








