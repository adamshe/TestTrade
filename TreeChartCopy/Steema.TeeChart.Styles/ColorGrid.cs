namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(ColorGrid), "SeriesIcons.ColorGrid.bmp")]
    public class ColorGrid : Custom3DGrid
    {
        private System.Drawing.Bitmap bbitmap;
        private bool centered;
        private int tmpDec;
        private double XStep;
        private double ZStep;

        public ColorGrid() : this(null)
        {
        }

        public ColorGrid(Chart c) : base(c)
        {
            base.Marks.Callout.Length = 0;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.NoGrid);
        }

        protected internal override void Draw()
        {
            if (base.Count > 0)
            {
                if (base.IrregularGrid)
                {
                    this.ZStep = this.GetZStep();
                    this.XStep = this.GetXStep();
                    this.ZStep = (this.XStep == 0.0) ? 0.0 : this.ZStep;
                    this.XStep = (this.ZStep == 0.0) ? 0.0 : this.XStep;
                }
                this.tmpDec = 0;
                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(base.iNumXValues - this.tmpDec, base.iNumZValues - this.tmpDec))
                {
                    this.FillBitmap(bitmap);
                    Rectangle r = new Rectangle(0, 0, 0, 0);
                    this.DrawBitmap(bitmap, ref r);
                    this.bbitmap = (System.Drawing.Bitmap) bitmap.Clone();
                    if (base.Pen.Visible)
                    {
                        this.DrawGrid(r);
                    }
                }
            }
        }

        private void DrawBitmap(System.Drawing.Bitmap bitmap, ref Rectangle r)
        {
            Axis getHorizAxis = base.GetHorizAxis;
            r.X = getHorizAxis.CalcPosValue(this.MinXValue());
            r.Width = getHorizAxis.CalcPosValue(this.MaxXValue()) - r.X;
            getHorizAxis = base.GetVertAxis;
            r.Y = getHorizAxis.CalcPosValue(this.MinZValue());
            r.Height = getHorizAxis.CalcPosValue(this.MaxZValue()) - r.Y;
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.PrepareDrawImage();
            if (base.chart.Aspect.View3D)
            {
                graphicsd.Draw(graphicsd.CalcRect3D(r, base.MiddleZ), bitmap, false);
            }
            else
            {
                graphicsd.Draw(r, bitmap, false);
            }
        }

        private void DrawGrid(Rectangle r)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Pen = base.Pen;
            double num = 0.0;
            double num2 = 0.0;
            if (base.IrregularGrid)
            {
                num = this.centered ? (this.ZStep / 2.0) : 0.0;
                num2 = this.centered ? (this.XStep / 2.0) : 0.0;
            }
            else
            {
                num = this.centered ? 0.5 : 0.0;
                num2 = this.centered ? 0.5 : 0.0;
            }
            for (int i = 2; i <= (base.iNumZValues - this.tmpDec); i++)
            {
                int y = base.GetVertAxis.CalcPosValue(base.vzValues[base.gridIndex[1][i]] - num);
                if (base.chart.aspect.view3D)
                {
                    graphicsd.HorizontalLine(r.X, r.Right, y, base.middleZ);
                }
                else
                {
                    graphicsd.HorizontalLine(r.X, r.Right, y);
                }
            }
            for (int j = 2; j <= (base.iNumXValues - this.tmpDec); j++)
            {
                int x = base.GetHorizAxis.CalcPosValue(base.vxValues[base.gridIndex[j][1]] - num2);
                if (base.chart.aspect.View3D)
                {
                    graphicsd.VerticalLine(x, r.Y, r.Bottom, base.startZ);
                }
                else
                {
                    graphicsd.VerticalLine(x, r.Y, r.Bottom);
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            double num = 0.0;
            double num2 = 0.0;
            if (base.IrregularGrid)
            {
                num2 = this.centered ? 0.0 : (this.ZStep / 2.0);
                num = this.centered ? 0.0 : (this.XStep / 2.0);
            }
            else
            {
                num2 = this.centered ? 0.0 : 0.5;
                num = this.centered ? 0.0 : 0.5;
            }
            aPosition.LeftTop.Y = base.GetVertAxis.CalcPosValue(base.vzValues.Value[valueIndex] + num2) - (aPosition.Height / 2);
            aPosition.LeftTop.X = base.GetHorizAxis.CalcPosValue(base.vxValues.Value[valueIndex] + num) - (aPosition.Width / 2);
            base.marks.InternalDraw(valueIndex, this.ValueColor(valueIndex), st, aPosition);
        }

        private void FillBitmap(System.Drawing.Bitmap bitmap)
        {
            for (int i = 1; i <= (base.iNumZValues - this.tmpDec); i++)
            {
                for (int j = 1; j <= (base.iNumXValues - this.tmpDec); j++)
                {
                    int valueIndex = base.gridIndex[j][i];
                    if (valueIndex != -1)
                    {
                        bitmap.SetPixel(j - 1, i - 1, this.ValueColor(valueIndex));
                    }
                }
            }
        }

        protected internal override void GalleryChanged3D(bool Is3D)
        {
            base.chart.Aspect.View3D = false;
            base.chart.Aspect.ClipPoints = true;
        }

        protected internal double GetXStep()
        {
            double num4 = 0.0;
            bool flag = true;
            for (int i = 1; i <= base.iNumXValues; i++)
            {
                double num = base.vxValues[base.gridIndex[i][1]];
                if ((i + 1) <= base.iNumXValues)
                {
                    double num2 = base.vxValues[base.gridIndex[i + 1][1]];
                    double num3 = num2 - num;
                    if (i == 1)
                    {
                        num4 = num3;
                    }
                    if (!flag)
                    {
                        break;
                    }
                    flag = Math.Abs((double) (num4 - num3)) <= 1E-05;
                    num4 = num3;
                }
            }
            if (flag)
            {
                return num4;
            }
            return 0.0;
        }

        protected internal double GetZStep()
        {
            double num4 = 0.0;
            bool flag = true;
            for (int i = 1; i <= base.iNumZValues; i++)
            {
                double num = base.vzValues[base.gridIndex[1][i]];
                if ((i + 1) <= base.iNumXValues)
                {
                    double num2 = base.vzValues[base.gridIndex[1][i + 1]];
                    double num3 = num2 - num;
                    if (i == 1)
                    {
                        num4 = num3;
                    }
                    if (!flag)
                    {
                        break;
                    }
                    flag = flag = Math.Abs((double) (num4 - num3)) <= 1E-05;
                    num4 = num3;
                }
            }
            if (flag)
            {
                return num4;
            }
            return 0.0;
        }

        public override double MaxXValue()
        {
            double maximum = base.vxValues.Maximum;
            if (this.centered)
            {
                if (base.IrregularGrid)
                {
                    if (this.GetZStep() != 0.0)
                    {
                        maximum += this.GetXStep() / 2.0;
                    }
                    return maximum;
                }
                return (maximum + 0.5);
            }
            if (base.IrregularGrid)
            {
                if (this.GetZStep() != 0.0)
                {
                    maximum += this.GetXStep();
                }
                return maximum;
            }
            maximum++;
            return maximum;
        }

        public override double MaxYValue()
        {
            return this.MaxZValue();
        }

        public override double MaxZValue()
        {
            double maximum = base.vzValues.Maximum;
            if (this.centered)
            {
                if (base.IrregularGrid)
                {
                    if (this.GetXStep() != 0.0)
                    {
                        maximum += this.GetZStep() / 2.0;
                    }
                    return maximum;
                }
                return (maximum + 0.5);
            }
            if (base.IrregularGrid)
            {
                if (this.GetXStep() != 0.0)
                {
                    maximum += this.GetZStep();
                }
                return maximum;
            }
            maximum++;
            return maximum;
        }

        public override double MinXValue()
        {
            double minimum = base.vxValues.Minimum;
            if (!this.centered)
            {
                return minimum;
            }
            if (base.IrregularGrid)
            {
                if (this.GetZStep() != 0.0)
                {
                    minimum -= this.GetXStep() / 2.0;
                }
                return minimum;
            }
            return (minimum - 0.5);
        }

        public override double MinYValue()
        {
            return this.MinZValue();
        }

        public override double MinZValue()
        {
            double minimum = base.vzValues.Minimum;
            if (!this.centered)
            {
                return minimum;
            }
            if (base.IrregularGrid)
            {
                if (this.GetXStep() != 0.0)
                {
                    minimum -= this.GetZStep() / 2.0;
                }
                return minimum;
            }
            return (minimum - 0.5);
        }

        private void SetBitmap(System.Drawing.Bitmap bitmap)
        {
            this.bbitmap = (System.Drawing.Bitmap) bitmap.Clone();
            base.NumXValues = bitmap.Width;
            base.NumZValues = bitmap.Height;
            base.BeginUpdate();
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    base.Add((double) i, (double) pixel.ToArgb(), (double) j, "", pixel);
                }
            }
            base.EndUpdate();
        }

        protected internal override void SetSubGallery(int index)
        {
            if (index == 2)
            {
                base.Pen.Visible = false;
            }
            else
            {
                base.SetSubGallery(index);
            }
        }

        [Description("Bitmap property"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Bitmap Bitmap
        {
            get
            {
                return this.bbitmap;
            }
            set
            {
                this.SetBitmap(value);
            }
        }

        [DefaultValue(false)]
        public bool CenteredPoints
        {
            get
            {
                return this.centered;
            }
            set
            {
                base.SetBooleanProperty(ref this.centered, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryColorGrid;
            }
        }
    }
}

