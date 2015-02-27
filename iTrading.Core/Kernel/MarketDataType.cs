namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an market data type. Please note, that the pool of available market data types (see <see cref="P:iTrading.Core.Kernel.Connection.MarketDataTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("5FF606D2-87B3-4a72-BF67-6A3D65023247")]
    public class MarketDataType : IComMarketDataType
    {
        private static MarketDataTypeDictionary all = null;
        private MarketDataTypeId id;
        private string mapId;

        internal MarketDataType(MarketDataTypeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.MarketDataTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.MarketDataType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static MarketDataTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new MarketDataTypeDictionary();
                        all.Add(new MarketDataType(MarketDataTypeId.Ask, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.Bid, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.DailyHigh, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.DailyLow, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.DailyVolume, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.Last, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.LastClose, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.Opening, ""));
                        all.Add(new MarketDataType(MarketDataTypeId.Unknown, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the MarketDataType. This id is independent from the underlying broker system.
        /// </summary>
        public MarketDataTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the MarketDataType. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the MarketDataType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case MarketDataTypeId.Ask:
                        return "Ask";

                    case MarketDataTypeId.Bid:
                        return "Bid";

                    case MarketDataTypeId.Last:
                        return "Last";

                    case MarketDataTypeId.DailyHigh:
                        return "Daily High";

                    case MarketDataTypeId.DailyLow:
                        return "Daily Low";

                    case MarketDataTypeId.DailyVolume:
                        return "Daily Volume";

                    case MarketDataTypeId.LastClose:
                        return "Last Close";

                    case MarketDataTypeId.Opening:
                        return "Opening";

                    case MarketDataTypeId.Unknown:
                        return "Unknown";
                }
                return "Unknown";
            }
        }
    }
}

