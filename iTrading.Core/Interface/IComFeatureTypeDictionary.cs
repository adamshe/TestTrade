namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.FeatureTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("775FA15D-107C-4cc5-B10F-61DADFD8CCCB")]
    public interface IComFeatureTypeDictionary
    {
        /// <summary>
        /// Checks if the FeatureType exists in this container.
        /// </summary>
        /// <param name="featureType"></param>
        /// <returns></returns>
        bool Contains(FeatureType featureType);
        /// <summary>
        /// Retrieves an FeatureType object by its name.
        /// </summary>
        /// <param name="name"></param>
        FeatureType Find(string name);
        /// <summary>
        /// Retrieves an FeatureType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FeatureType this[FeatureTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.FeatureType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

