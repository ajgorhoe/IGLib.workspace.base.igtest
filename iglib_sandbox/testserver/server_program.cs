using System;
using System.Collections;
// using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using System.Net;
using System.Net.Sockets;
using System.IO;

using IG.Forms;

namespace IG.Forms
{


    public class ServerMessages
    {
        public const string ConnectAcknowledged = "OK";
        public const string DisconnectAcknowledged = "DisconnectOK";
        public const string PingRequestSrv = "Ping";
        public const string PingAcknowledgeSrv = "PingOK";
        public const string UnknownRequestSrv = "Unknown";
        public const string TooManyConnectionsSrv = "TooMany";
        public const string DisconnectSrv = "Bye";
        public const string ReadySrv = "ServerReady";
        public const string SendSrv = "SendSrv";
        public const string ReceivedSrv = "ReceivedSrv";
    }

    public class ClientMessages
    {
        public const string RequestConnect = "Hello";
        public const string PingRequestClient = "Ping";
        public const string PingAcknowledgedClient = "PingOK";
        public const string UnknownRequestClient = "Unknown";
        public const string DisconnectClient = "Bye";
        public const string ReadyClient = "ClientReady";
        public const string SendClient = "SendClient";
        public const string ReceivedClient = "ReceivedClient";
    }


    public class TestTcpMultiThreadedServer
    {

        public int Port = 8000;
        public string IPstr = "127.0.0.1";
        public bool SingleClient = false;
        public int MaxClients = 20; /// Maximal permitted number of clients.
        public int SleepBetweenLaunches = 200;  /// Minimum time in ms between successive launches.
                                              // This prevents overflow attacks, together with maxclients.
        public bool Stop = false;  /// Setting this to true makes the server loop exit.
        public int ID = 0;
        public string Name="";  // Server name
        private static int numinstances = 0;  // Number of instances created in the current application.



        private int NumLaunched=0;  /// number of launched connections.
                                   // This includes the last one where connnection is not yet established.
        internal int NumActiveConnections = 0; // Number of active connections
        TestTcpServer LastServer=null;
        ArrayList Servers=new ArrayList();



        public TestTcpMultiThreadedServer ()
        /// Constructor with default IP address and port.
        {
            ++numinstances;
            ID=numinstances;
            Name = "TCP Multi-client Server " + ID.ToString();
        }

        public TestTcpMultiThreadedServer (string IP, int port)
        // Constructor that assigns user defined IP address and port number.
        {
            ++numinstances;
            ID=numinstances;
            Name = "TCP Multi-client Server " + ID.ToString();
            Port = port;
            IP = IPstr;
        }



        public Thread ListenerThread = null;

        private Thread LastServerThread;
        private object ServerLaunchLock = new object();
        private TcpListener listener = null;
        private TcpClient client = null, ClientForServer=null;
        private int LaunchingServer = 0;


        private void ServerThreadFunc()
        {
            UtilForms.WriteLine("    >> Starting server thread, NumLaunched = " + NumLaunched);
            try
            {
                lock (ServerLaunchLock)
                {
                    try
                    {
                        // Create a new server and prepare its data:
                        LastServer = new TestTcpServer(IPstr, Port);
                        LastServer.ID = NumLaunched;
                        LastServer.Name = "TCP Server " + (NumLaunched).ToString();
                        LastServer.parent = this;
                        LastServer.SingleClient = SingleClient;
                        LastServer.client = ClientForServer;
                        if (NumLaunched > MaxClients)
                        {
                            // The maximum number of connections has already beenreached. A new server is still
                            // launched, but it will just establish a connection, inform the client thet the
                            // maximum number of connections has been reached, and close.
                            LastServer.TooManyConnections=true;
                        }
                    }
                    catch (Exception e) { UtilForms.ReportError(e); }
                    --LaunchingServer;  // Notify that launching has been completed
                    UtilForms.WriteLine("    >> LaunchingServer decremented, value = " + LaunchingServer);
                    Servers.Add(LastServer);
                }  // lock (ServerLaunchLock)
                // Start the new server:
                LastServer.StartServer();
            }
            catch (Exception e) { UtilForms.ReportError(e); }
        }


        public void StartServer()
        /// Launches a loop that listens for connection requests and in response launches servers for them.
        {
            ListenerThread = new Thread (new ThreadStart( ListenerLoop ));
            ListenerThread.IsBackground = false;
            ListenerThread.Start();
        }

