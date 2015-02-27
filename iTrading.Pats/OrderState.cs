namespace TradeMagic.Pats
{
    using System;

    internal enum OrderState
    {
        AmendPending = 10,
        BalCancelled = 6,
        CancelHeldOrder = 14,
        Cancelled = 5,
        CancelPending = 9,
        Filled = 8,
        HeldOrder = 13,
        PartFilled = 7,
        Queued = 1,
        Rejected = 4,
        Sent = 2,
        UnconfirmedFilled = 11,
        UnconfirmedPartFilled = 12,
        Working = 3
    }
}

