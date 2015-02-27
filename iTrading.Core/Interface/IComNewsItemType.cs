namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.NewsItemType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("70DB4C9F-7514-4140-BF6A-8FCAC8AAC640")]
    public interface IComNewsItemType
    {
        /// <summary>
        /// The TradeMagic id of the NewsItemType. This id is independent from the underlying provider system.
        /// </summary>
        NewsItemTypeId Id { get; }
        /// <summary>
        /// The name of the NewsItemType.
        /// </summary>
        string Name { get; }
    }
}

