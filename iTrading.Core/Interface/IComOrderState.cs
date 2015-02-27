namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderState" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("BCB2D907-9823-4913-A144-30D9BD6F3CA3")]
    public interface IComOrderState
    {
        /// <summary>
        /// The TradeMagic id of the OrderState. This id is independent from the underlying broker system.
        /// </summary>
        OrderStateId Id { get; }
        /// <summary>
        /// The name of the OrderState.
        /// </summary>
        string Name { get; }
    }
}

