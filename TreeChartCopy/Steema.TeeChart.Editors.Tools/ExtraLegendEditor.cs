namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ExtraLegendEditor : ToolSeriesEditor
    {
        private Button button1;
        private IContainer components;
        private ExtraLegend tool;

        public ExtraLegendEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ExtraLegendEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = (ExtraLegend) t;
            base.SetTool(this.tool);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LegendEditor c = new LegendEditor(this.tool.chart, this.tool.Legend, null);
            EditorUtils.Translate(c);
            EditorUtils.ShowFormModal(c);
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
            base.CBSeries.Name = "CBSeries";
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x31, 0x38);
            this.button1.Name = "button1";
            this.button1.Size = new Size(140, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "Edit Legend...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.button1);
            base.Name = "ExtraLegendEditor";
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.ResumeLayout(false);
        }
    }
}

