using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Lib
{
    public interface ILoadable: ILockable
    {

        string WorkingDirectory
        { get; set; }

        void Initialize();

        string Execute(string[] arguments);

    }


    public abstract class LoadableBase : ILoadable, ILockable
    {

        /// <summary>Constructs the class, calls the <see cref="Initialize"/>() method, which in turn 
        /// calls the <see cref="InitializeSpecific"/>() method()</summary>
        private LoadableBase()
        {
            Initialize();
        }

        public LoadableBase(string workingDirectory) : this()
        {
            this.WorkingDirectory = workingDirectory;
        }


        #region ThreadLocking

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ThreadLocking

        #region ILoadable_Interface

        private string _workingDir;

        private bool _isInitialized = false;

        /// <summary>Working directory of the class.</summary>
        public virtual string WorkingDirectory
        {
            get { return _workingDir; }
            set { _workingDir = value; }
        }

        /// <summary>Whether the object has been initialized or not.</summary>
        public virtual bool IsInitialized
        {  
            get { return _isInitialized; }
            protected set { _isInitialized = value; }
        }

        /// <summary>Initializes the object. 
        /// This method should already be called in constructor.</summary>
        public void Initialize()
        {
            lock (Lock)
            {
                if (!IsInitialized)
                {
                    InitializeSpecific();
                    IsInitialized = true;
                }
            }
        }

        /// <summary>Performs all the necessary initializations of the object.
        /// Override this method in derived classes (if extra initialization is needed)
        /// and call the base class' method in it.</summary>
        protected abstract void InitializeSpecific();

        /// <summary>Performs the action of this object.
        /// Override this in derived classes!</summary>
        /// <param name="arguments">Arguments through which different information can be passed.</param>
        /// <returns>String that represents result of the action.</returns>
        public abstract string Execute(string[] arguments);

        #endregion ILoadable_Interface
    }




}
