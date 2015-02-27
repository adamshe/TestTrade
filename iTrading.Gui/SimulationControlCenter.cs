namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Form to control TradeMagic Gui in Simulation mode.
    /// </summary>
    public class SimulationControlCenter : Form
    {
        private Container components = null;
        private Connection connection = null;
        private Button goButton;
        private Button haltButton;
        private Label label1;
        private Label label2;
        private NumericUpDown speedNumericUpDown;
        private DateTimePicker startDateTimePicker;
        private bool updateTime = true;

        /// <summary>
        /// </summary>
        public SimulationControlCenter()
        {
            this.InitializeComponent();
        }

        private void connection_Timer(object sender, TimerEventArgs e)
        {
            if (this.updateTime)
            {
                this.startDateTimePicker.Value = e.Time;
            }
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

        private void goButton_Click(object sender, EventArgs e)
        {
            Trace.Assert(this.Connection != null, "Cbi.SimulationController.startButton_Click: set 'Connection' property");
            this.connection.SimulationSpeed = (double) this.speedNumericUpDown.Value;
            this.connection.Now = this.startDateTimePicker.Value;
        }

        private void haltButton_Click(object sender, EventArgs e)
        {
            Trace.Assert(this.Connection != null, "Cbi.SimulationController.startButton_Click: set 'Connection' property");
            this.connection.SimulationSpeed = 0.0;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.speedNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.goButton = new System.Windows.Forms.Button();
            this.haltButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.speedNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "S&tart:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "S&peed:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.Location = new System.Drawing.Point(40, 0);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(85, 20);
            this.startDateTimePicker.TabIndex = 2;
            // 
            // speedNumericUpDown
            // 
            this.speedNumericUpDown.DecimalPlaces = 1;
            this.speedNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.speedNumericUpDown.Location = new System.Drawing.Point(40, 20);
            this.speedNumericUpDown.Name = "speedNumericUpDown";
            this.speedNumericUpDown.Size = new System.Drawing.Size(85, 20);
            this.speedNumericUpDown.TabIndex = 3;
            this.speedNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.speedNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(131, 0);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(48, 20);
            this.goButton.TabIndex = 4;
            this.goButton.Text = "&Go";
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // haltButton
            // 
            this.haltButton.Location = new System.Drawing.Point(131, 20);
            this.haltButton.Name = "haltButton";
            this.haltButton.Size = new System.Drawing.Size(48, 20);
            this.haltButton.TabIndex = 5;
            this.haltButton.Text = "&Halt";
            this.haltButton.Click += new System.EventHandler(this.haltButton_Click);
            // 
            // SimulationControlCenter
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(203, 51);
            this.Controls.Add(this.haltButton);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.speedNumericUpDown);
            this.Controls.Add(this.startDateTimePicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SimulationControlCenter";
            this.ShowInTaskbar = false;
            this.Text = "Simulation control center";
            this.Load += new System.EventHandler(this.SimulationController_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SimulationController_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.speedNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        private void SimulationController_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        private void SimulationController_Load(object sender, EventArgs e)
        {
            this.startDateTimePicker.Format = DateTimePickerFormat.Custom;
            this.startDateTimePicker.CustomFormat = "HH:mm:ss";
            this.startDateTimePicker.Enter += new EventHandler(this.startDateTimePicker_Enter);
            this.startDateTimePicker.Leave += new EventHandler(this.startDateTimePicker_Leave);
        }

        private void startDateTimePicker_Enter(object sender, EventArgs e)
        {
            this.updateTime = false;
        }

        private void startDateTimePicker_Leave(object sender, EventArgs e)
        {
            this.updateTime = true;
        }

        /// <summary>
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
                this.connection.Timer += new TimerEventHandler(this.connection_Timer);
            }
        }
    }
}

