﻿using System;
using System.Diagnostics;

using Meta.Numerics;

namespace Meta.Numerics.Functions {

    // mostly direct and straightforward adaptations of the Bessel routines

    public static partial class AdvancedMath {

        /// <summary>
        /// Computes modified cylindrical Bessel functions.
        /// </summary>
        /// <param name="nu">The order, which must be non-negative.</param>
        /// <param name="x">The argument, which must be non-negative.</param>
        /// <returns>The values of I, I', K, and K' for the given order and argument.</returns>
        /// <remarks>
        /// <para>The modified bessel functions fufill a differential equation similiar to the bessel differential equation.</para>
        /// <img src="../images/ModifiedBesselODE.png" />
        /// </remarks>
        public static SolutionPair ModifiedBessel (double nu, double x) {
            if (nu < 0.0) throw new ArgumentOutOfRangeException("nu");
            if (x < 0.0) throw new ArgumentOutOfRangeException("x");

            if (x < 2.0) {
                
                // use series to determine I and I'
                double I, IP;
                ModifiedBesselI_Series(nu, x, out I, out IP);

                // use series to determine K and K' at -1/2 <= mu <= 1/2 that is an integer offset from nu
                int n = (int)Math.Floor(nu + 0.5);
                double mu = nu - n;

                double K, K1;
                ModifiedBesselK_Series(mu, x, out K, out K1);
                double KP = (mu / x) * K - K1;

                // recurr K, K' upward to order nu
                ModifiedBesselK_RecurrUpward(mu, x, ref K, ref KP, n);

                // return the result
                return (new SolutionPair(I, IP, K, KP));

            } else if (x > 32.0 + nu * nu / 2.0) {

                double sI, sIP, sK, sKP;
                ModifiedBessel_Asymptotic(nu, x, out sI, out sIP, out sK, out sKP);
                double e = Math.Exp(x);
                return (new SolutionPair(e * sI, e * sIP, sK / e, sKP / e));

            } else {

                // find 0 <= mu < 1 with same fractional part as nu
                // this is necessary because CF2 does not produce K with good accuracy except at very low orders
                int n = (int) Math.Floor(nu);
                double mu = nu - n;

                // compute K, K' at this point (which is beyond the turning point because mu is small) using CF2
                double K, KP, g;
                ModifiedBessel_CF_K(mu, x, out K, out g);
                KP = g * K;

                // recurse upward to order nu
                ModifiedBesselK_RecurrUpward(mu, x, ref K, ref KP, n);

                // determine I'/I at the desired point
                double f = ModifiedBessel_CF1(nu, x);

                // Use the wronskian relationship K I' - I K' = 1/x to determine I and I' seperately
                double I = 1.0 / (f * K - KP) / x;
                double IP = f * I;

                return (new SolutionPair(I, IP, K, KP));

            }

        }


        // Unlike the analogous recurrance for J, Y, this recurrence, as written, is only good for K, not I
        // I and e^{i\pi\nu} K share the same recurrence, but we have written it here so as to avoid the e^{i\pi\nu} factor

        private static void ModifiedBesselK_RecurrUpward (double mu, double x, ref double K, ref double KP, int n) {

            for (int i = 0; i < n; i++) {
                double t = K;
                K = (mu / x) * K - KP;
                mu += 1.0;
                KP = -(t + (mu / x) * K);
            }

        }

