using System.Linq;
using ElFartas.InstantEnglish.Interfaces;

namespace ElFartas.InstantEnglish.SqlDataAccess
{
    public class DataAccess : IDataAccess
    {
        private DictionaryEntities db;

        public DataAccess()
        {
            db = new DictionaryEntities();
        }

        // ---------------------------------------- GET WORD -------------------------------------------
        public IQueryable<IWord> GetWords()
        {
            return db.Words;
        }
        public IQueryable<IWord> GetWords(ICategory category)
        {
            return db.Categories.Find(category.Id).Items.SelectMany(i => i.Words).AsQueryable();
        }
        public IQueryable<IWord> GetWords(IItem item)
        {
            return db.Items.Find(item.Id).Words.AsQueryable();
        }
        public IQueryable<IWord> GetWords(IItem item,string language)
        {
            return db.Items.Find(item.Id).Words.Where(w => w.Language == language).AsQueryable();
        }

        public IQueryable<string> GetLanguages()
        {
            return db.Words.Select(w => w.Language).Distinct();
        }

        // ---------------------------------------- GET ITEM -------------------------------------------
        public IQueryable<IItem> GetItems()
        {
            return db.Items;
        }
        public IQueryable<IItem> GetItems(ICategory category)
        {
            return db.Categories.Find(category.Id).Items.AsQueryable();
        }
        public IItem GetItem(IWord word)
        {
            return db.Words.Find(word.Id).Item;
        }

        // ---------------------------------------- GET CATEGORY -------------------------------------------
        public IQueryable<ICategory> GetCategories()
        {
            return db.Categories;
        }
        public ICategory GetCategory(IItem item)
        {
            return db.Items.Find(item.Id).Category;
        }
        public ICategory GetCategory(IWord word)
        {
            return db.Words.Find(word.Id).Item.Category;
        }

        // ---------------------------------------- GET USER/STATS -------------------------------------------
        public IUser GetUser(string username)
        {
            return db.Users.FirstOrDefault(u => u.Username == username);
        }

        public IUser GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public IQueryable<IStat> GetStats(IUser user)
        {
            return db.Users.Find(user.Id).Stats.AsQueryable();
        }

        public IStat GetStat(IUser user, IItem item)
        {
            return db.Users.Find(user.Id).Stats.FirstOrDefault(s => s.ItemId == item.Id);
        }


        // ---------------------------------------- UPDATE -------------------------------------------
        public void Update(IWord word)
        {
            db.Words.Find(word.Id).Text = word.Text;
        }

        public void Update(IItem item)
        {
            db.Items.Find(item.Id).Image = item.Image;
        }

        public void Update(ICategory category)
        {
            db.Categories.Find(category.Id).Difficulty = category.Difficulty;
        }

        public void Update(IStat stat)
        {
            var s = db.Stats.Find(stat.Id);
            s.Correct = stat.Correct;
            s.Failed = stat.Failed;
        }

        public void Update(IUser user)
        {
            db.Users.Find(user.Id).Password = user.Password;
        }


        // ---------------------------------------- DELETE -------------------------------------------
        public void Delete(IWord word)
        {
            db.Words.Remove(db.Words.Find(word.Id));
        }

        public void Delete(IItem item)
        {
            db.Items.Remove(db.Items.Find(item.Id));
        }

        public void Delete(ICategory category)
        {
            db.Categories.Remove(db.Categories.Find(category.Id));
        }

        public void Delete(IStat stat)
        {
            db.Stats.Remove(db.Stats.Find(stat.Id));
        }

        public void Delete(IUser user)
        {
            db.Users.Remove(db.Users.Find(user.Id));
        }

        // ------------------------------------- CREATE --------------------------------------------
        public void AddWord(IItem item, string text,string language)
        {
            var word = db.Words.Create();
            word.ItemId = item.Id;
            word.Text = text;
            word.Language = language;
            db.Words.Add(word);
        }

        public void AddItem(ICategory category,string name, byte[] image = null)
        {
            var item = db.Items.Create();
            item.CategoryId = category.Id;
            item.Name = name;
            item.Image = image;
            db.Items.Add(item);
        }

        public void AddCategory(string name, int difficulty)
        {
            var category = db.Categories.Create();
            category.Name = name;
            category.Difficulty = difficulty;
            db.Categories.Add(category);
        }

        public void AddUser(string username, string passHash)
        {
            var user = db.Users.Create();
            user.Username = username;
            user.Password = passHash;
            db.Users.Add(user);
        }

        public void AddStat(IUser user, IItem item)
        {
            var stat = db.Stats.Create();
            stat.UserId = user.Id;
            stat.ItemId = item.Id;
            db.Stats.Add(stat);
        }


        // ---------------------------------------- SAVE -------------------------------------------
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
