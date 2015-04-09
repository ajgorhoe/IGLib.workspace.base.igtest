// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Lib;
using IG.Num;


namespace IG.NumExperimental
{


    /// <summary>Base class for higher level classes that perform a single or several 
    /// response evaluations and output their results.
    /// This class contains a response evaluator obect that inherits form <typeparamref name="ResponseEvaluatorBase"/>, 
    /// for evaluation and storage of responses. In such a way, derived classes can deal with response evaluation and
    /// storage in an unified way. A variety of derived classe include classes for evaluation of single response,
    /// for generating 1 dimensional or 2 dimensional tables of responses, etc., for different kinds of responses
    /// and corresponding response evaluation objects, such as vector fucntions or dierect analysis (in optimization)
    /// response evaluation functions.
    /// Some common functionality is included such as properties that define log files, result files, and corresponding 
    /// streams.</summary>
    /// $A Igor Jul08 Mar09 Aug11;
    [Obsolete("Other classes should be used instead. Delete this class when others are fully developed!")]
    public abstract class EvaluatorBase<TypeResponseEvaluator, ResultType, FunctionType> : ILockable, IDisposable
        where TypeResponseEvaluator: ResponseEvaluatorWithStorageBase<ResultType, FunctionType>
        where ResultType: class
        where FunctionType: class
    {


        #region ThreadLocking

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        private object _internalLock = new object();

        /// <summary>Used internally for locking access to internal fields.</summary>
        protected object InternalLock { get { return _internalLock; } }

        //private object waitlock = new object();

        ///// <summary>Must be used only for locking waiting the Waiting() block (since it is potentially time consuming).</summary>
        //protected object WaitLock { get { return waitlock; } }

        #endregion ThreadLocking


        #region Operation

        TypeResponseEvaluator _responseEvaluator;


        /// <summary>Object used to evaluate responses at differnt sets of parameters and for
        /// storing responses to a list.</summary>
        TypeResponseEvaluator ResponseEvaluator
        {
            get { lock (Lock) { return _responseEvaluator; } }
            set { _responseEvaluator = value; } 
        }

        #region Output

        private int _loggingLevel = 2;
        private int _outputFrequency = 1;

        private string _outputDirectory;

        private bool _appendLogfile = true;
        private string _filenameBase;
        private string _logFileName;
        private string _logFilePath;
        private TextWriter _logWriter;
        private bool _logWriterSetFromOutside = false;

        private string _resultfileJsonPrefix;
        private string _resultfileJsonName;
        private string _resultFileJsonPath;

        /// <summary>Level of output.</summary>
        public int LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        /// <summary>Frequency of writing results of the response evaluation.
        /// This property is used in evaluators that evaluate several responses in one round.
        /// The property determines on how many iterations the results are stored in the result
        /// file. 1 means in every iteration, 0 means only at the end, -1 means that results are
        /// not stored at all, 4 means, fo rexample, that the results are stored in every 4th 
        /// iteration and at the end. Storing results before calculation is finished enables
        /// restarting broken calculations later in such a way that only the missing samples are
        /// calculated (e.g. in tables).</summary>
        public int OutputFrequency
        {
            get { return _outputFrequency; }
            set { _outputFrequency = value; }
        }

        /// <summary>Directory where output files are written, if not specified otherwise.
        /// Getter returns the currrent directory if the directory has not been set before.
        /// Directory can therefore be set to null and the object will still operete correctly,
        /// with the directory set to input directory.</summary>
        public string Outputdirectory
        {
            get 
            {
                lock (Lock)
                {
                    if (_outputDirectory == null)
                    {
                        Outputdirectory = Path.GetFullPath(Directory.GetCurrentDirectory());
                    }
                }
                return _outputDirectory;
            }
            set
            {
                lock (Lock)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = null;
                        if (!Directory.Exists(value))
                            throw new InvalidOperationException("Attempt to set output directory to a non-existent path. "
                                + Environment.NewLine + "  Path: " + value);
                    }
                    _outputDirectory = value;
                }
            }
        }

        public bool AppendLogfile
        {
            get { return _appendLogfile; }
            set { _appendLogfile = value; }
        }

        /// <summary>Base part of the file names used by the current object.
        /// If some file name is not defined (such as that of result file or log file)
        /// then the appropriate getter will generate hte name by using this property.
        /// Can be set to null, in this case the default file name base will be generated
        /// at next access.</summary>
        public string FileNameBase
        {
           get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(_filenameBase))
                    {
                        FileNameBase = EvaluatorConst.FileNameBaseDefault;
                    }
                    return _filenameBase;
                }
            }
            set
            {
                lock (Lock)
                {
                    _filenameBase = value;
                }
            }
        }

        /// <summary>File name of the log file (this is where operations and their resuts are logged).
        /// Can be set to null, in tihs case the name of the log file name will be generated at next access.</summary>
        public string LogfileName
        {
            get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(_logFileName))
                    {
                        LogfileName = FileNameBase + EvaluatorConst.LogfileExtension;
                    }
                    return _logFileName;
                }
            }
            set
            {
                lock (Lock)
                {
                    _logFileName = value;
                }
            }
        }

        /// <summary>Path to the log file (this is where operations and their resuts are logged).
        /// Can be set to null, in tihs case the name of hte path will be generated at next access.</summary>
        public string LogfilePath
        {
            get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(_logFilePath))
                    {
                        LogfilePath = Path.Combine(Outputdirectory, LogfileName);
                    }
                    return _logFilePath;
                }
            }
            set
            {
                lock (Lock)
                {
                    _logFilePath = value;
                }
            }
        }

        /// <summary>Text writer for writing to a log file.
        /// When getter is called and the writer does not exist, it is automatically created 
        /// by opening a file located at <see cref="LogfilePath"/>.
        /// Setter closes (disposes) evantual old writer if defined.
        /// WARNING: 
        /// Do not Close() or Dispose() the returned writer!
        /// Inside the method of this and derived classes, don't use the 
        /// setter except to set the writer to null, but just use the getter
        /// that automatically creates the writer if necessary. This is to 
        /// keep track whether the writer is open or not.</summary>
        public TextWriter LogWriter
        {
            get
            {
                lock (Lock)
                {
                    if (_logWriter == null)
                    {
                        LogWriter = new StreamWriter(LogfilePath, AppendLogfile);
                        _logWriterSetFromOutside = false;
                    }
                    return _logWriter;
                }
            }
            set
            {
                if (value != _logWriter && _logWriter!=null && _logWriterSetFromOutside)
                    _logWriter.Dispose();
                _logWriter = value;
                _logWriterSetFromOutside = true;   // this will be overridden if it is set from inside
            }
        }


        /// <summary>Prefix of the filename for the resutl file in JSON format that is 
        /// used by the current object to store a parts of its state, including the result
        /// of evaluations and settings.
        /// REMARK:
        /// Implementations in derived classes should follow implementation of <see cref="ResultfileJsonPrefixDefault"/>,
        /// except that the <see cref="EvaluatorConst.ResultfileJsonPrefix"/> should be replaced by the specific prefix
        /// appropriate to the specific type of the <see cref="EvaluatorBase"/> class.</summary>
        public abstract string ResultfileJsonPrefix
        { get; set; }


        /// <summary>Example of how the <see cref="ResultfileJsonPrefix"/> can be defined.
        /// If you don't want to use any specific prefix, you can just call getter and setter of this property in
        /// your implementation in the derived class.</summary>
        protected string ResultfileJsonPrefixDefault
        {
            get { lock (Lock) {
                if (_resultfileJsonPrefix == null)
                    _resultfileJsonPrefix = EvaluatorConst.ResultfileJsonPrefix;
                return _resultfileJsonPrefix;
            } }
            set
            { lock (Lock) { _resultfileJsonPrefix = value; } }
        }


        /// <summary>File name of the result file in JSON format.</summary>
        public string ResultfileJsonName
        {
            get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(_resultfileJsonName))
                    {
                        ResultfileJsonName = ResultfileJsonPrefix + FileNameBase + EvaluatorConst.ResultfileJsonExtension;
                    }
                    return _resultfileJsonName;
                }
            }
            set
            {
                lock (Lock)
                {
                    _resultfileJsonName = value;
                }
            }
        }

        /// <summary>Path to the result file in JSON format.</summary>
        public string ResultfileJsonPath
        {
            get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(_resultFileJsonPath))
                    {
                        LogfilePath = Path.Combine(Outputdirectory, ResultfileJsonName);
                    }
                    return _resultFileJsonPath;
                }
            }
            set
            {
                lock (Lock)
                {
                    _resultFileJsonPath = value;
                }
            }
        }


        #endregion Output


        #endregion Operation

        #region IDisposable


        ~EvaluatorBase()
        {
            Dispose(false);
        }


        private bool disposed = false;

        /// <summary>Implementation of IDisposable interface.</summary>
        public void Dispose()
        {
            lock(Lock)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>Does the job of freeing resources. This method can be 
        /// eventually overridden in derived classes (if they use other
        /// resources that must be freed - in addition to such resources of
        /// the current class).</summary>
        /// <param name="disposing">Tells whether the method has been called form Dispose() method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                LogWriter = null;  // this calls _logwriter.Dispose() if it has not been null before
                disposed = true;
            }
        }

        #endregion IDisposable


        /// <summary>Returns a string containing the basic information about the current evaluator
        /// such as path to the files where information is output, etc.</summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Response evaluator.");
            sb.AppendLine("  Output directory: " + Outputdirectory);
            sb.AppendLine("  Log file: " + LogfilePath);
            sb.AppendLine("  Result file: " + ResultfileJsonPath);
            return sb.ToString();
        }


    } // class EvaluatorBase

}