namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesMarksEditor : Form
    {
        private Button BCalloutPointer;
        private ButtonPen BMarkLinCol;
        private ButtonPen button1;
        private CheckBox cbAllVisible;
        private CheckBox cbClip;
        private ComboBox CBHead;
        private CheckBox cbMultiLine;
        private ComboBox cbStyle;
        private CheckBox cbVisible;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private SeriesMarks marks;
        private CustomShapeEditor shapeEditor;
        private TabPage tabArrow;
        private TabControl tabControl1;
        private TabPage tabStyle;
        private NumericUpDown udAngle;
        private NumericUpDown UDArrowDist;
        private NumericUpDown UDArrowLength;
        private NumericUpDown udDrawEvery;
        private NumericUpDown UDHeadSize;
        private NumericUpDown udLength;

        public SeriesMarksEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public SeriesMarksEditor(SeriesMarks m, Control parent) : this()
        {
            this.marks = m;
            this.cbVisible.Checked = m.Visible;
            this.cbClip.Checked = m.Clip;
            this.cbMultiLine.Checked = m.MultiLine;
            this.udDrawEvery.Value = m.DrawEvery;
            this.udAngle.Value = Utils.Round(m.Angle);
            this.udLength.Value = m.ArrowLength;
            this.cbAllVisible.Checked = m.AllSeriesVisible();
            this.button1.Pen = m.Arrow;
            this.button1.Visible = false;
            this.udLength.Visible = false;
            this.label2.Visible = false;
            this.groupBox1.Visible = false;
            switch (this.marks.Style)
            {
                case MarksStyles.Value:
                    this.cbStyle.SelectedIndex = 0;
                    break;

                case MarksStyles.Percent:
                    this.cbStyle.SelectedIndex = 1;
                    break;

                case MarksStyles.Label:
                    this.cbStyle.SelectedIndex = 2;
                    break;

                case MarksStyles.LabelPercent:
                    this.cbStyle.SelectedIndex = 3;
                    break;

                case MarksStyles.LabelValue:
                    this.cbStyle.SelectedIndex = 4;
                    break;

                case MarksStyles.Legend:
                    this.cbStyle.SelectedIndex = 5;
                    break;

                case MarksStyles.PercentTotal:
                    this.cbStyle.SelectedIndex = 6;
                    break;

                case MarksStyles.LabelPercentTotal:
                    this.cbStyle.SelectedIndex = 7;
                    break;

                case MarksStyles.XValue:
                    this.cbStyle.SelectedIndex = 8;
                    break;

                case MarksStyles.XY:
                    this.cbStyle.SelectedIndex = 9;
                    break;
            }
            this.BMarkLinCol.Pen = m.Callout.Arrow;
            this.UDArrowDist.Value = m.Callout.Distance;
            this.CBHead.SelectedIndex = (int) m.Callout.ArrowHead;
            this.UDHeadSize.Value = m.Callout.ArrowHeadSize;
            this.UDArrowLength.Value = m.Callout.Length;
            this.shapeEditor = CustomShapeEditor.Add(this.tabControl1, this.marks);
            EditorUtils.InsertForm(this, parent);
        }

        private void BCalloutPointer_Click(object sender, EventArgs e)
        {
            EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.SeriesPointer(this.marks.Callout));
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            this.marks.Clip = this.cbClip.Checked;
        }

        private void CBHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.marks.Callout.ArrowHead = (ArrowHeadStyles) this.CBHead.SelectedIndex;
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbStyle.SelectedIndex)
            {
                case 0:
                    this.marks.Style = MarksStyles.Value;
                    return;

                case 1:
                    this.marks.Style = MarksStyles.Percent;
                    return;

                case 2:
                    this.marks.Style = MarksStyles.Label;
                    return;

                case 3:
                    this.marks.Style = MarksStyles.LabelPercent;
                    return;

                case 4:
                    this.marks.Style = MarksStyles.LabelValue;
                    return;

                case 5:
                    this.marks.Style = MarksStyles.Legend;
                    return;

                case 6:
                    this.marks.Style = MarksStyles.PercentTotal;
                    return;

                case 7:
                    this.marks.Style = MarksStyles.LabelPercentTotal;
                    return;

                case 8:
                    this.marks.Style = MarksStyles.XValue;
                    return;

                case 9:
                    this.marks.Style = MarksStyles.XY;
                    return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.marks.Visible = this.cbVisible.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Steema.TeeChart.Styles.Series series in this.marks.Chart.Series)
            {
                series.Marks.Visible = this.cbAllVisible.Checked;
            }
            this.cbVisible.Checked = this.cbAllVisible.Checked;
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
            this.tabControl1 = new TabControl();
            this.tabStyle = new TabPage();
            this.cbMultiLine = new CheckBox();
            this.udDrawEvery = new NumericUpDown();
            this.label4 = new Label();
            this.udAngle = new NumericUpDown();
            this.label3 = new Label();
            this.groupBox1 = new GroupBox();
            this.udLength = new NumericUpDown();
            this.label2 = new Label();
            this.button1 = new ButtonPen();
            this.cbStyle = new ComboBox();
            this.label1 = new Label();
            this.cbClip = new CheckBox();
            this.cbAllVisible = new CheckBox();
            this.cbVisible = new CheckBox();
            this.tabArrow = new TabPage();
            this.UDArrowLength = new NumericUpDown();
            this.label5 = new Label();
            this.UDHeadSize = new NumericUpDown();
            this.label12 = new Label();
            this.CBHead = new ComboBox();
            this.label11 = new Label();
            this.UDArrowDist = new NumericUpDown();
            this.label10 = new Label();
            this.BCalloutPointer = new Button();
            this.BMarkLinCol = new ButtonPen();
            this.tabControl1.SuspendLayout();
            this.tabStyle.SuspendLayout();
            this.udDrawEvery.BeginInit();
            this.udAngle.BeginInit();
            this.groupBox1.SuspendLayout();
            this.udLength.BeginInit();
            this.tabArrow.SuspendLayout();
            this.UDArrowLength.BeginInit();
            this.UDHeadSize.BeginInit();
            this.UDArrowDist.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabStyle);
            this.tabControl1.Controls.Add(this.tabArrow);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x120, 0xd5);
            this.tabControl1.TabIndex = 0;
            this.tabStyle.Controls.Add(this.cbMultiLine);
            this.tabStyle.Controls.Add(this.udDrawEvery);
            this.tabStyle.Controls.Add(this.label4);
            this.tabStyle.Controls.Add(this.udAngle);
            this.tabStyle.Controls.Add(this.label3);
            this.tabStyle.Controls.Add(this.groupBox1);
            this.tabStyle.Controls.Add(this.cbStyle);
            this.tabStyle.Controls.Add(this.label1);
            this.tabStyle.Controls.Add(this.cbClip);
            this.tabStyle.Controls.Add(this.cbAllVisible);
            this.tabStyle.Controls.Add(this.cbVisible);
            this.tabStyle.Location = new Point(4, 0x16);
            this.tabStyle.Name = "tabStyle";
            this.tabStyle.Size = new Size(280, 0xbb);
            this.tabStyle.TabIndex = 0;
            this.tabStyle.Text = "Style";
            this.cbMultiLine.Enabled = false;
            this.cbMultiLine.FlatStyle = FlatStyle.Flat;
            this.cbMultiLine.Location = new Point(8, 0x3b);
            this.cbMultiLine.Name = "cbMultiLine";
            this.cbMultiLine.Size = new Size(0x70, 0x18);
            this.cbMultiLine.TabIndex = 10;
            this.cbMultiLine.Text = "&Multi-line";
            this.udDrawEvery.BorderStyle = BorderStyle.FixedSingle;
            this.udDrawEvery.Location = new Point(0x58, 0x60);
            int[] bits = new int[4];
            bits[0] = 0x5f5e100;
            this.udDrawEvery.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.udDrawEvery.Minimum = new decimal(bits);
            this.udDrawEvery.Name = "udDrawEvery";
            this.udDrawEvery.Size = new Size(0x30, 20);
            this.udDrawEvery.TabIndex = 9;
            this.udDrawEvery.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.udDrawEvery.Value = new decimal(bits);
            this.udDrawEvery.TextChanged += new EventHandler(this.udDrawEvery_ValueChanged);
            this.udDrawEvery.ValueChanged += new EventHandler(this.udDrawEvery_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x18, 0x60);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 0x10);
            this.label4.TabIndex = 8;
            this.label4.Text = "Dra&w every:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.udAngle.BorderStyle = BorderStyle.FixedSingle;
            bits = new int[4];
            bits[0] = 5;
            this.udAngle.Increment = new decimal(bits);
            this.udAngle.Location = new Point(200, 0x60);
            bits = new int[4];
            bits[0] = 360;
            this.udAngle.Maximum = new decimal(bits);
            this.udAngle.Name = "udAngle";
            this.udAngle.Size = new Size(0x3d, 20);
            this.udAngle.TabIndex = 7;
            this.udAngle.TextAlign = HorizontalAlignment.Right;
            this.udAngle.TextChanged += new EventHandler(this.udAngle_ValueChanged);
            this.udAngle.ValueChanged += new EventHandler(this.udAngle_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(160, 0x60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 0x10);
            this.label3.TabIndex = 6;
            this.label3.Text = "A&ngle:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.groupBox1.Controls.Add(this.udLength);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new Point(9, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x100, 0x2f);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "L&ines:";
            this.udLength.BorderStyle = BorderStyle.FixedSingle;
            this.udLength.Location = new Point(0xb8, 0x11);
            bits = new int[4];
            bits[0] = 0x2710;
            this.udLength.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x2710;
            bits[3] = -2147483648;
            this.udLength.Minimum = new decimal(bits);
            this.udLength.Name = "udLength";
            this.udLength.Size = new Size(0x3d, 20);
            this.udLength.TabIndex = 2;
            this.udLength.TextAlign = HorizontalAlignment.Right;
            this.udLength.TextChanged += new EventHandler(this.udLength_ValueChanged);
            this.udLength.ValueChanged += new EventHandler(this.udLength_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x88, 0x13);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2a, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Length:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x10, 15);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Color...";
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.AddRange(new object[] { "Value", "Percent", "Label", "Label and Percent", "Label and Value", "Legend", "Percent Total", "Label and Percent Total", "X value", "X and Y values" });
            this.cbStyle.Location = new Point(0x90, 0x29);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x79, 0x15);
            this.cbStyle.TabIndex = 4;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x70, 0x2b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Style:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.cbClip.FlatStyle = FlatStyle.Flat;
            this.cbClip.Location = new Point(8, 0x21);
            this.cbClip.Name = "cbClip";
            this.cbClip.Size = new Size(0x60, 0x18);
            this.cbClip.TabIndex = 2;
            this.cbClip.Text = "Clippe&d";
            this.cbClip.CheckedChanged += new EventHandler(this.cb_CheckedChanged);
            this.cbAllVisible.FlatStyle = FlatStyle.Flat;
            this.cbAllVisible.Location = new Point(0x5e, 8);
            this.cbAllVisible.Name = "cbAllVisible";
            this.cbAllVisible.Size = new Size(0xa8, 0x18);
            this.cbAllVisible.TabIndex = 1;
            this.cbAllVisible.Text = "&All Series Visible";
            this.cbAllVisible.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.cbVisible.FlatStyle = FlatStyle.Flat;
            this.cbVisible.Location = new Point(8, 8);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new Size(80, 0x18);
            this.cbVisible.TabIndex = 0;
            this.cbVisible.Text = "&Visible";
            this.cbVisible.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.tabArrow.Controls.Add(this.UDArrowLength);
            this.tabArrow.Controls.Add(this.label5);
            this.tabArrow.Controls.Add(this.UDHeadSize);
            this.tabArrow.Controls.Add(this.label12);
            this.tabArrow.Controls.Add(this.CBHead);
            this.tabArrow.Controls.Add(this.label11);
            this.tabArrow.Controls.Add(this.UDArrowDist);
            this.tabArrow.Controls.Add(this.label10);
            this.tabArrow.Controls.Add(this.BCalloutPointer);
            this.tabArrow.Controls.Add(this.BMarkLinCol);
            this.tabArrow.Location = new Point(4, 0x16);
            this.tabArrow.Name = "tabArrow";
            this.tabArrow.Size = new Size(280, 0xbb);
            this.tabArrow.TabIndex = 1;
            this.tabArrow.Text = "Arrow";
            this.UDArrowLength.BorderStyle = BorderStyle.FixedSingle;
            this.UDArrowLength.Location = new Point(200, 80);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDArrowLength.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDArrowLength.Minimum = new decimal(bits);
            this.UDArrowLength.Name = "UDArrowLength";
            this.UDArrowLength.Size = new Size(0x36, 20);
            this.UDArrowLength.TabIndex = 13;
            this.UDArrowLength.TextAlign = HorizontalAlignment.Right;
            this.UDArrowLength.ValueChanged += new EventHandler(this.UDArrowLength_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x98, 0x52);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x2a, 0x10);
            this.label5.TabIndex = 0x11;
            this.label5.Text = "&Length:";
            this.UDHeadSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDHeadSize.Location = new Point(0x4e, 0x70);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDHeadSize.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDHeadSize.Minimum = new decimal(bits);
            this.UDHeadSize.Name = "UDHeadSize";
            this.UDHeadSize.Size = new Size(0x30, 20);
            this.UDHeadSize.TabIndex = 12;
            this.UDHeadSize.TextAlign = HorizontalAlignment.Right;
            this.UDHeadSize.ValueChanged += new EventHandler(this.UDHeadSize_ValueChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0x2f, 0x72);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x1d, 0x10);
            this.label12.TabIndex = 15;
            this.label12.Text = "&Size:";
            this.label12.TextAlign = ContentAlignment.TopRight;
            this.CBHead.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBHead.Items.AddRange(new object[] { "None", "Line", "Solid" });
            this.CBHead.Location = new Point(0x18, 80);
            this.CBHead.Name = "CBHead";
            this.CBHead.Size = new Size(0x68, 0x15);
            this.CBHead.TabIndex = 11;
            this.CBHead.SelectedIndexChanged += new EventHandler(this.CBHead_SelectedIndexChanged);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x18, 0x40);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x41, 0x10);
            this.label11.TabIndex = 13;
            this.label11.Text = "&Arrow head:";
            this.UDArrowDist.BorderStyle = BorderStyle.FixedSingle;
            this.UDArrowDist.Location = new Point(200, 0x70);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDArrowDist.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDArrowDist.Minimum = new decimal(bits);
            this.UDArrowDist.Name = "UDArrowDist";
            this.UDArrowDist.Size = new Size(0x36, 20);
            this.UDArrowDist.TabIndex = 14;
            this.UDArrowDist.TextAlign = HorizontalAlignment.Right;
            this.UDArrowDist.ValueChanged += new EventHandler(this.UDArrowDist_ValueChanged);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x98, 0x72);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x33, 0x10);
            this.label10.TabIndex = 11;
            this.label10.Text = "&Distance:";
            this.BCalloutPointer.FlatStyle = FlatStyle.Flat;
            this.BCalloutPointer.Location = new Point(0x68, 0x17);
            this.BCalloutPointer.Name = "BCalloutPointer";
            this.BCalloutPointer.Size = new Size(0x48, 0x18);
            this.BCalloutPointer.TabIndex = 10;
            this.BCalloutPointer.Text = "&Pointer...";
            this.BCalloutPointer.Click += new EventHandler(this.BCalloutPointer_Click);
            this.BMarkLinCol.FlatStyle = FlatStyle.Flat;
            this.BMarkLinCol.Location = new Point(0x18, 0x17);
            this.BMarkLinCol.Name = "BMarkLinCol";
            this.BMarkLinCol.Size = new Size(0x48, 0x18);
            this.BMarkLinCol.TabIndex = 9;
            this.BMarkLinCol.Text = "&Border...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x120, 0xd5);
            base.Controls.Add(this.tabControl1);
            base.Name = "SeriesMarksEditor";
            this.Text = "SeriesMarks";
            this.tabControl1.ResumeLayout(false);
            this.tabStyle.ResumeLayout(false);
            this.udDrawEvery.EndInit();
            this.udAngle.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.udLength.EndInit();
            this.tabArrow.ResumeLayout(false);
            this.UDArrowLength.EndInit();
            this.UDHeadSize.EndInit();
            this.UDArrowDist.EndInit();
            base.ResumeLayout(false);
        }

        private void udAngle_ValueChanged(object sender, EventArgs e)
        {
            if (this.marks != null)
            {
                this.marks.Angle = (double) this.udAngle.Value;
            }
        }

        private void UDArrowDist_ValueChanged(object sender, EventArgs e)
        {
            this.marks.Callout.Distance = (int) this.UDArrowDist.Value;
        }

        private void UDArrowLength_ValueChanged(object sender, EventArgs e)
        {
            this.marks.Callout.Length = (int) this.UDArrowLength.Value;
        }

        private void udDrawEvery_ValueChanged(object sender, EventArgs e)
        {
            if (this.marks != null)
            {
                this.marks.DrawEvery = (int) this.udDrawEvery.Value;
            }
        }

        private void UDHeadSize_ValueChanged(object sender, EventArgs e)
        {
            this.marks.Callout.ArrowHeadSize = (int) this.UDHeadSize.Value;
        }

        private void udLength_ValueChanged(object sender, EventArgs e)
        {
            if (this.marks != null)
            {
                this.marks.ArrowLength = (int) this.udLength.Value;
            }
        }
    }
}

