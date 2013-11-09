
    // CONSTANTS USED IN INTERPRETATION AND CREATION OF MESSAGES for communication with Bis.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;
// using LabexUtilities;

namespace IG.Lib
{


    public enum MessageType
    {
        Unknown = 0, SpecimenObservationOrder, SpecimenObservationEvent,
        PersonRegistry, DetailedFinancialTransaction
    }

    public enum ObservationType { Unknown = 0, Pathological, Histological, Cytological }

    public enum OoStatus { Unknown = 0, Active, Completed, Nullified }

    // Remark: action enumerators must be synchronized withh table!

    public enum OoAction {  Unknown = 0, Create = 1, Revise = 2, Nullify = 3  }

    public enum OeAction { Unknown = 0, Activate = 11, Complete = 12, ReviseCompleted = 13 }

    // public enum FtAction { Unknown = 0 }

    public enum ActionPriority { Unknown = 0, Routine, Urgent, Critical  }

    // Message status (this enum. must be synchronized with the tbl02_MSG_STATUS table):
    public enum BisMessageStatus { NotReady = 0,  // incoming, outgoing
        Arrived = 1,                 // incoming
        ReceiptConfirmed = 2,        // incoming
        IncomingRejected = 5,        // incoming
        Prepared = 10,               // outgoing
        Sent = 11,                   // outgoing
        ConfirmationObtained = 12,   // outgoing
        OutgoingRejected = 15        // outgoing
    }



    /// <summary>Common Constants related to messages sent with BIS.
    /// Remark: class MsgConst is composed of different part in order to make it easily scanned.</summary>
    public abstract class MsgConstCommon
    {

        #region Constants
        // General constants common for all messages:

        #region GeneralCodes

        // Constants used in conversions or for encoding specific meaning:

        public string
            ActionPriorityRoutine = "R",  // routine
            ActionPriorityUrgent = "UR",  // urgent
            ActionPriorityCritical = "T",  // time critical

            LeftSideCode = "L",  // denotes left-hand side
            RightsideCode = "D",   // denotes right-hand side
            
            ObservationTypePatology = "PATO",
            ObservationTypeHistology = "HISLAB",
            ObservationTypeCitology = "CIT",
            
            MissingDataString = "NP";  // String that is assigned to properties that are not specified in messages 
                        // but are obligatory in principle (such as patient name, for example). The system must allow
                        // in some cases that such data is not specified.


        // Disgnoses - related constants:
        public string DiagnosisCodeSystem = "MKB10";

        public int  // Diagnosis type codes, must be syncgronized with the table tbl01_DGN_TYPE
            DiagnosisTypeClinical = 1,
            DiagnosisTypeHistopatological = 2;


        public int GenderMale = 1, GenderFemale = 2;  // To je spol za tabele tbl02.
        public readonly DateTime NullDateTime = new DateTime(1990, 1, 1);  // Datetime that is used in place of null. Do not change this setting!
        public int NullInt = -1;  // Number that is used in place of null. Do not change this setting!
        public bool NullBool = false;  // Boolean that is used in place of null. Do not change this setting!


        public string

            LabexId = "Labex",
            LabexIdOid = "1.2.3.4.1000.61.x.1",
            BisId = "b21App",
            // BisId = "b21",
            BisIdOid = "1.2.3.4.1000.60.x.1",


            // location where receiver is set in order to send the msg:
            ReceiverPath = "/*/communicationFunctionRCV/deviceRCV/id",
            SenderPath = "/*/communicationFunctionSND/deviceSND/id",
            ResponderPath = "/*/communicationFunctionRSP/deviceRSP/id",
            SenderReceiverAttribute = "extension";

        // Elements and attributes for specific data common to different msg:
        public string
            OoRootName = "MCCI_MT000100HT03.Message",  // Root element name for SpecimenObservationOrder
            OeRootName = "MCCI_MT000100HT02.Message",  // Root element name for SpecimenObservationEvent
            FtRootName = "MCCI_MT000100HT03.Message",  // Root element name for DetailedFinancialTransaction
            FtAction = "FIAB_TE000001",

