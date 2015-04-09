using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Principal;
using System.Security.AccessControl;

namespace IG.Lib
{

    public class UtilSettings
    {

        #region UtilOriginal

        /// <summary>Check whether the specified mutex has been abandoned, and returns true
        /// if it has been (otherwise, false is returned).
        /// <para>After the call, mutex is no longer in abandoned state (WaitOne() will not throw an exception)
        /// if it has been before the call.</para>
        /// <para>Call does not block.</para></summary>
        /// <param name="m">Mutex that is checked, must not be null.</param>
        /// <returns>true if mutex has been abandoned, false otherwise.</returns>
        public static bool MutexCheckAbandoned(Mutex m)
        {
            bool ret = false;
            if (m == null)
                throw new ArgumentException("Mutex to be checked is not specified (null argument).");
            bool acquired = false;
            try
            {
                acquired = m.WaitOne(0);
                if (acquired)
                {
                    try
                    {
                        m.ReleaseMutex();
                    }
                    catch { }
                }
            }
            catch
            {
                ret = true;
                try
                {
                    m.ReleaseMutex();
                }
                catch { }
            }
            return ret;
        }

        /// <summary>Name of the global mutex.</summary>
        public const string MutexGlobalName = "Global\\IG.Lib.Utils.MutexGlobal.xx";

        protected static volatile Mutex _mutexGlobal;

