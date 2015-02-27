namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ActionTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("06B373D1-6FB9-4884-9D16-41057F2FCC90")]
    public interface IComActionTypeDictionary
    {
        /// <summary>
        /// Checks if the ActionType exists in this container.
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        bool Contains(ActionType actionType);
        /// <summary>
        /// Retrieves an ActionType object by its name.
        /// </summary>
        /// <param name="name"></param>
        ActionType Find(string name);
        /// <summary>
        /// Retrieves an ActionType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionType this[ActionTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.ActionType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

