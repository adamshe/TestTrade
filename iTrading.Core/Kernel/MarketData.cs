using System.Windows.Forms;
using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Controls the market data stream for a symbol.
    /// </summary>
    [Guid("2BC3F3A1-682D-4cc1-ADE9-8A5D6BA15912"), ClassInterface(ClassInterfaceType.None)]
    public class MarketData : StreamingRequest, ITradingSerializable, IComMarketData
    {
        internal MarketDataEventArgs lastAsk;
        internal MarketDataEventArgs lastBid;
        internal MarketDataEventArgs lastDailyHigh;
        internal MarketDataEventArgs lastDailyLow;
        internal MarketDataEventArgs lastDailyVolume;
        internal MarketDataEventArgs lastLast;
        internal MarketDataEventArgs lastLastClose;
        internal MarketDataEventArgs lastOpening;
        internal MarketDataBuf marketDataBuf;
        internal MarketDataItemEventHandler marketDataItemEventHandler;
        internal int nextEventId;
        private iTrading.Core.Kernel.Symbol symbol;

        /// <summary>
        /// This event will be thrown when a new <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> is received.
        /// </summary>
        public event MarketDataItemEventHandler MarketDataItem
        {
            add
            {
                if (base.Connection.SecondaryConnectionStatusId == ConnectionStatusId.Disconnected)
                {
                    throw new TMException(ErrorCode.NotConnected);
                }
                base.Connection.SynchronizeInvoke.Invoke(new Connection.WorkerArgs1(this.MarketDataItemAddNow), new object[] { value });
            }
            remove
            {
                base.Connection.SynchronizeInvoke.Invoke(new Connection.WorkerArgs1(this.MarketDataItemRemoveNow), new object[] { value });
            }
        }

        internal MarketData(Connection currentConnection, iTrading.Core.Kernel.Symbol symbol) : base(currentConnection)
        {
            this.nextEventId = 0;
            this.lastAsk = null;
            this.lastBid = null;
            this.lastDailyHigh = null;
            this.lastDailyLow = null;
            this.lastDailyVolume = null;
            this.lastLast = null;
            this.lastLastClose = null;
            this.lastOpening = null;
            this.marketDataBuf = null;
            this.symbol = symbol;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MarketData(Bytes bytes, int version) : base(bytes, version)
        {
            this.nextEventId = 0;
            this.lastAsk = null;
            this.lastBid = null;
            this.lastDailyHigh = null;
            this.lastDailyLow = null;
            this.lastDailyVolume = null;
            this.lastLast = null;
            this.lastLastClose = null;
            this.lastOpening = null;
            this.marketDataBuf = null;
            this.symbol = bytes.ReadSymbol();
        }

        /// <summary>
        /// Stop receiving <see cref="E:iTrading.Core.Kernel.MarketData.MarketDataItem" /> events.
        /// </summary>
        public override void Cancel()
        {
            base.Connection.SynchronizeInvoke.Invoke(new MethodInvoker(this.CancelNow), null);
        }

        private void CancelNow()
        {
            try
            {
                if (base.Connection.marketData == null)
                {
                    base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.FeatureNotSupported, "", "Provider does not support market data"));
                }
                else
                {
                    lock (this)
                    {
                        if ((base.Connection.SecondaryConnectionStatusId != ConnectionStatusId.Disconnected) && base.IsRunning)
                        {
                            if (Globals.TraceSwitch.MarketData)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.MarketData unsubscribe: " + this.Symbol.FullName);
                            }
                            if (base.connection.Options.Mode.Id == ModeTypeId.Simulation)
                            {
                                this.marketDataBuf.CancelReplay();
                            }
                            else
                            {
                                base.Connection.marketData.Unsubscribe(this);
                            }
                            base.isRunning = false;
                            lock (base.Connection.MarketDataStreams)
                            {
                                base.Connection.MarketDataStreams.Remove(this);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketData.CancelNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Stop recording of market data.
        /// </summary>
        public void CancelRecorder()
        {
            lock (this)
            {
                if ((this.marketDataBuf != null) && this.marketDataBuf.Recording)
                {
                    if (this.symbol.connection.Options.Mode.Id == ModeTypeId.Simulation)
                    {
                        throw new TMException(ErrorCode.Panic, "Unable to stop recorder in simulation mode");
                    }
                    this.marketDataBuf.Cancel();
                    this.marketDataBuf = null;
                }
            }
        }

        /// <summary>
        /// Dump recorded data to a file.
        /// </summary>
        /// <param name="fromDate">Start date.</param>
        /// <param name="toDate">End date.</param>
        /// <param name="path">File path. An existing file will be overriden.</param>
        public void Dump(DateTime fromDate, DateTime toDate, string path)
        {
            MarketDataBuf.Dump(this.symbol.Connection, this.symbol, fromDate, toDate, path);
        }

        /// <summary>
        /// Load data file to the recorder repository.
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            MarketDataBuf.Load(this.symbol.connection, this.symbol, path);
        }

        private void MarketDataItemAddNow(object arg)
        {
            try
            {
                MarketDataItemEventHandler b = (MarketDataItemEventHandler) arg;
                lock (this)
                {
                    this.marketDataItemEventHandler = (MarketDataItemEventHandler) Delegate.Combine(this.marketDataItemEventHandler, b);
                    if (this.marketDataItemEventHandler.GetInvocationList().Length == 1)
                    {
                        this.Start();
                    }
                }
                if (this.marketDataItemEventHandler.GetInvocationList().Length > 1)
                {
                    if (this.lastAsk != null)
                    {
                        b(this, this.lastAsk);
                    }
                    if (this.lastBid != null)
                    {
                        b(this, this.lastBid);
                    }
                    if (this.lastDailyHigh != null)
                    {
                        b(this, this.lastDailyHigh);
                    }
                    if (this.lastDailyLow != null)
                    {
                        b(this, this.lastDailyLow);
                    }
                    if (this.lastDailyVolume != null)
                    {
                        b(this, this.lastDailyVolume);
                    }
                    if (this.lastLast != null)
                    {
                        b(this, this.lastLast);
                    }
                    if (this.lastLastClose != null)
                    {
                        b(this, this.lastLastClose);
                    }
                    if (this.lastOpening != null)
                    {
                        b(this, this.lastOpening);
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketData.MarketDataItemAddNow: exception caught: " + exception.Message));
            }
        }

        private void MarketDataItemRemoveNow(object arg)
        {
            try
            {
                MarketDataItemEventHandler handler = (MarketDataItemEventHandler) arg;
                lock (this)
                {
                    this.marketDataItemEventHandler = (MarketDataItemEventHandler) Delegate.Remove(this.marketDataItemEventHandler, handler);
                    if (this.marketDataItemEventHandler == null)
                    {
                        this.Cancel();
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketData.MarketDataItemRemoveNow: exception caught: " + exception.Message));
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
            bytes.WriteSymbol(this.symbol);
        }

        /// <summary>
        /// Starts the request. <see cref="E:iTrading.Core.Kernel.MarketData.MarketDataItem" /> events will be thrown, when data is received.
        /// Please note: The request is started automatically on registering the first delegate.
        /// </summary>
        public void Start()
        {
            base.Connection.SynchronizeInvoke.Invoke(new MethodInvoker(this.StartNow), null);
        }

        private void StartNow()
        {
            try
            {
                if (((base.Connection.marketData == null) || (base.Connection.FeatureTypes[FeatureTypeId.MarketData] == null)) || (base.Connection.FeatureTypes[FeatureTypeId.MaxMarketDataStreams] == null))
                {
                    base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.FeatureNotSupported, "", "Provider does not support market data"));
                }
                else
                {
                    lock (this)
                    {
                        if ((base.Connection.SecondaryConnectionStatusId != ConnectionStatusId.Disconnected) && !base.IsRunning)
                        {
                            lock (base.Connection.MarketDataStreams)
                            {
                                int num = (int) base.Connection.FeatureTypes[FeatureTypeId.MaxMarketDataStreams].Value;
                                if (base.Connection.marketDataStreams.Count >= num)
                                {
                                    base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.TooManyMarketDataStreams, "", "Max. # of market data streams (" + num + ") for this connection is exceeded."));
                                    return;
                                }
                                base.Connection.MarketDataStreams.Add(this);
                            }
                            this.lastAsk = null;
                            this.lastBid = null;
                            this.lastDailyHigh = null;
                            this.lastDailyLow = null;
                            this.lastDailyVolume = null;
                            this.lastLast = null;
                            this.lastLastClose = null;
                            this.lastOpening = null;
                            if (Globals.TraceSwitch.MarketData)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.MarketData subscribe: " + this.Symbol.FullName);
                            }
                            if (base.connection.Options.Mode.Id == ModeTypeId.Simulation)
                            {
                                this.marketDataBuf = new MarketDataBuf(this.symbol, base.connection.Now);
                                this.marketDataBuf.StartReplay();
                            }
                            else
                            {
                                base.Connection.marketData.Subscribe(this);
                            }
                            base.isRunning = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketData.StartNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Start market data recorder.
        /// </summary>
        public void StartRecorder()
        {
            lock (this)
            {
                if (this.marketDataBuf == null)
                {
                    if (this.symbol.connection.Options.Mode.Id == ModeTypeId.Simulation)
                    {
                        throw new TMException(ErrorCode.Panic, "Unable to start recorder in simulation mode");
                    }
                    this.marketDataBuf = new MarketDataBuf(this.symbol, this.symbol.connection.Now);
                    this.marketDataBuf.Start();
                }
            }
        }

        /// <summary>
        /// Get last ask change event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no ask change event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs Ask
        {
            get
            {
                return this.lastAsk;
            }
        }

        /// <summary>
        /// Get last bid change event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no bid change event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs Bid
        {
            get
            {
                return this.lastBid;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MarketData.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MarketData;
            }
        }

        /// <summary>
        /// Get last daily-high event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-high event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs DailyHigh
        {
            get
            {
                return this.lastDailyHigh;
            }
        }

        /// <summary>
        /// Get last daily-low event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-low event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs DailyLow
        {
            get
            {
                return this.lastDailyLow;
            }
        }

        /// <summary>
        /// Get last daily-volume event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-volume event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs DailyVolume
        {
            get
            {
                return this.lastDailyVolume;
            }
        }

        /// <summary>
        /// Get last price change event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no price change event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs Last
        {
            get
            {
                return this.lastLast;
            }
        }

        /// <summary>
        /// Get last previous day close event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no previous day close event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs LastClose
        {
            get
            {
                return this.lastLastClose;
            }
        }

        /// <summary>
        /// Get daily opening event. 
        /// NULL, if <see cref="P:iTrading.Core.Kernel.StreamingRequest.IsRunning" /> is FALSE, or no daily-opening event has ben seen yet.
        /// </summary>
        public MarketDataEventArgs Opening
        {
            get
            {
                return this.lastOpening;
            }
        }

        /// <summary>
        /// Symbol where market data is requested for.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
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

