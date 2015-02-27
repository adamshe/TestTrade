namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of stock splits.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("3F0CAF37-51F7-4228-8581-D75EB05F6BA1")]
    public class SplitDictionary : SortedList, IComSplitDictionary
    {
        /// <summary>
        /// Add a split.
        /// </summary>
        /// <param name="splitDate"></param>
        /// <param name="factor"></param>
        public void Add(DateTime splitDate, double factor)
        {
            base.Add(splitDate, factor);
        }

        /// <summary>
        /// Checks if the split exists in this container.
        /// </summary>
        /// <param name="splitDate"></param>
        /// <returns></returns>
        public bool Contains(DateTime splitDate)
        {
            return base.Contains(splitDate.Date);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Exchange[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Get the split factor for a given date.
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns></returns>
        public double GetSplitFactor(DateTime atDate)
        {
            if (this.Count == 0)
            {
                return 1.0;
            }
            double num = 1.0;
            IList keyList = this.GetKeyList();
            for (int i = keyList.Count - 1; i >= 0; i--)
            {
                DateTime time = (DateTime) keyList[i];
                if (time <= atDate)
                {
                    return num;
                }
                num *= this[time];
            }
            return num;
        }

        /// <summary>
        /// Retrieves a split factor by a split date. If date is not a split date, 1 is returned.
        /// </summary>
        /// <param name="splitDate"></param>
        public double this[DateTime splitDate]
        {
            get
            {
                if (this.Contains(splitDate.Date))
                {
                    return (double) base[splitDate.Date];
                }
                return 1.0;
            }
            set
            {
                if (this.Contains(splitDate.Date))
                {
                    base[splitDate.Date] = value;
                }
                else
                {
                    base.Add(splitDate.Date, value);
                }
            }
        }

        /// <summary>
        /// The collection of available split dates.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.SplitDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        public ValuesCollection KeyCollection
        {
            get
            {
                return new ValuesCollection(base.Keys);
            }
        }
    }
}

