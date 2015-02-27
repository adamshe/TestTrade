namespace iTrading.Track
{
    using System;
    using iTrading.Core.Kernel;

    internal class BrokerErrorMessageRequest : iTrading.Track.Request
    {
        internal BrokerErrorMessageRequest(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            reader.ReadByte();
            string message = reader.ReadString(80).Trim();
            base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NativeError, "", message));
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            return iTrading.Track.ErrorCode.NoError;
        }
    }
}

