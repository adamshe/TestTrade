namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CustomShapeEditor : Form
    {
        private ButtonColor BBackColor;
        private BevelEditor bevelEditor;
        private ButtonPen Button4;
        private Button Button6;
        private CheckBox CBRound;
        private CheckBox CBTransparent;
        private Container components;
        private GradientEditor gradientEditor;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private TabControl PC1;
        private ShadowEditor shadowEditor;
        private TextShape shape;
        private TabPage tabBevels;
        internal TabPage tabFormat;
        internal TabPage tabGradient;
        internal TabPage tabShadow;
        internal TabPage tabText;
        private TextEditor textEditor;

        public CustomShapeEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CustomShapeEditor(TextShape s) : this()
        {
            this.shape = s;
            this.numericUpDown1.Value = this.shape.Transparency;
            this.CBTransparent.Checked = this.shape.Transparent;
            this.BBackColor.Color = this.shape.Color;
            this.Button4.Pen = this.shape.Pen;
            this.Translate();
        }

        public static CustomShapeEditor Add(TabControl tabControl, TextShape s)
        {
            CustomShapeEditor editor = new CustomShapeEditor(s);
            editor.Dock = DockStyle.Fill;
            tabControl.TabPages.Add(editor.tabFormat);
            tabControl.TabPages.Add(editor.tabText);
            tabControl.TabPages.Add(editor.tabGradient);
            tabControl.TabPages.Add(editor.tabShadow);
            tabControl.TabPages.Add(editor.tabBevels);
            editor.textEditor = new TextEditor(s.Font, editor.tabText);
            EditorUtils.Translate(editor.textEditor);
            editor.gradientEditor = new GradientEditor(s.Gradient, editor.tabGradient);
            EditorUtils.Translate(editor.gradientEditor);
            editor.shadowEditor = new ShadowEditor(s.Shadow, editor.tabShadow);
            EditorUtils.Translate(editor.shadowEditor);
            editor.bevelEditor = new BevelEditor(s.Bevel, editor.tabBevels);
            EditorUtils.Translate(editor.bevelEditor);
            return editor;
        }

        private void BBackColor_Click(object sender, EventArgs e)
        {
            this.shape.Color = this.BBackColor.Color;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.shape.Brush);
        }

        private void CBRound_CheckedChanged(object sender, EventArgs e)
        {
            this.shape.ShapeStyle = this.CBRound.Checked ? TextShapeStyle.RoundRectangle : TextShapeStyle.Rectangle;
        }

        private void CBTransparent_CheckedChanged(object sender, EventArgs e)
        {
            this.shape.Transparent = this.CBTransparent.Checked;
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
            this.PC1 = new TabControl();
            this.tabFormat = new TabPage();
            this.label4 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.CBTransparent = new CheckBox();
            this.CBRound = new CheckBox();
            this.Button4 = new ButtonPen();
            this.Button6 = new Button();
            this.BBackColor = new ButtonColor();
            this.tabShadow = new TabPage();
            this.tabGradient = new TabPage();
            this.tabText = new TabPage();
            this.tabBevels = new TabPage();
            this.PC1.SuspendLayout();
            this.tabFormat.SuspendLayout();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.PC1.Controls.Add(this.tabFormat);
            this.PC1.Controls.Add(this.tabShadow);
            this.PC1.Controls.Add(this.tabGradient);
            this.PC1.Controls.Add(this.tabText);
            this.PC1.Controls.Add(this.tabBevels);
            this.PC1.Dock = DockStyle.Fill;
            this.PC1.HotTrack = true;
            this.PC1.Location = new Point(0, 0);
            this.PC1.Name = "PC1";
            this.PC1.RightToLeft = RightToLeft.No;
            this.PC1.SelectedIndex = 0;
            this.PC1.Size = new Size(0x130, 0xc5);
            this.PC1.TabIndex = 0;
            this.PC1.SelectedIndexChanged += new EventHandler(this.PC1_SelectedIndexChanged);
            this.tabFormat.Controls.Add(this.label4);
            this.tabFormat.Controls.Add(this.numericUpDown1);
            this.tabFormat.Controls.Add(this.CBTransparent);
            this.tabFormat.Controls.Add(this.CBRound);
            this.tabFormat.Controls.Add(this.Button4);
            this.tabFormat.Controls.Add(this.Button6);
            this.tabFormat.Controls.Add(this.BBackColor);
            this.tabFormat.Location = new Point(4, 0x16);
            this.tabFormat.Name = "tabFormat";
            this.tabFormat.RightToLeft = RightToLeft.No;
            this.tabFormat.Size = new Size(0x128, 0xab);
            this.tabFormat.TabIndex = 0;
            this.tabFormat.Text = "Format";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x70, 0x4f);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4d, 0x10);
            this.label4.TabIndex = 5;
            this.label4.Text = "Transparency:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0xbf, 0x4d);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x30, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.CBTransparent.FlatStyle = FlatStyle.Flat;
            this.CBTransparent.Location = new Point(0x80, 40);
            this.CBTransparent.Name = "CBTransparent";
            this.CBTransparent.Size = new Size(0x80, 0x10);
            this.CBTransparent.TabIndex = 4;
            this.CBTransparent.Text = "&Transparent";
            this.CBTransparent.CheckedChanged += new EventHandler(this.CBTransparent_CheckedChanged);
            this.CBRound.FlatStyle = FlatStyle.Flat;
            this.CBRound.Location = new Point(0x80, 0x10);
            this.CBRound.Name = "CBRound";
            this.CBRound.Size = new Size(0x80, 0x10);
            this.CBRound.TabIndex = 3;
            this.CBRound.Text = "&Round Frame";
            this.CBRound.CheckedChanged += new EventHandler(this.CBRound_CheckedChanged);
            this.Button4.FlatStyle = FlatStyle.Flat;
            this.Button4.Location = new Point(12, 0x2d);
            this.Button4.Name = "Button4";
            this.Button4.Size = new Size(0x55, 0x17);
            this.Button4.TabIndex = 1;
            this.Button4.Text = "&Frame...";
            this.Button6.FlatStyle = FlatStyle.Flat;
            this.Button6.Location = new Point(12, 0x4d);
            this.Button6.Name = "Button6";
            this.Button6.Size = new Size(0x55, 0x17);
            this.Button6.TabIndex = 2;
            this.Button6.Text = "&Pattern...";
            this.Button6.Click += new EventHandler(this.Button6_Click);
            this.BBackColor.Color = Color.Empty;
            this.BBackColor.Location = new Point(12, 13);
            this.BBackColor.Name = "BBackColor";
            this.BBackColor.Size = new Size(0x55, 0x17);
            this.BBackColor.TabIndex = 0;
            this.BBackColor.Text = "&Color...";
            this.BBackColor.Click += new EventHandler(this.BBackColor_Click);
            this.tabShadow.Location = new Point(4, 0x16);
            this.tabShadow.Name = "tabShadow";
            this.tabShadow.Size = new Size(0x128, 0xab);
            this.tabShadow.TabIndex = 3;
            this.tabShadow.Text = "Shadow";
            this.tabGradient.Location = new Point(4, 0x16);
            this.tabGradient.Name = "tabGradient";
            this.tabGradient.Size = new Size(0x128, 0xab);
            this.tabGradient.TabIndex = 2;
            this.tabGradient.Text = "Gradient";
            this.tabText.Location = new Point(4, 0x16);
            this.tabText.Name = "tabText";
            this.tabText.Size = new Size(0x128, 0xab);
            this.tabText.TabIndex = 1;
            this.tabText.Text = "Text";
            this.tabBevels.Location = new Point(4, 0x16);
            this.tabBevels.Name = "tabBevels";
            this.tabBevels.Size = new Size(0x128, 0xab);
            this.tabBevels.TabIndex = 4;
            this.tabBevels.Text = "Bevels";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x130, 0xc5);
            base.Controls.Add(this.PC1);
            base.Name = "CustomShapeEditor";
            this.PC1.ResumeLayout(false);
            this.tabFormat.ResumeLayout(false);
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (this.shape != null)
            {
                this.shape.Transparency = (int) this.numericUpDown1.Value;
            }
        }

        private void PC1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.PC1.SelectedTab == this.tabText) && (this.textEditor == null))
            {
                this.textEditor = new TextEditor(this.shape.Font, this.tabText);
                EditorUtils.Translate(this.textEditor);
            }
            else if ((this.PC1.SelectedTab == this.tabGradient) && (this.gradientEditor == null))
            {
                this.gradientEditor = new GradientEditor(this.shape.Gradient, this.tabGradient);
                EditorUtils.Translate(this.gradientEditor);
            }
            else if ((this.PC1.SelectedTab == this.tabShadow) && (this.shadowEditor == null))
            {
                this.shadowEditor = new ShadowEditor(this.shape.Shadow, this.tabShadow);
                EditorUtils.Translate(this.shadowEditor);
            }
            else if ((this.PC1.SelectedTab == this.tabBevels) && (this.bevelEditor == null))
            {
                this.bevelEditor = new BevelEditor(this.shape.Bevel, this.tabBevels);
                EditorUtils.Translate(this.bevelEditor);
            }
        }

        public static void Remove(TabControl tabControl, CustomShapeEditor s)
        {
            tabControl.TabPages.Remove(s.tabFormat);
            tabControl.TabPages.Remove(s.tabText);
            tabControl.TabPages.Remove(s.tabGradient);
            tabControl.TabPages.Remove(s.tabShadow);
            tabControl.TabPages.Remove(s.tabBevels);
        }

        internal void Translate()
        {
            EditorUtils.Translate(this.tabFormat);
            EditorUtils.Translate(this.tabText);
            EditorUtils.Translate(this.tabGradient);
            EditorUtils.Translate(this.tabShadow);
            EditorUtils.Translate(this.tabBevels);
        }
    }
}

