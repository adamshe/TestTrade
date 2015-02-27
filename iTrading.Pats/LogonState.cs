namespace TradeMagic.Pats
{
    using System;

    internal enum LogonState
    {
        DatabaseErr = 5,
        ForcedOut = 2,
        InvalidAppl = 8,
        InvalidLogonState = 0x63,
        InvalidPwd = 7,
        InvalidUser = 6,
        LoggedOn = 9,
        LogonFailed = 0,
        LogonSucceeded = 1,
        ObsoleteVers = 3,
        WrongEnv = 4
    }
}

