namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ValuesCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("00A4226F-8EB2-4b34-8BFA-E866350A5E76")]
    public interface IComValuesCollection
    {
        /// <summary>
        /// Checks if the Object exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(object value);
        /// <summary>
        /// Get the n-th Object of the container.
        /// </summary>
        object this[int index] { get; }
        /// <summary>
        /// The number of available elements.
        /// </summary>
        int Count { get; }
    }
}

