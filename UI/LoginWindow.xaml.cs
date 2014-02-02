using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ElFartas.InstantEnglish.Interfaces;
using ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindows : Window
    {
        public LoginViewModel LoginVM = new LoginViewModel();
        public LoginWindows()
        {
            InitializeComponent();
            DataContext = LoginVM;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var pass = ((PasswordBox) sender).Password;
            var data = Encoding.Unicode.GetBytes(pass);
            var hash = md5.ComputeHash(data);
            LoginVM.Password = Convert.ToBase64String(hash);
        }

        private void TryLogin(object sender, RoutedEventArgs e)
        {
            if (!LoginVM.AuthtenticateUser())
            {
                MessageBox.Show("Invalid username or password");
                return;
            }
            var menu = new MainMenu();
            menu.Owner = this;
            menu.Closed += (o, args) => this.Show();
            this.Hide();
            menu.ShowDialog();
        }

        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            if (LoginVM.RegsterNewUser())
            {
                TryLogin(sender, e);
            }
            else
            {
                MessageBox.Show("Cannot register user.");
            }
        }
    }
}
