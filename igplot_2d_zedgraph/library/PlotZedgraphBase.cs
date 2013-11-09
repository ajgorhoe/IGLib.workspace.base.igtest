// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;

using ZedGraph;

namespace IG.Gr
{

    // MOVED  TO igplot2d!!



    ///// <summary>Base class for plots that are shown in a <see cref="ZedGraphControl"/> object.</summary>
    ///// $A Igor Jun09;
    //public abstract class PlotZedGraphBase: ILockable, IDisposable
    //{
        
    //    #region Construction

    //    /// <summary>Prevent calling argument-less constructor in derived classes.</summary>
    //    private PlotZedGraphBase() 
    //    { }

    //    /// <summary>Constructor.</summary>
    //    /// <param name="plotter">ZedGraph plotter that is used for plotting graphs produeced by the 
    //    /// current plotting class.</param>
    //    public PlotZedGraphBase(PlotterZedGraph plotter)
    //    {
    //        if (plotter == null)
    //            throw new ArgumentNullException("plotter", "The ZedGraph plotter connected to the created plotting object is not specified.");
    //        this.Plotter = plotter;
    //        plotter.AddPlotObject(this);
    //    }

    //    #endregion Construction


    //    #region ThreadLocking

    //    private object _mainLock = new object();

    //    /// <summary>This object's central lock object to be used by other object.
    //    /// Do not use this object for locking in class' methods, for this you should use 
    //    /// InternalLock.</summary>
    //    public object Lock { get { return _mainLock; } }

    //    #endregion ThreadLocking


    //    #region Settings

    //    private int _outputLevel = PlotterZedGraph.DefaultOutputLevel;

    //    /// <summary>Level of output to the console for the current object.
    //    /// The defalult output level for newly created object is specified by <see cref="VtkPlotter"/>.<see cref="DefaultOutputLevel"/>.</summary>
    //    public int OutputLevel
    //    { get { return _outputLevel; } set { _outputLevel = value; } }

    //    private StopWatch _timer;

    //    /// <summary>Stopwatch that can be used to measure the time efficiency of actions.</summary>
    //    public StopWatch Timer
    //    { get { if (_timer == null) _timer = new StopWatch(); return _timer; } }


    //    /// <summary>Sets background color of the plotter that is used by the current plot object.
    //    /// <para>Task is delegated to the plotter.</para></summary>
    //    public color BackGround
    //    {
    //        get { return Plotter.Background; }
    //        set { Plotter.Background = value; }
    //    }

    //    #endregion Settings


    //    #region Data


    //    PlotterZedGraph _plotter;

    //    /// <summary>Zedgraph plotter that is used for plotting graphs produeced by the 
    //    /// current plotting class, on a ZedGraph control.
    //    /// <para>Getter is not thread safe (for better efficiency).</para></summary>
    //    public PlotterZedGraph Plotter
    //    {
    //        get
    //        {
    //            return _plotter;
    //        }
    //        set
    //        {
    //            lock (Lock)
    //            {
    //                if (value == null)
    //                    throw new InvalidOperationException("Invalid attempt to set ZedGraph plotter object to null.");
    //                _plotter = value;
    //            }
    //        }
    //    }


    //    #endregion Data

    //    protected string _legendString;

    //    /// <summary>String that is used for the current plot item in the legend.</summary>
    //    public string LegendString
    //    {
    //        get { lock (Lock) { return _legendString; } }
    //        set { lock (Lock) { _legendString = value; } }
    //    }


        
    //    #region Operation


    //    /// <summary>Creates data for the plot. Basically, this creates and updates the internal data
    //    /// structures used by the plot, while <see cref="Update"/> will also update the plot 
    //    /// in the window where it is shown.</summary>
    //    public abstract void CreateData();

        
    //    /// <summary>Updates the plot.</summary>
    //    public virtual void Update()
    //    {
    //        CreateData();
    //    }


    //    #endregion Operation


    //    #region IDisposable


    //    ~PlotZedGraphBase()
    //    {
    //        Dispose(false);
    //    }


    //    private bool disposed = false;

    //    /// <summary>Implementation of IDisposable interface.</summary>
    //    public void Dispose()
    //    {
    //        lock(Lock)
    //        {
    //            Dispose(true);
    //            GC.SuppressFinalize(this);
    //        }
    //    }

    //    /// <summary>Does the job of freeing resources. 
    //    /// <para></para>This method can be  eventually overridden in derived classes (if they use other 
    //    /// resources that must be freed - in addition to such resources of the current class).
    //    /// In the case of overriding this method, you should usually call the base.<see cref="Dispose"/>(<paramref name="disposing"/>).
    //    /// in the overriding method.</para></summary>
    //    /// <param name="disposing">Tells whether the method has been called form Dispose() method.</param>
    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposed)
    //        {
    //            if (disposing)
    //            {
    //                // Free other state (managed objects).
    //            }
    //            // Free unmanaged objects resources:

    //            // Set large objects to null:

    //            disposed = true;
    //        }
    //    }

    //    #endregion IDisposable



    //} // class PlotZedGraphBase

}