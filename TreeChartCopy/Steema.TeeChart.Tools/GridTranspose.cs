namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(GridTranspose), "ToolsIcons.GridTranspose.bmp"), Description("Exchanges X and Z coordinates to rotate Series 90 degree.")]
    public class GridTranspose : Tool
    {
        private Custom3DGrid series;

        public GridTranspose() : this(null)
        {
        }

        public GridTranspose(Chart c) : base(c)
        {
        }

        public void Transpose()
        {
            if (base.Active && (this.series != null))
            {
                using (Custom3DGrid grid = new Custom3DGrid())
                {
                    for (int i = 0; i < this.series.Count; i++)
                    {
                        grid.Add(this.series.ZValues.Value[i], this.series.YValues.Value[i], this.series.XValues.Value[i]);
                    }
                    this.series.AssignValues(grid);
                }
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GridTranspose;
            }
        }

        public Custom3DGrid Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.GridTransposeSummary;
            }
        }
    }
}

