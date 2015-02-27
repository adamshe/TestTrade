namespace iTrading.Mbt
{
    using System;

    public abstract class HandlePopup
    {
        protected const string loginDialogClassName = "#32770";
        private bool popupHandled = false;
        protected const int stringCapacity = 0x100;

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

