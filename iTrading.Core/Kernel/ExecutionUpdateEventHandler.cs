namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Delegate for execution updates.
    /// </summary>
    [ComVisible(false)]
    public delegate void ExecutionUpdateEventHandler(object sender, ExecutionUpdateEventArgs e);
}

