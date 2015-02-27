namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;

    [Description("Axes properties."), Editor(typeof(AxesComponentEditor), typeof(UITypeEditor))]
    public class Axes : TeeBase
    {
        private Axis bottom;
        internal CustomAxes custom;
        internal DepthAxis depth;
        private bool drawBehind;
        private Axis left;
        private Axis right;
        private Axis top;
        private bool visible;

        public Axes(Chart c) : base(c)
        {
            this.custom = new CustomAxes();
            this.visible = true;
            this.drawBehind = true;
            this.custom.Chart = base.chart;
            this.left = new Axis(base.chart);
            this.left.Title.SetInitialAngle(90);
            this.right = new Axis(false, true, base.chart);
            this.right.ZPosition = 100.0;
            this.right.Title.SetInitialAngle(270);
            this.top = new Axis(true, true, base.chart);
            this.top.ZPosition = 100.0;
            this.bottom = new Axis(true, false, base.chart);
            this.depth = new DepthAxis(false, true, base.chart);
        }

        internal void AdjustMaxMin()
        {
            this.left.AdjustMaxMin();
            this.top.AdjustMaxMin();
            this.right.AdjustMaxMin();
            this.bottom.AdjustMaxMin();
            this.depth.AdjustMaxMin();
            foreach (Axis axis in this.custom)
            {
                axis.AdjustMaxMin();
            }
        }

        internal void CheckAxis(ref Axis a)
        {
            if (a == null)
            {
                a = new Axis(base.chart);
            }
        }

        public static Axis CreateNewAxis(Chart chart)
        {
            Axis axis = (Axis) Activator.CreateInstance(typeof(Axis));
            axis.Chart = chart;
            return axis;
        }

        internal void DoZoom(int x0, int y0, int x1, int y1)
        {
            base.chart.DoZoom(this.Top.CalcPosPoint(x0), this.Top.CalcPosPoint(x1), this.Bottom.CalcPosPoint(x0), this.Bottom.CalcPosPoint(x1), this.Left.CalcPosPoint(y1), this.Left.CalcPosPoint(y0), this.Right.CalcPosPoint(y1), this.Right.CalcPosPoint(y0));
        }

        public void Draw()
        {
            this.Draw(base.chart.graphics3D);
        }

        public void Draw(Graphics3D g)
        {
            if (base.chart.parent != null)
            {
                base.chart.parent.DoBeforeDrawAxes();
            }
            if (base.chart.IsAxisVisible(this.left))
            {
                this.left.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.right))
            {
                this.right.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.top))
            {
                this.top.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.bottom))
            {
                this.bottom.Draw(true);
            }
            if (base.chart.IsAxisVisible(this.depth))
            {
                this.depth.Draw(true);
            }
            foreach (Axis axis in this.custom)
            {
                if (axis.Visible)
                {
                    axis.Draw(true);
                }
            }
        }

        public int IndexOf(Axis a)
        {
            if (a == this.left)
            {
                return 0;
            }
            if (a == this.top)
            {
                return 1;
            }
            if (a == this.right)
            {
                return 2;
            }
            if (a == this.bottom)
            {
                return 3;
            }
            if (a == this.depth)
            {
                return 4;
            }
            return this.custom.IndexOf(a);
        }

        internal void InternalCalcPositions()
        {
            this.left.InternalCalcPositions();
            this.top.InternalCalcPositions();
            this.right.InternalCalcPositions();
            this.bottom.InternalCalcPositions();
            this.depth.InternalCalcPositions();
            foreach (Axis axis in this.custom)
            {
                axis.InternalCalcPositions();
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.custom.Chart = c;
        }

        public string[] StringItems()
        {
            string[] strArray = new string[4 + this.custom.Count];
            strArray[0] = Texts.LeftAxis;
            strArray[1] = Texts.TopAxis;
            strArray[2] = Texts.RightAxis;
            strArray[3] = Texts.BottomAxis;
            for (int i = 0; i < this.custom.Count; i++)
            {
                strArray[4 + i] = "Custom " + i.ToString();
            }
            return strArray;
        }

        [Category("Axes"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Bottom Axis.")]
        public Axis Bottom
        {
            get
            {
                this.CheckAxis(ref this.bottom);
                return this.bottom;
            }
        }

        [Browsable(false), Description("Returns the number of custom axes."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get
            {
                return (this.custom.Count + 5);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Custom defined axes collection.")]
        public CustomAxes Custom
        {
            get
            {
                return this.custom;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Axes"), Description("Depth Axis.")]
        public Axis Depth
        {
            get
            {
                if (this.depth == null)
                {
                    this.depth = new DepthAxis(false, true, base.chart);
                }
                return this.depth;
            }
        }

        [Description("Draw axes behind or in front of Series."), DefaultValue(true)]
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawBehind, value);
            }
        }

        public Axis this[int index]
        {
            get
            {
                if (index < (this.custom.Count + 5))
                {
                    if (index >= 5)
                    {
                        return this.custom[index - 5];
                    }
                    switch (index)
                    {
                        case 0:
                            return this.left;

                        case 1:
                            return this.top;

                        case 2:
                            return this.right;

                        case 3:
                            return this.bottom;

                        case 4:
                            return this.depth;
                    }
                }
                return null;
            }
        }

        [Description("Left Axis."), Category("Axes"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Axis Left
        {
            get
            {
                this.CheckAxis(ref this.left);
                return this.left;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Right Axis."), Category("Axes")]
        public Axis Right
        {
            get
            {
                this.CheckAxis(ref this.right);
                return this.right;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Top Axis."), Category("Axes")]
        public Axis Top
        {
            get
            {
                this.CheckAxis(ref this.top);
                return this.top;
            }
        }

        [Category("Axes"), DefaultValue(true), Description("Shows / Hides all Chart axes.")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }

        internal class AxesComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Axes axes = (Axes) value;
                bool flag = EditorUtils.ShowFormModal(new AxesEditor(axes.Chart, null));
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

