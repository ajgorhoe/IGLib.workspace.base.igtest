
using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
// using System.Configuration;


namespace IG.Lib
{


    /// <summary>Holds sample data.</summary>
    public class Sample
    {
        public string
            BisId = null,
            BisIdOid = null,
            SampleCode = null,
            SampleCodeSystem = null,
            SampleCodeDescription = null,

            SampleBarcode = null,
            SampleBarcodeSystem = null,

            FixativeCode = null,
            FixativeCodeSystem = null,
            FixativeCodeDescription = null,
            SideCode = null,
            SideCodeSystem = null,
            SideCodeDescription = null,
            OrganCode = null,
            OrganCodeSystem = null,
            OrganCodeDescription = null,
            CreationTimeString = null;

        public bool
            RightSide = true;

        public int?
            NumPieces = null;

        // Remark:
        // This is not creation time but sample time! take all necessary measures in order to give it 
        // appropriate meaning! Also, this information can be unspecified (null)!
        public DateTime CreationTime = DateTime.Now;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Sample:");
            sb.AppendLine("  Bis ID: " + BisId + ", OID: " + BisIdOid);
            sb.AppendLine("  SampleCode: " + SampleCode);
            sb.AppendLine("  SampleCodeSystem: " + SampleCodeSystem);
            sb.AppendLine("  SampleCodeDescription: " + SampleCodeDescription);
            sb.AppendLine("  Planned time of taking specimen: " + CreationTime.ToString());
            if (!string.IsNullOrEmpty(SampleBarcode))
                sb.AppendLine("  SampleBarcode: " + SampleBarcode);
            else
                sb.AppendLine("  Bar code is not specified.");
            sb.AppendLine("  FixativeCode: " + FixativeCode);
            sb.AppendLine("  FixativeCodeSystem: " + FixativeCodeSystem);
            sb.AppendLine("  FixativeCodeDescription: " + FixativeCodeDescription);
            sb.AppendLine("  SideCode: " + SideCode);
            sb.AppendLine("  SideCodeSystem: " + SideCodeSystem);
            sb.AppendLine("  SideCodeDescription: " + SideCodeDescription);
            sb.AppendLine("  Flag for Right-hand side: " + RightSide.ToString());
            sb.AppendLine("  OrganCode: " + OrganCode);
            sb.AppendLine("  OrganCodeSystem: " + OrganCodeSystem);
            sb.AppendLine("  OrganCodeDescription: " + OrganCodeDescription);
            if (NumPieces == null)
                sb.AppendLine("Number of pieces is not specified.");
            else
                sb.AppendLine("  Number of pieces: " + NumPieces);
            return sb.ToString();
        }
    } // class Sample

    /// <summary>Holds patient's data.</summary>
    public class Patient
    // $A Igor Apr09;
    {
        public string
            BisId = null,
            BisIdOid = null,
            Name = null,
            Surname = null,
            Phone1 = null,
            Phone2 = null,
            // Gender = null,
            BirthDateStr = null,
            TimeOfDeathStr = null,
            Country = null,
            CountryCode = null,
            CountrySystem = null,
            City = null,
            PostalCode = null,
            StreetAddress = null,
            HealthSecurityId = null,
            Birthplace = null;

        public bool?
            IsMale = null,
            IsAlive = null;

        public DateTime? BirthDate = null;
        public DateTime? TimeOfDeath = null;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Patient:");
            sb.AppendLine("  Bis ID: " + BisId + ", OID: " + BisIdOid);
            sb.AppendLine("  Name: " + Name);
            sb.AppendLine("  Surname: " + Surname);
            sb.AppendLine("  Phone1: " + Phone1);
            sb.AppendLine("  Sex - male: " + IsMale.ToString());
            if (BirthDate == null)
                sb.AppendLine("Date of birth is not specified.");
            else
                sb.AppendLine("  Date of birth: " + BirthDate.ToString());
            bool alive = false;
            if (IsAlive != null)
                alive = IsAlive.Value;
            if (alive)
            {
                if (TimeOfDeath != null)
                    sb.AppendLine("  Warning: Patient is alive but time of death is specified.");
            }
            else
            {
                if (TimeOfDeath == null)
                    sb.AppendLine("  Warning: Patient is dead but time of death is not specified.");
            }
            if (TimeOfDeath!=null)
                sb.AppendLine("  Time of death: " + TimeOfDeath.ToString());
            sb.AppendLine("  Country: " + Country);
            sb.AppendLine("  CountryCode: " + CountryCode);
            sb.AppendLine("  CountrySystem: " + CountrySystem);
            sb.AppendLine("  City: " + City);
            sb.AppendLine("  PostalCode: " + PostalCode);
            sb.AppendLine("  StreetAddress: " + StreetAddress);
            sb.AppendLine("  HealthSecurityId: " + HealthSecurityId);
            sb.AppendLine("  BirthPlace: " + Birthplace);
            sb.AppendLine("  Patient alive: " + IsAlive.ToString());
            return sb.ToString();
        }
    }  // class Patient

    /// <summary>Holds author's data.</summary>
    public class Author
    // $A Igor Apr09;
    {
        public string
            BisId = null,  // = BPI of the author (unique physician name) or internal ID for other personnel
            BisIdOid = null,
            BisIdOrganization = null,
            BisIdOrganizationOid = null,
            OrganizationName = null,
            Country = null,
            CountryCode = null,
            CountrySystem = null,
            City = null,
            PostalCode = null,
            StreetAddress = null,
            Name = null,
            Surname = null,
            Prefix = null,
            Suffix = null;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Person placing order:");
            sb.AppendLine("  Bis ID: " + BisId + ", OID: " + BisIdOid);
            sb.AppendLine("  Bis ID of organization: " + BisIdOrganization + ", OID: " + BisIdOrganizationOid);
            sb.AppendLine("  Organization name: " + OrganizationName);
            sb.AppendLine("  Country: " + Country);
            sb.AppendLine("  CountryCode: " + CountryCode);
            sb.AppendLine("  CountrySystem: " + CountrySystem);
            sb.AppendLine("  City: " + City);
            sb.AppendLine("  PostalCode: " + PostalCode);
            sb.AppendLine("  StreetAddress: " + StreetAddress);
            sb.AppendLine("  Name: " + Name);
            sb.AppendLine("  Surname: " + Surname);
            sb.AppendLine("  Prefix: " + Prefix);
            sb.AppendLine("  Suffix: " + Suffix);
            return sb.ToString();
        }
    }  // class Author

    /// <summary>Holds observation data (goes to rsr... in the main table).</summary>
    public class Observation
    // $A Igor Apr09;
    {
        public string
            BisId = null,
            BisIdOid = null,
            RsrCode = null,
            RsrCodeSystem = null;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Observation data:");
            sb.AppendLine("  Bis ID: " + BisId + ", OID: " + BisIdOid);
            sb.AppendLine("  RsrCode: " + RsrCode);
            sb.AppendLine("  RsrCodeSystem: " + RsrCodeSystem);
            return sb.ToString();
        }
    }  // class Observation


    // Medical data:

    /// <summary>Holds any data that is specified in the section that can contain any medical data,
    /// i.e. within XML element whose name equals MsgConst.OoMedicalDataContainer (="support").
    /// This class contains converters to classes holding specific data.</summary>
    public class MedicalData
    // $A Igor Apr09;
    {
        public string
            DataKind = null,  // value of the introductory attribute that also distinguished different kinds of data
            DataType = null,  // 
            Code = null,
            CodeSystem = null,
            AttributeValue = null,
            ElementValue = null;  // Value specified as node Text rather than with attribute

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  DataKind: " + DataKind);
            sb.AppendLine("  DataType: " + DataType);
            sb.AppendLine("  Code: " + Code);
            sb.AppendLine("  CodeSystem: " + CodeSystem);
            sb.AppendLine("  AttributeValue: \"" + AttributeValue + "\"");
            sb.AppendLine("  ElementValue: \"" + ElementValue + "\"");
            return sb.ToString();
        }

        private MsgConst C { get { return MsgConst.Const; } }

        // Checks whether medical data stored in this object repretents specific datatype of data:
        public bool IsDiagnosisCode { get { return (DataKind == C.OoDiagnosisCodeDataKind && DataType == C.OoDiagnosisCodeDataType); } }
        public bool IsDiagnosisText { get { return (DataKind == C.OoDiagnosisTextDataKind && DataType == C.OoDiagnosisTextDataType); } }
        public bool IsAutopsyDeathReason { get { return (DataKind == C.OoAutopsyDeathReasonDataKind && 
            (DataType == C.OoMedicalDataTypeCode  || DataType == C.OoMedicalDataTypeText)); } }
        public bool IsAutopsyBasicDisease { get { return (DataKind == C.OoAutopsyBasicDiseaseDataKind &&
            (DataType == C.OoMedicalDataTypeCode  || DataType == C.OoMedicalDataTypeText)); } }
        public bool IsAutopsyAssociatedDisease { get { return (DataKind == C.OoAutopsyAssociatedDiseaseDataKind && 
            (DataType == C.OoMedicalDataTypeCode  || DataType == C.OoMedicalDataTypeText)); } }
        public bool IsAutopsyOtherInformation { get { return (DataKind == C.OoAutopsyOtherInformationDataKind && 
            (DataType == C.OoMedicalDataTypeCode  || DataType == C.OoMedicalDataTypeText)); } }


        // conversions to medical data classes containing specific datatype of information:

        /// <summary>Converts medical data contained in this object to diagnosis code.</summary>
        public DiagnosisCodeClass DiagonsisCode
        // $A Igor May09;
        {
            get
            {
                if (!IsDiagnosisCode)
                    throw new Exception("Medical data does not represent a diagnosis code. Data kind: " + DataKind
                        + ", data type: " + DataType);
                DiagnosisCodeClass ret = new DiagnosisCodeClass();
                ret.Code = Code;
                ret.CodeSystem = CodeSystem;
                // ret.Description = ElementValue;
                ret.Description = AttributeValue;
                return ret;
            }
        }

        /// <summary>Converts medical data contained in this object to diagnosis text.</summary>
        public DiagnosisText DiagnosisText
        // $A Igor May09;
        {
            get
            {
                if (!IsDiagnosisText)
                    throw new Exception("Medical data does not represent a diagnosis free text. Data kind: " + DataKind
                        + ", data type: " + DataType);
                DiagnosisText ret = new DiagnosisText();
                // ret.Text = ElementValue;
                ret.Text = AttributeValue;
                return ret;
            }
        }

        /// <summary>Converts medical data contained in this object to AutopsyDeathReason.</summary>
        public AutopsyDeathReason AutopsyDeathReason
        // $A Igor May09;
        {
            get
            {
                if (!IsAutopsyDeathReason)
                    throw new Exception("Medical data does not contain death reason. Data kind: " + DataKind
                        + ", data type: " + DataType);
                AutopsyDeathReason ret = new AutopsyDeathReason();
                ret.Text = AttributeValue;  // text is contained in the value attribute for this type of data.
                return ret;
            }
        }

        /// <summary>Converts medical data contained in this object to AutopsyBasicDisease.</summary>
        public AutopsyBasicDisease AutopsyBasicDisease
        // $A Igor May09;
        {
            get
            {
                if (!IsAutopsyBasicDisease)
                    throw new Exception("Medical data does not contain basic desiese. Data kind: " + DataKind
                        + ", data type: " + DataType);
                AutopsyBasicDisease ret = new AutopsyBasicDisease();
                ret.Text = AttributeValue;  // text is contained in the value attribute for this type of data.
                return ret;
            }
        }

        /// <summary>Converts medical data contained in this object to AutopsyAssociatedDisease.</summary>
        public AutopsyAssociatedDisease AutopsyAssociatedDisease
        // $A Igor May09;
        {
            get
            {
                if (!IsAutopsyAssociatedDisease)
                    throw new Exception("Medical data does not contain associated diseases. Data kind: " + DataKind
                        + ", data type: " + DataType);
                AutopsyAssociatedDisease ret = new AutopsyAssociatedDisease();
                ret.Text = AttributeValue;  // text is contained in the value attribute for this type of data.
                return ret;
            }
        }

        /// <summary>Converts medical data contained in this object to AutopsyOtherInformation.</summary>
        public AutopsyOtherInformation AutopsyOtherInformation
        // $A Igor May09;
        {
            get
            {
                if (!IsAutopsyOtherInformation)
                    throw new Exception("Medical data does not contain other information for autopsy. Data kind: " + DataKind
                        + ", data type: " + DataType);
                AutopsyOtherInformation ret = new AutopsyOtherInformation();
                ret.Text = AttributeValue;  // text is contained in the value attribute for this type of data.
                return ret;
            }
        }



    }  // class MedicalData


    /// <summary>Represents the diagnosis code with description.</summary>
    public class DiagnosisCodeClass 
    // $A Igor Apr09, May09;
    {
        public string
            Code = null,
            CodeSystem = null,
            Description = null;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Diagnosis code:");
            //sb.AppendLine("  Bis ID: " + BisId + ", OID: " +  BisIdOid);
            sb.AppendLine("  Code: " + Code);
            sb.AppendLine("  CodeSystem: " + CodeSystem);
            sb.AppendLine("  Description: \"" + Description + "\"");
            return sb.ToString();
        }
    }  // class DiagnosisCodeClass

    /// <summary>Tepresents the free Text diagnosis.</summary>
    public class DiagnosisText
    // $A IgorApr09,  May09;
    {
        public string
            // Code = null,
            // CodeSystem = null,
            Text = null;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Diagnosis text:");
            //sb.AppendLine("  Bis ID: " + BisId + ", OID: " +  BisIdOid);
            // sb.AppendLine("  Code: " + Code);
            // sb.AppendLine("  CodeSystem: " + CodeSystem);
            sb.AppendLine("  Text: \"" + Text + "\"");
            return sb.ToString();
        }
    } // class DiagnosisText


    // Classes that represent autopsy fields:

    /// <summary>Base class for Text fields of autopsy declaration</summary>
    public abstract class AutopsyBase
    // $A Igor May09;
    {
        protected string _title = null;
        public string Title
        { get { return _title; } }

        /// <summary>Text of the specific field of autopsy declaration.</summary>
        public string Text = null;

        public override string ToString()
        {
            return _title + ": " + Environment.NewLine + Text + Environment.NewLine;
        }
    }

    public class AutopsyDeathReason : AutopsyBase
    {
        public AutopsyDeathReason() { _title = MsgConst.Const.OoAutopsyDeathReasonTitle; }
    }

    public class AutopsyBasicDisease : AutopsyBase
    {
        public AutopsyBasicDisease() { _title = MsgConst.Const.OoAutopsyBasicDiseaseTitle; }
    }

    public class AutopsyAssociatedDisease : AutopsyBase
    {
        public AutopsyAssociatedDisease() { _title = MsgConst.Const.OoAutopsyAssociatedDiseaseTitle; }
    }

    public class AutopsyOtherInformation : AutopsyBase
    {
        public AutopsyOtherInformation() { _title = MsgConst.Const.OoAutopsyOtherInformationTitle; }
    }



    /// <summary>Class for holding and manipulating the data about observation order.
    /// Includes parsing an XML file, storing data internally, and transcription of read data to a PADO object
    /// that enables saving data to a database.</summary>
    public class MsgObervationOrder : MsgBase
    // $A Igor Apr09 May09;
    {

        /// <summary>Default constructor, sets the type information.</summary>
        public MsgObervationOrder()
        {
            Type = MessageType.SpecimenObservationOrder;
        }

        // const string MsgType = "SpecimenObservationOrder";

        //private string FilePath = null;



        /// <summary>Reads msg data from the internal XML document containing the msg.</summary>
        public override void Read()
        {
            Read(Data /* , TableRecord */ );
        }

        /// <summary>Read msg data from an XML document containing the msg.</summary>
        /// <param name="doc">Document containing the msg.</param>
        public override void Read(XmlParser data /* , PAT_bis_classes.clsMsgOrder order */ )
        {
            string location = "Document", aux = null;
            XmlNode auxnode = null;
            try
            {
                ++R.Depth;
                location = "Document";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Started...");
                // Check whether the document is loaded:
                if (data == null)
                    throw new ArgumentException("The XML parser (and builder) with message document is not specified (null reference).");
                if (data.Doc == null)
                    throw new XmlException("The XML message document is not loaded on the XML parser.");
                // Get the Root element and check its name:
                location = "Root";
                data.GoToRoot();
                if (data.Name != Const.OoRootName)
                    throw new XmlException("Wrong name of the root element: " + data.Name + " instead of " + Const.OoRootName);

                // Skip coments and read the msg Id:
                location = "Message Id";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading message ID...");
                if (data.StepIn() == null)
                    throw new XmlException("Root element does not contain any child nodes.");
                data.NextOrCurrentElement();
                if (data.Name != Const.IdElement)
                    throw new XmlException("The first subelement of the root element does not contain message Id. Element name: " + data.Name);
                MessageNumberOid = data.Attribute(Const.IdOidAttribute);
                MessageNumber = data.Attribute(Const.IdIdAttribute);
                // Read creation time:
                location = "Creation time";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading creation time...");
                data.NextElement(Const.CreationTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Message creation time is missing.");
                } else
                {
                    aux = data.Attribute(Const.CreationTimeAttribute);
                    if (string.IsNullOrEmpty(aux))
                        throw new XmlException("Badly formed creation time.");
                    CreationTime = Const.ConvertTime(aux);
                }
                // Read code of the action that should be performed:
                location = "Action code";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading action code...");
                data.NextElement(Const.ActionElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Action specification is missing.");
                } else
                {
                    aux = data.Attribute(Const.ActionAttribute);
                    ActionCode = aux;
                    Action = Const.ConvertOoAction(aux);
                    if (Action == OoAction.Unknown)
                        throw new XmlException("Badly formed action code.");
                    ActionRoot = data.Attribute(Const.ActionOidAttribute);
                }

                data.SetMark("BeforeSender");

                // read sender, receiver and responer of the msg:
                location = "Message sender, receiver and responder";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading data about sender, receiver and responder of the message...");
                while (data.Current != null &&
                    (data.Name != Const.ReceiverContainer && data.Name != Const.ResponderContainer && data.Name != Const.SenderContainer))
                {
                    data.NextElement();
                }
                if (data.Current == null)
                {
                    // There is no sender, receiver or responder:
                    data.BackToMark("BeforeSender");
                    throw new XmlException("Data about receiver, sender and responder of the message is missing.");
                } else
                {
                    data.RemoveMarks("BeforeSender");
                    while (data.Name == Const.ReceiverContainer || data.Name == Const.ResponderContainer || data.Name == Const.SenderContainer)
                    {
                        auxnode = data.Current;
                        if (data.Name == Const.ReceiverContainer)
                        {
                            // read information on receiver of the msg:
                            location = "Receiver";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading receiver data...");
                            data.StepIn();
                            data.NextOrCurrentElement(Const.ReceiverSubContainer);
                            data.StepIn();
                            data.NextOrCurrentElement(Const.IdElement);
                            if (data.Current == null)
                                throw new XmlException("Data about message receiver is badly formed. Container element: "
                                    + Const.ReceiverContainer + ".");
                            MessageReceiver = data.Attribute(Const.IdIdAttribute);
                            MessageReceiverOid = data.Attribute(Const.IdOidAttribute);
                        }
                        else if (data.Name == Const.ResponderContainer)
                        {
                            // read information on responder of the msg:
                            location = "Responder";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading responder data...");
                            data.StepIn();
                            data.NextOrCurrentElement(Const.ResponderSubContainer);
                            data.StepIn();
                            data.NextOrCurrentElement(Const.IdElement);
                            if (data.Current == null)
                                throw new XmlException("Data about message responder is badly formed. Container element: "
                                    + Const.ResponderContainer + ".");
                            MessageResponder = data.Attribute(Const.IdIdAttribute);
                            MessageResponderOid = data.Attribute(Const.IdOidAttribute);
                        }
                        else if (data.Name == Const.SenderContainer)
                        {
                            // read information on sender of the msg:
                            location = "Sender";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading sender data...");
                            data.StepIn();
                            data.NextOrCurrentElement(Const.SenderSubContainer);
                            data.StepIn();
                            data.NextOrCurrentElement(Const.IdElement);
                            if (data.Current == null)
                                throw new XmlException("Data about message sender is badly formed. Container element: "
                                    + Const.SenderContainer + ".");
                            MessageSender = data.Attribute(Const.IdIdAttribute);
                            MessageSenderOid = data.Attribute(Const.IdOidAttribute);
                        }
                        data.Current = auxnode;
                        data.NextElement();
                    }
                }
                // Read the msg:
                location = "Message outer container";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading message contents...");
                data.NextOrCurrentElement(Const.OoMessageContainer);
                if (data.Current == null)
                    throw new XmlException("There is no message outer container - element " + Const.OoMessageContainer);
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("Message outer container does not contain any child nodes - element " + Const.OoMessageContainer);
                // Read the msg code:
                location = "Messge code";
                data.NextOrCurrentElement(Const.CodeElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Message code is not specified - element " + Const.CodeElement);
                } else
                {
                    MessageCode = data.Attribute(Const.CodeAttribute);
                    MessageCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                    // This code must correspond to the action:
                    OoAction control = Const.ConvertOoAction(MessageCode);
                    if (control != Action)
                        throw new XmlException("Message code (\"" + MessageCode + "\") does not agree with the required action ("
                            + Action.ToString() + ").");
                }
                // Go deeper into subcontainers where significan contents is digged:
                location = "Message subcontainer";
                data.NextElement(Const.MessageSubContainer);
                if (data.Current == null)
                    throw new XmlException("There is no message subcontainer - element " + Const.MessageSubContainer);
                aux = data.Attribute(Const.MessageSubContainerTypeAttribute);
                if (aux != Const.MessageSubContainerTypeAttributeValue)
                    throw new XmlException("Wrong " + Const.MessageSubContainerTypeAttribute + " attribute, element " + Const.MessageSubContainer
                        + " (" + aux + " instead of " + Const.MessageSubContainerTypeAttributeValue + ").");
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("There is no message contents - inside element " + Const.MessageSubContainer);
                // Step into the mesage sub-subcontainer:
                location = "Message sub-subcontainer";
                data.NextOrCurrentElement(Const.OoMessageSubContainer2);
                if ((aux = data.Attribute(Const.MessageTypeAttribute)) != Const.OoMessageType)
                    throw new XmlException("Wrong message type: " + aux + ", shoud be: " + Const.OoMessageType + ".");
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("There is no message contents - inside element " + Const.OoMessageSubContainer2);
                // Read sender's order Id
                location = "Order ID";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading order ID...");
                data.NextOrCurrentElement(Const.IdElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Order ID is not specified.");
                } else
                {
                    BisOrderId = data.Attribute(Const.IdIdAttribute);
                    BisOrderIdOid = data.Attribute(Const.IdOidAttribute);
                }
                // Read observatiion code:
                location = "Observation code";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading observation code...");
                data.NextElement(Const.CodeElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("There is no observation code - element " + Const.CodeElement);
                } else
                {
                    ObservationType = data.Attribute(Const.CodeAttribute);
                    RsrTypeCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                }
                // Read Order status:
                location = "Order status";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading order status code...");
                data.NextElement(Const.OoOrderStatusElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Order status is not specified - element " + Const.OoOrderStatusElement);
                } else
                {
                    OrderStatus = data.Attribute(Const.OoOrderStatusAttribute);
                    OoStatus status = Const.ConvertOoStatus(aux = OrderStatus);
                    if (status != OoStatus.Completed && status != OoStatus.Nullified)
                    {
                        OrderStatus = null;
                        throw new XmlException("Invalid order status or status not allowed in this context: " + aux);
                    }
                }
                // Read Order comment:
                location = "Order comment";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading order comment (descriptive text)...");
                data.SetMark();
                data.NextElement(Const.OoCommentElement);
                if (data.Current == null)
                {
                    OrderComment = null;
                    data.BackToMark();  // go back to previous node
                }
                else
                {
                    OrderComment = data.Value;
                    data.RemoveMark();
                }
                // Read order time:
                location = "Order time";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading order time...");
                data.NextElement(Const.OoOrderTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Order time is not specified - element " + Const.OoOrderTimeElement);
                } else
                {
                    aux = data.Attribute(Const.OoOrderTimeAttribute);
                    if (string.IsNullOrEmpty(aux))
                        throw new XmlException("Order time is not specified properly - element " + Const.OoOrderTimeElement);
                    Ordertime = Const.ConvertTime(aux);
                }
                // Read order priority:
                location = "Order priority";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading order priority...");
                data.NextElement(Const.OoActionPriorityElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Order priority is not specified - element " + Const.OoActionPriorityElement);
                } else
                {
                    Priority = data.Attribute(Const.OoActionPriorityCodeAttribute);
                    if (Const.ConvertActionPriority(Priority) == ActionPriority.Unknown)
                        throw new XmlException("Invalid priority code: " + Priority);
                    PriorityCodeSystem = data.Attribute(Const.OoActionPriorityCodeSystemAttribute);
                }
                // Read Samples data:   
                location = "Samples data";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading samples data...");
                bool SamplesRead = false;
                int NumSamplesRead = 0;
                data.SetMark("BeforeSamples");
                while (data.Current != null)
                {
                    data.NextElement(Const.OoSampleContainer);
                    if (data.Current != null)
                    {
                        // Mark sample container; The last of these marks (if any exists) will contain the last sample containter.
                        Data.SetMark("LastSampleContainer");
                        data.StepIn();
                        data.NextOrCurrentElement(Const.OoSampleSubContainer);
                        data.StepIn();
                        ++NumSamplesRead;  // Increment the number of samples that are read
                        if (data.Current == null)
                        {
                            throw new XmlException("Invalid definition of sample No. " + NumSamplesRead.ToString()
                                + ": container element does not include further data.");
                        } else
                        {
                            SamplesRead = true;  // yes, we have read at least one sample
                            // Read data for the next sample, allocate space first and read sample Bis ID:
                            location = "Sample " + NumSamplesRead.ToString();
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading sample No. "
                                + NumSamplesRead.ToString() + "...");
                            Sample sample = new Sample();
                            Samples.Add(sample);
                            data.NextOrCurrentElement(Const.IdElement);
                            // Remark: should exception actually be thrown in this situation?
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("There is no sample ID.");
                            } else
                            {
                                sample.BisId = data.Attribute(Const.IdIdAttribute);
                                sample.BisIdOid = data.Attribute(Const.IdOidAttribute);
                            }
                            // Read sample code and description:
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading sample data...");
                            //// $$$$ Za spodnji 2 vrstici ni čisto ziher, da je OK, če sta zakomentirani (verjetno pa tudi ni 
                            //// čisto prav, da sta odkomentirani, to lahko privede do dvojnega zaporednega klica data.Back(), kar je napaka!)
                            //if (data.Current == null)
                            //    data.Back();
                            data.NextElement(Const.CodeElement);
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("Sample code is not specified.");
                            } else
                            {
                                sample.SampleCode = data.Attribute(Const.CodeAttribute);
                                sample.SampleCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                                sample.SampleCodeDescription = data.Attribute(Const.CodeDescriptionAttribute);
                            }


                            /* Read planned time when sample should be taken (if defined): */
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading planned time time of the sample...");
                            data.SetMark("pos");
                            data.NextElement(Const.OoSampleTimeElement);
                            if (data.Current == null)
                                data.BackToMark("pos");
                            else
                            {
                                data.RemoveMarks("pos");
                                sample.CreationTimeString = data.Attribute(Const.ValueAttribute);
                                if (sample.CreationTimeString != null)
                                    sample.CreationTime = Const.ConvertTime(sample.CreationTimeString);
                            }


                            // Read sample data:
                            location = "Sample data for sample " + NumSamplesRead.ToString();
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading sample data...");
                            location = "Sample data";
                            //if (data.Current==null)
                            //    data.Back();

                            //$$$ 

                            data.NextElement(Const.OoSampleDataContainer);
                            if (data.Current == null)
                                data.Back();
                            else
                            {
                                data.SetMark("SampleBasicData");
                                data.StepIn();
                                data.NextOrCurrentElement(Const.OoSampleDataSubContainer);
                                if (data.Current == null)
                                    throw new XmlException("There is no data subcontainer.");
                                data.StepIn();
                                if (data.Current != null)
                                {
                                    // Read sample bar code:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading sample barcode...");
                                    data.SetMark();
                                    data.NextOrCurrentElement(Const.CodeElement);
                                    if (data.Current == null)
                                    {
                                        data.BackToMark();
                                    } else
                                    {
                                        data.RemoveMark();
                                        sample.SampleBarcode = data.Attribute(Const.CodeAttribute);
                                        sample.SampleBarcodeSystem = data.Attribute(Const.CodeSystemAttribute);
                                    }
                                    /* Jump to status code:  */
                                    data.NextOrCurrentElement(Const.OoSampleStatuscodeElement);
                                    if (data.Current == null)
                                    {
                                        // Status code in our case is constant ("completed"), therefore we only check that it
                                        // exists. Throwind exception could eventually be removed (?), but then we should
                                        // note that the element is missing ans perhaps report a warning in consistancy
                                        // check.
                                        data.Back();
                                        throw new XmlException("Status code is not defined.");
                                    }
                                    /* Read fixative if defined: */
                                    // Remark: currently, only one fixative can be defined per sample:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading fixative");
                                    data.SetMark("pos");
                                    data.NextOrCurrentElement(Const.OoSampleFixativeElement);
                                    if (data.Current == null)
                                        data.BackToMark("pos");
                                    else
                                    {
                                        data.RemoveMarks("pos");
                                        sample.FixativeCode = data.Attribute(Const.CodeAttribute);
                                        sample.FixativeCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                                        sample.FixativeCodeDescription = data.Attribute(Const.CodeDescriptionAttribute);
                                        // Verify that there are not more than one:
                                        data.SetMark("cp");
                                        data.NextElement(Const.OoSampleFixativeElement);
                                        if (data.Current != null)
                                            throw new XmlException("There are more than one fixatives on this sample.");
                                        data.BackToMark("cp");
                                    }
                                    /* Read the side of the organ on which the specimen must be taken: */
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading sampling side");
                                    data.SetMark("pos");
                                    data.NextOrCurrentElement(Const.OoSampleSideElement);
                                    if (data.Current == null)
                                        data.BackToMark("pos");
                                    else
                                    {
                                        data.RemoveMarks("pos");
                                        sample.SideCode = data.Attribute(Const.CodeAttribute);
                                        sample.SideCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                                        sample.SideCodeDescription = data.Attribute(Const.CodeDescriptionAttribute);
                                        // The code has been read, now convert the code immediately:
                                        if (sample.SideCode == Const.LeftSideCode)
                                            sample.RightSide = false;
                                        else if (sample.SideCode == Const.RightsideCode)
                                            sample.RightSide = true;
                                        else
                                            throw new XmlException("Unknown organ side code of the sample: \""
                                                + sample.SideCode + "\".");
                                        // Verify that there are not more than one:
                                        data.SetMark("cp");
                                        data.NextElement(Const.OoSampleSideElement);
                                        if (data.Current != null)
                                            throw new XmlException("There are more than one sampling sides on this sample.");
                                        data.BackToMark("cp");
                                    }
                                    /* Read organ if defined: */
                                    // Remark: only one organ (and side also) can currentlty be defined per sample.
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading organ");
                                    data.SetMark("pos");
                                    data.NextOrCurrentElement(Const.OoSampleOrganElement);
                                    if (data.Current == null)
                                        data.BackToMark("pos");
                                    else
                                    {
                                        data.RemoveMarks("pos");
                                        sample.OrganCode = data.Attribute(Const.CodeAttribute);
                                        sample.OrganCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                                        sample.OrganCodeDescription = data.Attribute(Const.CodeDescriptionAttribute);
                                        // Verify that there are not more than one:
                                        data.SetMark("cp");
                                        data.NextElement(Const.OoSampleOrganElement);
                                        if (data.Current != null)
                                            throw new XmlException("There are more than one organs on this sample.");
                                        data.BackToMark("cp");
                                    }
                                }

                                data.GoToMark("SampleBasicData");

                            }
                            // Read number of sample pieces:
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading number of pieces in a sample...");
                            location = "Sample - number of pieces";
                            data.NextOrCurrentElement(Const.OoSampleNumPiecesContainer);
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("Data about sample's number of pieces is missing. Element: "
                                    + Const.OoSampleNumPiecesSubContainer);
                            } else
                            {
                                data.SetMark("NumPieces");
                                data.StepIn();
                                data.NextOrCurrentElement(Const.OoSampleNumPiecesSubContainer);
                                if (data.Current == null)
                                    throw new XmlException("Badly formed sample's number of pieces: subcontainer is missing. Element: "
                                        + Const.OoSampleNumPiecesSubContainer);
                                data.StepIn();
                                data.NextOrCurrentElement(Const.OoSampleNumPiecesElement);
                                if (data.Current == null)
                                    throw new Exception("Badly formed sample's number of pieces: inner-most element is missing. Element: "
                                        + Const.OoSampleNumPiecesElement);
                                string numpieces = data.Attribute(Const.OoSampleNumPiecesAttribute);
                                try
                                {
                                    sample.NumPieces = int.Parse(numpieces);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Error in parsing number of pieces, could not convert value to integer."
                                        + Environment.NewLine + "Details: " + ex.Message);
                                }
                                data.BackToMark("NumPieces");
                            }
                        }
                        data.GoToMark("LastSampleContainer");
                    }  // if container was found
                }  // while (data.Current != null)  - reading individual samples
                // Prepare position for next elements:
                if (data.GoToMark("LastSampleContainer") == null)
                    data.GoToMark("BeforeSamples");
                data.RemoveMarks("BeforeSamples"); // remove all marks used when reading the patient's data
                if (!SamplesRead) { }

                // Read Patients data:   
                location = "Patients data";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patients data...");
                // bool PatientsRead = false;
                int NumPatientsRead = 0;
                data.SetMark("BeforePatients");
                while (data.Current != null)
                {
                    data.NextElement(Const.OoPatientContainer);
                    if (data.Current != null)
                    {
                        // Mark patient container; The last of these marks (if any exists) will contain the last patient containter.
                        Data.SetMark("LastPatientContainer");
                        data.StepIn();
                        data.NextOrCurrentElement(Const.OoPatientSubContainer);
                        data.StepIn();
                        ++NumPatientsRead;  // Increment the number of patients that are read
                        if (data.Current == null)
                        {
                            throw new XmlException("Invalid definition of patient No. " + NumPatientsRead.ToString()
                                + ": container element does not include further data.");
                        } else
                        {
                            // PatientsRead = true;  // yes, we have read at least one patient
                            // Read data for the next patient, allocate space first and read patient Bis ID:
                            location = "Patient " + NumPatientsRead.ToString();
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient No. "
                                + NumPatientsRead.ToString() + "...");
                            Patient patient = new Patient();
                            Patients.Add(patient);
                            data.NextOrCurrentElement(Const.IdElement);
                            if (data.Current == null)
                            {
                                data.Back();
                                // Remark: consider whether exception should actually be thrown in this situation (i.e. no BisId)!
                                // - probably YES, but could this be moved to data consistency check performed after reading
                                throw new XmlException("There is no patient ID.");
                            } else
                            {
                                patient.BisId = data.Attribute(Const.IdIdAttribute);
                                patient.BisIdOid = data.Attribute(Const.IdOidAttribute);
                            }
                            // Read patient data:
                            location = "Patient " + NumPatientsRead.ToString() + " - personal data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient personal data...");
                            data.NextElement(Const.OoPatientDataContainer);
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("Patient personal data is not defined.");
                            } else
                            {
                                data.StepIn();
                                // Read patient's name and surname:
                                location = "Patient " + NumPatientsRead.ToString() + " - name and surname";
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient name...");
                                data.NextOrCurrentElement(Const.OoPatientNameContainer);
                                if (data.Current == null)
                                    data.Back();
                                else
                                {
                                    data.StepIn();
                                    // Read patient's name:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's surname...");
                                    data.NextOrCurrentElement(Const.OoPatientSurnameElement);
                                    if (data.Current == null)
                                        data.Back();
                                    else
                                        patient.Surname = data.Value;
                                    // Read patient's name:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's given name...");
                                    data.NextOrCurrentElement(Const.OoPatientNameElement);
                                    if (data.Current == null)
                                        data.Back();
                                    else
                                        patient.Name = data.Value;

                                    data.StepOut();  // step out of the container containing names
                                }
                            }
                            // Read patient's gender:
                            // Remark: consider later whether the code system should also be read here; currently, gender
                            // coding is hard-coded, and works fine:
                            location = "Patient " + NumPatientsRead.ToString() + " - personal data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's gender...");
                            data.NextOrCurrentElement(Const.OoPatientGenderElement);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                string str = data.Attribute(Const.OoPatientGenderAttribute);
                                int which = -1;
                                try
                                {
                                    which = int.Parse(str);
                                }
                                catch { }
                                if (which == Const.OoGenderMale)
                                    patient.IsMale = true;
                                else if (which == Const.OoGenderFemale)
                                    patient.IsMale = false;
                                else
                                    throw new XmlException("Unrecognized patient's gender coode (should be 1 for male or 2 for female): "
                                        + str);
                            }
                            // Read patient's date of birth:
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's date of birth...");
                            data.NextOrCurrentElement(Const.OoPatientBirthDateElement);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                patient.BirthDateStr = data.Attribute(Const.ValueAttribute);
                                patient.BirthDate = Const.ConvertTime(patient.BirthDateStr);
                            }
                            // Read patient's mortal status:
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's mortal status...");
                            data.NextOrCurrentElement(Const.OoPatientDeceasedElement);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                string str = null;
                                str = data.Attribute(Const.ValueAttribute);
                                if (str == null)
                                    str = "";
                                str = str.ToLower();
                                if (str == "true")
                                    patient.IsAlive = false;
                                else if (str == "false")
                                    patient.IsAlive = true;
                                else throw new XmlException("Invalid format of patient's mortal status (should be 'true' for deceased and false for alive patients): "
                                    + str);
                            }

                            // Read Patient's death time:
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's time of death...");
                            data.NextOrCurrentElement(Const.OoPatientTimeOfDeathElement);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                patient.TimeOfDeathStr = data.Attribute(Const.ValueAttribute);
                                if (patient.TimeOfDeathStr != null)
                                {
                                    patient.TimeOfDeath = Const.ConvertTime(patient.TimeOfDeathStr);
                                    // REMARK: check below was moved to data consistency check!
                                    //if (Const.OoCheckAliveConsistency)
                                    //{
                                    //    if (patient.IsAlive != null)
                                    //        if (patient.IsAlive.Value)
                                    //            throw new XmlException("Patient is alive but time of death is specified. Time: "
                                    //                + patient.TimeOfDeathStr);
                                    //}
                                }
                            }

                            /* Read patient's address: */
                            location = "Patient " + NumPatientsRead.ToString() + " - address data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's address data...");
                            data.NextOrCurrentElement(Const.OoAddressContainter);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                data.SetMark("Address");
                                data.StepIn();
                                // Read patient's country:
                                // Remark: this is implemented according to current agreement.
                                // In the (probably far) future, country full name may be read as well as country code!
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's country...");
                                data.NextOrCurrentElement(Const.OoCountryCodeElement);
                                if (data.Current == null)
                                    data.Back();
                                else
                                    patient.CountryCode = data.Value;
                                // Read patient's city:
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's city...");
                                data.NextOrCurrentElement(Const.OoCityElement);
                                if (data.Current == null)
                                    data.Back();
                                else
                                    patient.City = data.Value;
                                // Read patient's postal code:
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's postal code...");
                                data.NextOrCurrentElement(Const.OoPostalCodeElement);
                                if (data.Current == null)
                                    data.Back();
                                else
                                    patient.PostalCode = data.Value;
                                // Read patient's street address:
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's postal street address...");
                                data.NextOrCurrentElement(Const.OoStreetAddressElement);
                                if (data.Current == null)
                                    data.Back();
                                else
                                    patient.StreetAddress = data.Value;
                                // data.StepOut(); // step out of the subcontainer
                                data.BackToMark("Address");
                            }


                            // Read patient's place of birth:
                            location = "Patient " + NumPatientsRead.ToString() + " - address place of birth data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's place of birth data...");
                            data.NextOrCurrentElement(Const.OoPatientBirthPlaceContainer);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                data.SetMark("BirthPlace");
                                data.StepIn();
                                data.NextOrCurrentElement(Const.OoPatientBirthPlaceSubcontainer);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    throw new XmlException("Invalid format of patient's place of birth data.");
                                } else
                                {
                                    data.StepIn();
                                    // Read patient's place of birth:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's place of birth...");
                                    data.NextOrCurrentElement(Const.OoPatientBirthPlaceElement);
                                    if (data.Current == null)
                                        throw new XmlException("Invalid format of patient's place of birth.");
                                    patient.Birthplace = data.Value;
                                }
                                data.BackToMark("BirthPlace");
                            }
                            // Read patient's health insurance number:
                            location = "Patient " + NumPatientsRead.ToString() + " - health insurance data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading patient's health insurance data...");
                            data.NextOrCurrentElement(Const.OoPatientHealthInsuranceContainer);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                data.SetMark("HealthInsurance");
                                data.StepIn();
                                if (data.Current == null)
                                    throw new XmlException("Invalid format of health insurance data.");
                                data.NextOrCurrentElement(Const.IdElement);
                                patient.HealthSecurityId = data.Attribute(Const.IdIdAttribute);
                                data.BackToMark("HealthInsurance");
                            }
                        }
                        data.GoToMark("LastPatientContainer");
                    }  // if container was found
                }  // while (data.Current != null)  - reading individual patients
                // Prepare position for next elements:
                if (data.GoToMark("LastPatientContainer") == null)
                    data.GoToMark("BeforePatients");
                data.RemoveMarks("BeforePatients"); // remove all marks used when reading the patient's data
                // Checks below are moved to the data consistency check procedure:
                //if (!PatientsRead)
                //    throw new XmlException("Patients data is not defined.");
                //if (NumPatientsRead < 1)
                //    throw new XmlException("Patients data is not defined correctly, no patients were read.");
                //else if (NumPatientsRead > 1)
                //    throw new XmlException("Data for more than one patient is contained in the message, this is not allowed.");

                // Read Author's data:   
                location = "Authors data";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading authors data...");
                // bool AuthorsRead = false;
                int NumAuthorsRead = 0;
                data.SetMark("BeforeAuthors");
                while (data.Current != null)
                {
                    data.NextElement(Const.OoAuthorContainer);
                    if (data.Current != null)
                    {
                        // Mark author container; The last of these marks (if any exists) will contain the last author containter.
                        Data.SetMark("LastAuthorContainer");
                        data.StepIn();
                        data.NextOrCurrentElement(Const.OoAuthorSubcontainer);
                        data.StepIn();
                        ++NumAuthorsRead;  // Increment the number of authors that are read
                        if (data.Current == null)
                        {
                            throw new XmlException("Invalid definition of author No. " + NumAuthorsRead.ToString()
                                + ": container element does not include further data.");
                        } else
                        {
                            // AuthorsRead = true;  // yes, we have read at least one author
                            // Read data for the next author, allocate space first and read author Bis ID:
                            location = "Author " + NumAuthorsRead.ToString();
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author No. "
                                + NumAuthorsRead.ToString() + "...");
                            Author author = new Author();
                            Authors.Add(author);
                            data.NextOrCurrentElement(Const.IdElement);
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("There is no author ID.");
                            } else
                            {
                                author.BisId = data.Attribute(Const.IdIdAttribute);
                                author.BisIdOid = data.Attribute(Const.IdOidAttribute);
                            }
                            // Read author's organization data:
                            location = "Author " + NumAuthorsRead.ToString() + " - organization data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author organization data...");
                            data.NextOrCurrentElement(Const.OoAuthorOrgContainer);
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("Author's organization data is missing.");
                            } else
                            {
                                data.SetMark("AuthorOrg");
                                data.StepIn();
                                // Read author's organization ID:
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author organization ID...");
                                data.NextOrCurrentElement(Const.IdElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    // throw new XmlException("Author organization's ID is not specified.");  // now verified in data consistency check!
                                } else
                                {
                                    author.BisIdOrganization = data.Attribute(Const.IdIdAttribute);
                                    author.BisIdOrganizationOid = data.Attribute(Const.IdOidAttribute);
                                }
                                // Check below is moved to the data consistency check:
                                //if (author.BisIdOrganization == null)
                                //{
                                //    throw new XmlException("Author's organization ID is not specified.");
                                //}
                                // Read author's organization name:
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author organization name...");
                                data.NextOrCurrentElement(Const.OoAuthorOrgNameElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                } else
                                {
                                    author.OrganizationName = data.Value;
                                }

                                /* Read author's address: */
                                location = "Author " + NumAuthorsRead.ToString() + " - address data";
                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author's address data...");
                                data.NextOrCurrentElement(Const.OoAddressContainter);
                                if (data.Current == null)
                                {
                                    data.Back();
                                } else
                                {
                                    data.SetMark("Address");
                                    data.StepIn();
                                    // Read author's country:
                                    // Remark: Modify if agreement changes, country full name might be read as well as country code in the (probably far) future!
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author's country...");
                                    data.NextOrCurrentElement(Const.OoCountryCodeElement);
                                    if (data.Current == null)
                                    {
                                        data.Back();
                                    } else
                                    {
                                        author.CountryCode = data.Value;
                                    }
                                    // Read author's city:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author's city...");
                                    data.NextOrCurrentElement(Const.OoCityElement);
                                    if (data.Current == null)
                                    {
                                        data.Back();
                                    } else
                                    {
                                        author.City = data.Value;
                                    }
                                    // Read author's postal code:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author's postal code...");
                                    data.NextOrCurrentElement(Const.OoPostalCodeElement);
                                    if (data.Current == null)
                                    {
                                        data.Back();
                                    } else
                                    {
                                        author.PostalCode = data.Value;
                                    }
                                    // Read author's street address:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author's postal street address...");
                                    data.NextOrCurrentElement(Const.OoStreetAddressElement);
                                    if (data.Current == null)
                                    {
                                        data.Back();
                                    } else
                                    {
                                        author.StreetAddress = data.Value;
                                    }
                                    // data.StepOut(); // step out of the subcontainer
                                    data.BackToMark("Address");
                                }

                                data.BackToMark("AuthorOrg");
                            }


                            // Read author's personal data:
                            location = "Author " + NumAuthorsRead.ToString() + " - personal data";
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author person data...");
                            data.NextOrCurrentElement(Const.OoAuthorDataContainer);
                            if (data.Current == null)
                            {
                                data.Back();
                            } else
                            {
                                data.SetMark("AuthorPersonal");
                                data.StepIn();
                                data.NextOrCurrentElement(Const.OoAuthorDataSubcontainer);
                                data.StepIn();
                                if (data.Current == null)
                                {
                                    // Check below has moved to data consistency check!
                                    //throw new XmlException("Invalid format of author's personal data, there is no element named"
                                    //    + Const.OoAuthorDataSubcontainer);
                                } else
                                {
                                    // Read author's name and surname:
                                    if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author name data...");
                                    data.NextOrCurrentElement(Const.OoAuthorNameContainer);
                                    if (data.Current == null)
                                    {
                                        throw new XmlException("Author name is not defined.");
                                    } else
                                    {
                                        data.StepIn();
                                        if (data.Current == null)
                                            throw new XmlException("Author's name data is invalid.");

                                        // Read author's surname:
                                        if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author surname...");
                                        data.NextOrCurrentElement(Const.OoAuthorSurnameElement);
                                        if (data.Current == null)
                                            data.Back();
                                        else
                                            author.Surname = data.Value;
                                        // Read author's name:
                                        if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author name...");
                                        data.NextOrCurrentElement(Const.OoAuthorNameElement);
                                        if (data.Current == null)
                                            data.Back();
                                        else
                                            author.Name = data.Value;
                                        // Read author's prefix:
                                        if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author prefix...");
                                        data.NextOrCurrentElement(Const.OoAuthorPrefixElement);
                                        if (data.Current == null)
                                            data.Back();
                                        else
                                            author.Prefix = data.Value;
                                        // Read author's suffix:
                                        if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading author suffix...");
                                        data.NextOrCurrentElement(Const.OoAuthorSuffixElement);
                                        if (data.Current == null)
                                            data.Back();
                                        else
                                            author.Suffix = data.Value;
                                        data.StepOut();
                                    }
                                }
                                data.BackToMark("AuthorPersonal");
                            }
                        }
                        data.GoToMark("LastAuthorContainer");
                    }  // if container was found
                }  // while (data.Current != null)  - reading individual authors
                // Prepare position for next elements:
                if (data.GoToMark("LastAuthorContainer") == null)
                    data.GoToMark("BeforeAuthors");
                data.RemoveMarks("BeforeAuthors"); // remove all marks used when reading the author's data
                // Checks below are moved to data consistency checks:
                //if (!AuthorsRead)
                //{
                //    throw new XmlException("Authors data is not defined.");
                //}
                //if (NumAuthorsRead < 1)
                //    throw new XmlException("Authors data is not defined correctly, no authors were read.");
                //else if (NumAuthorsRead > 1)
                //    throw new XmlException("Data for more than one author is contained in the message, this is not allowed.");

                // Read various medical data:   
                location = "Medical data";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading medical data...");
                bool DiagnosisCodesRead = false,
                     DiagnosisTextRead = false,
                     MedicalDataRead = false;
                int NumMedicalDataRead = 0,
                    NumDiagnosisCodesRead = 0,
                    NumDiagnosisTextRead = 0,
                    NumAutopsyDeathReasonRead = 0,
                    NumAutopsyBasicDiseaseRead = 0,
                    NumAutopsyAssociatedDiseaseRead = 0,
                    NumAutopsyOtherInformationRead = 0;
                data.SetMark("BeforeMedicalDataElements");
                while (data.Current != null)
                {
                    data.NextElement(Const.OoMedicalDataContainer);
                    if (data.Current != null)
                    {
                        // Mark medical data container container; The last of these marks (if any exists) will contain the last containter.
                        Data.SetMark("LastMedicalDataContainer");
                        data.StepIn();
                        data.NextOrCurrentElement(Const.OoMedicalDataSubContainer);
                        data.StepIn();
                        if (data.Current == null)
                        {
                            throw new XmlException("Invalid medical data No. " + (1 + NumMedicalDataRead).ToString()
                                + ": container element does not include further data.");
                        } else
                        {
                            // Read next medical data element:
                            location = "Medical data No. " + (1 + NumMedicalDataRead).ToString();
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading medical data No. "
                                + (1 + NumDiagnosisCodesRead).ToString());
                            // Read medical data:
                            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading medical data...");
                            data.NextOrCurrentElement(Const.OoMedicalDataKindElement);
                            if (data.Current == null)
                            {
                                throw new XmlException("There is no medical data introduction element named "
                                    + Const.OoMedicalDataKindElement + ".");
                            }
                            else
                            {

                                MedicalData medicaldata = new MedicalData();

                                //string datakind = data.Attribute(Const.OoMedicalDataKindAttribute);
                                //if (datakind == null)
                                //    datakind = "";

                                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Reading medical data...");
                                // Read data contained in the medical data element:
                                medicaldata.DataKind = data.Attribute(Const.OoMedicalDataKindAttribute);

                                data.NextOrCurrentElement(Const.OoMedicalDataElement);
                                if (data.Current == null)
                                    throw new XmlException("There is no medical data value element " + Const.OoMedicalDataElement + ".");
                                medicaldata.DataType = data.Attribute(Const.OoMedicalDataTypeAttribute);
                                medicaldata.Code = data.Attribute(Const.OoMedicalDataCodeAttribute);
                                medicaldata.CodeSystem = data.Attribute((Const.OoMedicalDataCodeSystemAttribute));
                                medicaldata.AttributeValue = data.Attribute(Const.OoMedicalDataValueAttribute);
                                medicaldata.ElementValue = data.Value;
                                MedicalDataRead = true;  // yes, we have read at least one set of medical data.
                                ++NumMedicalDataRead;

                                if (medicaldata.IsDiagnosisText)
                                {
                                    DiagnosisTextRead = true;  // yes, we have read at least one diagnosis Text
                                    ++NumDiagnosisTextRead;
                                    DiagnosisText diagnosistext = medicaldata.DiagnosisText;
                                    DiagnosisTexts.Add(diagnosistext);
                                } else if (medicaldata.IsDiagnosisCode)
                                {
                                    DiagnosisCodesRead = true;  // yes, we have read at least one diagnosis code
                                    ++NumDiagnosisCodesRead;
                                    DiagnosisCodeClass diagnosiscode = medicaldata.DiagonsisCode;
                                    DiagnosisCodes.Add(diagnosiscode);
                                } else if (medicaldata.IsAutopsyDeathReason)
                                {
                                    ++NumAutopsyDeathReasonRead;
                                    if (NumAutopsyDeathReasonRead > 1)
                                        throw new XmlException("Number of 'AutopsyDeathReason' records is greater than one, only one is allowed.");
                                    else
                                        AutopsyDeathReason = medicaldata.AutopsyDeathReason;
                                } else if (medicaldata.IsAutopsyBasicDisease)
                                {
                                    ++NumAutopsyBasicDiseaseRead;
                                    if (NumAutopsyBasicDiseaseRead > 1)
                                        throw new XmlException("Number of 'AutopsyBasicDisease' records is greater than one, only one is allowed.");
                                    else
                                        AutopsyBasicDisease = medicaldata.AutopsyBasicDisease;
                                } else if (medicaldata.IsAutopsyAssociatedDisease)
                                {
                                    ++NumAutopsyAssociatedDiseaseRead;
                                    if (NumAutopsyAssociatedDiseaseRead > 1)
                                        throw new XmlException("Number of 'AutopsyAssociatedDisease' records is greater than one, only one is allowed.");
                                    else
                                        AutopsyAssociatedDisease = medicaldata.AutopsyAssociatedDisease;
                                } else if (medicaldata.IsAutopsyOtherInformation)
                                {
                                    ++NumAutopsyOtherInformationRead;
                                    if (NumAutopsyOtherInformationRead > 1)
                                        throw new XmlException("Number of 'AutopsyOtherInformation' records is greater than one, only one is allowed.");
                                    else
                                        AutopsyOtherInformation = medicaldata.AutopsyOtherInformation;
                                } else
                                    throw new XmlException("Medical data of unknown type has occurred. Data kind: "
                                        + medicaldata.DataKind + ", data type: " + medicaldata.DataType);
                            }
                        }
                        data.GoToMark("LastMedicalDataContainer");
                    }  // if container was found
                }  // while (data.Current != null)  - reading individual diagnosis codes
                // Prepare position for next elements:
                if (data.GoToMark("LastMedicalDataContainer") == null)
                    data.GoToMark("BeforeMedicalDataElements");
                data.RemoveMarks("BeforeMedicalDataElements"); // remove all marks used when reading the diagnosis code's data
                // Checks below are mainly moved to data consistency checks:
                if (!MedicalDataRead) { }
                if (!DiagnosisTextRead) { }
                if (NumDiagnosisTextRead > 1)
                {
                    throw new XmlException("There are more than one diagnosis free text desctiptions, at most one is allowed.");
                }
                if (!DiagnosisCodesRead) { }
                if (NumDiagnosisCodesRead < 1) { }
                // End of reading medical information.
                
                // Check data consistency:
                location = "Data consistency check";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)","Checking consistency...");
                AccummulatedReport rep = new AccummulatedReport(R, location, true);
                CheckConsistency(rep);
                rep.Report();

            }
            catch (Exception ex)
            {
                R.ReportError("Location: " + location + " ", ex);
                throw ReporterBase.ReviseException(ex,
                        "MsgObervationOrder.Read(XmlDocument); " + location + ": ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Finished.");
                --R.Depth;
            }

        }  // Read(XmlDocument)


        /// <summary>Check correctness and consistency of data and creates a report on this.</summary>
        /// <param name="rep">Object where error, warning and info reports are accumulated.</param>
        public override void CheckConsistency(AccummulatedReport rep)
        {
            if (rep == null)
                throw new ArgumentNullException("Report object is not specified (null argument).");
            else
            {
                try
                {
                    // Check validity of the message type:
                    if (Type != MessageType.SpecimenObservationOrder)
                        rep.AddError("Wrong message type: " + Type.ToString() + " instead of " 
                            + MessageType.SpecimenObservationOrder.ToString() + ".");
                    // Check validity of the observation type code:
                    if (ObservationType != Const.ObservationTypePatology && ObservationType != Const.ObservationTypeCitology
                        && ObservationType != Const.ObservationTypeHistology)
                    {
                        rep.AddError("Unknown observation type code: \"" + ObservationType + "\".");
                    }
                    // Check specifics of different kinds of observations:
                    if (ObservationType == Const.ObservationTypePatology)
                    {
                        // On pathological observation there should be no samples:
                        if (NumSamples > 0)
                            rep.AddWarning("There are samples defined on a patological observation.");
                        // Check whether the death reason is specified:
                        if (AutopsyDeathReason == null)
                            rep.AddError("Death reason is not specified for a pathological observation.");
                        else if (string.IsNullOrEmpty(AutopsyDeathReason.Text))
                            rep.AddError("Death reason is not specified for a pathological observation (Text is empty).");
                        // Check whether the basic disease is specified:
                        if (AutopsyBasicDisease == null)
                            rep.AddError("Basic disease is not specified for a pathological observation.");
                        else if (string.IsNullOrEmpty(AutopsyBasicDisease.Text))
                            rep.AddError("Basic disease is not specified for a pathological observation (Text is empty).");
                        // Check whether the associated diseases are specified:
                        if (AutopsyAssociatedDisease == null)
                            rep.AddError("Associated diseases are not specified for a pathological observation.");
                        else if (string.IsNullOrEmpty(AutopsyAssociatedDisease.Text))
                            rep.AddError("Associated diseases are not specified for a pathological observation (Text is empty).");
                        // Check whether the associated questions and other informatio are specified:
                        if (AutopsyOtherInformation == null)
                            rep.AddError("Questions and other information are not specified for a pathological observation.");
                        else if (string.IsNullOrEmpty(AutopsyOtherInformation.Text))
                            rep.AddError("Questions and other information are not specified for a pathological observation (Text is empty).");
                    }


                    // Check order data:
                    if (string.IsNullOrEmpty(BisOrderId))
                        rep.AddError("BIS Order Id is not specified.");
                    if (Ordertime == null)
                        rep.AddError("Order time is not specified.");
                    if (Const.ConvertActionPriority(Priority) == ActionPriority.Unknown)
                        rep.AddError("Order priority is not specified.");
                    
                    // Check sample data:
                    if (Samples != null)
                    {
                        for (int i = 0; i < Samples.Count; ++i)
                        {
                            Sample sample = Samples[i];
                            if (string.IsNullOrEmpty(sample.BisId))
                                rep.AddError("Sample No. " + (i+1).ToString() + ": BIS sample ID is not specified.");
                            if (string.IsNullOrEmpty(sample.SampleCode))
                                rep.AddError("Sample No. " + (i+1).ToString() + ": Sample type code is not specified.");
                            if (sample.NumPieces == null)
                                rep.AddError("Sample No. " + (i + 1).ToString() + ": Number of pieces is not specified.");
                        }
                    }

                    // Check patient data:
                    int numpatients = 0;
                    if (Patients!=null)
                        numpatients = Patients.Count;

                    if (numpatients < 1)
                        rep.AddError("There are no patient records.");
                    else
                    {
                        if (numpatients > 1)
                            rep.AddError("There are more than one (" + numpatients.ToString() + ") patients on the observation order.");
                        for (int i = 0; i < numpatients; ++i)
                        {
                            Patient patient = Patients[i];
                            if (patient == null)
                            {
                                if (numpatients == 1)
                                    rep.AddError("Patient data is not specified.");
                                else
                                    rep.AddError("Patient data is not specified for patient No. " + (i + 1).ToString());
                            } else
                            {
                                // Check patient's data:
                                if (string.IsNullOrEmpty(patient.BisId))
                                    rep.AddError("Patient's BIS Id is not specified.");
                                if (string.IsNullOrEmpty(patient.Surname))
                                {
                                    patient.Surname = Const.MissingDataString;
                                    rep.AddWarning("Patient's surname is not specified. Replaced by " + Const.MissingDataString + ".");
                                }
                                if (string.IsNullOrEmpty(patient.Name))
                                {
                                    patient.Name = Const.MissingDataString;
                                    rep.AddWarning("Patient's name is not specified. Replaced by " + Const.MissingDataString + ".");
                                }
                                if (patient.IsMale == null)
                                    rep.AddWarning("Patient's geder is not specified.");
                                if (patient.IsAlive == null)
                                    rep.AddWarning("It is not specified whether patient is dead or alive.");

                                // Check consistency of data for deceived or alive patient:
                                if (Const.OoCheckAliveConsistency)
                                {
                                    if (PatientIsAlive)
                                    {
                                        // Patient is alive:
                                        if (ObservationType == Const.ObservationTypePatology)
                                            rep.AddError("Patient is alive but type of the research is autopsy.");
                                        if (patient.TimeOfDeathStr != null)
                                            rep.AddError("Patient is alive but time of death is specified.");
                                    }
                                    else
                                    {
                                        // Patient is dead:
                                        if (ObservationType != Const.ObservationTypePatology)
                                            rep.AddWarning("Patient is deceased but observation type is not autopsy.");
                                        if (AutopsyDeathReason == null && AutopsyBasicDisease == null
                                            && AutopsyAssociatedDisease == null && AutopsyOtherInformation == null)
                                            rep.AddWarning("Patient is deceased but none of the autopsy declaration fields is specified.");
                                    }
                                    // In the case of autopsy order, check that date of death is not null!
                                    if (ObservationType == Const.ObservationTypePatology)
                                    {
                                        // Currently we have limitation that for pathological observation time of teatu must be specified.
                                        if (patient != null)
                                            if (patient.TimeOfDeath == null)
                                                rep.AddError("Time of death is not specified for an autopsy order.");
                                    }
                                    else
                                    {
                                        // If observation is not an autopsy then check if date of death is not specified!
                                        if (patient != null)
                                            if (patient.TimeOfDeath != null)
                                                rep.AddWarning("Time of death is specified but observation type is not autopsy.");
                                    }
                                }
                            }  // patient's data
                        }
                    }

                    // Check author data:
                    int numauthors = 0;
                    if (Authors != null)
                        numauthors = Authors.Count;
                    if (numauthors < 1)
                        rep.AddError("Author of the order is not specified.");
                    else
                    {
                        if (numauthors > 1)
                            rep.AddError("There are " + numauthors.ToString() + " observation authors, should be only one.");
                        for (int i = 0; i < numauthors; ++i)
                        {
                            Author author = Authors[i];
                            if (author == null)
                            {
                                if (numauthors == 1)
                                    rep.AddError("Author data is not specified (null reference).");
                                else
                                    rep.AddError("Author data is not specified for author No. " + i.ToString() + " (null reference).");
                            } else
                            {
                                if (string.IsNullOrEmpty(author.BisId))
                                    rep.AddError("Author's BPI (BIS Id) is not specified.");
                                if (string.IsNullOrEmpty(author.BisIdOrganization))
                                    rep.AddError("Author's organization's BIS ID is not specified.");

                                if (string.IsNullOrEmpty(author.CountryCode))
                                {
                                    rep.AddWarning("Author's country code is not specified.");
                                    author.CountryCode = Const.MissingDataString;
                                }
                                if (string.IsNullOrEmpty(author.City))
                                {
                                    rep.AddWarning("Author's city is not specified.");
                                    author.City = Const.MissingDataString;
                                }
                                if (string.IsNullOrEmpty(author.PostalCode))
                                {
                                    rep.AddWarning("Author's postal code is not specified.");
                                    author.PostalCode = Const.MissingDataString;
                                }
                                if (string.IsNullOrEmpty(author.StreetAddress))
                                {
                                    rep.AddWarning("Author's street address is not specified.");
                                    author.StreetAddress = Const.MissingDataString;
                                }

                                if (string.IsNullOrEmpty(author.Name))
                                    rep.AddError("Author's name is not specified.");
                                if (string.IsNullOrEmpty(author.Surname))
                                    rep.AddError("Author's surname is not specified.");
                            }
                        }
                    }


                    // Check whether clinical diagnosis text is defined (it can be missing only if observation type is autopsy):
                    bool HasClinicalDiagnosisText = false;
                    if (DiagnosisTexts != null)
                        if (DiagnosisTexts.Count > 0)  // There can be only one clinical diagnosis Text!
                        {
                            DiagnosisText diagnosistext = DiagnosisTexts[0];
                            if (diagnosistext != null)
                            {
                                if (!string.IsNullOrEmpty(diagnosistext.Text))
                                    HasClinicalDiagnosisText = true;
                            }
                        }
                    if (!HasClinicalDiagnosisText && ObservationType != Const.ObservationTypePatology)
                        rep.AddError("Clinical diagnosis text is missing but observation type is not autopsy.");

                }
                catch (Exception ex)
                {
                    rep.AddError("Exception has been thrown during data consistency check. Details: " + Environment.NewLine + ex.Message);
                }
            }
        }  // CheckConsistency()




        /// <summary>Gets number of samples.</summary>
        public int NumSamples
        {
            get
            {
                if (Samples!=null)
                    return Samples.Count;
                else
                    return 0;
            }
        }

        public List<Observation> Observations = new List<Observation>();
        public List<Sample> Samples = new List<Sample>();
        public List<Patient> Patients = new List<Patient>();
        public List<Author> Authors = new List<Author>();
        public List<DiagnosisText> DiagnosisTexts = new List<DiagnosisText>();
        public List<DiagnosisCodeClass> DiagnosisCodes = new List<DiagnosisCodeClass>();

        /// <summary>Obduction declaration data:</summary>
        public AutopsyDeathReason AutopsyDeathReason = null;
        public AutopsyBasicDisease AutopsyBasicDisease = null;
        public AutopsyAssociatedDisease AutopsyAssociatedDisease = null;
        public AutopsyOtherInformation AutopsyOtherInformation = null;


        OoAction Action = OoAction.Unknown;
        string
            ActionRoot = null,
            ActionCode = null;

        DateTime? CreationTime = null;  // time of message creation
        DateTime? Ordertime = null;   // order time



        string
            // MessageNumber = null, MessageNumberOid = null, // msg ID as written in the msg
            // MessageReceiver = null, MessageReceiverOid = null, // msg receiver
            // MessageResponder = null, MessageResponderOid = null,  // msg responder
            // MessageSender = null, MessageSenderOid = null,  // msg sender
            MessageCode = null,  // msg code, must correspond to Action
            MessageCodeSystem = null,
            BisOrderId = null, BisOrderIdOid = null,  // BIS order ID
            ObservationType = null, RsrTypeCodeSystem = null,  // koda preiskave
            OrderStatus = null,  // status naročila
            OrderComment = null,  // opomba naročila
            Priority = null, PriorityCodeSystem = null; // prioriteta naročila (R, UR, T)

        /// <summary>Returns BIS Order Id for this msg.</summary>
        public string GetBisOrderId() { return BisOrderId; }

        /// <summary>Indicates whether patient is alive.</summary>
        public bool PatientIsAlive
        {
            get
            {
                bool ret=false;
                if (Patients != null)
                    if (Patients.Count > 0)
                    {
                        Patient p = Patients[0];
                        if (p.IsAlive != null)
                            ret = p.IsAlive.Value;
                    }
                return ret;
            }
        }

        public override string ToString()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.ToString()", "Started...");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("Data from web service: ");
                if (string.IsNullOrEmpty(MessageId))
                    sb.AppendLine("  Message ID is not specified. ");
                else
                    sb.AppendLine("  Message ID: " + MessageId.ToString());
                if (string.IsNullOrEmpty(MessageXml))
                    sb.AppendLine("  Message XML is not stored.");
                else
                    sb.AppendLine("  Length of the message XML string: " + MessageXml.Length.ToString() + " bytes.");
                if (string.IsNullOrEmpty(MessageFile))
                    sb.AppendLine("Message file is not specified.");
                else
                    sb.AppendLine("Messge file: " + Environment.NewLine + "  " + MessageFile);
                sb.AppendLine();
                sb.AppendLine("Mesage " + Type.ToString() + ":");
                sb.AppendLine("Message Id: " + MessageNumber + "; OID: " + MessageNumberOid);

                if (CreationTime == null)
                    sb.AppendLine("Message creation time is not specified.");
                else
                    sb.AppendLine("Message creation time: " + CreationTime.ToString());

                sb.AppendLine("Action to be performed: " + Action.ToString());
                sb.AppendLine("Action code: " + ActionCode + "; Root: " + ActionRoot);

                sb.AppendLine("Message receiver: " + MessageReceiver + ", OID: " + MessageReceiverOid);
                sb.AppendLine("Message responder: " + MessageResponder + ", OID: " + MessageResponderOid);
                sb.AppendLine("Message sender: " + MessageSender + ", OID: " + MessageSenderOid);
                sb.AppendLine();
                sb.AppendLine("Message contents:");

                sb.AppendLine("Message code: " + MessageCode);
                if (Const.ConvertOoAction(MessageCode) == Action)
                    sb.AppendLine("Agreement with action: Yes");
                else
                    sb.AppendLine("Agreement with action: No.");
                sb.AppendLine("Sender's order ID: " + BisOrderId);
                sb.AppendLine("Observation code: " + ObservationType + ", code system: " + RsrTypeCodeSystem);
                sb.AppendLine("Order status: " + OrderStatus);
                sb.AppendLine("Order comment: " + Environment.NewLine + "\"" + OrderComment + "\"");
                sb.AppendLine();
                if (Ordertime == null)
                    sb.AppendLine("Order time is not specified.");
                else
                    sb.AppendLine("Order time: " + Ordertime.ToString());
                sb.AppendLine("Order priority: " + Priority + ", Code system: " + PriorityCodeSystem);
                // Append samples data:
                if (Samples == null || Samples.Count < 1)
                    sb.AppendLine("No samples specified");
                else
                {
                    sb.AppendLine("This order contains " + Samples.Count.ToString() + " samples.");
                    for (int i = 0; i < Samples.Count; ++i)
                    {
                        sb.AppendLine("Sample No. " + i.ToString() + ":");
                        sb.Append(Samples[i].ToString());
                    }
                }
                // Append patients data:
                if (Patients == null || Patients.Count < 1)
                    sb.AppendLine("No patients specified");
                else for (int i = 0; i < Patients.Count; ++i)
                    {
                        sb.Append(Patients[i].ToString());
                    }
                // Append authors data:
                if (Authors == null || Authors.Count < 1)
                    sb.AppendLine("No ordering persons specified");
                else for (int i = 0; i < Authors.Count; ++i)
                    {
                        sb.Append(Authors[i].ToString());
                    }
                // Append observation data:
                if (Observations == null || Observations.Count < 1)
                    sb.AppendLine("No observations specified");
                else for (int i = 0; i < Observations.Count; ++i)
                    {
                        sb.Append(Observations[i].ToString());
                    }
                // Append diagnosis texts:
                if (DiagnosisTexts == null || DiagnosisTexts.Count < 1)
                    sb.AppendLine("No diagnosis texts specified");
                else
                {
                    sb.AppendLine("This order contains " + DiagnosisTexts.Count.ToString() + " diagnosis texts.");
                    for (int i = 0; i < DiagnosisTexts.Count; ++i)
                    {
                        sb.AppendLine("Diagnosis text No. " + i.ToString() + ":");
                        sb.Append(DiagnosisTexts[i].ToString());
                    }
                }
                // Append diagnosis codes:
                if (DiagnosisCodes == null || DiagnosisCodes.Count < 1)
                    sb.AppendLine("No diagnisis codes specified");
                else
                {
                    sb.AppendLine("This order contains " + DiagnosisCodes.Count.ToString() + " diagnisis codes.");
                    for (int i = 0; i < DiagnosisCodes.Count; ++i)
                    {
                        sb.AppendLine("Diagnisis code No. " + i.ToString() + ":");
                        sb.Append(DiagnosisCodes[i].ToString());
                    }
                }

                // Append data belonging to obduction declaration:
                if (!PatientIsAlive)
                {
                    // Patient is dead, therefore we would expect data for obduction:
                    sb.AppendLine();
                    if (AutopsyDeathReason == null)
                        sb.AppendLine("Field 'ObdDeathReason' is null." + Environment.NewLine);
                    else
                        sb.AppendLine(AutopsyDeathReason.ToString());
                    if (AutopsyBasicDisease == null)
                        sb.AppendLine("Field 'ObdBasicDisease' is null." + Environment.NewLine);
                    else
                        sb.AppendLine(AutopsyBasicDisease.ToString());
                    if (AutopsyAssociatedDisease == null)
                        sb.AppendLine("Field 'ObdAssociatedDisease' is null." + Environment.NewLine);
                    else
                        sb.AppendLine(AutopsyAssociatedDisease.ToString());
                    if (AutopsyOtherInformation == null)
                        sb.AppendLine("Field 'ObdOtherInformation' is null." + Environment.NewLine);
                    else
                        sb.AppendLine(AutopsyOtherInformation.ToString());
                }
                else
                {
                    // Patient is alive, therefore there should be no obduction data:
                    if (AutopsyDeathReason != null)
                        sb.AppendLine(Environment.NewLine + "Warning: patient is alive but 'ObdDeathReason' is specified:" + 
                            Environment.NewLine + AutopsyDeathReason.ToString());
                    if (AutopsyBasicDisease != null)
                        sb.AppendLine(Environment.NewLine + "Warning: patient is alive but 'ObdBasicDisease' is specified:" +
                            Environment.NewLine + AutopsyBasicDisease.ToString());
                    if (AutopsyAssociatedDisease != null)
                        sb.AppendLine(Environment.NewLine + "Warning: patient is alive but 'ObdAssociatedDisease' is specified:" +
                            Environment.NewLine + AutopsyAssociatedDisease.ToString());
                    if (AutopsyOtherInformation != null)
                        sb.AppendLine(Environment.NewLine + "Warning: patient is alive but 'ObdOtherInformation' is specified:" +
                            Environment.NewLine + AutopsyOtherInformation.ToString());
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.ToString()", "Finished.");
                --R.Depth;
            }
        }

        /// <summary>Converts a msg to Xml and returns it.</summary>
        public override XmlDocument ToXml()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.ToXml()", "Started...");
            try
            {
                Data.Doc = new XmlDocument();

                // Remark: this is currently not necessary, but implementation might be added in the (far) future!
                throw new NotImplementedException("Export of data to Xml is not implemented.");

            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.ToXml()", "Finished.");
                --R.Depth;
            }
        }


        #region MessageResponse
        // Methods for preparation of messages to be sent as response (e.g. for revocation of orders)

        // Preparation of observation order response:






        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// response to another observation order message, for the case where message is specified a
        /// string.
        /// Sender, receiver and responder are re-set consistently such that Labex becomes sender of the message,
        /// action is set to Nullify and comment is set in addition.</summary>
        /// <param name="message">String containing the message.</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        public static void PrepareOoNullify(ref string message, string comment)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message string is not specified.");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);
            bool changed = false;
            PrepareOoNullify(ref doc, comment, out changed);
            if (changed)
            {
                message = doc.OuterXml;
            }
        }


        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// response to another observation order message, for the case where message is specified a
        /// string.
        /// Sender, receiver and responder are re-set consistently such that Labex becomes sender of the message,
        /// action is set to Nullify and comment is set in addition.</summary>
        /// <param name="msg">XmlDocument containing the message.</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        public static void PrepareOoNullify(ref XmlDocument msg, string comment)
        {
            if (msg == null)
                throw new ArgumentNullException("Message document is not specified.");
            XmlBuilder data = new XmlBuilder();
            data.LoadXml(msg);
            bool changed = false;
            PrepareOoNullify(ref data, comment, out changed);
        }


        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// response to another observation order message, for the case where message is specified a
        /// string.
        /// Sender, receiver and responder are re-set consistently such that Labex becomes sender of the message,
        /// action is set to Nullify and comment is set in addition.</summary>
        /// <param name="msg">XmlDocument containing the message.</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        /// <param name="changed">Output flag indicating whether message contents has been changed by this function.</param>
        public static void PrepareOoNullify(ref XmlDocument msg, string comment, out bool changed)
        {
            changed = false;
            if (msg == null)
                throw new ArgumentNullException("Message document is not specified.");
            XmlBuilder data = new XmlBuilder();
            data.LoadXml(msg);
            PrepareOoNullify(ref data, comment, out changed);
        }



        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// nullify request for another observation order  message.
        /// Sender, receiver and responder are re-set consistently such that Labex becomes sender of the message,
        /// action is set to Nullify and comment is set in addition.</summary>
        /// <param name="data">XmlBuilder containing the message.</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        public static void PrepareOoNullify(ref XmlBuilder data, string comment)
        {
            string MessageSender = MsgConst.Const.LabexId;
            string MessageResponder = MsgConst.Const.LabexId;
            string MessageReceiver = MsgConst.Const.BisId;
            bool changed = false;
            PrepareOoResponse(ref data, OoAction.Nullify, comment,
                MessageSender, MessageReceiver, MessageResponder, out changed);
        }

        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// nullify request for another observation order  message.
        /// Sender, receiver and responder are re-set consistently such that Labex becomes sender of the message,
        /// action is set to Nullify and comment is set in addition.</summary>
        /// <param name="data">XmlBuilder containing the message.</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        /// <param name="changed">Output flag indicating whether message contents has been changed by this function.</param>
        public static void PrepareOoNullify(ref XmlBuilder data, string comment, out bool changed)
        {
            string MessageSender = MsgConst.Const.LabexId;
            string MessageResponder = MsgConst.Const.LabexId;
            string MessageReceiver = MsgConst.Const.BisId;
            PrepareOoResponse(ref data, OoAction.Nullify, comment,
                MessageSender, MessageReceiver, MessageResponder, out changed);
        }



        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// response to another observation order message.
        /// Contents of the message is modified according to parameters.</summary>
        /// <param name="message">String containing the message.</param>
        /// <param name="action">Action to be set on the message (if OoAction.Unknown then message is not modified!)</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        /// <param name="sender">Sender to be set on the message. If null or empty string then sender is not modified on the message.</param>
        /// <param name="receiver">Receiver to be set on the message. If null or empty string then receiver is not modified on the message.</param>
        /// <param name="responder">Responder to be set on the message. If null or empty string then responder is not modified on the message.</param>
        /// <param name="changed">Output flag indicating whether message contents has been changed by this function.</param>
        public static void PrepareOoResponse(ref string message, OoAction action, string comment,
            string sender, string receiver, string responder, out bool changed)
        {
            changed = false;
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message string is not specified.");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);
            PrepareOoResponse(ref doc, action, comment, sender, receiver, responder, out changed);
            if (changed)
            {
                message = doc.OuterXml;
            }
        }

        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// response to another observation order message.
        /// Contents of the message is modified according to parameters.</summary>
        /// <param name="msg">XmlDocument containing the message.</param>
        /// <param name="action">Action to be set on the message (if OoAction.Unknown then message is not modified!)</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        /// <param name="sender">Sender to be set on the message. If null or empty string then sender is not modified on the message.</param>
        /// <param name="receiver">Receiver to be set on the message. If null or empty string then receiver is not modified on the message.</param>
        /// <param name="responder">Responder to be set on the message. If null or empty string then responder is not modified on the message.</param>
        /// <param name="changed">Output flag indicating whether message contents has been changed by this function.</param>
        public static void PrepareOoResponse(ref XmlDocument msg, OoAction action, string comment,
            string sender, string receiver, string responder, out bool changed)
        {
            changed = false;
            if (msg == null)
                throw new ArgumentNullException("Message document is not specified.");
            XmlBuilder data = new XmlBuilder();
            data.LoadXml(msg);
            PrepareOoResponse(ref data, action, comment, sender, receiver, responder, out changed);
        }


        /// <summary>Prepares the observation order message to be sent to the Calypso web service as
        /// response to another observation order message.
        /// Contents of the message is modified according to parameters.</summary>
        /// <param name="data">XmlBuilder containing the message.</param>
        /// <param name="action">Action to be set on the message (if OoAction.Unknown then message is not modified!)</param>
        /// <param name="comment">Comment to be set on the message (if null then comment is not set, i.e. eventual
        /// current comment is preserved).</param>
        /// <param name="sender">Sender to be set on the message. If null or empty string then sender is not modified on the message.</param>
        /// <param name="receiver">Receiver to be set on the message. If null or empty string then receiver is not modified on the message.</param>
        /// <param name="responder">Responder to be set on the message. If null or empty string then responder is not modified on the message.</param>
        /// <param name="changed">Output flag indicating whether message contents has been changed by this function.</param>
        public static void PrepareOoResponse(ref XmlBuilder data, OoAction action, string comment,
            string sender, string receiver, string responder, out bool changed)
        {
            changed = false;
            if (data == null)
                throw new ArgumentException("Message builder is not specified.");
            bool ch = false;
            // Set action code on the message:
            if (action != OoAction.Unknown)
            {
                SetOoAction(ref data, action, out ch);
                if (ch)
                    changed = true;
            }
            // Set comment text on the message:
            if (comment != null)
            {
                SetOoComment(ref data, comment, out ch);
                if (ch)
                    changed = true;
            }
            // Set message sender:
            if (!string.IsNullOrEmpty(sender))
            {
                SetMessageSender(ref data, sender, out ch);
                if (ch)
                    changed = true;
            }
            // Set message receiver:
            if (!string.IsNullOrEmpty(receiver))
            {
                SetMessageReceiver(ref data, receiver, out ch);
                if (ch)
                    changed = true;
            }
            // Set message responder:
            if (!string.IsNullOrEmpty(responder))
            {
                SetMessageResponder(ref data, responder, out ch);
                if (ch)
                    changed = true;
            }
        }  // PrepareOoResponse




        // Change message comment:

        /// <summary>Sets the comment in the the Xml document containing the SpecimenObservationOrter
        /// message (this is contained in the "text" element of the message body)</summary>
        /// <param name="data">XML builder containing the message where comment text is to be set.</param>
        /// <param name="comment">The new comment to be set on the message.</param>
        /// <param name="changed">Indicates whether the underlying XML message has been changed (i.e., whether the new
        /// comment replaced the old one in the message). Message is not changed only when the new message equals the old
        /// one or when an error occurs that prevents setting the message.</param>
        public static void SetOoComment(ref XmlBuilder data, string comment, out bool changed)
        {
            changed = false;
            if (data == null)
                throw new ArgumentException("Message builder is not specified.");
            //if (string.IsNullOrEmpty(receiver))
            //    throw new ArgumentException("Receiver of the message is not specified.");
            data.GoToRoot();
            if (data.Current == null)
                throw new XmlException("Message XML does not contain a root element.");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Root element is empty.");
            // Step into message container:
            data.NextOrCurrentElement(Const.OoMessageContainer);
            if (data.Current == null)
                throw new XmlException("There is no message outer container - element " + Const.OoMessageContainer);
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message outer container does not contain any child nodes - element " + Const.OoMessageContainer);
            // Go deeper into subcontainers where significan contents is digged:
            data.NextElement(Const.MessageSubContainer);
            if (data.Current == null)
                throw new XmlException("There is no message subcontainer - element " + Const.MessageSubContainer);
            
            //// This control is probably not necessary here:
            //string aux = data.Attribute(Const.MessageSubContainerTypeAttribute);
            //if (aux != Const.MessageSubContainerTypeAttributeValue)
            //    throw new XmlException("Wrong " + Const.MessageSubContainerTypeAttribute + " attribute, element " + Const.MessageSubContainer
            //        + " (" + aux + " instead of " + Const.MessageSubContainerTypeAttributeValue + ").");
            
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("There is no message contents - inside element " + Const.MessageSubContainer);
            // Step into the mesage sub-subcontainer:
            data.NextOrCurrentElement(Const.OoMessageSubContainer2);
            //// This control is probably not needed here:
            //if ((aux = data.Attribute(Const.MessageTypeAttribute)) != Const.OoMessageType)
            //    throw new XmlException("Wrong message type: " + aux + ", shoud be: " + Const.OoMessageType + ".");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("There is no message contents - inside element " + Const.OoMessageSubContainer2);
            string MessageContentsMark = "MessageContents";
            data.SetMark(MessageContentsMark);

           // Go to order comment (if it exists):
            data.SetMark();
            data.NextElement(Const.OoCommentElement);
            if (data.Current == null)
            {
                // Currently there is no comment element in the message and we need to add it.
                // Find the most appropriate node after which the comment is inserted, according to specification:
                data.NextOrCurrentElement(Const.OoOrderStatusElement);
                if (data.Current == null)
                {
                    // There is no status element, check for observation code, after whihch text element can
                    // also be inserted:
                    data.GoToMark(MessageContentsMark);
                    data.NextOrCurrentElement(Const.CodeElement);
                }
                //// Commented parts below would enable inserting comments after two other nodes if the right one did not exist:
                //if (data.Current == null)
                //{
                //    // There is no status element, check for observation code, after whihch text element can
                //    // also be inserted:
                //    data.GoToMark(MessageContentsMark);
                //    data.NextOrCurrentElement(Const.CodeElement);
                //}
                //if (data.Current == null)
                //{
                //    // There is no observation code element, check for sender's order Id alement, after whihch text element can
                //    // also be inserted:
                //    data.GoToMark(MessageContentsMark);
                //    data.NextOrCurrentElement(Const.IdElement);
                //}

                if (data.Current == null)
                {
                    throw new XmlException("There is no suitable element (best suited is the the status code) after which comment could be inserted.");
                } else
                {
                    // Insert the comment element where comment will be set:
                    data.CreateElement(Const.OoCommentElement);
                    // Warning: this is hard-coded! This code must be modified if specification for the comment 
                    // element changes.
                    data.SetAttribute("encoding","TXT");
                    data.SetAttribute("mediaType","text/plain");
                    data.InsertAfter();
                    data.MoveToNewest();  // move to the inserted comment elment
                }
            }
            data.RemoveMarks(MessageContentsMark);
            data.NextOrCurrentElement(Const.OoCommentElement);
            if (data.Current == null)
                throw new XmlException("Could not locate or create the message comment element: " + Const.OoCommentElement);
            else
            {
                string OrderComment = data.Value;
                if (OrderComment != comment)
                {
                    // Set the new value of the comment:
                    data.SetValue(comment);
                    changed = true;
                }
            }
        }  // SetOoComment(...)




        // Change message action:

        /// <summary>Sets the receiver of the communication msg stored in msg to the specified receiver,
        /// and indicates through an output argument whether the receiver has been changed in msg.
        /// If the receiver stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="msg">Message document in which receiver should be set.</param>
        /// <param name="receiver">The new receiver of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetOoAction(ref string message, OoAction action, out bool changed)
        {
            changed = false;
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message string is not specified.");
            //if (string.IsNullOrEmpty(receiver))
            //    throw new ArgumentException("Receiver of the message is not specified.");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(message);
            SetOoAction(ref doc, action, out changed);
            if (changed)
            {
                message = doc.OuterXml;
            }
        }


        /// <summary>Sets the receiver of the communication msg stored in msg to the specified receiver,
        /// and indicates through an output argument whether the receiver has been changed in msg.
        /// If the receiver stated in the msg already equals to the specified one then nothing
        /// changes in the msg and the output indicator becomes false.</summary>
        /// <param name="msg">Message in which receiver should be set.</param>
        /// <param name="receiver">The new receiver of the msg.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetOoAction(ref XmlDocument msg, OoAction action, out bool changed)
        {
            changed = false;
            if (msg == null)
                throw new ArgumentNullException("Message document is not specified.");
            XmlBuilder data = new XmlBuilder();
            data.LoadXml(msg);
            SetOoAction(ref data, action, out changed);
        }



        /// <summary>Sets the action code of the Xml document containing the SpecimenObservationOrter
        /// message. 
        /// Action code is set consistently in such a way that both dependent elements are set (also the "message code").</summary>
        /// <param name="data">XML builder containing the message where action code is to be set.</param>
        /// <param name="action">The new action code to be set on the message.</param>
        /// <param name="changed">Indicates whether the msg has been changed (i.e., whether the new
        /// receiver has been written to the msg).</param>
        public static void SetOoAction(ref XmlBuilder data, OoAction action, out bool changed)
        {
            changed = false;
            if (data == null)
                throw new ArgumentException("Message builder is not specified.");
            //if (string.IsNullOrEmpty(receiver))
            //    throw new ArgumentException("Receiver of the message is not specified.");
            data.GoToRoot();
            if (data.Current == null)
                throw new XmlException("Message XML does not contain a root element.");
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Root element is empty.");
            // Get the current action code and set it to the prescribed value if it does not match:
            data.NextElement(Const.ActionElement);
            if (data.Current == null)
            {
                data.Back();
                throw new XmlException("Action specification is missing.");
            } else
            {
                string ActionCode = data.Attribute(Const.ActionAttribute);
                OoAction CurrentAction = Const.ConvertOoAction(ActionCode);
                if (CurrentAction != action)
                {
                    // Currently set action does not match the one prescribed by the argument, therefore we
                    // change it:
                    string NewActionCode = Const.ConvertOoAction(action);
                    data.SetAttribute(Const.ActionAttribute, NewActionCode);
                    changed = true;  // note that the underlying XML document has changed
                }
            }
            // Step into message container and locate the message code that must match with the action:
            data.NextOrCurrentElement(Const.OoMessageContainer);
            if (data.Current == null)
                throw new XmlException("There is no message outer container - element " + Const.OoMessageContainer);
            data.StepIn();
            if (data.Current == null)
                throw new XmlException("Message outer container does not contain any child nodes - element " + Const.OoMessageContainer);
            // Get the current message code and set it consisently with the prescribed action code if it does not match:
            data.NextOrCurrentElement(Const.CodeElement);
            if (data.Current == null)
            {
                data.Back();
                throw new XmlException("Message code is not specified - element " + Const.CodeElement);
            } else
            {
                string MessageCode = data.Attribute(Const.CodeAttribute);
                // This code must correspond to the action:
                OoAction control = Const.ConvertOoAction(MessageCode);
                if (control != action)
                {
                    string NewMessageCode = Const.ConvertOoCode(action);
                    data.SetAttribute(Const.CodeAttribute, NewMessageCode);
                    changed = true;  // note that the underlying XML document has changed
                }
            }
        }  // SetOoAction



        #endregion  // MessageResponse





    } // class MsgObervationOrder

}
