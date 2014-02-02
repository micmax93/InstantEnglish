using System.Windows;
using ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserStats.xaml
    /// </summary>
    public partial class UserStats : Window
    {
        public UserStats()
        {
            InitializeComponent();
            DataContext = new UserStatsViewModel();
        }
    }
}
