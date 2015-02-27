using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an execution.
    /// </summary>
    [Guid("D58AE832-3852-4e91-B9DF-90C02DA52F06"), ClassInterface(ClassInterfaceType.None)]
    public class Execution : IComExecution, ITradingSerializable
    {
        private Account account;
        private double avgPrice;
        private string id;
        private iTrading.Core.Kernel.MarketPosition marketPosition;
        private string orderId;
        private int quantity;
        private iTrading.Core.Kernel.Symbol symbol;
        private DateTime time;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public Execution(Bytes bytes, int version)
        {
            this.account = bytes.ReadAccount();
            this.avgPrice = bytes.ReadDouble();
            this.id = bytes.ReadString();
            this.orderId = bytes.ReadString();
            this.marketPosition = bytes.ReadMarketPosition();
            this.quantity = bytes.ReadInt32();
            this.symbol = bytes.ReadSymbol();
            this.time = bytes.ReadDateTime();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="symbol"></param>
        /// <param name="time"></param>
        /// <param name="marketPosition"></param>
        /// <param name="orderId"></param>
        /// <param name="quantity"></param>
        /// <param name="avgPrice"></param>
        public Execution(string id, Account account, iTrading.Core.Kernel.Symbol symbol, DateTime time, iTrading.Core.Kernel.MarketPosition marketPosition, string orderId, int quantity, double avgPrice)
        {
            this.account = account;
            this.avgPrice = avgPrice;
            this.id = id;
            this.orderId = orderId;
            this.marketPosition = marketPosition;
            this.quantity = quantity;
            this.symbol = symbol;
            this.time = time;
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public void Serialize(Bytes bytes, int version)
        {
            bytes.Write(this.account);
            bytes.Write(this.avgPrice);
            bytes.Write(this.id);
            bytes.Write(this.orderId);
            bytes.Write(this.marketPosition);
            bytes.Write(this.quantity);
            bytes.WriteSymbol(this.symbol);
            bytes.Write(this.time);
        }

        internal void Update(ExecutionUpdateEventArgs eventArgs)
        {
            this.avgPrice = eventArgs.AvgPrice;
            this.marketPosition = eventArgs.MarketPosition;
            this.quantity = eventArgs.Quantity;
            this.time = eventArgs.Time;
        }

        /// <summary>
        /// Account where the execution is belonging to.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
        }

        /// <summary>
        /// Average cost per unit.
        /// </summary>
        public double AvgPrice
        {
            get
            {
                return this.avgPrice;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.Execution.ClassId" /> of current object.
        /// </summary>
        public iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.Execution;
            }
        }

        /// <summary>
        /// Identifies the execution. If the brokers native execution id is a numeric value, it will be converted 
        /// to a string accordingly.
        /// </summary>
        public string Id
        {
            get
            {
                if (this.id != null)
                {
                    return this.id;
                }
                return "";
            }
        }

        /// <summary>
        /// Identifies the execution side.
        /// </summary>
        public iTrading.Core.Kernel.MarketPosition MarketPosition
        {
            get
            {
                return this.marketPosition;
            }
        }

        /// <summary>
        /// Identifies the associated order.
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
        /// Number of units which have been executed.
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }
        }

        /// <summary>
        /// Order has been executed on this symbol.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        /// <summary>
        /// Execution time.
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
        public int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

