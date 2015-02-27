namespace Steema.TeeChart.Export
{
    using System;

    public class ExportFormat
    {
        public string FileExtension = "";

        internal ExportFormat()
        {
        }

        internal virtual string FilterFiles()
        {
            return "";
        }
    }
}

