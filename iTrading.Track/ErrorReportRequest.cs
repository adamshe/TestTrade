    using iTrading.Core.Kernel;

    namespace iTrading.Track
{
    

    internal class ErrorReportRequest : iTrading.Track.Request
    {
        internal ErrorReportRequest(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core .Kernel.ErrorCode.NativeError, "", "Got error '" + code + "'"));
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            return iTrading.Track.ErrorCode.NoError;
        }
    }
}

