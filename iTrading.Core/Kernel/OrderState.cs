namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an order state. Please note, that the pool of available order states (see <see cref="P:iTrading.Core.Kernel.Connection.OrderStates" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("92FAB807-5287-435e-9F04-61EF00C742FE")]
    public class OrderState : IComOrderState
    {
        private static OrderStateDictionary all = null;
        private OrderStateId id;
        private string mapId;

        internal OrderState(OrderStateId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.OrderStates" /> for a collection of <see cref="T:iTrading.Core.Kernel.OrderState" /> objects supported
        /// by the current broker.
        /// </summary>
        public static OrderStateDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new OrderStateDictionary();
                        all.Add(new OrderState(OrderStateId.Accepted, ""));
                        all.Add(new OrderState(OrderStateId.Cancelled, ""));
                        all.Add(new OrderState(OrderStateId.Filled, ""));
                        all.Add(new OrderState(OrderStateId.Initialized, ""));
                        all.Add(new OrderState(OrderStateId.PartFilled, ""));
                        all.Add(new OrderState(OrderStateId.PendingCancel, ""));
                        all.Add(new OrderState(OrderStateId.PendingChange, ""));
                        all.Add(new OrderState(OrderStateId.PendingSubmit, ""));
                        all.Add(new OrderState(OrderStateId.Rejected, ""));
                        all.Add(new OrderState(OrderStateId.Unknown, ""));
                        all.Add(new OrderState(OrderStateId.Working, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the OrderState. This id is independent from the underlying broker system.
        /// </summary>
        public OrderStateId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the OrderState. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the OrderState.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case OrderStateId.Accepted:
                        return "Accepted";

                    case OrderStateId.Cancelled:
                        return "Cancelled";

                    case OrderStateId.Filled:
                        return "Filled";

                    case OrderStateId.Initialized:
                        return "Initialized";

                    case OrderStateId.PartFilled:
                        return "Partially Filled";

                    case OrderStateId.PendingCancel:
                        return "Pending Cancel";

                    case OrderStateId.PendingChange:
                        return "Pending Change";

                    case OrderStateId.PendingSubmit:
                        return "Pending Submit";

                    case OrderStateId.Rejected:
                        return "Rejected";

                    case OrderStateId.Unknown:
                        return "Unknown";

                    case OrderStateId.Working:
                        return "Working";
                }
                return "Initialized";
            }
        }
    }
}

