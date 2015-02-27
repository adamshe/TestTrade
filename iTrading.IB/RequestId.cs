namespace iTrading.IB
{
    using System;

    internal enum RequestId
    {
        CANCEL_MKT_DATA = 2,
        CANCEL_MKT_DEPTH = 11,
        CANCEL_NEWS_BULLETINS = 13,
        CANCEL_ORDER = 4,
        PLACE_ORDER = 3,
        REQ_ACCT_DATA = 6,
        REQ_ALL_OPEN_ORDERS = 0x10,
        REQ_AUTO_OPEN_ORDERS = 15,
        REQ_CONTRACT_DATA = 9,
        REQ_EXECUTIONS = 7,
        REQ_FA = 0x12,
        REQ_IDS = 8,
        REQ_MANAGED_ACCTS = 0x11,
        REQ_MKT_DATA = 1,
        REQ_MKT_DEPTH = 10,
        REQ_NEWS_BULLETINS = 12,
        REQ_OPEN_ORDERS = 5,
        SET_SERVER_LOGLEVEL = 14
    }
}

