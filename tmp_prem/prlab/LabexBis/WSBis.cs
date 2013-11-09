
// COMMUNICATION WITH THE Calypso WEB SERVICE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Configuration;

//using LabexUtilities;

//using Premisa;
//using Premisa.PadoInterfaces;
using System.Xml;

using System.Data;
using System.Data.SqlClient;

using IG.Lib;

// using WSBP;

 


namespace LabexBis
{

    public enum SettingSource { Unknown = 0, AppConfig, SettingsServer }


    /// <summary>Holds settings used by the Labex - Bis communication module.</summary>
    // public class BisCommunicationSettings : LabexUtilities.SettingsRreader
    public class BisCommunicationSettings : SettingsReaderAppConfig
    {

        ///// <summary>Default constructor, sets source to application settings file.</summary>
        //public BisCommunicationSettings()
        //{
        //    SetSourceAppconfig();
        //}

        /// <summary>Gets objects that contains all constants related to communication messages.</summary>
        static protected MsgConst Msg
        {
            get { return MsgConst.Const; }
        }

        static IReporter _rep = new ReporterConsole();

        /// <summary>Reporter for this class.</summary>
        public static IReporter R { get { return _rep; } }

        public const string
            SettingName_StartCalypsoReceiver = "StartCalypsoReceiver",
            SettingName_ForceMessageReceiptConfirmation = "ForceMessageReceiptConfirmation",
            SettingName_MaxNumMessages = "MaxNumMessages",
            SettingName_DebugCalypsoReceiver = "DebugCalypsoReceiver",
            SettingName_MessageFile = "MessageFile",
            SettingName_MessageSchemaDir = "MessageSchemaDir",
            SettingName_MessageIncomingDir = "MessageIncomingDir",
            SettingName_MessageOutgoingDir = "MessageOutgoingDir",
            SettingName_MessageServiceUrl = "MessageServiceUrl",
            SettingName_ModelFileObservationOrder = "ModelFileObservationOrder",
            SettingName_ModelFileObservationEvent = "ModelFileObservationEvent",
            SettingName_ModelFileFinancialTransaction = "ModelFileFinancialTransaction";

        /* REMARK:
            It might turn useful to define some settings as properties whose get accessors would first
        check whether or not the settings have already been read (either collectively or individually).
        Since this is situation specific, implementors might later re-consider changing the implementation. 
        */

        public bool 
            StartCalypsoReceiver = false,
            ForceMessageReceiptConfirmation = false,
            DebugCalypsoReceiver = false;
        public int MaxNumMessages = 0;
        public string
            MessageFile = null,
            MessageSchemaDir = null,
            MessageIncomingDir = null,
            ForcedIncomingReceiptsDirectory = "Forced",
            MessageOutgoingDir = null,
            MessageServiceUrl = null,
            ModelFileObservationOrder,
            ModelFileObservationEvent,
            ModelFileFinancialTransaction;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SettingName_StartCalypsoReceiver + ": " +  StartCalypsoReceiver 
                + ", " + SettingName_DebugCalypsoReceiver + ": " + DebugCalypsoReceiver);
            sb.AppendLine(SettingName_ForceMessageReceiptConfirmation + ": " + ForceMessageReceiptConfirmation.ToString());
            sb.AppendLine(SettingName_MaxNumMessages + ": " + MaxNumMessages);
            sb.AppendLine(SettingName_MessageFile + ": " +  MessageFile);
            sb.AppendLine(SettingName_MessageSchemaDir + ": " + MessageSchemaDir);
            sb.AppendLine(SettingName_MessageIncomingDir + ": " + MessageIncomingDir);
            sb.AppendLine(SettingName_MessageOutgoingDir + ": " + MessageOutgoingDir);
            sb.AppendLine(SettingName_MessageServiceUrl + ": " + MessageServiceUrl);
            sb.AppendLine(SettingName_ModelFileObservationOrder + ": " + ModelFileObservationOrder);
            sb.AppendLine(SettingName_ModelFileObservationEvent + ": " + ModelFileObservationEvent);
            sb.AppendLine(SettingName_ModelFileFinancialTransaction + ": " + ModelFileFinancialTransaction);


