using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Schema;
using System.Reflection;

namespace TestGui
{
    public partial class Form1 : Form
    {
        //string path = "c:\\paket_test2.xml";
        string path = "";
        string paketXml = "";

        string paketStatus = "";
        //localhost.Service service;

        System.Drawing.Color normalbg = System.Drawing.Color.White;
        System.Drawing.Color normalfg = System.Drawing.Color.Black;
        System.Drawing.Color errorbg = System.Drawing.Color.Orange;
        System.Drawing.Color errorfg = System.Drawing.Color.Blue;
        System.Drawing.Color unchangedbg = System.Drawing.Color.Yellow;
        System.Drawing.Color unchangedfg = System.Drawing.Color.Blue;



        private void Form1_Load(object sender, EventArgs e)
        {
            string str;
            // Captura normal textbox colors:
            normalbg = this.textBoxXMLPath.BackColor;
            normalfg = this.textBoxXMLPath.ForeColor;

            // Set default values for text fields of the form:
            this.textBoxXMLPath.Text = "c:\\Users\\igogre\\Documents\\test.xml";
            if ((str = System.Configuration.ConfigurationManager.AppSettings
                        ["XMLPath"]).Length > 0)
                this.textBoxXMLPath.Text = str;

            this.txtPaketTip.Text = "1";
            this.txtPaketDatum.Text = "1.4.2008";
            this.txtStZapisov.Text = "1";
            this.txtVsotaDokumentDatum.Text = "1";
            this.txtVsotaCenaNeto.Text = "22.55";
            this.txtServiceUrl.Text = "http://localhost:1792/Service.asmx";

        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
        }

        private void btnPosljiPaket_Click(object sender, EventArgs e)
        {
            if (!InputOk())
            {
                MessageBox.Show("Enter required data");
                return;
            }
            StreamReader sr = new StreamReader(this.textBoxXMLPath.Text);
            paketXml = sr.ReadToEnd();
            
            PosljiPaket(byte.Parse(txtPaketTip.Text), DateTime.Now, int.Parse(txtStZapisov.Text), decimal.Parse(txtVsotaDokumentDatum.Text), decimal.Parse(txtVsotaCenaNeto.Text), paketXml);
        }

        private bool InputOk()
        {
            if (this.txtPaketDatum.Text == "") { return false; }
            if (this.txtPaketTip.Text == "") { return false; }
            if (this.txtStZapisov.Text == "") { return false; }
            if (this.txtVsotaCenaNeto.Text == "") { return false; }
            if (this.txtVsotaDokumentDatum.Text == "") { return false; }
            return true;
        }

