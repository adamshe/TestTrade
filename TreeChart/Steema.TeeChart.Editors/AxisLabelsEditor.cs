namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    public class AxisLabelsEditor : Form
    {
        private Axis axis;
        private AxisLabels axisLabels;
        private CheckBox cbExpo;
        private ComboBox cbFormat;
        private CheckBox cbLabelAlign;
        private CheckBox cbLabels;
        private CheckBox cbMultiline;
        private CheckBox cbOnAxis;
        private CheckBox cbRoundFirst;
        private Container components;
        private GroupBox groupBox1;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label labelAxisFormat;
        private TextEditor labelText;
        private RadioButton rbAuto;
        private RadioButton rbMark;
        private RadioButton rbNone;
        private RadioButton rbText;
        private RadioButton rbValue;
        private TabControl tabControl3;
        private TabPage tabLabelFormat;
        private TabPage tabLabelStyle;
        private TabPage tabLabelText;
        private NumericUpDown udLabelsAngle;
        private NumericUpDown udLabelsSize;
        private NumericUpDown udSepar;

        public AxisLabelsEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AxisLabelsEditor(Control parent) : this()
        {
            EditorUtils.Translate(this);
            EditorUtils.InsertForm(this, parent);
        }

        private void cbExpo_CheckedChanged(object sender, EventArgs e)
        {
            this.axisLabels.Exponent = this.cbExpo.Checked;
        }

        private void cbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.axis.IsDateTime)
            {
                this.axisLabels.DateTimeFormat = this.cbFormat.Text;
            }
            else
            {
                this.axisLabels.ValueFormat = this.cbFormat.Text;
            }
        }

        private void cbFormat_TextChanged(object sender, EventArgs e)
        {
            this.cbFormat_SelectedIndexChanged(sender, e);
        }

        private void cbLabelAlign_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbLabelAlign.Checked)
            {
                this.axisLabels.Align = AxisLabelAlign.Default;
            }
            else
            {
                this.axisLabels.Align = AxisLabelAlign.Opposite;
            }
        }

        private void cbLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.axisLabels.Visible = this.cbLabels.Checked;
        }

        private void cbMultiline_CheckedChanged(object sender, EventArgs e)
        {
            this.axisLabels.MultiLine = this.cbMultiline.Checked;
        }

        private void cbOnAxis_CheckedChanged(object sender, EventArgs e)
        {
            this.axisLabels.OnAxis = this.cbOnAxis.Checked;
        }

        private void cbRoundFirst_CheckedChanged(object sender, EventArgs e)
        {
            this.axisLabels.RoundFirstLabel = this.cbRoundFirst.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetLabelStyle(int styleIndex)
        {
            switch (styleIndex)
            {
                case 0:
                    this.rbAuto.Checked = true;
                    return;

                case 1:
                    this.rbNone.Checked = true;
                    return;

                case 2:
                    this.rbValue.Checked = true;
                    return;

                case 3:
                    this.rbMark.Checked = true;
                    return;

                case 4:
                    this.rbText.Checked = true;
                    return;
            }
        }

        private void InitializeComponent()
        {
            this.tabControl3 = new TabControl();
            this.tabLabelStyle = new TabPage();
            this.groupBox1 = new GroupBox();
            this.rbMark = new RadioButton();
            this.rbNone = new RadioButton();
            this.rbText = new RadioButton();
            this.rbValue = new RadioButton();
            this.rbAuto = new RadioButton();
            this.udSepar = new NumericUpDown();
            this.label14 = new Label();
            this.udLabelsAngle = new NumericUpDown();
            this.label13 = new Label();
            this.udLabelsSize = new NumericUpDown();
            this.label12 = new Label();
            this.cbOnAxis = new CheckBox();
            this.cbRoundFirst = new CheckBox();
            this.cbMultiline = new CheckBox();
            this.cbLabels = new CheckBox();
            this.tabLabelFormat = new TabPage();
            this.cbLabelAlign = new CheckBox();
            this.cbFormat = new ComboBox();
            this.labelAxisFormat = new Label();
            this.cbExpo = new CheckBox();
            this.tabLabelText = new TabPage();
            this.tabControl3.SuspendLayout();
            this.tabLabelStyle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.udSepar.BeginInit();
            this.udLabelsAngle.BeginInit();
            this.udLabelsSize.BeginInit();
            this.tabLabelFormat.SuspendLayout();
            base.SuspendLayout();
            this.tabControl3.Controls.Add(this.tabLabelStyle);
            this.tabControl3.Controls.Add(this.tabLabelFormat);
            this.tabControl3.Controls.Add(this.tabLabelText);
            this.tabControl3.Dock = DockStyle.Fill;
            this.tabControl3.HotTrack = true;
            this.tabControl3.Location = new Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new Size(0x128, 0xc5);
            this.tabControl3.TabIndex = 1;
            this.tabControl3.SelectedIndexChanged += new EventHandler(this.tabControl3_SelectedIndexChanged);
            this.tabLabelStyle.Controls.Add(this.groupBox1);
            this.tabLabelStyle.Controls.Add(this.udSepar);
            this.tabLabelStyle.Controls.Add(this.label14);
            this.tabLabelStyle.Controls.Add(this.udLabelsAngle);
            this.tabLabelStyle.Controls.Add(this.label13);
            this.tabLabelStyle.Controls.Add(this.udLabelsSize);
            this.tabLabelStyle.Controls.Add(this.label12);
            this.tabLabelStyle.Controls.Add(this.cbOnAxis);
            this.tabLabelStyle.Controls.Add(this.cbRoundFirst);
            this.tabLabelStyle.Controls.Add(this.cbMultiline);
            this.tabLabelStyle.Controls.Add(this.cbLabels);
            this.tabLabelStyle.Location = new Point(4, 0x16);
            this.tabLabelStyle.Name = "tabLabelStyle";
            this.tabLabelStyle.Size = new Size(0x120, 0xab);
            this.tabLabelStyle.TabIndex = 0;
            this.tabLabelStyle.Text = "Style";
            this.groupBox1.BackColor = SystemColors.Control;
            this.groupBox1.Controls.Add(this.rbMark);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Controls.Add(this.rbText);
            this.groupBox1.Controls.Add(this.rbValue);
            this.groupBox1.Controls.Add(this.rbAuto);
            this.groupBox1.Location = new Point(11, 0x60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x109, 0x42);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Style:";
            this.rbMark.FlatStyle = FlatStyle.Flat;
            this.rbMark.Location = new Point(0x6b, 0x22);
            this.rbMark.Name = "rbMark";
            this.rbMark.Size = new Size(0x4b, 0x1c);
            this.rbMark.TabIndex = 4;
            this.rbMark.Text = "Mar&k";
            this.rbMark.CheckedChanged += new EventHandler(this.rbValue_CheckedChanged);
            this.rbNone.FlatStyle = FlatStyle.Flat;
            this.rbNone.Location = new Point(12, 0x22);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new Size(0x54, 0x1c);
            this.rbNone.TabIndex = 3;
            this.rbNone.Text = "&None";
            this.rbNone.CheckedChanged += new EventHandler(this.rbValue_CheckedChanged);
            this.rbText.FlatStyle = FlatStyle.Flat;
            this.rbText.Location = new Point(0xca, 12);
            this.rbText.Name = "rbText";
            this.rbText.Size = new Size(0x37, 0x1c);
            this.rbText.TabIndex = 2;
            this.rbText.Text = "&Text";
            this.rbText.CheckedChanged += new EventHandler(this.rbValue_CheckedChanged);
            this.rbValue.FlatStyle = FlatStyle.Flat;
            this.rbValue.Location = new Point(0x6b, 12);
            this.rbValue.Name = "rbValue";
            this.rbValue.Size = new Size(0x4b, 0x1c);
            this.rbValue.TabIndex = 1;
            this.rbValue.Text = "Val&ue";
            this.rbValue.CheckedChanged += new EventHandler(this.rbValue_CheckedChanged);
            this.rbAuto.FlatStyle = FlatStyle.Flat;
            this.rbAuto.Location = new Point(12, 12);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new Size(0x54, 0x1c);
            this.rbAuto.TabIndex = 0;
            this.rbAuto.Text = "&Auto";
            this.rbAuto.CheckedChanged += new EventHandler(this.rbValue_CheckedChanged);
            this.udSepar.BorderStyle = BorderStyle.FixedSingle;
            this.udSepar.Location = new Point(0xe3, 0x47);
            this.udSepar.Name = "udSepar";
            this.udSepar.Size = new Size(0x30, 20);
            this.udSepar.TabIndex = 11;
            this.udSepar.TextAlign = HorizontalAlignment.Right;
            this.udSepar.ValueChanged += new EventHandler(this.udSepar_ValueChanged);
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x7b, 0x49);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x61, 0x10);
            this.label14.TabIndex = 10;
            this.label14.Text = "M&in. Separation %";
            this.label14.TextAlign = ContentAlignment.TopRight;
            this.udLabelsAngle.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 90;
            this.udLabelsAngle.Increment = new decimal(bits);
            this.udLabelsAngle.Location = new Point(0xe3, 0x27);
            bits = new int[4];
            bits[0] = 360;
            this.udLabelsAngle.Maximum = new decimal(bits);
            this.udLabelsAngle.Name = "udLabelsAngle";
            this.udLabelsAngle.Size = new Size(0x30, 20);
            this.udLabelsAngle.TabIndex = 9;
            this.udLabelsAngle.TextAlign = HorizontalAlignment.Right;
            this.udLabelsAngle.TextChanged += new EventHandler(this.udLabelsAngle_TextChanged);
            this.udLabelsAngle.ValueChanged += new EventHandler(this.udLabelsAngle_ValueChanged);
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0xb8, 0x29);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x24, 0x10);
            this.label13.TabIndex = 8;
            this.label13.Text = "Angl&e:";
            this.label13.TextAlign = ContentAlignment.TopRight;
            this.udLabelsSize.BorderStyle = BorderStyle.FixedSingle;
            this.udLabelsSize.Location = new Point(0xe3, 8);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.udLabelsSize.Maximum = new decimal(bits);
            this.udLabelsSize.Name = "udLabelsSize";
            this.udLabelsSize.Size = new Size(0x30, 20);
            this.udLabelsSize.TabIndex = 7;
            this.udLabelsSize.TextAlign = HorizontalAlignment.Right;
            this.udLabelsSize.ValueChanged += new EventHandler(this.udLabelsSize_ValueChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0xbf, 10);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x1d, 0x10);
            this.label12.TabIndex = 6;
            this.label12.Text = "Si&ze:";
            this.label12.TextAlign = ContentAlignment.TopRight;
            this.cbOnAxis.FlatStyle = FlatStyle.Flat;
            this.cbOnAxis.Location = new Point(11, 0x44);
            this.cbOnAxis.Name = "cbOnAxis";
            this.cbOnAxis.Size = new Size(0x6d, 0x12);
            this.cbOnAxis.TabIndex = 3;
            this.cbOnAxis.Text = "&Label on axis";
            this.cbOnAxis.CheckedChanged += new EventHandler(this.cbOnAxis_CheckedChanged);
            this.cbRoundFirst.FlatStyle = FlatStyle.Flat;
            this.cbRoundFirst.Location = new Point(11, 0x31);
            this.cbRoundFirst.Name = "cbRoundFirst";
            this.cbRoundFirst.Size = new Size(0x6d, 0x11);
            this.cbRoundFirst.TabIndex = 2;
            this.cbRoundFirst.Text = "&Round first";
            this.cbRoundFirst.CheckedChanged += new EventHandler(this.cbRoundFirst_CheckedChanged);
            this.cbMultiline.FlatStyle = FlatStyle.Flat;
            this.cbMultiline.Location = new Point(11, 0x1a);
            this.cbMultiline.Name = "cbMultiline";
            this.cbMultiline.Size = new Size(0x6d, 0x15);
            this.cbMultiline.TabIndex = 1;
            this.cbMultiline.Text = "&Multi-line";
            this.cbMultiline.CheckedChanged += new EventHandler(this.cbMultiline_CheckedChanged);
            this.cbLabels.FlatStyle = FlatStyle.Flat;
            this.cbLabels.Location = new Point(11, 6);
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new Size(0x6d, 0x12);
            this.cbLabels.TabIndex = 0;
            this.cbLabels.Text = "&Visible";
            this.cbLabels.CheckedChanged += new EventHandler(this.cbLabels_CheckedChanged);
            this.tabLabelFormat.Controls.Add(this.cbLabelAlign);
            this.tabLabelFormat.Controls.Add(this.cbFormat);
            this.tabLabelFormat.Controls.Add(this.labelAxisFormat);
            this.tabLabelFormat.Controls.Add(this.cbExpo);
            this.tabLabelFormat.Location = new Point(4, 0x16);
            this.tabLabelFormat.Name = "tabLabelFormat";
            this.tabLabelFormat.Size = new Size(0x120, 0xab);
            this.tabLabelFormat.TabIndex = 1;
            this.tabLabelFormat.Text = "Format";
            this.tabLabelFormat.Visible = false;
            this.cbLabelAlign.FlatStyle = FlatStyle.Flat;
            this.cbLabelAlign.Location = new Point(14, 0x5b);
            this.cbLabelAlign.Name = "cbLabelAlign";
            this.cbLabelAlign.Size = new Size(170, 0x15);
            this.cbLabelAlign.TabIndex = 3;
            this.cbLabelAlign.Text = "Default &Alignment";
            this.cbLabelAlign.CheckedChanged += new EventHandler(this.cbLabelAlign_CheckedChanged);
            this.cbFormat.Location = new Point(14, 0x39);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new Size(0xb2, 0x15);
            this.cbFormat.TabIndex = 2;
            this.cbFormat.TextChanged += new EventHandler(this.cbFormat_TextChanged);
            this.cbFormat.SelectedIndexChanged += new EventHandler(this.cbFormat_SelectedIndexChanged);
            this.labelAxisFormat.AutoSize = true;
            this.labelAxisFormat.Location = new Point(14, 0x26);
            this.labelAxisFormat.Name = "labelAxisFormat";
            this.labelAxisFormat.Size = new Size(80, 0x10);
            this.labelAxisFormat.TabIndex = 1;
            this.labelAxisFormat.Text = "Labels &Format:";
            this.cbExpo.FlatStyle = FlatStyle.Flat;
            this.cbExpo.Location = new Point(14, 10);
            this.cbExpo.Name = "cbExpo";
            this.cbExpo.Size = new Size(0xa2, 20);
            this.cbExpo.TabIndex = 0;
            this.cbExpo.Text = "&Exponential";
            this.cbExpo.CheckedChanged += new EventHandler(this.cbExpo_CheckedChanged);
            this.tabLabelText.Location = new Point(4, 0x16);
            this.tabLabelText.Name = "tabLabelText";
            this.tabLabelText.Size = new Size(0x120, 0xab);
            this.tabLabelText.TabIndex = 2;
            this.tabLabelText.Text = "Text";
            this.tabLabelText.Visible = false;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x128, 0xc5);
            base.Controls.Add(this.tabControl3);
            base.Name = "AxisLabelsEditor";
            this.tabControl3.ResumeLayout(false);
            this.tabLabelStyle.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.udSepar.EndInit();
            this.udLabelsAngle.EndInit();
            this.udLabelsSize.EndInit();
            this.tabLabelFormat.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void rbValue_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton) sender).Checked)
            {
                this.SetLabelStyle();
            }
        }

        internal void SetAxis(Axis a)
        {
            this.axis = a;
            this.axisLabels = a.Labels;
            string dateTimeFormat = "";
            this.cbLabels.Checked = this.axisLabels.visible;
            this.GetLabelStyle((int) this.axisLabels.iStyle);
            this.cbOnAxis.Checked = this.axisLabels.OnAxis;
            this.cbRoundFirst.Checked = this.axisLabels.RoundFirstLabel;
            this.udLabelsAngle.Value = this.axisLabels.Angle;
            this.udSepar.Value = this.axisLabels.Separation;
            this.udLabelsSize.Value = Convert.ToDecimal(this.axisLabels.CustomSize);
            this.cbMultiline.Checked = this.axisLabels.MultiLine;
            this.cbExpo.Checked = this.axisLabels.Exponent;
            this.cbFormat.Items.Clear();
            if (this.axis.IsDateTime)
            {
                if ((this.axis.Maximum - this.axis.Minimum) <= 1.0)
                {
                    this.cbFormat.Items.Add(DateTimeFormatInfo.CurrentInfo.ShortTimePattern);
                    this.cbFormat.Items.Add(DateTimeFormatInfo.CurrentInfo.LongTimePattern);
                    this.cbFormat.Items.Add("t");
                    this.cbFormat.Items.Add("T");
                    this.cbFormat.Items.Add("HH:mm");
                    this.cbFormat.Items.Add("hh:mm");
                    this.cbFormat.Items.Add("hh:mm:ss");
                    this.cbFormat.Items.Add("hh:mm:ss tt");
                    this.cbFormat.Items.Add("hh:mm:ss.fff");
                }
                else
                {
                    this.cbFormat.Items.Add(DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
                    this.cbFormat.Items.Add(DateTimeFormatInfo.CurrentInfo.LongDatePattern);
                    this.cbFormat.Items.Add("d");
                    this.cbFormat.Items.Add("D");
                    this.cbFormat.Items.Add("yyyy/mm/dd");
                    this.cbFormat.Items.Add("mm/dd/yyyy");
                    this.cbFormat.Items.Add("dd/mm/yyyy");
                    this.cbFormat.Items.Add("ddd");
                    this.cbFormat.Items.Add("dddd");
                    this.cbFormat.Items.Add("MMM");
                    this.cbFormat.Items.Add("MMMM");
                }
                this.labelAxisFormat.Text = Texts.DateTimeFormat;
                dateTimeFormat = this.axisLabels.DateTimeFormat;
                if (dateTimeFormat.Length == 0)
                {
                    dateTimeFormat = this.axis.DateTimeDefaultFormat(this.axis.Maximum - this.axis.Minimum);
                }
            }
            else
            {
                EditorUtils.AddDefaultValueFormats(this.cbFormat.Items);
                this.labelAxisFormat.Text = Texts.ValuesFormat;
                dateTimeFormat = this.axisLabels.ValueFormat;
                if (dateTimeFormat.Length == 0)
                {
                    dateTimeFormat = "#,##0.###";
                }
                dateTimeFormat = dateTimeFormat;
            }
            if (this.cbFormat.Items.IndexOf(dateTimeFormat) == -1)
            {
                this.cbFormat.Items.Add(dateTimeFormat);
            }
            this.cbFormat.Text = dateTimeFormat;
            if (this.labelText != null)
            {
                this.labelText.RefreshControls(this.axis.Labels.Font);
            }
            this.cbLabelAlign.Checked = this.axisLabels.Align == AxisLabelAlign.Default;
            this.cbLabelAlign.Enabled = !this.axis.Horizontal && !this.axis.IsDepthAxis;
        }

        private void SetLabelStyle()
        {
            if (this.rbAuto.Checked)
            {
                this.axisLabels.Style = AxisLabelStyle.Auto;
            }
            else if (this.rbValue.Checked)
            {
                this.axisLabels.Style = AxisLabelStyle.Value;
            }
            else if (this.rbText.Checked)
            {
                this.axisLabels.Style = AxisLabelStyle.Text;
            }
            else if (this.rbMark.Checked)
            {
                this.axisLabels.Style = AxisLabelStyle.Mark;
            }
            else if (this.rbNone.Checked)
            {
                this.axisLabels.Style = AxisLabelStyle.None;
            }
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.tabControl3.SelectedTab == this.tabLabelText) && (this.labelText == null))
            {
                this.labelText = new TextEditor(this.axisLabels.Font, this.tabLabelText);
                EditorUtils.Translate(this.labelText);
            }
        }

        private void udLabelsAngle_TextChanged(object sender, EventArgs e)
        {
            this.udLabelsAngle_ValueChanged(sender, e);
        }

        private void udLabelsAngle_ValueChanged(object sender, EventArgs e)
        {
            this.axisLabels.Angle = (int) this.udLabelsAngle.Value;
        }

        private void udLabelsSize_ValueChanged(object sender, EventArgs e)
        {
            this.axisLabels.CustomSize = (int) this.udLabelsSize.Value;
        }

        private void udSepar_ValueChanged(object sender, EventArgs e)
        {
            this.axisLabels.Separation = (int) this.udSepar.Value;
        }
    }
}

