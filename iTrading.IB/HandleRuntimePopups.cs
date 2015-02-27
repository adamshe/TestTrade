namespace iTrading.IB
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal class HandleRuntimePopups : HandlePopup
    {
        protected override bool HandleWindow(int handle)
        {
            StringBuilder buf = new StringBuilder(0x100);
            WinApi.GetClassName(handle, buf, 0x100);
            if ((buf.ToString() == "SunAwtDialog") && WinApi.IsWindowVisible(handle))
            {
                StringBuilder builder2 = new StringBuilder(0x100);
                WinApi.GetWindowText(handle, builder2, 0x100);
                if (builder2.ToString() == "IB Trader Workstation")
                {
                    WinApi.SetForegroundWindow(handle);
                    Thread.Sleep(100);
                    Application.DoEvents();
                    SendKeys.SendWait("{ENTER}");
                }
            }
            return true;
        }
    }
}

