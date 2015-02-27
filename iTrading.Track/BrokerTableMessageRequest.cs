namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BrokerTableMessageRequest : iTrading.Track.Request
    {
        internal BrokerTableMessageRequest(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerTableMessageRequest.Process");
            }
            char ch = (char) reader.ReadByte();
            reader.Skip(3);
            SourceType type = (SourceType) reader.ReadInteger();
            int countFields = reader.ReadInteger();
            reader.ReadInteger();
            StringCollection fields = reader.ReadBrokerTableFields(countFields);
            switch (type)
            {
                case SourceType.OpenOrders:
                    BrokerRequestOrder.ProcessNow(base.Adapter, fields, null);
                    return;

                case SourceType.SecurityTransactions:
                    Trace.Assert(ch == 'A', "Track.BrokerTableMessageRequest.Process: type='" + ch + "'");
                    BrokerRequestTransactionData.ProcessNow(base.Adapter, fields);
                    return;

                case SourceType.OpenPositions:
                    BrokerRequestAcctPositions.ProcessNow(base.Adapter, ch, fields);
                    break;
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            return iTrading.Track.ErrorCode.NoError;
        }
    }
}

