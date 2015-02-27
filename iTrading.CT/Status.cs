namespace iTrading.CT
{
    using System;

    internal enum Status
    {
        ClassUnregistered = 0x6d,
        ConnectError = 0x68,
        FeedStatusChange = 0x69,
        InvalidAuthorization = 0x6c,
        InvalidRequest = 0x6b,
        LogonFailed = 0x6a,
        NoServerAvailable = 0x67,
        Ready = 0x65,
        UserCancelLogon = 100
    }
}

