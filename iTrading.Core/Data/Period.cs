using System.Diagnostics;
using System.Runtime.InteropServices;
using iTrading.Core.Interface;
using iTrading.Core.Data;

namespace iTrading.Core.Data
{
    /// <summary>
    /// Size of <see cref="T:Quotes" /> object.
    /// E.g. 5-minutes: PeriodTypeId = PeriodTypeId.Minute and Value = 5.
    /// </summary>
    [Guid("C3CB47AB-A37D-4e7d-ABB3-EBAD8387B623"), ClassInterface(ClassInterfaceType.None)]
    public class Period : IComPeriod
    {
        private PeriodTypeId id;
        private int val;

        /// <summary>
        /// </summary>
        /// <param name="periodTypeId"></param>
        /// <param name="periodValue">Must be greater than 0.</param>
        public Period(PeriodTypeId periodTypeId, int periodValue)
        {
            Trace.Assert(periodValue > 0, "Cbi.Period.ctor: # of periods must be greater than 0");
            this.id = periodTypeId;
            this.val = periodValue;
        }

        /// <summary>
        /// Returns the printable string value of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Id == PeriodTypeId.Tick)
            {
                return (this.val + " tick" + ((this.val == 1) ? "" : "s"));
            }
            if (this.Id == PeriodTypeId.Minute)
            {
                return (this.val + " min");
            }
            if (this.val != 1)
            {
                return (this.val + " days");
            }
            return "Daily";
        }

        /// <summary></summary>
        public PeriodTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary></summary>
        public int Value
        {
            get
            {
                return this.val;
            }
        }
    }
}