using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LabexUtilities
{

    /// <summary>Provides functionality for retrieving settings from various sources such as 
    /// application settings file or PADO server configuration file.</summary>
    public class SettingsReader
    {
        // REMARK: you can set this class back to abstract when the bug with initialization is removed.

        public enum SettingSource { Unknown = 0, AppConfig, PadoServer }

        public object lockobj = new object();

        public bool ExpandEnvironmentVariables = true;

        #region Type_Conversions

        /// <summary>Converts a string representation of a boolean setting to boolean.
        /// Strings "true", "yes" and "on" (regardless of capitalization) or non-zero integer representations
        /// result to true, anything else (including null or empty string) result in false.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <returns>Boolean value corresponding to the setting.</returns>
        private bool ToBoolean(string strsetting)
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
        private long ToInt(string strsetting)
        {
            return ToInt(strsetting, 0 /* defaultvalue */);
        }


        /// <summary>Converts a string representation of an integer setting to an integer value.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <param name="defaultvalue">Default value returned in the case that the setting is not defined.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        private long ToInt(string strsetting, long defaultvalue)
        {
            long ret = defaultvalue;
            if (!string.IsNullOrEmpty(strsetting))
                ret = long.Parse(strsetting);
            return ret;
        }

        #endregion  // Type_Conversions


        #region General

        // Data that specify where from and how settings are read:

        protected SettingSource Source = SettingSource.Unknown;
        private Premisa.PadoInterfaces.itfPadoServer PadoServer = null;

        /// <summary>Sets the source of settings to application configuration file.
        /// Warning: A call to this method and subsequent reading operations must be enclosed in a lock block,
        /// more specifically in "lock (lockobj) { .. }".</summary>
        protected void SetSourceAppconfig()
        {
            Source = SettingSource.AppConfig;
        }

        /// <summary>Sets the source of settings to the Pado Server configuration file.
        /// Warning: A call to this method and subsequent reading operations must be enclosed in a lock block,
        /// more specifically in "lock (lockobj) { .. }".</summary>
        /// <param name="padoserver">Reference to the Pado Server through wihich the server settings can be accessed.</param>
        protected void SetSourcePadoServer(Premisa.PadoInterfaces.itfPadoServer padoserver)
        {
            Source = SettingSource.PadoServer;
            PadoServer = padoserver;
        }

        /// <summary>Sets the source of settings to the Pado Server configuration file.
        /// Warning: A call to this method and subsequent reading operations must be enclosed in a lock block,
        /// more specifically in "lock (lockobj) { .. }".</summary>
        /// <param name="om">Reference to the Pado ObjectManager through wihich the server settings can be accessed.</param>
        protected void SetSourcePadoServer(Premisa.PadoServerClasses.clsPadoObjectManager om)
        {
            Source = SettingSource.PadoServer;
            PadoServer = om.Server;
        }


        // Reading of settings from any source, which must be pre-sed before the methods from this 
        // region are called. In general, the source must be pre-set within the lock(lockobj) block
        // to avoid inter-thread conflicts!


        /// <summary>Retrieves the specified setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        public string GetSetting(string settingname)
        {
            string ret = null;
            switch (Source)
            {
                case SettingSource.AppConfig:
                    ret = GetSettingAppConfig(settingname);
                    break;
                case SettingSource.PadoServer:
                    if (PadoServer == null)
                        throw new Exception("Pado server to manage reading of settings is not specified.");
                    ret = GetSettingPadoServer(PadoServer, settingname);
                    break;
                default:
                    throw new NotImplementedException("Reading of settings is not implemented for the folloving source: "
                        + Source.ToString());
            }
            return ret;
        }

        /// <summary>Retrieves the specified boolean setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        public bool GetBooleanSetting(string settingname)
        {
            return ToBoolean(GetSetting(settingname));
        }

        /// <summary>Retrieves the specified integer setting from the current settings source.
        /// If the specific setting is not specified in the configuration then 0 is returned.</summary>
        /// <param name="settingname">Setting name.</param>
        public long GetIntegerSetting(string settingname)
        {
            return ToInt(GetSetting(settingname));
        }

        /// <summary>Retrieves the specified integer setting from the current settings source.</summary>
        /// <param name="settingname">Setting name.</param>
        /// <param name="defaultvalue">Value returned in the case that the correspondig setting is not defined.</param>
        public long GetIntegerSetting(string settingname, long defaultvalue)
        {
            return ToInt(GetSetting(settingname), defaultvalue);
        }

        #endregion  // General


        #region AppConfig

        // Methods for reading settings from the application configuration file:

        public string GetSettingAppConfig(string settingname)
        {
            string ret = ConfigurationManager.AppSettings.Get(settingname);
            return ret;
        }

        public bool GetBooleanSettingAppConfig(string settingname)
        {
            return ToBoolean(GetSettingAppConfig(settingname));
        }

        #endregion  // AppConfig


        #region Pado

        // Methods for reading settings from the application configuration file:

        /// <summary>Retrieves the specified setting from the PADO Server's connfiguration file.</summary>
        /// <param name="server">Pado server whose setting file defines the settings.</param>
        /// <param name="settingname">Setting name.</param>
        public string GetSettingPadoServer(Premisa.PadoInterfaces.itfPadoServer server, string settingname)
        {
            string ret = null;

            if (server.Settings[settingname] != null)
            {
                ret = server.Settings[settingname].Value;
                if (!string.IsNullOrEmpty(ret) && ExpandEnvironmentVariables)
                {
                    ret = Environment.ExpandEnvironmentVariables(ret);
                }
            }
            return ret;
        }


        /// <summary>Retrieves the specified boolean setting from the PADO Server's connfiguration file.</summary>
        /// <param name="om"></param>
        /// <param name="settingname"></param>
        public bool GetBooleanSettingPadoServer(Premisa.PadoInterfaces.itfPadoServer server, string settingname)
        {
            return ToBoolean(GetSettingPadoServer(server, settingname));
        }


        // Using Object manager instead of the itfPadoServer interface:

        /// <summary>Retrieves the specified setting from the PADO Server's connfiguration file.</summary>
        public string GetSettingPadoServer(Premisa.PadoServerClasses.clsPadoObjectManager om, string settingname)
        { return GetSettingPadoServer(om.Server, settingname); }

        /// <summary>Retrieves the specified boolean setting from the PADO Server's connfiguration file.</summary>
        public bool GetBooleanSettingPadoServer(Premisa.PadoServerClasses.clsPadoObjectManager om, string settingname)
        { return GetBooleanSettingPadoServer(om.Server, settingname); }


        #endregion // Pado


    }  // class SettingsReader

}
