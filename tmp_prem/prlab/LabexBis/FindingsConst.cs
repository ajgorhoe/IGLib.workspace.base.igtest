using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
// using LabexUtilities;
// using System.Configuration;

namespace IG.Lib
{


    /// <summary>Contains various names and other constants used in findings.</summary>
    public abstract class FindingsConsttBase
    {

        public string
            ViewInstructionTarget = "xml-stylesheet",
            ViewInstructionData = "type=\"text/xsl\" href=\"FindingsHistoPatological.xsl\"",

            RootName = "Findings",  // name of the root element

            HeadContainer = "Head",
            HeaderElement = "Header",
            Footerelement = "Footer",
            HeadingElement = "Heading",
            OrderCodeElement = "OrderCode",

            PatientContainer = "Patient",
            PatientNameElement = "Name",
            PatientFamilyNameElement = "FamilyName",
            PatientMiddleNamesElement = "MiddleNames",
            PatientAddressElement = "Address",
            PatientSexElement = "Sex",
            PatientDateOfBirthElement = "DateOfBirth",
            PatientAgeElement = "Age",
            PatientAgeMonthsElement = "AgeMonths",
            PatientPersonIdElement = "PersonId",

            PhysicianContainer = "Physician",
            PhysicianNameElement = "Name",
            PhysicianFamilyNameElement = "FamilyName",
            PhysicianMiddleNamesElement = "MiddleNames",
            PhysicianAppellationElement = "Appellation",
            PhysicianTitleElement = "Title",
            PhysicianTitleRightElement = "TitleRight",

            OrdererElement = "Orderer",
            OrdererAddresselement = "OrdererAddress",
            SampleTimeElement = "SampleTime",
            ReceptionTimeElement = "ReceptionTime",
            CompletionTimeElement = "CompletionTime",

            DiagnosisClinicalContainter = "DiagnosisClinical",

            DiagnosisTextElement = "Text",

            DiagnosisCodeCollectionContainter = "Codes",
            DiagnosisCodeContainer = "DiagnosisCode",
            DiagnosisCodeCodeElement = "Code",
            DiagnosisCodeDescriptionElement = "CodeDescription",
            // DiagnosisCodeExtensionElement = "CodeExtension",
            DiagnosisCodeClassificationSystemElement = "ClassificationSystem",

            InnerDescriptionElement = "InnerDescription",
            OuterDescriptionElement = "OuterDescription",

            MacroDescriptionElement = "MacroDescription",
            MicroDescriptionElement = "MicroDescription",

            MacroDescriptionElement2 = "MacroDescription2",
            MicroDescriptionElement2 = "MicroDescription2",

            TemporaryOpinionElement = "TemporaryOpinion",
            AppendixElement = "Appendix",

            FormerFindingsCollectionContainer = "FormerFindings",

            FormerFindingContainer = "Finding",
            FormerFindingReferenceElement = "FindingReference",
            FormerFinfingRemarkElement = "Remark",

            DiagnosisPatoHistologicalContainer = "DiagnosisPatoHistological",

            SignerPartElement = "SignerPart",
            SignDatePartElement = "SignDatePart",

            SignerFinishElement = "SignerFinish",
            SignDateFinishElement = "SignDateFinish",

            SignerAdditionElement = "SignerAddition",
            SignDateAdditionElement = "SignDateAddition";


    }  // abstract class FindingsConsttBase


    /// <summaryContains constants for XML findings.
    /// Constants are not defined static in order to enable corrections for different variants of the program.</summary>
    public class FindingsConst : FindingsConsttBase
    {

        #region Global


        private static FindingsConst _findingsConst = null;

        /// <summary>Returns the process-wide class containing constants used in XML findings.
        /// Class is initialized with respect to the application variant that is currently running.</summary>
        public static FindingsConst Const
        {
            get
            {
                if (_findingsConst == null)
                {
                    _findingsConst = new FindingConstGolnik();
                }
                return _findingsConst;
            }
        }

        #endregion Global



    }  // class FindingsConst


    /// <summary>Here one can handle specifics with respect to different variants of the program
    /// for different customers.
    /// This class should not contain any additional definitions!</summary>
    public class FindingConstGolnik : FindingsConst
    {


        // REMARK:
        // This class is empty because the base class MsgConst is adjusted for Golnik. 
        // For other classes, differences with respect to specification in MsgConst 
        // (now adjusted for Golnik) will be put into class' constructor.


    }  





}   // namespace LabexBis

