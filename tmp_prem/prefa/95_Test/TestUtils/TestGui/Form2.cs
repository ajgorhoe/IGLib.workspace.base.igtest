using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using efakt1;

namespace TestGui
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        efakt1.WebServiceSimulator web;

        byte paketTipId;
        DateTime paketDatum;
        int stZapisov;
        decimal vsotaDokumentDatum;
        decimal vsotaCenaNeto;

        string paketXml;

        decimal paketId;
        string paketStatus;


        public byte PaketTipId
        {
            get
            {
                return paketTipId;
            }
            set
            {
                paketTipId = value;
            }
        }
        public DateTime PaketDatum
        {
            get
            {
                return paketDatum;
            }
            set
            {
                paketDatum = value;
            }
        }
        public int StZapisov
        {
            get
            {
                return stZapisov;
            }
            set
            {
                stZapisov = value;
            }
        }
        public decimal VsotaDokumentDatum
        {
            get
            {
                return vsotaDokumentDatum;
            }
            set
            {
                vsotaDokumentDatum = value;
            }
        }
        public decimal VsotaCenaNeto
        {
            get
            {
                return vsotaCenaNeto;
            }
            set
            {
                vsotaCenaNeto = value;
            }
        }
        public string PaketXml
        {
            get
            {
                return paketXml;
            }
            set
            {
                paketXml = value;
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            web = new efakt1.WebServiceSimulator();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                web.Connect();
                paketId = web.InsertIntoEfaPaket(paketTipId, paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto, 1);
                web.Disconnect();
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                web.Connect();
                web.SaveXmlToDisk(paketXml, paketId);
                web.Disconnect();
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                web.Connect();
                web.InsertPaketXml(paketId);
                web.Disconnect();
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                web.Connect();
                web.UpdateGlavaPostavke(paketId);
                web.Disconnect();
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                web.Connect();
                paketStatus = web.GetPaketStatusXml(paketId);
                web.Disconnect();
                System.Xml.XmlDataDocument doc = new System.Xml.XmlDataDocument();
                doc.LoadXml(paketStatus);
                textBox1.Text = doc.OuterXml;
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                //object[] arg = new object[] { paketTipId,paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto, paketXml };
                
                //Exception ex = new Exception();
                //object state = new object();

                //localhost.PosljiPaketCompletedEventArgs paket = new GUI_forma.localhost.PosljiPaketCompletedEventArgs(arg,ex, false, state);

                localhost.Service ser = new TestGui.localhost.Service();

                

                paketStatus = ser.PosljiPaket(paketXml);

                //paketStatus = web.PosljiPaket(paketTipId, paketDatum, stZapisov, vsotaDokumentDatum, vsotaCenaNeto, paketXml);
                System.Xml.XmlDataDocument doc = new System.Xml.XmlDataDocument();
                doc.LoadXml(paketStatus);
                textBox1.Text = doc.OuterXml;
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int napake = 0;
            try
            {
                web.Connect();
                napake = web.ValidateXml(paketXml);
                web.Disconnect();
                MessageBox.Show("napake: " + napake.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly ass;

            ass = System.Reflection.Assembly.LoadFrom(@"c:\DevProjects\eFakturiranje\EFA_Solution\95_Test\99_Deploy\GUI\PS.GlavnoOkno.UI.Base.dll");
            Type[] types = ass.GetTypes();
            
            ass = System.Reflection.Assembly.LoadFrom(@"c:\DevProjects\eFakturiranje\EFA_Solution\95_Test\99_Deploy\GUI\EFA.dll");
            types = ass.GetTypes();

            Type _type = ass.GetType("EFA.Paketi");
            _type = ass.GetType("EFA.mdlEfa");
        }


    }
}
