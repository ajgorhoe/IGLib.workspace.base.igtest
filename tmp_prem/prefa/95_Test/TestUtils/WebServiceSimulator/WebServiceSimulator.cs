using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

namespace efakt1
{
  public  class WebServiceSimulator
    {

        string cnnString;
        string cnnStringBulk;
        string xmlAnnSchemaPathName;
        string xmlStatusAnnSchemaPathName;
        string xmlPathName;


        System.Data.SqlClient.SqlConnection cnn = null;
        System.Data.SqlClient.SqlTransaction trans = null;


        public WebServiceSimulator()
        {
            ReadSettings();
        }


        void ReadSettings()
        {
            cnnString = GetCnnString();
            cnnStringBulk = GetCnnStringBulk();
            xmlAnnSchemaPathName = GetXmlAnnSchemaPathName();
            xmlStatusAnnSchemaPathName = GetXmlStatusAnnSchemaPathName();
            xmlPathName = GetXmlPathName();
        }

        public void Connect()
        {
            cnn = new SqlConnection();
            cnn.ConnectionString = cnnString;
            cnn.Open();
        }

        public void Disconnect()
        {
            if (cnn.State == System.Data.ConnectionState.Open) cnn.Close();
        }

        public string PosljiPaket(byte paketTipId, DateTime paketDatum, int stZapisov, decimal vsotaDokumentDatum, decimal vsotaCenaNeto, string paketXml)
        {
            decimal paketId;
            string result = string.Empty;

            try
            {
                //cnn.ConnectionString = cnnString;
                //cnn.Open();
                Connect();

                trans = cnn.BeginTransaction();

                paketId = InsertIntoEfaPaket(paketTipId, paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto,1);
                SaveXmlToDisk(paketXml, paketId);

                trans.Commit();


                trans = cnn.BeginTransaction();

                InsertPaketXml(paketId);
                UpdateGlavaPostavke(paketId);

                result = GetPaketStatusXml(paketId);

                trans.Commit();
                //cnn.Close();
                Disconnect();
                //result = "asdas";

            }
            catch (Exception ex)
            {
                trans.Rollback();
                Disconnect();
                //if (cnn.State == System.Data.ConnectionState.Open) cnn.Close();
            }

            return result;
        }

       public int InsertIntoEfaPaket(byte paketTipId, DateTime paketDatum, int stZapisov, decimal vsotaDokumentDatum, decimal vsotaCenaNeto, int paketStatusId)
        {

            //string insert = "insert into efapaket(pakettipid, paketdatum, stevilozapisov,vsotadokumentdatum,vsotacenaneto,stevilozapisovkontrola,vsotadokumentdatumkontrola,vsotacenanetokontrola,paketstatusid)" +
            //    "values(@pakettipid, @paketdatum, @stevilozapisov,@vsotadokumentdatum,@vsotacenaneto,@stevilozapisovkontrola,@vsotadokumentdatumkontrola,@vsotacenanetokontrola,@paketstatusid)";

            //string selectMaxId = "SELECT MAX(Id) FROM efapaket";

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter();

            int maxId = 0;

            try
            {
                //cnn = new System.Data.SqlClient.SqlConnection();
                //cnn.ConnectionString = mstCnnString;
                //cnn.Open();
                //insertCmd = new System.Data.SqlClient.SqlCommand(insert, cnn);

                //param = System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
                //System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
                cmd = new System.Data.SqlClient.SqlCommand();
                param = new System.Data.SqlClient.SqlParameter();

                cmd.Connection = cnn;
                cmd.Transaction = trans;
                cmd.CommandText = "InsertIntoEfaPaket";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                param = cmd.Parameters.Add("@paketTipId", System.Data.SqlDbType.TinyInt);
                param.Value = paketTipId;

                param = cmd.Parameters.Add("@paketDatum", System.Data.SqlDbType.DateTime);
                param.Value = paketDatum;

                param = cmd.Parameters.Add("@stZapisov", System.Data.SqlDbType.BigInt);
                param.Value = stZapisov;

                param = cmd.Parameters.Add("@vsotaDokumentDatum", System.Data.SqlDbType.Float);
                param.Value = vsotaDokumentDatum;

                param = cmd.Parameters.Add("vsotaCenaNeto", System.Data.SqlDbType.Decimal);
                param.Value = vsotaCenaNeto;

                //param = cmd.Parameters.Add("stZapisovKontrola", System.Data.SqlDbType.BigInt);
                //param.Value = stZapisovKontrola;

                //param = cmd.Parameters.Add("vsotaDokumentDatumKontrola", System.Data.SqlDbType.Float);
                //param.Value = vsotaDokumentDatumKontrola;

                //param = cmd.Parameters.Add("vsotaCenaNetoKontrola", System.Data.SqlDbType.Decimal);
                //param.Value = vsotaCenaNetoKontrola;

                param = cmd.Parameters.Add("paketStatusId", System.Data.SqlDbType.TinyInt);
                param.Value = paketStatusId;

                //cnn.Open();

                maxId = (int)cmd.ExecuteScalar();

                //cnn.Close();

            }
            finally
            {
                param = null;
                cmd.Dispose();
            }
            //catch (Exception ex)
            //{
            //    //MessageBox.Show("Ne morem odpreti baze!" + Environment.NewLine + ex.Message);
            //    //if (cnn.State == ConnectionState.Open) cnn.Close();
            //    status = false;
            //    err = ex.Message;

            //}
            return maxId;
        }


