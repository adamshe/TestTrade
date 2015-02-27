namespace iTrading.Mbt
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    internal class WinApi
    {
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern int EnumWindows(Callback Callback, int intParam);
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern int FindWindow(string className, string windowName);
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern int GetClassName(int intHandle, [MarshalAs(UnmanagedType.LPStr)] StringBuilder Buf, int intBufLength);
        [SuppressUnmanagedCodeSecurity, DllImport("user32", EntryPoint="GetWindowTextA")]
        internal static extern int GetWindowText(int intHandle, StringBuilder Buf, int intBufLength);
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern int ShowWindow(int hWnd, int showCmd);

        internal delegate bool Callback(int intHandle, int intParam);
    }
}

