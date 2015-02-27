namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SymbolCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("AE794FE1-5AFE-4094-A52B-6CD2C4329B0B")]
    public interface IComSymbolCollection
    {
        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="newSymbol"></param>
        void Add(Symbol newSymbol);
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(Symbol value);
        /// <summary>
        /// Get the n-th Symbol of the container.
        /// </summary>
        Symbol this[int index] { get; }
        /// <summary>
        /// Remove an item.
        /// </summary>
        /// <param name="symbol"></param>
        void Remove(Symbol symbol);
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.SymbolCollection" /> instances.
        /// </summary>
        int Count { get; }
    }
}

