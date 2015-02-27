using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Reperesent an market depth item. <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> are received for the bid/ask side.
    /// This object will be passed as argument to the <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> event handler.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("86FF7CFE-C80B-46b4-BFE2-A1FC55FF61B4")]
    public class MarketDepthEventArgs : ITradingBaseEventArgs, ITradingSerializable, IComMarketDepthEventArgs
    {
        internal bool initOnly;
        private iTrading.Core.Kernel.MarketDataType marketDataType;
        private string marketMaker;
        private iTrading.Core.Kernel.Operation operation;
        private int position;
        private double price;
        private iTrading.Core.Kernel.Symbol symbol;
        private DateTime time;
        private int volume;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MarketDepthEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.initOnly = false;
            this.marketDataType = bytes.ReadMarketData();
            this.marketMaker = bytes.ReadString();
            this.operation = (iTrading.Core.Kernel.Operation) bytes.ReadInt32();
            this.position = bytes.ReadInt32();
            this.price = bytes.ReadDouble();
            this.time = bytes.ReadDateTime();
            this.symbol = bytes.ReadSymbol();
            this.volume = bytes.ReadInt32();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="marketDepthData"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="operation"></param>
        /// <param name="marketMaker"></param>
        /// <param name="price"></param>
        /// <param name="symbol"></param>
        /// <param name="position"></param>
        /// <param name="marketDataType"></param>
        /// <param name="volume"></param>
        /// <param name="time"></param>
        public MarketDepthEventArgs(MarketDepth marketDepthData, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.Symbol symbol, int position, string marketMaker, iTrading.Core.Kernel.Operation operation, iTrading.Core.Kernel.MarketDataType marketDataType, double price, int volume, DateTime time) : base(marketDepthData, errorCode, nativeError)
        {
            this.initOnly = false;
            this.marketDataType = marketDataType;
            this.marketMaker = marketMaker;
            this.operation = operation;
            this.price = price;
            this.position = position;
            this.symbol = symbol;
            this.volume = volume;
            this.time = time;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == this.symbol.MarketDepth, "Cbi.MarketDepthEventArgs.Process");
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Request.Connection.IdPlus, ") Cbi.MarketDepthEventArgs.Process: ", this.Symbol.FullName, " time='", this.Time, "'" }));
            }
            if (((MarketDepth) base.Request).marketDepthItemDelegate != null)
            {
                ((MarketDepth) base.Request).marketDepthItemDelegate(base.Request, this);
            }
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.marketDataType);
            bytes.Write(this.marketMaker);
            bytes.Write((int) this.operation);
            bytes.Write(this.position);
            bytes.Write(this.price);
            bytes.Write(this.time);
            bytes.WriteSymbol(this.symbol);
            bytes.Write(this.volume);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MarketDepthEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MarketDepthEventArgs;
            }
        }

        /// <summary>
        /// Side of market depth item.
        /// </summary>
        public iTrading.Core.Kernel.MarketDataType MarketDataType
        {
            get
            {
                return this.marketDataType;
            }
        }

        /// <summary>
        /// Market maker.
        /// </summary>
        public string MarketMaker
        {
            get
            {
                return this.marketMaker;
            }
        }

        /// <summary>
        /// Type of actual market depth item.
        /// </summary>
        public iTrading.Core.Kernel.Operation Operation
        {
            get
            {
                return this.operation;
            }
        }

        /// <summary>
        /// Position of market depth item. Can be used to e.g. display the <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> in a datagrid.
        /// </summary>
        public int Position
        {
            get
            {
                return this.position;
            }
        }

        /// <summary>
        /// Price of market depth item.
        /// </summary>
        public double Price
        {
            get
            {
                return this.price;
            }
        }

        /// <summary>
        /// The affected symbol.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        /// <summary>
        /// Time value.
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

        /// <summary>
        /// Volume of market depth item.
        /// </summary>
        public int Volume
        {
            get
            {
                return this.volume;
            }
        }
    }
}

