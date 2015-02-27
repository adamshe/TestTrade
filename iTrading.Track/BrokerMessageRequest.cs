namespace iTrading.Track
{
    using System;

    internal class BrokerMessageRequest : Request
    {
        internal BrokerMessageRequest(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadString(160).Trim();
        }

        internal override ErrorCode Send()
        {
            return ErrorCode.NoError;
        }
    }
}

