#region About
// Course material: C# for numerical methods
// Copyright (c) 2009-2010, Igor Grešovnik
#endregion About


// DEMONSTRACIJA RISANJA GRAFOV Z UPORABO KNJIŽNICE ZedGraph
// Če se želiš naučiti več o uporabi knjižnice, lahko:
//   * poženeš projekt ZedGraph.Demo
//   * prebereš članek na http://www.codeproject.com/KB/graphics/zedgraph.aspx 
//   * prebrskaš dokumentacijo v projektu (datoteka "ZedGraph Help.chm" v projektu ZedGraph)

using System;
using System.Collections.Generic;


// Za uporabo grafike vključimo spodnje imenske prostore. 
// Dodati moramo tudi ustrezne reference. Prva dva sta v sistemskih knjižnicah .NET, 
// zadnja pa je projekt (knjižnica) ZedGraph, ki je vključen v rešitev.
using System.Windows.Forms;
using System.Drawing;
using ZedGraph;


namespace IG.Gr
{
	static class TestZedGraph
	{
		
        // POZOR: Za uporabo okenskih kontrol (to velja tudi za risanje grafov) mora biti metoda Main
        // vedno definirana z atributom [STAThread], kot je to spodaj.
		//[STAThread]
		public static void TestZedgraph1()
		{
            try
            {
                
                // Inicializacija grafičnega okolja:
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);


                // ***************************************************************************************
                // GRAF DVEH PARAMETRIČNO PODANIH KRIVULJ NA PODLAGI MNOŽICE TOČK:

                // Najprej pripravimo podatke v 4 tabelah; tabele morajo biti dostopne metodi ParametricPlot, 
                // ki jo definiramo za izris podatkov.
                int
                    n1 = 50,  // število izračunanih točk na 1. krivulji
                    n2 = 200;  // število izračunanih točk na 2. krivulji

                X1 = new double[n1];  // abscise prve krivulje
                Y1 = new double[n1];  // ordinate prve krivulje
                X2 = new double[n2];  // abscise druge krivulje
                Y2 = new double[n2];  // ordinate druge krivulje
                // Napolnimo tabeli X in Y za prvo krivuljo:
                for (int i = 0; i < n1; ++i)
                {
                    double t = (double)i;
                    X1[i] = 10 * Math.Sin(2 * Math.PI * t / n1);
                    Y1[i] = Math.Sin((double)t * Math.PI / 15.0) * 16.0;
                }

                double
                    a = 1.0,
                    b = 0.1,
                    turns = 6.0;
                // Napolnimo tabeli X in Y za drugo krivuljo (logaritmična spirala):
                for (int i = 0; i < n2; ++i)
                {
                    double t = turns * 2 * Math.PI * (double)i / (double)(n2 - 1);
                    X2[i] = a * Math.Exp(b * t) * Math.Cos(t);
                    Y2[i] = a * Math.Exp(b * t) * Math.Sin(t);
                }


                // Nastavitev parametrov izrisa:
                PlotProportional = false;  // določa, ali je enaka skala za oba grafa
                PlotLegend = true;  // določa, ali se izriše legenda
                PlotTip = false;  // določa, ali se izriše škatla z namihom

                // IZRIS PRVIH DVEH GRAFOV:
                // Naredimo novo kontrolo tipa ZedGraphWindow:
                ZedGraphWindow PlotPane = new ZedGraphWindow();
                // Na kontroli nastavimo metodo, ki bo izrisala stvari:
                PlotPane.PlotDelegate = ParametricPlot;
                // Odpremo kontrolo:
                // Application.Run(PlotPane);

                
                // ***************************************************************************************
                // GRAF FUNKCIJ oblike y=f(x):
                // Graf naredimo na podobrn način, le da je zdaj pri generaciji tabele X neodvisna 
                // spremenljivka.
                // Najprej pripravimo podatke v 4 tabelah; tabele morajo biti dostopne metodi ParametricPlot, 
                // ki jo definiramo za izris podatkov.
                int
                    n = 20;  // število izračunanih točk na krivuljah
                X1 = new double[n];  // abscise prve krivulje
                Y1 = new double[n];  // ordinate prve krivulje
                X2 = new double[n];  // abscise druge krivulje
                Y2 = new double[n];  // ordinate druge krivulje
                // Napolnimo tabeli X in Y za obe krivulji:
                for (int i = 0; i < n; ++i)
                {
                    double x, y1, y2;
                    x = (double)4 * Math.PI * i / n;
                    y1 = Math.Sin(x) + Math.Exp(x / 15 * Math.PI);
                    y2 = Math.Cos(x);
                    X1[i] = X2[i] = x;
                    Y1[i] = y1;
                    Y2[i] = y2;
                }

                // Nastavitev parametrov izrisa:
                PlotProportional = true;  // določa, ali je enaka skala za oba grafa
                PlotLegend = true;  // določa, ali se izriše legenda
                PlotTip = false;  // določa, ali se izriše škatla z namihom

                // IZRIS DRUGIH DVEH GRAFOV:
                // Naredimo novo kontrolo tipa ZedGraphWindow:
                ZedGraphWindow PlotPane2 = new ZedGraphWindow();
                // Na kontroli nastavimo metodo, ki bo izrisala stvari:
                //PlotPane2.PlotDelegate =  ParametricPlot;

                PlotterZedGraph plotter = new PlotterZedGraph(PlotPane2.GraphControl);
                

                PlotZedgraphCurve curve1 = new PlotZedgraphCurve(plotter);

                curve1.LegendString = "Curve by CurvePlot object.";

                curve1.AddPoints(X2, Y2);

                // PlotZedgraphCurve curve2 = new PlotZedgraphCurve(plotter);

                plotter.Update();

                // Odpremo kontrolo:
                Application.Run(PlotPane2);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine();
            }
		}  // Main()



