using iTrading.Core.Data;

namespace iTrading.Core.Data
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Delegate for updates of <see cref="T:iTrading.Core.Data.Bar" /> objects.
    /// </summary>
    [ComVisible(false)]
    public delegate void BarUpdateEventHandler(object sender, BarUpdateEventArgs e);
}

