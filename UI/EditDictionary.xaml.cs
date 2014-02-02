using System.Windows;
using ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for EditDictionary.xaml
    /// </summary>
    public partial class EditDictionary : Window
    {
        private DictionaryViewModel vm;
        public EditDictionary()
        {
            InitializeComponent();
            vm = new DictionaryViewModel();
            this.DataContext = vm;
            vm.EditEntry +=OnEditEntry;
        }

        private void OnEditEntry(DictionaryEntryViewModel viewModel)
        {
            viewModel.OnEntrySave += entry => vm.SaveEntry(entry);
            new EditEntry(viewModel).ShowDialog();
        }
    }
}
