namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Type of order.
    /// </summary>
   
    public enum OrderTypeId
    {
        /// <summary>
        /// Limit order.
        /// </summary>
        Limit = 1,
        /// <summary>
        /// Market order.
        /// </summary>
        Market = 0,
        /// <summary>
        /// Stop market order.
        /// </summary>
        Stop = 2,
        /// <summary>
        /// Stop limit order.
        /// </summary>
        StopLimit = 3,
        /// <summary>
        /// Unknown order type.
        /// </summary>
        Unknown = 0x63
    }
}

