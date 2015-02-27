namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface IOrderChange
    {
        /// <summary>
        /// Change order properties.
        /// </summary>
        /// <param name="order"></param>
        void Change(Order order);
    }
}

