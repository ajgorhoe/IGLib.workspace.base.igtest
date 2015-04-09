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
    /// response is evaluated by an (optimization) direct analysis.</summary>
    /// <typeparam name="TypeResponseEvaluator">Type parameter specifying the specific response evaluator type for which a concrete DTO
    /// is designed.</typeparam>
    /// $A Igor May10;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public abstract class ResponseEvaluatorWithStorageAnalysisDtoBase<TypeResponseEvaluator> : 
        SerializationDtoBase<TypeResponseEvaluator, ResponseEvaluatorWithStorageAnalysis>
        where TypeResponseEvaluator : ResponseEvaluatorWithStorageAnalysis
    {

        #region Construction

        /// <summary>Default constructor, sets IsNull to true.</summary>
        public ResponseEvaluatorWithStorageAnalysisDtoBase()
            : base()
        { }

        #endregion Construction

        #region Data

        public bool AddresultsAutomatically;

        public AnalysisResultsDto[] Results;

        /// <summary>Gets number of parameters of the current vector function
        /// (-1 for not defined, in case that function works with different 
        /// numbers of parameters).</summary>
        public int NumParameters;

        /// <summary>Number of objective functions (normally 1 for this type, but can be 0).</summary>
        public int NumObjectives;

        /// <summary>Number of constraints.</summary>
        public int NumConstraints;

        /// <summary>Number of equality constraints.</summary>
        public int NumEqualityConstraints;


        #region InputFlags


        /// <summary>Indicates whether calculation of objective function is/was requested.</summary>
        public bool ReqObjective;

        /// <summary>Indicates whether calculation of objective function gradient is/was requested.</summary>
        public bool ReqObjectiveGradient;

        /// <summary>Indicates whether calculation of objective function Hessian is/was requested.</summary>
        public bool ReqObjectiveHessian;

        /// <summary>Indicates whether calculation of constraint functions is/was requested.</summary>
        public bool ReqConstraints;

        /// <summary>Indicates whether calculation of constraint functions gradient is/was requested.</summary>
        public bool ReqConstraintGradients;

        /// <summary>Indicates whether calculation of constraint functions Hessian is/was requested.</summary>
        public bool ReqConstraintHessians;


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
        protected override void CopyFromPlain(ResponseEvaluatorWithStorageAnalysis responseEvaluator)
        {
            if (responseEvaluator == null)
                this.SetNull(true);
            else
            {
                this.SetNull(false);
                this.AddresultsAutomatically = responseEvaluator.AddResultsAutomatically;
                this.Results = new AnalysisResultsDto[responseEvaluator.Results.Count];
                for (int i = 0; i < this.Results.Length; ++i)
                {
                    this.Results[i] = new AnalysisResultsDto();
                    this.Results[i].CopyFrom(responseEvaluator.Results[i]);
                }
                // Specific for vector function response:
                this.NumParameters = responseEvaluator.NumParameters;
                this.NumObjectives = responseEvaluator.NumObjectives;
                this.NumConstraints = responseEvaluator.NumConstraints;
                this.NumEqualityConstraints = responseEvaluator.NumEqualityConstraints;
                // Evaluation request flags:
                this.ReqObjective = responseEvaluator.ReqObjective;
                this.ReqObjectiveGradient = responseEvaluator.ReqObjectiveGradient;
                this.ReqObjectiveHessian = responseEvaluator.ReqObjectiveHessian;
                this.ReqConstraints = responseEvaluator.ReqConstraints;
                this.ReqConstraintGradients = responseEvaluator.ReqConstraintGradients;
                this.ReqConstraintHessians = responseEvaluator.ReqConstraintHessians;
            }
        }

        /// <summary>Copies data from the current DTO to a response evaluator object.</summary>
        /// <param name="vec">Response evaluator object that data is copied to.</param>
        protected override void CopyToPlain(ref ResponseEvaluatorWithStorageAnalysis responseEvaluator)
        {
            if (this.GetNull())
                responseEvaluator = null;
            else
            {
                responseEvaluator = this.CreateObject();
                if (Results == null)
                {
                    responseEvaluator.Results.Clear();
                }
                else
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
                            AnalysisResults results = null;
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


    }  // abstract ResponseEvaluatorWithStorageAnalysisDtoBase<TypeResponseEvaluator>



    /// <summary>DTO (data transfer object) for response evaluators with storage where 
    /// response is evaluated by an (optimization) direct analysis function.</summary>
    /// $A Igor May10;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public class ResponseEvaluatorWithStorageAnalysisDto : ResponseEvaluatorWithStorageAnalysisDtoBase<ResponseEvaluatorWithStorageAnalysis>
    {

        #region Construction

        /// <summary>Creates a DTO for storing state of a response evaluator object for direct analyses.</summary>
        public ResponseEvaluatorWithStorageAnalysisDto()
            : base()
        { }

        #endregion Construction


        /// <summary>Creates and returns a new response evaluator of the specified type.</summary>
        public override ResponseEvaluatorWithStorageAnalysis CreateResponseEvaluator()
        {
            return new ResponseEvaluatorWithStorageAnalysis();
        }

    } // class ResponseEvaluatorWithStorageAnalysisDto



}