namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("7BD3B33F-3F21-439e-A254-705EA26C37E1")]
    public interface IComOrderTypeDictionary
    {
        /// <summary>
        /// Checks if the OrderType exists in this container.
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        bool Contains(OrderType orderType);
        /// <summary>
        /// Retrieves an OrderType object by its name.
        /// </summary>
        /// <param name="name"></param>
        OrderType Find(string name);
        /// <summary>
        /// Retrieves an OrderType object by its id.
        /// </summary>
        /// <param name="id"></param>
        OrderType this[OrderTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.OrderType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