        /// <summary>
        /// Computes the regular modified cynlindrical Bessel function.
        /// </summary>
        /// <param name="nu">The order parameter.</param>
        /// <param name="x">The argument, which must be non-negative.</param>
        /// <returns>The value of I<sub>&#x3BD;</sub>(x).</returns>
        /// <remarks>
        /// <para>The modified Bessel functions appear as the solutions of hyperbolic differential equations with
        /// cylindrical or circular symmetry, for example the conduction of heat through a cylindrical pipe.</para>
        /// <para>The regular modified Bessel functions are related to the Bessel fuctions with pure imaginary arguments.</para>
        /// <img src="../images/BesselIBesselJRelation.png" />
        /// <para>The regular modified Bessel functions increase monotonically and exponentially from the origin.</para>
        /// </remarks>
        /// <seealso cref="ModifiedBesselK"/>
        public static double ModifiedBesselI (double nu, double x) {

            // reflect negative nu to positive nu
            if (nu < 0.0) {
                if (Math.Round(nu) == nu) {
                    return (ModifiedBesselI(-nu, x));
                } else {
                    return (ModifiedBesselI(-nu, x) + 2.0 / Math.PI * Sin(0.0, -nu) * ModifiedBesselK(-nu, x));
                }
            }

            if (x < 0.0) throw new ArgumentOutOfRangeException("x");

            if (x < 4.0 + 2.0 * Math.Sqrt(nu)) {
                // close to the origin, use series
                return (ModifiedBesselI_Series(nu, x));
            } else if (x > 32.0 + nu * nu / 2.0) {
                // far from the origin, use asymptotic expansion
                double sI, sIP, sK, sKP;
                ModifiedBessel_Asymptotic(nu, x, out sI, out sIP, out sK, out sKP);
                return (Math.Exp(x) * sI);
            } else {
                // beyond the turning point, we will use CF1 + CF2 in a slightly different way than for the Bessel functions

                // find 0 <= mu < 1 with same fractional part as nu
                // this is necessary because CF2 does not produce K with good accuracy except at very low orders
                int n = (int) Math.Floor(nu);
                double mu = nu - n;

                // compute K, K' at this point (which is beyond the turning point because mu is small) using CF2
                //double K, KP, g;
                double K, g;
                ModifiedBessel_CF_K(mu, x, out K, out g);
                //KP = g * K;

                // recurse K, K' upward to nu
                for (int k = 0; k < n; k++) {
                    //double Kp1 = -KP + (mu / x) * K;
                    double Kp1 = (mu / x - g) * K;
                    mu += 1.0;
                    //KP = -K - (mu / x) * Kp1;
                    g = -(mu / x + K / Kp1);
                    K = Kp1;
                }

                // determine I'/I at the desired point
                double f = ModifiedBessel_CF1(nu, x);

                // use the Wronskian to determine I from (I'/I), (K'/K), and K
                //double I = 1.0 / (f * K - KP) / x;
                double I = 1.0 / (f - g) / K / x;

                return (I);

            }

        }

        /// <summary>
        /// Computes the irregular modified cynlindrical Bessel function.
        /// </summary>
        /// <param name="nu">The order parameter.</param>
        /// <param name="x">The argument.</param>
        /// <returns>The value of K<sub>&#x3BD;</sub>(x).</returns>
        /// <remarks>
        /// <para>The modified Bessel functions are related to the Bessel fuctions with pure imaginary arguments.</para>
        /// <para>The irregular modified Bessel function decreases monotonically and exponentially from the origin.</para>
        /// </remarks>
        public static double ModifiedBesselK (double nu, double x) {

            if (nu < 0.0) return (ModifiedBesselI(-nu, x));

            if (x < 0.0) throw new ArgumentOutOfRangeException("x");

            if (x > 32.0 + nu * nu / 2.0) {
                // for large x, use asymptotic series
                double sI, sIP, sK, sKP;
                ModifiedBessel_Asymptotic(nu, x, out sI, out sIP, out sK, out sKP);
                return(Math.Exp(-x) * sK);
            } else {
                // otherwise, reduce to a problem with -1/2 <= mu <= 1/2 and recurse up

                // determine mu
                int n = (int)Math.Floor(nu + 0.5);
                double mu = nu - n;

                // determine K and K' for mu
                double K, g;
                if (x < 2.0) {
                    double Kp1;
                    ModifiedBesselK_Series(mu, x, out K, out Kp1);
                    g = -Kp1 / K + mu / x;
                } else {
                    ModifiedBessel_CF_K(mu, x, out K, out g);
                }

                // recurse upward to nu
                for (int k = 0; k < n; k++) {
                    double Kp1 = (mu / x - g) * K;
                    mu += 1.0;
                    g = -(mu / x + K / Kp1);
                    K = Kp1;
                }

                return (K);

            }

        }

        // series near the origin; this is entirely analogous to the Bessel series near the origin
        // it has a corresponding radius of rapid convergence, x < 4 + 2 Sqrt(nu)

