using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;



using IG.Forms;

using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;

namespace IG.Crypt
{

    // Notes:
    // SHA-1 hashing alg. is commonly used to create MAC (Message Authentication Codes) -> HMAC-SHA-1


    public class Asym
    {

        /* REMARKS
          Asymmetric encryption can be applied to byte arrays of limited length (limited by key size),
          longer data must be encrypted by segments.
         * Windows tools:
              directory: c:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\
              sn.exe (strong name) - key management, signature generation, and signature verification
              makecert.exe - Creates a certificate.
                    example: makecert -r -pe -n "CN=MyTestServer" -b 01/01/2000 -e 01/01/2036 -eku 1.3.6.1.5.5.7.3.1 -ss my -sr CurrentUser -sky exchange -sp "Microsoft RSA SChannel Cryptographic Provider" -sy 12
              certmgr - certificate manager (imports, exports)
              signtool - signing of files
              
         * To Clear: 
                PKCS#12 format - impossible to export in this format? (even certmgr enables only import, not export)
                    Microsoft extension for .p12 files = .PFX !
                How to create RSA crypto provider from a certificate obtained in a file??
         * Create a certificate for testing:
                
            // Create a certificte for testing:
            // makecert -r -pe -sk Test_RSA_container -n "CN=Test_Certificate" -b 01/01/2000 -e 01/01/2015 -ss my -sr CurrentUser 
            //
            // Unused:  -sky exchange -sp "Microsoft Cryptographic Provider" 

        */



                /******************/
                /*                */
                /*  TRUSTED CODE  */
                /*                */
                /******************/


            /******************/
            /*  Certificates  */
            /******************/



        public static X509Certificate2 LoadCertificate(string certificateName ,StoreName storeName,
                    StoreLocation storeLocation, bool validOnly)
        /// Loads a certificate from a certificate store.
        {
            X509Store store = new X509Store(storeName, storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificateCollection =
                   store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, validOnly);
                if (certificateCollection.Count > 0)
                {
                    if (certificateCollection.Count > 1)
                        throw new Exception("There are more than one certificates satisfying the conditon.");
                    else
                        return certificateCollection[0];
                }
                else
                {
                    throw new Exception("Certificate not found.");
                }
            }
            finally
            {
                store.Close();
            }
        }

        public static X509Certificate2 LoadCertificate(string certificateName)
        {
            return LoadCertificate(certificateName, StoreName.My, 
                StoreLocation.CurrentUser, true);
        }

        RSACryptoServiceProvider CertPrivateKey(X509Certificate2 cert)
        {
            if (cert == null)
                throw (new ArgumentException("Certificate is not specified."));
            if (cert.HasPrivateKey)
            {
                RSACryptoServiceProvider ret = (RSACryptoServiceProvider)cert.PrivateKey;
                return ret;
            }
            else
            {
                throw new Exception("Certificate has no private key.");
            }
        }

        RSACryptoServiceProvider CertPublicKey(X509Certificate2 cert)
        {
            if (cert == null)
                throw (new ArgumentException("Certificate is not specified."));
            RSACryptoServiceProvider ret = (RSACryptoServiceProvider)cert.PrivateKey;
            return ret;
        }




        /*******************************/
        /*  Creation of certificates:  */
        /*******************************/


        public static string
            TestContainerName = "Test_RSA_container",
            TestCertificateName = "Test_Certificate";

        // Parameters that are assumed for certificate generation:
        public static string
            CertName = "Test_Certificate",  /// certificate name in the storage
            CertStorage = "my", /// storage name
            CertContainer = "Test_RSA_container",  /// key container name
            CertCommand = "makecert.exe", /// system command to call the certificate maker
            CertFile = null,  /// output file (should be .cert) to which certificate is exported
            CertValidFrom = null,  // Beginning of validity, default current date
            CertValidTo = null;  // End of validity, default is beginning + some time
        public static bool
            CertUserStore = true,    // if true then certificate is stored in the user store, otherwise
            // it is storesd in the machine store.
            CertRepConsole = true,  // If true then report is output to the standard console
            CertRepForm = false;  // If true then report is output to application window console


        public static void CertSetDefault()
        {
            CertName = "Test_Certificate";
            CertStorage = "my";
            CertContainer = "Test_RSA_container";
            CertCommand = "makecert.exe";
            CertFile = null;  // not output
            CertValidFrom = null;  // valid from current date
            CertValidTo = null;  // valis until current date + some time
            CertUserStore = true; // stored in user store, not accessible to other users
            CertRepConsole = true; // write a report to the standard output console
            CertRepForm = false;  // write a report to the Windows based application console
        }

        public static void CertCreate()
        // Creates a certificate by using parameters residing in static variables (that can
        // be changed prior to this call)
        {
            CreateTestCertificate(CertName, CertStorage,
                CertUserStore, CertContainer, CertCommand, CertRepConsole,
                CertRepForm, CertFile, CertValidFrom, CertValidTo );
        }



