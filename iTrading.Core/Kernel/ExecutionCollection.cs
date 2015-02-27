namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Container holding all executions of an account. <seealso cref="P:iTrading.Core.Kernel.Account.Executions" />
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("8F63FEDA-1471-4f93-BF27-FF2F7D717641")]
    public class ExecutionCollection : CollectionBase, IComExecutionCollection
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="execution"></param>
        public void Add(Execution execution)
        {
            base.List.Add(execution);
        }

        /// <summary>
        /// Checks if the execution exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Execution value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Execution[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves the execution with <see cref="P:iTrading.Core.Kernel.Execution.Id" /> = "execId".
        /// </summary>
        /// <param name="execId"></param>
        /// <returns></returns>
        public Execution FindByExecId(string execId)
        {
            lock (this)
            {
                foreach (Execution execution in base.List)
                {
                    if (execution.Id == execId)
                    {
                        return execution;
                    }
                }
            }
            return null;
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(Execution value)
        {
            return ((IList) this).IndexOf(value);
        }

        /// <summary>
        /// Get the n-th execution of the container.
        /// </summary>
        public Execution this[int index]
        {
            get
            {
                return (Execution) base.List[index];
            }
        }
    }
}

