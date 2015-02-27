namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Called when teh current processing is aborted.
    /// </summary>
    public delegate void AbortEventHandler(object sender, EventArgs e);
}

