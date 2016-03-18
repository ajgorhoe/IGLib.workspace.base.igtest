

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
using System.IO.Compression;
using System.Xml;

using IG.Lib;
using IG.Forms;
using IG.Num;


namespace igform_console_test
{
    public class test_igform_console_program
    {

        [STAThread]
        public static void Main(string[] args)
        {
            

            BrowserSimpleForm.Example();


            FadingMessage.Example();
            FadingMessage.ExampleInferior();


            UtilForms.ConsoleExample();
            UtilForms.ConsoleExample2();



            XmlTreeViewControl.Example();


            ConsoleForm.Example();
                


            //Console.WriteLine("\r\nWelcome to the IGForm'result console test.\r\d2\r\d2");

            //string str = "Str";
            ////Console.WriteLine("Size of string \"" + str + "\": " + sizeof(str).ToString());
            //Console.WriteLine("String Length: " +  str.Length.ToString() + ", character length: "
            //    + sizeof(char).ToString());



            ////Creating a fading message in this thread, which must be canceled explicitly (e.g. by pressing mouse button 3:)
            //string msgtitle = "FadeMessage; Naslov; Konec Naslova.\nXX\nYY\nZZ";
            //string msgtext = "My                                                                                                 label.\nLine 2\nLine 3\nLine 4\nLine 5\d2\nLLLLL";
            //FadeMessage fm = new FadeMessage(msgtitle, msgtext, 8000);
            ////Exit the application:
            //Thread.Sleep(5000);
            ////Application.Exit();
            //Environment.Exit(1);

            
            //// Some ConsoleForm teste:
            //new FadeMessage("Welcome to the console testing application.",3000);
            //// Console consform in a parallel thread:
            //ConsoleForm cons = new ConsoleForm("Testing console");
            //for (int i = 1; i <= 5; ++i)
            //{
            //    cons.WriteLine(i.ToString() + ".  vrstica");
            //}
            //cons.WriteLine("After for loop.");
            //Thread.Sleep(1000);
            //cons.WriteLine("After the first sleep after for loop.\d2");
            //Thread.Sleep(2000);
            //cons.OutSelectionColor = Color.Orange;
            //cons.WriteLine("This line should appear in Orange.");
            //cons.OutSelectionColor = Color.LightGreen;
            //cons.WriteLine("This line should appear in LightGreen.");
            //cons.Style = ConsoleForm.Styles.Normal;
            //cons.WriteLine("\nBack to normal style.");
            //cons.Style = ConsoleForm.Styles.Error;
            //cons.WriteLine("\nThis is in Error style.");
            //cons.SetMarkStyle();
            //cons.WriteLine("\nThis is in Mark Style.");
            //cons.Style = ConsoleForm.Styles.Normal;
            //cons.WriteLine("\nAnd this is again in normal style.");
            //cons.WriteLine("So we are back to normal style now.\d2");
            //cons.WriteLine("This should stil appear in normal style.\d2");
            //Thread.Sleep(3000);




            // Thread.Sleep(1000);
            Console.WriteLine("\r\nBye.\r\n");
        }  // Main()








    }  // class igform_console_test






} // namespace igform_console_test
