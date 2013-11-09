using System;
using System.Windows.Forms;

namespace IG.Lib
{
	/// <summary>Classes with Execute() method.</summary>
	public interface IRunnableOld
	{
		void Run();
	}

    public class JustSomething
    {

        public void Execute()
        {
            int a = 1;
            int i;
            MessageBox.Show("First message box.");

            MessageBox.Show("This is a testmessage");

        }
    }

}
