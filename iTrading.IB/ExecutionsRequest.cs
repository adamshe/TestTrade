using iTrading.Core.Kernel;

namespace iTrading.IB
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using iTrading.Core.Kernel;

    internal class ExecutionsRequest : iTrading.IB.Request
    {
        private readonly iTrading.IB.ExecutionFilter ExecutionFilter;

        internal ExecutionsRequest(Adapter adapter) : base(adapter.connection)
        {
            this.ExecutionFilter = new iTrading.IB.ExecutionFilter();
        }

        internal static void Process(Adapter adapter, int version)
        {
            iTrading.IB.Execution execution = new iTrading.IB.Execution();
            execution.orderId = adapter.ibSocket.ReadInteger();
            execution.contract.Symbol = adapter.ibSocket.ReadString();
            execution.contract.SecType = adapter.ibSocket.ReadString();
            execution.contract.Expiry = adapter.ibSocket.ReadExpiry();
            execution.contract.Strike = adapter.ibSocket.ReadDouble();
            execution.contract.Right = adapter.ibSocket.ReadRight();
            execution.contract.Exchange = adapter.ibSocket.ReadString();
            execution.contract.Currency = adapter.ibSocket.ReadString();
            execution.contract.LocalSymbol = adapter.ibSocket.ReadString();
            execution.execId = adapter.ibSocket.ReadString();
            execution.timeString = adapter.ibSocket.ReadString();
            execution.acctNumber = adapter.ibSocket.ReadString();
            execution.exchange = adapter.ibSocket.ReadString();
            execution.side = adapter.ibSocket.ReadString();
            execution.shares = adapter.ibSocket.ReadInteger();
            execution.price = adapter.ibSocket.ReadDouble();
            if (version >= 2)
            {
                execution.permId = adapter.ibSocket.ReadString();
            }
            if (version >= 3)
            {
                execution.connectionId = adapter.ibSocket.ReadInteger();
            }
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", adapter.connection.IdPlus, ") IB.ExecutionsRequest.Process: id='", execution.execId, "' symbol='", execution.contract.Symbol, "' orderid='", execution.permId, "' account='", execution.acctNumber, "' filled=", execution.shares, " price=", execution.price }));
            }
            adapter.connection.SynchronizeInvoke.AsyncInvoke(new ProcessDelegate(ExecutionsRequest.ProcessNow), new object[] { adapter, execution });
        }

        private static void ProcessNow(Adapter adapter, iTrading.IB.Execution execution)
        {
            Account account = null;
            account = adapter.GetAccount(execution.acctNumber);
            if (account == null)
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + adapter.connection.IdPlus + ") IB.ExecutionsRequest.ProcessNow: id='" + execution.execId + "', account='" + execution.acctNumber + "' not found");
                }
                return;
            }
            if (((execution.contract.SecType != "FUT") && (execution.contract.SecType != "STK")) && ((execution.contract.SecType != "OPT") && (execution.contract.SecType != "IND")))
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + adapter.connection.IdPlus + ") IB.ExecutionsRequest.ProcessNow: id='" + execution.execId + "', sectype='" + execution.contract.SecType + "' is not supported");
                }
                return;
            }
            if (account.Executions.FindByExecId(execution.execId) != null)
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + adapter.connection.IdPlus + ") IB.ExecutionsRequest.ProcessNow: id='" + execution.execId + "', execId='" + execution.execId + "' already exists");
                }
                return;
            }
            DateTime time = new DateTime(Convert.ToInt32(execution.timeString.Substring(0, 4)), Convert.ToInt32(execution.timeString.Substring(4, 2)), Convert.ToInt32(execution.timeString.Substring(6, 2)), Convert.ToInt32(execution.timeString.Substring(10, 2)), Convert.ToInt32(execution.timeString.Substring(13, 2)), Convert.ToInt32(execution.timeString.Substring(0x10, 2)));
            MarketPosition marketPosition = adapter.connection.MarketPositions.FindByMapId(execution.side);
            if (marketPosition == null)
            {
                marketPosition = adapter.connection.MarketPositions[MarketPositionId.Unknown];
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.AccountUpdatesRequest.Process: Unknown IB market position '" + execution.side + "'"));
            }
            Symbol symbol = adapter.Convert(execution.contract, null, 0.01, 1.0, null);
            if (symbol == null)
            {
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.InvalidNativeSymbol, "", string.Concat(new object[] { "IB.ExecutionsRequest.Process: Contract '", execution.contract.Symbol, "/", execution.contract.SecType, "/", execution.contract.Expiry, "/", execution.contract.Exchange })));
                return;
            }
            double price = execution.price;
            string executionId = execution.contract.LocalSymbol + "/" + execution.execId;
            iTrading.Core .Kernel .Operation insert = iTrading.Core .Kernel .Operation.Insert;
            if ((execution.contract.Symbol == "Z") && (execution.contract.SecType == "FUT"))
            {
                price *= 100.0;
            }
            if (((symbol.MarketData.Ask != null) && (symbol.MarketData.Bid != null)) && (symbol.MarketData.Last != null))
            {
                if (price <= 0.0)
                {
                    lock (account.Executions)
                    {
                        OrderRequest request = (OrderRequest) adapter.orders[execution.orderId];
                        if (request == null)
                        {
                            price = symbol.MarketData.Last.Price;
                            Trace.WriteLine("WARNING: execution price '0' was set to last traded price (" + execution.execId + ")");
                        }
                        else if ((request.TMOrder.Action.Id == ActionTypeId.Buy) || (request.TMOrder.Action.Id == ActionTypeId.BuyToCover))
                        {
                            price = symbol.MarketData.Ask.Price;
                            Trace.WriteLine("WARNING: execution price '0' was set to ask price (" + execution.execId + ")");
                        }
                        else
                        {
                            price = symbol.MarketData.Bid.Price;
                            Trace.WriteLine("WARNING: execution price '0' was set to bid price (" + execution.execId + ")");
                        }
                        goto Label_0763;
                    }
                }
                if (((symbol.Exchange.Id == ExchangeId.Eurex) && (execution.execId.Length > 2)) && (execution.execId.Substring(execution.execId.Length - 2) == "01"))
                {
                    OrderRequest request2 = (OrderRequest) adapter.orders[execution.orderId];
                    if ((request2 != null) && ((request2.TMOrder.OrderType.Id == OrderTypeId.Limit) || (request2.TMOrder.OrderType.Id == OrderTypeId.StopLimit)))
                    {
                        if (((request2.TMOrder.Action.Id == ActionTypeId.Buy) || (request2.TMOrder.Action.Id == ActionTypeId.BuyToCover)) && (price > (request2.TMOrder.Symbol.MarketData.Ask.Price + request2.TMOrder.Symbol.TickSize)))
                        {
                            Trace.WriteLine(string.Concat(new object[] { "WARNING: execution price '", price, "' was set to '", request2.TMOrder.Symbol.MarketData.Ask.Price, "' price (", execution.execId, ")" }));
                            price = request2.TMOrder.Symbol.MarketData.Ask.Price;
                        }
                        else if (((request2.TMOrder.Action.Id == ActionTypeId.Sell) || (request2.TMOrder.Action.Id == ActionTypeId.SellShort)) && (price < (request2.TMOrder.Symbol.MarketData.Bid.Price - request2.TMOrder.Symbol.TickSize)))
                        {
                            Trace.WriteLine(string.Concat(new object[] { "WARNING: execution price '", price, "' was set to '", request2.TMOrder.Symbol.MarketData.Bid.Price, "' price (", execution.execId, ")" }));
                            price = request2.TMOrder.Symbol.MarketData.Bid.Price;
                        }
                    }
                }
            }
        Label_0763:
            if ((execution.execId.Length > 2) && (executionId.Substring(executionId.Length - 2) != "01"))
            {
                executionId = executionId.Substring(0, executionId.Length - 2) + "01";
                insert = iTrading.Core .Kernel .Operation.Update;
            }
            adapter.connection.ProcessEventArgs(new ExecutionUpdateEventArgs(adapter.connection, ErrorCode.NoError, "", insert, executionId, account, symbol, time, marketPosition, execution.permId, execution.shares, price));
            lock (adapter.accountUpdateRequests)
            {
                AccountUpdatesRequest request3 = null;
                for (int i = 0; i < adapter.accountUpdateRequests.Count; i++)
                {
                    AccountUpdatesRequest request4 = (AccountUpdatesRequest) adapter.accountUpdateRequests[i];
                    if (request4.account == account.Name)
                    {
                        request3 = request4;
                        break;
                    }
                }
                if (request3 == null)
                {
                    adapter.accountUpdateRequests.Add(new AccountUpdatesRequest(adapter, account.Name, true));
                }
                else if (request3 == ((AccountUpdatesRequest) adapter.accountUpdateRequests[0]))
                {
                    request3.elapsed = 0;
                }
            }
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(7);
            adapter.ibSocket.Send(2);
            if (adapter.ibSocket.ServerVersion >= 9)
            {
                adapter.ibSocket.Send(this.ExecutionFilter.ConnectionId);
                adapter.ibSocket.Send(this.ExecutionFilter.AccountCode);
                adapter.ibSocket.Send(this.ExecutionFilter.Time);
                adapter.ibSocket.Send(this.ExecutionFilter.Symbol);
                adapter.ibSocket.Send(this.ExecutionFilter.SecType);
                adapter.ibSocket.Send(this.ExecutionFilter.Exchange);
                adapter.ibSocket.Send(this.ExecutionFilter.Side);
            }
        }

        private delegate void ProcessDelegate(Adapter adapter, iTrading.IB.Execution execution);
    }
}

