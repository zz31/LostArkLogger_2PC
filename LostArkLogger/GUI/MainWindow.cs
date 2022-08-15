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
        CharacterSearch cSearch;
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
            regionSelector.SelectedIndex = (int)Properties.Settings.Default.Region;
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

        private void checkUpdate_Click(object sender, EventArgs e)
        {
            using (var wc = new WebClient())
            {
                wc.Headers["User-Agent"] = "LostArkLogger";
                var json = wc.DownloadString(@"https://api.github.com/repos/shalzuth/LostArkLogger/releases/latest");
                var version = json.Substring(json.IndexOf("tag_name") + 11);
                version = version.Substring(0, version.IndexOf("\""));
                if (version == System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()) MessageBox.Show("Current version is up to date : " + version, "Version Info");
                else
                {
                    var exeUrl = json.Substring(json.IndexOf("browser_download_url") + 23);
                    exeUrl = exeUrl.Substring(0, exeUrl.IndexOf("\""));
                    var curFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
                    if (File.Exists(curFileName + ".old")) File.Delete(curFileName + ".old");
                    File.Move(curFileName, curFileName + ".old"); // need to delete this old breadcrumb elegantly. maybe on app start. not going to solve right now.
                    wc.DownloadFile(exeUrl, curFileName);
                    System.Diagnostics.Process.Start(curFileName);
                    Environment.Exit(0);
                }
            }
        }

        private void regionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Region = (Region)Enum.Parse(typeof(Region), regionSelector.Text);
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.Region == LostArkLogger.Region.Korea)
            {
                specCheck.Visible = true;
            } else
            {
                specCheck.Visible = false;
            }
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
            this.Location = new System.Drawing.Point(0, 0);
        }

        private void nicListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            nicListBox.Enabled = false;
            regionSelector.Enabled = false;
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
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PacketCount)));
                };
                overlay = new Overlay();
                overlay.Show();
                overlay.Location = new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width / 2, 0);
                overlay.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height);
                sniffer.startParse(nicListBox.SelectedItem.ToString());
                overlay.AddSniffer(sniffer);
                sniffer.onHpChange += overlay.onhpUpdate;
                addBgColor.Enabled = true;
                if (specCheck.Checked == true)
                {
                    overlay.specCheckerEnabled = specCheck.Checked;//enabled일때만 출력
                    sniffer.specCheckerEnabled = specCheck.Checked;//enabled일때 onnewPC 이벤트 넘김
                    cSearch.specCheckerEnabled = specCheck.Checked;//enabled일때만 검색
                    cSearch = new CharacterSearch();
                    sniffer.onNewZone += cSearch.resetLatestUser;
                    overlay.getLatestUserInfo += cSearch.getPlayerLast8;
                    overlay.updateUserInfo += cSearch.doParse;
                    cSearch.getLvl += overlay.GetLevel;
                    cSearch.onDataUpdated += overlay.updateUI;
                    sniffer.onNewPC += cSearch.onNewPC;
                }
                else if (specCheck.Checked == false)
                {
                    specCheck.Enabled = false;
                }
                timer1.Enabled = true;
            }
        }

        private void specCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (nicListBox.Enabled == true)
            {
                if (specCheck.Checked == true && Properties.Settings.Default.Region == LostArkLogger.Region.Steam)
                {
                    specCheck.Checked = false;
                    MessageBox.Show("This function uses an external website api to check other players information. So it only works on the KR server.");
                } else if (specCheck.Checked == true && Properties.Settings.Default.Region == LostArkLogger.Region.Korea)
                {
                    MessageBox.Show("해당 기능은 외부 웹사이트 api를 이용합니다.\n미터기를 실행한 PC가 인터넷에 연결되어 있지 않다면 오류만 나고 작동하지 않으니, 해당 기능을 해제해 주세요.\n\n또한, 현재 오류로 이벤트 보석은 보석 없음으로 표기되니 참고바랍니다.");
                }
            } else if (nicListBox.Enabled == false)
            {
                if (specCheck.Checked == true && Properties.Settings.Default.Region == LostArkLogger.Region.Steam)
                {
                    specCheck.Enabled = false;
                    specCheck.Checked = false;
                    MessageBox.Show("This function uses an external website api to check other players information. So it only works on the KR server.");
                } else if (Properties.Settings.Default.Region == LostArkLogger.Region.Korea)
                {
                    overlay.specCheckerEnabled = specCheck.Checked;
                    cSearch.specCheckerEnabled = specCheck.Checked;
                    //sniffer.specCheckerEnabled = specCheck.Checked;//off더라도 최근 유저목록은 꾸준히 갱신하고 on했을때 갱신
                    if (specCheck.Checked == true)
                    {
                        overlay.updateUI();
                        cSearch.doParse();
                    } else
                    {
                        overlay.updateUI();
                    }
                }
            }
        }

        private void addBgColor_CheckedChanged(object sender, EventArgs e)
        {
            overlay.addBGColor = addBgColor.Checked;
        }
    }
}
