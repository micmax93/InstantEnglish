using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElFartas.InstantEnglish.DAO;
using ElFartas.InstantEnglish.Interfaces;

namespace ViewModel
{
    public class SettingsViewModel
    {
        private IDataAccess dictionary;

        public List<string> ValueTypes { get; set; }
        private string _questionType;
        private string _anwserType;
        private int _level = 8;
        public int Views { get; set; }
        public int Rating { get; set; }
        public ObservableCollection<ICategory> AvalibleCategories { get; set; }

        public int UpdateCategories()
        {
            if (AnwserType != null && QuestionType != null)
            {
                var evl = EvaluateCategories().ToList();
                AvalibleCategories.Clear();
                foreach (var category in evl)
                {
                    AvalibleCategories.Add(category);
                }
                return evl.Count;
            }
            return 0;
        }
        public List<ICategory> SelectedCategories { get; set; }

        public string QuestionType
        {
            get { return _questionType; }
            set
            {
                _questionType = value;
                UpdateCategories();
            }
        }

        public string AnwserType
        {
            get { return _anwserType; }
            set
            {
                _anwserType = value;
                UpdateCategories();
            }
        }

        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                UpdateCategories();
            }
        }

        public SettingsViewModel()
        {
            dictionary = DataProvider.GetDataAccess();

            ValueTypes = new List<string>();
            ValueTypes.Add("Original");
            ValueTypes.Add("Image");
            ValueTypes.AddRange(dictionary.GetLanguages());

            AvalibleCategories = new ObservableCollection<ICategory>(dictionary.GetCategories());
        }

        
        private IQueryable<QuestionaryValue> _questions;
        private IQueryable<QuestionaryValue> _anwsers;

        public string ResultCount
        {
            get
            {
                if (_questions == null || _anwsers == null) return null;
                return _questions.Count() + " questions, " + _anwsers.Count() + " anwsers";
            }
        }

        public IQueryable<ICategory> EvaluateCategories()
        {
            var categories = dictionary.GetCategories();

            categories = categories.Where(c => c.Difficulty <= this.Level);
            
            _questions = GetQuestionaryValues(QuestionType);
            _anwsers = GetQuestionaryValues(AnwserType);
            _questions = _questions.Where(q => _anwsers.Any(a => a.ItemId == q.ItemId));

            var cItems = categories.SelectMany(c => dictionary.GetItems(c));
            var qItems = _questions.Select(q => q.ItemId);
            var items = cItems.Where(i => qItems.Contains(i.Id));
            
            categories = categories.Where(c => items.Any(i=>i.CategoryId==c.Id));
            _questions = _questions.Where(q => items.Any(i => i.Id == q.ItemId));

            //Console.WriteLine(ResultCount ?? "Null");

            return categories;
        }

        public IQueryable<QuestionaryValue> GetQuestionaryValues(string type)
        {
            if (type == "Original")
            {
                return dictionary.GetItems().Select(i => new QuestionaryValue(i,false));
            }
            else if(type == "Image")
            {
                return dictionary.GetItems().Where(i=>i.Image!=null).Select(i => new QuestionaryValue(i,true));
            }
            else
            {
                return dictionary.GetWords().Where(w=>w.Language==type).Select(w => new QuestionaryValue(w));
            }
        }

        public int NVL(QuestionaryValue qv)
        {
            return qv != null ? qv.ItemId : Int32.MaxValue;
        }


        public QuestionSet GenerateQuestionSet()
        {
            var r = new Random();
            List<QuestionaryViewModel> ql = new List<QuestionaryViewModel>();
            foreach (var question in _questions)
            {
                var qvm = new QuestionaryViewModel();
                qvm.Question = question;
                List<QuestionaryValue> qa = new List<QuestionaryValue>();
                qvm.CorrectAnwser = _anwsers.First(a => a.ItemId == question.ItemId);
                qa.Add(qvm.CorrectAnwser);
                var wrong = _anwsers.Where(a => !qa.Any(i => i.ItemId == a.ItemId));
                qa.Add(wrong.ElementAtOrDefault(r.Next(wrong.Count())));
                wrong = wrong.Where(a => !qa.Any(i => i.ItemId == a.ItemId));
                qa.Add(wrong.ElementAtOrDefault(r.Next(wrong.Count())));
                wrong = wrong.Where(a => !qa.Any(i => i.ItemId == a.ItemId));
                qa.Add(wrong.ElementAtOrDefault(r.Next(wrong.Count())));
                qa = qa.OrderBy(NVL).ToList();
                qvm.AnwserA = qa[0];
                qvm.AnwserB = qa[1];
                qvm.AnwserC = qa[2];
                qvm.AnwserD = qa[3];
                ql.Add(qvm);
            }
            return new QuestionSet(ql,Views,Rating);
        }
    }
}
