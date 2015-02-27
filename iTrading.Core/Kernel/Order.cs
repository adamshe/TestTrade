using System.Windows.Forms;
using iTrading.Core.Data;


namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an order.
    /// </summary>
    [Guid("3F1B427B-690B-4d9d-9709-5F992B302F21"), ClassInterface(ClassInterfaceType.None)]
    public class Order : StreamingRequest, IComOrder, ITradingSerializable
    {
        private Account account;
        private ActionType action;
        private double avgFillPrice;
        private string customText;
        private int filled;
        private OrderStatusEventCollection history;
        private double limitPrice;
        private string ocaGroup;
        internal string orderId;
        private iTrading.Core.Kernel.OrderState orderState;
        private iTrading.Core.Kernel.OrderType orderType;
        private int quantity;
        internal Simulator simulator;
        private double stopPrice;
        private iTrading.Core.Kernel.Symbol symbol;
        private DateTime time;
        private iTrading.Core.Kernel.TimeInForce timeInForce;
        private string token;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public Order(Bytes bytes, int version) : base(bytes, version)
        {
            this.avgFillPrice = 0.0;
            this.customText = "";
            this.filled = 0;
            this.orderId = "";
            this.ocaGroup = "";
            this.simulator = null;
            this.history = new OrderStatusEventCollection();
            this.action = bytes.ReadActionType();
            this.account = bytes.ReadAccount();
            this.avgFillPrice = bytes.ReadDouble();
            this.customText = bytes.ReadString();
            this.filled = bytes.ReadInt32();
            this.limitPrice = bytes.ReadDouble();
            this.ocaGroup = bytes.ReadString();
            this.orderId = bytes.ReadString();
            this.orderType = bytes.ReadOrderType();
            this.quantity = bytes.ReadInt32();
            this.orderState = bytes.ReadOrderState();
            this.stopPrice = bytes.ReadDouble();
            this.symbol = bytes.ReadSymbol();
            this.time = bytes.ReadDateTime();
            this.timeInForce = bytes.ReadTimeInForce();
            this.token = bytes.ReadString();
            int num = bytes.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                this.history.Add(new OrderStatusEventArgs(this, (ErrorCode) bytes.ReadInt32(), bytes.ReadString(), bytes, version));
            }
        }

        /// <summary>
        /// For internal use only. See <see cref="M:iTrading.Core.Kernel.Account.CreateOrder(iTrading.Core.Kernel.Symbol,iTrading.Core.Kernel.ActionTypeId,iTrading.Core.Kernel.OrderTypeId,iTrading.Core.Kernel.TimeInForceId,System.Int32,System.Double,System.Double,System.String,System.Object)" /> for creating orders.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="symbol"></param>
        /// <param name="action"></param>
        /// <param name="orderType"></param>
        /// <param name="filled"></param>
        /// <param name="avgFillPrice"></param>
        /// <param name="timeInForce"></param>
        /// <param name="quantity"></param>
        /// <param name="limitPrice"></param>
        /// <param name="stopPrice"></param>
        /// <param name="token"></param>
        /// <param name="initialOrderId"></param>
        /// <param name="ocaGroup"></param>
        /// <param name="initialOrderState"></param>
        /// <param name="time"></param>
        public Order(Account account, iTrading.Core.Kernel.Symbol symbol, ActionType action, iTrading.Core.Kernel.OrderType orderType, iTrading.Core.Kernel.TimeInForce timeInForce, int quantity, double limitPrice, double stopPrice, string token, iTrading.Core.Kernel.OrderState initialOrderState, string initialOrderId, string ocaGroup, int filled, double avgFillPrice, DateTime time) : base(account.Connection)
        {
            this.avgFillPrice = 0.0;
            this.customText = "";
            this.filled = 0;
            this.orderId = "";
            this.ocaGroup = "";
            this.simulator = null;
            this.orderId = (initialOrderId.Length == 0) ? string.Concat(new object[] { account.IsSimulation ? "SIM" : "TM", base.Id, "_", account.Connection.ClientId }) : initialOrderId;
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", account.Connection.IdPlus, ") Cbi.Order.ctor1: id='", this.orderId, "' state='", initialOrderState.Name, "' symbol='", (symbol == null) ? "null" : symbol.FullName, "' quantity=", quantity, " filled=", filled, " price=", avgFillPrice }));
            }
            this.account = account;
            this.action = action;
            this.avgFillPrice = avgFillPrice;
            this.filled = filled;
            this.history = new OrderStatusEventCollection();
            this.limitPrice = (symbol != null) ? symbol.Round2TickSize(limitPrice) : limitPrice;
            this.quantity = quantity;
            this.ocaGroup = ocaGroup;
            this.orderState = initialOrderState;
            this.stopPrice = (symbol != null) ? symbol.Round2TickSize(stopPrice) : stopPrice;
            this.symbol = symbol;
            this.orderType = orderType;
            this.time = time;
            this.timeInForce = timeInForce;
            this.token = (token.Length == 0) ? ((account.IsSimulation ? "SIM_" : "TM_") + Guid.NewGuid().ToString()) : token;
            if (((orderType.Id == OrderTypeId.Limit) || (orderType.Id == OrderTypeId.StopLimit)) && (this.limitPrice == 0.0))
            {
                Trace.WriteLine("WARNING: Cbi.Order.ctor: '" + this.orderId + "': limit price must not be 0");
            }
            if (((orderType.Id == OrderTypeId.Stop) || (orderType.Id == OrderTypeId.StopLimit)) && (this.stopPrice == 0.0))
            {
                Trace.WriteLine("WARNING: Cbi.Order.ctor: '" + this.orderId + "': stop price must not be 0");
            }
            if ((symbol != null) && (account.Connection != symbol.Connection))
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Symbol and Cbi.Account must belong to the same connection");
            }
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + account.Connection.IdPlus + ") Cbi.Order.ctor2: " + this.ToString());
            }
        }

        /// <summary>
        /// Cancels the actual order. Please note: This has no effect, if <see cref="P:iTrading.Core.Kernel.Order.OrderState" /> 
        /// is <see cref="F:iTrading.Core.Kernel.OrderStateId.Initialized" /> or <see cref="F:iTrading.Core.Kernel.OrderStateId.Cancelled" />.
        /// </summary>
        public override void Cancel()
        {
            base.Connection.SynchronizeInvoke.Invoke(new MethodInvoker(this.CancelNow), null);
        }

        private void CancelNow()
        {
            try
            {
                if ((base.Connection.ConnectionStatusId != ConnectionStatusId.Connected) && (base.Connection.ConnectionStatusId != ConnectionStatusId.Connecting))
                {
                    this.account.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.UnableToCancelOrder, "Order '" + this.OrderId + "' can't be cancelled: connection is unavailable", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.OrderState, this.account.Connection.Now));
                }
                else if ((((this.OrderState.Id != OrderStateId.Cancelled) && (this.OrderState.Id != OrderStateId.Initialized)) && (this.OrderState.Id != OrderStateId.Filled)) && (this.OrderState.Id != OrderStateId.Rejected))
                {
                    if (this.OrderState.Id == OrderStateId.Unknown)
                    {
                        base.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.NoError, "", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.account.IsSimulation ? iTrading.Core.Kernel.OrderState.All[OrderStateId.PendingCancel] : base.Connection.OrderStates[OrderStateId.PendingCancel], base.Connection.Now));
                        base.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.NoError, "", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.account.IsSimulation ? iTrading.Core.Kernel.OrderState.All[OrderStateId.Cancelled] : base.Connection.OrderStates[OrderStateId.Cancelled], base.Connection.Now));
                    }
                    else
                    {
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.Order.CancelNow: " + this.ToString());
                        }
                        base.operation = Operation.Delete;
                        if (this.account.IsSimulation)
                        {
                            if (this.simulator == null)
                            {
                                this.simulator = new Simulator(this, SimulationSymbolOptions.Restore(this.customText));
                            }
                            this.simulator.Cancel();
                        }
                        else
                        {
                            base.Connection.order.Cancel(this);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.Order.Cancel: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Change order attributes at the broker's system.
        /// </summary>
        public void Change()
        {
            base.Connection.SynchronizeInvoke.Invoke(new MethodInvoker(this.ChangeNow), null);
        }

        private void ChangeNow()
        {
            try
            {
                if (base.Connection.ConnectionStatusId != ConnectionStatusId.Connected)
                {
                    this.account.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.UnableToChangeOrder, "Order '" + this.OrderId + "' can't be changed: connection is unavailable", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.OrderState, this.account.Connection.Now));
                }
                else if ((((this.OrderState.Id == OrderStateId.Cancelled) || (this.OrderState.Id == OrderStateId.PendingCancel)) || ((this.OrderState.Id == OrderStateId.Filled) || (this.OrderState.Id == OrderStateId.Initialized))) || (((this.OrderState.Id == OrderStateId.PendingSubmit) || (this.OrderState.Id == OrderStateId.Rejected)) || (this.OrderState.Id == OrderStateId.Unknown)))
                {
                    this.account.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.UnableToChangeOrder, string.Concat(new object[] { "Order '", this.OrderId, "' can't be changed: order status is ", this.OrderState.Id, "." }), this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.OrderState, this.account.Connection.Now));
                }
                else if (!this.account.IsSimulation && (base.Connection.orderChange == null))
                {
                    this.account.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.UnableToChangeOrder, "Provider does not support order changes", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.OrderState, this.account.Connection.Now));
                }
                else
                {
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.Order.Change: " + this.ToString());
                    }
                    base.operation = Operation.Update;
                    if (this.account.IsSimulation)
                    {
                        if (this.simulator == null)
                        {
                            this.simulator = new Simulator(this, SimulationSymbolOptions.Restore(this.customText));
                        }
                        this.simulator.Change();
                    }
                    else
                    {
                        base.Connection.orderChange.Change(this);
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.Order.ChangeNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Manually fill orders in state <see cref="F:iTrading.Core.Kernel.OrderStateId.Unknown" />.
        /// </summary>
        /// <param name="filled">set the fill quantity</param>
        /// <param name="avgFillPrice"> set the avg. fill price</param>
        public void Fill(int filled, double avgFillPrice)
        {
            if (this.OrderState.Id != OrderStateId.Unknown)
            {
                base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.UnableToPerformAction, "", "Only orders in state 'Unknown' can be filled manually."));
            }
            else
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.Order.Fill: " + this.ToString());
                }
                base.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.NoError, "", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, avgFillPrice, this.quantity, this.account.IsSimulation ? iTrading.Core.Kernel.OrderState.All[OrderStateId.Filled] : base.Connection.OrderStates[OrderStateId.Filled], base.Connection.Now));
            }
        }

        internal static void HandleOcaOrdersNow(object eventArgs, object f)
        {
            try
            {
                OrderStatusEventArgs args = (OrderStatusEventArgs) eventArgs;
                int num1 = (int) f;
                lock (args.Order.Account.Orders)
                {
                    foreach (Order order in args.Order.Account.Orders)
                    {
                        if (!(order.OcaGroup == args.Order.OcaGroup) || (order == args.Order))
                        {
                            continue;
                        }
                        if (((args.OrderState.Id == OrderStateId.Rejected) || (args.OrderState.Id == OrderStateId.PendingCancel)) || ((args.OrderState.Id == OrderStateId.Cancelled) || (args.OrderState.Id == OrderStateId.Filled)))
                        {
                            if (order.OrderState.Id != OrderStateId.PendingCancel)
                            {
                                order.Cancel();
                            }
                            continue;
                        }
                        if (((args.OrderState.Id == OrderStateId.PartFilled) && (args.Order.Account.IsSimulation || (args.order.connection.FeatureTypes[FeatureTypeId.OrderChange] != null))) && ((args.order.connection.Options.SimOcaPartialFills && (args.Order.History.Count > 0)) && (order.History.Count > 0)))
                        {
                            int quantity = args.Order.History[0].Quantity;
                            int num2 = order.History[0].Quantity;
                            order.Quantity = (int) Math.Ceiling((double) ((num2 * (quantity - args.Filled)) / ((double) quantity)));
                            order.Change();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ((OrderStatusEventArgs) eventArgs).order.connection.ProcessEventArgs(new ITradingErrorEventArgs(((OrderStatusEventArgs) eventArgs).order.connection, ErrorCode.Panic, "", "Cbi.Order.HandleOcaOrdersNow: exception caught: " + exception.Message));
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
            bytes.Write(this.action);
            bytes.Write(this.account);
            bytes.Write(this.avgFillPrice);
            bytes.Write(this.customText);
            bytes.Write(this.filled);
            bytes.Write(this.limitPrice);
            bytes.Write(this.ocaGroup);
            bytes.Write(this.orderId);
            bytes.Write(this.orderType);
            bytes.Write(this.quantity);
            bytes.Write(this.orderState);
            bytes.Write(this.stopPrice);
            bytes.WriteSymbol(this.symbol);
            bytes.Write(this.time);
            bytes.Write(this.timeInForce);
            bytes.Write(this.token);
            bytes.Write(this.history.Count);
            foreach (OrderStatusEventArgs args in this.history)
            {
                bytes.Write((int) args.Error);
                bytes.Write(args.NativeError);
                args.Serialize(bytes, version, true);
            }
        }

        /// <summary>
        /// For internal user only.
        /// </summary>
        /// <param name="ocaGroup"></param>
        public void SetOcaGroup(string ocaGroup)
        {
            this.ocaGroup = ocaGroup;
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="token"></param>
        public void SetToken(string token)
        {
            this.token = token;
        }

        /// <overloads>Submits the order.</overloads>
        /// <summary>
        /// Submits the order to the broker. Please note: This operation has no effect, 
        /// when <see cref="P:iTrading.Core.Kernel.Order.OrderState" /> != <see cref="F:iTrading.Core.Kernel.OrderStateId.Initialized" />.
        /// </summary>
        public void Submit()
        {
            base.Connection.SynchronizeInvoke.Invoke(new Connection.WorkerArgs1(this.SubmitNow), new object[1]);
        }

        /// <summary>
        /// Submits the order to a simulation account.
        /// Please note: This method may only be called on simulation accounts.
        /// </summary>
        /// <param name="simulationSymbolOptions">Set the options for this order. If NULL, default options will be used.</param>
        public void Submit(SimulationSymbolOptions simulationSymbolOptions)
        {
            if (!this.account.IsSimulation)
            {
                throw new TMException(ErrorCode.Panic, "This method may only be called on simulation accounts.");
            }
            base.Connection.SynchronizeInvoke.Invoke(new Connection.WorkerArgs1(this.SubmitNow), new object[] { simulationSymbolOptions });
        }

        private void SubmitNow(object simulationSymbolOptions)
        {
            try
            {
                if (this.account.Connection.ConnectionStatusId != ConnectionStatusId.Connected)
                {
                    this.account.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.UnableToSubmitOrder, "Order '" + this.OrderId + "' can't be submitted: connection is unavailable", this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.account.IsSimulation ? iTrading.Core.Kernel.OrderState.All[OrderStateId.Rejected] : base.Connection.OrderStates[OrderStateId.Rejected], base.Connection.Now));
                }
                else if (this.OrderState.Id != OrderStateId.Initialized)
                {
                    this.account.Connection.ProcessEventArgs(new OrderStatusEventArgs(this, ErrorCode.UnableToSubmitOrder, string.Concat(new object[] { "Order '", this.OrderId, "' can't be submitted: order status is ", this.OrderState.Id, "." }), this.OrderId, this.LimitPrice, this.StopPrice, this.Quantity, this.AvgFillPrice, this.Filled, this.OrderState, this.account.Connection.Now));
                }
                else
                {
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine("(" + base.Connection.IdPlus + ") Cbi.Order.Submit: " + this.ToString());
                    }
                    base.operation = Operation.Insert;
                    if (this.account.IsSimulation)
                    {
                        if (this.simulator == null)
                        {
                            this.simulator = new Simulator(this, (simulationSymbolOptions == null) ? SimulationSymbolOptions.GetDefaultSimulationSymbolOptions(this.Symbol) : ((SimulationSymbolOptions) simulationSymbolOptions));
                        }
                        this.customText = this.simulator.simulationSymbolOptions.Save(this.customText);
                        Globals.DB.Update(this, false);
                        this.simulator.Submit();
                    }
                    else
                    {
                        base.Connection.order.Submit(this);
                    }
                }
            }
            catch (Exception exception)
            {
                base.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.connection, ErrorCode.Panic, "", "Cbi.Order.SubmitNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Returns the printable string value of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(new object[] { 
                "id ='", this.orderId, "' state=", this.orderState.Id, " symbol='", (this.symbol == null) ? "null" : this.symbol.FullName, "' action=", this.action.Id, " limit=", this.limitPrice, " stop=", this.stopPrice, " quantity=", this.quantity, " type=", this.orderType.Id, 
                " tif=", this.timeInForce.Id, " oca='", this.ocaGroup, "' filled=", this.filled, " price=", this.avgFillPrice, " token='", this.token, "'"
             });
        }

        internal void Update(OrderStatusEventArgs eventArgs)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.connection.IdPlus + ") Cbi.Order.Update: oldid='" + this.orderId + "' " + eventArgs.ToString());
            }
            if ((eventArgs.OrderState.Id != OrderStateId.Initialized) || (this.history.Count == 0))
            {
                lock (this.history)
                {
                    this.history.Add(eventArgs);
                }
            }
            this.orderState = eventArgs.OrderState;
            if (((eventArgs.Error == ErrorCode.NoError) || (eventArgs.Error == ErrorCode.UnableToChangeOrder)) || (eventArgs.Error == ErrorCode.UnableToCancelOrder))
            {
                this.avgFillPrice = eventArgs.AvgFillPrice;
                this.filled = eventArgs.Filled;
                this.orderId = eventArgs.OrderId;
                this.quantity = eventArgs.Quantity;
                this.time = eventArgs.Time;
                if (((this.orderType.Id == OrderTypeId.Limit) || (this.orderType.Id == OrderTypeId.StopLimit)) && (eventArgs.LimitPrice > 0.0))
                {
                    this.limitPrice = eventArgs.LimitPrice;
                }
                if (((this.orderType.Id == OrderTypeId.Stop) || (this.orderType.Id == OrderTypeId.StopLimit)) && (eventArgs.StopPrice > 0.0))
                {
                    this.stopPrice = eventArgs.StopPrice;
                }
            }
        }

        /// <summary>
        /// The account the order belongs to.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
        }

        /// <summary>
        /// Identifies the order's action.
        /// </summary>
        public ActionType Action
        {
            get
            {
                return this.action;
            }
        }

        /// <summary>
        /// The average fill price of all partial fills.
        /// </summary>
        public double AvgFillPrice
        {
            get
            {
                return this.avgFillPrice;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.Order.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.Order;
            }
        }

        /// <summary>
        /// Get/set custom text. This text must be well formatted xml text. 
        /// TradeMagic itself adds internal information and relies on well formatted xml text.
        /// Make sure the root node name is "TradeMagic" and you do not use node names starting with underscore "_".
        /// </summary>
        public string CustomText
        {
            get
            {
                if (this.customText != null)
                {
                    return this.customText;
                }
                return "";
            }
            set
            {
                this.customText = value;
            }
        }

        /// <summary>
        /// The number of units filled.
        /// </summary>
        public int Filled
        {
            get
            {
                return this.filled;
            }
        }

        /// <summary>
        /// Get the history of <see cref="T:iTrading.Core.Kernel.OrderStatusEventArgs" /> events.
        /// </summary>
        public OrderStatusEventCollection History
        {
            get
            {
                return this.history;
            }
        }

        /// <summary>
        /// Limit price. Is valid for stop and stop limit orders. Default = 0.
        /// Please note: Setting <see cref="P:iTrading.Core.Kernel.Order.LimitPrice" /> only becomes effective after calling <see cref="M:iTrading.Core.Kernel.Order.Change" />.
        /// </summary>
        public double LimitPrice
        {
            get
            {
                return this.limitPrice;
            }
            set
            {
                if (((this.orderType.Id == OrderTypeId.Limit) || (this.orderType.Id == OrderTypeId.StopLimit)) && (value == 0.0))
                {
                    throw new TMException(ErrorCode.InvalidOrderPrice, "Order.LimitPrice: '" + this.orderId + "': limit price must not be 0");
                }
                this.limitPrice = this.symbol.Round2TickSize(value);
            }
        }

        /// <summary>
        /// Get OCA group. All orders of an account having the same <see cref="P:iTrading.Core.Kernel.Order.OcaGroup" /> property
        /// belong to the same OCA group.
        /// </summary>
        public string OcaGroup
        {
            get
            {
                if (this.ocaGroup != null)
                {
                    return this.ocaGroup;
                }
                return "";
            }
        }

        /// <summary>
        /// Identifies the order.
        /// </summary>
        public string OrderId
        {
            get
            {
                if (this.orderId != null)
                {
                    return this.orderId;
                }
                return "";
            }
        }

        /// <summary>
        /// The actual order state.
        /// </summary>
        public iTrading.Core.Kernel.OrderState OrderState
        {
            get
            {
                return this.orderState;
            }
        }

        /// <summary>
        /// Order type.
        /// </summary>
        public iTrading.Core.Kernel.OrderType OrderType
        {
            get
            {
                return this.orderType;
            }
        }

        /// <summary>
        /// Number of shares or contract to execute.
        /// Please note: Setting <see cref="P:iTrading.Core.Kernel.Order.Quantity" /> only becomes effective after calling <see cref="M:iTrading.Core.Kernel.Order.Change" />.
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }

        /// <summary>
        /// Stop price. Is valid for stop market and stop limit orders. default = 0.
        /// Please note: Setting <see cref="P:iTrading.Core.Kernel.Order.StopPrice" /> only becomes effective after calling <see cref="M:iTrading.Core.Kernel.Order.Change" />.
        /// </summary>
        public double StopPrice
        {
            get
            {
                return this.stopPrice;
            }
            set
            {
                if (((this.orderType.Id == OrderTypeId.Stop) || (this.orderType.Id == OrderTypeId.StopLimit)) && (value == 0.0))
                {
                    throw new TMException(ErrorCode.InvalidOrderPrice, "Order.StopPrice: '" + this.orderId + "': stop price must not be 0");
                }
                this.stopPrice = this.symbol.Round2TickSize(value);
            }
        }

        /// <summary>
        /// Identifies the share/contract to order.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        /// <summary>
        /// Time of last change.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// The actual time in force setting.
        /// </summary>
        public iTrading.Core.Kernel.TimeInForce TimeInForce
        {
            get
            {
                return this.timeInForce;
            }
        }

        /// <summary>
        /// TradeMagic internal permanent order identifier. This identifier never changes throughout the lifetime
        /// of the order.
        /// </summary>
        public string Token
        {
            get
            {
                if (this.token != null)
                {
                    return this.token;
                }
                return "";
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

