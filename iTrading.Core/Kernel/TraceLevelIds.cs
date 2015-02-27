namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the Tracelevel type.
    /// </summary>
    [Flags]
    public enum TraceLevelIds
    {
        /// <summary>
        /// Traces everything
        /// </summary>
        All = 0xffff,
        /// <summary>
        /// Traces COM related actions.
        /// </summary>
        Com = 1,
        /// <summary>
        /// Traces custom application related actions.
        /// </summary>
        ComClient = 4,
        /// <summary>
        /// Traces connect/disconnect related actions.
        /// </summary>
        Connect = 2,
        /// <summary>
        /// Trace database related actions.
        /// </summary>
        DataBase = 8,
        /// <summary>
        /// Traces indicator related actions.
        /// </summary>
        Indicator = 0x4000,
        /// <summary>
        /// Traces marketdata related actions (may decrease performance).
        /// </summary>
        MarketData = 0x10,
        /// <summary>
        /// Traces marketdepth related actions (may decrease performance).
        /// </summary>
        MarketDepth = 0x20,
        /// <summary>
        /// Traces adapter internal processing (may decrease performance).
        /// </summary>
        Native = 0x40,
        /// <summary>
        /// Traces news related actions.
        /// </summary>
        News = 0x80,
        /// <summary>
        /// Traces nothing.
        /// </summary>
        None = 0,
        /// <summary>
        /// Traces order related actions incl. executions, position updates etc.
        /// </summary>
        Order = 0x100,
        /// <summary>
        /// Traces quotes related actions.
        /// </summary>
        Quotes = 0x1000,
        /// <summary>
        /// Turns on some internal checks.
        /// </summary>
        Strict = 0x8000,
        /// <summary>
        /// Traces symbol lookup related actions.
        /// </summary>
        SymbolLookup = 0x200,
        /// <summary>
        /// Traces the test driver.
        /// </summary>
        Test = 0x400,
        /// <summary>
        /// Traces timer related actions.
        /// </summary>
        Timer = 0x2000,
        /// <summary>
        /// Traces XXXTypeEvents (e.g. <see cref="E:iTrading.Core.Kernel.ExchangeDictionary.Exchange" />) related actions.
        /// </summary>
        Types = 0x800
    }
}

