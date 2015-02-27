namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Called when a <see cref="T:iTrading.Core.Kernel.NewsEventArgs" /> is received.
    /// </summary>
    [ComVisible(false)]
    public delegate void NewsEventHandler(object sender, NewsEventArgs e);
}

