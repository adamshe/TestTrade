namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Account" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("E67174EE-AE8B-4192-8891-192CE7689718")]
    public interface IComAccount
    {
        /// <summary>
        /// The <see cref="T:iTrading.Core.Kernel.Connection" /> where the actual account belongs to.
        /// </summary>
        iTrading.Core.Kernel.Connection Connection { get; }
        /// <summary>
        /// </summary>
        string CustomText { get; set; }
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
        Order CreateOrder(Symbol symbol, ActionTypeId action, OrderTypeId orderType, TimeInForceId timeInForce, int quantity, double limitPrice, double stopPrice, string ocaGroup, object customLink);
        /// <summary>
        /// Get dictionary of all currencies used at this moment by the account.
        /// </summary>
        CurrencyDictionary Currencies { get; }
        /// <summary>
        /// Retrieves container which holds all executions since establishign the connection.
        /// </summary>
        ExecutionCollection Executions { get; }
        /// <summary>
        /// Get account item value by it's type and currency.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        AccountItem GetItem(AccountItemType itemType, iTrading.Core.Kernel.Currency currency);
        /// <summary>
        /// Does this account simulate order execution ?
        /// </summary>
        bool IsSimulation { get; }
        /// <summary>
        /// Retrieves container which holds all open positions.
        /// </summary>
        PositionCollection Positions { get; }
        /// <summary>
        /// Account name or id. Account numbers will be converted to string accordingly.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Time of last update.
        /// </summary>
        DateTime LastUpdate { get; }
        /// <summary>
        /// Retrieves container holding all open orders.
        /// </summary>
        OrderCollection Orders { get; }
        /// <summary>
        /// Get the simulation options.
        /// Please note: NULL, if account is not a simulation account.
        /// </summary>
        iTrading.Core.Kernel.SimulationAccountOptions SimulationAccountOptions { get; set; }
        /// <summary>
        /// Reset a simulation account. All recorded order and execution data will be removed from the repository. All collections are cleared and the account
        /// items are re-initialized.
        /// Note: Works on simulation accounts only and has no effect on any other account.
        /// </summary>
        void SimulationReset();
    }
}