        void PosljiPaket(byte paketTipId, DateTime paketDatum, int stZapisov, decimal vsotaDokumentDatum, decimal vsotaCenaNeto, string paketXml)
        {
            //MessageBox.Show("Si v funkcoiji");
            //return "narejeno";

            try
            {

                localhost.Service service = new TestGui.localhost.Service();
                //textBox8.Text= service.Url;
                service.Url = txtServiceUrl.Text  ;


                paketStatus = service.PosljiPaket(paketXml);
                //paketStatus = service.PosljiPaket_2(paketTipId, paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto, paketXml);
                //paketStatus = web.PosljiPaket(paketTipId, paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto, paketXml);
                System.Xml.XmlDataDocument doc = new System.Xml.XmlDataDocument();
                doc.LoadXml(paketStatus);
                txtResult.Clear();
                txtResult.Text = doc.OuterXml;
                MessageBox.Show("Paket poslan. Vrnjen xml!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Form2 testforma = new Form2();

            testforma.PaketTipId = byte.Parse(txtPaketTip.Text);
            testforma.PaketDatum = DateTime.Now;
            testforma.StZapisov = int.Parse(txtStZapisov.Text);
            testforma.VsotaDokumentDatum = decimal.Parse(txtVsotaDokumentDatum.Text);
            testforma.VsotaCenaNeto = decimal.Parse(txtVsotaCenaNeto.Text);

            testforma.PaketXml = paketXml;

            testforma.ShowDialog(this.FindForm());

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string initialfile = textBoxXMLPath.Text;   // get starting name from the dialog box
                OpenFileDialog dlgOpen = new OpenFileDialog();
                if (File.Exists(initialfile))
                {
                    // Set the initial state for the dialog box (starting dr. & file)
                    dlgOpen.FileName = initialfile;
                    dlgOpen.InitialDirectory = Path.GetDirectoryName(initialfile);
                }
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    path = dlgOpen.FileName.ToString();
                    textBoxXMLPath.Text = path;
                }
                if (!File.Exists(path) && path.Length>0)
                    MessageBox.Show("Datoteka ne obstaja: \n" + path);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                localhost.Service service = new TestGui.localhost.Service();
                service.Url = txtServiceUrl.Text;
                MessageBox.Show("OK");
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetPaketShema_Click(object sender, EventArgs e)
        {
            try
            {
                //--------------------------------------------------------------------

                localhost.Service service = new TestGui.localhost.Service();
                service.Url = txtServiceUrl.Text;

                Byte[] _paketShema = service.PaketShema();
                MemoryStream _ms = new MemoryStream(_paketShema);
                BinaryFormatter _bf = new BinaryFormatter();
                FileInfo _fi = _bf.Deserialize(_ms) as FileInfo;
                StreamReader _sr = _fi.OpenText();

                string result = _sr.ReadToEnd();
                _sr.Close();

                txtResult.Clear();
                txtResult.Text = result;

                MessageBox.Show("Shema paketa (PaketShema.xsd) prejeta!");

                //http://msdn.microsoft.com/en-us/library/system.xml.schema.xmlschemaobject(VS.80).aspx

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetPaketStatusShema_Click(object sender, EventArgs e)
        {
            try
            {
                localhost.Service service = new TestGui.localhost.Service();
                service.Url = txtServiceUrl.Text;

                Byte[] _paketStatusShema = service.PaketStatusShema();
                MemoryStream _ms = new MemoryStream(_paketStatusShema);
                BinaryFormatter _bf = new BinaryFormatter();
                FileInfo _fi = _bf.Deserialize(_ms) as FileInfo;
                StreamReader _sr = _fi.OpenText();

                string result = _sr.ReadToEnd();
                _sr.Close();

                txtResult.Clear();
                txtResult.Text = result;

                MessageBox.Show("Shema statusa paketa (PaketStatusShema.xsd) prejeta!");

                //http://msdn.microsoft.com/en-us/library/system.xml.schema.xmlschemaobject(VS.80).aspx

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPaketTipZV_Click(object sender, EventArgs e)
        {
            try
            {
                localhost.Service service = new TestGui.localhost.Service();
                service.Url = txtServiceUrl.Text;

                DataSet _ds = service.PaketTipZalogaVrednosti();
                //DataTable _dt = _ds.Tables[0];

                txtResult.Clear();
                txtResult.Text = _ds.GetXml();

                MessageBox.Show("Zaloge vrednosti tipov paketov so prenešene");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPaketStatusZV_Click(object sender, EventArgs e)
        {
            try
            {
                localhost.Service service = new TestGui.localhost.Service();
                service.Url = txtServiceUrl.Text;
                DataSet _ds = service.PaketStatusZalogaVrednosti();
                //DataTable _dt = _ds.Tables[0];
                txtResult.Clear();
                txtResult.Text = _ds.GetXml();
                MessageBox.Show("Zaloge vrednosti statusov paketov so prenešene");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPodrocjaZV_Click(object sender, EventArgs e)
        // Ajgor, 21.5.08
        {
            try
            {
                localhost.Service service = new TestGui.localhost.Service();
                service.Url = txtServiceUrl.Text;

                DataSet _ds = service.PodrocjaZalogaVrednosti();


                //DataTable _dt = _ds.Tables[0];
                txtResult.Clear();
                txtResult.Text = _ds.GetXml();
                MessageBox.Show("Zaloge vrednosti področij so prenešene");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPaketDatum_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxXMLPath_TextChanged(object sender, EventArgs e)
        {
            string str = this.textBoxXMLPath.Text;
            if (!File.Exists(str))
            {
                this.textBoxXMLPath.BackColor = errorbg;
                this.textBoxXMLPath.ForeColor = errorfg;
                MessageBox.Show("Opozorilo: Datoteka ne obstaja! \nIme datoteke: "+str);
            } else
            {
                this.textBoxXMLPath.BackColor = normalbg;
                this.textBoxXMLPath.ForeColor = normalfg;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }



        //[STAThread]
        //private void StartBrowseWSThread()
        //{
        //    // MessageBox.Show("This message is shown in a new thread!");
        //    Application.Run(new BrowseWS());
        //}

        //private void buttonDodatno_Click(object sender, EventArgs e)
        //{
        //    // Application.Run(new ajgor_dodatno());
        //    Thread t = new Thread(new ThreadStart(StartBrowseWSThread));
        //    t.Start();
        //}


        private void buttonBrowse_Click(object sender, EventArgs e)
        {

        }



    }
}