        // This is exactly the same as BesselJ_Series with xx -> -xx.
        // We could even factor this out into a common method with an additional parameter.

        private static void ModifiedBesselI_Series (double nu, double x, out double I, out double IP) {

            if (x == 0.0) {
                if (nu == 0.0) {
                    I = 1.0;
                    IP = 0.0;
                } else if (nu < 1.0) {
                    I = 0.0;
                    IP = Double.PositiveInfinity;
                } else if (nu == 1.0) {
                    I = 0.0;
                    IP = 0.5;
                } else {
                    I = 0.0;
                    IP = 0.0;
                }
            } else {
                double x2 = x / 2.0;
                double xx = x2 * x2;
                double dI;
                if (nu < 128.0) {
                    dI = Math.Pow(x2, nu) / AdvancedMath.Gamma(nu + 1.0);
                } else {
                    dI = Math.Exp(nu * Math.Log(x2) - AdvancedMath.LogGamma(nu + 1.0));
                }
                I = dI; IP = nu * dI;
                for (int k = 1; k < Global.SeriesMax; k++) {
                    double I_old = I; double IP_old = IP;
                    dI *= xx / k / (nu + k);
                    I += dI; IP += (nu + 2 * k) * dI;
                    if ((I == I_old) && (IP == IP_old)) {
                        IP = IP / x;
                        return;
                    }
                }
            }

        }

        private static double ModifiedBesselI_Series (double nu, double x) {

            if (x == 0.0) {
                // handle x = 0 specially
                if (nu == 0.0) {
                    return (1.0);
                } else {
                    return (0.0);
                }
            } else {
                double dI = Math.Pow(x / 2.0, nu) / AdvancedMath.Gamma(nu + 1.0);
                double I = dI;
                double xx = x * x / 4.0;
                for (int k = 1; k < Global.SeriesMax; k++) {
                    double I_old = I;
                    dI = dI * xx / (nu + k) / k;
                    I += dI;
                    if (I == I_old) {
                        return (I);
                    }
                }
                throw new NonconvergenceException();
            }
        }

        // good for x > 32 + nu^2 / 2
        // this returns scaled values; I = e^(x) sI, K = e^(-x) sK

        // Asymptotic expansions
        //   I_{\nu}(x) = \frac{e^x}{\sqrt{2 \pi x}}  \left[ 1 - \frac{\mu-1}{8x} + \frac{(\mu-1)(\mu-9)}{2! (8x)^2} + \cdots \right]
        //   K_{\nu}(x) = \sqrt{\frac{\pi}{2x}}e^{-x} \left[ 1 + \frac{\mu-1}{8x} + \frac{(\mu-1)(\mu-9)}{2! (8x)^2} + \cdots \right]
        // where \mu = 4 \nu^2. Derivatives
        //   I_{\nu}'(x) = \frac{e^x}{\sqrt{2 \pi x}}   \left[ 1 - \frac{\mu+3}{8x} + \frac{(\mu-1)(\mu+15)}{2! (8x)^2} + \cdots \right]
        //   K_{\nu}'(x) = -\sqrt{\frac{\pi}{2x}}e^{-x} \left[ 1 + \frac{\mu+3}{8x} + \frac{(\mu-1)(\mu+15)}{2! (8x)^2} + \cdots \right]
        // Note series differ only by alternating vs. same sign of terms.

