namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolsGallery : Form
    {
        private Button button1;
        private Button button2;
        private Container components = null;
        private static bool IsWinForm = false;
        private Label lDescription;
        private ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ArrayList tools;

        public ToolsGallery()
        {
            this.InitializeComponent();
            this.FillTools();
        }

        public static Steema.TeeChart.Tools.Tool CreateNew(Chart chart, IContainer container)
        {
            IsWinForm = chart.parent.FindParentForm() != null;
            using (ToolsGallery gallery = new ToolsGallery())
            {
                Steema.TeeChart.Tools.Tool component = null;
                if (gallery.ShowDialog() == DialogResult.OK)
                {
                    component = gallery.ToolAt(gallery.listBox1.SelectedIndices[0]);
                    if (container != null)
                    {
                        container.Add(component);
                    }
                }
                return component;
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

        private void FillTools()
        {
            this.tools = new ArrayList();
            this.listBox1.BeginUpdate();
            this.listBox1.Items.Clear();
            foreach (System.Type type in Utils.ToolTypesOf)
            {
                Steema.TeeChart.Tools.Tool t = (Steema.TeeChart.Tools.Tool) Activator.CreateInstance(type);
                if (this.FilterTool(t))
                {
                    this.tools.Add(t);
                    this.listBox1.Items.Add(t.ToString());
                }
            }
            this.listBox1.EndUpdate();
        }

        private bool FilterTool(Steema.TeeChart.Tools.Tool t)
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 0:
                    if (IsWinForm)
                    {
                        return (t is ToolSeries);
                    }
                    return (t is ExtraLegend);

                case 1:
                    if (IsWinForm)
                    {
                        return (t is ToolAxis);
                    }
                    return ((t is ColorBand) || (t is GridBand));

                case 2:
                    if (IsWinForm)
                    {
                        if (t is ToolSeries)
                        {
                            return false;
                        }
                        return !(t is ToolAxis);
                    }
                    return ((t is Annotation) || (t is PageNumber));
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new Button();
            this.button1 = new Button();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.listBox1 = new ListBox();
            this.tabPage2 = new TabPage();
            this.tabPage3 = new TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lDescription = new Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x128, 40);
            this.panel1.TabIndex = 0;
            this.button2.DialogResult = DialogResult.Cancel;
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0xd0, 8);
            this.button2.Name = "button2";
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button1.DialogResult = DialogResult.OK;
            this.button1.Enabled = false;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(120, 8);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Add";
            this.tabControl1.Appearance = TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x128, 0xd5);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Location = new Point(4, 0x19);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x120, 0xb8);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Series";
            this.listBox1.Dock = DockStyle.Fill;
            this.listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox1.IntegralHeight = false;
            this.listBox1.ItemHeight = 0x12;
            this.listBox1.Location = new Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(0x120, 0xb8);
            this.listBox1.Sorted = true;
            this.listBox1.TabIndex = 0;
            this.listBox1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
            this.listBox1.DrawItem += new DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
            this.tabPage2.Location = new Point(4, 0x19);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x120, 0xb8);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Axis";
            this.tabPage3.Location = new Point(4, 0x19);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0x120, 0xb8);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Other";
            this.panel2.Controls.Add(this.lDescription);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0xd5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x128, 0x30);
            this.panel2.TabIndex = 2;
            this.lDescription.BorderStyle = BorderStyle.FixedSingle;
            this.lDescription.Dock = DockStyle.Fill;
            this.lDescription.Location = new Point(0, 0);
            this.lDescription.Name = "lDescription";
            this.lDescription.Size = new Size(0x128, 0x30);
            this.lDescription.TabIndex = 0;
            this.lDescription.UseMnemonic = false;
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x128, 0xd5);
            this.panel3.TabIndex = 3;
            base.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.button2;
            base.ClientSize = new Size(0x128, 0x12d);
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "ToolsGallery";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chart Tools Gallery";
            base.Load += new EventHandler(this.ToolsGallery_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index > -1)
            {
                using (Image image = this.ToolAt(e.Index).GetBitmapEditor())
                {
                    if (image != null)
                    {
                        e.Graphics.DrawImage(image, e.Bounds.Left, e.Bounds.Top + 1);
                    }
                }
                Point point = new Point(0x16, e.Bounds.Top + ((this.listBox1.ItemHeight - this.listBox1.Font.Height) / 2));
                string s = this.listBox1.Items[e.Index].ToString();
                e.Graphics.DrawString(s, this.listBox1.Font, new SolidBrush(e.ForeColor), (PointF) point);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = true;
            this.lDescription.Text = this.ToolDescription(this.ToolAt(this.listBox1.SelectedIndex));
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillTools();
            this.listBox1.Parent = this.tabControl1.SelectedTab;
            this.lDescription.Text = "";
        }

        private Steema.TeeChart.Tools.Tool ToolAt(int index)
        {
            string str = this.listBox1.Items[index].ToString();
            foreach (object obj2 in this.tools)
            {
                if (obj2.ToString() == str)
                {
                    return (Steema.TeeChart.Tools.Tool) obj2;
                }
            }
            return null;
        }

        private string ToolDescription(Steema.TeeChart.Tools.Tool t)
        {
            return t.Summary;
        }

        private void ToolsGallery_Load(object sender, EventArgs e)
        {
            EditorUtils.Translate(this);
        }
    }
}

