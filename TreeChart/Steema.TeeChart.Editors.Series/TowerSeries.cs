namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TowerSeries : Form
    {
        private Button button1;
        private ButtonPen buttonPen1;
        private CheckBox cbOrigin;
        private ComboBox cbStyle;
        private CheckBox checkBox1;
        private Container components;
        private TextBox eOrigin;
        private Grid3DSeries grid3DEditor;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private Tower tower;

        public TowerSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public TowerSeries(Series s) : this()
        {
            this.tower = (Tower) s;
            this.numericUpDown1.Value = this.tower.PercentDepth;
            this.numericUpDown2.Value = this.tower.PercentWidth;
            this.numericUpDown3.Value = this.tower.Transparency;
            this.checkBox1.Checked = this.tower.Dark3D;
            this.buttonPen1.Pen = this.tower.Pen;
            this.cbOrigin.Checked = this.tower.UseOrigin;
            this.eOrigin.Text = this.tower.Origin.ToString();
            this.cbStyle.Items.AddRange(Enum.GetNames(typeof(TowerStyles)));
            this.cbStyle.SelectedItem = this.tower.TowerStyle.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tower.Brush);
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbStyle.SelectedIndex)
            {
                case 0:
                    this.tower.TowerStyle = TowerStyles.Cube;
                    return;

                case 1:
                    this.tower.TowerStyle = TowerStyles.Rectangle;
                    return;

                case 2:
                    this.tower.TowerStyle = TowerStyles.Cover;
                    return;

                case 3:
                    this.tower.TowerStyle = TowerStyles.Cylinder;
                    return;

                case 4:
                    this.tower.TowerStyle = TowerStyles.Arrow;
                    return;

                case 5:
                    this.tower.TowerStyle = TowerStyles.Cone;
                    return;

                case 6:
                    this.tower.TowerStyle = TowerStyles.Pyramid;
                    return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.tower.Dark3D = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.tower.UseOrigin = this.cbOrigin.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void eOrigin_TextChanged(object sender, EventArgs e)
        {
            this.tower.Origin = Convert.ToDouble(this.eOrigin.Text);
        }

        private void InitializeComponent()
        {
            this.buttonPen1 = new ButtonPen();
            this.button1 = new Button();
            this.checkBox1 = new CheckBox();
            this.label1 = new Label();
            this.cbStyle = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.eOrigin = new TextBox();
            this.cbOrigin = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.numericUpDown2 = new NumericUpDown();
            this.numericUpDown1 = new NumericUpDown();
            this.label3 = new Label();
            this.label2 = new Label();
            this.numericUpDown3 = new NumericUpDown();
            this.label4 = new Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.numericUpDown2.BeginInit();
            this.numericUpDown1.BeginInit();
            this.numericUpDown3.BeginInit();
            base.SuspendLayout();
            this.buttonPen1.FlatStyle = FlatStyle.Flat;
            this.buttonPen1.Location = new Point(8, 8);
            this.buttonPen1.Name = "buttonPen1";
            this.buttonPen1.TabIndex = 0;
            this.buttonPen1.Text = "&Border...";
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x5d, 8);
            this.button1.Name = "button1";
            this.button1.TabIndex = 1;
            this.button1.Text = "&Pattern...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0xb0, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x70, 0x18);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "&Dark 3D";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1d, 0x30);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Style:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Location = new Point(0x3d, 0x2d);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x6b, 0x15);
            this.cbStyle.TabIndex = 4;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.groupBox1.Controls.Add(this.eOrigin);
            this.groupBox1.Controls.Add(this.cbOrigin);
            this.groupBox1.Location = new Point(0xb0, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 0x48);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Origin:";
            this.eOrigin.BorderStyle = BorderStyle.FixedSingle;
            this.eOrigin.Location = new Point(8, 0x2a);
            this.eOrigin.Name = "eOrigin";
            this.eOrigin.Size = new Size(0x58, 20);
            this.eOrigin.TabIndex = 1;
            this.eOrigin.Text = "";
            this.eOrigin.TextAlign = HorizontalAlignment.Right;
            this.eOrigin.TextChanged += new EventHandler(this.eOrigin_TextChanged);
            this.cbOrigin.FlatStyle = FlatStyle.Flat;
            this.cbOrigin.Location = new Point(8, 0x12);
            this.cbOrigin.Name = "cbOrigin";
            this.cbOrigin.TabIndex = 0;
            this.cbOrigin.Text = "&Use Origin";
            this.cbOrigin.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.groupBox2.Controls.Add(this.numericUpDown2);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new Point(8, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(160, 0x40);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Percent:";
            this.numericUpDown2.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown2.Location = new Point(0x54, 0x24);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x40, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x54, 13);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x40, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(40, 0x26);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 0x10);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Width:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x26, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x26, 0x10);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Depth:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown3.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown3.Location = new Point(0x68, 0x98);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x40, 20);
            this.numericUpDown3.TabIndex = 8;
            this.numericUpDown3.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown3.TextChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown3.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x18, 0x9a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4d, 0x10);
            this.label4.TabIndex = 7;
            this.label4.Text = "&Transparency:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x130, 0xb5);
            base.Controls.Add(this.numericUpDown3);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.cbStyle);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.buttonPen1);
            base.Name = "TowerSeries";
            this.Text = "TowerEditor";
            base.Load += new EventHandler(this.TowerSeries_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.numericUpDown2.EndInit();
            this.numericUpDown1.EndInit();
            this.numericUpDown3.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.tower.PercentDepth = (int) this.numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.tower.PercentWidth = (int) this.numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.tower.Transparency = (int) this.numericUpDown3.Value;
        }

        private void TowerSeries_Load(object sender, EventArgs e)
        {
            if ((this.tower != null) && (this.grid3DEditor == null))
            {
                this.grid3DEditor = new Grid3DSeries(this.tower, this);
            }
        }
    }
}

