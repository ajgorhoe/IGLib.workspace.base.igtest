// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;


namespace IG.Physics
{

    /// <summary>Chemical compoind. 
    /// <para>Consists of a list of chemical elements with quantities.</para></summary>
    /// $A Igor Nov08;
    public class ChemicalCompound 
    {

        /// <summary>Constructs a new chemical compound that consists of the specified stoichiometric quantities of elements
        /// listed in the specified order.</summary>
        /// <param name="elementQuantities"></param>
        public ChemicalCompound(params ChemicalElementQuantity[] elementQuantities)
        {
            if (elementQuantities != null)
            {
                for (int i = 0; i < elementQuantities.Length; ++i)
                    AddElement(elementQuantities[i]);
            }
        }

        #region ConstructionHardCoded


        /// <summary>Construct a chemical compound containing the specified element with corresponding stoichiometric number.
        /// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        /// st. number, second element, second number, etc.</para>
        /// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        public ChemicalCompound(
            ChemicalElements el1, int q1):
            this(el1, q1,
            ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0,
            ChemicalElements.None, 0, ChemicalElements.None, 0,ChemicalElements.None, 0,  ChemicalElements.None, 0)
        {  }

        ///// <summary>Construct a chemical compound containing the specified 2 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2):
        //    this(el1, q1, el2, q2,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0,ChemicalElements.None, 0,  ChemicalElements.None, 0)
        //{  }

        ///// <summary>Construct a chemical compound containing the specified 3 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2,
        //    ChemicalElements el3, int q3):
        //    this(el1, q1, el2, q2, el3, q3,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0)
        //{  }

        ///// <summary>Construct a chemical compound containing the specified 4 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2,
        //    ChemicalElements el3, int q3,
        //    ChemicalElements el4, int q4):
        //    this(el1, q1, el2, q2, el3, q3, el4, q4,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, 
        //    ChemicalElements.None, 0, ChemicalElements.None, 0)
        //{  }

