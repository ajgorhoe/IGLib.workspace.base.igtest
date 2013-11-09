// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Physics;



/// <summary>DTO (Data Transfer Objects) for chemical element properties (the <see cref="ChemicalCompound"/> class).
/// Used to store a state of a chemical element properties object.</summary>
/// <typeparam name="ChemicalCompoundType">Type parameter specifying the specific chemical element type for which concrete DTO
/// is designed.</typeparam>
/// $A Igor Nov09;
public class ChemicalCompoundDto : SerializationDtoBase<ChemicalCompound, ChemicalCompound>
{

    #region Construction

    /// <summary>Default constructor, sets IsNull to true.</summary>
    public ChemicalCompoundDto()
        : base()
    {
        this.SetNull(true);
    }

    /// <summary>Constructor, prepares the current DTO for storing a specific chemical element properties object
    /// (type <see cref="ChemicalCompound"/>).</summary>
    /// <param name="length">Element properties object whose data is stored in the current DTO.</param>
    public ChemicalCompoundDto(ChemicalCompound elementProperties)
        : this()
    {
        this.SetNull(false);
        CopyFrom(elementProperties);
    }


    #endregion Construction

    #region Data

    int NumElements;

    ChemicalElementQuantityDto[] Elements;

    #endregion Data


    #region Operation


    /// <summary>Creates and returns a new chemical compound.</summary>
    public override ChemicalCompound CreateObject()
    {
        return new ChemicalCompound();
    }

    /// <summary>Copies data to the current DTO from a chemical compound object.</summary>
    /// <param name="compound">Chemical compound object from which data is copied.</param>
    protected override void CopyFromPlain(ChemicalCompound compound)
    {
        if (compound == null)
        {
            NumElements = 0;
            Elements = null;
            SetNull(true);
        }
        else
        {
            SetNull(false);
            this.NumElements = compound.NumElements;
            ChemicalElementQuantity[] elements = compound.GetElements();
            if (elements==null)
                this.Elements = null;
            else
            {
                this.Elements = new ChemicalElementQuantityDto[elements.Length];
                for (int i = 0; i < elements.Length; ++i)
                {
                    this.Elements[i] = new ChemicalElementQuantityDto(elements[i]);
                }
            }
        }
    }

    /// <summary>Copies data from the current DTO to a chemical compound object.</summary>
    /// <param name="compound">Chemical compound object that data is copied to.</param>
    protected override void CopyToPlain(ref ChemicalCompound compound)
    {
        if (GetNull())
            compound = null;
        else
        {
            if (compound == null)
                compound = CreateObject();
            if (Elements != null)
            {
                for (int i = 0; i < Elements.Length; ++i)
                {
                    ChemicalElementQuantityDto dto = Elements[i];
                    if (dto == null)
                        ; // compound.AddElements(null);
                    else
                    {
                        ChemicalElementQuantity element = dto.CreateObject();
                        dto.CopyTo(ref element);
                        compound.AddElements(element);
                    }
                }
            }
        }
    }

    #endregion Operation


}  // abstract class ChemicalCompoundDto



