// Copyright (c) Igor Grešovnik (2008-present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;

using IG.Lib;


namespace IG.Num
{

    /// <summary>Base class for definition of a system of first order ordinary differential equations (initial problem).</summary>
    /// <remarks>See also remarks on saving the state for the <see cref="DifferentialFirstOrderSolverBase"/> and
    /// <see cref="DifferentialFirstOrderSystemBase"/> classes.</remarks>
    /// $A Igor Mar2009;
    public abstract class DifferentialFirstOrderSolverEuler : DifferentialFirstOrderSolverBase
    {

        // private DifferentialFirstOrderSolverEuler() { } // prevent argumentless constructor

        /// <summary>Constructs a solver for the specified system of first order ordinary differential
        /// equations.</summary>
        /// <param name="equations"></param>
        public DifferentialFirstOrderSolverEuler(DifferentialFirstOrderSystemBase equations):
            base(equations)
        {  }


    }

}