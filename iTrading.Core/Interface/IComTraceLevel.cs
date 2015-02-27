namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.TraceLevel" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("2E227725-CE78-403c-AD2E-764A86682C8C")]
    public interface IComTraceLevel
    {
        /// <summary>
        /// The id of the TraceLevel.
        /// </summary>
        TraceLevelIds Id { get; }
        /// <summary>
        /// The name of the TraceLevel.
        /// </summary>
        string Name { get; }
    }
}

