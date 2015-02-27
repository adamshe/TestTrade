namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PointSeries : Form
    {
        private ButtonColor button1;
        private ComboBox comboBox1;
        private Container components;
        private Label label1;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private Points series;

        public PointSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PointSeries(Series s) : this(s, null)
        {
        }

        public PointSeries(Series s, Control parent) : this()
        {
            this.series = (Points) s;
            switch (this.series.Stacked)
            {
                case CustomStack.None:
                    this.comboBox1.SelectedIndex = 0;
                    break;

                case CustomStack.Overlap:
                    this.comboBox1.SelectedIndex = 1;
                    break;

                case CustomStack.Stack:
                    this.comboBox1.SelectedIndex = 2;
                    break;

                case CustomStack.Stack100:
                    this.comboBox1.SelectedIndex = 3;
                    break;
            }
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.series.Color = this.button1.Color;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
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
            this.button1 = new ButtonColor();
            this.label1 = new Label();
            this.comboBox1 = new ComboBox();
            base.SuspendLayout();
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(8, 8);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x18, 50);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Stacked:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "None", "Overlap", "Stack", "Stack 100%" });
            this.comboBox1.Location = new Point(80, 0x30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x79, 0x15);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf8, 0x7d);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.button1);
            base.Name = "PointSeries";
            base.Load += new EventHandler(this.PointSeries_Load);
            base.ResumeLayout(false);
        }

        private void PointSeries_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                if (this.pointerEditor == null)
                {
                    this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.Pointer);
                    this.pointerEditor.CBDrawPoint.Visible = false;
                }
                this.button1.Color = this.series.Color;
            }
        }
    }
}

