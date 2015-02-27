namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Delegate for order updates.
    /// </summary>
    [ComVisible(false)]
    public delegate void OrderStatusEventHandler(object sender, OrderStatusEventArgs e);
}

