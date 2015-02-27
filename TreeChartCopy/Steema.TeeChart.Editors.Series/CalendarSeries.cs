namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CalendarSeries : Form
    {
        private Button ButtonPen1;
        private CheckBox CBMonths;
        private CheckBox CBMonthUpper;
        private CheckBox CBNext;
        private CheckBox CBPrevious;
        private CheckBox CBToday;
        private CheckBox CBTrailing;
        private CheckBox CBWeekDays;
        private CheckBox CBWeekUpper;
        private Container components;
        private Calendar series;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabControl tabControl3;
        private TabControl tabControl4;
        private TabControl tabControl5;
        private TabControl tabControl6;
        private TabControl tabControl7;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;

        public CalendarSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CalendarSeries(Series s) : this()
        {
            this.series = (Calendar) s;
            bool flag = (Texts.Translator == null) || Texts.Translator.HasUpperCase();
            this.CBWeekUpper.Visible = flag;
            this.CBMonthUpper.Visible = flag;
        }

        private void AddShapeEditor(TabPage page, TabControl tabControl, TextShape c)
        {
            if (page.Tag == null)
            {
                page.Tag = CustomShapeEditor.Add(tabControl, c);
            }
        }

        private void ButtonPen1_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.Pen);
        }

        private void CalendarSeries_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.CBPrevious.Checked = this.series.PreviousMonthButton.Visible;
                this.CBNext.Checked = this.series.NextMonthButton.Visible;
                this.CBToday.Checked = this.series.Today.Visible;
                this.CBWeekDays.Checked = this.series.WeekDays.Visible;
                this.CBWeekUpper.Checked = this.series.WeekDays.UpperCase;
                this.CBMonths.Checked = this.series.Months.Visible;
                this.CBMonthUpper.Checked = this.series.Months.UpperCase;
                this.CBTrailing.Checked = this.series.Trailing.Visible;
            }
        }

        private void CBMonths_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Months.Visible = this.CBMonths.Checked;
        }

        private void CBMonthUpper_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Months.UpperCase = this.CBMonthUpper.Checked;
        }

        private void CBNext_CheckedChanged(object sender, EventArgs e)
        {
            this.series.NextMonthButton.Visible = this.CBNext.Checked;
        }

        private void CBPrevious_CheckedChanged(object sender, EventArgs e)
        {
            this.series.PreviousMonthButton.Visible = this.CBPrevious.Checked;
        }

        private void CBToday_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Today.Visible = this.CBToday.Checked;
        }

        private void CBTrailing_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Trailing.Visible = this.CBTrailing.Checked;
        }

        private void CBWeekDays_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WeekDays.Visible = this.CBWeekDays.Checked;
        }

        private void CBWeekUpper_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WeekDays.UpperCase = this.CBWeekUpper.Checked;
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
            this.tabPage1 = new TabPage();
            this.CBNext = new CheckBox();
            this.CBPrevious = new CheckBox();
            this.CBMonthUpper = new CheckBox();
            this.CBMonths = new CheckBox();
            this.CBToday = new CheckBox();
            this.CBTrailing = new CheckBox();
            this.CBWeekUpper = new CheckBox();
            this.CBWeekDays = new CheckBox();
            this.ButtonPen1 = new Button();
            this.tabPage2 = new TabPage();
            this.tabControl2 = new TabControl();
            this.tabPage3 = new TabPage();
            this.tabControl3 = new TabControl();
            this.tabPage4 = new TabPage();
            this.tabControl4 = new TabControl();
            this.tabPage5 = new TabPage();
            this.tabControl5 = new TabControl();
            this.tabPage6 = new TabPage();
            this.tabControl6 = new TabControl();
            this.tabPage7 = new TabPage();
            this.tabControl7 = new TabControl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.AddRange(new Control[] { this.tabPage1, this.tabPage2, this.tabPage3, this.tabPage4, this.tabPage5, this.tabPage6, this.tabPage7 });
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x155, 0xe2);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabPage1.Controls.AddRange(new Control[] { this.CBNext, this.CBPrevious, this.CBMonthUpper, this.CBMonths, this.CBToday, this.CBTrailing, this.CBWeekUpper, this.CBWeekDays, this.ButtonPen1 });
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x14d, 200);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Options";
            this.CBNext.FlatStyle = FlatStyle.Flat;
            this.CBNext.Location = new Point(0xb0, 0x80);
            this.CBNext.Name = "CBNext";
            this.CBNext.Size = new Size(0x88, 0x10);
            this.CBNext.TabIndex = 8;
            this.CBNext.Text = "Show Ne&xt Button";
            this.CBNext.CheckedChanged += new EventHandler(this.CBNext_CheckedChanged);
            this.CBPrevious.FlatStyle = FlatStyle.Flat;
            this.CBPrevious.Location = new Point(0xb0, 0x68);
            this.CBPrevious.Name = "CBPrevious";
            this.CBPrevious.Size = new Size(0x88, 0x10);
            this.CBPrevious.TabIndex = 7;
            this.CBPrevious.Text = "Show Pr&evious Button";
            this.CBPrevious.CheckedChanged += new EventHandler(this.CBPrevious_CheckedChanged);
            this.CBMonthUpper.Checked = true;
            this.CBMonthUpper.CheckState = CheckState.Checked;
            this.CBMonthUpper.FlatStyle = FlatStyle.Flat;
            this.CBMonthUpper.Location = new Point(0xb0, 0x48);
            this.CBMonthUpper.Name = "CBMonthUpper";
            this.CBMonthUpper.Size = new Size(0x80, 0x10);
            this.CBMonthUpper.TabIndex = 6;
            this.CBMonthUpper.Text = "U&ppercase";
            this.CBMonthUpper.CheckedChanged += new EventHandler(this.CBMonthUpper_CheckedChanged);
            this.CBMonths.Checked = true;
            this.CBMonths.CheckState = CheckState.Checked;
            this.CBMonths.FlatStyle = FlatStyle.Flat;
            this.CBMonths.Location = new Point(0xb0, 0x30);
            this.CBMonths.Name = "CBMonths";
            this.CBMonths.Size = new Size(0x80, 0x10);
            this.CBMonths.TabIndex = 5;
            this.CBMonths.Text = "Show &Months";
            this.CBMonths.CheckedChanged += new EventHandler(this.CBMonths_CheckedChanged);
            this.CBToday.Checked = true;
            this.CBToday.CheckState = CheckState.Checked;
            this.CBToday.FlatStyle = FlatStyle.Flat;
            this.CBToday.Location = new Point(0x18, 0x80);
            this.CBToday.Name = "CBToday";
            this.CBToday.Size = new Size(0x60, 0x10);
            this.CBToday.TabIndex = 4;
            this.CBToday.Text = "Show T&oday";
            this.CBToday.CheckedChanged += new EventHandler(this.CBToday_CheckedChanged);
            this.CBTrailing.Checked = true;
            this.CBTrailing.CheckState = CheckState.Checked;
            this.CBTrailing.FlatStyle = FlatStyle.Flat;
            this.CBTrailing.Location = new Point(0x18, 0x68);
            this.CBTrailing.Name = "CBTrailing";
            this.CBTrailing.Size = new Size(0x60, 0x10);
            this.CBTrailing.TabIndex = 3;
            this.CBTrailing.Text = "&Trailing days";
            this.CBTrailing.CheckedChanged += new EventHandler(this.CBTrailing_CheckedChanged);
            this.CBWeekUpper.Checked = true;
            this.CBWeekUpper.CheckState = CheckState.Checked;
            this.CBWeekUpper.FlatStyle = FlatStyle.Flat;
            this.CBWeekUpper.Location = new Point(0x18, 0x48);
            this.CBWeekUpper.Name = "CBWeekUpper";
            this.CBWeekUpper.Size = new Size(0x60, 0x10);
            this.CBWeekUpper.TabIndex = 2;
            this.CBWeekUpper.Text = "&Uppercase";
            this.CBWeekUpper.CheckedChanged += new EventHandler(this.CBWeekUpper_CheckedChanged);
            this.CBWeekDays.Checked = true;
            this.CBWeekDays.CheckState = CheckState.Checked;
            this.CBWeekDays.FlatStyle = FlatStyle.Flat;
            this.CBWeekDays.Location = new Point(0x18, 0x30);
            this.CBWeekDays.Name = "CBWeekDays";
            this.CBWeekDays.Size = new Size(0x70, 0x10);
            this.CBWeekDays.TabIndex = 1;
            this.CBWeekDays.Text = "Show &Weekdays";
            this.CBWeekDays.CheckedChanged += new EventHandler(this.CBWeekDays_CheckedChanged);
            this.ButtonPen1.FlatStyle = FlatStyle.Flat;
            this.ButtonPen1.Location = new Point(0x18, 0x10);
            this.ButtonPen1.Name = "ButtonPen1";
            this.ButtonPen1.TabIndex = 0;
            this.ButtonPen1.Text = "&Lines...";
            this.ButtonPen1.Click += new EventHandler(this.ButtonPen1_Click);
            this.tabPage2.Controls.AddRange(new Control[] { this.tabControl2 });
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(0x14d, 200);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Days";
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x14d, 200);
            this.tabControl2.TabIndex = 0;
            this.tabPage3.Controls.AddRange(new Control[] { this.tabControl3 });
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(0x14d, 200);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Weekdays";
            this.tabControl3.Dock = DockStyle.Fill;
            this.tabControl3.HotTrack = true;
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new Size(0x14d, 200);
            this.tabControl3.TabIndex = 0;
            this.tabPage4.Controls.AddRange(new Control[] { this.tabControl4 });
            this.tabPage4.Location = new Point(4, 0x16);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new Size(0x14d, 200);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Today";
            this.tabControl4.Dock = DockStyle.Fill;
            this.tabControl4.HotTrack = true;
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new Size(0x14d, 200);
            this.tabControl4.TabIndex = 0;
            this.tabPage5.Controls.AddRange(new Control[] { this.tabControl5 });
            this.tabPage5.Location = new Point(4, 0x16);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new Size(0x14d, 200);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Sunday";
            this.tabControl5.Dock = DockStyle.Fill;
            this.tabControl5.HotTrack = true;
            this.tabControl5.Name = "tabControl5";
            this.tabControl5.SelectedIndex = 0;
            this.tabControl5.Size = new Size(0x14d, 200);
            this.tabControl5.TabIndex = 0;
            this.tabPage6.Controls.AddRange(new Control[] { this.tabControl6 });
            this.tabPage6.Location = new Point(4, 0x16);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new Size(0x14d, 200);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Trailing";
            this.tabControl6.Dock = DockStyle.Fill;
            this.tabControl6.HotTrack = true;
            this.tabControl6.Name = "tabControl6";
            this.tabControl6.SelectedIndex = 0;
            this.tabControl6.Size = new Size(0x14d, 200);
            this.tabControl6.TabIndex = 0;
            this.tabPage7.Controls.AddRange(new Control[] { this.tabControl7 });
            this.tabPage7.Location = new Point(4, 0x16);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new Size(0x14d, 200);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "Months";
            this.tabControl7.Dock = DockStyle.Fill;
            this.tabControl7.HotTrack = true;
            this.tabControl7.Name = "tabControl7";
            this.tabControl7.SelectedIndex = 0;
            this.tabControl7.Size = new Size(0x14d, 200);
            this.tabControl7.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x155, 0xe2);
            base.Controls.AddRange(new Control[] { this.tabControl1 });
            base.Name = "CalendarSeries";
            base.Load += new EventHandler(this.CalendarSeries_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControl1.SelectedIndex)
            {
                case 1:
                    this.AddShapeEditor(this.tabPage1, this.tabControl2, this.series.Days);
                    return;

                case 2:
                    this.AddShapeEditor(this.tabPage2, this.tabControl3, this.series.WeekDays);
                    return;

                case 3:
                    this.AddShapeEditor(this.tabPage3, this.tabControl4, this.series.Today);
                    return;

                case 4:
                    this.AddShapeEditor(this.tabPage4, this.tabControl5, this.series.Sunday);
                    return;

                case 5:
                    this.AddShapeEditor(this.tabPage5, this.tabControl6, this.series.Trailing);
                    return;

                case 6:
                    this.AddShapeEditor(this.tabPage6, this.tabControl7, this.series.Months);
                    return;
            }
        }
    }
}

