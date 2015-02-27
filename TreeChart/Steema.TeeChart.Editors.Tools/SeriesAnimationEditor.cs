namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesAnimationEditor : ToolSeriesEditor
    {
        private Button button1;
        private CheckBox cbStartMin;
        private IContainer components;
        private TextBox eStart;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown numericUpDown1;
        private SeriesAnimation tool;
        private TrackBar trackBar1;

        public SeriesAnimationEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public SeriesAnimationEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = (SeriesAnimation) t;
            base.SetTool(this.tool);
            if (this.tool != null)
            {
                this.trackBar1.Value = this.tool.Steps;
                this.label3.Text = this.tool.Steps.ToString();
                this.cbStartMin.Checked = this.tool.StartAtMin;
                this.eStart.Text = this.tool.StartValue.ToString();
                this.eStart.Enabled = !this.cbStartMin.Checked;
                this.numericUpDown1.Value = this.tool.DrawEvery;
                this.button1.Enabled = this.tool.Series != null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            try
            {
                this.tool.Execute();
            }
            finally
            {
                this.button1.Enabled = true;
            }
        }

        private void CBSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = this.tool.Series != null;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.StartAtMin = this.cbStartMin.Checked;
            this.eStart.Enabled = !this.cbStartMin.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void eStart_TextChanged(object sender, EventArgs e)
        {
            this.tool.StartValue = double.Parse(this.eStart.Text);
        }

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.trackBar1 = new TrackBar();
            this.label3 = new Label();
            this.cbStartMin = new CheckBox();
            this.label4 = new Label();
            this.eStart = new TextBox();
            this.label5 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.button1 = new Button();
            this.trackBar1.BeginInit();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            base.CBSeries.Name = "CBSeries";
            base.CBSeries.SelectedIndexChanged += new EventHandler(this.CBSeries_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x22, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "S&teps:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 50;
            this.trackBar1.Location = new Point(0x45, 0x25);
            this.trackBar1.Maximum = 0x3e8;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(0x91, 0x20);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new EventHandler(this.trackBar1_Scroll);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xd9, 40);
            this.label3.Name = "label3";
            this.label3.Size = new Size(10, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "0";
            this.cbStartMin.FlatStyle = FlatStyle.Flat;
            this.cbStartMin.Location = new Point(0x37, 0x48);
            this.cbStartMin.Name = "cbStartMin";
            this.cbStartMin.Size = new Size(0xb1, 0x18);
            this.cbStartMin.TabIndex = 5;
            this.cbStartMin.Text = "Start at &min. value";
            this.cbStartMin.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(30, 0x68);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x3d, 0x10);
            this.label4.TabIndex = 6;
            this.label4.Text = "Start &value:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.eStart.BorderStyle = BorderStyle.FixedSingle;
            this.eStart.Location = new Point(0x5e, 0x68);
            this.eStart.Name = "eStart";
            this.eStart.Size = new Size(120, 20);
            this.eStart.TabIndex = 7;
            this.eStart.Text = "";
            this.eStart.TextChanged += new EventHandler(this.eStart_TextChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(30, 0x88);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x40, 0x10);
            this.label5.TabIndex = 8;
            this.label5.Text = "Dr&aw every:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x5e, 0x86);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x48, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x5e, 0xa8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Execute";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0xcd);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.eStart);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cbStartMin);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.trackBar1);
            base.Controls.Add(this.label2);
            base.Name = "SeriesAnimationEditor";
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.trackBar1, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.cbStartMin, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.eStart, 0);
            base.Controls.SetChildIndex(this.label5, 0);
            base.Controls.SetChildIndex(this.numericUpDown1, 0);
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.trackBar1.EndInit();
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.tool.DrawEvery = (int) this.numericUpDown1.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.tool.Steps = this.trackBar1.Value;
            this.label3.Text = this.tool.Steps.ToString();
        }
    }
}

