
using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace IG.Lib
{




    /// <summary>Represents the Attachment of a msg (intended e.g. for storage of XML findings).
    /// Remark: Currently XML findings are not stored in an attachment element.</summary>
    public class MessageAttachment
    // $A Igor Apr09, May09;
    {
        public string
            Id = null,
            IdOid = null,
            Text = null;
        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Message attachment:");
            sb.AppendLine("  ID: " + Id + ", OID: " +  IdOid);
            sb.AppendLine("  Text: " + Text);
            return sb.ToString();
        }
    }  // class DiagnosisCodeClass




    /// <summary>Class for holding and manipulating the data about observation event.
    /// Includes parsing an XML file, storing data internally, and transcription of read data to a PADO object
    /// that enables saving data to a database.</summary>
    public class MsgObservationEvent : MsgBaseWithModel
    // $A Igor Apr09 May09;
    {

        /// <summary>Default constructor, sets the type information.</summary>
        public MsgObservationEvent()
        {
            Type = MessageType.SpecimenObservationEvent;
        }



        /// <summary>Reads msg data from the internal XML document containing the msg.</summary>
        public override void Read()
        {
            Read(Data);
        }


        /// <summary>Read msg data from an XML document containing the msg.</summary>
        /// <param name="doc">Document containing the msg.</param>
        public override void Read(XmlParser data)
        {
            string location = "Document", aux = null;
            XmlNode auxnode = null;
            try
            {
                Id = null;
                FindingsId = null;
                location = "Document";
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Started...");
                // Check whether the document is loaded:
                if (data == null)
                    throw new ArgumentException("The XML parser (and builder) with message document is not specified (null reference).");
                if (data.Doc == null)
                    throw new XmlException("The XML message document is not loaded on the XML parser.");
                // Get the Root element and check its name:
                location = "Root";
                data.GoToRoot();
                if (data.Name != Const.OeRootName)
                    throw new XmlException("Wrong name of the root element: " + data.Name + " instead of " + Const.FtRootName);

                // Skip coments and read the msg Id:
                location = "Message Id";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading message ID...");
                if (data.StepIn() == null)
                    throw new XmlException("Root element does not contain any child nodes.");
                data.NextOrCurrentElement();
                if (data.Name != Const.IdElement)
                    throw new XmlException("The first subelement of the root element does not conteain message Id. Element name: " + data.Name);
                MessageNumberOid = data.Attribute(Const.IdOidAttribute);
                MessageNumber = data.Attribute(Const.IdIdAttribute);
                // Read creation time:
                location = "Creation time";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading creation time...");
                data.NextElement(Const.CreationTimeElement);
                if (data.Current == null)
                    throw new XmlException("Message creation time is missing.");
                aux = data.Attribute(Const.CreationTimeAttribute);
                if (string.IsNullOrEmpty(aux))
                    throw new XmlException("Badly formed creation time.");
                CreationTime = Const.ConvertTime(aux);
                // Read code of the action that should be performed:
                location = "Action code";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading action code...");
                data.NextElement(Const.ActionElement);
                if (data.Current == null)
                    throw new XmlException("Action specification is missing.");
                aux = data.Attribute(Const.ActionAttribute);
                ActionCode = aux; // This will set action!
                if (Action == OeAction.Unknown)
                    throw new XmlException("Badly formed action code.");
                ActionRoot = data.Attribute(Const.ActionOidAttribute);
                // read sender, receiver and responer of the msg:
                location = "Message sender, receiver and responder";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading data about sender, receiver and responder of the message...");
                while (data.Current != null &&
                    (data.Name != Const.ReceiverContainer && data.Name != Const.ResponderContainer && data.Name != Const.SenderContainer))
                {
                    data.NextElement();
                }
                if (data.Current == null)
                    throw new XmlException("Data about receiver, sender and responder of the message is missing.");
                while (data.Name == Const.ReceiverContainer || data.Name == Const.ResponderContainer || data.Name == Const.SenderContainer)
                {
                    auxnode = data.Current;
                    if (data.Name == Const.ReceiverContainer)
                    {
                        // read information on receiver of the msg:
                        location = "Receiver";
                        if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading receiver data...");
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
                        if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading responder data...");
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
                        if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading sender data...");
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
                // Read the attachments:
                location = "Attachments";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading message attachment(s)...");
                data.SetMark();
                data.NextOrCurrentElement(Const.OeAttachmentContainer);
                if (data.Current==null)
                    throw new XmlException("No attacments are defined.");
                while (data.Current != null && data.Name == Const.OeAttachmentContainer)
                {
                    data.RemoveMark(); 
                    data.SetMark();
                    // MessageAttachment attachment = new MessageAttachment();
                    data.StepIn();
                    data.NextOrCurrentElement(Const.IdElement);
                    if (data.Current == null)
                        throw new XmlException("Current attaxhment ID is not defined.");
                    FindingsId = data.Attribute(Const.IdIdAttribute);
                    FindingsIdOid = data.Attribute(Const.IdOidAttribute);
                    data.NextOrCurrentElement(Const.OeAttachmentElement);
                    if (data.Current == null)
                        throw new XmlException("Attachment text is not defined.");
                    else
                    {
                        FindingsString = Data.Current.InnerText;
                    }

                    // Attachments.Add(attachment);
                    data.StepOut();
                    data.NextElement(Const.OeAttachmentContainer);
                }
                data.BackToMark();
                data.NextNode();
                // Read the msg:
                location = "Message outer container";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading message contents...");
                data.NextOrCurrentElement(Const.OeMessageContainer);
                if (data.Current == null)
                    throw new XmlException("There is no message outer container - element " + Const.OeMessageContainer);
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("Message outer container does not contain any child nodes - element " + Const.OeMessageContainer);
                // Read the msg code:
                location = "Messge code";
                data.NextOrCurrentElement(Const.CodeElement);
                if (data.Current == null)
                    throw new XmlException("Message code is not specified - element " + Const.CodeElement);
                OeAction control = Action;  // store current value action (read before) to control match with MessageCode
                MessageCode = data.Attribute(Const.CodeAttribute);
                MessageCodeSystem = data.Attribute(Const.CodeSystemAttribute);  // this assignment will change the action
                // This code must correspond to the action:
                if (control != Action)
                    throw new XmlException("Message code (\"" + data.Attribute(Const.CodeAttribute) + "\") does not agree with the action read before ("
                        + control.ToString() + ").");
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
                data.NextOrCurrentElement(Const.OeMessageSubContainer2);
                if ((aux = data.Attribute(Const.MessageTypeAttribute)) != Const.OeMessageType)
                    throw new XmlException("Wrong message type: " + aux + ", shoud be: " + Const.OeMessageType + ".");
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("There is no message contents - inside element " + Const.OeMessageSubContainer2);
                
                
                // Read sender's order Id
                location = "Protocol number";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading protocol number...");
                data.NextOrCurrentElement(Const.IdElement);
                ProtocolNumber = data.Attribute(Const.IdIdAttribute);
                ProtocolNumberOid = data.Attribute(Const.IdOidAttribute);
                // Read observation code:
                location = "Observation code";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading observation code...");
                data.NextElement(Const.CodeElement);
                if (data.Current == null)
                    throw new XmlException("There is no observation code - element " + Const.CodeElement);
                RsrType = data.Attribute(Const.CodeAttribute);
                RsrTypeCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                // Read comment:
                location = "Comment";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading comment...");
                data.SetMark();
                data.NextOrCurrentElement(Const.OeCommentElement);
                if (data.Current == null)
                    data.BackToMark();
                else
                {
                    data.RemoveMark();
                    Comment = data.Value;
                }

                // Read Order status:
                location = "Order status";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading order status code...");
                data.NextElement(Const.OoOrderStatusElement);
                if (data.Current == null)
                    throw new XmlException("Order status is not specified - element " + Const.OoOrderStatusElement);
                OrderStatus = OoStatus.Unknown;
                string stroostatus = data.Attribute(Const.OoOrderStatusAttribute);
                if (!string.IsNullOrEmpty(stroostatus))
                {
                    OrderStatus = Const.ConvertOoStatus(stroostatus);
                    if (OrderStatus != OoStatus.Active && OrderStatus != OoStatus.Completed)
                        throw new XmlException("The following order status is not allowed in this message: " + stroostatus);
                } else
                    throw new XmlException("Order status is not defined - malformed attribute: " + Const.OoOrderStatusAttribute);
                // Read important times:
                location = "Message times";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading relevant times...");
                data.NextOrCurrentElement(Const.OeTimeContainer);
                if (data.Current == null)
                    throw new XmlException("Relevent times are not specified, no element named " + Const.OeTimeContainer);
                else
                {
                    data.StepIn();
                    data.NextOrCurrentElement(Const.OeOrderCreationTimeElement);
                    if (data.Current == null)
                    {
                        data.Back();
                        throw new XmlException("Order creation time is not specified.");
                    } else
                    {
                        aux = data.Attribute(Const.CreationTimeAttribute);
                        if (string.IsNullOrEmpty(aux))
                            throw new XmlException("Order creation time is not specified properly.");
                        else 
                            OrderCreationTime = Const.ConvertTime(aux);
                    }
                    data.NextOrCurrentElement(Const.OeOrderCompletionTimeElement);
                    if (data.Current == null)
                        data.Back();
                    else
                    {
                        aux = data.Attribute(Const.CreationTimeAttribute);
                        if (!string.IsNullOrEmpty(aux))
                        {
                            OrderCompletionTime = Const.ConvertTime(aux);    
                        }
                    }
                    data.StepOut();
                }
                data.SetMark();
                // Read patient data:
                location = "Patient data";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading patient data...");
                data.NextOrCurrentElement(Const.OePatientContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Patient data is not defined, element name: " + Const.OePatientContainer);
                }
                else
                {
                    data.SetMark();
                    data.StepIn();
                    data.NextOrCurrentElement(Const.OePatientSubcontainer);
                    if (data.Current == null)
                        throw new Exception("Invalid format of patient data, subcontainer not defined: " + Const.OePatientSubcontainer);
                    data.StepIn();
                    data.NextOrCurrentElement(Const.IdElement);
                    if (data.Current == null)
                        throw new XmlException("Patient ID is not defined.");
                    PatientId = data.Attribute(Const.IdIdAttribute);
                    PatientIdOid = data.Attribute(Const.IdOidAttribute);
                    data.BackToMark();
                    data.NextNode();
                }
                // Read author data:
                location = "Author data";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading author data...");
                data.NextOrCurrentElement(Const.OeAuthorContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Author data is not defined, element name: " + Const.OeAuthorContainer);
                }
                else
                {
                    data.SetMark();
                    data.StepIn();
                    data.NextOrCurrentElement(Const.OeAuthorSubcontainer);
                    if (data.Current == null)
                        throw new Exception("Invalid format of author data, subcontainer not defined: " + Const.OeAuthorSubcontainer);
                    data.StepIn();
                    data.NextOrCurrentElement(Const.IdElement);
                    if (data.Current == null)
                        throw new XmlException("Author ID is not defined.");
                    AuthorId = data.Attribute(Const.IdIdAttribute);
                    AuthorIdOid = data.Attribute(Const.IdOidAttribute);
                    data.BackToMark();
                    data.NextNode();
                }


                // Read verifier data:
                location = "Verifier data";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading verifier data...");
                data.NextOrCurrentElement(Const.OeVerifierContainter);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Verifier data is not defined, element name: " + Const.OeVerifierContainter);
                }
                else
                {
                    data.SetMark();
                    data.StepIn();
                    data.NextOrCurrentElement(Const.OeVerifierSubContainter);
                    if (data.Current == null)
                        throw new Exception("Invalid format of verifier data, subcontainer not defined: " + Const.OeVerifierSubContainter);
                    data.StepIn();
                    data.NextOrCurrentElement(Const.IdElement);
                    if (data.Current == null)
                        throw new XmlException("Verifier ID is not defined.");
                    VerifierId = data.Attribute(Const.IdIdAttribute);
                    VerifierIdOid = data.Attribute(Const.IdOidAttribute);
                    data.BackToMark();
                    data.NextNode();
                }

                // Read BIS order ID:
                location = "BIS Order Id";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Reading BIS Order Id...");
                data.NextOrCurrentElement(Const.OeBisOrderIdContainter);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("BIS orter Id not defined, element name: " + Const.OeBisOrderIdContainter);
                }
                else
                {
                    data.SetMark();
                    data.StepIn();
                    data.NextOrCurrentElement(Const.OeBisOrderIdSubContainter);
                    if (data.Current == null)
                        throw new Exception("Invalid format of BIS order Id, subcontainer not defined: " + Const.OeBisOrderIdSubContainter);
                    data.StepIn();
                    data.NextOrCurrentElement(Const.IdElement);
                    if (data.Current == null)
                        throw new XmlException("BIS Order Id is not defined.");
                    BisorderId = data.Attribute(Const.IdIdAttribute);
                    BisorderIdOid = data.Attribute(Const.IdOidAttribute);
                    data.BackToMark();
                    data.NextNode();
                }

                // Check data consistency:
                location = "Data consistency check";
                if (R.TreatInfo) R.ReportInfo("MsgObervationOrder.Read(doc)", "Checking consistency...");
                AccummulatedReport rep = new AccummulatedReport(R, location, true);
                CheckConsistency(rep);
                rep.Report();

            }
            catch (Exception ex)
            {
                R.ReportError("Location: " + location + " ",ex);
                throw ReporterBase.ReviseException(ex,
                        "MsgObservationEvent.Read(XmlDocument); " + location + ": ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.Read(doc)", "Finished.");
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

                    rep.AddInfo("Data consistency check is not yet implemented for the class MsgObservationEvent.");

                }
                catch (Exception ex)
                {
                    rep.AddError("Exception has been thrown during data consistency check. Details: " + Environment.NewLine + ex.Message);
                }
            }
        }  // CheckConsistency()




        /// <summary>Action is bound to order status, therefore this propety is implemented in this way!</summary>
        OeAction Action
        {
            set
            {
                _action = value;
            }
            get
            {
                if (OrderStatus == OoStatus.Active)
                    return OeAction.Activate;
                else if (OrderStatus == OoStatus.Completed)
                    return OeAction.Complete;
                else
                    return _action;
            }
        }

        /// <summary>Actioncode is bound to action, which is ensured by this property.</summary>
        string ActionCode
        {
            get { return Const.ConvertOeAction(Action); }
            set 
            {
                try
                {
                    Action = OeAction.Unknown;
                    Action = Const.ConvertOeAction(value);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not convert the following string code to OeAction enumerator: " + value 
                        +Environment.NewLine + "Details: " + ex.Message);
                }
            }
        }

        /// <summary>MessageCode is bound to action, which is ensured by this property.</summary>
        string MessageCode
        {
            get { return Const.ConvertOeActionCode(Action); }
            set
            {
                try
                {
                    Action = OeAction.Unknown;
                    Action = Const.ConvertOeAction(value);
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not convert the following string code to OeAction enumerator: " + value
                        + Environment.NewLine + "Details: " + ex.Message);
                }
            }
        }

            // ActionCode = Const.ConvertOeAction(Action);
            // MessageCode = Const.ConvertOeActionCode(Action);


        /// <summary>Postavi ID avtorizatorja, ta funkcija je narejena samo za testiranje in se v delovnem
        /// okolju ne sme uporabljati!</summary>
        public void SetVerifierId(string ID)
        {
            string old = VerifierId;
            VerifierId = ID;
            R.ReportWarning("MsgObservationEvent.SetVerifier","Verifier ID has been changed from \"" + old + "\" to \"" 
                + VerifierId + "\". " + Environment.NewLine + "This is only acceptable in testing. In working environment, user data must be set correctly.");
        }


        string
            ActionRoot = null,
            
            MessageCodeSystem = null,

            ProtocolNumber = null, ProtocolNumberOid = null,  // BIS order ID
            RsrType = null, RsrTypeCodeSystem = null,  // koda preiskave
            Comment = null,
            // OrderStatusString = null,  // status naročila

            PatientId = null,
            PatientIdOid = null,

            AuthorId = null,
            AuthorIdOid = null,
            VerifierId = null,
            VerifierIdOid = null,
            BisorderId = null,
            BisorderIdOid = null;


        OeAction _action = OeAction.Unknown;

        OoStatus OrderStatus = OoStatus.Unknown;  // status naročila

        DateTime 
            CreationTime = DateTime.Now;
            // OrderCreationTime = DateTime.Now, 
            //OrderCompletionTime = DateTime.Now;

        DateTime? 
            OrderCreationTime = null,
            OrderCompletionTime = null;
        

        // public List<MessageAttachment> Attachments = new List<MessageAttachment>();

        public int? Id; // ID of the rsr in the database (will be null if the data has not been read form a database)
        
        public string FindingsIdOid = "";

        private string _FindingsId = null;  // stores Findings ID in the case that msg is read from the file


        public string FindingsId
        {
            get
            {
                if (Findings != null)
                    if (Findings.Id != null)
                        return Findings.Id.Value.ToString();
                return _FindingsId;
            }
            private set
            {
                _FindingsId = value;
            }
        }

        // Document that contains findings
        public DocFindings Findings = null;


        // Storage of findings (dual storage / XmlDocument & string representation of XML):
        private string _findingsStr = null;
        private XmlDocument _findingsXml = null;


        public string aaa = null;


        /// <summary>Gets or sets findings (attachment) as XML string.
        /// This is coupled with FindingsXml.</summary>
        public string FindingsString
        {
            get
            {
                if (_findingsStr == null)
                    if (_findingsXml != null)
                        _findingsStr = _findingsXml.OuterXml;
                return _findingsStr;
            }
            set
            {
                _findingsStr = value;
                _findingsXml = null;
            }
        }

        /// <summary>Gets or sets findings (attachment) as XML string.
        /// This is coupled with FindingsXml.</summary>
        public string FindingsStringPartial
        {
            get
            {
                if (_findingsStr == null)
                    if (_findingsXml != null)
                        _findingsStr = _findingsXml.DocumentElement.OuterXml;
                return _findingsStr;
            }
            //set
            //{
            //    _findingsStr = value;
            //    _findingsXml = null;
            //}
        }

        /// <summary>Gets or sets findings (attachment) as XmlDocument.
        /// This is coupled with FindingsStringPartial.</summary>
        public XmlDocument FindingsXml
        {
            get
            {
                if (_findingsXml == null)
                    if (_findingsStr != null)
                    {
                        try
                        {
                            _findingsXml = new XmlDocument();
                            _findingsXml.LoadXml(_findingsStr);
                        }
                        catch { }
                    }
                return _findingsXml;
            }
            set
            {
                _findingsStr = null;
                _findingsXml = value;
            }
        }

        /// <summary>Loads findings from a file.</summary>
        public void LoadFindings(string FileName)
        {
            
            if (string.IsNullOrEmpty(FileName))
                throw new Exception("File name containing XML findings is not specified.");
            if (!File.Exists(FileName))
                throw new Exception("File name containing XML findings does not exist: " + FileName);
            XmlDocument doc = new XmlDocument();
            doc.Load(FileName);
            FindingsXml = doc;
        }



        public override string ToString()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToString()", "Started...");
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

                sb.AppendLine("Message creation time: " + CreationTime.ToString());
                sb.AppendLine("Action to be performed: " + Action.ToString());
                sb.AppendLine("Action code: " + ActionCode + "; Root: " + ActionRoot);

                sb.AppendLine("Message receiver: " + MessageReceiver + ", OID: " + MessageReceiverOid);
                sb.AppendLine("Message responder: " + MessageResponder + ", OID: " + MessageResponderOid);
                sb.AppendLine("Message sender: " + MessageSender + ", OID: " + MessageSenderOid);
                sb.AppendLine();

                sb.AppendLine("Attached findings ID: " +  FindingsId + "; OID: " + FindingsIdOid);
                if (FindingsStringPartial == null)
                    sb.AppendLine("Attached findings are not specified.");
                else
                    sb.AppendLine("Attached findings: " + Environment.NewLine + "\"" 
                        + FindingsStringPartial + "\"" + Environment.NewLine);
                //if (Attachments == null)
                //    sb.AppendLine("List of attachments is null!");
                //else if (Attachments.Count < 1)
                //    sb.AppendLine("There are no attachments.");
                //else {
                //    for (int i = 0; i < Attachments.Count; ++i)
                //    {
                //        sb.AppendLine("Attachment No. " + (i+1).ToString() + ":");
                //        sb.AppendLine(Attachments[i].ToString());
                //    } }
                Console.WriteLine();
                sb.AppendLine("Message contents:");
                sb.AppendLine("Message code: " + MessageCode);
                if (Const.ConvertOeAction(MessageCode) == Action)
                    sb.AppendLine("Agreement with action: Yes");
                else
                    sb.AppendLine("Agreement with action: No.");



                sb.AppendLine("Order Protocol number: " + ProtocolNumber);
                sb.AppendLine("Observation code: " + RsrType + ", code system: " + RsrTypeCodeSystem);
                sb.AppendLine("Event's comment: " + Environment.NewLine + "\"" + Comment + "\"");
                sb.AppendLine("Order status: " + OrderStatus);
                sb.AppendLine();
                sb.AppendLine("Order creation time: " + OrderCreationTime.ToString());
                sb.AppendLine("Event creation time: " + OrderCompletionTime.ToString());

                sb.AppendLine("Patient ID: " + PatientId + "; OID: " + PatientIdOid);
                sb.AppendLine("Author ID: " + AuthorId + "; OID: " + AuthorIdOid);
                sb.AppendLine("Verifier ID: " + VerifierId + "; OID: " + VerifierIdOid);

                sb.AppendLine("BIS Order ID: " + BisorderId + "; OID: " + BisorderIdOid);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                R.ReportError(ex);
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToString()", "Finished.");
                --R.Depth;
            }
        }


       static int IdWithinSession = 0;

        /// <summary>Returns a generated msg Id.</summary>
        string GenerateMessageId()
        {
            ++IdWithinSession;
            string ret = Const.ConvertTime(DateTime.Now, true /* includetime */, false /* underscores */);
            if (PatientId==null)
                ret+= "__" + PatientId.ToString() ;
            else
                ret+= "__xx";
            ret+= "_" +  IdWithinSession.ToString();
            return ret;
        }




        /// <summary>Converts a msg to Xml and returns it.</summary>
        public override XmlDocument ToXml()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Started...");
            string location = "Document";
            try
            {
                // Generate Message number, which will at the same time serve as msg ID:
                if (string.IsNullOrEmpty(MessageNumber))
                    MessageNumber = GenerateMessageId();

                Data.Doc = ModelDocument;
                if (Data.Doc == null)
                    throw new Exception("Model document for creation of a message is not specified.");
                // Get the Root element and check its name:
                location = "Root";
                Data.GoToRoot();
                if (Data.Name != Const.OeRootName)
                {
                    throw new XmlException("Wrong name of the root element: " + Data.Name + " instead of " + Const.FtRootName);
                }
                else 
                {
                    Data.StepIn();
                    Data.NextOrCurrentElement();
                    if (Data.Current == null)
                        throw new XmlException("Root element does not contain any elements.");
                }
                // Set extension attrubite of the root node to Message Id:
                Data.SetAttribute(Const.ExtensionAttribute,MessageNumber);
                // Set the Message Id: 
                location = "Message Id";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message Id...");
                Data.NextOrCurrentElement(Const.IdElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Message Id element does not exist in the model message: " + Const.IdElement);
                }
                else
                {
                    Data.SetAttribute(Const.IdIdAttribute, MessageNumber);
                }
                // Set the Message Creation time: 
                location = "Message creation time:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message creation time...");
                Data.NextOrCurrentElement(Const.CreationTimeElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Creation time element does not exist in the model message: " + Const.CreationTimeElement);
                }
                else
                {
                    Data.SetAttribute(Const.CreationTimeAttribute, Const.ConvertTime(DateTime.Now,true,false));
                }
                // Set the Message actionAttribute: 
                location = "Message action:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message action ...");
                Data.NextOrCurrentElement(Const.ActionElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Action element does not exist in the model message: " + Const.ActionElement);
                } else
                {
                    Data.SetAttribute(Const.ActionAttribute, Const.ConvertOeAction(Action));
                }
                // Set the Message Receiver: 
                location = "Message receiver:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message receiver ...");
                Data.NextOrCurrentElement(Const.ReceiverContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Receiver container does not exist in the model message: " + Const.ReceiverContainer);
                }
                else
                {
                    Data.SetMark("Container");
                    Data.StepIn();
                    Data.NextOrCurrentElement(Const.ReceiverSubContainer);
                    if (Data.Current == null)
                    {
                        Data.Back();
                        throw new XmlException("Receiver container does not contain sub-container: " + Const.ReceiverSubContainer);
                    }
                    else
                    {
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                        {
                            throw new XmlException("Message receiver container does not contain the Id element in the model message.");
                        }
                        else
                        {
                            // Set the receiver attribute
                            Data.SetAttribute(Const.IdIdAttribute, MessageReceiver);
                        }
                    }
                    Data.BackToMark("Container");
                }
                // Set the Message Responder: 
                location = "Message responder:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message responder ...");
                Data.NextOrCurrentElement(Const.ResponderContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Responder container does not exist in the model message: " + Const.ResponderContainer);
                }
                else
                {
                    Data.SetMark("Container");
                    Data.StepIn();
                    Data.NextOrCurrentElement(Const.ResponderSubContainer);
                    if (Data.Current == null)
                    {
                        throw new XmlException("Responder container does not contain sub-container: " + Const.ResponderSubContainer);
                    }
                    else
                    {
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                        {
                            throw new XmlException("Message responder container does not contain the Id element in the model message.");
                        }
                        else
                        {
                            // Set the responder attribute
                            Data.SetAttribute(Const.IdIdAttribute, MessageResponder);
                        }
                    }
                    Data.BackToMark("Container");
                }
                // Set the Message Sender: 
                location = "Message sender:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message sender ...");
                Data.NextOrCurrentElement(Const.SenderContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Sender container does not exist in the model message: " + Const.SenderContainer);
                }
                else
                {
                    Data.SetMark("Container");
                    Data.StepIn();
                    Data.NextOrCurrentElement(Const.SenderSubContainer);
                    if (Data.Current == null)
                    {
                        throw new XmlException("Sender container does not contain sub-container: " + Const.SenderSubContainer);
                    } else
                    {
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                        {
                            throw new XmlException("Message sender container does not contain the Id element in the model message.");
                        } else
                        {
                            // Set the sender attribute
                            Data.SetAttribute(Const.IdIdAttribute, MessageSender);
                        }
                    }
                    Data.BackToMark("Container");
                }

                // Set the Attachment - XML findings: 
                location = "Attachment - XML findings:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting message attachment - XML findings...");
                Data.NextOrCurrentElement(Const.OeAttachmentContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Attachment container does not exist in the model message: " + Const.OeAttachmentContainer);
                }
                else
                {
                    if (FindingsXml != null)
                    {
                        Data.SetMark("Findings");
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                            throw new XmlException("Attachment Id element is not definet in the model message. Container: "
                                + Const.OeAttachmentContainer);
                        Data.SetAttribute(Const.IdIdAttribute, FindingsId);
                        Data.NextOrCurrentElement(Const.OeAttachmentElement);
                        if (Data.Current == null)
                            throw new XmlException("Attachment element does not exist in the model message: " + Const.OeAttachmentElement);
                        // $$$$ Change: instead as inserting an XML node, the findings document will be inserted as text:
                        // Data.SetInnerXml(FindingsStringPartial);
                        Data.SetInnerText(FindingsStringPartial);

                        //try
                        //{
                        //    // JUST FOR TEST:
                        //    // Save text as XML in order to see how findings attachment is generated:
                        //    string FileName = @"c:\temp\FindingsAttachment.xml";
                        //    Console.WriteLine("************************************************");
                        //    Console.WriteLine("Saving attached findings to the file " + FileName + "...");
                        //    Console.WriteLine("Length of the findings string: " + FindingsStringPartial.Length);
                        //    XmlDocument doc = new XmlDocument();
                        //    doc.LoadXml(FindingsStringPartial);
                        //    doc.Save(FileName);
                        //    Console.WriteLine("Saving done.");

                        //    Console.WriteLine("************************************************");
                        //}
                        //catch(Exception ex)
                        //{
                        //    R.ReportError(ex);
                        //}

                        Data.BackToMark("Findings");
                    } else
                    {
                        Data.RemoveCurrent();
                    }
                }
                // Message contents: 
                location = "Message contents:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting contents...");
                Data.NextOrCurrentElement(Const.OeMessageContainer);
                Data.StepIn();
                Data.NextOrCurrentElement();
                if (Data.Current == null)
                    throw new XmlException("Message container is not defined in the model message or contains no elements: " 
                        + Const.OeMessageContainer);
                Data.NextOrCurrentElement(Const.CodeElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Code element does not exist in the message container of the model message.");
                } else
                {
                    Data.SetAttribute(Const.CodeAttribute,Const.ConvertOeActionCode(Action));
                }
                Data.NextOrCurrentElement(Const.MessageSubContainer);
                Data.StepIn();
                Data.NextOrCurrentElement(Const.OeMessageSubContainer2);
                Data.StepIn();
                Data.NextOrCurrentElement();
                if (Data.Current == null)
                    throw new Exception("Message sub-subcontainer does not exist in the model message or it contains no elemens: "
                        + Const.OeMessageSubContainer2);
                Data.NextOrCurrentElement();
                // Protocol number: 
                location = "Protocol number:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting protocol number...");
                Data.NextOrCurrentElement(Const.IdElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Protocol number is not defined in the model message. Element: " + Const.IdElement);
                } else
                {
                    Data.SetAttribute(Const.IdIdAttribute, ProtocolNumber);
                }
                // Comment: 
                location = "Comment:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting comment...");
                Data.NextOrCurrentElement(Const.OeCommentElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Comment is not defined in the model message. Element: " + Const.OeCommentElement);
                } else
                {
                    if (string.IsNullOrEmpty(Comment))
                        Data.RemoveCurrent();
                    else
                        Data.SetInnerText(Comment);
                }
                // OrderStatus:
                location = "Order status:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting order status...");
                Data.NextOrCurrentElement(Const.OoOrderStatusElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Order status element does not exist in the model message: " + Const.OoOrderStatusElement);
                } else
                {
                    Data.SetAttribute(Const.OoOrderStatusAttribute, Const.ConvertOoStatus(OrderStatus));
                }
                // Relevant times:
                location = "Relevant times";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting relevant times...");
                Data.NextOrCurrentElement(Const.OeTimeContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Container for relevant times is not defined in the model message: " + Const.OeTimeContainer);
                }
                else
                {
                    Data.SetMark("Times");
                    Data.StepIn();
                    Data.NextOrCurrentElement();
                    if (Data.Current == null)
                        throw new XmlException("Container for relevane times does not contain any elements in the model message: " 
                            + Const.OeTimeContainer);
                    // Order creation time:
                    Data.NextOrCurrentElement(Const.OeOrderCreationTimeElement);
                    if (Data.Current == null)
                    {
                        Data.Back();
                        throw new XmlException("Order creation time element does not exist in the model message: "
                            + Const.OeOrderCreationTimeElement);
                    } else
                    {
                        if (OrderCreationTime == null)
                        {
                            Data.RemoveCurrent();
                            throw new InvalidDataException("Order creation time is not specified.");
                        } else
                        {
                            Data.SetAttribute(Const.CreationTimeAttribute,
                                Const.ConvertTime(OrderCreationTime.Value,true /* includetime */,false /* underscores */));
                        }
                    }

                    // Order completion time:
                    Data.NextOrCurrentElement(Const.OeOrderCompletionTimeElement);
                    if (Data.Current == null)
                    {
                        Data.Back();
                        throw new XmlException("Order completion time element does not exist in the model message: "
                            + Const.OeOrderCompletionTimeElement);
                    } else
                    {
                        if (OrderCompletionTime == null)
                        {
                            Data.RemoveCurrent();
                            throw new InvalidDataException("Order completion time is not specified.");
                        } else
                        {
                            Data.SetAttribute(Const.CreationTimeAttribute,
                                Const.ConvertTime(OrderCompletionTime.Value,true /* includetime */,false /* underscores */));
                        }
                    }
                    Data.BackToMark("Times");
                }
                // Patient:
                location = "Patient";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting patient ID...");
                Data.NextOrCurrentElement(Const.OePatientContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Patient container does not exist in the model message: " + Const.OePatientContainer);
                } else
                {
                    if (PatientId == null)
                        Data.RemoveCurrent();
                    else 
                    {
                        Data.SetMark("Patient");
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.OePatientSubcontainer);
                        if (Data.Current == null)
                            throw new XmlException("Patient container does not contain the sub-container: " + Const.OePatientSubcontainer);
                        // Step into subcontainer:
                        Data.StepIn();
                        Data.NextOrCurrentElement();
                        if (Data.Current == null)
                            throw new XmlException("Patient subcontainer does not contain any elements in the model message. Container element: "
                            + Const.OePatientSubcontainer);
                        // Set patient's Id:
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                            throw new XmlException("Patient subcontainer does not contain an ID element in the model message.");
                        else
                            Data.SetAttribute(Const.IdIdAttribute, PatientId);
                        Data.BackToMark("Patient");
                    }
                }
                // Author:
                location = "Author";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting author ID...");
                Data.NextOrCurrentElement(Const.OeAuthorContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Author container does not exist in the model message: " + Const.OeAuthorContainer);
                } else
                {
                    if (AuthorId == null)
                        Data.RemoveCurrent();
                    else 
                    {
                        Data.SetMark("Author");
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.OeAuthorSubcontainer);
                        if (Data.Current == null)
                            throw new XmlException("Author container does not contain the sub-container: " + Const.OeAuthorSubcontainer);
                        // Step into subcontainer:
                        Data.StepIn();
                        Data.NextOrCurrentElement();
                        if (Data.Current == null)
                            throw new XmlException("Author subcontainer does not contain any elements in the model message. Container element: "
                            + Const.OeAuthorSubcontainer);
                        // Set author's Id:
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                            throw new XmlException("Author subcontainer does not contain an ID element in the model message.");
                        else
                            Data.SetAttribute(Const.IdIdAttribute, AuthorId);
                        Data.BackToMark("Author");
                    }
                }


                // Verifier:
                location = "Verifier";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting verifier ID...");
                Data.NextOrCurrentElement(Const.OeVerifierContainter);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Verifier container does not exist in the model message: " + Const.OeVerifierContainter);
                }
                else
                {
                    if (VerifierId == null)
                        Data.RemoveCurrent();
                    else
                    {
                        Data.SetMark("Verifier");
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.OeVerifierSubContainter);
                        if (Data.Current == null)
                            throw new XmlException("Verifier container does not contain the sub-container: " + Const.OeVerifierSubContainter);
                        // Step into subcontainer:
                        Data.StepIn();
                        Data.NextOrCurrentElement();
                        if (Data.Current == null)
                            throw new XmlException("Verifier subcontainer does not contain any elements in the model message. Container element: "
                            + Const.OeVerifierSubContainter);
                        // Set verifier's Id:
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                            throw new XmlException("Verifier subcontainer does not contain an ID element in the model message.");
                        else
                            Data.SetAttribute(Const.IdIdAttribute, VerifierId);
                        Data.BackToMark("Verifier");
                    }
                }


                
                // BIS order Id:
                location = "BIS Order Id";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting BIS Order ID...");
                Data.NextOrCurrentElement(Const.OeBisOrderIdContainter);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("BIS order ID container does not exist in the model message: " + Const.OeBisOrderIdContainter);
                }
                else
                {
                    if (BisorderId == null)
                        Data.RemoveCurrent();
                    else
                    {
                        Data.SetMark("BisOrderId");
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.OeBisOrderIdSubContainter);
                        if (Data.Current == null)
                            throw new XmlException("BIS order ID container does not contain the sub-container: " + Const.OeBisOrderIdSubContainter);
                        // Step into subcontainer:
                        Data.StepIn();
                        // Set BIS Order Id:
                        Data.NextOrCurrentElement(Const.IdElement);
                        if (Data.Current == null)
                            throw new XmlException("BIS Order ID subcontainer does not contain an ID element in the model message.");
                        else
                            Data.SetAttribute(Const.IdIdAttribute, BisorderId);
                        Data.BackToMark("BisOrderId");
                    }
                }

            }
            catch (Exception ex)
            {
                R.ReportError("Location: " + location + " ", ex);
                throw ReporterBase.ReviseException(ex,
                        "MsgObservationEvent.ToXml(); " + location + ": ");
                throw ex;
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Finished.");
                --R.Depth;
            }
            return Doc;
        }  // ToXml()


    } // class MsgObservationEvent


}
