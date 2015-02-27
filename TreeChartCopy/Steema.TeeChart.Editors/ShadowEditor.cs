namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ShadowEditor : Form
    {
        private ButtonColor button1;
        private Button button2;
        private CheckBox checkBox1;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Shadow shadow;
        private NumericUpDown udTransp;

        public ShadowEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ShadowEditor(Shadow s, Control parent) : this()
        {
            this.UpdateDialog(s);
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.shadow.Color = this.button1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.shadow.Brush);
            this.udTransp.Value = this.shadow.Brush.Transparency;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.shadow.Visible = this.checkBox1.Checked;
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
            this.numericUpDown2 = new NumericUpDown();
            this.label1 = new Label();
            this.label2 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.button1 = new ButtonColor();
            this.groupBox1 = new GroupBox();
            this.checkBox1 = new CheckBox();
            this.button2 = new Button();
            this.label3 = new Label();
            this.udTransp = new NumericUpDown();
            this.numericUpDown2.BeginInit();
            this.numericUpDown1.BeginInit();
            this.groupBox1.SuspendLayout();
            this.udTransp.BeginInit();
            base.SuspendLayout();
            this.numericUpDown2.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown2.Location = new Point(0x43, 0x2b);
            int[] bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.numericUpDown2.Minimum = new decimal(bits);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x23, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x3a, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Horizontal:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x15, 0x2d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2d, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Vertical:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x43, 0x11);
            bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.numericUpDown1.Minimum = new decimal(bits);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x23, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(0x84, 5);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(3, 0x12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 70);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size:";
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(3, 1);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x40, 0x10);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "&Visible";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x84, 0x25);
            this.button2.Name = "button2";
            this.button2.Size = new Size(80, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Pattern...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x7d, 0x44);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4d, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Transparency:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.udTransp.BorderStyle = BorderStyle.FixedSingle;
            this.udTransp.Location = new Point(0xcc, 0x42);
            this.udTransp.Name = "udTransp";
            this.udTransp.Size = new Size(40, 20);
            this.udTransp.TabIndex = 5;
            this.udTransp.TextAlign = HorizontalAlignment.Right;
            this.udTransp.TextChanged += new EventHandler(this.ndTransp_ValueChanged);
            this.udTransp.ValueChanged += new EventHandler(this.ndTransp_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf8, 0x5d);
            base.Controls.Add(this.udTransp);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.groupBox1);
            base.Name = "ShadowEditor";
            this.Text = "Shadow Editor";
            this.numericUpDown2.EndInit();
            this.numericUpDown1.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.udTransp.EndInit();
            base.ResumeLayout(false);
        }

        private void ndTransp_ValueChanged(object sender, EventArgs e)
        {
            this.shadow.Transparency = (int) this.udTransp.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.shadow.Width = (int) this.numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.shadow.Height = (int) this.numericUpDown2.Value;
        }

        internal void RefreshControls(Shadow s)
        {
            this.UpdateDialog(s);
        }

        private void UpdateDialog(Shadow s)
        {
            this.shadow = s;
            this.numericUpDown1.Value = s.Width;
            this.numericUpDown2.Value = s.Height;
            this.checkBox1.Checked = s.Visible;
            this.udTransp.Value = s.Transparency;
            this.button1.Color = this.shadow.Color;
        }
    }
}

