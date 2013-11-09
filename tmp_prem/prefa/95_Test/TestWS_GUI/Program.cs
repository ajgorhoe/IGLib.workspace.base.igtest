using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestWS_GUI
{
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form form = new Form1();
            form.Height = 500;
            form.Width = 700;
            form.Text = "Testiranje WS";
            form.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(form);
        }
    }
}
