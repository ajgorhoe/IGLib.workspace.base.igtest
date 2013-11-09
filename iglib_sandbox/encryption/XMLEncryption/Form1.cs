using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//required for XmlDocument
using System.Xml;
//required for performing RSA encryption
using System.Security.Cryptography;
//required to use EncryptedXml object, requires the System.Security assembly to be referenced
using System.Security.Cryptography.Xml;

/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */ 

namespace XMLEncryption
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// RSA asymmetric algorithim used to encrypt the session key, key is generated during the encryption process
        /// in this example a key is generated however in an actual application you will need to load and save the 
        /// keys as appropriate
        /// 
        /// </summary>
        RSACryptoServiceProvider rsa;

        /// <summary>
        /// 
        /// Loads the Xml into an Xml document and encrypts the full document from the root
        /// it does not cover selecting and encrypting individual nodes in the document
        /// however the place where this code would be placed is highlighted.
        /// 
        /// It also doesn't cover loading or saving the asymmetric keys, 
        /// the key is created in the encrypt method and because of the way the application works the same key is used to decrypt
        /// a new key is generated every time an encyption is ran
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check there is Xml to encrypt
            if (string.IsNullOrEmpty(this.txtXml.Text) == false)
            {
                try
                {
                    //1. load the xml in to a document, full document will be encrypted
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(this.txtXml.Text);

                    //2. Generate an asymmetric key (RSA is used), rsa object needs to have class level
                    //as it's used to decrypt the encrypted xml later in the class
                    rsa = new RSACryptoServiceProvider();
                    
                    //3. Select the element that will be encrypted, 
                    XmlElement xmlElemt;
                    xmlElemt = xmlDoc.DocumentElement;  //at this point you could use SelectSingleNode
                    //to select a specific node to encrypt

                    //4. Encrypt the element in the document using the EncryptedXml object,
                    //the EncryptedXml object is found in the System.Security.Cryptography.Xml namespace
                    //this namespace is not available without adding a reference to the System.Security assembly
                    EncryptedXml xmlEnc = new EncryptedXml(xmlDoc);
                    
                    //Add key name mapping is used to name the asymmetric RSA key used to encrypt the session key
                    //public rsa key is used to encrypt the key
                    xmlEnc.AddKeyNameMapping("asyncKey", rsa);

                    //EncryptedData object represents the <EncryptedData> element, at this point the <EncryptedData> element
                    //is not part of the document
                    EncryptedData encXml = xmlEnc.Encrypt(xmlElemt, "asyncKey");

                    //5. Replace use the static ReplaceElement method of the EncryptedData object to replace
                    //the element that was encrypted with the <EncryptedData> element
                    EncryptedXml.ReplaceElement(xmlElemt, encXml, false);

                    //display encrypted data and clear the xml to allow the decrypt to show the reverse
                    //as you can see the XmlDocument is updated with the replace, the original document has changed
                    this.txtEncryptedXml.Text = xmlDoc.OuterXml;
                    this.txtXml.Text = string.Empty;

                }
                catch (XmlException ex)
                {
                    //likely the xml is not well formed
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtEncryptedXml.Text) == false)
            {
                //1. Load the encrypted xml
                XmlDocument xmlEncDoc = new XmlDocument();
                xmlEncDoc.LoadXml(this.txtEncryptedXml.Text);

                //2. Create the encrypted xml and specify the same key, and key name, that was used to encrypt it
                //private rsa key is used here to decrypt the data
                EncryptedXml encXml = new EncryptedXml(xmlEncDoc);
                encXml.AddKeyNameMapping("asyncKey", rsa);
                
                //3. Decrypt the document <EncryptedData> elements containing the names key
                encXml.DecryptDocument();

                //xml document has been modified and now contains the decrypted xml
                this.txtXml.Text = xmlEncDoc.OuterXml;
                this.txtEncryptedXml.Text = string.Empty;
            }
        }


    }
}