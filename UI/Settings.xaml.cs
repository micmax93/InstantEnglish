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
            var result = MessageBox.Show(svm.ResultCount + "\nContinue?","", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                new Questionary(svm.GenerateQuestionSet()).ShowDialog();
            }
        }
    }
}
