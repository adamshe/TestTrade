namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PieToolEditor : Form
    {
        private ButtonPen buttonPen1;
        private ComboBox cbSeries;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private PieTool tool;

        public PieToolEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PieToolEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.tool = (PieTool) s;
            if (this.tool.Style == PieToolStyle.Focus)
            {
                this.radioButton1.Checked = true;
            }
            else
            {
                this.radioButton2.Checked = true;
            }
            this.buttonPen1.Pen = this.tool.Pen;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tool.Series = (Series) this.cbSeries.SelectedItem;
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
            this.label1 = new Label();
            this.cbSeries = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.buttonPen1 = new ButtonPen();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x3b, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Pie Series:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.cbSeries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbSeries.Location = new Point(0x4c, 4);
            this.cbSeries.Name = "cbSeries";
            this.cbSeries.Size = new Size(0x79, 0x15);
            this.cbSeries.TabIndex = 1;
            this.cbSeries.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(10, 0x23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xb8, 0x30);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Style:";
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x58, 0x10);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(80, 0x18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Explode";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x40, 0x18);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&Focus";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(11, 0x5d);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.TabIndex = 3;
            this.buttonPen1.Text = "&Border...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd0, 0x7d);
            base.Controls.Add(this.buttonPen1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.cbSeries);
            base.Controls.Add(this.label1);
            base.Name = "PieToolEditor";
            this.Text = "PieToolEditor";
            base.Load += new EventHandler(this.PieToolEditor_Load);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void PieToolEditor_Load(object sender, EventArgs e)
        {
            if (this.tool != null)
            {
                foreach (Series series in this.tool.Chart.Series)
                {
                    if (series is Pie)
                    {
                        this.cbSeries.Items.Add(series);
                    }
                }
                this.cbSeries.Enabled = this.tool.Chart.Series.Count > 0;
                this.cbSeries.SelectedItem = this.tool.Series;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.Style = PieToolStyle.Focus;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.Style = PieToolStyle.Explode;
        }
    }
}

