using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Order" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("6C9150C6-F5AF-4a0a-AC9A-1BCA51697E85")]
    public interface IComOrder
    {
        /// <summary>
        /// Identifies the order's action.
        /// </summary>
        ActionType Action { get; }
        /// <summary>
        /// The average fill price of all partial fills.
        /// </summary>
        double AvgFillPrice { get; }
        /// <summary>
        /// The account the order belongs to.
        /// </summary>
        Account Account { get; }
        /// <summary>
        /// Cancels the actual order. Please note: This has no effect, if <see cref="P:iTrading.Core.Kernel.Order.OrderState" /> 
        /// is <see cref="F:iTrading.Core.Kernel.OrderStateId.Initialized" /> or <see cref="F:iTrading.Core.Kernel.OrderStateId.Cancelled" />.
        /// </summary>
        void Cancel();
        /// <summary>
        /// Change order attributes at the broker's system.
        /// </summary>
        void Change();
        /// <summary>
        /// <see cref="T:iTrading.Core.Kernel.Connection" /> where market data is requested from.
        /// </summary>
        iTrading.Core.Kernel.Connection Connection { get; }
        /// <summary>
        /// <see cref="P:iTrading.Core.Kernel.Request.CustomLink" />. This property may be used to attach any object to the request.
        /// </summary>
        object CustomLink { get; set; }
        /// <summary>
        /// Custom text.
        /// </summary>
        string CustomText { get; set; }
        /// <summary>
        /// Manually fill orders in state <see cref="F:iTrading.Core.Kernel.OrderStateId.Unknown" />.
        /// </summary>
        /// <param name="filled">set the fill quantity</param>
        /// <param name="avgFillPrice"> set the avg. fill price</param>
        void Fill(int filled, double avgFillPrice);
        /// <summary>
        /// The number of units filled.
        /// </summary>
        int Filled { get; }
        /// <summary>
        /// Get the history of <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> events.
        /// </summary>
        OrderStatusEventCollection History { get; }
        /// <summary>
        /// Identifies the <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Limit price. Is valid for stop and stop limit orders. Default = 0.
        /// Please note: Setting <see cref="P:iTrading.Core.Kernel.Order.LimitPrice" /> only becomes effective after calling <see cref="M:iTrading.Core.Kernel.Order.Change" />.
        /// </summary>
        double LimitPrice { get; set; }
        /// <summary>
        /// Get/set OCA group. All orders of an account having the same <see cref="P:iTrading.Core.Kernel.Order.OcaGroup" /> property
        /// belong to the same OCA group.
        /// </summary>
        string OcaGroup { get; }
        /// <summary>
        /// Identifies the order.
        /// </summary>
        string OrderId { get; }
        /// <summary>
        /// Number of shares or contract to execute.
        /// Please note: Setting <see cref="P:iTrading.Core.Kernel.Order.Quantity" /> only becomes effective after calling <see cref="M:iTrading.Core.Kernel.Order.Change" />.
        /// </summary>
        int Quantity { get; set; }
        /// <summary>
        /// The actual order state.  
        /// For a list of OrderStates, see <see cref="T:iTrading.Core.Kernel.OrderStateId" />
        /// </summary>
        iTrading.Core.Kernel.OrderState OrderState { get; }
        /// <summary>
        /// Stop price. Is valid for stop market and stop limit orders. default = 0.
        /// Please note: Setting <see cref="P:iTrading.Core.Kernel.Order.StopPrice" /> only becomes effective after calling <see cref="M:iTrading.Core.Kernel.Order.Change" />.
        /// </summary>
        double StopPrice { get; set; }
        /// <summary>
        /// Submits the order to the broker. Please note: This operation has no effect, 
        /// when <see cref="P:iTrading.Core.Kernel.Order.OrderState" /> != <see cref="F:iTrading.Core.Kernel.OrderStateId.Initialized" />.
        /// </summary>
        void Submit();
        /// <summary>
        /// Submitsan order to a simulation account.
        /// Please note: This method may only be called on simulation accounts.
        /// </summary>
        /// <param name="simulationSymbolOptions">Set the options for this order. If NULL, default options will be used.</param>
        void Submit(SimulationSymbolOptions simulationSymbolOptions);
        /// <summary>
        /// Identifies the share/contract to execute.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
        /// <summary>
        /// Order type.
        /// </summary>
        iTrading.Core.Kernel.OrderType OrderType { get; }
        /// <summary>
        /// Time of last change.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// The actual time in force setting.
        /// </summary>
        iTrading.Core.Kernel.TimeInForce TimeInForce { get; }
        /// <summary>
        /// TradeMagic internal permanent order identifier. This indentifier never changes throughout the lifetime
        /// of the order.
        /// </summary>
        string Token { get; }
        /// <summary>
        /// Returns the printable string value of this object.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}

