namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Grid3DSeries : Form
    {
        private ButtonColor BColor;
        private ButtonColor BFromColor;
        private ButtonColor BMidColor;
        private ButtonColor BToColor;
        private CheckBox CBIrreg;
        private ComboBox CBPalStyle;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private RadioButton RGColor;
        private RadioButton RGPalette;
        private RadioButton RGRange;
        protected internal Custom3DPalette series;
        private Custom3DGrid seriesgrid;
        private bool setting;
        private NumericUpDown UDDepth;
        private NumericUpDown UDPalette;
        private NumericUpDown UDXGrid;
        private NumericUpDown UDZGrid;

        public Grid3DSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Grid3DSeries(Custom3DPalette s, Control c) : this()
        {
            TabControl parent = (TabControl) ((TabPage) c.Parent).Parent;
            TabPage page = new TabPage(Texts.Grid3D);
            parent.TabPages.Add(page);
            this.series = s;
            this.SetProperties();
            EditorUtils.InsertForm(this, page);
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            Color color = this.BColor.Color;
            if (color != this.series.Color)
            {
                this.RGColor.Checked = true;
                this.series.Color = color;
            }
        }

        private void BFromColor_Click(object sender, EventArgs e)
        {
            Color color = this.BFromColor.Color;
            if (color != this.series.StartColor)
            {
                this.RGRange.Checked = true;
                this.series.StartColor = color;
            }
        }

        private void BMidColor_Click(object sender, EventArgs e)
        {
            Color color = this.BMidColor.Color;
            if (color != this.series.MidColor)
            {
                this.RGRange.Checked = true;
                this.series.MidColor = color;
            }
        }

        private void BToColor_Click(object sender, EventArgs e)
        {
            Color color = this.BToColor.Color;
            if (color != this.series.EndColor)
            {
                this.RGRange.Checked = true;
                this.series.EndColor = color;
            }
        }

        private void CBIrreg_CheckedChanged(object sender, EventArgs e)
        {
            this.seriesgrid.IrregularGrid = this.CBIrreg.Checked;
        }

        private void CBPalStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.RGPalette.Checked = true;
                if (this.CBPalStyle.SelectedIndex == 0)
                {
                    this.series.PaletteStyle = PaletteStyles.Pale;
                }
                else if (this.CBPalStyle.SelectedIndex == 1)
                {
                    this.series.PaletteStyle = PaletteStyles.Strong;
                }
                else
                {
                    this.series.PaletteStyle = PaletteStyles.GrayScale;
                }
            }
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
            this.groupBox1 = new GroupBox();
            this.UDPalette = new NumericUpDown();
            this.CBPalStyle = new ComboBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.RGPalette = new RadioButton();
            this.RGRange = new RadioButton();
            this.RGColor = new RadioButton();
            this.BToColor = new ButtonColor();
            this.BMidColor = new ButtonColor();
            this.BFromColor = new ButtonColor();
            this.BColor = new ButtonColor();
            this.groupBox2 = new GroupBox();
            this.UDDepth = new NumericUpDown();
            this.UDZGrid = new NumericUpDown();
            this.UDXGrid = new NumericUpDown();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.CBIrreg = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.UDPalette.BeginInit();
            this.groupBox2.SuspendLayout();
            this.UDDepth.BeginInit();
            this.UDZGrid.BeginInit();
            this.UDXGrid.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.UDPalette);
            this.groupBox1.Controls.Add(this.CBPalStyle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.RGPalette);
            this.groupBox1.Controls.Add(this.RGRange);
            this.groupBox1.Controls.Add(this.RGColor);
            this.groupBox1.Controls.Add(this.BToColor);
            this.groupBox1.Controls.Add(this.BMidColor);
            this.groupBox1.Controls.Add(this.BFromColor);
            this.groupBox1.Controls.Add(this.BColor);
            this.groupBox1.Location = new Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(240, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color Mode:";
            this.UDPalette.BorderStyle = BorderStyle.FixedSingle;
            this.UDPalette.Location = new Point(0x83, 0x44);
            int[] bits = new int[4];
            bits[0] = 0x7d0;
            this.UDPalette.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDPalette.Minimum = new decimal(bits);
            this.UDPalette.Name = "UDPalette";
            this.UDPalette.Size = new Size(0x35, 20);
            this.UDPalette.TabIndex = 8;
            this.UDPalette.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDPalette.Value = new decimal(bits);
            this.UDPalette.ValueChanged += new EventHandler(this.UDPalette_ValueChanged);
            this.CBPalStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBPalStyle.Items.AddRange(new object[] { "Pale", "Strong", "Gray Scale" });
            this.CBPalStyle.Location = new Point(0x84, 0x5b);
            this.CBPalStyle.Name = "CBPalStyle";
            this.CBPalStyle.Size = new Size(0x66, 0x15);
            this.CBPalStyle.TabIndex = 10;
            this.CBPalStyle.SelectedIndexChanged += new EventHandler(this.CBPalStyle_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x60, 0x5d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 0x10);
            this.label2.TabIndex = 9;
            this.label2.Text = "St&yle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x60, 70);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 0x10);
            this.label1.TabIndex = 7;
            this.label1.Text = "S&teps:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.RGPalette.FlatStyle = FlatStyle.Flat;
            this.RGPalette.Location = new Point(8, 0x45);
            this.RGPalette.Name = "RGPalette";
            this.RGPalette.Size = new Size(80, 0x10);
            this.RGPalette.TabIndex = 6;
            this.RGPalette.Text = "&Palette:";
            this.RGPalette.CheckedChanged += new EventHandler(this.RGPalette_CheckedChanged);
            this.RGRange.FlatStyle = FlatStyle.Flat;
            this.RGRange.Location = new Point(8, 0x2a);
            this.RGRange.Name = "RGRange";
            this.RGRange.Size = new Size(80, 0x10);
            this.RGRange.TabIndex = 2;
            this.RGRange.Text = "&Range:";
            this.RGRange.CheckedChanged += new EventHandler(this.RGRange_CheckedChanged);
            this.RGColor.FlatStyle = FlatStyle.Flat;
            this.RGColor.Location = new Point(8, 0x10);
            this.RGColor.Name = "RGColor";
            this.RGColor.Size = new Size(80, 0x10);
            this.RGColor.TabIndex = 0;
            this.RGColor.Text = "&Series:";
            this.RGColor.CheckedChanged += new EventHandler(this.RGColor_CheckedChanged);
            this.BToColor.Color = Color.Empty;
            this.BToColor.Location = new Point(0xc0, 40);
            this.BToColor.Name = "BToColor";
            this.BToColor.Size = new Size(40, 0x17);
            this.BToColor.TabIndex = 5;
            this.BToColor.Click += new EventHandler(this.BToColor_Click);
            this.BMidColor.Color = Color.Empty;
            this.BMidColor.Location = new Point(0x90, 40);
            this.BMidColor.Name = "BMidColor";
            this.BMidColor.Size = new Size(40, 0x17);
            this.BMidColor.TabIndex = 4;
            this.BMidColor.Click += new EventHandler(this.BMidColor_Click);
            this.BFromColor.Color = Color.Empty;
            this.BFromColor.Location = new Point(0x60, 40);
            this.BFromColor.Name = "BFromColor";
            this.BFromColor.Size = new Size(40, 0x17);
            this.BFromColor.TabIndex = 3;
            this.BFromColor.Click += new EventHandler(this.BFromColor_Click);
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(0x60, 11);
            this.BColor.Name = "BColor";
            this.BColor.TabIndex = 1;
            this.BColor.Text = "C&olor...";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.groupBox2.Controls.Add(this.UDDepth);
            this.groupBox2.Controls.Add(this.UDZGrid);
            this.groupBox2.Controls.Add(this.UDXGrid);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new Point(0x100, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x68, 0x58);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Grid size:";
            this.UDDepth.BorderStyle = BorderStyle.FixedSingle;
            this.UDDepth.Location = new Point(0x30, 0x40);
            bits = new int[4];
            bits[0] = 1;
            this.UDDepth.Minimum = new decimal(bits);
            this.UDDepth.Name = "UDDepth";
            this.UDDepth.Size = new Size(0x30, 20);
            this.UDDepth.TabIndex = 5;
            this.UDDepth.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDDepth.Value = new decimal(bits);
            this.UDDepth.ValueChanged += new EventHandler(this.UDDepth_ValueChanged);
            this.UDZGrid.BorderStyle = BorderStyle.FixedSingle;
            this.UDZGrid.Location = new Point(0x30, 40);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDZGrid.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDZGrid.Minimum = new decimal(bits);
            this.UDZGrid.Name = "UDZGrid";
            this.UDZGrid.Size = new Size(0x30, 20);
            this.UDZGrid.TabIndex = 3;
            this.UDZGrid.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDZGrid.Value = new decimal(bits);
            this.UDZGrid.ValueChanged += new EventHandler(this.UDXGrid_ValueChanged);
            this.UDXGrid.BorderStyle = BorderStyle.FixedSingle;
            this.UDXGrid.Location = new Point(0x30, 0x10);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDXGrid.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDXGrid.Minimum = new decimal(bits);
            this.UDXGrid.Name = "UDXGrid";
            this.UDXGrid.Size = new Size(0x30, 20);
            this.UDXGrid.TabIndex = 1;
            this.UDXGrid.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDXGrid.Value = new decimal(bits);
            this.UDXGrid.ValueChanged += new EventHandler(this.UDXGrid_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(8, 0x42);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x26, 0x10);
            this.label5.TabIndex = 4;
            this.label5.Text = "&Depth:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x20, 0x2a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(14, 0x10);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Z:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x20, 0x12);
            this.label3.Name = "label3";
            this.label3.Size = new Size(15, 0x10);
            this.label3.TabIndex = 0;
            this.label3.Text = "&X:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.CBIrreg.FlatStyle = FlatStyle.Flat;
            this.CBIrreg.Location = new Point(0x103, 0x65);
            this.CBIrreg.Name = "CBIrreg";
            this.CBIrreg.Size = new Size(0x5d, 0x10);
            this.CBIrreg.TabIndex = 2;
            this.CBIrreg.Text = "&Irregular";
            this.CBIrreg.CheckedChanged += new EventHandler(this.CBIrreg_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x16b, 0x87);
            base.Controls.Add(this.CBIrreg);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Name = "Grid3DSeries";
            this.groupBox1.ResumeLayout(false);
            this.UDPalette.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.UDDepth.EndInit();
            this.UDZGrid.EndInit();
            this.UDXGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void RGColor_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = true;
            this.series.ColorEach = false;
            this.series.UseColorRange = false;
            this.series.UsePalette = false;
        }

        private void RGPalette_CheckedChanged(object sender, EventArgs e)
        {
            this.series.UseColorRange = false;
            this.series.UsePalette = true;
        }

        private void RGRange_CheckedChanged(object sender, EventArgs e)
        {
            this.series.UseColorRange = true;
            this.series.UsePalette = false;
        }

        protected void SetProperties()
        {
            this.setting = true;
            if (this.series is Custom3DGrid)
            {
                this.seriesgrid = (Custom3DGrid) this.series;
                this.UDXGrid.Value = this.seriesgrid.NumXValues;
                this.UDZGrid.Value = this.seriesgrid.NumZValues;
                this.CBIrreg.Checked = this.seriesgrid.IrregularGrid;
                bool flag = this.seriesgrid.CanCreateValues();
                this.UDXGrid.Enabled = flag;
                this.UDZGrid.Enabled = flag;
            }
            else
            {
                this.groupBox2.Visible = false;
                this.CBIrreg.Visible = false;
            }
            this.UDDepth.Value = this.series.TimesZOrder;
            this.UDPalette.Value = this.series.PaletteSteps;
            if (this.series.PaletteStyle == PaletteStyles.Pale)
            {
                this.CBPalStyle.SelectedIndex = 0;
            }
            else if (this.series.PaletteStyle == PaletteStyles.Strong)
            {
                this.CBPalStyle.SelectedIndex = 1;
            }
            else
            {
                this.CBPalStyle.SelectedIndex = 2;
            }
            this.RGRange.Checked = this.series.UseColorRange;
            this.RGPalette.Checked = this.series.UsePalette;
            this.RGColor.Checked = !this.series.UseColorRange && !this.series.UsePalette;
            this.BColor.Color = this.series.Color;
            this.BFromColor.Color = this.series.StartColor;
            this.BMidColor.Color = this.series.MidColor;
            this.BToColor.Color = this.series.EndColor;
            this.setting = false;
        }

        private void UDDepth_ValueChanged(object sender, EventArgs e)
        {
            this.series.TimesZOrder = (int) this.UDDepth.Value;
        }

        private void UDPalette_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.RGPalette.Checked = true;
                this.series.PaletteSteps = (int) this.UDPalette.Value;
            }
        }

        private void UDXGrid_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting && ((this.UDXGrid.Value != this.seriesgrid.NumXValues) || (this.UDZGrid.Value != this.seriesgrid.NumZValues)))
            {
                this.seriesgrid.CreateValues((int) this.UDXGrid.Value, (int) this.UDZGrid.Value);
            }
        }
    }
}

