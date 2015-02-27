namespace iTrading.Core.Kernel
{
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface ILoader
    {
        /// <summary>
        /// Create a broker adapter instance.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        IAdapter Create(Connection connection);
    }
}