        private void ListenerLoop()
        {
            try
            {
                // Create a new listener on port 8000.
                try
                {
                    listener = new TcpListener(IPAddress.Parse(IPstr), Port);
                    listener.Start();
                }
                catch (Exception e) { UtilForms.ReportError(e, "Problem with 'new TcpListener(...)'."); }
                while (!Stop)
                {
                    // Listen for connection requests, establis connections with clients and launch servers
                    // in separate threadd:
                    try
                    {
                        UtilForms.WriteLine("\nWaiting for connection request, NumLaunched = "+NumLaunched);
                        UtilForms.WriteLine("                                  NumActive = "+NumActiveConnections);
                        UtilForms.WriteLine("                                  LaunchingServer = " + LaunchingServer);

                        // Listen on the port and wait for the next connection request:
                        client = listener.AcceptTcpClient();

                        UtilForms.WriteLine("Request received, wait for: LaunchingServer = " + LaunchingServer);

                        // Wait until last launch is completed, such that data for the new server
                        // can be put on temporary storage (i.e. not to overwrite the data that 
                        // is being passed by the current launc):
                        while (LaunchingServer > 0)
                            Thread.Sleep(10);  // this will seldom occur
                        ++LaunchingServer;
                        // Prepare data for the newly launched server
                        ClientForServer = client;
                        // Launch a new server 
                        ++NumLaunched;
                        ++NumActiveConnections;
                        LastServerThread = new Thread(new ThreadStart(ServerThreadFunc));
                        LastServerThread.IsBackground = true;
                        LastServerThread.Start();

                        // Code below helps to prevent overflows by enforcing some minimal pause between
                        // successive server launches:
                        if (SleepBetweenLaunches > 0)
                            Thread.Sleep(SleepBetweenLaunches);
                    }
                    catch (Exception e) { UtilForms.ReportError(e, "Problem with 'listener.AcceptTcpClient()'."); }

                }  // while (!Stop)
                
            }
            catch (Exception e) { UtilForms.ReportError(e); }
        }
    }


    public class TestTcpServer
    {
        public int Port = 8000;
        public string IPstr = "127.0.0.1";
        public int ID=0;
        public string Name = "";  // Server name
        public bool SingleClient = true;
        public bool 
            TooManyConnections = false,
            StopRequested = false;
        public TestTcpMultiThreadedServer parent;  /// Parent that launhed this server instance.

        public Thread ServerThread=null;
        private int MaxReportLength = 60;
        private static int numinstances = 0;  // Number of instances created by the current application


        private string 
            ReceivedMessage=null,
            ReceivedData=null;
        private bool Stop=false;
        private int NumRequests = 0;
        
        private TcpListener listener = null;
        internal TcpClient client = null;
        NetworkStream stream = null;
        private BinaryWriter SWriter = null;
        private BinaryReader SReader = null;


        public TestTcpServer() 
        /// Constructor with default IP address and port (local:8000).
        {
            ++numinstances;
            ID = numinstances;
            Name = "TCP Server " + ID.ToString();
        }

        public TestTcpServer(string IP, int port)
        // Constructor that assigns user defined IP address and port number.
        {
            ++numinstances;
            ID = numinstances;
            Name = "TCP Server " + ID.ToString();
            Port = port;
            IP = IPstr;
        }


        public void StopRequest()
        /// A gentle way to stop the server.
        {
            StopRequested=true;
        }

        private void SendData(string Data)
        {
            try
            {
                string ShortDataStr;
                if (Data.Length > MaxReportLength)
                    ShortDataStr = Data.Substring(0, MaxReportLength) + "...";
                else
                    ShortDataStr = Data;
                Report("\nSending data to the client...\nData:\n  \"" + ShortDataStr + "\"\n");
                SWriter.Write(ServerMessages.SendSrv);
                ReceivedMessage = SReader.ReadString();
                if (ReceivedMessage != ClientMessages.ReadyClient)
                {
                    ReportError("Can not send data, Client did not respond with the Ready message.");
                }
                else
                {
                    SWriter.Write(Data);
                    ReceivedMessage = SReader.ReadString();
                    if (ReceivedMessage == ClientMessages.ReceivedClient)
                        Report("Data was sent successfully.");
                    else
                        ReportError("The client did not send data receipt.");
                }
            }
            catch (Exception e) { ReportError(e); }
        }

        

