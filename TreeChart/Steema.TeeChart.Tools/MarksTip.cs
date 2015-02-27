namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Description("Displays \"tips\" or \"hints\" when the end-user moves or clicks the mouse over a series point."), ToolboxBitmap(typeof(MarksTip), "ToolsIcons.MarksTip.bmp")]
    public class MarksTip : ToolSeries
    {
        private MarksTipMouseAction mouseAction;
        private MarksStyles style;

        public event MarksTipGetTextEventHandler GetText;

        public MarksTip() : this(null)
        {
        }

        public MarksTip(Chart c) : base(c)
        {
            this.mouseAction = MarksTipMouseAction.Move;
            this.style = MarksStyles.Label;
        }

        private void DoGetText(ref string tmpText)
        {
            if (this.GetText != null)
            {
                MarksTipGetTextEventArgs e = new MarksTipGetTextEventArgs(tmpText);
                this.GetText(this, e);
                tmpText = e.Text;
            }
        }

        private Series FindClickedSeries(int x, int y, out int index)
        {
            foreach (Series series in base.chart.Series)
            {
                index = series.Clicked(x, y);
                if (index != -1)
                {
                    return series;
                }
            }
            index = -1;
            return null;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            base.MouseEvent(kind, e, ref c);
            if (((this.MouseAction == MarksTipMouseAction.Move) && (kind == MouseEventKinds.Move)) || ((this.MouseAction == MarksTipMouseAction.Click) && (kind == MouseEventKinds.Down)))
            {
                Series iSeries = null;
                int index = -1;
                if (base.iSeries != null)
                {
                    iSeries = base.iSeries;
                    index = iSeries.Clicked(e.X, e.Y);
                }
                else
                {
                    iSeries = this.FindClickedSeries(e.X, e.Y, out index);
                }
                if (index != -1)
                {
                    MarksStyles style = iSeries.Marks.Style;
                    bool autoRepaint = base.chart.AutoRepaint;
                    base.chart.AutoRepaint = false;
                    iSeries.Marks.Style = this.style;
                    string tmpText = iSeries.ValueMarkText(index);
                    this.DoGetText(ref tmpText);
                    if (base.chart.ToolTip.Text != tmpText)
                    {
                        base.chart.ToolTip.Text = tmpText;
                        base.chart.ToolTip.Show();
                    }
                    iSeries.Marks.Style = style;
                    base.chart.AutoRepaint = autoRepaint;
                }
                else
                {
                    bool flag = true;
                    foreach (Steema.TeeChart.Tools.Tool tool in base.chart.Tools)
                    {
                        if ((tool != this) && (tool is MarksTip))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        base.chart.ToolTip.Text = "";
                        base.chart.ToolTip.Hide();
                    }
                }
            }
        }

        protected override void SetActive(bool value)
        {
            base.SetActive(value);
            if (!base.Active)
            {
                base.chart.ToolTip.Text = "";
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.MarksTipTool;
            }
        }

        [DefaultValue(0), Description("Activates Mark Tip on mouse move or click.")]
        public MarksTipMouseAction MouseAction
        {
            get
            {
                return this.mouseAction;
            }
            set
            {
                this.mouseAction = value;
                base.chart.ToolTip.Text = "";
            }
        }

        [Description("Sets the time lag before the Tool Tip appears."), DefaultValue(500)]
        public int MouseDelay
        {
            get
            {
                return base.chart.ToolTip.InitialDelay;
            }
            set
            {
                base.chart.ToolTip.InitialDelay = value;
            }
        }

        [Description("Defines the text format of the Mark ToolTip."), DefaultValue(2)]
        public MarksStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.MarksTipSummary;
            }
        }
    }
}

