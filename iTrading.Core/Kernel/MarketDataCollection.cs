namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("D527A34B-F0C6-4aa0-BE29-B82940D2DE7D")]
    public class MarketDataCollection : CollectionBase, IComMarketDataCollection
    {
        internal MarketDataCollection()
        {
        }

        internal void Add(MarketData marketData)
        {
            base.List.Add(marketData);
        }

        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(MarketData value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(MarketData[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(MarketData value)
        {
            return ((IList) this).IndexOf(value);
        }

        internal void Remove(MarketData marketData)
        {
            base.List.Remove(marketData);
        }

        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.MarketData" /> item of the container.
        /// </summary>
        public MarketData this[int index]
        {
            get
            {
                return (MarketData) base.List[index];
            }
        }
    }
}

