using System.Collections.Generic;
using ElFartas.InstantEnglish.Interfaces;

namespace ElFartas.InstantEnglish.XmlDataAccess
{
    public class MyDictionary
    {
        public MyDictionary()
        {
            Categories = new List<Category>();
            Items = new List<Item>();
            Words = new List<Word>();
        }
        public List<Category> Categories;
        public List<Item> Items;
        public List<Word> Words;
        public List<User> Users;
        public List<Stat> Stats;
    }
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Difficulty { get; set; }

    }
    public class Item : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int CategoryId { get; set; }
    }
    public class Word : IWord
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
        public int ItemId { get; set; }
    }

    public class User : IUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Stat: IStat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int Correct { get; set; }
        public int Failed { get; set; }
    }
}
