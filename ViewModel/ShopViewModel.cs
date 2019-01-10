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
        public List<good_view> goods_base { get; set; }
        //public List<goods_category> cats_base { get; set; }
        public ObservableCollection<good> order { get; set; }
        public string InfoTitlePageShop
        {
            get
            {
                return "Shop Window";
            }
        }
        public RelayCommand<string> AddCommand {
            get;
            set;
        }

        private object AddingGood(string param)
        {
            var msg = new GoToPageMessage() { PageName = "AddGood", Parameter = param };
            Messenger.Default.Send<GoToPageMessage>(msg);
            return null;
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ShopViewModel(IDataService dataService)
        {
            AddCommand = new RelayCommand<string>((param) => AddingGood(param));
            _dataService = dataService;
            _dataService.GetData(
                (user item, Exception error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    DbClient db = new DbClient();
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