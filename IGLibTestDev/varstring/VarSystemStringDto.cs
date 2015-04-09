using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Lib
{



    /// <summary>Base class for various DTOs (Data Transfer Objects) for systems of string-valued variables.
    /// Used to store a state of a variable system or to store a set of variables to be imported into a system.</summary>
    /// <typeparam name="VariableSystemType">Type parameter specifying the specific variable system type for which concrete DTO
    /// is designed.</typeparam>
    /// $A Igor Jun09;
    public abstract class StringVariableSystemDtoBase<VariableSystemType> : SerializationDtoBase<VariableSystemType, StringVariableSystem>
        where VariableSystemType : StringVariableSystem
    {

        #region Construction

        /// <summary>Default constructor, sets IsNull to true.</summary>
        public StringVariableSystemDtoBase()
            : base()
        { }

        #endregion Construction


        #region Data

        /// <summary>Variable definitions.
        /// <para>Each element must be an array with two elements (variable name and variable value).</para></summary>
        public string[][] Variables;

        public string[] VariablesToRemove;

        public bool SetNullOnImport = true;

        #endregion Data


        #region Operation

        /// <summary>Creates and returns a new vector of the specified dimension.</summary>
        /// <param name="length">Vector dimension.</param>
        public abstract VariableSystemType CreateVariableSystem();

        /// <summary>Creates and returns a new vector of the specified type and dimension.</summary>
        public override VariableSystemType CreateObject()
        {
            return CreateVariableSystem();
        }

        /// <summary>Copies data to the current DTO from a string variable system object.</summary>
        /// <param name="variableSystem">Variable system object from which data is copied.</param>
        protected override void CopyFromPlain(StringVariableSystem variableSystem)
        {
            if (variableSystem == null)
            {
                Variables = null;
                SetNull(true);
            } else
            {
                this.SetNull(false);
                SetNullOnImport = variableSystem.SetNullOnImport;
                Variables = variableSystem.ToArray();
            }
        }

        /// <summary>Copies data from the current DTO to a variable system object.</summary>
        /// <param name="variableSystem">Variable system object that data is copied to.</param>
        protected override void CopyToPlain(ref StringVariableSystem variableSystem)
        {
            if (GetNull())
                variableSystem = null;
            else
            {
                if (variableSystem == null)
                    variableSystem = CreateObject();
                variableSystem.SetNullOnImport = SetNullOnImport;
                variableSystem.ImportVariables(Variables);
                variableSystem.RemoveVariables(VariablesToRemove);
            }
        }

        #endregion Operation


        #region Misc

        /// <summary>Creates and returns a string representation of the curren variable system DTO.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Variable system DTO, tpe = " + typeof(VariableSystemType).ToString() + ": ");
            sb.AppendLine("  Variables: ");
            for (int i = 0; i < Variables.Length; ++i)
            {
                if (Variables[i]!=null)
                    if (Variables[i].Length>=2)
                    {
                        sb.AppendLine(String.Format("{0,10}:  {1,-20}", Variables[i][0], Variables[i][1]));
                    }
            }
            // sb.AppendLine();
            return sb.ToString();
        }

        #endregion Misc

    }  // abstract class StringVariableSystemDtoBase<SystemType>

    
    /// <summary>Generic class for various DTOs (Data Transfer Objects) for systems of string-valued variables.
    /// Used to store a state of a variable system or to store a set of variables to be imported into a system.</summary>
    /// <typeparam name="VariableSystemType">Type parameter specifying the specific variable system type for which concrete DTO
    /// is designed.</typeparam>
    /// $A Igor Jun09;
    public class StringVariableSystemDto<VariableSystemType> : StringVariableSystemDtoBase<VariableSystemType>
        where VariableSystemType : StringVariableSystem, new()
    {


        /// <summary>Creates and returns a new variable system of type <see cref="StringSettings"/></summary>
        public override VariableSystemType CreateVariableSystem()
        {
            return new VariableSystemType();
        }


    }

    /// <summary>DTO (data transfer object) for String variable systems.</summary>
    /// $A Igor Jun09;
    public class StringVariableSystemDto : StringVariableSystemDtoBase<StringVariableSystem>
    {

        #region Construction

        /// <summary>Creates a DTO for storing state of a vector object of any vector type</summary>
        public StringVariableSystemDto()
            : base()
        { }

        #endregion Construction

        /// <summary>Creates and returns a new variable system of type <see cref="StringVariableSystem"/></summary>
        public override StringVariableSystem CreateVariableSystem()
        {
            return new StringVariableSystem();
        }


    } // class StringVariableSystemDtoBase


    /// <summary>Data Transfer Object (DTO) for vectors of type IG.Num.Vector.
    /// Used to store, transfer, serialize and deserialize objects of type Vector.</summary>
    /// $A Igor Aug09;
    public class StringSettingsDto : StringVariableSystemDtoBase<StringSettings>
    {

        #region Construction

        /// <summary>Creates a DTO for storing state of a vector object of any vector type</summary>
        public StringSettingsDto()
            : base()
        { }


        #endregion Construction


        /// <summary>Creates and returns a new variable system of type <see cref="StringSettings"/></summary>
        public override StringSettings CreateVariableSystem()
        {
            return new StringSettings();
        }

    }  // class StringSettingsDto





}
