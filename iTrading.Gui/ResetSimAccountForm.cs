namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public class ResetSimAccountForm : Form
    {
        private Button cancelButton;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button okButton;

        /// <summary>
        /// </summary>
        public ResetSimAccountForm()
        {
            this.InitializeComponent();
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
            ResourceManager manager = new ResourceManager(typeof(ResetSimAccountForm));
            this.label1 = new Label();
            this.label2 = new Label();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.label3 = new Label();
            base.SuspendLayout();
            this.label1.ForeColor = Color.Red;
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1c8, 0x38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Warning: Resetting the simulation account removes any recorded order and execution data from the repository.";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Location = new Point(8, 0x80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1c8, 0x17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Do you want to proceed ?";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.okButton.BackColor = SystemColors.Control;
            this.okButton.Location = new Point(0x68, 0xa8);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(0x6b, 0x1a);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&Ok";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            this.cancelButton.BackColor = SystemColors.Control;
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Location = new Point(0x108, 0xa8);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new Size(0x6b, 0x1a);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "&Cancel";
            this.label3.Location = new Point(8, 80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1c8, 0x20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Note: Close and re-open your simulation account window to make the reset effect visible.";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            base.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.CancelButton = this.cancelButton;
            base.ClientSize = new Size(0x1da, 0xd5);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cancelButton);
            base.Controls.Add(this.okButton);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "ResetSimAccountForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Reset simulation account";
            base.ResumeLayout(false);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
    }
}

