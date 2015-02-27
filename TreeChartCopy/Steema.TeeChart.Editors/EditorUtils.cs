namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public sealed class EditorUtils
    {
        public static System.Type[] SeriesEditorsOf = new System.Type[] { 
            typeof(CustomSeries), typeof(PointSeries), typeof(AreaSeries), typeof(FastLineSeries), typeof(CustomSeries), typeof(BarSeries), typeof(BarSeries), typeof(PieSeries), typeof(ShapeSeries), typeof(ArrowSeries), typeof(PointSeries), typeof(GanttSeries), typeof(CandleSeries), typeof(DonutSeries), typeof(VolumeSeries), typeof(BarSeries), 
            typeof(Point3DSeries), typeof(PolarSeries), typeof(PolarSeries), typeof(ClockSeries), typeof(PolarSeries), typeof(PyramidSeries), typeof(SurfaceSeries), typeof(PointSeries), typeof(BarSeries), typeof(ColorGridSeries), typeof(WaterfallSeries), typeof(HistogramSeries), typeof(ErrorSeries), typeof(ErrorSeries), typeof(ContourSeries), typeof(SmithSeries), 
            typeof(BezierSeries), typeof(CalendarSeries), typeof(HighLow), typeof(TriSurfaceSeries), typeof(FunnelSeries), typeof(BoxSeriesEditor), typeof(BoxSeriesEditor), typeof(AreaSeries), typeof(TowerSeries), typeof(PointFigure), typeof(GaugesSeries), typeof(Vector3DSeries)
         };
        public static System.Type[] ToolEditorsOf = new System.Type[] { 
            typeof(AnnotationEditor), typeof(AnnotationEditor), typeof(RotateEditor), typeof(ImageEditor), typeof(CursorEditor), typeof(DragMarksEditor), typeof(AxisArrowEditor), typeof(ColorLineEditor), typeof(ColorBandEditor), typeof(DrawLineEditor), typeof(NearestPointEditor), typeof(MarksTipEditor), typeof(ExtraLegendEditor), typeof(DragPointEditor), typeof(SeriesAnimationEditor), typeof(PieToolEditor), 
            typeof(GridTransposeEditor), typeof(GanttToolEditor), typeof(GridBandEditor)
         };

        private EditorUtils()
        {
        }

        internal static void AddDefaultValueFormats(IList aItems)
        {
            aItems.Add("#,##0.###");
            aItems.Add("0");
            aItems.Add("0.0");
            aItems.Add("0.#");
            aItems.Add("#.#");
            aItems.Add("#,##0.00;(#,##0.00)");
            aItems.Add("00e-0");
            aItems.Add("#.0 \"x10\" E+0");
            aItems.Add("#.# x10E-#");
        }

        public static bool EditFont(ChartFont font)
        {
            using (FontDialog dialog = new FontDialog())
            {
                dialog.Font = font.DrawingFont;
                dialog.ShowColor = true;
                dialog.Color = font.Color;
                bool flag = dialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    font.Name = dialog.Font.Name;
                    font.Size = (int) dialog.Font.Size;
                    font.Bold = dialog.Font.Bold;
                    font.Italic = dialog.Font.Italic;
                    font.Underline = dialog.Font.Underline;
                    font.Strikeout = dialog.Font.Strikeout;
                    font.Color = dialog.Color;
                }
                return flag;
            }
        }

        public static void FillCursors(ComboBox combo, Cursor cursor)
        {
            if (combo.Items.Count == 0)
            {
                foreach (PropertyInfo info in typeof(Cursors).GetProperties())
                {
                    if (info.PropertyType.Equals(typeof(Cursor)))
                    {
                        combo.Items.Add(info.Name);
                    }
                }
            }
            string str = cursor.ToString();
            int index = str.IndexOf(":");
            if (index > 0)
            {
                str = str.Remove(0, index + 2);
                index = str.IndexOf("]");
                if (index > 0)
                {
                    str = str.Substring(0, index);
                }
                combo.SelectedIndex = combo.Items.IndexOf(str);
            }
            else
            {
                combo.SelectedIndex = -1;
            }
        }

        internal static void GetUpDown(Button bUp, Button bDown)
        {
            string str = "Steema.TeeChart.Editors.Images.";
            bUp.Image = Utils.GetBitmapResource(str + "Up.bmp");
            bDown.Image = Utils.GetBitmapResource(str + "Down.bmp");
        }

        internal static int ImageModeToIndex(ImageMode mode)
        {
            if (mode == ImageMode.Stretch)
            {
                return 0;
            }
            if (mode == ImageMode.Tile)
            {
                return 1;
            }
            if (mode == ImageMode.Center)
            {
                return 2;
            }
            return 3;
        }

        internal static ImageMode IndexToImageMode(int index)
        {
            if (index == 0)
            {
                return ImageMode.Stretch;
            }
            if (index == 1)
            {
                return ImageMode.Tile;
            }
            if (index == 2)
            {
                return ImageMode.Center;
            }
            return ImageMode.Normal;
        }

        public static void InsertForm(Form f, Control c)
        {
            if (c != null)
            {
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ControlBox = false;
                c.Controls.Add(f);
                f.Show();
            }
        }

        public static MouseButtons MouseButtonFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return MouseButtons.Left;

                case 1:
                    return MouseButtons.Middle;

                case 2:
                    return MouseButtons.Right;

                case 3:
                    return MouseButtons.XButton1;

                case 4:
                    return MouseButtons.XButton2;
            }
            return MouseButtons.None;
        }

        public static int MouseButtonIndex(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return 0;

                case MouseButtons.Right:
                    return 2;

                case MouseButtons.Middle:
                    return 1;

                case MouseButtons.XButton1:
                    return 3;

                case MouseButtons.XButton2:
                    return 4;
            }
            return -1;
        }

        public static bool ShowFormModal(Form f)
        {
            return ShowFormModal(f, null);
        }

        public static bool ShowFormModal(Form f, IWin32Window w)
        {
            using (DialogEditor editor = new DialogEditor())
            {
                editor.InsertForm(f);
                DialogResult result = editor.ShowDialog(w);
                f.Dispose();
                return (result == DialogResult.OK);
            }
        }

        public static Cursor StringToCursor(string s)
        {
            foreach (PropertyInfo info in typeof(Cursors).GetProperties())
            {
                if (info.PropertyType.Equals(typeof(Cursor)) && (info.Name == s))
                {
                    return (Cursor) info.GetValue(null, null);
                }
            }
            return Cursors.Default;
        }

        internal static Icon TChartIcon()
        {
            Bitmap bitmapResource = (Bitmap) Utils.GetBitmapResource("Steema.TeeChart.Images.TChart.ico");
            if (bitmapResource != null)
            {
                return Icon.FromHandle(bitmapResource.GetHicon());
            }
            return null;
        }

        public static void Translate(Control c)
        {
            if (Texts.Translator != null)
            {
                Texts.Translator.Translate(c);
            }
        }
    }
}

