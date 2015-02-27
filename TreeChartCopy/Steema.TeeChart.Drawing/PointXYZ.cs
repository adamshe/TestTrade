namespace Steema.TeeChart.Drawing
{
    using System;
    using System.ComponentModel;

    public class PointXYZ
    {
        private int x;
        private int y;
        private int z;

        public PointXYZ()
        {
        }

        public PointXYZ(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [Description("This property gets or sets the X location in pixels.")]
        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        [Description("This property gets or sets the Y location in pixels.")]
        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        [Description("This property gets or sets the Z location in pixels.")]
        public int Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = value;
            }
        }
    }
}

