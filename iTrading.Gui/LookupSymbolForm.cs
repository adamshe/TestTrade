using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// Form to query symbols from the repository.
    /// </summary>
    public class LookupSymbolForm : Form
    {
        private DateTime clickTime = Globals.MinDate;
        private Container components = null;
        private iTrading.Gui.LookupSymbol lookupSymbol;

        /// <summary>
        /// </summary>
        public LookupSymbolForm()
        {
            this.InitializeComponent();
        }

        private void DataGridMouseDown(object Sender, MouseEventArgs Evt)
        {
            if (DateTime.Now < this.clickTime.AddMilliseconds((double) SystemInformation.DoubleClickTime))
            {
                base.Close();
            }
            this.clickTime = DateTime.Now;
        }

        /// <summary>
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
            ResourceManager manager = new ResourceManager(typeof(LookupSymbolForm));
            this.lookupSymbol = new iTrading.Gui.LookupSymbol();
            base.SuspendLayout();
            this.lookupSymbol.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lookupSymbol.Connection = null;
            this.lookupSymbol.Location = new Point(0, 0);
            this.lookupSymbol.Name = "lookupSymbol";
            this.lookupSymbol.Size = new Size(760, 0x218);
            this.lookupSymbol.TabIndex = 1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(760, 0x218);
            base.Controls.Add(this.lookupSymbol);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimumSize = new Size(0x1a0, 0x120);
            base.Name = "LookupSymbolForm";
            base.ShowInTaskbar = false;
            this.Text = "Lookup symbol";
            base.Load += new EventHandler(this.LookupSymbolForm_Load);
            base.ResumeLayout(false);
        }

        private void LookupSymbolForm_Load(object sender, EventArgs e)
        {
            this.LookupSymbol.SymbolsDataGrid.MouseDown += new MouseEventHandler(this.DataGridMouseDown);
        }

        /// <summary>
        /// </summary>
        public iTrading.Gui.LookupSymbol LookupSymbol
        {
            get
            {
                return this.lookupSymbol;
            }
        }
    }
}

