namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class LogoffRequest : iTrading.Track.Request
    {
        private bool reconnect;

        internal LogoffRequest(Adapter adapter, bool reconnect) : base(adapter)
        {
            this.reconnect = reconnect;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.LogoffRequest.Process");
            }
            reader.ReadByte();
            Api.Disconnect();
            Api.DeInit();
            base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
            base.Adapter.Cleanup();
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.LogoffRequest.Send");
            }
            if (base.Adapter.checkStatusTimer != null)
            {
                base.Adapter.checkStatusTimer.Stop();
            }
            base.Adapter.checkStatusTimer = null;
            if (base.Adapter.timerRequest != null)
            {
                base.Adapter.timerRequest.Halt();
            }
            base.Adapter.timerRequest = null;
            if (base.Adapter.newsRequest != null)
            {
                base.Adapter.newsRequest.Halt();
            }
            base.Adapter.newsRequest = null;
            Api.RequestLogoff(base.Rqn);
            Api.Disconnect();
            Api.DeInit();
            if (this.reconnect)
            {
                base.Adapter.Cleanup();
                new LogonRequest(base.Adapter).Send();
            }
            else
            {
                base.Adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                base.Adapter.Cleanup();
            }
            return iTrading.Track.ErrorCode.NoError;
        }
    }
}