        private static void ModifiedBessel_Asymptotic (double nu, double x, out double sI, out double sIP, out double sK, out double sKP) {

            // precompute some values we will use
            double mu = 4.0 * nu * nu;
            double xx = 8.0 * x;

            // initialize the series
            double I = 1.0; double IP = 1.0;
            double K = 1.0; double KP = 1.0;

            // intialize the term value
            double t = 1.0;

            for (int k = 1; k < Global.SeriesMax; k++) {

                double I_old = I; double K_old = K;
                double IP_old = IP; double KP_old = KP;

                // determine next term values
                int k2 = 2 * k;
                t /= k * xx;
                double tp = (mu + (k2 * k2 - 1)) * t;
                //t *= (mu - (k2-1) * (k2-1));
                t *= (2.0 * (nu - k) + 1.0) * (2.0 * (nu + k) - 1.0);

                // add them, with alternating-sign for I series and same-sign for K series
                if (k % 2 == 0) {
                    I += t;
                    IP += tp;
                } else {
                    I -= t;
                    IP -= tp;
                }
                K += t;
                KP += tp;

                // check for convergence
                if ((I == I_old) && (K == K_old) && (IP == IP_old) && (KP == KP_old)) {
                    double fI = Math.Sqrt(Global.TwoPI * x);
                    double fK = Math.Sqrt(Global.HalfPI / x);
                    sI = I / fI;
                    sIP = IP / fI;
                    sK = K * fK;
                    sKP = -KP * fK;
                    return;
                }

            }


            throw new NonconvergenceException();

            /*
            double fI = 1.0;
            double fK = 1.0;
            double df = 1.0;
            for (int k = 1; k < Global.SeriesMax; k++) {

                double fI_old = fI; double fK_old = fK;

                df = df * (2.0 * (nu - k) + 1.0) * (2.0 * (nu + k) - 1.0) / k / (8.0 * x);

                if (k % 2 == 0) {
                    fI += df;
                } else {
                    fI -= df;
                }

                fK += df;

                if ((fI == fI_old) && (fK == fK_old)) {
                    sI = fI / Math.Sqrt(Global.TwoPI * x);
                    sK = fK / Math.Sqrt(2.0 * x / Math.PI);
                    return;

                }

            }

            throw new NonconvergenceException();
            */
        }

        // compute I'/I via continued fraction
        // this is analogous to CF1 for normal Bessel functions
        // it converges quickly for x < nu and slowly for x > nu

        private static double ModifiedBessel_CF1 (double nu, double x) {
            double q = 2.0 * (nu + 1.0);
            double a = 1.0;
            double b = (q / x);
            double D = 1.0 / b;
            double Df = a / b;
            double f = nu / x + Df;
            for (int k = 2; k < Global.SeriesMax; k++) {
                double f_old = f;

                q += 2.0;
                a = 1.0;
                b = q / x;
                D = 1.0 / (b + a * D);
                Df = (b * D - 1.0) * Df;
                f += Df;

                if (f == f_old) {
                    return (f);
                }
            }

            throw new NonconvergenceException();
        }

        // the continued fraction here is 

        private static void ModifiedBessel_CF_K (double nu, double x, out double K, out double g) {

            double a = 1.0;
            double b = 2.0 * (1.0 + x);

            double D = 1.0 / b;
            double Df = a / b;
            double f = Df;

            // Thompson-Barnett do normalizing sum using these auxiluary variables (see Numerical Recipies)

            double C = (0.5 - nu) * (0.5 + nu);
            double q0 = 0.0;
            double q1 = 1.0;
            double Q = C * q1;
            double S = 1.0 + Q * Df;
            //decimal S = (decimal) (1.0 + Q * Df);

            for (int k = 2; k < Global.SeriesMax; k++) {

                double f_old = f;

                // Lenz method for z1/z0

                a = -(k - nu - 0.5) * (k + nu - 0.5);
                b += 2.0;

                D = 1.0 / (b + a * D);
                Df = (b * D - 1.0) * Df;
                f += Df;

                // Thompson-Barnett sum trick

                double S_old = S;
                
                C = -a / k * C;
                double q2 = (q0 - (b - 2.0) * q1) / a;
                Q += C * q2;
                //S += (decimal) (Q * Df);
                S += Q * Df;

                if ((S == S_old) && (f == f_old)) {
                    g = -((x + 0.5) + (nu + 0.5) * (nu - 0.5) * f) / x;
                    K = Math.Sqrt(Math.PI / 2.0 / x) * Math.Exp(-x) / ((double) S);
                    return;
                }

                q0 = q1; q1 = q2;

            }

            throw new NonconvergenceException();
        }

        // use Wronskian I' K - I K' = 1/x, plus values of I'/I, K'/K, and K, to compute I, I', K, and K'

