namespace Steema.TeeChart.Import
{
    using Steema.TeeChart;
    using Steema.TeeChart.Export;
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public sealed class TemplateImport : Imports
    {
        internal string fileExtension;

        public TemplateImport()
        {
            this.fileExtension = "";
            this.FileExtension = Texts.TeeFilesExtension;
        }

        public TemplateImport(Chart c)
        {
            this.fileExtension = "";
            base.chart = c;
            this.FileExtension = Texts.TeeFilesExtension;
        }

        public void FromURL(string url)
        {
            using (WebClient client = new WebClient())
            {
                this.Load(new MemoryStream(client.DownloadData(url)));
            }
        }

        public void Load(Stream stream)
        {
            IChart parent = base.chart.parent;
            SerializeBinder binder = new SerializeBinder();
            binder.BindToType("TeeChart", "Chart");
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            formatter.Binder = binder;
            base.chart.RemoveAllComponents();
            base.chart = (Chart) formatter.Deserialize(stream);
            base.chart.parent = parent;
            base.chart.parent.SetChart(base.chart);
        }

        public void Load(string fileName)
        {
            Stream stream = System.IO.File.OpenRead(fileName);
            try
            {
                this.Load(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public void LoadFileDialog()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select TeeChart file to open...";
                dialog.Filter = TemplateExport.FileFilter();
                if ((dialog.ShowDialog() == DialogResult.OK) && (dialog.FileName.Length != 0))
                {
                    System.IO.File.OpenRead(dialog.FileName);
                    this.Load(dialog.FileName);
                }
            }
        }

        public string FileExtension
        {
            get
            {
                return this.fileExtension;
            }
            set
            {
                this.fileExtension = value;
            }
        }
    }
}

