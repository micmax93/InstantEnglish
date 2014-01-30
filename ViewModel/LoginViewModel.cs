using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;

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
    public class LoginViewModel
    {
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

        void AuthtenticateUser()
        {
            if (Password == "ha#lo")
            {
                MessageBox.Show("Zalogowano");
            }
            else
            {
                MessageBox.Show(Password);
            }
        }

        void RegsterNewUser()
        {
            
        }

        public LoginViewModel()
        {
            DoLogin = new SimpleCommand(AuthtenticateUser);
            DoRegister = new SimpleCommand(RegsterNewUser);
        }

    }
}
