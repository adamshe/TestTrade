namespace iTrading.Track
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Timers;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;
    using System.Windows.Forms;
    internal class Adapter : IAdapter, IMarketData, IMarketDepth, IQuotes, IOrder
    {
        internal BrokerErrorMessageRequest brokerErrorMessageRequest = null;
        internal BrokerMessageRequest brokerMessageRequest = null;
        internal BrokerTableMessageRequest brokerTableMessageRequest = null;
        internal System.Timers.Timer checkStatusTimer = null;
        internal Connection connection = null;
        internal CultureInfo cultureInfo = new CultureInfo("en-US");
        private int dllHandle = 0;
        private ErrorReportRequest errorReportRequest = null;
        internal ArrayList marketData2Recover = new ArrayList();
        internal ArrayList marketDepth2Recover = new ArrayList();
        internal Hashtable newsIds = new Hashtable();
        internal NewsRequest newsRequest = null;
        internal int nextRqn = 0;
        private Hashtable ocaOrdersByGroup = new Hashtable();
        private ByteReader reader = null;
        internal Thread receiveThread = null;
        internal Regex regExDigits = new Regex("[0-9]");
        internal Regex regExProvider = new Regex(@"^\ *\([A-Za-z\ ]*\)\ *");
        internal Regex regExSpaces = new Regex(@"\ +");
        internal ArrayList requests = new ArrayList();
        internal ArrayList startupRequests = new ArrayList();
        internal object[] syncIntradayUpdate = new object[0];
        internal object[] syncThreadStart = new object[0];
        internal DateTime timeLastRequest = Globals.MaxDate;
        internal TimerRequest timerRequest = null;
        internal string workingHost = "";

        internal Adapter(Connection connection)
        {
            this.connection = connection;
            this.reader = new ByteReader(this);
            Api.adapter = this;
        }

        public void Cancel(Order order)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Cancel: " + order.ToString());
            }
            if (order.OcaGroup.Length > 0)
            {
                ArrayList list = new ArrayList();
                lock (this.ocaOrdersByGroup)
                {
                    ArrayList list2 = (ArrayList) this.ocaOrdersByGroup[order.OcaGroup];
                    if (list2 != null)
                    {
                        foreach (Order order2 in list2)
                        {
                            if (order2 != order)
                            {
                                list.Add(order2);
                            }
                        }
                        list2.Clear();
                    }
                }
                foreach (Order order3 in list)
                {
                    order3.Cancel();
                }
            }
            iTrading.Track.ErrorCode code = new BrokerCancelOrder(this, order).Send();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToCancelOrder, code.ToString() + ": Order '" + order.OrderId + "' can't be cancelled now", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
            }
            else
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingCancel], this.connection.Now));
            }
        }

        internal void Cleanup()
        {
            Thread receiveThread = this.receiveThread;
            this.receiveThread = null;
            if (receiveThread != null)
            {
                receiveThread.Abort();
            }
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Track.Adapter.Connect1:  host='", this.Options.Host, "' port=", this.Options.Port, " messageWaitMilliSeconds=", this.Options.MessageWaitMilliSeconds }));
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.ConnectSimulation)).Start();
            }
            else
            {
                this.brokerErrorMessageRequest = new BrokerErrorMessageRequest(this);
                this.brokerMessageRequest = new BrokerMessageRequest(this);
                this.brokerTableMessageRequest = new BrokerTableMessageRequest(this);
                this.errorReportRequest = new ErrorReportRequest(this);
                if (this.dllHandle == 0)
                {
                    try
                    {
                        string dllName = Globals.InstallDir + @"\bin\Track\mytrackDLL.dll";
                        this.dllHandle = Api.LoadLibrary(dllName);
                        if (this.dllHandle != 0)
                        {
                            LogonRequest request = new LogonRequest(this);
                            if (request.Send() != iTrading.Track.ErrorCode.NoError)
                            {
                                Api.FreeLibrary(this.dllHandle);
                                this.dllHandle = 0;
                            }
                        }
                        else
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NativeError, "Failed to load '" + dllName + "'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        }
                    }
                    catch (Exception exception)
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.InvalidLicense, "Failed to locate 'mytrackDLL.dll': " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                }
            }
        }

        private void ConnectSimulation()
        {
            this.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
        }

        internal void ConnectTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if ((this.connection.ConnectionStatusId == ConnectionStatusId.Connected) && !Api.IsConnected())
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.ConnectionLost, ConnectionStatusId.ConnectionLost, 0, ""));
                this.connection.SynchronizeInvoke.AsyncInvoke(new MethodInvoker(this.ReconnectNow), new object[0]);
            }
        }

        internal void Delay()
        {
            int milliseconds = DateTime.Now.Subtract(this.timeLastRequest).Milliseconds;
            if ((this.timeLastRequest != Globals.MaxDate) && (milliseconds < this.Options.WaitMilliSecondsRequest))
            {
                Thread.Sleep((int) (this.Options.WaitMilliSecondsRequest - milliseconds));
            }
            this.timeLastRequest = DateTime.Now;
        }

        public void Disconnect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Disconnect");
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                new LogoffRequest(this, false).Send();
            }
        }

        private void DisconnectNow()
        {
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        internal void Init()
        {
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.AustralianDollar, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.BritishPound, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.CanadianDollar, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.Euro, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.HongKongDollar, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.JapaneseYen, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.SwissFranc, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.Unknown, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.UsDollar, ""));
            if (this.Options.Mode.Id == ModeTypeId.Simulation)
            {
                this.connection.CreateSimulationAccount(this.connection.SimulationAccountName, new SimulationAccountOptions());
            }
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", AccountItemTypeId.BuyingPower, "Buying Power"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", AccountItemTypeId.CashValue, "Cash"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", AccountItemTypeId.RealizedProfitLoss, "Realized P&L"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.Buy, "1"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.BuyToCover, "3"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.Sell, "2"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.SellShort, "4"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Default, ""));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.ClockSynchronization, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MarketDepth, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 999.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MaxMarketDepthStreams, 16.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.NativeOcaOrders, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.News, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.Order, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.Quotes1Minute, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.QuotesDaily, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.QuotesTick, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.SymbolLookup, 0.0));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Ask, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Bid, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.DailyHigh, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.DailyLow, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.DailyVolume, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Last, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.LastClose, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Opening, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketPositionId.Long, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketPositionId.Short, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketPositionId.Unknown, ""));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.AfxFocus, "6"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.AfxUk, "E"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.BusinessWire, "B"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.Comtex, "C"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.Default, ""));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.DowJones, "D"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.FirstCall, "1"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.FuturesWorld, "W"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.InternetWire, "IW"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.JagNotes, "JAG"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.MarketNewsPub, "MNP"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.MidnightTrader, "MN"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.PrimeZone, "PZ"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.PREuro, "PE"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.PRNewswire, "PRN"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.RealTimeTrader, "TT"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.Reuters, "S"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.ReutersBasic, "RD"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.ReutersPremium, "R"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.TheFlyOnTheWall, "FLY"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.ZacksTrader, "Z"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Cancelled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Filled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Initialized, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PartFilled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PendingCancel, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PendingSubmit, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PendingChange, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Rejected, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Accepted, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Unknown, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Working, ""));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Market, "1"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Limit, "2"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Stop, "4"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.StopLimit, "3"));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", SymbolTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", SymbolTypeId.Future, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", SymbolTypeId.Index, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", SymbolTypeId.Stock, ""));
            this.connection.ProcessEventArgs(new TimeInForceEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", TimeInForceId.Day, "0"));
            this.connection.ProcessEventArgs(new TimeInForceEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", TimeInForceId.Gtc, "1"));
        }

        internal void Init2()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Init2");
            }
            lock (this.startupRequests)
            {
                if (this.startupRequests.Count > 0)
                {
                    iTrading.Track.Request request = (iTrading.Track.Request) this.startupRequests[0];
                    this.startupRequests.RemoveAt(0);
                    request.Send();
                    return;
                }
            }
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
            foreach (MarketData data in this.marketData2Recover)
            {
                data.Start();
            }
            foreach (MarketDepth depth in this.marketDepth2Recover)
            {
                depth.Start();
            }
            new NewsVendorsRequest(this).Send();
            this.timerRequest = new TimerRequest(this);
            this.timerRequest.Send();
            this.newsRequest = new NewsRequest(this);
            this.newsRequest.Send();
            if (this.checkStatusTimer != null)
            {
                this.checkStatusTimer.Stop();
            }
            this.checkStatusTimer = new System.Timers.Timer((double) this.Options.CheckStatusMilliSeconds);
            this.checkStatusTimer.AutoReset = true;
            this.checkStatusTimer.Elapsed += new ElapsedEventHandler(this.ConnectTimerElapsed);
            this.checkStatusTimer.Start();
        }

        private string MonthCode(Symbol symbol)
        {
            if (symbol.Expiry == Globals.ContinousContractExpiry)
            {
                return "##";
            }
            switch (symbol.Expiry.Month)
            {
                case 1:
                    return "F";

                case 2:
                    return "G";

                case 3:
                    return "H";

                case 4:
                    return "J";

                case 5:
                    return "K";

                case 6:
                    return "M";

                case 7:
                    return "N";

                case 8:
                    return "Q";

                case 9:
                    return "U";

                case 10:
                    return "V";

                case 11:
                    return "X";
            }
            return "Z";
        }

        internal void ReceiveThreadLoop()
        {
            iTrading.Track.ErrorCode noError;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.ReceiveThreadLoop");
            }
            lock (this.syncThreadStart)
            {
                Monitor.Pulse(this.syncThreadStart);
            }
        Label_004E:
            do
            {
                if (this.receiveThread == null)
                {
                    return;
                }
                noError = iTrading.Track.ErrorCode.NoError;
                try
                {
                    noError = this.reader.GetMessage();
                }
                catch (ThreadAbortException)
                {
                    return;
                }
            }
            while ((noError == iTrading.Track.ErrorCode.NoMessage) || (this.connection.ConnectionStatusId == ConnectionStatusId.Disconnected));
            MessageCode msgCode = (MessageCode) this.reader.ReadShort();
            if (Globals.TraceSwitch.Native)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Track.Adapter.MessageLoop: msgCode=", msgCode, " err=", noError }));
            }
            this.reader.Skip(2);
            int num = this.reader.ReadInteger();
            if ((num < 0) || (num > (this.requests.Count - 1)))
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", string.Concat(new object[] { "Track.Adapter.MessageLoop: rqn ", num, " is out of valid range 0 - ", this.requests.Count - 1 })));
            }
            else if (num != 0)
            {
                iTrading.Track.Request request = (iTrading.Track.Request) this.requests[num];
                if (request == null)
                {
                    if (msgCode != MessageCode.FromTrack)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.Adapter.MessageLoop: unknown rqn " + num));
                    }
                }
                else
                {
                    request.Process(msgCode, this.reader);
                }
            }
            else
            {
                switch (msgCode)
                {
                    case MessageCode.BrokerErrorMessage:
                        this.brokerErrorMessageRequest.Process(msgCode, this.reader);
                        goto Label_004E;

                    case MessageCode.BrokerMessage:
                        this.brokerMessageRequest.Process(msgCode, this.reader);
                        goto Label_004E;

                    case MessageCode.BrokerTableMessage:
                        this.brokerTableMessageRequest.Process(msgCode, this.reader);
                        goto Label_004E;

                    case MessageCode.ErrorReport:
                        this.errorReportRequest.Process(msgCode, this.reader);
                        goto Label_004E;
                }
                if (msgCode != MessageCode.FromTrack)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.Adapter.MessageLoop: unexpected message code '" + msgCode + "'"));
                }
            }
            goto Label_004E;
        }

        private void ReconnectNow()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.ReconnectNow");
            }
            foreach (MarketData data in this.connection.MarketDataStreams)
            {
                this.marketData2Recover.Add(data);
            }
            foreach (MarketData data2 in this.marketData2Recover)
            {
                data2.Cancel();
            }
            foreach (MarketDepth depth in this.connection.MarketDepthStreams)
            {
                this.marketDepth2Recover.Add(depth);
            }
            foreach (MarketDepth depth2 in this.marketDepth2Recover)
            {
                depth2.Cancel();
            }
            new LogoffRequest(this, true).Send();
        }

        public void Request(Quotes quotes)
        {
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Request: " + quotes.ToString());
            }
            if (quotes.Period.Id == PeriodTypeId.Day)
            {
                new HistoricalDataRequest(this, quotes).Send();
            }
            else if (quotes.Period.Id == PeriodTypeId.Minute)
            {
                new IntradayDataRequest(this, quotes).Send();
            }
            else if (quotes.Period.Id == PeriodTypeId.Tick)
            {
                new IntradayHistoryRequest(this, quotes).Send();
            }
            else
            {
                Trace.Assert(false, "Track.Adapter.Request: " + ((int) quotes.Period.Id));
            }
        }

        public void Submit(Order order)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Submit: " + order.ToString());
            }
            if (order.Symbol.SymbolType.Id != SymbolTypeId.Stock)
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, "TrackData only supports orders on stocks", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
            }
            else
            {
                if (order.OcaGroup.Length > 0)
                {
                    lock (this.ocaOrdersByGroup)
                    {
                        ArrayList list = (ArrayList) this.ocaOrdersByGroup[order.OcaGroup];
                        if (list == null)
                        {
                            list = new ArrayList();
                            this.ocaOrdersByGroup[order.OcaGroup] = list;
                            list.Add(order);
                        }
                        else if (list.Count == 0)
                        {
                            list.Add(order);
                        }
                        else if (list.Count > 1)
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, "TrackData only supports 2 OCA orders per OCA group", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                        }
                        else
                        {
                            list.Add(order);
                            Order order2 = (Order) list[0];
                            iTrading.Track.ErrorCode code = new BrokerEnterCondOrder(this, order2, order).Send();
                            if (code != iTrading.Track.ErrorCode.NoError)
                            {
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, code.ToString() + ": Order could not be submitted", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order2, iTrading.Core.Kernel.ErrorCode.OrderRejected, code.ToString() + ": Order could not be submitted", order2.OrderId, order2.LimitPrice, order2.StopPrice, order2.Quantity, order2.AvgFillPrice, order2.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                            }
                            else
                            {
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingSubmit], this.connection.Now));
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order2, iTrading.Core.Kernel.ErrorCode.NoError, "", order2.OrderId, order2.LimitPrice, order2.StopPrice, order2.Quantity, order2.AvgFillPrice, order2.Filled, this.connection.OrderStates[OrderStateId.PendingSubmit], this.connection.Now));
                            }
                        }
                        return;
                    }
                }
                iTrading.Track.ErrorCode code2 = new BrokerEnterOrder(this, order).Send();
                if (code2 != iTrading.Track.ErrorCode.NoError)
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, code2.ToString() + ": Order could not be submitted", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                }
                else
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingSubmit], this.connection.Now));
                }
            }
        }

        public void Subscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Subscribe.MarketData: symbol='" + marketData.Symbol.FullName + "'");
            }
            QuoteDataRequest request = new QuoteDataRequest(this, marketData);
            marketData.AdapterLink = request;
            request.Send();
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Subscribe.MarketDepth: symbol='" + marketDepth.Symbol.FullName + "'");
            }
            NasdaqLevel2Request request = new NasdaqLevel2Request(this, marketDepth.Symbol);
            marketDepth.AdapterLink = request;
            request.Send();
        }

        public void SymbolLookup(Symbol template)
        {
            new BackgroundDataRequest(this, template).Send();
        }

        internal string ToBrokerName(Symbol symbol)
        {
            string str = symbol.GetProviderName(ProviderTypeId.TrackData).ToUpper();
            if (symbol.SymbolType.Id == SymbolTypeId.Future)
            {
                str = str + '`' + this.MonthCode(symbol);
            }
            return str;
        }

        internal Api.BrokerOrder ToBrokerOrder(Order order)
        {
            Api.BrokerOrder order2 = new Api.BrokerOrder();
            string str = this.ToBrokerName(order.Symbol);
            order2.Init();
            order.Account.Name.CopyTo(0, order2.acct, 0, order.Account.Name.Length);
            str.CopyTo(0, order2.ticker, 0, str.Length);
            order2.actionCode = (char) Convert.ToInt32(order.Action.MapId);
            order2.instrumentType = '\x0001';
            order2.fillKill = '\0';
            order2.AllNone = '\0';
            order2.orderType = (char) Convert.ToInt32(order.OrderType.MapId);
            order2.dayGTC = (char) Convert.ToInt32(order.TimeInForce.MapId);
            order2.orderQty = order.Quantity;
            order2.limitPrice = (float) order.LimitPrice;
            order2.stopPrice = (float) order.StopPrice;
            return order2;
        }

        internal Symbol ToSymbol(string name)
        {
            if (name.IndexOf('`') >= 0)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.Adapter.ToSymbol: Unable to convert '" + name + "' to TradeMagic symbol"));
                return null;
            }
            Symbol symbol = this.connection.GetSymbolByProviderName(name, Globals.MaxDate, this.connection.SymbolTypes[SymbolTypeId.Stock], this.connection.Exchanges[ExchangeId.Default], 0.0, RightId.Unknown, LookupPolicyId.RepositoryOnly);
            if (symbol != null)
            {
                return symbol;
            }
            return this.connection.CreateSymbol(name, Globals.MaxDate, this.connection.SymbolTypes[SymbolTypeId.Stock], this.connection.Exchanges[ExchangeId.Default], 0.0, RightId.Unknown, this.connection.Currencies[CurrencyId.Unknown], 0.01, 0.0, "", null, 0, null, null, null);
        }

        public void Unsubscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Unsubscribe.MarketData: symbol='" + marketData.Symbol.FullName + "'");
            }
            Trace.Assert(marketData.AdapterLink != null, "Track.Adapter.Unsubscribe.MarketDepth: symbol='" + marketData.Symbol.FullName + "' AdapterLink=null");
            ((QuoteDataRequest) marketData.AdapterLink).Halt();
            marketData.AdapterLink = null;
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Track.Adapter.Unsubscribe.MarketDepth: symbol='" + marketDepth.Symbol.FullName + "'");
            }
            Trace.Assert(marketDepth.AdapterLink != null, "Track.Adapter.Unsubscribe.MarketDepth: symbol='" + marketDepth.Symbol.FullName + "' AdapterLink=null");
            ((NasdaqLevel2Request) marketDepth.AdapterLink).Halt();
            marketDepth.AdapterLink = null;
        }

        internal TrackOptions Options
        {
            get
            {
                return (TrackOptions) this.connection.Options;
            }
        }
    }
}

