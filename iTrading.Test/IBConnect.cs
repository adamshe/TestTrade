namespace iTrading.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Threading;
    using iTrading.Core.Kernel;
    using iTrading.IB;

    /// <summary>
    /// IB connection test.
    /// </summary>
    [TestFixture]
    public class IBConnect : TestBase
    {
        private int clientId = 0;
        private Connection connection = null;
        private Connection connectionManual = null;
        internal int done = 0;
        private bool expectConnect = true;
        private ErrorCode expectedErrorCode = ErrorCode.NoError;
        private IBOptions ibOptions = null;
        internal int maxClients = 10;
        internal int port = new IBOptions().DefaultPort;
        internal bool runMultiple = false;
        private string savedPassword;
        private string savedUser;
        private int state = 0;
        private Hashtable threads = new Hashtable();

        private void Connection_ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            if (this.expectConnect)
            {
                 iTrading.Test.Globals.Assert("000", e.Connection.ConnectionStatusId == ConnectionStatusId.Connected);
            }
            else
            {
                 iTrading.Test.Globals.Assert("001", e.Connection.ConnectionStatusId != ConnectionStatusId.Connected);
            }
             iTrading.Test.Globals.Assert("002  " + this.state, e.Error == this.expectedErrorCode);
             iTrading.Test.Globals.Assert("003  " + this.state, (e.Error != ErrorCode.NoError) ? (e.NativeError.Length != 0) : true);
            this.ProcessState();
        }

        private void Connection_Error(object sender, ITradingErrorEventArgs e)
        {
            if (this.expectedErrorCode != e.Error)
            {
                 iTrading.Test.Globals.Assert(e.Message, false);
            }
        }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new IBConnect();
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        protected override void DoTest()
        {
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Single" /> in a multi connection environment.
        /// </summary>
        [Test]
        public override void Multiple()
        {
            if ((base.Broker.Id == ProviderTypeId.InteractiveBrokers) && (base.Environment !=  iTrading.Test.Environment.Server))
            {
                this.done = 0;
                this.port = new IBOptions().DefaultPort;
                for (int i = 0; i < base.MaxConnections; i++)
                {
                    ThreadStart start = new ThreadStart(this.MultipleThreadStart);
                    Thread thread = new Thread(start);
                    thread.Name = "ConnectMultipleTws " + i;
                    thread.Start();
                }
                while (this.done < base.MaxConnections)
                {
                     iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                }
            }
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Multiple" /> in an infinite loop. 
        /// Please note: This method will not terminate, unless an error occurs.
        /// </summary>
        [Test]
        public override void MultipleInfinite()
        {
            if ((base.Broker.Id == ProviderTypeId.InteractiveBrokers) && (base.Environment !=  iTrading.Test.Environment.Server))
            {
                while (true)
                {
                    this.Multiple();
                }
            }
        }

        private void MultipleThreadStart()
        {
            IBConnect connect = new IBConnect();
            lock (this)
            {
                connect.port = this.port;
                this.port++;
            }
            connect.runMultiple = true;
            connect.Single();
            this.done++;
        }

        private void ProcessState()
        {
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Connection.IdPlus, ") IBConnect.ProcessState ", this.state }));
            }
            FindTws tws = new FindTws();
            switch (this.state)
            {
                case 0x70:
                    this.state = 0x71;
                    this.ibOptions.ClientId = 9;
                    this.ibOptions.Connect2RunningTws = false;
                    this.ibOptions.UseSsl = false;
                    this.expectConnect = true;
                    this.expectedErrorCode = ErrorCode.NoError;
                    this.connection.Connect(this.ibOptions);
                    return;

                case 0x71:
                    this.state = 0x72;
                    this.expectConnect = false;
                    this.ibOptions.ClientId = 0;
                    this.ibOptions.UseSsl = true;
                     iTrading.Test.Globals.Sleep(this.SleepMilliSeconds);
                    this.connection.Close();
                    return;

                case 0x72:
                    this.state = 0x73;
                    return;

                case 0x69:
                    if (this.clientId < 5)
                    {
                        this.state = 100;
                        this.expectConnect = false;
                        this.connection.Close();
                        return;
                    }
                    this.state = 0x70;
                    this.connectionManual.ConnectionStatus -= new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
                    this.expectConnect = false;
                    this.expectedErrorCode = ErrorCode.ServerConnectionIsBroken;
                    this.connectionManual.Close();
                    return;

                case 90:
                    this.state = 0x5b;
                    this.expectConnect = true;
                    this.ibOptions.Connect2RunningTws = false;
                    this.connection.Connect(this.ibOptions);
                    return;

                case 0x5b:
                    this.state = 0x5c;
                    this.expectConnect = false;
                     iTrading.Test.Globals.Sleep(this.SleepMilliSeconds);
                    this.connection.Close();
                    return;

                case 0x5c:
                    this.state = 0x5d;
                    this.expectConnect = false;
                    this.expectedErrorCode = ErrorCode.LoginFailed;
                    this.ibOptions.Password = "blablabla";
                    this.connection.Connect(this.ibOptions);
                    return;

                case 0x5d:
                    this.state = 0x5e;
                    if (!this.runMultiple)
                    {
                         iTrading.Test.Globals.Assert("100", !tws.Run());
                    }
                    this.expectConnect = false;
                    this.ibOptions.Password = this.savedPassword;
                    this.ibOptions.User = "blablabla";
                    this.connection.Close();
                    this.connection.Connect(this.ibOptions);
                    return;

                case 0x5e:
                    this.state = 100;
                    if (!this.runMultiple)
                    {
                         iTrading.Test.Globals.Assert("101", !tws.Run());
                    }
                    this.expectConnect = true;
                    this.expectedErrorCode = ErrorCode.NoError;
                    this.ibOptions.ClientId = this.clientId++;
                    this.ibOptions.User = this.savedUser;
                    this.connectionManual.Connect(this.ibOptions);
                    return;

                case 100:
                    this.state = 0x69;
                    this.ibOptions.Connect2RunningTws = true;
                    this.expectConnect = true;
                    this.ibOptions.ClientId = this.clientId++;
                    this.connection.Connect(this.ibOptions);
                    return;

                case 0:
                    return;
            }
             iTrading.Test.Globals.Assert("unexpected test driver state " + this.state, false);
        }

        /// <summary>
        /// Test running on one TWS instance. First only one connection is established, next mutiple connections
        /// having different client ids are tested against the same TWS instance.
        /// </summary>
        [Test]
        public override void Single()
        {
            if ((base.Broker.Id == ProviderTypeId.InteractiveBrokers) && (base.Environment !=  iTrading.Test.Environment.Server))
            {
                foreach (ModeType type in ModeType.All.Values)
                {
                    if (type.Id == ModeTypeId.Test)
                    {
                        continue;
                    }
                    if (type.Id == ModeTypeId.Live)
                    {
                        if (!this.runMultiple)
                        {
                            this.ibOptions = (IBOptions) OptionsBase.Restore(ProviderType.All[ProviderTypeId.InteractiveBrokers], type);
                            if (this.ibOptions != null)
                            {
                                goto Label_008E;
                            }
                        }
                        continue;
                    }
                    this.ibOptions = new IBOptions();
                Label_008E:
                    this.clientId = 0;
                    this.connection = new Connection();
                    this.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
                    this.connection.Error += new ErrorArgsEventHandler(this.Connection_Error);
                    this.connectionManual = new Connection();
                    this.connectionManual.ConnectionStatus += new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
                    this.connectionManual.Error += new ErrorArgsEventHandler(this.Connection_Error);
                    this.done = 0;
                    this.expectConnect = true;
                    this.ibOptions.Port = this.port;
                    this.savedPassword = this.ibOptions.Password;
                    this.savedUser = this.ibOptions.User;
                    this.state = 90;
                    this.ProcessState();
                    while (this.state < 0x73)
                    {
                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                    }
                    this.connection.ConnectionStatus -= new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
                    this.connection.Error -= new ErrorArgsEventHandler(this.Connection_Error);
                    this.connectionManual.ConnectionStatus -= new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
                    this.connectionManual.Error -= new ErrorArgsEventHandler(this.Connection_Error);
                    this.done = 0;
                    this.connection = new Connection();
                    this.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
                    this.connection.Error += new ErrorArgsEventHandler(this.Connection_Error);
                    this.expectConnect = true;
                    this.ibOptions.Connect2RunningTws = false;
                    this.ibOptions.ClientId = 0x270f;
                    this.ibOptions.Port = this.port;
                    this.state = 0;
                    this.connection.Connect(this.ibOptions);
                    while (this.connection.ConnectionStatusId != ConnectionStatusId.Connected)
                    {
                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                    }
                    for (int i = 0; i < this.maxClients; i++)
                    {
                        ThreadState state = new ThreadState(this, this.port, i);
                        Thread thread = new Thread(new ThreadStart(state.ProcessState));
                        thread.Name = "ConnectMultiple " + i;
                        thread.Start();
                    }
                    while (this.done < this.maxClients)
                    {
                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                    }
                    this.expectConnect = false;
                    this.connection.Close();
                }
            }
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Single" /> in an infinite loop. 
        /// Please note: This method will not terminate, unless an error occurs.
        /// </summary>
        [Test]
        public override void SingleInfinite()
        {
            if ((base.Broker.Id == ProviderTypeId.InteractiveBrokers) && (base.Environment !=  iTrading.Test.Environment.Server))
            {
                while (true)
                {
                    this.Single();
                }
            }
        }

        internal int SleepMilliSeconds
        {
            get
            {
                if (!this.runMultiple)
                {
                    return 0;
                }
                return 0xea60;
            }
        }

        private class ThreadState
        {
            public bool abort = false;
            public Connection connection = new Connection();
            public int count = 0;
            public bool expectConnect = true;
            public ErrorCode expectedErrorCode = ErrorCode.NoError;
            public IBConnect ibConnect;
            public IBOptions ibOptions = new IBOptions();
            public int maxRepeats = 5;
            public int port = 0;
            public int state = 0;

            internal ThreadState(IBConnect ibConnect, int port, int clientId)
            {
                this.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.ConnectionStatus);
                this.connection.Error += new ErrorArgsEventHandler(this.Connection_Error);
                this.ibConnect = ibConnect;
                this.ibOptions.ClientId = clientId;
                this.ibOptions.Connect2RunningTws = true;
                this.ibOptions.Port = port;
            }

            private void Connection_Error(object sender, ITradingErrorEventArgs e)
            {
                if (this.expectedErrorCode != e.Error)
                {
                     iTrading.Test.Globals.Assert(e.Message, false);
                }
            }

            private void ConnectionStatus(object sender, ConnectionStatusEventArgs e)
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Test.IBConnect.ThreadState.ConnectionStatus: ", this.ibOptions.ClientId, "/", ((IBOptions) e.Connection.Options).ClientId, " state=", this.state }));
                }
                if (this.expectConnect)
                {
                     iTrading.Test.Globals.Assert("t000", e.Connection.ConnectionStatusId == ConnectionStatusId.Connected);
                }
                else
                {
                     iTrading.Test.Globals.Assert("t001", e.Connection.ConnectionStatusId != ConnectionStatusId.Connected);
                }
                 iTrading.Test.Globals.Assert("t002  " + this.state, e.Error == this.expectedErrorCode);
                 iTrading.Test.Globals.Assert("t003  " + this.state, (e.Error != ErrorCode.NoError) ? (e.NativeError.Length != 0) : true);
                this.ProcessState();
            }

            internal void ProcessState()
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Test.IBConnect.ThreadState.ProcessState: ", this.ibOptions.ClientId, " state=", this.state }));
                }
                switch (this.state)
                {
                    case 0:
                        this.state = 1;
                        this.expectConnect = true;
                        this.connection.Connect(this.ibOptions);
                        return;

                    case 1:
                        this.state = 2;
                        this.expectConnect = false;
                         iTrading.Test.Globals.Sleep(this.ibConnect.SleepMilliSeconds);
                        this.connection.Close();
                        return;

                    case 2:
                        this.state = 3;
                        this.expectConnect = false;
                        this.expectedErrorCode = ErrorCode.Panic;
                        this.port = this.ibOptions.Port;
                        this.ibOptions.Port = 0x22b8;
                        this.ibOptions.User = new IBOptions().DemoUser;
                        this.connection.Connect(this.ibOptions);
                        return;

                    case 3:
                        this.expectedErrorCode = ErrorCode.NoError;
                        this.ibOptions.Port = this.port;
                        if (++this.count >= this.maxRepeats)
                        {
                            this.ibConnect.done++;
                            return;
                        }
                        this.state = 0;
                        this.ProcessState();
                        return;
                }
                 iTrading.Test.Globals.Assert(base.GetType().FullName + ": unexpected test driver state " + this.state, false);
            }
        }
    }
}

