namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GeneralEditor : Form
    {
        private Button BPrint;
        private Button BZoomColor;
        private ButtonPen BZoomPen;
        private CheckBox CBAllowZoom;
        private CheckBox CBAnimatedZoom;
        private ComboBox cbCursor;
        private ComboBox CBDir;
        private ComboBox CBScrollMouse;
        private ComboBox CBZoomMouse;
        private Chart chart;
        private Zoom chartZoom;
        private ComboBox comboBox1;
        private Container components;
        private GroupBox GBMargins;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label labelCursor;
        private TabControl PageControl1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private GroupBox RGPanning;
        private TabPage tabPage1;
        private TabPage TabSheet2;
        private NumericUpDown UDAniZoomSteps;
        private NumericUpDown UDBotMa;
        private NumericUpDown UDLeftMa;
        private NumericUpDown UDMinPix;
        private NumericUpDown UDRightMa;
        private NumericUpDown UDTopMa;

        public GeneralEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GeneralEditor(Chart c, Control parent) : this()
        {
            this.chart = c;
            this.chartZoom = c.Zoom;
            EditorUtils.InsertForm(this, parent);
            this.UDAniZoomSteps.Value = this.chart.Zoom.AnimatedSteps;
            this.UDBotMa.Value = (int) this.chart.Panel.MarginBottom;
            this.UDRightMa.Value = (int) this.chart.Panel.MarginRight;
            this.UDLeftMa.Value = (int) this.chart.Panel.MarginLeft;
            this.UDTopMa.Value = (int) this.chart.Panel.MarginTop;
            this.CBAllowZoom.Checked = this.chart.Zoom.Allow;
            this.CBAnimatedZoom.Checked = this.chart.Zoom.Animated;
            this.UDMinPix.Value = this.chart.Zoom.MinPixels;
            this.BZoomPen.Pen = this.chart.Zoom.Pen;
            this.CBDir.SelectedIndex = (int) this.chart.Zoom.Direction;
            this.CBZoomMouse.SelectedIndex = EditorUtils.MouseButtonIndex(this.chart.Zoom.MouseButton);
            if (this.chart.Panel.MarginUnits == PanelMarginUnits.Percent)
            {
                this.comboBox1.SelectedIndex = 0;
            }
            else
            {
                this.comboBox1.SelectedIndex = 1;
            }
            this.CBScrollMouse.SelectedIndex = EditorUtils.MouseButtonIndex(this.chart.Panning.MouseButton);
            switch (this.chart.Panning.Allow)
            {
                case ScrollModes.None:
                    this.radioButton1.Checked = true;
                    break;

                case ScrollModes.Vertical:
                    this.radioButton3.Checked = true;
                    break;

                case ScrollModes.Horizontal:
                    this.radioButton2.Checked = true;
                    break;

                case ScrollModes.Both:
                    this.radioButton4.Checked = true;
                    break;
            }
            if (this.chart.parent != null)
            {
                Cursor cursor = this.chart.parent.GetCursor();
                if (cursor == null)
                {
                    this.cbCursor.Visible = false;
                    this.labelCursor.Visible = false;
                }
                else
                {
                    EditorUtils.FillCursors(this.cbCursor, cursor);
                }
            }
        }

        private void BPrint_Click(object sender, EventArgs e)
        {
            PrintPreview.ShowModal(this.chart);
        }

        private void BZoomColor_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.chart.Zoom.Brush);
        }

        private void CBAllowZoom_CheckedChanged(object sender, EventArgs e)
        {
            this.chartZoom.Active = this.CBAllowZoom.Checked;
        }

        private void CBAnimatedZoom_CheckedChanged(object sender, EventArgs e)
        {
            this.chartZoom.Animated = this.CBAnimatedZoom.Checked;
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chart.parent.SetCursor(EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString()));
        }

        private void CBScrollMouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chart.Panning.MouseButton = EditorUtils.MouseButtonFromIndex(this.CBScrollMouse.SelectedIndex);
        }

        private void CBZoomMouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chart.Zoom.MouseButton = EditorUtils.MouseButtonFromIndex(this.CBZoomMouse.SelectedIndex);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == 0)
            {
                this.chart.Panel.MarginUnits = PanelMarginUnits.Percent;
                this.SetUpDownLimits(0, 100);
            }
            else
            {
                this.chart.Panel.MarginUnits = PanelMarginUnits.Pixels;
                this.SetUpDownLimits(0, 0x7d0);
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
            this.BPrint = new Button();
            this.GBMargins = new GroupBox();
            this.comboBox1 = new ComboBox();
            this.label6 = new Label();
            this.UDBotMa = new NumericUpDown();
            this.UDLeftMa = new NumericUpDown();
            this.UDTopMa = new NumericUpDown();
            this.UDRightMa = new NumericUpDown();
            this.PageControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.UDAniZoomSteps = new NumericUpDown();
            this.UDMinPix = new NumericUpDown();
            this.CBZoomMouse = new ComboBox();
            this.CBDir = new ComboBox();
            this.BZoomColor = new Button();
            this.BZoomPen = new ButtonPen();
            this.CBAnimatedZoom = new CheckBox();
            this.CBAllowZoom = new CheckBox();
            this.TabSheet2 = new TabPage();
            this.label1 = new Label();
            this.CBScrollMouse = new ComboBox();
            this.RGPanning = new GroupBox();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.cbCursor = new ComboBox();
            this.labelCursor = new Label();
            this.GBMargins.SuspendLayout();
            this.UDBotMa.BeginInit();
            this.UDLeftMa.BeginInit();
            this.UDTopMa.BeginInit();
            this.UDRightMa.BeginInit();
            this.PageControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.UDAniZoomSteps.BeginInit();
            this.UDMinPix.BeginInit();
            this.TabSheet2.SuspendLayout();
            this.RGPanning.SuspendLayout();
            base.SuspendLayout();
            this.BPrint.FlatStyle = FlatStyle.Flat;
            this.BPrint.Location = new Point(0x18, 10);
            this.BPrint.Name = "BPrint";
            this.BPrint.Size = new Size(0x70, 0x17);
            this.BPrint.TabIndex = 0;
            this.BPrint.Text = "Print &Preview...";
            this.BPrint.Click += new EventHandler(this.BPrint_Click);
            this.GBMargins.Controls.Add(this.comboBox1);
            this.GBMargins.Controls.Add(this.label6);
            this.GBMargins.Controls.Add(this.UDBotMa);
            this.GBMargins.Controls.Add(this.UDLeftMa);
            this.GBMargins.Controls.Add(this.UDTopMa);
            this.GBMargins.Controls.Add(this.UDRightMa);
            this.GBMargins.Location = new Point(0x10, 0x29);
            this.GBMargins.Name = "GBMargins";
            this.GBMargins.Size = new Size(0x88, 0x88);
            this.GBMargins.TabIndex = 1;
            this.GBMargins.TabStop = false;
            this.GBMargins.Text = "Margins (%)";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "Percent", "Pixels" });
            this.comboBox1.Location = new Point(14, 0x6b);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x68, 0x15);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(12, 0x58);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x21, 0x10);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Units:";
            this.UDBotMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDBotMa.Location = new Point(0x30, 0x3e);
            this.UDBotMa.Name = "UDBotMa";
            this.UDBotMa.Size = new Size(40, 20);
            this.UDBotMa.TabIndex = 3;
            this.UDBotMa.TextAlign = HorizontalAlignment.Right;
            int[] bits = new int[4];
            bits[0] = 3;
            this.UDBotMa.Value = new decimal(bits);
            this.UDBotMa.TextChanged += new EventHandler(this.UDBotMa_ValueChanged);
            this.UDBotMa.ValueChanged += new EventHandler(this.UDBotMa_ValueChanged);
            this.UDLeftMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDLeftMa.Location = new Point(8, 40);
            this.UDLeftMa.Name = "UDLeftMa";
            this.UDLeftMa.Size = new Size(40, 20);
            this.UDLeftMa.TabIndex = 1;
            this.UDLeftMa.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 3;
            this.UDLeftMa.Value = new decimal(bits);
            this.UDLeftMa.TextChanged += new EventHandler(this.SELeftMa_ValueChanged);
            this.UDLeftMa.ValueChanged += new EventHandler(this.SELeftMa_ValueChanged);
            this.UDTopMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDTopMa.Location = new Point(0x30, 0x10);
            this.UDTopMa.Name = "UDTopMa";
            this.UDTopMa.Size = new Size(40, 20);
            this.UDTopMa.TabIndex = 0;
            this.UDTopMa.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 3;
            this.UDTopMa.Value = new decimal(bits);
            this.UDTopMa.TextChanged += new EventHandler(this.UDTopMa_ValueChanged);
            this.UDTopMa.ValueChanged += new EventHandler(this.UDTopMa_ValueChanged);
            this.UDRightMa.BorderStyle = BorderStyle.FixedSingle;
            this.UDRightMa.Location = new Point(0x58, 40);
            this.UDRightMa.Name = "UDRightMa";
            this.UDRightMa.Size = new Size(40, 20);
            this.UDRightMa.TabIndex = 2;
            this.UDRightMa.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 3;
            this.UDRightMa.Value = new decimal(bits);
            this.UDRightMa.TextChanged += new EventHandler(this.UDRightMa_ValueChanged);
            this.UDRightMa.ValueChanged += new EventHandler(this.UDRightMa_ValueChanged);
            this.PageControl1.Controls.Add(this.tabPage1);
            this.PageControl1.Controls.Add(this.TabSheet2);
            this.PageControl1.HotTrack = true;
            this.PageControl1.Location = new Point(160, 8);
            this.PageControl1.Name = "PageControl1";
            this.PageControl1.SelectedIndex = 0;
            this.PageControl1.Size = new Size(0xd0, 0xd0);
            this.PageControl1.TabIndex = 2;
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.UDAniZoomSteps);
            this.tabPage1.Controls.Add(this.UDMinPix);
            this.tabPage1.Controls.Add(this.CBZoomMouse);
            this.tabPage1.Controls.Add(this.CBDir);
            this.tabPage1.Controls.Add(this.BZoomColor);
            this.tabPage1.Controls.Add(this.BZoomPen);
            this.tabPage1.Controls.Add(this.CBAnimatedZoom);
            this.tabPage1.Controls.Add(this.CBAllowZoom);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(200, 0xb6);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Zoom";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(11, 0x9a);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4d, 0x10);
            this.label5.TabIndex = 10;
            this.label5.Text = "&Mouse Button:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x24, 130);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x34, 0x10);
            this.label4.TabIndex = 8;
            this.label4.Text = "&Direction:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(60, 0x6a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x56, 0x10);
            this.label3.TabIndex = 6;
            this.label3.Text = "&Minimum pixels:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x6c, 0x29);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Steps:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDAniZoomSteps.BorderStyle = BorderStyle.FixedSingle;
            this.UDAniZoomSteps.Location = new Point(0x94, 0x27);
            this.UDAniZoomSteps.Name = "UDAniZoomSteps";
            this.UDAniZoomSteps.Size = new Size(40, 20);
            this.UDAniZoomSteps.TabIndex = 3;
            this.UDAniZoomSteps.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 8;
            this.UDAniZoomSteps.Value = new decimal(bits);
            this.UDAniZoomSteps.TextChanged += new EventHandler(this.UDAniZoomSteps_ValueChanged);
            this.UDAniZoomSteps.ValueChanged += new EventHandler(this.UDAniZoomSteps_ValueChanged);
            this.UDMinPix.BorderStyle = BorderStyle.FixedSingle;
            this.UDMinPix.Location = new Point(0x94, 0x68);
            this.UDMinPix.Name = "UDMinPix";
            this.UDMinPix.Size = new Size(40, 20);
            this.UDMinPix.TabIndex = 7;
            this.UDMinPix.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 0x10;
            this.UDMinPix.Value = new decimal(bits);
            this.UDMinPix.TextChanged += new EventHandler(this.UDMinPix_ValueChanged);
            this.UDMinPix.ValueChanged += new EventHandler(this.UDMinPix_ValueChanged);
            this.CBZoomMouse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBZoomMouse.Items.AddRange(new object[] { "Left", "Middle", "Right", "X Button 1", "X Button 2" });
            this.CBZoomMouse.Location = new Point(0x5c, 0x98);
            this.CBZoomMouse.Name = "CBZoomMouse";
            this.CBZoomMouse.Size = new Size(0x60, 0x15);
            this.CBZoomMouse.TabIndex = 11;
            this.CBZoomMouse.SelectedIndexChanged += new EventHandler(this.CBZoomMouse_SelectedIndexChanged);
            this.CBDir.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBDir.Items.AddRange(new object[] { "Horizontal", "Vertical", "Both" });
            this.CBDir.Location = new Point(0x5c, 0x80);
            this.CBDir.Name = "CBDir";
            this.CBDir.Size = new Size(0x60, 0x15);
            this.CBDir.TabIndex = 9;
            this.BZoomColor.FlatStyle = FlatStyle.Flat;
            this.BZoomColor.Location = new Point(0x6c, 0x48);
            this.BZoomColor.Name = "BZoomColor";
            this.BZoomColor.Size = new Size(80, 0x17);
            this.BZoomColor.TabIndex = 5;
            this.BZoomColor.Text = "P&attern...";
            this.BZoomColor.Click += new EventHandler(this.BZoomColor_Click);
            this.BZoomPen.FlatStyle = FlatStyle.Flat;
            this.BZoomPen.Location = new Point(12, 0x48);
            this.BZoomPen.Name = "BZoomPen";
            this.BZoomPen.Size = new Size(80, 0x17);
            this.BZoomPen.TabIndex = 4;
            this.BZoomPen.Text = "P&en...";
            this.CBAnimatedZoom.FlatStyle = FlatStyle.Flat;
            this.CBAnimatedZoom.Location = new Point(12, 40);
            this.CBAnimatedZoom.Name = "CBAnimatedZoom";
            this.CBAnimatedZoom.Size = new Size(0x5c, 0x10);
            this.CBAnimatedZoom.TabIndex = 1;
            this.CBAnimatedZoom.Text = "An&imated";
            this.CBAnimatedZoom.CheckedChanged += new EventHandler(this.CBAnimatedZoom_CheckedChanged);
            this.CBAllowZoom.FlatStyle = FlatStyle.Flat;
            this.CBAllowZoom.Location = new Point(12, 0x10);
            this.CBAllowZoom.Name = "CBAllowZoom";
            this.CBAllowZoom.Size = new Size(80, 0x10);
            this.CBAllowZoom.TabIndex = 0;
            this.CBAllowZoom.Text = "&Allow";
            this.CBAllowZoom.CheckedChanged += new EventHandler(this.CBAllowZoom_CheckedChanged);
            this.TabSheet2.Controls.Add(this.label1);
            this.TabSheet2.Controls.Add(this.CBScrollMouse);
            this.TabSheet2.Controls.Add(this.RGPanning);
            this.TabSheet2.Location = new Point(4, 0x16);
            this.TabSheet2.Name = "TabSheet2";
            this.TabSheet2.Size = new Size(200, 0xb6);
            this.TabSheet2.TabIndex = 1;
            this.TabSheet2.Text = "Scroll";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 0x90);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Mouse Button:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBScrollMouse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBScrollMouse.Items.AddRange(new object[] { "Left", "Middle", "Right", "X Button 1", "X Button 2" });
            this.CBScrollMouse.Location = new Point(0x5d, 140);
            this.CBScrollMouse.Name = "CBScrollMouse";
            this.CBScrollMouse.Size = new Size(0x60, 0x15);
            this.CBScrollMouse.TabIndex = 2;
            this.CBScrollMouse.SelectedIndexChanged += new EventHandler(this.CBScrollMouse_SelectedIndexChanged);
            this.RGPanning.Controls.Add(this.radioButton4);
            this.RGPanning.Controls.Add(this.radioButton3);
            this.RGPanning.Controls.Add(this.radioButton2);
            this.RGPanning.Controls.Add(this.radioButton1);
            this.RGPanning.Location = new Point(40, 0x10);
            this.RGPanning.Name = "RGPanning";
            this.RGPanning.Size = new Size(120, 0x70);
            this.RGPanning.TabIndex = 0;
            this.RGPanning.TabStop = false;
            this.RGPanning.Text = "Allow Scroll:";
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(8, 0x58);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x60, 0x10);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "&Both";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(8, 0x40);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x60, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&Vertical";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 40);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x60, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Horizontal";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x60, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&None";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.cbCursor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursor.Location = new Point(0x38, 0xb8);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x60, 0x15);
            this.cbCursor.TabIndex = 4;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.labelCursor.AutoSize = true;
            this.labelCursor.Location = new Point(0x10, 0xba);
            this.labelCursor.Name = "labelCursor";
            this.labelCursor.Size = new Size(0x29, 0x10);
            this.labelCursor.TabIndex = 3;
            this.labelCursor.Text = "&Cursor:";
            this.labelCursor.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x170, 0xda);
            base.Controls.Add(this.cbCursor);
            base.Controls.Add(this.labelCursor);
            base.Controls.Add(this.PageControl1);
            base.Controls.Add(this.GBMargins);
            base.Controls.Add(this.BPrint);
            base.Name = "GeneralEditor";
            this.GBMargins.ResumeLayout(false);
            this.UDBotMa.EndInit();
            this.UDLeftMa.EndInit();
            this.UDTopMa.EndInit();
            this.UDRightMa.EndInit();
            this.PageControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.UDAniZoomSteps.EndInit();
            this.UDMinPix.EndInit();
            this.TabSheet2.ResumeLayout(false);
            this.RGPanning.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.Horizontal;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.Vertical;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Panning.Allow = ScrollModes.Both;
        }

        private void SELeftMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Panel.MarginLeft = Convert.ToDouble(this.UDLeftMa.Value);
            }
        }

        private void SetUpDownLimits(int min, int max)
        {
            this.UDBotMa.Minimum = min;
            this.UDBotMa.Maximum = max;
            this.UDTopMa.Minimum = min;
            this.UDTopMa.Maximum = max;
            this.UDLeftMa.Minimum = min;
            this.UDLeftMa.Maximum = max;
            this.UDRightMa.Minimum = min;
            this.UDRightMa.Maximum = max;
        }

        private void UDAniZoomSteps_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chartZoom.AnimatedSteps = (int) this.UDAniZoomSteps.Value;
            }
        }

        private void UDBotMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Panel.MarginBottom = Convert.ToDouble(this.UDBotMa.Value);
            }
        }

        private void UDMinPix_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Zoom.MinPixels = (int) this.UDMinPix.Value;
            }
        }

        private void UDRightMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Panel.MarginRight = Convert.ToDouble(this.UDRightMa.Value);
            }
        }

        private void UDTopMa_ValueChanged(object sender, EventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Panel.MarginTop = Convert.ToDouble(this.UDTopMa.Value);
            }
        }
    }
}

