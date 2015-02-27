namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Currency" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("E121E79B-1F15-4197-A2DB-70BF36FA3AF5")]
    public interface IComCurrency
    {
        /// <summary>
        /// The TradeMagic id of the currency. This id is independent from the underlying provider system.
        /// </summary>
        CurrencyId Id { get; }
        /// <summary>
        /// The provider dependent id of the currency. 
        /// </summary>
        string MapId { get; }
        /// <summary>
        /// The name of the currency.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The currency symbol.
        /// </summary>
        string Sign { get; }
    }
}

