using System.Collections;
using iTrading.Core.Chart;

namespace iTrading.Core.Chart
{
    /// <summary>
    /// Container holding chart levels <seealso cref="T:iTrading.Core.Chart.Level" />.
    /// </summary>
    public class LevelCollection : CollectionBase
    {
        /// <summary>
        /// Add a level.
        /// </summary>
        /// <param name="level"></param>
        public void Add(Level level)
        {
            base.List.Add(level);
        }

        /// <summary>
        /// Checks if the level exists in this container.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool Contains(Level level)
        {
            return ((IList) this).Contains(level);
        }

        /// <summary></summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Level[] array, int index)
        {
            ((ICollection) this).CopyTo(array, index);
        }

        /// <summary></summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int IndexOf(Level level)
        {
            return ((IList) this).IndexOf(level);
        }

        /// <summary>
        /// Get the n-th level of the container.
        /// </summary>
        public Level this[int index]
        {
            get
            {
                return (Level) base.List[index];
            }
        }
    }
}