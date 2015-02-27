namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class TimerRequest : iTrading.Track.Request
    {
        internal TimerRequest(Adapter adapter) : base(adapter)
        {
        }

        internal iTrading.Track.ErrorCode Halt()
        {
            if (Globals.TraceSwitch.Timer)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.TimerRequest.Halt");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestIntradayUpdate(base.Rqn, "TIME", "", '\x0002');
            if ((code != iTrading.Track.ErrorCode.NoError) && (code != iTrading.Track.ErrorCode.NotConnected))
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core .Kernel.ErrorCode.Panic, "", "Track.TimerRequest.Halt: error " + code));
            }
            return code;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Timer)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.TimerRequest.Process");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.TimerRequest.Process: error " + code));
            }
            else
            {
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                base.Adapter.connection.ProcessEventArgs(new TimerEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", reader.ReadCharTime(), true));
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.Timer)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.TimerRequest.Send");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestIntradayUpdate(base.Rqn, "TIME", "", '\x0001');
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.TimerRequest.Send: error " + code));
            }
            return code;
        }
    }
}

