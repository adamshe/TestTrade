using iTrading.Core.Kernel;

namespace iTrading.IB
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using iTrading.Core.Kernel;

    internal class OrderRequest : iTrading.IB.Request
    {
        private Adapter adapter;
        private Contract contract;
        internal iTrading.IB.Order order;
        internal int orderId;

        internal OrderRequest(Adapter adapter, iTrading.Core.Kernel.Order order)
            : base(order)
        {
            this.adapter = adapter;
            this.contract = adapter.Convert(order.Symbol);
            this.order = new iTrading.IB.Order(adapter, order.Account.Name, order.Quantity, order.Action.MapId, order.OrderType.MapId, order.TimeInForce.MapId, order.LimitPrice, order.StopPrice, order.OcaGroup, order.CustomText);
        }

        internal bool AdjustOrderStatus(OrderStateId newOrderStateId, string newOrderId)
        {
            iTrading.Core.Kernel.Order tMRequest = (iTrading.Core.Kernel.Order)base.TMRequest;
            if ((newOrderStateId == OrderStateId.Working) && (this.TMOrder.OrderState.Id == OrderStateId.PendingCancel))
            {
                return false;
            }
            if ((newOrderStateId == OrderStateId.Rejected) && (this.TMOrder.OrderState.Id == OrderStateId.Initialized))
            {
                return false;
            }
            if ((newOrderStateId == OrderStateId.Rejected) && ((this.TMOrder.OrderState.Id == OrderStateId.PendingSubmit) || (this.TMOrder.OrderState.Id == OrderStateId.PendingChange)))
            {
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(tMRequest, ErrorCode.NoError, "", newOrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.Accepted], this.adapter.connection.Now));
            }
            if ((newOrderStateId == OrderStateId.Working) && ((this.TMOrder.OrderState.Id == OrderStateId.PendingSubmit) || (this.TMOrder.OrderState.Id == OrderStateId.PendingChange)))
            {
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(tMRequest, ErrorCode.NoError, "", newOrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.Accepted], this.adapter.connection.Now));
            }
            if ((newOrderStateId == OrderStateId.Filled) && (this.TMOrder.OrderState.Id == OrderStateId.Initialized))
            {
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(tMRequest, ErrorCode.NoError, "", newOrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.PendingSubmit], this.adapter.connection.Now));
            }
            if ((newOrderStateId == OrderStateId.Filled) && (this.TMOrder.OrderState.Id == OrderStateId.PendingSubmit))
            {
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(tMRequest, ErrorCode.NoError, "", newOrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.Accepted], this.adapter.connection.Now));
            }
            if ((newOrderStateId == OrderStateId.Filled) && (this.TMOrder.OrderState.Id == OrderStateId.Accepted))
            {
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(tMRequest, ErrorCode.NoError, "", newOrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.Working], this.adapter.connection.Now));
            }
            return true;
        }

        internal bool Cancel()
        {
            if (((OrderRequest) base.TMRequest.AdapterLink).orderId == 0)
            {
                this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.CancelRejected, "", "Orders created by TWS can't be cancelled"));
                return false;
            }
            this.adapter.ibSocket.Send(4);
            this.adapter.ibSocket.Send(1);
            this.adapter.ibSocket.Send(((OrderRequest) base.TMRequest.AdapterLink).orderId);
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") IB.OrderRequest.Cancel: tmOrderId='", ((iTrading.Core .Kernel .Order) base.TMRequest).OrderId, "' twsOrderId='", ((OrderRequest) base.TMRequest.AdapterLink).orderId, "'" }));
            }
            if (this.TMOrder.OrderState.Id == OrderStateId.Rejected)
            {
                iTrading.Core .Kernel .Order tMRequest = (iTrading.Core .Kernel .Order) base.TMRequest;
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs((iTrading.Core .Kernel .Order) base.TMRequest, ErrorCode.NoError, "", tMRequest.OrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.Accepted], this.adapter.connection.Now));
                this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs((iTrading.Core .Kernel .Order) base.TMRequest, ErrorCode.NoError, "", tMRequest.OrderId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, tMRequest.AvgFillPrice, tMRequest.Filled, this.adapter.connection.OrderStates[OrderStateId.Cancelled], this.adapter.connection.Now));
            }
            return true;
        }

        internal static void Process(Adapter adapter, int version)
        {
            OrderStatus status = new OrderStatus();
            status.orderId = adapter.ibSocket.ReadInteger();
            status.orderStatus = adapter.ibSocket.ReadString();
            status.filled = adapter.ibSocket.ReadInteger();
            status.remaining = adapter.ibSocket.ReadInteger();
            status.avgFillPrice = adapter.ibSocket.ReadDouble();
            if (version >= 2)
            {
                status.permId = adapter.ibSocket.ReadString();
            }
            if (version >= 3)
            {
                status.parentId = adapter.ibSocket.ReadInteger();
            }
            if (version >= 4)
            {
                status.lastFillPrice = adapter.ibSocket.ReadDouble();
            }
            if (version >= 5)
            {
                status.connectionId = adapter.ibSocket.ReadInteger();
            }
            if (Globals.TraceSwitch.Order)
            {
                OrderRequest request = (OrderRequest) adapter.orders[status.orderId];
                Trace.WriteLine(string.Concat(new object[] { "(", adapter.connection.IdPlus, ") IB.OrderRequest.Process: id='", status.permId, "' state='", status.orderStatus, "' filled=", status.filled, " price=", status.avgFillPrice, " / ", (request == null) ? "(null)" : request.TMOrder.ToString() }));
            }
            adapter.connection.SynchronizeInvoke.AsyncInvoke(new ProcessDelegate(OrderRequest.ProcessNow), new object[] { adapter, status.Clone() });
        }

        private static void ProcessNow(Adapter adapter, OrderStatus status)
        {
            OrderRequest request = (OrderRequest) adapter.orders[status.orderId];
            if (request == null)
            {
                lock (adapter.postponedOrderStatus)
                {
                    adapter.postponedOrderStatus.Add(status);
                }
            }
            else
            {
                OrderState state = adapter.connection.OrderStates.FindByMapId(status.orderStatus);
                if (state == null)
                {
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.OrderRequest.Process: unable to convert iTrading.IB order status '" + status.orderStatus + "'"));
                    state = adapter.connection.OrderStates[OrderStateId.Initialized];
                }
                if ((request.TMOrder.OrderState.Id != state.Id) && ((request.TMOrder.OrderState.Id != OrderStateId.PendingChange) || (state.Id != OrderStateId.PendingSubmit)))
                {
                    if (request.TMOrder.Account.Orders.FindByOrderId(request.TMOrder.OrderId) == null)
                    {
                        if ((request.TMOrder.OrderState.Id != OrderStateId.Cancelled) && (request.TMOrder.OrderState.Id != OrderStateId.Rejected))
                        {
                            adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.OrderRequest.ProcessNow: unable to find order '" + request.TMOrder.OrderId + "', status '" + status.orderStatus + "'"));
                        }
                    }
                    else if (request.AdjustOrderStatus(state.Id, status.permId))
                    {
                        if ((state.Id == OrderStateId.Filled) && (status.filled != request.order.totalQuantity))
                        {
                            state = adapter.connection.OrderStates[OrderStateId.PartFilled];
                        }
                        else if (status.filled == request.order.totalQuantity)
                        {
                            state = adapter.connection.OrderStates[OrderStateId.Filled];
                        }
                        double avgFillPrice = status.avgFillPrice;
                        iTrading.Core .Kernel .Order tMRequest = (iTrading.Core .Kernel .Order) request.TMRequest;
                        if ((request.contract.Symbol == "Z") && (request.contract.SecType == "FUT"))
                        {
                            avgFillPrice *= 100.0;
                        }
                        adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(tMRequest, ErrorCode.NoError, "", status.permId, tMRequest.LimitPrice, tMRequest.StopPrice, tMRequest.Quantity, avgFillPrice, status.filled, adapter.connection.OrderStates[state.Id], adapter.connection.Now));
                    }
                }
            }
        }

        internal static void ProcessOpenOrders(Adapter adapter, int version)
        {
            Contract contract = new Contract("");
            iTrading.IB.Order order = new iTrading.IB.Order(adapter, "", 0, "", "", "", 0.0, 0.0, "", "");
            int num = adapter.ibSocket.ReadInteger();
            contract.Symbol = adapter.ibSocket.ReadString();
            contract.SecType = adapter.ibSocket.ReadString();
            contract.Expiry = adapter.ibSocket.ReadExpiry();
            contract.Strike = adapter.ibSocket.ReadDouble();
            contract.Right = adapter.ibSocket.ReadRight();
            contract.Exchange = adapter.ibSocket.ReadString();
            contract.Currency = adapter.ibSocket.ReadString();
            if (version >= 2)
            {
                contract.LocalSymbol = adapter.ibSocket.ReadString();
            }
            order.Action = adapter.ibSocket.ReadString();
            order.totalQuantity = adapter.ibSocket.ReadInteger();
            order.OrderType = adapter.ibSocket.ReadString();
            order.LmtPrice = adapter.ibSocket.ReadDouble();
            order.AuxPrice = adapter.ibSocket.ReadDouble();
            order.Tif = adapter.ibSocket.ReadString();
            order.OcaGroup = adapter.ibSocket.ReadString();
            order.Account = adapter.ibSocket.ReadString();
            order.OpenClose = adapter.ibSocket.ReadString();
            order.Origin = (Origin) adapter.ibSocket.ReadInteger();
            order.OrderRef = adapter.ibSocket.ReadString();
            if (version >= 3)
            {
                order.ConnectionId = adapter.ibSocket.ReadInteger();
            }
            if (version >= 4)
            {
                order.permId = adapter.ibSocket.ReadInteger();
                order.IgnoreRth = adapter.ibSocket.ReadInteger() == 1;
                order.Hidden = adapter.ibSocket.ReadInteger() == 1;
                order.discretionaryAmt = adapter.ibSocket.ReadDouble();
            }
            if (version >= 5)
            {
                order.goodAfterTime = adapter.ibSocket.ReadString();
            }
            if (version >= 6)
            {
                order.SharesAllocation = adapter.ibSocket.ReadString();
            }
            if (version >= 7)
            {
                order.faGroup = adapter.ibSocket.ReadString();
                order.faMethod = adapter.ibSocket.ReadString();
                order.faPercentage = adapter.ibSocket.ReadString();
                order.faProfile = adapter.ibSocket.ReadString();
            }
            if ((order.OrderType == "STP") || (order.OrderType == "MKT"))
            {
                order.LmtPrice = 0.0;
            }
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", adapter.connection.IdPlus, ") IB.OrderRequest.ProcessOpenOrders: id='", order.permId, "' quantity=", order.totalQuantity, " lmtprice=", order.LmtPrice, " auxprice=", order.AuxPrice, " account='", order.Account, "'" }));
            }
            adapter.connection.SynchronizeInvoke.AsyncInvoke(new ProcessOpenOrdersDelegate(OrderRequest.ProcessOpenOrdersNow), new object[] { adapter, contract, order, num });
        }

        private static void ProcessOpenOrdersNow(Adapter adapter, Contract contract, iTrading.IB.Order order, int orderId)
        {
            Symbol symbol;
            Account account;
            ActionType type;
            Hashtable hashtable;
            if (((contract.SecType == "FUT") || (contract.SecType == "STK")) || ((contract.SecType == "OPT") || (contract.SecType == "IND")))
            {
                if (adapter.approvedPermIds != null)
                {
                    lock (adapter.approvedPermIds)
                    {
                        if (!adapter.approvedPermIds.Contains(order.permId.ToString()))
                        {
                            adapter.approvedPermIds.Add(order.permId.ToString());
                        }
                    }
                }
                OrderRequest request = (OrderRequest) adapter.orders[orderId];
                if (request != null)
                {
                    if ((((request.TMOrder.OrderState.Id == OrderStateId.Accepted) || (request.TMOrder.OrderState.Id == OrderStateId.Working)) || (request.TMOrder.OrderState.Id == OrderStateId.PartFilled)) && (((Math.Abs((double) (request.TMOrder.LimitPrice - order.LmtPrice)) >= (request.TMOrder.Symbol.TickSize / 2.0)) || (Math.Abs((double) (request.TMOrder.StopPrice - order.AuxPrice)) >= (request.TMOrder.Symbol.TickSize / 2.0))) || (request.TMOrder.Quantity != order.totalQuantity)))
                    {
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine(string.Concat(new object[] { 
                                "(", adapter.connection.IdPlus, ") IB.OrderRequest.ProcessOpenOrdersNow: id='", order.permId, "' orderstate=", request.TMOrder.OrderState.Name, " quantity=", request.TMOrder.Quantity, "/", order.totalQuantity, " limit=", request.TMOrder.LimitPrice, "/", order.LmtPrice, " stop='", request.TMOrder.StopPrice, 
                                "/", order.AuxPrice
                             }));
                        }
                        adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.NoError, "", request.TMOrder.OrderId, order.LmtPrice, order.AuxPrice, order.totalQuantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, request.TMOrder.OrderState, request.TMOrder.Time));
                    }
                    return;
                }
                lock ((hashtable = adapter.orders))
                {
                    OrderRequest request2 = null;
                    foreach (OrderRequest request3 in adapter.orders.Values)
                    {
                        if ((request3.order.permId == order.permId) && (request3.orderId != orderId))
                        {
                            request2 = request3;
                            return;
                        }
                    }
                    if (request2 != null)
                    {
                        adapter.orders.Remove(request2.orderId);
                        request2.orderId = orderId;
                        adapter.orders.Add(orderId, request2);
                        return;
                    }
                }
                symbol = adapter.Convert(contract, null, 0.01, 1.0, null);
                if (symbol != null)
                {
                    account = adapter.GetAccount(order.Account);
                    if (account != null)
                    {
                        type = null;
                        if (order.Action == "BUY")
                        {
                            type = adapter.connection.ActionTypes[ActionTypeId.Buy];
                            goto Label_04ED;
                        }
                        if (order.Action == "SELL")
                        {
                            type = adapter.connection.ActionTypes[ActionTypeId.Sell];
                            goto Label_04ED;
                        }
                        type = adapter.connection.ActionTypes[ActionTypeId.Sell];
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.Adapter.Convert: can't convert iTrading.IB order action '" + order.Action + "'"));
                    }
                    return;
                }
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.InvalidNativeSymbol, "", string.Concat(new object[] { "IB.OrderRequest.ProcessOpenOrders: Contract '", contract.Symbol, "/", contract.SecType, "/", contract.Expiry, "/", contract.Exchange })));
            }
            return;
        Label_04ED:
            if (order.OrderType == "STP LMT")
            {
                order.OrderType = "STPLMT";
            }
            OrderType type2 = adapter.connection.OrderTypes.FindByMapId(order.OrderType);
            if (type2 == null)
            {
                type2 = adapter.connection.OrderTypes[OrderTypeId.Unknown];
            }
            iTrading.Core .Kernel .Order order2 = account.CreateOrder(symbol, type.Id, type2.Id, (order.Tif == "DAY") ? TimeInForceId.Day : TimeInForceId.Gtc, order.totalQuantity, order.LmtPrice, order.AuxPrice, "", adapter.connection.OrderStates[OrderStateId.Initialized], order.permId.ToString(), order.OcaGroup, null, null);
            OrderRequest request4 = new OrderRequest(adapter, order2);
            lock ((hashtable = adapter.orders))
            {
                adapter.orders.Add(request4.orderId = orderId, request4);
                adapter.nextOrderId = Math.Max(adapter.nextOrderId - 1, orderId) + 1;
            }
            order2.AdapterLink = request4;
            lock (adapter.postponedOrderStatus)
            {
                ArrayList list = new ArrayList();
                foreach (OrderStatus status in adapter.postponedOrderStatus)
                {
                    if (status.permId == order2.OrderId)
                    {
                        list.Add(status);
                    }
                }
                foreach (OrderStatus status2 in list)
                {
                    ProcessNow(adapter, status2);
                    adapter.postponedOrderStatus.Remove(status2);
                }
            }
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(3);
            adapter.ibSocket.Send(13);
            adapter.ibSocket.Send(this.orderId);
            adapter.ibSocket.Send(this.contract.Symbol);
            adapter.ibSocket.Send(this.contract.SecType);
            adapter.ibSocket.Send((this.contract.Expiry == Globals.MaxDate) ? "" : this.contract.Expiry.ToString("yyyyMM"));
            adapter.ibSocket.Send(this.contract.Strike);
            adapter.ibSocket.Send((string) Names.Rights[this.contract.Right]);
            adapter.ibSocket.Send(this.contract.Exchange);
            adapter.ibSocket.Send(this.contract.Currency);
            if (adapter.ibSocket.ServerVersion >= 2)
            {
                adapter.ibSocket.Send(this.contract.LocalSymbol);
            }
            adapter.ibSocket.Send(this.order.Action);
            adapter.ibSocket.Send(this.order.totalQuantity);
            adapter.ibSocket.Send(this.order.OrderType);
            adapter.ibSocket.Send(this.order.LmtPrice);
            adapter.ibSocket.Send(this.order.AuxPrice);
            adapter.ibSocket.Send(this.order.Tif);
            adapter.ibSocket.Send(this.order.OcaGroup);
            adapter.ibSocket.Send(this.order.Account);
            adapter.ibSocket.Send(this.order.OpenClose);
            adapter.ibSocket.Send((int) this.order.Origin);
            adapter.ibSocket.Send(this.order.OrderRef);
            adapter.ibSocket.Send(this.order.Transmit ? 1 : 0);
            if (adapter.ibSocket.ServerVersion >= 4)
            {
                adapter.ibSocket.Send(this.order.ParentId);
            }
            if (adapter.ibSocket.ServerVersion >= 5)
            {
                adapter.ibSocket.Send(this.order.BlOrder ? 1 : 0);
                adapter.ibSocket.Send(this.order.SweepToFill ? 1 : 0);
                adapter.ibSocket.Send(this.order.DisplaySize);
                adapter.ibSocket.Send(this.order.TriggerMethod);
                adapter.ibSocket.Send(this.order.IgnoreRth ? 1 : 0);
            }
            if (adapter.ibSocket.ServerVersion >= 7)
            {
                adapter.ibSocket.Send(this.order.Hidden ? 1 : 0);
            }
            if ((adapter.ibSocket.ServerVersion >= 8) && (this.contract.SecType == "BAG"))
            {
                adapter.ibSocket.Send(this.contract.ComboLegs.Count);
                foreach (ComboLeg leg in this.contract.ComboLegs)
                {
                    adapter.ibSocket.Send(leg.ConId);
                    adapter.ibSocket.Send(leg.Ratio);
                    adapter.ibSocket.Send(leg.Action);
                    adapter.ibSocket.Send(leg.ConId);
                    adapter.ibSocket.Send((int) leg.OpenClose);
                }
            }
            if (adapter.ibSocket.ServerVersion >= 9)
            {
                adapter.ibSocket.Send(this.order.SharesAllocation);
            }
            if (adapter.ibSocket.ServerVersion >= 10)
            {
                adapter.ibSocket.Send(this.order.discretionaryAmt);
            }
            if (adapter.ibSocket.ServerVersion >= 11)
            {
                adapter.ibSocket.Send(this.order.goodAfterTime);
            }
            if (adapter.ibSocket.ServerVersion >= 12)
            {
                adapter.ibSocket.Send(this.order.goodTillDate);
            }
            if (adapter.ibSocket.ServerVersion >= 13)
            {
                adapter.ibSocket.Send(this.order.faGroup);
                adapter.ibSocket.Send(this.order.faMethod);
                adapter.ibSocket.Send(this.order.faPercentage);
                adapter.ibSocket.Send(this.order.faProfile);
            }
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + adapter.connection.IdPlus + ") IB.OrderRequest.Send: tmOrderId='" + this.TMOrder.OrderId + "' " + this.ToString());
            }
        }

        public override string ToString()
        {
            return string.Concat(new object[] { 
                "orderId ='", this.orderId, "' symbol='", this.contract.Symbol, "' expiry='", (this.contract.Expiry == Globals.MaxDate) ? "" : this.contract.Expiry.ToString("yyyyMM"), "' strikePrice=", this.contract.Strike, " right='", (string) Names.Rights[this.contract.Right], "' exchange='", this.contract.Exchange, "' action='", this.order.Action, "' limitPrice=", this.order.LmtPrice, 
                " auxPrice=", this.order.AuxPrice, " quantity=", this.order.totalQuantity, " type='", this.order.OrderType, "' tif='", this.order.Tif, "' oca='", this.order.OcaGroup, "'"
             });
        }

        internal iTrading.Core .Kernel .Order TMOrder
        {
            get
            {
                return (iTrading.Core.Kernel.Order)base.TMRequest;
            }
        }

        private delegate void ProcessDelegate(Adapter adapter, OrderStatus status);

        private delegate void ProcessOpenOrdersDelegate(Adapter adapter, Contract contract, iTrading.IB.Order order, int orderId);
    }
}

