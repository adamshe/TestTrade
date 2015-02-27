namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BoxSeriesEditor : Form
    {
        private Button BMedian;
        private Steema.TeeChart.Editors.SeriesPointer boxEditor;
        private Button BWhisker;
        private Container components;
        private TextBox ELength;
        private TextBox EPos;
        private Steema.TeeChart.Editors.SeriesPointer extrOutEditor;
        private Label label1;
        private Label label2;
        private Steema.TeeChart.Editors.SeriesPointer mildOutEditor;
        private CustomBox series;

        public BoxSeriesEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public BoxSeriesEditor(Series s) : this()
        {
            this.series = (CustomBox) s;
            this.EPos.Text = this.series.Position.ToString();
            this.ELength.Text = this.series.WhiskerLength.ToString();
        }

        private void BMedian_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.MedianPen);
        }

        private void BoxSeriesEditor_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                if (this.mildOutEditor == null)
                {
                    this.mildOutEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.MildOut, Texts.MildOut);
                }
                if (this.extrOutEditor == null)
                {
                    this.extrOutEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.ExtrOut, Texts.ExtrOut);
                }
                if (this.boxEditor == null)
                {
                    this.boxEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.Box, Texts.Box);
                }
            }
        }

        private void BWhisker_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.WhiskerPen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ELength_TextChanged(object sender, EventArgs e)
        {
            this.series.WhiskerLength = Convert.ToDouble(this.ELength.Text);
        }

        private void EPos_TextChanged(object sender, EventArgs e)
        {
            this.series.Position = Convert.ToDouble(this.EPos.Text);
        }

        private void InitializeComponent()
        {
            this.BMedian = new Button();
            this.BWhisker = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.EPos = new TextBox();
            this.ELength = new TextBox();
            base.SuspendLayout();
            this.BMedian.FlatStyle = FlatStyle.Flat;
            this.BMedian.Location = new Point(0x20, 0x18);
            this.BMedian.Name = "BMedian";
            this.BMedian.TabIndex = 0;
            this.BMedian.Text = "&Median...";
            this.BMedian.Click += new EventHandler(this.BMedian_Click);
            this.BWhisker.FlatStyle = FlatStyle.Flat;
            this.BWhisker.Location = new Point(0x20, 0x40);
            this.BWhisker.Name = "BWhisker";
            this.BWhisker.TabIndex = 3;
            this.BWhisker.Text = "&Whisker...";
            this.BWhisker.Click += new EventHandler(this.BWhisker_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x7b, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Position:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x81, 0x44);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2a, 0x10);
            this.label2.TabIndex = 4;
            this.label2.Text = "&Length:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.EPos.BorderStyle = BorderStyle.FixedSingle;
            this.EPos.Location = new Point(0xa9, 0x1a);
            this.EPos.Name = "EPos";
            this.EPos.Size = new Size(0x30, 20);
            this.EPos.TabIndex = 2;
            this.EPos.Text = "";
            this.EPos.TextChanged += new EventHandler(this.EPos_TextChanged);
            this.ELength.BorderStyle = BorderStyle.FixedSingle;
            this.ELength.Location = new Point(0xa9, 0x42);
            this.ELength.Name = "ELength";
            this.ELength.Size = new Size(0x30, 20);
            this.ELength.TabIndex = 5;
            this.ELength.Text = "";
            this.ELength.TextChanged += new EventHandler(this.ELength_TextChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf1, 0x74);
            base.Controls.Add(this.ELength);
            base.Controls.Add(this.EPos);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BWhisker);
            base.Controls.Add(this.BMedian);
            base.Name = "BoxSeriesEditor";
            base.Load += new EventHandler(this.BoxSeriesEditor_Load);
            base.ResumeLayout(false);
        }
    }
}

