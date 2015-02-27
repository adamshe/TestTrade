using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// This object will be passed as argument to the <see cref="E:iTrading.Core.Kernel.MarketData.MarketDataItem" /> event handler.
    /// It holds tick data.
    /// </summary>
    [Guid("2D5AF20B-AB1F-4b4e-B283-F405C77E07AB"), ClassInterface(ClassInterfaceType.None)]
    public class MarketDataEventArgs : ITradingBaseEventArgs, ITradingSerializable, IComMarketDataEventArgs
    {
        private int eventId;
        internal bool initOnly;
        private iTrading.Core.Kernel.MarketDataType marketDataType;
        private double price;
        private iTrading.Core.Kernel.Symbol symbol;
        private DateTime time;
        private int volume;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MarketDataEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.initOnly = false;
            this.marketDataType = bytes.ReadMarketData();
            this.price = bytes.ReadDouble();
            this.time = bytes.ReadDateTime();
            this.symbol = bytes.ReadSymbol();
            this.volume = bytes.ReadInt32();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="marketData"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="symbol"></param>
        /// <param name="marketDataType"></param>
        /// <param name="price"></param>
        /// <param name="volume"></param>
        /// <param name="time"></param>
        public MarketDataEventArgs(MarketData marketData, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.Symbol symbol, iTrading.Core.Kernel.MarketDataType marketDataType, double price, int volume, DateTime time) : base(marketData, errorCode, nativeError)
        {
            this.initOnly = false;
            this.eventId = ++marketData.nextEventId;
            this.marketDataType = marketDataType;
            this.price = price;
            this.volume = volume;
            this.symbol = symbol;
            this.time = time;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == this.symbol.MarketData, "Cbi.MarketDataEventArgs.Process");
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.MarketDataEventArgs: " + this.Symbol.FullName);
            }
            if (((MarketData) base.Request).marketDataItemEventHandler != null)
            {
                ((MarketData) base.Request).marketDataItemEventHandler(base.Request, this);
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
            bytes.Write(this.price);
            bytes.Write(this.time);
            bytes.WriteSymbol(this.symbol);
            bytes.Write(this.volume);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MarketDataEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MarketDataEventArgs;
            }
        }

        /// <summary>
        /// For internal use.
        /// </summary>
        public int EventId
        {
            get
            {
                return this.eventId;
            }
        }

        /// <summary>
        /// Type of actual data item.
        /// </summary>
        public iTrading.Core.Kernel.MarketDataType MarketDataType
        {
            get
            {
                return this.marketDataType;
            }
        }

        /// <summary>
        /// Price of actual data item.
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
        /// Time of actual data item.
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
        /// Volume of actual data item.
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

