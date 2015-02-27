namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class CompressOHLC : Function
    {
        private CompressionPeriod FCompress;

        public event CompressGetDateHandler OnGetDate;

        public CompressOHLC() : this(null)
        {
        }

        public CompressOHLC(Chart c) : base(c)
        {
            base.CanUsePeriod = false;
            this.FCompress = CompressionPeriod.ocWeek;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if ((series.Count > 0) && (base.Series is OHLC))
                {
                    this.CompressSeries(series, (OHLC) base.Series, null, null);
                }
            }
        }

        private void CompressSeries(Series Source, OHLC DestOHLC, Series Volume, Series DestVolume)
        {
            DestOHLC.Clear();
            if (DestVolume != null)
            {
                DestVolume.Clear();
            }
            int num3 = 0;
            int dayOfWeek = 0;
            ValueList list = base.ValueList(Source);
            int period = (int) base.Period;
            for (int i = 0; i < Source.Count; i++)
            {
                int num4;
                bool flag;
                double num8;
                DateTime date = DateTime.FromOADate(Source.notMandatory[i]);
                if (period == 0)
                {
                    CompressGetDateEventArgs e = new CompressGetDateEventArgs(Source, i, date);
                    if (this.OnGetDate != null)
                    {
                        this.OnGetDate(this, e);
                    }
                    int day = date.Day;
                    int month = date.Month;
                    int year = date.Year;
                    switch (this.Compress)
                    {
                        case CompressionPeriod.ocDay:
                            dayOfWeek = (int) Math.Round(date.ToOADate(), 0);
                            break;

                        case CompressionPeriod.ocWeek:
                            dayOfWeek = (int) date.DayOfWeek;
                            if (dayOfWeek == 0)
                            {
                                dayOfWeek = 7;
                            }
                            break;

                        case CompressionPeriod.ocMonth:
                            dayOfWeek = month;
                            break;

                        case CompressionPeriod.ocBiMonth:
                            dayOfWeek = (month - 1) / 2;
                            break;

                        case CompressionPeriod.ocQuarter:
                            dayOfWeek = (month - 1) / 3;
                            break;

                        case CompressionPeriod.ocYear:
                            dayOfWeek = year;
                            break;
                    }
                    if (this.Compress == CompressionPeriod.ocWeek)
                    {
                        flag = dayOfWeek < num3;
                    }
                    else
                    {
                        flag = dayOfWeek != num3;
                    }
                    num3 = dayOfWeek;
                }
                else
                {
                    flag = (i % period) == 0;
                }
                if ((i == 0) || flag)
                {
                    if (Source is OHLC)
                    {
                        num4 = DestOHLC.Add(date, ((OHLC) Source).OpenValues[i], ((OHLC) Source).HighValues[i], ((OHLC) Source).LowValues[i], ((OHLC) Source).CloseValues[i]);
                    }
                    else
                    {
                        num8 = list.Value[i];
                        num4 = DestOHLC.Add(date, num8, num8, num8, num8);
                    }
                    DestOHLC.Labels[num4] = Source.Labels[i];
                    if (DestVolume != null)
                    {
                        DestVolume.Add(Source.notMandatory[i], Volume.YValues[i]);
                    }
                }
                else
                {
                    num4 = DestOHLC.Count - 1;
                    if (Source is OHLC)
                    {
                        DestOHLC.CloseValues[num4] = ((OHLC) Source).CloseValues[i];
                        DestOHLC.DateValues.Value[num4] = ((OHLC) Source).DateValues[i];
                        if (((OHLC) Source).HighValues[i] > DestOHLC.HighValues[num4])
                        {
                            DestOHLC.HighValues[num4] = ((OHLC) Source).HighValues[i];
                        }
                        if (((OHLC) Source).LowValues[i] < DestOHLC.LowValues.Value[num4])
                        {
                            DestOHLC.LowValues[num4] = ((OHLC) Source).LowValues[i];
                        }
                    }
                    else
                    {
                        num8 = list[i];
                        DestOHLC.CloseValues[num4] = num8;
                        DestOHLC.DateValues[num4] = Source.notMandatory[i];
                        if (num8 > DestOHLC.HighValues[num4])
                        {
                            DestOHLC.HighValues[num4] = num8;
                        }
                        if (num8 < DestOHLC.LowValues[num4])
                        {
                            DestOHLC.LowValues[num4] = num8;
                        }
                    }
                    DestOHLC.Labels[num4] = Source.Labels[i];
                    if (DestVolume != null)
                    {
                        DestVolume.YValues[num4] += Volume.YValues[i];
                        DestVolume.XValues.Value[num4] = Volume.XValues[i];
                    }
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionCompressOHLC;
        }

        [Description("Gets or sets the Compression Period as defined by the CompressionPeriod enumeration.")]
        public CompressionPeriod Compress
        {
            get
            {
                return this.FCompress;
            }
            set
            {
                if (this.FCompress != value)
                {
                    this.FCompress = value;
                }
                base.Recalculate();
            }
        }
    }
}

