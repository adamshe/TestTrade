namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AxisArrowEditor : AxisToolEdit
    {
        private Button button1;
        private CheckBox cbInverted;
        private ComboBox cbPosition;
        private IContainer components;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown ndLength;
        private NumericUpDown ndScroll;
        private bool setting;
        private AxisArrow tool;

        public AxisArrowEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AxisArrowEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.setting = true;
            this.tool = (AxisArrow) t;
            base.SetTool(this.tool);
            this.cbInverted.Checked = this.tool.ScrollInverted;
            this.ndLength.Value = this.tool.Length;
            this.ndScroll.Value = this.tool.ScrollPercent;
            this.cbPosition.SelectedIndex = (int) this.tool.Position;
            this.setting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Brush);
        }

        private void cbInverted_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.ScrollInverted = this.cbInverted.Checked;
            }
        }

        private void cbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Position = (AxisArrowPosition) this.cbPosition.SelectedIndex;
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
            this.cbInverted = new CheckBox();
            this.label2 = new Label();
            this.ndLength = new NumericUpDown();
            this.ndScroll = new NumericUpDown();
            this.label3 = new Label();
            this.label4 = new Label();
            this.button1 = new Button();
            this.label5 = new Label();
            this.cbPosition = new ComboBox();
            this.ndLength.BeginInit();
            this.ndScroll.BeginInit();
            base.SuspendLayout();
            base.BPen.Name = "BPen";
            this.cbInverted.FlatStyle = FlatStyle.Flat;
            this.cbInverted.Location = new Point(0x48, 0x66);
            this.cbInverted.Name = "cbInverted";
            this.cbInverted.Size = new Size(0x98, 0x18);
            this.cbInverted.TabIndex = 6;
            this.cbInverted.Text = "&Inverted scroll";
            this.cbInverted.CheckedChanged += new EventHandler(this.cbInverted_CheckedChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x18, 80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2a, 0x10);
            this.label2.TabIndex = 4;
            this.label2.Text = "&Length:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.ndLength.BorderStyle = BorderStyle.FixedSingle;
            this.ndLength.Location = new Point(0x48, 0x4e);
            int[] bits = new int[4];
            bits[0] = 200;
            this.ndLength.Maximum = new decimal(bits);
            this.ndLength.Name = "ndLength";
            this.ndLength.Size = new Size(0x38, 20);
            this.ndLength.TabIndex = 5;
            this.ndLength.TextAlign = HorizontalAlignment.Right;
            this.ndLength.ValueChanged += new EventHandler(this.ndLength_ValueChanged);
            this.ndScroll.BorderStyle = BorderStyle.FixedSingle;
            this.ndScroll.Location = new Point(0x48, 0x86);
            this.ndScroll.Name = "ndScroll";
            this.ndScroll.Size = new Size(0x38, 20);
            this.ndScroll.TabIndex = 8;
            this.ndScroll.TextAlign = HorizontalAlignment.Right;
            this.ndScroll.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x20, 0x88);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 0x10);
            this.label3.TabIndex = 7;
            this.label3.Text = "&Scroll:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x88, 0x8a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(14, 0x10);
            this.label4.TabIndex = 9;
            this.label4.Text = "%";
            this.label4.UseMnemonic = false;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x9d, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4e, 0x17);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Fill...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x18, 0xa2);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x30, 0x10);
            this.label5.TabIndex = 10;
            this.label5.Text = "&Position:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.cbPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbPosition.Items.AddRange(new object[] { "Start", "End", "Both" });
            this.cbPosition.Location = new Point(0x48, 160);
            this.cbPosition.Name = "cbPosition";
            this.cbPosition.Size = new Size(0x79, 0x15);
            this.cbPosition.TabIndex = 11;
            this.cbPosition.SelectedIndexChanged += new EventHandler(this.cbPosition_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0xbd);
            base.Controls.Add(this.cbPosition);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.ndScroll);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.ndLength);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cbInverted);
            base.Name = "AxisArrowEditor";
            base.Controls.SetChildIndex(base.BPen, 0);
            base.Controls.SetChildIndex(this.cbInverted, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.ndLength, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.ndScroll, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(this.label5, 0);
            base.Controls.SetChildIndex(this.cbPosition, 0);
            this.ndLength.EndInit();
            this.ndScroll.EndInit();
            base.ResumeLayout(false);
        }

        private void ndLength_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Length = (int) this.ndLength.Value;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.ScrollPercent = (int) this.ndScroll.Value;
            }
        }
    }
}

