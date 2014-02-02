using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ElFartas.InstantEnglish.DAO;
using ElFartas.InstantEnglish.Interfaces;

namespace ViewModel
{
    public class QuestionaryValue : IItem
    {
        public int ItemId;
        public string Text { get; set; }
        public ImageSource ImageSource { get; set; }

        public QuestionaryValue()
        {
        }

        public QuestionaryValue(IItem item, bool getImage = false)
        {
            ItemId = item.Id;
            if (!getImage)
            {
                Text = item.Name;
            }
            else
            {
                if (item.Image == null) return;
                MemoryStream ms = new MemoryStream(item.Image);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                ImageSource = bi;
            }
        }

        public QuestionaryValue(IWord word)
        {
            ItemId = word.ItemId;
            Text = word.Text;
        }

        public int Id
        {
            get { return ItemId; }
            set { }
        }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int CategoryId { get; set; }
    }
    public class QuestionaryViewModel
    {
        public QuestionaryValue Question { get; set; }
        public QuestionaryValue CorrectAnwser { get; set; }
        public QuestionaryValue AnwserA { get; set; }
        public QuestionaryValue AnwserB { get; set; }
        public QuestionaryValue AnwserC { get; set; }
        public QuestionaryValue AnwserD { get; set; }
    }

    public class QuestionSet : INotifyPropertyChanged
    {
        public Stack<QuestionaryViewModel> Questions = new Stack<QuestionaryViewModel>();
        private IDataAccess dictionary;
        public QuestionaryViewModel Current
        {
            get
            {
                if (Questions.Count == 0) return null;
                return Questions.Peek();
            }
        }

        private int _correct = 0, _failed = 0;

        public QuestionSet() { }
        public QuestionSet(IEnumerable<QuestionaryViewModel> questions, int views, int ratings)
        {
            var sorted = new List<KeyValuePair<double, QuestionaryViewModel>>();
            dictionary = DataProvider.GetDataAccess();

            var global = dictionary.GetStats(LoginViewModel.CurrentUser);
            if (global == null || !global.Any())
            {
                Questions = new Stack<QuestionaryViewModel>(questions);
                return;
            }

            double maxViews = global.Max(s => s.Correct + s.Failed);
            double maxRating = global.Max(s => s.Correct/(s.Correct + s.Failed + 1));
            if (maxViews == 0) maxViews = 1;
            if (maxRating == 0) maxRating = 1;
            double targetViews = (views/100)*maxViews;
            double targetRatings = (views/100)*maxRating;

            foreach (var q in questions)
            {
                double v, r;
                var stat = dictionary.GetStat(LoginViewModel.CurrentUser, q.Question);
                if (stat == null)
                {
                    v = 0;
                    r = 0;
                }
                else
                {
                    v = stat.Correct + stat.Failed;
                    r = stat.Correct / (stat.Correct + stat.Failed + 1);
                }
                double eval = (v - targetViews)/maxViews + (r - targetRatings)/maxRating;
                sorted.Add(new KeyValuePair<double, QuestionaryViewModel>(eval, q));
                //Console.WriteLine(q.Question.Text + " " + eval);
            }
            Questions = new Stack<QuestionaryViewModel>(sorted.OrderBy(e => e.Key).Select(e => e.Value));
        }

        public int Result
        {
            get
            {
                int sum = _correct + _failed;
                Console.WriteLine(sum);
                if (sum == 0) return 0;
                return 100 * _correct / sum;
            }
        }


        public bool Anwser(bool isCorrect)
        {
            var stat = dictionary.GetStat(LoginViewModel.CurrentUser, Current.Question);
            if (stat == null)
            {
                dictionary.AddStat(LoginViewModel.CurrentUser,Current.Question);
                stat = dictionary.GetStat(LoginViewModel.CurrentUser, Current.Question);
            }

            if (isCorrect)
            {
                _correct++;
                stat.Correct++;
            }
            else
            {
                _failed++;
                stat.Failed++;
            }
            dictionary.Save();
            return Next();
        }

        public bool Next()
        {
            if (Questions.Count == 0) return false;
            Questions.Pop();
            if (Questions.Count == 0) return false;
            OnPropertyChanged("Current");
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
