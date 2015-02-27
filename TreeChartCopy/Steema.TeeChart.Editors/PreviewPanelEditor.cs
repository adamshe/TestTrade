namespace Steema.TeeChart.Editors
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PreviewPanelEditor : Form
    {
        private Button BPaperColor;
        private Button BShadowColor;
        private Button button1;
        private Button Button2;
        private CheckBox CBAllowMove;
        private CheckBox CBAllowResize;
        private CheckBox CBAsBitmap;
        private CheckBox CBDragImage;
        private CheckBox CBShowImage;
        private Container components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Panel panel1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private NumericUpDown UDShadowSize;

        public PreviewPanelEditor()
        {
            this.InitializeComponent();
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
            this.panel1 = new Panel();
            this.button1 = new Button();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.groupBox1 = new GroupBox();
            this.UDShadowSize = new NumericUpDown();
            this.label1 = new Label();
            this.BShadowColor = new Button();
            this.Button2 = new Button();
            this.BPaperColor = new Button();
            this.CBAsBitmap = new CheckBox();
            this.CBDragImage = new CheckBox();
            this.CBShowImage = new CheckBox();
            this.CBAllowResize = new CheckBox();
            this.CBAllowMove = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.UDShadowSize.BeginInit();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 230);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x14b, 40);
            this.panel1.TabIndex = 0;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(240, 8);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x14b, 230);
            this.tabControl1.TabIndex = 1;
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.Button2);
            this.tabPage1.Controls.Add(this.BPaperColor);
            this.tabPage1.Controls.Add(this.CBAsBitmap);
            this.tabPage1.Controls.Add(this.CBDragImage);
            this.tabPage1.Controls.Add(this.CBShowImage);
            this.tabPage1.Controls.Add(this.CBAllowResize);
            this.tabPage1.Controls.Add(this.CBAllowMove);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x143, 0xcc);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.groupBox1.Controls.Add(this.UDShadowSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.BShadowColor);
            this.groupBox1.Location = new Point(0x10, 0x60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 80);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shadow:";
            this.UDShadowSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDShadowSize.Location = new Point(0x35, 20);
            this.UDShadowSize.Name = "UDShadowSize";
            this.UDShadowSize.Size = new Size(0x38, 20);
            this.UDShadowSize.TabIndex = 2;
            this.UDShadowSize.TextAlign = HorizontalAlignment.Right;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDShadowSize.Value = new decimal(bits);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x15, 0x15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "Si&ze:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.BShadowColor.FlatStyle = FlatStyle.Flat;
            this.BShadowColor.Location = new Point(0x18, 0x30);
            this.BShadowColor.Name = "BShadowColor";
            this.BShadowColor.TabIndex = 0;
            this.BShadowColor.Text = "&Color...";
            this.Button2.FlatStyle = FlatStyle.Flat;
            this.Button2.Location = new Point(240, 0x38);
            this.Button2.Name = "Button2";
            this.Button2.TabIndex = 6;
            this.Button2.Text = "M&argins";
            this.BPaperColor.FlatStyle = FlatStyle.Flat;
            this.BPaperColor.Location = new Point(240, 0x10);
            this.BPaperColor.Name = "BPaperColor";
            this.BPaperColor.TabIndex = 5;
            this.BPaperColor.Text = "C&olor...";
            this.CBAsBitmap.FlatStyle = FlatStyle.Flat;
            this.CBAsBitmap.Location = new Point(0x86, 40);
            this.CBAsBitmap.Name = "CBAsBitmap";
            this.CBAsBitmap.Size = new Size(0x62, 0x10);
            this.CBAsBitmap.TabIndex = 4;
            this.CBAsBitmap.Text = "As &Bitmap";
            this.CBDragImage.FlatStyle = FlatStyle.Flat;
            this.CBDragImage.Location = new Point(0x86, 0x10);
            this.CBDragImage.Name = "CBDragImage";
            this.CBDragImage.Size = new Size(0x62, 0x10);
            this.CBDragImage.TabIndex = 3;
            this.CBDragImage.Text = "&Drag Image";
            this.CBShowImage.Checked = true;
            this.CBShowImage.CheckState = CheckState.Checked;
            this.CBShowImage.FlatStyle = FlatStyle.Flat;
            this.CBShowImage.Location = new Point(14, 0x40);
            this.CBShowImage.Name = "CBShowImage";
            this.CBShowImage.Size = new Size(0x68, 0x10);
            this.CBShowImage.TabIndex = 2;
            this.CBShowImage.Text = "Show &Image";
            this.CBAllowResize.Checked = true;
            this.CBAllowResize.CheckState = CheckState.Checked;
            this.CBAllowResize.FlatStyle = FlatStyle.Flat;
            this.CBAllowResize.Location = new Point(14, 40);
            this.CBAllowResize.Name = "CBAllowResize";
            this.CBAllowResize.Size = new Size(0x68, 0x10);
            this.CBAllowResize.TabIndex = 1;
            this.CBAllowResize.Text = "Allow &Resize";
            this.CBAllowMove.Checked = true;
            this.CBAllowMove.CheckState = CheckState.Checked;
            this.CBAllowMove.FlatStyle = FlatStyle.Flat;
            this.CBAllowMove.Location = new Point(14, 0x10);
            this.CBAllowMove.Name = "CBAllowMove";
            this.CBAllowMove.Size = new Size(0x68, 0x10);
            this.CBAllowMove.TabIndex = 0;
            this.CBAllowMove.Text = "Allow &Move";
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new Point(0x90, 0x60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x70, 80);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Orien&tation:";
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(12, 0x38);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x58, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&Landscape";
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(12, 0x24);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x58, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "P&ortrait";
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(12, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x58, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "&Default";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x14b, 270);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.panel1);
            base.Name = "PreviewPanelEditor";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.UDShadowSize.EndInit();
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

