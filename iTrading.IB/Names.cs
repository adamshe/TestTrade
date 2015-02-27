namespace iTrading.IB
{
    using System;
    using System.Collections;

    internal class Names
    {
        private static SortedList Rights_ = new SortedList();

        internal static SortedList Rights
        {
            get
            {
                if (Rights_.Count == 0)
                {
                    Rights_.Add(Right.ANY, "");
                    Rights_.Add(Right.C, "C");
                    Rights_.Add(Right.CALL, "CALL");
                    Rights_.Add(Right.P, "P");
                    Rights_.Add(Right.PUT, "PUT");
                }
                return Rights_;
            }
        }
    }
}

