namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CursorEditor : ToolSeriesEditor
    {
        private ButtonPen button1;
        private CheckBox cbFollow;
        private CheckBox cbSnap;
        private ComboBox cbStyle;
        private IContainer components;
        private CursorTool cursor;
        private Label label2;

        public CursorEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CursorEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            base.setting = true;
            this.cursor = (CursorTool) s;
            base.SetTool(this.cursor);
            switch (this.cursor.Style)
            {
                case CursorToolStyles.Horizontal:
                    this.cbStyle.SelectedIndex = 0;
                    break;

                case CursorToolStyles.Vertical:
                    this.cbStyle.SelectedIndex = 1;
                    break;

                default:
                    this.cbStyle.SelectedIndex = 2;
                    break;
            }
            this.cbSnap.Checked = this.cursor.Snap;
            this.cbFollow.Checked = this.cursor.FollowMouse;
            this.button1.Pen = this.cursor.Pen;
            base.setting = false;
        }

        private void cbFollow_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.FollowMouse = this.cbFollow.Checked;
            }
        }

        private void cbSnap_CheckedChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                this.cursor.Snap = this.cbSnap.Checked;
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!base.setting)
            {
                switch (this.cbStyle.SelectedIndex)
                {
                    case 0:
                        this.cursor.Style = CursorToolStyles.Horizontal;
                        return;

                    case 1:
                        this.cursor.Style = CursorToolStyles.Vertical;
                        return;
                }
                this.cursor.Style = CursorToolStyles.Both;
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
            this.cbSnap = new CheckBox();
            this.cbFollow = new CheckBox();
            this.button1 = new ButtonPen();
            base.SuspendLayout();
            base.CBSeries.Name = "CBSeries";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x31, 0x2a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "S&tyle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Items.AddRange(new object[] { "Horizontal", "Vertical", "Both" });
            this.cbStyle.Location = new Point(0x51, 40);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x88, 0x15);
            this.cbStyle.TabIndex = 3;
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.cbSnap.FlatStyle = FlatStyle.Flat;
            this.cbSnap.Location = new Point(0x51, 0x45);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new Size(0x90, 0x18);
            this.cbSnap.TabIndex = 4;
            this.cbSnap.Text = "S&nap";
            this.cbSnap.CheckedChanged += new EventHandler(this.cbSnap_CheckedChanged);
            this.cbFollow.FlatStyle = FlatStyle.Flat;
            this.cbFollow.Location = new Point(0x51, 0x62);
            this.cbFollow.Name = "cbFollow";
            this.cbFollow.Size = new Size(0x90, 0x18);
            this.cbFollow.TabIndex = 5;
            this.cbFollow.Text = "&Follow Mouse";
            this.cbFollow.CheckedChanged += new EventHandler(this.cbFollow_CheckedChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x51, 0x88);
            this.button1.Name = "button1";
            this.button1.TabIndex = 6;
            this.button1.Text = "&Pen...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe9, 0xac);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.cbFollow);
            base.Controls.Add(this.cbSnap);
            base.Controls.Add(this.cbStyle);
            base.Controls.Add(this.label2);
            base.Name = "CursorEditor";
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbStyle, 0);
            base.Controls.SetChildIndex(this.cbSnap, 0);
            base.Controls.SetChildIndex(this.cbFollow, 0);
            base.Controls.SetChildIndex(this.button1, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.ResumeLayout(false);
        }
    }
}

