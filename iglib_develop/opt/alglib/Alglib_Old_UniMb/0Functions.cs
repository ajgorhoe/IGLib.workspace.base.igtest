using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NumLib
{


    /// <summary>Base class for implementation of various functions with gradients.</summary>
    public abstract class FunctionWithGradient
    {

        public abstract string Description
        {
            get;
        }

        /// <summary>Returns function value at the specified parameters.</summary>
        /// <param name="parameters">Parameters where function is evaluated.</param>
        public abstract double Value(double[] parameters);

        /// <summary>Returns function value at the specified parameters.</summary>
        /// <param name="parameters">Parameters where function is evaluated.</param>
        public abstract double[] Gradient(double[] parameters);

        /// <summary>Calculates and returns Euclidean norm of the specified vector.</summary>
        /// <param name="x">Array of elements of the vector for which norm is calculated.</param>
        /// <returns></returns>
        public static double Norm(double[] x)
        {
            int dim = 0;
            double ret = 0;
            if (x != null)
                dim = x.Length;
            if (dim > 0)
            {
                for (int i = 0; i < dim; ++i)
                {
                    ret += x[i] * x[i];
                }
            }
            ret = Math.Sqrt(ret);
            return ret;
        }

        public static string ArrayToString(double[] x)
        {
            int dim = 0;
            if (x != null)
                dim = x.Length;
            if (x == null)
                return null;
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                for (int i = 0; i < dim; ++i)
                {
                    sb.Append(x[i]);
                    if (i < dim - 1)
                        sb.Append(", ");
                }
                sb.Append("}");
                return sb.ToString();
            }
        }

    }  // class FunctionWithGradient


    /// <summary>Shifted and stretched square function in arbitrary dimensions.</summary>
    public class QuadraticFunctionShifted : FunctionWithGradient
    {

        /// <summary>Constructor.</summary>
        /// <param name="dimension">Dimension of space where function is defined.</param>
        public QuadraticFunctionShifted(int dimension): this(new double[dimension])
        {  }

        /// <summary>Constructor.</summary>
        /// <param name="shifts">Vector by which stationary point of the square function is translated.</param>
        public QuadraticFunctionShifted(double[] shifts): this(shifts, null /* factors */)
        {  }

        /// <summary>Constructor.</summary>
        /// <param name="shifts">Vector by which stationary point of the square function is translated.</param>
        /// <param name="factors">Factors by which individual quadratic terms are multiplied.</param>
        public QuadraticFunctionShifted(double[] shifts, double[] factors)
        {
            if (shifts != null)
                Dimension = shifts.Length;
            else if (factors != null)
                Dimension = factors.Length;
            else
                Dimension = 2;
            this.Shifts = shifts;
            this.Factors = factors;
            if (Shifts == null)
            {
                Shifts = new double[Dimension];
                for (int i = 0; i < Dimension; ++i)
                    Shifts[i] = 0;
            }
            if (Factors == null)
            {
                Factors = new double[Dimension];
                for (int i = 0; i < Dimension; ++i)
                    Factors[i] = 1;
            }
            if (Shifts.Length != Dimension)
                throw new ArgumentException("Wrong dimension of translation vector: " + Shifts.Length + " instead of " + Dimension + ".");
            if (Factors.Length != Dimension)
                throw new ArgumentException("Wrong dimension of vector of factors: " + Factors.Length + " instead of " + Dimension + ".");
        }

        double[] Factors;

        double[] Shifts;

        int Dimension = 0;

        private string _description;

        public override string Description
        {

            get
            {
                if (_description == null)
                {
                    _description = "Quadratic function, dim = " + Dimension + ", stat. point: {" + 
                        FunctionWithGradient.ArrayToString(Shifts);
                }
                return _description;
            }
        }

        public override double Value(double[] parameters)
        {
            if (parameters == null)
                throw new ArgumentException("Array of parameters is not specified (null argument).");
            if (parameters.Length != Dimension)
                throw new ArgumentException("Wrong number of parameters, " + parameters.Length + " instead of " + Dimension + ".");
            double val = 0;
            for (int i = 0; i < Dimension; ++i)
            {
                val += Factors[i] * (parameters[i] - Shifts[i]) * (parameters[i] - Shifts[i]);
            }
            return val;
        }

        public override double[] Gradient(double[] parameters)
        {
            if (parameters == null)
                throw new ArgumentException("Array of parameters is not specified (null argument).");
            if (parameters.Length != Dimension)
                throw new ArgumentException("Wrong number of parameters, " + parameters.Length + " instead of " + Dimension + ".");
            double[] grad = new double[Dimension];
            for (int i = 0; i < Dimension; ++i)
            {
                grad[i] = 2 * Factors[i] * (parameters[i]-Shifts[i]);
            }
            return grad;
        }


    }


    /// <summary>The Rosenbrock Function.</summary>
    public class RosenbrockFunction : FunctionWithGradient
    {

        public override string Description
        {
            get { return "Rosenbrock function, f(x,y) = Sqr(1 - x) + 100.0 * Sqr(y - Sqr(x))"; }
        }

        public override double Value(double[] parameters)
        {
            if (parameters == null)
                throw new ArgumentException("Array of parameters is not specified (null argument).");
            if (parameters.Length != 2)
                throw new ArgumentException("Number of parameters of the Rosenbrock function must be 2. Provided: " + parameters.Length + ".");
            double x = parameters[0];
            double y = parameters[1];
            return (1 - x) * (1 - x) + 100.0 * (y - (x*x)) * (y - (x*x));
        }

        public override double[] Gradient(double[] parameters)
        {
            if (parameters == null)
                throw new ArgumentException("Array of parameters is not specified (null argument).");
            if (parameters.Length != 2)
                throw new ArgumentException("Number of parameters of the Rosenbrock function must be 2. Provided: " + parameters.Length + ".");
            double x = parameters[0];
            double y = parameters[1];
            return new double[] {
                2.0 * (x - 1) + 400.0 * x * ((x*x) - y),
                200.0 * (y - (x*x))
            };
        }
    }



}
