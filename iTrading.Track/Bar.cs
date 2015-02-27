namespace iTrading.Track
{
    using System;

    internal class Bar
    {
        internal double close;
        internal double high;
        internal double low;
        internal double open;
        internal DateTime time;
        internal int volume;

        internal Bar(double open, double high, double low, double close, DateTime time, int volume)
        {
            this.close = close;
            this.high = high;
            this.low = low;
            this.open = open;
            this.time = time;
            this.volume = volume;
        }
    }
}

