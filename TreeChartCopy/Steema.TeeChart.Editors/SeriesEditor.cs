namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesEditor : Form
    {
        private Button bApplyRandom;
        private ComboBox cbCursor;
        private ComboBox cbDataSource;
        private CheckBox cbDepth;
        private ComboBox cbFormat;
        private ComboBox cbHorizAxis;
        private CheckBox cbHorizDate;
        private CheckBox cbLegend;
        private CheckBox cbSamplesDefault;
        private ComboBox cbSort;
        private ComboBox cbVertAxis;
        private CheckBox cbVertDate;
        private bool changingSource;
        private Container components;
        private Form DataSourceStyle;
        private TextBox ePercentFormat;
        private TextBox eSamples;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label lDepth;
        private Label lSample;
        private Label lSort;
        private SeriesMarksEditor marksEditor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public Series series;
        private Form seriesForm;
        private bool setting;
        internal TabControl tabControl1;
        private TabPage tabFormat;
        private TabPage tabGeneral;
        private TabPage tabMarks;
        internal TabPage tabSource;
        private NumericUpDown udDepth;

        public SeriesEditor()
        {
            this.components = null;
            this.changingSource = false;
            this.DataSourceStyle = null;
            this.InitializeComponent();
        }

        public SeriesEditor(Series s, Control parent) : this()
        {
            this.setting = true;
            this.series = s;
            EditorUtils.InsertForm(this, parent);
            this.ShowSeriesForm(Utils.SeriesTypesIndex(this.series));
            this.setting = false;
        }

        private void bApplyRandom_Click(object sender, EventArgs e)
        {
            this.series.DataSource = null;
            int iNumSampleValues = this.series.INumSampleValues;
            if (iNumSampleValues == 0)
            {
                this.series.FillSampleValues();
            }
            else
            {
                this.series.FillSampleValues(iNumSampleValues);
            }
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.Cursor = EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString());
            }
        }

        private void cbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.changingSource && (this.cbDataSource.SelectedIndex == 0))
            {
                this.series.manualData = true;
                this.series.DataSource = null;
            }
            this.CreateDataSourceForm();
        }

        private void cbDepth_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbDepth.Checked)
            {
                this.series.Depth = -1;
            }
            else
            {
                this.series.Depth = (int) this.udDepth.Value;
            }
        }

        private void cbHorizAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbHorizAxis.SelectedIndex < 3)
            {
                this.series.HorizAxis = (HorizontalAxis) this.cbHorizAxis.SelectedIndex;
            }
            else
            {
                this.series.HorizAxis = HorizontalAxis.Custom;
                this.series.CustomHorizAxis = this.series.chart.Axes.Custom[this.cbHorizAxis.SelectedIndex - 3];
            }
        }

        private void cbHorizDate_CheckedChanged(object sender, EventArgs e)
        {
            this.series.XValues.DateTime = this.cbHorizDate.Checked;
        }

        private void cbSamplesDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbSamplesDefault.Checked)
            {
                this.eSamples.Text = this.series.NumSampleValues().ToString();
                this.cbSamplesDefault.Enabled = false;
            }
        }

        private void cbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.series.notMandatory.Order = ValueListOrder.None;
                if (this.cbSort.SelectedIndex == 2)
                {
                    this.series.mandatory.Order = ValueListOrder.None;
                }
                else
                {
                    this.series.mandatory.Order = (this.cbSort.SelectedIndex == 0) ? ValueListOrder.Ascending : ValueListOrder.Descending;
                    this.series.CheckOrder();
                }
            }
        }

        private void cbVertAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbVertAxis.SelectedIndex < 3)
            {
                this.series.VertAxis = (VerticalAxis) this.cbVertAxis.SelectedIndex;
            }
            else
            {
                this.series.VertAxis = VerticalAxis.Custom;
                this.series.CustomVertAxis = this.series.chart.Axes.Custom[this.cbVertAxis.SelectedIndex - 3];
            }
        }

        private void cbVertDate_CheckedChanged(object sender, EventArgs e)
        {
            this.series.YValues.DateTime = this.cbVertDate.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ShowInLegend = this.cbLegend.Checked;
        }

        private void CheckCBSamples()
        {
            int iNumSampleValues = this.series.INumSampleValues;
            this.cbSamplesDefault.Checked = (iNumSampleValues == 0) || (iNumSampleValues == this.series.NumSampleValues());
            this.cbSamplesDefault.Enabled = !this.cbSamplesDefault.Checked;
        }

        private void CreateDataSourceForm()
        {
            if (this.DataSourceStyle != null)
            {
                this.DataSourceStyle.Close();
                this.DataSourceStyle.Dispose();
            }
            this.ShowSampleControls(false);
            switch (this.cbDataSource.SelectedIndex)
            {
                case 1:
                {
                    this.ShowSampleControls(true);
                    int iNumSampleValues = this.series.INumSampleValues;
                    this.CheckCBSamples();
                    if (iNumSampleValues == 0)
                    {
                        iNumSampleValues = this.series.NumSampleValues();
                    }
                    this.eSamples.Text = iNumSampleValues.ToString();
                    return;
                }
                case 2:
                    this.DataSourceStyle = new FunctionEditor(this.series);
                    EditorUtils.InsertForm(this.DataSourceStyle, this.panel2);
                    EditorUtils.Translate(this.DataSourceStyle);
                    return;

                case 3:
                    this.DataSourceStyle = new DatabaseEditor(this.series);
                    EditorUtils.InsertForm(this.DataSourceStyle, this.panel2);
                    EditorUtils.Translate(this.DataSourceStyle);
                    return;

                case 4:
                    this.DataSourceStyle = new Steema.TeeChart.Editors.TextSource(this.series);
                    EditorUtils.InsertForm(this.DataSourceStyle, this.panel2);
                    EditorUtils.Translate(this.DataSourceStyle);
                    return;

                case 5:
                    this.DataSourceStyle = new SingleSourceEditor(this.series);
                    EditorUtils.InsertForm(this.DataSourceStyle, this.panel2);
                    EditorUtils.Translate(this.DataSourceStyle);
                    return;

                case 6:
                    this.DataSourceStyle = new SummaryEditor(this.series);
                    EditorUtils.InsertForm(this.DataSourceStyle, this.panel2);
                    EditorUtils.Translate(this.DataSourceStyle);
                    return;
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

        private void eSamples_TextChanged(object sender, EventArgs e)
        {
            if (this.eSamples.Text.Length != 0)
            {
                int num = Convert.ToInt32(this.eSamples.Text);
                this.series.INumSampleValues = num;
                this.CheckCBSamples();
            }
            this.bApplyRandom.Enabled = true;
        }

        private void FillAxes(IList aItems, bool horizontal)
        {
            while (aItems.Count > 3)
            {
                aItems.RemoveAt(aItems.Count - 1);
            }
            Chart chart = this.series.chart;
            if (chart != null)
            {
                for (int i = 5; i < chart.Axes.Count; i++)
                {
                    if ((chart.Axes[i].Horizontal && horizontal) || (!chart.Axes[i].Horizontal && !horizontal))
                    {
                        aItems.Add(Texts.CustomAxesEditor + " " + ((i - 5)).ToString());
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabFormat = new TabPage();
            this.tabMarks = new TabPage();
            this.tabGeneral = new TabPage();
            this.cbSort = new ComboBox();
            this.lSort = new Label();
            this.groupBox4 = new GroupBox();
            this.cbVertDate = new CheckBox();
            this.cbVertAxis = new ComboBox();
            this.groupBox3 = new GroupBox();
            this.ePercentFormat = new TextBox();
            this.cbFormat = new ComboBox();
            this.label5 = new Label();
            this.label4 = new Label();
            this.groupBox2 = new GroupBox();
            this.cbHorizDate = new CheckBox();
            this.cbHorizAxis = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.cbDepth = new CheckBox();
            this.udDepth = new NumericUpDown();
            this.lDepth = new Label();
            this.cbCursor = new ComboBox();
            this.label2 = new Label();
            this.cbLegend = new CheckBox();
            this.tabSource = new TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bApplyRandom = new Button();
            this.cbSamplesDefault = new CheckBox();
            this.eSamples = new TextBox();
            this.lSample = new Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDataSource = new ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.udDepth.BeginInit();
            this.tabSource.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabFormat);
            this.tabControl1.Controls.Add(this.tabMarks);
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabSource);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x170, 0xd5);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabFormat.Location = new Point(4, 0x16);
            this.tabFormat.Name = "tabFormat";
            this.tabFormat.Size = new Size(360, 0xbb);
            this.tabFormat.TabIndex = 0;
            this.tabFormat.Text = "Format";
            this.tabMarks.Location = new Point(4, 0x16);
            this.tabMarks.Name = "tabMarks";
            this.tabMarks.Size = new Size(360, 0xbb);
            this.tabMarks.TabIndex = 2;
            this.tabMarks.Text = "Marks";
            this.tabGeneral.Controls.Add(this.cbSort);
            this.tabGeneral.Controls.Add(this.lSort);
            this.tabGeneral.Controls.Add(this.groupBox4);
            this.tabGeneral.Controls.Add(this.groupBox3);
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.groupBox1);
            this.tabGeneral.Location = new Point(4, 0x16);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new Size(360, 0xbb);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General";
            this.cbSort.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbSort.Items.AddRange(new object[] { "Ascending", "Descending", "None" });
            this.cbSort.Location = new Point(240, 0x9b);
            this.cbSort.Name = "cbSort";
            this.cbSort.Size = new Size(0x60, 0x15);
            this.cbSort.TabIndex = 5;
            this.cbSort.SelectedIndexChanged += new EventHandler(this.cbSort_SelectedIndexChanged);
            this.lSort.AutoSize = true;
            this.lSort.Location = new Point(0xd3, 0x9d);
            this.lSort.Name = "lSort";
            this.lSort.Size = new Size(0x1c, 0x10);
            this.lSort.TabIndex = 4;
            this.lSort.Text = "&Sort:";
            this.lSort.TextAlign = ContentAlignment.TopRight;
            this.groupBox4.Controls.Add(this.cbVertDate);
            this.groupBox4.Controls.Add(this.cbVertAxis);
            this.groupBox4.Location = new Point(200, 0x52);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x88, 0x41);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "&Vertical Axis:";
            this.cbVertDate.FlatStyle = FlatStyle.Flat;
            this.cbVertDate.Location = new Point(8, 0x27);
            this.cbVertDate.Name = "cbVertDate";
            this.cbVertDate.Size = new Size(120, 0x18);
            this.cbVertDate.TabIndex = 1;
            this.cbVertDate.Text = "Date Ti&me";
            this.cbVertDate.CheckedChanged += new EventHandler(this.cbVertDate_CheckedChanged);
            this.cbVertAxis.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbVertAxis.Items.AddRange(new object[] { "Left", "Right", "Left and Right" });
            this.cbVertAxis.Location = new Point(8, 0x12);
            this.cbVertAxis.Name = "cbVertAxis";
            this.cbVertAxis.Size = new Size(0x79, 0x15);
            this.cbVertAxis.TabIndex = 0;
            this.cbVertAxis.SelectedIndexChanged += new EventHandler(this.cbVertAxis_SelectedIndexChanged);
            this.groupBox3.Controls.Add(this.ePercentFormat);
            this.groupBox3.Controls.Add(this.cbFormat);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new Point(8, 0x6b);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xb0, 0x45);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.ePercentFormat.BorderStyle = BorderStyle.FixedSingle;
            this.ePercentFormat.Location = new Point(0x44, 40);
            this.ePercentFormat.Name = "ePercentFormat";
            this.ePercentFormat.TabIndex = 3;
            this.ePercentFormat.Text = "";
            this.cbFormat.Location = new Point(0x44, 15);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new Size(100, 0x15);
            this.cbFormat.TabIndex = 1;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(13, 0x2b);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x34, 0x10);
            this.label5.TabIndex = 2;
            this.label5.Text = "&Percents:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x17, 0x12);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x2a, 0x10);
            this.label4.TabIndex = 0;
            this.label4.Text = "Valu&es:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.groupBox2.Controls.Add(this.cbHorizDate);
            this.groupBox2.Controls.Add(this.cbHorizAxis);
            this.groupBox2.Location = new Point(200, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x88, 0x48);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "&Horizontal Axis:";
            this.cbHorizDate.FlatStyle = FlatStyle.Flat;
            this.cbHorizDate.Location = new Point(10, 0x2b);
            this.cbHorizDate.Name = "cbHorizDate";
            this.cbHorizDate.Size = new Size(0x76, 0x18);
            this.cbHorizDate.TabIndex = 1;
            this.cbHorizDate.Text = "Date &Time";
            this.cbHorizDate.CheckedChanged += new EventHandler(this.cbHorizDate_CheckedChanged);
            this.cbHorizAxis.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbHorizAxis.Items.AddRange(new object[] { "Top", "Bottom", "Top and Bottom" });
            this.cbHorizAxis.Location = new Point(8, 0x10);
            this.cbHorizAxis.Name = "cbHorizAxis";
            this.cbHorizAxis.Size = new Size(0x79, 0x15);
            this.cbHorizAxis.TabIndex = 0;
            this.cbHorizAxis.SelectedIndexChanged += new EventHandler(this.cbHorizAxis_SelectedIndexChanged);
            this.groupBox1.Controls.Add(this.cbDepth);
            this.groupBox1.Controls.Add(this.udDepth);
            this.groupBox1.Controls.Add(this.lDepth);
            this.groupBox1.Controls.Add(this.cbCursor);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbLegend);
            this.groupBox1.Location = new Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xb0, 0x60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General:";
            this.cbDepth.FlatStyle = FlatStyle.Flat;
            this.cbDepth.Location = new Point(0x74, 0x48);
            this.cbDepth.Name = "cbDepth";
            this.cbDepth.Size = new Size(0x34, 0x10);
            this.cbDepth.TabIndex = 5;
            this.cbDepth.Text = "&Auto";
            this.cbDepth.CheckedChanged += new EventHandler(this.cbDepth_CheckedChanged);
            this.udDepth.BorderStyle = BorderStyle.FixedSingle;
            this.udDepth.Location = new Point(60, 70);
            int[] bits = new int[4];
            bits[0] = 0x4b0;
            this.udDepth.Maximum = new decimal(bits);
            this.udDepth.Name = "udDepth";
            this.udDepth.Size = new Size(0x30, 20);
            this.udDepth.TabIndex = 4;
            this.udDepth.TextAlign = HorizontalAlignment.Right;
            this.udDepth.ValueChanged += new EventHandler(this.udDepth_ValueChanged);
            this.lDepth.AutoSize = true;
            this.lDepth.Location = new Point(0x16, 0x48);
            this.lDepth.Name = "lDepth";
            this.lDepth.Size = new Size(0x26, 0x10);
            this.lDepth.TabIndex = 3;
            this.lDepth.Text = "&Depth:";
            this.lDepth.TextAlign = ContentAlignment.TopRight;
            this.cbCursor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursor.Location = new Point(60, 0x2c);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x6c, 0x15);
            this.cbCursor.TabIndex = 2;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.FlatStyle = FlatStyle.Flat;
            this.label2.Location = new Point(20, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x29, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Cursor:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbLegend.FlatStyle = FlatStyle.Flat;
            this.cbLegend.Location = new Point(0x10, 0x10);
            this.cbLegend.Name = "cbLegend";
            this.cbLegend.Size = new Size(0x98, 0x18);
            this.cbLegend.TabIndex = 0;
            this.cbLegend.Text = "Show in &Legend";
            this.cbLegend.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.tabSource.Controls.Add(this.panel2);
            this.tabSource.Controls.Add(this.panel1);
            this.tabSource.Location = new Point(4, 0x16);
            this.tabSource.Name = "tabSource";
            this.tabSource.Size = new Size(360, 0xbb);
            this.tabSource.TabIndex = 3;
            this.tabSource.Text = "Data Source";
            this.panel2.Controls.Add(this.bApplyRandom);
            this.panel2.Controls.Add(this.cbSamplesDefault);
            this.panel2.Controls.Add(this.eSamples);
            this.panel2.Controls.Add(this.lSample);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 0x18);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(360, 0xa3);
            this.panel2.TabIndex = 2;
            this.bApplyRandom.FlatStyle = FlatStyle.Flat;
            this.bApplyRandom.Location = new Point(0x98, 0x48);
            this.bApplyRandom.Name = "bApplyRandom";
            this.bApplyRandom.TabIndex = 3;
            this.bApplyRandom.Text = "&Apply";
            this.bApplyRandom.Click += new EventHandler(this.bApplyRandom_Click);
            this.cbSamplesDefault.FlatStyle = FlatStyle.Flat;
            this.cbSamplesDefault.Location = new Point(0xec, 0x23);
            this.cbSamplesDefault.Name = "cbSamplesDefault";
            this.cbSamplesDefault.Size = new Size(0x54, 0x18);
            this.cbSamplesDefault.TabIndex = 2;
            this.cbSamplesDefault.Text = "&Default";
            this.cbSamplesDefault.CheckedChanged += new EventHandler(this.cbSamplesDefault_CheckedChanged);
            this.eSamples.BorderStyle = BorderStyle.FixedSingle;
            this.eSamples.Location = new Point(0x98, 0x25);
            this.eSamples.Name = "eSamples";
            this.eSamples.Size = new Size(0x48, 20);
            this.eSamples.TabIndex = 1;
            this.eSamples.Text = "0";
            this.eSamples.TextAlign = HorizontalAlignment.Right;
            this.eSamples.TextChanged += new EventHandler(this.eSamples_TextChanged);
            this.lSample.AutoSize = true;
            this.lSample.Location = new Point(0x10, 40);
            this.lSample.Name = "lSample";
            this.lSample.Size = new Size(0x87, 0x10);
            this.lSample.TabIndex = 0;
            this.lSample.Text = "&Number of sample values:";
            this.lSample.TextAlign = ContentAlignment.TopRight;
            this.panel1.Controls.Add(this.cbDataSource);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(360, 0x18);
            this.panel1.TabIndex = 1;
            this.cbDataSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbDataSource.Items.AddRange(new object[] { "Manual", "Random", "Function", "Database", "Text file", "Single Record" });
            this.cbDataSource.Location = new Point(7, 3);
            this.cbDataSource.Name = "cbDataSource";
            this.cbDataSource.Size = new Size(0x79, 0x15);
            this.cbDataSource.TabIndex = 1;
            this.cbDataSource.SelectedIndexChanged += new EventHandler(this.cbDataSource_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x170, 0xd5);
            base.Controls.Add(this.tabControl1);
            base.Name = "SeriesEditor";
            base.Load += new EventHandler(this.SeriesEditor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.udDepth.EndInit();
            this.tabSource.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SeriesEditor_Load(object sender, EventArgs e)
        {
        }

        private void SetTabSeriesGeneral()
        {
            this.setting = true;
            this.cbLegend.Checked = this.series.ShowInLegend;
            this.cbDepth.Checked = this.series.Depth == -1;
            this.udDepth.Value = (this.series.Depth == -1) ? 0 : this.series.Depth;
            EditorUtils.AddDefaultValueFormats(this.cbFormat.Items);
            string valueFormat = this.series.ValueFormat;
            if (this.cbFormat.Items.IndexOf(valueFormat) == -1)
            {
                this.cbFormat.Items.Add(valueFormat);
            }
            this.cbFormat.Text = valueFormat;
            this.ePercentFormat.Text = this.series.PercentFormat;
            this.FillAxes(this.cbHorizAxis.Items, true);
            this.FillAxes(this.cbVertAxis.Items, false);
            switch (this.series.HorizAxis)
            {
                case HorizontalAxis.Top:
                    this.cbHorizAxis.SelectedIndex = 0;
                    break;

                case HorizontalAxis.Bottom:
                    this.cbHorizAxis.SelectedIndex = 1;
                    break;

                case HorizontalAxis.Both:
                    this.cbHorizAxis.SelectedIndex = 2;
                    break;

                default:
                    this.cbHorizAxis.SelectedIndex = this.cbHorizAxis.Items.IndexOf(Texts.CustomAxesEditor + " " + this.series.chart.axes.Custom.IndexOf(this.series.CustomHorizAxis));
                    break;
            }
            switch (this.series.VertAxis)
            {
                case VerticalAxis.Left:
                    this.cbVertAxis.SelectedIndex = 0;
                    break;

                case VerticalAxis.Right:
                    this.cbVertAxis.SelectedIndex = 1;
                    break;

                case VerticalAxis.Both:
                    this.cbVertAxis.SelectedIndex = 2;
                    break;

                default:
                    this.cbVertAxis.SelectedIndex = this.cbVertAxis.Items.IndexOf(Texts.CustomAxesEditor + " " + this.series.chart.Axes.Custom.IndexOf(this.series.CustomVertAxis));
                    break;
            }
            this.cbHorizDate.Checked = this.series.XValues.DateTime;
            this.cbVertDate.Checked = this.series.YValues.DateTime;
            switch (this.series.mandatory.Order)
            {
                case ValueListOrder.None:
                    this.cbSort.SelectedIndex = 2;
                    break;

                case ValueListOrder.Ascending:
                    this.cbSort.SelectedIndex = 0;
                    break;

                case ValueListOrder.Descending:
                    this.cbSort.SelectedIndex = 1;
                    break;
            }
            bool flag = this.series.ValuesLists.Count < 3;
            this.cbSort.Visible = flag;
            this.lSort.Visible = flag;
            this.lDepth.Visible = flag;
            this.udDepth.Visible = flag;
            this.cbDepth.Visible = flag;
            EditorUtils.FillCursors(this.cbCursor, this.series.Cursor);
            this.setting = false;
        }

        public static bool ShowEditor(Series s)
        {
            return ShowEditor(s, ChartEditorTabs.Main);
        }

        public static bool ShowEditor(Series s, ChartEditorTabs tab)
        {
            if (s.chart != null)
            {
                return ChartEditor.ShowModal(s, tab);
            }
            return ShowModal(s);
        }

        public static bool ShowModal(Series s)
        {
            SeriesEditor c = new SeriesEditor(s, null);
            c.Text = s.Description;
            EditorUtils.Translate(c);
            return EditorUtils.ShowFormModal(c);
        }

        private void ShowSampleControls(bool visible)
        {
            this.eSamples.Visible = visible;
            this.cbSamplesDefault.Visible = visible;
            this.lSample.Visible = visible;
            this.bApplyRandom.Visible = visible;
        }

        public void ShowSeriesForm(int index)
        {
            if (this.seriesForm != null)
            {
                this.seriesForm.Dispose();
            }
            object[] args = new object[] { this.series };
            this.seriesForm = (Form) Activator.CreateInstance(EditorUtils.SeriesEditorsOf[index], args);
            EditorUtils.InsertForm(this.seriesForm, this.tabFormat);
        }

        public void ShowSeriesSource()
        {
            this.tabControl1.SelectedTab = this.tabSource;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabMarks)
            {
                if (this.marksEditor == null)
                {
                    this.marksEditor = new SeriesMarksEditor(this.series.Marks, this.tabMarks);
                    this.marksEditor.Dock = DockStyle.Fill;
                    base.Invalidate();
                    EditorUtils.Translate(this.marksEditor);
                }
            }
            else if (this.tabControl1.SelectedTab == this.tabGeneral)
            {
                this.SetTabSeriesGeneral();
            }
            else if (this.tabControl1.SelectedTab == this.tabSource)
            {
                this.changingSource = true;
                try
                {
                    if (this.series.Function != null)
                    {
                        this.cbDataSource.SelectedIndex = 2;
                    }
                    else if (this.series.DataSource == null)
                    {
                        this.cbDataSource.SelectedIndex = ((this.series.Count > 0) && !this.series.manualData) ? 1 : 0;
                    }
                    else if (this.series.DataSource is Series)
                    {
                        this.cbDataSource.SelectedIndex = 2;
                    }
                    else
                    {
                        ArrayList list = this.series.DataSourceArray();
                        if ((list != null) && (list[0] is Series))
                        {
                            this.cbDataSource.SelectedIndex = 2;
                        }
                        else if (DataSeriesSource.IsValidSource(this.series.DataSource))
                        {
                            this.cbDataSource.SelectedIndex = 3;
                        }
                        else if (this.series.DataSource is Steema.TeeChart.Data.TextSource)
                        {
                            this.cbDataSource.SelectedIndex = 4;
                        }
                        else if (this.series.DataSource is SingleRecordSource)
                        {
                            this.cbDataSource.SelectedIndex = 5;
                        }
                    }
                }
                finally
                {
                    this.changingSource = false;
                }
            }
        }

        private void udDepth_ValueChanged(object sender, EventArgs e)
        {
            this.series.Depth = (int) this.udDepth.Value;
        }
    }
}

