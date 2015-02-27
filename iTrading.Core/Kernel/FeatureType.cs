namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an feature type. Please note, that the pool of available feature types (see <see cref="P:iTrading.Core.Kernel.Connection.FeatureTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("DAA653D8-DDB6-47e8-A28D-CFE6062D7E94")]
    public class FeatureType : IComFeatureType
    {
        private static FeatureTypeDictionary all = null;
        private double customValue;
        private FeatureTypeId id;

        internal FeatureType(FeatureTypeId id, double customValue)
        {
            this.customValue = customValue;
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.FeatureTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.FeatureType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static FeatureTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new FeatureTypeDictionary();
                        all.Add(new FeatureType(FeatureTypeId.ClockSynchronization, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.MarketData, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.MarketDepth, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.MaxMarketDataStreams, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.MaxMarketDepthStreams, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.MultipleConnections, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.NativeOcaOrders, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.News, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.Order, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.OrderChange, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.Quotes1Minute, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.QuotesDaily, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.QuotesTick, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.SplitsAdjustedDaily, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.SymbolLookup, 0.0));
                        all.Add(new FeatureType(FeatureTypeId.SynchronousSymbolLookup, 0.0));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the FeatureType. This id is independent from the underlying broker system.
        /// </summary>
        public FeatureTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the FeatureType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case FeatureTypeId.ClockSynchronization:
                        return "Clock Synchronization";

                    case FeatureTypeId.MarketData:
                        return "Market Data";

                    case FeatureTypeId.MarketDepth:
                        return "Market Depth Data";

                    case FeatureTypeId.MaxMarketDataStreams:
                        return "Max. Market Data Streams";

                    case FeatureTypeId.MaxMarketDepthStreams:
                        return "Max. Market Depth Streams";

                    case FeatureTypeId.MultipleConnections:
                        return "Multiple Connections";

                    case FeatureTypeId.NativeOcaOrders:
                        return "Native OCA Orders";

                    case FeatureTypeId.News:
                        return "News";

                    case FeatureTypeId.Order:
                        return "Order Management";

                    case FeatureTypeId.OrderChange:
                        return "Order Shanges";

                    case FeatureTypeId.Quotes1Minute:
                        return "Historical 1 Minute Bars";

                    case FeatureTypeId.QuotesDaily:
                        return "Historical Daily Bars";

                    case FeatureTypeId.QuotesTick:
                        return "Historical Tick Data";

                    case FeatureTypeId.SplitsAdjustedDaily:
                        return "Split Adjusted (Daily)";

                    case FeatureTypeId.SymbolLookup:
                        return "Symbol Lookup";
                }
                return "";
            }
        }

        /// <summary>
        /// Get the associated value.
        /// </summary>
        public double Value
        {
            get
            {
                return this.customValue;
            }
        }
    }
}

