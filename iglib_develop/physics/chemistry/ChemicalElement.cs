// Copyright (c) Igor Grešovnik (2008/present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Physics;



namespace IG.Physics
{

    /// <summary>Chemical element with stoichiometric quantity.</summary>
    /// $A Igor Nov08;
    public class ChemicalElementQuantity : ChemicalElement
    {

        #region Construction

        /// <summary>Constructs a chemical element quaintity object that is a deep copy of the specified chemical
        /// element (can also be of type <see cref="ChemicalElementQuantity"/> since it inherits form chemical element) 
        /// and has the specified quantity.</summary>
        /// <param name="element">Chemical elmment from which element quantity is constructed by deep copying.</param>
        /// <param name="quantity">Quantity of the element.</param>
        public ChemicalElementQuantity(double quantity, ChemicalElement element) :
            base(element)
        {
            this.Quantity = quantity;
        }

        /// <summary>Constructs a chemical element quaintity object that is a deep copy of the specified existing chemical
        /// element quantity.</summary>
        /// <param name="elementQuantity">Chemical elmment quantity from which element quantity is constructed by deep copying.</param>
        public ChemicalElementQuantity(ChemicalElementQuantity elementQuantity) :
            base(elementQuantity)
        {
            this.Quantity = elementQuantity.Quantity;
        }

        /// <summary>Constructs a new chemical element element enumerator.</summary>
        /// <param name="quantity">Stoichiometric quantity of the element.</param>
        /// <param name="whichElement">Enum of type <see cref="ChemicalElements"/> that defines which chemical element is created.</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        /// <param name="properties">Other properties of this element (optional).</param>
        public ChemicalElementQuantity(double quantity, ChemicalElements whichElement, double atomicWeight, ChemicalElementProperties properties):
            base(whichElement, atomicWeight, properties)
        {
            this.Quantity = quantity;
        }

        /// <summary>Constructs a new chemical element element enumerator.</summary>
        /// <param name="quantity">Stoichiometric quantity of the element.</param>
        /// <param name="whichElement">Enum of type <see cref="ChemicalElements"/> that defines which chemical element is created.</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        public ChemicalElementQuantity(double quantity, ChemicalElements whichElement, double atomicWeight)
            : this(quantity, whichElement, atomicWeight, null /* properties */)
        {  }

        /// <summary>Constructs a new chemical element element enumerator, with atomic weight set to 0.</summary>
        /// <param name="quantity">Stoichiometric quantity of the element.</param>
        /// <param name="whichElement">Enum of type <see cref="ChemicalElements"/> that defines which chemical element is created.</param>
        public ChemicalElementQuantity(double quantity, ChemicalElements whichElement)
            : this(quantity, whichElement, 0 /* atomicWeight */, null /* properties */)
        {  }
        
        /// <summary>Constructs a new chemical element with specified element symbol.</summary>
        /// <param name="quantity">Stoichiometric quantity of the element.</param>
        /// <param name="symbol">Symbol of the chemical element. Enum of type <see cref="ChemicalElements"/> is
        /// obtained by parsing this symbol, whichis case sensitive (If there is no such enumeration value, exception is thrown).</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        /// <param name="properties">Other properties of this element (optional).</param>
        public ChemicalElementQuantity(double quantity, string symbol, double atomicWeight, ChemicalElementProperties properties):
            base(symbol, atomicWeight, properties)
        {
            this.Quantity = quantity;
        }
                
        /// <summary>Constructs a new chemical element with specified element symbol.</summary>
        /// <param name="quantity">Stoichiometric quantity of the element.</param>
        /// <param name="symbol">Symbol of the chemical element. Enum of type <see cref="ChemicalElements"/> is
        /// obtained by parsing this symbol, whichis case sensitive (If there is no such enumeration value, exception is thrown).</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        public ChemicalElementQuantity(double quantity, string symbol, double atomicWeight) :
            this(quantity, symbol, atomicWeight, null /* properties */)
        {  }