        public void StartServer()
        {

            try
            {
                ServerThread = Thread.CurrentThread;
                if (client == null)  /* listener needs to be launched first */
                {
                    // Create a new listener on port 8000.
                    try
                    {
                        listener = new TcpListener(IPAddress.Parse(IPstr), Port);
                    }
                    catch (Exception e) { ReportError(e, "Problem with 'new TcpListener(...)'."); }
                    try
                    {
                        Report("Initializing TCP Server at " + IPstr + ":" + Port.ToString() + ".");
                        listener.Start();
                    }
                    catch (Exception e) { ReportError(e, "Problem with 'listener.Start()'."); }

                    try
                    {
                        Report("Listening for a connection...");

                        // Wait for a connection request, 
                        // and return a TcpClient initialized for communication. 
                        client = listener.AcceptTcpClient();

                        // Connection established (but not confirmed), start server in a new thread:
                    }
                    catch (Exception e) { ReportError(e, "Problem with 'listener.AcceptTcpClient()'."); }
                }
                else
                {
                    // client!=NULL (i.e. it was provided from outside):
                    Report("Initializing TCP Server at " + IPstr + ":" + Port.ToString() + ".");
                }

                Report("Connection established.");

                // Retrieve the network stream.
                stream = client.GetStream();

                // Create a BinaryWriter for writing to the stream.
                SWriter = new BinaryWriter(stream);

                // Create a BinaryReader for reading from the stream.
                SReader = new BinaryReader(stream);

                while (!Stop)
                {
                    if (TooManyConnections)
                        Stop = true;
                    if (StopRequested)
                        Stop = true;
                    try
                    {
                        ReceivedMessage = SReader.ReadString();
                        ++NumRequests;
                        if (NumRequests == 1)
                        {
                            // First message
                            if (ReceivedMessage == ClientMessages.RequestConnect)
                            {
                                // connection request received - required to be the first client message
                                // by the protocole
                                if (!Stop)
                                {
                                    Report("Connection acknowledged.");
                                    SWriter.Write(ServerMessages.ConnectAcknowledged);
                                }
                                else
                                {
                                }
                            }
                            else
                            {
                                string ShortMsgStr;
                                if (ReceivedMessage.Length > MaxReportLength)
                                    ShortMsgStr = ReceivedMessage.Substring(0, MaxReportLength) + "...";
                                else
                                    ShortMsgStr = ReceivedMessage;
                                ServerConsole.ReportError("Invalid first message:\n  \""
                                    + ShortMsgStr + "\".");
                                Report("\nCould not complete connection, Disconnect.\n");
                                SWriter.Write(ServerMessages.DisconnectSrv);
                                Stop = true;
                            }
                        }
                        else
                        {
                            if (ReceivedMessage == ClientMessages.PingRequestClient)
                            {
                                Report("Ping answered.");
                                SWriter.Write(ServerMessages.PingAcknowledgeSrv);
                            }
                            else if (ReceivedMessage == ClientMessages.DisconnectClient)
                            {
                                Report("Disconnect request received...");
                                SWriter.Write(ServerMessages.DisconnectAcknowledged);
                                Stop = true;
                            }
                            else if (ReceivedMessage == ClientMessages.ReadyClient)
                            {
                                Report("Client has sent \"Ready\" notification.");
                            }
                            else if (ReceivedMessage == ClientMessages.SendClient)
                            {
                                // Accept data sent by client...
                                Report("\nClient will send data, sending back Ready message...");
                                SWriter.Write(ServerMessages.ReadySrv);
                                ReceivedData = SReader.ReadString();
                                Report("Data received, sending back the Receipt.");
                                SWriter.Write(ServerMessages.ReceivedSrv);
                                string ShortDataStr;
                                if (ReceivedData.Length > MaxReportLength)
                                    ShortDataStr = ReceivedData.Substring(0, MaxReportLength) + "...";
                                else
                                    ShortDataStr = ReceivedData;
                                Report("The following data was sent by the client:\n  \"" +
                                    ShortDataStr + "\"\n\n");
                            }
                            else
                            {
                                // Unknown message...
                                string ShortMsgStr;
                                if (ReceivedMessage.Length > MaxReportLength)
                                    ShortMsgStr = ReceivedMessage.Substring(0, MaxReportLength) + "...";
                                else
                                    ShortMsgStr = ReceivedMessage;
                                ServerConsole.ReportError("Unknown message in this context:\n  \""
                                    + ShortMsgStr + "\".");
                                // Notify teh client that message was not understood:
                                SWriter.Write(ServerMessages.UnknownRequestSrv);
                            }
                        }
                    }  // try{...} - one iteration of StartServer()
                    catch (Exception e) { ReportError(e); }
                }  // while (!Stop)

                // Close the connection socket.
                try
                {
                    if (TooManyConnections)
                    {
                        Report("Connection rejected due to too many open connections.");
                        SWriter.Write(ServerMessages.TooManyConnectionsSrv);
                    }
                    if (StopRequested)
                    {
                        Report("Connection stopped on programatic request.");
                    }
                    SWriter.Write(ServerMessages.DisconnectSrv);  // Sent closing message to a client.
                }
                catch (Exception e) { ReportError(e,"Problem in notifying client about closing the connection."); }
                try
                {
                    // Close connection:
                    client.Close();
                    if (parent != null)
                        --parent.NumActiveConnections;
                    Report("Connection closed.");
                    if (listener != null)
                    {
                        // Also stop listening for new requests if this was a single thread server.
                        listener.Stop();
                        Report("Listener stopped.");
                    }
                }
                catch (Exception e) { ReportError(e,"Closing the connection socket"); }

                // Dispose resources:
                if (servercons != null)
                    servercons.CloseForm();

            }  // Outer try {} of StartServer()
            catch (Exception e) { ReportError(e); }
        }  // StartServer()




