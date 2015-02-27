namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;

    [Description("Properties to define Chart 3D appearance."), Editor(typeof(AspectComponentEditor), typeof(UITypeEditor))]
    public sealed class Aspect : TeeBase
    {
        internal bool applyZOrder;
        private int chart3D;
        internal bool clipPoints;
        private int elevation;
        public int Height3D;
        private int horizOffset;
        private int orthoAngle;
        internal bool orthogonal;
        private int perspective;
        internal int rotation;
        private int tilt;
        private int vertOffset;
        internal bool view3D;
        public int Width3D;
        private int zoom;
        private bool zoomText;

        public Aspect() : this(null)
        {
        }

        public Aspect(Chart c) : base(c)
        {
            this.applyZOrder = true;
            this.chart3D = 15;
            this.clipPoints = true;
            this.elevation = 0x159;
            this.orthoAngle = 0x2d;
            this.orthogonal = true;
            this.perspective = 15;
            this.rotation = 0x159;
            this.view3D = true;
            this.zoom = 100;
            this.zoomText = true;
        }

        public void Assign(Aspect a)
        {
            this.applyZOrder = a.applyZOrder;
            this.chart3D = a.chart3D;
            this.clipPoints = a.clipPoints;
            this.elevation = a.elevation;
            this.horizOffset = a.horizOffset;
            this.orthoAngle = a.orthoAngle;
            this.orthogonal = a.orthogonal;
            this.perspective = a.perspective;
            this.rotation = a.rotation;
            this.tilt = a.tilt;
            this.vertOffset = a.vertOffset;
            this.view3D = a.view3D;
            this.zoom = a.zoom;
            this.zoomText = a.zoomText;
        }

        [DefaultValue(true), Description("When True, multiple Series are displayed at a different 3D 'Z' (depth) position.")]
        public bool ApplyZOrder
        {
            get
            {
                return this.applyZOrder;
            }
            set
            {
                base.SetBooleanProperty(ref this.applyZOrder, value);
            }
        }

        [Description("Percent from 0 to 100 of Z Depth."), DefaultValue(15)]
        public int Chart3DPercent
        {
            get
            {
                return this.chart3D;
            }
            set
            {
                base.SetIntegerProperty(ref this.chart3D, value);
            }
        }

        [DefaultValue(true), Description("When True, restricts Series points to display outside the Chart axes rectangle.")]
        public bool ClipPoints
        {
            get
            {
                return this.clipPoints;
            }
            set
            {
                base.SetBooleanProperty(ref this.clipPoints, value);
            }
        }

        [Description("Gets or sets the angle in degrees of 3D elevation."), DefaultValue(0x159)]
        public int Elevation
        {
            get
            {
                return this.elevation;
            }
            set
            {
                base.SetIntegerProperty(ref this.elevation, value);
            }
        }

        [Description("Amount (postive or negative) in pixels of horizontal displacement."), DefaultValue(0)]
        public int HorizOffset
        {
            get
            {
                return this.horizOffset;
            }
            set
            {
                base.SetIntegerProperty(ref this.horizOffset, value);
            }
        }

        [Description("Angle in degrees, from 0 to 90, when displaying in Orthogonal mode."), DefaultValue(0x2d)]
        public int OrthoAngle
        {
            get
            {
                return this.orthoAngle;
            }
            set
            {
                base.SetIntegerProperty(ref this.orthoAngle, value);
            }
        }

        [Description("When True, display in semi-3D mode."), DefaultValue(true)]
        public bool Orthogonal
        {
            get
            {
                return this.orthogonal;
            }
            set
            {
                base.SetBooleanProperty(ref this.orthogonal, value);
            }
        }

        [DefaultValue(15), Description("Percent of 3D perspective.")]
        public int Perspective
        {
            get
            {
                return this.perspective;
            }
            set
            {
                base.SetIntegerProperty(ref this.perspective, value);
            }
        }

        [Description("Gets or sets the angle in degrees of 3D rotation."), DefaultValue(0x159)]
        public int Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                base.SetIntegerProperty(ref this.rotation, value);
            }
        }

        [Description("Chooses between speed or display quality."), DefaultValue(1)]
        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get
            {
                return base.chart.graphics3D.SmoothingMode;
            }
            set
            {
                base.chart.graphics3D.SmoothingMode = value;
            }
        }

        [DefaultValue(0)]
        public System.Drawing.Text.TextRenderingHint TextRenderingHint
        {
            get
            {
                return base.chart.graphics3D.TextRenderingHint;
            }
            set
            {
                base.chart.graphics3D.TextRenderingHint = value;
            }
        }

        [DefaultValue(0), Description("Gets or sets the angle in degrees of 3D tilt.")]
        public int Tilt
        {
            get
            {
                return this.tilt;
            }
            set
            {
                base.SetIntegerProperty(ref this.tilt, value);
            }
        }

        [DefaultValue(0), Description("Amount (postive or negative) in pixels of vertical displacement.")]
        public int VertOffset
        {
            get
            {
                return this.vertOffset;
            }
            set
            {
                base.SetIntegerProperty(ref this.vertOffset, value);
            }
        }

        [Description("Gets or sets 3D display mode."), DefaultValue(true)]
        public bool View3D
        {
            get
            {
                return this.view3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.view3D, value);
                base.chart.BroadcastEvent(new View3DEvent());
            }
        }

        [Description("Percent of zoom in 3D mode."), DefaultValue(100)]
        public int Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                base.SetIntegerProperty(ref this.zoom, value);
            }
        }

        [Description("When True, all texts are resized according to Zoom property."), DefaultValue(true)]
        public bool ZoomText
        {
            get
            {
                return this.zoomText;
            }
            set
            {
                base.SetBooleanProperty(ref this.zoomText, value);
            }
        }

        internal class AspectComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Aspect aspect = (Aspect) value;
                bool flag = EditorUtils.ShowFormModal(new AspectEditor(aspect.Chart, null));
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