        /*
        private static SolutionPair ModifiedBessel_Steed (double nu, double x) {

            double f = ModifiedBessel_CF1(nu, x);

            double K, g;
            ModifiedBessel_CF_K(nu, x, out K, out g);

            double KP = g * K;

            double I = 1.0 / K / (f - g) / x;

            double IP = f * I;

            return (new SolutionPair(nu, x, I, IP, K, KP));

        }
        */

        // Temme's series for K0 and K1 for small nu and small x
        // applies only for -1/2 <= nu <= 1/2
        // accurate to all but last couple digits for x < 2; converges further out but accuracy deteriorates rapidly
        // initial constant determination shared with BesselY_Series; we should factor out the common parts

        private static void ModifiedBesselK_Series (double nu, double x, out double K0, out double K1) {

            Debug.Assert((-0.5 <= nu) && (nu <= 0.5));

            if (x == 0.0) {
                K0 = Double.PositiveInfinity;
                K1 = Double.PositiveInfinity;
                return;
            }

            // polynomial variable and coefficient

            double xx = x * x / 4.0;
            double cx = 1.0;

            // determine initial p, q

            double GP = Gamma(1.0 + nu);
            double GM = Gamma(1.0 - nu);

            double p = Math.Pow(x / 2.0, -nu) * GP / 2.0;
            double q = Math.Pow(x / 2.0, +nu) * GM / 2.0;

            // determine initial f; this is rather complicated

            double s = nu * Math.Log(2.0 / x);
            double C1, C2;
            if (Math.Abs(s) < 1.0e-3) {
                C1 = 1.0 + s * s / 2.0 + s * s * s * s / 24;
                C2 = 1.0 + s * s / 6.0 + s * s * s * s / 120.0;
            } else {
                C1 = Math.Cosh(s);
                C2 = Math.Sinh(s) / s;
            }

            double G1, G2, F;
            if (Math.Abs(nu) < 1.0e-5) {
                G1 = -EulerGamma;
                G2 = 1.0;
                F = 1.0;
            } else {
                G1 = (1.0 / GM - 1.0 / GP) / (2.0 * nu);
                G2 = (1.0 / GM + 1.0 / GP) / 2.0;
                F = (nu * Math.PI) / Math.Sin(nu * Math.PI);
            }

            double f = F * (C1 * G1 + C2 * G2 * Math.Log(2.0 / x));

            // run the series

            K0 = cx * f;
            K1 = cx * p;
            for (int k = 1; k < Global.SeriesMax; k++) {

                double K0_old = K0;
                double K1_old = K1;

                cx *= xx / k;

                double kpnu = k + nu;
                double kmnu = k - nu;
                f = (k * f + p + q) / (kpnu * kmnu);
                p = p / kmnu;
                q = q / kpnu;
                double h = p - k * f;

                K0 += cx * f;
                K1 += cx * h;

                if ((K0 == K0_old) && (K1 == K1_old)) {
                    K1 = 2.0 / x * K1;
                    return;
                }
            }
            throw new NonconvergenceException();

        }

#if FUTURE

        /// <summary>
        /// Computes the modified Bessel function of the first kind.
        /// </summary>
        /// <param name="n">The order parameter.</param>
        /// <param name="x">The argument.</param>
        /// <returns>The value of I<sub>n</sub>(x).</returns>
        /// <remarks>
        /// <para>The modified Bessel function are essentially the Bessel functions with an imaginary argument.
        /// In particular, I<sub>n</sub>(x) = (-1)<sup>n</sup> J<sub>n</sub>(i x).</para>
        /// </remarks>
        /// <seealso cref="BesselJ(int,double)"/>
        public static double BesselI (int n, double x) {

            if (n < 0) {
                return (BesselI(-n, x));
            }

            if (x < 0) {
                double I = BesselI(n, -x);
                if (n % 2 != 0) I = -I;
                return (I);
            }

            if (x < 4.0 + 2.0 * Math.Sqrt(n)) {
                return (BesselI_Series(n, x));
            } else if (x > 20.0 + n * n / 4.0) {
                return (Math.Exp(x) * BesselI_Reduced_Asymptotic(n, x));
            } else {
                return (Math.Exp(x) * BesselI_Reduced_Miller(n, x));
            }
        }

