using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading .Mbt;

    /// <summary>
    /// Base class for TradeMagic tests.
    /// </summary>
    [TestFixture]
    public abstract class TestBase
    {
        private bool accountItemTypeSeen = false;
        private bool accountSeen = false;
        private bool actionTypeSeen = false;
        private ProviderType broker = null;
        private bool connected = false;
        private iTrading.Core.Kernel.Connection connection = new iTrading.Core.Kernel.Connection();
        private bool currencySeen = false;
        private int doneMultiConnect = 0;
        private int doneMultiThread = 0;
        private SymbolTemplate[] dtnSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("MSFT", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("CSCO", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("DELL", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("INTC", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("ES", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("NQ", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("YM", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("DJIA", typeIndex, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true) };
        private iTrading.Test.Environment environment = iTrading.Test.Environment.Local;
        private SymbolTemplate[] eSignalSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("MSFT", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("CSCO", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("DELL", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("INTC", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("ES", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("NQ", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("YM", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("DJIA", typeIndex, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true) };
        private static ExchangeId exCbot = ExchangeId.ECbot;
        private bool exchangeSeen = false;
        private static ExchangeId exDefault = ExchangeId.Default;
        private bool executeInLiveMode = true;
        private static ExchangeId exEurex = ExchangeId.Eurex;
        private static ExchangeId exGlobex = ExchangeId.Globex;
        private static ExchangeId exIsland = ExchangeId.Island;
        private static ExchangeId exLiffe = ExchangeId.Liffe;
        private bool expectConnect = true;
        private SymbolTemplate[] ibSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("MSFT", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exIsland, true), new SymbolTemplate("CSCO", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exIsland, true), new SymbolTemplate("DELL", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exIsland, true), new SymbolTemplate("INTC", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exIsland, true), new SymbolTemplate("DJIA", typeIndex, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("ES", typeFuture, new DateTime(0x7d5, 3, 1), exGlobex, true), new SymbolTemplate("NQ", typeFuture, new DateTime(0x7d5, 3, 1), exGlobex, true), new SymbolTemplate("blabla", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exEurex, false), new SymbolTemplate("blabla", typeUnknown, iTrading.Core.Kernel.Globals.MaxDate, exEurex, false), new SymbolTemplate("MSFT", typeStock, invalidDate, exIsland, true), new SymbolTemplate("CSCO", typeStock, invalidDate, exIsland, true), new SymbolTemplate("DELL", typeStock, invalidDate, exIsland, true), new SymbolTemplate("INTC", typeStock, invalidDate, exIsland, true), new SymbolTemplate("ES", typeFuture, invalidDate, exGlobex, false), new SymbolTemplate("NQ", typeFuture, invalidDate, exGlobex, false) };
        private static DateTime invalidDate = new DateTime(0x7d4, 11, 1);
        private bool marketDataTypeSeen = false;
        private bool marketPositionSeen = false;
        private int maxConnections = 5;
        private int maxThreads = 10;
        private static NavigatorForm mbtNavigator = null;
        private SymbolTemplate[] mbtSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("MSFT", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("DELL", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("INTC", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("GE", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("ES", typeFuture, new DateTime(0x7d4, 12, 1), exDefault, true), new SymbolTemplate("NQ", typeFuture, new DateTime(0x7d4, 12, 1), exDefault, true), new SymbolTemplate("YM", typeFuture, new DateTime(0x7d4, 12, 1), exDefault, true), new SymbolTemplate("DJIA", typeIndex, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true) };
        private ModeType mode = ModeType.All[ModeTypeId.Demo];
        private bool newsItemTypeSeen = false;
        private OptionsBase options;
        private bool orderStateSeen = false;
        private bool orderTypeSeen = false;
        private SymbolTemplate[] patsSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("ES", typeFuture, new DateTime(0x7d4, 12, 1), exGlobex, true), new SymbolTemplate("NQ", typeFuture, new DateTime(0x7d4, 12, 1), exGlobex, true), new SymbolTemplate("ZB", typeFuture, new DateTime(0x7d4, 12, 1), exCbot, true), new SymbolTemplate("ZN", typeFuture, new DateTime(0x7d4, 12, 1), exCbot, true), new SymbolTemplate("YM", typeFuture, new DateTime(0x7d4, 12, 1), exCbot, true), new SymbolTemplate("CF", typeFuture, new DateTime(0x7d4, 12, 1), exLiffe, true), new SymbolTemplate("ES", typeFuture, invalidDate, exGlobex, false) };
        private SymbolTemplate[] patsSymbolTemplatesTest = new SymbolTemplate[] { new SymbolTemplate("ES", typeFuture, new DateTime(0x7d4, 9, 1), exDefault, true), new SymbolTemplate("ZB", typeFuture, new DateTime(0x7d4, 9, 1), exDefault, true), new SymbolTemplate("ZN", typeFuture, invalidDate, exDefault, false) };
        private bool runMultiple = false;
        private bool runMultiThread = false;
        private int seqNr = 0;
        private bool symbolTypeSeen = false;
        private SymbolTemplate[] trackSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("MSFT", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("CSCO", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("DELL", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("INTC", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("ES", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("NQ", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("YM", typeFuture, new DateTime(0x7d5, 3, 1), exDefault, true), new SymbolTemplate("DJIA", typeIndex, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true) };
        private static SymbolTypeId typeFuture = SymbolTypeId.Future;
        private static SymbolTypeId typeIndex = SymbolTypeId.Index;
        private static SymbolTypeId typeStock = SymbolTypeId.Stock;
        private static SymbolTypeId typeUnknown = SymbolTypeId.Unknown;
        private SymbolTemplate[] yahooSymbolTemplates = new SymbolTemplate[] { new SymbolTemplate("MSFT", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("CSCO", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("DELL", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("INTC", typeStock, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true), new SymbolTemplate("DJIA", typeIndex, iTrading.Core.Kernel.Globals.MaxDate, exDefault, true) };

        protected TestBase()
        {
        }

        private void AccountItemType_AccountItemTypes(object sender, AccountItemTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("AccountItemTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("AccountItemTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("AccountItemTypeEventArgs 2", e.AccountItemType != null);
            this.accountItemTypeSeen = true;
        }

        private void Accounts_Account(object sender, AccountEventArgs e)
        {
             iTrading.Test.Globals.Assert("AccountEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("AccountEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("AccountEventArgs 2", e.Account.Name.Length > 0);
            this.accountSeen = true;
        }

        private void ActionTypes_ActionType(object sender, ActionTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("ActionTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("ActionTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("ActionTypeEventArgs 2", e.ActionType != null);
            this.actionTypeSeen = true;
        }

        private void Connection_ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            if ((this.expectConnect && (e.ConnectionStatusId == ConnectionStatusId.Connected)) && (e.OldConnectionStatusId == ConnectionStatusId.Connecting))
            {
                bool flag = e.Connection.FeatureTypes[FeatureTypeId.Order] != null;
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 000", e.Connection.ConnectionStatusId == ConnectionStatusId.Connected);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 001", flag ? this.accountSeen : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 002", flag ? (e.Connection.Accounts.Count > 0) : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 003", flag ? this.accountItemTypeSeen : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 004", flag ? (e.Connection.AccountItemTypes.Count > 0) : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 005", flag ? this.actionTypeSeen : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 006", flag ? (e.Connection.ActionTypes.Count > 0) : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 007", this.currencySeen);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 008", e.Connection.Currencies.Count > 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 008", this.exchangeSeen);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 009", e.Connection.Exchanges.Count > 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 010", this.marketDataTypeSeen);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 011", e.Connection.MarketDataTypes.Count > 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 012", flag ? this.marketPositionSeen : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 013", flag ? (e.Connection.MarketPositions.Count > 0) : true);
                if (e.Connection.FeatureTypes[FeatureTypeId.News] != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 014", this.newsItemTypeSeen);
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 015", e.Connection.NewsItemTypes.Count > 0);
                }
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 016", flag ? this.orderStateSeen : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 017", flag ? (e.Connection.OrderStates.Count > 0) : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 018", flag ? this.orderTypeSeen : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 019", flag ? (e.Connection.OrderTypes.Count > 0) : true);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 020", this.symbolTypeSeen);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 021", e.Connection.SymbolTypes.Count > 0);
                e.Connection.CreateSimulationAccount(e.Connection.SimulationAccountName, new SimulationAccountOptions());
                this.TestCustomText(e);
                this.TestSerialization(this.Connection);
                this.TestRounding();
            }
            else if (!this.expectConnect && (e.ConnectionStatusId == ConnectionStatusId.Disconnected))
            {
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 100", e.Connection.ConnectionStatusId != ConnectionStatusId.Connected);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 101", e.Connection.Accounts.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 102", e.Connection.AccountItemTypes.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 103", e.Connection.ActionTypes.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 104", e.Connection.Currencies.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 105", e.Connection.Exchanges.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 106", e.Connection.MarketDataTypes.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 108", e.Connection.MarketPositions.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 109", e.Connection.NewsItemTypes.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 110", e.Connection.OrderStates.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 111", e.Connection.OrderTypes.Count == 0);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs 112", e.Connection.SymbolTypes.Count == 0);
                if ((this.Broker.Id == ProviderTypeId.MBTrading) && (mbtNavigator != null))
                {
                    mbtNavigator.Hide();
                }
            }
            this.connected = e.Connection.ConnectionStatusId == ConnectionStatusId.Connected;
        }

        private void Connection_Error(object sender, iTrading.Core.Kernel.ITradingErrorEventArgs e)
        {
             iTrading.Test.Globals.Assert(e.Message, false);
        }

        /// <summary>
        /// Override this method to create instances of the derived test class.
        /// </summary>
        /// <returns></returns>
        protected abstract TestBase CreateInstance();
        private void Currencies_Currency(object sender, CurrencyEventArgs e)
        {
             iTrading.Test.Globals.Assert("CurrencyEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("CurrencyEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("CurrencyEventArgs 2", e.Currency != null);
            this.currencySeen = true;
        }

        /// <summary>
        /// Setup test environment.
        /// </summary>
        protected virtual void DoSetUp()
        {
        }

        /// <summary>
        /// Tear down test environment.
        /// </summary>
        protected virtual void DoTearDown()
        {
        }

        /// <summary>
        /// Override this method to perform the test.
        /// </summary>
        protected abstract void DoTest();
        private void Exchanges_Exchange(object sender, ExchangeEventArgs e)
        {
             iTrading.Test.Globals.Assert("ExchangeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("ExchangeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("ExchangeEventArgs 2", e.Exchange != null);
            this.exchangeSeen = true;
        }

        private void FeatureTypes_FeatureType(object sender, FeatureTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("FeatureTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("FeatureTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("FeatureTypeEventArgs 2", e.FeatureType != null);
        }

        private void MarketDataTypes_MarketDataType(object sender, MarketDataTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("MarketDataTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("MarketDataTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("MarketDataTypeEventArgs 2", e.MarketDataType != null);
            this.marketDataTypeSeen = true;
        }

        private void MarketPositions_MarketPosition(object sender, MarketPositionEventArgs e)
        {
             iTrading.Test.Globals.Assert("MarketPositionEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("MarketPositionEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("MarketPositionEventArgs 2", e.MarketPosition != null);
            this.marketPositionSeen = true;
        }

        private void MultiConnectInfiniteStart()
        {
            TestBase base2 = this.CreateInstance();
            base2.ExecuteInLiveMode = false;
            base2.environment = this.environment;
            base2.options = this.options;
            base2.runMultiple = true;
            lock (this)
            {
                base2.seqNr = this.seqNr++;
            }
            base2.SingleInfinite();
        }

        private void MultiConnectStart()
        {
            TestBase base2 = this.CreateInstance();
            base2.ExecuteInLiveMode = false;
            base2.environment = this.environment;
            base2.options = this.options;
            base2.runMultiple = true;
            lock (this)
            {
                base2.seqNr = this.seqNr++;
            }
            base2.Single();
            this.doneMultiConnect++;
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Single" /> in a multi connection environment.
        /// </summary>
        [Test]
        public virtual void Multiple()
        {
            this.SetUpServer();
            this.doneMultiConnect = 0;
            for (int i = 0; i < this.maxConnections; i++)
            {
                ThreadStart start = new ThreadStart(this.MultiConnectStart);
                Thread thread = new Thread(start);
                thread.Name = base.GetType().Name + i;
                thread.Start();
            }
            while (this.doneMultiConnect < this.maxConnections)
            {
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
            }
            this.TearDownServer();
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.SingleInfinite" /> in a multi connection environment.
        /// Please note: This method will not terminate, unless an error occurs.
        /// </summary>
        [Test]
        public virtual void MultipleInfinite()
        {
            for (int i = 0; i < this.maxConnections; i++)
            {
                ThreadStart start = new ThreadStart(this.MultiConnectInfiniteStart);
                Thread thread = new Thread(start);
                thread.Name = base.GetType().Name + i;
                thread.Start();
            }
        }

        private void MultiThreadStart()
        {
            TestBase base2 = this.CreateInstance();
            base2.Broker = this.Broker;
            base2.connection = this.Connection;
            base2.environment = this.environment;
            base2.options = this.options;
            base2.runMultiple = this.runMultiple;
            base2.runMultiThread = this.runMultiThread;
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + this.Connection.IdPlus + ") " + base2.GetType().Name + ": DoSetUp");
            }
            base2.DoSetUp();
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + this.Connection.IdPlus + ") " + base2.GetType().Name + ": DoTest");
            }
            base2.DoTest();
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + this.Connection.IdPlus + ") " + base2.GetType().Name + ": DoTearDown");
            }
            base2.DoTearDown();
            this.doneMultiThread++;
        }

        private void NewsItemTypes_NewsItemType(object sender, NewsItemTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("NewsItemTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("NewsItemTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("NewsItemTypeEventArgs 2", e.NewsItemType != null);
            this.newsItemTypeSeen = true;
        }

        private void OrderStates_OrderState(object sender, OrderStateEventArgs e)
        {
             iTrading.Test.Globals.Assert("OrderStateEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("OrderStateEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("OrderStateEventArgs 2", e.OrderState != null);
            this.orderStateSeen = true;
        }

        private void OrderTypes_OrderType(object sender, OrderTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("OrderTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("OrderTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("OrderTypeEventArgs 2", e.OrderType != null);
            this.orderTypeSeen = true;
        }

        private void SetUp()
        {
            int num;
            this.expectConnect = true;
            this.connection.AccountItemTypes.AccountItemType += new AccountItemTypeEventHandler(this.AccountItemType_AccountItemTypes);
            this.connection.Accounts.Account += new AccountEventHandler(this.Accounts_Account);
            this.connection.ActionTypes.ActionType += new ActionTypeEventHandler(this.ActionTypes_ActionType);
            this.connection.Currencies.Currency += new CurrencyEventHandler(this.Currencies_Currency);
            this.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.Connection_ConnectionStatus);
            this.connection.Error += new ErrorArgsEventHandler(this.Connection_Error);
            this.connection.Exchanges.Exchange += new ExchangeEventHandler(this.Exchanges_Exchange);
            this.connection.FeatureTypes.FeatureType += new FeatureTypeEventHandler(this.FeatureTypes_FeatureType);
            this.connection.MarketDataTypes.MarketDataType += new MarketDataTypeEventHandler(this.MarketDataTypes_MarketDataType);
            this.connection.MarketPositions.MarketPosition += new MarketPositionEventHandler(this.MarketPositions_MarketPosition);
            this.connection.NewsItemTypes.NewsItemType += new NewsItemTypeEventHandler(this.NewsItemTypes_NewsItemType);
            this.connection.OrderStates.OrderState += new OrderStateEventHandler(this.OrderStates_OrderState);
            this.connection.OrderTypes.OrderType += new OrderTypeEventHandler(this.OrderTypes_OrderType);
            this.connection.SymbolTypes.SymbolType += new SymbolTypeEventHandler(this.SymbolTypes_SymbolType);
            this.connection.Connect(this.Options);
            while (this.connection.ConnectionStatusId != ConnectionStatusId.Connected)
            {
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
            }
            if (!this.Options.RunAtServer && (this.Broker.Id == ProviderTypeId.MBTrading))
            {
                if (mbtNavigator == null)
                {
                    mbtNavigator = new NavigatorForm();
                    mbtNavigator.WindowState = FormWindowState.Normal;
                }
                mbtNavigator.Show();
            }
            if (((this.Connection.Options.Mode.Id == ModeTypeId.Live) ||  iTrading.Test.Globals.noCleanUpOnConnect) || (this.Connection.Options.Provider.Id != ProviderTypeId.TrackData))
            {
                return;
            }
            lock (this.Connection.Accounts[0].Orders)
            {
                foreach (iTrading.Core.Kernel.Order order in this.Connection.Accounts[0].Orders)
                {
                    order.Cancel();
                }
            }
        Label_02AB:
            num = 0;
            foreach (iTrading.Core.Kernel.Order order2 in this.Connection.Accounts[0].Orders)
            {
                if (((order2.OrderState.Id != OrderStateId.Cancelled) && (order2.OrderState.Id != OrderStateId.Filled)) && (order2.OrderState.Id != OrderStateId.Rejected))
                {
                    num++;
                }
            }
            if (num != 0)
            {
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                goto Label_02AB;
            }
            lock (this.Connection.Accounts[0].Positions)
            {
                foreach (Position position in this.Connection.Accounts[0].Positions)
                {
                    this.Connection.Accounts[0].CreateOrder(position.Symbol, (position.MarketPosition.Id == MarketPositionId.Long) ? ActionTypeId.Sell : ActionTypeId.BuyToCover, OrderTypeId.Market, TimeInForceId.Gtc, position.Quantity, 0.0, 0.0, "", null).Submit();
                }
                goto Label_040D;
            }
        Label_0403:
             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
        Label_040D:
            if (this.Connection.Accounts[0].Positions.Count > 0)
            {
                goto Label_0403;
            }
        }

        private void SetUpServer()
        {
            if ((!this.runMultiple && (this.Environment ==  iTrading.Test.Environment.Server)) && ! iTrading.Test.Globals.noServerStartUp)
            {
                 iTrading.Test.Globals.StartServer();
            }
        }

        /// <summary>
        /// Execute test for all provided <see cref="T:iTrading.Core.Kernel.ProviderType" />
        /// instances, all <see cref="T:iTrading.Core.Kernel.ModeType" /> instances and all <see cref="T:iTrading.Core.Kernel.LicenseType" />
        /// instances. Tests are first performed on a single and then on a multi thread base.
        /// </summary>
        [Test]
        public virtual void Single()
        {
            this.SetUpServer();
            ArrayList list = new ArrayList();
            if (this.broker != null)
            {
                list.Add(this.broker);
            }
            else
            {
                foreach (ProviderType type in ProviderType.All.Values)
                {
                    list.Add(type);
                }
            }
            foreach (ProviderType type2 in list)
            {
                if ((this.runMultiple && ((type2.Id == ProviderTypeId.Patsystems) || (type2.Id == ProviderTypeId.MBTrading))) || ((this.mode.Id == ModeTypeId.Live) && !this.ExecuteInLiveMode))
                {
                    continue;
                }
                this.options = null;
                OptionsBase base2 = OptionsBase.Restore(type2, this.mode);
                if (base2 != null)
                {
                    this.options = base2;
                }
                if (this.options == null)
                {
                    MessageBox.Show("Set up connection by starting TradeMagic", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    continue;
                }
                if (type2.Id == ProviderTypeId.Dtn)
                {
                    if (this.mode.Id != ModeTypeId.Test)
                    {
                        goto Label_01D9;
                    }
                    continue;
                }
                if (type2.Id == ProviderTypeId.ESignal)
                {
                    if (this.mode.Id != ModeTypeId.Test)
                    {
                        goto Label_01D9;
                    }
                    continue;
                }
                if (type2.Id == ProviderTypeId.InteractiveBrokers)
                {
                    if (this.mode.Id == ModeTypeId.Test)
                    {
                        continue;
                    }
                    ((IBOptions) this.options).CheckForMarketData = false;
                    ((IBOptions) this.options).Port = new IBOptions().DefaultPort + this.seqNr;
                }
                else
                {
                    if (type2.Id == ProviderTypeId.MBTrading)
                    {
                        if (this.mode.Id != ModeTypeId.Test)
                        {
                            goto Label_01D9;
                        }
                        continue;
                    }
                    if (type2.Id == ProviderTypeId.TrackData)
                    {
                        if (this.mode.Id != ModeTypeId.Test)
                        {
                            goto Label_01D9;
                        }
                        continue;
                    }
                    if ((type2.Id == ProviderTypeId.Yahoo) && (this.mode.Id == ModeTypeId.Test))
                    {
                        continue;
                    }
                }
            Label_01D9:
                this.options.Mode = this.mode;
                this.options.RunAtServer = this.Environment ==  iTrading.Test.Environment.Server;
                this.runMultiThread = false;
                this.SetUp();
                this.MultiThreadStart();
                this.doneMultiThread = 0;
                this.runMultiThread = true;
                for (int i = 0; i < this.maxThreads; i++)
                {
                    ThreadStart start = new ThreadStart(this.MultiThreadStart);
                    Thread thread = new Thread(start);
                    thread.Name = base.GetType().Name + i;
                    thread.Start();
                }
                while (this.doneMultiThread < this.maxThreads)
                {
                     iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                }
                this.TearDown();
            }
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Single" /> in an infinite loop. 
        /// Please note: This method will not terminate, unless an error occurs.
        /// </summary>
        [Test]
        public virtual void SingleInfinite()
        {
            while (true)
            {
                this.Single();
            }
        }

        private void SymbolTypes_SymbolType(object sender, SymbolTypeEventArgs e)
        {
             iTrading.Test.Globals.Assert("SymbolTypeEventArgs 0", e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert("SymbolTypeEventArgs 1", e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert("SymbolTypeEventArgs 2", e.SymbolType != null);
            this.symbolTypeSeen = true;
        }

        private void TearDown()
        {
            this.expectConnect = false;
            this.connection.Close();
            while (this.connected)
            {
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
            }
        }

        private void TearDownServer()
        {
            if (!this.runMultiple)
            {
                 iTrading.Test.Globals.TerminateServer();
            }
        }

        private void TestCustomText(ConnectionStatusEventArgs e)
        {
            foreach (string str in new string[] { e.CustomText, this.Connection.CustomText })
            {
                if (str.Length != 0)
                {
                    if (this.Connection.Options.Provider.Id == ProviderTypeId.InteractiveBrokers)
                    {
                        XmlDocument document = new XmlDocument();
                        XmlTextReader reader = new XmlTextReader(new StringReader(str));
                        document.Load(reader);
                        reader.Close();
                        string str2 = "";
                        foreach (XmlNode node in document["TradeMagic"][this.Connection.Options.Provider.Id.ToString()]["ListOfAccountAliases"])
                        {
                            str2 = str2 + node["account"].InnerText + "/";
                            str2 = str2 + node["alias"].InnerText + " ";
                        }
                    Label_0227:
                        foreach (XmlNode node2 in document["TradeMagic"][this.Connection.Options.Provider.Id.ToString()]["ListOfAllocationProfiles"])
                        {
                            str2 = str2 + node2["name"] + "/";
                            str2 = str2 + node2["type"] + "/";
                            foreach (XmlNode node3 in node2["ListOfAllocations"])
                            {
                                str2 = str2 + node3["acct"].InnerText + "/";
                                str2 = str2 + node3["amount"].InnerText + "/";
                            }
                            goto Label_0227;
                        }
                    Label_0323:
                        foreach (XmlNode node4 in document["TradeMagic"][this.Connection.Options.Provider.Id.ToString()]["ListOfGroups"])
                        {
                            str2 = (str2 + node4["name"] + "/") + node4["defaultMethod"] + "/";
                            foreach (XmlNode node5 in node4["ListOfAccts"])
                            {
                                str2 = str2 + node5.InnerText + "/";
                            }
                            goto Label_0323;
                        }
                    }
                    else if (this.Connection.Options.Provider.Id == ProviderTypeId.Patsystems)
                    {
                        XmlDocument document2 = new XmlDocument();
                        XmlTextReader reader2 = new XmlTextReader(new StringReader(str));
                        document2.Load(reader2);
                        reader2.Close();
                        string str3 = "";
                        foreach (XmlNode node6 in document2["TradeMagic"][this.Connection.Options.Provider.Id.ToString()]["Contracts"])
                        {
                            str3 = (((str3 + node6["Name"].InnerText + "/") + node6["Exchange"].InnerText + "/") + node6["Expiry"].InnerText + "/") + node6["ExternalName"].InnerText + " ";
                        }
                    }
                }
            }
        }

        private void TestRounding()
        {
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e001", Symbol.Round2TickSize(9.49, 1.0) == 9.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e001", Symbol.Round2TickSize(9.51, 1.0) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e000", Symbol.Round2TickSize(10.49, 1.0) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e000", Symbol.Round2TickSize(10.51, 1.0) == 11.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e002", Symbol.Round2TickSize(7.49, 5.0) == 5.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e002", Symbol.Round2TickSize(7.51, 5.0) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e003", Symbol.Round2TickSize(12.49, 5.0) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e003", Symbol.Round2TickSize(12.51, 5.0) == 15.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e004", Symbol.Round2TickSize(9.749, 0.5) == 9.5);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e004", Symbol.Round2TickSize(9.751, 0.5) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e005", Symbol.Round2TickSize(10.249, 0.5) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e005", Symbol.Round2TickSize(10.251, 0.5) == 10.5);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e004", Symbol.Round2TickSize(9.9949, 0.01) == 9.99);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e004", Symbol.Round2TickSize(9.9951, 0.01) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e005", Symbol.Round2TickSize(10.0049, 0.01) == 10.0);
             iTrading.Test.Globals.Assert("ConnectionStatusEventArgs e005", Symbol.Round2TickSize(10.0051, 0.01) == 10.01);
        }

        private void TestSerialization(iTrading.Core.Kernel.Connection connection)
        {
            DateTime time;
            Bytes bytes = new TestBytes(this.Connection);
            for (int i = 0; i < 100; i++)
            {
                int num2 = 0;
                ProviderType provider = null;
                foreach (ProviderType type2 in ProviderType.All.Values)
                {
                    if ((num2++ % ProviderType.All.Count) == 0)
                    {
                        provider = type2;
                    }
                }
                num2 = 0;
                LicenseType license = null;
                foreach (LicenseType type4 in LicenseType.All.Values)
                {
                    if ((num2++ % LicenseType.All.Count) == 0)
                    {
                        license = type4;
                    }
                }
                num2 = 0;
                ModeType mode = null;
                foreach (ModeType type6 in ModeType.All.Values)
                {
                    if ((num2++ % ModeType.All.Count) == 0)
                    {
                        mode = type6;
                    }
                }
                num2 = 0;
                iTrading.Core.Kernel.Currency currency = null;
                foreach (iTrading.Core.Kernel.Currency currency2 in this.Connection.Currencies.Values)
                {
                    if ((num2++ % this.Connection.Currencies.Count) == 0)
                    {
                        currency = currency2;
                    }
                }
                num2 = 0;
                iTrading.Core.Kernel.ActionType type = null;
                foreach (iTrading.Core.Kernel.ActionType type8 in this.Connection.ActionTypes.Values)
                {
                    if ((num2++ % this.Connection.ActionTypes.Count) == 0)
                    {
                        type = type8;
                    }
                }
                num2 = 0;
                AccountItemType type9 = null;
                foreach (AccountItemType type10 in this.Connection.AccountItemTypes.Values)
                {
                    if ((num2++ % this.Connection.AccountItemTypes.Count) == 0)
                    {
                        type9 = type10;
                    }
                }
                num2 = 0;
                Exchange myValue = null;
                foreach (Exchange exchange2 in this.Connection.Exchanges.Values)
                {
                    if ((num2++ % this.Connection.Exchanges.Count) == 0)
                    {
                        myValue = exchange2;
                    }
                }
                num2 = 0;
                FeatureType type11 = null;
                foreach (FeatureType type12 in this.Connection.FeatureTypes.Values)
                {
                    if ((num2++ % this.Connection.FeatureTypes.Count) == 0)
                    {
                        type11 = type12;
                    }
                }
                num2 = 0;
                MarketDataType type13 = null;
                foreach (MarketDataType type14 in this.Connection.MarketDataTypes.Values)
                {
                    if ((num2++ % this.Connection.MarketDataTypes.Count) == 0)
                    {
                        type13 = type14;
                    }
                }
                num2 = 0;
                MarketPosition position = null;
                foreach (MarketPosition position2 in this.Connection.MarketPositions.Values)
                {
                    if ((num2++ % this.Connection.MarketPositions.Count) == 0)
                    {
                        position = position2;
                    }
                }
                num2 = 0;
                NewsItemType type15 = null;
                foreach (NewsItemType type16 in this.Connection.NewsItemTypes.Values)
                {
                    if ((num2++ % this.Connection.NewsItemTypes.Count) == 0)
                    {
                        type15 = type16;
                    }
                }
                num2 = 0;
                OrderState state = null;
                foreach (OrderState state2 in this.Connection.OrderStates.Values)
                {
                    if ((num2++ % this.Connection.OrderStates.Count) == 0)
                    {
                        state = state2;
                    }
                }
                num2 = 0;
                iTrading.Core.Kernel.OrderType type17 = null;
                foreach (iTrading.Core.Kernel.OrderType type18 in this.Connection.OrderTypes.Values)
                {
                    if ((num2++ % this.Connection.OrderTypes.Count) == 0)
                    {
                        type17 = type18;
                    }
                }
                num2 = 0;
                SymbolType type19 = null;
                foreach (SymbolType type20 in this.Connection.SymbolTypes.Values)
                {
                    if ((num2++ % this.Connection.SymbolTypes.Count) == 0)
                    {
                        type19 = type20;
                    }
                }
                num2 = 0;
                Account account = null;
                foreach (Account account2 in this.Connection.Accounts)
                {
                    if ((num2++ % this.Connection.Accounts.Count) == 0)
                    {
                        account = account2;
                    }
                }
                bytes.Write(i);
                bytes.Write(i < 50);
                bytes.Write("this is a string this is a string" + i);
                bytes.Write(currency.Id);
                bytes.Write((double) i);
                time = new DateTime(0x7d0, 1, 1);
                bytes.Write(time.AddHours((double) i));
                if (provider != null)
                {
                    bytes.Write(provider);
                }
                if (license != null)
                {
                    bytes.Write(license);
                }
                if (mode != null)
                {
                    bytes.Write(mode);
                }
                if (currency != null)
                {
                    bytes.Write(currency);
                }
                if (type != null)
                {
                    bytes.Write(type);
                }
                if (type9 != null)
                {
                    bytes.Write(type9);
                }
                if (myValue != null)
                {
                    bytes.Write(myValue);
                }
                if (type11 != null)
                {
                    bytes.Write(type11);
                }
                if (type13 != null)
                {
                    bytes.Write(type13);
                }
                if (position != null)
                {
                    bytes.Write(position);
                }
                if (type15 != null)
                {
                    bytes.Write(type15);
                }
                if (state != null)
                {
                    bytes.Write(state);
                }
                if (type17 != null)
                {
                    bytes.Write(type17);
                }
                if (type19 != null)
                {
                    bytes.Write(type19);
                }
                if (account != null)
                {
                    bytes.Write(account);
                }
            }
            byte[] inBuf = new byte[bytes.OutLength];
            for (int j = 0; j < bytes.OutLength; j++)
            {
                inBuf[j] = bytes.Out[j];
            }
            bytes = new TestBytes(this.Connection);
            bytes.Reset(inBuf);
            for (int k = 0; k < 100; k++)
            {
                int num5 = 0;
                ProviderType type21 = null;
                foreach (ProviderType type22 in ProviderType.All.Values)
                {
                    if ((num5++ % ProviderType.All.Count) == 0)
                    {
                        type21 = type22;
                    }
                }
                num5 = 0;
                LicenseType type23 = null;
                foreach (LicenseType type24 in LicenseType.All.Values)
                {
                    if ((num5++ % LicenseType.All.Count) == 0)
                    {
                        type23 = type24;
                    }
                }
                num5 = 0;
                ModeType type25 = null;
                foreach (ModeType type26 in ModeType.All.Values)
                {
                    if ((num5++ % ModeType.All.Count) == 0)
                    {
                        type25 = type26;
                    }
                }
                num5 = 0;
                iTrading.Core.Kernel.Currency currency3 = null;
                foreach (iTrading.Core.Kernel.Currency currency4 in this.Connection.Currencies.Values)
                {
                    if ((num5++ % this.Connection.Currencies.Count) == 0)
                    {
                        currency3 = currency4;
                    }
                }
                num5 = 0;
                iTrading.Core.Kernel.ActionType type27 = null;
                foreach (iTrading.Core.Kernel.ActionType type28 in this.Connection.ActionTypes.Values)
                {
                    if ((num5++ % this.Connection.ActionTypes.Count) == 0)
                    {
                        type27 = type28;
                    }
                }
                num5 = 0;
                AccountItemType type29 = null;
                foreach (AccountItemType type30 in this.Connection.AccountItemTypes.Values)
                {
                    if ((num5++ % this.Connection.AccountItemTypes.Count) == 0)
                    {
                        type29 = type30;
                    }
                }
                num5 = 0;
                Exchange exchange3 = null;
                foreach (Exchange exchange4 in this.Connection.Exchanges.Values)
                {
                    if ((num5++ % this.Connection.Exchanges.Count) == 0)
                    {
                        exchange3 = exchange4;
                    }
                }
                num5 = 0;
                FeatureType type31 = null;
                foreach (FeatureType type32 in this.Connection.FeatureTypes.Values)
                {
                    if ((num5++ % this.Connection.FeatureTypes.Count) == 0)
                    {
                        type31 = type32;
                    }
                }
                num5 = 0;
                MarketDataType type33 = null;
                foreach (MarketDataType type34 in this.Connection.MarketDataTypes.Values)
                {
                    if ((num5++ % this.Connection.MarketDataTypes.Count) == 0)
                    {
                        type33 = type34;
                    }
                }
                num5 = 0;
                MarketPosition position3 = null;
                foreach (MarketPosition position4 in this.Connection.MarketPositions.Values)
                {
                    if ((num5++ % this.Connection.MarketPositions.Count) == 0)
                    {
                        position3 = position4;
                    }
                }
                num5 = 0;
                NewsItemType type35 = null;
                foreach (NewsItemType type36 in this.Connection.NewsItemTypes.Values)
                {
                    if ((num5++ % this.Connection.NewsItemTypes.Count) == 0)
                    {
                        type35 = type36;
                    }
                }
                num5 = 0;
                OrderState state3 = null;
                foreach (OrderState state4 in this.Connection.OrderStates.Values)
                {
                    if ((num5++ % this.Connection.OrderStates.Count) == 0)
                    {
                        state3 = state4;
                    }
                }
                num5 = 0;
                iTrading.Core.Kernel.OrderType type37 = null;
                foreach (iTrading.Core.Kernel.OrderType type38 in this.Connection.OrderTypes.Values)
                {
                    if ((num5++ % this.Connection.OrderTypes.Count) == 0)
                    {
                        type37 = type38;
                    }
                }
                num5 = 0;
                SymbolType type39 = null;
                foreach (SymbolType type40 in this.Connection.SymbolTypes.Values)
                {
                    if ((num5++ % this.Connection.SymbolTypes.Count) == 0)
                    {
                        type39 = type40;
                    }
                }
                num5 = 0;
                Account account3 = null;
                foreach (Account account4 in this.Connection.Accounts)
                {
                    if ((num5++ % this.Connection.Accounts.Count) == 0)
                    {
                        account3 = account4;
                    }
                }
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b000 " + k, bytes.ReadInt32() == k);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b001 " + k, bytes.ReadBoolean() == (k < 50));
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b002 " + k, bytes.ReadString() == ("this is a string this is a string" + k));
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b003 " + k, bytes.ReadCurrencyId() == currency3.Id);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b004 " + k, bytes.ReadDouble() == k);
                time = new DateTime(0x7d0, 1, 1);
                 iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b005 " + k, bytes.ReadDateTime() == time.AddHours((double) k));
                if (type21 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b101 " + k, bytes.ReadProvider().Id == type21.Id);
                }
                if (type23 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b102 " + k, bytes.ReadLicenseType().Id == type23.Id);
                }
                if (type25 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b103 " + k, bytes.ReadModeType().Id == type25.Id);
                }
                if (currency3 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b104 " + k, bytes.ReadCurrency().Id == currency3.Id);
                }
                if (type27 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b105 " + k, bytes.ReadActionType().Id == type27.Id);
                }
                if (type29 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b106 " + k, bytes.ReadAccountItemType().Id == type29.Id);
                }
                if (exchange3 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b107 " + k, bytes.ReadExchange().Id == exchange3.Id);
                }
                if (type31 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b108 " + k, bytes.ReadFeatureType().Id == type31.Id);
                }
                if (type33 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b109 " + k, bytes.ReadMarketData().Id == type33.Id);
                }
                if (position3 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b111 " + k, bytes.ReadMarketPosition().Id == position3.Id);
                }
                if (type35 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b112 " + k, bytes.ReadNewsItemType().Id == type35.Id);
                }
                if (state3 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b113 " + k, bytes.ReadOrderState().Id == state3.Id);
                }
                if (type37 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b114 " + k, bytes.ReadOrderType().Id == type37.Id);
                }
                if (type39 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b115 " + k, bytes.ReadSymbolType().Id == type39.Id);
                }
                if (account3 != null)
                {
                     iTrading.Test.Globals.Assert("ConnectionStatusEventArgs b116 " + k, bytes.ReadAccount().Name == account3.Name);
                }
            }
        }

        internal ProviderType Broker
        {
            get
            {
                return this.broker;
            }
            set
            {
                this.broker = value;
            }
        }

        /// <summary>
        /// The actual connection.
        /// </summary>
        protected iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Get/set test environment.
        /// </summary>
        public iTrading.Test.Environment Environment
        {
            get
            {
                return this.environment;
            }
            set
            {
                this.environment = value;
            }
        }

        /// <summary>
        /// Set TRUE to execute tests also in live account. Beware of performing order related tests in live account.
        /// Default = TRUE;
        /// </summary>
        protected bool ExecuteInLiveMode
        {
            get
            {
                return this.executeInLiveMode;
            }
            set
            {
                this.executeInLiveMode = value;
            }
        }

        /// <summary>
        /// Get/set the number of max. connection for multi connection test. Default = 10.
        /// <see cref="M: iTrading.Test.TestBase.Multiple" />
        /// </summary>
        public int MaxConnections
        {
            get
            {
                return this.maxConnections;
            }
            set
            {
                this.maxConnections = value;
            }
        }

        /// <summary>
        /// Get/set the number of max. threads for multi thread test. Default = 10.
        /// </summary>
        public int MaxThreads
        {
            get
            {
                return this.maxThreads;
            }
            set
            {
                this.maxThreads = value;
            }
        }

        /// <summary>
        /// Get/set test mode.
        /// </summary>
        public ModeType Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }

        /// <summary>
        /// The actual connection options.
        /// </summary>
        protected OptionsBase Options
        {
            get
            {
                return this.options;
            }
        }

        /// <summary>
        /// </summary>
        protected bool RunMultiple
        {
            get
            {
                return this.runMultiple;
            }
        }

        /// <summary>
        /// </summary>
        protected bool RunMultiThread
        {
            get
            {
                return this.runMultiThread;
            }
        }

        internal SymbolTemplate[] SymbolTemplates
        {
            get
            {
                if (((this.Connection.Options == null) || (this.Connection.Options.Provider == null)) || (this.Connection.Options.Provider.Id == ProviderTypeId.InteractiveBrokers))
                {
                    return this.ibSymbolTemplates;
                }
                if (this.Connection.Options.Provider.Id == ProviderTypeId.Dtn)
                {
                    return this.dtnSymbolTemplates;
                }
                if (this.Connection.Options.Provider.Id == ProviderTypeId.ESignal)
                {
                    return this.eSignalSymbolTemplates;
                }
                if (this.Connection.Options.Provider.Id == ProviderTypeId.Patsystems)
                {
                    if (this.Connection.Options.Mode.Id != ModeTypeId.Test)
                    {
                        return this.patsSymbolTemplates;
                    }
                    return this.patsSymbolTemplatesTest;
                }
                if (this.Connection.Options.Provider.Id == ProviderTypeId.MBTrading)
                {
                    return this.mbtSymbolTemplates;
                }
                if (this.Connection.Options.Provider.Id == ProviderTypeId.TrackData)
                {
                    return this.trackSymbolTemplates;
                }
                if (this.Connection.Options.Provider.Id == ProviderTypeId.Yahoo)
                {
                    return this.yahooSymbolTemplates;
                }
                return null;
            }
        }
    }
}

