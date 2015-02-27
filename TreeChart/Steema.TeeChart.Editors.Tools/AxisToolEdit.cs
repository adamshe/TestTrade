namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AxisToolEdit : Form
    {
        protected ButtonPen BPen;
        private ComboBox CBAxis;
        private Container components = null;
        private Label label1;
        private bool setting;
        private ToolAxis tool;

        public AxisToolEdit()
        {
            this.InitializeComponent();
        }

        private void CBAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                if (this.CBAxis.SelectedIndex <= 0)
                {
                    this.tool.Axis = null;
                }
                else
                {
                    this.tool.Axis = this.tool.Chart.Axes[this.CBAxis.SelectedIndex - 1];
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
            this.label1 = new Label();
            this.CBAxis = new ComboBox();
            this.BPen = new ButtonPen();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x29, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Axis:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBAxis.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBAxis.Location = new Point(0x48, 8);
            this.CBAxis.Name = "CBAxis";
            this.CBAxis.Size = new Size(0x88, 0x15);
            this.CBAxis.TabIndex = 1;
            this.CBAxis.SelectedIndexChanged += new EventHandler(this.CBAxis_SelectedIndexChanged);
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(0x48, 40);
            this.BPen.Name = "BPen";
            this.BPen.Size = new Size(80, 0x17);
            this.BPen.TabIndex = 2;
            this.BPen.Text = "&Border...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe8, 0x8f);
            base.Controls.Add(this.BPen);
            base.Controls.Add(this.CBAxis);
            base.Controls.Add(this.label1);
            base.Name = "AxisToolEdit";
            base.ResumeLayout(false);
        }

        internal void SetTool(ToolAxis t)
        {
            this.setting = true;
            this.tool = t;
            this.CBAxis.Items.Clear();
            this.CBAxis.Items.Add(Texts.None);
            this.CBAxis.Items.AddRange(this.tool.chart.Axes.StringItems());
            if (this.tool.Axis == null)
            {
                this.CBAxis.SelectedIndex = 0;
            }
            else
            {
                this.CBAxis.SelectedIndex = 1 + this.tool.chart.axes.IndexOf(this.tool.Axis);
            }
            this.BPen.Pen = this.tool.Pen;
            this.setting = false;
        }
    }
}

