using System;
using iTrading.Core.Data;

namespace iTrading.Core.IndicatorBase
{
    /// <summary>
    /// Attribute to mark indicator parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ParameterAttribute : Attribute
    {
        private double maxValue;
        private double minValue;
        private string name;
        private PriceTypeId priceTypeId;
        private System.Type type;

        /// <summary>
        /// </summary>
        /// <param name="priceTypeId"></param>
        public ParameterAttribute(PriceTypeId priceTypeId)
        {
            this.maxValue = double.MaxValue;
            this.minValue = double.MinValue;
            this.name = "";
            this.priceTypeId =PriceTypeId.Open;
            this.type = null;
            this.priceTypeId = priceTypeId;
            this.type = typeof(PriceTypeId);
        }

        /// <summary>
        /// </summary>
        /// <param name="priceTypeId"></param>
        /// <param name="name"></param>
        public ParameterAttribute(PriceTypeId priceTypeId, string name)
        {
            this.maxValue = double.MaxValue;
            this.minValue = double.MinValue;
            this.name = "";
            this.priceTypeId = PriceTypeId.Open;
            this.type = null;
            this.name = name;
            this.priceTypeId = priceTypeId;
            this.type = typeof(PriceTypeId);
        }

        /// <summary>
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="name"></param>
        public ParameterAttribute(double minValue, double maxValue, string name)
        {
            this.maxValue = double.MaxValue;
            this.minValue = double.MinValue;
            this.name = "";
            this.priceTypeId = PriceTypeId.Open;
            this.type = null;
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.name = name;
            this.type = typeof(double);
        }

        /// <summary>
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="name"></param>
        public ParameterAttribute(int minValue, int maxValue, string name)
        {
            this.maxValue = double.MaxValue;
            this.minValue = double.MinValue;
            this.name = "";
            this.priceTypeId = PriceTypeId.Open;
            this.type = null;
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.name = name;
            this.type = typeof(int);
        }

        /// <summary>
        /// </summary>
        public double MaxValue
        {
            get
            {
                return Math.Min(999999.0, this.maxValue);
            }
        }

        /// <summary>
        /// </summary>
        public double MinValue
        {
            get
            {
                return Math.Max(-999999.0, this.minValue);
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// </summary>
        public PriceTypeId PriceTypeId
        {
            get
            {
                return this.priceTypeId;
            }
        }

        /// <summary>
        /// Type of the parameter value.
        /// </summary>
        public System.Type Type
        {
            get
            {
                return this.type;
            }
        }
    }
}