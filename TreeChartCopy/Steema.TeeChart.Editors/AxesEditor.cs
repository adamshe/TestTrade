namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AxesEditor : Form
    {
        private AxisEditor axisEditor;
        private Button bAdd;
        private Button bDelete;
        private Chart chart;
        private CheckBox checkBoxBehind;
        private CheckBox checkBoxVisible;
        private Container components;
        private Label label1;
        private ListBox lbAxes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private Splitter splitter1;

        public AxesEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AxesEditor(Axis a, Control parent) : this(a.Chart, parent)
        {
            this.lbAxes.SelectedIndex = this.AxisIndex(a);
        }

        public AxesEditor(Chart c, Control parent) : this()
        {
            this.chart = c;
            this.lbAxes.Items.AddRange(new object[] { Texts.LeftAxis, Texts.RightAxis, Texts.TopAxis, Texts.BottomAxis, Texts.DepthAxis });
            if (this.axisEditor == null)
            {
                this.axisEditor = new AxisEditor(this.chart.axes.Left, this.panel5);
            }
            this.FillAxes();
            this.checkBoxBehind.Checked = this.chart.Axes.DrawBehind;
            this.checkBoxVisible.Checked = this.chart.Axes.Visible;
            EditorUtils.InsertForm(this, parent);
        }

        private void AddToContainer(Axis a)
        {
            IContainer chartContainer = this.chart.ChartContainer;
            if (chartContainer != null)
            {
                a.AddToContainer(chartContainer);
            }
        }

        private void AxesEditor_Load(object sender, EventArgs e)
        {
        }

        private int AxisIndex(Axis a)
        {
            if (a == this.chart.Axes.Left)
            {
                return 0;
            }
            if (a == this.chart.Axes.Right)
            {
                return 1;
            }
            if (a == this.chart.Axes.Top)
            {
                return 2;
            }
            if (a == this.chart.Axes.Bottom)
            {
                return 3;
            }
            if (a == this.chart.Axes.Depth)
            {
                return 4;
            }
            return (this.chart.Axes.custom.IndexOf(a) + 5);
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            Axis axis = Axes.CreateNewAxis(this.chart);
            this.chart.Axes.custom.Add(axis);
            this.AddToContainer(axis);
            this.FillAxes();
            this.lbAxes.SelectedIndex = this.lbAxes.Items.Count - 1;
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            this.RemoveFromContainer(this.chart.Axes.custom[this.lbAxes.SelectedIndex - 5]);
            this.chart.Axes.custom.RemoveAt(this.lbAxes.SelectedIndex - 5);
            this.FillAxes();
        }

        private void checkBoxBehind_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Axes.DrawBehind = this.checkBoxBehind.Checked;
        }

        private void checkBoxVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.chart.Axes.Visible = this.checkBoxVisible.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillAxes()
        {
            this.lbAxes.SelectedIndex = 0;
            while (this.lbAxes.Items.Count > 5)
            {
                this.lbAxes.Items.RemoveAt(5);
            }
            foreach (Axis axis in this.chart.Axes.custom)
            {
                this.lbAxes.Items.Add("Custom " + this.chart.Axes.custom.IndexOf(axis).ToString());
            }
            if (this.lbAxes.SelectedIndex == -1)
            {
                this.lbAxes.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbAxes = new ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new Label();
            this.checkBoxBehind = new CheckBox();
            this.checkBoxVisible = new CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bDelete = new Button();
            this.bAdd = new Button();
            this.splitter1 = new Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x58, 0xed);
            this.panel1.TabIndex = 0;
            this.panel4.Controls.Add(this.lbAxes);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(0, 0x48);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x58, 0x89);
            this.panel4.TabIndex = 6;
            this.lbAxes.Dock = DockStyle.Fill;
            this.lbAxes.IntegralHeight = false;
            this.lbAxes.Location = new Point(0, 0);
            this.lbAxes.Name = "lbAxes";
            this.lbAxes.Size = new Size(0x58, 0x89);
            this.lbAxes.TabIndex = 0;
            this.lbAxes.SelectedIndexChanged += new EventHandler(this.lbAxes_SelectedIndexChanged);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.checkBoxBehind);
            this.panel3.Controls.Add(this.checkBoxVisible);
            this.panel3.Dock = DockStyle.Top;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x58, 0x48);
            this.panel3.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(7, 0x35);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x21, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "Axes:";
            this.label1.UseMnemonic = false;
            this.checkBoxBehind.FlatStyle = FlatStyle.Flat;
            this.checkBoxBehind.Location = new Point(9, 0x1a);
            this.checkBoxBehind.Name = "checkBoxBehind";
            this.checkBoxBehind.Size = new Size(0x48, 0x18);
            this.checkBoxBehind.TabIndex = 1;
            this.checkBoxBehind.Text = "&Behind";
            this.checkBoxBehind.CheckedChanged += new EventHandler(this.checkBoxBehind_CheckedChanged);
            this.checkBoxVisible.FlatStyle = FlatStyle.Flat;
            this.checkBoxVisible.Location = new Point(9, 5);
            this.checkBoxVisible.Name = "checkBoxVisible";
            this.checkBoxVisible.Size = new Size(0x48, 0x18);
            this.checkBoxVisible.TabIndex = 0;
            this.checkBoxVisible.Text = "&Visible";
            this.checkBoxVisible.CheckedChanged += new EventHandler(this.checkBoxVisible_CheckedChanged);
            this.panel2.Controls.Add(this.bDelete);
            this.panel2.Controls.Add(this.bAdd);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0xd1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x58, 0x1c);
            this.panel2.TabIndex = 1;
            this.bDelete.Enabled = false;
            this.bDelete.FlatStyle = FlatStyle.Flat;
            this.bDelete.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.bDelete.Location = new Point(0x30, 2);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new Size(0x18, 0x17);
            this.bDelete.TabIndex = 1;
            this.bDelete.Text = "-";
            this.bDelete.Click += new EventHandler(this.bDelete_Click);
            this.bAdd.FlatStyle = FlatStyle.Flat;
            this.bAdd.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.bAdd.Location = new Point(8, 2);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new Size(0x18, 0x17);
            this.bAdd.TabIndex = 0;
            this.bAdd.Text = "+";
            this.bAdd.Click += new EventHandler(this.bAdd_Click);
            this.splitter1.BorderStyle = BorderStyle.FixedSingle;
            this.splitter1.Location = new Point(0x58, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0xed);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            this.panel5.Dock = DockStyle.Fill;
            this.panel5.Location = new Point(0x5b, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x135, 0xed);
            this.panel5.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(400, 0xed);
            base.Controls.Add(this.panel5);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.panel1);
            base.Name = "AxesEditor";
            this.Text = "AxisEditor";
            base.Load += new EventHandler(this.AxesEditor_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lbAxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bDelete.Enabled = this.lbAxes.SelectedIndex > 4;
            base.Invalidate();
            this.axisEditor.Axis = this.axis;
        }

        private void RemoveFromContainer(Axis a)
        {
            IContainer chartContainer = this.chart.ChartContainer;
            if (chartContainer != null)
            {
                a.RemoveFromContainer(chartContainer);
            }
        }

        private Axis axis
        {
            get
            {
                switch (this.lbAxes.SelectedIndex)
                {
                    case -1:
                        return null;

                    case 0:
                        return this.chart.Axes.Left;

                    case 1:
                        return this.chart.Axes.Right;

                    case 2:
                        return this.chart.Axes.Top;

                    case 3:
                        return this.chart.Axes.Bottom;

                    case 4:
                        return this.chart.Axes.Depth;
                }
                return this.chart.Axes.custom[this.lbAxes.SelectedIndex - 5];
            }
        }
    }
}

