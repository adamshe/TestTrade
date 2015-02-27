namespace iTrading.IB
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    internal class HandleStartupPopups : HandlePopup
    {
        private bool close;
        protected const string logonCaption = "New Login";
        private string password;
        private string user;

        internal HandleStartupPopups()
        {
            this.close = false;
            this.password = "";
            this.user = "";
            this.close = true;
        }

        internal HandleStartupPopups(string user, string password)
        {
            this.close = false;
            this.password = "";
            this.user = "";
            this.password = password;
            this.user = user;
        }

        protected override bool HandleWindow(int handle)
        {
            StringBuilder buf = new StringBuilder(0x100);
            WinApi.GetClassName(handle, buf, 0x100);
            if ((buf.ToString() == "SunAwtDialog") && WinApi.IsWindowVisible(handle))
            {
                StringBuilder builder2 = new StringBuilder(0x100);
                WinApi.GetWindowText(handle, builder2, 0x100);
                if (builder2.ToString().Length == 0)
                {
                    WinApi.SetForegroundWindow(handle);
                    Thread.Sleep(100);
                    Application.DoEvents();
                    if (this.close)
                    {
                        SendKeys.SendWait("%{F4}");
                    }
                    else
                    {
                        SendKeys.SendWait(" ");
                    }
                    Application.DoEvents();
                    Thread.Sleep(0x3e8);
                }
            }
            if ((buf.ToString() == "jclient.go") && WinApi.IsWindowVisible(handle))
            {
                StringBuilder builder3 = new StringBuilder(0x100);
                WinApi.GetWindowText(handle, builder3, 0x100);
                if (builder3.ToString() == "New Login")
                {
                    WinApi.SetForegroundWindow(handle);
                    Thread.Sleep(100);
                    Application.DoEvents();
                    if (this.close)
                    {
                        SendKeys.SendWait("%{F4}");
                    }
                    else
                    {
                        SendKeys.SendWait(this.user);
                        SendKeys.SendWait("{TAB}");
                        Thread.Sleep(100);
                        SendKeys.SendWait(this.password);
                        SendKeys.SendWait("{TAB}");
                        Thread.Sleep(100);
                        SendKeys.SendWait("{TAB}");
                        Thread.Sleep(100);
                        SendKeys.SendWait("{TAB}");
                        Thread.Sleep(100);
                        SendKeys.SendWait(" ");
                    }
                    Application.DoEvents();
                    Thread.Sleep(0x3e8);
                    return false;
                }
            }
            return true;
        }
    }
}

