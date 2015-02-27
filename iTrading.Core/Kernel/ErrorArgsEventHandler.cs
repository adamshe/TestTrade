namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Call back for errors which are not associated to explicit call back delegates.
    /// </summary>
    [ComVisible(false)]
    public delegate void ErrorArgsEventHandler(object sender, ITradingErrorEventArgs e);
}

