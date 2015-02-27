namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("AE75660F-D2DC-43a7-8877-53AF49781567")]
    public interface IComOrderType
    {
        /// <summary>
        /// The TradeMagic id of the OrderType. This id is independent from the underlying broker system.
        /// </summary>
        OrderTypeId Id { get; }
        /// <summary>
        /// The name of the OrderType.
        /// </summary>
        string Name { get; }
    }
}