        /// <summary>Constructs a new chemical element with specified element symbol, with atomic weight set to 0.</summary>
        /// <param name="quantity">Stoichiometric quantity of the element.</param>
        /// <param name="symbol">Symbol of the chemical element. Enum of type <see cref="ChemicalElements"/> is
        /// obtained by parsing this symbol, whichis case sensitive (If there is no such enumeration value, exception is thrown).</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        public ChemicalElementQuantity(double quantity, string symbol) :
            this(quantity, symbol, 0.0 /* atomicWeight */, null /* properties */)
        {  }

        #endregion Construction

        /// <summary>Stoichiometric quantity of the element.</summary>
        public double Quantity;


        #region Operation

        /// <summary>Returns string representation of the current chemical element with quantity, which is just the quantity and element symbol.
        /// <para>Warning:</para>
        /// <para>Function of this method can change in the future!</para>
        /// <para>In orter to make sure that only quantity and symbol is written, use the <see cref="TostringShort"/> method.</para></summary>
        public override string ToString()
        {
            return Symbol + Quantity.ToString();
        }

        /// <summary>Returns string representation of the current chemical element, which is just the element symbol.</summary>
        public override string TostringShort()
        {
            return Symbol + Quantity.ToString();
        }

        /// <summary>Returns longer string representation of the current themical element, which includes <see cref="Properties"/>.</summary>
        public override string ToStringLong()
        {
            return Quantity.ToString() + "*" + base.ToString();
        }

        #endregion Operation 

    }  // class ChemicalElementQuantity



    /// <summary>Represents a sigle chemical element with its main data.</summary>
    /// $A Igor Oct08;
    public class ChemicalElement
    {

        #region Construction

        private ChemicalElement()  // prevent default constructor
        {
        }


        /// <summary>Constructs a new chemical element element enumerator.</summary>
        /// <param name="whichElement">Enum of type <see cref="ChemicalElements"/> that defines which chemical element is created.</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        /// <param name="properties">Other properties of this element (optional).</param>
        public ChemicalElement(ChemicalElements whichElement, double atomicWeight, ChemicalElementProperties properties)
        {
            this.Element = whichElement;
            this.Symbol = whichElement.ToString();
            this.AtomicNumber = (int) whichElement;
            this.AtomicWeight = atomicWeight;
        }


        /// <summary>Constructs a new chemical element element enumerator.</summary>
        /// <param name="whichElement">Enum of type <see cref="ChemicalElements"/> that defines which chemical element is created.</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        public ChemicalElement(ChemicalElements whichElement, double atomicWeight)
            : this(whichElement, atomicWeight, null /* properties */)
        {  }

        /// <summary>Constructs a new chemical element element enumerator, with atomic weight set to 0.</summary>
        /// <param name="whichElement">Enum of type <see cref="ChemicalElements"/> that defines which chemical element is created.</param>
        public ChemicalElement(ChemicalElements whichElement)
            : this(whichElement, 0 /* atomicWeight */, null /* properties */)
        {  }

        
        /// <summary>Constructs a new chemical element with specified element symbol.</summary>
        /// <param name="symbol">Symbol of the chemical element. Enum of type <see cref="ChemicalElements"/> is
        /// obtained by parsing this symbol, whichis case sensitive (If there is no such enumeration value, exception is thrown).</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        /// <param name="properties">Other properties of this element (optional).</param>
        public ChemicalElement(string symbol, double atomicWeight, ChemicalElementProperties properties)
        {
            ChemicalElements whichElement;
            // whichElement = (ChemicalElements) Enum.Parse(typeof(ChemicalElements), symbol);
            bool successful = Enum.TryParse<ChemicalElements>(symbol, false /* ignoreCase */, out whichElement);
            if (!successful)
                throw new ArgumentException("Symbol \"" + symbol + "\" is not a known symbol of chemical element.");
            this.Element = whichElement;
            this.Symbol = symbol;
            this.AtomicNumber = (int) whichElement;
            this.AtomicWeight = atomicWeight;
        }

                
        /// <summary>Constructs a new chemical element with specified element symbol.</summary>
        /// <param name="symbol">Symbol of the chemical element. Enum of type <see cref="ChemicalElements"/> is
        /// obtained by parsing this symbol, whichis case sensitive (If there is no such enumeration value, exception is thrown).</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        public ChemicalElement(string symbol, double atomicWeight):
            this(symbol, atomicWeight, null /* properties */)
        {  }

