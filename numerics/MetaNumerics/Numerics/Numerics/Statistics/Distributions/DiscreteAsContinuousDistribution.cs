﻿using System;


namespace Meta.Numerics.Statistics.Distributions {

    // When mapping a discrete to a continuous distribution, should P(x) map to the probability left of x including the bin x or excluding it?
    // If we choose the former, then P(x_min) > 0, which doesn't make sense. If we choose the latter, then P(x_max) < 1, which also doesn't make sense.
    // If we make either choice, then for a symmetric distribution P(x_mid) != 0.5, because to get 0.5 we would need to take half of the mid-bin.

    // Here is a way that we can get everything we want and have P(x) increase continuously rather than in jumps Define
    //   z = (x - x_min) / (x_max - x_min)
    // to be the fraction of the way through the continuos interval and define
    //   k = k_min + z (k_max - k_min + 1)
    // to be the corresponding "effective k". Overall this means
    //   k = k_min + m (x - x_min)    m = (k_max - k_min + 1)/(x_max - x_min)
    // Split k = [k] + {k} into into integer [k] and fractional {k} parts. Then let
    //   P(x) = P_L([k]) + {k} P([k])
    // where P_L([k]) is the exclusive left probability for bin [k] and P([k]) is the probability for bin [k].

    // Then
    //    x = a + e => z = 0 + e => k = k_min + e => P = P_L(k_min) + e P(k_min) = 0 + e
    //    x = b - e => z = 1 - e => k = k_max + 1 - e => P = P_L(k_max) + (1 - e) P(k_max) = 1 - e
    //    x = (a+b)/2 => z = 1/2 => k = (k_max + k_min + 1) / 2 => P = P_L((k_max + k_min)/2) + 1/2 P((k_max + k_min)/2) = half of mid-bin
    // which is what we wanted.

    // If k_max and x_max are infinite, just let m = 1.

    /// <summary>
    /// Represents a discrete distribution as a continous distribution.
    /// </summary>
    public class DiscreteAsContinuousDistribution : Distribution {

        /// <summary>
        /// Initializes a new shim that represents a discrete distribution as a continuous distribution.
        /// </summary>
        /// <param name="distribution">The discrete distiribution to represent.</param>
        public DiscreteAsContinuousDistribution (DiscreteDistribution distribution) {
            if (distribution == null) throw new ArgumentNullException("distribution"); 
            this.d = distribution;
            this.xSupport = Interval.FromEndpoints(d.Minimum, d.Maximum);
        }

        /// <summary>
        /// Initializes a new shim that represents a discrete distribution as a continuous distribution.
        /// </summary>
        /// <param name="distribution">The discrete distiribution to represent.</param>
        /// <param name="support">The continuous support interval into which the discrete support interval is to be mapped.</param>
        public DiscreteAsContinuousDistribution (DiscreteDistribution distribution, Interval support) {
            if (distribution == null) throw new ArgumentNullException("distribution");
            this.d = distribution;
            this.xSupport = support;
        }

        private DiscreteDistribution d;
        private Interval xSupport;

        private void ComputeEffectiveBin (double x, out int ki, out double kf) {
            double z = (x - xSupport.LeftEndpoint) / xSupport.Width;
            double k = d.Minimum + z * (d.Maximum - d.Minimum + 1);
            ki = (int) Math.Floor(k);
            kf = k - ki;
        }

        /// <inheritdoc />
        public override double LeftProbability (double x) {
            if (x < xSupport.LeftEndpoint) {
                return (0.0);
            } else if (x > xSupport.RightEndpoint) {
                return (1.0);
            } else {
                int ki; double kf; ComputeEffectiveBin(x, out ki, out kf);
                return (d.LeftExclusiveProbability(ki) + kf * d.ProbabilityMass(ki));
            }
        }

        /// <inheritdoc />
        public override double RightProbability (double x) {
            if (x < xSupport.LeftEndpoint) {
                return (1.0);
            } else if (x > xSupport.RightEndpoint) {
                return (0.0);
            } else {
                int ki; double kf; ComputeEffectiveBin(x, out ki, out kf);
                return ((1.0 - kf) * d.ProbabilityMass(ki) + d.RightExclusiveProbability(ki));
            }
        }

        /// <summary>
        /// Not valid for discrete distributions.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>Throws an exception.</returns>
        /// <remarks>
        /// <para>Technically, the PDF of a discrete probability distribution consists of delta functions at the integers, each
        /// with a weight equal to the PMF at that integer. Since this is not representable in floating point arithmetic, calling
        /// this method is an invalid operation.</para>
        /// </remarks>
        public override double ProbabilityDensity (double x) {
            if ((x < xSupport.LeftEndpoint) || (x > xSupport.RightEndpoint)) {
                return (0.0);
            } else {
                int ki; double kf; ComputeEffectiveBin(x, out ki, out kf);
                return (d.ProbabilityMass(ki) * (d.Maximum - d.Minimum + 1) / xSupport.Width);
            }
        }

        /// <inheritdoc />
        public override double Mean {
            get {
                return (xSupport.LeftEndpoint + (d.Mean - d.Minimum) * xSupport.Width / (d.Maximum - d.Minimum));
            }
        }

        /// <inheritdoc />
        public override double Variance {
            get {
                return (d.Variance * MoreMath.Pow(xSupport.Width / (d.Maximum - d.Minimum), 2));
            }
        }

        /// <inheritdoc />
        public override double Skewness {
            get {
                return (d.Skewness);
            }
        }

        /// <inheritdoc />
        public override double Moment (int n) {
            return (d.Moment(n));
        }

        /// <inheritdoc />
        public override double MomentAboutMean (int n) {
            return (d.MomentAboutMean(n) * MoreMath.Pow(xSupport.Width / (d.Maximum - d.Minimum), n));
        }


        /// <inheritdoc />
        public override double InverseLeftProbability (double P) {
            return (d.InverseLeftProbability(P));
        }

        /// <inheritdoc />
        public override Interval Support {
            get {
                return (xSupport);
            }
        }

    }

}
