using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("users")]
    public class user: INotifyPropertyChanged
    {
        [Key]
        public int id_user
        {
            get;
            set;
        }
        public string login
        {
            get;//{ return login; }
            set;
            /*{
                login = value;
                OnPropertyChanged("login");
            }*/
        }
        public string password
        {
            get;// { return password; }
            set;
            /*{
                password = value;
                OnPropertyChanged("password");
            }*/
        }
        public int user_cat
        {
            get;// { return user_cat; }
            set;
            /*{
                /*user_cat = value;
                OnPropertyChanged("user_cat");
            }*/
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    [Table("user_categories")]
    public class user_categories
    {
        [Key]
        public int id_uc { get; set; }
        public string name_uc { get; set; }
    }

    public class user_view: INotifyPropertyChanged
    {
        private int _id_user;
        private string _login;
        private string _password;
        private int _user_cat;
        private string _name_uc;

        public int id_user
        {
            get { return _id_user; }
            set
            {
                _id_user = value;
                OnPropertyChanged("id_user");
            }
        }
        public string login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("login");
            }
        }
        public string password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("password");
            }
        }
        public int user_cat
        {
            get { return _user_cat; }
            set
            {
                _user_cat = value;
                OnPropertyChanged("user_cat");
            }
        }
        public string name_uc
        {
            get { return _name_uc; }
            set
            {
                _name_uc = value;
                OnPropertyChanged("name_uc");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
