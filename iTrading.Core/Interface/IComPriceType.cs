namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Data;

    /// <summary>
    /// Defines the <see cref="T:TradeMagic.Data.PriceType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("37D21B3F-E812-4f71-BF14-37487D21B444")]
    public interface IComPriceType
    {
        /// <summary>
        /// The TradeMagic id of the PriceType.
        /// </summary>
        PriceTypeId Id { get; }
        /// <summary>
        /// The name of the PriceType.
        /// </summary>
        string Name { get; }
    }
}

