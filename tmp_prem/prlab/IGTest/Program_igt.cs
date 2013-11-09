using System;
using System.Collections.Generic;
using System.Xml;

using System.Windows.Forms;

using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using Premisa.PadoInterfaces;


using System.IO;

using xProcs.Common.Converter;  // For conversion of roman numbers

using LabexBis;  // communication with the Birpis system


namespace IGTest
{
    class ProgramIGTest
    {
        /// <summary>Reporter for this class.</summary>
        public static Premisa.ReporterMsg.Reporter R { get { return LabexUtilities.Rep.Server; } }


        static void Main(string[] args)
        {

            try
            {

                //TestNumberConverter();
                //TestW3cWs();




                // TESTING OF SpecimenObservationOrder:
                #region TestObservationOrder

                bool TestObservationOrder = false;
                if (TestObservationOrder)
                {

                    TestMsgObservationOrder(
                    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\Incoming_2009_05_27_15_31_59__ID.7347013c-c86d-1004-8308-fceda6e7ba8a.xml");


                                // Šablonsko sporočilo:
                                TestMsgObservationOrder(
                    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationOrder.xml");


                    TestMsgObservationEvent(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent.xml");

                    TestMsgFinancialTransaction(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction.xml");

                    TestcalypsoWsSimple();

                    TestCalypsoWs();

                    // $$ Testing of messages:
                    ReceiveSpecimenObservationOrder(true /*  get off the queue after reading */ );

                }

                #endregion  // TestObservationOrder



                // TESTING OF Findings:
                #region TestFindings

                //TestFindings(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\FindingsHistoPatological_Example.xml",
                //    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\FindingsHistoPatological_Out.xml", true);

                //// Final check: read data from output of the previous call and write it to a new output file. Both files can
                //// then be compared to check for possible data loss or modification.
                //// Remark: some data WILL be lost at data and gender fields because output format is different than input format.
                //// This is prevented by setting last argument in the previous call to true.
                //TestFindings(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\FindingsHistoPatological_Out.xml",
                //    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\FindingsHistoPatological_Out_1.xml", true);

                #endregion  // TestFindings


                #region TestNullifyObservationOrder
                bool TestNullifyObservationOrder = true;
                bool SendToCalypso = true;   // If true then the message is actually sent to the web service:

                if (TestNullifyObservationOrder)
                {
                    string InputNullify = @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\Incoming_2009_08_12_10_48__ID.f49df78d-c907-1004-890e-885276b8b55a____bbfa9f51-8e6e-4d10-ab9e-d022a16a9fa4.xml";
                    string OutputNullify = @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\Nullify_2009_08_12_10_13__ID.e51d21ad-c907-1004-890e-885276b8b55a____edd6daad-0ef6-44b9-9a68-18b58ccb01f8.xml";

                    if (InputNullify.IndexOf("Incoming") >= 0)
                    {
                        OutputNullify = InputNullify.Replace("Incoming", "Nullify");
                        Console.WriteLine("Resulting file name generated from the original one. Filename: "
                            + Environment.NewLine + "  " + OutputNullify);
                    }

                    if (SendToCalypso)
                    {
                        // Actually send the nullification request to calypso:

                        InputNullify = @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\Incoming_2009_08_13_16_47__ID.719752cd-c90a-1004-890e-885276b8b55a____a2369d8b-7a72-45db-b0d8-4515d1ecce0e.xml";
                        string MessageId = "ID.719752cd-c90a-1004-890e-885276b8b55a";

                        TestMsgObservationOrderNullifySend(InputNullify, MessageId);
                    } else
                    {
                        // Just save the nullification message generated from the original:
                        TestMsgObservationOrderNullify(InputNullify, OutputNullify);
                    }
                }


                #endregion // TestNullifyObservationOrder


                // TESTING OF ObservationEvent:
                #region ObservationEvent

                bool TestObservationEvent        = false;
                bool TestSendingObservationEvent = false;
                if (TestObservationEvent)
                {

                    string FindingsFile = null;
                    // Set findings file (if not set then findings will just be read from the example message):
                    FindingsFile = @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\FindingsHistoPatological_Example.xml";
                    FindingsFile = null;

                    if (!TestSendingObservationEvent)
                    {
                        //Console.WriteLine("Testing ObservationEvent, files from the schemas directory.");
                        //Console.WriteLine();
                        //TestMsgObservationEvent(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Example.xml",
                        //    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Model.xml",
                        //    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Out.xml",
                        //    FindingsFile);

                        //// Final check: read data from output of the previous call and write it to a new output file. Both files can
                        //// then be compared to check for possible data loss or modification.
                        //TestMsgObservationEvent(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Out.xml",
                        //    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Model.xml",
                        //    @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Out1.xml",
                        //    null /* FindingsFile */ );


                        // Take files from test directory instead of schema directory:

                        Console.WriteLine("Testing ObservationEvent, files from the testmessages directory.");
                        Console.WriteLine();

                        FindingsFile = @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\FindingsHistoPatological_Example_01.xml";

                        TestMsgObservationEvent(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\SpecimenObservationEvent_Example_01.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Model.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\SpecimenObservationEvent_Out.xml",
                            FindingsFile);

                        // Final check: read data from output of the previous call and write it to a new output file. Both files can
                        // then be compared to check for possible data loss or modification.
                        TestMsgObservationEvent(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\SpecimenObservationEvent_Out.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Model.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\SpecimenObservationEvent_Out1.xml",
                            null /* FindingsFile */ );

                    }
                    else
                    {

                        // SENDING of ObservationEvent:
                        Console.WriteLine("Sending of ObservationEvent to the web service...");
                        FindingsFile = @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\FindingsHistoPatological_Example_01.xml";

                        TestMsgObservationEvent(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\SpecimenObservationEvent_Example_01.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationEvent_Model.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\SpecimenObservationEvent_Out.xml",
                            FindingsFile, true);

                    }
                }  // if TestObservationEvent

                #endregion  // ObservationEvent


                // TESTING OF FinancialTransaction:
                #region FinancialTransaction

                bool TestFinancialTransaction        = false;
                bool TestSendingFinancialTransaction = false;
                if (TestFinancialTransaction)
                {
                    if (!TestSendingFinancialTransaction)
                    {

                        Console.WriteLine("Testing Financial transaction...");
                        Console.WriteLine();
                        TestMsgFinancialTransaction(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Example.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Model.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Out.xml");

                        // Final check: read data from output of the previous call and write it to a new output file. Both files can
                        // then be compared to check for possible data loss or modification.
                        TestMsgFinancialTransaction(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Out.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Model.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Out1.xml");

                        // Files taken from the testmessages directory:
                        Console.WriteLine("Testing Financial transaction, files taken from testmessages directory...");
                        Console.WriteLine();


                    } else  // perform sending
                    {

                        // SENDING of FinancialTransaction:
                        Console.WriteLine("Sending of FinancialTransaction to the web service...");

                        TestMsgFinancialTransaction(@"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\DetailedFinancialTransaction_Example_01.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\DetailedFinancialTransaction_Model.xml",
                            @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\testmessages\DetailedFinancialTransaction_Out.xml",
                            true);

                    }

                }

                #endregion  // FinancialTransaction


                // Testing RTF findings:
                #region TestRtffindings
                bool TestRtfFindings = false;
                if (TestRtfFindings)
                {

                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Running the RTF test in a form..." + Environment.NewLine);

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FindingsRtfTest());

                    Console.WriteLine("RTF test done." + Environment.NewLine + Environment.NewLine);
                }
                #endregion  // TestRtffindings





            }
            catch (Exception ex)
            {
                R.ReportError(ex,"Main block");
            }
        }    // Main



        #region SpecimenOservationEvent


        /// <summary>Reads observatin event message from the specified XML file and outputs the contents.
        /// File must contian data according to specification of SpecimenOservationEvent.
        /// DOES NOT convert the object back to XML!!!</summary>
        /// <param name="MessageFile">Name of the file containing the message.</param>
        static void TestMsgObservationEvent(string MessageFile)
        {
            TestMsgObservationEvent(MessageFile, null, null, null);
        }

        /// <summary>Test of a message read from the specified file and reads its data.
        /// Message contained in the file must be of type SpecimenObservationEvent.</summary>
        /// <param name="MessageFilename">Name of the file containing the message. If not specified
        /// then name is obtained from application settings.</param>
        /// <param name="ModelFile">Name of the file that is used as a model for message construction.
        /// Its specification is obligatory when object is converted to XML.</param>
        /// <param name="OutputXmlFile">Name of the file to which XML representation of the object
        /// is written after object contents is read (interpreted).</param>
        /// <param name="FindingsFile">Name of the file that contains findings to be included in the message.
        /// If not specified then findings are just transcribed from the XML file.</param>
        static void TestMsgObservationEvent(string MessageFilename, string ModelFile,
                string OutputXmlFile, string FindingsFile)
        {
            TestMsgObservationEvent(MessageFilename, ModelFile, OutputXmlFile, FindingsFile, false /* SendToCalypso */);
        }

        /// <summary>Test of a message read from the specified file and reads its data.
        /// Message contained in the file must be of type SpecimenObservationEvent.</summary>
        /// <param name="MessageFilename">Name of the file containing the message. If not specified
        /// then name is obtained from application settings.</param>
        /// <param name="ModelFile">Name of the file that is used as a model for message construction.
        /// Its specification is obligatory when object is converted to XML.</param>
        /// <param name="OutputXmlFile">Name of the file to which XML representation of the object
        /// is written after object contents is read (interpreted).</param>
        /// <param name="FindingsFile">Name of the file that contains findings to be included in the message.
        /// If not specified then findings are just transcribed from the XML file.</param>
        static void TestMsgObservationEvent(string MessageFilename, string ModelFile, 
                string OutputXmlFile, string FindingsFile, bool SendToCalypso)
        {
            Exception exsave = null;
            // Test reading of the SpecimenObervationOrder message from an XML file:
            Calypso.Global.Settings.ReadSettingsAppConfig();
            string filename = Calypso.Global.Settings.ExampleFile(MessageType.SpecimenObservationOrder);
            if (!string.IsNullOrEmpty(MessageFilename))
                filename = MessageFilename;
            // @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationOrder.xml";

            if (!File.Exists(filename))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Message file does not exist: \"" + filename + "\"");
                Console.Write("Insert a new file name: ");
                filename = Console.ReadLine();
                Console.WriteLine();
            }
            MsgObservationEvent msg = new MsgObservationEvent();
            msg.Load(filename);
            try
            {
            msg.Read();
            }
            catch (Exception ex)
            {
                exsave = ex;
                R.ReportError(ex);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Contents of the message that was read from the file:");
            Console.WriteLine("==================================================================");
            msg.WriteToConsole();
            Console.WriteLine("==================================================================");
            Console.WriteLine();
            Console.WriteLine();
            if (exsave != null)
                R.ReportError(exsave);


            // Create XML from the contents of the MsgObservationEvent object:
            // findingsData.XmlCreateInputFormat = CreateXmlInputFormat;  // whether input format is used in creation of XML from data
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Writing contents of ObservationOrder to an XML document...");

            try
            {
                if (!string.IsNullOrEmpty(ModelFile))
                    msg.SetModelFile(ModelFile);

                if (!string.IsNullOrEmpty(FindingsFile))
                    msg.LoadFindings(FindingsFile);

                msg.SetSenderLabex();
                msg.ToXml();

                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine("Contents of the XML file generated from the findings object: ");
                //Console.WriteLine();
                //Console.WriteLine(msg.Doc.OuterXml);
                //Console.WriteLine();
                //Console.WriteLine();
            }

            catch (Exception ex)
            {
                exsave = ex;
                R.ReportError(ex);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            // Write contents of the created XML to a file:
            if (string.IsNullOrEmpty(OutputXmlFile))
                Console.WriteLine("Output file is not specified.");
            else
            {
                Console.WriteLine("Saving object to an XML file.");
                Console.WriteLine("Output XML file: " + OutputXmlFile);
                msg.Save(OutputXmlFile);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (exsave != null)
                R.ReportError(exsave);

            if (SendToCalypso)
            {

                // Initializing communication object for Calypso:
                Calypso.Global.Settings.ReadSettingsAppConfig();
                Calypso calypso = Calypso.Global;
                Console.WriteLine("Communication object settings: " + Environment.NewLine);
                Console.WriteLine(calypso.Settings.ToString());

                string Id = null;
                bool awake = true;
                // awake = calypso.TestWs(out Id);
                if (awake)
                    Console.WriteLine("Calypso is awake, unique Id received: " + Id);
                else
                {
                    Console.WriteLine("Calypso web service is not ready.");
                    return;
                }
                Console.WriteLine();


                // Sendong of the message to Calypso server:
                // Send the message to the Labex queue on calypso:
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Sending the message to BIS queue on Calypso...");
                msg.SetSenderLabex();


                calypso.SendMessageToBis(msg.Doc.OuterXml,msg.MessageNumber);


                Console.WriteLine("Sending done.");
                Console.WriteLine();
                Console.WriteLine();

                // Console.Readline();

            }

        }  // TestMsgObservationEvent(...)



        #endregion  // SpecimenOservationEvent




        #region DetailedFinancialTransaction


        /// <summary>Reads financial transaction message from the specified XML file and outputs the contents.
        /// File must contian data according to specification of DetailedFinancialTransaction.
        /// DOES NOT convert the object back to XML!!!</summary>
        /// <param name="MessageFile">Name of the file containing the message.</param>
        static void TestMsgFinancialTransaction(string MessageFile)
        {
            TestMsgFinancialTransaction(MessageFile, null, null);
        }

        /// <summary>Test without sending to Calypso.</summary>
        static void TestMsgFinancialTransaction(string MessageFilename, string ModelFile, string OutputXmlFile)
        {
            TestMsgFinancialTransaction(MessageFilename, ModelFile, OutputXmlFile, false /* SendToCalypso */);
        }


        /// <summary>Test of a message read from the specified file and reads its data.
        /// Message contained in the file must be of type DetailedFinancialTransaction.</summary>
        /// <param name="MessageFilename">Name of the file containing the message. If not specified
        /// then name is obtained from application settings.</param>
        /// <param name="ModelFile">Name of the file that is used as a model for message construction.
        /// Its specification is obligatory when object is converted to XML.</param>
        /// <param name="OutputXmlFile">Name of the file to which XML representation of the object
        /// is written after object contents is read (interpreted).</param>
        static void TestMsgFinancialTransaction(string MessageFilename, string ModelFile,
                string OutputXmlFile, bool SendToCalypso)
        {
            Exception exsave = null;
            // Test reading of the SpecimenObervationOrder message from an XML file:
            Calypso.Global.Settings.ReadSettingsAppConfig();
            string filename = Calypso.Global.Settings.ExampleFile(MessageType.DetailedFinancialTransaction);
            if (!string.IsNullOrEmpty(MessageFilename))
                filename = MessageFilename;

            if (!File.Exists(filename))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Message file does not exist: \"" + filename + "\"");
                Console.Write("Insert a new file name: ");
                filename = Console.ReadLine();
                Console.WriteLine();
            }
            MsgFinancialTransaction msg = new MsgFinancialTransaction();
            msg.Load(filename);
            try
            {
                msg.Read();
            }
            catch (Exception ex)
            {
                exsave = ex;
                R.ReportError(ex);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Contents of the message that was read from the file:");
            Console.WriteLine("==================================================================");
            msg.WriteToConsole();
            Console.WriteLine("==================================================================");
            Console.WriteLine();
            Console.WriteLine();
            if (exsave != null)
                R.ReportError(exsave);


            // Create XML from the contents of the MsgFinancialTransaction object:
            // findingsData.XmlCreateInputFormat = CreateXmlInputFormat;  // whether input format is used in creation of XML from data
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Writing contents of FinantialTransaction to an XML document...");

            try
            {
                if (!string.IsNullOrEmpty(ModelFile))
                    msg.SetModelFile(ModelFile);

                msg.SetSenderLabex();
                msg.ToXml();

                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine("Contents of the XML file generated from the findings object: ");
                //Console.WriteLine();
                //Console.WriteLine(msg.Doc.OuterXml);
                //Console.WriteLine();
                //Console.WriteLine();
            }

            catch (Exception ex)
            {
                exsave = ex;
                R.ReportError(ex);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            // Write contents of the created XML to a file:
            if (string.IsNullOrEmpty(OutputXmlFile))
                Console.WriteLine("Output file is not specified.");
            else
            {
                Console.WriteLine("Saving object to an XML file.");
                Console.WriteLine("Output XML file: " + OutputXmlFile);
                msg.Save(OutputXmlFile);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (exsave != null)
                R.ReportError(exsave);

            if (SendToCalypso)
            {

                // Initializing communication object for Calypso:
                Calypso.Global.Settings.ReadSettingsAppConfig();
                Calypso calypso = Calypso.Global;
                Console.WriteLine("Communication object settings: " + Environment.NewLine);
                Console.WriteLine(calypso.Settings.ToString());

                string Id = null;
                bool awake = true;
                // awake = calypso.TestWs(out Id);
                if (awake)
                    Console.WriteLine("Calypso is awake, unique Id received: " + Id);
                else
                {
                    Console.WriteLine("Calypso web service is not ready.");
                    return;
                }
                Console.WriteLine();

                // Sendong of the message to Calypso server:
                // Send the message to the Labex queue on calypso:
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Sending the message to BIS queue on Calypso...");
                msg.SetSenderLabex();

                calypso.SendMessageToBis(msg.Doc.OuterXml, msg.MessageNumber);

                Console.WriteLine("Sending done.");
                Console.WriteLine();
                Console.WriteLine();

                // Console.Readline();

            }


        }  // TestMsgFinancialTransaction(...)


        #endregion  // DetailedFinancialTransaction


        #region Findings


        /// <summary>Reads findings data from the specified XML file and outputs the contents.
        /// File must contian data according to specification of Findings.
        /// DOES NOT convert the object back to XML!!!</summary>
        /// <param name="MessageFile">Name of the file containing the message.</param>
        static void TestFindings(string FindingsFile)
        {
            TestFindings(FindingsFile, null, false);
        }

        /// <summary>Reads findings data from the specified XML file and outputs the contents,
        /// then writes data back to XML.
        /// File must contian data according to specification of Findings.</summary>
        /// <param name="MessageFile">Name of the file containing the message.</param>
        /// <param name="OutputXmlFile">Name of the file to wghich Xml is saved.</param>
        /// <param name="CreateXmlInputFormat">If true then XML is created in iput format when created from data.</param>
        static void TestFindings(string FindingsFile, string OutputXmlFile, bool CreateXmlInputFormat)
        {
            Exception exsave = null;
            // Test reading of Findings from an XML file:
            if (string.IsNullOrEmpty(FindingsFile))
                throw new ArgumentNullException("File containing the fingdings data is not specified (null argument).");
            if (!File.Exists(FindingsFile))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Findings file does not exist: \"" + FindingsFile + "\"");
                Console.Write("Insert a new file name: ");
                FindingsFile = Console.ReadLine();
                Console.WriteLine();
            }
            DocFindings findingsData = new DocFindings();
            findingsData.Load(FindingsFile);
            // Test parsing the contents of the read-in XML file:
            try
            {
                findingsData.Read();
            }
            catch (Exception ex)
            {
                exsave = ex;
                R.ReportError(ex);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Contents of the findings data that was read from the file:");
            Console.WriteLine("==================================================================");
            findingsData.WriteToConsole();
            Console.WriteLine("==================================================================");
            Console.WriteLine();
            Console.WriteLine();
            if (exsave != null)
                R.ReportError(exsave);


            // Create XML from the contents of the Findings object:
            findingsData.XmlCreateInputFormat = CreateXmlInputFormat;  // whether input format is used in creation of XML from data
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Writing contents of findings to an XML document...");

            try
            {
                findingsData.ToXml();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Contents of the XML file generated from the findings object: ");
                Console.WriteLine();
                Console.WriteLine(findingsData.Doc.OuterXml);
                Console.WriteLine();
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                exsave = ex;
                R.ReportError(ex);
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            // Write contents of the created XML to a file:
            if (string.IsNullOrEmpty(OutputXmlFile))
                Console.WriteLine("Output file is not specified.");
            else
            {
                Console.WriteLine("Saving object to an XML file.");
                Console.WriteLine("Output XL file: " + OutputXmlFile);
                findingsData.Save(OutputXmlFile);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            if (exsave != null)
                R.ReportError(exsave);

        }  // TestFindings(...)



        #endregion  // Findings



        #region SpecimenObservationOrder

        /// <summary>Testing of receiving SpecimenObservationOrder:</summary>
        /// <param name="TakeFromQueue">Indicates whether the received message should be taken 
        /// from the queue if everything is OK.</param>
        static void ReceiveSpecimenObservationOrder(bool TakeFromQueue)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine 
                + "Testing messages of typee MsgObservationOrder: " + Environment.NewLine);
            Console.WriteLine("Reading communication settings...");
            Calypso.Global.Settings.ReadSettingsAppConfig();
            Calypso calypso = Calypso.Global;
            Console.WriteLine("Communication object settings: " + Environment.NewLine);
            Console.WriteLine(calypso.Settings.ToString());

            // Receive the last message from the queue:
            Console.WriteLine("Getting the last message...");
            MsgBase msg;
            msg = calypso.GetMessage();
            if (msg == null)
                Console.WriteLine(Environment.NewLine + "Timout occurred!" + Environment.NewLine);
            if (msg != null)
            {
                Console.WriteLine("Message received from the Calypso server.");
                Console.WriteLine("Message type: ", msg.Type.ToString());
                if (msg.Type == MessageType.SpecimenObservationOrder)
                {
                    MsgObervationOrder msgspecific = msg as MsgObervationOrder;
                    if (msgspecific == null)
                    {
                        Console.WriteLine(Environment.NewLine + "ERROR: casting of message to its specific type did not succeed."
                            + Environment.NewLine + Environment.NewLine);
                    }
                    // Message of type SpecimenObservationOrder has been received, 
                    // process the message:
                    msg.LoadXml(msg.MessageXml);
                    try
                    {
                        msg.Read();
                    }
                    catch (Exception ex)
                    {
                        R.ReportError(ex, "Msg.Read");
                        //Console.WriteLine();
                        //Console.WriteLine("Error in reading data: " + ex.Message);
                        //Console.WriteLine();
                    }
                    Console.WriteLine(Environment.NewLine + "Message contents: " + Environment.NewLine);
                    msg.WriteToConsole();

                    Console.WriteLine();
                    Console.WriteLine("Message ID: " + msg.MessageId + ", type: " + msg.Type.ToString());
                    Console.WriteLine("BIS order ID: " + msgspecific.GetBisOrderId());
                    Console.WriteLine();

                    Console.WriteLine("Saving message to a file...");
                    calypso.SaveIncomingMessage(msg, false  /* addGuid */ );
                    Console.WriteLine("Saving done.");



                    if (TakeFromQueue)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Message receipt will be confirmed and the message will be taken off the queue.");
                        Console.WriteLine();

                        Console.WriteLine("Confirming receipt...");
                        calypso.ConfirmMessageReceipt(msg.MessageId);
                        Console.WriteLine("Confirmation done.");
                    }
                } else
                {
                    Console.WriteLine(Environment.NewLine + "Message type is not SpecimenObservationOrder but " 
                        + msg.Type.ToString() + "." +  Environment.NewLine);
                }
            }

        }

        // **************************************************
        // 


        /// <summary>Test of a message read from the specified file and reads its data.
        /// Message contained in the file must be of type SpecimenObservationOrder</summary>
        /// <param name="MessageFilename">Name of the file containing the message. If not specified
        /// then name is obtained from application settings.</param>
        static void TestMsgObservationOrder(string MessageFilename)
        {

            // Test reading of the SpecimenObervationOrder message from an XML file:
            Calypso.Global.Settings.ReadSettingsAppConfig();
            string filename = Calypso.Global.Settings.ExampleFile(MessageType.SpecimenObservationOrder);
            if (!string.IsNullOrEmpty(MessageFilename))
                filename = MessageFilename;
                // @"c:\DevProjects\LABEX\LabexSolution\00_Utilities\LabexBis\schemas\SpecimenObservationOrder.xml";

            if (!File.Exists(filename))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Message file does not exist: \"" +  filename + "\"");
                Console.Write("Insert a new file name: ");
                filename = Console.ReadLine();
                Console.WriteLine();
            }
            MsgObervationOrder msg = new MsgObervationOrder();
            msg.Load(filename);
            msg.Read();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Contents of the message that was read from the file:");
            Console.WriteLine("==================================================================");
            msg.WriteToConsole();
            Console.WriteLine("==================================================================");
            Console.WriteLine();
            Console.WriteLine();
        }



        /// <summary>Test of a message read from the specified file and reads its data.
        /// Message contained in the file must be of type SpecimenObservationOrder</summary>
        /// <param name="MessageFilename">Name of the file containing the message.</param>
        /// <param name="OutputXmlFile">Name of the file containing the message. If not specified
        /// then name is obtained from application settings.</param>
        static void TestMsgObservationOrderNullify(string MessageFilename, string OutputXmlFile)
        {

            // Test reading of the SpecimenObervationOrder message from an XML file:
            Calypso.Global.Settings.ReadSettingsAppConfig();
            string filename = MessageFilename;
            if (!string.IsNullOrEmpty(MessageFilename))
                filename = Calypso.Global.Settings.ExampleFile(MessageType.SpecimenObservationOrder);

            if (!File.Exists(filename))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Message file does not exist: \"" + filename + "\"");
                Console.Write("Insert a new file name: ");
                filename = Console.ReadLine();
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Reading observation order from a file...");
            Console.WriteLine("  File name: " + filename);
            Console.WriteLine();
            MsgObervationOrder msg = new MsgObervationOrder();
            msg.Load(filename);
            Console.WriteLine();
            Console.WriteLine("Loading file contents done, parsing...");
            msg.Read();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Contents of the message that was read from the file:");
            Console.WriteLine("==================================================================");
            msg.WriteToConsole();
            Console.WriteLine("==================================================================");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Preparing observation order message to nullify response...");
            XmlDocument doc = new XmlDocument(); //  = msg.Doc;
            string MessageString = msg.Doc.OuterXml;
            MsgObervationOrder.PrepareOoNullify(ref MessageString, "This is an automatic nullify answer to the observation order.");
            doc.LoadXml(MessageString);
            Console.WriteLine("Message prepared.");
            Console.WriteLine();
            Console.WriteLine("Saving the message...");

            if (string.IsNullOrEmpty(OutputXmlFile))
                throw new Exception("Output file to store the prepared message to is not specified.");
            string dir = Path.GetDirectoryName(OutputXmlFile);
            if (!Directory.Exists(dir))
                throw new Exception("Directory of the output file does not exist.");
            doc.Save(OutputXmlFile);
            Console.WriteLine("Message saved to " + OutputXmlFile + ".");
        }  // TestMsgObservationOrderNullify





        /// <summary>Sends the nullification request for the specified Observation order message.</summary>
        /// <param name="MessageFilename">Name of the file that contains the message.</param>
        /// <param name="MessageId">Id of the message.</param>
        static void TestMsgObservationOrderNullifySend(string MessageFilename, string MessageId)
        {
            try
            {


                Console.WriteLine();
                Console.WriteLine("Test of sending nullification request for the specified message... ");
                Console.WriteLine("Message file: " + MessageFilename);
                Console.WriteLine("Message Id: " + MessageId);
                Console.WriteLine();

                // Load the message from an XML file:
                string filename = MessageFilename;

                if (!File.Exists(filename))
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Message file does not exist: \"" + filename + "\"");
                    Console.Write("Insert a new file name: ");
                    filename = Console.ReadLine();
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("Reading observation order from a file...");
                Console.WriteLine("  File name: " + filename);
                Console.WriteLine();
                MsgObervationOrder msg = new MsgObervationOrder();
                msg.Load(filename);
                Console.WriteLine("Message has been loaded.");
                Console.WriteLine();

                Calypso.Global.Settings.ReadSettingsAppConfig();
                Calypso calypso = Calypso.Global;

                //Console.WriteLine("Communication object settings: " + Environment.NewLine);
                //Console.WriteLine(calypso.Settings.ToString());

                // Set the specified message Id before sending hte nullification request:
                msg.MessageId = MessageId;

                Console.WriteLine();
                Console.WriteLine("Sending nullification request to Calypso... ");
                // send the nullification request:
                calypso.SendOoNullifyToBis(msg, "To je tetna zavrnitev naročila. " + Environment.NewLine
                + "Poslana ni bila iz Labexa, ampak iz testnega programa. " + Environment.NewLine
                + "Id sporočila (za test): " + msg.MessageId );

                Console.WriteLine("Nullification request sent.");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                R.ReportError("Error occurred when executing Calypso WS methods: ", ex);
                Console.WriteLine("Error occurred when executing Calypso WS methods: " + ex.Message);
            }
        }  // TestMsgObservationOrderNullifySend()




        #endregion // SpecimenObservationOrder




        #region WSOperation


        /// <summary>Simple test of web service, just receives an Id and a message.</summary>
        static void TestcalypsoWsSimple()
        {
            try
            {
                Calypso.Global.Settings.ReadSettingsAppConfig();
                Calypso calypso = Calypso.Global;
                Console.WriteLine("Communication object settings: " + Environment.NewLine);
                Console.WriteLine(calypso.Settings.ToString());
                Console.WriteLine("Test of Calypso web service: ");
                for (int i = 1; i <= 1; ++i)
                {
                    string id = Calypso.Global.GetUniqueID();
                    Console.WriteLine("New unique ID form Calypso: \"" + id + "\".");
                    Console.WriteLine();
                }
                Console.WriteLine();
                string Id, Message;
                Calypso.Global.GetMessage(out Id, out Message);
                Console.WriteLine("a message has been received from Calypso.");
                Console.WriteLine("Message ID: " + Id);
                Console.WriteLine("File to store the message: " + Calypso.Global.Settings.IncomingFileName(Id));
                Console.WriteLine("Message: " + Environment.NewLine + "  " + Message);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                R.ReportError("Error occurred when executing Calypso WS methods: ", ex);
                Console.WriteLine("Error occurred when executing Calypso WS methods: " + ex.Message);
            }
        }


        /// <summary>Gets the specific test from the Calypso web service.</summary>
        static void TestCalypsoWs()
        {
            try
            {
                Calypso.Global.Settings.ReadSettingsAppConfig();
                Calypso calypso = Calypso.Global;
                Console.WriteLine("Communication object settings: " + Environment.NewLine);
                Console.WriteLine(calypso.Settings.ToString());

                string Id = null;
                bool awake = calypso.TestWs(out Id);
                if (awake)
                    Console.WriteLine("Calypso is awake, unique Id received: " + Id);
                else
                {
                    Console.WriteLine("Calypso web service is not ready.");
                    return;
                }
                Console.WriteLine();
                // Send a message from a file to a queue:
                Console.WriteLine("Test: sending message from the file to Labex queue:");
                string filename = calypso.Settings.MessageFile;
                Console.WriteLine("Message file: \"" + filename + "\"");
                Console.WriteLine("Reading message contents from the a file...");
                string message = null;
                using (TextReader tr = new StreamReader(filename))
                {
                    message = tr.ReadToEnd();
                }
                //Console.WriteLine("Message content:");
                //Console.WriteLine(message);
                // Check the contents of the message read from the file:

                Console.WriteLine("Parsing the message: ");
                Console.WriteLine("Loading the message from a string: ");
                Console.WriteLine("");
                MsgObervationOrder msg = new MsgObervationOrder();
                msg.LoadXml(message);
                msg.Read();
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Message data (parsed): " + Environment.NewLine);
                msg.WriteToConsole();
                Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine);
                // Receive the last message from the queue:
                MsgBase msgr;
                msgr = calypso.GetMessage();
                if (msgr != null)
                {
                    Console.WriteLine("Message received from the Calypso server.");
                    Console.WriteLine("Message type: ", msgr.Type.ToString());
                }
                // Send the message to the Labex queue on calypso:
                Console.WriteLine("Sending the message to Labex queue on Calypso...");
                calypso.SendMessageToMyself(message,"Id_Izmisljen");
                Console.WriteLine("Sending done.");
                Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Read the message from the Calypso queue...");
            }
            catch (Exception ex)
            {
                R.ReportError("Error occurred when executing Calypso WS methods: ", ex);
                Console.WriteLine("Error occurred when executing Calypso WS methods: " + ex.Message);
            }
        }


