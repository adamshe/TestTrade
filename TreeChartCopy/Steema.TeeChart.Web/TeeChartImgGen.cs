namespace Steema.TeeChart.Web
{
    using Steema.TeeChart.Export;
    using System;
    using System.IO;
    using System.Web;

    internal class TeeChartImgGen : WebChart, IHttpHandler
    {
        private PictureFormats pictureFormat = PictureFormats.PNG;

        public void ProcessRequest(HttpContext context)
        {
            string chartID = context.Request["ChartID"];
            base.InternalLoadViewState(base.ReadFromFile(chartID, base.designPath + @"\", "tmp"), ref this.iChart);
            ImageExportFormat format = base.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = Convert.ToInt32(context.Request["Width"]);
            format.Height = Convert.ToInt32(context.Request["Height"]);
            MemoryStream stream = new MemoryStream();
            format.Save(stream);
            stream.Flush();
            context.Response.OutputStream.Write(stream.ToArray(), 0, (int) stream.Length);
            stream.Close();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}

