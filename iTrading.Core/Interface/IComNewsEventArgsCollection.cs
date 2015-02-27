namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.NewsEventArgsCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("CA0609AA-FAC8-4a07-9F43-A016CB5A8B67")]
    public interface IComNewsEventArgsCollection
    {
        /// <summary>
        /// Get the n-th NewsEventArgs of the container.
        /// </summary>
        NewsEventArgs this[int index] { get; }
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(NewsEventArgs value);
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.NewsEventArgs" /> instances.
        /// </summary>
        int Count { get; }
    }
}

