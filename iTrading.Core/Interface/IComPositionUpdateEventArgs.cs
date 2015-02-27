using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.PositionUpdateEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("1AC42A21-EF23-41f9-B760-5C989647CE10")]
    public interface IComPositionUpdateEventArgs
    {
        /// <summary>
        /// The account the positions belongs to.
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
        /// The update operation.
        /// </summary>
        iTrading.Core.Kernel.Operation Operation { get; }
        /// <summary>
        /// The inserted/updated/deleted position.
        /// </summary>
        iTrading.Core.Kernel.Position Position { get; }
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
    }
}

