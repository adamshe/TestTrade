namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextEditor : Form
    {
        private Button button1;
        private Button button2;
        private Container components;
        private ChartFont font;
        private GroupBox groupBox1;
        private Label labelColor;
        private Panel panel1;
        private ShadowEditor shadowEditor;

        public TextEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public TextEditor(ChartFont f, Control parent) : this()
        {
            this.UpdateDialog(f);
            this.shadowEditor = new ShadowEditor(this.font.Shadow, this.groupBox1);
            EditorUtils.InsertForm(this, parent);
            base.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (EditorUtils.EditFont(this.font))
            {
                this.SetLabelColor();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (BrushEditor.Edit(this.font.Brush, false))
            {
                this.SetLabelColor();
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
            this.labelColor = new Label();
            this.button1 = new Button();
            this.groupBox1 = new GroupBox();
            this.button2 = new Button();
            this.panel1 = new Panel();
            base.SuspendLayout();
            this.labelColor.BorderStyle = BorderStyle.Fixed3D;
            this.labelColor.Cursor = Cursors.Hand;
            this.labelColor.FlatStyle = FlatStyle.Flat;
            this.labelColor.Location = new Point(0x65, 10);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new Size(0x12, 0x12);
            this.labelColor.TabIndex = 1;
            this.labelColor.UseMnemonic = false;
            this.labelColor.Click += new EventHandler(this.label1_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "F&ont...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.groupBox1.Dock = DockStyle.Fill;
            this.groupBox1.Location = new Point(0, 0x24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(320, 0x8a);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shadow:";
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x90, 8);
            this.button2.Name = "button2";
            this.button2.Size = new Size(80, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "&Fill...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(320, 0x24);
            this.panel1.TabIndex = 4;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(320, 0xae);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.labelColor);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.panel1);
            base.Name = "TextEditor";
            this.Text = "Text Editor";
            base.ResumeLayout(false);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.font.Color = ColorEditor.Choose(this.font.Color, this);
            this.SetLabelColor();
        }

        internal void RefreshControls(ChartFont f)
        {
            this.UpdateDialog(f);
            if (this.shadowEditor != null)
            {
                this.shadowEditor.RefreshControls(this.font.Shadow);
            }
        }

        private void SetLabelColor()
        {
            this.labelColor.BackColor = this.font.Color;
        }

        private void UpdateDialog(ChartFont f)
        {
            this.font = f;
            this.SetLabelColor();
        }
    }
}

