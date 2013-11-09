using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Diagnostics;

using Premisa.PadoInterfaces;
using Premisa.PadoUtilities;
using Premisa.PadoUtilities.PadoAttributes;
using Premisa.PadoBaseClasses;

using EFA_Utilities;


namespace EFA_WS
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://www.posta.si/EFA")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service : System.Web.Services.WebService
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        public Service()
        {
            ReadSettings();
            //TODO: Remove these two lines and connect this web service to remote trace listener!
            Trace.Listeners.Add(new System.Diagnostics.EventLogTraceListener("Application"));
            PadoVariables.PadoTraceSwitch.Level = TraceLevel.Verbose;

            exEvent += new ExceptionEventDelegate(Service_exEvent);
            // Remark: Service's constructor is executed each time the service gets a request.
            // Check whether debugmode is configured:
            //if (this.DebugMode)
            //    System.Windows.Forms.MessageBox.Show("EFA WS: \n\nThe service is started in debug mode.\n\nSome errors will be reported in messageboxes.\n");
        }

        public bool GetAppSettingFlag(string flagname)
        // Returns true if the variable named flag name is defined and set to either "true" 
        // (case insensitive) or non-zero integer.
        {
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.GetAppSettingFlag", 
                    "Started, flag name = \"" + flagname + "\"...",
                    PadoEnums.enTraceMsgSource.Server));
                string str;
                if ((str = System.Configuration.ConfigurationManager.AppSettings.Get(flagname)) != null) if (str.Length > 0)
                    {
                        int num = 0;
                        bool parsed = true;
                        try { num = int.Parse(str); }
                        catch (Exception) { parsed = false; }
                        if (str.ToLower() == "true" || (parsed && num != 0))
                            return true;
                    }
            }
            catch (Exception ex) 
            { 
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.GetAppSettingFlag", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.GetAppSettingFlag", "Finished.", PadoEnums.enTraceMsgSource.Server));
            }
            return false;
        }


        // Implementation of the DebugMode property 
        // (for example, in debug mode errors are directly reported by launching a message box)
        private bool flag_debug=false, queried_debug = false;
        public bool DebugMode
        {
            get 
            {
                if (!queried_debug)
                {
                    queried_debug = true;
                    flag_debug = GetAppSettingFlag("debugmode");
                }
                return flag_debug;
            }
            set { queried_debug = true; flag_debug = value; }
        }

        // Implementation of the RemoteMode property 
        private bool flag_remote = false, queried_remote = false;
        public bool RemoteMode
        {
            get
            {
                if (!queried_remote)
                {
                    queried_remote = true;
                    flag_remote = GetAppSettingFlag("remotemode");
                }
                return flag_remote;
            }
            set { queried_remote = true; flag_remote = value; }
        }


        public void ReportError(Exception ex)
        // Reports an error given exception ex; since this message box is written for services, 
        // a message box is launched only in debug mode.
        {
            string errstr = "",functionname=null,filename=null;
            int line = -1, column = -1;
            System.Diagnostics.StackTrace trace = null;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.ReportError", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                try
                {
                    // Extract info about error location:
                    trace = new System.Diagnostics.StackTrace(ex, true);
                    functionname = trace.GetFrame(0).GetMethod().Name;
                    filename = trace.GetFrame(0).GetFileName();
                    line = trace.GetFrame(0).GetFileLineNumber();
                    column = trace.GetFrame(0).GetFileColumnNumber();
                }
                catch (Exception ex1) 
                { 
                    if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Error, "EFA_WS.Service.ReportError", "Error: " + ex1.Message,
                        PadoEnums.enTraceMsgSource.Server));
                }
                if (this.DebugMode)
                {
                    errstr += "EFA WS - ERROR in " + functionname +
                        "\n  < " + filename +
                        " (" + line.ToString() +
                        ", " + column.ToString() +
                        ") >: \n\n";
                    errstr += ex.Message;

                   Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                   EventLogEntryType.Error, "EFA_WS.Service.ReportError", "Error: " + errstr,
                   PadoEnums.enTraceMsgSource.Server));

                    //System.Windows.Forms.MessageBox.Show(errstr); No message boxes allowed in server, Web Service!
                }
            }
            catch (Exception ex2) 
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.ReportError", "Error: " + ex2.Message,
                    PadoEnums.enTraceMsgSource.Server));
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.ReportError", "Finished.", PadoEnums.enTraceMsgSource.Server));
            }
        }

        #region Posiljanje_Paketa



        /// <summary>Class that is used to pass data to the InsertPaketXml() method that is launched in a parallel thread.
        /// Thread is launched just because for SQLXML the thread must be STA, and thread launched by WS is not of this kind.</summary>
        private class InsertPaketData
        {
            public int PaketId = 0 ;
            public string PaketFileName ;
        }

        public string PosljiPaket_Base(string paketXml)
        // Base function for web methods PosljiPaket and PosljiPaketZip. The latter just unzips the received byte array, converts it
        // to a string and calls this function. The first one calls this function directly.
        {
            if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.PosljiPaket_Base", "Before extracting data from XML document ...",
                PadoEnums.enTraceMsgSource.Server));
            // Extract packet data that is stored on the XML:
            string result = "<Napaka/>", exmsg = "", extrace="";
            result = 
