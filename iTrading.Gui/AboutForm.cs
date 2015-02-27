namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Zusammenfassung für About.
    /// </summary>
    public class AboutForm : Form
    {
        private IContainer components = null;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label machineId;
        private Button okButton;
        private Panel panel1;
        private Button supportButton;
        private Button websiteButton;

        /// <summary>
        /// 
        /// </summary>
        public AboutForm()
        {
            this.InitializeComponent();
            this.label3.Text = "Version: " + Assembly.GetAssembly(typeof(Connection)).GetName().Version.ToString();
            this.machineId.Text = "Machine ID: " + Globals.MachineId;
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
            this.okButton = new Button();
            this.supportButton = new Button();
            this.websiteButton = new Button();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label2 = new Label();
            this.panel1 = new Panel();
            this.machineId = new Label();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.okButton.BackColor = SystemColors.Control;
            this.okButton.Location = new Point(240, 0x80);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(0x59, 0x17);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            this.supportButton.BackColor = SystemColors.Control;
            this.supportButton.Location = new Point(0x18, 0x80);
            this.supportButton.Name = "supportButton";
            this.supportButton.Size = new Size(0x58, 0x17);
            this.supportButton.TabIndex = 3;
            this.supportButton.Text = "E-Mail Support";
            this.supportButton.Click += new EventHandler(this.supportButton_Click);
            this.websiteButton.BackColor = SystemColors.Control;
            this.websiteButton.Location = new Point(0x80, 0x80);
            this.websiteButton.Name = "websiteButton";
            this.websiteButton.Size = new Size(0x59, 0x17);
            this.websiteButton.TabIndex = 4;
            this.websiteButton.Text = "Web Site";
            this.websiteButton.Click += new EventHandler(this.websiteButton_Click);
            this.label3.BackColor = Color.Transparent;
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label3.ForeColor = SystemColors.MenuText;
            this.label3.Location = new Point(0, 0x30);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x158, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "Build:";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.label4.BackColor = Color.Transparent;
            this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.ForeColor = SystemColors.MenuText;
            this.label4.Location = new Point(-8, 0x48);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x160, 0x10);
            this.label4.TabIndex = 6;
            this.label4.Text = "Copyright \x00a9 2003 - 2004 Dierk Droth";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.BackColor = Color.Transparent;
            this.label2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.ForeColor = SystemColors.MenuText;
            this.label2.Location = new Point(0, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x160, 0x18);
            this.label2.TabIndex = 7;
            this.label2.Text = "TradeMagic 3";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.panel1.Controls.Add(this.machineId);
            this.panel1.Controls.Add(this.okButton);
            this.panel1.Controls.Add(this.supportButton);
            this.panel1.Controls.Add(this.websiteButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x160, 0xc0);
            this.panel1.TabIndex = 8;
            this.machineId.BackColor = Color.Transparent;
            this.machineId.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.machineId.ForeColor = SystemColors.MenuText;
            this.machineId.Location = new Point(9, 0x60);
            this.machineId.Name = "machineId";
            this.machineId.Size = new Size(0x14d, 0x10);
            this.machineId.TabIndex = 8;
            this.machineId.Text = "Machine ID:";
            this.machineId.TextAlign = ContentAlignment.MiddleCenter;
            base.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0xa7);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AboutForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About TradeMagic";
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void supportButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("mailto:support@trademagic.net");
            }
            catch
            {
            }
            finally
            {
                base.Close();
            }
        }

        private void websiteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.trademagic.net");
            }
            catch
            {
            }
            finally
            {
                base.Close();
            }
        }
    }
}

