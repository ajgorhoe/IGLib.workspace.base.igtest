
using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace IG.Lib
{



    /// <summary>Contains data about individual service that is charged and stated in the DetailedFinancialTransaction msg.</summary>
    public class ServiceDetail
    {
        public string
            Id = null,  // Id of this specific cervice in the database
            IdOid = null, // OID of service Id
            Code = null, // code from the national book of standard services ("Zelena knjiga")
            CodeDescription = null, // description of the code above
            StatusCode = null; // 
        public Decimal Quantity=0.0M;
        public DateTime?
            StartTime = null,   // Here we can probably input the time of creation of msg
            CompletionTime = null,
            ActivityTime = null;  // time when the service was performed
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Service charge data: ");
            sb.AppendLine("  Service ID: " + Id);
            sb.AppendLine("  Service Code: " + Code);
            sb.AppendLine("  Quantity: " + Quantity.ToString());
            if (StartTime == null)
                sb.AppendLine("  StartTime is not specified.");
            else
                sb.AppendLine("  StartTime: " + StartTime.ToString());
            if (CompletionTime == null)
                sb.AppendLine("  CompletionTime is not specified.");
            else
                sb.AppendLine("  CompletionTime: " + CompletionTime.ToString());
            if (ActivityTime == null)
                sb.AppendLine("  ActivityTime is not specified.");
            else
                sb.AppendLine("  ActivityTime: " + ActivityTime.ToString());
            return sb.ToString();
        }
    }  // class ServiceDetail




    /// <summary>Class for holding and manipulating the data about financial transaction.
    /// Includes parsing an XML file, storing data internally, and transcription of read data to a PADO object
    /// that enables saving data to a database.</summary>
    public class MsgFinancialTransaction : MsgBaseWithModel
    // $A Igor Apr09 May09;
    {

        /// <summary>Default constructor, sets the type information.</summary>
        public MsgFinancialTransaction()
        {
            Type = MessageType.DetailedFinancialTransaction;
        }



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
                location = "Document";
                ++R.Depth;
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Started...");
                // Check whether the document is loaded:
                if (data == null)
                    throw new ArgumentException("The XML parser (and builder) with message document is not specified (null reference).");
                if (data.Doc == null)
                    throw new XmlException("The XML message document is not loaded on the XML parser.");
                // Get the Root element and check its name:
                location = "Root";
                data.GoToRoot();
                if (data.Name != Const.FtRootName)
                    throw new XmlException("Wrong name of the root element: " + data.Name + " instead of " + Const.FtRootName);

                // Skip coments and read the msg Id:
                location = "Message Id";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading message ID...");
                if (data.StepIn() == null)
                    throw new XmlException("Root element does not contain any child nodes.");
                data.NextOrCurrentElement();
                if (data.Name != Const.IdElement)
                    throw new XmlException("The first subelement of the root element does not conteain message Id. Element name: " + data.Name);
                MessageNumberOid = data.Attribute(Const.IdOidAttribute);
                MessageNumber = data.Attribute(Const.IdIdAttribute);
                // Read creation time:
                location = "Creation time";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading creation time...");
                data.NextElement(Const.CreationTimeElement);
                if (data.Current == null)
                    throw new XmlException("Message creation time is missing.");
                aux = data.Attribute(Const.CreationTimeAttribute);
                if (string.IsNullOrEmpty(aux))
                    throw new XmlException("Badly formed creation time.");
                CreationTime = Const.ConvertTime(aux);

                // Read code of the action that should be performed:
                location = "Action code";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading action code...");
                data.NextElement(Const.ActionElement);
                if (data.Current == null)
                    throw new XmlException("Action specification is missing.");
                aux = data.Attribute(Const.ActionAttribute);
                ActionCode = aux;
                //Action = Const.ConvertFtAction(aux);
                //if (Action == FtAction.Unknown)
                //    throw new XmlException("Badly formed action code.");
                ActionRoot = data.Attribute(Const.ActionOidAttribute);
                // read sender, receiver and responder of the msg:
                location = "Message sender, receiver and responder";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading data about sender, receiver and responder of the message...");
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
                        if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading receiver data...");
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
                        if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading responder data...");
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
                        if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading sender data...");
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


                // Read the msg:
                location = "Message outer container";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading message contents...");
                data.NextOrCurrentElement(Const.FtMessageContainer);
                if (data.Current == null)
                    throw new XmlException("There is no message outer container - element " + Const.FtMessageContainer);
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("Message outer container does not contain any child nodes - element " + Const.FtMessageContainer);
                // Read the msg code:
                location = "Messge code";
                data.NextOrCurrentElement(Const.CodeElement);
                if (data.Current == null)
                    throw new XmlException("Message code is not specified - element " + Const.CodeElement);
                MessageCode = data.Attribute(Const.CodeAttribute);
                MessageCodeSystem = data.Attribute(Const.CodeSystemAttribute);
                //// This code must correspond to the action:
                //FtAction control = Const.ConvertFtAction(MessageCode);
                //if (control != Action)
                //    throw new XmlException("Message code (\"" + MessageCode + "\") does not agree with the required action ("
                //        + Action.ToString() + ").");
                // Go deeper into subcontainers where significan contents is dug in:
                location = "Message subcontainer";
                data.NextElement(Const.MessageSubContainer);
                if (data.Current == null)
                    throw new XmlException("There is no message subcontainer - element " + Const.MessageSubContainer);
                //aux = data.Attribute(Const.MessageSubContainerTypeAttribute);
                //if (aux != Const.MessageSubContainerTypeAttributeValue)
                //    throw new XmlException("Wrong " + Const.MessageSubContainerTypeAttribute + " attribute, element " + Const.MessageSubContainer
                //        + " (" + aux + " instead of " + Const.MessageSubContainerTypeAttributeValue + ").");
                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("There is no message contents - inside element " + Const.MessageSubContainer);
                // Step into the msg sub-subcontainer:
                location = "Message sub-subcontainer";
                data.NextOrCurrentElement(Const.FtMessageSubContainer2);
                //if ((aux = data.Attribute(Const.MessageTypeAttribute)) != Const.FtMessageType)
                //    throw new XmlException("Wrong msg type: " + aux + ", shoud be: " + Const.FtMessageType + ".");
                data.StepIn();
                data.NextOrCurrentElement();
                if (data.Current == null)
                    throw new XmlException("There is no message contents - inside element " + Const.FtMessageSubContainer2);

                // Read Bis Order ID:
                location = "Order ID";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading order ID...");
                data.NextOrCurrentElement(Const.IdElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("BIS Order Id is not specified.");
                } else
                {
                    BisOrderId = data.Attribute(Const.IdIdAttribute);
                    BisOrderIdOid = data.Attribute(Const.IdOidAttribute);
                    if (string.IsNullOrEmpty(BisOrderId))
                        throw new XmlException("Malformed order Id.");
                }
                // Read Order status:
                location = "Order status";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading order status code...");
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
                }
                else
                    throw new XmlException("Order status is not defined - malformed attribute: " + Const.OoOrderStatusAttribute);
                // Read times (these times are not used at the moment):
                location = "Message times";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading relevant times...");
                data.NextOrCurrentElement(Const.FtEffectiveTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Effective time is not specified, no element named " + Const.FtEffectiveTimeElement);
                } else
                {
                    aux = data.Attribute(Const.FtEffectiveTimeAttribute);
                    if (string.IsNullOrEmpty(aux))
                        throw new XmlException("Effective time is not specified.");
                    else
                        EffectiveTime = Const.ConvertTime(aux);
                }
                data.NextOrCurrentElement(Const.FtActivityTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Activity time is not specified, no element named " + Const.FtActivityTimeElement);
                } else
                {
                    aux = data.Attribute(Const.FtActivityTimeAttribute);
                    if (string.IsNullOrEmpty(aux))
                        throw new XmlException("Activity time is not specified.");
                    else
                        ActivityTime = Const.ConvertTime(aux);
                }

                // Read author data:
                location = "Author data";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading author data...");
                data.NextOrCurrentElement(Const.FtAuthorContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Author container is not specified: " + Const.FtAuthorContainer);
                }
                else
                {
                    data.SetMark("Author");
                    data.StepIn();
                    data.NextOrCurrentElement(Const.FtAuthorSubcontainer);
                    if (data.Current==null)
                    {
                        data.Back();
                        throw new XmlException("Authr sub-container is not specified: " + Const.FtAuthorSubcontainer);
                    } else
                    {
                        data.StepIn();
                        data.NextOrCurrentElement(Const.FtAuthorSubcontainer2);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Authr sub-sub-container is not specified: " + Const.FtAuthorSubcontainer2);
                        } else
                        {
                            data.StepIn();
                            data.NextOrCurrentElement(Const.FtAuthorSubcontainer3);
                            if (data.Current == null)
                            {
                                data.Back();
                                throw new XmlException("Authr sub-sub-sub-container is not specified: " + Const.FtAuthorSubcontainer3);
                            } else
                            {
                                data.StepIn();
                                data.NextOrCurrentElement(Const.IdElement);
                                if (data.Current == null)
                                {
                                    throw new XmlException("Malformed author data: there is no Id element.");
                                } else
                                {
                                    AuthorId = data.Attribute(Const.IdIdAttribute);
                                    AuthorIdOid = data.Attribute(Const.IdOidAttribute);
                                    if (string.IsNullOrEmpty(AuthorId))
                                        throw new XmlException("Author's ID is not specified.");
                                }
                            }
                        }
                    }
                    data.BackToMark("Author");
                }


                // Read charged services data:
                location = "Charged services data";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading charged services data...");
                data.NextOrCurrentElement(Const.FtServiceContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("There are no charged services containers; element name: " + Const.FtServiceContainer);
                } 
                else
                {
                    // Read all service blocks:
                    XmlElement ServiceElement = data.CurrentElement;
                    int NumServices = 0;
                    do
                    {
                        data.SetMark("Service");
                        ++ NumServices;
                        // Read next service block:
                        location = "Charged services No. " + NumServices.ToString();
                        if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading charged service No. "
                            + NumServices.ToString() + "... ");
                        data.StepIn();
                        data.NextOrCurrentElement(Const.FtServiceSubcontainer);
                        if (data.Current==null)
                        {
                            data.Back();
                            throw new XmlException("Service container No. " + NumServices.ToString() + " does not contain the cub-container element: "
                                + Const.FtServiceSubcontainer);
                        } else
                        {
                            data.StepIn();
                            data.NextOrCurrentElement();
                            if (data.Current==null)
                            {
                                data.Back();
                                throw new XmlException("Charged service sub-conteiner does not contain any elements; sub-container name: "
                                    + Const.FtServiceSubcontainer);
                            } else
                            {
                                ServiceDetail service = new ServiceDetail();
                                Services.Add(service);
                                // Read service Id:
                                data.NextOrCurrentElement(Const.IdElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    throw new XmlException("Service Id is not specified.");
                                } else
                                {
                                    service.Id = data.Attribute(Const.IdIdAttribute);
                                    service.IdOid = data.Attribute(Const.IdOidAttribute);
                                }
                                // Read service code:
                                data.NextOrCurrentElement(Const.FtServiceCodeElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    throw new XmlException("Service code is not specified. Element: " + Const.FtServiceCodeAttribute);
                                } else
                                {
                                    service.Code = data.Attribute(Const.FtServiceCodeAttribute);
                                }
                                // Read service code desctiption:
                                data.NextOrCurrentElement(Const.FtServiceCodeDescriptionElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    throw new XmlException("Service code description is not specified. Element: " + Const.FtServiceCodeDescriptionElement);
                                } else
                                {
                                    service.CodeDescription = data.Value;
                                }
                                // Read service status code:
                                data.NextOrCurrentElement(Const.FtServiceStatusCodeElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    throw new XmlException("Service status code is not specified. Element: " + Const.FtServiceStatusCodeElement);
                                } else
                                {
                                    service.StatusCode = data.Attribute(Const.FtServiceStatusCodeAttribute);
                                }
                                // Read relevant times related to the service:
                                data.NextOrCurrentElement(Const.FtServiceTimeContainer);
                                if (data.Current == null)
                                {
                                    data.Back();
                                } else
                                {
                                    data.SetMark("Time");
                                    data.StepIn();
                                    data.NextOrCurrentElement();
                                    if (data.Current == null)
                                    {
                                        throw new XmlException("Times container contains no elements. Container name: "
                                            + Const.FtServiceTimeContainer);
                                    } else
                                    {
                                        string strtime = null;
                                        // Start time:
                                        data.NextOrCurrentElement(Const.FtServiceStartTimeElement);
                                        if (data.Current == null)
                                        {
                                            data.Back();
                                        } else
                                        {
                                            strtime = data.Attribute(Const.FtServiceTimeAttribute);
                                            if (string.IsNullOrEmpty(strtime))
                                                throw new XmlException("Malformed start time, element: " + Const.FtServiceStartTimeElement);
                                            else
                                                service.StartTime = Const.ConvertTime(strtime);
                                        }
                                        // Completion time:
                                        data.NextOrCurrentElement(Const.FtServiceCompletionTimeElement);
                                        if (data.Current == null)
                                        {
                                            data.Back();
                                        } else
                                        {
                                            strtime = data.Attribute(Const.FtServiceTimeAttribute);
                                            if (string.IsNullOrEmpty(strtime))
                                                throw new XmlException("Malformed completion time, element: " + Const.FtServiceStartTimeElement);
                                            else
                                                service.CompletionTime = Const.ConvertTime(strtime);
                                        }
                                    }
                                    data.BackToMark("Time");
                                }
                                // Read activity time:
                                data.NextOrCurrentElement(Const.FtServiceActivityTimeElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    // throw new XmlException("Service activity time is not specified. Element: " + Const.FtServiceActivityTimeElement);
                                } else 
                                {
                                    service.ActivityTime = Const.ConvertTime(data.Attribute(Const.FtServiceActivityTimeAttribute));
                                }
                                // Read service quantity:
                                data.NextOrCurrentElement(Const.FtServiceQuantityElement);
                                if (data.Current == null)
                                {
                                    data.Back();
                                    throw new XmlException("Service quantity is not specified. Element: " + Const.FtServiceQuantityElement);
                                } else 
                                {
                                    service.Quantity = Const.ConvertQuantity(data.Attribute(Const.FtServiceQuantityAttribute));
                                }
                            }
                        }
                        data.BackToMark("Service");
                        // Go to next service container, if defined:
                        data.NextElement(Const.FtServiceContainer);
                        if ((ServiceElement = data.CurrentElement) == null)
                            data.Back();
                    } while (ServiceElement !=null);
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
                R.ReportError("Location: " + location + " ", ex);
                throw ReporterBase.ReviseException(ex,
                        "MsgFinancialTransaction.Read(XmlDocument); " + location + ": ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Finished.");
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

                    rep.AddInfo("Data consistency check is not implemented for the class MsgFinancialTransaction.");

                }
                catch (Exception ex)
                {
                    rep.AddError("Exception has been thrown during data consistency check. Details: " + Environment.NewLine + ex.Message);
                }
            }
        }  // CheckConsistency()



        // NEW VARIABLES 


        string

            ActionRoot = null,
            ActionCode = Const.FtAction,

            MessageCode = null,  // msg code, must correspond to Action
            MessageCodeSystem = null,

            BisOrderId = null,
            BisOrderIdOid = null,

            AuthorId = null, 
            AuthorIdOid = null;


        OoStatus OrderStatus = OoStatus.Unknown;  // status naročila

        DateTime
            CreationTime = DateTime.Now;
        
        DateTime
            EffectiveTime = DateTime.Now, 
            ActivityTime = DateTime.Now;

        List<ServiceDetail> Services = new List<ServiceDetail>();

        int? Id = null;  // ID of the corresponding database rsr (will be null if hte data is not read from the database)

        public override string ToString()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToString()", "Started...");
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

                sb.AppendLine("Action code: " + ActionCode + "; Root: " + ActionRoot);

                sb.AppendLine("Message receiver: " + MessageReceiver + ", OID: " + MessageReceiverOid);
                sb.AppendLine("Message responder: " + MessageResponder + ", OID: " + MessageResponderOid);
                sb.AppendLine("Message sender: " + MessageSender + ", OID: " + MessageSenderOid);
                sb.AppendLine();


                sb.AppendLine("Message contents:");
                sb.AppendLine("Message code: " + MessageCode);
                //if (Const.ConvertFtAction(MessageCode) == Action)
                //    sb.AppendLine("Agreement with action: Yes");
                //else
                //    sb.AppendLine("Agreement with action: No.");

                sb.AppendLine("Sender's order ID: " + BisOrderId + ", OID: " + BisOrderIdOid);
                sb.AppendLine("Order status: " + OrderStatus);

                sb.AppendLine("EffectiveTime: " + EffectiveTime.ToString());
                sb.AppendLine("ActivityTime: " + ActivityTime.ToString());
                // Append authors data:
                sb.AppendLine("Author ID: " + AuthorId + "; OID: " + AuthorIdOid);
                // Append service dta:
                if (Services==null)
                    sb.AppendLine("Services are not specified (list is null).");
                else if (Services.Count<1)
                    sb.AppendLine("There are no services.");
                else
                {
                    for (int i=0;i<Services.Count;++i)
                    {
                        sb.AppendLine("Service No. " + (i+1).ToString() + ":");
                        sb.AppendLine(Services[i].ToString());
                    }
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
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToString()", "Finished.");
                --R.Depth;
            }
        }


        static int IdWithinSession = 0;

        /// <summary>Returns a generated msg Id.</summary>
        /// <remarks>Message numbers should later be generated in a different way, probably by the database.</remarks>
        string GenerateMessageId()
        {
            ++IdWithinSession;
            string ret = "Ft_" + Const.ConvertTime(DateTime.Now, true /* includetime */, false /* underscores */);
            //if (PatientId == null)
            //    ret += "__" + PatientId.ToString();
            //else
            //    ret += "__xx";
            ret += "_" + IdWithinSession.ToString();
            return ret;
        }

        // TODO: insert here the full path of the model file!
        protected string _modelFile = "ModelFileFinancialTransaction";

        /// <summary>Gets or sets the XML document that is used as model for creation of XML.
        /// Warning: Each call to execution of get() makes a clone of an XmlDocument.</summary>
        public override XmlDocument ModelDocument
        {
            get
            {
                XmlDocument ret = base.ModelDocument;
                if (ret == null)
                {
                    // Model document is not set, try to get it ftom the file specified in WS settings:
                    //try
                    //{
                        // SetModelFile(Calypso.Global.Settings.ModelFileFinancialTransaction);
                        SetModelFile(_modelFile);
                    //}
                    //catch { }
                    ret = base.ModelDocument;
                }
                return ret;
            }
            set { base.ModelDocument = value; }
        }



        /// <summary>Converts a msg to Xml and returns it.</summary>
        public override XmlDocument ToXml()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Started...");
            string location = "Document";
            try
            {
                Data.Doc = new XmlDocument();

                // Generate Message number, which will at the same time serve as msg ID:
                if (string.IsNullOrEmpty(MessageNumber))
                    MessageNumber = GenerateMessageId();

                Data.Doc = ModelDocument;
                if (Data.Doc == null)
                    throw new Exception("Model document for creation of a message is not specified.");
                // Get the Root element and check its name:
                location = "Root";
                Data.GoToRoot();
                if (Data.Name != Const.FtRootName)
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
                Data.SetAttribute(Const.ExtensionAttribute, MessageNumber);
                // Set the Message Id: 
                location = "Message Id";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Setting message Id...");
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
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Setting message creation time...");
                Data.NextOrCurrentElement(Const.CreationTimeElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Creation time element does not exist in the model message: " + Const.CreationTimeElement);
                }
                else
                {
                    Data.SetAttribute(Const.CreationTimeAttribute, Const.ConvertTime(DateTime.Now, true, false));
                }
                // Set Message Action: 
                location = "Action:";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Setting action...");
                Data.NextOrCurrentElement(Const.ActionElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Action element does not exist in the model message: " + Const.ActionElement);
                } else
                {
                    if (!string.IsNullOrEmpty(ActionCode))
                        Data.SetAttribute(Const.ActionAttribute, ActionCode);
                    if (!string.IsNullOrEmpty(ActionRoot))
                        Data.SetAttribute(Const.ActionOidAttribute, ActionRoot);
                }
                // Set the Message Receiver: 
                location = "Message receiver:";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Setting message receiver ...");
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
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Setting message responder ...");
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
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Setting message sender ...");
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

                // Message contents: 
                location = "Message contents:";
                if (R.TreatInfo) R.ReportInfo("MsgObservationEvent.ToXml()", "Setting contents...");
                Data.NextOrCurrentElement(Const.FtMessageContainer);
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
                    Data.SetAttribute(Const.CodeAttribute, MessageCode);
                }
                Data.NextOrCurrentElement(Const.MessageSubContainer);
                Data.StepIn();
                Data.NextOrCurrentElement(Const.FtMessageSubContainer2);
                Data.StepIn();
                Data.NextOrCurrentElement();
                if (Data.Current == null)
                    throw new Exception("Message sub-subcontainer does not exist in the model message or it contains no elemens: "
                        + Const.FtMessageSubContainer2);
                Data.NextOrCurrentElement();

                // Set Bis Order Id:
                location = "Bis Order ID";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Setting BIS order ID...");
                Data.NextOrCurrentElement(Const.IdElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Order Id is not specified in the model message.");
                } else
                {
                    Data.SetAttribute(Const.IdIdAttribute,BisOrderId);
                    Data.SetAttribute(Const.IdOidAttribute,BisOrderIdOid);
                    if (string.IsNullOrEmpty(BisOrderId))
                        throw new XmlException("Malformed order Id.");
                }
                // Set Order status:
                location = "Order status";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Setting order status code...");
                Data.NextElement(Const.OoOrderStatusElement);
                if (Data.Current == null)
                    throw new XmlException("Order status is not specified in the model message - element " + Const.OoOrderStatusElement);
                else
                    Data.SetAttribute(Const.OoOrderStatusAttribute,Const.ConvertOoStatus(OrderStatus));

                // Set times (these times are not used at the moment):
                location = "Message times";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Reading relevant times...");
                Data.NextOrCurrentElement(Const.FtEffectiveTimeElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Effective time is not specified in the model message, no element named " + Const.FtEffectiveTimeElement);
                }
                else
                {
                    // Set effective time simply to current time:
                    EffectiveTime = DateTime.Now;
                    Data.SetAttribute(Const.FtEffectiveTimeAttribute,
                        Const.ConvertTime(EffectiveTime,true, false /* includeunderscores */) );
                }
                Data.NextOrCurrentElement(Const.FtActivityTimeElement);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Activity time is not specified in the model message, no element named " + Const.FtActivityTimeElement);
                }
                else
                {
                    // Set effective time simply to current time:
                    EffectiveTime = DateTime.Now;
                    Data.SetAttribute(Const.FtActivityTimeAttribute,
                        Const.ConvertTime(ActivityTime,true, false /* includeunderscores */) );
                }

                // Set author data:
                location = "Author data";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Setting author data...");
                Data.NextOrCurrentElement(Const.FtAuthorContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("Author container is not specified in the model message: " + Const.FtAuthorContainer);
                }
                else
                {
                    Data.SetMark("Author");
                    Data.StepIn();
                    Data.NextOrCurrentElement(Const.FtAuthorSubcontainer);
                    if (Data.Current == null)
                    {
                        Data.Back();
                        throw new XmlException("Authr sub-container is not specified in the model message: " + Const.FtAuthorSubcontainer);
                    }
                    else
                    {
                        Data.StepIn();
                        Data.NextOrCurrentElement(Const.FtAuthorSubcontainer2);
                        if (Data.Current == null)
                        {
                            Data.Back();
                            throw new XmlException("Authr sub-sub-container is not specified in the model message: " + Const.FtAuthorSubcontainer2);
                        }
                        else
                        {
                            Data.StepIn();
                            Data.NextOrCurrentElement(Const.FtAuthorSubcontainer3);
                            if (Data.Current == null)
                            {
                                Data.Back();
                                throw new XmlException("Authr sub-sub-sub-container is not specified in the model message: " + Const.FtAuthorSubcontainer3);
                            }
                            else
                            {
                                Data.StepIn();
                                Data.NextOrCurrentElement(Const.IdElement);
                                if (Data.Current == null)
                                {
                                    throw new XmlException("Malformed author data in the model message: there is no Id element.");
                                } else
                                {
                                    Data.SetAttribute(Const.IdIdAttribute,AuthorId);
                                    Data.SetAttribute(Const.IdOidAttribute,AuthorIdOid);
                                }
                            }

                        }
                    }
                    Data.BackToMark("Author");
                }

                // Set charged services data:
                location = "Charged services data";
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.Read(doc)", "Setting charged services data...");
                Data.NextOrCurrentElement(Const.FtServiceContainer);
                if (Data.Current == null)
                {
                    Data.Back();
                    throw new XmlException("There is no charged services container in the model message; element name: " + Const.FtServiceContainer);
                } else
                {
                    // Insert service blocks:
                    // Mark the model services, copies of this element will be inserted before it:
                    Data.SetMark("ModelService");
                    if (Services==null)
                    {
                        Data.RemoveCurrent();
                        throw new InvalidDataException("There are no charged services on the object (list has null reference).");
                    } else if (Services.Count<1)
                    {
                        Data.RemoveCurrent();
                        throw new InvalidDataException("There are no charged services on the object.");
                    } else
                    {
                        for (int i=0;i<Services.Count;++i)
                        {
                            // Add data for service No. i:
                            ServiceDetail service = Services[i];
                            if (service==null)
                                throw new InvalidDataException("Service data No. " + (i+1).ToString() + " is not specified.");

                            // Copy model element:
                            Data.GoToMark("ModelService");
                            Data.CreateClone();
                            Data.InsertBefore();
                            Data.MoveToNewest();
                            // Now we write contents of the model node:

                            Data.StepIn();
                            Data.NextOrCurrentElement(Const.FtServiceSubcontainer);
                            if (Data.Current==null)
                            {
                                Data.Back();
                                throw new XmlException("Service container of the model message does not contain the cub-container element: "
                                    + Const.FtServiceSubcontainer);
                            } else
                            {
                                Data.StepIn();
                                Data.NextOrCurrentElement();
                                if (Data.Current==null)
                                {
                                    Data.Back();
                                    throw new XmlException("Charged service sub-conteiner does not contain any elements in the model message; sub-container name: "
                                        + Const.FtServiceSubcontainer);
                                } else
                                {
                                    // Set service Id:
                                    Data.NextOrCurrentElement(Const.IdElement);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service Id element is missing in the model message.");
                                    } else
                                    {
                                        Data.SetAttribute(Const.IdOidAttribute, service.IdOid);
                                        Data.SetAttribute(Const.IdIdAttribute, service.Id);
                                    }
                                    // Set service code:
                                    Data.NextOrCurrentElement(Const.FtServiceCodeElement);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service code element is not specified in the model message. Element: " + Const.FtServiceCodeAttribute);
                                    } else
                                    {
                                        Data.SetAttribute(Const.FtServiceCodeAttribute,service.Code);
                                    }
                                    // Read service code desctiption:
                                    Data.NextOrCurrentElement(Const.FtServiceCodeDescriptionElement);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service code description is not specified in the model message. Element: " + Const.FtServiceCodeDescriptionElement);
                                    } else
                                    {
                                        if (service.CodeDescription==null)
                                            Data.RemoveCurrent();
                                        else
                                            Data.SetValue(service.CodeDescription);
                                    }
                                    // Read service status code:
                                    Data.NextOrCurrentElement(Const.FtServiceStatusCodeElement);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service status code element is not specified in the model message. Element: " + Const.FtServiceStatusCodeElement);
                                    } else
                                    {
                                        Data.SetAttribute(Const.FtServiceStatusCodeAttribute, service.StatusCode);
                                    }
                                    // Read relevant times related to the service:
                                    Data.NextOrCurrentElement(Const.FtServiceTimeContainer);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service times container is not specified in the model message. Element: " + Const.FtServiceTimeContainer);
                                    } else
                                    {
                                        Data.SetMark("Time");
                                        Data.StepIn();
                                        Data.NextOrCurrentElement();
                                        if (Data.Current == null)
                                        {
                                            throw new XmlException("Times container contains no elements in the model message. Container name: "
                                                + Const.FtServiceTimeContainer);
                                        } else
                                        {
                                            // string strtime = null;
                                            // Start time:
                                            Data.NextOrCurrentElement(Const.FtServiceStartTimeElement);
                                            if (Data.Current == null)
                                            {
                                                Data.Back();
                                                throw new XmlException("Service start time element is not defined in the model message.");
                                            } else
                                            {
                                                if (service.StartTime==null)
                                                    Data.RemoveCurrent();
                                                else
                                                {
                                                    Data.SetAttribute(Const.FtServiceTimeAttribute, 
                                                        Const.ConvertTime(service.StartTime.Value,
                                                        true /* includetime */, false /* underscores */));
                                                }
                                            }
                                            // Completion time:
                                            Data.NextOrCurrentElement(Const.FtServiceCompletionTimeElement);
                                            if (Data.Current == null)
                                            {
                                                Data.Back();
                                                throw new XmlException("Service completion time element is not defined in the model message.");
                                            } else
                                            {
                                                Data.SetAttribute(Const.FtServiceTimeAttribute, 
                                                    Const.ConvertTime(service.CompletionTime.Value,
                                                    true /* includetime */, false /* underscores */));
                                            }
                                        }
                                        Data.BackToMark("Time");
                                    }
                                    // Read activity time:
                                    Data.NextOrCurrentElement(Const.FtServiceActivityTimeElement);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service activity time is not specified in the model message. Element: " + Const.FtServiceActivityTimeElement);
                                    } else 
                                    {
                                        if (service.ActivityTime==null)
                                            Data.RemoveCurrent();
                                        else
                                            Data.SetAttribute(Const.FtServiceActivityTimeAttribute, 
                                                Const.ConvertTime(service.ActivityTime.Value,true /* includetime */, false /* underscores */));
                                    }
                                    // Read service quantity:
                                    Data.NextOrCurrentElement(Const.FtServiceQuantityElement);
                                    if (Data.Current == null)
                                    {
                                        Data.Back();
                                        throw new XmlException("Service quantity is not specified in the model message. Element: " + Const.FtServiceQuantityElement);
                                    } else 
                                    {
                                        Data.SetAttribute(Const.FtServiceQuantityAttribute, Const.ConvertQuantity(service.Quantity));
                                    }
                                }
                            }

                        }

                        Data.GoToMark("ModelService");
                        Data.RemoveCurrent();
                    }
                }

            }
            catch (Exception ex)
            {
                R.ReportError("Location: " + location + " ", ex);
                throw ReporterBase.ReviseException(ex,
                        "MsgFinancialTransaction.ToXml(); " + location + ": ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("MsgFinancialTransaction.ToXml()", "Finished.");
                --R.Depth;
            }
            return Doc;
        }  // ToXml()

    } // class MsgFinancialTransaction

}
