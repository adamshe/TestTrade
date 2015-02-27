namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.IO;

    public class ExcelFormat : DataExportFormat
    {
        private ushort[] beginExcel;
        private ushort[] buf;
        private ushort[] endExcel;
        private BinaryWriter s;
        private Stream stream;

        public ExcelFormat(Chart c) : base(c)
        {
            this.buf = new ushort[5];
            this.beginExcel = new ushort[] { 0x809, 8, 0, 0x10, 0, 0 };
            ushort[] numArray = new ushort[2];
            numArray[0] = 10;
            this.endExcel = numArray;
            base.FileExtension = "xls";
        }

        internal override string FilterFiles()
        {
            return Texts.ExcelFilter;
        }

        internal override string GetContent()
        {
            return "";
        }

        public override void Save(Stream stream)
        {
            this.SaveToStream(stream);
        }

        private void SaveToStream(Stream aStream)
        {
            int num;
            int num2;
            int num3;
            ushort[] numArray;
            base.Prepare();
            this.stream = aStream;
            this.s = new BinaryWriter(this.stream);
            for (int i = 0; i < this.beginExcel.GetLength(0); i++)
            {
                this.s.Write(this.beginExcel[i]);
            }
            this.WriteBuf(0x200, 10);
            this.buf[0] = 0;
            this.buf[2] = 0;
            this.buf[3] = 0;
            this.buf[4] = 0;
            this.buf[1] = (ushort) base.MaxSeriesCount();
            if (base.IncludeHeader)
            {
                (numArray = this.buf)[1] = (ushort) (numArray[1] + 1);
            }
            int num4 = (base.Series != null) ? 1 : base.Chart.Series.Count;
            if (base.hasLabels)
            {
                (numArray = this.buf)[3] = (ushort) (numArray[3] + ((ushort) num4));
            }
            if (base.hasNoMandatory)
            {
                (numArray = this.buf)[3] = (ushort) (numArray[3] + ((ushort) num4));
            }
            if (base.Series != null)
            {
                (numArray = this.buf)[3] = (ushort) (numArray[3] + ((ushort) (base.Series.ValuesLists.Count - 1)));
            }
            else
            {
                for (num = 0; num < base.Chart.Series.Count; num++)
                {
                    (numArray = this.buf)[3] = (ushort) (numArray[3] + ((ushort) (base.Chart[num].ValuesLists.Count - 1)));
                }
            }
            for (int j = 0; j < this.buf.GetLength(0); j++)
            {
                this.s.Write(this.buf[j]);
            }
            if (base.IncludeHeader)
            {
                num = -1;
                num2 = 0;
                if (base.IncludeIndex)
                {
                    this.WriteText(Texts.Index, num, num2++);
                }
                if (base.Series != null)
                {
                    this.WriteHeaderSeries(base.Series, ref num, ref num2);
                }
                else
                {
                    num3 = 0;
                    while (num3 < base.Chart.Series.Count)
                    {
                        this.WriteHeaderSeries(base.Chart[num3], ref num, ref num2);
                        num3++;
                    }
                }
            }
            for (num = 0; num < base.MaxSeriesCount(); num++)
            {
                num2 = 0;
                if (base.IncludeIndex)
                {
                    this.Writedouble((double) num, num, num2++);
                }
                if (base.Series != null)
                {
                    this.WriteSeries(base.Series, ref num, ref num2);
                }
                else
                {
                    for (num3 = 0; num3 < base.Chart.Series.Count; num3++)
                    {
                        this.WriteSeries(base.Chart[num3], ref num, ref num2);
                    }
                }
            }
            for (int k = 0; k < this.endExcel.GetLength(0); k++)
            {
                this.s.Write(this.endExcel[k]);
            }
            this.s.Flush();
        }

        private void WriteBuf(ushort value, ushort size)
        {
            this.s.Write(value);
            this.s.Write(size);
            this.s.Flush();
        }

        private void Writedouble(double value, int row, int col)
        {
            this.WriteParams(3, 8, row, col);
            this.s.Write(value);
        }

        private void WriteHeaderSeries(Series aSeries, ref int row, ref int col)
        {
            if (base.hasLabels)
            {
                this.WriteText(Texts.Text, row, col++);
            }
            if (base.hasNoMandatory)
            {
                this.WriteText(aSeries.notMandatory.Name, row, col++);
            }
            for (int i = 1; i < aSeries.ValuesLists.Count; i++)
            {
                this.WriteText(aSeries.ValuesLists[i].Name, row, col++);
            }
        }

        private void WriteNull(int row, int col)
        {
            this.WriteParams(1, 0, row, col);
        }

        private void WriteParams(ushort value, ushort size, int row, int col)
        {
            this.WriteBuf(value, (ushort) ((size + 4) + 3));
            if (base.IncludeHeader)
            {
                row++;
            }
            this.WriteBuf((ushort) row, (ushort) col);
            byte[] buffer = new byte[3];
            this.s.Write(buffer);
        }

        private void WriteSeries(Series aSeries, ref int row, ref int col)
        {
            int num;
            if ((aSeries.Count - 1) < row)
            {
                if (base.hasLabels)
                {
                    this.WriteText("", row, col++);
                }
                if (base.hasNoMandatory)
                {
                    this.WriteNull(row, col++);
                }
                for (num = 1; num < aSeries.ValuesLists.Count; num++)
                {
                    this.WriteNull(row, col++);
                }
            }
            else
            {
                if (base.hasLabels)
                {
                    this.WriteText(aSeries.Labels[row], row, col++);
                }
                if (base.hasNoMandatory)
                {
                    this.Writedouble(aSeries.notMandatory[row], row, col++);
                }
                if (aSeries.IsNull(row))
                {
                    for (num = 1; num < aSeries.ValuesLists.Count; num++)
                    {
                        this.WriteNull(row, col++);
                    }
                }
                else
                {
                    this.Writedouble(aSeries.mandatory[row], row, col++);
                    for (num = 2; num < aSeries.ValuesLists.Count; num++)
                    {
                        this.Writedouble(aSeries.ValuesLists[num][row], row, col++);
                    }
                }
            }
        }

        private void WriteText(string Value, int row, int col)
        {
            this.WriteParams(4, (ushort) (Value.Length + 1), row, col);
            this.s.Write(Value);
        }
    }
}

