using System;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Speech.Synthesis;
using System.Windows.Controls;
using ViewModel;

namespace UI
{
    /// <summary>
    /// Interaction logic for Questionary.xaml
    /// </summary>
    public partial class Questionary : Window
    {
        private QuestionSet qs;
        public Questionary(QuestionSet questionSet)
        {
            InitializeComponent();
            qs = questionSet;
            this.DataContext = questionSet;
        }

        private void StartSpeak(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(qs.Current.Question.Text)) return;
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Rate = -5;
            synth.Speak(qs.Current.Question.Text);
        }

        private void OnAnwser(object sender, RoutedEventArgs e)
        {
            bool isCorrect = ((Button) sender).DataContext == qs.Current.CorrectAnwser;
            MessageBox.Show(isCorrect ? "Correct" : "Failed");
            
            if (qs.Anwser(isCorrect)) return;
            MessageBox.Show("Finished\nYour result is " + qs.Result + "%");
            this.Close();
        }
    }
}
