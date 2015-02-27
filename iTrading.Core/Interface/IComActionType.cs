namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ActionType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("1AC591AC-D47F-4673-8009-B62DB17D3EEE")]
    public interface IComActionType
    {
        /// <summary>
        /// The TradeMagic id of the ActionType. This id is independent from the underlying provider system.
        /// </summary>
        ActionTypeId Id { get; }
        /// <summary>
        /// The name of the ActionType.
        /// </summary>
        string Name { get; }
    }
}

