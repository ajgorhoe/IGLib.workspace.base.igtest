

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Threading;
using System.Text;
using System.IO;
using System.Xml;

using IG.Forms;

using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;



namespace IG.Crypt
{



    public class encryption_test
    {

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("\r\nWelcome to the IGForm's console test.\r\n\r\n");



            // Cerate a tet certificate with a test name etc.:
            Asym.CertSetDefault();
            Asym.CertValidFrom = "12/21/2001";
            //Asym.CertValidTo = "01/01/2022";
            Asym.CertFile = @"c:\Users\ajgorhoe\cvis\igcs\forms\00test\encryption\My_Test_Certificate.cer";

            Asym.CertCreate();


            //// In-memory symmetric encryption/decryption of a string:
            //SymStringEncrypt.Test();

            //// Storing data to symmetrically encrypted file and retrieving data from it:
            //SymFileEncrypt.Test();

            //// Hashing utilities:
            //HashUtilities.Test();

            //// Asyymmetric data encryption:
            //Asym.TestEncrypt("Perica reze raci rep.");


            //Import a pcks#12 certificate and its private keys (usual file ext. is pfx)
            // TODO - write a functio nwith arguments nam & pwd that calls the MainPfx in TestPfx!





            //// Some form teste:
            //new FadeMessage("Welcome to the console testing application.",3000);
            //new FadeMessage("Welcome to the console testing application.", 3000);
            //// Console form in a parallel thread:
            //ConsoleForm cons = new ConsoleForm("Testing console");
            //for (int i = 1; i <= 5; ++i)
            //{
            //    cons.WriteLine(i.ToString() + ".  vrstica");
            //}


