namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ModeType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("9E6B6EFA-2430-43fd-BE89-D68F29CFD993")]
    public interface IComModeType
    {
        /// <summary>
        /// The TradeMagic id of the ModeType.
        /// </summary>
        ModeTypeId Id { get; }
        /// <summary>
        /// The name of the ModeType.
        /// </summary>
        string Name { get; }
    }
}

