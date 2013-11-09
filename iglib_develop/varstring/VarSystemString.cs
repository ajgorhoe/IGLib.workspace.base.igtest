using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Lib
{
    
    /// <summary>A lightweight system of string-valued variables, with some support to type conversion.</summary>
    public class StringVariableSystem: ILockable
    {

        #region Construction

        /// <summary>Constructs a new system of string variables.</summary>
        public StringVariableSystem()
        {
        }

        #endregion Construction


        #region ILockable

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ILockable


        #region Settings

        bool _setNullOnImport = false;

        /// <summary>Flag indicating whether null variable values are also set when importing variable values.
        /// If false then, if some imported variable has null value, this variable will not be set on the
        /// current system of variables. This means that the value of eventually existent variable will remain
        /// unchanged if variables are imported from another system where this variable is defined but has null 
        /// value.</summary>
        /// <remarks>Default value is false. This behavior can be useful e.g. when importing settings from a file.
        /// Many times we don't want to import variables with null value because somebody might have set values to 
        /// null instead of completely removing those variables he or she didn't want to change. This might not be
        /// the preferred behavior when null-valued variables actually serve some purpose and it is important 
        /// that we have possibility of defining null values for variables e.g. in a setting file.</remarks>
        public bool SetNullOnImport
        {
            get { lock (Lock) { return _setNullOnImport; } }
            set { lock (Lock) { _setNullOnImport = value; } }
        }

        #endregion Settings


        #region Data

        protected SortedList<string, string> _variables = new SortedList<string, string>();

        protected virtual SortedList<string, string> Variables
        {
            get { return _variables; }
        }

        #endregion Data


        #region Operation.Basic

        /// <summary>Gets or sets value of the specified variable.
        /// <para>Getter returnd value of the variable or null if it is not defined.</para>
        /// <para>Setter overwrites existing value or creates a variable if it is not yet defined.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual string this[string varName]
        {
            get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(varName))
                    {
                        throw new ArgumentException("Name of the variable whose value is retrieved is not specified (null or empty string).");
                    }
                    string value;
                    this.Variables.TryGetValue(varName, out value);
                    return value;
                }
            }
            set
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(varName))
                    {
                        throw new ArgumentException("Name of the variable to be set is not specified (null or empty string).");
                    }
                    Variables[varName] = value;
                }
            }
        }

        /// <summary>Returns value of the specified variable, or null if it is not defined.</summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual string GetVariable(string varName)
        {
            lock (Lock)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    throw new ArgumentException("Name of the variable whose value is retrieved is not specified (null or empty string).");
                }
                string varValue;
                this.Variables.TryGetValue(varName, out varValue);
                return varValue;
            }
        }

        /// <summary>Sets the specified variable to the specified value.
        /// <para>If variable does not exist then it is created, otherwise its value is overwritten.</para></summary>
        /// <param name="varName">Name of the variable to be set. Must not be null or empty string.</param>
        /// <param name="varValue">Value that is assigned to the variable.</param>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual void SetVariable(string varName, string varValue)
        {
            lock (Lock)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    throw new ArgumentException("Name of the variable to be set is not specified (null or empty string).");
                }
                Variables[varName] = varValue;
            }
        }

        /// <summary>Adds a new variable of the specified name and with the specified value (which can be null).
        /// <para>If the variable with the specified name already exists then <see cref="ArgumentException"/> exception is thrown.</para></summary>
        /// <param name="varName">Name of the variable to be added. Must not be null or empty string.</param>
        /// <param name="varValue">Value that is assigned to the variable.</param>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string,
        /// or when the variable with the specified name already exists.</exception>
        public virtual void AddNewVariable(string varName, string varValue)
        {
            lock (Lock)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    throw new ArgumentException("Name of the variable to be set is not specified (null or empty string).");
                }
                SortedList<string, string> variables = this.Variables;
                if (variables.ContainsKey(varName))
                {
                    throw new ArgumentException("Variable named \"" + varName + "\" already exists, can not be added anew.");
                }
                variables.Add(varName, varValue);
            }
        }

        /// <summary>Removes a variable with the specified name from the current variable system, and returns a flag
        /// indicating whether variable was actually removed.
        /// <para>False is returned but no exception is thrown if the specified variable is not defined.</para></summary>
        /// <param name="varName">Name of the variable to be removed. Must not be null or empty string.</param>
        /// <returns>True if a variable has been removed, false it the specified variable was not defined.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual bool RemoveVariable(string varName)
        {
            lock (Lock)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    throw new ArgumentException("Name of the variable to be removed is not specified (null or empty string).");
                }
                return this.Variables.Remove(varName);
            }
        }

        /// <summary>Removes all variables whose names are specified by the argument,
        /// and returns the number of variables that were actully been removed.</summary>
        /// <param name="varNames">Names of variables to be removed.</param>
        /// <returns>Number of variables that were actually removed.</returns>
        public virtual int RemoveVariables(params string[] varNames)
        {
            lock (Lock)
            {
                int ret = 0;
                if (varNames != null)
                {
                    foreach (string varName in varNames)
                    {
                        if (this.Variables.Remove(varName))
                            ++ret;
                    }
                }
                return ret;
            }
        }


        /// <summary>Unsets the variable with the specified name on the current variable system, and returns a flag
        /// indicating whether variable was defined.
        /// <para>If the variable is defined then it is jus tset to null and true is returned.</para>
        /// <para>False is returned but no exception is thrown if the specified variable is not defined.</para></summary>
        /// <param name="varName">Name of the variable to be removed. Must not be null or empty string.</param>
        /// <returns>True if a variable is defined (in which case it is set to null), false it the specified variable was not defined.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual bool UnSetVariable(string varName)
        {
            lock (Lock)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    throw new ArgumentException("Name of the variable to be unset is not specified (null or empty string).");
                }
                SortedList<string, string> variables = this.Variables;
                if (variables.ContainsKey(varName))
                {
                    variables[varName] = null;
                    return true;
                }
                return false;
            }
        }


        /// <summary>Returns true if the specified variable is defined, false otherwise.</summary>
        /// <param name="varName">Name of the variable whose existence is queried. Must not be null or empty string.</param>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual bool IsVariableDefined(string varName)
        {
            lock (Lock)
            {
                if (string.IsNullOrEmpty(varName))
                {
                    throw new ArgumentException("Name of the variable whose existence is checked is not specified (null or empty string).");
                }
                return Variables.ContainsKey(varName);
            }
        }

        #endregion Operation.Basic


        #region Operation.Collective

        /// <summary>Returns array representation of variables that are currently defined on the system.</summary>
        /// <returns>Array whose elements are arrays of two strings, of which the 0-th element is variable name
        /// and the 1st element is its value.</returns>
        public virtual string[][] ToArray()
        {
            lock (Lock)
            {
                SortedList<string, string> variables = this.Variables;
                string[][] ret= new string[variables.Count][];
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in variables)
                {
                    ret[i] = new string[2];
                    ret[i][0] = kvp.Key;
                    ret[i][1] = kvp.Value;
                    ++i;
                }
                return ret;
            }
        }


        /// <summary>Imports variables defined in form of two dimensional array.</summary>
        /// <param name="variables"></param>
        public virtual void ImportVariables(string[][] variables)
        {
            if (variables != null)
            {
                for (int i = 0; i < variables.Length; ++i)
                {
                    string[] var = variables[i];
                    if (var!=null)
                        if (var.Length >= 2)
                        {
                            if (var[1] != null || SetNullOnImport)
                            {

                                SetVariable(var[0], var[1]);
                            }
                        }
                }
            }
        }

        #endregion Operation.Collective


        #region Operation.TypeSupportDefault


        /// <summary>Sets the specified variable to the specified value.
        /// <para>If value is null then variable value is set to null, otherwise it is set to object's
        /// string representation that is obtained by calling its ToString() method.</para>
        /// <para>If variable does not exist then it is created, otherwise its value is overwritten.</para></summary>
        /// <param name="varName">Name of the variable to be set. Must not be null or empty string.</param>
        /// <param name="varValue">Value that is assigned to the variable. If the object is null then null is assigned,
        /// otherwise string representation of the object obtained by the ToString() method is assigned.</param>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public virtual void SetVariable(string varName, object varValue)
        {
            if (varValue == null)
            {
                SetVariable(varName, null);
            }
            else
            {
                SetVariable(varName, varValue.ToString());
            }
        }


        /// <summary>Obtains a value of an integer-typed variable of the specified name, if such a
        /// variable is defined and its value can be interpreted as an integer.
        /// <para>Returns a flag indicating whether the value has been successfully obtained.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <returns>True if the value has been successfully obtained, false if not.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public bool TryGetIntegerVariable(string varName, out int varTypedValue)
        {
            bool successfullyParsed = false;
            string varValue = this.GetVariable(varName);
            if (varValue == null)
                throw new ArgumentException("Integer-typed variable named \"" + varName + "\" is not defined.");
            successfullyParsed = int.TryParse(varValue, out varTypedValue);
            return successfullyParsed;
        }

        /// <summary>Returns the value of an integer-valued variable of the specified name.
        /// <para>Exception is thrown if the specified variable does not exist or its string value 
        /// can not be interpreted as integer.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <returns>Integar value of the specified variable.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string or
        /// when a variable with the specified name does not exist or its string value does not represent an integer.</exception>
        public int GetIntegerVariable(string varName)
        {
            int varTypedValue;
            bool successfullyParsed = false;
            string varValue = this.GetVariable(varName);
            if (varValue == null)
                throw new ArgumentException("Integer-typed variable named \"" + varName + "\" is not defined.");
            successfullyParsed = int.TryParse(varValue, out varTypedValue);
            if (!successfullyParsed)
                throw new ArgumentException("Value of variable named \"" + varName + "\" (" + varValue + ") is not an integer.");
            return varTypedValue;
        }


        /// <summary>Obtains a value of a double-typed variable of the specified name, if such a
        /// variable is defined and its value can be interpreted as a number of type double.
        /// <para>Returns a flag indicating whether the value has been successfully obtained.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <returns>True if the value has been successfully obtained, false if not.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public bool TryGetDoubleVariable(string varName, out double varTypedValue)
        {
            bool successfullyParsed = false;
            string varValue = this.GetVariable(varName);
            if (varValue == null)
                throw new ArgumentException("Double-typed variable named \"" + varName + "\" is not defined.");
            successfullyParsed = double.TryParse(varValue, out varTypedValue);
            return successfullyParsed;
        }

        /// <summary>Returns the value of a double-valued variable of the specified name.
        /// <para>Exception is thrown if the specified variable does not exist or its string value 
        /// can not be interpreted as a number of type double.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <returns>Double value of the specified variable.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string or
        /// when a variable with the specified name does not exist or its string value does not represent a number of type double.</exception>
        public double GetDoubleVariable(string varName)
        {
            double varTypedValue;
            bool successfullyParsed = false;
            string varValue = this.GetVariable(varName);
            if (varValue == null)
                throw new ArgumentException("Double-typed variable named \"" + varName + "\" is not defined.");
            successfullyParsed = double.TryParse(varValue, out varTypedValue);
            if (!successfullyParsed)
                throw new ArgumentException("Value of variable named \"" + varName + "\" (" + varValue + ") is not a number of type double.");
            return varTypedValue;
        }


        /// <summary>Obtains a value of a boolean-typed variable of the specified name, if such a
        /// variable is defined and its value can be interpreted as a boolean value.
        /// <para>Returns a flag indicating whether the value has been successfully obtained.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <returns>True if the value has been successfully obtained, false if not.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string.</exception>
        public bool TryGetBooleanVariable(string varName, out bool varTypedValue)
        {
            bool successfullyParsed = false;
            string varValue = this.GetVariable(varName);
            if (varValue == null)
                throw new ArgumentException("Double-typed variable named \"" + varName + "\" is not defined.");
            bool varTypedValueTrial = default(bool);
            successfullyParsed = Util.TryParseBoolean(varValue, ref varTypedValueTrial);
            varTypedValue = varTypedValueTrial;
            return successfullyParsed;
        }

        /// <summary>Returns the value of a boolean-valued variable of the specified name.
        /// <para>Exception is thrown if the specified variable does not exist or its string value 
        /// can not be interpreted as a boolean value.</para></summary>
        /// <param name="varName">Name of the variable whose value is queried. Must not be null or empty string.</param>
        /// <returns>Boolean value of the specified variable.</returns>
        /// <exception cref="ArgumentException">When the specified variable name is null or empty string or
        /// when a variable with the specified name does not exist or its string value does not represent a boolean value.</exception>
        public bool GetBooleanVariable(string varName)
        {
            bool varTypedValue = default(bool);
            bool successfullyParsed = false;
            string varValue = this.GetVariable(varName);
            if (varValue == null)
                throw new ArgumentException("Double-typed variable named \"" + varName + "\" is not defined.");
            successfullyParsed = Util.TryParseBoolean(varValue, ref varTypedValue);
            if (!successfullyParsed)
                throw new ArgumentException("Value of variable named \"" + varName + "\" (" + varValue + ") is not a number of type double.");
            return varTypedValue;
        }


        /// <summary>Returns value of the flag with the specified name.
        /// <para>Flag is considered to be false if the variable of the specified flag name is not defined
        /// or its string name does not represent a boolean value, or it is the value of the specified variable
        /// interpreted as boolean.</para></summary>
        /// <param name="flagName">Name of the flag to be returned.</param>
        /// <returns>Value of the specified flag, i.e. value of the variable named <paramref name="flagName"/>
        /// interpreted as boolean, or false if there is no such variable or its value can not be 
        /// interpreted as boolean.</returns>
        /// <exception cref="ArgumentException">When the specified flag name is null or empty string.</exception>
        public bool GetFlag(string flagName)
        {
            bool varTypedValue = false;
            bool typedValueDefined = TryGetBooleanVariable(flagName, out varTypedValue);
            if (typedValueDefined)
                return varTypedValue;
            else
                return false;
        }


        /// <summary>Sets the flag of the specified name to the specified value.
        /// <para>Variable of the specified name is assigned string representation of the specified boolean value.</para></summary>
        /// <param name="flagName">Name of the flag to be set.</param>
        /// <param name="flagValue">Value to be assigned to the flag of the specified name.</param>
        public void SetFlag(string flagName, bool flagValue)
        {
            if (string.IsNullOrEmpty(flagName))
                throw new ArgumentException("Name of the flag to be set is not specified (null or empty string).");
            SetVariable(flagName, flagValue);
        }

        
        #endregion Operations.TypeSupportDefault


        #region ImportExport


        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// <para>If the file already exists, contents overwrite the file.</para></summary>
        /// <param name="filePath">Path to the file in variables and properties of the system are saved.</param>
        public virtual void Export(string filePath)
        {
            Export(filePath, false /* append */);
        }

        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// <para>If the file already exists, contents either overwrite the file or are appended at the end, 
        /// dependent on the value of the append flag.</para></summary>
        /// <param name="filePath">Path to the file in variables and properties of the system are saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public virtual void Export(string filePath, bool append)
        {
            StringVariableSystemDto savedVariables = new StringVariableSystemDto();
            savedVariables.CopyFrom(this);
            ExportDto(savedVariables, filePath, append);
        }

        /// <summary>Imports variables from the specified variable system in JSON format.</summary>
        /// <param name="filePath">File from which object is restored.</param>
        public virtual void Import(string filePath)
        {
            StringVariableSystemDto savedVariables = new StringVariableSystemDto();
            ImportDto(filePath, ref savedVariables);
            StringVariableSystem varSystem = this;
            savedVariables.CopyTo(ref varSystem);
        }

        #region ImportExport.Static


        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// File is owerwritten if it exists.</summary>
        /// <param name="variableSystem">Object that is saved to a file.</param>
        /// <param name="filePath">Path to the file in which object is is saved.</param>
        public static void SaveJson(StringVariableSystem variableSystem, string filePath)
        {
            SaveJson(variableSystem, filePath, false /* append */ );
        }

        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// If the file already exists, contents either overwrites the file or is appended at the end, 
        /// dependent on the value of the append flag.</summary>
        /// <param name="variableSystem">Object that is saved to a file.</param>
        /// <param name="filePath">Path to the file into which object is saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public static void SaveJson(StringVariableSystem variableSystem, string filePath, bool append)
        {
            StringVariableSystemDto dtoOriginal = new StringVariableSystemDto();
            dtoOriginal.CopyFrom(variableSystem);
            ISerializer serializer = new SerializerJson();
            serializer.Serialize<StringVariableSystemDto>(dtoOriginal, filePath, append);
        }

        /// <summary>Restores (deserializes) a vector from the specified file in JSON format.</summary>
        /// <param name="filePath">File from which object is restored.</param>
        /// <param name="variableSystemRestored">Object that is restored by deserialization.</param>
        public static void LoadJson(string filePath, ref StringVariableSystem variableSystemRestored)
        {
            ISerializer serializer = new SerializerJson();
            StringVariableSystemDto dtoRestored = serializer.DeserializeFile<StringVariableSystemDto>(filePath);
            dtoRestored.CopyTo(ref variableSystemRestored);
        }


        // DTO:

        /// <summary>Saves (serializes) the specified variable system DTO to the specified JSON file.
        /// <para>If the file already exists, contents overwrite the file.</para></summary>
        /// <param name="settings">Variable system DTO that is saved to a file.</param>
        /// <param name="filePath">Path to the file into which variables (and settings and other stuff) are saved.</param>
        public static void ExportDto(StringVariableSystemDto settings, string filePath)
        {
            ExportDto(settings, filePath, false /* append */);
        }

        /// <summary>Saves (serializes) the specified variable system DTO to the specified JSON file.
        /// <para>If the file already exists, contents either overwrite the file or are appended at the end, 
        /// dependent on the value of the append flag.</para></summary>
        /// <param name="variables">Variable system DTO that is saved to a file.</param>
        /// <param name="filePath">Path to the file into which variables (and settings and other stuff) are saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public static void ExportDto(StringVariableSystemDto variables, string filePath, bool append)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path not specified properly (null or empty string).");
            if (variables == null)
                throw new ArgumentException("Settings object is not specified (null reference).");
            ISerializer serializer = new SerializerJson();
            serializer.Serialize<StringVariableSystemDto>(variables, filePath, append);
        }


        /// <summary>Restores (deserializes) variable system DTO from the specified file in JSON format.
        /// <para>The restored DTO can be used e.g. to import variables to a variable system.</para></summary>
        /// <param name="filePath">File from which object is restored.</param>
        /// <param name="settings">Variable system DTO that is restored by deserialization.</param>
        public static void ImportDto(string filePath, ref StringVariableSystemDto settings)
        {
            ISerializer serializer = new SerializerJson();
            settings = serializer.DeserializeFile<StringVariableSystemDto>(filePath);
        }


        #endregion ImportExport.Static

        #endregion ImportExport


        #region Auxiliary


        /// <summary>Prints all variables (with their values) defined on the current object.</summary>
        public void PrintVariables()
        {
            lock (Lock)
            {
                SortedList<string, string> variables = this.Variables;
                Console.WriteLine("{0,10}: {1,-20}", "** NAME", "VALUE");
                foreach (KeyValuePair<string, string> kvp in variables)
                {
                    Console.WriteLine("{0,10}:  {1,-20}", kvp.Key, kvp.Value);
                }
            }
        }


        #endregion Auxiliary


        #region Examples

        /// <summary>Miscellaneous tests.</summary>
        public static void TestMisc()
        {
            StringVariableSystem vars = new StringVariableSystem();
            string varInt1 = "intVar1";
            int valInt1 = 2;
            vars.SetVariable(varInt1, valInt1);
            Console.WriteLine("Variables: ");
            vars.PrintVariables();
        }

        #endregion Examples

    }
}
