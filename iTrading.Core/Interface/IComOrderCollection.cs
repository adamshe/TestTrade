namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.OrderCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("889045C8-77C5-49ac-BE77-93E2C94FC479")]
    public interface IComOrderCollection
    {
        /// <summary>
        /// Checks if an order exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(Order value);
        /// <summary>
        /// Retrieves the order with <see cref="P:iTrading.Core.Kernel.Order.OrderId" /> = "orderId".
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order FindByOrderId(string orderId);
        /// <summary>
        /// Retrieves the order with <see cref="P:iTrading.Core.Kernel.Order.Token" /> = "token".
        /// Please note: Order token are unique, across sessions and days.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Order FindByToken(string token);
        /// <summary>
        /// Get the n-th order of the container.
        /// </summary>
        Order this[int index] { get; }
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.Order" /> instances.
        /// </summary>
        int Count { get; }
    }
}

