namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class GradientEditor : Form
    {
        private ButtonColor bMiddle;
        private ButtonColor button1;
        private ButtonColor button2;
        private Button button3;
        private CheckBox cbNoMiddle;
        internal CheckBox cbVisible;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private ComboBox comboBox1;
        private Container components;
        private Steema.TeeChart.Drawing.Gradient gradient;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown ndAngle;
        private NumericUpDown ndTransp;
        private System.Windows.Forms.Panel panel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TrackBar trackBar1;
        private TrackBar trackBar2;

        public GradientEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GradientEditor(Steema.TeeChart.Drawing.Gradient g) : this()
        {
            this.SetGradient(g);
        }

        public GradientEditor(Steema.TeeChart.Drawing.Gradient g, Control parent) : this()
        {
            this.SetGradient(g);
            this.panel1.Visible = false;
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.gradient.StartColor = this.button1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.gradient.EndColor = this.button2.Color;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.gradient.MiddleColor = this.bMiddle.Color;
            this.cbNoMiddle.Checked = !this.gradient.UseMiddle;
        }

        private void cbNoMiddle_CheckedChanged(object sender, EventArgs e)
        {
            this.gradient.UseMiddle = !this.cbNoMiddle.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.gradient.Visible = this.cbVisible.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.gradient.Sigma = this.checkBox2.Checked;
            this.EnableSigma();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.gradient.GammaCorrection = this.checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.gradient.Polygonal = this.checkBox4.Checked;
            this.gradient.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.gradient.Direction = LinearGradientMode.BackwardDiagonal;
                    break;

                case 1:
                    this.gradient.Direction = LinearGradientMode.ForwardDiagonal;
                    break;

                case 2:
                    this.gradient.Direction = LinearGradientMode.Horizontal;
                    break;

                case 3:
                    this.gradient.Direction = LinearGradientMode.Vertical;
                    break;
            }
            this.gradient.Angle = 0.0;
            this.ndAngle.Value = (int) this.gradient.Angle;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static void Edit(Steema.TeeChart.Drawing.Gradient g)
        {
            using (GradientEditor editor = new GradientEditor(g))
            {
                EditorUtils.Translate(editor);
                editor.ShowDialog();
            }
        }

        private void EnableSigma()
        {
            this.trackBar1.Enabled = this.gradient.Sigma;
            this.trackBar2.Enabled = this.gradient.Sigma;
            this.bMiddle.Enabled = !this.gradient.Sigma;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage3 = new TabPage();
            this.ndAngle = new NumericUpDown();
            this.label5 = new Label();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.cbVisible = new CheckBox();
            this.tabPage1 = new TabPage();
            this.ndTransp = new NumericUpDown();
            this.label4 = new Label();
            this.cbNoMiddle = new CheckBox();
            this.checkBox3 = new CheckBox();
            this.bMiddle = new ButtonColor();
            this.button2 = new ButtonColor();
            this.button1 = new ButtonColor();
            this.tabPage2 = new TabPage();
            this.checkBox4 = new CheckBox();
            this.label3 = new Label();
            this.checkBox2 = new CheckBox();
            this.trackBar2 = new TrackBar();
            this.label2 = new Label();
            this.trackBar1 = new TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new Button();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.ndAngle.BeginInit();
            this.tabPage1.SuspendLayout();
            this.ndTransp.BeginInit();
            this.tabPage2.SuspendLayout();
            this.trackBar2.BeginInit();
            this.trackBar1.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(250, 0xe7);
            this.tabControl1.TabIndex = 0x11;
            this.tabPage3.Controls.Add(this.ndAngle);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.comboBox1);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.cbVisible);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0xf2, 0xcd);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Format";
            this.ndAngle.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.ndAngle.Increment = new decimal(bits);
            this.ndAngle.Location = new Point(0x40, 80);
            bits = new int[4];
            bits[0] = 360;
            this.ndAngle.Maximum = new decimal(bits);
            this.ndAngle.Name = "ndAngle";
            this.ndAngle.Size = new Size(0x40, 20);
            this.ndAngle.TabIndex = 5;
            this.ndAngle.TextAlign = HorizontalAlignment.Right;
            this.ndAngle.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged_1);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x18, 80);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x24, 0x10);
            this.label5.TabIndex = 4;
            this.label5.Text = "&Angle:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.DropDownWidth = 0x79;
            this.comboBox1.Items.AddRange(new object[] { "Backward diagonal", "Forward diagonal", "Horizontal", "Vertical" });
            this.comboBox1.Location = new Point(8, 0x30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x79, 0x15);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.FlatStyle = FlatStyle.Flat;
            this.label1.Location = new Point(8, 0x20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x34, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Direction:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.cbVisible.FlatStyle = FlatStyle.Flat;
            this.cbVisible.Location = new Point(8, 8);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new Size(0x60, 0x16);
            this.cbVisible.TabIndex = 1;
            this.cbVisible.Text = "&Visible";
            this.cbVisible.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.tabPage1.Controls.Add(this.ndTransp);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.cbNoMiddle);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.bMiddle);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0xf2, 0xcd);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Colors";
            this.ndTransp.BorderStyle = BorderStyle.FixedSingle;
            this.ndTransp.Location = new Point(0x58, 0x80);
            this.ndTransp.Name = "ndTransp";
            this.ndTransp.Size = new Size(0x30, 20);
            this.ndTransp.TabIndex = 20;
            this.ndTransp.TextAlign = HorizontalAlignment.Right;
            this.ndTransp.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(8, 0x80);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4d, 0x10);
            this.label4.TabIndex = 0x13;
            this.label4.Text = "&Transparency:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.cbNoMiddle.FlatStyle = FlatStyle.Flat;
            this.cbNoMiddle.Location = new Point(0x68, 0x2b);
            this.cbNoMiddle.Name = "cbNoMiddle";
            this.cbNoMiddle.Size = new Size(120, 0x10);
            this.cbNoMiddle.TabIndex = 0x12;
            this.cbNoMiddle.Text = "&No Middle Color";
            this.cbNoMiddle.CheckedChanged += new EventHandler(this.cbNoMiddle_CheckedChanged);
            this.checkBox3.FlatStyle = FlatStyle.Flat;
            this.checkBox3.Location = new Point(8, 0x68);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x80, 0x10);
            this.checkBox3.TabIndex = 0x11;
            this.checkBox3.Text = "&Gamma Correction";
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.bMiddle.Color = Color.Empty;
            this.bMiddle.Location = new Point(8, 40);
            this.bMiddle.Name = "bMiddle";
            this.bMiddle.Size = new Size(0x59, 0x17);
            this.bMiddle.TabIndex = 7;
            this.bMiddle.Text = "&Middle...";
            this.bMiddle.Click += new EventHandler(this.button4_Click);
            this.button2.Color = Color.Empty;
            this.button2.Location = new Point(8, 0x48);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x59, 0x17);
            this.button2.TabIndex = 8;
            this.button2.Text = "&End...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(8, 8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x59, 0x17);
            this.button1.TabIndex = 6;
            this.button1.Text = "&Start...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.tabPage2.Controls.Add(this.checkBox4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Controls.Add(this.trackBar2);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.trackBar1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0xf2, 0xcd);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Options";
            this.checkBox4.FlatStyle = FlatStyle.Flat;
            this.checkBox4.Location = new Point(0x7b, 8);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new Size(0x65, 0x10);
            this.checkBox4.TabIndex = 0x12;
            this.checkBox4.Text = "&Polygonal";
            this.checkBox4.Visible = false;
            this.checkBox4.CheckedChanged += new EventHandler(this.checkBox4_CheckedChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x10, 0x51);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x47, 0x10);
            this.label3.TabIndex = 0x11;
            this.label3.Text = "Sigma S&cale:";
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(0x10, 8);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0x68, 0x10);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "Sigm&a";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.trackBar2.AutoSize = false;
            this.trackBar2.LargeChange = 100;
            this.trackBar2.Location = new Point(8, 0x61);
            this.trackBar2.Maximum = 0x3e8;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new Size(0x70, 0x2a);
            this.trackBar2.SmallChange = 10;
            this.trackBar2.TabIndex = 0x10;
            this.trackBar2.TickFrequency = 100;
            this.trackBar2.ValueChanged += new EventHandler(this.trackBar2_ValueChanged);
            this.trackBar2.Scroll += new EventHandler(this.trackBar2_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x49, 0x10);
            this.label2.TabIndex = 14;
            this.label2.Text = "Sigma &Focus:";
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 100;
            this.trackBar1.Location = new Point(8, 0x30);
            this.trackBar1.Maximum = 0x3e8;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(0x70, 0x2a);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 15;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.ValueChanged += new EventHandler(this.trackBar1_ValueChanged);
            this.trackBar1.Scroll += new EventHandler(this.trackBar1_ValueChanged);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xbf);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(250, 40);
            this.panel1.TabIndex = 0x12;
            this.button3.DialogResult = DialogResult.OK;
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.Location = new Point(160, 8);
            this.button3.Name = "button3";
            this.button3.TabIndex = 14;
            this.button3.Text = "OK";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(250, 0xe7);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.tabControl1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "GradientEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "GradientEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ndAngle.EndInit();
            this.tabPage1.ResumeLayout(false);
            this.ndTransp.EndInit();
            this.tabPage2.ResumeLayout(false);
            this.trackBar2.EndInit();
            this.trackBar1.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.gradient.Transparency = (int) this.ndTransp.Value;
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            if (this.ndAngle.Value != ((int) this.gradient.Angle))
            {
                this.gradient.Angle = (int) this.ndAngle.Value;
            }
        }

        private void SetGradient(Steema.TeeChart.Drawing.Gradient g)
        {
            this.gradient = g;
            this.trackBar1.Value = Utils.Round((float) (this.gradient.SigmaFocus * 1000f));
            this.trackBar2.Value = Utils.Round((float) (this.gradient.SigmaScale * 1000f));
            this.cbVisible.Checked = this.gradient.Visible;
            this.checkBox2.Checked = this.gradient.Sigma;
            this.button1.Color = this.gradient.StartColor;
            this.button2.Color = this.gradient.EndColor;
            this.bMiddle.Color = this.gradient.MiddleColor;
            this.cbNoMiddle.Checked = !this.gradient.UseMiddle;
            switch (this.gradient.Direction)
            {
                case LinearGradientMode.Horizontal:
                    this.comboBox1.SelectedIndex = 2;
                    break;

                case LinearGradientMode.Vertical:
                    this.comboBox1.SelectedIndex = 3;
                    break;

                case LinearGradientMode.ForwardDiagonal:
                    this.comboBox1.SelectedIndex = 1;
                    break;

                case LinearGradientMode.BackwardDiagonal:
                    this.comboBox1.SelectedIndex = 0;
                    break;
            }
            this.ndTransp.Value = this.gradient.Transparency;
            this.ndAngle.Value = (int) this.gradient.Angle;
            this.checkBox3.Checked = this.gradient.GammaCorrection;
            this.EnableSigma();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.gradient.SigmaFocus = ((float) this.trackBar1.Value) / 1000f;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            this.gradient.SigmaScale = ((float) this.trackBar2.Value) / 1000f;
        }

        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.gradient;
            }
            set
            {
                this.gradient = value;
            }
        }
    }
}

