namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ImageEditor : ToolSeriesEditor
    {
        private Button button1;
        private ComboBox comboBox1;
        private IContainer components;
        private GroupBox groupBox1;
        private ChartImage image;
        private Label label2;
        private PictureBox pictureBox1;

        public ImageEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ImageEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.image = (ChartImage) s;
            base.SetTool(this.image);
            if (this.image != null)
            {
                this.CheckImageButton();
                switch (this.image.ImageMode)
                {
                    case ImageMode.Stretch:
                        this.comboBox1.SelectedIndex = 1;
                        return;

                    case ImageMode.Tile:
                        this.comboBox1.SelectedIndex = 3;
                        return;

                    case ImageMode.Center:
                        this.comboBox1.SelectedIndex = 2;
                        return;
                }
                this.comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (((int) this.button1.Tag) == 1)
            {
                this.image.Image = null;
            }
            else
            {
                string filename = PictureDialog.FileName(this);
                if (filename.Length != 0)
                {
                    this.image.Image = Image.FromFile(filename);
                }
            }
            this.CheckImageButton();
        }

        private void CheckImageButton()
        {
            if (this.image.Image != null)
            {
                this.button1.Tag = 1;
                this.button1.Text = Texts.ClearImage;
                this.pictureBox1.Image = this.image.Image;
            }
            else
            {
                this.button1.Tag = 0;
                this.button1.Text = Texts.BrowseImage;
                this.pictureBox1.Image = null;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.image.ImageMode = ImageMode.Normal;
                    return;

                case 1:
                    this.image.ImageMode = ImageMode.Stretch;
                    return;

                case 2:
                    this.image.ImageMode = ImageMode.Center;
                    return;
            }
            this.image.ImageMode = ImageMode.Tile;
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
            this.pictureBox1 = new PictureBox();
            this.button1 = new Button();
            this.label2 = new Label();
            this.comboBox1 = new ComboBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            base.CBSeries.Name = "CBSeries";
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new Point(0x10, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd0, 0x80);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image:";
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox1.Location = new Point(0x63, 0x18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x60, 0x58);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(10, 0x18);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Browse...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(11, 0x43);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Mode:";
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "Normal", "Stretch", "Center", "Tile" });
            this.comboBox1.Location = new Point(11, 0x58);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x4d, 0x15);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0xb5);
            base.Controls.Add(this.groupBox1);
            base.Name = "ImageEditor";
            base.Controls.SetChildIndex(this.groupBox1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