        #endregion  // WSOperation



        #region OtherTests


        /// <summary>This tests a simple web service from W3C in order to check the outwards connection.</summary>
        static void TestW3cWs()
        {
            IGTest.TestWebReference.TempConvert service = new IGTest.TestWebReference.TempConvert();
            double celsius, fahrenheit;
            string str;
            celsius = 0.0;
            fahrenheit = double.Parse(service.CelsiusToFahrenheit(celsius.ToString()));
            str = service.Url;
            Console.WriteLine(celsius.ToString() + " degrees celsius equals " + fahrenheit.ToString() + " degrees fahrenheit.");
            celsius = 10.0;
            fahrenheit = double.Parse(service.CelsiusToFahrenheit(celsius.ToString()));
            Console.WriteLine(celsius.ToString() + " degrees celsius equals " + fahrenheit.ToString() + " degrees fahrenheit.");
            celsius = 20.0;
            fahrenheit = double.Parse(service.CelsiusToFahrenheit(celsius.ToString()));
            Console.WriteLine(celsius.ToString() + " degrees celsius equals " + fahrenheit.ToString() + " degrees fahrenheit.");
            celsius = 100.0;
            fahrenheit = double.Parse(service.CelsiusToFahrenheit(celsius.ToString()));
            Console.WriteLine(celsius.ToString() + " degrees celsius equals " + fahrenheit.ToString() + " degrees fahrenheit.");
        }



        /// <summary>Tests conversion form Arabic to Roman numbers and conversely.</summary>
        static void TestNumberConverter()
        {
            int goon = 1;
            while (goon != 0)
            {

                Console.WriteLine(); Console.WriteLine();
                Console.Write("Insert an integer: ");
                int num = 55;
                int.TryParse(Console.ReadLine(), out num);

                string roman = ConverterBase.Roman.ToString(num);
                int num1 = ConverterBase.Roman.ToInt(roman);

                Console.WriteLine(); Console.WriteLine();
                Console.WriteLine("Number:                     " + num.ToString());
                Console.WriteLine();
                Console.WriteLine("Corresponding Roman number: " + roman);
                Console.WriteLine();
                Console.WriteLine("Converted back:             " + num1.ToString());
                if (num != num1)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: backward converted number (" + num1.ToString()
                        + ") is not the same as the original(" + num.ToString() + ").");
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Continue (0/1)? ");
                if (!int.TryParse(Console.ReadLine(), out goon))
                    goon = 1;

            }
        }


        #endregion  // OtherTests


    }
}

