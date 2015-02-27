namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control to manipulate connect options.
    /// </summary>
    public class ConnectOptions : UserControl
    {
        private ComboBox Broker;
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox Mode;
        private Button MoreButton;
        private OptionsBase options = new IBOptions();
        private TextBox Password;
        private CheckBox RunAtServer;
        private TextBox User;

        /// <summary></summary>
        public ConnectOptions()
        {
            this.InitializeComponent();
        }

        private void Broker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Mode.Text.Length != 0)
            {
                this.FillMode(ProviderType.All.Find(this.Broker.Text).Id);
                if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.CyberTrader)
                {
                    this.options = new CTOptions(this.options);
                    this.Password.Text = "";
                    this.User.Text = "";
                    this.Password.Enabled = true;
                    this.User.Enabled = true;
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.Dtn)
                {
                    this.options = new DtnOptions(this.options);
                    this.Password.Text = "";
                    this.User.Text = "";
                    this.Password.Enabled = false;
                    this.User.Enabled = false;
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.ESignal)
                {
                    this.options = new ESignalOptions(this.options);
                    this.Password.Text = "";
                    this.Password.Enabled = false;
                    this.User.Enabled = true;
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.InteractiveBrokers)
                {
                    this.options = new IBOptions(this.options);
                    ((IBOptions) this.options).Connect2RunningTws = true;
                    ((IBOptions) this.options).Port = new IBOptions().DefaultPort;
                    if ((ModeType.All.Find(this.Mode.Text).Id == ModeTypeId.Demo) || (ModeType.All.Find(this.Mode.Text).Id == ModeTypeId.Test))
                    {
                        this.Password.Text = new IBOptions().DemoPassword;
                        this.User.Text = new IBOptions().DemoUser;
                        this.Password.Enabled = false;
                        this.User.Enabled = false;
                    }
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.MBTrading)
                {
                    this.options = new MbtOptions(this.options);
                    this.Password.Text = "";
                    this.User.Text = "";
                    this.Password.Enabled = true;
                    this.User.Enabled = true;
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.Patsystems)
                {
                    this.options = new PatsOptions(this.options);
                    if (ModeType.All.Find(this.Mode.Text).Id == ModeTypeId.Demo)
                    {
                        this.Password.Enabled = false;
                        this.User.Enabled = false;
                    }
                    this.Password.Text = "";
                    this.User.Text = "";
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.TrackData)
                {
                    this.options = new TrackOptions(this.options);
                    this.Password.Text = "";
                    this.User.Text = "";
                    this.Password.Enabled = true;
                    this.User.Enabled = true;
                }
                else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.Yahoo)
                {
                    this.options = new YahooOptions(this.options);
                    this.Password.Text = "";
                    this.Password.Enabled = false;
                    this.User.Enabled = false;
                }
                else
                {
                    this.options = new OptionsBase(this.options);
                    this.options.TMPort = new OptionsBase().TMDefaultPort;
                    this.Password.Enabled = true;
                    this.User.Enabled = true;
                }
                OptionsBase base2 = OptionsBase.Restore(ProviderType.All.Find(this.Broker.Text), ModeType.All.Find(this.Mode.Text));
                if (base2 != null)
                {
                    this.options = base2;
                    this.Data2Gui(this.options);
                }
            }
        }

        private void ConnectOptions_Load(object sender, EventArgs e)
        {
            foreach (ProviderType type in ProviderType.All.Values)
            {
                if (type.Id != ProviderTypeId.Simulation)
                {
                    this.Broker.Items.Add(type.Name);
                }
            }
            this.Broker.SelectedItem = ProviderType.All[ProviderTypeId.InteractiveBrokers].Name;
            this.FillMode(ProviderTypeId.InteractiveBrokers);
            OptionsBase base2 = OptionsBase.Restore(ProviderType.All[ProviderTypeId.InteractiveBrokers], ModeType.All[ModeTypeId.Demo]);
            if (base2 != null)
            {
                this.options = base2;
            }
            this.Data2Gui(this.options);
        }

        /// <summary>
        /// Initializes the Gui according the data value.
        /// </summary>
        /// <param name="options">Initial data values.</param>
        public void Data2Gui(OptionsBase options)
        {
            this.options = options;
            this.Broker.SelectedItem = options.Provider.Name;
            this.Mode.SelectedItem = options.Mode.Name;
            this.Password.Text = options.Password;
            this.RunAtServer.Checked = options.RunAtServer;
            this.User.Text = options.User;
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
        /// Fill the Mode combobox with appropriate values
        /// </summary>
        private void FillMode(ProviderTypeId broker)
        {
            this.Mode.Items.Clear();
            foreach (ModeType type in ModeType.All.Values)
            {
                if (type.Id == ModeTypeId.Test)
                {
                    if (broker == ProviderTypeId.Patsystems)
                    {
                        this.Mode.Items.Add(type.Name);
                    }
                }
                else
                {
                    this.Mode.Items.Add(type.Name);
                }
            }
            this.Mode.SelectedItem = ModeType.All[ModeTypeId.Demo].Name;
        }

        /// <summary>
        /// Returns the data values according the current Gui settings.
        /// </summary>
        public OptionsBase Gui2Data()
        {
            this.options.Provider = ProviderType.All.Find(this.Broker.Text);
            this.options.Mode = ModeType.All.Find(this.Mode.Text);
            this.options.Password = this.Password.Text;
            this.options.User = this.User.Text;
            this.options.RunAtServer = this.RunAtServer.Checked;
            return this.options;
        }

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.User = new TextBox();
            this.Password = new TextBox();
            this.label2 = new Label();
            this.label4 = new Label();
            this.Broker = new ComboBox();
            this.MoreButton = new Button();
            this.Mode = new ComboBox();
            this.label3 = new Label();
            this.RunAtServer = new CheckBox();
            base.SuspendLayout();
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x40, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "&User:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.User.Location = new Point(0x58, 0);
            this.User.Name = "User";
            this.User.Size = new Size(120, 20);
            this.User.TabIndex = 1;
            this.User.Text = "";
            this.Password.Location = new Point(0x58, 0x20);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new Size(120, 20);
            this.Password.TabIndex = 2;
            this.Password.Text = "";
            this.label2.Location = new Point(0, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x40, 0x17);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Password:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.label4.Location = new Point(0, 0x40);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 0x17);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Broker:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.Broker.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Broker.Location = new Point(0x58, 0x40);
            this.Broker.Name = "Broker";
            this.Broker.Size = new Size(120, 0x15);
            this.Broker.Sorted = true;
            this.Broker.TabIndex = 3;
            this.Broker.SelectedIndexChanged += new EventHandler(this.Broker_SelectedIndexChanged);
            this.MoreButton.Location = new Point(0xa8, 0x80);
            this.MoreButton.Name = "MoreButton";
            this.MoreButton.Size = new Size(40, 0x15);
            this.MoreButton.TabIndex = 6;
            this.MoreButton.Text = "...";
            this.MoreButton.Click += new EventHandler(this.MoreButton_Click);
            this.Mode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Mode.Location = new Point(0x58, 0x60);
            this.Mode.Name = "Mode";
            this.Mode.Size = new Size(120, 0x15);
            this.Mode.Sorted = true;
            this.Mode.TabIndex = 4;
            this.Mode.SelectedIndexChanged += new EventHandler(this.Mode_SelectedIndexChanged);
            this.label3.Location = new Point(0, 0x60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x17);
            this.label3.TabIndex = 10;
            this.label3.Text = "&Mode:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.RunAtServer.Enabled = false;
            this.RunAtServer.Location = new Point(-1, 0x80);
            this.RunAtServer.Name = "RunAtServer";
            this.RunAtServer.RightToLeft = RightToLeft.Yes;
            this.RunAtServer.TabIndex = 5;
            this.RunAtServer.Text = "&Run at server";
            this.RunAtServer.TextAlign = ContentAlignment.MiddleRight;
            base.Controls.Add(this.RunAtServer);
            base.Controls.Add(this.Mode);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.MoreButton);
            base.Controls.Add(this.Broker);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.Password);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.User);
            base.Controls.Add(this.label1);
            base.Name = "ConnectOptions";
            base.Size = new Size(0xd0, 0x98);
            base.Load += new EventHandler(this.ConnectOptions_Load);
            base.ResumeLayout(false);
        }

        private void Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ModeType.All.Find(this.Mode.Text).Id == ModeTypeId.Live)
            {
                this.Password.Enabled = true;
                this.User.Enabled = true;
            }
            else if (ModeType.All.Find(this.Mode.Text).Id == ModeTypeId.Demo)
            {
                if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.InteractiveBrokers)
                {
                    this.Password.Text = new IBOptions().DemoPassword;
                    this.User.Text = new IBOptions().DemoUser;
                    this.Password.Enabled = false;
                    this.User.Enabled = false;
                }
                else
                {
                    this.Password.Text = "";
                    this.User.Text = "";
                }
            }
            else if (ModeType.All.Find(this.Mode.Text).Id == ModeTypeId.Test)
            {
                if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.InteractiveBrokers)
                {
                    this.Password.Text = new IBOptions().DemoPassword;
                    this.User.Text = new IBOptions().DemoUser;
                }
                this.Password.Enabled = true;
                this.User.Enabled = true;
            }
            OptionsBase base2 = OptionsBase.Restore(ProviderType.All.Find(this.Broker.Text), ModeType.All.Find(this.Mode.Text));
            if (base2 != null)
            {
                this.options = base2;
                this.Data2Gui(this.options);
            }
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.CyberTrader)
            {
                MoreCTConnectOptionsForm form = new MoreCTConnectOptionsForm();
                form.Data2Gui((CTOptions) this.options);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    form.Gui2Data((CTOptions) this.options);
                }
            }
            else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.InteractiveBrokers)
            {
                MoreConnectionOptionsForm form2 = new MoreConnectionOptionsForm();
                form2.Data2Gui((IBOptions) this.options);
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    form2.Gui2Data((IBOptions) this.options);
                }
            }
            else if (ProviderType.All.Find(this.Broker.Text).Id == ProviderTypeId.Patsystems)
            {
                MorePatsConnectOptionsForm form3 = new MorePatsConnectOptionsForm();
                form3.Data2Gui((PatsOptions) this.options);
                if (form3.ShowDialog() == DialogResult.OK)
                {
                    form3.Gui2Data((PatsOptions) this.options);
                }
            }
        }
    }
}

