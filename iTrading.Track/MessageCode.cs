namespace iTrading.Track
{
    using System;

    internal enum MessageCode
    {
        BrokerErrorMessage = 310,
        BrokerMessage = 0x13c,
        BrokerTableMessage = 0x13b,
        CompositeValue = 0x1c,
        ErrorReport = 0,
        FromTrack = 14,
        IntradayUpdate = 6
    }
}

