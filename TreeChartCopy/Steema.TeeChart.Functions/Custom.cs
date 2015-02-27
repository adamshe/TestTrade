namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Custom : Function
    {
        private int numPoints;
        private double startX;
        public double X;

        public event CalculateEventHandler CalculateEvent;

        public Custom() : this(null)
        {
        }

        public Custom(Chart c) : base(c)
        {
            this.numPoints = 100;
            base.NoSourceRequired = true;
            base.dPeriod = 1.0;
        }

        public override void AddPoints(Array source)
        {
            if (!base.updating)
            {
                base.Series.Clear();
                this.X = this.startX;
                CalculateEventArgs e = new CalculateEventArgs(this.X, 0.0);
                for (int i = 1; i <= this.numPoints; i++)
                {
                    e.x = this.X;
                    if (this.CalculateEvent != null)
                    {
                        this.CalculateEvent(this, e);
                    }
                    if (base.Series.yMandatory)
                    {
                        base.Series.Add(this.X, e.Y);
                    }
                    else
                    {
                        base.Series.Add(e.Y, this.X);
                    }
                    this.X += base.Period;
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionCustom;
        }

        [DefaultValue(100), Description("Gets or sets the number of points.")]
        public int NumPoints
        {
            get
            {
                return this.numPoints;
            }
            set
            {
                this.numPoints = value;
            }
        }

        [DefaultValue((double) 0.0), Description("The initial value for the X parameter.")]
        public double StartX
        {
            get
            {
                return this.startX;
            }
            set
            {
                this.startX = value;
            }
        }
    }
}

