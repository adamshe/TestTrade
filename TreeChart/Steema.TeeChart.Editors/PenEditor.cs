namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class PenEditor : Form
    {
        private ButtonColor button1;
        private CheckBox cbVisible;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown ndTransp;
        private NumericUpDown numericUpDown1;
        private Button oKbutton;
        protected ChartPen pen;

        public PenEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PenEditor(ChartPen p) : this()
        {
            this.SetPen(p);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pen.Invalidate();
            this.pen.Color = this.button1.Color;
            this.ndTransp.Value = Graphics3D.Transparency(this.pen.Color);
            base.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.pen.Visible = this.cbVisible.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.pen.Style = DashStyle.Solid;
                    break;

                case 1:
                    this.pen.Style = DashStyle.Dash;
                    break;

                case 2:
                    this.pen.Style = DashStyle.Dot;
                    break;

                case 3:
                    this.pen.Style = DashStyle.DashDot;
                    break;

                default:
                    this.pen.Style = DashStyle.DashDotDot;
                    break;
            }
            this.comboBox3.Enabled = this.comboBox1.SelectedIndex != 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox2.SelectedIndex)
            {
                case 0:
                    this.pen.EndCap = LineCap.Flat;
                    return;

                case 1:
                    this.pen.EndCap = LineCap.Round;
                    return;

                case 2:
                    this.pen.EndCap = LineCap.Square;
                    return;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox3.SelectedIndex)
            {
                case 0:
                    this.pen.DashCap = DashCap.Flat;
                    return;

                case 1:
                    this.pen.DashCap = DashCap.Round;
                    return;

                case 2:
                    this.pen.DashCap = DashCap.Triangle;
                    return;
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

        public static bool Edit(ChartPen pen)
        {
            using (PenEditor editor = new PenEditor(pen))
            {
                EditorUtils.Translate(editor);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        private void InitializeComponent()
        {
            this.button1 = new ButtonColor();
            this.cbVisible = new CheckBox();
            this.oKbutton = new Button();
            this.label1 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.label2 = new Label();
            this.comboBox1 = new ComboBox();
            this.label3 = new Label();
            this.comboBox2 = new ComboBox();
            this.comboBox3 = new ComboBox();
            this.label4 = new Label();
            this.label5 = new Label();
            this.ndTransp = new NumericUpDown();
            this.numericUpDown1.BeginInit();
            this.ndTransp.BeginInit();
            base.SuspendLayout();
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(9, 30);
            this.button1.Name = "button1";
            this.button1.TabIndex = 1;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.cbVisible.FlatStyle = FlatStyle.Flat;
            this.cbVisible.Location = new Point(9, 5);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new Size(0x47, 20);
            this.cbVisible.TabIndex = 0;
            this.cbVisible.Text = "&Visible";
            this.cbVisible.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.oKbutton.DialogResult = DialogResult.OK;
            this.oKbutton.FlatStyle = FlatStyle.Flat;
            this.oKbutton.Location = new Point(200, 0x70);
            this.oKbutton.Name = "oKbutton";
            this.oKbutton.TabIndex = 2;
            this.oKbutton.Text = "OK";
            this.oKbutton.Click += new EventHandler(this.button2_Click);
            this.label1.AutoSize = true;
            this.label1.FlatStyle = FlatStyle.Flat;
            this.label1.Location = new Point(0x88, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0xaf, 7);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x30, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x88, 0x22);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 0x10);
            this.label2.TabIndex = 5;
            this.label2.Text = "&Style:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.DropDownWidth = 0x66;
            this.comboBox1.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "Dash Dot", "Dash Dot Dot" });
            this.comboBox1.Location = new Point(0xb0, 0x20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(120, 0x15);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(9, 0x3f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x2b, 0x10);
            this.label3.TabIndex = 7;
            this.label3.Text = "&Ending:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.DropDownWidth = 0x48;
            this.comboBox2.Items.AddRange(new object[] { "Flat", "Round", "Square" });
            this.comboBox2.Location = new Point(0x38, 0x3d);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x54, 0x15);
            this.comboBox2.TabIndex = 8;
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox3.DropDownWidth = 0x48;
            this.comboBox3.Items.AddRange(new object[] { "Flat", "Round", "Triangle" });
            this.comboBox3.Location = new Point(0x38, 0x58);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new Size(0x54, 0x15);
            this.comboBox3.TabIndex = 10;
            this.comboBox3.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x11, 90);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x22, 0x10);
            this.label4.TabIndex = 9;
            this.label4.Text = "&Dash:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label5.AutoSize = true;
            this.label5.FlatStyle = FlatStyle.Flat;
            this.label5.Location = new Point(0xa8, 0x3e);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4d, 0x10);
            this.label5.TabIndex = 11;
            this.label5.Text = "&Transparency:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.ndTransp.BorderStyle = BorderStyle.FixedSingle;
            this.ndTransp.Location = new Point(0xf6, 60);
            this.ndTransp.Name = "ndTransp";
            this.ndTransp.Size = new Size(0x31, 20);
            this.ndTransp.TabIndex = 12;
            this.ndTransp.TextAlign = HorizontalAlignment.Right;
            this.ndTransp.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.ndTransp.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            base.AcceptButton = this.oKbutton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x132, 0x8f);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.ndTransp);
            base.Controls.Add(this.comboBox3);
            base.Controls.Add(this.comboBox2);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.oKbutton);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.cbVisible);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "PenEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Border Editor";
            this.numericUpDown1.EndInit();
            this.ndTransp.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.pen.Width = (int) this.numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.pen.Transparency = (int) this.ndTransp.Value;
        }

        public static int PenStyle(ChartPen pen)
        {
            switch (pen.Style)
            {
                case DashStyle.Solid:
                    return 0;

                case DashStyle.Dash:
                    return 1;

                case DashStyle.Dot:
                    return 2;

                case DashStyle.DashDot:
                    return 3;
            }
            return 4;
        }

        protected void SetPen(ChartPen p)
        {
            this.pen = p;
            this.cbVisible.Checked = this.pen.Visible;
            this.cbVisible.Visible = this.pen.usesVisible;
            this.numericUpDown1.Value = Convert.ToDecimal(this.pen.Width);
            this.comboBox1.SelectedIndex = PenStyle(this.pen);
            switch (this.pen.EndCap)
            {
                case LineCap.Square:
                    this.comboBox2.SelectedIndex = 2;
                    break;

                case LineCap.Round:
                    this.comboBox2.SelectedIndex = 1;
                    break;

                default:
                    this.comboBox2.SelectedIndex = 0;
                    break;
            }
            this.comboBox3.SelectedIndex = (int) this.pen.DashCap;
            this.comboBox3.Enabled = this.comboBox1.SelectedIndex != 0;
            this.ndTransp.Value = this.pen.Transparency;
            this.button1.Color = this.pen.Color;
        }

        protected Button OkButton
        {
            get
            {
                return this.oKbutton;
            }
        }

        protected CheckBox VisibleCheckBox
        {
            get
            {
                return this.cbVisible;
            }
        }
    }
}

