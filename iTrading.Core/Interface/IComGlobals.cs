namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Globals" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("223621EE-751B-4bf2-BF27-04FB45B33DDB")]
    public interface IComGlobals
    {
        /// <summary>
        /// Get a collection of all available provider types.
        /// </summary>
        ProviderTypeDictionary ProviderTypes { get; }
        /// <summary>
        /// TradeMagic installation directory.
        /// </summary>
        string InstallDir { get; }
        /// <summary>
        /// Get a collection of all available license types.
        /// </summary>
        LicenseTypeDictionary LicenseTypes { get; }
        /// <summary>
        /// Get a collection of all available mode types.
        /// </summary>
        ModeTypeDictionary ModeTypes { get; }
        /// <summary>
        /// Get a collection of all available quote period types.
        /// </summary>
        PeriodTypeDictionary PeriodTypes { get; }
        /// <summary>
        /// Get the TradeMagic trace switch.
        /// </summary>
        TMTraceSwitch TraceSwitch { get; }
        /// <summary>
        /// Get a collection of all available trace levels.
        /// </summary>
        TraceLevelDictionary TraceLevels { get; }
    }
}

