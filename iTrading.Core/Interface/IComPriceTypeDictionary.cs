namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Defines the <see cref="T:TradeMagic.Data.PriceTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("19A73DA5-0635-4c81-B8CC-A64C20F427B9")]
    public interface IComPriceTypeDictionary
    {
        /// <summary>
        /// Checks if the PriceType exists in this container.
        /// </summary>
        /// <param name="priceType"></param>
        /// <returns></returns>
        bool Contains(PriceType priceType);
        /// <summary>
        /// Retrieves an PriceType object by its name.
        /// </summary>
        /// <param name="name"></param>
        PriceType Find(string name);
        /// <summary>
        /// Retrieves an PriceType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PriceType this[PriceTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:TradeMagic.Data.PriceType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

