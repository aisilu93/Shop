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
        static public int userid = 0;
        private readonly IDataService _dataService;
        private DbClient db;
        public ObservableCollection<good_view> goods_base { get; set; }
        public ObservableCollection<good> order { get; set; }
        public ObservableCollection<order> orders_base { get; set; }
        public ObservableCollection<order_item> order_items { get; set; }

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
        public RelayCommand<int> AddCommand             { get; set; }
        public RelayCommand<int> DeleteCommand          { get; set; }
        public RelayCommand CreateOrderCommand          { get; set; }
        public RelayCommand<string> GetOrderItemsCommand   { get; set; }

        private object AddingGood(int param)
        {
            good_view selected_item = goods_base.First(x => x.id_good == param);
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
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int timestamp = (int)t.TotalSeconds;
            string query = "INSERT INTO orders(`id_user`,`timestamp`) VALUES ("+userid+","+ timestamp+")";
            db.Database.ExecuteSqlCommand(query);
            db.SaveChanges();

            query = "SELECT max(id_ord) FROM orders";
            var orderid = db.Database.SqlQuery<int?>(query).First();

            foreach(var item in order)
            {
                query = "INSERT INTO order_items(`id_order`,`id_good`,`amount`,`price`) VALUES("+orderid+","+ item.id_good+ "," + item.in_storage + "," + item.price + ")";
                db.Database.ExecuteSqlCommand(query);
                query = "UPDATE goods SET `in_storage`=`in_storage`-"+item.in_storage+" WHERE `id_good`="+item.id_good;
                db.Database.ExecuteSqlCommand(query);
                db.SaveChanges();
            }
            order.Clear();
            goods_base.Clear();
            LoadGoodsBase();
            res.prop = 0;
            var msg = new GoToPageMessage() { PageName = "CreateOrder", Parameter=orderid.ToString()};
            Messenger.Default.Send<GoToPageMessage>(msg);
            return null;
        }

        private void LoadGoodsBase()
        {
            if (goods_base==null) goods_base = new ObservableCollection<good_view>();
            else goods_base.Clear();
            string query = "SELECT * FROM goods, goods_categories WHERE goods.cat_g=goods_categories.id_gc";
            List<good_view> temp = db.Database.SqlQuery<good_view>(query).ToList<good_view>();
            foreach(var item in temp)
            {
                goods_base.Add(item);
            }
            if (orders_base == null) orders_base = new ObservableCollection<order>();
            else orders_base.Clear();
            query = "SELECT orders.*,users.login FROM orders, users WHERE orders.id_user=users.id_user";
            List<order> temp_orders = db.Database.SqlQuery<order>(query).ToList<order>();
            foreach (var item in temp_orders)
            {
                orders_base.Add(item);
            }
        }

        private void GetOrderItems(string param)
        {
            order_items.Clear();
            string query = "SELECT order_items.*, goods.name_g FROM order_items,goods WHERE order_items.id_order="+param+" AND order_items.id_good=goods.id_good";
            List<order_item> temp_items = db.Database.SqlQuery<order_item>(query).ToList<order_item>();
            foreach (var item in temp_items)
            {
                order_items.Add(item);
            }
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
            GetOrderItemsCommand = new RelayCommand<string>((param) => GetOrderItems(param));
            order_items = new ObservableCollection<order_item>();
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
                    LoadGoodsBase();
                    /*goods_base = new List<good_view>();
                    string query = "SELECT * FROM goods, goods_categories WHERE goods.cat_g=goods_categories.id_gc";
                    goods_base = db.Database.SqlQuery<good_view>(query).ToList<good_view>();*/
                    
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