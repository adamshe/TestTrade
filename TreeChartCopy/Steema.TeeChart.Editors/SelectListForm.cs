namespace Steema.TeeChart.Editors
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SelectListForm : Form
    {
        private Button BLeftAll;
        private Button BLeftOne;
        private Button BMoveDown;
        private Button BMoveUP;
        private Button BRightAll;
        private Button BRightOne;
        private Container components = null;
        internal Control controlToEnable;
        public ListBox FromList;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Splitter splitter1;
        public ListBox ToList;

        public SelectListForm()
        {
            this.InitializeComponent();
        }

        private void BLeftAll_Click(object sender, EventArgs e)
        {
            this.MoveListAll(this.ToList, this.FromList);
            this.Changed();
        }

        private void BLeftOne_Click(object sender, EventArgs e)
        {
            this.MoveList(this.ToList, this.FromList);
            this.Changed();
        }

        private void BMoveDown_Click(object sender, EventArgs e)
        {
            int num = (sender == this.BMoveUP) ? -1 : 1;
            int selectedIndex = this.ToList.SelectedIndex;
            object obj2 = this.ToList.Items[selectedIndex];
            object obj3 = this.ToList.Items[selectedIndex + num];
            this.ToList.Items[selectedIndex] = obj3;
            this.ToList.Items[selectedIndex + num] = obj2;
            this.ToList.SelectedIndex = selectedIndex + num;
            this.ToList.Focus();
            this.Changed();
        }

        private void BRightAll_Click(object sender, EventArgs e)
        {
            this.MoveListAll(this.FromList, this.ToList);
            this.Changed();
        }

        private void BRightOne_Click(object sender, EventArgs e)
        {
            this.MoveList(this.FromList, this.ToList);
            this.Changed();
            if (this.FromList.Items.Count == 0)
            {
                this.ToList.Focus();
            }
        }

        private void Changed()
        {
            this.EnableButtons();
            if (this.controlToEnable != null)
            {
                this.controlToEnable.Enabled = true;
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

        public void EnableButtons()
        {
            this.BRightOne.Enabled = this.FromList.SelectedIndex != -1;
            this.BRightAll.Enabled = this.FromList.Items.Count > 0;
            this.BLeftOne.Enabled = this.ToList.SelectedIndex != -1;
            this.BLeftAll.Enabled = this.ToList.Items.Count > 0;
            this.BMoveUP.Enabled = this.BLeftOne.Enabled && (this.ToList.SelectedIndex > 0);
            this.BMoveDown.Enabled = this.BLeftOne.Enabled && (this.ToList.SelectedIndex < (this.ToList.Items.Count - 1));
        }

        private void FromList_DoubleClick(object sender, EventArgs e)
        {
            this.BRightOne_Click(this, null);
            this.ToList.SelectedIndex = this.ToList.Items.Count - 1;
            this.EnableButtons();
        }

        private void FromList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.BMoveDown = new Button();
            this.BMoveUP = new Button();
            this.panel2 = new Panel();
            this.label2 = new Label();
            this.label1 = new Label();
            this.panel3 = new Panel();
            this.splitter1 = new Splitter();
            this.panel4 = new Panel();
            this.ToList = new ListBox();
            this.panel5 = new Panel();
            this.BLeftAll = new Button();
            this.BLeftOne = new Button();
            this.BRightAll = new Button();
            this.BRightOne = new Button();
            this.FromList = new ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.BMoveDown);
            this.panel1.Controls.Add(this.BMoveUP);
            this.panel1.Dock = DockStyle.Right;
            this.panel1.Location = new Point(0x155, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x2a, 0x85);
            this.panel1.TabIndex = 0;
            this.BMoveDown.BackColor = Color.Silver;
            this.BMoveDown.Enabled = false;
            this.BMoveDown.FlatStyle = FlatStyle.Flat;
            this.BMoveDown.Location = new Point(8, 60);
            this.BMoveDown.Name = "BMoveDown";
            this.BMoveDown.Size = new Size(0x1a, 0x17);
            this.BMoveDown.TabIndex = 1;
            this.BMoveDown.Click += new EventHandler(this.BMoveDown_Click);
            this.BMoveUP.BackColor = Color.Silver;
            this.BMoveUP.Enabled = false;
            this.BMoveUP.FlatStyle = FlatStyle.Flat;
            this.BMoveUP.Location = new Point(8, 0x1a);
            this.BMoveUP.Name = "BMoveUP";
            this.BMoveUP.Size = new Size(0x1a, 0x17);
            this.BMoveUP.TabIndex = 0;
            this.BMoveUP.Click += new EventHandler(this.BMoveDown_Click);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = DockStyle.Top;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x155, 0x12);
            this.panel2.TabIndex = 12;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xa2, 2);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x33, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Selected:";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "A&vailable:";
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.FromList);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0, 0x12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x155, 0x73);
            this.panel3.TabIndex = 13;
            this.splitter1.Location = new Point(130, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0x73);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.panel4.Controls.Add(this.ToList);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(130, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0xd3, 0x73);
            this.panel4.TabIndex = 13;
            this.ToList.Dock = DockStyle.Fill;
            this.ToList.IntegralHeight = false;
            this.ToList.Location = new Point(0x22, 0);
            this.ToList.Name = "ToList";
            this.ToList.Size = new Size(0xb1, 0x73);
            this.ToList.TabIndex = 0;
            this.ToList.DoubleClick += new EventHandler(this.ToList_DoubleClick);
            this.ToList.SelectedIndexChanged += new EventHandler(this.ToList_SelectedIndexChanged);
            this.panel5.Controls.Add(this.BLeftAll);
            this.panel5.Controls.Add(this.BLeftOne);
            this.panel5.Controls.Add(this.BRightAll);
            this.panel5.Controls.Add(this.BRightOne);
            this.panel5.Dock = DockStyle.Left;
            this.panel5.Location = new Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x22, 0x73);
            this.panel5.TabIndex = 0;
            this.BLeftAll.FlatStyle = FlatStyle.Flat;
            this.BLeftAll.Location = new Point(4, 0x4f);
            this.BLeftAll.Name = "BLeftAll";
            this.BLeftAll.Size = new Size(0x1c, 0x17);
            this.BLeftAll.TabIndex = 2;
            this.BLeftAll.Text = "<<";
            this.BLeftAll.Click += new EventHandler(this.BLeftAll_Click);
            this.BLeftOne.FlatStyle = FlatStyle.Flat;
            this.BLeftOne.Location = new Point(4, 0x35);
            this.BLeftOne.Name = "BLeftOne";
            this.BLeftOne.Size = new Size(0x1c, 0x17);
            this.BLeftOne.TabIndex = 1;
            this.BLeftOne.Text = "<";
            this.BLeftOne.Click += new EventHandler(this.BLeftOne_Click);
            this.BRightAll.FlatStyle = FlatStyle.Flat;
            this.BRightAll.Location = new Point(4, 0x1b);
            this.BRightAll.Name = "BRightAll";
            this.BRightAll.Size = new Size(0x1c, 0x17);
            this.BRightAll.TabIndex = 0;
            this.BRightAll.Text = ">>";
            this.BRightAll.Click += new EventHandler(this.BRightAll_Click);
            this.BRightOne.FlatStyle = FlatStyle.Flat;
            this.BRightOne.Location = new Point(4, 1);
            this.BRightOne.Name = "BRightOne";
            this.BRightOne.Size = new Size(0x1c, 0x17);
            this.BRightOne.TabIndex = 13;
            this.BRightOne.Text = ">";
            this.BRightOne.Click += new EventHandler(this.BRightOne_Click);
            this.FromList.Dock = DockStyle.Left;
            this.FromList.IntegralHeight = false;
            this.FromList.Location = new Point(0, 0);
            this.FromList.Name = "FromList";
            this.FromList.Size = new Size(130, 0x73);
            this.FromList.TabIndex = 0;
            this.FromList.DoubleClick += new EventHandler(this.FromList_DoubleClick);
            this.FromList.SelectedIndexChanged += new EventHandler(this.FromList_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x17f, 0x85);
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "SelectListForm";
            base.Load += new EventHandler(this.SelectListForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void MoveList(ListBox source, ListBox dest)
        {
            object[] destination = new object[source.SelectedItems.Count];
            source.SelectedItems.CopyTo(destination, 0);
            dest.Items.AddRange(destination);
            foreach (int num in source.SelectedIndices)
            {
                source.Items.RemoveAt(num);
            }
        }

        private void MoveListAll(ListBox source, ListBox dest)
        {
            dest.Items.AddRange(source.Items);
            source.Items.Clear();
        }

        private void SelectListForm_Load(object sender, EventArgs e)
        {
            EditorUtils.GetUpDown(this.BMoveUP, this.BMoveDown);
            this.EnableButtons();
            if (this.FromList.Items.Count > 0)
            {
                this.FromList.Focus();
            }
            else
            {
                this.ToList.Focus();
            }
        }

        private void ToList_DoubleClick(object sender, EventArgs e)
        {
            this.BLeftOne_Click(this, null);
        }

        private void ToList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableButtons();
        }
    }
}

