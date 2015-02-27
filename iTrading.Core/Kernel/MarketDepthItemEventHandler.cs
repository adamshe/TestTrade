namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Called when a <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> is received.
    /// </summary>
    [ComVisible(false)]
    public delegate void MarketDepthItemEventHandler(object sender, MarketDepthEventArgs e);
}

