namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SeriesPointer : Form
    {
        private ButtonPen BPoinPenCol;
        private Button BPointFillColor;
        private CheckBox CB3dPoint;
        private CheckBox CBColorEach;
        private CheckBox CBDefBrushColor;
        internal CheckBox CBDrawPoint;
        private CheckBox CBInflate;
        private CheckBox CBPoDark;
        private ComboBox CBStyle;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Steema.TeeChart.Styles.SeriesPointer point;
        private NumericUpDown UDPointHorizSize;
        private NumericUpDown UDPointVertSize;
        private NumericUpDown udTransp;

        public SeriesPointer()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public SeriesPointer(Steema.TeeChart.Styles.SeriesPointer p) : this(p, null)
        {
        }

        public SeriesPointer(Steema.TeeChart.Styles.SeriesPointer p, Control parent) : this()
        {
            this.point = p;
            this.CBDrawPoint.Checked = this.point.Visible;
            this.CB3dPoint.Checked = this.point.Draw3D;
            this.CBInflate.Checked = this.point.InflateMargins;
            this.CBPoDark.Checked = this.point.Dark3D;
            this.CBColorEach.Visible = p.Series != null;
            if (p.Series != null)
            {
                this.CBColorEach.Checked = this.point.Series.ColorEach;
            }
            this.UDPointHorizSize.Value = this.point.HorizSize;
            this.UDPointVertSize.Value = this.point.VertSize;
            this.CBStyle.SelectedIndex = (int) this.point.Style;
            this.udTransp.Value = this.point.Transparency;
            this.BPoinPenCol.Pen = this.point.Pen;
            EditorUtils.Translate(this);
            EditorUtils.InsertForm(this, parent);
        }

        private void BPointFillColor_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.point.Brush);
            this.udTransp.Value = this.point.Transparency;
            this.CBDefBrushColor.Checked = false;
        }

        private void CB3dPoint_CheckedChanged(object sender, EventArgs e)
        {
            this.point.Draw3D = this.CB3dPoint.Checked;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.point.Series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBDefBrushColor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CBDefBrushColor.Checked)
            {
                this.point.Brush.Transparency = 0;
                this.point.Brush.Color = Color.Empty;
            }
        }

        private void CBDrawPoint_CheckedChanged(object sender, EventArgs e)
        {
            this.point.Visible = this.CBDrawPoint.Checked;
        }

        private void CBInflate_CheckedChanged(object sender, EventArgs e)
        {
            this.point.InflateMargins = this.CBInflate.Checked;
        }

        private void CBPoDark_CheckedChanged(object sender, EventArgs e)
        {
            this.point.Dark3D = this.CBPoDark.Checked;
        }

        private void CBStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBStyle.SelectedIndex)
            {
                case 0:
                    this.point.Style = PointerStyles.Rectangle;
                    return;

                case 1:
                    this.point.Style = PointerStyles.Circle;
                    return;

                case 2:
                    this.point.Style = PointerStyles.Triangle;
                    return;

                case 3:
                    this.point.Style = PointerStyles.DownTriangle;
                    return;

                case 4:
                    this.point.Style = PointerStyles.Cross;
                    return;

                case 5:
                    this.point.Style = PointerStyles.DiagCross;
                    return;

                case 6:
                    this.point.Style = PointerStyles.Star;
                    return;

                case 7:
                    this.point.Style = PointerStyles.Diamond;
                    return;

                case 8:
                    this.point.Style = PointerStyles.SmallDot;
                    return;

                case 9:
                    this.point.Style = PointerStyles.Nothing;
                    return;

                case 10:
                    this.point.Style = PointerStyles.LeftTriangle;
                    return;

                case 11:
                    this.point.Style = PointerStyles.RightTriangle;
                    return;

                case 12:
                    this.point.Style = PointerStyles.Sphere;
                    return;

                case 13:
                    this.point.Style = PointerStyles.PolishedSphere;
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

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.label3 = new Label();
            this.CBStyle = new ComboBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.CBPoDark = new CheckBox();
            this.CBInflate = new CheckBox();
            this.CB3dPoint = new CheckBox();
            this.CBDrawPoint = new CheckBox();
            this.UDPointVertSize = new NumericUpDown();
            this.UDPointHorizSize = new NumericUpDown();
            this.BPoinPenCol = new ButtonPen();
            this.groupBox2 = new GroupBox();
            this.CBDefBrushColor = new CheckBox();
            this.BPointFillColor = new Button();
            this.CBColorEach = new CheckBox();
            this.label4 = new Label();
            this.udTransp = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.UDPointVertSize.BeginInit();
            this.UDPointHorizSize.BeginInit();
            this.groupBox2.SuspendLayout();
            this.udTransp.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.CBStyle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CBPoDark);
            this.groupBox1.Controls.Add(this.CBInflate);
            this.groupBox1.Controls.Add(this.CB3dPoint);
            this.groupBox1.Controls.Add(this.CBDrawPoint);
            this.groupBox1.Controls.Add(this.UDPointVertSize);
            this.groupBox1.Controls.Add(this.UDPointHorizSize);
            this.groupBox1.Controls.Add(this.BPoinPenCol);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x130, 0x90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x88, 0x12);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x21, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Style:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.CBStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBStyle.Items.AddRange(new object[] { "Square", "Circle", "Triangle", "Down Triangle", "Cross", "Diagonal Cross", "Star", "Diamond", "Small Dot", "Nothing", "Left Triangle", "Right Triangle", "Sphere", "Polished Sphere" });
            this.CBStyle.Location = new Point(0xac, 0x10);
            this.CBStyle.Name = "CBStyle";
            this.CBStyle.Size = new Size(0x7c, 0x15);
            this.CBStyle.TabIndex = 5;
            this.CBStyle.SelectedIndexChanged += new EventHandler(this.CBStyle_SelectedIndexChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(200, 0x4f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 0x10);
            this.label2.TabIndex = 8;
            this.label2.Text = "&Height:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xcc, 0x35);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 0x10);
            this.label1.TabIndex = 6;
            this.label1.Text = "&Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBPoDark.FlatStyle = FlatStyle.Flat;
            this.CBPoDark.Location = new Point(0x48, 0x29);
            this.CBPoDark.Name = "CBPoDark";
            this.CBPoDark.Size = new Size(0x4c, 0x10);
            this.CBPoDark.TabIndex = 2;
            this.CBPoDark.Text = "Dar&k 3D";
            this.CBPoDark.CheckedChanged += new EventHandler(this.CBPoDark_CheckedChanged);
            this.CBInflate.FlatStyle = FlatStyle.Flat;
            this.CBInflate.Location = new Point(12, 0x44);
            this.CBInflate.Name = "CBInflate";
            this.CBInflate.Size = new Size(0x70, 0x10);
            this.CBInflate.TabIndex = 3;
            this.CBInflate.Text = "Inflate &Margins";
            this.CBInflate.CheckedChanged += new EventHandler(this.CBInflate_CheckedChanged);
            this.CB3dPoint.FlatStyle = FlatStyle.Flat;
            this.CB3dPoint.Location = new Point(12, 40);
            this.CB3dPoint.Name = "CB3dPoint";
            this.CB3dPoint.Size = new Size(0x2c, 0x10);
            this.CB3dPoint.TabIndex = 1;
            this.CB3dPoint.Text = "&3D";
            this.CB3dPoint.CheckedChanged += new EventHandler(this.CB3dPoint_CheckedChanged);
            this.CBDrawPoint.FlatStyle = FlatStyle.Flat;
            this.CBDrawPoint.Location = new Point(12, 0x10);
            this.CBDrawPoint.Name = "CBDrawPoint";
            this.CBDrawPoint.Size = new Size(80, 0x10);
            this.CBDrawPoint.TabIndex = 0;
            this.CBDrawPoint.Text = "&Visible";
            this.CBDrawPoint.CheckedChanged += new EventHandler(this.CBDrawPoint_CheckedChanged);
            this.UDPointVertSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDPointVertSize.Location = new Point(0xf4, 0x4d);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.UDPointVertSize.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDPointVertSize.Minimum = new decimal(bits);
            this.UDPointVertSize.Name = "UDPointVertSize";
            this.UDPointVertSize.Size = new Size(0x34, 20);
            this.UDPointVertSize.TabIndex = 9;
            this.UDPointVertSize.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDPointVertSize.Value = new decimal(bits);
            this.UDPointVertSize.TextChanged += new EventHandler(this.UDPointVertSize_ValueChanged);
            this.UDPointVertSize.ValueChanged += new EventHandler(this.UDPointVertSize_ValueChanged);
            this.UDPointHorizSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDPointHorizSize.Location = new Point(0xf4, 0x33);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDPointHorizSize.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDPointHorizSize.Minimum = new decimal(bits);
            this.UDPointHorizSize.Name = "UDPointHorizSize";
            this.UDPointHorizSize.Size = new Size(0x34, 20);
            this.UDPointHorizSize.TabIndex = 7;
            this.UDPointHorizSize.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDPointHorizSize.Value = new decimal(bits);
            this.UDPointHorizSize.TextChanged += new EventHandler(this.UDPointHorizSize_ValueChanged);
            this.UDPointHorizSize.ValueChanged += new EventHandler(this.UDPointHorizSize_ValueChanged);
            this.BPoinPenCol.FlatStyle = FlatStyle.Flat;
            this.BPoinPenCol.Location = new Point(220, 0x6b);
            this.BPoinPenCol.Name = "BPoinPenCol";
            this.BPoinPenCol.TabIndex = 11;
            this.BPoinPenCol.Text = "B&order...";
            this.groupBox2.Controls.Add(this.CBDefBrushColor);
            this.groupBox2.Controls.Add(this.BPointFillColor);
            this.groupBox2.Location = new Point(8, 0x5f);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xc4, 0x2a);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.CBDefBrushColor.FlatStyle = FlatStyle.Flat;
            this.CBDefBrushColor.Location = new Point(0x66, 15);
            this.CBDefBrushColor.Name = "CBDefBrushColor";
            this.CBDefBrushColor.Size = new Size(0x52, 0x10);
            this.CBDefBrushColor.TabIndex = 1;
            this.CBDefBrushColor.Text = "&Default";
            this.CBDefBrushColor.CheckedChanged += new EventHandler(this.CBDefBrushColor_CheckedChanged);
            this.BPointFillColor.FlatStyle = FlatStyle.Flat;
            this.BPointFillColor.Location = new Point(13, 11);
            this.BPointFillColor.Name = "BPointFillColor";
            this.BPointFillColor.TabIndex = 0;
            this.BPointFillColor.Text = "&Pattern...";
            this.BPointFillColor.Click += new EventHandler(this.BPointFillColor_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(8, 0x99);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x70, 0x10);
            this.CBColorEach.TabIndex = 1;
            this.CBColorEach.Text = "&Color Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(160, 0x9b);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4d, 0x10);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Transparency:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.udTransp.BorderStyle = BorderStyle.FixedSingle;
            this.udTransp.Location = new Point(0xf8, 0x99);
            this.udTransp.Name = "udTransp";
            this.udTransp.Size = new Size(0x34, 20);
            this.udTransp.TabIndex = 3;
            this.udTransp.TextAlign = HorizontalAlignment.Right;
            this.udTransp.TextChanged += new EventHandler(this.udTransp_ValueChanged);
            this.udTransp.ValueChanged += new EventHandler(this.udTransp_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x135, 0xb6);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.udTransp);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.groupBox1);
            base.Name = "SeriesPointer";
            this.Text = "Pointer";
            this.groupBox1.ResumeLayout(false);
            this.UDPointVertSize.EndInit();
            this.UDPointHorizSize.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.udTransp.EndInit();
            base.ResumeLayout(false);
        }

        public static Steema.TeeChart.Editors.SeriesPointer InsertPointer(Control parent, Steema.TeeChart.Styles.SeriesPointer pointer)
        {
            return InsertPointer(parent, pointer, Texts.GalleryPoint);
        }

        public static Steema.TeeChart.Editors.SeriesPointer InsertPointer(Control parent, Steema.TeeChart.Styles.SeriesPointer pointer, string title)
        {
            TabControl control = (TabControl) ((TabPage) parent).Parent;
            TabPage page = new TabPage(title);
            control.TabPages.Add(page);
            Steema.TeeChart.Editors.SeriesPointer pointer2 = new Steema.TeeChart.Editors.SeriesPointer(pointer, page);
            TabPage page2 = control.TabPages[1];
            control.TabPages[1] = page;
            control.TabPages[control.TabCount - 1] = page2;
            return pointer2;
        }

        public static bool ShowModal(Steema.TeeChart.Styles.SeriesPointer p)
        {
            using (Steema.TeeChart.Editors.SeriesPointer pointer = new Steema.TeeChart.Editors.SeriesPointer(p))
            {
                pointer.StartPosition = FormStartPosition.CenterParent;
                return (pointer.ShowDialog() == DialogResult.OK);
            }
        }

        private void UDPointHorizSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.point != null)
            {
                this.point.HorizSize = (int) this.UDPointHorizSize.Value;
            }
        }

        private void UDPointVertSize_ValueChanged(object sender, EventArgs e)
        {
            if (this.point != null)
            {
                this.point.VertSize = (int) this.UDPointVertSize.Value;
            }
        }

        private void udTransp_ValueChanged(object sender, EventArgs e)
        {
            if (this.point != null)
            {
                this.point.Transparency = (int) this.udTransp.Value;
            }
        }
    }
}

