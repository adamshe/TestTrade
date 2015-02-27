namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an OrderType. Please note, that the pool of available OrderTypes (see <see cref="P:iTrading.Core.Kernel.Connection.OrderTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [Guid("EB455F5D-7750-4449-AD39-3A7FA52B6FC0"), ClassInterface(ClassInterfaceType.None)]
    public class OrderType : IComOrderType
    {
        private static OrderTypeDictionary all = null;
        private OrderTypeId id;
        private string mapId;

        internal OrderType(OrderTypeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.OrderTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.OrderType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static OrderTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new OrderTypeDictionary();
                        all.Add(new OrderType(OrderTypeId.Limit, ""));
                        all.Add(new OrderType(OrderTypeId.Market, ""));
                        all.Add(new OrderType(OrderTypeId.Stop, ""));
                        all.Add(new OrderType(OrderTypeId.StopLimit, ""));
                        all.Add(new OrderType(OrderTypeId.Unknown, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the OrderType. This id is independent from the underlying broker system.
        /// </summary>
        public OrderTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the OrderType. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the OrderType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case OrderTypeId.Market:
                        return "Market";

                    case OrderTypeId.Limit:
                        return "Limit";

                    case OrderTypeId.Stop:
                        return "Stop";

                    case OrderTypeId.StopLimit:
                        return "Stop Limit";

                    case OrderTypeId.Unknown:
                        return "Unknown";
                }
                return "Market";
            }
        }
    }
}

