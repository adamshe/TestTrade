namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of ConnectionStatus types.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("8661B84A-4A3B-43ee-A345-3E3F59737639")]
    public class ConnectionStatusDictionary : DictionaryBase, IComConnectionStatusDictionary
    {
        internal void Add(ConnectionStatus connectionStatus)
        {
            base.Dictionary.Add(connectionStatus.Id, connectionStatus);
        }

        /// <summary>
        /// Checks if the ConnectionStatus exists in this container.
        /// </summary>
        /// <param name="connectionStatus"></param>
        /// <returns></returns>
        public bool Contains(ConnectionStatus connectionStatus)
        {
            return base.Dictionary.Contains(connectionStatus.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(ConnectionStatus[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an ConnectionStatus object by its name.
        /// </summary>
        /// <param name="name"></param>
        public ConnectionStatus Find(string name)
        {
            foreach (ConnectionStatus status in base.Dictionary.Values)
            {
                if (status.Name == name)
                {
                    return status;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an ConnectionStatus object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConnectionStatus this[ConnectionStatusId id]
        {
            get
            {
                return (ConnectionStatus) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The ICollection of available <see cref="T:iTrading.Core.Kernel.ConnectionStatus" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.ConnectionStatus" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.ConnectionStatusDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        public iTrading.Core.Kernel.ValuesCollection ValuesCollection
        {
            get
            {
                return new iTrading.Core.Kernel.ValuesCollection(base.Dictionary.Values);
            }
        }
    }
}

