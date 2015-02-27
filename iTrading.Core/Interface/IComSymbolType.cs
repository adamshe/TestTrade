namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SymbolType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("BAADBA85-7431-47cd-8699-0C580387A696")]
    public interface IComSymbolType
    {
        /// <summary>
        /// The TradeMagic id of the SymbolType. This id is independent from the underlying provider system.
        /// </summary>
        SymbolTypeId Id { get; }
        /// <summary>
        /// The name of the SymbolType.
        /// </summary>
        string Name { get; }
    }
}

