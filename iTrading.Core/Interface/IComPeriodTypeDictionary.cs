namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Dictionary of quotes size types.
    /// </summary>
    [Guid("13C22356-BE5B-4f1c-92D5-F6D040641390")]
    public interface IComPeriodTypeDictionary
    {
        /// <summary>
        /// Checks if the PeriodType exists in this container.
        /// </summary>
        /// <param name="periodType"></param>
        /// <returns></returns>
        bool Contains(PeriodType periodType);
        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void CopyTo(PeriodType[] array, int index);
        /// <summary>
        /// Retrieves an <see cref="T:TradeMagic.Data.PeriodType" /> object by its name.
        /// </summary>
        /// <param name="name"></param>
        PeriodType Find(string name);
        /// <summary>
        /// Retrieves an <see cref="T:TradeMagic.Data.PeriodType" /> object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PeriodType this[PeriodTypeId id] { get; }
        /// <summary>
        /// The Collection of available <see cref="T:TradeMagic.Data.PeriodType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:TradeMagic.Data.PeriodTypeDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

