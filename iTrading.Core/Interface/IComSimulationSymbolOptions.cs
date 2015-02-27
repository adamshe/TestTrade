namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.SimulationSymbolOptions" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("4D4548DD-F73C-4f13-B147-15C48066159B")]
    public interface IComSimulationSymbolOptions
    {
        /// <summary>
        /// Minimum fixed commission per side.
        /// Total commission = <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionMin" /> + <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionPerUnit" /> * units.
        /// </summary>
        double CommissionMin { get; set; }
        /// <summary>
        /// Commission per unit and side.
        /// Total commission = <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionMin" /> + <see cref="P:iTrading.Core.Kernel.SimulationSymbolOptions.CommissionPerUnit" /> * units.
        /// </summary>
        double CommissionPerUnit { get; set; }
        /// <summary>
        /// Margin requirement per unit of the traded symbol.
        /// </summary>
        double Margin { get; set; }
        /// <summary>
        /// Slippage in ticks per side.
        /// </summary>
        double Slippage { get; set; }
        /// <summary>
        /// Version number.
        /// </summary>
        int Version { get; }
    }
}

