using System;
using System.Runtime.InteropServices;

namespace JavaScience
{
public class Win32
{
//----  CryptoAPI  CSP and key functions ------
 [DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
    public static extern bool CryptAcquireContext(
	ref IntPtr hProv,
	string pszContainer,
	string pszProvider,
	uint dwProvType,
	uint dwFlags) ;


 [DllImport("crypt32.dll")]
    public static extern bool CryptDecodeObject(
	uint CertEncodingType,
	uint lpszStructType,
	byte[] pbEncoded,
	uint cbEncoded,
	uint flags,
	[In, Out] byte[] pvStructInfo,
	ref uint cbStructInfo);



//--- CryptoAPI certificate functions ------
 [DllImport("crypt32.dll", SetLastError=true)]
    public static extern bool CertCloseStore(
	IntPtr hCertStore,
	uint dwFlags) ;

 [DllImport("crypt32.dll", SetLastError=true)]
    public static extern bool CertFreeCertificateContext(
	IntPtr hCertStore) ;

 [DllImport("crypt32.dll", SetLastError=true)]
    public static extern IntPtr CertEnumCertificatesInStore(
	IntPtr hCertStore,
	IntPtr pPrevCertContext) ;

 [DllImport("crypt32.dll", SetLastError=true)]	
  public static extern bool CertGetCertificateContextProperty(
  IntPtr pCertContext, uint dwPropId, IntPtr pvData, ref uint pcbData);


//---  CryptoAPI pfx functions ------
 [DllImport("crypt32.dll", SetLastError=true)]	
  public static extern IntPtr PFXImportCertStore(
	ref CRYPT_DATA_BLOB pPfx, 
	[MarshalAs(UnmanagedType.LPWStr)] String szPassword,
	uint dwFlags);

 [DllImport("crypt32.dll", SetLastError=true)]	
  public static extern bool PFXVerifyPassword(
	ref CRYPT_DATA_BLOB pPfx, 
	[MarshalAs(UnmanagedType.LPWStr)] String szPassword,
	uint dwFlags);

 [DllImport("crypt32.dll", SetLastError=true)]	
  public static extern bool PFXIsPFXBlob(ref CRYPT_DATA_BLOB pPfx);

}

//--------  Win32 structs prototypes ---------------
 [StructLayout(LayoutKind.Sequential)]
  public struct CRYPT_KEY_PROV_INFO
  {
	[MarshalAs(UnmanagedType.LPWStr)] public String ContainerName;
	[MarshalAs(UnmanagedType.LPWStr)] public String ProvName;
	public uint ProvType;
	public uint Flags;
	public uint ProvParam;
	public IntPtr rgProvParam;
	public uint KeySpec;
  }

 [StructLayout(LayoutKind.Sequential)]
  public struct CRYPT_DATA_BLOB
  {
	public int cbData;
	public IntPtr pbData;
  }


 [StructLayout(LayoutKind.Sequential)]
  public struct PUBKEYBLOBHEADERS {
	public byte bType;	//BLOBHEADER
	public byte bVersion;	//BLOBHEADER
	public short reserved;	//BLOBHEADER
	public uint aiKeyAlg;	//BLOBHEADER
	public uint magic;	//RSAPUBKEY
	public uint bitlen;	//RSAPUBKEY
	public uint pubexp;	//RSAPUBKEY
 }


}
