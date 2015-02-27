namespace Steema.TeeChart
{
    using System;
    using System.ComponentModel;

    public class Margins
    {
        private int bottom;
        private int left;
        private int right;
        private int top;

        public Margins(int l, int t, int r, int b)
        {
            this.left = l;
            this.top = t;
            this.right = r;
            this.bottom = b;
        }

        [Description("Sets Bottom margin as percentage of paper dimensions.")]
        public int Bottom
        {
            get
            {
                return this.bottom;
            }
            set
            {
                this.bottom = value;
            }
        }

        [Description("Sets Left margin as percentage of paper dimensions.")]
        public int Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        [Description("Sets Right margin as percentage of paper dimensions.")]
        public int Right
        {
            get
            {
                return this.right;
            }
            set
            {
                this.right = value;
            }
        }

        [Description("Sets Top margin as percentage of paper dimensions.")]
        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }
    }
}

