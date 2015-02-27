namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// A collection of running <see cref="T:iTrading.Core.Kernel.MarketDepth" /> streams.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("E2ABE225-2EFD-490a-919C-A6D9F95B56E8")]
    public class MarketDepthCollection : CollectionBase, IComMarketDepthCollection
    {
        internal MarketDepthCollection()
        {
        }

        internal void Add(MarketDepth marketDepth)
        {
            base.List.Add(marketDepth);
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(MarketDepth value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(MarketDepth[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(MarketDepth value)
        {
            return ((IList) this).IndexOf(value);
        }

        internal void Remove(MarketDepth marketDepth)
        {
            base.List.Remove(marketDepth);
        }

        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.MarketDepth" /> item of the container.
        /// </summary>
        public MarketDepth this[int index]
        {
            get
            {
                return (MarketDepth) base.List[index];
            }
        }
    }
}