        public static void CreateTestCertificate(string CertificateName, string StorageName,
                bool UserStore, string ContainerName, string CommandPath, bool ReportConsole,
                bool ReportForm, string OutputFile,string ValidFrom, string ValidTo)
        /// Creates a new certificate named CertificateName, with keys stored in the container
        /// named ContainerName, and stored in the storage denoted by StorageName. If
        /// UserStore==true then the certificate is stored in the current user's store,
        /// otherwise it is stored in the machine store. If command path is a string of
        /// non-zero length then it specified the path of the makecert system command.
        /// If ReportConsole==true then reporting is performed on standard console. If 
        /// ReportForm is true then reporting is performed to IGForm.Console.
        /// OutputFile ia the name of the .cer file where the X.509 certificate will be written.
        {
            try
            {
                string CommandArgStr = "";
                string compath = "makecert.exe";
                if (CommandPath != null) if (CommandPath.Length > 0) compath = CommandPath;
                string certname = Asym.TestCertificateName;
                if (CertificateName != null) if (CertificateName.Length > 0) certname = CertificateName;
                string contname = null;
                if (ContainerName != null) if (ContainerName.Length > 0) contname = ContainerName;
                if (contname == null)
                {  // take the default container name
                    if (certname.CompareTo(Asym.TestCertificateName) == 0)
                        contname = Asym.TestContainerName;
                    else
                        contname = certname;
                }
                string storage = "my";
                if (StorageName != null) if (StorageName.Length > 0) storage = StorageName;
                string storelocation;
                if (UserStore) storelocation = "currentuser"; else storelocation = "machine";
                // Specify teh validity interval:
                string from, to;
                DateTime begin = DateTime.Now, end = DateTime.Now;
                TimeSpan period = new TimeSpan(365 * 5, 0, 0, 0);
                end = end.Add(period);
                from = String.Format("{0:D2}/{1:D2}/{2:D}", begin.Day, begin.Month, begin.Year);
                to = String.Format("{0:D2}/{1:D2}/{2:D}", end.Day, end.Month, end.Year);
                if (ValidFrom != null)
                    if (ValidFrom.Length > 0)
                        from = ValidFrom;
                if (ValidTo != null)
                    if (ValidTo.Length > 0)
                        to = ValidTo;
                string issuername = "Asym_Class_Crypto_Lib";
                CommandArgStr += " -r -pe -sk " + contname
                    + " -n \"CN=" + certname
                    + "\" -b " + from + " -e " + to
                    + " -ss " + storage + " -sr " + storelocation;
                //if (issuername!=null)
                //    if (issuername.Length>0)
                //        CommandArgStr += " -in " + issuername ;
                if (OutputFile != null)
                    if (OutputFile.Length > 0)
                        CommandArgStr += " " + OutputFile;
                if (ReportConsole)
                {
                    Console.WriteLine("\n\nCreating a certificate with the following parameters:");
                    Console.WriteLine("  Cert. name:         " + certname);
                    Console.WriteLine("  Storage name:       " + storage);
                    Console.WriteLine("  Storage location:   " + storelocation);
                    Console.WriteLine("  Key container name: " + contname);
                    Console.WriteLine("  Valid from:         " + from);
                    Console.WriteLine("  Valid to:           " + to);
                    Console.WriteLine("  Issued by:          " + issuername);
                    Console.WriteLine("Command string: \n  " + compath + CommandArgStr + "\n");
                }
                if (ReportForm)
                {
                    UtilForms.WriteLine("\n\nCreating a certificate with the following parameters:");
                    UtilForms.WriteLine("  Cert. name:         " + certname);
                    UtilForms.WriteLine("  Storage name:       " + storage);
                    UtilForms.WriteLine("  Storage location:   " + storelocation);
                    UtilForms.WriteLine("  Key container name: " + contname);
                    UtilForms.WriteLine("  Valid from:         " + from);
                    UtilForms.WriteLine("  Valid to:           " + to);
                    UtilForms.WriteLine("  Issued by:          " + issuername);
                    UtilForms.WriteLine("Command string: \n  " + compath + CommandArgStr + "\n");
                }
                // Launch certificate maker via external command:
                System.Diagnostics.Process.Start(compath, CommandArgStr);
                if (ReportConsole)
                {
                    Console.WriteLine("\nCertificate created.\n\n");
                }
                if (ReportForm)
                {
                    UtilForms.WriteLine("\nCertificate created.\n\n");
                }
            }
            catch (Exception e)
            {
                if (ReportConsole)
                    Console.WriteLine("\n\nERROR in certificate creation:\n"+e.Message);
                if (ReportForm)
                    UtilForms.ReportError(e,"Problem in Certificate creation.");
                throw;
            }
        }

