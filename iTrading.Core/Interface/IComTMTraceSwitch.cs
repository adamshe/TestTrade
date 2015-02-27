namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.TMTraceSwitch" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("E9E4173A-33B2-4744-8A17-23493F62E0C5")]
    public interface IComTMTraceSwitch
    {
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.All" /> trace level it set.
        /// </summary>
        bool All { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Connect" /> trace level it set.
        /// </summary>
        bool Connect { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Indicator" /> trace level it set.
        /// </summary>
        bool Indicator { get; }
        /// <summary>
        /// Get/set trace level.
        /// </summary>
        TraceLevelIds Level { get; set; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.MarketData" /> trace level it set.
        /// </summary>
        bool MarketData { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.MarketDepth" /> trace level it set.
        /// </summary>
        bool MarketDepth { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.News" /> trace level it set.
        /// </summary>
        bool News { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Order" /> trace level it set.
        /// </summary>
        bool Order { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Quotes" /> trace level it set.
        /// </summary>
        bool Quotes { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Strict" /> trace level it set.
        /// </summary>
        bool Strict { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.SymbolLookup" /> trace level it set.
        /// </summary>
        bool SymbolLookup { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Timer" /> trace level it set.
        /// </summary>
        bool Timer { get; }
        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Types" /> trace level it set.
        /// </summary>
        bool Types { get; }
    }
}

