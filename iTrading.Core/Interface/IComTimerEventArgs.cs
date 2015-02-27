namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.TimerEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("B08D14C0-0F03-4275-A1EF-051E0EF24C55")]
    public interface IComTimerEventArgs
    {
        /// <summary>
        /// The affected connection.
        /// </summary>
        iTrading.Core.Kernel.Connection Connection { get; }
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
        /// <summary>
        /// Current time.
        /// </summary>
        DateTime Time { get; }
    }
}

