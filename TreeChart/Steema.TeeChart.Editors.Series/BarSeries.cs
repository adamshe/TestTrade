namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BarSeries : Form
    {
        internal bool addStackEditor;
        private Button BBarBrush;
        private ButtonColor BBarColor;
        private ButtonPen BBarPen;
        private Button BGradient;
        private CheckBox CBBarSideMargins;
        private ComboBox CBBarStyle;
        private CheckBox CBColorEach;
        private CheckBox CBDarkBar;
        private CheckBox CBMarksAutoPosition;
        private CheckBox checkBox1;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label LStyle;
        private CustomBar series;
        private StackBarSeries stackEditor;
        private NumericUpDown UDBarOffset;
        private NumericUpDown UDBarWidth;

        public BarSeries()
        {
            this.components = null;
            this.addStackEditor = true;
            this.InitializeComponent();
        }

        public BarSeries(Series s) : this()
        {
            this.series = (CustomBar) s;
        }

        private void BarSeries_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.CBColorEach.Checked = this.series.ColorEach;
                this.CBDarkBar.Checked = this.series.Dark3D;
                this.CBBarSideMargins.Checked = this.series.SideMargins;
                this.UDBarOffset.Value = this.series.OffsetPercent;
                this.UDBarWidth.Value = this.series.barSizePercent;
                this.CBMarksAutoPosition.Checked = this.series.AutoMarkPosition;
                this.checkBox1.Checked = this.series.GradientRelative;
                this.checkBox1.Enabled = this.series.BarStyle == BarStyles.RectGradient;
                this.BBarPen.Pen = this.series.Pen;
                switch (this.series.BarStyle)
                {
                    case BarStyles.Rectangle:
                        this.CBBarStyle.SelectedIndex = 6;
                        break;

                    case BarStyles.Pyramid:
                        this.CBBarStyle.SelectedIndex = 4;
                        break;

                    case BarStyles.InvPyramid:
                        this.CBBarStyle.SelectedIndex = 5;
                        break;

                    case BarStyles.Cylinder:
                        this.CBBarStyle.SelectedIndex = 2;
                        break;

                    case BarStyles.Ellipse:
                        this.CBBarStyle.SelectedIndex = 3;
                        break;

                    case BarStyles.Arrow:
                        this.CBBarStyle.SelectedIndex = 0;
                        break;

                    case BarStyles.RectGradient:
                        this.CBBarStyle.SelectedIndex = 7;
                        break;

                    case BarStyles.Cone:
                        this.CBBarStyle.SelectedIndex = 1;
                        break;
                }
                if (this.addStackEditor && (this.stackEditor == null))
                {
                    TabControl parent = (TabControl) ((TabPage) base.Parent).Parent;
                    TabPage page = new TabPage(Texts.Stack);
                    parent.TabPages.Add(page);
                    this.stackEditor = new StackBarSeries(this.series, page);
                    TabPage page2 = parent.TabPages[1];
                    parent.TabPages[1] = page;
                    parent.TabPages[parent.TabCount - 1] = page2;
                }
            }
        }

        private void BBarBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void BBarColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BBarColor.Color;
            if (this.CBColorEach.Checked)
            {
                this.CBColorEach.Checked = false;
            }
        }

        private void BGradient_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.series.Gradient);
            this.checkBox1.Enabled = this.series.Gradient.Visible;
        }

        private void CBBarSideMargins_CheckedChanged(object sender, EventArgs e)
        {
            this.series.SideMargins = this.CBBarSideMargins.Checked;
        }

        private void CBBarStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBBarStyle.SelectedIndex)
            {
                case 0:
                    this.series.BarStyle = BarStyles.Arrow;
                    break;

                case 1:
                    this.series.BarStyle = BarStyles.Cone;
                    break;

                case 2:
                    this.series.BarStyle = BarStyles.Cylinder;
                    break;

                case 3:
                    this.series.BarStyle = BarStyles.Ellipse;
                    break;

                case 4:
                    this.series.BarStyle = BarStyles.Pyramid;
                    break;

                case 5:
                    this.series.BarStyle = BarStyles.InvPyramid;
                    break;

                case 6:
                    this.series.BarStyle = BarStyles.Rectangle;
                    break;

                case 7:
                    this.series.BarStyle = BarStyles.RectGradient;
                    break;
            }
            this.checkBox1.Enabled = this.series.BarStyle == BarStyles.RectGradient;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBDarkBar_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Dark3D = this.CBDarkBar.Checked;
        }

        private void CBMarksAutoPosition_CheckedChanged(object sender, EventArgs e)
        {
            this.series.AutoMarkPosition = this.CBMarksAutoPosition.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.GradientRelative = this.checkBox1.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.LStyle = new Label();
            this.CBBarStyle = new ComboBox();
            this.BBarPen = new ButtonPen();
            this.BBarBrush = new Button();
            this.BBarColor = new ButtonColor();
            this.groupBox1 = new GroupBox();
            this.CBColorEach = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.CBMarksAutoPosition = new CheckBox();
            this.CBBarSideMargins = new CheckBox();
            this.CBDarkBar = new CheckBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.UDBarWidth = new NumericUpDown();
            this.UDBarOffset = new NumericUpDown();
            this.groupBox3 = new GroupBox();
            this.checkBox1 = new CheckBox();
            this.BGradient = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.UDBarWidth.BeginInit();
            this.UDBarOffset.BeginInit();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.LStyle.AutoSize = true;
            this.LStyle.Location = new Point(0x1a, 9);
            this.LStyle.Name = "LStyle";
            this.LStyle.Size = new Size(0x21, 0x10);
            this.LStyle.TabIndex = 0;
            this.LStyle.Text = "St&yle:";
            this.LStyle.TextAlign = ContentAlignment.TopRight;
            this.CBBarStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBBarStyle.Items.AddRange(new object[] { "Arrow", "Cone", "Cylinder", "Ellipse", "Pyramid", "Inv. Pyramid", "Rectangle", "Rect. Gradient" });
            this.CBBarStyle.Location = new Point(0x3b, 6);
            this.CBBarStyle.Name = "CBBarStyle";
            this.CBBarStyle.Size = new Size(0x61, 0x15);
            this.CBBarStyle.TabIndex = 1;
            this.CBBarStyle.SelectedIndexChanged += new EventHandler(this.CBBarStyle_SelectedIndexChanged);
            this.BBarPen.FlatStyle = FlatStyle.Flat;
            this.BBarPen.Location = new Point(0xa1, 5);
            this.BBarPen.Name = "BBarPen";
            this.BBarPen.TabIndex = 2;
            this.BBarPen.Text = "&Border...";
            this.BBarBrush.FlatStyle = FlatStyle.Flat;
            this.BBarBrush.Location = new Point(0xf1, 5);
            this.BBarBrush.Name = "BBarBrush";
            this.BBarBrush.TabIndex = 3;
            this.BBarBrush.Text = "&Pattern...";
            this.BBarBrush.Click += new EventHandler(this.BBarBrush_Click);
            this.BBarColor.Color = Color.Empty;
            this.BBarColor.Location = new Point(8, 0x20);
            this.BBarColor.Name = "BBarColor";
            this.BBarColor.Size = new Size(0x58, 0x17);
            this.BBarColor.TabIndex = 1;
            this.BBarColor.Text = "&Color...";
            this.BBarColor.Click += new EventHandler(this.BBarColor_Click);
            this.groupBox1.Controls.Add(this.BBarColor);
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.FlatStyle = FlatStyle.Flat;
            this.groupBox1.Location = new Point(0x10, 0x20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 0x3f);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(8, 10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x68, 0x10);
            this.CBColorEach.TabIndex = 0;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.groupBox2.Controls.Add(this.CBMarksAutoPosition);
            this.groupBox2.Controls.Add(this.CBBarSideMargins);
            this.groupBox2.Controls.Add(this.CBDarkBar);
            this.groupBox2.Location = new Point(0x99, 0x60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(160, 0x48);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.CBMarksAutoPosition.FlatStyle = FlatStyle.Flat;
            this.CBMarksAutoPosition.Location = new Point(8, 50);
            this.CBMarksAutoPosition.Name = "CBMarksAutoPosition";
            this.CBMarksAutoPosition.Size = new Size(0x90, 0x10);
            this.CBMarksAutoPosition.TabIndex = 2;
            this.CBMarksAutoPosition.Text = "&Auto Mark Position";
            this.CBMarksAutoPosition.CheckedChanged += new EventHandler(this.CBMarksAutoPosition_CheckedChanged);
            this.CBBarSideMargins.FlatStyle = FlatStyle.Flat;
            this.CBBarSideMargins.Location = new Point(8, 0x1f);
            this.CBBarSideMargins.Name = "CBBarSideMargins";
            this.CBBarSideMargins.Size = new Size(0x90, 0x10);
            this.CBBarSideMargins.TabIndex = 1;
            this.CBBarSideMargins.Text = "Bar S&ide Margins";
            this.CBBarSideMargins.CheckedChanged += new EventHandler(this.CBBarSideMargins_CheckedChanged);
            this.CBDarkBar.FlatStyle = FlatStyle.Flat;
            this.CBDarkBar.Location = new Point(8, 12);
            this.CBDarkBar.Name = "CBDarkBar";
            this.CBDarkBar.Size = new Size(0x90, 0x10);
            this.CBDarkBar.TabIndex = 0;
            this.CBDarkBar.Text = "&Dark Bar 3D Sides";
            this.CBDarkBar.CheckedChanged += new EventHandler(this.CBDarkBar_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xc1, 0x2f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(70, 0x10);
            this.label1.TabIndex = 5;
            this.label1.Text = "% Bar &Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xc1, 0x4a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x47, 0x10);
            this.label2.TabIndex = 7;
            this.label2.Text = "% Bar O&ffset:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDBarWidth.BorderStyle = BorderStyle.FixedSingle;
            this.UDBarWidth.Location = new Point(0x106, 0x2d);
            this.UDBarWidth.Name = "UDBarWidth";
            this.UDBarWidth.Size = new Size(0x33, 20);
            this.UDBarWidth.TabIndex = 6;
            this.UDBarWidth.TextAlign = HorizontalAlignment.Right;
            this.UDBarWidth.TextChanged += new EventHandler(this.UDBarWidth_ValueChanged);
            this.UDBarWidth.ValueChanged += new EventHandler(this.UDBarWidth_ValueChanged);
            this.UDBarOffset.BorderStyle = BorderStyle.FixedSingle;
            this.UDBarOffset.Location = new Point(0x106, 0x48);
            int[] bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.UDBarOffset.Minimum = new decimal(bits);
            this.UDBarOffset.Name = "UDBarOffset";
            this.UDBarOffset.Size = new Size(0x33, 20);
            this.UDBarOffset.TabIndex = 8;
            this.UDBarOffset.TextAlign = HorizontalAlignment.Right;
            this.UDBarOffset.TextChanged += new EventHandler(this.UDBarOffset_ValueChanged);
            this.UDBarOffset.ValueChanged += new EventHandler(this.UDBarOffset_ValueChanged);
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.BGradient);
            this.groupBox3.FlatStyle = FlatStyle.Flat;
            this.groupBox3.Location = new Point(0x10, 0x60);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(120, 0x48);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gradient:";
            this.checkBox1.Enabled = false;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x10, 0x30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x58, 0x10);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "&Relative";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.BGradient.FlatStyle = FlatStyle.Flat;
            this.BGradient.Location = new Point(8, 0x12);
            this.BGradient.Name = "BGradient";
            this.BGradient.Size = new Size(0x58, 0x17);
            this.BGradient.TabIndex = 0;
            this.BGradient.Text = "&Gradient...";
            this.BGradient.Click += new EventHandler(this.BGradient_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(320, 0xad);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.UDBarOffset);
            base.Controls.Add(this.UDBarWidth);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.LStyle);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.BBarBrush);
            base.Controls.Add(this.BBarPen);
            base.Controls.Add(this.CBBarStyle);
            base.Name = "BarSeries";
            base.Load += new EventHandler(this.BarSeries_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.UDBarWidth.EndInit();
            this.UDBarOffset.EndInit();
            this.groupBox3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public static BarSeries InsertEditor(Control parent, Bar s)
        {
            TabControl control = (TabControl) ((TabPage) parent).Parent;
            TabPage page = new TabPage("Bar");
            control.TabPages.Add(page);
            BarSeries f = new BarSeries(s);
            f.addStackEditor = false;
            EditorUtils.InsertForm(f, page);
            TabPage page2 = control.TabPages[1];
            control.TabPages[1] = page;
            control.TabPages[control.TabCount - 1] = page2;
            return f;
        }

        private void UDBarOffset_ValueChanged(object sender, EventArgs e)
        {
            this.series.OffsetPercent = (int) this.UDBarOffset.Value;
        }

        private void UDBarWidth_ValueChanged(object sender, EventArgs e)
        {
            this.series.SetBarSizePercent((int) this.UDBarWidth.Value);
        }
    }
}

