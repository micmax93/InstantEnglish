using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using ElFartas.InstantEnglish.Interfaces;

namespace ElFartas.InstantEnglish.XmlDataAccess
{
    public class DataAccess: IDataAccess
    {
        private MyDictionary db;
        public DataAccess()
        {
            try
            {
                var path = ConfigurationManager.AppSettings["XmlDbPath"];
                var textReader = new XmlTextReader(path);
                try
                {
                    var serializer = new XmlSerializer(typeof (MyDictionary));
                    db = (MyDictionary) serializer.Deserialize(textReader);
                }
                catch
                {
                    db = new MyDictionary();
                }
                textReader.Close();
            }
            catch (Exception e)
            {
                db = new MyDictionary();
            }
        }

        // ---------------------------------------- GET WORD -------------------------------------------
        public IQueryable<IWord> GetWords()
        {
            return db.Words.AsQueryable();
        }
        public IQueryable<IWord> GetWords(ICategory category)
        {
            var items = db.Items.Where(i => i.CategoryId == category.Id);
            return db.Words.Where(w => items.Any(i => i.Id == w.ItemId)).AsQueryable();
        }
        public IQueryable<IWord> GetWords(IItem item)
        {
            return db.Words.Where(w => w.ItemId == item.Id).AsQueryable();
        }
        public IQueryable<IWord> GetWords(IItem item, string language)
        {
            return db.Words.Where(w => w.ItemId == item.Id).Where(w => w.Language == language).AsQueryable();
        }

        public IQueryable<string> GetLanguages()
        {
            return db.Words.Select(w => w.Language).Distinct().AsQueryable();
        }

        // ---------------------------------------- GET ITEM -------------------------------------------
        public IQueryable<IItem> GetItems()
        {
            return db.Items.AsQueryable();
        }
        public IQueryable<IItem> GetItems(ICategory category)
        {
            return db.Items.Where(i => i.CategoryId == category.Id).AsQueryable();
        }
        public IItem GetItem(IWord word)
        {
            return db.Items.Find(i => i.Id == word.ItemId);
        }

        // ---------------------------------------- GET ITEM -------------------------------------------
        public IQueryable<ICategory> GetCategories()
        {
            return db.Categories.AsQueryable();
        }
        public ICategory GetCategory(IItem item)
        {
            return db.Categories.Find(c => c.Id == item.Id);
        }
        public ICategory GetCategory(IWord word)
        {
            return GetCategory(GetItem(word));
        }


        // ---------------------------------------- GET USER/STATS -------------------------------------------
        public IUser GetUser(string username)
        {
            return db.Users.FirstOrDefault(u => u.Username == username);
        }

        public IUser GetUser(int id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<IStat> GetStats(IUser user)
        {
            return db.Stats.Where(s => s.UserId == user.Id).AsQueryable();
        }

        public IStat GetStat(IUser user, IItem item)
        {
            return db.Stats.FirstOrDefault(s => s.UserId == user.Id && s.ItemId==item.Id);
        }


        // ---------------------------------------- UPDATE -------------------------------------------
        public void Update(IWord word)
        {
            db.Words.Find(o => o.Id == word.Id).Text = word.Text;
        }

        public void Update(IItem item)
        {
            db.Items.Find(o => o.Id == item.Id).Image = item.Image;
        }

        public void Update(ICategory category)
        {
            db.Categories.Find(o => o.Id == category.Id).Difficulty = category.Difficulty;
        }

        public void Update(IStat stat)
        {
            var s = db.Stats.Find(_s => _s.Id == stat.Id);
            s.Correct = stat.Correct;
            s.Failed = stat.Failed;
        }

        public void Update(IUser user)
        {
            db.Users.Find(u => u.Id == user.Id).Password = user.Password;
        }


        // ---------------------------------------- DELETE -------------------------------------------
        public void Delete(IWord word)
        {
            db.Words.Remove(db.Words.Find(o => o.Id == word.Id));
        }

        public void Delete(IItem item)
        {
            db.Items.Remove(db.Items.Find(o => o.Id == item.Id));
        }

        public void Delete(ICategory category)
        {
            db.Categories.Remove(db.Categories.Find(o => o.Id == category.Id));
        }

        public void Delete(IStat stat)
        {
            db.Stats.Remove(db.Stats.Find(s => s.Id == stat.Id));
        }

        public void Delete(IUser user)
        {
            db.Users.Remove(db.Users.Find(u => u.Id == user.Id));
        }

        // ------------------------------------- CREATE --------------------------------------------
        public void AddWord(IItem item, string text, string language)
        {
            var word = new Word();
            word.Id = db.Words.Count > 0 ? db.Words.Max(w => w.Id + 1) : 1;
            word.ItemId = item.Id;
            word.Text = text;
            word.Language = language;
            db.Words.Add(word);
        }

        public void AddItem(ICategory category, string name, byte[] image = null)
        {
            var item = new Item();
            item.Id = db.Items.Count > 0 ? db.Items.Max(i => i.Id + 1) : 1;
            item.CategoryId = category.Id;
            item.Name = name;
            item.Image = image;
            db.Items.Add(item);
        }

        public void AddCategory(string name, int difficulty)
        {
            var category = new Category();
            category.Id = db.Categories.Count > 0 ? db.Categories.Max(c => c.Id + 1) : 1;
            category.Name = name;
            category.Difficulty = difficulty;
            db.Categories.Add(category);
        }
        public void AddUser(string username, string passHash)
        {
            var user = new User();
            user.Username = username;
            user.Password = passHash;
            db.Users.Add(user);
        }

        public void AddStat(IUser user, IItem item)
        {
            var stat = new Stat();
            stat.UserId = user.Id;
            stat.ItemId = item.Id;
            db.Stats.Add(stat);
        }


        // ---------------------------------------- SAVE -------------------------------------------
        public void Save()
        {
            var path = ConfigurationManager.AppSettings["XmlDbPath"];
            TextWriter textWriter = new StreamWriter(path, false);
            
            var serializer = new XmlSerializer(typeof(MyDictionary));
            serializer.Serialize(textWriter, db);

            textWriter.Close();
        }
    }
}
