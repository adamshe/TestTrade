namespace Steema.TeeChart
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TeeChartException : Exception
    {
        public TeeChartException()
        {
        }

        public TeeChartException(string message) : base(message)
        {
        }

        protected TeeChartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TeeChartException(string message, Exception e) : base(message, e)
        {
        }
    }
}

