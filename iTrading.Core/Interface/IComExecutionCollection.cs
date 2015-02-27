namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.ExecutionCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("F238A8D9-CDC9-49d4-B6AD-FA6F1FAEBE56")]
    public interface IComExecutionCollection
    {
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(Execution value);
        /// <summary>
        /// Get the n-th execution of the container.
        /// </summary>
        Execution this[int index] { get; }
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.Execution" /> instances.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Retrieves the execution with <see cref="P:iTrading.Core.Kernel.Execution.Id" /> = "execId".
        /// </summary>
        /// <param name="execId"></param>
        /// <returns></returns>
        Execution FindByExecId(string execId);
    }
}

