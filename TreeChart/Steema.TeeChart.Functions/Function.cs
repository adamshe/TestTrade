namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable, Designer(typeof(FunctionDesigner)), ToolboxBitmap(typeof(Function), "Images.BasicFunction.bmp"), DefaultProperty("Period"), Editor(typeof(FunctionEditor), typeof(UITypeEditor))]
    public class Function : TeeBase
    {
        protected bool CanUsePeriod;
        protected double dPeriod;
        internal bool NoSourceRequired;
        private PeriodAligns periodAlign;
        private PeriodStyles periodStyle;
        internal Steema.TeeChart.Styles.Series series;
        protected bool updating;

        public Function() : this(null)
        {
        }

        public Function(Chart c) : base(c)
        {
            this.periodStyle = PeriodStyles.NumPoints;
            this.periodAlign = PeriodAligns.Center;
            this.CanUsePeriod = true;
        }

        protected void AddFunctionXY(bool yMandatorySource, double tmpX, double tmpY)
        {
            if (yMandatorySource)
            {
                this.series.Add(tmpX, tmpY);
            }
            else
            {
                this.series.Add(tmpY, tmpX);
            }
        }

        public virtual void AddPoints(Array source)
        {
            if (!this.updating && (source != null))
            {
                if (source.Length > 1)
                {
                    this.CalculateFunctionMany(source);
                }
                else if (source.Length > 0)
                {
                    Steema.TeeChart.Styles.Series series = (Steema.TeeChart.Styles.Series) source.GetValue(0);
                    if (series.Count > 0)
                    {
                        this.DoCalculation(series, series.notMandatory);
                    }
                }
            }
        }

        private void BeginUpdate()
        {
            this.updating = true;
        }

        public virtual double Calculate(Steema.TeeChart.Styles.Series source, int first, int last)
        {
            return 0.0;
        }

        protected void CalculateAllPoints(Steema.TeeChart.Styles.Series source, Steema.TeeChart.Styles.ValueList notMandatorySource)
        {
            double minimum;
            double tmpY = this.Calculate(source, -1, -1);
            if (!this.series.AllowSinglePoint)
            {
                minimum = notMandatorySource.Minimum;
                this.AddFunctionXY(source.yMandatory, minimum, tmpY);
                minimum = notMandatorySource.Maximum;
                this.AddFunctionXY(source.yMandatory, minimum, tmpY);
            }
            else if (!source.yMandatory && this.series.yMandatory)
            {
                minimum = notMandatorySource.Minimum + (0.5 * notMandatorySource.Range);
                this.series.Add(minimum, tmpY);
            }
            else
            {
                minimum = notMandatorySource.Minimum + (0.5 * notMandatorySource.Range);
                if (this.series.yMandatory)
                {
                    this.AddFunctionXY(source.yMandatory, minimum, tmpY);
                }
                else
                {
                    this.series.Add(tmpY, minimum);
                }
            }
        }

        protected void CalculateByPeriod(Steema.TeeChart.Styles.Series source, Steema.TeeChart.Styles.ValueList notMandatorySource)
        {
            int firstIndex = 0;
            int count = source.Count;
            double num7 = notMandatorySource[firstIndex];
            DateTimeSteps tmpWhichDatetime = Axis.FindDateTimeStep(this.dPeriod);
            do
            {
                int num2;
                double num3;
                double num4 = 0.0;
                if (this.periodStyle == PeriodStyles.NumPoints)
                {
                    num2 = (firstIndex + Utils.Round(this.dPeriod)) - 1;
                    num3 = notMandatorySource[firstIndex];
                    if (num2 < count)
                    {
                        num4 = notMandatorySource[num2];
                    }
                }
                else
                {
                    num2 = firstIndex;
                    num3 = num7;
                    this.series.GetHorizAxis.IncDecDateTime(true, ref num7, this.dPeriod, tmpWhichDatetime);
                    num4 = num7 - (this.dPeriod * 0.001);
                    while (num2 < (count - 1))
                    {
                        if (notMandatorySource[num2 + 1] >= num7)
                        {
                            break;
                        }
                        num2++;
                    }
                }
                bool flag = false;
                if (num2 < count)
                {
                    double num;
                    if (this.periodAlign == PeriodAligns.First)
                    {
                        num = num3;
                    }
                    else if (this.periodAlign == PeriodAligns.Last)
                    {
                        num = num4;
                    }
                    else
                    {
                        num = (num3 + num4) * 0.5;
                    }
                    if ((this.periodStyle == PeriodStyles.Range) && (notMandatorySource[firstIndex] < num7))
                    {
                        flag = true;
                    }
                    if ((this.periodStyle == PeriodStyles.NumPoints) || flag)
                    {
                        this.CalculatePeriod(source, num, firstIndex, num2);
                    }
                    else
                    {
                        this.AddFunctionXY(source.yMandatory, num, 0.0);
                    }
                }
                if ((this.periodStyle == PeriodStyles.NumPoints) || flag)
                {
                    firstIndex = num2 + 1;
                }
            }
            while (firstIndex <= (count - 1));
        }

        private void CalculateFunctionMany(Array Source)
        {
            Steema.TeeChart.Styles.Series series = (Steema.TeeChart.Styles.Series) Source.GetValue(0);
            Steema.TeeChart.Styles.ValueList notMandatory = series.notMandatory;
            foreach (object obj2 in Source)
            {
                if ((obj2 != null) && (((Steema.TeeChart.Styles.Series) obj2).Count > series.Count))
                {
                    series = (Steema.TeeChart.Styles.Series) obj2;
                    notMandatory = series.notMandatory;
                }
            }
            if (notMandatory != null)
            {
                ArrayList sourceSeries = new ArrayList();
                foreach (object obj3 in Source)
                {
                    sourceSeries.Add(obj3);
                }
                for (int i = 0; i < series.Count; i++)
                {
                    double x = notMandatory[i];
                    double y = this.CalculateMany(sourceSeries, i);
                    if (!series.yMandatory)
                    {
                        double num4 = x;
                        x = y;
                        y = num4;
                    }
                    this.series.Add(x, y, series.Labels[i]);
                }
            }
        }

        public virtual double CalculateMany(ArrayList sourceSeries, int valueIndex)
        {
            return 0.0;
        }

        protected void CalculatePeriod(Steema.TeeChart.Styles.Series source, double tmpX, int firstIndex, int lastIndex)
        {
            this.AddFunctionXY(source.yMandatory, tmpX, this.Calculate(source, firstIndex, lastIndex));
        }

        public virtual string Description()
        {
            return Texts.GalleryFunctions;
        }

        protected virtual void DoCalculation(Steema.TeeChart.Styles.Series source, Steema.TeeChart.Styles.ValueList notMandatorySource)
        {
            if (this.dPeriod == 0.0)
            {
                this.CalculateAllPoints(source, notMandatorySource);
            }
            else
            {
                this.CalculateByPeriod(source, notMandatorySource);
            }
        }

        private void EndUpdate()
        {
            if (this.updating)
            {
                this.updating = false;
                this.Recalculate();
            }
        }

        internal static Function NewInstance(Type f)
        {
            return (Function) Activator.CreateInstance(f);
        }

        public void Recalculate()
        {
            if (!this.updating && (this.series != null))
            {
                this.series.CheckDataSource();
            }
        }

        protected Steema.TeeChart.Styles.ValueList ValueList(Steema.TeeChart.Styles.Series s)
        {
            string aListName = (this.series != null) ? this.series.mandatory.valueSource : "";
            if (aListName.Length == 0)
            {
                return s.mandatory;
            }
            return s.GetYValueList(aListName);
        }

        [Description("Defines range of source values for calculation. Zero means all source points."), DefaultValue((double) 0.0)]
        public double Period
        {
            get
            {
                return this.dPeriod;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new TeeChartException(Texts.FunctionPeriod);
                }
                if (this.dPeriod != value)
                {
                    this.dPeriod = value;
                    this.Recalculate();
                }
            }
        }

        [DefaultValue(1), Description("The position of calculation output points within range.")]
        public PeriodAligns PeriodAlign
        {
            get
            {
                return this.periodAlign;
            }
            set
            {
                if (this.periodAlign != value)
                {
                    this.periodAlign = value;
                    this.Recalculate();
                }
            }
        }

        [Description("Controls how to use Period property. (as number of points or as range)"), DefaultValue(0)]
        public PeriodStyles PeriodStyle
        {
            get
            {
                return this.periodStyle;
            }
            set
            {
                if (this.periodStyle != value)
                {
                    this.periodStyle = value;
                    this.Recalculate();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
            set
            {
                if (this.series != value)
                {
                    if (this.series != null)
                    {
                        this.series.Function = null;
                    }
                    this.series.Function = this;
                }
            }
        }

        internal sealed class FunctionDesigner : ComponentDesigner
        {
            public FunctionDesigner()
            {
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.OnEdit)));
            }

            private void OnEdit(object sender, EventArgs e)
            {
                Function component = (Function) base.Component;
                if (ChartEditor.ShowModal(component.series, ChartEditorTabs.SeriesDataSource))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }
        }

        internal sealed class FunctionEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Function function = (Function) value;
                bool flag = ChartEditor.ShowModal(function.series, ChartEditorTabs.SeriesDataSource);
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

