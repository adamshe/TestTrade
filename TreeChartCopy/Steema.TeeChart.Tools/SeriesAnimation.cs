namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [ToolboxBitmap(typeof(SeriesAnimation), "ToolsIcons.SeriesAnimation.bmp"), Description("Performs growing animation of Series points.")]
    public class SeriesAnimation : ToolSeries
    {
        private Series backup;
        private int drawEvery;
        private bool IStop;
        private bool startAtMin;
        private double startValue;
        private int steps;

        public event AnimationStepEventHandler Step;

        public SeriesAnimation() : this((Chart) null)
        {
        }

        public SeriesAnimation(Chart c) : base(c)
        {
            this.steps = 100;
            this.startAtMin = true;
            this.IStop = true;
        }

        public SeriesAnimation(Series s) : this(s.chart)
        {
            base.iSeries = s;
        }

        private void DoAnimation()
        {
            int num4;
            ValueList mandatory = base.iSeries.mandatory;
            double num = this.StartAtMin ? mandatory.Minimum : this.StartValue;
            for (int i = 0; i < base.iSeries.Count; i++)
            {
                mandatory.Value[i] = num;
            }
            this.UpdateChart();
            double num3 = 1.0 / ((double) this.Steps);
            ValueList list2 = this.backup.mandatory;
            if (this.DrawEvery == 0)
            {
                num4 = 0;
            }
            else
            {
                num4 = base.iSeries.Count / this.DrawEvery;
                if ((base.iSeries.Count % this.DrawEvery) == 0)
                {
                    num4--;
                }
            }
            for (int j = 0; j <= num4; j++)
            {
                int num6;
                int num5 = j * this.DrawEvery;
                if (this.DrawEvery == 0)
                {
                    num6 = base.iSeries.Count - 1;
                }
                else
                {
                    num6 = Math.Min((int) (base.iSeries.Count - 1), (int) ((num5 + this.DrawEvery) - 1));
                }
                for (int k = 0; k <= this.Steps; k++)
                {
                    for (int m = num5; m <= num6; m++)
                    {
                        mandatory.Value[m] = num + (((list2.Value[m] - num) * k) * num3);
                    }
                    if (this.Step != null)
                    {
                        this.Step(this, new AnimationStepEventArgs(k));
                    }
                    this.UpdateChart();
                    if (this.IStop)
                    {
                        break;
                    }
                }
            }
        }

        public void Execute()
        {
            if ((base.iSeries != null) && this.IStop)
            {
                this.backup = Series.NewFromType(base.iSeries.GetType());
                try
                {
                    this.backup.AssignValues(base.iSeries);
                    base.iSeries.BeginUpdate();
                    this.IStop = false;
                    try
                    {
                        this.DoAnimation();
                    }
                    finally
                    {
                        this.IStop = true;
                        base.iSeries.EndUpdate();
                    }
                }
                finally
                {
                    this.backup.Dispose();
                }
            }
        }

        private void UpdateChart()
        {
            base.chart.parent.RefreshControl();
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.SeriesAnimTool;
            }
        }

        [DefaultValue(0)]
        public int DrawEvery
        {
            get
            {
                return this.drawEvery;
            }
            set
            {
                this.drawEvery = value;
            }
        }

        [DefaultValue(true)]
        public bool StartAtMin
        {
            get
            {
                return this.startAtMin;
            }
            set
            {
                this.startAtMin = value;
            }
        }

        [DefaultValue(0)]
        public double StartValue
        {
            get
            {
                return this.startValue;
            }
            set
            {
                this.startValue = value;
            }
        }

        [DefaultValue(100)]
        public int Steps
        {
            get
            {
                return this.steps;
            }
            set
            {
                this.steps = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.SeriesAnimationSummary;
            }
        }
    }
}

