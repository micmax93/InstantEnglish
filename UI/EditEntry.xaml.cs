using System;
using System.Windows;
using System.Windows.Input;

namespace UI
{
    /// <summary>
    /// Interaction logic for DictionaryEntry.xaml
    /// </summary>
    public partial class EditEntry : Window
    {
        public EditEntry()
        {
            InitializeComponent();
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
    }
}
