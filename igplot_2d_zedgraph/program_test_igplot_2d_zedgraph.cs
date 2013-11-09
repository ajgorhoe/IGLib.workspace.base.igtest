

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


using IG.Gr;



namespace IG.Test
{

    public class test_igform_console_program
    {

        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Test of Zedgraph graphics.");

            PlotterZedGraph.ExampleDecorations();


            //PlotterZedGraph.ExampleLissajous();

            //PlotterZedGraph.ExampleCurveStylesWithSave("c:/temp/ImageZedGraph.bmp");


            // PlotterZedGraph.ExempleSinePlots();
            
            //TestZedGraph.TestZedgraph1();

            Console.WriteLine("\r\nBye.\r\n");
        }  // Main()








    }  // class igform_console_test






} // namespace igform_console_test
