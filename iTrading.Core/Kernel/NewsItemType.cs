namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an news item type. Please note, that the pool of available news item types (see <see cref="P:iTrading.Core.Kernel.Connection.NewsItemTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("9E2086E4-E922-4a8e-A742-9F3100717947")]
    public class NewsItemType : IComNewsItemType
    {
        private static NewsItemTypeDictionary all = null;
        private NewsItemTypeId id;
        private string mapId;

        internal NewsItemType(NewsItemTypeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.NewsItemTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.NewsItemType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static NewsItemTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new NewsItemTypeDictionary();
                        all.Add(new NewsItemType(NewsItemTypeId.AfxFocus, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.AfxUk, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.AssociatedPress, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.BusinessWire, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Catalog, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Comtex, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.CbsMarketWatch, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Commercial, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Default, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.DowJones, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Dtn, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.DtnNewsBreak, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.FirstCall, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.FuturesWorld, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.InternetWire, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.JagNotes, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.MarketGuide, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.MarketNewsPub, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.MidnightTrader, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.PrimeZone, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.PREuro, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.PRNewswire, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.RealTimeTrader, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Reuters, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.ReutersBasic, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.ReutersPremium, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.TheFlyOnTheWall, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.Usda, ""));
                        all.Add(new NewsItemType(NewsItemTypeId.ZacksTrader, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the NewsItemType. This id is independent from the underlying broker system.
        /// </summary>
        public NewsItemTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the NewsItemType. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the NewsItemType.
        /// </summary>
        public string Name
        {
            get
            {
                return this.id.ToString();
            }
        }
    }
}

