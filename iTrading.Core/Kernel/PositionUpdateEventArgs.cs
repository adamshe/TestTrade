using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an update operation of a position. An instance of this class will be passed as argument 
    /// to a <see cref="T:iTrading.Core.Kernel.PositionUpdateEventArgs" />. <seealso cref="P:iTrading.Core.Kernel.Account.Positions" />
    /// </summary>
    [ComVisible(false)]
    public class PositionUpdateEventArgs : ITradingBaseEventArgs, IComPositionUpdateEventArgs
    {
        private Account account;
        private double avgPrice;
        private iTrading.Core.Kernel.Currency currency;
        private iTrading.Core.Kernel.MarketPosition marketPosition;
        private iTrading.Core.Kernel.Operation operation;
        internal iTrading.Core.Kernel.Position position;
        private int quantity;
        private iTrading.Core.Kernel.Symbol symbol;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public PositionUpdateEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.position = null;
            this.account = bytes.ReadAccount();
            this.avgPrice = bytes.ReadDouble();
            this.currency = bytes.ReadCurrency();
            this.marketPosition = bytes.ReadMarketPosition();
            this.operation = (iTrading.Core.Kernel.Operation) bytes.ReadInt32();
            this.quantity = bytes.ReadInt32();
            this.symbol = bytes.ReadSymbol();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="operation"></param>
        /// <param name="account"></param>
        /// <param name="symbol"></param>
        /// <param name="marketPosition"></param>
        /// <param name="quantity"></param>
        /// <param name="currency"></param>
        /// <param name="avgPrice"></param>
        public PositionUpdateEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.Operation operation, Account account, iTrading.Core.Kernel.Symbol symbol, iTrading.Core.Kernel.MarketPosition marketPosition, int quantity, iTrading.Core.Kernel.Currency currency, double avgPrice) : base(currentConnection, errorCode, nativeError)
        {
            this.position = null;
            this.account = account;
            this.avgPrice = avgPrice;
            this.currency = currency;
            this.marketPosition = marketPosition;
            this.operation = operation;
            this.quantity = quantity;
            this.symbol = symbol;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.PositionUpdateEventArgs.Process");
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Request.Connection.IdPlus, ") Cbi.PositionUpdateEventArgs.Process: symbol='", this.Symbol.FullName, "' #=", this.Position.Quantity, " price=", this.Position.AvgPrice }));
            }
           
                this.Account.OnPositionUpdate( this.Account, this);
           
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
            bytes.Write(this.currency);
            bytes.Write(this.marketPosition);
            bytes.Write((int) this.operation);
            bytes.Write(this.quantity);
            bytes.WriteSymbol(this.symbol);
        }

        /// <summary>
        /// The account the positions belongs to.
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
        /// Gets <see cref="P:iTrading.Core.Kernel.PositionUpdateEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.PositionUpdateEventArgs;
            }
        }

        /// <summary>
        /// Currency.
        /// </summary>
        public iTrading.Core.Kernel.Currency Currency
        {
            get
            {
                return this.currency;
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
        /// The affected position.
        /// </summary>
        public iTrading.Core.Kernel.Position Position
        {
            get
            {
                return this.position;
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
        /// Symbol.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }
    }
}

