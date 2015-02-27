using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;
using Steema.TeeChart;
using Steema.TeeChart.Drawing;
using iTrading.Core.Data;

namespace iTrading.Core.Chart
{
    /// <summary>
    /// </summary>
    public class ChartForm : Form
    {
        private ChartControl chartControl;
        private Container components = null;

        /// <summary>
        /// </summary>
        public ChartForm()
        {
            this.InitializeComponent();
            this.chartControl.TeeChart.AfterDraw += new PaintChartEventHandler(this.TeeChart_AfterDraw);
        }

        private void ChartForm_Closing(object sender, CancelEventArgs e)
        {
            this.chartControl.Quotes = null;
        }

        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(ChartForm));
            this.chartControl = new ChartControl();
            base.SuspendLayout();
            this.chartControl.Dock = DockStyle.Fill;
            this.chartControl.Location = new Point(0, 0);
            this.chartControl.Name = "chartControl";
            this.chartControl.Quotes = null;
            this.chartControl.Size = new Size(0x410, 0x2ab);
            this.chartControl.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x410, 0x2ab);
            base.Controls.Add(this.chartControl);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimumSize = new Size(400, 300);
            base.Name = "ChartForm";
            this.Text = "Chart";
            base.Closing += new CancelEventHandler(this.ChartForm_Closing);
            base.ResumeLayout(false);
        }

        private void TeeChart_AfterDraw(object sender, Graphics3D g)
        {
            string str = "";
            if (this.chartControl.First >= 0)
            {
                if (this.chartControl.Quotes.Period.Id == PeriodTypeId.Day)
                {
                    str = "  " + this.chartControl.Quotes.Bars[this.chartControl.First].Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) + " - " + this.chartControl.Quotes.Bars[this.chartControl.Last].Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                }
                else
                {
                    str = "  " + this.chartControl.Quotes.Bars[this.chartControl.First].Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) + " ";
                }
            }
            this.Text = this.chartControl.Quotes.Symbol.FullName + str + " (" + this.chartControl.Quotes.Period.ToString() + ")";
        }

        /// <summary>
        /// </summary>
        public ChartControl ChartControl
        {
            get
            {
                return this.chartControl;
            }
        }
    }
}