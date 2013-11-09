// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Physics;

namespace IG.Physics
{

/// <summary>Base class for various DTOs (Data Transfer Objects) for chemical elements and derived classes.
    /// Used to store a state of a chemical element.</summary>
    /// <typeparam name="ChemicalElementType">Type parameter specifying the specific chemical element type for which concrete DTO
    /// is designed.</typeparam>
    /// $A Igor Oct09;
    public abstract class ChemicalElementDtoBase<ChemicalElementType> : SerializationDtoBase<ChemicalElementType, ChemicalElement>
        where ChemicalElementType : ChemicalElement
    {

        #region Construction

        /// <summary>Default constructor, sets IsNull to true.</summary>
        public ChemicalElementDtoBase()
            : base()
        { this.SetNull(true); }

        /// <summary>Constructor, prepares the current DTO for storing a specific chemical element.</summary>
        /// <param name="length">Element whose data is stored in the current DTO.</param>
        public ChemicalElementDtoBase(ChemicalElementType element)
            : this()
        {
            this.SetNull(false);
            CopyFrom(element);
        }

        #endregion Construction

        #region Data

        /// <summary>Enum of type <see cref="ChemicalElements"/> specifying which chemical element this is.</summary>
        public ChemicalElements Element;

        /// <summary>Chemical element's symbol.</summary>
        public string Symbol;

        /// <summary>Long name of the chemical element.</summary>
        public string Name;

        /// <summary>Atomic number of the chemical element.</summary>
        public int AtomicNumber;

        /// <summary>Standard atomic weight in atomic mass units.</summary>
        public double AtomicWeight;

        public ChemicalElementPropertiesDto Properties;

        #endregion Data


        #region Operation

        /// <summary>Creates and returns a new chemical element.</summary>
        public abstract ChemicalElementType CreateElement();

        /// <summary>Creates and returns a new chemical element of the appropriate type.</summary>
        public override ChemicalElementType CreateObject()
        {
            return CreateElement();
        }

        /// <summary>Copies data to the current DTO from a chemical element object.</summary>
        /// <param name="element">Chemical element object from which data is copied.</param>
        protected override void CopyFromPlain(ChemicalElement element)
        {
            if (element == null)
            {
                this.Element = ChemicalElements.Unknown;
                SetNull(true);
            } else
            {
                SetNull(false);
                this.Element = element.Element;
                this.Symbol = element.Symbol;
                this.Name = element.Name;
                this.AtomicNumber = element.AtomicNumber;
                this.AtomicWeight = element.AtomicWeight;
                if (element.Properties == null)
                    this.Properties = null;
                else
                {
                    this.Properties = new ChemicalElementPropertiesDto();
                    this.Properties.CopyFrom(element.Properties);
                }
            }
        }

        /// <summary>Copies data from the current DTO to a chemical element object.</summary>
        /// <param name="element">Chemical element object that data is copied to.</param>
        protected override void CopyToPlain(ref ChemicalElement element)
        {
            if (GetNull())
                element = null;
            else
            {
                if (element == null)
                    element = CreateObject();
                element.Element = this.Element;
                element.Symbol = this.Symbol;
                element.Name = this.Name;
                element.AtomicNumber = this.AtomicNumber;
                element.AtomicWeight = this.AtomicWeight;
                if (this.Properties == null)
                {
                    element.Properties = null;
                } else
                {
                    this.Properties.CopyTo(ref element.Properties);
                }
            }
        }

        #endregion Operation


    }  // abstract class ChemicalElementDtoBase<ChemicalElementType>


    /// <summary>DTO (data transfer object) for chemical element (<see cref="ChemicalElement"/> class).</summary>
    /// $A Igor Oct09;
    public class ChemicalElementDto : ChemicalElementDtoBase<ChemicalElement>
    {

        #region Construction

        /// <summary>Creates a DTO for storing state of a chemical element object.</summary>
        public ChemicalElementDto()
            : base()
        { }

        /// <summary>Creates a DTO for storing a chemical element.</summary>
        /// <param name="element">Chemical element that is used as basis for data.</param>
        public ChemicalElementDto(ChemicalElement element)
            : base(element)
        {  }

        #endregion Construction

        /// <summary>Creates and returns a new vector cast to the interface type IVector.</summary>
        /// <param name="length">Vector dimension.</param>
        public override ChemicalElement CreateElement()
        {
            return new ChemicalElement(Element, AtomicWeight);
        }

    } // class ChemicalElementDto


    /// <summary>DTO (data transfer object) for chemical element (<see cref="ChemicalElement"/> class).</summary>
    /// $A Igor Oct09;
    public class ChemicalElementQuantityDto : ChemicalElementDtoBase<ChemicalElementQuantity>
    {

        #region Construction

        /// <summary>Creates a DTO for storing state of a chemical element with quantity.</summary>
        public ChemicalElementQuantityDto()
            : base()
        { }

        /// <summary>Creates a DTO for storing a chemical element with quantity.</summary>
        /// <param name="element">Chemical element with quantity that is used as basis for data.</param>
        public ChemicalElementQuantityDto(ChemicalElementQuantity element)
            : base(element)
        { this.Quantity = element.Quantity; }

        #endregion Construction

        #region Data

        /// <summary>Quantty of chemical element.</summary>
        double Quantity;

        #endregion Data

        #region Operation 

        /// <summary>Creates and returns a new chemical element with quantity.</summary>
        public override ChemicalElementQuantity CreateElement()
        {
            return new ChemicalElementQuantity(Quantity, Element, AtomicWeight);
        }

        /// <summary>Copies data to the current DTO from a chemical element with quantity object.</summary>
        /// <param name="element">Chemical element with quabntity object from which data is copied.</param>
        protected override void CopyFromPlain(ChemicalElement element)
        {
            base.CopyFromPlain(element);
            if (element != null)
            {
                ChemicalElementQuantity elementThis = element as ChemicalElementQuantity;
                if (elementThis!=null)
                    this.Quantity = elementThis.Quantity;
            }
        }

        /// <summary>Copies data from the current DTO to a chemical element with quantity object.</summary>
        /// <param name="element">Chemical element with quantity object that data is copied to.</param>
        protected override void CopyToPlain(ref ChemicalElement element)
        {
            base.CopyToPlain(ref element);
            if (element != null)
            {
                ChemicalElementQuantity elementThis = element as ChemicalElementQuantity;
                if (elementThis!=null)
                    elementThis.Quantity = this.Quantity;
            }
        }

        #endregion Operation 

    } // class ChemicalElementQuantityDto

}


