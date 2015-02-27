namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AnnotationEditor : Form
    {
        private Annotation annotation;
        private ButtonPen BCalloutPen;
        private Button BCalloutPointer;
        private ComboBox cbCursor;
        private CheckBox CBCustPos;
        private ComboBox CBHead;
        private ComboBox CBPos;
        private ComboBox cbTextAlign;
        private Container components;
        private NumericUpDown EX;
        private NumericUpDown EY;
        private NumericUpDown EZ;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox MemoText;
        private CustomShapeEditor shapeForm;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Steema.TeeChart.Tools.Tool tool;
        private NumericUpDown UDArrowDist;
        private NumericUpDown UDHeadSize;
        private NumericUpDown UDLeft;
        private NumericUpDown UDTop;

        public AnnotationEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AnnotationEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.tool = s;
            this.annotation = (Annotation) s;
            this.shapeForm = CustomShapeEditor.Add(this.tabControl1, this.annotation.Shape);
            if (s is PageNumber)
            {
                this.MemoText.Text = ((PageNumber) s).Format;
            }
            else
            {
                this.MemoText.Text = this.annotation.Text;
            }
            this.CBCustPos.Checked = this.annotation.Shape.CustomPosition;
            this.UDLeft.Value = this.annotation.Left;
            this.UDTop.Value = this.annotation.Top;
            this.UDLeft.Enabled = this.CBCustPos.Checked;
            this.UDTop.Enabled = this.CBCustPos.Checked;
            switch (this.annotation.TextAlign)
            {
                case StringAlignment.Near:
                    this.cbTextAlign.SelectedIndex = 0;
                    break;

                case StringAlignment.Center:
                    this.cbTextAlign.SelectedIndex = 1;
                    break;

                default:
                    this.cbTextAlign.SelectedIndex = 2;
                    break;
            }
            this.CBPos.SelectedIndex = (int) this.annotation.Position;
            this.BCalloutPen.Pen = this.annotation.Callout.Arrow;
            this.EX.Value = this.annotation.Callout.XPosition;
            this.EY.Value = this.annotation.Callout.YPosition;
            this.EZ.Value = this.annotation.Callout.ZPosition;
            this.UDArrowDist.Value = this.annotation.Callout.Distance;
            this.CBHead.SelectedIndex = (int) this.annotation.Callout.ArrowHead;
            this.UDHeadSize.Value = this.annotation.Callout.ArrowHeadSize;
            EditorUtils.FillCursors(this.cbCursor, this.annotation.Cursor);
        }

        private void BCalloutPointer_Click(object sender, EventArgs e)
        {
            EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.SeriesPointer(this.annotation.Callout));
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.annotation.Cursor = EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString());
        }

        private void CBCustPos_CheckedChanged(object sender, EventArgs e)
        {
            this.annotation.Shape.CustomPosition = this.CBCustPos.Checked;
            this.UDLeft.Enabled = this.CBCustPos.Checked;
            this.UDTop.Enabled = this.CBCustPos.Checked;
        }

        private void CBHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.annotation.Callout.ArrowHead = (ArrowHeadStyles) this.CBHead.SelectedIndex;
        }

        private void CBPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.annotation.Position = (AnnotationPositions) this.CBPos.SelectedIndex;
        }

        private void cbTextAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cbTextAlign.SelectedIndex)
            {
                case 0:
                    this.annotation.TextAlign = StringAlignment.Near;
                    return;

                case 1:
                    this.annotation.TextAlign = StringAlignment.Center;
                    return;
            }
            this.annotation.TextAlign = StringAlignment.Far;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EX_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Callout.XPosition = (int) this.EX.Value;
        }

        private void EY_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Callout.YPosition = (int) this.EY.Value;
        }

        private void EZ_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Callout.ZPosition = (int) this.EZ.Value;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.cbCursor = new ComboBox();
            this.cbTextAlign = new ComboBox();
            this.label6 = new Label();
            this.label5 = new Label();
            this.MemoText = new TextBox();
            this.label1 = new Label();
            this.tabPage2 = new TabPage();
            this.UDLeft = new NumericUpDown();
            this.UDTop = new NumericUpDown();
            this.label4 = new Label();
            this.label3 = new Label();
            this.CBCustPos = new CheckBox();
            this.label2 = new Label();
            this.CBPos = new ComboBox();
            this.tabPage3 = new TabPage();
            this.UDHeadSize = new NumericUpDown();
            this.label12 = new Label();
            this.CBHead = new ComboBox();
            this.label11 = new Label();
            this.UDArrowDist = new NumericUpDown();
            this.label10 = new Label();
            this.groupBox1 = new GroupBox();
            this.EZ = new NumericUpDown();
            this.EY = new NumericUpDown();
            this.label9 = new Label();
            this.label8 = new Label();
            this.label7 = new Label();
            this.EX = new NumericUpDown();
            this.BCalloutPointer = new Button();
            this.BCalloutPen = new ButtonPen();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.UDLeft.BeginInit();
            this.UDTop.BeginInit();
            this.tabPage3.SuspendLayout();
            this.UDHeadSize.BeginInit();
            this.UDArrowDist.BeginInit();
            this.groupBox1.SuspendLayout();
            this.EZ.BeginInit();
            this.EY.BeginInit();
            this.EX.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x110, 0xd5);
            this.tabControl1.TabIndex = 0;
            this.tabPage1.Controls.Add(this.cbCursor);
            this.tabPage1.Controls.Add(this.cbTextAlign);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.MemoText);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x108, 0xbb);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.cbCursor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursor.Location = new Point(0x99, 0x90);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x60, 0x15);
            this.cbCursor.TabIndex = 5;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.cbTextAlign.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbTextAlign.Items.AddRange(new object[] { "Left", "Center", "Right" });
            this.cbTextAlign.Location = new Point(0x10, 0x90);
            this.cbTextAlign.Name = "cbTextAlign";
            this.cbTextAlign.Size = new Size(0x79, 0x15);
            this.cbTextAlign.TabIndex = 3;
            this.cbTextAlign.SelectedIndexChanged += new EventHandler(this.cbTextAlign_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x98, 0x80);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x29, 0x10);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Cursor:";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x10, 0x80);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x52, 0x10);
            this.label5.TabIndex = 2;
            this.label5.Text = "Text &alignment:";
            this.MemoText.BorderStyle = BorderStyle.FixedSingle;
            this.MemoText.Location = new Point(0x10, 0x20);
            this.MemoText.Multiline = true;
            this.MemoText.Name = "MemoText";
            this.MemoText.Size = new Size(0xe8, 0x58);
            this.MemoText.TabIndex = 1;
            this.MemoText.Text = "";
            this.MemoText.TextChanged += new EventHandler(this.MemoText_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Text:";
            this.tabPage2.Controls.Add(this.UDLeft);
            this.tabPage2.Controls.Add(this.UDTop);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.CBCustPos);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.CBPos);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x108, 0xbb);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Position";
            this.UDLeft.BorderStyle = BorderStyle.FixedSingle;
            this.UDLeft.Enabled = false;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDLeft.Increment = new decimal(bits);
            this.UDLeft.Location = new Point(80, 0x58);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDLeft.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.UDLeft.Minimum = new decimal(bits);
            this.UDLeft.Name = "UDLeft";
            this.UDLeft.Size = new Size(0x38, 20);
            this.UDLeft.TabIndex = 4;
            this.UDLeft.TextAlign = HorizontalAlignment.Right;
            this.UDLeft.ValueChanged += new EventHandler(this.UDLeft_ValueChanged);
            this.UDTop.BorderStyle = BorderStyle.FixedSingle;
            this.UDTop.Enabled = false;
            bits = new int[4];
            bits[0] = 5;
            this.UDTop.Increment = new decimal(bits);
            this.UDTop.Location = new Point(80, 120);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDTop.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 100;
            bits[3] = -2147483648;
            this.UDTop.Minimum = new decimal(bits);
            this.UDTop.Name = "UDTop";
            this.UDTop.Size = new Size(0x38, 20);
            this.UDTop.TabIndex = 6;
            this.UDTop.TextAlign = HorizontalAlignment.Right;
            this.UDTop.ValueChanged += new EventHandler(this.UDTop_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x2f, 0x7a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1b, 0x10);
            this.label4.TabIndex = 5;
            this.label4.Text = "T&op:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x30, 90);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1a, 0x10);
            this.label3.TabIndex = 3;
            this.label3.Text = "L&eft:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.CBCustPos.FlatStyle = FlatStyle.Flat;
            this.CBCustPos.Location = new Point(80, 0x38);
            this.CBCustPos.Name = "CBCustPos";
            this.CBCustPos.Size = new Size(0x51, 0x10);
            this.CBCustPos.TabIndex = 2;
            this.CBCustPos.Text = "&Custom";
            this.CBCustPos.CheckedChanged += new EventHandler(this.CBCustPos_CheckedChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(40, 0x18);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1f, 0x10);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Auto:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.CBPos.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBPos.Items.AddRange(new object[] { "Left top", "Left bottom", "Right top", "Right bottom" });
            this.CBPos.Location = new Point(80, 20);
            this.CBPos.Name = "CBPos";
            this.CBPos.Size = new Size(0x88, 0x15);
            this.CBPos.TabIndex = 1;
            this.CBPos.SelectedIndexChanged += new EventHandler(this.CBPos_SelectedIndexChanged);
            this.tabPage3.Controls.Add(this.UDHeadSize);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.CBHead);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.UDArrowDist);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.BCalloutPointer);
            this.tabPage3.Controls.Add(this.BCalloutPen);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0x108, 0xbb);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Callout";
            this.UDHeadSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDHeadSize.Location = new Point(200, 0x88);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDHeadSize.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDHeadSize.Minimum = new decimal(bits);
            this.UDHeadSize.Name = "UDHeadSize";
            this.UDHeadSize.Size = new Size(0x30, 20);
            this.UDHeadSize.TabIndex = 8;
            this.UDHeadSize.TextAlign = HorizontalAlignment.Right;
            this.UDHeadSize.ValueChanged += new EventHandler(this.UDHeadSize_ValueChanged);
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0xac, 0x8a);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x1d, 0x10);
            this.label12.TabIndex = 7;
            this.label12.Text = "&Size:";
            this.CBHead.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBHead.Items.AddRange(new object[] { "None", "Line", "Solid" });
            this.CBHead.Location = new Point(0x90, 0x68);
            this.CBHead.Name = "CBHead";
            this.CBHead.Size = new Size(0x68, 0x15);
            this.CBHead.TabIndex = 6;
            this.CBHead.SelectedIndexChanged += new EventHandler(this.CBHead_SelectedIndexChanged);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x90, 0x58);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x41, 0x10);
            this.label11.TabIndex = 5;
            this.label11.Text = "&Arrow head:";
            this.UDArrowDist.BorderStyle = BorderStyle.FixedSingle;
            this.UDArrowDist.Location = new Point(0xc2, 0x36);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDArrowDist.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDArrowDist.Minimum = new decimal(bits);
            this.UDArrowDist.Name = "UDArrowDist";
            this.UDArrowDist.Size = new Size(0x36, 20);
            this.UDArrowDist.TabIndex = 4;
            this.UDArrowDist.TextAlign = HorizontalAlignment.Right;
            this.UDArrowDist.ValueChanged += new EventHandler(this.UDArrowDist_ValueChanged);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x90, 0x38);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x33, 0x10);
            this.label10.TabIndex = 3;
            this.label10.Text = "&Distance:";
            this.label10.TextAlign = ContentAlignment.TopRight;
            this.groupBox1.Controls.Add(this.EZ);
            this.groupBox1.Controls.Add(this.EY);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.EX);
            this.groupBox1.Location = new Point(0x10, 0x38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x70, 0x68);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "P&osition";
            this.EZ.BorderStyle = BorderStyle.FixedSingle;
            this.EZ.Location = new Point(40, 70);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.EZ.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.EZ.Minimum = new decimal(bits);
            this.EZ.Name = "EZ";
            this.EZ.Size = new Size(0x40, 20);
            this.EZ.TabIndex = 11;
            this.EZ.TextAlign = HorizontalAlignment.Right;
            this.EZ.ValueChanged += new EventHandler(this.EZ_ValueChanged);
            this.EY.BorderStyle = BorderStyle.FixedSingle;
            this.EY.Location = new Point(40, 0x2e);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.EY.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.EY.Minimum = new decimal(bits);
            this.EY.Name = "EY";
            this.EY.Size = new Size(0x40, 20);
            this.EY.TabIndex = 10;
            this.EY.TextAlign = HorizontalAlignment.Right;
            this.EY.ValueChanged += new EventHandler(this.EY_ValueChanged);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x18, 0x48);
            this.label9.Name = "label9";
            this.label9.Size = new Size(14, 0x10);
            this.label9.TabIndex = 7;
            this.label9.Text = "&Z:";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x18, 0x30);
            this.label8.Name = "label8";
            this.label8.Size = new Size(15, 0x10);
            this.label8.TabIndex = 6;
            this.label8.Text = "&Y:";
            this.label8.TextAlign = ContentAlignment.TopRight;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x18, 0x18);
            this.label7.Name = "label7";
            this.label7.Size = new Size(15, 0x10);
            this.label7.TabIndex = 3;
            this.label7.Text = "&X:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.EX.BorderStyle = BorderStyle.FixedSingle;
            this.EX.Location = new Point(40, 0x16);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.EX.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.EX.Minimum = new decimal(bits);
            this.EX.Name = "EX";
            this.EX.Size = new Size(0x40, 20);
            this.EX.TabIndex = 9;
            this.EX.TextAlign = HorizontalAlignment.Right;
            this.EX.ValueChanged += new EventHandler(this.EX_ValueChanged);
            this.BCalloutPointer.FlatStyle = FlatStyle.Flat;
            this.BCalloutPointer.Location = new Point(0x60, 0x10);
            this.BCalloutPointer.Name = "BCalloutPointer";
            this.BCalloutPointer.Size = new Size(0x48, 0x18);
            this.BCalloutPointer.TabIndex = 1;
            this.BCalloutPointer.Text = "&Pointer...";
            this.BCalloutPointer.Click += new EventHandler(this.BCalloutPointer_Click);
            this.BCalloutPen.FlatStyle = FlatStyle.Flat;
            this.BCalloutPen.Location = new Point(0x10, 0x10);
            this.BCalloutPen.Name = "BCalloutPen";
            this.BCalloutPen.Size = new Size(0x48, 0x18);
            this.BCalloutPen.TabIndex = 0;
            this.BCalloutPen.Text = "&Border...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x110, 0xd5);
            base.Controls.Add(this.tabControl1);
            base.Name = "AnnotationEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.UDLeft.EndInit();
            this.UDTop.EndInit();
            this.tabPage3.ResumeLayout(false);
            this.UDHeadSize.EndInit();
            this.UDArrowDist.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.EZ.EndInit();
            this.EY.EndInit();
            this.EX.EndInit();
            base.ResumeLayout(false);
        }

        private void MemoText_TextChanged(object sender, EventArgs e)
        {
            if (this.tool is PageNumber)
            {
                string text = this.MemoText.Text;
                while (text.IndexOf("{") != -1)
                {
                    int index = text.IndexOf("{");
                    text = text.Substring(index + 1);
                    if ((text.IndexOf("}") == -1) || ((text.IndexOf("{") != -1) && (text.IndexOf("{") < text.IndexOf("}"))))
                    {
                        return;
                    }
                }
                text = this.MemoText.Text;
                if (((text.IndexOf("}") == -1) || (text.IndexOf("{") != -1)) && ((text.IndexOf("}") == -1) || (text.IndexOf("}") >= text.IndexOf("{"))))
                {
                    this.annotation.Text = this.MemoText.Text;
                }
            }
            else
            {
                this.annotation.Text = this.MemoText.Text;
            }
        }

        private void UDArrowDist_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Callout.Distance = (int) this.UDArrowDist.Value;
        }

        private void UDHeadSize_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Callout.ArrowHeadSize = (int) this.UDHeadSize.Value;
        }

        private void UDLeft_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Left = (int) this.UDLeft.Value;
        }

        private void UDTop_ValueChanged(object sender, EventArgs e)
        {
            this.annotation.Top = (int) this.UDTop.Value;
        }
    }
}

