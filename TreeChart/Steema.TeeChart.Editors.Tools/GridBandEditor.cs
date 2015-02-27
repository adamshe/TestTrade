namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GridBandEditor : AxisToolEdit
    {
        private Button BBand1;
        private Button BBand2;
        private ButtonColor buttonColor1;
        private ButtonColor buttonColor2;
        private IContainer components;
        private GridBand tool;

        public GridBandEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GridBandEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = (GridBand) t;
            base.SetTool(this.tool);
            base.BPen.Visible = false;
            this.buttonColor1.Color = this.tool.Band1.Color;
            this.buttonColor2.Color = this.tool.Band2.Color;
            EditorUtils.Translate(this);
        }

        private void BBand1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Band1, true);
            this.buttonColor1.Color = this.tool.Band1.Color;
        }

        private void BBand2_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.tool.Band2, true);
            this.buttonColor2.Color = this.tool.Band2.Color;
        }

        private void buttonColor1_Click(object sender, EventArgs e)
        {
            this.tool.Band1.Color = this.buttonColor1.Color;
        }

        private void buttonColor2_Click(object sender, EventArgs e)
        {
            this.tool.Band2.Color = this.buttonColor2.Color;
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
            this.BBand1 = new Button();
            this.BBand2 = new Button();
            this.buttonColor1 = new ButtonColor();
            this.buttonColor2 = new ButtonColor();
            base.SuspendLayout();
            this.BBand1.FlatStyle = FlatStyle.Flat;
            this.BBand1.Location = new Point(0x2d, 0x49);
            this.BBand1.Name = "BBand1";
            this.BBand1.TabIndex = 3;
            this.BBand1.Text = "Band &1...";
            this.BBand1.Click += new EventHandler(this.BBand1_Click);
            this.BBand2.FlatStyle = FlatStyle.Flat;
            this.BBand2.Location = new Point(0x2d, 110);
            this.BBand2.Name = "BBand2";
            this.BBand2.TabIndex = 4;
            this.BBand2.Text = "Band &2...";
            this.BBand2.Click += new EventHandler(this.BBand2_Click);
            this.buttonColor1.Color = Color.Empty;
            this.buttonColor1.Location = new Point(0x88, 0x49);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.TabIndex = 5;
            this.buttonColor1.Text = "Color...";
            this.buttonColor1.Click += new EventHandler(this.buttonColor1_Click);
            this.buttonColor2.Color = Color.Empty;
            this.buttonColor2.Location = new Point(0x87, 110);
            this.buttonColor2.Name = "buttonColor2";
            this.buttonColor2.TabIndex = 6;
            this.buttonColor2.Text = "Color...";
            this.buttonColor2.Click += new EventHandler(this.buttonColor2_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe8, 0x8f);
            base.Controls.Add(this.buttonColor2);
            base.Controls.Add(this.buttonColor1);
            base.Controls.Add(this.BBand2);
            base.Controls.Add(this.BBand1);
            base.Name = "GridBandEditor";
            base.Controls.SetChildIndex(this.BBand1, 0);
            base.Controls.SetChildIndex(this.BBand2, 0);
            base.Controls.SetChildIndex(this.buttonColor1, 0);
            base.Controls.SetChildIndex(this.buttonColor2, 0);
            base.ResumeLayout(false);
        }
    }
}

