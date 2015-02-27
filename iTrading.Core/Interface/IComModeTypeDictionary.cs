namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ModeTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("326C0010-C90D-42c5-97BF-F71E75EB4A1F")]
    public interface IComModeTypeDictionary
    {
        /// <summary>
        /// Checks if the ModeType exists in this container.
        /// </summary>
        /// <param name="modeType"></param>
        /// <returns></returns>
        bool Contains(ModeType modeType);
        /// <summary>
        /// Retrieves an ModeType object by its name.
        /// </summary>
        /// <param name="name"></param>
        ModeType Find(string name);
        /// <summary>
        /// Retrieves an ModeType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ModeType this[ModeTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.ModeType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

