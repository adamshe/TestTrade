namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.Windows.Forms;

    public sealed class PictureDialog
    {
        private PictureDialog()
        {
        }

        public static string FileName(Control c)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.DereferenceLinks = true;
                dialog.Title = Texts.SelectPictureFile;
                dialog.Filter = Texts.AllPictureFilter + "|" + Texts.BMPFilter + "|" + Texts.JPEGFilter + "|" + Texts.GIFFilter + "|" + Texts.PNGFilter + "|" + Texts.AllFilesFilter;
                if (dialog.ShowDialog(c) == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return "";
        }
    }
}

