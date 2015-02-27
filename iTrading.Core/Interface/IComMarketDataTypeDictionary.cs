namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDataTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("EB3829DE-6123-435c-84B0-FBDEAC3CCE79")]
    public interface IComMarketDataTypeDictionary
    {
        /// <summary>
        /// Checks if the MarketDataType exists in this container.
        /// </summary>
        /// <param name="marketDataType"></param>
        /// <returns></returns>
        bool Contains(MarketDataType marketDataType);
        /// <summary>
        /// Retrieves an MarketDataType object by its name.
        /// </summary>
        /// <param name="name"></param>
        MarketDataType Find(string name);
        /// <summary>
        /// Retrieves an MarketDataType object by its id.
        /// </summary>
        /// <param name="id"></param>
        MarketDataType this[MarketDataTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.MarketDataType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

