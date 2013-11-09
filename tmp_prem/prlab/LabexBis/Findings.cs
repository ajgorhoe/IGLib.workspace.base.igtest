using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
// using System.Configuration;
using System.Windows.Forms;

namespace IG.Lib
{

 


    /// <summary>Patient data for inclusion in findings.</summary>
    public class FindingsPatient
    {

        public string
            Name = null,
            FamilyName = null,
            MiddleNames = null,
            Address = null,
            GenderStr = null,
            DateOfBirthStr = null,
            AgeStr = null,
            AgeMonthsStr = null,
            PersonId = null;

        public DateTime? DateOfBirth = null;

        public bool?
            IsMale = null;

        public int Age
        {
            get 
            {
                if (DateOfBirth == null)
                    return 0;
                return (int) Math.Floor((DateTime.Now-DateOfBirth.Value).TotalDays/365.25);
            }
        }

        public int AgeMonths
        {
            get 
            {
                if (DateOfBirth == null)
                    return 0;
                return (int) Math.Floor((DateTime.Now-DateOfBirth.Value).TotalDays/365.25);
                // Array age = LabexUtilities.LabexFunctions.ReturnAge(DateOfBirth.Value);
                //if (age!=null)
                //    return (int)age.GetValue(1);
                //else
                //    return 0;

                //Dim age As Array = LabexUtilities.LabexFunctions.ReturnAge(rsr.PatientBirthDate)
                //_msgFindings.PatientAge = CInt(age.GetValue )
                //_msgFindings.PatientAgeMonths = CInt(age.GetValue(1))
            }
        }

        /// <summary>Gets the string corresponding describing patient's gender for output in XML prepared for viewing and printing.</summary>
        public string GenderOutputString
        {
            get
            {
                if (IsMale == null)
                    return "XX";
                else if (IsMale == true)
                    return "M";
                else
                    return "F";
            }
        }

        /// <summary>Gets the string corresponding describing patient's gender for output in XML prepared for viewing and printing.</summary>
        public string GenderOutputStringInputFormat
        {
            get
            {
                if (IsMale == null)
                    return "XX";
                else if (IsMale == true)
                    return MsgConst.Const.GenderMale.ToString();
                else
                    return MsgConst.Const.GenderFemale.ToString();
            }
        }

        /// <summary>Gets output string representing patient's date of birth.</summary>
        public string DateOfBirthOutputString
        {
            get
            {
                if (DateOfBirth == null)
                    return "XX";
                else return DateOfBirth.Value.ToShortDateString();
            }
        }

