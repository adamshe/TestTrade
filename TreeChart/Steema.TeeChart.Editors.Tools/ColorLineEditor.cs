namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ColorLineEditor : AxisToolEdit
    {
        private CheckBox cb3D;
        private CheckBox cbBehind;
        private CheckBox cbDrag;
        private CheckBox cbDragRepaint;
        private CheckBox cbNoLimit;
        private IContainer components;
        private TextBox eValue;
        private Label label2;
        private bool setting;
        private ColorLine tool;

        public ColorLineEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ColorLineEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.setting = true;
            this.tool = (ColorLine) t;
            base.SetTool(this.tool);
            this.eValue.Text = this.tool.Value.ToString();
            this.cb3D.Checked = this.tool.Draw3D;
            this.cbBehind.Checked = this.tool.DrawBehind;
            this.cbDrag.Checked = this.tool.AllowDrag;
            this.cbDragRepaint.Checked = this.tool.DragRepaint;
            this.cbNoLimit.Checked = this.tool.NoLimitDrag;
            this.setting = false;
        }

        private void cb3D_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Draw3D = this.cb3D.Checked;
            }
        }

        private void cbBehind_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.DrawBehind = this.cbBehind.Checked;
            }
        }

        private void cbDrag_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.AllowDrag = this.cbDrag.Checked;
            }
        }

        private void cbDragRepaint_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.DragRepaint = this.cbDragRepaint.Checked;
            }
        }

        private void cbNoLimit_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.NoLimitDrag = this.cbNoLimit.Checked;
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

        private void eValue_TextChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.tool.Value = Convert.ToDouble(this.eValue.Text);
            }
        }

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.eValue = new TextBox();
            this.cbDrag = new CheckBox();
            this.cbDragRepaint = new CheckBox();
            this.cbNoLimit = new CheckBox();
            this.cbBehind = new CheckBox();
            this.cb3D = new CheckBox();
            base.SuspendLayout();
            base.BPen.Name = "BPen";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x20, 0x4a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Value:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.eValue.BorderStyle = BorderStyle.FixedSingle;
            this.eValue.Location = new Point(0x48, 0x48);
            this.eValue.Name = "eValue";
            this.eValue.TabIndex = 4;
            this.eValue.Text = "";
            this.eValue.TextAlign = HorizontalAlignment.Right;
            this.eValue.TextChanged += new EventHandler(this.eValue_TextChanged);
            this.cbDrag.FlatStyle = FlatStyle.Flat;
            this.cbDrag.Location = new Point(0x48, 0x65);
            this.cbDrag.Name = "cbDrag";
            this.cbDrag.Size = new Size(0x98, 0x18);
            this.cbDrag.TabIndex = 5;
            this.cbDrag.Text = "Allow &Drag";
            this.cbDrag.CheckedChanged += new EventHandler(this.cbDrag_CheckedChanged);
            this.cbDragRepaint.FlatStyle = FlatStyle.Flat;
            this.cbDragRepaint.Location = new Point(0x48, 0x7c);
            this.cbDragRepaint.Name = "cbDragRepaint";
            this.cbDragRepaint.Size = new Size(0x98, 0x18);
            this.cbDragRepaint.TabIndex = 6;
            this.cbDragRepaint.Text = "Drag &Repaint";
            this.cbDragRepaint.CheckedChanged += new EventHandler(this.cbDragRepaint_CheckedChanged);
            this.cbNoLimit.FlatStyle = FlatStyle.Flat;
            this.cbNoLimit.Location = new Point(0x48, 0x92);
            this.cbNoLimit.Name = "cbNoLimit";
            this.cbNoLimit.Size = new Size(0x98, 0x18);
            this.cbNoLimit.TabIndex = 7;
            this.cbNoLimit.Text = "No &Limit Drag";
            this.cbNoLimit.CheckedChanged += new EventHandler(this.cbNoLimit_CheckedChanged);
            this.cbBehind.FlatStyle = FlatStyle.Flat;
            this.cbBehind.Location = new Point(0x48, 0xa8);
            this.cbBehind.Name = "cbBehind";
            this.cbBehind.Size = new Size(0x98, 0x18);
            this.cbBehind.TabIndex = 8;
            this.cbBehind.Text = "Draw B&ehind";
            this.cbBehind.CheckedChanged += new EventHandler(this.cbBehind_CheckedChanged);
            this.cb3D.FlatStyle = FlatStyle.Flat;
            this.cb3D.Location = new Point(0x48, 0xc0);
            this.cb3D.Name = "cb3D";
            this.cb3D.Size = new Size(0x98, 0x18);
            this.cb3D.TabIndex = 9;
            this.cb3D.Text = "Draw &3D";
            this.cb3D.CheckedChanged += new EventHandler(this.cb3D_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe8, 0xe5);
            base.Controls.Add(this.cb3D);
            base.Controls.Add(this.cbBehind);
            base.Controls.Add(this.cbNoLimit);
            base.Controls.Add(this.cbDragRepaint);
            base.Controls.Add(this.cbDrag);
            base.Controls.Add(this.eValue);
            base.Controls.Add(this.label2);
            base.Name = "ColorLineEditor";
            base.Controls.SetChildIndex(base.BPen, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.eValue, 0);
            base.Controls.SetChildIndex(this.cbDrag, 0);
            base.Controls.SetChildIndex(this.cbDragRepaint, 0);
            base.Controls.SetChildIndex(this.cbNoLimit, 0);
            base.Controls.SetChildIndex(this.cbBehind, 0);
            base.Controls.SetChildIndex(this.cb3D, 0);
            base.ResumeLayout(false);
        }
    }
}

