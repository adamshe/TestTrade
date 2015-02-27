using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Execution" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("362C7AB6-859F-44e0-AE85-DA5D7D92F7B0")]
    public interface IComExecution
    {
        /// <summary>
        /// Account where the execution is belonging to.
        /// </summary>
        Account Account { get; }
        /// <summary>
        /// Average cost per unit.
        /// </summary>
        double AvgPrice { get; }
        /// <summary>
        /// Identifies the execution. If the brokers native execution id is a numeric value, it will be converted 
        /// to a string accordingly.
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Identifies the associated order.
        /// </summary>
        string OrderId { get; }
        /// <summary>
        /// Identifies the execution side.
        /// </summary>
        iTrading.Core.Kernel.MarketPosition MarketPosition { get; }
        /// <summary>
        /// Number of units which have been executed.
        /// </summary>
        int Quantity { get; }
        /// <summary>
        /// Order has been executed on this symbol.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
        /// <summary>
        /// Execution time.
        /// </summary>
        DateTime Time { get; }
    }
}

