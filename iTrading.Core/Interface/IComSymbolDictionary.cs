namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SymbolDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("DA86621A-8140-4859-949D-88778CB128B2")]
    public interface IComSymbolDictionary
    {
        /// <summary>
        /// Checks if the symbol exists in this container.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        bool Contains(Symbol symbol);
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.Symbol" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

