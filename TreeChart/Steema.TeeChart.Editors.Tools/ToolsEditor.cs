namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolsEditor : Form
    {
        private Button bDown;
        private Button bUp;
        private Button button11;
        private Button button12;
        private Chart chart;
        private CheckBox checkBox9;
        private Container components;
        private ListBox listBox1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private Splitter splitter1;
        private Form toolsForm;

        public ToolsEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ToolsEditor(Chart c, Control parent) : this()
        {
            this.chart = c;
            this.FillTools();
            EditorUtils.InsertForm(this, parent);
            if (this.chart.Tools.Count > 0)
            {
                this.listBox1.SelectedIndex = 0;
            }
            EditorUtils.GetUpDown(this.bUp, this.bDown);
            this.EnableUpDown();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Tools.Tool tool = ToolsGallery.CreateNew(this.chart, this.chart.ChartContainer);
            if (tool != null)
            {
                int num = this.chart.Tools.Add(tool);
                this.FillTools();
                this.listBox1.SelectedIndex = num;
                this.EnableUpDown();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBox1.SelectedIndex;
            Steema.TeeChart.Tools.Tool s = this.Tool;
            this.chart.Tools.Remove(s);
            this.RemoveToolsForm();
            s.Dispose();
            this.FillTools();
            if (selectedIndex >= this.listBox1.Items.Count)
            {
                selectedIndex--;
            }
            if (selectedIndex != -1)
            {
                this.listBox1.SelectedIndex = selectedIndex;
            }
            this.button12.Enabled = this.listBox1.SelectedIndex >= 0;
            this.checkBox9.Enabled = this.button12.Enabled;
            this.EnableUpDown();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            this.Tool.Active = this.checkBox9.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableUpDown()
        {
            this.bUp.Enabled = this.listBox1.SelectedIndex > 0;
            this.bDown.Enabled = this.listBox1.SelectedIndex < (this.listBox1.Items.Count - 1);
        }

        private void FillTools()
        {
            this.listBox1.BeginUpdate();
            this.listBox1.Items.Clear();
            foreach (Steema.TeeChart.Tools.Tool tool in this.chart.Tools)
            {
                this.listBox1.Items.Add(tool.ToString());
            }
            this.listBox1.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.panel7 = new System.Windows.Forms.Panel();
            this.listBox1 = new ListBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.bDown = new Button();
            this.bUp = new Button();
            this.checkBox9 = new CheckBox();
            this.button12 = new Button();
            this.button11 = new Button();
            this.splitter1 = new Splitter();
            this.panel6.SuspendLayout();
            base.SuspendLayout();
            this.panel7.Dock = DockStyle.Fill;
            this.panel7.Location = new Point(0x80, 40);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(0xf8, 0xc5);
            this.panel7.TabIndex = 5;
            this.listBox1.BorderStyle = BorderStyle.FixedSingle;
            this.listBox1.Dock = DockStyle.Left;
            this.listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 0x12;
            this.listBox1.Location = new Point(0, 40);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(0x80, 0xc5);
            this.listBox1.TabIndex = 4;
            this.listBox1.DrawItem += new DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
            this.panel6.Controls.Add(this.bDown);
            this.panel6.Controls.Add(this.bUp);
            this.panel6.Controls.Add(this.checkBox9);
            this.panel6.Controls.Add(this.button12);
            this.panel6.Controls.Add(this.button11);
            this.panel6.Dock = DockStyle.Top;
            this.panel6.Location = new Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(0x178, 40);
            this.panel6.TabIndex = 3;
            this.bDown.FlatStyle = FlatStyle.Flat;
            this.bDown.Location = new Point(0x141, 8);
            this.bDown.Name = "bDown";
            this.bDown.Size = new Size(0x17, 0x17);
            this.bDown.TabIndex = 4;
            this.bUp.FlatStyle = FlatStyle.Flat;
            this.bUp.Location = new Point(0x120, 8);
            this.bUp.Name = "bUp";
            this.bUp.Size = new Size(0x17, 0x17);
            this.bUp.TabIndex = 3;
            this.checkBox9.Enabled = false;
            this.checkBox9.FlatStyle = FlatStyle.Flat;
            this.checkBox9.Location = new Point(200, 8);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new Size(80, 0x18);
            this.checkBox9.TabIndex = 2;
            this.checkBox9.Text = "&Active";
            this.checkBox9.CheckedChanged += new EventHandler(this.checkBox9_CheckedChanged);
            this.button12.Enabled = false;
            this.button12.FlatStyle = FlatStyle.Flat;
            this.button12.Location = new Point(0x60, 8);
            this.button12.Name = "button12";
            this.button12.TabIndex = 1;
            this.button12.Text = "D&elete";
            this.button12.Click += new EventHandler(this.button12_Click);
            this.button11.FlatStyle = FlatStyle.Flat;
            this.button11.Location = new Point(8, 8);
            this.button11.Name = "button11";
            this.button11.TabIndex = 0;
            this.button11.Text = "A&dd...";
            this.button11.Click += new EventHandler(this.button11_Click);
            this.splitter1.Location = new Point(0x80, 40);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0xc5);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x178, 0xed);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.panel7);
            base.Controls.Add(this.listBox1);
            base.Controls.Add(this.panel6);
            base.Name = "ToolsEditor";
            this.Text = "ToolsEditor";
            this.panel6.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index > -1)
            {
                Steema.TeeChart.Tools.Tool tool = this.chart.Tools[e.Index];
                using (Image image = tool.GetBitmapEditor())
                {
                    if (image != null)
                    {
                        e.Graphics.DrawImage(image, e.Bounds.Left, e.Bounds.Top + 1);
                    }
                }
                Point point = new Point(0x26, e.Bounds.Top + ((this.listBox1.ItemHeight - this.listBox1.Font.Height) / 2));
                string description = tool.Description;
                e.Graphics.DrawString(description, this.listBox1.Font, new SolidBrush(e.ForeColor), (PointF) point);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = Utils.ToolTypeIndex(this.chart.Tools[this.listBox1.SelectedIndex]);
            System.Type type = EditorUtils.ToolEditorsOf[index];
            if (type != null)
            {
                this.RemoveToolsForm();
                object[] args = new object[] { this.Tool };
                this.toolsForm = (Form) Activator.CreateInstance(type, args);
                EditorUtils.InsertForm(this.toolsForm, this.panel7);
                EditorUtils.Translate(this.toolsForm);
            }
            this.button12.Enabled = true;
            this.checkBox9.Enabled = true;
            this.checkBox9.Checked = this.Tool.Active;
            this.EnableUpDown();
        }

        internal void Reload()
        {
            if (this.chart.Tools.Count != this.listBox1.Items.Count)
            {
                this.FillTools();
            }
            if (this.listBox1.SelectedIndex != -1)
            {
                this.listBox1_SelectedIndexChanged(this.listBox1, null);
            }
        }

        private void RemoveToolsForm()
        {
            if (this.toolsForm != null)
            {
                this.toolsForm.Dispose();
            }
        }

        public static bool ShowEditor(Steema.TeeChart.Tools.Tool t)
        {
            return ShowEditor(t, null);
        }

        public static bool ShowEditor(Steema.TeeChart.Tools.Tool t, ITypeDescriptorContext context)
        {
            int index = Utils.ToolTypeIndex(t);
            if (EditorUtils.ToolEditorsOf[index] != null)
            {
                object[] args = new object[] { t };
                using (Form form = (Form) Activator.CreateInstance(EditorUtils.ToolEditorsOf[index], args))
                {
                    bool flag = EditorUtils.ShowFormModal(form);
                    if ((context != null) && flag)
                    {
                        context.OnComponentChanged();
                    }
                    return flag;
                }
            }
            return false;
        }

        private Steema.TeeChart.Tools.Tool Tool
        {
            get
            {
                return this.chart.Tools[this.listBox1.SelectedIndex];
            }
        }
    }
}

