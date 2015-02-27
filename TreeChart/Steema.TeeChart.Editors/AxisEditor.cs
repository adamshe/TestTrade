namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    public class AxisEditor : Form
    {
        private Steema.TeeChart.Axis axis;
        private Button bAxisMax;
        private Button bAxisMin;
        private ButtonPen bAxisPen;
        private ButtonPen bGridPen;
        private ButtonPen bMinorGrid;
        private ButtonPen BTickInner;
        private ButtonPen bTickMinor;
        private ButtonPen bTickPen;
        private Button button5;
        private CheckBox cbAutomatic;
        private CheckBox cbAutoMax;
        private CheckBox cbAutoMin;
        private CheckBox cbGridCentered;
        private CheckBox cbHorizAxis;
        private CheckBox cbInverted;
        private CheckBox cbLogarithmic;
        private CheckBox cbLogE;
        private CheckBox cbOtherSide;
        private ComboBox cbPosUnits;
        private CheckBox cbTickOnLabels;
        private CheckBox cbTitleVisible;
        private CheckBox cbVisible;
        private Container components;
        private TextBox eLogBase;
        private TextBox eTitle;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private AxisLabelsEditor labelsEditor;
        private Label lAxisIncre;
        private Label lAxisMax;
        private Label lAxisMin;
        private NumericUpDown ndAxisZ;
        private NumericUpDown ndMaxOff;
        private NumericUpDown ndMinOff;
        private System.Windows.Forms.Panel panel1;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabControl tabControl4;
        private TabPage tabLabels;
        private TabPage tabMinorTicks;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPositions;
        private TabPage tabScales;
        private TabPage tabTicks;
        private TabPage tabTitle;
        private TabPage tabTitleStyle;
        private TabPage tabTitleText;
        private TextEditor titleText;
        private NumericUpDown udAxisMinorTickLen;
        private NumericUpDown udAxisTickLength;
        private NumericUpDown udEnd;
        private NumericUpDown udInnerTicksLength;
        private NumericUpDown udMinorCount;
        private NumericUpDown udPos;
        private NumericUpDown udStart;
        private NumericUpDown udTitleAngle;
        private NumericUpDown udTitleSize;

        public AxisEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AxisEditor(Steema.TeeChart.Axis a, Control parent)
        {
            this.components = null;
            this.axis = a;
            this.InitializeComponent();
            EditorUtils.InsertForm(this, parent);
            this.SetProperties();
        }

        private void AxisEditor_SizeChanged(object sender, EventArgs e)
        {
            this.tabControl4.Height = 0x60;
        }

        private void bAxisMax_Click(object sender, EventArgs e)
        {
            this.EditAxisMaxMin(Texts.Maximum, this.cbAutoMax, true);
        }

        private void bAxisMax_Click_1(object sender, EventArgs e)
        {
            this.EditAxisMaxMin(Texts.Maximum, this.cbAutoMax, true);
        }

        private void bAxisMin_Click_1(object sender, EventArgs e)
        {
            this.EditAxisMaxMin(Texts.Minimum, this.cbAutoMin, false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AxisIncrement increment = new AxisIncrement();
            increment.Text = string.Format(Texts.DesiredIncrement, this.axis.TitleOrName());
            increment.isDateTime = this.axis.IsDateTime;
            increment.isExact = this.axis.Labels.ExactDateTime;
            increment.dIncrement = this.axis.Increment;
            increment.iStep = Steema.TeeChart.Axis.FindDateTimeStep(increment.dIncrement);
            if (increment.ShowDialog(this) == DialogResult.OK)
            {
                this.axis.Increment = increment.dIncrement;
                this.axis.Labels.ExactDateTime = increment.isExact;
                this.SetAxisScales();
                this.axis.Invalidate();
            }
            increment.Dispose();
        }

        private void cbAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.Automatic = this.cbAutomatic.Checked;
            if (this.axis.Automatic)
            {
                this.axis.AdjustMaxMin();
            }
            else
            {
                this.axis.AutomaticMaximum = false;
                this.axis.AutomaticMinimum = false;
            }
            this.SetAxisScales();
        }

        private void cbAutoMax_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.AutomaticMaximum = this.cbAutoMax.Checked;
            this.axis.AdjustMaxMin();
            this.SetAxisScales();
        }

        private void cbAutoMin_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.AutomaticMinimum = this.cbAutoMin.Checked;
            this.axis.AdjustMaxMin();
            this.SetAxisScales();
        }

        private void cbGridCentered_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.Grid.Centered = this.cbGridCentered.Checked;
        }

        private void cbHorizAxis_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.horizontal = this.cbHorizAxis.Checked;
        }

        private void cbInverted_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.Inverted = this.cbInverted.Checked;
        }

        private void cbLogarithmic_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.Logarithmic = this.cbLogarithmic.Checked;
            this.EnableLogBaseControls();
        }

        private void cbLogE_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbLogE.Checked)
            {
                this.eLogBase.Text = Math.Exp(1.0).ToString();
                this.cbLogE.Enabled = false;
            }
        }

        private void cbOtherSide_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.OtherSide = this.cbOtherSide.Checked;
        }

        private void cbPosUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbPosUnits.SelectedIndex == 0)
            {
                this.axis.PositionUnits = PositionUnits.Percent;
            }
            else
            {
                this.axis.PositionUnits = PositionUnits.Pixels;
            }
        }

        private void cbTickOnLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.TickOnLabelsOnly = this.cbTickOnLabels.Checked;
        }

        private void cbTitleVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.Title.Visible = this.cbTitleVisible.Checked;
        }

        private void cbVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.axis.Visible = this.cbVisible.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditAxisMaxMin(string aTitle, CheckBox checkBox, bool isMax)
        {
            AxisMaxMin min = new AxisMaxMin();
            min.Text = aTitle + ' ' + this.axis.TitleOrName();
            min.isDateTime = this.axis.IsDateTime;
            if (isMax)
            {
                min.maxMin = this.axis.Maximum;
            }
            else
            {
                min.maxMin = this.axis.Minimum;
            }
            if (min.ShowDialog(this) == DialogResult.OK)
            {
                if (isMax)
                {
                    this.axis.Maximum = min.maxMin;
                }
                else
                {
                    this.axis.Minimum = min.maxMin;
                }
                checkBox.Checked = false;
                this.SetAxisScales();
            }
        }

        private void eLogBase_TextChanged(object sender, EventArgs e)
        {
            this.axis.LogarithmicBase = Utils.StringToDouble(this.eLogBase.Text, this.axis.LogarithmicBase);
            this.cbLogE.Checked = Math.Round(this.axis.LogarithmicBase, 12) == Math.Round(Math.Exp(1.0), 12);
            this.cbLogE.Enabled = !this.cbLogE.Checked;
        }

        private void EnableLogBaseControls()
        {
            this.eLogBase.Enabled = this.cbLogarithmic.Checked && this.cbLogarithmic.Enabled;
            this.cbLogE.Checked = Math.Round(this.axis.LogarithmicBase, 12) == Math.Round(Math.Exp(1.0), 12);
            this.cbLogE.Enabled = this.eLogBase.Enabled ? !this.cbLogE.Checked : false;
        }

        private void eTitle_TextChanged(object sender, EventArgs e)
        {
            if (this.axis != null)
            {
                this.axis.Title.Text = this.eTitle.Text;
            }
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabScales = new TabPage();
            this.tabControl4 = new TabControl();
            this.tabPage1 = new TabPage();
            this.ndMinOff = new NumericUpDown();
            this.label1 = new Label();
            this.lAxisMin = new Label();
            this.bAxisMin = new Button();
            this.cbAutoMin = new CheckBox();
            this.tabPage2 = new TabPage();
            this.ndMaxOff = new NumericUpDown();
            this.label3 = new Label();
            this.lAxisMax = new Label();
            this.bAxisMax = new Button();
            this.cbAutoMax = new CheckBox();
            this.cbLogE = new CheckBox();
            this.lAxisIncre = new Label();
            this.label7 = new Label();
            this.button5 = new Button();
            this.eLogBase = new TextBox();
            this.label2 = new Label();
            this.cbLogarithmic = new CheckBox();
            this.cbInverted = new CheckBox();
            this.cbVisible = new CheckBox();
            this.cbAutomatic = new CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabLabels = new TabPage();
            this.tabTicks = new TabPage();
            this.cbTickOnLabels = new CheckBox();
            this.BTickInner = new ButtonPen();
            this.udInnerTicksLength = new NumericUpDown();
            this.label6 = new Label();
            this.udAxisTickLength = new NumericUpDown();
            this.label5 = new Label();
            this.cbGridCentered = new CheckBox();
            this.bTickPen = new ButtonPen();
            this.bGridPen = new ButtonPen();
            this.bAxisPen = new ButtonPen();
            this.tabTitle = new TabPage();
            this.tabControl2 = new TabControl();
            this.tabTitleStyle = new TabPage();
            this.cbTitleVisible = new CheckBox();
            this.udTitleSize = new NumericUpDown();
            this.label11 = new Label();
            this.udTitleAngle = new NumericUpDown();
            this.label10 = new Label();
            this.eTitle = new TextBox();
            this.label9 = new Label();
            this.tabTitleText = new TabPage();
            this.tabMinorTicks = new TabPage();
            this.bMinorGrid = new ButtonPen();
            this.udMinorCount = new NumericUpDown();
            this.label8 = new Label();
            this.udAxisMinorTickLen = new NumericUpDown();
            this.label15 = new Label();
            this.bTickMinor = new ButtonPen();
            this.tabPositions = new TabPage();
            this.label19 = new Label();
            this.cbPosUnits = new ComboBox();
            this.ndAxisZ = new NumericUpDown();
            this.label4 = new Label();
            this.cbHorizAxis = new CheckBox();
            this.cbOtherSide = new CheckBox();
            this.udEnd = new NumericUpDown();
            this.label16 = new Label();
            this.udStart = new NumericUpDown();
            this.label17 = new Label();
            this.udPos = new NumericUpDown();
            this.label18 = new Label();
            this.tabControl1.SuspendLayout();
            this.tabScales.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ndMinOff.BeginInit();
            this.tabPage2.SuspendLayout();
            this.ndMaxOff.BeginInit();
            this.tabTicks.SuspendLayout();
            this.udInnerTicksLength.BeginInit();
            this.udAxisTickLength.BeginInit();
            this.tabTitle.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabTitleStyle.SuspendLayout();
            this.udTitleSize.BeginInit();
            this.udTitleAngle.BeginInit();
            this.tabMinorTicks.SuspendLayout();
            this.udMinorCount.BeginInit();
            this.udAxisMinorTickLen.BeginInit();
            this.tabPositions.SuspendLayout();
            this.ndAxisZ.BeginInit();
            this.udEnd.BeginInit();
            this.udStart.BeginInit();
            this.udPos.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabScales);
            this.tabControl1.Controls.Add(this.tabLabels);
            this.tabControl1.Controls.Add(this.tabTicks);
            this.tabControl1.Controls.Add(this.tabTitle);
            this.tabControl1.Controls.Add(this.tabMinorTicks);
            this.tabControl1.Controls.Add(this.tabPositions);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(360, 0xdd);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabScales.Controls.Add(this.tabControl4);
            this.tabScales.Controls.Add(this.cbLogE);
            this.tabScales.Controls.Add(this.lAxisIncre);
            this.tabScales.Controls.Add(this.label7);
            this.tabScales.Controls.Add(this.button5);
            this.tabScales.Controls.Add(this.eLogBase);
            this.tabScales.Controls.Add(this.label2);
            this.tabScales.Controls.Add(this.cbLogarithmic);
            this.tabScales.Controls.Add(this.cbInverted);
            this.tabScales.Controls.Add(this.cbVisible);
            this.tabScales.Controls.Add(this.cbAutomatic);
            this.tabScales.Controls.Add(this.panel1);
            this.tabScales.Location = new Point(4, 0x16);
            this.tabScales.Name = "tabScales";
            this.tabScales.Size = new Size(0x160, 0xc3);
            this.tabScales.TabIndex = 0;
            this.tabScales.Text = "Scales";
            this.tabControl4.Controls.Add(this.tabPage1);
            this.tabControl4.Controls.Add(this.tabPage2);
            this.tabControl4.Dock = DockStyle.Fill;
            this.tabControl4.HotTrack = true;
            this.tabControl4.Location = new Point(0, 0x58);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new Size(0x160, 0x6b);
            this.tabControl4.TabIndex = 10;
            this.tabPage1.Controls.Add(this.ndMinOff);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lAxisMin);
            this.tabPage1.Controls.Add(this.bAxisMin);
            this.tabPage1.Controls.Add(this.cbAutoMin);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x158, 0x51);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Minimum";
            this.ndMinOff.BorderStyle = BorderStyle.FixedSingle;
            this.ndMinOff.Location = new Point(0x98, 0x21);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.ndMinOff.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.ndMinOff.Minimum = new decimal(bits);
            this.ndMinOff.Name = "ndMinOff";
            this.ndMinOff.Size = new Size(0x37, 20);
            this.ndMinOff.TabIndex = 4;
            this.ndMinOff.TextAlign = HorizontalAlignment.Right;
            this.ndMinOff.ValueChanged += new EventHandler(this.ndMinOff_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x70, 0x23);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x26, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Offset:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.lAxisMin.AutoSize = true;
            this.lAxisMin.Location = new Point(0x60, 8);
            this.lAxisMin.Name = "lAxisMin";
            this.lAxisMin.Size = new Size(30, 0x10);
            this.lAxisMin.TabIndex = 1;
            this.lAxisMin.Text = "(min)";
            this.lAxisMin.UseMnemonic = false;
            this.bAxisMin.FlatStyle = FlatStyle.Flat;
            this.bAxisMin.Location = new Point(8, 0x20);
            this.bAxisMin.Name = "bAxisMin";
            this.bAxisMin.TabIndex = 2;
            this.bAxisMin.Text = "C&hange...";
            this.bAxisMin.Click += new EventHandler(this.bAxisMin_Click_1);
            this.cbAutoMin.FlatStyle = FlatStyle.Flat;
            this.cbAutoMin.Location = new Point(8, 5);
            this.cbAutoMin.Name = "cbAutoMin";
            this.cbAutoMin.Size = new Size(80, 0x17);
            this.cbAutoMin.TabIndex = 0;
            this.cbAutoMin.Text = "A&uto";
            this.cbAutoMin.CheckedChanged += new EventHandler(this.cbAutoMin_CheckedChanged);
            this.tabPage2.Controls.Add(this.ndMaxOff);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.lAxisMax);
            this.tabPage2.Controls.Add(this.bAxisMax);
            this.tabPage2.Controls.Add(this.cbAutoMax);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x158, 0x51);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Maximum";
            this.ndMaxOff.BorderStyle = BorderStyle.FixedSingle;
            this.ndMaxOff.Location = new Point(0x98, 0x21);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.ndMaxOff.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.ndMaxOff.Minimum = new decimal(bits);
            this.ndMaxOff.Name = "ndMaxOff";
            this.ndMaxOff.Size = new Size(0x37, 20);
            this.ndMaxOff.TabIndex = 4;
            this.ndMaxOff.TextAlign = HorizontalAlignment.Right;
            this.ndMaxOff.ValueChanged += new EventHandler(this.ndMaxOff_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x70, 0x23);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x26, 0x10);
            this.label3.TabIndex = 3;
            this.label3.Text = "&Offset:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.lAxisMax.AutoSize = true;
            this.lAxisMax.Location = new Point(0x60, 8);
            this.lAxisMax.Name = "lAxisMax";
            this.lAxisMax.Size = new Size(0x21, 0x10);
            this.lAxisMax.TabIndex = 1;
            this.lAxisMax.Text = "(max)";
            this.lAxisMax.UseMnemonic = false;
            this.bAxisMax.FlatStyle = FlatStyle.Flat;
            this.bAxisMax.Location = new Point(8, 0x20);
            this.bAxisMax.Name = "bAxisMax";
            this.bAxisMax.TabIndex = 2;
            this.bAxisMax.Text = "C&hange...";
            this.bAxisMax.Click += new EventHandler(this.bAxisMax_Click_1);
            this.cbAutoMax.FlatStyle = FlatStyle.Flat;
            this.cbAutoMax.Location = new Point(8, 5);
            this.cbAutoMax.Name = "cbAutoMax";
            this.cbAutoMax.Size = new Size(80, 0x17);
            this.cbAutoMax.TabIndex = 0;
            this.cbAutoMax.Text = "A&uto";
            this.cbAutoMax.CheckedChanged += new EventHandler(this.cbAutoMax_CheckedChanged);
            this.cbLogE.FlatStyle = FlatStyle.Flat;
            this.cbLogE.Location = new Point(0xed, 0x3b);
            this.cbLogE.Name = "cbLogE";
            this.cbLogE.Size = new Size(40, 0x18);
            this.cbLogE.TabIndex = 9;
            this.cbLogE.Text = "&e";
            this.cbLogE.CheckedChanged += new EventHandler(this.cbLogE_CheckedChanged);
            this.lAxisIncre.AutoSize = true;
            this.lAxisIncre.Location = new Point(0xb0, 0x25);
            this.lAxisIncre.Name = "lAxisIncre";
            this.lAxisIncre.Size = new Size(0x3e, 0x10);
            this.lAxisIncre.TabIndex = 5;
            this.lAxisIncre.Text = "(increment)";
            this.lAxisIncre.UseMnemonic = false;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x6f, 0x25);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x3a, 0x10);
            this.label7.TabIndex = 4;
            this.label7.Text = "Increment:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.label7.UseMnemonic = false;
            this.button5.FlatStyle = FlatStyle.Flat;
            this.button5.Location = new Point(8, 0x20);
            this.button5.Name = "button5";
            this.button5.TabIndex = 3;
            this.button5.Text = "&Change...";
            this.button5.Click += new EventHandler(this.button5_Click);
            this.eLogBase.BorderStyle = BorderStyle.FixedSingle;
            this.eLogBase.Location = new Point(0xb0, 0x3d);
            this.eLogBase.Name = "eLogBase";
            this.eLogBase.Size = new Size(0x37, 20);
            this.eLogBase.TabIndex = 8;
            this.eLogBase.Text = "10";
            this.eLogBase.TextAlign = HorizontalAlignment.Right;
            this.eLogBase.TextChanged += new EventHandler(this.eLogBase_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x79, 0x3f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x37, 0x10);
            this.label2.TabIndex = 7;
            this.label2.Text = "Log &Base:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbLogarithmic.FlatStyle = FlatStyle.Flat;
            this.cbLogarithmic.Location = new Point(8, 0x40);
            this.cbLogarithmic.Name = "cbLogarithmic";
            this.cbLogarithmic.Size = new Size(0x4f, 0x10);
            this.cbLogarithmic.TabIndex = 6;
            this.cbLogarithmic.Text = "&Logarithmic";
            this.cbLogarithmic.CheckedChanged += new EventHandler(this.cbLogarithmic_CheckedChanged);
            this.cbInverted.FlatStyle = FlatStyle.Flat;
            this.cbInverted.Location = new Point(0xb8, 9);
            this.cbInverted.Name = "cbInverted";
            this.cbInverted.Size = new Size(0x40, 0x11);
            this.cbInverted.TabIndex = 2;
            this.cbInverted.Text = "I&nverted";
            this.cbInverted.CheckedChanged += new EventHandler(this.cbInverted_CheckedChanged);
            this.cbVisible.FlatStyle = FlatStyle.Flat;
            this.cbVisible.Location = new Point(0x65, 9);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new Size(0x38, 0x11);
            this.cbVisible.TabIndex = 1;
            this.cbVisible.Text = "&Visible";
            this.cbVisible.CheckedChanged += new EventHandler(this.cbVisible_CheckedChanged);
            this.cbAutomatic.FlatStyle = FlatStyle.Flat;
            this.cbAutomatic.Location = new Point(9, 9);
            this.cbAutomatic.Name = "cbAutomatic";
            this.cbAutomatic.Size = new Size(0x47, 0x12);
            this.cbAutomatic.TabIndex = 0;
            this.cbAutomatic.Text = "&Automatic";
            this.cbAutomatic.CheckedChanged += new EventHandler(this.cbAutomatic_CheckedChanged);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x160, 0x58);
            this.panel1.TabIndex = 11;
            this.tabLabels.Location = new Point(4, 0x16);
            this.tabLabels.Name = "tabLabels";
            this.tabLabels.Size = new Size(0x160, 0xc3);
            this.tabLabels.TabIndex = 1;
            this.tabLabels.Text = "Labels";
            this.tabLabels.Visible = false;
            this.tabTicks.Controls.Add(this.cbTickOnLabels);
            this.tabTicks.Controls.Add(this.BTickInner);
            this.tabTicks.Controls.Add(this.udInnerTicksLength);
            this.tabTicks.Controls.Add(this.label6);
            this.tabTicks.Controls.Add(this.udAxisTickLength);
            this.tabTicks.Controls.Add(this.label5);
            this.tabTicks.Controls.Add(this.cbGridCentered);
            this.tabTicks.Controls.Add(this.bTickPen);
            this.tabTicks.Controls.Add(this.bGridPen);
            this.tabTicks.Controls.Add(this.bAxisPen);
            this.tabTicks.Location = new Point(4, 0x16);
            this.tabTicks.Name = "tabTicks";
            this.tabTicks.Size = new Size(0x160, 0xc3);
            this.tabTicks.TabIndex = 3;
            this.tabTicks.Text = "Ticks";
            this.tabTicks.Visible = false;
            this.cbTickOnLabels.FlatStyle = FlatStyle.Flat;
            this.cbTickOnLabels.Location = new Point(190, 0x33);
            this.cbTickOnLabels.Name = "cbTickOnLabels";
            this.cbTickOnLabels.Size = new Size(0x7f, 0x17);
            this.cbTickOnLabels.TabIndex = 9;
            this.cbTickOnLabels.Text = "At Labels Onl&y";
            this.cbTickOnLabels.CheckedChanged += new EventHandler(this.cbTickOnLabels_CheckedChanged);
            this.BTickInner.FlatStyle = FlatStyle.Flat;
            this.BTickInner.Location = new Point(7, 0x58);
            this.BTickInner.Name = "BTickInner";
            this.BTickInner.TabIndex = 8;
            this.BTickInner.Text = "I&nner...";
            this.udInnerTicksLength.BorderStyle = BorderStyle.FixedSingle;
            this.udInnerTicksLength.Location = new Point(0x7b, 0x59);
            bits = new int[4];
            bits[0] = 360;
            this.udInnerTicksLength.Maximum = new decimal(bits);
            this.udInnerTicksLength.Name = "udInnerTicksLength";
            this.udInnerTicksLength.Size = new Size(0x39, 20);
            this.udInnerTicksLength.TabIndex = 7;
            this.udInnerTicksLength.TextAlign = HorizontalAlignment.Right;
            this.udInnerTicksLength.TextChanged += new EventHandler(this.udInnerTicksLength_TextChanged);
            this.udInnerTicksLength.ValueChanged += new EventHandler(this.udInnerTicksLength_ValueChanged_1);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x5e, 0x5b);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x1a, 0x10);
            this.label6.TabIndex = 6;
            this.label6.Text = "&Len:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.udAxisTickLength.BorderStyle = BorderStyle.FixedSingle;
            this.udAxisTickLength.Location = new Point(0x7b, 0x36);
            bits = new int[4];
            bits[0] = 360;
            this.udAxisTickLength.Maximum = new decimal(bits);
            this.udAxisTickLength.Name = "udAxisTickLength";
            this.udAxisTickLength.Size = new Size(0x39, 20);
            this.udAxisTickLength.TabIndex = 5;
            this.udAxisTickLength.TextAlign = HorizontalAlignment.Right;
            this.udAxisTickLength.TextChanged += new EventHandler(this.udAxisTickLength_TextChanged);
            this.udAxisTickLength.ValueChanged += new EventHandler(this.udAxisTickLength_ValueChanged_1);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x5e, 0x38);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1a, 0x10);
            this.label5.TabIndex = 4;
            this.label5.Text = "Le&n:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.cbGridCentered.FlatStyle = FlatStyle.Flat;
            this.cbGridCentered.Location = new Point(190, 15);
            this.cbGridCentered.Name = "cbGridCentered";
            this.cbGridCentered.Size = new Size(0x77, 0x11);
            this.cbGridCentered.TabIndex = 3;
            this.cbGridCentered.Text = "&Centered";
            this.cbGridCentered.CheckedChanged += new EventHandler(this.cbGridCentered_CheckedChanged);
            this.bTickPen.FlatStyle = FlatStyle.Flat;
            this.bTickPen.Location = new Point(7, 0x33);
            this.bTickPen.Name = "bTickPen";
            this.bTickPen.TabIndex = 2;
            this.bTickPen.Text = "T&icks...";
            this.bGridPen.FlatStyle = FlatStyle.Flat;
            this.bGridPen.Location = new Point(0x63, 15);
            this.bGridPen.Name = "bGridPen";
            this.bGridPen.TabIndex = 1;
            this.bGridPen.Text = "&Grid...";
            this.bAxisPen.FlatStyle = FlatStyle.Flat;
            this.bAxisPen.Location = new Point(7, 15);
            this.bAxisPen.Name = "bAxisPen";
            this.bAxisPen.TabIndex = 0;
            this.bAxisPen.Text = "A&xis...";
            this.tabTitle.Controls.Add(this.tabControl2);
            this.tabTitle.Location = new Point(4, 0x16);
            this.tabTitle.Name = "tabTitle";
            this.tabTitle.Size = new Size(0x160, 0xc3);
            this.tabTitle.TabIndex = 2;
            this.tabTitle.Text = "Title";
            this.tabTitle.Visible = false;
            this.tabControl2.Controls.Add(this.tabTitleStyle);
            this.tabControl2.Controls.Add(this.tabTitleText);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Location = new Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x160, 0xc3);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.SelectedIndexChanged += new EventHandler(this.tabControl2_SelectedIndexChanged);
            this.tabTitleStyle.Controls.Add(this.cbTitleVisible);
            this.tabTitleStyle.Controls.Add(this.udTitleSize);
            this.tabTitleStyle.Controls.Add(this.label11);
            this.tabTitleStyle.Controls.Add(this.udTitleAngle);
            this.tabTitleStyle.Controls.Add(this.label10);
            this.tabTitleStyle.Controls.Add(this.eTitle);
            this.tabTitleStyle.Controls.Add(this.label9);
            this.tabTitleStyle.Location = new Point(4, 0x16);
            this.tabTitleStyle.Name = "tabTitleStyle";
            this.tabTitleStyle.Size = new Size(0x158, 0xa9);
            this.tabTitleStyle.TabIndex = 0;
            this.tabTitleStyle.Text = "Style";
            this.cbTitleVisible.FlatStyle = FlatStyle.Flat;
            this.cbTitleVisible.Location = new Point(0x36, 0x71);
            this.cbTitleVisible.Name = "cbTitleVisible";
            this.cbTitleVisible.Size = new Size(0x3b, 0x12);
            this.cbTitleVisible.TabIndex = 6;
            this.cbTitleVisible.Text = "&Visible";
            this.cbTitleVisible.CheckedChanged += new EventHandler(this.cbTitleVisible_CheckedChanged);
            this.udTitleSize.BorderStyle = BorderStyle.FixedSingle;
            this.udTitleSize.Location = new Point(0x36, 0x4e);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.udTitleSize.Maximum = new decimal(bits);
            this.udTitleSize.Name = "udTitleSize";
            this.udTitleSize.Size = new Size(0x47, 20);
            this.udTitleSize.TabIndex = 5;
            this.udTitleSize.TextAlign = HorizontalAlignment.Right;
            this.udTitleSize.TextChanged += new EventHandler(this.udTitleSize_ValueChanged);
            this.udTitleSize.ValueChanged += new EventHandler(this.udTitleSize_ValueChanged);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x12, 80);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x1d, 0x10);
            this.label11.TabIndex = 4;
            this.label11.Text = "Si&ze:";
            this.label11.TextAlign = ContentAlignment.TopRight;
            this.udTitleAngle.BorderStyle = BorderStyle.FixedSingle;
            bits = new int[4];
            bits[0] = 5;
            this.udTitleAngle.Increment = new decimal(bits);
            this.udTitleAngle.Location = new Point(0x36, 0x2e);
            bits = new int[4];
            bits[0] = 360;
            this.udTitleAngle.Maximum = new decimal(bits);
            this.udTitleAngle.Name = "udTitleAngle";
            this.udTitleAngle.Size = new Size(0x47, 20);
            this.udTitleAngle.TabIndex = 3;
            this.udTitleAngle.TextAlign = HorizontalAlignment.Right;
            this.udTitleAngle.TextChanged += new EventHandler(this.udTitleAngle_TextChanged);
            this.udTitleAngle.ValueChanged += new EventHandler(this.udTitleAngle_ValueChanged);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(11, 0x30);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x24, 0x10);
            this.label10.TabIndex = 2;
            this.label10.Text = "&Angle:";
            this.label10.TextAlign = ContentAlignment.TopRight;
            this.eTitle.BorderStyle = BorderStyle.FixedSingle;
            this.eTitle.Location = new Point(0x36, 15);
            this.eTitle.Name = "eTitle";
            this.eTitle.Size = new Size(0xc2, 20);
            this.eTitle.TabIndex = 1;
            this.eTitle.Text = "";
            this.eTitle.TextChanged += new EventHandler(this.eTitle_TextChanged);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x12, 0x11);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x1d, 0x10);
            this.label9.TabIndex = 0;
            this.label9.Text = "&Title:";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.tabTitleText.Location = new Point(4, 0x16);
            this.tabTitleText.Name = "tabTitleText";
            this.tabTitleText.Size = new Size(0x158, 0xa9);
            this.tabTitleText.TabIndex = 1;
            this.tabTitleText.Text = "Text";
            this.tabTitleText.Visible = false;
            this.tabMinorTicks.Controls.Add(this.bMinorGrid);
            this.tabMinorTicks.Controls.Add(this.udMinorCount);
            this.tabMinorTicks.Controls.Add(this.label8);
            this.tabMinorTicks.Controls.Add(this.udAxisMinorTickLen);
            this.tabMinorTicks.Controls.Add(this.label15);
            this.tabMinorTicks.Controls.Add(this.bTickMinor);
            this.tabMinorTicks.Location = new Point(4, 0x16);
            this.tabMinorTicks.Name = "tabMinorTicks";
            this.tabMinorTicks.Size = new Size(0x160, 0xc3);
            this.tabMinorTicks.TabIndex = 4;
            this.tabMinorTicks.Text = "Minor";
            this.tabMinorTicks.Visible = false;
            this.bMinorGrid.FlatStyle = FlatStyle.Flat;
            this.bMinorGrid.Location = new Point(14, 0x35);
            this.bMinorGrid.Name = "bMinorGrid";
            this.bMinorGrid.TabIndex = 14;
            this.bMinorGrid.Text = "&Grid...";
            this.udMinorCount.BorderStyle = BorderStyle.FixedSingle;
            this.udMinorCount.Location = new Point(0x9f, 0x37);
            bits = new int[4];
            bits[0] = 360;
            this.udMinorCount.Maximum = new decimal(bits);
            this.udMinorCount.Name = "udMinorCount";
            this.udMinorCount.Size = new Size(0x39, 20);
            this.udMinorCount.TabIndex = 13;
            this.udMinorCount.TextAlign = HorizontalAlignment.Right;
            this.udMinorCount.TextChanged += new EventHandler(this.udMinorCount_ValueChanged);
            this.udMinorCount.ValueChanged += new EventHandler(this.udMinorCount_ValueChanged);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x75, 0x39);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x26, 0x10);
            this.label8.TabIndex = 12;
            this.label8.Text = "&Count:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.udAxisMinorTickLen.BorderStyle = BorderStyle.FixedSingle;
            this.udAxisMinorTickLen.Location = new Point(0x9f, 0x11);
            bits = new int[4];
            bits[0] = 360;
            this.udAxisMinorTickLen.Maximum = new decimal(bits);
            this.udAxisMinorTickLen.Name = "udAxisMinorTickLen";
            this.udAxisMinorTickLen.Size = new Size(0x39, 20);
            this.udAxisMinorTickLen.TabIndex = 11;
            this.udAxisMinorTickLen.TextAlign = HorizontalAlignment.Right;
            this.udAxisMinorTickLen.TextChanged += new EventHandler(this.udAxisMinorTickLen_ValueChanged);
            this.udAxisMinorTickLen.ValueChanged += new EventHandler(this.udAxisMinorTickLen_ValueChanged);
            this.label15.AutoSize = true;
            this.label15.Location = new Point(0x71, 0x13);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x2a, 0x10);
            this.label15.TabIndex = 10;
            this.label15.Text = "&Length:";
            this.label15.TextAlign = ContentAlignment.TopRight;
            this.bTickMinor.FlatStyle = FlatStyle.Flat;
            this.bTickMinor.Location = new Point(14, 0x10);
            this.bTickMinor.Name = "bTickMinor";
            this.bTickMinor.TabIndex = 9;
            this.bTickMinor.Text = "T&icks...";
            this.tabPositions.Controls.Add(this.label19);
            this.tabPositions.Controls.Add(this.cbPosUnits);
            this.tabPositions.Controls.Add(this.ndAxisZ);
            this.tabPositions.Controls.Add(this.label4);
            this.tabPositions.Controls.Add(this.cbHorizAxis);
            this.tabPositions.Controls.Add(this.cbOtherSide);
            this.tabPositions.Controls.Add(this.udEnd);
            this.tabPositions.Controls.Add(this.label16);
            this.tabPositions.Controls.Add(this.udStart);
            this.tabPositions.Controls.Add(this.label17);
            this.tabPositions.Controls.Add(this.udPos);
            this.tabPositions.Controls.Add(this.label18);
            this.tabPositions.Location = new Point(4, 0x16);
            this.tabPositions.Name = "tabPositions";
            this.tabPositions.Size = new Size(0x160, 0xc3);
            this.tabPositions.TabIndex = 5;
            this.tabPositions.Text = "Position";
            this.tabPositions.Visible = false;
            this.label19.AutoSize = true;
            this.label19.FlatStyle = FlatStyle.Flat;
            this.label19.Location = new Point(0xa4, 0x10);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x21, 0x10);
            this.label19.TabIndex = 8;
            this.label19.Text = "&Units:";
            this.cbPosUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPosUnits.Items.AddRange(new object[] { "Percent", "Pixels" });
            this.cbPosUnits.Location = new Point(0xa4, 0x25);
            this.cbPosUnits.Name = "cbPosUnits";
            this.cbPosUnits.Size = new Size(0x58, 0x15);
            this.cbPosUnits.TabIndex = 9;
            this.cbPosUnits.SelectedIndexChanged += new EventHandler(this.cbPosUnits_SelectedIndexChanged);
            this.ndAxisZ.BorderStyle = BorderStyle.FixedSingle;
            this.ndAxisZ.Location = new Point(0xbc, 80);
            this.ndAxisZ.Name = "ndAxisZ";
            this.ndAxisZ.Size = new Size(50, 20);
            this.ndAxisZ.TabIndex = 11;
            this.ndAxisZ.TextAlign = HorizontalAlignment.Right;
            this.ndAxisZ.ValueChanged += new EventHandler(this.ndAxisZ_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x9d, 0x53);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1b, 0x10);
            this.label4.TabIndex = 10;
            this.label4.Text = "&Z %:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.cbHorizAxis.FlatStyle = FlatStyle.Flat;
            this.cbHorizAxis.Location = new Point(0x4f, 0x87);
            this.cbHorizAxis.Name = "cbHorizAxis";
            this.cbHorizAxis.Size = new Size(0x85, 0x11);
            this.cbHorizAxis.TabIndex = 7;
            this.cbHorizAxis.Text = "&Horizontal";
            this.cbHorizAxis.CheckedChanged += new EventHandler(this.cbHorizAxis_CheckedChanged);
            this.cbOtherSide.FlatStyle = FlatStyle.Flat;
            this.cbOtherSide.Location = new Point(0x4f, 0x6d);
            this.cbOtherSide.Name = "cbOtherSide";
            this.cbOtherSide.Size = new Size(0x85, 0x13);
            this.cbOtherSide.TabIndex = 6;
            this.cbOtherSide.Text = "&Other side";
            this.cbOtherSide.CheckedChanged += new EventHandler(this.cbOtherSide_CheckedChanged);
            this.udEnd.BorderStyle = BorderStyle.FixedSingle;
            this.udEnd.Location = new Point(0x4d, 0x4f);
            this.udEnd.Name = "udEnd";
            this.udEnd.Size = new Size(50, 20);
            this.udEnd.TabIndex = 5;
            this.udEnd.TextAlign = HorizontalAlignment.Right;
            this.udEnd.TextChanged += new EventHandler(this.udEnd_TextChanged);
            this.udEnd.ValueChanged += new EventHandler(this.udEnd_ValueChanged);
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x1f, 0x52);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x29, 0x10);
            this.label16.TabIndex = 4;
            this.label16.Text = "&End %:";
            this.label16.TextAlign = ContentAlignment.TopRight;
            this.udStart.BorderStyle = BorderStyle.FixedSingle;
            this.udStart.Location = new Point(0x4d, 0x2d);
            this.udStart.Name = "udStart";
            this.udStart.Size = new Size(50, 20);
            this.udStart.TabIndex = 3;
            this.udStart.TextAlign = HorizontalAlignment.Right;
            this.udStart.TextChanged += new EventHandler(this.udStart_TextChanged);
            this.udStart.ValueChanged += new EventHandler(this.udStart_ValueChanged);
            this.label17.AutoSize = true;
            this.label17.Location = new Point(0x1c, 0x31);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x2c, 0x10);
            this.label17.TabIndex = 2;
            this.label17.Text = "St&art %:";
            this.label17.TextAlign = ContentAlignment.TopRight;
            this.udPos.BorderStyle = BorderStyle.FixedSingle;
            this.udPos.Location = new Point(0x4d, 12);
            bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.udPos.Minimum = new decimal(bits);
            this.udPos.Name = "udPos";
            this.udPos.Size = new Size(50, 20);
            this.udPos.TabIndex = 1;
            this.udPos.TextAlign = HorizontalAlignment.Right;
            this.udPos.TextChanged += new EventHandler(this.udPos_TextChanged);
            this.udPos.ValueChanged += new EventHandler(this.udPos_ValueChanged_1);
            this.label18.AutoSize = true;
            this.label18.Location = new Point(11, 15);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x3d, 0x10);
            this.label18.TabIndex = 0;
            this.label18.Text = "&Position %:";
            this.label18.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(360, 0xdd);
            base.Controls.Add(this.tabControl1);
            base.Name = "AxisEditor";
            this.Text = "Axis editor";
            base.SizeChanged += new EventHandler(this.AxisEditor_SizeChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabScales.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ndMinOff.EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ndMaxOff.EndInit();
            this.tabTicks.ResumeLayout(false);
            this.udInnerTicksLength.EndInit();
            this.udAxisTickLength.EndInit();
            this.tabTitle.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabTitleStyle.ResumeLayout(false);
            this.udTitleSize.EndInit();
            this.udTitleAngle.EndInit();
            this.tabMinorTicks.ResumeLayout(false);
            this.udMinorCount.EndInit();
            this.udAxisMinorTickLen.EndInit();
            this.tabPositions.ResumeLayout(false);
            this.ndAxisZ.EndInit();
            this.udEnd.EndInit();
            this.udStart.EndInit();
            this.udPos.EndInit();
            base.ResumeLayout(false);
        }

        private void ndAxisZ_ValueChanged(object sender, EventArgs e)
        {
            this.axis.ZPosition = Convert.ToDouble(this.ndAxisZ.Value);
        }

        private void ndMaxOff_ValueChanged(object sender, EventArgs e)
        {
            this.axis.MaximumOffset = (int) this.ndMaxOff.Value;
        }

        private void ndMinOff_ValueChanged(object sender, EventArgs e)
        {
            this.axis.MinimumOffset = (int) this.ndMinOff.Value;
        }

        public void SelectTab(int tabIndex)
        {
            this.tabControl1.SelectedIndex = tabIndex;
        }

        private void SetAxisLabels()
        {
            if (this.labelsEditor == null)
            {
                this.labelsEditor = new AxisLabelsEditor(this.tabLabels);
            }
            this.labelsEditor.SetAxis(this.axis);
        }

        private void SetAxisMinorTicks()
        {
            this.udAxisMinorTickLen.Value = this.axis.MinorTicks.Length;
            this.udMinorCount.Value = this.axis.MinorTickCount;
            this.bMinorGrid.Pen = this.axis.MinorGrid;
            this.bTickMinor.Pen = this.axis.MinorTicks;
        }

        private void SetAxisPositions()
        {
            this.udPos.Value = Convert.ToDecimal(Math.Round(this.axis.RelativePosition));
            this.udStart.Value = Convert.ToDecimal(Math.Round(this.axis.StartPosition));
            this.udEnd.Value = Convert.ToDecimal(Math.Round(this.axis.EndPosition));
            this.ndAxisZ.Value = Convert.ToDecimal(Math.Round(this.axis.ZPosition));
            if (this.axis.PositionUnits == PositionUnits.Percent)
            {
                this.cbPosUnits.SelectedIndex = 0;
            }
            else
            {
                this.cbPosUnits.SelectedIndex = 1;
            }
            this.cbOtherSide.Checked = this.axis.OtherSide;
            this.cbOtherSide.Enabled = this.axis.IsCustom();
            this.cbHorizAxis.Checked = this.axis.Horizontal;
            this.cbHorizAxis.Enabled = this.axis.IsCustom();
        }

        private void SetAxisScales()
        {
            this.lAxisIncre.Text = AxisIncrement.GetIncrementText(this, this.axis.Increment, this.axis.IsDateTime, this.axis.Labels.ExactDateTime, this.axis.Labels.ValueFormat);
            if (this.axis.IsDateTime)
            {
                string format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                if (this.axis.Minimum >= 1.0)
                {
                    this.lAxisMin.Text = DateTime.FromOADate(this.axis.Minimum).ToString(format);
                }
                else
                {
                    this.lAxisMin.Text = Utils.TimeToStr(this.axis.Minimum);
                }
                if (this.axis.Maximum >= 1.0)
                {
                    this.lAxisMax.Text = DateTime.FromOADate(this.axis.Maximum).ToString(format);
                }
                else
                {
                    this.lAxisMax.Text = Utils.TimeToStr(this.axis.Maximum);
                }
            }
            else
            {
                this.lAxisMin.Text = this.axis.Minimum.ToString(this.axis.Labels.ValueFormat);
                this.lAxisMax.Text = this.axis.Maximum.ToString(this.axis.Labels.ValueFormat);
            }
            this.cbAutomatic.Checked = this.axis.Automatic;
            this.cbAutoMax.Checked = this.axis.AutomaticMaximum;
            this.cbAutoMin.Checked = this.axis.AutomaticMinimum;
            this.ndMaxOff.Value = this.axis.MaximumOffset;
            this.ndMinOff.Value = this.axis.MinimumOffset;
            this.cbLogarithmic.Checked = this.axis.Logarithmic;
            this.cbLogarithmic.Enabled = !this.axis.IsDepthAxis;
            this.eLogBase.Text = this.axis.LogarithmicBase.ToString();
            this.EnableLogBaseControls();
            this.cbInverted.Checked = this.axis.Inverted;
            this.cbVisible.Checked = this.axis.Visible;
        }

        private void SetAxisTicks()
        {
            this.udAxisTickLength.Value = this.axis.Ticks.Length;
            this.udInnerTicksLength.Value = this.axis.TicksInner.Length;
            this.cbTickOnLabels.Checked = this.axis.TickOnLabelsOnly;
            this.cbGridCentered.Checked = this.axis.Grid.Centered;
            this.bAxisPen.Pen = this.axis.AxisPen;
            this.bGridPen.Pen = this.axis.Grid;
            this.bTickPen.Pen = this.axis.Ticks;
            this.BTickInner.Pen = this.axis.TicksInner;
        }

        private void SetAxisTitle(AxisTitle t)
        {
            this.eTitle.Text = t.Caption;
            this.udTitleAngle.Value = t.Angle;
            this.udTitleSize.Value = t.CustomSize;
            this.cbTitleVisible.Checked = t.Visible;
        }

        private void SetProperties()
        {
            if (this.axis != null)
            {
                this.tabControl1_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = this.tabControl1.SelectedTab;
            if (selectedTab == this.tabScales)
            {
                this.SetAxisScales();
            }
            else if (selectedTab == this.tabLabels)
            {
                this.SetAxisLabels();
            }
            else if (selectedTab == this.tabTitle)
            {
                this.SetAxisTitle(this.axis.Title);
            }
            else if (selectedTab == this.tabTicks)
            {
                this.SetAxisTicks();
            }
            else if (selectedTab == this.tabMinorTicks)
            {
                this.SetAxisMinorTicks();
            }
            else if (selectedTab == this.tabPositions)
            {
                this.SetAxisPositions();
            }
            this.cbVisible.Checked = this.axis.Visible;
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.tabControl2.SelectedTab == this.tabTitleText) && (this.titleText == null))
            {
                this.titleText = new TextEditor(this.axis.Title.Font, this.tabTitleText);
                EditorUtils.Translate(this.titleText);
            }
        }

        private void udAxisMinorTickLen_ValueChanged(object sender, EventArgs e)
        {
            this.axis.MinorTicks.Length = (int) this.udAxisMinorTickLen.Value;
        }

        private void udAxisTickLength_TextChanged(object sender, EventArgs e)
        {
            this.udAxisTickLength_ValueChanged(sender, e);
        }

        private void udAxisTickLength_ValueChanged(object sender, EventArgs e)
        {
            this.axis.Ticks.Length = (int) this.udAxisTickLength.Value;
        }

        private void udAxisTickLength_ValueChanged_1(object sender, EventArgs e)
        {
            this.axis.Ticks.Length = (int) this.udAxisTickLength.Value;
        }

        private void udEnd_TextChanged(object sender, EventArgs e)
        {
            this.udEnd_ValueChanged(sender, e);
        }

        private void udEnd_ValueChanged(object sender, EventArgs e)
        {
            this.axis.EndPosition = (int) this.udEnd.Value;
        }

        private void udInnerTicksLength_TextChanged(object sender, EventArgs e)
        {
            this.udInnerTicksLength_ValueChanged(sender, e);
        }

        private void udInnerTicksLength_ValueChanged(object sender, EventArgs e)
        {
            if (this.axis != null)
            {
                this.axis.TicksInner.Length = (int) this.udInnerTicksLength.Value;
            }
        }

        private void udInnerTicksLength_ValueChanged_1(object sender, EventArgs e)
        {
            this.axis.TicksInner.Length = (int) this.udInnerTicksLength.Value;
        }

        private void udLogBase_ValueChanged(object sender, EventArgs e)
        {
            this.axis.LogarithmicBase = double.Parse(this.eLogBase.Text);
        }

        private void udMinorCount_ValueChanged(object sender, EventArgs e)
        {
            this.axis.MinorTickCount = (int) this.udMinorCount.Value;
        }

        private void udPos_TextChanged(object sender, EventArgs e)
        {
            this.udPos_ValueChanged(sender, e);
        }

        private void udPos_ValueChanged(object sender, EventArgs e)
        {
            this.axis.RelativePosition = (int) this.udPos.Value;
        }

        private void udPos_ValueChanged_1(object sender, EventArgs e)
        {
            this.axis.RelativePosition = (int) this.udPos.Value;
        }

        private void udStart_TextChanged(object sender, EventArgs e)
        {
            this.udStart_ValueChanged(sender, e);
        }

        private void udStart_ValueChanged(object sender, EventArgs e)
        {
            this.axis.StartPosition = (int) this.udStart.Value;
        }

        private void udTitleAngle_TextChanged(object sender, EventArgs e)
        {
            this.udTitleAngle_ValueChanged(sender, e);
        }

        private void udTitleAngle_ValueChanged(object sender, EventArgs e)
        {
            if (this.axis != null)
            {
                this.axis.Title.Angle = (int) this.udTitleAngle.Value;
            }
        }

        private void udTitleSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.axis.Horizontal)
            {
                this.axis.Title.Size.Width = (int) this.udTitleSize.Value;
            }
            else
            {
                this.axis.Title.Size.Height = (int) this.udTitleSize.Value;
            }
        }

        public Steema.TeeChart.Axis Axis
        {
            get
            {
                return this.axis;
            }
            set
            {
                this.axis = value;
                this.SetProperties();
            }
        }
    }
}

