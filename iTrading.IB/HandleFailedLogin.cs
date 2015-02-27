namespace iTrading.IB
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal class HandleFailedLogin : HandlePopup
    {
        protected const string failedLoginCaption = "Login";

        protected override bool HandleWindow(int handle)
        {
            StringBuilder buf = new StringBuilder(0x100);
            WinApi.GetClassName(handle, buf, 0x100);
            if ((buf.ToString() == "SunAwtDialog") && WinApi.IsWindowVisible(handle))
            {
                StringBuilder builder2 = new StringBuilder(0x100);
                WinApi.GetWindowText(handle, builder2, 0x100);
                WinApi.RECT rectangle = new WinApi.RECT(0);
                WinApi.GetWindowRect(handle, ref rectangle);
                if (((builder2.ToString() == "Login") && (rectangle.Height > 110)) && (rectangle.Width > 300))
                {
                    SendKeys.SendWait(" ");
                    Application.DoEvents();
                    Thread.Sleep(0x3e8);
                    return false;
                }
            }
            return true;
        }
    }
}