        private static double BesselI_Series (double nu, double x) {

            if (x == 0.0) {
                if (nu == 0.0) {
                    return (1.0);
                } else {
                    return (0.0);
                }
            } else {
                double dI = Math.Pow(x / 2.0, nu) / AdvancedMath.Gamma(nu + 1.0);
                double I = dI;
                double xx = x * x / 4.0;
                for (int k = 1; k < Global.SeriesMax; k++) {
                    double I_old = I;
                    dI = dI * xx / (nu + k) / k;
                    I += dI;
                    if (I == I_old) {
                        return (I);
                    }
                }
                throw new NonconvergenceException();
            }
        }

        private static double BesselI_Reduced_Miller (int n, double x) {

            // use I_{k-1} = (2k/x) I_k + I_{k+1} to iterate down from a high k
            // use I_0 + 2 I_1 + 2 I_2 + 2 I_3 + 2 I_4 = e^x to normalize

            // this should be stable for all x, since I_k increases as k decreases

            // assume zero values at a high order
            double IP = 0.0;
            double I = 1.0;

            // do the renormalization sum as we go
            double sum = 0.0;

            // how much higher in order we start depends on how many significant figures we need
            // 30 is emperically enough for double precision
            // iterate down to I_n
            for (int k = n + 40; k > n; k--) {
                sum += I;
                double IM = (2 * k / x) * I + IP;
                IP = I;
                I = IM;
            }

            // remember I_n; we will renormalize it to get the answer
            double In = I;

            // iterate down to I_0
            for (int k = n; k > 0; k--) {
                sum += I;
                double IM = (2 * k / x) * I + IP;
                IP = I;
                I = IM;
            }

            // add the I_0 term to sum
            sum = 2.0 * sum + I;

            // since we are dividing by e^x, sum should be 1, so divide all terms by the actual sum

            return (In / sum);

        }

        private static double BesselI_Reduced_Asymptotic (double nu, double x) {

            // asmyptotic expansion for modified Bessel I
            // development is ~ (nu^2 / x)

            double mu = 4.0 * nu * nu;
            double xx = 8.0 * x;

            double dI = 1.0;
            double I = dI;
            for (int k = 1; k < Global.SeriesMax; k++) {
                double I_old = I;
                int kk = 2 * k - 1;
                dI = - dI * (mu - kk * kk) / xx / k;
                I += dI;
                if (I == I_old) {
                    return (I / Math.Sqrt(2.0 * Math.PI * x));
                }
            }

            // can rewrite as dI = - dI * a * b / xx / k, where a+=2 and b -= 2 for each iteration

            throw new NonconvergenceException();
        }

        // Neuman series no good for computing K0; it relies on a near-perfect cancelation between the I0 term
        // and the higher terms to achieve an exponentially supressed small value

#endif


        /// <summary>
        /// Computes the Airy function of the first kind.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value Ai(x).</returns>
        /// <remarks>
        /// <para>Airy functions are solutions to the Airy differential equation:</para>
        /// <img src="../images/AiryODE.png" />
        /// <para>The Airy functions appear in quantum mechanics in the semiclassical WKB solution to the wave functions in a potential.</para>
        /// <para>For negative arguments, Ai(x) is oscilatory. For positive arguments, it decreases exponentially with increasing x.</para>
        /// </remarks>
        /// <seealso cref="AiryBi"/>
        /// <seealso href="http://en.wikipedia.org/wiki/Airy_functions" />
        public static double AiryAi (double x) {
            if (x < -3.0) {
                double y = 2.0 / 3.0 * Math.Pow(-x, 3.0 / 2.0);
                SolutionPair s = Bessel(1.0 / 3.0, y);
                return (Math.Sqrt(-x) / 2.0 * (s.FirstSolutionValue - s.SecondSolutionValue / Global.SqrtThree));
            } else if (x < 2.0) {
                // Close to the origin, use a power series.
                // Convergence is slightly better for negative x than for positive, so we use it further out to the left of the origin.
                return (AiryAi_Series(x));
                // The definitions in terms of bessel functions becomes 0 X infinity at x=0, so we can't use them directly here anyway.
            } else {
                double y = 2.0 / 3.0 * Math.Pow(x, 3.0 / 2.0);
                return (Math.Sqrt(x / 3.0) / Math.PI * ModifiedBesselK(1.0 / 3.0, y));
            }
        }

