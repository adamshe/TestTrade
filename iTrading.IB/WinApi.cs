namespace iTrading.IB
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
        internal static extern int GetClassName(int intHandle, [MarshalAs(UnmanagedType.LPStr)] StringBuilder Buf, int intBufLength);
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern int GetForegroundWindow();
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern int GetWindowRect(int intHandler, ref RECT Rectangle);
        [SuppressUnmanagedCodeSecurity, DllImport("user32", EntryPoint="GetWindowTextA")]
        internal static extern int GetWindowText(int intHandle, StringBuilder Buf, int intBufLength);
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern bool IsWindowVisible(int intHandle);
        [SuppressUnmanagedCodeSecurity, DllImport("user32")]
        internal static extern bool SetForegroundWindow(int intHandle);

        internal delegate bool Callback(int intHandle, int intParam);

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public int Height
            {
                get
                {
                    return (this.Bottom - this.Top);
                }
            }
            public int Width
            {
                get
                {
                    return (this.Right - this.Left);
                }
            }
            public RECT(int intInit)
            {
                this.Left = intInit;
                this.Top = intInit;
                this.Right = intInit;
                this.Bottom = intInit;
            }
        }
    }
}

