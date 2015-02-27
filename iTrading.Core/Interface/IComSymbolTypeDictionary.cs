namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SymbolTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("564E813E-2AC4-473e-9ACE-63843078CD5F")]
    public interface IComSymbolTypeDictionary
    {
        /// <summary>
        /// Checks if the SymbolType exists in this container.
        /// </summary>
        /// <param name="symbolType"></param>
        /// <returns></returns>
        bool Contains(SymbolType symbolType);
        /// <summary>
        /// Retrieves an SymbolType object by its name.
        /// </summary>
        /// <param name="name"></param>
        SymbolType Find(string name);
        /// <summary>
        /// Retrieves an SymbolType object by its id.
        /// </summary>
        /// <param name="id"></param>
        SymbolType this[SymbolTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.SymbolType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

