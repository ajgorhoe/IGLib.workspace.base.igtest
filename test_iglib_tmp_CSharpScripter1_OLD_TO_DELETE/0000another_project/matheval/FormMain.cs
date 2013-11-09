using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

namespace CSharpScripter
{
	/// <summary>
	/// Zusammenfassung für Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox rtfCode;
		private System.Windows.Forms.Button btnCompile;
		private System.Windows.Forms.Button btnExecute;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.Button btnLoad;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormMain()
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

			//
			// TODO: Fügen Sie den Konstruktorcode nach dem Aufruf von InitializeComponent hinzu
			//
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.rtfCode = new System.Windows.Forms.RichTextBox();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtfCode
            // 
            this.rtfCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfCode.BackColor = System.Drawing.Color.Gainsboro;
            this.rtfCode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfCode.Location = new System.Drawing.Point(8, 8);
            this.rtfCode.Name = "rtfCode";
            this.rtfCode.Size = new System.Drawing.Size(624, 280);
            this.rtfCode.TabIndex = 0;
            this.rtfCode.Text = "";
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompile.Location = new System.Drawing.Point(104, 296);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(75, 23);
            this.btnCompile.TabIndex = 1;
            this.btnCompile.Text = "Compile";
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExecute.Location = new System.Drawing.Point(184, 296);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(560, 296);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 3;
            this.btnQuit.Text = "Quit";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(8, 296);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(88, 23);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load Example";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(640, 325);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.rtfCode);
            this.Name = "FormMain";
            this.Text = "CSharpScripter";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormMain());
		}

		private void btnQuit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void btnLoad_Click(object sender, System.EventArgs e)
		{
			this.rtfCode.Text = "using System;" + System.Environment.NewLine;
			this.rtfCode.Text += "using System.Collections;" + System.Environment.NewLine;
			this.rtfCode.Text += "using System.Xml;" + System.Environment.NewLine;
			this.rtfCode.Text += "using System.IO;" + System.Environment.NewLine;
			this.rtfCode.Text += "using System.Windows.Forms;" + System.Environment.NewLine;
			this.rtfCode.Text += System.Environment.NewLine;
			this.rtfCode.Text += "namespace CSharpScripter" + System.Environment.NewLine;
			this.rtfCode.Text += "{" + System.Environment.NewLine;
			this.rtfCode.Text += "	public class TestClass : CSharpScripter.Command" + System.Environment.NewLine;
			this.rtfCode.Text += "	{" + System.Environment.NewLine;
			this.rtfCode.Text += "		public TestClass()" + System.Environment.NewLine;
			this.rtfCode.Text += "		{" + System.Environment.NewLine;
			this.rtfCode.Text += "		}" + System.Environment.NewLine;
			this.rtfCode.Text += System.Environment.NewLine;
			this.rtfCode.Text += "		public void Execute() " + System.Environment.NewLine;
			this.rtfCode.Text += "		{" + System.Environment.NewLine;
			this.rtfCode.Text += "			MessageBox.Show(\"This is a testmessage\");" + System.Environment.NewLine;
			this.rtfCode.Text += "		}" + System.Environment.NewLine;
			this.rtfCode.Text += "	}" + System.Environment.NewLine;
			this.rtfCode.Text += "}" + System.Environment.NewLine;
		}

		private void btnCompile_Click(object sender, System.EventArgs e)
		{
            string librarypath;
            CSharpCodeProvider csp = new CSharpCodeProvider();
			ICodeCompiler cc = csp.CreateCompiler();
			CompilerParameters cp = new CompilerParameters();

            librarypath = Application.StartupPath + "\\TestClass.dll";
            // Selete the dll file if it exists:
            if (File.Exists(librarypath))
            {
                try
                {
                    File.Delete(librarypath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File could not be deleted: \n  " + librarypath+
                        "\n\nError description:  \n" + ex.Message);
                }
            }

			cp.OutputAssembly = Application.StartupPath + "\\TestClass.dll";
			cp.ReferencedAssemblies.Add("System.dll");
			cp.ReferencedAssemblies.Add("System.dll");
			cp.ReferencedAssemblies.Add("System.Data.dll");
			cp.ReferencedAssemblies.Add("System.Xml.dll");
			cp.ReferencedAssemblies.Add("mscorlib.dll");
			cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			cp.ReferencedAssemblies.Add("CSharpScripter.exe");
						
			cp.WarningLevel = 3;

			cp.CompilerOptions = "/target:library /optimize";
			cp.GenerateExecutable = false;
			cp.GenerateInMemory = false;

			System.CodeDom.Compiler.TempFileCollection tfc = new TempFileCollection(Application.StartupPath, false);
			CompilerResults cr  = new CompilerResults(tfc);

			cr = cc.CompileAssemblyFromSource(cp, this.rtfCode.Text);

			if (cr.Errors.Count > 0) 
			{
				foreach (CompilerError ce in cr.Errors) 
				{
					Console.WriteLine(ce.ErrorNumber + ": " + ce.ErrorText);
				}
				MessageBox.Show(this, "Errors occoured", "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.btnExecute.Enabled = false;
			} 
			else 
			{
				this.btnExecute.Enabled = true;
			}
			
			System.Collections.Specialized.StringCollection sc = cr.Output;
			foreach (string s in sc) 
			{
				Console.WriteLine(s);
			}
            Console.WriteLine("Compiling has been completed.\n");
		}

		private void CheckErrors(CompilerErrorCollection cec) 
		{

		}

		private void btnExecute_Click(object sender, System.EventArgs e)
		{
			AppDomainSetup ads = new AppDomainSetup();
			ads.ShadowCopyFiles = "true";
			AppDomain.CurrentDomain.SetShadowCopyFiles();

			AppDomain newDomain = AppDomain.CreateDomain("newDomain");
      
			byte[] rawAssembly = loadFile("TestClass.dll");
            // byte[] rawAssembly = loadFile(Application.StartupPath + "TestClass.dll");
			Assembly assembly = newDomain.Load(rawAssembly, null);

			Command testClass = (Command)assembly.CreateInstance("CSharpScripter.TestClass");
			testClass.Execute();

			testClass = null;
			assembly = null;

			// AppDomain.Unload(newDomain);
			newDomain = null;
		}

		private byte[] loadFile(string filename) 
		{
			FileStream fs = new FileStream(filename, FileMode.Open);
			byte[] buffer = new byte[(int) fs.Length];
			fs.Read(buffer, 0, buffer.Length);
			fs.Close();
			fs = null;
			return buffer;
		}


	}
}
