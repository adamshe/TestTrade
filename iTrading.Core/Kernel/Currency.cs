namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an order currency type. Please note, that the pool of available currency types (see <see cref="P:iTrading.Core.Kernel.Connection.Currencies" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("34E0B2F3-5AA8-42c1-AA47-255180B63F4B")]
    public class Currency : IComCurrency
    {
        private static CurrencyDictionary all = null;
        private CurrencyId id;
        private string mapId;

        internal Currency(CurrencyId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.Currencies" /> for a collection of <see cref="T:iTrading.Core.Kernel.Currency" /> objects supported
        /// by the current broker.
        /// </summary>
        public static CurrencyDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new CurrencyDictionary();
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.AustralianDollar, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.BritishPound, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.CanadianDollar, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.Euro, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.HongKongDollar, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.JapaneseYen, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.SwissFranc, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.Unknown, ""));
                        all.Add(new iTrading.Core.Kernel.Currency(CurrencyId.UsDollar, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the currency. This id is independent from the underlying broker system.
        /// </summary>
        public CurrencyId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the currency. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The currency name.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case CurrencyId.AustralianDollar:
                        return "Australian Dollar";

                    case CurrencyId.BritishPound:
                        return "British Pound";

                    case CurrencyId.CanadianDollar:
                        return "Canadian Dollar";

                    case CurrencyId.Euro:
                        return "Euro";

                    case CurrencyId.HongKongDollar:
                        return "Hong Kong Dollar";

                    case CurrencyId.JapaneseYen:
                        return "Japanese Yen";

                    case CurrencyId.SwissFranc:
                        return "Swiss Francs";

                    case CurrencyId.Unknown:
                        return "Unknown";

                    case CurrencyId.UsDollar:
                        return "US Dollar";
                }
                return "";
            }
        }

        /// <summary>
        /// The currency symbol.
        /// </summary>
        public string Sign
        {
            get
            {
                switch (this.id)
                {
                    case CurrencyId.AustralianDollar:
                        return "$";

                    case CurrencyId.BritishPound:
                        return "\x00a3";

                    case CurrencyId.CanadianDollar:
                        return "$";

                    case CurrencyId.Euro:
                        return "€";

                    case CurrencyId.HongKongDollar:
                        return "$";

                    case CurrencyId.JapaneseYen:
                        return "\x00a5";

                    case CurrencyId.SwissFranc:
                        return "";

                    case CurrencyId.Unknown:
                        return "";

                    case CurrencyId.UsDollar:
                        return "$";
                }
                return "";
            }
        }
    }
}

