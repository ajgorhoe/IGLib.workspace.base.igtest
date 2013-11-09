// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;


namespace IG.NumExperimental
{


    /// <summary>Base class for response evaluators where response is evaluated by objects of type
    /// <typeparamref name="IAnalysis"/>.</summary>
    /// $A Igor Dec10;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public class ResponseEvaluatorWithStorageVector : ResponseEvaluatorWithStorageBase<VectorFunctionResults, IVectorFunction>,
        ILockable
    {

                
        #region construction 

        /// <summary>Creates a <typeparamref name="ResponseEvaluatorAnalysis"/> object.</summary>
        public ResponseEvaluatorWithStorageVector(): this(null)
        {  }


        /// <summary>Creates a <typeparamref name="ResponseEvaluatorAnalysis"/> object with
        /// the specified response evaluator (analysis object) that will be used for response evaluation.</summary>
        /// <param name="evaluationFunction">Analysis object that is used for calculation of respones.</param>
        public ResponseEvaluatorWithStorageVector(IVectorFunction evaluationFunction) : base(null)
        {  }

        #endregion construction

        #region Overridden

        IVectorFunction _vectorFunction;

        /// <summary>Analysis object that is actually used for evaluation of response.</summary>
        public override IVectorFunction EvaluationObject 
        {
            get
            {
                lock (Lock)
                {
                    return _vectorFunction;
                }
            }
            set
            {
                _vectorFunction = value;
                if (value == null)
                {
                    this.NumParameters = 0;
                    this.NumFunctions = 0;
                    this.ValueDefined = false;
                    this.DerivativeDefined = false;
                    this.SecondDerivativeDefined = false;
                }
                else
                {
                    this.NumParameters = value.NumParameters;
                    this.NumFunctions = value.NumValues;
                    this.ValueDefined = value.ValueDefined;
                    this.DerivativeDefined = value.DerivativeDefined;
                    this.SecondDerivativeDefined = value.SecondDerivativeDefined;
                }
            }
        }


        /// <summary>Copies input data (problem characteristics such as number of parameters, number of 
        /// contraints, etc., and analysis request flags) from the specified analysis results object.</summary>
        /// <param name="results">Analysis results object form which various input data is copied.</param>
        protected override void GetData(VectorFunctionResults results)
        {
            // Copy problem characteristics:
            this.NumParameters = results.NumParameters;
            this.NumFunctions = results.NumFunctions;
            // Copy request flags:
            this.ReqValues = results.ReqValues;
            this.ReqGradients = results.ReqGradients;
            this.ReqHessians = results.ReqHessians;
        }

        /// <summary>Creates and returns a copy of the speccified results object.</summary>
        /// <param name="results">Results object to be copied.</param>
        /// <returns>A copy of results object.</returns>
        protected override VectorFunctionResults CopyResults(VectorFunctionResults results)
        {
            if (results == null)
                return null;
            else
                return results.GetCopy() as VectorFunctionResults;
        }


        /// <summary>Creates and returns request for response evaluation at the specified
        /// parameters, where request options are transcribed from the current response evaluator.</summary>
        /// <param name="parameters">Vector of parameters for which response will be calculated.</param>
        /// <returns>Object containing complete request data for response evaluation.</returns>
        protected override VectorFunctionResults CreateRequestThis(IVector parameters)
        {
            VectorFunctionResults ret = new VectorFunctionResults();
            // Copy problem characteristics:
            ret.NumParameters = this.NumParameters;
            ret.NumFunctions = this.NumFunctions;
            // Copy request flags:
            ret.ReqValues = this.ReqValues;
            ret.ReqGradients = this.ReqGradients;
            ret.ReqHessians = this.ReqHessians;
            // Copy parameters:
            IVector param = parameters.GetCopy();
            ret.Parameters = param;
            return ret;
        }


        /// <summary>Evaluates the response.</summary>
        /// <param name="results">Object that must contain complete response evaluation request data,
        /// and where results of response evaluation will be stored.</param>
        protected override void EvaluateResponseThis(VectorFunctionResults requestAndResponse)
        {
            if (EvaluationObject == null)
                throw new InvalidOperationException("Can not evaluate response, evaluation funciton is not specified.");
            else
                EvaluationObject.Evaluate(requestAndResponse);
        }


        #endregion Overridden


        #region ResponseCharacteristics

        protected int _numParameters = -1;  // -1 for undefined

        protected int _numValues = -1;  // -1 for undefined

        protected bool _valuesDefined = true;

        protected bool _derivativeDefined = false;

        protected bool _secondDerivativeDefined = true;

        protected bool _componentEvaluation = false;

        /// <summary>Gets number of parameters of the current vector function
        /// (-1 for not defined, in case that function works with different 
        /// numbers of parameters).</summary>
        public virtual int NumParameters
        {
            get { return _numParameters; }
            protected set { _numParameters = value; }
        }

        /// <summary>Gets number of values of the current vector function
        /// (-1 for not defined, e.g. in case that function works with different 
        /// numbers of parameters and number of functions depends on number of
        /// parameters).</summary>
        public virtual int NumFunctions
        {
            get { return _numValues; }
            protected set { _numValues = value; }
        }

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public virtual bool ValueDefined
        {
            get { return _valuesDefined; }
            protected set { _valuesDefined = value; }
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public virtual bool DerivativeDefined
        {
            get { return _derivativeDefined; }
            protected set { _derivativeDefined = value; }
        }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public virtual bool SecondDerivativeDefined
        {
            get { return _secondDerivativeDefined; }
            protected set { _secondDerivativeDefined = value; }
        }

        /// <summary>Copies some data from the corresponding Dto. This method is exposed because
        /// some properties' setters don't have public access and can therefore not be accessed
        /// from the DTO's CopyTo() method.</summary>
        /// <typeparam name="TypeResponseEvaluator">Type of response evaluator.</typeparam>
        /// <param name="Dto">DTO from which data is copied.</param>
        public virtual void CopyFromDto<TypeResponseEvaluator>(ResponseEvaluatorWithStorageVectorDtoBase<TypeResponseEvaluator> Dto)
            where TypeResponseEvaluator : ResponseEvaluatorWithStorageVector
        {
            this.NumParameters = Dto.NumParameters;
            this.NumFunctions = Dto.NumFunctions;
            this.ValueDefined = Dto.ValueDefined;
            this.DerivativeDefined = Dto.DerivativeDefined;
            this.SecondDerivativeDefined = Dto.SecondDerivativeDefined;
            // Evaluation request flags:
            this.ReqValues = Dto.ReqValues;
            this.ReqGradients = Dto.ReqGradients;
            this.ReqHessians = Dto.ReqHessians;
        }

        #endregion ResponseCharacteristics


        #region InputFlags

        protected bool _reqvalues = true;

        protected bool _reqGradients = false;

        protected bool _reqHessians = false;

        /// <summary>Indicates whether calculation of functions is requested.</summary>
        public virtual bool ReqValues
        { get { return _reqvalues; } set { _reqvalues = value; } }

        /// <summary>Indicates whether calculation of function gradients is requested.</summary>
        public virtual bool ReqGradients
        { get { return _reqGradients; } set { _reqGradients = value; } }

        /// <summary>Indicates whether calculation of functions' Hessians is requested.</summary>
        public virtual bool ReqHessians
        { get { return _reqHessians; } set { _reqHessians = value; } }

        #endregion InputFlags


    }  // class ResponseEvaluatorVector

}