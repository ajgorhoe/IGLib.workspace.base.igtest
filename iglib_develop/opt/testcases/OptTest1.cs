using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Unconstrained optimization examples.
    /// <para>Contains definitions of example optimization problems in form of IAnalysis classes.</para></summary>
    public class OptUnconstrained
    {

        /// <summary>The Rosenbrock's unconstrained optimization problem (2D).
        /// <para>min f(x) = f(x,y) = (1-x)^2 + 100 * (y-x^2)^2</para>
        /// <para>Unique local minimum:</para>
        /// <para>  x = 1, y = 1, f(x, y) = 0.</para></summary>
        /// <remarks><para>This function is often used for testing optimization algorithms due to the banana - like
        /// shape of its contours.</para>
        /// <para>On Wikipedia:  http://en.wikipedia.org/wiki/Rosenbrock_function </para></remarks>
        public class ExampleRosenbrock : AnalysisBase, IAnalysis, ILockable
        {

            #region Dimensions

            /// <summary>Number of parameters.</summary>
            public override int NumParameters
            {
                get { return 2; }
                set { throw new ArgumentException("Number of parameters can not be set."); }
            }

            /// <summary>Number of objectives.</summary>
            public override int NumObjectives
            {
                get { return 1; }
                set { throw new ArgumentException("Number of objectives can not be set."); }
            }

            /// <summary>Number of constraints.</summary>
            public override int NumConstraints
            {
                get { return 0; }
                set { throw new ArgumentException("Number of constraints can not be set."); }
            }

            #endregion Dimensions

            protected ScalarFunctionBase _objectiveFunction;

            /// <summary>The scalar function object that can perform evaluation of the Rosenbrock function and eventually
            /// its derivatives.</summary>
            public virtual ScalarFunctionBase ObjectiveFunction
            {
                get
                {
                    if (_objectiveFunction == null)
                        _objectiveFunction = new ScalarFunctionExamples.Rosenbrock();
                    return _objectiveFunction;
                }
                protected set { throw new InvalidOperationException("Object for calculation of the objective function can not be set."); }
            }

            protected IVector _grad = null;

            protected IMatrix _hess = null;

            /// <summary>Performs the direct analysis, i.e. calculation of the response functions of the optimization
            /// problem.</summary>
            /// <param name="analysisData">Object that contains analysis request data and where analysis results are stored.</param>
            public override void Analyse(IAnalysisResults analysisData)
            {
                lock (Lock)
                {
                    if (analysisData == null)
                        throw new ArgumentException("Analysis data is not specified (null argument).");
                    if (analysisData.Parameters == null)
                        throw new ArgumentException("Vector of parameters is not defined on the analysis data.");
                    else if (analysisData.Parameters.Length != NumParameters)
                        throw new ArgumentException("Number of parameters is not " + NumParameters + ".");
                    // Prepare result storage:
                    analysisData.PrepareResultStorage(NumParameters, NumObjectives, NumConstraints, true);
                    double x = analysisData.Parameters[0];
                    double y = analysisData.Parameters[1];
                    // Calculate response functions:
                    if (analysisData.ReqObjective)
                    {
                        analysisData.Objective = ObjectiveFunction.Value(analysisData.Parameters);
                        analysisData.CalculatedObjective = true;
                    }
                    if (analysisData.ReqObjectiveGradient)
                    {
                        _grad = analysisData.ObjectiveGradient;
                        ObjectiveFunction.Gradient(analysisData.Parameters, ref _grad);
                        analysisData.ObjectiveGradient = _grad;
                        analysisData.CalculatedObjectiveGradient = true;
                    }
                    if (analysisData.ReqObjectiveHessian)
                    {
                        _hess = analysisData.ObjectiveHessian;
                        ObjectiveFunction.Hessian(analysisData.Parameters, ref _hess);
                        analysisData.ObjectiveHessian = _hess;
                        analysisData.CalculatedObjectiveHessian = true;
                    }
                    if (analysisData.ReqConstraints)
                    {
                        analysisData.ErrorCode = -1;
                        analysisData.ErrorString += "Unconstrained problem, can not calculate constraints." + Environment.NewLine;
                        analysisData.CalculatedConstraints = false;
                    }
                    if (analysisData.ReqConstraintGradients)
                    {
                        analysisData.ErrorCode = -1;
                        analysisData.ErrorString += "Unconstrained problem, can not calculate constraint gradients." + Environment.NewLine;
                        analysisData.CalculatedConstraintGradients = false;
                    }
                    if (analysisData.ReqConstraintHessians)
                    {
                        analysisData.ErrorCode = -1;
                        analysisData.ErrorString += "Unconstrained problem, can not calculate constraint Hessians." + Environment.NewLine;
                        analysisData.CalculatedConstraintHessians = false;
                    }
                }
            }
        }  // class ExampleRosenbrock 


        /// <summary>The generalized Rosenbrock's unconstrained optimization problem in arbitrary 
        /// dimensions Dim >= 2.
        /// <para>f(x,y) = Sum[i=0...N-2]{(1-x_{i})^2 + 100 * (x_{i+1}-x_{i}^2)^2}</para></summary>
        /// <remarks>This is one of the generalizations of the 2D Rosenbrock function. 
        /// <para></para>
        /// <para>Moved from stand-alone class, now nested in the <see cref="ScalarFunctionExamples"/> class.</para>
        /// <para>See also:</para>
        /// <para>I. Grešovnik: Test functions for Unconstrained Minimization, Igor's internal report. </para>
        /// <para>Definition at AlgLib page: http://www.alglib.net/optimization/lbfgsandcg.php#header4 </para>
        /// <para>Definition at Wikipedia: http://en.wikipedia.org/wiki/Rosenbrock_function#Multidimensional_generalisations </para>
        /// </remarks>
        public class ExampleRosenbrockGeneralizedAdjacent : AnalysisBase, IAnalysis, ILockable
        {


            #region ILockable

            private object _lock = new object();

            public object Lock
            {
                get { return _lock; }
            }

            #endregion ILockable


            #region Dimensions

            

            /// <summary>Number of objectives.</summary>
            public override int NumObjectives
            {
                get { return 1; }
                set { throw new ArgumentException("Number of objectives can not be set."); }
            }

            /// <summary>Number of constraints.</summary>
            public override int NumConstraints
            {
                get { return 0; }
                set { throw new ArgumentException("Number of constraints can not be set."); }
            }

            #endregion Dimensions

            protected ScalarFunctionBase _objectiveFunction;

            /// <summary>The scalar function object that can perform evaluation of the Rosenbrock function and eventually
            /// its derivatives.</summary>
            public virtual ScalarFunctionBase ObjectiveFunction
            {
                get
                {
                    lock (Lock)
                    {
                        if (_objectiveFunction == null)
                        {
                            _objectiveFunction = new ScalarFunctionExamples.RosenbrockGeneralizedAdjacent();
                        }
                        return _objectiveFunction;
                    }
                }
                protected set
                {
                    lock (Lock)
                    {
                        if (value != null)
                            throw new InvalidOperationException("Object for calculation of the objective function can not be changed.");
                        else
                            _objectiveFunction = null;
                    }
                }
            }

            protected IVector _grad = null;

            protected IMatrix _hess = null;

            /// <summary>Performs the direct analysis, i.e. calculation of the response functions of the optimization
            /// problem.</summary>
            /// <param name="analysisData">Object that contains analysis request data and where analysis results are stored.</param>
            public override void Analyse(IAnalysisResults analysisData)
            {
                lock (Lock)
                {
                    if (analysisData == null)
                        throw new ArgumentException("Analysis data is not specified (null argument).");
                    if (analysisData.Parameters == null)
                        throw new ArgumentException("Vector of parameters is not defined on the analysis data.");
                    NumParameters = analysisData.NumParameters;
                    // Prepare result storage:
                    analysisData.PrepareResultStorage(NumParameters, NumObjectives, NumConstraints, true);
                    // Calculate response functions:
                    if (analysisData.ReqObjective)
                    {
                        analysisData.Objective = ObjectiveFunction.Value(analysisData.Parameters);
                        analysisData.CalculatedObjective = true;
                    }
                    if (analysisData.ReqObjectiveGradient)
                    {
                        _grad = analysisData.ObjectiveGradient;
                        ObjectiveFunction.Gradient(analysisData.Parameters, ref _grad);
                        analysisData.ObjectiveGradient = _grad;
                        analysisData.CalculatedObjectiveGradient = true;
                    }
                    if (analysisData.ReqObjectiveHessian)
                    {
                        _hess = analysisData.ObjectiveHessian;
                        ObjectiveFunction.Hessian(analysisData.Parameters, ref _hess);
                        analysisData.ObjectiveHessian = _hess;
                        analysisData.CalculatedObjectiveHessian = true;
                    }
                    if (analysisData.ReqConstraints)
                    {
                        analysisData.ErrorCode = -1;
                        analysisData.ErrorString += "Unconstrained problem, can not calculate constraints." + Environment.NewLine;
                        analysisData.CalculatedConstraints = false;
                    }
                    if (analysisData.ReqConstraintGradients)
                    {
                        analysisData.ErrorCode = -1;
                        analysisData.ErrorString += "Unconstrained problem, can not calculate constraint gradients." + Environment.NewLine;
                        analysisData.CalculatedConstraintGradients = false;
                    }
                    if (analysisData.ReqConstraintHessians)
                    {
                        analysisData.ErrorCode = -1;
                        analysisData.ErrorString += "Unconstrained problem, can not calculate constraint Hessians." + Environment.NewLine;
                        analysisData.CalculatedConstraintHessians = false;
                    }
                }
            }
        }  // class ExampleRosenbrockGeneralizedAdjacent 


        /// <summary>The generalized Rosenbrock's unconstrained optimization problem in arbitrary 
        /// dimensions Dim >= 2.
        /// <para>f(x,y) = Sum[i=0...N-2]{(1-x_{i})^2 + 100 * (x_{i+1}-x_{i}^2)^2}</para></summary>
        /// <remarks>This is one of the generalizations of the 2D Rosenbrock function. 
        /// <para></para>
        /// <para>Moved from stand-alone class, now nested in the <see cref="ScalarFunctionExamples"/> class.</para>
        /// <para>See also:</para>
        /// <para>I. Grešovnik: Test functions for Unconstrained Minimization, Igor's internal report. </para>
        /// <para>Definition at AlgLib page: http://www.alglib.net/optimization/lbfgsandcg.php#header4 </para>
        /// <para>Definition at Wikipedia: http://en.wikipedia.org/wiki/Rosenbrock_function#Multidimensional_generalisations </para>
        /// </remarks>
        public class ExampleRosenbrockGeneralizedExhaustive : ExampleRosenbrockGeneralizedAdjacent,
            IAnalysis, ILockable
        {

            /// <summary>The scalar function object that can perform evaluation of the Rosenbrock function and eventually
            /// its derivatives.</summary>
            public override ScalarFunctionBase ObjectiveFunction
            {
                get
                {
                    lock (Lock)
                    {
                        if (_objectiveFunction == null)
                        {
                            _objectiveFunction = new ScalarFunctionExamples.RosenbrockGeneralizedExhaustive();
                        }
                        return _objectiveFunction;
                    }
                }
                protected set
                {
                    lock (Lock)
                    {
                        if (value != null)
                            throw new InvalidOperationException("Object for calculation of the objective function can not be changed.");
                        else
                            _objectiveFunction = null;
                    }
                }
            }

        }  // class ExampleRosenbrockGeneralizedExhaustive 

    }  // class OptUnconstrained


    
    /// <summary>Unconstrained optimization examples.
    /// <para>Contains definitions of example optimization problems in form of IAnalysis.</para></summary>
    public class OptConstrained
    {

        /// <summary>A simple test optimization problem (nonlinear constrained, 2 parameters, 2 constraints).
        /// <para>Problem solved is: min f(x,y)=x^2+y^4, subject to y>=(x-3)^6 and y>=17-x^2 . </para>
        /// <para>  f(x,y)=x^2+y^4</para>
        /// <para>  c_1(x,y)=(x-3)^6-y</para>
        /// <para>  c_2(x,y)=17-x^2-y</para>
        /// <para>Known local solution:</para>
        /// <para>   x=4, y=1, f(x,y) = 17.</para></summary>
        public class Example2dTest1 : AnalysisBase, IAnalysis
        {

            #region Dimensions

            /// <summary>Number of parameters.</summary>
            public override int NumParameters
            {
                get { return 2; }
                set { throw new ArgumentException("Number of parameters can not be set."); }
            }

            /// <summary>Number of objectives.</summary>
            public override int NumObjectives
            {
                get { return 1; }
                set { throw new ArgumentException("Number of objectives can not be set."); }
            }

            /// <summary>Number of constraints.</summary>
            public override int NumConstraints
            {
                get { return 2; }
                set { throw new ArgumentException("Number of constraints can not be set."); }
            }

            #endregion Dimensions


            /// <summary>Performs the direct analysis, i.e. calculation of the response functions of the optimization
            /// problem.</summary>
            /// <param name="analysisData">Object that contains analysis request data and where analysis results are stored.</param>
            public override void Analyse(IAnalysisResults analysisData)
            {
                if (analysisData == null)
                    throw new ArgumentException("Analysis data is not specified (null argument).");
                if (analysisData.Parameters == null)
                    throw new ArgumentException("Vector of parameters is not defined on the analysis data.");
                else if (analysisData.Parameters.Length != NumParameters)
                    throw new ArgumentException("Number of parameters is not " + NumParameters + ".");
                double
                    x = analysisData.Parameters[0],
                    y = analysisData.Parameters[1];
                // Prepare result storage:
                analysisData.PrepareResultStorage(NumParameters, NumObjectives, NumConstraints, true);
                // Calculate response functions:
                if (analysisData.ReqObjective)
                {
                    analysisData.Objective = x * x + Math.Pow(y, 4);
                    analysisData.CalculatedObjective = true;
                }
                if (analysisData.ReqConstraints)
                {
                    analysisData.Constraints[0] = Math.Pow(x - 3, 6) - y;
                    analysisData.Constraints[1] = 17 - x * x - y;
                    analysisData.CalculatedConstraints = true;
                }
                if (analysisData.ReqObjectiveGradient)
                {

                    analysisData.ObjectiveGradient[0] = 2 * x;
                    analysisData.ObjectiveGradient[1] = 4 * Math.Pow(y, 3);
                    analysisData.CalculatedObjectiveGradient = true;
                }
                if (analysisData.ReqConstraintGradients)
                {
                    analysisData.ConstraintGradients[0][0] = 6 * Math.Pow(x - 3, 5);
                    analysisData.ConstraintGradients[0][1] = -1;
                    analysisData.ConstraintGradients[1][0] = -2 * x;
                    analysisData.ConstraintGradients[1][1] = -1;
                    analysisData.CalculatedConstraintGradients = true;
                }
                if (analysisData.ReqObjectiveHessian)
                {
                    // Calculation of objective Hessian is not implemented:
                    analysisData.ErrorCode = -1;
                    analysisData.ErrorString += "Calculation of objective function Hessian is not implemented." + Environment.NewLine;
                    analysisData.CalculatedObjectiveHessian = false;
                }
                if (analysisData.ReqConstraintHessians)
                {
                    // Calculation of objective Hessian is not implemented:
                    analysisData.ErrorCode = -1;
                    analysisData.ErrorString += "Calculation of constraint function Hessians is not implemented." + Environment.NewLine;
                    analysisData.CalculatedConstraintHessians = false;
                }

            }

        }  // class OptExampleConstrained2dTest1



        /// <summary>A simple test optimization problem (nonlinear constrained, 2 parameters, 2 constraints).
        /// <para>Problem solved is: min f(x,y)=(sin(sqrt(A*(x-CX)^2+B*(y-CY)^2)))^2, subject to x>=TX+y^2 and y>=TY,
        /// where A=0.1, B=0.005, CX=0.2, CY=-1, TX=0.6, and TY=1 :</para>
        /// <para>  f(x,y)=(sin(sqrt(A*(x-CX)^2+B*(y-CY)^2)))^2 </para>
        /// <para>  c_1(x,y)=TX+y*y-x </para>
        /// <para>  c_2(x,y)=TY-y </para>
        /// <para>Known local solution:</para>
        /// <para>   x=1.6, y=1, f(x,y) = 0.200889 .</para></summary>
        public class Example2dTest : AnalysisBase, IAnalysis
        {

            #region Dimensions

            /// <summary>Number of parameters.</summary>
            public override int NumParameters
            {
                get { return 2; }
                set { throw new ArgumentException("Number of parameters can not be set."); }
            }

            /// <summary>Number of objectives.</summary>
            public override int NumObjectives
            {
                get { return 1; }
                set { throw new ArgumentException("Number of objectives can not be set."); }
            }

            /// <summary>Number of constraints.</summary>
            public override int NumConstraints
            {
                get { return 2; }
                set { throw new ArgumentException("Number of constraints can not be set."); }
            }

            #endregion Dimensions


            #region Data

            double 
                A=0.1, 
                B=0.005, 
                CX=0.2, 
                CY=-1, 
                TX=0.6, 
                TY=1;


            #endregion Data

            /// <summary>Performs the direct analysis, i.e. calculation of the response functions of the optimization
            /// problem.</summary>
            /// <param name="analysisData">Object that contains analysis request data and where analysis results are stored.</param>
            public override void Analyse(IAnalysisResults analysisData)
            {
                if (analysisData == null)
                    throw new ArgumentException("Analysis data is not specified (null argument).");
                if (analysisData.Parameters == null)
                    throw new ArgumentException("Vector of parameters is not defined on the analysis data.");
                else if (analysisData.Parameters.Length != NumParameters)
                    throw new ArgumentException("Number of parameters is not " + NumParameters + ".");
                double
                    x = analysisData.Parameters[0],
                    y = analysisData.Parameters[1];
                double expr = A * (x - CX) * (x - CX) + B * (y - CY) * (y - CY);
                double rootexpr = Math.Sqrt(expr);
                double sinroot = Math.Sin(rootexpr);
                double cosroot = Math.Cos(rootexpr);
                // Prepare result storage:
                analysisData.PrepareResultStorage(NumParameters, NumObjectives, NumConstraints, true);
                // Calculate response functions:
                if (analysisData.ReqObjective)
                {
                    analysisData.Objective = sinroot*sinroot;
                    analysisData.CalculatedObjective = true;
                }
                if (analysisData.ReqConstraints)
                {
                    analysisData.Constraints[0] = TX - x + y * y;
                    analysisData.Constraints[1] = TY - y;
                    analysisData.CalculatedConstraints = true;
                }
                if (analysisData.ReqObjectiveGradient)
                {

                    analysisData.ObjectiveGradient[0] = 2 * A * (x - CX) * cosroot * sinroot / rootexpr;
                    analysisData.ObjectiveGradient[1] = 2 * B * (y - CY) * cosroot * sinroot / rootexpr;
                    analysisData.CalculatedObjectiveGradient = true;
                }
                if (analysisData.ReqConstraintGradients)
                {
                    analysisData.ConstraintGradients[0][0] = -1;
                    analysisData.ConstraintGradients[0][1] = 2 * y;
                    analysisData.ConstraintGradients[1][0] = 0;
                    analysisData.ConstraintGradients[1][1] = -1;
                    analysisData.CalculatedConstraintGradients = true;
                }
                if (analysisData.ReqObjectiveHessian)
                {
                    // Calculation of objective Hessian is not implemented:
                    analysisData.ErrorCode = -1;
                    analysisData.ErrorString += "Calculation of objective function Hessian is not implemented." + Environment.NewLine;
                    analysisData.CalculatedObjectiveHessian = false;
                }
                if (analysisData.ReqConstraintHessians)
                {
                    // Calculation of objective Hessian is not implemented:
                    analysisData.ErrorCode = -1;
                    analysisData.ErrorString += "Calculation of constraint function Hessians is not implemented." + Environment.NewLine;
                    analysisData.CalculatedConstraintHessians = false;
                }

            }

        }  // class OptExampleConstrained2dTest1




        /// <summary>A simple test optimization problem (quadratic constrained, 2 parameters, 2 linear constraints).
        /// <para>Problem solved is: min f(x,y)=(x/2)^2+(y/1)^2, subject to  x+y>=2  and  y>=0.5 :</para>
        /// <para>  f(x,y)= min f(x,y)=(x/2)^2+(y)^2 </para>
        /// <para>  c_1(x,y)= 2-x-y </para>
        /// <para>  c_2(x,y)= 0.5-y </para>
        /// <para>Known local solution:</para>
        /// <para>   x=1.5, y=0.5, f(x,y) = 1.5625 .</para></summary>
        public class Example2dTestSimple : AnalysisBase, IAnalysis
        {

            #region Dimensions

            /// <summary>Number of parameters.</summary>
            public override int NumParameters
            {
                get { return 2; }
                set { throw new ArgumentException("Number of parameters can not be set."); }
            }

            /// <summary>Number of objectives.</summary>
            public override int NumObjectives
            {
                get { return 1; }
                set { throw new ArgumentException("Number of objectives can not be set."); }
            }

            /// <summary>Number of constraints.</summary>
            public override int NumConstraints
            {
                get { return 2; }
                set { throw new ArgumentException("Number of constraints can not be set."); }
            }

            #endregion Dimensions

            /// <summary>Performs the direct analysis, i.e. calculation of the response functions of the optimization
            /// problem.</summary>
            /// <param name="analysisData">Object that contains analysis request data and where analysis results are stored.</param>
            public override void Analyse(IAnalysisResults analysisData)
            {
                if (analysisData == null)
                    throw new ArgumentException("Analysis data is not specified (null argument).");
                if (analysisData.Parameters == null)
                    throw new ArgumentException("Vector of parameters is not defined on the analysis data.");
                else if (analysisData.Parameters.Length != NumParameters)
                    throw new ArgumentException("Number of parameters is not " + NumParameters + ".");
                double
                    x = analysisData.Parameters[0],
                    y = analysisData.Parameters[1];
                // Prepare result storage:
                analysisData.PrepareResultStorage(NumParameters, NumObjectives, NumConstraints, true);
                // Calculate response functions:
                if (analysisData.ReqObjective)
                {
                    analysisData.Objective = 0.25 * x * x + y * y;
                    analysisData.CalculatedObjective = true;
                }
                if (analysisData.ReqConstraints)
                {
                    analysisData.Constraints[0] = 2 - x - y;
                    analysisData.Constraints[1] = 0.5 - y;
                    analysisData.CalculatedConstraints = true;
                }
                if (analysisData.ReqObjectiveGradient)
                {

                    analysisData.ObjectiveGradient[0] = 0.5 * x;
                    analysisData.ObjectiveGradient[1] = 2 * y;
                    analysisData.CalculatedObjectiveGradient = true;
                }
                if (analysisData.ReqConstraintGradients)
                {
                    analysisData.ConstraintGradients[0][0] = -1;
                    analysisData.ConstraintGradients[0][1] = -1;
                    analysisData.ConstraintGradients[1][0] = 0;
                    analysisData.ConstraintGradients[1][1] = -1;
                    analysisData.CalculatedConstraintGradients = true;
                }
                if (analysisData.ReqObjectiveHessian)
                {
                    // Calculation of objective Hessian is not implemented:
                    analysisData.ErrorCode = -1;
                    analysisData.ErrorString += "Calculation of objective function Hessian is not implemented." + Environment.NewLine;
                    analysisData.CalculatedObjectiveHessian = false;
                }
                if (analysisData.ReqConstraintHessians)
                {
                    // Calculation of objective Hessian is not implemented:
                    analysisData.ErrorCode = -1;
                    analysisData.ErrorString += "Calculation of constraint function Hessians is not implemented." + Environment.NewLine;
                    analysisData.CalculatedConstraintHessians = false;
                }

            }

        }  // class OptExampleConstrained2dTest1



    }  // OptConstrained


}


