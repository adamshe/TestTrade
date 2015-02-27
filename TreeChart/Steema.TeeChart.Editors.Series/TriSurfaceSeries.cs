namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TriSurfaceSeries : Form
    {
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private Container components;
        private Grid3DSeries grid3DEditor;
        private TriSurface series;

        public TriSurfaceSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public TriSurfaceSeries(Series s) : this()
        {
            this.series = (TriSurface) s;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.Outline);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.Pen);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
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
            this.Button2 = new Button();
            this.Button3 = new Button();
            this.Button1 = new Button();
            base.SuspendLayout();
            this.Button2.FlatStyle = FlatStyle.Flat;
            this.Button2.Location = new Point(20, 5);
            this.Button2.Name = "Button2";
            this.Button2.TabIndex = 0;
            this.Button2.Text = "&Pen...";
            this.Button2.Click += new EventHandler(this.Button2_Click);
            this.Button3.FlatStyle = FlatStyle.Flat;
            this.Button3.Location = new Point(20, 0x25);
            this.Button3.Name = "Button3";
            this.Button3.TabIndex = 1;
            this.Button3.Text = "&Brush...";
            this.Button3.Click += new EventHandler(this.Button3_Click);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0x84, 0x25);
            this.Button1.Name = "Button1";
            this.Button1.TabIndex = 2;
            this.Button1.Text = "&Outline...";
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xee, 0x45);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.Button3);
            base.Controls.Add(this.Button2);
            base.Name = "TriSurfaceSeries";
            base.Load += new EventHandler(this.TriSurfaceSeries_Load);
            base.ResumeLayout(false);
        }

        private void TriSurfaceSeries_Load(object sender, EventArgs e)
        {
            if ((this.series != null) && (this.grid3DEditor == null))
            {
                this.grid3DEditor = new Grid3DSeries(this.series, this);
            }
        }
    }
}

