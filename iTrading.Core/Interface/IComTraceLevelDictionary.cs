namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.TraceLevelDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("CEECDF5F-7E5D-4a1d-A1EE-76C414EFB02B")]
    public interface IComTraceLevelDictionary
    {
        /// <summary>
        /// Checks if the tracelevel exists in this container.
        /// </summary>
        /// <param name="traceLevel"></param>
        /// <returns></returns>
        bool Contains(TraceLevel traceLevel);
        /// <summary>
        /// Retrieves an TraceLevel object by its name.
        /// </summary>
        /// <param name="name"></param>
        TraceLevel Find(string name);
        /// <summary>
        /// Retrieves an TraceLevel object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TraceLevel this[TraceLevelIds id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.TraceLevel" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

