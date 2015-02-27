namespace iTrading.IB
{
    using System;

    internal class ComboLeg
    {
        internal string Action = "";
        internal int ConId = 0;
        internal string Exchange = "";
        internal LegOpenClose OpenClose = LegOpenClose.UNKNOWN_POS;
        internal double Ratio = 0.0;
    }
}

