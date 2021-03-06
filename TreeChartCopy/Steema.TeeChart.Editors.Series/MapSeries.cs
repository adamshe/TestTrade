namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MapSeries : Form
    {
        private Button BBrush;
        private Button BGradient;
        private Button BMapBrush;
        private Button ButtonColor1;
        private ButtonPen ButtonPen1;
        private Button ButtonPen2;
        private CheckBox CBGlobalBrush;
        private CheckBox CBGlobalPen;
        private DataGrid ChartGrid1;
        private ListBox ChartListBox1;
        private Container components;
        private TextBox EditZ;
        private TextBox EText;
        private Label label1;
        private Label label2;
        private Map m;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Button SBAdd;
        private Button SBDelete;
        private Splitter splitter1;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage TabGlobal;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage TabShapes;

        public MapSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public MapSeries(Series s)
        {
            this.components = null;
            this.m = (Map) s;
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
            this.tabControl1 = new TabControl();
            this.TabGlobal = new TabPage();
            this.BMapBrush = new Button();
            this.ButtonPen1 = new ButtonPen();
            this.TabShapes = new TabPage();
            this.tabControl2 = new TabControl();
            this.tabPage1 = new TabPage();
            this.EditZ = new TextBox();
            this.EText = new TextBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.CBGlobalBrush = new CheckBox();
            this.CBGlobalPen = new CheckBox();
            this.BGradient = new Button();
            this.ButtonColor1 = new Button();
            this.BBrush = new Button();
            this.ButtonPen2 = new Button();
            this.tabPage2 = new TabPage();
            this.ChartGrid1 = new DataGrid();
            this.splitter1 = new Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ChartListBox1 = new ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SBDelete = new Button();
            this.SBAdd = new Button();
            this.tabControl1.SuspendLayout();
            this.TabGlobal.SuspendLayout();
            this.TabShapes.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ChartGrid1.BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.TabGlobal);
            this.tabControl1.Controls.Add(this.TabShapes);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x184, 0xcb);
            this.tabControl1.TabIndex = 0;
            this.TabGlobal.Controls.Add(this.BMapBrush);
            this.TabGlobal.Controls.Add(this.ButtonPen1);
            this.TabGlobal.Location = new Point(4, 0x16);
            this.TabGlobal.Name = "TabGlobal";
            this.TabGlobal.Size = new Size(380, 0xb1);
            this.TabGlobal.TabIndex = 0;
            this.TabGlobal.Text = "Global";
            this.BMapBrush.FlatStyle = FlatStyle.Flat;
            this.BMapBrush.Location = new Point(12, 0x30);
            this.BMapBrush.Name = "BMapBrush";
            this.BMapBrush.TabIndex = 1;
            this.BMapBrush.Text = "B&rush...";
            this.ButtonPen1.FlatStyle = FlatStyle.Flat;
            this.ButtonPen1.Location = new Point(12, 0x10);
            this.ButtonPen1.Name = "ButtonPen1";
            this.ButtonPen1.TabIndex = 0;
            this.ButtonPen1.Text = "&Border...";
            this.TabShapes.Controls.Add(this.tabControl2);
            this.TabShapes.Controls.Add(this.splitter1);
            this.TabShapes.Controls.Add(this.panel1);
            this.TabShapes.Location = new Point(4, 0x16);
            this.TabShapes.Name = "TabShapes";
            this.TabShapes.Size = new Size(380, 0xb1);
            this.TabShapes.TabIndex = 1;
            this.TabShapes.Text = "Shapes";
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Location = new Point(0x60, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x11c, 0xb1);
            this.tabControl2.TabIndex = 0;
            this.tabPage1.Controls.Add(this.EditZ);
            this.tabPage1.Controls.Add(this.EText);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.CBGlobalBrush);
            this.tabPage1.Controls.Add(this.CBGlobalPen);
            this.tabPage1.Controls.Add(this.BGradient);
            this.tabPage1.Controls.Add(this.ButtonColor1);
            this.tabPage1.Controls.Add(this.BBrush);
            this.tabPage1.Controls.Add(this.ButtonPen2);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x114, 0x97);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Format";
            this.EditZ.BorderStyle = BorderStyle.FixedSingle;
            this.EditZ.Location = new Point(0x38, 0x70);
            this.EditZ.Name = "EditZ";
            this.EditZ.Size = new Size(0x70, 20);
            this.EditZ.TabIndex = 9;
            this.EditZ.Text = "0";
            this.EditZ.TextAlign = HorizontalAlignment.Right;
            this.EText.BorderStyle = BorderStyle.FixedSingle;
            this.EText.Location = new Point(0x38, 0x58);
            this.EText.Name = "EText";
            this.EText.Size = new Size(0x70, 20);
            this.EText.TabIndex = 7;
            this.EText.Text = "";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x20, 0x74);
            this.label2.Name = "label2";
            this.label2.Size = new Size(14, 0x10);
            this.label2.TabIndex = 8;
            this.label2.Text = "&Z:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 0x5c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 6;
            this.label1.Text = "&Text:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBGlobalBrush.FlatStyle = FlatStyle.Flat;
            this.CBGlobalBrush.Location = new Point(0x68, 0x34);
            this.CBGlobalBrush.Name = "CBGlobalBrush";
            this.CBGlobalBrush.Size = new Size(0x40, 0x10);
            this.CBGlobalBrush.TabIndex = 3;
            this.CBGlobalBrush.Text = "Gl&obal";
            this.CBGlobalPen.FlatStyle = FlatStyle.Flat;
            this.CBGlobalPen.Location = new Point(0x68, 20);
            this.CBGlobalPen.Name = "CBGlobalPen";
            this.CBGlobalPen.Size = new Size(0x40, 0x10);
            this.CBGlobalPen.TabIndex = 2;
            this.CBGlobalPen.Text = "&Global";
            this.BGradient.FlatStyle = FlatStyle.Flat;
            this.BGradient.Location = new Point(0xb8, 0x30);
            this.BGradient.Name = "BGradient";
            this.BGradient.TabIndex = 5;
            this.BGradient.Text = "&Gradient...";
            this.ButtonColor1.FlatStyle = FlatStyle.Flat;
            this.ButtonColor1.Location = new Point(0xb8, 0x10);
            this.ButtonColor1.Name = "ButtonColor1";
            this.ButtonColor1.TabIndex = 4;
            this.ButtonColor1.Text = "&Color...";
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(0x10, 0x30);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 1;
            this.BBrush.Text = "Br&ush...";
            this.ButtonPen2.FlatStyle = FlatStyle.Flat;
            this.ButtonPen2.Location = new Point(0x10, 0x10);
            this.ButtonPen2.Name = "ButtonPen2";
            this.ButtonPen2.TabIndex = 0;
            this.ButtonPen2.Text = "&Border...";
            this.tabPage2.Controls.Add(this.ChartGrid1);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x114, 0x97);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data";
            this.ChartGrid1.DataMember = "";
            this.ChartGrid1.Dock = DockStyle.Top;
            this.ChartGrid1.HeaderForeColor = SystemColors.ControlText;
            this.ChartGrid1.Location = new Point(0, 0);
            this.ChartGrid1.Name = "ChartGrid1";
            this.ChartGrid1.Size = new Size(0x114, 120);
            this.ChartGrid1.TabIndex = 0;
            this.splitter1.Location = new Point(0x58, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(8, 0xb1);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.panel1.Controls.Add(this.ChartListBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x58, 0xb1);
            this.panel1.TabIndex = 0;
            this.ChartListBox1.Dock = DockStyle.Fill;
            this.ChartListBox1.IntegralHeight = false;
            this.ChartListBox1.Location = new Point(0, 0);
            this.ChartListBox1.Name = "ChartListBox1";
            this.ChartListBox1.Size = new Size(0x58, 0x91);
            this.ChartListBox1.TabIndex = 0;
            this.panel2.Controls.Add(this.SBDelete);
            this.panel2.Controls.Add(this.SBAdd);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0x91);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x58, 0x20);
            this.panel2.TabIndex = 1;
            this.SBDelete.Enabled = false;
            this.SBDelete.FlatStyle = FlatStyle.Flat;
            this.SBDelete.Location = new Point(0x30, 5);
            this.SBDelete.Name = "SBDelete";
            this.SBDelete.Size = new Size(0x18, 0x17);
            this.SBDelete.TabIndex = 1;
            this.SBDelete.Text = "-";
            this.SBAdd.FlatStyle = FlatStyle.Flat;
            this.SBAdd.Location = new Point(15, 5);
            this.SBAdd.Name = "SBAdd";
            this.SBAdd.Size = new Size(0x18, 0x17);
            this.SBAdd.TabIndex = 0;
            this.SBAdd.Text = "+";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xcb);
            base.Controls.Add(this.tabControl1);
            base.Name = "MapSeries";
            this.tabControl1.ResumeLayout(false);
            this.TabGlobal.ResumeLayout(false);
            this.TabShapes.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ChartGrid1.EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

