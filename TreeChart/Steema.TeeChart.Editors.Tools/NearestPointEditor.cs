namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class NearestPointEditor : ToolSeriesEditor
    {
        private Button button1;
        private ButtonPen button2;
        private CheckBox cbDrawLine;
        private ComboBox cbStyle;
        private IContainer components;
        private Label label2;
        private Label label3;
        private NumericUpDown ndSize;
        private NearestPoint tool;

        public NearestPointEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public NearestPointEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (NearestPoint) t;
            base.SetTool(this.tool);
            this.cbDrawLine.Checked = this.tool.DrawLine;
            this.cbStyle.SelectedIndex = (int) this.tool.Style;
            this.ndSize.Value = this.tool.Size;
            this.button2.Pen = this.tool.Pen;
            base.setting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Brush);
        }

        private void cbDrawLine_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.DrawLine = this.cbDrawLine.Checked;
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Style = (NearestPointStyles) this.cbStyle.SelectedIndex;
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
            this.button1 = new Button();
            this.button2 = new ButtonPen();
            this.cbDrawLine = new CheckBox();
            this.cbStyle = new ComboBox();
            this.label2 = new Label();
            this.label3 = new Label();
            this.ndSize = new NumericUpDown();
            this.ndSize.BeginInit();
            base.SuspendLayout();
            base.CBSeries.ItemHeight = 13;
            base.CBSeries.Name = "CBSeries";
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x30, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Fill...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x88, 40);
            this.button2.Name = "button2";
            this.button2.Size = new Size(80, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Border...";
            this.cbDrawLine.FlatStyle = FlatStyle.Flat;
            this.cbDrawLine.Location = new Point(0x35, 0x48);
            this.cbDrawLine.Name = "cbDrawLine";
            this.cbDrawLine.TabIndex = 4;
            this.cbDrawLine.Text = "&Draw Line";
            this.cbDrawLine.CheckedChanged += new EventHandler(this.cbDrawLine_CheckedChanged);
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.AddRange(new object[] { "None", "Circle", "Rectangle", "Diamond" });
            this.cbStyle.Location = new Point(0x60, 0x69);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x79, 0x15);
            this.cbStyle.TabIndex = 6;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x3b, 0x6b);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 0x10);
            this.label2.TabIndex = 5;
            this.label2.Text = "S&tyle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x7d, 0x8a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1d, 0x10);
            this.label3.TabIndex = 7;
            this.label3.Text = "S&ize:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.ndSize.BorderStyle = BorderStyle.FixedSingle;
            this.ndSize.Location = new Point(0x9f, 0x88);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.ndSize.Maximum = new decimal(bits);
            this.ndSize.Name = "ndSize";
            this.ndSize.Size = new Size(0x38, 20);
            this.ndSize.TabIndex = 8;
            this.ndSize.TextAlign = HorizontalAlignment.Right;
            this.ndSize.ValueChanged += new EventHandler(this.ndSize_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.ndSize);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cbStyle);
            base.Controls.Add(this.cbDrawLine);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Name = "NearestPointEditor";
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(this.button2, 0);
            base.Controls.SetChildIndex(this.cbDrawLine, 0);
            base.Controls.SetChildIndex(this.cbStyle, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.ndSize, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.ndSize.EndInit();
            base.ResumeLayout(false);
        }

        private void ndSize_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Size = (int) this.ndSize.Value;
            }
        }
    }
}

