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
        static public int userright = 0;
        private readonly IDataService _dataService;
        private DbClient db;
        public ObservableCollection<good_view> goods_base { get; set; }
        public ObservableCollection<good> order { get; set; }
        public ObservableCollection<order> orders_base { get; set; }
        public ObservableCollection<user_view> users_base { get; set; }
        public ObservableCollection<order_item> order_items { get; set; }
        public ObservableCollection<orders_stats> statistics { get; set; }
        public ObservableCollection<goods_category> goods_categories { get; set; } // Категории товаров
        public List<user_categories> categories { get; set; }

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
        public result res                                   { get; set; }
        public user cur_user                                { get; set; }
        public RelayCommand<int> AddCommand                 { get; set; }
        public RelayCommand<int> DeleteCommand              { get; set; }
        public RelayCommand CreateOrderCommand              { get; set; }
        public RelayCommand<string> GetOrderItemsCommand    { get; set; }
        public RelayCommand<int> PreChangeUserCommand       { get; set; }
        public RelayCommand ChangeUserCommand               { get; set; }
        public RelayCommand CancelCommand                   { get; set; }
        public RelayCommand<int> EditGoodCommand            { get; set; } // Команда для изменения товара
        public RelayCommand<int> DeleteGoodCommand          { get; set; } // Команда удаления товара из базы данных
        public RelayCommand<int> ReplenishGoodsCommand      { get; set; } // Команда для пополнения запасов товара
        public RelayCommand OpenGoodCreationWindowCommand   { get; set; } // Команда открытия окна создания товара

        // Добавление товара к заказу
        private object AddingGood(int param)
        {
            good_view selected_item = goods_base.First(x => x.id_good == param);
            good order_item = order.Where(x => x.id_good == selected_item.id_good).FirstOrDefault();
            if (selected_item.in_storage > 0)
            {
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
            }
            res.prop = 0;
            foreach(var item in order)
            {
                res.prop += item.price*item.in_storage;
            }
            return null;
        }

        // Удаление товара из заказа
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

        // Сделать заказ
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

        // Загрузить информацию о товарах из базы
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

            if (statistics == null) statistics = new ObservableCollection<orders_stats>();
            else statistics.Clear();
            query = "SELECT goods_categories.name_gc || ': ' || SUM(order_items.amount) AS name_gc, SUM(order_items.amount) AS amount FROM goods_categories, order_items, goods WHERE goods.cat_g = goods_categories.id_gc AND goods.id_good = order_items.id_good GROUP BY goods_categories.id_gc";
            List<orders_stats> temp_stats = db.Database.SqlQuery<orders_stats>(query).ToList<orders_stats>();
            foreach (var item in temp_stats)
            {
                statistics.Add(item);
            }

            if (goods_categories == null) goods_categories = new ObservableCollection<goods_category>();
            else goods_categories.Clear();
            query = "SELECT * FROM goods_categories";
            List<goods_category> temp_categories = db.Database.SqlQuery<goods_category>(query).ToList<goods_category>();
            foreach (var item in temp_categories)
            {
                goods_categories.Add(item);
            }
        }

        // Загрузить информацию о пользователях из базы
        private void LoadUsersBase()
        {
            users_base.Clear();
            //string query = "SELECT users.*, user_categories.name_uc FROM users, user_categories WHERE users.user_cat=user_categories.id_uc";
            string query = "SELECT users.*, user_categories.name_uc FROM users LEFT JOIN user_categories ON users.user_cat = user_categories.id_uc";
            List<user_view> temp_users = db.Database.SqlQuery<user_view>(query).ToList<user_view>();
            foreach (var item in temp_users)
            {
                users_base.Add(item);
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

        private void PreChangeUser(int param=-1)
        {
            if(param!=-1)
                cur_user = db.users.Where(x => x.id_user == param).First<user>();
            var msg = new GoToPageMessage() { PageName = "UserEdit", Parameter = param.ToString() };
            Messenger.Default.Send<GoToPageMessage>(msg);
        }

        private void ChangeUser()
        {
            string query = "";
            if (cur_user.id_user == 0)
                query = "INSERT  INTO users(`login`,`password`,`user_cat`) VALUES ('"+cur_user.login+"','"+cur_user.password+"','"+cur_user.user_cat+"')";
            else query = "UPDATE users SET `login`='" + cur_user.login + "', `password`=" + cur_user.password + ", `user_cat`=" + cur_user.user_cat + " WHERE `id_user`=" + cur_user.id_user;
            db.Database.ExecuteSqlCommand(query);
            db.SaveChanges();
            var msg = new GoToPageMessage() { PageName = "UserSave" };
            Messenger.Default.Send<GoToPageMessage>(msg);
            cur_user = new user();
            PreChangeUser();
            LoadUsersBase();
        }

        private void CancelEdit()
        {
            cur_user = new user();
            PreChangeUser();
        }

        // Открыть окно изменения товара
        private void OpenEditWindow(int good_id)
        {
            EditGoodWindow editGoodWindow = new EditGoodWindow(goods_base.Where(g => g.id_good == good_id).First(), this);
            editGoodWindow.Show();
        }

        // Открыть окно создания товара
        private void OpenGoodCreationWindow()
        {
            CreateGoodWindow createGoodWindow = new CreateGoodWindow(this);
            createGoodWindow.Show();
        }

        // Изменить товар
        public void EditGood(int good_id, string good_name, int price, int category_id, int in_storage)
        {
            string query = $"UPDATE goods SET name_g=\"{good_name}\", price={price}, cat_g={category_id}, in_storage={in_storage} WHERE id_good={good_id}";
            db.Database.ExecuteSqlCommand(query);
            LoadGoodsBase();
        }

        // Добавить новый товар в базу
        public void AddNewGood(string good_name, int price, int category_id, int in_storage)
        {
            good new_good = new good();
            new_good.name_g = good_name;
            new_good.price = price;
            new_good.cat_g = category_id;
            new_good.in_storage = in_storage;
            db.goods.Add(new_good);
            db.SaveChanges();

            LoadGoodsBase();
        }

        // Удалить товар со склада
        public void DeleteGood(int good_id)
        {
            string query = $"UPDATE goods SET in_storage = 0 WHERE id_good={good_id}";
            db.Database.ExecuteSqlCommand(query);
            LoadGoodsBase();
        }

        // Пополнить запас товара на складе
        public void ReplenishGoods(int good_id)
        {
            ReplenishGoodsWindow replenishDialog = new ReplenishGoodsWindow();
            if (replenishDialog.ShowDialog() == true)
            {
                int amount;
                if (!Int32.TryParse(replenishDialog.AmountBox.Text.Trim(), out amount)) { amount = 0; }

                string query = $"UPDATE goods SET in_storage = in_storage + {amount} WHERE id_good={good_id}";
                db.Database.ExecuteSqlCommand(query);
                LoadGoodsBase();
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
            PreChangeUserCommand = new RelayCommand<int>((param) => PreChangeUser(param));
            ChangeUserCommand = new RelayCommand(() => ChangeUser());
            CancelCommand = new RelayCommand(() => CancelEdit());
            order_items = new ObservableCollection<order_item>();
            order = new ObservableCollection<good>();
            users_base = new ObservableCollection<user_view>();
            cur_user = new user();

            EditGoodCommand = new RelayCommand<int>((param) => OpenEditWindow(param));
            DeleteGoodCommand = new RelayCommand<int>((param) => DeleteGood(param));
            OpenGoodCreationWindowCommand = new RelayCommand(() => OpenGoodCreationWindow());
            ReplenishGoodsCommand = new RelayCommand<int>((good_id) => ReplenishGoods(good_id));

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
                    LoadGoodsBase();
                    LoadUsersBase();
                    db.user_cats.Load();
                    categories = db.user_cats.ToList();
                });
        }
    }
}