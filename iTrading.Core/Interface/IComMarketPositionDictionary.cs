namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketPositionDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("49C348B2-77EB-4d2c-9AF3-54F7F231BB76")]
    public interface IComMarketPositionDictionary
    {
        /// <summary>
        /// Checks if the MarketPosition exists in this container.
        /// </summary>
        /// <param name="marketPosition"></param>
        /// <returns></returns>
        bool Contains(MarketPosition marketPosition);
        /// <summary>
        /// Retrieves an MarketPosition object by its name.
        /// </summary>
        /// <param name="name"></param>
        MarketPosition Find(string name);
        /// <summary>
        /// Retrieves an MarketPosition object by its id.
        /// </summary>
        /// <param name="id"></param>
        MarketPosition this[MarketPositionId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.MarketPosition" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

