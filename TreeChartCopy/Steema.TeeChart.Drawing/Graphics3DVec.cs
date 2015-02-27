namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text;

    public abstract class Graphics3DVec : Graphics3D
    {
        private Stream istream;

        public Graphics3DVec(Stream stream, Chart c) : base(c)
        {
            this.istream = stream;
            base.UseBuffer = false;
            Bitmap image = new Bitmap(1, 1);
            base.g = Graphics.FromImage(image);
        }

        protected void AddToStream(string text)
        {
            string s = text + "\n";
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            this.istream.Write(bytes, 0, bytes.Length);
        }
    }
}

