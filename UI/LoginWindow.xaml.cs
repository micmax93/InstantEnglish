using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
            var menu = new MainMenu();
            menu.Owner = this;
            menu.Closed += (o, args) => this.Show();
            this.Hide();
            menu.ShowDialog();
        }
    }
}
