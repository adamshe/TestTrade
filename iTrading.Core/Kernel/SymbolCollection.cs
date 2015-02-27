namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Symbol collection.
    /// </summary>
    [Guid("46197652-E327-4fdb-A487-DC5DAED0F3E8"), ClassInterface(ClassInterfaceType.None)]
    public class SymbolCollection : CollectionBase, IComSymbolCollection
    {
        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="newSymbol"></param>
        public void Add(Symbol newSymbol)
        {
            base.List.Add(newSymbol);
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Symbol value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Symbol[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(Symbol value)
        {
            return ((IList) this).IndexOf(value);
        }

        /// <summary>
        /// Remove an item.
        /// </summary>
        /// <param name="symbol"></param>
        public void Remove(Symbol symbol)
        {
            base.List.Remove(symbol);
        }

        /// <summary>
        /// Get the n-th Symbol of the container.
        /// </summary>
        public Symbol this[int index]
        {
            get
            {
                return (Symbol) base.List[index];
            }
        }
    }
}