        // problematic for positive x once exponential decay sets in; don't use for x > 2

        //private static readonly double AiryNorm1 = 1.0 / (Math.Pow(3.0, 2.0 / 3.0) * AdvancedMath.Gamma(2.0 / 3.0));

        //private static readonly double AiryNorm2 = 1.0 / (Math.Pow(3.0, 1.0 / 3.0) * AdvancedMath.Gamma(1.0 / 3.0));

        private static double AiryAi_Series (double x) {

            double AiryNorm1 = 1.0 / (Math.Pow(3.0, 2.0 / 3.0) * AdvancedMath.Gamma(2.0 / 3.0));
            double AiryNorm2 = 1.0 / (Math.Pow(3.0, 1.0 / 3.0) * AdvancedMath.Gamma(1.0 / 3.0));

            double p = AiryNorm1;
            double q = AiryNorm2 * x;
            double f = p - q;
            //double fp = -AiryNorm2;

            double x3 = x * x * x;
            for (int k = 0; k < Global.SeriesMax; k+=3) {
                double f_old = f;
                p *= x3 / ((k + 2)*(k + 3));
                q *= x3 / ((k + 3)*(k + 4));
                f += p - q;
                //fp += (k + 3) * p / x - (k + 4) * q / x;
                if (f == f_old) {
                    return (f);
                }
            }
            throw new NonconvergenceException();

        }

        /// <summary>
        /// Computes the Airy function of the second kind.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value Bi(x).</returns>
        /// <remarks>
        /// <para>The Airy functions appear in quantum mechanics in the semiclassical WKB solution to the wave functions in a potential.</para>
        /// <para>For negative arguments, Bi(x) is oscilatory. For positive arguments, it increases exponentially with increasing x.</para>
        /// <para>While the notation Bi(x) was chosen simply as a natural complement to Ai(x), it has influenced the common nomenclature for
        /// this function, which is now often called the "Bairy function".</para>
        /// </remarks>        
        /// <seealso cref="AiryAi"/>
        /// <seealso href="http://en.wikipedia.org/wiki/Airy_functions" />
        public static double AiryBi (double x) {
            if (x < -3.0) {
                // change to use a function that returns J and Y together
                double y = 2.0 / 3.0 * Math.Pow(-x, 3.0 / 2.0);
                double J = BesselJ(1.0 / 3.0, y);
                double Y = BesselY(1.0 / 3.0, y);
                return (-Math.Sqrt(-x) / 2.0 * (J / Global.SqrtThree + Y));
            } else if (x < 5.0) {
                // The Bi series is better than the Ai series for positive values, because it blows up rather than down.
                // It's also slightly better for negative values, because a given number of oscilations occur further out.  
                return (AiryBi_Series(x));
            } else {
                // change to use a function that returns I and K together
                double y = 2.0 / 3.0 * Math.Pow(x, 3.0 / 2.0);
                SolutionPair s = ModifiedBessel(1.0 / 3.0, y);
                return (Math.Sqrt(x) * (2.0 / Global.SqrtThree * s.FirstSolutionValue + s.SecondSolutionValue / Math.PI));
            }
        }

        private static double AiryBi_Series (double x) {

            double p = 1.0 / (Math.Pow(3.0, 1.0 / 6.0) * AdvancedMath.Gamma(2.0 / 3.0));
            double q = x * (Math.Pow(3.0, 1.0 / 6.0) / AdvancedMath.Gamma(1.0 / 3.0));
            double f = p + q;

            double x3 = x * x * x;
            for (int k = 0; k < Global.SeriesMax; k += 3) {
                double f_old = f;
                p *= x3 / ((k + 2) * (k + 3));
                q *= x3 / ((k + 3) * (k + 4));
                f += p + q;
                if (f == f_old) return (f);
            }
            throw new NonconvergenceException();

        }


