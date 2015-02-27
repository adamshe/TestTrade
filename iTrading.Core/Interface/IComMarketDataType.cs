namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDataType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("ED3CA8C9-7040-43d7-8514-3BE724ADD2FA")]
    public interface IComMarketDataType
    {
        /// <summary>
        /// The TradeMagic id of the MarketDataType. This id is independent from the underlying provider system.
        /// </summary>
        MarketDataTypeId Id { get; }
        /// <summary>
        /// The name of the MarketDataType.
        /// </summary>
        string Name { get; }
    }
}

