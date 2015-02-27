namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an order action type. Please note, that the pool of available action types (see <see cref="P:iTrading.Core.Kernel.Connection.ActionTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("F46CEEC7-220C-4cf8-B24C-A01FF889BC67")]
    public class ActionType : IComActionType
    {
        private static ActionTypeDictionary all = null;
        private ActionTypeId id;
        private string mapId;

        internal ActionType(ActionTypeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.ActionTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.ActionType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static ActionTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new ActionTypeDictionary();
                        all.Add(new ActionType(ActionTypeId.Buy, ""));
                        all.Add(new ActionType(ActionTypeId.BuyToCover, ""));
                        all.Add(new ActionType(ActionTypeId.Sell, ""));
                        all.Add(new ActionType(ActionTypeId.SellShort, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the ActionType. This id is independent from the underlying broker system.
        /// </summary>
        public ActionTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the ActionType. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the ActionType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case ActionTypeId.Buy:
                        return "Buy";

                    case ActionTypeId.BuyToCover:
                        return "Buy to Cover";

                    case ActionTypeId.Sell:
                        return "Sell";

                    case ActionTypeId.SellShort:
                        return "Sell Short";
                }
                return "Sell";
            }
        }
    }
}

