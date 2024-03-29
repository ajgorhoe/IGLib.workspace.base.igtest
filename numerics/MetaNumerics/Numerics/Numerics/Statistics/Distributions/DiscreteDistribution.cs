﻿using System;

using Meta.Numerics.Functions;

namespace Meta.Numerics.Statistics.Distributions {

    /// <summary>
    /// Represents all discrete, univariate probability distrubtions.
    /// </summary>
    /// <remarks>
    /// <para>A discrete distribution is a distribution over the integers.</para>
    /// </remarks>
    public abstract class DiscreteDistribution : UnivariateDistribution {

        // replace ProbabilityDensity with ProbabilityMass

        /// <summary>
        /// Returns the probability of the obtaining the given value.
        /// </summary>
        /// <param name="k">The value.</param>
        /// <returns>The probability of obtaining the value.</returns>
        public abstract double ProbabilityMass (int k);

        /// <summary>
        /// Gets the smallest value in the distribution.
        /// </summary>
        public abstract int Minimum { get; }

        /// <summary>
        /// Gets the largest value in the distribution.
        /// </summary>
        public abstract int Maximum { get; }

        /// <summary>
        /// Computes the probability of obtaining a value less than the given value.
        /// </summary>
        /// <param name="k">The value.</param>
        /// <returns>The total probability of obtaining a value strictly less than <paramref name="k"/>.</returns>
        /// <seealso cref="RightExclusiveProbability"/>
        public virtual double LeftExclusiveProbability (int k) {
            if (k <= Minimum) {
                return (0.0);
            } if (k > Maximum) {
                return (1.0);
            } else {
                double P = 0.0;
                for (int j = Minimum; j < k; j++) {
                    P += ProbabilityMass(j);
                }
                return (P);
            }
        }

        /// <summary>
        /// Computes the probability of obtaining a value less than or equal to the given value.
        /// </summary>
        /// <param name="k">The value.</param>
        /// <returns>The total probability of obtaining a value les than or equal to <paramref name="k"/>.</returns>
        public virtual double LeftInclusiveProbability (int k) {
            if (k < Minimum) {
                return (0.0);
            } else if (k >= Maximum) {
                return (1.0);
            } else {
                return (LeftExclusiveProbability(k) + ProbabilityMass(k));
            }
        }

        /// <summary>
        /// Computes the probability of obtaining a value greater than the given value.
        /// </summary>
        /// <param name="k">The value.</param>
        /// <returns>The total probability of obtaining a value strictly greater than <paramref name="k"/>.</returns>
        /// <seealso cref="LeftExclusiveProbability"/>
        public virtual double RightExclusiveProbability (int k) {
            if (k < Minimum) {
                return (1.0);
            } else if (k >= Maximum) {
                return (0.0);
            } else {
                double Q = 0.0;
                for (int j = k + 1; j <= Maximum; j++) {
                    Q += ProbabilityMass(j);
                }
                return (Q);
            }
        }

        /// <summary>
        /// Computes the value corresponding to the given percentile.
        /// </summary>
        /// <param name="P">The percentile.</param>
        /// <returns>The value.</returns>
        public virtual int InverseLeftProbability (double P) {
            double PP = 0.0;
            for (int k = Minimum; k <= Maximum; k++) {
                PP += ProbabilityMass(k);
                if (PP >= P) return (k);
            }
            return (Maximum);
        }

        /// <summary>
        /// Computes the expectation value of an artibrary function.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <returns>The expectation value of the function.</returns>
        public virtual double ExpectationValue (Func<int, double> f) {

            if (f == null) throw new ArgumentNullException("f");

            // to avoid running over the whole support when it is large, we move out from the mean
            int i0 = (int) Math.Round(Mean);

            double s_left = 0.0;
            for (int i = i0 - 1; i >= Minimum; i--) {
                double s_left_old = s_left;
                double ds = f(i) * ProbabilityMass(i);
                s_left += f(i) * ProbabilityMass(i);
                if (s_left == s_left_old) break;
            }
            double s_right = 0.0;
            for (int i = i0 + 1; i <= Maximum; i++) {
                double s_right_old = s_right;
                double ds = f(i) * ProbabilityMass(i);
                s_right += f(i) * ProbabilityMass(i);
                if (s_right == s_right_old) break;
            }
            return (s_left + s_right + f(i0) * ProbabilityMass(i0));

        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public override double Mean {
            get {
                return (ExpectationValue((int k) => k));
            }
        }

        /// <summary>
        /// Gets a raw moment of the distribution.
        /// </summary>
        /// <param name="r">The order of the moment.</param>
        /// <returns>The raw moment M<sub>r</sub>.</returns>
        public override double Moment (int r) {
            if (r < 0) {
                throw new ArgumentOutOfRangeException("r");
            } else if (r == 0) {
                return (1.0);
            } else {
                return (ExpectationValue(delegate(int k) { return (MoreMath.Pow(k, r)); }));
            } 
        }

        /// <summary>
        /// Gets a central moment of the distribution.
        /// </summary>
        /// <param name="r">The order of the moment.</param>
        /// <returns>The central moment C<sub>r</sub>.</returns>
        public override double MomentAboutMean (int r) {
            if (r < 0) {
                throw new ArgumentOutOfRangeException("r");
            } else if (r == 0) {
                return (1.0);
            } else if (r == 1) {
                return (0.0);
            } else {
                double mu = Mean;
                return (ExpectationValue((int k) => MoreMath.Pow(k - mu, r)));
            }
        }

        /// <summary>
        /// Produces a random integer drawn from the distribution.
        /// </summary>
        /// <param name="rng">A random number generator.</param>
        /// <returns>A random integer drawn from the distribution.</returns>
        public virtual int GetRandomValue (Random rng) {
            if (rng == null) throw new ArgumentNullException("rng");
            return (InverseLeftProbability(rng.NextDouble()));
        }

    }



