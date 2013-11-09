using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using System.Net;
using System.Net.Sockets;
using System.IO;

using IG.Forms;


namespace IG.Forms
{




    public class TestTcpClient
    {

        public int Port = 8000;
        public string IPAddr = "127.0.0.1";

        private int MaxReportLength = 60;

        public int ID = 0;
        private static int numinstances = 0;  // Number of instances created in the current application.

        private string ReceivedMessage = null;
        private int NumRequests = 0;


        public TestTcpClient() 
        {
            ++numinstances;
            ID = numinstances;
        }

        public TestTcpClient(string IP, int portnum)
        // Constructor that assigns user defined IP address and port number.
        {
            ++numinstances;
            ID = numinstances;
            IPAddr = IP;
            Port = portnum;
        }


        private TcpClient client = null;
        NetworkStream stream = null;
        private BinaryWriter w = null;
        private BinaryReader r=null;
        private bool Connected = false;



        public void Connect()
        {
            try
            {
                NumRequests = 0;
                client = new TcpClient();
                Report("Attempting to connect to TCP Server at " + IPAddr + ":" + Port.ToString() + ".");
                client.Connect(IPAddress.Parse(IPAddr), Port);
                Report("Connection established.");
                // Retrieve the network stream.
                stream = client.GetStream();
                // Create a BinaryWriter for writing to the stream.
                w = new BinaryWriter(stream);
                // Create a BinaryReader for reading from the stream.
                r = new BinaryReader(stream);
                // Ask for Connection confirmation.
                w.Write(ClientMessages.RequestConnect);
                ReceivedMessage=r.ReadString();
                if (ReceivedMessage == ServerMessages.ConnectAcknowledged)
                {
                    Report("Connection acknowledged by the server.\n");
                    Connected = true;
                }
                else
                {
                    ReportError("Connection could not be completed.\n");
                    Connected = false;
                }
            }
            catch (Exception e) { ReportError(e); }
        }

        public void DisConnect()
        {
            try
            {
                Connected = false;
                Report("Disconnecting...");
                w.Write(ClientMessages.DisconnectClient);
                ReceivedMessage = r.ReadString();
                while (ReceivedMessage != ServerMessages.DisconnectAcknowledged)
                {
                    string ShortMsgStr;
                    if (ReceivedMessage.Length > MaxReportLength)
                        ShortMsgStr = ReceivedMessage.Substring(0, MaxReportLength) + "...";
                    else
                        ShortMsgStr = ReceivedMessage;
                    Report("Cleaning (ignoring) server's message in the que:\n  \"" + ShortMsgStr + "\"");
                }
                if (ReceivedMessage == ServerMessages.DisconnectAcknowledged)
                {
                    Report("Disconnect acknowledged by the server.");
                }
                else
                {
                    ReportError("Acknowledgment of disconnection request has not been received.");
                }
                // Close the connection socket.
                client.Close();
                Report("Port closed.");
            }
            catch (Exception e) { ReportError(e); }
        }


        public void Ping()
        {
            try
            {
                if (Connected)
                {
                    ++NumRequests;
                    Report("Sending ping request to the TCP Server at " + IPAddr + ":" + Port.ToString() + "...");
                    w.Write(ClientMessages.PingRequestClient);
                    ReceivedMessage = r.ReadString();
                    if (ReceivedMessage == ServerMessages.PingAcknowledgeSrv)
                        Report("Server answered the Ping request.");
                    else
                        ReportError("Server did not properly answer the Ping request.");

                }
                else
                {
                    Report("Pinging the unconnected TCP Server at " + IPAddr + ":" + Port.ToString() + "...");
                    Connect();
                    w.Write(ClientMessages.PingRequestClient);
                    ReceivedMessage = r.ReadString();
                    if (ReceivedMessage == ServerMessages.PingAcknowledgeSrv)
                        Report("Server answered to Ping request.");
                    else
                        ReportError("Server did not properly answer the ping request.");
                    Connected = false;
                    DisConnect();
                }
            }
            catch (Exception e) { ReportError(e); }
        }

