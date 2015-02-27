namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class BrushEditor : Form
    {
        private Button bOk;
        private ChartBrush brush;
        private ButtonColor button3;
        private Button button4;
        private ButtonColor button6;
        private CheckBox checkBox1;
        private ColorEditor colorChooser;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Container components;
        private GradientEditor gradientEditor;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label5;
        private Label label6;
        private ListBox listBox1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        internal System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panelHatch;
        private PictureBox pictureBox1;
        private bool setting;
        private TabControl tabControl1;
        private TabPage tabGradient;
        private TabPage tabHatch;
        private TabPage tabImage;
        private TabPage tabSolid;

        public BrushEditor()
        {
            this.components = null;
            this.InitializeComponent();
            foreach (object obj2 in Enum.GetValues(typeof(HatchStyle)))
            {
                this.listBox1.Items.Add(obj2);
            }
            foreach (object obj3 in Enum.GetValues(typeof(WrapMode)))
            {
                this.comboBox1.Items.Add(obj3);
            }
        }

        public BrushEditor(ChartBrush b) : this()
        {
            this.setting = true;
            this.brush = b;
            this.checkBox1.Checked = b.Visible;
            this.listBox1.SelectedItem = b.Style;
            this.button6.Color = this.brush.Color;
            this.button3.Color = this.brush.ForegroundColor;
            this.numericUpDown2.Value = b.Transparency;
            this.pictureBox1.Image = this.brush.Image;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.comboBox2.SelectedIndex = EditorUtils.ImageModeToIndex(this.brush.ImageMode);
            this.comboBox1.SelectedItem = b.WrapMode;
            this.comboBox1.Enabled = this.brush.ImageMode == ImageMode.Tile;
            if (b.Gradient.Visible)
            {
                this.tabControl1.SelectedIndex = 2;
            }
            else if (b.Solid)
            {
                this.tabControl1.SelectedIndex = 0;
            }
            else if (b.Image == null)
            {
                this.tabControl1.SelectedIndex = 1;
                this.RepaintSample();
            }
            else
            {
                this.tabControl1.SelectedIndex = 3;
            }
            this.setting = false;
        }

        private void BrushEditor_Load(object sender, EventArgs e)
        {
            if (this.brush != null)
            {
                if (this.gradientEditor == null)
                {
                    this.gradientEditor = new GradientEditor(this.brush.Gradient, this.tabGradient);
                    this.gradientEditor.cbVisible.Visible = false;
                    EditorUtils.Translate(this.gradientEditor);
                }
                if (this.colorChooser == null)
                {
                    this.colorChooser = new ColorEditor();
                    this.colorChooser.ColorChanged = (EventHandler) Delegate.Combine(this.colorChooser.ColorChanged, new EventHandler(this.Color_Changed));
                    this.colorChooser.ColorChoosen = (EventHandler) Delegate.Combine(this.colorChooser.ColorChoosen, new EventHandler(this.Color_Choosen));
                    this.colorChooser.panel2.Visible = false;
                    EditorUtils.InsertForm(this.colorChooser, this.tabSolid);
                    this.colorChooser.Fill(this.brush.Color, 0x38);
                    this.colorChooser.ndTransp.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
                    EditorUtils.Translate(this.colorChooser);
                }
                if (this.tabControl1.SelectedIndex == 1)
                {
                    this.RepaintSample();
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.brush.ForegroundColor = this.button3.Color;
            this.RepaintSample();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (this.brush.Image == null)
            {
                string filename = PictureDialog.FileName(this);
                if (filename.Length != 0)
                {
                    this.brush.Image = Image.FromFile(filename);
                    this.pictureBox1.Image = this.brush.Image;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.button4.Text = Texts.ClearImage;
                    this.brush.Solid = false;
                    this.brush.Gradient.visible = false;
                }
            }
            else
            {
                this.brush.Image = null;
                this.pictureBox1.Image = null;
                this.brush.Solid = true;
                this.brush.Gradient.Visible = false;
                this.button4.Text = Texts.BrowseImage;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.brush.Color = this.button6.Color;
            this.RepaintSample();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.brush.Visible = this.checkBox1.Checked;
            }
        }

        private void Color_Changed(object sender, EventArgs e)
        {
            this.brush.Color = this.colorChooser.Color;
        }

        private void Color_Choosen(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.brush.WrapMode = (WrapMode) this.comboBox1.SelectedItem;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.brush.ImageMode = EditorUtils.IndexToImageMode(this.comboBox2.SelectedIndex);
            this.comboBox1.Enabled = this.brush.ImageMode == ImageMode.Tile;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static void Edit(ChartBrush b)
        {
            Edit(b, true);
        }

        public static bool Edit(ChartBrush b, bool allowVisible)
        {
            using (BrushEditor editor = new BrushEditor(b))
            {
                editor.panel2.Visible = allowVisible;
                EditorUtils.Translate(editor);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bOk = new Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new TabControl();
            this.tabSolid = new TabPage();
            this.tabHatch = new TabPage();
            this.label6 = new Label();
            this.numericUpDown3 = new NumericUpDown();
            this.label5 = new Label();
            this.numericUpDown2 = new NumericUpDown();
            this.panelHatch = new System.Windows.Forms.Panel();
            this.button6 = new ButtonColor();
            this.button3 = new ButtonColor();
            this.label1 = new Label();
            this.listBox1 = new ListBox();
            this.tabGradient = new TabPage();
            this.tabImage = new TabPage();
            this.comboBox2 = new ComboBox();
            this.label3 = new Label();
            this.comboBox1 = new ComboBox();
            this.label2 = new Label();
            this.pictureBox1 = new PictureBox();
            this.button4 = new Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabHatch.SuspendLayout();
            this.numericUpDown3.BeginInit();
            this.numericUpDown2.BeginInit();
            this.tabImage.SuspendLayout();
            base.SuspendLayout();
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Dock = DockStyle.Top;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x11a, 0x18);
            this.panel2.TabIndex = 11;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(8, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x48, 0x10);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "&Visible";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged_1);
            this.panel3.Controls.Add(this.bOk);
            this.panel3.Dock = DockStyle.Bottom;
            this.panel3.Location = new Point(0, 0xf7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x11a, 0x20);
            this.panel3.TabIndex = 0;
            this.bOk.DialogResult = DialogResult.OK;
            this.bOk.FlatStyle = FlatStyle.Flat;
            this.bOk.Location = new Point(0xc7, 5);
            this.bOk.Name = "bOk";
            this.bOk.TabIndex = 0;
            this.bOk.Text = "OK";
            this.panel4.Controls.Add(this.tabControl1);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(0, 0x18);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x11a, 0xdf);
            this.panel4.TabIndex = 13;
            this.tabControl1.Controls.Add(this.tabSolid);
            this.tabControl1.Controls.Add(this.tabHatch);
            this.tabControl1.Controls.Add(this.tabGradient);
            this.tabControl1.Controls.Add(this.tabImage);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x11a, 0xdf);
            this.tabControl1.TabIndex = 11;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabSolid.Location = new Point(4, 0x16);
            this.tabSolid.Name = "tabSolid";
            this.tabSolid.Size = new Size(0x112, 0xc5);
            this.tabSolid.TabIndex = 0;
            this.tabSolid.Text = "Solid";
            this.tabHatch.Controls.Add(this.label6);
            this.tabHatch.Controls.Add(this.numericUpDown3);
            this.tabHatch.Controls.Add(this.label5);
            this.tabHatch.Controls.Add(this.numericUpDown2);
            this.tabHatch.Controls.Add(this.panelHatch);
            this.tabHatch.Controls.Add(this.button6);
            this.tabHatch.Controls.Add(this.button3);
            this.tabHatch.Controls.Add(this.label1);
            this.tabHatch.Controls.Add(this.listBox1);
            this.tabHatch.Location = new Point(4, 0x16);
            this.tabHatch.Name = "tabHatch";
            this.tabHatch.Size = new Size(0x112, 0xc5);
            this.tabHatch.TabIndex = 1;
            this.tabHatch.Text = "Hatch";
            this.tabHatch.Visible = false;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0xcc, 100);
            this.label6.Name = "label6";
            this.label6.Size = new Size(14, 0x10);
            this.label6.TabIndex = 7;
            this.label6.Text = "%";
            this.numericUpDown3.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown3.Location = new Point(0x90, 0x60);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x38, 20);
            this.numericUpDown3.TabIndex = 6;
            this.numericUpDown3.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown3.TextChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown3.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0xcd, 40);
            this.label5.Name = "label5";
            this.label5.Size = new Size(14, 0x10);
            this.label5.TabIndex = 4;
            this.label5.Text = "%";
            this.numericUpDown2.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown2.Location = new Point(0x90, 0x24);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x38, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.panelHatch.BorderStyle = BorderStyle.Fixed3D;
            this.panelHatch.Location = new Point(0x88, 0x80);
            this.panelHatch.Name = "panelHatch";
            this.panelHatch.Size = new Size(0x58, 0x20);
            this.panelHatch.TabIndex = 8;
            this.panelHatch.Paint += new PaintEventHandler(this.panelHatch_Paint);
            this.button6.Color = Color.Empty;
            this.button6.Location = new Point(0x88, 8);
            this.button6.Name = "button6";
            this.button6.Size = new Size(120, 0x17);
            this.button6.TabIndex = 2;
            this.button6.Text = "&Background...";
            this.button6.Click += new EventHandler(this.button6_Click);
            this.button3.Color = Color.Empty;
            this.button3.Location = new Point(0x88, 0x40);
            this.button3.Name = "button3";
            this.button3.Size = new Size(120, 0x17);
            this.button3.TabIndex = 5;
            this.button3.Text = "&Foreground...";
            this.button3.Click += new EventHandler(this.button3_Click_1);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Hatch Style:";
            this.listBox1.BorderStyle = BorderStyle.FixedSingle;
            this.listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new Point(8, 0x1b);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(120, 0x86);
            this.listBox1.Sorted = true;
            this.listBox1.TabIndex = 1;
            this.listBox1.DrawItem += new DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged_1);
            this.tabGradient.Location = new Point(4, 0x16);
            this.tabGradient.Name = "tabGradient";
            this.tabGradient.Size = new Size(0x112, 0xc5);
            this.tabGradient.TabIndex = 2;
            this.tabGradient.Text = "Gradient";
            this.tabGradient.Visible = false;
            this.tabImage.Controls.Add(this.comboBox2);
            this.tabImage.Controls.Add(this.label3);
            this.tabImage.Controls.Add(this.comboBox1);
            this.tabImage.Controls.Add(this.label2);
            this.tabImage.Controls.Add(this.pictureBox1);
            this.tabImage.Controls.Add(this.button4);
            this.tabImage.Location = new Point(4, 0x16);
            this.tabImage.Name = "tabImage";
            this.tabImage.Size = new Size(0x112, 0xc5);
            this.tabImage.TabIndex = 3;
            this.tabImage.Text = "Image";
            this.tabImage.Visible = false;
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.Items.AddRange(new object[] { "Stretch", "Tile", "Center", "Normal" });
            this.comboBox2.Location = new Point(8, 0x40);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x58, 0x15);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 0x30);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x21, 0x10);
            this.label3.TabIndex = 8;
            this.label3.Text = "&Style:";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new Point(0x68, 0x60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x48, 0x15);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Visible = false;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged_1);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x27, 0x62);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x39, 0x10);
            this.label2.TabIndex = 6;
            this.label2.Text = "&Tile mode:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label2.Visible = false;
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox1.Location = new Point(0x69, 0x10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x4f, 0x48);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.button4.FlatStyle = FlatStyle.Flat;
            this.button4.Location = new Point(8, 0x10);
            this.button4.Name = "button4";
            this.button4.TabIndex = 4;
            this.button4.Text = "&Browse...";
            this.button4.Click += new EventHandler(this.button4_Click_1);
            base.AcceptButton = this.bOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x11a, 0x117);
            base.Controls.Add(this.panel4);
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "BrushEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Hatch Brush Editor";
            base.Load += new EventHandler(this.BrushEditor_Load);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabHatch.ResumeLayout(false);
            this.numericUpDown3.EndInit();
            this.numericUpDown2.EndInit();
            this.tabImage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index > -1)
            {
                HatchBrush brush = new HatchBrush((HatchStyle) this.listBox1.Items[e.Index], this.brush.ForegroundColor, this.brush.Color);
                e.Graphics.FillRectangle(brush, e.Bounds.Left + 2, e.Bounds.Top + 2, 8, e.Bounds.Height - 4);
                Point point = new Point(14, e.Bounds.Top + ((this.listBox1.ItemHeight - this.listBox1.Font.Height) / 2));
                e.Graphics.DrawString(this.listBox1.Items[e.Index].ToString(), this.listBox1.Font, new SolidBrush(e.ForeColor), (PointF) point);
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.brush.Solid = false;
                this.brush.Style = (HatchStyle) this.listBox1.SelectedItem;
                this.RepaintSample();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.brush.Color = Graphics3D.TransparentColor((int) this.colorChooser.ndTransp.Value, this.brush.Color);
                this.numericUpDown2.Value = this.colorChooser.ndTransp.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.brush.Color = Graphics3D.TransparentColor((int) this.numericUpDown2.Value, this.brush.Color);
                if (this.colorChooser != null)
                {
                    this.colorChooser.ndTransp.Value = this.numericUpDown2.Value;
                }
                this.RepaintSample();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.brush.ForegroundColor = Graphics3D.TransparentColor((int) this.numericUpDown3.Value, this.brush.ForegroundColor);
                this.RepaintSample();
            }
        }

        private void panelHatch_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(this.brush.DrawingBrush, this.panelHatch.ClientRectangle);
        }

        private void RepaintSample()
        {
            this.panelHatch.Invalidate();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.brush != null)
            {
                switch (this.tabControl1.SelectedIndex)
                {
                    case 0:
                        this.brush.Solid = true;
                        this.brush.Gradient.Visible = false;
                        return;

                    case 1:
                        this.brush.Solid = false;
                        this.brush.Gradient.Visible = false;
                        this.RepaintSample();
                        return;

                    case 2:
                        this.brush.Gradient.Visible = true;
                        return;

                    default:
                        return;
                }
            }
        }
    }
}

