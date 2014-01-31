using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void StartCourse(object sender, RoutedEventArgs e)
        {
            var qw = new Questionary();
            qw.Owner = this;
            qw.ShowDialog();
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            var st = new Settings();
            st.Owner = this;
            st.ShowDialog();
        }

        private void EditDict(object sender, RoutedEventArgs e)
        {
            var ed = new EditDictionary();
            ed.Owner = this;
            ed.ShowDialog();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
}
