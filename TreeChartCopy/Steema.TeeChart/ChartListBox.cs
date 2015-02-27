namespace Steema.TeeChart
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ChartListBox), "Images.ChartListBox.bmp")]
    public class ChartListBox : ListBox, ITeeEventListener
    {
        private bool AskDelete;
        internal Steema.TeeChart.Chart chart;
        private Container components;
        private const int defaultItemHeight = 0x18;
        public bool EnableChangeColor;
        public bool EnableChangeType;
        private Section[] Sections;
        private TChart tChart;

        public event EditSeriesEventHandler EditSeries;

        public ChartListBox()
        {
            this.components = null;
            this.AskDelete = true;
            this.EnableChangeColor = true;
            this.EnableChangeType = true;
            this.Sections = new Section[4];
            this.InitializeComponent();
            this.SetProperties();
        }

        public ChartListBox(IContainer container)
        {
            this.components = null;
            this.AskDelete = true;
            this.EnableChangeColor = true;
            this.EnableChangeType = true;
            this.Sections = new Section[4];
            container.Add(this);
            this.InitializeComponent();
            this.SetProperties();
        }

        private bool AnySelected()
        {
            if (!this.MultiSelect)
            {
                return (this.SelectedIndex != -1);
            }
            return (base.SelectedIndices.Count > 0);
        }

        public void ChangeTypeSeries()
        {
            if (this.AnySelected())
            {
                SeriesCollection seriess = new SeriesCollection(this.chart);
                if (this.MultiSelect)
                {
                    for (int k = 0; k < this.Items.Count; k++)
                    {
                        if (this.IsSelected(k))
                        {
                            seriess.Add(this.Series(k));
                        }
                    }
                }
                else
                {
                    seriess.Add(this.SelectedSeries);
                }
                bool flag = true;
                System.Type newType = null;
                for (int i = 0; i < seriess.Count; i++)
                {
                    Steema.TeeChart.Styles.Series s = seriess[i];
                    if (flag)
                    {
                        newType = ChartGallery.ChangeSeriesType(this.chart, ref s);
                        flag = false;
                    }
                    else
                    {
                        Steema.TeeChart.Styles.Series.ChangeType(ref s, newType);
                    }
                    seriess[i] = s;
                }
                for (int j = 0; j < seriess.Count; j++)
                {
                    this.SelectSeries(seriess[j]);
                }
                this.FillSeries(this.SelectedSeries);
                this.DoRefresh();
            }
        }

        public Steema.TeeChart.Styles.Series CloneSeries()
        {
            if (this.SelectedIndex != -1)
            {
                Steema.TeeChart.Styles.Series series = this.SelectedSeries.Clone();
                this.RefreshDesigner();
                return series;
            }
            return null;
        }

        public bool DeleteSeries()
        {
            bool flag = false;
            if (!this.AnySelected())
            {
                return flag;
            }
            if (this.AskDelete)
            {
                string selectedSeries;
                if (!this.MultiSelect || (this.SelCount == 1))
                {
                    selectedSeries = this.SelectedSeries.ToString();
                }
                else
                {
                    selectedSeries = Texts.SelectedSeries;
                }
                if (Utils.YesNoDelete(selectedSeries))
                {
                    this.DoDelete();
                    flag = true;
                }
                return flag;
            }
            this.DoDelete();
            return true;
        }

        private void DoDelete()
        {
            int aIndex = -1;
            if (this.MultiSelect)
            {
                int index = 0;
                while (index < this.Items.Count)
                {
                    if (base.GetSelected(index))
                    {
                        if (aIndex == -1)
                        {
                            aIndex = index;
                        }
                        this.chart[index].Dispose();
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            else if (this.SelectedIndex != -1)
            {
                aIndex = this.SelectedIndex;
                this.chart[this.SelectedIndex].Dispose();
            }
            if (aIndex >= this.Items.Count)
            {
                aIndex = this.Items.Count - 1;
            }
            if (aIndex > -1)
            {
                this.SelectSeries(aIndex);
            }
            if ((this.SelectedIndex == -1) && (this.Items.Count > 0))
            {
                this.SelectSeries(0);
            }
            this.DoRefresh();
            this.RefreshDesigner();
        }

        public void DoRefresh()
        {
        }

        public void FillSeries(Steema.TeeChart.Styles.Series oldSeries)
        {
            this.Items.Clear();
            base.ClearSelected();
            base.BeginUpdate();
            if (this.chart != null)
            {
                this.chart.Series.FillItems(this.Items);
            }
            base.EndUpdate();
            int aIndex = -1;
            if (oldSeries != null)
            {
                aIndex = this.chart.Series.IndexOf(oldSeries);
            }
            if ((aIndex == -1) && (this.Items.Count > 0))
            {
                aIndex = 0;
            }
            if ((aIndex != -1) && (this.Items.Count > aIndex))
            {
                this.SelectSeries(aIndex);
            }
            this.DoRefresh();
            base.Invalidate();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private bool IsSelected(int tmp)
        {
            if (!this.MultiSelect)
            {
                return (this.SelectedIndex == tmp);
            }
            return base.SelectedIndices.Contains(tmp);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            Point point;
            int tmp = this.SeriesAtMousePos(out point);
            if ((tmp != -1) && this.IsSelected(tmp))
            {
                Steema.TeeChart.Styles.Series s = this.Series(tmp);
                if (this.PointInSection(point, 0) && this.EnableChangeType)
                {
                    this.ChangeTypeSeries();
                    return;
                }
                if (this.PointInSection(point, 2) && this.EnableChangeColor)
                {
                    if (s.UseSeriesColor && BrushEditor.Edit(s.bBrush, false))
                    {
                        s.Color = s.bBrush.Color;
                        s.ColorEach = false;
                        s.Invalidate();
                        base.Invalidate();
                    }
                    return;
                }
                if (this.PointInSection(point, 3) && (this.EditSeries != null))
                {
                    this.EditSeries(this, s);
                    return;
                }
            }
            base.OnDoubleClick(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush brush = new SolidBrush(this.BackColor);
            Rectangle rect = new Rectangle(e.Bounds.Location, e.Bounds.Size);
            rect.Width = this.SectionLeft(3);
            e.Graphics.FillRectangle(brush, rect);
            brush.Color = e.BackColor;
            rect = new Rectangle(e.Bounds.Location, e.Bounds.Size);
            rect.X = this.SectionLeft(4);
            e.Graphics.FillRectangle(brush, rect);
            if (((e.Index != -1) && (this.chart != null)) && (e.Index < this.chart.Series.Count))
            {
                int num;
                Steema.TeeChart.Styles.Series series = this.chart[e.Index];
                if ((series != null) && this.ShowSeriesIcon)
                {
                    Image bitmapEditor = series.GetBitmapEditor();
                    if (bitmapEditor != null)
                    {
                        e.Graphics.DrawImage(bitmapEditor, this.SectionLeft(0), e.Bounds.Top);
                        bitmapEditor.Dispose();
                    }
                }
                if (this.ShowSeriesColor && series.UseSeriesColor)
                {
                    num = this.SectionLeft(2) - 2;
                    rect = new Rectangle(num, e.Bounds.Top, this.Sections[2].Width, e.Bounds.Height);
                    rect.Inflate(-4, -4);
                    series.PaintLegend(e.Graphics, rect);
                }
                if (this.ShowActiveCheck)
                {
                    num = this.SectionLeft(1);
                    Rectangle rectangle2 = new Rectangle(num + 2, e.Bounds.Top + 5, 10, 12);
                    Utils.DrawCheckBox(rectangle2.X, rectangle2.Y, e.Graphics, series.Active, e.BackColor);
                }
                if (this.ShowSeriesTitle)
                {
                    Point point = new Point(this.SectionLeft(3) + 1, e.Bounds.Top + ((this.ItemHeight - this.Font.Height) / 2));
                    string s = this.Items[e.Index].ToString().Replace('\n', ' ');
                    e.Graphics.DrawString(s, this.Font, new SolidBrush(e.ForeColor), (PointF) point);
                }
                else
                {
                    e.Graphics.DrawString("", this.Font, brush, (PointF) new Point(0, 0));
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (base.SelectedIndices.Count > 0)
            {
                Point point;
                int num = this.SeriesAtMousePos(out point);
                if ((num != -1) && this.PointInSection(point, 1))
                {
                    this.chart[num].Active = !this.chart[num].Active;
                    if (this.chart[num].chart == null)
                    {
                        base.Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point point;
            base.OnMouseMove(e);
            if ((this.SeriesAtMousePos(out point) != -1) && (this.PointInSection(point, 0) || this.PointInSection(point, 2)))
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool PointInSection(Point p, int ASection)
        {
            if (!this.Sections[ASection].Visible)
            {
                return false;
            }
            int num = this.SectionLeft(ASection);
            return ((p.X > num) && (p.X < (num + this.Sections[ASection].Width)));
        }

        private void RefreshDesigner()
        {
        }

        private int SectionLeft(int ASection)
        {
            int num = 0;
            for (int i = 0; i < ASection; i++)
            {
                if (this.Sections[i].Visible)
                {
                    num += this.Sections[i].Width;
                }
            }
            return num;
        }

        public void SelectAll()
        {
            if (this.MultiSelect)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    base.SetSelected(i, true);
                }
                this.RefreshDesigner();
            }
        }

        private void SelectSeries(Steema.TeeChart.Styles.Series s)
        {
            this.SelectSeries(this.chart.Series.IndexOf(s));
        }

        private void SelectSeries(int AIndex)
        {
            if (this.MultiSelect)
            {
                base.SetSelected(AIndex, true);
            }
            else
            {
                this.SelectedIndex = AIndex;
            }
        }

        public Steema.TeeChart.Styles.Series Series(int index)
        {
            return this.chart[index];
        }

        private int SeriesAtMousePos(out Point p)
        {
            p = base.PointToClient(Control.MousePosition);
            return base.IndexFromPoint(p);
        }

        internal void SetChart(Steema.TeeChart.Chart c)
        {
            if (this.chart != null)
            {
                this.chart.RemoveListener(this);
            }
            this.chart = c;
            if (this.chart == null)
            {
                this.Items.Clear();
            }
            else
            {
                if (this.chart.Listeners != null)
                {
                    this.chart.Listeners.Add(this);
                }
                this.FillSeries(null);
            }
        }

        private void SetProperties()
        {
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ItemHeight = 0x18;
            base.Sorted = false;
            this.MultiSelect = true;
            this.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Sections[0].Width = 0x1a;
            this.Sections[0].Visible = true;
            this.Sections[1].Width = 0x10;
            this.Sections[1].Visible = true;
            this.Sections[2].Width = 0x1a;
            this.Sections[2].Visible = true;
            this.Sections[3].Width = 0xd8;
            this.Sections[3].Visible = true;
        }

        public void TeeEvent(Steema.TeeChart.TeeEvent e)
        {
            if (e is SeriesEvent)
            {
                Steema.TeeChart.Styles.Series series = (e as SeriesEvent).Series;
                switch ((e as SeriesEvent).Event)
                {
                    case SeriesEventStyle.Add:
                    case SeriesEventStyle.Swap:
                        this.FillSeries(this.SelectedSeries);
                        return;

                    case SeriesEventStyle.Remove:
                    {
                        int index = this.Items.IndexOf(series);
                        if (index != -1)
                        {
                            this.Items.RemoveAt(index);
                        }
                        return;
                    }
                    case SeriesEventStyle.RemoveAll:
                        this.FillSeries(null);
                        return;

                    case SeriesEventStyle.ChangeTitle:
                    case SeriesEventStyle.ChangeColor:
                    case SeriesEventStyle.ChangeActive:
                        base.Invalidate();
                        return;

                    default:
                        return;
                }
            }
        }

        [DefaultValue((string) null)]
        public TChart Chart
        {
            get
            {
                return this.tChart;
            }
            set
            {
                this.tChart = value;
                if (this.tChart != null)
                {
                    this.SetChart(this.tChart.Chart);
                }
                else
                {
                    this.SetChart(null);
                }
            }
        }

        [DefaultValue(1), Browsable(false)]
        public System.Windows.Forms.DrawMode DrawMode
        {
            get
            {
                return this.DrawMode;
            }
            set
            {
                this.DrawMode = value;
            }
        }

        public Steema.TeeChart.Styles.Series this[int index]
        {
            get
            {
                return this.chart[index];
            }
            set
            {
                this.chart[index] = value;
            }
        }

        [DefaultValue(0x18)]
        public int ItemHeight
        {
            get
            {
                return this.ItemHeight;
            }
            set
            {
                this.ItemHeight = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ListBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        [DefaultValue(true)]
        public bool MultiSelect
        {
            get
            {
                if (this.SelectionMode != System.Windows.Forms.SelectionMode.MultiSimple)
                {
                    return (this.SelectionMode == System.Windows.Forms.SelectionMode.MultiExtended);
                }
                return true;
            }
            set
            {
                if (value)
                {
                    this.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
                }
                else
                {
                    this.SelectionMode = System.Windows.Forms.SelectionMode.One;
                }
            }
        }

        private int SelCount
        {
            get
            {
                return base.SelectedIndices.Count;
            }
        }

        [Browsable(false), Description("Shows the SeriesList index value of the selected Series."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Styles.Series SelectedSeries
        {
            get
            {
                if (this.SelectedIndex != -1)
                {
                    return this.chart[this.SelectedIndex];
                }
                return null;
            }
            set
            {
                base.ClearSelected();
                int index = this.chart.Series.IndexOf(value);
                base.SetSelected(index, true);
            }
        }

        [DefaultValue(3)]
        public System.Windows.Forms.SelectionMode SelectionMode
        {
            get
            {
                return this.SelectionMode;
            }
            set
            {
                this.SelectionMode = value;
            }
        }

        [DefaultValue(true), Description("Displays/Hides the 'Active Series' CheckBox in the Series List.")]
        public bool ShowActiveCheck
        {
            get
            {
                return this.Sections[1].Visible;
            }
            set
            {
                this.Sections[1].Visible = value;
                base.Invalidate();
            }
        }

        [DefaultValue(true), Description("Displays/Hides the 'Series Colour' box in the Series List.")]
        public bool ShowSeriesColor
        {
            get
            {
                return this.Sections[2].Visible;
            }
            set
            {
                this.Sections[2].Visible = value;
                base.Invalidate();
            }
        }

        [DefaultValue(true), Description("Displays/Hides the 'Series type icon' in the Series List.")]
        public bool ShowSeriesIcon
        {
            get
            {
                return this.Sections[0].Visible;
            }
            set
            {
                this.Sections[0].Visible = value;
                base.Invalidate();
            }
        }

        [Description("Displays/Hides the 'Series Title' in the Series List."), DefaultValue(true)]
        public bool ShowSeriesTitle
        {
            get
            {
                return this.Sections[3].Visible;
            }
            set
            {
                this.Sections[3].Visible = value;
                base.Invalidate();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Section
        {
            public int Width;
            public bool Visible;
        }
    }
}