"<?xml version=\"1.0\" encoding=\"UTF-16\"?>\r\n" +
"<EfaPaketStatus>\r\n" +
"    <EfaPaket>\r\n" +
"        <Id>-1</Id>\r\n" +
"	    <PaketStatusId>1</PaketStatusId>\r\n" +
"	    <PaketStatusOpis>Zavrnjen</PaketStatusOpis>\r\n" +
"       <NapakaSporocilo>{0}</NapakaSporocilo>\r\n" +
"    </EfaPaket>\r\n" +
"    <SeznamNapak>Pošiljanje spodletelo, podatki se niso zapisali.</SeznamNapak>\r\n" +
"</EfaPaketStatus>\r\n";
            // Exception exlast = null;
            int podrocjeId = -1;
            byte paketTipId = 0; 
            DateTime paketDatum = new DateTime(2000,1,1);
            int stZapisov = -1;
            decimal vsotaDokumentDatum = 0.0M;
            decimal vsotaCenaNeto = 0.0M;
            XmlDocument DocPaket = null;
            bool WritePacket = true, PacketWritten = false, InsertPacketData = false, 
                PacketDataInserted = false, ErrorsInserted = false;
            string XmlFileName = null;

            if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.PosljiPaket_Base", "Before object locking ...",
                PadoEnums.enTraceMsgSource.Server));
            lock (syncObject)
            {
                //decimal paketId = -1; 
                //string ret = string.Empty;

                try
                {
                    if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Information, "EFA_WS.Service.PosljiPaket_Base", "Started ...",
                        PadoEnums.enTraceMsgSource.Server));
                    //TODO: Wrap exception handler around InsertIntoEfaPaket, SaveXmlToDisk, InsertPaketXml,
                    //      UpdateGlavaPostavke, GetPaketStatusXml and XmlValidation to catch specific exception,
                    //      which messages will be inserted into EfaPaketNapake.  (?? - ta tabela ne obstaja. Morda EfaPaketPovratnice?)

                    //TODO: Move ReadSettings to constructor???
                    //      Move connectionstrings to <ConnectionStrings> in web.config
                    //      Default values for settings
                    //ReadSettings();

                    //TODO: synchronize for multithreading DONE

                    //TODO: bulkcopy from memory stream

                    try
                    {
                        DocPaket = new XmlDocument();
                        DocPaket.LoadXml(paketXml);
                    }
                    catch (Exception ex)
                    {
                        WritePacket = false;
                        if (string.IsNullOrEmpty(extrace))
                            extrace = ex.StackTrace;
                        if (!string.IsNullOrEmpty(exmsg))
                        {
                            exmsg += Environment.NewLine + "  *** ";
                        }
                        exmsg += "Napaka pri ekstrakciji XML dokumenta iz paketa: " + ex.Message;
                    }
                    try
                    {
                        EFA_Functions.ExtractPaketData(DocPaket, out podrocjeId, out paketTipId, out paketDatum,
                                out stZapisov, out vsotaDokumentDatum, out vsotaCenaNeto);

                        InsertPacketData = true; // Če pri ekstrakciji podatkov pride do napake, se ne sme insertirati podatkov o paketu.
                                    // To se bo zgodilo tudi v primeru neveljavnega XML dokumenta.

                        // Opomba: kontrolo vrednosti polja StZapisov tu izpustimo, ker se to tako preveri v kontrolah.
                        //if (stZapisov < 1)
                        //    throw new Exception("Ni zapisov - v paketu je deklarirano število zapisov enako " 
                        //        + stZapisov.ToString());
                    }
                    catch (Exception ex)
                    {
                        WritePacket = false;
                        if (string.IsNullOrEmpty(extrace))
                            extrace = ex.StackTrace;
                        if (!string.IsNullOrEmpty(exmsg))
                        {
                            exmsg += Environment.NewLine + "  *** ";
                        }
                        exmsg += "Napaka pri ekstrakciji splošnih podatkov iz XML dokumenta: " + Environment.NewLine + ex.Message;
                    }
                    try
                    {
                        if (InsertPacketData)
                        {
                            Connect();
                            trans = cnn.BeginTransaction();
                            paketId = InsertIntoEfaPaket(podrocjeId, paketTipId, paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto, 10);
                            trans.Commit();//To je pravilno: podatke o paketu shranimo, tudi če kasneje ne moremo vnesti glav in postavk --> v takem primeru k paketu zapišemo sporočilo o napaki!
                            PacketDataInserted = true;
                            XmlFileName = GetXmlFileName(paketId);
                        }
                    }
                    catch (Exception ex)
                    {
                        WritePacket = false;
                        trans.Rollback();  // Transaction is rolled back if there was an error.
                        if (string.IsNullOrEmpty(extrace))
                            extrace = ex.StackTrace;
                        if (!string.IsNullOrEmpty(exmsg))
                        {
                            exmsg += Environment.NewLine + "  *** ";
                        }
                        exmsg += "Napaka pri vnosu podatkov o paketu v bazo: " + ex.Message;
                    }


                    //TODO: enums for paketStatus, paketTip ...


                    try
                    {
                        if (XmlFileName != null && paketId != -1)
                        {
                            SaveXmlToDisk(paketXml, paketId, XmlFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(extrace))
                            extrace = ex.StackTrace;
                        if (!string.IsNullOrEmpty(exmsg))
                        {
                            exmsg += Environment.NewLine + "  *** ";
                        }
                        exmsg += "Napaka pri zapisovanju paketa na disk: " + ex.Message;
                    }

                    //trans.Commit();//TODO: treba se je zmeniti kako glede tega!!!

                    //TODO: XmlValidation goes here

                    if (WritePacket && XmlFileName != null && paketId != -1)
                    {
                        try
                        {
                            trans = cnn.BeginTransaction();
                            // Run InsertPaketXml() in a separate thread to insert the packet:
                            // We call the InsertPaketXml() method in a new theread EXCLUSIVELY because of the BulkLoad tool, which requires a 
                            // single thread appartment. Since the thread started when a web method is called is not an STA thread, we need
                            // execute the method in another thread that we can explicitly set to STA.
                            InsertPaketData data = new InsertPaketData();
                            data.PaketId = paketId;  // a new class has been introduced to pass information to InsertPaketXml
                            data.PaketFileName = XmlFileName;
                            System.Threading.Thread staThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InsertPaketXml));
                            staThread.SetApartmentState(System.Threading.ApartmentState.STA);
                            staThread.Start(data);
                            staThread.Join();

                            try
                            {
                                UpdateGlavaPostavke(paketId);
                            }
                            catch (Exception ex1)
                            {
                                throw new Exception("Napaka pri postavljanju PaketId v bazi: " + ex1.Message);
                            }
                            trans.Commit();
                            PacketWritten = true;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();  // We must not commit transaction if something went wrong.
                            if (string.IsNullOrEmpty(extrace))
                                extrace = ex.StackTrace;
                            if (!string.IsNullOrEmpty(exmsg))
                            {
                                exmsg += Environment.NewLine + "  *** ";
                            }
                            exmsg += "Napaka pri zapisovanju podatkov iz paketa v bazo: " + ex.Message;
                        }

                        try
                        {
                            // If the Packet data has been inserted, but importion of data from XML to database failed, we have to
                            // update the status in the EfaPaket table to rejected!
                            if (PacketDataInserted && !PacketWritten)
                                SetPaketStatusZavrnjen(paketId);
                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(exmsg))
                            {
                                exmsg += Environment.NewLine + "  *** ";
                            }
                            exmsg += "Napaka: ne morem postaviti statusa Paketa št. " + paketId.ToString()
                                + " na Zavrnjen. Podrobnosti: " + ex.Message;
                        }
                        try
                        {
                            if (PacketDataInserted)
                            {
                                string result1 = GetPaketStatusXml(paketId, null, exmsg);  // creates XML that contains status; 
                                //this is NOT EXECUTED here if
                                // exception is thrown in InsertPaketXml() (method called in a thread launched above). This is because 
                                // exception thrown in InsertPaketXml() causes execution of an event handler in this thread, and that 
                                // handler throws an exception in this thread. Such arrangement is correct, therefore do not move this
                                // into finally{} block!!!  -- ?? - TODO: Vprašati, če je res prav, da se to ne izvede v primeru napake (zdaj ni tako in SE IZVEDE)
                                if (!string.IsNullOrEmpty(result1))
                                {
                                    result = result1;
                                    ErrorsInserted = true;  // Because GetPaketStatusXml inserts reports about errors.
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (string.IsNullOrEmpty(extrace))
                                extrace = ex.StackTrace;
                            if (!string.IsNullOrEmpty(exmsg))
                            {
                                exmsg += Environment.NewLine + "  ** Naslednja napaka: ";
                            }
                            exmsg += "Napaka pri pridobivanju XML statusa paketa: " + ex.Message;

                        }

                    }
                    else
                    {
                        try
                        {
                            exmsg += Environment.NewLine + "  *** Paket ni bil zapisan! ";
                        }
                        catch { }
                    }


                    try
                    {
                        result = string.Format(result, exmsg); // we don't insert extrace to result --> security reason!!! --> result is returned to client of this web service!
                        InsertIntoEfaPaketPovratnica(paketId, result, exmsg, extrace);   // inserts into the database what was sent back to web method
                    }
                    catch (Exception ex)
                    {
                        if (string.IsNullOrEmpty(extrace))
                            extrace = ex.StackTrace;
                        if (!string.IsNullOrEmpty(exmsg))
                        {
                            exmsg += Environment.NewLine + "  ** Naslednja napaka: ";
                        }
                        exmsg +="Napaka pri zapisovanju povratnice v bazo: " + ex.Message;
                        if (ErrorsInserted)
                        {
                            try
                            {
                                // Error messages wil not be inserted any more into the result (representing the status document), 
                                // therefore we insert this specific message separately:
                                result = IsertErrorMessageStatusXml(result, "Napaka pri zapisovanju povratnice v bazo: " + ex.Message);
                            }
                            catch { }
                        }
                    }
                    if (!string.IsNullOrEmpty(exmsg))
                    {
                        if (!ErrorsInserted)
                        {
                            try
                            {
                                // Error messages has not yet been inserted into the result (XML markup representing the packet status), 
                                // therefore we insert this message here:
                                result = IsertErrorMessageStatusXml(result, exmsg );
                            }
                            catch { }
                        }
                        // Throw an exception if any crucial exception has been catched:
                        throw new Exception(exmsg);
                    }
                }
                
                //catch (SaveXmlToDiskException ex)
                //{
                //    ReportError(ex); 
                //    //trans.Rollback(); //TODO: treba se je zmeniti kako glede tega!!!
                //    //SetPaketStatus(paketId, 1);
                //    //ret = GetPaketStatusXml(paketId);
                //    //InsertIntoEfaPaketPovratnica(paketId, ret, ex.Message, ex.StackTrace);
                //}
                //catch (BulkCopyException ex)
                //{
                //    ReportError(ex);
                //    trans.Rollback();
                //    //SetPaketStatus(paketId, 1);
                //    //ret = GetPaketStatusXml(paketId);
                //    //InsertIntoEfaPaketPovratnica(paketId, ret, ex.Message, ex.StackTrace);
                //}
                ////catch (PaketStatusPovratnicaException ex)
                ////{

                ////}

                catch (Exception ex)
                {
                    if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Error, "EFA_WS.Service.PosljiPaket_Base", "Error: " + ex.Message,
                        PadoEnums.enTraceMsgSource.Server));
                    ReportError(ex);
                    //trans.Rollback(); //TODO: treba se je zmeniti kako glede tega!!!
                }
                finally
                {
                    if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Information, "EFA_WS.Service.PosljiPaket_Base", "Finished.", PadoEnums.enTraceMsgSource.Server));
                    Disconnect();
                }

                return result;
            }
        }



        /// <summary>
        /// Metoda je namenjena pošiljanju Xml paketa podatkov za fakturiranje.
        /// Z argumentom paketTipId povemo ali gre za ovrednoten ali neovrednoten
        /// paket podatkov.
        /// Podatek o viru (virId) je del sheme po kateri mora biti zgrajen
        /// paketXml; velja namreč pravilo, da lahko v enem paketu pošljemo podatke iz več
        /// virov (npr. UPO1 in UPO2), ki pa morajo biti vsi ovrednoteni ali neovrednoteni.
        /// </summary>
         /// <param name="paketXml"></param>
        /// <returns>Metoda vrne Xml s podatki o sprejetem paketu. V njem so podatki o Id-ju paketa,
        /// morebitnih napakah, itd.</returns>
        [WebMethod]
        public string PosljiPaket(string paketXml)
        {
            return PosljiPaket_Base(paketXml);
        }


        public static byte[] CompressBytes(byte[] Original)
        // Compresses the byte array Original by using the GZipStream and returns the compressed array.
        // $A Igor sep08;
        {
            byte[] zippedArray = null;
            MemoryStream ms = null;
            GZipStream compressedzipStream = null;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.CompressBytes", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                // Create a memory stream for compressed data:
                ms = new MemoryStream();
                compressedzipStream = new GZipStream(ms, CompressionMode.Compress);
                compressedzipStream.Write(Original, 0, Original.Length);
                // WARNING: The GZipStream MUST be closed before the data that is written is read:
                compressedzipStream.Close();
                compressedzipStream = null;
                // Warning: after closing compressedzipStream, ms.Position may not be set!
                //ms.Flush();
                //ms.Position = 0;
                zippedArray = ms.ToArray();
            }
            finally
            {
                try
                {
                    //Close the streams:
                    if (compressedzipStream != null)
                        compressedzipStream.Close();
                    if (ms != null)
                        ms.Close();
                }
                catch (Exception ex) 
                {
                    if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Error, "EFA_WS.Service.CompressBytes", "Error (re-thrown): " + ex.Message,
                        PadoEnums.enTraceMsgSource.Server));
                    throw ex;
                }
                finally
                {
                    if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Information, "EFA_WS.Service.CompressBytes", "Finished.", PadoEnums.enTraceMsgSource.Server));
                }
            }
            return zippedArray;
        }


        public static byte[] UncompressBytes(byte[] zippedArray)
        // Uncompresses the byte array Original by using the GZipStream and returns the compressed array.
        // $A Igor sep08;
        {
            return UncompressBytes(zippedArray, 100000);
        }

        public static byte[] UncompressBytes(byte[] zippedArray, int BufferSize)
        // Uncompresses the byte array Original by using the GZipStream and returns the compressed array.
        // BufferSize defines size of the buffer (amount of data that is worked at once).
        // $A Igor sep08;
        {
            if (BufferSize<=0)
                BufferSize = 100000;
            byte[] unzippedArray = null;
            MemoryStream msIn = null, msOut = null;
            GZipStream zipStream = null;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.UncompressBytes", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                bool InstantiateFromArray = true;
                if (InstantiateFromArray)
                {
                    // Instantiate a memory stream on an array:
                    msIn = new MemoryStream(zippedArray);
                }
                else
                {
                    // Instantiate an empty memory stream and write the array into it:
                    msIn = new MemoryStream();
                    msIn.Write(zippedArray, 0, zippedArray.Length);
                }
                // Reset the memory stream position to begin decompression.
                msIn.Position = 0;
                zipStream = new GZipStream(msIn, CompressionMode.Decompress);
                msOut = new MemoryStream();
                byte[] Buffer = new byte[BufferSize];
                int totalRead = 0, bytesRead = 0;
                do
                {
                    bytesRead = zipStream.Read(Buffer, 0, BufferSize);
                    if (bytesRead > 0)
                    {
                        totalRead += bytesRead;
                        msOut.Write(Buffer, 0, bytesRead);
                    }
                } while (bytesRead > 0);
                unzippedArray = msOut.ToArray();
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.UncompressBytes", "Error (re-thrown): " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                throw ex;
            }
            finally
            {
                try
                {
                    //Close the streams:
                    if (zipStream != null)
                        zipStream.Close();
                    if (msIn != null)
                        msIn.Close();
                    if (msOut != null)
                        msOut.Close();
                }
                catch (Exception ex1)
                {
                    if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Error, "EFA_WS.Service.UncompressBytes", "Error (re-thrown): " + ex1.Message,
                        PadoEnums.enTraceMsgSource.Server));
                    throw ex1;
                }
                finally
                {
                    if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Information, "EFA_WS.Service.UncompressBytes", "Finished.", PadoEnums.enTraceMsgSource.Server));
                }
            }
            return unzippedArray;
        }


        private string DecompressToString(byte[] paketZip, string ResultFilename)
        // Decompresses the Unicode - encoded byte array paketZip, converts it to a string and returns the
        // string.
        // If ResultFilename!=null then ret of decompression is saved to the file with that name.
        // $A Igor sep08;
        {
            string paketXml = null;
            const int BufferSize = 100000;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.DecompressToString", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                byte[] paketUnzipped = UncompressBytes(paketZip, BufferSize);
                // Convert the byte array containing compressed data to a string. The same encoding as
                // here must be used in EFA/PosljiPaketZip(...).
                paketXml = System.Text.Encoding.Unicode.GetString(paketUnzipped);
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.DecompressToString", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
            }
            finally
            {
                try
                {
                    if (ResultFilename != null) if (ResultFilename.Length > 0)
                        {
                            StreamWriter writer = new StreamWriter(ResultFilename);
                            writer.Write(paketXml);
                            writer.Close();
                            writer.Dispose();
                            //Console.WriteLine("The uncompressed packet has been written to a file (no database operations performed).\n\nFile name:\n"
                            //    + ResultFilename);
                        }
                }
                catch (Exception ex1)
                {
                    if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Error, "EFA_WS.Service.DecompressToString", "Error: " + ex1.Message,
                        PadoEnums.enTraceMsgSource.Server));
                    ReportError(ex1);
                }
                finally
                {
                    if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                        EventLogEntryType.Information, "EFA_WS.Service.DecompressToString", "Finished.", PadoEnums.enTraceMsgSource.Server));
                }
            }
            return paketXml;
        }


        /// <summary>
        /// Metoda je namenjena pošiljanju Xml paketa podatkov za fakturiranjev komprimirani obliki.
        /// Z argumentom paketTipId povemo ali gre za ovrednoten ali neovrednoten paket podatkov.
        /// Podatek o viru (virId) je del sheme, po kateri mora biti zgrajen
        /// paketXml; velja namreč pravilo, da lahko v enem paketu pošljemo podatke iz več
        /// virov (npr. UPO1 in UPO2), ki pa morajo biti vsi ovrednoteni ali neovrednoteni.
        /// </summary>
         /// <param name="paketXml"></param>
        /// <returns>Metoda vrne Xml s podatki o sprejetem paketu. V njem so podatki o Id-ju paketa,
        /// morebitnih napakah, itd.</returns>
        [WebMethod]
        public string PosljiPaketZip(byte[] paketZip)
        {
            int BufferSize = 100000;
            string paketXml = null, ret=null;
            bool UpdateInDataBase = true;  // set to false for testing (to avoid updating data in a database)
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.PosljiPaketZip", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                if (UpdateInDataBase)
                {
                    byte[] paketUnzipped = UncompressBytes(paketZip, BufferSize);
                    // Convert the byte array containing compressed data to a string. The same encoding as
                    // here must be used in EFA/PosljiPaketZip(...).
                    paketXml = System.Text.Encoding.Unicode.GetString(paketUnzipped);
                    //paketXml = DecompressToString(paketZip, null);
                    ret = PosljiPaket_Base(paketXml);
                }
                else
                {
                    if (DebugMode)
                        paketXml = DecompressToString(paketZip, "c:\\temp\\Paket_DecomPressed_on_WS.xml");
                }
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.PosljiPaketZip", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.PosljiPaketZip", "Finished.", PadoEnums.enTraceMsgSource.Server));
            }
            return ret;
        }

        #endregion  // Posiljanje_Paketa


        static int _stevilkaSeje=0;

        /// <summary>
        /// Vrne številko seje. 
        /// Zaenkrat se metoda uporablja za testiranje, ali je WS sploh postavljen (vrniti mora št.>=0).
        /// </summary>
        /// <returns>Current ession number.</returns>
        [WebMethod]
        public int StevilkaSeje()
        {
            return _stevilkaSeje;
        }



        /// <summary>
        /// Metoda vrne Xml (kot string) s podatki o statusu paketa.
        /// </summary>
        /// <param name="paketId">Id paketa, ki smo ga dobili ob pošiljanju paketa.</param>
        /// <returns></returns>
        [WebMethod]
        public string PaketStatus(decimal paketid)
        {
            string result;
            result = GetPaketStatusXml(paketid, null);
            // InsertIntoEfaPaketPovratnica(paketId, ret);
            return result;
        }


        /// <summary>
        /// Metoda vrne byte[] serializiranega file-a (type FileInfo) PaketShema.xsd.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public byte[] PaketShema()
        {
            byte[] ret=null;
            FileInfo _xsdFileInfo;
            // XmlDocument _xsdXmlDoc;
            MemoryStream _ms;
            BinaryFormatter _bf;
            // System.Xml.Schema.XmlSchema a;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.PaketShema", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));

                _xsdFileInfo = new FileInfo(_paketSchemaPathName);

                _ms = new MemoryStream();
                _bf = new BinaryFormatter();
                _bf.Serialize(_ms, _xsdFileInfo);
                //_bf.Serialize(_ms, _xsdXmlDoc); -> to ne dela ker razred XmlDocument ni serializable (isto velja za System.Xml.Schema.XmlSchema)
                ret = _ms.ToArray();
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.PaketShema", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
                //todo: all exceptions must be logged to one place 
                ret = new byte[] { };
                //throw;
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.PaketShema", "Finished.", PadoEnums.enTraceMsgSource.Server));
            }
            return ret;
        }


        /// <summary>
        /// Metoda vrne byte[] serializiranega file-a (type FileInfo) PaketStatusShema.xsd.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public byte[] PaketStatusShema()
        {
            byte[] ret=null;
            FileInfo _xsdFileInfo;
            MemoryStream _ms;
            BinaryFormatter _bf;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.PaketStatusShema", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                _xsdFileInfo = new FileInfo(_paketStatusSchemaPathName);
                _ms = new MemoryStream();
                _bf = new BinaryFormatter();
                _bf.Serialize(_ms, _xsdFileInfo);
                ret = _ms.ToArray();
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.PaketStatusShema", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
                //todo: all exceptions must be logged to one place 
                ret = new byte[] { };
                //throw;
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.PaketStatusShema", "Finished.", PadoEnums.enTraceMsgSource.Server));
            }
            return ret;
        }

        /// <summary>
        /// Metoda vrne zalogo vrednosti za tipe paketa.
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet PaketTipZalogaVrednosti()
        {
            //ta metoda uporablja svoj connection ...
            DataSet result = new DataSet("PaketTipZalogaVrednosti");
            System.Data.SqlClient.SqlConnection _cnnLocal = new System.Data.SqlClient.SqlConnection(cnnString);

            try
            {
                _cnnLocal.Open();
                System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter(selectPaketTipZalogaVrednosti, _cnnLocal);
                sqlDA.Fill(result);
                return result;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return result;
                //todo: log error ...
                //throw;
            }
            finally
            {
                _cnnLocal.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet PaketStatusZalogaVrednosti()
        {
            //ta metoda uporablja svoj connection ...
            DataSet result = new DataSet("PaketStatusZalogaVrednosti");
            System.Data.SqlClient.SqlConnection _cnnLocal = new System.Data.SqlClient.SqlConnection(cnnString);

            try
            {
                _cnnLocal.Open();
                System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter(selectPaketStatusZalogaVrednosti, _cnnLocal);
                sqlDA.Fill(result);
                return result;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return result;
                //todo: log error ...
                //throw;
            }
            finally
            {
                _cnnLocal.Close();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet PodrocjaZalogaVrednosti()
        {
            //ta metoda uporablja svoj connection ...
            DataSet result = new DataSet("PodrocjaZalogaVrednosti");
            System.Data.SqlClient.SqlConnection _cnnLocal = new System.Data.SqlClient.SqlConnection(cnnString);

            try
            {
                _cnnLocal.Open();
                System.Data.SqlClient.SqlDataAdapter sqlDA = new System.Data.SqlClient.SqlDataAdapter(selectPodrocjaZalogaVrednosti, _cnnLocal);
                sqlDA.Fill(result);
                return result;
            }
            catch (Exception ex)
            {
                ReportError(ex);
                return result;
                //todo: log error ...
                //throw;
            }
            finally
            {
                _cnnLocal.Close();
            }
        }

        //OD TU NAPREJ:

        string cnnString;
        string cnnStringBulk;
        string _paketSchemaPathName;
        string _paketStatusSchemaPathName;
        string _pathName;
        string _SQLServerTempFilePath;
        // string xsdPaketSchema;
        // string xsdPaketStatusSchema;
        static string selectPaketStatusZalogaVrednosti = "SELECT * FROM EfaPaketStatus";
        static string selectPaketTipZalogaVrednosti = "SELECT * FROM EfaPaketTip";
        static string selectPodrocjaZalogaVrednosti = "SELECT * FROM EfaPodrocjaView";

        //sinhronizacija niti
        static readonly object syncObject = new object();

        System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlTransaction trans = null;

        event ExceptionEventDelegate exEvent;

        /// <summary>This triggers the exEvent. The point is that the eventhandler is called in the basic web service thread
        /// (in the thread where the event is initialized!).
        /// IMPORTANT: The event handler THROWS AN EXCEPTION (which happens in the basic thread, therefore execution of that thread
        /// is broken). If onExEvent() method is called in a parallel thread that was launched form the basic one then the event
        /// handler will be executed at approximately the same time as Join() is called for the parallel thread.</summary>
        /// <param name="args"></param>
        void onExEvent(ExceptionEventArgs args)
        {
            if (exEvent != null)
                exEvent(this, args);
        }

        int paketId = -1;
        string result = "<Napaka/>";
        string ErrorFileName = null;

        void Service_exEvent(object sender, ExceptionEventArgs e)
        {
            //string ret = string.Empty;

            try
            {
                throw e.Ex;
            }
            catch (BulkCopyException ex)
            {
                ReportError(ex);
                trans.Rollback();
                SetPaketStatus(paketId, 1);
                result = GetPaketStatusXml(paketId, ErrorFileName);
                InsertIntoEfaPaketPovratnica(paketId, result, ex.Message, ex.StackTrace);
            }
            catch (SaveXmlToDiskException ex)
            {
                ReportError(ex);
                //trans.Rollback(); //TODO: treba se je zmeniti kako glede tega!!!
                SetPaketStatus(paketId, 1);
                result = GetPaketStatusXml(paketId, null);
                InsertIntoEfaPaketPovratnica(paketId, result, ex.Message, ex.StackTrace);
            }
            catch (PaketStatusPovratnicaException ex)
            {
                ReportError(ex);
                //TODO: Do nothing????
            }
            catch (Exception ex)
            {
                ReportError(ex);
                //TODO: Do nothing????
            }
        }

        void ReadSettings()
        {
            cnnString = GetCnnString();
            cnnStringBulk = GetCnnStringBulk();
            _paketSchemaPathName = GetPaketSchemaPathName();
            _paketStatusSchemaPathName = GetPaketStatusSchemaPathName();
            _pathName = GetPathName();
            _SQLServerTempFilePath = GetSQLServerTempFilePath();
        }

        void Connect()
        {
            cnn.ConnectionString = cnnString;

            if (cnn.State == ConnectionState.Closed || cnn.State == ConnectionState.Broken)
            {
                cnn.Open();

                //Send SET LANGUAGE command to be sure in this connection all commands will send date in us_english format
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SET LANGUAGE us_english", cnn);
                cmd.ExecuteNonQuery();
            }
        }

        void Disconnect()
        {
            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
            }
        }


        /// <summary>Returns number of entries in EfaGlava with a specified PaketId.</summary>
        /// <param name="PaketId">PaketId whose corresponding entries in EfaGlava are counted.</param>
        /// <returns>Number of entries in EfaGlava with a given PaketId.</returns>
        int CountStzapisov_EfaGlava(int PaketId)
        {
            return CountStzapisov_EfaGlava(PaketId, null);
        }

        /// <summary>Returns number of entries in EfaGlava with a specified PaketId.</summary>
        /// <param name="PaketId">PaketId whose corresponding entries in EfaGlava are counted.</param>
        /// <returns>Number of entries in EfaGlava with a given PaketId.</returns>
        int CountStzapisov_EfaGlava(int PaketId, System.Data.SqlClient.SqlTransaction transaction)
        {
            int ret = 0;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "SELECT COUNT (Id) FROM EfaGlava WHERE PaketId = " + PaketId.ToString();
                cmd.CommandType = System.Data.CommandType.Text;
                if (transaction != null)
                    cmd.Transaction = transaction;
                ret = (int) cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    cmd.Dispose();
                }
                catch { }
            }
            return ret;
        }

        /// <summary>Sets status </summary>
        /// <param name="PaketId"></param>
        void SetPaketStatusZavrnjen(int PaketId)
        {
            const int StatusZavrnjen = 1;
            SetPaketStatus(paketId, StatusZavrnjen);
        }

        /// <summary>Sets status of packet with a given PaketId to StatusId.</summary>
        /// <param name="PaketId">Id of entry in EfaPaket.</param>
        /// <param name="StatusId">Value to which PaketStatusId is set.</param>
        void SetPaketStatus(int PaketId, int StatusId)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "UPDATE efaPaket SET PaketStatusId = " + StatusId.ToString() + "  WHERE Id = " + PaketId.ToString();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cmd.Dispose();
            }
        }


        int InsertIntoEfaPaket(int podrocjeId, byte paketTipId, DateTime paketDatum, int stZapisov, decimal vsotaDokumentDatum, 
            decimal vsotaCenaNeto, int paketStatusId)
        {
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter();

            int maxId = -1;

            try
            {
                cmd = new System.Data.SqlClient.SqlCommand();
                
                cmd.Connection = cnn;
                cmd.Transaction = trans;
                cmd.CommandText = "EfaPaketInsert";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@podrocjeId", System.Data.SqlDbType.Int);
                param.Value = podrocjeId;

                param = cmd.Parameters.Add("@paketTipId", System.Data.SqlDbType.TinyInt);
                param.Value = paketTipId;

                param = cmd.Parameters.Add("@paketDatum", System.Data.SqlDbType.VarChar); //Če je bil tukaj DateTime je zadeva crkovala, ker je WS poslal na Pošti na njihovem strežniku dvakrat enojni narekovaj, pojma nimam zakaj -- 2.12.2008, DG
                param.Value = paketDatum.ToString(new System.Globalization.CultureInfo("en-US")); //TODO: Tale culture info mora it v web.config!!!! - se mora pa ujemat s formatom datuma, ki ga nastavimo pri povezavi na SQL strežnik.

                param = cmd.Parameters.Add("@stZapisov", System.Data.SqlDbType.BigInt);
                param.Value = stZapisov;

                param = cmd.Parameters.Add("@vsotaDokumentDatum", System.Data.SqlDbType.Decimal);
                param.Value = vsotaDokumentDatum;

                param = cmd.Parameters.Add("@vsotaCenaNeto", System.Data.SqlDbType.Decimal);
                param.Value = vsotaCenaNeto;

                param = cmd.Parameters.Add("@paketStatusId", System.Data.SqlDbType.TinyInt);
                param.Value = paketStatusId;

                maxId = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                    ReportError(ex);
                    // It is crucial that exception is re-thrown!
                    throw (ex);
            }
            finally
            {
                param = null;
                cmd.Dispose();
            }

            return maxId;
        }


        void UpdateGlavaPostavke(decimal paketId)
        {
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            System.Data.SqlClient.SqlParameter param = null;

            try
            {
                cmd.Connection = cnn;
                cmd.Transaction = trans;
                cmd.CommandText = "EfaGlavaEfaPostavkeUpdatePaketId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@paketId", System.Data.SqlDbType.Decimal);
                param.Value = paketId;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ReportError(ex);
                ExceptionEventArgs exevarg = new ExceptionEventArgs();
                exevarg.Ex = new BulkCopyException(ex.Message, ex);
                //exevarg.PaketId = paketId;
                onExEvent(exevarg);
            }
            finally
            {
                param = null;
                cmd.Dispose();
            }
        }

        void SaveXmlToDisk(string paketXml, decimal paketId, string FileName)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.LoadXml(paketXml);
                doc.Save(_pathName + FileName);
            }
            catch (Exception ex)
            {
                ReportError(ex);
                ExceptionEventArgs exevarg = new ExceptionEventArgs();
                exevarg.Ex = new SaveXmlToDiskException(ex.Message, ex);
                //exevarg.PaketId = paketId;
                onExEvent(exevarg);
            }
            finally
            {
                doc = null;
            }
        }

        void InsertPaketXml(object arg)
        {
            try
            {
                InsertPaketData data = arg as InsertPaketData;
                if (data == null)
                    throw new ArgumentException("Invalid data argument: null reference or not of the type InsertPaketData.");

                SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class objBL = new SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class();

                objBL.ConnectionString = cnnStringBulk;
                // Error file name must be written to the object to be accessible to the calling thread:
                ErrorFileName = objBL.ErrorLogFile = 
                        _pathName + GetXmlErrorFileName((decimal) data.PaketId);
                objBL.Transaction = true;
                objBL.KeepIdentity = false;
                objBL.CheckConstraints = true;
                objBL.KeepNulls = true;

                if (RemoteMode)
                {
                    objBL.TempFilePath = _SQLServerTempFilePath;
                }
                else
                {
                    objBL.TempFilePath = _pathName;
                }

                // objBL.Execute(_paketSchemaPathName, _pathName + GetXmlFileName((decimal)data.PaketId));
                objBL.Execute(_paketSchemaPathName, _pathName + data.PaketFileName);
            }
            //catch (System.Runtime.InteropServices.COMException comEx)
            //{
            //    ReportError(ex);
            //    //todo: Replace the messagebox below with throw (now commented)! 
            //    // throw new BulkCopyException(comEx.Message);
            //    System.Windows.Forms.MessageBox.Show(comEx.Message);
                
            //}
            catch (Exception ex)
            {
                ReportError(ex);
                ExceptionEventArgs exevarg = new ExceptionEventArgs();
                exevarg.Ex = new BulkCopyException(ex.Message, ex);
                //exevarg.PaketId = (decimal)paketId;
                // WARNING: onExEvent() throws an exception, but this happens in the calling thread 
                // (the one that launched the current one), therefore execution of that thread is broken approximately
                // wehen Join() is called for the current thread (since at that very moment the event handler
                // is executed).
                onExEvent(exevarg);
            }
            finally
            {
                // If an error file was written by SQLXMLBulkLoad4Class then this file was used in onExEvent()
                // method and it will not be needed again:
                ErrorFileName = null;
                //objBL = null;
            }
        }


        /// <summary>Adds to the XML document a node with a list of errors, which is extracted from a file
        /// created by the SqlXml tool (if the file exists).</summary>
        /// <param name="status">XML document that is updated with error information.</param>
        /// <param name="filename">Name of the file that contains information on errors.</param>
        static void IsertErrorInfoStatusXml(XmlDocument status, string filename)
        // Adds information from the error file named filename (created by SqlXml) into Xml document describing
        // the status
        {
            try
            {
                if (status != null && !string.IsNullOrEmpty(filename))
                {
                    if (File.Exists(filename))
                    {
                        // Error file is not a proper XML document because the file level contains several
                        // XML elements rather than a single root element. We enclose this in a root element and 
                        // create an XML document from it.
                        string contents = File.ReadAllText(filename);
                        int posfirstelement = contents.IndexOf(">", 0);
                        if (posfirstelement > 0)
                        {
                            // We skip the <?xml> declaraton:
                            ++posfirstelement;
                            string strdoc = contents.Substring(0, posfirstelement)
                                + " <ErrorData> "
                                + contents.Substring(posfirstelement)
                                + "</ErrorData>";
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(strdoc);
                            XmlNode root = EFA_Functions.GetRootElement(doc); // doc.FirstChild;
                            XmlNode statusroot = EFA_Functions.GetRootElement(status);  // status.FirstChild;
                            XmlNode errornode = status.CreateElement("SeznamNapak");
                            statusroot.AppendChild(errornode);
                            for (int i = 0; i < root.ChildNodes.Count; ++i)
                            {
                                XmlNode child = root.ChildNodes[i];
                                errornode.AppendChild(status.ImportNode(child, true));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>Adds to the XML document a node with the error message. This is intended for errors that 
        /// occur during insertion of XML document into the database. 
        /// XML document is specified as a string argument, and a string containing an updated document is returned./// </summary>
        /// <param name="status">String containing the original XML document representing the packet status.</param>
        /// <param name="errorstring">Error message to be inserted in the XML document.</param>
        /// <returns>String representing the updated document.</returns>
        static string IsertErrorMessageStatusXml(string status, string errorstring)
        {
            string ret=status;
            try
            {
                XmlDocument statusdoc = new XmlDocument();
                statusdoc.LoadXml(status);
                IsertErrorMessageStatusXml(statusdoc,errorstring);
                ret = statusdoc.OuterXml;
            }
            catch { }
            return ret;
        }

        /// <summary>Adds to the XML document a node with the error message. This is intended for errors that 
        /// occur during insertion of XML document into the database.</summary>
        /// <param name="status">XML document that is updated with error information.</param>
        /// <param name="errorstring">String containing error messages.</param>
        static void IsertErrorMessageStatusXml(XmlDocument status, string errorstring)
        // Adds information from the error file named filename (created by SqlXml) into Xml document describing
        // the status
        {
            try
            {
                if (status != null && !string.IsNullOrEmpty(errorstring))
                {
                    XmlNode statusroot = EFA_Functions.GetRootElement(status);  // status.FirstChild;
                    XmlNode errornode = status.CreateElement("SeznamNapak");
                    statusroot.AppendChild(errornode);
                    XmlNode child = errornode.AppendChild(status.CreateTextNode(errorstring));
                }
            }
            catch { }
        }

        string GetPaketStatusXml(decimal paketId, string ErrorFileName)
        {
            return GetPaketStatusXml(paketId, ErrorFileName, null);
        }

        /// <summary>Returns an XML document containing status of the given Packet.
        /// ErrorFileName should only be specified when the packet is inserted.</summary>
        /// <param name="paketId">Id of the packet whose information is returned as XML string.</param>
        /// <param name="ErrorFileName">Eventual error file that was created by SqlXML, or null if no error file is provided.
        /// This parameter can only be passed immediately after inserting the packet into the database, when the error file still exists.
        /// If ErrorFile is specified then the method inserts information form the file to the returned XML string.</param>
        /// <returns>An Xml string containing information about packet status.</returns>
        string GetPaketStatusXml(decimal paketId, string ErrorFileName, string ErrorString)
        {
            Microsoft.Data.SqlXml.SqlXmlCommand cmd = new Microsoft.Data.SqlXml.SqlXmlCommand(cnnStringBulk);
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            string ret = string.Empty;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.GetPaketStatusXml", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                cmd.RootTag = "EfaPaketStatus";
                cmd.OutputEncoding = "UTF-16";
                cmd.CommandText = "EfaPaket[Id = " + paketId.ToString() + "]";
                cmd.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.XPath;
                cmd.SchemaPath = _paketStatusSchemaPathName;

                System.IO.Stream stream;
                stream = cmd.ExecuteStream();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                {
                    ret = sr.ReadToEnd();
                }
                xmldoc.LoadXml(ret);
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.GetPaketStatusXml", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
                ExceptionEventArgs exevarg = new ExceptionEventArgs();
                exevarg.Ex = new PaketStatusPovratnicaException(ex.Message, ex);
                onExEvent(exevarg);
            }
            finally
            {
                if (!string.IsNullOrEmpty(ErrorString))
                {
                    try
                    {
                        IsertErrorMessageStatusXml(xmldoc,ErrorString);
                    }
                    catch {  }
                }
                if (!string.IsNullOrEmpty(ErrorFileName)) if (File.Exists(ErrorFileName))
                {
                    try
                    {
                        IsertErrorInfoStatusXml(xmldoc, ErrorFileName);
                    }
                    catch {  }

                }
                try
                {
                    ret = xmldoc.OuterXml;
                }
                catch {  }
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.GetPaketStatusXml", "Finished.", PadoEnums.enTraceMsgSource.Server));
                xmldoc = null;
                cmd = null;
            }

            return ret;
        }


        /// <summary>Updates the field PaketStatusId for a given entry of the EfaPaket table, with the specified value.</summary>
        /// <param name="paketId">ID of the packet for which the entry must be updatet.</param>
        /// <param name="paketStatusId">The value of the PaketStatusId field that should be set for the given entry.</param>
        void SetPaketStatus(decimal paketId, int paketStatusId)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlParameter param = null;
            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.SetPaketStatus", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                cmd = new System.Data.SqlClient.SqlCommand();
                param = new System.Data.SqlClient.SqlParameter();

                cmd.Connection = cnn;
                //cmd.Transaction = trans;

                cmd.CommandText = "UPDATE EfaPaket SET PaketStatusId = @paketStatusId WHERE ID = @paketId";
                cmd.CommandType = System.Data.CommandType.Text;


                param = cmd.Parameters.Add("@paketId", System.Data.SqlDbType.Int);
                param.Value = paketId;

                param = cmd.Parameters.Add("@paketStatusId", System.Data.SqlDbType.TinyInt);
                param.Value = paketStatusId;

                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.SetPaketStatus", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
                ExceptionEventArgs exevarg = new ExceptionEventArgs();
                exevarg.Ex = new PaketStatusPovratnicaException(ex.Message, ex);
                onExEvent(exevarg);
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.SetPaketStatus", "Finished.", PadoEnums.enTraceMsgSource.Server));
                param = null;
                try { cmd.Dispose(); }
                catch { }
            }
        }

        /// <summary>Insert a new entry for the given packet into the table EfaPaketPovratnica.</summary>
        /// <param name="paketId">ID of the packet for which data is inserted.</param>
        /// <param name="paketStatusXml">XML document describing the status of the packet that was sent to the web service.</param>
        /// <param name="exMessage">Eventual exception's message.</param>
        /// <param name="exStackTrace">Eventual exception's stack trace.</param>
        void InsertIntoEfaPaketPovratnica(decimal paketId, string paketStatusXml, string exMessage, string exStackTrace)
        {
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlParameter param = null;

            try
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.InsertIntoEfaPaketPovratnica", "Started ...",
                    PadoEnums.enTraceMsgSource.Server));
                cmd = new System.Data.SqlClient.SqlCommand();
                param = new System.Data.SqlClient.SqlParameter();

                cmd.Connection = cnn;
                //cmd.Transaction = trans;
                cmd.CommandText = @"INSERT INTO EfaPaketPovratnica (PaketId, PaketStatusXml, NapakaSporocilo, NapakaStackTrace) 
                                           VALUES (@paketId, @paketStatusXml, @exMessage, @exStackTrace)";
                cmd.CommandType = System.Data.CommandType.Text;

                param = cmd.Parameters.Add("@paketId", System.Data.SqlDbType.Int);
                param.Value = paketId;

                param = cmd.Parameters.Add("@paketStatusXml", System.Data.SqlDbType.Xml);
                param.Value = paketStatusXml;

                param = cmd.Parameters.Add("@exMessage", System.Data.SqlDbType.VarChar);
                if (exMessage != null)
                {
                    param.Value = exMessage;
                }
                else
                {
                    param.Value = System.DBNull.Value;
                }

                param = cmd.Parameters.Add("@exStackTrace", System.Data.SqlDbType.VarChar);
                if (exStackTrace != null)
                {
                    param.Value = exStackTrace;
                }
                else
                {
                    param.Value = System.DBNull.Value;
                }

                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                if (PadoVariables.PadoTraceSwitch.TraceError) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Error, "EFA_WS.Service.InsertIntoEfaPaketPovratnica", "Error: " + ex.Message,
                    PadoEnums.enTraceMsgSource.Server));
                ReportError(ex);
                ExceptionEventArgs exevarg = new ExceptionEventArgs();
                exevarg.Ex = new PaketStatusPovratnicaException(ex.Message, ex);
                onExEvent(exevarg);
            }
            finally
            {
                if (PadoVariables.PadoTraceSwitch.TraceInfo) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                    EventLogEntryType.Information, "EFA_WS.Service.InsertIntoEfaPaketPovratnica", "Finished.", PadoEnums.enTraceMsgSource.Server));
                param = null;
                try { cmd.Dispose(); }
                catch { }
            }
        }

        void InsertIntoEfaPaketPovratnica(decimal paketId, string paketStatusXml)
        {
            InsertIntoEfaPaketPovratnica(paketId, paketStatusXml, null, null);
        }


        string GetXmlFileName(decimal paketId)
        {
            //$$
            string fileName = null;
            fileName = "EfaPaket_"  + paketId.ToString() + ".xml";
            fileName = "EfaPaket_WS_" + paketId.ToString() + "_" + Guid.NewGuid().ToString() + ".xml";
            return fileName;
        }

        string GetXmlErrorFileName(decimal paketId)
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetXmlErrorFileName", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string fileName = "ERR_EfaPaket" + "_" + paketId.ToString() + "_" + Guid.NewGuid() + ".xml";
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetXmlErrorFileName", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return fileName;
        }

        string GetPaketSchemaPathName()
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetPaketSchemaPathName", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string path = System.Configuration.ConfigurationManager.AppSettings.Get("PaketSchemaPathName");
            if (path == null)
            {
                //TODO: throw exception if path is not specified!
                path = "c:\\paketannschema_test.xml";
            } else
            {
                path = System.Environment.ExpandEnvironmentVariables(path);
            }
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetPaketSchemaPathName", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return path;
        }

        string GetPaketStatusSchemaPathName()
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetPaketStatusSchemaPathName", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string path = System.Configuration.ConfigurationManager.AppSettings.Get("PaketStatusSchemaPathName");
            if (path == null)
            {
                //TODO: throw exception if path is not specified!
                path = "c:\\paketstatusannschema_test.xml";
            } else
            {
                path = System.Environment.ExpandEnvironmentVariables(path);
            }
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetPaketStatusSchemaPathName", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return path;
        }

        string GetPathName()
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetPathName", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string path = System.Configuration.ConfigurationManager.AppSettings.Get("PathName");
            if (path == null)
            {
                path = "c:\\";
            }  else
            {
                path = System.Environment.ExpandEnvironmentVariables(path);
            }
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetPathName", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return path;
        }

        string GetSQLServerTempFilePath()
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetSQLServerTempFilePath", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string path = System.Configuration.ConfigurationManager.AppSettings.Get("SQLServerTempFilePath");
            if (path == null)
            {
                path = "c:\\";
            }
            else
            {
                path = System.Environment.ExpandEnvironmentVariables(path);
            }
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetSQLServerTempFilePath", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return path;
        }


        string GetCnnString()
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetCnnString", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string cnnString = System.Configuration.ConfigurationManager.AppSettings.Get("cnnString");

            if (cnnString == null)
            {
                cnnString = "Server=(local);" +
                                      "Database=efa2;" +
                                      "Persist Security Info=False;" +
                                      "Trusted_Connection=True;";
            } else
            {
                cnnString = System.Environment.ExpandEnvironmentVariables(cnnString);
            }
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetCnnString", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return cnnString;
        }


        string GetCnnStringBulk()
        {
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetCnnStringBulk", "Started ...",
                PadoEnums.enTraceMsgSource.Server));
            string cnnString = System.Configuration.ConfigurationManager.AppSettings.Get("cnnStringBulk");
            
            if (cnnString == null)
            {
                cnnString = "Server=(local);" +
                                      "Database=efa2;" +
                                      "Persist Security Info=False;" +
                                      "Trusted_Connection=True;";
                cnnString = "Provider=SQLOLEDB;" + cnnString + "Integrated Security=SSPI";
            } else
            {
                cnnString = System.Environment.ExpandEnvironmentVariables(cnnString);
            }
            if (PadoVariables.PadoTraceSwitch.TraceVerbose) Trace.WriteLine(PadoFunctions.FormatTraceMsg(
                EventLogEntryType.Information, "EFA_WS.Service.GetCnnStringBulk", "Finished.",
                PadoEnums.enTraceMsgSource.Server));
            return cnnString;
        }

        private void InitializeComponent()
        {

        }
    }
}
