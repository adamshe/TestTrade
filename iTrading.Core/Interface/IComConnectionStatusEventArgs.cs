namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ConnectionStatusEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("2FB78DDB-5ADA-46c9-891A-19274E40BFE2")]
    public interface IComConnectionStatusEventArgs
    {
        /// <summary>
        /// Connection status id.
        /// </summary>
        iTrading.Core.Kernel.ConnectionStatusId ConnectionStatusId { get; }
        /// <summary>
        /// The affected connection.
        /// </summary>
        iTrading.Core.Kernel.Connection Connection { get; }
        /// <summary>
        /// Custom text.
        /// </summary>
        string CustomText { get; }
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
        /// Old/previous connection status
        /// </summary>
        iTrading.Core.Kernel.ConnectionStatusId OldConnectionStatusId { get; }
        /// <summary>
        /// Old/previous connection status or the secondary server(s), e.g. price feed server.
        /// </summary>
        iTrading.Core.Kernel.ConnectionStatusId OldSecondaryConnectionStatusId { get; }
        /// <summary>
        /// Request causing the Args.
        /// </summary>
        iTrading.Core.Kernel.Request Request { get; }
        /// <summary>
        /// Connection status of the secondars server(s), e.g. price feed server.
        /// </summary>
        iTrading.Core.Kernel.ConnectionStatusId SecondaryConnectionStatusId { get; }
    }
}

