namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(WindRose), "SeriesIcons.WindRose.bmp")]
    public class WindRose : Custom2DPolar
    {
        public WindRose() : this(null)
        {
        }

        public WindRose(Chart c) : base(c)
        {
            base.CircleLabels = true;
            base.RotationAngle = 90;
        }

        protected override string GetCircleLabel(double angle, int index)
        {
            switch (Utils.Round(angle))
            {
                case 0x2d:
                    return "NW";

                case 60:
                    return "NWW";

                case 0x4b:
                    return "NWWW";

                case 0:
                    return "N";

                case 15:
                    return "NNNW";

                case 30:
                    return "NNW";

                case 90:
                    return "W";

                case 0x69:
                    return "SWWW";

                case 120:
                    return "SWW";

                case 0x87:
                    return "SW";

                case 150:
                    return "SSW";

                case 0xa5:
                    return "SSSW";

                case 0xe1:
                    return "SE";

                case 240:
                    return "SEE";

                case 0xff:
                    return "SEEE";

                case 180:
                    return "S";

                case 0xc3:
                    return "SSSE";

                case 210:
                    return "SSE";

                case 270:
                    return "E";

                case 0x11d:
                    return "NEEE";

                case 300:
                    return "NEE";

                case 0x13b:
                    return "NE";

                case 330:
                    return "NNE";

                case 0x159:
                    return "NNNE";
            }
            return "";
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.AngleIncrement = 45.0;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryWindRose;
            }
        }
    }
}

