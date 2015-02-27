namespace Steema.TeeChart.Styles
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class ContourLevel
    {
        private System.Drawing.Color color;
        private Contour iSeries;
        private double upTo;

        public ContourLevel(Contour cs)
        {
            this.iSeries = cs;
        }

        private void CheckAuto()
        {
            if (!this.iSeries.iModifyingLevels)
            {
                this.iSeries.AutomaticLevels = false;
            }
        }

        private void SetColor(System.Drawing.Color value)
        {
            this.color = value;
            this.CheckAuto();
        }

        private void SetUpTo(double value)
        {
            this.upTo = value;
            this.CheckAuto();
        }

        [Category("Appearance"), Description("Colour of TContourSeries Level.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.SetColor(value);
            }
        }

        [Description("Sets range value for ContourLevel.")]
        public double UpToValue
        {
            get
            {
                return this.upTo;
            }
            set
            {
                this.SetUpTo(value);
            }
        }
    }
}

