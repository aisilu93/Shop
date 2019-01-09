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
            private set;
        }
        public string login
        {
            get;//{ return login; }
            private set;
            /*{
                login = value;
                OnPropertyChanged("login");
            }*/
        }
        public string password
        {
            get;// { return password; }
            private set;
            /*{
                password = value;
                OnPropertyChanged("password");
            }*/
        }
        public int user_cat
        {
            get;// { return user_cat; }
            private set;
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
}
