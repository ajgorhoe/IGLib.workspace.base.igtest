// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Physics;



/// <summary>DTO (Data Transfer Objects) for chemical element properties (the <see cref="ChemicalElementProperties"/> class).
/// Used to store a state of a chemical element properties object.</summary>
/// <typeparam name="ChemicalElementType">Type parameter specifying the specific chemical element type for which concrete DTO
/// is designed.</typeparam>
/// $A Igor Oct09;
public class ChemicalElementPropertiesDto : SerializationDtoBase<ChemicalElementProperties, ChemicalElementProperties>
{

    #region Construction

    /// <summary>Default constructor, sets IsNull to true.</summary>
    public ChemicalElementPropertiesDto()
        : base()
    {
        this.SetNull(true);
    }

    /// <summary>Constructor, prepares the current DTO for storing a specific chemical element properties object
    /// (type <see cref="ChemicalElementProperties"/>).</summary>
    /// <param name="length">Element properties object whose data is stored in the current DTO.</param>
    public ChemicalElementPropertiesDto(ChemicalElementProperties elementProperties)
        : this()
    {
        this.SetNull(false);
        CopyFrom(elementProperties);
    }


    #endregion Construction

    #region Data

    string Symbol;

    public int AtomicNumber;

    public double AtomicWeight;

    #endregion Data


    #region Operation


    /// <summary>Creates and returns a new chemical element of the appropriate type.</summary>
    public override ChemicalElementProperties CreateObject()
    {
        return new ChemicalElementProperties(Symbol, AtomicNumber, AtomicWeight);
    }

    /// <summary>Copies data to the current DTO from a chemical element properties object.</summary>
    /// <param name="elementProperties">Chemical element object from which data is copied.</param>
    protected override void CopyFromPlain(ChemicalElementProperties elementProperties)
    {
        if (elementProperties == null)
        {
            this.AtomicNumber = 0;
            this.Symbol = null;
            SetNull(true);
        }
        else
        {
            SetNull(false);
            this.Symbol = elementProperties.Symbol;
            this.AtomicNumber = elementProperties.AtomicNumber;
            this.AtomicWeight = elementProperties.AtomicWeight;
        }
    }

    /// <summary>Copies data from the current DTO to a chemical element properties object.</summary>
    /// <param name="elementProperties">Chemical element properties object that data is copied to.</param>
    protected override void CopyToPlain(ref ChemicalElementProperties elementProperties)
    {
        if (GetNull())
            elementProperties = null;
        else
        {
            if (elementProperties == null)
                elementProperties = CreateObject();
            elementProperties.Symbol = this.Symbol;
            elementProperties.AtomicNumber = this.AtomicNumber;
            elementProperties.AtomicWeight = this.AtomicWeight;
        }
    }

    #endregion Operation


}  // abstract class ChemicalElementPropertiesDto



