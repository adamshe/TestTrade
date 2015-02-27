namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;

    [Serializable]
    public class Average : Function
    {
        private bool includeNulls;

        [Description("Initializes a new Average function, using Null values.")]
        public Average() : this(true)
        {
        }

        [Description("If UseNulls is true, null values will be treated as zero in average calculation.")]
        public Average(bool UseNulls)
        {
            this.includeNulls = true;
            this.includeNulls = UseNulls;
        }

        public override double Calculate(Series sourceSeries, int firstIndex, int lastIndex)
        {
            if ((firstIndex == -1) && this.includeNulls)
            {
                if (sourceSeries.Count <= 0)
                {
                    return 0.0;
                }
                return (base.ValueList(sourceSeries).Total / ((double) sourceSeries.Count));
            }
            if (firstIndex == -1)
            {
                firstIndex = 0;
                lastIndex = sourceSeries.Count - 1;
            }
            double num = 0.0;
            int num2 = 0;
            Steema.TeeChart.Styles.ValueList list = base.ValueList(sourceSeries);
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                if (this.includeNulls || !sourceSeries.IsNull(i))
                {
                    num += list[i];
                    num2++;
                }
            }
            if (num2 != 0)
            {
                return (num / ((double) num2));
            }
            return 0.0;
        }

        public override double CalculateMany(ArrayList sourceSeriesList, int valueIndex)
        {
            if (sourceSeriesList.Count > 0)
            {
                double num = 0.0;
                int num2 = 0;
                for (int i = 0; i < sourceSeriesList.Count; i++)
                {
                    Series s = (Series) sourceSeriesList[i];
                    Steema.TeeChart.Styles.ValueList list = base.ValueList(s);
                    if ((list.Count > valueIndex) && (this.includeNulls || !s.IsNull(i)))
                    {
                        num2++;
                        num += list[valueIndex];
                    }
                }
                if (num2 != 0)
                {
                    return (num / ((double) num2));
                }
            }
            return 0.0;
        }

        public override string Description()
        {
            return Texts.FunctionAverage;
        }

        [DefaultValue(true), Description("Calculates the average using only the non-null points of a series, or not.")]
        public bool IncludeNulls
        {
            get
            {
                return this.includeNulls;
            }
            set
            {
                if (this.includeNulls != value)
                {
                    this.includeNulls = value;
                    base.Recalculate();
                }
            }
        }
    }
}