        /// <summary>Construct a chemical compound containing the specified 5 elements with corresponding stoichiometric numbers.
        /// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        /// st. number, second element, second number, etc.</para>
        /// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        public ChemicalCompound(
            ChemicalElements el1, int q1,
            ChemicalElements el2, int q2,
            ChemicalElements el3, int q3,
            ChemicalElements el4, int q4,
            ChemicalElements el5, int q5):
            this(el1, q1, el2, q2, el3, q3, el4, q4, el5, q5,
            ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0)
        {  }

        ///// <summary>Construct a chemical compound containing the specified 6 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2,
        //    ChemicalElements el3, int q3,
        //    ChemicalElements el4, int q4,
        //    ChemicalElements el5, int q5,
        //    ChemicalElements el6, int q6):
        //    this(el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0)
        //{  }

        ///// <summary>Construct a chemical compound containing the specified 7 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2,
        //    ChemicalElements el3, int q3,
        //    ChemicalElements el4, int q4,
        //    ChemicalElements el5, int q5,
        //    ChemicalElements el6, int q6,
        //    ChemicalElements el7, int q7):
        //    this(el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, el7, q7,
        //    ChemicalElements.None, 0, ChemicalElements.None, 0, ChemicalElements.None, 0)
        //{  }

        ///// <summary>Construct a chemical compound containing the specified 8 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2,
        //    ChemicalElements el3, int q3,
        //    ChemicalElements el4, int q4,
        //    ChemicalElements el5, int q5,
        //    ChemicalElements el6, int q6,
        //    ChemicalElements el7, int q7,
        //    ChemicalElements el8, int q8):
        //    this(el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, el7, q7, el8, q8, 
        //    ChemicalElements.None, 0, ChemicalElements.None, 0)
        //{  }

        ///// <summary>Construct a chemical compound containing the specified 9 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElements el1, int q1,
        //    ChemicalElements el2, int q2,
        //    ChemicalElements el3, int q3,
        //    ChemicalElements el4, int q4,
        //    ChemicalElements el5, int q5,
        //    ChemicalElements el6, int q6,
        //    ChemicalElements el7, int q7,
        //    ChemicalElements el8, int q8,
        //    ChemicalElements el9, int q9):
        //    this(el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, el7, q7, el8, q8, el9, q9,
        //    ChemicalElements.None,  0)
        //{  }

        /// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        /// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        /// st. number, second element, second number, etc.</para>
        /// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        public ChemicalCompound(
            ChemicalElements el1, int q1,
            ChemicalElements el2, int q2,
            ChemicalElements el3, int q3,
            ChemicalElements el4, int q4,
            ChemicalElements el5, int q5,
            ChemicalElements el6, int q6,
            ChemicalElements el7, int q7,
            ChemicalElements el8, int q8,
            ChemicalElements el9, int q9,
            ChemicalElements el10, int q10)
        {
            if (el1 != ChemicalElements.None && q1 > 0)
                AddElement(new ChemicalElementQuantity(q1, el1));
            if (el2 != ChemicalElements.None && q2 > 0)
                AddElement(new ChemicalElementQuantity(q2, el2));
            if (el3 != ChemicalElements.None && q3 > 0)
                AddElement(new ChemicalElementQuantity(q3, el3));
            if (el4 != ChemicalElements.None && q4 > 0)
                AddElement(new ChemicalElementQuantity(q4, el4));
            if (el5 != ChemicalElements.None && q5 > 0)
                AddElement(new ChemicalElementQuantity(q5, el5));
            if (el6 != ChemicalElements.None && q6 > 0)
                AddElement(new ChemicalElementQuantity(q6, el6));
            if (el7 != ChemicalElements.None && q7 > 0)
                AddElement(new ChemicalElementQuantity(q7, el7));
            if (el8 != ChemicalElements.None && q8 > 0)
                AddElement(new ChemicalElementQuantity(q8, el8));
            if (el9 != ChemicalElements.None && q9 > 0)
                AddElement(new ChemicalElementQuantity(q9, el9));
            if (el10 != ChemicalElements.None && q10 > 0)
                AddElement(new ChemicalElementQuantity(q10, el10));
        }

        /// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        /// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        /// st. number, second element, second number, etc.</para>
        /// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        public ChemicalCompound(
            ChemicalElement el1, int q1)
            : this(
                el1, q1, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0)
        { }

        ///// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1,
        //    ChemicalElement el2, int q2)
        //    : this(
        //        el1, q1, el2, q2, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0)
        //{ }

        ///// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1,
        //    ChemicalElement el2, int q2,
        //    ChemicalElement el3, int q3)
        //    : this(
        //        el1, q1, el2, q2, el3, q3, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0)
        //{ }

        ///// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1,
        //    ChemicalElement el2, int q2,
        //    ChemicalElement el3, int q3,
        //    ChemicalElement el4, int q4)
        //    : this(
        //        el1, q1, el2, q2, el3, q3, el4, q4, null, 0, null, 0, null, 0, null, 0, null, 0, null, 0)
        //{ }

        /// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        /// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        /// st. number, second element, second number, etc.</para>
        /// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        public ChemicalCompound(
            ChemicalElement el1, int q1,
            ChemicalElement el2, int q2,
            ChemicalElement el3, int q3,
            ChemicalElement el4, int q4,
            ChemicalElement el5, int q5)
            : this(
                el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, null, 0, null, 0, null, 0, null, 0, null, 0)
        { }

        ///// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1,
        //    ChemicalElement el2, int q2,
        //    ChemicalElement el3, int q3,
        //    ChemicalElement el4, int q4,
        //    ChemicalElement el5, int q5,
        //    ChemicalElement el6, int q6)
        //    : this(
        //        el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, null, 0, null, 0, null, 0, null, 0)
        //{ }

        ///// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1,
        //    ChemicalElement el2, int q2,
        //    ChemicalElement el3, int q3,
        //    ChemicalElement el4, int q4,
        //    ChemicalElement el5, int q5,
        //    ChemicalElement el6, int q6,
        //    ChemicalElement el7, int q7)
        //    : this(
        //        el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, el7, q7, null, 0, null, 0, null, 0)
        //{ }

        ///// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1,
        //    ChemicalElement el2, int q2,
        //    ChemicalElement el3, int q3,
        //    ChemicalElement el4, int q4,
        //    ChemicalElement el5, int q5,
        //    ChemicalElement el6, int q6,
        //    ChemicalElement el7, int q7,
        //    ChemicalElement el8, int q8)
        //    : this(
        //        el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, el7, q7, el8, q8, null, 0, null, 0)
        //{ }

        ///// <summary>Construct a chemical compound containing the specified 9 elements with corresponding stoichiometric numbers.
        ///// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        ///// st. number, second element, second number, etc.</para>
        ///// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        //public ChemicalCompound(
        //    ChemicalElement el1, int q1, 
        //    ChemicalElement el2, int q2,
        //    ChemicalElement el3, int q3,
        //    ChemicalElement el4, int q4,
        //    ChemicalElement el5, int q5,
        //    ChemicalElement el6, int q6,
        //    ChemicalElement el7, int q7,
        //    ChemicalElement el8, int q8,
        //    ChemicalElement el9, int q9): this(
        //    el1, q1, el2, q2, el3, q3, el4, q4, el5, q5, el6, q6, el7, q7, el8, q8, el9, q9, null, 0)
        //{ }

        /// <summary>Construct a chemical compound containing the specified 10 elements with corresponding stoichiometric numbers.
        /// <para>Elements and corresponding stoichiometric numbers are specified in alternating order - first element, first
        /// st. number, second element, second number, etc.</para>
        /// <para>Only elements that are defined and have stoichiometric numbers greater thatn 0 are added.</para></summary>
        public ChemicalCompound(
            ChemicalElement el1, int q1, 
            ChemicalElement el2, int q2,
            ChemicalElement el3, int q3,
            ChemicalElement el4, int q4,
            ChemicalElement el5, int q5,
            ChemicalElement el6, int q6,
            ChemicalElement el7, int q7,
            ChemicalElement el8, int q8,
            ChemicalElement el9, int q9,
            ChemicalElement el10, int q10)
        {
            if (el1!= null && q1 > 0)
                AddElement(new ChemicalElementQuantity(q1, el1));
            if (el2!=null && q2 > 0)
                AddElement(new ChemicalElementQuantity(q2, el2));
            if (el3 != null && q3 > 0)
                AddElement(new ChemicalElementQuantity(q3, el3));
            if (el4 != null && q4 > 0)
                AddElement(new ChemicalElementQuantity(q4, el4));
            if (el5 != null && q5 > 0)
                AddElement(new ChemicalElementQuantity(q5, el5));
            if (el6 != null && q6 > 0)
                AddElement(new ChemicalElementQuantity(q6, el6));
            if (el7 != null && q7 > 0)
                AddElement(new ChemicalElementQuantity(q7, el7));
            if (el8 != null && q8 > 0)
                AddElement(new ChemicalElementQuantity(q8, el8));
            if (el9 != null && q9 > 0)
                AddElement(new ChemicalElementQuantity(q9, el9));
            if (el10 != null && q10 > 0)
                AddElement(new ChemicalElementQuantity(q10, el10));
        }

        #endregion ConstructionHardCoded


        /// <summary>List of chemical elements with stoichiometric quantities that form the chemical compound.
        /// <para>Order of elements matters as it is used for printing the compound's molecular formula.</para></summary>
        protected List<ChemicalElementQuantity> _elements = new List<ChemicalElementQuantity>();

        /// <summary>Gets the number of elements in the chemical compound.</summary>
        public int NumElements
        {
            get { return _elements.Count; }
        }

        /// <summary>Returns an array of chemical elements with stoichiometric numbers that form the current chemical compound.</summary>
        public ChemicalElementQuantity[] GetElements()
        {
            return _elements.ToArray();
        }

        /// <summary>Clears the list of elements with stoichiometric quantities that form the compound.</summary>
        public void ClearElements()
        {
            _elements.Clear();
        }

        /// <summary>Adds the specified element with stoichiometric quantity to the list defining the compound.</summary>
        /// <param name="el">Element to be added.</param>
        public void AddElement(ChemicalElementQuantity el)
        {
            if (el != null)
                _elements.Add(el);
        }

        /// <summary>Adds the specified elements with stoichiometric quantities to the list defining the compound.</summary>
        /// <param name="el">Elements to be added.</param>
        public void AddElements(params ChemicalElementQuantity[] chemicalElements)
        {
            if (chemicalElements != null)
            {
                for (int i = 0; i < chemicalElements.Length; ++i)
                {
                    ChemicalElementQuantity el = chemicalElements[i];
                    _elements.Add(el);
                }
            }
        }

        /// <summary>Removes the specified element with stoichiometric quantity to the list defining the compound.</summary>
        /// <param name="el">Element to be removed.</param>
        public void RemoveElement(ChemicalElementQuantity el)
        {
            if (_elements.Contains(el))
                RemoveElement(el);
        }


        /// <summary>Removes the chemical element specified by the <see cref="ChemicalElements"/> enumerator from the list of elements forming the compound.</summary>
        /// <param name="whichElement">Specifies the element to be removed.</param>
        public void RemoveElement(ChemicalElements whichElement)
        {
            int eleemntIndex = GetElementIndex(whichElement);
            if (eleemntIndex >= 0)
                _elements.RemoveAt(eleemntIndex);
        }

        /// <summary>Removes the element with either the specified name or symbol from the list of elements forming the compound.</summary>
        /// <param name="elementSymbolOrName">Symbol or name (any ofthose two is valid) of the chemical element to be removed.</param>
        public void RemoveElement(string elementSymbolOrName)
        {
            int eleemntIndex = GetElementIndex(elementSymbolOrName);
            if (eleemntIndex >= 0)
                _elements.RemoveAt(eleemntIndex);
        }

        /// <summary>Returns index of the specified element (defined by the <see cref="ChemicalElements"/> enumerator)
        /// on the list of elements forming the chemical compound, or a negative number if there is no matching element on the list.</summary>
        /// <param name="whichElement">Enumerator of type <see cref="ChemicalElements"/> that specifies the element whose index is returned.</param>
        /// <returns>Index of the matching element, or a negative number if there is no matching element.</returns>
        public int GetElementIndex(ChemicalElements whichElement)
        {
            for (int i = 0; i < _elements.Count; ++i)
            {
                ChemicalElementQuantity el = _elements[i];
                if (el != null)
                    if (el.Element == whichElement)
                        return i;
            }
            return -1;
        }

        /// <summary>Returns index of the element with the specified name or symbol on the list of elements
        /// forming the chemical compound, or a negative number if there is no matching element on the list.</summary>
        /// <param name="elementSymbolOrName">Either symbol of name of the element whose index is returned (both are acceptable).</param>
        /// <returns>Index of the matching element, or a negative number if there is no matching element.</returns>
        public int GetElementIndex(string elementSymbolOrName)
        {
            if (string.IsNullOrEmpty(elementSymbolOrName))
                return -1;
            for (int i = 0; i < _elements.Count; ++i)
            {
                ChemicalElementQuantity el = _elements[i];
                if (el != null)
                    if (el.Symbol == elementSymbolOrName || el.Name == elementSymbolOrName)
                        return i;
            }
            return -1;
        }

        /// <summary>Returns the chemical element quantity from the list of elements that form the compound 
        /// that match the specified chemical element enumerator of type <see cref="ChemicalElements"/>, or null 
        /// if there is no matchig element.</summary>
        /// <param name="whichElement">Enumerator of type <see cref="ChemicalElements"/> that defines which element 
        /// with stoichiometric quantity is to be returned.</param>
        /// <returns>Matching element from the list of elements that form the compound, or null if there is no such element in the compound.</returns>
        public ChemicalElementQuantity GetElement(ChemicalElements whichElement)
        {
            int index = GetElementIndex(whichElement);
            if (index >= 0)
                return _elements[index];
            else
                return null;
        }

        /// <summary>Returns the chemical element with stoichiometric quantity from the list of elements that form the compound 
        /// that match the specified element symbol or name, or null if there is no matchig element.</summary>
        /// <param name="whichElement">Symbol or name of the element to be returned (both are accepted). 
        /// <para>If null or empty string then null is returned.</para></param>
        /// <returns>Matching element from the list of elements that form the compound, or null if there is no such element in the compound.</returns>
        public ChemicalElementQuantity GetElement(string elementSymbolOrName)
        {
            int index = GetElementIndex(elementSymbolOrName);
            if (index >= 0)
                return _elements[index];
            else
                return null;
        }


    } // class ChemicalCompound

}