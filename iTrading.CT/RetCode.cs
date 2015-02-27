namespace iTrading.CT
{
    using System;

    internal enum RetCode
    {
        AlreadyInitialized = 1,
        DataAuthorization = 10,
        Exception = 5,
        FailureCreatingWindow = 2,
        NotInitialized = 3,
        Success = 0,
        TcpInitError = 4
    }
}

