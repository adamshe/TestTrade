namespace iTrading.IB
{
    using System;
    using iTrading.Core.Kernel;

    internal class AccountUpdate
    {
        internal string account = "";
        internal Currency currency = null;
        internal double doubleValue = 0.0;
        internal AccountItemType itemType;
        internal string stringCurrency;
        internal string stringItem;
        internal string stringValue;
    }
}

