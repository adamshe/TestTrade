namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TitleEditor : Form
    {
        private CheckBox cbAdjust;
        private ComboBox cbAlign;
        private CheckBox cbCustomPos;
        private Chart chart;
        private CheckBox checkBox3;
        private ComboBox comboTitle;
        private Container components;
        private Label label1;
        private Label label5;
        private Label label6;
        private Label label8;
        private NumericUpDown ndLeft;
        private NumericUpDown ndTop;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private bool setting;
        private CustomShapeEditor shapeEditor;
        private TabControl tabControl4;
        private TabPage tabPage12;
        private TabPage tabPage8;
        private TextBox textTitle;

        public TitleEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public TitleEditor(Chart c, Control parent) : this()
        {
            this.chart = c;
            this.comboTitle.SelectedIndex = 0;
            EditorUtils.InsertForm(this, parent);
        }

        public TitleEditor(Steema.TeeChart.Title t, Control parent) : this()
        {
            this.chart = t.Chart;
            if (t == this.chart.Header)
            {
                this.comboTitle.SelectedIndex = 0;
            }
            else if (t == this.chart.SubHeader)
            {
                this.comboTitle.SelectedIndex = 1;
            }
            else if (t == this.chart.SubFooter)
            {
                this.comboTitle.SelectedIndex = 2;
            }
            else if (t == this.chart.Footer)
            {
                this.comboTitle.SelectedIndex = 3;
            }
            EditorUtils.InsertForm(this, parent);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Title.CustomPosition = this.cbCustomPos.Checked;
            this.EnableCustomPosition();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.Title.Visible = this.checkBox3.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            this.Title.AdjustFrame = this.cbAdjust.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbAlign.SelectedIndex)
            {
                case 0:
                    this.Title.Alignment = StringAlignment.Near;
                    return;

                case 1:
                    this.Title.Alignment = StringAlignment.Center;
                    return;
            }
            this.Title.Alignment = StringAlignment.Far;
        }

        private void comboTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.tabControl4.SelectedIndex;
            if (this.shapeEditor != null)
            {
                CustomShapeEditor.Remove(this.tabControl4, this.shapeEditor);
                this.shapeEditor.Dispose();
            }
            this.shapeEditor = CustomShapeEditor.Add(this.tabControl4, this.Title);
            this.shapeEditor.Translate();
            this.checkBox3.Checked = this.Title.Visible;
            this.setting = true;
            this.textTitle.Text = this.Title.Text;
            this.setting = false;
            this.cbAdjust.Checked = this.Title.AdjustFrame;
            this.cbAlign.SelectedIndex = (int) this.Title.Alignment;
            this.cbCustomPos.Checked = this.Title.CustomPosition;
            this.ndLeft.Value = this.Title.Left;
            this.ndTop.Value = this.Title.Top;
            this.tabControl4.SelectedIndex = selectedIndex;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableCustomPosition()
        {
            bool customPosition = this.Title.CustomPosition;
            this.label5.Enabled = customPosition;
            this.label6.Enabled = customPosition;
            this.ndLeft.Enabled = customPosition;
            this.ndTop.Enabled = customPosition;
            if (customPosition)
            {
                this.ndLeft.Value = this.Title.Left;
                this.ndTop.Value = this.Title.Top;
            }
        }

        private void InitializeComponent()
        {
            this.panel4 = new System.Windows.Forms.Panel();
            this.comboTitle = new ComboBox();
            this.label1 = new Label();
            this.tabControl4 = new TabControl();
            this.tabPage8 = new TabPage();
            this.textTitle = new TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbAlign = new ComboBox();
            this.label8 = new Label();
            this.cbAdjust = new CheckBox();
            this.checkBox3 = new CheckBox();
            this.tabPage12 = new TabPage();
            this.ndTop = new NumericUpDown();
            this.label6 = new Label();
            this.ndLeft = new NumericUpDown();
            this.label5 = new Label();
            this.cbCustomPos = new CheckBox();
            this.panel4.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.ndTop.BeginInit();
            this.ndLeft.BeginInit();
            base.SuspendLayout();
            this.panel4.Controls.Add(this.comboTitle);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = DockStyle.Top;
            this.panel4.Location = new Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x180, 0x20);
            this.panel4.TabIndex = 0;
            this.comboTitle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboTitle.DropDownWidth = 0x79;
            this.comboTitle.Items.AddRange(new object[] { "Header", "Sub Header", "Sub Footer", "Footer" });
            this.comboTitle.Location = new Point(0x37, 5);
            this.comboTitle.Name = "comboTitle";
            this.comboTitle.Size = new Size(0x79, 0x15);
            this.comboTitle.TabIndex = 1;
            this.comboTitle.SelectedIndexChanged += new EventHandler(this.comboTitle_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(20, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Title:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.tabControl4.Controls.Add(this.tabPage8);
            this.tabControl4.Controls.Add(this.tabPage12);
            this.tabControl4.Dock = DockStyle.Fill;
            this.tabControl4.HotTrack = true;
            this.tabControl4.Location = new Point(0, 0x20);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new Size(0x180, 0xd5);
            this.tabControl4.TabIndex = 1;
            this.tabPage8.Controls.Add(this.textTitle);
            this.tabPage8.Controls.Add(this.panel5);
            this.tabPage8.Location = new Point(4, 0x16);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new Size(0x178, 0xbb);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "Style";
            this.textTitle.BorderStyle = BorderStyle.FixedSingle;
            this.textTitle.Dock = DockStyle.Fill;
            this.textTitle.Location = new Point(0, 0x2a);
            this.textTitle.Multiline = true;
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new Size(0x178, 0x91);
            this.textTitle.TabIndex = 1;
            this.textTitle.Text = "";
            this.textTitle.TextChanged += new EventHandler(this.textTitle_TextChanged);
            this.panel5.Controls.Add(this.cbAlign);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.cbAdjust);
            this.panel5.Controls.Add(this.checkBox3);
            this.panel5.Dock = DockStyle.Top;
            this.panel5.Location = new Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x178, 0x2a);
            this.panel5.TabIndex = 0;
            this.cbAlign.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAlign.Items.AddRange(new object[] { "Left", "Center", "Right" });
            this.cbAlign.Location = new Point(0x10f, 11);
            this.cbAlign.Name = "cbAlign";
            this.cbAlign.Size = new Size(0x63, 0x15);
            this.cbAlign.TabIndex = 3;
            this.cbAlign.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(210, 14);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x3a, 0x10);
            this.label8.TabIndex = 2;
            this.label8.Text = "A&lignment:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.cbAdjust.FlatStyle = FlatStyle.Flat;
            this.cbAdjust.Location = new Point(0x56, 10);
            this.cbAdjust.Name = "cbAdjust";
            this.cbAdjust.Size = new Size(0x81, 0x17);
            this.cbAdjust.TabIndex = 1;
            this.cbAdjust.Text = "&Adjust Frame";
            this.cbAdjust.CheckedChanged += new EventHandler(this.checkBox7_CheckedChanged);
            this.checkBox3.FlatStyle = FlatStyle.Flat;
            this.checkBox3.Location = new Point(7, 10);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x4f, 0x17);
            this.checkBox3.TabIndex = 0;
            this.checkBox3.Text = "&Visible";
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.tabPage12.Controls.Add(this.ndTop);
            this.tabPage12.Controls.Add(this.label6);
            this.tabPage12.Controls.Add(this.ndLeft);
            this.tabPage12.Controls.Add(this.label5);
            this.tabPage12.Controls.Add(this.cbCustomPos);
            this.tabPage12.Location = new Point(4, 0x16);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new Size(0x178, 0xbb);
            this.tabPage12.TabIndex = 1;
            this.tabPage12.Text = "Position";
            this.tabPage12.Visible = false;
            this.ndTop.BorderStyle = BorderStyle.FixedSingle;
            this.ndTop.Location = new Point(120, 0x4d);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.ndTop.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.ndTop.Minimum = new decimal(bits);
            this.ndTop.Name = "ndTop";
            this.ndTop.Size = new Size(0x33, 20);
            this.ndTop.TabIndex = 4;
            this.ndTop.TextAlign = HorizontalAlignment.Right;
            this.ndTop.TextChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.ndTop.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(80, 0x4f);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x1b, 0x10);
            this.label6.TabIndex = 3;
            this.label6.Text = "&Top:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.ndLeft.BorderStyle = BorderStyle.FixedSingle;
            this.ndLeft.Location = new Point(120, 0x2c);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.ndLeft.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.ndLeft.Minimum = new decimal(bits);
            this.ndLeft.Name = "ndLeft";
            this.ndLeft.Size = new Size(0x33, 20);
            this.ndLeft.TabIndex = 2;
            this.ndLeft.TextAlign = HorizontalAlignment.Right;
            this.ndLeft.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.ndLeft.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(80, 0x2e);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1a, 0x10);
            this.label5.TabIndex = 1;
            this.label5.Text = "&Left:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.cbCustomPos.FlatStyle = FlatStyle.Flat;
            this.cbCustomPos.Location = new Point(0x30, 0x10);
            this.cbCustomPos.Name = "cbCustomPos";
            this.cbCustomPos.Size = new Size(0x48, 0x10);
            this.cbCustomPos.TabIndex = 0;
            this.cbCustomPos.Text = "&Custom:";
            this.cbCustomPos.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x180, 0xf5);
            base.Controls.Add(this.tabControl4);
            base.Controls.Add(this.panel4);
            base.Name = "TitleEditor";
            this.Text = "TitleEditor";
            this.panel4.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.ndTop.EndInit();
            this.ndLeft.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.Title.Left = (int) this.ndLeft.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.Title.Top = (int) this.ndTop.Value;
        }

        private void textTitle_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.Title.Lines = this.textTitle.Lines;
            }
        }

        public Steema.TeeChart.Title Title
        {
            get
            {
                switch (this.comboTitle.SelectedIndex)
                {
                    case 0:
                        return this.chart.Header;

                    case 1:
                        return this.chart.SubHeader;

                    case 2:
                        return this.chart.SubFooter;
                }
                return this.chart.Footer;
            }
        }
    }
}