        /// <summary>Gets output string representing patient's date of birth, but formed in input format such that
        /// it can be parsed.</summary>
        public string DateOfBirthOutputStringInputFormat
        {
            get
            {
                if (DateOfBirth == null)
                    return "XX";
                else return MsgConst.Const.ConvertTime(DateOfBirth.Value, true /* includetime */, false /* underscores */);
            }
        }

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Patient:");
            sb.AppendLine("  Name: " + Name);
            sb.AppendLine("  FamilyName: " + FamilyName);
            sb.AppendLine("  MiddleNames: " + MiddleNames);
            sb.AppendLine("  Address: " + Address);
            sb.AppendLine("  GenderStr: " + GenderStr);
            if (IsMale == null)
                sb.AppendLine("  Patient's gender is not specified.");
            else
                sb.AppendLine("  IsMale: " + IsMale.Value);
            sb.AppendLine("  DateOfBirthStr: " + DateOfBirth);
            sb.AppendLine("  Age: " + Age.ToString());
            sb.AppendLine("  AgeMonths: " + AgeMonths.ToString());
            if (AgeStr != null)
                sb.AppendLine("  AgeStr: " + AgeStr);
            if (AgeMonthsStr != null)
                sb.AppendLine("  AgeMonthsStr: " + AgeMonthsStr);
            sb.AppendLine("  PersonId: " + PersonId);
            if (DateOfBirth == null)
                sb.AppendLine("  Date of birth is not specified.");
            else 
                sb.AppendLine("  Date of birth: " +  DateOfBirth.ToString());
            return sb.ToString();
        }

    }  // class FindingsPatient



    /// <summary>Patient data for inclusion in findings.</summary>
    public class FindingsPhysician
    {

        public string
            Name = null,
            FamilyName = null,
            MiddleNames = null,
            Appellation = null,
            Title = null,
            TitleRight = null;

        /// <summary>Clears all data</summary>
        public void Clear()
        {
            Name = null;
            FamilyName = null;
            MiddleNames = null;
            Appellation = null;
            Title = null;
            TitleRight = null;
        }

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Patient:");
            sb.AppendLine("  Name: " + Name);
            sb.AppendLine("  FamilyName: " + FamilyName);
            sb.AppendLine("  MiddleNames: " + MiddleNames);
            sb.AppendLine("  Appellation: " + Appellation);
            sb.AppendLine("  Title: " + Title);
            sb.AppendLine("  TitleRight: " + TitleRight);
            return sb.ToString();
        }

        public string PrintString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(Title))
            {
                sb.Append(Title);
                sb.Append(" ");
            }
            if (!string.IsNullOrEmpty(Name))
            { 
                sb.Append(Name);
                sb.Append(" ");
            }
            if (!string.IsNullOrEmpty(MiddleNames))
            { 
                sb.Append(MiddleNames);
                sb.Append(" ");
            }
            if (!string.IsNullOrEmpty(FamilyName))
            { 
                sb.Append(FamilyName);
                sb.Append(" ");
            }
            if (!string.IsNullOrEmpty(TitleRight))
            {
                sb.Append(TitleRight);
                sb.Append(" ");
            }
            return sb.ToString();
        }


    }  // class FindingsPhysician


    /// <summary>Data about related observations (findings) - findings can contain a group of these.</summary>
    public class RalatedObservation
    {
        public string
            ObservationReference = null,
            Remark = null;

        /// <summary>Returns a string representation of the current object.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Finding:");
            sb.AppendLine("  FindingReference: " + ObservationReference);
            sb.AppendLine("  Remark: " + Remark);
            return sb.ToString();
        }
    }



    /// <summary>Class for holding and manipulating the data about observation order.
    /// Includes parsing an XML file, storing data internally, and transcription of read data to a PADO object
    /// that enables saving data to a database.</summary>
    public class DocFindings : MsgBase
    // $A Igor Apr09 May09;
    {

        protected new FindingsConst Const
        {
            get { return FindingsConst.Const; }
        }


        /// <summary>Default constructor, sets the type information.</summary>
        public DocFindings()
        {

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
            if (R.TreatInfo) R.ReportInfo("Finding.Read()", "Started...");
            string location = "Document";
            try
            {
                Id = null;  // since findings are read from a file, database rsr ID is not defined
                MsgEventId = null;
                ++R.Depth;
                location = "Document";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Started...");
                // Check whether the document is loaded:
                if (data == null)
                    throw new ArgumentException("The XML parser (and builder) with message document is not specified (null reference).");
                if (data.Doc == null)
                    throw new XmlException("The XML message document is not loaded on the XML parser.");
                // Get the Root element and check its name:
                location = "Root";
                data.GoToRoot();
                if (data.Name != Const.RootName)
                    throw new XmlException("Wrong name of the root element: " + data.Name + " instead of " + Const.RootName);
                // Skip coments and jump into Head element:
                location = "Head";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading findings Head...");
                if (data.StepIn() == null)
                    throw new XmlException("Root element does not contain any child nodes.");
                data.NextOrCurrentElement();
                if (data.Current==null)
                    throw new XmlException("Root element does not contain any child elements.");
                if (data.Name != Const.HeadContainer)
                    throw new XmlException("The first subelement of the root element is not a document Head. Element name: " + data.Name);
                data.SetMark("Head"); // position of the head element

                data.StepIn();
                if (data.Current == null)
                    throw new XmlException("Data element is not null.");
                else if (data.Current == null)
                    throw new XmlException("Head element does not contain any child nodes.");
                // Read Header:
                data.NextOrCurrentElement(Const.HeaderElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Header is not specified.");
                } else
                    Header = data.Value;
                // Read Footer:
                data.NextOrCurrentElement(Const.Footerelement);
                if (data.Current == null)
                {
                    data.Back();
                } else
                {
                    Footer = data.Value;
                }
                // Read Heading:
                data.NextOrCurrentElement(Const.HeadingElement);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Heading is not specified. Element name: " + Const.HeadingElement);
                }
                else
                {
                    Heading = data.Value;
                }
                // Read Order code:
                data.NextOrCurrentElement(Const.OrderCodeElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Order code is not specified.");
                }
                else
                {
                    OrderCode = data.Value;
                }
                // Read Patient data:
                location = "Patient";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading Patient data...");
                data.NextOrCurrentElement(Const.PatientContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Patient data is not specified, missing container element: " + Const.PatientContainer);
                } else
                {
                    data.SetMark("Patient");
                    data.StepIn();
                    if (data.Current != null)
                        data.NextOrCurrentElement();
                    if (data.Current == null)
                    {
                        throw new XmlException("Patient container does not contain any elements.");
                    }
                    else
                    {
                        Patient = new FindingsPatient();
                        // Read patient's name:
                        data.NextOrCurrentElement(Const.PatientNameElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Patient's name is not specified.");
                        } else
                        {
                            Patient.Name = data.Value;
                        }
                        // Read patient's family name:
                        data.NextOrCurrentElement(Const.PatientFamilyNameElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Patient's family name is not specified.");
                        } else
                        {
                            Patient.FamilyName = data.Value;
                        }
                        // Read patient's middle name(s):
                        data.NextOrCurrentElement(Const.PatientMiddleNamesElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            // throw new XmlException("Patient's middle names are not specified.");
                        } else
                        {
                            Patient.MiddleNames = data.Value;
                        }
                        // Read patient's address:
                        data.NextOrCurrentElement(Const.PatientAddressElement);
                        if (data.Current==null)
                        {
                            data.Back();
                        } else
                        {
                            Patient.Address = data.Value;
                        }
                        // Read patient's gender:
                        data.NextOrCurrentElement(Const.PatientSexElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Patient's gender is not specified.");
                        } else
                        {
                            Patient.GenderStr = data.Value;
                            try
                            {
                                int gender = int.Parse(Patient.GenderStr);
                                if (gender == MsgConst.Const.GenderMale)
                                {
                                    Patient.IsMale = true;
                                } else if (gender == MsgConst.Const.GenderFemale)
                                {
                                    Patient.IsMale = false;
                                } 
                                else throw new XmlException("Invalid gender code: " + gender
                                   + " (should be " + MsgConst.Const.GenderMale + " for male or "
                                   + MsgConst.Const.GenderFemale + " for female).");
                            }
                            catch /* (Exception ex) */
                            {
                                //throw new XmlException("Malformed patient's gender information. Element name: " 
                                //    +  Const.PatientSexElement + Environment.NewLine + "Details: " + ex.Message);
                            }
                        }
                        // Read patient's date of birth:
                        data.NextOrCurrentElement(Const.PatientDateOfBirthElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            // throw new XmlException("Patient's data of birth is not specified.");
                        } else
                        {
                            // string Patient.DateofBirth = data.Value;
                            Patient.DateOfBirthStr = data.Value;
                            if (string.IsNullOrEmpty(Patient.DateOfBirthStr))
                            {
                                // throw new XmlException("Patient's date of birth: date string is not specified.");
                            } else
                            {
                                try
                                {
                                    DateTime date = MsgConst.Const.ConvertTime(Patient.DateOfBirthStr);
                                    Patient.DateOfBirth = date;
                                } catch (Exception)
                                {
                                    // throw new Exception("Malformed patient's data of birth. Details: " +  ex.Message);
                                }
                            }
                        }
                        // Read age in years:
                        if (data.Current == null)
                        {
                            data.Back();
                            // throw new XmlException("Patient's age is not specified.");
                        } else
                        {
                            Patient.AgeStr = data.Value;
                        }
                        // Read age in months:
                        data.NextOrCurrentElement(Const.PatientAgeMonthsElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            // throw new XmlException("Patient's age in months is not specified.");
                        } else
                        {
                            Patient.AgeMonthsStr = data.Value;
                        }
                        // Read Person Id:
                        data.NextOrCurrentElement(Const.PatientPersonIdElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Patient's Person Id is not specified.");
                        } else
                        {
                            Patient.PersonId = data.Value;
                        }
                    }
                    data.BackToMark("Patient");
                } // end of reading patient's data
                // Read Physician data:
                location = "Physician";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading Physician data...");
                data.NextOrCurrentElement(Const.PhysicianContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Physician data is not specified, missing container element: " + Const.PhysicianContainer);
                } else
                {
                    data.SetMark("Physician");
                    data.StepIn();
                    if (data.Current != null)
                        data.NextOrCurrentElement();
                    if (data.Current == null)
                    {
                        throw new XmlException("Physician container does not contain any elements.");
                    }
                    else
                    {
                        Physician = new FindingsPhysician();
                        // Read physician's name:
                        data.NextOrCurrentElement(Const.PhysicianNameElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Physician's name is not specified.");
                        } else
                        {
                            Physician.Name = data.Value;
                        }
                        // Read physician's family name:
                        data.NextOrCurrentElement(Const.PhysicianFamilyNameElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Physician's family name is not specified.");
                        } else
                        {
                            Physician.FamilyName = data.Value;
                        }
                        // Read physician's middle name(s):
                        data.NextOrCurrentElement(Const.PhysicianMiddleNamesElement);
                        if (data.Current == null)
                        {
                            data.Back();
                        } else
                        {
                            Physician.MiddleNames = data.Value;
                        }
                        // Read physician's appelation:
                        data.NextOrCurrentElement(Const.PhysicianAppellationElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            // throw new XmlException("Physician's appellation not specified.");
                        } else
                        {
                            Physician.Appellation = data.Value;
                        }
                        // Read physician's Title:
                        data.NextOrCurrentElement(Const.PhysicianTitleElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            // throw new XmlException("Physician's title not specified.");
                        } else
                        {
                            Physician.Title = data.Value;
                        }
                        // Read physician's right-hand side title:
                        data.NextOrCurrentElement(Const.PhysicianTitleRightElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Physician's right-hand side title not specified.");
                        } else
                        {
                            Physician.TitleRight = data.Value;
                        }
                    }
                    data.BackToMark("Physician");
                } // end of reading physician's data
                // Read Orderer:
                location = "Orderer";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading Orderer...");
                data.NextOrCurrentElement(Const.OrdererElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Orderer is not specified: " + Const.OrdererElement);
                } else
                {
                    Orderer = data.Value;
                }
                // Read Orderer address:
                location = "Orderer address";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading Orderer address...");
                data.NextOrCurrentElement(Const.OrdererAddresselement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Orderer address is not specified: " + Const.OrdererAddresselement);
                } else
                {
                    OrdererAddress = data.Value;
                }
                // Read Specimen's sampling time:
                location = "Sample time";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading sample time...");
                data.NextOrCurrentElement(Const.SampleTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Sample time is not specified: " + Const.SampleTimeElement);
                } else
                {
                    SampleTimeString = data.Value;
                    if (string.IsNullOrEmpty(SampleTimeString))
                    {
                        // throw new XmlException("Sample time: date/time string is not specified.");
                    }
                    else
                    {
                        try
                        {
                            DateTime time = MsgConst.Const.ConvertTime(SampleTimeString);
                            SampleTime = time;
                        }
                        catch /* (Exception ex) */
                        {
                            // throw new Exception("Malformed sample time. Details: " + ex.Message);
                        }
                    }
                }
                // Read reception time:
                location = "Reception time";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading reception time...");
                data.NextOrCurrentElement(Const.ReceptionTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Reception time is not specified: " + Const.ReceptionTimeElement);
                } else
                {
                    ReceptionTimeString = data.Value;
                    if (string.IsNullOrEmpty(ReceptionTimeString))
                    {
                        throw new XmlException("Reception time: date/time string is not specified.");
                    }
                    else
                    {
                        try
                        {
                            DateTime time = MsgConst.Const.ConvertTime(ReceptionTimeString);
                            ReceptionTime = time;
                        }
                        catch /* (Exception ex) */
                        {
                            // throw new Exception("Malformed reception time. Details: " + ex.Message);
                        }
                    }
                }
                // Read completion time:
                location = "Completion time";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading completion time...");
                data.NextOrCurrentElement(Const.CompletionTimeElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Completion time is not specified: " + Const.CompletionTimeElement);
                } else
                {
                    CompletionTimeString = data.Value;
                    if (string.IsNullOrEmpty(CompletionTimeString))
                    {
                        throw new XmlException("Completion time: date/time string is not specified.");
                    }
                    else
                    {
                        try
                        {
                            DateTime time = MsgConst.Const.ConvertTime(CompletionTimeString);
                            CompletionTime = time;
                        }
                        catch /* (Exception ex) */
                        {
                            // throw new Exception("Malformed completion time. Details: " + ex.Message);
                        }
                    }
                }
                data.BackToMark("Head"); // end of Head data, go jump back to container and continue.
                            // instead of this, you can also use data.StepOut().
                // Read Clinical diagnosis data:
                location = "Clinical diagnosis";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading clinical diagnosis data...");
                data.NextOrCurrentElement(Const.DiagnosisClinicalContainter);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Clinical diagnosis data does not exist, container: " +  Const.DiagnosisClinicalContainter);
                } else
                {
                    data.SetMark("DiagnosisClinical");
                    data.StepIn();
                    data.NextOrCurrentElement();
                    if (data.Current == null)
                    {
                        throw new XmlException("Clinical diagnosis container does not contain any elements. Container name: "
                                + Const.DiagnosisClinicalContainter);
                    } else
                    {
                        // Read free text of the clinical diagnosis:
                        data.NextOrCurrentElement(Const.DiagnosisTextElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Clinical diagnosis: diagnosis text is not specified.");
                        } else
                        {
                            DiagnosisClinicalText = data.Value;
                        }
                        data.NextOrCurrentElement(Const.DiagnosisCodeCollectionContainter);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("A collection of clinical diagnosis codes is not specified: " + Const.DiagnosisCodeCollectionContainter);
                        }
                        else
                        {
                            data.StepIn();
                            data.NextOrCurrentElement();
                            if (data.Current == null)
                            {
                                throw new XmlException("Clinical diagnosis: collection of codes does not contain any codes. Element: "
                                        + Const.DiagnosisCodeCollectionContainter);
                            }
                            else
                            {
                                int numcodes = 0;
                                // Read clinical diagnosis codes:
                                do
                                {
                                    data.NextOrCurrentElement(Const.DiagnosisCodeContainer);
                                    if (data.Current != null)
                                    {
                                        ++ numcodes;
                                        data.SetMark("DiagnosisCode");

                                        data.StepIn();
                                        data.NextOrCurrentElement();
                                        if (data.Current == null)
                                        {
                                            throw new Exception("Clinical diagnosis code container does not contain any elements."
                                                + Environment.NewLine + "Diagnosis code No. " + numcodes + ", element: "
                                                + Const.DiagnosisCodeContainer);
                                        } else
                                        {
                                            if (DiagnosesClinical == null)
                                                DiagnosesClinical = new List<DiagnosisCodeClass>();
                                            DiagnosisCodeClass diagnosiscode = new DiagnosisCodeClass();
                                            DiagnosesClinical.Add(diagnosiscode);
                                            // Read Clinical diagnosis code's code:
                                            data.NextOrCurrentElement(Const.DiagnosisCodeCodeElement);
                                            if (data.Current == null)
                                            {
                                                data.Back();
                                                //throw new XmlException("Clinical diagnosis: code is not specified for diagnosis code No. " 
                                                //    + numfindings.ToString());
                                            } else
                                            {
                                                diagnosiscode.Code = data.Value;
                                            }
                                            // Read Clinical diagnosis code's description:
                                            data.NextOrCurrentElement(Const.DiagnosisCodeDescriptionElement);
                                            if (data.Current == null)
                                            {
                                                data.Back();
                                                //throw new XmlException("Clinical diagnosis: description is not specified for diagnosis code No. " 
                                                //    + numfindings.ToString());
                                            } else
                                            {
                                                diagnosiscode.Description = data.Value;
                                            }
                                            // Read Clinical diagnosis code's classification system:
                                            data.NextOrCurrentElement(Const.DiagnosisCodeClassificationSystemElement);
                                            if (data.Current == null)
                                            {
                                                data.Back();
                                                //throw new XmlException("Clinical diagnosis: classification is not specified for diagnosis code No. " 
                                                //    + numfindings.ToString());
                                            } else
                                            {
                                                diagnosiscode.CodeSystem = data.Value;
                                            }
                                        }
                                        data.BackToMark("DiagnosisCode");
                                        data.NextElement();
                                    }
                                } while (data.Current != null);
                            }
                        } // end of reading clinical diagnosis codes
                    }
                    data.BackToMark("DiagnosisClinical");
                }  // end of data for clinical diagnosis

                // Read macroscopic description:
                location = "Macroscopic description";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading macroscopic description...");
                data.NextOrCurrentElement(Const.MacroDescriptionElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Macroscopic description is not specified. Element name: " + Const.MacroDescriptionElement);
                } else
                {
                    MacroDescription = data.Value;
                }
                // Read microscopic description:
                location = "Microscopic description";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading microscopic description...");
                data.NextOrCurrentElement(Const.MicroDescriptionElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Microscopic description is not specified. Element name: " + Const.MicroDescriptionElement);
                } else
                {
                    MicroDescription = data.Value;
                }
                // Read temporary opinion:
                location = "Temporary opinion";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading temporary opinion...");
                data.NextOrCurrentElement(Const.TemporaryOpinionElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Temporary opinion is not specified. Element name: " + Const.TemporaryOpinionElement);
                } else
                {
                    TemporaryOpinion = data.Value;
                }
                // Read appendix:
                location = "Appendix";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading appendix...");
                data.NextOrCurrentElement(Const.AppendixElement);
                if (data.Current == null)
                {
                    data.Back();
                    // throw new XmlException("Appendix is not specified. Element name: " + Const.AppendixElement);
                } else
                {
                    Appendix = data.Value;
                }



                #region FormerFindings
                // Read former findings:
                location = "Former Findings";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading former findings data...");
                data.NextOrCurrentElement(Const.FormerFindingsCollectionContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Former findings are not specified.");
                }
                else
                {
                    data.SetMark("FormerFindings");
                    data.StepIn();
                    data.NextOrCurrentElement();
                    if (data.Current == null)
                    {
                        throw new XmlException("Former findings container does not contain any elements. Container name: "
                                + Const.FormerFindingsCollectionContainer);
                    }
                    else
                    {
                        int numfindings = 0;
                        // Read patohistological diagnosis codes:
                        do
                        {
                            data.NextOrCurrentElement(Const.FormerFindingContainer);
                            if (data.Current != null)
                            {
                                ++numfindings;
                                data.SetMark("FormerFinding");

                                data.StepIn();
                                data.NextOrCurrentElement();
                                if (data.Current == null)
                                {
                                    throw new Exception("Former finding container does not contain any elements."
                                        + Environment.NewLine + "Former finding No. " + numfindings + ", element: "
                                        + Const.FormerFindingContainer);
                                }
                                else
                                {
                                    if (FormerFindings == null)
                                        FormerFindings = new List<RalatedObservation>();
                                    RalatedObservation finding = new RalatedObservation();
                                    FormerFindings.Add(finding);
                                    // Read former finding's reference:
                                    data.NextOrCurrentElement(Const.FormerFindingReferenceElement);
                                    if (data.Current == null)
                                    {
                                        data.Back();
                                        //throw new XmlException("Former finding: reference is not specified for former finding No. " 
                                        //    + numfindings.ToString());
                                    }
                                    else
                                    {
                                        finding.ObservationReference = data.Value;
                                    }
                                    // Read former findinfg's remark:
                                    data.NextOrCurrentElement(Const.FormerFinfingRemarkElement);
                                    if (data.Current == null)
                                    {
                                        data.Back();
                                        //throw new XmlException("Former finding: remark is not specified for former finding No. " 
                                        //    + numfindings.ToString());
                                    }
                                    else
                                    {
                                        finding.Remark = data.Value;
                                    }
                                }
                                data.BackToMark("FormerFinding");
                                data.NextElement();
                            }
                        } while (data.Current != null);


                    }
                    data.BackToMark("FormerFindings");
                }
                #endregion  // FormerFindings



                #region DiagnosisPatoHistological
                // Read PatoHistological diagnosis data:
                location = "Patohistological diagnosis";
                if (R.TreatInfo) R.ReportInfo("DocFindings.Read(doc)", "Reading patohistological diagnosis data...");
                data.NextOrCurrentElement(Const.DiagnosisPatoHistologicalContainer);
                if (data.Current == null)
                {
                    data.Back();
                    throw new XmlException("Patohistological diagnosis data does not exist, container: " + Const.DiagnosisPatoHistologicalContainer);
                } else
                {
                    data.SetMark("DiagnosisPatoHistological");
                    data.StepIn();
                    data.NextOrCurrentElement();
                    if (data.Current == null)
                    {
                        throw new XmlException("Patohistological diagnosis container does not contain any elements. Container name: "
                                + Const.DiagnosisPatoHistologicalContainer);
                    } else
                    {
                        // Read free text of the patohistological diagnosis:
                        data.NextOrCurrentElement(Const.DiagnosisTextElement);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("Patohistological diagnosis: diagnosis text is not specified.");
                        }
                        else
                        {
                            DiagnosisPatoHistologicalText = data.Value;
                        }
                        data.NextOrCurrentElement(Const.DiagnosisCodeCollectionContainter);
                        if (data.Current == null)
                        {
                            data.Back();
                            throw new XmlException("A collection of patohistological diagnosis codes is not specified: " + Const.DiagnosisCodeCollectionContainter);
                        }
                        else
                        {
                            data.StepIn();
                            data.NextOrCurrentElement();
                            if (data.Current == null)
                            {
                                throw new XmlException("Patohistological diagnosis: collection of codes does not contain any codes. Element: "
                                        + Const.DiagnosisCodeCollectionContainter);
                            }
                            else
                            {
                                int numcodes = 0;
                                // Read patohistological diagnosis codes:
                                do
                                {
                                    data.NextOrCurrentElement(Const.DiagnosisCodeContainer);
                                    if (data.Current != null)
                                    {
                                        ++numcodes;
                                        data.SetMark("DiagnosisCode");

                                        data.StepIn();
                                        data.NextOrCurrentElement();
                                        if (data.Current == null)
                                        {
                                            throw new Exception("Patohistological diagnosis code container does not contain any elements."
                                                + Environment.NewLine + "Diagnosis code No. " + numcodes + ", element: "
                                                + Const.DiagnosisCodeContainer);
                                        }
                                        else
                                        {
                                            if (DiagnosesPatoHistological == null)
                                                DiagnosesPatoHistological = new List<DiagnosisCodeClass>();
                                            DiagnosisCodeClass diagnosiscode = new DiagnosisCodeClass();
                                            DiagnosesPatoHistological.Add(diagnosiscode);
                                            // Read patohistological diagnosis code's code:
                                            data.NextOrCurrentElement(Const.DiagnosisCodeCodeElement);
                                            if (data.Current == null)
                                            {
                                                data.Back();
                                                //throw new XmlException("PatoHistological diagnosis: code is not specified for diagnosis code No. " 
                                                //    + numfindings.ToString());
                                            }
                                            else
                                            {
                                                diagnosiscode.Code = data.Value;
                                            }
                                            // Read patohistological diagnosis code's description:
                                            data.NextOrCurrentElement(Const.DiagnosisCodeDescriptionElement);
                                            if (data.Current == null)
                                            {
                                                data.Back();
                                                //throw new XmlException("Patohistological diagnosis: description is not specified for diagnosis code No. " 
                                                //    + numfindings.ToString());
                                            }
                                            else
                                            {
                                                diagnosiscode.Description = data.Value;
                                            }
                                           // Read Patohistological diagnosis code's classification system:
                                            data.NextOrCurrentElement(Const.DiagnosisCodeClassificationSystemElement);
                                            if (data.Current == null)
                                            {
                                                data.Back();
                                                //throw new XmlException("Patohistological diagnosis: classification is not specified for diagnosis code No. " 
                                                //    + numfindings.ToString());
                                            }
                                            else
                                            {
                                                diagnosiscode.CodeSystem = data.Value;
                                            }
                                        }
                                        data.BackToMark("DiagnosisCode");
                                        data.NextElement();
                                    }
                                } while (data.Current != null);
                            }
                        } // end of reading patohistological diagnosis codes
                    }
                    data.BackToMark("DiagnosisPatoHistological");
                }  // end of data for patohistological diagnosis
                #endregion  // DiagnosisPatoHistological

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
                        "Findings.Read(XmlDocument); " + location + ": ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Findings.Read(doc)", "Finished.");
                --R.Depth;
            }
        }  // void Read(XmlParser)



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

                    // Remark: consistency check for findings can be implemented if necessary 
                    rep.AddInfo("Consistency check is not implemented for findings.");

                }
                catch (Exception ex)
                {
                    rep.AddError("Exception has been thrown during data consistency check. Details: " + Environment.NewLine + ex.Message);
                }
            }
        }  // CheckConsistency()


     


        /// <summary>Performs the necessary corrections such that other Labex tools will work with the generated XML.
        /// This is necessary e.g. to show findings, since methods for doing taht can not cope with absent XML fields.</summary>
        public void EnsureCompatibilityBeforeXmlGeneration()
        {
            int nullint = 0;  // replacement for a null integer.
            string nullstring = " ";  // replacement for a null string.
            bool nullbool = true;
            DateTime nulltime = new DateTime(1800,1,1);
            if (CompatibilityMode)
            {
                if (Id == null)
                    Id = nullint;
                if (MsgEventId == null)
                    MsgEventId = nullint;

                if (Header == null)
                    Header = nullstring;
                if (Footer == null)
                    Footer = nullstring;
                if (Heading == null)
                    Heading = nullstring;
                if (OrderCode == null)
                    OrderCode = nullstring;

                if (Patient==null)
                    Patient = new FindingsPatient();

                if (Patient.Name == null)
                    Patient.Name = nullstring;
                if (Patient.FamilyName == null)
                    Patient.FamilyName = nullstring;
                if (Patient.MiddleNames == null)
                    Patient.MiddleNames = nullstring;
                if (Patient.Address == null)
                    Patient.Address = nullstring;
                if (Patient.IsMale == null)
                    Patient.IsMale=nullbool;
                if (Patient.DateOfBirth == null)
                    Patient.DateOfBirth = nulltime;

                if (Physician==null)
                    Physician = new FindingsPhysician();
                
                if (Patient.PersonId == null)
                    Patient.PersonId = nullstring;
                if (Physician.Name == null)
                    Physician.Name = nullstring;
                if (Physician.FamilyName == null)
                    Physician.FamilyName = nullstring;
                if (Physician.MiddleNames == null)
                    Physician.MiddleNames = nullstring;
                if (Physician.Appellation == null)
                    Physician.Appellation = nullstring;
                if (Physician.Title == null)
                    Physician.Title = nullstring;
                if (Physician.TitleRight == null)
                    Physician.TitleRight = nullstring;
                if (Orderer == null)
                    Orderer = nullstring;
                if (OrdererAddress == null)
                    OrdererAddress = nullstring;
                if (SampleTime == null)
                    SampleTime = nulltime;
                if (ReceptionTime == null)
                    ReceptionTime = nulltime;
                if (CompletionTime == null)
                    CompletionTime = nulltime;
                if (MacroDescription == null)
                    MacroDescription = nullstring;
                if (MicroDescription == null)
                    MicroDescription = nullstring;
                if (TemporaryOpinion == null)
                    TemporaryOpinion = nullstring;
                if (Appendix == null)
                    Appendix = nullstring;

                if (DiagnosesClinical == null)
                    DiagnosesClinical = new List<DiagnosisCodeClass>();
                if (DiagnosesClinical.Count < 1)
                {
                    DiagnosisCodeClass code = new DiagnosisCodeClass();
                    DiagnosesClinical.Add(code);                
                }
                if (DiagnosesClinical!=null)
                    for (int i=0;i<DiagnosesClinical.Count;++i)
                    {
                        DiagnosisCodeClass code = DiagnosesClinical[i];
                        if (code.Code==null)
                            code.Code=nullstring;
                        if (code.Description==null)
                            code.Description=nullstring;
                        if (code.CodeSystem==null)
                            code.CodeSystem=nullstring;
                    }

                if (DiagnosesPatoHistological == null)
                    DiagnosesPatoHistological = new List<DiagnosisCodeClass>();
                if (DiagnosesPatoHistological.Count < 1)
                {
                    DiagnosisCodeClass code = new DiagnosisCodeClass();
                    DiagnosesPatoHistological.Add(code);                
                }
                if (DiagnosesPatoHistological != null)
                    for (int i=0;i<DiagnosesPatoHistological.Count;++i)
                    {
                        DiagnosisCodeClass code = DiagnosesPatoHistological[i];
                        if (code.Code==null)
                            code.Code=nullstring;
                        if (code.Description==null)
                            code.Description=nullstring;
                        if (code.CodeSystem==null)
                            code.CodeSystem=nullstring;
                    }


                if (DiagnosisClinicalText == null)
                    DiagnosisClinicalText = nullstring;
                if (DiagnosisPatoHistologicalText == null)
                    DiagnosisPatoHistologicalText = nullstring;

                if (FormerFindings == null)
                    FormerFindings = new List<RalatedObservation>();
                if (FormerFindings.Count < 1)
                {
                    RalatedObservation obs = new RalatedObservation();
                    FormerFindings.Add(obs);
                }
                if (FormerFindings!=null)
                    for (int i = 0; i < FormerFindings.Count; ++i)
                    {
                        RalatedObservation obs = FormerFindings[i];
                        if (obs.ObservationReference == null)
                            obs.ObservationReference = nullstring;
                        if (obs.Remark == null)
                            obs.Remark = nullstring;
                    }


            }  // if (CompatibilityMode)
        } // EnsureCompatibilityBeforeXmlGeneration



        #region RtfSupport

        // RTF Fields support:

        private static object LockRtb = new object();

        private static RichTextBox Rtb = null;


        /// <summary>Returns true if str is a valid rich text, or false otherwise.</summary>
        public static bool IsRtf(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            else
            {
                string rtfhead = "{\\rtf";
                if (string.Compare(str, 0, rtfhead, 0, rtfhead.Length, true /* IgnoreCase */) == 0)
                {
                    lock (LockRtb)
                    {
                        try
                        {
                            if (Rtb == null)
                                Rtb = new RichTextBox();
                            Rtb.Clear();
                            Rtb.Rtf = str;  // If string is actually a rich tect then this will not throw an exceptin, and true will be returned
                            return true;
                        }
                        catch { }
                    }
                }
            }
            return false;
        }




        /// <summary>Returns a rich text that corresponds to the string argument.
        /// If the argument is already a rich text then the unchanged argument is returned, otherwise it is wrapped in RTF markup.</summary>
        public static string ToRtf(string str)
        {
            if (IsRtf(str))
                return str;
            else
            {
                lock (LockRtb)
                {
                    if (Rtb == null)
                        Rtb = new RichTextBox();
                    Rtb.Clear();
                    Rtb.Text = str;
                    return Rtb.Rtf;
                }
            }
        }

        /// <summary>Returns plain text that corresponds to the string argument.
        /// If the argument is a rich text then then text is extracted from it and returned, otherwise the unchanged argument is returned.</summary>
        public static string ToText(string str)
        {
            if (!IsRtf(str))
                return str;
            else
            {
                lock (LockRtb)
                {
                    if (Rtb == null)
                        Rtb = new RichTextBox();
                    Rtb.Clear();
                    Rtb.Rtf = str;
                    return Rtb.Text;
                }
            }
        }


        /// <summary>Aoutonomously converts text either to plain text or to RTF, dependent on the value of UsePlainText flag.</summary>
        private string ConvertRtfField(string str)
        {
            if (UsePlainText)
                return ToText(str);
            else
                return ToRtf(str);
        }


        // Additional filter for setters (repairs the format of RTF that comes from txtControl):

        /// <summary>Returns a rich text that corresponds to the string argument.
        /// If the argument is not a rich text then it is wrapped in RTF markup.
        /// If it is a rich text, the oridinal text is filtered through a rixh text box control in order to
        /// c correct eventual formatting errors.</summary>
        private static string ToRtfSet(string str)
        {
            lock (LockRtb)
            {
                if (Rtb == null)
                    Rtb = new RichTextBox();
                Rtb.Clear();
                if (IsRtf(str))
                    Rtb.Rtf = str;
                else
                    Rtb.Text = str;
                return Rtb.Rtf;
            }
        }


        /// <summary>Aoutonomously converts text either to plain text or to RTF, dependent on the value of UsePlainText flag.</summary>
        private string ConvertRtfFieldSet(string str)
        {
            if (UsePlainText)
                return ToText(str);
            else
                return ToRtfSet(str);
        }

        #endregion  // RtfSupport

        #region Properties

        /// <summary>Inner description.</summary>
        public string InnerDescription
        {
            get { return ConvertRtfField(_innerDescription); }
            set { _innerDescription = ConvertRtfFieldSet(value); }
        }

        /// <summary>Outer description.</summary>
        public string OuterDescription
        {
            get { return ConvertRtfField(_outerDescription); }
            set { _outerDescription = ConvertRtfFieldSet(value); }
        }

        /// <summary>Macro description.</summary>
        public string MacroDescription 
        {
            get { return ConvertRtfField(_macroDescription); }
            set { _macroDescription = ConvertRtfFieldSet(value); }
        }
       
        /// <summary>Micro description.</summary>
        public string MicroDescription 
        {
            get { return ConvertRtfField(_microDescription); }
            set { _microDescription = ConvertRtfFieldSet(value); }
        }

        /// <summary>Macro description.</summary>
        public string MacroDescription2 
        {
            get { return ConvertRtfField(_macroDescription2); }
            set { _macroDescription2 = ConvertRtfFieldSet(value); }
        }
       
        /// <summary>Micro description.</summary>
        public string MicroDescription2 
        {
            get { return ConvertRtfField(_microDescription2); }
            set { _microDescription2 = ConvertRtfFieldSet(value); }
        }


        /// <summary>Free text of clinical diagnosis.</summary>
        public string DiagnosisClinicalText
        {
            get { return ToText(_diagnosisClinicalText); }
            set { _diagnosisClinicalText = ConvertRtfFieldSet(value); }
        }

        /// <summary>Free text of patohistological diagnosis.</summary>
        public string DiagnosisPatoHistologicalText
        {
            get { return ToText(_diagnosisPatoHistologicalText); }
            set { _diagnosisPatoHistologicalText = ConvertRtfFieldSet(value); }
        }


        /// <summary>Temporary opinion.</summary>
        public string TemporaryOpinion
        {
            get { return ConvertRtfField(_temporaryOpinion); }
            set { _temporaryOpinion = ConvertRtfFieldSet(value); }
        }

        /// <summary>Appendix.</summary>
        public string Appendix
        {
            get {
                string ret = ConvertRtfField(_appendix);
                // Console.WriteLine("Appendiks: \"" + ret + "\"");
                return ret;
            }
            set { _appendix = ConvertRtfFieldSet(value); }
        }


        /// <summary>Header.</summary>
        public string Header
        {
            get { return ToText(_header); }
            set { _header = ConvertRtfFieldSet(value); }
        }

        /// <summary>Footer.</summary>
        public string Footer
        {
            get { return ToText(_footer); }
            set { _footer = ConvertRtfFieldSet(value); }
        }


        public bool IsHistological { get { return(ObservationTypeFlag == ObservationType.Histological); } }
        public bool IsCytological { get { return(ObservationTypeFlag == ObservationType.Cytological); } }
        public bool IsPatological { get { return(ObservationTypeFlag == ObservationType.Pathological); } }


        // Signers:


        public string SignerPartString
        {
            get
            {
                if (SignerPart != null)
                    return SignerPart.PrintString();
                else
                    return null;
            }
            private set
            {
                SignerPart = null;
                if (!string.IsNullOrEmpty(value))
                {
                    SignerPart = new FindingsPhysician();
                    SignerPart.Name = value;
                }
            }
        }

        public string SignerFinishString
        {
            get
            {
                if (SignerFinish != null)
                    return SignerFinish.PrintString();
                else
                    return null;
            }
            private set
            {
                SignerFinish = null;
                if (!string.IsNullOrEmpty(value))
                {
                    SignerFinish = new FindingsPhysician();
                    SignerFinish.Name = value;
                }
            }
        }

        public string SignerAdditionString
        {
            get
            {
                if (SignerAddition != null)
                    return SignerAddition.PrintString();
                else
                    return null;
            }
            private set
            {
                SignerAddition = null;
                if (!string.IsNullOrEmpty(value))
                {
                    SignerAddition = new FindingsPhysician();
                    SignerAddition.Name = value;
                }
            }
        }

        // Signature times:

        /// <summary>Converts date to a string for output.</summary>
        private string DateString(DateTime t)
        {
            return t.ToShortDateString();
        }


        /// <summary>Converts date to a string for output.</summary>
        private string DateString(DateTime? t)
        {
            if (t == null)
                return null;
            else
                return DateString(t.Value);
        }

        

        public string SignTimePartString
        {
            get { return _signTimePart; }
            private set { _signTimePart = value; }
        }

        public string SignTimeFinishString
        {
            get { return _signTimeFinish; }
            private set { _signTimeFinish = value; }
        }

        public string SignTimeAdditionString
        {
            get { return _signTimeAddition; }
            private set { _signTimeAddition = value; }
        }


        #endregion  // Properties



        /// <summary>When this flag is set, operations are performed in compatible mode, such that
        /// results and inputs are compatible with the current state (this mainly refers to what Mirjam produced)</summary>
        public bool CompatibilityMode = false;

        /// <summary>If true then plain text is used for fields that are otherwise in RTF.</summary>
        public bool UsePlainText = false;

        public ObservationType ObservationTypeFlag = ObservationType.Unknown;

        string
            _header = null,
            _footer = null,
            Heading = null,
            OrderCode = null,  // Protocol number (Labex internal code)
            Orderer = null,  // consider whether orderer data should be grouped into a special class (element in xml); 
                            // probably this would not be necessary.
            OrdererAddress = null,

            SampleTimeString = null,
            ReceptionTimeString = null,
            CompletionTimeString = null,


            _diagnosisClinicalText = null,

            _innerDescription = null,  // "Inner description is not defined.  // Remark: set to null when not testing!",
            _outerDescription = null,  // "Outer description is not defined."  // Remark: set to null when not testing!,

            _macroDescription = null,
            _microDescription = null,

            _macroDescription2 = null,  // "Macro description 2 is not defined.",  // Remark: set to null when not testing!
            _microDescription2 = null,  // "Micro description 2 is not defined.",  // Remark: set to null when not testing!

            _temporaryOpinion = null,
            _appendix = null,

            _diagnosisPatoHistologicalText = null,

            // Strings representing times of signatures:
            _signTimePart = null,
            _signTimeFinish = null,
            _signTimeAddition = null;
            

        FindingsPatient Patient = null;
        FindingsPhysician 
            Physician = null,
            SignerPart = null,
            SignerFinish = null,
            SignerAddition = null;



        List<DiagnosisCodeClass>
            DiagnosesClinical = new List<DiagnosisCodeClass>(),
            DiagnosesPatoHistological = new List<DiagnosisCodeClass>();

        List<RalatedObservation>
            FormerFindings = new List<RalatedObservation>(),  // former observations for the same patient
            ParentObservations = new List<RalatedObservation>(),  // observation that induced the current oobservation 
            ChildObservations = new List<RalatedObservation>();  // observations induced by the current observation

        DateTime
            CreationTime = DateTime.Now;

        DateTime?
            SampleTime = null,
            ReceptionTime = null,
            CompletionTime = null;

        /// <summary>Id of the Findings database rsr; this is null when Finding data is not read from the database.</summary>
        public int? 
            Id = null,
            MsgEventId = null;


        // public List<MessageAttachment> Attachments = new List<MessageAttachment>();

        
        // Conversions of data to output formats:

        public string SampleTimeOutputString
        {
            get
            {
                if (SampleTime == null)
                    return "XX";
                else
                {
                    if (XmlCreateInputFormat)
                    {
                        return MsgConst.Const.ConvertTime(SampleTime.Value, true /* includetime */, false /* underscores */);
                    } else
                        return SampleTime.Value.ToString();
                }
            }
        }

        public string ReceptionTimeOutputString
        {
            get
            {
                if (ReceptionTime == null)
                    return "XX";
                else
                {
                    if (XmlCreateInputFormat)
                    {
                        return MsgConst.Const.ConvertTime(ReceptionTime.Value, true /* includetime */, false /* underscores */);
                    }
                    else
                        return ReceptionTime.Value.ToString();
                }
            }
        }

        public string CompletionTimeOutputString
        {
            get
            {
                if (CompletionTime == null)
                    return "XX";
                else
                {
                    if (XmlCreateInputFormat)
                    {
                        return MsgConst.Const.ConvertTime(CompletionTime.Value, true /* includetime */, false /* underscores */);
                    }
                    else
                        return CompletionTime.Value.ToString();
                }
            }
        }



        /// <summary>Returns a string describing contents of the current object.</summary>
        public override string ToString()
        {
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Finding.ToString()", "Started...");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine("Observation findings data: ");
                if (string.IsNullOrEmpty(MessageFile))
                    sb.AppendLine("Message file is not specified.");
                else
                    sb.AppendLine("Messge file: " + Environment.NewLine + "  " + MessageFile);
                sb.AppendLine("  **  Findings head  ** ");
                sb.AppendLine("Header: " + Header);

                sb.AppendLine("Footer: " + Footer);
                sb.AppendLine("Heading: " + Heading);
                sb.AppendLine("OrderCode: " + OrderCode);
                if (Patient == null)
                    sb.AppendLine("Patient data is not specified.");
                else sb.Append(Patient.ToString());
                if (Physician == null)
                    sb.AppendLine("Physician data is not specified.");
                else sb.Append(Physician.ToString());
                sb.AppendLine("Orderer: " + Orderer);
                sb.AppendLine("OrdererAddress: " + OrdererAddress);
                sb.AppendLine("  SampleTimeString: " + SampleTimeString);
                if (SampleTime == null)
                    sb.AppendLine("Time when specimens have been taken is not specified.");
                else
                    sb.AppendLine("Sample time: " + SampleTime.ToString());
                sb.AppendLine("  ReceptiontimeString: " + ReceptionTimeString);
                if (ReceptionTime == null)
                    sb.AppendLine("Reception time is not specified.");
                else
                    sb.AppendLine("Reception time: " + ReceptionTime.ToString());
                sb.AppendLine("  CompletionTimeString: " + CompletionTimeString);
                if (CompletionTime == null)
                    sb.AppendLine("Completion time is not specified.");
                else
                    sb.AppendLine("Completion time: " + CompletionTime.ToString());

                sb.AppendLine("  ** End of head.");
                // Output clinical diagnosis with codes:
                if (String.IsNullOrEmpty(DiagnosisClinicalText))
                    sb.AppendLine("Text of clinical diagnosis is not specified.");
                else
                    sb.AppendLine("Text of clinical diagnosis: " + Environment.NewLine
                        + DiagnosisClinicalText + Environment.NewLine);
                if (DiagnosesClinical==null)
                    sb.AppendLine("Clinical diagnosis codes are not specified (null list).");
                else if (DiagnosesClinical.Count==0)
                    sb.AppendLine("There are no clinical diagnosis codes.");
                else
                {
                    sb.AppendLine("Clinical diagnosis codes:");
                    for (int i=0;i<DiagnosesClinical.Count;++i)
                    {
                        sb.AppendLine("Code No. " +  (i+1).ToString() + ":");
                        sb.Append(DiagnosesClinical[i].ToString());
                    }
                }
                // Macroscopic description:
                if (String.IsNullOrEmpty(MacroDescription))
                    sb.AppendLine("Macroscopic description is not specified.");
                else
                    sb.AppendLine("Text of macroscopic description: " + Environment.NewLine
                        + MacroDescription + Environment.NewLine);
                // Microscopic description:
                if (String.IsNullOrEmpty(MicroDescription))
                    sb.AppendLine("Microscopic description is not specified.");
                else
                    sb.AppendLine("Text of microscopic description: " + Environment.NewLine
                        + MicroDescription + Environment.NewLine);

                // Macroscopic description No. 2:
                if (String.IsNullOrEmpty(MacroDescription2))
                    sb.AppendLine("Macroscopic description 2 is not specified.");
                else
                    sb.AppendLine("Text of macroscopic description 2: " + Environment.NewLine
                        + MacroDescription2 + Environment.NewLine);
                // Microscopic description No. 2:
                if (String.IsNullOrEmpty(MicroDescription2))
                    sb.AppendLine("Microscopic description 2 is not specified.");
                else
                    sb.AppendLine("Text of microscopic description 2: " + Environment.NewLine
                        + MicroDescription2 + Environment.NewLine);


                // Temporary opinion:
                if (String.IsNullOrEmpty(TemporaryOpinion))
                    sb.AppendLine("Temporary opinion is not specified.");
                else
                    sb.AppendLine("Text of temporary opinion: " + Environment.NewLine
                        + TemporaryOpinion + Environment.NewLine);
                // Appendix:
                if (String.IsNullOrEmpty(Appendix))
                    sb.AppendLine("Appendix is not specified.");
                else
                    sb.AppendLine("Appendix: " + Environment.NewLine
                        + Appendix + Environment.NewLine);
                // Former findings:
                if (FormerFindings==null)
                    sb.AppendLine("Former findings are not specified (null list).");
                else if (FormerFindings.Count==0)
                    sb.AppendLine("There are no former findings attached.");
                else
                {
                    sb.AppendLine("Former findings:");
                    for (int i=0;i<FormerFindings.Count;++i)
                    {
                        sb.AppendLine("Finding No. " +  (i+1).ToString());
                        sb.Append(FormerFindings[i].ToString());
                    }
                }
                // Patohistological diagnosis with codes:
                if (String.IsNullOrEmpty(DiagnosisPatoHistologicalText))
                    sb.AppendLine("Text of patohistological diagnosis is not specified.");
                else
                    sb.AppendLine("Text of patohistological diagnosis: " + Environment.NewLine
                        + DiagnosisPatoHistologicalText + Environment.NewLine);
                if (DiagnosesPatoHistological==null)
                    sb.AppendLine("Patohistological diagnosis codes are not specified (null list).");
                else if (DiagnosesPatoHistological.Count==0)
                    sb.AppendLine("There are no patohistological diagnosis codes.");
                else
                {
                    sb.AppendLine("Patohistological diagnosis codes:");
                    for (int i=0;i<DiagnosesPatoHistological.Count;++i)
                    {
                        sb.AppendLine("Code No. " +  (i+1).ToString());
                        sb.Append(DiagnosesPatoHistological[i].ToString());
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
                if (R.TreatInfo) R.ReportInfo("Finding.ToString()", "Finished.");
                --R.Depth;
            }
        }


        /// <summary>If true then XML is created for data in input format, which means that all fields can be read
        /// back when parding that XML.
        /// Default is false, in this case the generated XML is in output format more suitable for printing and jviewing
        /// (i.e. better for human reading), but in this form some fields (data and gender fields) can not be interpreted
        /// correctly when such a file is parsed.</summary>
        public bool XmlCreateInputFormat = false;

        /// <summary>If true then processing instruction is added to the XML documents created from data that
        /// enables viewing in browser in a human readable form.</summary>
        public bool XmlAddStyleSheetInstruction = true;

        /// <summary>Converts a msg to Xml and returns it.</summary>
        public override XmlDocument ToXml()
        {
            string location = "Building XML";
            ++R.Depth;
            if (R.TreatInfo) R.ReportInfo("Finding.ToXml()", "Started...");
            try
            {
                // Perform the necessary corrections such that other Labex tools will work with the generated XML:
                if (CompatibilityMode)
                    EnsureCompatibilityBeforeXmlGeneration();
                // Create a new document with specified root name:
                Data.NewDocument(Const.RootName);
                if (XmlAddStyleSheetInstruction)
                    Data.AddProcessingInstruction(Const.ViewInstructionTarget,Const.ViewInstructionData);
                #region Head
                // Create the Head container:
                location = "Head";
                if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Head...");
                Data.CreateElement(Const.HeadContainer);
                Data.AppendChild();
                // Move to the inserted head container and mark position:
                Data.MoveToNewest();  
                Data.SetMark("Head");
                // Create one by one the sub-elements of the Head container:
                if (Header != null)
                {
                    // Create and insert Head.Header:
                    Data.CreateElement(Const.HeaderElement);
                    Data.SetInnerText(Header);
                    Data.AppendChild();
                }
                // Create and insert Head.Footer:
                if (Footer != null)
                {
                    Data.CreateElement(Const.Footerelement);
                    Data.SetInnerText(Footer);
                    Data.AppendChild();
                }
                // Create and insert Head.Heading:
                if (Heading != null)
                {
                    Data.CreateElement(Const.HeadingElement);
                    Data.SetInnerText(Heading);
                    Data.AppendChild();
                }
                // Create and insert Head.OrderCode:
                if (OrderCode != null)
                {
                    Data.CreateElement(Const.OrderCodeElement);
                    Data.SetInnerText(OrderCode);
                    Data.AppendChild();
                }
                #region Patient
                if (Patient != null)
                {
                    // Create and insert Heading.Patient data:
                    location = "Patient";
                    if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Patient data...");
                    Data.CreateElement(Const.PatientContainer);
                    Data.AppendChild();
                    Data.MoveToNewest();  // Move to the inserted Patient container:
                    // Create and insert Head.Patient.Name:
                    if (Patient.Name != null)
                    {
                        Data.CreateElement(Const.PatientNameElement);
                        Data.SetInnerText(Patient.Name);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Patient.FamilyName:
                    if (Patient.FamilyName != null)
                    {
                        Data.CreateElement(Const.PatientFamilyNameElement);
                        Data.SetInnerText(Patient.FamilyName);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Patient.MiddleNames:
                    if (Patient.MiddleNames != null)
                    {
                        Data.CreateElement(Const.PatientMiddleNamesElement);
                        Data.SetInnerText(Patient.MiddleNames);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Patient.Address:
                    if (Patient.Address != null)
                    {
                        Data.CreateElement(Const.PatientAddressElement);
                        Data.SetInnerText(Patient.Address);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Patient.Gender:
                    Data.CreateElement(Const.PatientSexElement);
                    if (XmlCreateInputFormat)
                        Data.SetInnerText(Patient.GenderOutputStringInputFormat);
                    else
                        Data.SetInnerText(Patient.GenderOutputString);
                    Data.AppendChild();
                    // Create and insert Head.Patient.DateOfBirth:
                    Data.CreateElement(Const.PatientDateOfBirthElement);
                    if (XmlCreateInputFormat)
                        Data.SetInnerText(Patient.DateOfBirthOutputStringInputFormat);
                    else
                        Data.SetInnerText(Patient.DateOfBirthOutputString);
                    Data.AppendChild();
                    // Create and insert Head.Patient.Age:
                    Data.CreateElement(Const.PatientAgeElement);
                    Data.SetInnerText(Patient.Age.ToString());
                    Data.AppendChild();
                    // Create and insert Head.Patient.AgeMonths:
                    Data.CreateElement(Const.PatientAgeMonthsElement);
                    Data.SetInnerText(Patient.AgeMonths.ToString());
                    Data.AppendChild();
                    // Create and insert Head.Patient.PersonId:
                    if (Patient.PersonId != null)
                    {
                        Data.CreateElement(Const.PatientPersonIdElement);
                        Data.SetInnerText(Patient.PersonId);
                        Data.AppendChild();
                    }
                    // After inserting all Patient data, move back to the Head container:
                    Data.GoToMark("Head");
                }
                #endregion  // Patient
                #region Physician
                if (Physician != null)
                {
                    // Create and insert Heading.Physician data:
                    location = "Physician";
                    if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Physician data...");
                    Data.CreateElement(Const.PhysicianContainer);
                    Data.AppendChild();
                    Data.MoveToNewest();  // move to the inserted Physician container
                    // Create and insert Head.Physician.Name:
                    if (Physician.Name != null)
                    {
                        Data.CreateElement(Const.PhysicianNameElement);
                        Data.SetInnerText(Physician.Name);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Physician.FamilyName:
                    if (Physician.FamilyName != null)
                    {
                        Data.CreateElement(Const.PhysicianFamilyNameElement);
                        Data.SetInnerText(Physician.FamilyName);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Physician.MiddleNames:
                    // TODO: find a different solution for this!
                    // $$$$ now this element is inserted in any case even if contents are null
                    // This is because otherwise the XML could not be shown on the forms!
                    // If this deficiency is resolved in those forms then this will not be necessary any more!
                    if (true /* || Physician.MiddleNames != null */ )
                    {
                        Data.CreateElement(Const.PhysicianMiddleNamesElement);
                        Data.SetInnerText(Physician.MiddleNames);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Physician.Appellation:
                    if (Physician.Appellation != null)
                    {
                        Data.CreateElement(Const.PhysicianAppellationElement);
                        Data.SetInnerText(Physician.Appellation);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Physician.Title:
                    if (Physician.Title != null)
                    {
                        Data.CreateElement(Const.PhysicianTitleElement);
                        Data.SetInnerText(Physician.Title);
                        Data.AppendChild();
                    }
                    // Create and insert Head.Physician.TitleRight:
                    if (Physician.TitleRight != null)
                    {
                        Data.CreateElement(Const.PhysicianTitleRightElement);
                        Data.SetInnerText(Physician.TitleRight);
                        Data.AppendChild();
                    }
                    // After inserting all Patient data, move back to the Head container:
                    Data.GoToMark("Head");
                }
                #endregion  // Physician

                location = "Head";
                if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Head data...");

                // Create and insert Head.Orderer:
                if (Orderer != null)
                {
                    Data.CreateElement(Const.OrdererElement);
                    Data.SetInnerText(Orderer);
                    Data.AppendChild();
                }
                // Create and insert Head.OrdererAddress:
                if (OrdererAddress != null)
                {
                    Data.CreateElement(Const.OrdererAddresselement);
                    Data.SetInnerText(OrdererAddress);
                    Data.AppendChild();
                }
                // Sample time:
                Data.CreateElement(Const.SampleTimeElement);
                Data.SetInnerText(SampleTimeOutputString);
                Data.AppendChild();
                // Reception time:
                Data.CreateElement(Const.ReceptionTimeElement);
                Data.SetInnerText(ReceptionTimeOutputString);
                Data.AppendChild();
                // Completion time:
                Data.CreateElement(Const.CompletionTimeElement);
                Data.SetInnerText(CompletionTimeOutputString);
                Data.AppendChild();
                // Finish composition of the Head element:
                Data.BackToMark("Head");
                #endregion  // Head

                #region DiagnosisClinical
                if (!string.IsNullOrEmpty(DiagnosisClinicalText) || DiagnosesClinical.Count>0)
                {
                    // Clinical diagnosis:
                    location = "Clinical diagnosis";
                    if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Clinical diagnosis data...");

                    Data.CreateElement(Const.DiagnosisClinicalContainter);
                    Data.InsertAfter();
                    Data.MoveToNewest();  // move to the inserted clinical diagnosis container and mark position
                    Data.SetMark("DisgnosisClinical");
                    // Create and insert DiagnosisClinical.Text:
                    if (!String.IsNullOrEmpty(DiagnosisClinicalText))
                    {
                        Data.CreateElement(Const.DiagnosisTextElement);
                        Data.SetInnerText(DiagnosisClinicalText);
                        //Data.InsertAfter();
                        Data.AppendChild();
                    }
                    if (DiagnosesClinical.Count>0)
                    {
                        // Create and insert DiagnosisClinical.Codes (container of diagnosis codes) and move to it:
                        Data.CreateElement(Const.DiagnosisCodeCollectionContainter);
                        Data.AppendChild();
                        Data.MoveToNewest();
                        Data.SetMark("DisgnosisCodes");
                        for (int i = 0; i < DiagnosesClinical.Count; ++i)
                        {
                            DiagnosisCodeClass diag = DiagnosesClinical[i];
                            if (diag != null)
                            {
                                // Add next diagnosis code:
                                Data.CreateElement(Const.DiagnosisCodeContainer);
                                Data.AppendChild();
                                Data.MoveToNewest();
                                // Create Diagnosis code:
                                Data.CreateElement(Const.DiagnosisCodeCodeElement);
                                Data.SetInnerText(diag.Code);
                                Data.AppendChild();
                                //// Create Diagnosis code description:
                                //Data.CreateElement(Const.DiagnosisCodeDescriptionElement);
                                //Data.SetInnerText(diag.Description);
                                //Data.AppendChild();
                                // Create Diagnosis code description:
                                Data.CreateElement(Const.DiagnosisCodeDescriptionElement);
                                Data.SetInnerText(diag.Description);
                                Data.AppendChild();
                                // Create Diagnosis code classification system:
                                Data.CreateElement(Const.DiagnosisCodeClassificationSystemElement);
                                Data.SetInnerText(diag.CodeSystem);
                                Data.AppendChild();
                            }
                            Data.GoToMark("DisgnosisCodes");
                        }
                        Data.BackToMark("DisgnosisCodes");
                    }
                    Data.BackToMark("DisgnosisClinical");
                }
                #endregion  // DiagnosisClinical

                #region Texts
                // Insert inner description:
                if (!string.IsNullOrEmpty(InnerDescription))
                {
                    Data.CreateElement(Const.InnerDescriptionElement);
                    Data.SetInnerText(InnerDescription);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }
                // Insert outer description:
                if (!string.IsNullOrEmpty(OuterDescription))
                {
                    Data.CreateElement(Const.OuterDescriptionElement);
                    Data.SetInnerText(OuterDescription);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }


                // Insert macroscopic description:
                if (!string.IsNullOrEmpty(MacroDescription))
                {
                    Data.CreateElement(Const.MacroDescriptionElement);
                    Data.SetInnerText(MacroDescription);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }
                // Insert microscopic description:
                if (!string.IsNullOrEmpty(MicroDescription))
                {
                    Data.CreateElement(Const.MicroDescriptionElement);
                    Data.SetInnerText(MicroDescription);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }

                // Insert temporary opinion:
                if (!string.IsNullOrEmpty(TemporaryOpinion))
                {
                    Data.CreateElement(Const.TemporaryOpinionElement);
                    Data.SetInnerText(TemporaryOpinion);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }


                // Insert signer data for temporary opinion:
                if (!string.IsNullOrEmpty(SignerPartString))
                {
                    // Signer's name and title:
                    Data.CreateElement(Const.SignerPartElement);
                    Data.SetInnerText(SignerPartString);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                    // Signature date:
                    Data.CreateElement(Const.SignDatePartElement);
                    Data.SetInnerText(SignTimePartString);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }

                // Insert macroscopic description 2:
                if (!string.IsNullOrEmpty(MacroDescription2))
                {
                    Data.CreateElement(Const.MacroDescriptionElement2);
                    Data.SetInnerText(MacroDescription2);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }
                // Insert microscopic description 2:
                if (!string.IsNullOrEmpty(MicroDescription2))
                {
                    Data.CreateElement(Const.MicroDescriptionElement2);
                    Data.SetInnerText(MicroDescription2);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }

                // Insert signer data for finished findings:
                if (!string.IsNullOrEmpty(SignerFinishString))
                {
                    // Signer's name and title:
                    Data.CreateElement(Const.SignerFinishElement);
                    Data.SetInnerText(SignerFinishString);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                    // Signature date:
                    Data.CreateElement(Const.SignDateFinishElement);
                    Data.SetInnerText(SignTimeFinishString);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }

                // Insert appendix:
                if (!string.IsNullOrEmpty(Appendix))
                {
                    Data.CreateElement(Const.AppendixElement);
                    Data.SetInnerText(Appendix);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }

                // Insert signer data for addition to findings (appendix):
                if (!string.IsNullOrEmpty(SignerAdditionString))
                {
                    // Signer's name and title:
                    Data.CreateElement(Const.SignerAdditionElement);
                    Data.SetInnerText(SignerAdditionString);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                    // Signature date:
                    Data.CreateElement(Const.SignDateAdditionElement);
                    Data.SetInnerText(SignTimeAdditionString);
                    Data.InsertAfter();
                    Data.MoveToNewest();
                }

                #endregion  // Texts

                #region FormerFindings
                if (FormerFindings.Count > 0)
                {
                    // Former findings:
                    location = "Former findings";
                    if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Former findings data...");

                    Data.CreateElement(Const.FormerFindingsCollectionContainer);
                    Data.InsertAfter();
                    Data.MoveToNewest();  // move to the inserted former findings container and mark position
                    Data.SetMark("FormerFindings");
                    if (FormerFindings.Count > 0)
                    {
                        for (int i = 0; i < FormerFindings.Count; ++i)
                        {
                            RalatedObservation finding = FormerFindings[i];
                            if (finding != null)
                            {
                                // Add next former finding:
                                Data.CreateElement(Const.FormerFindingContainer);
                                Data.AppendChild();
                                Data.MoveToNewest();
                                // Create Finding reference:
                                Data.CreateElement(Const.FormerFindingReferenceElement);
                                Data.SetInnerText(finding.ObservationReference);
                                Data.AppendChild();
                                // Create Finding remark:
                                Data.CreateElement(Const.FormerFinfingRemarkElement);
                                Data.SetInnerText(finding.Remark);
                                Data.AppendChild();

                            }
                            Data.GoToMark("FormerFindings");
                        }
                    }
                    Data.BackToMark("FormerFindings");
                }
                #endregion  // FormerFindings

                #region DiagnosisPatoHistological
                if (!string.IsNullOrEmpty(DiagnosisPatoHistologicalText) || DiagnosesPatoHistological.Count > 0)
                {
                    // Patohistological diagnosis:
                    location = "Patohistological diagnosis";
                    if (R.TreatInfo) R.ReportInfo("DocFindings.ToXml()", "Creating Patohistological diagnosis data...");

                    Data.CreateElement(Const.DiagnosisPatoHistologicalContainer);
                    Data.InsertAfter();
                    Data.MoveToNewest();  // move to the inserted patohistological diagnosis container and mark position
                    Data.SetMark("DiagnosisPatoHistological");
                    // Create and insert DiagnosisPatoHistological.Text:
                    if (!String.IsNullOrEmpty(DiagnosisPatoHistologicalText))
                    {
                        Data.CreateElement(Const.DiagnosisTextElement);
                        Data.SetInnerText(DiagnosisPatoHistologicalText);
                        //Data.InsertAfter();
                        Data.AppendChild();
                    }
                    if (DiagnosesPatoHistological.Count > 0)
                    {
                        // Create and insert DiagnosisPatoHistological.Codes (container of diagnosis codes) and move to it:
                        Data.CreateElement(Const.DiagnosisCodeCollectionContainter);
                        Data.AppendChild();
                        Data.MoveToNewest();
                        Data.SetMark("DisgnosisCodes");
                        for (int i = 0; i < DiagnosesPatoHistological.Count; ++i)
                        {
                            DiagnosisCodeClass diag = DiagnosesPatoHistological[i];
                            if (diag != null)
                            {
                                // Add next diagnosis code:
                                Data.CreateElement(Const.DiagnosisCodeContainer);
                                Data.AppendChild();
                                Data.MoveToNewest();
                                // Create Diagnosis code:
                                Data.CreateElement(Const.DiagnosisCodeCodeElement);
                                Data.SetInnerText(diag.Code);
                                Data.AppendChild();
                                //// Create Diagnosis code description:
                                //Data.CreateElement(Const.DiagnosisCodeDescriptionElement);
                                //Data.SetInnerText(diag.Description);
                                //Data.AppendChild();
                                // Create Diagnosis code description:
                                Data.CreateElement(Const.DiagnosisCodeDescriptionElement);
                                Data.SetInnerText(diag.Description);
                                Data.AppendChild();
                                // Create Diagnosis code classification system:
                                Data.CreateElement(Const.DiagnosisCodeClassificationSystemElement);
                                Data.SetInnerText(diag.CodeSystem);
                                Data.AppendChild();
                            }
                            Data.GoToMark("DisgnosisCodes");
                        }
                        Data.BackToMark("DisgnosisCodes");
                    }
                    Data.BackToMark("DiagnosisPatoHistological");
                }
                #endregion  // DiagnosisPatoHistological
            }
            catch (Exception ex)
            {
                R.ReportError("Location: " + location + " ", ex);
                throw ReporterBase.ReviseException(ex,
                        "Findings.ToXml(); " + location + ": ");
            }
            finally
            {
                if (R.TreatInfo) R.ReportInfo("Findings.ToXml()", "Finished.");
                --R.Depth;
            }
            return Doc;
        }


    } // class Finding





}   // namespace LabexBis

