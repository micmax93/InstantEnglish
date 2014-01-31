using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Speech.Synthesis;

namespace UI
{
    /// <summary>
    /// Interaction logic for Questionary.xaml
    /// </summary>
    public partial class Questionary : Window
    {
        public Questionary()
        {
            InitializeComponent();
        }

        private void StartSpeak(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Rate = -5;
            synth.Speak("Microsoft");
        }
    }
}
