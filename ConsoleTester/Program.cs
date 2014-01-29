using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ElFartas.InstantEnglish.DAO;
using ElFartas.InstantEnglish.Interfaces;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //MD5 md5 = new MD5CryptoServiceProvider();
            //while (true)
            //{
            //    string pass = Console.ReadLine();
            //    var data = Encoding.Unicode.GetBytes(pass);
            //    var hash = md5.ComputeHash(data);
            //    Console.WriteLine(Convert.ToBase64String(hash));
            //}
            //return;
            IDataAccess da = DataProvider.GetDataAccess();
            da.AddCategory("Color", 2);
            da.AddItem(da.GetCategories().First(), "Blue", null);
            da.AddWord(da.GetItems().First(), "Niebieski", "Polski");
            
            foreach (var word in da.GetWords())
            {
                Console.WriteLine(word.Language + ": " + word.Text);
            }

            //Image img = Image.FromFile(@"C:\Users\micmax93\Pictures\avatar.jpg");
            //var buffer = new MemoryStream();
            //img.Save(buffer,ImageFormat.Jpeg);
            //da.GetItems().First().Image = buffer.ToArray();

            //da.Save();
            Console.ReadKey();
        }
    }
}
