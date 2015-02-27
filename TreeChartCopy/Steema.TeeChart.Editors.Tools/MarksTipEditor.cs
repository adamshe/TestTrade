namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MarksTipEditor : ToolSeriesEditor
    {
        private ComboBox cbAction;
        private ComboBox cbStyle;
        private IContainer components;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown ndDelay;
        private MarksTip tool;

        public MarksTipEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public MarksTipEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (MarksTip) t;
            base.SetTool(this.tool);
            base.CBSeries.Items[0] = Texts.All;
            if (this.tool.Series == null)
            {
                base.CBSeries.SelectedIndex = 0;
            }
            else
            {
                base.CBSeries.SelectedIndex = 1 + this.tool.chart.series.IndexOf(this.tool.Series);
            }
            this.cbStyle.SelectedIndex = (int) this.tool.Style;
            this.ndDelay.Value = this.tool.MouseDelay;
            if (this.tool.MouseAction == MarksTipMouseAction.Move)
            {
                this.cbAction.SelectedIndex = 0;
            }
            else
            {
                this.cbAction.SelectedIndex = 1;
            }
            base.setting = false;
        }

        private void cbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                if (this.cbAction.SelectedIndex == 0)
                {
                    this.tool.MouseAction = MarksTipMouseAction.Move;
                }
                else
                {
                    this.tool.MouseAction = MarksTipMouseAction.Click;
                }
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Style = (MarksStyles) this.cbStyle.SelectedIndex;
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
            this.label2 = new Label();
            this.cbStyle = new ComboBox();
            this.label3 = new Label();
            this.ndDelay = new NumericUpDown();
            this.cbAction = new ComboBox();
            this.label4 = new Label();
            this.ndDelay.BeginInit();
            base.SuspendLayout();
            base.CBSeries.ItemHeight = 13;
            base.CBSeries.Name = "CBSeries";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x2d, 0x2a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "S&tyle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.AddRange(new object[] { "Value", "Percent", "Label", "Label and Percent", "Label and Value", "Legend", "Percent and Total", "Label, Percent and Total", "X Value", "X and Y" });
            this.cbStyle.Location = new Point(0x52, 40);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x88, 0x15);
            this.cbStyle.TabIndex = 3;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x2a, 0x6f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Delay:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.ndDelay.BorderStyle = BorderStyle.FixedSingle;
            this.ndDelay.Location = new Point(0x54, 0x6d);
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.ndDelay.Maximum = new decimal(bits);
            this.ndDelay.Name = "ndDelay";
            this.ndDelay.Size = new Size(0x3e, 20);
            this.ndDelay.TabIndex = 5;
            this.ndDelay.TextAlign = HorizontalAlignment.Right;
            this.ndDelay.ValueChanged += new EventHandler(this.ndDelay_ValueChanged);
            this.cbAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAction.Items.AddRange(new object[] { "Move", "Click" });
            this.cbAction.Location = new Point(0x52, 0x48);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new Size(0x88, 0x15);
            this.cbAction.TabIndex = 7;
            this.cbAction.SelectedIndexChanged += new EventHandler(this.cbAction_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x2a, 0x4a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x27, 0x10);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Action:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.cbAction);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.ndDelay);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cbStyle);
            base.Controls.Add(this.label2);
            base.Name = "MarksTipEditor";
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbStyle, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.ndDelay, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.cbAction, 0);
            this.ndDelay.EndInit();
            base.ResumeLayout(false);
        }

        private void ndDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.MouseDelay = (int) this.ndDelay.Value;
            }
        }
    }
}

