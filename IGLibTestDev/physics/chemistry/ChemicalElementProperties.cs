// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Physics;



namespace IG.Physics
{


    /// <summary>Properties of chemical element.</summary>
    /// $A Igor Dec08;
    public class ChemicalElementProperties
    {

        private ChemicalElementProperties() { } // prevent argument-less constructor

        /// <summary>Constructs new chemical element properties.</summary>
        /// <param name="symbol">Chemical symbol of the element that is addressed.</param>
        /// <param name="atomicNumber">Atomic number of the element that is addressed.</param>
        /// <param name="atomicWeight">Atomic weight of the element that is addressed.</param>
        public ChemicalElementProperties(string symbol, int atomicNumber, double atomicWeight)
        {
            this.Symbol = symbol;
            this.AtomicNumber = atomicNumber;
            this.AtomicWeight = atomicWeight;
        }


        /// <summary>Constructs new chemical element properties that obtain data form the enumerator of type <see cref="ChemicalElements"/>.
        /// <para>Only some basic data can be obtained from the argument, other data can be set later.</para></summary>
        /// <param name="whichElement">Enumerator of type <see cref="ChemicalElements"/> that specifies for which element properties are
        /// taken.</param>
        public ChemicalElementProperties(ChemicalElements whichElement)
        {
            this.Symbol = whichElement.ToString();
            this.AtomicNumber = (int) whichElement;
            this.AtomicWeight = 0;
        }


        /// <summary>Constructs new chemical element properties that obtain data form the specified chemical element.
        /// <para>Only some basic data can be obtained from the argument, other data can be set later.</para></summary>
        /// <param name="symbol">Chemical symbol of the element that is addressed.</param>
        public ChemicalElementProperties(ChemicalElement element)
        {
            this.Symbol = element.Symbol;
            this.AtomicNumber = element.AtomicNumber;
            this.AtomicWeight = element.AtomicWeight;
        }

        public ChemicalElementProperties(ChemicalElementProperties elementProperties)
        {
            CopyPlain(elementProperties, this);
        }

        #region Data

        public string Symbol;

        public int AtomicNumber;

        public double AtomicWeight;

        #endregion Data

        #region Operation


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Element properties: ");
            sb.AppendLine("  Symbol: " + Symbol);
            sb.AppendLine("  Atomic number: " + AtomicNumber);
            sb.AppendLine("  Atomic weight: " + AtomicWeight);
            return sb.ToString();
        }

        #endregion Operation 

        #region Static

        /// <summary>Copies the specified chemical element properties to another object of the same type.
        /// <para>This is a plain method, it is not checked if objects are allocated.</para></summary>
        /// <param name="element">Chemical element properties that are copied.</param>
        /// <param name="result">Object to which properties are copied.</param>
        public static void CopyPlain(ChemicalElementProperties prop, ChemicalElementProperties result)
        {
            result.Symbol = prop.Symbol;
            result.AtomicNumber = prop.AtomicNumber;
            result.AtomicWeight = prop.AtomicWeight;
        }

        /// <summary>Copies chemical properties of the specified chemical element to the specified
        /// element properties object.
        /// <para>If the specified element contains the properties object (i.e. <see cref="ChemicalElement.Properties"/>!=null)
        /// then data from the properties object are copied, too.</para>
        /// <para>This is a plain method, it is not checked if objects are allocated.</para></summary>
        /// <param name="element">Element from which properties are copied.</param>
        /// <param name="result">Object to which properties are copied.</param>
        public static void CopyPlain(ChemicalElement element, ChemicalElementProperties result)
        {
            result.Symbol = element.Symbol;
            result.AtomicNumber = element.AtomicNumber;
            result.AtomicWeight = element.AtomicWeight;
            if (element.Properties != null)
                CopyPlain(element.Properties, result);
        }

        #endregion Static

    } // class ChemicalElementProperties


}

