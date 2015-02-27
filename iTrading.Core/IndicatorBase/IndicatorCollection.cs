using System;
using System.Collections;

namespace iTrading.Core.IndicatorBase
{
    /// <summary>
    /// Container holding indicators.
    /// </summary>
    public class IndicatorCollection : CollectionBase, ICloneable
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="indicator"></param>
        public void Add(IndicatorBase indicator)
        {
            base.List.Add(indicator);
        }

        /// <summary>
        /// Shallow clone.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            IndicatorCollection indicators = new IndicatorCollection();
            foreach (IndicatorBase base2 in this)
            {
                indicators.Add(base2);
            }
            return indicators;
        }

        /// <summary>
        /// Checks if an indicator exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(IndicatorBase value)
        {
            return ((IList) this).Contains(value);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(IndicatorBase[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(IndicatorBase value)
        {
            return ((IList) this).IndexOf(value);
        }

        /// <summary>
        /// Get the n-th indicator of the container.
        /// </summary>
        public IndicatorBase this[int index]
        {
            get
            {
                return (IndicatorBase) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}