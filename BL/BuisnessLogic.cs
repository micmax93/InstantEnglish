using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ElFartas.InstantEnglish.DAO;
using ElFartas.InstantEnglish.Interfaces;

namespace ElFartas.InstantEnglish.BL
{
    public class Value
    {
        public readonly string Text;
        public readonly Image Image;
        public bool IsImage
        {
            get { return Image != null; }
        }

        public Value(string text)
        {
            Text = text;
            Image = null;
        }
        public Value(string text, Image image)
        {
            Text = text;
            Image = image;
        }
        public Value(string text, byte[] image)
        {
            Text = text;
            MemoryStream stream = new MemoryStream(image);
            stream.Seek(0, SeekOrigin.Begin);
            Image = Image.FromStream(stream);
        }
    }
    public class Question
    {
        public Value QuestionValue;
        public List<Value> AnswerValues;
        public Value CorrectAnswerValue;
    }

    public enum QuestionType
    {
        Original,
        Translation
    }
    public enum TranslationType
    {
        Image,
        Language
    }

    public class LearningModel
    {
        public QuestionType QuestionType;
        public TranslationType TranslationType;
        public string Language;
        public IUser User;
        public ICategory Category = null;
        public int AnwsersCount;
    }

    public class SelectionMethod
    {
        public int Visits;
        public int Ratings;

        Random rand = new Random();
        private IQueryable<IStat> stats;
        private List<int> mostVisited, leastVisited, highestRated, lowesRated;
        public void Load(IQueryable<IStat> stats)
        {
            this.stats = stats;
            double avgVisits = stats.Average(s => s.Correct + s.Failed);
            double avgRatings = stats.Average(s => s.Correct/(s.Correct + s.Failed + 1));

            mostVisited = stats.Where(s => (s.Correct + s.Failed) > avgVisits).Select(s => s.ItemId).ToList();
            leastVisited = stats.Where(s => (s.Correct + s.Failed) <= avgVisits).Select(s => s.ItemId).ToList();

            highestRated = stats.Where(s => (s.Correct / (s.Correct + s.Failed + 1)) > avgRatings).Select(s => s.ItemId).ToList();
            lowesRated = stats.Where(s => (s.Correct / (s.Correct + s.Failed + 1)) > avgRatings).Select(s => s.ItemId).ToList();
        }

        private void RemoveId(int id)
        {
            if (mostVisited.Contains(id)) mostVisited.Remove(id);
            if (leastVisited.Contains(id)) leastVisited.Remove(id);
            if (highestRated.Contains(id)) highestRated.Remove(id);
            if (lowesRated.Contains(id)) lowesRated.Remove(id);
            
        }
        public int GetItemId()
        {
            IEnumerable<int> A, B;

            if (mostVisited.Count > 0 && leastVisited.Count > 0)
                A = Visits > rand.Next(0, 100) ? mostVisited : leastVisited;
            else
                A = mostVisited.Union(leastVisited);

            if (highestRated.Count > 0 && lowesRated.Count > 0)
                B = Ratings > rand.Next(0, 100) ? highestRated : lowesRated;
            else
                B = highestRated.Union(lowesRated);

            int r = A.Intersect(B).FirstOrDefault();
            RemoveId(r);
            return r;
        }
    }
}
