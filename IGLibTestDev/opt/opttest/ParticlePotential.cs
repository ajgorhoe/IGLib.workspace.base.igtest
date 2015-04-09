using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;


namespace IG.Lib.opt.opttest
{
    
    /// <summary>Base class for the charged particle potential problem.</summary>
    /// <remarks>
    /// <para>Description of the problem:</para>
    /// <para>This class represents the problem of minimal potential arrangement of a number of 
    /// charged particles that are confined to the specified region of space (in the current case,
    /// this happens to be the hyperball with radius 1). The class represent methods for calculation 
    /// of the total potential of particles and its gradienn with respect to parameters, as well as 
    /// individual particle potentials and their gradients with respect to the particle coordinates.
    /// </para>
    /// <para>Charges can be assigned to particles (by default, charges are eual to 1), as well as a 
    /// factor in potential (that replaces 1/(4 Pi epsilon_0) in physical euation for charged 
    /// particles potential; by default, this factor is also 1). By default, the potential funtion is
    /// equal to 1/r, but a customized potential function can also be provided.</para>
    /// <pare></pare>
    /// <para>Things to be done in the future:</para>
    /// <para>Possibility of adding charged particles with fixed positions. These particles can contribute
    /// to the potential and thus influence the equilibrium positions of free particles, but can not
    /// change positions. Adding this possibility can be useful for calculating maximal distance arrangements
    /// of sampling points with a given number of already sampled points whose positions are fixed.</para>
    /// </remarks>
    /// $A Igor xx Jul09 Apr10;
    public class ParticlePotentialProblem : ILockable
    {

        /// <summary>Prevent argument-less constructor.</summary>
        private ParticlePotentialProblem()
        { }

        #region ThreadLocking

        private readonly object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ThreadLocking

        #region Data


        protected int _numParticles = 8;

        /// <summary>Number of charged particles in the arrangement.
        /// <para>Once the object is created, this can not be changed.</para></summary>
        public int NumParticles
        {
            get { return _numParticles; }
            protected set
            {
                lock (Lock)
                {
                    // Update dependencies:
                    if (value != _numParticles)
                    {
                        ConstraintFunctions = null;
                    }
                    _numParticles = value;
                }
            }
        }

        protected int _spaceDimension = 2;

        /// <summary>Dimension of space in which particles are positioned.
        /// <para>Once the object is created, this can not be changed.</summary>
        public int SpaceDimension
        {
            get { return _spaceDimension; }
            protected set { _spaceDimension = value; }
        }

        /// <summary>Dimension of the problem, product of the space dimension and the number of free particles.</summary>
        public int ProblemDimension
        {
            get { return _numParticles * _spaceDimension; }
        }


        protected double _potentialFactor = 1.0;

        public double PotentialFactor
        {
            get { return _potentialFactor; }
            protected set { _potentialFactor = value; }
        }

        public double GetPotentialFactorPhysical()
        {
            return 1.0/(4.0 * Math.PI * ConstPhysical.ElectricConstant.Value);
        }


        private double[] _charges;

        /// <summary>Gets or sets the array of particle charges. Length of the array must be the same as the
        /// number of particles. If not set, getter will return the array in which all charges are equal to
        /// 1.</summary>
        public double[] Charges
        {
            get
            {
                lock (Lock)
                {
                    bool create = false;
                    if (_charges == null)
                        create = true;
                    else if (_charges.Length != _numParticles)
                        create = true;
                    if (create)
                    {
                        _charges = new double[_numParticles];
                        for (int i = 0; i < _numParticles; ++i)
                            _charges[i] = 1.0;
                    }
                    return _charges;
                }
            }
            set
            {
                lock (Lock)
                {
                    if (value != null)
                    {
                        if (value.Length != _numParticles)
                            throw new ArgumentException("Size of charges array (" + value.Length
                                + ") is different than the number of charges (" + _numParticles + ").");
                    }
                    _charges = value;
                }
            }
        }

        /// <summary>Sets all particle charges to the specified value.</summary>
        /// <param name="charge">Charge assigned to all particles.</param>
        public void SetCharges(double charge)
        {
            lock (Lock)
            {
                double[] charges = Charges;  // creates internal storage if it does not yet exist.
                for (int i = 0; i < _numParticles; ++i)
                    charges[i] = charge;
            }
        }


        protected IRealFunction _potentialFunction = Func.GetReciprocal();

        /// <summary>The potential function.
        /// <para>As function of distance between two particles, this function defines 
        /// electrostatic potential energy contributed by these two particles when multiplied
        /// by a product of particle charges and a multiplicative consrant (defined by the property <see cref="PotentialFactor"/>).</para>
        /// <para>Set to f(x)=1/x by default.</para></summary>
        public IRealFunction PotentialFunction
        {
            get
            {
                return _potentialFunction;
            }
            set
            {
                if (value == null)
                    value = Func.GetReciprocal();
                _potentialFunction = value;
            }
        }


