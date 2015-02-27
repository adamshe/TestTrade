namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Container holding all open positions of an account. <seealso cref="P:iTrading.Core.Kernel.Account.Positions" />
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("A3059F6F-C68D-4115-B0CC-0015B50A56C7")]
    public class PositionCollection : CollectionBase, IComPositionCollection
    {
        internal void Add(Position newPosition)
        {
            base.List.Add(newPosition);
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(Position value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Position[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary>
        /// Get positions matching a given <see cref="T:iTrading.Core.Kernel.Symbol" />. 
        /// Please note: <see cref="P:iTrading.Core.Kernel.Symbol.Exchange" /> is not used to identify the <see cref="T:iTrading.Core.Kernel.Symbol" />.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Position FindBySymbol(Symbol symbol)
        {
            lock (this)
            {
                foreach (Position position in base.List)
                {
                    if (position.Symbol.IsEqual(symbol))
                    {
                        return position;
                    }
                }
            }
            return null;
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(Position value)
        {
            return ((IList) this).IndexOf(value);
        }

        internal void Remove(Position position)
        {
            base.List.Remove(position);
        }

        /// <summary>
        /// Get the n-th position of the container.
        /// </summary>
        public Position this[int index]
        {
            get
            {
                return (Position) base.List[index];
            }
        }
    }
}

