// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;

namespace IG.Num
{



    /// <summary>Base class for a variety of response evaluators.
    /// Calculates response at specific parameters, stores calculated responses in a list, etc.
    /// The current generic class can be used for differnt types of respnses (results) and different 
    /// types of object for evaluation of responses, such as vector funcitons or optimization analyses.</summary>
    /// <typeparam name="TypeResults">Type of the objects wheer results of evaluation response are stored.
    /// Type of objects that hold requests for evaluation of respone must be the same.</typeparam>
    /// <typeparam name="TypeEvaluator">Type of the function that performs response calculation.</type>
    /// <remarks>
    /// <para>This class provides a generic way for performing evaluation of response at specified parameters 
    /// and for storing a list of evaluated responses on the followng grounds: </para>
    /// <para>* Response is evaluated by a special designated evaluation object.</para>
    /// <para>* Input parameters for response evaluation are specified as vector.</para>
    /// <para>* Evaluation object performs evaluation on basis of object of a special class that contains 
    /// the input parameters, the request flags specifying what and how should be evaluated, and results.</para>
    /// <para></para>
    /// <para></para>
    /// </remarks>
    /// $A Igor Jul08;
    public abstract class ResponseEvaluatorWithStorageBase<TypeData, TypeEvaluator>: ILockable
        where TypeData: class
        where TypeEvaluator: class
    {

        #region construction 

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorBase"/> object.</summary>
        public ResponseEvaluatorWithStorageBase(): this(null)
        {  }

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorBase"/> object with
        /// the specified response evaluator that will be used for response evaluation.</summary>
        /// <param name="evaluationFunction">Evaluator object that is used for calculation of respones.</param>
        public ResponseEvaluatorWithStorageBase(TypeEvaluator evaluationFunction)
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


        private TypeData _lastResults;

        /// <summary>Results of the last evaluation of response performed by the current response evaluator.</summary>
        public virtual TypeData LastResults
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

        private List<TypeData> _results;

        /// <summary>List of calculated results.
        /// The list is intended to store the results.</summary>
        public List<TypeData> Results
        {
            get
            {
                lock (Lock)
                {
                    if (_results == null)
                        Results = new List<TypeData>();
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

        public virtual TypeData this[int which]
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
                        Results = new List<TypeData>();
                    while (Results.Count < which)
                        Results.Add(null);
                    Results[which] = value;
                }
            }
        }

        #endregion Data


        #region Operation


        public abstract TypeEvaluator EvaluationObject { get; set; }

        /// <summary>Copies input data from results object.</summary>
        /// <param name="results"></param>
        protected abstract void GetData(TypeData results);

        /// <summary>Creates and returns a copy of the speccified results object.</summary>
        /// <param name="results">Results object to be copied.</param>
        /// <returns>A copy of results object.</returns>
        protected abstract TypeData CopyResults(TypeData results);

        /// <summary>Creates a request for response evaluation at the specified
        /// parameters, where request options are transcribed from the current response evaluator.</summary>
        /// <param name="parameters">Vector of parameters for which response will be calculated.</param>
        /// <param name="request">Reference to the object where request data for response evaluation is written to.</param>
        protected abstract void CreateRequestThis(IVector parameters, ref TypeData request);

        /// <summary>Creates and returns request for response evaluation at the specified
        /// parameters, where request options are transcribed from the current response evaluator.</summary>
        /// <param name="parameters">Vector of parameters for which response will be calculated.</param>
        /// <returns>Object where request data for response evaluation is written to.</returns>
        protected TypeData CreateRequestThis(IVector parameters)
        {
            TypeData request = null;
            CreateRequestThis(parameters, ref request);
            return request;
        }

        /// <summary>Evaluates the response.</summary>
        /// <param name="results">Object that must contain complete response evaluation request data,
        /// and where results of response evaluation will be stored.</param>
        protected abstract void EvaluateResponseThis(TypeData requestAndResponse);

        /// <summary>Evaluates the response at specific parameters.</summary>
        /// <param name="parameters"></param>
        public virtual void EvaluateResponse(IVector parameters)
        {
            lock (Lock)
            {
                if (parameters == null)
                    throw new ArgumentNullException("Vector of parameters is not specified (null reference).");
                TypeData requestAndResults = null;
                CreateRequestThis(parameters, ref requestAndResults);
                EvaluateResponseThis(requestAndResults);
                LastResults = requestAndResults;
            }
        }

        public virtual void EvaluateResponse(TypeData requestAndResponse)
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
        public void AddResult(TypeData result)
        {
            Results.Add(result);
        }

        #endregion Operation


    }  // class AnalysisEvaluatorBase<ResultsType, FunctionType>


}