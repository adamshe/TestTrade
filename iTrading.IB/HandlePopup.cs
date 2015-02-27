namespace iTrading.IB
{
    using System;

    public abstract class HandlePopup
    {
        internal const string loginDialogClassName = "jclient.go";
        internal const string popupCaption = "IB Trader Workstation";
        private bool popupHandled = false;
        internal const int stringCapacity = 0x100;
        internal const string sunAwtDialogClassName = "SunAwtDialog";
        internal const string twsClassName = "jclient.gp";

        protected HandlePopup()
        {
        }

        internal bool ForEachWindow(int handle, int param)
        {
            bool flag = this.HandleWindow(handle);
            if (!flag)
            {
                this.popupHandled = true;
            }
            return flag;
        }

        protected abstract bool HandleWindow(int handle);
        public virtual bool Run()
        {
            this.StartUp();
            this.popupHandled = false;
            lock (typeof(HandlePopup))
            {
                WinApi.EnumWindows(new WinApi.Callback(this.ForEachWindow), 0);
            }
            return this.popupHandled;
        }

        public virtual void StartUp()
        {
        }
    }
}