            OoMessageContainer = "MCAI_MT000001HT03.ControlActEvent",  // msg container element name
            OeMessageContainer = "MCAI_MT000001HT02.ControlActEvent",  // msg container element name
            FtMessageContainer = "MCAI_MT000001HT03.ControlActEvent",  // msg container element name for DetailedFinancialTransaction

            OoMessageSubContainer2 = "POXX_MT121000HT02.SpecimenObservationOrder",  // sub-subcontainer name
            OeMessageSubContainer2 = "POXX_MT111000HT01.SpecimenObservationEvent",  // sub-subcontainer name
            FtMessageSubContainer2 = "FIAB_MT020000HT01.FinancialTransaction",  // sub-subcontainer name for DetailedFinancialTransaction

            ExtensionAttribute = "extension",

            IdElement = "id",   // name of various Id elements
            IdIdAttribute = "extension",  // name of Id attribute of Id elements
            IdOidAttribute = "root",     // name of OID attribute of Id elements

            ValueAttribute = "value",  // value attribute for various elements

            ReceiverContainer = "communicationFunctionRCV",  // name of the element containing msg receiver information
            ReceiverSubContainer = "deviceRCV",
            ResponderContainer = "communicationFunctionRSP",  // name of the element containing msg responder information
            ResponderSubContainer = "deviceRSP",
            SenderContainer = "communicationFunctionSND",  // name of the element containing msg sender information
            SenderSubContainer = "deviceSND",

            MessageSubContainer = "subject",  // subcontainer name
            MessageSubContainerTypeAttribute = "type",
            MessageSubContainerTypeAttributeValue = "ActRelationship",
            MessageTypeAttribute = "type",

            CodeElement = "code",
            CodeAttribute = "code",
            CodeSystemAttribute = "codeSystemName",
            CodeDescriptionAttribute = "displayName";

        public int
            OoGenderMale = 1,
            OoGenderFemale = 2;


        #endregion GeneralCodes

        #endregion  // Constants

    }  // MsgConstCommon


 
    /// <summary>Constants related to the ObservationOrder msg.
    /// Remark: class MsgConst was composed of different parts in order to make it easily scanned.
    /// Constants are used in utilities needed for interpretation and construction  of messages.</summary>
    public abstract class MsgConstObservationOrder : MsgConstCommon
    {


        IReporter _R = null;

        /// <summary>Reporter for this class.</summary>
        public virtual IReporter R
        {
            get
            {

                // TODO: Change this!

                if (_R == null)
                    _R = new ReporterConsole();
                return _R;
            }
        }



        // Elements and attributes for specific data in SpecimenObservationOrder msg:

        #region Constants

        #region Instructions

        // Here you can specify how parsing and building behaves (e.g. switch of certain checks)

        public bool
            OoCheckAliveConsistency = false; // testing consistency of data with mortal status

        public bool OoCheckAll
        {
            set
            {
                OoCheckAliveConsistency = value;
            }
        }

        #endregion Instructions

        #region Codes

        /// <summary>Name of the Root XML node for all messages.</summary>
        public string
            CreationTimeElement = "creationTime",  // creation time element
            CreationTimeAttribute = "value",
            ActionElement = "interactionId",   // action specification
            ActionOidAttribute = "root",
            ActionAttribute = "extension",

            OoOrderStatusElement = "statusCode", OoOrderStatusAttribute = "code",  // order status
            OoCommentElement = "text",
            OoOrderTimeElement = "effectiveTime",
            OoOrderTimeAttribute = "value",
            OoActionPriorityElement = "priorityCode",
            OoActionPriorityCodeAttribute = "code",
            OoActionPriorityCodeSystemAttribute = "codeSystemName",

            OoSampleContainer = "subject",
            OoSampleSubContainer = "Specimen",

            OoSampleDataContainer = "productOf",
            OoSampleDataSubContainer = "procedureEventSpecimenCollection",

            OoSampleStatuscodeElement = "statusCode",

