namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.AccountItemType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("EEBDC9CA-7F31-4c38-914C-5756B7A99762")]
    public interface IComAccountItemType
    {
        /// <summary>
        /// The TradeMagic id of the AccountItemType. This id is independent from the underlying provider system.
        /// </summary>
        AccountItemTypeId Id { get; }
        /// <summary>
        /// The provider dependent id of the AccountItemType. 
        /// </summary>
        string MapId { get; }
        /// <summary>
        /// The name of the AccountItemType.
        /// </summary>
        string Name { get; }
    }
}

