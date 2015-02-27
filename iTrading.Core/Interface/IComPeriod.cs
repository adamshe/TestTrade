using iTrading.Core.Data;
using System.Runtime.InteropServices;

namespace iTrading.Core.Interface
{

    /// <summary>
    /// Size of <see cref="T:iTrading.Core.Data.Period" /> object.
    /// E.g. 5-minutes: PeriodTypeId = PeriodTypeId.Minute and Value = 5.
    /// </summary>
    [Guid("2BB41E61-C5F2-446f-9579-D916DE969D51")]
    public interface IComPeriod
    {
        /// <summary></summary>
        PeriodTypeId Id { get; }
        /// <summary>
        /// Returns the printable string value of this object.
        /// </summary>
        /// <returns></returns>
        string ToString();
        /// <summary></summary>
        int Value { get; }
    }
}

