namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketPositionEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("28314900-FB4C-4688-8E0E-C66DA513487D")]
    public interface IComMarketPositionEventArgs
    {
        /// <summary>
        /// MarketPosition.
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

