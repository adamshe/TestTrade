namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.RightDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("7F01F2E9-9836-471f-A64B-023F8A8FD77F")]
    public interface IComRightDictionary
    {
        /// <summary>
        /// Checks if the Right exists in this container.
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        bool Contains(Right right);
        /// <summary>
        /// Retrieves an Right object by its name.
        /// </summary>
        /// <param name="name"></param>
        Right Find(string name);
        /// <summary>
        /// Retrieves an Right object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Right this[RightId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.Right" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

