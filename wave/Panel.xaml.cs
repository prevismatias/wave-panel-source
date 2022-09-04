using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DiscordRPC;
using Newtonsoft.Json;
using SafeGuard;
using wave.deps.funcs;

namespace wave
{
    /// <summary>
    /// Interaction logic for Panel.xaml
    /// </summary>
    //made by prev
    public partial class Panel : Window
    {
        public Panel()
        {
            InitializeComponent();
            Tools.ProcessCheck();
            SafeCheck.Md5Check();
            ResponseInformation.vpnServers = SafeVPNConnect.GetAllVPNServers(ResponseInformation.loginresponse, ResponseInformation.Password);
        }

        #region RPC
        public DiscordRpcClient client;
        void Deinitialize()
        {
            client.Dispose();
        }
        void Initialize(string page)
        {
            WebClient wb = new WebClient();
            string RPC_ID = wb.DownloadString(pastebin.RPC_ID);
            WebClient wb2 = new WebClient();
            string link = wb2.DownloadString(pastebin.DiscordServer_Link);
            client = new DiscordRpcClient(RPC_ID);

            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = page,
                State = "V2 BETA",
                Timestamps = Timestamps.Now,

                Buttons = new DiscordRPC.Button[]
                        {
                            new DiscordRPC.Button() { Label = "Discord Server", Url = link }
                        },
                Assets = new Assets()
                {

                    LargeImageKey = "logo",
                }
            });
        }
        #endregion
        #region Window
        private void welcomeText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void exitPanel_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.cooldown)
            {
                MessageBox.Show($"cooldown active\ntime left: {cooldown - count} ", SafeGuardTitle.safeguardtitle);
            }
            else
            {
                Properties.Settings.Default.Save();
                System.Environment.Exit(1);
            }
        }
        private void minimizePanel_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion
        #region Stresser
        private async void StresserSnackNoti(string message)
        {
            if (stressersnackbar.IsActive == true)
            {
                await Task.Delay(5000);
                stressersnackbar.Message.Content = message;
                stressersnackbar.IsActive = true;
                await Task.Delay(4000);
                stressersnackbar.IsActive = false;
            }
            else
            {
                stressersnackbar.Message.Content = message;
                stressersnackbar.IsActive = true;
                await Task.Delay(4000);
                stressersnackbar.IsActive = false;
            }
        }
        private async void ShowMOTD()
        {
            if (motdSnack.IsActive == true)
            {
                await Task.Delay(9000);
                motdSnack.IsActive = true;
                await Task.Delay(7500);
                motdSnack.IsActive = false;
            }
            else
            {
                motdSnack.IsActive = true;
                await Task.Delay(7500);
                motdSnack.IsActive = false;
            }
        }
        private static System.Timers.Timer cooldowntimer;
        static int count = 0;
        static int cooldown;
        private void setCooldown(int time)
        {
            Properties.Settings.Default.cooldown = true;
            Properties.Settings.Default.cooldowntime = time;
            count = 0;
            cooldown = time;
            atkBtn.IsEnabled = false;
            cooldowntimer = new System.Timers.Timer(1000);
            cooldowntimer.Elapsed += OnTimedEvent;
            cooldowntimer.AutoReset = true;
            cooldowntimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            #region Levels
            /*
            int cooldowntime;
            switch (ResponseInformation.loginresponse.Level)
            {
                case 1:
                    cooldowntime = 40;
                    break;
                case 2:
                    cooldowntime = 40;
                    break;
                case 11:
                    cooldowntime = 5;
                    break;
                default:
                    cooldowntime = 60;
                    break;
            }
            */
            #endregion
            count++;
            Dispatcher.Invoke(() => { atkBtn.Content = $"COOLDOWN: {cooldown - count}"; });
            if (count >= cooldown)
            {
                cooldowntimer.Stop();
                cooldowntimer.Dispose();
                count = 0;
                Properties.Settings.Default.cooldown = false;
                Properties.Settings.Default.cooldowntime = 0;
                this.Dispatcher.Invoke(() =>
                {
                    StresserSnackNoti("cooldown ended");
                    atkBtn.IsEnabled = true;
                    atkBtn.Content = "SEND ATTACK";
                });
            }
        }
        #region datagrid
        public class attacklog
        {
            public string IP { get; set; }
            public string Port { get; set; }
            public string Method { get; set; }
            public string Time { get; set; }
            public string Sent { get; set; }
        }
        #endregion

        public static List<string> non_methods = new List<string> {
            "",
            " ",
            "[ Home Methods ]",
            "[ UDP Methods ]",
            "[ TCP Methods ]",
            "[ Bypass Methods ]"
        };

        private string sendatk(string username, string password, string programId, string ip, string port, string method, string time, bool ASync)
        {
            string response = ClientFunctions.AttackRequest(username, password, programId, ip, port, method, time, ASync);
            return response;
        }

        private async void atkBtn_Click(object sender, RoutedEventArgs e)
        {
            atkBtn.IsEnabled = false;
            #region Levels
            int maxtime;
            //int maxcons;
            int cooldowntime;
            int time = Int32.Parse(timeText.Text);
            switch (ResponseInformation.loginresponse.Level)
            {
                case 1:
                    maxtime = 300;
                    //maxcons = 1;
                    cooldowntime = time + 40;
                    //cooldowntime = 20;
                    break;
                case 2:
                    maxtime = 900;
                    //maxcons = 1;
                    cooldowntime = time + 40;
                    //cooldowntime = 20;
                    break;
                case 11:
                    maxtime = 1800;
                    //maxcons = 3;
                    cooldowntime = 5;
                    break;
                default:
                    maxtime = 1200;
                    //maxcons = 1;
                    cooldowntime = time;
                    break;
            }
            #endregion
            #region vars
            string username = ResponseInformation.loginresponse.UserName;
            string password = ResponseInformation.Password;
            string programId = ProgramInformation.ProgramId;
            string ip = ipText.Text;
            string port = portText.Text;
            string method = methodText.Text;
            string time_str = timeText.Text;
            bool async = true;
            if (ip == "" & port == "" & time_str == "")
            {
                StresserSnackNoti("Null Inputs");
                atkBtn.IsEnabled = true;
                return;
            }
            DateTime localDate = DateTime.Now;
            WebClient webClient1 = new WebClient();
            string vipmethods = webClient1.DownloadString(pastebin.vipmethods_link);
            List<string> vipmethodlist = new List<string>(vipmethods.Split(',').ToList());
            #endregion
            if (time > maxtime)
            {
                StresserSnackNoti($"Time Must Be Lower Than {maxtime} Seconds");
                atkBtn.IsEnabled = true;
                return;
            }
            if (non_methods.Contains(method))
            {
                StresserSnackNoti("Invalid Method");
                atkBtn.IsEnabled = true;
                return;
            }
            if (ResponseInformation.loginresponse.Level == 1)
            {
                if (vipmethodlist.Contains(method))
                {
                    StresserSnackNoti("You Do Not Have Access To This Method");
                    atkBtn.IsEnabled = true;
                    return;
                }
                else
                {
                    Task<string> task = new Task<string>(() => sendatk(username, password, programId, ip, port, method, time_str, async));
                    task.Start();
                    string response = await task;
                    StresserSnackNoti(response);
                    //string AttkLog = $"Attack Request\nIP: **{ip}**\nPort: **{port}**\nTime: **{timeText.Text}**\nMethod: **{method}**\nCool Down: **{cooldowntime}secs**\nClient IP: {Tools.GetClientIP()}\nISP: || NIGGA BALLS ||";
                    //await Task.Run(() => DiscordUtils.DiscordLog("attack", ResponseInformation.loginresponse.UserName, AttkLog));
                    StresserSnackNoti($"Cooldown Active, {cooldowntime}secs");
                    setCooldown(cooldowntime);
                    attacklog a = new attacklog();
                    a.IP = ip;
                    a.Port = port;
                    a.Method = method;
                    a.Time = timeText.Text;
                    a.Sent = localDate.ToString();
                    logGrid.Items.Add(a);
                    if (Properties.Settings.Default.savelogs)
                    {
                        WriteToAttackToFile(a);
                    }
                    return;
                }
            }
            else
            {
                Task<string> task = new Task<string>(() => sendatk(username, password, programId, ip, port, method, time_str, async));
                task.Start();
                string response = await task;
                StresserSnackNoti(response);
                //string AttkLog = $"Attack Request\nIP: **{ip}**\nPort: **{port}**\nTime: **{timeText.Text}**\nMethod: **{method}**\nCool Down: **{cooldowntime}secs**\nClient IP: {Tools.GetClientIP()}\nISP: || NIGGA BALLS ||";
                //await Task.Run(() => DiscordUtils.DiscordLog("attack", ResponseInformation.loginresponse.UserName, AttkLog));
                StresserSnackNoti($"Cooldown Active, {cooldowntime}secs");
                setCooldown(cooldowntime);
                attacklog a = new attacklog();
                a.IP = ip;
                a.Port = port;
                a.Method = method;
                a.Time = timeText.Text;
                a.Sent = localDate.ToString();
                logGrid.Items.Add(a);
                if (Properties.Settings.Default.savelogs)
                {
                    WriteToAttackToFile(a);
                }
                return;
            }
        }

        private List<attacklog> ReadAttackFile()
        {
            List<attacklog> uso = new List<attacklog>();
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "wave");
            string file = $"{path}\\AttackLogs.json";
            if (!Directory.Exists(path))
                return null;
            if (!File.Exists(file))
                return null;
            else
            {
                uso = JsonConvert.DeserializeObject<List<attacklog>>(File.ReadAllText(file));
                if (uso != null)
                    return uso;
                else
                    return null;
            }
        }

        private bool WriteToAttackToFile(attacklog UserObject)
        {
            List<attacklog> uso = new List<attacklog>();
            uso.Add(UserObject);
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "wave");
            string file = $"{path}\\AttackLogs.json";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(file))
            {
                File.Create(file).Close();
                File.WriteAllText(file, JsonConvert.SerializeObject(uso));
                return true;
            }
            else
            {
                try
                {
                    uso = null;
                    uso = JsonConvert.DeserializeObject<List<attacklog>>(File.ReadAllText(file));
                    uso.Add(UserObject);
                    File.WriteAllText(file, JsonConvert.SerializeObject(uso));
                    return true;
                }
                catch
                {
                    File.Move(file, $"{path}\\OldAttackLogs.json");
                    return false;
                }
            }
        }

        #region Tools

        private string geoip(string ip, string username, string password, string programId)
        {
            string info = SafeGuard.Tools.GeoIP(ip, username, password, programId);
            return info;
        }

        private async void geoipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (toolipText.Text == "")
            {
                if (ipText.Text == "")
                {
                    StresserSnackNoti("No IP Address Inputted");
                }
                else
                {
                    geoipBtn.IsEnabled = false;
                    string ip = toolipText.Text;
                    string username = ResponseInformation.loginresponse.UserName;
                    string password = ResponseInformation.Password;
                    string programId = ProgramInformation.ProgramId;
                    Task<string> task = new Task<string>(() => geoip(ip, username, password, programId));
                    task.Start();
                    string response = await task;
                    response = response.Replace("\"", string.Empty);
                    response = response.Replace("\\r\\n", Environment.NewLine);
                    MessageBox.Show(response, SafeGuardTitle.safeguardtitle);
                    geoipBtn.IsEnabled = true;
                }
            }
            else
            {
                geoipBtn.IsEnabled = false;
                string ip = toolipText.Text;
                string username = ResponseInformation.loginresponse.UserName;
                string password = ResponseInformation.Password;
                string programId = ProgramInformation.ProgramId;
                Task<string> task = new Task<string>(() => geoip(ip, username, password, programId));
                task.Start();
                string response = await task;
                response = response.Replace("\"", string.Empty);
                response = response.Replace("\\r\\n", Environment.NewLine);
                MessageBox.Show(response, SafeGuardTitle.safeguardtitle);
                geoipBtn.IsEnabled = true;
            }
        }

        private string portscan(string ip, string username, string password, string programId)
        {
            string ports = SafeGuard.Tools.PortScan(ip, username, password, programId);
            return ports;
        }

        private async void portscanBtn_Click(object sender, RoutedEventArgs e)
        {
            if (toolipText.Text == "")
            {
                if (ipText.Text == "")
                {
                    StresserSnackNoti("No IP Address Inputted");
                }
                else
                {
                    portscanBtn.IsEnabled = false;
                    string ip = ipText.Text;
                    string username = ResponseInformation.loginresponse.UserName;
                    string password = ResponseInformation.Password;
                    string programId = ProgramInformation.ProgramId;
                    Task<string> task = new Task<string>(() => portscan(ip, username, password, programId));
                    task.Start();
                    string open_ports = await task;
                    portscanBtn.IsEnabled = true;
                    //MessageBox.Show($"OPEN PORTS: {open_ports}", SafeGuardTitle.safeguardtitle);
                    StresserSnackNoti($"OPEN PORTS: {open_ports}");
                }
            }
            else
            {
                portscanBtn.IsEnabled = false;
                string ip = toolipText.Text;
                string username = ResponseInformation.loginresponse.UserName;
                string password = ResponseInformation.Password;
                string programId = ProgramInformation.ProgramId;
                Task<string> task = new Task<string>(() => portscan(ip, username, password, programId));
                task.Start();
                string open_ports = await task;
                //MessageBox.Show($"OPEN PORTS: {open_ports}", SafeGuardTitle.safeguardtitle);
                StresserSnackNoti($"OPEN PORTS: {open_ports}");
                portscanBtn.IsEnabled = true;
            }
        }

        private void icmppingBtn_Click(object sender, RoutedEventArgs e)
        {
            if (papingipText.Text == "")
            {
                if (ipText.Text == "")
                {
                    MessageBox.Show("No IP Address Inputted", SafeGuardTitle.safeguardtitle);
                }
                else
                {
                    SafeGuard.Tools.Ping(ipText.Text);
                }
            }
            else
            {
                SafeGuard.Tools.Ping(papingipText.Text);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (papingipText.Text == "" & papingportText.Text == "")
            {
                if (ipText.Text == "" & portText.Text == "")
                {
                    MessageBox.Show("No IP Address/port Inputted", SafeGuardTitle.safeguardtitle);
                }
                else
                {
                    new Process
                    {
                        StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "paping.exe",
                    Arguments = ipText.Text + " -p " + portText.Text
                }
                    }.Start();
                    string arguments = "paping.exe" + ipText.Text + " -p " + portText.Text;
                    Process.Start("ping.exe", arguments);
                }
            }
            else if (papingportText.Text == "")
            {
                MessageBox.Show("No IP Address/port Inputted", SafeGuardTitle.safeguardtitle);
            }
            else
            {
                new Process
                {
                    StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "paping.exe",
                    Arguments = papingipText.Text + " -p " + papingportText.Text
                }
                }.Start();
                string arguments = "paping.exe" + papingipText.Text + " -p " + papingportText.Text;
                Process.Start("ping.exe", arguments);
            }
        }
        #endregion

        private void togglemethodsToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string L7methods = wb.DownloadString(pastebin.L7list_link);
            methodText.ItemsSource = new List<string>(L7methods.Split(',').ToList());
        }

        private void togglemethodsToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string L4methods = wb.DownloadString(pastebin.L4list_link);
            methodText.ItemsSource = new List<string>(L4methods.Split(',').ToList());
        }

        private async void clearattackhistoryButton_Click(object sender, RoutedEventArgs e)
        {
            logGrid.Items.Clear();
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "wave");
            string file = $"{path}\\AttackLogs.json";
            try { if (File.Exists(file)) { File.Delete(file); StresserSnackNoti("History Cleared!"); } } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion
        #region Home
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShowMOTD();
        }

        public class DataItem
        {
            public string MOTD { get; set; }
        }
        #endregion
        #region Discord

        private void togglerpcToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Initialize("Panel");
            Properties.Settings.Default.RPC = true;
            Properties.Settings.Default.Save();
        }

        private void togglerpcToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Deinitialize();
            Properties.Settings.Default.RPC = false;
            Properties.Settings.Default.Save();
        }

        private void discordserverButton_Click(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string link = wb.DownloadString(pastebin.DiscordServer_Link);
            System.Diagnostics.Process.Start(link);
        }

        private void telegramButton_Click(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string link = wb.DownloadString(pastebin.Telegram_Link);
            System.Diagnostics.Process.Start(link);
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region spaget
            WebClient wb = new WebClient();
            string MOTD = wb.DownloadString(pastebin.MOTD_link);
            motdSnack.Message.Content = MOTD;
            ShowMOTD();
            string L4methods = wb.DownloadString(pastebin.L4list_link);
            methodText.ItemsSource = new List<string>(L4methods.Split(',').ToList());
            welcomeText.Content = $"Welcome, {ResponseInformation.loginresponse.UserName}";
            clientnameLabel.Content = ResponseInformation.loginresponse.UserName;
            TimeSpan timeLeft = ResponseInformation.loginresponse.ExpirationDate - DateTime.Now;
            planexpirationLabel.Content = $"Plan Expiration: {timeLeft.Days} Day(s) Left";
            if (timeLeft.Days > 100)
                planexpirationLabel.Content = "Plan Expiration: Lifetime";
            if (timeLeft.Days < 1)
                planexpirationLabel.Content = $"Plan Expiration: {timeLeft.Hours} Hour(s) Left";
            #region Levels
            int maxtime;
            //int maxcons;
            //int cooldowntime;
            string rank;
            switch (ResponseInformation.loginresponse.Level)
            {
                case 1:
                    maxtime = 300;
                    //maxcons = 1;
                    //cooldowntime = 60;
                    rank = "Basic";
                    break;
                case 2:
                    maxtime = 900;
                    //maxcons = 1;
                    //cooldowntime = 60;
                    rank = "VIP";
                    break;
                case 11:
                    maxtime = 1800;
                    //maxcons = 3;
                    //cooldowntime = 5;
                    rank = "Developer";
                    break;
                default:
                    maxtime = 1200;
                    //maxcons = 1;
                    //cooldowntime = 60;
                    rank = "Basic";
                    break;
            }
            #endregion
            levelLabel.Content = $"Plan: {rank}";
            maxtimeLabel.Content = $"Max Time: {maxtime}";
            ResponseInformation.count = DeveloperFunctions.CountCall(ProgramInformation.ProgramId);
            clientsLabel.Content = $"Clients: {ResponseInformation.count.UserCount}";
            serversLabel.Content = $"Servers: {ResponseInformation.count.BotnetCount}";
            bool rpc = Properties.Settings.Default.RPC;
            togglerpcToggleButton.IsChecked = rpc;
            bool logattacks = Properties.Settings.Default.savelogs;
            LogAttacksToggle.IsChecked = logattacks;
            bool autologin = Properties.Settings.Default.autologin;
            AutoLoginToggle.IsChecked = autologin;
            bool topmost = Properties.Settings.Default.topmost;
            PriorityToggle.IsChecked = topmost;
            var bitmapImage = new BitmapImage();
            string discordIDs = wb.DownloadString(pastebin.IDs);
            var ID = new List<string>(discordIDs.Split(',').ToList());
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("https://discord.c99.nl/widget/theme-4/" + ID[0] + ".png"); ;
            bitmapImage.EndInit();
            ownerbannerImage.Source = bitmapImage;
            var bitmapImage2 = new BitmapImage();
            bitmapImage2.BeginInit();
            bitmapImage2.UriSource = new Uri("https://discord.c99.nl/widget/theme-4/" + ID[1] + ".png"); ;
            bitmapImage2.EndInit();
            devbannerImage.Source = bitmapImage2;
            var bitmapImage3 = new BitmapImage();
            bitmapImage3.BeginInit();
            bitmapImage3.UriSource = new Uri("https://discord.c99.nl/widget/theme-4/" + ID[2] + ".png"); ;
            bitmapImage3.EndInit();
            adminbannerImage.Source = bitmapImage3;
            List<attacklog> uso = ReadAttackFile();
            if (uso != null)
            {
                logGrid.Items.Clear();
                foreach (attacklog Attack in uso)
                {
                    logGrid.Items.Insert(0, Attack);
                }
                cooldownBar.Maximum = 1;
                cooldownBar.Value = 1;
                #endregion
            }
            color1Theme.Color = Properties.Settings.Default.color1;
            color2Theme.Color = Properties.Settings.Default.color2;
            bool incooldown = Properties.Settings.Default.cooldown;
            if (incooldown == true)
            {
                setCooldown(Properties.Settings.Default.cooldowntime);
            }
        }
        private void renewBtn_Click(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string link = wb.DownloadString(pastebin.DiscordServer_Link);
            System.Diagnostics.Process.Start(link);
        }

        private void purchasebasicBtn_Click(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string link = wb.DownloadString(pastebin.sellix_basic);
            System.Diagnostics.Process.Start(link);
        }

        private void purchasevipBtn_Click(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string link = wb.DownloadString(pastebin.sellix_vip);
            System.Diagnostics.Process.Start(link);
        }

        private void rainbowpingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process[] proc = Process.GetProcessesByName("rainbow");
                new Process
                {
                    StartInfo =
                {
                    UseShellExecute = false,
                    FileName = "rainbow.bat",
                }
                }.Start();
            }

            catch
            {
                MessageBox.Show("Error: Rainbow Pinger Not Found ", SafeGuardTitle.safeguardtitle);
                return;
            }
        }

        private void cooldownBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void LogAttacksToggle_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.savelogs = true;
            Properties.Settings.Default.Save();
        }

        private void LogAttacksToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.savelogs = false;
            Properties.Settings.Default.Save();
        }

        private void AutoLoginToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.autologin = false;
            Properties.Settings.Default.Save();
        }

        private void AutoLoginToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (!Properties.Settings.Default.save_pass)
            {
                MessageBox.Show("Please Click Save Password Next Time You Login", SafeGuardTitle.safeguardtitle);
                AutoLoginToggle.IsChecked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.autologin = true;
                Properties.Settings.Default.Save();
            }
        }
        public PropertyPath ColorPath { get; set; }
        public PropertyPath SelectedColor { get; set; }
        public ICommand ChangeToPrimaryCommand { get; }
        public ICommand ChangeToSecondaryCommand { get; }
        private void color1Theme_ColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            Properties.Settings.Default.color1 = color1Theme.Color;
            Properties.Settings.Default.Save();
            //border.BorderBrush = color1Theme.Color();
        }

        private void color2Theme_ColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            Properties.Settings.Default.color2 = color2Theme.Color;
            Properties.Settings.Default.Save();
        }

        private void savethemeBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.color1 = color1Theme.Color;
            Properties.Settings.Default.color2 = color2Theme.Color;
            Properties.Settings.Default.Save();
            MessageBox.Show($"Saved Theme\n {Properties.Settings.Default.color1}\n{Properties.Settings.Default.color2}", SafeGuardTitle.safeguardtitle);
        }

        private void PriorityToggle_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            Properties.Settings.Default.topmost = true;
            Properties.Settings.Default.Save();
        }

        private void PriorityToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
            Properties.Settings.Default.topmost = false;
            Properties.Settings.Default.Save();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string bruh = "test";
            StresserSnackNoti(bruh);

        }

        private void clearattacktextboxesButton_Click(object sender, RoutedEventArgs e)
        {
            ipText.Text = "";
            portText.Text = "";
            timeText.Text = "";
            methodText.Text = "";
            toolipText.Text = "";
            papingipText.Text = "";
            papingportText.Text = "";
            StresserSnackNoti("Text Boxes Cleared!");
        }
    }
}