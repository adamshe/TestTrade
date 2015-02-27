namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the mode type.
    /// </summary>
    public enum ModeTypeId
    {
        /// <summary>
        /// TradeMagic established a connection to the brokers demo system.
        /// </summary>
        Demo = 1,
        /// <summary>
        /// TradeMagic established a connection to the brokers live system.
        /// </summary>
        Live = 2,
        /// <summary>
        /// TradeMagic does not establish any "real" connection, but simulates data feeds and order execution.
        /// </summary>
        Simulation = 3,
        /// <summary>
        /// TradeMagic established a connection to the brokers test system.
        /// </summary>
        Test = 4
    }
}

