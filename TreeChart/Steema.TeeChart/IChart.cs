namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    internal interface IChart
    {
        void CheckBackground(object sender, MouseEventArgs e);
        bool CheckClickSeries();
        bool CheckGetAxisLabelAssigned();
        void CheckTitle(Title ATitle, MouseEventArgs e);
        void DoAfterDraw();
        bool DoAllowScroll(Axis a, double Delta, ref double Min, ref double Max);
        void DoBeforeDraw();
        void DoBeforeDrawAxes();
        void DoBeforeDrawSeries();
        void DoChartPrint(object sender, PrintPageEventArgs e);
        void DoClickAxis(Axis a, MouseEventArgs e);
        void DoClickLegend(object sender, MouseEventArgs e);
        void DoClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e);
        void DoGetAxisLabel(object sender, Series s, int valueIndex, ref string labelText);
        void DoGetLegendPos(object sender, int index, ref int X, ref int Y, ref int XColor);
        void DoGetLegendRectangle(object sender, ref Rectangle rect);
        void DoGetLegendText(object sender, LegendStyles LegendStyle, int Index, ref string Text);
        void DoGetNextAxisLabel(object sender, int labelIndex, ref double labelValue, ref bool Stop);
        void DoInvalidate();
        void DoScroll(object sender, EventArgs e);
        void DoSetControlStyle();
        void DoUndoneZoom(object sender, EventArgs e);
        void DoZoomed(object sender, EventArgs e);
        Form FindParentForm();
        IContainer GetContainer();
        Control GetControl();
        Cursor GetCursor();
        Point PointToScreen(Point p);
        void RefreshControl();
        void SetChart(Chart c);
        void SetCursor(Cursor c);
    }
}

