namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Order state.
    /// </summary>
    public enum OrderStateId
    {
        Accepted,
        Cancelled,
        Filled,
        Initialized,
        PartFilled,
        PendingCancel,
        PendingChange,
        PendingSubmit,
        Rejected,
        Unknown,
        Working
    }
}