        #region StaticCharges

        private int _numStaticParticles;

        /// <summary>Gets the number of static particles.</summary>
        public int NumStaticParticles
        {
            get
            {
                return _numStaticParticles;
            }
            private set { _numStaticParticles = value; }
        }

        private IVector[] _staticParticleCoordinates;

        /// <summary>Array of coordinate vectors of staticle charges.</summary>
        public IVector[] StaticParticleCoordinates
        {
            get { return _staticParticleCoordinates; }
            set
            {
                lock (Lock)
                {
                    if (value != null)
                    {
                        _numStaticParticles = value.Length;
                        if (_staticParticleCharges != null)
                            if (_staticParticleCharges.Length != _numStaticParticles)
                                _staticParticleCharges = null;
                    }
                    else
                    {
                        _numStaticParticles = 0;
                    }
                    _staticParticleCoordinates = value;
                }
            }
        }

        private double[] _staticParticleCharges;

        /// <summary>Gets or sets the array of static particle's charges. 
        /// If not set, getter will return the array in which all charges are equal to 1.</summary>
        public double[] StaticParticleCharges
        {
            get
            {
                lock (Lock)
                {
                    bool create = false;
                    if (_staticParticleCharges == null)
                        create = true;
                    else if (_staticParticleCharges.Length != _numStaticParticles)
                        create = true;
                    if (create)
                    {
                        _staticParticleCharges = new double[_numStaticParticles];
                        for (int i = 0; i < _numStaticParticles; ++i)
                            _staticParticleCharges[i] = 1.0;
                    }
                    return _staticParticleCharges;
                }
            }
            set
            {
                lock (Lock)
                {
                    if (value != null)
                    {
                        if (value.Length != _numStaticParticles)
                            throw new ArgumentException("Size of static particle charges array (" + value.Length
                                + ") is different than the number of static particles (" + _numParticles + ").");
                    }
                    _staticParticleCharges = value;
                }
            }
        }

        /// <summary>Sets all static particle charges to the specified value.</summary>
        /// <param name="charge">Charge assigned to all static particles.</param>
        public void SetStaticCharges(double charge)
        {
            lock (Lock)
            {
                double[] charges = StaticParticleCharges;  // creates internal storage if it does not yet exist.
                int num = charges.Length;
                for (int i = 0; i < charges.Length; ++i)
                    charges[i] = charge;
            }
        }


        #endregion StaticCharges


        #endregion Data



        #region CalculationObjective


        /// <summary>Returns parameter index that corresponds to the specified particle index
        /// and to the specified particle coordinate.</summary>
        /// <param name="particleIndex">Particle index.</param>
        /// <param name="coordinateIndex">Index of particle coordinate.</param>
        /// <remarks>Coordinates of all particles are collected in a single parameter vector.
        /// Individual particle coordinates are elements of this vector, and there exist an unique 
        /// transformation between the parameter index and the pair (particle index, particle coordinate).</remarks>
        public int ParameterIndex(int particleIndex, int coordinateIndex)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            return particleIndex * _numParticles + coordinateIndex;
        }