        public void UpdateGlavaPostavke(decimal paketId)
        {
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            System.Data.SqlClient.SqlParameter param = null;

            try
            {
                cmd.Connection = cnn;
                cmd.Transaction = trans;
                cmd.CommandText = "UpdateGlavaPostavke";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@paketId", System.Data.SqlDbType.Decimal);
                param.Value = paketId;

                //cnn.Open();
                cmd.ExecuteNonQuery();
                //cnn.Close();
            }
            finally
            {
                param = null;
                cmd.Dispose();
            }
            //catch (Exception ex)
            //{
            //    //trans.Rollback();
            //    err = ex.Message;
            //    throw;
            //    //if (cnn.State == ConnectionState.Open) cnn.Close();
            //}

        }

        public void SaveXmlToDisk(string paketXml, decimal paketId)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(paketXml);
                //doc.Save("c:\\test1212.xml");
                doc.Save(xmlPathName + GetXmlFileName(paketId) + ".xml");
            }
            finally
            {
                doc = null;
            }
        }

        public void InsertPaketXml(decimal paketId)
        {
            //XmlDocument doc = new XmlDocument();
            SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class objBL = new SQLXMLBULKLOADLib.SQLXMLBulkLoad4Class();
            
            try
            {
                //TODO: for now, we save xml to the file fisrt, before inserting it to the database
                //
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(paketXml);
                ////doc.Save("c:\\test1212.xml");
                //doc.Save(xmlPathName + GetXmlFileName(paketId));

                //doc = null;

                //SQLXMLBULKLOADLib.SQLXMLBulkLoad3Class objBL = new SQLXMLBULKLOADLib.SQLXMLBulkLoad3Class();

                //objBL.ConnectionString = "Provider=SQLOLEDB;" + cnnString + "Integrated Security=SSPI";
                objBL.ConnectionString = cnnStringBulk;

                //objBL.ErrorLogFile = "c:\\error.xml";
                objBL.ErrorLogFile = xmlPathName + GetXmlErrorFileName(paketId) + ".xml";
                objBL.Transaction = true;
                objBL.KeepIdentity = false;
                objBL.CheckConstraints = false;
                objBL.KeepNulls = true;

                //objBL.Execute("c:\\paket223schema.xml", "c:\\paket223.xml");

                //objBL.Execute("c:\\paket223schema.xml", "c:\\paket223.xml");
                objBL.Execute(xmlAnnSchemaPathName, xmlPathName + GetXmlFileName(paketId) +".xml");

                //status = true;
            }
            finally
            {
                //doc = null;
                objBL = null;
            }
            //catch (Exception ex)
            //{
            //    err = ex.Message;
            //    //throw ex;
            //}
            //return status;
            //string st = GetXmlAnnSchemaPathName().ToString() + GetXmlFileName(1).ToString();
        }

        public string GetPaketStatusXml(decimal paketId)
        {
            Microsoft.Data.SqlXml.SqlXmlCommand cmd = new Microsoft.Data.SqlXml.SqlXmlCommand(cnnStringBulk);
            XmlDocument xmldoc = new XmlDocument();

            string result = string.Empty;

            try
            {
                cmd.RootTag = "EfaPaketStatus";
                cmd.CommandText = "EfaPaket[Id = " + paketId.ToString() + "]";
                cmd.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.XPath;
                cmd.SchemaPath = xmlStatusAnnSchemaPathName;

                System.IO.Stream stream;
                //cnn.Open();
                stream = cmd.ExecuteStream();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }

                xmldoc.LoadXml(result);
                //cnn.Close();
                //xmldoc.Save("c:\\jajaj.xml");

                result = xmldoc.OuterXml;

            }
            finally
            {
                xmldoc = null;
                cmd = null;
            }
            return result;
        }

        //public string GetPaketStatusXml_2(decimal paketId)
        //{
        //    Microsoft.Data.SqlXml.SqlXmlCommand cmd = new Microsoft.Data.SqlXml.SqlXmlCommand(cnnStringBulk);
        //    XmlDocument xmldoc = new XmlDocument();

        //    string result = string.Empty;

        //    try
        //    {
        //        cmd.RootTag = "EfaPaketStatus";
        //        cmd.CommandText = "EfaPaket[Id = " + paketId.ToString() + "]";
        //        cmd.CommandType = Microsoft.Data.SqlXml.SqlXmlCommandType.XPath;
        //        cmd.SchemaPath = xmlStatusAnnSchemaPathName;

        //        System.IO.Stream stream;
        //        //cnn.Open();
        //        stream = cmd.ExecuteStream();

        //        using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
        //        {
        //            result = sr.ReadToEnd();
        //        }

        //        xmldoc.LoadXml(result);
        //        //cnn.Close();
        //        //xmldoc.Save("c:\\jajaj.xml");

        //        result = xmldoc.OuterXml;

        //    }
        //    finally
        //    {
        //        xmldoc = null;
        //        cmd = null;
        //    }
        //    return result;
          
        //}

        string GetXmlFileName(decimal paketId)
        {
            string fileName = "EfaPaket" + paketId.ToString() + "_" + DateTime.Today.ToShortDateString();
            return fileName;
        }

        string GetXmlErrorFileName(decimal paketId)
        {
            string fileName = "ERR_EfaPaket" + paketId.ToString() + "_" + DateTime.Today.ToShortDateString();
            return fileName;
        }

        string GetXmlAnnSchemaPathName()
        {
            string path = ConfigurationSettings.AppSettings["xmlAnnSchemaPathName"];
            if (path == null)
            {
                path = "c:\\paketannschema_test.xml";
            }
            return path;
        }
        string GetXmlStatusAnnSchemaPathName()
        {
            string path = ConfigurationSettings.AppSettings["xmlAnnStatusSchemaPathName"];
            if (path == null)
            {
                path = "c:\\paketstatusannschema_test.xml";
            }
            return path;
        }

        string GetXmlPathName()
        {
            string path = ConfigurationSettings.AppSettings["xmlPath"];
            if (path == null)
            {
                path = "c:\\";
            }
            return path;

        }

        string GetCnnString()
        {
            string cnnString = ConfigurationManager.AppSettings["cnnString"];

            //string cnnString = ConfigurationSettings.AppSettings.Get["cnnString"];
            if (cnnString == null)
            {
                cnnString = "Server=(local);" +
                                      "Database=efa2;" +
                                      "Persist Security Info=False;" +
                                      "Trusted_Connection=True;";

                //cnnString = "Provider=SQLOLEDB;" + lstCnnString + "Integrated Security=SSPI";
            }
            return cnnString;

        }
        string GetCnnStringBulk()
        {
            string cnnString = ConfigurationSettings.AppSettings["cnnStringBulk"];
            if (cnnString == null)
            {
                cnnString = "Server=(local);" +
                                      "Database=efa2;" +
                                      "Persist Security Info=False;" +
                                      "Trusted_Connection=True;";

                cnnStringBulk = "Provider=SQLOLEDB;" + cnnString + "Integrated Security=SSPI";
            }
            return cnnStringBulk;

        }

       public  void ValidateXml()
        { 
        


        }

       // Validation Error Count
       int ErrorsCount = 0;

       // Validation Error Message
       string ErrorMessage = "";

       public void ValidationHandler(object sender,
                                            System.Xml.Schema.ValidationEventArgs args)
       {
           ErrorMessage = ErrorMessage + args.Message + "\r\n";
           ErrorsCount++;
       }

       public int ValidateXml(string strXMLDoc)
       {
           int napake;
           try
           {
               // Declare local objects
               XmlTextReader tr = null;
               System.Xml.Schema.XmlSchemaCollection xsc = null;
               XmlValidatingReader vr = null;

               //System.IO.StreamReader sr = new System.IO.StreamReader("c:\\EfaPaket1.xsd");


               // Text reader object
               tr = new XmlTextReader("c:\\EfaPaket1.xsd");
               xsc = new System.Xml.Schema.XmlSchemaCollection();
               xsc.Add(null, tr);

               // XML validator object

               vr = new XmlValidatingReader(strXMLDoc,
                            XmlNodeType.Document, null);

               vr.Schemas.Add(xsc);

               // Add validation event handler

               vr.ValidationType = ValidationType.Schema;
               vr.ValidationEventHandler +=
                        new System.Xml.Schema.ValidationEventHandler(ValidationHandler);

               // Validate XML data

               while (vr.Read()) ;

               vr.Close();

               // Raise exception, if XML validation fails
               if (ErrorsCount > 0)
               {
                   throw new Exception(ErrorMessage);
               }

               // XML Validation succeeded
               //Console.WriteLine("XML validation succeeded.\r\n");
               napake = 0;
           }
           catch (Exception error)
           {
               // XML Validation failed
               //Console.WriteLine("XML validation failed." + "\r\n" +
               //"Error Message: " + error.Message);
               napake = ErrorsCount;
           }
           return napake;
       }
    }
}
