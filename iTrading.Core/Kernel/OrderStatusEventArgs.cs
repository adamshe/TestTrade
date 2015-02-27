using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Core.Kernel.OrderStatusEventHandler" />.
    /// </summary>
    [Guid("2C0985C8-0708-4728-B546-8517DFF609AF"), ClassInterface(ClassInterfaceType.None)]
    public class OrderStatusEventArgs : ITradingBaseEventArgs, ITradingSerializable, IComOrderStatusEventArgs
    {
        private double avgFillPrice;
        private int filled;
        private double limitPrice;
        internal iTrading.Core.Kernel.Order order;
        private string orderId;
        private iTrading.Core.Kernel.OrderState orderState;
        private int quantity;
        private double stopPrice;
        private DateTime time;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public OrderStatusEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.avgFillPrice = bytes.ReadDouble();
            this.filled = bytes.ReadInt32();
            this.limitPrice = bytes.ReadDouble();
            this.orderId = bytes.ReadString();
            this.orderState = bytes.ReadOrderState();
            this.quantity = bytes.ReadInt32();
            this.stopPrice = bytes.ReadDouble();
            this.time = bytes.ReadDateTime();
            if (this.orderState.Id == OrderStateId.Initialized)
            {
                this.order = (iTrading.Core.Kernel.Order) bytes.ReadSerializable();
            }
            else
            {
                this.order = bytes.ReadOrder();
            }
        }

        internal OrderStatusEventArgs(iTrading.Core.Kernel.Order order, ErrorCode errorCode, string nativeError, Bytes bytes, int version) : base(order, errorCode, nativeError)
        {
            this.avgFillPrice = bytes.ReadDouble();
            this.filled = bytes.ReadInt32();
            this.limitPrice = bytes.ReadDouble();
            this.orderId = bytes.ReadString();
            this.orderState = bytes.ReadOrderState();
            this.quantity = bytes.ReadInt32();
            this.stopPrice = bytes.ReadDouble();
            this.time = bytes.ReadDateTime();
            this.order = order;
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="orderId"></param>
        /// <param name="avgFillPrice"></param>
        /// <param name="filled"></param>
        /// <param name="orderState"></param>
        /// <param name="limitPrice"></param>
        /// <param name="stopPrice"></param>
        /// <param name="quantity"></param>
        /// <param name="time"></param>
        public OrderStatusEventArgs(iTrading.Core.Kernel.Order order, ErrorCode errorCode, string nativeError, string orderId, double limitPrice, double stopPrice, int quantity, double avgFillPrice, int filled, iTrading.Core.Kernel.OrderState orderState, DateTime time) : base(order, errorCode, nativeError)
        {
            this.avgFillPrice = avgFillPrice;
            this.filled = filled;
            this.limitPrice = limitPrice;
            this.order = order;
            this.orderId = orderId;
            this.orderState = orderState;
            this.quantity = quantity;
            this.stopPrice = stopPrice;
            this.time = time;
            Trace.Assert(orderId.Length > 0, "Cbi.OrderStatusEventArgs.ctor: order id mustn't be empty");
            Trace.Assert(orderState != null, "Cbi.OrderStatusEventArgs.ctor: order status mustn'tb e NULL");
            if (order != null)
            {
                if (((order.OrderType.Id == OrderTypeId.Limit) || (order.OrderType.Id == OrderTypeId.StopLimit)) && (this.limitPrice == 0.0))
                {
                    Trace.WriteLine("WARNING: Cbi.OrderStatusEventArgs.ctor: '" + this.orderId + "' limit price must not be 0");
                }
                if (((order.OrderType.Id == OrderTypeId.Stop) || (order.OrderType.Id == OrderTypeId.StopLimit)) && (this.stopPrice == 0.0))
                {
                    Trace.WriteLine("WARNING: Cbi.OrderStatusEventArgs.ctor: '" + this.orderId + "' stop price must not be 0");
                }
            }
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == this.order, "Cbi.OrderStatusEventArgs.Process");
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.OrderStatusEventArgs.Process: " + this.ToString());
            }
            
            this.Order.Account.OnOrderStatusUpdate( this.Order, this);
            
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            this.Serialize(bytes, version, false);
        }

        internal void Serialize(Bytes bytes, int version, bool flat)
        {
            if (!flat)
            {
                base.Serialize(bytes, version);
            }
            bytes.Write(this.avgFillPrice);
            bytes.Write(this.filled);
            bytes.Write(this.limitPrice);
            bytes.Write(this.orderId);
            bytes.Write(this.orderState);
            bytes.Write(this.quantity);
            bytes.Write(this.stopPrice);
            bytes.Write(this.time);
            if (!flat)
            {
                if (this.orderState.Id == OrderStateId.Initialized)
                {
                    bytes.WriteSerializable(this.order);
                }
                else
                {
                    bytes.WriteOrder(this.order);
                }
            }
        }

        /// <summary>
        /// Returns the printable string value of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(new object[] { 
                "id ='", this.orderId, "' state=", this.orderState.Id, " limit=", this.limitPrice, " stop=", this.stopPrice, " quantity=", this.quantity, " filled=", this.filled, " price=", this.avgFillPrice, " error='", base.Error, 
                "' nativeError='", base.NativeError, "'"
             });
        }

        /// <summary>
        /// The average fill price of all partial fills.
        /// </summary>
        public double AvgFillPrice
        {
            get
            {
                return this.avgFillPrice;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.OrderStatusEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.OrderStatusEventArgs;
            }
        }

        /// <summary>
        /// The number of units filled.
        /// </summary>
        public int Filled
        {
            get
            {
                return this.filled;
            }
        }

        /// <summary>
        /// New limit price. The limit price may change on e.g. unsolicited order changes.
        /// These changes may be caused by e.g. the broker helpdesk.
        /// </summary>
        public double LimitPrice
        {
            get
            {
                return this.limitPrice;
            }
        }

        /// <summary>
        /// The updated order.
        /// </summary>
        public iTrading.Core.Kernel.Order Order
        {
            get
            {
                return this.order;
            }
        }

        /// <summary>
        /// The (new) id of the updated order.
        /// </summary>
        public string OrderId
        {
            get
            {
                if (this.orderId != null)
                {
                    return this.orderId;
                }
                return "";
            }
        }

        /// <summary>
        /// The actual order state.
        /// </summary>
        public iTrading.Core.Kernel.OrderState OrderState
        {
            get
            {
                return this.orderState;
            }
        }

        /// <summary>
        /// New quantity. The quantity may change on e.g. unsolicited order changes.
        /// These changes may be caused by e.g. the broker helpdesk.
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }
        }

        /// <summary>
        /// New stop price. The stop price may change on e.g. unsolicited order changes.
        /// These changes may be caused by e.g. the broker helpdesk.
        /// </summary>
        public double StopPrice
        {
            get
            {
                return this.stopPrice;
            }
        }

        /// <summary>
        /// Order update time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

