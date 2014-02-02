using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ElFartas.InstantEnglish.DAO;
using ElFartas.InstantEnglish.Interfaces;
using Microsoft.Expression.Interactivity.Core;

namespace ViewModel
{
    
    public class LoginViewModel
    {
        private IDataAccess dataAccess;
        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public ICommand DoLogin
        {
            get;
            private set;
        }

        public ICommand DoRegister
        {
            get;
            private set;
        }

        bool FormFilled()
        {
            return !(String.IsNullOrEmpty(Username)||String.IsNullOrEmpty(Password));
        }

        public static IUser CurrentUser;
        public bool AuthtenticateUser()
        {
            CurrentUser = dataAccess.GetUser(Username);
            return CurrentUser != null && CurrentUser.Password == Password;
        }

        public bool RegsterNewUser()
        {
            if (dataAccess.GetUser(Username) != null)
            {
                return false;
            }
            dataAccess.AddUser(Username,Password);
            dataAccess.Save();
            return true;
        }

        public LoginViewModel()
        {
            dataAccess = DataProvider.GetDataAccess();
        }

    }
}
