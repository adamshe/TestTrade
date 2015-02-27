namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Container holding all orders of an account. <seealso cref="P:iTrading.Core.Kernel.Account.Orders" />
    /// </summary>
    [Guid("46372181-C6CA-4f30-8411-8704EDB9CC2C"), ClassInterface(ClassInterfaceType.None)]
    public class OrderCollection : CollectionBase, IComOrderCollection
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="order"></param>
        public void Add(Order order)
        {
            base.List.Add(order);
        }

        /// <summary>
        /// Checks if the order exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Order value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Order[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves the order with <see cref="P:iTrading.Core.Kernel.Order.OrderId" /> = "orderId".
        /// Please note: Order ids may not be unqiue. Some broker only support unique numbers for the day. 
        /// Some brokers do not even support unqiue order ids amongst different sessions for the day.
        /// TradeMagic returns the order with the latest <see cref="P:iTrading.Core.Kernel.Order.Time" /> value.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order FindByOrderId(string orderId)
        {
            Order order = null;
            lock (this)
            {
                foreach (Order order2 in base.List)
                {
                    if (order2.OrderId == orderId)
                    {
                        if (order == null)
                        {
                            order = order2;
                        }
                        else if (order.Time < order2.Time)
                        {
                            order = order2;
                        }
                    }
                }
                return order;
            }
            return order;
        }

        /// <summary>
        /// Retrieves the order with <see cref="P:iTrading.Core.Kernel.Order.Token" /> = "token".
        /// Please note: Order token are unique, across sessions and days.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Order FindByToken(string token)
        {
            lock (this)
            {
                foreach (Order order in base.List)
                {
                    if (order.Token == token)
                    {
                        return order;
                    }
                }
            }
            return null;
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(Order value)
        {
            return ((IList) this).IndexOf(value);
        }

        internal void Remove(Order order)
        {
            base.List.Remove(order);
        }

        /// <summary>
        /// Get the n-th order of the container.
        /// </summary>
        public Order this[int index]
        {
            get
            {
                return (Order) base.List[index];
            }
        }
    }
}

