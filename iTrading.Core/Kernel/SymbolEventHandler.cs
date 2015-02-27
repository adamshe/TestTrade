namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This delegate will be called when a symbol was lookuped at the broker's system.
    /// </summary>
    [ComVisible(false)]
    public delegate void SymbolEventHandler(object sender, SymbolEventArgs e);
}

