namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("FDA57346-AC93-490f-94B2-4FFB4469B865")]
    public class MarketDepthRowCollection : CollectionBase, IComMarketDepthRowCollection
    {
        internal MarketDepthRowCollection()
        {
        }

        internal void Add(MarketDepthRow marketDepthRow)
        {
            base.List.Add(marketDepthRow);
        }

        internal  void Clear()
        {
            base.List.Clear();
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(MarketDepthRow value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(MarketDepthRow[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(MarketDepthRow value)
        {
            return ((IList) this).IndexOf(value);
        }

        internal void Insert(int idx, MarketDepthRow marketDepthRow)
        {
            base.List.Insert(idx, marketDepthRow);
        }

        internal void Remove(MarketDepthRow marketDepthRow)
        {
            base.List.Remove(marketDepthRow);
        }

        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.MarketDepth" /> item of the container.
        /// </summary>
        public MarketDepthRow this[int index]
        {
            get
            {
                return (MarketDepthRow) base.List[index];
            }
        }
    }
}

