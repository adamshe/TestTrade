namespace iTrading.Core.Interface
{
    using System.Runtime.InteropServices;
    using Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.AccountItemTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("89B9E695-DB16-47ba-8CEB-BE7D5CCD5749")]
    public interface IComAccountItemTypeDictionary
    {
        /// <summary>
        /// Checks if the AccountItemType exists in this container.
        /// </summary>
        /// <param name="accountItemType"></param>
        /// <returns></returns>
        bool Contains(AccountItemType accountItemType);
        /// <summary>
        /// Retrieves an AccountItemType object by its name.
        /// </summary>
        /// <param name="name"></param>
        AccountItemType Find(string name);
        /// <summary>
        /// Retrieves an AccountItemType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountItemType this[AccountItemTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.AccountItemType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