        public static void CreateTestCertificate(string CertificateName, string StorageName,
                bool UserStore, string ContainerName, string CommandPath, bool ReportConsole,
                bool ReportForm, string OutputFile)
        {
            CreateTestCertificate(CertificateName, StorageName,
                UserStore, ContainerName, CommandPath, ReportConsole,
                ReportForm, OutputFile, null /* ValidFrom */, null /* ValidTo */ );
        }

        // Overloads for CreateTestCertificate: 
        public static void CreateTestCertificate(string CertificateName, string StorageName,
                bool UserStore, string ContainerName, string CommandPath)
        {
            CreateTestCertificate(CertificateName, StorageName,
                UserStore, ContainerName, CommandPath, 
                true /* report to console */, false, /* report to a windows console */
                null /* output file */ );
        }
       
        public static void CreateTestCertificate(string CertificateName, string StorageName,
                bool UserStore, string ContainerName)
        {
            CreateTestCertificate(CertificateName, StorageName,
                UserStore, ContainerName, null /* default command path */,
                true /* report to console */, false, /* report to a windows console */
                null /* output file */ );
        }
       
        public static void CreateTestCertificate(string CertificateName)
        {
            CreateTestCertificate(CertificateName, null /* StorageName */,
                true /* UserStore */, null /* ContainerName */, null /* default command path */,
                true /* report to console */, false, /* report to a windows console */
                null /* output file */ );
        }
       
        public static void CreateTestCertificate()
        {
            CreateTestCertificate(null /* CertificateName */, null /* StorageName */,
                true /* UserStore */, null /* ContainerName */, null /* default command path */,
                true /* report to console */, false, /* report to a windows console */
                null /* output file */ );
        }




        /*************************************/
        /*  TRUSTED CODE under development:  */
        /*************************************/


                /*********************************/
                /*                               */
                /*  CODE TO BE FURTHER VERIFIED  */
                /*                               */
                /*********************************/




        //**********************************************************
        // Create or load & dleete keys from system key containers:
        //**********************************************************


        public static void LoadKeys(string containername) 
        // A method to create an RSACryptoServiceProvider and load keys from
        // a named CryptoAPI key containername if they exist; otherwise, the 
        // RSACryptoServiceProvider automatically generates new keys and 
        // persists them to the named key containername for future use.
        {
            // Create a new CspParameters object and set its KeyContainerName
            // field to the name of the specified containername.
            System.Security.Cryptography.CspParameters cspParams =
                new System.Security.Cryptography.CspParameters();
            cspParams.KeyContainerName = containername;

            // Create a new RSA asymmetric algorithm and pass the CspParameters
            // object that specifies the key containername details.
            using (System.Security.Cryptography.RSACryptoServiceProvider rsaAlg =
             new System.Security.Cryptography.RSACryptoServiceProvider(cspParams))
            {

                // Configure the RSACryptoServiceProvider object to persist
                // keys to the key containername.
                rsaAlg.PersistKeyInCsp = true;

                // Display the public keys to the console.
                System.Console.WriteLine(rsaAlg.ToXmlString(false));

                // Because the RSACryptoServiceProvider object is configured to  
                // persist keys, the keys are stored in the specified key containername.
            }
        }


        public static void DeleteKeys(string containername) 
        // A method to create an RSACryptoServiceProvider and clear existing
        // keys from a named CryptoAPI key containername.
        {
            // Create a new CspParameters object and set its KeyContainerName
            // field to the name of the containername to be cleared.
            System.Security.Cryptography.CspParameters cspParams =
                new System.Security.Cryptography.CspParameters();
            cspParams.KeyContainerName = containername;

            // Create a new RSA asymmetric algorithm and pass the CspParameters
            // object that specifies the key containername details.
            using (System.Security.Cryptography.RSACryptoServiceProvider rsaAlg =
             new System.Security.Cryptography.RSACryptoServiceProvider(cspParams))
            {

                // Configure the RSACryptoServiceProvider object not to persist
                // keys to the key containername.
                rsaAlg.PersistKeyInCsp = false;

                // Display the public keys to the console. Because we call 
                // Dispose() after this call, existing keys will not appear to 
                // change until the second time the method is called.
                System.Console.WriteLine(rsaAlg.ToXmlString(false));

                // As the code leaves this "using" block, Dispose is called on 
                // the RSACryptoServiceProvider object. Because the object is 
                // configured NOT to persist keys, the associated key containername 
                // is cleared. Instead of Dispose(), calling rsaAlg.Clear() would 
                // have the same effect, as it indirectly calls Dispose().
            }
        }




        //***********************************************
        // Encrypt & decrypt data using asymmetric keys:
        //***********************************************


