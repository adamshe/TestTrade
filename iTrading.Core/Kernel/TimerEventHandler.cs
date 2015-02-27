namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Call back for timer events. <seealso cref="E:iTrading.Core.Kernel.Connection.Timer" />.
    /// </summary>
    [ComVisible(false)]
    public delegate void TimerEventHandler(object sender, TimerEventArgs e);
}

