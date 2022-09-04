using SafeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wave
{
    /// <summary>
    /// Interaction logic for restoreWindow.xaml
    /// </summary>
    public partial class restoreWindow : Window
    {
        public restoreWindow()
        {
            InitializeComponent();
        }

        private void minimizeRestore_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void exitRestore_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErrorResponse err = ClientFunctions.ForgotPassword(usernamerecText.Text, ProgramInformation.ProgramId);
            if (err.Failure)
            {
                MessageBox.Show(err.Message, SafeGuardTitle.safeguardtitle);
            }
            else
            {
                MessageBox.Show("Check Email For Password Reset Token", SafeGuardTitle.safeguardtitle);
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            this.Hide();
            m.Show();
            this.Close();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ErrorResponse err = ClientFunctions.ResetPassword(usernamerecText.Text, ProgramInformation.ProgramId, passwordresText.Password, tokenresText.Text);
            if (err.Failure)
            {
                MessageBox.Show(err.Message, SafeGuardTitle.safeguardtitle);
            }
            else
            {
                MessageBox.Show(err.Message, SafeGuardTitle.safeguardtitle);

                ResponseInformation.loginresponse = ClientFunctions.Login(usernamerecText.Text, passwordresText.Password, ProgramInformation.ProgramId);
                if (ResponseInformation.loginresponse.Failure)
                {
                    MessageBox.Show(ResponseInformation.loginresponse.Message, SafeGuardTitle.safeguardtitle);
                }
                else
                {
                    MessageBox.Show(ResponseInformation.loginresponse.Message);
                    ResponseInformation.Password = passwordresText.Password;
                    Panel p = new Panel();
                    this.Hide();
                    p.ShowDialog();
                    this.Close();
                }
            }
        }

        private void openLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            this.Hide();
            m.Show();
            this.Close();
        }
    }
}
