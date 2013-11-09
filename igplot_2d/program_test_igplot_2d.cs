

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

// using IG.Forms;

using IG.Plot2d;



namespace IG.Test.Plot2D
{

    public class test_igform_console_program
    {

        [STAThread]
        public static void Main(string[] args)
        {

            // IG.Plot2d.NPlot_Demo.ExampleDemo();


            //GraphWindowTemplate win1 = new GraphWindowTemplate();
            //win1.ShowDialog();



            GraphWindowNPlot win = new GraphWindowNPlot();
            win.ShowDialog();
            Console.WriteLine("Press Enter to continue!");
            Console.ReadLine();



            //System.Windows.Forms.Form testForm = null;
                //testForm = new NPlot_Demo.PlotSurface2DDemo();
                //testForm.ShowDialog();




            // Thread.Sleep(1000);


            Console.WriteLine("\r\nBye.\r\n");
        }  // Main()








    }  // class igform_console_test






} // namespace igform_console_test