        private static byte[] EncryptMessage(byte[] plaintext, 
            RSAParameters rsaParams) 
        // RSA encrypts a message using the PUBLIC KEY
        // contained in an RSAParameters structure. 
        {
            
            byte[] ciphertext = null;
            
            // create an instance of the RSA algorithm.
            using (RSACryptoServiceProvider rsaAlg = 
                new RSACryptoServiceProvider()) 
            {
            
                rsaAlg.ImportParameters(rsaParams);

                // encrypt the plaintext using OAEP padding, which
                // is only supported on Windows XP and later versions
                // of Windows.
                ciphertext = rsaAlg.Encrypt(plaintext, true);
            }
            
            // Clear the values held in the plaintext byte array. This ensures 
            // that the secret data does not sit in memory after you release 
            // your reference to it. 
            Array.Clear(plaintext, 0, plaintext.Length);
            
            return ciphertext;
        }


        // A method to decrypt an RSA encrypted message using the PRIVATE KEY
        // from the key containername specified by the CspParameters object.
        private static byte[] DecryptMessage(byte[] ciphertext, 
            CspParameters cspParams ) 
        {

            // Declare a byte array to hold the decrypted plaintext.
            byte[] plaintext = null;
            
            // create an instance of the RSA algorithm.
            using (RSACryptoServiceProvider rsaAlg = 
            new RSACryptoServiceProvider(cspParams)) 
            {
                // decrypt the plaintext using OAEP padding.
                plaintext = rsaAlg.Decrypt(ciphertext, true);
            }
            
            return plaintext;
        }


        //****************************
        // Export keys from/to files:
        //*****************************

        //********************************
        // Export keys from/to XML files:
        //********************************

        private static RSACryptoServiceProvider rsaProvider;

