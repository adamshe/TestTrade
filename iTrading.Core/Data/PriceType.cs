namespace iTrading.Core.Data
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a price type.
    /// </summary>
    [Guid("A2569D71-3E65-4aea-8E7D-50A3983B7DFA"), ClassInterface(ClassInterfaceType.None)]
    public class PriceType : IComPriceType
    {
        private static PriceTypeDictionary all = null;
        private PriceTypeId id;

        internal PriceType(PriceTypeId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available price types.
        /// </summary>
        public static PriceTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new PriceTypeDictionary();
                        all.Add(new PriceType(PriceTypeId.Close));
                        all.Add(new PriceType(PriceTypeId.High));
                        all.Add(new PriceType(PriceTypeId.Low));
                        all.Add(new PriceType(PriceTypeId.Median));
                        all.Add(new PriceType(PriceTypeId.Open));
                        all.Add(new PriceType(PriceTypeId.Typical));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the PriceType.
        /// </summary>
        public PriceTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the PriceType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case PriceTypeId.Close:
                        return "Close";

                    case PriceTypeId.High:
                        return "High";

                    case PriceTypeId.Low:
                        return "Low";

                    case PriceTypeId.Median:
                        return "Median";

                    case PriceTypeId.Open:
                        return "Open";

                    case PriceTypeId.Typical:
                        return "Typical";
                }
                return "Open";
            }
        }
    }
}

