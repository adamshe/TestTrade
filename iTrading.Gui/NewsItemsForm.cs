namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Form to display news items.
    /// </summary>
    public class NewsItemsForm : Form
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private NewsItems newsItems;

        /// <summary>
        /// 
        /// </summary>
        public NewsItemsForm()
        {
            this.InitializeComponent();
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
            ResourceManager manager = new ResourceManager(typeof(NewsItemsForm));
            this.newsItems = new NewsItems();
            base.SuspendLayout();
            this.newsItems.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.newsItems.Connection = null;
            this.newsItems.Location = new Point(2, 2);
            this.newsItems.Name = "NewsItems";
            this.newsItems.Size = new Size(0x311, 0x182);
            this.newsItems.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x313, 0x180);
            base.Controls.Add(this.newsItems);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "NewsItemsForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "News";
            base.ResumeLayout(false);
        }

        /// <summary>
        /// Get/set the connection for news items. Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.newsItems.Connection;
            }
            set
            {
                this.newsItems.Connection = value;
            }
        }
    }
}

