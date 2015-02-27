namespace TradeMagic.Pats
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    internal class Callback
    {
        private Adapter adapter;
        internal bool expectConnected = false;
        internal bool hostReconnecting = false;
        private DateTime lastQuoteTime = Globals.MinDate;
        private byte[] marketDataBuffer = new byte[0x83c];
        private string marketMaker = "";
        private ExecutionCollection overNight = new ExecutionCollection();
        internal Thread patsThread = null;
        internal bool priceReconnecting = false;
        private object[] syncCallback = new object[1];
        private MarketDataType typeAsk;
        private MarketDataType typeBid;
        private MarketDataType typeDailyHigh;
        private MarketDataType typeDailyLow;
        private MarketDataType typeDailyVolume;
        private MarketDataType typeLast;
        private MarketDataType typeLastClose;
        private MarketDataType typeOpening;

        internal Callback(Adapter adapter)
        {
            this.adapter = adapter;
        }

        private void AddOrder(PatsApi.OrderDetail orderDetail)
        {
            Account account = this.GetAccount(orderDetail.traderAccount);
            if (account == null)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.InvalidNativeAccount, "", "Pats.CallBack.AddOrder: Account '" + PatsApi.ToString(orderDetail.traderAccount) + "'"));
            }
            else
            {
                iTrading.Core.Kernel.Symbol symbol = this.adapter.Convert(orderDetail.exchangeName, orderDetail.contractName, orderDetail.contractDate);
                if (symbol == null)
                {
                    this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.InvalidNativeSymbol, "", "Pats.CallBack.AddOrder: Contract '" + PatsApi.ToString(orderDetail.contractName) + "/" + PatsApi.ToString(orderDetail.exchangeName) + "/" + PatsApi.ToString(orderDetail.contractDate) + "'"));
                }
                else
                {
                    string str = PatsApi.ToString(orderDetail.exchangeName);
                    bool flag = false;
                    double limitPrice = 0.0;
                    iTrading.Core.Kernel.OrderType type = this.Connection.OrderTypes[OrderTypeId.Market];
                    double stopPrice = 0.0;
                    TimeInForceId day = TimeInForceId.Day;
                    string str2 = PatsApi.ToString(orderDetail.orderType);
                    Hashtable hashtable = (Hashtable) this.adapter.exchange2OrderTypes[str];
                    Trace.Assert(type != null, "Pats.Callback.AddOrder: unexpected exchange '" + str + "'");
                    foreach (int num3 in hashtable.Keys)
                    {
                        if (!(((string) hashtable[num3]) == str2))
                        {
                            continue;
                        }
                        if ((num3 / 10) == 1)
                        {
                            limitPrice = PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, orderDetail.price);
                            type = this.Connection.OrderTypes[OrderTypeId.Limit];
                        }
                        else if ((num3 / 10) != 0)
                        {
                            if ((num3 / 10) == 2)
                            {
                                stopPrice = PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, orderDetail.price);
                                type = this.Connection.OrderTypes[OrderTypeId.Stop];
                            }
                            else if ((num3 / 10) == 3)
                            {
                                limitPrice = PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, orderDetail.price2);
                                stopPrice = PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, orderDetail.price);
                                type = this.Connection.OrderTypes[OrderTypeId.StopLimit];
                            }
                            else
                            {
                                Trace.Assert(false, "Pats.Callback.AddOrder: unexpected order type " + num3.ToString());
                            }
                        }
                        flag = true;
                        day = (TimeInForceId) (num3 % 10);
                        break;
                    }
                    if (!flag)
                    {
                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.AddOrder: unable to find order type '" + str2 + "'"));
                    }
                    else
                    {
                        int count = 0;
                        int index = orderDetail.index;
                        OrderStatusEventCollection history = new OrderStatusEventCollection();
                        TradeMagic.Pats.ErrorCode code = this.adapter.patsApi.CountOrderHistory(index, out count);
                        if (code != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.AddOrder: ptCountOrderHistory error " + PatsApi.ToString(orderDetail.orderId) + code));
                        }
                        else
                        {
                            for (int i = count - 1; i >= 0; i--)
                            {
                                PatsApi.OrderDetail detail;
                                code = this.adapter.patsApi.GetOrderHistory(index, i, out detail);
                                if (code != TradeMagic.Pats.ErrorCode.Success)
                                {
                                    this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", string.Concat(new object[] { "Pats.Callback.AddOrder: ptGetOrderHistory error ", PatsApi.ToString(orderDetail.orderId), " ", code })));
                                    return;
                                }
                                iTrading.Core.Kernel.ErrorCode noError = iTrading.Core.Kernel.ErrorCode.NoError;
                                string nativeError = "";
                                OrderStateId unknown = OrderStateId.Unknown;
                                switch (detail.status)
                                {
                                    case 2:
                                        unknown = OrderStateId.Accepted;
                                        break;

                                    case 3:
                                        unknown = OrderStateId.Working;
                                        break;

                                    case 4:
                                        noError = iTrading.Core.Kernel.ErrorCode.OrderRejected;
                                        nativeError = PatsApi.ToString(detail.nonExecReason);
                                        unknown = OrderStateId.Rejected;
                                        break;

                                    case 5:
                                        unknown = OrderStateId.Cancelled;
                                        break;

                                    case 6:
                                        unknown = OrderStateId.Cancelled;
                                        break;

                                    case 7:
                                        unknown = OrderStateId.PartFilled;
                                        break;

                                    case 8:
                                        unknown = OrderStateId.Filled;
                                        break;

                                    case 9:
                                        unknown = OrderStateId.PendingCancel;
                                        break;

                                    case 10:
                                        unknown = OrderStateId.PendingChange;
                                        break;

                                    case 13:
                                        unknown = OrderStateId.Accepted;
                                        break;

                                    case 14:
                                        unknown = OrderStateId.Cancelled;
                                        break;
                                }
                                if (unknown != OrderStateId.Unknown)
                                {
                                    history.Add(new OrderStatusEventArgs(null, noError, nativeError, PatsApi.ToString(detail.orderId), limitPrice, stopPrice, detail.lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.adapter.connection.OrderStates[unknown], this.Connection.Now));
                                }
                            }
                            iTrading.Core.Kernel.Order cbiOrder = account.CreateOrder(symbol, this.Connection.ActionTypes[(orderDetail.buyOrSell == 'B') ? ActionTypeId.Buy : ActionTypeId.Sell].Id, type.Id, day, orderDetail.lots, limitPrice, stopPrice, "", this.adapter.connection.OrderStates[OrderStateId.Initialized], PatsApi.ToString(orderDetail.orderId), "", history, null);
                            if (cbiOrder == null)
                            {
                                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.AddOrder: failed to create order"));
                            }
                            else
                            {
                                TradeMagic.Pats.PatsApi.Order patsOrder = new TradeMagic.Pats.PatsApi.Order();
                                patsOrder.lots = orderDetail.lots;
                                patsOrder.xref = orderDetail.xref;
                                patsOrder.xrefP = orderDetail.xrefP;
                                patsOrder.buyOrSell = orderDetail.buyOrSell;
                                patsOrder.contractDate = orderDetail.contractDate;
                                patsOrder.contractName = orderDetail.contractName;
                                patsOrder.exchangeName = orderDetail.exchangeName;
                                patsOrder.goodTillDate = orderDetail.goodTillDate;
                                patsOrder.linkedOrder = orderDetail.linkedOrder;
                                patsOrder.openOrClose = orderDetail.openOrClose;
                                patsOrder.orderType = orderDetail.orderType;
                                patsOrder.price = orderDetail.price;
                                patsOrder.price2 = orderDetail.price2;
                                patsOrder.traderAccount = orderDetail.traderAccount;
                                Adapter.OrderStub stub = new Adapter.OrderStub(cbiOrder, patsOrder);
                                stub.orderId = PatsApi.ToString(orderDetail.orderId);
                                lock (this.adapter.orderStubs)
                                {
                                    this.adapter.orderStubs.Add(stub);
                                }
                                cbiOrder.AdapterLink = stub;
                            }
                        }
                    }
                }
            }
        }

        private void CancelOrderNow(object order)
        {
            this.adapter.Cancel((iTrading.Core.Kernel.Order) order);
        }

        internal void ContractAdded(ref PatsApi.ContractUpdate update)
        {
        }

        internal void ContractDeleted(ref PatsApi.ContractUpdate update)
        {
        }

        internal void DataDLComplete()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.DataDLComplete");
            }
        }

        internal void Fill(ref PatsApi.FillUpdate update)
        {
            object[] objArray;
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.Fill1: '" + PatsApi.ToString(update.fillId) + "'/'" + PatsApi.ToString(update.orderId) + "'");
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                TradeMagic.Pats.PatsApi.Fill fill;
                TradeMagic.Pats.ErrorCode fillByID = this.adapter.patsApi.GetFillByID(PatsApi.ToString(update.fillId), out fill);
                switch (fillByID)
                {
                    case TradeMagic.Pats.ErrorCode.Success:
                        break;

                    case TradeMagic.Pats.ErrorCode.ErrUnexpected:
                        Trace.WriteLine("WARNING: Pats.Callback.Fill: GetFillByID returned err=" + fillByID + ", trying again ...");
                        for (int i = 0; i < 50; i++)
                        {
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }
                        fillByID = this.adapter.patsApi.GetFillByID(PatsApi.ToString(update.fillId), out fill);
                        if (fillByID == TradeMagic.Pats.ErrorCode.Success)
                        {
                            break;
                        }
                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, fillByID.ToString(), string.Concat(new object[] { "Pats.Callback.Fill.ptGetFillByID: ", PatsApi.ToString(update.fillId), " ", fillByID })));
                        return;

                    default:
                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, fillByID.ToString(), string.Concat(new object[] { "Pats.Callback.Fill.ptGetFillByID: ", PatsApi.ToString(update.fillId), " ", fillByID })));
                        return;
                }
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.Fill2: '", PatsApi.ToString(fill.orderId), "' type=", (FillTypes) fill.fillType }));
                }
                if (fill.fillType != 3)
                {
                    this.HandleFill(fill);
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.Fill: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        internal void ForcedLogout()
        {
            object[] objArray;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.ForcedLogout");
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                this.InitVars();
                this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.ConnectionTerminated, "Connection terminated by host system", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.ForcedLogout: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        private Account GetAccount(char[] traderAccount)
        {
            foreach (Account account in this.Connection.Accounts)
            {
                if (account.Name == PatsApi.ToString(traderAccount))
                {
                    return account;
                }
            }
            this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.GetAccount: unknown account '" + PatsApi.ToString(traderAccount) + "'"));
            return null;
        }

        private double GetAvgPositionPrice(Account account, iTrading.Core.Kernel.Symbol symbol)
        {
            int num = 0;
            double num2 = 0.0;
            int num3 = 0;
            foreach (Execution execution in this.overNight)
            {
                if (execution.Symbol.IsEqual(symbol))
                {
                    if (execution.MarketPosition.Id == MarketPositionId.Long)
                    {
                        num3 += execution.Quantity;
                        num2 += execution.AvgPrice * execution.Quantity;
                        num += execution.Quantity;
                    }
                    else
                    {
                        num3 += execution.Quantity;
                        num2 += execution.AvgPrice * execution.Quantity;
                        num -= execution.Quantity;
                    }
                    break;
                }
            }
            foreach (Execution execution2 in account.Executions)
            {
                if (!execution2.Symbol.IsEqual(symbol))
                {
                    continue;
                }
                if (num == 0)
                {
                    if (execution2.MarketPosition.Id == MarketPositionId.Long)
                    {
                        num3 += execution2.Quantity;
                        num2 += execution2.AvgPrice * execution2.Quantity;
                        num += execution2.Quantity;
                    }
                    else
                    {
                        num3 += execution2.Quantity;
                        num2 += execution2.AvgPrice * execution2.Quantity;
                        num -= execution2.Quantity;
                    }
                    continue;
                }
                if (num > 0)
                {
                    if (execution2.MarketPosition.Id == MarketPositionId.Long)
                    {
                        num3 += execution2.Quantity;
                        num2 += execution2.AvgPrice * execution2.Quantity;
                        num += execution2.Quantity;
                    }
                    else if (num == execution2.Quantity)
                    {
                        num = 0;
                        num2 = 0.0;
                        num3 = 0;
                    }
                    else if (num > execution2.Quantity)
                    {
                        num -= execution2.Quantity;
                    }
                    else
                    {
                        num3 = execution2.Quantity - num;
                        num2 = num3 * execution2.AvgPrice;
                        num = -num3;
                    }
                    continue;
                }
                if (execution2.MarketPosition.Id == MarketPositionId.Short)
                {
                    num3 += execution2.Quantity;
                    num2 += execution2.AvgPrice * execution2.Quantity;
                    num -= execution2.Quantity;
                }
                else
                {
                    if (-num == execution2.Quantity)
                    {
                        num = 0;
                        num2 = 0.0;
                        num3 = 0;
                        continue;
                    }
                    if (-num > execution2.Quantity)
                    {
                        num += execution2.Quantity;
                        continue;
                    }
                    num3 = execution2.Quantity - -num;
                    num2 = num3 * execution2.AvgPrice;
                    num = num3;
                }
            }
            if (num3 != 0)
            {
                return (num2 / ((double) num3));
            }
            return 0.0;
        }

        private TradeMagic.Pats.ErrorCode GetRealizedProfitLoss(Account account, out double pl)
        {
            pl = 0.0;
            lock (account.Positions)
            {
                foreach (Position position in account.Positions)
                {
                    PatsApi.PositionStruct struct2;
                    TradeMagic.Pats.Symbol patsSymbol = this.adapter.GetPatsSymbol(position.Symbol);
                    if (patsSymbol == null)
                    {
                        this.adapter.connection.ProcessEventArgs(new SymbolEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", null));
                        return TradeMagic.Pats.ErrorCode.ErrUnknownContract;
                    }
                    TradeMagic.Pats.ErrorCode code = this.adapter.patsApi.GetContractPosition(PatsApi.ToString(patsSymbol.contract.exchangeName), PatsApi.ToString(patsSymbol.contract.contractName), PatsApi.ToString(patsSymbol.contract.contractDate), account.Name, out struct2);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.GetRealizedProfitLoss.GetOpenPosition: error " + code));
                        return code;
                    }
                    pl += PatsApi.ToDouble(struct2.profit, this.adapter.numberFormatInfo);
                    code = this.adapter.patsApi.GetOpenPosition(PatsApi.ToString(patsSymbol.contract.exchangeName), PatsApi.ToString(patsSymbol.contract.contractName), PatsApi.ToString(patsSymbol.contract.contractDate), account.Name, out struct2);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.GetRealizedProfitLoss.GetOpenPosition: error " + code));
                        return code;
                    }
                    pl -= PatsApi.ToDouble(struct2.profit, this.adapter.numberFormatInfo);
                }
            }
            return TradeMagic.Pats.ErrorCode.Success;
        }

        private void HandleFill(TradeMagic.Pats.PatsApi.Fill fill)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.HandleFill:  order='" + PatsApi.ToString(fill.orderId) + "' fillid='" + PatsApi.ToString(fill.fillId) + "'");
            }
            Account account = this.GetAccount(fill.traderAccount);
            if (account == null)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.InvalidNativeAccount, "", "Pats.CallBack.HandleFill: Account '" + PatsApi.ToString(fill.traderAccount) + "'"));
            }
            else
            {
                iTrading.Core.Kernel.Symbol symbol = this.adapter.Convert(fill.exchangeName, fill.contractName, fill.contractDate);
                if (symbol == null)
                {
                    this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.InvalidNativeSymbol, "", "Pats.CallBack.HandleFill: Contract '" + PatsApi.ToString(fill.contractName) + "/" + PatsApi.ToString(fill.exchangeName) + "/" + PatsApi.ToString(fill.contractDate) + "'"));
                }
                else
                {
                    MarketPosition marketPosition = this.Connection.MarketPositions[MarketPositionId.Long];
                    if (fill.buyOrSell == 'S')
                    {
                        marketPosition = this.Connection.MarketPositions[MarketPositionId.Short];
                    }
                    this.Connection.ProcessEventArgs(new ExecutionUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, PatsApi.ToString(fill.fillId), account, symbol, (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connecting) ? PatsApi.ToDateTime(fill.dateFilled, fill.timeFilled, this.Connection) : DateTime.Now.AddHours((double) -this.adapter.connection.Options.TimerDelayHours), marketPosition, PatsApi.ToString(fill.orderId), fill.lots, PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, fill.price)));
                    Position position2 = null;
                    foreach (Position position3 in account.Positions)
                    {
                        if (symbol.IsEqual(position3.Symbol))
                        {
                            position2 = position3;
                            break;
                        }
                    }
                    double avgPositionPrice = this.GetAvgPositionPrice(account, symbol);
                    int lots = fill.lots;
                    if (position2 == null)
                    {
                        this.Connection.ProcessEventArgs(new PositionUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, account, symbol, marketPosition, lots, this.Connection.Currencies[CurrencyId.Unknown], avgPositionPrice));
                    }
                    else if (marketPosition.Id == position2.MarketPosition.Id)
                    {
                        this.Connection.ProcessEventArgs(new PositionUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Update, position2.Account, position2.Symbol, position2.MarketPosition, lots + position2.Quantity, this.Connection.Currencies[CurrencyId.Unknown], avgPositionPrice));
                    }
                    else if (lots == position2.Quantity)
                    {
                        this.Connection.ProcessEventArgs(new PositionUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Delete, position2.Account, position2.Symbol, position2.MarketPosition, 0, this.Connection.Currencies[CurrencyId.Unknown], avgPositionPrice));
                    }
                    else if (lots < position2.Quantity)
                    {
                        this.Connection.ProcessEventArgs(new PositionUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Update, position2.Account, position2.Symbol, position2.MarketPosition, position2.Quantity - lots, this.Connection.Currencies[CurrencyId.Unknown], avgPositionPrice));
                    }
                    else
                    {
                        this.Connection.ProcessEventArgs(new PositionUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Update, position2.Account, position2.Symbol, marketPosition, lots - position2.Quantity, this.Connection.Currencies[CurrencyId.Unknown], avgPositionPrice));
                    }
                    double pl = 0.0;
                    if (this.GetRealizedProfitLoss(account, out pl) == TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new AccountUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", account, this.Connection.AccountItemTypes[AccountItemTypeId.RealizedProfitLoss], this.Connection.Currencies[CurrencyId.Unknown], pl, DateTime.Now.AddHours((double) -this.adapter.connection.Options.TimerDelayHours)));
                        StringBuilder bpRemaining = new StringBuilder(0x15);
                        TradeMagic.Pats.ErrorCode code = this.adapter.patsApi.BuyingPowerRemaining("", "", "", account.Name, bpRemaining);
                        if (code != TradeMagic.Pats.ErrorCode.Success)
                        {
                            if (this.adapter.connection.Options.Mode.Id != ModeTypeId.Demo)
                            {
                                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.HandleFill.BuyingPowerRemaining: error " + code));
                            }
                        }
                        else
                        {
                            this.Connection.ProcessEventArgs(new AccountUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", account, this.Connection.AccountItemTypes[AccountItemTypeId.BuyingPower], this.Connection.Currencies[CurrencyId.Unknown], Convert.ToDouble(bpRemaining.ToString(), this.adapter.numberFormatInfo), DateTime.Now.AddHours((double) -this.adapter.connection.Options.TimerDelayHours)));
                        }
                    }
                }
            }
        }

        internal void HostLinkStateCallback(ref PatsApi.LinkState state)
        {
            object[] objArray;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.HostLinkStateCallback ", state.newState, "/", state.oldState }));
            }
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "Pats";
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.HostLinkStateCallback: new state ", state.newState }));
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                if (this.expectConnected)
                {
                    if ((state.newState == 4) && !this.hostReconnecting)
                    {
                        this.hostReconnecting = true;
                        this.adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "Unexpected disconnect from server. Retrying connect ...", ConnectionStatusId.ConnectionLost, this.adapter.connection.SecondaryConnectionStatusId, 0, ""));
                    }
                    else if ((state.newState == 3) && this.hostReconnecting)
                    {
                        this.hostReconnecting = false;
                        this.adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Connected, this.adapter.connection.SecondaryConnectionStatusId, 0, ""));
                    }
                }
                else if (state.newState == 3)
                {
                    PatsApi.Logon logon = new PatsApi.Logon(this.Connection.Options.User.ToUpper(), this.Connection.Options.Password.ToUpper(), ((PatsOptions) this.Connection.Options).NewPassword.ToUpper());
                    TradeMagic.Pats.ErrorCode code = this.adapter.patsApi.LogOn(ref logon);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Connect.ptLogon: error " + code, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.HostLinkStateCallback: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        internal void Init()
        {
            bool flag = true;
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.AustralianDollar, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.BritishPound, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.CanadianDollar, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.Euro, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.HongKongDollar, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.JapaneseYen, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.SwissFranc, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.Unknown, ""));
            this.Connection.ProcessEventArgs(new CurrencyEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", CurrencyId.UsDollar, ""));
            this.Connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", AccountItemTypeId.BuyingPower, ""));
            this.Connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", AccountItemTypeId.RealizedProfitLoss, ""));
            this.Connection.ProcessEventArgs(new ActionTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.Buy, "B"));
            this.Connection.ProcessEventArgs(new ActionTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.BuyToCover, "B"));
            this.Connection.ProcessEventArgs(new ActionTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.Sell, "S"));
            this.Connection.ProcessEventArgs(new ActionTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ActionTypeId.SellShort, "S"));
            if (this.adapter.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                this.Connection.CreateSimulationAccount(this.Connection.SimulationAccountName, new SimulationAccountOptions());
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.ECbot, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Globex, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Belfox, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Monep, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Eurex, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.EurexUS, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Idem, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Liffe, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Me, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Meff, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Nybot, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Nymex, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.One, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Sfe, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Sgx, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Default, ""));
                this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Tse, ""));
                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Market, ""));
                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Limit, ""));
                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Stop, ""));
                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.StopLimit, ""));
            }
            else
            {
                int num;
                TradeMagic.Pats.ErrorCode trader = this.adapter.patsApi.CountTraders(out num);
                if (trader != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.CountTraders: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                for (int i = 0; i < num; i++)
                {
                    PatsApi.TraderAccount account;
                    trader = this.adapter.patsApi.GetTrader(i, out account);
                    if (trader != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.GetTrader: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    if (account.tradable == 'Y')
                    {
                        this.Connection.ProcessEventArgs(new AccountEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(account.traderAccount), null));
                    }
                }
                if (this.Connection.Accounts.Count == 0)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, "No tradable account found", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                trader = this.adapter.patsApi.SetPriceAgeCounter(0);
                if (trader != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.ptSetPriceAgeCounter: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                if (this.Connection.Options.Mode.Id != ModeTypeId.Demo)
                {
                    int num3;
                    int num4;
                    trader = this.adapter.patsApi.EnabledFunctionality(out num3, out num4);
                    if (trader != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.FunctionalityEnabled: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    if ((num3 & 1) != 1)
                    {
                        flag = false;
                    }
                }
                trader = this.adapter.patsApi.CountExchanges(out num);
                if (trader != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.ptCountExchanges: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                if (num == 0)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, "No exchange found", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                for (int j = 0; j < num; j++)
                {
                    PatsApi.Exchange exchange;
                    trader = this.adapter.patsApi.GetExchange(j, out exchange);
                    if (trader != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.ptGetExchange: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    string mapId = PatsApi.ToString(exchange.name);
                    if (mapId.ToUpper() == "ECBOT")
                    {
                        this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.ECbot, mapId));
                    }
                    else if (mapId.ToUpper() == "EUREX")
                    {
                        this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Eurex, mapId));
                    }
                    else if (mapId.ToUpper() == "MEFF")
                    {
                        this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Meff, mapId));
                    }
                }
                for (int k = 0; k < num; k++)
                {
                    PatsApi.Exchange exchange2;
                    trader = this.adapter.patsApi.GetExchange(k, out exchange2);
                    if (trader != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.ptGetExchange: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    string str2 = PatsApi.ToString(exchange2.name);
                    if (((str2.ToUpper() != "ECBOT") && (str2.ToUpper() != "EUREX")) && (str2.ToUpper() != "MEFF"))
                    {
                        if ((str2.ToUpper() == "CBOT") && (this.Connection.Exchanges[ExchangeId.ECbot] == null))
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.ECbot, str2));
                        }
                        else if (str2.ToUpper() == "CME")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Globex, str2));
                        }
                        else if (str2.ToUpper() == "ENXT_BRU")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Belfox, str2));
                        }
                        else if (str2.ToUpper() == "ENXT_PAR")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Monep, str2));
                        }
                        else if ((str2.ToUpper() == "XEUREX") && (this.Connection.Exchanges[ExchangeId.Eurex] == null))
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Eurex, str2));
                        }
                        else if (str2.ToUpper() == "EUREXUS")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.EurexUS, str2));
                        }
                        else if (str2.ToUpper() == "IDEM")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Idem, str2));
                        }
                        else if (str2.ToUpper() == "LIFFE")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Liffe, str2));
                        }
                        else if (str2.ToUpper() == "ME")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Me, str2));
                        }
                        else if ((str2.ToUpper() == "MEFFRV") && (this.Connection.Exchanges[ExchangeId.Meff] == null))
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Meff, str2));
                        }
                        else if (str2.ToUpper() == "NYBOT")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Nybot, str2));
                        }
                        else if (str2.ToUpper() == "NYMEX")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Nymex, str2));
                        }
                        else if (str2.ToUpper() == "ONECHICAGO")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.One, str2));
                        }
                        else if (str2.ToUpper() == "SFE")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Sfe, str2));
                        }
                        else if (str2.ToUpper() == "SGX")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Sgx, str2));
                        }
                        else if ((str2.ToUpper() == "SIM") && (this.Connection.Exchanges[ExchangeId.Default] == null))
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Default, str2));
                        }
                        else if (str2.ToUpper() == "TSE")
                        {
                            this.Connection.ProcessEventArgs(new ExchangeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ExchangeId.Tse, str2));
                        }
                        else
                        {
                            Trace.WriteLine("WARNING: exchange '" + str2 + "' not supported by TradeMagic");
                        }
                    }
                }
                trader = this.adapter.patsApi.CountOrderTypes(out num);
                if (trader != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.CountOrderTypes: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                if (num == 0)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, "No order types found", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return;
                }
                for (int m = 0; m < num; m++)
                {
                    PatsApi.OrderType type;
                    trader = this.adapter.patsApi.GetOrderType(m, out type);
                    if (trader != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.orderType: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    string str3 = PatsApi.ToString(type.exchangeName);
                    Hashtable hashtable = (Hashtable) this.adapter.exchange2OrderTypes[str3];
                    if (hashtable == null)
                    {
                        this.adapter.exchange2OrderTypes[str3] = hashtable = new Hashtable();
                    }
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.Init1: exchange='", str3, "' ordertype=", (TradeMagic.Pats.OrderType) type.orderTypeId }));
                    }
                    switch (((TradeMagic.Pats.OrderType) type.orderTypeId))
                    {
                        case TradeMagic.Pats.OrderType.OrderTypeMarket:
                            hashtable[0] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.Market] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Market, PatsApi.ToString(type.orderType)));
                            }
                            break;

                        case TradeMagic.Pats.OrderType.OrderTypeLimit:
                            hashtable[10] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.Limit] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Limit, PatsApi.ToString(type.orderType)));
                            }
                            break;

                        case TradeMagic.Pats.OrderType.OrderTypeStop:
                            hashtable[20] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.Stop] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Stop, PatsApi.ToString(type.orderType)));
                            }
                            break;

                        case TradeMagic.Pats.OrderType.OrderTypeStopLoss:
                            hashtable[30] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.StopLimit] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.StopLimit, PatsApi.ToString(type.orderType)));
                            }
                            break;

                        case TradeMagic.Pats.OrderType.GTCMarket:
                            hashtable[1] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.Market] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Market, PatsApi.ToString(type.orderType)));
                            }
                            break;

                        case TradeMagic.Pats.OrderType.GTCLimit:
                            hashtable[11] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.Limit] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Limit, PatsApi.ToString(type.orderType)));
                            }
                            break;

                        case TradeMagic.Pats.OrderType.GTCStop:
                            hashtable[0x15] = PatsApi.ToString(type.orderType);
                            if (this.Connection.OrderTypes[OrderTypeId.Stop] == null)
                            {
                                this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Stop, PatsApi.ToString(type.orderType)));
                            }
                            break;
                    }
                }
                for (int n = 0; n < num; n++)
                {
                    PatsApi.OrderType type2;
                    trader = this.adapter.patsApi.GetOrderType(n, out type2);
                    if (trader != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.Init.orderType: error " + trader, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    string str4 = PatsApi.ToString(type2.exchangeName);
                    Hashtable hashtable2 = (Hashtable) this.adapter.exchange2OrderTypes[str4];
                    if (hashtable2 == null)
                    {
                        this.adapter.exchange2OrderTypes[str4] = hashtable2 = new Hashtable();
                    }
                    switch (type2.orderTypeId)
                    {
                        case 6:
                            if (hashtable2[20] == null)
                            {
                                hashtable2[20] = PatsApi.ToString(type2.orderType);
                                if (this.Connection.OrderTypes[OrderTypeId.Stop] == null)
                                {
                                    this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.Stop, PatsApi.ToString(type2.orderType)));
                                }
                            }
                            break;

                        case 7:
                            if (hashtable2[30] == null)
                            {
                                hashtable2[30] = PatsApi.ToString(type2.orderType);
                                if (this.Connection.OrderTypes[OrderTypeId.StopLimit] == null)
                                {
                                    this.Connection.ProcessEventArgs(new OrderTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderTypeId.StopLimit, PatsApi.ToString(type2.orderType)));
                                }
                            }
                            break;
                    }
                }
            }
            this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 999.0));
            this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.Order, 0.0));
            this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.OrderChange, 0.0));
            this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.SymbolLookup, 0.0));
            this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.SynchronousSymbolLookup, 0.0));
            if (flag)
            {
                this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MarketDepth, 0.0));
                this.Connection.ProcessEventArgs(new FeatureTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", FeatureTypeId.MaxMarketDepthStreams, 999.0));
            }
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Ask, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Bid, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.DailyHigh, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.DailyLow, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.DailyVolume, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Last, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.LastClose, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Opening, ""));
            this.Connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketDataTypeId.Unknown, ""));
            this.Connection.ProcessEventArgs(new MarketPositionEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketPositionId.Long, ""));
            this.Connection.ProcessEventArgs(new MarketPositionEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketPositionId.Short, ""));
            this.Connection.ProcessEventArgs(new MarketPositionEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", MarketPositionId.Unknown, ""));
            this.Connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", NewsItemTypeId.Default, ""));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Cancelled, "5"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Filled, "8"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Initialized, ""));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PartFilled, "7"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PendingCancel, "9"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PendingSubmit, "1"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.PendingChange, "10"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Rejected, "4"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Accepted, "2"));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Unknown, ""));
            this.Connection.ProcessEventArgs(new OrderStateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", OrderStateId.Working, "3"));
            this.Connection.ProcessEventArgs(new SymbolTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", SymbolTypeId.Unknown, ""));
            this.Connection.ProcessEventArgs(new SymbolTypeEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", SymbolTypeId.Future, "FUT"));
            this.Connection.ProcessEventArgs(new TimeInForceEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", TimeInForceId.Day, ""));
            this.Connection.ProcessEventArgs(new TimeInForceEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", TimeInForceId.Gtc, ""));
        }

        internal void InitVars()
        {
            this.expectConnected = false;
            this.hostReconnecting = false;
            this.priceReconnecting = false;
        }

        internal void LogonStatus()
        {
            object[] objArray;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.LogonStatus");
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                TradeMagic.Pats.PatsApi.LogonStatus status;
                this.adapter.patsApi.GetLogonStatus(out status);
                if (status.status == 9)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, "Unable to logon: user already is logged in elsewhere", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                else if (status.status != 1)
                {
                    this.adapter.patsApi.Disconnect();
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, string.Concat(new object[] { "Unable to login, error ", (int) status.status, ": ", PatsApi.ToString(status.reason) }), ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.LogonStatus: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        internal void MessageCallback(string msgId)
        {
            object[] objArray;
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                PatsApi.Message message;
                TradeMagic.Pats.ErrorCode usrMsgByID = this.adapter.patsApi.GetUsrMsgByID(msgId, out message);
                if (usrMsgByID != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.MessageCallback.ptGetUsrMsgByID: error " + usrMsgByID));
                }
                else
                {
                    usrMsgByID = this.adapter.patsApi.AcknowledgeUsrMsg(msgId);
                    if (usrMsgByID != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.MessageCallback.ptAcknowledgeUsrMsg: error " + usrMsgByID));
                    }
                    else
                    {
                        if (message.isAlert == 'Y')
                        {
                            this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.CriticalProviderMessage, PatsApi.ToString(message.msgId), PatsApi.ToString(message.msgText)));
                        }
                        iTrading.Core.Kernel.Exchange exchange = this.Connection.Exchanges[ExchangeId.Default];
                        NewsItemType itemType = this.Connection.NewsItemTypes[NewsItemTypeId.Default];
                        if (exchange == null)
                        {
                            foreach (iTrading.Core.Kernel.Exchange exchange2 in this.Connection.Exchanges.Values)
                            {
                                exchange = exchange2;
                            }
                        }
                        Trace.Assert(exchange != null, "Pats.Callback.MessageCallback0");
                        Trace.Assert(itemType != null, "Pats.Callback.MessageCallback1");
                        this.Connection.ProcessEventArgs(new NewsEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", "TM_" + Guid.NewGuid().ToString(), itemType, this.Connection.Now, PatsApi.ToString(message.msgText), PatsApi.ToString(message.msgText)));
                    }
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.MessageCallback: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        internal void Order(ref PatsApi.OrderUpdate update)
        {
            object[] objArray;
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.Order1: " + PatsApi.ToString(update.orderId) + "/" + PatsApi.ToString(update.oldOrderId));
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                PatsApi.OrderDetail detail;
                TradeMagic.Pats.ErrorCode orderByID;
                Adapter.OrderStub stub = null;
                lock (this.adapter.syncOrderStatusEvent)
                {
                    orderByID = this.adapter.patsApi.GetOrderByID(PatsApi.ToString(update.orderId), out detail);
                }
                TradeMagic.Pats.OrderState status = (TradeMagic.Pats.OrderState) detail.status;
                if (status != TradeMagic.Pats.OrderState.Queued)
                {
                    ArrayList list2;
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.Order2: ", PatsApi.ToString(update.orderId), ", status ", status }));
                    }
                    lock ((list2 = this.adapter.orderStubs))
                    {
                        string str = PatsApi.ToString(update.oldOrderId);
                        foreach (Adapter.OrderStub stub2 in this.adapter.orderStubs)
                        {
                            if (stub2.orderId == str)
                            {
                                stub = stub2;
                            }
                        }
                        if (stub == null)
                        {
                            str = PatsApi.ToString(update.orderId);
                            foreach (Adapter.OrderStub stub3 in this.adapter.orderStubs)
                            {
                                if (stub3.orderId == str)
                                {
                                    stub = stub3;
                                }
                            }
                        }
                    }
                    if ((orderByID == TradeMagic.Pats.ErrorCode.Success) && (stub == null))
                    {
                        switch (status)
                        {
                            case TradeMagic.Pats.OrderState.Cancelled:
                            case TradeMagic.Pats.OrderState.CancelHeldOrder:
                            case TradeMagic.Pats.OrderState.BalCancelled:
                            case TradeMagic.Pats.OrderState.Filled:
                            case TradeMagic.Pats.OrderState.Rejected:
                                return;
                        }
                        if ((update.orderId[0] == 'N') || (update.orderId[0] == 'S'))
                        {
                            this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", string.Concat(new object[] { "Pats.Callback.Order: unknown order ", PatsApi.ToString(update.oldOrderId), ", status ", status })));
                        }
                        else
                        {
                            this.AddOrder(detail);
                        }
                    }
                    else if ((orderByID != TradeMagic.Pats.ErrorCode.ErrUnknownOrder) || ((update.orderId[0] != 'N') && (update.orderId[0] != 'S')))
                    {
                        if (orderByID != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.OrderRejected, PatsApi.ToString(detail.nonExecReason), stub.orderId, stub.cbiOrder.LimitPrice, stub.cbiOrder.StopPrice, stub.cbiOrder.Quantity, stub.cbiOrder.AvgFillPrice, stub.cbiOrder.Filled, stub.cbiOrder.OrderState, this.Connection.Now));
                        }
                        else
                        {
                            if (Globals.TraceSwitch.Order)
                            {
                                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.Order3: " + PatsApi.ToString(update.orderId) + ", found " + stub.cbiOrder.OrderId);
                            }
                            stub.orderId = PatsApi.ToString(update.orderId);
                            int count = 0;
                            int index = detail.index;
                            orderByID = this.adapter.patsApi.CountOrderHistory(index, out count);
                            if (orderByID != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", string.Concat(new object[] { "Pats.Callback.Order: ptCountOrderHistory error ", PatsApi.ToString(update.oldOrderId), ", status ", status, " ", orderByID })));
                            }
                            else
                            {
                                if (Globals.TraceSwitch.Order)
                                {
                                    Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.Order4: '", PatsApi.ToString(detail.orderId), "', status=", status, ", ", count, "/", stub.lastCountOrderHistory }));
                                }
                                for (int i = (count - stub.lastCountOrderHistory) - 1; i >= 0; i--)
                                {
                                    orderByID = this.adapter.patsApi.GetOrderHistory(index, i, out detail);
                                    if (orderByID != TradeMagic.Pats.ErrorCode.Success)
                                    {
                                        this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", string.Concat(new object[] { "Pats.Callback.Order: ptGetOrderHistory error ", PatsApi.ToString(update.oldOrderId), ", status ", status, " ", orderByID })));
                                        return;
                                    }
                                    status = (TradeMagic.Pats.OrderState) detail.status;
                                    if (Globals.TraceSwitch.Order)
                                    {
                                        Trace.WriteLine(string.Concat(new object[] { 
                                            "(", this.adapter.connection.IdPlus, ") Pats.Callback.Order5:  pos=", i, " status=", status, " orderId='", PatsApi.ToString(detail.orderId), "' orderType='", PatsApi.ToString(detail.orderType), "' contractName='", PatsApi.ToString(detail.contractName), "' price=", PatsApi.ToString(detail.price), "/", PatsApi.CharsToPrice(this.adapter.numberFormatInfo, stub.cbiOrder.Symbol, detail.price), 
                                            " price2=", PatsApi.ToString(detail.price2), "/", PatsApi.CharsToPrice(this.adapter.numberFormatInfo, stub.cbiOrder.Symbol, detail.price2), " lots=", detail.lots, " amountFilled=", detail.amountFilled, " averagePrice=", PatsApi.ToString(detail.averagePrice), "/", PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), " status=", (TradeMagic.Pats.OrderState) detail.status, " nonExecReason='", PatsApi.ToString(detail.nonExecReason), 
                                            "'"
                                         }));
                                    }
                                    if (status == TradeMagic.Pats.OrderState.BalCancelled)
                                    {
                                        status = TradeMagic.Pats.OrderState.Cancelled;
                                    }
                                    else if (((status == TradeMagic.Pats.OrderState.UnconfirmedFilled) || (status == TradeMagic.Pats.OrderState.UnconfirmedPartFilled)) || (this.Connection.OrderStates[stub.cbiOrder.OrderState.Id].MapId == ((int) status).ToString()))
                                    {
                                        continue;
                                    }
                                    iTrading.Core.Kernel.OrderType currentOrderType = this.adapter.GetCurrentOrderType(stub.cbiOrder);
                                    if (currentOrderType == null)
                                    {
                                        if (Globals.TraceSwitch.Order)
                                        {
                                            Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.Order4a: GetCurrentOrderType failed");
                                        }
                                        return;
                                    }
                                    double limitPrice = stub.cbiOrder.LimitPrice;
                                    int lots = detail.lots;
                                    double num6 = PatsApi.CharsToPrice(this.adapter.numberFormatInfo, stub.cbiOrder.Symbol, detail.price);
                                    double num7 = PatsApi.CharsToPrice(this.adapter.numberFormatInfo, stub.cbiOrder.Symbol, detail.price2);
                                    double stopPrice = stub.cbiOrder.StopPrice;
                                    if (currentOrderType.Id == OrderTypeId.Limit)
                                    {
                                        limitPrice = (num6 != 0.0) ? num6 : limitPrice;
                                    }
                                    else if (currentOrderType.Id == OrderTypeId.Stop)
                                    {
                                        stopPrice = (num6 != 0.0) ? num6 : stopPrice;
                                    }
                                    else if (currentOrderType.Id == OrderTypeId.StopLimit)
                                    {
                                        limitPrice = (num7 != 0.0) ? num7 : limitPrice;
                                        stopPrice = (num6 != 0.0) ? num6 : stopPrice;
                                    }
                                    switch (status)
                                    {
                                        case TradeMagic.Pats.OrderState.Sent:
                                            goto Label_1068;

                                        case TradeMagic.Pats.OrderState.Working:
                                        {
                                            if ((stub.cbiOrder.OrderState.Id == OrderStateId.PendingSubmit) || (stub.cbiOrder.OrderState.Id == OrderStateId.PendingChange))
                                            {
                                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Accepted], this.Connection.Now));
                                            }
                                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Working], this.Connection.Now));
                                            continue;
                                        }
                                        case TradeMagic.Pats.OrderState.Rejected:
                                            if ((stub.cbiOrder.OrderState.Id == OrderStateId.PendingSubmit) || (stub.cbiOrder.OrderState.Id == OrderStateId.PendingChange))
                                            {
                                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Accepted], this.Connection.Now));
                                            }
                                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.OrderRejected, PatsApi.ToString(detail.nonExecReason), PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Rejected], this.Connection.Now));
                                            lock ((list2 = this.adapter.orderStubs))
                                            {
                                                this.adapter.orderStubs.Remove(stub);
                                                continue;
                                            }
                                            goto Label_1068;

                                        case TradeMagic.Pats.OrderState.Cancelled:
                                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, PatsApi.ToString(detail.nonExecReason), PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Cancelled], this.Connection.Now));
                                            lock ((list2 = this.adapter.orderStubs))
                                            {
                                                this.adapter.orderStubs.Remove(stub);
                                                continue;
                                            }
                                            break;

                                        case TradeMagic.Pats.OrderState.BalCancelled:
                                        case TradeMagic.Pats.OrderState.UnconfirmedFilled:
                                        case TradeMagic.Pats.OrderState.UnconfirmedPartFilled:
                                        {
                                            continue;
                                        }
                                        case TradeMagic.Pats.OrderState.PartFilled:
                                        {
                                            if ((stub.cbiOrder.OrderState.Id == OrderStateId.PendingSubmit) || (stub.cbiOrder.OrderState.Id == OrderStateId.PendingChange))
                                            {
                                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Accepted], this.Connection.Now));
                                            }
                                            if (stub.cbiOrder.OrderState.Id == OrderStateId.Accepted)
                                            {
                                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Working], this.Connection.Now));
                                            }
                                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.PartFilled], this.Connection.Now));
                                            continue;
                                        }
                                        case TradeMagic.Pats.OrderState.Filled:
                                            if ((stub.cbiOrder.OrderState.Id == OrderStateId.PendingSubmit) || (stub.cbiOrder.OrderState.Id == OrderStateId.PendingChange))
                                            {
                                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Accepted], this.Connection.Now));
                                            }
                                            if (stub.cbiOrder.OrderState.Id == OrderStateId.Accepted)
                                            {
                                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Working], this.Connection.Now));
                                            }
                                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Filled], this.Connection.Now));
                                            lock ((list2 = this.adapter.orderStubs))
                                            {
                                                this.adapter.orderStubs.Remove(stub);
                                                continue;
                                            }
                                            goto Label_0D48;

                                        case TradeMagic.Pats.OrderState.CancelPending:
                                            goto Label_0B2D;

                                        case TradeMagic.Pats.OrderState.AmendPending:
                                        {
                                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.PendingChange], this.Connection.Now));
                                            continue;
                                        }
                                        case TradeMagic.Pats.OrderState.HeldOrder:
                                            goto Label_0D48;

                                        case TradeMagic.Pats.OrderState.CancelHeldOrder:
                                            break;

                                        default:
                                        {
                                            continue;
                                        }
                                    }
                                    this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Cancelled], this.Connection.Now));
                                    lock ((list2 = this.adapter.orderStubs))
                                    {
                                        this.adapter.orderStubs.Remove(stub);
                                        continue;
                                    }
                                Label_0B2D:
                                    this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.PendingCancel], this.Connection.Now));
                                    continue;
                                Label_0D48:
                                    this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Accepted], this.Connection.Now));
                                    continue;
                                Label_1068:
                                    this.Connection.ProcessEventArgs(new OrderStatusEventArgs(stub.cbiOrder, iTrading.Core.Kernel.ErrorCode.NoError, "", PatsApi.ToString(update.orderId), limitPrice, stopPrice, lots, PatsApi.ToDouble(detail.averagePrice, this.adapter.numberFormatInfo), detail.amountFilled, this.Connection.OrderStates[OrderStateId.Accepted], this.Connection.Now));
                                }
                                lock ((list2 = this.adapter.orders2Cancel))
                                {
                                    if (this.adapter.orders2Cancel.Count > 0)
                                    {
                                        ArrayList list = this.adapter.orders2Cancel;
                                        this.adapter.orders2Cancel = new ArrayList();
                                        foreach (Adapter.OrderStub stub4 in list)
                                        {
                                            if (Globals.TraceSwitch.Order)
                                            {
                                                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.Order6: " + stub.cbiOrder.ToString());
                                            }
                                            this.adapter.connection.SynchronizeInvoke.Invoke(new Adapter.Process(this.CancelOrderNow), new object[] { stub4.cbiOrder });
                                        }
                                    }
                                }
                                stub.lastCountOrderHistory = count;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.Order: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        internal void PriceLinkStateCallback(ref PatsApi.LinkState state)
        {
            object[] objArray;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.PriceLinkStateCallback: ", state.newState, "/", state.oldState }));
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                if (this.expectConnected)
                {
                    if ((state.newState == 4) && !this.priceReconnecting)
                    {
                        this.priceReconnecting = true;
                        this.adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "Unexpected disconnect from server. Retrying connect ...", this.adapter.connection.ConnectionStatusId, ConnectionStatusId.ConnectionLost, 0, ""));
                    }
                    else if ((state.newState == 3) && this.priceReconnecting)
                    {
                        this.priceReconnecting = false;
                        this.adapter.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", this.adapter.connection.ConnectionStatusId, ConnectionStatusId.Connected, 0, ""));
                    }
                }
                else if (state.newState == 3)
                {
                    this.StartUp();
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.PriceLinkStateCallback: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        internal void PriceUpdate(ref PatsApi.ContractUpdate update)
        {
            object[] objArray;
            if (Globals.TraceSwitch.MarketData || Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.PriceUpdate0: " + PatsApi.ToString(update.contractName) + "/" + PatsApi.ToString(update.contractDate) + "/" + PatsApi.ToString(update.exchangeName));
            }
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                if (this.typeAsk == null)
                {
                    this.typeAsk = this.adapter.connection.MarketDataTypes[MarketDataTypeId.Ask];
                    this.typeBid = this.adapter.connection.MarketDataTypes[MarketDataTypeId.Bid];
                    this.typeDailyHigh = this.adapter.connection.MarketDataTypes[MarketDataTypeId.DailyHigh];
                    this.typeDailyLow = this.adapter.connection.MarketDataTypes[MarketDataTypeId.DailyLow];
                    this.typeDailyVolume = this.adapter.connection.MarketDataTypes[MarketDataTypeId.DailyVolume];
                    this.typeLast = this.adapter.connection.MarketDataTypes[MarketDataTypeId.Last];
                    this.typeLastClose = this.adapter.connection.MarketDataTypes[MarketDataTypeId.LastClose];
                    this.typeOpening = this.adapter.connection.MarketDataTypes[MarketDataTypeId.Opening];
                }
                for (int i = 0; i < this.adapter.subscribedPriceData.Count; i++)
                {
                    Adapter.PriceStub stub = (Adapter.PriceStub) this.adapter.subscribedPriceData[i];
                    if (stub.symbol.IsEqual(update))
                    {
                        PatsApi.PriceDetailed offer;
                        PatsApi.Price price;
                        DateTime now = this.Connection.Now;
                        lock (this.marketDataBuffer)
                        {
                            TradeMagic.Pats.ErrorCode code = this.adapter.patsApi.GetPrice(stub.symbol.index, this.marketDataBuffer);
                            if (code != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.MessageCallback.PriceUpdate2: error " + code));
                                return;
                            }
                            if (Globals.TraceSwitch.Native)
                            {
                                StringBuilder builder = new StringBuilder();
                                builder.Append("(" + this.adapter.connection.IdPlus + ") Pats.Callback.PriceUpdate2: ");
                                for (int j = 0; j < this.marketDataBuffer.Length; j++)
                                {
                                    if (j != 0)
                                    {
                                        builder.Append("-");
                                    }
                                    builder.Append(this.marketDataBuffer[j].ToString("X", CultureInfo.CurrentCulture));
                                }
                                Trace.WriteLine(builder.ToString());
                            }
                            price = new PatsApi.Price(this.adapter, stub.cbiSymbol, this.marketDataBuffer);
                        }
                        uint changeMask = price.ChangeMask;
                        if (Globals.TraceSwitch.MarketData || Globals.TraceSwitch.MarketDepth)
                        {
                            Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") Pats.Callback.PriceUpdate3: symbol='", stub.cbiSymbol.MarketData.Symbol.FullName, "' changeMask=", changeMask }));
                        }
                        bool flag = false;
                        if (stub.marketData)
                        {
                            if ((changeMask == 0) || ((changeMask & 2) != 0))
                            {
                                offer = price.Offer;
                                if ((offer.price > 0.0) && ((((changeMask != 0) || (stub.cbiSymbol.MarketData.Ask == null)) || (stub.cbiSymbol.MarketData.Ask.Price != offer.price)) || (stub.cbiSymbol.MarketData.Ask.Volume != ((offer.volume < 0) ? 0 : offer.volume))))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeAsk, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 1) != 0))
                            {
                                offer = price.Bid;
                                if ((offer.price > 0.0) && ((((changeMask != 0) || (stub.cbiSymbol.MarketData.Bid == null)) || (stub.cbiSymbol.MarketData.Bid.Price != offer.price)) || (stub.cbiSymbol.MarketData.Bid.Volume != ((offer.volume < 0) ? 0 : offer.volume))))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeBid, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x80) != 0))
                            {
                                offer = price.High;
                                if ((offer.price > 0.0) && (((changeMask != 0) || (stub.cbiSymbol.MarketData.DailyHigh == null)) || (stub.cbiSymbol.MarketData.DailyHigh.Price != offer.price)))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeDailyHigh, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x100) != 0))
                            {
                                offer = price.Low;
                                if ((offer.price > 0.0) && (((changeMask != 0) || (stub.cbiSymbol.MarketData.DailyLow == null)) || (stub.cbiSymbol.MarketData.DailyLow.Price != offer.price)))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeDailyLow, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x40) != 0))
                            {
                                offer = price.Total;
                                if ((offer.volume > 0) && (((changeMask != 0) || (stub.cbiSymbol.MarketData.DailyVolume == null)) || (stub.cbiSymbol.MarketData.DailyVolume.Volume != offer.volume)))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeDailyVolume, (offer.price < 0.0) ? 0.0 : offer.price, offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x400) != 0))
                            {
                                offer = price.Closing;
                                if ((offer.price > 0.0) && ((((changeMask != 0) || (stub.cbiSymbol.MarketData.LastClose == null)) || (stub.cbiSymbol.MarketData.LastClose.Price != offer.price)) || (stub.cbiSymbol.MarketData.LastClose.Volume != ((offer.volume < 0) ? 0 : offer.volume))))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeLastClose, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x200) != 0))
                            {
                                offer = price.Opening;
                                if ((offer.price > 0.0) && ((((changeMask != 0) || (stub.cbiSymbol.MarketData.Opening == null)) || (stub.cbiSymbol.MarketData.Opening.Price != offer.price)) || (stub.cbiSymbol.MarketData.Opening.Volume != ((offer.volume < 0) ? 0 : offer.volume))))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeOpening, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x20) != 0))
                            {
                                offer = price.GetLast(0);
                                if ((offer.price > 0.0) && ((((changeMask != 0) || (stub.cbiSymbol.MarketData.Last == null)) || (stub.cbiSymbol.MarketData.Last.Price != offer.price)) || (stub.cbiSymbol.MarketData.Last.Volume != ((offer.volume < 0) ? 0 : offer.volume))))
                                {
                                    this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, this.typeLast, offer.price, (offer.volume < 0) ? 0 : offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                    flag = true;
                                }
                            }
                        }
                        if (stub.marketDepth)
                        {
                            MarketDepthRowCollection rows;
                            if ((changeMask == 0) || ((changeMask & 0x800) != 0))
                            {
                                lock ((rows = stub.cbiSymbol.MarketDepth.Bid))
                                {
                                    for (int k = 0; k < 20; k++)
                                    {
                                        offer = price.GetBidDOM(k);
                                        Operation insert = Operation.Update;
                                        if (k >= stub.cbiSymbol.MarketDepth.Bid.Count)
                                        {
                                            if (offer.price == 0.0)
                                            {
                                                continue;
                                            }
                                            insert = Operation.Insert;
                                        }
                                        else if (offer.price == 0.0)
                                        {
                                            insert = Operation.Delete;
                                        }
                                        else
                                        {
                                            MarketDepthRow row = stub.cbiSymbol.MarketDepth.Bid[k];
                                            if ((offer.price == row.Price) && (offer.volume == row.Volume))
                                            {
                                                continue;
                                            }
                                        }
                                        this.Connection.ProcessEventArgs(new MarketDepthEventArgs(stub.cbiSymbol.MarketDepth, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, k, this.marketMaker, insert, this.typeBid, offer.price, offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                        flag = true;
                                    }
                                }
                            }
                            if ((changeMask == 0) || ((changeMask & 0x1000) != 0))
                            {
                                lock ((rows = stub.cbiSymbol.MarketDepth.Ask))
                                {
                                    for (int m = 0; m < 20; m++)
                                    {
                                        offer = price.GetOfferDOM(m);
                                        Operation operation = Operation.Update;
                                        if (m >= stub.cbiSymbol.MarketDepth.Ask.Count)
                                        {
                                            if (offer.price == 0.0)
                                            {
                                                continue;
                                            }
                                            operation = Operation.Insert;
                                        }
                                        else if (offer.price == 0.0)
                                        {
                                            operation = Operation.Delete;
                                        }
                                        else
                                        {
                                            MarketDepthRow row2 = stub.cbiSymbol.MarketDepth.Ask[m];
                                            if ((offer.price == row2.Price) && (offer.volume == row2.Volume))
                                            {
                                                continue;
                                            }
                                        }
                                        this.Connection.ProcessEventArgs(new MarketDepthEventArgs(stub.cbiSymbol.MarketDepth, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.cbiSymbol, m, this.marketMaker, operation, this.typeAsk, offer.price, offer.volume, new DateTime(now.Year, now.Month, now.Day, offer.hour, offer.minute, offer.second)));
                                        flag = true;
                                    }
                                }
                            }
                        }
                        if (Globals.TraceSwitch.Native)
                        {
                            string message = "";
                            object obj2 = message;
                            message = string.Concat(new object[] { 
                                obj2, "(", this.adapter.connection.IdPlus, ") Pats.Callback.PriceUpdate4: symbol=", stub.cbiSymbol.FullName, " changeMask=", changeMask, " changeSeen=", flag, " marketData=", stub.marketData, " marketDepth=", stub.marketDepth, " bid=", price.Bid.ToString(), " ask=", 
                                price.Offer.ToString(), " last=", price.GetLast(0).ToString(), " opening=", price.Opening.ToString(), " dailyHigh=", price.High.ToString(), " dailyLow=", price.Low.ToString(), " lastClose=", price.Closing.ToString(), " dailyVolume=", price.Total.ToString(), " bid=", price.Bid.ToString()
                             });
                            for (int n = 0; n < 20; n++)
                            {
                                obj2 = message;
                                message = string.Concat(new object[] { obj2, " bid", n, "=", price.GetBidDOM(n).ToString() });
                            }
                            for (int num7 = 0; num7 < 20; num7++)
                            {
                                obj2 = message;
                                message = string.Concat(new object[] { obj2, " ask", num7, "=", price.GetOfferDOM(num7).ToString() });
                            }
                            Trace.WriteLine(message);
                        }
                        if (((changeMask == 0) && !flag) && (stub.marketData || stub.marketDepth))
                        {
                            if (((stub.lastNonZeroMaskEvent != Globals.MinDate) && (((PatsOptions) this.adapter.connection.Options).MaxChangeMaskZeroSecs != 0)) && ((stub.cbiSymbol.MarketData.Ask != null) && (now.Subtract(stub.lastNonZeroMaskEvent).TotalSeconds >= ((PatsOptions) this.adapter.connection.Options).MaxChangeMaskZeroSecs)))
                            {
                                this.Connection.ProcessEventArgs(new MarketDataEventArgs(stub.cbiSymbol.MarketData, iTrading.Core.Kernel.ErrorCode.UnexpectedDataStop, "", stub.cbiSymbol, this.typeAsk, stub.cbiSymbol.MarketData.Ask.Price, stub.cbiSymbol.MarketData.Ask.Volume, now));
                                stub.lastNonZeroMaskEvent = now;
                            }
                        }
                        else
                        {
                            stub.lastNonZeroMaskEvent = now;
                        }
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.PriceUpdate: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        private void StartUp()
        {
            object[] objArray;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.StartUp");
            }
            this.expectConnected = true;
            Monitor.Enter(objArray = this.syncCallback);
            try
            {
                this.Init();
                int count = 0;
                TradeMagic.Pats.ErrorCode order = this.adapter.patsApi.CountFills(out count);
                if (order != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.CountFills: error " + order, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        TradeMagic.Pats.PatsApi.Fill fill;
                        order = this.adapter.patsApi.GetFill(i, out fill);
                        if (order != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.GetFill: error " + order, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        if (fill.fillType == 3)
                        {
                            Account account = this.GetAccount(fill.traderAccount);
                            if (account == null)
                            {
                                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.InvalidNativeAccount, "", "Pats.CallBack.StartUp1: Account '" + PatsApi.ToString(fill.traderAccount) + "'"));
                                return;
                            }
                            iTrading.Core.Kernel.Symbol symbol = this.adapter.Convert(fill.exchangeName, fill.contractName, fill.contractDate);
                            if (symbol == null)
                            {
                                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.InvalidNativeSymbol, "", "Pats.CallBack.StartUp1: Contract '" + PatsApi.ToString(fill.contractName) + "/" + PatsApi.ToString(fill.exchangeName) + "/" + PatsApi.ToString(fill.contractDate) + "'"));
                                return;
                            }
                            MarketPosition marketPosition = this.Connection.MarketPositions[MarketPositionId.Long];
                            if (fill.buyOrSell == 'S')
                            {
                                marketPosition = this.Connection.MarketPositions[MarketPositionId.Short];
                            }
                            this.Connection.ProcessEventArgs(new PositionUpdateEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, account, symbol, marketPosition, fill.lots, this.Connection.Currencies[CurrencyId.Unknown], PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, fill.price)));
                            this.overNight.Add(new Execution("", account, symbol, DateTime.Now, marketPosition, "", fill.lots, PatsApi.CharsToPrice(this.adapter.numberFormatInfo, symbol, fill.price)));
                        }
                    }
                    for (int j = 0; j < count; j++)
                    {
                        TradeMagic.Pats.PatsApi.Fill fill2;
                        order = this.adapter.patsApi.GetFill(j, out fill2);
                        if (order != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.GetFill: error " + order, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.Startup:  order='" + PatsApi.ToString(fill2.orderId) + "' fillid='" + PatsApi.ToString(fill2.fillId) + "'");
                        }
                        if (fill2.fillType != 3)
                        {
                            this.HandleFill(fill2);
                        }
                    }
                    order = this.adapter.patsApi.CountOrders(out count);
                    if (order != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.CountOrders: error " + order, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                    else
                    {
                        int num6;
                        for (int k = 0; k < count; k++)
                        {
                            PatsApi.OrderDetail detail;
                            order = this.adapter.patsApi.GetOrder(k, out detail);
                            if (order != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.GetOrder: error " + order, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                return;
                            }
                            this.AddOrder(detail);
                        }
                        foreach (Account account2 in this.adapter.connection.Accounts)
                        {
                            double pl = 0.0;
                            if (this.GetRealizedProfitLoss(account2, out pl) != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Error on retrieving account information", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                return;
                            }
                            this.Connection.ProcessEventArgs(new AccountUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", account2, this.Connection.AccountItemTypes[AccountItemTypeId.RealizedProfitLoss], this.Connection.Currencies[CurrencyId.Unknown], pl, this.Connection.Now));
                            StringBuilder bpRemaining = new StringBuilder(0x15);
                            order = this.adapter.patsApi.BuyingPowerRemaining("", "", "", account2.Name, bpRemaining);
                            if (order != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Callback.StartUp.BuyingPowerRemaining: error " + order, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                return;
                            }
                            this.Connection.ProcessEventArgs(new AccountUpdateEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", account2, this.Connection.AccountItemTypes[AccountItemTypeId.BuyingPower], this.Connection.Currencies[CurrencyId.Unknown], Convert.ToDouble(bpRemaining.ToString(), this.adapter.numberFormatInfo), this.Connection.Now));
                        }
                        this.patsThread = Thread.CurrentThread;
                        order = this.adapter.patsApi.CountContracts(out num6);
                        if (order != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.adapter.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, order.ToString(), "Pats.Callback.StartUp.CountContracts"));
                        }
                        else
                        {
                            string name = this.adapter.connection.Options.Provider.Id.ToString();
                            XmlDocument document = new XmlDocument();
                            document.AppendChild(document.CreateElement("TradeMagic"));
                            document["TradeMagic"].AppendChild(document.CreateElement(name));
                            document["TradeMagic"][name].AppendChild(document.CreateElement("Contracts"));
                            for (int m = 0; m < num6; m++)
                            {
                                PatsApi.Contract contract;
                                order = this.adapter.patsApi.GetContract(m, out contract);
                                if (order != TradeMagic.Pats.ErrorCode.Success)
                                {
                                    this.adapter.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, order.ToString(), "Pats.Callback.StartUp.GetContract"));
                                    return;
                                }
                                XmlElement newChild = document.CreateElement("Contract");
                                newChild.AppendChild(document.CreateElement("Name"));
                                newChild.AppendChild(document.CreateElement("Exchange"));
                                newChild.AppendChild(document.CreateElement("Expiry"));
                                newChild.AppendChild(document.CreateElement("ExternalName"));
                                newChild["Name"].InnerText = PatsApi.ToString(contract.contractName);
                                newChild["Exchange"].InnerText = PatsApi.ToString(contract.exchangeName);
                                newChild["Expiry"].InnerText = PatsApi.ToString(contract.contractDate);
                                newChild["ExternalName"].InnerText = PatsApi.ToString(contract.leg0ExternalName);
                                document["TradeMagic"][name]["Contracts"].AppendChild(newChild);
                            }
                            StringWriter writer = new StringWriter();
                            document.Save(writer);
                            if (Globals.TraceSwitch.Native)
                            {
                                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Pats.Callback.StartUp1: " + writer.ToString());
                            }
                            this.Connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, writer.ToString()));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Callback.StartUp: exception caught: " + exception.Message));
            }
            finally
            {
                Monitor.Exit(objArray);
            }
        }

        private iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.adapter.connection;
            }
        }
    }
}

