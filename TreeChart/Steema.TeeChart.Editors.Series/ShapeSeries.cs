namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ShapeSeries : Form
    {
        private Button bShapeBrush;
        private ButtonColor bShapeColor;
        private Button bShapePen;
        private Button button1;
        private ComboBox cbStyle;
        private CheckBox checkRound;
        private CheckBox checkTrans;
        private ComboBox comboUnits;
        private Container components;
        private Label label1;
        private Label lblBottom;
        private Label lblLeft;
        private Label lblRight;
        private Label lblTop;
        private Label lblUnits;
        private RadioButton rbHCenter;
        private RadioButton rbHLeft;
        private RadioButton rbHRight;
        private RadioButton rbVBottom;
        private RadioButton rbVCenter;
        private RadioButton rbVTop;
        private GroupBox rgHAlignment;
        private GroupBox rgVertAlign;
        private Steema.TeeChart.Styles.Shape shape;
        private TextEditor shapeText;
        private TabControl tabControl1;
        private TabPage tabPosition;
        private TabPage tabStyle;
        private TabPage tabText;
        private TextBox textShape;
        private TextBox textX0;
        private TextBox textX1;
        private TextBox textY0;
        private TextBox textY1;

        public ShapeSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ShapeSeries(Series s) : this(s, null)
        {
        }

        public ShapeSeries(Series s, Control parent) : this()
        {
            this.shape = (Steema.TeeChart.Styles.Shape) s;
            EditorUtils.InsertForm(this, parent);
        }

        private void bShapeBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.shape.Brush);
        }

        private void bShapeColor_Click(object sender, EventArgs e)
        {
            this.shape.Color = this.bShapeColor.Color;
        }

        private void bShapePen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.shape.Pen);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.shape.Format.Gradient);
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbStyle.SelectedIndex)
            {
                case 0:
                    this.shape.Style = ShapeStyles.Rectangle;
                    break;

                case 1:
                    this.shape.Style = ShapeStyles.Circle;
                    break;

                case 2:
                    this.shape.Style = ShapeStyles.VertLine;
                    break;

                case 3:
                    this.shape.Style = ShapeStyles.HorizLine;
                    break;

                case 4:
                    this.shape.Style = ShapeStyles.Triangle;
                    break;

                case 5:
                    this.shape.Style = ShapeStyles.InvertTriangle;
                    break;

                case 6:
                    this.shape.Style = ShapeStyles.Line;
                    break;

                case 7:
                    this.shape.Style = ShapeStyles.Diamond;
                    break;

                case 8:
                    this.shape.Style = ShapeStyles.Cube;
                    break;

                case 9:
                    this.shape.Style = ShapeStyles.Cross;
                    break;

                case 10:
                    this.shape.Style = ShapeStyles.DiagCross;
                    break;

                case 11:
                    this.shape.Style = ShapeStyles.Star;
                    break;

                case 12:
                    this.shape.Style = ShapeStyles.Pyramid;
                    break;

                case 13:
                    this.shape.Style = ShapeStyles.InvertPyramid;
                    break;
            }
            this.EnableRound();
            this.checkRound.Enabled = this.shape.Style == ShapeStyles.Rectangle;
        }

        private void checkRound_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkRound.Checked)
            {
                this.shape.Format.ShapeStyle = TextShapeStyle.RoundRectangle;
            }
            else
            {
                this.shape.Format.ShapeStyle = TextShapeStyle.Rectangle;
            }
        }

        private void checkTrans_CheckedChanged(object sender, EventArgs e)
        {
            this.shape.Format.Transparent = this.checkTrans.Checked;
        }

        private void comboUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.shape.XYStyle = (ShapeXYStyles) this.comboUnits.SelectedIndex;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableRound()
        {
            this.checkRound.Enabled = (this.shape.Style == ShapeStyles.Rectangle) && !this.shape.Chart.Aspect.View3D;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabStyle = new TabPage();
            this.label1 = new Label();
            this.textShape = new TextBox();
            this.rgHAlignment = new GroupBox();
            this.rbHRight = new RadioButton();
            this.rbHCenter = new RadioButton();
            this.rbHLeft = new RadioButton();
            this.rgVertAlign = new GroupBox();
            this.rbVBottom = new RadioButton();
            this.rbVCenter = new RadioButton();
            this.rbVTop = new RadioButton();
            this.checkTrans = new CheckBox();
            this.checkRound = new CheckBox();
            this.button1 = new Button();
            this.bShapeBrush = new Button();
            this.bShapePen = new Button();
            this.bShapeColor = new ButtonColor();
            this.cbStyle = new ComboBox();
            this.tabPosition = new TabPage();
            this.lblUnits = new Label();
            this.lblBottom = new Label();
            this.lblRight = new Label();
            this.lblLeft = new Label();
            this.lblTop = new Label();
            this.textY1 = new TextBox();
            this.textX0 = new TextBox();
            this.textY0 = new TextBox();
            this.textX1 = new TextBox();
            this.comboUnits = new ComboBox();
            this.tabText = new TabPage();
            this.tabControl1.SuspendLayout();
            this.tabStyle.SuspendLayout();
            this.rgHAlignment.SuspendLayout();
            this.rgVertAlign.SuspendLayout();
            this.tabPosition.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.AddRange(new Control[] { this.tabStyle, this.tabPosition, this.tabText });
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(380, 0xd3);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabStyle.Controls.AddRange(new Control[] { this.label1, this.textShape, this.rgHAlignment, this.rgVertAlign, this.checkTrans, this.checkRound, this.button1, this.bShapeBrush, this.bShapePen, this.bShapeColor, this.cbStyle });
            this.tabStyle.Location = new Point(4, 0x16);
            this.tabStyle.Name = "tabStyle";
            this.tabStyle.Size = new Size(0x174, 0xb9);
            this.tabStyle.TabIndex = 0;
            this.tabStyle.Text = "Style";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x2f, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sty&le:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.textShape.BorderStyle = BorderStyle.FixedSingle;
            this.textShape.Location = new Point(0x57, 0x24);
            this.textShape.Multiline = true;
            this.textShape.Name = "textShape";
            this.textShape.Size = new Size(0x99, 0x5e);
            this.textShape.TabIndex = 6;
            this.textShape.Text = "";
            this.textShape.TextChanged += new EventHandler(this.textShape_TextChanged);
            this.rgHAlignment.Controls.AddRange(new Control[] { this.rbHRight, this.rbHCenter, this.rbHLeft });
            this.rgHAlignment.FlatStyle = FlatStyle.Flat;
            this.rgHAlignment.Location = new Point(0x57, 0x87);
            this.rgHAlignment.Name = "rgHAlignment";
            this.rgHAlignment.Size = new Size(0x111, 0x25);
            this.rgHAlignment.TabIndex = 10;
            this.rgHAlignment.TabStop = false;
            this.rgHAlignment.Text = "Horiz. Alignment";
            this.rbHRight.FlatStyle = FlatStyle.Flat;
            this.rbHRight.Location = new Point(0xbc, 13);
            this.rbHRight.Name = "rbHRight";
            this.rbHRight.Size = new Size(0x48, 0x15);
            this.rbHRight.TabIndex = 2;
            this.rbHRight.Text = "&Right";
            this.rbHRight.CheckedChanged += new EventHandler(this.rbHRight_CheckedChanged);
            this.rbHCenter.FlatStyle = FlatStyle.Flat;
            this.rbHCenter.Location = new Point(0x66, 13);
            this.rbHCenter.Name = "rbHCenter";
            this.rbHCenter.Size = new Size(0x47, 0x15);
            this.rbHCenter.TabIndex = 1;
            this.rbHCenter.Text = "Cen&ter";
            this.rbHCenter.CheckedChanged += new EventHandler(this.rbHCenter_CheckedChanged);
            this.rbHLeft.FlatStyle = FlatStyle.Flat;
            this.rbHLeft.Location = new Point(7, 13);
            this.rbHLeft.Name = "rbHLeft";
            this.rbHLeft.Size = new Size(80, 0x15);
            this.rbHLeft.TabIndex = 0;
            this.rbHLeft.Text = "&Left";
            this.rbHLeft.CheckedChanged += new EventHandler(this.rbHLeft_CheckedChanged);
            this.rgVertAlign.Controls.AddRange(new Control[] { this.rbVBottom, this.rbVCenter, this.rbVTop });
            this.rgVertAlign.FlatStyle = FlatStyle.Flat;
            this.rgVertAlign.Location = new Point(0x100, 0x33);
            this.rgVertAlign.Name = "rgVertAlign";
            this.rgVertAlign.Size = new Size(0x68, 0x55);
            this.rgVertAlign.TabIndex = 9;
            this.rgVertAlign.TabStop = false;
            this.rgVertAlign.Text = "Alignment";
            this.rbVBottom.FlatStyle = FlatStyle.Flat;
            this.rbVBottom.Location = new Point(7, 60);
            this.rbVBottom.Name = "rbVBottom";
            this.rbVBottom.Size = new Size(0x4a, 0x15);
            this.rbVBottom.TabIndex = 2;
            this.rbVBottom.Text = "Botto&m";
            this.rbVBottom.CheckedChanged += new EventHandler(this.rbVBottom_CheckedChanged);
            this.rbVCenter.FlatStyle = FlatStyle.Flat;
            this.rbVCenter.Location = new Point(7, 0x27);
            this.rbVCenter.Name = "rbVCenter";
            this.rbVCenter.Size = new Size(0x4a, 0x13);
            this.rbVCenter.TabIndex = 1;
            this.rbVCenter.Text = "Ce&nter";
            this.rbVCenter.CheckedChanged += new EventHandler(this.rbVCenter_CheckedChanged);
            this.rbVTop.FlatStyle = FlatStyle.Flat;
            this.rbVTop.Location = new Point(7, 0x11);
            this.rbVTop.Name = "rbVTop";
            this.rbVTop.Size = new Size(0x4a, 0x12);
            this.rbVTop.TabIndex = 0;
            this.rbVTop.Text = "&Top";
            this.rbVTop.CheckedChanged += new EventHandler(this.rbVTop_CheckedChanged);
            this.checkTrans.FlatStyle = FlatStyle.Flat;
            this.checkTrans.Location = new Point(0xf3, 30);
            this.checkTrans.Name = "checkTrans";
            this.checkTrans.Size = new Size(0x7d, 0x15);
            this.checkTrans.TabIndex = 8;
            this.checkTrans.Text = "Tran&sparent";
            this.checkTrans.CheckedChanged += new EventHandler(this.checkTrans_CheckedChanged);
            this.checkRound.FlatStyle = FlatStyle.Flat;
            this.checkRound.Location = new Point(0xf3, 7);
            this.checkRound.Name = "checkRound";
            this.checkRound.Size = new Size(0x7d, 0x15);
            this.checkRound.TabIndex = 7;
            this.checkRound.Text = "&Round Rectangle";
            this.checkRound.CheckedChanged += new EventHandler(this.checkRound_CheckedChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(7, 0x94);
            this.button1.Name = "button1";
            this.button1.TabIndex = 5;
            this.button1.Text = "&Gradient";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.bShapeBrush.FlatStyle = FlatStyle.Flat;
            this.bShapeBrush.Location = new Point(7, 0x6c);
            this.bShapeBrush.Name = "bShapeBrush";
            this.bShapeBrush.TabIndex = 4;
            this.bShapeBrush.Text = "&Pattern";
            this.bShapeBrush.Click += new EventHandler(this.bShapeBrush_Click);
            this.bShapePen.FlatStyle = FlatStyle.Flat;
            this.bShapePen.Location = new Point(7, 0x45);
            this.bShapePen.Name = "bShapePen";
            this.bShapePen.TabIndex = 3;
            this.bShapePen.Text = "Bor&der";
            this.bShapePen.Click += new EventHandler(this.bShapePen_Click);
            this.bShapeColor.Color = Color.Empty;
            this.bShapeColor.Location = new Point(7, 0x1f);
            this.bShapeColor.Name = "bShapeColor";
            this.bShapeColor.TabIndex = 2;
            this.bShapeColor.Text = "&Color";
            this.bShapeColor.Click += new EventHandler(this.bShapeColor_Click);
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.AddRange(new object[] { "Rectangle", "Circle", "Vertical Line", "Horiz. Line", "Triangle", "Invert. Triangle", "Line", "Diamond", "Cube", "Cross", "Diagonal Cross", "Star", "Pyramid", "Invert. Pyramid" });
            this.cbStyle.Location = new Point(0x57, 8);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x99, 0x15);
            this.cbStyle.TabIndex = 1;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.tabPosition.Controls.AddRange(new Control[] { this.lblUnits, this.lblBottom, this.lblRight, this.lblLeft, this.lblTop, this.textY1, this.textX0, this.textY0, this.textX1, this.comboUnits });
            this.tabPosition.Location = new Point(4, 0x16);
            this.tabPosition.Name = "tabPosition";
            this.tabPosition.Size = new Size(0x174, 0xb9);
            this.tabPosition.TabIndex = 1;
            this.tabPosition.Text = "Position";
            this.lblUnits.AutoSize = true;
            this.lblUnits.Location = new Point(0x49, 0x10);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new Size(0x21, 13);
            this.lblUnits.TabIndex = 0;
            this.lblUnits.Text = "&Units:";
            this.lblUnits.TextAlign = ContentAlignment.TopRight;
            this.lblBottom.AutoSize = true;
            this.lblBottom.Location = new Point(0x65, 0x8d);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new Size(0x2b, 13);
            this.lblBottom.TabIndex = 8;
            this.lblBottom.Text = "&Bottom:";
            this.lblBottom.TextAlign = ContentAlignment.TopRight;
            this.lblRight.AutoSize = true;
            this.lblRight.Location = new Point(0xbc, 0x63);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new Size(0x22, 13);
            this.lblRight.TabIndex = 6;
            this.lblRight.Text = "&Right:";
            this.lblRight.TextAlign = ContentAlignment.TopRight;
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new Point(40, 0x63);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new Size(0x1a, 13);
            this.lblLeft.TabIndex = 4;
            this.lblLeft.Text = "&Left:";
            this.lblLeft.TextAlign = ContentAlignment.TopRight;
            this.lblTop.AutoSize = true;
            this.lblTop.Location = new Point(0x71, 0x39);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new Size(0x1b, 13);
            this.lblTop.TabIndex = 2;
            this.lblTop.Text = "&Top:";
            this.lblTop.TextAlign = ContentAlignment.TopRight;
            this.textY1.BorderStyle = BorderStyle.FixedSingle;
            this.textY1.Location = new Point(150, 0x8b);
            this.textY1.Name = "textY1";
            this.textY1.Size = new Size(0x49, 20);
            this.textY1.TabIndex = 9;
            this.textY1.Text = "";
            this.textY1.TextChanged += new EventHandler(this.textY1_TextChanged);
            this.textX0.BorderStyle = BorderStyle.FixedSingle;
            this.textX0.Location = new Point(70, 0x61);
            this.textX0.Name = "textX0";
            this.textX0.Size = new Size(0x49, 20);
            this.textX0.TabIndex = 5;
            this.textX0.Text = "";
            this.textX0.TextChanged += new EventHandler(this.textX0_TextChanged);
            this.textY0.BorderStyle = BorderStyle.FixedSingle;
            this.textY0.Location = new Point(150, 0x37);
            this.textY0.Name = "textY0";
            this.textY0.Size = new Size(0x49, 20);
            this.textY0.TabIndex = 3;
            this.textY0.Text = "";
            this.textY0.TextChanged += new EventHandler(this.textY0_TextChanged);
            this.textX1.BorderStyle = BorderStyle.FixedSingle;
            this.textX1.Location = new Point(0xe3, 0x61);
            this.textX1.Name = "textX1";
            this.textX1.Size = new Size(0x49, 20);
            this.textX1.TabIndex = 7;
            this.textX1.Text = "";
            this.textX1.TextChanged += new EventHandler(this.textX1_TextChanged);
            this.comboUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboUnits.Items.AddRange(new object[] { "Pixels", "Axis", "Axis Origin" });
            this.comboUnits.Location = new Point(0x71, 14);
            this.comboUnits.Name = "comboUnits";
            this.comboUnits.Size = new Size(0x93, 0x15);
            this.comboUnits.TabIndex = 1;
            this.comboUnits.SelectedIndexChanged += new EventHandler(this.comboUnits_SelectedIndexChanged);
            this.tabText.Location = new Point(4, 0x16);
            this.tabText.Name = "tabText";
            this.tabText.Size = new Size(0x174, 0xb9);
            this.tabText.TabIndex = 2;
            this.tabText.Text = "Text";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(380, 0xd3);
            base.Controls.AddRange(new Control[] { this.tabControl1 });
            base.Name = "ShapeSeries";
            base.Load += new EventHandler(this.ShapeSeries_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabStyle.ResumeLayout(false);
            this.rgHAlignment.ResumeLayout(false);
            this.rgVertAlign.ResumeLayout(false);
            this.tabPosition.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void rbHCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbHCenter.Checked)
            {
                this.shape.HorizAlignment = ShapeTextHorizAlign.Center;
            }
        }

        private void rbHLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbHLeft.Checked)
            {
                this.shape.HorizAlignment = ShapeTextHorizAlign.Left;
            }
        }

        private void rbHRight_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbHRight.Checked)
            {
                this.shape.HorizAlignment = ShapeTextHorizAlign.Right;
            }
        }

        private void rbVBottom_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbVBottom.Checked)
            {
                this.shape.VertAlignment = ShapeTextVertAlign.Bottom;
            }
        }

        private void rbVCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbVCenter.Checked)
            {
                this.shape.VertAlignment = ShapeTextVertAlign.Center;
            }
        }

        private void rbVTop_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbVTop.Checked)
            {
                this.shape.VertAlignment = ShapeTextVertAlign.Top;
            }
        }

        private void ShapeSeries_Load(object sender, EventArgs e)
        {
            if (this.shape != null)
            {
                this.cbStyle.SelectedIndex = (int) this.shape.Style;
                this.checkTrans.Checked = this.shape.Transparent;
                if (this.shape.Count == 2)
                {
                    this.textX0.Text = this.shape.X0.ToString();
                    this.textY0.Text = this.shape.Y0.ToString();
                    this.textX1.Text = this.shape.X1.ToString();
                    this.textY1.Text = this.shape.Y1.ToString();
                }
                this.textShape.Font = new Font(this.shape.Font.Name, (float) this.shape.Font.Size);
                this.textShape.Lines = this.shape.Text;
                switch (this.shape.HorizAlignment)
                {
                    case ShapeTextHorizAlign.Left:
                        this.rbHLeft.Checked = true;
                        break;

                    case ShapeTextHorizAlign.Center:
                        this.rbHCenter.Checked = true;
                        break;

                    case ShapeTextHorizAlign.Right:
                        this.rbHRight.Checked = true;
                        break;
                }
                switch (this.shape.VertAlignment)
                {
                    case ShapeTextVertAlign.Top:
                        this.rbVTop.Checked = true;
                        break;

                    case ShapeTextVertAlign.Center:
                        this.rbVCenter.Checked = true;
                        break;

                    case ShapeTextVertAlign.Bottom:
                        this.rbVBottom.Checked = true;
                        break;
                }
                this.checkRound.Checked = this.shape.Format.ShapeStyle == TextShapeStyle.RoundRectangle;
                this.EnableRound();
                this.comboUnits.SelectedIndex = (int) this.shape.XYStyle;
                this.bShapeColor.Color = this.shape.Color;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.tabControl1.SelectedTab == this.tabText) && (this.shapeText == null))
            {
                this.shapeText = new TextEditor(this.shape.Format.Font, this.tabText);
            }
            EditorUtils.Translate(this.shapeText);
        }

        private void textShape_TextChanged(object sender, EventArgs e)
        {
            this.shape.Text = this.textShape.Lines;
        }

        private void textX0_TextChanged(object sender, EventArgs e)
        {
            this.shape.X0 = Utils.StringToDouble(this.textX0.Text, this.shape.X0);
        }

        private void textX1_TextChanged(object sender, EventArgs e)
        {
            this.shape.X1 = Utils.StringToDouble(this.textX1.Text, this.shape.X1);
        }

        private void textY0_TextChanged(object sender, EventArgs e)
        {
            this.shape.Y0 = Utils.StringToDouble(this.textY0.Text, this.shape.Y0);
        }

        private void textY1_TextChanged(object sender, EventArgs e)
        {
            this.shape.Y1 = Utils.StringToDouble(this.textY1.Text, this.shape.Y1);
        }
    }
}

