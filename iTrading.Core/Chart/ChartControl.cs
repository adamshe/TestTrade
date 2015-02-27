using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using iTrading.Core.Data;
using iTrading.Core.IndicatorBase;
using Steema.TeeChart;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using iTrading.Core.Kernel;
using iTrading.Core.Data;

namespace iTrading.Core.Chart
{
    /// <summary>
    /// </summary>
    public class ChartControl : UserControl
    {
        private bool activatedRegistered = false;
        private BarUpdateEventHandler barUpdateEventHandler = null;
        private Candle candleStyles;
        private const int candleStylesOffset = 0;
        private Hashtable chart2Indicators = new Hashtable();
        private Hashtable chart2Levels = new Hashtable();
        private IContainer components = null;
        private TextBox dummyTextBox;
        private int dummyValues = 0;
        private int first = -1;
        private HScrollBar hScrollBar;
        private ArrayList indicatorCharts = new ArrayList();
        private IndicatorCollection indicators = new IndicatorCollection();
        private bool isActive = false;
        private int last = -1;
        private int lookbackDailyData = 300;
        private const int maxCandleWidth = 20;
        private ArrayList midnight = new ArrayList();
        private const int mouseWheelAccelerator = 10;
        private ArrayList newChartMenuItems = new ArrayList();
        private int points = 0;
        private Quotes quotes = null;
        private Hashtable series2IndicatorSeries = new Hashtable();
        private double splitRatio = 0.66666666666666663;
        private Splitter splitter;
        private TChart teeChart;

        /// <summary>
        /// </summary>
        public ChartControl()
        {
            this.InitializeComponent();
            this.dummyTextBox.MouseWheel += new MouseEventHandler(this.dummyTextBox_MouseWheel);
            //    throw new TMException(ErrorCode.Panic, "This feature is not yet supported");
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            this.Register(((IndicatorSelectForm) ((Button) sender).Parent).Indicators);
        }

        internal void Arrange()
        {
            if ((this.quotes != null) && (this.quotes.Bars.Count != 0))
            {
                this.teeChart.AutoRepaint = false;
                foreach (TChart chart in this.indicatorCharts)
                {
                    chart.AutoRepaint = false;
                }
                this.teeChart.Axes.Bottom.MaximumOffset = (int) (1.5 * this.candleStyles.CandleWidth);
                this.teeChart.Axes.Bottom.MinimumOffset = (int) (1.5 * this.candleStyles.CandleWidth);
                foreach (TChart chart2 in indicatorCharts)
                {
                    chart2.Axes.Bottom.MaximumOffset = (int) (1.5 * this.candleStyles.CandleWidth);
                    chart2.Axes.Bottom.MinimumOffset = (int) (1.5 * this.candleStyles.CandleWidth);
                }
                this.points = (int) ((((base.Width - this.teeChart.Chart.Panel.MarginRight) - this.teeChart.Chart.Panel.MarginLeft) - (3.0 * this.candleStyles.CandleWidth)) / (1.5 * this.candleStyles.CandleWidth));
                if ((((this.last - this.points) + 1) < 0) && ((this.quotes.Bars.Count + this.dummyValues) > this.points))
                {
                    this.first = 0;
                    this.last = this.points - 1;
                }
                else
                {
                    first = Math.Max((this.last - this.points) + 1, 0);
                }
                this.midnight.Clear();
                double minValue = double.MinValue;
                double maxValue = double.MaxValue;
                for (int i = Math.Max(this.first, this.dummyValues); i <= this.last; i++)
                {
                    if (((this.quotes.Period.Id != PeriodTypeId.Day) && ((i - this.dummyValues) > this.first)) && (this.quotes.Bars[i - this.dummyValues].Time.Date > this.quotes.Bars[(i - this.dummyValues) - 1].Time.Date))
                    {
                        this.midnight.Add(i);
                    }
                    else if (((this.quotes.Period.Id == PeriodTypeId.Day) && (i > (this.first + this.dummyValues))) && (this.quotes.Bars[i - this.dummyValues].Time.Month > this.quotes.Bars[(i - this.dummyValues) - 1].Time.Month))
                    {
                        this.midnight.Add(i);
                    }
                    minValue = Math.Max(minValue, this.quotes.Bars[i - this.dummyValues].High);
                    maxValue = Math.Min(maxValue, this.quotes.Bars[i - this.dummyValues].Low);
                }
                if (minValue != double.MinValue)
                {
                    this.teeChart.Axes.Bottom.SetMinMax((double) this.first, (double) this.last);
                    this.teeChart.Axes.Right.SetMinMax(maxValue, minValue);
                    foreach (Steema.TeeChart.Chart chart3 in this.chart2Indicators.Keys)
                    {
                        double num4 = (chart3 == this.teeChart.Chart) ? minValue : double.MinValue;
                        double num5 = (chart3 == this.teeChart.Chart) ? maxValue : double.MaxValue;
                        foreach (IndicatorBase.IndicatorBase base2 in (ArrayList) this.chart2Indicators[chart3])
                        {
                            foreach (IndicatorSeries series in base2.resultSeries)
                            {
                                for (int j = Math.Max(this.first, this.dummyValues); j <= this.last; j++)
                                {
                                    num4 = Math.Max(num4, series[j - this.dummyValues]);
                                    num5 = Math.Min(num5, series[j - this.dummyValues]);
                                }
                            }
                        }
                        chart3.Axes.Bottom.SetMinMax((double) this.first, (double) this.last);
                        chart3.Axes.Right.SetMinMax(num5, num4);
                    }
                }
                this.teeChart.AutoRepaint = true;
                foreach (TChart chart4 in this.indicatorCharts)
                {
                    chart4.AutoRepaint = true;
                }
                if (!this.activatedRegistered && (this.Form != null))
                {
                    this.Form.Activated += new EventHandler(this.Form_Activated);
                    this.Form.Deactivate += new EventHandler(this.Form_Deactivate);
                    this.activatedRegistered = true;
                }
                this.hScrollBar.LargeChange = this.points;
                this.hScrollBar.Maximum = (this.quotes.Bars.Count + this.dummyValues) - 1;
                this.hScrollBar.Minimum = 0;
                this.hScrollBar.SmallChange = 1;
                this.hScrollBar.Value = Math.Max(0, (this.last - this.points) + 1);
                if (this.isActive)
                {
                    this.dummyTextBox.Focus();
                }
            }
        }

