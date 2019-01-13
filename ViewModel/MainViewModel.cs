using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Shop.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using System.Linq;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Messaging;

namespace Shop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="login" /> property's name.
        /// </summary>
        public const string LoginPropertyName = "login";
        /// <summary>
        /// The <see cref="password" /> property's name.
        /// </summary>
        public const string PassPropertyName = "password";

        private string _login = string.Empty;
        private string _password = string.Empty;

        /// <summary>
        /// Gets the login property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
            }
        }
        /// <summary>
        /// Gets the password property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        public RelayCommand EnterCommand
        {
            get;
            private set;
        }
        private object GoToPageShop()
        {
            DbClient db = new DbClient();
            db.users.Load();
            user us = new user();
            us = db.users
                 .Where(u => u.login == _login)
                 .FirstOrDefault<user>();
            if (us != null && us.password == _password)
            {
                var msg = new GoToPageMessage() { PageName = "ShopWindow", Parameter = us.id_user.ToString(), Parameter2 = us.user_cat.ToString() };
                Messenger.Default.Send<GoToPageMessage>(msg);
            }

            return null;
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            EnterCommand = new RelayCommand(() => GoToPageShop());
            _dataService = dataService;
            _dataService.GetData(
                (user item, Exception error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                });
            login = "test_admin";
            password = "123456";
        }
    }
}