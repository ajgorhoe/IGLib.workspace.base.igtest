

//
//   THIS IS AN EXAMPLE OF GETTING ASYMMETIRC KEYS FROM A PFX FILE
//      BUT THERE ARE SIMPLER WAYS!
//



//*********************************************************************
//
// TestPfx
//
// Copyright (C) 2003.  Michel I. Gallant
//*********************************************************************

using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

using System.Security;
using System.Security.Cryptography;

namespace JavaScience
{

public class TestPfx
 {

    // Certificate intermediate = Certificate.CreateFromCerFile("intermediate_cert.cer");

    [STAThread]
    public static void MainPfx(string pfxfilename, string pfxpasswd)
    {



        // ArrayList<string> args = new ArrayList<string>();
        ArrayList args = new ArrayList();
        if (pfxfilename!=null)
            if (pfxfilename.Length > 0)
            {
                args.Add(pfxfilename);
                if (pfxpasswd!=null)
                    if (pfxpasswd.Length > 0)
                    {
                        args.Add(pfxpasswd);
                    }
            }
        if (args.Count < 1)
            Console.WriteLine("The .pfx file name containing a certificate is not specified.\n");
        else
            MainPfx((string[]) args.ToArray(typeof(string)));
    }

 [STAThread]
 public static void MainPfx(string[] args) {
 if(args.Length<1 || args.Length>2)
  {
 	Console.WriteLine("Usage:   TestPfx  <pfxfilename>  [pswd]");
	return;
  }
 String pfxfilename = args[0] ;
 String pswd = "";
 if (!File.Exists(pfxfilename))
  {
	Console.WriteLine("File '{0}' not found.", pfxfilename);
	return;
  }

  if(args.Length == 2)
   pswd = args[1];
  else  // prompt user with password dialog.
  {
   PswdDialog dlg = new PswdDialog("PFX Password");
   DialogResult result = dlg.ShowDialog();
  //---- Process the dialogbox textfield ------
   if (result == DialogResult.OK)
	pswd = dlg.GetPswd();
   else
	Console.WriteLine("Cancelled password entry");
   dlg.Dispose();
  }

  PfxOpen pfx = new PfxOpen();
  if(!pfx.LoadPfx(pfxfilename, ref pswd))
  {
   Console.WriteLine("Failed to load pfx");
   return;
  }
   Console.WriteLine("Loaded pfx file '{0}'\n", pfxfilename);
   Console.WriteLine("\nKeycontainer and certificate parameters:\n{0}\n", pfx.ToString()) ;

  //--- Use the new keycontainer in .NET  ----
  //--- Our pfx instance has all properties for encryption or signatures --
    Console.WriteLine("Use the new keycontainer here (2 second pause) ....\n") ;
    Thread.Sleep(2000) ;

  //------ Delete the presistent keycontainer and assocated keys ---
 Console.WriteLine("Attempting to delete keycontainer: {0} ...", pfx.container);
 if(pfx.DeleteKeyContainer(pfx.container, pfx.provider, pfx.providertype))
   Console.WriteLine("Deleted keycontainer");
 else
   Console.WriteLine("Could not delete keycontainer");
 }

 }
}

