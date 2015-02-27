namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Display and manipulate ticksize aligned prices (including fractionals).
    /// </summary>
    public class PriceUpDown : NumericUpDown
    {
        private Container components = null;
        private bool readOnlyMode = false;
        private Symbol symbol = null;

        /// <summary>
        /// </summary>
        public PriceUpDown()
        {
            this.InitializeComponent();
            base.ValueChanged += new EventHandler(this.PriceUpDown_ValueChanged);
            base.KeyPress += new KeyPressEventHandler(this.PriceUpDown_KeyPress);
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
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
            this.components = new Container();
        }

        /// <summary>
        /// Ignore user key press if readOnlyMode is true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriceUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.readOnlyMode)
            {
                e.Handled = true;
            }
        }

        private void PriceUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (this.symbol != null)
            {
                this.UpdateDecimalPlaces();
                this.Text = this.symbol.FormatPrice((double) base.Value);
            }
        }

        private void UpdateDecimalPlaces()
        {
            if (this.symbol != null)
            {
                if (this.symbol.TickSize == Globals.TickSize8)
                {
                    base.DecimalPlaces = 3;
                }
                else if (this.symbol.TickSize == Globals.TickSize32)
                {
                    base.DecimalPlaces = 5;
                }
                else if (this.symbol.TickSize == Globals.TickSize64)
                {
                    base.DecimalPlaces = 6;
                }
                else if (this.symbol.TickSize == Globals.TickSize128)
                {
                    base.DecimalPlaces = 7;
                }
                else if (this.symbol.TickSize == 0.5)
                {
                    base.DecimalPlaces = 1;
                }
                else if (this.symbol.TickSize == 0.25)
                {
                    base.DecimalPlaces = 2;
                }
                else if (this.symbol.TickSize == 0.0)
                {
                    base.DecimalPlaces = 2;
                }
                else if (this.symbol.TickSize < 0.0001)
                {
                    base.DecimalPlaces = 5;
                }
                else if (this.symbol.TickSize < 0.001)
                {
                    base.DecimalPlaces = 4;
                }
                else if (this.symbol.TickSize < 0.01)
                {
                    base.DecimalPlaces = 3;
                }
                else if (this.symbol.TickSize < 0.1)
                {
                    base.DecimalPlaces = 2;
                }
                else if (this.symbol.TickSize < 1.0)
                {
                    base.DecimalPlaces = 1;
                }
                else
                {
                    base.DecimalPlaces = 0;
                }
            }
        }

        /// <summary>
        /// </summary>
        protected override void UpdateEditText()
        {
            if (this.symbol == null)
            {
                base.UpdateEditText();
            }
            else if (((this.symbol.TickSize != Globals.TickSize8) && (this.symbol.TickSize != Globals.TickSize32)) && ((this.symbol.TickSize != Globals.TickSize64) && (this.symbol.TickSize != Globals.TickSize128)))
            {
                base.UpdateEditText();
            }
        }

        /// <summary>
        /// </summary>
        protected override void ValidateEditText()
        {
            if (this.symbol == null)
            {
                base.ValidateEditText();
            }
            else if (((this.symbol.TickSize != Globals.TickSize8) && (this.symbol.TickSize != Globals.TickSize32)) && ((this.symbol.TickSize != Globals.TickSize64) && (this.symbol.TickSize != Globals.TickSize128)))
            {
                base.ValidateEditText();
            }
        }

        /// <summary>
        /// Get/set symbol. Prices will be aligned to the ticksize of this symbol.
        /// </summary>
        public Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.readOnlyMode = false;
                base.UserEdit = true;
                this.symbol = value;
                if (this.symbol != null)
                {
                    base.Increment = (decimal) this.symbol.TickSize;
                    if (((this.symbol.TickSize == Globals.TickSize8) || (this.symbol.TickSize == Globals.TickSize32)) || ((this.symbol.TickSize == Globals.TickSize64) || (this.symbol.TickSize == Globals.TickSize128)))
                    {
                        this.readOnlyMode = true;
                        base.UserEdit = false;
                    }
                }
            }
        }
    }
}