        public static void AssignParameter(string ContainerName)
        // Assign keys from a key container named ContainerName to rsaProvider.
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "DefaultTestingKey";
            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            if (ContainerName!=null)
                if (ContainerName.Length>0)
                    cspParams.KeyContainerName = ContainerName;
            // cspParams.Flags = CspProviderFlags.UseMachineKeyStore;  // This uses key store accessible to all users of the machine
            // cspParams.Flags = CspProviderFlags.UseUserProtectedKey;  // This uses key store accessible only to current user, with prompt to retrieve the keys
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            rsaProvider = new RSACryptoServiceProvider(cspParams);
        }


        public static void AssignNewKey(string ContainterName, string PrivateKeyFileName, string PublicKeyFileName)
        {
            AssignParameter(ContainterName);

            //provide public and private RSA params
            //StreamWriter writer = new StreamWriter(@"C:\Inetpub\wwwroot\dotnetspiderencryption\privatekey.xml");
            StreamWriter writer = new StreamWriter(PrivateKeyFileName);
            string publicPrivateKeyXML = rsaProvider.ToXmlString(true);
            writer.Write(publicPrivateKeyXML);
            writer.Close();

            //provide public only RSA params
            writer = new StreamWriter(PublicKeyFileName);
            string publicOnlyKeyXML = rsaProvider.ToXmlString(false);
            writer.Write(publicOnlyKeyXML);
            writer.Close();
        }


        public static string EncryptData(string ContainterName, string data2Encrypt, string XMLPublicKeyFileName)
        {
            AssignParameter(ContainterName);

            StreamReader reader = new StreamReader(XMLPublicKeyFileName);
            string publicOnlyKeyXML = reader.ReadToEnd();
            rsaProvider.FromXmlString(publicOnlyKeyXML);
            reader.Close();

            //read plaintext, encrypt it to ciphertext

            byte[] plainbytes = System.Text.Encoding.UTF8.GetBytes(data2Encrypt);
            byte[] cipherbytes = rsaProvider.Encrypt(plainbytes, false);
            return Convert.ToBase64String(cipherbytes);
        }

        public static string DecryptData(string ContainterName, string data2Decrypt, string PrivateKeyFilename)
        {
            AssignParameter(ContainterName);

            byte[] getpassword = Convert.FromBase64String(data2Decrypt);

            StreamReader reader = new StreamReader(PrivateKeyFilename);
            string publicPrivateKeyXML = reader.ReadToEnd();
            rsaProvider.FromXmlString(publicPrivateKeyXML);
            reader.Close();

            //read ciphertext, decrypt it to plaintext
            byte[] plain = rsaProvider.Decrypt(getpassword, false);
            return System.Text.Encoding.UTF8.GetString(plain);

        }



        //***************************
        // XML Encryption & Signing:
        //***************************



        public static void SignXml(XmlDocument Doc, RSA Key)
        // Sign an XML file; From MSDN.
        // This document cannot be verified unless the verifying 
        // code has the key with which it was signed. Private key is used to sign the document, and
        // public to verify the signature.
        {
            // Check arguments.
            if (Doc == null)
                throw new ArgumentException("XML Document not specified.");
            if (Key == null)
                throw new ArgumentException("RSA Key not specified.");

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(Doc);

            // Add the key to the SignedXml document.
            signedXml.SigningKey = Key;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            Doc.DocumentElement.AppendChild(Doc.ImportNode(xmlDigitalSignature, true));

        }


        public static Boolean VerifyXml(XmlDocument Doc, RSA Key)
        // Verify the signature of an XML file against an asymmetric 
        // algorithm and return the result.
        {

            // Check arguments.
            if (Doc == null)
                throw new ArgumentException("XML Document not specified.");
            if (Key == null)
                throw new ArgumentException("RSA Key not specified.");

            // Create a new SignedXml object and pass it
            // the XML document class.
            SignedXml signedXml = new SignedXml(Doc);

            // Find the "Signature" node and create a new
            // XmlNodeList object.
            XmlNodeList nodeList = Doc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for
            // the entire XML document.  Throw an exception 
            // if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.  
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature(Key);
        }



        
        //********
        // Demos:
        //********


        public static void TestXML(String[] args)
        {
            try
            {

                // **** Sign an XML document

                // TODO: set up proper parameters such as XML file name and the key container name!

                // Create a new CspParameters object to specify
                // a key container.
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = TestContainerName;

                // Create a new RSA signing key and save it in the container. 
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);

                // Create a new XML document.
                XmlDocument xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("test.xml");

                // Sign the XML document. 
                SignXml(xmlDoc, rsaKey);

                Console.WriteLine("XML file signed.");

                // Save the document.
                xmlDoc.Save("test.xml");



                // **** Verify the signature of an XML document:

                // Create a new CspParameters object to specify
                // a key container.
                // IMPORTANT: the same key must be used as for signature.
                cspParams = new CspParameters();
                cspParams.KeyContainerName = TestContainerName;

                // Create a new RSA signing key and save it in the container. 
                rsaKey = new RSACryptoServiceProvider(cspParams);

                // Create a new XML document.
                xmlDoc = new XmlDocument();

                // Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("test.xml");

                // Verify the signature of the signed XML.
                Console.WriteLine("Verifying signature...");
                bool result = VerifyXml(xmlDoc, rsaKey);

                // Display the results of the signature verification to 
                // the console.
                if (result)
                {
                    Console.WriteLine("The XML signature is valid.");
                }
                else
                {
                    Console.WriteLine("The XML signature is not valid.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }







        public static void TestEncrypt (string message)
        {

            // Declare an RSAParameters variable, which will contain the
            // PUBLIC KEY information for the target recipient of the 
            // encrypted message.
            RSAParameters recipientsPublicKey;

            // Declare a CspParameters variable, which will identify the 
            // key containername in which the recipients PRIVATE KEY is stored.
            // Normally, only the recipient would have access to this 
            // information. In order to create the example, we create
            // the RSA key pair at the beginning of the example and share
            // the keys between the sending and recieving sides of the 
            // exchange.
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "MyKeys";

            // For the purpose of this example, generate a new asymmetric 
            // key pair using the RSACryptoServiceProvider class. Store the
            // keys in a key store named "MyKeys" and extract the public key
            // information to the recipientsPublicKey variable for use in the
            // example.
            using (RSACryptoServiceProvider rsaAlg =
                new RSACryptoServiceProvider(cspParams))
            {

                // Configure the algorithm to persist keys to the key containername.
                rsaAlg.PersistKeyInCsp = true;

                // Extract the public key, which will cause the keys to be generated.
                recipientsPublicKey = rsaAlg.ExportParameters(false);
            }

            // Display the original plaintext message.
            Console.WriteLine("Original message = {0}", message);

            // Convert the original message to a byte array. It is best to not
            // pass secret information as strings between methods.
            byte[] plaintext = Encoding.Unicode.GetBytes( message );

            // Encrypt the message using the EncryptMessage method. The 
            // EncryptMessage method requires the PUBLIC KEY of the person
            // to which we are sending the message.
            byte[] ciphertext = EncryptMessage(plaintext, recipientsPublicKey);

            // Display the ciphertext returned by the EncryptMessage method. 
            // Use the BitConverter.ToString method for simplicity although 
            // this inserts hyphens (-) between byte values, which is not
            // true to the in-memory representation of the data.
            Console.WriteLine("Formatted Ciphertext = {0}",
                BitConverter.ToString(ciphertext));

            // Decrypt the encryted message using the DecryptMessage method. 
            // The DecryptMessage method requires access to the recipients 
            // PRIVATE KEY, which only the recipient should have access to. 
            // We pass a CspParameters object, which identifies to the 
            // RSACryptoServiceProvider which key containername the PRIVATE KEY 
            // is in. This provides a more secure solution than passing the 
            // raw PRIVATE KEY between methods.
            byte[] decData = DecryptMessage(ciphertext, cspParams);

            // Convert the decrypted message from a byte array to a string
            // and display it to the console.
            Console.WriteLine("Decrypted message = {0}",
                Encoding.Unicode.GetString(decData));

            // Wait to continue.
            Console.ReadLine();
        }




    }




    public class HashUtilities
    {
        public enum HashAlgorithms { MD5, SHA, SHA1, SHA256, SHA384 };

        static byte[] ComputeHash(string HashAlgorithmName, string Str)
        {
            // create a byte array from the string
            byte[] Data = Encoding.Default.GetBytes(Str);
            // create an instance of the hashing algorithm
            HashAlgorithm HashAlg = HashAlgorithm.Create(HashAlgorithmName);
            // obtain the hash code from the HashAlgorithm by 
            // using the ComputeHash method
            byte[] xHashCode = HashAlg.ComputeHash(Data);
            return xHashCode;
        }

        static string ComputeHashHexString(string HashAlgorithmName, string Str)
        {
            return HashToHexString(ComputeHash(HashAlgorithmName,Str));
        }

        static string HashToHexString(byte[] xHashCode )
        {
            StringWriter strw = new StringWriter();
            foreach (byte b in xHashCode)
            {
                strw.Write("{0:X2} ", b);
            }
            return strw.ToString();
        }

        public static byte[] ToASCIIByteArray(string characters)
        /// Converts string to an AscII Byte array.
        {
            ASCIIEncoding encoding = new ASCIIEncoding( );
            int numberOfChars = encoding.GetByteCount(characters);
            byte[] retArray = new byte[numberOfChars];
            retArray = encoding.GetBytes(characters);
            return (retArray);
        }

        public static byte[] ToUnicodeByteArray(string characters)
        /// Converts string to an Unicode Byte array.
        {
            UnicodeEncoding encoding = new UnicodeEncoding( );
            int numberOfChars = encoding.GetByteCount(characters);
            byte[] retArray = new byte[numberOfChars];
            retArray = encoding.GetBytes(characters);
            return (retArray);
        }

        public static string FromASCIIByteArray(byte[] characters)
        // Converts an ASCII byte arrray to a sting
        {
            ASCIIEncoding encoding = new ASCIIEncoding( );
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        public static string FromUnicodeByteArray(byte[] characters)
        // Converts an Unicode byte arrray to a sting
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        


        private static byte[] ParseHashCodeString (string p_hash_code_string)
        {
            // split the hash code on spaces
            string[] x_elements = p_hash_code_string.Split(' ');
            // create a byte array to hold the elements
            byte[] x_hash_code_array = new byte[x_elements.Length];
            // parse each string element into a byte
            for (int i = 0; i < x_elements.Length; i++)
            {
                x_hash_code_array[i] = Byte.Parse(x_elements[i],
                    System.Globalization.NumberStyles.HexNumber);
            }
            // return the byte array
            return x_hash_code_array;
        }

        private static bool CompareHashCodes(byte[] x_hash_code1,
            byte[] x_hash_code2)
        {

            // check that the hash codes are the same length
            if (x_hash_code1.Length == x_hash_code2.Length)
            {
                // run through the hash code and check 
                // each value in turn
                for (int i = 0; i < x_hash_code1.Length; i++)
                {
                    if (x_hash_code1[i] != x_hash_code2[i])
                    {
                        // the byte at this location is different
                        // in each hash code
                        return false;
                    }
                }
                // the hash codes are the same
                return true;
            }
            else
            {
                // the hash codes contain different numbers of
                // bytes and so cannot be the same
                return false;
            }
        }

        public static bool ValidateHashCode(string p_hash_algorithm,
            string p_hash_string, byte[] p_data)
        {

            // parse the hash code string into a byte array
            byte[] x_hash_code = ParseHashCodeString(p_hash_string);

            // create the hashing algorithm object using the 
            // name argument
            HashAlgorithm x_hash_alg = HashAlgorithm.Create(p_hash_algorithm);

            // compare the hash codes
            return CompareHashCodes(x_hash_code, x_hash_alg.ComputeHash(p_data));
        }

        public static void Test()
        {
            string data = "Perica reže raci rep.";
            string hashalg="MD5";
            Console.WriteLine("Data: {0}", data);
            string hexhash=HashUtilities.ComputeHashHexString(hashalg,data);
            Console.WriteLine("Hash code of the data: {0}", hexhash);
            // Converting a hash code to a sting that represent a byte array:
            byte[] hashbytes;
            string stringhash;
            hashbytes=HashUtilities.ParseHashCodeString(hexhash);
            stringhash = HashUtilities.FromUnicodeByteArray(hashbytes);
            Console.WriteLine("Hash code converted to a stirng: {0}",stringhash);



        }


    } // HashUtilities



    public sealed class SymStringEncrypt
    {
        private SymStringEncrypt( ) {}

        private static byte[] savedKey = null;
        private static byte[] savedIV = null;

         public static byte[] Key
        {
            get { return savedKey; }
            set { savedKey = value; }
        }

         public static byte[] IV
        {
            get { return savedIV; }
            set { savedIV = value; }
        }

        private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
        {
            if (savedKey == null)
            {
                rdProvider.KeySize = 256;
                rdProvider.GenerateKey( );
                savedKey = rdProvider.Key;
            }
        }

        private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
        {
            if (savedIV == null)
            {
                rdProvider.GenerateIV( );
                savedIV = rdProvider.IV;
            }
        }

        public static string Encrypt(string originalStr)
        {
            // Encode data string to be stored in memory
            byte[] originalStrAsBytes = Encoding.ASCII.GetBytes(originalStr);
            byte[] originalBytes = {};

            // Create MemoryStream to contain output
            MemoryStream memStream = new MemoryStream(originalStrAsBytes.Length);

            RijndaelManaged rijndael = new RijndaelManaged( );

            // Generate and save secret key and init vector
            RdGenerateSecretKey(rijndael);
            RdGenerateSecretInitVector(rijndael);

            if (savedKey == null || savedIV == null)
            {
                throw (new NullReferenceException(
                        "savedKey and savedIV must be non-null."));
            }

            // Create encryptor, and stream objects
            ICryptoTransform rdTransform = rijndael.CreateEncryptor((byte[])savedKey.
                                Clone( ),(byte[])savedIV.Clone( ));
            CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, 
                                CryptoStreamMode.Write);

            // Write encrypted data to the MemoryStream
            cryptoStream.Write(originalStrAsBytes, 0, originalStrAsBytes.Length);
            cryptoStream.FlushFinalBlock( );
            originalBytes = memStream.ToArray( );

            // Release all resources
            memStream.Close( );
            cryptoStream.Close( );
            rdTransform.Dispose( );
            rijndael.Clear( );

            // Convert encrypted string
            string encryptedStr = Convert.ToBase64String(originalBytes);
            return (encryptedStr);
        }

        public static string Decrypt(string encryptedStr)
        {
            // Unconvert encrypted string
            byte[] encryptedStrAsBytes = Convert.FromBase64String(encryptedStr);
            byte[] initialText = new Byte[encryptedStrAsBytes.Length];

            RijndaelManaged rijndael = new RijndaelManaged( );
            MemoryStream memStream = new MemoryStream(encryptedStrAsBytes);

            if (savedKey == null || savedIV == null)
            {
                throw (new NullReferenceException(
                        "savedKey and savedIV must be non-null."));
            }

            // Create decryptor, and stream objects
            ICryptoTransform rdTransform = rijndael.CreateDecryptor((byte[])savedKey.
                                Clone( ),(byte[])savedIV.Clone( ));
            CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, 
                                CryptoStreamMode.Read);

            // Read in decrypted string as a byte[]
            cryptoStream.Read(initialText, 0, initialText.Length);

            // Release all resources
            memStream.Close( );
            cryptoStream.Close( );
            rdTransform.Dispose( );
            rijndael.Clear( );

            // Convert byte[] to string
            string decryptedStr = Encoding.ASCII.GetString(initialText);
            return (decryptedStr);
        }


        public static void Test()
        {
            Console.WriteLine("String encryption by a symmetric Rijndael algorithm:");
            
            string str="Perica reže raci rep.";
            // Encrypt a string:

            string encryptedString = SymStringEncrypt.Encrypt(str);
            Console.WriteLine("Encrypted string: \"" + encryptedString + "\"" );
            // get the key and IV used so you can decryptedData it later
            byte [] key = SymStringEncrypt.Key;
            byte [] IV = SymStringEncrypt.IV;

            // Decrypt the string:
            SymStringEncrypt.Key = key;
            SymStringEncrypt.IV = IV;
            string decryptedString = SymStringEncrypt.Decrypt(encryptedString);
            Console.WriteLine("Decrypted string: \"" + decryptedString + "\"" );
        }

    }






    public class SymFileEncrypt
    {
        private byte[] savedKey = null;
        private byte[] savedIV = null;
        private SymmetricAlgorithm symmetricAlgorithm;
        string path;

        public byte[] Key
        {
            get { return savedKey; }
            set { savedKey = value; }
        }

        public byte[] IV
        {
            get { return savedIV; }
            set { savedIV = value; }
        }

        public SymFileEncrypt(SymmetricAlgorithm algorithm, string fileName)
        {
            symmetricAlgorithm = algorithm;
            path = fileName;
        }

        public void SaveSensitiveData(string sensitiveData)
        {
            // Encode data string to be stored in encrypted file
            byte[] encodedData = Encoding.Unicode.GetBytes(sensitiveData);

            // Create FileStream and crypto service provider objects
            FileStream fileStream = new FileStream(path,
                                                    FileMode.Create,
                                                    FileAccess.Write);

            // Generate and save secret key and init vector
            GenerateSecretKey();
            GenerateSecretInitVector();

            // Create crypto transform and stream objects
            ICryptoTransform transform = symmetricAlgorithm.CreateEncryptor(savedKey,
                                        savedIV);
            CryptoStream cryptoStream =
                new CryptoStream(fileStream, transform, CryptoStreamMode.Write);

            // Write encrypted data to the file 
            cryptoStream.Write(encodedData, 0, encodedData.Length);

            // Release all resources
            cryptoStream.Close();
            transform.Dispose();
            fileStream.Close();
        }

        public string ReadSensitiveData()
        {
            // Create file stream to read encrypted file back
            FileStream fileStream = new FileStream(path,
                                                    FileMode.Open,
                                                    FileAccess.Read);

            //print out the contents of the encrypted file
            BinaryReader binReader = new BinaryReader(fileStream);
            Console.WriteLine("---------- Encrypted Data ---------");
            int count = (Convert.ToInt32(binReader.BaseStream.Length));
            byte[] bytes = binReader.ReadBytes(count);
            char[] array = Encoding.Unicode.GetChars(bytes);
            string encdata = new string(array);
            Console.WriteLine(encdata);
            Console.WriteLine("---------- Encrypted Data ---------\r\n");

            // reset the file stream
            fileStream.Seek(0, SeekOrigin.Begin);

            // Create Decryptor
            ICryptoTransform transform = symmetricAlgorithm.CreateDecryptor(savedKey,
                                                                            savedIV);
            CryptoStream cryptoStream = new CryptoStream(fileStream,
                                                        transform,
                                                        CryptoStreamMode.Read);

            //print out the contents of the decrypted file
            StreamReader srDecrypted = new StreamReader(cryptoStream,
                                                            new UnicodeEncoding());
            Console.WriteLine("---------- Decrypted Data ---------");
            string decrypted = srDecrypted.ReadToEnd();
            Console.WriteLine(decrypted);
            Console.WriteLine("---------- Decrypted Data ---------");

            // Release all resources
            binReader.Close();
            srDecrypted.Close();
            cryptoStream.Close();
            transform.Dispose();
            fileStream.Close();
            return decrypted;
        }

        private void GenerateSecretKey()
        {
            if (null != (symmetricAlgorithm as TripleDESCryptoServiceProvider))
            {
                TripleDESCryptoServiceProvider tdes;
                tdes = symmetricAlgorithm as TripleDESCryptoServiceProvider;
                tdes.KeySize = 192; //  Maximum key size
                tdes.GenerateKey();
                savedKey = tdes.Key;
            }
            else if (null != (symmetricAlgorithm as RijndaelManaged))
            {
                RijndaelManaged rdProvider;
                rdProvider = symmetricAlgorithm as RijndaelManaged;
                rdProvider.KeySize = 256; // Maximum key size
                rdProvider.GenerateKey();
                savedKey = rdProvider.Key;
            }
        }

        private void GenerateSecretInitVector()
        {
            if (null != (symmetricAlgorithm as TripleDESCryptoServiceProvider))
            {
                TripleDESCryptoServiceProvider tdes;
                tdes = symmetricAlgorithm as TripleDESCryptoServiceProvider;
                tdes.GenerateIV();
                savedIV = tdes.IV;
            }
            else if (null != (symmetricAlgorithm as RijndaelManaged))
            {
                RijndaelManaged rdProvider;
                rdProvider = symmetricAlgorithm as RijndaelManaged;
                rdProvider.GenerateIV();
                savedIV = rdProvider.IV;
            }
        }

        static public void Test()
        {
            
            string fileData = "Perica reže\nraci rep.";

            // Encrypt and decryptedData a file by using TripleDES symmetric alg.:
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            SymFileEncrypt secretTDESFile = new SymFileEncrypt(tdes, "encryptedTDES.dat");

            Console.WriteLine("Writing secret data: \n\"{0}\"\n", fileData);
            secretTDESFile.SaveSensitiveData(fileData);
            // save for storage to read file
            byte[] key = secretTDESFile.Key;
            byte[] IV = secretTDESFile.IV;

            string decryptedData = secretTDESFile.ReadSensitiveData();
            Console.WriteLine("Read secret data: \n\"{0}\"\n", decryptedData);

            // release resources
            tdes.Clear();



            // Encrypt and decryptedData a file by using Rijndael symmetric alg.:
            // Use Rijndael
            RijndaelManaged rdProvider = new RijndaelManaged();
            SymFileEncrypt secretRDFile = new SymFileEncrypt(rdProvider, "rdtext.secret");

            Console.WriteLine("Writing secret data: {0}", fileData);
            secretRDFile.SaveSensitiveData(fileData);
            // save for storage to read file
            key = secretRDFile.Key;
            IV = secretRDFile.IV;

            decryptedData = secretRDFile.ReadSensitiveData();
            Console.WriteLine("Read secret data: {0}", decryptedData);

            // release resources
            rdProvider.Clear();

        }

    }



}



