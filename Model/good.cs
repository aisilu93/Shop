using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("goods")]
    public class good : INotifyPropertyChanged
    {
        [Key]
        public int id_good
        {
            get;
            private set;
        }
        public string name_g
        {
            get;// { return name_g; }
            private set;
            /*{
                name_g = value;
                OnPropertyChanged("name_g");
            }*/
        }
        public int cat_g
        {
            get;// { return cat_g; }
            private set;
            /*{
                cat_g = value;
                OnPropertyChanged("cat_g");
            }*/
        }
        public int price
        {
            get;// { return price; }
            private set;
            /*{
                price = value;
                OnPropertyChanged("price");
            }*/
        }
        public int in_storage
        {
            get;// { return in_storage; }
            private set;
            /*{
                in_storage = value;
                OnPropertyChanged("in_storage");
            }*/
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}