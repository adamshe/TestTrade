using System.Windows.Forms;
using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Controls the market depth data stream for a symbol.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("FDA42D48-AB07-4992-8984-05C4AF1642CB")]
    public class MarketDepth : StreamingRequest, IComMarketDepth, ITradingSerializable
    {
        private MarketDepthRowCollection ask;
        private MarketDepthRowCollection bid;
        internal MarketDepthBuf marketDepthBuf;
        internal MarketDepthItemEventHandler marketDepthItemDelegate;
        private Hashtable mms;
        private iTrading.Core.Kernel.Symbol symbol;
        private MarketDataType typeAsk;
        private MarketDataType typeBid;

        /// <summary>
        /// This event will be thrown when a new <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> is received.
        /// </summary>
        public event MarketDepthItemEventHandler MarketDepthItem
        {
            add
            {
                if (base.Connection.SecondaryConnectionStatusId == ConnectionStatusId.Disconnected)
                {
                    throw new TMException(ErrorCode.NotConnected);
                }
                base.Connection.SynchronizeInvoke.Invoke(new Connection.WorkerArgs1(this.MarketDepthItemAddNow), new object[] { value });
            }
            remove
            {
                base.Connection.SynchronizeInvoke.Invoke(new Connection.WorkerArgs1(this.MarketDepthItemRemoveNow), new object[] { value });
            }
        }

        internal MarketDepth(Connection currentConnection, iTrading.Core.Kernel.Symbol symbol) : base(currentConnection)
        {
            this.typeAsk = null;
            this.typeBid = null;
            this.ask = new MarketDepthRowCollection();
            this.bid = new MarketDepthRowCollection();
            this.marketDepthBuf = null;
            this.marketDepthItemDelegate = null;
            this.mms = new Hashtable();
            this.symbol = symbol;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public MarketDepth(Bytes bytes, int version) : base(bytes, version)
        {
            this.typeAsk = null;
            this.typeBid = null;
            this.ask = new MarketDepthRowCollection();
            this.bid = new MarketDepthRowCollection();
            this.marketDepthBuf = null;
            this.marketDepthItemDelegate = null;
            this.mms = new Hashtable();
            this.symbol = bytes.ReadSymbol();
        }

        /// <summary>
        /// Stop receiving <see cref="E:iTrading.Core.Kernel.MarketDepth.MarketDepthItem" /> events.
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
                    base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.FeatureNotSupported, "", "Provider does not support market depth data"));
                }
                else
                {
                    lock (this)
                    {
                        if ((base.Connection.SecondaryConnectionStatusId != ConnectionStatusId.Disconnected) && base.IsRunning)
                        {
                            if (Globals.TraceSwitch.MarketDepth)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.MarketDepth unsubscribe: " + this.Symbol.FullName);
                            }
                            if (base.connection.Options.Mode.Id == ModeTypeId.Simulation)
                            {
                                this.marketDepthBuf.CancelReplay();
                            }
                            else
                            {
                                base.Connection.marketDepth.Unsubscribe(this);
                            }
                            base.isRunning = false;
                            lock (base.Connection.MarketDepthStreams)
                            {
                                base.Connection.MarketDepthStreams.Remove(this);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketDepth.CancelNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Stop recording of market data.
        /// </summary>
        public void CancelRecorder()
        {
            lock (this)
            {
                if ((this.marketDepthBuf != null) && this.marketDepthBuf.Recording)
                {
                    if (this.symbol.connection.Options.Mode.Id == ModeTypeId.Simulation)
                    {
                        throw new TMException(ErrorCode.Panic, "Unable to start recorder in simulation mode");
                    }
                    this.marketDepthBuf.Cancel();
                    this.marketDepthBuf = null;
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
            MarketDepthBuf.Dump(this.symbol.Connection, this.symbol, fromDate, toDate, path);
        }

        /// <summary>
        /// Load data file to the recorder repository.
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            MarketDepthBuf.Load(this.symbol.connection, this.symbol, path);
        }

        private void MarketDepthItemAddNow(object arg)
        {
            try
            {
                MarketDepthItemEventHandler b = (MarketDepthItemEventHandler) arg;
                lock (this)
                {
                    this.marketDepthItemDelegate = (MarketDepthItemEventHandler) Delegate.Combine(this.marketDepthItemDelegate, b);
                    if (this.marketDepthItemDelegate.GetInvocationList().Length == 1)
                    {
                        this.Start();
                    }
                    if (this.marketDepthItemDelegate.GetInvocationList().Length > 1)
                    {
                        for (int i = 0; i < this.ask.Count; i++)
                        {
                            MarketDepthRow row = this.ask[i];
                            b(this, new MarketDepthEventArgs(this, ErrorCode.NoError, "", this.symbol, i, row.marketMaker, Operation.Insert, base.Connection.MarketDataTypes[MarketDataTypeId.Ask], row.price, row.volume, row.time));
                        }
                        for (int j = 0; j < this.bid.Count; j++)
                        {
                            MarketDepthRow row2 = this.bid[j];
                            b(this, new MarketDepthEventArgs(this, ErrorCode.NoError, "", this.symbol, j, row2.marketMaker, Operation.Insert, base.Connection.MarketDataTypes[MarketDataTypeId.Bid], row2.price, row2.volume, row2.time));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketDepth.MarketDepthItemAddNow: exception caught: " + exception.Message));
            }
        }

        private void MarketDepthItemRemoveNow(object arg)
        {
            try
            {
                MarketDepthItemEventHandler handler = (MarketDepthItemEventHandler) arg;
                lock (this)
                {
                    this.marketDepthItemDelegate = (MarketDepthItemEventHandler) Delegate.Remove(this.marketDepthItemDelegate, handler);
                    if (this.marketDepthItemDelegate == null)
                    {
                        this.Cancel();
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketDepth.MarketDepthItemRemoveNow: exception caught: " + exception.Message));
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
        /// Starts the request. <see cref="E:iTrading.Core.Kernel.MarketDepth.MarketDepthItem" /> events will be thrown, when data is received.
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
                if ((base.Connection.SecondaryConnectionStatusId != ConnectionStatusId.Disconnected) && !base.IsRunning)
                {
                    if (((base.Connection.marketData == null) || (base.Connection.FeatureTypes[FeatureTypeId.MarketDepth] == null)) || (base.Connection.FeatureTypes[FeatureTypeId.MaxMarketDepthStreams] == null))
                    {
                        base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.FeatureNotSupported, "", "Provider does not support market depth data"));
                    }
                    else
                    {
                        lock (this)
                        {
                            lock (base.Connection.MarketDepthStreams)
                            {
                                int num = (int) base.Connection.FeatureTypes[FeatureTypeId.MaxMarketDepthStreams].Value;
                                if (base.Connection.marketDepthStreams.Count >= num)
                                {
                                    base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.TooManyMarketDepthStreams, "", "Max. # of market depth data streams (" + num + ") for this connection is exceeded."));
                                    return;
                                }
                                base.Connection.MarketDepthStreams.Add(this);
                            }
                            if (Globals.TraceSwitch.MarketDepth)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.MarketDepth subscribe: " + this.Symbol.FullName);
                            }
                            if (base.connection.Options.Mode.Id != ModeTypeId.Simulation)
                            {
                                goto Label_01DE;
                            }
                            this.marketDepthBuf = new MarketDepthBuf(this.symbol, base.connection.Now);
                            this.marketDepthBuf.StartReplay();
                            goto Label_0270;
                        Label_0186:
                            base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, 0, "", Operation.Delete, base.connection.MarketDataTypes[MarketDataTypeId.Ask], 0.0, 0, base.connection.Now));
                        Label_01DE:
                            if (this.ask.Count > 0)
                            {
                                goto Label_0186;
                            }
                            while (this.bid.Count > 0)
                            {
                                base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, 0, "", Operation.Delete, base.connection.MarketDataTypes[MarketDataTypeId.Bid], 0.0, 0, base.connection.Now));
                            }
                            this.mms.Clear();
                            base.Connection.marketDepth.Subscribe(this);
                        Label_0270:
                            base.isRunning = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.MarketDepth.StartNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Start market data recorder.
        /// </summary>
        public void StartRecorder()
        {
            lock (this)
            {
                if (this.marketDepthBuf == null)
                {
                    if (this.symbol.connection.Options.Mode.Id == ModeTypeId.Simulation)
                    {
                        throw new TMException(ErrorCode.Panic, "Unable to start recorder in simulation mode");
                    }
                    this.marketDepthBuf = new MarketDepthBuf(this.symbol, this.symbol.connection.Now);
                    this.marketDepthBuf.Start();
                }
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="mmId"></param>
        /// <param name="dataType"></param>
        /// <param name="askPrice"></param>
        /// <param name="askSize"></param>
        /// <param name="bidPrice"></param>
        /// <param name="bidSize"></param>
        /// <param name="time"></param>
        public void Update(string mmId, MarketDataType dataType, double askPrice, int askSize, double bidPrice, int bidSize, DateTime time)
        {
            bool flag = (dataType == null) || (dataType.Id == MarketDataTypeId.Ask);
            bool flag2 = (dataType == null) || (dataType.Id == MarketDataTypeId.Bid);
            if (!flag & !flag2)
            {
                Trace.Assert(false, "Cbi.MarketDepth.Update: mmdId='" + mmId + "'");
            }
            if (this.typeAsk == null)
            {
                this.typeAsk = base.connection.MarketDataTypes[MarketDataTypeId.Ask];
                Trace.Assert(this.typeAsk != null, "Cbi.MarketDepth.Update1");
            }
            if (this.typeBid == null)
            {
                this.typeBid = base.connection.MarketDataTypes[MarketDataTypeId.Bid];
                Trace.Assert(this.typeBid != null, "Cbi.MarketDepth.Update2");
            }
            MM mm = (MM) this.mms[mmId];
            if (mm == null)
            {
                mm = new MM(mmId);
                this.mms.Add(mmId, mm);
            }
            if ((flag && ((askPrice == 0.0) || (askSize == 0))) || (flag2 && ((bidPrice == 0.0) || (bidSize == 0))))
            {
                if (flag && (mm.askPos >= 0))
                {
                    base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.askPos, mmId, Operation.Delete, this.typeAsk, 0.0, 0, time));
                    for (int i = mm.askPos; i < this.symbol.MarketDepth.Ask.Count; i++)
                    {
                        ((MM) this.symbol.MarketDepth.Ask[i].AdapterLink).askPos = i;
                    }
                    mm.Init(true);
                }
                if (flag2 && (mm.bidPos >= 0))
                {
                    base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.bidPos, mmId, Operation.Delete, this.typeBid, 0.0, 0, time));
                    for (int j = mm.bidPos; j < this.symbol.MarketDepth.Bid.Count; j++)
                    {
                        ((MM) this.symbol.MarketDepth.Bid[j].AdapterLink).bidPos = j;
                    }
                    mm.Init(false);
                }
            }
            else
            {
                if ((!flag || (askPrice == -1.0)) || (mm.ask == askPrice))
                {
                    if (flag && (mm.askSize != askSize))
                    {
                        mm.askSize = askSize;
                        if (mm.askPos >= 0)
                        {
                            base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.askPos, mmId, Operation.Update, this.typeAsk, mm.ask, mm.askSize, time));
                        }
                    }
                }
                else
                {
                    if (mm.askPos >= 0)
                    {
                        base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.askPos, mmId, Operation.Delete, this.typeAsk, 0.0, 0, time));
                        for (int m = mm.askPos; m < this.symbol.MarketDepth.Ask.Count; m++)
                        {
                            ((MM) this.symbol.MarketDepth.Ask[m].AdapterLink).askPos = m;
                        }
                    }
                    int num4 = 0;
                    while (num4 < this.symbol.MarketDepth.Ask.Count)
                    {
                        MarketDepthRow row = this.symbol.MarketDepth.Ask[num4];
                        if (row.Price >= askPrice)
                        {
                            break;
                        }
                        num4++;
                    }
                    mm.ask = askPrice;
                    mm.askPos = num4;
                    if (askSize != -1)
                    {
                        mm.askSize = askSize;
                    }
                    base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.askPos, mmId, Operation.Insert, this.typeAsk, mm.ask, mm.askSize, time));
                    this.symbol.MarketDepth.Ask[mm.askPos].AdapterLink = mm;
                    for (int k = mm.askPos + 1; k < this.symbol.MarketDepth.Ask.Count; k++)
                    {
                        ((MM) this.symbol.MarketDepth.Ask[k].AdapterLink).askPos = k;
                    }
                }
                if ((!flag2 || (bidPrice == -1.0)) || (mm.bid == bidPrice))
                {
                    if (flag2 && (mm.bidSize != bidSize))
                    {
                        mm.bidSize = bidSize;
                        if (mm.bidPos >= 0)
                        {
                            base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.bidPos, mmId, Operation.Update, this.typeBid, mm.bid, mm.bidSize, time));
                        }
                    }
                }
                else
                {
                    if (mm.bidPos >= 0)
                    {
                        base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.bidPos, mmId, Operation.Delete, this.typeBid, 0.0, 0, time));
                        for (int num6 = mm.bidPos; num6 < this.symbol.MarketDepth.Bid.Count; num6++)
                        {
                            ((MM) this.symbol.MarketDepth.Bid[num6].AdapterLink).bidPos = num6;
                        }
                    }
                    int num7 = 0;
                    while (num7 < this.symbol.MarketDepth.Bid.Count)
                    {
                        MarketDepthRow row2 = this.symbol.MarketDepth.Bid[num7];
                        if (row2.Price <= bidPrice)
                        {
                            break;
                        }
                        num7++;
                    }
                    mm.bid = bidPrice;
                    mm.bidPos = num7;
                    if (bidSize != -1)
                    {
                        mm.bidSize = bidSize;
                    }
                    base.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, mm.bidPos, mmId, Operation.Insert, this.typeBid, mm.bid, mm.bidSize, time));
                    this.symbol.MarketDepth.Bid[mm.bidPos].AdapterLink = mm;
                    for (int n = mm.bidPos + 1; n < this.symbol.MarketDepth.Bid.Count; n++)
                    {
                        ((MM) this.symbol.MarketDepth.Bid[n].AdapterLink).bidPos = n;
                    }
                }
            }
        }

        /// <summary>
        /// Collection of ask side rows. Sorted by price.
        /// </summary>
        public MarketDepthRowCollection Ask
        {
            get
            {
                return this.ask;
            }
        }

        /// <summary>
        /// Collection of bid side rows. Sorted by Price.
        /// </summary>
        public MarketDepthRowCollection Bid
        {
            get
            {
                return this.bid;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.MarketDepth.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.MarketDepth;
            }
        }

        /// <summary>
        /// Symbol where market depth data is requested for.
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

        private class MM
        {
            internal double ask;
            internal int askPos;
            internal int askSize;
            internal double bid;
            internal int bidPos;
            internal int bidSize;
            internal string mmId;

            internal MM(string mmId)
            {
                this.mmId = mmId;
                this.Init(true);
                this.Init(false);
            }

            internal void Init(bool isAsk)
            {
                if (isAsk)
                {
                    this.ask = 0.0;
                    this.askSize = 0;
                    this.askPos = -1;
                }
                else
                {
                    this.bid = 0.0;
                    this.bidSize = 0;
                    this.bidPos = -1;
                }
            }
        }
    }
}

