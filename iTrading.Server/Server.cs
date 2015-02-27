namespace TradeMagic.Server
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    internal class Server
    {
        internal ManualResetEvent allDone = new ManualResetEvent(false);
        internal ArrayList clients = new ArrayList();
        internal TradeMagic.Server.ConnectionPool connectionPool;
        private Socket listenerSocket;
        private Thread listenerThread;
        internal MainForm mainForm;
        internal int maxConnections = 100;
        internal int port = new OptionsBase().TMDefaultPort;
        internal SynchronizeInvoke synchronizeInvoke;

        internal Server()
        {
            this.connectionPool = new TradeMagic.Server.ConnectionPool(this);
            this.mainForm = new MainForm(this);
            this.synchronizeInvoke = new SynchronizeInvoke();
            if (new Connection().GetLicense("Server").Id != LicenseTypeId.Professional)
            {
                Trace.WriteLine("There is no valid TradeMagic professional license. TradeMagic server will not start without prior registration.");
                Application.Exit();
            }
            else
            {
                Globals.AppId = "TradeMagic Server";
                Trace.WriteLine("Starting listener thread ...");
                ThreadStart start = new ThreadStart(this.Start);
                this.listenerThread = new Thread(start);
                this.listenerThread.Name = "Listener";
                this.listenerThread.Start();
                Thread.CurrentThread.Name = "Main";
                Application.Run();
            }
        }

        internal void Error(ErrorCode errorCode, string nativeError, string message)
        {
            this.mainForm.Error(errorCode, nativeError, message);
        }

        internal void Exit()
        {
            Trace.WriteLine("Exiting server ...");
            this.listenerSocket.Close();
            this.listenerThread.Abort();
            foreach (Client client in (ArrayList) this.clients.Clone())
            {
                Trace.WriteLine("Close client " + client.id);
                client.Close();
            }
            Trace.WriteLine("Done");
            Application.Exit();
        }

        [STAThread]
        private static void Main()
        {
            new TradeMagic.Server.Server();
        }

        internal void Start()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, this.port);
            this.listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                this.listenerSocket.Bind(localEP);
                this.listenerSocket.Listen(this.maxConnections);
                while (true)
                {
                    this.allDone.Reset();
                    Client client = new Client(this, this.listenerSocket.Accept());
                    lock (this.clients)
                    {
                        this.clients.Add(client);
                    }
                    this.allDone.WaitOne();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (SocketException)
            {
            }
            catch (Exception exception)
            {
                throw new TMException(ErrorCode.Panic, "Server.Start " + exception.Message);
            }
        }
    }
}

