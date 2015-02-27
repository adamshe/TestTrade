namespace Steema.TeeChart.Editors
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextInput : Form
    {
        private Button button1;
        private Button button2;
        private Container components = null;
        internal Label label1;
        internal TextBox textBox1;

        public TextInput()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            this.button1 = new Button();
            this.button2 = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Text:";
            this.textBox1.BorderStyle = BorderStyle.FixedSingle;
            this.textBox1.Location = new Point(8, 0x1b);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x108, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "text";
            this.button1.DialogResult = DialogResult.OK;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(100, 0x41);
            this.button1.Name = "button1";
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button2.DialogResult = DialogResult.Cancel;
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0xc4, 0x41);
            this.button2.Name = "button2";
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            base.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.button2;
            base.ClientSize = new Size(0x119, 0x5f);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "TextInput";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TextInput";
            base.ResumeLayout(false);
        }

        public static bool Query(string caption, string input, ref string value)
        {
            using (TextInput input2 = new TextInput())
            {
                EditorUtils.Translate(input2);
                input2.Text = caption;
                input2.label1.Text = input;
                input2.textBox1.Text = value;
                if (input2.ShowDialog() == DialogResult.OK)
                {
                    value = input2.textBox1.Text;
                    return true;
                }
                return false;
            }
        }
    }
}

