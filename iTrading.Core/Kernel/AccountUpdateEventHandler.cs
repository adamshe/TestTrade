namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This delegate will be called on account updates.
    /// </summary>
    [ComVisible(false)]
    public delegate void AccountUpdateEventHandler(object sender, AccountUpdateEventArgs e);
}

