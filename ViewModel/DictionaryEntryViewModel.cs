using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ElFartas.InstantEnglish.Interfaces;
using System.Drawing;

namespace ViewModel
{
    public enum EditTarget { Category, Item, Word };
    public class DictionaryEntryViewModel
    {

        public EditTarget Target;

        public event DictionaryViewModel.EntryCallback OnEntrySave;
        public ICommand Save { get; set; }
        public DictionaryEntryViewModel()
        {
            Target = EditTarget.Word;
            Save = new SimpleCommand(() => OnEntrySave.Invoke(this));
        }

        public bool CategoryEnabled
        {
            get
            {
                return (Target == EditTarget.Category);
            }
        }
        public bool ItemEnabled
        {
            get
            {
                return (Target == EditTarget.Item);
            }
        }
        public bool WordEnabled
        {
            get
            {
                return (Target == EditTarget.Word);
            }
        }

        public string CategoryName
        {
            get;
            set;
        }

        public int CategoryLevel
        {
            get;
            set;
        }
        public string CategoryLevelStr
        {
            get { return CategoryLevel.ToString(); }
            set
            {
                int num;
                if (Int32.TryParse(value, out num))
                {
                    CategoryLevel = num;
                }
            }
        }
        public string ItemName
        {
            get;
            set;
        }

        public byte[] ImageBuffer
        {
            get;
            set;
        }

        public Image Image
        {
            get
            {
                if (ImageBuffer != null)
                {
                    return Image.FromStream(new MemoryStream(ImageBuffer));
                }
                return null;
            }
            set
            {
                var buffer = new MemoryStream();
                value.Save(buffer, ImageFormat.Jpeg);
                ImageBuffer = buffer.ToArray();
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                if (ImageBuffer == null) return null;
                MemoryStream ms = new MemoryStream(ImageBuffer);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                return bi;
            }
        }

        public string WordText
        {
            get;
            set;
        }

        public IEnumerable<string> LanguageList
        {
            get;
            set;
        }

        private string _language;
        public string Language
        {
            get
            {
                if (_language != null)
                {
                    return _language;
                }
                if (!string.IsNullOrEmpty(NewLanguage))
                {
                    return NewLanguage;
                }
                return null;
            }
            set { _language = value; }
        }

        public string NewLanguage
        {
            set;
            get;
        }

        private ICategory _category = null;
        private IItem _item = null;
        private IWord _word = null;

        public ICategory Category
        {
            get
            {
                if (_category != null)
                {
                    _category.Name = CategoryName;
                    _category.Difficulty = CategoryLevel;
                }
                return _category;
            }
            set
            {
                _category = value;
                CategoryName = (value != null) ? value.Name : "";
                CategoryLevel = (value != null) ? value.Difficulty : 0;
            }
        }

        public IItem Item
        {
            get
            {
                if (_item!=null)
                {
                    _item.Name = ItemName;
                    _item.Image = ImageBuffer;
                }
                return _item;
            }
            set
            {
                _item = value;
                ItemName = (value != null) ? value.Name : "";
                ImageBuffer = (value != null) ? value.Image : null;
                ImageSourceConverter c = new ImageSourceConverter();
            }
        }

        public IWord Word
        {
            get
            {
                if (_word != null)
                {
                    _word.Text = WordText;
                    _word.Language = Language;
                }
                return _word;
            }
            set
            {
                _word = value;
                WordText = (value != null) ? value.Text : "";
                Language = (value != null) ? value.Language : null;
            }
        }
    }
}
