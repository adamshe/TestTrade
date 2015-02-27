namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.NewsItemTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("CD382946-AEC2-4f45-B6E5-8B774FE3AD93")]
    public interface IComNewsItemTypeDictionary
    {
        /// <summary>
        /// Checks if the NewsItemType exists in this container.
        /// </summary>
        /// <param name="newsItemType"></param>
        /// <returns></returns>
        bool Contains(NewsItemType newsItemType);
        /// <summary>
        /// Retrieves an NewsItemType object by its name.
        /// </summary>
        /// <param name="name"></param>
        NewsItemType Find(string name);
        /// <summary>
        /// Retrieves an NewsItemType object by its id.
        /// </summary>
        /// <param name="id"></param>
        NewsItemType this[NewsItemTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.NewsItemType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