        private void ChartControl_Resize(object sender, EventArgs e)
        {
            this.ResizeControls();
        }

        private void CreateChartSeries(IndicatorBase.IndicatorBase indicator, TChart chart)
        {
            Trace.Assert(indicator.ChartSeries != null, "Chart.ChartControl.CreateSeries: Indicator.IndicatorBase.ChartSeries can not be NULL");
            for (int i = 0; i < indicator.CountSeries; i++)
            {
                Series s = null;
                if (i < indicator.ChartSeries.Count)
                {
                    s = indicator.ChartSeries[i];
                }
                else
                {
                    Line series = new Line();
                    series.Brush.Color = Color.DarkViolet;
                    indicator.ChartSeries.Add(series);
                    s = series;
                }
                s.HorizAxis = this.candleStyles.HorizAxis;
                s.VertAxis = this.candleStyles.VertAxis;
                chart.Series.Add(s);
                this.series2IndicatorSeries.Add(s, indicator.resultSeries[i]);
            }
            ArrayList list = null;
            list = (ArrayList) this.chart2Indicators[chart.Chart];
            if (list == null)
            {
                list = new ArrayList();
                this.chart2Indicators[chart.Chart] = list;
            }
            list.Add(indicator);
            LevelCollection levels = null;
            levels = (LevelCollection) this.chart2Levels[chart.Chart];
            if (levels == null)
            {
                levels = new LevelCollection();
                this.chart2Levels[chart.Chart] = levels;
            }
            foreach (Level level in indicator.Levels)
            {
                levels.Add(level);
            }
        }

        /// <summary> 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dummyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = false;
            if ((e.KeyCode == Keys.Left) && (this.last > (this.points - 1)))
            {
                this.last--;
                flag = true;
            }
            else if ((e.KeyCode == Keys.Right) && (this.last < ((this.quotes.Bars.Count + this.dummyValues) - 1)))
            {
                this.last++;
                flag = true;
            }
            else if ((e.KeyCode == Keys.Next) && (this.last > (this.points - 1)))
            {
                this.last = Math.Max((int) (this.points - 1), (int) ((this.last - this.points) + 1));
                flag = true;
            }
            else if ((e.KeyCode == Keys.Prior) && (this.last < ((this.quotes.Bars.Count + this.dummyValues) - 1)))
            {
                this.last = Math.Min((int) ((this.quotes.Bars.Count + this.dummyValues) - 1), (int) ((this.last + this.points) - 1));
                flag = true;
            }
            else if (e.KeyCode == Keys.Home)
            {
                this.last = 0;
                flag = true;
            }
            else if (e.KeyCode == Keys.End)
            {
                this.last = (this.quotes.Bars.Count + this.dummyValues) - 1;
                flag = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                this.NextDay();
                flag = true;
            }
            else if (e.KeyCode == Keys.F4)
            {
                this.PreviousDay();
                flag = true;
            }
            else if ((e.KeyCode == Keys.Down) && (this.candleStyles.CandleWidth < 20))
            {
                this.candleStyles.CandleWidth++;
                flag = true;
            }
            else if ((e.KeyCode == Keys.Up) && (this.candleStyles.CandleWidth > 2))
            {
                this.candleStyles.CandleWidth--;
                flag = true;
            }
            if (flag)
            {
                this.Arrange();
                this.Refresh();
            }
        }

