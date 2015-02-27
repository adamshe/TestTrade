namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Delegate for position updates.
    /// </summary>
    [ComVisible(false)]
    public delegate void PositionUpdateEventHandler(object sender, PositionUpdateEventArgs e);
}

