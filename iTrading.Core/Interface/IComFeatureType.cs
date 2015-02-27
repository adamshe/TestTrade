namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.FeatureType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("068C3FC2-90A8-46f0-914E-178CEDF1241E")]
    public interface IComFeatureType
    {
        /// <summary>
        /// The TradeMagic id of the FeatureType. This id is independent from the underlying provider system.
        /// </summary>
        FeatureTypeId Id { get; }
        /// <summary>
        /// The name of the FeatureType.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Get the associated value.
        /// </summary>
        double Value { get; }
    }
}