        private void dummyTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            bool flag = false;
            int num = (Math.Abs(e.Delta) / 120) * (((Control.ModifierKeys & Keys.Control) == Keys.Control) ? (this.points / 10) : 1);
            if ((e.Delta > 0) && (this.last < ((this.quotes.Bars.Count + this.dummyValues) - 1)))
            {
                this.last = Math.Min((int) ((this.quotes.Bars.Count + this.dummyValues) - 1), (int) (this.last + num));
                flag = true;
            }
            else if ((e.Delta < 0) && (this.last > (this.points - 1)))
            {
                this.last = Math.Max((int) (this.points - 1), (int) (this.last - num));
                flag = true;
            }
            if (flag)
            {
                this.Arrange();
                this.Refresh();
            }
        }

        private void FillSeries()
        {
            this.teeChart.AutoRepaint = false;
            foreach (TChart chart in this.indicatorCharts)
            {
                chart.AutoRepaint = false;
            }
            this.candleStyles.Clear();
            for (int i = 0; i < this.quotes.Bars.Count; i++)
            {
                this.candleStyles.Add(this.quotes.Bars[i].Open, this.quotes.Bars[i].High, this.quotes.Bars[i].Low, this.quotes.Bars[i].Close);
            }
            foreach (Series series in this.series2IndicatorSeries.Keys)
            {
                series.Clear();
                IndicatorSeries series2 = (IndicatorSeries) this.series2IndicatorSeries[series];
                for (int j = 0; j < this.quotes.Bars.Count; j++)
                {
                    series.Add(series2[j]);
                }
            }
            if (this.candleStyles.Count == 0)
            {
                this.dummyValues = 1;
                this.candleStyles.Add();
                foreach (Series series3 in this.series2IndicatorSeries.Keys)
                {
                    series3.Add();
                }
            }
            this.teeChart.AutoRepaint = true;
            foreach (TChart chart2 in this.indicatorCharts)
            {
                chart2.AutoRepaint = true;
            }
        }

        private void Form_Activated(object sender, EventArgs e)
        {
            this.isActive = true;
        }

        private void Form_Deactivate(object sender, EventArgs e)
        {
            this.isActive = false;
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if ((this.quotes != null) && (((this.last - this.points) + 1) != e.NewValue))
            {
                this.last = Math.Min((int) ((this.quotes.Bars.Count + this.dummyValues) - 1), (int) ((e.NewValue + this.points) - 1));
                this.Arrange();
                this.Refresh();
            }
        }

        private void InitializeComponent()
        {
            this.teeChart = new TChart();
            this.candleStyles = new Candle();
            this.dummyTextBox = new TextBox();
            this.hScrollBar = new HScrollBar();
            this.splitter = new Splitter();
            base.SuspendLayout();
            this.teeChart.Aspect.View3D = false;
            this.teeChart.Axes.Bottom.AxisPen.Width = 1;
            this.teeChart.Axes.Bottom.Grid.Color = Color.FromArgb(0xfe, 0xa9, 0xa9, 0xa9);
            this.teeChart.Axes.Bottom.Grid.Visible = false;
            this.teeChart.Axes.Bottom.Labels.DateTimeFormat = "dd.MM.yyyy";
            this.teeChart.Axes.Left.Labels.Style = AxisLabelStyle.Text;
            this.teeChart.Axes.Right.AxisPen.Width = 1;
            this.teeChart.Axes.Right.Labels.CustomSize = 50;
            this.teeChart.Cursor = Cursors.Default;
            this.teeChart.Dock = DockStyle.Top;
            this.teeChart.Header.Lines = new string[0];
            this.teeChart.Legend.Shadow.Visible = false;
            this.teeChart.Legend.Visible = false;
            this.teeChart.Location = new Point(0, 0);
            this.teeChart.Name = "teeChart";
            this.teeChart.Panel.Brush.Gradient.EndColor = Color.FromArgb(0xfe, 0xb3, 0xb3, 0xff);
            this.teeChart.Panel.Brush.Gradient.GammaCorrection = true;
            this.teeChart.Panel.Brush.Gradient.MiddleColor = Color.FromArgb(0xfe, 0xe0, 0xe0, 0xe0);
            this.teeChart.Panel.Brush.Gradient.SigmaFocus = 1f;
            this.teeChart.Panel.Brush.Gradient.StartColor = Color.FromArgb(0xfe, 0xff, 0xff, 0xff);
            this.teeChart.Panel.Brush.Gradient.UseMiddle = true;
            this.teeChart.Panel.Brush.Gradient.Visible = true;
            this.teeChart.Panel.Gradient.EndColor = Color.FromArgb(0xfe, 0xb3, 0xb3, 0xff);
            this.teeChart.Panel.Gradient.GammaCorrection = true;
            this.teeChart.Panel.Gradient.MiddleColor = Color.FromArgb(0xfe, 0xe0, 0xe0, 0xe0);
            this.teeChart.Panel.Gradient.SigmaFocus = 1f;
            this.teeChart.Panel.Gradient.StartColor = Color.FromArgb(0xfe, 0xff, 0xff, 0xff);
            this.teeChart.Panel.Gradient.UseMiddle = true;
            this.teeChart.Panel.Gradient.Visible = true;
            this.teeChart.Panel.MarginBottom = 0.0;
            this.teeChart.Panel.MarginLeft = 0.0;
            this.teeChart.Panel.MarginRight = 0.0;
            this.teeChart.Panel.MarginTop = 0.0;
            this.teeChart.Panel.MarginUnits = PanelMarginUnits.Pixels;
            this.teeChart.Panel.Pen.Width = 0;
            this.teeChart.Series.Add(this.candleStyles);
            this.teeChart.Size = new Size(0x358, 400);
            this.teeChart.TabIndex = 0;
            this.teeChart.Walls.Back.Visible = false;
            this.teeChart.Walls.Left.Visible = false;
            this.teeChart.MouseDown += new MouseEventHandler(this.teeChart_MouseDown);
            this.teeChart.SizeChanged += new EventHandler(this.teeChart_SizeChanged);
            this.teeChart.GetAxisLabel += new GetAxisLabelEventHandler(this.teeChart_GetAxisLabel);
            this.teeChart.AfterDraw += new PaintChartEventHandler(this.teeChart_AfterDraw);
            this.candleStyles.Brush.Color = Color.Red;
            this.candleStyles.CloseValues = this.candleStyles.YValues;
            this.candleStyles.DateValues = this.candleStyles.XValues;
            this.candleStyles.Pointer.Brush.Color = Color.FromArgb(0xfe, 0, 0, 0);
            this.candleStyles.Pointer.Draw3D = false;
            this.candleStyles.Pointer.Style = PointerStyles.Rectangle;
            this.candleStyles.Title = "candle1";
            this.candleStyles.VertAxis = VerticalAxis.Right;
            this.candleStyles.XValues.DateTime = true;
            this.candleStyles.XValues.Order = ValueListOrder.Ascending;
            this.candleStyles.YValues.Order = ValueListOrder.Ascending;
            this.dummyTextBox.Location = new Point(-100, -100);
            this.dummyTextBox.Name = "dummyTextBox";
            this.dummyTextBox.TabIndex = 1;
            this.dummyTextBox.Text = "";
            this.dummyTextBox.KeyDown += new KeyEventHandler(this.dummyTextBox_KeyDown);
            this.hScrollBar.Dock = DockStyle.Bottom;
            this.hScrollBar.Location = new Point(0, 0x228);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new Size(0x358, 0x18);
            this.hScrollBar.TabIndex = 3;
            this.hScrollBar.Scroll += new ScrollEventHandler(this.hScrollBar_Scroll);
            this.splitter.BorderStyle = BorderStyle.FixedSingle;
            this.splitter.Dock = DockStyle.Top;
            this.splitter.Location = new Point(0, 400);
            this.splitter.Name = "splitter";
            this.splitter.Size = new Size(0x358, 4);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            this.splitter.Visible = false;
            this.splitter.SplitterMoved += new SplitterEventHandler(this.splitter_SplitterMoved);
            base.Controls.Add(this.splitter);
            base.Controls.Add(this.hScrollBar);
            base.Controls.Add(this.dummyTextBox);
            base.Controls.Add(this.teeChart);
            base.Name = "ChartControl";
            base.Size = new Size(0x358, 0x240);
            base.Resize += new EventHandler(this.ChartControl_Resize);
            base.ResumeLayout(false);
        }

        private void MenuItemEvt(object sender, EventArgs e)
        {
            bool flag = false;
            string text = ((MenuItem) sender).Text;
            if ((text == "Previous bar") && (this.last > (this.points - 1)))
            {
                this.last--;
                flag = true;
            }
            else if ((text == "Next bar") && (this.last < ((this.quotes.Bars.Count + this.dummyValues) - 1)))
            {
                this.last++;
                flag = true;
            }
            else if ((text == "Previous page") && (this.last > (this.points - 1)))
            {
                this.last = Math.Max((int) (this.points - 1), (int) ((this.last - this.points) + 1));
                flag = true;
            }
            else if ((text == "Next page") && (this.last < ((this.quotes.Bars.Count + this.dummyValues) - 1)))
            {
                this.last = Math.Min((int) ((this.quotes.Bars.Count + this.dummyValues) - 1), (int) ((this.last + this.points) - 1));
                flag = true;
            }
            else if ((text == "Zoom in") && (this.candleStyles.CandleWidth < 20))
            {
                this.candleStyles.CandleWidth++;
                flag = true;
            }
            else if ((text == "Zoom out") && (this.candleStyles.CandleWidth > 2))
            {
                this.candleStyles.CandleWidth--;
                flag = true;
            }
            else if (text == "Next day")
            {
                this.NextDay();
                flag = true;
            }
            else if (text == "Previous day")
            {
                this.PreviousDay();
                flag = true;
            }
            else if (text == "Indicators ...")
            {
                IndicatorSelectForm form = new IndicatorSelectForm();
                form.applyButton.Click += new EventHandler(this.applyButton_Click);
                form.Indicators = (IndicatorCollection) this.indicators.Clone();
                form.Quotes = this.quotes;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    this.Register(form.Indicators);
                }
            }
            if (flag)
            {
                this.Arrange();
                this.Refresh();
            }
        }

        private void NewChartEvt(object sender, EventArgs e)
        {
            Period newPeriod = null;
            string text = ((MenuItem) sender).Text;
            foreach (Period period2 in this.newChartMenuItems)
            {
                if ((period2 != null) && (period2.ToString() == ((MenuItem) sender).Text))
                {
                    newPeriod = period2;
                    break;
                }
            }
            Trace.Assert(newPeriod != null, "Chart.ChartControl.NewChartEvt: " + ((MenuItem) sender).Text);
            if ((newPeriod.Id == PeriodTypeId.Day) && (newPeriod.Value == 1))
            {
                this.quotes.Symbol.Connection.Bar += new BarUpdateEventHandler(this.ReceiveQuotes);
                this.quotes.Symbol.RequestQuotes(this.quotes.To.AddDays((double) -this.lookbackDailyData), this.quotes.To, new Period(PeriodTypeId.Day, 1), false, LookupPolicyId.RepositoryAndProvider, this);
            }
            else
            {
                Quotes quotes = this.quotes.Copy(newPeriod);
                ChartForm form = new ChartForm();
                form.MdiParent = this.AppWindow;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ChartControl.Quotes = quotes;
                form.Show();
            }
        }

        private void NextDay()
        {
            if (((this.quotes.Period.Id != PeriodTypeId.Day) && (this.first >= 0)) && ((this.first - this.dummyValues) < this.quotes.Bars.Count))
            {
                DateTime date = this.quotes.Bars[this.first].Time.Date;
                for (int i = Math.Max(this.first, this.dummyValues); i < (this.quotes.Bars.Count + this.dummyValues); i++)
                {
                    if (this.quotes.Bars[i - this.dummyValues].Time.Date != date)
                    {
                        this.first = Math.Min(i, ((this.quotes.Bars.Count + this.dummyValues) - this.points) + 1);
                        this.last = (this.first + this.points) - 1;
                        return;
                    }
                }
            }
        }

        private void PreviousDay()
        {
            if (((this.quotes.Period.Id != PeriodTypeId.Day) && (this.first >= 0)) && ((this.first - this.dummyValues) < this.quotes.Bars.Count))
            {
                DateTime date = this.quotes.Bars[this.first - this.dummyValues].Time.Date;
                int first = this.first;
                DateTime time2 = date;
                while (first >= 0)
                {
                    time2 = this.quotes.Bars[first - this.dummyValues].Time.Date;
                    if (time2 != date)
                    {
                        break;
                    }
                    first--;
                }
                while (first >= 0)
                {
                    if ((first == 0) || (this.quotes.Bars[first].Time.Date < time2))
                    {
                        this.first = first + ((first == 0) ? 0 : 1);
                        this.last = (this.first + this.points) - 1;
                        return;
                    }
                    first--;
                }
            }
        }

        private void ReceiveQuotes(object sender, BarUpdateEventArgs e)
        {
            if (e.Quotes.CustomLink == this)
            {
                e.Quotes.Symbol.Connection.Bar -= new BarUpdateEventHandler(this.ReceiveQuotes);
                if (e.Error != ErrorCode.NoError)
                {
                    this.quotes.Symbol.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.quotes.Symbol.connection, e.Error, e.NativeError, "Error on requesting quotes"));
                }
                else
                {
                    ChartForm form = new ChartForm();
                    form.MdiParent = this.AppWindow;
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ChartControl.Quotes = e.Quotes;
                    form.Show();
                }
            }
        }

        internal void Register(IndicatorCollection newIndicators)
        {
            foreach (TChart chart in this.indicatorCharts)
            {
                base.Controls.Remove(chart);
            }
            ArrayList list = new ArrayList();
            foreach (Series series in this.series2IndicatorSeries.Keys)
            {
                foreach (Series series2 in this.teeChart.Series)
                {
                    if (series2 == series)
                    {
                        list.Add(series);
                    }
                }
            }
            foreach (Series series3 in list)
            {
                this.teeChart.Series.Remove(series3);
            }
            this.indicators.Clear();
            foreach (IndicatorBase.IndicatorBase base2 in newIndicators)
            {
                this.indicators.Add(base2.CloneWithQuotes(this.quotes));
            }
            this.indicatorCharts.Clear();
            this.chart2Indicators.Clear();
            this.chart2Levels.Clear();
            this.series2IndicatorSeries.Clear();
            if (this.indicators.Count > 0)
            {
                int num = 0;
                foreach (IndicatorBase.IndicatorBase base3 in this.indicators)
                {
                    if (!base3.IsPriceIndicator)
                    {
                        num++;
                    }
                    else
                    {
                        this.CreateChartSeries(base3, this.teeChart);
                    }
                }
                if (num > 0)
                {
                    this.splitRatio = 2.0 / ((double) (num + 2));
                    this.splitter.Visible = true;
                    this.splitter.SplitPosition = (int) (this.splitRatio * base.ClientSize.Height);
                    this.splitter.TabIndex = 1;
                    int num2 = 0;
                    foreach (IndicatorBase.IndicatorBase base4 in this.indicators)
                    {
                        if (!base4.IsPriceIndicator)
                        {
                            TChart chart2 = new TChart();
                            chart2.AfterDraw += new PaintChartEventHandler(this.teeChart_AfterDraw);
                            chart2.Aspect.View3D = false;
                            chart2.Axes.Bottom.Automatic = false;
                            chart2.Axes.Bottom.MaximumOffset = this.teeChart.Axes.Bottom.MaximumOffset;
                            chart2.Axes.Bottom.MinimumOffset = this.teeChart.Axes.Bottom.MinimumOffset;
                            chart2.Axes.Bottom.Visible = false;
                            chart2.Axes.Right.AxisPen.Width = this.teeChart.Axes.Right.AxisPen.Width;
                            chart2.Axes.Right.Automatic = false;
                            chart2.Axes.Right.Labels.CustomSize = this.teeChart.Axes.Right.Labels.CustomSize;
                            chart2.Axes.Right.MaximumOffset = this.teeChart.Axes.Right.MaximumOffset;
                            chart2.Axes.Right.MinimumOffset = this.teeChart.Axes.Right.MinimumOffset;
                            chart2.Cursor = Cursors.Default;
                            chart2.Header.Lines = new string[0];
                            chart2.Legend.Shadow.Visible = false;
                            chart2.Legend.Visible = false;
                            chart2.Location = new Point(0, 0);
                            chart2.MouseDown += new MouseEventHandler(this.teeChart_MouseDown);
                            chart2.Name = "chart" + num2++;
                            chart2.TabIndex = base.Controls.Count + 1;
                            chart2.Panel.Brush.Gradient.EndColor = this.teeChart.Panel.Brush.Gradient.EndColor;
                            chart2.Panel.Brush.Gradient.GammaCorrection = this.teeChart.Panel.Brush.Gradient.GammaCorrection;
                            chart2.Panel.Brush.Gradient.MiddleColor = this.teeChart.Panel.Brush.Gradient.MiddleColor;
                            chart2.Panel.Brush.Gradient.SigmaFocus = this.teeChart.Panel.Brush.Gradient.SigmaFocus;
                            chart2.Panel.Brush.Gradient.StartColor = this.teeChart.Panel.Brush.Gradient.StartColor;
                            chart2.Panel.Brush.Gradient.UseMiddle = this.teeChart.Panel.Brush.Gradient.UseMiddle;
                            chart2.Panel.Brush.Gradient.Visible = this.teeChart.Panel.Brush.Gradient.Visible;
                            chart2.Panel.Gradient.EndColor = this.teeChart.Panel.Gradient.EndColor;
                            chart2.Panel.Gradient.GammaCorrection = this.teeChart.Panel.Gradient.GammaCorrection;
                            chart2.Panel.Gradient.MiddleColor = this.teeChart.Panel.Gradient.MiddleColor;
                            chart2.Panel.Gradient.SigmaFocus = this.teeChart.Panel.Gradient.SigmaFocus;
                            chart2.Panel.Gradient.StartColor = this.teeChart.Panel.Gradient.StartColor;
                            chart2.Panel.Gradient.UseMiddle = this.teeChart.Panel.Gradient.UseMiddle;
                            chart2.Panel.Gradient.Visible = this.teeChart.Panel.Gradient.Visible;
                            chart2.Panel.MarginUnits = PanelMarginUnits.Pixels;
                            chart2.Panel.MarginBottom = 5.0;
                            chart2.Panel.MarginLeft = this.teeChart.Panel.MarginLeft;
                            chart2.Panel.MarginRight = this.teeChart.Panel.MarginRight;
                            chart2.Panel.MarginTop = 5.0;
                            chart2.Panel.MarginUnits = PanelMarginUnits.Pixels;
                            chart2.Walls.Back.Visible = this.teeChart.Walls.Back.Visible;
                            chart2.Walls.Bottom.Visible = this.teeChart.Walls.Bottom.Visible;
                            chart2.Walls.Left.Visible = this.teeChart.Walls.Left.Visible;
                            chart2.Walls.Right.Visible = this.teeChart.Walls.Right.Visible;
                            this.CreateChartSeries(base4, chart2);
                            base.Controls.Add(chart2);
                            this.indicatorCharts.Add(chart2);
                        }
                    }
                }
                else
                {
                    this.splitter.Visible = false;
                }
            }
            this.hScrollBar.TabIndex = base.Controls.Count;
            this.FillSeries();
            this.Arrange();
            this.ResizeControls();
            if (this.quotes != null)
            {
                this.quotes.Connection.Bar -= this.barUpdateEventHandler;
                this.quotes.Connection.Bar += this.barUpdateEventHandler;
            }
            this.Refresh();
        }

        private void ResizeControls()
        {
            if (!this.splitter.Visible)
            {
                this.teeChart.Size = new Size(base.Width, base.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight);
            }
            else
            {
                if (((int) (this.splitRatio * base.ClientSize.Height)) != this.splitter.SplitPosition)
                {
                    this.splitter.SplitPosition = (int) (this.splitRatio * base.ClientSize.Height);
                }
                int y = this.splitter.SplitPosition + this.splitter.Height;
                for (int i = 0; i < this.indicatorCharts.Count; i++)
                {
                    int num3 = (int) Math.Floor((double) ((((base.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) - this.splitter.SplitPosition) - this.splitter.Height) / this.indicatorCharts.Count));
                    ((Control) this.indicatorCharts[i]).Location = new Point(0, y);
                    ((Control) this.indicatorCharts[i]).Size = new Size(base.ClientSize.Width, (i == (this.indicatorCharts.Count - 1)) ? ((base.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) - y) : num3);
                    y += num3;
                }
            }
        }

        private void splitter_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.splitRatio = ((double) this.splitter.SplitPosition) / ((double) base.ClientSize.Height);
            this.ResizeControls();
        }

        private void teeChart_AfterDraw(object sender, Graphics3D g)
        {
            if ((this.midnight.Count > 0) && (this.points > 1))
            {
                int num = 0;
                int num2 = 0;
                double num3 = (((((g.Chart.Axes.Right.Position - g.Chart.Axes.Left.Position) - g.Chart.Panel.MarginLeft) - g.Chart.Axes.Left.Ticks.Width) - num) - num2) / ((double) (this.points - 1));
                foreach (int num4 in this.midnight)
                {
                    if (num4 != this.first)
                    {
                        int x = ((g.Chart.Axes.Left.Position + ((int) g.Chart.Panel.MarginLeft)) + num) + ((int) (((num4 - this.first) - 0.5) * num3));
                        Point p = new Point(x, (g.Chart.Axes.Top.Position + ((int) g.Chart.Panel.MarginTop)) + g.Chart.Axes.Top.Ticks.Width);
                        Point point2 = new Point(x, (g.Chart.Axes.Bottom.Position != 0) ? g.Chart.Axes.Bottom.Position : (g.Chart.Height - ((int) g.Chart.Panel.MarginBottom)));
                        g.Pen.Color = g.Chart.Axes.Bottom.Ticks.Color;
                        g.Pen.Style = DashStyle.Dash;
                        g.MoveTo(p);
                        g.LineTo(point2, 0);
                    }
                }
            }
            LevelCollection levels = (LevelCollection) this.chart2Levels[g.Chart];
            if (levels != null)
            {
                foreach (Level level in levels)
                {
                    if ((((g.Chart.Axes.Right.Maximum - g.Chart.Axes.Right.Minimum) != 0.0) && (level.Value >= g.Chart.Axes.Right.Minimum)) && (level.Value <= g.Chart.Axes.Right.Maximum))
                    {
                        int y = (((g.Chart.Height - ((int) g.Chart.Panel.MarginBottom)) - g.Chart.Axes.Right.MinimumOffset) - ((int) ((((((g.Chart.Height - g.Chart.Panel.MarginBottom) - g.Chart.Panel.MarginTop) - g.Chart.Axes.Right.MinimumOffset) - g.Chart.Axes.Right.MaximumOffset) * (level.Value - g.Chart.Axes.Right.Minimum)) / (g.Chart.Axes.Right.Maximum - g.Chart.Axes.Right.Minimum)))) - 1;
                        g.Pen = level.Pen;
                        g.MoveTo(new Point(g.Chart.Axes.Left.Position, y));
                        g.LineTo(new Point(g.Chart.Axes.Right.Position, y), 0);
                    }
                }
            }
            ArrayList list = (ArrayList) this.chart2Indicators[g.Chart];
            if (list != null)
            {
                string text = "";
                foreach (IndicatorBase.IndicatorBase base2 in list)
                {
                    text = text + ((text.Length > 0) ? ", " : "") + base2.ToString();
                }
                g.TextOut(0, 0, text);
            }
            if (this.isActive)
            {
                this.dummyTextBox.Focus();
            }
        }

        private void teeChart_GetAxisLabel(object sender, GetAxisLabelEventArgs e)
        {
            if ((this.quotes != null) && (((sender == this.teeChart.Axes.Bottom) && ((e.ValueIndex - this.dummyValues) >= 0)) && ((e.ValueIndex - this.dummyValues) < this.quotes.Bars.Count)))
            {
                IBar bar = this.quotes.Bars[e.ValueIndex - this.dummyValues];
                e.LabelText = (this.quotes.Period.Id == PeriodTypeId.Day) ? (bar.Time.Month.ToString("00") + "/" + bar.Time.Day.ToString("00")) : bar.Time.ToString("HH:mm");
            }
        }

        private void teeChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenu.Show(this, new Point(e.X, e.Y));
            }
        }

        private void teeChart_SizeChanged(object sender, EventArgs e)
        {
            this.Arrange();
        }

        private void UpdateQuotes(object sender, BarUpdateEventArgs e)
        {
            if (e.Quotes == this.quotes)
            {
                if (e.Operation == Operation.Insert)
                {
                    int count = this.candleStyles.Count;
                    for (int i = e.First; i <= e.Last; i++)
                    {
                        this.candleStyles.Add(e.Quotes.Bars[i].Open, e.Quotes.Bars[i].High, e.Quotes.Bars[i].Low, e.Quotes.Bars[i].Close);
                        foreach (Series series in this.series2IndicatorSeries.Keys)
                        {
                            series.Add(((IndicatorSeries) this.series2IndicatorSeries[series])[i]);
                        }
                    }
                    if (this.last == (count - 1))
                    {
                        this.last = this.candleStyles.Count - 1;
                    }
                }
                else if (e.Operation == Operation.Update)
                {
                    if (this.candleStyles.Count > this.dummyValues)
                    {
                        this.candleStyles.Delete(this.candleStyles.Count - 1);
                    }
                    this.candleStyles.Add(e.Quotes.Bars[e.Quotes.Bars.Count - 1].Open, e.Quotes.Bars[e.Quotes.Bars.Count - 1].High, e.Quotes.Bars[e.Quotes.Bars.Count - 1].Low, e.Quotes.Bars[e.Quotes.Bars.Count - 1].Close);
                    foreach (Series series2 in this.series2IndicatorSeries.Keys)
                    {
                        if (this.candleStyles.Count > this.dummyValues)
                        {
                            series2.Delete(series2.Count - 1);
                        }
                        series2.Add(((IndicatorSeries) this.series2IndicatorSeries[series2])[e.Quotes.Bars.Count - 1]);
                    }
                }
                if (this.last < 0)
                {
                    this.last = (this.dummyValues + e.Quotes.Bars.Count) - 1;
                }
                if (e.First <= this.last)
                {
                    this.Arrange();
                    this.Refresh();
                }
            }
        }

        private System.Windows.Forms.Form AppWindow
        {
            get
            {
                for (Control control = base.Parent; control != null; control = control.Parent)
                {
                    if (control is System.Windows.Forms.Form)
                    {
                        return ((System.Windows.Forms.Form) control).MdiParent;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Index of first quote bar displayed.
        /// </summary>
        public int First
        {
            get
            {
                return this.first;
            }
        }

        private System.Windows.Forms.Form Form
        {
            get
            {
                for (Control control = base.Parent; control != null; control = control.Parent)
                {
                    if (control is System.Windows.Forms.Form)
                    {
                        return (System.Windows.Forms.Form) control;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Index of last quote bar displayed.
        /// </summary>
        public int Last
        {
            get
            {
                return this.last;
            }
        }

        /// <summary>
        /// The quotes to be displayed. Set NULL before closing the control.
        /// </summary>
        public Quotes Quotes
        {
            get
            {
                return this.quotes;
            }
            set
            {
                if (this.quotes != null)
                {
                    this.quotes.Symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.quotes.MarketDataItem);
                    this.quotes.Connection.Bar -= this.barUpdateEventHandler;
                }
                this.quotes = value;
                this.candleStyles.Clear();
                if (this.quotes != null)
                {
                    if (this.quotes.Period.Id == PeriodTypeId.Day)
                    {
                        this.teeChart.Axes.Bottom.Labels.DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        if (this.quotes.Period.Value <= 2)
                        {
                            this.teeChart.Axes.Bottom.Increment = Utils.GetDateTimeStep(DateTimeSteps.OneWeek);
                        }
                        if (this.quotes.Period.Value <= 7)
                        {
                            this.teeChart.Axes.Bottom.Increment = Utils.GetDateTimeStep(DateTimeSteps.HalfMonth);
                        }
                        else
                        {
                            this.teeChart.Axes.Bottom.Increment = Utils.GetDateTimeStep(DateTimeSteps.OneMonth);
                        }
                    }
                    else if (this.quotes.Period.Id == PeriodTypeId.Minute)
                    {
                        this.teeChart.Axes.Bottom.Labels.DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                        if (this.quotes.Period.Value <= 3)
                        {
                            this.teeChart.Axes.Bottom.Increment = Utils.GetDateTimeStep(DateTimeSteps.FifteenMinutes);
                        }
                        else if (this.quotes.Period.Value <= 10)
                        {
                            this.teeChart.Axes.Bottom.Increment = Utils.GetDateTimeStep(DateTimeSteps.OneHour);
                        }
                        else if (this.quotes.Period.Value <= 30)
                        {
                            this.teeChart.Axes.Bottom.Increment = Utils.GetDateTimeStep(DateTimeSteps.TwoHours);
                        }
                    }
                    else
                    {
                        this.teeChart.Axes.Bottom.Labels.DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
                        this.teeChart.Axes.Bottom.Increment = 5.0;
                    }
                    this.teeChart.Axes.Bottom.Automatic = false;
                    this.teeChart.Axes.Bottom.Labels.Style = AxisLabelStyle.Text;
                    this.teeChart.Axes.Bottom.MaximumOffset = (int) (1.5 * this.candleStyles.CandleWidth);
                    this.teeChart.Axes.Bottom.MinimumOffset = (int) (1.5 * this.candleStyles.CandleWidth);
                    this.teeChart.Axes.Right.Automatic = false;
                    this.teeChart.Axes.Right.Increment = this.quotes.Symbol.TickSize;
                    this.teeChart.Axes.Right.Labels.ValueFormat = this.quotes.Symbol.FormatString;
                    this.teeChart.Axes.Right.MaximumOffset = 2;
                    this.teeChart.Axes.Right.MinimumOffset = 2;
                    this.teeChart.Panel.MarginLeft = 5.0;
                    this.teeChart.Panel.MarginTop = 5.0;
                    this.teeChart.Panning.Allow = ScrollModes.None;
                    this.teeChart.Zoom.Allow = false;
                    this.newChartMenuItems.Clear();
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 2));
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 3));
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 5));
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 10));
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 20));
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 30));
                    this.newChartMenuItems.Add(new Period(this.quotes.Period.Id, this.quotes.Period.Value * 60));
                    this.newChartMenuItems.Add(null);
                    if (this.quotes.Period.Id == PeriodTypeId.Tick)
                    {
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 1));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 2));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 3));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 5));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 10));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 20));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 30));
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Minute, 60));
                        this.newChartMenuItems.Add(null);
                    }
                    if (this.quotes.Period.Id != PeriodTypeId.Day)
                    {
                        this.newChartMenuItems.Add(new Period(PeriodTypeId.Day, 1));
                    }
                    MenuItem[] items = new MenuItem[this.newChartMenuItems.Count];
                    for (int i = 0; i < this.newChartMenuItems.Count; i++)
                    {
                        items[i] = (this.newChartMenuItems[i] == null) ? new MenuItem("-") : new MenuItem(((Period) this.newChartMenuItems[i]).ToString(), new EventHandler(this.NewChartEvt));
                    }
                    this.ContextMenu = new ContextMenu();
                    this.ContextMenu.MenuItems.Add("New chart", items);
                    this.ContextMenu.MenuItems.Add("Indicators ...", new EventHandler(this.MenuItemEvt));
                    this.ContextMenu.MenuItems.Add("-");
                    this.ContextMenu.MenuItems.Add("Next page", new EventHandler(this.MenuItemEvt));
                    this.ContextMenu.MenuItems.Add("Previous page", new EventHandler(this.MenuItemEvt));
                    this.ContextMenu.MenuItems.Add("Next bar", new EventHandler(this.MenuItemEvt));
                    this.ContextMenu.MenuItems.Add("Previous bar", new EventHandler(this.MenuItemEvt));
                    this.ContextMenu.MenuItems.Add("-");
                    this.ContextMenu.MenuItems.Add("Zoom in", new EventHandler(this.MenuItemEvt));
                    this.ContextMenu.MenuItems.Add("Zoom out", new EventHandler(this.MenuItemEvt));
                    if (this.Quotes.Period.Id != PeriodTypeId.Day)
                    {
                        this.ContextMenu.MenuItems.Add("-");
                        this.ContextMenu.MenuItems.Add(new MenuItem("Next day", new EventHandler(this.MenuItemEvt), Shortcut.F5));
                        this.ContextMenu.MenuItems.Add(new MenuItem("Previous day", new EventHandler(this.MenuItemEvt), Shortcut.F4));
                    }
                    this.last = this.quotes.Bars.Count - 1;
                    this.Register((IndicatorCollection) this.indicators.Clone());
                    this.Refresh();
                    this.barUpdateEventHandler = new BarUpdateEventHandler(this.UpdateQuotes);
                    this.quotes.Connection.Bar += this.barUpdateEventHandler;
                    this.quotes.Symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.quotes.MarketDataItem);
                }
            }
        }

        /// <summary>
        /// The chart onctrol itself.
        /// </summary>
        public TChart TeeChart
        {
            get
            {
                return this.teeChart;
            }
        }
    }
}