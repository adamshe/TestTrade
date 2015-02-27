namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Container holding the history of <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> for an order.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("6C9368DA-5207-421d-AE1D-A65EE07A8B35")]
    public class OrderStatusEventCollection : CollectionBase, IComOrderStatusEventCollection
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="e"></param>
        public void Add(OrderStatusEventArgs e)
        {
            base.List.Add(e);
        }

        /// <summary>
        /// Checks if an <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(OrderStatusEventArgs value)
        {
            return ((IList) this).Contains(value);
        }

        internal void Remove(OrderStatusEventArgs e)
        {
            base.List.Remove(e);
        }

        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> of the container.
        /// </summary>
        public OrderStatusEventArgs this[int index]
        {
            get
            {
                return (OrderStatusEventArgs) base.List[index];
            }
        }
    }
}

