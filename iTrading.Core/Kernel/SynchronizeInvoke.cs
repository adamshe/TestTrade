namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// Thread synchronization.
    /// </summary>
    [Guid("E55FDCEB-BE16-412a-917F-E8771CEF48FB")]
    public class SynchronizeInvoke
    {
        private Control synchronizingObject = new Control();
        private Thread targetThread = Thread.CurrentThread;

        /// <summary>
        /// </summary>
        public SynchronizeInvoke()
        {
            this.synchronizingObject.CreateControl();
        }

        /// <summary>
        /// Switches to target thread and excutes the delegate asynchronous.
        /// If we are already in target thread contect the delegate is called synchronous.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public void AsyncInvoke(Delegate method, object[] args)
        {
            if (Thread.CurrentThread != this.targetThread)
            {
                this.synchronizingObject.BeginInvoke(method, args);
            }
            else
            {
                method.Method.Invoke(method.Target, args);
            }
        }

        /// <summary>
        /// Switches to target thread and excutes the delegate synchronous.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Invoke(Delegate method, object[] args)
        {
            if (Thread.CurrentThread != this.targetThread)
            {
                return this.synchronizingObject.Invoke(method, args);
            }
            return method.Method.Invoke(method.Target, args);
        }

        private delegate void Invoker(Delegate method, object[] args);
    }
}

