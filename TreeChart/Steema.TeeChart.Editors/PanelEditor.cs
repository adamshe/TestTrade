namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PanelEditor : Form
    {
        private ButtonColor bColor;
        private ButtonPen button10;
        private Button button7;
        private Button button9;
        private CheckBox CBImageTrans;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private Container components;
        private GradientEditor gradientEditor;
        private GroupBox groupBox1;
        private Label label1;
        private Steema.TeeChart.Panel panel;
        private System.Windows.Forms.Panel panel1;
        private TabControl PanelPages;
        private PictureBox pictureBox1;
        private ShadowEditor shadowEditor;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private TabPage tabPagePanelGradient;
        private TabPage tabShadow;

        public PanelEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PanelEditor(Steema.TeeChart.Panel p, Control parent) : this()
        {
            this.panel = p;
            this.bColor.Color = this.panel.Color;
            this.button10.Pen = this.panel.Pen;
            this.checkBox1.Checked = this.panel.Color == Color.Transparent;
            this.comboBox1.SelectedIndex = EditorUtils.ImageModeToIndex(this.panel.ImageMode);
            new BevelEditor(this.panel.Bevel, this.panel1);
            EditorUtils.InsertForm(this, parent);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.panel.Brush);
            this.bColor.Color = this.panel.Color;
            this.checkBox1.Checked = this.panel.Color == Color.Transparent;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.panel.Color = this.bColor.Color;
            this.checkBox1.Checked = this.panel.Color == Color.Transparent;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.button9.Text == Texts.ClearImage)
            {
                this.panel.Image = null;
                this.pictureBox1.Image = null;
                this.button9.Text = Texts.BrowseImage;
                this.CBImageTrans.Checked = false;
                this.CBImageTrans.Enabled = false;
            }
            else
            {
                string filename = PictureDialog.FileName(this);
                if (filename.Length != 0)
                {
                    this.panel.Image = Image.FromFile(filename);
                    this.pictureBox1.Image = this.panel.Image;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.button9.Text = Texts.ClearImage;
                    this.CBImageTrans.Enabled = true;
                }
            }
        }

        private void CBImageTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CBImageTrans.Checked)
            {
                this.panel.ImageTransparent = true;
            }
            else
            {
                this.panel.ImageTransparent = false;
            }
        }

        private void CBImageTrans_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.panel.Color = Color.Transparent;
                if (this.panel.chart.parent != null)
                {
                    this.panel.chart.parent.DoSetControlStyle();
                }
            }
            else if ((this.panel.Color == Color.FromArgb(0, 0xff, 0xff, 0xff)) || (this.panel.Color == Color.Transparent))
            {
                this.panel.Color = Color.Empty;
                if ((this.panel.Color == Color.FromArgb(0, 0, 0, 0)) || (this.panel.Color == Color.Empty))
                {
                    this.panel.Color = Color.FromArgb(0xff, SystemColors.Control.R, SystemColors.Control.G, SystemColors.Control.B);
                }
            }
            this.bColor.Color = this.panel.Color;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel.ImageMode = EditorUtils.IndexToImageMode(this.comboBox1.SelectedIndex);
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
            this.PanelPages = new TabControl();
            this.tabPage7 = new TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button10 = new ButtonPen();
            this.tabPage6 = new TabPage();
            this.checkBox1 = new CheckBox();
            this.button7 = new Button();
            this.groupBox1 = new GroupBox();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.pictureBox1 = new PictureBox();
            this.button9 = new Button();
            this.CBImageTrans = new CheckBox();
            this.bColor = new ButtonColor();
            this.tabPagePanelGradient = new TabPage();
            this.tabShadow = new TabPage();
            this.PanelPages.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.PanelPages.Controls.Add(this.tabPage7);
            this.PanelPages.Controls.Add(this.tabPage6);
            this.PanelPages.Controls.Add(this.tabPagePanelGradient);
            this.PanelPages.Controls.Add(this.tabShadow);
            this.PanelPages.Dock = DockStyle.Fill;
            this.PanelPages.HotTrack = true;
            this.PanelPages.Location = new Point(0, 0);
            this.PanelPages.Name = "PanelPages";
            this.PanelPages.SelectedIndex = 0;
            this.PanelPages.Size = new Size(0x130, 0xed);
            this.PanelPages.TabIndex = 1;
            this.PanelPages.SelectedIndexChanged += new EventHandler(this.PanelPages_SelectedIndexChanged);
            this.tabPage7.Controls.Add(this.panel1);
            this.tabPage7.Controls.Add(this.button10);
            this.tabPage7.Location = new Point(4, 0x16);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new Size(0x128, 0xd3);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Borders";
            this.panel1.Location = new Point(0x10, 0x38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xe0, 0x88);
            this.panel1.TabIndex = 2;
            this.button10.FlatStyle = FlatStyle.Flat;
            this.button10.Location = new Point(0x10, 0x10);
            this.button10.Name = "button10";
            this.button10.TabIndex = 1;
            this.button10.Text = "&Border...";
            this.tabPage6.Controls.Add(this.checkBox1);
            this.tabPage6.Controls.Add(this.button7);
            this.tabPage6.Controls.Add(this.groupBox1);
            this.tabPage6.Controls.Add(this.bColor);
            this.tabPage6.Location = new Point(4, 0x16);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new Size(0x128, 0xd3);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Background";
            this.tabPage6.Visible = false;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x10, 0x26);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x88, 0x18);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "&Transparent";
            this.checkBox1.Click += new EventHandler(this.checkBox1_Click);
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.button7.FlatStyle = FlatStyle.Flat;
            this.button7.Location = new Point(0x80, 8);
            this.button7.Name = "button7";
            this.button7.TabIndex = 2;
            this.button7.Text = "&Pattern...";
            this.button7.Click += new EventHandler(this.button7_Click);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.CBImageTrans);
            this.groupBox1.Location = new Point(0x10, 0x40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(200, 0x7f);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Background Image:";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "Stretch", "Tile", "Center", "Normal" });
            this.comboBox1.Location = new Point(0x10, 0x4c);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x58, 0x15);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 0x3b);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Style:";
            this.pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox1.Location = new Point(0x77, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x43, 0x4c);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.button9.FlatStyle = FlatStyle.Flat;
            this.button9.Location = new Point(14, 0x18);
            this.button9.Name = "button9";
            this.button9.TabIndex = 0;
            this.button9.Text = "&Browse...";
            this.button9.Click += new EventHandler(this.button9_Click);
            this.CBImageTrans.FlatStyle = FlatStyle.Flat;
            this.CBImageTrans.Location = new Point(0x11, 0x62);
            this.CBImageTrans.Name = "CBImageTrans";
            this.CBImageTrans.Size = new Size(0x88, 0x18);
            this.CBImageTrans.TabIndex = 4;
            this.CBImageTrans.Text = "&Transparent";
            this.CBImageTrans.Click += new EventHandler(this.CBImageTrans_Click);
            this.CBImageTrans.CheckedChanged += new EventHandler(this.CBImageTrans_CheckedChanged);
            this.bColor.Color = Color.Empty;
            this.bColor.Location = new Point(0x10, 8);
            this.bColor.Name = "bColor";
            this.bColor.TabIndex = 0;
            this.bColor.Text = "&Color...";
            this.bColor.Click += new EventHandler(this.button8_Click);
            this.tabPagePanelGradient.Location = new Point(4, 0x16);
            this.tabPagePanelGradient.Name = "tabPagePanelGradient";
            this.tabPagePanelGradient.Size = new Size(0x128, 0xd3);
            this.tabPagePanelGradient.TabIndex = 2;
            this.tabPagePanelGradient.Text = "Gradient";
            this.tabPagePanelGradient.Visible = false;
            this.tabShadow.Location = new Point(4, 0x16);
            this.tabShadow.Name = "tabShadow";
            this.tabShadow.Size = new Size(0x128, 0xd3);
            this.tabShadow.TabIndex = 3;
            this.tabShadow.Text = "Shadow";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x130, 0xed);
            base.Controls.Add(this.PanelPages);
            base.Name = "PanelEditor";
            this.Text = "Panel Editor";
            this.PanelPages.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void PanelPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.PanelPages.SelectedTab == this.tabPagePanelGradient) & (this.gradientEditor == null))
            {
                this.gradientEditor = new GradientEditor(this.panel.Gradient, this.tabPagePanelGradient);
                EditorUtils.Translate(this.gradientEditor);
            }
            else if ((this.PanelPages.SelectedTab == this.tabShadow) & (this.shadowEditor == null))
            {
                this.shadowEditor = new ShadowEditor(this.panel.Shadow, this.tabShadow);
                EditorUtils.Translate(this.shadowEditor);
            }
            else if (this.PanelPages.SelectedTab == this.tabPage6)
            {
                this.checkBox1.Checked = (this.panel.Color == Color.Transparent) || (this.panel.Color == Color.FromArgb(0, 0xff, 0xff, 0xff));
                if (this.panel.Image != null)
                {
                    this.pictureBox1.Image = this.panel.Image;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.button9.Text = Texts.ClearImage;
                    this.CBImageTrans.Enabled = true;
                    this.CBImageTrans.Checked = this.panel.ImageTransparent;
                }
                else
                {
                    this.CBImageTrans.Enabled = false;
                }
            }
        }
    }
}

