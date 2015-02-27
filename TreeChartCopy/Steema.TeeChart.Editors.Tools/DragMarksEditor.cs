namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DragMarksEditor : ToolSeriesEditor
    {
        private Button button1;
        private IContainer components;
        private DragMarks tool;

        public DragMarksEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public DragMarksEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = (DragMarks) t;
            base.SetTool(this.tool);
            base.CBSeries.Items[0] = Texts.All;
            if (base.CBSeries.SelectedIndex == -1)
            {
                base.CBSeries.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.tool.Series != null)
            {
                this.tool.Series.Marks.ResetPositions();
            }
            else
            {
                foreach (Series series in this.tool.Chart.Series)
                {
                    series.Marks.ResetPositions();
                }
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
            this.button1 = new Button();
            base.SuspendLayout();
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x30, 40);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x90, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "&Reset positions";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.AddRange(new Control[] { this.button1 });
            base.Name = "DragMarksEditor";
            base.ResumeLayout(false);
        }
    }
}

