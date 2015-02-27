namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface IOrder
    {
        /// <summary>
        /// Cancel order.
        /// </summary>
        /// <param name="order"></param>
        void Cancel(Order order);
        /// <summary>
        /// Submit order to the broker system.
        /// </summary>
        /// <param name="order"></param>
        void Submit(Order order);
    }
}