            OoSampleFixativeElement = "methodCode",

            OoSampleSideElement = "approachSiteCode",

            OoSampleOrganElement = "targetSiteCode",

            OoSampleTimeElement = "effectiveTime",

            OoSampleNumPiecesContainer = "specimenMaterialChoice",
            OoSampleNumPiecesSubContainer = "Material",
            OoSampleNumPiecesElement = "quantity",
            OoSampleNumPiecesAttribute = "value",



            OoPatientContainer = "recordTarget",
            OoPatientSubContainer = "Patient",
            OoPatientDataContainer = "Person",

            OoPatientNameContainer = "name",
            OoPatientSurnameElement = "family",
            OoPatientNameElement = "given",

            OoPatientGenderElement = "administrativeGenderCode", OoPatientGenderAttribute = "code",
            OoPatientBirthDateElement = "birthTime",
            OoPatientDeceasedElement = "deceasedInd",
            OoPatientTimeOfDeathElement = "deceasedTime",

            OoAddressContainter = "addr",
            OoCountryCodeElement = "country",  // Currently, there is no country name in the messages (so we must have a fixed table of countries)
            OoCityElement = "city",
            OoPostalCodeElement = "postalCode",
            OoStreetAddressElement = "streetAddressLine",

            OoPatientBirthPlaceContainer = "birthplace",
            OoPatientBirthPlaceSubcontainer = "Place",
            OoPatientBirthPlaceElement = "name",

            OoPatientHealthInsuranceContainer = "playedIdentifications",
            OoPatientHealthInsuranceElement = "id",

            OoAuthorContainer = "author",
            OoAuthorSubcontainer = "AssignedEntity",

            OoAuthorOrgContainer = "Organization",
            OoAuthorOrgNameElement = "name",

            OoAuthorDataContainer = "assignedEntityChoice",
            OoAuthorDataSubcontainer = "Person",

            OoAuthorNameContainer = "name",
            OoAuthorPrefixElement = "prefix",
            OoAuthorSurnameElement = "family",
            OoAuthorNameElement = "given",
            OoAuthorSuffixElement = "suffix",

            // Medical data - containing various fields:
            OoMedicalDataContainer = "support",
            OoMedicalDataSubContainer = "ObservationEventGeneral",
            OoMedicalDataKindElement = "code", // element, attribute and its value that introduce the 
            OoMedicalDataKindAttribute = "code",   // diagnosis element (either diag. code or Text)
            OoMedicalDataElement = "value",
            OoMedicalDataTypeAttribute = "dataType",
            OoMedicalDataCodeAttribute = "code",
            OoMedicalDataCodeSystemAttribute = "codeSystemName",
            OoMedicalDataValueAttribute = "value",

            // Diagnosis free Text:
            OoDiagnosisTextDataKind = "NAPDIAG",
            OoDiagnosisTextDataType = "ST",

            // Diagnosis code 
            OoDiagnosisCodeDataKind = "NAPDIAG",
            OoDiagnosisCodeDataType = "CE",

            // Fields for obduction declaration:

            OoMedicalDataTypeCode = "CE",
            OoMedicalDataTypeText = "ST",

            OoAutopsyDeathReasonDataKind = "NVZS",
            OoObductionDeathReasonDataType = "ST",
            OoAutopsyDeathReasonTitle = "Neposredni vzrok smrti",

            OoAutopsyBasicDiseaseDataKind = "OOB",
            OoObductionBasicDiseaseDataType = "ST",
            OoAutopsyBasicDiseaseTitle = "Osnovno obolenje / bolezen",

            OoAutopsyAssociatedDiseaseDataKind = "PRBOL",
            OoObductionAssociatedDiseaseDataType = "ST",
            OoAutopsyAssociatedDiseaseTitle = "Pridružene bolezni",

            OoAutopsyOtherInformationDataKind = "QINF",
            OoObductionOtherInformationDataType = "ST",
            OoAutopsyOtherInformationTitle = "Vprašanja, druge informacije";

