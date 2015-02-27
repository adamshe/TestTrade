namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.CurrencyDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("A63A68D4-A222-4ddf-8EF4-D63BCD324EE6")]
    public interface IComCurrencyDictionary
    {
        /// <summary>
        /// Checks if the Currency exists in this container.
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        bool Contains(iTrading.Core.Kernel.Currency currency);
        /// <summary>
        /// Retrieves an Currency object by its name.
        /// </summary>
        /// <param name="name"></param>
        iTrading.Core.Kernel.Currency Find(string name);
        /// <summary>
        /// Retrieves an Currency object by its id.
        /// </summary>
        /// <param name="id"></param>
        iTrading.Core.Kernel.Currency this[CurrencyId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.Currency" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

