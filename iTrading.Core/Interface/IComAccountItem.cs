namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.AccountItem" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("6841AF66-72C6-4844-8A75-71502BD36DB3")]
    public interface IComAccountItem
    {
        /// <summary>
        /// Currency.
        /// </summary>
        iTrading.Core.Kernel.Currency Currency { get; }
        /// <summary>
        /// </summary>
        /// <param name="myObject"></param>
        /// <returns></returns>
        bool Equals(object myObject);
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>Hash code value</returns>
        int GetHashCode();
        /// <summary>
        /// </summary>
        /// <returns></returns>
        string ToString();
        /// <summary>
        /// Value.
        /// </summary>
        double Value { get; }
    }
}

