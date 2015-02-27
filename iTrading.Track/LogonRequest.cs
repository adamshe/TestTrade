namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using iTrading.Core.Kernel;

    internal class LogonRequest : iTrading.Track.Request
    {
        internal LogonRequest(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.LogonRequest.Process1");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            if (code == iTrading.Track.ErrorCode.NoError)
            {
                reader.Skip(5);
                base.Adapter.workingHost = reader.ReadString(8);
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.LogonRequest.Process2: workingHost='" + base.Adapter.workingHost + "'");
                }
                if (base.Adapter.connection.Currencies.Count == 0)
                {
                    base.Adapter.Init();
                }
                if (base.Adapter.Options.Mode.Id == ModeTypeId.Live)
                {
                    code = (iTrading.Track.ErrorCode) Api.BrokerUseRealAcct();
                    if (code != iTrading.Track.ErrorCode.NoError)
                    {
                        base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Track.LogonRequest.Send.mtBrokerUseRealAcct: error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                }
                else
                {
                    code = (iTrading.Track.ErrorCode) Api.BrokerUseContest();
                    if (code != iTrading.Track.ErrorCode.NoError)
                    {
                        base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Track.LogonRequest.Send.mtBrokerUseContest: error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                }
                lock (base.Adapter.startupRequests)
                {
                    base.Adapter.startupRequests.Add(new BrokerRequestAccountIds(base.Adapter));
                }
                base.Adapter.Init2();
            }
            else
            {
                Api.Disconnect();
                Api.DeInit();
                base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Logonrequest returned error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                base.Adapter.Cleanup();
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.LogonRequest.Send");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.Init();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Track.LogonRequest.Send.mtInit: error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return code;
            }
            code = (iTrading.Track.ErrorCode) Api.SetHost(base.Adapter.Options.Host, base.Adapter.Options.Port);
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Track.LogonRequest.Send.mtSetHost: error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return code;
            }
            code = (iTrading.Track.ErrorCode) Api.ConnectToServer();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Track.LogonRequest.Send.mtConnectToServer: error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return code;
            }
            lock (base.Adapter.syncThreadStart)
            {
                base.Adapter.receiveThread = new Thread(new ThreadStart(base.Adapter.ReceiveThreadLoop));
                base.Adapter.receiveThread.Name = "TM TrackReceive";
                base.Adapter.receiveThread.Start();
                Monitor.Wait(base.Adapter.syncThreadStart);
            }
            code = (iTrading.Track.ErrorCode) Api.RequestLogon(base.Rqn, base.Adapter.Options.User, base.Adapter.Options.Password, "TRADEMAGIC", 1, 1);
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.receiveThread.Abort();
                base.Adapter.receiveThread = null;
                base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Track.LogonRequest.Send.mtRequestLogon: error " + code + "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return code;
            }
            return iTrading.Track.ErrorCode.NoError;
        }
    }
}

