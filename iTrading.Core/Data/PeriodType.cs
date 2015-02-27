namespace iTrading.Core.Data
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a <see cref="T:TradeMagic.Data.PeriodType" /> type.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("13A01C8C-6769-4d23-868A-0B300D69E85E")]
    public class PeriodType : IComPeriodType
    {
        private static PeriodTypeDictionary all = null;
        private PeriodTypeId id;

        internal PeriodType(PeriodTypeId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available quotes size type.
        /// </summary>
        public static PeriodTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new PeriodTypeDictionary();
                        all.Add(new PeriodType(PeriodTypeId.Day));
                        all.Add(new PeriodType(PeriodTypeId.Minute));
                        all.Add(new PeriodType(PeriodTypeId.Tick));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the quotes size type.
        /// </summary>
        public PeriodTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the <see cref="T:TradeMagic.Data.PeriodType" />.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case PeriodTypeId.Day:
                        return "Day";

                    case PeriodTypeId.Minute:
                        return "Minute";

                    case PeriodTypeId.Tick:
                        return "Tick";
                }
                return "Minute";
            }
        }
    }
}