        /// <summary>Constructs a new chemical element with specified element symbol, with atomic weight set to 0.</summary>
        /// <param name="symbol">Symbol of the chemical element. Enum of type <see cref="ChemicalElements"/> is
        /// obtained by parsing this symbol, whichis case sensitive (If there is no such enumeration value, exception is thrown).</param>
        /// <param name="atomicWeight">Atomic weight of the element.</param>
        public ChemicalElement(string symbol):
            this(symbol, 0.0 /* atomicWeight */, null /* properties */)
        {  }

        /// <summary>Constructs a new chemical element object that is a deep copy of the specified chemical element.</summary>
        /// <param name="element">Existing chemical element that is copied.</param>
        public ChemicalElement(ChemicalElement element)
        {
            if (element == null)
                throw new ArgumentException("Chemical element to be copied is not specified.");
            CopyPlain(element, this);
        }

        #endregion Construction

        #region Data

        /// <summary>Enum of type <see cref="ChemicalElements"/> specifying which chemical element this is.</summary>
        public ChemicalElements Element;

        /// <summary>Chemical element's symbol.</summary>
        public string Symbol;

        private string _name;

        /// <summary>Long name of the chemical element. 
        /// <para>If not specified then element symbol is returned by getter.</para></summary>
        public string Name
        {
            get 
            {
                if (string.IsNullOrEmpty(_name))
                    return Symbol;
                else
                    return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>Atomic number of the chemical element.</summary>
        public int AtomicNumber;

        /// <summary>Standard atomic weight in atomic mass units.</summary>
        public double AtomicWeight;

        public ChemicalElementProperties Properties;

        #endregion Data

        #region Operation

        /// <summary>Returns string representation of the current chemical element, which is just the element symbol.
        /// <para>Warning:</para>
        /// <para>Function of this method can change in the future!</para>
        /// <para>In orter to make sure that only a symbol is written, use the <see cref="TostringShort"/> method.</para></summary>
        public override string ToString()
        {
            return Symbol;
        }

        /// <summary>Returns string representation of the current chemical element, which is just the element symbol.</summary>
        public virtual string TostringShort()
        {
            return Symbol;
        }

        /// <summary>Returns longer string representation of the current themical element, which includes <see cref="Properties"/>.</summary>
        public virtual string ToStringLong()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Chemical element " + Symbol + ":");
            sb.AppendLine("  Name: " + Name);
            sb.AppendLine("  Atomic number: " + AtomicNumber);
            sb.AppendLine("  Atomic weight: " + AtomicWeight);
            if (Properties!=null)
                sb.AppendLine(" " + Properties.ToString());
            return sb.ToString();
        }

        #endregion Operation 

        #region Static

        /// <summary>Deep copies the specified chemical element to another chemical element object.</summary>
        /// <param name="original">Original chemical element that is copied to another object.</param>
        /// <param name="result">Object to which the original element is copied.</param>
        public static void CopyPlain(ChemicalElement original, ChemicalElement result)
        {
            result.Symbol = original.Symbol;
            result.Name = original.Name;
            result.AtomicNumber = original.AtomicNumber;
            result.AtomicWeight = original.AtomicWeight;
            if (original.Properties == null)
                result.Properties = null;
            else
            {
                result.Properties = new ChemicalElementProperties(original.Properties);
            }
        }

        #endregion Static

    }  // public class ChemicalElement

}