        /// <summary>Mutex for system-wide exclusive locks.</summary>
        public static Mutex MutexGlobal
        {
            get
            {
                if (_mutexGlobal == null)
                {
                    lock (Util.LockGlobal)
                    {
                        if (_mutexGlobal == null)
                        {
                            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                            MutexSecurity mutexsecurity = new MutexSecurity();
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow));
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.ChangePermissions, AccessControlType.Deny));
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.Delete, AccessControlType.Deny));
                            bool createdNew;
                            //_mutexGlobal = new Mutex(false, MutexGlobalName);
                            _mutexGlobal = new Mutex(false, MutexGlobalName,
                                out createdNew, mutexsecurity);
                        }
                    }
                }
                return _mutexGlobal;
            }
        }

        /// <summary>Check whether the global mutex (property <see cref="MutexGlobal"/>) has been abandoned, 
        /// and returns true if it has been (otherwise, false is returned).
        /// <para>After the call, mutex is no longer in abandoned state (WaitOne() will not throw an exception)
        /// if it has been before the call.</para>
        /// <para>Call does not block.</para></summary>
        /// <returns>true if mutex has been abandoned, false otherwise.</returns>
        public static bool MutexGlobalCheckAbandoned()
        {
            return MutexCheckAbandoned(MutexGlobal);
        }


        #endregion UtilOriginal


    }


    public class StringSettings : StringVariableSystem, ILockable
    {

        #region Construction

        public StringSettings()
            : base()
        {  }

        #endregion Construction


        #region Operation



        #endregion Operation


        #region ImportExport


        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// <para>If the file already exists, contents overwrite the file.</para></summary>
        /// <param name="filePath">Path to the file in variables and properties of the system are saved.</param>
        public override void Export(string filePath)
        {
            Export(filePath, false /* append */);
        }

        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// <para>If the file already exists, contents either overwrite the file or are appended at the end, 
        /// dependent on the value of the append flag.</para></summary>
        /// <param name="filePath">Path to the file in variables and properties of the system are saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public override void Export(string filePath, bool append)
        {
            StringVariableSystemDto savedVariables = new StringVariableSystemDto();
            savedVariables.CopyFrom(this);
            ExportDto(savedVariables, filePath, append);
        }

        /// <summary>Imports variables from the specified variable system in JSON format.</summary>
        /// <param name="filePath">File from which object is restored.</param>
        public override void Import(string filePath)
        {
            StringVariableSystemDto savedVariables = new StringVariableSystemDto();
            ImportDto(filePath, ref savedVariables);
            StringVariableSystem varSystem = this;
            savedVariables.CopyTo(ref varSystem);
        }

        #region ImportExport.Static


        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// File is owerwritten if it exists.</summary>
        /// <param name="settings">Object that is saved to a file.</param>
        /// <param name="filePath">Path to the file in which object is is saved.</param>
        public static void SaveJson(StringSettings settings, string filePath)
        {
            SaveJson(settings, filePath, false /* append */ );
        }

        /// <summary>Saves (serializes) the specified variable system to the specified JSON file.
        /// If the file already exists, contents either overwrites the file or is appended at the end, 
        /// dependent on the value of the append flag.</summary>
        /// <param name="settings">Object that is saved to a file.</param>
        /// <param name="filePath">Path to the file into which object is saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public static void SaveJson(StringSettings settings, string filePath, bool append)
        {
            StringSettingsDto dtoOriginal = new StringSettingsDto();
            dtoOriginal.CopyFrom(settings);
            ISerializer serializer = new SerializerJson();
            serializer.Serialize<StringSettingsDto>(dtoOriginal, filePath, append);
        }

        /// <summary>Restores (deserializes) a vector from the specified file in JSON format.</summary>
        /// <param name="filePath">File from which object is restored.</param>
        /// <param name="settingsRestored">Object that is restored by deserialization.</param>
        public static void LoadJson(string filePath, ref StringSettings settingsRestored)
        {
            ISerializer serializer = new SerializerJson();
            StringSettingsDto dtoRestored = serializer.DeserializeFile<StringSettingsDto>(filePath);
            dtoRestored.CopyTo(ref settingsRestored);
        }


        // DTO:

        /// <summary>Saves (serializes) the specified variable system DTO to the specified JSON file.
        /// <para>If the file already exists, contents overwrite the file.</para></summary>
        /// <param name="settings">Variable system DTO that is saved to a file.</param>
        /// <param name="filePath">Path to the file into which variables (and settings and other stuff) are saved.</param>
        public static void ExportDto(StringSettingsDto settings, string filePath)
        {
            ExportDto(settings, filePath, false /* append */);
        }

        /// <summary>Saves (serializes) the specified variable system DTO to the specified JSON file.
        /// <para>If the file already exists, contents either overwrite the file or are appended at the end, 
        /// dependent on the value of the append flag.</para></summary>
        /// <param name="variables">Variable system DTO that is saved to a file.</param>
        /// <param name="filePath">Path to the file into which variables (and settings and other stuff) are saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public static void ExportDto(StringSettingsDto variables, string filePath, bool append)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path not specified properly (null or empty string).");
            if (variables == null)
                throw new ArgumentException("Settings object is not specified (null reference).");
            ISerializer serializer = new SerializerJson();
            serializer.Serialize<StringSettingsDto>(variables, filePath, append);
        }


        /// <summary>Restores (deserializes) variable system DTO from the specified file in JSON format.
        /// <para>The restored DTO can be used e.g. to import variables to a variable system.</para></summary>
        /// <param name="filePath">File from which object is restored.</param>
        /// <param name="settings">Variable system DTO that is restored by deserialization.</param>
        public static void ImportDto(string filePath, ref StringSettingsDto settings)
        {
            ISerializer serializer = new SerializerJson();
            settings = serializer.DeserializeFile<StringSettingsDto>(filePath);
        }


        #endregion ImportExport.Static

        #endregion ImportExport


        #region AutoImport

        /// <summary>Default name of the mutex for system-wide locking of files used by the current class.</summary>
        public const string DefaultLockFileMutexName = "Global\\IG.Lib.Utils.MutexSettingsDefault.R2D2_by_Igor_Gresovnik";

        /// <summary>Default name of the file containing settings to be imported.</summary>
        public const string DefaultImportingFilename = "settingsglobal.json";

        /// <summary>Default name of the message file instructing object of this class to import the settings file once
        /// and then delete the message.</summary>
        public const string DefaultMsgImportOnceFilename = "importsettings.msg";

        /// <summary>Default name of the message file instructing object of this class to continuously import the 
        /// settings file without deleting the message.</summary>
        public const string DefaultMsgImportContinuoslyFilename = "importsettingscontinuously.msg";

        /// <summary>Default period for checking for settings to be imported (ind importing them, if applicable).</summary>
        public const double DefaultImportCheckPeriodSeconds = 1.0421;

        /// <summary>Minimal period for checking for settings to be imported (ind importing them, if applicable).</summary>
        public const double MinimalImportCheckPeriodSeconds = 0.01;

        protected string _lockFileMutexName = DefaultLockFileMutexName;

        /// <summary>Name of the mutex for system-wide locking of files used by the current class.</summary>
        public string LockFileMutexName
        {
            get
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(_lockFileMutexName))
                        throw new InvalidOperationException("File locking mutex name is not specified (null or empty string).");
                    return _lockFileMutexName;
                }
            }
            set {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException("File locking mutex name is not specified (null or empty string).");
                    _lockFileMutexName = value;
                    LockFileMutex = null;
                }
            }
        }

        protected Mutex _lockFileMutex;

        /// <summary>Mutex for system-wide exclusive locks for file system operations
        /// related to the current class.</summary>
        protected virtual Mutex LockFileMutex
        {
            get
            {
                if (_lockFileMutex == null)
                {
                    lock (Lock)
                    {
                        if (_lockFileMutex == null)
                        {
                            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                            MutexSecurity mutexsecurity = new MutexSecurity();
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow));
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.ChangePermissions, AccessControlType.Deny));
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.Delete, AccessControlType.Deny));
                            bool createdNew;
                            _lockFileMutex = new Mutex(false, LockFileMutexName,
                                out createdNew, mutexsecurity);
                        }
                    }
                }
                return _lockFileMutex;
            }
            set
            {
                _lockFileMutex = value;
            }
        }


        /// <summary>Check whether the filesystem locking mutex (property <see cref="LockFileMutex"/>) has been abandoned, 
        /// and returns true if it has been (otherwise, false is returned).
        /// <para>After the call, mutex is no longer in abandoned state (WaitOne() will not throw an exception)
        /// if it has been before the call.</para>
        /// <para>Call does not block.</para></summary>
        /// <returns>true if mutex has been abandoned, false otherwise.</returns>
        public bool LockFileMutexCheckAbandoned()
        {
            return Util.MutexCheckAbandoned(LockFileMutex);
        }

        protected string _msgImportOnceFilename = DefaultMsgImportOnceFilename;

        /// <summary>Name of the message file instructing object of this class to import the settings file once
        /// and then delete the message.</summary>
        public string MsgImportOnceFilename
        {
            get { lock (Lock) { return _msgImportOnceFilename; } }
            set
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException("Name of the message file for importing settings once is not specified (null or empty string).");
                    _msgImportOnceFilename = value;
                }
            }
        }

        protected string _msgImportContinuouslyFilename = DefaultMsgImportContinuoslyFilename;

        /// <summary>Name of the message file instructing object of this class to continuously import the 
        /// settings file without deleting the message.</summary>
        public string MsgImportContinuoslyFilename
        {
            get { lock (Lock) { return _msgImportContinuouslyFilename; } }
            set
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException("Name of the message file for importing settings periodically is not specified (null or empty string).");
                    _msgImportContinuouslyFilename = value;
                }
            }
        }

        protected string _importingFilename = DefaultImportingFilename;

        /// <summary>Name of the message file instructing object of this class to continuously import the 
        /// settings file without deleting the message.</summary>
        public string ImportingFilename
        {
            get { lock (Lock) { return _importingFilename; } }
            set
            {
                lock (Lock)
                {
                    if (string.IsNullOrEmpty(value))
                        throw new ArgumentException("Name of the file from which settings are imported is not specified (null or empty string).");
                    _importingFilename = value;
                }
            }
        }

        protected bool _doImportPeriodically = true;

        /// <summary>Flag indicating whether the current object should periodically check the 
        /// designated directories for settings to be imported, and import them.
        /// <para>Directories that are checked for settings are set by the <see cref="AddSettingDirectories"/> and 
        /// <see cref="RemoveSettingDirectories"/> methods. Time periods are specified by the <see cref="ImportCheckPeriodSeconds"/> property.</para>
        /// <para>Checking and importing settings is performed by the <see cref="CheckAndImport"/> method.</para></summary>
        protected bool DoImportPeriodically
        {
            get { lock (Lock) { return _doImportPeriodically; } }
            set
            {
                bool valueChanged;
                lock (Lock)
                {
                    valueChanged = (value != _doImportPeriodically);
                    _doImportPeriodically = value;
                }
                if (valueChanged)
                {
                    OnChangedImportingStatus();
                }
            }
        }


        protected double _importCheckPeriodSeconds = DefaultImportCheckPeriodSeconds;

        /// <summary>Period for checking for settings to be imported (ind importing them, if applicable).</summary>
        public double ImportCheckPeriodSeconds
        {
            get { lock (Lock) { return _importCheckPeriodSeconds; } }
            set
            {
                bool valueChanged;
                lock (Lock)
                {
                    if (value < MinimalImportCheckPeriodSeconds)
                        value = MinimalImportCheckPeriodSeconds;
                    valueChanged = (value != _importCheckPeriodSeconds);
                    _importCheckPeriodSeconds = value;
                }
                if (valueChanged)
                {
                    OnChangedImportingStatus();
                }
            }
        }

        private List<string> _settingDirectories;

        protected List<string> SettingDirectories
        {
            get {
                lock (Lock)
                {
                    if (_settingDirectories == null)
                        _settingDirectories = new List<string>();
                    return _settingDirectories;
                }
            }
        }


        /// <summary>Adds the specified directory paths to the list of directories that are periodically
        /// checked for settings to be imported.</summary>
        /// <param name="directoryPaths">Paths to be added to the list, can be relative or absolute.</param>
        /// <returns>Number of directories that were actually added to the list.</returns>
        public int AddSettingDirectories(params string[] directoryPaths)
        {
            int count = 0;
            if (directoryPaths != null)
            {
                int numPaths = directoryPaths.Length;
                if (numPaths > 0)
                {
                    lock (Lock)
                    {
                        List<string> dirs = SettingDirectories;
                        for (int i = 0; i < directoryPaths.Length; ++i)
                        {
                            string dir = directoryPaths[i];
                            if (!string.IsNullOrEmpty(dir))
                            {
                                UtilSystem.StandardizeDirectoryPath(ref dir);
                                if (!dirs.Contains(dir))
                                {
                                    dirs.Add(dir);
                                    ++count;
                                }
                            }
                        }
                    }
                }
            }
            if (count > 0)
            {
                OnChangedImportingStatus();
            }
            return count;
        }


        /// <summary>Removes the specified directory paths to the list of directories that are periodically
        /// checked for settings to be imported.</summary>
        /// <param name="directoryPaths">Paths to be added to the list, can be relative or absolute.</param>
        /// <returns>Number of directories that were actually removed from the list.</returns>
        public int RemoveSettingDirectories(params string[] directoryPaths)
        {
            int count = 0;
            if (directoryPaths != null)
            {
                int numPaths = directoryPaths.Length;
                if (numPaths > 0)
                {
                    lock (Lock)
                    {
                        List<string> dirs = SettingDirectories;
                        for (int i = 0; i < directoryPaths.Length; ++i)
                        {
                            string dir = directoryPaths[i];
                            if (!string.IsNullOrEmpty(dir))
                            {
                                UtilSystem.StandardizeDirectoryPath(ref dir);
                                if (dirs.Contains(dir))
                                {
                                    dirs.Remove(dir);
                                    ++count;
                                }
                            }
                        }
                    }
                }
            }
            if (count > 0)
            {
                OnChangedImportingStatus();
            }
            return count;
        }


        /// <summary>Check the directory with the specified path for instructions to import the settings file,
        /// imports the settings file if instructed so and if the settings file exists, and eventualy
        /// removes the message instructing a single import.
        /// <para>Returns true if settings were imported, false otherwise.</para></summary>
        /// <param name="directoryPath">Path to the directory where settings are checked.</param>
        protected bool CheckAndImport(string directoryPath)
        {
            bool imported = false;
            if (Directory.Exists(directoryPath))
            {
                bool lockAcquired = false;
                try
                {
                    lockAcquired = LockFileMutex.WaitOne(-1 /* wait indefinitely */);
                    string pathMsgImportOnce = Path.Combine(directoryPath, MsgImportOnceFilename);
                    if (File.Exists(pathMsgImportOnce))
                    {
                        string importPath = Path.Combine(directoryPath, ImportingFilename);
                        if (File.Exists(importPath))
                        {
                            Import(importPath);
                            File.Delete(importPath);
                            imported = true;
                        }
                    }
                    else
                    {
                        string pathMsgImportContinuously = Path.Combine(directoryPath, MsgImportContinuoslyFilename);
                        if (File.Exists(pathMsgImportContinuously))
                        {
                            string importPath = Path.Combine(directoryPath, ImportingFilename);
                            if (File.Exists(importPath))
                            {
                                Import(importPath);
                                imported = true;
                            }
                        }
                    }
                }
                catch (AbandonedMutexException ex)
                {
                    if (ex.Mutex != null)
                    {
                        ex.Mutex.ReleaseMutex();
                        lockAcquired = false;
                    }
                }
                finally
                {
                    if (lockAcquired)
                        LockFileMutex.ReleaseMutex();
                }
            }
            return imported;
        }


        /// <summary>Periodically checks the designated directories for settings to be imported, and imports them 
        /// if availeble.
        /// <para>Check and import is performed by the <see cref="CheckAndImport"/> method.</para></summary>
        protected void ImportPeriodically()
        {
            bool doCheckAndImport = true;
            while (doCheckAndImport)
            {
                if (DoImportPeriodically)
                {
                    string[] settingsPaths;
                    lock (Lock)
                    {
                        settingsPaths = SettingDirectories.ToArray();
                    }
                    if (settingsPaths != null)
                    {
                        foreach (string path in settingsPaths)
                        {
                            CheckAndImport(path);
                        }
                    }
                }
                Util.SleepSeconds(ImportCheckPeriodSeconds);
            }
        }

        /// <summary>Returns a flag indicating whether the state of the current setting object is such that
        /// periodic checks and imports of settings form files are performed.</summary>
        public bool IsPeriodicImportCheckPerformed()
        {
            lock (Lock)
            {
                return (DoImportPeriodically && SettingDirectories.Count > 0);
            }
        }


        protected Thread _importingThread;

        /// <summary>Starts the thread that periodically checks for settings to be imported and imports them.</summary>
        protected void StartImportingThreadIfNotRunning()
        {
            lock (Lock)
            {
                if (_importingThread != null)
                {
                    if (!_importingThread.IsAlive)
                        _importingThread = null;
                }
                if (_importingThread == null)
                {
                    _importingThread = new Thread(ImportPeriodically);
                    _importingThread.IsBackground = true;
                    _importingThread.Start();
                }
            }
        }

        /// <summary>Stops the thread that periodically checks for settings to be imported and imports them.</summary>
        protected void StopImportingThreadIfRunning()
        {
            Thread runninghread = null;
            lock (Lock)
            {
                if (_importingThread != null)
                {
                    if (_importingThread.IsAlive)
                    {
                        runninghread = _importingThread;
                    }
                }
                _importingThread = null;
            }
            if (runninghread != null)
            {
                try
                {
                    runninghread.Abort();
                    runninghread.Join();
                }
                catch { }
            }
        }


        /// <summary>Method that is called every time the state of variables that influence how
        /// periodic importing is done, is changed.</summary>
        /// <remarks>This method checks whether periodic checking should be performed or not.
        /// If it should be performed, then it starts the thread that does it, if it is not already running.
        /// If it sould not be performed, then it stops that tread if it is running.</remarks>
        protected void OnChangedImportingStatus()
        {
            if (IsPeriodicImportCheckPerformed())
                StartImportingThreadIfNotRunning();
            else
                StopImportingThreadIfRunning();
        }


        #endregion AutoImport

    }


}
