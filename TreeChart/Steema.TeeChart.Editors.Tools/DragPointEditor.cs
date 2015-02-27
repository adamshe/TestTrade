namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DragPointEditor : ToolSeriesEditor
    {
        private ComboBox cbButton;
        private ComboBox cbCursor;
        private IContainer components;
        private GroupBox groupBox1;
        private Label label2;
        private Label label3;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private DragPoint tool;

        public DragPointEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public DragPointEditor(Steema.TeeChart.Tools.Tool t) : this()
        {
            this.tool = (DragPoint) t;
            base.SetTool(this.tool);
            if (this.tool.Style == DragPointStyles.X)
            {
                this.radioButton1.Checked = true;
            }
            else if (this.tool.Style == DragPointStyles.Y)
            {
                this.radioButton2.Checked = true;
            }
            else
            {
                this.radioButton3.Checked = true;
            }
            EditorUtils.FillCursors(this.cbCursor, this.tool.Cursor);
            this.cbButton.SelectedIndex = EditorUtils.MouseButtonIndex(this.tool.Button);
        }

        private void cbButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tool.Button = EditorUtils.MouseButtonFromIndex(this.cbButton.SelectedIndex);
        }

        private void cbCursor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tool.Cursor = EditorUtils.StringToCursor(this.cbCursor.SelectedItem.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DragPointEditor_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.label2 = new Label();
            this.cbButton = new ComboBox();
            this.label3 = new Label();
            this.cbCursor = new ComboBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            base.CBSeries.ItemHeight = 13;
            base.CBSeries.Name = "CBSeries";
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(0x33, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x88, 0x58);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Styl&e:";
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(8, 0x42);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x70, 20);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&Both";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 0x2a);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x70, 20);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Y";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x70, 20);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&X";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(8, 0x90);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4d, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mouse &Button:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.cbButton.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbButton.Items.AddRange(new object[] { "Left", "Middle", "Right", "X Button 1", "X Button 2" });
            this.cbButton.Location = new Point(0x58, 0x8d);
            this.cbButton.Name = "cbButton";
            this.cbButton.Size = new Size(0x68, 0x15);
            this.cbButton.TabIndex = 4;
            this.cbButton.SelectedIndexChanged += new EventHandler(this.cbButton_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x25, 180);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x29, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Cursor:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.cbCursor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbCursor.Location = new Point(0x58, 0xb0);
            this.cbCursor.Name = "cbCursor";
            this.cbCursor.Size = new Size(0x68, 0x15);
            this.cbCursor.TabIndex = 6;
            this.cbCursor.SelectedIndexChanged += new EventHandler(this.cbCursor_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe8, 0xdd);
            base.Controls.Add(this.cbCursor);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cbButton);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.groupBox1);
            base.Name = "DragPointEditor";
            base.Load += new EventHandler(this.DragPointEditor_Load);
            base.Controls.SetChildIndex(this.groupBox1, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.cbButton, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.cbCursor, 0);
            base.Controls.SetChildIndex(base.CBSeries, 0);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.Style = DragPointStyles.X;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.Style = DragPointStyles.Y;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.tool.Style = DragPointStyles.Both;
        }
    }
}

