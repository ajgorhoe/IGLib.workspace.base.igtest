//*********************************************************************
//
// PfxOpen
// Opens Pfx (pkcs#12) file, check validity, opens to memory store.
// Enumerate all certs in memory store; uses first cert with private
// key and sets private key and public key properties.
// 
// 
// Method DeleteKeyContainer() enables removal of presistent keycontainer
//
// 			Copyright (C) 2003.  Michel I. Gallant
//*********************************************************************

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace JavaScience
{

public class PfxOpen
 {
 const String TITLE 		= "PfxOpen";
 const uint CRYPT_EXPORTABLE	= 0x00000001;
 const uint CRYPT_USER_PROTECTED= 0x00000002;
 const uint CRYPT_MACHINE_KEYSET= 0x00000020;
 const uint CRYPT_USER_KEYSET	= 0x00001000;     
 const uint CERT_KEY_PROV_INFO_PROP_ID	= 0x00000002;
 const uint X509_ASN_ENCODING 		= 0x00000001;
 const uint PKCS_7_ASN_ENCODING 	= 0x00010000;
 const uint RSA_CSP_PUBLICKEYBLOB	= 19;
 static uint ENCODING_TYPE 		= PKCS_7_ASN_ENCODING | X509_ASN_ENCODING ;

 bool verbose = false;

 String[] CSPTypes = {
  null,
 "PROV_RSA_FULL",	//1
 "PROV_RSA_SIG",	//2
 "PROV_DSS",
 "PROV_FORTEZZA",
 "PROV_MS_EXCHANGE",
 "PROV_SSL",
  null, null, null, null, null,
 "PROV_RSA_SCHANNEL",	//12
 "PROV_DSS_DH",
 "PROV_EC_ECDSA_SIG",
 "PROV_EC_ECNRA_SIG",
 "PROV_EC_ECDSA_FULL",
 "PROV_EC_ECNRA_FULL",
 "PROV_DH_SCHANNEL",
  null,
 "PROV_SPYRUS_LYNKS",	//20
 "PROV_RNG",
 "PROV_INTEL_SEC",
 "PROV_REPLACE_OWF",
 "PROV_RSA_AES"		// 24
 };
 String[] keyspecs = {null, "AT_KEYEXCHANGE", "AT_SIGNATURE"};


 private X509Certificate pfxcert;
 private String pfxcontainer;
 private String pfxprovname;
 private uint   pfxprovtype;
 private uint   pfxkeyspec;
 private uint   pfxcertkeysize;
 private byte[] pfxcertkeyexponent;
 private byte[] pfxcertkeymodulus;

//---- private key container properties ----
 public String container
 {
  get{return pfxcontainer;}
 }
 public String provider
 {
  get{return pfxprovname;}
 }
 public uint providertype
 {
  get{return pfxprovtype;}
 }
 public uint keyspec
 {
  get{return pfxkeyspec;}
 }


//---- public key properties -----
 public X509Certificate cert
 {
  get{return pfxcert;}
 }
 public uint keysize
 {
  get{return pfxcertkeysize;}
 }
 public byte[] keyexponent
 {
  get{return pfxcertkeyexponent;}
 }
 public byte[] keymodulus
 {
  get{return pfxcertkeymodulus;}
 }



//--- load pfx file, get first keycontainer and set private key instance variables ---
 internal bool LoadPfx(String pfxfilename, ref String pswd){
  IntPtr hMemStore  = IntPtr.Zero;
  IntPtr hCertCntxt = IntPtr.Zero;
  IntPtr pProvInfo  = IntPtr.Zero;
  uint provinfosize = 0;
  bool result = false;

 if (!File.Exists(pfxfilename))
  {
	Console.WriteLine("File '{0}' not found.", pfxfilename);
	return result;
  }

 byte[] pfxdata = PfxOpen.GetFileBytes(pfxfilename);
 if(pfxdata == null || pfxdata.Length==0)
	return result;

//--- initialize struct, allocate memory for pfx blob data --
  CRYPT_DATA_BLOB ppfx = new CRYPT_DATA_BLOB();
  ppfx.cbData = pfxdata.Length;
  ppfx.pbData = Marshal.AllocHGlobal(pfxdata.Length);
  Marshal.Copy(pfxdata, 0, ppfx.pbData, pfxdata.Length);


//--- make sure we have a valid pfx file ---
 if(!Win32.PFXIsPFXBlob(ref ppfx))
  {
	Console.WriteLine("!!!! File '{0}' is NOT a valid pfx blob !!!", pfxfilename);
	return result;
  }

//---- try to import to memory store ------
 hMemStore = Win32.PFXImportCertStore(ref ppfx, pswd, CRYPT_USER_KEYSET) ;
 pswd = null;
  if(hMemStore == IntPtr.Zero){
	string errormessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
	Console.WriteLine("\n{0}", errormessage);
	Marshal.FreeHGlobal(ppfx.pbData);
	return result;
  }
 Marshal.FreeHGlobal(ppfx.pbData);

 //------ iterate loaded memory store and return only first cert with private key container  ----
 //-----  ToDo:  May be several key containers; store info. in array and ask user which to use -- 
 while((hCertCntxt = Win32.CertEnumCertificatesInStore(hMemStore, hCertCntxt)) !=IntPtr.Zero)
   {
	if(Win32.CertGetCertificateContextProperty(hCertCntxt, CERT_KEY_PROV_INFO_PROP_ID, IntPtr.Zero, ref provinfosize))
 	  pProvInfo = Marshal.AllocHGlobal((int)provinfosize);
	else
	  continue;
  	if(Win32.CertGetCertificateContextProperty(hCertCntxt, CERT_KEY_PROV_INFO_PROP_ID, pProvInfo, ref provinfosize))
	{
	 CRYPT_KEY_PROV_INFO ckinfo = (CRYPT_KEY_PROV_INFO)Marshal.PtrToStructure(pProvInfo, typeof(CRYPT_KEY_PROV_INFO));
	 Marshal.FreeHGlobal(pProvInfo);

	 this.pfxcontainer= ckinfo.ContainerName;
	 this.pfxprovname = ckinfo.ProvName;
	 this.pfxprovtype = ckinfo.ProvType;
	 this.pfxkeyspec  = ckinfo.KeySpec;
	 this.pfxcert     = new X509Certificate(hCertCntxt);
	 if(!this.GetCertPublicKey(pfxcert))
	   Console.WriteLine("Couldn't get certificate public key");
	 result = true;
	 break; 
       }
  }
//-------  Clean Up  -----------
  if(pProvInfo != IntPtr.Zero)
	Marshal.FreeHGlobal(pProvInfo);
  if(hCertCntxt != IntPtr.Zero)
	Win32.CertFreeCertificateContext(hCertCntxt);
  if(hMemStore != IntPtr.Zero)
	Win32.CertCloseStore(hMemStore, 0) ;
  return result;
 }




//----- decode certificate and set public key instance variables ----
 private bool GetCertPublicKey(X509Certificate cert)
 {
	byte[] publickeyblob ;
        byte[] encodedpubkey = cert.GetPublicKey(); //asn.1 encoded public key

       	uint blobbytes=0;
	if(Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, null, ref blobbytes))
	 {
	  publickeyblob = new byte[blobbytes];
	  Win32.CryptDecodeObject(ENCODING_TYPE, RSA_CSP_PUBLICKEYBLOB, encodedpubkey, (uint)encodedpubkey.Length, 0, publickeyblob, ref blobbytes);
	 }
	else
	  return false;
	 
	PUBKEYBLOBHEADERS pkheaders = new PUBKEYBLOBHEADERS() ;
	int headerslength = Marshal.SizeOf(pkheaders);
	IntPtr buffer = Marshal.AllocHGlobal( headerslength);
	Marshal.Copy( publickeyblob, 0, buffer, headerslength );
	pkheaders = (PUBKEYBLOBHEADERS) Marshal.PtrToStructure( buffer, typeof(PUBKEYBLOBHEADERS) );
	Marshal.FreeHGlobal( buffer );
	if(verbose) {
	 Console.WriteLine("\n ---- PUBLICKEYBLOB headers ------");
	 Console.WriteLine("  btype     {0}", pkheaders.bType);
	 Console.WriteLine("  bversion  {0}", pkheaders.bVersion);
	 Console.WriteLine("  reserved  {0}", pkheaders.reserved);
	 Console.WriteLine("  aiKeyAlg  0x{0:x8}", pkheaders.aiKeyAlg);
	 String magicstring = (new ASCIIEncoding()).GetString(BitConverter.GetBytes(pkheaders.magic)) ;
	 Console.WriteLine("  magic     0x{0:x8}     '{1}'", pkheaders.magic, magicstring);
	 Console.WriteLine("  bitlen    {0}", pkheaders.bitlen);
	 Console.WriteLine("  pubexp    {0}", pkheaders.pubexp);
	 Console.WriteLine(" --------------------------------");
	}
	//-----  Get public key size in bits -------------
	this.pfxcertkeysize = pkheaders.bitlen;

	//-----  Get public exponent -------------
	byte[] exponent = BitConverter.GetBytes(pkheaders.pubexp); //little-endian ordered
	Array.Reverse(exponent);    //convert to big-endian order
	this.pfxcertkeyexponent = exponent;

	//-----  Get modulus  -------------
	int modulusbytes = (int)pkheaders.bitlen/8 ;
	byte[] modulus = new byte[modulusbytes];
	try{
		Array.Copy(publickeyblob, headerslength, modulus, 0, modulusbytes);
		Array.Reverse(modulus);   //convert from little to big-endian ordering.
		this.pfxcertkeymodulus = modulus;
	}
	catch(Exception){
		Console.WriteLine("Problem getting modulus from publickeyblob");
		return false;
	}
   return true;
 }



 public override string ToString(){
  string about = String.Format(
	"ContainerName:\t{0}\nProviderName:\t{1}\nProvType: {2}\t({3}) \nKeySpec:  {4}\t({5})\n{6}Keysize:\t{7} bits",
	 this.container,
	 this.provider,
	 this.providertype, CSPTypes[this.providertype],
	 this.keyspec,  keyspecs[this.keyspec],
	 this.cert.ToString(true),
	 this.keysize) ;
  return about;
 }


//----  Delete keycontainer; PFXImportCertStore() does NOT remove keycontainers automatically --- 
 internal bool DeleteKeyContainer(String containername, String provname, uint provtype)
 {
  const uint CRYPT_DELETEKEYSET	= 0x00000010;
  IntPtr hCryptProv = IntPtr.Zero;
  if(containername == null || provname == null || provtype == 0)
	return false;
  if (Win32.CryptAcquireContext(ref hCryptProv, containername, provname, provtype, CRYPT_DELETEKEYSET)){
	 return true;
	}
  else
	{
	 PfxOpen.showWin32Error(Marshal.GetLastWin32Error());
	 return false;
	}
 }


 internal bool NetDeleteKeyContainer(String containername, String provname, int provtype){
   CspParameters cp = new CspParameters();
   cp.KeyContainerName = containername;
   cp.ProviderName = provname;
   cp.ProviderType = provtype;
   Console.WriteLine("Container  "+ containername);
   Console.WriteLine("provname   " + provname);
   Console.WriteLine("provtype   " + provtype);
   RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider(cp);
   oRSA.PersistKeyInCsp = false;
   oRSA.Clear();
   return true;
 }


 private static byte[] GetFileBytes(String filename){
        if(!File.Exists(filename))
         return null;
        Stream stream=new FileStream(filename,FileMode.Open);
        int datalen = (int)stream.Length;
        byte[] filebytes =new byte[datalen];
        stream.Seek(0,SeekOrigin.Begin);
        stream.Read(filebytes,0,datalen);
        stream.Close();
        return filebytes;
  }


 private static void showWin32Error(int errorcode){
       Win32Exception myEx=new Win32Exception(errorcode);
       Console.WriteLine("Error code:\t 0x{0:X}", myEx.ErrorCode);
       Console.WriteLine("Error message:\t " + myEx.Message);
  }



 }
}

