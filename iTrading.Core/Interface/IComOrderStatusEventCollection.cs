namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("C3BC9075-7491-4474-9F44-4061E772DA11")]
    public interface IComOrderStatusEventCollection
    {
        /// <summary>
        /// Checks if an <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(OrderStatusEventArgs value);
        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> of the container.
        /// </summary>
        OrderStatusEventArgs this[int index] { get; }
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> instances.
        /// </summary>
        int Count { get; }
    }
}

