#region About
// Course material: C# for numerical methods
// Copyright (c) 2009-2010, Igor Gre�ovnik
#endregion About


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

using IG.Gr;

namespace IG.Gr
{

    // MOVED  TO igplot2d!!



    ///// <summary>Delegate for plotting on a Zedgraph pane.</summary>
    ///// <param name="PlotPanel">Zedgraph control used for plotting graphs within a window.</param>
    ///// $A Igor Jun09;
    //public delegate void ZedGraphPlotDlg(ZedGraphControl PlotPane);

    ///// <summary>Window form containing a Zedgraph control for plotting graphs.</summary>
    ///// $A Igor Jun09 Nov11;
    //public partial class ZedGraphWindow : Form
    //{
    //    public ZedGraphWindow()
    //    {
    //        InitializeComponent();
    //    }

    //    public ZedGraphPlotDlg PlotDelegate = null;


    //    private void Form1_Load( object sender, EventArgs e )
    //    {
    //        if (PlotDelegate != null)
    //            PlotDelegate(GraphControl);


    //        //// OPTIONAL: Show tooltips when the mouse hovers over a point
    //        //GraphControl.IsShowPointValues = true;
    //        //GraphControl.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

    //        // OPTIONAL: Add a custom context menu item
    //        GraphControl.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
    //                        MyContextMenuBuilder);

    //        // OPTIONAL: Handle the Zoom Event
    //        GraphControl.ZoomEvent += new ZedGraphControl.ZoomEventHandler(MyZoomEvent);

    //        // Size the control to fit the window
    //        SetSize();

    //        // Tell ZedGraph to calculate the axis ranges
    //        // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
    //        // up the proper scrolling parameters
    //        GraphControl.AxisChange();
    //        // Make sure the Graph gets redrawn
    //        GraphControl.Invalidate();

    //    }



    //    /// <summary>
    //    /// On resize action, resize the ZedGraphControl to fill most of the Form, with a small
    //    /// margin around the outside
    //    /// </summary>
    //    private void Form1_Resize( object sender, EventArgs e )
    //    {
    //        SetSize();
    //    }

    //    private void SetSize()
    //    {
    //        GraphControl.Location = new Point( 10, 10 );
    //        // Leave a small margin around the outside of the control
    //        GraphControl.Size = new Size( this.ClientRectangle.Width - 20,
    //                this.ClientRectangle.Height - 20 );
    //    }

    //    /// <summary>
    //    /// Display customized tooltips when the mouse hovers over a point
    //    /// </summary>
    //    private string MyPointValueHandler( ZedGraphControl control, GraphPane pane,
    //                    CurveItem curve, int iPt )
    //    {
    //        // Get the PointPair that is under the mouse
    //        PointPair pt = curve[iPt];
    //        return curve.Label.Text + " is " + pt.Y.ToString( "f2" ) + " units at " + pt.X.ToString( "f1" ) + " days";
    //    }

    //    /// <summary>
    //    /// Customize the context menu by adding a new item to the end of the menu
    //    /// </summary>
    //    private void MyContextMenuBuilder( ZedGraphControl control, ContextMenuStrip menuStrip,
    //                    Point mousePt, ZedGraphControl.ContextMenuObjectState objState )
    //    {
    //        ToolStripMenuItem item = new ToolStripMenuItem();
    //        item.Name = "add-beta";
    //        item.Tag = "add-beta";
    //        item.Text = "Add a new Beta Point";
    //        item.Click += new System.EventHandler( AddBetaPoint );

    //        menuStrip.Items.Add( item );
    //    }

    //    /// <summary>
    //    /// Handle the "Add New Beta Point" context menu item.  This finds the curve with
    //    /// the CurveItem.Label = "Beta", and adds a new point to it.
    //    /// </summary>
    //    private void AddBetaPoint( object sender, EventArgs args )
    //    {
    //        // Get a reference to the "Beta" curve IPointListEdit
    //        IPointListEdit ip = GraphControl.GraphPane.CurveList["Beta"].Points as IPointListEdit;
    //        if ( ip != null )
    //        {
    //            double x = ip.Count * 5.0;
    //            double y = Math.Sin( ip.Count * Math.PI / 15.0 ) * 16.0 * 13.5;
    //            ip.Add( x, y );
    //            GraphControl.AxisChange();
    //            GraphControl.Refresh();
    //        }
    //    }

    //    // Respond to a Zoom Event
    //    private void MyZoomEvent( ZedGraphControl control, ZoomState oldState,
    //                ZoomState newState )
    //    {
    //        // Here we get notification everytime the user zooms
    //    }

    //    private void btnCancel_Click(object sender, EventArgs e)
    //    {
    //        this.Dispose();
    //    }

    //    private void chkBlackBg_CheckedChanged(object sender, EventArgs e)
    //    {
    //        if (chkBlackBg.Checked)
    //           GraphControl.GraphPane.Chart.Fill = new Fill(Color.Blue, Color.Black, 45.0f);
    //        else
    //            GraphControl.GraphPane.Chart.Fill = new Fill(Color.White, Color.White, 45.0f);
    //        GraphControl.Invalidate();
    //    }


    //}


}