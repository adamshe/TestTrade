namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PointFigure : Form
    {
        private Container components;
        private Steema.TeeChart.Editors.SeriesPointer downEditor;
        private Label label1;
        private Label label2;
        private Steema.TeeChart.Styles.PointFigure pointFigure;
        private TextBox tbBoxSize;
        private TextBox tbReversalAmt;
        private Steema.TeeChart.Editors.SeriesPointer upEditor;

        public PointFigure()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PointFigure(Series s) : this(s, null)
        {
        }

        public PointFigure(Series s, Control parent) : this()
        {
            this.pointFigure = (Steema.TeeChart.Styles.PointFigure) s;
            this.tbBoxSize.Text = this.pointFigure.BoxSize.ToString();
            this.tbReversalAmt.Text = this.pointFigure.ReversalAmount.ToString();
            EditorUtils.InsertForm(this, parent);
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.tbBoxSize = new TextBox();
            this.tbReversalAmt = new TextBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x40, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Box size:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x15, 0x40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x5d, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Reversal amount:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.tbBoxSize.BorderStyle = BorderStyle.FixedSingle;
            this.tbBoxSize.Location = new Point(0x76, 0x17);
            this.tbBoxSize.Name = "tbBoxSize";
            this.tbBoxSize.Size = new Size(90, 20);
            this.tbBoxSize.TabIndex = 1;
            this.tbBoxSize.Text = "1";
            this.tbBoxSize.TextAlign = HorizontalAlignment.Right;
            this.tbBoxSize.TextChanged += new EventHandler(this.tbBoxSize_TextChanged);
            this.tbReversalAmt.BorderStyle = BorderStyle.FixedSingle;
            this.tbReversalAmt.Location = new Point(0x76, 0x3e);
            this.tbReversalAmt.Name = "tbReversalAmt";
            this.tbReversalAmt.Size = new Size(90, 20);
            this.tbReversalAmt.TabIndex = 3;
            this.tbReversalAmt.Text = "3";
            this.tbReversalAmt.TextAlign = HorizontalAlignment.Right;
            this.tbReversalAmt.TextChanged += new EventHandler(this.tbReversalAmt_TextChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x100, 0x75);
            base.Controls.Add(this.tbReversalAmt);
            base.Controls.Add(this.tbBoxSize);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "PointFigure";
            this.Text = "PointFigureEditor";
            base.Load += new EventHandler(this.PointFigure_Load);
            base.ResumeLayout(false);
        }

        private void PointFigure_Load(object sender, EventArgs e)
        {
            if ((this.pointFigure != null) && (this.downEditor == null))
            {
                this.downEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.pointFigure.DownSymbol, Texts.Down);
            }
            if ((this.pointFigure != null) && (this.upEditor == null))
            {
                this.upEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.pointFigure.UpSymbol, Texts.Up);
            }
        }

        private void tbBoxSize_TextChanged(object sender, EventArgs e)
        {
            this.pointFigure.BoxSize = Convert.ToDouble(this.tbBoxSize.Text);
        }

        private void tbReversalAmt_TextChanged(object sender, EventArgs e)
        {
            this.pointFigure.ReversalAmount = Convert.ToDouble(this.tbReversalAmt.Text);
        }
    }
}