        public void SendData(string Data)
        {
            try
            {
                if (!Connected)
                {
                    ReportError("Can not send the data, not connected!");
                }
                else
                {
                    string ShortDataStr;
                    if (Data.Length > MaxReportLength)
                        ShortDataStr = Data.Substring(0, MaxReportLength) + "...";
                    else
                        ShortDataStr = Data;
                    Report("\nSending data to the server...\nData:\n  \"" + ShortDataStr + "\"\n");
                    w.Write(ClientMessages.SendClient);
                    ReceivedMessage = r.ReadString();
                    if (ReceivedMessage != ServerMessages.ReadySrv)
                    {
                        ReportError("Can not send data, Server did not respond with the Ready message.");
                    }
                    else
                    {
                        w.Write(Data);
                        ReceivedMessage = r.ReadString();
                        if (ReceivedMessage == ServerMessages.ReceivedSrv)
                            Report("Data was sent successfully.");
                        else
                            ReportError("The server did not send data receipt.");
                    }
                }
            }
            catch (Exception e) { ReportError(e); }
        }


        //*********************************
        // Various client implementations:
        //*********************************

        public void ClientTest()
        {
            try
            {
                ReportMarked("ClientTest begin...\n");

                Connect();

                ReportMarked("Ping after connection established:");
                Ping();
                // ClientConsole.ReadLine("Press <Enter> to continue!");


                ReportMarked("Ping 2 after connection established:");
                Ping();
                // ClientConsole.ReadLine("Press <Enter> to continue!");

                //ReportMarked("Sending data to server...");
                //string datastr = "Data String.";
                //while (datastr.Length > 0)
                //{
                //    ClientConsole.ReadString(ref datastr, "Input data string to be sent to server: ");
                //    ReportMarked("Sending string \"" + datastr + "\"...");
                //    SendData(datastr);
                //}



                int averagetime = 3000, deviation = 2000;
                Random rnd = new Random(DateTime.Now.Second);  // time initialization of random generator
                int num = 4+rnd.Next(10);
                ReportMarked("Sending data strings to server...");
                for (int i = 1; i < num; ++i)
                {
                    int sleeptime=averagetime+rnd.Next(deviation);
                    float sleeptimesec=(float) sleeptime/(float) 1000;
                    string msg = "Message " + i.ToString() + "/" + num.ToString() + ".";
                    ReportMarked("Sending string \"" + msg + "\".\n  Next sleep: " 
                        + sleeptimesec.ToString() + " s.\n");
                    SendData(msg);
                    Thread.Sleep(sleeptime);
                }




                //for (int i = 1; i <= 800; ++i) 
                //{
                //    int showtime=averagetime+rnd.Next(deviation);
                //    float f=(float)showtime/(float) 1000;
                //    new FadeMessage("Fade message " + i.ToString() + ", " + f.ToString()+" s", 
                //            averagetime+rnd.Next(deviation), 
                //            0.5);



                    //ReportMarked("Ping before connection established:");
                    //Ping();
                    //ReportMarked("Ping 2 before connection established:");
                    //Ping();

                    ClientConsole.ReadLine("Press <Enter> to DISCONNECT!");
                DisConnect();

                ReportMarked("ClientTest finished.\n");
            }
            catch (Exception e) { ReportError(e); }
        }

        public void ClientExampleMain()
        {

            client = new TcpClient();

            try
            {
                Report("Attempting to connect to  TCP Server at " + IPAddr + ":"+Port.ToString() + ".");
                client.Connect(IPAddress.Parse(IPAddr), Port);
                Report("Connection established.");

                // Retrieve the network stream.
                NetworkStream stream = client.GetStream();

                // Create a BinaryWriter for writing to the stream.
                w = new BinaryWriter(stream);

                // Create a BinaryReader for reading from the stream.
                r = new BinaryReader(stream);

                // Start a dialogue.
                w.Write(ClientMessages.RequestConnect);

                if (r.ReadString() == ServerMessages.ConnectAcknowledged)
                {

                    Report("Connected.");
                    Report("Press Enter to disconnect.");
                    ClientConsole.ReadLine();
                    Report("Disconnecting...");
                    w.Write(ClientMessages.DisconnectClient);
                }
                else
                {
                    Report("Connection not completed.");
                }

                // Close the connection socket.
                client.Close();
                Report("Port closed.");
            }
            catch (Exception err)  { ReportError(err.ToString());  }
            ClientConsole.ReadLine();
            Environment.Exit(0);
        }



