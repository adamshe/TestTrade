namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Exchange" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("79CCD34C-5F55-418f-B5B8-F4EAA04E267D")]
    public interface IComExchange
    {
        /// <summary>
        /// The TradeMagic id of the exchange. This id is independent from the underlying provider system.
        /// </summary>
        ExchangeId Id { get; }
        /// <summary>
        /// The name of the exchange.
        /// </summary>
        string Name { get; }
    }
}

