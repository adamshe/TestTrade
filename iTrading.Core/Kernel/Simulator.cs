using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    internal class Simulator
    {
        private double commissionMin = 0.0;
        private iTrading.Core.Kernel.Currency currency = null;
        private OrderState lastOrderState = null;
        private double limitPrice = 0.0;
        private Order order = null;
        private OrderType orderType = null;
        private int quantity = 0;
        private int queuePosition = 0;
        private int queueSize = 0;
        internal SimulationSymbolOptions simulationSymbolOptions = null;
        private double stopPrice = 0.0;
        private System.Timers.Timer timerCancel = null;
        private System.Timers.Timer timerChange = null;
        private System.Timers.Timer timerSubmit = null;

        internal Simulator(Order order, SimulationSymbolOptions simulationSymbolOptions)
        {
            this.currency = iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown];
            this.commissionMin = simulationSymbolOptions.CommissionMin;
            this.limitPrice = order.LimitPrice;
            this.order = order;
            this.orderType = order.OrderType;
            this.quantity = order.Quantity;
            this.simulationSymbolOptions = simulationSymbolOptions;
            this.stopPrice = order.StopPrice;
            if (((order.OrderState.Id != OrderStateId.Cancelled) && (order.OrderState.Id != OrderStateId.Filled)) && ((order.OrderState.Id != OrderStateId.Rejected) && (order.OrderState.Id != OrderStateId.Unknown)))
            {
                order.Account.OrderStatus += new OrderStatusEventHandler(this.Account_OrderStatus);
                order.Symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                if (this.orderType.Id == OrderTypeId.Limit)
                {
                    order.Symbol.MarketDepth.MarketDepthItem += new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                    if (order.OrderState.Id != OrderStateId.Initialized)
                    {
                        this.PlaceInQueue();
                    }
                }
            }
        }

        private void Account_OrderStatus(object sender, OrderStatusEventArgs e)
        {
            if ((e.order == this.order) && (((e.OrderState.Id == OrderStateId.Filled) || (e.OrderState.Id == OrderStateId.Cancelled)) || ((e.OrderState.Id == OrderStateId.Rejected) || (e.OrderState.Id == OrderStateId.Unknown))))
            {
                this.order.Account.OrderStatus -= new OrderStatusEventHandler(this.Account_OrderStatus);
                this.order.Symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                if ((this.order.OrderType.Id == OrderTypeId.Limit) || (this.order.OrderType.Id == OrderTypeId.StopLimit))
                {
                    this.order.Symbol.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                }
            }
        }

        internal void Cancel()
        {
            this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.PendingCancel], this.order.connection.Now));
            this.timerCancel = new System.Timers.Timer((double) (this.order.Account.simulationAccountOptions.DelayExchange + (2 * this.order.Account.simulationAccountOptions.DelayCommunication)));
            this.timerCancel.Elapsed += new ElapsedEventHandler(this.timerCancel_Elapsed);
            this.timerCancel.AutoReset = false;
            this.timerCancel.Enabled = true;
        }

        internal void Change()
        {
            this.lastOrderState = this.order.OrderState;
            this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.order.LimitPrice, this.order.StopPrice, this.order.Quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.PendingChange], this.order.connection.Now));
            this.timerChange = new System.Timers.Timer((double) (this.order.Account.simulationAccountOptions.DelayExchange + (2 * this.order.Account.simulationAccountOptions.DelayCommunication)));
            this.timerChange.Elapsed += new ElapsedEventHandler(this.timerChange_Elapsed);
            this.timerChange.AutoReset = false;
            this.timerChange.Enabled = true;
        }

        private bool CheckExcessEquity()
        {
            double num = this.order.Account.GetItem(AccountItemType.All[AccountItemTypeId.CashValue], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown]).Value;
            int quantity = 0;
            int num3 = 0;
            MarketPositionId id = ((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) ? MarketPositionId.Long : MarketPositionId.Short;
            Position position = this.order.Account.Positions.FindBySymbol(this.order.Symbol);
            double num4 = (this.order.Symbol.SymbolType.Id == SymbolTypeId.Future) ? this.simulationSymbolOptions.Margin : this.order.Symbol.MarketData.Last.Price;
            if (position == null)
            {
                quantity = this.order.Quantity;
                num3 = 0;
            }
            else if (position.MarketPosition.Id == id)
            {
                quantity = this.order.Quantity;
                num3 = 0;
            }
            else if (position.Quantity == this.order.Quantity)
            {
                quantity = 0;
                num3 = this.order.Quantity;
            }
            else if (position.Quantity >= this.order.Quantity)
            {
                quantity = 0;
                num3 = this.order.Quantity;
            }
            else
            {
                quantity = this.order.Quantity - position.Quantity;
                num3 = position.Quantity;
            }
            if (quantity > 0)
            {
                if (id == MarketPositionId.Long)
                {
                    num = (num - (quantity * (num4 + this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                }
                else
                {
                    num = (num + (quantity * (num4 - this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                }
            }
            if (num3 > 0)
            {
                if (id == MarketPositionId.Long)
                {
                    num = (num - (num3 * (num4 + this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                }
                else
                {
                    num = (num + (num3 * (num4 - this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                }
            }
            double num5 = 0.0;
            foreach (Position position2 in this.order.Account.Positions)
            {
                num5 += (((((position2.MarketPosition.Id == MarketPositionId.Short) ? -1 : 1) * position2.Quantity) * ((position2.Symbol.SymbolType.Id == SymbolTypeId.Future) ? this.simulationSymbolOptions.Margin : position2.AvgPrice)) - (position2.Quantity * this.simulationSymbolOptions.CommissionPerUnit)) - this.commissionMin;
            }
            num5 += (((((id == MarketPositionId.Short) ? -1 : 1) * this.order.Quantity) * ((this.order.Symbol.SymbolType.Id == SymbolTypeId.Future) ? this.simulationSymbolOptions.Margin : num4)) - (this.order.Quantity * this.simulationSymbolOptions.CommissionPerUnit)) - this.commissionMin;
            double num6 = ((num + num5) - Math.Min((double) (Math.Abs(num5) - num), (double) 0.0)) - (this.order.Account.SimulationAccountOptions.MaintenanceMargin * Math.Abs(num5));
            if ((num6 >= 0.0) && (num >= 0.0))
            {
                return true;
            }
            this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.OrderRejected, "Excess equity", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Rejected], this.order.connection.Now));
            return false;
        }

        private void Fill(int fillQuantity, double avgFillPrice)
        {
            Application.DoEvents();
            if (((this.order.OrderState.Id == OrderStateId.Working) || (this.order.OrderState.Id == OrderStateId.PartFilled)) && (((this.timerCancel == null) && (this.timerChange == null)) && (this.timerSubmit == null)))
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.order.Connection.IdPlus, ") Cbi.Simulator.Fill: id='", this.order.OrderId, "' state='", this.order.OrderState.Name, "' symbol='", this.order.Symbol.FullName, "' quantity=", fillQuantity, " filled=", this.order.Filled, " price=", avgFillPrice }));
                }
                MarketPosition marketPosition = MarketPosition.All[((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) ? MarketPositionId.Long : MarketPositionId.Short];
                avgFillPrice = this.simulationSymbolOptions.GetAvgPrice(avgFillPrice, this.order, marketPosition);
                ExecutionUpdateEventArgs eventArgs = new ExecutionUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", Operation.Insert, "SIM_" + Guid.NewGuid().ToString(), this.order.Account, this.order.Symbol, this.order.connection.Now, marketPosition, this.order.orderId, fillQuantity, avgFillPrice);
                double avgPositionPrice = this.GetAvgPositionPrice(this.order.Account, new Execution("", this.order.Account, this.order.Symbol, this.order.connection.Now, marketPosition, "", fillQuantity, avgFillPrice));
                PositionUpdateEventArgs args2 = null;
                int num2 = 0;
                int quantity = 0;
                Position position2 = this.order.Account.Positions.FindBySymbol(this.order.Symbol);
                if (position2 == null)
                {
                    args2 = new PositionUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", Operation.Insert, this.order.Account, this.order.Symbol, marketPosition, fillQuantity, this.currency, avgPositionPrice);
                    num2 = fillQuantity;
                    quantity = 0;
                }
                else if (position2.MarketPosition.Id == marketPosition.Id)
                {
                    args2 = new PositionUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", Operation.Update, this.order.Account, this.order.Symbol, marketPosition, position2.Quantity + fillQuantity, this.currency, avgPositionPrice);
                    num2 = fillQuantity;
                    quantity = 0;
                }
                else if (position2.Quantity == fillQuantity)
                {
                    args2 = new PositionUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", Operation.Delete, this.order.Account, this.order.Symbol, position2.MarketPosition, 0, this.currency, avgPositionPrice);
                    num2 = 0;
                    quantity = fillQuantity;
                }
                else if (position2.Quantity >= fillQuantity)
                {
                    args2 = new PositionUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", Operation.Update, this.order.Account, this.order.Symbol, position2.MarketPosition, position2.Quantity - fillQuantity, this.currency, avgPositionPrice);
                    num2 = 0;
                    quantity = fillQuantity;
                }
                else
                {
                    args2 = new PositionUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", Operation.Update, this.order.Account, this.order.Symbol, marketPosition, fillQuantity - position2.Quantity, this.currency, avgPositionPrice);
                    num2 = fillQuantity - position2.Quantity;
                    quantity = position2.Quantity;
                }
                double newValue = this.order.Account.GetItem(AccountItemType.All[AccountItemTypeId.CashValue], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown]).Value;
                double num5 = (this.order.Symbol.SymbolType.Id == SymbolTypeId.Future) ? this.simulationSymbolOptions.Margin : avgFillPrice;
                if (num2 > 0)
                {
                    if (marketPosition.Id == MarketPositionId.Long)
                    {
                        newValue = (newValue - (num2 * (num5 + this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                    }
                    else
                    {
                        newValue = (newValue + (num2 * (num5 - this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                    }
                }
                if (quantity > 0)
                {
                    if (marketPosition.Id == MarketPositionId.Long)
                    {
                        newValue = (newValue - (quantity * (num5 + this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                    }
                    else
                    {
                        newValue = (newValue + (quantity * (num5 - this.simulationSymbolOptions.CommissionPerUnit))) - this.commissionMin;
                    }
                }
                double num6 = 0.0;
                double num7 = 0.0;
                foreach (Position position3 in this.order.Account.Positions)
                {
                    int num8 = position3.Quantity;
                    double avgPrice = position3.AvgPrice;
                    if (position3 == position2)
                    {
                        num8 = args2.Quantity;
                        avgPrice = args2.AvgPrice;
                    }
                    num6 += num8 * ((position3.Symbol.SymbolType.Id == SymbolTypeId.Future) ? this.simulationSymbolOptions.Margin : (this.order.Account.SimulationAccountOptions.Margin * avgPrice));
                    num7 += (((((position3.MarketPosition.Id == MarketPositionId.Short) ? -1 : 1) * num8) * ((position3.Symbol.SymbolType.Id == SymbolTypeId.Future) ? this.simulationSymbolOptions.Margin : avgPrice)) - (num8 * this.simulationSymbolOptions.CommissionPerUnit)) - this.commissionMin;
                }
                double num10 = ((newValue + num7) - Math.Min((double) (Math.Abs(num7) - newValue), (double) 0.0)) - (this.order.Account.SimulationAccountOptions.MaintenanceMargin * Math.Abs(num7));
                if ((num10 < 0.0) || (newValue < 0.0))
                {
                    this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.OrderRejected, "Excess equity", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Rejected], this.order.connection.Now));
                }
                else
                {
                    this.order.connection.ProcessEventArgs(eventArgs);
                    this.order.connection.ProcessEventArgs(args2);
                    this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, ((this.order.Filled * this.order.AvgFillPrice) + (fillQuantity * avgFillPrice)) / ((double) (this.order.Filled + fillQuantity)), Math.Min(this.order.Filled + fillQuantity, this.quantity), OrderState.All[((this.order.Filled + fillQuantity) >= this.quantity) ? OrderStateId.Filled : OrderStateId.PartFilled], this.order.connection.Now));
                    this.order.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", this.order.Account, AccountItemType.All[AccountItemTypeId.BuyingPower], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 2.0 * newValue, this.order.connection.Now));
                    this.order.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", this.order.Account, AccountItemType.All[AccountItemTypeId.CashValue], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], newValue, this.order.connection.Now));
                    this.order.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", this.order.Account, AccountItemType.All[AccountItemTypeId.ExcessEquity], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], num10, this.order.connection.Now));
                    this.order.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.order.connection, ErrorCode.NoError, "", this.order.Account, AccountItemType.All[AccountItemTypeId.InitialMargin], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], num6, this.order.connection.Now));
                    this.order.Account.SimulationAccountUpdate();
                    this.commissionMin = 0.0;
                }
            }
        }

        private double GetAvgPositionPrice(Account account, Execution newExecution)
        {
            int num = 0;
            double num2 = 0.0;
            int num3 = 0;
            foreach (Execution execution in account.simulationOverNight)
            {
                if (execution.Symbol.IsEqual(this.order.Symbol))
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
            ExecutionCollection executions = new ExecutionCollection();
            foreach (Execution execution2 in account.Executions)
            {
                executions.Add(execution2);
            }
            executions.Add(newExecution);
            foreach (Execution execution3 in executions)
            {
                if (!execution3.Symbol.IsEqual(this.order.Symbol))
                {
                    continue;
                }
                if (num == 0)
                {
                    if (execution3.MarketPosition.Id == MarketPositionId.Long)
                    {
                        num3 += execution3.Quantity;
                        num2 += execution3.AvgPrice * execution3.Quantity;
                        num += execution3.Quantity;
                    }
                    else
                    {
                        num3 += execution3.Quantity;
                        num2 += execution3.AvgPrice * execution3.Quantity;
                        num -= execution3.Quantity;
                    }
                    continue;
                }
                if (num > 0)
                {
                    if (execution3.MarketPosition.Id == MarketPositionId.Long)
                    {
                        num3 += execution3.Quantity;
                        num2 += execution3.AvgPrice * execution3.Quantity;
                        num += execution3.Quantity;
                    }
                    else if (num == execution3.Quantity)
                    {
                        num = 0;
                        num2 = 0.0;
                        num3 = 0;
                    }
                    else if (num > execution3.Quantity)
                    {
                        num -= execution3.Quantity;
                    }
                    else
                    {
                        num3 = execution3.Quantity - num;
                        num2 = num3 * execution3.AvgPrice;
                        num = -num3;
                    }
                    continue;
                }
                if (execution3.MarketPosition.Id == MarketPositionId.Short)
                {
                    num3 += execution3.Quantity;
                    num2 += execution3.AvgPrice * execution3.Quantity;
                    num -= execution3.Quantity;
                }
                else
                {
                    if (-num == execution3.Quantity)
                    {
                        num = 0;
                        num2 = 0.0;
                        num3 = 0;
                        continue;
                    }
                    if (-num > execution3.Quantity)
                    {
                        num += execution3.Quantity;
                        continue;
                    }
                    num3 = execution3.Quantity - -num;
                    num2 = num3 * execution3.AvgPrice;
                    num = num3;
                }
            }
            if (num3 != 0)
            {
                return (num2 / ((double) num3));
            }
            return 0.0;
        }

        private void MarketData_MarketDataItem(object sender, MarketDataEventArgs e)
        {
            int volume = e.Volume;
            if (volume == 0)
            {
                volume = (this.order.Symbol.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1;
            }
            if (this.orderType.Id == OrderTypeId.Market)
            {
                if (((this.order.OrderState.Id == OrderStateId.Working) || (this.order.OrderState.Id == OrderStateId.PartFilled)) && ((((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) && (e.MarketDataType.Id == MarketDataTypeId.Ask)) || (((this.order.Action.Id == ActionTypeId.Sell) || (this.order.Action.Id == ActionTypeId.SellShort)) && (e.MarketDataType.Id == MarketDataTypeId.Bid))))
                {
                    this.Fill(Math.Min(volume, this.quantity - this.order.Filled), e.Price);
                }
            }
            else if (this.orderType.Id == OrderTypeId.Limit)
            {
                if ((this.order.OrderState.Id == OrderStateId.Working) || (this.order.OrderState.Id == OrderStateId.PartFilled))
                {
                    if ((((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) && ((e.MarketDataType.Id == MarketDataTypeId.Ask) && ((e.Price + this.Epsilon) < this.limitPrice))) || (((this.order.Action.Id == ActionTypeId.Sell) || (this.order.Action.Id == ActionTypeId.SellShort)) && ((e.MarketDataType.Id == MarketDataTypeId.Bid) && ((e.Price - this.Epsilon) > this.limitPrice))))
                    {
                        this.Fill(Math.Min(volume, this.quantity - this.order.Filled), ((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) ? Math.Min(this.limitPrice, e.Price) : Math.Max(this.limitPrice, e.Price));
                    }
                    else if (((((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) && ((e.MarketDataType.Id == MarketDataTypeId.Last) && (this.order.Symbol.MarketData.Bid != null))) && (((this.order.Symbol.MarketData.Bid.Price < (this.limitPrice - this.Epsilon)) && (this.order.Symbol.MarketData.Ask != null)) && (((this.order.Symbol.MarketData.Ask.Price > (this.limitPrice + this.Epsilon)) && (this.order.Symbol.MarketData.Last != null)) && (this.order.Symbol.MarketData.Last.Price < (this.limitPrice + this.Epsilon))))) || (((((this.order.Action.Id == ActionTypeId.Sell) || (this.order.Action.Id == ActionTypeId.SellShort)) && ((e.MarketDataType.Id == MarketDataTypeId.Last) && (this.order.Symbol.MarketData.Bid != null))) && (((this.order.Symbol.MarketData.Bid.Price < (this.limitPrice - this.Epsilon)) && (this.order.Symbol.MarketData.Ask != null)) && ((this.order.Symbol.MarketData.Ask.Price > (this.limitPrice + this.Epsilon)) && (this.order.Symbol.MarketData.Last != null)))) && (this.order.Symbol.MarketData.Last.Price > (this.limitPrice - this.Epsilon))))
                    {
                        this.Fill(Math.Min(volume, this.quantity - this.order.Filled), e.Price);
                    }
                    else if ((e.MarketDataType.Id == MarketDataTypeId.Last) && (Math.Abs((double) (e.Price - this.order.LimitPrice)) < this.Epsilon))
                    {
                        if ((volume < this.queueSize) && (this.queuePosition > this.order.Quantity))
                        {
                            this.queuePosition -= (int) ((((double) this.queuePosition) / ((double) this.queueSize)) * volume);
                        }
                        if (this.queuePosition <= this.order.Quantity)
                        {
                            this.Fill(Math.Min(volume, this.quantity - this.order.Filled), this.order.LimitPrice);
                        }
                    }
                }
            }
            else if ((((this.orderType.Id == OrderTypeId.Stop) || (this.orderType.Id == OrderTypeId.StopLimit)) && ((((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) && ((e.MarketDataType.Id == MarketDataTypeId.Last) && (e.Price >= (this.stopPrice - this.Epsilon)))) || (((this.order.Action.Id == ActionTypeId.Sell) || (this.order.Action.Id == ActionTypeId.SellShort)) && ((e.MarketDataType.Id == MarketDataTypeId.Last) && (e.Price <= (this.stopPrice + this.Epsilon)))))) && this.CheckExcessEquity())
            {
                this.orderType = OrderType.All[(this.orderType.Id == OrderTypeId.Stop) ? OrderTypeId.Market : OrderTypeId.Limit];
                this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Working], this.order.connection.Now));
                if (this.orderType.Id == OrderTypeId.Limit)
                {
                    this.PlaceInQueue();
                }
            }
        }

        internal void MarketDepth_MarketDepthItem(object sender, MarketDepthEventArgs e)
        {
            if (((this.orderType.Id == OrderTypeId.Limit) && ((this.order.OrderState.Id == OrderStateId.Working) || (this.order.OrderState.Id == OrderStateId.PartFilled))) && (e.Operation != Operation.Delete))
            {
                int volume = e.Volume;
                if (volume == 0)
                {
                    volume = (this.order.Symbol.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1;
                }
                if ((((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) && ((e.MarketDataType.Id == MarketDataTypeId.Bid) && (Math.Abs((double) (e.Price - this.order.LimitPrice)) < this.Epsilon))) || (((this.order.Action.Id == ActionTypeId.Sell) || (this.order.Action.Id == ActionTypeId.SellShort)) && ((e.MarketDataType.Id == MarketDataTypeId.Ask) && (Math.Abs((double) (e.Price - this.order.LimitPrice)) < this.Epsilon))))
                {
                    if ((volume < this.queueSize) && (this.queuePosition > this.order.Quantity))
                    {
                        this.queuePosition -= (int) ((((double) this.queuePosition) / ((double) this.queueSize)) * volume);
                    }
                    this.queueSize = Math.Max(volume, this.queuePosition);
                    if (this.queuePosition <= this.order.Quantity)
                    {
                        this.Fill(Math.Min(volume, this.quantity - this.order.Filled), this.order.LimitPrice);
                    }
                }
            }
        }

        internal void PlaceInQueue()
        {
            this.queuePosition = 0;
            for (int i = 0; i < 500; i++)
            {
                if ((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover))
                {
                    if (this.order.Symbol.MarketDepth.Bid.Count > 0)
                    {
                        double num2 = 0.0;
                        for (int j = 0; j < this.order.Symbol.MarketDepth.Bid.Count; j++)
                        {
                            if (Math.Abs((double) (this.order.Symbol.MarketDepth.Bid[j].price - this.order.LimitPrice)) < this.Epsilon)
                            {
                                num2 = this.order.Symbol.MarketDepth.Bid[j].price * this.order.Symbol.MarketDepth.Bid.Count;
                                break;
                            }
                            num2 += this.order.Symbol.MarketDepth.Bid[j].price;
                        }
                        this.queuePosition = ((int) num2) / this.order.Symbol.MarketDepth.Bid.Count;
                        break;
                    }
                    if ((this.order.Symbol.MarketData.Bid == null) || (this.order.Symbol.MarketData.Ask == null))
                    {
                        goto Label_037B;
                    }
                    this.queuePosition = ((int) (this.order.Symbol.MarketData.Bid.Price + this.order.Symbol.MarketData.Ask.Price)) / 2;
                    break;
                }
                if ((this.order.Action.Id == ActionTypeId.Sell) || (this.order.Action.Id == ActionTypeId.SellShort))
                {
                    if (this.order.Symbol.MarketDepth.Ask.Count > 0)
                    {
                        double num4 = 0.0;
                        for (int k = 0; k < this.order.Symbol.MarketDepth.Ask.Count; k++)
                        {
                            if (Math.Abs((double) (this.order.Symbol.MarketDepth.Ask[k].price - this.order.LimitPrice)) < this.Epsilon)
                            {
                                num4 = this.order.Symbol.MarketDepth.Ask[k].price * this.order.Symbol.MarketDepth.Ask.Count;
                                break;
                            }
                            num4 += this.order.Symbol.MarketDepth.Ask[k].price;
                        }
                        this.queuePosition = ((int) num4) / this.order.Symbol.MarketDepth.Ask.Count;
                        break;
                    }
                    if ((this.order.Symbol.MarketData.Bid != null) && (this.order.Symbol.MarketData.Ask != null))
                    {
                        this.queuePosition = ((int) (this.order.Symbol.MarketData.Bid.Price + this.order.Symbol.MarketData.Ask.Price)) / 2;
                        break;
                    }
                }
            Label_037B:
                Application.DoEvents();
                Thread.Sleep(10);
            }
            if (this.queuePosition == 0)
            {
                if (((this.order.Action.Id != ActionTypeId.Buy) && (this.order.Action.Id != ActionTypeId.BuyToCover)) || ((this.order.Symbol.MarketData.Bid == null) || (this.order.Symbol.MarketData.Ask == null)))
                {
                    if (((this.order.Action.Id != ActionTypeId.Sell) && (this.order.Action.Id != ActionTypeId.SellShort)) || ((this.order.Symbol.MarketData.Bid == null) || (this.order.Symbol.MarketData.Ask == null)))
                    {
                        this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.OrderRejected, "Unable to run simulator: no market (depth) data available", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Rejected], this.order.connection.Now));
                        return;
                    }
                    this.queuePosition = ((int) (this.order.Symbol.MarketData.Bid.Price + this.order.Symbol.MarketData.Ask.Price)) / 2;
                }
                else
                {
                    this.queuePosition = ((int) (this.order.Symbol.MarketData.Bid.Price + this.order.Symbol.MarketData.Ask.Price)) / 2;
                }
            }
            this.queueSize = this.queuePosition;
        }

        internal void Submit()
        {
            for (int i = this.order.Account.SimulationAccountOptions.WaitForMarketDataSeconds * 0x3e8; i > 0; i -= 10)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                if (this.order.Symbol.MarketData.Last != null)
                {
                    break;
                }
            }
            if (this.order.Symbol.MarketData.Last == null)
            {
                this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.OrderRejected, "Order can't be submitted, since there is no market data (yet). Turn on the market data stream and try again later.", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Rejected], this.order.connection.Now));
            }
            else
            {
                this.limitPrice = this.order.LimitPrice;
                this.quantity = this.order.Quantity;
                this.stopPrice = this.order.StopPrice;
                this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.PendingSubmit], this.order.connection.Now));
                this.timerSubmit = new System.Timers.Timer((double) (this.order.Account.simulationAccountOptions.DelayExchange + (2 * this.order.Account.simulationAccountOptions.DelayCommunication)));
                this.timerSubmit.Elapsed += new ElapsedEventHandler(this.timerSubmit_Elapsed);
                this.timerSubmit.AutoReset = false;
                this.timerSubmit.Enabled = true;
            }
        }

        private void timerCancel_Elapsed(object source, ElapsedEventArgs e)
        {
            this.order.connection.SynchronizeInvoke.AsyncInvoke(new MethodInvoker(this.timerCancel_ElapsedNow), null);
        }

        private void timerCancel_ElapsedNow()
        {
            this.timerCancel = null;
            this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Cancelled], this.order.connection.Now));
        }

        private void timerChange_Elapsed(object source, ElapsedEventArgs e)
        {
            this.order.connection.SynchronizeInvoke.AsyncInvoke(new MethodInvoker(this.timerChange_ElapsedNow), null);
        }

        private void timerChange_ElapsedNow()
        {
            this.timerChange = null;
            if (this.orderType.Id == OrderTypeId.Limit)
            {
                this.PlaceInQueue();
            }
            this.limitPrice = this.order.LimitPrice;
            this.quantity = this.order.Quantity;
            this.stopPrice = this.order.StopPrice;
            this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[(this.lastOrderState.Id == OrderStateId.Accepted) ? OrderStateId.Accepted : OrderStateId.Accepted], this.order.connection.Now));
            if (this.CheckExcessEquity())
            {
                this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[(this.lastOrderState.Id == OrderStateId.Accepted) ? OrderStateId.Accepted : OrderStateId.Working], this.order.connection.Now));
            }
        }

        private void timerSubmit_Elapsed(object source, ElapsedEventArgs e)
        {
            this.order.connection.SynchronizeInvoke.AsyncInvoke(new MethodInvoker(this.timerSubmit_ElapsedNow), null);
        }

        private void timerSubmit_ElapsedNow()
        {
            if (this.orderType.Id == OrderTypeId.Limit)
            {
                this.PlaceInQueue();
            }
            this.timerSubmit = null;
            this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Accepted], this.order.connection.Now));
            int num = 0;
            foreach (Position position in this.order.Account.Positions)
            {
                if (position.Symbol.IsEqual(this.order.Symbol))
                {
                    num += position.Quantity * ((position.MarketPosition.Id == MarketPositionId.Long) ? 1 : -1);
                }
            }
            num += this.order.Quantity * (((this.order.Action.Id == ActionTypeId.Buy) || (this.order.Action.Id == ActionTypeId.BuyToCover)) ? 1 : -1);
            Trace.Assert(this.order.Symbol.MarketData.Last != null, "Cbi.Simulator.timerSubmit_ElapsedNow");
            if (this.CheckExcessEquity() && ((this.orderType.Id == OrderTypeId.Market) || (this.orderType.Id == OrderTypeId.Limit)))
            {
                this.order.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, ErrorCode.NoError, "", this.order.Token, this.limitPrice, this.stopPrice, this.quantity, this.order.AvgFillPrice, this.order.Filled, OrderState.All[OrderStateId.Working], this.order.connection.Now));
            }
        }

        private double Epsilon
        {
            get
            {
                return (this.order.Symbol.TickSize / 4.0);
            }
        }
    }
}

