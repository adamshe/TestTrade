namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("65D58D27-134C-44c3-8810-C763E83ADDE8")]
    public interface IComOrderStatusEventArgs
    {
        /// <summary>
        /// The average fill price of all partial fills.
        /// </summary>
        double AvgFillPrice { get; }
        /// <summary>
        /// The number of units filled.
        /// </summary>
        int Filled { get; }
        /// <summary>
        /// New limit price. The limit price may change on e.g. unsolicited order changes.
        /// These changes may be caused by e.g. the broker helpdesk.
        /// </summary>
        double LimitPrice { get; }
        /// <summary>
        /// Identifies the updated order.
        /// </summary>
        iTrading.Core.Kernel.Order Order { get; }
        /// <summary>
        /// The (new) id of the updated order.
        /// </summary>
        string OrderId { get; }
        /// <summary>
        /// The actual order state.
        /// </summary>
        iTrading.Core.Kernel.OrderState OrderState { get; }
        /// <summary>
        /// Returns the TradeMagic error code of the Args. 
        /// If the asyncronous call has been successful, the value will be <seealso cref="F:iTrading.Core.Kernel.ErrorCode.NoError" />.
        /// </summary>
        ErrorCode Error { get; }
        /// <summary>
        /// Native error code of underlying broker or data provider.
        /// </summary>
        string NativeError { get; }
        /// <summary>
        /// Request causing the Args.
        /// </summary>
        iTrading.Core.Kernel.Request Request { get; }
        /// <summary>
        /// New quantity. The quantity may change on e.g. unsolicited order changes.
        /// These changes may be caused by e.g. the broker helpdesk.
        /// </summary>
        int Quantity { get; }
        /// <summary>
        /// New stop price. The stop price may change on e.g. unsolicited order changes.
        /// These changes may be caused by e.g. the broker helpdesk.
        /// </summary>
        double StopPrice { get; }
        /// <summary>
        /// Order update time.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Returns the printable string value of this object.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}

