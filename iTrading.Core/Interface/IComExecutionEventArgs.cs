using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ExecutionUpdateEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("5004FB0E-B1D7-4ff6-B7BF-BE6909B5E6C7")]
    public interface IComExecutionEventArgs
    {
        /// <summary>
        /// The account the execution belongs to.
        /// </summary>
        Account Account { get; }
        /// <summary>
        /// Average cost per unit.
        /// </summary>
        double AvgPrice { get; }
        /// <summary>
        /// Execution id.
        /// </summary>
        string ExecutionId { get; }
        /// <summary>
        /// Returns the TradeMagic error code of the Args. 
        /// If the asyncronous call has been successful, the value will be <seealso cref="F:iTrading.Core.Kernel.ErrorCode.NoError" />.
        /// </summary>
        ErrorCode Error { get; }
        /// <summary>
        /// Type of position item.
        /// </summary>
        iTrading.Core.Kernel.MarketPosition MarketPosition { get; }
        /// <summary>
        /// Native error code of underlying broker or data provider.
        /// </summary>
        string NativeError { get; }
        /// <summary>
        /// The update operation.
        /// </summary>
        iTrading.Core.Kernel.Operation Operation { get; }
        /// <summary>
        /// Id of order causing the execution.
        /// </summary>
        string OrderId { get; }
        /// <summary>
        /// Number of shares/units/contracts.
        /// </summary>
        int Quantity { get; }
        /// <summary>
        /// Request causing the Args.
        /// </summary>
        iTrading.Core.Kernel.Request Request { get; }
        /// <summary>
        /// Execution time.
        /// </summary>
        DateTime Time { get; }
    }
}

