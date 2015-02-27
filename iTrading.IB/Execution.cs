namespace iTrading.IB
{
    using System;

    internal class Execution
    {
        internal string acctNumber;
        internal int connectionId;
        internal Contract contract = new Contract("");
        internal string exchange;
        internal string execId;
        internal int orderId;
        internal string permId = "";
        internal double price;
        internal int shares;
        internal string side;
        internal string timeString;
    }
}