        /// <summary>
        /// Computes both Airy functions and their derivatives.
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The values of Ai(x), Ai'(x), Bi(x), and Bi'(x).</returns>
        public static SolutionPair Airy (double x) {
            if (x < -2.0) {
                // Map to Bessel functions for negative values
                double z = 2.0 / 3.0 * Math.Pow(-x, 3.0 / 2.0);
                SolutionPair p = Bessel(1.0 / 3.0, z);
                double a = (p.FirstSolutionValue - p.SecondSolutionValue / Global.SqrtThree) / 2.0;
                double b = (p.FirstSolutionValue / Global.SqrtThree + p.SecondSolutionValue) / 2.0;
                double sx = Math.Sqrt(-x);
                return (new SolutionPair(
                    sx * a, (x * (p.FirstSolutionDerivative - p.SecondSolutionDerivative / Global.SqrtThree) - a / sx) / 2.0,
                    -sx * b, (b / sx - x * (p.FirstSolutionDerivative / Global.SqrtThree + p.SecondSolutionDerivative)) / 2.0
                ));
            } else if (x < 2.0) {
                // Use series near origin
                return (Airy_Series(x));
            } else {
                // Map to modified Bessel functions for positive values
                double z = 2.0 / 3.0 * Math.Pow(x, 3.0 / 2.0);
                SolutionPair p = ModifiedBessel(1.0 / 3.0, z);
                double a = 1.0 / (Global.SqrtThree * Math.PI);
                double b = 2.0 / Global.SqrtThree * p.FirstSolutionValue + p.SecondSolutionValue / Math.PI;
                double sx = Math.Sqrt(x);
                return (new SolutionPair(
                    a * sx * p.SecondSolutionValue, a * (x * p.SecondSolutionDerivative + p.SecondSolutionValue / sx / 2.0),
                    sx * b, x * (2.0 / Global.SqrtThree * p.FirstSolutionDerivative + p.SecondSolutionDerivative / Math.PI) + b / sx / 2.0
                ));
            }

            // NR recommends against using the Ai' and Bi' expressions obtained from simply differentiating the Ai and Bi expressions
            // They give instead expressions involving Bessel functions of different orders. But their reason is to avoid the cancellations
            // among terms ~1/x that get large near x ~ 0, and their method would require two Bessel evaluations.
            // Since we use the power series near x ~ 0, we avoid their cancelation problem and can get away with a single Bessel evaluation.

            // It might appear that we can optimize by not computing both \sqrt{x} and x^{3/2} explicitly, but instead computing \sqrt{x} and
            // then (\sqrt{x})^3. But the latter method looses accuracy, presumably because \sqrt{x} compresses range and cubing then expands
            // it, skipping over the double that is actuall closest to x^{3/2}. So we compute both explicitly.

        }


        private static SolutionPair Airy_Series (double x) {

            // compute k = 0 terms in f' and g' series, and in f and g series
            // compute terms to get k = 0 terms in a' and b' and a and b series

            double g = 1.0 / Math.Pow(3.0, 1.0 / 3.0) / AdvancedMath.Gamma(1.0 / 3.0);
            double ap = -g;
            double bp = g;

            double f = 1.0 / Math.Pow(3.0, 2.0 / 3.0) / AdvancedMath.Gamma(2.0 / 3.0);
            g *= x;
            double a = f - g;
            double b = f + g;

            // we will need to multiply by x^2 to produce higher terms, so remember it
            double x2 = x * x;

            for (int k = 1; k < Global.SeriesMax; k++) {

                // remember old values
                double a_old = a;
                double b_old = b;
                double ap_old = ap;
                double bp_old = bp;

                // compute 3k
                double tk = 3 * k;

                // kth term in f' and g' series, and corresponding a' and b' series
                f *= x2 / (tk - 1);
                g *= x2 / tk;
                ap += (f - g);
                bp += (f + g);

                // kth term in f and g series, and corresponding a and b series
                f *= x / tk;
                g *= x / (tk + 1);
                a += (f - g);
                b += (f + g);

                // check for convergence
                if ((a == a_old) && (b == b_old) && (ap == ap_old) && (bp == bp_old)) {
                    return (new SolutionPair(
                        a, ap,
                        Global.SqrtThree * b, Global.SqrtThree * bp
                    ));
                }
            }

            throw new NonconvergenceException();

        }

    }

}
