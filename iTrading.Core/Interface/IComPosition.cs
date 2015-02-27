using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Position" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("015578B7-E9C8-41e1-ABF3-CFA3A2147D7D")]
    public interface IComPosition
    {
        /// <summary>
        /// Account where the position is belonging to.
        /// </summary>
        Account Account { get; }
        /// <summary>
        /// Average cost per unit.
        /// </summary>
        double AvgPrice { get; }
        /// <summary>
        /// Currency.
        /// </summary>
        iTrading.Core.Kernel.Currency Currency { get; }
        /// <summary>
        /// Number of shares/units/contracts.
        /// </summary>
        int Quantity { get; }
        /// <summary>
        /// Symbol.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
        /// <summary>
        /// Type of position item.
        /// </summary>
        iTrading.Core.Kernel.MarketPosition MarketPosition { get; }
    }
}

