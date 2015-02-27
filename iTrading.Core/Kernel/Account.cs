using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using iTrading.Core.Kernel;
using iTrading.Core.Interface;

namespace iTrading.Core.Kernel
{
    /// <summary>
    /// Represents an account.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("A70781D5-B89D-466e-B1D1-C12D628FC2A1")]
    public class Account : IComAccount
    {
        private string accountName;
        private iTrading.Core.Kernel.Connection connection;
        private string customText = "";
        private ExecutionCollection executions;
        private bool isSimulation = false;
        private Hashtable items = new Hashtable();
        internal DateTime lastUpdate;
        private OrderCollection orders;
        private PositionCollection positions;
        internal iTrading.Core.Kernel.SimulationAccountOptions simulationAccountOptions = null;
        internal ExecutionCollection simulationOverNight = new ExecutionCollection();

        /// <summary>
        /// This event will be thrown when a <see cref="T:iTrading.Core.Kernel.AccountItem" /> is updated.
        /// </summary>
        public event AccountUpdateEventHandler AccountUpdate;

        /// <summary>
        /// This event will be thrown when an order is filled or upon opening the connection
        /// to indicate already existing executions.
        /// </summary>
        public event ExecutionUpdateEventHandler Execution;

        /// <summary>
        /// This event will be thrown when an order is updated.
        /// </summary>
        public event OrderStatusEventHandler OrderStatus;

        /// <summary>
        /// This event will be thrown when a position is updated.
        /// </summary>
        public event PositionUpdateEventHandler PositionUpdate;

        #region
        public void OnAccountUpdate (object pSender, AccountUpdateEventArgs pEvent)
        {
            if (AccountUpdate != null)
                AccountUpdate(pSender, pEvent);
        }

        public void OnExecutionUpdate(object pSender, ExecutionUpdateEventArgs pEvent)
        {
            if (Execution != null)
                Execution(pSender, pEvent);
        }

        public void OnOrderStatusUpdate(object pSender, OrderStatusEventArgs pEvent)
        {
            if (OrderStatus != null)
                OrderStatus(pSender, pEvent);
        }

        public void OnPositionUpdate(object pSender, PositionUpdateEventArgs pEvent)
        {
            if (PositionUpdate != null)
                PositionUpdate(pSender, pEvent);
        }
        #endregion
        internal Account(iTrading.Core.Kernel.Connection connection, string accountName, iTrading.Core.Kernel.SimulationAccountOptions simulationAccountOptions)
        {
            this.accountName = accountName;
            this.connection = connection;
            this.executions = new ExecutionCollection();
            this.isSimulation = simulationAccountOptions != null;
            this.lastUpdate = Globals.MaxDate;
            this.orders = new OrderCollection();
            this.positions = new PositionCollection();
            this.simulationAccountOptions = simulationAccountOptions;
        }

