namespace iTrading.CT
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

     internal  class Api
    {
        private static int nextRqn = 0;

        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode AdviseDataMidApi(string symbols, Feed feed);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_CloseConnection")]
        internal static extern void CloseConnection(string logon);
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode ConnectMidApi(bool useApiLogon, char type);
        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32")]
        internal static extern bool FreeLibrary(int handle);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_Initialize")]
        internal static extern void Initialize();
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode InitMidApi(string authCode, StatusCallback status, DataCallback data, ErrorCallback error);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_IsConnected")]
        internal static extern bool IsConnected(string logon);
        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32")]
        internal static extern int LoadLibrary(string dllName);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_OpenConnection")]
        internal static extern bool OpenConnection(string logon, string pwd, int demo, string ip, string ipBackup);
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode RequestDataMidApi(string symbols, Feed feed, int param1, int param2, int rqn);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnConnectCallback")]
        internal static extern void SetConnectCallback(OnConnect callback);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnConnectFailCallback")]
        internal static extern void SetConnectFailCallback(OnConnectFail callback);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnDisconnectCallback")]
        internal static extern void SetDisconnectCallback(OnDisconnect callback);
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode SetIntMidApi(SetString type, int val);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnServerConnectCallback")]
        internal static extern void SetServerConnectCallback(OnServerConnect callback);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnServerDisconnectCallback")]
        internal static extern void SetServerDisconnectCallback(OnServerDisconnect callback);
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode SetStringMidApi(SetString type, string val);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnUserLogoffCallback")]
        internal static extern void SetUserLogoffCallback(OnUserLogoff callback);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnUserLogonCallback")]
        internal static extern void SetUserLogonCallback(OnUserLogon callback);
        [SuppressUnmanagedCodeSecurity, DllImport("CXPDLL", EntryPoint="CYBERAPI3_UC_SetOnUserLogonFailCallback")]
        internal static extern void SetUserLogonFailCallback(OnUserLogonFail callback);
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode TerminateMidApi();
        [SuppressUnmanagedCodeSecurity, DllImport("MidAPI")]
        internal static extern RetCode UnadviseDataMidApi(string symbols, Feed feed);

        internal static int NextRqn
        {
            get
            {
                lock (typeof(Api))
                {
                    return nextRqn++;
                }
            }
        }

        internal unsafe delegate void DataCallback(Feed feed, DataType type, byte* data);

        internal delegate void ErrorCallback(Feed feed, Failure type, string symbol);

        internal delegate void OnConnect(int demo);

        internal delegate void OnConnectFail(ConnectFail reason);

        internal delegate void OnDisconnect(bool error);

        internal delegate void OnServerConnect(string user);

        internal delegate void OnServerDisconnect(string user);

        internal delegate void OnUserLogoff(string user);

        internal delegate void OnUserLogon(string user, bool demo);

        internal delegate void OnUserLogonFail(string user, ConnectFail reason);

        internal delegate void StatusCallback(Status status, string msg);
    }
}

