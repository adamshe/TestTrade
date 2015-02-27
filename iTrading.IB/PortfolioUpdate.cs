namespace iTrading.IB
{
    using System;

    internal class PortfolioUpdate
    {
        internal string account = "";
        internal double averageCost = 0.0;
        internal Contract contract = new Contract("");
        internal double marketPrice;
        internal double marketValue;
        internal double realizedPNL = 0.0;
        internal int size;
        internal double unrealizedPNL = 0.0;
    }
}

