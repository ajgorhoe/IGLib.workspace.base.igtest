
// APPLICATION MESSAGE REPORTERS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Premisa.ReporterMsg;   // Message reporter

namespace LabexUtilities
{


    public partial class Rep
    {

        #region PadoReporters

        // PADO REPORTERS:

        /// <summary>Returns a properly initialized global reporter for applications without a console.</summary>
        public static ReporterPadoServer Server
        {
            get
            {
                if (!ReporterPadoServer.GlobalInitialized)
                {
                    Reporter r = Reporter.Global;

                    // REMARK: 
                    // Settings below are intended for testing stage of the software. 
                    // You can comment or rehsape this section later in order to better suite application needs!
                    // Note that reporter settings for the global reporter are already read from application settings
                    // for the default and "General" groups. Settings from this section only define the default
                    // behavior that applies in the case that some particular setting is not set in the corresponding
                    // configuration file.
                    r.UseTrace = true;
                    r.UseTextLogger = r.UseTextWriter = true;
                    r.ReportingLevel = ReportLevel.Warning;
                    r.LoggingLevel = ReportLevel.Off;
                    r.TracingLevel = ReportLevel.Warning;


                    r.ReadApplicationSettings("Server");
                }
                return ReporterPadoServer.Global;
            }
        }

        /// <summary>Returns a properly initialized global reporter for applications without a console.</summary>
        public static ReporterPadoWebService WS
        {
            get
            {
                if (!ReporterPadoWebService.GlobalInitialized)
                {
                    Reporter r = Reporter.Global;

                    // REMARK: 
                    // Settings below are intended for testing stage of the software. 
                    // You can comment or rehsape this section later in order to better suite application needs!
                    // Note that reporter settings for the global reporter are already read from application settings
                    // for the default and "General" groups. Settings from this section only define the default
                    // behavior that applies in the case that some particular setting is not set in the corresponding
                    // configuration file.
                    r.UseTrace = true;
                    r.UseTextLogger = r.UseTextWriter = true;
                    r.ReportingLevel = ReportLevel.Warning;
                    r.LoggingLevel = ReportLevel.Off;
                    r.TracingLevel = ReportLevel.Warning;


                    r.ReadApplicationSettings("Server");
                }
                return ReporterPadoWebService.Global;
            }
        }

        /// <summary>Returns a properly initialized global reporter for client applications with a console.</summary>
        public static ReporterPadoClient ConsoleClient
        {
            get
            {
                if (!ReporterPadoClient.GlobalInitialized)
                {
                    Reporter r = Reporter.Global;

                    // REMARK: 
                    // Settings below are intended for testing stage of the software. 
                    // You can comment or rehsape this section later in order to better suite application needs!
                    // Note that reporter settings for the global reporter are already read from application settings
                    // for the default and "General" groups. Settings from this section only define the default
                    // behavior that applies in the case that some particular setting is not set in the corresponding
                    // configuration file.
                    r.UseTrace = true;
                    r.UseTextLogger = r.UseTextWriter = true;
                    r.ReportingLevel = ReportLevel.Warning;
                    r.LoggingLevel = ReportLevel.Off;
                    r.TracingLevel = ReportLevel.Warning;

                    r.ReadApplicationSettings("Client");
                }
                return ReporterPadoClient.Global;
            }
        }

        #endregion  // PadoReporters



    }  // class Rep






}