        // SpecimenObservationOrder action codes:
        public string
            OoActionCreate = "POXX_HN121002",
            OoActionRevise = "POXX_HN121011",
            OoActionNullify = "POXX_HN121009",
            OoCodeCreate = "POXX_TE121002",
            OoCodeRevise = "POXX_TE121011",
            OoCodeNullify = "POXX_TE121009";

        // Order status codes:
        public string
            OoOrderStatusCompleted = "completed",
            OoOrderStatusNullified = "nullified",
            OoOrderStatusActive = "active";

        // SpecimenObservationOrder type code:
        public string
            OoMessageType = "Observation";  // messagestr type code for SpecimenObservationOrder

        #endregion  // Codes

        #endregion  // Constants




        /// <summary></summary>
    } // class MsgConstObservationOrder


    /// <summary>Constants related to the ObservationEvent msg.
    /// Remark: class MsgConst was composed of different parts in order to make it easily scanned.
    /// Constants are used in utilities needed for interpretation and construction of messages.</summary>
    public abstract class MsgConstObservationEvent : MsgConstObservationOrder
    {


        #region Constants


        #region Instructions

        #endregion  // Instructions




        // SpecimenObservationEvent action codes:
        public string 
            OeActionActivate = "POXX_HN111003",
            OeActionComplete = "POXX_HN111004",
            OeActionReviseCompleted = "POXX_HN111005",

            OeCodeActivate = "POXX_TE111003",
            OeCodeComplete = "POXX_TE111004",
            OeCodeReviseCompleted = "POXX_TE111005",

            OeAttachmentContainer = "attachment",
            OeAttachmentContainerTypeAttribute = "type",
            OeAttachmentContainerTypeAttributeValue = "Attachment",
            OeAttachmentElement = "text",

            // MessageSubContainer = "subject",  // subcontainer name
            // MessageSubContainerTypeAttribute = "type",
            // MessageSubContainerTypeAttributeValue = "ActRelationship",
            //MessageTypeAttribute = "type",

            OeCommentElement = "text",

            OeTimeContainer = "effectiveTime",
            OeOrderCreationTimeElement = "low",   // time of creation of order in Labex
            OeOrderCompletionTimeElement = "high",   // time of creation of event (i.e. the current msg)

            OePatientContainer = "recordTarget",
            OePatientSubcontainer = "Patient",

            OeAuthorContainer = "author",
            OeAuthorSubcontainer = "EmploymentStaff",

            OeVerifierContainter = "verifier",
            OeVerifierSubContainter = "EmploymentStaff",

            OeBisOrderIdContainter = "inFulfillmentOf",
            OeBisOrderIdSubContainter = "SpecimenObservationOrder",


            OeMessageType = "Observation";


        #endregion  // Constants


    } // class MsgConstObservationEvent


    /// <summary>Constants related to the ObservationEvent msg.
    /// Remark: class MsgConst was composed of different parts in order to make it easily scanned.
    /// Constants are used in utilities needed for interpretation and construction of messages.</summary>
    public abstract class MsgConstFinancialTransaction : MsgConstObservationEvent
    {

        #region Constants


        #region Instructions

        #endregion  // Instructions




        // SpecimenObservationEvent action codes:
        public string
            //FtActionActivate = "POXX_HN111003",
            //FtActionComplete = "POXX_HN111004",
            //FtActionReviseCompleted = "POXX_HN111005",

            //FtCodeActivate = "POXX_TE111003",
            //FtCodeComplete = "POXX_TE111004",
            //FtCodeReviseCompleted = "POXX_TE111005",

            // FtAction = "FIAB_TE000001",
            FtCode = "FIAB_TE000001",

            //FtAttachmentContainer = "attachment",
            //FtAttachmentContainerTypeAttribute = "type",
            //FtAttachmentContainerTypeAttributeValue = "Attachment",
            //FtAttachmentElement = "text",


            // FtCommentElement = "text",

            //FtTimeContainer = "effectiveTime",
            //FtOrderCreationTimeElement = "low",   // time of creation of order in Labex
            //FtOrderCompletionTimeElement = "high",   // time of creation of event (i.e. the current msg)