        //*****************
        // Server console:
        //*****************

        private IG.Forms.ConsoleForm servercons = null,    // holds the application console
                    auxcons;
        private int reccount = 0;
        private static int totcount = 0;
        private bool ConsoleReady()
        {
            try
            {
                if (servercons != null)
                    if ( // servercons.IsReady &&
                                !servercons.IsDisposed && !servercons.Disposing)
                        return true;
            }
            catch { }
            return false;
        }

         public ConsoleForm ServerConsole
        /// Returns the server's own console.
        {
            get
            {
                if (!ConsoleReady())
                {
                    try
                    {
                        servercons = new ConsoleForm("TCP Server at " + IPstr + ":"+Port.ToString()+
                                " (" + Name + ")" );
                        servercons.HideInput();
                        servercons.Disposed += CloseConsoleHandler;
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
                    servercons = null;
                    ++totcount;
                    if (totcount == 1)  // report this error only once
                        UtilForms.ReportError("Server console can not be initialized.");
                }
                return servercons;
            }
            set
            {
                if (ConsoleReady())
                {
                    UtilForms.ReportWarning("Applicatin console is replaced by another console form.");
                    servercons.CloseForm();
                };
                servercons = value; 
            }
        }  // ServerConsole

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
             catch{}
         }


         //************
         // Reporting:
         //************


        public void Report(string str)
        {
            try
            {
                if (!ConsoleReady())
                    auxcons = ServerConsole; // Try to initialize console bu property get {}
                if (ConsoleReady())
                {
                    ConsoleForm.Styles currentstyle = ServerConsole.Style;
                    ServerConsole.Style = ConsoleForm.Styles.Normal;
                    ServerConsole.WriteLine(
                            "\n< " + DateTime.Now.ToString() + " > \n" +
                            str);
                    ServerConsole.Style = currentstyle;
                }
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportMarked(string str)
        {
            try
            {
                if (!ConsoleReady())
                    auxcons = ServerConsole; // Try to initialize console bu property get {}
                if (ConsoleReady())
                {
                    ConsoleForm.Styles currentstyle = ServerConsole.Style;
                    ServerConsole.Style = ConsoleForm.Styles.Mark;
                    ServerConsole.WriteLine(
                            "\n< " + DateTime.Now.ToString() + " > \n" +
                            str);
                    ServerConsole.Style = currentstyle;
                }
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportError(string str)
        {
            try
            {
                if (!ConsoleReady())
                    auxcons = ServerConsole; // Try to initialize console bu property get {}
                if (ConsoleReady())
                {
                    ConsoleForm.Styles currentstyle = ServerConsole.Style;
                    ServerConsole.Style = ConsoleForm.Styles.Error;
                    ServerConsole.WriteLine(
                            "\n\nERROR: < " + DateTime.Now.ToString() + " > \n" +
                            str + "\n");
                    ServerConsole.Style = currentstyle;
                }
            }
            catch (Exception ex) { UtilForms.ReportError(ex); }
        }

        public void ReportError(Exception e, string additionalmessage)
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



    }  //  class TestTcpServer



    static class server_program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        //void TestThreadFunc()
        //{
        //    IGForm.Console.WriteLine("\nInside the Threaded function.\n");
        //    Thread.Sleep(2000);
        //    IGForm.Console.WriteLine(Before exiting the thread);
        //}


        [STAThread]
        static void Main()
        {


            // Multi-client TCP Server (when a client establishes conection with a server, a new
            // Server is launched in a separate thread that listens for new connection requests).


            TestTcpMultiThreadedServer multiclientsrv = new TestTcpMultiThreadedServer();
            multiclientsrv.StartServer();


            while (true)
                Thread.Sleep(100);

            //// Single client server:
            //TestTcpServer srv = new TestTcpServer();
            //srv.StartServer();



            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ServerForm());
        }
    }
}
