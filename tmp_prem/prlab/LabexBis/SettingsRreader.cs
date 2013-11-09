﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IG.Lib
{

    // TODO: remove two definitions below later! 
    public interface ISettingsServer
    {
    }
    public class SettingsServerBase : ISettingsServer
    {
        public SettingsServerBase() { }
    }



    /// <summary>Interface for settings readers, which read pairs key/vvalue from various 
    /// files or configuration servers. Intended for simple configurations!</summary>
    public interface ISettingsRreader
    {

    }

    public abstract class SettingsReaderBase: ISettingsRreader
    {

        public object lockobj = new object();


        #region General

        protected bool _expandEnv = true;

        public virtual bool ExpandEnvironmentVariables
        {
            get { return _expandEnv; }
            set { _expandEnv = true; }
        }

        
        /// <summary>Retrieves the specified setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        public abstract string GetSetting(string settingname);
 

        /// <summary>Retrieves the specified boolean setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        public virtual bool GetBooleanSetting(string settingname)
        {
            return ToBoolean(GetSetting(settingname));
        }

        /// <summary>Retrieves the specified integer setting from the current settings source.
        /// If the specific setting is not specified in the configuration then 0 is returned.</summary>
        /// <param name="settingname">Setting name.</param>
        public virtual long GetIntegerSetting(string settingname)
        {
            return ToInt(GetSetting(settingname));
        }

        /// <summary>Retrieves the specified integer setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        /// <param name="defaultvalue">Value returned in the case that the correspondig setting is not defined.</param>
        public virtual long GetIntegerSetting(string settingname, long defaultvalue)
        {
            return ToInt(GetSetting(settingname), defaultvalue);
        }

        #endregion General




        #region Type_Conversions

        /// <summary>Converts a string representation of a boolean setting to boolean.
        /// Strings "true", "yes" and "on" (regardless of capitalization) or non-zero integer representations
        /// result to true, anything else (including null or empty string) result in false.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <returns>Boolean value corresponding to the setting.</returns>
        protected bool ToBoolean(string strsetting)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(strsetting))
            {
                bool interpreted = false;
                strsetting = strsetting.ToLower();
                if (strsetting == "true" || strsetting == "yes" || strsetting == "on")
                {
                    ret = true;
                    interpreted = true;
                }
                else if (strsetting == "false" || strsetting == "no" || strsetting == "off")
                {
                    ret = false;
                    interpreted = true;
                }
                else
                {
                    try
                    {
                        int i = 0;
                        i = int.Parse(strsetting);
                        interpreted = true;
                        if (i != 0)
                            ret = true;
                    }
                    catch { }
                }
                if (!interpreted)
                    throw new Exception("Could not interpret the following boolean setting: "
                        + strsetting + ".");
            }
            return ret;
        }


        /// <summary>Converts a string representation of an integer setting to an integer value.
        /// If the setting is not defined then 0 is returned.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        protected long ToInt(string strsetting)
        {
            return ToInt(strsetting, 0 /* defaultvalue */);
        }


        /// <summary>Converts a string representation of an integer setting to an integer value.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <param name="defaultvalue">Default value returned in the case that the setting is not defined.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        protected long ToInt(string strsetting, long defaultvalue)
        {
            long ret = defaultvalue;
            if (!string.IsNullOrEmpty(strsetting))
                ret = long.Parse(strsetting);
            return ret;
        }

        #endregion  // Type_Conversions



        #region Static_Functions

        static ISettingsRreader FromAppConfig()
        {
            return new SettingsReaderAppConfig();
        }

        #endregion Static_Functions


    }  // SettingsReaderBase


       // public enum SettingSource { Unknown = 0, AppConfig, SettingsServer }

    

    


    /// <summary>Provides functionality for retrieving settings from various sources such as 
    /// application settings file.</summary>
    /// $A Igor Apr10;
    public class SettingsReaderAppConfig : SettingsReaderBase
    {

        #region General

        /// <summary>Retrieves the specified setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        public override string GetSetting(string settingname)
        {
            return GetSettingAppConfig(settingname);
        }

        #endregion  // General


        #region AppConfig

        // Methods for reading settings from the application configuration file:

        protected virtual string GetSettingAppConfig(string settingname)
        {
            string ret = ConfigurationManager.AppSettings.Get(settingname);
            return ret;
        }

        #endregion  // AppConfig


    }  // class SettingsReader

}
