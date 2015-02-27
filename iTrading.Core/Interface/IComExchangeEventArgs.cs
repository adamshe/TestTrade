namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ExchangeEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("555A4913-12AF-4fac-BC94-374EFE913D25")]
    public interface IComExchangeEventArgs
    {
        /// <summary>
        /// Exchange.
        /// </summary>
        iTrading.Core.Kernel.Exchange Exchange { get; }
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