    /// <summary>
    /// Represents an interval on the integers.
    /// </summary>
    public struct DiscreteInterval : IEquatable<DiscreteInterval> {

        public DiscreteInterval (int a, int b) {
            if (b < a) Global.Swap(ref a, ref b);
            this.a = a;
            this.b = b;
        }

        int a, b;

        /// <summary>
        /// Gets the lower (left) boundary.
        /// </summary>
        public int LeftEndpoint {
            get {
                return (a);
            }
        }

        /// <summary>
        /// Gets the upper (right) boundary.
        /// </summary>
        public int RightEndpoint {
            get {
                return (b);
            }
        }

        /// <summary>
        /// Gets the width of the interval.
        /// </summary>
        /// <remarks>
        /// <para>Note that the width of the interval is one less than the number of integers in it. For example:
        /// the interval [1,3] has width two, but contains three integers.</para>
        /// </remarks>
        public int Width {
            get {
                return (b - a);
            }
        }


        // convert int to double, replacing minimum and maxiumum integer values by negative and positivel infinity

        private static double IntToDouble (int i) {
            if (i == Int32.MinValue) {
                return(Double.NegativeInfinity);
            } else if (i == Int32.MaxValue) {
                return(Double.PositiveInfinity);
            } else {
                return((double) i);
            }
        }

        /// <summary>
        /// Converts a discrete interval to a continuous interval.
        /// </summary>
        /// <param name="i">The discrete interval.</param>
        /// <returns>The corresponding continuous interval.</returns>
        public static implicit operator Interval (DiscreteInterval i) {
            return (Interval.FromEndpoints(IntToDouble(i.a), IntToDouble(i.b)));
        }

        // equality stuff

        /// <summary>
        /// Determines if the given discrete intervals equals the current discrete interval.
        /// </summary>
        /// <param name="other">The interval to compare.</param>
        /// <returns>True if the discrete intervals are equal, otherwise false.</returns>
        public bool Equals (DiscreteInterval other) {
            return ((this.a == other.a) && (this.b == other.b));
        }

        /// <summary>
        /// Determines if the given object equals the current discrete interval.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the object describes the same discrete interval, otherwise false.</returns>
        public override bool Equals (object obj) {
            if (obj is DiscreteInterval) {
                return (this.Equals((DiscreteInterval)obj));
            } else {
                return (false);
            }
        }

        /// <summary>
        /// Determines if the given discrete intervals are equal.
        /// </summary>
        /// <param name="interval1">The first discrete interval.</param>
        /// <param name="interval2">The second discrete interval.</param>
        /// <returns>True if the discrete intervals are equal, otherwise false.</returns>
        public static bool operator == (DiscreteInterval interval1, DiscreteInterval interval2) {
            return (interval1.Equals(interval2));
        }

        /// <summary>
        /// Determines if the given discrete intervals are unequal.
        /// </summary>
        /// <param name="interval1">The first discrete interval.</param>
        /// <param name="interval2">The second discrete interval.</param>
        /// <returns>True if the discrete intervals are unequal, otherwise false.</returns>
        public static bool operator != (DiscreteInterval interval1, DiscreteInterval interval2) {
            return (!(interval1 == interval2));
        }

        /// <summary>
        /// Returns a hash value for the current discrete interval.
        /// </summary>
        /// <returns>A hash value for the discrete interval.</returns>
        public override int GetHashCode () {
            return (a ^ (b << 15));
        }

    }


}