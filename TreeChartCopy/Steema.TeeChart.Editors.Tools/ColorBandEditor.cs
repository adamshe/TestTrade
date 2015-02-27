namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ColorBandEditor : AxisToolEdit
    {
        private Button button1;
        private Button button2;
        private ButtonColor button3;
        private CheckBox cbBehind;
        private IContainer components;
        private TextBox eEnd;
        private TextBox eStart;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown ndTransp;
        private bool setting;
        private ColorBand tool;

        public ColorBandEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ColorBandEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.setting = true;
            this.tool = (ColorBand) t;
            base.SetTool(this.tool);
            this.ndTransp.Value = this.tool.Transparency;
            this.eStart.Text = this.tool.Start.ToString();
            this.eEnd.Text = this.tool.End.ToString();
            this.cbBehind.Checked = this.tool.DrawBehind;
            this.button3.Color = this.tool.Color;
            this.setting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Brush, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.tool.Gradient);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.tool.Color = this.button3.Color;
        }

        private void cbBehind_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.DrawBehind = this.cbBehind.Checked;
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

        private void eEnd_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.End = Convert.ToDouble(this.eEnd.Text);
            }
        }

        private void eStart_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Start = Convert.ToDouble(this.eStart.Text);
            }
        }

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.eStart = new TextBox();
            this.eEnd = new TextBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.ndTransp = new NumericUpDown();
            this.cbBehind = new CheckBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new ButtonColor();
            this.ndTransp.BeginInit();
            base.SuspendLayout();
            base.BPen.Name = "BPen";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x47, 0x68);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3f, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Start Value:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.eStart.BorderStyle = BorderStyle.FixedSingle;
            this.eStart.Location = new Point(0x87, 0x66);
            this.eStart.Name = "eStart";
            this.eStart.TabIndex = 4;
            this.eStart.Text = "";
            this.eStart.TextChanged += new EventHandler(this.eStart_TextChanged);
            this.eEnd.BorderStyle = BorderStyle.FixedSingle;
            this.eEnd.Location = new Point(0x87, 0x80);
            this.eEnd.Name = "eEnd";
            this.eEnd.TabIndex = 6;
            this.eEnd.Text = "";
            this.eEnd.TextChanged += new EventHandler(this.eEnd_TextChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x47, 130);
            this.label3.Name = "label3";
            this.label3.Size = new Size(60, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "&End Value:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x63, 0x9f);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4d, 0x10);
            this.label4.TabIndex = 7;
            this.label4.Text = "&Transparency:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.ndTransp.BorderStyle = BorderStyle.FixedSingle;
            this.ndTransp.Location = new Point(0xb2, 0x9d);
            this.ndTransp.Name = "ndTransp";
            this.ndTransp.Size = new Size(0x38, 20);
            this.ndTransp.TabIndex = 8;
            this.ndTransp.TextAlign = HorizontalAlignment.Right;
            this.ndTransp.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.ndTransp.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.cbBehind.FlatStyle = FlatStyle.Flat;
            this.cbBehind.Location = new Point(0x48, 0xb8);
            this.cbBehind.Name = "cbBehind";
            this.cbBehind.TabIndex = 9;
            this.cbBehind.Text = "Draw &Behind";
            this.cbBehind.CheckedChanged += new EventHandler(this.cbBehind_CheckedChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x9b, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 10;
            this.button1.Text = "&Pattern...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x48, 0x47);
            this.button2.Name = "button2";
            this.button2.Size = new Size(80, 0x17);
            this.button2.TabIndex = 11;
            this.button2.Text = "&Gradient...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.Color = Color.Empty;
            this.button3.Location = new Point(0x9b, 0x47);
            this.button3.Name = "button3";
            this.button3.Size = new Size(80, 0x17);
            this.button3.TabIndex = 12;
            this.button3.Text = "&Color...";
            this.button3.Click += new EventHandler(this.button3_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0xd5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.cbBehind);
            base.Controls.Add(this.ndTransp);
            base.Controls.Add(this.eEnd);
            base.Controls.Add(this.eStart);
            base.Name = "ColorBandEditor";
            base.Controls.SetChildIndex(base.BPen, 0);
            base.Controls.SetChildIndex(this.eStart, 0);
            base.Controls.SetChildIndex(this.eEnd, 0);
            base.Controls.SetChildIndex(this.ndTransp, 0);
            base.Controls.SetChildIndex(this.cbBehind, 0);
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(this.button2, 0);
            base.Controls.SetChildIndex(this.button3, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            this.ndTransp.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Transparency = (int) this.ndTransp.Value;
            }
        }
    }
}

