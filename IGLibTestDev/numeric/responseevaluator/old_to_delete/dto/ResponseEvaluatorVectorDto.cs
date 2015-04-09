// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;

namespace IG.NumExperimental
{



    /// <summary>Base class for DTOs (Data Transfer Objects) for response evaluators with storage where 
    /// response is evaluated by a vector function.</summary>
    /// <typeparam name="TypeResponseEvaluator">Type parameter specifying the specific response evaluator type for which a concrete DTO
    /// is designed.</typeparam>
    /// $A Igor May10;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public abstract class ResponseEvaluatorWithStorageVectorDtoBase<TypeResponseEvaluator> : SerializationDtoBase<TypeResponseEvaluator, ResponseEvaluatorWithStorageVector>
        where TypeResponseEvaluator : ResponseEvaluatorWithStorageVector
    {

        #region Construction

        /// <summary>Default constructor, sets IsNull to true.</summary>
        public ResponseEvaluatorWithStorageVectorDtoBase()
            : base()
        { }

        #endregion Construction

        #region Data

        public bool AddresultsAutomatically;

        public VectorFunctionResultsDto[] Results;

        /// <summary>Gets number of parameters of the current vector function
        /// (-1 for not defined, in case that function works with different 
        /// numbers of parameters).</summary>
        public int NumParameters;

        /// <summary>Gets number of values of the current vector function
        /// (-1 for not defined, e.g. in case that function works with different 
        /// numbers of parameters and number of functions depends on number of
        /// parameters).</summary>
        public int NumFunctions;

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public bool ValueDefined;

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public bool DerivativeDefined;

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public bool SecondDerivativeDefined;


        #region InputFlags

        /// <summary>Indicates whether calculation of functions is requested.</summary>
        public bool ReqValues;

        /// <summary>Indicates whether calculation of function gradients is requested.</summary>
        public bool ReqGradients;

        /// <summary>Indicates whether calculation of functions' Hessians is requested.</summary>
        public bool ReqHessians;


        #endregion InputFlags

        #endregion Data


        #region Operation

        /// <summary>Creates and returns a new response evaluator of the specified type.</summary>
        /// <param name="length">Vector dimension.</param>
        public abstract TypeResponseEvaluator CreateResponseEvaluator();

        /// <summary>Creates and returns a new response evaluator of the specified type.</summary>
        public override TypeResponseEvaluator CreateObject()
        {
            return CreateResponseEvaluator();
        }

        /// <summary>Copies data to the current DTO from a vector function-based response evaluator object.</summary>
        /// <param name="responseEvaluator">Response evaluator object from which data is copied.</param>
        protected override void CopyFromPlain(ResponseEvaluatorWithStorageVector responseEvaluator)
        {
            if (responseEvaluator == null)
                this.SetNull(true);
            else
            {
                this.SetNull(false);
                this.AddresultsAutomatically = responseEvaluator.AddResultsAutomatically;
                this.Results = new VectorFunctionResultsDto[responseEvaluator.Results.Count];
                for (int i = 0; i < this.Results.Length; ++i)
                {
                    this.Results[i] = new VectorFunctionResultsDto();
                    this.Results[i].CopyFrom(responseEvaluator.Results[i]);
                }
                // Specific for vector function response:
                this.NumParameters = responseEvaluator.NumParameters;
                this.NumFunctions = responseEvaluator.NumFunctions;
                this.ValueDefined = responseEvaluator.ValueDefined;
                this.DerivativeDefined = responseEvaluator.DerivativeDefined;
                this.SecondDerivativeDefined = responseEvaluator.SecondDerivativeDefined;
                // Evaluation request flags:
                this.ReqValues = responseEvaluator.ReqValues;
                this.ReqGradients = responseEvaluator.ReqGradients;
                this.ReqHessians = responseEvaluator.ReqHessians;
            }
        }

        /// <summary>Copies data from the current DTO to a response evaluator object.</summary>
        /// <param name="vec">Response evaluator object that data is copied to.</param>
        protected override void CopyToPlain(ref ResponseEvaluatorWithStorageVector responseEvaluator)
        {
            if (this.GetNull())
                responseEvaluator = null;
            else
            {
                responseEvaluator = this.CreateObject();
                if (Results == null)
                {
                    responseEvaluator.Results.Clear();
                } else
                {
                    responseEvaluator.LastResults = null; // set last results undefined when deserializing! This may be changed in the future.
                    responseEvaluator.AddResultsAutomatically = this.AddresultsAutomatically;
                    responseEvaluator.Results.Clear();
                    for (int i = 0; i < this.Results.Length; ++i)
                    {
                        if (this.Results[i] == null)
                            responseEvaluator.Results.Add(null);
                        else
                        {
                            VectorFunctionResults results = null;
                            this.Results[i].CopyTo(ref results);
                            responseEvaluator.Results.Add(results);
                        }
                    }
                }
                // Specific for vector function response:
                responseEvaluator.CopyFromDto<TypeResponseEvaluator>(this);
            }
        }


        #endregion Operation


    }  // abstract ResponseEvaluatorWithStorageVectorDtoBase<TypeResponseEvaluator>


    /// <summary>DTO (data transfer object) for response evaluators with storage where 
    /// response is evaluated by a vector function.</summary>
    /// $A Igor May10;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public class ResponseEvaluatorWithStorageVectorDto : ResponseEvaluatorWithStorageVectorDtoBase<ResponseEvaluatorWithStorageVector>
    {

        #region Construction

        /// <summary>Creates a DTO for storing state of a response evaluator object for vector functions.</summary>
        public ResponseEvaluatorWithStorageVectorDto()
            : base()
        { }

        #endregion Construction


        /// <summary>Creates and returns a new response evaluator of the specified type.</summary>
        public override ResponseEvaluatorWithStorageVector CreateResponseEvaluator()
        {
            return new ResponseEvaluatorWithStorageVector();
        }

    } // class ResponseEvaluatorWithStorageVectorDto



}