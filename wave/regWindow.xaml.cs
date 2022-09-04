using System.Threading.Tasks;
using System.Windows;
using SafeGuard;

namespace wave
{
    /// <summary>
    /// Interaction logic for regWindow.xaml
    /// </summary>
    public partial class regWindow : Window
    {
        public regWindow()
        {
            InitializeComponent();
            Tools.ProcessCheck();
            SafeCheck.Md5Check();
        }

        private bool RegInfo(string username, string password, string token, string email, string programId)
        {
            Tools.ProcessCheck();
            ResponseInformation.registerinfo = ClientFunctions.Register(username, password, token, email, programId);
            if (ResponseInformation.registerinfo.Failure)
            {
                return false;
            }
            else
            {
                return true;
            }
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
        //register btn
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            regBtn.IsEnabled = false;
           string username = usernameText.Text;
            string password = passwordText.Password;
           string token = tokenText.Text;
           string email = emailText.Text;
           string programId = ProgramInformation.ProgramId;
            Task<bool> task = new Task<bool>(() => RegInfo(username, password, token, email, programId));
            task.Start();
            bool register = await task;
            if (register)
            {
                ResponseInformation.loginresponse = ClientFunctions.Login(usernameText.Text, passwordText.Password, ProgramInformation.ProgramId);
                if (ResponseInformation.loginresponse.Failure)
                {
                    MessageBox.Show(ResponseInformation.loginresponse.Message, SafeGuardTitle.safeguardtitle);
                }
                else
                {
                    MessageBox.Show(ResponseInformation.loginresponse.Message);
                    ResponseInformation.Password = passwordText.Password;
                    Panel p = new Panel();
                    this.Hide();
                    p.ShowDialog();
                    this.Close();
                }
            }
            else if (!register)
            {
                MessageBox.Show(ResponseInformation.registerinfo.Message, SafeGuardTitle.safeguardtitle);
                regBtn.IsEnabled = true;
            }
        }

        private void openLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            this.Hide();
            m.Show();
            this.Close();
        }

        private void minimizeReg_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void exitReg_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