            return sb.ToString();
        }


        protected bool settingsread = false;


        /// <summary>Common method used for retrieving the settings.
        /// Methods for reading settings form different sources call this method.</summary>
        protected void getsettings()
        {
            //if (Source == SettingSource.Unknown)
            //    throw new Exception("Attempt to read settings without specifying the source." + Environment.NewLine
            //        + "Probable reason is that a general method for reading settins was called by one of the internal methods of the current class.");
            StartCalypsoReceiver = GetBooleanSetting(SettingName_StartCalypsoReceiver);
            ForceMessageReceiptConfirmation = GetBooleanSetting(SettingName_ForceMessageReceiptConfirmation);
            MaxNumMessages = (int) GetIntegerSetting(SettingName_MaxNumMessages);
            DebugCalypsoReceiver = GetBooleanSetting(SettingName_DebugCalypsoReceiver);
            MessageFile = GetSetting(SettingName_MessageFile);
            MessageSchemaDir = GetSetting(SettingName_MessageSchemaDir);
            MessageIncomingDir = GetSetting(SettingName_MessageIncomingDir);
            MessageOutgoingDir = GetSetting(SettingName_MessageOutgoingDir);
            MessageServiceUrl = GetSetting(SettingName_MessageServiceUrl);
            ModelFileObservationOrder = GetSetting(SettingName_ModelFileObservationOrder);
            ModelFileObservationEvent = GetSetting(SettingName_ModelFileObservationEvent);
            ModelFileFinancialTransaction = GetSetting(SettingName_ModelFileFinancialTransaction);

            settingsread = true;

            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine("WS SETTINGS:");
            //Console.WriteLine(ToString());
            //Console.WriteLine();
            //Console.WriteLine();
        }


        /// <summary>Reads settindgs for communication with Bis from the application settings file.</param>
        public void ReadSettingsAppConfig()
        {
            lock (lockobj)  // prevent other thread changing setings while operation is performed (since the operation is state-dependent)
            {
                // SetSourceAppconfig();
                getsettings();
            }
        }



        #region Utilities

        /// <summary>Returns the name of the file where messages are stored.</summary>
        /// <param name="leader">The leading string of the file name, such as "Incoming".</param>
        /// <param name="Id">Id incorporated in the file name.</param>
        /// <param name="AddGuid">If true then a global unique string is included in file name.</param>
        /// <returns>Composed file name.</returns>
        protected string MessageFileName(string leader, string Id, bool AddGuid)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.MessageFileName(Leader,Id)", "Started...");
            string ret = null;
            string GuidStr = "";
            if (AddGuid)
                GuidStr = "____" + Guid.NewGuid();
            if (Id == null)
                Id = ".UnknownId.";
            if (string.IsNullOrEmpty(leader))
                leader = "Message";
            ret = leader + "_" + Msg.TimeStamp(true /* includetime */, true /* underscores */)
                + "__" + Id.Replace(":", ".") + GuidStr + ".xml";
            if (R.TreatInfo) R.ReportInfo("Msg.MessageFileName(Leader,Id)", "Finished.");
            --R.Depth;
            return ret;
        }


        /// <summary>Returns the name of the file in which incoming msg is stored.</summary>
        /// <param name="Id">ID of the incoming msg.</param>
        public string IncomingFileName(string Id)
        {
            return IncomingFileName(Id, true /* AddGuid */ );
        }

        /// <summary>Returns the name of the file in which incoming msg is stored.</summary>
        /// <param name="Id">ID of the incoming msg.</param>
        /// <param name="AddGuid">Specifies whether or not a GUID is added to msg filenames.</param>
        public string IncomingFileName(string Id, bool AddGuid)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.IncomingFileName(Id)", "Started...");
            string ret = "";
            try
            {
                if (!settingsread)
                    getsettings();
                string dirname = MessageIncomingDir;
                if (string.IsNullOrEmpty(dirname))
                    throw new Exception("Directory to store incoming messages in is not specified.");
                if (!Directory.Exists(dirname))
                {
                    //Directory.CreateDirectory(dirname);
                    //if (!Directory.Exists(dirname))
                    throw new Exception("Directory for stroing incoming messages does not exist: \""
                        + dirname + "\".");
                }
                ret = Path.Combine(dirname,MessageFileName("Incoming",Id,AddGuid));
                return ret;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw (ex);
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.IncomingFileName(Id)", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Returns the name of the file in which an outgoing msg is stored.</summary>
        /// <param name="Id">ID of the outgoing msg.</param>
        public string OutgoingFileName(string Id)
        {
            return OutgoingFileName(Id, true /* AddGuid */ );
        }

        /// <summary>Returns the file name for an outgoing msg with the specified Id.</summary>
        /// <param name="Id">Id of the msg.</param>
        /// <returns>Name of the file where outgoing msg is stored.</returns>
        public string OutgoingFileName(string Id, bool AddGuid)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.OutgoingFileName.get(Id)", "Started...");
            string ret = "";
            try
            {
                if (!settingsread)
                    getsettings();
                string dirname = MessageOutgoingDir;
                if (string.IsNullOrEmpty(dirname))
                    throw new Exception("Directory to store outgoind messages in is not specified.");
                if (!Directory.Exists(dirname))
                {
                    //Directory.CreateDirectory(dirname);
                    //if (!Directory.Exists(dirname))
                    throw new Exception("Directory for stroing outgoing messages does not exist: \""
                        + dirname + "\".");
                }
                ret = Path.Combine(dirname,MessageFileName("Outgoing",Id,AddGuid));
                return ret;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw (ex);
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.OutgoingFileName.get(Id)", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Returns the name of the file in which an nullified message is stored.</summary>
        /// <param name="Id">ID of the outgoing msg.</param>
        public string NullifiedFileName(string Id)
        {
            return NullifiedFileName(Id, true /* AddGuid */ );
        }

        /// <summary>Returns the file name for a nullified message with the specified Id.</summary>
        /// <param name="Id">Id of the msg.</param>
        /// <returns>Name of the file where outgoing msg is stored.</returns>
        public string NullifiedFileName(string Id, bool AddGuid)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.NullifiedFileName.get(Id)", "Started...");
            string ret = "";
            try
            {
                if (!settingsread)
                    getsettings();
                string dirname = MessageOutgoingDir;
                if (string.IsNullOrEmpty(dirname))
                    throw new Exception("Directory to store outgoind messages in is not specified.");
                if (!Directory.Exists(dirname))
                {
                    //Directory.CreateDirectory(dirname);
                    //if (!Directory.Exists(dirname))
                    throw new Exception("Directory for stroing outgoing messages does not exist: \""
                        + dirname + "\".");
                }
                ret = Path.Combine(dirname, MessageFileName("Nullified", Id, AddGuid));
                return ret;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw (ex);
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.NullifiedFileName.get(Id)", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Returns filename of an example msg.</summary>
        public string ExampleFile(MessageType type)
        {
            string ret = "";
            try
            {
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("Msg.ExampleFile(type)", "Started...");
                if (!settingsread)
                    getsettings();
                string dirname = MessageSchemaDir;
                if (!Directory.Exists(dirname))
                {
                    throw new Exception("Directory for stroing schemas and example files does not exist: \""
                        + dirname + "\".");
                }
                ret = Path.Combine(dirname, type.ToString() + ".xml");
                if (!File.Exists(ret))
                {
                    throw new Exception("Example file does not exist: \""
                        + dirname + "\".");
                }
                return ret;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw (ex);
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.ExampleFile(type)", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Returns the name of the file in which incoming msg should be stored for a file whose
        /// receipt confirmation was forced.</summary>
        /// <param name="Id">ID of the incoming msg.</param>
        public string ForcedFileName(string Id)
        {
            return ForcedFileName(Id, true /* AddGuid */ );
        }

        /// <summary>Returns the name of the file in which incoming msg should be stored for a file whose
        /// receipt confirmation was forced.</summary>
        /// <param name="Id">ID of the incoming msg.</param>
        /// <param name="AddGuid">Specifies whether or not a GUID is added to msg filenames.</param>
        public string ForcedFileName(string Id, bool AddGuid)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.ForcedFileName(Id)", "Started...");
            string ret = "";
            try
            {
                if (!settingsread)
                    getsettings();
                string dirname = MessageIncomingDir;
                if (string.IsNullOrEmpty(dirname))
                    throw new Exception("Directory to store incoming messages in is not specified.");
                if (!Directory.Exists(dirname))
                {
                    //Directory.CreateDirectory(dirname);
                    //if (!Directory.Exists(dirname))
                    throw new Exception("Directory for stroing incoming messages does not exist: \""
                        + dirname + "\".");
                } else
                {
                    dirname = Path.Combine(dirname,ForcedIncomingReceiptsDirectory);
                    if (!Directory.Exists(dirname))
                    {
                        Directory.CreateDirectory(dirname);
                        if (!Directory.Exists(dirname))
                            throw new Exception("Directory for stroing incoming messages does not exist: \""
                                + dirname + "\".");
                    }                }
                ret = Path.Combine(dirname, MessageFileName("Forced_Incoming_Confirmation", Id, AddGuid));
                return ret;
            }
            catch (Exception ex)
            {
                // R.ReportError(ex);
                throw (ex);
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.ForcedFileName(Id)", "Finished.");
                --R.Depth;
            }
        }


        // ReadSettings ReadSettings ReadSettings 

        #endregion  // Utilities

    }  // class BisCommunicationSettings


    /// <summary>Utilities for communication with the web service Calypso.</summary>
    public class Calypso : System.IDisposable
    {
        static IReporter _rep = new ReporterConsole();

        /// <summary>Reporter for this class.</summary>
        public static IReporter R { get { return _rep; } }

        /// <summary>Gets objects that contains all constants related to communication messages.</summary>
        protected MsgConst Const
        {
            get { return MsgConst.Const; }
        }
        

        private static Calypso _global = null;

        /// <summary>Global communication object (application-wide).</summary>
        public static Calypso Global
        {
            get 
            {
                if (_global == null)
                    _global = new Calypso();
                return _global;
            }
        }


        private LabexBis.WebReference.CalypsoWSNSService _service = null;

        /// <summary>Web service used for sending messages between systems.</summary>
        protected LabexBis.WebReference.CalypsoWSNSService Service
        {
            get
            {
                if (_service == null)
                {
                    LabexBis.WebReference.CalypsoWSNSService serviceaux;
                    serviceaux = new LabexBis.WebReference.CalypsoWSNSService();
                    if (Settings == null)
                        throw new Exception("Bis communication object: Settings are not defined. This is a critical error.");
                    if (string.IsNullOrEmpty(Settings.MessageServiceUrl))
                        throw new Exception("Communication Settings: service URL is not specified. " + Environment.NewLine
                            + "Verify that application is properly configured and settings are read. " + Environment.NewLine
                            + "Settings must be read explicitly after communication object is initialized.");
                    serviceaux.Url = Settings.MessageServiceUrl;
                    _service=serviceaux;
                }
                return _service;
            }
        }


        const string 
            QueueNameLabex = "Labex",  // "Premisa",  // name of the messague queue where Labex receives messages
            QueueNameBis = "",
            //ReceiverLabex = "Labex",
            //ReceiverBis = "b21App",
            HL7Version = "v30";

        public string ReceiverBis
        { get { return MsgConst.Const.BisId; } }

        public string ReceiverLabex
        { get { return MsgConst.Const.LabexId; } }

        #region MessageReceiver



        // private static biscom MessageFile = null;

        // A reference to communication settings that were retreived at  application-specific location:
        public BisCommunicationSettings Settings = new BisCommunicationSettings();

        private bool _dbCmdInitialized = false;

 

        /// <summary>Receives and processes one msg from the callypso server.
        /// Dependent on Pado server settings, msg can aldo be read from a file (for testing purposes).</summary>
        /// <param name="ReadFromWs">If false then msg is not read from the web service but from a file.</param>
        private void ReceiveAndProcessMessage(bool ReadFromWS) { }

        //private void ReceiveAndProcessMessage(bool ReadFromWS)
        //{
        //    string ForcedReceiptFileName = ""; // this will eventually store the name of the file where incoming message has been stored
        //                    // in the case that data could not be imported but receipt confirmation was sent anyway (forced by settings)
        //    if (ControlOutputs)
        //    {
        //        Console.WriteLine(Environment.NewLine + "Checking for the next message on Calypso. " +
        //            DateTime.Now.ToString() + Environment.NewLine);
        //    }
        //    MsgBase msg1 = null;
        //    if (ReadFromWS)
        //    {
        //        // Read the next msg from Calypso:
        //        msg1 = GetMessage();
        //        if (ControlOutputs)
        //            Console.WriteLine("A message has been received from Calypso.");
        //    } else
        //    {
        //        // Simulation of reading the next msg - read from file:
        //        string MessageLocation = Settings.MessageFile;
        //        if (ControlOutputs)
        //            Console.WriteLine("Reading message from file (emulation) ...");
        //        using (StreamReader streamReader = new StreamReader(MessageLocation))
        //        {
        //            string messagestr = streamReader.ReadToEnd();
        //            if (string.IsNullOrEmpty(messagestr))
        //                throw new Exception("Could not load XML from the following file: "
        //                    + Environment.NewLine + "  " + MessageLocation );
        //            msg1 = new MsgObervationOrder();
        //            msg1.MessageXml = messagestr;
        //        }
        //    }

        //    if (msg1 != null)
        //    {
        //        // Message has been read, process the msg (extract data):
        //        if (ControlOutputs)
        //            Console.WriteLine("Message has been read, processing...");

        //        if (Settings.ForceMessageReceiptConfirmation && ReadFromWS)
        //        {
        //            // Confirm receipt of the message even if some of the later operations will fail:
        //            try
        //            {
        //                if (string.IsNullOrEmpty(msg1.MessageId))
        //                {
        //                    // Forced receipt confirmation is set but the message has probably been read from a file rather than obtianed from WS:
        //                    R.ReportWarning("Calypso.ReceiveAndProcessMessage", "Forced receipt confirmatin is switched on, but message ID necessary for confirmation is not specified."
        //                        + Environment.NewLine + "A likely cause is that the message was not obtained from the web service but read from a file."
        //                        + Environment.NewLine + "Receipt confirmation will not be sent. File will just be imported if no errors occur.");
        //                }
        //                else
        //                {
        //                    // Save the message in the special directory:
        //                    ForcedReceiptFileName = Settings.ForcedFileName(msg1.MessageId);
        //                    R.ReportWarning("Calypso.ReceiveAndProcessMessage", "Receipt confirmation will be forced for the received messags."
        //                        + Environment.NewLine + "Before that, message will be saved to the following file (deleted if import succeeds):"
        //                        + Environment.NewLine + ForcedReceiptFileName);
        //                    msg1.Doc.Save(ForcedReceiptFileName);
        //                    msg1.MessageFile = ForcedReceiptFileName;

        //                    ConfirmMessageReceipt(msg1.MessageId);
        //                    Console.WriteLine(Environment.NewLine + "Message receipt confirmed for message with ID: "
        //                        + msg1.MessageId + Environment.NewLine);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                R.ReportError("Forced receipt confirmation failed. Details: " + ex.Message);
        //            }
        //        }

        //        //Init db connection and db command:
        //        InitDbCommand();
        //        msg1.LoadXml(msg1.MessageXml);
        //        if (ControlOutputs)
        //            msg1.WriteToConsole();  // Output to the console window
        //        if (msg1.Type == MessageType.SpecimenObservationOrder)
        //        {



        //            MsgObervationOrder msg = msg1 as MsgObervationOrder;
        //            bool SaveSucceeded = false;
        //            string errorstring = null;
        //            try
        //            {
        //                msg1.Read();
        //                // Treat a msg of type SpecimenObservationOrder:
        //                if (msg == null)
        //                    throw new Exception("Type of the message object is not consistent with type specification ("
        //                        + msg.Type.ToString() + ").");

        //                // Create a database rsr object for orders:
        //                Premisa.PadoUtilities.PadoEnums.enGetObjectParam objectparam =
        //                        Premisa.PadoUtilities.PadoEnums.enGetObjectParam.enTrustedOperation;
        //                PAT_bis_classes.clsMsgOrder MsgOrder = (PAT_bis_classes.clsMsgOrder)
        //                    ObjectManager.GetObjectNew(null, typeof(PAT_bis_classes.clsMsgOrder),
        //                    null, null, ref objectparam, null, null);
        //                // Copy extracted data to the database rsr object:
        //                if (ControlOutputs)
        //                {
        //                    Console.WriteLine(Environment.NewLine + "Before copying message to the database record object.");
        //                    //Console.WriteLine("Press <Enter>!");
        //                    //Console.ReadLine();
        //                }

        //                if (ObjectManager == null)
        //                {
        //                    if (ControlOutputs)
        //                        Console.WriteLine("Object manager is null!");
        //                    throw new Exception("A reference to object manager is null.");
        //                }
        //                if (MsgOrder == null)
        //                {
        //                    if (ControlOutputs)
        //                        Console.WriteLine("Message record object is null!");
        //                    throw new Exception("A reference to message record object is null.");
        //                }

        //                msg.CopyToDbRecord(ObjectManager, MsgOrder);
        //                if (ControlOutputs)
        //                    Console.WriteLine("Before saving the order to the database...");
        //                // TableRecord.
        //                Premisa.PadoInterfaces.itfPadoObject MsgOrderArg = MsgOrder;
        //                System.Collections.Hashtable _params = new System.Collections.Hashtable();
        //                itfPadoObjectDbFacade _dbFacadeOrder = ObjectManager.ObjectDbFacadeList[typeof(PAT_bis_classes.clsMsgOrder).FullName];
        //                itfPadoObjectDbFacade _dbFacadeSample = ObjectManager.ObjectDbFacadeList[typeof(PAT_bis_classes.clsMsgSample).FullName];
        //                itfPadoObjectDbFacade _dbFacadeDiagnosis = ObjectManager.ObjectDbFacadeList[typeof(PAT_bis_classes.clsMsgDgnClinical).FullName];
        //                _params.Add(_dbFacadeOrder.TypeName, _dbFacadeOrder.GetClonedParamArrays());
        //                _params.Add(_dbFacadeSample.TypeName, _dbFacadeSample.GetClonedParamArrays());
        //                _params.Add(_dbFacadeDiagnosis.TypeName, _dbFacadeDiagnosis.GetClonedParamArrays());
        //                // begin tran
        //                bool saveOk = false;
        //                //// Remark: proper transaction code will be added by MH! 
        //                //System.Data.IDbTransaction tran = null;
        //                try
        //                {

        //                    //// Transaction related code: comment/uncomment!
        //                    //tran = ObjectManager.DbCnn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        //                    //mDbCmd.Transaction = tran;

        //                    saveOk = ObjectManager.SaveObject(null, ref MsgOrderArg, mDbCmd, _params, true, false, false, false, false, true);
        //                    if (saveOk)
        //                        SaveSucceeded = true;

        //                    //// Transaction related code: comment/uncomment!
        //                    //if (saveOk)
        //                    //    tran.Commit();
        //                    //else
        //                    //    tran.Rollback();


        //                    if (ControlOutputs)
        //                        Console.WriteLine("Saving done." + Environment.NewLine);
        //                }
        //                catch (Exception ex)
        //                {
        //                    //// Transaction related code: comment/uncomment!
        //                    //if (tran != null) { tran.Rollback(); }
                            
                            
        //                    R.ReportError(ex);
        //                    throw ex;
        //                }


        //                // Since operations until here were successful, confirm message receipt:
        //                if (!Settings.ForceMessageReceiptConfirmation)
        //                {
        //                    if (!string.IsNullOrEmpty(msg1.MessageId) && ReadFromWS && SaveSucceeded)
        //                        ConfirmMessageReceipt(msg.MessageId);
        //                    Console.WriteLine(Environment.NewLine + "Message receipt confirmed for message with ID: "
        //                        + msg.MessageId + Environment.NewLine);
        //                }
        //                else
        //                {
        //                    // Since the forced receipt confirmation is on, receipt has already been confirmed.
        //                    // Now confirmation is regular (because there were no errors), so we do not need to keep 
        //                    // the message file in the forced directory any more:
        //                    if (!string.IsNullOrEmpty(ForcedReceiptFileName))
        //                        if (File.Exists(ForcedReceiptFileName))
        //                            File.Delete(ForcedReceiptFileName);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                errorstring = ex.Message;
        //            }
        //            // If the messages could not be saved then we reject the message and confirm its receipt:
        //            if (ReadFromWS && !SaveSucceeded)
        //            {
        //                if (ControlOutputs)
        //                {
        //                    Console.WriteLine();
        //                    Console.WriteLine("Message is rejected due to critical errors.");
        //                    Console.WriteLine();
        //                }

        //                // Reject the message:
        //                string comment = null;
        //                comment = "Naročilo je zavrnjeno zaradi nekonsistentnosti podatkov.";
        //                if (errorstring != null)
        //                {
        //                    comment += Environment.NewLine + "Sporočila o napakah: "
        //                        + Environment.NewLine + errorstring;
        //                }
        //                SendOoNullifyToBis(msg, comment);
        //                // Confirm message receipt:
        //                if (!string.IsNullOrEmpty(msg1.MessageId))
        //                    ConfirmMessageReceipt(msg.MessageId);
        //            }
        //            // End: Message type: SpecimenObservationOrder
        //        }
        //        else if (msg1.Type == MessageType.Unknown)
        //        {
        //            if (!string.IsNullOrEmpty(msg1.MessageId))
        //                ConfirmMessageReceipt(msg1.MessageId);
        //            throw new Exception("A message of unknown type has been received. The message has been taken from the queue.");
        //        } else
        //        {
        //            if (!string.IsNullOrEmpty(msg1.MessageId))
        //                ConfirmMessageReceipt(msg1.MessageId);
        //            throw new NotImplementedException("Treatment is not implemented for the messages of the following type: "
        //                + msg1.Type);
        //        }

        //    }
        //}  // ReceiveAndProcessMessage





        // commit commit



        int
            PauseBetweenReads_ms = 5000,  // pause length between msg receipts
            PauseBeforeReads_ms = 400;    // pause before the first read is performed
        bool ControlOutputs = true;  // if true then we print crucial things about msg receiver to console

        /// <summary>Periodically receives messages from Calypso and works them.</summary>
        private void ReceiverThreadMain() { }
        
        //private void ReceiverThreadMain()
        //{
        //    if (ControlOutputs)
        //        Console.WriteLine(" ... started." + Environment.NewLine);
        //    Calypso ws = new Calypso();
        //    bool ReadFromWS = true;  // Set this to true in order to receive messages from the web service!
        //    // Verify whether the msg shouold be obtained read from the file rather than received from WS:
        //    if (!string.IsNullOrEmpty(Settings.MessageFile))
        //    {
        //        if (File.Exists(Settings.MessageFile))
        //            ReadFromWS = false;
        //        else if (ControlOutputs)
        //        {
        //            Console.WriteLine("Test MESSAGE FILE is specified but it DOES NOT EXIST. File name: "
        //                + "  " + Settings.MessageFile);
        //        }
        //    }
        //    // Parameters for the case when the msg is read from file (used for testing, but should stay in the code):
        //    int numread = 0, maxread = 0;
        //    maxread = Settings.MaxNumMessages;
        //    if (!ReadFromWS)
        //    {
        //        if (maxread > -1)
        //            maxread = 1;  // when reading from a file, at most one message is read
        //    }
        //    if (ControlOutputs)
        //    {
        //        // Print some data on console
        //        if (ReadFromWS)
        //        {
        //            if (maxread == 1)
        //                Console.WriteLine("Only one message will be read.");
        //            else if (maxread < 0)
        //                Console.WriteLine("Mo messages will be read.");
        //            else if (maxread == 0)
        //                Console.WriteLine("Unlimited number of messages will be read.");
        //            else
        //                Console.WriteLine("At most " + maxread.ToString() + " messages will be read.");
        //        }
        //        else if (!string.IsNullOrEmpty(Settings.MessageFile))
        //            Console.WriteLine("One message will be read from the following file: " + Settings.MessageFile);

        //    }

        //    // Give the system some time before starting to read messages:
        //    Thread.Sleep(PauseBeforeReads_ms);

        //    while (numread < maxread || maxread == 0 /* unlimited */)
        //    {
        //        try
        //        {
        //            ++numread;
        //            ReceiveAndProcessMessage(ReadFromWS);
        //        }
        //        catch (Exception ex)
        //        {
        //            R.ReportError("An error occurred in Bis message receiver.", ex);
        //        }
        //        Thread.Sleep(PauseBetweenReads_ms); // give system some time after the msg has been treated (to reduce the load)
        //    }
        //}


        private bool ReceiverRun = false;  // Indicates that the receiver thread has already been run.

        /// <summary>Performs some tests after the receiver has been started.
        /// This function is here for debuggind purposes. It should be called after the receiver is started,
        /// and it can be empty in the release version of the application.</summary>
        public void TestAfterReceiverStarted()
        {
        }


        ///// <summary>Starts a new thread that continuously receives messages from the Calypso server and works them.</summary>
        ///// <param name="om">Reference to object manager, necessary for instantiation of Pado classes</param>
        ///// <param name="messagefile">Message file - in the case that msg is read from a file (for testing) instead from
        ///// a web service.</param>
        //public void StartReceiverThread(
        //    Premisa.PadoServerClasses.clsPadoObjectManager om /*,
        //    BisCommunicationSettings bissettings */ )
        //{

        //    //string filename = om.Server.Settings["FilePath"].Value;


        //    if (ReceiverRun)
        //    {
        //        R.ReportError("Calypso.StartReceiver: thread has already been started. Please shut down the server.");
        //        throw new Exception("Calypso message receiver thread has already been started.");
        //    }

        //    // Make object manager accessible to the started thred:
        //    ObjectManager = om;

        //    if (Settings.DebugCalypsoReceiver)
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine();
        //        Console.WriteLine("DebugCalypsoReceiver is on");
        //        Console.WriteLine();
        //        Console.WriteLine("Press <Enter> in order to continue server initialization!");
        //        Console.ReadLine();
        //        Console.WriteLine();
        //    }

        //    // Create the receiver thread with lower priority and start it:
        //    Thread ReceiverThread = new Thread(new ThreadStart(ReceiverThreadMain));
        //    ReceiverThread.Priority = ThreadPriority.BelowNormal;
        //    ReceiverThread.IsBackground = true;
        //    ReceiverRun = true;

        //    Console.Write(Environment.NewLine + "Starting Calypso message receiver... ");
        //    ReceiverThread.Start();
        //}



        #endregion  // MessageReceiver

        #region TestWebService


        /// <summary>Returns true if the communication web service is responding, and false if it is not.</summary>
        /// <param name="Id">Output parameter through which a unique Id returned by the web service is obtained
        /// when the web service is responding properly.</param>
        /// <returns></returns>
        public bool TestWs(out string Id)
        {
            Id=null;
            try
            {
                Id = GetUniqueID();
            }
            catch { }
            if (string.IsNullOrEmpty(Id))
                return false;
            else
                return true;
        }

        /// <summary>Returns true if the communication web service is responding, and false if it is not.</summary>
        public bool TestWs()
        {
            string Id;
            return TestWs(out Id);
        }

        #endregion  // WSTest


        #region WSGet

        /// <summary>Gets a unique ID from the Calypso web service and returns it.</summary>
        /// <returns>The ID received from the web service.</returns>
        public string GetUniqueID()
        {
            string Id = null;
            try
            {
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("Calypso.GetUniqueID", "Started...");
                //LabexBis.WebReference.CalypsoWSNSService service = new LabexBis.WebReference.CalypsoWSNSService();
                //service.Url = Settings.MessageServiceUrl;
                Id = Service.getUniqueID();
                return Id;
            }
            catch (Exception ex)
            {
                R.ReportError(ex, "Calypso (web service).GetUniqueId");
                throw ReporterBase.ReviseException(ex, "Web service error: ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Calypso.GetUniqueID", "Finished. ID: \"" + Id == null ? "<null>" : Id + "\".");
                --R.Depth;
            }
        }


        /// <summary>Sends a msg that is contained in msg, to the Labex queue.
        /// If necessary then the msg is modified such that the right receiver is set in it (i.e. Labex)</summary>
        /// <param name="msg">Xml document containing the msg to be sent.</param>
        /// <param name="msgid">Message Id.</param>
        public void SendMessageToMyself(string message, string msgid)
        {
            SendMessage(message, ReceiverLabex, msgid);
        }

         /// <summary>Sends a msg that is contained in msg, to the Bis queue.
        /// If necessary then the msg is modified such that the right receiver is set in it (i.e. Bis)</summary>
        /// <param name="msg">Xml document containing the msg to be sent.</param>
        /// <param name="msgid">Message Id.</param>
       public void SendMessageToBis(string message, string msgid)
        {
            SendMessage(message, ReceiverBis, msgid);
        }


        /// <summary>Sends a msg to the specified receiver through the calypso WS.</summary>
        /// <param name="msg">Xml document containing the msg to be sent.</param>
        /// <param name="receiver">Code of the application that will receive the msg.</param>
        /// <param name="msgid">Message Id.</param>
        public void SendMessage(string message, string receiver, string msgid)
        {
            SendMessage(HL7Version, message, receiver, msgid);
        }

        /// <summary>Sending a msg to WS with HL7 version specification.</summary>
        /// <param name="hl7version">Version of the HL7 specification.</param>
        /// <param name="msg">Xml document containing the msg to be sent.</param>
        /// <param name="receiver">Code of the application that will receive the msg.</param>
        /// <param name="msgid">Message Id.</param>
        private void SendMessage(string hl7version, string msg, string receiver, string msgid)
        {
            try
            {
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "Started, receiver: " + receiver + "...");

                if (string.IsNullOrEmpty(msg))
                    throw new ArgumentException("Sending a message: Message is not specified.");
                if (string.IsNullOrEmpty(receiver))
                    throw new ArgumentException("Sending a message: receiver is not specified.");
                // If necessary, change the receiver written in the msg:
                bool changed = false;
                MsgBase.SetMessageReceiver(ref msg, receiver, out changed);
                //LabexBis.WebReference.CalypsoWSNSService service = new LabexBis.WebReference.CalypsoWSNSService();
                //service.Url = Settings.MessageServiceUrl;

                // $$$$
                try
                {

                    // Write the msg to a file:
                    if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "Before writing message to a file...");
                    string filepath = Settings.OutgoingFileName(msgid);  // GetMessageFilePath(msg);
                    if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "File name: " + filepath);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(msg);
                    doc.Save(filepath);

                    //using (TextWriter tw = new StreamWriter(filepath,false) )
                    //{
                    //    if (msg != null)
                    //        tw.Write(msg);
                    //}
                    if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "After writing message to a file.");

                    // Send the msg:
                    if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "Before sending the messsage...");

                    Service.sendMessage(msg, HL7Version);
                    
                    if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "Sending done.");

                }
                catch (Exception ex)
                {
                    R.ReportError("Error in writing the received message to a file.",ex);
                    throw ex;
                }
                
            }
            catch (Exception ex)
            {
                R.ReportError("Error in sending a message to the calypso server, receiver = " + receiver + ".", ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Calypso.SendMessage", "Message has been sent to "
                    + receiver + ".");
                --R.Depth;
            }
        }



        /// <summary>Sends nullification message for the specified Observation order, to the Bis queue.
        /// Original observation order message (as stored in the database) is modified in such a way that the right 
        /// action code, receiver, sender and responder are set in it, and the specified comment is set in the message.
        ///  Original message XML is obtained from the message table through the ID in tbl01_RSR.</summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IdRsR"></param>
        /// <param name="comment"></param>
        public void SendOoNullifyToBis(string connectionstring, int IdRsr)
        {
            R.ReportInfo("LabexBis.SendOoNullifyToBis", "Started...");
            string 
                commandstrMessage = null,
                commandstrComment = null,
                MessageXml = null,
                MessageComment = null;
            try
            {
                commandstrMessage = "SELECT  dbo.fn_ObservationOrderXml(@IdRsr) ; ";
                commandstrComment = "SELECT NOTE_STORNO FROM tbl01_RSR WHERE ID_RSR = @IdRsr ; ";
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        connection.Open();
                        // Get original observation order message (an XML string) from the database:
                        SqlCommand command = new SqlCommand(
                            commandstrMessage, connection);
                        command.Parameters.Add("@IdRsR", SqlDbType.Int);
                        command.Parameters["@IdRsR"].Value = IdRsr;
                        MessageXml = (string) command.ExecuteScalar();
                        // Get the comment (storno note) to be sent in the message:
                        command = new SqlCommand(
                            commandstrComment, connection);
                        command.Parameters.Add("@IdRsR", SqlDbType.Int);
                        command.Parameters["@IdRsR"].Value = IdRsr;
                        // connection.Open();
                        MessageComment = (string) command.ExecuteScalar();
                    }
                    catch (Exception ex2)
                    {
                        R.ReportError(ex2);
                        throw ex2;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                if (string.IsNullOrEmpty(MessageXml))
                    throw new DataException("The original observation order message could not be obtained for observation (research) ID " 
                        + IdRsr.ToString() + "." );
                else
                {
                    // Now we have the original observation order message, modify it appropriately *according to 
                    // nullify specification) and send it to the communication web service:
                    SendOoNullifyToBis(MessageXml, "Nullify_ID_RSR_" + IdRsr.ToString(), MessageComment);
                }
            }
            catch (Exception ex)
            {
                R.ReportError("RacunExport.GetRacunStatus", null, ex);
                throw ex;
            }
            finally
            {
                R.ReportInfo("LabexBis.SendOoNullifyToBis","Finished.");
            }
        }


        /// <summary>Sends nullification message for the Observation order that is contained in msg, to the Bis queue.
        /// msg is modified in such a way that the right action code, receiver, sender and responder are set in it,
        /// and the specified comment is set.</summary>
        /// <param name="msg">Xml document containing the original observation order to be nullified.</param>
        /// <param name="msgid">Message Id.</param>
        /// <param name="comment">Comment that is set in the message.</param>
        public void SendOoNullifyToBis(MsgObervationOrder msg, string comment)
        {
            XmlDocument doc = msg.Doc;
            // Prepare data on the message:
            MsgObervationOrder.PrepareOoNullify(ref doc, comment);
            string MessageId = msg.MessageId;  // this is needed only to form the name of the file where message is stored
            if (string.IsNullOrEmpty(MessageId))
            {
                MessageId = msg.MessageNumber;
                if (string.IsNullOrEmpty(MessageId) && msg.Type == MessageType.SpecimenObservationOrder)
                {
                    MsgObervationOrder msgorder = msg as MsgObervationOrder;
                }
            }
            SendMessageToBis(doc.OuterXml, MessageId);
        }

        /// <summary>Sends nullification message for the Observation order that is contained in message, to the Bis queue.
        /// message is modified in such a way that the right action code, receiver, sender and responder are set in it,
        /// and the specified comment is set.</summary>
        /// <param name="message">Xml document containing the original observation order to be nullified.</param>
        /// <param name="msgid">Message Id. Needed to form the file name where message is stored before sending.</param>
        /// <param name="comment">Comment that is set in the message.</param>
        public void SendOoNullifyToBis(string message, string msgid, string comment)
        {
            MsgObervationOrder.PrepareOoNullify(ref message, comment);
            SendMessageToBis(message, msgid);
        }



        /// <summary>Sends confirmation back to the WS massage queue that the msg with the specified ID
        /// has been received and treated and that it doed not need to be kept in the queue any more.</summary>
        /// <param name="Id">Id of the received msg (as obtained from the web service).</param>
        public void ConfirmMessageReceipt(string Id)
        {
            ConfirmMessageReceipt(Id, QueueNameLabex);
        }

        /// <summary>Sends confirmation back to the WS massage queue that the msg with the specified ID
        /// has been received and treated and that it doed not need to be kept in the queue any more.</summary>
        /// <param name="Id">Id of the received msg (as obtained from the web service).</param>
        /// <param name="queuwName">Name of the queue from which the msg has been picked.</param>
        public void ConfirmMessageReceipt(string Id, string queueName)
        {

            ////REMARK: you can enable this method by disabling the statement below!
            //if (false)
            //{
            //    R.ReportWarning("Calypso.ConfirmMessageReceipt", "Confirmation of the msg receipt is skipped for testing purposes.");
            //    return;
            //}

            try
            {
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("Calypso.ConfirmMessageReceipt", "Started, Id = " + Id + "...");
                //LabexBis.WebReference.CalypsoWSNSService service = new LabexBis.WebReference.CalypsoWSNSService();
                //service.Url = Settings.MessageServiceUrl;
                Service.ackMessage(Id, queueName);
            }
            catch (Exception ex)
            {
                R.ReportError("Error in confirning message receipt, Id = " + Id + ".", ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Calypso.ConfirmMessageReceipt", "Finished. Message Id: "
                    + Id + ".");
                --R.Depth;
            }

            // throw new NotImplementedException("Receipt confirmation is not yet implemented.");
        }


        /// <summary>Saves the msg Xml to a file. File name is determined according to seddings.
        /// No GUID is added to file name.</summary>
        /// <param name="msg">Message to be saved.</param>
        public void SaveIncomingMessage(MsgBase msg)
        {
            if (msg == null)
                throw new ArgumentNullException("Can not save the mesage to a file, message is not specified (null reference).");
            string filename = Settings.IncomingFileName(msg.MessageId);
            SaveIncomingMessage(msg,filename);
        }

        /// <summary>Saves the msg Xml to a file. File name is determined according to seddings.</summary>
        /// <param name="msg">Message to be saved.</param>
        /// <param name="addGuid">If true then GUID is added to file name.</param>
        public void SaveIncomingMessage(MsgBase msg, bool addGuid)
        {
            if (msg == null)
                throw new ArgumentNullException("Can not save the mesage to a file, message is not specified (null reference).");
            string filename = Settings.IncomingFileName(msg.MessageId, addGuid);
            SaveIncomingMessage(msg,filename);
        }

        /// <summary>Saves the msg Xml to a file with a specified name.</summary>
        /// <param name="msg">Message object whose Xml is saved.</param>
        /// <param name="filename">Name of a file into which the object is saved.</param>
        public void SaveIncomingMessage(MsgBase msg, string filename)
        {
            if (msg == null)
                throw new ArgumentNullException("Can not save the mesage to a file, message is not specified (null reference).");
            if (msg.MessageXml == null)
                throw new ArgumentNullException("Can not save the mesage to a file, message contents is not specified.");
            if (string.IsNullOrEmpty(filename))
                throw new Exception("Name of the file to save the message contents in is not specified.");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg.MessageXml);
            doc.Save(filename);
        }


        /// <summary>Receives the last msg from the Calypso web service and returns msg object.</summary>
        public MsgBase GetMessage()
        {
            return GetMessage(HL7Version);
        }

        /// <summary>Receives the last msg from the Calypso web service and returns msg object. 
        /// The message is also storedto a file whose name is determined from the msg ID.
        /// Version of the standard must also be specified.</summary>
        protected MsgBase GetMessage(string hl7version)
        {
            MsgBase msg = null;
            string Id = null, MessageText = null, filepath = null;
            GetMessage(hl7version, out Id, out MessageText);
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "Before writing message to a file...");
                    filepath = Settings.IncomingFileName(Id); 
                    if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "File name: " + filepath);
                    using (TextWriter tw = new StreamWriter(filepath, false))
                    {
                        if (MessageText != null)
                            tw.Write(MessageText);
                    }
                    if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "After writing message to a file.");
                }
            }
            catch (Exception ex)
            {
                R.ReportError("Error in writing the received message to a file.", ex);
                throw ex;
            }
            MessageType msgtype = MsgBase.GetMessageType(MessageText);
            switch (msgtype)
            {
                case MessageType.SpecimenObservationOrder:
                    msg = new MsgObervationOrder();
                    break;
                case MessageType.SpecimenObservationEvent:
                    msg = new MsgObservationEvent();
                    break;
                case MessageType.DetailedFinancialTransaction:
                    msg = new MsgFinancialTransaction();
                    break;
                case MessageType.Unknown:
                    throw new XmlException("Type of the message could not be determined.");
                default:
                    throw new NotImplementedException("Messages of the following type can not be received (not supported by the system): "
                        + msgtype.ToString());
            }
            if (msg != null)
            {
                msg.MessageId = Id;
                msg.MessageXml = MessageText;
                msg.MessageFile = filepath;
            }
            return msg;
        }

        /// <summary>Receives a msg from the Calypso web service and returns msg Id and Text.</summary>
        /// <param name="Id">messagestr ID that is a part of the received msg.</param>
        /// <param name="MessageText">messagestr itself.</param>
        public void GetMessage(out string Id, out string MessageText)
        {
            GetMessage(HL7Version, out Id, out MessageText);
        }

        /// <summary>Receives a msg from the Calypso web service and returns msg Id and Text.</summary>
        /// <param name="hl7version">Version of the HL7 standard according to which the msg is assembled.</param>
        /// <param name="Id">messagestr ID that is a part of the received msg.</param>
        /// <param name="MessageText">messagestr itself.</param>
        protected void GetMessage(string HL7Version, out string Id, out string MessageText)
        {
            try
            {
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage()", "Started...");
                Id = null;
                MessageText = null;
                LabexBis.WebReference.Message msg = null;
                if (!string.IsNullOrEmpty(HL7Version))
                    msg = GetNextMessage(HL7Version);
                else
                    msg = GetNextMessage();
                Id = msg.ID;
                MessageText = msg.text;
            }
            catch(Exception ex)
            {
                R.ReportError(ex);
                throw (ex);
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Receives a new msg from the Calypso web service and returns it. The msg is also stored
        /// to a file whose name is determined from the msg ID.</summary>
        /// <returns>The msg received.</returns>
        public LabexBis.WebReference.Message GetNextMessage()
        {
            return GetNextMessage(HL7Version);
        }

        /// <summary>Receives a new msg from the Calypso web service and returns it.</summary>
        /// <param name="hl7version">The version of the HL7 standard used for messages.</param>
        /// <returns>The msg received.</returns>
        protected LabexBis.WebReference.Message GetNextMessage(string hl7version)
        {
            LabexBis.WebReference.Message msg = null;
            string msgid = null;
            try
            {
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "Started...");
                //LabexBis.WebReference.CalypsoWSNSService service = new LabexBis.WebReference.CalypsoWSNSService();
                //service.Url = Settings.MessageServiceUrl;
                if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "Before calling the web service method...");                
                if (string.IsNullOrEmpty(hl7version))
                    hl7version = HL7Version;
                msg = Service.receiveMessage(QueueNameLabex, HL7Version);
                if (msg != null)
                    msgid = msg.ID;
                // msgid = (msg == null ? "" : (msg.ID == null ? "" : msg.ID));  // store msg Id for construction of file names
                if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "After calling the web service method. Message Id: "
                    + msgid );
                if (msg==null)
                    throw new Exception("The received message is empty (null reference).");
                if (string.IsNullOrEmpty(msg.ID))
                    throw new Exception("Message ID is not specified (null reference).");
                if (string.IsNullOrEmpty(msg.text))
                    throw new Exception("Message is not specified (null reference); Message ID: \"" + msg.ID + "\".");
                return msg;
            }
            catch (Exception ex)
            {
                R.ReportError(ex, "Calypso (web service).GetMessage");
                throw ReporterBase.ReviseException(ex, "Web service error: ");
            }
            finally
            {
                if (msg == null)
                {
                    if (R.TreatWarning) R.ReportWarning("Calypso.GetMessage", "Finished. Message is null.");
                }
                else
                {
                    if (msg.ID == null)
                    {
                        if (R.TreatInfo) R.ReportWarning("Calypso.GetMessage", "Finished. Message ID is null.");
                    }
                    else
                    {
                        if (R.TreatInfo) R.ReportInfo("Calypso.GetMessage", "Finished. Message Id: " + msg.ID);
                    }
                }
                --R.Depth;
            }
        }

        #endregion  // WSGet


        #region IDisposable Members

        public void Dispose() {}
        //{
        //    if (_dbCmdInitialized)
        //    {
        //        if (mDbCnn != null && mDbCnn.State == System.Data.ConnectionState.Open) { mDbCnn.Close(); }
        //        if (mDbCmd != null) { mDbCmd.Dispose(); }
        //        _dbCmdInitialized = false;
        //    }
        //}

        #endregion
    }  // class Calypso



 

}  // namespace LabexBis


