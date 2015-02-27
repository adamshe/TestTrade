namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Error code.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Order cancellation has been rejected.
        /// </summary>
        CancelRejected = 0,
        /// <summary>
        /// Server has closed connection.
        /// </summary>
        ConnectionTerminated = 1,
        /// <summary>
        /// Broker / data provider sends a critical message
        /// </summary>
        CriticalProviderMessage = 0x1b,
        /// <summary>
        /// Error on processing the CustomText property
        /// </summary>
        CustomText = 2,
        /// <summary>
        /// There are two or more indicators having the same name or description.
        /// </summary>
        DuplicateIndicator = 0x22,
        /// <summary>
        /// Feature not supported by broker or data vendor.
        /// </summary>
        FeatureNotSupported = 3,
        /// <summary>
        /// GUI component was not initialized properly.
        /// </summary>
        GuiNotInitialized = 4,
        /// <summary>
        /// The IndicatorBase.Calculate method threw an exception.
        /// </summary>
        IndicatorCalculate = 0x20,
        /// <summary>
        /// The IndicatorBase.Init method threw an exception.
        /// </summary>
        IndicatorInit = 0x21,
        /// <summary>
        /// Invalid license or invalid combination of broker and license.
        /// </summary>
        InvalidLicense = 6,
        /// <summary>
        /// Name exceeds max length.
        /// </summary>
        InvalidName = 7,
        /// <summary>
        /// Native broker account name is not supported.
        /// </summary>
        InvalidNativeAccount = 8,
        /// <summary>
        /// Native broker symbol is not supported.
        /// </summary>
        InvalidNativeSymbol = 9,
        /// <summary>
        /// Invalid order price for this instrument.
        /// </summary>
        InvalidOrderPrice = 5,
        /// <summary>
        /// Invalid symbol name: could not be converted to a <see cref="T:iTrading.Core.Kernel.Symbol" /> object.
        /// </summary>
        InvalidSymbolName = 30,
        /// <summary>
        /// Failed to login. Invalid user or password.
        /// </summary>
        LoginFailed = 10,
        /// <summary>
        /// Broker / data provider does not support multiple connections.
        /// </summary>
        MultipleConnectionsNotSupported = 11,
        /// <summary>
        /// Native broker error. See native broker or data provider API documentation.
        /// </summary>
        NativeError = 12,
        /// <summary>
        /// No error.
        /// </summary>
        NoError = 0x3e8,
        /// <summary>
        /// Provider does not support this symbol. Unknown symbol.
        /// </summary>
        NoSuchSymbol = 0x10,
        /// <summary>
        /// Can't execute this operation. Connection is not established.
        /// </summary>
        NotConnected = 13,
        /// <summary>
        /// TradeMagic user id is not registered.
        /// </summary>
        NotRegistered = 14,
        /// <summary>
        /// Feature not supported by this broker / data provider.
        /// </summary>
        NotSupported = 15,
        /// <summary>
        /// Broker has rejected current order.
        /// </summary>
        OrderRejected = 0x11,
        /// <summary>
        /// Internal error. Please contact TradeMagic support "support@trademagic.net".
        /// </summary>
        Panic = 0x12,
        /// <summary>
        /// Broker or data provider server terminated the communication.
        /// </summary>
        ServerConnectionIsBroken = 0x13,
        /// <summary>
        /// Timeout
        /// </summary>
        TimeOut = 0x1c,
        /// <summary>
        /// Max. # of market data streams for this connection is exceeded.
        /// </summary>
        TooManyMarketDataStreams = 0x19,
        /// <summary>
        /// Max. # of market depth streams for this connection is exceeded.
        /// </summary>
        TooManyMarketDepthStreams = 0x1a,
        /// <summary>
        /// Order can't be cancelled now. Please try again later.
        /// </summary>
        UnableToCancelOrder = 20,
        /// <summary>
        /// Order can't be updated. Current exchange/broker does not support order updates for this symbol or
        /// order is not yet submitted.
        /// </summary>
        UnableToChangeOrder = 0x15,
        /// <summary>
        /// Unable to check license.
        /// </summary>
        UnableToCheckLicense = 0x16,
        /// <summary>
        /// Symbol can't be deleted from repository. There are orders/executions related to that symbol.
        /// </summary>
        UnableToDeleteSymbol = 0x1f,
        /// <summary>
        /// Unable to perform selected action.
        /// </summary>
        UnableToPerformAction = 0x17,
        /// <summary>
        /// Order can't be submitted now. Please try again later.
        /// </summary>
        UnableToSubmitOrder = 0x18,
        /// <summary>
        /// There was an unexpected stop on market data or market depth data at the Patsystems adapter detected.
        /// </summary>
        UnexpectedDataStop = 0x23,
        /// <summary>
        /// Processing was aborted by the user.
        /// </summary>
        UserAbort = 0x1d
    }
}

