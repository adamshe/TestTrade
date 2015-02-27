namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Right" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("F4D4F493-7749-4e7a-8397-836EBAC9157E")]
    public interface IComRight
    {
        /// <summary>
        /// The TradeMagic id of the Right.
        /// </summary>
        RightId Id { get; }
        /// <summary>
        /// The name of the Right.
        /// </summary>
        string Name { get; }
    }
}

