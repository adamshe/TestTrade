namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class ImportEditor : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Chart chart;
        private Container components = null;
        private OpenFileDialog openFileDialog1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private TextBox textBox1;
        private TextBox textBox2;

        public ImportEditor()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (this.radioButton2.Checked)
                {
                    this.chart.Import.Template.FromURL(this.textBox2.Text);
                }
                else
                {
                    FileStream stream = new FileStream(this.textBox1.Text, FileMode.Open);
                    this.chart.Import.Template.Load(stream);
                    stream.Close();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
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
            this.textBox1 = new TextBox();
            this.textBox2 = new TextBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.radioButton1 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.button3 = new Button();
            this.openFileDialog1 = new OpenFileDialog();
            base.SuspendLayout();
            this.textBox1.BorderStyle = BorderStyle.FixedSingle;
            this.textBox1.Location = new Point(0x67, 0x18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x101, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "tChart1.ten";
            this.textBox1.Enter += new EventHandler(this.textBox1_Enter);
            this.textBox2.BorderStyle = BorderStyle.FixedSingle;
            this.textBox2.Location = new Point(0x67, 0x3e);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(0x121, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "http://www.teechart.net/demo.ten";
            this.textBox2.Enter += new EventHandler(this.textBox2_Enter);
            this.button1.DialogResult = DialogResult.OK;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0xe8, 0x68);
            this.button1.Name = "button1";
            this.button1.TabIndex = 4;
            this.button1.Text = "&Import";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.DialogResult = DialogResult.Cancel;
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(320, 0x68);
            this.button2.Name = "button2";
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x58, 0x18);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "From &File:";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 0x3e);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x58, 0x18);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.Text = "From &URL:";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.button3.Cursor = Cursors.Hand;
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.Location = new Point(0x16d, 0x16);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x1b, 0x17);
            this.button3.TabIndex = 8;
            this.button3.Text = "...";
            this.button3.Click += new EventHandler(this.button3_Click);
            this.openFileDialog1.DefaultExt = "tee";
            this.openFileDialog1.Filter = "TeeChart files|*.ten";
            this.openFileDialog1.Title = "Import TeeChart file";
            base.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.button2;
            base.ClientSize = new Size(0x198, 0x8d);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.radioButton2);
            base.Controls.Add(this.radioButton1);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.textBox2);
            base.Controls.Add(this.textBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "ImportEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TeeChart Import";
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked && !this.textBox1.Focused)
            {
                this.textBox1.Focus();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked && !this.textBox2.Focused)
            {
                this.textBox2.Focus();
            }
        }

        private void ReadFromStream(Stream s)
        {
        }

        public static void ShowModal(Chart c)
        {
            using (ImportEditor editor = new ImportEditor())
            {
                editor.chart = c;
                EditorUtils.Translate(editor);
                editor.ShowDialog();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (!this.radioButton1.Checked)
            {
                this.radioButton1.Checked = true;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (!this.radioButton2.Checked)
            {
                this.radioButton2.Checked = true;
            }
        }
    }
}

