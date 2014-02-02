using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for DictionaryEntry.xaml
    /// </summary>
    public partial class EditEntry : Window
    {
        private DictionaryEntryViewModel _entry;
        //public EditEntry()
        //{
        //    InitializeComponent();
        //    _entry = new DictionaryEntryViewModel();
        //    this.DataContext = _entry;
        //}
        public EditEntry(DictionaryEntryViewModel vm)
        {
            InitializeComponent();
            _entry = vm ?? new DictionaryEntryViewModel();
            this.DataContext = _entry;
        }

        private void LevelInput(object sender, TextCompositionEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Text))
            {
                int num;
                if (!Int32.TryParse(e.Text, out num))
                {
                    e.Handled = true;
                }
            }
        }

        private void LoadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog(this) == true)
            {
                Image img = Image.FromFile(dialog.FileName);
                _entry.Image = img;
                ImagePreview.Source = _entry.ImageSource;
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(_entry.Language);
            Close();
        }
    }
}
