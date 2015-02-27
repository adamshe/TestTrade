namespace iTrading.Gui
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Describes the style of a grid column.
    /// </summary>
    public class ColumnStyle
    {
        private HorizontalAlignment alignment = HorizontalAlignment.Right;
        private string name = "";
        private bool readOnly = false;
        private System.Type type = typeof(object);
        private bool unique = false;
        private int width = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="type">Data type.</param>
        /// <param name="width">Column width <see cref="P:iTrading.Gui.ColumnStyle.Width" />.</param>
        /// <param name="isReadOnly">Column is read-only.</param>
        /// <param name="unique">Column value is unique.</param>
        /// <param name="alignment">Alignment.</param>
        public ColumnStyle(string name, System.Type type, int width, bool isReadOnly, bool unique, HorizontalAlignment alignment)
        {
            this.alignment = alignment;
            this.type = type;
            this.name = name;
            this.readOnly = isReadOnly;
            this.width = width;
            this.unique = unique;
        }

        /// <summary>
        /// Aligment.
        /// </summary>
        public HorizontalAlignment Alignment
        {
            get
            {
                return this.alignment;
            }
        }

        /// <summary>
        /// Name of column.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Is the column read-only ?
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                this.readOnly = value;
            }
        }

        /// <summary>
        /// Datatype of grid column.
        /// </summary>
        public System.Type Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Is the column value unique ?
        /// </summary>
        public bool Unique
        {
            get
            {
                return this.unique;
            }
        }

        /// <summary>
        /// Width of column. Are <see cref="P:iTrading.Gui.ColumnStyle.Width" /> vales are added and the value
        /// of the actual <see cref="T:iTrading.Gui.ColumnStyle" /> is a fraction of the total sum.
        /// The relation betwenn sum value and fraction describes the column width compares to the <see cref="T:iTrading.Gui.TMDataGrid" />
        /// width.
        /// </summary>
        public int Width
        {
            get
            {
                return this.width;
            }
        }
    }
}

