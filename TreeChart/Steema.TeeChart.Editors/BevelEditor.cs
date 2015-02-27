namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BevelEditor : Form
    {
        private Bevel bevel;
        private ButtonColor button1;
        private ButtonColor button2;
        private ComboBox CBBevel;
        private ComboBox comboBox1;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown UDBevW;

        public BevelEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public BevelEditor(Bevel b, Control parent)
        {
            this.components = null;
            this.bevel = b;
            this.InitializeComponent();
            this.UDBevW.Value = this.bevel.Width;
            this.SetBevel(this.bevel.Inner, this.CBBevel);
            this.SetBevel(this.bevel.Outer, this.comboBox1);
            this.button1.Color = this.bevel.ColorOne;
            this.button2.Color = this.bevel.ColorTwo;
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.bevel.ColorOne = this.button1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.bevel.ColorTwo = this.button2.Color;
        }

        private void CBBevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bevel.Inner = this.GetBevel(this.CBBevel.SelectedIndex);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bevel.Outer = this.GetBevel(this.comboBox1.SelectedIndex);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private BevelStyles GetBevel(int index)
        {
            switch (index)
            {
                case 1:
                    return BevelStyles.Lowered;

                case 2:
                    return BevelStyles.Raised;
            }
            return BevelStyles.None;
        }

        private void InitializeComponent()
        {
            this.button2 = new ButtonColor();
            this.button1 = new ButtonColor();
            this.comboBox1 = new ComboBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.UDBevW = new NumericUpDown();
            this.CBBevel = new ComboBox();
            this.label1 = new Label();
            this.UDBevW.BeginInit();
            base.SuspendLayout();
            this.button2.Color = Color.Empty;
            this.button2.Location = new Point(0x71, 0x39);
            this.button2.Name = "button2";
            this.button2.TabIndex = 5;
            this.button2.Text = "C&olor...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(8, 0x39);
            this.button1.Name = "button1";
            this.button1.TabIndex = 4;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "None", "Lowered", "Raised" });
            this.comboBox1.Location = new Point(8, 0x1b);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(80, 0x15);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x41, 0x10);
            this.label3.TabIndex = 0;
            this.label3.Text = "Bevel &outer:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x40, 0x5d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1d, 0x10);
            this.label2.TabIndex = 6;
            this.label2.Text = "Siz&e:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDBevW.BorderStyle = BorderStyle.FixedSingle;
            this.UDBevW.Location = new Point(0x60, 0x5b);
            int[] bits = new int[4];
            bits[0] = 200;
            this.UDBevW.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDBevW.Minimum = new decimal(bits);
            this.UDBevW.Name = "UDBevW";
            this.UDBevW.Size = new Size(0x30, 20);
            this.UDBevW.TabIndex = 7;
            this.UDBevW.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDBevW.Value = new decimal(bits);
            this.UDBevW.TextChanged += new EventHandler(this.UDBevW_ValueChanged);
            this.UDBevW.ValueChanged += new EventHandler(this.UDBevW_ValueChanged);
            this.CBBevel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBBevel.Items.AddRange(new object[] { "None", "Lowered", "Raised" });
            this.CBBevel.Location = new Point(0x71, 0x1b);
            this.CBBevel.Name = "CBBevel";
            this.CBBevel.Size = new Size(80, 0x15);
            this.CBBevel.TabIndex = 3;
            this.CBBevel.SelectedIndexChanged += new EventHandler(this.CBBevel_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x71, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x40, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bevel &inner:";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe8, 0x8d);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.UDBevW);
            base.Controls.Add(this.CBBevel);
            base.Name = "BevelEditor";
            this.Text = "Bevel editor";
            this.UDBevW.EndInit();
            base.ResumeLayout(false);
        }

        private void SetBevel(BevelStyles b, ComboBox combo)
        {
            switch (b)
            {
                case BevelStyles.None:
                    combo.SelectedIndex = 0;
                    return;

                case BevelStyles.Lowered:
                    combo.SelectedIndex = 1;
                    return;

                case BevelStyles.Raised:
                    combo.SelectedIndex = 2;
                    return;
            }
        }

        private void UDBevW_ValueChanged(object sender, EventArgs e)
        {
            if (this.bevel != null)
            {
                this.bevel.Width = (int) this.UDBevW.Value;
            }
        }
    }
}

