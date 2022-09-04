using System.Windows;
using System.Windows.Input;
using SafeGuard;
using System.Windows.Controls;
using System.Globalization;
using DiscordRPC;
using wave.deps.funcs;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Threading;
using System;

//made by prev
namespace wave
{
    public partial class MainWindow : Window
    {
        static Mutex singleton = new Mutex(true, "wave");
        public MainWindow()
        {
            InitializeComponent();
            Tools.ProcessCheck();
            SafeCheck.Md5Check();
            if (!singleton.WaitOne(System.TimeSpan.Zero, true))
            {
                MessageBox.Show("Duplicate Instance Found!", SafeGuardTitle.safeguardtitle);
                Application.Current.Shutdown();
            }
            Update.update();
        }
    #region RPC
    void Deinitialize()
        {
            client.Dispose();
        }
        public DiscordRpcClient client;
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
        public class NotEmptyValidationRule : ValidationRule
        {
            public string Message { get; set; }

            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                if (string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    return new ValidationResult(false, Message ?? "A value is required");
                }
                return ValidationResult.ValidResult;
            }
        }

        private void bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void exitLogin_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void minimizeLogin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private bool LoginVerif(string user, string password, string programId)
        {
            Tools.ProcessCheck();
            ResponseInformation.loginresponse = ClientFunctions.Login(user, password, programId);
            ResponseInformation.Password = password;
            if (ResponseInformation.loginresponse.Failure)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            loginBtn.IsEnabled = false;
            string username = usernameText.Text;
            string password = passwordText.Password;
            string programId = ProgramInformation.ProgramId;
            Task<bool> task = new Task<bool>(() => LoginVerif(username, password, programId));
            task.Start();
            bool login = await task;
            if (!login)
            {
                MessageBox.Show(ResponseInformation.loginresponse.Message, SafeGuardTitle.safeguardtitle);
                loginBtn.IsEnabled = true;
            }
            else
            {
                if (savePass.IsChecked == true)
                {
                    Properties.Settings.Default.username = usernameText.Text;
                    Properties.Settings.Default.password = passwordText.Password;
                    Properties.Settings.Default.save_pass = true;
                    Properties.Settings.Default.Save();
                    Panel p = new Panel();
                    this.Hide();
                    Deinitialize();
                    Thread.Sleep(1000);
                    MessageBox.Show("Login Success!", SafeGuardTitle.safeguardtitle);
                    Thread.Sleep(1000);
                    p.Show();
                    this.Close();
                }
                else
                {
                    Properties.Settings.Default.username = usernameText.Text;
                    Properties.Settings.Default.password = "";
                    Properties.Settings.Default.Save();
                    Panel p = new Panel();
                    this.Hide();
                    Deinitialize();
                    Thread.Sleep(1000);
                    MessageBox.Show("Login Success!", SafeGuardTitle.safeguardtitle);
                    Thread.Sleep(1000);
                    p.Show();
                    this.Close();
                }
            }
        }

        private void openReg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Deinitialize();
            regWindow r = new regWindow();
            r.Show();
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.autologin)
            {
                loginBtn.IsEnabled = false;
                loginBtn.Content = "AUTO LOGGING";
                string username = Properties.Settings.Default.username;
                string password = Properties.Settings.Default.password;
                string programId = ProgramInformation.ProgramId;
                Task<bool> task = new Task<bool>(() => LoginVerif(username, password, programId));
                task.Start();
                bool login = await task;
                if (!login)
                {
                    MessageBox.Show("Auto Login Failed: " + ResponseInformation.loginresponse.Message, SafeGuardTitle.safeguardtitle);
                    loginBtn.IsEnabled = true;
                    loginBtn.Content = "LOGIN";
                }
                else
                {
                    Panel p = new Panel();
                    this.Hide();
                    Thread.Sleep(1000);
                    p.Show();
                    this.Close();
                }
            }
            else
            {
                if (Properties.Settings.Default.RPC == true)
                {
                    Initialize("Login");
                }
                usernameText.Text = Properties.Settings.Default.username;
                passwordText.Password = Properties.Settings.Default.password;
                Properties.Settings.Default.Save();
                if (Properties.Settings.Default.save_pass == true)
                {
                    savePass.IsChecked = true;
                }
            }
        }

        private void openForgot_Click(object sender, RoutedEventArgs e)
        {
            restoreWindow r = new restoreWindow();
            this.Hide();
            r.Show();
            this.Close();
        }
    }
}