        /// <summary>Returns index of the particle to which the specified element of the global parameter
        /// vector belongs.</summary>
        /// <param name="parameterIndex">Index of the element of the global parameter vector.</param>
        /// <remarks>Coordinates of all particles are collected in a single parameter vector.
        /// Individual particle coordinates are elements of this vector, and there exist an unique 
        /// transformation between the parameter index and the pair (particle index, particle coordinate).</remarks>
        public int ParticleIndex(int parameterIndex)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            return parameterIndex / _numParticles;
        }

        /// <summary>Returns coordinate index of the corresponding particle that is equivalent to the
        /// specified element of the global parameter vector.</summary>
        /// <param name="parameterIndex">Index of the element of the global parameter vector.</param>
        /// <remarks>Coordinates of all particles are collected in a single parameter vector.
        /// Individual particle coordinates are elements of this vector, and there exist an unique 
        /// transformation between the parameter index and the pair (particle index, particle coordinate).</remarks>
        public int CoordinateIndex(int parameterIndex)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            return parameterIndex % _numParticles;
        }

        /// <summary>Extracts and returns the specified coordinate of the specified particle from the 
        /// specified parameter vector.</summary>
        /// <param name="particleIndex">Particle index.</param>
        /// <param name="coordinateIndex">Index of particle coordinate.</param>
        /// <param name="parameters">Vector of parameters where coordinates fo all particles are collected.</param>
        public double ParticleCoordinate(int particleIndex, int coordinateIndex, IVector parameters)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            return parameters[ParameterIndex(particleIndex, coordinateIndex)];
        }

        /// <summary>Returns the Euclidean distance between the two particles with specified indices.</summary>
        /// <param name="firstIndex">Index of the first particle.</param>
        /// <param name="secondIndex">Index of the second particle.</param>
        /// <param name="parameters">Vector of parameters.</param>
        public double ParticleDistance(int firstIndex, int secondIndex, IVector parameters)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            double ret = 0;
            for (int iCoordinate = 0; iCoordinate < _spaceDimension; ++iCoordinate)
            {
                double coord1 = ParticleCoordinate(firstIndex, iCoordinate, parameters);
                double coord2 = ParticleCoordinate(secondIndex, iCoordinate, parameters);
                ret += (coord2 - coord1) * (coord2 - coord1);
            }
            return ret;
        }


        #region StaticPotentialTerms

        /// <summary>Calculates and returns the distance between the specified static particle and the
        /// specified free particle of the particle arrangement.</summary>
        /// <param name="staticParticleIndex">Index of the static particle.</param>
        /// <param name="particleIndex">Index of free particle.</param>
        /// <param name="parameters">Problem parameters, assembled vector of coordinated of all free particles.</param>
        /// <param name="staticCoordinates">Array of coordinates of static particles.</param>
        public virtual double StaticParticleDistance(int staticParticleIndex, int particleIndex,
            IVector parameters, IVector[] staticCoordinates)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            double ret = 0;
            for (int iCoordinate = 0; iCoordinate < _spaceDimension; ++iCoordinate)
            {
                double coord1 = ParticleCoordinate(particleIndex, iCoordinate, parameters);
                double coord2 = staticCoordinates[staticParticleIndex][iCoordinate];
                ret += (coord2 - coord1) * (coord2 - coord1);
            }
            return ret;
        }


        /// <summary>Calculates and returns pairwise electrical potential energy of the specified static and
        /// the specified free particle, providing the necessary coefficiets through parameters.</summary>
        /// <param name="staticParticleIndex">Index of the static particle.</param>
        /// <param name="particleIndex">Index of the free particle.</param>
        /// <param name="potentialFactor">Factor by which potential is multiplied.</param>
        /// <param name="charges">Array of particle charges.</param>
        /// <param name="potentialFunction">Function of distance between two particles that defines 
        /// electrostatic potential energy contributed by these two particles when multiplied
        /// by a product of particle charges and a multiplicative constant.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        /// <param name="staticCharges">Array of static particle's charges.</param>
        /// <param name="staticCoordinates">Array of coordinate vectors of static particles.</param>
        public virtual double StaticPairwisePotentialEnergy(int staticParticleIndex, int particleIndex,
            double potentialFactor, double[] charges, IRealFunction potentialFunction, IVector parameters,
            double[] staticCharges, IVector[] staticCoordinates)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            return potentialFactor * staticCharges[staticParticleIndex] * charges[particleIndex]
                * potentialFunction.Value(StaticParticleDistance(staticParticleIndex, particleIndex, parameters, staticCoordinates));
        }


        /// <summary>Calculates and returns electrical potential energy of the specified static and free
        /// particle.
        /// <para>All the necessary parameters are obteined from the object.</para></summary>
        /// <param name="referenceIndex">Indx of the static particle.</param>
        /// <param name="particleIndex">Index of the free particle.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        public virtual double StaticPairwisePotentialEnergy(int staticParticleIndex, int particleIndex,
            IVector parameters)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                double[] charges = Charges;
                double[] staticCharges = StaticParticleCharges;
                IVector[] staticCoordinates = StaticParticleCoordinates;
                IRealFunction potentialFunction = this.PotentialFunction;
                return StaticPairwisePotentialEnergy(staticParticleIndex, particleIndex, potentialFactor,
                    charges, potentialFunction, parameters, staticCharges, staticCoordinates);
            }
        }

        #endregion StaticPotentialTerms


        /// <summary>Calculates and returns pairwise electrical potential energy of two particles,
        /// providing the necessary coefficiets through parameters.</summary>
        /// <param name="referenceIndex">Indx of the first particle.</param>
        /// <param name="particleIndex">Index of the second particle.</param>
        /// <param name="potentialFactor">Factor by which potential is multiplied.</param>
        /// <param name="charges">Array of particle charges.</param>
        /// <param name="potentialFunction">Function of distance between two particles that defines 
        /// electrostatic potential energy contributed by these two particles when multiplied
        /// by a product of particle charges and a multiplicative consrant.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        public virtual double PairwisePotentialEnergy(int referenceIndex, int particleIndex,
            double potentialFactor, double[] charges, IRealFunction potentialFunction, IVector parameters)
        {
            if (referenceIndex == particleIndex)
                throw new ArgumentException("Both particle indices are the same. Can not calculate pairwise potential "
                    + Environment.NewLine + "  energy between a particle and itself.");
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            return potentialFactor * charges[referenceIndex] * charges[particleIndex]
                * potentialFunction.Value(ParticleDistance(referenceIndex, particleIndex, parameters));
        }

        /// <summary>Calculates and returns electrical potential energy of two particles.</summary>
        /// <param name="referenceIndex">Indx of the first particle.</param>
        /// <param name="particleIndex">Index of the second particle.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        public virtual double PairwisePotentialEnergy(int referenceIndex, int particleIndex, IVector parameters)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                double[] charges = Charges;
                IRealFunction potentialFunction = this.PotentialFunction;
                return PairwisePotentialEnergy(referenceIndex, particleIndex, potentialFactor,
                    charges, potentialFunction, parameters);
            }
        }

        /// <summary>Calculates and returns the total potential energy of all particles in the formation.</summary>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        public virtual double PotentialEnergy(IVector parameters)
        {
            lock (Lock)
            {
                double[] charges = this.Charges;
                double potentialFactor = this.PotentialFactor;
                IRealFunction potentialFunction = this.PotentialFunction;
                double ret = 0;
                for (int i = 0; i < _numParticles; ++i)
                {
                    for (int j = i + 1; j < _numParticles; ++j)
                    {
                        if (i!=j)
                            ret += PairwisePotentialEnergy(i, j, potentialFactor, charges, potentialFunction, parameters);
                    }
                }
                // Add contributributions from static particles if defined:
                if (_numStaticParticles > 0)
                {
                    double[] staticCharges = StaticParticleCharges;
                    IVector[] staticCoordinates = StaticParticleCoordinates;
                    for (int staticParticleIndex = 0; staticParticleIndex < _numStaticParticles; ++staticParticleIndex)
                        for (int particleIndex=0; particleIndex<_numParticles; ++ particleIndex)
                    {
                        ret += StaticPairwisePotentialEnergy(
                            staticParticleIndex, particleIndex, potentialFactor, charges, potentialFunction, parameters, staticCharges, staticCoordinates);
                    }
                }
                return ret;
            }
        }

        
        #region StaticForceTerms



        /// <summary>Calculates the electrostatic force acting on the specified free particle by a single specified statitc particle.</summary>
        /// <param name="staticParticleIndex">Index of the particle that is a source of electrostatic force.</param>
        /// <param name="particleIndex">Index of the particle on which the calculated force acts.</param>
        /// <param name="potentialFactor">Factor by which potential is multiplied.</param>
        /// <param name="charges">Array of particle charges.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        /// <param name="particleForce">Vector where particle force is stored to.</param>
        public void StaticParticleSingleForce(int staticParticleIndex, int particleIndex,
            double potentialFactor, double[] charges, IRealFunction potentialFunction, IVector parameters,
            double[] staticCharges, IVector[] staticCoordinates,
            ref IVector particleForce)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            Vector.Resize(ref particleForce, _spaceDimension);
            double distance = StaticParticleDistance(staticParticleIndex, particleIndex, parameters,staticCoordinates);
            // Calculate factor by which difference of position vectors is multiplied;
            // negative sign is because force is in the negative direction of the potential growth 
            double factor = -staticCharges[staticParticleIndex] * charges[particleIndex]
                * potentialFactor * potentialFunction.Derivative(distance);
            for (int i = 0; i < _spaceDimension; ++i)
            {
                particleForce[i] = factor * (ParticleCoordinate(particleIndex, i, parameters)
                    - staticCoordinates[staticParticleIndex][i]);
            }
        }

        /// <summary>Calculates the electrostatic force acting on the specified free particle by a single 
        /// specified static particle.</summary>
        /// <param name="staticParticleIndex">Index of the particle that is a source of electrostatic force.</param>
        /// <param name="particleIndex">Index of the particle on which the calculated force acts.</param>
        /// <param name="particleForce">Vector where particle force is stored to.</param>
        /// <remarks><para>Charges of other particles, potential factor and potential function are obtained </para></remarks>
        public void StaticParticleSingleForce(int staticParticleIndex, int particleIndex, IVector parameters, ref IVector particleForce)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                double[] charges = Charges;
                double[] staticCarges = StaticParticleCharges;
                IVector[] staticCoordinates = StaticParticleCoordinates;
                IRealFunction potentialFunction = this.PotentialFunction;
                StaticParticleSingleForce(staticParticleIndex, particleIndex, potentialFactor, charges,
                    potentialFunction, parameters, staticCarges, staticCoordinates, ref particleForce);
            }
        }


        /// <summary>Calculates the electrostatic force acting on the specified free particle by all 
        /// static particles in the arrangement.</summary>
        /// <param name="particleIndex">Index of the particle on which the calculated force acts.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        /// <param name="particleForce">Vector where particle force is stored to.</param>
        public virtual void StaticParticleSingleForce(int particleIndex, IVector parameters, ref IVector particleForce)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                IRealFunction potentialFunction = this.PotentialFunction;
                double[] charges = Charges;
                double[] staticCarges = StaticParticleCharges;
                IVector[] staticCoordinates = StaticParticleCoordinates;
                Vector.Resize(ref particleForce, _spaceDimension);
                Vector.SetZero(particleForce);
                for (int staticIndex = 0; staticIndex < _numParticles; ++staticIndex)
                {
                    // Calculate contribution to force from a single static particle:
                    StaticParticleSingleForce(staticIndex, particleIndex, potentialFactor, charges,
                        potentialFunction, parameters, staticCarges, staticCoordinates, ref _auxParticleVector);
                    // Add this contribution to the total force on the specified particle
                    for (int compIndex = 0; compIndex < _spaceDimension; ++compIndex)
                        particleForce[compIndex] += _auxParticleVector[compIndex];
                }
            }
        }


        #endregion StaticForceTerms


        /// <summary>Calculates the electrostatic force acting on the specified particle by a single specified another particle.</summary>
        /// <param name="referenceIndex">Index of the particle that is a source of electrostatic force.</param>
        /// <param name="particleIndex">Index of the particle on which the calculated force acts.</param>
        /// <param name="potentialFactor">Factor by which potential is multiplied.</param>
        /// <param name="charges">Array of particle charges.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        /// <param name="particleForce">Vector where particle force is stored to.</param>
        public void ParticleSingleForce(int referenceIndex, int particleIndex,
            double potentialFactor, double[] charges, IRealFunction potentialFunction, IVector parameters, ref IVector particleForce)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            Vector.Resize(ref particleForce, _spaceDimension);
            double distance = ParticleDistance(referenceIndex, particleIndex, parameters);
            // Calculate factor by which difference of position vectors is multiplied;
            // negative sign is because force is in the negative direction of the potential growth 
            double factor = -charges[referenceIndex] * charges[particleIndex]
                * potentialFactor * potentialFunction.Derivative(distance);
            for (int i = 0; i < _spaceDimension; ++i)
            {
                particleForce[i] = factor * (ParticleCoordinate(particleIndex, i, parameters)
                    - ParticleCoordinate(referenceIndex, i, parameters));
            }
        }

        /// <summary>Calculates the electrostatic force acting on the specified particle by a single 
        /// specified another free particle.</summary>
        /// <param name="referenceIndex">Index of the particle that is a source of electrostatic force.</param>
        /// <param name="particleIndex">Index of the particle on which the calculated force acts.</param>
        /// <param name="particleForce">Vector where particle force is stored to.</param>
        /// <remarks><para>Charges of other particles, potential factor and potential function are obtained </para></remarks>
        public void ParticleSingleForce(int referenceIndex, int particleIndex, IVector parameters, ref IVector particleForce)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                double[] charges = Charges;
                IRealFunction potentialFunction = this.PotentialFunction;
                ParticleSingleForce(referenceIndex, particleIndex, potentialFactor, charges,
                    potentialFunction, parameters, ref particleForce);
            }
        }

        protected IVector _auxParticleVector = null;

        /// <summary>Calculates the electrostatic force acting on the specified particle by all 
        /// other free particles in the arrangement.</summary>
        /// <param name="particleIndex">Index of the particle on which the calculated force acts.</param>
        /// <param name="parameters">Vector of parameters that contains coordinates of all particles.</param>
        /// <param name="particleForce">Vector where particle force is stored to.</param>
        public virtual void ParticleSingleForce(int particleIndex, IVector parameters, ref IVector particleForce)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                double[] charges = Charges;
                IRealFunction potentialFunction = this.PotentialFunction;
                Vector.Resize(ref particleForce, _spaceDimension);
                Vector.SetZero(particleForce);
                for (int referenceIndex = 0; referenceIndex < _numParticles; ++referenceIndex)
                {
                    if (referenceIndex != particleIndex)
                    {
                        // Calculate contribution to force from a single particle that is different
                        // than the observed one:
                        ParticleSingleForce(referenceIndex, particleIndex, potentialFactor, charges,
                            potentialFunction, parameters, ref _auxParticleVector);
                        // Add this contribution to the total force on the specified particle
                        for (int compIndex = 0; compIndex < _spaceDimension; ++compIndex)
                            particleForce[compIndex] += _auxParticleVector[compIndex];
                    }
                }
            }
        }


        /// <summary>Calculates gradient of total potential, which is effectively composed of forces acting on 
        /// the individual particles by all other particles in the arrangement (and that equals to gradients of 
        /// potential energy of individual particle in the field of other particles).</summary>
        /// <param name="parameters">Vector of problem parameters where coordinates of all free particles
        /// are gathered.</param>
        /// <param name="gradient">Vector where gradient of potential energy is stored.</param>
        public virtual void PotentialEnergyGradient(IVector parameters, ref IVector gradient)
        {
            lock (Lock)
            {
                double potentialFactor = PotentialFactor;
                double[] charges = Charges;
                IRealFunction potentialFunction = this.PotentialFunction;
                Vector.Resize(ref gradient, ProblemDimension);
                gradient.SetZero();
                for (int particleIndex = 0; particleIndex < _numParticles; ++particleIndex)
                {
                    for (int referenceIndex = 0; referenceIndex < _numParticles; ++referenceIndex)
                    {
                        if (referenceIndex != particleIndex)
                        {
                            // Calculate contribution to force from a single particle that is different
                            // than the observed one:
                            ParticleSingleForce(referenceIndex, particleIndex, potentialFactor, charges,
                                potentialFunction, parameters, ref _auxParticleVector);
                            // Add this contribution to the total force on the specified particle
                            for (int compIndex = 0; compIndex < _spaceDimension; ++compIndex)
                                gradient[ParameterIndex(particleIndex, compIndex)] += _auxParticleVector[compIndex];
                        }
                    }
                }
                if (_numStaticParticles > 0)
                {
                    double[] staticCarges = StaticParticleCharges;
                    IVector[] staticCoordinates = StaticParticleCoordinates;
                    for (int particleIndex = 0; particleIndex < _numParticles; ++particleIndex)
                    {
                        for (int staticParticleIndex = 0; staticParticleIndex < _numParticles; ++staticParticleIndex)
                        {
                            // Calculate contribution to force from a single static particle:
                            StaticParticleSingleForce(staticParticleIndex, particleIndex, potentialFactor, charges,
                                potentialFunction, parameters, staticCarges, staticCoordinates, ref _auxParticleVector);
                            // Add this contribution to the total force on the specified particle
                            for (int compIndex = 0; compIndex < _spaceDimension; ++compIndex)
                                gradient[ParameterIndex(particleIndex, compIndex)] += _auxParticleVector[compIndex];

                        }
                    }
                }
            }
        }


        #endregion CalculationObjective


        #region CalculationConstraints


        /// <summary>Returns value of the constraint function for the specified particle in the arrangement.
        /// <para>Constraint is c_i(r_i) = ||r_i||-1 &lt;= 0.</para></summary>
        /// <param name="particleIndex">Index of the particle for which constraint function value is returned.</param>
        /// <param name="parameters">Vector of parameters (global coordinates - assembly of coordinates of all free particles).</param>
        public double ParticleConstraintValue(int particleIndex, IVector parameters)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            // Distance of the particle from the coordinate origin:
            double r = 0;
            for (int iCoordinate = 0; iCoordinate < _spaceDimension; ++iCoordinate)
            {
                double coord = ParticleCoordinate(particleIndex, iCoordinate, parameters);
                r += coord * coord;
            }
            r = Math.Sqrt(r);
            return r - 1.0;
        }


        /// <summary>Calculates and stores gradient of the constraint function related to the 
        /// specified particle, in terms of particle coordinates.
        /// <para>Constraint gradient related to particle i is ∇c_i(r_i) = r_i/||r_i||.</para></summary>
        /// <param name="particleIndex">Index of the particle for which constraint gradient is calculated.</param>
        /// <param name="parameters">Vector of parameters (global coordinates - assembly of coordinates of all free particles).</param>
        /// <param name="partConstraintGrad">Vector of the space dimension where constraint gradient is stored.</param>
        public void ParticleConstraintGradient(int particleIndex, IVector parameters, ref IVector partConstraintGrad)
        {
            // Do not need to lock, only NumParticles and SpaceDimension used of internal variables, but these can not be changed.
            Vector.Resize(ref partConstraintGrad, _spaceDimension);
            // Distance of the particle from the coordinate origin:
            double r = 0;
            for (int iCoordinate = 0; iCoordinate < _spaceDimension; ++iCoordinate)
            {
                double coord = ParticleCoordinate(particleIndex, iCoordinate, parameters);
                r += coord * coord;
            }
            r = Math.Sqrt(r);
            for (int iCoordinate = 0; iCoordinate < _spaceDimension; ++iCoordinate)
            {
                partConstraintGrad[iCoordinate] = ParticleCoordinate(particleIndex, iCoordinate, parameters) / r;
            }
        }



        /// <summary>Returns value of the specified constraint function related to specified particle in the arrangement.
        /// <para>Constraint is c_i(r_i) = ||r_i||-1 &lt;= 0.</para></summary>
        /// <param name="whichConstraint">Specifies Index of the constraint function (that corresponds the index of the related particle).</param>
        /// <param name="parameters">Vector of parameters (global coordinates - assembly of coordinates of all free particles).</param>
        public double ConstraintFunction(int whichConstraint, IVector parameters)
        {
            return ParticleConstraintValue(whichConstraint, parameters);
        }


        /// <summary>Calculates and stores gradient of the specified constraint function that is related to the 
        /// specified particle, in terms of global parameters (vector assembly of all particle coordinates).
        /// <para>Constraint gradient related to particle i is ∇c_i(r_i) = r_i/||r_i||.</para></summary>
        /// <param name="whichConstraint">Specifies Index of the constraint function (that corresponds the index of the related particle).</param>
        /// <param name="parameters">Vector of parameters (global coordinates - assembly of coordinates of all free particles).</param>
        /// <param name="constraintGradient">Vector where constraint gradient is stored, dimension equals dimension of <paramref name="parameters"/>.</param>
        public void ConstraintGradient(int whichConstraint, IVector parameters, ref IVector constraintGradient)
        {
            lock (Lock)
            {
                Vector.Resize(ref constraintGradient, ProblemDimension);
                constraintGradient.SetZero();
                ParticleConstraintGradient(whichConstraint, parameters, ref _auxParticleVector);
                for (int iCoordinate = 0; iCoordinate < _spaceDimension; ++iCoordinate)
                {
                    constraintGradient[ParameterIndex(whichConstraint, iCoordinate)] = _auxParticleVector[iCoordinate];
                }
            }
        }


        #endregion CalculationConstraints


        #region CalculationConstraints





        #endregion CalculationConstraints


        #region ScalarFunctions

        ScalarFunctionBase _objectiveFunction;

        /// <summary>Returns the scalar function that represents the objective function of the current 
        /// minimal particle potential energy problem.</summary>
        public ScalarFunctionBase ObjectiveFunction
        {
            get
            {
                lock (Lock)
                {
                    if (_objectiveFunction == null)
                    {
                        _objectiveFunction = new ObjectiveFunctionPotentialEnergy(this);
                    }
                    return _objectiveFunction;
                }
            }
        }

        ScalarFunctionBase[] _constraintFunctions;

        /// <summary>Returns an array of scalar functions that represent the constraint functions of the current 
        /// minimal particle potential energy problem.</summary>
        public ScalarFunctionBase[] ConstraintFunctions
        {
            get
            {
                lock (Lock)
                {
                    if (_constraintFunctions == null)
                    {
                        _constraintFunctions = new ConstraintFunctionParticleConstraint[_numParticles];
                        for (int iParticle = 0; iParticle < _numParticles; ++iParticle)
                        {
                            _constraintFunctions[iParticle] = new ConstraintFunctionParticleConstraint(this, iParticle);
                        }
                    }
                    return _constraintFunctions;
                }
            }
            protected set
            {
                _constraintFunctions = value;
            }
        }

        #endregion ScalarFunctions



        #region AnalysisFunctions



        #endregion AnalysisFunctions



        #region FunctionClasses


        /// <summary>Class that returns the objective function (total potential energy) of the specified 
        /// particle potential problem.
        /// <para>The underlying particle potential problem is defined as property on the class, 
        /// which is set at initialization.</para></summary>
        /// <remarks></remarks>
        /// $A Igor Jul09;
        protected class ObjectiveFunctionPotentialEnergy : ScalarFunctionBase, IScalarFunction
        {

            #region Construction

            /// <summary>Prevents calling default constructor.</summary>
            private ObjectiveFunctionPotentialEnergy()
            { }

            /// <summary>Cosnstructs a new objective function (total potential energy) of the specified 
            /// particle potential problem.</summary>
            /// <param name="underlyingProblem">Definition of the minimal particle potential problem.</param>
            public ObjectiveFunctionPotentialEnergy(ParticlePotentialProblem underlyingProblem)
                : this(null, underlyingProblem)
            {
                this.ProblemDefinition = underlyingProblem;
            }

            /// <summary>Constructs a new objective function (total potential energy) of the specified 
            /// particle potential problem defined on transformed coordinates.</summary>
            /// <param name="transf">Afifne transformation that transforms the parameters (global coordinates).</param>
            /// <param name="underlyingProblem">Definition of the minimal particle potential energy problem.</param>
            public ObjectiveFunctionPotentialEnergy(IAffineTransformation transf, ParticlePotentialProblem underlyingProblem)
                : base(transf)
            {
                this.ProblemDefinition = underlyingProblem;
            }

            #endregion Construction


            #region Data

            protected ParticlePotentialProblem _problemDefinition;

            public ParticlePotentialProblem ProblemDefinition
            {
                get { return ProblemDefinition; }
                set
                {
                    if (value == null)
                        throw new ArgumentException("Problem definition not specified (null reference).");
                    _problemDefinition = value;
                }

            }

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Total electrostatic energy."; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Objective function of the minimal particle potential energy (total electrostatic energy)."; }
            }

            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                if (parameters == null)
                    throw new ArgumentException("Vector of parameters not specifed (null argument).");
                return ProblemDefinition.PotentialEnergy(parameters);
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                if (parameters == null)
                    throw new ArgumentException("Vector of parameters is not specifed (null argument).");
                ProblemDefinition.PotentialEnergyGradient(parameters, ref gradient);
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }


            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                throw new NotImplementedException("Calculation of the Hessian matrix is not implemented for this function.");
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return false; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation

        }  // class ObjectiveFunctionPotentialEnergy


        /// <summary>Class that returns the objective function (total potential energy) of the specified 
        /// particle potential problem.
        /// <para>The underlying particle potential problem is defined as property on the class, 
        /// which is set at initialization.</para></summary>
        /// <remarks></remarks>
        /// $A Igor Jul09;
        protected class ConstraintFunctionParticleConstraint : ScalarFunctionBase, IScalarFunction
        {

            #region Construction

            /// <summary>Prevents calling default constructor.</summary>
            private ConstraintFunctionParticleConstraint()
            { }

            /// <summary>Constructs a new constraint function for the specified minimal particle potential energy,
            /// corresponding to the specified particle.</summary>
            /// <param name="underlyingProblem">Definition of the minimal particle potential energy problem.</param>
            /// <param name="whichParticle">Index of the particle to which the represented constraint function is assigned.</param>
            public ConstraintFunctionParticleConstraint(ParticlePotentialProblem underlyingProblem, int whichParticle)
                : this(null, underlyingProblem, whichParticle)
            {  }

            /// <summary>Constructs a new constraint function for the specified minimal particle potential energy with coordinate transforamtion,
            /// corresponding to the specified particle.</summary>
            /// <param name="transf">Afifne transformation that transforms the parameters (global coordinates).</param>
            /// <param name="underlyingProblem">Definition of the minimal particle potential energy problem.</param>
            /// <param name="whichParticle">Index of the particle to which the represented constraint function is assigned.</param>
            public ConstraintFunctionParticleConstraint(IAffineTransformation transf, ParticlePotentialProblem underlyingProblem, int whichParticle)
                : base(transf)
            {
                this.ProblemDefinition = underlyingProblem;
                this.ParticleIndex = whichParticle;
            }

            #endregion Construction


            #region Data

            protected int _particleIndex;

            public int ParticleIndex
            {
                get { return _particleIndex; }
                set
                {
                    if (value < 0)
                        throw new ArgumentException("Index of particle corresponding to the constraint function can not be less than 0.");
                    if (_problemDefinition != null)
                    {
                        if (value >= _problemDefinition.NumParticles)
                            throw new ArgumentException("Index of particle corresponding to the constraint function is out of range: "
                                + Environment.NewLine + "  attempted to set to " + value + ", maximum: " +
                                (_problemDefinition.NumParticles - 1));
                    }
                    _particleIndex = value;
                }
            }

            protected ParticlePotentialProblem _problemDefinition;

            public ParticlePotentialProblem ProblemDefinition
            {
                get { return ProblemDefinition; }
                set
                {
                    if (value == null)
                        throw new ArgumentException("Problem definition not specified (null reference).");
                    _problemDefinition = value;
                }

            }

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Particle constraint No. " + ParticleIndex + "."; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Particle constraint No. " + ParticleIndex + "."; }
            }


            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                if (parameters == null)
                    throw new ArgumentException("Vector of parameters not specifed (null argument).");
                return ProblemDefinition.ConstraintFunction(ParticleIndex, parameters);
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                if (parameters == null)
                    throw new ArgumentException("Vector of parameters not specifed (null argument).");
                ProblemDefinition.ConstraintGradient(ParticleIndex, parameters, ref gradient);
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }


            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                throw new NotImplementedException("Calculation of the Hessian matrix is not implemented for this function.");
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return false; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation

        }  // class ConstraintFunctionParticleConstraint


        #endregion FunctionClasses



        #region Examples




        #endregion Examples


    } // class ParticlePotentialProblem





}
