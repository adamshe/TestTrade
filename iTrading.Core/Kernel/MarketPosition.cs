namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an order MarketPosition type. Please note, that the pool of available MarketPosition types (see <see cref="P:iTrading.Core.Kernel.Connection.Currencies" />)
    /// varies between brokers/data providers.
    /// </summary>
    [Guid("E88F4461-792A-4404-A86B-6E815E26046F"), ClassInterface(ClassInterfaceType.None)]
    public class MarketPosition : IComMarketPosition
    {
        private static MarketPositionDictionary all = null;
        private MarketPositionId id;
        private string mapId;

        internal MarketPosition(MarketPositionId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.MarketPositions" /> for a collection of <see cref="T:iTrading.Core.Kernel.MarketPosition" /> objects supported
        /// by the current broker.
        /// </summary>
        public static MarketPositionDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new MarketPositionDictionary();
                        all.Add(new MarketPosition(MarketPositionId.Long, ""));
                        all.Add(new MarketPosition(MarketPositionId.Short, ""));
                        all.Add(new MarketPosition(MarketPositionId.Unknown, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the MarketPosition. This id is independent from the underlying broker system.
        /// </summary>
        public MarketPositionId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the MarketPosition. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the MarketPosition.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case MarketPositionId.Long:
                        return "Long";

                    case MarketPositionId.Short:
                        return "Short";

                    case MarketPositionId.Unknown:
                        return "";
                }
                return "";
            }
        }
    }
}

