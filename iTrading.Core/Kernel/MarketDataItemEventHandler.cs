namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Called when a <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> is received.
    /// </summary>
    [ComVisible(false)]
    public delegate void MarketDataItemEventHandler(object sender, MarketDataEventArgs e);
}

