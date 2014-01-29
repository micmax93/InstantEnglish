using System.Linq;

namespace ElFartas.InstantEnglish.Interfaces
{
    public interface IDataAccess
    {
        IQueryable<IWord> GetWords();
        IQueryable<IWord> GetWords(ICategory category);
        IQueryable<IWord> GetWords(IItem item);
        IQueryable<IWord> GetWords(IItem item, string language);
        IQueryable<string> GetLanguages();
        IQueryable<IItem> GetItems();
        IQueryable<IItem> GetItems(ICategory category);
        IItem GetItem(IWord word);
        IQueryable<ICategory> GetCategories();
        ICategory GetCategory(IItem item);
        ICategory GetCategory(IWord word);
        IUser GetUser(int id);
        IUser GetUser(string username);
        IQueryable<IStat> GetStats(IUser user);
        IStat GetStat(IUser user,IItem item); 
        void Update(IWord word);
        void Update(IItem item);
        void Update(ICategory category);
        void Update(IStat stat);
        void Update(IUser user);
        void Delete(IWord word);
        void Delete(IItem item);
        void Delete(ICategory category);
        void Delete(IStat stat);
        void Delete(IUser user);
        void AddCategory(string name, int difficulty);
        void AddWord(IItem item, string text, string language);
        void AddItem(ICategory category, string name, byte[] image = null);
        void AddUser(string username, string passHash);
        void AddStat(IUser user, IItem item);
        void Save();
    }
}