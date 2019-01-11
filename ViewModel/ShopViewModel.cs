using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Shop.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ShopViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private DbClient db;
        public List<good_view> goods_base { get; set; }
        public ObservableCollection<good> order { get; set; }
        public class result: INotifyPropertyChanged
        {
            private int _prop;
            public int prop
            {
                get { return _prop; }
                set
                {
                    _prop = value;
                    OnPropertyChanged("prop");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName]string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
        public result res { get; set; }
        public RelayCommand<int> AddCommand     { get; set; }
        public RelayCommand<int> DeleteCommand  { get; set; }
        public RelayCommand CreateOrderCommand  { get; set; }

        private object AddingGood(int param)
        {
            good_view selected_item = goods_base.Find(x => x.id_good == param);
            good order_item = order.Where(x => x.id_good == selected_item.id_good).FirstOrDefault();
            if (order_item != null)
            {
                if (order_item.in_storage < selected_item.in_storage)
                {
                    good i = order_item;
                    i.in_storage++;
                    int index = order.IndexOf(order_item);
                    order.Remove(order_item);
                    order.Insert(index, i);
                }
            }
            else
            {
                good j = selected_item.ToGood();
                order.Add(j);
            }
            res.prop = 0;
            foreach(var item in order)
            {
                res.prop += item.price*item.in_storage;
            }
            return null;
        }
        private object DeletingGood(int param)
        {
            good order_item = order.Where(x => x.id_good == param).FirstOrDefault();
            if (order_item != null) order.Remove(order_item);
            res.prop = 0;
            foreach (var item in order)
            {
                res.prop += item.price * item.in_storage;
            }
            return null;
        }
        private object NewOrder()
        {
            string query = "SELECT max(id_ord) FROM orders";
            var orderid = db.Database.SqlQuery<int?>(query).First();

            query = "";

            var msg = new GoToPageMessage() { PageName = "CreateOrder", Parameter=orderid.ToString()};
            Messenger.Default.Send<GoToPageMessage>(msg);
            return null;
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ShopViewModel(IDataService dataService)
        {
            res = new result();
            res.prop = 0;
            AddCommand = new RelayCommand<int>((param) => AddingGood(param));
            DeleteCommand = new RelayCommand<int>((param) => DeletingGood(param));
            CreateOrderCommand = new RelayCommand(() => NewOrder());
            _dataService = dataService;
            _dataService.GetData(
                (user item, Exception error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    db = new DbClient();
                    //db.goods.Load();
                    order = new ObservableCollection<good>();
                    goods_base = new List<good_view>();
                    string query = "SELECT * FROM goods, goods_categories WHERE goods.cat_g=goods_categories.id_gc";
                    goods_base = db.Database.SqlQuery<good_view>(query).ToList<good_view>();
                    
                    //goods_base =db.goods.ToList<good>();
                    //foreach (var i in db.goods.ToList<good>()) { goods_base.Add(i.name_g); }
                    //WelcomeTitle2 = string.Join(" ", item.lst);
                });
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}