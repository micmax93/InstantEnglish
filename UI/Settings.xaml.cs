using System.Windows;
using ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        SettingsViewModel svm = new SettingsViewModel();
        public Settings()
        {
            InitializeComponent();
            DataContext = svm;
        }

        private void StartQuestionary(object sender, RoutedEventArgs e)
        {
            var count = svm.ResultCount;
            if (count == null)
            {
                MessageBox.Show("Incomplete form.");
                return;
            }
            var qs = svm.GenerateQuestionSet();
            if (qs.Questions.Count == 0)
            {
                MessageBox.Show("No questions avalible.");
                return;
            }
            var result = MessageBox.Show(count + "\nContinue?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                new Questionary(qs).ShowDialog();
            }
        }
    }
}
