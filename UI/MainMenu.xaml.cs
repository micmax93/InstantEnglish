using System.Windows;
using ViewModel;

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
            QuestionSet qs = new QuestionSet();
            var qw = new Questionary(qs);
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

        private void OpenStats(object sender, RoutedEventArgs e)
        {
            new UserStats().ShowDialog();
        }
    }
}
