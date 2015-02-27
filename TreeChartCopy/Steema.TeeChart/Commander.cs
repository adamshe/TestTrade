namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Commander), "Images.Commander.bmp")]
    public class Commander : ToolBar, ITeeEventListener
    {
        private ToolBarButton bCopy = new ToolBarButton();
        private ToolBarButton bDepth = new ToolBarButton();
        private ToolBarButton bEdit = new ToolBarButton();
        private ToolBarButton bExport = new ToolBarButton();
        private ToolBarButton bMove = new ToolBarButton();
        private ToolBarButton bNormal = new ToolBarButton();
        private ToolBarButton bPrint = new ToolBarButton();
        private ToolBarButton bRotate = new ToolBarButton();
        private ToolBarButton bSeparator = new ToolBarButton();
        private ToolBarButton bView3D = new ToolBarButton();
        private ToolBarButton bZoom = new ToolBarButton();
        private TChart chart;
        private IContainer components;
        private bool dragging;
        private int draggingIndex;
        private ImageList imageList1;
        private string label;
        private bool labelValues;
        private MouseButtons mouseButtons;
        private int oldX;
        private int oldY;
        private TChart panel;
        private ToolBarButton[] theButtons;

        public event SetLabelEventHandler SetLabel;

        public Commander()
        {
            if (!base.DesignMode)
            {
                DesignTimeOptions.InitLanguage(false, false);
            }
            this.AddToolBarButtons(this.CreateStandardButtons());
            this.InitializeComponent();
            base.ButtonClick += new ToolBarButtonClickEventHandler(this.toolbar_ButtonClick);
        }

        private void AddToolBarButtons(ToolBarButton[] tButtons)
        {
            this.theButtons = tButtons;
            base.Buttons.AddRange(tButtons);
        }

        private bool ButtonNormalUp()
        {
            if (this.bNormal == null)
            {
                return !this.bNormal.Pushed;
            }
            return true;
        }

        private int CalcAngleChange(int aAngle, int aChange)
        {
            int num;
            if (aChange > 0)
            {
                return Math.Min(360, aAngle + aChange);
            }
            if (this.Panel.Graphics3D.SupportsFullRotation)
            {
                num = 0;
            }
            else
            {
                num = 270;
            }
            return Math.Max(num, aAngle + aChange);
        }

        private int CalcDistPercent(int aPercent, int aWidth, int aHeight, int X, int Y)
        {
            int num = (int) Math.Round(Math.Sqrt(Utils.Sqr((1.0 * this.oldX) - X) + Utils.Sqr((1.0 * this.oldY) - Y)));
            int num2 = (int) Math.Round(Math.Sqrt(Utils.Sqr(1.0 * aWidth) + Utils.Sqr(1.0 * aHeight)));
            return (int) Math.Round((double) (((1.0 * aPercent) * num) / ((double) num2)));
        }

        private int CorrectAngle(int aAngle)
        {
            int num = aAngle;
            if (num > 360)
            {
                return (num - 360);
            }
            if (num < 0)
            {
                num = 360 + num;
            }
            return num;
        }

        private ToolBarButton[] CreateStandardButtons()
        {
            this.bNormal.ImageIndex = 0;
            this.bNormal.Pushed = true;
            this.bNormal.ToolTipText = Texts.CommanMsgNormal;
            this.bNormal.Style = ToolBarButtonStyle.ToggleButton;
            this.bNormal.Tag = "bNormal";
            this.bSeparator.Style = ToolBarButtonStyle.Separator;
            this.bSeparator.Tag = "bSeparator";
            this.bRotate.ImageIndex = 1;
            this.bRotate.ToolTipText = Texts.CommanMsgRotate;
            this.bRotate.Style = ToolBarButtonStyle.ToggleButton;
            this.bRotate.Tag = "bRotate";
            this.bMove.ImageIndex = 2;
            this.bMove.ToolTipText = Texts.CommanMsgMove;
            this.bMove.Style = ToolBarButtonStyle.ToggleButton;
            this.bMove.Tag = "bMove";
            this.bZoom.ImageIndex = 3;
            this.bZoom.ToolTipText = Texts.CommanMsgZoom;
            this.bZoom.Style = ToolBarButtonStyle.ToggleButton;
            this.bZoom.Tag = "bZoom";
            this.bDepth.ImageIndex = 4;
            this.bDepth.ToolTipText = Texts.CommanMsgDepth;
            this.bDepth.Style = ToolBarButtonStyle.ToggleButton;
            this.bDepth.Tag = "bDepth";
            this.bView3D.ImageIndex = 5;
            this.bView3D.ToolTipText = Texts.CommanMsg3D;
            this.bView3D.Style = ToolBarButtonStyle.ToggleButton;
            this.bView3D.Tag = "bView3D";
            this.bEdit.ImageIndex = 6;
            this.bEdit.ToolTipText = Texts.CommanMsgEdit;
            this.bEdit.Style = ToolBarButtonStyle.PushButton;
            this.bEdit.Tag = "bEdit";
            this.bPrint.ImageIndex = 7;
            this.bPrint.ToolTipText = Texts.CommanMsgPrint;
            this.bPrint.Style = ToolBarButtonStyle.PushButton;
            this.bPrint.Tag = "bPrint";
            this.bCopy.ImageIndex = 8;
            this.bCopy.ToolTipText = Texts.CommanMsgCopy;
            this.bCopy.Style = ToolBarButtonStyle.PushButton;
            this.bCopy.Tag = "bCopy";
            this.bExport.ImageIndex = 9;
            this.bExport.ToolTipText = Texts.CommanMsgSave;
            this.bExport.Style = ToolBarButtonStyle.PushButton;
            this.bExport.Tag = "bExport";
            return new ToolBarButton[] { this.bNormal, this.bSeparator, this.bRotate, this.bMove, this.bZoom, this.bDepth, this.bView3D, this.bSeparator, this.bEdit, this.bPrint, this.bCopy, this.bExport };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoDepth(int X, int Y)
        {
            this.Set3D();
            int num = this.CalcDistPercent(200, this.Panel.Width, this.Panel.Height, X, Y);
            if (num >= 1)
            {
                this.Panel.Aspect.Chart3DPercent = Math.Min(100, num);
            }
        }

        private void DoMouseDown(int X, int Y)
        {
            Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
            if ((pie != null) || this.ButtonNormalUp())
            {
                this.dragging = true;
                this.oldX = X;
                this.oldY = Y;
                if (pie != null)
                {
                    this.draggingIndex = pie.Clicked(X, Y);
                }
                else
                {
                    this.draggingIndex = -1;
                }
                if (this.Panel != null)
                {
                    this.Panel.Chart.CancelMouse = (this.draggingIndex != -1) || this.ButtonNormalUp();
                }
            }
        }

        private void DoMouseMove(int X, int Y)
        {
            if (this.dragging)
            {
                this.dragging = false;
                if (this.IsButtonDown(this.bNormal))
                {
                    this.DoNormal(X, Y);
                }
                else if (this.mouseButtons == MouseButtons.Right)
                {
                    this.DoMove(X, Y);
                }
                else if (this.IsButtonDown(this.bRotate))
                {
                    this.DoRotate(X, Y);
                }
                else if (this.IsButtonDown(this.bMove))
                {
                    this.DoMove(X, Y);
                }
                else if (this.IsButtonDown(this.bZoom))
                {
                    this.DoZoom(X, Y);
                }
                else if (this.IsButtonDown(this.bDepth))
                {
                    this.DoDepth(X, Y);
                }
                if (this.labelValues)
                {
                    this.ShowValues();
                }
                this.dragging = true;
            }
        }

        private void DoMouseUp()
        {
            this.dragging = false;
            this.draggingIndex = -1;
        }

        private void DoMove(int X, int Y)
        {
            this.Set3D();
            this.Panel.Aspect.HorizOffset += X - this.oldX;
            this.Panel.Aspect.VertOffset += Y - this.oldY;
            this.oldX = X;
            this.oldY = Y;
        }

        private void DoNormal(int X, int Y)
        {
            if (this.draggingIndex != -1)
            {
                Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
                if (pie != null)
                {
                    int num = Math.Min(100, this.CalcDistPercent(100, pie.CircleWidth, pie.CircleHeight, X, Y));
                    pie.ExplodedSlice[this.draggingIndex] = num;
                    pie.Invalidate();
                }
            }
        }

        private bool DoPanelMouse()
        {
            return (((this.IsButtonUp(this.bRotate) && this.IsButtonUp(this.bDepth)) && this.IsButtonUp(this.bMove)) && this.IsButtonUp(this.bZoom));
        }

        private void DoRotate(int X, int Y)
        {
            this.Set3D();
            this.Panel.Aspect.Orthogonal = false;
            int aChange = (int) Math.Round((double) ((90.0 * (X - this.oldX)) / ((double) this.Panel.Width)));
            int num2 = (int) Math.Round((double) ((90.0 * (this.oldY - Y)) / ((double) this.Panel.Height)));
            Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
            if (this.Panel.Graphics3D.SupportsFullRotation)
            {
                this.Panel.Aspect.Rotation = this.CorrectAngle(this.Panel.Aspect.Rotation + aChange);
                this.Panel.Aspect.Elevation = this.CorrectAngle(this.Panel.Aspect.Elevation + num2);
            }
            else
            {
                if (pie != null)
                {
                    this.Panel.Aspect.Rotation = 360;
                    if (!this.Panel.Graphics3D.SupportsFullRotation)
                    {
                        this.Panel.Aspect.Perspective = 0;
                    }
                    if (aChange != 0)
                    {
                        pie.RotationAngle = this.CorrectAngle(pie.RotationAngle + aChange);
                    }
                }
                else
                {
                    this.Panel.Aspect.Rotation = this.CalcAngleChange(this.Panel.Aspect.Rotation, aChange);
                }
                this.Panel.Aspect.Elevation = this.CalcAngleChange(this.Panel.Aspect.Elevation, num2);
            }
            this.oldX = X;
            this.oldY = Y;
        }

        private void DoZoom(int X, int Y)
        {
            this.Set3D();
            int num = (int) Math.Round((double) ((10.0 * (this.oldY - Y)) / Math.Sqrt(Utils.Sqr((double) this.Panel.Width) + Utils.Sqr((double) this.Panel.Height))));
            int num2 = (int) Math.Round((double) (((double) (num * this.Panel.Aspect.Zoom)) / 100.0));
            if (num > 0)
            {
                this.Panel.Aspect.Zoom += Math.Max(1, num2);
            }
            else
            {
                this.Panel.Aspect.Zoom += Math.Min(-1, num2);
            }
            if (this.Panel.Aspect.Zoom < 5)
            {
                this.Panel.Aspect.Zoom = 5;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(Commander));
            this.imageList1 = new ImageList(this.components);
            this.imageList1.ImageSize = new Size(0x19, 0x19);
            this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Lime;
            base.Appearance = ToolBarAppearance.Flat;
            base.ImageList = this.imageList1;
        }

        private bool IsButtonDown(ToolBarButton aButton)
        {
            return ((aButton != null) && aButton.Pushed);
        }

        private bool IsButtonUp(ToolBarButton aButton)
        {
            if (aButton != null)
            {
                return !aButton.Pushed;
            }
            return true;
        }

        protected internal void OnSetLabel(object sender, string statusText)
        {
            if (this.SetLabel != null)
            {
                SetLabelEventArgs e = new SetLabelEventArgs(statusText);
                this.SetLabel(sender, e);
            }
        }

        private void Set3D()
        {
            this.Panel.Aspect.View3D = true;
            if (this.bView3D != null)
            {
                this.bView3D.Pushed = true;
            }
        }

        private void SetLabelCaption(string aCaption)
        {
            string statusText = aCaption;
            if (this.labelValues)
            {
                this.label = statusText;
                this.OnSetLabel(this, statusText);
            }
        }

        private void SetPanel(TChart Value)
        {
            if (this.panel != null)
            {
                this.panel.Chart.RemoveListener(this);
            }
            this.panel = Value;
            if ((this.panel != null) && (this.panel.Chart.Listeners != null))
            {
                this.panel.Chart.Listeners.Add(this);
            }
            this.ShowHideControls(this.panel != null);
        }

        private void ShowHideControls(bool value)
        {
            this.bRotate.Enabled = value;
            this.bMove.Enabled = value;
            this.bZoom.Enabled = value;
            this.bNormal.Enabled = value;
            this.bCopy.Enabled = value;
            this.bExport.Enabled = value;
            this.bPrint.Enabled = value;
            this.bView3D.Enabled = value;
            if ((this.bView3D != null) && (this.panel != null))
            {
                this.bView3D.Pushed = this.panel.Aspect.View3D;
            }
        }

        private void ShowValues()
        {
            string aCaption = "";
            if (this.IsButtonDown(this.bRotate))
            {
                aCaption = string.Format(Texts.CommanMsgRotating, this.Panel.Aspect.Rotation, this.Panel.Aspect.Elevation);
            }
            else if (this.IsButtonDown(this.bMove))
            {
                aCaption = string.Format(Texts.CommanMsgMoving, this.Panel.Aspect.HorizOffset, this.Panel.Aspect.VertOffset);
            }
            else if (this.IsButtonDown(this.bZoom))
            {
                aCaption = string.Format(Texts.CommanMsgZooming, this.Panel.Aspect.Zoom);
            }
            else if (this.IsButtonDown(this.bDepth))
            {
                aCaption = string.Format(Texts.CommanMsgDepthing, this.Panel.Aspect.Chart3DPercent);
            }
            else if (this.IsButtonDown(this.bNormal) && (this.draggingIndex != -1))
            {
                Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
                if (pie == null)
                {
                    aCaption = "";
                }
                else
                {
                    aCaption = string.Format(Texts.CommanMsgPieExploding, this.draggingIndex, pie.ExplodedSlice[this.draggingIndex]);
                }
            }
            else if (this.label != null)
            {
                aCaption = "";
            }
            this.SetLabelCaption(aCaption);
        }

        public void TeeEvent(Steema.TeeChart.TeeEvent e)
        {
            if (!(e is TeeMouseEvent))
            {
                if (e is View3DEvent)
                {
                    this.ShowHideControls(this.Panel != null);
                }
            }
            else
            {
                TeeMouseEvent event2 = (TeeMouseEvent) e;
                switch (event2.Event)
                {
                    case MouseEventKinds.Down:
                        this.mouseButtons = event2.Button;
                        this.DoMouseDown(event2.mArgs.X, event2.mArgs.Y);
                        break;

                    case MouseEventKinds.Move:
                        this.DoMouseMove(event2.mArgs.X, event2.mArgs.Y);
                        break;

                    case MouseEventKinds.Up:
                        this.DoMouseUp();
                        break;
                }
                event2.sender.CancelMouse = !this.DoPanelMouse();
            }
        }

        private void toolbar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button != this.bView3D)
            {
                foreach (ToolBarButton button in base.Buttons)
                {
                    if (((button.Style == ToolBarButtonStyle.ToggleButton) && (base.Buttons.IndexOf(button) != base.Buttons.IndexOf(e.Button))) && ((base.Buttons.IndexOf(button) != base.Buttons.IndexOf(this.bView3D)) && e.Button.Pushed))
                    {
                        button.Pushed = false;
                    }
                }
            }
            if (this.Panel != null)
            {
                if (e.Button == this.bEdit)
                {
                    this.Panel.ShowEditor();
                }
                else if (e.Button == this.bView3D)
                {
                    if (this.bView3D.Pushed)
                    {
                        this.Panel.Aspect.View3D = true;
                    }
                    else
                    {
                        this.Panel.Aspect.View3D = false;
                    }
                }
                else if (e.Button == this.bCopy)
                {
                    this.Panel.Export.Image.Bitmap.CopyToClipboard();
                }
                else if (e.Button == this.bPrint)
                {
                    this.Panel.Printer.Preview();
                }
                else if (e.Button == this.bExport)
                {
                    ExportEditor.ShowModal(this.Panel.Chart);
                }
            }
        }

        [Browsable(false)]
        public ToolBarButton[] Buttons
        {
            get
            {
                return this.theButtons;
            }
            set
            {
                this.theButtons = value;
                base.Buttons.Clear();
                base.Buttons.AddRange(this.theButtons);
            }
        }

        [DefaultValue((string) null)]
        public TChart Chart
        {
            get
            {
                return this.chart;
            }
            set
            {
                this.chart = value;
                this.Panel = this.chart;
            }
        }

        [Description("Accesses the LabelText for the TeeCommander navigator bar."), DefaultValue((string) null)]
        public string LabelText
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        [DefaultValue(false), Description("Enables/disables the display of TeeCommander's advisory status messages.")]
        public bool LabelValues
        {
            get
            {
                return this.labelValues;
            }
            set
            {
                this.labelValues = value;
            }
        }

        private TChart Panel
        {
            get
            {
                return this.panel;
            }
            set
            {
                this.SetPanel(value);
            }
        }

        public class SetLabelEventArgs : EventArgs
        {
            private string statusMsg;

            public SetLabelEventArgs(string statusText)
            {
                this.statusMsg = statusText;
            }

            public string StatusMsg
            {
                get
                {
                    return this.statusMsg;
                }
            }
        }

        public delegate void SetLabelEventHandler(object sender, Commander.SetLabelEventArgs e);
    }
}

