using Steema.TeeChart.Functions;

namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class Utils
    {
        public static double[] DateTimeStep = new double[] { 
            1.1574074074074074E-08, 1.1574074074074074E-05, 5.7870370370370373E-05, 0.00011574074074074075, 0.00017361111111111112, 0.00034722222222222224, 0.00069444444444444447, 0.003472222222222222, 0.0069444444444444441, 0.010416666666666666, 0.020833333333333332, 0.041666666666666664, 0.083333333333333329, 0.25, 0.5, 1.0, 
            2.0, 3.0, 7.0, 15.0, 30.0, 60.0, 90.0, 120.0, 182.0, 365.0, 1.0
         };
        public static int[] FunctionGalleryPage = new int[] { 
            0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 0, 2, 2, 
            1, 1, 2, 3, 1, 1, 1, 1, 1, 1, 3
         };
        public const int FunctionTypesCount = 0x1b;
        public static System.Type[] FunctionTypesOf = new System.Type[] { 
            typeof(Add), typeof(Subtract), typeof(Multiply), typeof(Divide), typeof(High), typeof(Low), typeof(Average), typeof(Count), typeof(Momentum), typeof(MomentumDivision), typeof(Cumulative), typeof(ExpAverage), typeof(Smoothing), typeof(Custom), typeof(RootMeanSquare), typeof(StdDeviation), 
            typeof(Stochastic), typeof(ExpMovAverage), typeof(Performance), typeof(CrossPoints), typeof(CompressOHLC), typeof(CLVFunction), typeof(OBVFunction), typeof(CCIFunction), typeof(MovingAverage), typeof(PVOFunction), typeof(DownSampling)
         };
        public static double PiStep = 0.017453292519943295;
        public static int[] SeriesGalleryCount = new int[] { 
            2, 2, 2, 2, 2, 2, 2, 1, 3, 1, 1, 1, 1, 1, 1, 1, 
            1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 
            2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1
         };
        public static int[] SeriesGalleryPage = new int[] { 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 3, 1, 4, 
            4, 3, 3, 5, 5, 3, 4, 5, 5, 4, 4, 2, 2, 2, 4, 3, 
            3, 5, 2, 4, 2, 2, 2, 0, 4, 1, 3, 4
         };
        public const int SeriesTypesCount = 0x2c;
        public static System.Type[] SeriesTypesOf = new System.Type[] { 
            typeof(Line), typeof(Points), typeof(Area), typeof(FastLine), typeof(HorizLine), typeof(Bar), typeof(HorizBar), typeof(Pie), typeof(Shape), typeof(Arrow), typeof(Bubble), typeof(Gantt), typeof(Candle), typeof(Donut), typeof(Volume), typeof(Bar3D), 
            typeof(Points3D), typeof(Polar), typeof(Radar), typeof(Clock), typeof(WindRose), typeof(Pyramid), typeof(Surface), typeof(LinePoint), typeof(BarJoin), typeof(ColorGrid), typeof(Waterfall), typeof(Histogram), typeof(Error), typeof(ErrorBar), typeof(Contour), typeof(Smith), 
            typeof(Bezier), typeof(Calendar), typeof(HighLow), typeof(TriSurface), typeof(Funnel), typeof(Box), typeof(HorizBox), typeof(HorizArea), typeof(Tower), typeof(PointFigure), typeof(Gauges), typeof(Vector3D)
         };
        public const int ToolTypesCount = 0x13;
        public static System.Type[] ToolTypesOf = new System.Type[] { 
            typeof(Annotation), typeof(PageNumber), typeof(Rotate), typeof(ChartImage), typeof(CursorTool), typeof(DragMarks), typeof(AxisArrow), typeof(ColorLine), typeof(ColorBand), typeof(DrawLine), typeof(NearestPoint), typeof(MarksTip), typeof(ExtraLegend), typeof(DragPoint), typeof(SeriesAnimation), typeof(PieTool), 
            typeof(GridTranspose), typeof(GanttTool), typeof(GridBand)
         };

        private Utils()
        {
        }

        public static double DateTime(System.DateTime value)
        {
            return value.ToOADate();
        }

        public static System.DateTime DateTime(double value)
        {
            return System.DateTime.FromOADate(value);
        }

        public static string DateTimeDefaultFormat(double tmpValue)
        {
            return DateTimeToStr(DateTime(tmpValue));
        }

        public static string DateTimeToStr(System.DateTime datetime)
        {
            return datetime.ToShortDateString();
        }

        public static string DateTimeToStr(double datetime)
        {
            return DateTime(datetime).ToShortDateString();
        }

        public static void DrawCheckBox(int x, int y, Graphics g, bool drawChecked, Color backColor)
        {
            ButtonState state = drawChecked ? ButtonState.Checked : ButtonState.Normal;
            ControlPaint.DrawCheckBox(g, x, y, 14, 14, state | ButtonState.Flat);
        }

        public static string FormatFloat(string format, double value)
        {
            return value.ToString(format);
        }

        internal static Image GetBitmapResource(string name)
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (manifestResourceStream != null)
            {
                return new Bitmap(manifestResourceStream);
            }
            return null;
        }

        public static double GetDateTimeStep(DateTimeSteps value)
        {
            return DateTimeStep[(int) value];
        }

        public static Point PointAtDistance(Point AFrom, Point ATo, int ADist)
        {
            Point point = new Point(ATo.X, ATo.Y);
            if (AFrom.X != ATo.X)
            {
                double num;
                double num2;
                SinCos(Math.Atan2((double) (ATo.Y - AFrom.Y), (double) (ATo.X - AFrom.X)), out num, out num2);
                point.X -= Round((double) (ADist * num2));
                point.Y -= Round((double) (ADist * num));
                return point;
            }
            if (ATo.Y < AFrom.Y)
            {
                point.Y += ADist;
                return point;
            }
            point.Y -= ADist;
            return point;
        }

        private static void PrivateSort(int l, int r, CompareEventHandler Compare, SwapEventHandler Swap)
        {
            int a = l;
            int b = r;
            int num3 = (a + b) / 2;
            while (a < b)
            {
                while (Compare(a, num3) < 0)
                {
                    a++;
                }
                while (Compare(num3, b) < 0)
                {
                    b--;
                }
                if (a < b)
                {
                    Swap(a, b);
                    if (a == num3)
                    {
                        num3 = b;
                    }
                    else if (b == num3)
                    {
                        num3 = a;
                    }
                }
                if (a <= b)
                {
                    a++;
                    b--;
                }
            }
            if (l < b)
            {
                PrivateSort(l, b, Compare, Swap);
            }
            if (a < r)
            {
                PrivateSort(a, r, Compare, Swap);
            }
        }

        public static int Round(double value)
        {
            return (int) value;
        }

        public static int Round(float value)
        {
            return (int) value;
        }

        public static int SeriesTypesIndex(Series s)
        {
            return SeriesTypesIndex(s.GetType());
        }

        public static int SeriesTypesIndex(System.Type seriesType)
        {
            for (int i = 0; i < 0x2c; i++)
            {
                if (SeriesTypesOf[i] == seriesType)
                {
                    return i;
                }
            }
            return -1;
        }

        public static void SinCos(double angle, out double resultSin, out double resultCos)
        {
            resultSin = Math.Sin(angle);
            resultCos = Math.Cos(angle);
        }

        public static void Sort(int startIndex, int endIndex, CompareEventHandler compareFunction, SwapEventHandler swap)
        {
            PrivateSort(startIndex, endIndex, compareFunction, swap);
        }

        public static double Sqr(double value)
        {
            return (value * value);
        }

        public static double StringToDouble(string text, double value)
        {
            if (text.Length == 0)
            {
                return value;
            }
            try
            {
                return double.Parse(text);
            }
            catch (ArgumentNullException)
            {
                return value;
            }
            catch (FormatException)
            {
                return value;
            }
            catch (OverflowException)
            {
                return value;
            }
        }

        public static string TimeToStr(System.DateTime datetime)
        {
            return datetime.ToShortTimeString();
        }

        public static string TimeToStr(double datetime)
        {
            return DateTime(datetime).ToShortTimeString();
        }

        public static int ToolTypeIndex(Steema.TeeChart.Tools.Tool tool)
        {
            for (int i = 0; i < 0x13; i++)
            {
                if (ToolTypesOf[i] == tool.GetType())
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool YesNo(string s)
        {
            return (MessageBox.Show(s, Texts.Confirm, MessageBoxButtons.YesNo) == DialogResult.Yes);
        }

        public static bool YesNoDelete(string s)
        {
            return YesNo(string.Format(Texts.SureToDelete, s));
        }

        public delegate int CompareEventHandler(int a, int b);

        public delegate void SwapEventHandler(int a, int b);
    }
}

