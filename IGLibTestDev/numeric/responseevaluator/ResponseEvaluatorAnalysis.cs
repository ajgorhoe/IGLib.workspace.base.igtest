// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Base class for response evaluators where response is evaluated by objects of type
    /// <typeparamref name="IAnalysis"/>.</summary>
    /// $A Igor Jul08;
    public class ResponseEvaluatorWithStorageAnalysis : ResponseEvaluatorWithStorageBase<AnalysisResults, IAnalysis>,
        ILockable
    {

        
        #region construction 

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorAnalysis"/> object.</summary>
        public ResponseEvaluatorWithStorageAnalysis(): this(null)
        {  }

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorAnalysis"/> object with
        /// the specified response evaluator (analysis object) that will be used for response evaluation.</summary>
        /// <param name="analysis">Analysis object that is used for calculation of respones.</param>
        public ResponseEvaluatorWithStorageAnalysis(IAnalysis analysis) : base(analysis)
        {  }

        #endregion construction


        #region Overridden

        IAnalysis _analysis;

        /// <summary>Analysis object that is actually used for evaluation of response.</summary>
        public override IAnalysis EvaluationObject 
        {
            get
            {
                lock (Lock)
                {
                    return _analysis;
                }
            }
            set
            {
                _analysis = value;
                if (value == null)
                {
                    this.NumObjectives = 1;
                    this.NumParameters = 0;
                    this.NumConstraints = 0;
                    this.NumEqualityConstraints = 0;
                }
                else
                {
                    this.NumParameters = value.NumParameters;
                    this.NumObjectives = value.NumObjectives;
                    this.NumConstraints = value.NumConstraints;
                    this.NumEqualityConstraints = value.NumEqualityConstraints;
                }
            }
        }


        /// <summary>Copies input data (problem characteristics such as number of parameters, number of 
        /// contraints, etc., and analysis request flags) from the specified analysis results object.</summary>
        /// <param name="results">Analysis results object form which various input data is copied.</param>
        protected override void GetData(AnalysisResults results)
        {
            // Copy problem characteristics:
            this.NumParameters = results.NumParameters;
            this.NumObjectives = results.NumObjectives;
            this.NumConstraints = results.NumConstraints;
            this.NumEqualityConstraints = results.NumEqualityConstraints;
            // Copy request flags:
            this.ReqObjective = results.ReqObjective;
            this.ReqConstraints = results.ReqConstraints;
            this.ReqObjectiveGradient = results.ReqObjectiveGradient;
            this.ReqConstraintGradients = results.ReqConstraintGradients;
            this.ReqObjectiveHessian = results.ReqObjectiveHessian;
            this.ReqConstraintHessians = results.ReqConstraintHessians;
        }

        /// <summary>Creates and returns a copy of the speccified results object.</summary>
        /// <param name="results">Results object to be copied.</param>
        /// <returns>A copy of results object.</returns>
        protected override AnalysisResults CopyResults(AnalysisResults results)
        {
            if (results == null)
                return null;
            AnalysisResults ret = new AnalysisResults();
            ret.Copy(results);
            return ret;
        }


        /// <summary>Creates and returns request for response evaluation at the specified
        /// parameters, where request options are transcribed from the current response evaluator.</summary>
        /// <param name="parameters">Vector of parameters for which response will be calculated.</param>
        /// <returns>Object containing complete request data for response evaluation.</returns>
        /// <param name="request">Reference to the object where request data for response evaluation is written to.</param>
        protected override void CreateRequestThis(IVector parameters, ref AnalysisResults request)
        {
            request = new AnalysisResults();
            // Copy problem characteristics:
            request.NumParameters = this.NumParameters;
            request.NumObjectives = this.NumObjectives;
            request.NumConstraints = this.NumConstraints;
            request.NumEqualityConstraints = this.NumEqualityConstraints;
            // Copy request flags:
            request.ReqObjective = this.ReqObjective;
            request.ReqConstraints = this.ReqConstraints;
            request.ReqObjectiveGradient = this.ReqObjectiveGradient;
            request.ReqConstraintGradients = this.ReqConstraintGradients;
            request.ReqObjectiveHessian = this.ReqObjectiveHessian;
            request.ReqConstraintHessians = this.ReqConstraintHessians;
            // Copy parameters:
            IVector param = parameters.GetCopy();
            request.Parameters = param;
            //return ret;
        }


        /// <summary>Evaluates the response.</summary>
        /// <param name="results">Object that must contain complete response evaluation request data,
        /// and where results of response evaluation will be stored.</param>
        protected override void EvaluateResponseThis(AnalysisResults requestAndResponse)
        {
            if (EvaluationObject == null)
                throw new InvalidOperationException("Can not evaluate response, evaluation funciton is not specified.");
            else
                EvaluationObject.Analyse(requestAndResponse);
        }


        #endregion Overridden


        #region ResponseCharacteristics

        protected int _numParameters = 0;

        protected int _numObjectives = 1;

        protected int _numConstraints = 0;

        protected int _numEqualityConstraints = 0;

        /// <summary>Number of parameters.</summary>
        public virtual int NumParameters 
        { get { return _numParameters; } protected set { _numParameters = value; } }

        /// <summary>Number of objective functions (normally 1 for this type, but can be 0).</summary>
        public virtual int NumObjectives 
        { get { return _numObjectives; } protected set { _numObjectives = value; } }

        /// <summary>Number of constraints.</summary>
        public virtual int NumConstraints 
        { get { return _numConstraints; } protected set { _numConstraints = value; } }

        /// <summary>Number of equality constraints.</summary>
        public virtual int NumEqualityConstraints 
        { get { return _numEqualityConstraints; } protected set { _numEqualityConstraints = value; } }

        #endregion ResponseCharacteristics


        #region InputFlags

        protected bool _reqObjective = false;
        protected bool _reqConstraints = false;
        protected bool _reqObjectiveGradient = false;
        protected bool _reqConstraintGradients = false;
        protected bool _reqObjectiveHessian = false;

        protected bool _reqConstraintHessians = false;

        /// <summary>Indicates whether calculation of objective function is/was requested.</summary>
        public virtual bool ReqObjective
        { get { return _reqObjective; } set { _reqObjective = value; } }

        /// <summary>Indicates whether calculation of objective function gradient is/was requested.</summary>
        public virtual bool ReqObjectiveGradient
        { get { return _reqObjectiveGradient; } set { _reqObjectiveGradient = value; } }

        /// <summary>Indicates whether calculation of objective function Hessian is/was requested.</summary>
        public virtual bool ReqObjectiveHessian
        { get { return _reqObjectiveHessian; } set { _reqObjectiveHessian = value; } }

        /// <summary>Indicates whether calculation of constraint functions is/was requested.</summary>
        public virtual bool ReqConstraints
        { get { return _reqConstraints; } set { _reqConstraints = value; } }

        /// <summary>Indicates whether calculation of constraint functions gradient is/was requested.</summary>
        public virtual bool ReqConstraintGradients
        { get { return _reqConstraintGradients; } set { _reqConstraintGradients = value; } }

        /// <summary>Indicates whether calculation of constraint functions Hessian is/was requested.</summary>
        public virtual bool ReqConstraintHessians
        { get { return _reqConstraintHessians; } set { _reqConstraintHessians = value; } }

        /// <summary>Copies some data from the corresponding Dto. This method is exposed because
        /// some properties' setters don't have public access and can therefore not be accessed
        /// from the DTO's CopyTo() method.</summary>
        /// <typeparam name="TypeResponseEvaluator">Type of response evaluator.</typeparam>
        /// <param name="Dto">DTO from which data is copied.</param>
        public virtual void CopyFromDto<TypeResponseEvaluator>(ResponseEvaluatorWithStorageAnalysisDtoBase<TypeResponseEvaluator> Dto)
            where TypeResponseEvaluator : ResponseEvaluatorWithStorageAnalysis
        {
            this.NumParameters = Dto.NumParameters;
            this.NumObjectives = Dto.NumObjectives;
            this.NumConstraints = Dto.NumConstraints;
            this.NumEqualityConstraints = Dto.NumEqualityConstraints;
            // Evaluation request flags:
            this.ReqObjective = Dto.ReqObjective;
            this.ReqObjectiveGradient = Dto.ReqObjectiveGradient;
            this.ReqObjectiveHessian = Dto.ReqObjectiveHessian;
            this.ReqConstraints = Dto.ReqConstraints;
            this.ReqConstraintGradients = Dto.ReqConstraintGradients;
            this.ReqConstraintHessians = Dto.ReqConstraintHessians;
        }


        #endregion InputFlags



    }  // class ResponseEvaluatorAnalysis


}