namespace iTrading.Mbt
{
    using System;
    using System.Text;
    using System.Windows.Forms;

    internal class HandleFailedLogin : HandlePopup
    {
        protected const string failedLoginCaption = "MBT Navigator";
        protected const string loginCaption = "MBT Navigator Login";

        protected override bool HandleWindow(int handle)
        {
            StringBuilder buf = new StringBuilder(0x100);
            StringBuilder builder2 = new StringBuilder(0x100);
            WinApi.GetClassName(handle, buf, 0x100);
            WinApi.GetWindowText(handle, builder2, 0x100);
            if ((buf.ToString() == "#32770") && (builder2.ToString() == "MBT Navigator"))
            {
                SendKeys.SendWait(" ");
            }
            else if ((buf.ToString() == "#32770") && (builder2.ToString() == "MBT Navigator Login"))
            {
                SendKeys.SendWait("{ESC}");
            }
            return true;
        }
    }
}

