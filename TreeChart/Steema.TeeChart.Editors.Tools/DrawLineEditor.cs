namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DrawLineEditor : ToolSeriesEditor
    {
        private ButtonPen bPen;
        private Button button1;
        private ComboBox cbButton;
        private CheckBox cbEnableDraw;
        private CheckBox cbEnableSelect;
        private IContainer components;
        private Label label2;
        private DrawLine tool;

        public DrawLineEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public DrawLineEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            base.setting = true;
            this.tool = (DrawLine) t;
            base.SetTool(this.tool);
            this.cbButton.SelectedIndex = EditorUtils.MouseButtonIndex(this.tool.Button);
            this.cbEnableDraw.Checked = this.tool.EnableDraw;
            this.cbEnableSelect.Checked = this.tool.EnableSelect;
            this.button1.Enabled = this.tool.Lines.Count > 0;
            this.bPen.Pen = this.tool.Pen;
            base.setting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.tool.Lines.Clear();
            this.button1.Enabled = false;
        }

        private void cbButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.Button = EditorUtils.MouseButtonFromIndex(this.cbButton.SelectedIndex);
            }
        }

        private void cbEnableSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.EnableSelect = this.cbEnableSelect.Checked;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.tool.EnableDraw = this.cbEnableDraw.Checked;
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
            this.bPen = new ButtonPen();
            this.label2 = new Label();
            this.cbButton = new ComboBox();
            this.cbEnableDraw = new CheckBox();
            this.cbEnableSelect = new CheckBox();
            this.button1 = new Button();
            base.SuspendLayout();
            base.CBSeries.ItemHeight = 13;
            base.CBSeries.Name = "CBSeries";
            this.bPen.FlatStyle = FlatStyle.Flat;
            this.bPen.Location = new Point(0x30, 40);
            this.bPen.Name = "bPen";
            this.bPen.TabIndex = 2;
            this.bPen.Text = "&Pen...";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(14, 0x4f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Button:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbButton.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbButton.Items.AddRange(new object[] { "Left", "Middle", "Right", "X Button 1", "X Button 2" });
            this.cbButton.Location = new Point(0x38, 0x4d);
            this.cbButton.Name = "cbButton";
            this.cbButton.Size = new Size(0x79, 0x15);
            this.cbButton.TabIndex = 4;
            this.cbButton.SelectedIndexChanged += new EventHandler(this.cbButton_SelectedIndexChanged);
            this.cbEnableDraw.FlatStyle = FlatStyle.Flat;
            this.cbEnableDraw.Location = new Point(0x38, 0x6c);
            this.cbEnableDraw.Name = "cbEnableDraw";
            this.cbEnableDraw.Size = new Size(0x90, 0x18);
            this.cbEnableDraw.TabIndex = 5;
            this.cbEnableDraw.Text = "Enable &Draw";
            this.cbEnableDraw.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.cbEnableSelect.FlatStyle = FlatStyle.Flat;
            this.cbEnableSelect.Location = new Point(0x38, 0x88);
            this.cbEnableSelect.Name = "cbEnableSelect";
            this.cbEnableSelect.Size = new Size(0x90, 0x10);
            this.cbEnableSelect.TabIndex = 6;
            this.cbEnableSelect.Text = "Enable S&elect";
            this.cbEnableSelect.CheckedChanged += new EventHandler(this.cbEnableSelect_CheckedChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x33, 0xa3);
            this.button1.Name = "button1";
            this.button1.TabIndex = 7;
            this.button1.Text = "Remove &All";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf8, 0xc5);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.cbEnableSelect);
            base.Controls.Add(this.cbEnableDraw);
            base.Controls.Add(this.cbButton);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.bPen);
            base.Name = "DrawLineEditor";
            base.Controls.SetChildIndex(this.bPen, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbButton, 0);
            base.Controls.SetChildIndex(this.cbEnableDraw, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.cbEnableSelect, 0);
            base.Controls.SetChildIndex(this.button1, 0);
            base.ResumeLayout(false);
        }
    }
}

