using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an update operation of an execution. An instance of this class will be passed as argument 
    /// to a <see cref="T:iTrading.Core.Kernel.ExecutionUpdateEventHandler" />. <seealso cref="P:iTrading.Core.Kernel.Account.Positions" />
    /// </summary>
    [ComVisible(false)]
    public class ExecutionUpdateEventArgs : ITradingBaseEventArgs, IComExecutionEventArgs, ITradingSerializable
    {
        private Account account;
        private double avgPrice;
        internal iTrading.Core.Kernel.Execution execution;
        private string executionId;
        private iTrading.Core.Kernel.MarketPosition marketPosition;
        private iTrading.Core.Kernel.Operation operation;
        private string orderId;
        private int quantity;
        private iTrading.Core.Kernel.Symbol symbol;
        private DateTime time;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public ExecutionUpdateEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.execution = null;
            this.account = bytes.ReadAccount();
            this.avgPrice = bytes.ReadDouble();
            this.executionId = bytes.ReadString();
            this.marketPosition = bytes.ReadMarketPosition();
            this.operation = (iTrading.Core.Kernel.Operation) bytes.ReadInt32();
            this.orderId = bytes.ReadString();
            this.quantity = bytes.ReadInt32();
            this.symbol = bytes.ReadSymbol();
            this.time = bytes.ReadDateTime();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="executionId"></param>
        /// <param name="account"></param>
        /// <param name="symbol"></param>
        /// <param name="time"></param>
        /// <param name="marketPosition"></param>
        /// <param name="orderId"></param>
        /// <param name="operation"></param>
        /// <param name="quantity"></param>
        /// <param name="avgPrice"></param>
        public ExecutionUpdateEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.Operation operation, string executionId, Account account, iTrading.Core.Kernel.Symbol symbol, DateTime time, iTrading.Core.Kernel.MarketPosition marketPosition, string orderId, int quantity, double avgPrice) : base(currentConnection, errorCode, nativeError)
        {
            this.execution = null;
            this.account = account;
            this.avgPrice = avgPrice;
            this.executionId = executionId;
            this.marketPosition = marketPosition;
            this.operation = operation;
            this.orderId = orderId;
            this.quantity = quantity;
            this.symbol = symbol;
            this.time = time;
            Trace.Assert(operation != iTrading.Core.Kernel.Operation.Delete, "Cbi.ExecutionUpdateEventArgs.ctor: operation (" + operation + ") not supported");
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.ExecutionUpdateEventArgs.Process");
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Request.Connection.IdPlus, ") Cbi.ExecutionEventArgs.Process: id='", this.executionId, "' symbol='", this.symbol.FullName, "' orderid='", this.orderId, "' #=", this.quantity, " price=", this.avgPrice }));
            }
           
                this.account.OnExecutionUpdate(this.account, this);           
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.account);
            bytes.Write(this.avgPrice);
            bytes.Write(this.executionId);
            bytes.Write(this.marketPosition);
            bytes.Write((int) this.operation);
            bytes.Write(this.orderId);
            bytes.Write(this.quantity);
            bytes.WriteSymbol(this.symbol);
            bytes.Write(this.time);
        }

        /// <summary>
        /// The account the execution belongs to.
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
        /// Gets <see cref="P:iTrading.Core.Kernel.ExecutionUpdateEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.ExecutionUpdateEventArgs;
            }
        }

        /// <summary>
        /// Execution.
        /// </summary>
        public iTrading.Core.Kernel.Execution Execution
        {
            get
            {
                return this.execution;
            }
        }

        /// <summary>
        /// Execution id.
        /// </summary>
        public string ExecutionId
        {
            get
            {
                return this.executionId;
            }
        }

        /// <summary>
        /// Type of position item.
        /// </summary>
        public iTrading.Core.Kernel.MarketPosition MarketPosition
        {
            get
            {
                return this.marketPosition;
            }
        }

        /// <summary>
        /// The update operation.
        /// </summary>
        public iTrading.Core.Kernel.Operation Operation
        {
            get
            {
                return this.operation;
            }
        }

        /// <summary>
        /// Id of order causing the execution.
        /// </summary>
        public string OrderId
        {
            get
            {
                return this.orderId;
            }
        }

        /// <summary>
        /// Number of shares/units/contracts.
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }
        }

        /// <summary>
        /// Execution symbol.
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
        public override int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