        // PODATKI, ki jih uporablja metoda za izris:
        // Teh podatkov ne moremo prenesti preko argumentov (ker so tipi argumentov 
        // metode vnaprej določeni), zato morajo biti neposredno dostopni v telesu
        // metode. To dosežemo ali tako, da metodo in podatek definiramo kot static,
        // ali pa tako, da so definirani znotraj istega razreda.
        static double[] X1 = null, X2 = null, Y1 = null, Y2=null; 
        // Nastavitev, ki določa, ali sta skali za krivulji enaki:
        static bool PlotProportional = false;
        // Izris legende in škatle z namigom:
        static bool
            PlotLegend = true,
            PlotTip = true;


        /// <summary>Sets properties and plotted object on the ZedGraphControl.</summary>
        /// <param name="GraphControl">ZedGraphControl object that on which this method plots.</param>
        public static void ParametricPlot(ZedGraphControl GraphControl)
        {

            if (X1 == null)
                throw new Exception("First X table is null.");
            if (Y1 == null)
                throw new Exception("First Y table is null.");
            if (X1.Length != Y1.Length || X1.Length < 1)
                throw new Exception("Table lengths for the first graph are not consistent.");
            if (X2 == null)
                throw new Exception("Second X table is null.");
            if (Y2 == null)
                throw new Exception("Second Y table is null.");
            if (X2.Length != Y2.Length || X2.Length < 1)
                throw new Exception("Table lengths for the second graph are not consistent.");


            // Naredimo seznama točk, ki ju bomo izrisali:
            PointPairList PointList1 = new PointPairList();
            PointPairList PointList2 = new PointPairList();

            // Skopiramo naše podatke v seznama:
            for (int i = 0; i < X1.Length; ++i)
            {
                PointList1.Add(X1[i],Y1[i]);
            }
            for (int i = 0; i < X2.Length; ++i)
            {
                PointList2.Add(X2[i],Y2[i]);
            }


            // Iz seznamov točk naredimo krivulji, ki ju dodamo grafu.
            // Če želimo izrisati samo en graf, naredimo samo eno krivuljo:

            // Get a reference to the GraphPane instance in the ZedGraphControl
            GraphPane myPane = GraphControl.GraphPane;

            // Specify descriptions for the legend:
            string LegendString1 = "K1";
            string LegendString2 = "K2";
            if (!PlotLegend)
            {
                LegendString1 = LegendString2 = null;
            }

            // Generate a red curve with diamond symbols and LegendString1 in the legend
            LineItem myCurve = myPane.AddCurve(LegendString1,
                PointList1, Color.Red, SymbolType.Diamond);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Generate a blue curve with circle symbols, and LegendString2 in the legend
            myCurve = myPane.AddCurve(LegendString2,
                PointList2, Color.Blue, SymbolType.Circle);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Associate this curve with the Y2 axis

            if (!PlotProportional)
            {
                // POZOR: S tem določimo, da imamo za drugo krivuljo drugi osi x os in y in je zato
                // drugače skalirana. Ne glede na to, kakšna je razlika v razponu vrednosti , bosta 
                // krivulji vedno zavzeli približno enak del grafa.
                // Brez spodnjega stavka bi bili izrisani v dejanskih proporcah.
                myCurve.IsX2Axis = true;
                myCurve.IsY2Axis = true;
            }
            else
            {
                myCurve.IsX2Axis = false;
                myCurve.IsY2Axis = false;
            }


            // NASTAVITVE ZA IZRIS GRAFA:
            // Od tu naprej so razne nastavitve, ki vplivajo na to, kaj in kako se na grafu
            // izriše:

            // Set the titles and axis labels
            myPane.Title.Text = "Izris parametričnih krivulje (X(t), Y(t))";
            myPane.XAxis.Title.Text = "X1(t)";
            myPane.YAxis.Title.Text = "Y1(t)";
            myPane.X2Axis.Title.Text = "X2(t)";
            myPane.Y2Axis.Title.Text = "Y2(t)";

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = -30;
            myPane.YAxis.Scale.Max = 30;

            // Enable the Y2 axis display
            myPane.Y2Axis.IsVisible = true;
            // Make the Y2 axis scale blue
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            myPane.Y2Axis.MajorTic.IsOpposite = false;
            myPane.Y2Axis.MinorTic.IsOpposite = false;
            // Display the Y2 axis grid lines
            myPane.Y2Axis.MajorGrid.IsVisible = true;
            // Align the Y2 axis labels so they are flush to the axis
            myPane.Y2Axis.Scale.Align = AlignP.Inside;

            // Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

            if (PlotTip)
            {
                // Add a text box with instructions
                TextObj text = new TextObj(
                    "Zoom: left mouse & drag\nPan: middle mouse & drag\nContext Menu: right mouse",
                    0.05f, 0.95f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
                text.FontSpec.StringAlignment = StringAlignment.Near;
                myPane.GraphObjList.Add(text);
            }

            // Enable scrollbars if needed
            GraphControl.IsShowHScrollBar = true;
            GraphControl.IsShowVScrollBar = true;
            GraphControl.IsAutoScrollRange = true;
            GraphControl.IsScrollY2 = true;

        }  //



	}
}