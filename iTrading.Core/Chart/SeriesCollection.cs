using System.Collections;
using Steema.TeeChart.Styles;

namespace iTrading.Core.Chart
{
    /// <summary>
    /// Container holding charting series.
    /// </summary>
    public class SeriesCollection : CollectionBase
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="series"></param>
        public void Add(Series series)
        {
            base.List.Add(series);
        }

        /// <summary>
        /// Checks if the series exists in this container.
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        public bool Contains(Series series)
        {
            return ((IList) this).Contains(series);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Series[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="series"></param>
        /// <returns></returns>
        public int IndexOf(Series series)
        {
            return ((IList) this).IndexOf(series);
        }

        /// <summary>
        /// Get the n-th series of the container.
        /// </summary>
        public Series this[int index]
        {
            get
            {
                return (Series) base.List[index];
            }
        }
    }
}