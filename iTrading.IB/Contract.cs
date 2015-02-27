using iTrading.Core.Kernel;

namespace iTrading.IB
{
    using System;
    using System.Collections;

    internal class Contract
    {
        internal ArrayList ComboLegs = new ArrayList();
        internal string Currency = "";
        internal string Exchange = "";
        internal DateTime Expiry = Globals.MaxDate;
        internal string LocalSymbol = "";
        internal iTrading.IB.Right Right = iTrading.IB.Right.ANY;
        internal string SecType = "";
        internal double Strike = 0.0;
        internal string Symbol = "";

        internal Contract(string strSymbol)
        {
            this.Symbol = strSymbol;
        }
    }
}

