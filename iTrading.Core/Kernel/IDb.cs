using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Data;
    using System.Runtime.InteropServices;
    using iTrading.Core.Data;

    /// <summary>
    /// Data storage interface.
    /// </summary>
    [Guid("042C6CF4-FFB4-4c1b-838C-46F610BE9E48")]
    public interface IDb
    {
        /// <summary>
        /// Begin transaction.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Commit transaction.
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// Connect to data storage.
        /// </summary>
        /// <param name="historyMaintained">Number of days to maintain order history.
        /// Set 0, if no cleanup should be performed.</param>
        void Connect(int historyMaintained);
        /// <summary>
        /// Handle to the underlying DBMS.
        /// </summary>
        IDbConnection Connection { get; }
        /// <summary>
        /// Insert an existing account from the repository. Any associated order and execution information is deleted too. 
        /// This method is executed synchronously.
        /// </summary>
        /// <param name="account">The affected account</param>
        /// <returns>TRUE: account was deleted successfully. 
        /// FALSE: account could not be deleted.</returns>
        bool Delete(Account account);
        /// <summary>
        /// Insert an existing symbol from the repository. 
        /// This method is executed synchronously.
        /// </summary>
        /// <param name="symbol">The affected symbol object</param>
        /// <returns>TRUE: symbol was deleted successfully. 
        /// FALSE: symbol could not be deleted since there exist orders and/or executions referencing this symbol.</returns>
        bool Delete(Symbol symbol);
        /// <summary>
        /// Disconnect from data storage.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Add an order history item and update the affected db order object.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="synchronous">TRUE: the method returns after update/insert has been completed,
        /// FALSE: the method returns immediately without waiting for completion</param>
        void Insert(OrderStatusEventArgs e, bool synchronous);
        /// <summary>
        /// Synchronize open orders/executions/order history with the recorded status.
        /// </summary>
        void Recover(Account account);
        /// <summary>
        /// Get executions from the database.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        ExecutionCollection Select(Account account, DateTime minDate, DateTime maxDate);
        /// <summary>
        /// Query for a collection of symbols. The query parameters may be combined and-wise.
        /// Please note: The returned symbols may not be used to e.g. place orders or subsribe
        /// to market data, they are inactive. 
        /// Call <see cref="M:iTrading.Core.Kernel.Connection.GetSymbol(System.String,System.DateTime,iTrading.Core.Kernel.SymbolType,iTrading.Core.Kernel.Exchange,System.Double,iTrading.Core.Kernel.RightId,iTrading.Core.Kernel.LookupPolicyId)" /> by using these templates to get active <see cref="T:iTrading.Core.Kernel.Symbol" /> objects.
        /// </summary>
        /// <param name="name">Query for a symbol name. SQL wildcards '%' and '_' may be applied. Set NULL if not needed.</param>
        /// <param name="brokerType">If NULL, the broker independant name is queried.
        /// It not NULL, the broker dependant name for that broker is queried.</param>
        /// <param name="companyName">Query for a company name. SQL wildcards '%' and '_' may be applied. Set NULL if not needed.</param>
        /// <param name="expiry">Query for an expiry. Set <see cref="F:iTrading.Core.Kernel.Globals.MaxDate" /> if not needed.</param>
        /// <param name="symbolType">Query for symbol type. Set NULL if not needed.</param>
        /// <param name="exchange">Query for an exchange. Set NULL if not needed.</param>
        /// <param name="strikePrice">Query for an option strike price. Set 0 if not needed.</param>
        /// <param name="rightId">Query for an option right. Set <see cref="F:iTrading.Core.Kernel.RightId.Unknown" /> if not needed.</param>
        /// <param name="customText">Query for custom text. SQL wildcard '%' and '_' may be applied.  Set NULL if not needed.</param>
        /// <returns>A collection of symbols.</returns>
        SymbolCollection Select(string name, ProviderType brokerType, string companyName, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, string customText);
        /// <summary>
        /// Get quotes from the database.
        /// </summary>
        /// <param name="quotes"></param>
        /// <returns></returns>
        int Select(Quotes quotes);
        /// <summary>
        /// Synchronizing object in multi-thread environment.
        /// </summary>
        object SyncRoot { get; }
        /// <summary>
        /// Insert a new account or update an existing account.
        /// </summary>
        /// <param name="account">The affected account</param>
        /// <param name="synchronous">TRUE: the method returns after update/insert has been completed,
        /// FALSE: the method returns immediately without waiting for completion</param>
        void Update(Account account, bool synchronous);
        /// <summary>
        /// Insert a new execution or update an existing one
        /// </summary>
        /// <param name="execution">The affected execution object</param>
        /// <param name="synchronous">TRUE: the method returns after update/insert has been completed,
        /// FALSE: the method returns immediately without waiting for completion</param>
        void Update(Execution execution, bool synchronous);
        /// <summary>
        /// Insert a new order or update an existing one
        /// </summary>
        /// <param name="order">The affected order object</param>
        /// <param name="synchronous">TRUE: the method returns after update/insert has been completed,
        /// FALSE: the method returns immediately without waiting for completion</param>
        void Update(Order order, bool synchronous);
        /// <summary>
        /// Insert new quotes data or override existing data.
        /// </summary>
        /// <param name="quotes">The affected quotes object</param>
        /// <param name="synchronous">TRUE: the method returns after update/insert has been completed,
        /// FALSE: the method returns immediately without waiting for completion</param>
        void Update(Quotes quotes, bool synchronous);
        /// <summary>
        /// Insert a new symbol or update an existing one
        /// </summary>
        /// <param name="symbol">The affected symbol object</param>
        /// <param name="synchronous">TRUE: the method returns after update/insert has been completed,
        /// FALSE: the method returns immediately without waiting for completion</param>
        void Update(Symbol symbol, bool synchronous);
    }
}

