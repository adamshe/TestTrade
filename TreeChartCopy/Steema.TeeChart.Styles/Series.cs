namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Functions;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    [Serializable, DefaultProperty("Color"), Designer(typeof(SeriesDesigner)), DesignTimeVisible(true), ToolboxBitmap(typeof(Bitmap))]
    public class Series : TeeBase
    {
        protected internal bool AllowSinglePoint;
        public const int AutoDepth = -1;
        public const int AutoZOrder = -1;
        protected internal bool bActive;
        protected internal ChartBrush bBrush;
        protected bool bColorEach;
        protected bool calcVisiblePoints;
        internal string colorMember;
        private System.Windows.Forms.Cursor cursor;
        private Axis customHorizAxis;
        private Axis customVertAxis;
        private object datasource;
        private int depth;
        protected bool DrawBetweenPoints;
        internal int endZ;
        protected internal int firstVisible;
        private Steema.TeeChart.Functions.Function function;
        public Axis GetHorizAxis;
        public Axis GetVertAxis;
        public bool HasZValues;
        private HorizontalAxis horizAxis;
        internal ColorList iColors;
        private ValueListOrder ILabelOrder;
        internal int INumSampleValues;
        private bool isMouseInside;
        internal int iZOrder;
        internal string labelMember;
        protected internal int lastVisible;
        internal Steema.TeeChart.Styles.ValueList mandatory;
        internal bool manualData;
        internal SeriesMarks marks;
        internal int middleZ;
        internal Steema.TeeChart.Styles.ValueList notMandatory;
        internal string percentFormat;
        private bool showInLegend;
        protected internal StringList sLabels;
        internal int startZ;
        private string title;
        private int updating;
        public bool UseAxis;
        protected internal bool UseSeriesColor;
        internal string valueFormat;
        internal Steema.TeeChart.Styles.ValuesLists valuesList;
        private VerticalAxis vertAxis;
        protected internal Steema.TeeChart.Styles.ValueList vxValues;
        protected internal Steema.TeeChart.Styles.ValueList vyValues;
        protected internal bool yMandatory;
        private int zOrder;

        public event PaintChartEventHandler AfterDrawValues;

        public event PaintChartEventHandler BeforeDrawValues;

        public event MouseEventHandler Click;

        public event MouseEventHandler DblClick;

        public event GetSeriesMarkEventHandler GetSeriesMark;

        public event EventHandler MouseEnter;

        public event EventHandler MouseLeave;

        private Series() : this((Chart) null)
        {
        }

        protected Series(Chart c) : base(c)
        {
            this.bBrush = new ChartBrush(null, System.Drawing.Color.Empty);
            this.bActive = true;
            this.title = "";
            this.horizAxis = HorizontalAxis.Bottom;
            this.vertAxis = VerticalAxis.Left;
            this.valuesList = new Steema.TeeChart.Styles.ValuesLists();
            this.zOrder = -1;
            this.depth = -1;
            this.showInLegend = true;
            this.DrawBetweenPoints = true;
            this.yMandatory = true;
            this.AllowSinglePoint = true;
            this.UseAxis = true;
            this.UseSeriesColor = true;
            this.calcVisiblePoints = true;
            this.valueFormat = "#,##0.###";
            this.labelMember = "";
            this.colorMember = "";
            this.percentFormat = "##0.## %";
            this.cursor = Cursors.Default;
            this.Click = null;
            this.bBrush.chart = c;
            this.vxValues = new Steema.TeeChart.Styles.ValueList(this, "X");
            this.vxValues.Order = ValueListOrder.Ascending;
            this.vyValues = new Steema.TeeChart.Styles.ValueList(this, "Y");
            this.mandatory = this.vyValues;
            this.notMandatory = this.vxValues;
            if (base.chart != null)
            {
                base.chart.Series.Add(this);
                this.Added();
            }
        }

        public Series(IContainer container) : this((Chart) null)
        {
            container.Add(this);
        }

        [Description("Adds a new null (transparent) point.")]
        public int Add()
        {
            return this.Add(0.0, System.Drawing.Color.Transparent);
        }

        [Description("Adds all points in source Series.")]
        public void Add(Series source)
        {
            this.AddValuesFrom(source);
        }

        public void Add(IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                this.Add(Convert.ToDouble(list[i]));
            }
        }

        public virtual void Add(DataTable table)
        {
            this.Add(new DataView(table));
        }

        public virtual void Add(DataView view)
        {
            Steema.TeeChart.Styles.ValueList otherList = null;
            int fields = this.GetFields(ref otherList);
            int num3 = -1;
            if (fields != 0)
            {
                int index = view.Table.Columns.IndexOf(this.mandatory.DataMember);
                if (fields > 1)
                {
                    num3 = view.Table.Columns.IndexOf(otherList.DataMember);
                }
                if (index != -1)
                {
                    int num4 = -1;
                    if (this.labelMember.Length != 0)
                    {
                        num4 = view.Table.Columns.IndexOf(this.labelMember);
                    }
                    int num5 = -1;
                    if (this.colorMember.Length != 0)
                    {
                        num5 = view.Table.Columns.IndexOf(this.colorMember);
                    }
                    System.Drawing.Color empty = System.Drawing.Color.Empty;
                    string text = "";
                    double x = 0.0;
                    for (int i = 0; i < view.Count; i++)
                    {
                        DataRow row = view[i].Row;
                        if (row.IsNull(index))
                        {
                            this.Add();
                        }
                        else
                        {
                            if (num5 >= 0)
                            {
                                empty = (System.Drawing.Color) row[num5];
                            }
                            if (num4 >= 0)
                            {
                                text = Convert.ToString(row[num4]);
                            }
                            if (row[index] is DateTime)
                            {
                                this.mandatory.TempValue = Utils.DateTime((DateTime) row[index]);
                            }
                            else
                            {
                                this.mandatory.TempValue = Convert.ToDouble(row[index]);
                            }
                            if (num3 != -1)
                            {
                                if (row[num3] is DateTime)
                                {
                                    x = Utils.DateTime((DateTime) row[num3]);
                                }
                                else
                                {
                                    x = Convert.ToDouble(row[num3]);
                                }
                            }
                            if (fields == 1)
                            {
                                this.Add(this.mandatory.TempValue, text, empty);
                            }
                            else if (fields > 1)
                            {
                                this.Add(x, this.mandatory.TempValue, text, empty);
                            }
                        }
                    }
                }
            }
        }

        public void Add(IDataReader r)
        {
            DataSeriesSource.FillSeries(this, r);
        }

        [Description("Adds a new point with specified value.")]
        public int Add(double value)
        {
            if (this.yMandatory)
            {
                return this.Add((double) this.Count, value);
            }
            return this.Add(value, (double) this.Count);
        }

        public int Add(PointF p)
        {
            return this.Add((double) p.X, (double) p.Y);
        }

        [Description("Adds a new point with specified value.")]
        public int Add(int value)
        {
            if (this.yMandatory)
            {
                return this.Add((double) this.Count, (double) value);
            }
            return this.Add((double) value, (double) this.Count);
        }

        [Description("Adds a new point with specified value.")]
        public int Add(float value)
        {
            return this.Add((double) value);
        }

        [Description("Adds a new null point with specified text.")]
        public int Add(string text)
        {
            return this.Add(0.0, text, System.Drawing.Color.Transparent);
        }

        [Description("Adds the array of double values.")]
        public void Add(double[] values)
        {
            foreach (double num in values)
            {
                this.Add(num);
            }
        }

        [Description("Adds the array of integer values.")]
        public void Add(int[] values)
        {
            foreach (int num in values)
            {
                this.Add(num);
            }
        }

        [Description("Adds the array of float values.")]
        public void Add(float[] values)
        {
            foreach (float num in values)
            {
                this.Add(num);
            }
        }

        [Description("Adds the X and Y arrays.")]
        public void Add(Array xValues, Array yValues)
        {
            int length = yValues.GetLength(0);
            this.XValues.Count = length;
            this.XValues.Value = this.ConvertArray(xValues, length);
            this.YValues.Count = length;
            this.YValues.Value = this.ConvertArray(yValues, length);
            this.XValues.statsOk = false;
            this.YValues.statsOk = false;
            this.Invalidate();
        }

        public int Add(DateTime x, DateTime y)
        {
            this.vxValues.dateTime = true;
            this.vyValues.dateTime = true;
            return this.Add(Utils.DateTime(x), Utils.DateTime(y));
        }

        public int Add(DateTime x, double y)
        {
            this.vxValues.dateTime = true;
            return this.Add(Utils.DateTime(x), y);
        }

        [Description("Adds a new point with specified x and y values.")]
        public int Add(double x, double y)
        {
            int valueIndex = this.vxValues.AddChartValue(x);
            this.vyValues.InsertChartValue(valueIndex, y);
            int count = this.valuesList.Count;
            if (count > 2)
            {
                for (int i = 2; i < count; i++)
                {
                    Steema.TeeChart.Styles.ValueList list = this.valuesList[i];
                    list.InsertChartValue(valueIndex, list.TempValue);
                }
            }
            this.Invalidate();
            return valueIndex;
        }

        public int Add(double value, System.Drawing.Color color)
        {
            if (this.yMandatory)
            {
                return this.Add((double) this.Count, value, color);
            }
            return this.Add(value, (double) this.Count, color);
        }

        [Description("Adds a new point with specified value and text.")]
        public int Add(double value, string text)
        {
            if (this.yMandatory)
            {
                return this.Add((double) this.Count, value, text);
            }
            return this.Add(value, (double) this.Count, text);
        }

        public int Add(DateTime x, double y, System.Drawing.Color c)
        {
            this.vxValues.dateTime = true;
            return this.Add(Utils.DateTime(x), y, c);
        }

        public int Add(DateTime x, double y, string text)
        {
            this.vxValues.dateTime = true;
            return this.Add(Utils.DateTime(x), y, text);
        }

        public int Add(double x, double y, System.Drawing.Color color)
        {
            return this.Add(x, y, "", color);
        }

        [Description("Adds a new point with specified x,y values and text.")]
        public int Add(double x, double y, string text)
        {
            return this.Add(x, y, text, System.Drawing.Color.Empty);
        }

        public int Add(double value, string text, System.Drawing.Color color)
        {
            if (this.yMandatory)
            {
                return this.Add((double) this.Count, value, text, color);
            }
            return this.Add(value, (double) this.Count, text, color);
        }

        public int Add(DateTime aDate, double y, string text, System.Drawing.Color color)
        {
            this.vxValues.dateTime = true;
            return this.Add(Utils.DateTime(aDate), y, text, color);
        }

        public int Add(double x, double y, string text, System.Drawing.Color color)
        {
            int num = this.Add(x, y);
            if (!color.IsEmpty)
            {
                this.Colors[num] = color;
            }
            if ((text != null) && (text.Length != 0))
            {
                this.Labels[num] = text;
            }
            return num;
        }

        protected internal int AddChartValue(Series source, int valueIndex)
        {
            double aValue = source.XValues[valueIndex];
            double num2 = source.YValues[valueIndex];
            if (this.yMandatory != source.yMandatory)
            {
                double num3 = aValue;
                aValue = num2;
                num2 = num3;
            }
            int num4 = this.vxValues.AddChartValue(aValue);
            this.YValues.InsertChartValue(num4, num2);
            int count = this.valuesList.Count;
            if (count > 2)
            {
                int num6 = source.valuesList.Count - 1;
                for (int i = 2; i < count; i++)
                {
                    num2 = (i <= num6) ? source.valuesList[i][valueIndex] : 0.0;
                    this.valuesList[i].InsertChartValue(num4, num2);
                }
            }
            return num4;
        }

        protected internal virtual void Added()
        {
            if (this.Color.IsEmpty)
            {
                this.Color = base.chart.FreeSeriesColor(true);
            }
            this.RecalcGetAxis();
            this.CheckDataSource();
        }

        private void AddedValue(Series source, int valueIndex)
        {
            int num = this.AddChartValue(source, valueIndex);
            if (source.iColors != null)
            {
                this.Colors[num] = source.Colors[valueIndex];
            }
            if (source.sLabels != null)
            {
                this.Labels[num] = source.sLabels[valueIndex];
            }
            this.NotifyNewValue(this, num);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Add() method without parameters.")]
        public int AddNull()
        {
            return this.Add();
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Add(x,y,Color.Transparent) method.")]
        public int AddNullXY(double x, double y)
        {
            return this.Add(x, y, System.Drawing.Color.Transparent);
        }

        protected virtual void AddSampleValues(int numValues)
        {
            SeriesRandom random = this.RandomBounds(numValues);
            int num = Utils.Round((double) (random.DifY * 0.25));
            for (int i = 1; i <= numValues; i++)
            {
                random.tmpY = (int) Math.Abs((double) ((random.tmpY + (num * random.Random())) - (num / 2)));
                if (this.yMandatory)
                {
                    this.Add(random.tmpX, random.tmpY);
                }
                else
                {
                    this.Add(random.tmpY, random.tmpX);
                }
                random.tmpX += random.StepX;
            }
        }

        internal void AddToContainer(IContainer c)
        {
            c.Add(this);
            ISite site = this.Site;
            if (site != null)
            {
                this.Title = site.Name;
            }
            this.Added();
        }

        protected void AddValues(Array source)
        {
            Series series = (Series) source.GetValue(0);
            if (this.IsValidSourceOf(series))
            {
                this.BeginUpdate();
                this.Clear();
                if (this.Function == null)
                {
                    if (this.yMandatory != series.yMandatory)
                    {
                        this.XValues.DateTime = series.YValues.DateTime;
                        this.YValues.DateTime = series.XValues.DateTime;
                    }
                    else
                    {
                        this.XValues.DateTime = series.XValues.DateTime;
                        this.YValues.DateTime = series.YValues.DateTime;
                    }
                    this.AddValuesFrom(series);
                }
                else
                {
                    this.XValues.DateTime = series.XValues.DateTime;
                    this.YValues.DateTime = series.YValues.DateTime;
                    this.Function.AddPoints(source);
                }
                this.EndUpdate();
            }
        }

        private void AddValuesFrom(Series source)
        {
            int count = source.Count;
            for (int i = 0; i < count; i++)
            {
                this.AddedValue(source, i);
            }
        }

        public void Assign(Series source)
        {
            this.title = source.title;
            this.bBrush.Assign(source.bBrush);
            this.bColorEach = source.bColorEach;
            this.showInLegend = source.showInLegend;
            this.valueFormat = source.valueFormat;
            this.percentFormat = source.percentFormat;
            this.bActive = source.bActive;
            if (this.datasource == null)
            {
                this.AssignValues(source);
            }
            this.CheckDataSource();
        }

        internal static void AssignDispose(ref Series s, Series newSeries)
        {
            int index = s.chart.Series.IndexOf(s);
            newSeries.Assign(s);
            ISite site = s.Site;
            IContainer c = (site != null) ? site.Container : null;
            s.Dispose(true);
            s = newSeries;
            if (c != null)
            {
                s.AddToContainer(c);
            }
            s.chart.Series.MoveTo(s, index);
        }

        public void AssignValues(Series source)
        {
            Array array = new Series[1];
            array.SetValue(source, 0);
            this.AddValues(array);
        }

        protected internal virtual bool AssociatedToAxis(Axis a)
        {
            if (!this.UseAxis)
            {
                return false;
            }
            if (!a.horizontal || ((this.GetHorizAxis != a) && (this.horizAxis != HorizontalAxis.Both)))
            {
                if (a.horizontal)
                {
                    return false;
                }
                if (this.GetVertAxis != a)
                {
                    return (this.vertAxis == VerticalAxis.Both);
                }
            }
            return true;
        }

        public void BeginUpdate()
        {
            this.updating++;
        }

        internal void CalcFirstLastVisibleIndex()
        {
            Rectangle chartRect = base.chart.ChartRect;
            this.firstVisible = -1;
            this.lastVisible = -1;
            if (this.Count > 0)
            {
                int num3 = this.Count - 1;
                if (!this.calcVisiblePoints || (this.notMandatory.Order == ValueListOrder.None))
                {
                    this.firstVisible = 0;
                    this.lastVisible = num3;
                }
                else
                {
                    double num = this.CalcMinMaxValue(chartRect.X, chartRect.Y, chartRect.Right, chartRect.Bottom);
                    this.firstVisible = 0;
                    while (this.notMandatory[this.firstVisible] < num)
                    {
                        this.firstVisible++;
                        if (this.firstVisible > num3)
                        {
                            this.firstVisible = -1;
                            break;
                        }
                    }
                    if (this.firstVisible >= 0)
                    {
                        double num2 = this.CalcMinMaxValue(chartRect.Right, chartRect.Bottom, chartRect.X, chartRect.Y);
                        if (this.notMandatory.Last <= num2)
                        {
                            this.lastVisible = num3;
                        }
                        else
                        {
                            this.lastVisible = this.firstVisible;
                            while (this.notMandatory[this.lastVisible] < num2)
                            {
                                this.lastVisible++;
                                if (this.lastVisible > num3)
                                {
                                    this.lastVisible = num3;
                                    break;
                                }
                            }
                            if (!this.DrawBetweenPoints && (this.notMandatory[this.lastVisible] > num2))
                            {
                                this.lastVisible--;
                            }
                        }
                    }
                }
            }
        }

        protected internal virtual void CalcHorizMargins(ref int leftMargin, ref int rightMargin)
        {
            leftMargin = 0;
            rightMargin = 0;
        }

        private double CalcMinMaxValue(int A, int B, int C, int D)
        {
            Axis axis = this.yMandatory ? this.GetHorizAxis : this.GetVertAxis;
            if (this.yMandatory)
            {
                if (!axis.Inverted)
                {
                    return axis.CalcPosPoint(A);
                }
                return axis.CalcPosPoint(C);
            }
            if (!axis.Inverted)
            {
                return axis.CalcPosPoint(D);
            }
            return axis.CalcPosPoint(B);
        }

        private string CalcPercentSt(double tmpValue)
        {
            Steema.TeeChart.Styles.ValueList mandatory = this.mandatory;
            return Utils.FormatFloat(this.percentFormat, (mandatory.TotalABS == 0.0) ? 100.0 : (tmpValue / mandatory.TotalABS));
        }

        public int CalcPosValue(double value)
        {
            if (!this.yMandatory)
            {
                return this.CalcXPosValue(value);
            }
            return this.CalcYPosValue(value);
        }

        protected internal virtual void CalcVerticalMargins(ref int topMargin, ref int bottomMargin)
        {
            topMargin = 0;
            bottomMargin = 0;
        }

        public virtual int CalcXPos(int index)
        {
            return this.CalcXPosValue(this.vxValues.Value[index]);
        }

        public int CalcXPosValue(double value)
        {
            return this.GetHorizAxis.CalcXPosValue(value);
        }

        public int CalcXSizeValue(double value)
        {
            return this.GetHorizAxis.CalcSizeValue(value);
        }

        private string CalcXValue(int valueIndex)
        {
            if (this.GetHorizAxis != null)
            {
                return this.GetHorizAxis.Labels.LabelValue(this.vxValues.Value[valueIndex]);
            }
            return this.vxValues.Value[valueIndex].ToString(this.valueFormat);
        }

        public virtual int CalcYPos(int index)
        {
            return this.CalcYPosValue(this.YValues.Value[index]);
        }

        public int CalcYPosValue(double value)
        {
            return this.GetVertAxis.CalcYPosValue(value);
        }

        public int CalcYSizeValue(double value)
        {
            return this.GetVertAxis.CalcSizeValue(value);
        }

        protected internal virtual void CalcZOrder()
        {
            if (this.zOrder == -1)
            {
                if (base.chart.Aspect.View3D)
                {
                    base.chart.maxZOrder++;
                    this.iZOrder = base.chart.maxZOrder;
                }
                else
                {
                    this.iZOrder = 0;
                }
            }
            else
            {
                base.chart.maxZOrder = Math.Max(base.chart.maxZOrder, this.ZOrder);
            }
        }

        protected internal bool CanAddRandomPoints()
        {
            return base.DesignMode;
        }

        public static void ChangeType(ref Series s, System.Type newType)
        {
            if (s.GetType() != newType)
            {
                Series newSeries = CreateNewSeries(s.chart, newType, null);
                if (newSeries != null)
                {
                    AssignDispose(ref s, newSeries);
                }
            }
        }

        public void CheckDataSource()
        {
            if (this.datasource != null)
            {
                this.FillFromDataSource();
            }
            else if (this.function != null)
            {
                if (this.function.NoSourceRequired)
                {
                    this.BeginUpdate();
                    this.Clear();
                    this.function.AddPoints(null);
                    this.EndUpdate();
                }
            }
            else if (!this.manualData && this.CanAddRandomPoints())
            {
                this.FillSampleValues();
            }
        }

        protected internal bool CheckMouse(ref System.Windows.Forms.Cursor c, int x, int y)
        {
            if (((this.Cursor != Cursors.Default) || (this.MouseEnter != null)) || (this.MouseLeave != null))
            {
                if (this.Clicked(x, y) != -1)
                {
                    if (this.Cursor != Cursors.Default)
                    {
                        c = this.Cursor;
                    }
                    if (!this.isMouseInside)
                    {
                        this.isMouseInside = true;
                        if (this.MouseEnter != null)
                        {
                            this.MouseEnter(this, new EventArgs());
                        }
                    }
                    return true;
                }
                if (this.isMouseInside)
                {
                    this.isMouseInside = false;
                    if (this.MouseLeave != null)
                    {
                        this.MouseLeave(this, new EventArgs());
                    }
                }
            }
            return false;
        }

        [Description("Reorders points according to Order property of X,Y,etc value lists.")]
        public void CheckOrder()
        {
            if (this.mandatory.Order != ValueListOrder.None)
            {
                this.mandatory.Sort();
                if (this.notMandatory.valueSource.Length == 0)
                {
                    this.notMandatory.FillSequence();
                }
                this.Repaint();
            }
        }

        internal void CheckOtherSeries(Series dest)
        {
            if (dest == this)
            {
                throw new TeeChartException(Texts.CircularSeries);
            }
            ArrayList list = dest.DataSourceArray();
            if (list != null)
            {
                foreach (object obj2 in list)
                {
                    ((Series) obj2).CheckOtherSeries(this);
                }
            }
        }

        public virtual void Clear()
        {
            this.ClearLists();
            if (this.updating == 0)
            {
                this.Invalidate();
            }
        }

        protected virtual void ClearLists()
        {
            foreach (Steema.TeeChart.Styles.ValueList list in this.valuesList)
            {
                list.Clear();
            }
            if (this.sLabels != null)
            {
                this.sLabels.Clear();
            }
            if (this.iColors != null)
            {
                this.iColors.Clear();
            }
            if (this.marks != null)
            {
                this.marks.Clear();
            }
        }

        public int Clicked(Point p)
        {
            return this.Clicked(p.X, p.Y);
        }

        public virtual int Clicked(int x, int y)
        {
            return -1;
        }

        private void ClipRegionCreate(ref bool ActiveRegion)
        {
            Rectangle chartRect = base.chart.ChartRect;
            if (base.chart.CanClip())
            {
                chartRect.Height++;
                base.chart.Graphics3D.ClipCube(chartRect, 0, base.chart.Aspect.Width3D);
                ActiveRegion = true;
            }
        }

        private void ClipRegionDone(ref bool ActiveRegion)
        {
            if (ActiveRegion)
            {
                base.chart.Graphics3D.UnClip();
                ActiveRegion = false;
            }
        }

        public Series Clone()
        {
            System.Type type;
            if (this.Function == null)
            {
                type = null;
            }
            else
            {
                type = this.Function.GetType();
            }
            Series series = CreateNewSeries(base.chart, base.GetType(), type);
            series.AssignValues(this);
            return series;
        }

        private int CompareLabelIndex(int a, int b)
        {
            int num = string.Compare(this.Labels[a], this.Labels[b]);
            if (this.ILabelOrder != ValueListOrder.Descending)
            {
                return num;
            }
            return -num;
        }

        internal double[] ConvertArray(Array a, int numPoints)
        {
            double[] array = null;
            if (a is DateTime[])
            {
                array = new double[numPoints];
                int upperBound = a.GetUpperBound(0);
                for (int i = a.GetLowerBound(0); i <= upperBound; i++)
                {
                    array[i] = Utils.DateTime((DateTime) a.GetValue(i));
                }
                return array;
            }
            if (((a is int[]) || (a is float[])) || (a is decimal[]))
            {
                array = new double[numPoints];
                a.CopyTo(array, 0);
                return array;
            }
            if (a is double[])
            {
                array = (double[]) a;
            }
            return array;
        }

        protected internal virtual int CountLegendItems()
        {
            return this.Count;
        }

        public static Series CreateNewSeries(Chart chart, System.Type type, System.Type aFunction)
        {
            return CreateNewSeries(chart, type, aFunction, 0);
        }

        public static Series CreateNewSeries(Chart chart, System.Type type, System.Type aFunction, int subIndex)
        {
            Series series = NewFromType(type);
            series.Chart = chart;
            if (aFunction != null)
            {
                series.Function = Steema.TeeChart.Functions.Function.NewInstance(aFunction);
            }
            if (subIndex != 0)
            {
                series.SetSubGallery(subIndex);
            }
            return series;
        }

        protected internal virtual void CreateSubGallery(SubGalleryEventHandler AddSubChart)
        {
            AddSubChart(Texts.Normal);
        }

        internal ArrayList DataSourceArray()
        {
            if (this.datasource is Array)
            {
                Array datasource = (Array) this.datasource;
                if (datasource.GetValue(0) is Series)
                {
                    ArrayList list = new ArrayList();
                    foreach (object obj2 in datasource)
                    {
                        list.Add(obj2);
                    }
                    return list;
                }
            }
            else if (this.datasource is ArrayList)
            {
                ArrayList list2 = (ArrayList) this.datasource;
                if (list2[0] is Series)
                {
                    return list2;
                }
            }
            else if (this.datasource is Series)
            {
                ArrayList list3 = new ArrayList();
                list3.Add(this.datasource);
                return list3;
            }
            return null;
        }

        public void Delete(int index)
        {
            this.vxValues.RemoveAt(index);
            this.vyValues.RemoveAt(index);
            if ((this.sLabels != null) && (this.sLabels.Count > index))
            {
                this.sLabels.RemoveAt(index);
            }
            if ((this.iColors != null) && (this.iColors.Count > index))
            {
                this.iColors.RemoveAt(index);
            }
            if (this.marks != null)
            {
                if (this.marks.Positions.Count > index)
                {
                    this.marks.Positions.RemoveAt(index);
                }
                if (this.marks.Items.Count > index)
                {
                    this.marks.Items.RemoveAt(index);
                }
            }
            if (this.updating == 0)
            {
                this.Invalidate();
            }
        }

        public void Delete(int index, int count)
        {
            this.Delete(index, count, false);
        }

        public void Delete(int index, int count, bool removeGap)
        {
            this.vxValues.RemoveRange(index, count);
            this.vyValues.RemoveRange(index, count);
            if ((this.sLabels != null) && (this.sLabels.Count > ((index + count) - 1)))
            {
                this.sLabels.RemoveRange(index, count);
            }
            if ((this.iColors != null) && (this.iColors.Count > ((index + count) - 1)))
            {
                this.iColors.RemoveRange(index, count);
            }
            if ((this.marks != null) && (this.marks.Positions.Count > ((index + count) - 1)))
            {
                this.marks.Positions.RemoveRange(index, count);
            }
            if (removeGap)
            {
                this.notMandatory.FillSequence();
            }
            if (this.updating == 0)
            {
                this.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Chart = null;
            base.Dispose(disposing);
        }

        protected virtual void DoAfterDrawValues()
        {
            if (this.AfterDrawValues != null)
            {
                this.AfterDrawValues(this, base.chart.graphics3D);
            }
        }

        protected internal virtual void DoBeforeDrawChart()
        {
        }

        protected virtual void DoBeforeDrawValues()
        {
            if (this.BeforeDrawValues != null)
            {
                this.BeforeDrawValues(this, base.chart.graphics3D);
            }
        }

        protected void DoGetSeriesMark(int valueIndex, ref string markText)
        {
            if (this.GetSeriesMark != null)
            {
                GetSeriesMarkEventArgs e = new GetSeriesMarkEventArgs(valueIndex, markText);
                this.GetSeriesMark(this, e);
                markText = e.MarkText;
            }
        }

        protected internal virtual void Draw()
        {
            if (this.DrawValuesForward())
            {
                for (int i = this.firstVisible; i <= this.lastVisible; i++)
                {
                    this.DrawValue(i);
                }
            }
            else
            {
                for (int j = this.lastVisible; j >= this.firstVisible; j--)
                {
                    this.DrawValue(j);
                }
            }
        }

        private void DrawAllSeriesValue(int valueIndex)
        {
            int num;
            int index = base.chart.Series.IndexOf(this);
            int num3 = base.chart.Series.Count - 1;
            if (valueIndex < this.Count)
            {
                if (this.DrawSeriesForward(valueIndex))
                {
                    for (num = index; num <= num3; num++)
                    {
                        this.TryDrawSeries(base.chart[num], valueIndex);
                    }
                }
                else
                {
                    for (num = num3; num >= index; num--)
                    {
                        this.TryDrawSeries(base.chart[num], valueIndex);
                    }
                }
            }
            else
            {
                for (num = index; num <= num3; num++)
                {
                    this.TryDrawSeries(base.chart[num], valueIndex);
                }
            }
        }

        internal void DrawLegend(int valueIndex, Rectangle rect)
        {
            this.DrawLegend(base.chart.graphics3D, valueIndex, rect);
        }

        internal void DrawLegend(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if ((valueIndex != -1) || !this.ColorEach)
            {
                System.Drawing.Color color2;
                if (valueIndex == -1)
                {
                    color2 = this.Color;
                }
                else
                {
                    color2 = this.LegendItemColor(valueIndex);
                }
                g.Brush.Color = color2;
                if (!color2.IsEmpty)
                {
                    System.Drawing.Color backColor = base.chart.Legend.Color;
                    ChartBrush aBrush = new ChartBrush(base.chart);
                    aBrush.Assign(this.bBrush);
                    this.PrepareLegendCanvas(g, valueIndex, ref backColor, ref aBrush);
                    if (base.chart.legendPen != null)
                    {
                        g.Pen = base.chart.legendPen;
                    }
                    if (backColor.IsEmpty)
                    {
                        backColor = base.chart.Legend.Color;
                        if (backColor.IsEmpty)
                        {
                            backColor = base.chart.Panel.Color;
                        }
                    }
                    base.chart.SetBrushCanvas(g.Brush.color, aBrush, backColor);
                    this.DrawLegendShape(g, valueIndex, rect);
                }
            }
        }

        protected virtual void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            g.Rectangle(rect);
            if (g.Brush.ForegroundColor == base.chart.Legend.Color)
            {
                System.Drawing.Color color = g.Pen.Color;
                if (g.Brush.ForegroundColor == System.Drawing.Color.Black)
                {
                    g.Pen.Color = System.Drawing.Color.White;
                }
                g.Brush.Visible = false;
                g.Rectangle(rect);
                g.Pen.Color = color;
            }
        }

        protected internal virtual void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            this.marks.InternalDraw(valueIndex, this.ValueColor(valueIndex), st, aPosition);
        }

        private void DrawMarks()
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            Aspect aspect = base.chart.aspect;
            bool flag = ((aspect.view3D && !graphicsd.Supports3DText) && aspect.ZoomText) && (aspect.Zoom != 100);
            for (int i = this.firstVisible; i <= this.lastVisible; i++)
            {
                if (((i % this.marks.DrawEvery) == 0) && !this.IsNull(i))
                {
                    string markText = this.GetMarkText(i);
                    if (markText.Length != 0)
                    {
                        TextShape shape = this.Marks.MarkItem(i);
                        if (shape.Visible)
                        {
                            int num2;
                            graphicsd.Font = shape.Font;
                            if (flag)
                            {
                                ChartFont font = graphicsd.Font;
                                font.Size = Math.Max(1, Utils.Round((double) ((0.01 * aspect.Zoom) * font.Size)));
                            }
                            int num3 = base.chart.MultiLineTextWidth(markText, out num2);
                            num2 *= graphicsd.FontHeight;
                            graphicsd.Pen = shape.Pen;
                            if (shape.Pen.Visible)
                            {
                                int num4 = 2 * Utils.Round((float) shape.Pen.Width);
                                num3 += num4;
                                num2 += num4;
                            }
                            else
                            {
                                num2++;
                            }
                            SeriesMarks.Position aPosition = new SeriesMarks.Position();
                            aPosition.Width = num3;
                            aPosition.Height = num2;
                            aPosition.ArrowTo.X = this.CalcXPos(i);
                            aPosition.ArrowTo.Y = this.CalcYPos(i);
                            aPosition.ArrowFrom = aPosition.ArrowTo;
                            aPosition.LeftTop.X = aPosition.ArrowTo.X - (num3 / 2);
                            aPosition.LeftTop.Y = (aPosition.ArrowTo.Y - num2) + 1;
                            this.DrawMark(i, markText, aPosition);
                        }
                    }
                }
            }
        }

        private void DrawMarksSeries(Series s, ref bool ActiveRegion)
        {
            if ((s.Count > 0) && s.marks.Visible)
            {
                if (s.marks.bClip)
                {
                    this.ClipRegionCreate(ref ActiveRegion);
                }
                s.DrawMarks();
                if (s.marks.bClip)
                {
                    this.ClipRegionDone(ref ActiveRegion);
                }
            }
        }

        public void DrawSeries()
        {
            bool activeRegion = false;
            if (base.chart.Aspect.View3D && this.MoreSameZOrder())
            {
                if (this.FirstInZOrder())
                {
                    activeRegion = false;
                    int num = -1;
                    int num2 = -1;
                    for (int i = base.chart.Series.IndexOf(this); i < base.chart.Series.Count; i++)
                    {
                        Series series = base.chart[i];
                        if (series.Active && (series.ZOrder == this.ZOrder))
                        {
                            series.CalcFirstLastVisibleIndex();
                            if (series.firstVisible != -1)
                            {
                                num = (num == -1) ? series.firstVisible : Math.Max(num, series.firstVisible);
                                num2 = (num2 == -1) ? series.lastVisible : Math.Max(num2, series.lastVisible);
                                series.DoBeforeDrawValues();
                                if (base.chart.Aspect.ClipPoints && !activeRegion)
                                {
                                    this.ClipRegionCreate(ref activeRegion);
                                }
                            }
                        }
                    }
                    if (num != -1)
                    {
                        if (this.DrawValuesForward())
                        {
                            for (int j = num; j <= num2; j++)
                            {
                                this.DrawAllSeriesValue(j);
                            }
                        }
                        else
                        {
                            for (int k = num2; k >= num; k--)
                            {
                                this.DrawAllSeriesValue(k);
                            }
                        }
                    }
                    this.ClipRegionDone(ref activeRegion);
                    foreach (Series series2 in base.chart.Series)
                    {
                        if ((series2.Active && (series2.ZOrder == this.ZOrder)) && (series2.firstVisible != -1))
                        {
                            this.DrawMarksSeries(series2, ref activeRegion);
                            this.DoAfterDrawValues();
                        }
                    }
                }
            }
            else
            {
                this.CalcFirstLastVisibleIndex();
                if (this.firstVisible != -1)
                {
                    this.DoBeforeDrawValues();
                    if (this.UseAxis && base.chart.aspect.ClipPoints)
                    {
                        this.ClipRegionCreate(ref activeRegion);
                    }
                    this.Draw();
                    this.ClipRegionDone(ref activeRegion);
                    this.DrawMarksSeries(this, ref activeRegion);
                    this.DoAfterDrawValues();
                }
            }
        }

        protected virtual bool DrawSeriesForward(int valueIndex)
        {
            return true;
        }

        public virtual void DrawValue(int index)
        {
        }

        public virtual bool DrawValuesForward()
        {
            bool flag = true;
            if (this.mandatory != this.vyValues)
            {
                return !this.GetVertAxis.inverted;
            }
            flag = !this.GetHorizAxis.inverted;
            if ((base.chart.aspect.view3D && !base.chart.aspect.orthogonal) && (base.chart.aspect.rotation < 270))
            {
                flag = !flag;
            }
            return flag;
        }

        public void EndUpdate()
        {
            this.updating--;
            if (this.updating == 0)
            {
                this.RefreshSeries();
                this.Invalidate();
            }
        }

        private void FillFromDataSource()
        {
            if (this.datasource is Series)
            {
                this.AddValues(new Series[] { (Series) this.datasource });
            }
            else if (this.datasource is Array)
            {
                Array datasource = (Array) this.datasource;
                if ((datasource.Length > 0) && (datasource.GetValue(0) is Series))
                {
                    this.AddValues(datasource);
                }
            }
            else if (this.datasource is ArrayList)
            {
                ArrayList list = (ArrayList) this.datasource;
                if (list.Count > 0)
                {
                    object obj3 = list[0];
                    if (obj3 is Series)
                    {
                        Array array = new Series[list.Count];
                        list.CopyTo(array, 0);
                        this.AddValues(array);
                    }
                }
            }
            else if (this.datasource is SeriesSource)
            {
                ((SeriesSource) this.datasource).RefreshData();
            }
            else if (!DataSeriesSource.TryRefreshData(this))
            {
                throw new TeeChartException("Cannot bind to non-supported datasource: " + this.datasource.ToString());
            }
        }

        [Description("Add random sample values to series.")]
        public void FillSampleValues()
        {
            this.FillSampleValues(this.NumSampleValues());
        }

        [Description("Add numValues random sample values to series.")]
        public void FillSampleValues(int numValues)
        {
            if (numValues == 0)
            {
                numValues = this.INumSampleValues;
                if (numValues <= 0)
                {
                    numValues = this.NumSampleValues();
                }
            }
            this.BeginUpdate();
            this.Clear();
            this.AddSampleValues(numValues);
            this.CheckOrder();
            this.EndUpdate();
            this.manualData = false;
        }

        private bool FirstInZOrder()
        {
            if (!this.Active)
            {
                return false;
            }
            foreach (Series series in base.chart.Series)
            {
                if (series == this)
                {
                    break;
                }
                if (series.Active && (series.ZOrder == this.ZOrder))
                {
                    return false;
                }
            }
            return true;
        }

        protected internal virtual void GalleryChanged3D(bool Is3D)
        {
            base.chart.Aspect.View3D = Is3D;
        }

        private string GetAXValue(int valueIndex)
        {
            if (this.GetHorizAxis != null)
            {
                return this.vxValues.Value[valueIndex].ToString(this.valueFormat);
            }
            return this.GetHorizAxis.Labels.LabelValue(this.vxValues.Value[valueIndex]);
        }

        private string GetAYValue(int valueIndex)
        {
            return this.GetMarkValue(valueIndex).ToString(this.valueFormat);
        }

        public Image GetBitmapEditor()
        {
            return Utils.GetBitmapResource(base.GetType().Namespace + ".SeriesIcons." + base.GetType().Name + ".bmp");
        }

        private System.Drawing.Color GetDefaultColor(int valueIndex)
        {
            if (this.bColorEach)
            {
                System.Drawing.Color defaultColor = Graphics3D.GetDefaultColor(valueIndex);
                return Graphics3D.TransparentColor(this.bBrush.Transparency, defaultColor);
            }
            return this.Color;
        }

        internal int GetFields(ref Steema.TeeChart.Styles.ValueList otherList)
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.ValueList list in this.ValuesLists)
            {
                if (list.DataMember.Length != 0)
                {
                    num++;
                    if (this.mandatory != list)
                    {
                        otherList = list;
                    }
                }
            }
            return num;
        }

        protected internal virtual string GetMarkText(int valueIndex)
        {
            string aYValue;
            char ch = this.marks.MultiLine ? '\n' : ' ';
            switch (this.marks.markerStyle)
            {
                case MarksStyles.Value:
                    aYValue = this.GetAYValue(valueIndex);
                    break;

                case MarksStyles.Percent:
                    aYValue = this.marks.PercentString(valueIndex, false);
                    break;

                case MarksStyles.Label:
                    aYValue = this.LabelOrValue(valueIndex);
                    break;

                case MarksStyles.LabelPercent:
                    aYValue = this.LabelOrValue(valueIndex) + ch + this.marks.PercentString(valueIndex, false);
                    break;

                case MarksStyles.LabelValue:
                    aYValue = this.LabelOrValue(valueIndex) + ch + this.GetAYValue(valueIndex);
                    break;

                case MarksStyles.Legend:
                    aYValue = base.chart.FormattedValueLegend(this, valueIndex);
                    break;

                case MarksStyles.PercentTotal:
                    aYValue = this.marks.PercentString(valueIndex, true);
                    break;

                case MarksStyles.LabelPercentTotal:
                    aYValue = this.LabelOrValue(valueIndex) + ch + this.marks.PercentString(valueIndex, true);
                    break;

                case MarksStyles.XValue:
                    aYValue = this.GetAXValue(valueIndex);
                    break;

                case MarksStyles.XY:
                    aYValue = this.GetAXValue(valueIndex) + ch + this.GetAYValue(valueIndex);
                    break;

                default:
                    aYValue = "";
                    break;
            }
            this.DoGetSeriesMark(valueIndex, ref aYValue);
            return aYValue;
        }

        public double GetMarkValue(int valueIndex)
        {
            return this.mandatory[valueIndex];
        }

        protected internal virtual double GetOriginValue(int valueIndex)
        {
            return this.GetMarkValue(valueIndex);
        }

        protected virtual System.Drawing.Color GetSeriesColor()
        {
            return this.bBrush.color;
        }

        internal Steema.TeeChart.Styles.ValueList GetYValueList(string AListName)
        {
            AListName = AListName.ToUpper();
            for (int i = 2; i < this.valuesList.Count; i++)
            {
                if (AListName == this.valuesList[i].Name.ToUpper())
                {
                    return this.valuesList[i];
                }
            }
            return this.vyValues;
        }

        internal bool HasClickEvents()
        {
            if (this.Click == null)
            {
                return (this.DblClick != null);
            }
            return true;
        }

        internal bool HasDataSource(object source)
        {
            if (this.datasource == null)
            {
                return false;
            }
            if (this.datasource == source)
            {
                return true;
            }
            ArrayList list = this.DataSourceArray();
            if (list == null)
            {
                return false;
            }
            return (list.IndexOf(source) != -1);
        }

        private void InsertLabel(int valueIndex, string text)
        {
            if (text.Length != 0)
            {
                this.Labels[valueIndex] = text;
            }
        }

        public bool IsNull(int index)
        {
            return (((this.iColors != null) && (this.iColors.Count > index)) && (this.iColors[index] == System.Drawing.Color.Transparent));
        }

        protected virtual bool IsValidSeriesSource(Series value)
        {
            return true;
        }

        public virtual bool IsValidSourceOf(Series value)
        {
            return (this != value);
        }

        private string LabelOrValue(int valueIndex)
        {
            string str = (this.sLabels == null) ? "" : this.sLabels[valueIndex].ToString();
            if (str.Length == 0)
            {
                str = this.GetMarkValue(valueIndex).ToString(this.valueFormat);
            }
            return str;
        }

        protected internal virtual System.Drawing.Color LegendItemColor(int index)
        {
            return this.ValueColor(index);
        }

        public virtual string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            int valueIndex = this.LegendToValueIndex(legendIndex);
            string str = (this.sLabels == null) ? "" : this.sLabels[valueIndex];
            if (legendTextStyle != LegendTextStyles.Plain)
            {
                double markValue = this.GetMarkValue(valueIndex);
                switch (legendTextStyle)
                {
                    case LegendTextStyles.LeftValue:
                        if (str.Length != 0)
                        {
                            str = '\x0006' + str;
                        }
                        return (this.ValueToStr(markValue) + str);

                    case LegendTextStyles.RightValue:
                        if (str.Length != 0)
                        {
                            str = str + '\x0006';
                        }
                        return (str + this.ValueToStr(markValue));

                    case LegendTextStyles.LeftPercent:
                        if (str.Length != 0)
                        {
                            str = '\x0006' + str;
                        }
                        return (this.CalcPercentSt(markValue) + str);

                    case LegendTextStyles.RightPercent:
                        if (str.Length != 0)
                        {
                            str = str + '\x0006';
                        }
                        return (str + this.CalcPercentSt(markValue));

                    case LegendTextStyles.XValue:
                        return this.CalcXValue(valueIndex);

                    case LegendTextStyles.Value:
                        return this.ValueToStr(markValue);

                    case LegendTextStyles.Percent:
                        return this.CalcPercentSt(markValue);

                    case LegendTextStyles.XAndValue:
                        return (this.CalcXValue(valueIndex) + '\x0006' + this.ValueToStr(markValue));

                    case LegendTextStyles.XAndPercent:
                        return (this.CalcXValue(valueIndex) + '\x0006' + this.CalcPercentSt(markValue));
                }
            }
            return str;
        }

        protected internal virtual int LegendToValueIndex(int legendIndex)
        {
            return legendIndex;
        }

        public int MaxMarkWidth()
        {
            int num = 0;
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                num = Math.Max(num, this.Marks.TextWidth(i));
            }
            return num;
        }

        public virtual double MaxXValue()
        {
            return this.vxValues.Maximum;
        }

        public virtual double MaxYValue()
        {
            return this.vyValues.Maximum;
        }

        public virtual double MaxZValue()
        {
            return (double) this.ZOrder;
        }

        public virtual double MinXValue()
        {
            return this.vxValues.Minimum;
        }

        public virtual double MinYValue()
        {
            return this.vyValues.Minimum;
        }

        public virtual double MinZValue()
        {
            return (double) this.ZOrder;
        }

        protected virtual bool MoreSameZOrder()
        {
            if (base.chart.Aspect.ApplyZOrder)
            {
                foreach (Series series in base.chart.series)
                {
                    if (((series != this) && series.bActive) && (!series.HasZValues && (series.ZOrder == this.ZOrder)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected internal virtual void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref System.Windows.Forms.Cursor c)
        {
        }

        public static Series NewFromType(System.Type type)
        {
            return (Series) Activator.CreateInstance(type);
        }

        private void NotifyNewValue(Series Sender, int valueIndex)
        {
            if (this.bActive)
            {
                this.Repaint();
            }
        }

        private void NotifyValue(int valueIndex)
        {
        }

        protected internal virtual int NumSampleValues()
        {
            return 0x19;
        }

        internal void OnClick(MouseEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        internal void OnDblClick(MouseEventArgs e)
        {
            if (this.DblClick != null)
            {
                this.DblClick(this, e);
            }
        }

        protected internal virtual void OnDisposing()
        {
        }

        protected internal void PaintLegend(Graphics g, Rectangle r)
        {
            Chart chart;
            if (base.chart == null)
            {
                chart = new Chart();
                chart.AutoRepaint = false;
                base.Chart = chart;
            }
            else
            {
                chart = null;
                base.Chart.AutoRepaint = false;
            }
            try
            {
                Graphics3DGdiPlus plus = new Graphics3DGdiPlus(base.chart);
                plus.g = g;
                this.DrawLegend(plus, -1, r);
            }
            finally
            {
                if (base.chart == chart)
                {
                    base.Chart = null;
                    chart.Dispose();
                }
                else
                {
                    base.Chart.AutoRepaint = true;
                }
            }
        }

        internal virtual void PrepareForGallery(bool IsEnabled)
        {
            this.FillSampleValues(4);
            this.Marks.Visible = false;
            this.Marks.Font.Size = 7;
            this.Marks.ArrowLength = 4;
            this.Marks.DrawEvery = 2;
            this.Marks.Callout.Length = 4;
            this.ColorEach = false;
            if (IsEnabled)
            {
                if (base.chart.Series.IndexOf(this) == 0)
                {
                    this.Color = System.Drawing.Color.Red;
                }
                else
                {
                    this.Color = System.Drawing.Color.Blue;
                }
            }
            else
            {
                this.Color = System.Drawing.Color.Silver;
            }
        }

        protected virtual void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref System.Drawing.Color backColor, ref ChartBrush aBrush)
        {
        }

        protected SeriesRandom RandomBounds(int numValues)
        {
            double minY;
            double num2;
            SeriesRandom random = new SeriesRandom();
            random.MinY = 0.0;
            double num3 = 1000.0;
            if ((base.chart != null) && (base.chart.GetMaxValuesCount() > 0))
            {
                random.MinY = base.chart.MinYValue(this.GetVertAxis);
                num3 = base.chart.MaxYValue(this.GetVertAxis);
                if (num3 == random.MinY)
                {
                    if (num3 == 0.0)
                    {
                        num3 = 1000.0;
                    }
                    else
                    {
                        num3 = 2.0 * random.MinY;
                    }
                }
                minY = base.chart.MinXValue(this.GetHorizAxis);
                num2 = base.chart.MaxXValue(this.GetHorizAxis);
                if (num2 == minY)
                {
                    if (num2 == 0.0)
                    {
                        num2 = numValues;
                    }
                    else
                    {
                        num2 = 2.0 * minY;
                    }
                }
                if (!this.yMandatory)
                {
                    double num4 = minY;
                    minY = random.MinY;
                    random.MinY = num4;
                    num4 = num2;
                    num2 = num3;
                    num3 = num4;
                }
            }
            else if (this.vxValues.DateTime)
            {
                minY = Utils.DateTime(DateTime.Now);
                num2 = (minY + numValues) - 1.0;
            }
            else
            {
                minY = 0.0;
                num2 = numValues - 1;
            }
            random.StepX = num2 - minY;
            if (numValues > 1)
            {
                random.StepX /= (double) (numValues - 1);
            }
            random.DifY = num3 - random.MinY;
            if (random.DifY > 2147483647.0)
            {
                random.DifY = 2147483647.0;
            }
            else if (random.DifY < -2147483647.0)
            {
                random.DifY = -2147483647.0;
            }
            random.tmpY = random.MinY + (random.DifY * random.Random());
            random.tmpX = minY;
            return random;
        }

        protected void RecalcGetAxis()
        {
            if (base.chart != null)
            {
                this.GetHorizAxis = base.chart.Axes.Bottom;
                if (this.horizAxis == HorizontalAxis.Top)
                {
                    this.GetHorizAxis = base.chart.Axes.Top;
                }
                else if ((this.horizAxis == HorizontalAxis.Custom) && (this.customHorizAxis != null))
                {
                    this.GetHorizAxis = this.customHorizAxis;
                }
                this.GetVertAxis = base.chart.Axes.Left;
                if (this.vertAxis == VerticalAxis.Right)
                {
                    this.GetVertAxis = base.chart.Axes.Right;
                }
                else if ((this.vertAxis == VerticalAxis.Custom) && (this.customVertAxis != null))
                {
                    this.GetVertAxis = this.customVertAxis;
                }
            }
            else
            {
                this.GetHorizAxis = null;
                this.GetVertAxis = null;
            }
        }

        public void RefreshSeries()
        {
            if (base.chart != null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (series.HasDataSource(this))
                    {
                        series.CheckDataSource();
                    }
                }
            }
        }

        public void Repaint()
        {
            this.Invalidate();
        }

        protected bool SameClass(Series s)
        {
            return (base.GetType() == s.GetType());
        }

        protected virtual void SetActive(bool value)
        {
            base.SetBooleanProperty(ref this.bActive, value);
            if (base.chart != null)
            {
                base.chart.BroadcastEvent(this, SeriesEventStyle.ChangeActive);
            }
        }

        protected override void SetChart(Chart value)
        {
            if (base.chart != value)
            {
                if (base.chart != null)
                {
                    base.chart.series.Remove(this);
                }
                base.SetChart(value);
                this.bBrush.Chart = base.chart;
                if (base.chart != null)
                {
                    base.chart.series.Add(this);
                    this.Added();
                }
                if (this.marks != null)
                {
                    this.marks.Chart = base.chart;
                    this.marks.Callout.Chart = base.chart;
                }
                if (this.function != null)
                {
                    this.function.Chart = base.chart;
                }
                if (base.chart != null)
                {
                    base.chart.Invalidate();
                }
            }
        }

        protected virtual void SetColorEach(bool value)
        {
            base.SetBooleanProperty(ref this.bColorEach, value);
            if (!this.bColorEach)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this.IsNull(i))
                    {
                        return;
                    }
                }
                this.iColors = null;
                this.Invalidate();
            }
        }

        protected void SetHorizontal()
        {
            this.mandatory = this.vxValues;
            this.notMandatory = this.vyValues;
            this.yMandatory = false;
        }

        public void SetNull(int valueIndex)
        {
            this.Colors[valueIndex] = System.Drawing.Color.Transparent;
        }

        protected virtual void SetSeriesColor(System.Drawing.Color value)
        {
            this.bBrush.Color = value;
            if (base.chart != null)
            {
                base.chart.BroadcastEvent(this, SeriesEventStyle.ChangeColor);
            }
        }

        protected internal virtual void SetSubGallery(int index)
        {
        }

        protected void SetValueList(Steema.TeeChart.Styles.ValueList l, Steema.TeeChart.Styles.ValueList value)
        {
            l.Assign(value);
            this.Repaint();
        }

        public void SortByLabels()
        {
            this.SortByLabels(ValueListOrder.Ascending);
        }

        public void SortByLabels(ValueListOrder order)
        {
            if (order != ValueListOrder.None)
            {
                this.ILabelOrder = order;
                Utils.Sort(0, this.Count - 1, new Utils.CompareEventHandler(this.CompareLabelIndex), new Utils.SwapEventHandler(this.SwapValueIndex));
                this.ILabelOrder = ValueListOrder.None;
                this.notMandatory.FillSequence();
                this.Invalidate();
            }
        }

        internal virtual void SwapValueIndex(int a, int b)
        {
            foreach (Steema.TeeChart.Styles.ValueList list in this.valuesList)
            {
                list.Exchange(a, b);
            }
            if (this.iColors != null)
            {
                this.iColors.Exchange(a, b);
            }
            if (this.sLabels != null)
            {
                this.sLabels.Exchange(a, b);
            }
        }

        [Obsolete("Please use ToString() method.")]
        public string TitleOrName()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            string title = this.title;
            if (title.Length != 0)
            {
                return title;
            }
            ISite site = this.Site;
            if (site != null)
            {
                title = site.Name;
            }
            if (title.Length != 0)
            {
                return title;
            }
            if (base.chart != null)
            {
                return (Texts.Series + " " + base.chart.Series.IndexOf(this).ToString());
            }
            return base.ToString();
        }

        private void TryDrawSeries(Series s, int valueIndex)
        {
            if ((s.bActive && (s.ZOrder == this.ZOrder)) && (valueIndex < s.Count))
            {
                s.DrawValue(valueIndex);
            }
        }

        public virtual System.Drawing.Color ValueColor(int valueIndex)
        {
            if ((this.iColors != null) && (this.iColors.Count > valueIndex))
            {
                System.Drawing.Color color = this.iColors[valueIndex];
                if (!color.IsEmpty)
                {
                    return color;
                }
            }
            return this.GetDefaultColor(valueIndex);
        }

        public string ValueMarkText(int index)
        {
            return this.GetMarkText(index);
        }

        private string ValueToStr(double tmpValue)
        {
            if (this.mandatory.DateTime)
            {
                return Utils.DateTimeDefaultFormat(tmpValue);
            }
            return tmpValue.ToString(this.valueFormat);
        }

        public double XScreenToValue(int screenPos)
        {
            return this.GetHorizAxis.CalcPosPoint(screenPos);
        }

        public double YScreenToValue(int screenPos)
        {
            return this.GetVertAxis.CalcPosPoint(screenPos);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Shows or hides this series."), DefaultValue(true)]
        public bool Active
        {
            get
            {
                return this.bActive;
            }
            set
            {
                this.SetActive(value);
            }
        }

        [DefaultValue(typeof(System.Drawing.Color), ""), Description("Default color for all points. See also: ColorEach property."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get
            {
                return this.GetSeriesColor();
            }
            set
            {
                this.SetSeriesColor(value);
            }
        }

        [Category("Appearance"), DefaultValue(false), Description("Draw points with different preset Colors.")]
        public bool ColorEach
        {
            get
            {
                return this.bColorEach;
            }
            set
            {
                this.SetColorEach(value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Please use ColorEach property."), DefaultValue(false), Browsable(false)]
        public bool ColorEachPoint
        {
            get
            {
                return this.bColorEach;
            }
            set
            {
                this.ColorEach = value;
            }
        }

        [DefaultValue(""), Description("Gets or sets the Datasource Color Field")]
        public string ColorMember
        {
            get
            {
                return this.colorMember;
            }
            set
            {
                if (this.colorMember != value)
                {
                    this.colorMember = value;
                    this.CheckDataSource();
                }
            }
        }

        [Browsable(false), DefaultValue((string) null)]
        public ColorList Colors
        {
            get
            {
                if (this.iColors == null)
                {
                    this.iColors = new ColorList((this.Count > 0) ? this.Count : Steema.TeeChart.Styles.ValueList.DefaultCapacity);
                }
                return this.iColors;
            }
            set
            {
                this.iColors = value;
            }
        }

        [Browsable(false), Description("Returns the number of points in the Series.")]
        public int Count
        {
            get
            {
                return this.mandatory.Count;
            }
        }

        [Description("Cursor displayed when mouse is over a series point."), Category("Appearance"), DefaultValue(typeof(System.Windows.Forms.Cursor), "Default")]
        public System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return this.cursor;
            }
            set
            {
                this.cursor = value;
            }
        }

        [DefaultValue((string) null), Browsable(false), Description("")]
        public Axis CustomHorizAxis
        {
            get
            {
                return this.customHorizAxis;
            }
            set
            {
                this.customHorizAxis = value;
                this.horizAxis = (value != null) ? HorizontalAxis.Custom : HorizontalAxis.Bottom;
                this.RecalcGetAxis();
                this.Repaint();
            }
        }

        [DefaultValue((string) null), Browsable(false), Description("")]
        public Axis CustomVertAxis
        {
            get
            {
                return this.customVertAxis;
            }
            set
            {
                this.customVertAxis = value;
                this.vertAxis = (value != null) ? VerticalAxis.Custom : VerticalAxis.Left;
                this.RecalcGetAxis();
                this.Repaint();
            }
        }

        [Editor(typeof(SeriesDataSourceEditor), typeof(UITypeEditor)), DefaultValue((string) null), Description("Object to load data from.")]
        public object DataSource
        {
            get
            {
                return this.datasource;
            }
            set
            {
                if (this.datasource != value)
                {
                    if (this.datasource is SeriesSource)
                    {
                        ((SeriesSource) this.datasource).Series = null;
                    }
                    this.datasource = value;
                    if (this.datasource is SeriesSource)
                    {
                        ((SeriesSource) this.datasource).Series = this;
                    }
                    if ((!(this.datasource is Series) && !(this.datasource is Array)) && !(this.datasource is ArrayList))
                    {
                        this.Function = null;
                    }
                    this.CheckDataSource();
                }
            }
        }

        [Description("Sets Depth of the series points or interconnecting lines."), DefaultValue(-1)]
        public int Depth
        {
            get
            {
                return this.depth;
            }
            set
            {
                base.SetIntegerProperty(ref this.depth, value);
            }
        }

        [Description("Gets descriptive text."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Description
        {
            get
            {
                return "";
            }
        }

        [Description(""), Browsable(false)]
        public int EndZ
        {
            get
            {
                return this.endZ;
            }
        }

        [Description("Returns the index of the Series' first visible point")]
        public int FirstVisibleIndex
        {
            get
            {
                return this.firstVisible;
            }
        }

        [DefaultValue((string) null), Description("Function object to calculate values.")]
        public Steema.TeeChart.Functions.Function Function
        {
            get
            {
                return this.function;
            }
            set
            {
                this.function = value;
                if (this.function != null)
                {
                    this.function.series = this;
                    IContainer chartContainer = null;
                    if (base.chart != null)
                    {
                        chartContainer = base.chart.ChartContainer;
                    }
                    if (chartContainer != null)
                    {
                        chartContainer.Add(this.function);
                    }
                }
                this.CheckDataSource();
            }
        }

        [DefaultValue(1), Description("Horizontal axis to associate to this Series.")]
        public HorizontalAxis HorizAxis
        {
            get
            {
                return this.horizAxis;
            }
            set
            {
                if (this.horizAxis != value)
                {
                    this.horizAxis = value;
                    if (this.horizAxis != HorizontalAxis.Custom)
                    {
                        this.customHorizAxis = null;
                    }
                    this.RecalcGetAxis();
                    this.Invalidate();
                }
            }
        }

        [Description("Point characteristics")]
        public SeriesXYPoint this[int index]
        {
            get
            {
                return new SeriesXYPoint(this, index);
            }
        }

        [Description("Gets or sets the Datasource Label Field"), DefaultValue(""), Editor(typeof(LabelMemberEditor), typeof(UITypeEditor))]
        public string LabelMember
        {
            get
            {
                return this.labelMember;
            }
            set
            {
                if (this.labelMember != value)
                {
                    this.labelMember = value;
                    this.CheckDataSource();
                }
            }
        }

        [Description(""), Browsable(false)]
        public StringList Labels
        {
            get
            {
                if (this.sLabels == null)
                {
                    this.sLabels = new StringList((this.Count > 0) ? this.Count : Steema.TeeChart.Styles.ValueList.DefaultCapacity);
                }
                return this.sLabels;
            }
            set
            {
                this.sLabels = value;
            }
        }

        [Description("Returns the index of the Series' last visible point")]
        public int LastVisibleIndex
        {
            get
            {
                return this.lastVisible;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines all necessary properties to draw a mark.")]
        public SeriesMarks Marks
        {
            get
            {
                if (this.marks == null)
                {
                    this.marks = new SeriesMarks(this);
                }
                return this.marks;
            }
        }

        [Description(""), Browsable(false)]
        public int MiddleZ
        {
            get
            {
                return this.middleZ;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("")]
        public virtual int NumGallerySeries
        {
            get
            {
                return 2;
            }
        }

        [DefaultValue("##0.## %"), Description("Sets the Format to display point values as percentage.")]
        public string PercentFormat
        {
            get
            {
                return this.percentFormat;
            }
            set
            {
                base.SetStringProperty(ref this.percentFormat, value);
            }
        }

        [Obsolete("Use the Series.Color property."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public System.Drawing.Color SeriesColor
        {
            get
            {
                return this.Color;
            }
            set
            {
                this.Color = value;
            }
        }

        [DefaultValue(true), Description("Displays this Series Title in Legend.")]
        public bool ShowInLegend
        {
            get
            {
                return this.showInLegend;
            }
            set
            {
                base.SetBooleanProperty(ref this.showInLegend, value);
            }
        }

        [Browsable(false), Description("")]
        public int StartZ
        {
            get
            {
                return this.startZ;
            }
        }

        [DefaultValue(""), Description("Series description to show in Legend and editors.")]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                base.SetStringProperty(ref this.title, value);
                if (base.chart != null)
                {
                    base.chart.BroadcastEvent(this, SeriesEventStyle.ChangeTitle);
                }
            }
        }

        [Description("Determines the Format to display point values."), DefaultValue("#,##0.###")]
        public string ValueFormat
        {
            get
            {
                return this.valueFormat;
            }
            set
            {
                base.SetStringProperty(ref this.valueFormat, value);
            }
        }

        [Browsable(false), Description("")]
        public Steema.TeeChart.Styles.ValuesLists ValuesLists
        {
            get
            {
                return this.valuesList;
            }
        }

        [DefaultValue(0), Description("Vertical axis to associate to this Series.")]
        public VerticalAxis VertAxis
        {
            get
            {
                return this.vertAxis;
            }
            set
            {
                if (this.vertAxis != value)
                {
                    this.vertAxis = value;
                    if (this.vertAxis != VerticalAxis.Custom)
                    {
                        this.customVertAxis = null;
                    }
                    this.RecalcGetAxis();
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(true), Description("Shows or hides this series.")]
        public bool Visible
        {
            get
            {
                return this.bActive;
            }
            set
            {
                this.SetActive(value);
            }
        }

        [Description("Values defining horizontal point positions."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Styles.ValueList XValues
        {
            get
            {
                return this.vxValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Values defining vertical point positions.")]
        public Steema.TeeChart.Styles.ValueList YValues
        {
            get
            {
                return this.vyValues;
            }
        }

        [DefaultValue(-1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Determines where on the depth axis the Series is drawn.")]
        public int ZOrder
        {
            get
            {
                if (this.zOrder != -1)
                {
                    return this.zOrder;
                }
                return this.iZOrder;
            }
            set
            {
                base.SetIntegerProperty(ref this.zOrder, value);
                this.iZOrder = (this.zOrder == -1) ? 0 : this.zOrder;
            }
        }

        public delegate void GetSeriesMarkEventHandler(Series series, GetSeriesMarkEventArgs e);

        internal sealed class LabelMemberEditor : UITypeEditor
        {
            private IWindowsFormsEditorService edSvc = null;

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (((context != null) && (context.Instance != null)) && (provider != null))
                {
                    this.edSvc = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                    if (this.edSvc != null)
                    {
                        ListBox combo = new ListBox();
                        combo.SelectedIndexChanged += new EventHandler(this.ValueChanged);
                        DataSeriesSource.FillFields(this.Source(context.Instance), combo, null);
                        combo.SelectedIndex = combo.Items.IndexOf(value);
                        this.edSvc.DropDownControl(combo);
                        value = combo.SelectedItem;
                    }
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                if (((context != null) && (context.Instance != null)) && this.SourceIsData(context.Instance))
                {
                    return UITypeEditorEditStyle.DropDown;
                }
                return base.GetEditStyle(context);
            }

            private object Source(object instance)
            {
                if (instance is Series)
                {
                    return ((Series) instance).DataSource;
                }
                if (instance is Steema.TeeChart.Styles.ValueList)
                {
                    return ((Steema.TeeChart.Styles.ValueList) instance).series.DataSource;
                }
                return null;
            }

            private bool SourceIsData(object instance)
            {
                object c = this.Source(instance);
                return ((c != null) && DataSeriesSource.IsValidSource(c));
            }

            private void ValueChanged(object sender, EventArgs e)
            {
                if (this.edSvc != null)
                {
                    this.edSvc.CloseDropDown();
                }
            }
        }

        private sealed class SeriesDataSourceEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Series instance = (Series) context.Instance;
                bool flag = SeriesEditor.ShowEditor(instance, ChartEditorTabs.SeriesDataSource);
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        internal sealed class SeriesDesigner : ComponentDesigner
        {
            public SeriesDesigner()
            {
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.OnEdit)));
                this.Verbs.Add(new DesignerVerb(Texts.DataSource, new EventHandler(this.OnDataSource)));
            }

            private void OnDataSource(object sender, EventArgs e)
            {
                if (SeriesEditor.ShowEditor(this.Series, ChartEditorTabs.SeriesDataSource))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }

            private void OnEdit(object sender, EventArgs e)
            {
                if (SeriesEditor.ShowEditor(this.Series))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }

            private Steema.TeeChart.Styles.Series Series
            {
                get
                {
                    return (Steema.TeeChart.Styles.Series) base.Component;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SeriesRandom
        {
            private static System.Random r;
            public double tmpX;
            public double StepX;
            public double tmpY;
            public double MinY;
            public double DifY;
            public double Random()
            {
                return r.NextDouble();
            }

            static SeriesRandom()
            {
                r = new System.Random(DateTime.Now.Millisecond);
            }
        }

        internal protected delegate Chart SubGalleryEventHandler(string Name);
    }
}

