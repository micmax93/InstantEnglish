using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ElFartas.InstantEnglish.DAO;
using ElFartas.InstantEnglish.Interfaces;

namespace ViewModel
{
    public class SimpleCommand : ICommand
    {
        public delegate void Exec();
        public delegate bool Check();

        public SimpleCommand(Exec execFunc)
        {
            ExecFunc = execFunc;

        }
        public Exec ExecFunc
        {
            get;
            private set;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ExecFunc.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }


    public class DictionaryViewModel
    {
        private IDataAccess dictionary;
 
        public ObservableCollection<ICategory> Categories
        {
            get;
            private set;
        }

        public ObservableCollection<IItem> Items
        {
            get;
            private set;
        }
        public ObservableCollection<IWord> Words
        {
            get;
            private set;
        }

        private ICategory _selectedCategory;
        private IItem _selectedItem;

        public ICategory SelectedCategory
        {
            set
            {
                
                _selectedCategory = value;
                _selectedItem = null;
                SelectedWord = null;
                Words.Clear();
                Items.Clear();
                if (value == null) return;
                foreach (var item in dictionary.GetItems(value))
                {
                    Items.Add(item);
                }
            }
            get
            {
                return _selectedCategory;
            }
        }
        public IItem SelectedItem
        {
            set
            {
                _selectedItem = value;
                SelectedWord = null;
                Words.Clear();
                if (value == null) return;
                foreach (var word in dictionary.GetWords(value))
                {
                    Words.Add(word);
                }
            }
            get
            {
                return _selectedItem;
            }
        }
        public IWord SelectedWord
        {
            get;
            set;
        }

        public DictionaryEntryViewModel EntryViewModel
        {
            get
            {
                var entry = new DictionaryEntryViewModel();
                entry.Category = SelectedCategory;
                entry.Item = SelectedItem;
                entry.Word = SelectedWord;
                return entry;
            }
        }

        public delegate void EntryCallback(DictionaryEntryViewModel viewModel);

        public event EntryCallback EditEntry;

        public void OpenEditEntry(EditTarget Target)
        {
            if (Target == EditTarget.Category && SelectedCategory == null) return;
            if (Target == EditTarget.Item && SelectedItem == null) return;
            if (Target == EditTarget.Word && SelectedWord == null) return;

            if (EditEntry != null)
            {
                var vm = new DictionaryEntryViewModel();
                vm.LanguageList = dictionary.GetLanguages();
                vm.Category = SelectedCategory;
                vm.Item = SelectedItem;
                vm.Word = SelectedWord;
                vm.Target = Target;
                EditEntry.Invoke(vm);
            }
        }
        public void OpenAddEntry(EditTarget Target)
        {
            if (EditEntry != null)
            {
                var vm = new DictionaryEntryViewModel();
                vm.LanguageList = dictionary.GetLanguages();
                vm.Category = (Target != EditTarget.Category) ? SelectedCategory : null;
                vm.Item = (Target != EditTarget.Item) ? SelectedItem : null;
                vm.Word = (Target != EditTarget.Word) ? SelectedWord : null;
                vm.Target = Target;
                EditEntry.Invoke(vm);
            }
        }
        public void DeleteEntry(EditTarget Target)
        {
            if (Target == EditTarget.Category && SelectedCategory != null)
            {
                dictionary.Delete(SelectedCategory);
            }
            else if (Target == EditTarget.Item && SelectedItem != null)
            {
                dictionary.Delete(SelectedItem);
            }
            else if (Target == EditTarget.Word && SelectedItem != null)
            {
                dictionary.Delete(SelectedWord);
            }
            else
            {
                return;
            }
            dictionary.Save();
            Categories.Clear();
            foreach (var category in dictionary.GetCategories())
            {
                Categories.Add(category);
            }
        }

        public void SaveEntry(DictionaryEntryViewModel entry)
        {
            switch (entry.Target)
            {
                case EditTarget.Category:
                    if (entry.Category != null)
                        dictionary.Update(entry.Category);
                    else
                        dictionary.AddCategory(entry.CategoryName, entry.CategoryLevel);
                    break;
                case EditTarget.Item:
                    if (entry.Item != null)
                        dictionary.Update(entry.Item);
                    else
                        dictionary.AddItem(entry.Category, entry.ItemName, entry.ImageBuffer);
                    break;
                case EditTarget.Word:
                    if (entry.Word != null)
                        dictionary.Update(entry.Word);
                    else
                        dictionary.AddWord(entry.Item, entry.WordText, entry.Language);
                    break;
            }
            dictionary.Save();
            Categories.Clear();
            foreach (var category in dictionary.GetCategories())
            {
                Categories.Add(category);
            }
        }


        public ICommand AddCategory { get; set; }
        public ICommand EditCategory { get; set; }
        public ICommand DeleteCategory { get; set; }
        public ICommand AddItem { get; set; }
        public ICommand EditItem { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand AddWord { get; set; }
        public ICommand EditWord { get; set; }
        public ICommand DeleteWord { get; set; }


        public DictionaryViewModel()
        {
            dictionary = DataProvider.GetDataAccess();
            
            Categories = new ObservableCollection<ICategory>(dictionary.GetCategories());
            Items = new ObservableCollection<IItem>();
            Words = new ObservableCollection<IWord>();

            AddCategory = new SimpleCommand(() => OpenAddEntry(EditTarget.Category));
            EditCategory = new SimpleCommand(() => OpenEditEntry(EditTarget.Category));
            DeleteCategory = new SimpleCommand(() => DeleteEntry(EditTarget.Category));

            AddItem = new SimpleCommand(() => OpenAddEntry(EditTarget.Item));
            EditItem = new SimpleCommand(() => OpenEditEntry(EditTarget.Item));
            DeleteItem = new SimpleCommand(() => DeleteEntry(EditTarget.Item));

            AddWord = new SimpleCommand(() => OpenAddEntry(EditTarget.Word));
            EditWord = new SimpleCommand(() => OpenEditEntry(EditTarget.Word));
            DeleteWord = new SimpleCommand(() => DeleteEntry(EditTarget.Word));
        }
    }
}