            FtEffectiveTimeElement = "effectiveTime",
            FtEffectiveTimeAttribute = "value",
            FtActivityTimeElement = "activityTime",
            FtActivityTimeAttribute = "value",

            FtAuthorContainer = "author",
            FtAuthorSubcontainer = "participant",
            FtAuthorSubcontainer2 = "identifiedParty",
            FtAuthorSubcontainer3 = "Person",

            FtServiceContainer = "component",
            FtServiceSubcontainer = "financialTransactionChargeDetail",

            FtServiceCodeElement = "code",
            FtServiceCodeAttribute = "code",

            FtServiceCodeDescriptionElement = "title",

            FtServiceStatusCodeElement = "statusCode",
            FtServiceStatusCodeAttribute = "code",

            FtServiceTimeContainer = "effectiveTime",
            FtServiceStartTimeElement = "low",
            FtServiceCompletionTimeElement = "high",
            FtServiceTimeAttribute = "value",

            FtServiceActivityTimeElement = "activityTime",
            FtServiceActivityTimeAttribute = "value",

            FtServiceQuantityElement = "unitQuantity",
            FtServiceQuantityAttribute = "value";

        #endregion Constants

    } // class MsgConstObservationEvent



    /// <summary>Constants related to msg for communication with BIS, 
    /// conversion utilities from and to formats used in messageds, etc.
    /// Constants are not defined static in order to enable corrections for different variants of the program.</summary>
    public class MsgConst : MsgConstFinancialTransaction
    {


        #region Formatting

        // Time formats:

        /// <summary>Converts a atring that represents date and time in the messages to a DateTime object.</summary>
        /// <param name="timestr">String that represents the time, format is "YYYYMMDDhhmmss".</param>
        /// <returns>Converted date and time object.</returns>
        public DateTime ConvertTime(string timestr)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.ConvertTime(string)", "Started...");
            try
            {
                if (string.IsNullOrEmpty(timestr))
                    throw new ArgumentNullException("Can not convert an empty string to date and time.");
                int year = 0, month = 0, dayofmonth = 0, hour = 0, minute = 0, second = 0;
                year = int.Parse(timestr.Substring(0, 4));
                month = int.Parse(timestr.Substring(4, 2));
                dayofmonth = int.Parse(timestr.Substring(6, 2));
                try
                {
                    hour = int.Parse(timestr.Substring(8, 2));
                    minute = int.Parse(timestr.Substring(10, 2));
                    second = int.Parse(timestr.Substring(12, 2));
                }
                catch { }
                DateTime ret = new DateTime(year, month, dayofmonth, hour, minute, second);
                return ret;
            }
            catch (Exception ex)
            {
                R.ReportWarning(ex,"Msg.ConvertTime(string)");
                throw ReporterBase.ReviseException(ex, "Msg.ConvertTime(string): ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.ConvertTime(string)", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>Converts a DateTime object to a string that represents date and time in the messages.</summary>
        /// <param name="t">Time to be converted to a string.</param>
        /// <returns>Converted date and time object string whose format is "YYYYMMDDhhmmss".</returns>
        public string ConvertTime(DateTime t)
        {
            return ConvertTime(t, true /* includetime */, false /* underscores */);
        }

        /// <summary>Converts a DateTime object to a string that represents date and time in the messages.
        /// Seconds are not included even if time is included.</summary>
        /// <param name="t">Time to be converted to a string.
        /// Seconds are not included even if time is included.</param>
        /// <param name="includetime">If true then time is also included, otherwise only date is included.</param>
        /// <returns>Converted date and time object string whose format is:
        /// "YYYYMMDDhhmmss" if includetime = true.
        /// "YYYYMMDD" if includetime = false.</returns>
        public string ConvertTime(DateTime t, bool includetime)
        {
            return ConvertTime(t, includetime, false /* underscores */);
        }



        /// <summary>Converts a DateTime object to a string that represents date and time in the messages.
        /// Seconds are not included even if time is included.</summary>
        /// <param name="t">Time to be converted to a string.</param>
        /// <param name="includetime">If true then time is also included, otherwise only date is included.</param>
        /// <param name="underscores"></param>
        /// <returns>Converted date and time object string whose format can be: 
        ///   "YYYYMMDDhhmmss" (includetime = true, underscores = false)
        ///   "YYYYMMDD" (includetime = false, underscores = false)
        ///   "YYYY_MM_DD_hh_mm_ss" (includetime = true, underscores = true)
        ///   "YYYY_MM_DD" (includetime = false, underscores = true)</returns>
        public string ConvertTime(DateTime t, bool includetime, bool underscores)
        {
            return ConvertTime(t, includetime, false /* includeseconds */, underscores);
        }



        /// <summary>Converts a DateTime object to a string that represents date and time in the messages.</summary>
        /// <param name="t">Time to be converted to a string.</param>
        /// <param name="includetime">If true then time is also included, otherwise only date is included.</param>
        /// <param name="includeseconds">If true then seconds time are also included in time.</param>
        /// <param name="underscores"></param>
        /// <returns>Converted date and time object string whose format can be: 
        ///   "YYYYMMDDhhmmss" (includetime = true, underscores = false)
        ///   "YYYYMMDD" (includetime = false, underscores = false)
        ///   "YYYY_MM_DD_hh_mm_ss" (includetime = true, underscores = true)
        ///   "YYYY_MM_DD" (includetime = false, underscores = true)</returns>
        public string ConvertTime(DateTime t, bool includetime, bool includeseconds, bool underscores)
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Msg.ConvertTime()", "Started...");
            try
            {
                int count = 0;
                if (t == null)
                    throw new ArgumentNullException("Time to convert is not specified (null reference).");
                StringBuilder sb = new StringBuilder();
                string str;
                // Print 4 digit year:
                str = t.Year.ToString();
                count = 4 - str.Length; if (count < 0) count = 0;
                sb.Append('0', count);  // prepend leading zeros
                sb.Append(str);
                // Print 2 digit month:
                str = t.Month.ToString();
                if (underscores)
                    sb.Append("_");
                count = 2 - str.Length; if (count < 0) count = 0;
                sb.Append('0', count);  // prepend leading zeros
                sb.Append(str);
                // Print 2 digit day of month:
                str = t.Day.ToString();
                if (underscores)
                    sb.Append("_");
                count = 2 - str.Length; if (count < 0) count = 0;
                sb.Append('0', count);  // prepend leading zeros
                sb.Append(str);
                if (includetime)
                {
                    // Print 2 digit hour:
                    str = t.Hour.ToString();
                    if (underscores)
                        sb.Append("_");
                    count = 2 - str.Length; if (count < 0) count = 0;
                    sb.Append('0', count);  // prepend leading zeros
                    sb.Append(str);
                    // Print 2 digit minutes: 
                    str = t.Minute.ToString();
                    if (underscores)
                        sb.Append("_");
                    count = 2 - str.Length; if (count < 0) count = 0;
                    sb.Append('0', count);  // prepend leading zeros
                    sb.Append(str);
                    if (includeseconds)
                    {
                        // Print 2 digit seconds:
                        str = t.Second.ToString();
                        if (underscores)
                            sb.Append("_");
                        count = 2 - str.Length; if (count < 0) count = 0;
                        sb.Append('0', count);  // prepend leading zeros
                        sb.Append(str);
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ReporterBase.ReviseException(ex, "Msg.ConvertTime(): ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Msg.ConvertTime()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Returns a timestamp for the Current time. 
        /// The string is produced by ConvertTime() called with the Current time.</summary>
        public string TimeStamp(bool includetime, bool underscores)
        {
            return ConvertTime(DateTime.Now, includetime, underscores);
        }

        /// <summary>Returns a timestamp for the Current time. 
        /// The string is produced by ConvertTime() called with the Current time.</summary>
        public string TimeStamp(bool includetime)
        {
            return ConvertTime(DateTime.Now, includetime);
        }

        /// <summary>Returns a timestamp for the Current time. 
        /// The string is produced by ConvertTime() called with the Current time.</summary>
        public string TimeStamp()
        {
            return ConvertTime(DateTime.Now);
        }


        // SpecimenObservationEvent actions:



        /// <summary>Returns action corresponding to the specified action code for SpecimenObesrvationEvent.</summary>
        /// <param name="actioncode">Action code.</param>
        /// <returns></returns>
        public OeAction ConvertOeAction(string actioncode)
        {
            if (actioncode == OeActionActivate || actioncode == OeCodeActivate)
                return OeAction.Activate;
            else if (actioncode == OeActionComplete || actioncode == OeCodeComplete)
                return OeAction.Complete;
            else if (actioncode == OeActionReviseCompleted || actioncode == OeCodeReviseCompleted)
                return OeAction.ReviseCompleted;
            return OeAction.Unknown;  // default
        }


        /// <summary>Converts OeAction enumerator to string value for Action element.</summary>
        public string ConvertOeAction(OeAction action)
        {
            string ret=null;
            switch (action)
            {
                case OeAction.Activate:
                    ret = OeActionActivate;
                    break;
                case OeAction.Complete:
                    ret = OeActionComplete;
                    break;
                case OeAction.ReviseCompleted:
                    ret = OeActionReviseCompleted;
                    break;
                default:
                    throw new Exception("Does not know how to convert the following action to string: " + action.ToString());
            }
            return ret;
        }


        /// <summary>Converts OeAction enumerator to string value for Action CODE element.</summary>
        public string ConvertOeActionCode(OeAction action)
        {
            string ret = null;
            switch (action)
            {
                case OeAction.Activate:
                    ret = OeCodeActivate;
                    break;
                case OeAction.Complete:
                    ret = OeCodeComplete;
                    break;
                case OeAction.ReviseCompleted:
                    ret = OeCodeReviseCompleted;
                    break;
                default:
                    throw new Exception("Does not know how to convert the following action to string: " + action.ToString());
            }
            return ret;
        }


        // SpecimenObservationOrder actions:


        /// <summary>Returns action corresponding to the specified action code for SpecimenObservationOrder.</summary>
        /// <param name="actioncode">Action code.</param>
        /// <returns></returns>
        public OoAction ConvertOoAction(string actioncode)
        {
            if (actioncode == OoActionCreate || actioncode == OoCodeCreate)
                return OoAction.Create;
            else if (actioncode == OoActionRevise || actioncode == OoCodeRevise)
                return OoAction.Revise;
            else if (actioncode == OoActionNullify || actioncode == OoCodeNullify)
                return OoAction.Nullify;
            return OoAction.Unknown;  // default
        }

        /// <summary>Returns a string corresponding to the specified observation order action code,
        /// as stated in the SpecimenObservationOrder specification.</summary>
        public string ConvertOoAction(OoAction actioncode)
        {
            string ret = null;
            switch (actioncode)
            {
                case OoAction.Create:
                    ret = OoActionCreate;
                    break;
                case OoAction.Revise:
                    ret = OoActionRevise;
                    break;
                case OoAction.Nullify:
                    ret = OoActionNullify;
                    break;
                default:
                    throw new ArgumentException("Unknown (untreated) order action code: " + actioncode.ToString());
            }
            return ret;
        }



        /// <summary>Returns a string corresponding to the specified observation order action code,
        /// as stated in the SpecimenObservationOrder specification for the message code (not action code).
        /// This is used for generation of the appropriate XML message code attribute when this needs to
        /// be modified.</summary>
        public string ConvertOoCode(OoAction actioncode)
        {
            string ret = null;
            switch (actioncode)
            {
                case OoAction.Create:
                    ret = OoCodeCreate;
                    break;
                case OoAction.Revise:
                    ret = OoCodeRevise;
                    break;
                case OoAction.Nullify:
                    ret = OoCodeNullify;
                    break;
                default:
                    throw new ArgumentException("Unknown (untreated) order action code: " + actioncode.ToString());
            }
            return ret;
        }



        /// <summary>Returns observation order status corresponding to the status code.</summary>
        public OoStatus ConvertOoStatus(string statuscode)
        {
            if (statuscode == OoOrderStatusCompleted)
                return OoStatus.Completed;
            else if (statuscode == OoOrderStatusNullified)
                return OoStatus.Nullified;
            else if (statuscode == OoOrderStatusActive)
                return OoStatus.Active;
            return OoStatus.Unknown;
        }

        /// <summary>Returns a string corresponding to the specified observation order status code.</summary>
        public string ConvertOoStatus(OoStatus statuscode)
        {
            string ret = null;
            switch (statuscode)
            {
                case OoStatus.Completed:
                    ret = OoOrderStatusCompleted;
                    break;
                case OoStatus.Nullified:
                    ret = OoOrderStatusNullified;
                    break;
                case OoStatus.Active:
                    ret = OoOrderStatusActive;
                    break;
                default:
                    throw new ArgumentException("Unknown order status code: " + statuscode.ToString());
            }
            return ret;
        }

        /// <summary>Returns action priority corresponding to the string code.</summary>
        public ActionPriority ConvertActionPriority(string code)
        {
            if (string.IsNullOrEmpty(code))
                return ActionPriority.Unknown;
            else
            {
                if (code==ActionPriorityRoutine)
                    return ActionPriority.Routine;
                else if (code == ActionPriorityUrgent)
                    return ActionPriority.Urgent;
                else if (code ==  ActionPriorityCritical)
                    return ActionPriority.Critical;
                return ActionPriority.Unknown;
            }
        }


        // DetailedFinancialTransaction - quantity:

        /// <summary>Converts a quantity to string format that is used in messages (9 numerical digits, 
        /// with eventual leading zeros, last two are after the comma).</summary>
        /// <param name="q">Quantity to be converted.</param>
        /// <returns>String representing the quantity in format suitable for messages.</returns>
        public string ConvertQuantity(decimal q)
        {
            const int reqlength = 9;  // required length
            double qint = Math.Round(100*(double) q);
            string qstr = qint.ToString();
            int numzeros = reqlength - qstr.Length;
            if (numzeros<0)
                throw new ArgumentException("Overflow occurred, string representation of the quantity " 
                    + q.ToString() + "is longer than the requested " + reqlength + " characters." );
            string ret = "";
            for (int i = 0; i < numzeros; ++i)
                ret += "0";
            ret += qstr;
            return ret;

        }

        /// <summary>Converts a string representation of quantity to number type. String form containts 9 numerical digits, 
        /// with eventual leading zeros, last two are after the comma.</summary>
        /// <param name="q">Quantity to be converted.</param>
        /// <returns>String representing the quantity in format suitable for messages.</returns>
        public decimal ConvertQuantity(string qstr)
        {
            decimal ret;
            ret=decimal.Parse(qstr);
            ret=ret/(decimal) 100.0;
            return ret;
        }

        #endregion  // Formatting
       


        #region Global

        private static MsgConst _msgConst = null;

        /// <summary>Returns the process-wide class containing constants used in BIS messages.
        /// Class is initialized with respect to the application variant that is currently running.</summary>
        public static MsgConst Const
        {
            get
            {
                if (_msgConst == null)
                {
                    _msgConst = new MsgConstGolnik();
                }
                return _msgConst;
            }
        }

        #endregion Global


    }



    /// <summary>Here one can handle specifics with respect to different versions of the program
    /// for different customers.
    /// This class should not contain any additional definitions!</summary>
    class MsgConstGolnik : MsgConst
    {
        // REMARK:
        // This class is empty because the base class MsgConst is adjusted for Golnik. 
        // For other classes, differences with respect to specification in MsgConst 
        // (now adjusted for Golnik) will be put into class' constructor.

        public MsgConstGolnik()
        {
            DiagnosisCodeSystem = "Golnik";  // Remark: find out which is the name of the code system used by Golnik!
        }



    }







} // namespace LabexBis