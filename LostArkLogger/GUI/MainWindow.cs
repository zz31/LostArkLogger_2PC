using LostArkLogger.Utilities;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace LostArkLogger
{
    public partial class MainWindow : Form//, INotifyPropertyChanged
    {
        Parser sniffer;
        Overlay overlay;
        HttpBridge httpBridge;
        private int _packetCount;
        string[] startArgs;
        //public event PropertyChangedEventHandler PropertyChanged;

        public string PacketCount
        {
            get { return "Packets: " + _packetCount; }
        }

        public MainWindow(string[] args)
        {
            startArgs = args;
            InitializeComponent();
            Oodle.Init();
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            regionSelector.DataSource = Enum.GetValues(typeof(Region));
            regionSelector.SelectedItem = Properties.Settings.Default.Region;
            regionSelector.SelectedIndexChanged += new EventHandler(regionSelector_SelectedIndexChanged);
            loggedPacketCountLabel.Text = "Packets: 0";
            //loggedPacketCountLabel.DataBindings.Add("Text", this, nameof(PacketCount));
            displayName.Checked = Properties.Settings.Default.DisplayNames;
            displayName.CheckedChanged += new EventHandler(displayName_CheckedChanged);
            //sniffModeCheckbox.Checked = Properties.Settings.Default.Npcap;
            this.FormClosed += new FormClosedEventHandler(form_CloseAll);
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                nicListBox.Items.Add(nic.Name);
            }

            if (args != null && args.Length != 0)
            {
                this.Text = "CONSOLE MODE";
                debugLog.Visible = false;
                addBgColor.Visible = false;
                cboxEnableLogger.Visible = false;
                lblSetBGColor.Visible = false;
                if (Properties.Settings.Default.LockedNICname.Length == 0 && Properties.Settings.Default.LockedRegionName.Length == 0)
                {
                    MessageBox.Show("Select nic and region to link with loa detail.");
                }
            } else
            {
                if (Properties.Settings.Default.LogEnabled == true)
                {
                    enableLogger_notice = true;
                    cboxEnableLogger.Checked = true;
                }
            }
            if (Properties.Settings.Default.LockedNICname.Length > 0 && Properties.Settings.Default.LockedRegionName.Length > 0)
            {
                regionSelector.SelectedItem = Properties.Settings.Default.LockedRegionName;
                nicListBox.SelectedItem = Properties.Settings.Default.LockedNICname;
                cbox_lockNic.Checked = true;
            }
        }

        private void form_CloseAll(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void weblink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/shalzuth/LostArkLogger");
        }

        private void debugLog_CheckedChanged(object sender, EventArgs e)
        {
            Logger.debugLog = debugLog.Checked;
        }

        private bool rs_init = false;
        private void regionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rs_init == false)
            {//block init index change event
                rs_init = true;
                return;
            }
            Properties.Settings.Default.Region = (Region)Enum.Parse(typeof(Region), regionSelector.Text);
            Properties.Settings.Default.Save();
            //Environment.Exit(0);
        }

        private void displayName_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisplayNames = displayName.Checked;
            Properties.Settings.Default.Save();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            loggedPacketCountLabel.Text = PacketCount;
            overlay.tryUpdate();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.OverlayPos_Right == true)
            {
                this.Location = new System.Drawing.Point(0, 0);
            } else
            {
                this.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 0);
            }
        }

        private void nicListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            nicListBox.Enabled = false;
            nicListBox.Visible = false;
            regionSelector.Enabled = false;
            regionSelector.Visible = false;
            if (startArgs != null && startArgs.Length != 0)
            {
                this.Visible = false;
                httpBridge = new HttpBridge();
                httpBridge.args = startArgs;
                httpBridge.Start(nicListBox.SelectedItem.ToString());
            } else
            {
                sniffer = new Parser();
                sniffer.onPacketTotalCount += (int totalPacketCount) =>
                {
                    _packetCount = totalPacketCount;
                };
                overlay = new Overlay();
                overlay.Show();
                if (Properties.Settings.Default.OverlayPos_Right == true)
                {
                    overlay.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width / 2, 0);
                } else
                {
                    overlay.Location = new System.Drawing.Point(0, 0);
                }
                overlay.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height);
                sniffer.startParse(nicListBox.SelectedItem.ToString());
                overlay.AddSniffer(sniffer);
                sniffer.onHpChange += overlay.onhpUpdate;
                addBgColor.Enabled = true;

                cbox_lockNic.Enabled = true;
                cbox_lockNic.Visible = true;
                lblSetBGColor.Enabled = true;
                cbox_lockNic.Text = regionSelector.SelectedItem.ToString() + " / "+ nicListBox.SelectedItem.ToString();
                cboxEnableLogger.Enabled = true;
                versionLabel.Enabled = true;

                timer1.Enabled = true;
            }
        }

        private void addBgColor_CheckedChanged(object sender, EventArgs e)
        {
            overlay.addBGColor = addBgColor.Checked;
        }

        private void cbox_lockNic_CheckedChanged(object sender, EventArgs e)
        {
            if (cbox_lockNic.Checked == true)
            {
                Properties.Settings.Default.LockedNICname = nicListBox.SelectedItem.ToString();
                Properties.Settings.Default.LockedRegionName = regionSelector.SelectedItem.ToString();
                Properties.Settings.Default.Save();
            } else
            {
                Properties.Settings.Default.LockedNICname = "";
                Properties.Settings.Default.LockedRegionName = "";
                Properties.Settings.Default.Save();
            }
        }

        private void lblSetBGColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;
            MyDialog.ShowHelp = true;
            MyDialog.Color = lblSetBGColor.ForeColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                lblSetBGColor.ForeColor = MyDialog.Color;
                Properties.Settings.Default.BackgroundColor = MyDialog.Color;
                overlay.bgColor = new System.Drawing.SolidBrush(MyDialog.Color);
                overlay.updateUI();
                Properties.Settings.Default.Save();
            }
        }

        private bool enableLogger_notice = false;
        private void cboxEnableLogger_CheckedChanged(object sender, EventArgs e)
        {
            if (enableLogger_notice == false)
            {
                switch(Properties.Settings.Default.Region)
                {
                    case LostArkLogger.Region.Korea:
                        MessageBox.Show("해당 기능은 Loa details용으로 로그를 생성하는 기능입니다.\n로그 생성 시 나중에 Loa Details로 전투 기록을 확인할 수 있습니다.");
                        break;
                    case LostArkLogger.Region.Steam:
                        MessageBox.Show("Generates logs that are compatible with Loa-details,\nyou can check combat history with Loa-details later.");
                        break;
                }
                enableLogger_notice = true;
            }
            Properties.Settings.Default.LogEnabled = cboxEnableLogger.Checked;
            Properties.Settings.Default.Save();
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.OverlayPos_Right == true)
            {
                this.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 0);
                overlay.Location = new System.Drawing.Point(0, 0);
                Properties.Settings.Default.OverlayPos_Right = false;
            } else
            {
                this.Location = new System.Drawing.Point(0, 0);
                overlay.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width / 2, 0);
                Properties.Settings.Default.OverlayPos_Right = true;
            }
            Properties.Settings.Default.Save();
        }
    }
}
