// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;
using IG.Num;

namespace IG.NumExperimental
{



    /// <summary>Base class for a variety of response evaluators.
    /// Calculates response at specific parameters, stores calculated responses in a list, etc.
    /// The current generic class can be used for differnt types of respnses (results) and different 
    /// types of object for evaluation of responses, such as vector funcitons or optimization analyses.</summary>
    /// <typeparam name="TypeResults">Type of the objects wheer results of evaluation response are stored.
    /// Type of objects that hold requests for evaluation of respone must be the same.</typeparam>
    /// <typeparam name="TypeFunction">Type of the function that performs response calculation.</typeparam>
    /// $A Igor Jul08;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public abstract class ResponseEvaluatorWithStorageBase<TypeResults, TypeFunction> : ILockable
        where TypeResults: class
        where TypeFunction: class
    {

        #region construction 

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorBase"/> object.</summary>
        public ResponseEvaluatorWithStorageBase(): this(null)
        {  }

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorBase"/> object with
        /// the specified response evaluator that will be used for response evaluation.</summary>
        /// <param name="evaluationFunction">Evaluator object that is used for calculation of respones.</param>
        public ResponseEvaluatorWithStorageBase(TypeFunction evaluationFunction)
        { this.EvaluationObject = evaluationFunction; }

        #endregion construction


        #region ThreadLocking

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        private object _internalLock = new object();

        /// <summary>Used internally for locking access to internal fields.</summary>
        protected object InternalLock { get { return _internalLock; } }

        //private object waitlock = new object();

        ///// <summary>Must be used only for locking waiting the Waiting() block (since it is potentially time consuming).</summary>
        //protected object WaitLock { get { return waitlock; } }

        #endregion ThreadLocking


        #region Data


        private TypeResults _lastResults;

        /// <summary>Results of the last evaluation of response performed by the current response evaluator.</summary>
        public virtual TypeResults LastResults
        {
            get { lock (Lock) { return _lastResults; } }
            set
            {
                lock (Lock)
                {
                    _lastResults = value;
                    if (AddResultsAutomatically)
                        Results.Add(LastResults);
                }
            }
        }

        private bool _addResultsAutomatically = true;

        /// <summary>Flag indicated whether the last evaluated results are automatically added to the list
        /// of results when these results are set (by setting the <see cref="LastResults"/> property).</summary>
        public virtual bool AddResultsAutomatically
        {
            get { lock (Lock) { return _addResultsAutomatically; } }
            set { lock (Lock) { _addResultsAutomatically = value; } }
        }

        private List<TypeResults> _results;

        /// <summary>List of calculated results.
        /// The list is intended to store the results.</summary>
        public List<TypeResults> Results
        {
            get
            {
                lock (Lock)
                {
                    if (_results == null)
                        Results = new List<TypeResults>();
                    return _results;
                }
            }
            protected set
            {
                lock (Lock)
                {
                    _results = value;
                }
            }
        }

        public virtual TypeResults this[int which]
        {
            get 
            {
                lock (Lock)
                {
                    if (Results == null)
                        throw new InvalidDataException("No results defined (null reference).");
                    else if (which < 0 || which > Results.Count)
                        throw new IndexOutOfRangeException("Result index out of range, should be between 0 and " + Results.Count + ", passed: " + which + ".");
                    return Results[which];
                }
            }
            set
            {
                lock (Lock) 
                {
                    if (Results == null)
                        Results = new List<TypeResults>();
                    while (Results.Count < which)
                        Results.Add(null);
                    Results[which] = value;
                }
            }
        }

        #endregion Data


        #region Operation


        public abstract TypeFunction EvaluationObject { get; set; }

        /// <summary>Copies input data from results object.</summary>
        /// <param name="results"></param>
        protected abstract void GetData(TypeResults results);

        /// <summary>Creates and returns a copy of the speccified results object.</summary>
        /// <param name="results">Results object to be copied.</param>
        /// <returns>A copy of results object.</returns>
        protected abstract TypeResults CopyResults(TypeResults results);

        /// <summary>Creates and returns request for response evaluation at the specified
        /// parameters, where request options are transcribed from the current response evaluator.</summary>
        /// <param name="parameters">Vector of parameters for which response will be calculated.</param>
        /// <returns>Object containing complete request data for response evaluation.</returns>
        protected abstract TypeResults CreateRequestThis(IVector parameters);

        /// <summary>Evaluates the response.</summary>
        /// <param name="results">Object that must contain complete response evaluation request data,
        /// and where results of response evaluation will be stored.</param>
        protected abstract void EvaluateResponseThis(TypeResults requestAndResponse);

        /// <summary>Evaluates the response at specific parameters.</summary>
        /// <param name="parameters"></param>
        public virtual void EvaluateResponse(IVector parameters)
        {
            lock (Lock)
            {
                if (parameters == null)
                    throw new ArgumentNullException("Vector of parameters is not specified (null reference).");
                TypeResults requestAndResults = CreateRequestThis(parameters);
                EvaluateResponseThis(requestAndResults);
                LastResults = requestAndResults;
            }
        }

        public virtual void EvaluateResponse(TypeResults requestAndResponse)
        {
            lock (Lock)
            {
                if (requestAndResponse == null)
                    throw new ArgumentNullException("Response evaluation request data not specified (null reference).");
                GetData(requestAndResponse);
                EvaluateResponseThis(requestAndResponse);
                LastResults = CopyResults(requestAndResponse);
            }
        }
        /// <summary>Removes all results from the list of results.
        /// The list is intended to store the results.</summary>
        public void ClearResults()
        {
            lock (Lock)
            {
                Results.Clear();
            }
        }

        /// <summary>Adds a new result to the list of results.
        /// The list is intended to store the results.</summary>
        /// <param name="result">Object containing results to be added.</param>
        public void AddResult(TypeResults result)
        {
            Results.Add(result);
        }

        #endregion Operation


    }  // class AnalysisEvaluatorBase<ResultsType, FunctionType>


}