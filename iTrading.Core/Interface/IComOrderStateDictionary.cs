namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderStateDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("A11A2B24-711B-43d3-B62F-3D363C1BCD90")]
    public interface IComOrderStateDictionary
    {
        /// <summary>
        /// Checks if the OrderState exists in this container.
        /// </summary>
        /// <param name="orderState"></param>
        /// <returns></returns>
        bool Contains(OrderState orderState);
        /// <summary>
        /// Retrieves an OrderState object by its name.
        /// </summary>
        /// <param name="name"></param>
        OrderState Find(string name);
        /// <summary>
        /// Retrieves an OrderState object by its id.
        /// </summary>
        /// <param name="id"></param>
        OrderState this[OrderStateId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.OrderState" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

