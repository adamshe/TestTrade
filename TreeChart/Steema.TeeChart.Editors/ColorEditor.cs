namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ColorEditor : Form
    {
        private Button bCancel;
        private Button bOk;
        private Button button1;
        private System.Drawing.Color color;
        public EventHandler ColorChanged;
        public EventHandler ColorChoosen;
        private static System.Drawing.Color[] ColorPalette = new System.Drawing.Color[] { 
            System.Drawing.Color.White, System.Drawing.Color.FromArgb(0xff, 0xc0, 0xc0), System.Drawing.Color.FromArgb(0xff, 0xe0, 0xc0), System.Drawing.Color.FromArgb(0xff, 0xff, 0xc0), System.Drawing.Color.FromArgb(0xc0, 0xff, 0xc0), System.Drawing.Color.FromArgb(0xc0, 0xff, 0xff), System.Drawing.Color.FromArgb(0xc0, 0xc0, 0xff), System.Drawing.Color.FromArgb(0xff, 0xc0, 0xff), System.Drawing.Color.FromArgb(0xe0, 0xe0, 0xe0), System.Drawing.Color.FromArgb(0xff, 0x80, 0x80), System.Drawing.Color.FromArgb(0xff, 0xc0, 0x80), System.Drawing.Color.FromArgb(0xff, 0xff, 0x80), System.Drawing.Color.FromArgb(0x80, 0xff, 0x80), System.Drawing.Color.FromArgb(0x80, 0xff, 0xff), System.Drawing.Color.FromArgb(0x80, 0x80, 0xff), System.Drawing.Color.FromArgb(0xff, 0x80, 0xff), 
            System.Drawing.Color.FromArgb(0xc0, 0xc0, 0xc0), System.Drawing.Color.Red, System.Drawing.Color.FromArgb(0xff, 0x80, 0), System.Drawing.Color.FromArgb(0xff, 0xff, 0), System.Drawing.Color.Green, System.Drawing.Color.FromArgb(0, 0xff, 0xff), System.Drawing.Color.Blue, System.Drawing.Color.FromArgb(0xff, 0, 0xff), System.Drawing.Color.FromArgb(0x80, 0x80, 0x80), System.Drawing.Color.FromArgb(0xc0, 0, 0), System.Drawing.Color.FromArgb(0xc0, 0x40, 0), System.Drawing.Color.FromArgb(0xc0, 0xc0, 0), System.Drawing.Color.FromArgb(0, 0xc0, 0), System.Drawing.Color.FromArgb(0, 0xc0, 0xc0), System.Drawing.Color.FromArgb(0, 0, 0xc0), System.Drawing.Color.FromArgb(0xc0, 0, 0xc0), 
            System.Drawing.Color.FromArgb(0x40, 0x40, 0x40), System.Drawing.Color.FromArgb(0x80, 0, 0), System.Drawing.Color.FromArgb(0x80, 0x40, 0), System.Drawing.Color.FromArgb(0x80, 0x80, 0), System.Drawing.Color.FromArgb(0, 0x80, 0), System.Drawing.Color.FromArgb(0, 0x80, 0x80), System.Drawing.Color.FromArgb(0, 0, 0x80), System.Drawing.Color.FromArgb(0x80, 0, 0x80), System.Drawing.Color.Black, System.Drawing.Color.FromArgb(0x40, 0, 0), System.Drawing.Color.FromArgb(0x80, 0x40, 0x40), System.Drawing.Color.FromArgb(0x40, 0x40, 0), System.Drawing.Color.FromArgb(0, 0x40, 0), System.Drawing.Color.FromArgb(0, 0x40, 0x40), System.Drawing.Color.FromArgb(0, 0, 0x40), System.Drawing.Color.FromArgb(0x40, 0, 0x40), 
            System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.White
         };
        private IContainer components;
        private PictureBox customPicture;
        private Label label3;
        private Label label4;
        internal NumericUpDown ndTransp;
        private PictureBox oldFocused;
        private Panel panel1;
        internal Panel panel2;
        private ToolTip toolTip1;

        public ColorEditor()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.oldFocused.Tag != null)
            {
                this.CustomColor(this.oldFocused, this.color);
            }
            else
            {
                this.CustomColor(this.customPicture, this.color);
            }
        }

        private void ChangeColor()
        {
            if (this.oldFocused != null)
            {
                this.color = this.oldFocused.BackColor;
            }
            if (this.ColorChanged != null)
            {
                this.ColorChanged(this, EventArgs.Empty);
            }
        }

        public static System.Drawing.Color Choose(System.Drawing.Color c)
        {
            return Choose(c, null);
        }

        public static System.Drawing.Color Choose(System.Drawing.Color c, IWin32Window w)
        {
            using (ColorEditor editor = new ColorEditor())
            {
                editor.FormBorderStyle = FormBorderStyle.FixedDialog;
                editor.Fill(c, 0x40);
                editor.MaximizeBox = false;
                editor.StartPosition = FormStartPosition.CenterParent;
                EditorUtils.Translate(editor);
                if (editor.ShowDialog(w) != DialogResult.OK)
                {
                    return c;
                }
                return editor.Color;
            }
        }

        private void Color_Click(object sender, EventArgs e)
        {
            if (this.oldFocused != null)
            {
                this.DrawFocus(this.oldFocused, System.Drawing.Color.FromKnownColor(KnownColor.Control));
            }
            this.oldFocused = (PictureBox) sender;
            this.DrawFocus(this.oldFocused, System.Drawing.Color.Black);
            this.ChangeColor();
        }

        private void ColorChooser_Paint(object sender, PaintEventArgs e)
        {
            if (this.oldFocused != null)
            {
                this.DrawFocus(this.oldFocused, System.Drawing.Color.Black);
            }
        }

        private string ColorToString(System.Drawing.Color c)
        {
            if ((c.IsKnownColor || c.IsSystemColor) || c.IsNamedColor)
            {
                return c.Name;
            }
            foreach (PropertyInfo info in typeof(System.Drawing.Color).GetProperties())
            {
                if (info.PropertyType.Equals(typeof(System.Drawing.Color)))
                {
                    System.Drawing.Color a = (System.Drawing.Color) info.GetValue(null, null);
                    if (this.SameColor(a, c))
                    {
                        return a.ToString();
                    }
                }
            }
            return c.ToString();
        }

        private void CustomColor(PictureBox p, System.Drawing.Color startColor)
        {
            bool ok = false;
            System.Drawing.Color color = WindowsDialog(startColor, this, true, out ok);
            if (ok)
            {
                p.BackColor = color;
                this.oldFocused = p;
                this.ChangeColor();
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

        private void DrawFocus(PictureBox p, System.Drawing.Color c)
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                Rectangle bounds = p.Bounds;
                bounds.X--;
                bounds.Y--;
                bounds.Width += 2;
                bounds.Height += 2;
                using (Pen pen = new Pen(c, 2f))
                {
                    graphics.DrawRectangle(pen, bounds);
                }
            }
        }

        internal void Fill(System.Drawing.Color c, int maxColors)
        {
            this.color = c;
            if (this.color.IsEmpty)
            {
                this.ndTransp.Value = 0M;
            }
            else
            {
                this.ndTransp.Value = Graphics3D.Transparency(this.color);
            }
            for (int i = 0; i < maxColors; i++)
            {
                int num = i / 8;
                int num2 = i % 8;
                PictureBox control = new PictureBox();
                control.Location = new Point(0x12 + (num2 * 0x18), 6 + (num * 0x16));
                control.Size = new Size(0x16, 20);
                control.BorderStyle = BorderStyle.Fixed3D;
                control.Cursor = Cursors.Hand;
                control.BackColor = ColorPalette[i];
                if ((this.oldFocused == null) && this.SameColor(control.BackColor, this.color))
                {
                    this.oldFocused = control;
                }
                this.toolTip1.SetToolTip(control, this.ColorToString(ColorPalette[i]));
                control.Click += new EventHandler(this.Color_Click);
                control.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
                control.DoubleClick += new EventHandler(this.pictureBox1_DoubleClick);
                control.Parent = this;
                control.Show();
                if (i == 0x30)
                {
                    this.customPicture = control;
                }
                if (i >= 0x30)
                {
                    control.Tag = 1;
                }
            }
            if (this.oldFocused == null)
            {
                this.customPicture.BackColor = System.Drawing.Color.FromArgb(this.color.R, this.color.G, this.color.B);
                this.oldFocused = this.customPicture;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.panel1 = new Panel();
            this.button1 = new Button();
            this.label4 = new Label();
            this.ndTransp = new NumericUpDown();
            this.label3 = new Label();
            this.toolTip1 = new ToolTip(this.components);
            this.panel2 = new Panel();
            this.bCancel = new Button();
            this.bOk = new Button();
            this.panel1.SuspendLayout();
            this.ndTransp.BeginInit();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ndTransp);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xbd);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xf8, 0x20);
            this.panel1.TabIndex = 0;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0xa5, 5);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0x10;
            this.button1.Text = "&Custom...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x86, 10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(14, 0x10);
            this.label4.TabIndex = 15;
            this.label4.Text = "%";
            this.label4.UseMnemonic = false;
            this.ndTransp.BorderStyle = BorderStyle.FixedSingle;
            this.ndTransp.Location = new Point(0x55, 6);
            this.ndTransp.Name = "ndTransp";
            this.ndTransp.Size = new Size(0x2b, 20);
            this.ndTransp.TabIndex = 14;
            this.ndTransp.TextAlign = HorizontalAlignment.Right;
            this.ndTransp.TextChanged += new EventHandler(this.ndTransp_ValueChanged);
            this.ndTransp.ValueChanged += new EventHandler(this.ndTransp_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4d, 0x10);
            this.label3.TabIndex = 13;
            this.label3.Text = "&Transparency:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.panel2.Controls.Add(this.bCancel);
            this.panel2.Controls.Add(this.bOk);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0xdd);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0xf8, 0x20);
            this.panel2.TabIndex = 1;
            this.bCancel.DialogResult = DialogResult.Cancel;
            this.bCancel.FlatStyle = FlatStyle.Flat;
            this.bCancel.Location = new Point(0xa5, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.TabIndex = 20;
            this.bCancel.Text = "Cancel";
            this.bOk.DialogResult = DialogResult.OK;
            this.bOk.FlatStyle = FlatStyle.Flat;
            this.bOk.Location = new Point(0x51, 5);
            this.bOk.Name = "bOk";
            this.bOk.TabIndex = 0x13;
            this.bOk.Text = "OK";
            base.AcceptButton = this.bOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.bCancel;
            base.ClientSize = new Size(0xf8, 0xfd);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.panel2);
            base.Name = "ColorEditor";
            this.Text = "Color Editor";
            base.Paint += new PaintEventHandler(this.ColorChooser_Paint);
            this.panel1.ResumeLayout(false);
            this.ndTransp.EndInit();
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void ndTransp_ValueChanged(object sender, EventArgs e)
        {
            this.ChangeColor();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.ColorChoosen != null)
            {
                this.ColorChoosen(this, EventArgs.Empty);
            }
            else if (this.panel2.Visible)
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PictureBox p = (PictureBox) sender;
                this.CustomColor(p, p.BackColor);
            }
        }

        private bool SameColor(System.Drawing.Color a, System.Drawing.Color b)
        {
            return (((a.R == b.R) && (a.G == b.G)) && (a.B == b.B));
        }

        private static System.Drawing.Color WindowsDialog(System.Drawing.Color color, IWin32Window w, bool fullOpen, out bool ok)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = color;
                dialog.FullOpen = fullOpen;
                ok = dialog.ShowDialog(w) == DialogResult.OK;
                byte alpha = color.IsEmpty ? ((byte) 0) : color.A;
                return (ok ? System.Drawing.Color.FromArgb(alpha, dialog.Color) : color);
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return Graphics3D.TransparentColor((int) this.ndTransp.Value, this.color);
            }
        }
    }
}

