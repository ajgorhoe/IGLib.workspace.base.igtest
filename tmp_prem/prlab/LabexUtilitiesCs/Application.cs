
// APPLICATION-RELATED DATA,
// DISTINGUISHING VARIANTS OF APPLICATION, etc.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Premisa.ReporterMsg;   // Message reporter

namespace LabexUtilities
{

    /// <summary>Defines a variant of the Labex application. Used to switch between specifics
    /// of different variants of the application that are customized for different customers.</summary>
    public enum ApplicationVariant {Unknown = 0, InstitutZaPatologijo, Golnik}


    /// <summary>Provides information about the current application such as application variant.</summary>
    public static class ApplicationData
    {

        /// <summary>REMARK / Warning:
        /// Take care that the correct application variant is set (connect this with a general mechanism for establishing teh application variant)!
        /// This will become relevant later when there are more application variants available (currently there is only golnik).</summary>
        private static ApplicationVariant _variant = ApplicationVariant.Golnik;

        /// <summary>Gets the value indicating which variant of the application is currently running.</summary>
        public static ApplicationVariant Variant
        {
            get{ return _variant; }
            internal set
            {
                _variant = value; 
            }
        }

    } // class ApplicationData




}

