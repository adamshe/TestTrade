using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.AccountUpdateEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("4CF39BF3-9EE7-4f02-9CE6-6E851DDFDE6F")]
    public interface IComAccountUpdateEventArgs
    {
        /// <summary>
        /// Account being updated. <seealso cref="E:iTrading.Core.Kernel.Account.AccountUpdate" />
        /// </summary>
        Account Account { get; }
        /// <summary>
        /// Identifies the attribute being updated.
        /// </summary>
        AccountItemType ItemType { get; }
        /// <summary>
        /// Currency of the new value.
        /// </summary>
        iTrading.Core.Kernel.Currency Currency { get; }
        /// <summary>
        /// The new value of the attribute.
        /// </summary>
        double Value { get; }
        /// <summary>
        /// The new time value.
        /// </summary>
        DateTime Time { get; }
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

