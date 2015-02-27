namespace iTrading.Db
{
    using System;
    using iTrading.Core.Kernel;

    internal class SymbolStore
    {
        internal Exchange exchange;
        internal DateTime expiry;
        internal string name;
        internal RightId rightId;
        internal double strikePrice;
        internal SymbolType symbolType;

        internal SymbolStore(string name, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId)
        {
            this.exchange = exchange;
            this.expiry = expiry;
            this.name = name;
            this.rightId = rightId;
            this.strikePrice = strikePrice;
            this.symbolType = symbolType;
        }
    }
}

