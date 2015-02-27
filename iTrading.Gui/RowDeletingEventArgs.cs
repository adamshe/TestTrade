namespace iTrading.Gui
{
    using System;
    using System.Collections;

    /// <summary>
    /// An instance of this class will be passed as argument to a <see cref="T:iTrading.Gui.RowDeletingEventHandler" />.
    /// </summary>
    public class RowDeletingEventArgs : EventArgs
    {
        private bool cancel = false;
        private ArrayList dataRows;

        internal RowDeletingEventArgs(ArrayList dataRows)
        {
            this.dataRows = dataRows;
        }

        /// <summary>
        /// Set to true for cancel the deletion of the row.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                this.cancel = value;
            }
        }

        /// <summary>
        /// The list of datarows to delete.
        /// </summary>
        public ArrayList Rows
        {
            get
            {
                return this.dataRows;
            }
        }
    }
}