        /// <overloads>Creates an order.</overloads>
        /// <summary>
        /// Creates a new order. To submit the order, call <see cref="M:iTrading.Core.Kernel.Order.Submit" />
        /// </summary>
        /// <param name="symbol">Symbol to execute.</param>
        /// <param name="action">Order action.</param>
        /// <param name="orderType">Order type.</param>
        /// <param name="timeInForce">Time in force.</param>
        /// <param name="quantity">Number of units to order.</param>
        /// <param name="limitPrice">Limit price, if any. Set 0 if not needed.</param>
        /// <param name="stopPrice">Stop price, if any. Set 0 if not needed.</param>
        /// <param name="customLink">Link to immediately attach custom objects to the order.</param>
        /// <param name="ocaGroup">All orders of an account having the same parameter value belong to the same OCA group.
        /// Set "" or NULL if not applicable.</param>
        public Order CreateOrder(Symbol symbol, ActionTypeId action, OrderTypeId orderType, TimeInForceId timeInForce, int quantity, double limitPrice, double stopPrice, string ocaGroup, object customLink)
        {
            return this.CreateOrder(symbol, action, orderType, timeInForce, quantity, limitPrice, stopPrice, "", this.IsSimulation ? OrderState.All[OrderStateId.Initialized] : this.Connection.OrderStates[OrderStateId.Initialized], "", (ocaGroup == null) ? "" : ocaGroup, null, customLink);
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="action"></param>
        /// <param name="orderType"></param>
        /// <param name="timeInForce"></param>
        /// <param name="quantity"></param>
        /// <param name="limitPrice"></param>
        /// <param name="stopPrice"></param>
        /// <param name="initialOrderId"></param>
        /// <param name="history"></param>
        /// <param name="token"></param>
        /// <param name="ocaGroup"></param>
        /// <param name="initialOrderState"></param>
        /// <param name="customLink"></param>
        public Order CreateOrder(Symbol symbol, ActionTypeId action, OrderTypeId orderType, TimeInForceId timeInForce, int quantity, double limitPrice, double stopPrice, string token, OrderState initialOrderState, string initialOrderId, string ocaGroup, OrderStatusEventCollection history, object customLink)
        {
            ActionType type = null;
            OrderType type2 = null;
            TimeInForce force = null;
            if (this.IsSimulation)
            {
                if (ActionType.All[action] == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Action type '" + action.ToString() + "' not supported by this provider");
                }
                action = ActionType.All[action].Id;
                if (TimeInForce.All[timeInForce] == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Time-in-force type '" + timeInForce.ToString() + "' not supported by this provider");
                }
                timeInForce = TimeInForce.All[timeInForce].Id;
                if (OrderType.All[orderType] == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Order type '" + orderType.ToString() + "' not supported by this provider");
                }
                orderType = OrderType.All[orderType].Id;
                if (initialOrderState != null)
                {
                    if (OrderState.All[initialOrderState.Id] == null)
                    {
                        throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Order state type '" + initialOrderState.Name + "' not supported by this provider");
                    }
                    initialOrderState = OrderState.All[initialOrderState.Id];
                }
                else
                {
                    initialOrderState = OrderState.All[OrderStateId.Initialized];
                }
                type = ActionType.All[action];
                type2 = OrderType.All[orderType];
                force = TimeInForce.All[timeInForce];
            }
            else
            {
                if (this.Connection.order == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Provider does not support order management");
                }
                if (this.Connection.ActionTypes[action] == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Action type '" + action.ToString() + "' not supported by this provider");
                }
                action = this.Connection.ActionTypes[action].Id;
                if (this.Connection.TimeInForces[timeInForce] == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Time-in-force type '" + timeInForce.ToString() + "' not supported by this provider");
                }
                timeInForce = this.Connection.TimeInForces[timeInForce].Id;
                if (this.Connection.OrderTypes[orderType] == null)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Order type '" + orderType.ToString() + "' not supported by this provider");
                }
                orderType = this.Connection.OrderTypes[orderType].Id;
                if (initialOrderState != null)
                {
                    if (this.Connection.OrderStates[initialOrderState.Id] == null)
                    {
                        throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Order state type '" + initialOrderState.Name + "' not supported by provider");
                    }
                    initialOrderState = this.Connection.OrderStates[initialOrderState.Id];
                }
                else
                {
                    initialOrderState = this.Connection.OrderStates[OrderStateId.Initialized];
                }
                type = this.Connection.ActionTypes[action];
                type2 = this.Connection.OrderTypes[orderType];
                force = this.Connection.TimeInForces[timeInForce];
            }
            if (type == null)
            {
                throw new TMException(ErrorCode.NotSupported, "action type '" + action.ToString() + "' not supported");
            }
            if (type2 == null)
            {
                throw new TMException(ErrorCode.NotSupported, "order type '" + type2.ToString() + "' not supported");
            }
            if (force == null)
            {
                throw new TMException(ErrorCode.NotSupported, "time in force type '" + force.ToString() + "' not supported");
            }
            if (initialOrderState == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Account.CreateOrder: Order state '" + OrderStateId.Initialized.ToString() + "' not supported by this provider");
            }
            if (symbol.Connection != this.Connection)
            {
                symbol = this.Connection.GetSymbol(symbol.Name, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, LookupPolicyId.RepositoryAndProvider);
                if (symbol == null)
                {
                    this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, ErrorCode.NoSuchSymbol, "", "Symbol '" + symbol.FullName + "' not supported by this provider"));
                    return null;
                }
            }
            try
            {
                Order order = new Order(this, symbol, type, type2, force, quantity, limitPrice, stopPrice, token, initialOrderState, initialOrderId, ocaGroup, 0, 0.0, this.Connection.Now);
                order.CustomLink = customLink;
                lock (this.Orders)
                {
                    this.Orders.Add(order);
                }
                if (!this.connection.Options.RunAtServer && ((this.connection.ConnectionStatusId == ConnectionStatusId.Connected) || (this.connection.ConnectionStatusId == ConnectionStatusId.ConnectionLost)))
                {
                    Globals.DB.Update(order, false);
                }
                if ((history != null) && (history.Count > 0))
                {
                    OrderStatusEventArgs args;
                    order.History.Clear();
                    for (int i = 0; i <= (history.Count - 2); i++)
                    {
                        args = history[i];
                        if (i == 0)
                        {
                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", args.OrderId, args.LimitPrice, args.StopPrice, args.Quantity, args.AvgFillPrice, args.Filled, initialOrderState, args.Time));
                            if (args.OrderState.Id == OrderStateId.Initialized)
                            {
                                continue;
                            }
                        }
                        order.Update(new OrderStatusEventArgs(order, args.Error, args.NativeError, args.OrderId, args.LimitPrice, args.StopPrice, args.Quantity, args.AvgFillPrice, args.Filled, args.OrderState, args.Time));
                    }
                    args = history[history.Count - 1];
                    this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, args.Error, args.NativeError, args.OrderId, args.LimitPrice, args.StopPrice, args.Quantity, args.AvgFillPrice, args.Filled, args.OrderState, args.Time));
                }
                else
                {
                    this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, initialOrderState, this.Connection.Now));
                }
                return order;
            }
            catch (TMException exception)
            {
                this.Connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.Connection, exception.Error, "", exception.Message));
                return null;
            }
        }

        /// <summary>
        /// Get account item value by it's type and currency.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public AccountItem GetItem(AccountItemType itemType, iTrading.Core.Kernel.Currency currency)
        {
            Hashtable hashtable = (Hashtable) this.items[currency.Id];
            if (hashtable == null)
            {
                hashtable = new Hashtable();
                this.items.Add(currency.Id, hashtable);
            }
            AccountItem item = (AccountItem) hashtable[itemType.Id];
            if (item == null)
            {
                item = new AccountItem(0.0, currency);
                hashtable.Add(itemType.Id, item);
            }
            return item;
        }

        internal void SimulationAccountUpdate()
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(new StringReader(this.customText));
            string name = "_" + ModeTypeId.Simulation.ToString();// (CultureInfo.InvariantCulture);
            if (this.customText.Length > 0)
            {
                try
                {
                    document.Load(reader);
                    document["TradeMagic"][name].RemoveAll();
                }
                catch
                {
                }
            }
            if (document["TradeMagic"] == null)
            {
                document.AppendChild(document.CreateElement("TradeMagic"));
            }
            if (document["TradeMagic"][name] == null)
            {
                document["TradeMagic"].AppendChild(document.CreateElement(name));
            }
            document["TradeMagic"][name].AppendChild(document.CreateElement("AccountItems"));
            XmlElement newChild = document.CreateElement("AccountItem");
            newChild.AppendChild(document.CreateElement("AccountItemType"));
            newChild.AppendChild(document.CreateElement("Currency"));
            newChild.AppendChild(document.CreateElement("Value"));
            newChild["AccountItemType"].InnerText = 0.ToString(CultureInfo.InvariantCulture);
            newChild["Currency"].InnerText = 7.ToString(CultureInfo.InvariantCulture);
            newChild["Value"].InnerText = this.GetItem(AccountItemType.All[AccountItemTypeId.BuyingPower], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown]).Value.ToString(CultureInfo.InvariantCulture);
            document["TradeMagic"][name]["AccountItems"].AppendChild(newChild);
            newChild = document.CreateElement("AccountItem");
            newChild.AppendChild(document.CreateElement("AccountItemType"));
            newChild.AppendChild(document.CreateElement("Currency"));
            newChild.AppendChild(document.CreateElement("Value"));
            newChild["AccountItemType"].InnerText = 1.ToString(CultureInfo.InvariantCulture);
            newChild["Currency"].InnerText = 7.ToString(CultureInfo.InvariantCulture);
            newChild["Value"].InnerText = this.GetItem(AccountItemType.All[AccountItemTypeId.CashValue], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown]).Value.ToString(CultureInfo.InvariantCulture);
            document["TradeMagic"][name]["AccountItems"].AppendChild(newChild);
            newChild = document.CreateElement("AccountItem");
            newChild.AppendChild(document.CreateElement("AccountItemType"));
            newChild.AppendChild(document.CreateElement("Currency"));
            newChild.AppendChild(document.CreateElement("Value"));
            newChild["AccountItemType"].InnerText = 2.ToString(CultureInfo.InvariantCulture);
            newChild["Currency"].InnerText = 7.ToString(CultureInfo.InvariantCulture);
            newChild["Value"].InnerText = this.GetItem(AccountItemType.All[AccountItemTypeId.ExcessEquity], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown]).Value.ToString(CultureInfo.InvariantCulture);
            document["TradeMagic"][name]["AccountItems"].AppendChild(newChild);
            newChild = document.CreateElement("AccountItem");
            newChild.AppendChild(document.CreateElement("AccountItemType"));
            newChild.AppendChild(document.CreateElement("Currency"));
            newChild.AppendChild(document.CreateElement("Value"));
            newChild["AccountItemType"].InnerText = 3.ToString(CultureInfo.InvariantCulture);
            newChild["Currency"].InnerText = 7.ToString(CultureInfo.InvariantCulture);
            newChild["Value"].InnerText = this.GetItem(AccountItemType.All[AccountItemTypeId.InitialMargin], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown]).Value.ToString(CultureInfo.InvariantCulture);
            document["TradeMagic"][name]["AccountItems"].AppendChild(newChild);
            document["TradeMagic"][name].AppendChild(document.CreateElement("Positions"));
            foreach (Position position in this.Positions)
            {
                newChild = document.CreateElement("Position");
                newChild.AppendChild(document.CreateElement("AvgPrice"));
                newChild.AppendChild(document.CreateElement("Currency"));
                newChild.AppendChild(document.CreateElement("MarketPosition"));
                newChild.AppendChild(document.CreateElement("Quantity"));
                newChild.AppendChild(document.CreateElement("Symbol"));
                newChild["AvgPrice"].InnerText = position.AvgPrice.ToString(CultureInfo.InvariantCulture);
                newChild["Currency"].InnerText = ((int) position.Currency.Id).ToString(CultureInfo.InvariantCulture);
                newChild["MarketPosition"].InnerText = ((int) position.MarketPosition.Id).ToString(CultureInfo.InvariantCulture);
                newChild["Quantity"].InnerText = position.Quantity.ToString(CultureInfo.InvariantCulture);
                newChild["Symbol"].InnerText = position.Symbol.ToString();
                document["TradeMagic"][name]["Positions"].AppendChild(newChild);
            }
            document["TradeMagic"][name].AppendChild(document.CreateElement("OverNight"));
            foreach (iTrading.Core.Kernel.Execution execution in this.simulationOverNight)
            {
                newChild = document.CreateElement("Execution");
                newChild.AppendChild(document.CreateElement("AvgPrice"));
                newChild.AppendChild(document.CreateElement("MarketPosition"));
                newChild.AppendChild(document.CreateElement("Quantity"));
                newChild.AppendChild(document.CreateElement("Symbol"));
                newChild["AvgPrice"].InnerText = execution.AvgPrice.ToString(CultureInfo.InvariantCulture);
                newChild["MarketPosition"].InnerText = ((int) execution.MarketPosition.Id).ToString(CultureInfo.InvariantCulture);
                newChild["Quantity"].InnerText = execution.Quantity.ToString(CultureInfo.InvariantCulture);
                newChild["Symbol"].InnerText = execution.Symbol.ToString();
                document["TradeMagic"][name]["OverNight"].AppendChild(newChild);
            }
            StringWriter writer = new StringWriter();
            document.Save(writer);
            this.customText = writer.ToString();
            Globals.DB.Update((iTrading.Core.Kernel.Account) this, false);
        }

        /// <summary>
        /// Reset a simulation account. All recorded order and execution data will be removed from the repository. All collections are cleared and the account
        /// items are re-initialized.
        /// Note: Works on simulation accounts only and has no effect on any other account.
        /// </summary>
        public void SimulationReset()
        {
            if (this.IsSimulation)
            {
                this.Orders.Clear();
                this.Executions.Clear();
                this.Positions.Clear();
                Globals.DB.Delete((iTrading.Core.Kernel.Account) this);
                this.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.connection, ErrorCode.NoError, "", this, AccountItemType.All[AccountItemTypeId.BuyingPower], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 2.0 * this.simulationAccountOptions.InitialCashValue, this.connection.Now));
                this.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.connection, ErrorCode.NoError, "", this, AccountItemType.All[AccountItemTypeId.CashValue], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], this.simulationAccountOptions.InitialCashValue, this.connection.Now));
                this.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.connection, ErrorCode.NoError, "", this, AccountItemType.All[AccountItemTypeId.ExcessEquity], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 2.0 * this.simulationAccountOptions.InitialCashValue, this.connection.Now));
                this.connection.ProcessEventArgs(new AccountUpdateEventArgs(this.connection, ErrorCode.NoError, "", this, AccountItemType.All[AccountItemTypeId.InitialMargin], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 0.0, this.connection.Now));
                this.SimulationAccountUpdate();
                Globals.DB.Update((iTrading.Core.Kernel.Account) this, true);
            }
        }

        /// <summary>
        /// The connection where the actual account belongs to.
        /// </summary>
        public iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Get dictionary of all currencies used at this moment by the account.
        /// </summary>
        public CurrencyDictionary Currencies
        {
            get
            {
                CurrencyDictionary dictionary = new CurrencyDictionary();
                foreach (CurrencyId id in this.items.Keys)
                {
                    dictionary.Add(iTrading.Core.Kernel.Currency.All[id]);
                }
                return dictionary;
            }
        }

        /// <summary>
        /// Get/set custom text. This property is used TM internally too and must be well formatted XML text.
        /// For adding custom information, scan the existing XML tree or build a new one with root node 
        /// "TradeMagic" and sub nodes not (!) starting with underscore "_".
        /// </summary>
        public string CustomText
        {
            get
            {
                return this.customText;
            }
            set
            {
                this.customText = value;
            }
        }

        /// <summary>
        /// Retrieves container which holds all executions since establishign the connection.
        /// </summary>
        public ExecutionCollection Executions
        {
            get
            {
                return this.executions;
            }
        }

        /// <summary>
        /// Does this account simulate order execution ?
        /// </summary>
        public bool IsSimulation
        {
            get
            {
                return this.isSimulation;
            }
        }

        /// <summary>
        /// Time of last update.
        /// </summary>
        public DateTime LastUpdate
        {
            get
            {
                return this.lastUpdate;
            }
        }

        /// <summary>
        /// Account name or id. Account numbers will be converted to string accordingly.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.accountName != null)
                {
                    return this.accountName;
                }
                return "";
            }
        }

        /// <summary>
        /// Retrieves container holding all orders.
        /// </summary>
        public OrderCollection Orders
        {
            get
            {
                return this.orders;
            }
        }

        /// <summary>
        /// Retrieves container which holds all open positions.
        /// </summary>
        public PositionCollection Positions
        {
            get
            {
                return this.positions;
            }
        }

        /// <summary>
        /// Get the simulation options.
        /// Please note: NULL, if account is not a simulation account.
        /// </summary>
        public iTrading.Core.Kernel.SimulationAccountOptions SimulationAccountOptions
        {
            get
            {
                return this.simulationAccountOptions;
            }
            set
            {
                if (this.IsSimulation && (value == null))
                {
                    throw new ArgumentNullException("SimulationAccountOptions", "NULL value can't be set for simulation accounts");
                }
                this.simulationAccountOptions = value;
            }
        }
    }
}