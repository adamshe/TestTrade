namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an exchange. Please note, that the pool of available exchanges (see <see cref="P:iTrading.Core.Kernel.Connection.Exchanges" />)
    /// varies between brokers/data providers.
    /// </summary>
    [Guid("2EDA0E60-E0C4-4899-853C-C2415CB12FC4"), ClassInterface(ClassInterfaceType.None)]
    public class Exchange : IComExchange
    {
        private static ExchangeDictionary all = null;
        private ExchangeId id;
        private string mapId;

        internal Exchange(ExchangeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.Exchanges" /> for a collection of <see cref="T:iTrading.Core.Kernel.Exchange" /> objects supported
        /// by the current broker.
        /// </summary>
        public static ExchangeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new ExchangeDictionary();
                        all.Add(new Exchange(ExchangeId.Ace, ""));
                        all.Add(new Exchange(ExchangeId.Amex, ""));
                        all.Add(new Exchange(ExchangeId.Arca, ""));
                        all.Add(new Exchange(ExchangeId.Belfox, ""));
                        all.Add(new Exchange(ExchangeId.Box, ""));
                        all.Add(new Exchange(ExchangeId.Brut, ""));
                        all.Add(new Exchange(ExchangeId.BTrade, ""));
                        all.Add(new Exchange(ExchangeId.Caes, ""));
                        all.Add(new Exchange(ExchangeId.Cboe, ""));
                        all.Add(new Exchange(ExchangeId.Cfe, ""));
                        all.Add(new Exchange(ExchangeId.Default, ""));
                        all.Add(new Exchange(ExchangeId.ECbot, ""));
                        all.Add(new Exchange(ExchangeId.Eurex, ""));
                        all.Add(new Exchange(ExchangeId.EurexSW, ""));
                        all.Add(new Exchange(ExchangeId.EurexUS, ""));
                        all.Add(new Exchange(ExchangeId.Fta, ""));
                        all.Add(new Exchange(ExchangeId.Globex, ""));
                        all.Add(new Exchange(ExchangeId.Hkfe, ""));
                        all.Add(new Exchange(ExchangeId.IBIdeal, ""));
                        all.Add(new Exchange(ExchangeId.IBTmbr, ""));
                        all.Add(new Exchange(ExchangeId.IBVwap, ""));
                        all.Add(new Exchange(ExchangeId.IBIdealPro, ""));
                        all.Add(new Exchange(ExchangeId.Idem, ""));
                        all.Add(new Exchange(ExchangeId.Inca, ""));
                        all.Add(new Exchange(ExchangeId.Ise, ""));
                        all.Add(new Exchange(ExchangeId.Island, ""));
                        all.Add(new Exchange(ExchangeId.Liffe, ""));
                        all.Add(new Exchange(ExchangeId.Lse, ""));
                        all.Add(new Exchange(ExchangeId.Me, ""));
                        all.Add(new Exchange(ExchangeId.Meff, ""));
                        all.Add(new Exchange(ExchangeId.Monep, ""));
                        all.Add(new Exchange(ExchangeId.Nasdaq, ""));
                        all.Add(new Exchange(ExchangeId.Nnm, ""));
                        all.Add(new Exchange(ExchangeId.Nqlx, ""));
                        all.Add(new Exchange(ExchangeId.Nscm, ""));
                        all.Add(new Exchange(ExchangeId.Nybot, ""));
                        all.Add(new Exchange(ExchangeId.Nymex, ""));
                        all.Add(new Exchange(ExchangeId.Nyse, ""));
                        all.Add(new Exchange(ExchangeId.Oes, ""));
                        all.Add(new Exchange(ExchangeId.One, ""));
                        all.Add(new Exchange(ExchangeId.Opra, ""));
                        all.Add(new Exchange(ExchangeId.Ose, ""));
                        all.Add(new Exchange(ExchangeId.OtcBB, ""));
                        all.Add(new Exchange(ExchangeId.Phlx, ""));
                        all.Add(new Exchange(ExchangeId.Pse, ""));
                        all.Add(new Exchange(ExchangeId.Redi, ""));
                        all.Add(new Exchange(ExchangeId.SDot, ""));
                        all.Add(new Exchange(ExchangeId.Sfe, ""));
                        all.Add(new Exchange(ExchangeId.Sfx, ""));
                        all.Add(new Exchange(ExchangeId.Sgx, ""));
                        all.Add(new Exchange(ExchangeId.Soes, ""));
                        all.Add(new Exchange(ExchangeId.Snfe, ""));
                        all.Add(new Exchange(ExchangeId.Swb, ""));
                        all.Add(new Exchange(ExchangeId.Swx, ""));
                        all.Add(new Exchange(ExchangeId.Tse, ""));
                        all.Add(new Exchange(ExchangeId.Tsx, ""));
                        all.Add(new Exchange(ExchangeId.TsxV, ""));
                        all.Add(new Exchange(ExchangeId.VirtX, ""));
                        all.Add(new Exchange(ExchangeId.Xetra, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the exchange. This id is independent from the underlying broker system.
        /// </summary>
        public ExchangeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the exchange. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the exchange.
        /// </summary>
        public string Name
        {
            get
            {
                return this.id.ToString().ToUpper();
            }
        }
    }
}

