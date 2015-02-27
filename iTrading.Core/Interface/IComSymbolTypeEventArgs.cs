namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SymbolTypeEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("8497CE49-DA29-45c2-B480-F260E93D3AB0")]
    public interface IComSymbolTypeEventArgs
    {
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
        /// SymbolType.
        /// </summary>
        iTrading.Core.Kernel.SymbolType SymbolType { get; }
    }
}