        //*****************
        // client console:
        //*****************


        private ConsoleForm clientcons = null,    // holds the application console
                    auxcons;
        private int reccount = 0;
        private static int totcount = 0;
        private bool ConsoleReady()
        {
            try
            {
                if (clientcons != null)
                    if ( // clientcons.IsReady &&
                                !clientcons.IsDisposed && !clientcons.Disposing)
                        return true;
            }
            catch { }
            return false;
        }

        public ConsoleForm ClientConsole
        /// Returns the client's own console.
        {
            get
            {
                if (!ConsoleReady())
                {
                    try
                    {
                        clientcons = new ConsoleForm("TCP Client for " + IPAddr + ":" + Port.ToString() +
                            " (ID=" + ID.ToString() + ")");
                        clientcons.HideInput();
                        clientcons.Disposed += CloseConsoleHandler;
                    }
                    catch (Exception e)
                    {
                        ++reccount;  // recursion counter prevents infinite recursion
                        if (reccount == 1)
                            UtilForms.ReportError(e);
                        --reccount;
                    }
                }
                if (!ConsoleReady())
                {
                    clientcons = null;
                    ++totcount;
                    if (totcount == 1)  // report this error only once
                        UtilForms.ReportError("Client console can not be initialized.");
                }
                return clientcons;
            }
            set
            {
                if (ConsoleReady())
                {
                    UtilForms.ReportWarning("Applicatin console is replaced by another console form.");
                    clientcons.CloseForm();
                };
                clientcons = value;
            }
        }  // ClientConsole


        private void CloseConsoleHandler(object sender, System.EventArgs e)
        // Event handler that exits server if the concole window is closed.
        {
            try
            {
                Thread.Sleep(50);
                // Application.ExitThread();
                // Application.Exit();
                System.Environment.Exit(0);
            }
            catch { }
        }



        //************
        // Reporting:
        //************

        public void Report(string str)
        {
            try
            {
                if (!ConsoleReady())
                    auxcons = ClientConsole; // Try to initialize console bu property get {}
                if (ConsoleReady())
                {
                    ConsoleForm.Styles currentstyle = ClientConsole.Style;
                    ClientConsole.Style = ConsoleForm.Styles.Normal;
                    ClientConsole.WriteLine(
                            "\n< " + DateTime.Now.ToString() + " > \n" +
                            str);
                    ClientConsole.Style = currentstyle;
                }
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportMarked(string str)
        {
            try
            {
                if (!ConsoleReady())
                    auxcons = ClientConsole; // Try to initialize console bu property get {}
                if (ConsoleReady())
                {
                    ConsoleForm.Styles currentstyle = ClientConsole.Style;
                    ClientConsole.Style = ConsoleForm.Styles.Mark;
                    ClientConsole.WriteLine(
                            "\n< " + DateTime.Now.ToString() + " > \n" +
                            str);
                    ClientConsole.Style = currentstyle;
                }
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportError(string str)
        {
            try
            {
                if (!ConsoleReady())
                    auxcons = ClientConsole; // Try to initialize console bu property get {}
                if (ConsoleReady())
                {
                    ConsoleForm.Styles currentstyle = ClientConsole.Style;
                    ClientConsole.Style = ConsoleForm.Styles.Error;
                    ClientConsole.WriteLine(
                            "\n\nERROR: < " + DateTime.Now.ToString() + " > \n" +
                            str + "\n");
                    ClientConsole.Style = currentstyle;
                }
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportError(Exception e,string additionalmessage)
        {
            try
            {
                string str = e.Message;
                if (additionalmessage != null)
                    if (additionalmessage.Length > 0)
                        str += "\n" + additionalmessage;
                ReportError(str);
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportError(Exception e) { ReportError(e, null); }
           


    } // Class  TestTcpClient






    static class client_program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            TestTcpClient client = new TestTcpClient();
            client.ClientTest();
            //client.ClientExampleMain();
        

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClientForm());
        }
    }


}  // namespace IG.Forms
