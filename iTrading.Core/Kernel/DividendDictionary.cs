namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of stock dividends.
    /// </summary>
    [Guid("FF43E185-156A-469b-8F84-D39777D52771"), ClassInterface(ClassInterfaceType.None)]
    public class DividendDictionary : SortedList, IComDividendDictionary
    {
        /// <summary>
        /// Add a dividend.
        /// </summary>
        /// <param name="dividendDate"></param>
        /// <param name="dividend"></param>
        public void Add(DateTime dividendDate, double dividend)
        {
            base.Add(dividendDate, dividend);
        }

        /// <summary>
        /// Checks if the dividend date exists in this container.
        /// </summary>
        /// <param name="dividendDate"></param>
        /// <returns></returns>
        public bool Contains(DateTime dividendDate)
        {
            return base.Contains(dividendDate.Date);
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
        /// Retrieves a dividend by a dividend date. If date is not a dividend date, 0 is returned.
        /// </summary>
        /// <param name="dividendDate"></param>
        public double this[DateTime dividendDate]
        {
            get
            {
                if (this.Contains(dividendDate.Date))
                {
                    return (double) base[dividendDate.Date];
                }
                return 0.0;
            }
            set
            {
                if (this.Contains(dividendDate.Date))
                {
                    base[dividendDate.Date] = value;
                }
                else
                {
                    base.Add(dividendDate.Date, value);
                }
            }
        }

        /// <summary>
        /// The collection of available dividend dates.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.DividendDictionary" />
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

