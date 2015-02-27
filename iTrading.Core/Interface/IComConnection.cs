using iTrading.Core.Kernel;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.Connection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("7D343228-B066-4808-8A16-E5C75BB0ED6F")]
    public interface IComConnection
    {
        /// <summary>
        /// Retrieves a container holding all accounts for this connection. Usually there will not be more than one
        /// account. But some brokers support multiple accounts managed by a master connection.
        /// Please note: This collection is empty, when <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        AccountCollection Accounts { get; }
        /// <summary>
        /// Get a collection of all available order action types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        ActionTypeDictionary ActionTypes { get; }
        /// <summary>
        /// Get a collection of all available account item types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        AccountItemTypeDictionary AccountItemTypes { get; }
        /// <summary>
        /// Indicates the current connection status.
        /// </summary>
        iTrading.Core.Kernel.ConnectionStatusId ConnectionStatusId { get; }
        /// <summary>
        /// Create account for order execution simulation.
        /// </summary>
        /// <param name="accountName">Account name, must be unique within the <see cref="P:iTrading.Core.Kernel.Connection.Accounts" /> collection</param>
        /// <param name="simulationAccountOptions">Default simulation options for this account.
        /// may be overriden by <see cref="M:iTrading.Core.Kernel.Order.Submit" />.</param>
        /// <returns></returns>
        Account CreateSimulationAccount(string accountName, SimulationAccountOptions simulationAccountOptions);
        /// <summary>
        /// Get a collection of all available currencies. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        CurrencyDictionary Currencies { get; }
        /// <summary>
        /// Custom text.
        /// </summary>
        string CustomText { get; }
        /// <summary>
        /// Get a collection of all available exchanges. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        ExchangeDictionary Exchanges { get; }
        /// <summary>
        /// Get a collection of all available feature types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        FeatureTypeDictionary FeatureTypes { get; }
        /// <summary>
        /// The actual license.
        /// </summary>
        LicenseType GetLicense(string moduleName);
        /// <summary>
        /// Retrieves a symbol object from the providers dictionary.
        /// Please note: There exists only one instance for each symbol.
        /// </summary>
        /// <param name="name">Symbol name.</param>
        /// <param name="expiry">Expiry date. Used for futures only.</param>
        /// <param name="symbolType">Type of symbol.</param>
        /// <param name="exchange">Identifies the exchange.</param>
        /// <param name="strikePrice">Strike price. For options only. Set to 0 for any other symbol type.</param>
        /// <param name="rightId">Options right. For options only. Set to any <see cref="T:iTrading.Core.Kernel.RightId" /> value for any other symbol type.</param>
        /// <param name="lookupPolicyId">Identifies the exchange.</param>
        /// <returns>Symbol in the directory. NULL, if provider does not support this symbol or if connection is not
        /// established.</returns>
        Symbol GetSymbol(string name, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, LookupPolicyId lookupPolicyId);
        /// <summary>
        /// Get a symbol macthing a name created by <see cref="M:iTrading.Core.Kernel.Symbol.ToString" />.
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        Symbol GetSymbolByName(string symbolName);
        /// <summary>
        /// Get <see cref="P:iTrading.Core.Kernel.Request.Id" /> plus the current thread's name as string.
        /// </summary>
        string IdPlus { get; }
        /// <summary>
        /// Get a collection of all symbols with running <see cref="T:iTrading.Core.Kernel.MarketData" /> streams. 
        /// Please note: This collection is empty, when <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        MarketDataCollection MarketDataStreams { get; }
        /// <summary>
        /// Get a collection of all available order market data types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        MarketDataTypeDictionary MarketDataTypes { get; }
        /// <summary>
        /// Get a collection of all symbols with running <see cref="T:iTrading.Core.Kernel.MarketDepth" /> streams. 
        /// Please note: This collection is empty, when <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        MarketDepthCollection MarketDepthStreams { get; }
        /// <summary>
        /// Get a collection of all available market position types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        MarketPositionDictionary MarketPositions { get; }
        /// <summary>
        /// Get a collection of all available market position types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        NewsEventArgsCollection News { get; }
        /// <summary>
        /// Get a collection of all available news item types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        NewsItemTypeDictionary NewsItemTypes { get; }
        /// <summary>
        /// Get/set current time, synchronized with the provider's "heartbeat" (if provided).
        /// Plase note: Setting the time only is effective in simulation mode.
        /// (<see cref="P:iTrading.Core.Kernel.Connection.Options" />.Mode.Id == <see cref="F:iTrading.Core.Kernel.ModeTypeId.Simulation" />).
        /// </summary>
        DateTime Now { get; set; }
        /// <summary>
        /// Options of current connection.
        /// </summary>
        OptionsBase Options { get; }
        /// <summary>
        /// Get a collection of all available order status instances. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        OrderStateDictionary OrderStates { get; }
        /// <summary>
        /// Get a collection of all available order types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        OrderTypeDictionary OrderTypes { get; }
        /// <summary>
        /// Indicates the current connection status id of the secondary server(s), e.g. price feed server.
        /// </summary>
        iTrading.Core.Kernel.ConnectionStatusId SecondaryConnectionStatusId { get; }
        /// <summary>
        /// Get/set name of simulation account. Set this property before calling <see cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />.
        /// Default: "TM Simulation".
        /// </summary>
        string SimulationAccountName { get; set; }
        /// <summary>
        /// Get/set speed of replayed data for simulation. 
        /// Effective when <see cref="P:iTrading.Core.Kernel.Connection.Options" />.Mode.Id == <see cref="F:iTrading.Core.Kernel.ModeTypeId.Simulation" />.
        /// Only values greater/equal zero are accepted
        /// 1 = realtime speed, 0 = stop.
        /// </summary>
        double SimulationSpeed { get; set; }
        /// <summary>
        /// Get a collection of all symbols. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        SymbolDictionary Symbols { get; }
        /// <summary>
        /// Get a collection of all available symbol types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        SymbolTypeDictionary SymbolTypes { get; }
        /// <summary>
        /// Get a collection of all available time in force values. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        TimeInForceDictionary TimeInForces { get; }
    }
}

