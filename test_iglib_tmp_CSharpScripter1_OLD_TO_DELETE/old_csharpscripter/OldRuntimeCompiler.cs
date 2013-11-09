using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Diagnostics;


namespace IG.Lib
{


    public class RuntimeCompilerOld
    {

        #region Constructors


        #endregion Constructors


        #region Data

        private Logger _logger = new Logger();

        public Logger Logger
        {
            get { return _logger; }
        }

        /// <summary>Gets code base (a template code for compiling) where names of classes, 
        /// methods etc. are properly set.</summary>
        public virtual string CodeBase
        {
            get
            {
                return @"
using System;
using System.Collections;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using IG.Lib;

namespace IG.Lib
{
	public class " + RunnableClass + ": " + RunnableInterface + @"
	{
		public TestClass()
		{
		}

		public void " + RunnableFunction + @"() 
		{
			MessageBox.Show(""This is a testmessage."");
		}
	}
}
";
            }
        }

        protected string _code = null;

        /// <summary>Gets or sets code to be compiled.
        /// Get: If not assigned explicitly then CodeBase is taken.</summary>
        public string Code
        {
            get 
            {
                if (_code == null)
                    _code = CodeBase;
                return _code;
            }
            set { _code = value; }
        }


        protected string _runnableNamespace = "IG.Lib";
        protected string _runnableClass = "TestClass";
        protected string _runnableInterface = "IRunnableOld";
        protected string _runnableFunction = "Run";

        public string RunnableNamespace
        {
            get { return _runnableNamespace; }
            protected set { _runnableNamespace = value; }
        }

        public string RunnableInterface
        {
            get { return _runnableInterface; }
            protected set { _runnableInterface = value; }
        }

        public string RunnableClass
        {
            get { return _runnableClass; }
            protected set { _runnableClass = value; }
        }

        public string RunnableFullClass
        {
            get { return RunnableNamespace + "." + RunnableClass; }
        }

        public string RunnableFunction
        {
            get { return _runnableFunction; }
            protected set { _runnableFunction = value; }
        }

        protected string _libraryOriginalName = "TestClass";

        protected int _idLibraryName = 0;

        protected string _libraryFileName = "TestClass.dll";

        /// <summary>Generates a new library file name.</summary>
        protected void GenerateNewLibraryName()
        {
            ++_idLibraryName;
            _libraryFileName = _libraryOriginalName + _idLibraryName.ToString() + "dll";
        }


        /// <summary>Returns the directory containing the executable that started the current
        /// application.</summary>
        public string GetExecutableDirectory()
        {
            // return Application.StartupPath;
            // string executableFilePath = Assembly.GetEntryAssembly().Location;
            string executableFilePath = Process.GetCurrentProcess().MainModule.FileName;
            return Path.GetDirectoryName(executableFilePath);
        }
        
        private string _executableDirectory = null;

        /// <summary>Returns directory where library will be compiled, which will be the
        /// directory of the executable that started the application.</summary>
        public virtual string LibraryDirectory
        {
            get 
            {
                if (_executableDirectory == null)
                {
                    _executableDirectory = GetExecutableDirectory();
                }
                return _executableDirectory;
            }
        }

        /// <summary>Path to the dll where code is compiled.
        /// We take the directory where executable is located.</summary>
        public virtual string LibraryPath
        {
            get 
            {
                return Path.Combine(LibraryDirectory, "TestClass.dll"); 
            }
        }

        protected bool _readyToRun = false;

        /// <summary>Wheather the compiled class' method is ready to run.</summary>
        public virtual bool ReadyToRun
        {
            get { return _readyToRun; }
            set { _readyToRun = value; }
        }

        #endregion Data


        #region Operation



        /// <summary>Copies the specified file to byte array and returns it.</summary>
        /// TODO: move this to utils or something like that!
        public byte[] LoadFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            fs = null;
            return buffer;
        }


        public void Compile()
        {
            if (File.Exists(LibraryPath))
            {
                try
                {
                    File.Delete(LibraryPath);
                    Console.WriteLine(Environment.NewLine + "Library file deleted: " + LibraryPath
                        + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Logger.LogError("File could not be deleted: \n  " + LibraryPath, ex);
                }
            }


            CompilerResults compResults = null;

            

            CSharpCodeProvider compiler = new CSharpCodeProvider();
            ICodeCompiler cc = compiler.CreateCompiler();
            CompilerParameters compilerParameters = new CompilerParameters();

            compilerParameters.OutputAssembly = LibraryPath;
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
            compilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compilerParameters.ReferencedAssemblies.Add("CSharpScripter.exe");

            compilerParameters.WarningLevel = 3;

            compilerParameters.CompilerOptions = "/target:library /optimize";
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = false;

            System.CodeDom.Compiler.TempFileCollection tfc = new TempFileCollection(LibraryDirectory, false);
            compResults = new CompilerResults(tfc);

            compResults = cc.CompileAssemblyFromSource(compilerParameters, Code);

            

            if (compResults.Errors.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("The following compiler errors occurred:");
                int numErrors = 0, numWarnings = 0;
                foreach (CompilerError ce in compResults.Errors)
                {
                    if (ce.IsWarning)
                    {
                        Logger.LogWarning("Compiler warning No. " + ce.ErrorNumber 
                            + " at (" + ce.Line + ", " + ce.Column + ") in " + ce.FileName + ": "
                            + Environment.NewLine + "  " + ce.ErrorText);
                        ++numWarnings;
                    }
                    else
                    {
                        Logger.LogError("Compiler ERROR No. " + ce.ErrorNumber 
                            + " at (" + ce.Line + ", " + ce.Column + ") in " + ce.FileName + ": "
                            + Environment.NewLine + "  " + ce.ErrorText);
                        ++numErrors;
                    }
                }
                if (numErrors > 0)
                    this.ReadyToRun = false;
                else
                    this.ReadyToRun = true;
            }
            else
            {
                this.ReadyToRun = true;
            }

            System.Collections.Specialized.StringCollection sc = compResults.Output;
            foreach (string s in sc)
            {
                Logger.LogInfo("Compiler: " + s);
            }
        }


        #endregion Operation 


    }  // class RuntimeCompiler

}