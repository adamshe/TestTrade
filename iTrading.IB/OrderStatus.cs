namespace iTrading.IB
{
    using System;

    internal class OrderStatus : ICloneable
    {
        internal double avgFillPrice;
        internal int connectionId;
        internal int filled;
        internal double lastFillPrice;
        internal int orderId;
        internal string orderStatus;
        internal int parentId = 0;
        internal string permId = "";
        internal int remaining;

        public object Clone()
        {
            OrderStatus status = new OrderStatus();
            status.avgFillPrice = this.avgFillPrice;
            status.connectionId = this.connectionId;
            status.filled = this.filled;
            status.lastFillPrice = this.lastFillPrice;
            status.orderId = this.orderId;
            status.orderStatus = this.orderStatus;
            status.parentId = this.parentId;
            status.permId = this.permId;
            status.remaining = this.remaining;
            return status;
        }
    }
}

