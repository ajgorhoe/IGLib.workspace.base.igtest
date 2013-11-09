
    // TEST FUNCTIONS FOR 1D LINE SEARCH ALGORITHMS


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

using F = IG.Num.M;

namespace IG.Num
{

    public class LineSearchTestFunc
    {

        /// <summary>A RealFunction class representing quadratic function for testing line search algorithms.</summary>
        public class Quadratic : RealFunction
        {

            
            public Quadratic(): this(1.0, 0.0, 1.0, 0.0)
            {  }

            public Quadratic(double Kx, double Sx) : this(Kx, Sx, 1.0, 0.0)
            { }

            public Quadratic(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "TestFuncLinesearchQuadratic";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            { return x*x; }

            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException(
                    "Can not set a flag for value defined for function " + Name + "."); }
            }

            protected override double RefDerivative(double x)
            { return 2*x; }

            public override bool DerivativeDefined
            {
                get { return true; }
                protected set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for derivative defined for function " + Name + ".");
                }
            }

            protected override double RefSecondDerivative(double x)
            { return 2; }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }

            public override double Derivative(double x, int order)
            {
                if (order > 2)
                    return 0;
                else if (order == 2)
                    return SecondDerivative(x);
                else if (order == 1)
                    return Derivative(x);
                else if (order == 0)
                    return Value(x);
                else throw new ArgumentException("Derivative of order " + order + " not defined. Function name: " + Name);
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                return true;
            }

            //protected override void SetHighestDerivativeDefined(int order)
            //{ throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
            //    + Name + "."); }

            protected override double RefIntegral(double x)
            { return Math.Exp(x) - 1.0; }

            public override bool IntegralDefined
            {
                get { return true; }
                protected set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            protected override double RefInverse(double x)
            {
                if (x < 0)
                    throw new ArgumentException("Inverse function is not defined for negative values for function x^2.");
                else
                    return Math.Sqrt(x);
            }

            public override bool InverseDefined
            {
                get { return true; }
                protected set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }


        }









    }  // class LineSearchTestFunc
    





}



