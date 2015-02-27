namespace TradeMagic.Pats
{
    using System;

    internal enum ErrorCode
    {
        ErrAmendDisabled = 30,
        ErrBadMsgType = 14,
        ErrBadPassword = 0x11,
        ErrBlankPassword = 6,
        ErrBufferOverflow = 0x10,
        ErrCallbackNotSet = 2,
        ErrFalse = 11,
        ErrInvalidIndex = 8,
        ErrInvalidPassword = 5,
        ErrInvalidPrice = 0x1b,
        ErrInvalidState = 0x1a,
        ErrInvalidUnderlying = 0x26,
        ErrInvalidVolume = 0x1d,
        ErrLast = 0x27,
        ErrMDSUnavailable = 0x24,
        ErrNoData = 10,
        ErrNoReport = 20,
        ErrNotAlphaNumeric = 0x25,
        ErrNotConnected = 0x12,
        ErrNotEnabled = 7,
        ErrNotInitialised = 1,
        ErrNotLoggedOn = 4,
        ErrNotTradable = 0x22,
        ErrPriceNotRequired = 0x1c,
        ErrPriceRequired = 0x18,
        ErrQueryDisabled = 0x1f,
        ErrTASUnavailable = 0x23,
        ErrUnexpected = 0x63,
        ErrUnknownAccount = 9,
        ErrUnknownCallback = 3,
        ErrUnknownCommodity = 0x17,
        ErrUnknownContract = 0x16,
        ErrUnknownCurrency = 0x13,
        ErrUnknownError = 12,
        ErrUnknownExchange = 0x20,
        ErrUnknownFill = 0x21,
        ErrUnknownMsgID = 15,
        ErrUnknownOrder = 0x19,
        ErrUnknownOrderType = 0x15,
        ErrUntradableOType = 0x27,
        ErrWrongVersion = 13,
        Success = 0
    }
}

