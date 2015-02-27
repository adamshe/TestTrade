namespace Steema.TeeChart.Styles
{
    using System;
    using System.ComponentModel;

    public class SeriesXYZPoint : SeriesXYPoint
    {
        internal Custom3D custom3D;

        public SeriesXYZPoint()
        {
        }

        public SeriesXYZPoint(Custom3D s, int i)
        {
            base.index = i;
            this.custom3D = s;
            base.series = s;
        }

        [Description("Indexed Point Z value")]
        public double Z
        {
            get
            {
                return this.custom3D.ZValues[base.index];
            }
            set
            {
                this.custom3D.ZValues[base.index] = value;
                this.custom3D.Invalidate();
            }
        }
    }
}

