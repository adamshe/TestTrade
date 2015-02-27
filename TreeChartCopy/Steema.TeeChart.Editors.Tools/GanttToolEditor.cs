namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GanttToolEditor : ToolSeriesEditor
    {
        private CheckBox cbAllowDrag;
        private CheckBox cbAllowResize;
        private ComboBox cbCursorDrag;
        private ComboBox cbCursorResize;
        private IContainer components;
        private GanttTool gantt;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown ndMinPixels;

        public GanttToolEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GanttToolEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.gantt = (GanttTool) s;
            base.SetTool(this.gantt);
            this.cbAllowDrag.Checked = this.gantt.AllowDrag;
            this.cbAllowResize.Checked = this.gantt.AllowResize;
            this.ndMinPixels.Value = this.gantt.MinPixels;
            EditorUtils.FillCursors(this.cbCursorDrag, this.gantt.CursorDrag);
            EditorUtils.FillCursors(this.cbCursorResize, this.gantt.CursorResize);
        }

        private void cbAllowDrag_CheckedChanged(object sender, EventArgs e)
        {
            this.gantt.AllowDrag = this.cbAllowDrag.Checked;
        }

        private void cbCursorDrag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gantt.CursorDrag = EditorUtils.StringToCursor(this.cbCursorDrag.SelectedItem.ToString());
        }

        private void cbCursorResize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gantt.CursorResize = EditorUtils.StringToCursor(this.cbCursorResize.SelectedItem.ToString());
        }

        private void cbResize_CheckedChanged(object sender, EventArgs e)
        {
            this.gantt.AllowResize = this.cbAllowResize.Checked;
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
            this.ndMinPixels = new NumericUpDown();
            this.cbAllowDrag = new CheckBox();
            this.label3 = new Label();
            this.cbCursorDrag = new ComboBox();
            this.cbCursorResize = new ComboBox();
            this.label4 = new Label();
            this.cbAllowResize = new CheckBox();
            this.ndMinPixels.BeginInit();
            base.SuspendLayout();
            base.CBSeries.Name = "CBSeries";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x25, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x7c, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "Resize pixels &tolerance:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.ndMinPixels.BorderStyle = BorderStyle.FixedSingle;
            this.ndMinPixels.Location = new Point(0xa2, 0x25);
            this.ndMinPixels.Name = "ndMinPixels";
            this.ndMinPixels.Size = new Size(0x37, 20);
            this.ndMinPixels.TabIndex = 3;
            this.ndMinPixels.TextAlign = HorizontalAlignment.Right;
            this.ndMinPixels.ValueChanged += new EventHandler(this.ndMinPixels_ValueChanged);
            this.cbAllowDrag.FlatStyle = FlatStyle.Flat;
            this.cbAllowDrag.Location = new Point(0x30, 0x45);
            this.cbAllowDrag.Name = "cbAllowDrag";
            this.cbAllowDrag.Size = new Size(0x90, 0x18);
            this.cbAllowDrag.TabIndex = 4;
            this.cbAllowDrag.Text = "Allow &Drag";
            this.cbAllowDrag.CheckedChanged += new EventHandler(this.cbAllowDrag_CheckedChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x30, 0x60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "Cursor:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.cbCursorDrag.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursorDrag.Location = new Point(0x60, 0x5e);
            this.cbCursorDrag.Name = "cbCursorDrag";
            this.cbCursorDrag.Size = new Size(0x79, 0x15);
            this.cbCursorDrag.TabIndex = 6;
            this.cbCursorDrag.SelectedIndexChanged += new EventHandler(this.cbCursorDrag_SelectedIndexChanged);
            this.cbCursorResize.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursorResize.Location = new Point(0x60, 0x92);
            this.cbCursorResize.Name = "cbCursorResize";
            this.cbCursorResize.Size = new Size(0x79, 0x15);
            this.cbCursorResize.TabIndex = 8;
            this.cbCursorResize.SelectedIndexChanged += new EventHandler(this.cbCursorResize_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x30, 0x94);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x29, 0x10);
            this.label4.TabIndex = 7;
            this.label4.Text = "Cursor:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.cbAllowResize.FlatStyle = FlatStyle.Flat;
            this.cbAllowResize.Location = new Point(0x30, 0x7a);
            this.cbAllowResize.Name = "cbAllowResize";
            this.cbAllowResize.Size = new Size(0x90, 0x18);
            this.cbAllowResize.TabIndex = 9;
            this.cbAllowResize.Text = "Allow &Resize";
            this.cbAllowResize.CheckedChanged += new EventHandler(this.cbResize_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe9, 0xd5);
            base.Controls.Add(this.cbAllowResize);
            base.Controls.Add(this.cbCursorResize);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cbCursorDrag);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cbAllowDrag);
            base.Controls.Add(this.ndMinPixels);
            base.Controls.Add(this.label2);
            base.Name = "GanttToolEditor";
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.ndMinPixels, 0);
            base.Controls.SetChildIndex(this.cbAllowDrag, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.cbCursorDrag, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.cbCursorResize, 0);
            base.Controls.SetChildIndex(this.cbAllowResize, 0);
            this.ndMinPixels.EndInit();
            base.ResumeLayout(false);
        }

        private void ndMinPixels_ValueChanged(object sender, EventArgs e)
        {
            this.gantt.MinPixels = (int) this.ndMinPixels.Value;
        }
    }
}

