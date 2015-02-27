namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WallEditor : Form
    {
        private Button button1;
        private ButtonPen button2;
        private Button button3;
        private ButtonColor button8;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private Container components;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        public Steema.TeeChart.Wall Wall;

        public WallEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public WallEditor(Steema.TeeChart.Wall w, Control parent) : this()
        {
            this.Wall = w;
            this.checkBox1.Checked = this.Wall.Visible;
            this.checkBox2.Checked = this.Wall.Transparent;
            this.checkBox3.Checked = this.Wall.ApplyDark;
            this.numericUpDown1.Value = this.Wall.Size;
            this.numericUpDown2.Value = this.Wall.Transparency;
            this.button8.Color = this.Wall.Color;
            this.button2.Pen = this.Wall.Pen;
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.Wall.Gradient);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.Wall.Brush);
            this.numericUpDown2.Value = this.Wall.Brush.Transparency;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Wall.Color = this.button8.Color;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Wall.Visible = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Wall.Transparent = this.checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.Wall.ApplyDark = this.checkBox3.Checked;
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
            this.checkBox1 = new CheckBox();
            this.checkBox2 = new CheckBox();
            this.button8 = new ButtonColor();
            this.button1 = new Button();
            this.button2 = new ButtonPen();
            this.button3 = new Button();
            this.checkBox3 = new CheckBox();
            this.label1 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.numericUpDown2 = new NumericUpDown();
            this.label2 = new Label();
            this.numericUpDown1.BeginInit();
            this.numericUpDown2.BeginInit();
            base.SuspendLayout();
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x68, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x80, 0x18);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "&Visible";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(0x68, 0x5d);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "&Transparent";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.button8.Color = Color.Empty;
            this.button8.Location = new Point(8, 8);
            this.button8.Name = "button8";
            this.button8.TabIndex = 0;
            this.button8.Text = "&Color...";
            this.button8.Click += new EventHandler(this.button8_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 110);
            this.button1.Name = "button1";
            this.button1.TabIndex = 8;
            this.button1.Text = "&Gradient...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(8, 0x2a);
            this.button2.Name = "button2";
            this.button2.TabIndex = 2;
            this.button2.Text = "&Border...";
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.Location = new Point(8, 0x4c);
            this.button3.Name = "button3";
            this.button3.TabIndex = 4;
            this.button3.Text = "&Pattern...";
            this.button3.Click += new EventHandler(this.button3_Click);
            this.checkBox3.Enabled = false;
            this.checkBox3.FlatStyle = FlatStyle.Flat;
            this.checkBox3.Location = new Point(0x68, 0x26);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x80, 0x18);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "&Dark 3D";
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x68, 0x48);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2f, 0x10);
            this.label1.TabIndex = 5;
            this.label1.Text = "&Size 3D:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x9c, 70);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x30, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown2.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown2.Location = new Point(0xb8, 120);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x30, 20);
            this.numericUpDown2.TabIndex = 10;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x68, 0x7a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4d, 0x10);
            this.label2.TabIndex = 9;
            this.label2.Text = "&Transparency:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf8, 0x9d);
            base.Controls.Add(this.numericUpDown2);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.checkBox3);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.checkBox2);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.button8);
            base.Name = "WallEditor";
            this.Text = "Wall Editor";
            this.numericUpDown1.EndInit();
            this.numericUpDown2.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.Wall.Size = (int) this.numericUpDown1.Value;
            this.checkBox3.Enabled = this.Wall.Size > 0;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.Wall.Transparency = (int) this.numericUpDown2.Value;
        }
    }
}

