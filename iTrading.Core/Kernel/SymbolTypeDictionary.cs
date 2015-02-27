namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of SymbolTypes.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("D3625207-F11C-4d6b-9187-DA7D5D9CEFFF")]
    public class SymbolTypeDictionary : DictionaryBase, IComSymbolTypeDictionary
    {
        /// <summary>
        /// This event will be thrown once for every new SymbolType supported after opening the connection.
        /// </summary>
        public event iTrading.Core.Kernel.SymbolTypeEventHandler SymbolType;

        public void OnSymbolTypeChange(object pSender, SymbolTypeEventArgs pEvent)
        {
            if (SymbolType != null)
                SymbolType(pSender, pEvent);
        }

        internal void Add(iTrading.Core.Kernel.SymbolType symbolType)
        {
            base.Dictionary.Add(symbolType.Id, symbolType);
        }

        /// <summary>
        /// Checks if the SymbolType exists in this container.
        /// </summary>
        /// <param name="symbolType"></param>
        /// <returns></returns>
        public bool Contains(iTrading.Core.Kernel.SymbolType symbolType)
        {
            return base.Dictionary.Contains(symbolType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(iTrading.Core.Kernel.SymbolType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves a SymbolType object by its name.
        /// Will return null if not found.
        /// </summary>
        /// <param name="name"></param>
        public iTrading.Core.Kernel.SymbolType Find(string name)
        {
            foreach (iTrading.Core.Kernel.SymbolType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves a SymbolType object, by its broker dependent map id.
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.SymbolType FindByMapId(string mapId)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.SymbolType type in base.Dictionary.Values)
                {
                    if (type.MapId == mapId)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves the symbol type with <see cref="P:iTrading.Core.Kernel.Account.Name" /> = "name".
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.SymbolType FindByName(string name)
        {
            lock (this)
            {
                foreach (iTrading.Core.Kernel.SymbolType type in base.Dictionary.Values)
                {
                    if (type.Name == name)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves a SymbolType object by its id.
        /// </summary>
        /// <param name="id"></param>
        public iTrading.Core.Kernel.SymbolType this[SymbolTypeId id]
        {
            get
            {
                return (iTrading.Core.Kernel.SymbolType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="E:iTrading.Core.Kernel.SymbolTypeDictionary.SymbolType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.SymbolType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.SymbolTypeDictionary" />
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

