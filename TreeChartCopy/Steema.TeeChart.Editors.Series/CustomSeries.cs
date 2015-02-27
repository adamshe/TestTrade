namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CustomSeries : Form
    {
        private Button BBrush;
        private ButtonPen BLineBorder;
        private ButtonColor BLineColor;
        private ButtonPen BOutline;
        private CheckBox CBClick;
        private CheckBox CBColorEach;
        private CheckBox CBColorEachLine;
        private CheckBox CBDark3D;
        private CheckBox CBInvStairs;
        private ComboBox CBStack;
        private CheckBox CBStairs;
        private Container components;
        private NumericUpDown EHeight;
        private GroupBox GBStair;
        private Label label1;
        private Label label2;
        private Label LHeight;
        private NumericUpDown numericUpDown1;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private Custom series;

        public CustomSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CustomSeries(Series s) : this(s, null)
        {
        }

        public CustomSeries(Series s, Control parent) : this()
        {
            this.series = (Custom) s;
            this.CBDark3D.Checked = this.series.Dark3D;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.CBClick.Checked = this.series.ClickableLine;
            this.CBStairs.Checked = this.series.Stairs;
            this.CBInvStairs.Checked = this.series.InvertedStairs;
            this.CBInvStairs.Enabled = this.series.Stairs;
            this.EHeight.Value = this.series.LineHeight;
            this.CBColorEachLine.Checked = this.series.ColorEachLine;
            this.CBStack.SelectedIndex = (int) this.series.Stacked;
            this.numericUpDown1.Value = this.series.Transparency;
            this.BLineColor.Color = this.series.Color;
            this.BOutline.Pen = this.series.OutLine;
            this.BLineBorder.Pen = this.series.LinePen;
            EditorUtils.InsertForm(this, parent);
        }

        private void BBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void BLineColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BLineColor.Color;
        }

        private void CBClick_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ClickableLine = this.CBClick.Checked;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBDark3D_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Dark3D = this.CBDark3D.Checked;
        }

        private void CBInvStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.series.InvertedStairs = this.CBInvStairs.Checked;
        }

        private void CBStack_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBStack.SelectedIndex)
            {
                case 0:
                    this.series.Stacked = CustomStack.None;
                    return;

                case 1:
                    this.series.Stacked = CustomStack.Overlap;
                    return;

                case 2:
                    this.series.Stacked = CustomStack.Stack;
                    return;

                case 3:
                    this.series.Stacked = CustomStack.Stack100;
                    return;
            }
        }

        private void CBStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Stairs = this.CBStairs.Checked;
            this.CBInvStairs.Enabled = this.series.Stairs;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEachLine = this.CBColorEachLine.Checked;
        }

        private void CustomSeries_Load(object sender, EventArgs e)
        {
            if ((this.series != null) && (this.pointerEditor == null))
            {
                this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.Pointer);
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

        private void EHeight_ValueChanged(object sender, EventArgs e)
        {
            this.series.LineHeight = (int) this.EHeight.Value;
        }

        private void InitializeComponent()
        {
            this.BLineBorder = new ButtonPen();
            this.BLineColor = new ButtonColor();
            this.BBrush = new Button();
            this.BOutline = new ButtonPen();
            this.CBDark3D = new CheckBox();
            this.CBColorEach = new CheckBox();
            this.CBClick = new CheckBox();
            this.LHeight = new Label();
            this.label2 = new Label();
            this.EHeight = new NumericUpDown();
            this.CBStack = new ComboBox();
            this.GBStair = new GroupBox();
            this.CBInvStairs = new CheckBox();
            this.CBStairs = new CheckBox();
            this.CBColorEachLine = new CheckBox();
            this.numericUpDown1 = new NumericUpDown();
            this.label1 = new Label();
            this.EHeight.BeginInit();
            this.GBStair.SuspendLayout();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.BLineBorder.FlatStyle = FlatStyle.Flat;
            this.BLineBorder.Location = new Point(7, 6);
            this.BLineBorder.Name = "BLineBorder";
            this.BLineBorder.TabIndex = 0;
            this.BLineBorder.Text = "&Border...";
            this.BLineColor.Color = Color.Empty;
            this.BLineColor.Location = new Point(7, 0x26);
            this.BLineColor.Name = "BLineColor";
            this.BLineColor.TabIndex = 1;
            this.BLineColor.Text = "&Color...";
            this.BLineColor.Click += new EventHandler(this.BLineColor_Click);
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(7, 70);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 2;
            this.BBrush.Text = "&Pattern...";
            this.BBrush.Click += new EventHandler(this.BBrush_Click);
            this.BOutline.FlatStyle = FlatStyle.Flat;
            this.BOutline.Location = new Point(0xd0, 0x58);
            this.BOutline.Name = "BOutline";
            this.BOutline.TabIndex = 8;
            this.BOutline.Text = "&Outline...";
            this.CBDark3D.FlatStyle = FlatStyle.Flat;
            this.CBDark3D.Location = new Point(0x59, 10);
            this.CBDark3D.Name = "CBDark3D";
            this.CBDark3D.Size = new Size(0x6f, 0x10);
            this.CBDark3D.TabIndex = 3;
            this.CBDark3D.Text = "&Dark 3D";
            this.CBDark3D.CheckedChanged += new EventHandler(this.CBDark3D_CheckedChanged);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x59, 0x22);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x6f, 0x10);
            this.CBColorEach.TabIndex = 4;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.CBClick.FlatStyle = FlatStyle.Flat;
            this.CBClick.Location = new Point(0x59, 0x39);
            this.CBClick.Name = "CBClick";
            this.CBClick.Size = new Size(0x6f, 0x10);
            this.CBClick.TabIndex = 5;
            this.CBClick.Text = "Clic&kable";
            this.CBClick.CheckedChanged += new EventHandler(this.CBClick_CheckedChanged);
            this.LHeight.AutoSize = true;
            this.LHeight.Location = new Point(0x17, 0x6b);
            this.LHeight.Name = "LHeight";
            this.LHeight.Size = new Size(0x3a, 0x10);
            this.LHeight.TabIndex = 9;
            this.LHeight.Text = "&Height 3D:";
            this.LHeight.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x29, 0x87);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 11;
            this.label2.Text = "Stac&k:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.EHeight.BorderStyle = BorderStyle.FixedSingle;
            this.EHeight.Location = new Point(0x4f, 0x69);
            this.EHeight.Name = "EHeight";
            this.EHeight.Size = new Size(0x31, 20);
            this.EHeight.TabIndex = 10;
            this.EHeight.TextAlign = HorizontalAlignment.Right;
            this.EHeight.TextChanged += new EventHandler(this.EHeight_ValueChanged);
            this.EHeight.ValueChanged += new EventHandler(this.EHeight_ValueChanged);
            this.CBStack.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBStack.Items.AddRange(new object[] { "None", "Overlap", "Stack", "Stack 100%" });
            this.CBStack.Location = new Point(0x4f, 0x85);
            this.CBStack.Name = "CBStack";
            this.CBStack.Size = new Size(0x79, 0x15);
            this.CBStack.TabIndex = 12;
            this.CBStack.SelectedIndexChanged += new EventHandler(this.CBStack_SelectedIndexChanged);
            this.GBStair.Controls.Add(this.CBInvStairs);
            this.GBStair.Controls.Add(this.CBStairs);
            this.GBStair.Location = new Point(0xcc, 8);
            this.GBStair.Name = "GBStair";
            this.GBStair.Size = new Size(0x5c, 0x48);
            this.GBStair.TabIndex = 7;
            this.GBStair.TabStop = false;
            this.GBStair.Text = "Line Mode:";
            this.CBInvStairs.FlatStyle = FlatStyle.Flat;
            this.CBInvStairs.Location = new Point(6, 0x2b);
            this.CBInvStairs.Name = "CBInvStairs";
            this.CBInvStairs.Size = new Size(0x52, 0x10);
            this.CBInvStairs.TabIndex = 1;
            this.CBInvStairs.Text = "&Inverted";
            this.CBInvStairs.CheckedChanged += new EventHandler(this.CBInvStairs_CheckedChanged);
            this.CBStairs.FlatStyle = FlatStyle.Flat;
            this.CBStairs.Location = new Point(6, 0x13);
            this.CBStairs.Name = "CBStairs";
            this.CBStairs.Size = new Size(0x52, 0x10);
            this.CBStairs.TabIndex = 0;
            this.CBStairs.Text = "&Stairs";
            this.CBStairs.CheckedChanged += new EventHandler(this.CBStairs_CheckedChanged);
            this.CBColorEachLine.FlatStyle = FlatStyle.Flat;
            this.CBColorEachLine.Location = new Point(0x59, 0x4d);
            this.CBColorEachLine.Name = "CBColorEachLine";
            this.CBColorEachLine.Size = new Size(0x6f, 0x18);
            this.CBColorEachLine.TabIndex = 6;
            this.CBColorEachLine.Text = "Color Each li&ne";
            this.CBColorEachLine.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(80, 160);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x30, 20);
            this.numericUpDown1.TabIndex = 14;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(4, 0xa2);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 0x10);
            this.label1.TabIndex = 13;
            this.label1.Text = "&Transparency:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x130, 0xb8);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBColorEachLine);
            base.Controls.Add(this.GBStair);
            base.Controls.Add(this.CBStack);
            base.Controls.Add(this.EHeight);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.LHeight);
            base.Controls.Add(this.CBClick);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.CBDark3D);
            base.Controls.Add(this.BOutline);
            base.Controls.Add(this.BBrush);
            base.Controls.Add(this.BLineColor);
            base.Controls.Add(this.BLineBorder);
            base.Name = "CustomSeries";
            base.Load += new EventHandler(this.CustomSeries_Load);
            this.EHeight.EndInit();
            this.GBStair.ResumeLayout(false);
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.series.Transparency = (int) this.numericUpDown1.Value;
        }
    }
}