            Thread.Sleep(1000);
            Console.WriteLine("\r\nBye.\r\n");
        }
    }


    



        //************************
        // Things in development:
        //************************




    // Create a certificte:
    // makecert -r -pe -sk Test_RSA_container -n "CN=Test_Certificate" -b 01/01/2000 -e 01/01/2015 -ss my -sr CurrentUser 
    //
    // Unused:  -sky exchange -sp "Microsoft Cryptographic Provider" 




    //********************************************************************
    // Import certificate form certificate store, get its keys & encrypt:
    //********************************************************************


    public class pfx_certificates
    {

        //************************************
        // Elementary tools for certificates:
        //************************************

        public static X509Certificate2 LoadCertificate(StoreName storeName, 
                    StoreLocation storeLocation, string certificateName)
        // Loads a certificate from a certificate store.
        {
            bool validOnly = false;
            
            X509Store store = new X509Store(storeName, storeLocation);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certificateCollection =
                   store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, validOnly);

                if (certificateCollection.Count > 0)
                {
                    //  We ignore if there is more than one matching cert, we just return the first one.
                    return certificateCollection[0];
                }
                else
                {
                    throw new ArgumentException("Certificate not found");
                }
            }
            finally
            {
                store.Close();
            }
        }

        public static byte[] DecryptByCert(byte[] encryptedData, bool fOAEP, 
                X509Certificate2 certificate)
        // Decrypts a file using a certificate, whose public key is extracted so that it
        // is first conveted to XML String that is imported into RSACryptoServiceProvider .
        {
             if (encryptedData == null)
             {
                 throw new ArgumentNullException("encryptedData");
             }
             if (certificate == null)
             {
                 throw new ArgumentNullException("certificate");
             }
             using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
             {
                 // Note that we use the private key to decrypt
                 provider.FromXmlString(GetXmlKeyPair(certificate));
         
                 return provider.Decrypt(encryptedData, fOAEP);
             }
         }

        public static byte[] EncryptByCert(byte[] plainData, bool fOAEP, X509Certificate2 certificate)
        {
            if (plainData == null)
            {
                throw new ArgumentNullException("plainData");
            }
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                // Note that we use the public key to encrypt
                provider.FromXmlString(GetPublicKey(certificate));

                return provider.Encrypt(plainData, fOAEP);
            }
        }

        public static string GetXmlKeyPair(X509Certificate2 certificate)
        // Returns certificate's private key as an XML string. That key can be imported to
        // RSACryptoServiceProvider via FromXmlString() class member function.
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            if (!certificate.HasPrivateKey)
            {
                throw new ArgumentException("the given certificate does not have a private key");
            }
            else
            {
                return certificate.PrivateKey.ToXmlString(true);
            }
        }

        public static string GetPublicKey(X509Certificate2 certificate)
        // Returns certificate's public key as an XML string. That key can be imported to
        // RSACryptoServiceProvider via FromXmlString() class member function.
        {
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            return certificate.PublicKey.Key.ToXmlString(false);
        }

        public static void UseCertificateDemo()
        // Demo that uses the above utilities:
        {
            X509Certificate2 cert = LoadCertificate(System.Security.Cryptography.X509Certificates.StoreName.My,
                System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine, "Test");
            byte[] encoded = System.Text.UTF8Encoding.UTF8.GetBytes("Encrypt me");
            byte[] encrypted = EncryptByCert(encoded, true, cert);
            byte[] decrypted = DecryptByCert(encrypted, true, cert);
            System.Console.Out.WriteLine(System.Text.UTF8Encoding.UTF8.GetString(decrypted));
        }








        //********************************************************************
        // Import certificate form certificate store, get its keys & encrypt:
        //********************************************************************


        // string DigitalCertificateName = "";
        /// <summary>
        /// Constructor
        /// Author : Ranajit Biswal
        /// Date : 24th May 2007
        /// Pupose : Used to Encrypt and Decrypt string using Digital certificate which has a Private Key.
        /// Requirement : WSE 2.0 and .Net Framework 2.0
        /// </summary>

        //Read digital certificate from Current User store.
        public string GetEncryptedText(string PlainStringToEncrypt, string DigitalCertificateName)
        {
            X509Store store1 = new X509Store(StoreName.TrustedPeople);
            X509Store store = new X509Store(StoreName.My);
            X509Certificate2 x509_2 = null;
            store.Open(OpenFlags.ReadWrite);
            if (DigitalCertificateName.Length > 0)
            {
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.SubjectName.Name.Contains(DigitalCertificateName))
                    {
                        x509_2 = cert;
                        break;
                    }
                }
                if (x509_2 == null)
                    throw new Exception("No Certificate could be found in name " + DigitalCertificateName);
            }
            else
            {
                x509_2 = store.Certificates[0];
            }

            try
            {
                string PlainString = PlainStringToEncrypt.Trim();
                byte[] cipherbytes = ASCIIEncoding.ASCII.GetBytes(PlainString);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509_2.PublicKey.Key;
                byte[] cipher = rsa.Encrypt(cipherbytes, false);
                return Convert.ToBase64String(cipher);
            }
            catch (Exception e)
            {
                //Hadle exception
                throw e;
            }

        }  // GetEncryptedText



        /// <summary>
        /// To Decrypt clear text using RSACryptoServer Provider and Digital Certificate having Private Key.
        /// </summary>
        /// <param name="EncryptedStringToDecrypt"></param>
        /// <returns></returns>
        public string GetDecryptedText(string EncryptedStringToDecrypt, string DigitalCertificateName)
        {
            X509Store store = new X509Store(StoreName.My);
            X509Certificate2 x509_2 = null;
            store.Open(OpenFlags.ReadWrite);
            if (DigitalCertificateName.Length > 0)
            {
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.SubjectName.Name.Contains(DigitalCertificateName))
                    {
                        x509_2 = cert;
                        break;
                    }
                }
                if (x509_2 == null)
                    throw new Exception("No Certificate could be found in name " + DigitalCertificateName);
            }
            else
            {
                x509_2 = store.Certificates[0];
            }

            try
            {
                byte[] cipherbytes = Convert.FromBase64String(EncryptedStringToDecrypt);
                if (x509_2.HasPrivateKey)
                {
                    RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509_2.PrivateKey;
                    byte[] plainbytes = rsa.Decrypt(cipherbytes, false);
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    return enc.GetString(plainbytes);
                }
                else
                {
                    throw new Exception("Certificate used for has no private key.");
                }
            }
            catch (Exception e)
            {
                //Hadle exception
                throw e;
            }
        } // GetDecryptedText()



 
        //******************************************************************
        // Decrypt data using asymmetric key form a certificate in a file:
        //******************************************************************

        // By using the X509Certificate2 class, I was able to read in the key 
        // [this reads in several key formats, by the way], then create the 
        // RSACryptoServiceProvider object and cast the .PrivateKey field into it, 
        // and perform the decryption.


        public static string DecryptEncryptedData(string Base64EncryptedData, string PathToPrivateKeyFile) {
            X509Certificate2 myCertificate;
            try{
                myCertificate = new X509Certificate2(PathToPrivateKeyFile);
            } catch{
                throw new CryptographicException("Unable to open key file.");
            }

            RSACryptoServiceProvider rsaObj;
            if(myCertificate.HasPrivateKey) {
                 rsaObj = (RSACryptoServiceProvider)myCertificate.PrivateKey;
            } else
                throw new CryptographicException("Private key not contained within certificate.");

            if(rsaObj == null)
                return String.Empty;

            byte[] decryptedBytes;
            try{
                decryptedBytes = rsaObj.Decrypt(Convert.FromBase64String(Base64EncryptedData), 
                    true);
            } catch {
                throw new CryptographicException("Unable to decrypt data.");
            }

            //    Check to make sure we decrpyted the string
           if(decryptedBytes.Length == 0)
                return String.Empty;
            else
                return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }




        //*****************************************************
        // Sign data and verify signature using a certificate:
        //*****************************************************

        //Author's comment:
        //Today I'm posting a sample which shows how to sign a text with a certificate 
        //in my Personal store (this cert will have public and private key associated to it)
        //and how to verify that signature with a .cer file (for i.e. WinForms) applications 
        // or a client certificate (for i.e. ASP.NET) (both will only have public key 
        // associated to them).

        static byte[] Sign(string text, string certSubject)
        {
            // Access Personal (MY) certificate store of current user
            X509Store my = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);

            // Find the certificate we'll use to sign            
            RSACryptoServiceProvider csp = null;
            foreach (X509Certificate2 cert in my.Certificates)
            {
                if (cert.Subject.Contains(certSubject))
                {
                    // We found it. 
                    // Get its associated CSP and private key
                    csp = (RSACryptoServiceProvider)cert.PrivateKey;
                }
            }
            if (csp == null)
            {
                throw new Exception("No valid cert was found");
            }

            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = sha1.ComputeHash(data);

            // Sign the hash
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }

        static bool Verify(string text, byte[] signature, string certPath)
        {
            // Load the certificate we'll use to verify the signature from a file 
            X509Certificate2 cert = new X509Certificate2(certPath);
            // Note: 
            // If we want to use the client cert in an ASP.NET app, we may use something like this instead:
            // X509Certificate2 cert = new X509Certificate2(Request.ClientCertificate.Certificate);

            // Get its associated CSP and public key
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PublicKey.Key;

            // Hash the data
            SHA1Managed sha1 = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = sha1.ComputeHash(data);

            // Verify the signature with the hash
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }

        static void CertSignTest()
        {
            // Usage sample
            try
            {
                // Sign text
                byte[] signature = Sign("Test", "cn=my cert subject");

                // Verify signature. Testcert.cer corresponds to "cn=my cert subject"
                if (Verify("Test", signature, @"C:\testcert.cer"))
                {
                    Console.WriteLine("Signature verified");
                }
                else
                {
                    Console.WriteLine("ERROR: Signature not valid!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
            }
            Console.ReadKey();
        }


    }  // class pfx_encryption




    public class pfx_tests  // OBSOLETE, see class pfx_certificates instead.
    // Import a X509 Certificate from a PFC file (=PKCS#12) and explore its abilities.
    {
        static void TestPFX1()
        //************************************************
        // Import certificate form file, get its keys:
        //************************************************
        {
            string pfxFile = null, pfxPwd = null;

            //AsymmetricAlgorithm privateKey = new X509Certificate2(pfxFile,
            //    password).PrivateKey;


            X509Certificate2 cert = new X509Certificate2(pfxFile,
                    pfxPwd);
            // IMPORT A CERTIFICATE FORM A FILE!
            cert.Import(pfxFile);

            // cert.PublicKey.Oid.Friendlt

            // byte[] privatekey = cert.GetPrivateKey();
            // Get the public key from a certificate:
            byte[] publickey = cert.GetPublicKey();
            // Get algorithm and algorithmparameters form a certificate (don't know 
            //    exactly what to do with them):
            //string key = cert.GetKeyAlgorithm();
            //byte[] keyparam = cert.GetKeyAlgorithmParameters();

            // Get a private key form a PFX file:
            string decryptDataInBase64 = "My Text.";
            X509Certificate2 myCert2 = new X509Certificate2(@"C:\mycerts\myprivatekeyfile.pfx");
            RSACryptoServiceProvider rsa1 = (RSACryptoServiceProvider)myCert2.PrivateKey;
            byte[] plain = rsa1.Decrypt(Convert.FromBase64String(decryptDataInBase64), false);
            MessageBox.Show(System.Text.Encoding.UTF8.GetString(plain));


        }
    }  // class pfx_tests 







}
