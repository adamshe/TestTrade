namespace iTrading.Track
{
    using System;

    internal enum SourceType
    {
        AllOrders = 0x29,
        AllPositions = 0x26,
        ClosedOrders = 0x23,
        ClosedPositions = 0x25,
        OpenOrders = 0x20,
        OpenPositions = 0x22,
        OtherOrders = 0x24,
        OtherTransactions = 30,
        SecurityTransactions = 0x21
    }
}

