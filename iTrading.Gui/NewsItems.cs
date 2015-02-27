namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control to display news items.
    /// </summary>
    public class NewsItems : UserControl
    {
        private const string columnHeadLine = "Headline";
        private const string columnId = "Id";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Id", typeof(string), 1, true, false, HorizontalAlignment.Left), new iTrading.Gui.ColumnStyle("Time", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Type", typeof(string), 1, true, false, HorizontalAlignment.Left), new iTrading.Gui.ColumnStyle("Headline", typeof(string), 3, true, false, HorizontalAlignment.Left), new iTrading.Gui.ColumnStyle("Text", typeof(string), 3, true, false, HorizontalAlignment.Left) };
        private const string columnText = "Text";
        private const string columnTime = "Time";
        private const string columnType = "Type";
        private Container components = null;
        private Connection connection = null;
        private TMDataGrid newsItemsDataGrid;

        /// <summary>
        /// </summary>
        public NewsItems()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.NewsItems_Disposed);
        }

        private void Add(NewsEventArgs e)
        {
            DataRow row = this.newsItemsDataGrid.DataTable.NewRow();
            row["Headline"] = e.HeadLine;
            row["Id"] = e.Id;
            row["Text"] = e.Text;
            row["Time"] = e.Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) + " " + e.Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
            row["Type"] = e.ItemType.Name;
            this.newsItemsDataGrid.DataTable.Rows.Add(row);
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
            this.newsItemsDataGrid = new TMDataGrid();
            this.newsItemsDataGrid.BeginInit();
            base.SuspendLayout();
            this.newsItemsDataGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.newsItemsDataGrid.CaptionVisible = false;
            this.newsItemsDataGrid.ColumnStyles = null;
            this.newsItemsDataGrid.DataMember = "";
            this.newsItemsDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.newsItemsDataGrid.Location = new Point(0, 0);
            this.newsItemsDataGrid.Name = "NewsItemsDataGrid";
            this.newsItemsDataGrid.Size = new Size(0x1c0, 0xf8);
            this.newsItemsDataGrid.TabIndex = 0;
            base.Controls.Add(this.newsItemsDataGrid);
            base.Name = "NewsItems";
            base.Size = new Size(0x1c0, 0xf8);
            base.Load += new EventHandler(this.NewsItems_Load);
            this.newsItemsDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void NewsItem(object sender, NewsEventArgs e)
        {
            this.Add(e);
        }

        private void NewsItems_Disposed(object sender, EventArgs e)
        {
            if (!base.Visible)
            {
                this.connection.News.News -= new NewsEventHandler(this.NewsItem);
            }
        }

        private void NewsItems_Load(object sender, EventArgs e)
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.GuiNotInitialized, "NewsItems.Connection property is NULL. Control is not initialized properly.");
            }
            if (this.connection.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                this.newsItemsDataGrid.RowHeadersVisible = false;
                this.newsItemsDataGrid.ColumnStyles = this.columnStyles;
                this.newsItemsDataGrid.DataView.AllowNew = false;
                this.connection.News.News += new NewsEventHandler(this.NewsItem);
            }
        }

        /// <summary>
        /// Get/set the connection for retrieving news items. Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }
    }
}

